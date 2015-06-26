<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MainImportIndicacao.aspx.cs" Inherits="SWGP.MainImportIndicacao" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
 <h3>|>> Importação de Indicações em Lote</h3>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
           <asp:HiddenField runat="server" ID="hidItem" />
            <fieldset>
                <asp:Button ID="btnImportaLote" Text="Importar" runat="server" CssClass="DkoButtonImport" OnClick="btnImportaLote_Click" />
                <input type="reset" name="name" value="Limpar" class="DkoButtonClean" />
                <input type="button" value="Voltar" onclick="history.go(-1)" class="DkoButtonBack"
                    id="btnVoltar">
            </fieldset>
            <fieldset>
                <legend>Dados para Importação</legend>
                <table class="DkoFormView">
                    <tr class="DkoFormViewRow">
                        <td class="DkoFormViewLabel">
                           <asp:Label Text="UA" ID="lblUA" runat="server" />
                        </td>
                         <td colspan="2">
                            <asp:DropDownList runat="server" ID="ddlUA" Width="250"  OnSelectedIndexChanged="ddlUA_OnSelectedIndexChanged" AutoPostBack="true"
                              DataTextField="Nome" DataValueField="idUa">
                                <asp:ListItem Text="Selecione..." Enabled="true" Selected="True" Value="1"/>
                            </asp:DropDownList>
                        </td>
                        <td class="DkoFormViewLabel">
                           <asp:Label Text="Usuário de Destino" ID="lblUsuarioDestino" runat="server" />
                        </td>
                         <td colspan="2">
                            <asp:DropDownList runat="server" ID="ddlUsuarioParaIndicacao" Width="250" DataTextField="UserName" DataValueField="idUsuario" />
                        </td>
                        <tr>
                            <td style="width:500px">
                                <asp:Label Text="Arquivo" runat="server" id="lblArquivo"/>
                            </td>
                            <td class="DkoFormViewField" colspan="5" nowrap="nowrap">
                                   <asp:FileUpload id="fuNomeArquivo" runat="server" Width="400" CssClass="DkoButtonSearch" />
                                <asp:Label Text="(arquivo deve estar salvo do C:\)" runat="server" />
                            </td>
                        </tr>
                    </tr>
                </table>
            </fieldset>
     </ContentTemplate>
  </asp:UpdatePanel>
</asp:Content>
