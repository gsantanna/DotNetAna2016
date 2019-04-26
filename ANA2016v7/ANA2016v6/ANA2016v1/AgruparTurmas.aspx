<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="AgruparTurmas.aspx.vb" Inherits="ANA2016v1.MesclarTurmas" %>

<%@ Register Src="~/ctlQuadroMesclagemTopo.ascx" TagPrefix="uc1" TagName="ctlQuadroMesclagemTopo" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <style type="text/css">
        .tblmain th {
            width: 20%;
            min-width: 20%;
            max-width: 20%;
            text-align: center;
            vertical-align: top;
        }

        .tblMain td , .tblMain tr {
            vertical-align: top;
        }


        .hdr {
            background-color: LightGrey;
            min-height: 20px;
            font-weight: bold;
            color: #fff;
        }

        .tblTurmasOriginais {
            width: 100%;
            border-collapse: collapse;
            padding-top: 10px;
            
        }

            .tblTurmasOriginais tr {
                padding-left: 10px;
            }

            .tblTurmasOriginais hdr {
                background-color: #808080;
                padding-left: 0px;
            }


            .bll {
                 border-left:1px solid #808080;

            }
            .brr {
               
            }

            .brTb {
                border-top:1px solid #808080;
                border-bottom:1px solid #808080;
                border-right:1px solid #808080;
            }

            table td 
            {
                vertical-align:top;

            }

    </style>




    <p>&nbsp;</p>


    <h4>Mesclagem de turmas extras da escola: <asp:Label ID="lblEscola" runat="server"></asp:Label>

       
        <img src="FIGURAS/FigDia1.png" width="60" alt="Dia 1 " runat="server" id="imgD1"  class="pull-right" />       
    </h4>

    <h5>Aplicação dia: <asp:Label ID="lblAplicacao" runat="server"></asp:Label> </h5>


    <asp:Label ID="MensagemERRO" CssClass="lblErro" runat="server" ForeColor="Red"></asp:Label>
    <div id="lblErro" style="color:red;"></div>
    <hr />






    <table style="width: 100%" class="tblmain brTb">
        <thead>
            <tr class="hdr">
                <th class="bll">Manhã Antes do recreio</th>
                <th  class="bll">Manhã Após o recreio</th>
                <th  class="bll">Tarde Antes do recreio</th>
                <th  class="bll">Tarde Após o recreio</th>
                <th  class="bll brr">Noite</th>
            </tr>
        </thead>



        <tr style="vertical-align: top">

            <asp:Repeater ID="rptCabecalho" runat="server">
                <ItemTemplate>
                    <td class="bll">
                        <uc1:ctlQuadroMesclagemTopo runat="server" ID="ctlMesclagem1" ID_ESCOLA='<%# Bind("ID_ESCOLA") %>'   ID_GRUPO_SALA='3' TURNO='<%# Bind("TURNO")  %>' ID_PROVA='<%# Bind("ID_PROVA") %>'  ID_DIA_APLICACAO='<%# Bind("ID_DIA_APLICACAO") %>'  />
                        <uc1:ctlQuadroMesclagemTopo runat="server" ID="ctlMesclagem2" ID_ESCOLA='<%# Bind("ID_ESCOLA") %>' ID_GRUPO_SALA='4' TURNO='<%# Bind("TURNO")  %>' ID_PROVA='<%# Bind("ID_PROVA") %>'  ID_DIA_APLICACAO='<%# Bind("ID_DIA_APLICACAO") %>'  />
                    </td>
                </ItemTemplate>
            </asp:Repeater>

            <asp:Label ID="lblVazio" runat="server" Visible="false">
                <td colspan="5" style="padding:60px 0px 60px 20px;text-align:center;">Não há nenhuma turma disponível para agrupamento.</td>
            </asp:Label>
        </tr>


        <tr>
            <td colspan="5" class="hdr bll"  style="margin-top:5px;margin-bottom:5px;padding-top:10px;padding-bottom:10px;text-align:center;">Salas após a mesclagem</td>
        </tr>


        <tr runat="server" id="trSeletores">

            <td id="col1" class="bll"></td>
            <td id="col2" class="bll "></td>
            <td id="col3" class="bll "></td>
            <td id="col4" class="bll "></td>
            <td id="col5" class="bll brr "></td>            

        </tr>



        

        
    </table>

    <div style="padding-top:30px;width:100%;">
    <asp:Button ID="cmdEntrar" runat="server" BackColor="#16B665" BorderStyle="None" ForeColor="White" Text="Confirmar e salvar" Width="140px" Height="24px" UseSubmitBehavior="False"  OnClientClick="if(!valida()) return false;"       />
    &nbsp;
                    <asp:Button ID="cmdCancelar" runat="server" BackColor="White" BorderStyle="Double" ForeColor="Black" Text="Cancelar" UseSubmitBehavior="False" />
                
    </div>
    

    
    <script type="text/javascript">

        $(document).ready(function () { //é executado após o carregamento da página
            $(".cmbColaborador").change(function () { //binda o evento no ONCHANGE do combobox                

                //carrega o novo valor selecionado do controle que disparou o evento $(this)
                var cpf = $("#" + $(this).attr("ID")).val();
                var turno = $("#" + $(this).attr("ID")).attr("data-turno")

                $(".cmbColaborador:not(#" + $(this).attr("ID") + ")").each(function (idx, obj) { //loopa todos os outros controles com a classe css cmbColaborador. MENOS o controle que disparou o evento
                    if ( $(obj).attr("data-turno") == turno &&    $(obj).val() == cpf             ) $(obj).val("0"); 
                });
            })
        })

    
        var valida = function()
        {
            if ($(".cmbColaborador option:checked[value='0']").length > 0) //existe algum item sem preenchimento 
            {
                $("#lblErro").html("É necessário selecionar um aplicador para todas as salas.").show();
                return false; 
            } else
            {
                return true;
            }


        }
        
        
        </script>







</asp:Content>

