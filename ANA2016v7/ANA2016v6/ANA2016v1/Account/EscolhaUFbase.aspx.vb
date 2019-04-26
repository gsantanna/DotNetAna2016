Public Class EscolhaUFbase
    Inherits System.Web.UI.Page

    Dim Parametros As String()
    Dim mvFiltros As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' Verifica se a query string está OK
        Dim tmp As String = ConsisteQueryString("NovaSenha", Request, CompletaTamanhoMascara("SSSS"), Parametros, IsPostBack, False)
        If tmp <> "OK" Then
            MensagemERRO.Text = TextoVermelho(tmp)
            Exit Sub
        End If

    End Sub

    Protected Sub cmdEntrar_Click(sender As Object, e As EventArgs) Handles cmdOK.Click
        If Left(cmbEstadoBase.Text, 7) = "Escolha" Then
            MensagemERRO.Text = TextoVermelho("É preciso escolher algum estado")
        Else
            ' Reinicializa os filtros da última rodada
            mvFiltros = gcFiltrosInicializacao

            ' Verifica se mudou o estado
            Parametros(gcParametroUFbase) = UFdeUmEstado(cmbEstadoBase.Text)

            ' Atualiza o valor do filtro da string na posição zero (UF)
            mvFiltros = EmpacotaUmFiltro(0, UFdeUmEstado(cmbEstadoBase.Text))

            ' Atualiza os filtros no BD
            Dim sSQL As String = "UPDATE LOGIN set FILTROS='" & mvFiltros & "' WHERE CPF='" & Parametros(gcParametroCPF) & "'"
            Call ExecutaSQL(sSQL)


            ' Vai em paz
            Response.Redirect(ResolveUrl("~/BemVindo.aspx" & MontaQueryStringFromParametros("EscolhaUFbase", "BemVindo", Parametros)))
        End If
    End Sub

    Protected Function ValorDeUmFiltro(ByRef Posicao As Int16) As String
        Dim x As String() = Split(mvFiltros, gcSeparadorFiltros)
        Return x(Posicao)
    End Function

    Protected Function EmpacotaUmFiltro(ByRef QuePosicao As Int16, ByRef QueValor As String) As String
        'Dim x As String() = Split(Parametros(gcParametroFiltros), gcSeparadorFiltros)
        Dim x As String() = Split(mvFiltros, gcSeparadorFiltros)
        x(QuePosicao) = QueValor
        ' Reconstroi a string de filtros
        Dim tmp As String = x(0)
        For k = 1 To UBound(x)
            tmp &= gcSeparadorFiltros & x(k)
        Next
        Return tmp
    End Function
End Class