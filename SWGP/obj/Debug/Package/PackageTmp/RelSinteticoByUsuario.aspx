<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RelSinteticoByUsuario.aspx.cs" Inherits="SWGP.RelSinteticoBuUsuario" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
  <h3>|>> Relatório Sintético por Usuário</h3>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hidItem" />
            <rsweb:ReportViewer ID="rvRelSinteticoByUsuario" runat="server" Font-Names="Verdana" 
                Font-Size="8pt" InteractiveDeviceInfos="(Collection)"  Width="100%" Height="800px"
                WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
                <LocalReport ReportPath="RelatorioSinteticoByUsuario.rdlc">
                    <DataSources>
                        <rsweb:ReportDataSource DataSourceId="odsRelSinteticoByUsuario" 
                            Name="dsRelatorioSinteticoByUsuario" />
                    </DataSources>
                </LocalReport>
            </rsweb:ReportViewer>

            <asp:ObjectDataSource ID="odsRelSinteticoByUsuario" runat="server" 
                SelectMethod="GetData" 
                
                TypeName="SWGP.dsRelatorioAnaliticoTableAdapters.dtGetRelatorioSinteticoByUsuarioTableAdapter" 
                OldValuesParameterFormatString="original_{0}" >
                 <SelectParameters>
                    <asp:Parameter Name="dtFinal"   DefaultValue="2030/12/31"  Type="DateTime"/>
                    <asp:Parameter Name="dtInicial" DefaultValue="2000/01/01"  Type="DateTime"/>
                    <asp:Parameter Name="usuarioNome"  Type="String" DefaultValue=""/>
                 </SelectParameters>
            </asp:ObjectDataSource>

        </ContentTemplate>
  </asp:UpdatePanel>
</asp:Content>
