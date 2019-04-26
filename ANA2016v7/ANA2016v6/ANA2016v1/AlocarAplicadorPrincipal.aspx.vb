Public Class AlocarAplicadorPrincipal
    Inherits System.Web.UI.Page

    Dim Parametros As String()
    Dim mvPaginaAtrasada As Boolean
    Dim x As String()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        ' Se houver um erros na submissão da página (query string inválida ou  página atrasada), impede eventos posteriores
        mvPaginaAtrasada = False
        ' Verifica se a query string está OK
        Dim tmp As String = ConsisteQueryString("AlocarAplicadorPrincipal", Request, CompletaTamanhoMascara("SSS"), Parametros, IsPostBack, True)
        If tmp <> "OK" Then
            MensagemERRO.Text = TextoVermelho(tmp)
            mvPaginaAtrasada = True
            Exit Sub
        End If

        ' Verifica se o parâmetro chegou corretamente
        ' Parâmetros passados na query string turma: CO_TURMA_CENSO, ID_PROVA, ID_DIA_APLICACAO, TURNO_BLOQUEIO_APLICADOR
        Dim message As String = Request.QueryString("turma")
        If message = Nothing Then
            Response.Redirect(ResolveUrl("~/BemVindo.aspx" & MontaQueryStringFromParametros("AlocarAplicadorPrincipal", "BemVindo", Parametros)))
            Exit Sub
        End If
        x = Split(message, ",")

        '' Inclusão ou update, é preciso carregar o combo na primeira vez
        If Not IsPostBack Then
            '    'carrega os campos
            Call CarregaComboPessoas(CInt(x(2)), x(3))   ' Passa ID_DIA_APLICACAO, TURNO_BLOQUEIO

        End If
    End Sub

    Private Sub CarregaComboPessoas(ByRef QueDiaAplicacao As Int16, ByRef QueTurnoBloqueio As String)
        Using SqlC As New SqlClient.SqlConnection(Ligacao())
            'TO DO: Adicionar aqui o filtro para travar somente o pessoal do polo 
            Dim strSql As String =
            "SELECT  p.CPF, P.NO_PESSOA ,  " &
            " ltrim(rtrim(max( case when  ap.NO_FUNCAO = 'Guia-Intérprete'      then 'Guia-Intérprete ' else '' end )" &
                       "+ max( case when  ap.NO_FUNCAO = 'Intérprete de Libras' then 'Intérprete de Libras ' else '' end )" &
                       "+ max( case when  ap.NO_FUNCAO = 'Ledor/Transcritor'    then 'Ledor/Transcritor ' else '' end )" &
                       "+ max( case when  ap.NO_FUNCAO = 'Aplicador'            then '' else '' end)  )) DSC_FUNCAO" &
            " from ATRIBUICAO_POLO ap " &
            " Inner join PESSOAL p on p.CPF=ap.CPF" &
            " Inner join ESCOLA e on e.id_polo=ap.id_polo " &
            " where e.ID_ESCOLA=@ID_ESCOLA" &
            "   and NOT EXISTS (select * FROM TURNO_HORARIO tr, TURMA_SALA ts, TURMA_APLICACAO ta, TURMA t" &
                              " where ts.CO_TURMA_CENSO=t.CO_TURMA_CENSO" &
                              " and ta.CO_TURMA_CENSO=t.CO_TURMA_CENSO" &
                              " and ts.ID_PROVA=ta.ID_PROVA" &
                              " and ta.ID_DIA_APLICACAO=@ID_DIA_APLICACAO" &
                              " and ts.CPF_APLICADOR=p.CPF" &
                              " and tr.TURNO_BLOQUEIO_APLICADOR=@TURNO_BLOQUEIO_APLICADOR" &
                              " and tr.ID_TURNO_HORARIO=t.ID_HORARIO" &
                              ")" &
            " group by p.CPF , p.NO_PESSOA Order by NO_PESSOA"


            Using Sda As New SqlClient.SqlDataAdapter(strSql, SqlC)
                Sda.SelectCommand.Parameters.AddWithValue("@ID_ESCOLA", Split(Request.QueryString("ESCOLA"), ",", 0)(0))
                Sda.SelectCommand.Parameters.AddWithValue("@ID_DIA_APLICACAO", QueDiaAplicacao)
                Sda.SelectCommand.Parameters.AddWithValue("@TURNO_BLOQUEIO_APLICADOR", QueTurnoBloqueio)
                Dim dtPessoas As DataTable = New DataTable()
                Sda.Fill(dtPessoas)
                For Each r As DataRow In dtPessoas.Rows
                    Dim nome As String = r("NO_PESSOA") & " - " & EditaCPF(r("CPF")) & IIf(r("DSC_FUNCAO") <> "", "(" & r("DSC_FUNCAO") & ")", "")
                    cmbFuncionario.Items.Add(New ListItem(nome, r("CPF")))
                Next

            End Using

            ' Carrega os dados da alocação para selecionar o item certo no combo
            Dim strSqlAlocacao As String = "SELECT ta.CPF_APLICADOR, NO_TURMA , DS_SERIE  from TURMA_SALA ta " &
            " inner join TURMA t on T.CO_TURMA_CENSO=Ta.CO_TURMA_CENSO WHERE ta.CO_TURMA_CENSO=@CO_TURMA_CENSO And ID_PROVA=@ID_PROVA "


            Using Sda As New SqlClient.SqlDataAdapter(strSqlAlocacao, SqlC)

                Dim x As String() = Split(Request.QueryString("turma"), ",")
                Sda.SelectCommand.Parameters.AddWithValue("@CO_TURMA_CENSO", x(0))
                Sda.SelectCommand.Parameters.AddWithValue("@ID_PROVA", x(1))

                Dim dt As DataTable = New DataTable()
                Sda.Fill(dt)
                Dim registro = dt.Rows(0)
                CampoTurma.Text = registro("NO_TURMA") & " - " & registro("DS_SERIE")
                If (Not IsDBNull(registro("CPF_APLICADOR"))) Then
                    cmbFuncionario.SelectedValue = registro("CPF_APLICADOR")
                End If


            End Using

        End Using




    End Sub

    Protected Function ConsisteCampos() As String

        Dim erros As String = ""
        If Left(cmbFuncionario.Text, 7) = "Escolha" Then
            erros &= "É preciso selecionar um aplicador "
        ElseIf PoderDeUmaFuncao(Parametros(gcParametroFuncao)) < 3 Then
            erros &= "Desculpe, mas você não tem permissão para realizar essa operação. "
        End If



        Return erros
    End Function





    Private Sub cmdEntrar_Click(sender As Object, e As EventArgs) Handles cmdEntrar.Click
        If mvPaginaAtrasada Then Exit Sub

        Dim tmp As String = ConsisteCampos()
        If tmp <> "" Then
            MensagemERRO.Text = TextoVermelho(tmp)
        Else

            Dim x As String() = Split(Request.QueryString("turma"), ",")

            Using SqlC As New SqlClient.SqlConnection(Ligacao())
                Call SqlC.Open()

                Dim strSql As String = "UPDATE TURMA_SALA SET CPF_APLICADOR=@CPF WHERE CO_TURMA_CENSO=@CO_TURMA_CENSO And ID_PROVA=@ID_PROVA And SEQ_TURMA_SALA=1"

                Using Cmd As New SqlClient.SqlCommand(strSql, SqlC)

                    Cmd.Parameters.AddWithValue("@CPF", cmbFuncionario.SelectedValue)
                    Cmd.Parameters.AddWithValue("@CO_TURMA_CENSO", x(0))
                    Cmd.Parameters.AddWithValue("@ID_PROVA", x(1))
                    Cmd.ExecuteNonQuery()
                    Call VoltaParaQuemChamou()

                End Using

            End Using

        End If
    End Sub


    Protected Sub VoltaParaQuemChamou()

        Dim message As String = Request.QueryString("escola")
        Response.Redirect(ResolveUrl("~/AGENDAMENTO.aspx" & MontaQueryStringFromParametros("AlocarAplicadorPrincipal", "AGENDAMENTO", Parametros) & "&escola=" & message & ",U"))

    End Sub

    Private Sub cmdCancelar_Click(sender As Object, e As EventArgs) Handles cmdCancelar.Click
        If mvPaginaAtrasada Then Exit Sub
        Call VoltaParaQuemChamou()
    End Sub

    Protected Sub cmdExcluir_Click(sender As Object, e As EventArgs) Handles cmdExcluir.Click
        Dim x As String() = Split(Request.QueryString("turma"), ",")

        Using SqlC As New SqlClient.SqlConnection(Ligacao())
            Call SqlC.Open()

            Dim strSql As String = "UPDATE TURMA_SALA SET CPF_APLICADOR=NULL WHERE CO_TURMA_CENSO=@CO_TURMA_CENSO And ID_PROVA=@ID_PROVA And SEQ_TURMA_SALA=1"

            Using Cmd As New SqlClient.SqlCommand(strSql, SqlC)

                Cmd.Parameters.AddWithValue("@CO_TURMA_CENSO", x(0))
                Cmd.Parameters.AddWithValue("@ID_PROVA", x(1))
                Cmd.ExecuteNonQuery()
                Call VoltaParaQuemChamou()

            End Using

        End Using
    End Sub






    'Protected Sub CarregaPolosDeUmMunicipoDeAlguem(ByRef QueCombo As DropDownList, ByRef QueUF As String, ByRef QueValorInicial As String)
    '    Dim sSQL As String = "Select NO_POLO X FROM POLO p" &
    '                                  " WHERE p.SG_UF='" & QueUF & "'" &
    '                                  " ORDER BY p.NO_POLO"

    '    ' A procedure carrega um atributo genérico X no DropDown
    '    Call PreencheDropDownList(QueCombo, sSQL, QueValorInicial)
    'End Sub





End Class