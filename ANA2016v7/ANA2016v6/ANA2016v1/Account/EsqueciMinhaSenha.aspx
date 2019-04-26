<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="EsqueciMinhaSenha.aspx.vb" Inherits="ANA2016v1.EsqueciMinhaSenha" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="w3-main"   style="margin-left:60px;margin-top:30px;">
        <fieldset class="login">
                   
                    <legend>Uma nova senha foi enviada para o e-mail </legend>
        <asp:Literal ID="QueEmail" runat="server"></asp:Literal>
    </fieldset>
        </div>
</asp:Content>
