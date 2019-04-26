<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="PerguntasTurma.aspx.vb" Inherits="ANA2016v1.PerguntasTurma" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .textEntry {
            text-align: center;
        }

        .rcorners2 {
            border-radius: 5px;
            border: 1px solid #1c2f67;
            padding: 15px;
            width: 800px;
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
    </style>
    <asp:Literal ID="MensagemERRO" runat="server" />
    <h5>Turma:
        <asp:Literal ID="CampoTurma" runat="server"></asp:Literal>&nbsp;<asp:TextBox ID="CampoCO_CENSO_TURMA" runat="server" CssClass="textEntry" MaxLength="4" Width="85px" Style="font-family: Arial; font-size: large" ReadOnly="True" Visible="False"></asp:TextBox>
        &nbsp;
                           
                   

                            Total de alunos:
        <asp:Label ID="txtTotalTurma" runat="server">0</asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;&nbsp;
                
                           <div style="display:none">
                               <!-- esconde o combo original -->
                               <asp:DropDownList ID="cmbHorario" CssClass="cmbHorario1" runat="server" Height="43px" Width="524px">
                                   <asp:ListItem Value="0" Text="Escolha..."></asp:ListItem>
                               </asp:DropDownList>
                               <asp:DropDownList ID="cmbHorario2" CssClass="cmbHorario2" runat="server" Height="38px" Width="402px">
                                   <asp:ListItem Value="0" Text="Escolha..."></asp:ListItem>
                               </asp:DropDownList>
                           </div>
        &nbsp;<span id="lblErro" style="color: red; display: none;">Favor selecionar um horário</span>




    </h5>


    
    <table cellpadding="2">
        <tr>
            <td colspan="3">&nbsp;</td>
        </tr>
        <tr>
            <td> Período da aplicação: 
                <select id="cmbPeriodo1" class="cmbPeriodo" data-ctrl="1" style="height: 30px; width: 180px;">
                </select></td>
            <td>
                Horário de início dia 1: <select id="cmbHoras1" class="cmbHora" style="height: 30px; width: 84px;" data-ctrl="1">
                    <option value="0">Selecione um período</option>
                </select></td>

            <td> Horário de início dia 2:
                <select id="cmbHoras2" class="cmbHora" style="height: 30px; width: 84px;" data-ctrl="2">
                    <option value="0">Selecione um período</option>
                </select>

            </td>


        </tr>
                

    </table>

    <br /><br />




    <div class="rcorners2" runat="server" id="divcontroles">
        Quantidade de alunos comos com<br />
        <asp:Label ID="PasswordLabel33" runat="server" class="meulabel" Width="351px"></asp:Label>
        <asp:Label ID="UserNameLabel29" runat="server" class="meulabel" Width="139px" Height="16px" Font-Bold="True" Font-Underline="True">PREVISTOS NO CENSO</asp:Label>
        <asp:Label ID="UserNameLabel30" runat="server" class="meulabel" Width="95px" Font-Underline="True">CONFIRMADOS</asp:Label>
        <asp:Label ID="UserNameLabel31" runat="server" class="meulabel" Width="142px" Font-Underline="True">EXTRAS, NÃO PREVISTOS</asp:Label>
        <br />
        <br />
        <asp:Label ID="PasswordLabel24" runat="server" class="meulabel" Width="380px">CEGUEIRA e atendimento INDIVIDUAL ESPECIALIZADO (ledor)</asp:Label>
        &nbsp;&nbsp;<asp:TextBox ID="txtPrevistos1" runat="server" CssClass="textEntry" MaxLength="3" Width="30px" Style="font-family: Arial; font-size: small" ReadOnly="True"></asp:TextBox>
        <asp:Label ID="UserNameLabel11" runat="server" class="meulabel" Width="70px"></asp:Label>
        <asp:DropDownList ID="cmbConfirmados1" runat="server" CssClass="auto-style1" Width="60px" Height="16px">
        </asp:DropDownList>
        <asp:Label ID="UserNameLabel32" runat="server" class="meulabel" Width="63px"></asp:Label>
        <asp:DropDownList ID="cmbNaoPrev10" runat="server" CssClass="auto-style1" Width="60px" Height="16px">
        </asp:DropDownList>
        <br />
        <br />
        <asp:Label ID="PasswordLabel25" runat="server" class="meulabel" Width="380px">SURDEZ e atendimento INDIVIDUAL ESPECIALIZADO (intérprete libras)</asp:Label>
        &nbsp;
                        <asp:TextBox ID="txtPrevistos2" runat="server" CssClass="textEntry" MaxLength="3" Width="30px" Style="font-family: Arial; font-size: small" ReadOnly="True"></asp:TextBox>
        <asp:Label ID="UserNameLabel33" runat="server" class="meulabel" Width="70px"></asp:Label>
        <asp:DropDownList ID="cmbConfirmados2" runat="server" CssClass="auto-style1" Width="60px" Height="16px">
        </asp:DropDownList>
        <asp:Label ID="UserNameLabel34" runat="server" class="meulabel" Width="63px"></asp:Label>
        <asp:DropDownList ID="cmbNaoPrev9" runat="server" CssClass="auto-style1" Width="60px" Height="16px">
        </asp:DropDownList>
        &nbsp;&nbsp;&nbsp;<br />
        <br />
        <asp:Label ID="PasswordLabel26" runat="server" class="meulabel" Width="380px">SURDO_CEGUEIRA e atendimento INDIVIDUAL ESPECIALIZADO (guia-intérprete)</asp:Label>
        &nbsp;
                        <asp:TextBox ID="txtPrevistos3" runat="server" CssClass="textEntry" MaxLength="3" Width="30px" Style="font-family: Arial; font-size: small" ReadOnly="True"></asp:TextBox>
        <asp:Label ID="UserNameLabel35" runat="server" class="meulabel" Width="70px"></asp:Label>
        <asp:DropDownList ID="cmbConfirmados3" runat="server" CssClass="auto-style1" Width="60px" Height="16px">
        </asp:DropDownList>
        <asp:Label ID="UserNameLabel36" runat="server" class="meulabel" Width="63px"></asp:Label>
        <asp:DropDownList ID="cmbNaoPrev11" runat="server" CssClass="auto-style1" Width="60px" Height="16px">
        </asp:DropDownList>
        &nbsp;&nbsp;&nbsp;
                        <br />
        <br />
        <asp:Label ID="PasswordLabel27" runat="server" class="meulabel" Width="380px">BAIXA VISÃO e atendimento INDIVIDUAL COMUM (com leitor/transcritor da escola - prova ampliada/superampliada)</asp:Label>
        &nbsp;
                        <asp:TextBox ID="txtPrevistos4" runat="server" CssClass="textEntry" MaxLength="3" Width="30px" Style="font-family: Arial; font-size: small" ReadOnly="True"></asp:TextBox>
        <asp:Label ID="UserNameLabel37" runat="server" class="meulabel" Width="70px"></asp:Label>
        <asp:DropDownList ID="cmbConfirmados4" runat="server" CssClass="auto-style1" Width="60px" Height="16px">
        </asp:DropDownList>
        <asp:Label ID="UserNameLabel38" runat="server" class="meulabel" Width="63px"></asp:Label>
        &nbsp;&nbsp;&nbsp;
                        <br />
        <br />
        <asp:Label ID="PasswordLabel28" runat="server" class="meulabel" Width="380px">BAIXA VISÃO e atendimento COMUM em grupo de 10 (sem ledor/transcritor da escola - prova ampliada/superampliada)</asp:Label>
        &nbsp;
                        <asp:TextBox ID="txtPrevistos6" runat="server" CssClass="textEntry" MaxLength="3" Width="30px" Style="font-family: Arial; font-size: small" ReadOnly="True"></asp:TextBox>
        <asp:Label ID="UserNameLabel39" runat="server" class="meulabel" Width="70px"></asp:Label>
        <asp:DropDownList ID="cmbConfirmados6" runat="server" CssClass="auto-style1" Width="60px" Height="16px">
        </asp:DropDownList>
        <asp:Label ID="UserNameLabel40" runat="server" class="meulabel" Width="63px"></asp:Label>
        &nbsp;&nbsp;&nbsp;
                        <br />
        <br />
        <asp:Label ID="PasswordLabel29" runat="server" class="meulabel" Width="380px">NÃO PREVISTOS com BAIXA VISÃO (COM ledor/transcritor da escola)</asp:Label>
        &nbsp;
                        
                        
                        <asp:Label ID="UserNameLabel41" runat="server" class="meulabel" Width="104px"></asp:Label>
        <asp:Label ID="UserNameLabel42" runat="server" class="meulabel" Width="127px"></asp:Label>
        <asp:DropDownList ID="cmbNaoPrev4" runat="server" CssClass="auto-style1" Width="60px" Height="16px">
        </asp:DropDownList>
        &nbsp;&nbsp;&nbsp;
                        <br />
        <br />
        <asp:Label ID="PasswordLabel30" runat="server" class="meulabel" Width="380px">NÃO PREVISTOS com BAIXA VISÃO (SEM ledor/transcritor da escola)</asp:Label>
        &nbsp;
                        <asp:Label ID="UserNameLabel43" runat="server" class="meulabel" Width="104px"></asp:Label>
        <asp:Label ID="UserNameLabel44" runat="server" class="meulabel" Width="127px"></asp:Label>
        <asp:DropDownList ID="cmbNaoPrev6" runat="server" CssClass="auto-style1" Width="60px" Height="16px">
        </asp:DropDownList>
        &nbsp;&nbsp;&nbsp;
                        <br />
        <br />
        <asp:Label ID="PasswordLabel31" runat="server" class="meulabel" Width="380px">OUTRAS DEFICIÊNCIAS/TRANSTORNOS (COM ledor/transcritor da escola)</asp:Label>
        &nbsp;
                        <asp:TextBox ID="txtPrevistos5" runat="server" CssClass="textEntry" MaxLength="3" Width="30px" Style="font-family: Arial; font-size: small" ReadOnly="True"></asp:TextBox>
        <asp:Label ID="UserNameLabel45" runat="server" class="meulabel" Width="70px"></asp:Label>
        <asp:DropDownList ID="cmbConfirmados5" runat="server" CssClass="auto-style1" Width="60px" Height="16px">
        </asp:DropDownList>
        <asp:Label ID="UserNameLabel46" runat="server" class="meulabel" Width="63px"></asp:Label>
        <asp:DropDownList ID="cmbNaoPrev5" runat="server" CssClass="auto-style1" Width="60px" Height="16px">
        </asp:DropDownList>
        &nbsp;&nbsp;&nbsp;
                        <br />
        <br />
        <asp:Label ID="PasswordLabel32" runat="server" class="meulabel" Width="380px">OUTRAS DEFICIÊNCIAS/TRANSTORNOS (SEM ledor/transcritor da escola)</asp:Label>
        &nbsp;
                        <asp:TextBox ID="txtPrevistos7" runat="server" CssClass="textEntry" MaxLength="3" Width="30px" Style="font-family: Arial; font-size: small" ReadOnly="True"></asp:TextBox>
        <asp:Label ID="UserNameLabel47" runat="server" class="meulabel" Width="70px"></asp:Label>
        <asp:DropDownList ID="cmbConfirmados7" runat="server" CssClass="auto-style1" Width="60px" Height="16px">
        </asp:DropDownList>
        <asp:Label ID="UserNameLabel48" runat="server" class="meulabel" Width="63px"></asp:Label>
        <asp:DropDownList ID="cmbNaoPrev7" runat="server" CssClass="auto-style1" Width="60px" Height="16px">
        </asp:DropDownList>
        &nbsp;&nbsp;&nbsp;
                        <br />
        <br />
        <br />
        &nbsp;&nbsp;
                    <asp:Label ID="UserNameLabel28" runat="server" class="meulabel" Width="320px" Height="16px"></asp:Label>
        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="cmdEntrar" runat="server" BackColor="#16B665" BorderStyle="None" Text="Confirmar e salvar" Width="156px" ForeColor="White" CssClass="btn-entrar" ToolTip="Clique aqui para validar os dados e salvar" UseSubmitBehavior="False"
                            BorderWidth="0px" Height="36px" OnClientClick="if( !valida()) return false;                " />
        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="cmdCancelar" runat="server" BackColor="White" BorderStyle="Double" Text="Cancelar" OnClick="cmdCancelar_Click" Width="94px" ForeColor="Black" ToolTip="Clique aqui para abandonar o cadastramento" UseSubmitBehavior="False" />
        <br />
        <br />
        <asp:Label ID="LabelIncAlt" runat="server" Width="370px" Style="font-family: Arial; font-size: x-small;" BackColor="White" ForeColor="#1C2F67"></asp:Label>
    </div>



    <script type="text/javascript">


        function valida() {

            //avisa o usuário caso a turma já esteja confirmada.
            if ($(".btn-entrar").attr("data-conf")) {
                if (!confirm("Deseja mesmo salvar este item? As seleções de aplicadores precisarão ser realizadas novamente.")) return false;
            } 


            if ($("#MainContent_cmbHorario").val() == "0" || $("#MainContent_cmbHorario2").val() == "0"  ) {
                $("#lblErro").show();
                return false;
            } else {
                $("#lblErro").hide();
                return true;
            }
        }


        $(document).ready(function () { //somente é executado após o carregamento total da página



            //encontra o combo original  só para pegar os valores (domínio)
            var opcoes = $("#MainContent_cmbHorario option");

            //gera uma lista distinta de períodos
            var periodos = $.map(opcoes, function (o) {
                return $.trim($(o).html().split("-")[0]) ;
            }).unique();

            
            //Preenche os combos de Períodos
            $(periodos).each(function (i, opt) { //para cada período...
                $("#cmbPeriodo1").append("<option value='" + opt + "'>" + opt + "</option>")
            })

            


            

            //adiciona o evento (onchange) no combo de períodos. (que vai carregar o segundo combo)
            $(".cmbPeriodo").change(function () {

                //identifica o respectivo combo de horas 
                var comboPeriodos = $("#cmbPeriodo" + $(this).attr("data-ctrl"));
                var comboDotNet = $(".cmbHorario" + $(this).attr("data-ctrl"));
                
                //Limpa os horários..
                $(".cmbHora").empty().empty();

                //Adiciona o 'selecione...
                $(".cmbHora").append("<option value='0'>Escolha...</option>");
                                
                //monta a lista de itens que devem ser exibidos no combo da direita (horarios)
                var horarios = $.map(
                        $("#MainContent_cmbHorario option:contains('" + $(comboPeriodos).val() + "')"), //seletor de todos os itens que contém o texto da esquerda
                        function (o) { return { id: $(o).val(), texto: $.trim($(o).html().split('-')[1]) } });  //retorna um array de objetos contendo o ID dos horários e o texto (somente hora split..[1] ) 

                //loopa a lista de horarios inserindo os itens no combo novo 
                $(horarios).each(function (indice, objeto) {
                    $(".cmbHora").append("<option value='" + objeto.id + "'>" + objeto.texto  + "</option>");
                });

                $(".cmbHora").trigger("change"); //aciona o "change" do combo horas

            }) //fim da função change do combo de peíodos
            




            $(".cmbHora").change(function () { //sincroniza o valor do combo de horas com o combo final .net que está oculto na página

                var comboDotNet = $(".cmbHorario" + $(this).attr("data-ctrl"));
                $(comboDotNet).val($(this).val());
                //$("#MainContent_cmbHorario").val($("#cmbHoras option:selected").val());

            })

            


            //Seta os valores originais (vindos do banco)
            var periodo = $.trim($(".cmbHorario1 option:selected").html().split("-")[0]);
            var horario1 = $(".cmbHorario1").val();
            var horario2 = $(".cmbHorario2").val();

            $(".cmbPeriodo").val(periodo).trigger("change");

            $("#cmbHoras1").val(horario1).trigger("change");
            $("#cmbHoras2").val(horario2).trigger("change");          




        })





        //função para remover duplicados no array
        Array.prototype.unique = function () {
            var a = [];
            for (i = 0; i < this.length; i++) {
                var current = this[i];
                if (a.indexOf(current) < 0) a.push(current);
            }
            return a;
        }




    </script>



</asp:Content>
