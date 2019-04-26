<%@ Page Title="Log in" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Login.aspx.vb" Inherits="ANA2016v1.Login" Async="true" %>



<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">  


    <div class="w3-main"   style="margin-left:30px;margin-top:20px; margin-bottom: 0px;">

            <div class="accountInfo" style="width: 870px">
                <asp:Literal ID="MessageError" runat="server" />
                <br />
                <h4>Bem-vindo ao portal FGV - ANA 2016</h4>
                <br />
                <table style="width:868px; height: 338px;">
                    <tr>
                        <td vertical-align: top; style="width: 106px">
                            <asp:Image ID="Image1" runat="server" Height="45px" ImageUrl="~/FIGURAS/Numero1comSeta.png" Width="104px" />
                        </td>

                        <td vertical-align: top; style="width: 180px">
                            Preencha seu CPF e...
                            <asp:TextBox ID="UserName" runat="server" CssClass="textEntry" MaxLength="11" Width="153px"></asp:TextBox>
                            <br />
                        </td>

                        <td style="width: 31px"></td>

                        <td  style="width: 58px">
                            <asp:Image ID="Image2" runat="server" ImageUrl="~/FIGURAS/basic2-024_key.png" />
                        </td>

                        <td vertical-align: top; style="width: 358px">
                            Digite sua senha e clique aqui se já estiver cadastrado<br />
                        
                        <asp:TextBox ID="Password" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                        
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="cmdEntrar"  runat="server" BackColor="#16B665" BorderStyle="None" Text="Entrar" OnClick="cmdEntrar_Click" Width="136px" ForeColor="White" />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 106px; height: 86px;">
                            <p style="width: 113px"> </p>
                            <p style="width: 113px"> </p>
                        </td>

                        <td style="height: 86px">
                            </td>
                        
                        <td style="height: 86px; width: 31px;"></td>
                        <td style="width: 58px; height: 86px">
                            <asp:Image ID="Image3" runat="server" ImageUrl="~/FIGURAS/basic2-175_light_bulb_on.png" />
                        </td>

                        <td style="height: 86px">
                            <asp:LinkButton ID="LinkButton1" runat="server">Clique aqui se precisar de uma nova senha</asp:LinkButton>
                        </td>


                    </tr>
                    <tr>
                        <td style="width: 106px; height: 86px;">
                        </td>

                        <td style="height: 86px">
                        </td>

                        <td style="height: 86px; width: 31px;"></td>
                        <td style="width: 58px; height: 86px">
                            <asp:Image ID="Image4" runat="server" ImageUrl="~/FIGURAS/basic2-257_personal_id_photo.png" />
                        </td>

                        <td style="height: 86px; font-family: Arial; font-size: small;">
                            Se você reside no estado do AC, AM, BA, DF, GO, MA, MS, PA, PI, RO ou RS, é professor e tem experiência em processos de avaliação educacional, clique abaixo para fazer seu cadastro e participar do FGV ANA 2016.</td>
                    </tr>
                    <tr>
                        <td style="width: 106px; height: 60px;">
                        </td>

                        <td style="height: 50px">
                        </td>
                        <td>
                        </td>
                        <td style="width: 58px; height: 50px">
                        </td>
                        <td style="height: 60px">
                            <asp:Button ID="cmdNovoCadastro" runat="server"  BackColor="#16B665" BorderStyle="None" Text="Quero fazer um novo cadastro" OnClick="cmdNovoCadastro_Click" Width="277px" ForeColor="White" UseSubmitBehavior="False" />
                            <br />
                            <br />
                        </td>
                    </tr>
                </table>
            </div>
</div>

</asp:Content>


