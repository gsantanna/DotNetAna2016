Public Class OrcamentoPolo2
    Inherits System.Web.UI.Page

    Dim Parametros As String()
    Dim Operacao As Char
    Dim QuePolo As String
    Dim mvPaginaAtrasada As Boolean


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Se houver um erros na submissão da página (query string inválida ou  página atrasada), impede eventos posteriores
        mvPaginaAtrasada = False

        ' Verifica se a query string está OK e se as credenciais são atendidas
        Dim tmp As String = ConsisteQueryString("OrcamentoPolo2", Request, CompletaTamanhoMascara("SSSS"), Parametros, IsPostBack, True)
        If tmp <> "OK" Then
            MensagemERRO.Text = TextoVermelho(tmp)
            mvPaginaAtrasada = True
            Exit Sub
        End If

        ' Descobre qual é a operação
        Dim QuePolo = Request.QueryString("polo")
        If QuePolo Is Nothing Then
            ' Trata-se de uma inclusão de polo porque não houve passagem de parâmetro
            Operacao = "I"
            CampoID_POLO.Enabled = False
        Else
            ' Pode ser edição ou visualização
            Dim x As String() = Split(QuePolo, ",")
            If UBound(x) < 1 Then
                MensagemERRO.Text = TextoVermelho("Houve um erro de sistema: falta o parâmetro de operação. ")
                'Exit Sub
            Else
                QuePolo = x(0)
                Operacao = x(1)
            End If
        End If

        ' Inclusão ou update, é preciso carregar o combo na primeira vez
        If Not IsPostBack Then
            ' Somente update e visualização: é preciso carregar os dados do polo
            Call PrencheBancos()
            Call CarregaDados(QuePolo)
        End If

    End Sub
    Protected Sub PrencheBancos()
        Dim sSQL As String = "SELECT DS_BANCO X FROM BANCO ORDER BY GrandeBanco desc, NO_BANCO"

        ' A procedure carrega um atributo genérico no DropDown
        Call PreencheDropDownList(cmbBanco, sSQL, "Escolha...")
    End Sub
    Protected Sub VoltaParaQuemChamou()
        ' Volta para quem chamou
        Dim QueryStringPlus As String = ""
        Dim x As String() = Split(Parametros(gcParametroOrigem), ",")       ' Descobre se há um query string adicional de retorno
        Dim paginaURL As String = "~/" & x(0) & ".aspx"
        If UBound(x) = 1 Then
            QueryStringPlus = "&" & x(1)
        End If
        Response.Redirect(ResolveUrl(paginaURL & MontaQueryStringFromParametros("CadastroPOLO", x(0), Parametros)) & QueryStringPlus)
    End Sub
    Protected Sub cmdCancelar_click(sender As Object, e As EventArgs) Handles cmdCancelar.Click
        If mvPaginaAtrasada Then Exit Sub
        Call VoltaParaQuemChamou()
    End Sub

    Private Function ConsisteCampos(ByRef QueOperacao As Char) As String
        Dim erros As String = ""
        Dim OK As Boolean = False

        If Trim(CampoCPFCGC.Text) = "" Then
            erros &= "É preciso fornecer um CPF ou CGC. "
        ElseIf Not IsNumeric(CampoCPFCGC.Text) Then
            erros &= "O CPF ou CGC deve ser numérico. "
        ElseIf Len(CampoCPFCGC.Text) = 11 Then
            erros &= ValidaCPF(CampoCPFCGC.Text)
        ElseIf Len(CampoCPFCGC.Text) = 14 Then
            erros &= ValidaCGC(CampoCPFCGC.Text)
        Else
            erros &= "É preciso fornecer um CPF ou um CGC válido(somente números). "
        End If

        CampoNome.Text = LimpaCampo(CampoNome.Text)
        If CampoNome.Text = "" Then
            erros &= "Nome não foi preenchido. "
        Else
            If CampoNome.Text.Length < 10 Then
                erros &= "Nome é muito curto. "
            ElseIf CampoNome.Text.Length > 50 Then
                erros &= "Nome é muito longo. "
            End If
        End If

        If IncluiCaracateresInvalidos(CampoNome.Text) Then
            erros &= "Nome contém símbolos não permitidos. "
        End If

        If Len(CampoCPFCGC.Text) = 14 Then
            CampoNomeResponsavel.Text = LimpaCampo(CampoNomeResponsavel.Text)
            If IncluiCaracateresInvalidos(CampoNomeResponsavel.Text) Then
                erros &= "Nome do responsável contém caracteres inválidos. "
            End If
            ' Trata-se de pessoa jurídica
            If CampoNomeResponsavel.Text = "" Then
                erros &= "Nome do responsável deve ser prenchido para pessoas jurídicas. "
            ElseIf CampoNomeResponsavel.Text.Length < 10 Then
                erros &= "Nome da mãe é muito curto. "
            ElseIf CampoNomeResponsavel.Text.Length > 50 Then
                erros &= "Nome da mãe é muito longo. "
            End If
        Else
            ' Trata-se de pessoa física
            If CampoNomeResponsavel.Text <> "" Then
                erros &= "Nome do responsável não deve ser preenchido para pessoas físicas. "
            End If
        End If

        CampoRGnumero.Text = LimpaCampo(CampoRGnumero.Text)
        If Len(CampoCPFCGC.Text) = 11 Then
            ' Trata-se de pessoa física
            CampoRGnumero.Text = LimpaCampo(CampoRGnumero.Text)
            If CampoRGnumero.Text = "" Then
                erros &= "Número do RG não foi preenchido. "
            Else
                If CampoRGnumero.Text.Length < 4 Then
                    erros &= "Número do RG é muito curto. "
                ElseIf CampoRGnumero.Text.Length > 20 Then
                    erros &= "Número do RG é muito longo. "
                End If
            End If
            CampoRGorgao.Text = LimpaCampo(CampoRGorgao.Text)
            If CampoRGorgao.Text = "" Then erros &= "Órgão emissor do RG não foi preenchido. "
            If Left(cmbUFdoRG.Text, 7) = "Escolha" Then erros &= "UF do RG não foi indicado. "

            Dim M As String = NumeroDeUmMes(cmbMesExpedicao.Text())
            If M = Nothing Then
                erros &= "Mês da data de expedição inválido. "
            End If

            Dim DataExpedicao As Date = TestaData(cmbAnoExpedicao.Text, M, CampoDiaExpedicao.Text)
            If DataExpedicao = Nothing Then
                erros &= "Data de expedição inválida. "
            ElseIf DateDiff(DateInterval.Year, DataExpedicao, Now()) > 50 Then
                erros &= "Data de expedição muito antiga. "
            End If
            'Else
            '    ' Trata-se de pessoa jurídica
            '    If CampoRGnumero.Text <> "" Or CampoRGorgao.Text <> "" Then
            '        erros &= "RG não deve ser preenchido para pessoas jurídicas. "
            '    End If
        End If

        ' O banco agora é obrigatório
        'If Left(cmbBanco.Text, 7) <> "Escolha" Then
        ' Banco tem que ser válido

        If SeparaCodigoBanco(cmbBanco.Text) = "" Then
            erros &= "Código de banco inválido. "
        End If
        If SeparaNomeBanco(cmbBanco.Text) = "" Then
            erros &= "Nome do banco inválido. "
        End If

        ' Conta tem que estar preenchido
        If campoConta.Text = "" Then
            erros &= "Número da conta bancária não foi preenchido. "
        ElseIf Not IsNumeric(campoConta.Text) Then
            erros &= "Número da conta bancária deve conter apenas dígitos numéricos. "
        ElseIf Len(campoConta.Text) < 4 Then
            erros &= "Número da conta bancária é muito curto. "
        ElseIf Len(campoConta.Text) > 12 Then
            erros &= "Número da conta bancária é muito longo. "
        End If

        ' Agência deve estar preenchida
        If CampoAgencia.Text = "" Then
            erros &= "Código da agência bancária não foi preenchido. "
        ElseIf Not IsNumeric(CampoAgencia.Text) Then
            erros &= "Código da agência bancária deve conter apenas dígitos numéricos. "
        ElseIf Len(CampoAgencia.Text) <> 4 Then
            erros &= "Código da agência bancária deve ter quatro dígitos. "
        End If

        ' Agência DV pode estar preenchida ou não
        CampoAgenciaDV.Text = UCase(CampoAgenciaDV.Text)
        If CampoAgenciaDV.Text <> "" Then
            If Not IsNumeric(CampoAgenciaDV.Text) And UCase(CampoAgenciaDV.Text) <> "X" Then
                erros &= "DV da agência bancária deve ser um dígito numérico ou 'X'."
            ElseIf Len(CampoAgenciaDV.Text) <> 1 Then
                erros &= "DV da agência bancária deve ter apenas um dígito. "
            End If
        End If

        ' Conta DV deve estar preenchida
        If CampoContaDV.Text = "" Then
            erros &= "DV da conta bancária não foi preenchido. "
        ElseIf Not IsNumeric(CampoContaDV.Text) And CampoContaDV.Text <> "X" Then
            erros &= "DV da conta bancária deve ser um dígito numérico ou X. "
        ElseIf Len(CampoContaDV.Text) <> 1 Then
            erros &= "DV da conta bancária deve ter apenas um dígito numérico ou X. "

            erros &= ConsisteTelefone(CampoDDD1.Text, CampoTel1.Text, "1")
        End If

        If CampoEmail.Text = "" Then
            erros &= "E-mail não foi preenchido. "
        ElseIf Not IsEmail(CampoEmail.Text) Then
            erros &= "E-mail inválido. "
        ElseIf Len(CampoEmail.Text) > 40 Then
            erros &= "E-mail é muito longo. "
        End If

        If CampoValor.Text = "" Then
            erros &= "Campo valor não foi preenchido. "
        ElseIf Not IsNumeric(CampoValor.Text) Then
            erros &= "Campo valor deve conter apenas números. "
        End If

        Return erros

    End Function

    Protected Sub cmdEntrar_Click(sender As Object, e As EventArgs) Handles cmdEntrar.Click
        If mvPaginaAtrasada Then Exit Sub
        ' O botão tem nova função nesse caso
        If Operacao = "V" Then Call VoltaParaQuemChamou()

        Dim tmp As String = ConsisteCampos(Operacao)
        If tmp <> "" Then
            MensagemERRO.Text = TextoVermelho(tmp)
            Exit Sub
        Else
            MensagemERRO.Text = ""
        End If

        tmp = UPDATEcadastro()

        If tmp = "OK" Then
            Call VoltaParaQuemChamou()
        Else
            MensagemERRO.Text = TextoVermelho(tmp)
        End If
    End Sub


    Private Function UPDATEcadastro() As String

        Dim MeuComando = New System.Data.SqlClient.SqlCommand()
        Dim OK As String = "OK"
        Dim tmp As String
        ' Abre uma conexão
        Dim MyC = New System.Data.SqlClient.SqlConnection(Ligacao())
        MyC.Open()
        Try
            MeuComando.Connection = MyC
            tmp =
                "UPDATE POLO Set " &
                "LOCADOR_CPFCGC=@LOCADOR_CPFCGC," &
                "LOCADOR_NOME=@LOCADOR_NOME," &
                "LOCADOR_TX_EMAIL=@LOCADOR_TX_EMAIL," &
                "LOCADOR_CO_BANCO =@CO_BANCO,LOCADOR_NO_BANCO=@NO_BANCO,LOCADOR_CO_AGENCIA=@CO_AGENCIA,LOCADOR_DV_AGENCIA=@DV_AGENCIA," &
                "LOCADOR_CO_CONTA=@CO_CONTA,LOCADOR_DV_CONTA=@DV_CONTA," &
                "LOCADOR_TEL1_DDD=@LOCADOR_TEL1_DDD," &
                "LOCADOR_TEL1_NUM=@LOCADOR_TEL1_NUM,"

            If Len(CampoCPFCGC.Text) = 11 Then
                tmp = tmp &
                "LOCADOR_RG_NUMERO=@RG_NUMERO," &
                "LOCADOR_RG_ORGAO=@RG_ORGAO," &
                "LOCADOR_RG_SG_UF=@RG_SG_UF," &
                "LOCADOR_DT_EXPEDICAO=@DT_EXPEDICAO," &
                "LOCADOR_RESPONSAVEL='',"
            Else
                tmp = tmp &
                "LOCADOR_RG_NUMERO=''," &
                "LOCADOR_RG_ORGAO=''," &
                "LOCADOR_RG_SG_UF=''," &
                "LOCADOR_DT_EXPEDICAO=NULL," &
                "LOCADOR_RESPONSAVEL=@LOCADOR_RESPONSAVEL,"
            End If

            tmp = tmp &
                "LOCADOR_VALOR=@LOCADOR_VALOR," &
                "LOCADOR_CODIGO_CONTATO=@LOCADOR_CODIGO_CONTATO," &
                "LOCADOR_OBS=@LOCADOR_OBS," &
                "AltDate=GETDATE()" &
                 " WHERE POLO.ID_POLO=@ID_POLO"

            Call PreencheVariaveis(MeuComando.Parameters)
            MeuComando.CommandText = tmp
            tmp = MeuComando.ToString()
            MeuComando.ExecuteNonQuery()
        Catch ex As Exception
            OK = "Erro na regravação dos dados." & vbCrLf & ex.Message
        Finally
            MeuComando.Dispose()
            MyC.Close()
            MyC.Dispose()
        End Try
        Return OK
    End Function


    Private Sub PreencheVariaveis(ByRef QueParameters As SqlClient.SqlParameterCollection)

        With QueParameters
            .Add("@ID_POLO", SqlDbType.VarChar).Value = CampoID_POLO.Text
            .Add("@NO_POLO", SqlDbType.VarChar).Value = CampoNO_POLO.Text
            .Add("@LOCADOR_CPFCGC", SqlDbType.VarChar).Value = CampoCPFCGC.Text
            .Add("@LOCADOR_NOME", SqlDbType.VarChar).Value = CampoNome.Text
            .Add("@LOCADOR_TX_EMAIL", SqlDbType.VarChar).Value = CampoEmail.Text

            .Add("@CO_BANCO", SqlDbType.VarChar).Value = SeparaCodigoBanco(cmbBanco.Text)
            .Add("@NO_BANCO", SqlDbType.VarChar).Value = SeparaNomeBanco(cmbBanco.Text)
            .Add("@CO_AGENCIA", SqlDbType.VarChar).Value = CampoAgencia.Text
            .Add("@DV_AGENCIA", SqlDbType.VarChar).Value = CampoAgenciaDV.Text
            .Add("@CO_CONTA", SqlDbType.VarChar).Value = campoConta.Text
            .Add("@DV_CONTA", SqlDbType.VarChar).Value = CampoContaDV.Text
            .Add("@LOCADOR_TEL1_DDD", SqlDbType.VarChar).Value = CampoDDD1.Text
            .Add("@LOCADOR_TEL1_NUM", SqlDbType.VarChar).Value = CampoTel1.Text

            If Len(CampoCPFCGC.Text) = 11 Then
                .Add("@RG_NUMERO", SqlDbType.VarChar).Value = CampoRGnumero.Text
                .Add("@RG_ORGAO", SqlDbType.VarChar).Value = CampoRGorgao.Text
                .Add("@RG_SG_UF", SqlDbType.VarChar).Value = cmbUFdoRG.Text
                .Add("@DT_EXPEDICAO", SqlDbType.DateTime).Value =
                                  PreparaDataNascimento(cmbAnoExpedicao.Text, cmbMesExpedicao.Text, CampoDiaExpedicao.Text)
                .Add("@LOCADOR_RESPONSAVEL", SqlDbType.VarChar).Value = ""
            Else
                .Add("@LOCADOR_RESPONSAVEL", SqlDbType.VarChar).Value = CampoNomeResponsavel.Text
                '    .Add("@RG_NUMERO", SqlDbType.VarChar).Value = ""
                '    .Add("@RG_ORGAO", SqlDbType.VarChar).Value = ""
                '    .Add("@RG_SG_UF", SqlDbType.VarChar).Value = ""
                '    .Add("@DT_EXPEDICAO", SqlDbType.DateTime).Value = DBNull.Value
                '    .Add("@LOCADOR_RESPONSAVEL", SqlDbType.DateTime).Value = CampoNomeResponsavel.Text
            End If
            .Add("@LOCADOR_VALOR", SqlDbType.Money).Value = CampoValor.Text
            .Add("@LOCADOR_CODIGO_CONTATO", SqlDbType.VarChar).Value = CampoCodigoContrato.Text
            .Add("@LOCADOR_OBS", SqlDbType.VarChar).Value = CampoOBS.Text
        End With

    End Sub

    Private Sub CarregaDados(ByRef QueID_POLO As String)

        Dim OK As String = "OK"
        Try
            Dim sSQL As String =
            "Select  p.* FROM POLO p WHERE p.ID_POLO='" & QueID_POLO & "'"

            ' Abre uma conexão
            Dim MyC = New System.Data.SqlClient.SqlConnection(Ligacao())
            MyC.Open()

            Dim MeuComando = New System.Data.SqlClient.SqlCommand(sSQL, MyC)
            Dim RS As System.Data.SqlClient.SqlDataReader
            RS = MeuComando.ExecuteReader

            If RS.HasRows Then
                Call InstanciaCampos(RS)
            Else
                OK = "Dados não puderam ser mostrados. "
            End If

            ' Encerra a leitura
            RS.Close()
            RS.Dispose()
            MeuComando.Dispose()
            MyC.Dispose()

        Catch ex As Exception
            OK = "Erro na leitura dos dados de polos." & vbCrLf & ex.Message
        End Try
        If OK <> "OK" Then MensagemERRO.Text = TextoVermelho(OK)
    End Sub

    Private Sub InstanciaCampos(ByRef RS As System.Data.SqlClient.SqlDataReader)

        RS.Read()

        'Preenche os campos
        CampoID_POLO.Text = RS.GetString(RS.GetOrdinal("ID_POLO"))
        CampoNO_POLO.Text = RS.GetString(RS.GetOrdinal("NO_POLO"))
        CampoSG_UF.Text = RS.GetString(RS.GetOrdinal("SG_UF"))
        CampoCPFCGC.Text = Trim(RS.GetString(RS.GetOrdinal("LOCADOR_CPFCGC")))
        CampoNome.Text = RS.GetString(RS.GetOrdinal("LOCADOR_NOME"))
        CampoNomeResponsavel.Text = RS.GetString(RS.GetOrdinal("LOCADOR_RESPONSAVEL"))
        CampoRGnumero.Text = RS.GetString(RS.GetOrdinal("LOCADOR_RG_NUMERO"))
        CampoRGorgao.Text = RS.GetString(RS.GetOrdinal("LOCADOR_RG_ORGAO"))
        If RS.GetString(RS.GetOrdinal("LOCADOR_RG_SG_UF")) <> "" Then
            cmbUFdoRG.Text = RS.GetString(RS.GetOrdinal("LOCADOR_RG_SG_UF"))
        End If

        If Not RS.IsDBNull(RS.GetOrdinal("LOCADOR_DT_EXPEDICAO")) Then
            Dim dt As Date = RS.GetValue(RS.GetOrdinal("LOCADOR_DT_EXPEDICAO"))
            CampoDiaExpedicao.Text = Format(dt.Day(), "00")
            cmbMesExpedicao.Text = NomeDeUmMes(dt.Month())
            cmbAnoExpedicao.Text = Format(dt.Year(), "0000")
        End If

        CampoDDD1.Text = RS.GetString(RS.GetOrdinal("LOCADOR_TEL1_DDD"))
        CampoTel1.Text = RS.GetString(RS.GetOrdinal("LOCADOR_TEL1_NUM"))

        CampoEmail.Text = RS.GetString(RS.GetOrdinal("LOCADOR_TX_EMAIL"))
        CampoOBS.Text = RS.GetString(RS.GetOrdinal("LOCADOR_OBS"))

        If RS.GetString(RS.GetOrdinal("LOCADOR_CO_BANCO")) <> "" Then
            cmbBanco.Text = MontaCodigoBanco(RS.GetString(RS.GetOrdinal("LOCADOR_CO_BANCO")),
                                             RS.GetString(RS.GetOrdinal("LOCADOR_NO_BANCO")))
            CampoAgencia.Text = RS.GetString(RS.GetOrdinal("LOCADOR_CO_AGENCIA"))
            CampoAgenciaDV.Text = Trim(RS.GetString(RS.GetOrdinal("LOCADOR_DV_AGENCIA")))
            campoConta.Text = RS.GetString(RS.GetOrdinal("LOCADOR_CO_CONTA"))
            CampoContaDV.Text = RS.GetString(RS.GetOrdinal("LOCADOR_DV_CONTA"))
        End If

        CampoValor.Text = Format(RS.GetValue(RS.GetOrdinal("LOCADOR_VALOR")), "######0.00")
        CampoCodigoContrato.Text = RS.GetString(RS.GetOrdinal("LOCADOR_CODIGO_CONTATO"))

        LabelIncAlt.Text = "Última atualização " & Format(RS.GetValue(RS.GetOrdinal("AltDate")), "dd/MM/yyyy") &
                           "    " &
                           "Incluído em " & Format(RS.GetValue(RS.GetOrdinal("IncDate")), "dd/MM/yyyy")

    End Sub

End Class