Public Class ctlSalaEspecial
    Inherits System.Web.UI.UserControl

    Private _id_turma_sala As Integer
    Public Property ID_TURMA_SALA() As String
        Get
            Return _id_turma_sala
        End Get
        Set(ByVal value As String)
            _id_turma_sala = value
        End Set
    End Property

    Private _no_sala As String
    Public Property SALA() As String
        Get
            Return _no_sala
        End Get
        Set(ByVal value As String)
            _no_sala = value
        End Set
    End Property


    Private _alunos As List(Of String) = New List(Of String)

    Public Property ALUNOS() As List(Of String)
        Get
            Return _alunos
        End Get
        Set(ByVal value As List(Of String))


            _alunos = value


        End Set
    End Property


    Private _dt_pessoas As DataTable
    Public WriteOnly Property DT_PESSOAS As DataTable
        Set(ByVal value As DataTable)
            _dt_pessoas = value
        End Set
    End Property



    Public Property CPF_APLICADOR() As String
        Get
            Return cmbColaborador.SelectedValue
        End Get
        Set(ByVal value As String)
            cmbColaborador.SelectedValue = value
        End Set
    End Property


    Private _naoprevista As Boolean = False
    Public Property NAO_PREVISTA() As String
        Get
            Return _naoprevista
        End Get
        Set(ByVal value As String)
            _naoprevista = value
        End Set
    End Property




    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

    Public Sub Carrega()

        lblTitulo.Text = _no_sala

        cmbColaborador.Items.Clear()
        cmbColaborador.Items.Add(New ListItem("Escolha...", "0"))
        cmbColaborador.ClientIDMode = ClientIDMode.AutoID



        For Each r As DataRow In _dt_pessoas.Rows
            Dim nome As String = r("NO_PESSOA") & " - " & EditaCPF(r("CPF")) & IIf(r("DSC_FUNCAO") <> "", "(" & r("DSC_FUNCAO") & ")", "")
            cmbColaborador.Items.Add(New ListItem(nome, r("CPF")))
        Next

        If (CPF_APLICADOR <> "") Then
            cmbColaborador.SelectedValue = CPF_APLICADOR
        End If

        If (_naoprevista) Then lblQtdAlunos.Visible = False


    End Sub

    Public Sub AtualizaAlunos()

        'Dim htmlSaida As String = "<table>"

        'For Each a As String In _alunos

        '    htmlSaida &= "<tr><td>" & a & "</td></tr>"

        'Next

        'htmlSaida &= "</table>"

        'lblAluno.Text = htmlSaida
        lblQtdAlunos.Text = ALUNOS.Count




    End Sub



End Class