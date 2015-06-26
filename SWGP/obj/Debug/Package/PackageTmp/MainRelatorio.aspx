<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MainRelatorio.aspx.cs" Inherits="SWGP.RelAnalitico" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h3>|>> Relatórios</h3>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hidItem" />
            <fieldset>
                <asp:Button ID="btnGeraRelatorio" Text="Gerar" runat="server" CssClass="DkoButtonExport" OnClick="btnGeraRelatorio_Click" />
                <input type="reset" name="name" value="Limpar" class="DkoButtonClean" />
                <input type="button" value="Voltar" onclick="history.go(-1)" class="DkoButtonBack"
                    id="btnVoltar">
            </fieldset>
            <fieldset>
                <legend>Filtro</legend>
                <table class="DkoFormView">
                    <tr class="DkoFormViewRow">
                        <td class="DkoFormViewLabel">
                            <asp:Label Text="Tipo de Relatório" ID="lblTipoRelatorio" runat="server" />
                        </td>
                        <td class="DkoFormViewField" colspan="2">
                            <asp:DropDownList runat="server" ID="ddlTipoRelatorio" >
                                 <asp:ListItem Text="Selecione..." ></asp:ListItem>
                                 <asp:ListItem Text="Sintético" ></asp:ListItem>
                                 <asp:ListItem Text="Analítico" ></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                         <td class="DkoFormViewLabel">
                            <asp:Label Text="Período" ID="lblPeriodo" runat="server" />
                        </td>
                        <td  nowrap="nowrap" colspan="3">
                            <asp:TextBox runat="server" id="txtDtInicio" Width="34%"/>
                                <asp:MaskedEditExtender ID="meeDtInicio" runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="txtDtInicio" >
                             </asp:MaskedEditExtender>
                            <asp:Button runat="server" ID="btnCalendar1" CssClass="DkoButtonCalendar" />
                                <asp:CalendarExtender ID="CalendarExtender3" runat="server" PopupButtonID="btnCalendar1" TargetControlID="txtDtInicio">
                                </asp:CalendarExtender>
                            <span>até</span>  
                            <asp:TextBox runat="server" id="txtDtFim" Width="33%"/> 
                                 <asp:MaskedEditExtender ID="meeDtFim" runat="server" Mask="99/99/9999" MaskType="Date"  TargetControlID="txtDtFim" >
                                </asp:MaskedEditExtender>
                             <asp:Button  ID="btnCalendar2" runat="server" CssClass="DkoButtonCalendar"/> 
                                <asp:CalendarExtender ID="CalendarExtender4" runat="server" PopupButtonID="btnCalendar2" TargetControlID="txtDtFim">
                                </asp:CalendarExtender>
                         </td>

                    </tr>
                    <tr class="DkoFormViewRow">
                        <td class="DkoFormViewLabel">
                           <asp:Label Text="UA" ID="lblUA" runat="server" />
                        </td>
                         <td colspan="2">
                            <asp:DropDownList runat="server" ID="ddlUA" Width="94%"  OnSelectedIndexChanged="ddlUA_OnSelectedIndexChanged" AutoPostBack="true"
                              DataTextField="Nome" DataValueField="idUa">
                                <asp:ListItem Text="Selecione..." Enabled="true" Selected="True" Value="1"/>
                            </asp:DropDownList>
                        </td>
                        <td class="DkoFormViewLabel">
                           <asp:Label Text="Usuário" ID="lblUsuarioDestino" runat="server" />
                        </td>
                         <td colspan="2">
                            <asp:DropDownList runat="server" ID="ddlUsuario" Width="100%" DataTextField="UserName" DataValueField="idUsuario" />
                         </td>
                       
                        <td style="margin-left:25px;" colspan="2">
                            <asp:CheckBox Text="        Todos" runat="server" id="cbTotUsuarios" Checked="false"/>
                        </td>
                        
                    </tr>
                </table>
            </fieldset>

            <asp:ObjectDataSource ID="odsDdlNomesUsuario" runat="server" 
               SelectMethod="GetAll" TypeName="SWGPgen.UsuarioControl"></asp:ObjectDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
