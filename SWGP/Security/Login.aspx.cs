using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using SWGPgen;
using System.Collections;

namespace SWGP
{
    public partial class Login : System.Web.UI.Page
    {
        #region Atributos
        
        public string Username 
        { get{ return (frmLogin.FindControl("txtUsername") as TextBox).Text;}
          set { (frmLogin.FindControl("txtUsername") as TextBox).Text = value; }
        }

        public string Password
        {
            get { return (frmLogin.FindControl("txtPassword") as TextBox).Text; }
            set { (frmLogin.FindControl("txtPassword") as TextBox).Text = value; }
        }

        public bool ManterConectado
        {
            get { return (frmLogin.FindControl("chbManterConectado") as CheckBox).Checked; }
            
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
          
        }

        protected void btnEntrar_Click(object sender, EventArgs e)
        {
            UsuarioValidator usuarioBus = new UsuarioValidator();
            UsuarioFields usuarioObj = new UsuarioFields();
            UsuarioControl usuarioDal = new UsuarioControl();
            
            try
            {


                if (usuarioDal.FindByPassword(FormsAuthentication.HashPasswordForStoringInConfigFile(Password, "MD5")) != null) 
                    {
                        if (usuarioDal.FindByUserName(Username) != null)
                        {
                            usuarioObj = usuarioDal.FindByUserName(Username);

                            Session["moduloUsuario"] = usuarioObj.Modulo;
                            Session["idUsuario"] = usuarioObj.idUsuario;
                            Session["usuarioObj"] = usuarioObj;

                            if (usuarioObj.Situacao != "I")
                            {
                                FormsAuthentication.RedirectFromLoginPage(Username, ManterConectado);
                            }
                            else
                            {
                                MessageBox.Show("Usuário inativo favor contatar o administrador.", MessageBox.MessageType.Info);
                            }
                            
                        }
                         
                         else 
                          MessageBox.Show("Usuário e/ou Senha incorretos, favor verificar", MessageBox.MessageType.Info);
                    
                    }
                    else 
                    {
                        MessageBox.Show("Usuário e/ou Senha incorretos, favor verificar", MessageBox.MessageType.Info);
                    }
                
            }
            
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
            
            

        }
    }
}