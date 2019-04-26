Public Class AGENDAMENTO
    Inherits System.Web.UI.Page

    Dim Operacao As String
    Dim Parametros As String()
    Dim mvCorPautas As Drawing.Color = Drawing.Color.WhiteSmoke
    Dim mvPaginaAtrasada As Boolean
    Dim mvQueTipo As String                 ' Escola, Polo, ...
    Const mcOffSet = 3
    Const mcNumColunasAgendamento = 7
    Const mcNumLinhasAgendamento = 2
    Const mcNumDatas = 8
    Const mcLarguraComum = 10


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Se houver um erros na submissão da página (query string inválida ou  página atrasada), impede eventos posteriores
        mvPaginaAtrasada = False

        ' Verifica se a query string está OK e se as credenciais são atendidas
        Dim tmp As String = ConsisteQueryString("AGENDAMENTO", Request, CompletaTamanhoMascara("SSSS"), Parametros, IsPostBack, True)
        If tmp <> "OK" Then
            MensagemERRO.Text = TextoVermelho(tmp)
            Exit Sub
        End If

        ' Descobre qual é a operação
        Dim QueEscola = Request.QueryString("escola")
        If QueEscola Is Nothing Then
            MensagemERRO.Text = TextoVermelho("Nenhuma escola foi selecionada. ")
            mvPaginaAtrasada = True
            Exit Sub
        Else
            ' Pode ser edição ou visualização
            Dim x As String() = Split(QueEscola, ",")
            If UBound(x) < 1 Then
                MensagemERRO.Text = TextoVermelho("Houve um erro de sistema: falta o parâmetro de operação. ")
            Else
                QueEscola = x(0)
                Operacao = x(1)
            End If
        End If

        ' -------------------------
        If Not IsPostBack Then

        Else
            MensagemERRO.Text = ""
            ' Pega o evento do Java script
            tmp = Request.Form("__EVENTTARGET")
            If tmp <> "" Then
                Dim x As String() = Split(Request.Form("__EVENTTARGET"), ";")
                If UBound(x) = 2 Then
                    ' Age de acordo com ícone clicado
                    Select Case x(0)
                        Case "Retroceder"
                            Dim sSQL As String = "UPDATE TURMA_APLICACAO set" &
                                               " ID_DIA_APLICACAO = ID_DIA_APLICACAO -1," &
                                               " AltDate=GETDATE()," &
                                               " AltUser='" & Parametros(gcParametroCPF) & "'" &
                                                " WHERE ID=" & x(2) & " And ID_DIA_APLICACAO > 1" &
                                                " And Not exists (select * from TURMA_APLICACAO ta" &
                                                                " where ta.CO_TURMA_CENSO=TURMA_APLICACAO.CO_TURMA_CENSO" &
                                                                " And ta.ID_DIA_APLICACAO=TURMA_APLICACAO.ID_DIA_APLICACAO-1" &
                                                  ")"
                            Call ExecutaSQL(sSQL)
                        Case "Avançar"
                            Dim sSQL As String = "UPDATE TURMA_APLICACAO set" &
                                               " ID_DIA_APLICACAO = ID_DIA_APLICACAO +1," &
                                               " AltDate=GETDATE()," &
                                               " AltUser='" & Parametros(gcParametroCPF) & "'" &
                                                " WHERE ID=" & x(2) & " And ID_DIA_APLICACAO<" & mcNumDatas &
                                                " And Not exists (select * from TURMA_APLICACAO ta" &
                                                                " where ta.CO_TURMA_CENSO=TURMA_APLICACAO.CO_TURMA_CENSO" &
                                                                " And ta.ID_DIA_APLICACAO=TURMA_APLICACAO.ID_DIA_APLICACAO+1" &
                                                  ")"
                            Call ExecutaSQL(sSQL)
                        Case "Alocar"
                            ' Parâmetros passados na query string turma: CO_TURMA_CENSO, ID_PROVA, ID_DIA_APLICACAO, TURNO_BLOQUEIO_APLICADOR
                            Response.Redirect(ResolveUrl("~/AlocarAplicadorPrincipal.aspx" & MontaQueryStringFromParametros("AGENDAMENTO" & ",tipo=" & mvQueTipo, "POLOS", Parametros) _
                                                                                           & "&turma=" & x(2) & "&escola=" & QueEscola))
                        Case "AlocarSalasExtras"
                            Response.Redirect(ResolveUrl("~/AlocarAplicadorSalasExtras.aspx" & MontaQueryStringFromParametros("AGENDAMENTO" & ",tipo=" & mvQueTipo, "POLOS", Parametros) & "&turma=" & x(2) & "&escola=" & QueEscola))

                        Case "Registrar"
                            Response.Redirect(ResolveUrl("~/PerguntasTurma.aspx" & MontaQueryStringFromParametros("POLOS" & ",tipo=" & mvQueTipo, "POLOS", Parametros) & "&turma=" & x(2) & "&escola=" & QueEscola))

                        Case Else
                            MensagemERRO.Text = TextoVermelho("Houve um erro de sistema: operação não definida.")
                    End Select
                End If
            End If
        End If

        ' ------------------------------
        Call CarregaEscolas(QueEscola)
    End Sub
    Private Sub CarregaEscolas(ByRef QueEscola As String)
        Dim K As Int16
        Dim OK As String = "OK"
        Dim row1 As TableRow
        Dim row2 As TableRow = Nothing
        'Dim row3 As TableRow
        'Dim row4 As TableRow
        Dim TurmaAplicacaoID As String
        Dim TurmaCO As String
        'Dim tmp As String
        Try

            ' Com minuto inicial
            'Dim sSQL As String =
            '"SELECT t.CO_TURNO,t.TX_HR_INICIAL,t.TX_MI_INICIAL,t.NO_TURMA,t.CO_TURMA_CENSO,m.NO_MUNICIPIO,e.ID_ESCOLA,e.NO_ESCOLA,p.NO_POLO," &
            '      " ta.ID_TURMA_APLICACAO,ta.ID_DIA_APLICACAO,ta.ID_PROVA,ta.ID,da.NO_DIMA_APLICACAO,pr.NO_PROVA,tn.NO_TURNO" &
            '" FROM ESCOLA e INNER JOIN MUNICIPIO m on e.CO_MUNICIPIO= m.CO_MUNICIPIO" &
            '     " INNER JOIN POLO p On e.ID_POLO = p.ID_POLO" &
            '     " INNER JOIN TURMA t On t.ID_ESCOLA = e.ID_ESCOLA" &
            '     " LEFT JOIN TURMA_APLICACAO ta On t.CO_TURMA_CENSO=ta.CO_TURMA_CENSO" &
            '     " LEFT JOIN DIA_APLICACAO da On ta.ID_DIA_APLICACAO=da.ID_DIA_APLICACAO" &
            '     " LEFT JOIN PROVA pr On ta.ID_PROVA=pr.ID_PROVA" &
            '     " LEFT JOIN TURNO tn On tn.CO_TURNO=t.CO_TURNO" &
            '" WHERE e.ID_ESCOLA=@ID_ESCOLA" &
            '" ORDER BY t.TX_HR_INICIAL,t.NO_TURMA,ta.ID_PROVA"

            ' Sem minuto inicial
            Dim sSQL As String =
            "SELECT t.CO_TURNO,t.TX_HR_INICIAL, 0 TX_MI_INICIAL,t.NO_TURMA,t.CO_TURMA_CENSO,m.NO_MUNICIPIO,e.ID_ESCOLA,e.NO_ESCOLA," &
                  "e.NU_TELEFONE,e.NU_TELEFONE_PUBLICO,e.NU_TELEFONE_CONTATO,p.NO_POLO,  IsNull( t.FLG_CONFIRMADO ,'N')  FLG_CONFIRMADO," &
                  "IsNull(th.DS_TURNO,'') TURNO, IsNull( th.DS_HORARIO,'') HORARIO, IsNull( TURNO_BLOQUEIO_APLICADOR,'') TURNO_BLOQUEIO_APLICADOR, " &
                  " ta.ID_TURMA_APLICACAO,ta.ID_DIA_APLICACAO,ta.ID_PROVA,ta.ID,da.NO_DIMA_APLICACAO,pr.NO_PROVA,tn.NO_TURNO,pp.NO_PESSOA," &
                  " case WHEN EXISTS(SELECT * from TURMA_APLICACAO ta2" &
                                   " WHERE ta2.CO_TURMA_CENSO=ta.CO_TURMA_CENSO And ta2.ID_DIA_APLICACAO=ta.ID_DIA_APLICACAO - 1) Then 'S' else 'N' end EXISTE_ESQUERDA," &
                  " case WHEN EXISTS(SELECT * from TURMA_APLICACAO ta2" &
                                   " WHERE ta2.CO_TURMA_CENSO=ta.CO_TURMA_CENSO And ta2.ID_DIA_APLICACAO=ta.ID_DIA_APLICACAO + 1) Then 'S' else 'N' end EXISTE_DIREITA," &
                  " case WHEN EXISTS(SELECT * from TURMA_SALA ts" &
                                   " WHERE ts.CO_TURMA_CENSO=ta.CO_TURMA_CENSO And ts.ID_PROVA=ta.ID_PROVA and ts.ID_GRUPO_SALA <> 0) Then 'S' else 'N' end EXISTE_SALA_EXTRA," &
                  " case WHEN NOT EXISTS(SELECT * from TURMA_SALA ts" &
                                   " WHERE ts.CO_TURMA_CENSO=ta.CO_TURMA_CENSO And ts.ID_PROVA=ta.ID_PROVA and ts.CPF_APLICADOR IS NULL) Then 'S' else 'N' end TURMA_DIA_FECHADO," &
                  " case WHEN EXISTS(SELECT * from TURMA_SALA ts" &
                                   " WHERE ts.CO_TURMA_CENSO=ta.CO_TURMA_CENSO And ts.ID_PROVA=ta.ID_PROVA and ts.CPF_APLICADOR IS NOT NULL) Then 'S' else 'N' end EXISTE_ALOCACAO" &
            " FROM (select * FROM ESCOLA e WHERE e.ID_ESCOLA=@ID_ESCOLA) e" &
                 " INNER JOIN MUNICIPIO m on e.CO_MUNICIPIO= m.CO_MUNICIPIO" &
                 " INNER JOIN TURMA t On t.ID_ESCOLA = e.ID_ESCOLA" &
                 " LEFT JOIN POLO p On e.ID_POLO = p.ID_POLO" &
                 " LEFT JOIN TURMA_APLICACAO ta On t.CO_TURMA_CENSO=ta.CO_TURMA_CENSO" &
                 " LEFT JOIN DIA_APLICACAO da On ta.ID_DIA_APLICACAO=da.ID_DIA_APLICACAO" &
                 " LEFT JOIN PROVA pr On ta.ID_PROVA=pr.ID_PROVA" &
                 " LEFT JOIN TURNO tn On tn.CO_TURNO=t.CO_TURNO" &
                 " LEFT JOIN TURMA_SALA ts On ts.CO_TURMA_CENSO=t.CO_TURMA_CENSO And ts.ID_PROVA=pr.ID_PROVA And  ts.SEQ_TURMA_SALA=1  " &
                 " LEFT Join PESSOAL pp on pp.CPF = ts.CPF_APLICADOR  " &
                 " LEFT JOIN TURNO_HORARIO th on t.ID_HORARIO=th.ID_TURNO_HORARIO " &
            " ORDER BY t.TX_HR_INICIAL,t.NO_TURMA,ta.ID_PROVA"

            ' Call AbreConexao()
            Dim MyC = New System.Data.SqlClient.SqlConnection(Ligacao())
            MyC.Open()

            Dim MeuComando = New System.Data.SqlClient.SqlCommand(sSQL, MyC)
            Dim RS As System.Data.SqlClient.SqlDataReader
            MeuComando.Parameters.Add("@ID_ESCOLA", SqlDbType.VarChar).Value = QueEscola
            MeuComando.CommandText = sSQL
            RS = MeuComando.ExecuteReader()

            ' Limpa a tabela e define a largura de cada coluna
            TabelaPolos.Controls.Clear()

            ' CABEÇALHO ------
            row1 = PreparaLinhaCompleta()
            row1.Height = 20

            ' Preenche primeira parte do cabeçalho
            row1.Cells(0).Text = "TURMA / hr"
            Dim Largura As Unit = New Unit(250, UnitType.Pixel)
            row1.Cells(0).Width = Largura
            row1.Cells(1).Text = ""
            row1.Cells(1).Style.Add("text-align", "center")
            Largura = New Unit(60, UnitType.Pixel)
            row1.Cells(1).Width = Largura
            row1.Cells(2).Text = "Horário/Turno Aplicação"
            Largura = New Unit(200, UnitType.Pixel)
            row1.Cells(2).Width = Largura
            row1.BackColor = Drawing.Color.LightGray
            row1.Font.Bold = True
            row1.ForeColor = Drawing.Color.White

            ' Preenche datas no cabeçalho
            Dim Hoje As Date = Now()
            For K = 1 To mcNumDatas
                row1.Cells(IndexOfCOmpleta(K)).Text = Dias(K)
            Next

            ' Adiciona a nova linha à tabela
            TabelaPolos.Controls.Add(row1)
            ' Coloca linhas de separação
            row1 = PreparaLinhaCompleta()
            row1.Height = 4
            TabelaPolos.Controls.Add(row1)

            ' RECORDSET
            Dim C As Int32 = 0
            Dim Y As Int16 = 0
            Dim Linhas As Int16 = 0
            Dim R As New Random
            Dim Cor As Drawing.Color
            Dim UltimaTurma As String = ""
            While RS.Read()
                If C = 0 Then
                    Titulo.Text = RS.GetString(RS.GetOrdinal("ID_ESCOLA")) &
                                  " " & RS.GetString(RS.GetOrdinal("NO_ESCOLA")) &
                                  "  Tels. " & RS.GetString(RS.GetOrdinal("NU_TELEFONE")) &
                                  " " & RS.GetString(RS.GetOrdinal("NU_TELEFONE_PUBLICO")) &
                                  " " & RS.GetString(RS.GetOrdinal("NU_TELEFONE_CONTATO")) &
                                  " " & RS.GetString(RS.GetOrdinal("NO_MUNICIPIO"))
                End If
                C += 1
                Linhas += 1
                If RS.GetString(RS.GetOrdinal("CO_TURMA_CENSO")) <> UltimaTurma Then
                    ' Insere na tabela as linhas da última turma processada
                    If UltimaTurma <> "" Then
                        TabelaPolos.Controls.Add(row1)
                        TabelaPolos.Controls.Add(row2)

                        ' Coloca linhas de separação
                        row1 = PreparaLinhaCompleta()
                        row1.Height = 4
                        TabelaPolos.Controls.Add(row1)

                        row1 = PreparaLinhaCompleta()
                        row1.Height = 1
                        row1.BackColor = Drawing.Color.LightGray
                        TabelaPolos.Controls.Add(row1)

                        row1 = PreparaLinhaCompleta()
                        row1.Height = 12
                        TurmaCO = Trim(Str(RS.GetValue(RS.GetOrdinal("CO_TURMA_CENSO"))))
                        '' Posiciona controle para confirmação CENSO
                        'Call CarregaColunaDeBotao(row2.Cells(2), "Registrar", TurmaCO, "basic1-002_write_pencil_new_edit.png",
                        '                               "Confirmar dados Do censo e editar casos não previstos", mvQueTipo)
                        TabelaPolos.Controls.Add(row1)
                    End If

                    ' Marca onde começou esse agendaemnto
                    row1 = PreparaLinhaCompleta()
                    row2 = PreparaLinhaCompleta()

                    ' Prepara as colunas iniciais da primeira linha
                    row2.Cells(2).Text = RS.GetString(RS.GetOrdinal("TURNO"))
                    row2.Cells(2).Font.Size = 12
                    row2.Cells(2).Font.Bold = True
                    row1.Cells(2).Text = " " & RS.GetString(RS.GetOrdinal("HORARIO"))
                    row1.Cells(2).Font.Size = 12
                    row1.Cells(2).Font.Bold = True
                    row1.Cells(2).HorizontalAlign = HorizontalAlign.Center
                    row1.Cells(0).Text = RS.GetString(RS.GetOrdinal("NO_TURMA"))
                    row1.Cells(0).Font.Size = 12
                    row1.Cells(0).Font.Bold = True
                    UltimaTurma = RS.GetString(RS.GetOrdinal("CO_TURMA_CENSO"))

                    ' Prepara as colunas iniciais da segunda linha
                    row2.Cells(0).Text = RS.GetString(RS.GetOrdinal("NO_TURNO")) & ", " & RS.GetString(RS.GetOrdinal("TX_HR_INICIAL")) & "h"

                    If (RS("FLG_CONFIRMADO") <> "S") Then

                        Call CarregaColunaDeBotao(row1.Cells(1), "Registrar",
                                             RS.GetString(RS.GetOrdinal("CO_TURMA_CENSO")) &
                                             "," & RS.GetString(RS.GetOrdinal("NO_TURMA")),
                                             "basic1-002_write_pencil_new_edit.png",
                                             "Confirmar dados do censo e editar não previstos", mvQueTipo, True)
                    Else


                        Call CarregaColunaDeBotao(row1.Cells(1), "Registrar",
                                             RS.GetString(RS.GetOrdinal("CO_TURMA_CENSO")) &
                                             "," & RS.GetString(RS.GetOrdinal("NO_TURMA")),
                                             "basic1-179_check_yes.png",
                                             "Dados já confirmados!", mvQueTipo, True)

                    End If

                End If

                ' Descobre o dia do agendamento
                Dim Coluna As Int16 = RS.GetValue(RS.GetOrdinal("ID_DIA_APLICACAO"))

                ' Prepara os dados para o Java Script "javascript: __doPostBack('xxx','xxx')"
                TurmaAplicacaoID = Trim(Str(RS.GetValue(RS.GetOrdinal("ID"))))

                ' Preenche o agendamento
                K = IndexOfCOmpleta(Coluna)

                'If Operacao = "U" Then

                ' Disponibiliza os botões apenas se for Update
                If Coluna > 1 And RS.GetString(RS.GetOrdinal("EXISTE_ESQUERDA")) = "N" _
                        And RS.GetString(RS.GetOrdinal("EXISTE_ALOCACAO")) = "N" Then
                        Call CarregaColunaDeBotao(row2.Cells(K + 1), "Retroceder", TurmaAplicacaoID, "SetaEsquerda.png",
                                                  "Passar esta aplicação para o dia anterior", mvQueTipo,
                                                  True)
                    End If
                    If Coluna < mcNumDatas And RS.GetString(RS.GetOrdinal("EXISTE_DIREITA")) = "N" _
                        And RS.GetString(RS.GetOrdinal("EXISTE_ALOCACAO")) = "N" Then
                        Call CarregaColunaDeBotao(row2.Cells(K + 2), "Avançar", TurmaAplicacaoID, "basic3-124_open_share_window.png",
                                                  "Passar esta aplicação para o dia seguinte", mvQueTipo,
                                                  True)
                    End If
                Call CarregaColunaDeBotao(row1.Cells(K + 1), "Alocar",
                                                  RS.GetString(RS.GetOrdinal("CO_TURMA_CENSO")) &
                                                          "," & RS.GetString(RS.GetOrdinal("ID_PROVA")) &
                                                          "," & Trim(Str(RS.GetValue(RS.GetOrdinal("ID_DIA_APLICACAO")))) &
                                                          "," & RS.GetString(RS.GetOrdinal("TURNO_BLOQUEIO_APLICADOR")),
                                                  "basic2-110_user.png", "Alocar aplicador regular", mvQueTipo,
                                                  RS.GetValue(RS.GetOrdinal("TURNO")) <> "")

                If RS.GetString(RS.GetOrdinal("EXISTE_SALA_EXTRA")) = "S" Then


                    Call CarregaColunaDeBotao(row1.Cells(K + 2), "AlocarSalasExtras",
                                                  RS.GetString(RS.GetOrdinal("CO_TURMA_CENSO")) &
                                                          "," & RS.GetString(RS.GetOrdinal("ID_PROVA")),
                                                  "basic1-117_user_group_couple.png",
                                                  "Alocar aplicadores especializados/extras", mvQueTipo,
                                                  RS.GetValue(RS.GetOrdinal("TURNO")) <> "")
                End If
                    TurmaCO = Trim(Str(RS.GetValue(RS.GetOrdinal("CO_TURMA_CENSO"))))
                    '' Posiciona controle para confirmação CENSO
                    'Call CarregaColunaDeBotao(row2.Cells(2), "Registrar",
                    '                                         RS.GetString(RS.GetOrdinal("CO_TURMA_CENSO")) &
                    '                                         "," & RS.GetString(RS.GetOrdinal("NO_TURMA")),
                    '                                         "basic1-002_write_pencil_new_edit.png",
                    '                                   "Confirmar dados do censo e editar não previstos", mvQueTipo)

                    'End If

                    ' Colore as colunas do agendamento
                    If RS.GetString(RS.GetOrdinal("ID_PROVA")) = "1" Then
                    'Cor = Drawing.Color.WhiteSmoke
                    Cor = System.Drawing.Color.FromArgb(248, 193, 214)
                Else
                    'Cor = Drawing.Color.White
                    Cor = System.Drawing.Color.FromArgb(253, 210, 193)
                End If

                ' Nome do aplicador
                row1.Cells(K).Text = RS.GetString(RS.GetOrdinal("NO_PROVA"))

                If (IsDBNull(RS("NO_PESSOA"))) Then
                    row2.Cells(K).Text = ""
                Else
                    row2.Cells(K).Text = RS.GetString(RS.GetOrdinal("NO_PESSOA"))
                End If
                row2.Cells(K).Font.Size = 8


                'row1.Cells(K + 1).BackColor = Cor
                'row2.Cells(K + 1).BackColor = Cor
                'row1.Cells(K + 1).ForeColor = Drawing.Color.White
                'row2.Cells(K + 1).ForeColor = Drawing.Color.White

                'row1.Cells(K + 1).BorderStyle = BorderStyle.Outset
                'row1.Cells(K + 1).ForeColor = Drawing.Color.Black
                'row1.Cells(K + 1).BorderColor = Cor
                ''row2.Cells(K + 1).BorderStyle = BorderStyle.Solid
                ''row2.Cells(K + 1).ForeColor = Drawing.Color.Black
                ''row2.Cells(K + 1).BorderWidth = 1

                For J = K - 1 To K - 1 + mcNumColunasAgendamento - 2
                    row1.Cells(J).BackColor = Cor
                    row2.Cells(J).BackColor = Cor
                Next

            End While

            If C > 0 Then
                TabelaPolos.Controls.Add(row1)
                TabelaPolos.Controls.Add(row2)
                'TabelaPolos.Controls.Add(row3)
            End If
            ' Encerra a leitura
            RS.Close()
            RS.Dispose()
            MeuComando.Dispose()
            MyC.Dispose()

        Catch ex As Exception
            OK = "Erro na leitura dos dados das escolas. " & vbCrLf & ex.Message
        End Try
        If OK <> "OK" Then MensagemERRO.Text = TextoVermelho(OK)
    End Sub


    Public Sub CarregaColunaDeBotao(ByRef QueCELL As TableCell, ByRef QueClasse As String,
                                    ByRef QueID As String, ByRef QueImagem As String, ByRef QueToolTip As String, QueTipo As String, FlgEnabled As Boolean)
        Dim MeuBotao = New ImageButton
        MeuBotao.ImageUrl = "~/FIGURAS/" & QueImagem
        MeuBotao.ToolTip = QueToolTip
        MeuBotao.Enabled = FlgEnabled
        Dim Largura As Unit = New Unit(10, UnitType.Pixel)
        QueCELL.Width = Largura
        QueCELL.Controls.Add(MeuBotao)
        QueCELL.Attributes.Add("onclick", MakeJavaScriptPostBack(QueClasse, QueTipo & ";" & QueID))
        QueCELL.Attributes.Add("style", "cursor:pointer")
    End Sub

    'Public Sub CarregaColunaDeBotaoComFlag(ByRef QueCELL As TableCell, ByRef QueClasse As String,
    '                                ByRef QueID As String, ByRef QueImagem As String, ByRef QueToolTip As String, QueTipo As String, FlgEnabled As Boolean)
    '    Dim MeuBotao = New ImageButton
    '    MeuBotao.ImageUrl = "~/FIGURAS/" & QueImagem
    '    MeuBotao.ToolTip = QueToolTip
    '    MeuBotao.Enabled = FlgEnabled
    '    Dim Largura As Unit = New Unit(10, UnitType.Pixel)
    '    QueCELL.Width = Largura
    '    QueCELL.Controls.Add(MeuBotao)
    '    QueCELL.Attributes.Add("onclick", MakeJavaScriptPostBack(QueClasse, QueTipo & ";" & QueID))
    '    QueCELL.Attributes.Add("style", "cursor:pointer")
    'End Sub


    Protected Sub PreparaAgendamentoCheio(ByRef QueProva As String, ByRef QueAplicador As String,
                                          ByRef QueRow1 As TableRow, ByRef QueRow2 As TableRow, ByRef QueRow3 As TableRow)
        Dim LL1 As Int16() = {10, 10, 10, 10, 10}
        Dim I As Int16 = QueRow1.Cells.Count()
        Call CarregaColunasDeTextoNumaLinha(QueRow1, QueAplicador, LL1)
        QueRow1.Cells(I).ColumnSpan = 4

    End Sub

    Protected Sub ColoreColunas(ByRef QueRow As TableRow, ByRef QueCor As Drawing.Color, ByRef QueColunaInicial As Int16, ByRef QueColunaFinal As Int16)
        For k = QueColunaInicial To QueColunaFinal
            QueRow.Cells(k).BackColor = QueCor
        Next
    End Sub

    Protected Sub CarregaColunaDeControle(ByRef QueRow As TableRow)
        Dim Inicio = QueRow.Cells.Count
        Dim MeuBotao As UserControlAgendamento
        MeuBotao = New UserControlAgendamento
        'MeuBotao.ImageUrl = "~/FIGURAS/" & QueImagem
        'MeuBotao.ToolTip = QueToolTip
        Dim cell = New TableCell
        Dim Largura As Unit = New Unit(160, UnitType.Pixel)
        'MeuBotao.SetJavaScriptEditar("javascript: alert('Here we go!')")
        cell.Width = Largura
        cell.Controls.Add(MeuBotao)
        cell.Attributes.Add("style", "cursor:pointer")
        QueRow.Controls.Add(cell)
    End Sub

    Protected Function Pauta(ByRef C As Int32) As Boolean
        Return C Mod 4 = 1 Or C Mod 4 = 2
    End Function


    Protected Function PreparaLinhaCompleta() As TableRow
        Dim Row As New TableRow
        Call CarregaNcolunasEmBranco(Row, mcOffSet, {40, 30, 80})
        For k = 1 To mcNumDatas
            Call CarregaNcolunasEmBranco(Row, 1, {2})  ' Separadora
            Call CarregaNcolunasEmBranco(Row, 1, {10})  ' Separadora
            Call Carrega3Colunas(Row)
            Call CarregaNcolunasEmBranco(Row, 1, {10})  ' Separadora
            Call CarregaNcolunasEmBranco(Row, 1, {2})  ' Separadora
        Next
        ' Coloca a coluna final, não utilizada, apenas para comprimir a página
        Call CarregaNcolunasEmBranco(Row, 1, {100})
        Row.Height = 10
        Return Row
    End Function


    Public Sub CarregaNcolunasEmBranco(ByRef QueRow As TableRow, ByRef QuantasColunas As Int16, ByRef QueLL As Int16())
        Dim Cell As TableCell
        Dim Inicio = QueRow.Cells.Count

        For k As Int16 = 1 To QuantasColunas
            Cell = New TableCell With {.Text = ""}
            Dim Largura As Unit = New Unit(QueLL(k - 1), UnitType.Pixel)
            Cell.Width = Largura
            QueRow.Controls.Add(Cell)
        Next
    End Sub


    Protected Function IndexOfCOmpleta(ByRef QueOrdem As Int16) As Int16
        Return (QueOrdem - 1) * mcNumColunasAgendamento + mcOffSet + 2  ' Tem que pular as duas separadoras
    End Function

    Protected Sub Carrega3Colunas(ByRef QueRow As TableRow)
        Dim Cell As TableCell
        For k = 1 To 3
            Cell = New TableCell With {.Text = ""}
            Dim Largura As Unit = New Unit(IIf(k <> 1, 5, 160), UnitType.Pixel)
            Cell.Width = Largura
            QueRow.Controls.Add(Cell)
        Next
    End Sub





    Protected Sub VoltaParaQuemChamou()
        ' Volta para quem chamou
        Response.Redirect(ResolveUrl("~/POLOS.aspx" & MontaQueryStringFromParametros("AGENDAMENTO", "POLOS", Parametros)) & "&tipo=escolaagendamento")
    End Sub

    Protected Sub cmdVoltar_Click(sender As Object, e As EventArgs) Handles cmdVoltar.Click
        If mvPaginaAtrasada Then Exit Sub
        Call VoltaParaQuemChamou()
    End Sub
End Class