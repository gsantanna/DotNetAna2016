<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Notifica.aspx.vb" Inherits="ANA2016v1.Notifica" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


     <asp:Literal ID="MessageErro" runat="server" />

    <h5>A última página recebida violou a regra de sequência do site. Isso pode ter ocorrido por alguma das seguintes razões:</h5>
     <p style="width: 669px">1. Você enviou uma página antiga, provavelmente recebida numa outra sessão de uso.</p>
     <p style="width: 672px">2. Você utilizou o comando &quot;Voltar&quot; do browser em desacordo com o encadeamento lógico deste site.</p>
     <p>3. Houve algum tipo de violação dos parâmetros internos de abertura da página.</p>
     <p>&nbsp;</p>
     <p>&nbsp;</p>
     <p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="cmdVoltar" runat="server" BackColor="#16B665" Text="Voltar para a página de login do site FGV - ANA 2016" Width="381px" ForeColor="White" Height="34px" BorderWidth="0px" ToolTip="Voltar para a página de login do ANA2016" UseSubmitBehavior="False" ViewStateMode="Disabled" />

    </p>
     <h5>. </h5>

</asp:Content>
