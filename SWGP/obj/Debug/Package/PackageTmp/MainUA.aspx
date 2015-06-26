<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MainUA.aspx.cs" Inherits="SWGP.MainUA1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<link href="~/Styles/DkoFormView.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h3>|>> Unidade de Atendimento</h3>
    <asp:UpdatePanel runat="server">
       <ContentTemplate>
           <fieldset>
                <asp:Button Text="Salvar" runat="server" id="btnSalvar" CssClass="DkoButtonSave" OnClick="btnSalvar_Click"/>
                <input type="reset" name="name" value="Limpar" class="DkoButtonClean" />
                <input type="button" value="Voltar" onclick="history.go(-1)" class="DkoButtonBack">
            </fieldset>
           <fieldset>
           <legend>Dados Cadastro UA</legend>
                <table class="DkoFormView">
                        <tr class="DkoFormViewRow">
                            <td class="DkoFormViewLabel">
                                <asp:Label ID="lblNome" Text="Nome UA" runat="server" />
                            </td>
                            <td class="DkoFormViewField" colspan="2">
                                <asp:TextBox runat="server" id="txtNomeUA"/>
                            </td>
                            <td class="DkoFormViewEmptyDataRow" colspan="4">
                                
                            </td>
                        </tr>
                  </table>
              </fieldset>

               <fieldset>
                <legend>UA´s Cadastradas</legend>
                  <asp:GridView ID="gvPrincipal" runat="server" AllowSorting="false" AllowPaging="true" AutoGenerateColumns="False" CssClass="DkoGridView"
                DataSourceID="odsGrid" EmptyDataText="Não existem dados a serem exibidos"   PagerSettings-Mode="NextPreviousFirstLast" PageSize="5" 
                PagerSettings-FirstPageText="<|" PagerSettings-LastPageText="|>" PagerSettings-NextPageText=">" PagerSettings-PreviousPageText="<" 
                PagerStyle-HorizontalAlign="Center" PagerStyle-CssClass="DkoGridViewPager" 
                FooterStyle-CssClass="DkoGridViewFooter" HeaderStyle-CssClass="DkoGridViewHeader" 
                ShowFooter="True" PagerSettings-Visible="true" PageIndex="1" DataKeyNames="idUA" >
            <FooterStyle CssClass="DkoGridViewFooter" />
            <EmptyDataRowStyle CssClass="DkoGridViewEmptyDataRow" />
            <EditRowStyle CssClass="DkoGridViewEditRow" />
            <HeaderStyle CssClass="DkoGridViewHeader"></HeaderStyle>
                <Columns>
                <asp:BoundField DataField="idUA"  HeaderText="IdUA" SortExpression="IdUA"  HeaderStyle-Width="20%" Visible="false"/>
                <asp:BoundField DataField="Nome"  HeaderText="Nome" SortExpression="Nome"  HeaderStyle-Width="40%" />
                    <asp:TemplateField  SortExpression="" HeaderText="Excluir" HeaderStyle-Wrap="false" HeaderStyle-Width="3%">
                        <ItemTemplate>
                            <asp:ImageButton ImageUrl="Images/DkoGridView/delete.png" runat="server" id="btnExcluir" OnClick="btnExcluir_Click" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                <PagerSettings FirstPageText="<img src='Images/DkoGridView/document-page.png' border='0'  title='Primeira Página' />" 
                               LastPageText="<img src='Images/DkoGridView/document-page-last.png' border='0'  title='Última Página' />" 
                               NextPageText="<img src='Images/DkoGridView/document-page-next.png' border='0'  title='Próxima Página' />"
                               PreviousPageText="<img src='Images/DkoGridView/document-page-previous.png' border='0'  title='Página Anterior' />" />
                <RowStyle CssClass="DkoGridViewRow" />
                <SelectedRowStyle CssClass="DkoGridViewSelectedRow" />
                <PagerStyle CssClass="DkoGridViewPager" />
                <AlternatingRowStyle CssClass="DkoGridViewAlternatingRow" />
            </asp:GridView>     
            </fieldset>
           <asp:ObjectDataSource ID="odsGrid" runat="server" 
               SelectMethod="GetAll" TypeName="SWGPgen.UAControl"></asp:ObjectDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
