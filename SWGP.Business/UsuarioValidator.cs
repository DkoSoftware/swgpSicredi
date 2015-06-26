using System;
using System.Text;
using System.Text.RegularExpressions;

namespace SWGPgen
{


    /// <summary> 
    /// Autor: DAL Creator .net  
    /// Data de criação: 24/03/2012 20:08:47 
    /// Descrição: Classe que valida o objeto "UsuarioFields". 
    /// </summary> 
    public class UsuarioValidator 
    {


        #region Propriedade que armazena erros de execução 
        private string _ErrorMessage = string.Empty;
        public string ErrorMessage { get { return _ErrorMessage; } }
        #endregion


        public UsuarioValidator() {}


        public bool isValid( UsuarioFields fieldInfo )
        {
            try
            {


                //Field Nome
                if (  fieldInfo.Nome != string.Empty ) 
                   if ( fieldInfo.Nome.Trim().Length > 200  )
                      throw new Exception("O campo \"Nome\" deve ter comprimento máximo de 200 caracter(es).");


                //Field UserName
                if (  fieldInfo.UserName != string.Empty ) 
                   if ( fieldInfo.UserName.Trim().Length > 100  )
                      throw new Exception("O campo \"UserName\" deve ter comprimento máximo de 100 caracter(es).");


                //Field Password
                if (  fieldInfo.Password != string.Empty ) 
                   if ( fieldInfo.Password.Trim().Length > 100  )
                      throw new Exception("O campo \"Password\" deve ter comprimento máximo de 100 caracter(es).");


                //Field Cargo
                if (  fieldInfo.Cargo != string.Empty ) 
                   if ( fieldInfo.Cargo.Trim().Length > 50  )
                      throw new Exception("O campo \"Cargo\" deve ter comprimento máximo de 50 caracter(es).");


                //Field Situacao
                if (  fieldInfo.Situacao != string.Empty ) 
                   if ( fieldInfo.Situacao.Trim().Length > 1  )
                      throw new Exception("O campo \"Situacao\" deve ter comprimento máximo de 1 caracter(es).");


                //Field Modulo
                if (  fieldInfo.Modulo != string.Empty ) 
                   if ( fieldInfo.Modulo.Trim().Length > 1  )
                      throw new Exception("O campo \"Modulo\" deve ter comprimento máximo de 1 caracter(es).");


                //Field FkUa
                if ( !( fieldInfo.FkUa > 0 ) )
                   throw new Exception("O campo \"FkUa\" deve ser maior que zero.");


                //Field Funcao
                if (  fieldInfo.Funcao != string.Empty ) 
                   if ( fieldInfo.Funcao.Trim().Length > 30  )
                      throw new Exception("O campo \"Funcao\" deve ter comprimento máximo de 30 caracter(es).");

                return true;

            }
            catch (Exception e)
            {
                this._ErrorMessage = e.Message;
                return false;
            }

        }
    }

}






//Projeto substituído ------------------------
//using System;
//using System.Text;
//using System.Text.RegularExpressions;
//
//namespace SWGPgen
//{
//
//
//    /// <summary> 
//    /// Autor: DAL Creator .net  
//    /// Data de criação: 19/03/2012 22:46:52 
//    /// Descrição: Classe que valida o objeto "UsuarioFields". 
//    /// </summary> 
//    public class UsuarioValidator 
//    {
//
//
//        #region Propriedade que armazena erros de execução 
//        private string _ErrorMessage = string.Empty;
//        public string ErrorMessage { get { return _ErrorMessage; } }
//        #endregion
//
//
//        public UsuarioValidator() {}
//
//
//        public bool isValid( UsuarioFields fieldInfo )
//        {
//            try
//            {
//
//
//                //Field Nome
//                if (  fieldInfo.Nome != string.Empty ) 
//                   if ( fieldInfo.Nome.Trim().Length > 200  )
//                      throw new Exception("O campo \"Nome\" deve ter comprimento máximo de 200 caracter(es).");
//
//
//                //Field UserName
//                if (  fieldInfo.UserName != string.Empty ) 
//                   if ( fieldInfo.UserName.Trim().Length > 100  )
//                      throw new Exception("O campo \"UserName\" deve ter comprimento máximo de 100 caracter(es).");
//
//
//                //Field Password
//                if (  fieldInfo.Password != string.Empty ) 
//                   if ( fieldInfo.Password.Trim().Length > 100  )
//                      throw new Exception("O campo \"Password\" deve ter comprimento máximo de 100 caracter(es).");
//
//
//                //Field Cargo
//                if (  fieldInfo.Cargo != string.Empty ) 
//                   if ( fieldInfo.Cargo.Trim().Length > 50  )
//                      throw new Exception("O campo \"Cargo\" deve ter comprimento máximo de 50 caracter(es).");
//
//
//                //Field Situacao
//                if (  fieldInfo.Situacao != string.Empty ) 
//                   if ( fieldInfo.Situacao.Trim().Length > 1  )
//                      throw new Exception("O campo \"Situacao\" deve ter comprimento máximo de 1 caracter(es).");
//
//
//                //Field Modulo
//                if (  fieldInfo.Modulo != string.Empty ) 
//                   if ( fieldInfo.Modulo.Trim().Length > 1  )
//                      throw new Exception("O campo \"Modulo\" deve ter comprimento máximo de 1 caracter(es).");
//
//
//                //Field FkUa
//                if ( !( fieldInfo.FkUa > 0 ) )
//                   throw new Exception("O campo \"FkUa\" deve ser maior que zero.");
//
//                return true;
//
//            }
//            catch (Exception e)
//            {
//                this._ErrorMessage = e.Message;
//                return false;
//            }
//
//        }
//    }
//
//}
//
//
//
//
//
//
////Projeto substituído ------------------------
////using System;
////using System.Text;
////using System.Text.RegularExpressions;
////
////namespace SWGPgen
////{
////
////
////    /// <summary> 
////    /// Autor: DAL Creator .net  
////    /// Data de criação: 13/03/2012 21:30:59 
////    /// Descrição: Classe que valida o objeto "UsuarioFields". 
////    /// </summary> 
////    public class UsuarioValidator 
////    {
////
////
////        #region Propriedade que armazena erros de execução 
////        private string _ErrorMessage = string.Empty;
////        public string ErrorMessage { get { return _ErrorMessage; } }
////        #endregion
////
////
////        public UsuarioValidator() {}
////
////
////        public bool isValid( UsuarioFields fieldInfo )
////        {
////            try
////            {
////
////
////                //Field Nome
////                if (  fieldInfo.Nome != string.Empty ) 
////                   if ( fieldInfo.Nome.Trim().Length > 200  )
////                      throw new Exception("O campo \"Nome\" deve ter comprimento máximo de 200 caracter(es).");
////
////
////                //Field UserName
////                if (  fieldInfo.UserName != string.Empty ) 
////                   if ( fieldInfo.UserName.Trim().Length > 100  )
////                      throw new Exception("O campo \"UserName\" deve ter comprimento máximo de 100 caracter(es).");
////
////
////                //Field Password
////                if (  fieldInfo.Password != string.Empty ) 
////                   if ( fieldInfo.Password.Trim().Length > 100  )
////                      throw new Exception("O campo \"Password\" deve ter comprimento máximo de 100 caracter(es).");
////
////
////                //Field Cargo
////                if (  fieldInfo.Cargo != string.Empty ) 
////                   if ( fieldInfo.Cargo.Trim().Length > 50  )
////                      throw new Exception("O campo \"Cargo\" deve ter comprimento máximo de 50 caracter(es).");
////
////
////                //Field Situacao
////                if (  fieldInfo.Situacao != string.Empty ) 
////                   if ( fieldInfo.Situacao.Trim().Length > 1  )
////                      throw new Exception("O campo \"Situacao\" deve ter comprimento máximo de 1 caracter(es).");
////
////
////                //Field Modulo
////                if (  fieldInfo.Modulo != string.Empty ) 
////                   if ( fieldInfo.Modulo.Trim().Length > 1  )
////                      throw new Exception("O campo \"Modulo\" deve ter comprimento máximo de 1 caracter(es).");
////
////
////                //Field FkUa
////                if ( !( fieldInfo.FkUa > 0 ) )
////                   throw new Exception("O campo \"FkUa\" deve ser maior que zero.");
////
////                return true;
////
////            }
////            catch (Exception e)
////            {
////                this._ErrorMessage = e.Message;
////                return false;
////            }
////
////        }
////    }
////
////}
////
////
////
////
////
////
//////Projeto substituído ------------------------
//////using System;
//////using System.Text;
//////using System.Text.RegularExpressions;
//////
//////namespace SWGPgen
//////{
//////
//////
//////    /// <summary> 
//////    /// Autor: DAL Creator .net  
//////    /// Data de criação: 13/03/2012 21:19:06 
//////    /// Descrição: Classe que valida o objeto "UsuarioFields". 
//////    /// </summary> 
//////    public class UsuarioValidator 
//////    {
//////
//////
//////        #region Propriedade que armazena erros de execução 
//////        private string _ErrorMessage = string.Empty;
//////        public string ErrorMessage { get { return _ErrorMessage; } }
//////        #endregion
//////
//////
//////        public UsuarioValidator() {}
//////
//////
//////        public bool isValid( UsuarioFields fieldInfo )
//////        {
//////            try
//////            {
//////
//////
//////                //Field Nome
//////                if (  fieldInfo.Nome != string.Empty ) 
//////                   if ( fieldInfo.Nome.Trim().Length > 200  )
//////                      throw new Exception("O campo \"Nome\" deve ter comprimento máximo de 200 caracter(es).");
//////
//////
//////                //Field UserName
//////                if (  fieldInfo.UserName != string.Empty ) 
//////                   if ( fieldInfo.UserName.Trim().Length > 100  )
//////                      throw new Exception("O campo \"UserName\" deve ter comprimento máximo de 100 caracter(es).");
//////
//////
//////                //Field Password
//////                if (  fieldInfo.Password != string.Empty ) 
//////                   if ( fieldInfo.Password.Trim().Length > 100  )
//////                      throw new Exception("O campo \"Password\" deve ter comprimento máximo de 100 caracter(es).");
//////
//////
//////                //Field Cargo
//////                if (  fieldInfo.Cargo != string.Empty ) 
//////                   if ( fieldInfo.Cargo.Trim().Length > 50  )
//////                      throw new Exception("O campo \"Cargo\" deve ter comprimento máximo de 50 caracter(es).");
//////
//////
//////                //Field FkUa
//////                if ( !( fieldInfo.FkUa > 0 ) )
//////                   throw new Exception("O campo \"FkUa\" deve ser maior que zero.");
//////
//////                return true;
//////
//////            }
//////            catch (Exception e)
//////            {
//////                this._ErrorMessage = e.Message;
//////                return false;
//////            }
//////
//////        }
//////    }
//////
//////}
//////
