<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="NovaSenha.aspx.vb" Inherits="ANA2016v1.NovaSenha" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="w3-main"   style="margin-left:10px;margin-top:10px;">
    <div class="accountInfo" style="width: 870px">
        <div>
            <asp:Literal ID="MensagemERRO" runat="server" />

        <h4>Sua senha temporária deve ser substituída agora. Escolha uma nova senha.</h4>


        <table style="width:750px;">
            <tr>
                <td style="width:160px;"></td>
                <td  style="width:300px;">
                    <asp:Label ID="Label1" runat="server" AssociatedControlID="CampoNovaSenha" Width="179px" style="font-family: Arial; font-size: small">Nova senha</asp:Label>
                    <asp:TextBox ID="CampoNovaSenha" runat="server" CssClass="passwordEntry" MaxLength="9" Width="100px" TextMode="Password"></asp:TextBox>
                    <span style="font-family: Arial; font-size: small">
                    <br />
                    <asp:Label ID="Label2" runat="server" AssociatedControlID="CampoConfirmacao" Width="179px" style="font-family: Arial; font-size: small">Confirmação da nova senha</asp:Label>
                    <asp:TextBox ID="CampoConfirmacao" runat="server" CssClass="passwordEntry" MaxLength="9" Width="100px" TabIndex="1" TextMode="Password"></asp:TextBox>
                    <br />
                    <asp:Label ID="Label3" runat="server" AssociatedControlID="CampoSenha" Width="179px" style="font-family: Arial; font-size: small">Senha atual</asp:Label>
                    <asp:TextBox ID="CampoSenha" runat="server" CssClass="passwordEntry" MaxLength="9" Width="100px" TabIndex="2" TextMode="Password"></asp:TextBox>
                    <br />
                    </span>
                    <br />
                    <br />
                        <span style="font-family: Arial; font-size: small">
                    <asp:Label ID="Label4" runat="server" Width="179px" style="font-family: Arial; font-size: small"></asp:Label>
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
