using System;
using System.Text;
using System.Text.RegularExpressions;

namespace SWGPgen
{


    /// <summary> 
    /// Autor: DAL Creator .net  
    /// Data de cria��o: 19/03/2012 22:46:51 
    /// Descri��o: Classe que valida o objeto "LembreteFields". 
    /// </summary> 
    public class LembreteValidator 
    {


        #region Propriedade que armazena erros de execu��o 
        private string _ErrorMessage = string.Empty;
        public string ErrorMessage { get { return _ErrorMessage; } }
        #endregion


        public LembreteValidator() {}


        public bool isValid( LembreteFields fieldInfo )
        {
            try
            {


                //Field Descricao
                if (  fieldInfo.Descricao != string.Empty ) 
                   if ( fieldInfo.Descricao.Trim().Length > 250  )
                      throw new Exception("O campo \"Descricao\" deve ter comprimento m�ximo de 250 caracter(es).");


                //Field FkUsuario
                if ( !( fieldInfo.FkUsuario > 0 ) )
                   throw new Exception("O campo \"FkUsuario\" deve ser maior que zero.");

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






//Projeto substitu�do ------------------------
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
//    /// Data de cria��o: 13/03/2012 21:19:06 
//    /// Descri��o: Classe que valida o objeto "LembreteFields". 
//    /// </summary> 
//    public class LembreteValidator 
//    {
//
//
//        #region Propriedade que armazena erros de execu��o 
//        private string _ErrorMessage = string.Empty;
//        public string ErrorMessage { get { return _ErrorMessage; } }
//        #endregion
//
//
//        public LembreteValidator() {}
//
//
//        public bool isValid( LembreteFields fieldInfo )
//        {
//            try
//            {
//
//
//                //Field Descricao
//                if (  fieldInfo.Descricao != string.Empty ) 
//                   if ( fieldInfo.Descricao.Trim().Length > 250  )
//                      throw new Exception("O campo \"Descricao\" deve ter comprimento m�ximo de 250 caracter(es).");
//
//
//                //Field FkUsuario
//                if ( !( fieldInfo.FkUsuario > 0 ) )
//                   throw new Exception("O campo \"FkUsuario\" deve ser maior que zero.");
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
