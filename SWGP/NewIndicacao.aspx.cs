using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SWGPgen;
using SIMLgen;

namespace SWGP
{
    public partial class NewIndicacao : Common.BaseWebUi
    {

        #region Attributes
        IndicacaoFields indicacaoObj;
            
        #endregion

        #region Methods
        
        protected void Page_Load(object sender, EventArgs e)
        {
            MessageBox.Include(this);
            if (Session["NovaIndicacao"] != null)
            {
                Session.Remove("NovaIndicacao");
                ClearFields();
            }


            if (!IsPostBack)
            {
                ClearFields();
                if (Session["TpEdita"] != null)
                    Session.Remove("TpEdita");

                if (Session["EditaIndicacao"] != null)
                {

                    indicacaoObj = (IndicacaoFields)Session["EditaIndicacao"];
                    SetFields();
                    Session.Remove("EditaIndicacao");
                    Session["TpEdita"] = true;
                }
                else if (Session["VisualizarIndicacao"] != null)
                {
                    indicacaoObj = (IndicacaoFields)Session["VisualizarIndicacao"];
                    hfIdProspect.Value = indicacaoObj.idIndicacao.ToString(); ;
                    SetFields();
                    Session.Remove("VisualizarIndicacao");
                    Session["TpEdita"] = true;
                }
            }

        }

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
                indicacaoNew.Bairro= txtBairro.Text;
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
                        prospectInd.Cidade= indicacaoNew.Cidade;
                        prospectInd.Bairro = indicacaoNew.Bairro;
                        prospectInd.SituacaoProspect = "Indicação";
                        new ProspectControl().Add(ref prospectInd);
                        
                    }
                        
                    else
                        throw new Exception("Erro ao tentar incluir indicação.");
                    ClearFields();
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

    }
}