using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SWGP
{
    public partial class RelSinteticoBuUsuario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            MessageBox.Include(this);

            string nomeUsuario = string.Empty;
            DateTime dtInicial = Convert.ToDateTime("2000/01/01");
            DateTime dtFinal = Convert.ToDateTime("2099/12/31");
            if (!IsPostBack)
            {
                odsRelSinteticoByUsuario.SelectParameters.Clear();

                //se for por usuario o relatorio
                if (Session["idUsuarioRel"] != null)
                {
                    nomeUsuario = Session["idUsuarioRel"].ToString();
                    Session.Remove("idUsuarioRel");
                    odsRelSinteticoByUsuario.SelectParameters.Add("idUsuario", nomeUsuario.ToString());

                    if (Session["dtInicial"] != null)
                    {
                        dtInicial = Convert.ToDateTime(Session["dtInicial"].ToString());
                        odsRelSinteticoByUsuario.SelectParameters.Add("dtInicial", dtInicial.ToString("yyyy/MM/dd"));
                        Session.Remove("dtInicial");
                    }
                    else
                    {
                        odsRelSinteticoByUsuario.SelectParameters.Add("dtInicial", dtInicial.ToString("yyyy/MM/dd"));
                    }


                    if (Session["dtFinal"] != null)
                    {
                        dtFinal = Convert.ToDateTime(Session["dtFinal"].ToString());
                        odsRelSinteticoByUsuario.SelectParameters.Add("dtFinal", dtFinal.ToString("yyyy/MM/dd"));
                        Session.Remove("dtFinal");
                    }
                    else
                    {
                        odsRelSinteticoByUsuario.SelectParameters.Add("dtFinal", dtFinal.ToString("yyyy/MM/dd"));
                    }

                    if (odsRelSinteticoByUsuario == null)
                    {
                        MessageBox.Show("Não há dados para pesquisa realizada.", MessageBox.MessageType.Warning);
                        Response.Redirect("MainRelatorio.aspx");
                    }
                    rvRelSinteticoByUsuario.DataBind();

            }
            
            }
        }
    }
}