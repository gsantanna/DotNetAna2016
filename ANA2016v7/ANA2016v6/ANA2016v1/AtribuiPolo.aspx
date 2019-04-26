<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="AtribuiPolo.aspx.vb" Inherits="ANA2016v1.AtribuiPolo" %>
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
         font-size: medium;
     }
 </style>
    <div class="w3-main"   style="margin-left:10px;margin-top:10px;">
        <asp:Literal ID="MensagemERRO" runat="server" />
        <div class="rcorners2">
            <h4>Associação a polo para</h4>
            <h4>&nbsp;<asp:Literal ID="CampoPessoa" runat="server"></asp:Literal>
                &nbsp;
            </h4>
            <p>
                &nbsp;</p>
            <asp:Label ID="Label1" runat="server" class="meulabel" Width="60px" >CPF</asp:Label>
            <asp:TextBox ID="CampoCPF" runat="server" BackColor="Silver" BorderStyle="None" CssClass="auto-style1" Enabled="False" Width="137px"></asp:TextBox>
            <br />
            <br />
            <asp:Label ID="Label3" runat="server" class="meulabel" Width="126px" Height="16px">Função/Especialidade</asp:Label>
            <asp:DropDownList ID="cmbFuncao" runat="server" CssClass="auto-style1" Width="300px">
                <asp:ListItem>Escolha...</asp:ListItem>
                <asp:ListItem>Aplicador</asp:ListItem>
                <asp:ListItem>Guia-Intérprete</asp:ListItem>
                <asp:ListItem>Intérprete de Libras</asp:ListItem>
                <asp:ListItem>Ledor/Transcritor</asp:ListItem>
            </asp:DropDownList>
            &nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="CampoSG_UFbase" runat="server" BackColor="Silver" BorderStyle="None" CssClass="auto-style1" Visible="False" Width="48px"></asp:TextBox>
            <br />
            <asp:Label ID="Label2" runat="server" class="meulabel" Width="126px" Height="16px">Polo</asp:Label>
            <asp:DropDownList ID="cmbPolo" runat="server" CssClass="auto-style1" Width="300px">
            </asp:DropDownList>
            <br />
            <br />
            <br />
            <asp:Label ID="Label4" runat="server" class="meulabel" Width="60px"></asp:Label>
            <asp:Button ID="cmdEntrar" runat="server" BackColor="#16B665" BorderStyle="None" ForeColor="White" Text="Confirmar e salvar" Width="140px" Height="24px" UseSubmitBehavior="False" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="cmdCancelar" runat="server" BackColor="White" BorderStyle="Double" ForeColor="Black" Text="Cancelar" UseSubmitBehavior="False" />
            <br />
        </div>
    </div>
</asp:Content>
