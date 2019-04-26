Imports System.Data.SqlClient


Public Class POLOS
    Inherits System.Web.UI.Page
    Const mcLimiteLinhasLidas = 100000
    Const mcLimiteLinhasMostradas = 120

    Dim mvPrimeiraPaginaMostrada As Int16 = 1
    Dim mvTurmaEscolhida As String = ""

    Dim mvCorPautas As Drawing.Color = Drawing.Color.WhiteSmoke
    Dim Parametros As String()
    Dim mvFiltros As String
    Dim mvEmRemocao As Boolean = True       ' Controla se o usuário deve confirmar a remoção ou não
    Dim mvQueTipo As String                 ' Escola, Polo, ...
    Dim OK As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sSQL As String
        Dim FiltroAlterado As Boolean = False

        ' Verifica se a query string está OK
        Dim tmp As String = ConsisteQueryString("Polos", Request, CompletaTamanhoMascara("SSSSS"), Parametros, IsPostBack, False)
        If tmp <> "OK" Then
            MensagemERROS.Text = TextoVermelho(tmp)
            Response.Redirect(ResolveUrl("~/Notifica.aspx"))
            'Exit Sub  VOLTAR DEPOIS XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX, desviando para a página de LogIn
        End If

        ' Descobre qual é o tipo a ser exibido
        mvQueTipo = Request.QueryString("tipo")
        Select Case mvQueTipo
            Case "polo", "escola", "polopendencias", "colaboradortriagem", "colaboradorfuncao", "alocacao", "polocolaborador", "escolaagendamento", "aplicacoes",
                 "turmaregistro", "escolaturmas", "carregaespecializados"
                ' Tudo bem; apenas documentação

            Case Else
                MensagemERROS.Text = TextoVermelho("Requisição deve conter um parâmetro de tipo. ")
                ' Retorna para página de login
                ' XXXXXXXXXXXXXXXXXXXXX
                Exit Sub
        End Select

        ' Neste ponto, a página pode ter sido ativada por um request inicial ou um post back resultante de uma ação do usuário
        ' Depois de acertar o estado dos filtros, é preciso carregar os dados sempre

        ' Recupera os filtros da última rodada
        sSQL = "SELECT FILTROS FROM LOGIN WHERE CPF='" & Parametros(gcParametroCPF) & "'"
        'Parametros(gcParametroFiltros) = ColetaValorString(sSQL)
        mvFiltros = ColetaValorString(sSQL)

        ' Volta ao estado base utilizado no último login
        FiltroAlterado = True
        If ValorDeUmFiltro(0) = "Todos" Then
            ' Esse filtro pode passar para Todos se houver uma interrupção no processo de inclusão de cadastro
            mvFiltros = EmpacotaUmFiltro(mvFiltros, 0, Parametros(gcParametroUFbase))
        ElseIf ValorDeUmFiltro(0) <> Parametros(gcParametroUFbase) Then
            Parametros(gcParametroUFbase) = ValorDeUmFiltro(0)
        Else
            FiltroAlterado = False
        End If

        ' Indica se alguma foi escolhida para drill down
        mvTurmaEscolhida = ""
        If Not IsPostBack Then
            ' Pega o estado que está no filtro; isso ocorre porque pode ser um Administrador o usuário
            Bandeirinha.ImageUrl = "../FIGURAS/" & "icone-" & ValorDeUmFiltro(0) & ".png"
            mvPrimeiraPaginaMostrada = 1

            ' Limpa a lupinha se estiver preenchida
            If ValorDeUmFiltro(7) <> "" Then
                mvFiltros = EmpacotaUmFiltro(mvFiltros, 7, "")
                FiltroAlterado = True
            End If
            'Bandeirinha.ImageUrl = "../FIGURAS/" & "icone-" & Parametros(gcParametroUFbase) & ".png"
            '' O estado base vem do parâmetro, embora seja um filtro
            'Parametros(gcParametroFiltros) = EmpacotaUmFiltro(0, Parametros(gcParametroUFbase))        
        Else

            ' Pega o evento do Java script
            tmp = Request.Form("__EVENTTARGET")
            If tmp <> "" Then
                Dim x As String() = Split(Request.Form("__EVENTTARGET"), ";")
                If Left(x(0), 1) <> "@" And UBound(x) = 2 Then
                    ' Com 3 parâmetros: algum dos comandos da tabela foi acionado: Editar Remover, Agendar, etc.

                    ' Empacota os filtros e guarda no parâmetro adequado pois provavelmente serão passados adiante
                    'Parametros(gcParametroFiltros) = EmpacotaFiltros()
                    'mvFiltros = EmpacotaFiltros()
                    ' Age de acordo com o tipo de dado que está sendo tratado
                    If x(1) <> "polopendencias" And x(0) <> "Remover" Then CaixaRemocao.Text = ""
                    Select Case x(1)
                        Case "polo", "polopendencias"
                            Select Case x(0)
                                Case "Editar"
                                    Response.Redirect(ResolveUrl("~/CadastroPolo.aspx" & MontaQueryStringFromParametros("POLOS" & ",tipo=" & mvQueTipo, "CadastroPolo", Parametros) & "&polo=" & x(2)))
                                Case "Orçar"
                                    If IsOrcamentoFechado(Parametros(gcParametroUFbase), 1) And Parametros(gcParametroFuncao) <> "Administrador" Then
                                        MensagemERROS.Text = TextoVermelho("A edição de dados de orçamento da capacitação está encerrada.")
                                    Else
                                        Response.Redirect(ResolveUrl("~/OrcamentoPolo.aspx" & MontaQueryStringFromParametros("POLOS" & ",tipo=" & mvQueTipo, "OrcamentoPolo", Parametros) & "&polo=" & x(2)))
                                    End If
                                Case "Alugar"
                                    If IsOrcamentoFechado(Parametros(gcParametroUFbase), 2) And Parametros(gcParametroFuncao) <> "Administrador" Then
                                        MensagemERROS.Text = TextoVermelho("A edição de dados de orçamento da locação está encerrada.")
                                    Else
                                        Response.Redirect(ResolveUrl("~/OrcamentoPolo2.aspx" & MontaQueryStringFromParametros("POLOS" & ",tipo=" & mvQueTipo, "OrcamentoPolo2", Parametros) & "&polo=" & x(2)))
                                    End If
                                Case "Capacitar"
                                    If IsOrcamentoFechado(Parametros(gcParametroUFbase), 3) And Parametros(gcParametroFuncao) <> "Administrador" Then
                                        MensagemERROS.Text = TextoVermelho("A edição de dados de orçamento da capacitação está encerrada.")
                                    Else
                                        Response.Redirect(ResolveUrl("~/OrcamentoPolo3.aspx" & MontaQueryStringFromParametros("POLOS" & ",tipo=" & mvQueTipo, "OrcamentoPolo3", Parametros) & "&polo=" & x(2)))
                                    End If
                                Case "Remover"
                                    If RemoveUmPolo(x(2)) <> "OK" Then
                                        MensagemERROS.Text = TextoVermelho("O polo não pode ser removido.")
                                    End If
                                Case "Visualizar"
                                    Response.Redirect(ResolveUrl("~/CadastroPolo.aspx" & MontaQueryStringFromParametros("POLOS" & ",tipo=" & mvQueTipo, "CadastroPolo", Parametros) & "&polo=" & x(2)))
                                Case "VisualizarOrçamento"
                                    Response.Redirect(ResolveUrl("~/OrcamentoPolo.aspx" & MontaQueryStringFromParametros("POLOS" & ",tipo=" & mvQueTipo, "OrcamentoPolo", Parametros) & "&polo=" & x(2)))
                                Case "Upload"
                                    Response.Redirect(ResolveUrl("~/InstrumentoInspecaoPOLO.aspx" & MontaQueryStringFromParametros("POLOS" & ",tipo=" & mvQueTipo, "InstrumentoInspecaoPOLO", Parametros) & "&polo=" & x(2)))

                            End Select
                        Case "escola"
                                    Select Case x(0)
                                Case "Agendar"
                                    Response.Redirect(ResolveUrl("~/AGENDAMENTO.aspx" & MontaQueryStringFromParametros("POLOS" & ",tipo=" & mvQueTipo, "AGENDAMENTO", Parametros) & "&escola=" & x(2)))

                                    'MensagemERRO.Text = "O processo de alocação das aplicações será iniciado após o recebimento dos dados do Censo Escolar 2016, <br/>previsto para a segunda semana de setembro."
                                Case "Editar"
                                    Response.Redirect(ResolveUrl("~/CadastroEscola.aspx" & MontaQueryStringFromParametros("POLOS" & ",tipo=" & mvQueTipo, "CadastroEscola", Parametros) & "&escola=" & x(2)))
                                Case "Visualizar"
                                    Response.Redirect(ResolveUrl("~/CadastroEscola.aspx" & MontaQueryStringFromParametros("POLOS" & ",tipo=" & mvQueTipo, "CadastroEscola", Parametros) & "&escola=" & x(2)))
                                Case Else
                            End Select
                        Case "escolaturmas"
                            Select Case x(0)
                                Case "Visualizar"
                                    Response.Redirect(ResolveUrl("~/CadastroEscola.aspx" & MontaQueryStringFromParametros("POLOS" & ",tipo=" & mvQueTipo, "CadastroEscola", Parametros) & "&escola=" & x(2)))
                                Case Else
                            End Select
                        Case "escolaagendamento"
                            Select Case x(0)
                                Case "Agendar"
                                    Response.Redirect(ResolveUrl("~/AGENDAMENTO.aspx" & MontaQueryStringFromParametros("POLOS" & ",tipo=" & mvQueTipo, "AGENDAMENTO", Parametros) & "&escola=" & x(2)))

                                    'MensagemERRO.Text = "O processo de alocação das aplicações será iniciado após o recebimento dos dados do Censo Escolar 2016, <br/>previsto para a segunda semana de setembro."
                                Case "Editar"
                                    Response.Redirect(ResolveUrl("~/CadastroEscola.aspx" & MontaQueryStringFromParametros("POLOS" & ",tipo=" & mvQueTipo, "CadastroEscola", Parametros) & "&escola=" & x(2)))
                                Case "Visualizar"
                                    Response.Redirect(ResolveUrl("~/CadastroEscola.aspx" & MontaQueryStringFromParametros("POLOS" & ",tipo=" & mvQueTipo, "CadastroEscola", Parametros) & "&escola=" & x(2)))
                                Case "Agrupar"
                                    Response.Redirect(ResolveUrl("~/AgruparTurmas.aspx" & MontaQueryStringFromParametros("POLOS" & ",tipo=" & mvQueTipo, "AgruparTurmas", Parametros) & "&escola=" & x(2)))
                                Case Else
                            End Select
                        Case "colaboradortriagem"
                            Select Case x(0)
                                Case "Aceitar", "PromoverDemover", "Reservar"
                                    Dim sSQL1 As String = ""
                                    Select Case x(0)
                                        Case "Aceitar"
                                            sSQL1 = "UPDATE PESSOAL set NO_STATUS='Apto' WHERE CPF='" & x(2) & "'"
                                        Case "PromoverDemover"
                                            sSQL1 = "UPDATE PESSOAL set NO_CATEGORIA = Case when NO_CATEGORIA='Gestor' then 'Aplicador' else 'Gestor' End WHERE CPF='" & x(2) & "'"
                                        Case "Reservar"
                                            sSQL1 = "UPDATE PESSOAL set NO_STATUS='Reserva' WHERE CPF='" & x(2) & "'"
                                    End Select
                                    Try
                                        Call ExecutaSQL(sSQL1)
                                        'Call CarregaDADOS()
                                    Catch ex As Exception
                                        MensagemERROS.Text = TextoVermelho("Erro na alteração dos dados." & vbCrLf & ex.Message)
                                    End Try

                                Case "Remover"
                                    Dim sSQL1 As String = "DELETE PESSOAL WHERE CPF='" & x(2) & "'"
                                    Call ExecutaSQL(sSQL1)
                                    'Call CarregaDADOS()

                                Case "Visualizar"
                                    Response.Redirect(ResolveUrl("~/CadastroPessoal.aspx" & MontaQueryStringFromParametros("POLOS" & ",tipo=" & mvQueTipo, "CadastroPessoal", Parametros) & "&pessoa=" & x(2)))

                                Case "Associar"
                                    Response.Redirect(ResolveUrl("~/AtribuiPolo.aspx" & MontaQueryStringFromParametros("POLOS" & ",tipo=" & mvQueTipo, "POLOS", Parametros) & "&pessoa=" & x(2)))
                                Case "ResetarSenha"
                                    Dim sSQLx As String = "UPDATE PESSOAL set Senha='' WHERE CPF='" & x(2) & "'"
                                    Try
                                        Call ExecutaSQL(sSQLx)
                                    Catch ex As Exception
                                        MensagemERROS.Text = TextoVermelho("Erro na na operação de reset da senha." & vbCrLf & ex.Message)
                                    End Try
                                Case Else
                            End Select
                        Case "colaboradorfuncao"
                            Select Case x(0)
                                Case "Atribuir"
                                    Response.Redirect(ResolveUrl("~/AtribuiFuncao.aspx" & MontaQueryStringFromParametros("POLOS" & ",tipo=" & mvQueTipo, "POLOS", Parametros) & "&pessoa=" & x(2)))
                                Case "Remover"
                                    Try
                                        Dim sSQL1 As String = "DELETE ATRIBUICAO_GESTOR WHERE ID=" & x(2)
                                        Call ExecutaSQL(sSQL1)
                                        'Call CarregaDADOS()
                                    Catch ex As Exception
                                        MensagemERROS.Text = TextoVermelho("Erro na remoção da função." & vbCrLf & ex.Message)
                                    End Try
                                Case "Editar"
                                    Response.Redirect(ResolveUrl("~/CadastroPolo.aspx" & MontaQueryStringFromParametros("POLOS" & ",tipo=" & mvQueTipo, "CadastroPolo", Parametros) & "&polo=" & x(2)))
                                Case "Visualizar"
                                    Response.Redirect(ResolveUrl("~/CadastroPessoal.aspx" & MontaQueryStringFromParametros("POLOS" & ",tipo=" & mvQueTipo, "CadastroPessoal", Parametros) & "&pessoa=" & x(2)))
                                Case Else
                            End Select
                        Case "polocolaborador"
                            Select Case x(0)
                                Case "Associar"
                                    Response.Redirect(ResolveUrl("~/AtribuiPolo.aspx" & MontaQueryStringFromParametros("POLOS" & ",tipo=" & mvQueTipo, "POLOS", Parametros) & "&pessoa=" & x(2)))
                                Case "Remover"
                                    Try
                                        Dim sSQL1 As String = "DELETE ATRIBUICAO_POLO WHERE ID=" & x(2)
                                        Call ExecutaSQL(sSQL1)
                                    Catch ex As Exception
                                        MensagemERROS.Text = TextoVermelho("Erro na remoção da associação." & vbCrLf & ex.Message)
                                    End Try
                                Case "Visualizar"
                                    Response.Redirect(ResolveUrl("~/CadastroPessoal.aspx" & MontaQueryStringFromParametros("POLOS" & ",tipo=" & mvQueTipo, "CadastroPessoal", Parametros) & "&pessoa=" & x(2)))
                                Case "Visualizar"
                                    Response.Redirect(ResolveUrl("~/CadastroPessoal.aspx" & MontaQueryStringFromParametros("POLOS" & ",tipo=" & mvQueTipo, "CadastroPessoal", Parametros) & "&pessoa=" & x(2)))
                                Case Else
                            End Select
                        Case "turmaregistro"
                            Response.Redirect(ResolveUrl("~/RegistraAplicacaoTurma.aspx" & MontaQueryStringFromParametros("POLOS" & ",tipo=" & mvQueTipo, "RegistraAplicacaoTurma", Parametros) & "&turma=" & x(2)))
                        Case "carregaespecializados"
                            Select Case x(0)
                                Case "Visualizar"
                                    Response.Redirect(ResolveUrl("~/CadastroPessoal.aspx" & MontaQueryStringFromParametros("POLOS" & ",tipo=" & mvQueTipo, "CadastroPessoal", Parametros) & "&pessoa=" & x(2)))
                                Case Else
                            End Select
                        Case Else
                    End Select

                ElseIf Left(x(0), 1) = "@" Then
                    ' Nesse caso houve um clique de algum dos combos ou do comando de busca
                    CaixaRemocao.Text = ""

                    ' Verifica se o evento foi originado de algum combo via JavaScript
                    Select Case x(0)
                        Case "@Filtro"

                            ' Descobre o combo que originou o evento
                            Dim Posicao As Int16
                            Select Case x(1)
                                Case "U"            ' UF
                                    Posicao = 0
                                Case "M"            ' Município
                                    Posicao = 1
                                Case "E"            ' Escola
                                    Posicao = 2
                                Case "P"            ' Polo
                                    Posicao = 3
                                Case "C"            ' Categoria
                                    Posicao = 4
                                Case "F"            ' Função
                                    Posicao = 5
                                Case "S"            ' Status
                                    Posicao = 6
                                Case "B"            ' String de busca
                                    Posicao = 7
                                Case "A", "R"       ' Avançar/Retroceder na paginação
                                    Posicao = -2
                                Case "PG"           ' Páginação
                                    Posicao = -3
                                Case Else
                                    Posicao = -1
                            End Select
                            If Posicao >= 0 Then
                                ' Atualiza os filtros
                                If Posicao <> 7 Then
                                    mvFiltros = EmpacotaUmFiltro(mvFiltros, Posicao, x(2))
                                    ' Em alguns casos, reseta a string de busca
                                    If Posicao <> 6 Then mvFiltros = EmpacotaUmFiltro(mvFiltros, 7, "")
                                Else
                                    ' Atualiza s string de busca
                                    mvFiltros = EmpacotaUmFiltro(mvFiltros, 7, x(3))
                                End If

                                ' Marca a necessidade de atualizar os filtros no banco de dados
                                FiltroAlterado = True
                                mvPrimeiraPaginaMostrada = 1
                            ElseIf Posicao = -2 Then
                                Dim wDe As Int16 = CInt(x(4))
                                If x(1) = "R" Then
                                    ' Retroceder
                                    mvPrimeiraPaginaMostrada = IIf(wDe - 1 < 1, 1, wDe - 1)
                                Else
                                    ' Avançar
                                    mvPrimeiraPaginaMostrada = wDe + 1
                                End If
                            ElseIf Posicao = -3 Then
                                ' Escolheu um número pde página
                                mvPrimeiraPaginaMostrada = CInt(x(4))
                            End If

                        Case "@Turma"
                            mvTurmaEscolhida = x(1)
                            mvPrimeiraPaginaMostrada = CInt(x(2))

                    End Select
                Else
                    ' Foi um evento dos botões da página
                    Beep()
                End If
            End If
        End If

        ' Neste ponto, os eventos já foram tratados. Sejam de request ou de postback
        ' É preciso carregar tudo da página novamente
        Call ExibeOcultaFiltros()
        Call CarregaEstadosFgvNumCombo(cmbUF, "Todos")
        Call PreencheMunicipiosDeUmEstado(cmbMunicipio, ValorDeUmFiltro(0), "Todos")
        If cmbPolo.Visible Then
            Select Case mvQueTipo
                Case "escolaagendamento", "turmaregistro"
                    Call CarregaPolosDeUmCPF(cmbPolo, Parametros(gcParametroCPF), ValorDeUmFiltro(0), "Todos")
                Case Else
                    Call CarregaPolosDeUmEstadoMunicipio(cmbPolo, ValorDeUmFiltro(0), ValorDeUmFiltro(1), "Todos")
            End Select

        End If
        If cmbEscola.Visible Then Call CarregaEscolasDeUmEstadoMunicipioPolo(cmbEscola, ValorDeUmFiltro(0), ValorDeUmFiltro(1), ValorDeUmFiltro(3), "Todas")
        ' Seta os valores selecionados em cada combo
        Call AplicaFiltrosDoPassaporte()
        Bandeirinha.ImageUrl = "../FIGURAS/" & "icone-" & ValorDeUmFiltro(0) & ".png"
        ' É preciso atualizar os filtros porque algum deles pode ter sido alterado no tratamento dos eventos
        If FiltroAlterado Then
            'sSQL = "UPDATE LOGIN set FILTROS='" & Parametros(gcParametroFiltros) & "' WHERE CPF='" & Parametros(gcParametroCPF) & "'"
            sSQL = "UPDATE LOGIN set FILTROS='" & mvFiltros & "' WHERE CPF='" & Parametros(gcParametroCPF) & "'"
            Call ExecutaSQL(sSQL)
        End If
        Call CarregaDADOS()
    End Sub


    Private Sub AplicaFiltrosDoPassaporte()
        cmbUF.Text = ValorDeUmFiltro(0)
        'If Parametros(gcParametroFuncao) <> "Administrador" And Parametros(gcParametroFuncao) <> "Observador" Then
        '    cmbMunicipio.Enabled = False
        'End If
        cmbMunicipio.Text = ValorDeUmFiltro(1)
        If cmbEscola.Visible Then cmbEscola.Text = ValorDeUmFiltro(2)
        If cmbPolo.Visible Then cmbPolo.Text = ValorDeUmFiltro(3)
        If cmbCategoria.Visible Then cmbCategoria.Text = ValorDeUmFiltro(4)
        If cmbFuncao.Visible Then cmbFuncao.Text = ValorDeUmFiltro(5)
        If cmbStatus.Visible Then cmbStatus.Text = ValorDeUmFiltro(6)
        CampoFiltroBusca.Text = ValorDeUmFiltro(7)
    End Sub


    Private Sub CarregaPolos()

        Dim OK As String = "OK"
        Dim row As TableRow
        Dim PoloID As String

        Try
            Dim sSQL As String =
            "SELECT TOP " & mcLimiteLinhasLidas & " p.SG_UF + ' - ' + isnull(m.NO_MUNICIPIO,'') UF_MUNIC, p.NO_POLO," &
                   " isnull(pp0.CPF,'') COORDENADOR_CPF,isnull(pp0.NO_PESSOA,'') NO_PESSOA," &
                   " isnull(pp1.CPF,'') APOIO_LOGISTICO_CPF,isnull(pp1.NO_PESSOA,'') NO_PESSOAal," &
                   " isnull(pp2.CPF,'') SUBCOORDENADOR_CPF,isnull(pp2.NO_PESSOA,'') NO_PESSOAsub," &
                   " isnull(pe.NO_POLO,'') NO_POLO_ENTREGA,p.*," &
                   "(SELECT COUNT(*) FROM ESCOLA e WHERE p.ID_POLO=e.ID_POLO) NU_ESCOLAS," &
                   "(SELECT COUNT(*) FROM TURMA t, ESCOLA e WHERE t.ID_ESCOLA=e.ID_ESCOLA And p.ID_POLO=e.ID_POLO) NU_TURMAS" &
           " FROM (SELECT * FROM POLO WHERE" &
                " (@SG_UF='Todos' or @SG_UF='BR' or SG_UF=@SG_UF)" &
                " And (@NO_POLO='Todos' or NO_POLO=@NO_POLO)" &
                " And (@NO_MUNICIPIO='Todos' or NO_MUNICIPIO=@NO_MUNICIPIO)" &
                " And (@BUSCA='' or NO_POLO like '%' + @BUSCA + '%')" &
                " And EXISTS(select * from ATRIBUICAO_GESTOR a where a.CPF=@MeuCPF and (a.ID_POLO='*' or a.ID_POLO=POLO.ID_POLO)))p" &
                " LEFT JOIN MUNICIPIO m on p.CO_MUNICIPIO= m.CO_MUNICIPIO" &
                " Left Join(SELECT p.NO_PESSOA, p.CPF, p.TX_EMAIL, a.ID_POLO" &
                                                " From PESSOAL p, ATRIBUICAO_GESTOR a  Where p.CPF = a.CPF And a.NO_FUNCAO ='Coordenador de Polo')pp0 on p.ID_POLO=pp0.ID_POLO" &
                " Left Join (Select p.NO_PESSOA, p.CPF, p.TX_EMAIL, a.ID_POLO" &
                                                   " From PESSOAL p, ATRIBUICAO_GESTOR a  Where p.CPF = a.CPF And a.NO_FUNCAO ='Apoio Logístico')pp1 on p.ID_POLO=pp1.ID_POLO" &
                " Left Join (Select p.NO_PESSOA, p.CPF, p.TX_EMAIL, a.ID_POLO" &
                                                   " From PESSOAL p, ATRIBUICAO_GESTOR a  Where p.CPF = a.CPF And a.NO_FUNCAO ='Subcoordenador Estadual')pp2 on p.ID_POLO=pp2.ID_POLO" &
                " Left Join POLO pe on p.ID_POLO_ENTREGA=pe.ID_POLO"

            'Call AbreConexao()
            Dim MyC = New System.Data.SqlClient.SqlConnection(Ligacao())
            MyC.Open()

            Dim MeuComando = New System.Data.SqlClient.SqlCommand(sSQL, MyC)
            Dim RS As System.Data.SqlClient.SqlDataReader

            MeuComando.Parameters.Add("@SG_UF", SqlDbType.VarChar).Value = cmbUF.Text
            MeuComando.Parameters.Add("@NO_MUNICIPIO", SqlDbType.VarChar).Value = cmbMunicipio.Text
            MeuComando.Parameters.Add("@NO_POLO", SqlDbType.VarChar).Value = cmbPolo.Text
            MeuComando.Parameters.Add("@BUSCA", SqlDbType.VarChar).Value = CampoFiltroBusca.Text
            MeuComando.Parameters.Add("@MeuCPF", SqlDbType.VarChar).Value = Parametros(gcParametroCPF)

            MeuComando.CommandText = sSQL
            RS = MeuComando.ExecuteReader

            ' Limpa a tabela e define a largura de cada coluna
            TabelaPolos.Controls.Clear()
            Dim LL As Int16() = {90, 60, 80, 20, 80, 80, 80, 20, 20, 20, 20, 20, 20}

            row = New TableRow()
            Call CarregaColunasDeTextoNumaLinha(row, "Polo|UF - Município|Endereço|Telefone|Coordenador de Polo|Subcoordenador Estadual|Apoio Logístico|Escolas|Turmas|Visualização|Edição|Remoção", LL)
            TabelaPolos.Controls.Add(row)

            ' Varre record set
            Dim C As Int32 = 0
            Dim Linhas As Int16 = 0
            While RS.Read()
                C += 1
                ' Sem limitação de linhas para polos
                If Linhas < mcLimiteLinhasMostradas + 1000 Then
                    Linhas += 1
                    row = New TableRow()
                    Dim tmpSTR As String = RS.GetString(RS.GetOrdinal("NO_POLO")) + "|" +
                                       RS.GetString(RS.GetOrdinal("UF_MUNIC")) + "|" +
                                       RS.GetString(RS.GetOrdinal("End_Parte1")) + "|" +
                                       RS.GetString(RS.GetOrdinal("NU_TELEFONE1")) + "|" +
                                       RS.GetString(RS.GetOrdinal("NO_PESSOA")) + "|" +
                                       RS.GetString(RS.GetOrdinal("NO_PESSOAsub")) + "|" +
                                       RS.GetString(RS.GetOrdinal("NO_PESSOAal")) + "|" +
                                       Str(RS.GetValue(RS.GetOrdinal("NU_ESCOLAS"))) + "|" +
                                       Str(RS.GetValue(RS.GetOrdinal("NU_TURMAS")))

                    ' Prepara as colunas
                    Call CarregaColunasDeTextoNumaLinha(row, tmpSTR, LL)

                    ' Prepara Java Script "javascript: __doPostBack('xxx','xxx')"
                    PoloID = RS.GetString(RS.GetOrdinal("ID_POLO"))

                    '' Posiciona os botões
                    'Call CarregaColunaDeBotaoNumaLinha(row, "Fotos", PoloID, "basic1-099_image_photo_galery_album.png",
                    '                               "Carregar as três fotos do polo", mvQueTipo)

                    ' Qualquer um pode visualizar
                    Call CarregaColunaDeBotaoNumaLinha(row, "Visualizar", PoloID & ",V", "basic3-020_presentation_powerpoint_keynote.png",
                                                           "Clique aqui para visualizar os dados do polo", mvQueTipo)

                    ' Somente coordenadores estaduais e gestores do polo podem editar
                    If Parametros(gcParametroFuncao) = "Coordenador Estadual" _
                        Or Parametros(gcParametroFuncao) = "Administrador" _
                        Or RS.GetString(RS.GetOrdinal("COORDENADOR_CPF")) = Parametros(gcParametroCPF) _
                        Or RS.GetString(RS.GetOrdinal("SUBCOORDENADOR_CPF")) = Parametros(gcParametroCPF) _
                        Or RS.GetString(RS.GetOrdinal("APOIO_LOGISTICO_CPF")) = Parametros(gcParametroCPF) Then
                        Call CarregaColunaDeBotaoNumaLinha(row, "Editar", PoloID & ",U", "basic1-002_write_pencil_new_edit.png",
                                                           "Editar os dados do polo", mvQueTipo)
                    Else
                        Call CarregaColunasDeTextoNumaLinha(row, "", LL)
                    End If

                    ' Somente coordenadores estaduais podem remover, desde que não haja gestores vinculados ao polo
                    If (Parametros(gcParametroFuncao) = "Coordenador Estadual" Or Parametros(gcParametroFuncao) = "Administrador") _
                    And RS.GetString(RS.GetOrdinal("NO_PESSOA")) = "" _
                    And RS.GetString(RS.GetOrdinal("NO_PESSOAsub")) = "" _
                    And RS.GetString(RS.GetOrdinal("NO_PESSOAal")) = "" Then
                        Call CarregaColunaDeBotaoNumaLinha(row, "Remover", PoloID, "basic1-020_bin_trash_delete.png",
                                                           "Clique aqui para remover este polo", mvQueTipo)
                    Else
                        Call CarregaColunasDeTextoNumaLinha(row, "", LL)
                    End If

                    If Pauta(C) Then row.BackColor = mvCorPautas
                    TabelaPolos.Controls.Add(row)
                End If
            End While

            ' Mostra o totalde linhas
            LiteralNumLinhas.Text = RecadoSobreLinhas(Linhas, C, False)
            ' Encerra a leitura
            RS.Close()
            RS.Dispose()
            MeuComando.Dispose()
            MyC.Dispose()

        Catch ex As Exception
            OK = "Erro na leitura dos dados de polos." & vbCrLf & ex.Message

        End Try

        If OK <> "OK" Then MensagemERROS.Text = TextoVermelho(OK)

    End Sub


    Protected Function Pauta(ByRef C As Int32) As Boolean
        Return C Mod 4 = 1 Or C Mod 4 = 2
    End Function


    Private Sub CarregaEscolas()

        Dim OK As String = "OK"
        Dim row As TableRow
        Dim EscolaID As String

        Try
            Dim Juncao1 As String = IIf(cmbMunicipio.Text = "Todos", " LEFT JOIN ", " INNER JOIN ")
            Dim Juncao2 As String = IIf(cmbPolo.Text = "Todos", " LEFT JOIN ", " INNER JOIN ")

            Dim sSQL As String =
            "SELECT TOP " & mcLimiteLinhasLidas &
                  " x.SG_UF + ' - ' + isnull(m.NO_MUNICIPIO,'') UF_MUNIC, x.ID_ESCOLA,x.NO_ESCOLA,x.End_Parte1," &
                  " isnull(p.NO_POLO,'') NO_POLO, isnull(p.ID_POLO,'') ID_POLO," &
                  " isnull(a1.CPF,'') COORDENADOR_CPF," &
                  " isnull(a2.CPF,'') APOIO_LOGISTICO_CPF," &
                  " isnull(a3.CPF,'') SUBCOORDENADOR_CPF," &
                  "(SELECT COUNT(*) FROM TURMA t WHERE t.ID_ESCOLA=x.ID_ESCOLA) NU_TURMAS" &
           " FROM (SELECT * FROM ESCOLA e WHERE" &
                " (@SG_UF='Todos' or @SG_UF='BR' or SG_UF=@SG_UF)" &
                " And (@BUSCA='' or NO_ESCOLA like '%' + @BUSCA + '%')" &
                " )x" &
                Juncao1 & "(select * FROM MUNICIPIO where @NO_MUNICIPIO='Todos' or NO_MUNICIPIO=@NO_MUNICIPIO)m on x.CO_MUNICIPIO= m.CO_MUNICIPIO" &
                Juncao2 & "(select * FROM POLO where @NO_POLO='Todos' or NO_POLO=@NO_POLO)p on x.ID_POLO = p.ID_POLO" &
                " Left Join(SELECT CPF,ID_POLO" &
                          " From ATRIBUICAO_GESTOR Where NO_FUNCAO ='Coordenador de Polo')a1 on a1.ID_POLO=x.ID_POLO" &
                " Left Join (SELECT CPF,ID_POLO" &
                          " From ATRIBUICAO_GESTOR Where NO_FUNCAO ='Apoio Logístico')a2 on a2.ID_POLO=x.ID_POLO" &
                " Left Join (SELECT CPF,ID_POLO" &
                          " From ATRIBUICAO_GESTOR Where NO_FUNCAO ='Subcoordenador Estadual')a3 on a3.ID_POLO=x.ID_POLO" &
           " ORDER BY case when p.NO_POLO is null then ' ' + x.NO_ESCOLA else x.SG_UF + ' - ' + m.NO_MUNICIPIO end," &
                    " x.NO_ESCOLA"

            'Call AbreConexao()
            Dim MyC = New System.Data.SqlClient.SqlConnection(Ligacao())
            MyC.Open()

            Dim MeuComando = New System.Data.SqlClient.SqlCommand(sSQL, MyC)
            Dim RS As System.Data.SqlClient.SqlDataReader

            MeuComando.Parameters.Add("@SG_UF", SqlDbType.VarChar).Value = cmbUF.Text
            MeuComando.Parameters.Add("@NO_MUNICIPIO", SqlDbType.VarChar).Value = cmbMunicipio.Text
            MeuComando.Parameters.Add("@NO_POLO", SqlDbType.VarChar).Value = cmbPolo.Text
            MeuComando.Parameters.Add("@BUSCA", SqlDbType.VarChar).Value = CampoFiltroBusca.Text

            MeuComando.CommandText = sSQL
            RS = MeuComando.ExecuteReader()

            ' Limpa a tabela e define a largura de cada coluna
            TabelaPolos.Controls.Clear()
            Dim LL As Int16() = {120, 120, 90, 20, 20, 20, 20, 20, 20, 20, 20, 20}

            row = New TableRow()
            Call CarregaColunasDeTextoNumaLinha(row, "Escola|Endereço|UF - Município|Polo|Turmas|Visualização|Edição", LL)    ' Deixa uma coluna em branco no final, para não alargar muito
            TabelaPolos.Controls.Add(row)

            ' Varre record set
            Dim C As Int32 = 0
            Dim Linhas As Int32 = 0
            While RS.Read()
                C += 1
                If C >= (mvPrimeiraPaginaMostrada - 1) * mcLimiteLinhasMostradas + 1 And Linhas < mcLimiteLinhasMostradas Then
                    Linhas += 1
                    row = New TableRow()
                    Dim tmpSTR As String = RS.GetString(RS.GetOrdinal("NO_ESCOLA")) + "|" +
                                   RS.GetString(RS.GetOrdinal("End_Parte1")) + "|" +
                                   RS.GetString(RS.GetOrdinal("UF_MUNIC")) + "|" +
                                   RS.GetString(RS.GetOrdinal("NO_POLO")) + "|" +
                                   Str(RS.GetValue(RS.GetOrdinal("NU_TURMAS")))
                    ' Prepara as colunas
                    Call CarregaColunasDeTextoNumaLinha(row, tmpSTR, LL)
                    ' Prepara Java Script "javascript: __doPostBack('xxx','xxx')"
                    EscolaID = RS.GetString(RS.GetOrdinal("ID_ESCOLA"))

                    ' Posiciona os botões
                    ' Qualquer um pode visualizar
                    Call CarregaColunaDeBotaoNumaLinha(row, "Visualizar", EscolaID & ",V", "basic3-020_presentation_powerpoint_keynote.png",
                                                           "Clique aqui para visualizar os dados da escola", mvQueTipo)

                    If Parametros(gcParametroFuncao) = "Coordenador Estadual" _
                        Or Parametros(gcParametroFuncao) = "Administrador" _
                        Or RS.GetString(RS.GetOrdinal("COORDENADOR_CPF")) = Parametros(gcParametroCPF) _
                        Or RS.GetString(RS.GetOrdinal("SUBCOORDENADOR_CPF")) = Parametros(gcParametroCPF) _
                        Or RS.GetString(RS.GetOrdinal("APOIO_LOGISTICO_CPF")) = Parametros(gcParametroCPF) Then

                        Call CarregaColunaDeBotaoNumaLinha(row, "Editar", EscolaID & ",U", "basic1-002_write_pencil_new_edit.png",
                           "Clique aqui para editar os dados da escola", mvQueTipo)

                    Else
                        ' Colunas em branco
                        Call CarregaColunasDeTextoNumaLinha(row, "", LL)
                    End If
                    If Pauta(C) Then row.BackColor = mvCorPautas
                    TabelaPolos.Controls.Add(row)
                End If
            End While

            ' Mostra o totalde linhas
            LiteralNumLinhas.Text = RecadoSobreLinhas(Linhas, C, True)
            Call AcertaPaginacao(C)
            cmbPaginas.Items.Clear()
            For kkk = 1 To System.Math.Ceiling(C / mcLimiteLinhasMostradas)
                cmbPaginas.Items.Add(Trim(Str(kkk)))
            Next
            cmbPaginas.Text = Trim(Str(mvPrimeiraPaginaMostrada))

            ' Encerra a leitura
            RS.Close()
            RS.Dispose()
            MeuComando.Dispose()
            MyC.Dispose()

        Catch ex As Exception
            OK = "Erro na leitura dos dados das escolas. " & vbCrLf & ex.Message
        End Try
        If OK <> "OK" Then MensagemERROS.Text = TextoVermelho(OK)
    End Sub


    Private Sub CarregaAPLICACOES()

        Dim OK As String = "OK"
        Dim row As TableRow

        Try

            Dim sSQL As String =
            "SELECT p.NO_POLO, e.NO_ESCOLA, e.End_Parte1,t.CO_TURMA_CENSO,t.NO_TURMA,pr.NO_PROVA," &
                   "ts.SEQ_TURMA_SALA,ts.ID_GRUPO_SALA,gs.DS_GRUPO_SALA,pp.NO_PESSOA" &
           " FROM PROVA pr," &
                "(SELECT * FROM POLO where @NO_POLO='Todos' or NO_POLO=@NO_POLO) p," &
                "(SELECT * FROM ESCOLA e WHERE (@SG_UF='Todos' or SG_UF=@SG_UF) and (@NO_MUNICIPIO='Todos' or NO_MUNICIPIO=@NO_MUNICIPIO)) e," &
                "TURMA t, TURMA_APLICACAO ta, TURMA_SALA ts, GRUPO_SALA gs," &
                "(SELECT * FROM PESSOAL p WHERE (@SG_UF='Todos' or SG_UF_ALOC=@SG_UF)" &
                                          " And (@MeuCPF='' or CPF=@MeuCPF)" &
                                          " And (@BUSCA='' or NO_PESSOA like '%' + @BUSCA + '%' or CPF like '%' + @BUSCA + '%')) pp" &
           " WHERE" &
               " e.ID_POLO=p.ID_POLO And e.ID_ESCOLA=t.ID_ESCOLA And ta.CO_TURMA_CENSO=t.CO_TURMA_CENSO And ta.ID_PROVA=pr.ID_PROVA" &
           " And ts.CO_TURMA_CENSO=t.CO_TURMA_CENSO And ts.ID_PROVA=pr.ID_PROVA And ts.ID_GRUPO_SALA=gs.ID_GRUPO_SALA And ts.CPF_APLICADOR=pp.CPF" &
           " ORDER BY p.NO_POLO, e.NO_ESCOLA,t.NO_TURMA,pr.NO_PROVA"

            'Call AbreConexao()
            Dim MyC = New System.Data.SqlClient.SqlConnection(Ligacao())
            MyC.Open()

            Dim MeuComando = New System.Data.SqlClient.SqlCommand(sSQL, MyC)
            Dim RS As System.Data.SqlClient.SqlDataReader

            MeuComando.Parameters.Add("@SG_UF", SqlDbType.VarChar).Value = cmbUF.Text
            MeuComando.Parameters.Add("@NO_MUNICIPIO", SqlDbType.VarChar).Value = cmbMunicipio.Text
            MeuComando.Parameters.Add("@NO_POLO", SqlDbType.VarChar).Value = cmbPolo.Text
            MeuComando.Parameters.Add("@BUSCA", SqlDbType.VarChar).Value = CampoFiltroBusca.Text
            MeuComando.Parameters.Add("@MeuCPF", SqlDbType.VarChar).Value = ""

            MeuComando.CommandText = sSQL
            RS = MeuComando.ExecuteReader()

            ' Limpa a tabela e define a largura de cada coluna
            TabelaPolos.Controls.Clear()
            Dim LL As Int16() = {90, 80, 90, 20, 20, 20, 20, 20, 20, 20, 20, 20}

            row = New TableRow()
            Call CarregaColunasDeTextoNumaLinha(row, "Escola|UF - Polo|Escola|Endereço|Cód Turma|Turma|Prova|Sala|Atendimento|Aplicador", LL)    ' Deixa uma coluna em branco no final, para não alargar muito
            TabelaPolos.Controls.Add(row)

            ' Varre record set
            Dim C As Int32 = 0
            Dim Linhas As Int32 = 0
            While RS.Read()
                C += 1
                If C >= (mvPrimeiraPaginaMostrada - 1) * mcLimiteLinhasMostradas + 1 And Linhas < mcLimiteLinhasMostradas Then
                    Linhas += 1
                    row = New TableRow()
                    Dim tmpSTR As String = RS.GetString(RS.GetOrdinal("NO_POLO")) + "|" +
                                   RS.GetString(RS.GetOrdinal("NO_ESCOLA")) + "|" +
                                   RS.GetString(RS.GetOrdinal("End_Parte1")) + "|" +
                                   RS.GetString(RS.GetOrdinal("CO_TURMA_CENSO")) + "|" +
                                   RS.GetString(RS.GetOrdinal("NO_TURMA")) + "|" +
                                   RS.GetString(RS.GetOrdinal("CO_TURMA_CENSO")) + "|" +
                                   RS.GetString(RS.GetOrdinal("NO_PROVA")) + "|" +
                                   Str(RS.GetValue(RS.GetOrdinal("SEQ_TURMA_SALA"))) + "|" +
                                   RS.GetString(RS.GetOrdinal("DS_GRUPO_SALA")) + "|" +
                                   RS.GetString(RS.GetOrdinal("NO_PESSOA"))

                    ' Prepara as colunas
                    Call CarregaColunasDeTextoNumaLinha(row, tmpSTR, LL)
                    '' Prepara Java Script "javascript: __doPostBack('xxx','xxx')"
                    'EscolaID = RS.GetString(RS.GetOrdinal("ID_ESCOLA"))

                    '' Posiciona os botões
                    '' Qualquer um pode visualizar
                    'Call CarregaColunaDeBotaoNumaLinha(row, "Visualizar", EscolaID & ",V", "basic3-020_presentation_powerpoint_keynote.png",
                    '                                       "Clique aqui para visualizar os dados da escola", mvQueTipo)

                    'If Parametros(gcParametroFuncao) = "Observador" Then
                    '    ' Somente estão na consulta gestores ligados direta ou indiretamente ao polo
                    '    ' ESCONDE O AGENDAMENTO ------------------------------------
                    '    Call CarregaColunaDeBotaoNumaLinha(row, "Agendar", EscolaID & ",U", "basic1-011_calendar.png",
                    '                                                  "Clique aqui para agendar as aplicações da escola", mvQueTipo)
                    '    'Call CarregaColunasDeTextoNumaLinha(row, "", LL)
                    '    ' ------------------------------------
                    'Else
                    '    ' Observadores podem visualizar apenas
                    '    Call CarregaColunaDeBotaoNumaLinha(row, "Agendar", EscolaID & ",V", "basic1-011_calendar.png",
                    '                                                  "Clique aqui para visualizar o agendamento da escola", mvQueTipo)
                    'End If

                    If Pauta(C) Then row.BackColor = mvCorPautas
                    TabelaPolos.Controls.Add(row)
                End If
            End While

            ' Mostra o totalde linhas
            LiteralNumLinhas.Text = RecadoSobreLinhas(Linhas, C, True)
            Call AcertaPaginacao(C)
            cmbPaginas.Items.Clear()
            For kkk = 1 To System.Math.Ceiling(C / mcLimiteLinhasMostradas)
                cmbPaginas.Items.Add(Trim(Str(kkk)))
            Next
            cmbPaginas.Text = Trim(Str(mvPrimeiraPaginaMostrada))

            ' Encerra a leitura
            RS.Close()
            RS.Dispose()
            MeuComando.Dispose()
            MyC.Dispose()

        Catch ex As Exception
            OK = "Erro na leitura dos dados das escolas. " & vbCrLf & ex.Message
        End Try
        If OK <> "OK" Then MensagemERROS.Text = TextoVermelho(OK)
    End Sub

    Private Sub CarregaEscolasAgendamento()

        Dim OK As String = "OK"
        Dim row As TableRow
        Dim EscolaID As String = ""
        Dim PermiteAgrupamento As Boolean = False

        Try

            Dim sSQL As String =
            "SELECT TOP " & mcLimiteLinhasLidas &
            "      x.*,p.NO_POLO," &
            "      (SELECT COUNT(*) FROM TURMA t WHERE t.ID_ESCOLA=x.ID_ESCOLA) NU_TURMAS," &
            "      (SELECT COUNT(*) FROM TURMA_SALA ts WHERE x.ID_ESCOLA=ts.ID_ESCOLA And ts.ID_GRUPO_SALA=1) NU_SALAS_GRUPO1," &
            "      (SELECT COUNT(*) FROM TURMA_SALA ts WHERE x.ID_ESCOLA=ts.ID_ESCOLA And ts.ID_GRUPO_SALA=2) NU_SALAS_GRUPO2," &
            "      (SELECT COUNT(*) FROM TURMA_SALA ts WHERE x.ID_ESCOLA=ts.ID_ESCOLA And ts.ID_GRUPO_SALA=3) NU_SALAS_GRUPO3," &
            "      (SELECT COUNT(*) FROM TURMA_SALA ts WHERE x.ID_ESCOLA=ts.ID_ESCOLA And ts.ID_GRUPO_SALA=4) NU_SALAS_GRUPO4," &
            "      (SELECT COUNT(*) FROM TURMA_SALA ts WHERE x.ID_ESCOLA=ts.ID_ESCOLA And ts.ID_GRUPO_SALA=0 and ts.CPF_APLICADOR is null) SLS_SEM_APLICADOR_PRINCIPAL," &
            "      (SELECT COUNT(*) FROM TURMA_SALA ts WHERE x.ID_ESCOLA=ts.ID_ESCOLA And ts.ID_GRUPO_SALA in (1,2) and ts.CPF_APLICADOR is null) SLS_SEM_APLICADOR_EXTRA," &
            "      (SELECT COUNT(*) FROM TURMA_SALA ts WHERE x.ID_ESCOLA=ts.ID_ESCOLA And ts.ID_GRUPO_SALA in (3,4) and ts.CPF_APLICADOR is null) SLS_SEM_APLICADOR_GRUPOS," &
            "      CASE when EXISTS (select * FROM TURMA t where t.FLG_CONFIRMADO='N' and t.ID_ESCOLA=x.ID_ESCOLA) then 'S' else 'N' end EXISTE_TURMA_NAO_CONFIRMADA" &
            " FROM (SELECT distinct e.NO_MUNICIPIO, e.ID_ESCOLA, e.NO_ESCOLA, e.NU_TELEFONE, ta.ID_DIA_APLICACAO, ta.ID_PROVA,e.ID_POLO" &
            "       FROM ESCOLA e, TURMA_APLICACAO ta" &
            "       WHERE (@SG_UF='Todos' or SG_UF=@SG_UF) And (@BUSCA='' or NO_ESCOLA like '%' + @BUSCA + '%') " &
            "         And EXISTS(select * from ATRIBUICAO_GESTOR a where a.CPF=@MeuCPF And (a.ID_POLO='*' or a.ID_POLO=e.ID_POLO))" &
            "         And e.ID_ESCOLA=ta.ID_ESCOLA) x," &
            "    (select * FROM POLO where @NO_POLO='Todos' or NO_POLO=@NO_POLO) p " &
            " WHERE p.ID_POLO=x.ID_POLO" &
            " ORDER BY x.NO_ESCOLA,x.ID_DIA_APLICACAO, x.ID_PROVA"

            'Call AbreConexao()
            Dim MyC = New System.Data.SqlClient.SqlConnection(Ligacao())
            MyC.Open()

            Dim MeuComando = New System.Data.SqlClient.SqlCommand(sSQL, MyC)
            Dim RS As System.Data.SqlClient.SqlDataReader

            MeuComando.Parameters.Add("@SG_UF", SqlDbType.VarChar).Value = cmbUF.Text
            MeuComando.Parameters.Add("@NO_MUNICIPIO", SqlDbType.VarChar).Value = cmbMunicipio.Text
            MeuComando.Parameters.Add("@NO_POLO", SqlDbType.VarChar).Value = cmbPolo.Text
            MeuComando.Parameters.Add("@BUSCA", SqlDbType.VarChar).Value = CampoFiltroBusca.Text
            MeuComando.Parameters.Add("@MeuCPF", SqlDbType.VarChar).Value = Parametros(gcParametroCPF)

            MeuComando.CommandText = sSQL
            RS = MeuComando.ExecuteReader()

            ' Limpa a tabela e define a largura de cada coluna
            TabelaPolos.Controls.Clear()
            Dim LL As Int16() = {20, 20, 100, 70, 20, 20, 20, 20, 20, 20, 20, 20, 20, 60, 20, 20}

            ' Controla a mudança
            Dim EscolaAnterior As String = ""

            row = New TableRow()
            Call CarregaColunasDeTextoNumaLinha(row, "|Censo|Escola|Município|Polo|Turmas|Cegueira /Surdez /Surdocegueira|Auxilio Ledor /Auxilio Transcrição|Baixa Visão (10)|Outras Deficiências (10)|||Agendamento|O que falta?|", LL)    ' Deixa uma coluna em branco no final, para não alargar muito
            TabelaPolos.Controls.Add(row)

            ' Varre record set
            Dim C As Int32 = 0
            Dim Linhas As Int32 = 0
            While RS.Read()
                C += 1
                If C >= (mvPrimeiraPaginaMostrada - 1) * mcLimiteLinhasMostradas + 1 And Linhas < mcLimiteLinhasMostradas Then
                    Linhas += 1
                    If EscolaAnterior <> RS.GetString(RS.GetOrdinal("ID_ESCOLA")) Then

                        ' Insere uma linha fininha se não for a primeira escola
                        If EscolaAnterior <> "" Then
                            row = New TableRow()
                            row.Height = 4
                            TabelaPolos.Controls.Add(row)
                        End If

                        ' Começa uma nova escola
                        PermiteAgrupamento = True

                        row = New TableRow()

                        ' Qualquer um pode visualizar
                        Call CarregaColunaDeBotaoNumaLinha(row, "Visualizar",
                                                           RS.GetString(RS.GetOrdinal("ID_ESCOLA")) & ",V",
                                                           "basic3-020_presentation_powerpoint_keynote.png",
                                                           "Clique aqui para visualizar os dados da escola", mvQueTipo)

                        Dim tmpSTR As String = RS.GetString(RS.GetOrdinal("ID_ESCOLA")) + "|" +
                                   RS.GetString(RS.GetOrdinal("NO_ESCOLA")) + "|" +
                                   RS.GetString(RS.GetOrdinal("NO_MUNICIPIO")) + "|" +
                                   RS.GetString(RS.GetOrdinal("NO_POLO")) + "|" +
                                   Str(RS.GetValue(RS.GetOrdinal("NU_TURMAS"))) + "|" +
                                   Str(RS.GetValue(RS.GetOrdinal("NU_SALAS_GRUPO1"))) + "|" +
                                   Str(RS.GetValue(RS.GetOrdinal("NU_SALAS_GRUPO2"))) + "|" +
                                   Str(RS.GetValue(RS.GetOrdinal("NU_SALAS_GRUPO3"))) + "|" +
                                   Str(RS.GetValue(RS.GetOrdinal("NU_SALAS_GRUPO4")))

                        ' Prepara as colunas
                        Call CarregaColunasDeTextoNumaLinha(row, tmpSTR, LL)
                        EscolaID = RS.GetString(RS.GetOrdinal("ID_ESCOLA"))

                        'Pula duas colunas
                        Call CarregaColunasDeTextoNumaLinha(row, "0", LL)
                        Call CarregaColunasDeTextoNumaLinha(row, "0", LL)

                        If Parametros(gcParametroFuncao) = "Observador" Then
                            ' Somente estão na consulta gestores ligados direta ou indiretamente ao polo
                            Call CarregaColunaDeBotaoNumaLinha(row, "Agendar", EscolaID & ",V", "basic1-011_calendar.png",
                                                               "Clique aqui para agendar as aplicações da escola", mvQueTipo)
                        Else
                            ' Observadores podem visualizar apenas
                            Call CarregaColunaDeBotaoNumaLinha(row, "Agendar", EscolaID & ",U", "basic1-011_calendar.png",
                                                               "Clique aqui para visualizar o agendamento da escola", mvQueTipo)
                        End If

                        ' Insere a coluna que vai receber o diagnóstico da turma
                        Call CarregaColunasDeTextoNumaLinha(row, "", LL)

                        ' Faz a indicação do que falta fazerFLG_CONFIRMADO
                        If RS.GetString(RS.GetOrdinal("EXISTE_TURMA_NAO_CONFIRMADA")) = "S" Then
                            Call CarregaColunaDeFiguraNumaLinha(row.Cells(row.Cells.Count - 1), "basic1-013_time_clock.png",
                                                                "Confirmação de dados e horário para alguma turma")
                            PermiteAgrupamento = False
                        End If
                        If RS.GetValue(RS.GetOrdinal("SLS_SEM_APLICADOR_PRINCIPAL")) > 0 Then
                            Call CarregaColunaDeFiguraNumaLinha(row.Cells(row.Cells.Count - 1), "basic2-110_user.png",
                                                                "Aplicador regular para alguma turma")
                            PermiteAgrupamento = False
                        End If
                        If RS.GetValue(RS.GetOrdinal("SLS_SEM_APLICADOR_EXTRA")) > 0 Then
                            Call CarregaColunaDeFiguraNumaLinha(row.Cells(row.Cells.Count - 1), "basic1-117_user_group_couple.png",
                                                                "Aplicador extra/especializado para alguma turma")
                            PermiteAgrupamento = False
                        End If
                        If RS.GetValue(RS.GetOrdinal("SLS_SEM_APLICADOR_GRUPOS")) > 0 Then
                            Call CarregaColunaDeFiguraNumaLinha(row.Cells(row.Cells.Count - 1), "basic3-140_graph_relations_connections_hierarchy.png",
                                                                "Aplicador extra/especializado para alguma turma")
                        End If

                        row.Cells(row.Cells.Count - 1).BackColor = Drawing.Color.White

                        EscolaAnterior = RS.GetString(RS.GetOrdinal("ID_ESCOLA"))
                        row.BackColor = mvCorPautas
                        TabelaPolos.Controls.Add(row)
                    End If

                    If PermiteAgrupamento And RS.GetValue(RS.GetOrdinal("NU_SALAS_GRUPO3")) + RS.GetValue(RS.GetOrdinal("NU_SALAS_GRUPO4")) > 0 Then
                        ' Começã a mostrar as linha adicionais, para agrupamento
                        row = New TableRow()
                        ' Pula colunas
                        For k As Int16 = 1 To 1
                            Call CarregaColunasDeTextoNumaLinha(row, "", LL)
                        Next

                        ' Coluna com a data da prova
                        Dim tmp As String = Dias(RS.GetValue(RS.GetOrdinal("ID_DIA_APLICACAO")))
                        Call CarregaColunasDeTextoNumaLinha(row, tmp, LL)
                        'row.Cells(row.Cells.Count - 1).Font.Size = 12
                        row.Cells(row.Cells.Count - 1).Font.Bold = True

                        ' Prepara os parâmetros: ID_ESCOLA, ID_DIA_APLICACAO, ID_PROVA
                        Dim Valores As String = RS.GetString(RS.GetOrdinal("ID_ESCOLA")) &
                                            "," & Trim(Str(RS.GetValue(RS.GetOrdinal("ID_DIA_APLICACAO")))) &
                                            "," & RS.GetString(RS.GetOrdinal("ID_PROVA"))

                        If RS.GetValue(RS.GetOrdinal("ID_PROVA")) = 1 Then
                            Call CarregaColunaDeLinkNumaLinha(row, "Agrupar", Valores,
                                                          "Agrupar salas de Português",
                                                          "Agrupar as aplicações da prova de Português em " & tmp, mvQueTipo)
                        Else
                            Call CarregaColunaDeLinkNumaLinha(row, "Agrupar", Valores,
                                                          "Agrupar salas de Matemática",
                                                          "Agrupar As aplicações da prova de Matemática  em " & tmp, mvQueTipo)
                        End If


                        'If Pauta(C) Then row.BackColor = mvCorPautas
                        TabelaPolos.Controls.Add(row)
                    Else
                        ' Deixa uma linha em branco quando não mostrar agrupamentos
                        row = New TableRow()
                        TabelaPolos.Controls.Add(row)
                    End If
                End If

            End While

            ' Mostra o totalde linhas
            LiteralNumLinhas.Text = RecadoSobreLinhas(Linhas, C, True)
            Call AcertaPaginacao(C)
            cmbPaginas.Items.Clear()
            For kkk = 1 To System.Math.Ceiling(C / mcLimiteLinhasMostradas)
                cmbPaginas.Items.Add(Trim(Str(kkk)))
            Next
            cmbPaginas.Text = Trim(Str(mvPrimeiraPaginaMostrada))

            ' Encerra a leitura
            RS.Close()
            RS.Dispose()
            MeuComando.Dispose()
            MyC.Dispose()

        Catch ex As Exception
            OK = "Erro na leitura dos dados das escolas. " & vbCrLf & ex.Message
        End Try
        If OK <> "OK" Then MensagemERROS.Text = TextoVermelho(OK)
    End Sub


    Private Sub CarregaEscolasTURMAS()

        Dim OK As String = "OK"
        Dim row As TableRow
        Dim EscolaID As String = ""
        Dim tmpSTR As String

        Try

            Dim sSQL As String =
            "SELECT TOP " & mcLimiteLinhasLidas &
            "      x.*,p.NO_POLO," &
            "      (SELECT COUNT(*) FROM TURMA t WHERE t.ID_ESCOLA=x.ID_ESCOLA) NU_TURMAS," &
            "      (SELECT COUNT(*) FROM TURMA_SALA ts WHERE x.CO_TURMA_CENSO=ts.CO_TURMA_CENSO And ts.ID_GRUPO_SALA=1) NU_SALAS_GRUPO1," &
            "      (SELECT COUNT(*) FROM TURMA_SALA ts WHERE x.CO_TURMA_CENSO=ts.CO_TURMA_CENSO And ts.ID_GRUPO_SALA=2) NU_SALAS_GRUPO2," &
            "      (SELECT COUNT(*) FROM TURMA_SALA ts WHERE x.CO_TURMA_CENSO=ts.CO_TURMA_CENSO And ts.ID_GRUPO_SALA=3) NU_SALAS_GRUPO3," &
            "      (SELECT COUNT(*) FROM TURMA_SALA ts WHERE x.CO_TURMA_CENSO=ts.CO_TURMA_CENSO And ts.ID_GRUPO_SALA=4) NU_SALAS_GRUPO4" &
            " FROM (SELECT distinct e.NO_MUNICIPIO, e.ID_ESCOLA, e.NO_ESCOLA, e.NU_TELEFONE,e.ID_POLO,e.QT_ALUNO_ESCOLAS," &
            "                       t.CO_TURMA_CENSO,t.NO_TURMA,t.QT_ALUNO_TURMAS,t.DS_TURNO,t.TX_HR_INICIAL" &
            "       FROM ESCOLA e, TURMA t, TURMA_SALA ts" &
            "       WHERE (@SG_UF='Todos' or SG_UF=@SG_UF) And (@NO_ESCOLA='Todas' or NO_ESCOLA=@NO_ESCOLA) And (@BUSCA='' or NO_ESCOLA like '%' + @BUSCA + '%') " &
            "         And (@NO_MUNICIPIO='Todos' or NO_MUNICIPIO=@NO_MUNICIPIO)" &
            "         And EXISTS(select * from ATRIBUICAO_GESTOR a where a.CPF=@MeuCPF And (a.ID_POLO='*' or a.ID_POLO=e.ID_POLO))" &
            "         And e.ID_ESCOLA=t.ID_ESCOLA and t.CO_TURMA_CENSO=ts.CO_TURMA_CENSO and ts.ID_PROVA='1') x," &
            "      (SELECT * FROM POLO where @NO_POLO='Todos' or NO_POLO=@NO_POLO) p " &
            " WHERE p.ID_POLO=x.ID_POLO" &
            " ORDER BY x.NO_ESCOLA,x.NO_TURMA"

            'Call AbreConexao()
            Dim MyC = New System.Data.SqlClient.SqlConnection(Ligacao())
            MyC.Open()

            Dim MeuComando = New System.Data.SqlClient.SqlCommand(sSQL, MyC)
            Dim RS As System.Data.SqlClient.SqlDataReader

            MeuComando.Parameters.Add("@SG_UF", SqlDbType.VarChar).Value = cmbUF.Text
            MeuComando.Parameters.Add("@NO_MUNICIPIO", SqlDbType.VarChar).Value = cmbMunicipio.Text
            MeuComando.Parameters.Add("@NO_POLO", SqlDbType.VarChar).Value = cmbPolo.Text
            MeuComando.Parameters.Add("@NO_ESCOLA", SqlDbType.VarChar).Value = cmbEscola.Text
            MeuComando.Parameters.Add("@BUSCA", SqlDbType.VarChar).Value = CampoFiltroBusca.Text
            MeuComando.Parameters.Add("@MeuCPF", SqlDbType.VarChar).Value = Parametros(gcParametroCPF)

            MeuComando.CommandText = sSQL
            RS = MeuComando.ExecuteReader()

            ' Limpa a tabela e define a largura de cada coluna
            TabelaPolos.Controls.Clear()
            Dim LL As Int16() = {20, 20, 150, 100, 100, 20, 20, 80, 80, 20, 20, 40, 40, 20, 20, 20}

            ' Controla a mudança
            Dim EscolaAnterior As String = ""


            row = New TableRow()
            Call CarregaColunasDeTextoNumaLinha(row, "|Censo|Escola|Município|Polo|Alunos|Turmas|Cegueira /Surdez /Surdocegueira|Auxilio Ledor /Auxilio Transcrição|Baixa Visão (10)|Outras Deficiências (10)|", LL)    ' Deixa uma coluna em branco no final, para não alargar muito
            TabelaPolos.Controls.Add(row)

            ' Varre record set
            Dim C As Int32 = 0
            Dim Linhas As Int32 = 0
            While RS.Read()
                C += 1
                If C >= (mvPrimeiraPaginaMostrada - 1) * mcLimiteLinhasMostradas + 1 And Linhas < mcLimiteLinhasMostradas Then
                    Linhas += 1
                    If EscolaAnterior <> RS.GetString(RS.GetOrdinal("ID_ESCOLA")) Then

                        ' Insere uma linha fininha se não for a primeira escola
                        If EscolaAnterior <> "" Then
                            row = New TableRow()
                            row.Height = 8
                            TabelaPolos.Controls.Add(row)
                        End If
                        row = New TableRow()

                        ' Qualquer um pode visualizar
                        Call CarregaColunaDeBotaoNumaLinha(row, "Visualizar",
                                                           RS.GetString(RS.GetOrdinal("ID_ESCOLA")) & ", V",
                                                           "basic3-020_presentation_powerpoint_keynote.png",
                                                           "Clique aqui para visualizar os dados da escola", mvQueTipo)

                        tmpSTR = RS.GetString(RS.GetOrdinal("ID_ESCOLA")) + "|" +
                                   RS.GetString(RS.GetOrdinal("NO_ESCOLA")) + "|" +
                                   RS.GetString(RS.GetOrdinal("NO_MUNICIPIO")) + "|" +
                                   RS.GetString(RS.GetOrdinal("NO_POLO")) + "|" +
                                   Str(RS.GetValue(RS.GetOrdinal("QT_ALUNO_ESCOLAS"))) + "|" +
                                   Str(RS.GetValue(RS.GetOrdinal("NU_TURMAS"))) + "|" +
                                   "" + "|" +
                                   "" + "|" +
                                   "" + "|" +
                                   "" + "|"

                        ' Prepara as colunas
                        Call CarregaColunasDeTextoNumaLinha(row, tmpSTR, LL)
                        ' Prepara Java Script "javascript: __doPostBack('xxx','xxx')"
                        EscolaID = RS.GetString(RS.GetOrdinal("ID_ESCOLA"))

                        EscolaAnterior = RS.GetString(RS.GetOrdinal("ID_ESCOLA"))
                        row.BackColor = mvCorPautas
                        TabelaPolos.Controls.Add(row)

                        ' Insere um cabeçalho para as turmas
                        row = New TableRow()
                        Call CarregaColunasDeTextoNumaLinha(row, "||Turma|Turno|Horário", LL)    ' Deixa uma coluna em branco no final, para não alargar muito
                        'row.BackColor = mvCorPautas
                        TabelaPolos.Controls.Add(row)

                    End If

                    row = New TableRow()
                    '' Pula dez colunas
                    'For k As Int16 = 1 To 1
                    '    Call CarregaColunasDeTextoNumaLinha(row, "", LL)
                    'Next


                    ' Visualiza os alunos
                    If RS.GetString(RS.GetOrdinal("CO_TURMA_CENSO")) <> mvTurmaEscolhida Then
                        Call CarregaColunaDeBotaoAtivoNumaLinha(row, "Turma",
                                                                RS.GetString(RS.GetOrdinal("CO_TURMA_CENSO")),
                                                                "basic3-078_user_woman_child_family_group.png",
                                                                "Clique aqui para visualizar os alunos da turma", mvQueTipo)
                    Else
                        Call CarregaColunasDeTextoNumaLinha(row, "", LL)
                    End If

                    tmpSTR = RS.GetString(RS.GetOrdinal("CO_TURMA_CENSO")) + "|" +
                                   RS.GetString(RS.GetOrdinal("NO_TURMA")) + "|" +
                                   RS.GetString(RS.GetOrdinal("DS_TURNO")) + "|" +
                                   Str(RS.GetValue(RS.GetOrdinal("TX_HR_INICIAL"))) & "h" + "|" +
                                   Str(RS.GetValue(RS.GetOrdinal("QT_ALUNO_TURMAS"))) + "|" +
                                   "" + "|" +
                                   Str(RS.GetValue(RS.GetOrdinal("NU_SALAS_GRUPO1"))) + "|" +
                                   Str(RS.GetValue(RS.GetOrdinal("NU_SALAS_GRUPO2"))) + "|" +
                                   Str(RS.GetValue(RS.GetOrdinal("NU_SALAS_GRUPO3"))) + "|" +
                                   Str(RS.GetValue(RS.GetOrdinal("NU_SALAS_GRUPO4")))

                    ' Prepara as colunas
                    Call CarregaColunasDeTextoNumaLinha(row, tmpSTR, LL)

                    ' Insere a linha da turma na tabela
                    TabelaPolos.Controls.Add(row)

                    ' Se for a turma escolhida, drill down
                    If RS.GetString(RS.GetOrdinal("CO_TURMA_CENSO")) = mvTurmaEscolhida Then

                        ' Insere um cabeçalho para os alunos
                        row = New TableRow()
                        Call CarregaColunasDeTextoNumaLinha(row, "||Aluno|Dia1|Dia2|||Kit1|Kit2", LL)
                        'row.BorderStyle = BorderStyle.Dotted
                        row.BackColor = Drawing.Color.LightGreen
                        TabelaPolos.Controls.Add(row)

                        ' =================
                        Dim MyCT = New System.Data.SqlClient.SqlConnection(Ligacao())
                        MyCT.Open()

                        Dim MeuComandoT = New System.Data.SqlClient.SqlCommand(sSQL, MyCT)
                        Dim RST As System.Data.SqlClient.SqlDataReader

                        Dim sSQLT As String =
                            "SELECT a.CO_ALUNO_CENSO,a.NO_ALUNO," &
                           " Case When gs1.ID_GRUPO_APLIC=8 then '' else gs1.DS_GRUPO_APLIC end DS_GRUPO_APLIC1," &
                           " Case When gs2.ID_GRUPO_APLIC=8 then '' else gs2.DS_GRUPO_APLIC end DS_GRUPO_APLIC2," &
                           " Case When left(k1.DS_KIT_PROVA_ESPECIAL,11)='Prova Comum' then '' else k1.DS_KIT_PROVA_ESPECIAL end DS_KIT_PROVA_ESPECIAL1," &
                           " Case When left(k2.DS_KIT_PROVA_ESPECIAL,11)='Prova Comum' then '' else k2.DS_KIT_PROVA_ESPECIAL end DS_KIT_PROVA_ESPECIAL2" &
                           " FROM ALUNO a, ATENDIMENTO at1, GRUPO_APLIC gs1, ATENDIMENTO at2, GRUPO_APLIC gs2, KIT k1, KIT k2" &
                           " Where a.CO_TURMA_CENSO ='" & mvTurmaEscolhida & " ' and a.NO_ALUNO <> ''" &
                           " And a.ID_ATENDIMENTO_DIA1=at1.ID and at1.ID_GRUPO_APLIC=gs1.ID_GRUPO_APLIC" &
                           " And a.ID_ATENDIMENTO_DIA2=at2.ID and at2.ID_GRUPO_APLIC=gs2.ID_GRUPO_APLIC" &
                           " And at1.ID_KIT=k1.ID_KIT_PROVA_ESPECIAL" &
                           " And at2.ID_KIT=k2.ID_KIT_PROVA_ESPECIAL" &
                           " ORDER BY a.NO_ALUNO"

                        MeuComandoT.CommandText = sSQLT
                        RST = MeuComandoT.ExecuteReader()

                        While RST.Read()
                            ' Monta alinha de um aluno
                            row = New TableRow()
                            ' Pula colunas
                            For k As Int16 = 1 To 1
                                Call CarregaColunasDeTextoNumaLinha(row, "", LL)
                            Next
                            tmpSTR = RST.GetString(RST.GetOrdinal("CO_ALUNO_CENSO")) + "|" +
                                     RST.GetString(RST.GetOrdinal("NO_ALUNO")) + "|" +
                                     RST.GetString(RST.GetOrdinal("DS_GRUPO_APLIC1")) + "|" +
                                     RST.GetString(RST.GetOrdinal("DS_GRUPO_APLIC2")) + "|" +
                                     "" + "|" +
                                     "" + "|" +
                                     RST.GetString(RST.GetOrdinal("DS_KIT_PROVA_ESPECIAL1")) + "|" +
                                     RST.GetString(RST.GetOrdinal("DS_KIT_PROVA_ESPECIAL2"))

                            ' Prepara as colunas
                            Call CarregaColunasDeTextoNumaLinha(row, tmpSTR, LL)
                            TabelaPolos.Controls.Add(row)
                        End While

                        ' Encerra a leitura
                        RST.Close()
                        RST.Dispose()
                        MeuComandoT.Dispose()
                        MyCT.Dispose()
                        ' ==================
                        ' Insere um rodapé para os alunos
                        row = New TableRow()
                        Call CarregaColunasDeTextoNumaLinha(row, "||||||||", LL)
                        row.BackColor = Drawing.Color.LightGreen
                        row.Height = 6
                        TabelaPolos.Controls.Add(row)

                    End If

                End If

            End While

            ' Mostra o total de linhas
            LiteralNumLinhas.Text = RecadoSobreLinhas(Linhas, C, True)
            Call AcertaPaginacao(C)
            cmbPaginas.Items.Clear()
            For kkk = 1 To System.Math.Ceiling(C / mcLimiteLinhasMostradas)
                cmbPaginas.Items.Add(Trim(Str(kkk)))
            Next
            cmbPaginas.Text = Trim(Str(mvPrimeiraPaginaMostrada))

            ' Encerra a leitura
            RS.Close()
            RS.Dispose()
            MeuComando.Dispose()
            MyC.Dispose()

        Catch ex As Exception
            OK = "Erro na leitura dos dados das escolas. " & vbCrLf & ex.Message
        End Try
        If OK <> "OK" Then MensagemERROS.Text = TextoVermelho(OK)
    End Sub

    Private Sub CarregaTurmaRegistro()

        Dim OK As String = "OK"
        Dim row As TableRow
        Dim CodTUrma As String

        Try

            Dim sSQL As String =
            "Select TOP " & mcLimiteLinhasLidas &
                  " x.ID_ESCOLA, x.NO_ESCOLA, " &
                  " p.NO_POLO, t.NO_TURMA, t.CO_TURMA_CENSO, ta.ID_PROVA, ta.NO_STATUS, ta.TX_MOTIVO_NAO_APLICACAO" &
           " FROM (Select * FROM ESCOLA e WHERE (@SG_UF='Todos' or SG_UF=@SG_UF) And (@BUSCA='' or NO_ESCOLA like '%' + @BUSCA + '%')) x" &
                " INNER JOIN (select * FROM POLO where @NO_POLO='Todos' or NO_POLO=@NO_POLO)p on x.ID_POLO = p.ID_POLO" &
                " INNER JOIN TURMA t on x.ID_ESCOLA=t.ID_ESCOLA" &
                " INNER JOIN TURMA_APLICACAO ta on t.CO_TURMA_CENSO=ta.CO_TURMA_CENSO" &
           " WHERE" &
                " EXISTS(select * from ATRIBUICAO_GESTOR a where a.CPF=@MeuCPF and (a.ID_POLO='*' or a.ID_POLO=p.ID_POLO))" &
           " ORDER BY x.NO_ESCOLA,t.NO_TURMA,ta.ID_PROVA"

            'Call AbreConexao()
            Dim MyC = New System.Data.SqlClient.SqlConnection(Ligacao())
            MyC.Open()

            Dim MeuComando = New System.Data.SqlClient.SqlCommand(sSQL, MyC)
            Dim RS As System.Data.SqlClient.SqlDataReader

            MeuComando.Parameters.Add("@SG_UF", SqlDbType.VarChar).Value = cmbUF.Text
            MeuComando.Parameters.Add("@NO_MUNICIPIO", SqlDbType.VarChar).Value = cmbMunicipio.Text
            MeuComando.Parameters.Add("@NO_POLO", SqlDbType.VarChar).Value = cmbPolo.Text
            MeuComando.Parameters.Add("@BUSCA", SqlDbType.VarChar).Value = CampoFiltroBusca.Text
            MeuComando.Parameters.Add("@MeuCPF", SqlDbType.VarChar).Value = Parametros(gcParametroCPF)

            MeuComando.CommandText = sSQL
            RS = MeuComando.ExecuteReader()

            ' Limpa a tabela e define a largura de cada coluna
            TabelaPolos.Controls.Clear()
            Dim LL As Int16() = {80, 20, 90, 60, 20, 20, 50, 80, 20, 20, 20, 20}

            row = New TableRow()
            Call CarregaColunasDeTextoNumaLinha(row, "Polo|Cod Escola|Escola|Turma|CodTurma|Dia|Status|Motivo|Registrar", LL)    ' Deixa uma coluna em branco no final, para não alargar muito
            TabelaPolos.Controls.Add(row)

            ' Varre record set
            Dim C As Int32 = 0
            Dim Linhas As Int32 = 0
            While RS.Read()
                C += 1
                If C >= (mvPrimeiraPaginaMostrada - 1) * mcLimiteLinhasMostradas + 1 And Linhas < mcLimiteLinhasMostradas Then
                    Linhas += 1
                    row = New TableRow()
                    Dim tmpSTR As String = RS.GetString(RS.GetOrdinal("NO_POLO")) + "|" +
                                           RS.GetString(RS.GetOrdinal("ID_ESCOLA")) + "|" +
                                           RS.GetString(RS.GetOrdinal("NO_ESCOLA")) + "|" +
                                           RS.GetString(RS.GetOrdinal("NO_TURMA")) + "|" +
                                           RS.GetString(RS.GetOrdinal("CO_TURMA_CENSO")) + "|" +
                                           RS.GetString(RS.GetOrdinal("ID_PROVA")) + "|" +
                                           RS.GetString(RS.GetOrdinal("NO_STATUS")) + "|" +
                                           RS.GetString(RS.GetOrdinal("TX_MOTIVO_NAO_APLICACAO")) + "|"
                    ' Prepara as colunas
                    Call CarregaColunasDeTextoNumaLinha(row, tmpSTR, LL)
                    ' Prepara Java Script "javascript: __doPostBack('xxx','xxx')"
                    CodTUrma = RS.GetString(RS.GetOrdinal("CO_TURMA_CENSO"))

                    ' Posiciona os botões
                    Call CarregaColunaDeBotaoNumaLinha(row, "Registrar", CodTUrma &
                                                       "|" & RS.GetString(RS.GetOrdinal("ID_PROVA")) &
                                                       "|" & RS.GetString(RS.GetOrdinal("NO_TURMA")),
                                                       "basic1-025_book_reading.png",
                                                       "Clique aqui para registrar as aplicações da escola", mvQueTipo)
                    'Call CarregaColunasDeTextoNumaLinha(row, "", LL)
                    ' ------------------------------------

                    If Pauta(C) Then row.BackColor = mvCorPautas
                    TabelaPolos.Controls.Add(row)
                End If
            End While

            ' Mostra o totalde linhas
            LiteralNumLinhas.Text = RecadoSobreLinhas(Linhas, C, True)
            Call AcertaPaginacao(C)
            cmbPaginas.Items.Clear()
            For kkk = 1 To System.Math.Ceiling(C / mcLimiteLinhasMostradas)
                cmbPaginas.Items.Add(Trim(Str(kkk)))
            Next
            cmbPaginas.Text = Trim(Str(mvPrimeiraPaginaMostrada))

            ' Encerra a leitura
            RS.Close()
            RS.Dispose()
            MeuComando.Dispose()
            MyC.Dispose()

        Catch ex As Exception
            OK = "Erro na leitura dos dados das escolas. " & vbCrLf & ex.Message
        End Try
        If OK <> "OK" Then MensagemERROS.Text = TextoVermelho(OK)
    End Sub


    Private Sub AcertaPaginacao(ByRef QuantasLinhasLidas As Int32)
        cmbPaginas.Items.Clear()
        For kkk = 1 To System.Math.Ceiling(QuantasLinhasLidas / mcLimiteLinhasMostradas)
            cmbPaginas.Items.Add(Trim(Str(kkk)))
        Next
        cmbPaginas.Text = Trim(Str(mvPrimeiraPaginaMostrada))
    End Sub
    Private Sub CarregaPolosPendencias()

        Dim OK As String = "OK"
        Dim row As TableRow
        Dim PoloID As String

        Try

            Dim sSQL As String =
            "SELECT TOP " & mcLimiteLinhasLidas & " p.ID_POLO, m.SG_UF + ' - ' + m.NO_MUNICIPIO UF_MUNIC,p.NO_POLO," &
            "p.NATUREZA_LOCAL,p.STATUS_NEGOCIACAO,p.NATUREZA_LOCAL,p.CO_DISTRIBUIDORA," &
                "Case when (p.NU_CEP = '' or p.DS_ENDERECO = '' or p.NO_POLO = '') then 'S' else 'N' end INCOMPLETO," &
                   " isnull(pp0.CPF,'') COORDENADOR_CPF,isnull(pp0.NO_PESSOA,'') NO_COORDENADOR," &
                   " isnull(pp1.CPF,'') APOIO_LOGISTICO_CPF,isnull(pp1.NO_PESSOA,'') NO_APOIO_LOGISTICO," &
                   " isnull(pp2.CPF,'') SUBCOORDENADOR_CPF,isnull(pp2.NO_PESSOA,'') NO_SUBCOORDENADOR," &
                   " isnull(pe.NO_POLO,'') NO_POLO_ENTREGA," &
                   "(SELECT COUNT(*) FROM ESCOLA e WHERE p.ID_POLO=e.ID_POLO) NU_ESCOLAS," &
                   "(SELECT COUNT(*) FROM TURMA t, ESCOLA e WHERE t.ID_ESCOLA=e.ID_ESCOLA And p.ID_POLO=e.ID_POLO) NU_TURMAS" &
          " FROM (SELECT * FROM POLO WHERE" &
                " (@SG_UF='Todos' or @SG_UF='BR' or SG_UF=@SG_UF)" &
                " And (@NO_MUNICIPIO='Todos' or NO_MUNICIPIO=@NO_MUNICIPIO)" &
                " And (@BUSCA='' or NO_POLO like '%' + @BUSCA + '%')" &
                " And EXISTS(select * from ATRIBUICAO_GESTOR a where a.CPF=@MeuCPF and (a.ID_POLO='*' or a.ID_POLO=POLO.ID_POLO)))p" &
                " LEFT JOIN MUNICIPIO m on p.CO_MUNICIPIO= m.CO_MUNICIPIO" &
                " Left Join(SELECT p.NO_PESSOA, p.CPF, a.ID_POLO" &
                                                " From PESSOAL p, ATRIBUICAO_GESTOR a  Where p.CPF = a.CPF And a.NO_FUNCAO ='Coordenador de Polo')pp0 on p.ID_POLO=pp0.ID_POLO" &
                " Left Join (Select p.NO_PESSOA, p.CPF, a.ID_POLO" &
                                                   " From PESSOAL p, ATRIBUICAO_GESTOR a  Where p.CPF = a.CPF And a.NO_FUNCAO ='Apoio Logístico')pp1 on p.ID_POLO=pp1.ID_POLO" &
                " Left Join (Select p.NO_PESSOA, p.CPF, a.ID_POLO" &
                                                   " From PESSOAL p, ATRIBUICAO_GESTOR a  Where p.CPF = a.CPF And a.NO_FUNCAO ='Subcoordenador Estadual')pp2 on p.ID_POLO=pp2.ID_POLO" &
                " Left Join POLO pe on p.ID_POLO_ENTREGA=pe.ID_POLO" &
          " ORDER BY 2, 3"

            'Call AbreConexao()
            Dim MyC = New System.Data.SqlClient.SqlConnection(Ligacao())
            MyC.Open()

            Dim MeuComando = New System.Data.SqlClient.SqlCommand(sSQL, MyC)
            Dim RS As System.Data.SqlClient.SqlDataReader

            MeuComando.Parameters.Add("@SG_UF", SqlDbType.VarChar).Value = cmbUF.Text
            MeuComando.Parameters.Add("@NO_MUNICIPIO", SqlDbType.VarChar).Value = cmbMunicipio.Text
            MeuComando.Parameters.Add("@BUSCA", SqlDbType.VarChar).Value = CampoFiltroBusca.Text
            MeuComando.Parameters.Add("@MeuCPF", SqlDbType.VarChar).Value = Parametros(gcParametroCPF)

            MeuComando.CommandText = sSQL
            RS = MeuComando.ExecuteReader

            ' Limpa a tabela e define a largura de cada coluna
            TabelaPolos.Controls.Clear()
            Dim LL As Int16() = {90, 100, 30, 30, 30, 30, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20}

            row = New TableRow()
            Call CarregaColunasDeTextoNumaLinha(row, "POLO|UF - MUNICÍPIO|Sede|Negociação|Coordenador|Subcoordenador|Apoio Logístico|Dados incompletos|Correio|Escolas|Turmas|Visualização|Edição|Orçamento|Locação|$ Capacitação|Instrumento de Avaliação|Remoção", LL)
            TabelaPolos.Controls.Add(row)

            ' Varre record set
            Dim C As Int32 = 0
            Dim Linhas As Int16 = 0
            While RS.Read()
                C += 1
                If Linhas < mcLimiteLinhasMostradas + 1000 Then
                    Linhas += 1
                    row = New TableRow()
                    Dim tmpSTR As String = ""

                    tmpSTR += RS.GetString(RS.GetOrdinal("NO_POLO"))
                    tmpSTR += "|" + RS.GetString(RS.GetOrdinal("UF_MUNIC"))
                    tmpSTR += "|" + RS.GetString(RS.GetOrdinal("NATUREZA_LOCAL"))

                    If RS.GetString(RS.GetOrdinal("STATUS_NEGOCIACAO")) <> "Concluída" Then
                        tmpSTR += "|" + RS.GetString(RS.GetOrdinal("STATUS_NEGOCIACAO"))
                    Else
                        tmpSTR += "|"
                    End If

                    If RS.GetString(RS.GetOrdinal("NO_COORDENADOR")) = "" Then
                        tmpSTR += "|" + "Não"
                    Else
                        tmpSTR += "|"
                    End If

                    If RS.GetString(RS.GetOrdinal("NO_SUBCOORDENADOR")) = "" Then
                        tmpSTR += "|" + "Não"
                    Else
                        tmpSTR += "|"
                    End If

                    If RS.GetString(RS.GetOrdinal("NO_APOIO_LOGISTICO")) = "" Then
                        tmpSTR += "|" + "Não"
                    Else
                        tmpSTR += "|"
                    End If

                    If RS.GetString(RS.GetOrdinal("INCOMPLETO")) = "S" Then
                        tmpSTR += "|" + "Sim"
                    Else
                        tmpSTR += "|"
                    End If

                    If RS.IsDBNull(RS.GetOrdinal("CO_DISTRIBUIDORA")) Then
                        tmpSTR += "|" + "Ausente"
                    Else
                        tmpSTR += "|"
                    End If

                    If RS.GetValue(RS.GetOrdinal("NU_ESCOLAS")) > 15 Then
                        tmpSTR += "|" + Str(RS.GetValue(RS.GetOrdinal("NU_ESCOLAS")))
                    ElseIf RS.GetValue(RS.GetOrdinal("NU_ESCOLAS")) < 3 Then
                        tmpSTR += "|" + Str(RS.GetValue(RS.GetOrdinal("NU_ESCOLAS")))
                    Else
                        tmpSTR += "|"
                    End If

                    If RS.GetValue(RS.GetOrdinal("NU_TURMAS")) > 50 Then
                        tmpSTR += "|" + Str(RS.GetValue(RS.GetOrdinal("NU_TURMAS")))
                    ElseIf RS.GetValue(RS.GetOrdinal("NU_TURMAS")) < 5 Then
                        tmpSTR += "|" + Str(RS.GetValue(RS.GetOrdinal("NU_TURMAS")))
                    Else
                        tmpSTR += "|"
                    End If

                    ' Prepara as colunas
                    Call CarregaColunasDeTextoNumaLinha(row, tmpSTR, LL)

                    ' Prepara para o Java Script "javascript: __doPostBack('xxx','xxx')"
                    PoloID = RS.GetString(RS.GetOrdinal("ID_POLO"))

                    ' Posiciona os botões
                    ' Qualquer um pode visualizar
                    Call CarregaColunaDeBotaoNumaLinha(row, "Visualizar", PoloID & ",V", "basic3-020_presentation_powerpoint_keynote.png",
                                                           "Clique aqui para visualizar os dados do polo", mvQueTipo)

                    ' Somente coordenadores estaduais e gestores do polo podem editar
                    If Parametros(gcParametroFuncao) = "Coordenador Estadual" _
                        Or Parametros(gcParametroFuncao) = "Administrador" _
                        Or RS.GetString(RS.GetOrdinal("COORDENADOR_CPF")) = Parametros(gcParametroCPF) _
                        Or RS.GetString(RS.GetOrdinal("SUBCOORDENADOR_CPF")) = Parametros(gcParametroCPF) _
                        Or RS.GetString(RS.GetOrdinal("APOIO_LOGISTICO_CPF")) = Parametros(gcParametroCPF) Then
                        Call CarregaColunaDeBotaoNumaLinha(row, "Editar", PoloID & ",U", "basic1-002_write_pencil_new_edit.png",
                                                           "Editar os dados do polo", mvQueTipo)
                        Call CarregaColunaDeBotaoNumaLinha(row, "Orçar", PoloID & ",U", "basic2-162_money_coin_dollar.png",
                                                           "Clique aqui para editar o orçamento do polo", mvQueTipo)
                        Call CarregaColunaDeBotaoNumaLinha(row, "Alugar", PoloID & ",U", "basic2-158_home_house.png",
                                   "Clique aqui para orçar despesas e aluguel do polo", mvQueTipo)
                        Call CarregaColunaDeBotaoNumaLinha(row, "Capacitar", PoloID & ",U", "basic2-156_award_achievement_star.png",
                                   "Clique aqui para orçar despesas de capacitação nos polos", mvQueTipo)
                    Else
                        Call CarregaColunasDeTextoNumaLinha(row, "", LL)
                        Call CarregaColunasDeTextoNumaLinha(row, "", LL)
                        Call CarregaColunasDeTextoNumaLinha(row, "", LL)
                        Call CarregaColunasDeTextoNumaLinha(row, "", LL)
                    End If

                    '' Somente coordenadores do polo e superiores podem avaliar
                    'If Parametros(gcParametroFuncao) = "Coordenador Estadual" _
                    '    Or Parametros(gcParametroFuncao) = "Administrador" _
                    '    Or RS.GetString(RS.GetOrdinal("COORDENADOR_CPF")) = Parametros(gcParametroCPF) _
                    '    Or RS.GetString(RS.GetOrdinal("SUBCOORDENADOR_CPF")) = Parametros(gcParametroCPF) Then
                    '    Call CarregaColunaDeBotaoNumaLinha(row, "Upload",
                    '                                       PoloID & "," + RS.GetString(RS.GetOrdinal("NO_POLO")),
                    '                                       "basic2-005_bubble_cloud.png",
                    '                                       "Editar os dados do polo", mvQueTipo)
                    'Else
                    Call CarregaColunasDeTextoNumaLinha(row, "", LL)
                    'End If

                    ' Somente coordenadores estaduais podem remover
                    If (Parametros(gcParametroFuncao) = "Coordenador Estadual" _
                           Or Parametros(gcParametroFuncao) = "Administrador") _
                       And RS.GetValue(RS.GetOrdinal("NU_ESCOLAS")) = 0 Then
                        Call CarregaColunaDeBotaoNumaLinha(row, "Remover", PoloID, "basic1-020_bin_trash_delete.png",
                                                           "Clique aqui para remover este polo", mvQueTipo)
                    Else
                        Call CarregaColunasDeTextoNumaLinha(row, "", LL)
                    End If

                    If Pauta(C) Then row.BackColor = mvCorPautas

                    TabelaPolos.Controls.Add(row)
                End If
            End While

            ' Mostra o total de linhas
            LiteralNumLinhas.Text = RecadoSobreLinhas(Linhas, C, False)
            If CaixaRemocao.Text <> "" Then
                MensagemERROS.Text += TextoVermelho("               Clique remover novamente para confirmar a operação. ")
            End If

            ' Encerra a leitura
            RS.Close()
            RS.Dispose()
            MeuComando.Dispose()
            MyC.Dispose()

        Catch ex As Exception
            OK = "Erro na leitura dos dados de polos." & vbCrLf & ex.Message

        End Try

        If OK <> "OK" Then MensagemERROS.Text = TextoVermelho(OK)

    End Sub


    Private Sub CarregaColaboradoresTriagem()

        Dim OK As String = "OK"
        Dim row As TableRow
        Dim PessoaID As String

        Try
            Dim Juncao1 As String = IIf(cmbMunicipio.Text = "Todos", " LEFT JOIN ", " INNER JOIN ")
            'Dim Juncao2 As String = IIf(cmbPolo.Text = "Todos", " LEFT JOIN ", " INNER JOIN ")

            Dim sSQL As String =
            "SELECT TOP " & mcLimiteLinhasLidas & " x.SG_UF_ENDERECO + ' - ' + isnull(m.NO_MUNICIPIO,'') UF_MUNIC,x.CPF,x.NO_PESSOA,x.SG_UF_ALOC,NO_STATUS,NO_CATEGORIA," &
            "case when exists (SELECT * FROM ATRIBUICAO_GESTOR a where a.CPF=x.CPF) then 'S' else 'N' end OCUPA_FUNCAO," &
            "case when exists (SELECT * FROM ATRIBUICAO_POLO ap, ATRIBUICAO_GESTOR ag where ap.CPF=x.CPF and ag.CPF=@MeuCPF and ap.ID_POLO=ag.ID_POLO) then 'S' else 'N' end ASSOCIADA_POLO," &
            "(SELECT COUNT(*) FROM ATRIBUICAO_POLO ap where ap.CPF=x.CPF) NU_POLOS," &
            "(SELECT NO_FUNCAO_FGV FROM APLICADORES_ESPECIALIZADOS ae where ae.CPF=x.CPF) CADASTRO_INEP" &
           " FROM (SELECT * FROM PESSOAL p WHERE" &
                " (@SG_UF_ALOC='Todos' or @SG_UF_ALOC='BR' or SG_UF_ALOC=@SG_UF_ALOC)" &
                " And (@NO_CATEGORIA='Todas' or NO_CATEGORIA=@NO_CATEGORIA)" &
                " And (@NO_STATUS='Todos' or NO_STATUS=@NO_STATUS)" &
                " And (@BUSCA='' or NO_PESSOA like '%' + @BUSCA + '%' or CPF like '%' + @BUSCA + '%')" &
                " )x" &
                Juncao1 & "(select * FROM MUNICIPIO where @NO_MUNICIPIO='Todos' or NO_MUNICIPIO=@NO_MUNICIPIO)m on x.CO_MUNICIPIO= m.CO_MUNICIPIO" &
           " ORDER BY 1,3"

            'Call AbreConexao()
            Dim MyC = New System.Data.SqlClient.SqlConnection(Ligacao())
            MyC.Open()

            Dim MeuComando = New System.Data.SqlClient.SqlCommand(sSQL, MyC)
            Dim RS As System.Data.SqlClient.SqlDataReader

            MeuComando.Parameters.Add("@SG_UF_ALOC", SqlDbType.VarChar).Value = cmbUF.Text
            MeuComando.Parameters.Add("@NO_MUNICIPIO", SqlDbType.VarChar).Value = cmbMunicipio.Text
            MeuComando.Parameters.Add("@MeuCPF", SqlDbType.VarChar).Value = Parametros(gcParametroCPF)
            MeuComando.Parameters.Add("@NO_CATEGORIA", SqlDbType.VarChar).Value = cmbCategoria.Text
            MeuComando.Parameters.Add("@NO_STATUS", SqlDbType.VarChar).Value = cmbStatus.Text
            MeuComando.Parameters.Add("@BUSCA", SqlDbType.VarChar).Value = CampoFiltroBusca.Text

            MeuComando.CommandText = sSQL
            RS = MeuComando.ExecuteReader()

            ' Limpa a tabela e define a largura de cada coluna
            TabelaPolos.Controls.Clear()
            Dim LL As Int16() = {50, 100, 80, 30, 30, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20}

            row = New TableRow()
            Call CarregaColunasDeTextoNumaLinha(row, "CPF|Nome|UF - Município|Status|Categoria|Tem cargo?|Número de polos|Associado a algum polo meu?|Cadastro INEP?|Visualização|Aceitação|Atribuição|Associação a polo|Reserva", LL)    ' Deixa uma coluna em branco no final, para não alargar muito
            TabelaPolos.Controls.Add(row)

            ' Varre record set
            Dim C As Int32 = 0
            Dim Linhas As Int16 = 0
            While RS.Read()
                C += 1
                If C >= (mvPrimeiraPaginaMostrada - 1) * mcLimiteLinhasMostradas + 1 And Linhas < mcLimiteLinhasMostradas Then
                    Linhas += 1
                    row = New TableRow()
                    Dim tmpSTR As String = RS.GetString(RS.GetOrdinal("CPF")) + "|" +
                                   RS.GetString(RS.GetOrdinal("NO_PESSOA")) + "|" +
                                   RS.GetString(RS.GetOrdinal("UF_MUNIC")) + "|" +
                                   RS.GetString(RS.GetOrdinal("NO_STATUS")) + "|" +
                                   RS.GetString(RS.GetOrdinal("NO_CATEGORIA")) + "|" +
                                   RS.GetString(RS.GetOrdinal("OCUPA_FUNCAO")) + "|" +
                                   Str(RS.GetValue(RS.GetOrdinal("NU_POLOS"))) + "|"
                    If (Parametros(gcParametroFuncao) = "Administrador" _
                        Or Parametros(gcParametroFuncao) = "Coordenador Estadual") _
                        Or Parametros(gcParametroFuncao) = "Coordenador Estadual" Then
                        tmpSTR &= "?"
                    Else
                        tmpSTR &= RS.GetString(RS.GetOrdinal("ASSOCIADA_POLO"))
                    End If

                    ' Prepara as colunas
                    Call CarregaColunasDeTextoNumaLinha(row, tmpSTR, LL)

                    ' Indica se pertence ao ocadastro INEP
                    If RS.IsDBNull(RS.GetOrdinal("CADASTRO_INEP")) Then
                        Call CarregaColunasDeTextoNumaLinha(row, "", LL)
                    Else
                        Call CarregaColunasDeTextoNumaLinha(row, RS.GetString(RS.GetOrdinal("CADASTRO_INEP")), LL)
                    End If

                    ' Prepara Java Script "javascript: __doPostBack('xxx','xxx')"
                    PessoaID = RS.GetString(RS.GetOrdinal("CPF"))

                    ' Posiciona os botões
                    Call CarregaColunaDeBotaoNumaLinha(row, "Visualizar", PessoaID & ",V", "basic3-020_presentation_powerpoint_keynote.png",
                                                       "Clique aqui para visualizar o cadastro", mvQueTipo)


                    If RS.GetString(RS.GetOrdinal("NO_STATUS")) <> "Apto" Then
                        Call CarregaColunaDeBotaoNumaLinha(row, "Aceitar", PessoaID, "basic1-179_check_yes.png",
                                                           "Tornar apto", mvQueTipo)
                    Else
                        Call CarregaColunasDeTextoNumaLinha(row, "", LL)
                    End If

                    If RS.GetString(RS.GetOrdinal("OCUPA_FUNCAO")) = "N" And RS.GetString(RS.GetOrdinal("ASSOCIADA_POLO")) = "N" _
                        And RS.GetString(RS.GetOrdinal("NO_STATUS")) = "Apto" Then
                        Call CarregaColunaDeBotaoNumaLinha(row, "PromoverDemover", PessoaID, "basic1-084_rotate_sync.png",
                                                   "Clique aqui para mudar categoria: gestor <-> aplicador", mvQueTipo)
                    Else
                        Call CarregaColunasDeTextoNumaLinha(row, "", LL)
                    End If


                    If RS.GetString(RS.GetOrdinal("NO_STATUS")) = "Apto" And RS.GetString(RS.GetOrdinal("NO_CATEGORIA")) = "Aplicador" Then
                        Call CarregaColunaDeBotaoNumaLinha(row, "Associar",
                                                           RS.GetString(RS.GetOrdinal("CPF")) &
                                                            "," & RS.GetString(RS.GetOrdinal("NO_PESSOA")) &
                                                            "," & RS.GetString(RS.GetOrdinal("SG_UF_ALOC")), "basic2-158_home_house.png",
                                                           "Associar um colaborador a um polo visando alocaçao para aplicações", mvQueTipo)
                    Else
                        Call CarregaColunasDeTextoNumaLinha(row, "", LL)
                    End If

                    If RS.GetString(RS.GetOrdinal("OCUPA_FUNCAO")) = "N" And RS.GetString(RS.GetOrdinal("ASSOCIADA_POLO")) = "N" _
                        And Parametros(gcParametroFuncao) <> "Observador" Then
                        If RS.GetString(RS.GetOrdinal("NO_STATUS")) = "Pendente" Or RS.GetString(RS.GetOrdinal("NO_STATUS")) = "Apto" Then
                            Call CarregaColunaDeBotaoNumaLinha(row, "Reservar", PessoaID, "basic3-037_relax_sun_bathing_coast_sea_holidays.png",
                                               "Colocar na reserva", mvQueTipo)
                        Else
                            Call CarregaColunasDeTextoNumaLinha(row, "", LL)
                        End If
                    Else
                        Call CarregaColunasDeTextoNumaLinha(row, "", LL)
                    End If

                    ' O administrador e coordenadores estaduais podem remover cadastrados
                    If RS.GetString(RS.GetOrdinal("OCUPA_FUNCAO")) = "N" _
                    And (Parametros(gcParametroFuncao) = "Administrador" Or Parametros(gcParametroFuncao) = "Coordenador Estadual") _
                    And RS.GetString(RS.GetOrdinal("NO_STATUS")) = "Pendente" And RS.GetString(RS.GetOrdinal("NO_CATEGORIA")) = "Aplicador" Then
                        Call CarregaColunaDeBotaoNumaLinha(row, "Remover", PessoaID, "basic1-020_bin_trash_delete.png",
                                                           "Remover do cadastro", mvQueTipo)
                    Else
                        Call CarregaColunasDeTextoNumaLinha(row, "", LL)
                    End If

                    ' O administrador e coordenadores estaduais podem resetar senhas
                    If Parametros(gcParametroFuncao) = "Administrador" Then
                        Call CarregaColunaDeBotaoCONFIRMANDONumaLinha(row, "ResetarSenha", PessoaID, "ChavePequena.png",
                                                                      "Resetar a senha para convidado", mvQueTipo,
                                                                      "Tem certeza de que deseja resetar a senha de " &
                                                                      RS.GetString(RS.GetOrdinal("NO_PESSOA")) & "?")
                    Else
                        Call CarregaColunasDeTextoNumaLinha(row, "", LL)
                    End If

                    If Pauta(C) Then row.BackColor = mvCorPautas
                    TabelaPolos.Controls.Add(row)
                End If
            End While

            ' Mostra o totalde linhas
            LiteralNumLinhas.Text = RecadoSobreLinhas(Linhas, C, True)
            Call AcertaPaginacao(C)

            ' Encerra a leitura
            RS.Close()
            RS.Dispose()
            MeuComando.Dispose()
            MyC.Dispose()

        Catch ex As Exception
            OK = "Erro na leitura dos dados dos colaboradores. " & vbCrLf & ex.Message
        End Try
        If OK <> "OK" Then MensagemERROS.Text = TextoVermelho(OK)
    End Sub


    Private Sub CarregaESPECIALIZADOS()

        Dim OK As String = "OK"
        Dim row As TableRow

        Try

            Dim sSQL As String =
            "SELECT TOP " & mcLimiteLinhasLidas & " ae.*," &
            "case when exists (SELECT * FROM PESSOAL p where ae.CPF=p.CPF) then 'Sim' else 'Não' end JA_CADASTRADO," &
            "case when exists (SELECT * FROM ATRIBUICAO_POLO a where a.CPF=ae.CPF) then 'Sim' else 'Não' end JA_ASSOCIADO" &
           " FROM (SELECT * FROM APLICADORES_ESPECIALIZADOS WHERE" &
                " (@SG_UF='Todos' or SG_UF=@SG_UF)" &
                " And (@BUSCA='' or NOME like '%' + @BUSCA + '%' or CPF like '%' + @BUSCA + '%')" &
                " )ae" &
           " ORDER BY 1,3"

            'Call AbreConexao()
            Dim MyC = New System.Data.SqlClient.SqlConnection(Ligacao())
            MyC.Open()

            Dim MeuComando = New System.Data.SqlClient.SqlCommand(sSQL, MyC)
            Dim RS As System.Data.SqlClient.SqlDataReader

            MeuComando.Parameters.Add("@SG_UF", SqlDbType.VarChar).Value = cmbUF.Text
            MeuComando.Parameters.Add("@BUSCA", SqlDbType.VarChar).Value = CampoFiltroBusca.Text

            MeuComando.CommandText = sSQL
            RS = MeuComando.ExecuteReader()

            ' Limpa a tabela e define a largura de cada coluna
            TabelaPolos.Controls.Clear()
            Dim LL As Int16() = {20, 30, 120, 70, 70, 40, 40, 40, 20, 20, 20, 20, 20, 20, 20, 20, 20}

            row = New TableRow()
            Call CarregaColunasDeTextoNumaLinha(row, "|CPF|Nome|e-mail|Especialidade, como definida no cadastro|Tem cadastro FGV?|Tem polo associado?|", LL)    ' Deixa uma coluna em branco no final, para não alargar muito
            TabelaPolos.Controls.Add(row)

            ' Varre record set
            Dim C As Int32 = 0
            Dim Linhas As Int16 = 0
            While RS.Read()
                C += 1
                If C >= (mvPrimeiraPaginaMostrada - 1) * mcLimiteLinhasMostradas + 1 And Linhas < mcLimiteLinhasMostradas Then
                    Linhas += 1
                    row = New TableRow()

                    ' Posiciona os botões
                    If RS.GetString(RS.GetOrdinal("JA_CADASTRADO")) = "Sim" Then
                        Call CarregaColunaDeBotaoNumaLinha(row, "Visualizar",
                                                           RS.GetString(RS.GetOrdinal("CPF")) & ",V",
                                                           "basic3-020_presentation_powerpoint_keynote.png",
                                                           "Clique aqui para visualizar o cadastro", mvQueTipo)
                    Else
                        Call CarregaColunasDeTextoNumaLinha(row, "", LL)
                    End If

                    Dim tmpSTR As String = RS.GetString(RS.GetOrdinal("CPF")) + "|" +
                                   RS.GetString(RS.GetOrdinal("NOME")) + "|" +
                                   RS.GetString(RS.GetOrdinal("TX_EMAIL")) + "|" +
                                   RS.GetString(RS.GetOrdinal("NO_FUNCAO")) + "|" +
                                   RS.GetString(RS.GetOrdinal("JA_CADASTRADO")) + "|" +
                                   RS.GetString(RS.GetOrdinal("JA_ASSOCIADO"))
                    ' Prepara as colunas
                    Call CarregaColunasDeTextoNumaLinha(row, tmpSTR, LL)

                    '' Prepara Java Script "javascript: __doPostBack('xxx','xxx')"
                    'PessoaID = RS.GetString(RS.GetOrdinal("CPF"))

                    If Pauta(C) Then row.BackColor = mvCorPautas
                    TabelaPolos.Controls.Add(row)
                End If
            End While

            ' Mostra o totalde linhas
            LiteralNumLinhas.Text = RecadoSobreLinhas(Linhas, C, True)
            Call AcertaPaginacao(C)

            ' Encerra a leitura
            RS.Close()
            RS.Dispose()
            MeuComando.Dispose()
            MyC.Dispose()

        Catch ex As Exception
            OK = "Erro na leitura dos dados dos especializados. " & vbCrLf & ex.Message
        End Try
        If OK <> "OK" Then MensagemERROS.Text = TextoVermelho(OK)
    End Sub


    Protected Sub CarregaDADOS()
        Select Case mvQueTipo
            Case "polo"
                Call CarregaPolos()
            Case "polocolaborador"
                Call CarregaPoloAssociados()
            Case "polopendencias"
                Call CarregaPolosPendencias()
            Case "escola"
                Call CarregaEscolas()
            Case "escolaagendamento"
                Call CarregaEscolasAgendamento()
            Case "colaboradortriagem"
                Call CarregaColaboradoresTriagem()
            Case "colaboradorfuncao"
                Call CarregaColaboradorFuncao()
            Case "aplicacoes"
                Call CarregaAPLICACOES()
            Case "turmaregistro"
                Call CarregaTurmaRegistro()
            Case "escolaturmas"
                Call CarregaEscolasTURMAS()
            Case "carregaespecializados"
                Call CarregaESPECIALIZADOS()
            Case Else
                MensagemERROS.Text = TextoVermelho("Houve um erro de sistema. ")
        End Select
    End Sub


    Protected Sub ExibeOcultaFiltros()
        cmbUF.Enabled = False
        Select Case mvQueTipo
            Case "polo"
                Titulo.Text = "Mapa geral dos polos"
                LabelPolo.Visible = False
                cmbPolo.Visible = False
                LabelEscola.Visible = False
                cmbEscola.Visible = False
                LabelFuncao.Visible = False
                cmbFuncao.Visible = False
                LabelCategoria.Visible = False
                cmbCategoria.Visible = False
            Case "polocolaborador"
                Titulo.Text = "Polos & colaboradores associados"
                LabelEscola.Visible = False
                cmbEscola.Visible = False
                LabelFuncao.Visible = False
                cmbFuncao.Visible = False
                LabelCategoria.Visible = False
                cmbCategoria.Visible = False
                LabelStatus.Visible = False
                cmbStatus.Visible = False
            Case "aplicacoes"
                Titulo.Text = "Polos, escolas & aplicações"
                LabelFuncao.Visible = False
                cmbFuncao.Visible = False
                LabelCategoria.Visible = False
                cmbCategoria.Visible = False
                LabelStatus.Visible = False
                cmbStatus.Visible = False
            Case "polopendencias"
                Titulo.Text = "Quadro de polos e suas pendências"
                LabelPolo.Visible = False
                cmbPolo.Visible = False
                LabelEscola.Visible = False
                cmbEscola.Visible = False
                LabelFuncao.Visible = False
                cmbFuncao.Visible = False
                LabelCategoria.Visible = False
                cmbCategoria.Visible = False
            Case "escola"
                Titulo.Text = "Mapa geral das escolas"
                LabelFuncao.Visible = False
                cmbFuncao.Visible = False
                LabelEscola.Visible = False
                cmbEscola.Visible = False
                LabelCategoria.Visible = False
                cmbCategoria.Visible = False
                LabelStatus.Visible = False
                cmbStatus.Visible = False
            Case "turmaregistro"
                Titulo.Text = "Registro de aplicações"
                LabelFuncao.Visible = False
                cmbFuncao.Visible = False
                'LabelEscola.Visible = False
                'cmbEscola.Visible = False
                LabelCategoria.Visible = False
                cmbCategoria.Visible = False
                LabelStatus.Visible = False
                cmbStatus.Visible = False
            Case "escolaagendamento"
                Titulo.Text = "Agendamento de escolas"
                LabelFuncao.Visible = False
                cmbFuncao.Visible = False
                LabelEscola.Visible = False
                cmbEscola.Visible = False
                'LabelMunicipio.Visible = False
                'cmbMunicipio.Visible = False
                LabelCategoria.Visible = False
                cmbCategoria.Visible = False
                LabelStatus.Visible = False
                cmbStatus.Visible = False
            Case "colaboradortriagem"
                Titulo.Text = "Quadro de triagem de colaboradores"
                LabelEscola.Visible = False
                cmbEscola.Visible = False
                LabelPolo.Visible = False
                cmbPolo.Visible = False
                LabelFuncao.Visible = False
                cmbFuncao.Visible = False
            Case "colaboradorfuncao"
                Titulo.Text = "Mapa de gestores e polos"
                LabelEscola.Visible = False
                cmbEscola.Visible = False
                LabelCategoria.Visible = False
                cmbCategoria.Visible = False
                LabelStatus.Visible = False
                cmbStatus.Visible = False
            Case "escolaturmas"
                Titulo.Text = "Mapa geral de turmas"
                LabelFuncao.Visible = False
                cmbFuncao.Visible = False
                LabelCategoria.Visible = False
                cmbCategoria.Visible = False
                LabelStatus.Visible = False
                cmbStatus.Visible = False
            Case "carregaespecializados"
                Titulo.Text = "Cadastro de aplicadores especializados INEP"
                LabelFuncao.Visible = False
                cmbFuncao.Visible = False
                LabelEscola.Visible = False
                cmbEscola.Visible = False
                LabelMunicipio.Visible = False
                cmbMunicipio.Visible = False
                LabelCategoria.Visible = False
                cmbCategoria.Visible = False
                LabelStatus.Visible = False
                cmbStatus.Visible = False
                LabelPolo.Visible = False
                cmbPolo.Visible = False
            Case Else
                MensagemERROS.Text = TextoVermelho("Houve um erro de sistema. ")
        End Select
    End Sub


    Private Sub CarregaPoloAssociados()

        Dim OK As String = "OK"
        Dim row As TableRow
        Dim PessoaID As String
        Dim AtribuicaoID As Int32

        Try
            Dim sSQL As String =
            "SELECT isnull(x.SG_UF_ALOC,'') SG_UF_ALOC, p.NO_MUNICIPIO,p.NO_POLO,isnull(x.CPF,'') CPF,isnull(x.NO_PESSOA,'') NO_PESSOA," &
                   "isnull(x.NU_TELEFONE1,'') NU_TELEFONE1,isnull(x.NU_TELEFONE2,'') NU_TELEFONE2,isnull(x.NO_CATEGORIA,'') NO_CATEGORIA," &
                   "isnull(a.NO_FUNCAO,'') NO_FUNCAO,isnull(a.ID,0) ID_ATRIBUICAO," &
                   "(SELECT COUNT(*) FROM ATRIBUICAO_POLO a WHERE a.ID_POLO=p.ID_POLO) NUM_APLICADORES," &
                   "(SELECT COUNT(distinct ID_POLO) FROM ATRIBUICAO_POLO a WHERE a.CPF=x.CPF) NUM_POLOS," &
                   "0 NUM_APLICACOES" &
           " FROM (SELECT * FROM POLO WHERE (@SG_UF_ALOC='Todos' or SG_UF=@SG_UF_ALOC)" &
                                      " And (@NO_POLO='Todos' or @NO_POLO=NO_POLO) and (@NO_MUNICIPIO='Todos' or @NO_MUNICIPIO=NO_MUNICIPIO)" &
                                      " And EXISTS(select * from ATRIBUICAO_GESTOR a where a.CPF=@MeuCPF and (a.ID_POLO='*' or a.ID_POLO=POLO.ID_POLO))" &
                  ") p" &
           " INNER JOIN ATRIBUICAO_POLO a On p.ID_POLO=a.ID_POLO" &
           " INNER JOIN (SELECT * FROM PESSOAL WHERE (@SG_UF_ALOC='Todos' or @SG_UF_ALOC='BR' or SG_UF_ALOC=@SG_UF_ALOC)" &
                                              " And (@BUSCA='' or NO_PESSOA like '%' + @BUSCA + '%' or CPF like '%' + @BUSCA + '%')" &
                      ") x On x.CPF=a.CPF" &
           " ORDER BY 2,3,5"

            Dim MyC = New System.Data.SqlClient.SqlConnection(Ligacao())
            MyC.Open()

            Dim MeuComando = New System.Data.SqlClient.SqlCommand(sSQL, MyC)
            Dim RS As System.Data.SqlClient.SqlDataReader

            MeuComando.Parameters.Add("@NO_MUNICIPIO", SqlDbType.VarChar).Value = cmbMunicipio.Text
            MeuComando.Parameters.Add("@NO_POLO", SqlDbType.VarChar).Value = cmbPolo.Text
            MeuComando.Parameters.Add("@NO_FUNCAO", SqlDbType.VarChar).Value = cmbFuncao.Text
            MeuComando.Parameters.Add("@SG_UF_ALOC", SqlDbType.VarChar).Value = Parametros(gcParametroUFbase)
            MeuComando.Parameters.Add("@BUSCA", SqlDbType.VarChar).Value = CampoFiltroBusca.Text
            MeuComando.Parameters.Add("@MeuCPF", SqlDbType.VarChar).Value = Parametros(gcParametroCPF)

            MeuComando.CommandText = sSQL
            RS = MeuComando.ExecuteReader

            ' Limpa a tabela e define a largura de cada coluna
            TabelaPolos.Controls.Clear()
            Dim LL As Int16() = {100, 100, 20, 20, 90, 30, 30, 20, 20, 20, 20, 20, 20, 20, 20}

            row = New TableRow()
            Call CarregaColunasDeTextoNumaLinha(row, "MUNICÍPIO do POLO|POLO|#Aplicadores|CPF|COLABORADOR|TELEFONES|FUNÇÃO|#Aplicações|#Polos|Associação|Visualizar|Remoção", LL)
            TabelaPolos.Controls.Add(row)

            ' Varre record set
            Dim C As Int32 = 0
            Dim Linhas As Int16 = 0
            While RS.Read()
                C += 1
                If C >= (mvPrimeiraPaginaMostrada - 1) * mcLimiteLinhasMostradas + 1 And Linhas < mcLimiteLinhasMostradas Then
                    Linhas += 1
                    row = New TableRow()
                    Dim tmpSTR As String = ""
                    tmpSTR += RS.GetString(RS.GetOrdinal("NO_MUNICIPIO"))
                    tmpSTR += "|" + RS.GetString(RS.GetOrdinal("NO_POLO"))
                    tmpSTR += "|" + Str(RS.GetValue(RS.GetOrdinal("NUM_APLICADORES")))
                    tmpSTR += "|" + RS.GetString(RS.GetOrdinal("CPF"))
                    tmpSTR += "|" + RS.GetString(RS.GetOrdinal("NO_PESSOA"))
                    tmpSTR += "|" + RS.GetString(RS.GetOrdinal("NU_TELEFONE1")) + " " + RS.GetString(RS.GetOrdinal("NU_TELEFONE2"))
                    tmpSTR += "|" + RS.GetString(RS.GetOrdinal("NO_FUNCAO"))
                    tmpSTR += "|" + Str(RS.GetValue(RS.GetOrdinal("NUM_APLICACOES")))
                    tmpSTR += "|" + Str(RS.GetValue(RS.GetOrdinal("NUM_POLOS")))

                    ' Prepara as colunas
                    Call CarregaColunasDeTextoNumaLinha(row, tmpSTR, LL)

                    '' Prepara Java Script "javascript: __doPostBack('xxx','xxx')"
                    'PoloID = RS.GetString(RS.GetOrdinal("ID_POLO"))
                    AtribuicaoID = RS.GetValue(RS.GetOrdinal("ID_ATRIBUICAO"))
                    PessoaID = RS.GetString(RS.GetOrdinal("CPF"))

                    ' Posiciona os botões
                    ' Sendo pelo menos Coordenador de Polo é possível atributir funções
                    If PoderDeUmaFuncao(Parametros(gcParametroFuncao)) >= 4 And Trim(RS.GetString(RS.GetOrdinal("CPF"))) <> "" Then
                        Call CarregaColunaDeBotaoNumaLinha(row, "Associar",
                                                            RS.GetString(RS.GetOrdinal("CPF")) &
                                                            "," & RS.GetString(RS.GetOrdinal("NO_PESSOA")) &
                                                            "," & RS.GetString(RS.GetOrdinal("SG_UF_ALOC")), "basic2-158_home_house.png",
                                                            "Fazer mais uma associação para este colaborador", mvQueTipo)
                    Else
                        ' Carrega uma coluna em branco
                        Call CarregaColunasDeTextoNumaLinha(row, "", LL)
                    End If

                    If Trim(RS.GetString(RS.GetOrdinal("CPF"))) <> "" Then
                        Call CarregaColunaDeBotaoNumaLinha(row, "Visualizar", PessoaID & ",V", "basic3-020_presentation_powerpoint_keynote.png",
                                                       "Clique aqui para visualizar o cadastro", mvQueTipo)
                    Else
                        ' Carrega uma coluna em branco
                        Call CarregaColunasDeTextoNumaLinha(row, "", LL)
                    End If


                    If AtribuicaoID <> 0 And PoderDeUmaFuncao(RS.GetString(RS.GetOrdinal("NO_FUNCAO"))) < PoderDeUmaFuncao(Parametros(gcParametroFuncao)) Then
                        Call CarregaColunaDeBotaoNumaLinha(row, "Remover", Trim(Str(AtribuicaoID)), "basic1-020_bin_trash_delete.png",
                                                             "Remover essa associação", mvQueTipo)
                    Else
                        ' Carrega uma coluna em branco
                        Call CarregaColunasDeTextoNumaLinha(row, "", LL)
                    End If

                    ' Pauta as linhas
                    If Pauta(C) Then row.BackColor = mvCorPautas

                    TabelaPolos.Controls.Add(row)
                End If
            End While

            ' Mostra o total de linhas
            LiteralNumLinhas.Text = RecadoSobreLinhas(Linhas, C, True)
            Call AcertaPaginacao(C)

            If CaixaRemocao.Text <> "" Then
                MensagemERROS.Text += TextoVermelho("               Clique remover novamente para confirmar a operação. ")
            End If

            ' Encerra a leitura
            RS.Close()
            RS.Dispose()
            MeuComando.Dispose()
            MyC.Dispose()

        Catch ex As Exception
            OK = "Erro na leitura dos dados de polos e colaboradores." & vbCrLf & ex.Message

        End Try

        If OK <> "OK" Then MensagemERROS.Text = TextoVermelho(OK)

    End Sub


    Private Sub CarregaColaboradorFuncao()

        Dim OK As String = "OK"
        Dim row As TableRow
        Dim PoloID As String
        Dim PessoaID As String
        Dim AtribuicaoID As Int32

        Try
            Dim Juncao1 As String = IIf(cmbMunicipio.Text = "Todos", " LEFT JOIN ", " INNER JOIN ")
            Dim Juncao2 As String = IIf(cmbPolo.Text = "Todos", " LEFT JOIN ", " INNER JOIN ")
            Dim Juncao3 As String = IIf(cmbFuncao.Text = "Todas", " LEFT JOIN ", " INNER JOIN ")

            Dim sSQL As String =
            "SELECT TOP " & mcLimiteLinhasLidas & " p.SG_UF_ALOC, p.SG_UF_ALOC + ' - ' + isnull(m.NO_MUNICIPIO,'') UF_MUNIC,p.CPF,p.NO_PESSOA,p.NO_CATEGORIA," &
                        "isnull(a.NO_FUNCAO,'') NO_FUNCAO,isnull(a.ID,0) ID_ATRIBUICAO, isnull(po.NO_POLO,'') NO_POLO,isnull(po.ID_POLO,'') ID_POLO," &
                        "case when exists (SELECT * FROM ATRIBUICAO_GESTOR a" &
                                         " WHERE a.ID_POLO=po.ID_POLO And a.CPF='" & Parametros(gcParametroCPF) & "') then 'S' else 'N' end EH_GESTOR_POLO," &
                        " case when (SELECT Data_Fechamento FROM UF WHERE UF.SG_UF=p.SG_UF_ALOC) < GETDATE() then 'S' else 'N' end ESTADO_FECHADO" &
           " FROM (Select * FROM PESSOAL WHERE" &
                " NO_CATEGORIA='Gestor'" &
                " And (@SG_UF_ALOC='Todos' or @SG_UF_ALOC='BR' or SG_UF_ALOC=@SG_UF_ALOC)" &
                " And (@BUSCA='' or NO_PESSOA like '%' + @BUSCA + '%' or CPF like '%' + @BUSCA + '%')" &
                " )p" &
                Juncao3 & "(SELECT * FROM ATRIBUICAO_GESTOR WHERE @NO_FUNCAO='Todas' or  @NO_FUNCAO=NO_FUNCAO) a on p.CPF=a.CPF" &
                Juncao2 & "(SELECT * FROM POLO WHERE @NO_POLO='Todos' or @NO_POLO=NO_POLO)po on a.ID_POLO=po.ID_POLO" &
                Juncao1 & "(select * FROM MUNICIPIO where @NO_MUNICIPIO='Todos' or NO_MUNICIPIO=@NO_MUNICIPIO)m on p.CO_MUNICIPIO= m.CO_MUNICIPIO" &
           " ORDER BY 2,4,6"

            'Call AbreConexao()
            Dim MyC = New System.Data.SqlClient.SqlConnection(Ligacao())
            MyC.Open()

            Dim MeuComando = New System.Data.SqlClient.SqlCommand(sSQL, MyC)
            Dim RS As System.Data.SqlClient.SqlDataReader

            'MeuComando.Parameters.Add("@SG_UF", SqlDbType.VarChar).Value = cmbUF.Text
            MeuComando.Parameters.Add("@NO_MUNICIPIO", SqlDbType.VarChar).Value = cmbMunicipio.Text
            MeuComando.Parameters.Add("@NO_POLO", SqlDbType.VarChar).Value = cmbPolo.Text
            MeuComando.Parameters.Add("@NO_FUNCAO", SqlDbType.VarChar).Value = cmbFuncao.Text
            MeuComando.Parameters.Add("@SG_UF_ALOC", SqlDbType.VarChar).Value = Parametros(gcParametroUFbase)
            MeuComando.Parameters.Add("@BUSCA", SqlDbType.VarChar).Value = CampoFiltroBusca.Text
            MeuComando.Parameters.Add("@MeuCPF", SqlDbType.VarChar).Value = Parametros(gcParametroCPF)

            MeuComando.CommandText = sSQL
            RS = MeuComando.ExecuteReader

            ' Limpa a tabela e define a largura de cada coluna
            TabelaPolos.Controls.Clear()
            Dim LL As Int16() = {50, 120, 90, 70, 90, 20, 20, 20, 20, 20, 20, 20}

            row = New TableRow()
            Call CarregaColunasDeTextoNumaLinha(row, "CPF|GESTOR|UF - MUNICÍPIO do GESTOR|FUNÇÃO|POLO|Atribuição|Edição|Remoção|Visualização", LL)
            TabelaPolos.Controls.Add(row)

            ' Varre record set
            Dim C As Int32 = 0
            Dim Linhas As Int16 = 0
            While RS.Read()
                C += 1
                If C >= (mvPrimeiraPaginaMostrada - 1) * mcLimiteLinhasMostradas + 1 And Linhas < mcLimiteLinhasMostradas Then
                    Linhas += 1
                    row = New TableRow()
                    Dim tmpSTR As String = ""
                    tmpSTR += RS.GetString(RS.GetOrdinal("CPF"))
                    tmpSTR += "|" + RS.GetString(RS.GetOrdinal("NO_PESSOA"))
                    tmpSTR += "|" + RS.GetString(RS.GetOrdinal("UF_MUNIC"))
                    tmpSTR += "|" + RS.GetString(RS.GetOrdinal("NO_FUNCAO"))
                    tmpSTR += "|" + RS.GetString(RS.GetOrdinal("NO_POLO"))


                    ' Prepara as colunas
                    Call CarregaColunasDeTextoNumaLinha(row, tmpSTR, LL)

                    ' Prepara Java Script "javascript: __doPostBack('xxx','xxx')"
                    PoloID = RS.GetString(RS.GetOrdinal("ID_POLO"))
                    AtribuicaoID = RS.GetValue(RS.GetOrdinal("ID_ATRIBUICAO"))
                    PessoaID = RS.GetString(RS.GetOrdinal("CPF"))

                    ' Posiciona os botões
                    ' Sendo pelo menos Coordenador de Polo é possível atributir funções
                    If PoderDeUmaFuncao(Parametros(gcParametroFuncao)) >= 4 _
                        And RS.GetString(RS.GetOrdinal("NO_FUNCAO")) <> "Observador" _
                        And RS.GetString(RS.GetOrdinal("NO_FUNCAO")) <> "Administrador" Then
                        Call CarregaColunaDeBotaoNumaLinha(row, "Atribuir",
                                                            RS.GetString(RS.GetOrdinal("CPF")) &
                                                            "," & RS.GetString(RS.GetOrdinal("NO_PESSOA")) &
                                                            "," & RS.GetString(RS.GetOrdinal("SG_UF_ALOC")), "basic2-112_to_do_list_clipboard.png",
                                                            "Atribuir uma funçao especial", mvQueTipo)
                    Else
                        ' Carrega uma coluna em branco
                        Call CarregaColunasDeTextoNumaLinha(row, "", LL)
                    End If

                    If Trim(PoloID) <> "" And (Parametros(gcParametroFuncao) = "Coordenador Estadual" _
                                               Or Parametros(gcParametroFuncao) = "Administrador" _
                                               Or RS.GetValue(RS.GetOrdinal("EH_GESTOR_POLO")) = "S"
                                               ) Then
                        Call CarregaColunaDeBotaoNumaLinha(row, "Editar", PoloID & ",U", "basic1-002_write_pencil_new_edit.png",
                                                           "Editar os dados do polo", mvQueTipo)
                    Else
                        ' Carrega uma coluna em branco
                        Call CarregaColunasDeTextoNumaLinha(row, "", LL)
                    End If

                    If AtribuicaoID <> 0 And (Parametros(gcParametroFuncao) = "Administrador" Or RS.GetString(RS.GetOrdinal("ESTADO_FECHADO")) = "N") _
                                         And (Parametros(gcParametroFuncao) = "Coordenador Estadual" _
                                               Or Parametros(gcParametroFuncao) = "Administrador" _
                                               Or RS.GetValue(RS.GetOrdinal("EH_GESTOR_POLO")) = "S"
                                               ) _
                                         And PoderDeUmaFuncao(RS.GetString(RS.GetOrdinal("NO_FUNCAO"))) < PoderDeUmaFuncao(Parametros(gcParametroFuncao)) Then
                        Call CarregaColunaDeBotaoNumaLinha(row, "Remover", Trim(Str(AtribuicaoID)), "basic1-020_bin_trash_delete.png",
                                                             "Remover essa atribuição de função", mvQueTipo)
                    Else
                        ' Carrega uma coluna em branco
                        Call CarregaColunasDeTextoNumaLinha(row, "", LL)
                    End If

                    ' Posiciona os botões
                    Call CarregaColunaDeBotaoNumaLinha(row, "Visualizar", PessoaID & ",V", "basic3-020_presentation_powerpoint_keynote.png",
                                                       "Visualizar o cadastro do gestor", mvQueTipo)

                    ' Pauta as linhas
                    If Pauta(C) Then row.BackColor = mvCorPautas

                    TabelaPolos.Controls.Add(row)
                End If
            End While

            ' Mostra o total de linhas
            LiteralNumLinhas.Text = RecadoSobreLinhas(Linhas, C, True)
            Call AcertaPaginacao(C)

            If CaixaRemocao.Text <> "" Then
                MensagemERROS.Text += TextoVermelho("               Clique remover novamente para confirmar a operação. ")
            End If

            ' Encerra a leitura
            RS.Close()
            RS.Dispose()
            MeuComando.Dispose()
            MyC.Dispose()

        Catch ex As Exception
            OK = "Erro na leitura dos dados de polos." & vbCrLf & ex.Message

        End Try

        If OK <> "OK" Then MensagemERROS.Text = TextoVermelho(OK)

    End Sub

    Protected Function RecadoSobreLinhas(ByRef QuantasLinhas As Int32, ByRef TotalDeLinhas As Int16, ByRef flgMostraPaginacao As Boolean) As String

        cmbPaginas.Visible = flgMostraPaginacao
        cmRetroceder.Visible = (Not mvPrimeiraPaginaMostrada = 1) And flgMostraPaginacao
        cmdAvancar.Visible = (Not mvPrimeiraPaginaMostrada = Math.Ceiling(TotalDeLinhas / mcLimiteLinhasMostradas)) And flgMostraPaginacao
        If QuantasLinhas = 0 Then
            Return "Nenhuma linha foi exibida."
        Else
            Return "Exibidas linhas " & (mvPrimeiraPaginaMostrada - 1) * mcLimiteLinhasMostradas + 1 & " até " &
                   (mvPrimeiraPaginaMostrada - 1) * mcLimiteLinhasMostradas + QuantasLinhas & " de um total de " & TotalDeLinhas & " linhas." &
                   IIf(QuantasLinhas < TotalDeLinhas Or QuantasLinhas = 0, " Use os filtros para reduzir a lista.", "")
        End If

    End Function


    Private Sub cmdVoltar_Click(sender As Object, e As EventArgs) Handles cmdVoltar.Click
        Response.Redirect(ResolveUrl("~/BemVindo.aspx" & MontaQueryStringFromParametros("POLOS", "BemVindo", Parametros)))
    End Sub


    Protected Function EmpacotaFiltros() As String
        Return IIf(cmbUF.Text = "Todos", "", cmbUF.Text) & gcSeparadorFiltros &
               IIf(cmbMunicipio.Text = "Todos", "", cmbMunicipio.Text) & gcSeparadorFiltros &
               IIf(cmbEscola.Text = "Todos", "", cmbUF.Text) & gcSeparadorFiltros &
               IIf(cmbPolo.Text = "Todos", "", cmbPolo.Text) & gcSeparadorFiltros &
               IIf(cmbCategoria.Text = "Todas", "", cmbCategoria.Text) & gcSeparadorFiltros &
               IIf(cmbFuncao.Text = "Todas", "", cmbFuncao.Text) & gcSeparadorFiltros &
               IIf(cmbStatus.Text = "Todos", "", cmbStatus.Text) & gcSeparadorFiltros &
               CampoFiltroBusca.Text
    End Function


    'Protected Function EmpacotaUmFiltro(ByRef QuePosicao As Int16, ByRef QueValor As String) As String
    '    'Dim x As String() = Split(Parametros(gcParametroFiltros), gcSeparadorFiltros)
    '    Dim x As String() = Split(mvFiltros, gcSeparadorFiltros)
    '    x(QuePosicao) = QueValor
    '    ' Reconstroi a string de filtros
    '    Dim tmp As String = x(0)
    '    For k = 1 To UBound(x)
    '        tmp &= gcSeparadorFiltros & x(k)
    '    Next
    '    Return tmp
    'End Function


    Protected Function ValorDeUmFiltro(ByRef Posicao As Int16) As String
        'Dim x As String() = Split(Parametros(gcParametroFiltros), gcSeparadorFiltros)
        Dim x As String() = Split(mvFiltros, gcSeparadorFiltros)
        Return x(Posicao)
    End Function


    Protected Sub DesEmpacotaFiltros(ByRef QueFiltros As String)
        ' IMPORTANTE: essa função define a posição dos filtros
        If QueFiltros = "@" Then
            ' Não inicializa, deixa os defaults
        Else
            Dim x As String() = Split(QueFiltros, gcSeparadorFiltros)
            cmbUF.Text = x(0)
            cmbMunicipio.Text = x(1)
            cmbEscola.Text = x(2)
            cmbPolo.Text = x(3)
            cmbCategoria.Text = x(4)
            cmbFuncao.Text = x(5)
            cmbStatus.Text = x(6)
            CampoFiltroBusca.Text = x(7)
        End If
    End Sub

End Class