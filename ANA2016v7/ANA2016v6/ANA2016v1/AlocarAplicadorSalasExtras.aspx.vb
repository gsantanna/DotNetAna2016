Imports System.Linq
Public Class AlocarAplicadorSalasExtras
    Inherits System.Web.UI.Page

    Dim Parametros As String()
    Dim mvPaginaAtrasada As Boolean

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
        Dim message As String = Request.QueryString("turma")
        If message = Nothing Then
            Response.Redirect(ResolveUrl("~/BemVindo.aspx" & MontaQueryStringFromParametros("AlocarAplicadorPrincipal", "BemVindo", Parametros)))
            Exit Sub
        End If


        '' Inclusão ou update, é preciso carregar o combo na primeira vez

        '    'carrega os campos
        Call CarregaListaTurmasEspeciais()



    End Sub

    Private Sub CarregaListaTurmasEspeciais()

        Dim escola As String = Request.QueryString("ESCOLA")
        Dim turma As String = Split(Request.QueryString("TURMA"), ",")(0)
        Dim prova As String = Split(Request.QueryString("TURMA"), ",")(1)


        Using SqlC As New SqlClient.SqlConnection(Ligacao())

            'Carrega todos os colaboradores disponíveis para carregar os combos (colocado aqui para ser carregado somente uma vez)
            Dim strSqlPessoas As String = "select  p.CPF, P.NO_PESSOA ,  " &
                " ltrim(rtrim(max( case when  ap.NO_FUNCAO = 'IntÃ©rprete' then 'IntÃ©rprete ' else '' end )  + max( case when  ap.NO_FUNCAO = 'Guia-IntÃ©rprete' then 'Guia-IntÃ©rprete ' else '' end )  + max( case when  ap.NO_FUNCAO = 'IntÃ©rprete de Libras' then 'IntÃ©rprete de Libras ' else '' end )  +max( case when  ap.NO_FUNCAO = 'Ledor/Transcritor' then 'Ledor/Transcritor ' else '' end )  +  " &
                " max( case when  ap.NO_FUNCAO = 'Ledor' then 'Ledor ' else '' end )  + ' ' + max( case when  ap.NO_FUNCAO = 'Leitura labial' then 'Leitura labial ' else '' end )   +  max( case when  ap.NO_FUNCAO = 'Aplicador' then '' else '' end)  )) DSC_FUNCAO" &
                " from ATRIBUICAO_POLO ap " &
                " Inner join PESSOAL p on p.CPF=ap.CPF" &
                " Inner join ESCOLA e on E.id_polo=ap.id_polo " &
                " where e.ID_ESCOLA=@ID_ESCOLA" &
                " group by p.CPF , P.NO_PESSOA" &
                " ORDER BY 2"


            Dim dtPessoas As DataTable = New DataTable()

            Using Sda As New SqlClient.SqlDataAdapter(strSqlPessoas, SqlC)
                Sda.SelectCommand.Parameters.AddWithValue("@ID_ESCOLA", escola)
                Sda.Fill(dtPessoas)
            End Using




            'Carrega todas as turmas especiais para o dia/prova 
            Dim strSqlTurmas As String = "select SEQ_TURMA_SALA, ts.DS_GRUPO_APLIC,  a.NO_ALUNO , t.NO_TURMA, t.DS_SERIE , ts.ID_TURMA_SALA, CPF_APLICADOR , FLG_NAO_PREVISTA from TURMA t " &
            " INNER JOIN ALUNO a on a.CO_TURMA_CENSO=t.CO_TURMA_CENSO" &
            " INNER JOIN ALUNO_DIA ad on ad.ID_ALUNO=a.ID" &
            " INNER JOIN TURMA_SALA ts on ts.ID_TURMA_SALA=ad.ID_TURMA_SALA" &
            " where   SEQ_TURMA_SALA > 1 and  ID_PROVA=@ID_PROVA " &
            " and  t.CO_TURMA_CENSO=@CO_TURMA_CENSO " &
            " UNION ALL " &
            " select SEQ_TURMA_SALA, 'Não previstos' ,  cast(NU_ALUNOS_ALOCADOS as varchar) + ' Alunos' NO_ALUNO , t.NO_TURMA, t.DS_SERIE , ts.ID_TURMA_SALA, CPF_APLICADOR, FLG_NAO_PREVISTA   " &
            " from TURMA t inner join TURMA_SALA ts On t.CO_TURMA_CENSO=Ts.CO_TURMA_CENSO " &
            " where FLG_NAO_PREVISTA ='S' AND ID_PROVA=@ID_PROVA AND t.CO_TURMA_CENSO = @CO_TURMA_CENSO " &
            " Order by 1 "




            Using Sda As New SqlClient.SqlDataAdapter(strSqlTurmas, SqlC)
                Sda.SelectCommand.Parameters.AddWithValue("@CO_TURMA_CENSO", turma)
                Sda.SelectCommand.Parameters.AddWithValue("@ID_PROVA", prova)
                Dim dtTurmas As DataTable = New DataTable()
                Sda.Fill(dtTurmas)


                Dim ta As Integer = -1 'declara a turma atual e o esqueleto do controle. (para utilizar como cursor na montagem da tela, identificando que a turma mudou, adicionando um novo controle na tela para a nova turma)
                Dim objControle As ctlSalaEspecial = Nothing


                For Each item As DataRow In dtTurmas.Rows 'percorre os registros (alunos) 

                    CampoTurma.Text = item("NO_TURMA")

                    If item("SEQ_TURMA_SALA") <> ta Then 'Criar um novo controle 


                        objControle = LoadControl("~/ctlSalaEspecial.ascx")
                        objControle.Visible = True
                        objControle.DT_PESSOAS = dtPessoas
                        objControle.ID_TURMA_SALA = item("ID_TURMA_SALA")
                        objControle.ALUNOS = New List(Of String)
                        objControle.SALA = "SALA: " & item("SEQ_TURMA_SALA") & "  (" & item("DS_GRUPO_APLIC") & ")"
                        objControle.NAO_PREVISTA = item("FLG_NAO_PREVISTA") = "S"



                        pnlMain.Controls.Add(objControle)
                        Call objControle.Carrega()

                        If Not IsPostBack And Not IsDBNull(item("CPF_APLICADOR")) Then
                            objControle.CPF_APLICADOR = item("CPF_APLICADOR")
                        End If



                    Else



                    End If


                    objControle.ALUNOS.Add(item("NO_ALUNO"))
                    objControle.AtualizaAlunos()
                    ta = item("SEQ_TURMA_SALA")

                Next



            End Using




        End Using
    End Sub




    Private Sub cmdEntrar_Click(sender As Object, e As EventArgs) Handles cmdEntrar.Click


        'cria uma lista de controles para facilitar a manipulação
        Dim controles As List(Of ctlSalaEspecial) = New List(Of ctlSalaEspecial)

        'Valida se todos os aplicadores foram preenchidos 
        For Each controle As Control In pnlMain.Controls
            If (controle.GetType().Name = "ctlsalaespecial_ascx") Then 'Filtra somente os controles do tipo salaespecial
                Dim objSala As ctlSalaEspecial = controle
                If (IsNothing(objSala.CPF_APLICADOR) Or Not objSala.CPF_APLICADOR > "0") Then
                    MensagemERRO.Text = TextoVermelho("Favor selecionar o aplicador em todos os itens")
                    Exit Sub
                End If
                controles.Add(controle)
            End If

        Next

        'Percorre os controles salvan do o aplicador

        Dim strUpd As String = "UPDATE TURMA_SALA Set CPF_APLICADOR=@CPF_APLICADOR WHERE ID_TURMA_SALA=@ID_TURMA_SALA "
        Using SqlC As New SqlClient.SqlConnection(Ligacao())
            Call SqlC.Open()

            For Each controle As ctlSalaEspecial In controles

                Using cmd As New SqlClient.SqlCommand(strUpd, SqlC)
                    cmd.Parameters.AddWithValue("@CPF_APLICADOR", controle.CPF_APLICADOR)
                    cmd.Parameters.AddWithValue("@ID_TURMA_SALA", controle.ID_TURMA_SALA)
                    cmd.ExecuteNonQuery()
                End Using

            Next


            Call VoltaParaQuemChamou()


        End Using






    End Sub


    Protected Sub VoltaParaQuemChamou()

        Dim message As String = Request.QueryString("escola")
        Response.Redirect(ResolveUrl("~/AGENDAMENTO.aspx" & MontaQueryStringFromParametros("AlocarAplicadorPrincipal", "AGENDAMENTO", Parametros) & "&escola=" & message & ", U"))

    End Sub

    Private Sub cmdCancelar_Click(sender As Object, e As EventArgs) Handles cmdCancelar.Click
        If mvPaginaAtrasada Then Exit Sub
        Call VoltaParaQuemChamou()
    End Sub

    Protected Sub cmdExcluir_Click(sender As Object, e As EventArgs) Handles cmdExcluir.Click

        Dim turma As String = Split(Request.QueryString("TURMA"), ",")(0)
        Dim prova As String = Split(Request.QueryString("TURMA"), ",")(1)

        Dim strUpd As String = "UPDATE TURMA_SALA Set CPF_APLICADOR=NULL WHERE CO_TURMA_CENSO=@CO_TURMA_CENSO and ID_PROVA=@ID_PROVA and SEQ_TURMA_SALA>1"
        Using SqlC As New SqlClient.SqlConnection(Ligacao())
            Call SqlC.Open()

            Using cmd As New SqlClient.SqlCommand(strUpd, SqlC)
                cmd.Parameters.AddWithValue("@CO_TURMA_CENSO", turma)
                cmd.Parameters.AddWithValue("@ID_PROVA", prova)
                cmd.ExecuteNonQuery()
            End Using


            Call VoltaParaQuemChamou()


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