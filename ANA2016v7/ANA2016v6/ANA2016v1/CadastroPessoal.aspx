<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="CadastroPessoal.aspx.vb" Inherits="ANA2016v1.CadastroPessoal" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
.rcorners2 {
    border-radius: 5px;
    border: 1px solid #73AD21;
    padding: 15px;
    width: 480px;
    height: 660px;
}
.meulabel {
    font-family: Arial;
    font-size: x-small;
}
.meucampo {
    font-family: Arial;
    font-size: x-small;
}
        .auto-style1 {
            font-size: small;
        }
        .auto-style2 {
            font-family: Arial;
            font-size: small;
        }
        .auto-style3 {
            font-size: x-small;
        }
    </style>

    <div class="w3-main"   style="margin-left:10px;margin-top:10px;">

        <div>
            <asp:Literal ID="MensagemERRO" runat="server" />
            <h4> <asp:Literal ID="Titulo" runat="server"></asp:Literal></h4>
            <table style="width: 1100px; height: 103px;">
                <tr>
                    <td>

                    <div class="rcorners2">
                    <asp:Label ID="LabelEstadoBAse" runat="server" Width="190px" style="font-family: Arial; font-size: medium">Estado onde vai trabalhar</asp:Label>
                        <strong>
                        <asp:DropDownList ID="cmbEstadoBase" runat="server" style="font-family: Arial; " Width="205px" AutoPostBack="True" CssClass="auto-style1">
                            <asp:ListItem>Escolha...</asp:ListItem>
                            <asp:ListItem>Acre</asp:ListItem>
                            <asp:ListItem>Amazonas</asp:ListItem>
                            <asp:ListItem>Bahia</asp:ListItem>
                            <asp:ListItem>Distrito Federal</asp:ListItem>
                            <asp:ListItem>Rondônia</asp:ListItem>
                            <asp:ListItem>Maranhão</asp:ListItem>
                            <asp:ListItem>Goiás</asp:ListItem>
                            <asp:ListItem>Mato Grosso do Sul</asp:ListItem>
                            <asp:ListItem>Pará</asp:ListItem>
                            <asp:ListItem>Piauí</asp:ListItem>
                            <asp:ListItem>Rio Grande do Sul</asp:ListItem>
                        </asp:DropDownList>
                    <asp:TextBox ID="CampoCPF" runat="server" CssClass="textEntry" MaxLength="10" Width="24px" ToolTip="Digite números e barras" style="font-family: Arial; font-size: small" Visible="False"></asp:TextBox>
                        </strong>
                        <br />
                    <asp:Label ID="Label11" runat="server" Width="450px" style="font-family: Arial; " BackColor="LightGray" CssClass="auto-style1">Dados pessoais</asp:Label>
                        <br />
                    <asp:Label ID="PasswordLabel" class="meulabel" runat="server" AssociatedControlID="CampoNome" Width="85px">Nome</asp:Label>
                    <asp:TextBox ID="CampoNome" runat="server" CssClass="textEntry" MaxLength="100" Width="421px" Wrap="False" style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
                    <asp:Label ID="PasswordLabel20" class="meulabel" runat="server" AssociatedControlID="CampoDiaNascimento" Width="140px">Dt Nasc (dia/mês/ano)</asp:Label>
                    <asp:TextBox ID="CampoDiaNascimento" runat="server" CssClass="textEntry" MaxLength="2" Width="38px" ToolTip="Digite números e barras" style="font-family: Arial; font-size: small"></asp:TextBox>
                        &nbsp;/
                    <asp:DropDownList ID="cmbMesNascimento" runat="server" Width="110px"  CssClass="auto-style2">
                        <asp:ListItem>Escolha...</asp:ListItem>
                        <asp:ListItem>Janeiro</asp:ListItem>
                        <asp:ListItem>Fevereiro</asp:ListItem>
                        <asp:ListItem>Março</asp:ListItem>
                        <asp:ListItem>Abril</asp:ListItem>
                        <asp:ListItem>Maio</asp:ListItem>
                        <asp:ListItem>Junho</asp:ListItem>
                        <asp:ListItem>Julho</asp:ListItem>
                        <asp:ListItem>Agosto</asp:ListItem>
                        <asp:ListItem>Setembro</asp:ListItem>
                        <asp:ListItem>Outubro</asp:ListItem>
                        <asp:ListItem>Novembro</asp:ListItem>
                        <asp:ListItem>Dezembro</asp:ListItem>
                    </asp:DropDownList>
                        &nbsp;/
                    <asp:DropDownList ID="cmbAnoNascimento" runat="server" Width="90px"  CssClass="auto-style2">
                        <asp:ListItem>Escolha...</asp:ListItem>
                        <asp:ListItem>1999</asp:ListItem>
                        <asp:ListItem>1998</asp:ListItem>
                        <asp:ListItem>1997</asp:ListItem>
                        <asp:ListItem>1996</asp:ListItem>
                        <asp:ListItem>1995</asp:ListItem>
                        <asp:ListItem>1994</asp:ListItem>
                        <asp:ListItem>1993</asp:ListItem>
                        <asp:ListItem>1992</asp:ListItem>
                        <asp:ListItem>1991</asp:ListItem>
                        <asp:ListItem>1990</asp:ListItem>
                        <asp:ListItem>1989</asp:ListItem>
                        <asp:ListItem>1988</asp:ListItem>
                        <asp:ListItem>1987</asp:ListItem>
                        <asp:ListItem>1986</asp:ListItem>
                        <asp:ListItem>1985</asp:ListItem>
                        <asp:ListItem>1984</asp:ListItem>
                        <asp:ListItem>1983</asp:ListItem>
                        <asp:ListItem>1982</asp:ListItem>
                        <asp:ListItem>1981</asp:ListItem>
                        <asp:ListItem>1980</asp:ListItem>
                        <asp:ListItem>1979</asp:ListItem>
                        <asp:ListItem>1978</asp:ListItem>
                        <asp:ListItem>1977</asp:ListItem>
                        <asp:ListItem>1976</asp:ListItem>
                        <asp:ListItem>1975</asp:ListItem>
                        <asp:ListItem>1974</asp:ListItem>
                        <asp:ListItem>1973</asp:ListItem>
                        <asp:ListItem>1972</asp:ListItem>
                        <asp:ListItem>1971</asp:ListItem>
                        <asp:ListItem>1970</asp:ListItem>
                        <asp:ListItem>1969</asp:ListItem>
                        <asp:ListItem>1968</asp:ListItem>
                        <asp:ListItem>1967</asp:ListItem>
                        <asp:ListItem>1966</asp:ListItem>
                        <asp:ListItem>1965</asp:ListItem>
                        <asp:ListItem>1964</asp:ListItem>
                        <asp:ListItem>1963</asp:ListItem>
                        <asp:ListItem>1962</asp:ListItem>
                        <asp:ListItem>1961</asp:ListItem>
                        <asp:ListItem>1960</asp:ListItem>
                        <asp:ListItem>1959</asp:ListItem>
                        <asp:ListItem>1958</asp:ListItem>
                        <asp:ListItem>1957</asp:ListItem>
                        <asp:ListItem>1956</asp:ListItem>
                        <asp:ListItem>1955</asp:ListItem>
                        <asp:ListItem>1954</asp:ListItem>
                        <asp:ListItem>1953</asp:ListItem>
                        <asp:ListItem>1952</asp:ListItem>
                        <asp:ListItem>1951</asp:ListItem>
                        <asp:ListItem>1950</asp:ListItem>
                        <asp:ListItem>1949</asp:ListItem>
                        <asp:ListItem>1948</asp:ListItem>
                        <asp:ListItem>1947</asp:ListItem>
                        <asp:ListItem>1946</asp:ListItem>
                        <asp:ListItem>1945</asp:ListItem>
                        <asp:ListItem>1944</asp:ListItem>
                        <asp:ListItem>1943</asp:ListItem>
                        <asp:ListItem>1942</asp:ListItem>
                        <asp:ListItem>1941</asp:ListItem>
                    </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <br />
                    <asp:Label ID="UserNameLabel11" class="meulabel" runat="server" Width="140px">PIS/PASEP</asp:Label>
                    <asp:TextBox ID="CampoPisPasep" runat="server" CssClass="textEntry" MaxLength="11" Width="120px" style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
                    <asp:Label ID="UserNameLabel0" class="meulabel" runat="server" AssociatedControlID="cmbGenero" Width="140px">Gênero</asp:Label>
                    <asp:DropDownList ID="cmbGenero" runat="server" Width="180px" CssClass="auto-style2">
                        <asp:ListItem>Escolha...</asp:ListItem>
                        <asp:ListItem>Feminino</asp:ListItem>
                        <asp:ListItem>Masculino</asp:ListItem>
                    </asp:DropDownList>
                        <br />
                    <asp:Label ID="UserNameLabel2" class="meulabel" runat="server" AssociatedControlID="cmbEscolaridade" Width="140px">Grau de instrução</asp:Label>
                    <asp:DropDownList ID="cmbEscolaridade" runat="server" Width="180px" CssClass="auto-style2">
                        <asp:ListItem>Escolha...</asp:ListItem>
                        <asp:ListItem>1o Grau Completo - Fundamental</asp:ListItem>
                        <asp:ListItem>2o Grau Completo - Ensino Médio</asp:ListItem>
                        <asp:ListItem>Ensino Superior Completo</asp:ListItem>
                    </asp:DropDownList>
                        <br />
                    <asp:Label ID="UserNameLabel12" class="meulabel" runat="server" AssociatedControlID="cmbEscolaridade" Width="140px">Estado civil</asp:Label>
                        <asp:DropDownList ID="cmbEstadoCivil" runat="server" Width="180px" CssClass="auto-style2">
                            <asp:ListItem>Escolha...</asp:ListItem>
                            <asp:ListItem>Solteiro</asp:ListItem>
                            <asp:ListItem>Casado</asp:ListItem>
                            <asp:ListItem>Separado</asp:ListItem>
                            <asp:ListItem>Divorciado</asp:ListItem>
                            <asp:ListItem>Viúvo</asp:ListItem>
                        </asp:DropDownList>
                        <br />
                    <asp:Label ID="Label2" class="meulabel" runat="server" AssociatedControlID="CampoNomeMae" Width="85px">Nome da mãe</asp:Label>
                    <asp:TextBox ID="CampoNomeMae" runat="server" CssClass="textEntry" MaxLength="64" Width="416px" style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
                        <br />
                    <asp:Label ID="PasswordLabel14" runat="server" Width="450px" style="font-family: Arial; " BackColor="LightGray" CssClass="auto-style1">RG</asp:Label>
                        <br />
                    <asp:Label ID="PasswordLabel21" class="meulabel" runat="server" AssociatedControlID="CampoRGnumero" Width="148px">Número do documento</asp:Label>
                        <asp:Label ID="PasswordLabel15" class="meulabel" runat="server" AssociatedControlID="CampoRGorgao" Width="152px">Órgão expedidor</asp:Label>
                        <asp:Label ID="UserNameLabel6" class="meulabel" runat="server"  Width="30px">UF</asp:Label>
                        <br />
                    <asp:TextBox ID="CampoRGnumero" runat="server" CssClass="textEntry" MaxLength="20" Width="140px" style="font-size: small"></asp:TextBox>
                    <asp:TextBox ID="CampoRGorgao" runat="server" CssClass="textEntry" MaxLength="30" Width="140px" style="font-size: small"></asp:TextBox>
                    <asp:DropDownList ID="cmbUFdoRG" runat="server" Width="80px"  CssClass="auto-style1">
                        <asp:ListItem>Escolha...</asp:ListItem>
                        <asp:ListItem>AC</asp:ListItem>
                        <asp:ListItem>AL</asp:ListItem>
                        <asp:ListItem>AM</asp:ListItem>
                        <asp:ListItem>AP</asp:ListItem>
                        <asp:ListItem>BA</asp:ListItem>
                        <asp:ListItem>CE</asp:ListItem>
                        <asp:ListItem>DF</asp:ListItem>
                        <asp:ListItem>ES</asp:ListItem>
                        <asp:ListItem>GO</asp:ListItem>
                        <asp:ListItem>MA</asp:ListItem>
                        <asp:ListItem>MG</asp:ListItem>
                        <asp:ListItem>MS</asp:ListItem>
                        <asp:ListItem>MT</asp:ListItem>
                        <asp:ListItem>PA</asp:ListItem>
                        <asp:ListItem>PB</asp:ListItem>
                        <asp:ListItem>PE</asp:ListItem>
                        <asp:ListItem>PI</asp:ListItem>
                        <asp:ListItem>PR</asp:ListItem>
                        <asp:ListItem>RJ</asp:ListItem>
                        <asp:ListItem>RN</asp:ListItem>
                        <asp:ListItem>RO</asp:ListItem>
                        <asp:ListItem>RR</asp:ListItem>
                        <asp:ListItem>RS</asp:ListItem>
                        <asp:ListItem>SC</asp:ListItem>
                        <asp:ListItem>SE</asp:ListItem>
                        <asp:ListItem>SP</asp:ListItem>
                        <asp:ListItem>TO</asp:ListItem>
                    </asp:DropDownList>
                        <br />
                        <asp:Label ID="PasswordLabel22" class="meulabel" runat="server" AssociatedControlID="CampoRGorgao" Width="199px">Data de expedição (dia/mês/ano)</asp:Label>
                        <br />
                    <asp:TextBox ID="CampoDiaExpedicao" runat="server" CssClass="textEntry" MaxLength="2" Width="36px" ToolTip="Digite números e barras" style="font-size: small; font-family: Arial"></asp:TextBox>
                    &nbsp;/
                    <asp:DropDownList ID="cmbMesExpedicao" runat="server" Width="110px"  CssClass="auto-style2">
                        <asp:ListItem>Escolha...</asp:ListItem>
                        <asp:ListItem>Janeiro</asp:ListItem>
                        <asp:ListItem>Fevereiro</asp:ListItem>
                        <asp:ListItem>Março</asp:ListItem>
                        <asp:ListItem>Abril</asp:ListItem>
                        <asp:ListItem>Maio</asp:ListItem>
                        <asp:ListItem>Junho</asp:ListItem>
                        <asp:ListItem>Julho</asp:ListItem>
                        <asp:ListItem>Agosto</asp:ListItem>
                        <asp:ListItem>Setembro</asp:ListItem>
                        <asp:ListItem>Outubro</asp:ListItem>
                        <asp:ListItem>Novembro</asp:ListItem>
                        <asp:ListItem>Dezembro</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;/
                    <asp:DropDownList ID="cmbAnoExpedicao" runat="server" Width="90px"  CssClass="auto-style2">
                        <asp:ListItem>Escolha...</asp:ListItem>
                        <asp:ListItem>2016</asp:ListItem>
                        <asp:ListItem>2015</asp:ListItem>
                        <asp:ListItem>2014</asp:ListItem>
                        <asp:ListItem>2013</asp:ListItem>
                        <asp:ListItem>2012</asp:ListItem>
                        <asp:ListItem>2011</asp:ListItem>
                        <asp:ListItem>2010</asp:ListItem>
                        <asp:ListItem>2009</asp:ListItem>
                        <asp:ListItem>2008</asp:ListItem>
                        <asp:ListItem>2007</asp:ListItem>
                        <asp:ListItem>2006</asp:ListItem>
                        <asp:ListItem>2005</asp:ListItem>
                        <asp:ListItem>2004</asp:ListItem>
                        <asp:ListItem>2003</asp:ListItem>
                        <asp:ListItem>2002</asp:ListItem>
                        <asp:ListItem>2001</asp:ListItem>
                        <asp:ListItem>2000</asp:ListItem>
                        <asp:ListItem>1999</asp:ListItem>
                        <asp:ListItem>1998</asp:ListItem>
                        <asp:ListItem>1997</asp:ListItem>
                        <asp:ListItem>1996</asp:ListItem>
                        <asp:ListItem>1995</asp:ListItem>
                        <asp:ListItem>1994</asp:ListItem>
                        <asp:ListItem>1993</asp:ListItem>
                        <asp:ListItem>1992</asp:ListItem>
                        <asp:ListItem>1991</asp:ListItem>
                        <asp:ListItem>1990</asp:ListItem>
                        <asp:ListItem>1989</asp:ListItem>
                        <asp:ListItem>1988</asp:ListItem>
                        <asp:ListItem>1987</asp:ListItem>
                        <asp:ListItem>1986</asp:ListItem>
                        <asp:ListItem>1985</asp:ListItem>
                        <asp:ListItem>1984</asp:ListItem>
                        <asp:ListItem>1983</asp:ListItem>
                        <asp:ListItem>1982</asp:ListItem>
                        <asp:ListItem>1981</asp:ListItem>
                        <asp:ListItem>1980</asp:ListItem>
                        <asp:ListItem>1979</asp:ListItem>
                        <asp:ListItem>1978</asp:ListItem>
                        <asp:ListItem>1977</asp:ListItem>
                        <asp:ListItem>1976</asp:ListItem>
                        <asp:ListItem>1975</asp:ListItem>
                        <asp:ListItem>1974</asp:ListItem>
                        <asp:ListItem>1973</asp:ListItem>
                        <asp:ListItem>1972</asp:ListItem>
                        <asp:ListItem>1971</asp:ListItem>
                        <asp:ListItem>1970</asp:ListItem>
                        <asp:ListItem>1969</asp:ListItem>
                        <asp:ListItem>1968</asp:ListItem>
                        <asp:ListItem>1967</asp:ListItem>
                        <asp:ListItem>1966</asp:ListItem>
                    </asp:DropDownList>
                        <br />
                        <br />
                    <asp:Label ID="PasswordLabel12" runat="server" Width="450px" style="font-family: Arial; " BackColor="LightGray" CssClass="auto-style1">Endereço</asp:Label>
                        <br />
                        <asp:Label ID="PasswordLabel18" class="meulabel" runat="server" AssociatedControlID="CampoCEP" Width="85px">CEP</asp:Label>
                    <asp:TextBox ID="CampoCEP" runat="server" CssClass="textEntry" MaxLength="8" Width="80px" style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
                    <asp:Label ID="PasswordLabel1" class="meulabel" runat="server" AssociatedControlID="CampoLogradouro" Width="85px">Logradouro</asp:Label>
                    <asp:TextBox ID="CampoLogradouro" runat="server" CssClass="textEntry" MaxLength="60" Width="400px" style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
                    <asp:Label ID="PasswordLabel3" class="meulabel" runat="server" AssociatedControlID="CampoNumero" Width="85px">Número</asp:Label>
                    <asp:TextBox ID="CampoNumero" runat="server" CssClass="textEntry" MaxLength="50" Width="250px" style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
                    <asp:Label ID="PasswordLabel5" class="meulabel" runat="server" AssociatedControlID="CampoComplemento" Width="85px">Complemento</asp:Label>
                    <asp:TextBox ID="CampoComplemento" runat="server" CssClass="textEntry" MaxLength="60" Width="390px" style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
                    <asp:Label ID="PasswordLabel7" class="meulabel" runat="server" AssociatedControlID="CampoBairro" Width="85px">Bairro</asp:Label>
                    <asp:TextBox ID="CampoBairro" runat="server" CssClass="textEntry" MaxLength="50" Width="250px" style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
                    <asp:Label ID="UserNameLabel5" class="meulabel" runat="server" Width="85px" Visible="False">UF</asp:Label>
                        <br />
                    <asp:Label ID="UserNameLabel4" class="meulabel" runat="server"  AssociatedControlID="cmbMunicipio" Width="85px">Município</asp:Label>
                    <asp:DropDownList ID="cmbMunicipio" runat="server" Width="42%" CssClass="auto-style2" AutoPostBack="True">
                    </asp:DropDownList>
                    <asp:TextBox ID="CampoNO_MUNICIPIO" runat="server" CssClass="textEntry" MaxLength="9" Width="62px" Visible="False"></asp:TextBox>
                        <br />
                        <br />
                    </div>
                    </td>
                    <td style="width: 10px"></td>
                    <td>
                    <div class="rcorners2">
                    <asp:Label ID="Label12" runat="server" Width="370px" style="font-family: Arial; " BackColor="LightGray" CssClass="auto-style1">Dados bancários</asp:Label>
                        <br />
                        <asp:Label ID="Label7" class="meulabel" runat="server" Width="85px">Banco</asp:Label>
                    <asp:DropDownList ID="cmbBanco" runat="server" Width="280px" >
                    </asp:DropDownList>
                        <br />
                    <asp:Label ID="Label5" class="meulabel" runat="server" AssociatedControlID="CampoAgencia" Width="210px">Código da agência + DV</asp:Label>
                    <asp:TextBox ID="CampoAgencia" runat="server" CssClass="textEntry" MaxLength="4" Width="50px" style="font-family: Arial; font-size: small" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>
                        &nbsp;-
                    <asp:TextBox ID="CampoAgenciaDV" runat="server" CssClass="textEntry" MaxLength="1" Width="30px" style="font-family: Arial; font-size: small" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>
                        <br />
                        <asp:Label ID="Label6" class="meulabel" runat="server" AssociatedControlID="campoConta" Width="160px">Número da conta corrente + DV</asp:Label>
                    <asp:TextBox ID="campoConta" runat="server" CssClass="textEntry" MaxLength="16" Width="100px" style="font-family: Arial; font-size: small" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>
                        &nbsp;-
                    <asp:TextBox ID="CampoContaDV" runat="server" CssClass="textEntry" MaxLength="1" Width="30px" style="font-family: Arial; font-size: small" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>
                        <br />
                        <asp:Label ID="Label14" class="meulabel" runat="server" AssociatedControlID="CampoConfirmaSenha" Width="160px" Visible="False">Tipo de conta</asp:Label>
                        <asp:DropDownList ID="cmbTipoConta" runat="server" Width="180px" CssClass="auto-style2" Visible="False">
                            <asp:ListItem>Escolha...</asp:ListItem>
                            <asp:ListItem>Conta corrente</asp:ListItem>
                            <asp:ListItem>Conta poupança</asp:ListItem>
                        </asp:DropDownList>
                        <br />
                    <asp:Label ID="Label13" runat="server" Width="370px" style="font-family: Arial; " BackColor="LightGray" CssClass="auto-style1">Telefones</asp:Label>
                        <br />
                    <asp:Label ID="UserNameLabel7" class="meulabel" runat="server" AssociatedControlID="CampoDDD1" Width="85px">Telefone Cel</asp:Label>
                        <asp:TextBox ID="CampoDDD1" runat="server" CssClass="textEntry" MaxLength="3" Width="30px" style="font-size: small"></asp:TextBox>
                    <asp:TextBox ID="CampoTel1" runat="server" CssClass="textEntry" MaxLength="9" Width="90px" style="font-size: small"></asp:TextBox>
                        <br />
                    <asp:Label ID="UserNameLabel8" class="meulabel" runat="server" AssociatedControlID="CampoDDD2" Width="85px">Telefone Res</asp:Label>
                        <asp:TextBox ID="CampoDDD2" runat="server" CssClass="textEntry" MaxLength="3" Width="30px" style="font-size: small"></asp:TextBox>
                    <asp:TextBox ID="CampoTel2" runat="server" CssClass="textEntry" MaxLength="9" Width="90px" style="font-size: small"></asp:TextBox>
                        <br />
                    <asp:Label ID="UserNameLabel9" class="meulabel" runat="server" AssociatedControlID="CampoDDD3" Width="85px">Telefone Com</asp:Label>
                        <asp:TextBox ID="CampoDDD3" runat="server" CssClass="textEntry" MaxLength="3" Width="30px" style="font-size: small"></asp:TextBox>
                    <asp:TextBox ID="CampoTel3" runat="server" CssClass="textEntry" MaxLength="9" Width="90px" style="font-size: small"></asp:TextBox>
                        <br />
                    <asp:Label ID="UserNameLabel1" class="meulabel" runat="server" AssociatedControlID="CampoEmail" Width="85px">E-mail</asp:Label>
                    <asp:TextBox ID="CampoEmail" runat="server" CssClass="textEntry" MaxLength="40" Width="280px" style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
                        <br />
                    <asp:Label ID="Label8" runat="server" Width="370px" style="font-family: Arial; " BackColor="LightGray" CssClass="auto-style1">Senha (incluindo pelo menos 6 caracteres)</asp:Label>
                        <br />
                    <asp:Label ID="Label9" class="meulabel" runat="server" AssociatedControlID="CampoSenha" Width="149px">Senha</asp:Label>
                    <asp:TextBox ID="CampoSenha" runat="server" CssClass="passwordEntry" MaxLength="9" Width="80px" style="font-family: Arial; font-size: small" TextMode="Password"></asp:TextBox>
                        <br />
                    <asp:Label ID="Label10" class="meulabel" runat="server" Width="149px">Confirmação da senha</asp:Label>
                    <asp:TextBox ID="CampoConfirmaSenha" runat="server" CssClass="passwordEntry" MaxLength="9" Width="80px" style="font-family: Arial; font-size: small" TextMode="Password"></asp:TextBox>
                        <br />
                        <br />
                    <asp:TextBox ID="CampoEmail0" runat="server" CssClass="textEntry" MaxLength="40" Width="367px" style="font-family: Arial; font-size: small" BackColor="White" BorderStyle="None" ForeColor="#1C2F67" Height="109px" TextMode="MultiLine" ReadOnly="True">Ao confirmar, declaro que minhas informações de cadastro aqui prestadas são fidedignas, que a senha por mim escolhida é pessoal e intransferível, e que este cadastramento não implica qualquer tipo de oferta ou contratação por parte da FGV.</asp:TextBox>
                        <br />
                        <br />
                        <asp:Button ID="cmdEntrar" runat="server" BackColor="#16B665" BorderStyle="None" Text="Confirmar e salvar" OnClick="cmdEntrar_Click" Width="156px" ForeColor="White" ToolTip="Clique aqui para validar os dados e salvar" Height="36px" UseSubmitBehavior="False" BorderWidth="0px" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="cmdCancelar" runat="server" BackColor="White" BorderStyle="Double" Text="Cancelar" OnClick="cmdCancelar_Click" Width="94px" ForeColor="Black" ToolTip="Clique para abandonar o cadastramento" UseSubmitBehavior="False" />
                        <br />
                        <br />
                        <br />
                        <br />
                    <asp:Label ID="LabelIncAlt" runat="server" Width="370px" style="font-family: Arial; " BackColor="White" CssClass="auto-style3" ForeColor="#1C2F67"></asp:Label>
                        <br />
                    </div>
                    </td>
                </tr>
                
            </table>
            <br />
        </div>

    </div>
</asp:Content>
