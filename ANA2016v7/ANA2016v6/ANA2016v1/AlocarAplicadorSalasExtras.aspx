<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="AlocarAplicadorSalasExtras.aspx.vb" Inherits="ANA2016v1.AlocarAplicadorSalasExtras" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .rcorners2 {
            border-radius: 10px;
            border: 1px solid #73AD21;
            padding: 15px;
            width: 917px;
            min-height: 400px;
        }

        .meulabel {
            font-family: Arial;
            font-size: x-small;
        }
    </style>
    <div class="w3-main" style="margin-left: 10px; margin-top: 10px; width: 980px;">
        <asp:Literal ID="MensagemERRO" runat="server" />
        <div class="rcorners2">
            <h4>Atribuir aplicadores para as salas extras da turma:</h4>
            <h4>&nbsp;<asp:Literal ID="CampoTurma" runat="server"></asp:Literal>
                &nbsp;
            </h4>

            <asp:Panel ID="pnlMain" runat="server">
            </asp:Panel>



            <br />
            <asp:Button ID="cmdEntrar" runat="server" BackColor="#16B665" BorderStyle="None" ForeColor="White" Text="Confirmar e salvar" Width="140px" Height="24px" UseSubmitBehavior="False" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="cmdCancelar" runat="server" BackColor="White" BorderStyle="Double" ForeColor="Black" Text="Cancelar" UseSubmitBehavior="False" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="cmdExcluir" runat="server" BackColor="#16B665" BorderStyle="None" ForeColor="White" Text="Excluir todos os aplicadores" Width="221px" />
            <br />
        </div>
    </div>




    <script type="text/javascript">

        $(document).ready(function () { //é executado após o carregamento da página
            $(".cmbColaborador").change(function () { //binda o evento no ONCHANGE do combobox                
                //carrega o novo valor selecionado do controle que disparou o evento $(this)
                var cpf = $("#" + $(this).attr("ID")).val();              
                $(".cmbColaborador:not(#" + $(this).attr("ID") + ")").each(function (idx, obj) { //loopa todos os outros controles com a classe css cmbColaborador. MENOS o controle que disparou o evento
                    if ($(obj).val() == cpf) $(obj).val("0"); 
                });
            })
        })

    </script>




</asp:Content>



