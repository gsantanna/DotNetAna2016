<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="CadastroPOLO.aspx.vb" Inherits="ANA2016v1.CadastroPOLO" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
.rcorners2 {
    border-radius: 5px;
    border: 1px solid #1c2f67;
    padding: 15px;
    width: 480px;
    height: 550px;
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
        .auto-style2 {
            font-size: x-small;
        }
        .auto-style3 {
            font-size: small;
        }
    </style>


    <div class="w3-main"   style="margin-left:0px;margin-top:10px;">

        <div>
            <asp:Literal ID="MensagemERRO" runat="server" />
                       <h5>Polo <asp:Literal ID="CampoPolo" runat="server"></asp:Literal>&nbsp;<asp:TextBox ID="CampoID_POLO" runat="server" CssClass="textEntry" MaxLength="4" Width="85px" style="font-family: Arial; font-size: large"></asp:TextBox>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="cmdEntrar" runat="server" BackColor="#16B665" BorderStyle="None" Text="Confirmar e salvar" OnClick="cmdEntrar_Click" Width="156px" ForeColor="White" ToolTip="Clique aqui para validar os dados e salvar" UseSubmitBehavior="False" BorderWidth="0px" Height="36px" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="cmdCancelar" runat="server" BackColor="White" BorderStyle="Double" Text="Cancelar" OnClick="cmdCancelar_Click" Width="94px" ForeColor="Black" ToolTip="Clique aqui para abandonar o cadastramento" UseSubmitBehavior="False" />
                        </h5>
            <table style="width: 1100px; height: 607px;">
                <tr>
                    <td>
                    <div class="rcorners2">

                    <asp:Label ID="Label11" runat="server" Width="450px" style="font-family: Arial; " BackColor="LightGray" CssClass="auto-style3">Dados de identificação e localização</asp:Label>
                        <br />
                    <asp:Label ID="PasswordLabel" runat="server" class="meulabel" Width="85px">Nome do Polo</asp:Label>
                    <asp:TextBox ID="CampoNO_POLO" runat="server" CssClass="textEntry" MaxLength="100" Width="421px" Wrap="False" style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
                        <br />
                        <asp:Label ID="PasswordLabel18" runat="server"  class="meulabel" Width="85px">CEP</asp:Label>
                    <asp:TextBox ID="CampoCEP" runat="server" CssClass="textEntry" MaxLength="8" Width="80px" style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
                    <asp:Label ID="PasswordLabel1" runat="server"  class="meulabel" Width="85px">Logradouro</asp:Label>
                    <asp:TextBox ID="CampoLogradouro" runat="server" CssClass="textEntry" MaxLength="100" Width="441px" style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
                    <asp:Label ID="PasswordLabel3" runat="server"  class="meulabel" Width="85px">Número</asp:Label>
                    <asp:TextBox ID="CampoNumero" runat="server" CssClass="textEntry" MaxLength="50" Width="349px" style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
                    <asp:Label ID="PasswordLabel5" runat="server"  class="meulabel" Width="85px">Complemento</asp:Label>
                    <asp:TextBox ID="CampoComplemento" runat="server" CssClass="textEntry" MaxLength="100" Width="441px" style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
                    <asp:Label ID="PasswordLabel7" runat="server"  class="meulabel" Width="85px">Bairro</asp:Label>
                    <asp:TextBox ID="CampoBairro" runat="server" CssClass="textEntry" MaxLength="50" Width="352px" style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
                    <asp:Label ID="UserNameLabel5" runat="server"  class="meulabel" Width="100px">UF sede</asp:Label>
                    <asp:DropDownList ID="cmbUF" runat="server" Width="100px" OnSelectedIndexChanged="cmbUF_SelectedIndexChanged" AutoPostBack="True" style="font-family: Arial; font-size: small">
                        <asp:ListItem>Escolha...</asp:ListItem>
                        <asp:ListItem>AC</asp:ListItem>
                        <asp:ListItem>AM</asp:ListItem>
                        <asp:ListItem>BA</asp:ListItem>
                        <asp:ListItem>DF</asp:ListItem>
                        <asp:ListItem>GO</asp:ListItem>
                        <asp:ListItem>MA</asp:ListItem>
                        <asp:ListItem>MS</asp:ListItem>
                        <asp:ListItem>PA</asp:ListItem>
                        <asp:ListItem>PI</asp:ListItem>
                        <asp:ListItem>RO</asp:ListItem>
                        <asp:ListItem>RS</asp:ListItem>
                    </asp:DropDownList>
                        <br />
                    <asp:Label ID="UserNameLabel4" runat="server"  class="meulabel" Width="100px">Município sede</asp:Label>
                    <asp:DropDownList ID="cmbMunicipio" runat="server" Width="350px" style="font-family: Arial; font-size: small" AutoPostBack="True">
                    </asp:DropDownList>
                    <asp:TextBox ID="CampoNO_MUNICIPIO" runat="server" CssClass="textEntry" MaxLength="9" Width="20px" Visible="False"></asp:TextBox>
                        <br />
                    <asp:Label ID="Label13" runat="server" Width="450px" style="font-family: Arial; " BackColor="LightGray" CssClass="auto-style3">Telefones</asp:Label>
                        <br />
                    <asp:Label ID="UserNameLabel7" runat="server"  class="meulabel" Width="85px">Telefone 1</asp:Label>
                        <asp:TextBox ID="CampoDDD1" runat="server" CssClass="textEntry" MaxLength="3" Width="30px" style="font-family: Arial; font-size: small"></asp:TextBox>
                    <asp:TextBox ID="CampoTel1" runat="server" CssClass="textEntry" MaxLength="9" Width="90px" style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
                    <asp:Label ID="UserNameLabel8" runat="server"  class="meulabel" Width="85px">Telefone 2</asp:Label>
                        <asp:TextBox ID="CampoDDD2" runat="server" CssClass="textEntry" MaxLength="3" Width="30px" style="font-family: Arial; font-size: small"></asp:TextBox>
                    <asp:TextBox ID="CampoTel2" runat="server" CssClass="textEntry" MaxLength="9" Width="90px" style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
                    <asp:Label ID="UserNameLabel1" class="meulabel" runat="server" Width="85px">E-mail</asp:Label>
                        <asp:TextBox ID="CampoEmail" runat="server" Width="272px" CssClass="auto-style3"></asp:TextBox>
                        <br />
                    <asp:Label ID="UserNameLabel9" runat="server"  class="meulabel" Width="214px">Local alternativo para entrega</asp:Label>
                        <br />
                        <asp:DropDownList ID="cmbPoloEntrega" runat="server" style="font-family: Arial; font-size: small" Width="421px">
                        </asp:DropDownList>
                        <br />
                        <br />
                    <asp:Label ID="PasswordLabel24" runat="server"  class="meulabel" Width="110px">Instalação</asp:Label>
                        <asp:DropDownList ID="cmbInstalacao" runat="server" CssClass="auto-style1" Width="160px">
                            <asp:ListItem>Ainda não definida</asp:ListItem>
                            <asp:ListItem>Em prédio particular</asp:ListItem>
                            <asp:ListItem>Em prédio público</asp:ListItem>
                        </asp:DropDownList>
                        <br />
                    <asp:Label ID="PasswordLabel25" runat="server"  class="meulabel" Width="110px">Status da negociação</asp:Label>
                        <asp:DropDownList ID="cmbNegociacao" runat="server" CssClass="auto-style1" Width="160px">
                            <asp:ListItem>Ainda não iniciada</asp:ListItem>
                            <asp:ListItem>Em andamento</asp:ListItem>
                            <asp:ListItem>Concluída</asp:ListItem>
                        </asp:DropDownList>
                        <br />
                        <br />
                    <asp:Label ID="LabelIncAlt" runat="server" Width="370px" style="font-family: Arial; font-size: x-small;" BackColor="White" ForeColor="#1C2F67"></asp:Label>
                    </div>
                    </td>
                    <td style="width: 10px">
                        <br />
                    </td>
                    <td>
                    <div class="rcorners2">
                    <asp:Label ID="Label19" runat="server" Width="450px" style="font-family: Arial; " BackColor="LightGray" CssClass="auto-style3">Coordenadores</asp:Label>
                        <br />
                    <asp:Label ID="PasswordLabel20" runat="server" class="meulabel" Width="250px">Nome do Coordenador do Polo</asp:Label>
                        <br />
                        <asp:DropDownList ID="cmbCoordenador" runat="server" Width="421px" style="font-family: Arial; " CssClass="auto-style2">
                        </asp:DropDownList>
                        <br />
                    <asp:TextBox ID="CampoEmailcoord" runat="server" CssClass="textEntry" MaxLength="40" Width="333px" BackColor="#1C2F67" BorderStyle="None" Enabled="False" style="font-family: Arial; font-size: small" ForeColor="White"></asp:TextBox>
                    <asp:TextBox ID="CampoCPF" runat="server" CssClass="textEntry" MaxLength="9" Width="62px" Visible="False"></asp:TextBox>
                        <br />
                        <br />
                    <asp:Label ID="PasswordLabel21" runat="server" class="meulabel" Width="250px">Nome do Apoio Logístico</asp:Label>
                        <br />
                        <asp:DropDownList ID="cmbApoioLogistico" runat="server" Width="421px" style="font-family: Arial; " CssClass="auto-style2">
                        </asp:DropDownList>
                        <br />
                    <asp:TextBox ID="CampoEmail_al" runat="server" CssClass="textEntry" MaxLength="40" Width="333px" BackColor="#1C2F67" BorderStyle="None" Enabled="False" style="font-family: Arial; font-size: small" ForeColor="White"></asp:TextBox>
                    <asp:TextBox ID="CampoCPFal" runat="server" CssClass="textEntry" MaxLength="9" Width="62px" Visible="False"></asp:TextBox>
                        <br />
                        <br />
                    <asp:Label ID="PasswordLabel23" runat="server" class="meulabel" Width="300px">Nome do Subcoordenador Estadual</asp:Label>
                        <br />
                        <asp:DropDownList ID="cmbSubCoordenadorEst" runat="server" Width="421px" style="font-family: Arial; " CssClass="auto-style2">
                        </asp:DropDownList>
                        <br />
                    <asp:TextBox ID="CampoEmailsubc" runat="server" CssClass="textEntry" MaxLength="40" Width="333px" BackColor="#1C2F67" BorderStyle="None" Enabled="False" style="font-family: Arial; font-size: small" ForeColor="White"></asp:TextBox>
                    <asp:TextBox ID="CampoCPFcel" runat="server" CssClass="textEntry" MaxLength="9" Width="62px" Visible="False"></asp:TextBox>
                        <br />
                        <br />
                    <asp:Label ID="Label12" runat="server" Width="450px" style="font-family: Arial; " BackColor="LightGray" CssClass="auto-style3">Características</asp:Label>
                        <br />
                        <asp:Label ID="Label6" runat="server" class="meulabel" Width="150px">Caixas material adm.</asp:Label>
                    <asp:TextBox ID="CampoNU_TOT_CAIXA_ADM" runat="server" CssClass="textEntry" MaxLength="16" Width="100px" BackColor="#1C2F67" BorderStyle="None" Enabled="False" style="text-align:right; font-family: Arial; font-size: small;" ForeColor="White"></asp:TextBox>
                        <br />
                        <asp:Label ID="Label15" runat="server" class="meulabel" Width="150px">Alunos</asp:Label>
                    <asp:TextBox ID="CampoNU_QTALUNO" runat="server" CssClass="textEntry" MaxLength="16" Width="100px" BackColor="#1C2F67" BorderStyle="None" Enabled="False" style="text-align:right; font-family: Arial; font-size: small;" ForeColor="White"></asp:TextBox>
                        <br />
                        <asp:Label ID="Label16" runat="server" class="meulabel" Width="150px">Turmas</asp:Label>
                    <asp:TextBox ID="CampoQT_TURMAS" runat="server" CssClass="textEntry" MaxLength="16" Width="100px" BackColor="#1C2F67" BorderStyle="None" Enabled="False" style="text-align:right; font-family: Arial; font-size: small;" ForeColor="White"></asp:TextBox>
                        <br />
                        <asp:Label ID="Label20" runat="server" class="meulabel" Width="150px">Escolas</asp:Label>
                    <asp:TextBox ID="CampoQT_ESCOLAS" runat="server" CssClass="textEntry" MaxLength="16" Width="100px" BackColor="#1C2F67" BorderStyle="None" Enabled="False" style="text-align:right; font-family: Arial; font-size: small;" ForeColor="White"></asp:TextBox>
                        <br />
                        <asp:Label ID="Label17" runat="server"  class="meulabel" Width="150px">Peso material adm.</asp:Label>
                    <asp:TextBox ID="CampoNU_PESO_MATERIAL_ADM" runat="server" CssClass="textEntry" MaxLength="16" Width="100px" BackColor="#1C2F67" BorderStyle="None" Enabled="False" style="text-align:right; font-family: Arial; font-size: small;" ForeColor="White"></asp:TextBox>
                        <br />
                        <br />
                        <asp:Label ID="Label18" runat="server" class="meulabel" Width="150px">Observações</asp:Label>
                        <br />
                    <asp:TextBox ID="CampoOBS" runat="server" CssClass="textEntry" MaxLength="512" Width="450px" Height="67px" TextMode="MultiLine" ToolTip="Use este espaço para anotações livres." style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
                        <br />
                        &nbsp;&nbsp;&nbsp;
                        <br />
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
