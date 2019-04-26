Imports System.Configuration

Module Module2

    Private Const PalavraPadrão = "Rabud@101"
    Private Const mcALFABETO = "LMA9UV0CERBFG6HXYZI8PDQ4SJTW7NKOkl2ma@rs~3bcxyfijnpvwghzodeqtu51-_"
    Private mvChaves() As Integer = {11, 17, 7, 5, 13}


    ' Constantes que estabelecem a ordem dos parâmetros passados de página para página
    Public Const gcParametroTimeStamp = 0
    Public Const gcParametroOrigem = 1
    Public Const gcParametroCPF = 2
    Public Const gcParametroUFbase = 3
    Public Const gcParametroFuncao = 4
    Public Const gcParametroPolo = 5
    Public Const gcParametroNome = 6
    Public Const gcParametroOperacao = 7
    Public Const gcParametroDestino = 8
    'Public Const XXXgcParametroFiltros = 9
    Public Const gcParametroSessionID = 9
    Public Const gcParametroPalavraPadrao = 10      ' Deve ser sempre o último
    Public Const gcNumParametros = 11


    ' Máscaras de consistência do passaporte
    Public gcMascaraCpf = "SSS"
    Public gcMascaraCpfUf = "SSSS"
    Public gcMascaraCpfUfFuncao = "SSSSS"


    ' Tempos em segundos que limitam as sessões
    Public Const gcMaxDuracaoSessao = 1200      ' 20 minutos de espera até fazer logout (mesmo que seja PostBack)
    Public Const gcDuracaoSessao = 30           ' Tempo de espera se não for PostBack



    Function CompletaTamanhoMascara(ByRef QueM As String)
        Dim tmp As String = QueM
        While tmp.Length < gcNumParametros
            tmp &= "@"
        End While
        Return tmp
    End Function

    Function ConsisteQueryString(ByRef QueReceptora As String, ByRef QueRequest As HttpRequest,
                                 ByRef QueMascara As String, ByRef Parametros As String(),
                                 ByRef QueIsPostBack As Boolean, ByRef FlgConsisteTempo As Boolean) As String

        ' Verifica se a query string está OK
        Dim Retorno As String = ""
        Dim message = QueRequest.QueryString("q")
        If message Is Nothing Then
            Return "É preciso registrar-se no sistema pela tela apropiada."
        End If

        ' Verifica se está incluindo ou editando
        ReDim Parametros(gcNumParametros)
        Parametros = DeCodificaQS(message)
        If UBound(Parametros) > gcNumParametros Then
            Return "Esta página foi invocada com parâmetros incorretos."
        End If

        ' Verifica se a página de destino está correta (aceita, excepcionalmente, "*", para o caso da master page ser a receptora inicial)
        'If Parametros(gcParametroDestino).ToLower <> QueReceptora.ToLower And QueReceptora <> "*" Then
        '    Return "Houve um erro de direcionamento de página."
        'End If

        ' Verifica se todos os parâmetros obrigatórios estão preenchidos
        For k As Int16 = 0 To gcNumParametros - 1
            If Parametros(k) = "@" And QueMascara(k) = "S" Then
                Return "Um ou mais dos parâmetros obrigatórios não foi fornecido."
            End If
        Next

        Dim S As String() = Split(Parametros(gcParametroTimeStamp), "-")
        ' Creates a DateTime for the local time.
        Dim dt As New DateTime(CInt(S(0)), CInt(S(1)), CInt(S(2)), CInt(S(3)), CInt(S(4)), CInt(S(5)))

        ' Busca os controles na tabela LOGIN (sequencia e sessionID)
        If ColetaSessionIdRELOGIO(Parametros(gcParametroCPF)) <> Parametros(gcParametroSessionID) Then
            Return "A sessão em que essa página foi enviada está encerrada."
        End If

        ' Testa este quesito apenas se solicitado
        If FlgConsisteTempo Then
            'If ColetaUltimoRELOGIO(Parametros(gcParametroCPF)) > DateAdd(DateInterval.Second, 2, dt) Then
            '    Return "A ordem de submissão das páginas está incorreta."
            'End If
        End If

        Return "OK"

    End Function


    Private Function gfCriptografa(s As String) As String

        Dim I As Int16
        Dim J As Int16
        Dim Sout As String

        Dim L As Integer = mcALFABETO.Length
        Sout = ""
        For I = 0 To Len(s) - 1
            J = mcALFABETO.IndexOf(s(I))
            If J = -1 Then
                Sout = Sout & s(I)
            Else
                J = J + mvChaves(I Mod 5)
                Sout = Sout & mcALFABETO(J Mod L)
            End If
        Next I
        gfCriptografa = Sout
    End Function

    Private Function gfDeCriptografa(s As String) As String

        Dim I As Integer
        Dim J As Integer
        Dim Sout As String

        Dim L As Integer = mcALFABETO.Length
        Sout = ""
        For I = 0 To Len(s) - 1
            J = mcALFABETO.IndexOf(s(I))
            If J = -1 Then
                Sout = Sout & s(I)
            Else
                J = J - mvChaves(I Mod 5)
                If J < 0 Then J += L
                Sout = Sout & mcALFABETO(J Mod L)
            End If
        Next I
        gfDeCriptografa = Sout
    End Function

    Private Function GetRandom(ByVal Min As Integer, ByVal Max As Integer) As Integer
        ' by making Generator static, we preserve the same instance '
        ' (i.e., do not create new instances with the same seed over and over) '
        ' between calls '
        Static Generator As System.Random = New System.Random()
        Return Generator.Next(Min, Max)
    End Function

    Function Codifica(ByRef QueX As String()) As String

        Dim delta As Int16

        ' Força variações na criptografia
        delta = GetRandom(1, 7)
        Dim tmp As String = "~"
        For k = 2 To delta
            tmp &= "~"
        Next
        For k = 0 To UBound(QueX)
            tmp &= QueX(k) & "~"
        Next
        ' Força + variações na criptografia
        delta = GetRandom(1, 5)
        For k = 1 To delta
            tmp &= "~"
        Next
        tmp &= PalavraPadrão
        Return gfCriptografa(tmp)
    End Function
    Sub RelogioIniciaLogin(ByRef QueCPF As String, ByRef QueSessionID As String, ByRef QueUFbase As String)
        Dim Filtro As String = gcFiltrosInicializacao

        ' Atualiza o valor do filtro da string na posição zero (UF); se for primeira inclusão, entra um filtro inicializado
        Filtro = EmpacotaUmFiltro(Filtro, 0, QueUFbase)
        ' Coloca um segundo a menos porque a hora do sistema não contém decimais
        Dim sSQL As String = "UPDATE LOGIN set SessionID='" & QueSessionID & "',INICIO=GETDATE(),UltimoEnvio=GETDATE() where CPF='" & QueCPF & "';" &
                            " IF @@ROWCOUNT = 0 INSERT INTO LOGIN(CPF,SessionID,Filtros) VALUES ('" & QueCPF & "','" & QueSessionID & "','" & Filtro & "')"
        Call ExecutaSQL(sSQL)
    End Sub
    Sub RelogioUpdate(ByRef QueCPF As String, ByRef QueSessionID As String)
        Dim sSQL As String = "UPDATE LOGIN Set UltimoEnvio=GETDATE()  where CPF='" & QueCPF & "';"
        Call ExecutaSQL(sSQL)
    End Sub
    Sub RelogioUpdateComFiltro(ByRef QueCPF As String, ByRef QueFiltro As String)
        Dim sSQL As String = "UPDATE LOGIN Set UltimoEnvio=GETDATE(),Filtros='" & QueFiltro & "'  where CPF='" & QueCPF & "';"
        Call ExecutaSQL(sSQL)
    End Sub

    'Sub RelogioUpdate(ByRef QueCPF As String, ByRef QueSessionID As String)
    '    Dim sSQL As String = "UPDATE LOGIN Set UltimoEnvio=GETDATE()  where CPF='" & QueCPF & "';" &
    '                         "IF @@ROWCOUNT = 0 INSERT INTO RELOGIO(CPF,SessionID) VALUES ('" & QueCPF & "','" & QueSessionID & "')"
    '    Call ExecutaSQL(sSQL)
    'End Sub

    Sub RelogioZERA(ByRef QueCPF As String)
        Dim sSQL As String = "UPDATE LOGIN set SessionID='' where CPF='" & QueCPF & "'"
        Call ExecutaSQL(sSQL)
    End Sub

    Function MontaQueryString(ByRef QueOrigem As String, ByRef QueCPF As String, ByRef QueUF As String,
                              ByRef QueFuncao As String, ByRef QuePolo As String, ByRef QueNome As String,
                              ByRef QueOperacao As String, ByRef QueDestino As String, ByRef QueFiltros As String, ByRef QueSessionID As String) As String
        Dim Z As String() = {"", "", "", "", "", "", "", "", "", "", "", ""}

        ' Posicona os parâmetros na ordem estabelecida
        Z(gcParametroTimeStamp) = Trim(Str(DateTime.Now.Year)) & "-" & Trim(Str(DateTime.Now.Month)) & "-" & Trim(Str(DateTime.Now.Day)) & "-" &
                                  Trim(Str(DateTime.Now.Hour)) & "-" & Trim(Str(DateTime.Now.Minute)) & "-" & Trim(Str(DateTime.Now.Second))
        Z(gcParametroOrigem) = QueOrigem
        Z(gcParametroCPF) = QueCPF
        Z(gcParametroUFbase) = QueUF
        Z(gcParametroFuncao) = QueFuncao
        Z(gcParametroPolo) = QuePolo
        Z(gcParametroNome) = QueNome
        Z(gcParametroOperacao) = QueOperacao
        Z(gcParametroDestino) = QueDestino
        'Z(gcParametroFiltros) = QueFiltros
        Z(gcParametroSessionID) = QueSessionID

        Return "?q=" & gcDQ & Codifica(Z) & gcDQ

    End Function

    Function MontaQueryStringFromParametros(ByRef QueOrigem As String, ByRef QueDestino As String, ByRef QueParametros As String()) As String
        ' Esse procedimento renova o timestamp
        Return MontaQueryString(QueOrigem, QueParametros(gcParametroCPF), QueParametros(gcParametroUFbase), QueParametros(gcParametroFuncao),
                                QueParametros(gcParametroPolo), QueParametros(gcParametroNome), QueParametros(gcParametroOperacao),
                                QueDestino, "", QueParametros(gcParametroSessionID))
    End Function


    Function Ligacao() As String
        Return ConfigurationManager.ConnectionStrings("ANA2016_connection").ConnectionString
    End Function



    Function DeCodificaQS(ByRef QueS As String) As String()
        If QueS = Nothing Then
            Return Nothing
        End If
        Dim tmp As String = Mid(QueS, 2, QueS.Length - 2)
        tmp = gfDeCriptografa(tmp)

        Dim A As String() = Split(tmp, "~")

        ' Conta as instância válidas em A
        Dim C As Int16 = 0
        For k = 0 To A.Length - 1
            If A(k) <> "" Then
                C += 1
            End If
        Next

        ' Verifica se deu certo; o última é sempre a palavra padrão
        If A(UBound(A)) <> PalavraPadrão Then
            Return Nothing
        Else
            ' Prepara a string de retorno
            Dim R As String()
            ReDim R(gcNumParametros)
            C = 0
            For k = 0 To A.Length - 1
                If A(k) <> "" Then
                    R(C) = A(k)
                    C += 1
                End If
            Next

            ' Completa os parâmetros
            For k = C To gcNumParametros - 1
                R(C) = "@"
            Next

            Return R

        End If



    End Function
End Module
