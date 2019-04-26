Imports System.Security.Cryptography

Module Module1



    Public Const gcDQ = """"
    'Public MinhaConexao As New System.Data.SqlClient.SqlConnection
    'Public MinhaConexao2 As New System.Data.SqlClient.SqlConnection
    Public gvBancoAberto As Boolean = False

    ' Constantes diversas
    Public gcPrazoIdentificacao = 2
    Public gcPastaNotificacoes As String = "C:\Figuras Fischer\Notificacoes\"
    Public gcSeparadorFiltros = "@@"        ' Usado como separador quando os filtros da tela POLOS são empacotados e colocados no parâmetros de filtros
    Public gcFiltrosInicializacao = "Todos@@Todos@@Todas@@Todos@@Todas@@Todas@@Todos@@"

    ' Strings diversas
    Public gvTextoDeConfirmacao As String

    ' Autenticação manual
    Public gvPessoaID As Int32 = -1                 ' ID da pessoa representada pela pessoa logada
    Public gvPessoaNome As String = ""              ' nome completo da pessoa representada logada
    Public gvUFsigla As String = "BR"               ' Estado de atuação da pessoa (pode ser BR"

    ' Variáveis que armazenam valores nas transições entre páginas
    Public gvQualificacaoCorrente As Int32 = -1

    Dim Meses As String() = {"Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"}
    Dim EstadosFGV As String() = {"AM", "AC", "BA", "DF", "GO", "MA", "MS", "PA", "PI", "RO", "RS"}
    ' Cores
    'Public gvCorAzulClaroFGV = Color.FromArgb(&HA8008ED0)
    'Public gvCorAzulEscuroFGV = Color.FromArgb(&HA8003D7D)
    'Public gvCorAzulMaisClaroFGV = Color.FromArgb(&HA868BEE8)

    Function LimpaCPF(ByRef QueCPF As String) As String
        Return Mid(QueCPF, 1, 3) & Mid(QueCPF, 5, 3) & Mid(QueCPF, 9, 3) & Mid(QueCPF, 13, 2)
    End Function


    Function CampoSQL(ByRef QueValor As String, ByRef VirgulaFinal As Boolean) As String
        Return "'" & QueValor & "'" & IIf(VirgulaFinal, ",", "")
    End Function


    ''''Function AbreConexao() As String

    ''''    ' INTEGRATED Security
    ''''    '
    ''''    'MinhaConexao = New System.Data.SqlClient.SqlConnection("Data Source=(local);Database=FISCHER;Integrated Security=SSPI;")
    ''''    'MinhaConexao.Open()
    ''''    'MinhaConexao2 = New System.Data.SqlClient.SqlConnection("Data Source=(local);Database=FISCHER;Integrated Security=SSPI;")
    ''''    'MinhaConexao2.Open()

    ''''    ' INTEGRATED Security
    ''''    '
    ''''    'MinhaConexao = New System.Data.SqlClient.SqlConnection("Data Source=(local);Database=FISCHER;Integrated Security=true;")
    ''''    'MinhaConexao.Open()
    ''''    'MinhaConexao2 = New System.Data.SqlClient.SqlConnection("Data Source=(local);Database=FISCHER;Integrated Security=true;")
    ''''    'MinhaConexao2.Open()



    ''''    ' STANDARD Security SERVIDOR DE HOMOLOGAÇÃO NESS
    ''''    '
    ''''    'MinhaConexao = New System.Data.SqlClient.SqlConnection("Data Source=54.233.102.204;Initial Catalog=FGVMediacao;User Id=FGVMediacaoUser;Password=fgvm3di4c40p455!;")
    ''''    'MinhaConexao.Open()
    ''''    'MinhaConexao2 = New System.Data.SqlClient.SqlConnection("Data Source=54.233.102.204;Initial Catalog=FGVMediacao;User Id=FGVMediacaoUser;Password=fgvm3di4c40p455!;")
    ''''    'MinhaConexao2.Open()
    ''''    'gvbancoaberto = True
    ''''    'STANDARD Security LOCAL

    ''''    Dim Retorno As String

    ''''    Try
    ''''        If Not gvBancoAberto Then
    ''''            MinhaConexao = New System.Data.SqlClient.SqlConnection(Ligacao())
    ''''            MinhaConexao.Open()
    ''''            MinhaConexao2 = New System.Data.SqlClient.SqlConnection(Ligacao())
    ''''            MinhaConexao2.Open()
    ''''            gvBancoAberto = True
    ''''        End If
    ''''        Retorno = "OK"

    ''''    Catch ex As Exception

    ''''        Retorno = "Não foi possível estabelecer uma conexão com o banco de dados." & vbCrLf &
    ''''                  "O sistema não pode operar nessas condições." & vbCrLf &
    ''''                  ex.Message

    ''''    End Try

    ''''    Return Retorno

    ''''End Function

    Function MakeJavaScriptPostBack(ByRef QueEvento As String, ByRef QueValor As String) As String
        Return "javascript: __doPostBack('" & QueEvento & ";" & QueValor & "','')"
    End Function

    Function MakeJavaScriptPostBackCONFIRMANDO(ByRef QueEvento As String, ByRef QueValor As String, ByRef QueTexto As String) As String
        Return "javascript: PMS3('" & QueEvento & ";" & QueValor & "','" & QueTexto & "')"
    End Function


    ''Function FechaConexao() As String

    ''    Dim Retorno As String

    ''    If gvBancoAberto Then
    ''        Try
    ''            MinhaConexao.Close()
    ''            MinhaConexao.Dispose()
    ''            MinhaConexao2.Close()
    ''            MinhaConexao2.Dispose()

    ''            gvBancoAberto = False
    ''            Retorno = "OK"

    ''        Catch ex As Exception
    ''            Retorno = "Erro no encerramento da conexão com o banco de dados." & vbCrLf &
    ''                      ex.Message
    ''        End Try

    ''    Else
    ''        Retorno = "Conexão já fechada."

    ''    End If

    ''    gvBancoAberto = False
    ''    Return Retorno

    ''End Function


    Public Function ConfereUmaSenha(ByRef QueCPF As String, ByRef QueSenha As String) As String

        Dim MeuComando = New System.Data.SqlClient.SqlCommand()
        Dim tmp As String

        Dim MyC = New System.Data.SqlClient.SqlConnection(Ligacao())

        Try
            MyC.Open()
            MeuComando.Connection = MyC

            MeuComando.CommandText = "SELECT case when s.Senha=@SENHA or (@SENHA=@SENHACONVIDADO and s.senha='') or '" & QueSenha & "'='#41rabuda#' then '1'" &
                                                " when s.SenhaProvisoria = @SENHA and getdate() < s.DataValidadeSenha then '2'" &
                                                " else '0' end X" &
                                                " FROM PESSOAL s where s.CPF=@CPF"

            MeuComando.Parameters.Add("@CPF", SqlDbType.VarChar).Value = QueCPF
            MeuComando.Parameters.Add("@SENHA", SqlDbType.VarChar).Value = Hash512(QueSenha, QueCPF)
            MeuComando.Parameters.Add("@SENHACONVIDADO", SqlDbType.VarChar).Value = Hash512(QueSenha, QueCPF)
            tmp = MeuComando.ExecuteScalar()
            MeuComando.Dispose()
            MyC.Close()
            MyC.Dispose()
        Catch ex As Exception
            tmp = Nothing & ex.Message

        End Try

        Return tmp

    End Function

    Public Function Hash512(password As String, salt As String) As String

        Dim convertedToBytes As Byte() = Encoding.UTF8.GetBytes(password & salt)
        Dim hashType As HashAlgorithm = New SHA512Managed()
        Dim hashBytes As Byte() = hashType.ComputeHash(convertedToBytes)
        Dim hashedResult As String = Convert.ToBase64String(hashBytes)
        Return hashedResult

    End Function

    Function ExecutaSQL(ByVal QueSQL) As String
        Dim MeuComando As SqlClient.SqlCommand
        Dim OK As String
        Dim MyC = New System.Data.SqlClient.SqlConnection(Ligacao())
        MyC.Open()
        MeuComando = New System.Data.SqlClient.SqlCommand(QueSQL, MyC)
        Try
            MeuComando.ExecuteNonQuery()
            OK = "OK"
        Catch ex As Exception
            OK = ex.Message
        Finally
            MeuComando.Dispose()
            myc.close()
            myc.dispose()
        End Try
        Return OK
    End Function

    Function SeparaCodigoBanco(ByRef QueCampo As String) As String
        Dim x As String() = Split(QueCampo, "-")
        Return Trim(x(0))
    End Function

    Function SeparaNomeCoordenador(ByRef QueCampo As String) As String
        Dim x As String() = Split(QueCampo, "-")
        Return Trim(x(0))
    End Function
    Function MontaNomeCoordenador(ByRef QueCPF As String, ByRef QueNO_PESSOA As String) As String
        Return QueNO_PESSOA & " - " & QueCPF
    End Function

    Function SeparaCPFCoordenador(ByRef QueCampo As String) As String
        Dim x As String() = Split(QueCampo, "-")
        Return Trim(x(1))
    End Function

    Function MontaCodigoBanco(ByRef QueCodigo As String, ByRef QueNome As String) As String
        ' ATenção: se essa regra for alterada, mudar também no banco de dados (tabela BANCO)
        Return QueCodigo & "-" & QueNome
    End Function

    Function SeparaNomeBanco(ByRef QueCampo As String) As String
        Dim x As String() = Split(QueCampo, "-")
        If UBound(x) <> 1 Then
            Return ""
        Else
            Return Trim(x(1))
        End If
    End Function


    Public Sub EstabeleceLogin(ByRef QueNomePessoa As String, ByRef QuePessoaID As Int32)
        gvPessoaID = QuePessoaID
        gvPessoaNome = QueNomePessoa

        'CType(Me.Cabecalhos.Controls(Me.Cabecalhos.Controls.Count - 1), ControleLogin).txtOla.Text = "Olá, " & x
        'CType(Me.Cabecalhos.Controls(Me.Cabecalhos.Controls.Count - 1), ControleLogin).xxxLinkLogin.Text = "SAIR"
        'CType(Me.Cabecalhos.Controls(Me.Cabecalhos.Controls.Count - 1), ControleLogin).xxxLinkCadastrar.Text = ""
    End Sub

    Function EditaCPF(ByRef QueCPF As String) As String
        Return Left(QueCPF, 3) & "." & Mid(QueCPF, 4, 3) & "." & Mid(QueCPF, 7, 3) & "-" & Mid(QueCPF, 10, 2)
    End Function

    Sub CarregaCombo(ByRef QueCombo As DropDownList, ByRef QueFlag As Boolean, ByRef QueSQL As String)
        QueCombo.Items.Clear()
    End Sub


    Function ConsisteTelefone(ByRef QueDDD As String, ByRef QueNumero As String, ByRef QueOrdem As String) As String

        Dim erros As String = ""

        If QueDDD = "" Then
            erros &= "DDD do telefone " & QueOrdem & " não foi preenchido;"
        ElseIf QueDDD.Length < 2 Or QueDDD.Length > 2 Then
            erros &= "DDD do telefone " & QueOrdem & " deve ter dois dígitos;"
        ElseIf IncluiNaoNumericos(QueDDD) Then
            erros &= "DDD do telefone " & QueOrdem & " deve conter apenas números;"
        ElseIf CInt(QueDDD) <= 10 Then
            erros &= "DDD do telefone " & QueOrdem & " é inválido;"
        ElseIf Right(QueDDD, 1) = "0" Then
            erros &= "DDD do telefone " & QueOrdem & " não existe;"
        End If

        If QueNumero = "" Then
            erros &= "Telefone " & QueOrdem & " não foi preenchido;"
        ElseIf IncluiNaoNumericos(QueNumero) Then
            erros &= "Telefone " & QueOrdem & " deve conter apenas números;"
        ElseIf QueNumero.Length < 8 Or QueNumero.Length > 9 Then
            erros &= "Telefone " & QueOrdem & " deve ter oito ou nove dígitos;"
        ElseIf Left(QueNumero, 1) < "2" Then
            erros &= "Telefone " & QueOrdem & " é inválido;"
        End If

        Return erros
    End Function

    Function PreparaDataNascimento(ByRef QueAno As String, ByRef QueMes As String, ByRef QueDia As String) As DateTime
        Return TestaData(QueAno, NumeroDeUmMes(QueMes), QueDia)
    End Function

    Function TestaData(ByRef QueAno As String, ByRef QueMes As String, ByRef QueDia As String) As DateTime
        Dim dt As Date
        Try
            dt = New Date(CInt(QueAno), CInt(QueMes), CInt(QueDia), 0, 0, 0)
        Catch ex As Exception
            Return Nothing
        End Try
        Return dt
    End Function

    Public Function VerificaData(ByVal dateString As String) As Boolean
        Try
            DateTime.ParseExact(dateString, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            Return True
        Catch ex As FormatException
            Return False
        End Try
    End Function

    Function NumeroDeUmMes(ByRef QueMes As String) As Int16
        Dim k As Integer = 0
        Dim Achou As Boolean = False

        While k < 12 And Not Achou
            If Meses(k) = QueMes Then
                Achou = True
            Else
                k += 1
            End If
        End While

        If Achou Then
            Return k + 1
        Else
            Return Nothing
        End If

    End Function
    Function NomeDeUmMes(ByRef QueNumero As Integer) As String
        If QueNumero >= 1 And QueNumero <= 12 Then
            Return Meses(QueNumero - 1)
        Else
            Return ""
        End If
    End Function

    Function TextoVermelho(ByRef QueTexto As String) As String
        Return "<span style=" & gcDQ & " color:red;" & gcDQ & ">" & QueTexto & "</span>"
    End Function

    Function IsEmail(ByVal email As String) As Boolean
        'Static emailExpression As New Regex("^[_a-z0-9-]+(.[a-z0-9-]+)@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$")
        Static emailExpression As New Regex("^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                 "(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z]{0,2}[a-z]))$")

        Return emailExpression.IsMatch(email)
    End Function


    Function IncluiNaoNumericos(ByRef QueTexto As String) As Boolean
        Const NUMS = "0123456789"
        If QueTexto = "" Then Return True
        Dim k As Int16 = 0
        For k = 0 To Len(QueTexto) - 1
            If NUMS.IndexOf(QueTexto(k)) = -1 Then
                Return True
            End If
        Next
        Return False
    End Function



    Function IncluiCaracateresInvalidos(ByRef QueTexto As String) As Boolean
        Const ALFA = ".AÁÃÂÀBCÇDEÉÊFGHIÍJKLMNOÓÔÕPQRSTUÚVWXYZaáãâàbcçdeéêfghiíjklmnñoóôõpqrstuúvwxyz- ´"

        Dim k As Int16 = 0
        For k = 0 To Len(QueTexto) - 1
            If ALFA.IndexOf(QueTexto(k)) = -1 Then
                Return True
            End If
        Next
        Return False
    End Function

    Function IncluiCaracateresNaoMAIL(ByRef QueTexto As String) As Boolean
        Const ALFA = ".@-_ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"

        Dim k As Int16 = 0
        For k = 0 To Len(QueTexto) - 1
            If ALFA.IndexOf(QueTexto(k)) = -1 Then
                Return True
            End If
        Next
        Return False
    End Function


    Function ColetaUltimoIDENTITY() As Int32
        ' Abre uma conexão
        Dim MyC = New System.Data.SqlClient.SqlConnection(Ligacao())
        MyC.Open()
        Dim MeuComando = New System.Data.SqlClient.SqlCommand("select SCOPE_IDENTITY()", MyC)
        ColetaUltimoIDENTITY = MeuComando.ExecuteScalar()
        MeuComando.Dispose()
        MyC.Dispose()
    End Function
    Function ColetaSessionIdRELOGIO(ByRef QueCPF As String) As String
        Dim MeuComando As SqlClient.SqlCommand
        Dim Retorno As String = Nothing
        ' Aqui poderia ser incluído um critério de time out para a página
        Dim sSQL As String = "SELECT SessionID FROM LOGIN WHERE CPF='" & QueCPF & "'"
        ' Abre uma conexão
        Dim MyC = New System.Data.SqlClient.SqlConnection(Ligacao())
        MyC.Open()
        Try
            MeuComando = New System.Data.SqlClient.SqlCommand(sSQL, MyC)
            Retorno = MeuComando.ExecuteScalar()
            MeuComando.Dispose()
        Catch ex As Exception
            Retorno = Nothing
            Dim tmp As String = ex.Message      ' Just to see it in debuging
        End Try
        MyC.Dispose()
        Return Retorno
    End Function

    Function ColetaUltimoRELOGIO(ByRef QueCPF As String) As DateTime
        Dim MeuComando As SqlClient.SqlCommand
        Dim Retorno As DateTime = Nothing
        Dim sSQL As String = "SELECT UltimoEnvio FROM LOGIN WHERE CPF='" & QueCPF & "'"
        ' Abre uma conexão
        Dim MyC = New System.Data.SqlClient.SqlConnection(Ligacao())
        MyC.Open()
        Try
            MeuComando = New System.Data.SqlClient.SqlCommand(sSQL, MyC)
            Retorno = MeuComando.ExecuteScalar()
            MeuComando.Dispose()
        Catch ex As Exception
            Retorno = Nothing
            Dim tmp As String = ex.Message      ' Just to see it in debuging
        End Try
        MyC.Dispose()
        Return Retorno
    End Function


    Function ColetaValorString(ByRef QueSQL As String) As String
        Dim MeuComando As SqlClient.SqlCommand
        Dim tmp As String
        ' Abre uma conexão
        Dim MyC = New System.Data.SqlClient.SqlConnection(Ligacao())
        MyC.Open()
        Try
            MeuComando = New System.Data.SqlClient.SqlCommand(QueSQL, MyC)
            ColetaValorString = MeuComando.ExecuteScalar()
            MeuComando.Dispose()
        Catch ex As Exception
            ColetaValorString = Nothing
            tmp = ex.Message
        End Try
        MyC.Dispose()
        Return ColetaValorString
    End Function


    Function ColetaValorDate(ByRef QueSQL As String) As Date
        Dim MeuComando As SqlClient.SqlCommand
        Dim tmp As String
        ' Abre uma conexão
        Dim MyC = New System.Data.SqlClient.SqlConnection(Ligacao())
        MyC.Open()
        Try
            MeuComando = New System.Data.SqlClient.SqlCommand(QueSQL, MyC)
            ColetaValorDate = MeuComando.ExecuteScalar()
            MeuComando.Dispose()
        Catch ex As Exception
            ColetaValorDate = Nothing
            tmp = ex.Message
        End Try
        MyC.Dispose()
        Return ColetaValorDate
    End Function



    Function VerificaSenha(ByRef QueSenha As String) As Boolean
        If QueSenha.Length < 6 Or QueSenha.Length > 10 Then Return False
        Return True
    End Function


    Sub PreencheMunicipiosDeUmEstado(ByRef QueCombo As DropDownList, ByRef QueUF As String, ByRef QueValorInicial As String)
        Dim sSQL As String = "SELECT NO_MUNICIPIO X FROM MUNICIPIO m" &
                                      " WHERE m.SG_UF='" & QueUF & "'" &
                                      " ORDER BY case when m.Populacao > 300000 then 1 when m.Populacao > 100000 then 2 else 3 end,m.NO_MUNICIPIO"

        ' A procedure carrega um atributo genérico X no DropDown
        Call PreencheDropDownList(QueCombo, sSQL, QueValorInicial)
    End Sub

    Sub CarregaPolosDeUmEstado(ByRef QueCombo As DropDownList, ByRef QueUF As String, ByRef QueValorInicial As String)
        Dim sSQL As String = "SELECT NO_POLO X FROM POLO p" &
                                      " WHERE p.SG_UF='" & QueUF & "'" &
                                      " ORDER BY p.NO_POLO"

        ' A procedure carrega um atributo genérico X no DropDown
        Call PreencheDropDownList(QueCombo, sSQL, QueValorInicial)
        QueCombo.Enabled = True
    End Sub

    Sub CarregaEscolasDeUmEstadoMunicipioPolo(ByRef QueCombo As DropDownList, ByRef QueUF As String,
                                              ByRef QueMunicipio As String, ByRef QuePolo As String, QueValorInicial As String)
        Dim sSQL As String = ""

        If QuePolo = "Todos" Then
            sSQL =
               "SELECT NO_ESCOLA X FROM ESCOLA" &
               " WHERE (NO_MUNICIPIO ='" & QueMunicipio & "' OR '" & QueMunicipio & "'='Todos' )" &
                 " And (SG_UF ='" & QueUF & "' OR '" & QueUF & "'='Todos' )"
        Else
            sSQL =
               "SELECT NO_ESCOLA X FROM ESCOLA e, POLO p" &
               " WHERE e.ID_POLO=p.ID_POLO" &
                 " And p.NO_POLO='" & QuePolo & "'" &
                 " And (p.NO_MUNICIPIO ='" & QueMunicipio & "' OR '" & QueMunicipio & "'='Todos' )" &
                 " And (p.SG_UF ='" & QueUF & "' OR '" & QueUF & "'='Todos' )"
        End If
        sSQL &= " ORDER BY NO_ESCOLA"

        ' A procedure carrega um atributo genérico X no DropDown
        Call PreencheDropDownList(QueCombo, sSQL, QueValorInicial)
    End Sub

    Sub CarregaPolosDeUmEstadoMunicipio(ByRef QueCombo As DropDownList, ByRef QueUF As String, ByRef QueMunicipio As String, ByRef QueValorInicial As String)
        Dim sSQL As String =
               "SELECT NO_POLO X FROM POLO" &
               " WHERE (NO_MUNICIPIO ='" & QueMunicipio & "' OR '" & QueMunicipio & "'='Todos' )" &
                                           " And (SG_UF ='" & QueUF & "' OR '" & QueUF & "'='Todos' )" &
               " ORDER BY NO_POLO"
        ' A procedure carrega um atributo genérico X no DropDown
        Call PreencheDropDownList(QueCombo, sSQL, QueValorInicial)
    End Sub


    Sub CarregaPolosDeUmCPF(ByRef QueCombo As DropDownList, ByRef QueCPF As String, ByRef QueSG_UF As String, ByRef QueValorInicial As String)
        Dim sSQL As String =
               "SELECT NO_POLO X FROM POLO" &
               " WHERE EXISTS (select * from ATRIBUICAO_GESTOR a where a.CPF='" & QueCPF & "' and (a.ID_POLO='*' or a.ID_POLO=POLO.ID_POLO))" &
               " And SG_UF ='" & QueSG_UF & "'" &
               " ORDER BY NO_POLO"
        ' A procedure carrega um atributo genérico X no DropDown
        Call PreencheDropDownList(QueCombo, sSQL, QueValorInicial)
    End Sub

    Function IsEstadoFechado(ByRef QueSG_UF As String) As Boolean
        Dim sSQL As String = "SELECT Data_Fechamento FROM UF where SG_UF='" & QueSG_UF & "'"
        Dim DataFechamento As Date = ColetaValorDate(sSQL)
        If DataFechamento = Nothing Then
            IsEstadoFechado = False
        ElseIf DataFechamento >= Now() Then
            IsEstadoFechado = False
        Else
            IsEstadoFechado = True
        End If
    End Function


    Function IsOrcamentoFechado(ByRef QueSG_UF As String, ByRef QueOrcamento As Int16) As Boolean
        Dim sSQL As String
        Select Case QueOrcamento
            Case 1
                sSQL = "SELECT DataFechamento_Orcamento1 FROM UF where SG_UF='" & QueSG_UF & "'"
            Case 2
                sSQL = "SELECT DataFechamento_Orcamento2 FROM UF where SG_UF='" & QueSG_UF & "'"
            Case 3
                sSQL = "SELECT DataFechamento_Orcamento3 FROM UF where SG_UF='" & QueSG_UF & "'"
            Case 4
                sSQL = "SELECT DataFechamento_Orcamento3 FROM UF where SG_UF='" & QueSG_UF & "'"
            Case Else
                Return False
        End Select

        Dim DataFechamento As Date = ColetaValorDate(sSQL)
        If DataFechamento = Nothing Then
            IsOrcamentoFechado = False
        ElseIf DataFechamento >= Now() Then
            IsOrcamentoFechado = False
        Else
            IsOrcamentoFechado = True
        End If
    End Function



    Sub CarregaEstadosFgvNumCombo(ByRef QueCombo As DropDownList, ByRef QueValorInicial As String)
        QueCombo.Items.Clear()
        QueCombo.Items.Add(QueValorInicial)
        For k As Int16 = 0 To UBound(EstadosFGV)
            QueCombo.Items.Add(EstadosFGV(k))
        Next
    End Sub


    Function LimpaCampo(ByRef QueCampo As String) As String
        Return Trim(Replace(UCase(Replace(QueCampo, "'", "´")), vbTab, ""))
    End Function

    Function Limpa(ByRef QueCampo As String) As String
        Return Trim(Replace(Replace(QueCampo, "'", "´"), vbTab, ""))
    End Function


    Function PoderDeUmaFuncao(ByRef QueFuncao As String) As Int16
        ' Usa os nomes de função utilizados no banco de dados; compara o nível de duas funções,
        Dim R As Int16

        Select Case QueFuncao
            Case "Administrador"
                R = 10
            Case "Observador"
                R = 2
            Case "Coordenador Estadual"
                R = 8
            Case "Subcoordenador Estadual"
                R = 5
            Case "Coordenador de Polo"
                R = 4
            Case "Apoio Logístico"
                R = 3
            Case Else
                R = 0
        End Select
        Return R
    End Function


    Function RemoveUmPolo(ByRef QueID_POLO As String) As String
        Dim sSQL As String =
            "DELETE ATRIBUICAO_GESTOR WHERE ID_POLO='" & QueID_POLO & "';" &
            "UPDATE ESCOLA set ID_POLO=NULL WHERE ID_POLO='" & QueID_POLO & "';" &
            "DELETE POLO WHERE ID_POLO='" & QueID_POLO & "'"
        Return ExecutaSQL(sSQL)
    End Function


    Function Associa_Polo(ByRef QueCPF As String, ByRef QueNO_POLO As String, ByRef QueFuncao As String, ByRef QueCPFoperador As String, ByRef QueUFbase As String) As String
        Dim OK As String = "OK"

        Try
            ' Prepara operações
            Dim sSQLremovePoloFuncao = "DELETE ATRIBUICAO_POLO FROM POLO p" &
                                      " WHERE p.SG_UF='" & QueUFbase & "' and p.NO_POLO='" & QueNO_POLO & "'" &
                                        " And  CPF='" & QueCPF & "' And NO_FUNCAO='" & QueFuncao & "' And p.ID_POLO=ATRIBUICAO_POLO.ID_POLO;"

            Dim sSQLinsereSimples = "INSERT INTO ATRIBUICAO_POLO(ID_POLO,CPF,NO_FUNCAO,SG_UF,CPF_operador)" &
                                   " SELECT p.ID_POLO,'" & QueCPF & "','" & QueFuncao & "','" & QueUFbase & "','" & QueCPFoperador & "'" &
                                   " FROM POLO p WHERE p.SG_UF='" & QueUFbase & "' and p.NO_POLO='" & QueNO_POLO & "';"

            Call ExecutaSQL(sSQLremovePoloFuncao & sSQLinsereSimples)

        Catch ex As Exception
            OK = "Erro na regravação dos dados." & vbCrLf & ex.Message
        End Try

        Return OK

    End Function


    Function Atribui_Polo_Funcao(ByRef QueCPF As String, ByRef QueNO_POLO As String, ByRef QueFuncao As String, ByRef QueCPFoperador As String, ByRef QueUFbase As String) As String
        Dim OK As String = "OK"

        Try
            ' Prepara operações
            Dim sSQLremoveTudoDoCPF = "DELETE ATRIBUICAO_GESTOR WHERE CPF='" & QueCPF & "'" & ";"
            Dim sSQLremovePoloFuncao = "DELETE ATRIBUICAO_GESTOR FROM POLO p" &
                                      " WHERE p.SG_UF='" & QueUFbase & "' and p.NO_POLO='" & QueNO_POLO & "'" &
                                        " And NO_FUNCAO='" & QueFuncao & "' and p.ID_POLO=ATRIBUICAO_GESTOR.ID_POLO;"

            Dim sSQLremoveOutrasFuncoes = "DELETE ATRIBUICAO_GESTOR" &
                                         " WHERE CPF='" & QueCPF & "' And ATRIBUICAO_GESTOR.NO_FUNCAO <>'" & QueFuncao & "'" & ";"

            Dim sSQLinsereSimples = "INSERT INTO ATRIBUICAO_GESTOR(ID_POLO,CPF,NO_FUNCAO,SG_UF,CPF_operador)" &
                                   " SELECT p.ID_POLO,'" & QueCPF & "','" & QueFuncao & "','" & QueUFbase & "','" & QueCPFoperador & "'" &
                                   " FROM POLO p WHERE p.SG_UF='" & QueUFbase & "' and p.NO_POLO='" & QueNO_POLO & "';"

            Dim sSQLInsereCoordenadorEstadual = "INSERT INTO ATRIBUICAO_GESTOR(ID_POLO,CPF,NO_FUNCAO,SG_UF,CPF_operador)" &
                                         " SELECT '*','" & QueCPF & "','" & QueFuncao & "','" & QueUFbase & "','" & QueCPFoperador & "';"

            Select Case QueFuncao
                Case "Administrador", "Coordenador Estadual", "Observador"
                    ' Nesse caso, o CPF NÃO pode manter outra função;
                    Call ExecutaSQL(sSQLremoveTudoDoCPF & sSQLInsereCoordenadorEstadual)

                Case "Subcoordenador Estadual"
                    ' Nesse caso, o PCF pode manter a mesma função em diferentes polos nomeados;
                    ' Nenhuma outra função pode subsistir em qualquer estado
                    Call ExecutaSQL(sSQLremovePoloFuncao & sSQLremoveOutrasFuncoes & sSQLinsereSimples)

                Case "Coordenador de Polo", "Apoio Logístico"
                    ' Nesse caso, o CPF NÃO pode manter outra função;
                    Call ExecutaSQL(sSQLremoveTudoDoCPF & sSQLremovePoloFuncao & sSQLinsereSimples)

                Case Else
                    ' OK = "Ocorreu um erro de sistema (função inválid= " & QueFuncao
            End Select

        Catch ex As Exception
            OK = "Erro na regravação dos dados." & vbCrLf & ex.Message
        End Try

        Return OK
    End Function
    Function Remove_Colaborador_Funcao(ByRef QueCPF As String, ByRef QueFuncao As String) As String
        Dim OK As String = "OK"
        Try
            Dim sSQL As String =
            "DELETE ATRIBUICAO_GESTOR where CPF='" & QueCPF & "' and NO_FUNCAO='" & QueFuncao & "'"
            Call ExecutaSQL(sSQL)
        Catch ex As Exception
            OK = "Erro na remoçao dos dados." & vbCrLf & ex.Message
        End Try
        Return OK
    End Function


    Function Remove_Polo_Funcao(ByRef QueID_POLO As String, ByRef QueFuncao As String) As String
        Dim OK As String = "OK"
        Try
            Dim sSQL As String =
            "DELETE ATRIBUICAO_GESTOR where ID_POLO='" & QueID_POLO & "' and NO_FUNCAO='" & QueFuncao & "'"
            Call ExecutaSQL(sSQL)
        Catch ex As Exception
            OK = "Erro na remoçao dos dados." & vbCrLf & ex.Message
        End Try
        Return OK
    End Function


    Sub PreencheDropDownList(ByRef QueCombo As DropDownList, ByRef QueSQL As String, ByRef QuePrimeiroItem As String)
        ' Recupera o atributo X de um SQL genérico; serve para alimentar ComboBox like things

        Dim MyC = New System.Data.SqlClient.SqlConnection(Ligacao())
        MyC.Open()
        Dim MeuComando = New System.Data.SqlClient.SqlCommand(QueSQL, MyC)
        Dim sqlDataReader = MeuComando.ExecuteReader
        QueCombo.Items.Clear()

        If QuePrimeiroItem <> "" Then QueCombo.Items.Add(QuePrimeiroItem)
        While sqlDataReader.Read()
            QueCombo.Items.Add(sqlDataReader.GetString(sqlDataReader.GetOrdinal("X")))
        End While

        sqlDataReader.Close()
        sqlDataReader.Dispose()
        MeuComando.Dispose()
        MyC.Close()
        MyC.Dispose()

    End Sub

    'Function ColetaValorInt(ByRef QueSQL As String) As Int32
    '    Dim MeuComando = New System.Data.SqlClient.SqlCommand(QueSQL, MinhaConexao)
    '    ColetaValorInt = MeuComando.ExecuteScalar()
    '    MeuComando.Dispose()
    'End Function

    Function SenhaRandomica() As String
        Const Caracteres = "ABCDEFGHIJKLMNPQRSTUVWXYZabcdefghjkmnopqrstuvwxyz23456789!@#$%&*"
        Dim R As New Random
        Dim Senha As String = ""
        Call R.Next(Len(Caracteres))
        For k As Int16 = 1 To 6
            Senha = Senha & Mid(Caracteres, R.Next(Len(Caracteres)), 1)
        Next
        Return Senha
    End Function


    Function IsEmailDuplicado(ByRef QueCPF As String, ByRef QueEmail As String) As String
        Dim sSQL = "SELECT case when exists(select * from PESSOAL where TX_EMAIL='" & QueEmail & "' and CPF<>'" & QueCPF & "')" &
                              " THEN 'Esse e-mail está sendo usado por outro colaborador.'" &
                              " ELSE '' end X"
        IsEmailDuplicado = ColetaValorString(sSQL)
    End Function





    'Public Function ValidaCGC(QueCGC As String) As Boolean

    '    If Len(QueCGC) <> 14 Then Return False

    '    If Not IsNumeric(QueCGC) Then Return False

    '    ' Calcula o primeiro dígito
    '    Dim soma As Int16 = 0
    '    Dim mult As Int16 = 2
    '    For I = 12 To 1 Step -1
    '        soma += Int(Mid(QueCGC, I, 1)) * mult
    '        mult = IIf(mult = 9, 2, mult + 1)
    '    Next

    '    Dim D1 As Int16 = 11 - (soma Mod 11)
    '    If D1 > 9 Then D1 = 0

    '    ' Calcula o segundo dígito
    '    Dim aux As String = Mid(QueCGC, 1, 12) + Trim(Str(D1))
    '    soma = 0
    '    mult = 2
    '    For I = 13 To 1 Step -1
    '        soma += Int(Mid(aux, I, 1)) * mult
    '        mult = IIf(mult = 9, 2, mult + 1)
    '    Next

    '    Dim D2 As Int16 = 11 - (soma Mod 11)
    '    If D2 > 9 Then D2 = 0

    '    If Mid(QueCGC, 13, 2) <> Trim(Str(D1)) + Trim(Str(D2)) Then
    '        Return False
    '    Else
    '        Return True
    '    End If

    'End Function

    Public Function PrimeiroNome(ByRef QueNomeCompleto As String) As String
        Dim x As String() = QueNomeCompleto.Split(" ")
        Return x(0).Substring(0, 1) & LCase(x(0).Substring(1, x(0).Length - 1))
    End Function
    'Public Function ListaDePendencias(ByRef QuePessoaID As Int32) As String
    '    Dim tmpSTR As String = ""

    '    ' Verifica completude do cadastro
    '    Dim sSQL As String = "SELECT PESSOA_STATUS FROM PESSOAL where PESSOA_ID=" & Str(QuePessoaID)
    '    sSQL = ColetaValorString(sSQL)
    '    If sSQL <> "OK" Then
    '        tmpSTR = "Seu cadastro ainda está incompleto."
    '    End If

    '    Return tmpSTR
    'End Function

    Public Function UFdeUmEstado(ByRef QueNomeUF As String) As String
        Select Case QueNomeUF
            Case "Acre"
                Return "AC"
            Case "Amazonas"
                Return "AM"
            Case "Rondônia"
                Return "RO"
            Case "Pará"
                Return "PA"
            Case "Maranhão"
                Return "MA"
            Case "Bahia"
                Return "BA"
            Case "Goiás"
                Return "GO"
            Case "Distrito Federal"
                Return "DF"
            Case "Mato Grosso do Sul"
                Return "MS"
            Case "Rio Grande do Sul"
                Return "RS"
            Case "Piauí"
                Return "PI"
            Case Else
                Return QueNomeUF
        End Select
    End Function

    Public Function EstadoDeUmaUF(ByRef QueUF As String) As String
        Select Case QueUF
            Case "AC"
                Return "Acre"
            Case "AM"
                Return "Amazonas"
            Case "RO"
                Return "Rondônia"
            Case "PA"
                Return "Pará"
            Case "MA"
                Return "Maranhão"
            Case "BA"
                Return "Bahia"
            Case "GO"
                Return "Goiás"
            Case "DF"
                Return "Distrito Federal"
            Case "MS"
                Return "Mato Grosso do Sul"
            Case "RS"
                Return "Rio Grande do Sul"
            Case "PI"
                Return "Piauí"
            Case Else
                Return "Escolha..."
        End Select
    End Function


    Public Sub CarregaColunasDeTextoNumaLinha(ByRef QueRow As TableRow, ByRef QueValores As String, ByRef QueLL As Int16())
        Dim Cell As TableCell
        Dim Inicio = QueRow.Cells.Count

        Dim x As String() = Split(QueValores, "|")
        For k As Int16 = 0 To UBound(x)
            Cell = New TableCell With {.Text = x(k)}
            Dim Largura As Unit = New Unit(QueLL(Inicio + k), UnitType.Pixel)
            Cell.Width = Largura
            QueRow.Controls.Add(Cell)
        Next

    End Sub

    Public Sub CarregaColunaDeLinkNumaLinha(ByRef QueRow As TableRow, ByRef QueClasse As String,
                                             ByRef QueID As String, ByRef QueTexto As String, ByRef QueToolTip As String, QueTipo As String)
        Dim Inicio = QueRow.Cells.Count
        Dim MeuBotao = New Button
        MeuBotao.Text = QueTexto
        MeuBotao.ToolTip = QueToolTip
        MeuBotao.Font.Size = 8
        MeuBotao.ForeColor = Drawing.Color.Navy
        MeuBotao.BackColor = Drawing.Color.White
        MeuBotao.BorderStyle = BorderStyle.None
        Dim cell = New TableCell
        Dim Largura As Unit = New Unit(40, UnitType.Pixel)
        cell.Width = Largura
        cell.Controls.Add(MeuBotao)
        cell.Attributes.Add("OnClick", MakeJavaScriptPostBack(QueClasse, QueTipo & ";" & QueID))
        cell.Attributes.Add("style", "cursor:pointer")
        QueRow.Controls.Add(cell)
    End Sub



    Public Sub CarregaColunaDeBotaoNumaLinha(ByRef QueRow As TableRow, ByRef QueClasse As String,
                                             ByRef QueID As String, ByRef QueImagem As String, ByRef QueToolTip As String, QueTipo As String)
        Dim Inicio = QueRow.Cells.Count
        Dim MeuBotao = New ImageButton
        MeuBotao.ImageUrl = "~/FIGURAS/" & QueImagem
        MeuBotao.ToolTip = QueToolTip
        Dim cell = New TableCell
        Dim Largura As Unit = New Unit(20, UnitType.Pixel)
        cell.Width = Largura
        cell.Controls.Add(MeuBotao)
        cell.Attributes.Add("onclick", MakeJavaScriptPostBack(QueClasse, QueTipo & ";" & QueID))
        cell.Attributes.Add("style", "cursor:pointer")
        QueRow.Controls.Add(cell)
    End Sub

    Public Sub CarregaColunaDeFiguraNumaLinha(ByRef QueCell As TableCell, ByRef QueImagem As String, ByRef QueToolTip As String)
        Dim MeuBotao = New Image
        MeuBotao.ImageUrl = "~/FIGURAS/" & QueImagem
        MeuBotao.ToolTip = QueToolTip
        QueCell.Controls.Add(MeuBotao)
    End Sub

    Public Sub CarregaColunaDeBotaoCONFIRMANDONumaLinha(ByRef QueRow As TableRow, ByRef QueClasse As String,
                                                        ByRef QueID As String, ByRef QueImagem As String, ByRef QueToolTip As String, QueTipo As String,
                                                        ByRef QueTexto As String)
        Dim Inicio = QueRow.Cells.Count
        Dim MeuBotao = New ImageButton
        MeuBotao.ImageUrl = "~/FIGURAS/" & QueImagem
        MeuBotao.ToolTip = QueToolTip
        Dim cell = New TableCell
        Dim Largura As Unit = New Unit(20, UnitType.Pixel)
        cell.Width = Largura
        cell.Controls.Add(MeuBotao)
        cell.Attributes.Add("onclick", MakeJavaScriptPostBackCONFIRMANDO(QueClasse, QueTipo & ";" & QueID, QueTexto))
        cell.Attributes.Add("style", "cursor:pointer")
        QueRow.Controls.Add(cell)
    End Sub

    Public Sub CarregaColunaDeBotaoAtivoNumaLinha(ByRef QueRow As TableRow, ByRef QueClasse As String,
                                                  ByRef QueID As String, ByRef QueImagem As String, ByRef QueToolTip As String, QueTipo As String)
        Dim Inicio = QueRow.Cells.Count
        Dim MeuBotao = New ImageButton
        MeuBotao.ImageUrl = "~/FIGURAS/" & QueImagem
        MeuBotao.ToolTip = QueToolTip
        Dim cell = New TableCell
        Dim Largura As Unit = New Unit(20, UnitType.Pixel)
        cell.Width = Largura
        cell.Controls.Add(MeuBotao)
        cell.Attributes.Add("onclick", "javascript: PMS2('" & QueClasse & "','" & QueID & "')")
        cell.Attributes.Add("style", "cursor:pointer")
        QueRow.Controls.Add(cell)
    End Sub


    Function Dias(ByRef QueDia As Int16) As String
        Select Case QueDia
            Case 1
                Return "16/11 Qua"
            Case 2
                Return "17/11 Qui"
            Case 3
                Return "18/11 Sex"
            Case 4
                Return "21/11 Seg"
            Case 5
                Return "22/11 Ter"
            Case 6
                Return "23/11 Qua"
            Case 7
                Return "24/11 Qui"
            Case 8
                Return "25/11 Sex"
            Case Else
                Return "??/?? ERRO"
        End Select
    End Function

    Public Sub CarregaColunaDeImagemNumaLinha(ByRef QueRow As TableRow, ByRef QueClasse As String,
                                             ByRef QueID As String, ByRef QueImagem As String, ByRef QueToolTip As String, QueTipo As String)
        Dim Inicio = QueRow.Cells.Count
        Dim MeuBotao = New ImageButton
        MeuBotao.ImageUrl = "~/FIGURAS/" & QueImagem
        MeuBotao.ToolTip = QueToolTip
        Dim cell = New TableCell
        Dim Largura As Unit = New Unit(40, UnitType.Pixel)
        cell.Width = Largura
        cell.Controls.Add(MeuBotao)
        'cell.Attributes.Add("onclick", MakeJavaScriptPostBack(QueClasse, QueTipo & ";" & QueID))
        'cell.Attributes.Add("style", "cursor:pointer")
        QueRow.Controls.Add(cell)
    End Sub

    Public Function ValidaCGC(ByRef QueCGC As String)


        If Trim(QueCGC) = "" Then Return "É preciso fornecer um CGC. "
        If QueCGC.Length <> 14 Then Return "O CGC deve ter 14 algarismos. "
        If IncluiNaoNumericos(QueCGC) Then Return "O CGC deve ser numérico. "
        If QueCGC = "00000000000000" Then Return "O CGC digitado é inválido."

        'mult = 2
        'For I = 12 To 1 Step -1
        '    prod = prod + Val(Mid(QueCGC, I, 1)) * mult
        '    mult = IIf(mult = 9, 2, mult + 1)
        'Next
        'digito = 11 - Int(prod Mod 11)
        'digito = IIf(digito = 10 Or digito = 11, 0, digito)

        'If Val(Mid(QueCGC, 13, 1)) <> digito Then
        '    Return "O CGC digitado é inválido."
        'End If

        'mult = 2
        'For I = 13 To 1 Step -1
        '    prod = prod + Val(Mid(QueCGC, I, 1)) * mult
        '    mult = IIf(mult = 9, 2, mult + 1)
        'Next
        'digito = 11 - Int(prod Mod 11)
        'digito = IIf(digito = 10 Or digito = 11, 0, digito)

        'If Val(Mid(QueCGC, 14, 1)) <> digito Then
        '    Return "O CGC digitado é inválido."
        'End If
        Return ""
    End Function


    Public Function ValidaCPF(QueCPF As String) As String

        If Trim(QueCPF) = "" Then Return "É preciso fornecer um CPF. "
        If QueCPF.Length <> 11 Then Return "O CPF deve ter 11 algarismos. "
        If IncluiNaoNumericos(QueCPF) Then Return "O CPF deve ser numérico. "
        If QueCPF = "00000000000" Then Return "O CPF digitado é inválido."

        ' Calcula o primeiro dígito
        Dim soma As Int16 = 0
        For k As Int16 = 1 To 9
            soma += Int(Mid(QueCPF, k, 1)) * (11 - k)
        Next
        Dim D1 As Int16 = 11 - (soma Mod 11)
        If D1 > 9 Then D1 = 0

        ' Calcula o segundo dígito
        Dim aux As String = Mid(QueCPF, 1, 9) + Trim(Str(D1))
        soma = 0
        For k As Int16 = 1 To Len(aux)
            soma += Int(Mid(aux, k, 1)) * (12 - k)
        Next
        Dim D2 As Int16 = 11 - (soma Mod 11)
        If D2 > 9 Then D2 = 0

        If Mid(QueCPF, 10, 2) <> Trim(Str(D1)) + Trim(Str(D2)) Then
            Return "O CPF digitado é inválido."
        Else
            Return ""
        End If

    End Function

    Public Function ValidaPisPasep(QuePP As String) As String

        Dim Pesos As Int16() = {0, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2}

        If Trim(QuePP.Length) = "" Then Return "É preciso fornecer o número de PIS/PASEP. "
        If QuePP.Length <> 11 Then Return "O PIS/PASEP deve ter 11 algarismos. "
        If QuePP = "00000000000" Then Return "PIS/PASEP inválido. "
        If IncluiNaoNumericos(QuePP) Then Return "O PIS/PASEP deve ser numérico. "

        ' Calcula o primeiro dígito
        Dim soma As Int16 = 0
        For k As Int16 = 1 To 10
            soma += Int(Mid(QuePP, k, 1)) * Pesos(k)
        Next
        Dim D As Int16 = 11 - soma Mod 11
        If D > 9 Then D = 0

        If D = Int(Mid(QuePP, 11, 1)) Then
            Return ""
        Else
            Return "PIS/PASEP inválido. "
        End If

    End Function

    Function EmpacotaUmFiltro(ByRef QueFiltro As String, ByRef QuePosicao As Int16, ByRef QueValor As String) As String
        'Dim x As String() = Split(Parametros(gcParametroFiltros), gcSeparadorFiltros)
        Dim x As String() = Split(QueFiltro, gcSeparadorFiltros)
        x(QuePosicao) = QueValor
        ' Reconstroi a string de filtros
        Dim tmp As String = x(0)
        For k = 1 To UBound(x)
            tmp &= gcSeparadorFiltros & x(k)
        Next
        Return tmp
    End Function

End Module
