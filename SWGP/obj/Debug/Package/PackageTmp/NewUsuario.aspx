<%@ Page Title="" Language="C#" MasterPageFile="~/Lookpup.Master" AutoEventWireup="true" CodeBehind="NewUsuario.aspx.cs" Inherits="SWGP.NewUsuario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="~/Styles/DkoFormView.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h3>|>> Cadastro de Usuário</h3>
    <asp:UpdatePanel runat="server">
    <ContentTemplate>
        <fieldset>
              <asp:Button Text="Salvar" id="btnSalvar" runat="server" CssClass="DkoButtonSave"  CausesValidation="false" OnClick="btnSalvar_Click"/>
              <input type="reset" name="name" value="Limpar" class="DkoButtonClean" />
        </fieldset>
        <fieldset>
            <legend>Dados do Usuário</legend>
              <table class="DkoFormView">
                <tr class="DkoFormViewRow">
                    <td class="DkoFormViewLabel">
                        <asp:Label id="lblNome" Text="Nome" runat="server" />
                    </td>
                    <td class="DkoFormViewField">
                        <asp:TextBox runat="server" id="txtNome"/>
                        <asp:RequiredFieldValidator id="rfvNome" ErrorMessage="Campo 'Nome' é preenchimento obrigatório." ControlToValidate="txtNome"
                            runat="server" Text="*"/>
                    </td>
                    <td class="DkoFormViewLabel">
                        <asp:Label id="LblCargo" Text="Função" runat="server" />
                    </td>
                    <td class="DkoFormViewField">
                        <asp:DropDownList ID="ddlFuncao" runat="server" >
                            <asp:ListItem Text="Selecione..."  />
                            <asp:ListItem Text="GUA" />
                            <asp:ListItem Text="Gerente de Negócios PJ" />
                            <asp:ListItem Text="Gerente de Negócios PF" />
                            <asp:ListItem Text="Assistente" />
                            <asp:ListItem Text="Outro" />
                        </asp:DropDownList>
                    </td>
                </tr>

                <tr class="DkoFormViewRow">
                    <td class="DkoFormViewLabel">
                        <asp:Label id="lblUserName" Text="Usuário" runat="server" />
                    </td>
                    <td class="DkoFormViewField">
                        <asp:TextBox runat="server" id="txtUsuario"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="Campo 'Usuário' é preenchimento obrigatório." ControlToValidate="txtUsuario"
                            runat="server" Text="*" />
                    </td>
                    <td class="DkoFormViewLabel">
                        <asp:Label id="lblUA" Text="Unidade de Atendimento" runat="server" />
                    </td>
                    <td class="DkoFormViewField">
                        <asp:DropDownList runat="server" ID="ddlUA" />
                    </td>
                </tr>

                <tr class="DkoFormViewRow">
                    <td class="DkoFormViewLabel">
                        <asp:Label id="lblPassword" Text="Senha" runat="server" />
                    </td>
                    <td class="DkoFormViewField">
                        <asp:TextBox runat="server" id="txtSenha" TextMode="Password"/>
                         <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Campo 'Senha' é preenchimento obrigatório." ControlToValidate="txtSenha"
                            runat="server" Text="*" />
                    </td>
                </tr>

                <tr class="DkoFormViewRow">
                    <td class="DkoFormViewLabel">
                        <asp:Label id="lblConfirmPassword" Text="Confirma Senha" runat="server" />
                    </td> 
                    <td class="DkoFormViewField">
                        <asp:TextBox runat="server" id="txtConfirmPassword" TextMode="Password"/>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ErrorMessage="Campo 'Senha' e Confirme Senha estão divergentes, favor verificar." ControlToValidate="txtConfirmPassword"
                            runat="server" Text="*" Display="Static"/>
                    </td>
                </tr>
                <tr class="DkoFormViewRow">
                    <td class="DkoFormViewLabel">
                        <asp:Label ID="lblSituacao" Text="Situação" runat="server" />
                    </td>
                    <td class="DkoFormViewField">
                        <asp:CheckBox Text="  Ativo" runat="server" id="chbHabilitado"/>
                    </td>
                </tr>
                <tr>
                    <td class="DkoFormViewLabel">
                        <asp:Label Text="Módulo" id="lblModulo" runat="server" />
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlModulo">
                            <asp:ListItem Text="Usuário" Value="U"/>
                            <asp:ListItem Text="Administrador" Value="A"/>
                            <asp:ListItem Text="Administrador Master" Value="M"/>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </fieldset>
       
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

