Public Class OrcamentoPolo3
    Inherits System.Web.UI.Page

    Dim Parametros As String()
    Dim Operacao As Char
    Dim QuePolo As String
    Dim mvPaginaAtrasada As Boolean


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Se houver um erros na submissão da página (query string inválida ou  página atrasada), impede eventos posteriores
        mvPaginaAtrasada = False

        ' Verifica se a query string está OK e se as credenciais são atendidas
        Dim tmp As String = ConsisteQueryString("OrcamentoPolo", Request, CompletaTamanhoMascara("SSSS"), Parametros, IsPostBack, True)
        If tmp <> "OK" Then
            MensagemERRO.Text = TextoVermelho(tmp)
            mvPaginaAtrasada = True
            Exit Sub
        End If

        ' Descobre qual é a operação
        QuePolo = Request.QueryString("polo")
        If QuePolo Is Nothing Then
            MensagemERRO.Text = TextoVermelho("Houve um erro de sistema: falta o parâmetro de operação. ")
            cmdEntrar.Enabled = False
            'Exit Sub
        Else
            ' Pode ser edição ou visualização
            Dim x As String() = Split(QuePolo, ",")
            If UBound(x) < 1 Then
                MensagemERRO.Text = TextoVermelho("Houve um erro de sistema: falta o parâmetro de operação. ")
                'Exit Sub
            Else
                QuePolo = x(0)
                Operacao = Trim(x(1))
            End If
        End If

        ' Inclusão ou update, é preciso carregar o combo na primeira vez
        If Not IsPostBack Then
            ' Somente update e visualização: é preciso carregar os dados do polo
            Call CarregaDados(QuePolo)
        End If

    End Sub

    Protected Sub VoltaParaQuemChamou()
        ' Volta para quem chamou
        Dim QueryStringPlus As String = ""
        Dim x As String() = Split(Parametros(gcParametroOrigem), ",")       ' Descobre se há um query string adicional de retorno
        Dim paginaURL As String = "~/" & x(0) & ".aspx"
        If UBound(x) = 1 Then
            QueryStringPlus = "&" & x(1)
        End If
        Response.Redirect(ResolveUrl(paginaURL & MontaQueryStringFromParametros("CadastroPOLO", x(0), Parametros)) & QueryStringPlus)
    End Sub
    Protected Sub cmdCancelar_click(sender As Object, e As EventArgs) Handles cmdCancelar.Click
        If mvPaginaAtrasada Then Exit Sub
        Call VoltaParaQuemChamou()
    End Sub

    Private Function ConsisteCampos(ByRef QueOperacao As Char) As String
        Dim erros As String = ""
        Dim OK As Boolean = False

        If CampoNumPessoas.Text = "" Then
            erros &= "Número de pessoas não foi preenchido. "
        ElseIf IncluiNaoNumericos(CampoNumPessoas.Text) Then
            erros &= "Número de pessoas deve conter apenas números. "
        ElseIf CInt(CampoNumPessoas.Text) = 0 Then
            erros &= "Número de pessoas não deve ser zero. "
        End If

        Return erros

    End Function

    Protected Sub cmdEntrar_Click(sender As Object, e As EventArgs) Handles cmdEntrar.Click
        If mvPaginaAtrasada Then Exit Sub
        If Operacao = "V" Then Call VoltaParaQuemChamou()

        Dim tmp As String = ConsisteCampos(Operacao)
        If tmp <> "" Then
            MensagemERRO.Text = TextoVermelho(tmp)
            Exit Sub
        Else
            MensagemERRO.Text = ""
        End If

        tmp = UPDATEcadastro()

        If tmp = "OK" Then
            Call VoltaParaQuemChamou()
        Else
            MensagemERRO.Text = TextoVermelho(tmp)
        End If
    End Sub


    Private Function UPDATEcadastro() As String

        Dim MeuComando = New System.Data.SqlClient.SqlCommand()
        Dim OK As String = "OK"
        Dim tmp As String
        ' Abre uma conexão
        Dim MyC = New System.Data.SqlClient.SqlConnection(Ligacao())
        MyC.Open()
        Try
            MeuComando.Connection = MyC
            tmp =
                "UPDATE POLO Set " &
                "CAPACITACAO_NUM_PESSOAS=@CampoNumPessoas," &
                "CAPACITACAO_TX_OBS=@TX_OBS," &
                "CAPACITACAO_AltDate=GETDATE()" &
                 " WHERE POLO.ID_POLO=@ID_POLO"

            Call PreencheVariaveis(MeuComando.Parameters)
            MeuComando.CommandText = tmp
            tmp = MeuComando.ToString()
            MeuComando.ExecuteNonQuery()
        Catch ex As Exception
            OK = "Erro na regravação dos dados." & vbCrLf & ex.Message
        Finally
            MeuComando.Dispose()
            MyC.Close()
            MyC.Dispose()
        End Try
        Return OK
    End Function


    Private Sub PreencheVariaveis(ByRef QueParameters As SqlClient.SqlParameterCollection)

        With QueParameters
            .Add("@ID_POLO", SqlDbType.VarChar).Value = QuePolo
            .Add("@CampoNumPessoas", SqlDbType.Int).Value = CampoNumPessoas.Text
            .Add("@TX_OBS", SqlDbType.VarChar).Value = CampoOBS.Text
        End With

    End Sub

    Private Sub CarregaDados(ByRef QueID_POLO As String)

        Dim OK As String = "OK"
        Try
            Dim sSQL As String =
            "Select  p.* FROM POLO p WHERE p.ID_POLO='" & QueID_POLO & "'"

            ' Abre uma conexão
            Dim MyC = New System.Data.SqlClient.SqlConnection(Ligacao())
            MyC.Open()

            Dim MeuComando = New System.Data.SqlClient.SqlCommand(sSQL, MyC)
            Dim RS As System.Data.SqlClient.SqlDataReader
            RS = MeuComando.ExecuteReader

            If RS.HasRows Then
                Call InstanciaCampos(RS)
            Else
                OK = "Dados não puderam ser mostrados. "
            End If

            ' Encerra a leitura
            RS.Close()
            RS.Dispose()
            MeuComando.Dispose()
            MyC.Dispose()

        Catch ex As Exception
            OK = "Erro na leitura dos dados de polos." & vbCrLf & ex.Message
        End Try
        If OK <> "OK" Then MensagemERRO.Text = TextoVermelho(OK)
    End Sub

    Private Sub InstanciaCampos(ByRef RS As System.Data.SqlClient.SqlDataReader)

        RS.Read()

        'Preenche os campos
        CampoID_POLO.Text = RS.GetString(RS.GetOrdinal("ID_POLO"))
        CampoNO_POLO.Text = RS.GetString(RS.GetOrdinal("NO_POLO"))
        CampoSG_UF.Text = RS.GetString(RS.GetOrdinal("SG_UF"))

        If RS.IsDBNull(RS.GetOrdinal("CAPACITACAO_NUM_PESSOAS")) Then
            CampoNumPessoas.Text = ""
            CampoValorTotal.Text = Format(0, "0.00")
        Else
            CampoNumPessoas.Text = Trim(Str(RS.GetValue(RS.GetOrdinal("CAPACITACAO_NUM_PESSOAS"))))
            CampoValorTotal.Text = Format(RS.GetValue(RS.GetOrdinal("CAPACITACAO_NUM_PESSOAS")) * 30, "0.00")
        End If

        CampoOBS.Text = RS.GetString(RS.GetOrdinal("CAPACITACAO_TX_OBS"))
        If RS.IsDBNull(RS.GetOrdinal("CAPACITACAO_AltDate")) Then
            LabelIncAlt.Text = ""
        Else
            LabelIncAlt.Text = "Última atualização " & Format(RS.GetValue(RS.GetOrdinal("CAPACITACAO_AltDate")), "dd/MM/yyyy hh:mm")
        End If

        If Operacao = "V" Then
            CampoID_POLO.Enabled = False
            CampoNO_POLO.Enabled = False
            CampoNumPessoas.Enabled = False
            CampoOBS.Enabled = False
            cmdEntrar.Text = "OK"
        End If
    End Sub

End Class