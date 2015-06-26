using System;
using System.Text;
using System.Text.RegularExpressions;

namespace SWGPgen
{


    /// <summary> 
    /// Autor: DAL Creator .net  
    /// Data de criação: 02/04/2012 21:22:09 
    /// Descrição: Classe que valida o objeto "IndicacaoFields". 
    /// </summary> 
    public class IndicacaoValidator 
    {


        #region Propriedade que armazena erros de execução 
        private string _ErrorMessage = string.Empty;
        public string ErrorMessage { get { return _ErrorMessage; } }
        #endregion


        public IndicacaoValidator() {}


        public bool isValid( IndicacaoFields fieldInfo )
        {
            try
            {


                //Field Nome
                if (  fieldInfo.Nome != string.Empty ) 
                   if ( fieldInfo.Nome.Trim().Length > 150  )
                      throw new Exception("O campo \"Nome\" deve ter comprimento máximo de 150 caracter(es).");
                if ( ( fieldInfo.Nome == string.Empty ) || ( fieldInfo.Nome.Trim().Length < 1 ) )
                   throw new Exception("O campo \"Nome\" não pode ser nulo ou vazio e deve ter comprimento mínimo de 1 caracter(es).");


                //Field Telefone
                if (  fieldInfo.Telefone != string.Empty ) 
                   if ( fieldInfo.Telefone.Trim().Length > 50  )
                      throw new Exception("O campo \"Telefone\" deve ter comprimento máximo de 50 caracter(es).");


                //Field Endereco
                if (  fieldInfo.Endereco != string.Empty ) 
                   if ( fieldInfo.Endereco.Trim().Length > 150  )
                      throw new Exception("O campo \"Endereco\" deve ter comprimento máximo de 150 caracter(es).");


                //Field Bairro
                if (  fieldInfo.Bairro != string.Empty ) 
                   if ( fieldInfo.Bairro.Trim().Length > 150  )
                      throw new Exception("O campo \"Bairro\" deve ter comprimento máximo de 150 caracter(es).");


                //Field Cidade
                if (  fieldInfo.Cidade != string.Empty ) 
                   if ( fieldInfo.Cidade.Trim().Length > 150  )
                      throw new Exception("O campo \"Cidade\" deve ter comprimento máximo de 150 caracter(es).");


                //Field Estado
                if (  fieldInfo.Estado != string.Empty ) 
                   if ( fieldInfo.Estado.Trim().Length > 150  )
                      throw new Exception("O campo \"Estado\" deve ter comprimento máximo de 150 caracter(es).");


                //Field idUsuarioRecebe
                if ( !( fieldInfo.idUsuarioRecebe > 0 ) )
                   throw new Exception("O campo \"idUsuarioRecebe\" deve ser maior que zero.");


                //Field idUsuarioIndica
                if ( !( fieldInfo.idUsuarioIndica > 0 ) )
                   throw new Exception("O campo \"idUsuarioIndica\" deve ser maior que zero.");

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
//    /// Data de criação: 30/03/2012 01:12:52 
//    /// Descrição: Classe que valida o objeto "IndicacaoFields". 
//    /// </summary> 
//    public class IndicacaoValidator 
//    {
//
//
//        #region Propriedade que armazena erros de execução 
//        private string _ErrorMessage = string.Empty;
//        public string ErrorMessage { get { return _ErrorMessage; } }
//        #endregion
//
//
//        public IndicacaoValidator() {}
//
//
//        public bool isValid( IndicacaoFields fieldInfo )
//        {
//            try
//            {
//
//
//                //Field Nome
//                if (  fieldInfo.Nome != string.Empty ) 
//                   if ( fieldInfo.Nome.Trim().Length > 150  )
//                      throw new Exception("O campo \"Nome\" deve ter comprimento máximo de 150 caracter(es).");
//                if ( ( fieldInfo.Nome == string.Empty ) || ( fieldInfo.Nome.Trim().Length < 1 ) )
//                   throw new Exception("O campo \"Nome\" não pode ser nulo ou vazio e deve ter comprimento mínimo de 1 caracter(es).");
//
//
//                //Field Telefone
//                if (  fieldInfo.Telefone != string.Empty ) 
//                   if ( fieldInfo.Telefone.Trim().Length > 50  )
//                      throw new Exception("O campo \"Telefone\" deve ter comprimento máximo de 50 caracter(es).");
//
//
//                //Field Endereco
//                if (  fieldInfo.Endereco != string.Empty ) 
//                   if ( fieldInfo.Endereco.Trim().Length > 150  )
//                      throw new Exception("O campo \"Endereco\" deve ter comprimento máximo de 150 caracter(es).");
//
//
//                //Field Bairro
//                if (  fieldInfo.Bairro != string.Empty ) 
//                   if ( fieldInfo.Bairro.Trim().Length > 150  )
//                      throw new Exception("O campo \"Bairro\" deve ter comprimento máximo de 150 caracter(es).");
//
//
//                //Field Cidade
//                if (  fieldInfo.Cidade != string.Empty ) 
//                   if ( fieldInfo.Cidade.Trim().Length > 150  )
//                      throw new Exception("O campo \"Cidade\" deve ter comprimento máximo de 150 caracter(es).");
//
//
//                //Field Estado
//                if (  fieldInfo.Estado != string.Empty ) 
//                   if ( fieldInfo.Estado.Trim().Length > 150  )
//                      throw new Exception("O campo \"Estado\" deve ter comprimento máximo de 150 caracter(es).");
//
//
//                //Field idUsuarioRecebe
//                if ( !( fieldInfo.idUsuarioRecebe > 0 ) )
//                   throw new Exception("O campo \"idUsuarioRecebe\" deve ser maior que zero.");
//
//
//                //Field idUsuarioIndica
//                if ( !( fieldInfo.idUsuarioIndica > 0 ) )
//                   throw new Exception("O campo \"idUsuarioIndica\" deve ser maior que zero.");
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
////    /// Data de criação: 30/03/2012 00:35:28 
////    /// Descrição: Classe que valida o objeto "IndicacaoFields". 
////    /// </summary> 
////    public class IndicacaoValidator 
////    {
////
////
////        #region Propriedade que armazena erros de execução 
////        private string _ErrorMessage = string.Empty;
////        public string ErrorMessage { get { return _ErrorMessage; } }
////        #endregion
////
////
////        public IndicacaoValidator() {}
////
////
////        public bool isValid( IndicacaoFields fieldInfo )
////        {
////            try
////            {
////
////
////                //Field Nome
////                if (  fieldInfo.Nome != string.Empty ) 
////                   if ( fieldInfo.Nome.Trim().Length > 150  )
////                      throw new Exception("O campo \"Nome\" deve ter comprimento máximo de 150 caracter(es).");
////                if ( ( fieldInfo.Nome == string.Empty ) || ( fieldInfo.Nome.Trim().Length < 1 ) )
////                   throw new Exception("O campo \"Nome\" não pode ser nulo ou vazio e deve ter comprimento mínimo de 1 caracter(es).");
////
////
////                //Field Telefone
////                if (  fieldInfo.Telefone != string.Empty ) 
////                   if ( fieldInfo.Telefone.Trim().Length > 50  )
////                      throw new Exception("O campo \"Telefone\" deve ter comprimento máximo de 50 caracter(es).");
////
////
////                //Field Endereco
////                if (  fieldInfo.Endereco != string.Empty ) 
////                   if ( fieldInfo.Endereco.Trim().Length > 150  )
////                      throw new Exception("O campo \"Endereco\" deve ter comprimento máximo de 150 caracter(es).");
////
////
////                //Field Bairro
////                if (  fieldInfo.Bairro != string.Empty ) 
////                   if ( fieldInfo.Bairro.Trim().Length > 150  )
////                      throw new Exception("O campo \"Bairro\" deve ter comprimento máximo de 150 caracter(es).");
////
////
////                //Field Cidade
////                if (  fieldInfo.Cidade != string.Empty ) 
////                   if ( fieldInfo.Cidade.Trim().Length > 150  )
////                      throw new Exception("O campo \"Cidade\" deve ter comprimento máximo de 150 caracter(es).");
////
////
////                //Field Estado
////                if (  fieldInfo.Estado != string.Empty ) 
////                   if ( fieldInfo.Estado.Trim().Length > 150  )
////                      throw new Exception("O campo \"Estado\" deve ter comprimento máximo de 150 caracter(es).");
////
////
////                //Field idUsuarioRecebe
////                if ( !( fieldInfo.idUsuarioRecebe > 0 ) )
////                   throw new Exception("O campo \"idUsuarioRecebe\" deve ser maior que zero.");
////
////
////                //Field idUsuarioIndica
////                if ( !( fieldInfo.idUsuarioIndica > 0 ) )
////                   throw new Exception("O campo \"idUsuarioIndica\" deve ser maior que zero.");
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
//////    /// Data de criação: 27/03/2012 03:05:16 
//////    /// Descrição: Classe que valida o objeto "IndicacaoFields". 
//////    /// </summary> 
//////    public class IndicacaoValidator 
//////    {
//////
//////
//////        #region Propriedade que armazena erros de execução 
//////        private string _ErrorMessage = string.Empty;
//////        public string ErrorMessage { get { return _ErrorMessage; } }
//////        #endregion
//////
//////
//////        public IndicacaoValidator() {}
//////
//////
//////        public bool isValid( IndicacaoFields fieldInfo )
//////        {
//////            try
//////            {
//////
//////
//////                //Field Nome
//////                if (  fieldInfo.Nome != string.Empty ) 
//////                   if ( fieldInfo.Nome.Trim().Length > 150  )
//////                      throw new Exception("O campo \"Nome\" deve ter comprimento máximo de 150 caracter(es).");
//////                if ( ( fieldInfo.Nome == string.Empty ) || ( fieldInfo.Nome.Trim().Length < 1 ) )
//////                   throw new Exception("O campo \"Nome\" não pode ser nulo ou vazio e deve ter comprimento mínimo de 1 caracter(es).");
//////
//////
//////                //Field Telefone
//////                if (  fieldInfo.Telefone != string.Empty ) 
//////                   if ( fieldInfo.Telefone.Trim().Length > 50  )
//////                      throw new Exception("O campo \"Telefone\" deve ter comprimento máximo de 50 caracter(es).");
//////
//////
//////                //Field Endereco
//////                if (  fieldInfo.Endereco != string.Empty ) 
//////                   if ( fieldInfo.Endereco.Trim().Length > 150  )
//////                      throw new Exception("O campo \"Endereco\" deve ter comprimento máximo de 150 caracter(es).");
//////
//////
//////                //Field Bairro
//////                if (  fieldInfo.Bairro != string.Empty ) 
//////                   if ( fieldInfo.Bairro.Trim().Length > 150  )
//////                      throw new Exception("O campo \"Bairro\" deve ter comprimento máximo de 150 caracter(es).");
//////
//////
//////                //Field Cidade
//////                if (  fieldInfo.Cidade != string.Empty ) 
//////                   if ( fieldInfo.Cidade.Trim().Length > 150  )
//////                      throw new Exception("O campo \"Cidade\" deve ter comprimento máximo de 150 caracter(es).");
//////
//////
//////                //Field Estado
//////                if (  fieldInfo.Estado != string.Empty ) 
//////                   if ( fieldInfo.Estado.Trim().Length > 150  )
//////                      throw new Exception("O campo \"Estado\" deve ter comprimento máximo de 150 caracter(es).");
//////
//////
//////                //Field idUsuarioRecebe
//////                if ( !( fieldInfo.idUsuarioRecebe > 0 ) )
//////                   throw new Exception("O campo \"idUsuarioRecebe\" deve ser maior que zero.");
//////
//////
//////                //Field idUsuarioIndica
//////                if ( !( fieldInfo.idUsuarioIndica > 0 ) )
//////                   throw new Exception("O campo \"idUsuarioIndica\" deve ser maior que zero.");
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
//////
//////
//////
//////
//////
////////Projeto substituído ------------------------
////////using System;
////////using System.Text;
////////using System.Text.RegularExpressions;
////////
////////namespace SWGPgen
////////{
////////
////////
////////    /// <summary> 
////////    /// Autor: DAL Creator .net  
////////    /// Data de criação: 27/03/2012 02:25:19 
////////    /// Descrição: Classe que valida o objeto "IndicacaoFields". 
////////    /// </summary> 
////////    public class IndicacaoValidator 
////////    {
////////
////////
////////        #region Propriedade que armazena erros de execução 
////////        private string _ErrorMessage = string.Empty;
////////        public string ErrorMessage { get { return _ErrorMessage; } }
////////        #endregion
////////
////////
////////        public IndicacaoValidator() {}
////////
////////
////////        public bool isValid( IndicacaoFields fieldInfo )
////////        {
////////            try
////////            {
////////
////////
////////                //Field Nome
////////                if (  fieldInfo.Nome != string.Empty ) 
////////                   if ( fieldInfo.Nome.Trim().Length > 150  )
////////                      throw new Exception("O campo \"Nome\" deve ter comprimento máximo de 150 caracter(es).");
////////                if ( ( fieldInfo.Nome == string.Empty ) || ( fieldInfo.Nome.Trim().Length < 1 ) )
////////                   throw new Exception("O campo \"Nome\" não pode ser nulo ou vazio e deve ter comprimento mínimo de 1 caracter(es).");
////////
////////
////////                //Field Telefone
////////                if (  fieldInfo.Telefone != string.Empty ) 
////////                   if ( fieldInfo.Telefone.Trim().Length > 50  )
////////                      throw new Exception("O campo \"Telefone\" deve ter comprimento máximo de 50 caracter(es).");
////////
////////
////////                //Field Endereco
////////                if (  fieldInfo.Endereco != string.Empty ) 
////////                   if ( fieldInfo.Endereco.Trim().Length > 150  )
////////                      throw new Exception("O campo \"Endereco\" deve ter comprimento máximo de 150 caracter(es).");
////////
////////
////////                //Field Bairro
////////                if (  fieldInfo.Bairro != string.Empty ) 
////////                   if ( fieldInfo.Bairro.Trim().Length > 150  )
////////                      throw new Exception("O campo \"Bairro\" deve ter comprimento máximo de 150 caracter(es).");
////////
////////
////////                //Field Cidade
////////                if (  fieldInfo.Cidade != string.Empty ) 
////////                   if ( fieldInfo.Cidade.Trim().Length > 150  )
////////                      throw new Exception("O campo \"Cidade\" deve ter comprimento máximo de 150 caracter(es).");
////////
////////
////////                //Field Estado
////////                if (  fieldInfo.Estado != string.Empty ) 
////////                   if ( fieldInfo.Estado.Trim().Length > 150  )
////////                      throw new Exception("O campo \"Estado\" deve ter comprimento máximo de 150 caracter(es).");
////////
////////
////////                //Field idUsuarioRecebe
////////                if ( !( fieldInfo.idUsuarioRecebe > 0 ) )
////////                   throw new Exception("O campo \"idUsuarioRecebe\" deve ser maior que zero.");
////////
////////
////////                //Field idUsuarioIndica
////////                if ( !( fieldInfo.idUsuarioIndica > 0 ) )
////////                   throw new Exception("O campo \"idUsuarioIndica\" deve ser maior que zero.");
////////
////////                return true;
////////
////////            }
////////            catch (Exception e)
////////            {
////////                this._ErrorMessage = e.Message;
////////                return false;
////////            }
////////
////////        }
////////    }
////////
////////}
////////
////////
////////
////////
////////
////////
//////////Projeto substituído ------------------------
//////////using System;
//////////using System.Text;
//////////using System.Text.RegularExpressions;
//////////
//////////namespace SWGPgen
//////////{
//////////
//////////
//////////    /// <summary> 
//////////    /// Autor: DAL Creator .net  
//////////    /// Data de criação: 19/03/2012 22:46:51 
//////////    /// Descrição: Classe que valida o objeto "IndicacaoFields". 
//////////    /// </summary> 
//////////    public class IndicacaoValidator 
//////////    {
//////////
//////////
//////////        #region Propriedade que armazena erros de execução 
//////////        private string _ErrorMessage = string.Empty;
//////////        public string ErrorMessage { get { return _ErrorMessage; } }
//////////        #endregion
//////////
//////////
//////////        public IndicacaoValidator() {}
//////////
//////////
//////////        public bool isValid( IndicacaoFields fieldInfo )
//////////        {
//////////            try
//////////            {
//////////
//////////
//////////                //Field Nome
//////////                if (  fieldInfo.Nome != string.Empty ) 
//////////                   if ( fieldInfo.Nome.Trim().Length > 150  )
//////////                      throw new Exception("O campo \"Nome\" deve ter comprimento máximo de 150 caracter(es).");
//////////                if ( ( fieldInfo.Nome == string.Empty ) || ( fieldInfo.Nome.Trim().Length < 1 ) )
//////////                   throw new Exception("O campo \"Nome\" não pode ser nulo ou vazio e deve ter comprimento mínimo de 1 caracter(es).");
//////////
//////////
//////////                //Field Endereco
//////////                if (  fieldInfo.Endereco != string.Empty ) 
//////////                   if ( fieldInfo.Endereco.Trim().Length > 200  )
//////////                      throw new Exception("O campo \"Endereco\" deve ter comprimento máximo de 200 caracter(es).");
//////////
//////////
//////////                //Field Bairro
//////////                if (  fieldInfo.Bairro != string.Empty ) 
//////////                   if ( fieldInfo.Bairro.Trim().Length > 100  )
//////////                      throw new Exception("O campo \"Bairro\" deve ter comprimento máximo de 100 caracter(es).");
//////////
//////////
//////////                //Field Cidade
//////////                if (  fieldInfo.Cidade != string.Empty ) 
//////////                   if ( fieldInfo.Cidade.Trim().Length > 150  )
//////////                      throw new Exception("O campo \"Cidade\" deve ter comprimento máximo de 150 caracter(es).");
//////////
//////////
//////////                //Field Estado
//////////                if (  fieldInfo.Estado != string.Empty ) 
//////////                   if ( fieldInfo.Estado.Trim().Length > 2  )
//////////                      throw new Exception("O campo \"Estado\" deve ter comprimento máximo de 2 caracter(es).");
//////////
//////////
//////////                //Field Telefone
//////////                if (  fieldInfo.Telefone != string.Empty ) 
//////////                   if ( fieldInfo.Telefone.Trim().Length > 11  )
//////////                      throw new Exception("O campo \"Telefone\" deve ter comprimento máximo de 11 caracter(es).");
//////////
//////////
//////////                //Field FkPosicaoIndicacao
//////////                if ( !( fieldInfo.FkPosicaoIndicacao > 0 ) )
//////////                   throw new Exception("O campo \"FkPosicaoIndicacao\" deve ser maior que zero.");
//////////
//////////                return true;
//////////
//////////            }
//////////            catch (Exception e)
//////////            {
//////////                this._ErrorMessage = e.Message;
//////////                return false;
//////////            }
//////////
//////////        }
//////////    }
//////////
//////////}
//////////
//////////
//////////
//////////
//////////
//////////
////////////Projeto substituído ------------------------
////////////using System;
////////////using System.Text;
////////////using System.Text.RegularExpressions;
////////////
////////////namespace SWGPgen
////////////{
////////////
////////////
////////////    /// <summary> 
////////////    /// Autor: DAL Creator .net  
////////////    /// Data de criação: 13/03/2012 21:19:06 
////////////    /// Descrição: Classe que valida o objeto "IndicacaoFields". 
////////////    /// </summary> 
////////////    public class IndicacaoValidator 
////////////    {
////////////
////////////
////////////        #region Propriedade que armazena erros de execução 
////////////        private string _ErrorMessage = string.Empty;
////////////        public string ErrorMessage { get { return _ErrorMessage; } }
////////////        #endregion
////////////
////////////
////////////        public IndicacaoValidator() {}
////////////
////////////
////////////        public bool isValid( IndicacaoFields fieldInfo )
////////////        {
////////////            try
////////////            {
////////////
////////////
////////////                //Field Nome
////////////                if (  fieldInfo.Nome != string.Empty ) 
////////////                   if ( fieldInfo.Nome.Trim().Length > 150  )
////////////                      throw new Exception("O campo \"Nome\" deve ter comprimento máximo de 150 caracter(es).");
////////////                if ( ( fieldInfo.Nome == string.Empty ) || ( fieldInfo.Nome.Trim().Length < 1 ) )
////////////                   throw new Exception("O campo \"Nome\" não pode ser nulo ou vazio e deve ter comprimento mínimo de 1 caracter(es).");
////////////
////////////
////////////                //Field Endereco
////////////                if (  fieldInfo.Endereco != string.Empty ) 
////////////                   if ( fieldInfo.Endereco.Trim().Length > 200  )
////////////                      throw new Exception("O campo \"Endereco\" deve ter comprimento máximo de 200 caracter(es).");
////////////
////////////
////////////                //Field Bairro
////////////                if (  fieldInfo.Bairro != string.Empty ) 
////////////                   if ( fieldInfo.Bairro.Trim().Length > 100  )
////////////                      throw new Exception("O campo \"Bairro\" deve ter comprimento máximo de 100 caracter(es).");
////////////
////////////
////////////                //Field Cidade
////////////                if (  fieldInfo.Cidade != string.Empty ) 
////////////                   if ( fieldInfo.Cidade.Trim().Length > 150  )
////////////                      throw new Exception("O campo \"Cidade\" deve ter comprimento máximo de 150 caracter(es).");
////////////
////////////
////////////                //Field Estado
////////////                if (  fieldInfo.Estado != string.Empty ) 
////////////                   if ( fieldInfo.Estado.Trim().Length > 2  )
////////////                      throw new Exception("O campo \"Estado\" deve ter comprimento máximo de 2 caracter(es).");
////////////
////////////
////////////                //Field Telefone
////////////                if (  fieldInfo.Telefone != string.Empty ) 
////////////                   if ( fieldInfo.Telefone.Trim().Length > 11  )
////////////                      throw new Exception("O campo \"Telefone\" deve ter comprimento máximo de 11 caracter(es).");
////////////
////////////
////////////                //Field FkPosicaoIndicacao
////////////                if ( !( fieldInfo.FkPosicaoIndicacao > 0 ) )
////////////                   throw new Exception("O campo \"FkPosicaoIndicacao\" deve ser maior que zero.");
////////////
////////////                return true;
////////////
////////////            }
////////////            catch (Exception e)
////////////            {
////////////                this._ErrorMessage = e.Message;
////////////                return false;
////////////            }
////////////
////////////        }
////////////    }
////////////
////////////}
////////////
