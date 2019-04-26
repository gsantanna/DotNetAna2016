Public Class AtribuiPolo
    Inherits System.Web.UI.Page

    Dim Parametros As String()
    Dim mvPaginaAtrasada As Boolean

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
        Dim message As String = Request.QueryString("pessoa")
        If message = Nothing Then
            Response.Redirect(ResolveUrl("~/BemVindo.aspx" & MontaQueryStringFromParametros("AtribuiPolo", "BemVindo", Parametros)))
            Exit Sub
        End If

        ' Note que o parâmetro montado na página POLOS deve ter incluído a vírgula
        Dim x As String() = Split(message, ",")
        CampoCPF.Text = x(0)
        If UBound(x) < 2 Then
            MensagemERRO.Text = TextoVermelho("Houve um erro de sistema na passagem da parâmetros. ")
        Else
            CampoPessoa.Text = x(1)
            CampoSG_UFbase.Text = x(2)
        End If

        ' Inclusão ou update, é preciso carregar o combo na primeira vez
        If Not IsPostBack Then
            Call CarregaPolosDeUmMunicipoDeAlguem(cmbPolo, CampoSG_UFbase.Text, "Escolha...")
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
        If Left(cmbFuncao.Text, 7) = "Escolha" Then
            erros &= "É preciso indicar alguma função. "
        End If

        If Left(cmbPolo.Text, 7) = "Escolha" Then
            erros &= "É preciso indicar algum polo. "
        End If

        Return erros
    End Function


    Private Sub cmdEntrar_Click(sender As Object, e As EventArgs) Handles cmdEntrar.Click
        If mvPaginaAtrasada Then Exit Sub
        Dim tmp As String = ConsisteCampos()
        If tmp <> "" Then
            MensagemERRO.Text = TextoVermelho(tmp)
        Else
            Dim Polo As String
            If Left(cmbPolo.Text, 7) = "Escolha" Then
                Polo = "*"
            Else
                Polo = cmbPolo.Text
            End If
            ' Registra os dados, incluindo o CPF do autor
            tmp = Associa_Polo(CampoCPF.Text, Polo, cmbFuncao.Text, Parametros(gcParametroCPF), CampoSG_UFbase.Text)
            If tmp <> "OK" Then
                MensagemERRO.Text = TextoVermelho(tmp)
            Else
                Call VoltaParaQuemChamou()
            End If
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


    Protected Sub CarregaPolosDeUmMunicipoDeAlguem(ByRef QueCombo As DropDownList, ByRef QueUF As String, ByRef QueValorInicial As String)
        Dim sSQL As String =
            "SELECT NO_POLO X FROM POLO p" &
            " WHERE p.SG_UF='" & QueUF & "'" &
            " and EXISTS(select * from ATRIBUICAO_GESTOR a where a.CPF='" & Parametros(gcParametroCPF) & "' and (a.ID_POLO='*' or a.ID_POLO=p.ID_POLO))" &
            " ORDER BY p.NO_POLO"

        ' A procedure carrega um atributo genérico X no DropDown
        Call PreencheDropDownList(QueCombo, sSQL, QueValorInicial)
    End Sub

End Class