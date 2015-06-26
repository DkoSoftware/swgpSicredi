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
    public partial class NewProspect : Common.BaseWebUi
    {

        #region Attributes

        public int idProspect;

        public string Nome 
        { get{return txtNomeProspect.Text;}
            set { txtNomeProspect.Text = value; } 
        }

        public string Bairro
        {
            get { return txtBairro.Text; }
            set { txtBairro.Text = value; }
        }

        public string Cidade
        {
            get { return txtCidade.Text; }
            set { txtCidade.Text = value; }
        }

        public string CNPJ
        {
            get { return txtCNPJ.Text; }
            set { txtCNPJ.Text = value; }
        }

        public string Complemento
        {
            get { return txtComplemento.Text; }
            set { txtComplemento.Text = value; }
        }

        public string CPF
        {
            get { return txtCPF.Text; }
            set { txtCPF.Text = value; }
        }

        public string Email
        {
            get { return txtEmail.Text; }
            set { txtEmail.Text = value; }
        }

        public string Endereco
        {
            get { return txtEndereco.Text; }
            set { txtEndereco.Text = value; }
        }

        public string PessoaContato
        {
            get { return txtPessoaContato.Text; }
            set { txtPessoaContato.Text = value; }
        }

        public string Telefone
        {
            get { return txtTelefone.Text; }
            set { txtTelefone.Text = value; }
        }

        public string Estado
        {
            get { return ddlEstado.SelectedItem.Text; }
            set
            {
                this.ddlEstado.ClearSelection();
                if (this.ddlEstado.Items.FindByValue(value) != null)
                    this.ddlEstado.Items.FindByValue(value).Selected = true;

            }

        }

        public string TipoPessoa
        {
            get { return ddlTipoPessoa.SelectedItem.Text; }
            set
            {
                this.ddlTipoPessoa.ClearSelection();
                if (this.ddlTipoPessoa.Items.FindByValue(value) != null)
                    this.ddlTipoPessoa.Items.FindByValue(value).Selected = true;

            }

        }

        public string Segmento
        {
            get { return ddlSegmento.SelectedItem.Text; }
            set
            {
                this.ddlSegmento.ClearSelection();
                if (this.ddlSegmento.Items.FindByValue(value) != null)
                    this.ddlSegmento.Items.FindByValue(value).Selected = true;

            }

        }

        public string Observacao
        {
            get { return txtObservacao.Text; }
            set { txtObservacao.Text = value; }
        }
        

        #endregion

        #region Methods
        protected void Page_Load(object sender, EventArgs e)
        {
            MessageBox.Include(this);
           
            if (!IsPostBack)
            {
                CleanFields();
                if (Session["EditProspect"] != null)
                {
                    ProspectFields prospectObj = (ProspectFields)Session["EditProspect"];
                    hidItem.Value = prospectObj.idProspect.ToString();
                    SetFields(prospectObj);
                }
                
            }

        }

        private void CleanFields()
        {
            try
            {
                Nome = string.Empty;
                Endereco = string.Empty;
                Telefone = string.Empty;
                Email = string.Empty;
                TipoPessoa = string.Empty;
                Segmento = string.Empty;
                Observacao = string.Empty;
                Bairro = string.Empty;
                Cidade = string.Empty;
                Estado = string.Empty;
                PessoaContato = string.Empty;
                CPF = string.Empty;
                CNPJ = string.Empty;
                Complemento = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, MessageBox.MessageType.Error);
            }
        }

        private void SetFields(ProspectFields prospect)
        {

            try
            {
                txtNomeProspect.Text = prospect.Nome;
                txtEndereco.Text = prospect.Endereco;
                txtTelefone.Text = prospect.Telefone;
                txtEmail.Text = prospect.Email;
                TipoPessoa = prospect.Tipo;
                Segmento = prospect.Segmento;
                Observacao = prospect.Observacao;
                Bairro = prospect.Bairro;
                Cidade = prospect.Cidade;
                Estado = prospect.Estado;
                PessoaContato = prospect.PessoaContato;
                CPF = prospect.CPF;
                CNPJ = prospect.CNPJ;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, MessageBox.MessageType.Error);
            }
        }
        #endregion

        #region Events

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                ProspectControl prospectDal = new ProspectControl();

                if (Session["EditProspect"] != null)
                {
                    Session.Remove("EditProspect");
                    prospectDal.Update(GetFields());
                    CleanFields();
                    MessageBox.Show("Cadastro de Prospect atualizado com sucesso.", MessageBox.MessageType.Success);
                }

                else 
                {
                    ProspectFields prospect = GetFields();
                    prospectDal.Add(ref prospect);
                    CleanFields();
                    MessageBox.Show("Prospect incluido com sucesso.", MessageBox.MessageType.Success);
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageBox.MessageType.Error);
            }
            
        }

        private ProspectFields GetFields()
        {
            ProspectControl prospectDal = new ProspectControl();
            UsuarioFields user = (UsuarioFields)Session["usuarioObj"];
            ProspectFields prospect = (string.IsNullOrEmpty(hidItem.Value) ?  new ProspectFields() : prospectDal.GetItem(int.Parse(hidItem.Value)));
            prospect.Nome = txtNomeProspect.Text;
            prospect.Endereco = txtEndereco.Text;
            prospect.Telefone = txtTelefone.Text ;
            prospect.Email = txtEmail.Text ;
            if (TipoPessoa != "Selecione...")
                prospect.Tipo = TipoPessoa;
            else
                prospect.Tipo = string.Empty;

            if (Segmento != "Selecione...")
                prospect.Segmento = Segmento;
            else
                prospect.Segmento = string.Empty;
           
            prospect.Observacao = txtObservacao.Text;
            prospect.Bairro = txtBairro.Text ;
            prospect.Cidade = txtCidade.Text;

            if (Estado != "Selecione...")
                prospect.Estado = Estado;
            else
                prospect.Estado = string.Empty;
          
            prospect.PessoaContato = txtPessoaContato.Text;
            prospect.CPF = txtCPF.Text;
            prospect.CNPJ = txtCNPJ.Text ;
            prospect.FkUsuario = user.idUsuario;
            prospect.DataCadastro = DateTime.Now;

            string situacaoProspect = new ProspectControl().GetSituacaoProspect(prospect.idProspect);
            if (string.IsNullOrEmpty(situacaoProspect))
                prospect.SituacaoProspect = "Não Contatado";
            else
                prospect.SituacaoProspect = situacaoProspect;
            return prospect;
        }
        #endregion

        #region Events
        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            CleanFields();
        }
        #endregion



    }
}