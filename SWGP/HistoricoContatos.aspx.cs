using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SWGPgen;
using DKO.Framework;
using SIMLgen;

namespace SWGP
{
    public partial class HistoricoContatos : Common.BaseWebUi
    {
        #region Attributos

        public string NomeProspect
        {
            get { return txtNomeProspect.Text; }
            set { txtNomeProspect.Text = value; }
        }

        #endregion

        #region Methods
        protected void Page_Load(object sender, EventArgs e)
        {
            MessageBox.Include(this);
            hidItem.Value = String.Empty;
            if (!IsPostBack)
            {   
                CleanFields();
                if (Session["idProspectHist"] != null)
                {
                    hfIdProspect.Value = Session["idProspectHist"].ToString();
                    DoSearch();
                    BindFields();
                }

            }
        }

        private void DoSearch()
        {
            string idUsuario = Session["idUsuario"].ToString();

            ContatoControl contatoControl = new ContatoControl();
            gvPrincipal.DataSource = contatoControl.GetAllContactsByProspect(Convert.ToInt32(hfIdProspect.Value));
            gvPrincipal.DataBind();
        }
        private void BindFields()
        {
            ProspectControl pc = new ProspectControl();
            ProspectFields pf = pc.GetItem(Convert.ToInt32(hfIdProspect.Value));
            txtNomeProspect.Text = pf.Nome;
            txtNomeProspect.Enabled = false;
        }

        private void CleanFields()
        {

        }

        protected string ValidFields()
        {
            string validate = string.Empty;


            if (string.IsNullOrEmpty(validate))
                validate = "OK";

            return validate;
        }


        #endregion

        #region Events

        protected void gvPrincipal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litValue = (Literal)e.Row.FindControl("litValue");
                litValue.Text = gvPrincipal.DataKeys[e.Row.RowIndex]["idContato"].ToString();
            }
        }

        protected void gvPrincipal_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPrincipal.PageIndex = e.NewPageIndex;
            DoSearch();
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            switch (ValidFields())
            {
                case "Numero Conta Em Branco":
                    MessageBox.Show("Situação Associada. Campo Número da Conta é de preenchimento obrigatório.", MessageBox.MessageType.Info);
                    break;
                case "Descricao em Branco":
                    MessageBox.Show("Campo Descrição é de preenchimento obrigatório.", MessageBox.MessageType.Info);
                    break;
                case "OK":
                    try
                    {

                        /*
                        int idProspect = Convert.ToInt32(Session["idProspect"].ToString());
                        Session.Remove("idProspect");
                        ProspectControl prospectData = new ProspectControl();
                        ProspectFields prospectObj = new ProspectFields();

                        prospectObj = prospectData.GetItem(idProspect);
                        NomeProspect = prospectObj.Nome;

                        ContatoControl contatoDal = new ContatoControl();
                        ContatoFields contatoObj = new ContatoFields();

                        contatoObj.DataCadastro = DateTime.Now;
                        contatoObj.DataContato = Convert.ToDateTime(txtDate.Text);
                        contatoObj.Descricao = DescricaoContato;
                        contatoObj.Situacao = Situacao;
                        contatoObj.Tipo = Tipo;

                        contatoObj.fkProspect = prospectObj.idProspect;

                        contatoDal.Add(ref contatoObj);

                        AssociacaoControl associacaoDal = new AssociacaoControl();
                        AssociacaoFields associacaoObj = new AssociacaoFields();

                        associacaoObj.DataCadastro = contatoObj.DataCadastro;
                        associacaoObj.DataAssociacao = contatoObj.DataContato;
                        associacaoObj.fkProspect = prospectObj.idProspect;

                        associacaoDal.Add(ref associacaoObj);

                        CleanFields();*/
                        MessageBox.Show("Contato adicionado com sucesso.", MessageBox.MessageType.Success);

                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message, MessageBox.MessageType.Error);
                    }

                    break;
            }


        }

        protected void btnExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                ContatoControl contatoDal = new ContatoControl();
                ContatoFields contatoObj = new ContatoFields();
                ImageButton imgButton;
                imgButton = (ImageButton)sender; //converter objeto para checkbox
                GridViewRow row = (GridViewRow)imgButton.Parent.Parent; // pegar a linha pai desta checkbox
                int idContato = Convert.ToInt32(gvPrincipal.DataKeys[row.RowIndex]["idContato"].ToString());//pegar o código da datakey da linha
                
                AssociacaoControl associacaoDal = new AssociacaoControl();
                AssociacaoFields associacaoObj = associacaoDal.FindByfkContato(contatoObj.idContato);

                if (associacaoObj != null)
                    associacaoDal.Delete(associacaoObj.idAssociacao);

                contatoDal.Delete(Convert.ToInt32(idContato));

                MessageBox.Show("Contato apagado com sucesso.", MessageBox.MessageType.Success);
                DoSearch();
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
                    ContatoControl contatoDal = new ContatoControl();
                    int idUsuario = Helper.GetSelectedGridItemID(gvPrincipal, Request.Form[hidItem.UniqueID]);
                    ContatoFields uf = contatoDal.GetItem(idUsuario);
                    Session.Add("idContatoEdit", uf.idContato);
                   
                    mpeNovoContato.Show();
                    DoSearch();
                }
                else
                {
                    MessageBox.Show("Selecione um contato para editar!", MessageBox.MessageType.Error);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageBox.MessageType.Error);
            }

        }

        protected void btnAtualizar_Click(object sender, EventArgs e)
        {
            BindFields();
            DoSearch();
        }

        protected void btnNovoContato_Click(object sender, EventArgs e)
        {
            BindFields();
            mpeNovoContato.Show();
        }
    }

        #endregion
}
