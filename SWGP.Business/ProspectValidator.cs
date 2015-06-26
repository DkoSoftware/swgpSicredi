using System;
using System.Text;
using System.Text.RegularExpressions;

namespace SIMLgen
{


    /// <summary> 
    /// Autor: DAL Creator .net  
    /// Data de criação: 07/05/2012 21:02:06 
    /// Descrição: Classe que valida o objeto "ProspectFields". 
    /// </summary> 
    public class ProspectValidator 
    {


        #region Propriedade que armazena erros de execução 
        private string _ErrorMessage = string.Empty;
        public string ErrorMessage { get { return _ErrorMessage; } }
        #endregion


        public ProspectValidator() {}


        public bool isValid( ProspectFields fieldInfo )
        {
            try
            {


                //Field Nome
                if (  fieldInfo.Nome != string.Empty ) 
                   if ( fieldInfo.Nome.Trim().Length > 150  )
                      throw new Exception("O campo \"Nome\" deve ter comprimento máximo de 150 caracter(es).");
                if ( ( fieldInfo.Nome == string.Empty ) || ( fieldInfo.Nome.Trim().Length < 1 ) )
                   throw new Exception("O campo \"Nome\" não pode ser nulo ou vazio e deve ter comprimento mínimo de 1 caracter(es).");


                //Field Endereco
                if (  fieldInfo.Endereco != string.Empty ) 
                   if ( fieldInfo.Endereco.Trim().Length > 250  )
                      throw new Exception("O campo \"Endereco\" deve ter comprimento máximo de 250 caracter(es).");


                //Field Telefone
                if (  fieldInfo.Telefone != string.Empty ) 
                   if ( fieldInfo.Telefone.Trim().Length > 11  )
                      throw new Exception("O campo \"Telefone\" deve ter comprimento máximo de 11 caracter(es).");


                //Field Tipo
                if (  fieldInfo.Tipo != string.Empty ) 
                   if ( fieldInfo.Tipo.Trim().Length > 2  )
                      throw new Exception("O campo \"Tipo\" deve ter comprimento máximo de 2 caracter(es).");


                //Field Segmento
                if (  fieldInfo.Segmento != string.Empty ) 
                   if ( fieldInfo.Segmento.Trim().Length > 30  )
                      throw new Exception("O campo \"Segmento\" deve ter comprimento máximo de 30 caracter(es).");


                //Field Observacao
                if (  fieldInfo.Observacao != string.Empty ) 
                   if ( fieldInfo.Observacao.Trim().Length > 300  )
                      throw new Exception("O campo \"Observacao\" deve ter comprimento máximo de 300 caracter(es).");


                //Field Email
                if (  fieldInfo.Email != string.Empty ) 
                   if ( fieldInfo.Email.Trim().Length > 50  )
                      throw new Exception("O campo \"Email\" deve ter comprimento máximo de 50 caracter(es).");


                //Field Bairro
                if (  fieldInfo.Bairro != string.Empty ) 
                   if ( fieldInfo.Bairro.Trim().Length > 100  )
                      throw new Exception("O campo \"Bairro\" deve ter comprimento máximo de 100 caracter(es).");


                //Field Cidade
                if (  fieldInfo.Cidade != string.Empty ) 
                   if ( fieldInfo.Cidade.Trim().Length > 100  )
                      throw new Exception("O campo \"Cidade\" deve ter comprimento máximo de 100 caracter(es).");


                //Field Estado
                if (  fieldInfo.Estado != string.Empty ) 
                   if ( fieldInfo.Estado.Trim().Length > 2  )
                      throw new Exception("O campo \"Estado\" deve ter comprimento máximo de 2 caracter(es).");


                //Field PessoaContato
                if (  fieldInfo.PessoaContato != string.Empty ) 
                   if ( fieldInfo.PessoaContato.Trim().Length > 150  )
                      throw new Exception("O campo \"PessoaContato\" deve ter comprimento máximo de 150 caracter(es).");


                //Field CPF
                if (  fieldInfo.CPF != string.Empty ) 
                   if ( fieldInfo.CPF.Trim().Length > 50  )
                      throw new Exception("O campo \"CPF\" deve ter comprimento máximo de 50 caracter(es).");


                //Field CNPJ
                if (  fieldInfo.CNPJ != string.Empty ) 
                   if ( fieldInfo.CNPJ.Trim().Length > 50  )
                      throw new Exception("O campo \"CNPJ\" deve ter comprimento máximo de 50 caracter(es).");


                //Field FkUsuario
                if ( !( fieldInfo.FkUsuario > 0 ) )
                   throw new Exception("O campo \"FkUsuario\" deve ser maior que zero.");


                //Field SituacaoProspect
                if (  fieldInfo.SituacaoProspect != string.Empty ) 
                   if ( fieldInfo.SituacaoProspect.Trim().Length > 20  )
                      throw new Exception("O campo \"SituacaoProspect\" deve ter comprimento máximo de 20 caracter(es).");


                //Field fkIndicacao
                if ( !( fieldInfo.fkIndicacao > 0 ) )
                   throw new Exception("O campo \"fkIndicacao\" deve ser maior que zero.");

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

