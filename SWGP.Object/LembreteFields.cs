using System;
using System.Collections;
using System.Text;

namespace SWGPgen
{


    /// <summary> 
    /// Tabela: Lembrete  
    /// Autor: DAL Creator .net  
    /// Data de criação: 19/03/2012 22:46:51 
    /// Descrição: Classe que retorna todos os campos/propriedades da tabela Lembrete 
    /// </summary> 
    public class LembreteFields 
    {

        private int _idLembrete = 0;


        /// <summary>  
        /// Tipo de dados (DataBase): Int 
        /// Somente Leitura/Auto Incremental
        /// </summary> 
        public int idLembrete
        {
            get { return _idLembrete; }
            set { _idLembrete = value; }
        }

        private string _Descricao = string.Empty;


        /// <summary>  
        /// Tipo de dados (DataBase): nvarchar 
        /// Preenchimento obrigatório:  Não 
        /// Estilo: Normal  
        /// Tamanho Máximo: 250 
        /// </summary> 
        public string Descricao
        {
            get { return _Descricao.Trim(); }
            set { _Descricao = value.Trim(); }
        }

        private DateTime _DataCadastro = DateTime.MinValue;


        /// <summary>  
        /// Tipo de dados (DataBase): datetime 
        /// Preenchimento obrigatório:  Não 
        /// </summary> 
        public DateTime DataCadastro
        {
            get { return _DataCadastro; }
            set { _DataCadastro = value; }
        }

        private DateTime _DataLembrar = DateTime.MinValue;


        /// <summary>  
        /// Tipo de dados (DataBase): datetime 
        /// Preenchimento obrigatório:  Não 
        /// </summary> 
        public DateTime DataLembrar
        {
            get { return _DataLembrar; }
            set { _DataLembrar = value; }
        }

        private int _FkUsuario = 0;


        /// <summary>  
        /// Tipo de dados (DataBase): int 
        /// Preenchimento obrigatório:  Sim 
        /// Permitido:  Maior que zero 
        /// </summary> 
        public int FkUsuario
        {
            get { return _FkUsuario; }
            set { _FkUsuario = value; }
        }



        public LembreteFields() {}

        public LembreteFields(
                        string Param_Descricao, 
                        DateTime Param_DataCadastro, 
                        DateTime Param_DataLembrar, 
                        int Param_FkUsuario)
        {
               this._Descricao = Param_Descricao;
               this._DataCadastro = Param_DataCadastro;
               this._DataLembrar = Param_DataLembrar;
               this._FkUsuario = Param_FkUsuario;
        }
    }

}




//Projeto substituído ------------------------
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
//    /// Tabela: Lembrete  
//    /// Autor: DAL Creator .net  
//    /// Data de criação: 13/03/2012 21:19:06 
//    /// Descrição: Classe que retorna todos os campos/propriedades da tabela Lembrete 
//    /// </summary> 
//    public class LembreteFields 
//    {
//
//        private int _idLembrete = 0;
//
//
//        /// <summary>  
//        /// Tipo de dados (DataBase): Int 
//        /// Somente Leitura/Auto Incremental
//        /// </summary> 
//        public int idLembrete
//        {
//            get { return _idLembrete; }
//            set { _idLembrete = value; }
//        }
//
//        private string _Descricao = string.Empty;
//
//
//        /// <summary>  
//        /// Tipo de dados (DataBase): nvarchar 
//        /// Preenchimento obrigatório:  Não 
//        /// Estilo: Normal  
//        /// Tamanho Máximo: 250 
//        /// </summary> 
//        public string Descricao
//        {
//            get { return _Descricao.Trim(); }
//            set { _Descricao = value.Trim(); }
//        }
//
//        private DateTime _DataCadastro = DateTime.MinValue;
//
//
//        /// <summary>  
//        /// Tipo de dados (DataBase): datetime 
//        /// Preenchimento obrigatório:  Não 
//        /// </summary> 
//        public DateTime DataCadastro
//        {
//            get { return _DataCadastro; }
//            set { _DataCadastro = value; }
//        }
//
//        private DateTime _DataLembrar = DateTime.MinValue;
//
//
//        /// <summary>  
//        /// Tipo de dados (DataBase): datetime 
//        /// Preenchimento obrigatório:  Não 
//        /// </summary> 
//        public DateTime DataLembrar
//        {
//            get { return _DataLembrar; }
//            set { _DataLembrar = value; }
//        }
//
//        private int _FkUsuario = 0;
//
//
//        /// <summary>  
//        /// Tipo de dados (DataBase): int 
//        /// Preenchimento obrigatório:  Sim 
//        /// Permitido:  Maior que zero 
//        /// </summary> 
//        public int FkUsuario
//        {
//            get { return _FkUsuario; }
//            set { _FkUsuario = value; }
//        }
//
//
//
//        public LembreteFields() {}
//
//        public LembreteFields(
//                        string Param_Descricao, 
//                        DateTime Param_DataCadastro, 
//                        DateTime Param_DataLembrar, 
//                        int Param_FkUsuario)
//        {
//               this._Descricao = Param_Descricao;
//               this._DataCadastro = Param_DataCadastro;
//               this._DataLembrar = Param_DataLembrar;
//               this._FkUsuario = Param_FkUsuario;
//        }
//    }
//
//}
