<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="RegistraAplicacaoTurma.aspx.vb" Inherits="ANA2016v1.RegistraAplicacaoTurma" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>

.rcorners2 {
    border-radius: 10px;
    border: 1px solid #73AD21;
    padding: 15px;
    width: 700px;
    height: 500px;
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
            <h4>Registro de aplicação para</h4>
            <h4>&nbsp;<asp:Literal ID="CampoPessoa" runat="server"></asp:Literal>
                &nbsp;
            </h4>
            <p>
                &nbsp;</p>
            <br />
            <br />
            <asp:Label ID="Label3" runat="server" class="meulabel" Width="123px" Height="16px">Houve aplicação?</asp:Label>
            <asp:DropDownList ID="cmbSimNao" runat="server" CssClass="auto-style1" Width="111px" AutoPostBack="True" Height="20px">
                <asp:ListItem>Escolha...</asp:ListItem>
                <asp:ListItem>Sim</asp:ListItem>
                <asp:ListItem>Não</asp:ListItem>
            </asp:DropDownList>
            <asp:TextBox ID="CampoSG_UFbase" runat="server" BackColor="Silver" BorderStyle="None" CssClass="auto-style1" Visible="False" Width="48px"></asp:TextBox>
            <br />
            <br />
            <asp:Label ID="Label2" runat="server" class="meulabel" Width="134px">Motivo da não aplicação</asp:Label>
            <asp:DropDownList ID="cmbMotivo" runat="server" CssClass="auto-style1" Width="625px" Visible="False" Height="16px">
                <asp:ListItem>Escolha...</asp:ListItem>
                <asp:ListItem>Escola fechada</asp:ListItem>
                <asp:ListItem>Escola mudou de endereço</asp:ListItem>
                <asp:ListItem>Diretor(a) se negou a aplicar</asp:ListItem>
                <asp:ListItem>Turma inexistente ou não aplicável (ex:Indígena)</asp:ListItem>
                <asp:ListItem>Falta coletiva</asp:ListItem>
                <asp:ListItem>Atividade externa</asp:ListItem>
                <asp:ListItem>Outros</asp:ListItem>
                <asp:ListItem>Escola Inexistente</asp:ListItem>
                <asp:ListItem>Diretor(a) se negou a agendar</asp:ListItem>
                <asp:ListItem>Erro de preenchimento de cadastro do CENSO</asp:ListItem>
                <asp:ListItem>Temporal ou similar que impossibilitou aplicação.</asp:ListItem>
                <asp:ListItem>Turma UNIFICADA -&gt; INFORME A TURMA ASSOCIADA</asp:ListItem>
            </asp:DropDownList>
            <br />
            <br />
            <br />
                        <asp:Label ID="Label18" runat="server" class="meulabel" Width="115px">Observações</asp:Label>
                    <asp:TextBox ID="CampoOBS" runat="server" CssClass="textEntry" MaxLength="512" Width="503px" Height="67px" TextMode="MultiLine" ToolTip="Use este espaço para anotações livres." style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
            <br />
            <br />
            <asp:Label ID="Label4" runat="server" class="meulabel" Width="368px" Height="26px"></asp:Label>
            <asp:Button ID="cmdEntrar" runat="server" BackColor="#16B665" BorderStyle="None" ForeColor="White" Text="Confirmar e salvar" Width="140px" Height="24px" UseSubmitBehavior="False" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="cmdCancelar" runat="server" BackColor="White" BorderStyle="Double" ForeColor="Black" Text="Cancelar" UseSubmitBehavior="False" />
            <br />
        </div>
    </div>
</asp:Content>
