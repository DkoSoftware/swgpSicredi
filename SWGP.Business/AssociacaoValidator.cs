using System;
using System.Text;
using System.Text.RegularExpressions;

namespace SWGPgen
{


    /// <summary> 
    /// Autor: DAL Creator .net  
    /// Data de cria��o: 25/03/2012 03:26:53 
    /// Descri��o: Classe que valida o objeto "AssociacaoFields". 
    /// </summary> 
    public class AssociacaoValidator 
    {


        #region Propriedade que armazena erros de execu��o 
        private string _ErrorMessage = string.Empty;
        public string ErrorMessage { get { return _ErrorMessage; } }
        #endregion


        public AssociacaoValidator() {}


        public bool isValid( AssociacaoFields fieldInfo )
        {
            try
            {


                //Field fkContato
                if ( !( fieldInfo.fkContato > 0 ) )
                   throw new Exception("O campo \"fkContato\" deve ser maior que zero.");


                //Field NumeroConta
                if (  fieldInfo.NumeroConta != string.Empty ) 
                   if ( fieldInfo.NumeroConta.Trim().Length > 7  )
                      throw new Exception("O campo \"NumeroConta\" deve ter comprimento m�ximo de 7 caracter(es).");
                if ( ( fieldInfo.NumeroConta == string.Empty ) || ( fieldInfo.NumeroConta.Trim().Length < 1 ) )
                   throw new Exception("O campo \"NumeroConta\" n�o pode ser nulo ou vazio e deve ter comprimento m�nimo de 1 caracter(es).");

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
//    /// Data de cria��o: 19/03/2012 22:46:51 
//    /// Descri��o: Classe que valida o objeto "AssociacaoFields". 
//    /// </summary> 
//    public class AssociacaoValidator 
//    {
//
//
//        #region Propriedade que armazena erros de execu��o 
//        private string _ErrorMessage = string.Empty;
//        public string ErrorMessage { get { return _ErrorMessage; } }
//        #endregion
//
//
//        public AssociacaoValidator() {}
//
//
//        public bool isValid( AssociacaoFields fieldInfo )
//        {
//            try
//            {
//
//
//                //Field fkProspect
//                if ( !( fieldInfo.fkProspect > 0 ) )
//                   throw new Exception("O campo \"fkProspect\" deve ser maior que zero.");
//
//
//                //Field NumeroConta
//                if (  fieldInfo.NumeroConta != string.Empty ) 
//                   if ( fieldInfo.NumeroConta.Trim().Length > 7  )
//                      throw new Exception("O campo \"NumeroConta\" deve ter comprimento m�ximo de 7 caracter(es).");
//                if ( ( fieldInfo.NumeroConta == string.Empty ) || ( fieldInfo.NumeroConta.Trim().Length < 1 ) )
//                   throw new Exception("O campo \"NumeroConta\" n�o pode ser nulo ou vazio e deve ter comprimento m�nimo de 1 caracter(es).");
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
////Projeto substitu�do ------------------------
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
////    /// Data de cria��o: 13/03/2012 21:19:06 
////    /// Descri��o: Classe que valida o objeto "AssociacaoFields". 
////    /// </summary> 
////    public class AssociacaoValidator 
////    {
////
////
////        #region Propriedade que armazena erros de execu��o 
////        private string _ErrorMessage = string.Empty;
////        public string ErrorMessage { get { return _ErrorMessage; } }
////        #endregion
////
////
////        public AssociacaoValidator() {}
////
////
////        public bool isValid( AssociacaoFields fieldInfo )
////        {
////            try
////            {
////
////
////                //Field fkProspect
////                if ( !( fieldInfo.fkProspect > 0 ) )
////                   throw new Exception("O campo \"fkProspect\" deve ser maior que zero.");
////
////
////                //Field NumeroConta
////                if (  fieldInfo.NumeroConta != string.Empty ) 
////                   if ( fieldInfo.NumeroConta.Trim().Length > 7  )
////                      throw new Exception("O campo \"NumeroConta\" deve ter comprimento m�ximo de 7 caracter(es).");
////                if ( ( fieldInfo.NumeroConta == string.Empty ) || ( fieldInfo.NumeroConta.Trim().Length < 1 ) )
////                   throw new Exception("O campo \"NumeroConta\" n�o pode ser nulo ou vazio e deve ter comprimento m�nimo de 1 caracter(es).");
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
