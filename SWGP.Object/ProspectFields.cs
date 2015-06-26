using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SIMLgen
{


    /// <summary> 
    /// Tabela: Prospect  
    /// Autor: DAL Creator .net  
    /// Data de criação: 07/05/2012 21:02:06 
    /// Descrição: Classe que retorna todos os campos/propriedades da tabela Prospect 
    /// </summary> 
    public class ProspectFields 
    {

        private int _idProspect = 0;


        /// <summary>  
        /// Tipo de dados (DataBase): Int 
        /// Somente Leitura/Auto Incremental
        /// </summary> 
        public int idProspect
        {
            get { return _idProspect; }
            set { _idProspect = value; }
        }

        private string _Nome = string.Empty;


        /// <summary>  
        /// Tipo de dados (DataBase): nvarchar 
        /// Preenchimento obrigatório:  Sim 
        /// Estilo: Normal  
        /// Tamanho Máximo: 150 
        /// </summary> 
        public string Nome
        {
            get { return _Nome.Trim(); }
            set { _Nome = value.Trim(); }
        }

        private string _Endereco = string.Empty;


        /// <summary>  
        /// Tipo de dados (DataBase): nvarchar 
        /// Preenchimento obrigatório:  Não 
        /// Estilo: Normal  
        /// Tamanho Máximo: 250 
        /// </summary> 
        public string Endereco
        {
            get { return _Endereco.Trim(); }
            set { _Endereco = value.Trim(); }
        }

        private string _Telefone = string.Empty;


        /// <summary>  
        /// Tipo de dados (DataBase): nvarchar 
        /// Preenchimento obrigatório:  Não 
        /// Estilo: Normal  
        /// Tamanho Máximo: 11 
        /// </summary> 
        public string Telefone
        {
            get { return _Telefone.Trim(); }
            set { _Telefone = value.Trim(); }
        }

        private string _Tipo = string.Empty;


        /// <summary>  
        /// Tipo de dados (DataBase): char 
        /// Preenchimento obrigatório:  Não 
        /// Estilo: Normal  
        /// Tamanho Máximo: 2 
        /// </summary> 
        public string Tipo
        {
            get { return _Tipo.Trim(); }
            set { _Tipo = value.Trim(); }
        }

        private string _Segmento = string.Empty;


        /// <summary>  
        /// Tipo de dados (DataBase): nvarchar 
        /// Preenchimento obrigatório:  Não 
        /// Estilo: Normal  
        /// Tamanho Máximo: 30 
        /// </summary> 
        public string Segmento
        {
            get { return _Segmento.Trim(); }
            set { _Segmento = value.Trim(); }
        }

        private string _Observacao = string.Empty;


        /// <summary>  
        /// Tipo de dados (DataBase): nvarchar 
        /// Preenchimento obrigatório:  Não 
        /// Estilo: Normal  
        /// Tamanho Máximo: 300 
        /// </summary> 
        public string Observacao
        {
            get { return _Observacao.Trim(); }
            set { _Observacao = value.Trim(); }
        }

        private string _Email = string.Empty;


        /// <summary>  
        /// Tipo de dados (DataBase): nvarchar 
        /// Preenchimento obrigatório:  Não 
        /// Estilo: Normal  
        /// Tamanho Máximo: 50 
        /// </summary> 
        public string Email
        {
            get { return _Email.Trim(); }
            set { _Email = value.Trim(); }
        }

        private string _Bairro = string.Empty;


        /// <summary>  
        /// Tipo de dados (DataBase): nvarchar 
        /// Preenchimento obrigatório:  Não 
        /// Estilo: Normal  
        /// Tamanho Máximo: 100 
        /// </summary> 
        public string Bairro
        {
            get { return _Bairro.Trim(); }
            set { _Bairro = value.Trim(); }
        }

        private string _Cidade = string.Empty;


        /// <summary>  
        /// Tipo de dados (DataBase): nvarchar 
        /// Preenchimento obrigatório:  Não 
        /// Estilo: Normal  
        /// Tamanho Máximo: 100 
        /// </summary> 
        public string Cidade
        {
            get { return _Cidade.Trim(); }
            set { _Cidade = value.Trim(); }
        }

        private string _Estado = string.Empty;


        /// <summary>  
        /// Tipo de dados (DataBase): char 
        /// Preenchimento obrigatório:  Não 
        /// Estilo: Normal  
        /// Tamanho Máximo: 2 
        /// </summary> 
        public string Estado
        {
            get { return _Estado.Trim(); }
            set { _Estado = value.Trim(); }
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

        private string _PessoaContato = string.Empty;


        /// <summary>  
        /// Tipo de dados (DataBase): nvarchar 
        /// Preenchimento obrigatório:  Não 
        /// Estilo: Normal  
        /// Tamanho Máximo: 150 
        /// </summary> 
        public string PessoaContato
        {
            get { return _PessoaContato.Trim(); }
            set { _PessoaContato = value.Trim(); }
        }

        private string _CPF = string.Empty;


        /// <summary>  
        /// Tipo de dados (DataBase): nvarchar 
        /// Preenchimento obrigatório:  Não 
        /// Estilo: Normal  
        /// Tamanho Máximo: 50 
        /// </summary> 
        public string CPF
        {
            get { return _CPF.Trim(); }
            set { _CPF = value.Trim(); }
        }

        private string _CNPJ = string.Empty;


        /// <summary>  
        /// Tipo de dados (DataBase): nvarchar 
        /// Preenchimento obrigatório:  Não 
        /// Estilo: Normal  
        /// Tamanho Máximo: 50 
        /// </summary> 
        public string CNPJ
        {
            get { return _CNPJ.Trim(); }
            set { _CNPJ = value.Trim(); }
        }

        private int _FkUsuario = 0;


        /// <summary>  
        /// Tipo de dados (DataBase): int 
        /// Preenchimento obrigatório:  Não 
        /// Permitido:  Maior que zero 
        /// </summary> 
        public int FkUsuario
        {
            get { return _FkUsuario; }
            set { _FkUsuario = value; }
        }

        private string _SituacaoProspect = string.Empty;


        /// <summary>  
        /// Tipo de dados (DataBase): nvarchar 
        /// Preenchimento obrigatório:  Não 
        /// Estilo: Normal  
        /// Tamanho Máximo: 20 
        /// </summary> 
        public string SituacaoProspect
        {
            get { return _SituacaoProspect.Trim(); }
            set { _SituacaoProspect = value.Trim(); }
        }

        private int _fkIndicacao = 0;


        /// <summary>  
        /// Tipo de dados (DataBase): int 
        /// Preenchimento obrigatório:  Não 
        /// Permitido:  Maior que zero 
        /// </summary> 
        public int fkIndicacao
        {
            get { return _fkIndicacao; }
            set { _fkIndicacao = value; }
        }



        public ProspectFields() {}

        public ProspectFields(
                        string Param_Nome, 
                        string Param_Endereco, 
                        string Param_Telefone, 
                        string Param_Tipo, 
                        string Param_Segmento, 
                        string Param_Observacao, 
                        string Param_Email, 
                        string Param_Bairro, 
                        string Param_Cidade, 
                        string Param_Estado, 
                        DateTime Param_DataCadastro, 
                        string Param_PessoaContato, 
                        string Param_CPF, 
                        string Param_CNPJ, 
                        int Param_FkUsuario, 
                        string Param_SituacaoProspect, 
                        int Param_fkIndicacao)
        {
               this._Nome = Param_Nome;
               this._Endereco = Param_Endereco;
               this._Telefone = Param_Telefone;
               this._Tipo = Param_Tipo;
               this._Segmento = Param_Segmento;
               this._Observacao = Param_Observacao;
               this._Email = Param_Email;
               this._Bairro = Param_Bairro;
               this._Cidade = Param_Cidade;
               this._Estado = Param_Estado;
               this._DataCadastro = Param_DataCadastro;
               this._PessoaContato = Param_PessoaContato;
               this._CPF = Param_CPF;
               this._CNPJ = Param_CNPJ;
               this._FkUsuario = Param_FkUsuario;
               this._SituacaoProspect = Param_SituacaoProspect;
               this._fkIndicacao = Param_fkIndicacao;
        }
    }

}
