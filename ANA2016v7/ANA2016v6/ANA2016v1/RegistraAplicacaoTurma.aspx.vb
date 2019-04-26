Public Class RegistraAplicacaoTurma
    Inherits System.Web.UI.Page

    Dim Parametros As String()
    Dim mvPaginaAtrasada As Boolean
    Dim CodigoTurma As String
    Dim Dia As String


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Se houver um erros na submissão da página (query string inválida ou  página atrasada), impede eventos posteriores
        mvPaginaAtrasada = False
        ' Verifica se a query string está OK
        Dim tmp As String = ConsisteQueryString("CadastroPessoal", Request, CompletaTamanhoMascara("SSS"), Parametros, IsPostBack, True)
        If tmp <> "OK" Then
            MensagemERRO.Text = TextoVermelho(tmp)
            mvPaginaAtrasada = True
            Exit Sub
        End If

        ' Verifica se o parâmetro chegou corretamente
        Dim message As String = Request.QueryString("turma")
        If message = Nothing Then
            Response.Redirect(ResolveUrl("~/BemVindo.aspx" & MontaQueryStringFromParametros("RegistroAplicacaoTurma", "BemVindo", Parametros)))
            Exit Sub
        End If

        ' Note que o parâmetro montado na página POLOS deve ter incluído a vírgula
        Dim x As String() = Split(message, "|")
        If UBound(x) < 2 Then
            MensagemERRO.Text = TextoVermelho("Houve um erro de sistema na passagem da parâmetros. ")
        Else
            CodigoTurma = x(0)
            Dia = x(1)
            CampoPessoa.Text = x(0) & " " & x(2) & "   Dia " & x(1)
        End If

    End Sub

    'Protected Sub CarregaComboFuncoes()

    '    cmbFuncao.Items.Clear()
    '    cmbFuncao.Items.Add("Escolha...")
    '    Select Case Parametros(gcParametroFuncao)
    '        Case "Administrador"
    '            cmbFuncao.Items.Add("Apoio Logístico")
    '            cmbFuncao.Items.Add("Coordenador de Polo")
    '            cmbFuncao.Items.Add("Subcoordenador Estadual")
    '            cmbFuncao.Items.Add("Coordenador Estadual")
    '        Case "Coordenador Estadual"
    '            cmbFuncao.Items.Add("Apoio Logístico")
    '            cmbFuncao.Items.Add("Coordenador de Polo")
    '            cmbFuncao.Items.Add("Subcoordenador Estadual")
    '        Case "Subcoordenador Estadual"
    '            cmbFuncao.Items.Add("Apoio Logístico")
    '            cmbFuncao.Items.Add("Coordenador de Polo")
    '        Case "Coordenador de Polo"
    '            cmbFuncao.Items.Add("Apoio Logístico")
    '        Case Else
    '    End Select
    'End Sub

    Protected Function ConsisteCampos() As String
        Dim erros As String = ""
        If Left(cmbSimNao.Text, 7) = "Escolha" Then
            erros &= "É preciso escolher Sim ou Não. "
        ElseIf cmbSimNao.Text = "Sim" Then
            Return ""
        End If

        If cmbMotivo.Text = "Escolha..." Then
            erros &= "É preciso escolher um motivo. "
        End If

        Return erros
    End Function


    Private Sub cmdEntrar_Click(sender As Object, e As EventArgs) Handles cmdEntrar.Click
        If mvPaginaAtrasada Then Exit Sub
        Dim tmp As String = ConsisteCampos()
        If tmp = "" Then
            Call Finaliza()
            Call VoltaParaQuemChamou()
        Else
            MensagemERRO.Text = TextoVermelho(tmp)
        End If
    End Sub


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



    Protected Sub Finaliza()
        Dim sSQL As String
        If cmbSimNao.Text = "Sim" Then
            sSQL = "UPDATE TURMA_APLICACAO set NO_STATUS='OK', TX_MOTIVO='' WHERE CO_TURMA_CENSO='" & CodigoTurma & "' and ID_PROVA='" & Dia & "'"
        Else
            sSQL = "UPDATE TURMA_APLICACAO set NO_STATUS='NÃO HOUVE APLICAÇÃO', TX_MOTIVO='" & cmbMotivo.Text & "' WHERE CO_TURMA_CENSO='" & CodigoTurma & "' and ID_PROVA='" & Dia & "'"
        End If
        Call ExecutaSQL(sSQL)
    End Sub



    Protected Sub cmbSimNao_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSimNao.SelectedIndexChanged
        Dim tmp As String = ConsisteCampos()
        If tmp = "" Then
            If cmbSimNao.Text = "Sim" Then
                Call Finaliza()
                Call VoltaParaQuemChamou()
            End If
        Else
            MensagemERRO.Text = TextoVermelho(tmp)
        End If
    End Sub
End Class