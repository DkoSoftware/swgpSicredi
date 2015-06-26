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
    public partial class RelAnalitico :Common.BaseWebUi
    {
        private string ddlUaValue;

        protected void Page_Load(object sender, EventArgs e)
        {
            MessageBox.Include(this);
            ddlUaValue = ddlUA.SelectedValue;

            if (!IsPostBack)
            {
                
                BoundDropDownUA();

                if (Session["usuarioObj"] != null)
                {
                    UsuarioFields usuarioObj = (UsuarioFields)Session["usuarioObj"];

                    switch (usuarioObj.Modulo)
                    {
                        case "M":
                            break;

                        case "U":
                            BoundDropDownUA();
                            this.ddlUA.Items.FindByText(new UAControl().GetItem(usuarioObj.FkUa).Nome.ToString()).Selected = true;
                            this.ddlUA.Enabled = false;
                            ddlUaValue = usuarioObj.FkUa.ToString();

                            BoundDropDownUsuario();
                            this.ddlUsuario.Items.FindByValue(usuarioObj.idUsuario.ToString()).Selected = true;
                            this.ddlUsuario.Enabled = false;
                            this.cbTotUsuarios.Enabled = false;
                            break;

                        case "A":
                            BoundDropDownUA();
                            this.ddlUA.Items.FindByText(new UAControl().GetItem(usuarioObj.FkUa).Nome.ToString()).Selected = true;
                            this.ddlUA.Enabled = false;
                            ddlUaValue = ddlUA.Items.FindByText(new UAControl().GetItem(usuarioObj.FkUa).Nome).Value;
                            BoundDropDownUsuario();
                            break;
                    }
                }
            }
            

        }

        private void BoundDropDownUA()
        {
            ddlUA.ClearSelection();
            ddlUA.DataTextField = "Nome";
            ddlUA.DataValueField = "idUA";
            ddlUA.DataSource = new UAControl().GetAll();
            ddlUA.DataBind();
            ddlUA.Items.Insert(0, (new ListItem("Selecione...")));

        }

        private void BoundDropDownUsuario()
        {
            UsuarioControl usuarios = new UsuarioControl();
            ddlUsuario.ClearSelection();
            ddlUsuario.DataTextField = "UserName";
            ddlUsuario.DataValueField = "idUsuario";
            ddlUsuario.DataSource = usuarios.GetAllUsersByUA(int.Parse(ddlUaValue));
            ddlUsuario.DataBind();
            this.ddlUA.Items.FindByValue(ddlUaValue).Selected = true;

        }

        protected void ddlUA_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            BoundDropDownUsuario();
        }

        protected void btnGeraRelatorio_Click(object sender,EventArgs e)
        {
            try
            {
                if (ddlTipoRelatorio.SelectedItem.Text == "Selecione...")
                {
                    MessageBox.Show("Favor selecionar um tipo de relatório.", MessageBox.MessageType.Info);
                    return;
                }

                UsuarioControl usuarioDal = new UsuarioControl();

                if (cbTotUsuarios.Checked == false)
                    Session["idUsuarioRel"] = usuarioDal.FindByUserName(ddlUsuario.SelectedItem.Text).idUsuario;

                if (!string.IsNullOrEmpty(txtDtInicio.Text))
                    Session["dtInicial"] = txtDtInicio.Text;

                if (!string.IsNullOrEmpty(txtDtFim.Text))
                    Session["dtFinal"] = txtDtFim.Text;

                Session["TipoRelatorio"] = ddlTipoRelatorio.SelectedItem.Text;

                if (new UAControl().GetItem(int.Parse(ddlUA.SelectedValue)).Nome.ToUpper().Equals("SUREG"))
                    Session["idUA"] = "SUREG";
                else
                    Session["idUA"] = ddlUA.SelectedValue;

                
                //relatorio analitico
                if (ddlTipoRelatorio.SelectedItem.Text == "Analítico" && cbTotUsuarios.Checked == true)
                    Response.Redirect("~/RelAnalitico.aspx");
                else if (ddlTipoRelatorio.SelectedItem.Text == "Analítico" && cbTotUsuarios.Checked == false)
                    Response.Redirect("~/RelAnaliticoByUsuario.aspx");

                //relatorio sintetico
                else if (ddlTipoRelatorio.SelectedItem.Text == "Sintético" && cbTotUsuarios.Checked == true)
                    Response.Redirect("~/RelSintetico.aspx");
                else
                    Response.Redirect("~/RelSinteticoByUsuario.aspx");

                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageBox.MessageType.Error);
            }
            
            
 
        }
    }


}