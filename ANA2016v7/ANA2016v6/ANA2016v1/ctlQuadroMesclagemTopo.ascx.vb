Public Class ctlQuadroMesclagemTopo
    Inherits System.Web.UI.UserControl


    Private _id_dia_aplicacao As Integer
    Public Property ID_DIA_APLICACAO() As Integer
        Get
            Return _id_dia_aplicacao
        End Get
        Set(ByVal value As Integer)
            _id_dia_aplicacao = value
        End Set
    End Property

    Private _co_turma_censo As String
    Public Property CO_TURMA_CENSO() As String
        Get
            Return _co_turma_censo
        End Get
        Set(ByVal value As String)
            _co_turma_censo = value
        End Set
    End Property


    Private _id_turma_sala As List(Of Integer) = New List(Of Integer)
    Public Property ID_TURMA_SALA() As List(Of Integer)
        Get
            Return _id_turma_sala
        End Get
        Set(ByVal value As List(Of Integer))
            _id_turma_sala = value
        End Set
    End Property

    Private _turno As String
    Public Property TURNO() As String
        Get
            Return _turno
        End Get
        Set(ByVal value As String)
            _turno = value
        End Set
    End Property


    Private _id_escola As Integer
    Public Property ID_ESCOLA() As Integer
        Get
            Return _id_escola
        End Get
        Set(ByVal value As Integer)
            _id_escola = value
        End Set
    End Property

    Private _id_grupo_sala As Integer
    Public Property ID_GRUPO_SALA() As Integer
        Get
            Return _id_grupo_sala
        End Get
        Set(ByVal value As Integer)
            _id_grupo_sala = value
        End Set
    End Property

    Private _dsc_grupo_sala As String
    Public Property GRUPO_SALA() As String
        Get
            Return _dsc_grupo_sala
        End Get
        Set(ByVal value As String)
            _dsc_grupo_sala = value
        End Set
    End Property

    Private _qtd_turmas As Integer = 0
    Public Property QTD_TURMAS() As Integer
        Get
            Return _qtd_turmas
        End Get
        Set(ByVal value As Integer)
            _qtd_turmas = value
        End Set
    End Property

    Private _qtd_alunos As Integer = 0
    Public Property QTD_ALUNOS() As Integer
        Get
            Return _qtd_alunos
        End Get
        Set(ByVal value As Integer)
            _qtd_alunos = value
        End Set
    End Property

    Private _qtdCapacidade As Integer
    Public Property QTD_CAPACIDADE() As Integer
        Get
            Return _qtdCapacidade
        End Get
        Set(ByVal value As Integer)
            _qtdCapacidade = value
        End Set
    End Property

    Private _id_prova As Integer
    Public Property ID_PROVA() As Integer
        Get
            Return _id_prova
        End Get
        Set(ByVal value As Integer)
            _id_prova = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub




    ''' <summary>
    ''' Carrega os dados para preencher o controle
    ''' </summary>
    Public Sub Carrega()



        Using SqlC As New SqlClient.SqlConnection(Ligacao())


            'Query para carregar as turmas com possível mesclagem (turma sem ser a principal, somente a primeira prova para não duplicar, sem ser sala individual)
            Dim sb As String = "" &
            "	with base as ( " &
            "            	select t.CO_TURMA_CENSO, ID_TURMA_SALA, t.ID_ESCOLA,  SEQ_TURMA_SALA , gs.ID_GRUPO_SALA,  gs.DS_GRUPO_SALA , gs.QT_MAX_ALUNO , CAST(NU_ALUNOS_ALOCADOS as int) NU_ALUNOS_ALOCADOS, NU_CAPACIDADE_ALUNOS, NO_TURMA,  " &
            "            	tr.DS_TURNO  TURNO  , ts.ID_PROVA  , ta.ID_DIA_APLICACAO  ,  tr.TURNO_BLOQUEIO_APLICADOR" &
            "            	    from TURMA_SALA ts   " &
            "            	    Inner join TURMA t on t.CO_TURMA_CENSO=ts.CO_TURMA_CENSO   " &
            "            	    Inner join ESCOLA e on e.ID_ESCOLA = t.ID_ESCOLA  " &
            "            	    Inner join  TURNO_HORARIO tr on tr.ID_TURNO_HORARIO= t.ID_HORARIO  " &
            "            	    Inner join GRUPO_SALA gs on ts.ID_GRUPO_SALA= gs.ID_GRUPO_SALA  " &
            "				    Inner join TURMA_APLICACAO ta on ta.CO_TURMA_CENSO = t.CO_TURMA_CENSO and ta.ID_ESCOLA=t.ID_ESCOLA and ta.ID_PROVA = ts.ID_PROVA" &
            "            	where SEQ_TURMA_SALA > 1  " &
            "            	    AND QT_MAX_ALUNO > 1   " &
            "            	    AND ts.ID_PROVA=@ID_PROVA  " &
            "            	    AND tr.DS_TURNO = @TURNO   " &
            "            	    AND gs.ID_GRUPO_SALA = @ID_GRUPO_SALA   " &
            "            	    AND t.ID_ESCOLA = @ID_ESCOLA  " &
            "				    AND ta.ID_DIA_APLICACAO= @ID_DIA_APLICACAO" &
            "            	    AND (ts.ID_CONTROLE_AGRUPAMENTO is null OR  (FLG_AGRUPADA is null or FLG_AGRUPADA = 'N' )  )  " &
            "				) select * from base "



            Using sqlDc As New SqlClient.SqlDataAdapter(sb, SqlC)

                sqlDc.SelectCommand.Parameters.AddWithValue("@ID_ESCOLA", _id_escola)
                sqlDc.SelectCommand.Parameters.AddWithValue("@TURNO", _turno)
                sqlDc.SelectCommand.Parameters.AddWithValue("@ID_GRUPO_SALA", _id_grupo_sala)
                sqlDc.SelectCommand.Parameters.AddWithValue("@ID_PROVA", _id_prova)
                sqlDc.SelectCommand.Parameters.AddWithValue("@ID_DIA_APLICACAO", _id_dia_aplicacao)

                Dim dtSalas As New DataTable()
                sqlDc.Fill(dtSalas)






                If (dtSalas.Rows.Count > 1) Then 'Caso encontre  mais de uma sala possível de mesclagem (se voltar 1 registro só tem uma turma não precisa aparecere ) 

                    _qtd_alunos = dtSalas.Compute("SUM(NU_ALUNOS_ALOCADOS)", "") 'Carrega a quantidade total de alunos alocados. (para todas as turmas que estão exibidas no controle

                    _qtdCapacidade = dtSalas.Rows(0)("QT_MAX_ALUNO") 'Carrega a capacidade máxima do GRUPO SALA (utilizado para o cálculo das novas salas

                    _dsc_grupo_sala = dtSalas.Rows(0)("DS_GRUPO_SALA") 'carrega a descrição do grupo sala, facilita na hora de preencher as novas salas
                    lblDeficiencia.Text = _dsc_grupo_sala

                    _co_turma_censo = dtSalas.Rows(0)("CO_TURMA_CENSO")

                    _qtd_turmas = dtSalas.Compute("COUNT(ID_TURMA_SALA)", "NU_ALUNOS_ALOCADOS > 0")


                    For Each dt As DataRow In dtSalas.Rows
                        _id_turma_sala.Add(dt("ID_TURMA_SALA"))

                    Next

                    'Carrega o detalhe (lista de salas) usando um repeater
                    rptMain.DataSource = dtSalas 'seta o datasource do repeater com a tabela carregada na linha 111 

                    rptMain.DataBind() 'executa a carga de dados

                    pnlMain.Visible = True 'seta o conteúdo visível 




                End If

            End Using


        End Using




    End Sub







End Class