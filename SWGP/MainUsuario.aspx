<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MainUsuario.aspx.cs" Inherits="SWGP.MainUsuario" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" charset="UTF-8" src="Scripts/core.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h3>|>> Usuários</h3>
    <br />
    <asp:UpdatePanel>
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hidItem" />
            <fieldset>
                <asp:Button ID="btnNewUsuario" Text="Novo" runat="server" CssClass="DkoButtonNew" />
                <asp:Button ID="btnEdit" Text="Editar" runat="server" CssClass="DkoButtonEdit" OnClick="btnEdit_Click" />
                <asp:Button ID="btnRefresh" Text="Atualizar" runat="server" CssClass="DkoButtonRefresh" OnClick="btnRefresh_Click" />
                 
                <input type="button" value="Voltar" onclick="history.go(-1)" class="DkoButtonBack">
            </fieldset>
            <fieldset>
                <legend>Pesquisa Usuário</legend>
                <asp:TextBox runat="server" id="txtNameUserSearch" MaxLength="30" Width="30%"/>
                <asp:Button ID="btnSearch" Text="Pesquisar" Visible="true" runat="server" CssClass="DkoButtonSearch" OnClick="btnSearch_Click" />
            </fieldset>
            <asp:GridView ID="gvPrincipal" runat="server" AllowPaging="true"
                AutoGenerateColumns="False" CssClass="DkoGridView" 
                EmptyDataText="Não existem dados a serem exibidos" PagerSettings-Mode="NextPreviousFirstLast"
                PageSize="10" PagerSettings-FirstPageText="<|" onrowdatabound="gvPrincipal_RowDataBound"  OnPageIndexChanging="gvPrincipal_PageIndexChanging"
                PagerSettings-LastPageText="|>" PagerSettings-NextPageText=">" PagerSettings-PreviousPageText="<"
                PagerStyle-HorizontalAlign="Center" PagerStyle-CssClass="DkoGridViewPager" FooterStyle-CssClass="DkoGridViewFooter"
                HeaderStyle-CssClass="DkoGridViewHeader" ShowFooter="True" PagerSettings-Visible="true"
                PageIndex="1" DataKeyNames="idUsuario">
                <FooterStyle CssClass="DkoGridViewFooter" />
                <EmptyDataRowStyle CssClass="DkoGridViewEmptyDataRow" />
                <EditRowStyle CssClass="DkoGridViewEditRow" />
                <HeaderStyle CssClass="DkoGridViewHeader"></HeaderStyle>
                <Columns>
                    <asp:TemplateField ItemStyle-Width="5" HeaderStyle-Width="3%" ItemStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <input type="radio" name="rdoItem" onclick="javascript:setItem('<%#hidItem.ClientID %>');">
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField visible="false">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="litValue"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="idUsuario" HeaderText="idUsuario" SortExpression="idUsuario"
                        Visible="false" HeaderStyle-Width="20%" />
                    <asp:BoundField DataField="Nome" HeaderText="Nome" SortExpression="Nome" HeaderStyle-Width="20%" />
                    <asp:BoundField DataField="Cargo" HeaderText="Cargo" SortExpression="Cargo" HeaderStyle-Width="10%"
                        ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="UA" HeaderText="UA" SortExpression="UA" HeaderStyle-Width="15%"
                        ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="Situacao" HeaderText="Situação" SortExpression="Situacao"
                        HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="Modulo" HeaderText="Módulo" SortExpression="Modulo" HeaderStyle-Width="10%"
                        ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="Username" HeaderText="Nome Usuario" SortExpression="Username"
                        HeaderStyle-Width="10%" />
                    <asp:TemplateField SortExpression="" HeaderText="Bloquear/Desbloquear" HeaderStyle-Wrap="false"
                        HeaderStyle-Width="10%">
                        <ItemTemplate>
                            <asp:ImageButton ImageUrl="Images/DkoGridView/block.png" HeaderText="Bloquear" runat="server"
                                id="btnBloquear" OnClick="btnBloquear_Click" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="" HeaderText="Excluir" HeaderStyle-Wrap="false"
                        HeaderStyle-Width="5%">
                        <ItemTemplate>
                            <asp:ImageButton ImageUrl="Images/DkoGridView/delete.png" runat="server" id="btnExcluir"
                                OnClick="btnExcluir_Click" />
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
           <%-- <asp:ObjectDataSource ID="odsGridPrincipal" runat="server" SelectMethod="GetAllUsersPrincipal"
                TypeName="SWGPgen.UsuarioControl"></asp:ObjectDataSource>--%>
            <%--modal Novo Usuario--%>
            <asp:ModalPopupExtender ID="mpeNovoUsuario" runat="server" TargetControlID="btnNewUsuario"
                PopupControlID="panNovoUsuario" BackgroundCssClass="DkoModalPopupExtanderBackground"
                CancelControlID="btnCloseNovoUsuario" PopupDragHandleControlID="panNovoUsuario">
            </asp:ModalPopupExtender>
            <asp:Panel ID="panNovoUsuario" runat="server" CssClass="DkoModalPopupContentPanel" BorderStyle="Groove" BorderColor="Gray" BorderWidth="9"
                Width="800px" Height="450px">
                <iframe id="Iframe2" src="NewUsuario.aspx" scrolling="auto" width="100%" height="100%"
                    frameborder="1" class="DkoModalPopupContentPanel" title="Cadastro de Usuário">
                </iframe>
                <asp:Button ID="btnCloseNovoUsuario" runat="server" Text="Cancelar" CssClass="DkoButtonCloseLookup" />
                <%-- <asp:Button ID="btnSalvar" runat="server" Text="Salvar"  CssClass="DkoButtonCloseLookup"/> --%>
            </asp:Panel>
            <%--fim modal novo prospect--%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
