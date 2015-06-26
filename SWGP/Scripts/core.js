function setItem(c) {
    if ((c) != null) {
        if (document.forms[0].rdoItem != null && document.forms[0].rdoItem.length != null) {
            for (i = 0; i < document.forms[0].rdoItem.length; i++) {
                if (document.forms[0].rdoItem[i].checked) {
                    document.getElementById(c).value = i;
                    break;
                }
            }
        }
        else
            document.getElementById(c).value = 0;
    }
};


function ValidateData(sender) {
    var val = Page_ClientValidate();
    if (!val) {
        var i = 0;
        for (; i < Page_Validators.length; i++) {
            if (!Page_Validators[i].isvalid) {
                if (sender == Page_Validators[i].controltovalidate) {
                    document.getElementById(Page_Validators[i].controltovalidate).style.borderColor = '#FF0000';
                    document.getElementById(Page_Validators[i].controltovalidate).style.borderLeftStyle = "outset";
                    document.getElementById(Page_Validators[i].controltovalidate).style.borderTopStyle = "outset";
                }
                else {
                    document.getElementById(Page_Validators[i].controltovalidate).style.borderColor = '';
                    document.getElementById(Page_Validators[i].controltovalidate).style.borderLeftStyle = "outset";
                    document.getElementById(Page_Validators[i].controltovalidate).style.borderTopStyle = "outset";
                }
            }
        }
    }
    return val;
};
