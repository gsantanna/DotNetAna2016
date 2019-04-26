<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="OrcamentoPolo3.aspx.vb" Inherits="ANA2016v1.OrcamentoPolo3" %>
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
    font-size: small;
        font-weight: 400;
    }
.meucampo {
    font-family: Arial;
    font-size: x-small;
}
        .auto-style3 {
            font-size: medium;
            font-weight: 700;
        }
        .auto-style4 {
            width: 553px;
            height: 400px;
        }
        .auto-style5 {
            width: 521px;
        }
        </style>
<div class="w3-main"   style="margin-left:0px;margin-top:10px;">
    <div>
        <asp:Literal ID="MensagemERRO" runat="server" />
        <h5>&nbsp;<asp:TextBox ID="CampoID_POLO" runat="server" CssClass="textEntry" MaxLength="4" Width="85px" style="font-family: Arial; font-size: large" BorderStyle="None" Enabled="False"></asp:TextBox>
                        &nbsp;&nbsp; &nbsp;&nbsp;<asp:TextBox ID="CampoSG_UF" runat="server" CssClass="textEntry" MaxLength="4" Width="48px" style="font-family: Arial; font-size: large" BorderStyle="None" Enabled="False"></asp:TextBox>
                        &nbsp; 
            <asp:TextBox ID="CampoNO_POLO" runat="server" CssClass="textEntry" MaxLength="4" Width="300px" style="font-family: Arial; font-size: large" BorderStyle="None" Enabled="False"></asp:TextBox>
                           &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </h5>
        <h5>
                        <asp:Label ID="Label28" runat="server"  class="meulabel" Width="177px"></asp:Label>
                        &nbsp;<asp:Button ID="cmdEntrar" runat="server" BackColor="#16B665" BorderStyle="None" Text="Confirmar e salvar" OnClick="cmdEntrar_Click" Width="156px" ForeColor="White" ToolTip="Clique aqui para validar os dados e salvar" UseSubmitBehavior="False" BorderWidth="0px" Height="28px" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="cmdCancelar" runat="server" BackColor="White" BorderStyle="Double" Text="Cancelar" OnClick="cmdCancelar_Click" Width="94px" ForeColor="Black" ToolTip="Clique aqui para abandonar o cadastramento" UseSubmitBehavior="False" />
        </h5>
        <table class="auto-style4">
            <tr>
                <td class="auto-style5">
                    <div class="auto-style5">
                        <br />
                        <asp:Label ID="Label21" runat="server" Width="452px" BackColor="LightGray" CssClass="auto-style3" Height="25px">Capacitação dos aplicadores e Apoio Logístico</asp:Label>
                        <br />
                        <br />
                        <asp:Label ID="Label1" runat="server"  class="meulabel" Width="326px" Height="16px">Valor da ajuda de custo: R$ 30,00 por pessoa</asp:Label>
                        <br />
                        <br />
                        <asp:Label ID="Label2" runat="server"  class="meulabel" Width="363px">Número de aplicadores (incluindo Apoio Logístico) </asp:Label>
                        <asp:TextBox ID="CampoNumPessoas" runat="server" CssClass="textEntry" MaxLength="2" Width="80px" style="font-family: Arial; font-size: small" EnableViewState="False"></asp:TextBox>
                        <br />
                        <asp:Label ID="Label27" runat="server"  class="meulabel" Width="278px"></asp:Label>
                        <asp:Label ID="Label26" runat="server"  class="meulabel" Width="89px">Valor total:</asp:Label>
                        <asp:TextBox ID="CampoValorTotal" runat="server" CssClass="textEntry" MaxLength="8" Width="80px" style="font-family: Arial; font-size: medium" BorderStyle="None" Enabled="False"></asp:TextBox>
                        <br />
                        <br />
                        <asp:Label ID="Label29" runat="server"  class="meulabel" Width="89px">Observações</asp:Label>
                        <br />
                        <asp:TextBox ID="CampoOBS" runat="server" CssClass="textEntry" MaxLength="512" Width="454px" Height="67px" TextMode="MultiLine" ToolTip="Use este espaço para anotações livres." style="font-family: Arial; font-size: small"></asp:TextBox>
                        <br />
                        <br />
                        <asp:Label ID="LabelIncAlt" runat="server" Width="370px" style="font-family: Arial; font-size: x-small;" BackColor="White" ForeColor="#1C2F67"></asp:Label>
                    </div>
                </td>
            </tr>
        </table>
        <br />
    </div>
</div>
</asp:Content>
