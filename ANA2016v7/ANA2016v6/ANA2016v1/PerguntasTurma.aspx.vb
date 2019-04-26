Imports System.Data
Imports System.Linq


Public Class PerguntasTurma
    Inherits System.Web.UI.Page

    Dim mvPaginaAtrasada As Boolean = False
    Dim Parametros As String()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load



        '' Se houver um erro na submissão da página (query string inválida ou  página atrasada), isso ignora eventos posteriores
        'mvPaginaAtrasada = False

        '' Verifica se a query string está OK e se as credenciais são atendidas
        Dim tmp As String = ConsisteQueryString("CadastroPOLO", Request, CompletaTamanhoMascara("SSSSS"), Parametros, IsPostBack, True)
        If tmp <> "OK" Then
            MensagemERRO.Text = TextoVermelho(tmp)
            mvPaginaAtrasada = True
            Exit Sub
        End If


        ' Verifica se o parâmetro chegou corretamente
        Dim message As String = Request.QueryString("turma")
        If message = Nothing Then
            Response.Redirect(ResolveUrl("~/BemVindo.aspx" & MontaQueryStringFromParametros("AtribuiFuncao", "BemVindo", Parametros)))
            Exit Sub
        End If

        ' Note que o parâmetro montado na página POLOS deve ter incluído a vírgula
        Dim x As String() = Split(message, ",")
        If UBound(x) < 1 Then
            MensagemERRO.Text = TextoVermelho("Houve um erro de sistema na passagem da parâmetros. ")
        Else
            CampoCO_CENSO_TURMA.Text = x(0)
            CampoTurma.Text = "" & x(0) & " : " & x(1)
        End If




        'Preenche os valores previstos e valores confirmados dos campos 
        If (Not IsPostBack()) Then
            Call CarregarGrupoAplicacao()


        End If










    End Sub

    Private Sub CarregarGrupoAplicacao()

        Dim id_turma As String = Split(Request.QueryString("turma"), ",")(0)

        Using SqlC As New SqlClient.SqlConnection(Ligacao())

            Dim sSql As String = "select   GA.ID_GRUPO_APLIC,  IsNull(QT_ALU,0) QT_ALU ,  NU_CONFIRMADOS  ,  " &
            " IsNull(NU_NAO_PREVISTOS,0) NU_NAO_PREVISTOS ,  FLG_CONFIRMADO   from GRUPO_APLIC GA  " &
            " LEFT OUTER JOIN TURMA_GRUPO_APLICACAO TC on TC.ID_GRUPO_APLIC = GA.ID_GRUPO_APLIC  and CO_TURMA_CENSO=@CO_TURMA_CENSO " &
            " INNER JOIN TURMA T ON T.CO_TURMA_CENSO=@CO_TURMA_CENSO order by 1"

            Using Sda As New SqlClient.SqlDataAdapter(sSql, SqlC)

                'Adiciona os parametros
                Sda.SelectCommand.Parameters.AddWithValue("@CO_TURMA_CENSO", id_turma)

                'Declara o datatable que hospedará os registros do banco 
                Dim dt As DataTable = New DataTable()
                'Carrega o datatable 
                Sda.Fill(dt)

                'Para cada registro
                For Each r As DataRow In dt.Rows

                    'Se for o ID 8 (Total de Alunos) preenche o total de alunos previstos da turma e os campos de não previstos
                    If (r("ID_GRUPO_APLIC") = "8") Then
                        txtTotalTurma.Text = r("QT_ALU")
                    Else 'preenche o respectivo controle


                        Dim txtPrev As TextBox = divcontroles.FindControl("txtPrevistos" & r("ID_GRUPO_APLIC"))
                        'verifica se encontrou a respectiva coluna (evita erros se adicionarem no futuro algum grupo não previsto na tela
                        If (Not IsNothing(txtPrev)) Then
                            txtPrev.Text = r("QT_ALU")
                        End If

                        'Confirmados minimo 0 máximo o valor total previsto 
                        Dim cboConf As DropDownList = divcontroles.FindControl("cmbConfirmados" & r("ID_GRUPO_APLIC"))
                        Dim cboNaoPrevisto As DropDownList = divcontroles.FindControl("cmbNaoPrev" & r("ID_GRUPO_APLIC"))


                        'preenche os valores possíveis 
                        ObterItensDropDown(cboConf, r("QT_ALU"), 0)
                        ObterItensDropDown(cboNaoPrevisto, 0, 20)


                        'Seta o valor que veio do banco 
                        If (Not IsDBNull(r("NU_CONFIRMADOS")) And (Not IsNothing(cboConf))) Then
                            cboConf.SelectedValue = r("NU_CONFIRMADOS")
                        End If


                        If (Not IsDBNull(r("NU_NAO_PREVISTOS")) And (Not IsNothing(cboNaoPrevisto))) Then
                            cboNaoPrevisto.SelectedValue = r("NU_NAO_PREVISTOS")
                        End If


                        If (Not IsDBNull(r("FLG_CONFIRMADO")) AndAlso r("FLG_CONFIRMADO") = "S") Then
                            cmdEntrar.Attributes.Add("data-conf", "true")
                        End If


                    End If

                Next


            End Using


            'Combos especiais (tabela  TURMA)  e combo de horários 
            'Preenche as opções de horários disponíveis no combo de horarios 
            For Each horario As DataRow In ObterTurnos().Rows
                cmbHorario.Items.Add(New ListItem(horario("DS_TURNO") & " - " & horario("DS_HORARIO"), horario("ID_TURNO_HORARIO")))
                cmbHorario2.Items.Add(New ListItem(horario("DS_TURNO") & " - " & horario("DS_HORARIO"), horario("ID_TURNO_HORARIO")))
            Next




            Dim sSqlExp As String = " SELECT ta.ID_TURNO_HORARIO ID_HORARIO1 , ta2.ID_TURNO_HORARIO ID_HORARIO2 from TURMA t  " &
                                    " LEFT JOIN TURMA_APLICACAO ta on t.CO_TURMA_CENSO = ta.CO_TURMA_CENSO And ta.ID_PROVA=1 left join TURMA_APLICACAO ta2 on t.CO_TURMA_CENSO = ta2.CO_TURMA_CENSO And ta2.ID_PROVA=2 " &
                                    " WHERE t.CO_TURMA_CENSO=@CO_TURMA_CENSO "

            Using Sda As New SqlClient.SqlDataAdapter(sSqlExp, SqlC)




                'Adiciona os parametros
                Sda.SelectCommand.Parameters.AddWithValue("@CO_TURMA_CENSO", id_turma)
                Dim dtEspeciais As DataTable = New DataTable()
                Sda.Fill(dtEspeciais)

                If (dtEspeciais.Rows.Count > 0) Then
                    Dim registro As DataRow = dtEspeciais.Rows(0)

                    If (Not IsDBNull(registro("ID_HORARIO1"))) Then 'Turno dia 1  preenchido, setar o valor do combo
                        cmbHorario.SelectedValue = registro("ID_HORARIO1")
                    End If
                    If (Not IsDBNull(registro("ID_HORARIO2"))) Then 'Turno dia 2 preenchido, setar o valor do combo
                        cmbHorario2.SelectedValue = registro("ID_HORARIO2")
                    End If


                End If

            End Using

        End Using

    End Sub

    Protected Sub ObterItensDropDown(ByRef cbo As DropDownList, v1 As Integer, v2 As Integer)
        If (IsNothing(cbo)) Then Exit Sub

        If (v1 < v2) Then 'Crescente

            Do While v1 < v2
                cbo.Items.Add(New ListItem(v1, v1))
                v1 += 1
            Loop
        Else
            Do While v1 >= v2 'Decrescente
                cbo.Items.Add(New ListItem(v1, v1))
                v1 -= 1
            Loop
        End If

    End Sub

    Protected Sub cmdCancelar_Click(sender As Object, e As EventArgs) Handles cmdCancelar.Click
        If mvPaginaAtrasada Then Exit Sub
        Call VoltaParaQuemChamou()
    End Sub

    Protected Sub VoltaParaQuemChamou()

        Dim message As String = Request.QueryString("escola")
        Response.Redirect(ResolveUrl("~/AGENDAMENTO.aspx" & MontaQueryStringFromParametros("PerguntasTurma", "AGENDAMENTO", Parametros) & "&escola=" & message & ",U"))
    End Sub











    'SALVAR 
    Private Sub cmdEntrar_Click(sender As Object, e As EventArgs) Handles cmdEntrar.Click



        Using SqlC As New SqlClient.SqlConnection(Ligacao())
            SqlC.Open()

            Dim id_turma As String = Split(Request.QueryString("turma"), ",")(0)


            'Deleta todas as salas não previstas
            'Deleta as antigas (flag nao prevista = 'S') para todos os dias 
            Dim sdel As String = "DELETE TURMA_SALA WHERE CO_TURMA_CENSO= @CO_TURMA_CENSO And FLG_NAO_PREVISTA='S' "
            Using Cmd As New SqlClient.SqlCommand(sdel, SqlC)
                Cmd.Parameters.AddWithValue("@CO_TURMA_CENSO", id_turma)
                Cmd.ExecuteNonQuery()
            End Using


            'GRAVA os horários (combos) selecionados 
            Dim sqlHorarios = " UPDATE TURMA_APLICACAO SET ID_TURNO_HORARIO=@ID_HORARIO1 where CO_TURMA_CENSO=@CO_TURMA_CENSO and ID_PROVA=1 " &
                              " UPDATE TURMA_APLICACAO SET ID_TURNO_HORARIO=@ID_HORARIO2 where CO_TURMA_CENSO=@CO_TURMA_CENSO And ID_PROVA=2"
            Using Cmd As New SqlClient.SqlCommand(sqlHorarios, SqlC)
                Cmd.Parameters.AddWithValue("@CO_TURMA_CENSO", id_turma)
                Cmd.Parameters.AddWithValue("@ID_HORARIO1", cmbHorario.SelectedValue)
                Cmd.Parameters.AddWithValue("@ID_HORARIO2", cmbHorario2.SelectedValue)
                Cmd.ExecuteNonQuery()
            End Using



            'Carrega a lista de grupos aplicacao ( vai fazer um loop salvando ) 
            Dim dtGrupos As New DataTable()
            Using sqlDa As New SqlClient.SqlDataAdapter("select id_grupo_aplic, ds_grupo_aplic, ga.qt_max_aluno  , gs.ID_GRUPO_SALA , gs.DS_GRUPO_SALA from GRUPO_APLIC ga  left outer join grupo_sala gs on gs.ID_GRUPO_SALA=ga.id_grupo_sala  order by 1", SqlC)
                sqlDa.Fill(dtGrupos)
            End Using


            'Para cada grupo aplicação  Grava o Valor e em seguida cria a turma sala caso necessário 
            For Each RegGrupoAplicacao In dtGrupos.Rows


                'Encontra os combos referentes ao grupo aplicação 
                Dim id_grupo As Integer = RegGrupoAplicacao("ID_GRUPO_APLIC")
                Dim ds_grupo As String = RegGrupoAplicacao("ds_grupo_aplic")
                Dim qt_max_aluno = RegGrupoAplicacao("qt_max_aluno")

                'Caso não encontre o combo por não existir. Ele cria um FAKE pare ter o valor ZERO 
                Dim cboConf As DropDownList = divcontroles.FindControl("cmbConfirmados" & id_grupo)
                If (IsNothing(cboConf)) Then
                    cboConf = New DropDownList()
                    cboConf.Items.Add("0")
                End If


                'Caso não encontre o combo por não existir. Ele cria um FAKE pare ter o valor ZERO 
                Dim cboNaoPrevisto As DropDownList = divcontroles.FindControl("cmbNaoPrev" & id_grupo)
                If (IsNothing(cboNaoPrevisto)) Then
                    cboNaoPrevisto = New DropDownList()
                    cboNaoPrevisto.Items.Add("0")
                End If

                'idem para os previstos
                Dim txtPrevistos As TextBox = divcontroles.FindControl("txtPrevistos" & id_grupo)
                If (IsNothing(txtPrevistos)) Then
                    txtPrevistos = New TextBox()
                    txtPrevistos.Text = "0"
                End If



                'Query para gravar o valor da resposta
                Dim sSql As String = "" &
                " If EXISTS(Select * from TURMA_GRUPO_APLICACAO  where CO_TURMA_CENSO = @CO_TURMA_CENSO And ID_GRUPO_APLIC = @ID_GRUPO_APLIC)  BEGIN 	 " &
                " update TURMA_GRUPO_APLICACAO Set NU_CONFIRMADOS=@NU_CONFIRMADOS , NU_NAO_PREVISTOS = @NU_NAO_PREVISTOS where CO_TURMA_CENSO=@CO_TURMA_CENSO And ID_GRUPO_APLIC=@ID_GRUPO_APLIC " &
                " End  " &
                " Else BEGIN	 " &
                " insert into TURMA_GRUPO_APLICACAO (CO_TURMA_CENSO,ID_GRUPO_APLIC,DS_GRUPO_APLIC, QT_ALU, NU_CONFIRMADOS, NU_NAO_PREVISTOS, QT_APLIC)  " &
                " values( @CO_TURMA_CENSO, @ID_GRUPO_APLIC, (Select top 1 DS_GRUPO_APLIC from GRUPO_APLIC where ID_GRUPO_APLIC=@ID_GRUPO_APLIC) , 0 , @NU_CONFIRMADOS, @NU_NAO_PREVISTOS, 1) " &
                " End "


                'GRAVA o valor da resposta na tabela TURMA_GRUPO_APLICACAO
                Using Cmd As New SqlClient.SqlCommand(sSql, SqlC)
                    Cmd.Parameters.AddWithValue("@CO_TURMA_CENSO", id_turma)
                    Cmd.Parameters.AddWithValue("@ID_GRUPO_APLIC", id_grupo)
                    Cmd.Parameters.AddWithValue("@NU_CONFIRMADOS", cboConf.Text)
                    Cmd.Parameters.AddWithValue("@NU_NAO_PREVISTOS", cboNaoPrevisto.Text)
                    Call Cmd.ExecuteNonQuery()
                End Using





                Dim totalFinalTurma As Integer = CInt(cboNaoPrevisto.SelectedValue)
                Dim totalNaoConfirmado As Integer = CInt(txtPrevistos.Text) - CInt(cboConf.SelectedValue)
                Dim totalOriginal As Integer = txtPrevistos.Text

                'Antes de mais nada vou precisar de uma lista de turmas existentes 
                Dim strQueryTurmasExistentes As String = "Select ID_TURMA_SALA,  ID_PROVA,  NU_CAPACIDADE_ALUNOS, NU_ALUNOS_ALOCADOS , IsNULL(NU_ALUNOS_NAOPREVISTOS,0) NU_ALUNOS_NAOPREVISTOS, IsNull(NU_ALUNOS_NAOCONFIRMADOS,0) NU_ALUNOS_NAOCONFIRMADOS  , NU_CAPACIDADE_ALUNOS - NU_ALUNOS_ALOCADOS as VAGAS from TURMA_SALA   where FLG_ATIVA <> 'N' AND  CO_TURMA_CENSO = @CO_TURMA_CENSO and FLG_NAO_PREVISTA='N' and ID_GRUPO_APLIC = @ID_GRUPO_APLIC    "
                Dim dtTurmasExistentes As New DataTable
                Using sda As New SqlClient.SqlDataAdapter(strQueryTurmasExistentes, SqlC)
                    sda.SelectCommand.Parameters.AddWithValue("@CO_TURMA_CENSO", id_turma)
                    sda.SelectCommand.Parameters.AddWithValue("@ID_GRUPO_APLIC", id_grupo)
                    sda.Fill(dtTurmasExistentes)
                End Using






                For id_prova As Integer = 1 To 2 'Para cada prova... 

                    If (totalNaoConfirmado > 0) Then 'Existem alunos para excluir (setar campo de total não confirmado da turma sala) 
                        '****************************************************************************************************************************************************************************************************************
                        'Inicio trecho para REMOVER os alunos NÃO CONFIRMADOS das turmas!!! *********************************************************************************************************************************************
                        '****************************************************************************************************************************************************************************************************************
                        'Se o TOTAL não confirmado for superior a zero, teremos que remover da turma sala a quantidade de alunos necessárias. 
                        'Para isto será realizado um UPDATE com a quantidade de alunos não confirmados. 
                        'Turmas individuis, remover turmas de acordo com a quantidade de não confirmados.
                        'Turmas coletivas, ir diminuindo até que a turma esteja zerada ( ai passa para a próxima turma, caso tente diminuir de uma turma não existente será falha de integridade do banco
                        If (qt_max_aluno = 1) Then 'turma individual. Update direto! 
                            Dim strIndividuais As String = " with base as ( Select ID_TURMA_SALA , NU_ALUNOS_NAOCONFIRMADOS,  row_number()  over ( partition by co_turma_censo, id_grupo_aplic, id_prova order by SEQ_TURMA_SALA , NU_ALUNOS_NAOCONFIRMADOS DESC )  LINHA   from TURMA_SALA   " &
                                                           " where FLG_ATIVA <> 'N' AND  CO_TURMA_CENSO = @CO_TURMA_CENSO and FLG_NAO_PREVISTA='N' and ID_GRUPO_APLIC = @ID_GRUPO_APLIC  AND ID_PROVA=@ID_PROVA ) " &
                                                           " update base set NU_ALUNOS_NAOCONFIRMADOS = 1"

                            Using cmd As New SqlClient.SqlCommand(strIndividuais, SqlC)
                                cmd.Parameters.AddWithValue("@CO_TURMA_CENSO", id_turma)
                                cmd.Parameters.AddWithValue("@ID_GRUPO_APLIC", id_grupo)
                                cmd.Parameters.AddWithValue("@ID_PROVA", id_prova)
                                cmd.ExecuteNonQuery()
                            End Using


                        Else 'turmas coletivas


                            Dim QuantidadePendenteParaRemover As Integer = totalNaoConfirmado 'faz uma cópia da variável pois vamos precisar dela original mais tarde

                            For Each rExistente As DataRow In dtTurmasExistentes.Rows     'para cada turma atual..
                                If rExistente("ID_PROVA") <> id_prova Then Continue For 'um dia por vez



                                If (QuantidadePendenteParaRemover <= 0) Then Exit For 'se já tiver preenchido tudo que precisou

                                Dim strSqlUpdateexistente = "update turma_sala set NU_ALUNOS_NAOCONFIRMADOS = @NU_ALUNOS_NAOCONFIRMADOS where ID_TURMA_SALA= @ID_TURMA_SALA"
                                Using cmd As New SqlClient.SqlCommand(strSqlUpdateexistente, SqlC)
                                    cmd.Parameters.AddWithValue("@ID_TURMA_SALA", rExistente("ID_TURMA_SALA"))
                                    cmd.Parameters.AddWithValue("@NU_ALUNOS_NAOCONFIRMADOS", IIf(QuantidadePendenteParaRemover > qt_max_aluno, qt_max_aluno, QuantidadePendenteParaRemover))
                                    cmd.ExecuteNonQuery()
                                End Using

                                QuantidadePendenteParaRemover = QuantidadePendenteParaRemover - qt_max_aluno

                            Next
                        End If
                    End If
                    '****************************************************************************************************************************************************************************************************************
                    'FIM DO TRECHO PARA REMOVER OS ALUNOS NÃO CONFIRMADOS DAS TURMAS!!! *********************************************************************************************************************************************
                    '****************************************************************************************************************************************************************************************************************



                    '****************************************************************************************************************************************************************************************************************
                    'INICIO DO TRECHO PARA CRIAR TURMAS NOVAS  **********************************************************************************************************************************************************************
                    '****************************************************************************************************************************************************************************************************************
                    'Cria as salas extras de acordo com a necessidade (Query para inserir o TURMA SALA nos 2 dias 

                    Dim totalFinalPendente = totalFinalTurma 'cria uma copia da variavel pois será utilizada para as 2 provas  

                    Do While totalFinalPendente > 0

                        'Verifica se existe uma turma (prevista) com capacidade para receber os alunos restantes
                        Dim turmaDisponivel = dtTurmasExistentes.Select("id_prova = '" & id_prova & "' and VAGAS >= '" & totalFinalPendente & "'  ")

                        If (turmaDisponivel.Count > 0) Then 'Existe turma disponivel!! Não vai criar turma sala 

                            Dim vagas As Integer = turmaDisponivel(0)("VAGAS") 'Vê quantas vagas tem disponíveis na turma 
                            Dim QtdInseridosTurmaExistente As Integer = IIf(totalFinalPendente >= vagas, vagas, totalFinalPendente)  'Define quantos serão inseridos 
                            Dim strupd As String = "Update turma_sala set NU_ALUNOS_NAOPREVISTOS = @NU_ALUNOS_NAOPREVISTOS where ID_TURMA_SALA = @ID_TURMA_SALA " 'monta a query de update 
                            Using cmd As New SqlClient.SqlCommand(strupd, SqlC) 'executa o update 
                                cmd.Parameters.AddWithValue("@ID_TURMA_SALA", turmaDisponivel(0)("ID_TURMA_SALA"))
                                cmd.Parameters.AddWithValue("@NU_ALUNOS_NAOPREVISTOS", QtdInseridosTurmaExistente)
                                cmd.ExecuteNonQuery()
                            End Using
                            totalFinalPendente = totalFinalPendente - QtdInseridosTurmaExistente 'subtrai do total o que ele inseriu 

                        Else 'Não há turma disponível criar turma sala novo 

                            Dim strIns As String = "" &
                            " DECLARE @SEQ_TURMA_SALA INT = ( Select MAX(SEQ_TURMA_SALA) +1 FROM TURMA_SALA WHERE CO_TURMA_CENSO=@CO_TURMA_CENSO And ID_PROVA= @ID_PROVA And FLG_ATIVA <> 'N' )  " &
                            " INSERT INTO TURMA_SALA (ID_ESCOLA, CO_TURMA_CENSO, SEQ_TURMA_SALA, ID_PROVA, ID_GRUPO_APLIC, DS_GRUPO_APLIC, NU_CAPACIDADE_ALUNOS, NU_ALUNOS_ALOCADOS, FLG_NAO_PREVISTA, NU_ALUNOS_NAOCONFIRMADOS, NU_ALUNOS_NAOPREVISTOS, FLG_ATIVA)   " &
                            " VALUES(@ID_ESCOLA, @CO_TURMA_CENSO, @SEQ_TURMA_SALA, @ID_PROVA,  @ID_GRUPO_APLIC, @DS_GRUPO_APLIC, @NU_CAPACIDADE_ALUNOS, @NU_ALUNOS_ALOCADOS, 'S' ,				@NU_ALUNOS_NAOCONFIRMADOS,   0  , 'S')     "

                            Using Cmd As New SqlClient.SqlCommand(strIns, SqlC)
                                Cmd.Parameters.AddWithValue("@CO_TURMA_CENSO", id_turma)
                                Cmd.Parameters.AddWithValue("@NU_ALUNOS_ALOCADOS", IIf(totalFinalPendente <= qt_max_aluno, totalFinalPendente, qt_max_aluno))
                                Cmd.Parameters.AddWithValue("@ID_GRUPO_APLIC", id_grupo)
                                Cmd.Parameters.AddWithValue("@DS_GRUPO_APLIC", ds_grupo)
                                Cmd.Parameters.AddWithValue("@NU_CAPACIDADE_ALUNOS", qt_max_aluno)
                                Cmd.Parameters.AddWithValue("@NU_ALUNOS_NAOCONFIRMADOS", totalNaoConfirmado)
                                Cmd.Parameters.AddWithValue("@NU_ALUNOS_NAOPREVISTOS", totalFinalPendente)
                                Cmd.Parameters.AddWithValue("@ID_PROVA", id_prova)
                                Cmd.Parameters.AddWithValue("@ID_GRUPO_SALA", RegGrupoAplicacao("ID_GRUPO_SALA"))
                                Cmd.Parameters.AddWithValue("@DS_GRUPO_SALA", RegGrupoAplicacao("DS_GRUPO_SALA"))
                                Cmd.Parameters.AddWithValue("@ID_ESCOLA", Request.QueryString("escola"))
                                Cmd.ExecuteNonQuery()

                            End Using

                            totalFinalPendente = totalFinalPendente - qt_max_aluno
                        End If

                    Loop

                Next 'Próxima prova 

            Next 'Próximo grupo aplic 



            'Salva a confirmação no TURMA_SALA 
            Dim strSqlConfrm = "UPDATE TURMA SET FLG_CONFIRMADO='S' where CO_TURMA_CENSO=@CO_TURMA_CENSO"
            Using cmd As New SqlClient.SqlCommand(strSqlConfrm, SqlC)
                cmd.Parameters.AddWithValue("@CO_TURMA_CENSO", id_turma)
                cmd.ExecuteNonQuery()
            End Using


        End Using



        Call VoltaParaQuemChamou()



    End Sub


    ''' <summary>
    ''' Obtem os turnos para preenchimento dos combos na tela de PerguntasTurma
    ''' </summary>
    Protected Function ObterTurnos() As DataTable
        'Cria a tabela 
        Dim dt_turno_horario As New DataTable

        Dim strSql As String = "select ID_TURNO_HORARIO, DS_TURNO, DS_HORARIO from TURNO_HORARIO"
        Using SqlC As New SqlClient.SqlConnection(Ligacao())
            Using Sda As New SqlClient.SqlDataAdapter(strSql, SqlC)
                Sda.Fill(dt_turno_horario)
            End Using
        End Using
        Return dt_turno_horario

    End Function







End Class