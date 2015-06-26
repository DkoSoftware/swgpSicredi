<%@ Page Title="" Language="C#" MasterPageFile="~/Lookpup.Master" AutoEventWireup="true" CodeBehind="HistoricoContatos.aspx.cs" Inherits="SWGP.HistoricoContatos" %>
  <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" charset="UTF-8" src="Scripts/core.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="barModals">
        <p>|>>Histórico de Contatos</p>
    </div>
    <asp:UpdatePanel runat="server">
       <ContentTemplate>
       <asp:HiddenField runat="server" ID="hidItem" />
           <fieldset>
                <asp:Button Text="Novo Contato" runat="server" id="btnNovoContato" CssClass="DkoButtonSave" OnClick="btnNovoContato_Click" />
                <asp:Button ID="btnEdit" Text="Editar" runat="server" CssClass="DkoButtonEdit" OnClick="btnEdit_Click" />
                 <asp:Button Text="Atualizar" runat="server" id="btnAtualizar" CssClass="DkoButtonRefresh"  OnClick="btnAtualizar_Click"  />
            </fieldset>
            <fieldset>
            <table class="DkoFormView">
                    <tr class="DkoFormViewRow">
                        <td class="DkoFormViewLabel">
                            <asp:Label ID="Label1" Text="Nome Prospect" runat="server" />
                        </td>
                        <td class="DkoFormViewField" colspan="5">
                            <asp:TextBox runat="server" id="txtNomeProspect"/>
                        </td>
                    </tr>
                    </table>
                    <asp:HiddenField id="hfIdProspect" runat="server" />
                    <asp:HiddenField id="hfIdUsuario" runat="server"/>
                <legend>Contatos Realizados</legend>
                 <asp:GridView ID="gvPrincipal" runat="server" AllowSorting="True" 
                    AllowPaging="True" AutoGenerateColumns="False" CssClass="DkoGridView"
                     onrowdatabound="gvPrincipal_RowDataBound"
                    EmptyDataText="Não existem dados a serem exibidos"   
                    FooterStyle-CssClass="DkoGridViewFooter" HeaderStyle-CssClass="DkoGridViewHeader" 
                    ShowFooter="True" DataKeyNames="idContato"  PagerSettings-Mode="NextPreviousFirstLast"
                PageSize="10"  PagerStyle-CssClass="DkoGridViewPager" PagerSettings-Visible="true" PageIndex="1" OnPageIndexChanging="gvPrincipal_PageIndexChanging" >
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
                    <asp:BoundField DataField="idContato" HeaderText="idContato" 
                        SortExpression="idContato" Visible="false" >
                        <HeaderStyle Width="5%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DataContato" HeaderText="Data do Contato" dataformatstring="{0:dd-MM-yyyy}"
                        SortExpression="DataContato"  HeaderStyle-Width="10%" >
                        <HeaderStyle Width="10%" />
                    </asp:BoundField>

                    <asp:BoundField DataField="Nome" HeaderText="Nome"   HeaderStyle-Width="15%" Visible="false">
                        <HeaderStyle Width="15%" />
                    </asp:BoundField>

                    <asp:BoundField DataField="Telefone" HeaderText="Telefone"   HeaderStyle-Width="10%" >
                        <HeaderStyle Width="10%" />
                    </asp:BoundField>

                    <asp:BoundField DataField="Email" HeaderText="Email"   HeaderStyle-Width="10%" >
                        <HeaderStyle Width="15%" />
                    </asp:BoundField>

                    <asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" 
                        HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                        <HeaderStyle Width="7%" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>

                    <asp:BoundField DataField="Descricao" HeaderText="Descrição" 
                        SortExpression="Descricao" HeaderStyle-Width="17%" 
                        ItemStyle-HorizontalAlign="Left">
                        <HeaderStyle Width="17%" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Situacao" HeaderText="Situação" 
                        SortExpression="Situacao" HeaderStyle-Width="10%" 
                        ItemStyle-HorizontalAlign="Left">
                        <HeaderStyle Width="10%" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>

                    <asp:BoundField DataField="NumeroConta" HeaderText="Nº Conta" SortExpression="NumeroConta"  HeaderStyle-Width="13%" />
                    
                     <asp:TemplateField  SortExpression="" HeaderText="Excluir" HeaderStyle-Wrap="false" HeaderStyle-Width="13%">
                        <ItemTemplate>
                            <asp:ImageButton ImageUrl="Images/DkoGridView/delete.png" runat="server" id="btnExcluir" OnClick="btnExcluir_Click" />
                        </ItemTemplate>
                         <HeaderStyle Width="13%" Wrap="False" />
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
                  <asp:ObjectDataSource ID="odsHistoricoContato" runat="server" TypeName="SWGPgen.ContatoControl" SelectMethod="GetAllContactsByProspect">
                    <SelectParameters>
                     <asp:FormParameter FormField="hfIdProspect" Name="idProspect" />
                     <asp:FormParameter FormField="hfIdUsuario" Name="idUsuario" />
                    </SelectParameters>
                 </asp:ObjectDataSource>
            </fieldset>

             <%--modal novo Contato--%>
             <asp:HiddenField ID="hfNovoContato" runat="server"  />
             <asp:ModalPopupExtender ID="mpeNovoContato" runat="server" TargetControlID="hfNovoContato" PopupControlID="panelNovoContato" 
                 BackgroundCssClass="DkoModalPopupExtanderBackground"  CancelControlID="btnCloseNovoContato" PopupDragHandleControlID="panelNovoContato"> 
               </asp:ModalPopupExtender> 

                <asp:Panel ID="panelNovoContato" runat="server" CssClass="DkoModalPopupContentPanel" Width="800px" Height="380px" BorderStyle="Groove" BorderColor="Gray" BorderWidth="9">
                    <iframe id="frame1" src="NewContato.aspx" Scrolling="no" width="100%" height="100%" frameborder="1" 
                      class="DkoModalPopupContentPanel"  title="Cadastro de Contato"> 
                    </IFRAME>     
                     <asp:Button ID="btnCloseNovoContato" runat="server" Text="Fechar" Visible="true" CssClass="DkoButtonCloseLookup"/>
                   
                </asp:Panel> 
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
