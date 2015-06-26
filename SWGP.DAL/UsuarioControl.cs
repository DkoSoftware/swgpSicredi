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
    /// Tabela: Usuario  
    /// Autor: DAL Creator .net 
    /// Data de criação: 24/03/2012 20:08:47 
    /// Descrição: Classe responsável pela perssitência de dados. Utiliza a classe "UsuarioFields". 
    /// </summary> 
    public class UsuarioControl : IDisposable 
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


        public UsuarioControl() {}


        #region Inserindo dados na tabela 

        /// <summary> 
        /// Grava/Persiste um novo objeto UsuarioFields no banco de dados
        /// </summary>
        /// <param name="FieldInfo">Objeto UsuarioFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Add( ref UsuarioFields FieldInfo )
        {
            try
            {
                this.Conn = new SqlConnection(this.StrConnetionDB);
                this.Conn.Open();
                this.Tran = this.Conn.BeginTransaction();
                this.Cmd = new SqlCommand("Proc_Usuario_Add", this.Conn, this.Tran);
                this.Cmd.CommandType = CommandType.StoredProcedure;
                this.Cmd.Parameters.Clear();
                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
                this.Tran.Commit();
                FieldInfo.idUsuario = (int)this.Cmd.Parameters["@Param_idUsuario"].Value;
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
        /// Grava/Persiste um novo objeto UsuarioFields no banco de dados
        /// </summary>
        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
        /// <param name="FieldInfo">Objeto UsuarioFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Add( SqlConnection ConnIn, SqlTransaction TranIn, ref UsuarioFields FieldInfo )
        {
            try
            {
                this.Cmd = new SqlCommand("Proc_Usuario_Add", ConnIn, TranIn);
                this.Cmd.CommandType = CommandType.StoredProcedure;
                this.Cmd.Parameters.Clear();
                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
                FieldInfo.idUsuario = (int)this.Cmd.Parameters["@Param_idUsuario"].Value;
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
        /// Grava/Persiste as alterações em um objeto UsuarioFields no banco de dados
        /// </summary>
        /// <param name="FieldInfo">Objeto UsuarioFields a ser alterado.</param>
        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Update( UsuarioFields FieldInfo )
        {
            try
            {
                this.Conn = new SqlConnection(this.StrConnetionDB);
                this.Conn.Open();
                this.Tran = this.Conn.BeginTransaction();
                this.Cmd = new SqlCommand("Proc_Usuario_Update", this.Conn, this.Tran);
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
        /// Grava/Persiste as alterações em um objeto UsuarioFields no banco de dados
        /// </summary>
        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
        /// <param name="FieldInfo">Objeto UsuarioFields a ser alterado.</param>
        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Update( SqlConnection ConnIn, SqlTransaction TranIn, UsuarioFields FieldInfo )
        {
            try
            {
                this.Cmd = new SqlCommand("Proc_Usuario_Update", ConnIn, TranIn);
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
                this.Cmd = new SqlCommand("Proc_Usuario_DeleteAll", this.Conn, this.Tran);
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
                this.Cmd = new SqlCommand("Proc_Usuario_DeleteAll", ConnIn, TranIn);
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
        /// <param name="FieldInfo">Objeto UsuarioFields a ser excluído.</param>
        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Delete( UsuarioFields FieldInfo )
        {
            return Delete(FieldInfo.idUsuario);
        }

        /// <summary> 
        /// Exclui um registro da tabela no banco de dados
        /// </summary>
        /// <param name="Param_idUsuario">int</param>
        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Delete(
                                     int Param_idUsuario)
        {
            try
            {
                this.Conn = new SqlConnection(this.StrConnetionDB);
                this.Conn.Open();
                this.Tran = this.Conn.BeginTransaction();
                this.Cmd = new SqlCommand("Proc_Usuario_Delete", this.Conn, this.Tran);
                this.Cmd.CommandType = CommandType.StoredProcedure;
                this.Cmd.Parameters.Clear();
                this.Cmd.Parameters.Add(new SqlParameter("@Param_idUsuario", SqlDbType.Int)).Value = Param_idUsuario;
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
        /// <param name="FieldInfo">Objeto UsuarioFields a ser excluído.</param>
        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, UsuarioFields FieldInfo )
        {
            return Delete(ConnIn, TranIn, FieldInfo.idUsuario);
        }

        /// <summary> 
        /// Exclui um registro da tabela no banco de dados
        /// </summary>
        /// <param name="Param_idUsuario">int</param>
        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, 
                                     int Param_idUsuario)
        {
            try
            {
                this.Cmd = new SqlCommand("Proc_Usuario_Delete", ConnIn, TranIn);
                this.Cmd.CommandType = CommandType.StoredProcedure;
                this.Cmd.Parameters.Clear();
                this.Cmd.Parameters.Add(new SqlParameter("@Param_idUsuario", SqlDbType.Int)).Value = Param_idUsuario;
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
        /// Retorna um objeto UsuarioFields através da chave primária passada como parâmetro
        /// </summary>
        /// <param name="Param_idUsuario">int</param>
        /// <returns>Objeto UsuarioFields</returns> 
        public UsuarioFields GetItem(
                                     int Param_idUsuario)
        {
            UsuarioFields infoFields = new UsuarioFields();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Usuario_Select", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_idUsuario", SqlDbType.Int)).Value = Param_idUsuario;
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
        /// Seleciona todos os registro da tabela e preenche um ArrayList com o objeto UsuarioFields.
        /// </summary>
        /// <returns>List de objetos UsuarioFields</returns> 
        public List<UsuarioFields> GetAll()
        {
            List<UsuarioFields> arrayInfo = new List<UsuarioFields>();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Usuario_GetAll", this.Conn))
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
                    using (this.Cmd = new SqlCommand("Proc_Usuario_CountAll", this.Conn))
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

        public DataTable GetAllUsersCadastro(string nomeUsuario)
        {
            DataSet dsUsuario = new DataSet();
            try
            {
                SqlConnection Conn = new SqlConnection(this.StrConnetionDB);

                StringBuilder query = new StringBuilder();
                query.Append(" select us.idUsuario,us.Nome,us.Cargo,ua.Nome as UA,us.Situacao,us.Modulo,us.UserName");
                query.Append(" from Usuario us,Ua ua");
                query.Append(" Where us.FkUa = ua.idUA");
                if(!string.IsNullOrEmpty(nomeUsuario))
                query.AppendFormat(" And us.Nome like '%{0}%'",nomeUsuario);
                query.Append(" Order by us.Nome ASC");

                Conn.Open();
                SqlCommand Cmd = new SqlCommand(query.ToString(), Conn);
                Cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(Cmd);
                da.Fill(dsUsuario, "Usuario");

                return dsUsuario.Tables[0];

            }
            catch (SqlException e)
            {
                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.", e.Message);
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

        public DataSet GetAllUsersPrincipal()
        {
            DataSet dsUsuario = new DataSet();
            try
            {
                SqlConnection Conn = new SqlConnection(this.StrConnetionDB);

                StringBuilder query = new StringBuilder();
                query.Append(" select us.idUsuario,us.Nome,us.Cargo,ua.Nome as UA,us.Situacao,us.Modulo,us.UserName");
                query.Append(" from Usuario us,Ua ua");
                query.Append(" Where us.FkUa = UA.idUA");
                query.Append(" Order by us.Nome ASC");

                Conn.Open();
                SqlCommand Cmd = new SqlCommand(query.ToString(), Conn);
                Cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(Cmd);
                da.Fill(dsUsuario, "Usuario");

                return dsUsuario;

            }
            catch (SqlException e)
            {
                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.", e.Message);
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

        private string GetQuery()
        {
            return string.Empty;
        }

        #region Selecionando dados da tabela através do campo "Nome" 

        /// <summary> 
        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Nome.
        /// </summary>
        /// <param name="Param_Nome">string</param>
        /// <returns>UsuarioFields</returns> 
        public UsuarioFields FindByNome(
                               string Param_Nome )
        {
            UsuarioFields infoFields = new UsuarioFields();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindByNome", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Nome", SqlDbType.VarChar, 200)).Value = Param_Nome;
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



        #region Selecionando dados da tabela através do campo "UserName" 

        /// <summary> 
        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo UserName.
        /// </summary>
        /// <param name="Param_UserName">string</param>
        /// <returns>UsuarioFields</returns> 
        public UsuarioFields FindByUserName(
                               string Param_UserName )
        {
            UsuarioFields infoFields = new UsuarioFields();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindByUserName", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_UserName", SqlDbType.VarChar, 100)).Value = Param_UserName;
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



        #region Selecionando dados da tabela através do campo "Password" 

        /// <summary> 
        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Password.
        /// </summary>
        /// <param name="Param_Password">string</param>
        /// <returns>UsuarioFields</returns> 
        public UsuarioFields FindByPassword(
                               string Param_Password )
        {
            UsuarioFields infoFields = new UsuarioFields();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindByPassword", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Password", SqlDbType.VarChar, 100)).Value = Param_Password;
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



        #region Selecionando dados da tabela através do campo "Cargo" 

        /// <summary> 
        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Cargo.
        /// </summary>
        /// <param name="Param_Cargo">string</param>
        /// <returns>UsuarioFields</returns> 
        public UsuarioFields FindByCargo(
                               string Param_Cargo )
        {
            UsuarioFields infoFields = new UsuarioFields();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindByCargo", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Cargo", SqlDbType.VarChar, 50)).Value = Param_Cargo;
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



        #region Selecionando dados da tabela através do campo "Situacao" 

        /// <summary> 
        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Situacao.
        /// </summary>
        /// <param name="Param_Situacao">string</param>
        /// <returns>UsuarioFields</returns> 
        public UsuarioFields FindBySituacao(
                               string Param_Situacao )
        {
            UsuarioFields infoFields = new UsuarioFields();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindBySituacao", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Situacao", SqlDbType.VarChar, 1)).Value = Param_Situacao;
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



        #region Selecionando dados da tabela através do campo "Modulo" 

        /// <summary> 
        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Modulo.
        /// </summary>
        /// <param name="Param_Modulo">string</param>
        /// <returns>UsuarioFields</returns> 
        public UsuarioFields FindByModulo(
                               string Param_Modulo )
        {
            UsuarioFields infoFields = new UsuarioFields();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindByModulo", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Modulo", SqlDbType.VarChar, 1)).Value = Param_Modulo;
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



        #region Selecionando dados da tabela através do campo "FkUa" 

        /// <summary> 
        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo FkUa.
        /// </summary>
        /// <param name="Param_FkUa">int</param>
        /// <returns>UsuarioFields</returns> 
        public UsuarioFields FindByFkUa(
                               int Param_FkUa )
        {
            UsuarioFields infoFields = new UsuarioFields();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindByFkUa", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_FkUa", SqlDbType.Int)).Value = Param_FkUa;
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



        #region Selecionando dados da tabela através do campo "Funcao" 

        /// <summary> 
        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Funcao.
        /// </summary>
        /// <param name="Param_Funcao">string</param>
        /// <returns>UsuarioFields</returns> 
        public UsuarioFields FindByFuncao(
                               string Param_Funcao )
        {
            UsuarioFields infoFields = new UsuarioFields();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindByFuncao", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Funcao", SqlDbType.VarChar, 30)).Value = Param_Funcao;
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
        /// Retorna um objeto UsuarioFields preenchido com os valores dos campos do SqlDataReader
        /// </summary>
        /// <param name="dr">SqlDataReader - Preenche o objeto UsuarioFields </param>
        /// <returns>UsuarioFields</returns>
        private UsuarioFields GetDataFromReader( SqlDataReader dr )
        {
            UsuarioFields infoFields = new UsuarioFields();

            if (!dr.IsDBNull(0))
            { infoFields.idUsuario = dr.GetInt32(0); }
            else
            { infoFields.idUsuario = 0; }



            if (!dr.IsDBNull(1))
            { infoFields.Nome = dr.GetString(1); }
            else
            { infoFields.Nome = string.Empty; }



            if (!dr.IsDBNull(2))
            { infoFields.UserName = dr.GetString(2); }
            else
            { infoFields.UserName = string.Empty; }



            if (!dr.IsDBNull(3))
            { infoFields.Password = dr.GetString(3); }
            else
            { infoFields.Password = string.Empty; }



            if (!dr.IsDBNull(4))
            { infoFields.Cargo = dr.GetString(4); }
            else
            { infoFields.Cargo = string.Empty; }



            if (!dr.IsDBNull(5))
            { infoFields.Situacao = dr.GetString(5); }
            else
            { infoFields.Situacao = string.Empty; }



            if (!dr.IsDBNull(6))
            { infoFields.Modulo = dr.GetString(6); }
            else
            { infoFields.Modulo = string.Empty; }



            if (!dr.IsDBNull(7))
            { infoFields.FkUa = dr.GetInt32(7); }
            else
            { infoFields.FkUa = 0; }



            if (!dr.IsDBNull(8))
            { infoFields.Funcao = dr.GetString(8); }
            else
            { infoFields.Funcao = string.Empty; }


            return infoFields;
        }
        #endregion


        public DataTable GetAllUsersByUA(int idUA)
        {
            DataSet dsUsuarioUA = new DataSet();
            try
            {
                SqlConnection Conn = new SqlConnection(this.StrConnetionDB);

                StringBuilder query = new StringBuilder();
                query.Append(" select u.idUsuario,u.UserName from usuario u , UA ua ");
                query.AppendFormat(" where ua.idUA = {0} And u.FkUa = ua.idUA order by u.Nome ", idUA);

                Conn.Open();
                SqlCommand Cmd = new SqlCommand(query.ToString(), Conn);
                Cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(Cmd);
                da.Fill(dsUsuarioUA, "UsuarioUA");

                return dsUsuarioUA.Tables[0];

            }
            catch (SqlException e)
            {
                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.", e.Message);
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


        #region Função GetAllParameters

        /// <summary> 
        /// Retorna um array de parâmetros com campos para atualização, seleção e inserção no banco de dados
        /// </summary>
        /// <param name="FieldInfo">Objeto UsuarioFields</param>
        /// <param name="Modo">Tipo de oepração a ser executada no banco de dados</param>
        /// <returns>SqlParameter[] - Array de parâmetros</returns> 
        private SqlParameter[] GetAllParameters( UsuarioFields FieldInfo, SQLMode Modo )
        {
            SqlParameter[] Parameters;

            switch (Modo)
            {
                case SQLMode.Add:
                    Parameters = new SqlParameter[9];
                    for (int I = 0; I < Parameters.Length; I++)
                       Parameters[I] = new SqlParameter();
                    //Field idUsuario
                    Parameters[0].SqlDbType = SqlDbType.Int;
                    Parameters[0].Direction = ParameterDirection.Output;
                    Parameters[0].ParameterName = "@Param_idUsuario";
                    Parameters[0].Value = DBNull.Value;

                    break;

                case SQLMode.Update:
                    Parameters = new SqlParameter[9];
                    for (int I = 0; I < Parameters.Length; I++)
                       Parameters[I] = new SqlParameter();
                    //Field idUsuario
                    Parameters[0].SqlDbType = SqlDbType.Int;
                    Parameters[0].ParameterName = "@Param_idUsuario";
                    Parameters[0].Value = FieldInfo.idUsuario;

                    break;

                case SQLMode.SelectORDelete:
                    Parameters = new SqlParameter[1];
                    for (int I = 0; I < Parameters.Length; I++)
                       Parameters[I] = new SqlParameter();
                    //Field idUsuario
                    Parameters[0].SqlDbType = SqlDbType.Int;
                    Parameters[0].ParameterName = "@Param_idUsuario";
                    Parameters[0].Value = FieldInfo.idUsuario;

                    return Parameters;

                default:
                    Parameters = new SqlParameter[9];
                    for (int I = 0; I < Parameters.Length; I++)
                       Parameters[I] = new SqlParameter();
                    break;
            }

            //Field Nome
            Parameters[1].SqlDbType = SqlDbType.VarChar;
            Parameters[1].ParameterName = "@Param_Nome";
            if (( FieldInfo.Nome == null ) || ( FieldInfo.Nome == string.Empty ))
            { Parameters[1].Value = DBNull.Value; }
            else
            { Parameters[1].Value = FieldInfo.Nome; }
            Parameters[1].Size = 200;

            //Field UserName
            Parameters[2].SqlDbType = SqlDbType.VarChar;
            Parameters[2].ParameterName = "@Param_UserName";
            if (( FieldInfo.UserName == null ) || ( FieldInfo.UserName == string.Empty ))
            { Parameters[2].Value = DBNull.Value; }
            else
            { Parameters[2].Value = FieldInfo.UserName; }
            Parameters[2].Size = 100;

            //Field Password
            Parameters[3].SqlDbType = SqlDbType.VarChar;
            Parameters[3].ParameterName = "@Param_Password";
            if (( FieldInfo.Password == null ) || ( FieldInfo.Password == string.Empty ))
            { Parameters[3].Value = DBNull.Value; }
            else
            { Parameters[3].Value = FieldInfo.Password; }
            Parameters[3].Size = 100;

            //Field Cargo
            Parameters[4].SqlDbType = SqlDbType.VarChar;
            Parameters[4].ParameterName = "@Param_Cargo";
            if (( FieldInfo.Cargo == null ) || ( FieldInfo.Cargo == string.Empty ))
            { Parameters[4].Value = DBNull.Value; }
            else
            { Parameters[4].Value = FieldInfo.Cargo; }
            Parameters[4].Size = 50;

            //Field Situacao
            Parameters[5].SqlDbType = SqlDbType.VarChar;
            Parameters[5].ParameterName = "@Param_Situacao";
            if (( FieldInfo.Situacao == null ) || ( FieldInfo.Situacao == string.Empty ))
            { Parameters[5].Value = DBNull.Value; }
            else
            { Parameters[5].Value = FieldInfo.Situacao; }
            Parameters[5].Size = 1;

            //Field Modulo
            Parameters[6].SqlDbType = SqlDbType.VarChar;
            Parameters[6].ParameterName = "@Param_Modulo";
            if (( FieldInfo.Modulo == null ) || ( FieldInfo.Modulo == string.Empty ))
            { Parameters[6].Value = DBNull.Value; }
            else
            { Parameters[6].Value = FieldInfo.Modulo; }
            Parameters[6].Size = 1;

            //Field FkUa
            Parameters[7].SqlDbType = SqlDbType.Int;
            Parameters[7].ParameterName = "@Param_FkUa";
            Parameters[7].Value = FieldInfo.FkUa;

            //Field Funcao
            Parameters[8].SqlDbType = SqlDbType.VarChar;
            Parameters[8].ParameterName = "@Param_Funcao";
            if (( FieldInfo.Funcao == null ) || ( FieldInfo.Funcao == string.Empty ))
            { Parameters[8].Value = DBNull.Value; }
            else
            { Parameters[8].Value = FieldInfo.Funcao; }
            Parameters[8].Size = 30;

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

        ~UsuarioControl() 
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





//Projeto substituído ------------------------
//using System;
//using System.Collections;
//using System.Collections.Generic;
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
//    /// Tabela: Usuario  
//    /// Autor: DAL Creator .net 
//    /// Data de criação: 24/03/2012 19:35:22 
//    /// Descrição: Classe responsável pela perssitência de dados. Utiliza a classe "UsuarioFields". 
//    /// </summary> 
//    public class UsuarioControl : IDisposable 
//    {
//
//        #region String de conexão 
//        private string StrConnetionDB = ConfigurationManager.ConnectionStrings["Data Source=DEKO-PC;Initial Catalog=swgp;User Id=sureg;Password=@sureg2012;"].ToString();
//        #endregion
//
//
//        #region Propriedade que armazena erros de execução 
//        private string _ErrorMessage;
//        public string ErrorMessage { get { return _ErrorMessage; } }
//        #endregion
//
//
//        #region Objetos de conexão 
//        SqlConnection Conn;
//        SqlCommand Cmd;
//        SqlTransaction Tran;
//        #endregion
//
//
//        #region Funcões que retornam Conexões e Transações 
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
//        /// Representa o procedimento que está sendo executado na tabela.
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
//        public UsuarioControl() {}
//
//
//        #region Inserindo dados na tabela 
//
//        /// <summary> 
//        /// Grava/Persiste um novo objeto UsuarioFields no banco de dados
//        /// </summary>
//        /// <param name="FieldInfo">Objeto UsuarioFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
//        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//        public bool Add( ref UsuarioFields FieldInfo )
//        {
//            try
//            {
//                this.Conn = new SqlConnection(this.StrConnetionDB);
//                this.Conn.Open();
//                this.Tran = this.Conn.BeginTransaction();
//                this.Cmd = new SqlCommand("Proc_Usuario_Add", this.Conn, this.Tran);
//                this.Cmd.CommandType = CommandType.StoredProcedure;
//                this.Cmd.Parameters.Clear();
//                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
//                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
//                this.Tran.Commit();
//                FieldInfo.idUsuario = (int)this.Cmd.Parameters["@Param_idUsuario"].Value;
//                return true;
//
//            }
//            catch (SqlException e)
//            {
//                this.Tran.Rollback();
//                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
//        #region Inserindo dados na tabela utilizando conexão e transação externa (compartilhada) 
//
//        /// <summary> 
//        /// Grava/Persiste um novo objeto UsuarioFields no banco de dados
//        /// </summary>
//        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
//        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
//        /// <param name="FieldInfo">Objeto UsuarioFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
//        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//        public bool Add( SqlConnection ConnIn, SqlTransaction TranIn, ref UsuarioFields FieldInfo )
//        {
//            try
//            {
//                this.Cmd = new SqlCommand("Proc_Usuario_Add", ConnIn, TranIn);
//                this.Cmd.CommandType = CommandType.StoredProcedure;
//                this.Cmd.Parameters.Clear();
//                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
//                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
//                FieldInfo.idUsuario = (int)this.Cmd.Parameters["@Param_idUsuario"].Value;
//                return true;
//
//            }
//            catch (SqlException e)
//            {
//                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
//        /// Grava/Persiste as alterações em um objeto UsuarioFields no banco de dados
//        /// </summary>
//        /// <param name="FieldInfo">Objeto UsuarioFields a ser alterado.</param>
//        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//        public bool Update( UsuarioFields FieldInfo )
//        {
//            try
//            {
//                this.Conn = new SqlConnection(this.StrConnetionDB);
//                this.Conn.Open();
//                this.Tran = this.Conn.BeginTransaction();
//                this.Cmd = new SqlCommand("Proc_Usuario_Update", this.Conn, this.Tran);
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
//                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
//        #region Editando dados na tabela utilizando conexão e transação externa (compartilhada) 
//
//        /// <summary> 
//        /// Grava/Persiste as alterações em um objeto UsuarioFields no banco de dados
//        /// </summary>
//        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
//        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
//        /// <param name="FieldInfo">Objeto UsuarioFields a ser alterado.</param>
//        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//        public bool Update( SqlConnection ConnIn, SqlTransaction TranIn, UsuarioFields FieldInfo )
//        {
//            try
//            {
//                this.Cmd = new SqlCommand("Proc_Usuario_Update", ConnIn, TranIn);
//                this.Cmd.CommandType = CommandType.StoredProcedure;
//                this.Cmd.Parameters.Clear();
//                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Update));
//                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar atualizar registro!!");
//                return true;
//            }
//            catch (SqlException e)
//            {
//                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
//                this.Cmd = new SqlCommand("Proc_Usuario_DeleteAll", this.Conn, this.Tran);
//                this.Cmd.CommandType = CommandType.StoredProcedure;
//                this.Cmd.Parameters.Clear();
//                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
//                this.Tran.Commit();
//                return true;
//            }
//            catch (SqlException e)
//            {
//                this.Tran.Rollback();
//                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
//        #region Excluindo todos os dados da tabela utilizando conexão e transação externa (compartilhada)
//
//        /// <summary> 
//        /// Exclui todos os registros da tabela
//        /// </summary>
//        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
//        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
//        /// <returns>"true" = registros excluidos com sucesso, "false" = erro ao tentar excluir registros (consulte a propriedade ErrorMessage para detalhes)</returns> 
//        public bool DeleteAll(SqlConnection ConnIn, SqlTransaction TranIn)
//        {
//            try
//            {
//                this.Cmd = new SqlCommand("Proc_Usuario_DeleteAll", ConnIn, TranIn);
//                this.Cmd.CommandType = CommandType.StoredProcedure;
//                this.Cmd.Parameters.Clear();
//                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
//                return true;
//            }
//            catch (SqlException e)
//            {
//                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
//        /// <param name="FieldInfo">Objeto UsuarioFields a ser excluído.</param>
//        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//        public bool Delete( UsuarioFields FieldInfo )
//        {
//            return Delete(FieldInfo.idUsuario);
//        }
//
//        /// <summary> 
//        /// Exclui um registro da tabela no banco de dados
//        /// </summary>
//        /// <param name="Param_idUsuario">int</param>
//        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//        public bool Delete(
//                                     int Param_idUsuario)
//        {
//            try
//            {
//                this.Conn = new SqlConnection(this.StrConnetionDB);
//                this.Conn.Open();
//                this.Tran = this.Conn.BeginTransaction();
//                this.Cmd = new SqlCommand("Proc_Usuario_Delete", this.Conn, this.Tran);
//                this.Cmd.CommandType = CommandType.StoredProcedure;
//                this.Cmd.Parameters.Clear();
//                this.Cmd.Parameters.Add(new SqlParameter("@Param_idUsuario", SqlDbType.Int)).Value = Param_idUsuario;
//                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
//                this.Tran.Commit();
//                return true;
//            }
//            catch (SqlException e)
//            {
//                this.Tran.Rollback();
//                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
//        #region Excluindo dados da tabela utilizando conexão e transação externa (compartilhada)
//
//        /// <summary> 
//        /// Exclui um registro da tabela no banco de dados
//        /// </summary>
//        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
//        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
//        /// <param name="FieldInfo">Objeto UsuarioFields a ser excluído.</param>
//        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, UsuarioFields FieldInfo )
//        {
//            return Delete(ConnIn, TranIn, FieldInfo.idUsuario);
//        }
//
//        /// <summary> 
//        /// Exclui um registro da tabela no banco de dados
//        /// </summary>
//        /// <param name="Param_idUsuario">int</param>
//        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
//        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
//        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, 
//                                     int Param_idUsuario)
//        {
//            try
//            {
//                this.Cmd = new SqlCommand("Proc_Usuario_Delete", ConnIn, TranIn);
//                this.Cmd.CommandType = CommandType.StoredProcedure;
//                this.Cmd.Parameters.Clear();
//                this.Cmd.Parameters.Add(new SqlParameter("@Param_idUsuario", SqlDbType.Int)).Value = Param_idUsuario;
//                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
//                return true;
//            }
//            catch (SqlException e)
//            {
//                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
//        /// Retorna um objeto UsuarioFields através da chave primária passada como parâmetro
//        /// </summary>
//        /// <param name="Param_idUsuario">int</param>
//        /// <returns>Objeto UsuarioFields</returns> 
//        public UsuarioFields GetItem(
//                                     int Param_idUsuario)
//        {
//            UsuarioFields infoFields = new UsuarioFields();
//            try
//            {
//                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//                {
//                    using (this.Cmd = new SqlCommand("Proc_Usuario_Select", this.Conn))
//                    {
//                        this.Cmd.CommandType = CommandType.StoredProcedure;
//                        this.Cmd.Parameters.Clear();
//                        this.Cmd.Parameters.Add(new SqlParameter("@Param_idUsuario", SqlDbType.Int)).Value = Param_idUsuario;
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
//                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
//        /// Seleciona todos os registro da tabela e preenche um ArrayList com o objeto UsuarioFields.
//        /// </summary>
//        /// <returns>List de objetos UsuarioFields</returns> 
//        public List<UsuarioFields> GetAll()
//        {
//            List<UsuarioFields> arrayInfo = new List<UsuarioFields>();
//            try
//            {
//                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//                {
//                    using (this.Cmd = new SqlCommand("Proc_Usuario_GetAll", this.Conn))
//                    {
//                        this.Cmd.CommandType = CommandType.StoredProcedure;
//                        this.Cmd.Parameters.Clear();
//                        this.Cmd.Connection.Open();
//                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
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
//                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
//                    using (this.Cmd = new SqlCommand("Proc_Usuario_CountAll", this.Conn))
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
//                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
//        #region Selecionando dados da tabela através do campo "Nome" 
//
//        /// <summary> 
//        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Nome.
//        /// </summary>
//        /// <param name="Param_Nome">string</param>
//        /// <returns>UsuarioFields</returns> 
//        public UsuarioFields FindByNome(
//                               string Param_Nome )
//        {
//            UsuarioFields infoFields = new UsuarioFields();
//            try
//            {
//                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//                {
//                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindByNome", this.Conn))
//                    {
//                        this.Cmd.CommandType = CommandType.StoredProcedure;
//                        this.Cmd.Parameters.Clear();
//                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Nome", SqlDbType.VarChar, 200)).Value = Param_Nome;
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
//                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
//        #region Selecionando dados da tabela através do campo "UserName" 
//
//        /// <summary> 
//        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo UserName.
//        /// </summary>
//        /// <param name="Param_UserName">string</param>
//        /// <returns>UsuarioFields</returns> 
//        public UsuarioFields FindByUserName(
//                               string Param_UserName )
//        {
//            UsuarioFields infoFields = new UsuarioFields();
//            try
//            {
//                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//                {
//                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindByUserName", this.Conn))
//                    {
//                        this.Cmd.CommandType = CommandType.StoredProcedure;
//                        this.Cmd.Parameters.Clear();
//                        this.Cmd.Parameters.Add(new SqlParameter("@Param_UserName", SqlDbType.VarChar, 100)).Value = Param_UserName;
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
//                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
//        #region Selecionando dados da tabela através do campo "Password" 
//
//        /// <summary> 
//        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Password.
//        /// </summary>
//        /// <param name="Param_Password">string</param>
//        /// <returns>UsuarioFields</returns> 
//        public UsuarioFields FindByPassword(
//                               string Param_Password )
//        {
//            UsuarioFields infoFields = new UsuarioFields();
//            try
//            {
//                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//                {
//                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindByPassword", this.Conn))
//                    {
//                        this.Cmd.CommandType = CommandType.StoredProcedure;
//                        this.Cmd.Parameters.Clear();
//                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Password", SqlDbType.VarChar, 100)).Value = Param_Password;
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
//                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
//        #region Selecionando dados da tabela através do campo "Cargo" 
//
//        /// <summary> 
//        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Cargo.
//        /// </summary>
//        /// <param name="Param_Cargo">string</param>
//        /// <returns>UsuarioFields</returns> 
//        public UsuarioFields FindByCargo(
//                               string Param_Cargo )
//        {
//            UsuarioFields infoFields = new UsuarioFields();
//            try
//            {
//                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//                {
//                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindByCargo", this.Conn))
//                    {
//                        this.Cmd.CommandType = CommandType.StoredProcedure;
//                        this.Cmd.Parameters.Clear();
//                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Cargo", SqlDbType.VarChar, 50)).Value = Param_Cargo;
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
//                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
//        #region Selecionando dados da tabela através do campo "Situacao" 
//
//        /// <summary> 
//        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Situacao.
//        /// </summary>
//        /// <param name="Param_Situacao">string</param>
//        /// <returns>UsuarioFields</returns> 
//        public UsuarioFields FindBySituacao(
//                               string Param_Situacao )
//        {
//            UsuarioFields infoFields = new UsuarioFields();
//            try
//            {
//                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//                {
//                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindBySituacao", this.Conn))
//                    {
//                        this.Cmd.CommandType = CommandType.StoredProcedure;
//                        this.Cmd.Parameters.Clear();
//                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Situacao", SqlDbType.VarChar, 1)).Value = Param_Situacao;
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
//                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
//        #region Selecionando dados da tabela através do campo "Modulo" 
//
//        /// <summary> 
//        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Modulo.
//        /// </summary>
//        /// <param name="Param_Modulo">string</param>
//        /// <returns>UsuarioFields</returns> 
//        public UsuarioFields FindByModulo(
//                               string Param_Modulo )
//        {
//            UsuarioFields infoFields = new UsuarioFields();
//            try
//            {
//                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//                {
//                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindByModulo", this.Conn))
//                    {
//                        this.Cmd.CommandType = CommandType.StoredProcedure;
//                        this.Cmd.Parameters.Clear();
//                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Modulo", SqlDbType.VarChar, 1)).Value = Param_Modulo;
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
//                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
//        #region Selecionando dados da tabela através do campo "FkUa" 
//
//        /// <summary> 
//        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo FkUa.
//        /// </summary>
//        /// <param name="Param_FkUa">int</param>
//        /// <returns>UsuarioFields</returns> 
//        public UsuarioFields FindByFkUa(
//                               int Param_FkUa )
//        {
//            UsuarioFields infoFields = new UsuarioFields();
//            try
//            {
//                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//                {
//                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindByFkUa", this.Conn))
//                    {
//                        this.Cmd.CommandType = CommandType.StoredProcedure;
//                        this.Cmd.Parameters.Clear();
//                        this.Cmd.Parameters.Add(new SqlParameter("@Param_FkUa", SqlDbType.Int)).Value = Param_FkUa;
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
//                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
//        #region Selecionando dados da tabela através do campo "Funcao" 
//
//        /// <summary> 
//        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Funcao.
//        /// </summary>
//        /// <param name="Param_Funcao">string</param>
//        /// <returns>UsuarioFields</returns> 
//        public UsuarioFields FindByFuncao(
//                               string Param_Funcao )
//        {
//            UsuarioFields infoFields = new UsuarioFields();
//            try
//            {
//                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//                {
//                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindByFuncao", this.Conn))
//                    {
//                        this.Cmd.CommandType = CommandType.StoredProcedure;
//                        this.Cmd.Parameters.Clear();
//                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Funcao", SqlDbType.VarChar, 30)).Value = Param_Funcao;
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
//                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
//        #region Função GetDataFromReader
//
//        /// <summary> 
//        /// Retorna um objeto UsuarioFields preenchido com os valores dos campos do SqlDataReader
//        /// </summary>
//        /// <param name="dr">SqlDataReader - Preenche o objeto UsuarioFields </param>
//        /// <returns>UsuarioFields</returns>
//        private UsuarioFields GetDataFromReader( SqlDataReader dr )
//        {
//            UsuarioFields infoFields = new UsuarioFields();
//
//            if (!dr.IsDBNull(0))
//            { infoFields.idUsuario = dr.GetInt32(0); }
//            else
//            { infoFields.idUsuario = 0; }
//
//
//
//            if (!dr.IsDBNull(1))
//            { infoFields.Nome = dr.GetString(1); }
//            else
//            { infoFields.Nome = string.Empty; }
//
//
//
//            if (!dr.IsDBNull(2))
//            { infoFields.UserName = dr.GetString(2); }
//            else
//            { infoFields.UserName = string.Empty; }
//
//
//
//            if (!dr.IsDBNull(3))
//            { infoFields.Password = dr.GetString(3); }
//            else
//            { infoFields.Password = string.Empty; }
//
//
//
//            if (!dr.IsDBNull(4))
//            { infoFields.Cargo = dr.GetString(4); }
//            else
//            { infoFields.Cargo = string.Empty; }
//
//
//
//            if (!dr.IsDBNull(5))
//            { infoFields.Situacao = dr.GetString(5); }
//            else
//            { infoFields.Situacao = string.Empty; }
//
//
//
//            if (!dr.IsDBNull(6))
//            { infoFields.Modulo = dr.GetString(6); }
//            else
//            { infoFields.Modulo = string.Empty; }
//
//
//
//            if (!dr.IsDBNull(7))
//            { infoFields.FkUa = dr.GetInt32(7); }
//            else
//            { infoFields.FkUa = 0; }
//
//
//
//            if (!dr.IsDBNull(8))
//            { infoFields.Funcao = dr.GetString(8); }
//            else
//            { infoFields.Funcao = string.Empty; }
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
//        #region Função GetAllParameters
//
//        /// <summary> 
//        /// Retorna um array de parâmetros com campos para atualização, seleção e inserção no banco de dados
//        /// </summary>
//        /// <param name="FieldInfo">Objeto UsuarioFields</param>
//        /// <param name="Modo">Tipo de oepração a ser executada no banco de dados</param>
//        /// <returns>SqlParameter[] - Array de parâmetros</returns> 
//        private SqlParameter[] GetAllParameters( UsuarioFields FieldInfo, SQLMode Modo )
//        {
//            SqlParameter[] Parameters;
//
//            switch (Modo)
//            {
//                case SQLMode.Add:
//                    Parameters = new SqlParameter[9];
//                    for (int I = 0; I < Parameters.Length; I++)
//                       Parameters[I] = new SqlParameter();
//                    //Field idUsuario
//                    Parameters[0].SqlDbType = SqlDbType.Int;
//                    Parameters[0].Direction = ParameterDirection.Output;
//                    Parameters[0].ParameterName = "@Param_idUsuario";
//                    Parameters[0].Value = DBNull.Value;
//
//                    break;
//
//                case SQLMode.Update:
//                    Parameters = new SqlParameter[9];
//                    for (int I = 0; I < Parameters.Length; I++)
//                       Parameters[I] = new SqlParameter();
//                    //Field idUsuario
//                    Parameters[0].SqlDbType = SqlDbType.Int;
//                    Parameters[0].ParameterName = "@Param_idUsuario";
//                    Parameters[0].Value = FieldInfo.idUsuario;
//
//                    break;
//
//                case SQLMode.SelectORDelete:
//                    Parameters = new SqlParameter[1];
//                    for (int I = 0; I < Parameters.Length; I++)
//                       Parameters[I] = new SqlParameter();
//                    //Field idUsuario
//                    Parameters[0].SqlDbType = SqlDbType.Int;
//                    Parameters[0].ParameterName = "@Param_idUsuario";
//                    Parameters[0].Value = FieldInfo.idUsuario;
//
//                    return Parameters;
//
//                default:
//                    Parameters = new SqlParameter[9];
//                    for (int I = 0; I < Parameters.Length; I++)
//                       Parameters[I] = new SqlParameter();
//                    break;
//            }
//
//            //Field Nome
//            Parameters[1].SqlDbType = SqlDbType.VarChar;
//            Parameters[1].ParameterName = "@Param_Nome";
//            if (( FieldInfo.Nome == null ) || ( FieldInfo.Nome == string.Empty ))
//            { Parameters[1].Value = DBNull.Value; }
//            else
//            { Parameters[1].Value = FieldInfo.Nome; }
//            Parameters[1].Size = 200;
//
//            //Field UserName
//            Parameters[2].SqlDbType = SqlDbType.VarChar;
//            Parameters[2].ParameterName = "@Param_UserName";
//            if (( FieldInfo.UserName == null ) || ( FieldInfo.UserName == string.Empty ))
//            { Parameters[2].Value = DBNull.Value; }
//            else
//            { Parameters[2].Value = FieldInfo.UserName; }
//            Parameters[2].Size = 100;
//
//            //Field Password
//            Parameters[3].SqlDbType = SqlDbType.VarChar;
//            Parameters[3].ParameterName = "@Param_Password";
//            if (( FieldInfo.Password == null ) || ( FieldInfo.Password == string.Empty ))
//            { Parameters[3].Value = DBNull.Value; }
//            else
//            { Parameters[3].Value = FieldInfo.Password; }
//            Parameters[3].Size = 100;
//
//            //Field Cargo
//            Parameters[4].SqlDbType = SqlDbType.VarChar;
//            Parameters[4].ParameterName = "@Param_Cargo";
//            if (( FieldInfo.Cargo == null ) || ( FieldInfo.Cargo == string.Empty ))
//            { Parameters[4].Value = DBNull.Value; }
//            else
//            { Parameters[4].Value = FieldInfo.Cargo; }
//            Parameters[4].Size = 50;
//
//            //Field Situacao
//            Parameters[5].SqlDbType = SqlDbType.VarChar;
//            Parameters[5].ParameterName = "@Param_Situacao";
//            if (( FieldInfo.Situacao == null ) || ( FieldInfo.Situacao == string.Empty ))
//            { Parameters[5].Value = DBNull.Value; }
//            else
//            { Parameters[5].Value = FieldInfo.Situacao; }
//            Parameters[5].Size = 1;
//
//            //Field Modulo
//            Parameters[6].SqlDbType = SqlDbType.VarChar;
//            Parameters[6].ParameterName = "@Param_Modulo";
//            if (( FieldInfo.Modulo == null ) || ( FieldInfo.Modulo == string.Empty ))
//            { Parameters[6].Value = DBNull.Value; }
//            else
//            { Parameters[6].Value = FieldInfo.Modulo; }
//            Parameters[6].Size = 1;
//
//            //Field FkUa
//            Parameters[7].SqlDbType = SqlDbType.Int;
//            Parameters[7].ParameterName = "@Param_FkUa";
//            Parameters[7].Value = FieldInfo.FkUa;
//
//            //Field Funcao
//            Parameters[8].SqlDbType = SqlDbType.VarChar;
//            Parameters[8].ParameterName = "@Param_Funcao";
//            if (( FieldInfo.Funcao == null ) || ( FieldInfo.Funcao == string.Empty ))
//            { Parameters[8].Value = DBNull.Value; }
//            else
//            { Parameters[8].Value = FieldInfo.Funcao; }
//            Parameters[8].Size = 30;
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
//        ~UsuarioControl() 
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
////Projeto substituído ------------------------
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
////    /// Tabela: Usuario  
////    /// Autor: DAL Creator .net 
////    /// Data de criação: 19/03/2012 23:02:06 
////    /// Descrição: Classe responsável pela perssitência de dados. Utiliza a classe "UsuarioFields". 
////    /// </summary> 
////    public class UsuarioControl : IDisposable 
////    {
////
////        #region String de conexão 
////        private string StrConnetionDB = ConfigurationManager.ConnectionStrings["StringConn"].ToString();
////        #endregion
////
////
////        #region Propriedade que armazena erros de execução 
////        private string _ErrorMessage;
////        public string ErrorMessage { get { return _ErrorMessage; } }
////        #endregion
////
////
////        #region Objetos de conexão 
////        SqlConnection Conn;
////        SqlCommand Cmd;
////        SqlTransaction Tran;
////        #endregion
////
////
////        #region Funcões que retornam Conexões e Transações 
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
////        /// Representa o procedimento que está sendo executado na tabela.
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
////        public UsuarioControl() {}
////
////
////        #region Inserindo dados na tabela 
////
////        /// <summary> 
////        /// Grava/Persiste um novo objeto UsuarioFields no banco de dados
////        /// </summary>
////        /// <param name="FieldInfo">Objeto UsuarioFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
////        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////        public bool Add( ref UsuarioFields FieldInfo )
////        {
////            try
////            {
////                this.Conn = new SqlConnection(this.StrConnetionDB);
////                this.Conn.Open();
////                this.Tran = this.Conn.BeginTransaction();
////                this.Cmd = new SqlCommand("Proc_Usuario_Add", this.Conn, this.Tran);
////                this.Cmd.CommandType = CommandType.StoredProcedure;
////                this.Cmd.Parameters.Clear();
////                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
////                this.Tran.Commit();
////                FieldInfo.idUsuario = (int)this.Cmd.Parameters["@Param_idUsuario"].Value;
////                return true;
////
////            }
////            catch (SqlException e)
////            {
////                this.Tran.Rollback();
////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
////        #region Inserindo dados na tabela utilizando conexão e transação externa (compartilhada) 
////
////        /// <summary> 
////        /// Grava/Persiste um novo objeto UsuarioFields no banco de dados
////        /// </summary>
////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
////        /// <param name="FieldInfo">Objeto UsuarioFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
////        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////        public bool Add( SqlConnection ConnIn, SqlTransaction TranIn, ref UsuarioFields FieldInfo )
////        {
////            try
////            {
////                this.Cmd = new SqlCommand("Proc_Usuario_Add", ConnIn, TranIn);
////                this.Cmd.CommandType = CommandType.StoredProcedure;
////                this.Cmd.Parameters.Clear();
////                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
////                FieldInfo.idUsuario = (int)this.Cmd.Parameters["@Param_idUsuario"].Value;
////                return true;
////
////            }
////            catch (SqlException e)
////            {
////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
////        /// Grava/Persiste as alterações em um objeto UsuarioFields no banco de dados
////        /// </summary>
////        /// <param name="FieldInfo">Objeto UsuarioFields a ser alterado.</param>
////        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////        public bool Update( UsuarioFields FieldInfo )
////        {
////            try
////            {
////                this.Conn = new SqlConnection(this.StrConnetionDB);
////                this.Conn.Open();
////                this.Tran = this.Conn.BeginTransaction();
////                this.Cmd = new SqlCommand("Proc_Usuario_Update", this.Conn, this.Tran);
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
////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
////        #region Editando dados na tabela utilizando conexão e transação externa (compartilhada) 
////
////        /// <summary> 
////        /// Grava/Persiste as alterações em um objeto UsuarioFields no banco de dados
////        /// </summary>
////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
////        /// <param name="FieldInfo">Objeto UsuarioFields a ser alterado.</param>
////        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////        public bool Update( SqlConnection ConnIn, SqlTransaction TranIn, UsuarioFields FieldInfo )
////        {
////            try
////            {
////                this.Cmd = new SqlCommand("Proc_Usuario_Update", ConnIn, TranIn);
////                this.Cmd.CommandType = CommandType.StoredProcedure;
////                this.Cmd.Parameters.Clear();
////                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Update));
////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar atualizar registro!!");
////                return true;
////            }
////            catch (SqlException e)
////            {
////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
////                this.Cmd = new SqlCommand("Proc_Usuario_DeleteAll", this.Conn, this.Tran);
////                this.Cmd.CommandType = CommandType.StoredProcedure;
////                this.Cmd.Parameters.Clear();
////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
////                this.Tran.Commit();
////                return true;
////            }
////            catch (SqlException e)
////            {
////                this.Tran.Rollback();
////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
////        #region Excluindo todos os dados da tabela utilizando conexão e transação externa (compartilhada)
////
////        /// <summary> 
////        /// Exclui todos os registros da tabela
////        /// </summary>
////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
////        /// <returns>"true" = registros excluidos com sucesso, "false" = erro ao tentar excluir registros (consulte a propriedade ErrorMessage para detalhes)</returns> 
////        public bool DeleteAll(SqlConnection ConnIn, SqlTransaction TranIn)
////        {
////            try
////            {
////                this.Cmd = new SqlCommand("Proc_Usuario_DeleteAll", ConnIn, TranIn);
////                this.Cmd.CommandType = CommandType.StoredProcedure;
////                this.Cmd.Parameters.Clear();
////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
////                return true;
////            }
////            catch (SqlException e)
////            {
////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
////        /// <param name="FieldInfo">Objeto UsuarioFields a ser excluído.</param>
////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////        public bool Delete( UsuarioFields FieldInfo )
////        {
////            return Delete(FieldInfo.idUsuario);
////        }
////
////        /// <summary> 
////        /// Exclui um registro da tabela no banco de dados
////        /// </summary>
////        /// <param name="Param_idUsuario">int</param>
////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////        public bool Delete(
////                                     int Param_idUsuario)
////        {
////            try
////            {
////                this.Conn = new SqlConnection(this.StrConnetionDB);
////                this.Conn.Open();
////                this.Tran = this.Conn.BeginTransaction();
////                this.Cmd = new SqlCommand("Proc_Usuario_Delete", this.Conn, this.Tran);
////                this.Cmd.CommandType = CommandType.StoredProcedure;
////                this.Cmd.Parameters.Clear();
////                this.Cmd.Parameters.Add(new SqlParameter("@Param_idUsuario", SqlDbType.Int)).Value = Param_idUsuario;
////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
////                this.Tran.Commit();
////                return true;
////            }
////            catch (SqlException e)
////            {
////                this.Tran.Rollback();
////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
////        #region Excluindo dados da tabela utilizando conexão e transação externa (compartilhada)
////
////        /// <summary> 
////        /// Exclui um registro da tabela no banco de dados
////        /// </summary>
////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
////        /// <param name="FieldInfo">Objeto UsuarioFields a ser excluído.</param>
////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, UsuarioFields FieldInfo )
////        {
////            return Delete(ConnIn, TranIn, FieldInfo.idUsuario);
////        }
////
////        /// <summary> 
////        /// Exclui um registro da tabela no banco de dados
////        /// </summary>
////        /// <param name="Param_idUsuario">int</param>
////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, 
////                                     int Param_idUsuario)
////        {
////            try
////            {
////                this.Cmd = new SqlCommand("Proc_Usuario_Delete", ConnIn, TranIn);
////                this.Cmd.CommandType = CommandType.StoredProcedure;
////                this.Cmd.Parameters.Clear();
////                this.Cmd.Parameters.Add(new SqlParameter("@Param_idUsuario", SqlDbType.Int)).Value = Param_idUsuario;
////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
////                return true;
////            }
////            catch (SqlException e)
////            {
////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
////        /// Retorna um objeto UsuarioFields através da chave primária passada como parâmetro
////        /// </summary>
////        /// <param name="Param_idUsuario">int</param>
////        /// <returns>Objeto UsuarioFields</returns> 
////        public UsuarioFields GetItem(
////                                     int Param_idUsuario)
////        {
////            UsuarioFields infoFields = new UsuarioFields();
////            try
////            {
////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////                {
////                    using (this.Cmd = new SqlCommand("Proc_Usuario_Select", this.Conn))
////                    {
////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////                        this.Cmd.Parameters.Clear();
////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_idUsuario", SqlDbType.Int)).Value = Param_idUsuario;
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
////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
////        /// Seleciona todos os registro da tabela e preenche um ArrayList com o objeto UsuarioFields.
////        /// </summary>
////        /// <returns>List de objetos UsuarioFields</returns> 
////        public List<UsuarioFields> GetAll()
////        {
////            List<UsuarioFields> arrayInfo = new List<UsuarioFields>();
////            try
////            {
////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////                {
////                    using (this.Cmd = new SqlCommand("Proc_Usuario_GetAll", this.Conn))
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
////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
////                    using (this.Cmd = new SqlCommand("Proc_Usuario_CountAll", this.Conn))
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
////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
////        #region Selecionando dados da tabela através do campo "Nome" 
////
////        /// <summary> 
////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Nome.
////        /// </summary>
////        /// <param name="Param_Nome">string</param>
////        /// <returns>UsuarioFields</returns> 
////        public UsuarioFields FindByNome(
////                               string Param_Nome )
////        {
////            UsuarioFields infoFields = new UsuarioFields();
////            try
////            {
////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////                {
////                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindByNome", this.Conn))
////                    {
////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////                        this.Cmd.Parameters.Clear();
////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Nome", SqlDbType.VarChar, 200)).Value = Param_Nome;
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
////                }
////
////                return infoFields;
////
////            }
////            catch (SqlException e)
////            {
////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
////        #region Selecionando dados da tabela através do campo "UserName" 
////
////        /// <summary> 
////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo UserName.
////        /// </summary>
////        /// <param name="Param_UserName">string</param>
////        /// <returns>UsuarioFields</returns> 
////        public UsuarioFields FindByUserName(
////                               string Param_UserName )
////        {
////            UsuarioFields infoFields = new UsuarioFields();
////            try
////            {
////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////                {
////                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindByUserName", this.Conn))
////                    {
////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////                        this.Cmd.Parameters.Clear();
////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_UserName", SqlDbType.VarChar, 100)).Value = Param_UserName;
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
////                }
////
////                return infoFields;
////
////            }
////            catch (SqlException e)
////            {
////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
////        #region Selecionando dados da tabela através do campo "Password" 
////
////        /// <summary> 
////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Password.
////        /// </summary>
////        /// <param name="Param_Password">string</param>
////        /// <returns>UsuarioFields</returns> 
////        public UsuarioFields FindByPassword(
////                               string Param_Password )
////        {
////            UsuarioFields infoFields = new UsuarioFields();
////            try
////            {
////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////                {
////                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindByPassword", this.Conn))
////                    {
////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////                        this.Cmd.Parameters.Clear();
////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Password", SqlDbType.VarChar, 100)).Value = Param_Password;
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
////                }
////
////                return infoFields;
////
////            }
////            catch (SqlException e)
////            {
////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
////        #region Selecionando dados da tabela através do campo "Cargo" 
////
////        /// <summary> 
////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Cargo.
////        /// </summary>
////        /// <param name="Param_Cargo">string</param>
////        /// <returns>UsuarioFields</returns> 
////        public UsuarioFields FindByCargo(
////                               string Param_Cargo )
////        {
////            UsuarioFields infoFields = new UsuarioFields();
////            try
////            {
////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////                {
////                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindByCargo", this.Conn))
////                    {
////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////                        this.Cmd.Parameters.Clear();
////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Cargo", SqlDbType.VarChar, 50)).Value = Param_Cargo;
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
////                }
////
////                return infoFields;
////
////            }
////            catch (SqlException e)
////            {
////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
////        #region Selecionando dados da tabela através do campo "Situacao" 
////
////        /// <summary> 
////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Situacao.
////        /// </summary>
////        /// <param name="Param_Situacao">string</param>
////        /// <returns>UsuarioFields</returns> 
////        public UsuarioFields FindBySituacao(
////                               string Param_Situacao )
////        {
////            UsuarioFields infoFields = new UsuarioFields();
////            try
////            {
////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////                {
////                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindBySituacao", this.Conn))
////                    {
////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////                        this.Cmd.Parameters.Clear();
////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Situacao", SqlDbType.VarChar, 1)).Value = Param_Situacao;
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
////                }
////
////                return infoFields;
////
////            }
////            catch (SqlException e)
////            {
////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
////        #region Selecionando dados da tabela através do campo "Modulo" 
////
////        /// <summary> 
////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Modulo.
////        /// </summary>
////        /// <param name="Param_Modulo">string</param>
////        /// <returns>UsuarioFields</returns> 
////        public UsuarioFields FindByModulo(
////                               string Param_Modulo )
////        {
////            UsuarioFields infoFields = new UsuarioFields();
////            try
////            {
////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////                {
////                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindByModulo", this.Conn))
////                    {
////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////                        this.Cmd.Parameters.Clear();
////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Modulo", SqlDbType.VarChar, 1)).Value = Param_Modulo;
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
////                }
////
////                return infoFields;
////
////            }
////            catch (SqlException e)
////            {
////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
////        #region Selecionando dados da tabela através do campo "FkUa" 
////
////        /// <summary> 
////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo FkUa.
////        /// </summary>
////        /// <param name="Param_FkUa">int</param>
////        /// <returns>UsuarioFields</returns> 
////        public UsuarioFields FindByFkUa(
////                               int Param_FkUa )
////        {
////            UsuarioFields infoFields = new UsuarioFields();
////            try
////            {
////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////                {
////                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindByFkUa", this.Conn))
////                    {
////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////                        this.Cmd.Parameters.Clear();
////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_FkUa", SqlDbType.Int)).Value = Param_FkUa;
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
////                }
////
////                return infoFields;
////
////            }
////            catch (SqlException e)
////            {
////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
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
////        #region Função GetDataFromReader
////
////        /// <summary> 
////        /// Retorna um objeto UsuarioFields preenchido com os valores dos campos do SqlDataReader
////        /// </summary>
////        /// <param name="dr">SqlDataReader - Preenche o objeto UsuarioFields </param>
////        /// <returns>UsuarioFields</returns>
////        private UsuarioFields GetDataFromReader( SqlDataReader dr )
////        {
////            UsuarioFields infoFields = new UsuarioFields();
////
////            if (!dr.IsDBNull(0))
////            { infoFields.idUsuario = dr.GetInt32(0); }
////            else
////            { infoFields.idUsuario = 0; }
////
////
////
////            if (!dr.IsDBNull(1))
////            { infoFields.Nome = dr.GetString(1); }
////            else
////            { infoFields.Nome = string.Empty; }
////
////
////
////            if (!dr.IsDBNull(2))
////            { infoFields.UserName = dr.GetString(2); }
////            else
////            { infoFields.UserName = string.Empty; }
////
////
////
////            if (!dr.IsDBNull(3))
////            { infoFields.Password = dr.GetString(3); }
////            else
////            { infoFields.Password = string.Empty; }
////
////
////
////            if (!dr.IsDBNull(4))
////            { infoFields.Cargo = dr.GetString(4); }
////            else
////            { infoFields.Cargo = string.Empty; }
////
////
////
////            if (!dr.IsDBNull(5))
////            { infoFields.Situacao = dr.GetString(5); }
////            else
////            { infoFields.Situacao = string.Empty; }
////
////
////
////            if (!dr.IsDBNull(6))
////            { infoFields.Modulo = dr.GetString(6); }
////            else
////            { infoFields.Modulo = string.Empty; }
////
////
////
////            if (!dr.IsDBNull(7))
////            { infoFields.FkUa = dr.GetInt32(7); }
////            else
////            { infoFields.FkUa = 0; }
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
////        #region Função GetAllParameters
////
////        /// <summary> 
////        /// Retorna um array de parâmetros com campos para atualização, seleção e inserção no banco de dados
////        /// </summary>
////        /// <param name="FieldInfo">Objeto UsuarioFields</param>
////        /// <param name="Modo">Tipo de oepração a ser executada no banco de dados</param>
////        /// <returns>SqlParameter[] - Array de parâmetros</returns> 
////        private SqlParameter[] GetAllParameters( UsuarioFields FieldInfo, SQLMode Modo )
////        {
////            SqlParameter[] Parameters;
////
////            switch (Modo)
////            {
////                case SQLMode.Add:
////                    Parameters = new SqlParameter[8];
////                    for (int I = 0; I < Parameters.Length; I++)
////                       Parameters[I] = new SqlParameter();
////                    //Field idUsuario
////                    Parameters[0].SqlDbType = SqlDbType.Int;
////                    Parameters[0].Direction = ParameterDirection.Output;
////                    Parameters[0].ParameterName = "@Param_idUsuario";
////                    Parameters[0].Value = DBNull.Value;
////
////                    break;
////
////                case SQLMode.Update:
////                    Parameters = new SqlParameter[8];
////                    for (int I = 0; I < Parameters.Length; I++)
////                       Parameters[I] = new SqlParameter();
////                    //Field idUsuario
////                    Parameters[0].SqlDbType = SqlDbType.Int;
////                    Parameters[0].ParameterName = "@Param_idUsuario";
////                    Parameters[0].Value = FieldInfo.idUsuario;
////
////                    break;
////
////                case SQLMode.SelectORDelete:
////                    Parameters = new SqlParameter[1];
////                    for (int I = 0; I < Parameters.Length; I++)
////                       Parameters[I] = new SqlParameter();
////                    //Field idUsuario
////                    Parameters[0].SqlDbType = SqlDbType.Int;
////                    Parameters[0].ParameterName = "@Param_idUsuario";
////                    Parameters[0].Value = FieldInfo.idUsuario;
////
////                    return Parameters;
////
////                default:
////                    Parameters = new SqlParameter[8];
////                    for (int I = 0; I < Parameters.Length; I++)
////                       Parameters[I] = new SqlParameter();
////                    break;
////            }
////
////            //Field Nome
////            Parameters[1].SqlDbType = SqlDbType.VarChar;
////            Parameters[1].ParameterName = "@Param_Nome";
////            if (( FieldInfo.Nome == null ) || ( FieldInfo.Nome == string.Empty ))
////            { Parameters[1].Value = DBNull.Value; }
////            else
////            { Parameters[1].Value = FieldInfo.Nome; }
////            Parameters[1].Size = 200;
////
////            //Field UserName
////            Parameters[2].SqlDbType = SqlDbType.VarChar;
////            Parameters[2].ParameterName = "@Param_UserName";
////            if (( FieldInfo.UserName == null ) || ( FieldInfo.UserName == string.Empty ))
////            { Parameters[2].Value = DBNull.Value; }
////            else
////            { Parameters[2].Value = FieldInfo.UserName; }
////            Parameters[2].Size = 100;
////
////            //Field Password
////            Parameters[3].SqlDbType = SqlDbType.VarChar;
////            Parameters[3].ParameterName = "@Param_Password";
////            if (( FieldInfo.Password == null ) || ( FieldInfo.Password == string.Empty ))
////            { Parameters[3].Value = DBNull.Value; }
////            else
////            { Parameters[3].Value = FieldInfo.Password; }
////            Parameters[3].Size = 100;
////
////            //Field Cargo
////            Parameters[4].SqlDbType = SqlDbType.VarChar;
////            Parameters[4].ParameterName = "@Param_Cargo";
////            if (( FieldInfo.Cargo == null ) || ( FieldInfo.Cargo == string.Empty ))
////            { Parameters[4].Value = DBNull.Value; }
////            else
////            { Parameters[4].Value = FieldInfo.Cargo; }
////            Parameters[4].Size = 50;
////
////            //Field Situacao
////            Parameters[5].SqlDbType = SqlDbType.VarChar;
////            Parameters[5].ParameterName = "@Param_Situacao";
////            if (( FieldInfo.Situacao == null ) || ( FieldInfo.Situacao == string.Empty ))
////            { Parameters[5].Value = DBNull.Value; }
////            else
////            { Parameters[5].Value = FieldInfo.Situacao; }
////            Parameters[5].Size = 1;
////
////            //Field Modulo
////            Parameters[6].SqlDbType = SqlDbType.VarChar;
////            Parameters[6].ParameterName = "@Param_Modulo";
////            if (( FieldInfo.Modulo == null ) || ( FieldInfo.Modulo == string.Empty ))
////            { Parameters[6].Value = DBNull.Value; }
////            else
////            { Parameters[6].Value = FieldInfo.Modulo; }
////            Parameters[6].Size = 1;
////
////            //Field FkUa
////            Parameters[7].SqlDbType = SqlDbType.Int;
////            Parameters[7].ParameterName = "@Param_FkUa";
////            Parameters[7].Value = FieldInfo.FkUa;
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
////        ~UsuarioControl() 
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
////
////
////
////
////
//////Projeto substituído ------------------------
//////using System;
//////using System.Collections;
//////using System.Data;
//////using System.Data.SqlClient;
//////using System.Configuration;
//////using System.Text;
//////
//////namespace SWGPgen
//////{
//////
//////
//////    /// <summary> 
//////    /// Tabela: Usuario  
//////    /// Autor: DAL Creator .net 
//////    /// Data de criação: 19/03/2012 22:46:52 
//////    /// Descrição: Classe responsável pela perssitência de dados. Utiliza a classe "UsuarioFields". 
//////    /// </summary> 
//////    public class UsuarioControl : IDisposable 
//////    {
//////
//////        #region String de conexão 
//////        private string StrConnetionDB = ConfigurationManager.ConnectionStrings["Data Source=DEKO-PC;Initial Catalog=swgp;User Id=sureg;Password=@sureg2012;"].ToString();
//////        #endregion
//////
//////
//////        #region Propriedade que armazena erros de execução 
//////        private string _ErrorMessage;
//////        public string ErrorMessage { get { return _ErrorMessage; } }
//////        #endregion
//////
//////
//////        #region Objetos de conexão 
//////        SqlConnection Conn;
//////        SqlCommand Cmd;
//////        SqlTransaction Tran;
//////        #endregion
//////
//////
//////        #region Funcões que retornam Conexões e Transações 
//////
//////        public SqlTransaction GetNewTransaction(SqlConnection connIn)
//////        {
//////            if (connIn.State != ConnectionState.Open)
//////                connIn.Open();
//////            SqlTransaction TranOut = connIn.BeginTransaction();
//////            return TranOut;
//////        }
//////
//////        public SqlConnection GetNewConnection()
//////        {
//////            return GetNewConnection(this.StrConnetionDB);
//////        }
//////
//////        public SqlConnection GetNewConnection(string StringConnection)
//////        {
//////            SqlConnection connOut = new SqlConnection(StringConnection);
//////            return connOut;
//////        }
//////
//////        #endregion
//////
//////
//////        #region enum SQLMode 
//////        /// <summary>   
//////        /// Representa o procedimento que está sendo executado na tabela.
//////        /// </summary>
//////        public enum SQLMode
//////        {                     
//////            /// <summary>
//////            /// Adiciona registro na tabela.
//////            /// </summary>
//////            Add,
//////            /// <summary>
//////            /// Atualiza registro na tabela.
//////            /// </summary>
//////            Update,
//////            /// <summary>
//////            /// Excluir registro na tabela
//////            /// </summary>
//////            Delete,
//////            /// <summary>
//////            /// Exclui TODOS os registros da tabela.
//////            /// </summary>
//////            DeleteAll,
//////            /// <summary>
//////            /// Seleciona um registro na tabela.
//////            /// </summary>
//////            Select,
//////            /// <summary>
//////            /// Seleciona TODOS os registros da tabela.
//////            /// </summary>
//////            SelectAll,
//////            /// <summary>
//////            /// Excluir ou seleciona um registro na tabela.
//////            /// </summary>
//////            SelectORDelete
//////        }
//////        #endregion 
//////
//////
//////        public UsuarioControl() {}
//////
//////
//////        #region Inserindo dados na tabela 
//////
//////        /// <summary> 
//////        /// Grava/Persiste um novo objeto UsuarioFields no banco de dados
//////        /// </summary>
//////        /// <param name="FieldInfo">Objeto UsuarioFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
//////        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////        public bool Add( ref UsuarioFields FieldInfo )
//////        {
//////            try
//////            {
//////                this.Conn = new SqlConnection(this.StrConnetionDB);
//////                this.Conn.Open();
//////                this.Tran = this.Conn.BeginTransaction();
//////                this.Cmd = new SqlCommand("Proc_Usuario_Add", this.Conn, this.Tran);
//////                this.Cmd.CommandType = CommandType.StoredProcedure;
//////                this.Cmd.Parameters.Clear();
//////                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
//////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
//////                this.Tran.Commit();
//////                FieldInfo.idUsuario = (int)this.Cmd.Parameters["@Param_idUsuario"].Value;
//////                return true;
//////
//////            }
//////            catch (SqlException e)
//////            {
//////                this.Tran.Rollback();
//////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: {0}.",  e.Message);
//////                return false;
//////            }
//////            catch (Exception e)
//////            {
//////                this.Tran.Rollback();
//////                this._ErrorMessage = e.Message;
//////                return false;
//////            }
//////            finally
//////            {
//////                if (this.Conn != null)
//////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////                if (this.Cmd != null)
//////                  this.Cmd.Dispose();
//////            }
//////        }
//////
//////        #endregion
//////
//////
//////        #region Inserindo dados na tabela utilizando conexão e transação externa (compartilhada) 
//////
//////        /// <summary> 
//////        /// Grava/Persiste um novo objeto UsuarioFields no banco de dados
//////        /// </summary>
//////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
//////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
//////        /// <param name="FieldInfo">Objeto UsuarioFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
//////        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////        public bool Add( SqlConnection ConnIn, SqlTransaction TranIn, ref UsuarioFields FieldInfo )
//////        {
//////            try
//////            {
//////                this.Cmd = new SqlCommand("Proc_Usuario_Add", ConnIn, TranIn);
//////                this.Cmd.CommandType = CommandType.StoredProcedure;
//////                this.Cmd.Parameters.Clear();
//////                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
//////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
//////                FieldInfo.idUsuario = (int)this.Cmd.Parameters["@Param_idUsuario"].Value;
//////                return true;
//////
//////            }
//////            catch (SqlException e)
//////            {
//////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: {0}.",  e.Message);
//////                return false;
//////            }
//////            catch (Exception e)
//////            {
//////                this._ErrorMessage = e.Message;
//////                return false;
//////            }
//////        }
//////
//////        #endregion
//////
//////
//////        #region Editando dados na tabela 
//////
//////        /// <summary> 
//////        /// Grava/Persiste as alterações em um objeto UsuarioFields no banco de dados
//////        /// </summary>
//////        /// <param name="FieldInfo">Objeto UsuarioFields a ser alterado.</param>
//////        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////        public bool Update( UsuarioFields FieldInfo )
//////        {
//////            try
//////            {
//////                this.Conn = new SqlConnection(this.StrConnetionDB);
//////                this.Conn.Open();
//////                this.Tran = this.Conn.BeginTransaction();
//////                this.Cmd = new SqlCommand("Proc_Usuario_Update", this.Conn, this.Tran);
//////                this.Cmd.CommandType = CommandType.StoredProcedure;
//////                this.Cmd.Parameters.Clear();
//////                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Update));
//////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar atualizar registro!!");
//////                this.Tran.Commit();
//////                return true;
//////            }
//////            catch (SqlException e)
//////            {
//////                this.Tran.Rollback();
//////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: {0}.",  e.Message);
//////                return false;
//////            }
//////            catch (Exception e)
//////            {
//////                this.Tran.Rollback();
//////                this._ErrorMessage = e.Message;
//////                return false;
//////            }
//////            finally
//////            {
//////                if (this.Conn != null)
//////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////                if (this.Cmd != null)
//////                  this.Cmd.Dispose();
//////            }
//////        }
//////
//////        #endregion
//////
//////
//////        #region Editando dados na tabela utilizando conexão e transação externa (compartilhada) 
//////
//////        /// <summary> 
//////        /// Grava/Persiste as alterações em um objeto UsuarioFields no banco de dados
//////        /// </summary>
//////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
//////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
//////        /// <param name="FieldInfo">Objeto UsuarioFields a ser alterado.</param>
//////        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////        public bool Update( SqlConnection ConnIn, SqlTransaction TranIn, UsuarioFields FieldInfo )
//////        {
//////            try
//////            {
//////                this.Cmd = new SqlCommand("Proc_Usuario_Update", ConnIn, TranIn);
//////                this.Cmd.CommandType = CommandType.StoredProcedure;
//////                this.Cmd.Parameters.Clear();
//////                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Update));
//////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar atualizar registro!!");
//////                return true;
//////            }
//////            catch (SqlException e)
//////            {
//////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: {0}.",  e.Message);
//////                return false;
//////            }
//////            catch (Exception e)
//////            {
//////                this._ErrorMessage = e.Message;
//////                return false;
//////            }
//////        }
//////
//////        #endregion
//////
//////
//////        #region Excluindo todos os dados da tabela 
//////
//////        /// <summary> 
//////        /// Exclui todos os registros da tabela
//////        /// </summary>
//////        /// <returns>"true" = registros excluidos com sucesso, "false" = erro ao tentar excluir registros (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////        public bool DeleteAll()
//////        {
//////            try
//////            {
//////                this.Conn = new SqlConnection(this.StrConnetionDB);
//////                this.Conn.Open();
//////                this.Tran = this.Conn.BeginTransaction();
//////                this.Cmd = new SqlCommand("Proc_Usuario_DeleteAll", this.Conn, this.Tran);
//////                this.Cmd.CommandType = CommandType.StoredProcedure;
//////                this.Cmd.Parameters.Clear();
//////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
//////                this.Tran.Commit();
//////                return true;
//////            }
//////            catch (SqlException e)
//////            {
//////                this.Tran.Rollback();
//////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: {0}.",  e.Message);
//////                return false;
//////            }
//////            catch (Exception e)
//////            {
//////                this.Tran.Rollback();
//////                this._ErrorMessage = e.Message;
//////                return false;
//////            }
//////            finally
//////            {
//////                if (this.Conn != null)
//////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////                if (this.Cmd != null)
//////                  this.Cmd.Dispose();
//////            }
//////        }
//////
//////        #endregion
//////
//////
//////        #region Excluindo todos os dados da tabela utilizando conexão e transação externa (compartilhada)
//////
//////        /// <summary> 
//////        /// Exclui todos os registros da tabela
//////        /// </summary>
//////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
//////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
//////        /// <returns>"true" = registros excluidos com sucesso, "false" = erro ao tentar excluir registros (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////        public bool DeleteAll(SqlConnection ConnIn, SqlTransaction TranIn)
//////        {
//////            try
//////            {
//////                this.Cmd = new SqlCommand("Proc_Usuario_DeleteAll", ConnIn, TranIn);
//////                this.Cmd.CommandType = CommandType.StoredProcedure;
//////                this.Cmd.Parameters.Clear();
//////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
//////                return true;
//////            }
//////            catch (SqlException e)
//////            {
//////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: {0}.",  e.Message);
//////                return false;
//////            }
//////            catch (Exception e)
//////            {
//////                this._ErrorMessage = e.Message;
//////                return false;
//////            }
//////        }
//////
//////        #endregion
//////
//////
//////        #region Excluindo dados da tabela 
//////
//////        /// <summary> 
//////        /// Exclui um registro da tabela no banco de dados
//////        /// </summary>
//////        /// <param name="FieldInfo">Objeto UsuarioFields a ser excluído.</param>
//////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////        public bool Delete( UsuarioFields FieldInfo )
//////        {
//////            return Delete(FieldInfo.idUsuario);
//////        }
//////
//////        /// <summary> 
//////        /// Exclui um registro da tabela no banco de dados
//////        /// </summary>
//////        /// <param name="Param_idUsuario">int</param>
//////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////        public bool Delete(
//////                                     int Param_idUsuario)
//////        {
//////            try
//////            {
//////                this.Conn = new SqlConnection(this.StrConnetionDB);
//////                this.Conn.Open();
//////                this.Tran = this.Conn.BeginTransaction();
//////                this.Cmd = new SqlCommand("Proc_Usuario_Delete", this.Conn, this.Tran);
//////                this.Cmd.CommandType = CommandType.StoredProcedure;
//////                this.Cmd.Parameters.Clear();
//////                this.Cmd.Parameters.Add(new SqlParameter("@Param_idUsuario", SqlDbType.Int)).Value = Param_idUsuario;
//////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
//////                this.Tran.Commit();
//////                return true;
//////            }
//////            catch (SqlException e)
//////            {
//////                this.Tran.Rollback();
//////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: {0}.",  e.Message);
//////                return false;
//////            }
//////            catch (Exception e)
//////            {
//////                this.Tran.Rollback();
//////                this._ErrorMessage = e.Message;
//////                return false;
//////            }
//////            finally
//////            {
//////                if (this.Conn != null)
//////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////                if (this.Cmd != null)
//////                  this.Cmd.Dispose();
//////            }
//////        }
//////
//////        #endregion
//////
//////
//////        #region Excluindo dados da tabela utilizando conexão e transação externa (compartilhada)
//////
//////        /// <summary> 
//////        /// Exclui um registro da tabela no banco de dados
//////        /// </summary>
//////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
//////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
//////        /// <param name="FieldInfo">Objeto UsuarioFields a ser excluído.</param>
//////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, UsuarioFields FieldInfo )
//////        {
//////            return Delete(ConnIn, TranIn, FieldInfo.idUsuario);
//////        }
//////
//////        /// <summary> 
//////        /// Exclui um registro da tabela no banco de dados
//////        /// </summary>
//////        /// <param name="Param_idUsuario">int</param>
//////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
//////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
//////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, 
//////                                     int Param_idUsuario)
//////        {
//////            try
//////            {
//////                this.Cmd = new SqlCommand("Proc_Usuario_Delete", ConnIn, TranIn);
//////                this.Cmd.CommandType = CommandType.StoredProcedure;
//////                this.Cmd.Parameters.Clear();
//////                this.Cmd.Parameters.Add(new SqlParameter("@Param_idUsuario", SqlDbType.Int)).Value = Param_idUsuario;
//////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
//////                return true;
//////            }
//////            catch (SqlException e)
//////            {
//////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: {0}.",  e.Message);
//////                return false;
//////            }
//////            catch (Exception e)
//////            {
//////                this._ErrorMessage = e.Message;
//////                return false;
//////            }
//////        }
//////
//////        #endregion
//////
//////
//////        #region Selecionando um item da tabela 
//////
//////        /// <summary> 
//////        /// Retorna um objeto UsuarioFields através da chave primária passada como parâmetro
//////        /// </summary>
//////        /// <param name="Param_idUsuario">int</param>
//////        /// <returns>Objeto UsuarioFields</returns> 
//////        public UsuarioFields GetItem(
//////                                     int Param_idUsuario)
//////        {
//////            UsuarioFields infoFields = new UsuarioFields();
//////            try
//////            {
//////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////                {
//////                    using (this.Cmd = new SqlCommand("Proc_Usuario_Select", this.Conn))
//////                    {
//////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////                        this.Cmd.Parameters.Clear();
//////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_idUsuario", SqlDbType.Int)).Value = Param_idUsuario;
//////                        this.Cmd.Connection.Open();
//////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
//////                        {
//////                            if (!dr.HasRows) return null;
//////                            if (dr.Read())
//////                            {
//////                               infoFields = GetDataFromReader( dr );
//////                            }
//////                        }
//////                    }
//////                 }
//////
//////                 return infoFields;
//////
//////            }
//////            catch (SqlException e)
//////            {
//////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
//////                return null;
//////            }
//////            catch (Exception e)
//////            {
//////                this._ErrorMessage = e.Message;
//////                return null;
//////            }
//////            finally
//////            {
//////                if (this.Conn != null)
//////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////            }
//////        }
//////
//////        #endregion
//////
//////
//////        #region Selecionando todos os dados da tabela 
//////
//////        /// <summary> 
//////        /// Seleciona todos os registro da tabela e preenche um ArrayList com o objeto UsuarioFields.
//////        /// </summary>
//////        /// <returns>ArrayList de objetos UsuarioFields</returns> 
//////        public ArrayList GetAll()
//////        {
//////            ArrayList arrayInfo = new ArrayList();
//////            try
//////            {
//////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////                {
//////                    using (this.Cmd = new SqlCommand("Proc_Usuario_GetAll", this.Conn))
//////                    {
//////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////                        this.Cmd.Parameters.Clear();
//////                        this.Cmd.Connection.Open();
//////                        using (SqlDataReader dr = this.Cmd.ExecuteReader())
//////                        {
//////                           if (!dr.HasRows) return null;
//////                           while (dr.Read())
//////                           {
//////                              arrayInfo.Add(GetDataFromReader( dr ));
//////                           }
//////                        }
//////                    }
//////                }
//////
//////                return arrayInfo;
//////
//////            }
//////            catch (SqlException e)
//////            {
//////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
//////                return null;
//////            }
//////            catch (Exception e)
//////            {
//////                this._ErrorMessage = e.Message;
//////                return null;
//////            }
//////            finally
//////            {
//////                if (this.Conn != null)
//////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////            }
//////        }
//////
//////        #endregion
//////
//////
//////        #region Contando os dados da tabela 
//////
//////        /// <summary> 
//////        /// Retorna o total de registros contidos na tabela
//////        /// </summary>
//////        /// <returns>int</returns> 
//////        public int CountAll()
//////        {
//////            try
//////            {
//////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////                {
//////                    using (this.Cmd = new SqlCommand("Proc_Usuario_CountAll", this.Conn))
//////                    {
//////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////                        this.Cmd.Parameters.Clear();
//////                        this.Cmd.Connection.Open();
//////                        object CountRegs = this.Cmd.ExecuteScalar();
//////                        if (CountRegs == null)
//////                        { return 0; }
//////                        else
//////                        { return (int)CountRegs; }
//////                    }
//////                }
//////            }
//////            catch (SqlException e)
//////            {
//////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
//////                return 0;
//////            }
//////            catch (Exception e)
//////            {
//////                this._ErrorMessage = e.Message;
//////                return 0;
//////            }
//////            finally
//////            {
//////                if (this.Conn != null)
//////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////            }
//////        }
//////
//////        #endregion
//////
//////
//////        #region Selecionando dados da tabela através do campo "Nome" 
//////
//////        /// <summary> 
//////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Nome.
//////        /// </summary>
//////        /// <param name="Param_Nome">string</param>
//////        /// <returns>UsuarioFields</returns> 
//////        public UsuarioFields FindByNome(
//////                               string Param_Nome )
//////        {
//////            UsuarioFields infoFields = new UsuarioFields();
//////            try
//////            {
//////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////                {
//////                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindByNome", this.Conn))
//////                    {
//////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////                        this.Cmd.Parameters.Clear();
//////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Nome", SqlDbType.VarChar, 200)).Value = Param_Nome;
//////                        this.Cmd.Connection.Open();
//////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
//////                        {
//////                            if (!dr.HasRows) return null;
//////                            if (dr.Read())
//////                            {
//////                               infoFields = GetDataFromReader( dr );
//////                            }
//////                        }
//////                    }
//////                }
//////
//////                return infoFields;
//////
//////            }
//////            catch (SqlException e)
//////            {
//////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
//////                return null;
//////            }
//////            catch (Exception e)
//////            {
//////                this._ErrorMessage = e.Message;
//////                return null;
//////            }
//////            finally
//////            {
//////                if (this.Conn != null)
//////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////            }
//////        }
//////
//////        #endregion
//////
//////
//////
//////        #region Selecionando dados da tabela através do campo "UserName" 
//////
//////        /// <summary> 
//////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo UserName.
//////        /// </summary>
//////        /// <param name="Param_UserName">string</param>
//////        /// <returns>UsuarioFields</returns> 
//////        public UsuarioFields FindByUserName(
//////                               string Param_UserName )
//////        {
//////            UsuarioFields infoFields = new UsuarioFields();
//////            try
//////            {
//////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////                {
//////                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindByUserName", this.Conn))
//////                    {
//////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////                        this.Cmd.Parameters.Clear();
//////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_UserName", SqlDbType.VarChar, 100)).Value = Param_UserName;
//////                        this.Cmd.Connection.Open();
//////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
//////                        {
//////                            if (!dr.HasRows) return null;
//////                            if (dr.Read())
//////                            {
//////                               infoFields = GetDataFromReader( dr );
//////                            }
//////                        }
//////                    }
//////                }
//////
//////                return infoFields;
//////
//////            }
//////            catch (SqlException e)
//////            {
//////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
//////                return null;
//////            }
//////            catch (Exception e)
//////            {
//////                this._ErrorMessage = e.Message;
//////                return null;
//////            }
//////            finally
//////            {
//////                if (this.Conn != null)
//////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////            }
//////        }
//////
//////        #endregion
//////
//////
//////
//////        #region Selecionando dados da tabela através do campo "Password" 
//////
//////        /// <summary> 
//////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Password.
//////        /// </summary>
//////        /// <param name="Param_Password">string</param>
//////        /// <returns>UsuarioFields</returns> 
//////        public UsuarioFields FindByPassword(
//////                               string Param_Password )
//////        {
//////            UsuarioFields infoFields = new UsuarioFields();
//////            try
//////            {
//////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////                {
//////                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindByPassword", this.Conn))
//////                    {
//////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////                        this.Cmd.Parameters.Clear();
//////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Password", SqlDbType.VarChar, 100)).Value = Param_Password;
//////                        this.Cmd.Connection.Open();
//////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
//////                        {
//////                            if (!dr.HasRows) return null;
//////                            if (dr.Read())
//////                            {
//////                               infoFields = GetDataFromReader( dr );
//////                            }
//////                        }
//////                    }
//////                }
//////
//////                return infoFields;
//////
//////            }
//////            catch (SqlException e)
//////            {
//////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
//////                return null;
//////            }
//////            catch (Exception e)
//////            {
//////                this._ErrorMessage = e.Message;
//////                return null;
//////            }
//////            finally
//////            {
//////                if (this.Conn != null)
//////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////            }
//////        }
//////
//////        #endregion
//////
//////
//////
//////        #region Selecionando dados da tabela através do campo "Cargo" 
//////
//////        /// <summary> 
//////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Cargo.
//////        /// </summary>
//////        /// <param name="Param_Cargo">string</param>
//////        /// <returns>UsuarioFields</returns> 
//////        public UsuarioFields FindByCargo(
//////                               string Param_Cargo )
//////        {
//////            UsuarioFields infoFields = new UsuarioFields();
//////            try
//////            {
//////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////                {
//////                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindByCargo", this.Conn))
//////                    {
//////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////                        this.Cmd.Parameters.Clear();
//////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Cargo", SqlDbType.VarChar, 50)).Value = Param_Cargo;
//////                        this.Cmd.Connection.Open();
//////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
//////                        {
//////                            if (!dr.HasRows) return null;
//////                            if (dr.Read())
//////                            {
//////                               infoFields = GetDataFromReader( dr );
//////                            }
//////                        }
//////                    }
//////                }
//////
//////                return infoFields;
//////
//////            }
//////            catch (SqlException e)
//////            {
//////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
//////                return null;
//////            }
//////            catch (Exception e)
//////            {
//////                this._ErrorMessage = e.Message;
//////                return null;
//////            }
//////            finally
//////            {
//////                if (this.Conn != null)
//////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////            }
//////        }
//////
//////        #endregion
//////
//////
//////
//////        #region Selecionando dados da tabela através do campo "Situacao" 
//////
//////        /// <summary> 
//////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Situacao.
//////        /// </summary>
//////        /// <param name="Param_Situacao">string</param>
//////        /// <returns>UsuarioFields</returns> 
//////        public UsuarioFields FindBySituacao(
//////                               string Param_Situacao )
//////        {
//////            UsuarioFields infoFields = new UsuarioFields();
//////            try
//////            {
//////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////                {
//////                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindBySituacao", this.Conn))
//////                    {
//////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////                        this.Cmd.Parameters.Clear();
//////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Situacao", SqlDbType.VarChar, 1)).Value = Param_Situacao;
//////                        this.Cmd.Connection.Open();
//////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
//////                        {
//////                            if (!dr.HasRows) return null;
//////                            if (dr.Read())
//////                            {
//////                               infoFields = GetDataFromReader( dr );
//////                            }
//////                        }
//////                    }
//////                }
//////
//////                return infoFields;
//////
//////            }
//////            catch (SqlException e)
//////            {
//////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
//////                return null;
//////            }
//////            catch (Exception e)
//////            {
//////                this._ErrorMessage = e.Message;
//////                return null;
//////            }
//////            finally
//////            {
//////                if (this.Conn != null)
//////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////            }
//////        }
//////
//////        #endregion
//////
//////
//////
//////        #region Selecionando dados da tabela através do campo "Modulo" 
//////
//////        /// <summary> 
//////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Modulo.
//////        /// </summary>
//////        /// <param name="Param_Modulo">string</param>
//////        /// <returns>UsuarioFields</returns> 
//////        public UsuarioFields FindByModulo(
//////                               string Param_Modulo )
//////        {
//////            UsuarioFields infoFields = new UsuarioFields();
//////            try
//////            {
//////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////                {
//////                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindByModulo", this.Conn))
//////                    {
//////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////                        this.Cmd.Parameters.Clear();
//////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Modulo", SqlDbType.VarChar, 1)).Value = Param_Modulo;
//////                        this.Cmd.Connection.Open();
//////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
//////                        {
//////                            if (!dr.HasRows) return null;
//////                            if (dr.Read())
//////                            {
//////                               infoFields = GetDataFromReader( dr );
//////                            }
//////                        }
//////                    }
//////                }
//////
//////                return infoFields;
//////
//////            }
//////            catch (SqlException e)
//////            {
//////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
//////                return null;
//////            }
//////            catch (Exception e)
//////            {
//////                this._ErrorMessage = e.Message;
//////                return null;
//////            }
//////            finally
//////            {
//////                if (this.Conn != null)
//////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////            }
//////        }
//////
//////        #endregion
//////
//////
//////
//////        #region Selecionando dados da tabela através do campo "FkUa" 
//////
//////        /// <summary> 
//////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo FkUa.
//////        /// </summary>
//////        /// <param name="Param_FkUa">int</param>
//////        /// <returns>UsuarioFields</returns> 
//////        public UsuarioFields FindByFkUa(
//////                               int Param_FkUa )
//////        {
//////            UsuarioFields infoFields = new UsuarioFields();
//////            try
//////            {
//////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////                {
//////                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindByFkUa", this.Conn))
//////                    {
//////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////                        this.Cmd.Parameters.Clear();
//////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_FkUa", SqlDbType.Int)).Value = Param_FkUa;
//////                        this.Cmd.Connection.Open();
//////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
//////                        {
//////                            if (!dr.HasRows) return null;
//////                            if (dr.Read())
//////                            {
//////                               infoFields = GetDataFromReader( dr );
//////                            }
//////                        }
//////                    }
//////                }
//////
//////                return infoFields;
//////
//////            }
//////            catch (SqlException e)
//////            {
//////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
//////                return null;
//////            }
//////            catch (Exception e)
//////            {
//////                this._ErrorMessage = e.Message;
//////                return null;
//////            }
//////            finally
//////            {
//////                if (this.Conn != null)
//////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////            }
//////        }
//////
//////        #endregion
//////
//////
//////
//////        #region Função GetDataFromReader
//////
//////        /// <summary> 
//////        /// Retorna um objeto UsuarioFields preenchido com os valores dos campos do SqlDataReader
//////        /// </summary>
//////        /// <param name="dr">SqlDataReader - Preenche o objeto UsuarioFields </param>
//////        /// <returns>UsuarioFields</returns>
//////        private UsuarioFields GetDataFromReader( SqlDataReader dr )
//////        {
//////            UsuarioFields infoFields = new UsuarioFields();
//////
//////            if (!dr.IsDBNull(0))
//////            { infoFields.idUsuario = dr.GetInt32(0); }
//////            else
//////            { infoFields.idUsuario = 0; }
//////
//////
//////
//////            if (!dr.IsDBNull(1))
//////            { infoFields.Nome = dr.GetString(1); }
//////            else
//////            { infoFields.Nome = string.Empty; }
//////
//////
//////
//////            if (!dr.IsDBNull(2))
//////            { infoFields.UserName = dr.GetString(2); }
//////            else
//////            { infoFields.UserName = string.Empty; }
//////
//////
//////
//////            if (!dr.IsDBNull(3))
//////            { infoFields.Password = dr.GetString(3); }
//////            else
//////            { infoFields.Password = string.Empty; }
//////
//////
//////
//////            if (!dr.IsDBNull(4))
//////            { infoFields.Cargo = dr.GetString(4); }
//////            else
//////            { infoFields.Cargo = string.Empty; }
//////
//////
//////
//////            if (!dr.IsDBNull(5))
//////            { infoFields.Situacao = dr.GetString(5); }
//////            else
//////            { infoFields.Situacao = string.Empty; }
//////
//////
//////
//////            if (!dr.IsDBNull(6))
//////            { infoFields.Modulo = dr.GetString(6); }
//////            else
//////            { infoFields.Modulo = string.Empty; }
//////
//////
//////
//////            if (!dr.IsDBNull(7))
//////            { infoFields.FkUa = dr.GetInt32(7); }
//////            else
//////            { infoFields.FkUa = 0; }
//////
//////
//////            return infoFields;
//////        }
//////        #endregion
//////
//////
//////
//////
//////
//////
//////
//////
//////
//////
//////
//////
//////
//////
//////
//////
//////
//////
//////
//////
//////
//////
//////        #region Função GetAllParameters
//////
//////        /// <summary> 
//////        /// Retorna um array de parâmetros com campos para atualização, seleção e inserção no banco de dados
//////        /// </summary>
//////        /// <param name="FieldInfo">Objeto UsuarioFields</param>
//////        /// <param name="Modo">Tipo de oepração a ser executada no banco de dados</param>
//////        /// <returns>SqlParameter[] - Array de parâmetros</returns> 
//////        private SqlParameter[] GetAllParameters( UsuarioFields FieldInfo, SQLMode Modo )
//////        {
//////            SqlParameter[] Parameters;
//////
//////            switch (Modo)
//////            {
//////                case SQLMode.Add:
//////                    Parameters = new SqlParameter[8];
//////                    for (int I = 0; I < Parameters.Length; I++)
//////                       Parameters[I] = new SqlParameter();
//////                    //Field idUsuario
//////                    Parameters[0].SqlDbType = SqlDbType.Int;
//////                    Parameters[0].Direction = ParameterDirection.Output;
//////                    Parameters[0].ParameterName = "@Param_idUsuario";
//////                    Parameters[0].Value = DBNull.Value;
//////
//////                    break;
//////
//////                case SQLMode.Update:
//////                    Parameters = new SqlParameter[8];
//////                    for (int I = 0; I < Parameters.Length; I++)
//////                       Parameters[I] = new SqlParameter();
//////                    //Field idUsuario
//////                    Parameters[0].SqlDbType = SqlDbType.Int;
//////                    Parameters[0].ParameterName = "@Param_idUsuario";
//////                    Parameters[0].Value = FieldInfo.idUsuario;
//////
//////                    break;
//////
//////                case SQLMode.SelectORDelete:
//////                    Parameters = new SqlParameter[1];
//////                    for (int I = 0; I < Parameters.Length; I++)
//////                       Parameters[I] = new SqlParameter();
//////                    //Field idUsuario
//////                    Parameters[0].SqlDbType = SqlDbType.Int;
//////                    Parameters[0].ParameterName = "@Param_idUsuario";
//////                    Parameters[0].Value = FieldInfo.idUsuario;
//////
//////                    return Parameters;
//////
//////                default:
//////                    Parameters = new SqlParameter[8];
//////                    for (int I = 0; I < Parameters.Length; I++)
//////                       Parameters[I] = new SqlParameter();
//////                    break;
//////            }
//////
//////            //Field Nome
//////            Parameters[1].SqlDbType = SqlDbType.VarChar;
//////            Parameters[1].ParameterName = "@Param_Nome";
//////            if (( FieldInfo.Nome == null ) || ( FieldInfo.Nome == string.Empty ))
//////            { Parameters[1].Value = DBNull.Value; }
//////            else
//////            { Parameters[1].Value = FieldInfo.Nome; }
//////            Parameters[1].Size = 200;
//////
//////            //Field UserName
//////            Parameters[2].SqlDbType = SqlDbType.VarChar;
//////            Parameters[2].ParameterName = "@Param_UserName";
//////            if (( FieldInfo.UserName == null ) || ( FieldInfo.UserName == string.Empty ))
//////            { Parameters[2].Value = DBNull.Value; }
//////            else
//////            { Parameters[2].Value = FieldInfo.UserName; }
//////            Parameters[2].Size = 100;
//////
//////            //Field Password
//////            Parameters[3].SqlDbType = SqlDbType.VarChar;
//////            Parameters[3].ParameterName = "@Param_Password";
//////            if (( FieldInfo.Password == null ) || ( FieldInfo.Password == string.Empty ))
//////            { Parameters[3].Value = DBNull.Value; }
//////            else
//////            { Parameters[3].Value = FieldInfo.Password; }
//////            Parameters[3].Size = 100;
//////
//////            //Field Cargo
//////            Parameters[4].SqlDbType = SqlDbType.VarChar;
//////            Parameters[4].ParameterName = "@Param_Cargo";
//////            if (( FieldInfo.Cargo == null ) || ( FieldInfo.Cargo == string.Empty ))
//////            { Parameters[4].Value = DBNull.Value; }
//////            else
//////            { Parameters[4].Value = FieldInfo.Cargo; }
//////            Parameters[4].Size = 50;
//////
//////            //Field Situacao
//////            Parameters[5].SqlDbType = SqlDbType.VarChar;
//////            Parameters[5].ParameterName = "@Param_Situacao";
//////            if (( FieldInfo.Situacao == null ) || ( FieldInfo.Situacao == string.Empty ))
//////            { Parameters[5].Value = DBNull.Value; }
//////            else
//////            { Parameters[5].Value = FieldInfo.Situacao; }
//////            Parameters[5].Size = 1;
//////
//////            //Field Modulo
//////            Parameters[6].SqlDbType = SqlDbType.VarChar;
//////            Parameters[6].ParameterName = "@Param_Modulo";
//////            if (( FieldInfo.Modulo == null ) || ( FieldInfo.Modulo == string.Empty ))
//////            { Parameters[6].Value = DBNull.Value; }
//////            else
//////            { Parameters[6].Value = FieldInfo.Modulo; }
//////            Parameters[6].Size = 1;
//////
//////            //Field FkUa
//////            Parameters[7].SqlDbType = SqlDbType.Int;
//////            Parameters[7].ParameterName = "@Param_FkUa";
//////            Parameters[7].Value = FieldInfo.FkUa;
//////
//////            return Parameters;
//////        }
//////        #endregion
//////
//////
//////
//////
//////
//////        #region IDisposable Members 
//////
//////        bool disposed = false;
//////
//////        public void Dispose()
//////        {
//////            Dispose(true);
//////            GC.SuppressFinalize(this);
//////        }
//////
//////        ~UsuarioControl() 
//////        { 
//////            Dispose(false); 
//////        }
//////
//////        private void Dispose(bool disposing) 
//////        {
//////            if (!this.disposed)
//////            {
//////                if (disposing) 
//////                {
//////                    if (this.Conn != null)
//////                        if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////                }
//////            }
//////
//////        }
//////        #endregion 
//////
//////
//////
//////    }
//////
//////}
//////
//////
//////
//////
//////
////////Projeto substituído ------------------------
////////using System;
////////using System.Collections;
////////using System.Collections.Generic;
////////using System.Data;
////////using System.Data.SqlClient;
////////using System.Configuration;
////////using System.Text;
////////
////////namespace SWGPgen
////////{
////////
////////
////////    /// <summary> 
////////    /// Tabela: Usuario  
////////    /// Autor: DAL Creator .net 
////////    /// Data de criação: 13/03/2012 21:30:59 
////////    /// Descrição: Classe responsável pela perssitência de dados. Utiliza a classe "UsuarioFields". 
////////    /// </summary> 
////////    public class UsuarioControl : IDisposable 
////////    {
////////
////////        #region String de conexão 
////////        private string StrConnetionDB = "Data Source=DEKO-PC;Initial Catalog=swgp;User Id=sureg;Password=@sureg2012;";
////////        #endregion
////////
////////
////////        #region Propriedade que armazena erros de execução 
////////        private string _ErrorMessage;
////////        public string ErrorMessage { get { return _ErrorMessage; } }
////////        #endregion
////////
////////
////////        #region Objetos de conexão 
////////        SqlConnection Conn;
////////        SqlCommand Cmd;
////////        SqlTransaction Tran;
////////        #endregion
////////
////////
////////        #region Funcões que retornam Conexões e Transações 
////////
////////        public SqlTransaction GetNewTransaction(SqlConnection connIn)
////////        {
////////            if (connIn.State != ConnectionState.Open)
////////                connIn.Open();
////////            SqlTransaction TranOut = connIn.BeginTransaction();
////////            return TranOut;
////////        }
////////
////////        public SqlConnection GetNewConnection()
////////        {
////////            return GetNewConnection(this.StrConnetionDB);
////////        }
////////
////////        public SqlConnection GetNewConnection(string StringConnection)
////////        {
////////            SqlConnection connOut = new SqlConnection(StringConnection);
////////            return connOut;
////////        }
////////
////////        #endregion
////////
////////
////////        #region enum SQLMode 
////////        /// <summary>   
////////        /// Representa o procedimento que está sendo executado na tabela.
////////        /// </summary>
////////        public enum SQLMode
////////        {                     
////////            /// <summary>
////////            /// Adiciona registro na tabela.
////////            /// </summary>
////////            Add,
////////            /// <summary>
////////            /// Atualiza registro na tabela.
////////            /// </summary>
////////            Update,
////////            /// <summary>
////////            /// Excluir registro na tabela
////////            /// </summary>
////////            Delete,
////////            /// <summary>
////////            /// Exclui TODOS os registros da tabela.
////////            /// </summary>
////////            DeleteAll,
////////            /// <summary>
////////            /// Seleciona um registro na tabela.
////////            /// </summary>
////////            Select,
////////            /// <summary>
////////            /// Seleciona TODOS os registros da tabela.
////////            /// </summary>
////////            SelectAll,
////////            /// <summary>
////////            /// Excluir ou seleciona um registro na tabela.
////////            /// </summary>
////////            SelectORDelete
////////        }
////////        #endregion 
////////
////////
////////        public UsuarioControl() {}
////////
////////
////////        #region Inserindo dados na tabela 
////////
////////        /// <summary> 
////////        /// Grava/Persiste um novo objeto UsuarioFields no banco de dados
////////        /// </summary>
////////        /// <param name="FieldInfo">Objeto UsuarioFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
////////        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////////        public bool Add( ref UsuarioFields FieldInfo )
////////        {
////////            try
////////            {
////////                this.Conn = new SqlConnection(this.StrConnetionDB);
////////                this.Conn.Open();
////////                this.Tran = this.Conn.BeginTransaction();
////////                this.Cmd = new SqlCommand("Proc_Usuario_Add", this.Conn, this.Tran);
////////                this.Cmd.CommandType = CommandType.StoredProcedure;
////////                this.Cmd.Parameters.Clear();
////////                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
////////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
////////                this.Tran.Commit();
////////                FieldInfo.idUsuario = (int)this.Cmd.Parameters["@Param_idUsuario"].Value;
////////                return true;
////////
////////            }
////////            catch (SqlException e)
////////            {
////////                this.Tran.Rollback();
////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: {0}.",  e.Message);
////////                return false;
////////            }
////////            catch (Exception e)
////////            {
////////                this.Tran.Rollback();
////////                this._ErrorMessage = e.Message;
////////                return false;
////////            }
////////            finally
////////            {
////////                if (this.Conn != null)
////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////////                if (this.Cmd != null)
////////                  this.Cmd.Dispose();
////////            }
////////        }
////////
////////        #endregion
////////
////////
////////        #region Inserindo dados na tabela utilizando conexão e transação externa (compartilhada) 
////////
////////        /// <summary> 
////////        /// Grava/Persiste um novo objeto UsuarioFields no banco de dados
////////        /// </summary>
////////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
////////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
////////        /// <param name="FieldInfo">Objeto UsuarioFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
////////        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////////        public bool Add( SqlConnection ConnIn, SqlTransaction TranIn, ref UsuarioFields FieldInfo )
////////        {
////////            try
////////            {
////////                this.Cmd = new SqlCommand("Proc_Usuario_Add", ConnIn, TranIn);
////////                this.Cmd.CommandType = CommandType.StoredProcedure;
////////                this.Cmd.Parameters.Clear();
////////                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
////////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
////////                FieldInfo.idUsuario = (int)this.Cmd.Parameters["@Param_idUsuario"].Value;
////////                return true;
////////
////////            }
////////            catch (SqlException e)
////////            {
////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: {0}.",  e.Message);
////////                return false;
////////            }
////////            catch (Exception e)
////////            {
////////                this._ErrorMessage = e.Message;
////////                return false;
////////            }
////////        }
////////
////////        #endregion
////////
////////
////////        #region Editando dados na tabela 
////////
////////        /// <summary> 
////////        /// Grava/Persiste as alterações em um objeto UsuarioFields no banco de dados
////////        /// </summary>
////////        /// <param name="FieldInfo">Objeto UsuarioFields a ser alterado.</param>
////////        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////////        public bool Update( UsuarioFields FieldInfo )
////////        {
////////            try
////////            {
////////                this.Conn = new SqlConnection(this.StrConnetionDB);
////////                this.Conn.Open();
////////                this.Tran = this.Conn.BeginTransaction();
////////                this.Cmd = new SqlCommand("Proc_Usuario_Update", this.Conn, this.Tran);
////////                this.Cmd.CommandType = CommandType.StoredProcedure;
////////                this.Cmd.Parameters.Clear();
////////                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Update));
////////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar atualizar registro!!");
////////                this.Tran.Commit();
////////                return true;
////////            }
////////            catch (SqlException e)
////////            {
////////                this.Tran.Rollback();
////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: {0}.",  e.Message);
////////                return false;
////////            }
////////            catch (Exception e)
////////            {
////////                this.Tran.Rollback();
////////                this._ErrorMessage = e.Message;
////////                return false;
////////            }
////////            finally
////////            {
////////                if (this.Conn != null)
////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////////                if (this.Cmd != null)
////////                  this.Cmd.Dispose();
////////            }
////////        }
////////
////////        #endregion
////////
////////
////////        #region Editando dados na tabela utilizando conexão e transação externa (compartilhada) 
////////
////////        /// <summary> 
////////        /// Grava/Persiste as alterações em um objeto UsuarioFields no banco de dados
////////        /// </summary>
////////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
////////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
////////        /// <param name="FieldInfo">Objeto UsuarioFields a ser alterado.</param>
////////        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////////        public bool Update( SqlConnection ConnIn, SqlTransaction TranIn, UsuarioFields FieldInfo )
////////        {
////////            try
////////            {
////////                this.Cmd = new SqlCommand("Proc_Usuario_Update", ConnIn, TranIn);
////////                this.Cmd.CommandType = CommandType.StoredProcedure;
////////                this.Cmd.Parameters.Clear();
////////                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Update));
////////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar atualizar registro!!");
////////                return true;
////////            }
////////            catch (SqlException e)
////////            {
////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: {0}.",  e.Message);
////////                return false;
////////            }
////////            catch (Exception e)
////////            {
////////                this._ErrorMessage = e.Message;
////////                return false;
////////            }
////////        }
////////
////////        #endregion
////////
////////
////////        #region Excluindo todos os dados da tabela 
////////
////////        /// <summary> 
////////        /// Exclui todos os registros da tabela
////////        /// </summary>
////////        /// <returns>"true" = registros excluidos com sucesso, "false" = erro ao tentar excluir registros (consulte a propriedade ErrorMessage para detalhes)</returns> 
////////        public bool DeleteAll()
////////        {
////////            try
////////            {
////////                this.Conn = new SqlConnection(this.StrConnetionDB);
////////                this.Conn.Open();
////////                this.Tran = this.Conn.BeginTransaction();
////////                this.Cmd = new SqlCommand("Proc_Usuario_DeleteAll", this.Conn, this.Tran);
////////                this.Cmd.CommandType = CommandType.StoredProcedure;
////////                this.Cmd.Parameters.Clear();
////////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
////////                this.Tran.Commit();
////////                return true;
////////            }
////////            catch (SqlException e)
////////            {
////////                this.Tran.Rollback();
////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: {0}.",  e.Message);
////////                return false;
////////            }
////////            catch (Exception e)
////////            {
////////                this.Tran.Rollback();
////////                this._ErrorMessage = e.Message;
////////                return false;
////////            }
////////            finally
////////            {
////////                if (this.Conn != null)
////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////////                if (this.Cmd != null)
////////                  this.Cmd.Dispose();
////////            }
////////        }
////////
////////        #endregion
////////
////////
////////        #region Excluindo todos os dados da tabela utilizando conexão e transação externa (compartilhada)
////////
////////        /// <summary> 
////////        /// Exclui todos os registros da tabela
////////        /// </summary>
////////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
////////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
////////        /// <returns>"true" = registros excluidos com sucesso, "false" = erro ao tentar excluir registros (consulte a propriedade ErrorMessage para detalhes)</returns> 
////////        public bool DeleteAll(SqlConnection ConnIn, SqlTransaction TranIn)
////////        {
////////            try
////////            {
////////                this.Cmd = new SqlCommand("Proc_Usuario_DeleteAll", ConnIn, TranIn);
////////                this.Cmd.CommandType = CommandType.StoredProcedure;
////////                this.Cmd.Parameters.Clear();
////////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
////////                return true;
////////            }
////////            catch (SqlException e)
////////            {
////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: {0}.",  e.Message);
////////                return false;
////////            }
////////            catch (Exception e)
////////            {
////////                this._ErrorMessage = e.Message;
////////                return false;
////////            }
////////        }
////////
////////        #endregion
////////
////////
////////        #region Excluindo dados da tabela 
////////
////////        /// <summary> 
////////        /// Exclui um registro da tabela no banco de dados
////////        /// </summary>
////////        /// <param name="FieldInfo">Objeto UsuarioFields a ser excluído.</param>
////////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////////        public bool Delete( UsuarioFields FieldInfo )
////////        {
////////            return Delete(FieldInfo.idUsuario);
////////        }
////////
////////        /// <summary> 
////////        /// Exclui um registro da tabela no banco de dados
////////        /// </summary>
////////        /// <param name="Param_idUsuario">int</param>
////////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////////        public bool Delete(
////////                                     int Param_idUsuario)
////////        {
////////            try
////////            {
////////                this.Conn = new SqlConnection(this.StrConnetionDB);
////////                this.Conn.Open();
////////                this.Tran = this.Conn.BeginTransaction();
////////                this.Cmd = new SqlCommand("Proc_Usuario_Delete", this.Conn, this.Tran);
////////                this.Cmd.CommandType = CommandType.StoredProcedure;
////////                this.Cmd.Parameters.Clear();
////////                this.Cmd.Parameters.Add(new SqlParameter("@Param_idUsuario", SqlDbType.Int)).Value = Param_idUsuario;
////////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
////////                this.Tran.Commit();
////////                return true;
////////            }
////////            catch (SqlException e)
////////            {
////////                this.Tran.Rollback();
////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: {0}.",  e.Message);
////////                return false;
////////            }
////////            catch (Exception e)
////////            {
////////                this.Tran.Rollback();
////////                this._ErrorMessage = e.Message;
////////                return false;
////////            }
////////            finally
////////            {
////////                if (this.Conn != null)
////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////////                if (this.Cmd != null)
////////                  this.Cmd.Dispose();
////////            }
////////        }
////////
////////        #endregion
////////
////////
////////        #region Excluindo dados da tabela utilizando conexão e transação externa (compartilhada)
////////
////////        /// <summary> 
////////        /// Exclui um registro da tabela no banco de dados
////////        /// </summary>
////////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
////////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
////////        /// <param name="FieldInfo">Objeto UsuarioFields a ser excluído.</param>
////////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////////        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, UsuarioFields FieldInfo )
////////        {
////////            return Delete(ConnIn, TranIn, FieldInfo.idUsuario);
////////        }
////////
////////        /// <summary> 
////////        /// Exclui um registro da tabela no banco de dados
////////        /// </summary>
////////        /// <param name="Param_idUsuario">int</param>
////////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
////////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
////////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////////        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, 
////////                                     int Param_idUsuario)
////////        {
////////            try
////////            {
////////                this.Cmd = new SqlCommand("Proc_Usuario_Delete", ConnIn, TranIn);
////////                this.Cmd.CommandType = CommandType.StoredProcedure;
////////                this.Cmd.Parameters.Clear();
////////                this.Cmd.Parameters.Add(new SqlParameter("@Param_idUsuario", SqlDbType.Int)).Value = Param_idUsuario;
////////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
////////                return true;
////////            }
////////            catch (SqlException e)
////////            {
////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: {0}.",  e.Message);
////////                return false;
////////            }
////////            catch (Exception e)
////////            {
////////                this._ErrorMessage = e.Message;
////////                return false;
////////            }
////////        }
////////
////////        #endregion
////////
////////
////////        #region Selecionando um item da tabela 
////////
////////        /// <summary> 
////////        /// Retorna um objeto UsuarioFields através da chave primária passada como parâmetro
////////        /// </summary>
////////        /// <param name="Param_idUsuario">int</param>
////////        /// <returns>Objeto UsuarioFields</returns> 
////////        public UsuarioFields GetItem(
////////                                     int Param_idUsuario)
////////        {
////////            UsuarioFields infoFields = new UsuarioFields();
////////            try
////////            {
////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////////                {
////////                    using (this.Cmd = new SqlCommand("Proc_Usuario_Select", this.Conn))
////////                    {
////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////////                        this.Cmd.Parameters.Clear();
////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_idUsuario", SqlDbType.Int)).Value = Param_idUsuario;
////////                        this.Cmd.Connection.Open();
////////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
////////                        {
////////                            if (!dr.HasRows) return null;
////////                            if (dr.Read())
////////                            {
////////                               infoFields = GetDataFromReader( dr );
////////                            }
////////                        }
////////                    }
////////                 }
////////
////////                 return infoFields;
////////
////////            }
////////            catch (SqlException e)
////////            {
////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
////////                return null;
////////            }
////////            catch (Exception e)
////////            {
////////                this._ErrorMessage = e.Message;
////////                return null;
////////            }
////////            finally
////////            {
////////                if (this.Conn != null)
////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////////            }
////////        }
////////
////////        #endregion
////////
////////
////////        #region Selecionando todos os dados da tabela 
////////
////////        /// <summary> 
////////        /// Seleciona todos os registro da tabela e preenche um ArrayList com o objeto UsuarioFields.
////////        /// </summary>
////////        /// <returns>List de objetos UsuarioFields</returns> 
////////        public List<UsuarioFields> GetAll()
////////        {
////////            List<UsuarioFields> arrayInfo = new List<UsuarioFields>();
////////            try
////////            {
////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////////                {
////////                    using (this.Cmd = new SqlCommand("Proc_Usuario_GetAll", this.Conn))
////////                    {
////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////////                        this.Cmd.Parameters.Clear();
////////                        this.Cmd.Connection.Open();
////////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
////////                        {
////////                           if (!dr.HasRows) return null;
////////                           while (dr.Read())
////////                           {
////////                              arrayInfo.Add(GetDataFromReader( dr ));
////////                           }
////////                        }
////////                    }
////////                }
////////
////////                return arrayInfo;
////////
////////            }
////////            catch (SqlException e)
////////            {
////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
////////                return null;
////////            }
////////            catch (Exception e)
////////            {
////////                this._ErrorMessage = e.Message;
////////                return null;
////////            }
////////            finally
////////            {
////////                if (this.Conn != null)
////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////////            }
////////        }
////////
////////        #endregion
////////
////////
////////        #region Contando os dados da tabela 
////////
////////        /// <summary> 
////////        /// Retorna o total de registros contidos na tabela
////////        /// </summary>
////////        /// <returns>int</returns> 
////////        public int CountAll()
////////        {
////////            try
////////            {
////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////////                {
////////                    using (this.Cmd = new SqlCommand("Proc_Usuario_CountAll", this.Conn))
////////                    {
////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////////                        this.Cmd.Parameters.Clear();
////////                        this.Cmd.Connection.Open();
////////                        object CountRegs = this.Cmd.ExecuteScalar();
////////                        if (CountRegs == null)
////////                        { return 0; }
////////                        else
////////                        { return (int)CountRegs; }
////////                    }
////////                }
////////            }
////////            catch (SqlException e)
////////            {
////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
////////                return 0;
////////            }
////////            catch (Exception e)
////////            {
////////                this._ErrorMessage = e.Message;
////////                return 0;
////////            }
////////            finally
////////            {
////////                if (this.Conn != null)
////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////////            }
////////        }
////////
////////        #endregion
////////
////////
////////        #region Selecionando dados da tabela através do campo "Nome" 
////////
////////        /// <summary> 
////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Nome.
////////        /// </summary>
////////        /// <param name="Param_Nome">string</param>
////////        /// <returns>ArrayList</returns> 
////////        public ArrayList FindByNome(
////////                               string Param_Nome )
////////        {
////////            ArrayList arrayInfo = new ArrayList();
////////            try
////////            {
////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////////                {
////////                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindByNome", this.Conn))
////////                    {
////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////////                        this.Cmd.Parameters.Clear();
////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Nome", SqlDbType.VarChar, 200)).Value = Param_Nome;
////////                        this.Cmd.Connection.Open();
////////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
////////                        {
////////                            if (!dr.HasRows) return null;
////////                            while (dr.Read())
////////                            {
////////                               arrayInfo.Add(GetDataFromReader( dr ));
////////                            }
////////                        }
////////                    }
////////                }
////////
////////                return arrayInfo;
////////
////////            }
////////            catch (SqlException e)
////////            {
////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
////////                return null;
////////            }
////////            catch (Exception e)
////////            {
////////                this._ErrorMessage = e.Message;
////////                return null;
////////            }
////////            finally
////////            {
////////                if (this.Conn != null)
////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////////            }
////////        }
////////
////////        #endregion
////////
////////
////////
////////        #region Selecionando dados da tabela através do campo "UserName" 
////////
////////        /// <summary> 
////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo UserName.
////////        /// </summary>
////////        /// <param name="Param_UserName">string</param>
////////        /// <returns>ArrayList</returns> 
////////        public ArrayList FindByUserName(
////////                               string Param_UserName )
////////        {
////////            ArrayList arrayInfo = new ArrayList();
////////            try
////////            {
////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////////                {
////////                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindByUserName", this.Conn))
////////                    {
////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////////                        this.Cmd.Parameters.Clear();
////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_UserName", SqlDbType.VarChar, 100)).Value = Param_UserName;
////////                        this.Cmd.Connection.Open();
////////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
////////                        {
////////                            if (!dr.HasRows) return null;
////////                            while (dr.Read())
////////                            {
////////                               arrayInfo.Add(GetDataFromReader( dr ));
////////                            }
////////                        }
////////                    }
////////                }
////////
////////                return arrayInfo;
////////
////////            }
////////            catch (SqlException e)
////////            {
////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
////////                return null;
////////            }
////////            catch (Exception e)
////////            {
////////                this._ErrorMessage = e.Message;
////////                return null;
////////            }
////////            finally
////////            {
////////                if (this.Conn != null)
////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////////            }
////////        }
////////
////////        #endregion
////////
////////
////////
////////        #region Selecionando dados da tabela através do campo "Password" 
////////
////////        /// <summary> 
////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Password.
////////        /// </summary>
////////        /// <param name="Param_Password">string</param>
////////        /// <returns>ArrayList</returns> 
////////        public ArrayList FindByPassword(
////////                               string Param_Password )
////////        {
////////            ArrayList arrayInfo = new ArrayList();
////////            try
////////            {
////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////////                {
////////                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindByPassword", this.Conn))
////////                    {
////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////////                        this.Cmd.Parameters.Clear();
////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Password", SqlDbType.VarChar, 100)).Value = Param_Password;
////////                        this.Cmd.Connection.Open();
////////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
////////                        {
////////                            if (!dr.HasRows) return null;
////////                            while (dr.Read())
////////                            {
////////                               arrayInfo.Add(GetDataFromReader( dr ));
////////                            }
////////                        }
////////                    }
////////                }
////////
////////                return arrayInfo;
////////
////////            }
////////            catch (SqlException e)
////////            {
////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
////////                return null;
////////            }
////////            catch (Exception e)
////////            {
////////                this._ErrorMessage = e.Message;
////////                return null;
////////            }
////////            finally
////////            {
////////                if (this.Conn != null)
////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////////            }
////////        }
////////
////////        #endregion
////////
////////
////////
////////        #region Selecionando dados da tabela através do campo "Cargo" 
////////
////////        /// <summary> 
////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Cargo.
////////        /// </summary>
////////        /// <param name="Param_Cargo">string</param>
////////        /// <returns>ArrayList</returns> 
////////        public ArrayList FindByCargo(
////////                               string Param_Cargo )
////////        {
////////            ArrayList arrayInfo = new ArrayList();
////////            try
////////            {
////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////////                {
////////                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindByCargo", this.Conn))
////////                    {
////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////////                        this.Cmd.Parameters.Clear();
////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Cargo", SqlDbType.VarChar, 50)).Value = Param_Cargo;
////////                        this.Cmd.Connection.Open();
////////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
////////                        {
////////                            if (!dr.HasRows) return null;
////////                            while (dr.Read())
////////                            {
////////                               arrayInfo.Add(GetDataFromReader( dr ));
////////                            }
////////                        }
////////                    }
////////                }
////////
////////                return arrayInfo;
////////
////////            }
////////            catch (SqlException e)
////////            {
////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
////////                return null;
////////            }
////////            catch (Exception e)
////////            {
////////                this._ErrorMessage = e.Message;
////////                return null;
////////            }
////////            finally
////////            {
////////                if (this.Conn != null)
////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////////            }
////////        }
////////
////////        #endregion
////////
////////
////////
////////        #region Selecionando dados da tabela através do campo "Situacao" 
////////
////////        /// <summary> 
////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Situacao.
////////        /// </summary>
////////        /// <param name="Param_Situacao">string</param>
////////        /// <returns>ArrayList</returns> 
////////        public ArrayList FindBySituacao(
////////                               string Param_Situacao )
////////        {
////////            ArrayList arrayInfo = new ArrayList();
////////            try
////////            {
////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////////                {
////////                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindBySituacao", this.Conn))
////////                    {
////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////////                        this.Cmd.Parameters.Clear();
////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Situacao", SqlDbType.VarChar, 1)).Value = Param_Situacao;
////////                        this.Cmd.Connection.Open();
////////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
////////                        {
////////                            if (!dr.HasRows) return null;
////////                            while (dr.Read())
////////                            {
////////                               arrayInfo.Add(GetDataFromReader( dr ));
////////                            }
////////                        }
////////                    }
////////                }
////////
////////                return arrayInfo;
////////
////////            }
////////            catch (SqlException e)
////////            {
////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
////////                return null;
////////            }
////////            catch (Exception e)
////////            {
////////                this._ErrorMessage = e.Message;
////////                return null;
////////            }
////////            finally
////////            {
////////                if (this.Conn != null)
////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////////            }
////////        }
////////
////////        #endregion
////////
////////
////////
////////        #region Selecionando dados da tabela através do campo "Modulo" 
////////
////////        /// <summary> 
////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Modulo.
////////        /// </summary>
////////        /// <param name="Param_Modulo">string</param>
////////        /// <returns>ArrayList</returns> 
////////        public ArrayList FindByModulo(
////////                               string Param_Modulo )
////////        {
////////            ArrayList arrayInfo = new ArrayList();
////////            try
////////            {
////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////////                {
////////                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindByModulo", this.Conn))
////////                    {
////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////////                        this.Cmd.Parameters.Clear();
////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Modulo", SqlDbType.VarChar, 1)).Value = Param_Modulo;
////////                        this.Cmd.Connection.Open();
////////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
////////                        {
////////                            if (!dr.HasRows) return null;
////////                            while (dr.Read())
////////                            {
////////                               arrayInfo.Add(GetDataFromReader( dr ));
////////                            }
////////                        }
////////                    }
////////                }
////////
////////                return arrayInfo;
////////
////////            }
////////            catch (SqlException e)
////////            {
////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
////////                return null;
////////            }
////////            catch (Exception e)
////////            {
////////                this._ErrorMessage = e.Message;
////////                return null;
////////            }
////////            finally
////////            {
////////                if (this.Conn != null)
////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////////            }
////////        }
////////
////////        #endregion
////////
////////
////////
////////        #region Selecionando dados da tabela através do campo "FkUa" 
////////
////////        /// <summary> 
////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo FkUa.
////////        /// </summary>
////////        /// <param name="Param_FkUa">int</param>
////////        /// <returns>ArrayList</returns> 
////////        public ArrayList FindByFkUa(
////////                               int Param_FkUa )
////////        {
////////            ArrayList arrayInfo = new ArrayList();
////////            try
////////            {
////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////////                {
////////                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindByFkUa", this.Conn))
////////                    {
////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////////                        this.Cmd.Parameters.Clear();
////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_FkUa", SqlDbType.Int)).Value = Param_FkUa;
////////                        this.Cmd.Connection.Open();
////////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
////////                        {
////////                            if (!dr.HasRows) return null;
////////                            while (dr.Read())
////////                            {
////////                               arrayInfo.Add(GetDataFromReader( dr ));
////////                            }
////////                        }
////////                    }
////////                }
////////
////////                return arrayInfo;
////////
////////            }
////////            catch (SqlException e)
////////            {
////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
////////                return null;
////////            }
////////            catch (Exception e)
////////            {
////////                this._ErrorMessage = e.Message;
////////                return null;
////////            }
////////            finally
////////            {
////////                if (this.Conn != null)
////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////////            }
////////        }
////////
////////        #endregion
////////
////////
////////
////////        #region Função GetDataFromReader
////////
////////        /// <summary> 
////////        /// Retorna um objeto UsuarioFields preenchido com os valores dos campos do SqlDataReader
////////        /// </summary>
////////        /// <param name="dr">SqlDataReader - Preenche o objeto UsuarioFields </param>
////////        /// <returns>UsuarioFields</returns>
////////        private UsuarioFields GetDataFromReader( SqlDataReader dr )
////////        {
////////            UsuarioFields infoFields = new UsuarioFields();
////////
////////            if (!dr.IsDBNull(0))
////////            { infoFields.idUsuario = dr.GetInt32(0); }
////////            else
////////            { infoFields.idUsuario = 0; }
////////
////////
////////
////////            if (!dr.IsDBNull(1))
////////            { infoFields.Nome = dr.GetString(1); }
////////            else
////////            { infoFields.Nome = string.Empty; }
////////
////////
////////
////////            if (!dr.IsDBNull(2))
////////            { infoFields.UserName = dr.GetString(2); }
////////            else
////////            { infoFields.UserName = string.Empty; }
////////
////////
////////
////////            if (!dr.IsDBNull(3))
////////            { infoFields.Password = dr.GetString(3); }
////////            else
////////            { infoFields.Password = string.Empty; }
////////
////////
////////
////////            if (!dr.IsDBNull(4))
////////            { infoFields.Cargo = dr.GetString(4); }
////////            else
////////            { infoFields.Cargo = string.Empty; }
////////
////////
////////
////////            if (!dr.IsDBNull(5))
////////            { infoFields.Situacao = dr.GetString(5); }
////////            else
////////            { infoFields.Situacao = string.Empty; }
////////
////////
////////
////////            if (!dr.IsDBNull(6))
////////            { infoFields.Modulo = dr.GetString(6); }
////////            else
////////            { infoFields.Modulo = string.Empty; }
////////
////////
////////
////////            if (!dr.IsDBNull(7))
////////            { infoFields.FkUa = dr.GetInt32(7); }
////////            else
////////            { infoFields.FkUa = 0; }
////////
////////
////////            return infoFields;
////////        }
////////        #endregion
////////
////////
////////
////////
////////
////////
////////
////////
////////
////////
////////
////////
////////
////////
////////
////////
////////
////////
////////
////////
////////
////////
////////        #region Função GetAllParameters
////////
////////        /// <summary> 
////////        /// Retorna um array de parâmetros com campos para atualização, seleção e inserção no banco de dados
////////        /// </summary>
////////        /// <param name="FieldInfo">Objeto UsuarioFields</param>
////////        /// <param name="Modo">Tipo de oepração a ser executada no banco de dados</param>
////////        /// <returns>SqlParameter[] - Array de parâmetros</returns> 
////////        private SqlParameter[] GetAllParameters( UsuarioFields FieldInfo, SQLMode Modo )
////////        {
////////            SqlParameter[] Parameters;
////////
////////            switch (Modo)
////////            {
////////                case SQLMode.Add:
////////                    Parameters = new SqlParameter[8];
////////                    for (int I = 0; I < Parameters.Length; I++)
////////                       Parameters[I] = new SqlParameter();
////////                    //Field idUsuario
////////                    Parameters[0].SqlDbType = SqlDbType.Int;
////////                    Parameters[0].Direction = ParameterDirection.Output;
////////                    Parameters[0].ParameterName = "@Param_idUsuario";
////////                    Parameters[0].Value = DBNull.Value;
////////
////////                    break;
////////
////////                case SQLMode.Update:
////////                    Parameters = new SqlParameter[8];
////////                    for (int I = 0; I < Parameters.Length; I++)
////////                       Parameters[I] = new SqlParameter();
////////                    //Field idUsuario
////////                    Parameters[0].SqlDbType = SqlDbType.Int;
////////                    Parameters[0].ParameterName = "@Param_idUsuario";
////////                    Parameters[0].Value = FieldInfo.idUsuario;
////////
////////                    break;
////////
////////                case SQLMode.SelectORDelete:
////////                    Parameters = new SqlParameter[1];
////////                    for (int I = 0; I < Parameters.Length; I++)
////////                       Parameters[I] = new SqlParameter();
////////                    //Field idUsuario
////////                    Parameters[0].SqlDbType = SqlDbType.Int;
////////                    Parameters[0].ParameterName = "@Param_idUsuario";
////////                    Parameters[0].Value = FieldInfo.idUsuario;
////////
////////                    return Parameters;
////////
////////                default:
////////                    Parameters = new SqlParameter[8];
////////                    for (int I = 0; I < Parameters.Length; I++)
////////                       Parameters[I] = new SqlParameter();
////////                    break;
////////            }
////////
////////            //Field Nome
////////            Parameters[1].SqlDbType = SqlDbType.VarChar;
////////            Parameters[1].ParameterName = "@Param_Nome";
////////            if (( FieldInfo.Nome == null ) || ( FieldInfo.Nome == string.Empty ))
////////            { Parameters[1].Value = DBNull.Value; }
////////            else
////////            { Parameters[1].Value = FieldInfo.Nome; }
////////            Parameters[1].Size = 200;
////////
////////            //Field UserName
////////            Parameters[2].SqlDbType = SqlDbType.VarChar;
////////            Parameters[2].ParameterName = "@Param_UserName";
////////            if (( FieldInfo.UserName == null ) || ( FieldInfo.UserName == string.Empty ))
////////            { Parameters[2].Value = DBNull.Value; }
////////            else
////////            { Parameters[2].Value = FieldInfo.UserName; }
////////            Parameters[2].Size = 100;
////////
////////            //Field Password
////////            Parameters[3].SqlDbType = SqlDbType.VarChar;
////////            Parameters[3].ParameterName = "@Param_Password";
////////            if (( FieldInfo.Password == null ) || ( FieldInfo.Password == string.Empty ))
////////            { Parameters[3].Value = DBNull.Value; }
////////            else
////////            { Parameters[3].Value = FieldInfo.Password; }
////////            Parameters[3].Size = 100;
////////
////////            //Field Cargo
////////            Parameters[4].SqlDbType = SqlDbType.VarChar;
////////            Parameters[4].ParameterName = "@Param_Cargo";
////////            if (( FieldInfo.Cargo == null ) || ( FieldInfo.Cargo == string.Empty ))
////////            { Parameters[4].Value = DBNull.Value; }
////////            else
////////            { Parameters[4].Value = FieldInfo.Cargo; }
////////            Parameters[4].Size = 50;
////////
////////            //Field Situacao
////////            Parameters[5].SqlDbType = SqlDbType.VarChar;
////////            Parameters[5].ParameterName = "@Param_Situacao";
////////            if (( FieldInfo.Situacao == null ) || ( FieldInfo.Situacao == string.Empty ))
////////            { Parameters[5].Value = DBNull.Value; }
////////            else
////////            { Parameters[5].Value = FieldInfo.Situacao; }
////////            Parameters[5].Size = 1;
////////
////////            //Field Modulo
////////            Parameters[6].SqlDbType = SqlDbType.VarChar;
////////            Parameters[6].ParameterName = "@Param_Modulo";
////////            if (( FieldInfo.Modulo == null ) || ( FieldInfo.Modulo == string.Empty ))
////////            { Parameters[6].Value = DBNull.Value; }
////////            else
////////            { Parameters[6].Value = FieldInfo.Modulo; }
////////            Parameters[6].Size = 1;
////////
////////            //Field FkUa
////////            Parameters[7].SqlDbType = SqlDbType.Int;
////////            Parameters[7].ParameterName = "@Param_FkUa";
////////            Parameters[7].Value = FieldInfo.FkUa;
////////
////////            return Parameters;
////////        }
////////        #endregion
////////
////////
////////
////////
////////
////////        #region IDisposable Members 
////////
////////        bool disposed = false;
////////
////////        public void Dispose()
////////        {
////////            Dispose(true);
////////            GC.SuppressFinalize(this);
////////        }
////////
////////        ~UsuarioControl() 
////////        { 
////////            Dispose(false); 
////////        }
////////
////////        private void Dispose(bool disposing) 
////////        {
////////            if (!this.disposed)
////////            {
////////                if (disposing) 
////////                {
////////                    if (this.Conn != null)
////////                        if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////////                }
////////            }
////////
////////        }
////////        #endregion 
////////
////////
////////
////////    }
////////
////////}
////////
////////
////////
////////
////////
//////////Projeto substituído ------------------------
//////////using System;
//////////using System.Collections;
//////////using System.Collections.Generic;
//////////using System.Data;
//////////using System.Data.SqlClient;
//////////using System.Configuration;
//////////using System.Text;
//////////
//////////namespace SWGPgen
//////////{
//////////
//////////
//////////    /// <summary> 
//////////    /// Tabela: Usuario  
//////////    /// Autor: DAL Creator .net 
//////////    /// Data de criação: 13/03/2012 21:19:06 
//////////    /// Descrição: Classe responsável pela perssitência de dados. Utiliza a classe "UsuarioFields". 
//////////    /// </summary> 
//////////    public class UsuarioControl : IDisposable 
//////////    {
//////////
//////////        #region String de conexão 
//////////        private string StrConnetionDB = "Data Source=DEKO-PC;Initial Catalog=swgp;User Id=sureg;Password=@sureg2012;";
//////////        #endregion
//////////
//////////
//////////        #region Propriedade que armazena erros de execução 
//////////        private string _ErrorMessage;
//////////        public string ErrorMessage { get { return _ErrorMessage; } }
//////////        #endregion
//////////
//////////
//////////        #region Objetos de conexão 
//////////        SqlConnection Conn;
//////////        SqlCommand Cmd;
//////////        SqlTransaction Tran;
//////////        #endregion
//////////
//////////
//////////        #region Funcões que retornam Conexões e Transações 
//////////
//////////        public SqlTransaction GetNewTransaction(SqlConnection connIn)
//////////        {
//////////            if (connIn.State != ConnectionState.Open)
//////////                connIn.Open();
//////////            SqlTransaction TranOut = connIn.BeginTransaction();
//////////            return TranOut;
//////////        }
//////////
//////////        public SqlConnection GetNewConnection()
//////////        {
//////////            return GetNewConnection(this.StrConnetionDB);
//////////        }
//////////
//////////        public SqlConnection GetNewConnection(string StringConnection)
//////////        {
//////////            SqlConnection connOut = new SqlConnection(StringConnection);
//////////            return connOut;
//////////        }
//////////
//////////        #endregion
//////////
//////////
//////////        #region enum SQLMode 
//////////        /// <summary>   
//////////        /// Representa o procedimento que está sendo executado na tabela.
//////////        /// </summary>
//////////        public enum SQLMode
//////////        {                     
//////////            /// <summary>
//////////            /// Adiciona registro na tabela.
//////////            /// </summary>
//////////            Add,
//////////            /// <summary>
//////////            /// Atualiza registro na tabela.
//////////            /// </summary>
//////////            Update,
//////////            /// <summary>
//////////            /// Excluir registro na tabela
//////////            /// </summary>
//////////            Delete,
//////////            /// <summary>
//////////            /// Exclui TODOS os registros da tabela.
//////////            /// </summary>
//////////            DeleteAll,
//////////            /// <summary>
//////////            /// Seleciona um registro na tabela.
//////////            /// </summary>
//////////            Select,
//////////            /// <summary>
//////////            /// Seleciona TODOS os registros da tabela.
//////////            /// </summary>
//////////            SelectAll,
//////////            /// <summary>
//////////            /// Excluir ou seleciona um registro na tabela.
//////////            /// </summary>
//////////            SelectORDelete
//////////        }
//////////        #endregion 
//////////
//////////
//////////        public UsuarioControl() {}
//////////
//////////
//////////        #region Inserindo dados na tabela 
//////////
//////////        /// <summary> 
//////////        /// Grava/Persiste um novo objeto UsuarioFields no banco de dados
//////////        /// </summary>
//////////        /// <param name="FieldInfo">Objeto UsuarioFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
//////////        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////////        public bool Add( ref UsuarioFields FieldInfo )
//////////        {
//////////            try
//////////            {
//////////                this.Conn = new SqlConnection(this.StrConnetionDB);
//////////                this.Conn.Open();
//////////                this.Tran = this.Conn.BeginTransaction();
//////////                this.Cmd = new SqlCommand("Proc_Usuario_Add", this.Conn, this.Tran);
//////////                this.Cmd.CommandType = CommandType.StoredProcedure;
//////////                this.Cmd.Parameters.Clear();
//////////                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
//////////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
//////////                this.Tran.Commit();
//////////                FieldInfo.idUsuario = (int)this.Cmd.Parameters["@Param_idUsuario"].Value;
//////////                return true;
//////////
//////////            }
//////////            catch (SqlException e)
//////////            {
//////////                this.Tran.Rollback();
//////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: {0}.",  e.Message);
//////////                return false;
//////////            }
//////////            catch (Exception e)
//////////            {
//////////                this.Tran.Rollback();
//////////                this._ErrorMessage = e.Message;
//////////                return false;
//////////            }
//////////            finally
//////////            {
//////////                if (this.Conn != null)
//////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////////                if (this.Cmd != null)
//////////                  this.Cmd.Dispose();
//////////            }
//////////        }
//////////
//////////        #endregion
//////////
//////////
//////////        #region Inserindo dados na tabela utilizando conexão e transação externa (compartilhada) 
//////////
//////////        /// <summary> 
//////////        /// Grava/Persiste um novo objeto UsuarioFields no banco de dados
//////////        /// </summary>
//////////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
//////////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
//////////        /// <param name="FieldInfo">Objeto UsuarioFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
//////////        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////////        public bool Add( SqlConnection ConnIn, SqlTransaction TranIn, ref UsuarioFields FieldInfo )
//////////        {
//////////            try
//////////            {
//////////                this.Cmd = new SqlCommand("Proc_Usuario_Add", ConnIn, TranIn);
//////////                this.Cmd.CommandType = CommandType.StoredProcedure;
//////////                this.Cmd.Parameters.Clear();
//////////                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
//////////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
//////////                FieldInfo.idUsuario = (int)this.Cmd.Parameters["@Param_idUsuario"].Value;
//////////                return true;
//////////
//////////            }
//////////            catch (SqlException e)
//////////            {
//////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: {0}.",  e.Message);
//////////                return false;
//////////            }
//////////            catch (Exception e)
//////////            {
//////////                this._ErrorMessage = e.Message;
//////////                return false;
//////////            }
//////////        }
//////////
//////////        #endregion
//////////
//////////
//////////        #region Editando dados na tabela 
//////////
//////////        /// <summary> 
//////////        /// Grava/Persiste as alterações em um objeto UsuarioFields no banco de dados
//////////        /// </summary>
//////////        /// <param name="FieldInfo">Objeto UsuarioFields a ser alterado.</param>
//////////        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////////        public bool Update( UsuarioFields FieldInfo )
//////////        {
//////////            try
//////////            {
//////////                this.Conn = new SqlConnection(this.StrConnetionDB);
//////////                this.Conn.Open();
//////////                this.Tran = this.Conn.BeginTransaction();
//////////                this.Cmd = new SqlCommand("Proc_Usuario_Update", this.Conn, this.Tran);
//////////                this.Cmd.CommandType = CommandType.StoredProcedure;
//////////                this.Cmd.Parameters.Clear();
//////////                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Update));
//////////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar atualizar registro!!");
//////////                this.Tran.Commit();
//////////                return true;
//////////            }
//////////            catch (SqlException e)
//////////            {
//////////                this.Tran.Rollback();
//////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: {0}.",  e.Message);
//////////                return false;
//////////            }
//////////            catch (Exception e)
//////////            {
//////////                this.Tran.Rollback();
//////////                this._ErrorMessage = e.Message;
//////////                return false;
//////////            }
//////////            finally
//////////            {
//////////                if (this.Conn != null)
//////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////////                if (this.Cmd != null)
//////////                  this.Cmd.Dispose();
//////////            }
//////////        }
//////////
//////////        #endregion
//////////
//////////
//////////        #region Editando dados na tabela utilizando conexão e transação externa (compartilhada) 
//////////
//////////        /// <summary> 
//////////        /// Grava/Persiste as alterações em um objeto UsuarioFields no banco de dados
//////////        /// </summary>
//////////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
//////////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
//////////        /// <param name="FieldInfo">Objeto UsuarioFields a ser alterado.</param>
//////////        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////////        public bool Update( SqlConnection ConnIn, SqlTransaction TranIn, UsuarioFields FieldInfo )
//////////        {
//////////            try
//////////            {
//////////                this.Cmd = new SqlCommand("Proc_Usuario_Update", ConnIn, TranIn);
//////////                this.Cmd.CommandType = CommandType.StoredProcedure;
//////////                this.Cmd.Parameters.Clear();
//////////                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Update));
//////////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar atualizar registro!!");
//////////                return true;
//////////            }
//////////            catch (SqlException e)
//////////            {
//////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: {0}.",  e.Message);
//////////                return false;
//////////            }
//////////            catch (Exception e)
//////////            {
//////////                this._ErrorMessage = e.Message;
//////////                return false;
//////////            }
//////////        }
//////////
//////////        #endregion
//////////
//////////
//////////        #region Excluindo todos os dados da tabela 
//////////
//////////        /// <summary> 
//////////        /// Exclui todos os registros da tabela
//////////        /// </summary>
//////////        /// <returns>"true" = registros excluidos com sucesso, "false" = erro ao tentar excluir registros (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////////        public bool DeleteAll()
//////////        {
//////////            try
//////////            {
//////////                this.Conn = new SqlConnection(this.StrConnetionDB);
//////////                this.Conn.Open();
//////////                this.Tran = this.Conn.BeginTransaction();
//////////                this.Cmd = new SqlCommand("Proc_Usuario_DeleteAll", this.Conn, this.Tran);
//////////                this.Cmd.CommandType = CommandType.StoredProcedure;
//////////                this.Cmd.Parameters.Clear();
//////////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
//////////                this.Tran.Commit();
//////////                return true;
//////////            }
//////////            catch (SqlException e)
//////////            {
//////////                this.Tran.Rollback();
//////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: {0}.",  e.Message);
//////////                return false;
//////////            }
//////////            catch (Exception e)
//////////            {
//////////                this.Tran.Rollback();
//////////                this._ErrorMessage = e.Message;
//////////                return false;
//////////            }
//////////            finally
//////////            {
//////////                if (this.Conn != null)
//////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////////                if (this.Cmd != null)
//////////                  this.Cmd.Dispose();
//////////            }
//////////        }
//////////
//////////        #endregion
//////////
//////////
//////////        #region Excluindo todos os dados da tabela utilizando conexão e transação externa (compartilhada)
//////////
//////////        /// <summary> 
//////////        /// Exclui todos os registros da tabela
//////////        /// </summary>
//////////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
//////////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
//////////        /// <returns>"true" = registros excluidos com sucesso, "false" = erro ao tentar excluir registros (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////////        public bool DeleteAll(SqlConnection ConnIn, SqlTransaction TranIn)
//////////        {
//////////            try
//////////            {
//////////                this.Cmd = new SqlCommand("Proc_Usuario_DeleteAll", ConnIn, TranIn);
//////////                this.Cmd.CommandType = CommandType.StoredProcedure;
//////////                this.Cmd.Parameters.Clear();
//////////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
//////////                return true;
//////////            }
//////////            catch (SqlException e)
//////////            {
//////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: {0}.",  e.Message);
//////////                return false;
//////////            }
//////////            catch (Exception e)
//////////            {
//////////                this._ErrorMessage = e.Message;
//////////                return false;
//////////            }
//////////        }
//////////
//////////        #endregion
//////////
//////////
//////////        #region Excluindo dados da tabela 
//////////
//////////        /// <summary> 
//////////        /// Exclui um registro da tabela no banco de dados
//////////        /// </summary>
//////////        /// <param name="FieldInfo">Objeto UsuarioFields a ser excluído.</param>
//////////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////////        public bool Delete( UsuarioFields FieldInfo )
//////////        {
//////////            return Delete(FieldInfo.idUsuario);
//////////        }
//////////
//////////        /// <summary> 
//////////        /// Exclui um registro da tabela no banco de dados
//////////        /// </summary>
//////////        /// <param name="Param_idUsuario">int</param>
//////////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////////        public bool Delete(
//////////                                     int Param_idUsuario)
//////////        {
//////////            try
//////////            {
//////////                this.Conn = new SqlConnection(this.StrConnetionDB);
//////////                this.Conn.Open();
//////////                this.Tran = this.Conn.BeginTransaction();
//////////                this.Cmd = new SqlCommand("Proc_Usuario_Delete", this.Conn, this.Tran);
//////////                this.Cmd.CommandType = CommandType.StoredProcedure;
//////////                this.Cmd.Parameters.Clear();
//////////                this.Cmd.Parameters.Add(new SqlParameter("@Param_idUsuario", SqlDbType.Int)).Value = Param_idUsuario;
//////////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
//////////                this.Tran.Commit();
//////////                return true;
//////////            }
//////////            catch (SqlException e)
//////////            {
//////////                this.Tran.Rollback();
//////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: {0}.",  e.Message);
//////////                return false;
//////////            }
//////////            catch (Exception e)
//////////            {
//////////                this.Tran.Rollback();
//////////                this._ErrorMessage = e.Message;
//////////                return false;
//////////            }
//////////            finally
//////////            {
//////////                if (this.Conn != null)
//////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////////                if (this.Cmd != null)
//////////                  this.Cmd.Dispose();
//////////            }
//////////        }
//////////
//////////        #endregion
//////////
//////////
//////////        #region Excluindo dados da tabela utilizando conexão e transação externa (compartilhada)
//////////
//////////        /// <summary> 
//////////        /// Exclui um registro da tabela no banco de dados
//////////        /// </summary>
//////////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
//////////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
//////////        /// <param name="FieldInfo">Objeto UsuarioFields a ser excluído.</param>
//////////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////////        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, UsuarioFields FieldInfo )
//////////        {
//////////            return Delete(ConnIn, TranIn, FieldInfo.idUsuario);
//////////        }
//////////
//////////        /// <summary> 
//////////        /// Exclui um registro da tabela no banco de dados
//////////        /// </summary>
//////////        /// <param name="Param_idUsuario">int</param>
//////////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
//////////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
//////////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////////        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, 
//////////                                     int Param_idUsuario)
//////////        {
//////////            try
//////////            {
//////////                this.Cmd = new SqlCommand("Proc_Usuario_Delete", ConnIn, TranIn);
//////////                this.Cmd.CommandType = CommandType.StoredProcedure;
//////////                this.Cmd.Parameters.Clear();
//////////                this.Cmd.Parameters.Add(new SqlParameter("@Param_idUsuario", SqlDbType.Int)).Value = Param_idUsuario;
//////////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
//////////                return true;
//////////            }
//////////            catch (SqlException e)
//////////            {
//////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: {0}.",  e.Message);
//////////                return false;
//////////            }
//////////            catch (Exception e)
//////////            {
//////////                this._ErrorMessage = e.Message;
//////////                return false;
//////////            }
//////////        }
//////////
//////////        #endregion
//////////
//////////
//////////        #region Selecionando um item da tabela 
//////////
//////////        /// <summary> 
//////////        /// Retorna um objeto UsuarioFields através da chave primária passada como parâmetro
//////////        /// </summary>
//////////        /// <param name="Param_idUsuario">int</param>
//////////        /// <returns>Objeto UsuarioFields</returns> 
//////////        public UsuarioFields GetItem(
//////////                                     int Param_idUsuario)
//////////        {
//////////            UsuarioFields infoFields = new UsuarioFields();
//////////            try
//////////            {
//////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////////                {
//////////                    using (this.Cmd = new SqlCommand("Proc_Usuario_Select", this.Conn))
//////////                    {
//////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////////                        this.Cmd.Parameters.Clear();
//////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_idUsuario", SqlDbType.Int)).Value = Param_idUsuario;
//////////                        this.Cmd.Connection.Open();
//////////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
//////////                        {
//////////                            if (!dr.HasRows) return null;
//////////                            if (dr.Read())
//////////                            {
//////////                               infoFields = GetDataFromReader( dr );
//////////                            }
//////////                        }
//////////                    }
//////////                 }
//////////
//////////                 return infoFields;
//////////
//////////            }
//////////            catch (SqlException e)
//////////            {
//////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
//////////                return null;
//////////            }
//////////            catch (Exception e)
//////////            {
//////////                this._ErrorMessage = e.Message;
//////////                return null;
//////////            }
//////////            finally
//////////            {
//////////                if (this.Conn != null)
//////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////////            }
//////////        }
//////////
//////////        #endregion
//////////
//////////
//////////        #region Selecionando todos os dados da tabela 
//////////
//////////        /// <summary> 
//////////        /// Seleciona todos os registro da tabela e preenche um ArrayList com o objeto UsuarioFields.
//////////        /// </summary>
//////////        /// <returns>List de objetos UsuarioFields</returns> 
//////////        public List<UsuarioFields> GetAll()
//////////        {
//////////            List<UsuarioFields> arrayInfo = new List<UsuarioFields>();
//////////            try
//////////            {
//////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////////                {
//////////                    using (this.Cmd = new SqlCommand("Proc_Usuario_GetAll", this.Conn))
//////////                    {
//////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////////                        this.Cmd.Parameters.Clear();
//////////                        this.Cmd.Connection.Open();
//////////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
//////////                        {
//////////                           if (!dr.HasRows) return null;
//////////                           while (dr.Read())
//////////                           {
//////////                              arrayInfo.Add(GetDataFromReader( dr ));
//////////                           }
//////////                        }
//////////                    }
//////////                }
//////////
//////////                return arrayInfo;
//////////
//////////            }
//////////            catch (SqlException e)
//////////            {
//////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
//////////                return null;
//////////            }
//////////            catch (Exception e)
//////////            {
//////////                this._ErrorMessage = e.Message;
//////////                return null;
//////////            }
//////////            finally
//////////            {
//////////                if (this.Conn != null)
//////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////////            }
//////////        }
//////////
//////////        #endregion
//////////
//////////
//////////        #region Contando os dados da tabela 
//////////
//////////        /// <summary> 
//////////        /// Retorna o total de registros contidos na tabela
//////////        /// </summary>
//////////        /// <returns>int</returns> 
//////////        public int CountAll()
//////////        {
//////////            try
//////////            {
//////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////////                {
//////////                    using (this.Cmd = new SqlCommand("Proc_Usuario_CountAll", this.Conn))
//////////                    {
//////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////////                        this.Cmd.Parameters.Clear();
//////////                        this.Cmd.Connection.Open();
//////////                        object CountRegs = this.Cmd.ExecuteScalar();
//////////                        if (CountRegs == null)
//////////                        { return 0; }
//////////                        else
//////////                        { return (int)CountRegs; }
//////////                    }
//////////                }
//////////            }
//////////            catch (SqlException e)
//////////            {
//////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
//////////                return 0;
//////////            }
//////////            catch (Exception e)
//////////            {
//////////                this._ErrorMessage = e.Message;
//////////                return 0;
//////////            }
//////////            finally
//////////            {
//////////                if (this.Conn != null)
//////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////////            }
//////////        }
//////////
//////////        #endregion
//////////
//////////
//////////        #region Selecionando dados da tabela através do campo "Nome" 
//////////
//////////        /// <summary> 
//////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Nome.
//////////        /// </summary>
//////////        /// <param name="Param_Nome">string</param>
//////////        /// <returns>ArrayList</returns> 
//////////        public ArrayList FindByNome(
//////////                               string Param_Nome )
//////////        {
//////////            ArrayList arrayInfo = new ArrayList();
//////////            try
//////////            {
//////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////////                {
//////////                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindByNome", this.Conn))
//////////                    {
//////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////////                        this.Cmd.Parameters.Clear();
//////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Nome", SqlDbType.VarChar, 200)).Value = Param_Nome;
//////////                        this.Cmd.Connection.Open();
//////////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
//////////                        {
//////////                            if (!dr.HasRows) return null;
//////////                            while (dr.Read())
//////////                            {
//////////                               arrayInfo.Add(GetDataFromReader( dr ));
//////////                            }
//////////                        }
//////////                    }
//////////                }
//////////
//////////                return arrayInfo;
//////////
//////////            }
//////////            catch (SqlException e)
//////////            {
//////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
//////////                return null;
//////////            }
//////////            catch (Exception e)
//////////            {
//////////                this._ErrorMessage = e.Message;
//////////                return null;
//////////            }
//////////            finally
//////////            {
//////////                if (this.Conn != null)
//////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////////            }
//////////        }
//////////
//////////        #endregion
//////////
//////////
//////////
//////////        #region Selecionando dados da tabela através do campo "UserName" 
//////////
//////////        /// <summary> 
//////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo UserName.
//////////        /// </summary>
//////////        /// <param name="Param_UserName">string</param>
//////////        /// <returns>ArrayList</returns> 
//////////        public ArrayList FindByUserName(
//////////                               string Param_UserName )
//////////        {
//////////            ArrayList arrayInfo = new ArrayList();
//////////            try
//////////            {
//////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////////                {
//////////                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindByUserName", this.Conn))
//////////                    {
//////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////////                        this.Cmd.Parameters.Clear();
//////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_UserName", SqlDbType.VarChar, 100)).Value = Param_UserName;
//////////                        this.Cmd.Connection.Open();
//////////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
//////////                        {
//////////                            if (!dr.HasRows) return null;
//////////                            while (dr.Read())
//////////                            {
//////////                               arrayInfo.Add(GetDataFromReader( dr ));
//////////                            }
//////////                        }
//////////                    }
//////////                }
//////////
//////////                return arrayInfo;
//////////
//////////            }
//////////            catch (SqlException e)
//////////            {
//////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
//////////                return null;
//////////            }
//////////            catch (Exception e)
//////////            {
//////////                this._ErrorMessage = e.Message;
//////////                return null;
//////////            }
//////////            finally
//////////            {
//////////                if (this.Conn != null)
//////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////////            }
//////////        }
//////////
//////////        #endregion
//////////
//////////
//////////
//////////        #region Selecionando dados da tabela através do campo "Password" 
//////////
//////////        /// <summary> 
//////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Password.
//////////        /// </summary>
//////////        /// <param name="Param_Password">string</param>
//////////        /// <returns>ArrayList</returns> 
//////////        public ArrayList FindByPassword(
//////////                               string Param_Password )
//////////        {
//////////            ArrayList arrayInfo = new ArrayList();
//////////            try
//////////            {
//////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////////                {
//////////                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindByPassword", this.Conn))
//////////                    {
//////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////////                        this.Cmd.Parameters.Clear();
//////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Password", SqlDbType.VarChar, 100)).Value = Param_Password;
//////////                        this.Cmd.Connection.Open();
//////////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
//////////                        {
//////////                            if (!dr.HasRows) return null;
//////////                            while (dr.Read())
//////////                            {
//////////                               arrayInfo.Add(GetDataFromReader( dr ));
//////////                            }
//////////                        }
//////////                    }
//////////                }
//////////
//////////                return arrayInfo;
//////////
//////////            }
//////////            catch (SqlException e)
//////////            {
//////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
//////////                return null;
//////////            }
//////////            catch (Exception e)
//////////            {
//////////                this._ErrorMessage = e.Message;
//////////                return null;
//////////            }
//////////            finally
//////////            {
//////////                if (this.Conn != null)
//////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////////            }
//////////        }
//////////
//////////        #endregion
//////////
//////////
//////////
//////////        #region Selecionando dados da tabela através do campo "Cargo" 
//////////
//////////        /// <summary> 
//////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Cargo.
//////////        /// </summary>
//////////        /// <param name="Param_Cargo">string</param>
//////////        /// <returns>ArrayList</returns> 
//////////        public ArrayList FindByCargo(
//////////                               string Param_Cargo )
//////////        {
//////////            ArrayList arrayInfo = new ArrayList();
//////////            try
//////////            {
//////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////////                {
//////////                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindByCargo", this.Conn))
//////////                    {
//////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////////                        this.Cmd.Parameters.Clear();
//////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Cargo", SqlDbType.VarChar, 50)).Value = Param_Cargo;
//////////                        this.Cmd.Connection.Open();
//////////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
//////////                        {
//////////                            if (!dr.HasRows) return null;
//////////                            while (dr.Read())
//////////                            {
//////////                               arrayInfo.Add(GetDataFromReader( dr ));
//////////                            }
//////////                        }
//////////                    }
//////////                }
//////////
//////////                return arrayInfo;
//////////
//////////            }
//////////            catch (SqlException e)
//////////            {
//////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
//////////                return null;
//////////            }
//////////            catch (Exception e)
//////////            {
//////////                this._ErrorMessage = e.Message;
//////////                return null;
//////////            }
//////////            finally
//////////            {
//////////                if (this.Conn != null)
//////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////////            }
//////////        }
//////////
//////////        #endregion
//////////
//////////
//////////
//////////        #region Selecionando dados da tabela através do campo "FkUa" 
//////////
//////////        /// <summary> 
//////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo FkUa.
//////////        /// </summary>
//////////        /// <param name="Param_FkUa">int</param>
//////////        /// <returns>ArrayList</returns> 
//////////        public ArrayList FindByFkUa(
//////////                               int Param_FkUa )
//////////        {
//////////            ArrayList arrayInfo = new ArrayList();
//////////            try
//////////            {
//////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////////                {
//////////                    using (this.Cmd = new SqlCommand("Proc_Usuario_FindByFkUa", this.Conn))
//////////                    {
//////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////////                        this.Cmd.Parameters.Clear();
//////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_FkUa", SqlDbType.Int)).Value = Param_FkUa;
//////////                        this.Cmd.Connection.Open();
//////////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
//////////                        {
//////////                            if (!dr.HasRows) return null;
//////////                            while (dr.Read())
//////////                            {
//////////                               arrayInfo.Add(GetDataFromReader( dr ));
//////////                            }
//////////                        }
//////////                    }
//////////                }
//////////
//////////                return arrayInfo;
//////////
//////////            }
//////////            catch (SqlException e)
//////////            {
//////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
//////////                return null;
//////////            }
//////////            catch (Exception e)
//////////            {
//////////                this._ErrorMessage = e.Message;
//////////                return null;
//////////            }
//////////            finally
//////////            {
//////////                if (this.Conn != null)
//////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////////            }
//////////        }
//////////
//////////        #endregion
//////////
//////////
//////////
//////////        #region Função GetDataFromReader
//////////
//////////        /// <summary> 
//////////        /// Retorna um objeto UsuarioFields preenchido com os valores dos campos do SqlDataReader
//////////        /// </summary>
//////////        /// <param name="dr">SqlDataReader - Preenche o objeto UsuarioFields </param>
//////////        /// <returns>UsuarioFields</returns>
//////////        private UsuarioFields GetDataFromReader( SqlDataReader dr )
//////////        {
//////////            UsuarioFields infoFields = new UsuarioFields();
//////////
//////////            if (!dr.IsDBNull(0))
//////////            { infoFields.idUsuario = dr.GetInt32(0); }
//////////            else
//////////            { infoFields.idUsuario = 0; }
//////////
//////////
//////////
//////////            if (!dr.IsDBNull(1))
//////////            { infoFields.Nome = dr.GetString(1); }
//////////            else
//////////            { infoFields.Nome = string.Empty; }
//////////
//////////
//////////
//////////            if (!dr.IsDBNull(2))
//////////            { infoFields.UserName = dr.GetString(2); }
//////////            else
//////////            { infoFields.UserName = string.Empty; }
//////////
//////////
//////////
//////////            if (!dr.IsDBNull(3))
//////////            { infoFields.Password = dr.GetString(3); }
//////////            else
//////////            { infoFields.Password = string.Empty; }
//////////
//////////
//////////
//////////            if (!dr.IsDBNull(4))
//////////            { infoFields.Cargo = dr.GetString(4); }
//////////            else
//////////            { infoFields.Cargo = string.Empty; }
//////////
//////////
//////////
//////////            if (!dr.IsDBNull(5))
//////////            { infoFields.FkUa = dr.GetInt32(5); }
//////////            else
//////////            { infoFields.FkUa = 0; }
//////////
//////////
//////////            return infoFields;
//////////        }
//////////        #endregion
//////////
//////////
//////////
//////////
//////////
//////////
//////////
//////////
//////////
//////////
//////////
//////////
//////////
//////////
//////////
//////////
//////////
//////////
//////////        #region Função GetAllParameters
//////////
//////////        /// <summary> 
//////////        /// Retorna um array de parâmetros com campos para atualização, seleção e inserção no banco de dados
//////////        /// </summary>
//////////        /// <param name="FieldInfo">Objeto UsuarioFields</param>
//////////        /// <param name="Modo">Tipo de oepração a ser executada no banco de dados</param>
//////////        /// <returns>SqlParameter[] - Array de parâmetros</returns> 
//////////        private SqlParameter[] GetAllParameters( UsuarioFields FieldInfo, SQLMode Modo )
//////////        {
//////////            SqlParameter[] Parameters;
//////////
//////////            switch (Modo)
//////////            {
//////////                case SQLMode.Add:
//////////                    Parameters = new SqlParameter[6];
//////////                    for (int I = 0; I < Parameters.Length; I++)
//////////                       Parameters[I] = new SqlParameter();
//////////                    //Field idUsuario
//////////                    Parameters[0].SqlDbType = SqlDbType.Int;
//////////                    Parameters[0].Direction = ParameterDirection.Output;
//////////                    Parameters[0].ParameterName = "@Param_idUsuario";
//////////                    Parameters[0].Value = DBNull.Value;
//////////
//////////                    break;
//////////
//////////                case SQLMode.Update:
//////////                    Parameters = new SqlParameter[6];
//////////                    for (int I = 0; I < Parameters.Length; I++)
//////////                       Parameters[I] = new SqlParameter();
//////////                    //Field idUsuario
//////////                    Parameters[0].SqlDbType = SqlDbType.Int;
//////////                    Parameters[0].ParameterName = "@Param_idUsuario";
//////////                    Parameters[0].Value = FieldInfo.idUsuario;
//////////
//////////                    break;
//////////
//////////                case SQLMode.SelectORDelete:
//////////                    Parameters = new SqlParameter[1];
//////////                    for (int I = 0; I < Parameters.Length; I++)
//////////                       Parameters[I] = new SqlParameter();
//////////                    //Field idUsuario
//////////                    Parameters[0].SqlDbType = SqlDbType.Int;
//////////                    Parameters[0].ParameterName = "@Param_idUsuario";
//////////                    Parameters[0].Value = FieldInfo.idUsuario;
//////////
//////////                    return Parameters;
//////////
//////////                default:
//////////                    Parameters = new SqlParameter[6];
//////////                    for (int I = 0; I < Parameters.Length; I++)
//////////                       Parameters[I] = new SqlParameter();
//////////                    break;
//////////            }
//////////
//////////            //Field Nome
//////////            Parameters[1].SqlDbType = SqlDbType.VarChar;
//////////            Parameters[1].ParameterName = "@Param_Nome";
//////////            if (( FieldInfo.Nome == null ) || ( FieldInfo.Nome == string.Empty ))
//////////            { Parameters[1].Value = DBNull.Value; }
//////////            else
//////////            { Parameters[1].Value = FieldInfo.Nome; }
//////////            Parameters[1].Size = 200;
//////////
//////////            //Field UserName
//////////            Parameters[2].SqlDbType = SqlDbType.VarChar;
//////////            Parameters[2].ParameterName = "@Param_UserName";
//////////            if (( FieldInfo.UserName == null ) || ( FieldInfo.UserName == string.Empty ))
//////////            { Parameters[2].Value = DBNull.Value; }
//////////            else
//////////            { Parameters[2].Value = FieldInfo.UserName; }
//////////            Parameters[2].Size = 100;
//////////
//////////            //Field Password
//////////            Parameters[3].SqlDbType = SqlDbType.VarChar;
//////////            Parameters[3].ParameterName = "@Param_Password";
//////////            if (( FieldInfo.Password == null ) || ( FieldInfo.Password == string.Empty ))
//////////            { Parameters[3].Value = DBNull.Value; }
//////////            else
//////////            { Parameters[3].Value = FieldInfo.Password; }
//////////            Parameters[3].Size = 100;
//////////
//////////            //Field Cargo
//////////            Parameters[4].SqlDbType = SqlDbType.VarChar;
//////////            Parameters[4].ParameterName = "@Param_Cargo";
//////////            if (( FieldInfo.Cargo == null ) || ( FieldInfo.Cargo == string.Empty ))
//////////            { Parameters[4].Value = DBNull.Value; }
//////////            else
//////////            { Parameters[4].Value = FieldInfo.Cargo; }
//////////            Parameters[4].Size = 50;
//////////
//////////            //Field FkUa
//////////            Parameters[5].SqlDbType = SqlDbType.Int;
//////////            Parameters[5].ParameterName = "@Param_FkUa";
//////////            Parameters[5].Value = FieldInfo.FkUa;
//////////
//////////            return Parameters;
//////////        }
//////////        #endregion
//////////
//////////
//////////
//////////
//////////
//////////        #region IDisposable Members 
//////////
//////////        bool disposed = false;
//////////
//////////        public void Dispose()
//////////        {
//////////            Dispose(true);
//////////            GC.SuppressFinalize(this);
//////////        }
//////////
//////////        ~UsuarioControl() 
//////////        { 
//////////            Dispose(false); 
//////////        }
//////////
//////////        private void Dispose(bool disposing) 
//////////        {
//////////            if (!this.disposed)
//////////            {
//////////                if (disposing) 
//////////                {
//////////                    if (this.Conn != null)
//////////                        if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////////                }
//////////            }
//////////
//////////        }
//////////        #endregion 
//////////
//////////
//////////
//////////    }
//////////
//////////}
