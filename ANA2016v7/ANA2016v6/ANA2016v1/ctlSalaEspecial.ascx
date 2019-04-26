<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ctlSalaEspecial.ascx.vb" Inherits="ANA2016v1.ctlSalaEspecial" %>


<div class="container">

    <div class="row" style="vertical-align:middle;background-color:#f1f6f7;min-height: 40px;vertical-align: middle;padding-top: 10px;">


        <div class="col-md-8">
           <strong> <asp:Label ID="lblTitulo" runat="server"></asp:Label> 
            <br />
            </strong>

        </div>

    
        <div class="col-md-4 pull-right" style="padding-right:5px;"><asp:DropDownList CssClass="cmbColaborador" ID="cmbColaborador" runat="server" Width="100%">        
            </asp:DropDownList> </div>



    </div>

    <div class="row" style="border-bottom:1px solid #dddfe6;margin-bottom:5px;">
        
        <div class="col-md-12">
            <strong>Alunos: <asp:Label ID="lblQtdAlunos" runat="server"></asp:Label></strong>
            <asp:Label ID="lblAluno" runat="server"></asp:Label>

        </div>

    </div>
</div>