using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SWGPgen;
using SIMLgen;
using System.Data.SqlClient;
using System.Transactions;

namespace SWGP
{
    public partial class NewContato : Common.BaseWebUi
    {

        #region Attributos

        public string NomeProspect
        {
            get { return txtNomeProspect.Text; }
            set { txtNomeProspect.Text = value; }
        }

        public string NumeroConta
        {
            get { return txtNumConta.Text; }
            set { txtNumConta.Text = value; }
        }

        public string Situacao
        {
            get { return ddlSituacao.SelectedItem.Text; }

        }

        public string Tipo
        {
            get { return ddlTipo.SelectedItem.Text; }
        }

        public string DescricaoContato
        {
            get { return txtDescricaoContato.Text; }
            set { txtDescricaoContato.Text = value; }
        }

        public string Date
        {
            get { return txtDate.Text; }
            set { txtDate.Text = value; }
        }
        
        #endregion

        #region Methods
        protected void Page_Load(object sender, EventArgs e)
        {
            MessageBox.Include(this);
            txtNumConta.Enabled = false;
        
            if (!IsPostBack)
            {
                CleanFieldsFormContato();

                 if (Session["idProspectHist"] != null)
                {
                    BindProspectFields(Convert.ToInt32(Session["idProspectHist"]));
                    if (Session["idContatoEdit"] != null)
                    {
                        hfEditaContato.Value = Session["idContatoEdit"].ToString();
                        Session.Remove("idContatoEdit");
                        BindContactFields(Convert.ToInt32(hfEditaContato.Value));
                    }
                    else
                    {
                        //CleanFields();
                       // CleanFieldsFormContato();
                        txtDate.Text = DateTime.Now.ToShortDateString();
                    }
                }
                else if (Session["idProspectNew"] != null)
                {
                    //BindProspectFields(Convert.ToInt32(Session["idProspectNew"]));
                    txtDate.Text = DateTime.Now.ToShortDateString();
                    ddlTipo.Items.FindByText("Telefonema").Selected = true;
                }
                
            }
        }

        private void BindProspectFields(int id)
        {
            ProspectControl pc = new ProspectControl();
            ProspectFields pf = pc.GetItem(id);
            txtNomeProspect.Text = pf.Nome;
            txtNomeProspect.Enabled = false;
        }

        private void CleanFieldsFormContato()
        {
            NomeProspect = string.Empty;
            NumeroConta = string.Empty;
            ddlSituacao.SelectedIndex = 0;
            ddlTipo.SelectedIndex = 0;
            txtDescricaoContato.Text = string.Empty;
            txtNumConta.Text = string.Empty;
            txtDate.Text = string.Empty;

        }

        private void BindContactFields(int id)
        {
            ContatoControl pc = new ContatoControl();
            ContatoFields pf = pc.GetItem(id);

            if (pf != null)
            {
                txtDescricaoContato.Text = pf.Descricao;
                txtDate.Text = pf.DataContato.ToString("dd/MM/yyyy");


                txtNomeProspect.Enabled = false;

                this.ddlTipo.ClearSelection();
                if (this.ddlTipo.Items.FindByValue(pf.Tipo) != null)
                    this.ddlTipo.Items.FindByValue(pf.Tipo).Selected = true;


                this.ddlSituacao.ClearSelection();
                if (this.ddlSituacao.Items.FindByValue(pf.Situacao) != null)
                    this.ddlSituacao.Items.FindByValue(pf.Situacao).Selected = true;

                AssociacaoControl ac = new AssociacaoControl();
                AssociacaoFields af = ac.FindByfkContato(pf.idContato);
                if (af != null)
                {
                    txtNumConta.Text = af.NumeroConta;
                    txtNumConta.Enabled = true;
                }

            }
            
            
        }

        private void CleanFields()
        {
            NumeroConta = string.Empty;
            DescricaoContato = string.Empty;

        }

        protected string ValidFields()
        {
            string validate = string.Empty;


            if (Situacao == "Associada" && string.IsNullOrEmpty(NumeroConta))
            {
                validate = "Numero Conta Em Branco";

            }

            if (string.IsNullOrEmpty(DescricaoContato))
                validate = "Descricao em Branco";

            if (string.IsNullOrEmpty(Date))
                validate = "Data em Branco";

            if (string.IsNullOrEmpty(validate))
                validate = "OK";

            return validate;
        }


        #endregion

        #region Events


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
                case "Data em Branco":
                    MessageBox.Show("Campo Data é de preenchimento obrigatório.", MessageBox.MessageType.Info);
                    break;
                case "OK":

                    using (TransactionScope scope = new TransactionScope())
                    {
                        try
                        {

                            int idProspect = -1;
                            if (Session["idProspectNew"] != null)
                            {
                                idProspect = Convert.ToInt32(Session["idProspectNew"].ToString());
                            }
                            else if (Session["idProspectHist"] != null)
                            {
                                idProspect = Convert.ToInt32(Session["idProspectHist"].ToString());
                            }

                            ProspectControl prospectData = new ProspectControl();
                            ProspectFields prospectObj = new ProspectFields();

                            prospectObj = prospectData.GetItem(idProspect);
                            NomeProspect = prospectObj.Nome;

                            ContatoControl contatoDal = new ContatoControl();
                            ContatoFields contatoObj = new ContatoFields();

                            contatoObj.DataCadastro = DateTime.Now;
                            contatoObj.DataContato = Convert.ToDateTime(txtDate.Text + " " + DateTime.Now.TimeOfDay);
                            contatoObj.Descricao = DescricaoContato;
                            contatoObj.Situacao = Situacao;
                            contatoObj.Tipo = Tipo;
                            contatoObj.fkProspect = prospectObj.idProspect;
                            int id = -1;
                            
                            if (Convert.ToInt32(hfEditaContato.Value) > 0)
                            {
                                contatoObj.idContato = Convert.ToInt32(hfEditaContato.Value);
                                contatoDal.Update(contatoObj);
                                id = contatoObj.idContato;
                            }
                            else
                            {
                                string situacaoContatoProspect = prospectObj.SituacaoProspect;

                                if (situacaoContatoProspect != "Associada")
                                    id = contatoDal.Add(ref contatoObj);

                                else
                                {
                                    MessageBox.Show("Usuário com situação de Associada, favor verificar.", MessageBox.MessageType.Info);
                                    return;
                                }
                            }

                            if (contatoObj.Tipo == "Associada")
                            {
                                AssociacaoControl associacaoDal = new AssociacaoControl();
                                AssociacaoFields associacaoObj = new AssociacaoFields();

                                associacaoObj.DataCadastro = contatoObj.DataCadastro;
                                associacaoObj.DataAssociacao = contatoObj.DataContato;
                                associacaoObj.NumeroConta = txtNumConta.Text;
                                associacaoObj.fkContato = id;

                                if (Convert.ToInt32(hfEditaContato.Value) > 0)
                                {
                                    AssociacaoFields aux = associacaoDal.FindByfkContato(Convert.ToInt32(hfEditaContato.Value));
                                    if (aux != null)
                                    {
                                        associacaoObj.idAssociacao = aux.idAssociacao;
                                        associacaoDal.Update(associacaoObj);
                                        prospectObj.SituacaoProspect = Situacao;
                                        prospectData.Update(prospectObj);

                                    }
                                    else
                                    {
                                        associacaoDal.Add(ref associacaoObj);
                                        prospectObj.SituacaoProspect = Situacao;
                                        prospectData.Update(prospectObj);
                                    }

                                   
                                }
                                else
                                {
                                    string situacaoProspect = string.Empty;

                                    associacaoDal.Add(ref associacaoObj);
                                    prospectObj.SituacaoProspect = Situacao;
                                    prospectData.Update(prospectObj);

                                   
                                }
                            }

                            if (Session["idProspectNew"] != null)
                            {
                                CleanFields();
                                MessageBox.Show("Contato adicionado com sucesso.", MessageBox.MessageType.Success);
                            }
                            else if (Session["idProspectHist"] != null)
                            {
                                CleanFields();
                                MessageBox.Show("Contato alterado com sucesso.", MessageBox.MessageType.Success);
                            }

                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show(ex.Message, MessageBox.MessageType.Error);
                        }

                        scope.Complete();
                        break;
                    }

                    
            }

        }

        protected void ddlSituacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            if (ddl.SelectedItem.Text == "Associada")
            {
                txtNumConta.Enabled = true;
            }

        }

        #endregion
    }

}
