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
    /// Tabela: PosicaoIndicacao  
    /// Autor: DAL Creator .net 
    /// Data de cria��o: 19/03/2012 23:02:06 
    /// Descri��o: Classe respons�vel pela perssit�ncia de dados. Utiliza a classe "PosicaoIndicacaoFields". 
    /// </summary> 
    public class PosicaoIndicacaoControl : IDisposable 
    {

        #region String de conex�o 
        private string StrConnetionDB = ConfigurationManager.ConnectionStrings["StringConn"].ToString();
        #endregion


        #region Propriedade que armazena erros de execu��o 
        private string _ErrorMessage;
        public string ErrorMessage { get { return _ErrorMessage; } }
        #endregion


        #region Objetos de conex�o 
        SqlConnection Conn;
        SqlCommand Cmd;
        SqlTransaction Tran;
        #endregion


        #region Func�es que retornam Conex�es e Transa��es 

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
        /// Representa o procedimento que est� sendo executado na tabela.
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


        public PosicaoIndicacaoControl() {}


        #region Inserindo dados na tabela 

        /// <summary> 
        /// Grava/Persiste um novo objeto PosicaoIndicacaoFields no banco de dados
        /// </summary>
        /// <param name="FieldInfo">Objeto PosicaoIndicacaoFields a ser gravado.Caso o par�metro solicite a express�o "ref", ser� adicionado um novo valor a algum campo auto incremento.</param>
        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Add( ref PosicaoIndicacaoFields FieldInfo )
        {
            try
            {
                this.Conn = new SqlConnection(this.StrConnetionDB);
                this.Conn.Open();
                this.Tran = this.Conn.BeginTransaction();
                this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_Add", this.Conn, this.Tran);
                this.Cmd.CommandType = CommandType.StoredProcedure;
                this.Cmd.Parameters.Clear();
                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
                this.Tran.Commit();
                FieldInfo.idPosicaoIndicacao = (int)this.Cmd.Parameters["@Param_idPosicaoIndicacao"].Value;
                return true;

            }
            catch (SqlException e)
            {
                this.Tran.Rollback();
                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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


        #region Inserindo dados na tabela utilizando conex�o e transa��o externa (compartilhada) 

        /// <summary> 
        /// Grava/Persiste um novo objeto PosicaoIndicacaoFields no banco de dados
        /// </summary>
        /// <param name="ConnIn">Objeto SqlConnection respons�vel pela conex�o com o banco de dados.</param>
        /// <param name="TranIn">Objeto SqlTransaction respons�vel pela transa��o iniciada no banco de dados.</param>
        /// <param name="FieldInfo">Objeto PosicaoIndicacaoFields a ser gravado.Caso o par�metro solicite a express�o "ref", ser� adicionado um novo valor a algum campo auto incremento.</param>
        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Add( SqlConnection ConnIn, SqlTransaction TranIn, ref PosicaoIndicacaoFields FieldInfo )
        {
            try
            {
                this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_Add", ConnIn, TranIn);
                this.Cmd.CommandType = CommandType.StoredProcedure;
                this.Cmd.Parameters.Clear();
                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
                FieldInfo.idPosicaoIndicacao = (int)this.Cmd.Parameters["@Param_idPosicaoIndicacao"].Value;
                return true;

            }
            catch (SqlException e)
            {
                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
        /// Grava/Persiste as altera��es em um objeto PosicaoIndicacaoFields no banco de dados
        /// </summary>
        /// <param name="FieldInfo">Objeto PosicaoIndicacaoFields a ser alterado.</param>
        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Update( PosicaoIndicacaoFields FieldInfo )
        {
            try
            {
                this.Conn = new SqlConnection(this.StrConnetionDB);
                this.Conn.Open();
                this.Tran = this.Conn.BeginTransaction();
                this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_Update", this.Conn, this.Tran);
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
                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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


        #region Editando dados na tabela utilizando conex�o e transa��o externa (compartilhada) 

        /// <summary> 
        /// Grava/Persiste as altera��es em um objeto PosicaoIndicacaoFields no banco de dados
        /// </summary>
        /// <param name="ConnIn">Objeto SqlConnection respons�vel pela conex�o com o banco de dados.</param>
        /// <param name="TranIn">Objeto SqlTransaction respons�vel pela transa��o iniciada no banco de dados.</param>
        /// <param name="FieldInfo">Objeto PosicaoIndicacaoFields a ser alterado.</param>
        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Update( SqlConnection ConnIn, SqlTransaction TranIn, PosicaoIndicacaoFields FieldInfo )
        {
            try
            {
                this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_Update", ConnIn, TranIn);
                this.Cmd.CommandType = CommandType.StoredProcedure;
                this.Cmd.Parameters.Clear();
                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Update));
                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar atualizar registro!!");
                return true;
            }
            catch (SqlException e)
            {
                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
                this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_DeleteAll", this.Conn, this.Tran);
                this.Cmd.CommandType = CommandType.StoredProcedure;
                this.Cmd.Parameters.Clear();
                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
                this.Tran.Commit();
                return true;
            }
            catch (SqlException e)
            {
                this.Tran.Rollback();
                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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


        #region Excluindo todos os dados da tabela utilizando conex�o e transa��o externa (compartilhada)

        /// <summary> 
        /// Exclui todos os registros da tabela
        /// </summary>
        /// <param name="ConnIn">Objeto SqlConnection respons�vel pela conex�o com o banco de dados.</param>
        /// <param name="TranIn">Objeto SqlTransaction respons�vel pela transa��o iniciada no banco de dados.</param>
        /// <returns>"true" = registros excluidos com sucesso, "false" = erro ao tentar excluir registros (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool DeleteAll(SqlConnection ConnIn, SqlTransaction TranIn)
        {
            try
            {
                this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_DeleteAll", ConnIn, TranIn);
                this.Cmd.CommandType = CommandType.StoredProcedure;
                this.Cmd.Parameters.Clear();
                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
                return true;
            }
            catch (SqlException e)
            {
                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
        /// <param name="FieldInfo">Objeto PosicaoIndicacaoFields a ser exclu�do.</param>
        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Delete( PosicaoIndicacaoFields FieldInfo )
        {
            return Delete(FieldInfo.idPosicaoIndicacao);
        }

        /// <summary> 
        /// Exclui um registro da tabela no banco de dados
        /// </summary>
        /// <param name="Param_idPosicaoIndicacao">int</param>
        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Delete(
                                     int Param_idPosicaoIndicacao)
        {
            try
            {
                this.Conn = new SqlConnection(this.StrConnetionDB);
                this.Conn.Open();
                this.Tran = this.Conn.BeginTransaction();
                this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_Delete", this.Conn, this.Tran);
                this.Cmd.CommandType = CommandType.StoredProcedure;
                this.Cmd.Parameters.Clear();
                this.Cmd.Parameters.Add(new SqlParameter("@Param_idPosicaoIndicacao", SqlDbType.Int)).Value = Param_idPosicaoIndicacao;
                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
                this.Tran.Commit();
                return true;
            }
            catch (SqlException e)
            {
                this.Tran.Rollback();
                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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


        #region Excluindo dados da tabela utilizando conex�o e transa��o externa (compartilhada)

        /// <summary> 
        /// Exclui um registro da tabela no banco de dados
        /// </summary>
        /// <param name="ConnIn">Objeto SqlConnection respons�vel pela conex�o com o banco de dados.</param>
        /// <param name="TranIn">Objeto SqlTransaction respons�vel pela transa��o iniciada no banco de dados.</param>
        /// <param name="FieldInfo">Objeto PosicaoIndicacaoFields a ser exclu�do.</param>
        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, PosicaoIndicacaoFields FieldInfo )
        {
            return Delete(ConnIn, TranIn, FieldInfo.idPosicaoIndicacao);
        }

        /// <summary> 
        /// Exclui um registro da tabela no banco de dados
        /// </summary>
        /// <param name="Param_idPosicaoIndicacao">int</param>
        /// <param name="ConnIn">Objeto SqlConnection respons�vel pela conex�o com o banco de dados.</param>
        /// <param name="TranIn">Objeto SqlTransaction respons�vel pela transa��o iniciada no banco de dados.</param>
        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, 
                                     int Param_idPosicaoIndicacao)
        {
            try
            {
                this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_Delete", ConnIn, TranIn);
                this.Cmd.CommandType = CommandType.StoredProcedure;
                this.Cmd.Parameters.Clear();
                this.Cmd.Parameters.Add(new SqlParameter("@Param_idPosicaoIndicacao", SqlDbType.Int)).Value = Param_idPosicaoIndicacao;
                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
                return true;
            }
            catch (SqlException e)
            {
                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
        /// Retorna um objeto PosicaoIndicacaoFields atrav�s da chave prim�ria passada como par�metro
        /// </summary>
        /// <param name="Param_idPosicaoIndicacao">int</param>
        /// <returns>Objeto PosicaoIndicacaoFields</returns> 
        public PosicaoIndicacaoFields GetItem(
                                     int Param_idPosicaoIndicacao)
        {
            PosicaoIndicacaoFields infoFields = new PosicaoIndicacaoFields();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_Select", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_idPosicaoIndicacao", SqlDbType.Int)).Value = Param_idPosicaoIndicacao;
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
                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
        /// Seleciona todos os registro da tabela e preenche um ArrayList com o objeto PosicaoIndicacaoFields.
        /// </summary>
        /// <returns>List de objetos PosicaoIndicacaoFields</returns> 
        public List<PosicaoIndicacaoFields> GetAll()
        {
            List<PosicaoIndicacaoFields> arrayInfo = new List<PosicaoIndicacaoFields>();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_GetAll", this.Conn))
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
                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
                    using (this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_CountAll", this.Conn))
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
                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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


        #region Selecionando dados da tabela atrav�s do campo "NomeUsuarioRecebe" 

        /// <summary> 
        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo NomeUsuarioRecebe.
        /// </summary>
        /// <param name="Param_NomeUsuarioRecebe">string</param>
        /// <returns>PosicaoIndicacaoFields</returns> 
        public PosicaoIndicacaoFields FindByNomeUsuarioRecebe(
                               string Param_NomeUsuarioRecebe )
        {
            PosicaoIndicacaoFields infoFields = new PosicaoIndicacaoFields();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_FindByNomeUsuarioRecebe", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_NomeUsuarioRecebe", SqlDbType.VarChar, 50)).Value = Param_NomeUsuarioRecebe;
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
                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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



        #region Selecionando dados da tabela atrav�s do campo "NomeUsuarioIndica" 

        /// <summary> 
        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo NomeUsuarioIndica.
        /// </summary>
        /// <param name="Param_NomeUsuarioIndica">string</param>
        /// <returns>PosicaoIndicacaoFields</returns> 
        public PosicaoIndicacaoFields FindByNomeUsuarioIndica(
                               string Param_NomeUsuarioIndica )
        {
            PosicaoIndicacaoFields infoFields = new PosicaoIndicacaoFields();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_FindByNomeUsuarioIndica", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_NomeUsuarioIndica", SqlDbType.VarChar, 50)).Value = Param_NomeUsuarioIndica;
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
                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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



        #region Fun��o GetDataFromReader

        /// <summary> 
        /// Retorna um objeto PosicaoIndicacaoFields preenchido com os valores dos campos do SqlDataReader
        /// </summary>
        /// <param name="dr">SqlDataReader - Preenche o objeto PosicaoIndicacaoFields </param>
        /// <returns>PosicaoIndicacaoFields</returns>
        private PosicaoIndicacaoFields GetDataFromReader( SqlDataReader dr )
        {
            PosicaoIndicacaoFields infoFields = new PosicaoIndicacaoFields();

            if (!dr.IsDBNull(0))
            { infoFields.idPosicaoIndicacao = dr.GetInt32(0); }
            else
            { infoFields.idPosicaoIndicacao = 0; }



            if (!dr.IsDBNull(1))
            { infoFields.NomeUsuarioRecebe = dr.GetString(1); }
            else
            { infoFields.NomeUsuarioRecebe = string.Empty; }



            if (!dr.IsDBNull(2))
            { infoFields.NomeUsuarioIndica = dr.GetString(2); }
            else
            { infoFields.NomeUsuarioIndica = string.Empty; }


            return infoFields;
        }
        #endregion












        #region Fun��o GetAllParameters

        /// <summary> 
        /// Retorna um array de par�metros com campos para atualiza��o, sele��o e inser��o no banco de dados
        /// </summary>
        /// <param name="FieldInfo">Objeto PosicaoIndicacaoFields</param>
        /// <param name="Modo">Tipo de oepra��o a ser executada no banco de dados</param>
        /// <returns>SqlParameter[] - Array de par�metros</returns> 
        private SqlParameter[] GetAllParameters( PosicaoIndicacaoFields FieldInfo, SQLMode Modo )
        {
            SqlParameter[] Parameters;

            switch (Modo)
            {
                case SQLMode.Add:
                    Parameters = new SqlParameter[3];
                    for (int I = 0; I < Parameters.Length; I++)
                       Parameters[I] = new SqlParameter();
                    //Field idPosicaoIndicacao
                    Parameters[0].SqlDbType = SqlDbType.Int;
                    Parameters[0].Direction = ParameterDirection.Output;
                    Parameters[0].ParameterName = "@Param_idPosicaoIndicacao";
                    Parameters[0].Value = DBNull.Value;

                    break;

                case SQLMode.Update:
                    Parameters = new SqlParameter[3];
                    for (int I = 0; I < Parameters.Length; I++)
                       Parameters[I] = new SqlParameter();
                    //Field idPosicaoIndicacao
                    Parameters[0].SqlDbType = SqlDbType.Int;
                    Parameters[0].ParameterName = "@Param_idPosicaoIndicacao";
                    Parameters[0].Value = FieldInfo.idPosicaoIndicacao;

                    break;

                case SQLMode.SelectORDelete:
                    Parameters = new SqlParameter[1];
                    for (int I = 0; I < Parameters.Length; I++)
                       Parameters[I] = new SqlParameter();
                    //Field idPosicaoIndicacao
                    Parameters[0].SqlDbType = SqlDbType.Int;
                    Parameters[0].ParameterName = "@Param_idPosicaoIndicacao";
                    Parameters[0].Value = FieldInfo.idPosicaoIndicacao;

                    return Parameters;

                default:
                    Parameters = new SqlParameter[3];
                    for (int I = 0; I < Parameters.Length; I++)
                       Parameters[I] = new SqlParameter();
                    break;
            }

            //Field NomeUsuarioRecebe
            Parameters[1].SqlDbType = SqlDbType.VarChar;
            Parameters[1].ParameterName = "@Param_NomeUsuarioRecebe";
            if (( FieldInfo.NomeUsuarioRecebe == null ) || ( FieldInfo.NomeUsuarioRecebe == string.Empty ))
            { Parameters[1].Value = DBNull.Value; }
            else
            { Parameters[1].Value = FieldInfo.NomeUsuarioRecebe; }
            Parameters[1].Size = 50;

            //Field NomeUsuarioIndica
            Parameters[2].SqlDbType = SqlDbType.VarChar;
            Parameters[2].ParameterName = "@Param_NomeUsuarioIndica";
            if (( FieldInfo.NomeUsuarioIndica == null ) || ( FieldInfo.NomeUsuarioIndica == string.Empty ))
            { Parameters[2].Value = DBNull.Value; }
            else
            { Parameters[2].Value = FieldInfo.NomeUsuarioIndica; }
            Parameters[2].Size = 50;

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

        ~PosicaoIndicacaoControl() 
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





//Projeto substitu�do ------------------------
//using System;
//using System.Collections;
//using System.Data;
//using System.Data.SqlClient;
//using System.Configuration;
//using System.Text;
//
//namespace SWGPgen
//{
//
//
//    /// <summary> 
//    /// Tabela: PosicaoIndicacao  
//    /// Autor: DAL Creator .net 
//    /// Data de cria��o: 19/03/2012 22:46:51 
//    /// Descri��o: Classe respons�vel pela perssit�ncia de dados. Utiliza a classe "PosicaoIndicacaoFields". 
//    /// </summary> 
//    public class PosicaoIndicacaoControl : IDisposable 
//    {
//
//        #region String de conex�o 
//        private string StrConnetionDB = ConfigurationManager.ConnectionStrings["Data Source=DEKO-PC;Initial Catalog=swgp;User Id=sureg;Password=@sureg2012;"].ToString();
//        #endregion
//
//
//        #region Propriedade que armazena erros de execu��o 
//        private string _ErrorMessage;
//        public string ErrorMessage { get { return _ErrorMessage; } }
//        #endregion
//
//
//        #region Objetos de conex�o 
//        SqlConnection Conn;
//        SqlCommand Cmd;
//        SqlTransaction Tran;
//        #endregion
//
//
//        #region Func�es que retornam Conex�es e Transa��es 
//
//        public SqlTransaction GetNewTransaction(SqlConnection connIn)
//        {
//            if (connIn.State != ConnectionState.Open)
//                connIn.Open();
//            SqlTransaction TranOut = connIn.BeginTransaction();
//            return TranOut;
//        }
//
//        public SqlConnection GetNewConnection()
//        {
//            return GetNewConnection(this.StrConnetionDB);
//        }
//
//        public SqlConnection GetNewConnection(string StringConnection)
//        {
//            SqlConnection connOut = new SqlConnection(StringConnection);
//            return connOut;
//        }
//
//        #endregion
//
//
//        #region enum SQLMode 
//        /// <summary>   
//        /// Representa o procedimento que est� sendo executado na tabela.
//        /// </summary>
//        public enum SQLMode
//        {                     
//            /// <summary>
//            /// Adiciona registro na tabela.
//            /// </summary>
//            Add,
//            /// <summary>
//            /// Atualiza registro na tabela.
//            /// </summary>
//            Update,
//            /// <summary>
//            /// Excluir registro na tabela
//            /// </summary>
//            Delete,
//            /// <summary>
//            /// Exclui TODOS os registros da tabela.
//            /// </summary>
//            DeleteAll,
//            /// <summary>
//            /// Seleciona um registro na tabela.
//            /// </summary>
//            Select,
//            /// <summary>
//            /// Seleciona TODOS os registros da tabela.
//            /// </summary>
//            SelectAll,
//            /// <summary>
//            /// Excluir ou seleciona um registro na tabela.
//            /// </summary>
//            SelectORDelete
//        }
//        #endregion 
//
//
//        public PosicaoIndicacaoControl() {}
//
//
//        #region Inserindo dados na tabela 
//
//        /// <summary> 
//        /// Grava/Persiste um novo objeto PosicaoIndicacaoFields no banco de dados
//        /// </summary>
//        /// <param name="FieldInfo">Objeto PosicaoIndicacaoFields a ser gravado.Caso o par�metro solicite a express�o "ref", ser� adicionado um novo valor a algum campo auto incremento.</param>
//        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//        public bool Add( ref PosicaoIndicacaoFields FieldInfo )
//        {
//            try
//            {
//                this.Conn = new SqlConnection(this.StrConnetionDB);
//                this.Conn.Open();
//                this.Tran = this.Conn.BeginTransaction();
//                this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_Add", this.Conn, this.Tran);
//                this.Cmd.CommandType = CommandType.StoredProcedure;
//                this.Cmd.Parameters.Clear();
//                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
//                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
//                this.Tran.Commit();
//                FieldInfo.idPosicaoIndicacao = (int)this.Cmd.Parameters["@Param_idPosicaoIndicacao"].Value;
//                return true;
//
//            }
//            catch (SqlException e)
//            {
//                this.Tran.Rollback();
//                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: {0}.",  e.Message);
//                return false;
//            }
//            catch (Exception e)
//            {
//                this.Tran.Rollback();
//                this._ErrorMessage = e.Message;
//                return false;
//            }
//            finally
//            {
//                if (this.Conn != null)
//                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//                if (this.Cmd != null)
//                  this.Cmd.Dispose();
//            }
//        }
//
//        #endregion
//
//
//        #region Inserindo dados na tabela utilizando conex�o e transa��o externa (compartilhada) 
//
//        /// <summary> 
//        /// Grava/Persiste um novo objeto PosicaoIndicacaoFields no banco de dados
//        /// </summary>
//        /// <param name="ConnIn">Objeto SqlConnection respons�vel pela conex�o com o banco de dados.</param>
//        /// <param name="TranIn">Objeto SqlTransaction respons�vel pela transa��o iniciada no banco de dados.</param>
//        /// <param name="FieldInfo">Objeto PosicaoIndicacaoFields a ser gravado.Caso o par�metro solicite a express�o "ref", ser� adicionado um novo valor a algum campo auto incremento.</param>
//        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//        public bool Add( SqlConnection ConnIn, SqlTransaction TranIn, ref PosicaoIndicacaoFields FieldInfo )
//        {
//            try
//            {
//                this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_Add", ConnIn, TranIn);
//                this.Cmd.CommandType = CommandType.StoredProcedure;
//                this.Cmd.Parameters.Clear();
//                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
//                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
//                FieldInfo.idPosicaoIndicacao = (int)this.Cmd.Parameters["@Param_idPosicaoIndicacao"].Value;
//                return true;
//
//            }
//            catch (SqlException e)
//            {
//                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: {0}.",  e.Message);
//                return false;
//            }
//            catch (Exception e)
//            {
//                this._ErrorMessage = e.Message;
//                return false;
//            }
//        }
//
//        #endregion
//
//
//        #region Editando dados na tabela 
//
//        /// <summary> 
//        /// Grava/Persiste as altera��es em um objeto PosicaoIndicacaoFields no banco de dados
//        /// </summary>
//        /// <param name="FieldInfo">Objeto PosicaoIndicacaoFields a ser alterado.</param>
//        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//        public bool Update( PosicaoIndicacaoFields FieldInfo )
//        {
//            try
//            {
//                this.Conn = new SqlConnection(this.StrConnetionDB);
//                this.Conn.Open();
//                this.Tran = this.Conn.BeginTransaction();
//                this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_Update", this.Conn, this.Tran);
//                this.Cmd.CommandType = CommandType.StoredProcedure;
//                this.Cmd.Parameters.Clear();
//                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Update));
//                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar atualizar registro!!");
//                this.Tran.Commit();
//                return true;
//            }
//            catch (SqlException e)
//            {
//                this.Tran.Rollback();
//                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: {0}.",  e.Message);
//                return false;
//            }
//            catch (Exception e)
//            {
//                this.Tran.Rollback();
//                this._ErrorMessage = e.Message;
//                return false;
//            }
//            finally
//            {
//                if (this.Conn != null)
//                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//                if (this.Cmd != null)
//                  this.Cmd.Dispose();
//            }
//        }
//
//        #endregion
//
//
//        #region Editando dados na tabela utilizando conex�o e transa��o externa (compartilhada) 
//
//        /// <summary> 
//        /// Grava/Persiste as altera��es em um objeto PosicaoIndicacaoFields no banco de dados
//        /// </summary>
//        /// <param name="ConnIn">Objeto SqlConnection respons�vel pela conex�o com o banco de dados.</param>
//        /// <param name="TranIn">Objeto SqlTransaction respons�vel pela transa��o iniciada no banco de dados.</param>
//        /// <param name="FieldInfo">Objeto PosicaoIndicacaoFields a ser alterado.</param>
//        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//        public bool Update( SqlConnection ConnIn, SqlTransaction TranIn, PosicaoIndicacaoFields FieldInfo )
//        {
//            try
//            {
//                this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_Update", ConnIn, TranIn);
//                this.Cmd.CommandType = CommandType.StoredProcedure;
//                this.Cmd.Parameters.Clear();
//                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Update));
//                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar atualizar registro!!");
//                return true;
//            }
//            catch (SqlException e)
//            {
//                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: {0}.",  e.Message);
//                return false;
//            }
//            catch (Exception e)
//            {
//                this._ErrorMessage = e.Message;
//                return false;
//            }
//        }
//
//        #endregion
//
//
//        #region Excluindo todos os dados da tabela 
//
//        /// <summary> 
//        /// Exclui todos os registros da tabela
//        /// </summary>
//        /// <returns>"true" = registros excluidos com sucesso, "false" = erro ao tentar excluir registros (consulte a propriedade ErrorMessage para detalhes)</returns> 
//        public bool DeleteAll()
//        {
//            try
//            {
//                this.Conn = new SqlConnection(this.StrConnetionDB);
//                this.Conn.Open();
//                this.Tran = this.Conn.BeginTransaction();
//                this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_DeleteAll", this.Conn, this.Tran);
//                this.Cmd.CommandType = CommandType.StoredProcedure;
//                this.Cmd.Parameters.Clear();
//                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
//                this.Tran.Commit();
//                return true;
//            }
//            catch (SqlException e)
//            {
//                this.Tran.Rollback();
//                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: {0}.",  e.Message);
//                return false;
//            }
//            catch (Exception e)
//            {
//                this.Tran.Rollback();
//                this._ErrorMessage = e.Message;
//                return false;
//            }
//            finally
//            {
//                if (this.Conn != null)
//                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//                if (this.Cmd != null)
//                  this.Cmd.Dispose();
//            }
//        }
//
//        #endregion
//
//
//        #region Excluindo todos os dados da tabela utilizando conex�o e transa��o externa (compartilhada)
//
//        /// <summary> 
//        /// Exclui todos os registros da tabela
//        /// </summary>
//        /// <param name="ConnIn">Objeto SqlConnection respons�vel pela conex�o com o banco de dados.</param>
//        /// <param name="TranIn">Objeto SqlTransaction respons�vel pela transa��o iniciada no banco de dados.</param>
//        /// <returns>"true" = registros excluidos com sucesso, "false" = erro ao tentar excluir registros (consulte a propriedade ErrorMessage para detalhes)</returns> 
//        public bool DeleteAll(SqlConnection ConnIn, SqlTransaction TranIn)
//        {
//            try
//            {
//                this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_DeleteAll", ConnIn, TranIn);
//                this.Cmd.CommandType = CommandType.StoredProcedure;
//                this.Cmd.Parameters.Clear();
//                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
//                return true;
//            }
//            catch (SqlException e)
//            {
//                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: {0}.",  e.Message);
//                return false;
//            }
//            catch (Exception e)
//            {
//                this._ErrorMessage = e.Message;
//                return false;
//            }
//        }
//
//        #endregion
//
//
//        #region Excluindo dados da tabela 
//
//        /// <summary> 
//        /// Exclui um registro da tabela no banco de dados
//        /// </summary>
//        /// <param name="FieldInfo">Objeto PosicaoIndicacaoFields a ser exclu�do.</param>
//        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//        public bool Delete( PosicaoIndicacaoFields FieldInfo )
//        {
//            return Delete(FieldInfo.idPosicaoIndicacao);
//        }
//
//        /// <summary> 
//        /// Exclui um registro da tabela no banco de dados
//        /// </summary>
//        /// <param name="Param_idPosicaoIndicacao">int</param>
//        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//        public bool Delete(
//                                     int Param_idPosicaoIndicacao)
//        {
//            try
//            {
//                this.Conn = new SqlConnection(this.StrConnetionDB);
//                this.Conn.Open();
//                this.Tran = this.Conn.BeginTransaction();
//                this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_Delete", this.Conn, this.Tran);
//                this.Cmd.CommandType = CommandType.StoredProcedure;
//                this.Cmd.Parameters.Clear();
//                this.Cmd.Parameters.Add(new SqlParameter("@Param_idPosicaoIndicacao", SqlDbType.Int)).Value = Param_idPosicaoIndicacao;
//                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
//                this.Tran.Commit();
//                return true;
//            }
//            catch (SqlException e)
//            {
//                this.Tran.Rollback();
//                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: {0}.",  e.Message);
//                return false;
//            }
//            catch (Exception e)
//            {
//                this.Tran.Rollback();
//                this._ErrorMessage = e.Message;
//                return false;
//            }
//            finally
//            {
//                if (this.Conn != null)
//                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//                if (this.Cmd != null)
//                  this.Cmd.Dispose();
//            }
//        }
//
//        #endregion
//
//
//        #region Excluindo dados da tabela utilizando conex�o e transa��o externa (compartilhada)
//
//        /// <summary> 
//        /// Exclui um registro da tabela no banco de dados
//        /// </summary>
//        /// <param name="ConnIn">Objeto SqlConnection respons�vel pela conex�o com o banco de dados.</param>
//        /// <param name="TranIn">Objeto SqlTransaction respons�vel pela transa��o iniciada no banco de dados.</param>
//        /// <param name="FieldInfo">Objeto PosicaoIndicacaoFields a ser exclu�do.</param>
//        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, PosicaoIndicacaoFields FieldInfo )
//        {
//            return Delete(ConnIn, TranIn, FieldInfo.idPosicaoIndicacao);
//        }
//
//        /// <summary> 
//        /// Exclui um registro da tabela no banco de dados
//        /// </summary>
//        /// <param name="Param_idPosicaoIndicacao">int</param>
//        /// <param name="ConnIn">Objeto SqlConnection respons�vel pela conex�o com o banco de dados.</param>
//        /// <param name="TranIn">Objeto SqlTransaction respons�vel pela transa��o iniciada no banco de dados.</param>
//        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, 
//                                     int Param_idPosicaoIndicacao)
//        {
//            try
//            {
//                this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_Delete", ConnIn, TranIn);
//                this.Cmd.CommandType = CommandType.StoredProcedure;
//                this.Cmd.Parameters.Clear();
//                this.Cmd.Parameters.Add(new SqlParameter("@Param_idPosicaoIndicacao", SqlDbType.Int)).Value = Param_idPosicaoIndicacao;
//                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
//                return true;
//            }
//            catch (SqlException e)
//            {
//                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: {0}.",  e.Message);
//                return false;
//            }
//            catch (Exception e)
//            {
//                this._ErrorMessage = e.Message;
//                return false;
//            }
//        }
//
//        #endregion
//
//
//        #region Selecionando um item da tabela 
//
//        /// <summary> 
//        /// Retorna um objeto PosicaoIndicacaoFields atrav�s da chave prim�ria passada como par�metro
//        /// </summary>
//        /// <param name="Param_idPosicaoIndicacao">int</param>
//        /// <returns>Objeto PosicaoIndicacaoFields</returns> 
//        public PosicaoIndicacaoFields GetItem(
//                                     int Param_idPosicaoIndicacao)
//        {
//            PosicaoIndicacaoFields infoFields = new PosicaoIndicacaoFields();
//            try
//            {
//                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//                {
//                    using (this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_Select", this.Conn))
//                    {
//                        this.Cmd.CommandType = CommandType.StoredProcedure;
//                        this.Cmd.Parameters.Clear();
//                        this.Cmd.Parameters.Add(new SqlParameter("@Param_idPosicaoIndicacao", SqlDbType.Int)).Value = Param_idPosicaoIndicacao;
//                        this.Cmd.Connection.Open();
//                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
//                        {
//                            if (!dr.HasRows) return null;
//                            if (dr.Read())
//                            {
//                               infoFields = GetDataFromReader( dr );
//                            }
//                        }
//                    }
//                 }
//
//                 return infoFields;
//
//            }
//            catch (SqlException e)
//            {
//                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
//                return null;
//            }
//            catch (Exception e)
//            {
//                this._ErrorMessage = e.Message;
//                return null;
//            }
//            finally
//            {
//                if (this.Conn != null)
//                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//            }
//        }
//
//        #endregion
//
//
//        #region Selecionando todos os dados da tabela 
//
//        /// <summary> 
//        /// Seleciona todos os registro da tabela e preenche um ArrayList com o objeto PosicaoIndicacaoFields.
//        /// </summary>
//        /// <returns>ArrayList de objetos PosicaoIndicacaoFields</returns> 
//        public ArrayList GetAll()
//        {
//            ArrayList arrayInfo = new ArrayList();
//            try
//            {
//                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//                {
//                    using (this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_GetAll", this.Conn))
//                    {
//                        this.Cmd.CommandType = CommandType.StoredProcedure;
//                        this.Cmd.Parameters.Clear();
//                        this.Cmd.Connection.Open();
//                        using (SqlDataReader dr = this.Cmd.ExecuteReader())
//                        {
//                           if (!dr.HasRows) return null;
//                           while (dr.Read())
//                           {
//                              arrayInfo.Add(GetDataFromReader( dr ));
//                           }
//                        }
//                    }
//                }
//
//                return arrayInfo;
//
//            }
//            catch (SqlException e)
//            {
//                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
//                return null;
//            }
//            catch (Exception e)
//            {
//                this._ErrorMessage = e.Message;
//                return null;
//            }
//            finally
//            {
//                if (this.Conn != null)
//                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//            }
//        }
//
//        #endregion
//
//
//        #region Contando os dados da tabela 
//
//        /// <summary> 
//        /// Retorna o total de registros contidos na tabela
//        /// </summary>
//        /// <returns>int</returns> 
//        public int CountAll()
//        {
//            try
//            {
//                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//                {
//                    using (this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_CountAll", this.Conn))
//                    {
//                        this.Cmd.CommandType = CommandType.StoredProcedure;
//                        this.Cmd.Parameters.Clear();
//                        this.Cmd.Connection.Open();
//                        object CountRegs = this.Cmd.ExecuteScalar();
//                        if (CountRegs == null)
//                        { return 0; }
//                        else
//                        { return (int)CountRegs; }
//                    }
//                }
//            }
//            catch (SqlException e)
//            {
//                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
//                return 0;
//            }
//            catch (Exception e)
//            {
//                this._ErrorMessage = e.Message;
//                return 0;
//            }
//            finally
//            {
//                if (this.Conn != null)
//                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//            }
//        }
//
//        #endregion
//
//
//        #region Selecionando dados da tabela atrav�s do campo "NomeUsuarioRecebe" 
//
//        /// <summary> 
//        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo NomeUsuarioRecebe.
//        /// </summary>
//        /// <param name="Param_NomeUsuarioRecebe">string</param>
//        /// <returns>PosicaoIndicacaoFields</returns> 
//        public PosicaoIndicacaoFields FindByNomeUsuarioRecebe(
//                               string Param_NomeUsuarioRecebe )
//        {
//            PosicaoIndicacaoFields infoFields = new PosicaoIndicacaoFields();
//            try
//            {
//                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//                {
//                    using (this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_FindByNomeUsuarioRecebe", this.Conn))
//                    {
//                        this.Cmd.CommandType = CommandType.StoredProcedure;
//                        this.Cmd.Parameters.Clear();
//                        this.Cmd.Parameters.Add(new SqlParameter("@Param_NomeUsuarioRecebe", SqlDbType.VarChar, 50)).Value = Param_NomeUsuarioRecebe;
//                        this.Cmd.Connection.Open();
//                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
//                        {
//                            if (!dr.HasRows) return null;
//                            if (dr.Read())
//                            {
//                               infoFields = GetDataFromReader( dr );
//                            }
//                        }
//                    }
//                }
//
//                return infoFields;
//
//            }
//            catch (SqlException e)
//            {
//                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
//                return null;
//            }
//            catch (Exception e)
//            {
//                this._ErrorMessage = e.Message;
//                return null;
//            }
//            finally
//            {
//                if (this.Conn != null)
//                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//            }
//        }
//
//        #endregion
//
//
//
//        #region Selecionando dados da tabela atrav�s do campo "NomeUsuarioIndica" 
//
//        /// <summary> 
//        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo NomeUsuarioIndica.
//        /// </summary>
//        /// <param name="Param_NomeUsuarioIndica">string</param>
//        /// <returns>PosicaoIndicacaoFields</returns> 
//        public PosicaoIndicacaoFields FindByNomeUsuarioIndica(
//                               string Param_NomeUsuarioIndica )
//        {
//            PosicaoIndicacaoFields infoFields = new PosicaoIndicacaoFields();
//            try
//            {
//                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//                {
//                    using (this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_FindByNomeUsuarioIndica", this.Conn))
//                    {
//                        this.Cmd.CommandType = CommandType.StoredProcedure;
//                        this.Cmd.Parameters.Clear();
//                        this.Cmd.Parameters.Add(new SqlParameter("@Param_NomeUsuarioIndica", SqlDbType.VarChar, 50)).Value = Param_NomeUsuarioIndica;
//                        this.Cmd.Connection.Open();
//                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
//                        {
//                            if (!dr.HasRows) return null;
//                            if (dr.Read())
//                            {
//                               infoFields = GetDataFromReader( dr );
//                            }
//                        }
//                    }
//                }
//
//                return infoFields;
//
//            }
//            catch (SqlException e)
//            {
//                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
//                return null;
//            }
//            catch (Exception e)
//            {
//                this._ErrorMessage = e.Message;
//                return null;
//            }
//            finally
//            {
//                if (this.Conn != null)
//                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//            }
//        }
//
//        #endregion
//
//
//
//        #region Fun��o GetDataFromReader
//
//        /// <summary> 
//        /// Retorna um objeto PosicaoIndicacaoFields preenchido com os valores dos campos do SqlDataReader
//        /// </summary>
//        /// <param name="dr">SqlDataReader - Preenche o objeto PosicaoIndicacaoFields </param>
//        /// <returns>PosicaoIndicacaoFields</returns>
//        private PosicaoIndicacaoFields GetDataFromReader( SqlDataReader dr )
//        {
//            PosicaoIndicacaoFields infoFields = new PosicaoIndicacaoFields();
//
//            if (!dr.IsDBNull(0))
//            { infoFields.idPosicaoIndicacao = dr.GetInt32(0); }
//            else
//            { infoFields.idPosicaoIndicacao = 0; }
//
//
//
//            if (!dr.IsDBNull(1))
//            { infoFields.NomeUsuarioRecebe = dr.GetString(1); }
//            else
//            { infoFields.NomeUsuarioRecebe = string.Empty; }
//
//
//
//            if (!dr.IsDBNull(2))
//            { infoFields.NomeUsuarioIndica = dr.GetString(2); }
//            else
//            { infoFields.NomeUsuarioIndica = string.Empty; }
//
//
//            return infoFields;
//        }
//        #endregion
//
//
//
//
//
//
//
//
//
//
//
//
//        #region Fun��o GetAllParameters
//
//        /// <summary> 
//        /// Retorna um array de par�metros com campos para atualiza��o, sele��o e inser��o no banco de dados
//        /// </summary>
//        /// <param name="FieldInfo">Objeto PosicaoIndicacaoFields</param>
//        /// <param name="Modo">Tipo de oepra��o a ser executada no banco de dados</param>
//        /// <returns>SqlParameter[] - Array de par�metros</returns> 
//        private SqlParameter[] GetAllParameters( PosicaoIndicacaoFields FieldInfo, SQLMode Modo )
//        {
//            SqlParameter[] Parameters;
//
//            switch (Modo)
//            {
//                case SQLMode.Add:
//                    Parameters = new SqlParameter[3];
//                    for (int I = 0; I < Parameters.Length; I++)
//                       Parameters[I] = new SqlParameter();
//                    //Field idPosicaoIndicacao
//                    Parameters[0].SqlDbType = SqlDbType.Int;
//                    Parameters[0].Direction = ParameterDirection.Output;
//                    Parameters[0].ParameterName = "@Param_idPosicaoIndicacao";
//                    Parameters[0].Value = DBNull.Value;
//
//                    break;
//
//                case SQLMode.Update:
//                    Parameters = new SqlParameter[3];
//                    for (int I = 0; I < Parameters.Length; I++)
//                       Parameters[I] = new SqlParameter();
//                    //Field idPosicaoIndicacao
//                    Parameters[0].SqlDbType = SqlDbType.Int;
//                    Parameters[0].ParameterName = "@Param_idPosicaoIndicacao";
//                    Parameters[0].Value = FieldInfo.idPosicaoIndicacao;
//
//                    break;
//
//                case SQLMode.SelectORDelete:
//                    Parameters = new SqlParameter[1];
//                    for (int I = 0; I < Parameters.Length; I++)
//                       Parameters[I] = new SqlParameter();
//                    //Field idPosicaoIndicacao
//                    Parameters[0].SqlDbType = SqlDbType.Int;
//                    Parameters[0].ParameterName = "@Param_idPosicaoIndicacao";
//                    Parameters[0].Value = FieldInfo.idPosicaoIndicacao;
//
//                    return Parameters;
//
//                default:
//                    Parameters = new SqlParameter[3];
//                    for (int I = 0; I < Parameters.Length; I++)
//                       Parameters[I] = new SqlParameter();
//                    break;
//            }
//
//            //Field NomeUsuarioRecebe
//            Parameters[1].SqlDbType = SqlDbType.VarChar;
//            Parameters[1].ParameterName = "@Param_NomeUsuarioRecebe";
//            if (( FieldInfo.NomeUsuarioRecebe == null ) || ( FieldInfo.NomeUsuarioRecebe == string.Empty ))
//            { Parameters[1].Value = DBNull.Value; }
//            else
//            { Parameters[1].Value = FieldInfo.NomeUsuarioRecebe; }
//            Parameters[1].Size = 50;
//
//            //Field NomeUsuarioIndica
//            Parameters[2].SqlDbType = SqlDbType.VarChar;
//            Parameters[2].ParameterName = "@Param_NomeUsuarioIndica";
//            if (( FieldInfo.NomeUsuarioIndica == null ) || ( FieldInfo.NomeUsuarioIndica == string.Empty ))
//            { Parameters[2].Value = DBNull.Value; }
//            else
//            { Parameters[2].Value = FieldInfo.NomeUsuarioIndica; }
//            Parameters[2].Size = 50;
//
//            return Parameters;
//        }
//        #endregion
//
//
//
//
//
//        #region IDisposable Members 
//
//        bool disposed = false;
//
//        public void Dispose()
//        {
//            Dispose(true);
//            GC.SuppressFinalize(this);
//        }
//
//        ~PosicaoIndicacaoControl() 
//        { 
//            Dispose(false); 
//        }
//
//        private void Dispose(bool disposing) 
//        {
//            if (!this.disposed)
//            {
//                if (disposing) 
//                {
//                    if (this.Conn != null)
//                        if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//                }
//            }
//
//        }
//        #endregion 
//
//
//
//    }
//
//}
//
//
//
//
//
////Projeto substitu�do ------------------------
////using System;
////using System.Collections;
////using System.Collections.Generic;
////using System.Data;
////using System.Data.SqlClient;
////using System.Configuration;
////using System.Text;
////
////namespace SWGPgen
////{
////
////
////    /// <summary> 
////    /// Tabela: PosicaoIndicacao  
////    /// Autor: DAL Creator .net 
////    /// Data de cria��o: 13/03/2012 21:19:06 
////    /// Descri��o: Classe respons�vel pela perssit�ncia de dados. Utiliza a classe "PosicaoIndicacaoFields". 
////    /// </summary> 
////    public class PosicaoIndicacaoControl : IDisposable 
////    {
////
////        #region String de conex�o 
////        private string StrConnetionDB = "Data Source=DEKO-PC;Initial Catalog=swgp;User Id=sureg;Password=@sureg2012;";
////        #endregion
////
////
////        #region Propriedade que armazena erros de execu��o 
////        private string _ErrorMessage;
////        public string ErrorMessage { get { return _ErrorMessage; } }
////        #endregion
////
////
////        #region Objetos de conex�o 
////        SqlConnection Conn;
////        SqlCommand Cmd;
////        SqlTransaction Tran;
////        #endregion
////
////
////        #region Func�es que retornam Conex�es e Transa��es 
////
////        public SqlTransaction GetNewTransaction(SqlConnection connIn)
////        {
////            if (connIn.State != ConnectionState.Open)
////                connIn.Open();
////            SqlTransaction TranOut = connIn.BeginTransaction();
////            return TranOut;
////        }
////
////        public SqlConnection GetNewConnection()
////        {
////            return GetNewConnection(this.StrConnetionDB);
////        }
////
////        public SqlConnection GetNewConnection(string StringConnection)
////        {
////            SqlConnection connOut = new SqlConnection(StringConnection);
////            return connOut;
////        }
////
////        #endregion
////
////
////        #region enum SQLMode 
////        /// <summary>   
////        /// Representa o procedimento que est� sendo executado na tabela.
////        /// </summary>
////        public enum SQLMode
////        {                     
////            /// <summary>
////            /// Adiciona registro na tabela.
////            /// </summary>
////            Add,
////            /// <summary>
////            /// Atualiza registro na tabela.
////            /// </summary>
////            Update,
////            /// <summary>
////            /// Excluir registro na tabela
////            /// </summary>
////            Delete,
////            /// <summary>
////            /// Exclui TODOS os registros da tabela.
////            /// </summary>
////            DeleteAll,
////            /// <summary>
////            /// Seleciona um registro na tabela.
////            /// </summary>
////            Select,
////            /// <summary>
////            /// Seleciona TODOS os registros da tabela.
////            /// </summary>
////            SelectAll,
////            /// <summary>
////            /// Excluir ou seleciona um registro na tabela.
////            /// </summary>
////            SelectORDelete
////        }
////        #endregion 
////
////
////        public PosicaoIndicacaoControl() {}
////
////
////        #region Inserindo dados na tabela 
////
////        /// <summary> 
////        /// Grava/Persiste um novo objeto PosicaoIndicacaoFields no banco de dados
////        /// </summary>
////        /// <param name="FieldInfo">Objeto PosicaoIndicacaoFields a ser gravado.Caso o par�metro solicite a express�o "ref", ser� adicionado um novo valor a algum campo auto incremento.</param>
////        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////        public bool Add( ref PosicaoIndicacaoFields FieldInfo )
////        {
////            try
////            {
////                this.Conn = new SqlConnection(this.StrConnetionDB);
////                this.Conn.Open();
////                this.Tran = this.Conn.BeginTransaction();
////                this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_Add", this.Conn, this.Tran);
////                this.Cmd.CommandType = CommandType.StoredProcedure;
////                this.Cmd.Parameters.Clear();
////                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
////                this.Tran.Commit();
////                FieldInfo.idPosicaoIndicacao = (int)this.Cmd.Parameters["@Param_idPosicaoIndicacao"].Value;
////                return true;
////
////            }
////            catch (SqlException e)
////            {
////                this.Tran.Rollback();
////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: {0}.",  e.Message);
////                return false;
////            }
////            catch (Exception e)
////            {
////                this.Tran.Rollback();
////                this._ErrorMessage = e.Message;
////                return false;
////            }
////            finally
////            {
////                if (this.Conn != null)
////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////                if (this.Cmd != null)
////                  this.Cmd.Dispose();
////            }
////        }
////
////        #endregion
////
////
////        #region Inserindo dados na tabela utilizando conex�o e transa��o externa (compartilhada) 
////
////        /// <summary> 
////        /// Grava/Persiste um novo objeto PosicaoIndicacaoFields no banco de dados
////        /// </summary>
////        /// <param name="ConnIn">Objeto SqlConnection respons�vel pela conex�o com o banco de dados.</param>
////        /// <param name="TranIn">Objeto SqlTransaction respons�vel pela transa��o iniciada no banco de dados.</param>
////        /// <param name="FieldInfo">Objeto PosicaoIndicacaoFields a ser gravado.Caso o par�metro solicite a express�o "ref", ser� adicionado um novo valor a algum campo auto incremento.</param>
////        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////        public bool Add( SqlConnection ConnIn, SqlTransaction TranIn, ref PosicaoIndicacaoFields FieldInfo )
////        {
////            try
////            {
////                this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_Add", ConnIn, TranIn);
////                this.Cmd.CommandType = CommandType.StoredProcedure;
////                this.Cmd.Parameters.Clear();
////                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
////                FieldInfo.idPosicaoIndicacao = (int)this.Cmd.Parameters["@Param_idPosicaoIndicacao"].Value;
////                return true;
////
////            }
////            catch (SqlException e)
////            {
////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: {0}.",  e.Message);
////                return false;
////            }
////            catch (Exception e)
////            {
////                this._ErrorMessage = e.Message;
////                return false;
////            }
////        }
////
////        #endregion
////
////
////        #region Editando dados na tabela 
////
////        /// <summary> 
////        /// Grava/Persiste as altera��es em um objeto PosicaoIndicacaoFields no banco de dados
////        /// </summary>
////        /// <param name="FieldInfo">Objeto PosicaoIndicacaoFields a ser alterado.</param>
////        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////        public bool Update( PosicaoIndicacaoFields FieldInfo )
////        {
////            try
////            {
////                this.Conn = new SqlConnection(this.StrConnetionDB);
////                this.Conn.Open();
////                this.Tran = this.Conn.BeginTransaction();
////                this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_Update", this.Conn, this.Tran);
////                this.Cmd.CommandType = CommandType.StoredProcedure;
////                this.Cmd.Parameters.Clear();
////                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Update));
////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar atualizar registro!!");
////                this.Tran.Commit();
////                return true;
////            }
////            catch (SqlException e)
////            {
////                this.Tran.Rollback();
////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: {0}.",  e.Message);
////                return false;
////            }
////            catch (Exception e)
////            {
////                this.Tran.Rollback();
////                this._ErrorMessage = e.Message;
////                return false;
////            }
////            finally
////            {
////                if (this.Conn != null)
////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////                if (this.Cmd != null)
////                  this.Cmd.Dispose();
////            }
////        }
////
////        #endregion
////
////
////        #region Editando dados na tabela utilizando conex�o e transa��o externa (compartilhada) 
////
////        /// <summary> 
////        /// Grava/Persiste as altera��es em um objeto PosicaoIndicacaoFields no banco de dados
////        /// </summary>
////        /// <param name="ConnIn">Objeto SqlConnection respons�vel pela conex�o com o banco de dados.</param>
////        /// <param name="TranIn">Objeto SqlTransaction respons�vel pela transa��o iniciada no banco de dados.</param>
////        /// <param name="FieldInfo">Objeto PosicaoIndicacaoFields a ser alterado.</param>
////        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////        public bool Update( SqlConnection ConnIn, SqlTransaction TranIn, PosicaoIndicacaoFields FieldInfo )
////        {
////            try
////            {
////                this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_Update", ConnIn, TranIn);
////                this.Cmd.CommandType = CommandType.StoredProcedure;
////                this.Cmd.Parameters.Clear();
////                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Update));
////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar atualizar registro!!");
////                return true;
////            }
////            catch (SqlException e)
////            {
////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: {0}.",  e.Message);
////                return false;
////            }
////            catch (Exception e)
////            {
////                this._ErrorMessage = e.Message;
////                return false;
////            }
////        }
////
////        #endregion
////
////
////        #region Excluindo todos os dados da tabela 
////
////        /// <summary> 
////        /// Exclui todos os registros da tabela
////        /// </summary>
////        /// <returns>"true" = registros excluidos com sucesso, "false" = erro ao tentar excluir registros (consulte a propriedade ErrorMessage para detalhes)</returns> 
////        public bool DeleteAll()
////        {
////            try
////            {
////                this.Conn = new SqlConnection(this.StrConnetionDB);
////                this.Conn.Open();
////                this.Tran = this.Conn.BeginTransaction();
////                this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_DeleteAll", this.Conn, this.Tran);
////                this.Cmd.CommandType = CommandType.StoredProcedure;
////                this.Cmd.Parameters.Clear();
////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
////                this.Tran.Commit();
////                return true;
////            }
////            catch (SqlException e)
////            {
////                this.Tran.Rollback();
////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: {0}.",  e.Message);
////                return false;
////            }
////            catch (Exception e)
////            {
////                this.Tran.Rollback();
////                this._ErrorMessage = e.Message;
////                return false;
////            }
////            finally
////            {
////                if (this.Conn != null)
////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////                if (this.Cmd != null)
////                  this.Cmd.Dispose();
////            }
////        }
////
////        #endregion
////
////
////        #region Excluindo todos os dados da tabela utilizando conex�o e transa��o externa (compartilhada)
////
////        /// <summary> 
////        /// Exclui todos os registros da tabela
////        /// </summary>
////        /// <param name="ConnIn">Objeto SqlConnection respons�vel pela conex�o com o banco de dados.</param>
////        /// <param name="TranIn">Objeto SqlTransaction respons�vel pela transa��o iniciada no banco de dados.</param>
////        /// <returns>"true" = registros excluidos com sucesso, "false" = erro ao tentar excluir registros (consulte a propriedade ErrorMessage para detalhes)</returns> 
////        public bool DeleteAll(SqlConnection ConnIn, SqlTransaction TranIn)
////        {
////            try
////            {
////                this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_DeleteAll", ConnIn, TranIn);
////                this.Cmd.CommandType = CommandType.StoredProcedure;
////                this.Cmd.Parameters.Clear();
////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
////                return true;
////            }
////            catch (SqlException e)
////            {
////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: {0}.",  e.Message);
////                return false;
////            }
////            catch (Exception e)
////            {
////                this._ErrorMessage = e.Message;
////                return false;
////            }
////        }
////
////        #endregion
////
////
////        #region Excluindo dados da tabela 
////
////        /// <summary> 
////        /// Exclui um registro da tabela no banco de dados
////        /// </summary>
////        /// <param name="FieldInfo">Objeto PosicaoIndicacaoFields a ser exclu�do.</param>
////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////        public bool Delete( PosicaoIndicacaoFields FieldInfo )
////        {
////            return Delete(FieldInfo.idPosicaoIndicacao);
////        }
////
////        /// <summary> 
////        /// Exclui um registro da tabela no banco de dados
////        /// </summary>
////        /// <param name="Param_idPosicaoIndicacao">int</param>
////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////        public bool Delete(
////                                     int Param_idPosicaoIndicacao)
////        {
////            try
////            {
////                this.Conn = new SqlConnection(this.StrConnetionDB);
////                this.Conn.Open();
////                this.Tran = this.Conn.BeginTransaction();
////                this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_Delete", this.Conn, this.Tran);
////                this.Cmd.CommandType = CommandType.StoredProcedure;
////                this.Cmd.Parameters.Clear();
////                this.Cmd.Parameters.Add(new SqlParameter("@Param_idPosicaoIndicacao", SqlDbType.Int)).Value = Param_idPosicaoIndicacao;
////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
////                this.Tran.Commit();
////                return true;
////            }
////            catch (SqlException e)
////            {
////                this.Tran.Rollback();
////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: {0}.",  e.Message);
////                return false;
////            }
////            catch (Exception e)
////            {
////                this.Tran.Rollback();
////                this._ErrorMessage = e.Message;
////                return false;
////            }
////            finally
////            {
////                if (this.Conn != null)
////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////                if (this.Cmd != null)
////                  this.Cmd.Dispose();
////            }
////        }
////
////        #endregion
////
////
////        #region Excluindo dados da tabela utilizando conex�o e transa��o externa (compartilhada)
////
////        /// <summary> 
////        /// Exclui um registro da tabela no banco de dados
////        /// </summary>
////        /// <param name="ConnIn">Objeto SqlConnection respons�vel pela conex�o com o banco de dados.</param>
////        /// <param name="TranIn">Objeto SqlTransaction respons�vel pela transa��o iniciada no banco de dados.</param>
////        /// <param name="FieldInfo">Objeto PosicaoIndicacaoFields a ser exclu�do.</param>
////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, PosicaoIndicacaoFields FieldInfo )
////        {
////            return Delete(ConnIn, TranIn, FieldInfo.idPosicaoIndicacao);
////        }
////
////        /// <summary> 
////        /// Exclui um registro da tabela no banco de dados
////        /// </summary>
////        /// <param name="Param_idPosicaoIndicacao">int</param>
////        /// <param name="ConnIn">Objeto SqlConnection respons�vel pela conex�o com o banco de dados.</param>
////        /// <param name="TranIn">Objeto SqlTransaction respons�vel pela transa��o iniciada no banco de dados.</param>
////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, 
////                                     int Param_idPosicaoIndicacao)
////        {
////            try
////            {
////                this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_Delete", ConnIn, TranIn);
////                this.Cmd.CommandType = CommandType.StoredProcedure;
////                this.Cmd.Parameters.Clear();
////                this.Cmd.Parameters.Add(new SqlParameter("@Param_idPosicaoIndicacao", SqlDbType.Int)).Value = Param_idPosicaoIndicacao;
////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
////                return true;
////            }
////            catch (SqlException e)
////            {
////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: {0}.",  e.Message);
////                return false;
////            }
////            catch (Exception e)
////            {
////                this._ErrorMessage = e.Message;
////                return false;
////            }
////        }
////
////        #endregion
////
////
////        #region Selecionando um item da tabela 
////
////        /// <summary> 
////        /// Retorna um objeto PosicaoIndicacaoFields atrav�s da chave prim�ria passada como par�metro
////        /// </summary>
////        /// <param name="Param_idPosicaoIndicacao">int</param>
////        /// <returns>Objeto PosicaoIndicacaoFields</returns> 
////        public PosicaoIndicacaoFields GetItem(
////                                     int Param_idPosicaoIndicacao)
////        {
////            PosicaoIndicacaoFields infoFields = new PosicaoIndicacaoFields();
////            try
////            {
////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////                {
////                    using (this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_Select", this.Conn))
////                    {
////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////                        this.Cmd.Parameters.Clear();
////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_idPosicaoIndicacao", SqlDbType.Int)).Value = Param_idPosicaoIndicacao;
////                        this.Cmd.Connection.Open();
////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
////                        {
////                            if (!dr.HasRows) return null;
////                            if (dr.Read())
////                            {
////                               infoFields = GetDataFromReader( dr );
////                            }
////                        }
////                    }
////                 }
////
////                 return infoFields;
////
////            }
////            catch (SqlException e)
////            {
////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
////                return null;
////            }
////            catch (Exception e)
////            {
////                this._ErrorMessage = e.Message;
////                return null;
////            }
////            finally
////            {
////                if (this.Conn != null)
////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////            }
////        }
////
////        #endregion
////
////
////        #region Selecionando todos os dados da tabela 
////
////        /// <summary> 
////        /// Seleciona todos os registro da tabela e preenche um ArrayList com o objeto PosicaoIndicacaoFields.
////        /// </summary>
////        /// <returns>List de objetos PosicaoIndicacaoFields</returns> 
////        public List<PosicaoIndicacaoFields> GetAll()
////        {
////            List<PosicaoIndicacaoFields> arrayInfo = new List<PosicaoIndicacaoFields>();
////            try
////            {
////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////                {
////                    using (this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_GetAll", this.Conn))
////                    {
////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////                        this.Cmd.Parameters.Clear();
////                        this.Cmd.Connection.Open();
////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
////                        {
////                           if (!dr.HasRows) return null;
////                           while (dr.Read())
////                           {
////                              arrayInfo.Add(GetDataFromReader( dr ));
////                           }
////                        }
////                    }
////                }
////
////                return arrayInfo;
////
////            }
////            catch (SqlException e)
////            {
////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
////                return null;
////            }
////            catch (Exception e)
////            {
////                this._ErrorMessage = e.Message;
////                return null;
////            }
////            finally
////            {
////                if (this.Conn != null)
////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////            }
////        }
////
////        #endregion
////
////
////        #region Contando os dados da tabela 
////
////        /// <summary> 
////        /// Retorna o total de registros contidos na tabela
////        /// </summary>
////        /// <returns>int</returns> 
////        public int CountAll()
////        {
////            try
////            {
////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////                {
////                    using (this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_CountAll", this.Conn))
////                    {
////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////                        this.Cmd.Parameters.Clear();
////                        this.Cmd.Connection.Open();
////                        object CountRegs = this.Cmd.ExecuteScalar();
////                        if (CountRegs == null)
////                        { return 0; }
////                        else
////                        { return (int)CountRegs; }
////                    }
////                }
////            }
////            catch (SqlException e)
////            {
////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
////                return 0;
////            }
////            catch (Exception e)
////            {
////                this._ErrorMessage = e.Message;
////                return 0;
////            }
////            finally
////            {
////                if (this.Conn != null)
////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////            }
////        }
////
////        #endregion
////
////
////        #region Selecionando dados da tabela atrav�s do campo "NomeUsuarioRecebe" 
////
////        /// <summary> 
////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo NomeUsuarioRecebe.
////        /// </summary>
////        /// <param name="Param_NomeUsuarioRecebe">string</param>
////        /// <returns>ArrayList</returns> 
////        public ArrayList FindByNomeUsuarioRecebe(
////                               string Param_NomeUsuarioRecebe )
////        {
////            ArrayList arrayInfo = new ArrayList();
////            try
////            {
////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////                {
////                    using (this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_FindByNomeUsuarioRecebe", this.Conn))
////                    {
////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////                        this.Cmd.Parameters.Clear();
////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_NomeUsuarioRecebe", SqlDbType.VarChar, 50)).Value = Param_NomeUsuarioRecebe;
////                        this.Cmd.Connection.Open();
////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
////                        {
////                            if (!dr.HasRows) return null;
////                            while (dr.Read())
////                            {
////                               arrayInfo.Add(GetDataFromReader( dr ));
////                            }
////                        }
////                    }
////                }
////
////                return arrayInfo;
////
////            }
////            catch (SqlException e)
////            {
////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
////                return null;
////            }
////            catch (Exception e)
////            {
////                this._ErrorMessage = e.Message;
////                return null;
////            }
////            finally
////            {
////                if (this.Conn != null)
////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////            }
////        }
////
////        #endregion
////
////
////
////        #region Selecionando dados da tabela atrav�s do campo "NomeUsuarioIndica" 
////
////        /// <summary> 
////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo NomeUsuarioIndica.
////        /// </summary>
////        /// <param name="Param_NomeUsuarioIndica">string</param>
////        /// <returns>ArrayList</returns> 
////        public ArrayList FindByNomeUsuarioIndica(
////                               string Param_NomeUsuarioIndica )
////        {
////            ArrayList arrayInfo = new ArrayList();
////            try
////            {
////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////                {
////                    using (this.Cmd = new SqlCommand("Proc_PosicaoIndicacao_FindByNomeUsuarioIndica", this.Conn))
////                    {
////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////                        this.Cmd.Parameters.Clear();
////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_NomeUsuarioIndica", SqlDbType.VarChar, 50)).Value = Param_NomeUsuarioIndica;
////                        this.Cmd.Connection.Open();
////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
////                        {
////                            if (!dr.HasRows) return null;
////                            while (dr.Read())
////                            {
////                               arrayInfo.Add(GetDataFromReader( dr ));
////                            }
////                        }
////                    }
////                }
////
////                return arrayInfo;
////
////            }
////            catch (SqlException e)
////            {
////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: C�digo do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
////                return null;
////            }
////            catch (Exception e)
////            {
////                this._ErrorMessage = e.Message;
////                return null;
////            }
////            finally
////            {
////                if (this.Conn != null)
////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////            }
////        }
////
////        #endregion
////
////
////
////        #region Fun��o GetDataFromReader
////
////        /// <summary> 
////        /// Retorna um objeto PosicaoIndicacaoFields preenchido com os valores dos campos do SqlDataReader
////        /// </summary>
////        /// <param name="dr">SqlDataReader - Preenche o objeto PosicaoIndicacaoFields </param>
////        /// <returns>PosicaoIndicacaoFields</returns>
////        private PosicaoIndicacaoFields GetDataFromReader( SqlDataReader dr )
////        {
////            PosicaoIndicacaoFields infoFields = new PosicaoIndicacaoFields();
////
////            if (!dr.IsDBNull(0))
////            { infoFields.idPosicaoIndicacao = dr.GetInt32(0); }
////            else
////            { infoFields.idPosicaoIndicacao = 0; }
////
////
////
////            if (!dr.IsDBNull(1))
////            { infoFields.NomeUsuarioRecebe = dr.GetString(1); }
////            else
////            { infoFields.NomeUsuarioRecebe = string.Empty; }
////
////
////
////            if (!dr.IsDBNull(2))
////            { infoFields.NomeUsuarioIndica = dr.GetString(2); }
////            else
////            { infoFields.NomeUsuarioIndica = string.Empty; }
////
////
////            return infoFields;
////        }
////        #endregion
////
////
////
////
////
////
////
////
////
////
////
////
////        #region Fun��o GetAllParameters
////
////        /// <summary> 
////        /// Retorna um array de par�metros com campos para atualiza��o, sele��o e inser��o no banco de dados
////        /// </summary>
////        /// <param name="FieldInfo">Objeto PosicaoIndicacaoFields</param>
////        /// <param name="Modo">Tipo de oepra��o a ser executada no banco de dados</param>
////        /// <returns>SqlParameter[] - Array de par�metros</returns> 
////        private SqlParameter[] GetAllParameters( PosicaoIndicacaoFields FieldInfo, SQLMode Modo )
////        {
////            SqlParameter[] Parameters;
////
////            switch (Modo)
////            {
////                case SQLMode.Add:
////                    Parameters = new SqlParameter[3];
////                    for (int I = 0; I < Parameters.Length; I++)
////                       Parameters[I] = new SqlParameter();
////                    //Field idPosicaoIndicacao
////                    Parameters[0].SqlDbType = SqlDbType.Int;
////                    Parameters[0].Direction = ParameterDirection.Output;
////                    Parameters[0].ParameterName = "@Param_idPosicaoIndicacao";
////                    Parameters[0].Value = DBNull.Value;
////
////                    break;
////
////                case SQLMode.Update:
////                    Parameters = new SqlParameter[3];
////                    for (int I = 0; I < Parameters.Length; I++)
////                       Parameters[I] = new SqlParameter();
////                    //Field idPosicaoIndicacao
////                    Parameters[0].SqlDbType = SqlDbType.Int;
////                    Parameters[0].ParameterName = "@Param_idPosicaoIndicacao";
////                    Parameters[0].Value = FieldInfo.idPosicaoIndicacao;
////
////                    break;
////
////                case SQLMode.SelectORDelete:
////                    Parameters = new SqlParameter[1];
////                    for (int I = 0; I < Parameters.Length; I++)
////                       Parameters[I] = new SqlParameter();
////                    //Field idPosicaoIndicacao
////                    Parameters[0].SqlDbType = SqlDbType.Int;
////                    Parameters[0].ParameterName = "@Param_idPosicaoIndicacao";
////                    Parameters[0].Value = FieldInfo.idPosicaoIndicacao;
////
////                    return Parameters;
////
////                default:
////                    Parameters = new SqlParameter[3];
////                    for (int I = 0; I < Parameters.Length; I++)
////                       Parameters[I] = new SqlParameter();
////                    break;
////            }
////
////            //Field NomeUsuarioRecebe
////            Parameters[1].SqlDbType = SqlDbType.VarChar;
////            Parameters[1].ParameterName = "@Param_NomeUsuarioRecebe";
////            if (( FieldInfo.NomeUsuarioRecebe == null ) || ( FieldInfo.NomeUsuarioRecebe == string.Empty ))
////            { Parameters[1].Value = DBNull.Value; }
////            else
////            { Parameters[1].Value = FieldInfo.NomeUsuarioRecebe; }
////            Parameters[1].Size = 50;
////
////            //Field NomeUsuarioIndica
////            Parameters[2].SqlDbType = SqlDbType.VarChar;
////            Parameters[2].ParameterName = "@Param_NomeUsuarioIndica";
////            if (( FieldInfo.NomeUsuarioIndica == null ) || ( FieldInfo.NomeUsuarioIndica == string.Empty ))
////            { Parameters[2].Value = DBNull.Value; }
////            else
////            { Parameters[2].Value = FieldInfo.NomeUsuarioIndica; }
////            Parameters[2].Size = 50;
////
////            return Parameters;
////        }
////        #endregion
////
////
////
////
////
////        #region IDisposable Members 
////
////        bool disposed = false;
////
////        public void Dispose()
////        {
////            Dispose(true);
////            GC.SuppressFinalize(this);
////        }
////
////        ~PosicaoIndicacaoControl() 
////        { 
////            Dispose(false); 
////        }
////
////        private void Dispose(bool disposing) 
////        {
////            if (!this.disposed)
////            {
////                if (disposing) 
////                {
////                    if (this.Conn != null)
////                        if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////                }
////            }
////
////        }
////        #endregion 
////
////
////
////    }
////
////}
