using System;
using System.Collections;
using System.Text;

namespace SWGPgen
{


    /// <summary> 
    /// Tabela: PosicaoIndicacao  
    /// Autor: DAL Creator .net  
    /// Data de cria��o: 19/03/2012 22:46:51 
    /// Descri��o: Classe que retorna todos os campos/propriedades da tabela PosicaoIndicacao 
    /// </summary> 
    public class PosicaoIndicacaoFields 
    {

        private int _idPosicaoIndicacao = 0;


        /// <summary>  
        /// Tipo de dados (DataBase): Int 
        /// Somente Leitura/Auto Incremental
        /// </summary> 
        public int idPosicaoIndicacao
        {
            get { return _idPosicaoIndicacao; }
            set { _idPosicaoIndicacao = value; }
        }

        private string _NomeUsuarioRecebe = string.Empty;


        /// <summary>  
        /// Tipo de dados (DataBase): nvarchar 
        /// Preenchimento obrigat�rio:  N�o 
        /// Estilo: Normal  
        /// Tamanho M�ximo: 50 
        /// </summary> 
        public string NomeUsuarioRecebe
        {
            get { return _NomeUsuarioRecebe.Trim(); }
            set { _NomeUsuarioRecebe = value.Trim(); }
        }

        private string _NomeUsuarioIndica = string.Empty;


        /// <summary>  
        /// Tipo de dados (DataBase): nvarchar 
        /// Preenchimento obrigat�rio:  N�o 
        /// Estilo: Normal  
        /// Tamanho M�ximo: 50 
        /// </summary> 
        public string NomeUsuarioIndica
        {
            get { return _NomeUsuarioIndica.Trim(); }
            set { _NomeUsuarioIndica = value.Trim(); }
        }



        public PosicaoIndicacaoFields() {}

        public PosicaoIndicacaoFields(
                        string Param_NomeUsuarioRecebe, 
                        string Param_NomeUsuarioIndica)
        {
               this._NomeUsuarioRecebe = Param_NomeUsuarioRecebe;
               this._NomeUsuarioIndica = Param_NomeUsuarioIndica;
        }
    }

}




//Projeto substitu�do ------------------------
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Text;
//
//namespace SWGPgen
//{
//
//
//    /// <summary> 
//    /// Tabela: PosicaoIndicacao  
//    /// Autor: DAL Creator .net  
//    /// Data de cria��o: 13/03/2012 21:19:06 
//    /// Descri��o: Classe que retorna todos os campos/propriedades da tabela PosicaoIndicacao 
//    /// </summary> 
//    public class PosicaoIndicacaoFields 
//    {
//
//        private int _idPosicaoIndicacao = 0;
//
//
//        /// <summary>  
//        /// Tipo de dados (DataBase): Int 
//        /// Somente Leitura/Auto Incremental
//        /// </summary> 
//        public int idPosicaoIndicacao
//        {
//            get { return _idPosicaoIndicacao; }
//            set { _idPosicaoIndicacao = value; }
//        }
//
//        private string _NomeUsuarioRecebe = string.Empty;
//
//
//        /// <summary>  
//        /// Tipo de dados (DataBase): nvarchar 
//        /// Preenchimento obrigat�rio:  N�o 
//        /// Estilo: Normal  
//        /// Tamanho M�ximo: 50 
//        /// </summary> 
//        public string NomeUsuarioRecebe
//        {
//            get { return _NomeUsuarioRecebe.Trim(); }
//            set { _NomeUsuarioRecebe = value.Trim(); }
//        }
//
//        private string _NomeUsuarioIndica = string.Empty;
//
//
//        /// <summary>  
//        /// Tipo de dados (DataBase): nvarchar 
//        /// Preenchimento obrigat�rio:  N�o 
//        /// Estilo: Normal  
//        /// Tamanho M�ximo: 50 
//        /// </summary> 
//        public string NomeUsuarioIndica
//        {
//            get { return _NomeUsuarioIndica.Trim(); }
//            set { _NomeUsuarioIndica = value.Trim(); }
//        }
//
//
//
//        public PosicaoIndicacaoFields() {}
//
//        public PosicaoIndicacaoFields(
//                        string Param_NomeUsuarioRecebe, 
//                        string Param_NomeUsuarioIndica)
//        {
//               this._NomeUsuarioRecebe = Param_NomeUsuarioRecebe;
//               this._NomeUsuarioIndica = Param_NomeUsuarioIndica;
//        }
//    }
//
//}
