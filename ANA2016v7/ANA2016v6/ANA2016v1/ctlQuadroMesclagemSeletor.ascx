<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ctlQuadroMesclagemSeletor.ascx.vb" Inherits="ANA2016v1.ctlQUadroMesclagemSeletor" %>






<table class="tblTurmasOriginais" style="margin-top: 5px; margin-bottom: 20px;">

    <tr class="hdr">
        <td>
            <asp:Label runat="server" ID="lblDeficiencia">#### DEFICIENCIA ####</asp:Label>
        </td>
    </tr>



    <asp:Repeater ID="rptMain" runat="server" OnItemCreated="rptMain_ItemCreated">
        <ItemTemplate>
            <tr>

                <td>Alunos: 
                    <asp:Label ID="lblAlocados" runat="server" Text='<%# Bind("ALOCADOS") %>' />/
                    <asp:Label ID="lblCapacidade" runat="server" Text='<%# Bind("CAPACIDADE") %>' />
                    <br />
                    Aplicador:
                    <br />
                    <asp:DropDownList ID="cmbAplicadoresDisp" runat="server" CssClass="cmbColaborador" DataSource='<%# getAplicadoresDisponiveis() %>' DataValueField="CPF" DataTextField="NO_PESSOA" Width="90%" />




                </td>
            </tr>
        </ItemTemplate>

    </asp:Repeater>


</table>


