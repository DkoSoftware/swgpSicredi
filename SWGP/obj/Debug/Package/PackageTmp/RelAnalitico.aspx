<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RelAnalitico.aspx.cs" Inherits="SWGP.RelAnalitico1" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h3>|>> Relatório Analítico</h3>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hidItem" />
            
            <rsweb:ReportViewer ID="rvRelAnalitico" runat="server" Font-Names="Verdana" 
                Font-Size="8pt" InteractiveDeviceInfos="(Collection)"  Width="100%" Height="800px"
                WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
                <LocalReport ReportPath="Report1.rdlc">
                    <DataSources>
                        <rsweb:ReportDataSource DataSourceId="odsRelAnalitico" 
                            Name="DataSet1" />
                    </DataSources>
                </LocalReport>
            </rsweb:ReportViewer>

            <asp:ObjectDataSource ID="odsRelAnalitico" runat="server" 
                SelectMethod="GetData" 
                
                TypeName="SWGP.dsRelatorioAnaliticoTableAdapters.dtGetRelatorioAnaliticoTableAdapter" 
                OldValuesParameterFormatString="original_{0}" >
                 <SelectParameters>
                     <asp:Parameter DefaultValue="01/01/2000" Name="dtInicial" Type="DateTime" />
                    <asp:Parameter Name="dtFinal"   DefaultValue="31/12/2030"  Type="DateTime"/>
                    <asp:Parameter Name="UA"  Type="Int32" DefaultValue="0"/>
                     <asp:Parameter Name="UA2" Type="Int32" DefaultValue="0" />
                     <asp:Parameter Name="UA3" Type="Int32" DefaultValue="0" />
                     <asp:Parameter Name="UA4" Type="Int32" DefaultValue="0" />
                     <asp:Parameter Name="UA5" Type="Int32" DefaultValue="0" />
                     <asp:Parameter Name="UA6" Type="Int32" DefaultValue="0" />
                     <asp:Parameter Name="UA7" Type="Int32" DefaultValue="0" />
                     <asp:Parameter Name="UA8" Type="Int32" DefaultValue="0" />
                     <asp:Parameter Name="UA9" Type="Int32" DefaultValue="0" />
                     <asp:Parameter Name="UA10" Type="Int32" DefaultValue="0" />
                     <asp:Parameter Name="UA11" Type="Int32" DefaultValue="0" />
                     <asp:Parameter Name="UA12" Type="Int32" DefaultValue="0" />
                     <asp:Parameter Name="UA13" Type="Int32" DefaultValue="0" />
                     <asp:Parameter Name="UA14" Type="Int32" DefaultValue="0" />
                     <asp:Parameter Name="UA15" Type="Int32" DefaultValue="0" />
                     <asp:Parameter Name="UA16" Type="Int32" DefaultValue="0" />
                     <asp:Parameter Name="UA17" Type="Int32" DefaultValue="0" />
                     <asp:Parameter Name="UA18" Type="Int32" DefaultValue="0" />
                     <asp:Parameter Name="UA19" Type="Int32" DefaultValue="0" />
                     <asp:Parameter Name="UA20" Type="Int32" DefaultValue="0" />
                     <asp:Parameter Name="UA21" Type="Int32" DefaultValue="0" />
                     <asp:Parameter Name="UA22" Type="Int32" DefaultValue="0" />
                     <asp:Parameter Name="UA23" Type="Int32" DefaultValue="0" />
                     <asp:Parameter Name="UA24" Type="Int32" DefaultValue="0" />
                     <asp:Parameter Name="UA25" Type="Int32" DefaultValue="0" />
                     <asp:Parameter Name="UA26" Type="Int32" DefaultValue="0" />
                     <asp:Parameter Name="UA27" Type="Int32" DefaultValue="0" />
                     <asp:Parameter Name="UA28" Type="Int32" DefaultValue="0" />
                     <asp:Parameter Name="UA29" Type="Int32" DefaultValue="0" />
                     <asp:Parameter Name="UA30" Type="Int32" DefaultValue="0" />
                 </SelectParameters>
            </asp:ObjectDataSource>

        </ContentTemplate>
  </asp:UpdatePanel>
</asp:Content>
