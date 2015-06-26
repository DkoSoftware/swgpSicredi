using System;
using System.Collections;
using System.Text;

namespace SWGPgen
{


    /// <summary> 
    /// Tabela: Contato  
    /// Autor: DAL Creator .net  
    /// Data de cria��o: 19/03/2012 22:46:51 
    /// Descri��o: Classe que retorna todos os campos/propriedades da tabela Contato 
    /// </summary> 
    public class ContatoFields 
    {

        private int _idContato = 0;


        /// <summary>  
        /// Tipo de dados (DataBase): Int 
        /// Somente Leitura/Auto Incremental
        /// </summary> 
        public int idContato
        {
            get { return _idContato; }
            set { _idContato = value; }
        }

        private string _Tipo = string.Empty;


        /// <summary>  
        /// Tipo de dados (DataBase): nvarchar 
        /// Preenchimento obrigat�rio:  N�o 
        /// Estilo: Normal  
        /// Tamanho M�ximo: 30 
        /// </summary> 
        public string Tipo
        {
            get { return _Tipo.Trim(); }
            set { _Tipo = value.Trim(); }
        }

        private string _Descricao = string.Empty;


        /// <summary>  
        /// Tipo de dados (DataBase): nvarchar 
        /// Preenchimento obrigat�rio:  N�o 
        /// Estilo: Normal  
        /// Tamanho M�ximo: 300 
        /// </summary> 
        public string Descricao
        {
            get { return _Descricao.Trim(); }
            set { _Descricao = value.Trim(); }
        }

        private string _Situacao = string.Empty;


        /// <summary>  
        /// Tipo de dados (DataBase): nvarchar 
        /// Preenchimento obrigat�rio:  N�o 
        /// Estilo: Normal  
        /// Tamanho M�ximo: 30 
        /// </summary> 
        public string Situacao
        {
            get { return _Situacao.Trim(); }
            set { _Situacao = value.Trim(); }
        }

        private DateTime _DataContato = DateTime.MinValue;


        /// <summary>  
        /// Tipo de dados (DataBase): datetime 
        /// Preenchimento obrigat�rio:  N�o 
        /// </summary> 
        public DateTime DataContato
        {
            get { return _DataContato; }
            set { _DataContato = value; }
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

        private int _fkProspect = 0;


        /// <summary>  
        /// Tipo de dados (DataBase): int 
        /// Preenchimento obrigat�rio:  Sim 
        /// Permitido:  Maior que zero 
        /// </summary> 
        public int fkProspect
        {
            get { return _fkProspect; }
            set { _fkProspect = value; }
        }



        public ContatoFields() {}

        public ContatoFields(
                        string Param_Tipo, 
                        string Param_Descricao, 
                        string Param_Situacao, 
                        DateTime Param_DataContato, 
                        DateTime Param_DataCadastro, 
                        int Param_fkProspect)
        {
               this._Tipo = Param_Tipo;
               this._Descricao = Param_Descricao;
               this._Situacao = Param_Situacao;
               this._DataContato = Param_DataContato;
               this._DataCadastro = Param_DataCadastro;
               this._fkProspect = Param_fkProspect;
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
//    /// Tabela: Contato  
//    /// Autor: DAL Creator .net  
//    /// Data de cria��o: 13/03/2012 21:19:06 
//    /// Descri��o: Classe que retorna todos os campos/propriedades da tabela Contato 
//    /// </summary> 
//    public class ContatoFields 
//    {
//
//        private int _idContato = 0;
//
//
//        /// <summary>  
//        /// Tipo de dados (DataBase): Int 
//        /// Somente Leitura/Auto Incremental
//        /// </summary> 
//        public int idContato
//        {
//            get { return _idContato; }
//            set { _idContato = value; }
//        }
//
//        private string _Tipo = string.Empty;
//
//
//        /// <summary>  
//        /// Tipo de dados (DataBase): nvarchar 
//        /// Preenchimento obrigat�rio:  N�o 
//        /// Estilo: Normal  
//        /// Tamanho M�ximo: 30 
//        /// </summary> 
//        public string Tipo
//        {
//            get { return _Tipo.Trim(); }
//            set { _Tipo = value.Trim(); }
//        }
//
//        private string _Descricao = string.Empty;
//
//
//        /// <summary>  
//        /// Tipo de dados (DataBase): nvarchar 
//        /// Preenchimento obrigat�rio:  N�o 
//        /// Estilo: Normal  
//        /// Tamanho M�ximo: 300 
//        /// </summary> 
//        public string Descricao
//        {
//            get { return _Descricao.Trim(); }
//            set { _Descricao = value.Trim(); }
//        }
//
//        private string _Situacao = string.Empty;
//
//
//        /// <summary>  
//        /// Tipo de dados (DataBase): nvarchar 
//        /// Preenchimento obrigat�rio:  N�o 
//        /// Estilo: Normal  
//        /// Tamanho M�ximo: 30 
//        /// </summary> 
//        public string Situacao
//        {
//            get { return _Situacao.Trim(); }
//            set { _Situacao = value.Trim(); }
//        }
//
//        private DateTime _DataContato = DateTime.MinValue;
//
//
//        /// <summary>  
//        /// Tipo de dados (DataBase): datetime 
//        /// Preenchimento obrigat�rio:  N�o 
//        /// </summary> 
//        public DateTime DataContato
//        {
//            get { return _DataContato; }
//            set { _DataContato = value; }
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
//
//
//        public ContatoFields() {}
//
//        public ContatoFields(
//                        string Param_Tipo, 
//                        string Param_Descricao, 
//                        string Param_Situacao, 
//                        DateTime Param_DataContato, 
//                        DateTime Param_DataCadastro, 
//                        int Param_fkProspect)
//        {
//               this._Tipo = Param_Tipo;
//               this._Descricao = Param_Descricao;
//               this._Situacao = Param_Situacao;
//               this._DataContato = Param_DataContato;
//               this._DataCadastro = Param_DataCadastro;
//               this._fkProspect = Param_fkProspect;
//        }
//    }
//
//}
