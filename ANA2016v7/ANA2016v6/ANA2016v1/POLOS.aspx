<%@ Page Title="" Language="vb" AutoEventWireup="false" EnableViewState="False" EnableEventValidation="False" MasterPageFile="~/Site.Master" CodeBehind="POLOS.aspx.vb" Inherits="ANA2016v1.POLOS" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">

function PMS(queCombo)
{
    var selectedOption
    switch(queCombo) {
        case "U":
            selectedOption = $("#MainContent_cmbUF option:selected").text();
            break;
        case "M":
            selectedOption = $("#MainContent_cmbMunicipio option:selected").text();
            break;
        case "P":
            selectedOption = $("#MainContent_cmbPolo option:selected").text();
            break;
        case "E":
            selectedOption = $("#MainContent_cmbEscola option:selected").text();
            break;
        case "S":
            selectedOption = $("#MainContent_cmbStatus option:selected").text();
            break;
        case "C":
            selectedOption = $("#MainContent_cmbCategoria option:selected").text();
            break;
        case "F":
            selectedOption = $("#MainContent_cmbFuncao option:selected").text();
            break;
        case "A":
            selectedOption = "";
            break;
        case "R":
            selectedOption = "";
            break;
        case "PG":
            selectedOption = "";
            break;
        case "B":
            selectedOption = "";
            //selectedOption = "OK"
            break;
        default:
    }
    var OptionBusca = document.getElementById('<%= CampoFiltroBusca.ClientID %>').value;
    var OptionPaginacao =  $("#MainContent_cmbPaginas option:selected").text();
    OptionBusca = OptionBusca.replace(/;/g, '');
    //alert("Filtro" + ";" + queCombo + ";" + selectedOption + ";" + OptionBusca);
    __doPostBack("@Filtro" + ";" + queCombo + ";" + selectedOption + ";" + OptionBusca + ";" + OptionPaginacao, '')
}
function PMS2(queEvento,queTurma)
{
    var OptionPaginacao =  $("#MainContent_cmbPaginas option:selected").text();
    //alert(queEvento + " " + queTurma + " " + OptionPaginacao);
    __doPostBack('@' + queEvento + ';' + queTurma + ';' + OptionPaginacao, '')
}
function PMS3(queEvento,queTexto)
{
    var r = confirm(queTexto);
    if (r == true) {
        __doPostBack(queEvento, '');
        return True;
    } else {
        return False;
    }
}

</script>

    <style>
table {
    width:100%;
}
table, th, td {
    border: none;
    border-collapse: collapse;
    font-family:'Trebuchet MS', 'Lucida Sans Unicode', 'Lucida Grande', 'Lucida Sans', Arial, sans-serif;
    font-size:smaller  
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
        .auto-style1 {
            font-size: x-small;
        }
    </style>

           <div class="navbar navbar-default navbar-fixed-top">
               
                <asp:Panel ID="PainelBuscas" runat="server" Visible="true">
                    <asp:Image Height="25px" width="40px" ID="Bandeirinha" runat="server" BorderStyle="None" BorderWidth="0px" />

                    &nbsp;&nbsp;
                    <asp:Label ID="LabelUF" class="meulabel" runat="server" Width="20px">UF</asp:Label>
                    <asp:DropDownList ID="cmbUF" runat="server" onchange="javascript: PMS('U')" Width="70px" ToolTip="Filtro por estado" Font-Size="Small">
                    <asp:ListItem>Todos</asp:ListItem></asp:DropDownList>

                    &nbsp;&nbsp;
                    <asp:Label ID="LabelMunicipio" runat="server"  Width="50px" style="font-family: Arial; " CssClass="auto-style1">Município</asp:Label>
                    <asp:DropDownList ID="cmbMunicipio" runat="server" onchange="javascript: PMS('M')" Width="180px" ToolTip="Filtro por município" Font-Size="Small">
                    <asp:ListItem>Todos</asp:ListItem></asp:DropDownList>

                    &nbsp;&nbsp;                    
                    <asp:Label ID="Label7" class="meulabel" runat="server" Width="40px">Busca</asp:Label>
                    <asp:TextBox ID="CampoFiltroBusca" runat="server" BorderStyle="Solid" BorderWidth="1px" Width="140px" ToolTip="Texto de busca"></asp:TextBox>
                    &nbsp;&nbsp;<asp:ImageButton ID="cmdBuscar" runat="server" OnClientClick="javascript: PMS('B')" BorderStyle="None" ImageUrl="~/FIGURAS/basic1-015_search_zoom_find.png" ToolTip="Clique aqui para realizar a busca" />

                    &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="cmdVoltar" runat="server" BackColor="#16B665" Text="Voltar para minha página" Width="200px" ForeColor="White" Height="27px" BorderWidth="0px" UseSubmitBehavior="False" />

                    &nbsp;&nbsp;  
                                      
                     &nbsp;&nbsp;<asp:ImageButton ID="cmRetroceder" runat="server" OnClientClick="javascript: PMS('R')" BorderStyle="None" ImageUrl="~/FIGURAS/basic2-121_media_back.png" ToolTip="Clique aqui exibir a página anterior" />
                    &nbsp;<asp:DropDownList ID="cmbPaginas" runat="server" onchange="javascript: PMS('PG')" BackColor="White" ToolTip="Filtro por município" Font-Size="Small">
                    </asp:DropDownList>
                    <asp:ImageButton ID="cmdAvancar" runat="server" OnClientClick="javascript: PMS('A')" BorderStyle="None" ImageUrl="~/FIGURAS/basic2-124_media_play.png" ToolTip="Clique aqui para exibir a página seguinte" />
                
                </asp:Panel>
                <asp:Panel ID="Panel2" runat="server" Visible="true">
                    <asp:Label ID="Label6" class="meulabel" runat="server" Width="40px"></asp:Label>

                    &nbsp;&nbsp;
                    <asp:Label ID="LabelPolo"  class="meulabel" runat="server" Width="30px">Polo</asp:Label>
                    <asp:DropDownList ID="cmbPolo" runat="server" onchange="javascript: PMS('P')" Width="200px" ToolTip="Filtro por polo" Font-Size="Small">
                    <asp:ListItem>Todos</asp:ListItem></asp:DropDownList>

                    &nbsp;&nbsp;
                    <asp:Label ID="LabelEscola" class="meulabel" runat="server" Width="40px">Escola</asp:Label>
                    <asp:DropDownList ID="cmbEscola" runat="server" onchange="javascript: PMS('E')" Width="200px" ToolTip="Filtro por escola" Font-Size="Small">
                    <asp:ListItem>Todos</asp:ListItem></asp:DropDownList>
                    &nbsp;&nbsp;
                    <asp:Label ID="LabelFuncao" class="meulabel" runat="server" Width="40px">Função</asp:Label>
                    <asp:DropDownList ID="cmbFuncao" runat="server" onchange="javascript: PMS('F')" Width="120px" ToolTip="Filtro por função" Font-Size="Small">
                    <asp:ListItem>Todas</asp:ListItem>
                        <asp:ListItem>Coordenador Estadual</asp:ListItem>
                        <asp:ListItem>Subcoordenador Estadual</asp:ListItem>
                        <asp:ListItem>Coordenador de Polo</asp:ListItem>
                        <asp:ListItem>Apoio Logístico</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;&nbsp;
                    <asp:Label ID="LabelStatus" class="meulabel" runat="server" Width="40px">Status</asp:Label>
                    <asp:DropDownList ID="cmbStatus" runat="server" onchange="javascript: PMS('S')" Width="120px" ToolTip="Filtro por status" Font-Size="Small">
                    <asp:ListItem>Todos</asp:ListItem>
                        <asp:ListItem>Pendente</asp:ListItem>
                        <asp:ListItem>Apto</asp:ListItem>
                        <asp:ListItem>Reserva</asp:ListItem>
                    </asp:DropDownList>                  
                    
                    &nbsp;&nbsp;
                    <asp:Label ID="LabelCategoria" class="meulabel" runat="server" Width="56px">Categoria</asp:Label>
                    <asp:DropDownList ID="cmbCategoria" runat="server" onchange="javascript: PMS('C')" Width="120px" ToolTip="Filtro por categoria" Font-Size="Small">
                        <asp:ListItem>Todas</asp:ListItem>
                        <asp:ListItem>Aplicador</asp:ListItem>
                        <asp:ListItem>Gestor</asp:ListItem>
                    </asp:DropDownList>
                </asp:Panel>
            </div>

      <div>
            <asp:Literal ID="MensagemERROS" runat="server"  />
            <br/>
            <h6><asp:Literal ID="LiteralNumLinhas" runat="server"></asp:Literal></h6>
            <asp:TextBox ID="CaixaRemocao" runat="server" Visible="False" Width="115px"></asp:TextBox>
            <h4><asp:Literal ID="Titulo" runat="server"  /></h4>
            <asp:Table ID="TabelaPolos" runat="server" EnableViewState="False" ViewStateMode="Disabled"
                border-collapse="collapse" GridLines="Horizontal">
            </asp:Table>
            <br />
      </div>

</asp:Content>
