Public Class BemVindo
    Inherits System.Web.UI.Page

    Dim Parametros As String()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' Verifica se a query string está OK e se as credenciais são atendidas
        Dim tmp As String = ConsisteQueryString("CadastroPOLO", Request, CompletaTamanhoMascara("SSSS"), Parametros, IsPostBack, False)  ' CPF e estado base
        If Left(tmp, 2) <> "OK" Then
            Response.Redirect(ResolveUrl("~/Notifica.aspx"))
            Exit Sub
        End If

        ' Mostra apenas o que deve ser mostrado
        If Parametros(gcParametroFuncao) = "Administrador" Or Parametros(gcParametroFuncao) = "Observador" Then
            cmdOutroEstado.Visible = True
        End If
        If Parametros(gcParametroFuncao) = "Aplicador" Then
            Panel1.Visible = False
            Panel2.Visible = True
        Else
            Panel1.Visible = True
            Panel2.Visible = False
        End If

        ' Bloqueia a criação de polo se o estado estiver fechado
        If Parametros(gcParametroFuncao) <> "Administrador" Then
            If IsEstadoFechado(Parametros(gcParametroUFbase)) Then
                LinkNovoPolo.Enabled = False
            End If
        End If

        If Not IsPostBack Then
            Bandeirinha.ImageUrl = "../FIGURAS/" & "icone-" & Parametros(gcParametroUFbase) & ".png"
            NomeSaudacao.Text = Parametros(gcParametroNome) & " :: " & Parametros(gcParametroFuncao)
        End If

        If PoderDeUmaFuncao(Parametros(gcParametroFuncao)) < 5 Then
            LinkNovoPolo.Enabled = False
        End If
    End Sub

    Protected Sub LinkVerPolos_Click(sender As Object, e As EventArgs) Handles LinkVerPolos.Click
        Dim message = Request.QueryString("q")
        Dim Parametros As String() = DeCodificaQS(message)

        ' Direciona para a tela de visualização
        Response.Redirect(ResolveUrl("~/Polos.aspx" & MontaQueryStringFromParametros("BemVindo", "Polos", Parametros)) & "&tipo=polo")
    End Sub

    Protected Sub LinkNovoPolo_Click(sender As Object, e As EventArgs) Handles LinkNovoPolo.Click

        ' Direciona para a tela de cadastro
        Response.Redirect(ResolveUrl("~/CadastroPOLO.aspx" & MontaQueryStringFromParametros("BemVindo", "CadastroPOLO", Parametros)))

    End Sub

    Protected Sub LinkPendenciasPolo_Click(sender As Object, e As EventArgs) Handles LinkPendenciasPolo.Click
        Dim message = Request.QueryString("q")
        Dim Parametros As String() = DeCodificaQS(message)

        ' Direciona para a tela de visualização
        Response.Redirect(ResolveUrl("~/Polos.aspx" & MontaQueryStringFromParametros("BemVindo", "Polos", Parametros)) & "&tipo=polopendencias")
    End Sub

    Protected Sub LinkButton2_Click(sender As Object, e As EventArgs) Handles LinkButton2.Click
        Dim message = Request.QueryString("q")
        Dim Parametros As String() = DeCodificaQS(message)

        ' Direciona para a tela de visualização
        Response.Redirect(ResolveUrl("~/Polos.aspx" & MontaQueryStringFromParametros("BemVindo", "Polos", Parametros)) & "&tipo=escola")
    End Sub

    Protected Sub LinkTriagem_Click(sender As Object, e As EventArgs) Handles LinkTriagem.Click
        Dim message = Request.QueryString("q")
        Dim Parametros As String() = DeCodificaQS(message)

        ' Direciona para a tela de visualização
        Response.Redirect(ResolveUrl("~/Polos.aspx" & MontaQueryStringFromParametros("BemVindo", "Polos", Parametros)) & "&tipo=colaboradortriagem")
    End Sub

    Protected Sub LinkFuncoes_Click(sender As Object, e As EventArgs) Handles LinkFuncoes.Click
        Dim message = Request.QueryString("q")
        Dim Parametros As String() = DeCodificaQS(message)

        ' Direciona para a tela de visualização
        Response.Redirect(ResolveUrl("~/Polos.aspx" & MontaQueryStringFromParametros("BemVindo", "Polos", Parametros)) & "&tipo=colaboradorfuncao")
    End Sub

    Protected Sub cmdOutroEstado_Click(sender As Object, e As ImageClickEventArgs) Handles cmdOutroEstado.Click
        ' Direciona para a tela de visualização
        Response.Redirect(ResolveUrl("~/Account/EscolhaUFbase.aspx" & MontaQueryStringFromParametros("BemVindo", "EscolhaUFbase", Parametros)))
    End Sub

    Private Sub LinkMinhasAplicacoes_Click(sender As Object, e As EventArgs) Handles LinkMinhasAplicacoes.Click
        MessageError.Text = "Os procedimentos de agendamento ainda não foram iniciados."
    End Sub

    Protected Sub LinkMapaGeralAplicacoes_Click(sender As Object, e As EventArgs) Handles LinkMapaGeralAplicacoes.Click
        MessageError.Text = TextoVermelho("Os procedimentos de agendamento ainda não foram iniciados.")
        'MessageError.Text = TextoVermelho("Os procedimentos de agendamento ainda não foram iniciados.")
        '' Direciona para a tela de visualização
        'Response.Redirect(ResolveUrl("~/Polos.aspx" & MontaQueryStringFromParametros("BemVindo", "Polos", Parametros)) & "&tipo=aplicacoes")
    End Sub

    Protected Sub LinkPolosColaboradores_Click(sender As Object, e As EventArgs) Handles LinkPolosColaboradores.Click
        ' Direciona para a tela de visualização
        Response.Redirect(ResolveUrl("~/Polos.aspx" & MontaQueryStringFromParametros("BemVindo", "Polos", Parametros)) & "&tipo=polocolaborador")
    End Sub

    Private Sub LinkAgendamentoEscolas_Click(sender As Object, e As EventArgs) Handles LinkAgendamentoEscolas.Click
        'MessageError.Text = TextoVermelho("Os procedimentos de agendamento ainda não foram iniciados.")
        Dim message = Request.QueryString("q")
        Dim Parametros As String() = DeCodificaQS(message)

        ' Direciona para a tela de visualização
        Response.Redirect(ResolveUrl("~/Polos.aspx" & MontaQueryStringFromParametros("BemVindo", "Polos", Parametros)) & "&tipo=escolaagendamento")
    End Sub

    Private Sub LinkRegistro_Click(sender As Object, e As EventArgs) Handles LinkRegistro.Click
        MessageError.Text = TextoVermelho("Os procedimentos de registro de aplicação ainda não foram iniciados.")
        'Dim message = Request.QueryString("q")
        'Dim Parametros As String() = DeCodificaQS(message)

        '' Direciona para a tela de visualização
        'Response.Redirect(ResolveUrl("~/Polos.aspx" & MontaQueryStringFromParametros("BemVindo", "Polos", Parametros)) & "&tipo=turmaregistro")
    End Sub

    Private Sub LinkButtonTURMAS_Click(sender As Object, e As EventArgs) Handles LinkButtonTURMAS.Click
        Dim message = Request.QueryString("q")
        Dim Parametros As String() = DeCodificaQS(message)

        ' Direciona para a tela de visualização
        Response.Redirect(ResolveUrl("~/Polos.aspx" & MontaQueryStringFromParametros("BemVindo", "Polos", Parametros)) & "&tipo=escolaturmas")
    End Sub

    Private Sub LinkESPECIALIZADOS_Click(sender As Object, e As EventArgs) Handles LinkESPECIALIZADOS.Click
        Dim message = Request.QueryString("q")
        Dim Parametros As String() = DeCodificaQS(message)

        ' Direciona para a tela de visualização
        Response.Redirect(ResolveUrl("~/Polos.aspx" & MontaQueryStringFromParametros("BemVindo", "Polos", Parametros)) & "&tipo=carregaespecializados")
    End Sub
End Class