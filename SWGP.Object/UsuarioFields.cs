using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SWGPgen
{


    /// <summary> 
    /// Tabela: Usuario  
    /// Autor: DAL Creator .net  
    /// Data de criação: 24/03/2012 20:08:47 
    /// Descrição: Classe que retorna todos os campos/propriedades da tabela Usuario 
    /// </summary> 
    public class UsuarioFields 
    {

        private int _idUsuario = 0;


        /// <summary>  
        /// Tipo de dados (DataBase): Int 
        /// Somente Leitura/Auto Incremental
        /// </summary> 
        public int idUsuario
        {
            get { return _idUsuario; }
            set { _idUsuario = value; }
        }

        private string _Nome = string.Empty;


        /// <summary>  
        /// Tipo de dados (DataBase): nvarchar 
        /// Preenchimento obrigatório:  Não 
        /// Estilo: Normal  
        /// Tamanho Máximo: 200 
        /// </summary> 
        public string Nome
        {
            get { return _Nome.Trim(); }
            set { _Nome = value.Trim(); }
        }

        private string _UserName = string.Empty;


        /// <summary>  
        /// Tipo de dados (DataBase): nvarchar 
        /// Preenchimento obrigatório:  Não 
        /// Estilo: Normal  
        /// Tamanho Máximo: 100 
        /// </summary> 
        public string UserName
        {
            get { return _UserName.Trim(); }
            set { _UserName = value.Trim(); }
        }

        private string _Password = string.Empty;


        /// <summary>  
        /// Tipo de dados (DataBase): nvarchar 
        /// Preenchimento obrigatório:  Não 
        /// Estilo: Normal  
        /// Tamanho Máximo: 100 
        /// </summary> 
        public string Password
        {
            get { return _Password.Trim(); }
            set { _Password = value.Trim(); }
        }

        private string _Cargo = string.Empty;


        /// <summary>  
        /// Tipo de dados (DataBase): nvarchar 
        /// Preenchimento obrigatório:  Não 
        /// Estilo: Normal  
        /// Tamanho Máximo: 50 
        /// </summary> 
        public string Cargo
        {
            get { return _Cargo.Trim(); }
            set { _Cargo = value.Trim(); }
        }

        private string _Situacao = string.Empty;


        /// <summary>  
        /// Tipo de dados (DataBase): char 
        /// Preenchimento obrigatório:  Não 
        /// Estilo: Normal  
        /// Tamanho Máximo: 1 
        /// </summary> 
        public string Situacao
        {
            get { return _Situacao.Trim(); }
            set { _Situacao = value.Trim(); }
        }

        private string _Modulo = string.Empty;


        /// <summary>  
        /// Tipo de dados (DataBase): char 
        /// Preenchimento obrigatório:  Não 
        /// Estilo: Normal  
        /// Tamanho Máximo: 1 
        /// </summary> 
        public string Modulo
        {
            get { return _Modulo.Trim(); }
            set { _Modulo = value.Trim(); }
        }

        private int _FkUa = 0;


        /// <summary>  
        /// Tipo de dados (DataBase): int 
        /// Preenchimento obrigatório:  Sim 
        /// Permitido:  Maior que zero 
        /// </summary> 
        public int FkUa
        {
            get { return _FkUa; }
            set { _FkUa = value; }
        }

        private string _Funcao = string.Empty;


        /// <summary>  
        /// Tipo de dados (DataBase): varchar 
        /// Preenchimento obrigatório:  Não 
        /// Estilo: Normal  
        /// Tamanho Máximo: 30 
        /// </summary> 
        public string Funcao
        {
            get { return _Funcao.Trim(); }
            set { _Funcao = value.Trim(); }
        }



        public UsuarioFields() {}

        public UsuarioFields(
                        string Param_Nome, 
                        string Param_UserName, 
                        string Param_Password, 
                        string Param_Cargo, 
                        string Param_Situacao, 
                        string Param_Modulo, 
                        int Param_FkUa, 
                        string Param_Funcao)
        {
               this._Nome = Param_Nome;
               this._UserName = Param_UserName;
               this._Password = Param_Password;
               this._Cargo = Param_Cargo;
               this._Situacao = Param_Situacao;
               this._Modulo = Param_Modulo;
               this._FkUa = Param_FkUa;
               this._Funcao = Param_Funcao;
        }
    }

}




//Projeto substituído ------------------------
//using System;
//using System.Collections;
//using System.Text;
//
//namespace SWGPgen
//{
//
//
//    /// <summary> 
//    /// Tabela: Usuario  
//    /// Autor: DAL Creator .net  
//    /// Data de criação: 19/03/2012 22:46:52 
//    /// Descrição: Classe que retorna todos os campos/propriedades da tabela Usuario 
//    /// </summary> 
//    public class UsuarioFields 
//    {
//
//        private int _idUsuario = 0;
//
//
//        /// <summary>  
//        /// Tipo de dados (DataBase): Int 
//        /// Somente Leitura/Auto Incremental
//        /// </summary> 
//        public int idUsuario
//        {
//            get { return _idUsuario; }
//            set { _idUsuario = value; }
//        }
//
//        private string _Nome = string.Empty;
//
//
//        /// <summary>  
//        /// Tipo de dados (DataBase): nvarchar 
//        /// Preenchimento obrigatório:  Não 
//        /// Estilo: Normal  
//        /// Tamanho Máximo: 200 
//        /// </summary> 
//        public string Nome
//        {
//            get { return _Nome.Trim(); }
//            set { _Nome = value.Trim(); }
//        }
//
//        private string _UserName = string.Empty;
//
//
//        /// <summary>  
//        /// Tipo de dados (DataBase): nvarchar 
//        /// Preenchimento obrigatório:  Não 
//        /// Estilo: Normal  
//        /// Tamanho Máximo: 100 
//        /// </summary> 
//        public string UserName
//        {
//            get { return _UserName.Trim(); }
//            set { _UserName = value.Trim(); }
//        }
//
//        private string _Password = string.Empty;
//
//
//        /// <summary>  
//        /// Tipo de dados (DataBase): nvarchar 
//        /// Preenchimento obrigatório:  Não 
//        /// Estilo: Normal  
//        /// Tamanho Máximo: 100 
//        /// </summary> 
//        public string Password
//        {
//            get { return _Password.Trim(); }
//            set { _Password = value.Trim(); }
//        }
//
//        private string _Cargo = string.Empty;
//
//
//        /// <summary>  
//        /// Tipo de dados (DataBase): nvarchar 
//        /// Preenchimento obrigatório:  Não 
//        /// Estilo: Normal  
//        /// Tamanho Máximo: 50 
//        /// </summary> 
//        public string Cargo
//        {
//            get { return _Cargo.Trim(); }
//            set { _Cargo = value.Trim(); }
//        }
//
//        private string _Situacao = string.Empty;
//
//
//        /// <summary>  
//        /// Tipo de dados (DataBase): char 
//        /// Preenchimento obrigatório:  Não 
//        /// Estilo: Normal  
//        /// Tamanho Máximo: 1 
//        /// </summary> 
//        public string Situacao
//        {
//            get { return _Situacao.Trim(); }
//            set { _Situacao = value.Trim(); }
//        }
//
//        private string _Modulo = string.Empty;
//
//
//        /// <summary>  
//        /// Tipo de dados (DataBase): char 
//        /// Preenchimento obrigatório:  Não 
//        /// Estilo: Normal  
//        /// Tamanho Máximo: 1 
//        /// </summary> 
//        public string Modulo
//        {
//            get { return _Modulo.Trim(); }
//            set { _Modulo = value.Trim(); }
//        }
//
//        private int _FkUa = 0;
//
//
//        /// <summary>  
//        /// Tipo de dados (DataBase): int 
//        /// Preenchimento obrigatório:  Sim 
//        /// Permitido:  Maior que zero 
//        /// </summary> 
//        public int FkUa
//        {
//            get { return _FkUa; }
//            set { _FkUa = value; }
//        }
//
//
//
//        public UsuarioFields() {}
//
//        public UsuarioFields(
//                        string Param_Nome, 
//                        string Param_UserName, 
//                        string Param_Password, 
//                        string Param_Cargo, 
//                        string Param_Situacao, 
//                        string Param_Modulo, 
//                        int Param_FkUa)
//        {
//               this._Nome = Param_Nome;
//               this._UserName = Param_UserName;
//               this._Password = Param_Password;
//               this._Cargo = Param_Cargo;
//               this._Situacao = Param_Situacao;
//               this._Modulo = Param_Modulo;
//               this._FkUa = Param_FkUa;
//        }
//    }
//
//}
//
//
//
//
////Projeto substituído ------------------------
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
////    /// Tabela: Usuario  
////    /// Autor: DAL Creator .net  
////    /// Data de criação: 13/03/2012 21:30:59 
////    /// Descrição: Classe que retorna todos os campos/propriedades da tabela Usuario 
////    /// </summary> 
////    public class UsuarioFields 
////    {
////
////        private int _idUsuario = 0;
////
////
////        /// <summary>  
////        /// Tipo de dados (DataBase): Int 
////        /// Somente Leitura/Auto Incremental
////        /// </summary> 
////        public int idUsuario
////        {
////            get { return _idUsuario; }
////            set { _idUsuario = value; }
////        }
////
////        private string _Nome = string.Empty;
////
////
////        /// <summary>  
////        /// Tipo de dados (DataBase): nvarchar 
////        /// Preenchimento obrigatório:  Não 
////        /// Estilo: Normal  
////        /// Tamanho Máximo: 200 
////        /// </summary> 
////        public string Nome
////        {
////            get { return _Nome.Trim(); }
////            set { _Nome = value.Trim(); }
////        }
////
////        private string _UserName = string.Empty;
////
////
////        /// <summary>  
////        /// Tipo de dados (DataBase): nvarchar 
////        /// Preenchimento obrigatório:  Não 
////        /// Estilo: Normal  
////        /// Tamanho Máximo: 100 
////        /// </summary> 
////        public string UserName
////        {
////            get { return _UserName.Trim(); }
////            set { _UserName = value.Trim(); }
////        }
////
////        private string _Password = string.Empty;
////
////
////        /// <summary>  
////        /// Tipo de dados (DataBase): nvarchar 
////        /// Preenchimento obrigatório:  Não 
////        /// Estilo: Normal  
////        /// Tamanho Máximo: 100 
////        /// </summary> 
////        public string Password
////        {
////            get { return _Password.Trim(); }
////            set { _Password = value.Trim(); }
////        }
////
////        private string _Cargo = string.Empty;
////
////
////        /// <summary>  
////        /// Tipo de dados (DataBase): nvarchar 
////        /// Preenchimento obrigatório:  Não 
////        /// Estilo: Normal  
////        /// Tamanho Máximo: 50 
////        /// </summary> 
////        public string Cargo
////        {
////            get { return _Cargo.Trim(); }
////            set { _Cargo = value.Trim(); }
////        }
////
////        private string _Situacao = string.Empty;
////
////
////        /// <summary>  
////        /// Tipo de dados (DataBase): char 
////        /// Preenchimento obrigatório:  Não 
////        /// Estilo: Normal  
////        /// Tamanho Máximo: 1 
////        /// </summary> 
////        public string Situacao
////        {
////            get { return _Situacao.Trim(); }
////            set { _Situacao = value.Trim(); }
////        }
////
////        private string _Modulo = string.Empty;
////
////
////        /// <summary>  
////        /// Tipo de dados (DataBase): char 
////        /// Preenchimento obrigatório:  Não 
////        /// Estilo: Normal  
////        /// Tamanho Máximo: 1 
////        /// </summary> 
////        public string Modulo
////        {
////            get { return _Modulo.Trim(); }
////            set { _Modulo = value.Trim(); }
////        }
////
////        private int _FkUa = 0;
////
////
////        /// <summary>  
////        /// Tipo de dados (DataBase): int 
////        /// Preenchimento obrigatório:  Sim 
////        /// Permitido:  Maior que zero 
////        /// </summary> 
////        public int FkUa
////        {
////            get { return _FkUa; }
////            set { _FkUa = value; }
////        }
////
////
////
////        public UsuarioFields() {}
////
////        public UsuarioFields(
////                        string Param_Nome, 
////                        string Param_UserName, 
////                        string Param_Password, 
////                        string Param_Cargo, 
////                        string Param_Situacao, 
////                        string Param_Modulo, 
////                        int Param_FkUa)
////        {
////               this._Nome = Param_Nome;
////               this._UserName = Param_UserName;
////               this._Password = Param_Password;
////               this._Cargo = Param_Cargo;
////               this._Situacao = Param_Situacao;
////               this._Modulo = Param_Modulo;
////               this._FkUa = Param_FkUa;
////        }
////    }
////
////}
////
////
////
////
//////Projeto substituído ------------------------
//////using System;
//////using System.Collections;
//////using System.Collections.Generic;
//////using System.Text;
//////
//////namespace SWGPgen
//////{
//////
//////
//////    /// <summary> 
//////    /// Tabela: Usuario  
//////    /// Autor: DAL Creator .net  
//////    /// Data de criação: 13/03/2012 21:19:06 
//////    /// Descrição: Classe que retorna todos os campos/propriedades da tabela Usuario 
//////    /// </summary> 
//////    public class UsuarioFields 
//////    {
//////
//////        private int _idUsuario = 0;
//////
//////
//////        /// <summary>  
//////        /// Tipo de dados (DataBase): Int 
//////        /// Somente Leitura/Auto Incremental
//////        /// </summary> 
//////        public int idUsuario
//////        {
//////            get { return _idUsuario; }
//////            set { _idUsuario = value; }
//////        }
//////
//////        private string _Nome = string.Empty;
//////
//////
//////        /// <summary>  
//////        /// Tipo de dados (DataBase): nvarchar 
//////        /// Preenchimento obrigatório:  Não 
//////        /// Estilo: Normal  
//////        /// Tamanho Máximo: 200 
//////        /// </summary> 
//////        public string Nome
//////        {
//////            get { return _Nome.Trim(); }
//////            set { _Nome = value.Trim(); }
//////        }
//////
//////        private string _UserName = string.Empty;
//////
//////
//////        /// <summary>  
//////        /// Tipo de dados (DataBase): nvarchar 
//////        /// Preenchimento obrigatório:  Não 
//////        /// Estilo: Normal  
//////        /// Tamanho Máximo: 100 
//////        /// </summary> 
//////        public string UserName
//////        {
//////            get { return _UserName.Trim(); }
//////            set { _UserName = value.Trim(); }
//////        }
//////
//////        private string _Password = string.Empty;
//////
//////
//////        /// <summary>  
//////        /// Tipo de dados (DataBase): nvarchar 
//////        /// Preenchimento obrigatório:  Não 
//////        /// Estilo: Normal  
//////        /// Tamanho Máximo: 100 
//////        /// </summary> 
//////        public string Password
//////        {
//////            get { return _Password.Trim(); }
//////            set { _Password = value.Trim(); }
//////        }
//////
//////        private string _Cargo = string.Empty;
//////
//////
//////        /// <summary>  
//////        /// Tipo de dados (DataBase): nvarchar 
//////        /// Preenchimento obrigatório:  Não 
//////        /// Estilo: Normal  
//////        /// Tamanho Máximo: 50 
//////        /// </summary> 
//////        public string Cargo
//////        {
//////            get { return _Cargo.Trim(); }
//////            set { _Cargo = value.Trim(); }
//////        }
//////
//////        private int _FkUa = 0;
//////
//////
//////        /// <summary>  
//////        /// Tipo de dados (DataBase): int 
//////        /// Preenchimento obrigatório:  Sim 
//////        /// Permitido:  Maior que zero 
//////        /// </summary> 
//////        public int FkUa
//////        {
//////            get { return _FkUa; }
//////            set { _FkUa = value; }
//////        }
//////
//////
//////
//////        public UsuarioFields() {}
//////
//////        public UsuarioFields(
//////                        string Param_Nome, 
//////                        string Param_UserName, 
//////                        string Param_Password, 
//////                        string Param_Cargo, 
//////                        int Param_FkUa)
//////        {
//////               this._Nome = Param_Nome;
//////               this._UserName = Param_UserName;
//////               this._Password = Param_Password;
//////               this._Cargo = Param_Cargo;
//////               this._FkUa = Param_FkUa;
//////        }
//////    }
//////
//////}
