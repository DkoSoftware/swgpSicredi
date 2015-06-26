<%@ Page Title="" Language="C#" MasterPageFile="~/Lookpup.Master" AutoEventWireup="true" CodeBehind="NewIndicacao.aspx.cs" Inherits="SWGP.NewIndicacao" %>
  <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="barModals">
        <p>|>> Cadastro de Indicação</p>
    </div>
    <asp:UpdatePanel runat="server">
       <ContentTemplate>
         <asp:HiddenField  id="hfidUserIndica" Value=""  runat="server"/>
         <asp:HiddenField  id="hfidUserRecebe" Value="" runat="server"/>
         <asp:HiddenField  id="hfIdProspect" Value="" runat="server"/>
           <fieldset>
                <asp:Button Text="Salvar" runat="server" id="btnSalvar" CssClass="DkoButtonSave" OnClick="btnSalvar_Click"/>
                <input type="reset" name="name" value="Limpar" class="DkoButtonClean" />
                <asp:Button Text="Voltar" runat="server" id="btnVoltar" CssClass="DkoButtonBack" OnClick="btnVoltar_Click"/>
            </fieldset>
            <fieldset>
                <legend>Dados da Indicação</legend>
                <table class="DkoFormView">
                    <tr>
                        <td class="DkoFormViewLabel">
                            <asp:Label ID="lblNomePara" Text="Indicação para" runat="server" />
                        </td>
                        <td class="DkoFormViewField" colspan="3">
                            <asp:DropDownList runat="server" ID="ddlUsuarioParaIndicacao" DataSourceID="odsDdlNomesUsuario" DataTextField="UserName" DataValueField="idUsuario">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="DkoFormViewRow">
                        <td class="DkoFormViewLabel">
                            <asp:Label ID="lblNome" Text="Nome" runat="server" />
                        </td>
                        <td class="DkoFormViewField" colspan="3">
                            <asp:TextBox runat="server" id="txtNomeProspect"/>
                        </td>
                         <td class="DkoFormViewLabel">
                            <asp:Label ID="lblTelefone" Text="Telefone" runat="server" />
                        </td>
                        <td class="DkoFormViewField" colspan="2">
                            <asp:TextBox runat="server" id="txtTelefone"/>
                            <asp:MaskedEditExtender ID="meeTelefone" runat="server" MaskType="Number" Mask="99-99999999" TargetControlID="txtTelefone" />
                        </td>
                    </tr>

                    <tr class="DkoFormViewRow">
                        <td class="DkoFormViewLabel">
                            <asp:Label ID="lblEndereco" Text="Endereço" runat="server" />
                        </td>
                        <td class="DkoFormViewField" colspan="3">
                            <asp:TextBox runat="server" id="txtEndereco"/>
                        </td>
                        <td class="DkoFormViewLabel">
                            <asp:Label ID="lblBairro" Text="Bairro" runat="server" />
                        </td>
                        <td class="DkoFormViewField" colspan="2">
                            <asp:TextBox runat="server" id="txtBairro"/>
                        </td>
                    </tr>
                   
                    <tr class="DkoGridViewRow">
                        <td class="DkoFormViewLabel">
                            <asp:Label ID="lblCidade" Text="Cidade" runat="server" />
                        </td>
                        <td class="DkoFormViewField" colspan="3">
                            <asp:TextBox runat="server" id="txtCidade"/>
                        </td>
                        <td class="DkoFormViewLabel">
                            <asp:Label ID="lblEstado" Text="Estado" runat="server" />
                        </td>
                        <td class="DkoFormViewField" colspan="2">
                            <asp:DropDownList runat="server" ID="ddlUF">
                                <asp:ListItem Text="Selecione..." />
                                <asp:ListItem Text="AC" />
                                <asp:ListItem Text="AL" />
                                <asp:ListItem Text="AP" />
                                <asp:ListItem Text="AM" />
                                <asp:ListItem Text="BA" />
                                <asp:ListItem Text="CE" />
                                <asp:ListItem Text="DF" />
                                <asp:ListItem Text="ES" />
                                <asp:ListItem Text="GO" />
                                <asp:ListItem Text="MA" />
                                <asp:ListItem Text="MT" />
                                <asp:ListItem Text="MS" />
                                <asp:ListItem Text="MG" />
                                <asp:ListItem Text="PA" />
                                <asp:ListItem Text="PB" />
                                <asp:ListItem Text="PR" />
                                <asp:ListItem Text="PE" />
                                <asp:ListItem Text="PI" />
                                <asp:ListItem Text="RR" />
                                <asp:ListItem Text="RJ" />
                                <asp:ListItem Text="RN" />
                                <asp:ListItem Text="RS" />
                                <asp:ListItem Text="SC" />
                                <asp:ListItem Text="SP" />
                                <asp:ListItem Text="SE" />
                                <asp:ListItem Text="TO"/>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </fieldset>
                <asp:ObjectDataSource ID="odsDdlNomesUsuario" runat="server" 
               SelectMethod="GetAll" TypeName="SWGPgen.UsuarioControl"></asp:ObjectDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
