Public Class CadastroPessoal
    Inherits System.Web.UI.Page

    Dim Parametros As String()
    Dim Operacao As Char
    Dim mvPaginaAtrasada As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Se houver um erros na submissão da página (query string inválida ou  página atrasada), impede eventos posteriores
        mvPaginaAtrasada = False

        ' Verifica se a query string está OK
        Dim tmp As String = ConsisteQueryString("CadastroPessoal", Request, CompletaTamanhoMascara("SSS"), Parametros, IsPostBack, False)
        If tmp <> "OK" Then
            MensagemERRO.Text = TextoVermelho(tmp)
            mvPaginaAtrasada = True
        End If

        ' Verifica se é uma chamada para visualização
        Dim message As String = Request.QueryString("pessoa")
        If message = Nothing Then
            ' Descobre qual é a operação
            If Parametros(gcParametroNome) = "@" Then
                ' Trata-se de uma inclusão porque o nome está em branco
                Operacao = "I"
                CampoCPF.Text = Parametros(gcParametroCPF)
                Titulo.Text = "Inclusão para CPF " & EditaCPF(CampoCPF.Text)
            Else
                ' Trata-se de um update
                Operacao = "U"
                CampoCPF.Text = Parametros(gcParametroCPF)
                Titulo.Text = "Edição para CPF " & EditaCPF(Parametros(gcParametroCPF)) & " " & Parametros(gcParametroNome)
            End If
        Else
            ' Pode ser edição ou visualização
            Dim x As String() = Split(message, ",")
            If UBound(x) < 1 Then
                MensagemERRO.Text = TextoVermelho("Houve um erro de sistema: falta o parâmetro de operação. ")
                Operacao = "V"
            Else
                message = x(0)
                Operacao = x(1)
            End If

            CampoCPF.Text = message
            Titulo.Text = "Visualização de dados para CPF " & EditaCPF(message)
        End If

        ' Inclusão ou update, é preciso carregar o combo na primeira vez
        If Not IsPostBack Then
            Call PrencheBancos()
            ' Se é um update, é preciso carregar os dados do polo
            If Operacao <> "I" Then Call CarregaDados(CampoCPF.Text)
        End If

    End Sub


    Protected Sub VoltaParaQuemChamou()
        ' Volta para quem chamou
        Dim QueryStringPlus As String = ""
        Dim x As String() = Split(Parametros(gcParametroOrigem), ",")       ' Descobre se há uma query string adicional de retorno
        If x(0) = "*" Then
            ' Chamada da MasterPage
            x(0) = "BemVindo"
        End If
        Dim paginaURL As String = "~/" & x(0) & ".aspx"
        If UBound(x) = 1 Then
            QueryStringPlus = "&" & x(1)
        End If
        Response.Redirect(ResolveUrl(paginaURL & MontaQueryStringFromParametros("CadastroPOLO", x(0), Parametros)) & QueryStringPlus)
    End Sub


    Protected Sub cmdCancelar_click(sender As Object, e As EventArgs) Handles cmdCancelar.Click
        If mvPaginaAtrasada Then Exit Sub
        ' Marca um novo horário em LOGIN, pois isso impede que esta página não volte a ser submetida
        Call RelogioUpdate(Parametros(gcParametroCPF), Parametros(gcParametroSessionID))
        Call VoltaParaQuemChamou()
    End Sub

    Private Function ConsisteCampos(ByRef QueOperacao As Char) As String
        ' O parâmetro indica se é insert ou update

        Dim erros As String = ""
        Dim OK As Boolean = False

        'If Parametros(gcParametroFuncao) <> "Administrador" And Parametros(gcParametroFuncao) <> "Observador" Then
        '    If cmbEstadoBase.Text = "Brasil" Then
        '        erros &= "Desculpe, mas você não tem autorização para escolhar 'Brasil'. É preciso escolher um estado. "
        '    End If
        'End If

        If Left(cmbEstadoBase.Text, 7) = "Escolha" Then erros &= "Estado base não foi indicado. "

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

        If IncluiCaracateresInvalidos(CampoNome.Text) Then erros &= "Nome contém símbolos não permitidos. "

        If CampoDiaNascimento.Text = "" Then
            erros &= "Dia da data de nascimento não foi preenchido. "
        ElseIf IncluiNaoNumericos(CampoDiaNascimento.Text) Then
            erros &= "Dia da data de nascimento inválido. "
        ElseIf CInt(CampoDiaNascimento.Text) < 1 Or CInt(CampoDiaNascimento.Text) > 31 Then
            erros &= "Dia da data de nascimento inválido. "
        End If

        If cmbMesNascimento.Text = "" Then
            erros &= "Mês da data de nascimento não foi preenchido. "
        End If

        If Left(cmbAnoNascimento.Text, 7) = "Escolha" Then
            erros &= "Ano da data de nascimento não foi preenchido. "
        End If

        Dim M As String = NumeroDeUmMes(cmbMesNascimento.Text())
        If M = Nothing Then
            erros &= "Mês da data de nascimento inválido. "
        End If

        Dim DataNascimento As Date = TestaData(cmbAnoNascimento.Text, M, CampoDiaNascimento.Text)
        If DataNascimento = Nothing Then
            erros &= "Data de nascimento inválida. "
        End If

        If DateDiff(DateInterval.Year, DataNascimento, Now()) < 18 Then
            erros &= "Data de nascimento muito recente. "
        End If

        erros &= ValidaPisPasep(CampoPisPasep.Text)

        CampoNomeMae.Text = LimpaCampo(CampoNomeMae.Text)
        If CampoNomeMae.Text = "" Then
            erros &= "Nome da mãe não foi preenchido. "
        Else
            If CampoNomeMae.Text.Length < 10 Then
                erros &= "Nome da mãe é muito curto. "
            ElseIf CampoNomeMae.Text.Length > 50 Then
                erros &= "Nome da mãe é muito longo. "
            End If
        End If

        If IncluiCaracateresInvalidos(CampoNomeMae.Text) Then erros &= "Nome da mãe contém caracteres inválidos. "

        If Left(cmbEscolaridade.Text, 7) = "Escolha" Then erros &= "Escolaridade não foi indicada. "
        If Left(cmbGenero.Text, 7) = "Escolha" Then erros &= "Gênero não foi indicado. "
        If Left(cmbEstadoCivil.Text, 7) = "Escolha" Then erros &= "Estado civil não foi indicado. "

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

        M = NumeroDeUmMes(cmbMesExpedicao.Text())
        If M = Nothing Then
            erros &= "Mês da data de expedição inválido. "
        End If

        Dim DataExpedicao As Date = TestaData(cmbAnoExpedicao.Text, M, CampoDiaExpedicao.Text)
        If DataExpedicao = Nothing Then
            erros &= "Data de expedição inválida. "
        End If

        If DateDiff(DateInterval.Year, DataExpedicao, Now()) > 60 Then
            erros &= "Data de expedição muito antiga. "
        End If

        CampoLogradouro.Text = LimpaCampo(CampoLogradouro.Text)
        If CampoLogradouro.Text = "" Then
            erros &= "Logradouro do endereço não foi preenchido. "
        ElseIf CampoLogradouro.Text.Length < 4 Then
            erros &= "Logradouro do endereço é muito curto. "
        ElseIf CampoLogradouro.Text.Length > 70 Then
            erros &= "Logradouro do endereço é muito longo. "
        End If


        CampoNumero.Text = LimpaCampo(CampoNumero.Text)
        If CampoNumero.Text = "" Then erros &= "Número do endereço não foi preenchido. Preencha S/N se for o caso. "

        CampoBairro.Text = LimpaCampo(CampoBairro.Text)
        If CampoBairro.Text = "" Then erros &= "Bairro do endereço não foi preenchido. "

        If CampoCEP.Text = "" Then
            erros &= "CEP não foi preenchido. "
        ElseIf IncluiNaoNumericos(CampoCEP.Text) Then
            erros &= "CEP deve conter apenas números. "
        ElseIf Len(CampoCEP.Text) <> 8 Then
            erros &= "CEP deve ter oito dígitos. "
        End If

        If Left(cmbUFdoRG.Text, 7) = "Escolha" Then erros &= "UF do RG não foi indicado. "

        If Left(cmbMunicipio.Text, 7) = "Escolha" Then
            If CampoNO_MUNICIPIO.Text = "" Then
                erros &= "Município do endereço não foi indicado. "
            End If
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

        ' Conta tem que estar preenchida
        campoConta.Text = Limpa(campoConta.Text)
        If campoConta.Text = "" Then
            erros &= "Número da conta bancária não foi preenchido. "
        ElseIf IncluiNaoNumericos(campoConta.Text) Then
            erros &= "Número da conta bancária deve conter apenas dígitos numéricos. "
        ElseIf Len(campoConta.Text) < 4 Then
            erros &= "Número da conta bancária é muito curto. "
        ElseIf Len(campoConta.Text) > 12 Then
            erros &= "Número da conta bancária é muito longo. "
        End If

        ' Agência deve estar preenchida
        CampoAgencia.Text = Limpa(CampoAgencia.Text)
        If CampoAgencia.Text = "" Then
            erros &= "Código da agência bancária não foi preenchido. "
        ElseIf IncluiNaoNumericos(CampoAgencia.Text) Then
            erros &= "Código da agência bancária deve conter apenas dígitos numéricos. "
        ElseIf Len(CampoAgencia.Text) <> 4 Then
            erros &= "Código da agência bancária deve ter quatro dígitos. "
        End If

        ' Agência DV pode estar preenchida ou não
        CampoAgenciaDV.Text = UCase(limpa(CampoAgenciaDV.Text))
        If CampoAgenciaDV.Text <> "" Then
            If IncluiNaoNumericos(CampoAgenciaDV.Text) And UCase(CampoAgenciaDV.Text) <> "X" Then
                erros &= "DV da agência bancária deve ser um dígito numérico ou 'X'."
            ElseIf Len(CampoAgenciaDV.Text) <> 1 Then
                erros &= "DV da agência bancária deve ter apenas um dígito. "
            End If
        End If

        ' Conta DV deve estar preenchida
        If CampoContaDV.Text = "" Then
            erros &= "DV da conta bancária não foi preenchido. "
        ElseIf IncluiNaoNumericos(CampoContaDV.Text) And CampoContaDV.Text <> "X" Then
            erros &= "DV da conta bancária deve ser um dígito numérico ou X. "
        ElseIf Len(CampoContaDV.Text) <> 1 Then
            erros &= "DV da conta bancária deve ter apenas um dígito numérico ou X. "
        End If

        '' Tipo de conta deve estar preenchido
        'If Left(cmbTipoConta.Text, 7) = "Escolha" Then
        '    erros &= "Tipo de conta bancária não foi preenchido. "
        'End If

        'Else
        '    ' Banco não foi preenchido, não pode haver agência e nem conta
        '    If CampoAgencia.Text <> "" Or CampoAgenciaDV.Text <> "" Or campoConta.Text <> "" Or CampoContaDV.Text <> "" Then
        '        erros &= "Banco não foi indicado. "
        '    End If
        'End If

        CampoDDD1.Text = Limpa(CampoDDD1.Text)
        CampoTel1.Text = Limpa(CampoTel1.Text)

        CampoDDD2.Text = Limpa(CampoDDD2.Text)
        CampoTel2.Text = Limpa(CampoTel2.Text)

        CampoDDD3.Text = Limpa(CampoDDD3.Text)
        CampoTel3.Text = Limpa(CampoTel3.Text)

        erros &= ConsisteTelefone(CampoDDD1.Text, CampoTel1.Text, "1")
        If CampoDDD2.Text <> "" Or CampoTel2.Text <> "" Then erros &= ConsisteTelefone(CampoDDD2.Text, CampoTel2.Text, "2")
        If CampoDDD3.Text <> "" Or CampoTel3.Text <> "" Then erros &= ConsisteTelefone(CampoDDD3.Text, CampoTel3.Text, "3")

        If CampoDDD2.Text = "" And CampoTel2.Text = "" And CampoDDD3.Text <> "" And CampoTel3.Text <> "" Then
            CampoDDD2.Text = CampoDDD3.Text
            CampoDDD3.Text = ""
            CampoTel2.Text = CampoTel3.Text
            CampoTel3.Text = ""
        End If

        If CampoDDD1.Text <> "" And CampoDDD2.Text <> "" And CampoTel1.Text <> "" And CampoTel2.Text <> "" Then
            If CampoDDD1.Text = CampoDDD2.Text And CampoTel1.Text = CampoTel2.Text Then
                erros &= "Os telefones 1 e 2 são iguais."
            End If
        End If

        If CampoDDD1.Text <> "" And CampoDDD3.Text <> "" And CampoTel1.Text <> "" And CampoTel3.Text <> "" Then
            If CampoDDD1.Text = CampoDDD3.Text And CampoTel1.Text = CampoTel3.Text Then
                erros &= "Os telefones 1 e 3 são iguais."
            End If
        End If

        If CampoDDD2.Text <> "" And CampoDDD3.Text <> "" And CampoTel2.Text <> "" And CampoTel3.Text <> "" Then
            If CampoDDD2.Text = CampoDDD3.Text And CampoTel2.Text = CampoTel3.Text Then
                erros &= "Os telefones 2 e 3 são iguais."
            End If
        End If

        CampoEmail.Text = Limpa(CampoEmail.Text)
        If CampoEmail.Text = "" Then
            erros &= "E-mail não foi preenchido. "
        ElseIf Not IsEmail(CampoEmail.Text) Then
            erros &= "E-mail inválido. "
        ElseIf Len(CampoEmail.Text) > 40 Then
            erros &= "E-mail é muito longo. "
        ElseIf IncluiCaracateresNaoMAIL(CampoEmail.Text) Then
            erros &= "E-mail contém caracteres inválidos. "
        Else
            erros &= IsEmailDuplicado(Parametros(gcParametroCPF), CampoEmail.Text)


        End If

        If Operacao = "I" Then
            If CampoSenha.Text = "" Then
                erros &= "É preciso fornecer uma senha. "
            ElseIf Not VerificaSenha(CampoSenha.Text) Then
                erros &= "Senha inválida. "
            ElseIf CampoSenha.Text <> CampoConfirmaSenha.Text Then
                erros &= "A senha difere da confirmada. "
            End If
        End If

        Return erros

    End Function

    Protected Sub cmdEntrar_Click(sender As Object, e As EventArgs) Handles cmdEntrar.Click
        If mvPaginaAtrasada Then Exit Sub
        If Operacao = "V" Then Call VoltaParaQuemChamou()

        Dim tmp As String = ConsisteCampos(Operacao)
        If tmp <> "" Then
            MensagemERRO.Text = TextoVermelho(tmp)
            Exit Sub
        End If

        Select Case Operacao
            Case "I"
                tmp = INSERTcadastro()
            Case "U"
                tmp = UPDATEcadastro()
        End Select

        ' Se deu certo, retorna para algum lugar
        If tmp = "OK" Then
            ' Se for INCLUSÃO é preciso incializar o filtro com o UFbase
            ' ---------------------------------------------------------
            ' Marca um novo horário em LOGIN, pois isso impede que esta página não volte a ser submetida
            Call RelogioUpdate(Parametros(gcParametroCPF), Parametros(gcParametroSessionID))

            Parametros(gcParametroNome) = CampoNome.Text
            Parametros(gcParametroUFbase) = UFdeUmEstado(cmbEstadoBase.Text)
            If Operacao = "I" Then
                ' Inicia uma sessão        
                Parametros(gcParametroFuncao) = "Aplicador"     ' Todos entram como Aplicador
                ' Atualiza o valor do filtro da string na posição zero (UF); como é caso de inclusão, entra um filtro inicializado
                Dim Filtro As String = gcFiltrosInicializacao
                Filtro = EmpacotaUmFiltro(Filtro, 0, Parametros(gcParametroUFbase))
                ' Faz o update e acerta o estado base, que estava como 'Todos'
                Call RelogioUpdateComFiltro(Parametros(gcParametroCPF), Filtro)
                ' Volta para BemVIndo
                Response.Redirect(ResolveUrl("~/BemVindo.aspx" & MontaQueryStringFromParametros("BemVindo", "CadastroPessoal", Parametros)))
            Else
                ' Marca um novo horário em LOGIN, pois isso impede que esta página volte a ser submetida
                Call RelogioUpdate(Parametros(gcParametroCPF), Parametros(gcParametroSessionID))
                ' Se foi alteração, volta para quem chamou: pode ser POLOS ou BemVindo
                Call VoltaParaQuemChamou()
            End If
        Else
            ' Se não deu certo, fica onde está
            MensagemERRO.Text = TextoVermelho(tmp)
        End If

    End Sub




    Private Sub PreencheVariaveis(ByRef QueParameters As SqlClient.SqlParameterCollection)
        With QueParameters
            .Add("@SG_ESTADOBASE", SqlDbType.VarChar).Value = UFdeUmEstado(cmbEstadoBase.Text)
            .Add("@CPF", SqlDbType.VarChar).Value = Parametros(gcParametroCPF)
            .Add("@NO_PESSOA", SqlDbType.VarChar).Value = CampoNome.Text
            .Add("@RG_NUMERO", SqlDbType.VarChar).Value = CampoRGnumero.Text
            .Add("@RG_ORGAO", SqlDbType.VarChar).Value = CampoRGorgao.Text
            .Add("@RG_SG_UF", SqlDbType.VarChar).Value = cmbUFdoRG.Text
            .Add("@DT_EXPEDICAO", SqlDbType.DateTime).Value =
                                  PreparaDataNascimento(cmbAnoExpedicao.Text, cmbMesExpedicao.Text, CampoDiaExpedicao.Text)
            .Add("@NO_GENERO", SqlDbType.VarChar).Value = cmbGenero.Text
            .Add("@ESTADOCIVIL", SqlDbType.VarChar).Value = cmbEstadoCivil.Text
            .Add("@ESCOLARIDADE", SqlDbType.VarChar).Value = cmbEscolaridade.Text
            .Add("@PIS_PASEP", SqlDbType.VarChar).Value = CampoPisPasep.Text

            .Add("@NU_CEP", SqlDbType.VarChar).Value = CampoCEP.Text
            .Add("@DS_ENDERECO", SqlDbType.VarChar).Value = CampoLogradouro.Text
            .Add("@NU_ENDERECO", SqlDbType.VarChar).Value = CampoNumero.Text
            .Add("@DS_COMPLEMENTO_ENDERECO", SqlDbType.VarChar).Value = CampoComplemento.Text
            .Add("@DS_BAIRRO", SqlDbType.VarChar).Value = CampoBairro.Text
            .Add("@NO_MUNICIPIO", SqlDbType.VarChar).Value = CampoNO_MUNICIPIO.Text
            '.Add("@SG_UF_ENDERECO", SqlDbType.VarChar).Value = cmbUFXXXXXXX.Text
            .Add("@SG_UF_ENDERECO", SqlDbType.VarChar).Value = UFdeUmEstado(cmbEstadoBase.Text)
            .Add("@TEL1_DDD", SqlDbType.VarChar).Value = CampoDDD1.Text
            .Add("@TEL1_NUM", SqlDbType.VarChar).Value = CampoTel1.Text
            .Add("@TEL2_DDD", SqlDbType.VarChar).Value = CampoDDD2.Text
            .Add("@TEL2_NUM", SqlDbType.VarChar).Value = CampoTel2.Text
            .Add("@TEL3_DDD", SqlDbType.VarChar).Value = CampoDDD3.Text
            .Add("@TEL3_NUM", SqlDbType.VarChar).Value = CampoTel3.Text
            .Add("@TX_EMAIL", SqlDbType.VarChar).Value = CampoEmail.Text

            .Add("@DT_NASCIMENTO", SqlDbType.DateTime).Value =
                        PreparaDataNascimento(cmbAnoNascimento.Text, cmbMesNascimento.Text, CampoDiaNascimento.Text)
            .Add("@NO_MAE", SqlDbType.VarChar).Value = CampoNomeMae.Text
            .Add("@CO_BANCO", SqlDbType.VarChar).Value = SeparaCodigoBanco(cmbBanco.Text)
            .Add("@NO_BANCO", SqlDbType.VarChar).Value = SeparaNomeBanco(cmbBanco.Text)
            .Add("@CO_AGENCIA", SqlDbType.VarChar).Value = CampoAgencia.Text
            .Add("@DV_AGENCIA", SqlDbType.VarChar).Value = CampoAgenciaDV.Text
            .Add("@CO_CONTA", SqlDbType.VarChar).Value = campoConta.Text
            .Add("@DV_CONTA", SqlDbType.VarChar).Value = CampoContaDV.Text
            'If Left(cmbTipoConta.Text, 7) <> "Escolha" Then
            '    .Add("@TIPO_CONTA", SqlDbType.VarChar).Value = cmbTipoConta.Text
            'Else
            '    .Add("@TIPO_CONTA", SqlDbType.VarChar).Value = ""
            'End If
            .Add("@TIPO_CONTA", SqlDbType.VarChar).Value = "Conta corrente"     ' FIXADO ASSIM
            .Add("@SENHA", SqlDbType.VarChar).Value = Hash512(CampoSenha.Text, Parametros(gcParametroCPF))
        End With


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
                "UPDATE PESSOAL Set " &
                "SG_UF_ALOC=@SG_ESTADOBASE,CO_UF_ALOC=UFbase.CO_UF,NO_PESSOA=@NO_PESSOA," &
                "RG_NUMERO=@RG_NUMERO,RG_ORGAO= @RG_ORGAO,RG_SG_UF=@RG_SG_UF,DT_EXPEDICAO=@DT_EXPEDICAO," &
                "NO_GENERO=@NO_GENERO,ESCOLARIDADE=@ESCOLARIDADE,PIS_PASEP=@PIS_PASEP,ESTADOCIVIL=@ESTADOCIVIL," &
                "NU_CEP=@NU_CEP,DS_ENDERECO=@DS_ENDERECO,NU_ENDERECO=@NU_ENDERECO,DS_COMPLEMENTO_ENDERECO=@DS_COMPLEMENTO_ENDERECO," &
                "DS_BAIRRO=@DS_BAIRRO, CO_MUNICIPIO=m.CO_MUNICIPIO, NO_MUNICIPIO=@NO_MUNICIPIO," &
                "TEL1_DDD=@TEL1_DDD, TEL1_NUM=@TEL1_NUM, TEL2_DDD=@TEL2_DDD, TEL2_NUM= @TEL2_NUM, TEL3_DDD=@TEL3_DDD, TEL3_NUM=@TEL3_NUM, TX_EMAIL=@TX_EMAIL," &
                "CO_UF_ENDERECO=uf.CO_UF,SG_UF_ENDERECO=@SG_UF_ENDERECO," &
                "DT_NASCIMENTO=@DT_NASCIMENTO,NO_MAE=@NO_MAE," &
                "CO_BANCO =@CO_BANCO,NO_BANCO=@NO_BANCO,CO_AGENCIA=@CO_AGENCIA,DV_AGENCIA=@DV_AGENCIA,CO_CONTA=@CO_CONTA,DV_CONTA=@DV_CONTA,TIPO_CONTA=@TIPO_CONTA," &
                "AltDate=GETDATE()"

            If CampoSenha.Text <> "" Then
                tmp &= ",SENHA=@SENHA"
            End If

            tmp &= " FROM UF, MUNICIPIO m, UF UFbase" &
                    " WHERE CPF=@CPF And uf.SG_UF=@SG_UF_ENDERECO" &
                    " And m.SG_UF=@SG_UF_ENDERECO And m.NO_MUNICIPIO=@NO_MUNICIPIO" &
                    " And UFbase.SG_UF=@SG_ESTADOBASE"

            Call PreencheVariaveis(MeuComando.Parameters)

            MeuComando.CommandText = tmp
            tmp = MeuComando.ToString()
            MeuComando.ExecuteNonQuery()
        Catch ex As Exception
            OK = "Erro na regravação dos dados." & vbCrLf & ex.Message
        Finally
            MeuComando.Dispose()
        End Try
        MyC.Dispose()
        Return OK
    End Function

    Private Function INSERTcadastro() As String

        Dim MeuComando = New System.Data.SqlClient.SqlCommand()
        Dim OK As String = "OK"

        ' Abre uma conexão
        Dim MyC = New System.Data.SqlClient.SqlConnection(Ligacao())
        MyC.Open()

        Try
            MeuComando.Connection = MyC

            MeuComando.CommandText =
                "INSERT INTO PESSOAL (" &
                "SG_UF_ALOC,CO_UF_ALOC,CPF,NO_PESSOA,RG_NUMERO,RG_ORGAO,RG_SG_UF,DT_EXPEDICAO," &
                "NO_CATEGORIA,NO_GENERO,ESCOLARIDADE,PIS_PASEP,ESTADOCIVIL," &
                "NU_CEP,DS_ENDERECO,NU_ENDERECO,DS_COMPLEMENTO_ENDERECO, DS_BAIRRO, CO_MUNICIPIO, NO_MUNICIPIO," &
                "TEL1_DDD, TEL1_NUM, TEL2_DDD, TEL2_NUM, TEL3_DDD, TEL3_NUM, TX_EMAIL," &
                "CO_UF_ENDERECO,SG_UF_ENDERECO," &
                "DT_NASCIMENTO,NO_MAE,CO_BANCO,NO_BANCO,CO_AGENCIA,DV_AGENCIA,CO_CONTA,DV_CONTA,TIPO_CONTA,SENHA)" &
                "SELECT " &
                "@SG_ESTADOBASE,UFbase.CO_UF,@CPF, @NO_PESSOA, @RG_NUMERO, @RG_ORGAO, @RG_SG_UF, @DT_EXPEDICAO," &
                "'Aplicador',@NO_GENERO, @ESCOLARIDADE,@PIS_PASEP,@ESTADOCIVIL," &
                "@NU_CEP, @DS_ENDERECO, @NU_ENDERECO, @DS_COMPLEMENTO_ENDERECO, @DS_BAIRRO, m.CO_MUNICIPIO,@NO_MUNICIPIO, " &
                "@TEL1_DDD, @TEL1_NUM, @TEL2_DDD, @TEL2_NUM, @TEL3_DDD, @TEL3_NUM, @TX_EMAIL, " &
                "uf.CO_UF, uf.SG_UF," &
                "@DT_NASCIMENTO, @NO_MAE, @CO_BANCO, @NO_BANCO, @CO_AGENCIA,  @DV_AGENCIA, @CO_CONTA,@DV_CONTA, @TIPO_CONTA,@SENHA" &
                " FROM UF, MUNICIPIO m, UF UFbase" &
                " WHERE Not EXISTS (Select * from PESSOAL p where p.CPF=@CPF)" &
                " And uf.SG_UF=@SG_UF_ENDERECO" &
                " And m.SG_UF=@SG_UF_ENDERECO And m.NO_MUNICIPIO=@NO_MUNICIPIO" &
                " And UFbase.SG_UF=@SG_ESTADOBASE"

            Call PreencheVariaveis(MeuComando.Parameters)

            MeuComando.ExecuteNonQuery()

        Catch ex As Exception
            OK = "Erro na gravação dos dados." & vbCrLf & ex.Message
            Return OK
        Finally
            MeuComando.Dispose()
            MyC.Dispose()
        End Try

        Return OK

    End Function


    Private Sub CarregaDados(ByRef QueCPF As String)

        Dim tmp As String = "OK"
        Try
            Dim sSQL As String =
            "Select x.*,isnull(uf.UF_NOME,'') UF_NOME" &
            " FROM (SELECT * FROM PESSOAL WHERE CPF='" & QueCPF & "')x" &
            " LEFT JOIN UF on x.SG_UF_ALOC=uf.SG_UF"

            'Call AbreConexao()
            Dim MyC = New System.Data.SqlClient.SqlConnection(Ligacao())
            MyC.Open()

            Dim MeuComando = New System.Data.SqlClient.SqlCommand(sSQL, MyC)
            Dim RS As System.Data.SqlClient.SqlDataReader
            RS = MeuComando.ExecuteReader

            If RS.HasRows Then
                Call InstanciaCampos(RS)
            Else
                tmp = "Dados não puderam ser mostrados. "
            End If

            ' Encerra a leitura
            RS.Close()
            RS.Dispose()
            MeuComando.Dispose()
            MyC.Dispose()

        Catch ex As Exception
            tmp = "Erro na leitura dos dados." & vbCrLf & ex.Message

        End Try

        If tmp <> "OK" Then MensagemERRO.Text = TextoVermelho(tmp)

    End Sub

    Private Sub InstanciaCampos(ByRef RS As System.Data.SqlClient.SqlDataReader)

        RS.Read()

        'Preenche os campos
        'If Parametros(gcParametroFuncao) = "Administrador" Or Parametros(gcParametroFuncao) = "Observador" Then
        '    cmbEstadoBase.Visible = False
        '    LabelEstadoBAse.Visible = False
        'Else
        If RS.GetString(RS.GetOrdinal("UF_NOME")) <> "" Then
            cmbEstadoBase.Text = RS.GetString(RS.GetOrdinal("UF_NOME"))
            Dim tmp As String = RS.GetString(RS.GetOrdinal("SG_UF_ALOC"))
            Call PreencheMunicipiosDeUmEstado(cmbMunicipio, tmp, "Escolha...")

        End If
        'End If

        CampoNome.Text = RS.GetString(RS.GetOrdinal("NO_PESSOA"))

        If Not RS.IsDBNull(RS.GetOrdinal("DT_NASCIMENTO")) Then
            Dim dt As Date = RS.GetValue(RS.GetOrdinal("DT_NASCIMENTO"))
            CampoDiaNascimento.Text = Format(dt.Day(), "00")
            cmbMesNascimento.Text = NomeDeUmMes(dt.Month())
            cmbAnoNascimento.Text = Format(dt.Year(), "0000")
        End If

        CampoNomeMae.Text = RS.GetString(RS.GetOrdinal("NO_MAE"))

        CampoPisPasep.Text = RS.GetString(RS.GetOrdinal("PIS_PASEP"))

        If RS.GetString(RS.GetOrdinal("NO_GENERO")) <> "" Then
            cmbGenero.Text = RS.GetString(RS.GetOrdinal("NO_GENERO"))
        End If

        If RS.GetString(RS.GetOrdinal("ESCOLARIDADE")) <> "" Then
            cmbEscolaridade.Text = RS.GetString(RS.GetOrdinal("ESCOLARIDADE"))
        End If

        If RS.GetString(RS.GetOrdinal("ESTADOCIVIL")) <> "" Then
            cmbEstadoCivil.Text = RS.GetString(RS.GetOrdinal("ESTADOCIVIL"))
        End If

        CampoRGnumero.Text = RS.GetString(RS.GetOrdinal("RG_NUMERO"))
        CampoRGorgao.Text = RS.GetString(RS.GetOrdinal("RG_ORGAO"))
        If RS.GetString(RS.GetOrdinal("RG_SG_UF")) <> "" Then
            cmbUFdoRG.Text = RS.GetString(RS.GetOrdinal("RG_SG_UF"))
        End If

        If Not RS.IsDBNull(RS.GetOrdinal("DT_EXPEDICAO")) Then
            Dim dt As Date = RS.GetValue(RS.GetOrdinal("DT_EXPEDICAO"))
            CampoDiaExpedicao.Text = Format(dt.Day(), "00")
            cmbMesExpedicao.Text = NomeDeUmMes(dt.Month())
            cmbAnoExpedicao.Text = Format(dt.Year(), "0000")
        End If

        CampoCEP.Text = RS.GetString(RS.GetOrdinal("NU_CEP"))
        CampoLogradouro.Text = RS.GetString(RS.GetOrdinal("DS_ENDERECO"))
        CampoNumero.Text = RS.GetString(RS.GetOrdinal("NU_ENDERECO"))
        CampoComplemento.Text = RS.GetString(RS.GetOrdinal("DS_COMPLEMENTO_ENDERECO"))
        CampoBairro.Text = RS.GetString(RS.GetOrdinal("DS_BAIRRO"))

        'If RS.GetString(RS.GetOrdinal("SG_UF_ENDERECO")) <> "" Then
        '    cmbUFXXXXXXX.Text = RS.GetString(RS.GetOrdinal("SG_UF_ENDERECO"))
        '    Call PreencheMunicipiosDeUmEstado(cmbMunicipio, cmbUFXXXXXXX.Text, "Escolha...")
        'End If

        If RS.GetString(RS.GetOrdinal("NO_MUNICIPIO")) <> "" Then
            cmbMunicipio.Text = RS.GetString(RS.GetOrdinal("NO_MUNICIPIO"))
            ' Este campo é necessário porque a mudança da seleção não estava sendo reconhecida
            CampoNO_MUNICIPIO.Text = RS.GetString(RS.GetOrdinal("NO_MUNICIPIO"))
        Else
            CampoNO_MUNICIPIO.Text = ""
        End If

        CampoDDD1.Text = RS.GetString(RS.GetOrdinal("TEL1_DDD"))
        CampoTel1.Text = RS.GetString(RS.GetOrdinal("TEL1_NUM"))
        CampoDDD2.Text = RS.GetString(RS.GetOrdinal("TEL2_DDD"))
        CampoTel2.Text = RS.GetString(RS.GetOrdinal("TEL2_NUM"))
        CampoDDD3.Text = RS.GetString(RS.GetOrdinal("TEL3_DDD"))
        CampoTel3.Text = RS.GetString(RS.GetOrdinal("TEL3_NUM"))

        CampoEmail.Text = RS.GetString(RS.GetOrdinal("TX_EMAIL"))

        If RS.GetString(RS.GetOrdinal("CO_BANCO")) <> "" Then
            cmbBanco.Text = MontaCodigoBanco(RS.GetString(RS.GetOrdinal("CO_BANCO")),
                                             RS.GetString(RS.GetOrdinal("NO_BANCO")))
            CampoAgencia.Text = RS.GetString(RS.GetOrdinal("CO_AGENCIA"))
            CampoAgenciaDV.Text = Trim(RS.GetString(RS.GetOrdinal("DV_AGENCIA")))
            campoConta.Text = RS.GetString(RS.GetOrdinal("CO_CONTA"))
            CampoContaDV.Text = RS.GetString(RS.GetOrdinal("DV_CONTA"))
            'If RS.GetString(RS.GetOrdinal("TIPO_CONTA")) <> "" Then
            '    cmbTipoConta.Text = RS.GetString(RS.GetOrdinal("TIPO_CONTA"))
            'End If
            ' Eliminada o tipo de conta corrente
        End If

        If Operacao = "V" Then
            Call LockControls()
            campoConta.Visible = False
            CampoContaDV.Visible = False
            cmbAnoNascimento.Visible = False
        End If

        LabelIncAlt.Text = "Última atualização " & Format(RS.GetValue(RS.GetOrdinal("AltDate")), "dd/MM/yyyy") &
                           "    " &
                           "Incluído em " & Format(RS.GetValue(RS.GetOrdinal("IncDate")), "dd/MM/yyyy")

    End Sub

    Protected Sub LockControls()

        CampoNome.Enabled = False
        CampoNomeMae.Enabled = False
        CampoPisPasep.Enabled = False
        CampoEmail.Enabled = False
        cmbEstadoCivil.Enabled = False
        CampoNumero.Enabled = False
        CampoRGnumero.Enabled = False
        CampoRGorgao.Enabled = False
        CampoSenha.Enabled = False
        CampoConfirmaSenha.Enabled = False

        CampoBairro.Enabled = False
        CampoCEP.Enabled = False
        CampoComplemento.Enabled = False
        CampoAgencia.Enabled = False
        CampoAgenciaDV.Enabled = False
        campoConta.Enabled = False
        CampoContaDV.Enabled = False
        CampoDDD1.Enabled = False
        CampoDDD2.Enabled = False
        CampoDDD3.Enabled = False
        CampoTel1.Enabled = False
        CampoTel2.Enabled = False
        CampoTel3.Enabled = False
        CampoDiaExpedicao.Enabled = False
        CampoDiaNascimento.Enabled = False
        cmbAnoExpedicao.Enabled = False
        cmbAnoNascimento.Enabled = False
        cmbAnoExpedicao.Enabled = False
        cmbBanco.Enabled = False
        cmbEscolaridade.Enabled = False
        cmbEstadoBase.Enabled = False
        cmbEstadoCivil.Enabled = False
        cmbGenero.Enabled = False
        cmbMesExpedicao.Enabled = False
        cmbMesNascimento.Enabled = False
        cmbMunicipio.Enabled = False
        cmbTipoConta.Enabled = False
        'cmbUFXXXXXXX.Enabled = False
        cmbUFdoRG.Enabled = False
        CampoLogradouro.Enabled = False

        cmdEntrar.Text = "OK"
        cmdCancelar.Visible = False
    End Sub


    Protected Sub PrencheBancos()
        Dim sSQL As String = "SELECT DS_BANCO X FROM BANCO ORDER BY GrandeBanco desc, NO_BANCO"

        ' A procedure carrega um atributo genérico no DropDown
        Call PreencheDropDownList(cmbBanco, sSQL, "Escolha...")
    End Sub

    'Protected Sub PreencheMunicipiosDeUmEstado(ByRef QueUF As String)
    '    Dim sSQL As String = "SELECT NO_MUNICIPIO X FROM MUNICIPIO m" &
    '                        " WHERE m.SG_UF='" & QueUF & "'" &
    '                        " ORDER BY CONVERT(INT,m.Populacao / 60000) DESC,m.NO_MUNICIPIO"

    '    ' A procedure carrega um atributo genérico X no DropDown
    '    Call PreencheDropDownList(cmbMunicipio, sSQL, "Escolha...")
    '    cmbMunicipio.Enabled = True
    'End Sub

    'Protected Sub cmbUF_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbUFXXXXXXX.SelectedIndexChanged
    '    If Left(cmbUFXXXXXXX.Text, 7) = "Escolha" Then
    '        cmbMunicipio.Text = "Escolha..."
    '        cmbMunicipio.Enabled = False
    '    Else
    '        Call PreencheMunicipiosDeUmEstado(cmbMunicipio, cmbUFXXXXXXX.Text, "Escolha...")
    '    End If

    'End Sub

    Protected Sub cmbEstadoBase_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbEstadoBase.SelectedIndexChanged
        Dim tmp = UFdeUmEstado(cmbEstadoBase.Text)      ' É possível que o retorno seja "Escolha..."
        If Left(tmp, 7) <> "Escolha" Then
            'cmbUFXXXXXXX.Text = tmp
            Call PreencheMunicipiosDeUmEstado(cmbMunicipio, tmp, "Escolha...")
        End If
    End Sub

    Protected Sub cmbMunicipio_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbMunicipio.SelectedIndexChanged
        CampoNO_MUNICIPIO.Text = cmbMunicipio.Text
    End Sub
End Class