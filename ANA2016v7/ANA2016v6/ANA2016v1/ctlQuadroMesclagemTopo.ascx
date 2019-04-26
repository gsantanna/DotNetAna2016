<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ctlQuadroMesclagemTopo.ascx.vb" Inherits="ANA2016v1.ctlQuadroMesclagemTopo" %>







<asp:Panel ID="pnlMain" runat="server" Visible="false">
    <table class="tblTurmasOriginais">
        <tr class="hdr">
            <td colspan="2">
                <asp:Label runat="server" ID="lblDeficiencia">Deficiencia</asp:Label>

            </td>
        </tr>

        <asp:Repeater ID="rptMain" runat="server">
            <ItemTemplate>
                <tr>
                    <td>Sala: <asp:Label runat="server">
                        <%# DataBinder.Eval(Container.DataItem, "NO_TURMA") %> - 
                        <%# DataBinder.Eval(Container.DataItem, "SEQ_TURMA_SALA") %>


                              </asp:Label>
                        
                    </td>
                    <td> <%# DataBinder.Eval(Container.DataItem, "NU_ALUNOS_ALOCADOS") %>                        
                        / 
                        <%# DataBinder.Eval(Container.DataItem, "NU_CAPACIDADE_ALUNOS") %>

                        <asp:Label runat="server"></asp:Label></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>







    </table>










</asp:Panel>

