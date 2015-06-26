<%@ Page Title="" Language="C#" MasterPageFile="~/Lookpup.Master" AutoEventWireup="true"
    CodeBehind="NewContato.aspx.cs" Inherits="SWGP.NewContato" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="barModals">
        <p>|>> Cadastro de Contato</p>
    </div>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hfEditaContato" runat="server" Value="0" /> 
            <fieldset>
                <asp:Button Text="Salvar" runat="server" id="btnSalvar" CssClass="DkoButtonSave"
                    OnClick="btnSalvar_Click" />
                <input type="reset" name="name" value="Limpar" class="DkoButtonClean" />
                <%--<input type="button" value="Voltar" onclick="javascript:window.close();" class="DkoButtonBack" id="btnVoltar" runat="server"/>--%>
                <%--<asp:Button Text="Voltar" id="btnVoltarNovoContato" CssClass="DkoButtonBack" runat="server" />--%>
            </fieldset>
            <fieldset>
                <legend>Dados do Contato</legend>
                <table class="DkoFormView">
                    <tr class="DkoFormViewRow">
                        <td class="DkoFormViewLabel">
                            <asp:Label Text="Nome Prospect" runat="server" />
                        </td>
                        <td class="DkoFormViewField" colspan="2">
                            <asp:TextBox runat="server" id="txtNomeProspect" />
                        </td>
                        <td>
                            <asp:Label runat="server" text="Data Contato:"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox runat="server" id="txtDate" />
                            <asp:MaskedEditExtender ID="MaskedEditExtender3" runat="server" Mask="99/99/9999"
                                MaskType="Date" TargetControlID="txtDate">
                            </asp:MaskedEditExtender>
                            <asp:Button runat="server" ID="btnCalendar1" CssClass="DkoButtonCalendar" />
                            <asp:CalendarExtender ID="CalendarExtender3" runat="server" PopupButtonID="btnCalendar1"
                                TargetControlID="txtDate">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr class="DkoFormViewRow">
                        <td class="DkoFormViewLabel">
                            <asp:Label ID="lblTipoContato" Text="Tipo de Contato" runat="server" />
                        </td>
                        <td class="DkoFormViewField">
                            <asp:DropDownList runat="server" ID="ddlTipo">
                                <asp:ListItem Text="UA" />
                                <asp:ListItem Text="Visita" />
                                <asp:ListItem Text="Telefonema" />
                            </asp:DropDownList>
                        </td>
                        <td class="DkoFormViewLabel">
                            <asp:Label ID="lblSituacaoContato" Text="Situação" runat="server" />
                        </td>
                        <td class="DkoFormViewField">
                            <asp:DropDownList runat="server" ID="ddlSituacao" AutoPostBack="true" 
                                onselectedindexchanged="ddlSituacao_SelectedIndexChanged" >
                                <asp:ListItem Text="Em Andamento" />
                                <asp:ListItem Text="Associada" />
                                <asp:ListItem Text="Encerrada" />
                            </asp:DropDownList>
                        </td>
                        <td class="DkoFormViewLabel">
                            <asp:Label ID="lblNumConta" Text="Nº Conta" runat="server" />
                        </td>
                        <td class="DkoFormViewField">
                            <asp:TextBox runat="server" id="txtNumConta" />
                            <asp:MaskedEditExtender id="meeNumnConta" TargetControlID="txtNumConta" Mask="99999-9"
                                MaskType="Number" runat="server" />
                        </td>
                    </tr>
                    <tr class="DkoGridViewRow">
                        <td class="DkoFormViewLabel">
                            <asp:Label ID="lblDescricao" Text="Descrição do Contato" runat="server" />
                        </td>
                        <td class="DkoFormViewField" colspan="5">
                            <asp:TextBox runat="server" id="txtDescricaoContato" TextMode="MultiLine" Height="50px"
                                Width="90%" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
