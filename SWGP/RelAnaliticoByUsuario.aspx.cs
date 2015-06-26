using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using SWGPgen;


namespace SWGP
{
    public partial class RelAnaliticoByUsuario : Common.BaseWebUi
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            MessageBox.Include(this);

            int idUsuario = 0;
            DateTime dtInicial = Convert.ToDateTime("2000/01/01");
            DateTime dtFinal = Convert.ToDateTime("2099/12/31");
            if (!IsPostBack)
            {
                odsRelAnaliticoByUsuario.SelectParameters.Clear();

                //se for por usuario o relatorio
                if (Session["idUsuarioRel"] != null)
                {
                    idUsuario = int.Parse(Session["idUsuarioRel"].ToString());
                    Session.Remove("idUsuarioRel");
                    odsRelAnaliticoByUsuario.SelectParameters.Add("usuarioNome", new UsuarioControl().GetItem(idUsuario).Nome.ToString());

                    if (Session["dtInicial"] != null)
                    {
                        dtInicial = Convert.ToDateTime(Session["dtInicial"].ToString());
                        odsRelAnaliticoByUsuario.SelectParameters.Add("dtInicial", dtInicial.ToString("yyyy/MM/dd"));
                        Session.Remove("dtInicial");
                    }
                    else
                    {
                        odsRelAnaliticoByUsuario.SelectParameters.Add("dtInicial", dtInicial.ToString("yyyy/MM/dd"));
                    }


                    if (Session["dtFinal"] != null)
                    {
                        dtFinal = Convert.ToDateTime(Session["dtFinal"].ToString());
                        odsRelAnaliticoByUsuario.SelectParameters.Add("dtFinal", dtFinal.ToString("yyyy/MM/dd"));
                        Session.Remove("dtFinal");
                    }
                    else
                    {
                        odsRelAnaliticoByUsuario.SelectParameters.Add("dtFinal", dtFinal.ToString("yyyy/MM/dd"));
                    }


                    rvRelAnaliticoByUsuario.DataBind();
            }
            
            }
        }
    }
}