using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;

namespace SWGPgen
{


    /// <summary> 
    /// Tabela: Associacao  
    /// Autor: DAL Creator .net 
    /// Data de criação: 25/03/2012 03:26:53 
    /// Descrição: Classe responsável pela perssitência de dados. Utiliza a classe "AssociacaoFields". 
    /// </summary> 
    public class AssociacaoControl : IDisposable 
    {

        #region String de conexão 
        private string StrConnetionDB = ConfigurationManager.ConnectionStrings["StringConn"].ToString();
        #endregion


        #region Propriedade que armazena erros de execução 
        private string _ErrorMessage;
        public string ErrorMessage { get { return _ErrorMessage; } }
        #endregion


        #region Objetos de conexão 
        SqlConnection Conn;
        SqlCommand Cmd;
        SqlTransaction Tran;
        #endregion


        #region Funcões que retornam Conexões e Transações 

        public SqlTransaction GetNewTransaction(SqlConnection connIn)
        {
            if (connIn.State != ConnectionState.Open)
                connIn.Open();
            SqlTransaction TranOut = connIn.BeginTransaction();
            return TranOut;
        }

        public SqlConnection GetNewConnection()
        {
            return GetNewConnection(this.StrConnetionDB);
        }

        public SqlConnection GetNewConnection(string StringConnection)
        {
            SqlConnection connOut = new SqlConnection(StringConnection);
            return connOut;
        }

        #endregion


        #region enum SQLMode 
        /// <summary>   
        /// Representa o procedimento que está sendo executado na tabela.
        /// </summary>
        public enum SQLMode
        {                     
            /// <summary>
            /// Adiciona registro na tabela.
            /// </summary>
            Add,
            /// <summary>
            /// Atualiza registro na tabela.
            /// </summary>
            Update,
            /// <summary>
            /// Excluir registro na tabela
            /// </summary>
            Delete,
            /// <summary>
            /// Exclui TODOS os registros da tabela.
            /// </summary>
            DeleteAll,
            /// <summary>
            /// Seleciona um registro na tabela.
            /// </summary>
            Select,
            /// <summary>
            /// Seleciona TODOS os registros da tabela.
            /// </summary>
            SelectAll,
            /// <summary>
            /// Excluir ou seleciona um registro na tabela.
            /// </summary>
            SelectORDelete
        }
        #endregion 


        public AssociacaoControl() {}


        #region Inserindo dados na tabela 

        /// <summary> 
        /// Grava/Persiste um novo objeto AssociacaoFields no banco de dados
        /// </summary>
        /// <param name="FieldInfo">Objeto AssociacaoFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Add( ref AssociacaoFields FieldInfo )
        {
            try
            {
                this.Conn = new SqlConnection(this.StrConnetionDB);
                this.Conn.Open();
                this.Tran = this.Conn.BeginTransaction();
                this.Cmd = new SqlCommand("Proc_Associacao_Add", this.Conn, this.Tran);
                this.Cmd.CommandType = CommandType.StoredProcedure;
                this.Cmd.Parameters.Clear();
                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
                this.Tran.Commit();
                FieldInfo.idAssociacao = (int)this.Cmd.Parameters["@Param_idAssociacao"].Value;
                return true;

            }
            catch (SqlException e)
            {
                this.Tran.Rollback();
                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: {0}.",  e.Message);
                return false;
            }
            catch (Exception e)
            {
                this.Tran.Rollback();
                this._ErrorMessage = e.Message;
                return false;
            }
            finally
            {
                if (this.Conn != null)
                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
                if (this.Cmd != null)
                  this.Cmd.Dispose();
            }
        }

        #endregion


        #region Inserindo dados na tabela utilizando conexão e transação externa (compartilhada) 

        /// <summary> 
        /// Grava/Persiste um novo objeto AssociacaoFields no banco de dados
        /// </summary>
        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
        /// <param name="FieldInfo">Objeto AssociacaoFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Add( SqlConnection ConnIn, SqlTransaction TranIn, ref AssociacaoFields FieldInfo )
        {
            try
            {
                this.Cmd = new SqlCommand("Proc_Associacao_Add", ConnIn, TranIn);
                this.Cmd.CommandType = CommandType.StoredProcedure;
                this.Cmd.Parameters.Clear();
                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
                FieldInfo.idAssociacao = (int)this.Cmd.Parameters["@Param_idAssociacao"].Value;
                return true;

            }
            catch (SqlException e)
            {
                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: {0}.",  e.Message);
                return false;
            }
            catch (Exception e)
            {
                this._ErrorMessage = e.Message;
                return false;
            }
        }

        #endregion


        #region Editando dados na tabela 

        /// <summary> 
        /// Grava/Persiste as alterações em um objeto AssociacaoFields no banco de dados
        /// </summary>
        /// <param name="FieldInfo">Objeto AssociacaoFields a ser alterado.</param>
        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Update( AssociacaoFields FieldInfo )
        {
            try
            {
                this.Conn = new SqlConnection(this.StrConnetionDB);
                this.Conn.Open();
                this.Tran = this.Conn.BeginTransaction();
                this.Cmd = new SqlCommand("Proc_Associacao_Update", this.Conn, this.Tran);
                this.Cmd.CommandType = CommandType.StoredProcedure;
                this.Cmd.Parameters.Clear();
                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Update));
                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar atualizar registro!!");
                this.Tran.Commit();
                return true;
            }
            catch (SqlException e)
            {
                this.Tran.Rollback();
                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: {0}.",  e.Message);
                return false;
            }
            catch (Exception e)
            {
                this.Tran.Rollback();
                this._ErrorMessage = e.Message;
                return false;
            }
            finally
            {
                if (this.Conn != null)
                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
                if (this.Cmd != null)
                  this.Cmd.Dispose();
            }
        }

        #endregion


        #region Editando dados na tabela utilizando conexão e transação externa (compartilhada) 

        /// <summary> 
        /// Grava/Persiste as alterações em um objeto AssociacaoFields no banco de dados
        /// </summary>
        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
        /// <param name="FieldInfo">Objeto AssociacaoFields a ser alterado.</param>
        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Update( SqlConnection ConnIn, SqlTransaction TranIn, AssociacaoFields FieldInfo )
        {
            try
            {
                this.Cmd = new SqlCommand("Proc_Associacao_Update", ConnIn, TranIn);
                this.Cmd.CommandType = CommandType.StoredProcedure;
                this.Cmd.Parameters.Clear();
                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Update));
                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar atualizar registro!!");
                return true;
            }
            catch (SqlException e)
            {
                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: {0}.",  e.Message);
                return false;
            }
            catch (Exception e)
            {
                this._ErrorMessage = e.Message;
                return false;
            }
        }

        #endregion


        #region Excluindo todos os dados da tabela 

        /// <summary> 
        /// Exclui todos os registros da tabela
        /// </summary>
        /// <returns>"true" = registros excluidos com sucesso, "false" = erro ao tentar excluir registros (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool DeleteAll()
        {
            try
            {
                this.Conn = new SqlConnection(this.StrConnetionDB);
                this.Conn.Open();
                this.Tran = this.Conn.BeginTransaction();
                this.Cmd = new SqlCommand("Proc_Associacao_DeleteAll", this.Conn, this.Tran);
                this.Cmd.CommandType = CommandType.StoredProcedure;
                this.Cmd.Parameters.Clear();
                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
                this.Tran.Commit();
                return true;
            }
            catch (SqlException e)
            {
                this.Tran.Rollback();
                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: {0}.",  e.Message);
                return false;
            }
            catch (Exception e)
            {
                this.Tran.Rollback();
                this._ErrorMessage = e.Message;
                return false;
            }
            finally
            {
                if (this.Conn != null)
                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
                if (this.Cmd != null)
                  this.Cmd.Dispose();
            }
        }

        #endregion


        #region Excluindo todos os dados da tabela utilizando conexão e transação externa (compartilhada)

        /// <summary> 
        /// Exclui todos os registros da tabela
        /// </summary>
        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
        /// <returns>"true" = registros excluidos com sucesso, "false" = erro ao tentar excluir registros (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool DeleteAll(SqlConnection ConnIn, SqlTransaction TranIn)
        {
            try
            {
                this.Cmd = new SqlCommand("Proc_Associacao_DeleteAll", ConnIn, TranIn);
                this.Cmd.CommandType = CommandType.StoredProcedure;
                this.Cmd.Parameters.Clear();
                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
                return true;
            }
            catch (SqlException e)
            {
                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: {0}.",  e.Message);
                return false;
            }
            catch (Exception e)
            {
                this._ErrorMessage = e.Message;
                return false;
            }
        }

        #endregion


        #region Excluindo dados da tabela 

        /// <summary> 
        /// Exclui um registro da tabela no banco de dados
        /// </summary>
        /// <param name="FieldInfo">Objeto AssociacaoFields a ser excluído.</param>
        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Delete( AssociacaoFields FieldInfo )
        {
            return Delete(FieldInfo.idAssociacao);
        }

        /// <summary> 
        /// Exclui um registro da tabela no banco de dados
        /// </summary>
        /// <param name="Param_idAssociacao">int</param>
        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Delete(
                                     int Param_idAssociacao)
        {
            try
            {
                this.Conn = new SqlConnection(this.StrConnetionDB);
                this.Conn.Open();
                this.Tran = this.Conn.BeginTransaction();
                this.Cmd = new SqlCommand("Proc_Associacao_Delete", this.Conn, this.Tran);
                this.Cmd.CommandType = CommandType.StoredProcedure;
                this.Cmd.Parameters.Clear();
                this.Cmd.Parameters.Add(new SqlParameter("@Param_idAssociacao", SqlDbType.Int)).Value = Param_idAssociacao;
                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
                this.Tran.Commit();
                return true;
            }
            catch (SqlException e)
            {
                this.Tran.Rollback();
                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: {0}.",  e.Message);
                return false;
            }
            catch (Exception e)
            {
                this.Tran.Rollback();
                this._ErrorMessage = e.Message;
                return false;
            }
            finally
            {
                if (this.Conn != null)
                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
                if (this.Cmd != null)
                  this.Cmd.Dispose();
            }
        }

        #endregion


        #region Excluindo dados da tabela utilizando conexão e transação externa (compartilhada)

        /// <summary> 
        /// Exclui um registro da tabela no banco de dados
        /// </summary>
        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
        /// <param name="FieldInfo">Objeto AssociacaoFields a ser excluído.</param>
        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, AssociacaoFields FieldInfo )
        {
            return Delete(ConnIn, TranIn, FieldInfo.idAssociacao);
        }

        /// <summary> 
        /// Exclui um registro da tabela no banco de dados
        /// </summary>
        /// <param name="Param_idAssociacao">int</param>
        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, 
                                     int Param_idAssociacao)
        {
            try
            {
                this.Cmd = new SqlCommand("Proc_Associacao_Delete", ConnIn, TranIn);
                this.Cmd.CommandType = CommandType.StoredProcedure;
                this.Cmd.Parameters.Clear();
                this.Cmd.Parameters.Add(new SqlParameter("@Param_idAssociacao", SqlDbType.Int)).Value = Param_idAssociacao;
                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
                return true;
            }
            catch (SqlException e)
            {
                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: {0}.",  e.Message);
                return false;
            }
            catch (Exception e)
            {
                this._ErrorMessage = e.Message;
                return false;
            }
        }

        #endregion


        #region Selecionando um item da tabela 

        /// <summary> 
        /// Retorna um objeto AssociacaoFields através da chave primária passada como parâmetro
        /// </summary>
        /// <param name="Param_idAssociacao">int</param>
        /// <returns>Objeto AssociacaoFields</returns> 
        public AssociacaoFields GetItem(
                                     int Param_idAssociacao)
        {
            AssociacaoFields infoFields = new AssociacaoFields();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Associacao_Select", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_idAssociacao", SqlDbType.Int)).Value = Param_idAssociacao;
                        this.Cmd.Connection.Open();
                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                        {
                            if (!dr.HasRows) return null;
                            if (dr.Read())
                            {
                               infoFields = GetDataFromReader( dr );
                            }
                        }
                    }
                 }

                 return infoFields;

            }
            catch (SqlException e)
            {
                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
                return null;
            }
            catch (Exception e)
            {
                this._ErrorMessage = e.Message;
                return null;
            }
            finally
            {
                if (this.Conn != null)
                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
            }
        }

        #endregion


        #region Selecionando todos os dados da tabela 

        /// <summary> 
        /// Seleciona todos os registro da tabela e preenche um ArrayList com o objeto AssociacaoFields.
        /// </summary>
        /// <returns>List de objetos AssociacaoFields</returns> 
        public List<AssociacaoFields> GetAll()
        {
            List<AssociacaoFields> arrayInfo = new List<AssociacaoFields>();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Associacao_GetAll", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Connection.Open();
                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                        {
                           if (!dr.HasRows) return null;
                           while (dr.Read())
                           {
                              arrayInfo.Add(GetDataFromReader( dr ));
                           }
                        }
                    }
                }

                return arrayInfo;

            }
            catch (SqlException e)
            {
                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
                return null;
            }
            catch (Exception e)
            {
                this._ErrorMessage = e.Message;
                return null;
            }
            finally
            {
                if (this.Conn != null)
                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
            }
        }

        #endregion


        #region Contando os dados da tabela 

        /// <summary> 
        /// Retorna o total de registros contidos na tabela
        /// </summary>
        /// <returns>int</returns> 
        public int CountAll()
        {
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Associacao_CountAll", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Connection.Open();
                        object CountRegs = this.Cmd.ExecuteScalar();
                        if (CountRegs == null)
                        { return 0; }
                        else
                        { return (int)CountRegs; }
                    }
                }
            }
            catch (SqlException e)
            {
                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
                return 0;
            }
            catch (Exception e)
            {
                this._ErrorMessage = e.Message;
                return 0;
            }
            finally
            {
                if (this.Conn != null)
                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
            }
        }

        #endregion


        #region Selecionando dados da tabela através do campo "fkContato" 

        /// <summary> 
        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo fkContato.
        /// </summary>
        /// <param name="Param_fkContato">int</param>
        /// <returns>AssociacaoFields</returns> 
        public AssociacaoFields FindByfkContato(
                               int Param_fkContato )
        {
            AssociacaoFields infoFields = new AssociacaoFields();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Associacao_FindByfkContato", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_fkContato", SqlDbType.Int)).Value = Param_fkContato;
                        this.Cmd.Connection.Open();
                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                        {
                            if (!dr.HasRows) return null;
                            if (dr.Read())
                            {
                               infoFields = GetDataFromReader( dr );
                            }
                        }
                    }
                }

                return infoFields;

            }
            catch (SqlException e)
            {
                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
                return null;
            }
            catch (Exception e)
            {
                this._ErrorMessage = e.Message;
                return null;
            }
            finally
            {
                if (this.Conn != null)
                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
            }
        }

        #endregion



        #region Selecionando dados da tabela através do campo "NumeroConta" 

        /// <summary> 
        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo NumeroConta.
        /// </summary>
        /// <param name="Param_NumeroConta">string</param>
        /// <returns>AssociacaoFields</returns> 
        public AssociacaoFields FindByNumeroConta(
                               string Param_NumeroConta )
        {
            AssociacaoFields infoFields = new AssociacaoFields();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Associacao_FindByNumeroConta", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_NumeroConta", SqlDbType.VarChar, 7)).Value = Param_NumeroConta;
                        this.Cmd.Connection.Open();
                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                        {
                            if (!dr.HasRows) return null;
                            if (dr.Read())
                            {
                               infoFields = GetDataFromReader( dr );
                            }
                        }
                    }
                }

                return infoFields;

            }
            catch (SqlException e)
            {
                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
                return null;
            }
            catch (Exception e)
            {
                this._ErrorMessage = e.Message;
                return null;
            }
            finally
            {
                if (this.Conn != null)
                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
            }
        }

        #endregion



        #region Selecionando dados da tabela através do campo "DataAssociacao" 

        /// <summary> 
        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo DataAssociacao.
        /// </summary>
        /// <param name="Param_DataAssociacao">DateTime</param>
        /// <returns>AssociacaoFields</returns> 
        public AssociacaoFields FindByDataAssociacao(
                               DateTime Param_DataAssociacao )
        {
            AssociacaoFields infoFields = new AssociacaoFields();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Associacao_FindByDataAssociacao", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_DataAssociacao", SqlDbType.SmallDateTime)).Value = Param_DataAssociacao;
                        this.Cmd.Connection.Open();
                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                        {
                            if (!dr.HasRows) return null;
                            if (dr.Read())
                            {
                               infoFields = GetDataFromReader( dr );
                            }
                        }
                    }
                }

                return infoFields;

            }
            catch (SqlException e)
            {
                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
                return null;
            }
            catch (Exception e)
            {
                this._ErrorMessage = e.Message;
                return null;
            }
            finally
            {
                if (this.Conn != null)
                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
            }
        }

        #endregion



        #region Selecionando dados da tabela através do campo "DataCadastro" 

        /// <summary> 
        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo DataCadastro.
        /// </summary>
        /// <param name="Param_DataCadastro">DateTime</param>
        /// <returns>AssociacaoFields</returns> 
        public AssociacaoFields FindByDataCadastro(
                               DateTime Param_DataCadastro )
        {
            AssociacaoFields infoFields = new AssociacaoFields();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Associacao_FindByDataCadastro", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_DataCadastro", SqlDbType.SmallDateTime)).Value = Param_DataCadastro;
                        this.Cmd.Connection.Open();
                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                        {
                            if (!dr.HasRows) return null;
                            if (dr.Read())
                            {
                               infoFields = GetDataFromReader( dr );
                            }
                        }
                    }
                }

                return infoFields;

            }
            catch (SqlException e)
            {
                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
                return null;
            }
            catch (Exception e)
            {
                this._ErrorMessage = e.Message;
                return null;
            }
            finally
            {
                if (this.Conn != null)
                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
            }
        }

        #endregion



        #region Função GetDataFromReader

        /// <summary> 
        /// Retorna um objeto AssociacaoFields preenchido com os valores dos campos do SqlDataReader
        /// </summary>
        /// <param name="dr">SqlDataReader - Preenche o objeto AssociacaoFields </param>
        /// <returns>AssociacaoFields</returns>
        private AssociacaoFields GetDataFromReader( SqlDataReader dr )
        {
            AssociacaoFields infoFields = new AssociacaoFields();

            if (!dr.IsDBNull(0))
            { infoFields.idAssociacao = dr.GetInt32(0); }
            else
            { infoFields.idAssociacao = 0; }



            if (!dr.IsDBNull(1))
            { infoFields.fkContato = dr.GetInt32(1); }
            else
            { infoFields.fkContato = 0; }



            if (!dr.IsDBNull(2))
            { infoFields.NumeroConta = dr.GetString(2); }
            else
            { infoFields.NumeroConta = string.Empty; }



            if (!dr.IsDBNull(3))
            { infoFields.DataAssociacao = dr.GetDateTime(3); }
            else
            { infoFields.DataAssociacao = DateTime.MinValue; }



            if (!dr.IsDBNull(4))
            { infoFields.DataCadastro = dr.GetDateTime(4); }
            else
            { infoFields.DataCadastro = DateTime.MinValue; }


            return infoFields;
        }
        #endregion
















        #region Função GetAllParameters

        /// <summary> 
        /// Retorna um array de parâmetros com campos para atualização, seleção e inserção no banco de dados
        /// </summary>
        /// <param name="FieldInfo">Objeto AssociacaoFields</param>
        /// <param name="Modo">Tipo de oepração a ser executada no banco de dados</param>
        /// <returns>SqlParameter[] - Array de parâmetros</returns> 
        private SqlParameter[] GetAllParameters( AssociacaoFields FieldInfo, SQLMode Modo )
        {
            SqlParameter[] Parameters;

            switch (Modo)
            {
                case SQLMode.Add:
                    Parameters = new SqlParameter[5];
                    for (int I = 0; I < Parameters.Length; I++)
                       Parameters[I] = new SqlParameter();
                    //Field idAssociacao
                    Parameters[0].SqlDbType = SqlDbType.Int;
                    Parameters[0].Direction = ParameterDirection.Output;
                    Parameters[0].ParameterName = "@Param_idAssociacao";
                    Parameters[0].Value = DBNull.Value;

                    break;

                case SQLMode.Update:
                    Parameters = new SqlParameter[5];
                    for (int I = 0; I < Parameters.Length; I++)
                       Parameters[I] = new SqlParameter();
                    //Field idAssociacao
                    Parameters[0].SqlDbType = SqlDbType.Int;
                    Parameters[0].ParameterName = "@Param_idAssociacao";
                    Parameters[0].Value = FieldInfo.idAssociacao;

                    break;

                case SQLMode.SelectORDelete:
                    Parameters = new SqlParameter[1];
                    for (int I = 0; I < Parameters.Length; I++)
                       Parameters[I] = new SqlParameter();
                    //Field idAssociacao
                    Parameters[0].SqlDbType = SqlDbType.Int;
                    Parameters[0].ParameterName = "@Param_idAssociacao";
                    Parameters[0].Value = FieldInfo.idAssociacao;

                    return Parameters;

                default:
                    Parameters = new SqlParameter[5];
                    for (int I = 0; I < Parameters.Length; I++)
                       Parameters[I] = new SqlParameter();
                    break;
            }

            //Field fkContato
            Parameters[1].SqlDbType = SqlDbType.Int;
            Parameters[1].ParameterName = "@Param_fkContato";
            Parameters[1].Value = FieldInfo.fkContato;

            //Field NumeroConta
            Parameters[2].SqlDbType = SqlDbType.VarChar;
            Parameters[2].ParameterName = "@Param_NumeroConta";
            if (( FieldInfo.NumeroConta == null ) || ( FieldInfo.NumeroConta == string.Empty ))
            { Parameters[2].Value = DBNull.Value; }
            else
            { Parameters[2].Value = FieldInfo.NumeroConta; }
            Parameters[2].Size = 7;

            //Field DataAssociacao
            Parameters[3].SqlDbType = SqlDbType.SmallDateTime;
            Parameters[3].ParameterName = "@Param_DataAssociacao";
            if ( FieldInfo.DataAssociacao == DateTime.MinValue )
            { Parameters[3].Value = DBNull.Value; }
            else
            { Parameters[3].Value = FieldInfo.DataAssociacao; }

            //Field DataCadastro
            Parameters[4].SqlDbType = SqlDbType.SmallDateTime;
            Parameters[4].ParameterName = "@Param_DataCadastro";
            if ( FieldInfo.DataCadastro == DateTime.MinValue )
            { Parameters[4].Value = DBNull.Value; }
            else
            { Parameters[4].Value = FieldInfo.DataCadastro; }

            return Parameters;
        }
        #endregion





        #region IDisposable Members 

        bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~AssociacaoControl() 
        { 
            Dispose(false); 
        }

        private void Dispose(bool disposing) 
        {
            if (!this.disposed)
            {
                if (disposing) 
                {
                    if (this.Conn != null)
                        if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
                }
            }

        }
        #endregion 



    }

}


