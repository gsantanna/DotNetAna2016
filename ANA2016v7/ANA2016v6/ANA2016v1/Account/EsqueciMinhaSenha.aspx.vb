Public Class EsqueciMinhaSenha
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim X As String = Request.QueryString("q")
        QueEmail.Text = Request.QueryString("q")

    End Sub

End Class