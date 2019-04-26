<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UserControlAgendamento.ascx.vb" Inherits="ANA2016v1.UserControlAgendamento" %>
<style>
.rcorners2 {
    border-radius: 5px;
    border: 1px solid #73AD21;
    padding: 5px;
    width: 120px;
    height: 80px;
}
    .auto-style1 {
        font-size: x-small;
        margin-left: 0px;
    }
</style>
<div class="rcorners2">

    <asp:ImageButton ID="cmdEditar" runat="server" ImageUrl="~/FIGURAS/basic1-002_write_pencil_new_edit.png" />
    <br />
    <asp:TextBox ID="TextBox1" runat="server" BorderStyle="None" CssClass="auto-style1" Width="116px" Height="16px"></asp:TextBox>

 </div>

