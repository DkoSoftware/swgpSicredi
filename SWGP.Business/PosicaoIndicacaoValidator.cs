using System;
using System.Text;
using System.Text.RegularExpressions;

namespace SWGPgen
{


    /// <summary> 
    /// Autor: DAL Creator .net  
    /// Data de cria��o: 19/03/2012 22:46:51 
    /// Descri��o: Classe que valida o objeto "PosicaoIndicacaoFields". 
    /// </summary> 
    public class PosicaoIndicacaoValidator 
    {


        #region Propriedade que armazena erros de execu��o 
        private string _ErrorMessage = string.Empty;
        public string ErrorMessage { get { return _ErrorMessage; } }
        #endregion


        public PosicaoIndicacaoValidator() {}


        public bool isValid( PosicaoIndicacaoFields fieldInfo )
        {
            try
            {


                //Field NomeUsuarioRecebe
                if (  fieldInfo.NomeUsuarioRecebe != string.Empty ) 
                   if ( fieldInfo.NomeUsuarioRecebe.Trim().Length > 50  )
                      throw new Exception("O campo \"NomeUsuarioRecebe\" deve ter comprimento m�ximo de 50 caracter(es).");


                //Field NomeUsuarioIndica
                if (  fieldInfo.NomeUsuarioIndica != string.Empty ) 
                   if ( fieldInfo.NomeUsuarioIndica.Trim().Length > 50  )
                      throw new Exception("O campo \"NomeUsuarioIndica\" deve ter comprimento m�ximo de 50 caracter(es).");

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
//    /// Descri��o: Classe que valida o objeto "PosicaoIndicacaoFields". 
//    /// </summary> 
//    public class PosicaoIndicacaoValidator 
//    {
//
//
//        #region Propriedade que armazena erros de execu��o 
//        private string _ErrorMessage = string.Empty;
//        public string ErrorMessage { get { return _ErrorMessage; } }
//        #endregion
//
//
//        public PosicaoIndicacaoValidator() {}
//
//
//        public bool isValid( PosicaoIndicacaoFields fieldInfo )
//        {
//            try
//            {
//
//
//                //Field NomeUsuarioRecebe
//                if (  fieldInfo.NomeUsuarioRecebe != string.Empty ) 
//                   if ( fieldInfo.NomeUsuarioRecebe.Trim().Length > 50  )
//                      throw new Exception("O campo \"NomeUsuarioRecebe\" deve ter comprimento m�ximo de 50 caracter(es).");
//
//
//                //Field NomeUsuarioIndica
//                if (  fieldInfo.NomeUsuarioIndica != string.Empty ) 
//                   if ( fieldInfo.NomeUsuarioIndica.Trim().Length > 50  )
//                      throw new Exception("O campo \"NomeUsuarioIndica\" deve ter comprimento m�ximo de 50 caracter(es).");
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
