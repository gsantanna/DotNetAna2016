<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="OrcamentoPolo.aspx.vb" Inherits="ANA2016v1.OrcamentoPolo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style>
.rcorners2 {
    border-radius: 5px;
    border: 1px solid #1c2f67;
    padding: 15px;
    width: 740px;
    height: 500px;
}
.meulabel {
    font-family: Arial;
    font-size: x-small;
}
.meucampo {
    font-family: Arial;
    font-size: x-small;
}
        .auto-style3 {
            font-size: small;
        }
        .auto-style4 {
            width: 1100px;
            height: 534px;
        }
        .auto-style5 {
            width: 1047px;
        }
        </style>


    <div class="w3-main"   style="margin-left:0px;margin-top:10px;">

        <div>
            <asp:Literal ID="MensagemERRO" runat="server" />
                       <h5>&nbsp;<asp:TextBox ID="CampoID_POLO" runat="server" CssClass="textEntry" MaxLength="4" Width="85px" style="font-family: Arial; font-size: large" BorderStyle="None" Enabled="False"></asp:TextBox>
                        &nbsp;&nbsp; &nbsp;&nbsp;<asp:TextBox ID="CampoSG_UF" runat="server" CssClass="textEntry" MaxLength="4" Width="48px" style="font-family: Arial; font-size: large" BorderStyle="None" Enabled="False"></asp:TextBox>
                        &nbsp; <asp:TextBox ID="CampoNO_POLO" runat="server" CssClass="textEntry" MaxLength="4" Width="300px" style="font-family: Arial; font-size: large" BorderStyle="None" Enabled="False"></asp:TextBox>
                           &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="cmdEntrar" runat="server" BackColor="#16B665" BorderStyle="None" Text="Confirmar e salvar" OnClick="cmdEntrar_Click" Width="156px" ForeColor="White" ToolTip="Clique aqui para validar os dados e salvar" UseSubmitBehavior="False" BorderWidth="0px" Height="28px" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="cmdCancelar" runat="server" BackColor="White" BorderStyle="Double" Text="Cancelar" OnClick="cmdCancelar_Click" Width="94px" ForeColor="Black" ToolTip="Clique aqui para abandonar o cadastramento" UseSubmitBehavior="False" />
                        </h5>
            <table class="auto-style4">
                <tr>
                    <td class="auto-style5">
                    <div class="auto-style5">
                    <asp:Label ID="Label11" runat="server" Width="615px" style="font-family: Arial; " BackColor="LightGray" CssClass="auto-style3">Transporte não aéreo</asp:Label>
                        <br />
                        &nbsp;<asp:Label ID="PasswordLabel27" runat="server"  class="meulabel" Width="114px" Height="16px">Via terrestre</asp:Label>
                        <asp:Label ID="PasswordLabel26" runat="server"  class="meulabel" Width="123px">Via fluvial</asp:Label>
                        <asp:Label ID="PasswordLabel34" runat="server"  class="meulabel" Width="70px">Outras vias</asp:Label>
                        <br />
                    <asp:TextBox ID="CampoViaTerrestre" runat="server" CssClass="textEntry" MaxLength="8" Width="80px" style="font-family: Arial; font-size: small"></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="CampoViaFluvial" runat="server" CssClass="textEntry" MaxLength="8" Width="80px" style="font-family: Arial; font-size: small"></asp:TextBox>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="CampoOutrasVias" runat="server" CssClass="textEntry" MaxLength="8" Width="80px" style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
                        <asp:Label ID="PasswordLabel32" runat="server"  class="meulabel" Width="327px">Discriminação resumida das despesas com transporte não aéreo</asp:Label>
                        <br />
                    <asp:TextBox ID="CampoDiscriminacaoTransporte" runat="server" CssClass="textEntry" MaxLength="250" Width="550px" style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
                        <br />
                    <asp:Label ID="Label21" runat="server" Width="615px" style="font-family: Arial; " BackColor="LightGray" CssClass="auto-style3">Trechos aéreos</asp:Label>
                        <br />
                    <asp:Label ID="Label22" runat="server" Width="100px" style="font-family: Arial; " BackColor="LightGray" CssClass="auto-style3" Height="18px">Ida</asp:Label>
                        <br />
                    <asp:Label ID="UserNameLabel14" runat="server"  class="meulabel" Width="120px" Height="16px">Horário preferencial</asp:Label>
                    <asp:Label ID="UserNameLabel10" runat="server"  class="meulabel" Width="259px">De</asp:Label>
                    <asp:Label ID="UserNameLabel11" runat="server"  class="meulabel" Width="32px" Height="16px">Para</asp:Label>
                        <br />
                    <asp:DropDownList ID="cmbHorarioIda" runat="server" Width="100px" style="font-family: Arial; font-size: small">
                        <asp:ListItem>Escolha...</asp:ListItem>
                        <asp:ListItem>Manhã</asp:ListItem>
                        <asp:ListItem>Tarde</asp:ListItem>
                        <asp:ListItem>Noite</asp:ListItem>
                        <asp:ListItem>Madrugada</asp:ListItem>
                        <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="CampoMunicipioIdaDe" runat="server" CssClass="textEntry" MaxLength="50" Width="240px" style="font-family: Arial; font-size: small"></asp:TextBox>
                        &nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="CampoMunicipioIdaPara" runat="server" CssClass="textEntry" MaxLength="50" Width="240px" style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
                    <asp:Label ID="Label23" runat="server" Width="200px" style="font-family: Arial; " BackColor="LightGray" CssClass="auto-style3" Height="18px">Volta</asp:Label>
                        <br />
                    <asp:Label ID="UserNameLabel15" runat="server"  class="meulabel" Width="123px" Height="16px">Horário preferencial</asp:Label>
                    <asp:Label ID="UserNameLabel16" runat="server"  class="meulabel" Width="250px">De</asp:Label>
                    <asp:Label ID="UserNameLabel17" runat="server"  class="meulabel" Width="32px" Height="16px">Para</asp:Label>
                        <br />
                    <asp:DropDownList ID="cmbHorarioVolta" runat="server" Width="100px" style="font-family: Arial; font-size: small">
                        <asp:ListItem>Escolha...</asp:ListItem>
                        <asp:ListItem>Manhã</asp:ListItem>
                        <asp:ListItem>Tarde</asp:ListItem>
                        <asp:ListItem>Noite</asp:ListItem>
                        <asp:ListItem>Madrugada</asp:ListItem>
                        <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="CampoMunicipioVoltaDe" runat="server" CssClass="textEntry" MaxLength="50" Width="240px" style="font-family: Arial; font-size: small"></asp:TextBox>
                        &nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="CampoMunicipioVoltaPara" runat="server" CssClass="textEntry" MaxLength="50" Width="240px" style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
                        <br />

                    <asp:Label ID="Label24" runat="server" Width="615px" style="font-family: Arial; " BackColor="LightGray" CssClass="auto-style3">Despesas durante o percurso</asp:Label>
                        <br />
                        <asp:Label ID="PasswordLabel29" runat="server"  class="meulabel" Width="126px">Hospedagem</asp:Label>
                        <asp:Label ID="PasswordLabel30" runat="server"  class="meulabel" Width="132px" Height="16px">Alimentação</asp:Label>
                        <asp:Label ID="PasswordLabel31" runat="server"  class="meulabel" Width="95px">Despesas extras</asp:Label>
                        <br />
                    <asp:TextBox ID="CampoHospedagem" runat="server" CssClass="textEntry" MaxLength="8" Width="80px" style="font-family: Arial; font-size: small"></asp:TextBox>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="CampoAlimentacao" runat="server" CssClass="textEntry" MaxLength="8" Width="85px" style="font-family: Arial; font-size: small"></asp:TextBox>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="CampoOutrasDespesas" runat="server" CssClass="textEntry" MaxLength="8" Width="80px" style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
                    &nbsp;<asp:Label ID="PasswordLabel33" runat="server"  class="meulabel" Width="218px">Discriminação resumida das despesas extras</asp:Label>
                        <br />
                    <asp:TextBox ID="CampoDiscriminacaoDespeas" runat="server" CssClass="textEntry" MaxLength="250" Width="550px" style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
                        <br />
                    <asp:Label ID="Label25" runat="server" Width="84px" style="font-family: Arial; " BackColor="LightGray" CssClass="auto-style3">Observações</asp:Label>
                    &nbsp;&nbsp;
                    <asp:TextBox ID="CampoOBS" runat="server" CssClass="textEntry" MaxLength="512" Width="454px" Height="67px" TextMode="MultiLine" ToolTip="Use este espaço para anotações livres." style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
                        <br />
                    <asp:Label ID="LabelIncAlt" runat="server" Width="370px" style="font-family: Arial; font-size: x-small;" BackColor="White" ForeColor="#1C2F67"></asp:Label>
                    </div>
                    </td>
                    <td style="width: 10px">
                        <br />
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                
            </table>
            <br />
        </div>

    </div>
</asp:Content>
