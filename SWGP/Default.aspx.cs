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
    public partial class _Default : Common.BaseWebUi
    {

        #region Atributes
        public string hfGridRowSelected
        {
            get { return hfIdProspect.Value; }
            set { hfIdProspect.Value = value;}
        }
        #endregion

        #region Methods
        protected void Page_Load(object sender, EventArgs e)
        {
            UsuarioFields user = (UsuarioFields)Session["usuarioObj"];
            IndicacaoControl indic = new IndicacaoControl();

            if(new ProspectControl().CountAll() > 0)
            {
                var indicacaoImportada = from i in new ProspectControl().GetAll()
                                         where i.SituacaoProspect.Equals("Indicação") 
                                         && i.FkUsuario.Equals(user.idUsuario)
                                         select i;

                lblTotIndicacoes.Text = " " + indicacaoImportada.Count().ToString() + " ";
            }
            else
                lblTotIndicacoes.Text = " " + indic.CountAllIndicacaoByModuloUsuario(user).ToString() + " ";
            
            MessageBox.Include(this);
            if (!IsPostBack)
            {
                DoSearch();
            }
        }

        //private string GetSituacao()
        //{
        //    string situacao = string.Empty;
        //    if (!string.IsNullOrEmpty( rblSituacaoProspect.SelectedValue.ToString()))
        //        situacao = rblSituacaoProspect.SelectedItem.Text;
            
        //    return situacao;    
        //}

        private void DoSearch(int idProspect)
        {
            string idUsuario = Session["idUsuario"].ToString();
            ProspectControl prospectControl = new ProspectControl();
            gvPrincipal.DataSource = prospectControl.GetAllProspectdPrincipal(Convert.ToInt32(idUsuario), txtNomeProspect.Text, string.Empty, string.Empty, string.Empty, string.Empty,ddlSituacao.SelectedValue);
            gvPrincipal.DataBind();

        }

        protected void gvPrincipal_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPrincipal.PageIndex = e.NewPageIndex;
            DoSearch();
        }

        protected void gvPrincipal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litValue = (Literal)e.Row.FindControl("litValue");
                Literal litSituacao = (Literal)e.Row.FindControl("litSituacao");


                litValue.Text = gvPrincipal.DataKeys[e.Row.RowIndex]["idProspect"].ToString();
                //litSituacao.Text = String.IsNullOrEmpty(gvPrincipal.DataKeys[e.Row.RowIndex]["SituacaoProspect"].ToString()) ? "Não Contatado" : gvPrincipal.DataKeys[e.Row.RowIndex]["SituacaoProspect"].ToString();

            }
        }

        #endregion

        #region Events

        protected void btnHistorico_Click(object sender, EventArgs e)
        {
            try
            {
                ProspectControl prospectDal = new ProspectControl();
                ImageButton imgButton;
                imgButton = (ImageButton)sender; //converter objeto para checkbox
                GridViewRow row = (GridViewRow)imgButton.Parent.Parent; // pegar a linha pai desta checkbox
                int idProspect = Convert.ToInt32(gvPrincipal.DataKeys[row.RowIndex]["idProspect"].ToString());//pegar o código da datakey da linha

                Session.Add("idProspectHist",idProspect);
                mpeHistorico.Show();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageBox.MessageType.Error);
            }
            
            
        }

        protected void btnNewProspect_Click(object sender, EventArgs e)
        {
            try
            {  
                mpeNovoProspect.Show();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageBox.MessageType.Error);
            }
            
        }

        protected void btnAtualizar_Click(object sender, EventArgs e)
        {
            DoSearch();
            
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

            DoSearch();
            
        }

        private void DoSearch()
        {
            try
            {
                string NomeProspect = txtNomeProspect.Text;
                string NomeContato = txtNomeContato.Text;
                string TipoPessoa = ddlTipo.SelectedItem.Text;
                string CPF = txtCPF.Text;
                string CNPJ = txtCnpj.Text;
                string Situacao = string.Empty;
                if (cbSituacaoProspeccao.SelectedValue != "Selecione...")
                    Situacao = cbSituacaoProspeccao.SelectedItem.Text;

                string idUsuario = Session["idUsuario"].ToString();
                ProspectControl prospectControl = new ProspectControl();
                gvPrincipal.DataSource = prospectControl.GetAllProspectdPrincipal(Convert.ToInt32(idUsuario), NomeProspect, NomeContato, TipoPessoa, CPF, CNPJ, Situacao);
                gvPrincipal.DataBind();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageBox.MessageType.Error);
            }
        }

        protected void btnEditaProspect_Click(object sender, EventArgs e)
            
        {
            try
            {
                ProspectControl prospectDal = new ProspectControl();
                if (!string.IsNullOrEmpty(hidItem.Value))
                {
                    ProspectFields prospectObj = prospectDal.GetItem(Helper.GetSelectedGridItemID(gvPrincipal, Request.Form[hidItem.UniqueID]));
                    SetFields(prospectObj);
                    hidItem.Value = prospectObj.idProspect.ToString();
                    Session["EditProspect"] = prospectObj;
                    mpeNovoProspect.Show();
                }
                else
                {
                    MessageBox.Show("Nenhum Prospect selecionado.",MessageBox.MessageType.Info);
                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageBox.MessageType.Error);
            }

        }

        protected void btnNovoContato_Click(object sender, EventArgs e)
        {
            try
            {
                CleanFieldsFormContato();
                ProspectControl prospectDal = new ProspectControl();
                ImageButton imgButton;
                imgButton = (ImageButton)sender; //converter objeto para checkbox
                GridViewRow row = (GridViewRow)imgButton.Parent.Parent; // pegar a linha pai desta checkbox
                int idProspect = Convert.ToInt32(gvPrincipal.DataKeys[row.RowIndex]["idProspect"].ToString());//pegar o código da datakey da linha

               Session.Add("idProspectNew", idProspect);
               BindProspectFields(idProspect);
               mpeNovoContato.Show();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageBox.MessageType.Error);
            }
            
        }

        protected void btnExcluiProspect_Click(object sender, EventArgs e)
        {
            try
            {
                ProspectControl prospectDal = new ProspectControl();
                if (!string.IsNullOrEmpty(hidItem.Value))
                {
                    ProspectFields prospectObj = prospectDal.GetItem(Helper.GetSelectedGridItemID(gvPrincipal, Request.Form[hidItem.UniqueID]));
                    prospectDal.Delete(prospectObj.idProspect);
                    MessageBox.Show("Prospect apagado com sucesso.", MessageBox.MessageType.Success);
                    DoSearch();
                }
                else
                {
                    MessageBox.Show("Nenhum Prospect selecionado.", MessageBox.MessageType.Info);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageBox.MessageType.Error);
            }


        }

       
        #endregion

        //protected void gvPrincipal_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    try
        //    {
        //        ProspectControl prospectDal = new ProspectControl();
        //        ImageButton imgButton;
        //        imgButton = (ImageButton)sender; //converter objeto para checkbox
        //        GridViewRow row = (GridViewRow)imgButton.Parent.Parent; // pegar a linha pai desta checkbox
        //        int idProspect = Convert.ToInt32(gvPrincipal.DataKeys[row.RowIndex]["idProspect"].ToString());//pegar o código da datakey da linha

        //        Session.Add("idProspectHist", idProspect);
        //        mpeHistorico.Show();
        //    }
        //    catch (Exception ex)
        //    {

        //        MessageBox.Show(ex.Message, MessageBox.MessageType.Error);
        //    }
        //}

        #region Modal Novo Contato

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
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    MessageBox.Include(this);
        //    txtNumConta.Enabled = false;

        //    if (!IsPostBack)
        //    {
        //        if (Session["idProspectHist"] != null)
        //        {
        //            BindProspectFields(Convert.ToInt32(Session["idProspectHist"]));
        //            if (Session["idContatoEdit"] != null)
        //            {
        //                BindContactFields(Convert.ToInt32(Session["idContatoEdit"]));
        //            }
        //            else
        //            {
        //                CleanFields();

        //            }
        //        }
        //        else if (Session["idProspectNew"] != null)
        //        {
        //            BindProspectFields(Convert.ToInt32(Session["idProspectNew"]));
        //            txtDate.Text = DateTime.Now.ToShortDateString();
        //            ddlTipo.Items.FindByText("Telefonema").Selected = true;
        //        }

        //    }
        //}

        private void BindProspectFields(int id)
        {
            ProspectFields pf =  new ProspectControl().GetItem(id);
            txtNomeProspectMod.Text = pf.Nome;
            txtDate.Text = DateTime.Now.ToShortDateString();
            //txtNomeProspect.Enabled = false;

        }

        private void BindContactFields(int id)
        {
            ContatoControl pc = new ContatoControl();
            ContatoFields pf = pc.GetItem(id);
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

        private void CleanFields()
        {
            NumeroConta = string.Empty;
            DescricaoContato = string.Empty;

        }

        private void CleanFieldsFormContato()
        {
            NomeProspect = string.Empty;
            NumeroConta = string.Empty;
            ddlSituacao.SelectedIndex = 0;
            ddlTipoNovoContato.SelectedIndex = 0;
            txtDescricaoContato.Text = string.Empty;
            txtNumConta.Text = string.Empty;
            txtDate.Text = string.Empty;

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
                    mpeNovoContato.Show();
                    MessageBox.Show("Situação Associada. Campo Número da Conta é de preenchimento obrigatório.", MessageBox.MessageType.Info);
                    break;
                case "Descricao em Branco":
                    mpeNovoContato.Show();
                    MessageBox.Show("Campo Descrição é de preenchimento obrigatório.", MessageBox.MessageType.Info);
                    break;
                case "Data em Branco":
                    mpeNovoContato.Show();
                    MessageBox.Show("Campo Data é de preenchimento obrigatório.", MessageBox.MessageType.Info);
                    break;
                case "OK":
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
                        contatoObj.Tipo = ddlTipoNovoContato.SelectedItem.Text;
                        int id = -1;
                        contatoObj.fkProspect = prospectObj.idProspect;
                        if (Session["idContatoEdit"] != null)
                        {
                            contatoObj.idContato = Convert.ToInt32(Session["idContatoEdit"]);
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

                        AssociacaoControl associacaoDal = new AssociacaoControl();
                        AssociacaoFields associacaoObj = new AssociacaoFields();

                        associacaoObj.DataCadastro = contatoObj.DataCadastro;
                        associacaoObj.DataAssociacao = contatoObj.DataContato;
                        associacaoObj.NumeroConta = txtNumConta.Text;
                        associacaoObj.fkContato = id;

                        if (Session["idContatoEdit"] != null)
                        {
                            AssociacaoFields aux = associacaoDal.FindByfkContato(Convert.ToInt32(Session["idContatoEdit"]));
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

                        if (prospectObj.fkIndicacao > 0)
                        {
                            new IndicacaoControl().Delete(prospectObj.fkIndicacao);
                        }

                        NomeProspect = string.Empty;
                        CleanFields();
                        DoSearch();
                        MessageBox.Show("Contato adicionado com sucesso.", MessageBox.MessageType.Success);

                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message, MessageBox.MessageType.Error);
                    }

                    break;
            }


        }

        protected void ddlSituacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            if (ddl.SelectedItem.Text == "Associada")
            {
                txtNumConta.Enabled = true;
            }
            else
                txtNumConta.Enabled = false;

            mpeNovoContato.Show();
        }

        #endregion

        #endregion

        #region Modal novo Prospect

        #region Attributes

        public int idProspect;

        public string Nome
        {
            get { return txtNomeNovoProspect.Text; }
            set { txtNomeNovoProspect.Text = value; }
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
            get { return txtCNPJNovoProspect.Text; }
            set { txtCNPJNovoProspect.Text = value; }
        }

        public string Complemento
        {
            get { return txtComplemento.Text; }
            set { txtComplemento.Text = value; }
        }

        public string CPF
        {
            get { return txtCPFNovoProspect.Text; }
            set { txtCPFNovoProspect.Text = value; }
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
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    MessageBox.Include(this);

        //    if (!IsPostBack)
        //    {
        //        CleanFields();
        //        if (Session["EditProspect"] != null)
        //        {
        //            ProspectFields prospectObj = (ProspectFields)Session["EditProspect"];
        //            hidItem.Value = prospectObj.idProspect.ToString();
        //            SetFields(prospectObj);
        //        }

        //    }

       

        private void CleanFieldsNovoProspect()
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
                txtNomeNovoProspect.Text = prospect.Nome;
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

        protected void btnSalvarNovoProspect_Click(object sender, EventArgs e)
        {
            try
            {
                ProspectControl prospectDal = new ProspectControl();

                if (Session["EditProspect"] != null)
                {
                    Session.Remove("EditProspect");
                    prospectDal.Update(GetFieldsNovoProspect());
                    CleanFieldsNovoProspect();
                    DoSearch();
                    MessageBox.Show("Cadastro de Prospect atualizado com sucesso.", MessageBox.MessageType.Success);
                }

                else
                {
                    ProspectFields prospect = GetFieldsNovoProspect();
                    prospectDal.Add(ref prospect);
                    CleanFieldsNovoProspect();
                    DoSearch();
                    MessageBox.Show("Prospect incluido com sucesso.", MessageBox.MessageType.Success);
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageBox.MessageType.Error);
            }

        }

        private ProspectFields GetFieldsNovoProspect()
        {
            ProspectControl prospectDal = new ProspectControl();
            UsuarioFields user = (UsuarioFields)Session["usuarioObj"];
            ProspectFields prospect = (string.IsNullOrEmpty(hidItem.Value) ? new ProspectFields() : prospectDal.GetItem(int.Parse(hidItem.Value)));
            prospect.Nome = Nome;
            prospect.Endereco = txtEndereco.Text;
            prospect.Telefone = txtTelefone.Text;
            prospect.Email = txtEmail.Text;
            if (TipoPessoa != "Selecione...")
                prospect.Tipo = TipoPessoa;
            else
                prospect.Tipo = string.Empty;

            if (Segmento != "Selecione...")
                prospect.Segmento = Segmento;
            else
                prospect.Segmento = string.Empty;

            prospect.Observacao = txtObservacao.Text;
            prospect.Bairro = txtBairro.Text;
            prospect.Cidade = txtCidade.Text;

            if (Estado != "Selecione...")
                prospect.Estado = Estado;
            else
                prospect.Estado = string.Empty;

            
            prospect.PessoaContato = txtPessoaContato.Text;
            prospect.CPF = txtCPFNovoProspect.Text;
            prospect.CNPJ = txtCNPJNovoProspect.Text;
            prospect.FkUsuario = user.idUsuario;
            prospect.DataCadastro = DateTime.Now;

            string situacaoProspect = new ProspectControl().GetSituacaoProspect(prospect.idProspect);
            if (string.IsNullOrEmpty(situacaoProspect))
                prospect.SituacaoProspect = "Não Contatado";
            else
                prospect.SituacaoProspect = situacaoProspect;
            return prospect;
        }

        protected void btnLimparNovoProspect_Click(object sender, EventArgs e)
        {
            CleanFieldsNovoProspect();
        }
        #endregion

        #endregion
    }
}
