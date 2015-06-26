using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SWGPgen;
using System.Web.Security;

namespace SWGP
{
    public partial class NewUsuario : Common.BaseWebUi
    {
        UsuarioFields usuarioEdit;
        #region Atributos

        public string NomeUsuario
        {
            get { return txtNome.Text; }
            set { txtNome.Text = value; }
        }

        public string Funcao
        {
            get { return ddlFuncao.SelectedItem.Text; }
            set
            {
                this.ddlFuncao.ClearSelection();
                if (this.ddlFuncao.Items.FindByValue(value) != null)
                    this.ddlFuncao.Items.FindByValue(value).Selected = true;

            }

        }

        public string Usuario
        {
            get { return txtUsuario.Text; }
            set { txtUsuario.Text = value; }
        }

        public string Senha
        {
            get { return txtSenha.Text; }
            set { txtSenha.Text = value; }
        }

        public string ConfirmPassword
        {
            get { return txtConfirmPassword.Text; }
            set { txtConfirmPassword.Text = value; }
        }

        public string Modulo
        {
            get { return ddlModulo.SelectedItem.Text; }
            set
            {
                this.ddlModulo.ClearSelection();
                string aux = String.Empty; ;
                switch (value)
                {
                    case "U":
                        aux = "Usuário";
                        break;
                    case "A":
                        aux = "Administrador";
                        break;
                    case "M":
                        aux = "Administrador Master";
                        break;

                }

                if (this.ddlModulo.Items.FindByValue(aux) != null)
                    this.ddlModulo.Items.FindByValue(aux).Selected = true;

            }

        }

        public string UA
        {
            get { return ddlUA.SelectedItem.Text; }
            set
            {
                this.ddlUA.ClearSelection();
                if (this.ddlUA.Items.FindByValue(value) != null)
                    this.ddlUA.Items.FindByValue(value).Selected = true;

            }
        }

        public bool Situacao
        {
            get { return chbHabilitado.Checked; }
            set { chbHabilitado.Checked = value; }
        }

        #endregion

        #region Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            MessageBox.Include(this);
          
            if (!IsPostBack)
            {
                CleanFields();
                BindCombo();
                if (Session["EditUser"] != null)
                {
                    usuarioEdit = (UsuarioFields)Session["EditUser"];
                    SetFields(usuarioEdit);
                }
            }


        }

        private void BindCombo()
        {
            UAControl ua = new UAControl();
            ddlUA.ClearSelection();
            ddlUA.DataSource = ua.GetAll();
            ddlUA.DataTextField = "Nome";
            ddlUA.DataValueField = "idUA";
            ddlUA.DataBind();
        }

        private void SetFields(UsuarioFields usuario)
        {
            txtNome.Text = usuario.Nome;
            txtUsuario.Text = usuario.UserName;
            txtSenha.Text = usuario.Password;
            txtConfirmPassword.Text = usuario.Password;

            if (usuario.Situacao.Equals("A"))
                Situacao = true;
            else
                Situacao = false;

            UAControl uAControl = new UAControl();
            UAFields uAFields = uAControl.GetItem(usuario.FkUa);
            ddlFuncao.ClearSelection();
            ddlFuncao.Items.FindByText(usuario.Cargo).Selected = true;

            ddlModulo.ClearSelection();
            ddlModulo.Items.FindByValue(usuario.Modulo).Selected = true;

            ddlUA.ClearSelection();
            ddlUA.Items.FindByValue(uAFields.idUA.ToString()).Selected = true;
            
        }

        private void CleanFields()
        {
            Usuario = string.Empty;
            Senha = string.Empty;
            ConfirmPassword = string.Empty;
            NomeUsuario = string.Empty;
            Situacao = false;
            ddlFuncao.SelectedIndex = 0;
            ddlModulo.SelectedIndex = 0;
            ddlUA.SelectedIndex = 0;
            chbHabilitado.Checked = true;

        }

        private void GetFields(UsuarioFields usuario)
        {
            usuario.UserName = Usuario;
            if (Situacao.Equals(true))
                usuario.Situacao = "A";
            else
                usuario.Situacao = "I";
            usuario.Password = txtSenha.Text;

            if (Senha.Equals(ConfirmPassword))
                usuario.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(txtSenha.Text,"MD5");
            else
            {
                MessageBox.Show("Senhas divergentes favor verificar.", MessageBox.MessageType.Warning);
                return;
            }

            usuario.Nome = txtNome.Text;
            usuario.Modulo = ddlModulo.SelectedValue;
            usuario.Funcao = Funcao;
            usuario.FkUa = int.Parse(ddlUA.SelectedValue);
            usuario.Cargo = Funcao;

        }

        #endregion

        #region Events

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            UsuarioControl usuarioDal = new UsuarioControl();
            UsuarioFields usuarioObj = new UsuarioFields();
            UsuarioValidator usuarioBus = new UsuarioValidator();
            UsuarioFields usuarioVerify = new UsuarioFields();

            if (Session["EditUser"] != null)
            {
                usuarioObj = (UsuarioFields)Session["EditUser"];
               
                GetFields(usuarioObj);

                if(usuarioBus.isValid(usuarioObj))
                {
                    if(usuarioDal.Update(usuarioObj))
                    {
                        MessageBox.Show("Usuario atualizado com sucesso",MessageBox.MessageType.Success);
                        Session.Remove("EditUser");
                        CleanFields();
                    }
                    else
                    {
                        MessageBox.Show(usuarioDal.ErrorMessage,MessageBox.MessageType.Warning);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show(usuarioBus.ErrorMessage,MessageBox.MessageType.Warning);
                    return;
                }
            }
            else
            {
                if (Funcao == "Selecione...")
                {
                    MessageBox.Show("Favor selecionar uma função.", MessageBox.MessageType.Info);
                    return;
                }

                GetFields(usuarioObj);
                usuarioVerify = usuarioDal.FindByNome(usuarioObj.Nome);
                if (usuarioVerify != null)
                {
                    MessageBox.Show("Usuário já existente favor definir outro nome", MessageBox.MessageType.Warning);
                    return;
                }

                if (usuarioBus.isValid(usuarioObj))
                {
                    if (usuarioDal.Add(ref usuarioObj))
                    {
                        MessageBox.Show("Usuario cadastrado com sucesso", MessageBox.MessageType.Success);
                        CleanFields();
                    }
                    else
                    {
                        MessageBox.Show(usuarioDal.ErrorMessage, MessageBox.MessageType.Warning);
                        return;
                    }
                }
            }
        }

        #endregion
    }


}


