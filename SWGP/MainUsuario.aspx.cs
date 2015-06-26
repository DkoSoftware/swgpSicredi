using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SWGPgen;
using DKO.Framework;
using System.Data;

namespace SWGP
{
    public partial class MainUsuario : Common.BaseWebUi
    {

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            hidItem.Value = String.Empty;
            MessageBox.Include(this);
        }

        #endregion

        #region Events

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            gvPrincipal.DataBind();
        }

        protected void btnBloquear_Click(object sender, EventArgs e)
        {

            try
            {
                UsuarioControl usuarioDal = new UsuarioControl();
                UsuarioFields usuarioObj = new UsuarioFields();
                ImageButton imgBtn;
                imgBtn = (ImageButton)sender; //converter objeto para imagebutton
                GridViewRow row = (GridViewRow)imgBtn.Parent.Parent; // pegar a linha pai desta imagebutton
                int idUsuario = Convert.ToInt32(gvPrincipal.DataKeys[row.RowIndex]["idUsuario"].ToString());//pegar o código da datakey da linha

                usuarioObj = usuarioDal.GetItem(idUsuario);

                switch (usuarioObj.Situacao)
                {
                   
                    case "I": usuarioObj.Situacao = "A";
                    break;

                    case "A": usuarioObj.Situacao = "I"; ;
                    break;

                }

                usuarioDal.Update(usuarioObj);

                MessageBox.Show("Usuário alterado com sucesso.", MessageBox.MessageType.Success);
                gvPrincipal.DataBind();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageBox.MessageType.Error);
            }

        }

        protected void btnExcluir_Click(object sender, EventArgs e)
        {

            try
            {
                    UsuarioControl usuarioDal = new UsuarioControl();
                    ImageButton imgButton;
                    imgButton = (ImageButton)sender; //converter objeto para checkbox
                    GridViewRow row = (GridViewRow)imgButton.Parent.Parent; // pegar a linha pai desta checkbox
                    int idUsuario = Convert.ToInt32(gvPrincipal.DataKeys[row.RowIndex]["idUsuario"].ToString());//pegar o código da datakey da linha
                    usuarioDal.Delete(Convert.ToInt32(idUsuario));

                    MessageBox.Show("Usuário apagado com sucesso.", MessageBox.MessageType.Success);
                    gvPrincipal.DataBind();
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageBox.MessageType.Error);
            }

        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {

            try
            {
                if (!String.IsNullOrEmpty(Request.Form[hidItem.UniqueID]))
                {
                    UsuarioControl usuarioDal = new UsuarioControl();
                    int idUsuario = Helper.GetSelectedGridItemID(gvPrincipal, Request.Form[hidItem.UniqueID]);
                    UsuarioFields uf = usuarioDal.GetItem(idUsuario);
                    Session["EditUser"] = uf;
                    mpeNovoUsuario.Show();
                    gvPrincipal.DataBind();
                }
                else
                {
                    MessageBox.Show("Selecione um usuário para editar!", MessageBox.MessageType.Error);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageBox.MessageType.Error);
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BoundGridPrincipal();
        }


        protected void gvPrincipal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litValue = (Literal)e.Row.FindControl("litValue"); 
                litValue.Text = gvPrincipal.DataKeys[e.Row.RowIndex]["idUsuario"].ToString();
            }
        }
        
        #endregion

        #region Methods

        protected void gvPrincipal_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPrincipal.PageIndex = e.NewPageIndex;
            BoundGridPrincipal();
        }

        private void BoundGridPrincipal()
        {
            gvPrincipal.DataSource = new UsuarioControl().GetAllUsersCadastro(txtNameUserSearch.Text);
            gvPrincipal.DataBind();
        }

        #endregion
    }
}