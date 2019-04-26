<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="AGENDAMENTO.aspx.vb" Inherits="ANA2016v1.AGENDAMENTO" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">


</script>

    <style>
table {
    width:100%;
}
table, th, td {
    border: none;
    border-collapse: collapse;
    font-family:'Trebuchet MS', 'Lucida Sans Unicode', 'Lucida Grande', 'Lucida Sans', Arial, sans-serif;
    font-size:small  
}
th, td {
    padding: 1px;
    text-align: left;
}
table#t01 tr:nth-child(even) {
    background-color: #eee;
}
table#t01 tr:nth-child(odd) {
   background-color:#fff;
}
table#t01 th {
    background-color: black;
    color: white;
}

.meulabel {
    font-family: Arial;
    font-size: x-small;
}
.meucampo {
    font-family: Arial;
    font-size: small;
}

#MainContent_TabelaPolos td {
    text-align:center;


}
        </style>

      <div>
            <asp:Literal ID="MensagemERRO" runat="server"  />
            <asp:TextBox ID="CaixaRemocao" runat="server" Visible="False" Width="115px"></asp:TextBox>
            <h5><asp:Literal ID="Titulo" runat="server"  />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="cmdVoltar" runat="server" BackColor="#16B665" Text="Voltar" Width="153px" ForeColor="White" Height="34px" BorderWidth="0px" UseSubmitBehavior="False" />

                </h5>
            <asp:Table ID="TabelaPolos" runat="server" EnableViewState="False" ViewStateMode="Disabled"
                border-collapse="collapse" GridLines="Horizontal">
            </asp:Table>
            <br />
      </div>


</asp:Content>
