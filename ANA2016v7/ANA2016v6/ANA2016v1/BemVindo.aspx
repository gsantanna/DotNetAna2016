<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="BemVindo.aspx.vb" Inherits="ANA2016v1.BemVindo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
.pms_link {
    background:#ffffff;
    font-style:normal;
    height:12px;
    width:410px;
    font-family: Arial, Helvetica, sans-serif;
    font-size: small;
    color:#1c2f67;
}

.rcorners2 {
    background:#ffffff;
    border: 1px solid #0096d6;
    border-radius: 2px;
    padding: 15px;
    width: 430px;
    height: 220px;
}

/*.rcorners2 {
    background:#808080;
    border-radius: 5px;
    border: 0px;
    padding: 10px;
    width: 320px;
    height: 200px;
}*/


        .auto-style4 {
            font-family: Arial;
            font-size: x-small;
        }
        </style>
     <asp:Literal ID="MessageError" runat="server" />
    <p></p>
    <h4>
                    <asp:Image Height="25px" width="40px" ID="Bandeirinha" runat="server" BorderStyle="None" BorderWidth="0px" />
                    &nbsp;&nbsp;
                    Bem-vindo(a), 
        <asp:Literal ID="NomeSaudacao" runat="server"></asp:Literal>
    &nbsp;&nbsp;&nbsp;
                    <asp:ImageButton ID="cmdOutroEstado" runat="server" ImageUrl="~/FIGURAS/basic2-009_flag.png" ToolTip="Clique aqui para escolhar outro estado" Visible="False" />
    </h4>
    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/NOTIFICACOES/ANA2016_tutorial.pdf">Clique aqui para consultar o tutorial do sistema</asp:HyperLink>
    <br />
    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/NOTIFICACOES/ANA2016_tutorial.pptx">Clique para receber a apresentação Power Point</asp:HyperLink>
    <asp:Panel ID="Panel1" runat="server" Height="563px">

    <table>
         <tr>
             <td valign:"top" >
                 <div class="rcorners2">
                     <p>
                     <asp:Image ID="Image1" runat="server" ImageUrl="~/FIGURAS/basic2-158_home_house.png" />
                     &nbsp;&nbsp; <strong>POLOS</strong></p>
                         <asp:LinkButton ID="LinkVerPolos" runat="server" class="pms_link">MAPA GERAL DOS POLOS</asp:LinkButton>
                         <p class="auto-style4">Obter uma visão geral dos polos, seus gestores e localização</p>

                         <asp:LinkButton ID="LinkNovoPolo" runat="server" class="pms_link">CRIAR UM NOVO POLO</asp:LinkButton>
                         <p class="auto-style4">Criar um novo polo</p>

                         <asp:LinkButton ID="LinkPendenciasPolo" runat="server" class="pms_link">PENDÊNCIAS</asp:LinkButton>
                         <p class="auto-style4">Identificar pendências  relativas ao cadastro dos polos e designação de gestores<p>
                 </div>
             </td>
             <td style="width: 40px;"></td>
             <td valign:"top" >
                 <div class="rcorners2">
                     <p>
                     <asp:Image ID="Image2" runat="server" ImageUrl="~/FIGURAS/basic2-110_user.png" />
                     &nbsp;&nbsp; <strong>COLABORADORES</strong></p>
                         <asp:LinkButton ID="LinkTriagem" runat="server" class="pms_link">TRIAGEM DOS COLABORADORES</asp:LinkButton>
                         <p class="auto-style4">Aplicar o processo de triagem dos cadastrados, identificando gestores e aplicadores</p>

                         <asp:LinkButton ID="LinkFuncoes" runat="server" class="pms_link">GESTORES & FUNÇÕES</asp:LinkButton>
                         <p class="auto-style4">Analisar a distribuição de funções aos gestores e efeturar novas designações</p>

                         <asp:LinkButton ID="LinkPolosColaboradores" runat="server" class="pms_link">POLOS & COLABORADORES</asp:LinkButton>
                         <p class="auto-style4">Visualizar em cada polo os colaboradores associados para alocação em aplicações</p>

                         <asp:LinkButton ID="LinkESPECIALIZADOS" runat="server" class="pms_link">CADASTRO DE ESPECIALISTAS</asp:LinkButton>
                         &nbsp;&nbsp;&nbsp;
                     <asp:Image ID="Image6" runat="server" ImageUrl="~/FIGURAS/basic2-239_brightness_sun.png" />
                     &nbsp;<p class="auto-style4">Visualizar o cadastro de aplicadores especializados</p>
                 </div>
             </td>
        </tr>

        <tr><td style="height: 40px;"></td>

        </tr>
        
        <tr>
              <td valign:"top" >
                 <div class="rcorners2">
                     <p>
                     <asp:Image ID="Image3" runat="server" ImageUrl="~/FIGURAS/basic1-025_book_reading.png" />
                     &nbsp;&nbsp; <strong>ESCOLAS</strong></p>
                         <asp:LinkButton ID="LinkButton2" runat="server" class="pms_link">MAPA GERAL DAS ESCOLAS</asp:LinkButton>
                         <p class="auto-style4">Visualizar os dados principais das escolas</p>

                         <asp:LinkButton ID="LinkButtonTURMAS" runat="server" class="pms_link">MAPA GERAL DAS TURMAS</asp:LinkButton>
                         &nbsp;&nbsp;
                     <asp:Image ID="Image7" runat="server" ImageUrl="~/FIGURAS/basic2-239_brightness_sun.png" />
                     &nbsp;&nbsp;
                         <p class="auto-style4">Visualizar os dados principais das turmas</p>

                 </div>
             </td>
             <td style="width: 40px;"></td>
             <td valign:"top" >
                 <div class="rcorners2">
                     <p>
                     <asp:Image ID="Image4" runat="server" ImageUrl="~/FIGURAS/basic1-011_calendar.png" />
                     &nbsp;&nbsp; <strong>AGENDAMENTOS</strong></p>
                         <asp:LinkButton ID="LinkMapaGeralAplicacoes" runat="server" class="pms_link">MAPA GERAL DAS APLICAÇÕES</asp:LinkButton>
                         <p class="auto-style4">Analisar os agendamentos por polo, escola, município e aplicador</p>
                         <asp:LinkButton ID="LinkAgendamentoEscolas" runat="server" class="pms_link">AGENDAMENTO DE ESCOLAS</asp:LinkButton>
                         <p class="auto-style4">Agendamento de turmas por escola</p>
                         <asp:LinkButton ID="LinkRegistro" runat="server" class="pms_link">REGISTRO DE APLICAÇÕES</asp:LinkButton>
                         <p class="auto-style4">Registro de realização das aplicações</p>

                 </div>
             </td>
        </tr>
     </table>

     </asp:Panel>

    <asp:Panel ID="Panel2" runat="server" Height="563px">

    <table>
         <tr>
             <td valign:"top" >
                 <div class="rcorners2">
                     <p>
                     <asp:Image ID="Image5" runat="server" ImageUrl="~/FIGURAS/basic3-111_box_archive_documents_files.png" />
                     &nbsp;&nbsp; <strong>PARTICULAR</strong></p>
                     <asp:LinkButton ID="LinkMinhasAplicacoes" runat="server" class="pms_link">MINHAS APLICAÇÕES</asp:LinkButton>
                     <p class="auto-style4">Visualizar o agendamento das minhas turmas</p>
                 </div>
             </td>
             <td style="width: 40px;"></td>
             <td valign:"top" >
                 
             </td>
        </tr>

     </table>

     </asp:Panel>

    <br />

</asp:Content>
