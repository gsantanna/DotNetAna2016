Public Class OrcamentoPolo
    Inherits System.Web.UI.Page

    Dim Parametros As String()
    Dim Operacao As Char
    Dim QuePolo As String
    Dim mvPaginaAtrasada As Boolean


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Se houver um erros na submissão da página (query string inválida ou  página atrasada), impede eventos posteriores
        mvPaginaAtrasada = False

        ' Verifica se a query string está OK e se as credenciais são atendidas
        Dim tmp As String = ConsisteQueryString("OrcamentoPolo", Request, CompletaTamanhoMascara("SSSS"), Parametros, IsPostBack, True)
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
            Call CarregaDados(QuePolo)
        End If

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

        If CampoViaFluvial.Text = "" Then
            erros &= "Campo via fluvial não foi preenchido. "
        ElseIf Not IsNumeric(CampoViaFluvial.Text) Then
            erros &= "Campo via fluvial deve conter apenas números. "
        End If
        If CampoViaTerrestre.Text = "" Then
            erros &= "Campo via terrestre não foi preenchido. "
        ElseIf Not IsNumeric(CampoViaTerrestre.Text) Then
            erros &= "Campo via terrestre deve conter apenas números. "
        End If
        If CampoOutrasVias.Text = "" Then
            erros &= "Campo outras vias não foi preenchido. "
        ElseIf Not IsNumeric(CampoOutrasVias.Text) Then
            erros &= "Campo via outras vias deve conter apenas números. "
        End If
        If CampoHospedagem.Text = "" Then
            erros &= "Campo hospedagem não foi preenchido. "
        ElseIf Not IsNumeric(CampoHospedagem.Text) Then
            erros &= "Campo hospedagem deve conter apenas números. "
        End If
        If CampoAlimentacao.Text = "" Then
            erros &= "Campo alimentação não foi preenchido. "
        ElseIf Not IsNumeric(CampoAlimentacao.Text) Then
            erros &= "Campo alimentação deve conter apenas números. "
        End If
        If CampoOutrasDespesas.Text = "" Then
            erros &= "Campo outras despesas não foi preenchido. "
        ElseIf Not IsNumeric(CampoOutrasDespesas.Text) Then
            erros &= "Campo outras despesas deve conter apenas números. "
        End If

        If CampoMunicipioIdaDe.Text = "" And CampoMunicipioIdaPara.Text <> "" Then erros &= "Trecho de ida incompleto. "
        If CampoMunicipioIdaDe.Text <> "" And CampoMunicipioIdaPara.Text = "" Then erros &= "Trecho de ida incompleto. "
        If CampoMunicipioIdaDe.Text <> "" _
           And CampoMunicipioIdaDe.Text = CampoMunicipioIdaPara.Text Then erros &= "Trecho de ida inválido. "

        If CampoMunicipioVoltaDe.Text = "" And CampoMunicipioVoltaPara.Text <> "" Then erros &= "Trecho de ida incompleto. "
        If CampoMunicipioVoltaDe.Text <> "" And CampoMunicipioVoltaPara.Text = "" Then erros &= "Trecho de ida incompleto. "
        If CampoMunicipioVoltaDe.Text <> "" _
           And CampoMunicipioVoltaDe.Text = CampoMunicipioVoltaPara.Text Then erros &= "Trecho de ida inválido. "

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
                "NU_VALOR_VIA_FLUVIAL=@CampoViaFluvial," &
                "NU_VALOR_VIA_TERRESTRE=@CampoViaTerrestre," &
                "NU_VALOR_VIA_OUTRAS=@CampoOutrasVias," &
                "NU_VALOR_HOSPEDAGEM=@CampoHospedagem," &
                "NU_VALOR_ALIMENTACAO=@CampoAlimentacao," &
                "NU_VALOR_OUTRASDESPESAS=@CampoOutrasDespesas," &
                "NO_MUNICIPIO_IdaDe=@MunicipioIdaDe," &
                "NO_MUNICIPIO_IdaPara=@MunicipioIdaPara," &
                "NO_MUNICIPIO_Volta_De=@MunicipioVoltaDe," &
                "NO_MUNICIPIO_Volta_Para=@MunicipioVoltaPara," &
                "TX_DISCRIMININACAO_TRANSPORTE=@TX_DISCRIMININACAO_TRANSPORTE," &
                "TX_DISCRIMININACAO_OUTRASDESPESAS=@TX_DISCRIMININACAO_OUTRASDESPESAS," &
                "TX_HORARIO_IDA=@TX_HORARIO_IDA," &
                "TX_HORARIO_VOLTA=@TX_HORARIO_VOLTA," &
                "TX_OBS=@TX_OBS," &
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
            .Add("@CampoViaFluvial", SqlDbType.Money).Value = CDec(CampoViaFluvial.Text)
            .Add("@CampoViaTerrestre", SqlDbType.Money).Value = CDec(CampoViaTerrestre.Text)
            .Add("@CampoOutrasVias", SqlDbType.Money).Value = CDec(CampoOutrasVias.Text)
            .Add("@CampoHospedagem", SqlDbType.Money).Value = CDec(CampoHospedagem.Text)
            .Add("@CampoAlimentacao", SqlDbType.Money).Value = CDec(CampoAlimentacao.Text)
            .Add("@CampoOutrasDespesas", SqlDbType.Money).Value = CDec(CampoOutrasDespesas.Text)
            .Add("@TX_DISCRIMININACAO_TRANSPORTE", SqlDbType.VarChar).Value = CampoDiscriminacaoTransporte.Text
            .Add("@TX_DISCRIMININACAO_OUTRASDESPESAS", SqlDbType.VarChar).Value = CampoDiscriminacaoDespeas.Text

            If Left(cmbHorarioIda.Text, 7) <> "Escolha" Then
                .Add("@TX_HORARIO_IDA", SqlDbType.VarChar).Value = cmbHorarioIda.Text
            Else
                .Add("@TX_HORARIO_IDA", SqlDbType.VarChar).Value = ""
            End If

            If Left(cmbHorarioVolta.Text, 7) <> "Escolha" Then
                .Add("@TX_HORARIO_VOLTA", SqlDbType.VarChar).Value = cmbHorarioVolta.Text
            Else
                .Add("@TX_HORARIO_VOLTA", SqlDbType.VarChar).Value = ""
            End If

            .Add("@MunicipioIdaDe", SqlDbType.VarChar).Value = UCase(CampoMunicipioIdaDe.Text)
            .Add("@MunicipioIdaPara", SqlDbType.VarChar).Value = UCase(CampoMunicipioIdaPara.Text)
            .Add("@MunicipioVoltaDe", SqlDbType.VarChar).Value = UCase(CampoMunicipioVoltaDe.Text)
            .Add("@MunicipioVoltaPara", SqlDbType.VarChar).Value = UCase(CampoMunicipioVoltaPara.Text)
            .Add("@TX_OBS", SqlDbType.VarChar).Value = CampoOBS.Text
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
        CampoViaFluvial.Text = Format(RS.GetValue(RS.GetOrdinal("NU_VALOR_VIA_FLUVIAL")), "######0.00")
        CampoViaTerrestre.Text = Format(RS.GetValue(RS.GetOrdinal("NU_VALOR_VIA_TERRESTRE")), "######0.00")
        CampoOutrasVias.Text = Format(RS.GetValue(RS.GetOrdinal("NU_VALOR_VIA_OUTRAS")), "######0.00")
        CampoHospedagem.Text = Format(RS.GetValue(RS.GetOrdinal("NU_VALOR_HOSPEDAGEM")), "######0.00")
        CampoAlimentacao.Text = Format(RS.GetValue(RS.GetOrdinal("NU_VALOR_ALIMENTACAO")), "######0.00")
        CampoOutrasDespesas.Text = Format(RS.GetValue(RS.GetOrdinal("NU_VALOR_OUTRASDESPESAS")), "######0.00")

        CampoDiscriminacaoDespeas.Text = RS.GetString(RS.GetOrdinal("TX_DISCRIMININACAO_OUTRASDESPESAS"))
        CampoDiscriminacaoTransporte.Text = RS.GetString(RS.GetOrdinal("TX_DISCRIMININACAO_TRANSPORTE"))

        CampoMunicipioIdaDe.Text = RS.GetString(RS.GetOrdinal("NO_MUNICIPIO_IdaDe"))
        CampoMunicipioIdaPara.Text = RS.GetString(RS.GetOrdinal("NO_MUNICIPIO_IdaPara"))
        CampoMunicipioVoltaDe.Text = RS.GetString(RS.GetOrdinal("NO_MUNICIPIO_Volta_De"))
        CampoMunicipioVoltaPara.Text = RS.GetString(RS.GetOrdinal("NO_MUNICIPIO_Volta_Para"))

        If RS.GetString(RS.GetOrdinal("TX_HORARIO_IDA")) <> "" Then cmbHorarioIda.Text = RS.GetString(RS.GetOrdinal("TX_HORARIO_IDA"))
        If RS.GetString(RS.GetOrdinal("TX_HORARIO_VOLTA")) <> "" Then cmbHorarioVolta.Text = RS.GetString(RS.GetOrdinal("TX_HORARIO_VOLTA"))
        CampoOBS.Text = RS.GetString(RS.GetOrdinal("TX_OBS"))

        If Operacao = "V" Then
            CampoID_POLO.Enabled = False
            CampoNO_POLO.Enabled = False
            CampoViaFluvial.Enabled = False
            CampoViaTerrestre.Enabled = False
            CampoOutrasVias.Enabled = False
            CampoHospedagem.Enabled = False
            CampoAlimentacao.Enabled = False
            CampoOutrasDespesas.Enabled = False
            CampoDiscriminacaoTransporte.Enabled = False
            CampoDiscriminacaoDespeas.Enabled = False

            CampoMunicipioIdaDe.Enabled = False
            CampoMunicipioIdaPara.Enabled = False
            CampoMunicipioVoltaDe.Enabled = False
            CampoMunicipioVoltaPara.Enabled = False
            cmbHorarioIda.Enabled = False
            cmbHorarioVolta.Enabled = False
            CampoOBS.Enabled = False
        End If
    End Sub

End Class