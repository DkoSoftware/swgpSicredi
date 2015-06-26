using System;
using System.Text;
using System.Text.RegularExpressions;

namespace SWGPgen
{


    /// <summary> 
    /// Autor: DAL Creator .net  
    /// Data de criação: 19/03/2012 22:46:52 
    /// Descrição: Classe que valida o objeto "UAFields". 
    /// </summary> 
    public class UAValidator 
    {


        #region Propriedade que armazena erros de execução 
        private string _ErrorMessage = string.Empty;
        public string ErrorMessage { get { return _ErrorMessage; } }
        #endregion


        public UAValidator() {}


        public bool isValid( UAFields fieldInfo )
        {
            try
            {


                //Field Nome
                if (  fieldInfo.Nome != string.Empty ) 
                   if ( fieldInfo.Nome.Trim().Length > 50  )
                      throw new Exception("O campo \"Nome\" deve ter comprimento máximo de 50 caracter(es).");

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
//    /// Data de criação: 13/03/2012 21:19:06 
//    /// Descrição: Classe que valida o objeto "UAFields". 
//    /// </summary> 
//    public class UAValidator 
//    {
//
//
//        #region Propriedade que armazena erros de execução 
//        private string _ErrorMessage = string.Empty;
//        public string ErrorMessage { get { return _ErrorMessage; } }
//        #endregion
//
//
//        public UAValidator() {}
//
//
//        public bool isValid( UAFields fieldInfo )
//        {
//            try
//            {
//
//
//                //Field Nome
//                if (  fieldInfo.Nome != string.Empty ) 
//                   if ( fieldInfo.Nome.Trim().Length > 50  )
//                      throw new Exception("O campo \"Nome\" deve ter comprimento máximo de 50 caracter(es).");
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
