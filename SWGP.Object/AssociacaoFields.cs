using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SWGPgen
{


    /// <summary> 
    /// Tabela: Associacao  
    /// Autor: DAL Creator .net  
    /// Data de cria��o: 25/03/2012 03:26:53 
    /// Descri��o: Classe que retorna todos os campos/propriedades da tabela Associacao 
    /// </summary> 
    public class AssociacaoFields 
    {

        private int _idAssociacao = 0;


        /// <summary>  
        /// Tipo de dados (DataBase): Int 
        /// Somente Leitura/Auto Incremental
        /// </summary> 
        public int idAssociacao
        {
            get { return _idAssociacao; }
            set { _idAssociacao = value; }
        }

        private int _fkContato = 0;


        /// <summary>  
        /// Tipo de dados (DataBase): int 
        /// Preenchimento obrigat�rio:  Sim 
        /// Permitido:  Maior que zero 
        /// </summary> 
        public int fkContato
        {
            get { return _fkContato; }
            set { _fkContato = value; }
        }

        private string _NumeroConta = string.Empty;


        /// <summary>  
        /// Tipo de dados (DataBase): nvarchar 
        /// Preenchimento obrigat�rio:  Sim 
        /// Estilo: Normal  
        /// Tamanho M�ximo: 7 
        /// </summary> 
        public string NumeroConta
        {
            get { return _NumeroConta.Trim(); }
            set { _NumeroConta = value.Trim(); }
        }

        private DateTime _DataAssociacao = DateTime.MinValue;


        /// <summary>  
        /// Tipo de dados (DataBase): datetime 
        /// Preenchimento obrigat�rio:  N�o 
        /// </summary> 
        public DateTime DataAssociacao
        {
            get { return _DataAssociacao; }
            set { _DataAssociacao = value; }
        }

        private DateTime _DataCadastro = DateTime.MinValue;


        /// <summary>  
        /// Tipo de dados (DataBase): datetime 
        /// Preenchimento obrigat�rio:  N�o 
        /// </summary> 
        public DateTime DataCadastro
        {
            get { return _DataCadastro; }
            set { _DataCadastro = value; }
        }



        public AssociacaoFields() {}

        public AssociacaoFields(
                        int Param_fkContato, 
                        string Param_NumeroConta, 
                        DateTime Param_DataAssociacao, 
                        DateTime Param_DataCadastro)
        {
               this._fkContato = Param_fkContato;
               this._NumeroConta = Param_NumeroConta;
               this._DataAssociacao = Param_DataAssociacao;
               this._DataCadastro = Param_DataCadastro;
        }
    }

}




//Projeto substitu�do ------------------------
//using System;
//using System.Collections;
//using System.Text;
//
//namespace SWGPgen
//{
//
//
//    /// <summary> 
//    /// Tabela: Associacao  
//    /// Autor: DAL Creator .net  
//    /// Data de cria��o: 19/03/2012 22:46:51 
//    /// Descri��o: Classe que retorna todos os campos/propriedades da tabela Associacao 
//    /// </summary> 
//    public class AssociacaoFields 
//    {
//
//        private int _idAssociacao = 0;
//
//
//        /// <summary>  
//        /// Tipo de dados (DataBase): Int 
//        /// Somente Leitura/Auto Incremental
//        /// </summary> 
//        public int idAssociacao
//        {
//            get { return _idAssociacao; }
//            set { _idAssociacao = value; }
//        }
//
//        private int _fkProspect = 0;
//
//
//        /// <summary>  
//        /// Tipo de dados (DataBase): int 
//        /// Preenchimento obrigat�rio:  Sim 
//        /// Permitido:  Maior que zero 
//        /// </summary> 
//        public int fkProspect
//        {
//            get { return _fkProspect; }
//            set { _fkProspect = value; }
//        }
//
//        private string _NumeroConta = string.Empty;
//
//
//        /// <summary>  
//        /// Tipo de dados (DataBase): nvarchar 
//        /// Preenchimento obrigat�rio:  Sim 
//        /// Estilo: Normal  
//        /// Tamanho M�ximo: 7 
//        /// </summary> 
//        public string NumeroConta
//        {
//            get { return _NumeroConta.Trim(); }
//            set { _NumeroConta = value.Trim(); }
//        }
//
//        private DateTime _DataAssociacao = DateTime.MinValue;
//
//
//        /// <summary>  
//        /// Tipo de dados (DataBase): datetime 
//        /// Preenchimento obrigat�rio:  N�o 
//        /// </summary> 
//        public DateTime DataAssociacao
//        {
//            get { return _DataAssociacao; }
//            set { _DataAssociacao = value; }
//        }
//
//        private DateTime _DataCadastro = DateTime.MinValue;
//
//
//        /// <summary>  
//        /// Tipo de dados (DataBase): datetime 
//        /// Preenchimento obrigat�rio:  N�o 
//        /// </summary> 
//        public DateTime DataCadastro
//        {
//            get { return _DataCadastro; }
//            set { _DataCadastro = value; }
//        }
//
//
//
//        public AssociacaoFields() {}
//
//        public AssociacaoFields(
//                        int Param_fkProspect, 
//                        string Param_NumeroConta, 
//                        DateTime Param_DataAssociacao, 
//                        DateTime Param_DataCadastro)
//        {
//               this._fkProspect = Param_fkProspect;
//               this._NumeroConta = Param_NumeroConta;
//               this._DataAssociacao = Param_DataAssociacao;
//               this._DataCadastro = Param_DataCadastro;
//        }
//    }
//
//}
//
//
//
//
////Projeto substitu�do ------------------------
////using System;
////using System.Collections;
////using System.Collections.Generic;
////using System.Text;
////
////namespace SWGPgen
////{
////
////
////    /// <summary> 
////    /// Tabela: Associacao  
////    /// Autor: DAL Creator .net  
////    /// Data de cria��o: 13/03/2012 21:19:06 
////    /// Descri��o: Classe que retorna todos os campos/propriedades da tabela Associacao 
////    /// </summary> 
////    public class AssociacaoFields 
////    {
////
////        private int _idAssociacao = 0;
////
////
////        /// <summary>  
////        /// Tipo de dados (DataBase): Int 
////        /// Somente Leitura/Auto Incremental
////        /// </summary> 
////        public int idAssociacao
////        {
////            get { return _idAssociacao; }
////            set { _idAssociacao = value; }
////        }
////
////        private int _fkProspect = 0;
////
////
////        /// <summary>  
////        /// Tipo de dados (DataBase): int 
////        /// Preenchimento obrigat�rio:  Sim 
////        /// Permitido:  Maior que zero 
////        /// </summary> 
////        public int fkProspect
////        {
////            get { return _fkProspect; }
////            set { _fkProspect = value; }
////        }
////
////        private string _NumeroConta = string.Empty;
////
////
////        /// <summary>  
////        /// Tipo de dados (DataBase): nvarchar 
////        /// Preenchimento obrigat�rio:  Sim 
////        /// Estilo: Normal  
////        /// Tamanho M�ximo: 7 
////        /// </summary> 
////        public string NumeroConta
////        {
////            get { return _NumeroConta.Trim(); }
////            set { _NumeroConta = value.Trim(); }
////        }
////
////        private DateTime _DataAssociacao = DateTime.MinValue;
////
////
////        /// <summary>  
////        /// Tipo de dados (DataBase): datetime 
////        /// Preenchimento obrigat�rio:  N�o 
////        /// </summary> 
////        public DateTime DataAssociacao
////        {
////            get { return _DataAssociacao; }
////            set { _DataAssociacao = value; }
////        }
////
////        private DateTime _DataCadastro = DateTime.MinValue;
////
////
////        /// <summary>  
////        /// Tipo de dados (DataBase): datetime 
////        /// Preenchimento obrigat�rio:  N�o 
////        /// </summary> 
////        public DateTime DataCadastro
////        {
////            get { return _DataCadastro; }
////            set { _DataCadastro = value; }
////        }
////
////
////
////        public AssociacaoFields() {}
////
////        public AssociacaoFields(
////                        int Param_fkProspect, 
////                        string Param_NumeroConta, 
////                        DateTime Param_DataAssociacao, 
////                        DateTime Param_DataCadastro)
////        {
////               this._fkProspect = Param_fkProspect;
////               this._NumeroConta = Param_NumeroConta;
////               this._DataAssociacao = Param_DataAssociacao;
////               this._DataCadastro = Param_DataCadastro;
////        }
////    }
////
////}
