
Public Class ctlQuadroMesclagemSeletor
    Inherits System.Web.UI.UserControl






    Private _turmas_originais As List(Of Integer) = New List(Of Integer)
    Public Property TURMAS_ORIGINAIS() As List(Of Integer)
        Get
            Return _turmas_originais
        End Get
        Set(ByVal value As List(Of Integer))
            _turmas_originais = value
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


    Private _turno As String
    Public Property TURNO() As String
        Get
            Return _turno
        End Get
        Set(ByVal value As String)
            _turno = value
        End Set
    End Property

    Private _turnobloqueio As String
    Public Property TURNO_BLOQUEIO() As String
        Get
            Return _turnobloqueio
        End Get
        Set(ByVal value As String)
            _turnobloqueio = value
        End Set
    End Property



    Private _novasturmas As List(Of NovasTurmas) = New List(Of NovasTurmas)
    Public Property NOVAS_TURMAS() As List(Of NovasTurmas)
        Get
            Return _novasturmas
        End Get
        Set(ByVal value As List(Of NovasTurmas))
            _novasturmas = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub



    Public Class NovasTurmas

        Public Property CAPACIDADE As Integer
        Public Property ALOCADOS As Integer
        Public Property CPF_APLICADOR As String


    End Class


    Public Sub Carrega()

        lblDeficiencia.Text = _dsc_grupo_sala
        rptMain.DataSource = NOVAS_TURMAS
        rptMain.DataBind()



    End Sub



    Public Function getAplicadoresDisponiveis()

        Dim dtPessoas As DataTable = New DataTable()

        Using SqlC As New SqlClient.SqlConnection(Ligacao())

            'Carrega todos os colaboradores disponíveis para carregar os combos (colocado aqui para ser carregado somente uma vez)
            Dim strSqlPessoas As String = "select  p.CPF, P.NO_PESSOA ,  " &
            " ltrim(rtrim(max( case when  ap.NO_FUNCAO = 'IntÃ©rprete' then 'IntÃ©rprete ' else '' end )  + max( case when  ap.NO_FUNCAO = 'Guia-IntÃ©rprete' then 'Guia-IntÃ©rprete ' else '' end )  + max( case when  ap.NO_FUNCAO = 'IntÃ©rprete de Libras' then 'IntÃ©rprete de Libras ' else '' end )  +max( case when  ap.NO_FUNCAO = 'Ledor/Transcritor' then 'Ledor/Transcritor ' else '' end )  +  " &
            " max( case when  ap.NO_FUNCAO = 'Ledor' then 'Ledor ' else '' end )  + ' ' + max( case when  ap.NO_FUNCAO = 'Leitura labial' then 'Leitura labial ' else '' end )   +  max( case when  ap.NO_FUNCAO = 'Aplicador' then '' else '' end)  )) DSC_FUNCAO" &
            " from ATRIBUICAO_POLO ap " &
            " Inner join PESSOAL p on p.CPF=ap.CPF" &
            " Inner join ESCOLA e on E.id_polo=ap.id_polo " &
            " where e.ID_ESCOLA=@ID_ESCOLA or NO_PESSOA LIKE '%A%' " &
            " group by p.CPF , P.NO_PESSOA" &
            " UNION" &
            " SELECT '0',' NÃO CONFIRMADO',''" &
            " ORDER BY 2"

            Using Sda As New SqlClient.SqlDataAdapter(strSqlPessoas, SqlC)
                Sda.SelectCommand.Parameters.AddWithValue("@ID_ESCOLA", _id_escola)
                Sda.SelectCommand.Parameters.AddWithValue("@ID_GRUPO_SALA", _id_grupo_sala)
                Sda.Fill(dtPessoas)
            End Using
        End Using

        Return dtPessoas

    End Function

    Protected Sub rptMain_ItemCreated(sender As Object, e As RepeaterItemEventArgs)

        Dim combo As DropDownList = e.Item.FindControl("cmbAplicadoresDisp")
        combo.Attributes.Add("data-turno", _turnobloqueio)

    End Sub
End Class