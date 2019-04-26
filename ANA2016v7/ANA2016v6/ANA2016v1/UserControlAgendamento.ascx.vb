Public Class UserControlAgendamento
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub SetJavaScriptEditar(ByRef QueScript As String)
        Me.cmdEditar.Attributes.Add("onclick", QueScript)
    End Sub

End Class