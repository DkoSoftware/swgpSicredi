<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"  CodeBehind="Default.aspx.cs" Inherits="SWGP._Default" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script type="text/javascript" charset="UTF-8" src="Scripts/core.js"></script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h3>|>> Prospects</h3>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hidItem" />
            <fieldset>
                <%--<input type="reset" name="name" value="Limpar" class="DkoButtonClean" />--%>
                <asp:Button ID="btnNewProspect" Text="Novo" runat="server" CssClass="DkoButtonNew" OnClick="btnNewProspect_Click"/>
                <asp:Button ID="btnEditaProspect" Text="Editar" runat="server" CssClass="DkoButtonEdit"  OnClick="btnEditaProspect_Click"/>
                <asp:Button ID="btnExcluiProspect" Text="Excluir" runat="server" CssClass="DkoButtonDelete"
                    OnClick="btnExcluiProspect_Click" />
                <asp:Button Text="Atualizar" runat="server" id="btnAtualizar" CssClass="DkoButtonRefresh" OnClick="btnAtualizar_Click"/>

                <img src="Images/icon_status_indicacao.png" alt=""/>
                                <a href="MainIndicacao.aspx">Você tem<asp:Label Text=" 0 " runat="server" ID="lblTotIndicacoes" /><asp:Label
                                    ID="Label3" Text=" indicações" runat="server" />
                                </a>


                <input type="button" value="Voltar" onclick="history.go(-1)" class="DkoButtonBack"
                    id="btnVoltar">
            </fieldset>
            <fieldset>
                <legend>Filtro de Pesquisa</legend>
                <table class="DkoFormView">
                    <tr class="DkoFormViewRow" >
                        <td class="DkoFormViewLabel" nowrap="nowrap">
                            <asp:Label Text="Nome Prospect" ID="lblNomeProspect" runat="server" />
                        </td>
                        <td class="DkoFormViewField" colspan="2" nowrap="nowrap">
                            <asp:TextBox runat="server" id="txtNomeProspect" />
                        </td>
                        <td class="DkoFormViewLabel" nowrap="nowrap">
                            <asp:Label Text="Situação Prospect Prospect" ID="lblSitProspect" runat="server" />
                        </td>
                        <td class="DkoFormViewField" colspan="2" nowrap="nowrap">
                             <asp:DropDownList  id="cbSituacaoProspeccao" runat="server" Width="100%" DropDownStyle="DropDownList">
                                <asp:ListItem Text="Todas" Value="Selecione..." />
                                <asp:ListItem Text="Em Andamento" Value="Em Andamento" />
                                <asp:ListItem Text="Encerrada" Value="Encerrada" />
                                <asp:ListItem Text="Associada" Value="Associada" />
                                <asp:ListItem Text="Não Contatado" Value="Sem Contato" />
                                <asp:ListItem Text="Indicação" Value="Indicação" />
                            </asp:DropDownList>
                            <asp:Button ID="btnSearch" Text="Pesquisar" Visible="true" runat="server" CssClass="DkoButtonSearch" OnClick="btnSearch_Click" />
                        </td>

                      <%--<td class="DkoFormViewLabel">
                            <asp:Label Text="Nome Contato" ID="lblNomeContato" runat="server" />
                        </td>--%>
                        <td class="DkoFormViewField" colspan="2">
                            <asp:TextBox runat="server" id="txtNomeContato" Visible="false"/>
                        </td>
                    </tr>
                   <tr class="DkoFormViewRow">
                        <%--<td class="DkoFormViewLabel">
                            <asp:Label Text="Tipo Pessoa" ID="lblTpPessoa" runat="server" Visible="false"/>
                        </td>--%>
                        <td class="DkoFormViewField">
                            <asp:DropDownList runat="server" ID="ddlTipo" Visible="false">
                                <asp:ListItem Text="Todas" />
                                <asp:ListItem Text="PJ" />
                                <asp:ListItem Text="PF" />
                            </asp:DropDownList>
                        </td>
                        <%--<td class="DkoFormViewLabel">
                            <asp:Label Text="CPF" ID="lblCPF" runat="server"  Visible="false"/>
                        </td>--%>
                        <td class="DkoFormViewField">
                            <asp:TextBox runat="server" id="txtCPF" Visible="false" />
                            <asp:MaskedEditExtender Mask="999999999-99" MaskType="Number" TargetControlID="txtCPF" runat="server" />
                        </td>
                        <%--<td class="DkoFormViewLabel">
                            <asp:Label Text="CNPJ" ID="lblCNPJ" runat="server"  Visible="false"/>
                        </td>--%>
                        <td class="DkoFormViewField">
                            <asp:TextBox runat="server" id="txtCnpj" Visible="false"/>
                            <asp:MaskedEditExtender ID="MaskedEditExtender1" Mask="99999999/9999-99" MaskType="Number"
                                TargetControlID="txtCnpj" runat="server" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <asp:HiddenField ID="hfIdProspect" runat="server" />
            <asp:GridView ID="gvPrincipal" runat="server"  AllowSorting="false" AllowPaging="true" PagerStyle-HorizontalAlign="Center"
                AutoGenerateColumns="False" CssClass="DkoGridView" onrowdatabound="gvPrincipal_RowDataBound" EnableSortingAndPagingCallbacks="true"
                EmptyDataText="Não existem dados a serem exibidos" FooterStyle-CssClass="DkoGridViewFooter" PagerSettings-Position="Bottom"
                HeaderStyle-CssClass="DkoGridViewHeader" ShowFooter="True" OnPageIndexChanging="gvPrincipal_PageIndexChanging" 
                DataKeyNames="idProspect"  PagerSettings-Mode="NextPreviousFirstLast"
                PageSize="20"  PagerStyle-CssClass="DkoGridViewPager" PagerSettings-Visible="true" PageIndex="1" >
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
                    <asp:BoundField DataField="idProspect" HeaderText="ID" visible="false" SortExpression="idProspect" />
                    <asp:BoundField DataField="Nome" HeaderText="Nome" SortExpression="" HeaderStyle-Width="20%" />
                    <asp:BoundField DataField="Telefone" HeaderText="Telefone" SortExpression=""
                        HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="" HeaderStyle-Width="5%"
                        ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="" HeaderStyle-Width="15%"
                        ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="PessoaContato" HeaderText="Contato" SortExpression=""
                        HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="DataContato" HeaderText="Data Ultimo Contato" SortExpression="" />
                     <asp:BoundField DataField="SituacaoProspect" HeaderText="Situação" SortExpression=""
                        HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Left" />
                     <asp:TemplateField HeaderText="Situação" SortExpression="" Visible="false">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="litSituacao"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Contato" SortExpression="" HeaderStyle-Wrap="false"
                        HeaderStyle-Width="5%">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnNovoContato" runat="server" ImageUrl="~/Images/DkoGridView/add1.png"
                                ImageAlign="Middle" Visible="true" style="cursor: pointer;" OnClick="btnNovoContato_Click" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Histórico" SortExpression="" HeaderStyle-Wrap="false"
                        HeaderStyle-Width="5%">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnHistorico" runat="server" ImageUrl="~/Images/DkoGridView/historic1.png"
                                ImageAlign="Middle" Visible="true" style="cursor: pointer;" OnClick="btnHistorico_Click" />
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

            <%--modal novo Contato--%>
            <asp:HiddenField id="hfNovoContato" runat="server" />
            <asp:ModalPopupExtender ID="mpeNovoContato" runat="server" TargetControlID="hfNovoContato" 
                PopupControlID="panelNovoContato" BackgroundCssClass="DkoModalPopupExtanderBackground"
                PopupDragHandleControlID="panelNovoContato">
            </asp:ModalPopupExtender>
            <asp:Panel ID="panelNovoContato" runat="server" CssClass="DkoModalPopupContentPanel" BorderStyle="Groove" BorderColor="Gray" BorderWidth="9"
                Width="55%" Height="50%">
               <div class="barModals">
                <p>|>> Cadastro de Contato</p>
               </div>
                <fieldset>
                <asp:Button Text="Salvar" runat="server" id="btnSalvar" CssClass="DkoButtonSave"
                    OnClick="btnSalvar_Click" />
                <input type="reset" name="name" value="Limpar" class="DkoButtonClean" />
                <asp:Button Text="Voltar" id="btnVoltarNovoContato" CssClass="DkoButtonBack" runat="server" />
            </fieldset>
            <fieldset>
                <legend>Dados do Contato</legend>
                <table class="DkoFormView">
                    <tr class="DkoFormViewRow">
                        <td class="DkoFormViewLabel">
                            <asp:Label ID="Label1" Text="Nome Prospect" runat="server" />
                        </td>
                        <td class="DkoFormViewField" colspan="2">
                            <asp:TextBox runat="server" id="txtNomeProspectMod" />
                        </td>
                        <td>
                            <asp:Label ID="Label2" runat="server" text="Data Contato:"></asp:Label>
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
                            <asp:DropDownList runat="server" ID="ddlTipoNovoContato">
                                <asp:ListItem Text="Telefonema" />
                                <asp:ListItem Text="UA" />
                                <asp:ListItem Text="Visita" />
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
                            <asp:TextBox runat="server" id="txtNumConta"  Enabled="false"/>
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
               
            </asp:Panel>
            <%--fim modal novo contato--%>
            <%--modal Historico--%>
            <asp:ModalPopupExtender ID="mpeHistorico" runat="server" TargetControlID="linkHistorico"
                PopupControlID="panHistorico" BackgroundCssClass="DkoModalPopupExtanderBackground"
                CancelControlID="btnCloseHistorico" PopupDragHandleControlID="panHistorico">
            </asp:ModalPopupExtender>
            <asp:Panel ID="panHistorico" runat="server" CssClass="DkoModalPopupContentPanel" BorderStyle="Groove" BorderColor="Gray" BorderWidth="9"
                Width="85%" Height="500px">
                <iframe id="Iframe1" src="HistoricoContatos.aspx" scrolling="yes" width="100%" height="100%"
                    frameborder="1" class="DkoModalPopupContentPanel" title="Cadastro de Contato">
                    <asp:Button ID="Button2" runat="server" Text="Close" Visible="false" />
                </iframe>
                <asp:Button ID="btnCloseHistorico" runat="server" Text="Fechar" CssClass="DkoButtonCloseLookup" />
                <a id="linkHistorico" runat="server"></a>
            </asp:Panel>
            <%--fim modal novo prospect--%>
            
            <%--modal Novo Prospect--%>
            <asp:HiddenField  id="hfNovoprospect" runat="server" />
            <asp:ModalPopupExtender ID="mpeNovoProspect" runat="server" TargetControlID="hfNovoprospect"
                PopupControlID="panNovoProspect" BackgroundCssClass="DkoModalPopupExtanderBackground"
               PopupDragHandleControlID="panNovoProspect">
            </asp:ModalPopupExtender>
            <asp:Panel ID="panNovoProspect" runat="server" CssClass="DkoModalPopupContentPanel" BorderStyle="Groove" BorderColor="Gray" BorderWidth="9"
                Width="80%" Height="60%">
                <div class="barModals">
                <p>|>> Cadastro de Prospect</p>
               </div>
                 <asp:HiddenField runat="server" ID="HiddenField1" />
                   <fieldset>
                        <asp:Button Text="Salvar" runat="server" id="btnSalvarNovoProspect" CssClass="DkoButtonSave" OnClick="btnSalvarNovoProspect_Click"/>
                        <asp:Button Text="Limpar" runat="server" id="btnLimparNovoProspect" CssClass="DkoButtonClean" OnClick="btnLimparNovoProspect_Click"/>
                         <asp:Button Text="Voltar" id="Button1" CssClass="DkoButtonBack" runat="server" />
                    </fieldset>
                    <fieldset>
                    <table class="DkoFormView">
                    <tr class="DkoFormViewRow">
                        <td class="DkoFormViewLabel" >
                            <asp:Label ID="Label4" Text="Nome Prospect" runat="server" />
                        </td>
                        <td class="DkoFormViewField" colspan="3">
                            <asp:TextBox runat="server" id="txtNomeNovoProspect"/>
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
                            <asp:TextBox runat="server" id="txtCPFNovoProspect"/>
                            <asp:MaskedEditExtender id="meeCPF" Mask="999999999-99" MaskType="Number" TargetControlID="txtCPFNovoProspect" runat="server" />
                        </td>
                        <td class="DkoFormViewLabel">
                            <asp:Label ID="lblCNPJ" Text="CNPJ" runat="server" />
                        </td>
                        <td class="DkoFormViewField" colspan="2">
                            <asp:TextBox runat="server" id="txtCNPJNovoProspect"/>
                            <asp:MaskedEditExtender id="meeCNPJ" runat="server" Mask="99.999.999/9999-99" MaskType="Number" TargetControlID="txtCNPJNovoProspect" />
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
            </asp:Panel>
            <%--fim modal novo prospect--%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
