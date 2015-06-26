using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// Summary description for BaseWebUi
/// </summary>
namespace Common
{
    public class BaseWebUi : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            //se o div de Aguarde ainda estiver mostrando ele tira
            System.Web.UI.ScriptManager src = ScriptManager.GetCurrent(Page);
            if (src != null)
                ScriptManager.RegisterClientScriptBlock(this, typeof(void), "TiraDivAguarde", "if(document.getElementById('divProcessando'))document.getElementById('divProcessando').style.display = 'none';", true);
            else
                ClientScript.RegisterStartupScript(typeof(Page), "TiraDivAguarde", "if(document.getElementById('divProcessando'))document.getElementById('divProcessando').style.display = 'none';", true);

            ClientScript.RegisterOnSubmitStatement(this.GetType(), "zerarfiltro", "if(document.getElementById('divProcessando') && document.getElementById('divProcessando').style.display!='none')return false;");

            ClientScript.RegisterOnSubmitStatement(this.GetType(), "Aguarde", "if (typeof(ValidatorOnSubmit) == 'function' && ValidatorOnSubmit() == false) return false; avisoAguarde();");

            base.OnInit(e);
        }
    }
}