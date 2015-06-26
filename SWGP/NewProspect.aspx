<%@ Page Title="" Language="C#" MasterPageFile="~/Lookpup.Master" AutoEventWireup="true" CodeBehind="NewProspect.aspx.cs" Inherits="SWGP.NewProspect" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <script type="text/javascript" charset="UTF-8" src="Scripts/core.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h3>|>> Cadastro de Prospect</h3>
    <asp:UpdatePanel runat="server">
       <ContentTemplate>
         <asp:HiddenField runat="server" ID="hidItem" />
           <fieldset>
                <asp:Button Text="Salvar" runat="server" id="btnSalvar" CssClass="DkoButtonSave" OnClick="btnSalvar_Click"/>
                <asp:Button Text="Limpar" runat="server" id="btnLimpar" CssClass="DkoButtonClean" OnClick="btnLimpar_Click"/>
            </fieldset>
            <fieldset>
            <table class="DkoFormView">
                    <tr class="DkoFormViewRow">
                        <td class="DkoFormViewLabel" >
                            <asp:Label ID="Label1" Text="Nome Prospect" runat="server" />
                        </td>
                        <td class="DkoFormViewField" colspan="3">
                            <asp:TextBox runat="server" id="txtNomeProspect"/>
                        </td>
                        <td class="DkoFormViewLabel">
                            <asp:Label ID="LblTelefone" Text="Telefone" runat="server" />
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
                        <td class="DkoFormViewField" colspan="2">
                            <asp:TextBox runat="server" id="txtEndereco"/>
                        </td>
                        <td class="DkoFormViewLabel">
                            <asp:Label ID="lblBairro" Text="Bairro" runat="server" />
                        </td>
                        <td class="DkoFormViewField" colspan="2">
                            <asp:TextBox runat="server" id="txtBairro"/>
                        </td>
                        <td class="DkoFormViewLabel" >
                            <asp:Label ID="lblComplemento" Text="Complemento" runat="server" />
                        </td>
                        <td class="DkoFormViewField" >
                            <asp:TextBox runat="server" id="txtComplemento"/>
                        </td>
                    </tr>

                    <tr class="DkoFormViewRow"> 
                        <td class="DkoFormViewLabel">
                            <asp:Label ID="lblCidade" Text="Cidade" runat="server" />
                        </td>
                        <td class="DkoFormViewField" colspan="3">
                            <asp:TextBox runat="server" id="txtCidade"/>
                        </td>
                        <td class="DkoFormViewLabel">
                            <asp:Label ID="lblEstado" Text="Estado" runat="server" />
                        </td>
                        <td class="DkoFormViewField" >
                            <asp:DropDownList runat="server" ID="ddlEstado">
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

                    <tr class="DkoFormViewRow"> 
                        <td class="DkoFormViewLabel">
                            <asp:Label ID="lblTipo" Text="Tipo Pessoa" runat="server" />
                        </td>
                        <td class="DkoFormViewField" colspan="2"  >
                            <asp:DropDownList runat="server" id="ddlTipoPessoa">
                                <asp:ListItem Text="Selecione..." />
                                <asp:ListItem Text="PF" />
                                <asp:ListItem Text="PJ" />
                            </asp:DropDownList>
                        </td>

                        <td class="DkoFormViewLabel">
                            <asp:Label ID="lblSegmento" Text="Segmento" runat="server" />
                        </td>
                        <td class="DkoFormViewField" colspan="2"  >
                            <asp:DropDownList runat="server" id="ddlSegmento">
                                <asp:ListItem Text="Selecione..." />
                                <asp:ListItem Text="Indústria" />
                                <asp:ListItem Text="Comércio" />
                                <asp:ListItem Text="Serviço" />
                            </asp:DropDownList>
                        </td>
                    </tr>

                    <tr class="DkoFormViewRow"> 
                        <td class="DkoFormViewLabel">
                            <asp:Label ID="lblCPF" Text="CPF" runat="server" />
                        </td>
                        <td class="DkoFormViewField" colspan="2">
                            <asp:TextBox runat="server" id="txtCPF"/>
                            <asp:MaskedEditExtender id="meeCPF" Mask="999999999-99" MaskType="Number" TargetControlID="txtCPF" runat="server" />
                        </td>
                        <td class="DkoFormViewLabel">
                            <asp:Label ID="lblCNPJ" Text="CNPJ" runat="server" />
                        </td>
                        <td class="DkoFormViewField" colspan="2">
                            <asp:TextBox runat="server" id="txtCNPJ"/>
                            <asp:MaskedEditExtender id="meeCNPJ" runat="server" Mask="99.999.999/9999-99" MaskType="Number" TargetControlID="txtCNPJ" />
                        </td>
                    </tr>

                    <tr class="DkoFormViewRow"> 
                        <td class="DkoFormViewLabel">
                            <asp:Label ID="lblPessoaContato" Text="Pessoa Contato" runat="server" />
                        </td>
                        <td class="DkoFormViewField" colspan="2">
                            <asp:TextBox runat="server" id="txtPessoaContato"/>
                        </td>
                        <td class="DkoFormViewLabel">
                            <asp:Label ID="lblEmail" Text="Email" runat="server" />
                        </td>
                        <td class="DkoFormViewField" colspan="2">
                            <asp:TextBox runat="server" id="txtEmail"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="DkoFormViewLabel">
                            <asp:Label Text="Observação" runat="server" id="lblObservacao"/>
                        </td>
                        <td class="DkoFormViewField">
                           <asp:TextBox runat="server" id="txtObservacao" TextMode="MultiLine" Height="50px"
                                Width="490px" />
                        </td>
                    </tr>
              </table>
            </fieldset>
            <asp:ObjectDataSource ID="odsDdlNomesUsuario" runat="server" 
               SelectMethod="GetAll" TypeName="SWGPgen.UsuarioControl"></asp:ObjectDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
