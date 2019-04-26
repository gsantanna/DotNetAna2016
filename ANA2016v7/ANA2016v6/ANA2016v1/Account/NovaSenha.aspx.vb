Public Class NovaSenha
    Inherits System.Web.UI.Page

    Dim Parametros As String()
    Dim mvPaginaAtrasada As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Se houver um erros na submissão da página (query string inválida ou  página atrasada), impede eventos posteriores
        mvPaginaAtrasada = False
        ' Verifica se a query string está OK
        Dim tmp As String = ConsisteQueryString("NovaSenha", Request, CompletaTamanhoMascara("SSSS"), Parametros, IsPostBack, True)
        If tmp <> "OK" Then
            MensagemERRO.Text = TextoVermelho(tmp)
            mvPaginaAtrasada = True
            Exit Sub
        End If

    End Sub

    Protected Sub cmdEntrar_Click(sender As Object, e As EventArgs) Handles cmdOK.Click
        If mvPaginaAtrasada Then Exit Sub
        Dim erros As String = ""

        If CampoNovaSenha.Text = "" Then
            erros &= "É preciso fornecer uma nova senha. "
            MensagemERRO.Text = TextoVermelho(erros)
            Exit Sub
        End If

        If Not VerificaSenha(CampoNovaSenha.Text) Then
            erros &= "É preciso fornecer uma nova senha válida. "
            MensagemERRO.Text = TextoVermelho(erros)
            Exit Sub
        End If

        If CampoConfirmacao.Text = "" Then
            erros &= "É preciso confirmar a senha. "
        ElseIf CampoConfirmacao.Text <> CampoNovaSenha.Text Then
            erros &= "A senha de confirmação não confere. "
        End If

        If CampoSenha.Text = "" Then
            erros &= "É preciso fornecer a senha atual. "
        End If

        If erros <> "" Then
            MensagemERRO.Text = TextoVermelho(erros)
            Exit Sub
        End If

        Dim tmp As String
        tmp = ConfereUmaSenha(Parametros(gcParametroCPF), CampoSenha.Text)

        If tmp = Nothing Then
            MensagemERRO.Text = TextoVermelho("Houve um erro de sistema nessa operação. ")
            Exit Sub
        End If

        If tmp = "0" Then
            MensagemERRO.Text = TextoVermelho("A senha atual está incorreta. ")
            Exit Sub
        End If

        ' OK, tudo. Grava a nova senha.
        Dim MeuComando = New System.Data.SqlClient.SqlCommand()
        ' Abre uma conexão
        Dim MyC = New System.Data.SqlClient.SqlConnection(Ligacao())
        MyC.Open()
        Try
            MeuComando.Connection = MyC
            MeuComando.CommandText = "UPDATE PESSOAL set senha=@SENHA, senhaprovisoria='' where CPF=@CPF"
            MeuComando.Parameters.Add("@CPF", SqlDbType.VarChar).Value = Parametros(gcParametroCPF)
            MeuComando.Parameters.Add("@SENHA", SqlDbType.VarChar).Value = Hash512(CampoNovaSenha.Text, Parametros(gcParametroCPF))
            tmp = MeuComando.ExecuteNonQuery()
            MeuComando.Dispose()
        Catch ex As Exception
            MensagemERRO.Text = TextoVermelho("Houve um erro na gravação da nova senha." & vbCrLf & ex.Message)
            Exit Sub
        Finally

            MyC.Dispose()
        End Try

        ' Vai em paz
        'If Parametros(gcParametroUFbase) = "BR" Then
        '    Response.Redirect(ResolveUrl("~/Account/EscolhaUFbase.aspx" & MontaQueryStringFromParametros("NovaSenha", "EscolhaUFbase", Parametros)))
        'Else
        Response.Redirect(ResolveUrl("~/BemVindo.aspx" & MontaQueryStringFromParametros("NovaSenha", "BemVindo", Parametros)))
        'End If
    End Sub
End Class