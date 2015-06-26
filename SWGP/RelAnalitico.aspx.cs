using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SWGPgen;
using System.Data;
using Microsoft.Reporting.WebForms;

namespace SWGP
{
    public partial class RelAnalitico1 : Common.BaseWebUi
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MessageBox.Include(this);

            string nomeUsuario = string.Empty;
            DateTime dtInicial = Convert.ToDateTime("2000/01/01");
            DateTime dtFinal = Convert.ToDateTime("2099/12/31");

            odsRelAnalitico.SelectParameters.Clear();

            try
            {
                //caso for todos usuarios
                if (Session["dtInicial"] != null)
                {
                    dtInicial = Convert.ToDateTime(Session["dtInicial"].ToString());
                    odsRelAnalitico.SelectParameters.Add("dtInicial", dtInicial.ToString("yyyy/MM/dd"));
                    Session.Remove("dtInicial");
                }
                else
                {
                    odsRelAnalitico.SelectParameters.Add("dtInicial", dtInicial.ToString("yyyy/MM/dd"));
                }



                if (Session["dtFinal"] != null)
                {
                    dtFinal = Convert.ToDateTime(Session["dtFinal"].ToString());
                    odsRelAnalitico.SelectParameters.Add("dtFinal", dtFinal.ToString("yyyy/MM/dd"));
                    Session.Remove("dtFinal");
                }
                else
                {
                    odsRelAnalitico.SelectParameters.Add("dtFinal", dtFinal.ToString("yyyy/MM/dd"));
                }

                int idUA = 0;
                if (int.TryParse(Session["idUA"].ToString(), out idUA))
                {
                    odsRelAnalitico.SelectParameters.Add("UA", idUA.ToString());//,,,,,UA6,UA7,UA8,UA9,UA10,UA11,UA12,UA13,UA14,UA15,UA16,UA17,UA18,UA19,UA20,UA21,UA22,UA23,UA24,UA25,UA26,UA27,UA28,UA29,UA30", Session["idUA"].ToString());
                    odsRelAnalitico.SelectParameters.Add("UA2", "0");
                    odsRelAnalitico.SelectParameters.Add("UA3", "0");
                    odsRelAnalitico.SelectParameters.Add("UA4", "0");
                    odsRelAnalitico.SelectParameters.Add("UA5", "0");
                    odsRelAnalitico.SelectParameters.Add("UA6", "0");
                    odsRelAnalitico.SelectParameters.Add("UA7", "0");
                    odsRelAnalitico.SelectParameters.Add("UA8", "0");
                    odsRelAnalitico.SelectParameters.Add("UA9", "0");
                    odsRelAnalitico.SelectParameters.Add("UA10", "0");
                    odsRelAnalitico.SelectParameters.Add("UA11", "0");
                    odsRelAnalitico.SelectParameters.Add("UA12", "0");
                    odsRelAnalitico.SelectParameters.Add("UA13", "0");
                    odsRelAnalitico.SelectParameters.Add("UA14", "0");
                    odsRelAnalitico.SelectParameters.Add("UA15", "0");
                    odsRelAnalitico.SelectParameters.Add("UA16", "0");
                    odsRelAnalitico.SelectParameters.Add("UA17", "0");
                    odsRelAnalitico.SelectParameters.Add("UA18", "0");
                    odsRelAnalitico.SelectParameters.Add("UA19", "0");
                    odsRelAnalitico.SelectParameters.Add("UA20", "0");
                    odsRelAnalitico.SelectParameters.Add("UA21", "0");
                    odsRelAnalitico.SelectParameters.Add("UA22", "0");
                    odsRelAnalitico.SelectParameters.Add("UA23", "0");
                    odsRelAnalitico.SelectParameters.Add("UA24", "0");
                    odsRelAnalitico.SelectParameters.Add("UA25", "0");
                    odsRelAnalitico.SelectParameters.Add("UA26", "0");
                    odsRelAnalitico.SelectParameters.Add("UA27", "0");
                    odsRelAnalitico.SelectParameters.Add("UA28", "0");
                    odsRelAnalitico.SelectParameters.Add("UA29", "0");
                    odsRelAnalitico.SelectParameters.Add("UA30", "0");
                }
                else
                {
                    odsRelAnalitico.SelectParameters.Add("UA", "1");
                    odsRelAnalitico.SelectParameters.Add("UA2", "2");
                    odsRelAnalitico.SelectParameters.Add("UA3", "3");
                    odsRelAnalitico.SelectParameters.Add("UA4", "4");
                    odsRelAnalitico.SelectParameters.Add("UA5", "5");
                    odsRelAnalitico.SelectParameters.Add("UA6", "6");
                    odsRelAnalitico.SelectParameters.Add("UA7", "7");
                    odsRelAnalitico.SelectParameters.Add("UA8", "8");
                    odsRelAnalitico.SelectParameters.Add("UA9", "9");
                    odsRelAnalitico.SelectParameters.Add("UA10", "10");
                    odsRelAnalitico.SelectParameters.Add("UA11", "11");
                    odsRelAnalitico.SelectParameters.Add("UA12", "12");
                    odsRelAnalitico.SelectParameters.Add("UA13", "13");
                    odsRelAnalitico.SelectParameters.Add("UA14", "14");
                    odsRelAnalitico.SelectParameters.Add("UA15", "15");
                    odsRelAnalitico.SelectParameters.Add("UA16", "16");
                    odsRelAnalitico.SelectParameters.Add("UA17", "17");
                    odsRelAnalitico.SelectParameters.Add("UA18", "18");
                    odsRelAnalitico.SelectParameters.Add("UA19", "19");
                    odsRelAnalitico.SelectParameters.Add("UA20", "20");
                    odsRelAnalitico.SelectParameters.Add("UA21", "21");
                    odsRelAnalitico.SelectParameters.Add("UA22", "22");
                    odsRelAnalitico.SelectParameters.Add("UA23", "23");
                    odsRelAnalitico.SelectParameters.Add("UA24", "24");
                    odsRelAnalitico.SelectParameters.Add("UA25", "25");
                    odsRelAnalitico.SelectParameters.Add("UA26", "26");
                    odsRelAnalitico.SelectParameters.Add("UA27", "27");
                    odsRelAnalitico.SelectParameters.Add("UA28", "28");
                    odsRelAnalitico.SelectParameters.Add("UA29", "29");
                    odsRelAnalitico.SelectParameters.Add("UA30", "30");
                }

                rvRelAnalitico.DataBind();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageBox.MessageType.Error);
            }
           
    
        }
    }
}