using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SWGPgen;
using DKO.Framework;
using System.Data.SqlClient;
using System.Data;
using SIMLgen;

namespace SWGP
{
    public partial class MainIndicacao : Common.BaseWebUi
    {
        #region Atributes
        public string hfGridRowSelected
        {
            get { return hfIdIndicacao.Value; }
            set { hfIdIndicacao.Value = value; }
        }
        #endregion

        private enum TipoPesquisaIndicacao
        { 
            Recebida,
            Enviada
        }

        #region Methods
        protected void Page_Load(object sender, EventArgs e)
        {
            MessageBox.Include(this);
            if (!IsPostBack)
            {
               DoSearch();
               ddlUsuarioParaIndicacao.Enabled = false;

            }
        }

        protected void gvPrincipalRecebida_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPrincipalRecebida.PageIndex = e.NewPageIndex;
            DoSearch();
        }


        protected void gvPrincipalEnviada_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPrincipalEnviada.PageIndex = e.NewPageIndex;
            DoSearch();
        }

        private void DoSearch()
        {
            try 
	            {
                    UsuarioFields usuarioObj = (UsuarioFields)Session["usuarioObj"];
                    IndicacaoControl indicacao = new IndicacaoControl();
                    if (usuarioObj == null)
                        throw new Exception("Sessão de Usuário expirou favor efetuar login novamente.");
                    
                    gvPrincipalEnviada.DataSource = indicacao.GetAllIndicacaoByModuloUsuario(usuarioObj, IndicacaoControl.TipoIndicacao.Indicada);
                    if (gvPrincipalEnviada.DataSource == null)
                    {
                        MessageBox.Show("Não existe indicação realizada pelo usuário.", MessageBox.MessageType.Info);
                        gvPrincipalEnviada.DataSource = null;
                    }
                    else
                        gvPrincipalEnviada.DataBind();

                    gvPrincipalRecebida.DataSource = indicacao.GetAllIndicacaoByModuloUsuario(usuarioObj, IndicacaoControl.TipoIndicacao.Recebida);
                    if (gvPrincipalRecebida.DataSource == null)
                    {
                        MessageBox.Show("não existe indicação para o usuário.", MessageBox.MessageType.Info);
                        gvPrincipalRecebida.DataSource = null;
                    }
                    else
                            gvPrincipalRecebida.DataBind();
    
	            }
	            catch (Exception ex)
	            {
		
		            MessageBox.Show(ex.Message, MessageBox.MessageType.Error);
	            }
          }

        protected void gvPrincipalRecebida_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litValue = (Literal)e.Row.FindControl("litValue");
                Literal litSituacao = (Literal)e.Row.FindControl("litSituacao");
                litValue.Text = gvPrincipalRecebida.DataKeys[e.Row.RowIndex]["idIndicacao"].ToString();
               

            }
        }

        protected void gvPrincipalEnviada_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litValue = (Literal)e.Row.FindControl("litValue");
                Literal litSituacao = (Literal)e.Row.FindControl("litSituacao");
                litValue.Text = gvPrincipalEnviada.DataKeys[e.Row.RowIndex]["idIndicacao"].ToString();
               

            }
        }
        #endregion

        #region Events

        protected void btnNewIndicacao_Click(object sender, EventArgs e)
        {
            try
            {
                Session["NovaIndicacao"] = true;
                BoundDropDownUA();
                mpeNovaIndicacao.Show();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageBox.MessageType.Error);
            }

        }
        
        protected void btnEditaIndicacao_Click(object sender, EventArgs e)
        {
            try
            {
                GridView gv = (GridView)sender;
                IndicacaoControl indicacaotDal = new IndicacaoControl();
                IndicacaoFields indicacaoObj = indicacaotDal.GetItem(Helper.GetSelectedGridItemID(gv, Request.Form[hidItem.UniqueID]));
                Session["EditProspect"] = indicacaoObj;
                mpeNovaIndicacao.Show();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageBox.MessageType.Error);
            }

        }

        protected void btnAtualizar_Click(object sender, EventArgs e)
        {
            try
            {
                DoSearch();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, MessageBox.MessageType.Error);
            }


        }
        
        
        #region IndicacaoRecebida
        
        protected void btnVisualizarRecebida_Click(object sender, EventArgs e)
        {
            try
            {
                IndicacaoControl indicacaotDal = new IndicacaoControl();
                ImageButton imgBtn;
                imgBtn = (ImageButton)sender; //converter objeto para imagebutton
                GridViewRow row = (GridViewRow)imgBtn.Parent.Parent; // pegar a l
                indicacaoObj = indicacaotDal.GetItem(Convert.ToInt32(gvPrincipalRecebida.DataKeys[row.RowIndex]["idIndicacao"].ToString()));
                Session["VisualizarIndicacao"] = indicacaoObj;
                SetFields();
                mpeNovaIndicacao.Show();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageBox.MessageType.Error);
            }

        }

        protected void btnExcluirRecebida_Click(object sender, EventArgs e)
        {
            try
            {
                IndicacaoControl indicacaotDal = new IndicacaoControl();
                ImageButton imgBtn;
                imgBtn = (ImageButton)sender; //converter objeto para imagebutton
                GridViewRow row = (GridViewRow)imgBtn.Parent.Parent; // pegar a l
                IndicacaoFields indicacaoObj = indicacaotDal.GetItem(Convert.ToInt32(gvPrincipalRecebida.DataKeys[row.RowIndex]["idIndicacao"].ToString()));
                indicacaotDal.Delete(indicacaoObj.idIndicacao);
                MessageBox.Show("Prospect apagado com sucesso.", MessageBox.MessageType.Success);
                DoSearch();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, MessageBox.MessageType.Error);
            }

        }
        
        #endregion

        
        
        #region IndicacaoEnviada

        protected void btnVisualizarEnviada_Click(object sender, EventArgs e)
        {
            try
            {
                IndicacaoControl indicacaotDal = new IndicacaoControl();
                ImageButton imgBtn;
                imgBtn = (ImageButton)sender; //converter objeto para imagebutton
                GridViewRow row = (GridViewRow)imgBtn.Parent.Parent; // pegar a l
                indicacaoObj = indicacaotDal.GetItem(Convert.ToInt32(gvPrincipalEnviada.DataKeys[row.RowIndex]["idIndicacao"].ToString()));
                Session["VisualizarIndicacao"] = indicacaoObj;
                SetFields();
                mpeNovaIndicacao.Show();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageBox.MessageType.Error);
            }

        }

        protected void btnExcluirEnviada_Click(object sender, EventArgs e)
        {
            try
            {
                IndicacaoControl indicacaotDal = new IndicacaoControl();
                ImageButton imgBtn;
                imgBtn = (ImageButton)sender; //converter objeto para imagebutton
                GridViewRow row = (GridViewRow)imgBtn.Parent.Parent; // pegar a l
                IndicacaoFields indicacaoObj = indicacaotDal.GetItem(Convert.ToInt32(gvPrincipalEnviada.DataKeys[row.RowIndex]["idIndicacao"].ToString()));
                indicacaotDal.Delete(indicacaoObj.idIndicacao);
                MessageBox.Show("Prospect apagado com sucesso.", MessageBox.MessageType.Success);
                DoSearch();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, MessageBox.MessageType.Error);
            }


        }

        #endregion

        #endregion

        #region Modal Nova Indicacao


        #region Attributes
        IndicacaoFields indicacaoObj;

        #endregion

        #region Methods

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    MessageBox.Include(this);
        //    if (Session["NovaIndicacao"] != null)
        //    {
        //        Session.Remove("NovaIndicacao");
        //        ClearFields();
        //    }


        //    if (!IsPostBack)
        //    {
        //        ClearFields();
        //        if (Session["TpEdita"] != null)
        //            Session.Remove("TpEdita");

        //        if (Session["EditaIndicacao"] != null)
        //        {

        //            indicacaoObj = (IndicacaoFields)Session["EditaIndicacao"];
        //            SetFields();
        //            Session.Remove("EditaIndicacao");
        //            Session["TpEdita"] = true;
        //        }
        //        else if (Session["VisualizarIndicacao"] != null)
        //        {
        //            indicacaoObj = (IndicacaoFields)Session["VisualizarIndicacao"];
        //            hfIdProspect.Value = indicacaoObj.idIndicacao.ToString(); ;
        //            SetFields();
        //            Session.Remove("VisualizarIndicacao");
        //            Session["TpEdita"] = true;
        //        }
        //    }

        //}

        private void SetFields()
        {
            txtBairro.Text = indicacaoObj.Bairro;
            txtCidade.Text = indicacaoObj.Cidade;
            txtEndereco.Text = indicacaoObj.Endereco;
            txtNomeProspect.Text = indicacaoObj.Nome;
            txtTelefone.Text = indicacaoObj.Telefone;
            hfidUserIndica.Value = indicacaoObj.idUsuarioIndica.ToString();
            hfidUserRecebe.Value = indicacaoObj.idUsuarioRecebe.ToString();
            //ddlUF.SelectedValue = 
            //UsuarioControl userDal = new UsuarioControl();
            //UsuarioFields usuario = userDal.GetItem(indicacaoObj.idUsuarioRecebe);
            //ddlUsuarioParaIndicacao.Items.FindByValue(usuario.idUsuario.ToString()).Selected = true;

        }

        private void ClearFields()
        {
            txtBairro.Text = string.Empty;
            txtCidade.Text = string.Empty;
            txtEndereco.Text = string.Empty;
            txtNomeProspect.Text = string.Empty;
            txtTelefone.Text = string.Empty;
            ddlUF.SelectedIndex = 0;

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
            ddlUsuarioParaIndicacao.ClearSelection();
            ddlUsuarioParaIndicacao.DataTextField = "UserName";
            ddlUsuarioParaIndicacao.DataValueField = "idUsuario";
            ddlUsuarioParaIndicacao.DataSource = usuarios.GetAllUsersByUA(int.Parse(ddlUA.SelectedValue));
            ddlUsuarioParaIndicacao.DataBind();
            this.ddlUA.Items.FindByValue(ddlUA.SelectedValue).Selected = true;

        }

        protected void ddlUA_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            BoundDropDownUsuario();
            mpeNovaIndicacao.Show();
            ddlUsuarioParaIndicacao.Enabled = true;
        }


        #endregion

        #region Events

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtNomeProspect.Text))
                    throw new Exception("Campo Nome é preenchimento obrigatório.");

                IndicacaoControl indDal = new IndicacaoControl();
                IndicacaoFields indicacaoNew = new IndicacaoFields();

                if (Session["TpEdita"] != null)
                    indicacaoNew = indDal.GetItem(Convert.ToInt32(hfIdProspect.Value));

                //popula os obj indicacao
                indicacaoNew.Bairro = txtBairro.Text;
                indicacaoNew.Cidade = txtCidade.Text;
                indicacaoNew.Endereco = txtEndereco.Text;
                indicacaoNew.Nome = txtNomeProspect.Text;
                indicacaoNew.Telefone = txtTelefone.Text;
                indicacaoNew.Estado = ddlUF.SelectedValue;

                if (Session["TpEdita"] != null)
                {
                    UsuarioFields usuarioIndica = (UsuarioFields)Session["usuarioObj"];
                    indicacaoNew.idUsuarioIndica = usuarioIndica.idUsuario;
                    indicacaoNew.idUsuarioRecebe = int.Parse(ddlUsuarioParaIndicacao.SelectedValue);
                    IndicacaoValidator indicacaoBus = new IndicacaoValidator();
                    if (indicacaoBus.isValid(indicacaoNew))
                        indDal.Update(indicacaoNew);
                    else
                        throw new Exception("Erro ao tentar alterar indicação.");
                    
                    ClearFields();
                    DoSearch();
                    Session.Remove("TpEdita");
                    MessageBox.Show("Indicação alterada com sucesso.", MessageBox.MessageType.Success);
                }

                else
                {
                    UsuarioFields user = (UsuarioFields)Session["usuarioObj"];
                    UsuarioControl userRecebe = new UsuarioControl();
                    indicacaoNew.idUsuarioIndica = Convert.ToInt32(user.idUsuario);
                    int idUserRecebe = userRecebe.GetItem(Convert.ToInt32(ddlUsuarioParaIndicacao.SelectedValue)).idUsuario;
                    indicacaoNew.idUsuarioRecebe = idUserRecebe;
                    IndicacaoValidator indicacaoBus = new IndicacaoValidator();
                    if (indicacaoBus.isValid(indicacaoNew))
                    {
                        indDal.Add(ref indicacaoNew);
                        int fkIndicacao = indicacaoNew.idIndicacao;

                        //cadastra o prospect na base do usuario como indicacao recebida
                        ProspectFields prospectInd = new ProspectFields();
                        prospectInd.Nome = indicacaoNew.Nome;
                        prospectInd.Telefone = indicacaoNew.Telefone;
                        prospectInd.FkUsuario = indicacaoNew.idUsuarioRecebe;
                        prospectInd.fkIndicacao = indicacaoNew.idIndicacao;
                        prospectInd.Estado = indicacaoNew.Estado;
                        prospectInd.Endereco = indicacaoNew.Endereco;
                        prospectInd.Cidade = indicacaoNew.Cidade;
                        prospectInd.Bairro = indicacaoNew.Bairro;
                        prospectInd.SituacaoProspect = "Indicação";
                        new ProspectControl().Add(ref prospectInd);

                    }

                    else
                        throw new Exception("Erro ao tentar incluir indicação.");

                    ClearFields();
                    DoSearch();
                    MessageBox.Show("Indicação cadastrada com sucesso.", MessageBox.MessageType.Success);
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageBox.MessageType.Error);
            }

        }
        
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            ClearFields();
            mpeNovaIndicacao.Hide();
        }

        #endregion

        private bool ValidateFields()
        {
            bool camposValidos = false;

            if (!string.IsNullOrEmpty(txtBairro.Text))
                camposValidos = true;

            if (!string.IsNullOrEmpty(txtCidade.Text))
                camposValidos = true;
            else
                camposValidos = false;

            if (!string.IsNullOrEmpty(txtEndereco.Text))
                camposValidos = true;
            else
                camposValidos = false;

            if (!string.IsNullOrEmpty(txtNomeProspect.Text))
                camposValidos = true;
            else
                camposValidos = false;

            if (!string.IsNullOrEmpty(txtTelefone.Text))
                camposValidos = true;
            else
                camposValidos = false;

            return camposValidos;
        }

        #endregion


    }
}