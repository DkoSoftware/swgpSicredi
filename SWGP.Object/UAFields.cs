using System;
using System.Collections;
using System.Text;

namespace SWGPgen
{


    /// <summary> 
    /// Tabela: UA  
    /// Autor: DAL Creator .net  
    /// Data de criação: 19/03/2012 22:46:52 
    /// Descrição: Classe que retorna todos os campos/propriedades da tabela UA 
    /// </summary> 
    public class UAFields 
    {

        private int _idUA = 0;


        /// <summary>  
        /// Tipo de dados (DataBase): Int 
        /// Somente Leitura/Auto Incremental
        /// </summary> 
        public int idUA
        {
            get { return _idUA; }
            set { _idUA = value; }
        }

        private string _Nome = string.Empty;


        /// <summary>  
        /// Tipo de dados (DataBase): nvarchar 
        /// Preenchimento obrigatório:  Não 
        /// Estilo: Normal  
        /// Tamanho Máximo: 50 
        /// </summary> 
        public string Nome
        {
            get { return _Nome.Trim(); }
            set { _Nome = value.Trim(); }
        }



        public UAFields() {}

        public UAFields(
                        string Param_Nome)
        {
               this._Nome = Param_Nome;
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
//    /// Tabela: UA  
//    /// Autor: DAL Creator .net  
//    /// Data de criação: 13/03/2012 21:19:06 
//    /// Descrição: Classe que retorna todos os campos/propriedades da tabela UA 
//    /// </summary> 
//    public class UAFields 
//    {
//
//        private int _idUA = 0;
//
//
//        /// <summary>  
//        /// Tipo de dados (DataBase): Int 
//        /// Somente Leitura/Auto Incremental
//        /// </summary> 
//        public int idUA
//        {
//            get { return _idUA; }
//            set { _idUA = value; }
//        }
//
//        private string _Nome = string.Empty;
//
//
//        /// <summary>  
//        /// Tipo de dados (DataBase): nvarchar 
//        /// Preenchimento obrigatório:  Não 
//        /// Estilo: Normal  
//        /// Tamanho Máximo: 50 
//        /// </summary> 
//        public string Nome
//        {
//            get { return _Nome.Trim(); }
//            set { _Nome = value.Trim(); }
//        }
//
//
//
//        public UAFields() {}
//
//        public UAFields(
//                        string Param_Nome)
//        {
//               this._Nome = Param_Nome;
//        }
//    }
//
//}
