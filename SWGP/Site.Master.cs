using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SWGPgen;

namespace SWGP
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UsuarioFields usuarioCurrent = (UsuarioFields)Session["usuarioObj"];
            ValidateUser(usuarioCurrent.Modulo);

        }

        protected void img_logo_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }

        private void ValidateUser(string modulo)
        {
            switch (modulo)
            {
                case "U":
                    this.NavigationMenu.Items[3].Enabled = false;
                    this.NavigationMenu.Items[4].Enabled = false;
                    this.NavigationMenu.Items[5].Enabled = false;
                    break;

                case "A":
                    this.NavigationMenu.Items[3].Enabled = false;
                    this.NavigationMenu.Items[4].Enabled = false;
                    this.NavigationMenu.Items[5].Enabled = false;
                    break;

                case "M":
                    break;
            }
        }
    }
}
