Imports System.Linq

Public Class MesclarTurmas

    Inherits System.Web.UI.Page

    Dim Parametros As String()





    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Parametros = Split(Request.QueryString("escola"), ",")

        Dim id_escola = Parametros(0)
        Dim id_dia = Parametros(1)
        Dim id_prova = Parametros(2)

        If (Parametros.Length <> 3) Then
            MensagemERRO.Text = TextoVermelho("Parametros passados incorretamente")
            Exit Sub
        End If

        ' Verifica se a query string está OK e se as credenciais são atendidas
        Dim tmp As String = ConsisteQueryString("Agrupamento", Request, CompletaTamanhoMascara("SSSSS"), Parametros, IsPostBack, True)
        If tmp <> "OK" Then
            MensagemERRO.Text = TextoVermelho(tmp)
            Exit Sub
        End If


        'Configura a figura de acordo com o idprova 
        imgD1.Src = IIf(id_prova = 1, "FIGURAS/FigDia1.png", "FIGURAS/FigDia2.png")
        lblAplicacao.Text = Dias(id_dia) 'prenche o dia 

        'carrega o grid 
        CarregaTurmas()



    End Sub


    Protected Sub CarregaTurmas()


        Parametros = Split(Request.QueryString("escola"), ",")
        Dim id_escola = Parametros(0)
        Dim id_dia_aplicacao = Parametros(1)
        Dim id_prova = Parametros(2)







        Using SqlC As New SqlClient.SqlConnection(Ligacao())


            Dim dtCabecalho As DataTable = New DataTable() 'cria a tabela que armazenará a lista de cabecalhos 

            'Carrega os itens previsos
            Dim strSql As String = "select tr.DS_TURNO  TURNO , tr.TURNO_BLOQUEIO_APLICADOR , @ID_DIA_APLICACAO  ID_DIA_APLICACAO , @ID_ESCOLA ID_ESCOLA , @ID_PROVA as ID_PROVA , NO_ESCOLA from TURNO_HORARIO tr inner join escola e on e.ID_ESCOLA=@id_escola	group by ds_turno, TURNO_BLOQUEIO_APLICADOR, NO_ESCOLA	order by min(ds_horario)" 'Faz um select com os períodos para preencher as colunas
            Using sqlDc As New SqlClient.SqlDataAdapter(strSql, SqlC)

                sqlDc.SelectCommand.Parameters.AddWithValue("@ID_ESCOLA", id_escola)
                sqlDc.SelectCommand.Parameters.AddWithValue("@ID_PROVA", id_prova)
                sqlDc.SelectCommand.Parameters.AddWithValue("ID_DIA_APLICACAO", id_dia_aplicacao)


                sqlDc.Fill(dtCabecalho)
                rptCabecalho.DataSource = dtCabecalho
                rptCabecalho.DataBind()

                lblEscola.Text = dtCabecalho.Rows(0)("NO_ESCOLA")

            End Using



            'Calcula POR tipo a quantidade total de alunos para gerar as novas turmas 


            'Gera uma lista de controles
            Dim lstControles As List(Of ctlQuadroMesclagemTopo) = ObtemListaCabecalho()




            'verifica se há itens extras para não gerar processamento atoa caso não tenha, além de exibir a mensagem amigável de nenhum item encontrado caso não haja.
            If (lstControles.Count = 0) Then
                lblVazio.Visible = True
            Else
                Dim indice = 0

                'Agrupa as turmas por horário e em seguida pela mesma característica de necessidade especial 
                For Each rcol As DataRow In dtCabecalho.Rows 'para cada coluna (turno) ...

                    For _id_grupo_sala As Integer = 3 To 4 ' para cada grupo sala...

                        Dim _id_g = _id_grupo_sala

                        Dim controlesColuna = lstControles.Where(Function(f) f.TURNO = rcol("TURNO") And f.ID_GRUPO_SALA = _id_g) 'filtra todos os controles da coluna / grupo sala 

                        If (controlesColuna.Count > 0) Then 'Caso tenha algum controle... 

                            Dim qtAlunos = controlesColuna.Sum(Function(o) o.QTD_ALUNOS) 'Carrega a quantidade TOTAL de alunos ( sum (qtd_alunos)  ) 

                            Dim qtMaxAlunos = controlesColuna.First().QTD_CAPACIDADE



                            'Detecta quantas turmas vai precisar.
                            Dim qtTurmasFinal As Integer = Math.Ceiling(qtAlunos / qtMaxAlunos) 'arrendonda pra cima.



                            Dim qtAlunosPorTurmaSemResto = Int(qtAlunos / qtTurmasFinal)
                            Dim qtAlunosSobra = qtAlunos Mod qtTurmasFinal


                            Dim ctlSeletor = TryCast(LoadControl("~/ctlQuadroMesclagemSeletor.ascx"), ctlQuadroMesclagemSeletor)
                            ctlSeletor.ID_GRUPO_SALA = _id_grupo_sala
                            ctlSeletor.ID_ESCOLA = id_escola
                            ctlSeletor.GRUPO_SALA = controlesColuna.First().GRUPO_SALA
                            ctlSeletor.CO_TURMA_CENSO = controlesColuna.First().CO_TURMA_CENSO

                            ctlSeletor.TURNO = rcol("TURNO")
                            ctlSeletor.TURNO_BLOQUEIO = rcol("TURNO_BLOQUEIO_APLICADOR")


                            'Adiciona as turmas finais 
                            For i As Integer = 0 To qtTurmasFinal - 1
                                Dim objNovaTurma As New ctlQuadroMesclagemSeletor.NovasTurmas
                                objNovaTurma.CAPACIDADE = qtMaxAlunos
                                objNovaTurma.ALOCADOS = qtAlunosPorTurmaSemResto
                                If (qtAlunosSobra > 0) Then 'caso o numero de alunos seja diferente em turmas
                                    objNovaTurma.ALOCADOS += 1
                                    qtAlunosSobra -= 1
                                End If
                                ctlSeletor.NOVAS_TURMAS.Add(objNovaTurma)

                            Next

                            'carrega as turmas originais 
                            For Each ctl As ctlQuadroMesclagemTopo In controlesColuna
                                ctlSeletor.TURMAS_ORIGINAIS.AddRange(ctl.ID_TURMA_SALA)
                            Next


                            ctlSeletor.Carrega()
                            Call trSeletores.Controls(indice).Controls.Add(ctlSeletor)


                        End If

                    Next

                    indice += 1

                Next

            End If


        End Using

    End Sub

    Protected Sub cmdEntrar_Click(sender As Object, e As EventArgs) Handles cmdEntrar.Click



        Parametros = Split(Request.QueryString("escola"), ",")
        Dim id_escola = Parametros(0)
        Dim id_dia_aplicacao = Parametros(1)
        Dim id_prova = Parametros(2)






        Dim objListaCabecalho As List(Of ctlQuadroMesclagemTopo) = ObtemListaCabecalho() 'carrega a lista de turmas do cabecalho 

        Dim objListaTurmasMortas As New List(Of Integer) 'cria uma lista de turmas que devem ser desativadas ao fim do processo 

        Using SqlC As New SqlClient.SqlConnection(Ligacao())
            SqlC.Open()


            'Limpa o agrupamento existente para a escola, prova , dia da operação  (CASO EXISTA)
            Dim strLimpeza As String = "" &
            " /* REMOVE TODAS AS SALAS AGRUPADAS (NOVAS) */ " &
            "   DELETE ts from TURMA_SALA ts  " &
            "   inner join CONTROLE_AGRUPAMENTO ct on ct.ID_CONTROLE_AGRUPAMENTO=ts.ID_CONTROLE_AGRUPAMENTO  " &
            "    where id_escola = @ID_ESCOLA And ct.ID_PROVA = @ID_PROVA And ID_DIA_APLICACAO = @ID_DIA_APLICACAO And FLG_AGRUPADA='S' " &
            " /* REVERTE AO ESTADO ORIGINAL AS SALAS ORIGINAIS  */ " &
            "   UPDATE ts set TS.ID_CONTROLE_AGRUPAMENTO = null , FLG_ATIVO='S' from TURMA_SALA ts  " &
            "   inner Join CONTROLE_AGRUPAMENTO ct on ct.ID_CONTROLE_AGRUPAMENTO=ts.ID_CONTROLE_AGRUPAMENTO  " &
            "   where ID_ESCOLA= @ID_ESCOLA and ct.ID_PROVA = @ID_PROVA and ID_DIA_APLICACAO = @ID_DIA_APLICACAO"
            Using cmd As New SqlClient.SqlCommand(strLimpeza, SqlC)
                cmd.Parameters.AddWithValue("@ID_ESCOLA", id_escola)
                cmd.Parameters.AddWithValue("@ID_PROVA", id_prova)
                cmd.Parameters.AddWithValue("@ID_DIA_APLICACAO", id_dia_aplicacao)
                cmd.ExecuteNonQuery()
            End Using

            'Gera um novo ID agrupamento (separado para não ficar muito confuso)
            Dim id_controle As Integer = 0 'Cria a variável com o controle de execução do agrupamento.
            Dim strControle As String = "INSERT INTO CONTROLE_AGRUPAMENTO ( DT_AGRUPAMENTO, ID_ESCOLA, ID_PROVA, ID_DIA_APLICACAO  ) VALUES ( GETDATE(), @ID_ESCOLA, @ID_PROVA, @ID_DIA_APLICACAO ) SELECT @@IDENTITY"
            Using cmd As New SqlClient.SqlCommand(strControle, SqlC)
                cmd.Parameters.AddWithValue("@ID_ESCOLA", id_escola)
                cmd.Parameters.AddWithValue("@ID_PROVA", id_prova)
                cmd.Parameters.AddWithValue("@ID_DIA_APLICACAO", id_dia_aplicacao)
                id_controle = cmd.ExecuteScalar()
            End Using





            For indice_turno As Integer = 0 To 4 'Para cada turno...

                Dim containerControles As Control = trSeletores.Controls(indice_turno)

                If (containerControles.Controls.Count > 0) Then


                    For Each objSeletor As ctlQuadroMesclagemSeletor In containerControles.Controls 'para cada seletor (deficiencia) dentro do container ...

                        'desativa as salas originais primeiro. (utilizado para ajustes na sequencia da turma) já retornando a sequencia da turma
                        For Each id_turma_original As Integer In objSeletor.TURMAS_ORIGINAIS.Distinct()
                            Using cmd As New SqlClient.SqlCommand("UPDATE TURMA_SALA SET FLG_ATIVO='N', ID_CONTROLE_AGRUPAMENTO=@ID_CONTROLE_AGRUPAMENTO WHERE ID_TURMA_SALA = @ID_TURMA_SALA and (FLG_AGRUPADA='N' or FLG_AGRUPADA is Null)  ", SqlC)
                                cmd.Parameters.AddWithValue("@ID_CONTROLE_AGRUPAMENTO", id_controle)
                                cmd.Parameters.AddWithValue("@ID_TURMA_SALA", id_turma_original)
                                cmd.ExecuteNonQuery()
                                Dim x = 0

                            End Using
                        Next

                        Dim rep As Repeater = objSeletor.FindControl("rptMain")

                        For Each objRepeaterItem As RepeaterItem In rep.Items 'para cada sala nova criada 
                            'Grava a nova sala no banco de dados juntamente com o aplicador selecionado
                            Dim combo As DropDownList = CType(objRepeaterItem.FindControl("cmbAplicadoresDisp"), DropDownList)
                            Dim lblCapacidade As Label = CType(objRepeaterItem.FindControl("lblCapacidade"), Label)
                            Dim lblAlocados As Label = CType(objRepeaterItem.FindControl("lblAlocados"), Label)



                            Dim strInsert As String = "declare @SEQ_TURMA_SALA int " &
                            " set @SEQ_TURMA_SALA= (select max(SEQ_TURMA_SALA)+1 from TURMA_SALA where CO_TURMA_CENSO=@CO_TURMA_CENSO And FLG_ATIVO='S' and ID_PROVA=@ID_PROVA )  " &
                            " insert into TURMA_SALA ( CO_TURMA_CENSO ,  SEQ_TURMA_SALA,  ID_PROVA,  ID_GRUPO_SALA , NU_CAPACIDADE_ALUNOS , NU_ALUNOS_ALOCADOS, FLG_NAO_PREVISTA , CPF_APLICADOR, FLG_ATIVO ,  ID_CONTROLE_AGRUPAMENTO, FLG_AGRUPADA)  " &
                            " values					(@CO_TURMA_CENSO, @SEQ_TURMA_SALA, @ID_PROVA, @ID_GRUPO_SALA, @NU_CAPACIDADE_ALUNOS, @NU_ALUNOS_ALOCADOS, 'N'			   ,@CPF_APLICADOR, 'S',	    @ID_CONTROLE_AGENDAMENTO, 'S')  "

                            Using cmd As New SqlClient.SqlCommand(strInsert, SqlC)

                                With cmd.Parameters

                                    .AddWithValue("@CO_TURMA_CENSO", objSeletor.CO_TURMA_CENSO)
                                    .AddWithValue("@ID_PROVA", id_prova)
                                    .AddWithValue("@ID_GRUPO_SALA", objSeletor.ID_GRUPO_SALA)
                                    .AddWithValue("@NU_CAPACIDADE_ALUNOS", lblCapacidade.Text)
                                    .AddWithValue("@NU_ALUNOS_ALOCADOS", lblAlocados.Text)
                                    .AddWithValue("@CPF_APLICADOR", combo.SelectedValue)
                                    .AddWithValue("@ID_CONTROLE_AGENDAMENTO", id_controle)
                                End With

                                Call cmd.ExecuteNonQuery()

                            End Using

                            'TODO: Executar um UPDATE no turma sala para refazer a sequencia pois pode ter ficado algum buraco ( Verificar com Pedro )



                        Next

                    Next

                End If

            Next

        End Using

        VoltaParaQuemChamou()



    End Sub



    Protected Function ObtemListaCabecalho() As List(Of ctlQuadroMesclagemTopo)

        Dim lstControles As New List(Of ctlQuadroMesclagemTopo)


        For Each coluna As Control In rptCabecalho.Controls

            For Each controle As Control In coluna.Controls

                If controle.GetType().Name = "ctlquadromesclagemtopo_ascx" Then 'só carrega os valores se os controles estiverem visíveis
                    Call DirectCast(controle, ctlQuadroMesclagemTopo).Carrega()
                    If (DirectCast(controle, ctlQuadroMesclagemTopo).QTD_ALUNOS > 0) Then lstControles.Add(controle)

                End If

            Next

        Next


        Return lstControles


    End Function






    Protected Sub cmdCancelar_Click(sender As Object, e As EventArgs) Handles cmdCancelar.Click
        VoltaParaQuemChamou()

    End Sub

    Protected Sub VoltaParaQuemChamou()

        Dim escola As String = Split(Request.QueryString("escola"), ",")(0)
        Response.Redirect(ResolveUrl("~/AGENDAMENTO?" & Request.QueryString("q") & "&escola=" & escola & ",U"))
    End Sub



End Class
