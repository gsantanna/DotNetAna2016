<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="EscolhaUFbase.aspx.vb" Inherits="ANA2016v1.EscolhaUFbase" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    
    <div class="w3-main"   style="margin-left:10px;margin-top:10px;">
    <div class="accountInfo" style="width: 870px">
    <div>
        <asp:Literal ID="MensagemERRO" runat="server" />
        <br />
        <h4>Escolha um estado de referência para a presente sessão.</h4>

        <table style="width:447px;">
            <tr>
                <td style="width:160px;"></td>
                <td  style="width:300px;">
                    <br />
                    <br />
                    <asp:Label ID="Label1" runat="server" Width="147px" style="font-family: Arial; font-size: small">UF de referência</asp:Label>
                    <span style="font-family: Arial; font-size: small">
                        <strong>
                        <asp:DropDownList ID="cmbEstadoBase" runat="server" style="font-family: Arial; font-size: medium;" Width="261px" CssClass="auto-style1" Height="22px">
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
                        </strong>
                    <br />
                    <br />
                    </span>
                    <br />
                    <br />
                        <span style="font-family: Arial; font-size: small">
                    <asp:Label ID="Label4" runat="server" Width="118px" style="font-family: Arial; font-size: small"></asp:Label>
                    </span>
                        <asp:Button ID="cmdOK" runat="server" BackColor="Red" BorderStyle="Double" Text="OK" OnClick="cmdEntrar_Click" Width="100px" ForeColor="White" Height="35px" TabIndex="3" UseSubmitBehavior="False" />
                        <br />
                    <br />
                </td>
            
        </table>
    </div>
    </div>

    </div>

</asp:Content>
