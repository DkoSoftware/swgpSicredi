using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SWGPgen;

namespace SWGP
{
    public partial class MainUA1 : Common.BaseWebUi
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           // MessageBox.Include(this);
            MessageBox.Include(this);
            
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            UAControl uaDal = new UAControl();
            UAFields uaObj = new UAFields();
            UAValidator uaBus = new UAValidator();

            uaObj.Nome = txtNomeUA.Text;
            try
            {
                if (uaBus.isValid(uaObj))
                {
                    uaDal.Add(ref uaObj);
                    MessageBox.Show("Unidade de Atendimento Cadastrada com sucesso.", MessageBox.MessageType.Success);
                    txtNomeUA.Text = string.Empty;
                    gvPrincipal.DataBind();
                }
                else
                {
                    MessageBox.Show("Campo Nome UA é de preenchimento obrigatório", MessageBox.MessageType.Info);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageBox.MessageType.Error);
            }

        }


        //protected void GridViewGetSelectingValue(object sender, GridViewSelectEventArgs e) 
        //{
        //    string id = gvPrincipal.DataKeys[e.NewSelectedIndex].Value.ToString();
        //}

        protected void btnExcluir_Click(object sender, EventArgs e)
        {

            try
            {
                UAControl uaDal = new UAControl();
                ImageButton imgButton;
                imgButton = (ImageButton)sender; //converter objeto para checkbox
                GridViewRow row = (GridViewRow)imgButton.Parent.Parent; // pegar a linha pai desta checkbox
                int idUA = Convert.ToInt32(gvPrincipal.DataKeys[row.RowIndex]["idUA"].ToString());//pegar o código da datakey da linha
                uaDal.Delete(Convert.ToInt32(idUA));

                MessageBox.Show("Unidade de Atendimento apagada com sucesso.", MessageBox.MessageType.Success);
                gvPrincipal.DataBind();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageBox.MessageType.Error);
            }

        }
                       
    }
}