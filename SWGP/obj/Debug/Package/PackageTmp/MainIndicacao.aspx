<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MainIndicacao.aspx.cs" Inherits="SWGP.MainIndicacao" %>
 <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/DkoTabContainer.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" charset="UTF-8" src="Scripts/core.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h3>|>> Indicações    </h3><br />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
          <asp:HiddenField runat="server" ID="hidItem" />
            <fieldset>
                  <asp:Button ID="btnNewIndicacao" Text="Nova" runat="server" CssClass="DkoButtonNew" OnClick="btnNewIndicacao_Click"/>
                  <asp:Button Text="Atualizar" runat="server" id="btnAtualizar" CssClass="DkoButtonRefresh" OnClick="btnAtualizar_Click"/>
                  <input type="button" value="Voltar" onclick="history.go(-1)" class="DkoButtonBack" />
            </fieldset>

            <asp:TabContainer runat="server" ID="tabConteinerPrincipal" >
                 <asp:TabPanel ID="TabPanel1" TabIndex="0" Width="97%" runat="server">
                <HeaderTemplate>
                    <asp:Label ID="Label1" Text="Indicações Recebidas" runat="server" />
                </HeaderTemplate>
                <ContentTemplate>
                <asp:HiddenField ID="hfIdIndicacao" runat="server" Value="" />

                        <asp:GridView ID="gvPrincipalRecebida" runat="server" AllowSorting="True" 
                             AllowPaging="True" AutoGenerateColumns="False" CssClass="DkoGridView"
                            EmptyDataText="Não existem dados a serem exibidos" PageSize="20" onrowdatabound="gvPrincipalRecebida_RowDataBound"
                            ShowFooter="True" PageIndex="10" DataKeyNames="idIndicacao"  OnPageIndexChanging="gvPrincipalRecebida_PageIndexChanging"
                            PagerSettings-Mode="NextPreviousFirstLast"  PagerSettings-Position="Bottom" >
                        <FooterStyle CssClass="DkoGridViewFooter"  />
                        <EmptyDataRowStyle CssClass="DkoGridViewEmptyDataRow" />
                        <EditRowStyle CssClass="DkoGridViewEditRow" />
                        <HeaderStyle CssClass="DkoGridViewHeader"></HeaderStyle>
                            <Columns>
                           <asp:TemplateField Visible="false">
                                <ItemTemplate >
                                    <input  type="radio" name="rdoItem" onclick="javascript:setItem('<%#hidItem.ClientID %>');" >
                                </ItemTemplate>
                                     <HeaderStyle Width="3%" />
                                         <ItemStyle HorizontalAlign="Center" Width="5px" />
                            </asp:TemplateField>
                            <asp:TemplateField visible="False">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="litValue" Visible="false"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="idIndicacao"  HeaderText="idIndicacao" 
                                    SortExpression="idIndicacao" Visible="False">
                                <HeaderStyle Width="3%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Nome"  HeaderText="Nome" SortExpression="Nome" >
                                <HeaderStyle Width="40%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Telefone"  HeaderText="Telefone" 
                                    SortExpression="Telefone" >
                                <HeaderStyle Width="10%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="UserName"  HeaderText="Usuario que Enviou" 
                                    SortExpression="Username" >
                                <HeaderStyle Width="10%" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Visualizar">
                                <ItemTemplate>
                                    <asp:ImageButton ImageUrl="~/Images/DkoGridView/searchButton.gif" runat="server" id="btnVisualizar" OnClick="btnVisualizarRecebida_Click" />
                                </ItemTemplate>
                              <HeaderStyle Width="10%" Wrap="False" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Excluir">
                                <ItemTemplate>
                                    <asp:ImageButton ImageUrl="~/Images/DkoGridView/delete.png" runat="server" id="btnExcluir" OnClick="btnExcluirRecebida_Click"/>
                                </ItemTemplate>
                              <HeaderStyle Width="5%" Wrap="False" />
                                 <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            </Columns>
                           <PagerSettings FirstPageText="<img src='Images/DkoGridView/document-page.png' border='0'  title='Primeira Página' />" 
                                          LastPageText="<img src='Images/DkoGridView/document-page-last.png' border='0'  title='Última Página' />" 
                                           NextPageText="<img src='Images/DkoGridView/document-page-next.png' border='0'  title='Próxima Página' />"
                                             PreviousPageText="<img src='Images/DkoGridView/document-page-previous.png' border='0'  title='Página Anterior' />" />
                            <RowStyle CssClass="DkoGridViewRow" />
                            <SelectedRowStyle CssClass="DkoGridViewSelectedRow" />
                            <PagerStyle CssClass="DkoGridViewPager" HorizontalAlign="Center" />
                            <AlternatingRowStyle CssClass="DkoGridViewAlternatingRow" />
                        </asp:GridView>
                 </ContentTemplate>
               </asp:TabPanel>

                <asp:TabPanel ID="tpEnviada" TabIndex="0" Width="97%" runat="server">
                    <HeaderTemplate>
                        <asp:Label ID="lblTitulo" Text="Indicações Realizadas" runat="server" />
                    </HeaderTemplate>
                        <ContentTemplate>
                                <asp:GridView ID="gvPrincipalEnviada" runat="server" AllowSorting="true" AllowPaging="true" AutoGenerateColumns="False" CssClass="DkoGridView"
                                        EmptyDataText="Não existem dados a serem exibidos"  FooterStyle-CssClass="DkoGridViewFooter" HeaderStyle-CssClass="DkoGridViewHeader" onrowdatabound="gvPrincipalEnviada_RowDataBound"
                                        ShowFooter="True" DataKeyNames="idIndicacao" OnPageIndexChanging="gvPrincipalRecebida_PageIndexChanging" PageSize="10"  PagerSettings-Mode="NextPreviousFirstLast"  PagerSettings-Position="Bottom">
                                    <FooterStyle CssClass="DkoGridViewFooter" />
                                <EmptyDataRowStyle CssClass="DkoGridViewEmptyDataRow" />
                                <EditRowStyle CssClass="DkoGridViewEditRow" />
                                <HeaderStyle CssClass="DkoGridViewHeader"></HeaderStyle>
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="5" HeaderStyle-Width="3%" ItemStyle-HorizontalAlign="center" Visible="false">
                                            <ItemTemplate>
                                                <input type="radio" name="rdoItem" onclick="javascript:setItem('<%#hidItem.ClientID %>');">
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField visible="false">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="litValue"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="idIndicacao"  HeaderText="idIndicacao" SortExpression="idIndicacao"  HeaderStyle-Width="3%" Visible="false"/>
                                        <asp:BoundField DataField="Nome"  HeaderText="Nome" SortExpression="Nome"  HeaderStyle-Width="40%" />
                                        <asp:BoundField DataField="Telefone"  HeaderText="Telefone" SortExpression="Telefone"  HeaderStyle-Width="10%" />
                                        <asp:BoundField DataField="Username"  HeaderText="Usuario que Recebeu " SortExpression="Username"  HeaderStyle-Width="10%" />
                                        <asp:TemplateField  SortExpression="" HeaderText="Visualizar" HeaderStyle-Wrap="false" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:ImageButton ImageUrl="~/Images/DkoActionButton/searchButton.gif" runat="server" id="btnVisualizar" OnClick="btnVisualizarEnviada_Click" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                            
                                        <asp:TemplateField  SortExpression="" HeaderText="Excluir" HeaderStyle-Wrap="false" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:ImageButton ImageUrl="~/Images/DkoActionButton/delete.png" runat="server" id="btnExcluir" OnClick="btnExcluirEnviada_Click"/>
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
                        </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
            <p></p>

            <%--modal Nova Indicação--%>
            <asp:HiddenField ID="hfNovaIndicacao" runat="server" />
             <asp:ModalPopupExtender ID="mpeNovaIndicacao" runat="server" TargetControlID="hfNovaIndicacao" PopupControlID="panNovaIndicacao" 
                 BackgroundCssClass="DkoModalPopupExtanderBackground" PopupDragHandleControlID="panNovaIndicacao"> 
               </asp:ModalPopupExtender> 
                <asp:Panel ID="panNovaIndicacao" runat="server" CssClass="DkoModalPopupContentPanel" Width="800px" Height="450px" BorderStyle="Groove" BorderColor="Gray" BorderWidth="9">
                      <div class="barModals">
                            <p>|>> Cadastro de Indicação</p>
                        </div>
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
                            <asp:Label ID="Label2" Text="UA" runat="server" />
                        </td>
                        <td class="DkoFormViewField" colspan="3">
                            <asp:DropDownList runat="server" ID="ddlUA" AutoPostBack="true" OnSelectedIndexChanged="ddlUA_OnSelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="DkoFormViewLabel">
                            <asp:Label ID="lblNomePara" Text="Indicação para" runat="server" />
                        </td>
                        <td class="DkoFormViewField" colspan="3">
                            <asp:DropDownList runat="server" ID="ddlUsuarioParaIndicacao">
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
                </asp:Panel>
              <%--fim modal novo prospect--%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

