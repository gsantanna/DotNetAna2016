Public Class CadastroPOLO
    Inherits System.Web.UI.Page

    Dim Parametros As String()
    Dim Operacao As Char
    Dim QuePolo As String
    Dim mvPaginaAtrasada As Boolean


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Se houver um erro na submissão da página (query string inválida ou  página atrasada), isso ignora eventos posteriores
        mvPaginaAtrasada = False

        ' Verifica se a query string está OK e se as credenciais são atendidas
        Dim tmp As String = ConsisteQueryString("CadastroPOLO", Request, CompletaTamanhoMascara("SSSSS"), Parametros, IsPostBack, True)
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
            cmbPoloEntrega.Enabled = False
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
            Call CarregaPolosDeUmEstado(cmbPoloEntrega, Parametros(gcParametroUFbase), "Escolha...")
            ' Se é um update, é preciso carregar os dados do polo depois de preencher os gestores
            If Operacao = "U" Or Operacao = "V" Then
                Call CarregaDados(QuePolo)
            Else
                Call PreencheCoordenadores(Parametros(gcParametroUFbase),
                                           "",
                                           "",
                                           cmbSubCoordenadorEst)
            End If
        End If

    End Sub

    Protected Sub VoltaParaQuemChamou()
        '' Volta para quem chamou
        'Dim QueryStringPlus As String = ""
        'Dim x As String() = Split(Parametros(gcParametroOrigem), ",")       ' Descobre se há um query string adicional de retorno
        'Dim paginaURL As String = "~/" & x(0) & ".aspx"
        'If UBound(x) = 1 Then
        '    QueryStringPlus = "&" & x(1)
        'End If
        'Response.Redirect(ResolveUrl(paginaURL & MontaQueryStringFromParametros("CadastroPOLO", x(0), Parametros)) & QueryStringPlus)

        '    Protected Sub VoltaParaQuemChamou()
        ' Volta para quem chamou
        Dim QueryStringPlus As String = ""
        Dim x As String() = Split(Parametros(gcParametroOrigem), ",")       ' Descobre se há um query string adicional de retorno
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
    'End Sub
    Protected Sub cmdCancelar_click(sender As Object, e As EventArgs) Handles cmdCancelar.Click
        If mvPaginaAtrasada Then Exit Sub
        Call VoltaParaQuemChamou()
    End Sub

    Private Function ConsisteCampos(ByRef QueOperacao As Char) As String
        ' O parâmetro indica se é insert ou update

        Dim erros As String = ""
        Dim OK As Boolean = False

        If Operacao = "U" Then
            CampoID_POLO.Text = Trim(CampoID_POLO.Text)
            ' Código tem que estar preenchido se for UPDATE
            If CampoID_POLO.Text = "" Then
                erros &= "Código do polo não foi preenchido. "
            ElseIf Not IsNumeric(CampoID_POLO.Text) Then
                erros &= "Código do polo deve conter apenas dígitos numéricos. "
            End If
        End If

        CampoNO_POLO.Text = LimpaCampo(CampoNO_POLO.Text)
        If CampoNO_POLO.Text = "" Then
            erros &= "Nome não foi preenchido. "
        Else
            If CampoNO_POLO.Text.Length < 3 Then erros &= "Nome é muito curto. "
        End If

        CampoCEP.Text = LimpaCampo(CampoCEP.Text)
        CampoLogradouro.Text = LimpaCampo(CampoLogradouro.Text)
        CampoComplemento.Text = LimpaCampo(CampoComplemento.Text)
        CampoNumero.Text = LimpaCampo(CampoNumero.Text)
        CampoBairro.Text = LimpaCampo(CampoBairro.Text)

        If CampoCEP.Text <> "" Or CampoLogradouro.Text <> "" Or CampoNumero.Text <> "" Or CampoComplemento.Text <> "" Or CampoBairro.Text <> "" Then
            If CampoCEP.Text = "" Then
                erros &= "CEP não foi preenchido. "
            ElseIf Not IsNumeric(CampoCEP.Text) Then
                erros &= "CEP deve conter apenas números. "
            ElseIf Len(CampoCEP.Text) <> 8 Then
                erros &= "CEp deve ter oito dígitos. "
            End If

            If CampoLogradouro.Text = "" Then
                erros &= "Logradouro do endereço não foi preenchido. "
            ElseIf CampoLogradouro.Text.Length < 4 Then
                erros &= "Logradouro do endereço é muito curto. "
            End If

            If CampoNumero.Text = "" Then erros &= "Número do endereço não foi preenchido. Preencha S/N se for o caso. "

            If CampoBairro.Text = "" Then erros &= "Bairro do endereço não foi preenchido. "

        End If

        If Left(cmbUF.Text, 7) = "Escolha" Then erros &= "UF do polo não foi indicado. "

        If Left(cmbMunicipio.Text, 7) = "Escolha" Then
            If CampoNO_MUNICIPIO.Text = "" Then
                erros &= "Município do endereço não foi indicado. "
            End If
        End If

        erros &= ConsisteTelefone(CampoDDD1.Text, CampoTel1.Text, "1")
        If CampoDDD2.Text <> "" Or CampoTel2.Text <> "" Then
            erros &= ConsisteTelefone(CampoDDD2.Text, CampoTel2.Text, "2")
        End If

        If CampoDDD1.Text <> "" And CampoDDD2.Text <> "" And CampoTel1.Text <> "" And CampoTel2.Text <> "" Then
            If CampoDDD1.Text = CampoDDD2.Text And CampoTel1.Text = CampoTel2.Text Then
                erros &= "Os dois telefones são iguais."
            End If
        End If

        If CampoEmail.Text <> "" Then
            If CampoEmail.Text = "" Then
                erros &= "E-mail não foi preenchido. "
            ElseIf Not IsEmail(CampoEmail.Text) Then
                erros &= "E-mail inválido. "
            End If
        End If

        If Operacao = "I" Then
            erros &= VerificaDuplicidade()
        End If
        Return erros

    End Function

    Protected Function VerificaDuplicidade()
        Dim sSQL As String =
            "SELECT case when" &
            " exists(SELECT * FROM POLO WHERE NO_POLO='" & CampoNO_POLO.Text & "'" &
                                        " And SG_UF='" & cmbUF.Text & "')" &
                     " then 'Já existe um polo com esse nome nesse estado.'" &
                     " else '' END"
        Return ColetaValorString(sSQL)
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

        Select Case Operacao
            Case "I"
                tmp = INSERTcadastro()
            Case "U"
                tmp = UPDATEcadastro()
        End Select

        If tmp = "OK" Then
            If Left(cmbCoordenador.Text, 6) = "Nenhum" Then
                Remove_Polo_Funcao(CampoID_POLO.Text, "Coordenador de Polo")
            Else
                Atribui_Polo_Funcao(SeparaCPFCoordenador(cmbCoordenador.Text), CampoNO_POLO.Text, "Coordenador de Polo", Parametros(gcParametroCPF), cmbUF.Text)
            End If

            If Left(cmbApoioLogistico.Text, 6) = "Nenhum" Then
                Remove_Polo_Funcao(CampoID_POLO.Text, "Apoio Logístico")
            Else
                Atribui_Polo_Funcao(SeparaCPFCoordenador(cmbApoioLogistico.Text), CampoNO_POLO.Text, "Apoio Logístico", Parametros(gcParametroCPF), cmbUF.Text)
            End If

            If Left(cmbSubCoordenadorEst.Text, 6) = "Nenhum" Then
                Remove_Polo_Funcao(CampoID_POLO.Text, "Subcoordenador Estadual")
            Else
                Atribui_Polo_Funcao(SeparaCPFCoordenador(cmbSubCoordenadorEst.Text), CampoNO_POLO.Text, "Subcoordenador Estadual", Parametros(gcParametroCPF), cmbUF.Text)
            End If

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

            ' Usa dois SELECT para forçar o valor nulo
            tmp =
                "UPDATE POLO Set " &
                "NO_POLO=@NO_POLO," &
                "NU_CEP=@NU_CEP,DS_ENDERECO=@DS_ENDERECO,NU_ENDERECO=@NU_ENDERECO,DS_COMPLEMENTO_ENDERECO=@DS_COMPLEMENTO_ENDERECO," &
                "DS_BAIRRO=@DS_BAIRRO, CO_MUNICIPIO=m.CO_MUNICIPIO, NO_MUNICIPIO=@NO_MUNICIPIO," &
                "TEL1_DDD=@TEL1_DDD, TEL1_NUM=@TEL1_NUM, TEL2_DDD=@TEL2_DDD, TEL2_NUM= @TEL2_NUM,TX_EMAIL=@TX_EMAIL," &
                "CO_UF=uf.CO_UF,SG_UF=@SG_UF_ENDERECO,TX_OBS=@TX_OBS," &
                "ID_POLO_ENTREGA = isnull(pe.ID_POLO,POLO.ID_POLO)," &
                "NATUREZA_LOCAL=@NATUREZA_LOCAL,STATUS_NEGOCIACAO=@STATUS_NEGOCIACAO," &
                "AltDate=GETDATE()" &
                 " FROM UF, MUNICIPIO m, (SELECT(SELECT ID_POLO FROM POLO pp WHERE pp.NO_POLO=@NO_POLO_ENTREGA And pp.SG_UF=@SG_UF_ENDERECO) ID_POLO) pe" &
                    " WHERE POLO.ID_POLO=@ID_POLO" &
                      " And m.NO_MUNICIPIO=@NO_MUNICIPIO And UF.SG_UF=@SG_UF_ENDERECO And m.SG_UF=@SG_UF_ENDERECO"

            Call PreencheVariaveis(MeuComando.Parameters)

            MeuComando.CommandText = tmp
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

    Private Function INSERTcadastro()

        Dim MeuComando = New System.Data.SqlClient.SqlCommand()
        Dim OK As String = "OK"
        Dim tmp As String
        ' Abre uma conexão
        Dim MyC = New System.Data.SqlClient.SqlConnection(Ligacao())
        MyC.Open()
        Try
            MeuComando.Connection = MyC
            ' Não grava endereço alternativo na inclusão
            tmp =
                "INSERT INTO POLO (" &
                "ID_POLO,NO_POLO," &
                "NU_CEP,DS_ENDERECO,NU_ENDERECO,DS_COMPLEMENTO_ENDERECO," &
                "DS_BAIRRO, CO_MUNICIPIO, NO_MUNICIPIO," &
                "TEL1_DDD, TEL1_NUM, TEL2_DDD, TEL2_NUM,TX_EMAIL," &
                "CO_UF,SG_UF,TX_OBS,NATUREZA_LOCAL,STATUS_NEGOCIACAO,)" &
                "Select " &
                "@ID_POLO,@NO_POLO," &
                "@NU_CEP,@DS_ENDERECO,@NU_ENDERECO,@DS_COMPLEMENTO_ENDERECO," &
                "@DS_BAIRRO, m.CO_MUNICIPIO, @NO_MUNICIPIO," &
                "@TEL1_DDD, @TEL1_NUM, @TEL2_DDD, @TEL2_NUM,@TX_EMAIL," &
                "uf.CO_UF,uf.SG_UF,@TX_OBS,@NATUREZA_LOCAL,@STATUS_NEGOCIACAO" &
                " FROM UF, MUNICIPIO m" &
                " WHERE Not EXISTS (Select * from POLO p where p.ID_POLO=@ID_POLO)" &
                " And uf.SG_UF=@SG_UF_ENDERECO" &
                " And m.SG_UF=@SG_UF_ENDERECO And m.NO_MUNICIPIO=@NO_MUNICIPIO" &
                ";" &
                "UPDATE POLO Set ID_POLO=format(ID,'0000'),ID_POLO_ENTREGA=format(ID,'0000') where ID=(Select SCOPE_IDENTITY());" &
                "UPDATE POLO set CO_DISTRIBUIDORA = c.CO_DISTRIBUIDORA" &
                " from CORREIO c where c.CO_MUNICIPIO_DISTRIBUIDORA = POLO.CO_MUNICIPIO and POLO.ID=(Select SCOPE_IDENTITY())" &
                " and ( POLO.NU_CEP between c.NU_CEP_INICIAL and c.NU_CEP_FINAL or (c.NU_CEP_INICIAL = '00000000' and c.NU_CEP_FINAL = '00000000'))"

            Call PreencheVariaveis(MeuComando.Parameters)
            MeuComando.CommandText = tmp
            MeuComando.ExecuteNonQuery()
        Catch ex As Exception
            OK = "Erro na gravação dos dados." & vbCrLf & ex.Message
        Finally
            MeuComando.Dispose()
        End Try
        MyC.Dispose()
        Return OK
    End Function
    Private Sub PreencheVariaveis(ByRef QueParameters As SqlClient.SqlParameterCollection)

        With QueParameters
            .Add("@ID_POLO", SqlDbType.VarChar).Value = CampoID_POLO.Text
            .Add("@USUARIO", SqlDbType.VarChar).Value = Parametros(gcParametroCPF)
            .Add("@NO_POLO", SqlDbType.VarChar).Value = CampoNO_POLO.Text
            .Add("@NU_CEP", SqlDbType.VarChar).Value = CampoCEP.Text
            .Add("@DS_ENDERECO", SqlDbType.VarChar).Value = CampoLogradouro.Text
            .Add("@NU_ENDERECO", SqlDbType.VarChar).Value = CampoNumero.Text
            .Add("@DS_COMPLEMENTO_ENDERECO", SqlDbType.VarChar).Value = CampoComplemento.Text
            .Add("@DS_BAIRRO", SqlDbType.VarChar).Value = CampoBairro.Text
            .Add("@NO_MUNICIPIO", SqlDbType.VarChar).Value = CampoNO_MUNICIPIO.Text
            .Add("@SG_UF_ENDERECO", SqlDbType.VarChar).Value = cmbUF.Text
            .Add("@TEL1_DDD", SqlDbType.VarChar).Value = CampoDDD1.Text
            .Add("@TEL1_NUM", SqlDbType.VarChar).Value = CampoTel1.Text
            .Add("@TEL2_DDD", SqlDbType.VarChar).Value = CampoDDD2.Text
            .Add("@TEL2_NUM", SqlDbType.VarChar).Value = CampoTel2.Text
            .Add("@TX_EMAIL", SqlDbType.VarChar).Value = CampoEmail.Text
            .Add("@TX_OBS", SqlDbType.VarChar).Value = CampoOBS.Text
            If Left(cmbCoordenador.Text, 6) = "Nenhum" Then
                .Add("@COORDENADOR_CPF", SqlDbType.VarChar).Value = DBNull.Value
            Else
                .Add("@COORDENADOR_CPF", SqlDbType.VarChar).Value = SeparaCPFCoordenador(cmbCoordenador.Text)
            End If

            If Left(cmbApoioLogistico.Text, 6) = "Nenhum" Then
                .Add("@APOIO_LOGISTICO_CPF", SqlDbType.VarChar).Value = DBNull.Value
            Else
                .Add("@APOIO_LOGISTICO_CPF", SqlDbType.VarChar).Value = SeparaCPFCoordenador(cmbApoioLogistico.Text)
            End If

            If Left(cmbSubCoordenadorEst.Text, 6) = "Nenhum" Then
                .Add("@SUBCOORDENADOR_CPF", SqlDbType.VarChar).Value = DBNull.Value
            Else
                .Add("@SUBCOORDENADOR_CPF", SqlDbType.VarChar).Value = SeparaCPFCoordenador(cmbSubCoordenadorEst.Text)
            End If

            If Left(cmbPoloEntrega.Text, 7) <> "Escolha" Then
                .Add("@NO_POLO_ENTREGA", SqlDbType.VarChar).Value = cmbPoloEntrega.Text
            Else
                .Add("@NO_POLO_ENTREGA", SqlDbType.VarChar).Value = ""
            End If

            .Add("@NATUREZA_LOCAL", SqlDbType.VarChar).Value = cmbInstalacao.Text
            .Add("@STATUS_NEGOCIACAO", SqlDbType.VarChar).Value = cmbNegociacao.Text

        End With

    End Sub

    Private Sub CarregaDados(ByRef QueID_POLO As String)

        Dim OK As String = "OK"
        Try

            Dim sSQL As String =
            "SELECT  isnull(pp0.CPF,'') COORDENADOR_CPF,isnull(pp0.NO_PESSOA,'') NO_PESSOA,isnull(pp0.TX_EMAIL,'') TX_EMAILcoord," &
                   " isnull(pp1.CPF,'') APOIO_LOGISTICO_CPF,isnull(pp1.NO_PESSOA,'') NO_PESSOA_al,isnull(pp1.TX_EMAIL,'') TX_EMAIL_al," &
                   " isnull(pp2.CPF,'') SUBCOORDENADOR_CPF,isnull(pp2.NO_PESSOA,'') NO_PESSOA_sub,isnull(pp2.TX_EMAIL,'') TX_EMAIL_sub," &
                   " isnull(pe.NO_POLO,'') NO_POLO_ENTREGA,p.*," &
                   " (SELECT count(*) FROM ESCOLA e where e.ID_POLO='" & QueID_POLO & "') QT_ESCOLAS," &
                   " (SELECT count(*) FROM ESCOLA e, TURMA t WHERE e.ID_POLO='" & QueID_POLO & "'" &
                                                           " And e.ID_ESCOLA=t.ID_ESCOLA) QT_TURMAS," &
                   " case when (SELECT Data_Fechamento FROM UF WHERE UF.SG_UF=p.SG_UF) < GETDATE() then 'S' else 'N' end ESTADO_FECHADO" &
                   " FROM(Select * FROM POLO WHERE ID_POLO='" & QueID_POLO & "') p" &
                                      " Left Join(SELECT p.NO_PESSOA, p.CPF, p.TX_EMAIL, a.ID_POLO" &
                                                " From PESSOAL p, ATRIBUICAO_GESTOR a  Where p.CPF = a.CPF And a.NO_FUNCAO ='Coordenador de Polo')pp0 on p.ID_POLO=pp0.ID_POLO" &
                                      " Left Join (Select p.NO_PESSOA, p.CPF, p.TX_EMAIL, a.ID_POLO" &
                                                   " From PESSOAL p, ATRIBUICAO_GESTOR a  Where p.CPF = a.CPF And a.NO_FUNCAO ='Apoio Logístico')pp1 on p.ID_POLO=pp1.ID_POLO" &
                                      " Left Join (Select p.NO_PESSOA, p.CPF, p.TX_EMAIL, a.ID_POLO" &
                                                   " From PESSOAL p, ATRIBUICAO_GESTOR a  Where p.CPF = a.CPF And a.NO_FUNCAO ='Subcoordenador Estadual')pp2 on p.ID_POLO=pp2.ID_POLO" &
                                      " Left Join POLO pe on p.ID_POLO_ENTREGA=pe.ID_POLO"


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
        If Not RS.IsDBNull(RS.GetOrdinal("COORDENADOR_CPF")) Then
            Call PreencheCoordenadores(Parametros(gcParametroUFbase),
                                   RS.GetString(RS.GetOrdinal("NO_MUNICIPIO")),
                                   RS.GetString(RS.GetOrdinal("COORDENADOR_CPF")),
                                   cmbCoordenador)
            cmbCoordenador.Text = MontaNomeCoordenador(RS.GetString(RS.GetOrdinal("COORDENADOR_CPF")),
                                                       RS.GetString(RS.GetOrdinal("NO_PESSOA")))
            CampoEmailcoord.Text = RS.GetString(RS.GetOrdinal("TX_EMAIL"))
        Else
            Call PreencheCoordenadores(Parametros(gcParametroUFbase),
                                   RS.GetString(RS.GetOrdinal("NO_MUNICIPIO")),
                                   "",
                                   cmbCoordenador)
            CampoEmailcoord.Text = ""
        End If


        If Not RS.IsDBNull(RS.GetOrdinal("APOIO_LOGISTICO_CPF")) Then
            Call PreencheCoordenadores(Parametros(gcParametroUFbase),
                                   RS.GetString(RS.GetOrdinal("NO_MUNICIPIO")),
                                   RS.GetString(RS.GetOrdinal("APOIO_LOGISTICO_CPF")),
                                   cmbApoioLogistico)
            cmbApoioLogistico.Text = MontaNomeCoordenador(RS.GetString(RS.GetOrdinal("APOIO_LOGISTICO_CPF")),
                                                          RS.GetString(RS.GetOrdinal("NO_PESSOA_al")))
            CampoEmail_al.Text = RS.GetString(RS.GetOrdinal("TX_EMAIL_al"))
        Else
            Call PreencheCoordenadores(Parametros(gcParametroUFbase),
                                   RS.GetString(RS.GetOrdinal("NO_MUNICIPIO")),
                                   "",
                                   cmbApoioLogistico)
            CampoEmail_al.Text = ""
        End If

        If Not RS.IsDBNull(RS.GetOrdinal("SUBCOORDENADOR_CPF")) Then
            Call PreencheCoordenadores(Parametros(gcParametroUFbase),
                                   "",
                                   RS.GetString(RS.GetOrdinal("SUBCOORDENADOR_CPF")),
                                   cmbSubCoordenadorEst)
            cmbSubCoordenadorEst.Text = MontaNomeCoordenador(RS.GetString(RS.GetOrdinal("SUBCOORDENADOR_CPF")),
                                                             RS.GetString(RS.GetOrdinal("NO_PESSOA_sub")))
            CampoEmailsubc.Text = RS.GetString(RS.GetOrdinal("TX_EMAIL_sub"))
        Else
            Call PreencheCoordenadores(Parametros(gcParametroUFbase),
                                   "",
                                   "",
                                   cmbSubCoordenadorEst)
            CampoEmailsubc.Text = ""
        End If

        CampoCEP.Text = RS.GetString(RS.GetOrdinal("NU_CEP"))
        CampoLogradouro.Text = RS.GetString(RS.GetOrdinal("DS_ENDERECO"))
        CampoNumero.Text = RS.GetString(RS.GetOrdinal("NU_ENDERECO"))
        CampoComplemento.Text = RS.GetString(RS.GetOrdinal("DS_COMPLEMENTO_ENDERECO"))
        CampoBairro.Text = RS.GetString(RS.GetOrdinal("DS_BAIRRO"))

        If RS.GetString(RS.GetOrdinal("SG_UF")) <> "" Then
            cmbUF.Text = RS.GetString(RS.GetOrdinal("SG_UF"))
            Call PreencheMunicipiosDeUmEstado(cmbMunicipio, cmbUF.Text, "Escolha...")
        End If

        If RS.GetString(RS.GetOrdinal("NO_MUNICIPIO")) <> "" Then
            cmbMunicipio.Enabled = True
            cmbMunicipio.Text = RS.GetString(RS.GetOrdinal("NO_MUNICIPIO"))
            'Dim a As Int16 = cmbMunicipio.Items.IndexOf(cmbMunicipio.Items.FindByValue(RS.GetString(RS.GetOrdinal("NO_MUNICIPIO"))))
            ' Este campo é necessário porque a mudança da seleção não estava sendo reconhecida
            CampoNO_MUNICIPIO.Text = RS.GetString(RS.GetOrdinal("NO_MUNICIPIO"))
        End If

        CampoDDD1.Text = RS.GetString(RS.GetOrdinal("TEL1_DDD"))
        CampoTel1.Text = RS.GetString(RS.GetOrdinal("TEL1_NUM"))
        CampoDDD2.Text = RS.GetString(RS.GetOrdinal("TEL2_DDD"))
        CampoTel2.Text = RS.GetString(RS.GetOrdinal("TEL2_NUM"))
        CampoEmail.Text = RS.GetString(RS.GetOrdinal("TX_EMAIL"))

        CampoEmailcoord.Text = RS.GetString(RS.GetOrdinal("TX_EMAILcoord"))

        If RS.GetString(RS.GetOrdinal("NO_POLO_ENTREGA")) <> "" Then
            cmbPoloEntrega.Text = RS.GetString(RS.GetOrdinal("NO_POLO_ENTREGA"))
        End If

        CampoOBS.Text = RS.GetString(RS.GetOrdinal("TX_OBS"))
        CampoNU_PESO_MATERIAL_ADM.Text = RS.GetValue(RS.GetOrdinal("NU_PESO_MATERIAL_ADM"))
        CampoNU_QTALUNO.Text = RS.GetValue(RS.GetOrdinal("NU_QTALUNO"))
        CampoQT_TURMAS.Text = RS.GetValue(RS.GetOrdinal("QT_TURMAS"))
        CampoQT_ESCOLAS.Text = RS.GetValue(RS.GetOrdinal("QT_ESCOLAS"))
        CampoNU_TOT_CAIXA_ADM.Text = RS.GetValue(RS.GetOrdinal("NU_TOT_CAIXA_ADM"))

        cmbInstalacao.Text = RS.GetString(RS.GetOrdinal("NATUREZA_LOCAL"))
        cmbNegociacao.Text = RS.GetString(RS.GetOrdinal("STATUS_NEGOCIACAO"))

        LabelIncAlt.Text = "Última atualização " & Format(RS.GetValue(RS.GetOrdinal("AltDate")), "dd/MM/yyyy") &
                           "    " &
                           "Incluído em " & Format(RS.GetValue(RS.GetOrdinal("IncDate")), "dd/MM/yyyy")

        If Operacao = "V" Or (RS.GetValue(RS.GetOrdinal("ESTADO_FECHADO")) = "S" And Parametros(gcParametroFuncao) <> "Administrador") Then
            cmbInstalacao.Enabled = False
            cmbMunicipio.Enabled = False
            cmbNegociacao.Enabled = False
            cmbPoloEntrega.Enabled = False
            cmbUF.Enabled = False
            CampoBairro.Enabled = False
            CampoCEP.Enabled = False
            CampoComplemento.Enabled = False
            CampoCPF.Enabled = False
            CampoCPFal.Enabled = False
            CampoCPFcel.Enabled = False
            CampoDDD1.Enabled = False
            CampoDDD2.Enabled = False
            CampoEmail.Enabled = False
            CampoID_POLO.Enabled = False
            CampoLogradouro.Enabled = False
            CampoNO_MUNICIPIO.Enabled = False
            CampoNO_POLO.Enabled = False
            CampoNumero.Enabled = False
            CampoOBS.Enabled = False
            CampoTel1.Enabled = False
            CampoTel2.Enabled = False
            cmdEntrar.Text = "OK"
            cmdCancelar.Visible = False
        End If

        If Operacao = "U" And (RS.GetValue(RS.GetOrdinal("ESTADO_FECHADO")) = "S" And Parametros(gcParametroFuncao) <> "Administrador") Then
            MensagemERRO.Text = "A edição de dados de polos está encerrada."
        End If

        ' Bloqueia a edição de acordo com as credenciais
        If (Parametros(gcParametroFuncao) <> "Coordenador Estadual" And Parametros(gcParametroFuncao) <> "Administrador") Or Operacao = "V" Then
            cmbCoordenador.Enabled = False
            cmbSubCoordenadorEst.Enabled = False
            cmbApoioLogistico.Enabled = False
        End If

        '' Bloqueia quando o estado estiver "fechado"
        'If Parametros(gcParametroFuncao) <> "Administrador" And RS.GetValue(RS.GetOrdinal("ESTADO_FECHADO")) ='S' Then 
        '    cmbCoordenador.Enabled = False Then
        '    'cmbSubCoordenadorEst.Enabled = False
        '    'cmbApoioLogistico.Enabled = False
        '    cmbInstalacao.Enabled = False
        '    cmbMunicipio.Enabled = False
        '    cmbNegociacao.Enabled = False
        '    cmbPoloEntrega.Enabled = False
        '    cmbUF.Enabled = False
        '    CampoBairro.Enabled = False
        '    CampoCEP.Enabled = False
        '    CampoComplemento.Enabled = False
        '    CampoCPF.Enabled = False
        '    CampoCPFal.Enabled = False
        '    CampoCPFcel.Enabled = False
        '    'CampoDDD1.Enabled = False
        '    'CampoDDD2.Enabled = False
        '    'CampoEmail.Enabled = False
        '    CampoID_POLO.Enabled = False
        '    CampoLogradouro.Enabled = False
        '    CampoNO_MUNICIPIO.Enabled = False
        '    CampoNO_POLO.Enabled = False
        '    CampoNumero.Enabled = False
        '    'CampoOBS.Enabled = False
        '    'CampoTel1.Enabled = False
        '    'CampoTel2.Enabled = False
        '    'cmdEntrar.Text = "OK"
        '    'cmdCancelar.Visible = False
        'End If

    End Sub
    Protected Sub PreencheCoordenadores(ByRef QueUF As String, ByRef QueMunicipio As String,
                                        ByRef QueCPF As String, ByRef QueCombo As DropDownList)

        ' Pega todos que não tÊm uma função exclusiva, mora no município quando preenchido, ou o próprio que já tem a função
        Dim sSQL As String =
           "SELECT p.NO_PESSOA + ' - ' + p.CPF  X" &
          " FROM PESSOAL p WHERE (p.NO_CATEGORIA='Gestor' and SG_UF_ALOC='" & QueUF & "'" &
                                " And NOT EXISTS (SELECT * FROM ATRIBUICAO_GESTOR a" &
                                                " WHERE a.NO_FUNCAO <> 'Subcoordenador Estadual' And p.CPF=a.CPF)" &
                                " And (p.NO_MUNICIPIO='" & QueMunicipio & "' Or '" & QueMunicipio & "' = '')" &
                                ") Or p.CPF='" & QueCPF & "'" &
          " ORDER BY 1"
        Call PreencheDropDownList(QueCombo, sSQL, "Nenhum informado...")

        'Call PreencheDropDownList(cmbApoioLogistico, sSQL, "Nenhum informado...")

        '' Pega todos que não têm uma função exclusiva ou o próprio que já tem a função
        'sSQL =
        '   "SELECT p.NO_PESSOA + ' - ' + p.CPF  X" &
        '  " FROM PESSOAL p WHERE (p.NO_CATEGORIA='Gestor' and SG_UF_ALOC='" & QueUF & "'" &
        '                        " And NOT EXISTS (SELECT * FROM ATRIBUICAO_GESTOR a" &
        '                                        " WHERE a.NO_FUNCAO <> 'Subcoordenador Estadual' And p.CPF=a.CPF)" &
        '                        ") Or p.CPF='" & QueCPF & "'" &
        '  " ORDER BY 1"
        'Call PreencheDropDownList(cmbSubCoordenadorEst, sSQL, "Nenhum informado...")
    End Sub

    'Protected Sub PreencheMunicipiosDeUmEstado(ByRef QueUF As String)
    '    Dim sSQL As String = "SELECT NO_MUNICIPIO X FROM MUNICIPIO m" &
    '                                  " WHERE m.SG_UF='" & QueUF & "'" &
    '                                  " ORDER BY case when m.Populacao > 100000 then 1 else 2 end,m.NO_MUNICIPIO"

    '    ' A procedure carrega um atributo genérico X no DropDown
    '    Call PreencheDropDownList(cmbMunicipio, sSQL, "Escolha...")
    '    cmbMunicipio.Enabled = True
    'End Sub

    Protected Sub cmbUF_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbUF.SelectedIndexChanged, cmbCoordenador.SelectedIndexChanged
        Call PreencheMunicipiosDeUmEstado(cmbMunicipio, cmbUF.Text, "Escolha...")
        Call CarregaPolosDeUmEstado(cmbPoloEntrega, Parametros(gcParametroUFbase), "Escolha...")
    End Sub


    Protected Sub cmbMunicipio_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbMunicipio.SelectedIndexChanged
        CampoNO_MUNICIPIO.Text = cmbMunicipio.Text
        Call PreencheCoordenadores(Parametros(gcParametroUFbase),
                                   CampoNO_MUNICIPIO.Text,
                                   "",
                                   cmbCoordenador)
        Call PreencheCoordenadores(Parametros(gcParametroUFbase),
                                   CampoNO_MUNICIPIO.Text,
                                   "",
                                   cmbApoioLogistico)
    End Sub

End Class