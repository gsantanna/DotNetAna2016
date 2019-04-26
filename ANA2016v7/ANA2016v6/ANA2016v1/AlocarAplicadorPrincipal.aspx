<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="AlocarAplicadorPrincipal.aspx.vb" Inherits="ANA2016v1.AlocarAplicadorPrincipal" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>

.rcorners2 {
    border-radius: 10px;
    border: 1px solid #73AD21;
    padding: 15px;
    width: 600px;
    height: 400px;
}
.meulabel {
    font-family: Arial;
    font-size: x-small;
}
     .auto-style1 {
         font-family: Arial;
         font-size: small;
     }
 </style>
    <div class="w3-main"   style="margin-left:10px;margin-top:10px;">
        <asp:Literal ID="MensagemERRO" runat="server" />
        <div class="rcorners2">
            <h4>Alocar o aplicador principal para a turma:</h4>
            <h4>&nbsp;<asp:Literal ID="CampoTurma" runat="server"></asp:Literal>
                &nbsp;
            </h4>
            <p>
                &nbsp;</p>
            <asp:Label ID="Label3" runat="server" class="meulabel" Width="126px" Height="16px">Aplicador</asp:Label>
            <asp:DropDownList ID="cmbFuncionario" runat="server" CssClass="auto-style1" Width="400px">
                <asp:ListItem>Escolha...</asp:ListItem>               
            </asp:DropDownList>
            &nbsp;&nbsp;&nbsp;
            <br />
            <br />
            <br />
            <br />
            <asp:Label ID="Label4" runat="server" class="meulabel" Width="126px"></asp:Label>
            <asp:Button ID="cmdEntrar" runat="server" BackColor="#16B665" BorderStyle="None" ForeColor="White" Text="Confirmar e salvar" Width="140px" Height="24px" UseSubmitBehavior="False" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="cmdCancelar" runat="server" BackColor="White" BorderStyle="Double" ForeColor="Black" Text="Cancelar" UseSubmitBehavior="False" />
            <br />
            <br />
            <asp:Label ID="Label5" runat="server" class="meulabel" Width="126px"></asp:Label>
            <asp:Button ID="cmdExcluir" runat="server" BackColor="#16B665" BorderStyle="None" ForeColor="White" Text="Excluir aplicador" Width="137px" />
            <br />
        </div>
    </div>
</asp:Content>
