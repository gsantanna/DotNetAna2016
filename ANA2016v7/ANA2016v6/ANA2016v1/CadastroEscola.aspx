<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="CadastroEscola.aspx.vb" Inherits="ANA2016v1.CadastroEscola" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
.rcorners2 {
    border-radius: 5px;
    border: 1px solid #73AD21;
    padding: 15px;
    width: 480px;
    height: 560px;
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
            font-family: Arial;
            font-size: small;
        }
    </style>
        
    <div class="w3-main"   style="margin-left:10px;margin-top:10px; width: 1049px;">

        <div>
            <asp:Literal ID="MensagemERRO" runat="server" />
            <h4>Escola <asp:Literal ID="CampoEscola" runat="server"></asp:Literal>&nbsp;<asp:TextBox ID="CampoID_ESCOLA" runat="server" CssClass="textEntry" MaxLength="4" Width="113px" style="font-family: Arial; font-size: large" BackColor="#80FFFF" BorderStyle="None" Enabled="False"></asp:TextBox>
            </h4>
            <table style="width: 1000px;">
                <tr>
                    <td >
                    <div class="rcorners2">
                    <asp:Label ID="Label11" runat="server" Width="450px" style="font-family: Arial; " BackColor="LightGray">Dados de identificação e localização</asp:Label>
                        <br />
                    <asp:Label class="meulabel" ID="PasswordLabel" runat="server" Width="282px" >Nome da escola</asp:Label>
                        <br />
                    <asp:TextBox class="meucampo" ID="CampoNO_ESCOLA" runat="server"  style="width: 450px" MaxLength="100"  Wrap="False" BackColor="#80FFFF" BorderStyle="None" Enabled="False" CssClass="auto-style1"></asp:TextBox>
                        <br />
                    <asp:Label ID="PasswordLabel24" class="meulabel"  runat="server" Width="85px">Tipo de rede</asp:Label>
                        <asp:DropDownList ID="cmbTipoRede" runat="server" Width="132px" BackColor="#80FFFF" Enabled="False" CssClass="auto-style1">
                            <asp:ListItem>Escolha...</asp:ListItem>
                            <asp:ListItem>MUNICIPAL</asp:ListItem>
                            <asp:ListItem>ESTADUAL</asp:ListItem>
                            <asp:ListItem>FEDERAL</asp:ListItem>
                            <asp:ListItem>PRIVADA</asp:ListItem>
                        </asp:DropDownList>
                        <br />
                    <asp:Label ID="PasswordLabel25" class="meulabel" runat="server" Width="85px">Localização</asp:Label>
                        <asp:DropDownList ID="cmbLocalizacao" runat="server" Width="132px" BackColor="#80FFFF" Enabled="False" CssClass="auto-style1">
                            <asp:ListItem>Escolha...</asp:ListItem>
                            <asp:ListItem>URBANA</asp:ListItem>
                            <asp:ListItem>RURAL</asp:ListItem>
                            <asp:ListItem></asp:ListItem>
                        </asp:DropDownList>
                        <br />
                    <asp:Label ID="PasswordLabel32" class="meulabel" runat="server" Width="85px">Ambiente</asp:Label>
                        <asp:DropDownList ID="cmbCapital" runat="server" Width="132px" BackColor="#80FFFF" Enabled="False" CssClass="auto-style1">
                            <asp:ListItem>Escolha...</asp:ListItem>
                            <asp:ListItem>CAPITAL</asp:ListItem>
                            <asp:ListItem>INTERIOR</asp:ListItem>
                        </asp:DropDownList>
                        <br />
                        <br />
                    <asp:Label ID="Label20" runat="server" Width="450px" BackColor="LightGray" CssClass="auto-style1">Endereço</asp:Label>
                        <br />
                        <asp:Label ID="PasswordLabel18" class="meulabel" runat="server" AssociatedControlID="CampoCEP" Width="85px">CEP</asp:Label>
                    <asp:TextBox ID="CampoCEP" class="meulabel"  runat="server" CssClass="textEntry" MaxLength="8" Width="80px" BackColor="#80FFFF" BorderStyle="None" Enabled="False" style="font-size: small"></asp:TextBox>
                        <br />
                    <asp:Label ID="PasswordLabel1" class="meulabel" runat="server" AssociatedControlID="CampoLogradouro" Width="85px">Logradouro</asp:Label>
                    <asp:TextBox ID="CampoLogradouro" class="meulabel" runat="server" CssClass="textEntry" MaxLength="100" Width="365px" BackColor="#80FFFF" BorderStyle="None" Enabled="False" style="font-size: small"></asp:TextBox>
                        <br />
                    <asp:Label ID="PasswordLabel3" class="meulabel" runat="server" AssociatedControlID="CampoNumero" Width="85px">Número</asp:Label>
                    <asp:TextBox ID="CampoNumero" runat="server" CssClass="textEntry" MaxLength="50" Width="250px" style="font-family: Arial; font-size: small" BackColor="#80FFFF" BorderStyle="None" Enabled="False"></asp:TextBox>
                        <br />
                    <asp:Label ID="PasswordLabel5" class="meulabel" runat="server" AssociatedControlID="CampoComplemento" Width="85px">Complemento</asp:Label>
                    <asp:TextBox ID="CampoComplemento" runat="server" CssClass="textEntry" MaxLength="100" Width="365px" BackColor="#80FFFF" BorderStyle="None" Enabled="False" style="font-size: small"></asp:TextBox>
                        <br />
                    <asp:Label ID="PasswordLabel7" class="meulabel" runat="server" AssociatedControlID="CampoBairro" Width="85px">Bairro</asp:Label>
                    <asp:TextBox ID="CampoBairro" runat="server" CssClass="textEntry" MaxLength="50" Width="250px" BackColor="#80FFFF" BorderStyle="None" Enabled="False" style="font-size: small"></asp:TextBox>
                        <br />
                    <asp:Label ID="UserNameLabel5" class="meulabel" runat="server" AssociatedControlID="cmbUF" Width="100px">UF sede</asp:Label>
                    <asp:DropDownList ID="cmbUF" runat="server" Width="100px" OnSelectedIndexChanged="cmbUF_SelectedIndexChanged" AutoPostBack="True" BackColor="#80FFFF" Enabled="False" CssClass="auto-style1">
                    </asp:DropDownList>
                    <asp:TextBox ID="CampoNO_MUNICIPIO" runat="server" CssClass="textEntry" MaxLength="9" Width="20px" Visible="False"></asp:TextBox>
                        <br />
                    <asp:Label ID="UserNameLabel4" class="meulabel" runat="server" AssociatedControlID="cmbMunicipio" Width="100px">Município sede</asp:Label>
                    <asp:DropDownList ID="cmbMunicipio" runat="server" Width="340px" AutoPostBack="True" BackColor="#80FFFF" Enabled="False" CssClass="auto-style1">
                    </asp:DropDownList>
                        <br />
                        <br />
                    <asp:Label ID="Label13" runat="server" Width="450px" style="font-family: Arial; " BackColor="LightGray">Telefones &amp; contatos</asp:Label>
                        <br />
                    <asp:Label ID="UserNameLabel7" class="meulabel" runat="server" AssociatedControlID="CampoDDD1" Width="85px">Telefone</asp:Label>
                        <asp:TextBox ID="CampoDDD1" runat="server" CssClass="textEntry" MaxLength="3" Width="30px" BorderStyle="None"></asp:TextBox>
                    <asp:TextBox ID="CampoTel1" runat="server" CssClass="textEntry" MaxLength="9" Width="114px" BackColor="#80FFFF" BorderStyle="None" Enabled="False" style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
                    <asp:Label ID="UserNameLabel8" class="meulabel" runat="server" AssociatedControlID="CampoDDD2" Width="85px">Telefone público</asp:Label>
                        <asp:TextBox ID="CampoDDD2" runat="server" CssClass="textEntry" MaxLength="3" Width="30px" BackColor="White" BorderStyle="None"></asp:TextBox>
                    <asp:TextBox ID="CampoTel2" runat="server" CssClass="textEntry" MaxLength="9" Width="114px" BackColor="#80FFFF" BorderStyle="None" Enabled="False" style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
                    <asp:Label ID="UserNameLabel9" class="meulabel" runat="server" AssociatedControlID="CampoDDD2" Width="85px">Telefone contato</asp:Label>
                        <asp:TextBox ID="CampoDDD3" runat="server" CssClass="textEntry" MaxLength="3" Width="30px" BackColor="White" BorderStyle="None"></asp:TextBox>
                    <asp:TextBox ID="CampoTel3" runat="server" CssClass="textEntry" MaxLength="9" Width="114px" BackColor="#80FFFF" BorderStyle="None" Enabled="False" style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
                    <asp:Label ID="UserNameLabel10" class="meulabel" runat="server" AssociatedControlID="CampoDDD2" Width="85px">FAX</asp:Label>
                        <asp:TextBox ID="CampoDDD4" runat="server" CssClass="textEntry" MaxLength="3" Width="30px" BackColor="White" BorderStyle="None"></asp:TextBox>
                    <asp:TextBox ID="CampoTel4" runat="server" CssClass="textEntry" MaxLength="9" Width="114px" BackColor="#80FFFF" BorderStyle="None" Enabled="False" style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
                    <asp:Label ID="PasswordLabel33" class="meulabel" runat="server" AssociatedControlID="CampoBairro" Width="85px">E-mail</asp:Label>
                    <asp:TextBox ID="CampoEmail" runat="server" CssClass="textEntry" MaxLength="50" Width="355px" BackColor="#80FFFF" BorderStyle="None" Enabled="False" style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
                        <br />
&nbsp;&nbsp;&nbsp;&nbsp;
                        <br />
&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="LabelIncAlt" class="meulabel" runat="server" Width="370px"  BackColor="White" Enabled="False" CssClass="meulabel" ForeColor="#1C2F67"></asp:Label>
                    </div>
                    </td>
                    <td style="height: 10px; width: 15px;">

                    </td>
                    <td>
                    <div class="rcorners2">
                    <asp:Label ID="Label19" runat="server" Width="450px" style="font-family: Arial; " BackColor="LightGray">Polo e coordenadores</asp:Label>
                        <br />
                    <asp:Label ID="PasswordLabel26" runat="server" class="meulabel" Width="130px">Polo</asp:Label>
                        <br />
                        <asp:DropDownList ID="cmbPolo" runat="server" Width="450px" CssClass="auto-style1">
                        </asp:DropDownList>
                        <br />
                        <br />
                    <asp:Label ID="PasswordLabel29" class="meulabel" runat="server" Width="130px" >Coordenador de Polo</asp:Label>
                        <br />
                    <asp:TextBox ID="LabelNO_COORD" runat="server" CssClass="textEntry" MaxLength="100" Width="450px" BackColor="#80FFFF" BorderStyle="None" Enabled="False" style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
                    <asp:TextBox ID="LabelTX_COORD" runat="server" CssClass="textEntry" MaxLength="100" Width="450px" BackColor="#80FFFF" BorderStyle="None" Enabled="False" style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
                    <asp:Label ID="PasswordLabel30" class="meulabel" runat="server" Width="130px">Apoio Logístico</asp:Label>
                        <br />
                    <asp:TextBox ID="LabelNO_APOIO" runat="server" CssClass="textEntry" MaxLength="100" Width="450px" BackColor="#80FFFF" BorderStyle="None" Enabled="False" style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
                    <asp:TextBox ID="LabelTX_APOIO" runat="server" CssClass="textEntry" MaxLength="100" Width="450px" BackColor="#80FFFF" BorderStyle="None" Enabled="False" style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
                    <asp:Label ID="PasswordLabel31" class="meulabel" runat="server" Width="200px">Vinculada ao subcoordenador</asp:Label>
                        <br />
                    <asp:TextBox ID="LabelNO_SUBCOORD" runat="server" CssClass="textEntry" MaxLength="100" Width="450px" BackColor="#80FFFF" BorderStyle="None" Enabled="False" style="font-family: Arial; font-size: small"></asp:TextBox>
                    <asp:TextBox ID="LabelTX_SUBCOORD" runat="server" CssClass="textEntry" MaxLength="100" Width="450px" BackColor="#80FFFF" BorderStyle="None" Enabled="False" style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
                        <br />
                        <asp:Label ID="Label6" class="meulabel" runat="server" Width="96px">Total de malotes</asp:Label>
                    <asp:TextBox ID="CampoNU_TOTMALOTEESCOLA" runat="server" CssClass="textEntry" MaxLength="16" Width="70px" BackColor="Aqua" BorderStyle="None" Enabled="False" style="text-align:right; font-family: Arial; font-size: small;"></asp:TextBox>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label16" class="meulabel" runat="server" Width="70px">Turmas</asp:Label>
                    <asp:TextBox ID="CampoNU_TURMAS" runat="server" CssClass="textEntry" MaxLength="16" Width="70px" BackColor="Aqua" BorderStyle="None" Enabled="False" style="text-align:right; font-family: Arial; font-size: small;"></asp:TextBox>
                        <br />
                        <asp:Label ID="Label15" class="meulabel" runat="server" Width="96px">Alunos</asp:Label>
                    <asp:TextBox ID="CampoNU_QTALUNO" runat="server" CssClass="textEntry" MaxLength="16" Width="70px" BackColor="Aqua" BorderStyle="None" Enabled="False" style="text-align:right; font-family: Arial; font-size: small;"></asp:TextBox>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label17" class="meulabel" runat="server" Width="70px">Aplicadores</asp:Label>
                    <asp:TextBox ID="CampoNU_PESO_MATERIAL_ADM" runat="server" CssClass="textEntry" MaxLength="16" Width="70px" BackColor="Aqua" BorderStyle="None" Enabled="False" style="text-align:right; font-family: Arial; font-size: small;"></asp:TextBox>
                        <br />
                        <br />
                        <asp:Label ID="Label18" class="meulabel" runat="server" Width="150px">Observações</asp:Label>
                    <asp:TextBox ID="CampoOBS" runat="server" CssClass="textEntry" MaxLength="512" Width="450px" Height="89px" TextMode="MultiLine" ToolTip="Use este espaço para anotações livres." style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
                        <br />
                        <asp:Label ID="Label21" runat="server" Width="150px" style="font-family: Arial; font-size: small"></asp:Label>
                        <asp:Button ID="cmdEntrar" runat="server" BackColor="#16B665" BorderStyle="Double" Text="Confirmar e salvar" OnClick="cmdEntrar_Click" Width="156px" ForeColor="White" Height="31px" UseSubmitBehavior="False" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="cmdCancelar" runat="server" BackColor="White" BorderStyle="Double" Text="Cancelar" OnClick="cmdCancelar_Click" Width="94px" ForeColor="Black" UseSubmitBehavior="False" />
                        &nbsp;
                        <br />
                        <br />
                    </div>
                    </td>
                </tr>
                
            </table>
            <br />
        </div>

    </div>

</asp:Content>
