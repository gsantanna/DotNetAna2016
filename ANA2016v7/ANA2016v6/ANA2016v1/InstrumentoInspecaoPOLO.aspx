<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="InstrumentoInspecaoPOLO.aspx.vb" Inherits="ANA2016v1.InstrumentoInspecaoPOLO" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>


        .textEntry {

            text-align:center;


        }
.rcorners2 {
    border-radius: 5px;
    border: 1px solid #1c2f67;
    padding: 15px;
    width: 900px;
    height: 650px;
}
.meulabel {
    font-family: Arial;
    font-size: x-small;
        margin-bottom: 0px;
        font-weight: 680;
    }
.meucampo {
    font-family: Arial;
    font-size: x-small;
}
        .auto-style1 {
            font-family: Arial;
            font-size: small;
        }
        .auto-style2 {
            font-family: Arial;
            font-variant: small-caps;
        }
        </style>
    <asp:Literal ID="MensagemERRO" runat="server" />
    <h5>&nbsp;<asp:TextBox ID="CampoPOLO" runat="server" CssClass="textEntry" MaxLength="4" Width="483px" style="font-family: Arial; font-size: large" ReadOnly="True" Visible="False"></asp:TextBox>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp; </h5>
    <div class="rcorners2" runat="server" id="divcontroles" >
        <span class="auto-style2">1. Características da localização do Polo de Apoio Logístico</span><br />
        <br />
        <asp:Label ID="PasswordLabel24" runat="server"  class="meulabel" Width="682px">1.1  O Polo será instalado em local que atente para a disposição das escolas abrangidas de forma a facilitar a logística diária de aplicação?</asp:Label>
        <asp:DropDownList ID="cmbConfirmados1" runat="server" CssClass="auto-style1" Width="60px" Height="16px">
            <asp:ListItem> </asp:ListItem>
            <asp:ListItem>Sim</asp:ListItem>
            <asp:ListItem>Não</asp:ListItem>
        </asp:DropDownList>
        <br />
        <asp:Label ID="PasswordLabel25" runat="server"  class="meulabel" Width="682px">1.2  O Polo será bem localizado em função dos transportes oferecidos?</asp:Label>
        <asp:DropDownList ID="cmbConfirmados2" runat="server" CssClass="auto-style1" Width="60px" Height="16px">
            <asp:ListItem> </asp:ListItem>
            <asp:ListItem>Sim</asp:ListItem>
            <asp:ListItem>Não</asp:ListItem>
        </asp:DropDownList>
        <br />
        <asp:Label ID="PasswordLabel26" runat="server"  class="meulabel" Width="682px">1.3  A localização do Polo favorecerá a segurança dos aplicadores para o transporte das provas e devolução diária dos materiais aplicados?</asp:Label>
        <asp:DropDownList ID="cmbConfirmados3" runat="server" CssClass="auto-style1" Width="60px" Height="16px">
            <asp:ListItem> </asp:ListItem>
            <asp:ListItem>Sim</asp:ListItem>
            <asp:ListItem>Não</asp:ListItem>
        </asp:DropDownList>
        <br />
        <asp:Label ID="PasswordLabel27" runat="server"  class="meulabel" Width="682px">1.4  O local que sediará o Polo oferecerá segurança para o transporte e entrega das provas pela ECT?</asp:Label>
        <asp:DropDownList ID="cmbConfirmados4" runat="server" CssClass="auto-style1" Width="60px" Height="16px">
            <asp:ListItem> </asp:ListItem>
            <asp:ListItem>Sim</asp:ListItem>
            <asp:ListItem>Não</asp:ListItem>
        </asp:DropDownList>
        <br />
        <asp:Label ID="PasswordLabel28" runat="server"  class="meulabel" Width="682px">1.5  A localização do Polo previu a proximidade e quantidade de aplicações?</asp:Label>
        <asp:DropDownList ID="cmbConfirmados5" runat="server" CssClass="auto-style1" Width="60px" Height="16px">
            <asp:ListItem> </asp:ListItem>
            <asp:ListItem>Sim</asp:ListItem>
            <asp:ListItem>Não</asp:ListItem>
        </asp:DropDownList>
        <br />
        <asp:Label ID="PasswordLabel29" runat="server"  class="meulabel" Width="682px">1.6  O Polo será bem localizado em função de acolher outros municípios, se for o caso?</asp:Label>
        <asp:DropDownList ID="cmbConfirmados6" runat="server" CssClass="auto-style1" Width="60px" Height="16px">
            <asp:ListItem> </asp:ListItem>
            <asp:ListItem>Sim</asp:ListItem>
            <asp:ListItem>Não</asp:ListItem>
        </asp:DropDownList>
        <br />
        <asp:Label ID="PasswordLabel30" runat="server"  class="meulabel" Width="682px">1.7  As imediações do local que receberá o Polo são urbanizadas e possuem iluminação artificial?</asp:Label>
        <asp:DropDownList ID="cmbConfirmados7" runat="server" CssClass="auto-style1" Width="60px" Height="16px">
            <asp:ListItem> </asp:ListItem>
            <asp:ListItem>Sim</asp:ListItem>
            <asp:ListItem>Não</asp:ListItem>
        </asp:DropDownList>
        <br />
        <asp:Label ID="PasswordLabel31" runat="server"  class="meulabel" Width="682px">1.8  As imediações do local que receberá o Polo são consideradas seguras (sem violência)?</asp:Label>
        <asp:DropDownList ID="cmbConfirmados8" runat="server" CssClass="auto-style1" Width="60px" Height="16px">
            <asp:ListItem> </asp:ListItem>
            <asp:ListItem>Sim</asp:ListItem>
            <asp:ListItem>Não</asp:ListItem>
        </asp:DropDownList>
        <br />
        <br />
        <span class="auto-style2">2. Características da infraestrutura do Polo de Apoio Logístico</span><br />
        <br />
        <asp:Label ID="PasswordLabel32" runat="server"  class="meulabel" Width="682px">2.1  O local que sediará o Polo possui espaço suficiente para os dois ambientes?</asp:Label>
        <asp:DropDownList ID="cmbConfirmados9" runat="server" CssClass="auto-style1" Width="60px" Height="16px">
            <asp:ListItem> </asp:ListItem>
            <asp:ListItem>Sim</asp:ListItem>
            <asp:ListItem>Não</asp:ListItem>
        </asp:DropDownList>
        <br />
        <asp:Label ID="PasswordLabel33" runat="server"  class="meulabel" Width="682px">2.2  O espaço do Polo será satisfatório e seguro para armazenamento e manuseio do material de aplicação e para atividades administrativas e de logística?</asp:Label>
        <asp:DropDownList ID="cmbConfirmados10" runat="server" CssClass="auto-style1" Width="60px" Height="16px">
            <asp:ListItem> </asp:ListItem>
            <asp:ListItem>Sim</asp:ListItem>
            <asp:ListItem>Não</asp:ListItem>
        </asp:DropDownList>
        <br />
        <asp:Label ID="PasswordLabel34" runat="server"  class="meulabel" Width="682px">2.3  O local de funcionamento do Polo possui os equipamentos, insumos e materiais necessários para seu funcionamento?</asp:Label>
        <asp:DropDownList ID="cmbConfirmados11" runat="server" CssClass="auto-style1" Width="60px" Height="16px">
            <asp:ListItem> </asp:ListItem>
            <asp:ListItem>Sim</asp:ListItem>
            <asp:ListItem>Não</asp:ListItem>
        </asp:DropDownList>
        <br />
        <asp:Label ID="PasswordLabel35" runat="server"  class="meulabel" Width="682px">2.4  O ambiente que irá acolher o Polo possui iluminação e ventilação adequadas às atividades?</asp:Label>
        <asp:DropDownList ID="cmbConfirmados12" runat="server" CssClass="auto-style1" Width="60px" Height="16px">
            <asp:ListItem> </asp:ListItem>
            <asp:ListItem>Sim</asp:ListItem>
            <asp:ListItem>Não</asp:ListItem>
        </asp:DropDownList>
        <br />
        <asp:Label ID="PasswordLabel36" runat="server"  class="meulabel" Width="682px">2.5  O local que irá acolher o Polo possui instalações elétricas adequadas aos equipamentos necessários ao desenvolvimento das atividades?</asp:Label>
        <asp:DropDownList ID="cmbConfirmados13" runat="server" CssClass="auto-style1" Width="60px" Height="16px">
            <asp:ListItem> </asp:ListItem>
            <asp:ListItem>Sim</asp:ListItem>
            <asp:ListItem>Não</asp:ListItem>
        </asp:DropDownList>
        <br />
        <asp:Label ID="PasswordLabel37" runat="server"  class="meulabel" Width="682px">2.6  As portas e janelas da edificação na qual será instalado o Polo permitem bloqueio que impeça o acesso ao interior do local?</asp:Label>
        <asp:DropDownList ID="cmbConfirmados14" runat="server" CssClass="auto-style1" Width="60px" Height="16px">
            <asp:ListItem> </asp:ListItem>
            <asp:ListItem>Sim</asp:ListItem>
            <asp:ListItem>Não</asp:ListItem>
        </asp:DropDownList>
        <br />
        <asp:Label ID="PasswordLabel38" runat="server"  class="meulabel" Width="682px">2.7  A entra do local que será ocupado pelo Polo possui acesso direto a áreas de livre circulação de pessoas?</asp:Label>
        <asp:DropDownList ID="cmbConfirmados15" runat="server" CssClass="auto-style1" Width="60px" Height="16px">
            <asp:ListItem> </asp:ListItem>
            <asp:ListItem>Sim</asp:ListItem>
            <asp:ListItem>Não</asp:ListItem>
        </asp:DropDownList>
        <br />
        <asp:Label ID="PasswordLabel39" runat="server"  class="meulabel" Width="682px">2.8  O espaço do Polo destinado às atividades de agendamento das aplicações e seleção dos aplicadores é reservado?</asp:Label>
        <asp:DropDownList ID="cmbConfirmados16" runat="server" CssClass="auto-style1" Width="60px" Height="16px">
            <asp:ListItem> </asp:ListItem>
            <asp:ListItem>Sim</asp:ListItem>
            <asp:ListItem>Não</asp:ListItem>
        </asp:DropDownList>
        <br />
        <br />
        <br />
        <asp:Label ID="PasswordLabel40" runat="server"  class="meulabel" Width="682px">Localizaçao do arquivo contendo a imagem do formulário preenchido e assinado pelo Coordenador do Polo</asp:Label>
        <asp:FileUpload ID="FileUpload1" runat="server" ToolTip="Clique aqui para localizar o arquivo contendo a imagem do formulári opreenchido e assinado pelo Coordenador do Polo" Width="742px" />
        <br />
        <br />&nbsp;&nbsp;
                    <asp:Label ID="UserNameLabel28" runat="server"  class="meulabel" Width="445px" Height="16px"></asp:Label>
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="cmdEntrar" runat="server" BackColor="#16B665" BorderStyle="None" Text="Confirmar e salvar" Width="156px" ForeColor="White" ToolTip="Clique aqui para validar os dados e salvar" UseSubmitBehavior="False" BorderWidth="0px" Height="36px" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="cmdCancelar" runat="server" BackColor="White" BorderStyle="Double" Text="Cancelar" Width="94px" ForeColor="Black" ToolTip="Clique aqui para abandonar o cadastramento" UseSubmitBehavior="False" />
        <br />
        <br />
        <br />
    </div>
</asp:Content>
