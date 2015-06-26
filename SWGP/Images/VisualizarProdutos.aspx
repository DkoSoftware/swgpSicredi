<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/Site.Master" AutoEventWireup="true" CodeBehind="VisualizarProdutos.aspx.cs"
    Inherits="WLMWG.Web.Pages.GerenciamentoSistema.VisualizarProdutos" %>

<%@ Register Assembly="CWI.Framework.Web" Namespace="CWI.Framework.Web.UI.Controls"
    TagPrefix="cwi" %>
<%@ Register Assembly="CWI.Framework.Web" Namespace="CWI.Framework.Web.UI.Controls.Buttons"
    TagPrefix="cwi" %>
    
<asp:Content ID="Content3" ContentPlaceHolderID="headerPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentPlaceHolder" runat="server">
    <asp:UpdatePanel ID="uptProdutos" runat="server">
        <ContentTemplate>
            <cwi:Highlight ID="hltProdutos" runat="server" Text="Pesquisa de Produtos" />
            <cwi:ButtonContainer ID="btnContainer" runat="server">
                <cwi:ActionButtonSearch ID="btnPesquisar" runat="server"></cwi:ActionButtonSearch>
                <cwi:ActionButtonClear ID="btnLimpar" runat="server"></cwi:ActionButtonClear>
                <cwi:NavigationButtonBack ID="btnVoltar" runat="server" OnClick="btnVoltar_Click"></cwi:NavigationButtonBack>
            </cwi:ButtonContainer>
            
            <fieldset>
                <legend>Filtro de Pesquisa</legend>
                <cwi:FormView ID="frvPrincipal" runat="server" CssClass="CWIFormView" DataKeyNames="IdItem">
                    <EditItemTemplate>
                        <table id="tblFormView" class="CWIFormView">
                            <tr class="CWIFormViewRow">
                                <td class="CWIFormViewLabel">
                                    <cwi:Label ID="lblCodigo" runat="server">Código</cwi:Label>
                                </td>
                                <td class="CWIFormViewField" nowrap="nowrap">
                                    <cwi:TextBox ID="txtCodigo" runat="server" style="text-align: right;"
                                        TextTransform="Default" Width="90%"></cwi:TextBox>
                                    <cwi:MaskedIntegerExtender ID="mieCodigo" runat="server" InputDirection="RightToLeft"
                                        TargetControlID="txtCodigo" PromptCharacter="">
                                    </cwi:MaskedIntegerExtender>
                                </td>
                                <td class="CWIFormViewLabel">
                                    <cwi:Label ID="lblUpc" runat="server">UPC</cwi:Label>
                                </td>
                                <td class="CWIFormViewField" nowrap="nowrap">
                                    <cwi:TextBox ID="txtUpc" runat="server" MaskOrientation="LeftToRight" MaskType=""
                                        MaxLength="15" TextTransform="Default" Width="90%"></cwi:TextBox>
                                </td>
                            </tr>
                            
                            <tr class="CWIFormViewRow">
                                <td class="CWIFormViewLabel">
                                    <cwi:Label ID="lblDescricao" runat="server">Descrição</cwi:Label>
                                </td>
                                <td class="CWIFormViewField" nowrap="nowrap">
                                    <cwi:TextBox ID="txtDescricao" runat="server" MaskOrientation="LeftToRight" MaskType=""
                                        MaxLength="255" TextTransform="Default" Width="90%"></cwi:TextBox>
                                </td>
                                <td class="CWIFormViewLabel">
                                    <cwi:Label ID="lblSituacao" runat="server">Situação</cwi:Label>
                                </td>
                                <td class="CWIFormViewField" nowrap="nowrap">
                                    <cwi:DropDownList ID="ddlSituacao" runat="server" DataTextField="Text" DataValueField="Value"
                                        DefaultValue="S" ShowDefaultFirstItem="false" BindTextToToolTip="true">
                                        <asp:ListItem Text="Ativo" Value="S"></asp:ListItem>
                                        <asp:ListItem Text="Inativo" Value="N"></asp:ListItem>
                                    </cwi:DropDownList>
                                </td>
                            </tr>
                            
                            <tr>
                                <td class="CWIFormViewLabel">
                                    <cwi:Label ID="lblCentroDistribuicao" runat="server">Centro de Distribuição</cwi:Label>
                                </td>
                                <td class="" nowrap="nowrap">
                                    <cwi:TextBox ID="txtCodCentroDistribuicao" Text='<%# Bind("CodCentroDistribuicao") %>' runat="server"
                                        style="text-align: right;" TextTransform="Default" Width="25%"
                                        OnTextChanged="txtCodCentroDistribuicao_TextChanged" AutoPostBack="true">
                                    </cwi:TextBox>
                                    <cwi:MaskedIntegerExtender ID="mieCodCentroDistribuicao" runat="server" InputDirection="RightToLeft"
                                        TargetControlID="txtCodCentroDistribuicao" PromptCharacter="">
                                    </cwi:MaskedIntegerExtender>
                                    <cwi:TextBox ID="txtCentroDistribuicao" runat="server" MaskOrientation="LeftToRight" MaskType=""
                                        MaxLength="50" TextTransform="Default" Width="54%" ReadOnly="true" Enabled="false">
                                    </cwi:TextBox>
                                    <cwi:LookupExtender ID="lkpCentroDistribuicao" OnLookupClick="lkpCentroDistribuicao_LookupClick"
                                        runat="server" TargetControlID="txtCentroDistribuicao" ImageUrl="~/Images/CWILookupExtender/lookup.gif" />
                                </td>
                                <td class="CWIFormViewLabel">
                                    <cwi:Label ID="lblLoja" runat="server">Loja</cwi:Label>
                                </td>
                                <td class="" nowrap="nowrap">
                                    <cwi:TextBox ID="txtCodLoja" Text='<%# Bind("CodLoja") %>' runat="server"
                                        style="text-align: right;" TextTransform="Default" Width="25%"
                                        OnTextChanged="txtCodLoja_TextChanged" AutoPostBack="true">
                                    </cwi:TextBox>
                                    <cwi:MaskedIntegerExtender ID="mieCodLoja" runat="server" InputDirection="RightToLeft"
                                        TargetControlID="txtCodLoja" PromptCharacter="">
                                    </cwi:MaskedIntegerExtender>
                                    <cwi:TextBox ID="txtLoja" runat="server" MaskOrientation="LeftToRight" MaskType=""
                                        MaxLength="50" TextTransform="Default" Width="54%" ReadOnly="true" Enabled="false">
                                    </cwi:TextBox>
                                    <cwi:LookupExtender ID="lkpLoja" OnLookupClick="lkpLoja_LookupClick" runat="server"
                                        TargetControlID="txtLoja" ImageUrl="~/Images/CWILookupExtender/lookup.gif" />
                                </td>
                            </tr>
                            
                            <tr>
                                <td class="CWIFormViewLabel">
                                    <cwi:Label ID="lblDivisao" runat="server">Divisão</cwi:Label>
                                </td>
                                <td class="" nowrap="nowrap">
                                    <cwi:TextBox ID="txtCodDivisao" Text='<%# Bind("IdDivisao") %>' runat="server"
                                        style="text-align: right;" TextTransform="Default" Width="25%"
                                        OnTextChanged="txtCodDivisao_TextChanged" AutoPostBack="true">
                                    </cwi:TextBox>
                                    <cwi:MaskedIntegerExtender ID="mieCodDivisao" runat="server" InputDirection="RightToLeft"
                                        TargetControlID="txtCodDivisao" PromptCharacter="">
                                    </cwi:MaskedIntegerExtender>
                                    <cwi:TextBox ID="txtDivisao" runat="server" MaskOrientation="LeftToRight" MaskType=""
                                        MaxLength="50" TextTransform="Default" Width="54%" ReadOnly="true" Enabled="false">
                                    </cwi:TextBox>
                                    <cwi:LookupExtender ID="lkpDivisao" OnLookupClick="lkpDivisao_LookupClick" runat="server"
                                        TargetControlID="txtDivisao" ImageUrl="~/Images/CWILookupExtender/lookup.gif" />
                                </td>
                                <td class="CWIFormViewLabel">
                                    <cwi:Label ID="lblDepartamento" runat="server">Departamento</cwi:Label>
                                </td>
                                <td class="" nowrap="nowrap">
                                    <asp:HiddenField ID="hdnIdDepartamento" runat="server" />
                                    <cwi:TextBox ID="txtCodDepartamento" Text='<%# Bind("IdDepartamento") %>' runat="server"
                                        style="text-align: right;" TextTransform="Default" Width="25%"
                                        OnTextChanged="txtCodDepartamento_TextChanged" AutoPostBack="true">
                                    </cwi:TextBox>
                                    <cwi:MaskedIntegerExtender ID="mieCodDepartamento" runat="server" InputDirection="RightToLeft"
                                        TargetControlID="txtCodDepartamento" PromptCharacter="">
                                    </cwi:MaskedIntegerExtender>
                                    <cwi:TextBox ID="txtDepartamento" runat="server" MaskOrientation="LeftToRight" MaskType=""
                                        MaxLength="50" TextTransform="Default" Width="54%" ReadOnly="true" Enabled="false">
                                    </cwi:TextBox>
                                    <cwi:LookupExtender ID="lkpDepartamento" OnLookupClick="lkpDepartamento_LookupClick" runat="server"
                                        TargetControlID="txtDepartamento" ImageUrl="~/Images/CWILookupExtender/lookup.gif" />
                                </td>
                            </tr>
                            
                            <tr>
                                <td class="CWIFormViewLabel">
                                    <cwi:Label ID="lblCategoria" runat="server">Categoria</cwi:Label>
                                </td>
                                <td class="" nowrap="nowrap">
                                    <asp:HiddenField ID="hdnIdCategoria" runat="server" />
                                    <cwi:TextBox ID="txtCodCategoria" Text='<%# Bind("IdCategoria") %>' runat="server"
                                        style="text-align: right;" TextTransform="Default" Width="25%"
                                        OnTextChanged="txtCodCategoria_TextChanged" AutoPostBack="true">
                                    </cwi:TextBox>
                                    <cwi:MaskedIntegerExtender ID="mieCodCategoria" runat="server" InputDirection="RightToLeft"
                                        TargetControlID="txtCodCategoria" PromptCharacter="">
                                    </cwi:MaskedIntegerExtender>
                                    <cwi:TextBox ID="txtCategoria" runat="server" MaskOrientation="LeftToRight" MaskType=""
                                        MaxLength="50" TextTransform="Default" Width="54%" ReadOnly="true" Enabled="false">
                                    </cwi:TextBox>
                                    <cwi:LookupExtender ID="lkpCategoria" OnLookupClick="lkpCategoria_LookupClick" runat="server"
                                        TargetControlID="txtCategoria" ImageUrl="~/Images/CWILookupExtender/lookup.gif" />
                                </td>
                                <td class="CWIFormViewLabel">
                                    <cwi:Label ID="lblSubcategoria" runat="server">Subcategoria</cwi:Label>
                                </td>
                                <td class="" nowrap="nowrap">
                                    <asp:HiddenField ID="hdnIdSubcategoria" runat="server" />
                                    <cwi:TextBox ID="txtCodSubcategoria" Text='<%# Bind("IdSubcategoria") %>' runat="server"
                                        style="text-align: right;" TextTransform="Default" Width="25%"
                                        OnTextChanged="txtCodSubcategoria_TextChanged" AutoPostBack="true">
                                    </cwi:TextBox>
                                    <cwi:MaskedIntegerExtender ID="mieCodSubcategoria" runat="server" InputDirection="RightToLeft"
                                        TargetControlID="txtCodSubcategoria" PromptCharacter="">
                                    </cwi:MaskedIntegerExtender>
                                    <cwi:TextBox ID="txtSubcategoria" runat="server" MaskOrientation="LeftToRight" MaskType=""
                                        MaxLength="50" TextTransform="Default" Width="54%" ReadOnly="true" Enabled="false">
                                    </cwi:TextBox>
                                    <cwi:LookupExtender ID="lkpSubcategoria" OnLookupClick="lkpSubcategoria_LookupClick" runat="server"
                                        TargetControlID="txtSubcategoria" ImageUrl="~/Images/CWILookupExtender/lookup.gif" />
                                </td>
                            </tr>
                            
                            <tr>
                                <td class="CWIFormViewLabel">
                                    <cwi:Label ID="lblFineline" runat="server">Fineline</cwi:Label>
                                </td>
                                <td class="" nowrap="nowrap">
                                    <asp:HiddenField ID="hdnIdFineline" runat="server" />
                                    <cwi:TextBox ID="txtCodFineline" Text='<%# Bind("IdFineline") %>' runat="server"
                                        style="text-align: right;" TextTransform="Default" Width="25%"
                                        OnTextChanged="txtCodFineline_TextChanged" AutoPostBack="true">
                                    </cwi:TextBox>
                                    <cwi:MaskedIntegerExtender ID="mieIdFineline" runat="server" InputDirection="RightToLeft"
                                        TargetControlID="txtCodFineline" PromptCharacter="">
                                    </cwi:MaskedIntegerExtender>
                                    <cwi:TextBox ID="txtFineline" runat="server" MaskOrientation="LeftToRight" MaskType=""
                                        MaxLength="50" TextTransform="Default" Width="54%" ReadOnly="true" Enabled="false">
                                    </cwi:TextBox>
                                    <cwi:LookupExtender ID="lkpFineline" OnLookupClick="lkpFineline_LookupClick" runat="server"
                                        TargetControlID="txtFineline" ImageUrl="~/Images/CWILookupExtender/lookup.gif" />
                                </td>
                                <td class="CWIFormViewLabel">
                                    <cwi:Label ID="lblFornecedor" runat="server">Fornecedor</cwi:Label>
                                </td>
                                <td class="" nowrap="nowrap">
                                    <cwi:TextBox ID="txtCodFornecedor" Text='<%# Bind("CodFornecedor") %>' runat="server"
                                        style="text-align: right;" TextTransform="Default" Width="25%"
                                        OnTextChanged="txtCodFornecedor_TextChanged" AutoPostBack="true">
                                    </cwi:TextBox>
                                    <cwi:MaskedIntegerExtender ID="mieCodFornecedor" runat="server" InputDirection="RightToLeft"
                                        TargetControlID="txtCodFornecedor" PromptCharacter="">
                                    </cwi:MaskedIntegerExtender>
                                    <cwi:TextBox ID="txtFornecedor" runat="server" MaskOrientation="LeftToRight" MaskType=""
                                        MaxLength="50" TextTransform="Default" Width="54%" ReadOnly="true" Enabled="false">
                                    </cwi:TextBox>
                                    <cwi:LookupExtender ID="lkpFornecedor" OnLookupClick="lkpFornecedor_LookupClick" runat="server"
                                        TargetControlID="txtFornecedor" ImageUrl="~/Images/CWILookupExtender/lookup.gif" />
                                </td>
                            </tr>
                            
                            <tr class="CWIFormViewRow">
                                <td class="CWIFormViewLabel">
                                    <cwi:Label ID="lblSistema" runat="server">Sistema</cwi:Label>
                                </td>
                                <td class="CWIFormViewField" nowrap="nowrap">
                                    <cwi:DropDownList ID="ddlSistema" runat="server" DataTextField="Text" DataValueField="Value"
                                        DefaultValue="1" ShowDefaultFirstItem="false" BindTextToToolTip="true">
                                        <asp:ListItem Text="Walmart Stores" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Sams" Value="2"></asp:ListItem>
                                    </cwi:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </EditItemTemplate>
                </cwi:FormView>
            </fieldset>

            <cwi:GridView ID="grdPrincipal" runat="server" AutoGenerateColumns="False" CssClass="CWIGridView"
                PageSize="25" DataKeyNames="IdItem" DataSourceID="odsGridView" EmptyDataText="Não existem dados a serem exibidos"
                MaxCheckedRows="-1" PageSizeSelectorText="Tamanho da página: " RegistersUntilText="Registros de {0} até {1} de {2}"
                AllowSorting="true" Visible="true" ShowPageSizeSelector="True" ShowCheckBoxes="false" DefaultSortExpression="DescrItem"
                
                AllowExpand="True" ExpandedControlsID="grvItensSub" OnSelectedIndexChanged="grvItens_SelectedIndexChanged"
                OnExpandedIndexChanged="grvItens_ExpandedIndexChanged">
                <FooterStyle CssClass="CWIGridViewFooter" />
                <EmptyDataRowStyle CssClass="CWIGridViewEmptyDataRow" />
                <EditRowStyle CssClass="CWIGridViewEditRow" />
                <Columns>
                    <asp:BoundField HeaderText="Cód. Item" SortExpression="CodItem" DataField="CodItem" 
                        HeaderStyle-Wrap="false" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField HeaderText="UPC" SortExpression="Upc" DataField="Upc" 
                        HeaderStyle-Wrap="false" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField HeaderText="Descrição" SortExpression="DescrItem" DataField="DescrItem" 
                        HeaderStyle-Wrap="false" HeaderStyle-Width="30%" />
                    <asp:TemplateField HeaderText="Situação" SortExpression="Ativo" HeaderStyle-Wrap="false" HeaderStyle-Width="10%">
                        <ItemTemplate>
                            <asp:Label ID="lblAtivo" runat="server"
                                Text='<%# WLMWG.Common.Utils.GetEnumDescription_AtivoInativo(DataBinder.Eval(Container,"DataItem.Ativo").ToString()) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Data Importação" SortExpression="DataImportacao" HeaderStyle-Wrap="false" HeaderStyle-Width="10%">
                        <ItemTemplate>
                            <asp:Label ID="lblDataImportacao" runat="server"
                                Text='<%# WLMWG.Common.Format.DateFormat(DataBinder.Eval(Container,"DataItem.DataImportacao").ToString()) %>'
                                ToolTip='<%# WLMWG.Common.Format.DateFormat(DataBinder.Eval(Container,"DataItem.DataImportacao").ToString()) %>' />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Config." SortExpression="" HeaderStyle-Wrap="false" HeaderStyle-Width="5%">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnConfigLoja" runat="server"
                                ImageUrl="~\Images\CWIGridView\bt_editar.gif" ImageAlign="Middle" Visible="true"
                                CommandArgument='<%# DataBinder.Eval(Container, "DataItem.IdItem").ToString() %>'
                                style="cursor: pointer;" OnClick="btnConfigItem_Click"/>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Estoque" SortExpression="" HeaderStyle-Wrap="false" HeaderStyle-Width="5%">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnConsultarEstoque" runat="server"
                                ImageUrl="~\Images\CWIGridView\bt_editar.gif" ImageAlign="Middle" Visible="true"
                                CommandArgument='<%# DataBinder.Eval(Container, "DataItem.IdItem").ToString() %>'
                                style="cursor: pointer;" /><%--OnClick="btnConsultarEstoque_Click" />--%>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                  </Columns>
                <PagerSettings FirstPageText="Primeira" LastPageText="Última" NextPageText="Próxima"
                    PreviousPageText="Anterior" />
                <RowStyle CssClass="CWIGridViewRow" />
                <CheckBoxHeaderStyle HorizontalAlign="Center" Width="30px" Wrap="False" />
                <SelectedRowStyle CssClass="CWIGridViewSelectedRow" />
                <HeaderStyle CssClass="CWIGridViewHeader" />
                <PagerStyle CssClass="CWIGridViewPager" />
                <ExpandHeaderStyle CssClass="CWIGridViewHeader" HorizontalAlign="Center" Width="3%"
                    Wrap="False" />
                <AlternatingRowStyle CssClass="CWIGridViewAlternatingRow" />
            </cwi:GridView>

            <asp:ObjectDataSource ID="odsGridView" runat="server" SelectMethod="FindAll"
                TypeName="WLMWG.BUS.ItemBus" DataObjectTypeName="WLMWG.BUS.ItemBus"
                SortParameterName="orderBy">
                <SelectParameters>
                    <asp:FormParameter FormField="txtCodigo" Name="CodItem" />
                    <asp:FormParameter FormField="txtUpc" Name="UPC" />
                    <asp:FormParameter FormField="txtDescricao" Name="DescrItem" />
                    <asp:FormParameter FormField="ddlSistema" Name="CodSistema" DefaultValue="1" />
                    <asp:FormParameter FormField="ddlSituacao" Name="Ativo" DefaultValue="S" />
                    <asp:FormParameter FormField="txtCodCentroDistribuicao" Name="CodCentroDistribuicao" />
                    <asp:FormParameter FormField="txtCodLoja" Name="CodLoja" />
                    <asp:FormParameter FormField="txtCodDivisao" Name="CodDivisao" />
                    <asp:FormParameter FormField="hdnIdDepartamento" Name="IdDepartamento" />
                    <asp:FormParameter FormField="hdnIdCategoria" Name="IdCategoria" />
                    <asp:FormParameter FormField="hdnIdSubcategoria" Name="IdSubcategoria" />
                    <asp:FormParameter FormField="hdnIdFineline" Name="IdFineline" />
                    <asp:FormParameter FormField="txtCodFornecedor" Name="CodFornecedor" />
                </SelectParameters>
            </asp:ObjectDataSource>
            
            <div id="divSub" style="overflow-y: hidden; overflow-x: auto; width: 100%;">
                <cwi:GridView runat="server" ShowCheckBoxes="false" DataKeyNames="IdItem" ID="grvItensSub"
                    DataSourceID="odsItensSubgrid" Visible="True" AutoGenerateColumns="False" CssClass="CWIGridView"
                    EmptyDataText="Não existem dados a serem exibidos" MaxCheckedRows="-1"
                    PageSizeSelectorText="Tamanho da página: " RegistersUntilText="Registros de {0} até {1} de {2}"
                    AllowPaging="false" CanSelectRow="false" AllowSorting="false">
                    <FooterStyle CssClass="CWIGridViewFooter" />
                    <EmptyDataRowStyle CssClass="CWIGridViewEmptyDataRow" />
                    <EditRowStyle CssClass="CWIGridViewEditRow" />
                    <Columns>
                        <cwi:AdvancedBoundField HeaderText="Divisão" DataField="DivisaoCodDescr"
                            HeaderStyle-Width="5%" />
                        <cwi:AdvancedBoundField HeaderText="Departamento" DataField="DepartamentoCodDescr"
                            HeaderStyle-Width="5%" />
                        <cwi:AdvancedBoundField HeaderText="Categoria" DataField="CategoriaCodDescr"
                            HeaderStyle-Width="5%" />
                        <cwi:AdvancedBoundField HeaderText="Subcategoria" DataField="SubcategoriaCodDescr"
                            HeaderStyle-Width="5%" />
                        <cwi:AdvancedBoundField HeaderText="Fineline" DataField="FinelineCodDescr"
                            HeaderStyle-Width="5%" />
                        <cwi:AdvancedBoundField HeaderText="Fornecedor" DataField="Fornecedor.NomeFornecedor"
                            HeaderStyle-Width="5%" />
                        <asp:BoundField HeaderText="Cód. Item Fornecedor" DataField="CodItemFornecedor" HeaderStyle-Wrap="false"
                            HeaderStyle-Width="5%" />
                    </Columns>
                    <PagerSettings FirstPageText="Primeira" LastPageText="Última" NextPageText="Próxima"
                        PreviousPageText="Anterior" />
                    <RowStyle CssClass="CWIGridViewRow" />
                    
                    <CheckBoxHeaderStyle HorizontalAlign="Center" Width="30px" Wrap="False" />
                    <SelectedRowStyle CssClass="CWIGridViewSelectedRow" />
                    <HeaderStyle CssClass="CWIGridViewHeader" />
                    <PagerStyle CssClass="CWIGridViewPager" />
                    <ExpandHeaderStyle CssClass="CWIGridViewHeader" HorizontalAlign="Center" Width="20px"
                        Wrap="False" />
                    <AlternatingRowStyle CssClass="CWIGridViewAlternatingRow" />
                </cwi:GridView>
                <asp:ObjectDataSource ID="odsItensSubgrid" runat="server" DataObjectTypeName="WLMWG.BUS.ItemBus"
                    OnSelecting="odsItensSubgrid_Selecting" TypeName="WLMWG.BUS.ItemBus"
                    SelectMethod="FindOneByIdItem">
                    <SelectParameters>
                        <asp:Parameter Type="Int32" Name="IdItem" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>