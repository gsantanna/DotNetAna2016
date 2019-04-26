Public Class Notifica
    Inherits System.Web.UI.Page

    Dim Parametros As String()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub cmdVoltar_Click(sender As Object, e As EventArgs) Handles cmdVoltar.Click
        Response.Redirect(ResolveUrl("~/Login.aspx"))
    End Sub
End Class