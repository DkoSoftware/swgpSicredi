using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SWGPgen
{


    /// <summary> 
    /// Tabela: Indicacao  
    /// Autor: DAL Creator .net  
    /// Data de cria��o: 02/04/2012 21:22:09 
    /// Descri��o: Classe que retorna todos os campos/propriedades da tabela Indicacao 
    /// </summary> 
    public class IndicacaoFields 
    {

        private int _idIndicacao = 0;


        /// <summary>  
        /// Tipo de dados (DataBase): Int 
        /// Somente Leitura/Auto Incremental
        /// </summary> 
        public int idIndicacao
        {
            get { return _idIndicacao; }
            set { _idIndicacao = value; }
        }

        private string _Nome = string.Empty;


        /// <summary>  
        /// Tipo de dados (DataBase): nvarchar 
        /// Preenchimento obrigat�rio:  Sim 
        /// Estilo: Normal  
        /// Tamanho M�ximo: 150 
        /// </summary> 
        public string Nome
        {
            get { return _Nome.Trim(); }
            set { _Nome = value.Trim(); }
        }

        private string _Telefone = string.Empty;


        /// <summary>  
        /// Tipo de dados (DataBase): nvarchar 
        /// Preenchimento obrigat�rio:  N�o 
        /// Estilo: Normal  
        /// Tamanho M�ximo: 50 
        /// </summary> 
        public string Telefone
        {
            get { return _Telefone.Trim(); }
            set { _Telefone = value.Trim(); }
        }

        private string _Endereco = string.Empty;


        /// <summary>  
        /// Tipo de dados (DataBase): nvarchar 
        /// Preenchimento obrigat�rio:  N�o 
        /// Estilo: Normal  
        /// Tamanho M�ximo: 150 
        /// </summary> 
        public string Endereco
        {
            get { return _Endereco.Trim(); }
            set { _Endereco = value.Trim(); }
        }

        private string _Bairro = string.Empty;


        /// <summary>  
        /// Tipo de dados (DataBase): nvarchar 
        /// Preenchimento obrigat�rio:  N�o 
        /// Estilo: Normal  
        /// Tamanho M�ximo: 150 
        /// </summary> 
        public string Bairro
        {
            get { return _Bairro.Trim(); }
            set { _Bairro = value.Trim(); }
        }

        private string _Cidade = string.Empty;


        /// <summary>  
        /// Tipo de dados (DataBase): nvarchar 
        /// Preenchimento obrigat�rio:  N�o 
        /// Estilo: Normal  
        /// Tamanho M�ximo: 150 
        /// </summary> 
        public string Cidade
        {
            get { return _Cidade.Trim(); }
            set { _Cidade = value.Trim(); }
        }

        private string _Estado = string.Empty;


        /// <summary>  
        /// Tipo de dados (DataBase): nvarchar 
        /// Preenchimento obrigat�rio:  N�o 
        /// Estilo: Normal  
        /// Tamanho M�ximo: 150 
        /// </summary> 
        public string Estado
        {
            get { return _Estado.Trim(); }
            set { _Estado = value.Trim(); }
        }

        private int _idUsuarioRecebe = 0;


        /// <summary>  
        /// Tipo de dados (DataBase): int 
        /// Preenchimento obrigat�rio:  Sim 
        /// Permitido:  Maior que zero 
        /// </summary> 
        public int idUsuarioRecebe
        {
            get { return _idUsuarioRecebe; }
            set { _idUsuarioRecebe = value; }
        }

        private int _idUsuarioIndica = 0;


        /// <summary>  
        /// Tipo de dados (DataBase): int 
        /// Preenchimento obrigat�rio:  Sim 
        /// Permitido:  Maior que zero 
        /// </summary> 
        public int idUsuarioIndica
        {
            get { return _idUsuarioIndica; }
            set { _idUsuarioIndica = value; }
        }



        public IndicacaoFields() {}

        public IndicacaoFields(
                        string Param_Nome, 
                        string Param_Telefone, 
                        string Param_Endereco, 
                        string Param_Bairro, 
                        string Param_Cidade, 
                        string Param_Estado, 
                        int Param_idUsuarioRecebe, 
                        int Param_idUsuarioIndica)
        {
               this._Nome = Param_Nome;
               this._Telefone = Param_Telefone;
               this._Endereco = Param_Endereco;
               this._Bairro = Param_Bairro;
               this._Cidade = Param_Cidade;
               this._Estado = Param_Estado;
               this._idUsuarioRecebe = Param_idUsuarioRecebe;
               this._idUsuarioIndica = Param_idUsuarioIndica;
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
//    /// Tabela: Indicacao  
//    /// Autor: DAL Creator .net  
//    /// Data de cria��o: 30/03/2012 01:12:51 
//    /// Descri��o: Classe que retorna todos os campos/propriedades da tabela Indicacao 
//    /// </summary> 
//    public class IndicacaoFields 
//    {
//
//        private int _idIndicacao = 0;
//
//
//        /// <summary>  
//        /// Tipo de dados (DataBase): Int 
//        /// Somente Leitura/Auto Incremental
//        /// </summary> 
//        public int idIndicacao
//        {
//            get { return _idIndicacao; }
//            set { _idIndicacao = value; }
//        }
//
//        private string _Nome = string.Empty;
//
//
//        /// <summary>  
//        /// Tipo de dados (DataBase): nvarchar 
//        /// Preenchimento obrigat�rio:  Sim 
//        /// Estilo: Normal  
//        /// Tamanho M�ximo: 150 
//        /// </summary> 
//        public string Nome
//        {
//            get { return _Nome.Trim(); }
//            set { _Nome = value.Trim(); }
//        }
//
//        private string _Telefone = string.Empty;
//
//
//        /// <summary>  
//        /// Tipo de dados (DataBase): nvarchar 
//        /// Preenchimento obrigat�rio:  N�o 
//        /// Estilo: Normal  
//        /// Tamanho M�ximo: 50 
//        /// </summary> 
//        public string Telefone
//        {
//            get { return _Telefone.Trim(); }
//            set { _Telefone = value.Trim(); }
//        }
//
//        private string _Endereco = string.Empty;
//
//
//        /// <summary>  
//        /// Tipo de dados (DataBase): nvarchar 
//        /// Preenchimento obrigat�rio:  N�o 
//        /// Estilo: Normal  
//        /// Tamanho M�ximo: 150 
//        /// </summary> 
//        public string Endereco
//        {
//            get { return _Endereco.Trim(); }
//            set { _Endereco = value.Trim(); }
//        }
//
//        private string _Bairro = string.Empty;
//
//
//        /// <summary>  
//        /// Tipo de dados (DataBase): nvarchar 
//        /// Preenchimento obrigat�rio:  N�o 
//        /// Estilo: Normal  
//        /// Tamanho M�ximo: 150 
//        /// </summary> 
//        public string Bairro
//        {
//            get { return _Bairro.Trim(); }
//            set { _Bairro = value.Trim(); }
//        }
//
//        private string _Cidade = string.Empty;
//
//
//        /// <summary>  
//        /// Tipo de dados (DataBase): nvarchar 
//        /// Preenchimento obrigat�rio:  N�o 
//        /// Estilo: Normal  
//        /// Tamanho M�ximo: 150 
//        /// </summary> 
//        public string Cidade
//        {
//            get { return _Cidade.Trim(); }
//            set { _Cidade = value.Trim(); }
//        }
//
//        private string _Estado = string.Empty;
//
//
//        /// <summary>  
//        /// Tipo de dados (DataBase): nvarchar 
//        /// Preenchimento obrigat�rio:  N�o 
//        /// Estilo: Normal  
//        /// Tamanho M�ximo: 150 
//        /// </summary> 
//        public string Estado
//        {
//            get { return _Estado.Trim(); }
//            set { _Estado = value.Trim(); }
//        }
//
//        private int _idUsuarioRecebe = 0;
//
//
//        /// <summary>  
//        /// Tipo de dados (DataBase): int 
//        /// Preenchimento obrigat�rio:  Sim 
//        /// Permitido:  Maior que zero 
//        /// </summary> 
//        public int idUsuarioRecebe
//        {
//            get { return _idUsuarioRecebe; }
//            set { _idUsuarioRecebe = value; }
//        }
//
//        private int _idUsuarioIndica = 0;
//
//
//        /// <summary>  
//        /// Tipo de dados (DataBase): int 
//        /// Preenchimento obrigat�rio:  Sim 
//        /// Permitido:  Maior que zero 
//        /// </summary> 
//        public int idUsuarioIndica
//        {
//            get { return _idUsuarioIndica; }
//            set { _idUsuarioIndica = value; }
//        }
//
//
//
//        public IndicacaoFields() {}
//
//        public IndicacaoFields(
//                        string Param_Nome, 
//                        string Param_Telefone, 
//                        string Param_Endereco, 
//                        string Param_Bairro, 
//                        string Param_Cidade, 
//                        string Param_Estado, 
//                        int Param_idUsuarioRecebe, 
//                        int Param_idUsuarioIndica)
//        {
//               this._Nome = Param_Nome;
//               this._Telefone = Param_Telefone;
//               this._Endereco = Param_Endereco;
//               this._Bairro = Param_Bairro;
//               this._Cidade = Param_Cidade;
//               this._Estado = Param_Estado;
//               this._idUsuarioRecebe = Param_idUsuarioRecebe;
//               this._idUsuarioIndica = Param_idUsuarioIndica;
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
////    /// Tabela: Indicacao  
////    /// Autor: DAL Creator .net  
////    /// Data de cria��o: 30/03/2012 00:35:27 
////    /// Descri��o: Classe que retorna todos os campos/propriedades da tabela Indicacao 
////    /// </summary> 
////    public class IndicacaoFields 
////    {
////
////        private int _idIndicacao = 0;
////
////
////        /// <summary>  
////        /// Tipo de dados (DataBase): Int 
////        /// Somente Leitura/Auto Incremental
////        /// </summary> 
////        public int idIndicacao
////        {
////            get { return _idIndicacao; }
////            set { _idIndicacao = value; }
////        }
////
////        private string _Nome = string.Empty;
////
////
////        /// <summary>  
////        /// Tipo de dados (DataBase): nvarchar 
////        /// Preenchimento obrigat�rio:  Sim 
////        /// Estilo: Normal  
////        /// Tamanho M�ximo: 150 
////        /// </summary> 
////        public string Nome
////        {
////            get { return _Nome.Trim(); }
////            set { _Nome = value.Trim(); }
////        }
////
////        private string _Telefone = string.Empty;
////
////
////        /// <summary>  
////        /// Tipo de dados (DataBase): nvarchar 
////        /// Preenchimento obrigat�rio:  N�o 
////        /// Estilo: Normal  
////        /// Tamanho M�ximo: 50 
////        /// </summary> 
////        public string Telefone
////        {
////            get { return _Telefone.Trim(); }
////            set { _Telefone = value.Trim(); }
////        }
////
////        private string _Endereco = string.Empty;
////
////
////        /// <summary>  
////        /// Tipo de dados (DataBase): nvarchar 
////        /// Preenchimento obrigat�rio:  N�o 
////        /// Estilo: Normal  
////        /// Tamanho M�ximo: 150 
////        /// </summary> 
////        public string Endereco
////        {
////            get { return _Endereco.Trim(); }
////            set { _Endereco = value.Trim(); }
////        }
////
////        private string _Bairro = string.Empty;
////
////
////        /// <summary>  
////        /// Tipo de dados (DataBase): nvarchar 
////        /// Preenchimento obrigat�rio:  N�o 
////        /// Estilo: Normal  
////        /// Tamanho M�ximo: 150 
////        /// </summary> 
////        public string Bairro
////        {
////            get { return _Bairro.Trim(); }
////            set { _Bairro = value.Trim(); }
////        }
////
////        private string _Cidade = string.Empty;
////
////
////        /// <summary>  
////        /// Tipo de dados (DataBase): nvarchar 
////        /// Preenchimento obrigat�rio:  N�o 
////        /// Estilo: Normal  
////        /// Tamanho M�ximo: 150 
////        /// </summary> 
////        public string Cidade
////        {
////            get { return _Cidade.Trim(); }
////            set { _Cidade = value.Trim(); }
////        }
////
////        private string _Estado = string.Empty;
////
////
////        /// <summary>  
////        /// Tipo de dados (DataBase): nvarchar 
////        /// Preenchimento obrigat�rio:  N�o 
////        /// Estilo: Normal  
////        /// Tamanho M�ximo: 150 
////        /// </summary> 
////        public string Estado
////        {
////            get { return _Estado.Trim(); }
////            set { _Estado = value.Trim(); }
////        }
////
////        private int _idUsuarioRecebe = 0;
////
////
////        /// <summary>  
////        /// Tipo de dados (DataBase): int 
////        /// Preenchimento obrigat�rio:  Sim 
////        /// Permitido:  Maior que zero 
////        /// </summary> 
////        public int idUsuarioRecebe
////        {
////            get { return _idUsuarioRecebe; }
////            set { _idUsuarioRecebe = value; }
////        }
////
////        private int _idUsuarioIndica = 0;
////
////
////        /// <summary>  
////        /// Tipo de dados (DataBase): int 
////        /// Preenchimento obrigat�rio:  Sim 
////        /// Permitido:  Maior que zero 
////        /// </summary> 
////        public int idUsuarioIndica
////        {
////            get { return _idUsuarioIndica; }
////            set { _idUsuarioIndica = value; }
////        }
////
////
////
////        public IndicacaoFields() {}
////
////        public IndicacaoFields(
////                        string Param_Nome, 
////                        string Param_Telefone, 
////                        string Param_Endereco, 
////                        string Param_Bairro, 
////                        string Param_Cidade, 
////                        string Param_Estado, 
////                        int Param_idUsuarioRecebe, 
////                        int Param_idUsuarioIndica)
////        {
////               this._Nome = Param_Nome;
////               this._Telefone = Param_Telefone;
////               this._Endereco = Param_Endereco;
////               this._Bairro = Param_Bairro;
////               this._Cidade = Param_Cidade;
////               this._Estado = Param_Estado;
////               this._idUsuarioRecebe = Param_idUsuarioRecebe;
////               this._idUsuarioIndica = Param_idUsuarioIndica;
////        }
////    }
////
////}
////
////
////
////
//////Projeto substitu�do ------------------------
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
//////    /// Tabela: Indicacao  
//////    /// Autor: DAL Creator .net  
//////    /// Data de cria��o: 27/03/2012 03:05:16 
//////    /// Descri��o: Classe que retorna todos os campos/propriedades da tabela Indicacao 
//////    /// </summary> 
//////    public class IndicacaoFields 
//////    {
//////
//////        private int _idIndicacao = 0;
//////
//////
//////        /// <summary>  
//////        /// Tipo de dados (DataBase): Int 
//////        /// Somente Leitura/Auto Incremental
//////        /// </summary> 
//////        public int idIndicacao
//////        {
//////            get { return _idIndicacao; }
//////            set { _idIndicacao = value; }
//////        }
//////
//////        private string _Nome = string.Empty;
//////
//////
//////        /// <summary>  
//////        /// Tipo de dados (DataBase): nvarchar 
//////        /// Preenchimento obrigat�rio:  Sim 
//////        /// Estilo: Normal  
//////        /// Tamanho M�ximo: 150 
//////        /// </summary> 
//////        public string Nome
//////        {
//////            get { return _Nome.Trim(); }
//////            set { _Nome = value.Trim(); }
//////        }
//////
//////        private string _Telefone = string.Empty;
//////
//////
//////        /// <summary>  
//////        /// Tipo de dados (DataBase): nvarchar 
//////        /// Preenchimento obrigat�rio:  N�o 
//////        /// Estilo: Normal  
//////        /// Tamanho M�ximo: 50 
//////        /// </summary> 
//////        public string Telefone
//////        {
//////            get { return _Telefone.Trim(); }
//////            set { _Telefone = value.Trim(); }
//////        }
//////
//////        private string _Endereco = string.Empty;
//////
//////
//////        /// <summary>  
//////        /// Tipo de dados (DataBase): nvarchar 
//////        /// Preenchimento obrigat�rio:  N�o 
//////        /// Estilo: Normal  
//////        /// Tamanho M�ximo: 150 
//////        /// </summary> 
//////        public string Endereco
//////        {
//////            get { return _Endereco.Trim(); }
//////            set { _Endereco = value.Trim(); }
//////        }
//////
//////        private string _Bairro = string.Empty;
//////
//////
//////        /// <summary>  
//////        /// Tipo de dados (DataBase): nvarchar 
//////        /// Preenchimento obrigat�rio:  N�o 
//////        /// Estilo: Normal  
//////        /// Tamanho M�ximo: 150 
//////        /// </summary> 
//////        public string Bairro
//////        {
//////            get { return _Bairro.Trim(); }
//////            set { _Bairro = value.Trim(); }
//////        }
//////
//////        private string _Cidade = string.Empty;
//////
//////
//////        /// <summary>  
//////        /// Tipo de dados (DataBase): nvarchar 
//////        /// Preenchimento obrigat�rio:  N�o 
//////        /// Estilo: Normal  
//////        /// Tamanho M�ximo: 150 
//////        /// </summary> 
//////        public string Cidade
//////        {
//////            get { return _Cidade.Trim(); }
//////            set { _Cidade = value.Trim(); }
//////        }
//////
//////        private string _Estado = string.Empty;
//////
//////
//////        /// <summary>  
//////        /// Tipo de dados (DataBase): nvarchar 
//////        /// Preenchimento obrigat�rio:  N�o 
//////        /// Estilo: Normal  
//////        /// Tamanho M�ximo: 150 
//////        /// </summary> 
//////        public string Estado
//////        {
//////            get { return _Estado.Trim(); }
//////            set { _Estado = value.Trim(); }
//////        }
//////
//////        private int _idUsuarioRecebe = 0;
//////
//////
//////        /// <summary>  
//////        /// Tipo de dados (DataBase): int 
//////        /// Preenchimento obrigat�rio:  Sim 
//////        /// Permitido:  Maior que zero 
//////        /// </summary> 
//////        public int idUsuarioRecebe
//////        {
//////            get { return _idUsuarioRecebe; }
//////            set { _idUsuarioRecebe = value; }
//////        }
//////
//////        private int _idUsuarioIndica = 0;
//////
//////
//////        /// <summary>  
//////        /// Tipo de dados (DataBase): int 
//////        /// Preenchimento obrigat�rio:  Sim 
//////        /// Permitido:  Maior que zero 
//////        /// </summary> 
//////        public int idUsuarioIndica
//////        {
//////            get { return _idUsuarioIndica; }
//////            set { _idUsuarioIndica = value; }
//////        }
//////
//////
//////
//////        public IndicacaoFields() {}
//////
//////        public IndicacaoFields(
//////                        string Param_Nome, 
//////                        string Param_Telefone, 
//////                        string Param_Endereco, 
//////                        string Param_Bairro, 
//////                        string Param_Cidade, 
//////                        string Param_Estado, 
//////                        int Param_idUsuarioRecebe, 
//////                        int Param_idUsuarioIndica)
//////        {
//////               this._Nome = Param_Nome;
//////               this._Telefone = Param_Telefone;
//////               this._Endereco = Param_Endereco;
//////               this._Bairro = Param_Bairro;
//////               this._Cidade = Param_Cidade;
//////               this._Estado = Param_Estado;
//////               this._idUsuarioRecebe = Param_idUsuarioRecebe;
//////               this._idUsuarioIndica = Param_idUsuarioIndica;
//////        }
//////    }
//////
//////}
//////
//////
//////
//////
////////Projeto substitu�do ------------------------
////////using System;
////////using System.Collections;
////////using System.Collections.Generic;
////////using System.Text;
////////
////////namespace SWGPgen
////////{
////////
////////
////////    /// <summary> 
////////    /// Tabela: Indicacao  
////////    /// Autor: DAL Creator .net  
////////    /// Data de cria��o: 27/03/2012 02:25:18 
////////    /// Descri��o: Classe que retorna todos os campos/propriedades da tabela Indicacao 
////////    /// </summary> 
////////    public class IndicacaoFields 
////////    {
////////
////////        private int _idIndicacao = 0;
////////
////////
////////        /// <summary>  
////////        /// Tipo de dados (DataBase): Int 
////////        /// Somente Leitura/Auto Incremental
////////        /// </summary> 
////////        public int idIndicacao
////////        {
////////            get { return _idIndicacao; }
////////            set { _idIndicacao = value; }
////////        }
////////
////////        private string _Nome = string.Empty;
////////
////////
////////        /// <summary>  
////////        /// Tipo de dados (DataBase): nvarchar 
////////        /// Preenchimento obrigat�rio:  Sim 
////////        /// Estilo: Normal  
////////        /// Tamanho M�ximo: 150 
////////        /// </summary> 
////////        public string Nome
////////        {
////////            get { return _Nome.Trim(); }
////////            set { _Nome = value.Trim(); }
////////        }
////////
////////        private string _Telefone = string.Empty;
////////
////////
////////        /// <summary>  
////////        /// Tipo de dados (DataBase): nvarchar 
////////        /// Preenchimento obrigat�rio:  N�o 
////////        /// Estilo: Normal  
////////        /// Tamanho M�ximo: 50 
////////        /// </summary> 
////////        public string Telefone
////////        {
////////            get { return _Telefone.Trim(); }
////////            set { _Telefone = value.Trim(); }
////////        }
////////
////////        private string _Endereco = string.Empty;
////////
////////
////////        /// <summary>  
////////        /// Tipo de dados (DataBase): nvarchar 
////////        /// Preenchimento obrigat�rio:  N�o 
////////        /// Estilo: Normal  
////////        /// Tamanho M�ximo: 150 
////////        /// </summary> 
////////        public string Endereco
////////        {
////////            get { return _Endereco.Trim(); }
////////            set { _Endereco = value.Trim(); }
////////        }
////////
////////        private string _Bairro = string.Empty;
////////
////////
////////        /// <summary>  
////////        /// Tipo de dados (DataBase): nvarchar 
////////        /// Preenchimento obrigat�rio:  N�o 
////////        /// Estilo: Normal  
////////        /// Tamanho M�ximo: 150 
////////        /// </summary> 
////////        public string Bairro
////////        {
////////            get { return _Bairro.Trim(); }
////////            set { _Bairro = value.Trim(); }
////////        }
////////
////////        private string _Cidade = string.Empty;
////////
////////
////////        /// <summary>  
////////        /// Tipo de dados (DataBase): nvarchar 
////////        /// Preenchimento obrigat�rio:  N�o 
////////        /// Estilo: Normal  
////////        /// Tamanho M�ximo: 150 
////////        /// </summary> 
////////        public string Cidade
////////        {
////////            get { return _Cidade.Trim(); }
////////            set { _Cidade = value.Trim(); }
////////        }
////////
////////        private string _Estado = string.Empty;
////////
////////
////////        /// <summary>  
////////        /// Tipo de dados (DataBase): nvarchar 
////////        /// Preenchimento obrigat�rio:  N�o 
////////        /// Estilo: Normal  
////////        /// Tamanho M�ximo: 150 
////////        /// </summary> 
////////        public string Estado
////////        {
////////            get { return _Estado.Trim(); }
////////            set { _Estado = value.Trim(); }
////////        }
////////
////////        private int _idUsuarioRecebe = 0;
////////
////////
////////        /// <summary>  
////////        /// Tipo de dados (DataBase): int 
////////        /// Preenchimento obrigat�rio:  Sim 
////////        /// Permitido:  Maior que zero 
////////        /// </summary> 
////////        public int idUsuarioRecebe
////////        {
////////            get { return _idUsuarioRecebe; }
////////            set { _idUsuarioRecebe = value; }
////////        }
////////
////////        private int _idUsuarioIndica = 0;
////////
////////
////////        /// <summary>  
////////        /// Tipo de dados (DataBase): int 
////////        /// Preenchimento obrigat�rio:  Sim 
////////        /// Permitido:  Maior que zero 
////////        /// </summary> 
////////        public int idUsuarioIndica
////////        {
////////            get { return _idUsuarioIndica; }
////////            set { _idUsuarioIndica = value; }
////////        }
////////
////////
////////
////////        public IndicacaoFields() {}
////////
////////        public IndicacaoFields(
////////                        string Param_Nome, 
////////                        string Param_Telefone, 
////////                        string Param_Endereco, 
////////                        string Param_Bairro, 
////////                        string Param_Cidade, 
////////                        string Param_Estado, 
////////                        int Param_idUsuarioRecebe, 
////////                        int Param_idUsuarioIndica)
////////        {
////////               this._Nome = Param_Nome;
////////               this._Telefone = Param_Telefone;
////////               this._Endereco = Param_Endereco;
////////               this._Bairro = Param_Bairro;
////////               this._Cidade = Param_Cidade;
////////               this._Estado = Param_Estado;
////////               this._idUsuarioRecebe = Param_idUsuarioRecebe;
////////               this._idUsuarioIndica = Param_idUsuarioIndica;
////////        }
////////    }
////////
////////}
////////
////////
////////
////////
//////////Projeto substitu�do ------------------------
//////////using System;
//////////using System.Collections;
//////////using System.Text;
//////////
//////////namespace SWGPgen
//////////{
//////////
//////////
//////////    /// <summary> 
//////////    /// Tabela: Indicacao  
//////////    /// Autor: DAL Creator .net  
//////////    /// Data de cria��o: 19/03/2012 22:46:51 
//////////    /// Descri��o: Classe que retorna todos os campos/propriedades da tabela Indicacao 
//////////    /// </summary> 
//////////    public class IndicacaoFields 
//////////    {
//////////
//////////        private int _idIndicacao = 0;
//////////
//////////
//////////        /// <summary>  
//////////        /// Tipo de dados (DataBase): Int 
//////////        /// Somente Leitura/Auto Incremental
//////////        /// </summary> 
//////////        public int idIndicacao
//////////        {
//////////            get { return _idIndicacao; }
//////////            set { _idIndicacao = value; }
//////////        }
//////////
//////////        private string _Nome = string.Empty;
//////////
//////////
//////////        /// <summary>  
//////////        /// Tipo de dados (DataBase): nvarchar 
//////////        /// Preenchimento obrigat�rio:  Sim 
//////////        /// Estilo: Normal  
//////////        /// Tamanho M�ximo: 150 
//////////        /// </summary> 
//////////        public string Nome
//////////        {
//////////            get { return _Nome.Trim(); }
//////////            set { _Nome = value.Trim(); }
//////////        }
//////////
//////////        private string _Endereco = string.Empty;
//////////
//////////
//////////        /// <summary>  
//////////        /// Tipo de dados (DataBase): nvarchar 
//////////        /// Preenchimento obrigat�rio:  N�o 
//////////        /// Estilo: Normal  
//////////        /// Tamanho M�ximo: 200 
//////////        /// </summary> 
//////////        public string Endereco
//////////        {
//////////            get { return _Endereco.Trim(); }
//////////            set { _Endereco = value.Trim(); }
//////////        }
//////////
//////////        private string _Bairro = string.Empty;
//////////
//////////
//////////        /// <summary>  
//////////        /// Tipo de dados (DataBase): nvarchar 
//////////        /// Preenchimento obrigat�rio:  N�o 
//////////        /// Estilo: Normal  
//////////        /// Tamanho M�ximo: 100 
//////////        /// </summary> 
//////////        public string Bairro
//////////        {
//////////            get { return _Bairro.Trim(); }
//////////            set { _Bairro = value.Trim(); }
//////////        }
//////////
//////////        private string _Cidade = string.Empty;
//////////
//////////
//////////        /// <summary>  
//////////        /// Tipo de dados (DataBase): nvarchar 
//////////        /// Preenchimento obrigat�rio:  N�o 
//////////        /// Estilo: Normal  
//////////        /// Tamanho M�ximo: 150 
//////////        /// </summary> 
//////////        public string Cidade
//////////        {
//////////            get { return _Cidade.Trim(); }
//////////            set { _Cidade = value.Trim(); }
//////////        }
//////////
//////////        private string _Estado = string.Empty;
//////////
//////////
//////////        /// <summary>  
//////////        /// Tipo de dados (DataBase): char 
//////////        /// Preenchimento obrigat�rio:  N�o 
//////////        /// Estilo: Normal  
//////////        /// Tamanho M�ximo: 2 
//////////        /// </summary> 
//////////        public string Estado
//////////        {
//////////            get { return _Estado.Trim(); }
//////////            set { _Estado = value.Trim(); }
//////////        }
//////////
//////////        private string _Telefone = string.Empty;
//////////
//////////
//////////        /// <summary>  
//////////        /// Tipo de dados (DataBase): nvarchar 
//////////        /// Preenchimento obrigat�rio:  N�o 
//////////        /// Estilo: Normal  
//////////        /// Tamanho M�ximo: 11 
//////////        /// </summary> 
//////////        public string Telefone
//////////        {
//////////            get { return _Telefone.Trim(); }
//////////            set { _Telefone = value.Trim(); }
//////////        }
//////////
//////////        private int _FkPosicaoIndicacao = 0;
//////////
//////////
//////////        /// <summary>  
//////////        /// Tipo de dados (DataBase): int 
//////////        /// Preenchimento obrigat�rio:  Sim 
//////////        /// Permitido:  Maior que zero 
//////////        /// </summary> 
//////////        public int FkPosicaoIndicacao
//////////        {
//////////            get { return _FkPosicaoIndicacao; }
//////////            set { _FkPosicaoIndicacao = value; }
//////////        }
//////////
//////////
//////////
//////////        public IndicacaoFields() {}
//////////
//////////        public IndicacaoFields(
//////////                        string Param_Nome, 
//////////                        string Param_Endereco, 
//////////                        string Param_Bairro, 
//////////                        string Param_Cidade, 
//////////                        string Param_Estado, 
//////////                        string Param_Telefone, 
//////////                        int Param_FkPosicaoIndicacao)
//////////        {
//////////               this._Nome = Param_Nome;
//////////               this._Endereco = Param_Endereco;
//////////               this._Bairro = Param_Bairro;
//////////               this._Cidade = Param_Cidade;
//////////               this._Estado = Param_Estado;
//////////               this._Telefone = Param_Telefone;
//////////               this._FkPosicaoIndicacao = Param_FkPosicaoIndicacao;
//////////        }
//////////    }
//////////
//////////}
//////////
//////////
//////////
//////////
////////////Projeto substitu�do ------------------------
////////////using System;
////////////using System.Collections;
////////////using System.Collections.Generic;
////////////using System.Text;
////////////
////////////namespace SWGPgen
////////////{
////////////
////////////
////////////    /// <summary> 
////////////    /// Tabela: Indicacao  
////////////    /// Autor: DAL Creator .net  
////////////    /// Data de cria��o: 13/03/2012 21:19:06 
////////////    /// Descri��o: Classe que retorna todos os campos/propriedades da tabela Indicacao 
////////////    /// </summary> 
////////////    public class IndicacaoFields 
////////////    {
////////////
////////////        private int _idIndicacao = 0;
////////////
////////////
////////////        /// <summary>  
////////////        /// Tipo de dados (DataBase): Int 
////////////        /// Somente Leitura/Auto Incremental
////////////        /// </summary> 
////////////        public int idIndicacao
////////////        {
////////////            get { return _idIndicacao; }
////////////            set { _idIndicacao = value; }
////////////        }
////////////
////////////        private string _Nome = string.Empty;
////////////
////////////
////////////        /// <summary>  
////////////        /// Tipo de dados (DataBase): nvarchar 
////////////        /// Preenchimento obrigat�rio:  Sim 
////////////        /// Estilo: Normal  
////////////        /// Tamanho M�ximo: 150 
////////////        /// </summary> 
////////////        public string Nome
////////////        {
////////////            get { return _Nome.Trim(); }
////////////            set { _Nome = value.Trim(); }
////////////        }
////////////
////////////        private string _Endereco = string.Empty;
////////////
////////////
////////////        /// <summary>  
////////////        /// Tipo de dados (DataBase): nvarchar 
////////////        /// Preenchimento obrigat�rio:  N�o 
////////////        /// Estilo: Normal  
////////////        /// Tamanho M�ximo: 200 
////////////        /// </summary> 
////////////        public string Endereco
////////////        {
////////////            get { return _Endereco.Trim(); }
////////////            set { _Endereco = value.Trim(); }
////////////        }
////////////
////////////        private string _Bairro = string.Empty;
////////////
////////////
////////////        /// <summary>  
////////////        /// Tipo de dados (DataBase): nvarchar 
////////////        /// Preenchimento obrigat�rio:  N�o 
////////////        /// Estilo: Normal  
////////////        /// Tamanho M�ximo: 100 
////////////        /// </summary> 
////////////        public string Bairro
////////////        {
////////////            get { return _Bairro.Trim(); }
////////////            set { _Bairro = value.Trim(); }
////////////        }
////////////
////////////        private string _Cidade = string.Empty;
////////////
////////////
////////////        /// <summary>  
////////////        /// Tipo de dados (DataBase): nvarchar 
////////////        /// Preenchimento obrigat�rio:  N�o 
////////////        /// Estilo: Normal  
////////////        /// Tamanho M�ximo: 150 
////////////        /// </summary> 
////////////        public string Cidade
////////////        {
////////////            get { return _Cidade.Trim(); }
////////////            set { _Cidade = value.Trim(); }
////////////        }
////////////
////////////        private string _Estado = string.Empty;
////////////
////////////
////////////        /// <summary>  
////////////        /// Tipo de dados (DataBase): char 
////////////        /// Preenchimento obrigat�rio:  N�o 
////////////        /// Estilo: Normal  
////////////        /// Tamanho M�ximo: 2 
////////////        /// </summary> 
////////////        public string Estado
////////////        {
////////////            get { return _Estado.Trim(); }
////////////            set { _Estado = value.Trim(); }
////////////        }
////////////
////////////        private string _Telefone = string.Empty;
////////////
////////////
////////////        /// <summary>  
////////////        /// Tipo de dados (DataBase): nvarchar 
////////////        /// Preenchimento obrigat�rio:  N�o 
////////////        /// Estilo: Normal  
////////////        /// Tamanho M�ximo: 11 
////////////        /// </summary> 
////////////        public string Telefone
////////////        {
////////////            get { return _Telefone.Trim(); }
////////////            set { _Telefone = value.Trim(); }
////////////        }
////////////
////////////        private int _FkPosicaoIndicacao = 0;
////////////
////////////
////////////        /// <summary>  
////////////        /// Tipo de dados (DataBase): int 
////////////        /// Preenchimento obrigat�rio:  Sim 
////////////        /// Permitido:  Maior que zero 
////////////        /// </summary> 
////////////        public int FkPosicaoIndicacao
////////////        {
////////////            get { return _FkPosicaoIndicacao; }
////////////            set { _FkPosicaoIndicacao = value; }
////////////        }
////////////
////////////
////////////
////////////        public IndicacaoFields() {}
////////////
////////////        public IndicacaoFields(
////////////                        string Param_Nome, 
////////////                        string Param_Endereco, 
////////////                        string Param_Bairro, 
////////////                        string Param_Cidade, 
////////////                        string Param_Estado, 
////////////                        string Param_Telefone, 
////////////                        int Param_FkPosicaoIndicacao)
////////////        {
////////////               this._Nome = Param_Nome;
////////////               this._Endereco = Param_Endereco;
////////////               this._Bairro = Param_Bairro;
////////////               this._Cidade = Param_Cidade;
////////////               this._Estado = Param_Estado;
////////////               this._Telefone = Param_Telefone;
////////////               this._FkPosicaoIndicacao = Param_FkPosicaoIndicacao;
////////////        }
////////////    }
////////////
////////////}
