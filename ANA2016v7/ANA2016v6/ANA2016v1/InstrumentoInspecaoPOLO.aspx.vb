Public Class InstrumentoInspecaoPOLO
    Inherits System.Web.UI.Page

    Dim Parametros As String()
    Dim mvPaginaAtrasada As Boolean
    Dim mvID_POLO As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        ' Se houver erros na submissão da página (query string inválida ou  página atrasada), impede eventos posteriores
        mvPaginaAtrasada = False
        ' Verifica se a query string está OK
        Dim tmp As String = ConsisteQueryString("CadastroPessoal", Request, CompletaTamanhoMascara("SSS"), Parametros, IsPostBack, True)
        If tmp <> "OK" Then
            MensagemERRO.Text = TextoVermelho(tmp)
            mvPaginaAtrasada = True
            Exit Sub
        End If

        ' Verifica se o parâmetro chegou corretamente
        Dim message As String = Request.QueryString("polo")
        If message = Nothing Then
            Response.Redirect(ResolveUrl("~/BemVindo.aspx" & MontaQueryStringFromParametros("AtribuiFuncao", "BemVindo", Parametros)))
            Exit Sub
        End If

        ' Note que o parâmetro montado na página POLOS deve ter incluído a vírgula
        Dim x As String() = Split(message, ",")
        If UBound(x) <> 1 Then
            MensagemERRO.Text = TextoVermelho("Houve um erro de sistema na passagem da parâmetros. ")
        Else
            ' x(0) contém o ID_POLO; x(1) o nome do polo
            CampoPOLO.Text = "Instrumento de Inspeção :: " + x(1) + " (" + x(0) + ")"
            mvID_POLO = x(0)
        End If

    End Sub



    Protected Function ConsisteCampos() As String
        Dim erros As String = ""

        If Trim(cmbConfirmados1.Text) = "" Then erros += "A pergunta 1.1 não foi respondida. "
        If Trim(cmbConfirmados2.Text) = "" Then erros += "A pergunta 1.2 não foi respondida. "
        If Trim(cmbConfirmados3.Text) = "" Then erros += "A pergunta 1.3 não foi respondida. "
        If Trim(cmbConfirmados4.Text) = "" Then erros += "A pergunta 1.4 não foi respondida. "
        If Trim(cmbConfirmados5.Text) = "" Then erros += "A pergunta 1.5 não foi respondida. "
        If Trim(cmbConfirmados6.Text) = "" Then erros += "A pergunta 1.6 não foi respondida. "
        If Trim(cmbConfirmados7.Text) = "" Then erros += "A pergunta 1.7 não foi respondida. "
        If Trim(cmbConfirmados8.Text) = "" Then erros += "A pergunta 1.8 não foi respondida. "

        If Trim(cmbConfirmados9.Text) = "" Then erros += "A pergunta 2.1 não foi respondida. "
        If Trim(cmbConfirmados10.Text) = "" Then erros += "A pergunta 2.2 não foi respondida. "
        If Trim(cmbConfirmados11.Text) = "" Then erros += "A pergunta 2.3 não foi respondida. "
        If Trim(cmbConfirmados12.Text) = "" Then erros += "A pergunta 2.4 não foi respondida. "
        If Trim(cmbConfirmados13.Text) = "" Then erros += "A pergunta 2.5 não foi respondida. "
        If Trim(cmbConfirmados14.Text) = "" Then erros += "A pergunta 2.6 não foi respondida. "
        If Trim(cmbConfirmados15.Text) = "" Then erros += "A pergunta 2.7 não foi respondida. "
        If Trim(cmbConfirmados16.Text) = "" Then erros += "A pergunta 2.8 não foi respondida. "

        If Not FileUpload1.HasFile Then
            erros += "É preciso escolher um arquivo para upload. "
        Else
            Select Case System.IO.Path.GetExtension(FileUpload1.FileName).ToUpper()
                Case ".BMP", ".GIF", ".JPG", ".PNG"

                Case Else
                    erros += "O arquivo deve conter uma imagem de algum dos tipos: " + ".BMP .GIF .JPG .PNG"
            End Select
        End If

        Return erros

    End Function


    Protected Sub VoltaParaQuemChamou()
        ' Volta para quem chamou
        Dim QueryStringPlus As String = ""
        Dim x As String() = Split(Parametros(gcParametroOrigem), ",")       ' Descobre se há uma query string adicional de retorno
        If x(0) = "*" Then
            ' Chamada da MasterPage
            x(0) = "BemVindo"
        End If
        Dim paginaURL As String = "~/" & x(0) & ".aspx"
        If UBound(x) = 1 Then
            QueryStringPlus = "&" & x(1)
        End If
        Response.Redirect(ResolveUrl(paginaURL & MontaQueryStringFromParametros("CadastroPOLO", x(0), Parametros)) & QueryStringPlus)
    End Sub

    Private Sub cmdCancelar_Click(sender As Object, e As EventArgs) Handles cmdCancelar.Click
        If mvPaginaAtrasada Then Exit Sub
        Call VoltaParaQuemChamou()
    End Sub

    Private Sub cmdEntrar_Click(sender As Object, e As EventArgs) Handles cmdEntrar.Click
        If mvPaginaAtrasada Then Exit Sub
        Dim tmp As String = ConsisteCampos()
        If tmp <> "" Then
            MensagemERRO.Text = TextoVermelho(tmp)
        Else
            Dim xxx As String = Server.MapPath("~/") + "..\POLOS_INSPECAO\" + mvID_POLO + System.IO.Path.GetExtension(FileUpload1.FileName)
            FileUpload1.SaveAs(xxx)

            Dim sSQL As String =
                "UPDATE POLO set " +
                "IA_1_1='" + cmbConfirmados1.Text + "'," +
                "IA_1_2='" + cmbConfirmados2.Text + "'," +
                "IA_1_3='" + cmbConfirmados3.Text + "'," +
                "IA_1_4='" + cmbConfirmados4.Text + "'," +
                "IA_1_5='" + cmbConfirmados5.Text + "'," +
                "IA_1_6='" + cmbConfirmados6.Text + "'," +
                "IA_1_7='" + cmbConfirmados7.Text + "'," +
                "IA_1_8='" + cmbConfirmados8.Text + "'," +
                "IA_2_1='" + cmbConfirmados9.Text + "'," +
                "IA_2_2='" + cmbConfirmados10.Text + "'," +
                "IA_2_3='" + cmbConfirmados11.Text + "'," +
                "IA_2_4='" + cmbConfirmados12.Text + "'," +
                "IA_2_5='" + cmbConfirmados13.Text + "'," +
                "IA_2_6='" + cmbConfirmados14.Text + "'," +
                "IA_2_7='" + cmbConfirmados15.Text + "'," +
                "IA_2_8='" + cmbConfirmados16.Text + "'" +
                " WHERE ID_POLO='" + mvID_POLO + "'"
            Call ExecutaSQL(sSQL)
            Call VoltaParaQuemChamou()
        End If

    End Sub
End Class