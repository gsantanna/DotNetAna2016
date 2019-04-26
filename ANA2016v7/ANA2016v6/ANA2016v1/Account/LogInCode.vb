Public Class LogInCode

    Public QuePessoaId As Int32
    Public QuePessoaNome As String
    Public OK As Boolean


    Function RecebeLogin(ByRef QueCPF As String, ByRef QueSenha As String) As String
        ' Retorna a UF do CPF no último parâmetro
        Dim Retorno As String = ConfereUmaSenha(QueCPF, QueSenha)

        If Retorno = Nothing Then
            Return "CPF não cadastrado."
        End If

        If Len(Retorno) > 1 Then
            Return Retorno
        End If
        If Retorno = "0" Then
            Return "Senha incorreta."
        End If

        ' Coleta os dados para a query string
        Dim sSQL As String = "SELECT p.CPF + '|' + p.NO_PESSOA + '|' + p.SG_UF_ALOC + '|' + isnull(x.NO_FUNCAO,'Aplicador') + '|' + isnull(x.ID_POLO,'@')" &
                            " FROM (SELECT * FROM PESSOAL WHERE CPF='" & QueCPF & "') p" &
                                 " LEFT JOIN (Select TOP 1 a.NO_FUNCAO, a.ID_POLO, a.CPF FROM ATRIBUICAO_GESTOR a where a.CPF='" & QueCPF & "')x on x.CPF=p.CPF"

        Dim Dados As String = ColetaValorString(sSQL)

        ' Retorna o OK + indicador de senha provisória + os dados
        Return "OK|" + Retorno & "|" & Dados

    End Function
End Class
