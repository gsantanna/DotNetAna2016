Public Class CadastroEscola
    Inherits System.Web.UI.Page

    Dim Parametros As String()
    Dim Operacao As String
    Dim QueEscola As String
    Dim mvPaginaAtrasada As Boolean


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Se houver um erros na submissão da página (query string inválida ou  página atrasada), impede eventos posteriores
        mvPaginaAtrasada = False

        ' Verifica se a query string está OK e se as credenciais são atendidas
        Dim tmp As String = ConsisteQueryString("CadastroEscola", Request, CompletaTamanhoMascara("SSSS"), Parametros, IsPostBack, True)
        If tmp <> "OK" Then
            MensagemERRO.Text = TextoVermelho(tmp)
            mvPaginaAtrasada = True
            Exit Sub
        End If

        ' Descobre qual é a operação
        Dim QueEscola = Request.QueryString("escola")
        If QueEscola Is Nothing Then
            ' Trata-se de uma inclusão de polo porque não houve passagem de parâmetro
            Operacao = "I"
        Else
            ' Pode ser edição ou visualização
            Dim x As String() = Split(QueEscola, ",")
            If UBound(x) < 1 Then
                MensagemERRO.Text = TextoVermelho("Houve um erro de sistema: falta o parâmetro de operação. ")
                'Exit Sub
            Else
                QueEscola = x(0)
                Operacao = Trim(x(1))
            End If
        End If

        ' Inclusão ou update, é preciso carregar o combo na primeira vez
        If Not IsPostBack Then
            Call CarregaEstadosFgvNumCombo(cmbUF, "Escolha...")
            ' Se é um update, é preciso carregar dos dados do polo
            If Operacao = "U" Or Operacao = "V" Then
                Call CarregaDados(QueEscola)
            Else
                cmbPolo.Enabled = False       ' Mantém indisponível até que se escolha um estado
            End If
        End If

    End Sub




    Protected Sub cmdCancelar_click(sender As Object, e As EventArgs) Handles cmdCancelar.Click

        If mvPaginaAtrasada Then Exit Sub
        ' Marca um novo horário em LOGIN, pois isso impede que esta página volte a ser submetida
        Call RelogioUpdate(Parametros(gcParametroCPF), Parametros(gcParametroSessionID))
        Call VoltaParaQuemChamou()

        'If mvPaginaAtrasada Then Exit Sub
        'Select Case Operacao
        '    Case "I"
        '        ' A inclusão deve ter partido da minha página
        '        Response.Redirect(ResolveUrl("~/BemVindo.aspx" & MontaQueryStringFromParametros("CadastroEscola", "BemVindo", Parametros)))
        '    Case "U", "V"
        '        ' Retorna para a página de visualização das escolas
        '        Response.Redirect(ResolveUrl("~/POLOS.aspx" & MontaQueryStringFromParametros("CadastroEscola", "POLOS", Parametros)) & "&tipo=escola")
        'End Select
    End Sub

    Private Function ConsisteCampos(ByRef QueOperacao As Char) As String
        ' O parâmetro indica se é INSERT ou UPDATE

        Dim erros As String = ""
        Dim OK As Boolean = False

        'CampoID_ESCOLA.Text = Trim(CampoID_ESCOLA.Text)
        '' Código tem que estar preenchido
        'If CampoID_ESCOLA.Text = "" Then
        '    erros &= "Código da escola não foi preenchido. "
        'ElseIf Not IsNumeric(CampoID_ESCOLA.Text) Then
        '    erros &= "Código da escola deve conter apenas dígitos numéricos. "
        'End If

        'CampoNO_ESCOLA.Text = Replace(Trim(CampoNO_ESCOLA.Text), "'", "´")
        'If CampoNO_ESCOLA.Text = "" Then
        '    erros &= "Nome da escola não foi preenchido. "
        'Else
        '    If CampoNO_ESCOLA.Text.Length < 5 Then erros &= "Nome é muito curto. "
        'End If


        'If CampoCEP.Text = "" Then
        '    erros &= "CEP não foi preenchido. "
        'ElseIf Not IsNumeric(CampoCEP.Text) Then
        '    erros &= "CEP deve conter apenas números. "
        'ElseIf Len(CampoCEP.Text) <> 8 Then
        '    erros &= "CEp deve ter oito dígitos. "
        'End If

        'If CampoLogradouro.Text = "" Then
        '    erros &= "Logradouro do endereço não foi preenchido. "
        'ElseIf CampoLogradouro.Text.Length < 4 Then
        '    erros &= "Logradouro do endereço é muito curto. "
        'End If

        'If CampoNumero.Text = "" Then erros &= "Número do endereço não foi preenchido. Preencha S/N se for o caso. "

        'If CampoBairro.Text = "" Then erros &= "Bairro do endereço não foi preenchido. "


        'If Left(cmbUF.Text, 7) = "Escolha" Then erros &= "UF do polo não foi indicado. "

        'If Left(cmbMunicipio.Text, 7) = "Escolha" Then
        '    If CampoNO_MUNICIPIO.Text = "" Then
        '        erros &= "Município do endereço não foi indicado. "
        '    End If
        'End If

        '' Os dois telefones são opcionais
        'If CampoDDD1.Text <> "" And CampoTel1.Text <> "" Then
        '    erros &= ConsisteTelefone(CampoDDD1.Text, CampoTel1.Text, "1")
        'End If

        'If CampoDDD2.Text <> "" And CampoTel2.Text <> "" Then
        '    erros &= ConsisteTelefone(CampoDDD2.Text, CampoTel2.Text, "2")
        'End If

        'If CampoDDD1.Text <> "" And CampoDDD2.Text <> "" And CampoTel1.Text <> "" And CampoTel2.Text <> "" Then
        '    If CampoDDD1.Text = CampoDDD2.Text And CampoTel1.Text = CampoTel2.Text Then
        '        erros &= "Os dois telefones são iguais."
        '    End If
        'End If

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

        If Operacao = "U" Then tmp = UPDATEcadastro()

        If tmp = "OK" Then
            ' Retorna para a página de origem
            VoltaParaQuemChamou()
        Else
            MensagemERRO.Text = TextoVermelho(tmp)
        End If
    End Sub

    Protected Sub VoltaParaQuemChamou()
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
        Response.Redirect(ResolveUrl(paginaURL & MontaQueryStringFromParametros("CadastroEscola", x(0), Parametros)) & QueryStringPlus)
    End Sub

    Private Function UPDATEcadastro() As String

        Dim MyC = New System.Data.SqlClient.SqlConnection(Ligacao())
        MyC.Open()
        Dim MeuComando = New System.Data.SqlClient.SqlCommand()

        Dim OK As String = "OK"
        Dim tmp As String

        Try
            MeuComando.Connection = MyC

            'tmp =
            '    "UPDATE ESCOLA Set " &
            '    "ID_ESCOLA=@ID_ESCOLA,ID_TIPO_REDE=tr.ID_TIPO_REDE,TIPO_REDE=tr.NO_REDE,ID_LOCALIZACAO=lo.ID_LOCALIZACAO,NO_LOCALIZACAO=lo.NO_LOCALIZACAO," &
            '    "NU_CEP=@NU_CEP,DS_ENDERECO=@DS_ENDERECO,NU_ENDERECO=@NU_ENDERECO,DS_COMPLEMENTO_ENDERECO=@DS_COMPLEMENTO_ENDERECO," &
            '    "DS_BAIRRO=@DS_BAIRRO, CO_MUNICIPIO=m.CO_MUNICIPIO, NO_MUNICIPIO=@NO_MUNICIPIO," &
            '    "TEL1_DDD=@TEL1_DDD, TEL1_NUM=@TEL1_NUM, TEL2_DDD=@TEL2_DDD, TEL2_NUM= @TEL2_NUM," &
            '    "CO_UF=uf.CO_UF,SG_UF=@SG_UF,TX_OBS=@TX_OBS," &
            '    "ID_POLO=(select p.ID_POLO FROM POLO p where p.SG_UF=@SG_UF and p.NO_POLO=@NO_POLO)," &
            '    "AltDate=GETDATE()" &
            '     " FROM UF, MUNICIPIO m, TIPO_REDE tr, TIPO_LOCALIZACAO lo" &
            '        " WHERE ID_ESCOLA=@ID_ESCOLA" &
            '          " And m.NO_MUNICIPIO=@NO_MUNICIPIO And UF.SG_UF=@SG_UF And m.SG_UF=@SG_UF" &
            '          " And tr.NO_REDE=@TIPO_REDE And lo.NO_LOCALIZACAO=@NO_LOCALIZACAO"

            tmp =
                "UPDATE ESCOLA Set " &
                "TX_OBS=@TX_OBS," &
                "ID_POLO=(select p.ID_POLO FROM POLO p where p.SG_UF=@SG_UF and p.NO_POLO=@NO_POLO)," &
                "AltDate=GETDATE()" &
                " WHERE ID_ESCOLA=@ID_ESCOLA"

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
            tmp =
                "INSERT INTO ESCOLA (" &
                "ID_ESCOLA,NO_ESCOLA,ID_TIPO_REDE,TIPO_REDE,ID_LOCALIZACAO,NO_LOCALIZACAO," &
                "NU_CEP,DS_ENDERECO,NU_ENDERECO,DS_COMPLEMENTO_ENDERECO," &
                "DS_BAIRRO, CO_MUNICIPIO, NO_MUNICIPIO," &
                "TEL1_DDD, TEL1_NUM, TEL2_DDD, TEL2_NUM," &
                "CO_UF,SG_UF,ID_POLO)" &
                "Select " &
                "@ID_ESCOLA,@NO_ESCOLA,tr.ID_TIPO_REDE,tr.NO_REDE,lo.ID_LOCALIZACAO,lo.NO_LOCALIZACAO," &
                "@NU_CEP,@DS_ENDERECO,@NU_ENDERECO,@DS_COMPLEMENTO_ENDERECO," &
                "@DS_BAIRRO, @CO_MUNICIPIO, @NO_MUNICIPIO," &
                "@TEL1_DDD, @TEL1_NUM, @TEL2_DDD, @TEL2_NUM," &
                "uf.CO_UF,uf.SG_UF," &
                "(select p.ID_POLO FROM POLO p where p.SG_UF=@SG_UF and p.NO_POLO=@NO_POLO)" &
                " FROM UF, MUNICIPIO m" &
                " WHERE Not EXISTS (Select * from POLO p where p.ID_POLO=@ID_POLO)" &
                " And uf.SG_UF=@SG_UF_ENDERECO" &
                " And m.SG_UF=@SG_UF_ENDERECO And m.NO_MUNICIPIO=@NO_MUNICIPIO" &
                " And m.NO_MUNICIPIO=@NO_MUNICIPIO And UF.SG_UF=@SG_UF And m.SG_UF=@SG_UF" &
                " And tr.NO_REDE=@TIPO_REDE And lo.NO_LOCALIZACAO=@NO_LOCALIZACAO"

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
            .Add("@SG_UF", SqlDbType.VarChar).Value = cmbUF.Text
            .Add("@ID_ESCOLA", SqlDbType.VarChar).Value = CampoID_ESCOLA.Text
            .Add("@TX_OBS", SqlDbType.VarChar).Value = CampoOBS.Text
            If Left(cmbPolo.Text, 6) = "Nenhum" Then
                .Add("@NO_POLO", SqlDbType.VarChar).Value = ""
            Else
                .Add("@NO_POLO", SqlDbType.VarChar).Value = cmbPolo.Text
            End If
        End With

    End Sub

    Private Sub CarregaDados(ByRef QueID_ESCOLA As String)

        Dim OK As String = "OK"
        Try
            'Dim sSQL As String =
            '"SELECT e.*,isnull(pp0.NO_COORDENADOR,'') NO_COORDENADOR,isnull(pp0.TX_COORDENADOR,'') TX_COORDENADOR," &
            '           "isnull(pp1.NO_APOIO,'') NO_APOIO,isnull(pp1.TX_APOIO,'') TX_APOIO," &
            '           "isnull(pp2.NO_SUBC,'') NO_SUBC,isnull(pp2.TX_SUBC,'') TX_SUBC," &
            '           "isnull(p.NO_POLO,'') NO_POLO" &
            '  " FROM (SELECT * FROM ESCOLA WHERE ID_ESCOLA='" & QueID_ESCOLA & "')e" &
            '       " LEFT JOIN POLO p      On  e.ID_POLO=p.ID_POLO" &
            '       " LEFT JOIN PESSOAL pp0 On p.COORDENADOR_CPF=pp0.CPF" &
            '       " LEFT JOIN PESSOAL pp1 On p.APOIO_LOGISTICO_CPF=pp1.CPF" &
            '       " LEFT JOIN PESSOAL pp2 On p.SUBCOORDENADOR_CPF=pp2.CPF"

            Dim sSQL As String =
            "SELECT e.*,isnull(pp0.NO_COORDENADOR,'') NO_COORDENADOR,isnull(pp0.TX_COORDENADOR,'') TX_COORDENADOR," &
                       "isnull(pp1.NO_APOIO,'') NO_APOIO,isnull(pp1.TX_APOIO,'') TX_APOIO," &
                       "isnull(pp2.NO_SUBC,'') NO_SUBC,isnull(pp2.TX_SUBC,'') TX_SUBC," &
                       "isnull(p.NO_POLO,'') NO_POLO," &
                       "(SELECT count(*) FROM TURMA t where t.ID_ESCOLA='" & QueID_ESCOLA & "') QT_TURMAS," &
                       " case when (SELECT Data_Fechamento FROM UF WHERE UF.SG_UF=e.SG_UF) < GETDATE() then 'S' else 'N' end ESTADO_FECHADO" &
              " FROM (Select * FROM ESCOLA WHERE ID_ESCOLA='" & QueID_ESCOLA & "')e" &
                " LEFT JOIN POLO p      On  e.ID_POLO=p.ID_POLO" &
                " Left Join(SELECT p.NO_PESSOA NO_COORDENADOR, p.TX_EMAIL  TX_COORDENADOR,a.ID_POLO" &
                        " From PESSOAL p, ATRIBUICAO_GESTOR a  Where p.CPF = a.CPF And a.NO_FUNCAO ='Coordenador de Polo')pp0 on p.ID_POLO=pp0.ID_POLO" &
                " Left Join (Select p.NO_PESSOA NO_APOIO, p.TX_EMAIL TX_APOIO,a.ID_POLO" &
                            " From PESSOAL p, ATRIBUICAO_GESTOR a  Where p.CPF = a.CPF And a.NO_FUNCAO ='Apoio Logístico')pp1 on p.ID_POLO=pp1.ID_POLO" &
                " Left Join (Select p.NO_PESSOA NO_SUBC, p.TX_EMAIL TX_SUBC,a.ID_POLO" &
                            " From PESSOAL p, ATRIBUICAO_GESTOR a Where p.CPF = a.CPF And a.NO_FUNCAO ='Subcoordenador Estadual')pp2 on p.ID_POLO=pp2.ID_POLO"

            'Call AbreConexao()
            Dim MyC = New System.Data.SqlClient.SqlConnection(Ligacao())
            MyC.Open()

            Dim MeuComando = New System.Data.SqlClient.SqlCommand(sSQL, MyC)
            Dim RS As System.Data.SqlClient.SqlDataReader
            RS = MeuComando.ExecuteReader()

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
            OK = "Erro na leitura dos dados das escolas." & vbCrLf & ex.Message

        End Try

        If OK <> "OK" Then MensagemERRO.Text = TextoVermelho(OK)

    End Sub

    Private Sub InstanciaCampos(ByRef RS As System.Data.SqlClient.SqlDataReader)

        RS.Read()

        'Preenche os campos
        CampoID_ESCOLA.Text = RS.GetString(RS.GetOrdinal("ID_ESCOLA"))
        CampoNO_ESCOLA.Text = RS.GetString(RS.GetOrdinal("NO_ESCOLA"))
        cmbTipoRede.Text = RS.GetString(RS.GetOrdinal("TIPO_REDE"))
        cmbLocalizacao.Text = RS.GetString(RS.GetOrdinal("NO_LOCALIZACAO"))
        cmbCapital.Text = RS.GetString(RS.GetOrdinal("NO_CAPITAL"))
        LabelNO_COORD.Text = RS.GetString(RS.GetOrdinal("NO_COORDENADOR"))
        LabelTX_COORD.Text = RS.GetString(RS.GetOrdinal("TX_COORDENADOR"))
        LabelNO_APOIO.Text = RS.GetString(RS.GetOrdinal("NO_APOIO"))
        LabelTX_APOIO.Text = RS.GetString(RS.GetOrdinal("TX_APOIO"))
        LabelNO_SUBCOORD.Text = RS.GetString(RS.GetOrdinal("NO_SUBC"))
        LabelTX_SUBCOORD.Text = RS.GetString(RS.GetOrdinal("TX_SUBC"))

        CampoCEP.Text = RS.GetString(RS.GetOrdinal("NU_CEP"))
        CampoLogradouro.Text = RS.GetString(RS.GetOrdinal("DS_ENDERECO"))
        CampoNumero.Text = RS.GetString(RS.GetOrdinal("NU_ENDERECO"))
        CampoComplemento.Text = RS.GetString(RS.GetOrdinal("DS_COMPLEMENTO_ENDERECO"))
        CampoBairro.Text = RS.GetString(RS.GetOrdinal("DS_BAIRRO"))
        CampoEmail.Text = RS.GetString(RS.GetOrdinal("TX_EMAIL"))
        CampoTel1.Text = RS.GetString(RS.GetOrdinal("NU_TELEFONE"))
        CampoTel2.Text = RS.GetString(RS.GetOrdinal("NU_TELEFONE_PUBLICO"))
        CampoTel3.Text = RS.GetString(RS.GetOrdinal("NU_TELEFONE_CONTATO"))
        CampoTel4.Text = RS.GetString(RS.GetOrdinal("NU_FAX"))

        If RS.GetString(RS.GetOrdinal("SG_UF")) <> "" Then
            cmbUF.Text = RS.GetString(RS.GetOrdinal("SG_UF"))
            Call PreencheMunicipiosDeUmEstado(cmbMunicipio, cmbUF.Text, "Escolha...")
            Call CarregaPolosDeUmEstado(cmbPolo, cmbUF.Text, "Nenhum...")
            If RS.GetString(RS.GetOrdinal("NO_POLO")) <> "" Then cmbPolo.Text = RS.GetString(RS.GetOrdinal("NO_POLO"))
        End If

        If RS.GetString(RS.GetOrdinal("NO_MUNICIPIO")) <> "" Then
            cmbMunicipio.Enabled = True
            cmbMunicipio.Text = RS.GetString(RS.GetOrdinal("NO_MUNICIPIO"))
            ' Este campo é necessário porque a mudança da seleção não estava sendo reconhecida
            CampoNO_MUNICIPIO.Text = RS.GetString(RS.GetOrdinal("NO_MUNICIPIO"))
        End If

        CampoOBS.Text = RS.GetString(RS.GetOrdinal("TX_OBS"))
        CampoNU_QTALUNO.Text = RS.GetValue(RS.GetOrdinal("QT_ALUNO_ESCOLAS"))
        CampoNU_TURMAS.Text = RS.GetValue(RS.GetOrdinal("QT_TURMAS"))
        CampoNU_TOTMALOTEESCOLA.Text = RS.GetValue(RS.GetOrdinal("NU_TOTMALOTEESCOLA"))

        If Operacao = "V" Or (RS.GetValue(RS.GetOrdinal("ESTADO_FECHADO")) = "S" And Parametros(gcParametroFuncao) <> "Administrador") Then
            cmbPolo.Enabled = False
            CampoOBS.Enabled = False
            cmdEntrar.Text = "OK"
            cmdCancelar.Visible = False
        End If

        If Operacao = "U" And (RS.GetValue(RS.GetOrdinal("ESTADO_FECHADO")) = "S" And Parametros(gcParametroFuncao) <> "Administrador") Then
            MensagemERRO.Text = "A edição de dados de escolas está encerrada."
        End If

        LabelIncAlt.Text = "Editado em " & Format(RS.GetValue(RS.GetOrdinal("AltDate")), "dd/MM/yyyy") &
                           "    " &
                           " Incluído em " & Format(RS.GetValue(RS.GetOrdinal("IncDate")), "dd/MM/yyyy")
    End Sub


    Protected Sub cmbUF_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbUF.SelectedIndexChanged
        Call PreencheMunicipiosDeUmEstado(cmbMunicipio, cmbUF.Text, "Escolha...")
        CarregaPolosDeUmEstado(cmbPolo, cmbUF.Text, "Escolha...")
        cmbPolo.Enabled = True
    End Sub


    Protected Sub cmbMunicipio_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbMunicipio.SelectedIndexChanged
        CampoNO_MUNICIPIO.Text = cmbMunicipio.Text
    End Sub

End Class