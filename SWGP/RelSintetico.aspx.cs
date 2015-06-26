using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;

namespace SWGP
{
    public partial class RelSintetico : Common.BaseWebUi
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MessageBox.Include(this);

            string nomeUsuario = string.Empty;
            DateTime dtInicial = Convert.ToDateTime("2000/01/01");
            DateTime dtFinal = Convert.ToDateTime("2099/12/31");

            odsRelSintetico.SelectParameters.Clear();


            //caso for todos usuarios
            if (Session["dtInicial"] != null)
            {
                dtInicial = Convert.ToDateTime(Session["dtInicial"].ToString());
                odsRelSintetico.SelectParameters.Add("dtInicial", dtInicial.ToString("yyyy/MM/dd"));
                Session.Remove("dtInicial");
            }
            else
            {
                odsRelSintetico.SelectParameters.Add("dtInicial", dtInicial.ToString("yyyy/MM/dd"));
            }



            if (Session["dtFinal"] != null)
            {
                dtFinal = Convert.ToDateTime(Session["dtFinal"].ToString());
                odsRelSintetico.SelectParameters.Add("dtFinal", dtFinal.ToString("yyyy/MM/dd"));
                Session.Remove("dtFinal");
            }
            else
            {
                odsRelSintetico.SelectParameters.Add("dtFinal", dtFinal.ToString("yyyy/MM/dd"));
            }

            int idUA = 0;
            if(int.TryParse(Session["idUA"].ToString(),out idUA))
            {
                odsRelSintetico.SelectParameters.Add("UA",idUA.ToString());//,,,,,UA6,UA7,UA8,UA9,UA10,UA11,UA12,UA13,UA14,UA15,UA16,UA17,UA18,UA19,UA20,UA21,UA22,UA23,UA24,UA25,UA26,UA27,UA28,UA29,UA30", Session["idUA"].ToString());
                odsRelSintetico.SelectParameters.Add("UA2", "0");
                odsRelSintetico.SelectParameters.Add("UA3", "0");
                odsRelSintetico.SelectParameters.Add("UA4", "0");
                odsRelSintetico.SelectParameters.Add("UA5", "0");
                odsRelSintetico.SelectParameters.Add("UA6", "0");
                odsRelSintetico.SelectParameters.Add("UA7", "0");
                odsRelSintetico.SelectParameters.Add("UA8", "0");
                odsRelSintetico.SelectParameters.Add("UA9", "0");
                odsRelSintetico.SelectParameters.Add("UA10", "0");
                odsRelSintetico.SelectParameters.Add("UA11", "0");
                odsRelSintetico.SelectParameters.Add("UA12", "0");
                odsRelSintetico.SelectParameters.Add("UA13", "0");
                odsRelSintetico.SelectParameters.Add("UA14", "0");
                odsRelSintetico.SelectParameters.Add("UA15", "0");
                odsRelSintetico.SelectParameters.Add("UA16", "0");
                odsRelSintetico.SelectParameters.Add("UA17", "0");
                odsRelSintetico.SelectParameters.Add("UA18", "0");
                odsRelSintetico.SelectParameters.Add("UA19", "0");
                odsRelSintetico.SelectParameters.Add("UA20", "0");
                odsRelSintetico.SelectParameters.Add("UA21", "0");
                odsRelSintetico.SelectParameters.Add("UA22", "0");
                odsRelSintetico.SelectParameters.Add("UA23", "0");
                odsRelSintetico.SelectParameters.Add("UA24", "0");
                odsRelSintetico.SelectParameters.Add("UA25", "0");
                odsRelSintetico.SelectParameters.Add("UA26", "0");
                odsRelSintetico.SelectParameters.Add("UA27", "0");
                odsRelSintetico.SelectParameters.Add("UA28", "0");
                odsRelSintetico.SelectParameters.Add("UA29", "0");
                odsRelSintetico.SelectParameters.Add("UA30", "0");
            }
            else
            {
                odsRelSintetico.SelectParameters.Add("UA", "1");//,,,,,UA6,UA7,UA8,UA9,UA10,UA11,UA12,UA13,UA14,UA15,UA16,UA17,UA18,UA19,UA20,UA21,UA22,UA23,UA24,UA25,UA26,UA27,UA28,UA29,UA30", Session["idUA"].ToString());
                odsRelSintetico.SelectParameters.Add("UA2", "2");
                odsRelSintetico.SelectParameters.Add("UA3", "3");
                odsRelSintetico.SelectParameters.Add("UA4", "4");
                odsRelSintetico.SelectParameters.Add("UA5", "5");
                odsRelSintetico.SelectParameters.Add("UA6", "6");
                odsRelSintetico.SelectParameters.Add("UA7", "7");
                odsRelSintetico.SelectParameters.Add("UA8", "8");
                odsRelSintetico.SelectParameters.Add("UA9", "9");
                odsRelSintetico.SelectParameters.Add("UA10", "10");
                odsRelSintetico.SelectParameters.Add("UA11", "11");
                odsRelSintetico.SelectParameters.Add("UA12", "12");
                odsRelSintetico.SelectParameters.Add("UA13", "13");
                odsRelSintetico.SelectParameters.Add("UA14", "14");
                odsRelSintetico.SelectParameters.Add("UA15", "15");
                odsRelSintetico.SelectParameters.Add("UA16", "16");
                odsRelSintetico.SelectParameters.Add("UA17", "17");
                odsRelSintetico.SelectParameters.Add("UA18", "18");
                odsRelSintetico.SelectParameters.Add("UA19", "19");
                odsRelSintetico.SelectParameters.Add("UA20", "20");
                odsRelSintetico.SelectParameters.Add("UA21", "21");
                odsRelSintetico.SelectParameters.Add("UA22", "22");
                odsRelSintetico.SelectParameters.Add("UA23", "23");
                odsRelSintetico.SelectParameters.Add("UA24", "24");
                odsRelSintetico.SelectParameters.Add("UA25", "25");
                odsRelSintetico.SelectParameters.Add("UA26", "26");
                odsRelSintetico.SelectParameters.Add("UA27", "27");
                odsRelSintetico.SelectParameters.Add("UA28", "28");
                odsRelSintetico.SelectParameters.Add("UA29", "29");
                odsRelSintetico.SelectParameters.Add("UA30", "30");
            }
            

            rvRelSintetico.DataBind();

        }
    }
}