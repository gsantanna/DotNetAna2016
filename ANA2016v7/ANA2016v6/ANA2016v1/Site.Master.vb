Imports Microsoft.AspNet.Identity

Public Class SiteMaster
    Inherits MasterPage
    Private Const AntiXsrfTokenKey As String = "__AntiXsrfToken"
    Private Const AntiXsrfUserNameKey As String = "__AntiXsrfUserName"
    Private _antiXsrfTokenValue As String

    Dim Parametros As String()
    Dim PaginaCancelada As Boolean = False

    Protected Sub Page_Init(sender As Object, e As EventArgs)
        ' The code below helps to protect against XSRF attacks
        Dim requestCookie = Request.Cookies(AntiXsrfTokenKey)
        Dim requestCookieGuidValue As Guid
        If requestCookie IsNot Nothing AndAlso Guid.TryParse(requestCookie.Value, requestCookieGuidValue) Then
            ' Use the Anti-XSRF token from the cookie
            _antiXsrfTokenValue = requestCookie.Value
            Page.ViewStateUserKey = _antiXsrfTokenValue
        Else
            ' Generate a new Anti-XSRF token and save to the cookie
            _antiXsrfTokenValue = Guid.NewGuid().ToString("N")
            Page.ViewStateUserKey = _antiXsrfTokenValue

            Dim responseCookie = New HttpCookie(AntiXsrfTokenKey) With { _
                 .HttpOnly = True, _
                 .Value = _antiXsrfTokenValue _
            }
            If FormsAuthentication.RequireSSL AndAlso Request.IsSecureConnection Then
                responseCookie.Secure = True
            End If
            Response.Cookies.[Set](responseCookie)
        End If

        AddHandler Page.PreLoad, AddressOf master_Page_PreLoad
    End Sub

    Protected Sub master_Page_PreLoad(sender As Object, e As EventArgs)

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Verifica se a query string está OK
        Dim tmp As String = ConsisteQueryString("*", Request, CompletaTamanhoMascara("SSS"), Parametros, IsPostBack, False)
        cmdSair.Visible = tmp = "OK"
    End Sub


    Protected Sub click_cmdMeuCadastro(sender As Object, e As System.EventArgs)
        ' Verifica se a query string está OK
        Dim tmp As String = ConsisteQueryString("*", Request, CompletaTamanhoMascara("SSS"), Parametros, IsPostBack, False)
        If tmp = "OK" Then
            ' Direciona para a tela de cadastro
            Response.Redirect(ResolveUrl("~/CadastroPessoal.aspx" & MontaQueryStringFromParametros("*", "CadastroPessoal", Parametros)))
        End If
    End Sub


    Protected Sub click_cmdBemVindo(sender As Object, e As System.EventArgs)
        ' Verifica se a query string está OK
        Dim tmp As String = ConsisteQueryString("*", Request, CompletaTamanhoMascara("SSS"), Parametros, IsPostBack, False)
        If tmp = "OK" Then
            ' Direciona para a tela de cadastro
            Response.Redirect(ResolveUrl("~/BemVindo.aspx" & MontaQueryStringFromParametros("*", "BemVindo", Parametros)))
        End If
    End Sub
    Protected Sub click_cmdSair(sender As Object, e As System.EventArgs)
        ' Verifica se a query string está OK
        Dim tmp As String = ConsisteQueryString("*", Request, CompletaTamanhoMascara("SSS"), Parametros, IsPostBack, False)
        If tmp = "OK" Then
            ' Zera o RELOGIO para este CPF
            Call RelogioZERA(Parametros(gcParametroCPF))
            ' Direciona para a tela de cadastro
            Response.Redirect(ResolveUrl("~/Login.aspx"))
        End If
    End Sub
End Class