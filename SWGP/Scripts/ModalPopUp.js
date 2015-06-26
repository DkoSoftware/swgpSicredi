<script language="javascript" type="text/javascript">
    function ShowEditModal(ExpanseID) {
        var frame = $get('IframeEdit');
        frame.src = "IframeNovoContato.aspx?UIMODE=EDIT&EID=" + ExpanseID;
        $find('EditModalPopup').show();
    }
    function EditCancelScript() {
        var frame = $get('IframeEdit');
        frame.src = "DemoLoading.aspx";
    }
    function EditOkayScript() {
        RefreshDataGrid();
        EditCancelScript();
    }
    function RefreshDataGrid() {
        $get('btnSearch').click();
    }
</script>