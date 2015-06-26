using System;
using System.Text;
using System.Text.RegularExpressions;

namespace SWGPgen
{


    /// <summary> 
    /// Autor: DAL Creator .net  
    /// Data de criação: 19/03/2012 22:46:51 
    /// Descrição: Classe que valida o objeto "ContatoFields". 
    /// </summary> 
    public class ContatoValidator 
    {


        #region Propriedade que armazena erros de execução 
        private string _ErrorMessage = string.Empty;
        public string ErrorMessage { get { return _ErrorMessage; } }
        #endregion


        public ContatoValidator() {}


        public bool isValid( ContatoFields fieldInfo )
        {
            try
            {


                //Field Tipo
                if (  fieldInfo.Tipo != string.Empty ) 
                   if ( fieldInfo.Tipo.Trim().Length > 30  )
                      throw new Exception("O campo \"Tipo\" deve ter comprimento máximo de 30 caracter(es).");


                //Field Descricao
                if (  fieldInfo.Descricao != string.Empty ) 
                   if ( fieldInfo.Descricao.Trim().Length > 300  )
                      throw new Exception("O campo \"Descricao\" deve ter comprimento máximo de 300 caracter(es).");


                //Field Situacao
                if (  fieldInfo.Situacao != string.Empty ) 
                   if ( fieldInfo.Situacao.Trim().Length > 30  )
                      throw new Exception("O campo \"Situacao\" deve ter comprimento máximo de 30 caracter(es).");


                //Field fkProspect
                if ( !( fieldInfo.fkProspect > 0 ) )
                   throw new Exception("O campo \"fkProspect\" deve ser maior que zero.");

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
//    /// Descrição: Classe que valida o objeto "ContatoFields". 
//    /// </summary> 
//    public class ContatoValidator 
//    {
//
//
//        #region Propriedade que armazena erros de execução 
//        private string _ErrorMessage = string.Empty;
//        public string ErrorMessage { get { return _ErrorMessage; } }
//        #endregion
//
//
//        public ContatoValidator() {}
//
//
//        public bool isValid( ContatoFields fieldInfo )
//        {
//            try
//            {
//
//
//                //Field Tipo
//                if (  fieldInfo.Tipo != string.Empty ) 
//                   if ( fieldInfo.Tipo.Trim().Length > 30  )
//                      throw new Exception("O campo \"Tipo\" deve ter comprimento máximo de 30 caracter(es).");
//
//
//                //Field Descricao
//                if (  fieldInfo.Descricao != string.Empty ) 
//                   if ( fieldInfo.Descricao.Trim().Length > 300  )
//                      throw new Exception("O campo \"Descricao\" deve ter comprimento máximo de 300 caracter(es).");
//
//
//                //Field Situacao
//                if (  fieldInfo.Situacao != string.Empty ) 
//                   if ( fieldInfo.Situacao.Trim().Length > 30  )
//                      throw new Exception("O campo \"Situacao\" deve ter comprimento máximo de 30 caracter(es).");
//
//
//                //Field fkProspect
//                if ( !( fieldInfo.fkProspect > 0 ) )
//                   throw new Exception("O campo \"fkProspect\" deve ser maior que zero.");
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
