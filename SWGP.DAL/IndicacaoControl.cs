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
    /// Tabela: Indicacao  
    /// Autor: DAL Creator .net 
    /// Data de criação: 02/04/2012 21:22:09 
    /// Descrição: Classe responsável pela perssitência de dados. Utiliza a classe "IndicacaoFields". 
    /// </summary> 
    public class IndicacaoControl : IDisposable 
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


        public IndicacaoControl() {}


        #region Inserindo dados na tabela 

        /// <summary> 
        /// Grava/Persiste um novo objeto IndicacaoFields no banco de dados
        /// </summary>
        /// <param name="FieldInfo">Objeto IndicacaoFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Add( ref IndicacaoFields FieldInfo )
        {
            try
            {
                this.Conn = new SqlConnection(this.StrConnetionDB);
                this.Conn.Open();
                this.Tran = this.Conn.BeginTransaction();
                this.Cmd = new SqlCommand("Proc_Indicacao_Add", this.Conn, this.Tran);
                this.Cmd.CommandType = CommandType.StoredProcedure;
                this.Cmd.Parameters.Clear();
                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
                this.Tran.Commit();
                FieldInfo.idIndicacao = (int)this.Cmd.Parameters["@Param_idIndicacao"].Value;
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

        public DataSet GetAllIndicacaoByModuloUsuario(UsuarioFields usuario, TipoIndicacao tipoIndicacao)
        {
            DataSet dsIndicacao = new DataSet();
            try
            {
                SqlConnection Conn = new SqlConnection(this.StrConnetionDB);

                string query = GetQueryByModuloUser(usuario, tipoIndicacao);

                Conn.Open();
                DataTable dt = new DataTable();
                SqlCommand Cmd = new SqlCommand(query, Conn);
                Cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(Cmd);
                da.Fill(dsIndicacao, "Usuario");


                return dsIndicacao;

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

        public int CountAllIndicacaoByModuloUsuario(UsuarioFields usuario)
        {
            DataSet dsIndicacao = new DataSet();
            try
            {
                SqlConnection Conn = new SqlConnection(this.StrConnetionDB);
                StringBuilder query = new StringBuilder();
                query.Append(" select count(*)");
                query.Append(" from indicacao i inner join usuario u on i.idUsuarioRecebe = u.idUsuario");
                query.Append(" Where 1 = 1 ");

                if (usuario.Modulo == "U")
                    query.AppendFormat(" And i.idUsuarioRecebe = {0}", usuario.idUsuario);
                else if (usuario.Modulo == "M")
                    query.AppendFormat("  And u.FkUA = {0}", usuario.FkUa);

                Conn.Open();
                DataTable dt = new DataTable();
                SqlCommand Cmd = new SqlCommand(query.ToString(), Conn);
                Cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(Cmd);
                da.Fill(dsIndicacao, "Indicacao");

                dt = dsIndicacao.Tables[0];
                return Convert.ToInt32(dt.Rows[0][0]);

            }
            catch (SqlException e)
            {
                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.", e.Message);
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

        public enum TipoIndicacao
        {
            Recebida,
            Indicada
        }

        private string GetQueryByModuloUser(UsuarioFields usuario, TipoIndicacao tipoIndicacao)
        {

            if (usuario == null)
                throw new Exception("Modulo de usuário não encontrado.");

            StringBuilder query = new StringBuilder();

            if (tipoIndicacao == TipoIndicacao.Indicada)
            {
                query.Append(" select DISTINCT i.idIndicacao, i.Nome,i.Telefone,(select us.UserName from Usuario us where us.idUsuario =  i.idUsuarioRecebe) as UserName ");
                query.Append(" from indicacao i inner join usuario u on i.idUsuarioIndica = u.idUsuario ");
                query.Append(" Where 1 = 1 ");

                if (usuario.Modulo == "U")
                    query.AppendFormat(" And i.idUsuarioIndica = {0}", usuario.idUsuario);
                else if (usuario.Modulo == "M")
                    query.AppendFormat("  And u.FkUA = {0}", usuario.FkUa);

            }

            if (tipoIndicacao == TipoIndicacao.Recebida)
            {
                query.Append(" select DISTINCT i.idIndicacao, i.Nome,i.Telefone, (select us.UserName from Usuario us where us.idUsuario =  i.idUsuarioIndica) as UserName");
                query.Append(" from indicacao i inner join usuario u on i.idUsuarioRecebe = u.idUsuario");
                query.Append(" Where 1 = 1 ");

                if (usuario.Modulo == "U")
                    query.AppendFormat(" And i.idUsuarioRecebe = {0}", usuario.idUsuario);
                else if (usuario.Modulo == "M")
                    query.AppendFormat("  And u.FkUA = {0}", usuario.FkUa);
            }

            return query.ToString();
        }


        #region Inserindo dados na tabela utilizando conexão e transação externa (compartilhada) 

        /// <summary> 
        /// Grava/Persiste um novo objeto IndicacaoFields no banco de dados
        /// </summary>
        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
        /// <param name="FieldInfo">Objeto IndicacaoFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Add( SqlConnection ConnIn, SqlTransaction TranIn, ref IndicacaoFields FieldInfo )
        {
            try
            {
                this.Cmd = new SqlCommand("Proc_Indicacao_Add", ConnIn, TranIn);
                this.Cmd.CommandType = CommandType.StoredProcedure;
                this.Cmd.Parameters.Clear();
                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
                FieldInfo.idIndicacao = (int)this.Cmd.Parameters["@Param_idIndicacao"].Value;
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
        /// Grava/Persiste as alterações em um objeto IndicacaoFields no banco de dados
        /// </summary>
        /// <param name="FieldInfo">Objeto IndicacaoFields a ser alterado.</param>
        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Update( IndicacaoFields FieldInfo )
        {
            try
            {
                this.Conn = new SqlConnection(this.StrConnetionDB);
                this.Conn.Open();
                this.Tran = this.Conn.BeginTransaction();
                this.Cmd = new SqlCommand("Proc_Indicacao_Update", this.Conn, this.Tran);
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
        /// Grava/Persiste as alterações em um objeto IndicacaoFields no banco de dados
        /// </summary>
        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
        /// <param name="FieldInfo">Objeto IndicacaoFields a ser alterado.</param>
        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Update( SqlConnection ConnIn, SqlTransaction TranIn, IndicacaoFields FieldInfo )
        {
            try
            {
                this.Cmd = new SqlCommand("Proc_Indicacao_Update", ConnIn, TranIn);
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
                this.Cmd = new SqlCommand("Proc_Indicacao_DeleteAll", this.Conn, this.Tran);
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
                this.Cmd = new SqlCommand("Proc_Indicacao_DeleteAll", ConnIn, TranIn);
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
        /// <param name="FieldInfo">Objeto IndicacaoFields a ser excluído.</param>
        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Delete( IndicacaoFields FieldInfo )
        {
            return Delete(FieldInfo.idIndicacao);
        }

        /// <summary> 
        /// Exclui um registro da tabela no banco de dados
        /// </summary>
        /// <param name="Param_idIndicacao">int</param>
        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Delete(
                                     int Param_idIndicacao)
        {
            try
            {
                this.Conn = new SqlConnection(this.StrConnetionDB);
                this.Conn.Open();
                this.Tran = this.Conn.BeginTransaction();
                this.Cmd = new SqlCommand("Proc_Indicacao_Delete", this.Conn, this.Tran);
                this.Cmd.CommandType = CommandType.StoredProcedure;
                this.Cmd.Parameters.Clear();
                this.Cmd.Parameters.Add(new SqlParameter("@Param_idIndicacao", SqlDbType.Int)).Value = Param_idIndicacao;
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
        /// <param name="FieldInfo">Objeto IndicacaoFields a ser excluído.</param>
        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, IndicacaoFields FieldInfo )
        {
            return Delete(ConnIn, TranIn, FieldInfo.idIndicacao);
        }

        /// <summary> 
        /// Exclui um registro da tabela no banco de dados
        /// </summary>
        /// <param name="Param_idIndicacao">int</param>
        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, 
                                     int Param_idIndicacao)
        {
            try
            {
                this.Cmd = new SqlCommand("Proc_Indicacao_Delete", ConnIn, TranIn);
                this.Cmd.CommandType = CommandType.StoredProcedure;
                this.Cmd.Parameters.Clear();
                this.Cmd.Parameters.Add(new SqlParameter("@Param_idIndicacao", SqlDbType.Int)).Value = Param_idIndicacao;
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
        /// Retorna um objeto IndicacaoFields através da chave primária passada como parâmetro
        /// </summary>
        /// <param name="Param_idIndicacao">int</param>
        /// <returns>Objeto IndicacaoFields</returns> 
        public IndicacaoFields GetItem(
                                     int Param_idIndicacao)
        {
            IndicacaoFields infoFields = new IndicacaoFields();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Indicacao_Select", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_idIndicacao", SqlDbType.Int)).Value = Param_idIndicacao;
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
        /// Seleciona todos os registro da tabela e preenche um ArrayList com o objeto IndicacaoFields.
        /// </summary>
        /// <returns>List de objetos IndicacaoFields</returns> 
        public List<IndicacaoFields> GetAll()
        {
            List<IndicacaoFields> arrayInfo = new List<IndicacaoFields>();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Indicacao_GetAll", this.Conn))
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
                    using (this.Cmd = new SqlCommand("Proc_Indicacao_CountAll", this.Conn))
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


        #region Selecionando dados da tabela através do campo "Nome" 

        /// <summary> 
        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Nome.
        /// </summary>
        /// <param name="Param_Nome">string</param>
        /// <returns>IndicacaoFields</returns> 
        public IndicacaoFields FindByNome(
                               string Param_Nome )
        {
            IndicacaoFields infoFields = new IndicacaoFields();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByNome", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Nome", SqlDbType.VarChar, 150)).Value = Param_Nome;
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



        #region Selecionando dados da tabela através do campo "Telefone" 

        /// <summary> 
        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Telefone.
        /// </summary>
        /// <param name="Param_Telefone">string</param>
        /// <returns>IndicacaoFields</returns> 
        public IndicacaoFields FindByTelefone(
                               string Param_Telefone )
        {
            IndicacaoFields infoFields = new IndicacaoFields();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByTelefone", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Telefone", SqlDbType.VarChar, 50)).Value = Param_Telefone;
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



        #region Selecionando dados da tabela através do campo "Endereco" 

        /// <summary> 
        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Endereco.
        /// </summary>
        /// <param name="Param_Endereco">string</param>
        /// <returns>IndicacaoFields</returns> 
        public IndicacaoFields FindByEndereco(
                               string Param_Endereco )
        {
            IndicacaoFields infoFields = new IndicacaoFields();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByEndereco", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Endereco", SqlDbType.VarChar, 150)).Value = Param_Endereco;
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



        #region Selecionando dados da tabela através do campo "Bairro" 

        /// <summary> 
        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Bairro.
        /// </summary>
        /// <param name="Param_Bairro">string</param>
        /// <returns>IndicacaoFields</returns> 
        public IndicacaoFields FindByBairro(
                               string Param_Bairro )
        {
            IndicacaoFields infoFields = new IndicacaoFields();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByBairro", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Bairro", SqlDbType.VarChar, 150)).Value = Param_Bairro;
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



        #region Selecionando dados da tabela através do campo "Cidade" 

        /// <summary> 
        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Cidade.
        /// </summary>
        /// <param name="Param_Cidade">string</param>
        /// <returns>IndicacaoFields</returns> 
        public IndicacaoFields FindByCidade(
                               string Param_Cidade )
        {
            IndicacaoFields infoFields = new IndicacaoFields();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByCidade", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Cidade", SqlDbType.VarChar, 150)).Value = Param_Cidade;
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



        #region Selecionando dados da tabela através do campo "Estado" 

        /// <summary> 
        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Estado.
        /// </summary>
        /// <param name="Param_Estado">string</param>
        /// <returns>IndicacaoFields</returns> 
        public IndicacaoFields FindByEstado(
                               string Param_Estado )
        {
            IndicacaoFields infoFields = new IndicacaoFields();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByEstado", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Estado", SqlDbType.VarChar, 150)).Value = Param_Estado;
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



        #region Selecionando dados da tabela através do campo "idUsuarioRecebe" 

        /// <summary> 
        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo idUsuarioRecebe.
        /// </summary>
        /// <param name="Param_idUsuarioRecebe">int</param>
        /// <returns>IndicacaoFields</returns> 
        public IndicacaoFields FindByidUsuarioRecebe(
                               int Param_idUsuarioRecebe )
        {
            IndicacaoFields infoFields = new IndicacaoFields();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByidUsuarioRecebe", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_idUsuarioRecebe", SqlDbType.Int)).Value = Param_idUsuarioRecebe;
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



        #region Selecionando dados da tabela através do campo "idUsuarioIndica" 

        /// <summary> 
        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo idUsuarioIndica.
        /// </summary>
        /// <param name="Param_idUsuarioIndica">int</param>
        /// <returns>IndicacaoFields</returns> 
        public IndicacaoFields FindByidUsuarioIndica(
                               int Param_idUsuarioIndica )
        {
            IndicacaoFields infoFields = new IndicacaoFields();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByidUsuarioIndica", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_idUsuarioIndica", SqlDbType.Int)).Value = Param_idUsuarioIndica;
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
        /// Retorna um objeto IndicacaoFields preenchido com os valores dos campos do SqlDataReader
        /// </summary>
        /// <param name="dr">SqlDataReader - Preenche o objeto IndicacaoFields </param>
        /// <returns>IndicacaoFields</returns>
        private IndicacaoFields GetDataFromReader( SqlDataReader dr )
        {
            IndicacaoFields infoFields = new IndicacaoFields();

            if (!dr.IsDBNull(0))
            { infoFields.idIndicacao = dr.GetInt32(0); }
            else
            { infoFields.idIndicacao = 0; }



            if (!dr.IsDBNull(1))
            { infoFields.Nome = dr.GetString(1); }
            else
            { infoFields.Nome = string.Empty; }



            if (!dr.IsDBNull(2))
            { infoFields.Telefone = dr.GetString(2); }
            else
            { infoFields.Telefone = string.Empty; }



            if (!dr.IsDBNull(3))
            { infoFields.Endereco = dr.GetString(3); }
            else
            { infoFields.Endereco = string.Empty; }



            if (!dr.IsDBNull(4))
            { infoFields.Bairro = dr.GetString(4); }
            else
            { infoFields.Bairro = string.Empty; }



            if (!dr.IsDBNull(5))
            { infoFields.Cidade = dr.GetString(5); }
            else
            { infoFields.Cidade = string.Empty; }



            if (!dr.IsDBNull(6))
            { infoFields.Estado = dr.GetString(6); }
            else
            { infoFields.Estado = string.Empty; }



            if (!dr.IsDBNull(7))
            { infoFields.idUsuarioRecebe = dr.GetInt32(7); }
            else
            { infoFields.idUsuarioRecebe = 0; }



            if (!dr.IsDBNull(8))
            { infoFields.idUsuarioIndica = dr.GetInt32(8); }
            else
            { infoFields.idUsuarioIndica = 0; }


            return infoFields;
        }
        #endregion
























        #region Função GetAllParameters

        /// <summary> 
        /// Retorna um array de parâmetros com campos para atualização, seleção e inserção no banco de dados
        /// </summary>
        /// <param name="FieldInfo">Objeto IndicacaoFields</param>
        /// <param name="Modo">Tipo de oepração a ser executada no banco de dados</param>
        /// <returns>SqlParameter[] - Array de parâmetros</returns> 
        private SqlParameter[] GetAllParameters( IndicacaoFields FieldInfo, SQLMode Modo )
        {
            SqlParameter[] Parameters;

            switch (Modo)
            {
                case SQLMode.Add:
                    Parameters = new SqlParameter[9];
                    for (int I = 0; I < Parameters.Length; I++)
                       Parameters[I] = new SqlParameter();
                    //Field idIndicacao
                    Parameters[0].SqlDbType = SqlDbType.Int;
                    Parameters[0].Direction = ParameterDirection.Output;
                    Parameters[0].ParameterName = "@Param_idIndicacao";
                    Parameters[0].Value = DBNull.Value;

                    break;

                case SQLMode.Update:
                    Parameters = new SqlParameter[9];
                    for (int I = 0; I < Parameters.Length; I++)
                       Parameters[I] = new SqlParameter();
                    //Field idIndicacao
                    Parameters[0].SqlDbType = SqlDbType.Int;
                    Parameters[0].ParameterName = "@Param_idIndicacao";
                    Parameters[0].Value = FieldInfo.idIndicacao;

                    break;

                case SQLMode.SelectORDelete:
                    Parameters = new SqlParameter[1];
                    for (int I = 0; I < Parameters.Length; I++)
                       Parameters[I] = new SqlParameter();
                    //Field idIndicacao
                    Parameters[0].SqlDbType = SqlDbType.Int;
                    Parameters[0].ParameterName = "@Param_idIndicacao";
                    Parameters[0].Value = FieldInfo.idIndicacao;

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
            Parameters[1].Size = 150;

            //Field Telefone
            Parameters[2].SqlDbType = SqlDbType.VarChar;
            Parameters[2].ParameterName = "@Param_Telefone";
            if (( FieldInfo.Telefone == null ) || ( FieldInfo.Telefone == string.Empty ))
            { Parameters[2].Value = DBNull.Value; }
            else
            { Parameters[2].Value = FieldInfo.Telefone; }
            Parameters[2].Size = 50;

            //Field Endereco
            Parameters[3].SqlDbType = SqlDbType.VarChar;
            Parameters[3].ParameterName = "@Param_Endereco";
            if (( FieldInfo.Endereco == null ) || ( FieldInfo.Endereco == string.Empty ))
            { Parameters[3].Value = DBNull.Value; }
            else
            { Parameters[3].Value = FieldInfo.Endereco; }
            Parameters[3].Size = 150;

            //Field Bairro
            Parameters[4].SqlDbType = SqlDbType.VarChar;
            Parameters[4].ParameterName = "@Param_Bairro";
            if (( FieldInfo.Bairro == null ) || ( FieldInfo.Bairro == string.Empty ))
            { Parameters[4].Value = DBNull.Value; }
            else
            { Parameters[4].Value = FieldInfo.Bairro; }
            Parameters[4].Size = 150;

            //Field Cidade
            Parameters[5].SqlDbType = SqlDbType.VarChar;
            Parameters[5].ParameterName = "@Param_Cidade";
            if (( FieldInfo.Cidade == null ) || ( FieldInfo.Cidade == string.Empty ))
            { Parameters[5].Value = DBNull.Value; }
            else
            { Parameters[5].Value = FieldInfo.Cidade; }
            Parameters[5].Size = 150;

            //Field Estado
            Parameters[6].SqlDbType = SqlDbType.VarChar;
            Parameters[6].ParameterName = "@Param_Estado";
            if (( FieldInfo.Estado == null ) || ( FieldInfo.Estado == string.Empty ))
            { Parameters[6].Value = DBNull.Value; }
            else
            { Parameters[6].Value = FieldInfo.Estado; }
            Parameters[6].Size = 150;

            //Field idUsuarioRecebe
            Parameters[7].SqlDbType = SqlDbType.Int;
            Parameters[7].ParameterName = "@Param_idUsuarioRecebe";
            Parameters[7].Value = FieldInfo.idUsuarioRecebe;

            //Field idUsuarioIndica
            Parameters[8].SqlDbType = SqlDbType.Int;
            Parameters[8].ParameterName = "@Param_idUsuarioIndica";
            Parameters[8].Value = FieldInfo.idUsuarioIndica;

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

        ~IndicacaoControl() 
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
//    /// Tabela: Indicacao  
//    /// Autor: DAL Creator .net 
//    /// Data de criação: 30/03/2012 01:12:52 
//    /// Descrição: Classe responsável pela perssitência de dados. Utiliza a classe "IndicacaoFields". 
//    /// </summary> 
//    public class IndicacaoControl : IDisposable 
//    {
//
//        #region String de conexão 
//        private string StrConnetionDB = ConfigurationSettings.AppSettings["StringConn"].ToString();
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
//        public IndicacaoControl() {}
//
//
//        #region Inserindo dados na tabela 
//
//        /// <summary> 
//        /// Grava/Persiste um novo objeto IndicacaoFields no banco de dados
//        /// </summary>
//        /// <param name="FieldInfo">Objeto IndicacaoFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
//        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//        public bool Add( ref IndicacaoFields FieldInfo )
//        {
//            try
//            {
//                this.Conn = new SqlConnection(this.StrConnetionDB);
//                this.Conn.Open();
//                this.Tran = this.Conn.BeginTransaction();
//                this.Cmd = new SqlCommand("Proc_Indicacao_Add", this.Conn, this.Tran);
//                this.Cmd.CommandType = CommandType.StoredProcedure;
//                this.Cmd.Parameters.Clear();
//                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
//                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
//                this.Tran.Commit();
//                FieldInfo.idIndicacao = (int)this.Cmd.Parameters["@Param_idIndicacao"].Value;
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
//        /// Grava/Persiste um novo objeto IndicacaoFields no banco de dados
//        /// </summary>
//        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
//        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
//        /// <param name="FieldInfo">Objeto IndicacaoFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
//        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//        public bool Add( SqlConnection ConnIn, SqlTransaction TranIn, ref IndicacaoFields FieldInfo )
//        {
//            try
//            {
//                this.Cmd = new SqlCommand("Proc_Indicacao_Add", ConnIn, TranIn);
//                this.Cmd.CommandType = CommandType.StoredProcedure;
//                this.Cmd.Parameters.Clear();
//                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
//                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
//                FieldInfo.idIndicacao = (int)this.Cmd.Parameters["@Param_idIndicacao"].Value;
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
//        /// Grava/Persiste as alterações em um objeto IndicacaoFields no banco de dados
//        /// </summary>
//        /// <param name="FieldInfo">Objeto IndicacaoFields a ser alterado.</param>
//        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//        public bool Update( IndicacaoFields FieldInfo )
//        {
//            try
//            {
//                this.Conn = new SqlConnection(this.StrConnetionDB);
//                this.Conn.Open();
//                this.Tran = this.Conn.BeginTransaction();
//                this.Cmd = new SqlCommand("Proc_Indicacao_Update", this.Conn, this.Tran);
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
//        /// Grava/Persiste as alterações em um objeto IndicacaoFields no banco de dados
//        /// </summary>
//        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
//        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
//        /// <param name="FieldInfo">Objeto IndicacaoFields a ser alterado.</param>
//        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//        public bool Update( SqlConnection ConnIn, SqlTransaction TranIn, IndicacaoFields FieldInfo )
//        {
//            try
//            {
//                this.Cmd = new SqlCommand("Proc_Indicacao_Update", ConnIn, TranIn);
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
//                this.Cmd = new SqlCommand("Proc_Indicacao_DeleteAll", this.Conn, this.Tran);
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
//                this.Cmd = new SqlCommand("Proc_Indicacao_DeleteAll", ConnIn, TranIn);
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
//        /// <param name="FieldInfo">Objeto IndicacaoFields a ser excluído.</param>
//        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//        public bool Delete( IndicacaoFields FieldInfo )
//        {
//            return Delete(FieldInfo.idIndicacao);
//        }
//
//        /// <summary> 
//        /// Exclui um registro da tabela no banco de dados
//        /// </summary>
//        /// <param name="Param_idIndicacao">int</param>
//        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//        public bool Delete(
//                                     int Param_idIndicacao)
//        {
//            try
//            {
//                this.Conn = new SqlConnection(this.StrConnetionDB);
//                this.Conn.Open();
//                this.Tran = this.Conn.BeginTransaction();
//                this.Cmd = new SqlCommand("Proc_Indicacao_Delete", this.Conn, this.Tran);
//                this.Cmd.CommandType = CommandType.StoredProcedure;
//                this.Cmd.Parameters.Clear();
//                this.Cmd.Parameters.Add(new SqlParameter("@Param_idIndicacao", SqlDbType.Int)).Value = Param_idIndicacao;
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
//        /// <param name="FieldInfo">Objeto IndicacaoFields a ser excluído.</param>
//        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, IndicacaoFields FieldInfo )
//        {
//            return Delete(ConnIn, TranIn, FieldInfo.idIndicacao);
//        }
//
//        /// <summary> 
//        /// Exclui um registro da tabela no banco de dados
//        /// </summary>
//        /// <param name="Param_idIndicacao">int</param>
//        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
//        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
//        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, 
//                                     int Param_idIndicacao)
//        {
//            try
//            {
//                this.Cmd = new SqlCommand("Proc_Indicacao_Delete", ConnIn, TranIn);
//                this.Cmd.CommandType = CommandType.StoredProcedure;
//                this.Cmd.Parameters.Clear();
//                this.Cmd.Parameters.Add(new SqlParameter("@Param_idIndicacao", SqlDbType.Int)).Value = Param_idIndicacao;
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
//        /// Retorna um objeto IndicacaoFields através da chave primária passada como parâmetro
//        /// </summary>
//        /// <param name="Param_idIndicacao">int</param>
//        /// <returns>Objeto IndicacaoFields</returns> 
//        public IndicacaoFields GetItem(
//                                     int Param_idIndicacao)
//        {
//            IndicacaoFields infoFields = new IndicacaoFields();
//            try
//            {
//                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//                {
//                    using (this.Cmd = new SqlCommand("Proc_Indicacao_Select", this.Conn))
//                    {
//                        this.Cmd.CommandType = CommandType.StoredProcedure;
//                        this.Cmd.Parameters.Clear();
//                        this.Cmd.Parameters.Add(new SqlParameter("@Param_idIndicacao", SqlDbType.Int)).Value = Param_idIndicacao;
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
//        /// Seleciona todos os registro da tabela e preenche um ArrayList com o objeto IndicacaoFields.
//        /// </summary>
//        /// <returns>List de objetos IndicacaoFields</returns> 
//        public List<IndicacaoFields> GetAll()
//        {
//            List<IndicacaoFields> arrayInfo = new List<IndicacaoFields>();
//            try
//            {
//                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//                {
//                    using (this.Cmd = new SqlCommand("Proc_Indicacao_GetAll", this.Conn))
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
//                    using (this.Cmd = new SqlCommand("Proc_Indicacao_CountAll", this.Conn))
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
//        /// <returns>IndicacaoFields</returns> 
//        public IndicacaoFields FindByNome(
//                               string Param_Nome )
//        {
//            IndicacaoFields infoFields = new IndicacaoFields();
//            try
//            {
//                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//                {
//                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByNome", this.Conn))
//                    {
//                        this.Cmd.CommandType = CommandType.StoredProcedure;
//                        this.Cmd.Parameters.Clear();
//                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Nome", SqlDbType.VarChar, 150)).Value = Param_Nome;
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
//        #region Selecionando dados da tabela através do campo "Telefone" 
//
//        /// <summary> 
//        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Telefone.
//        /// </summary>
//        /// <param name="Param_Telefone">string</param>
//        /// <returns>IndicacaoFields</returns> 
//        public IndicacaoFields FindByTelefone(
//                               string Param_Telefone )
//        {
//            IndicacaoFields infoFields = new IndicacaoFields();
//            try
//            {
//                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//                {
//                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByTelefone", this.Conn))
//                    {
//                        this.Cmd.CommandType = CommandType.StoredProcedure;
//                        this.Cmd.Parameters.Clear();
//                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Telefone", SqlDbType.VarChar, 50)).Value = Param_Telefone;
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
//        #region Selecionando dados da tabela através do campo "Endereco" 
//
//        /// <summary> 
//        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Endereco.
//        /// </summary>
//        /// <param name="Param_Endereco">string</param>
//        /// <returns>IndicacaoFields</returns> 
//        public IndicacaoFields FindByEndereco(
//                               string Param_Endereco )
//        {
//            IndicacaoFields infoFields = new IndicacaoFields();
//            try
//            {
//                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//                {
//                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByEndereco", this.Conn))
//                    {
//                        this.Cmd.CommandType = CommandType.StoredProcedure;
//                        this.Cmd.Parameters.Clear();
//                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Endereco", SqlDbType.VarChar, 150)).Value = Param_Endereco;
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
//        #region Selecionando dados da tabela através do campo "Bairro" 
//
//        /// <summary> 
//        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Bairro.
//        /// </summary>
//        /// <param name="Param_Bairro">string</param>
//        /// <returns>IndicacaoFields</returns> 
//        public IndicacaoFields FindByBairro(
//                               string Param_Bairro )
//        {
//            IndicacaoFields infoFields = new IndicacaoFields();
//            try
//            {
//                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//                {
//                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByBairro", this.Conn))
//                    {
//                        this.Cmd.CommandType = CommandType.StoredProcedure;
//                        this.Cmd.Parameters.Clear();
//                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Bairro", SqlDbType.VarChar, 150)).Value = Param_Bairro;
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
//        #region Selecionando dados da tabela através do campo "Cidade" 
//
//        /// <summary> 
//        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Cidade.
//        /// </summary>
//        /// <param name="Param_Cidade">string</param>
//        /// <returns>IndicacaoFields</returns> 
//        public IndicacaoFields FindByCidade(
//                               string Param_Cidade )
//        {
//            IndicacaoFields infoFields = new IndicacaoFields();
//            try
//            {
//                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//                {
//                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByCidade", this.Conn))
//                    {
//                        this.Cmd.CommandType = CommandType.StoredProcedure;
//                        this.Cmd.Parameters.Clear();
//                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Cidade", SqlDbType.VarChar, 150)).Value = Param_Cidade;
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
//        #region Selecionando dados da tabela através do campo "Estado" 
//
//        /// <summary> 
//        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Estado.
//        /// </summary>
//        /// <param name="Param_Estado">string</param>
//        /// <returns>IndicacaoFields</returns> 
//        public IndicacaoFields FindByEstado(
//                               string Param_Estado )
//        {
//            IndicacaoFields infoFields = new IndicacaoFields();
//            try
//            {
//                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//                {
//                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByEstado", this.Conn))
//                    {
//                        this.Cmd.CommandType = CommandType.StoredProcedure;
//                        this.Cmd.Parameters.Clear();
//                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Estado", SqlDbType.VarChar, 150)).Value = Param_Estado;
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
//        #region Selecionando dados da tabela através do campo "idUsuarioRecebe" 
//
//        /// <summary> 
//        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo idUsuarioRecebe.
//        /// </summary>
//        /// <param name="Param_idUsuarioRecebe">int</param>
//        /// <returns>IndicacaoFields</returns> 
//        public IndicacaoFields FindByidUsuarioRecebe(
//                               int Param_idUsuarioRecebe )
//        {
//            IndicacaoFields infoFields = new IndicacaoFields();
//            try
//            {
//                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//                {
//                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByidUsuarioRecebe", this.Conn))
//                    {
//                        this.Cmd.CommandType = CommandType.StoredProcedure;
//                        this.Cmd.Parameters.Clear();
//                        this.Cmd.Parameters.Add(new SqlParameter("@Param_idUsuarioRecebe", SqlDbType.Int)).Value = Param_idUsuarioRecebe;
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
//        #region Selecionando dados da tabela através do campo "idUsuarioIndica" 
//
//        /// <summary> 
//        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo idUsuarioIndica.
//        /// </summary>
//        /// <param name="Param_idUsuarioIndica">int</param>
//        /// <returns>IndicacaoFields</returns> 
//        public IndicacaoFields FindByidUsuarioIndica(
//                               int Param_idUsuarioIndica )
//        {
//            IndicacaoFields infoFields = new IndicacaoFields();
//            try
//            {
//                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//                {
//                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByidUsuarioIndica", this.Conn))
//                    {
//                        this.Cmd.CommandType = CommandType.StoredProcedure;
//                        this.Cmd.Parameters.Clear();
//                        this.Cmd.Parameters.Add(new SqlParameter("@Param_idUsuarioIndica", SqlDbType.Int)).Value = Param_idUsuarioIndica;
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
//        /// Retorna um objeto IndicacaoFields preenchido com os valores dos campos do SqlDataReader
//        /// </summary>
//        /// <param name="dr">SqlDataReader - Preenche o objeto IndicacaoFields </param>
//        /// <returns>IndicacaoFields</returns>
//        private IndicacaoFields GetDataFromReader( SqlDataReader dr )
//        {
//            IndicacaoFields infoFields = new IndicacaoFields();
//
//            if (!dr.IsDBNull(0))
//            { infoFields.idIndicacao = dr.GetInt32(0); }
//            else
//            { infoFields.idIndicacao = 0; }
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
//            { infoFields.Telefone = dr.GetString(2); }
//            else
//            { infoFields.Telefone = string.Empty; }
//
//
//
//            if (!dr.IsDBNull(3))
//            { infoFields.Endereco = dr.GetString(3); }
//            else
//            { infoFields.Endereco = string.Empty; }
//
//
//
//            if (!dr.IsDBNull(4))
//            { infoFields.Bairro = dr.GetString(4); }
//            else
//            { infoFields.Bairro = string.Empty; }
//
//
//
//            if (!dr.IsDBNull(5))
//            { infoFields.Cidade = dr.GetString(5); }
//            else
//            { infoFields.Cidade = string.Empty; }
//
//
//
//            if (!dr.IsDBNull(6))
//            { infoFields.Estado = dr.GetString(6); }
//            else
//            { infoFields.Estado = string.Empty; }
//
//
//
//            if (!dr.IsDBNull(7))
//            { infoFields.idUsuarioRecebe = dr.GetInt32(7); }
//            else
//            { infoFields.idUsuarioRecebe = 0; }
//
//
//
//            if (!dr.IsDBNull(8))
//            { infoFields.idUsuarioIndica = dr.GetInt32(8); }
//            else
//            { infoFields.idUsuarioIndica = 0; }
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
//        /// <param name="FieldInfo">Objeto IndicacaoFields</param>
//        /// <param name="Modo">Tipo de oepração a ser executada no banco de dados</param>
//        /// <returns>SqlParameter[] - Array de parâmetros</returns> 
//        private SqlParameter[] GetAllParameters( IndicacaoFields FieldInfo, SQLMode Modo )
//        {
//            SqlParameter[] Parameters;
//
//            switch (Modo)
//            {
//                case SQLMode.Add:
//                    Parameters = new SqlParameter[9];
//                    for (int I = 0; I < Parameters.Length; I++)
//                       Parameters[I] = new SqlParameter();
//                    //Field idIndicacao
//                    Parameters[0].SqlDbType = SqlDbType.Int;
//                    Parameters[0].Direction = ParameterDirection.Output;
//                    Parameters[0].ParameterName = "@Param_idIndicacao";
//                    Parameters[0].Value = DBNull.Value;
//
//                    break;
//
//                case SQLMode.Update:
//                    Parameters = new SqlParameter[9];
//                    for (int I = 0; I < Parameters.Length; I++)
//                       Parameters[I] = new SqlParameter();
//                    //Field idIndicacao
//                    Parameters[0].SqlDbType = SqlDbType.Int;
//                    Parameters[0].ParameterName = "@Param_idIndicacao";
//                    Parameters[0].Value = FieldInfo.idIndicacao;
//
//                    break;
//
//                case SQLMode.SelectORDelete:
//                    Parameters = new SqlParameter[1];
//                    for (int I = 0; I < Parameters.Length; I++)
//                       Parameters[I] = new SqlParameter();
//                    //Field idIndicacao
//                    Parameters[0].SqlDbType = SqlDbType.Int;
//                    Parameters[0].ParameterName = "@Param_idIndicacao";
//                    Parameters[0].Value = FieldInfo.idIndicacao;
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
//            Parameters[1].Size = 150;
//
//            //Field Telefone
//            Parameters[2].SqlDbType = SqlDbType.VarChar;
//            Parameters[2].ParameterName = "@Param_Telefone";
//            if (( FieldInfo.Telefone == null ) || ( FieldInfo.Telefone == string.Empty ))
//            { Parameters[2].Value = DBNull.Value; }
//            else
//            { Parameters[2].Value = FieldInfo.Telefone; }
//            Parameters[2].Size = 50;
//
//            //Field Endereco
//            Parameters[3].SqlDbType = SqlDbType.VarChar;
//            Parameters[3].ParameterName = "@Param_Endereco";
//            if (( FieldInfo.Endereco == null ) || ( FieldInfo.Endereco == string.Empty ))
//            { Parameters[3].Value = DBNull.Value; }
//            else
//            { Parameters[3].Value = FieldInfo.Endereco; }
//            Parameters[3].Size = 150;
//
//            //Field Bairro
//            Parameters[4].SqlDbType = SqlDbType.VarChar;
//            Parameters[4].ParameterName = "@Param_Bairro";
//            if (( FieldInfo.Bairro == null ) || ( FieldInfo.Bairro == string.Empty ))
//            { Parameters[4].Value = DBNull.Value; }
//            else
//            { Parameters[4].Value = FieldInfo.Bairro; }
//            Parameters[4].Size = 150;
//
//            //Field Cidade
//            Parameters[5].SqlDbType = SqlDbType.VarChar;
//            Parameters[5].ParameterName = "@Param_Cidade";
//            if (( FieldInfo.Cidade == null ) || ( FieldInfo.Cidade == string.Empty ))
//            { Parameters[5].Value = DBNull.Value; }
//            else
//            { Parameters[5].Value = FieldInfo.Cidade; }
//            Parameters[5].Size = 150;
//
//            //Field Estado
//            Parameters[6].SqlDbType = SqlDbType.VarChar;
//            Parameters[6].ParameterName = "@Param_Estado";
//            if (( FieldInfo.Estado == null ) || ( FieldInfo.Estado == string.Empty ))
//            { Parameters[6].Value = DBNull.Value; }
//            else
//            { Parameters[6].Value = FieldInfo.Estado; }
//            Parameters[6].Size = 150;
//
//            //Field idUsuarioRecebe
//            Parameters[7].SqlDbType = SqlDbType.Int;
//            Parameters[7].ParameterName = "@Param_idUsuarioRecebe";
//            Parameters[7].Value = FieldInfo.idUsuarioRecebe;
//
//            //Field idUsuarioIndica
//            Parameters[8].SqlDbType = SqlDbType.Int;
//            Parameters[8].ParameterName = "@Param_idUsuarioIndica";
//            Parameters[8].Value = FieldInfo.idUsuarioIndica;
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
//        ~IndicacaoControl() 
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
////    /// Tabela: Indicacao  
////    /// Autor: DAL Creator .net 
////    /// Data de criação: 30/03/2012 00:35:27 
////    /// Descrição: Classe responsável pela perssitência de dados. Utiliza a classe "IndicacaoFields". 
////    /// </summary> 
////    public class IndicacaoControl : IDisposable 
////    {
////
////        #region String de conexão 
////        private string StrConnetionDB = ConfigurationSettings.AppSettings["StringConn"].ToString();
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
////        public IndicacaoControl() {}
////
////
////        #region Inserindo dados na tabela 
////
////        /// <summary> 
////        /// Grava/Persiste um novo objeto IndicacaoFields no banco de dados
////        /// </summary>
////        /// <param name="FieldInfo">Objeto IndicacaoFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
////        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////        public bool Add( ref IndicacaoFields FieldInfo )
////        {
////            try
////            {
////                this.Conn = new SqlConnection(this.StrConnetionDB);
////                this.Conn.Open();
////                this.Tran = this.Conn.BeginTransaction();
////                this.Cmd = new SqlCommand("Proc_Indicacao_Add", this.Conn, this.Tran);
////                this.Cmd.CommandType = CommandType.StoredProcedure;
////                this.Cmd.Parameters.Clear();
////                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
////                this.Tran.Commit();
////                FieldInfo.idIndicacao = (int)this.Cmd.Parameters["@Param_idIndicacao"].Value;
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
////        /// Grava/Persiste um novo objeto IndicacaoFields no banco de dados
////        /// </summary>
////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
////        /// <param name="FieldInfo">Objeto IndicacaoFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
////        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////        public bool Add( SqlConnection ConnIn, SqlTransaction TranIn, ref IndicacaoFields FieldInfo )
////        {
////            try
////            {
////                this.Cmd = new SqlCommand("Proc_Indicacao_Add", ConnIn, TranIn);
////                this.Cmd.CommandType = CommandType.StoredProcedure;
////                this.Cmd.Parameters.Clear();
////                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
////                FieldInfo.idIndicacao = (int)this.Cmd.Parameters["@Param_idIndicacao"].Value;
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
////        /// Grava/Persiste as alterações em um objeto IndicacaoFields no banco de dados
////        /// </summary>
////        /// <param name="FieldInfo">Objeto IndicacaoFields a ser alterado.</param>
////        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////        public bool Update( IndicacaoFields FieldInfo )
////        {
////            try
////            {
////                this.Conn = new SqlConnection(this.StrConnetionDB);
////                this.Conn.Open();
////                this.Tran = this.Conn.BeginTransaction();
////                this.Cmd = new SqlCommand("Proc_Indicacao_Update", this.Conn, this.Tran);
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
////        /// Grava/Persiste as alterações em um objeto IndicacaoFields no banco de dados
////        /// </summary>
////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
////        /// <param name="FieldInfo">Objeto IndicacaoFields a ser alterado.</param>
////        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////        public bool Update( SqlConnection ConnIn, SqlTransaction TranIn, IndicacaoFields FieldInfo )
////        {
////            try
////            {
////                this.Cmd = new SqlCommand("Proc_Indicacao_Update", ConnIn, TranIn);
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
////                this.Cmd = new SqlCommand("Proc_Indicacao_DeleteAll", this.Conn, this.Tran);
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
////                this.Cmd = new SqlCommand("Proc_Indicacao_DeleteAll", ConnIn, TranIn);
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
////        /// <param name="FieldInfo">Objeto IndicacaoFields a ser excluído.</param>
////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////        public bool Delete( IndicacaoFields FieldInfo )
////        {
////            return Delete(FieldInfo.idIndicacao);
////        }
////
////        /// <summary> 
////        /// Exclui um registro da tabela no banco de dados
////        /// </summary>
////        /// <param name="Param_idIndicacao">int</param>
////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////        public bool Delete(
////                                     int Param_idIndicacao)
////        {
////            try
////            {
////                this.Conn = new SqlConnection(this.StrConnetionDB);
////                this.Conn.Open();
////                this.Tran = this.Conn.BeginTransaction();
////                this.Cmd = new SqlCommand("Proc_Indicacao_Delete", this.Conn, this.Tran);
////                this.Cmd.CommandType = CommandType.StoredProcedure;
////                this.Cmd.Parameters.Clear();
////                this.Cmd.Parameters.Add(new SqlParameter("@Param_idIndicacao", SqlDbType.Int)).Value = Param_idIndicacao;
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
////        /// <param name="FieldInfo">Objeto IndicacaoFields a ser excluído.</param>
////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, IndicacaoFields FieldInfo )
////        {
////            return Delete(ConnIn, TranIn, FieldInfo.idIndicacao);
////        }
////
////        /// <summary> 
////        /// Exclui um registro da tabela no banco de dados
////        /// </summary>
////        /// <param name="Param_idIndicacao">int</param>
////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, 
////                                     int Param_idIndicacao)
////        {
////            try
////            {
////                this.Cmd = new SqlCommand("Proc_Indicacao_Delete", ConnIn, TranIn);
////                this.Cmd.CommandType = CommandType.StoredProcedure;
////                this.Cmd.Parameters.Clear();
////                this.Cmd.Parameters.Add(new SqlParameter("@Param_idIndicacao", SqlDbType.Int)).Value = Param_idIndicacao;
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
////        /// Retorna um objeto IndicacaoFields através da chave primária passada como parâmetro
////        /// </summary>
////        /// <param name="Param_idIndicacao">int</param>
////        /// <returns>Objeto IndicacaoFields</returns> 
////        public IndicacaoFields GetItem(
////                                     int Param_idIndicacao)
////        {
////            IndicacaoFields infoFields = new IndicacaoFields();
////            try
////            {
////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////                {
////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_Select", this.Conn))
////                    {
////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////                        this.Cmd.Parameters.Clear();
////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_idIndicacao", SqlDbType.Int)).Value = Param_idIndicacao;
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
////        /// Seleciona todos os registro da tabela e preenche um ArrayList com o objeto IndicacaoFields.
////        /// </summary>
////        /// <returns>List de objetos IndicacaoFields</returns> 
////        public List<IndicacaoFields> GetAll()
////        {
////            List<IndicacaoFields> arrayInfo = new List<IndicacaoFields>();
////            try
////            {
////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////                {
////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_GetAll", this.Conn))
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
////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_CountAll", this.Conn))
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
////        /// <returns>IndicacaoFields</returns> 
////        public IndicacaoFields FindByNome(
////                               string Param_Nome )
////        {
////            IndicacaoFields infoFields = new IndicacaoFields();
////            try
////            {
////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////                {
////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByNome", this.Conn))
////                    {
////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////                        this.Cmd.Parameters.Clear();
////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Nome", SqlDbType.VarChar, 150)).Value = Param_Nome;
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
////        #region Selecionando dados da tabela através do campo "Telefone" 
////
////        /// <summary> 
////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Telefone.
////        /// </summary>
////        /// <param name="Param_Telefone">string</param>
////        /// <returns>IndicacaoFields</returns> 
////        public IndicacaoFields FindByTelefone(
////                               string Param_Telefone )
////        {
////            IndicacaoFields infoFields = new IndicacaoFields();
////            try
////            {
////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////                {
////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByTelefone", this.Conn))
////                    {
////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////                        this.Cmd.Parameters.Clear();
////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Telefone", SqlDbType.VarChar, 50)).Value = Param_Telefone;
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
////        #region Selecionando dados da tabela através do campo "Endereco" 
////
////        /// <summary> 
////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Endereco.
////        /// </summary>
////        /// <param name="Param_Endereco">string</param>
////        /// <returns>IndicacaoFields</returns> 
////        public IndicacaoFields FindByEndereco(
////                               string Param_Endereco )
////        {
////            IndicacaoFields infoFields = new IndicacaoFields();
////            try
////            {
////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////                {
////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByEndereco", this.Conn))
////                    {
////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////                        this.Cmd.Parameters.Clear();
////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Endereco", SqlDbType.VarChar, 150)).Value = Param_Endereco;
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
////        #region Selecionando dados da tabela através do campo "Bairro" 
////
////        /// <summary> 
////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Bairro.
////        /// </summary>
////        /// <param name="Param_Bairro">string</param>
////        /// <returns>IndicacaoFields</returns> 
////        public IndicacaoFields FindByBairro(
////                               string Param_Bairro )
////        {
////            IndicacaoFields infoFields = new IndicacaoFields();
////            try
////            {
////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////                {
////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByBairro", this.Conn))
////                    {
////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////                        this.Cmd.Parameters.Clear();
////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Bairro", SqlDbType.VarChar, 150)).Value = Param_Bairro;
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
////        #region Selecionando dados da tabela através do campo "Cidade" 
////
////        /// <summary> 
////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Cidade.
////        /// </summary>
////        /// <param name="Param_Cidade">string</param>
////        /// <returns>IndicacaoFields</returns> 
////        public IndicacaoFields FindByCidade(
////                               string Param_Cidade )
////        {
////            IndicacaoFields infoFields = new IndicacaoFields();
////            try
////            {
////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////                {
////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByCidade", this.Conn))
////                    {
////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////                        this.Cmd.Parameters.Clear();
////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Cidade", SqlDbType.VarChar, 150)).Value = Param_Cidade;
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
////        #region Selecionando dados da tabela através do campo "Estado" 
////
////        /// <summary> 
////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Estado.
////        /// </summary>
////        /// <param name="Param_Estado">string</param>
////        /// <returns>IndicacaoFields</returns> 
////        public IndicacaoFields FindByEstado(
////                               string Param_Estado )
////        {
////            IndicacaoFields infoFields = new IndicacaoFields();
////            try
////            {
////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////                {
////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByEstado", this.Conn))
////                    {
////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////                        this.Cmd.Parameters.Clear();
////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Estado", SqlDbType.VarChar, 150)).Value = Param_Estado;
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
////        #region Selecionando dados da tabela através do campo "idUsuarioRecebe" 
////
////        /// <summary> 
////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo idUsuarioRecebe.
////        /// </summary>
////        /// <param name="Param_idUsuarioRecebe">int</param>
////        /// <returns>IndicacaoFields</returns> 
////        public IndicacaoFields FindByidUsuarioRecebe(
////                               int Param_idUsuarioRecebe )
////        {
////            IndicacaoFields infoFields = new IndicacaoFields();
////            try
////            {
////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////                {
////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByidUsuarioRecebe", this.Conn))
////                    {
////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////                        this.Cmd.Parameters.Clear();
////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_idUsuarioRecebe", SqlDbType.Int)).Value = Param_idUsuarioRecebe;
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
////        #region Selecionando dados da tabela através do campo "idUsuarioIndica" 
////
////        /// <summary> 
////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo idUsuarioIndica.
////        /// </summary>
////        /// <param name="Param_idUsuarioIndica">int</param>
////        /// <returns>IndicacaoFields</returns> 
////        public IndicacaoFields FindByidUsuarioIndica(
////                               int Param_idUsuarioIndica )
////        {
////            IndicacaoFields infoFields = new IndicacaoFields();
////            try
////            {
////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////                {
////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByidUsuarioIndica", this.Conn))
////                    {
////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////                        this.Cmd.Parameters.Clear();
////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_idUsuarioIndica", SqlDbType.Int)).Value = Param_idUsuarioIndica;
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
////        /// Retorna um objeto IndicacaoFields preenchido com os valores dos campos do SqlDataReader
////        /// </summary>
////        /// <param name="dr">SqlDataReader - Preenche o objeto IndicacaoFields </param>
////        /// <returns>IndicacaoFields</returns>
////        private IndicacaoFields GetDataFromReader( SqlDataReader dr )
////        {
////            IndicacaoFields infoFields = new IndicacaoFields();
////
////            if (!dr.IsDBNull(0))
////            { infoFields.idIndicacao = dr.GetInt32(0); }
////            else
////            { infoFields.idIndicacao = 0; }
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
////            { infoFields.Telefone = dr.GetString(2); }
////            else
////            { infoFields.Telefone = string.Empty; }
////
////
////
////            if (!dr.IsDBNull(3))
////            { infoFields.Endereco = dr.GetString(3); }
////            else
////            { infoFields.Endereco = string.Empty; }
////
////
////
////            if (!dr.IsDBNull(4))
////            { infoFields.Bairro = dr.GetString(4); }
////            else
////            { infoFields.Bairro = string.Empty; }
////
////
////
////            if (!dr.IsDBNull(5))
////            { infoFields.Cidade = dr.GetString(5); }
////            else
////            { infoFields.Cidade = string.Empty; }
////
////
////
////            if (!dr.IsDBNull(6))
////            { infoFields.Estado = dr.GetString(6); }
////            else
////            { infoFields.Estado = string.Empty; }
////
////
////
////            if (!dr.IsDBNull(7))
////            { infoFields.idUsuarioRecebe = dr.GetInt32(7); }
////            else
////            { infoFields.idUsuarioRecebe = 0; }
////
////
////
////            if (!dr.IsDBNull(8))
////            { infoFields.idUsuarioIndica = dr.GetInt32(8); }
////            else
////            { infoFields.idUsuarioIndica = 0; }
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
////
////
////        #region Função GetAllParameters
////
////        /// <summary> 
////        /// Retorna um array de parâmetros com campos para atualização, seleção e inserção no banco de dados
////        /// </summary>
////        /// <param name="FieldInfo">Objeto IndicacaoFields</param>
////        /// <param name="Modo">Tipo de oepração a ser executada no banco de dados</param>
////        /// <returns>SqlParameter[] - Array de parâmetros</returns> 
////        private SqlParameter[] GetAllParameters( IndicacaoFields FieldInfo, SQLMode Modo )
////        {
////            SqlParameter[] Parameters;
////
////            switch (Modo)
////            {
////                case SQLMode.Add:
////                    Parameters = new SqlParameter[9];
////                    for (int I = 0; I < Parameters.Length; I++)
////                       Parameters[I] = new SqlParameter();
////                    //Field idIndicacao
////                    Parameters[0].SqlDbType = SqlDbType.Int;
////                    Parameters[0].Direction = ParameterDirection.Output;
////                    Parameters[0].ParameterName = "@Param_idIndicacao";
////                    Parameters[0].Value = DBNull.Value;
////
////                    break;
////
////                case SQLMode.Update:
////                    Parameters = new SqlParameter[9];
////                    for (int I = 0; I < Parameters.Length; I++)
////                       Parameters[I] = new SqlParameter();
////                    //Field idIndicacao
////                    Parameters[0].SqlDbType = SqlDbType.Int;
////                    Parameters[0].ParameterName = "@Param_idIndicacao";
////                    Parameters[0].Value = FieldInfo.idIndicacao;
////
////                    break;
////
////                case SQLMode.SelectORDelete:
////                    Parameters = new SqlParameter[1];
////                    for (int I = 0; I < Parameters.Length; I++)
////                       Parameters[I] = new SqlParameter();
////                    //Field idIndicacao
////                    Parameters[0].SqlDbType = SqlDbType.Int;
////                    Parameters[0].ParameterName = "@Param_idIndicacao";
////                    Parameters[0].Value = FieldInfo.idIndicacao;
////
////                    return Parameters;
////
////                default:
////                    Parameters = new SqlParameter[9];
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
////            Parameters[1].Size = 150;
////
////            //Field Telefone
////            Parameters[2].SqlDbType = SqlDbType.VarChar;
////            Parameters[2].ParameterName = "@Param_Telefone";
////            if (( FieldInfo.Telefone == null ) || ( FieldInfo.Telefone == string.Empty ))
////            { Parameters[2].Value = DBNull.Value; }
////            else
////            { Parameters[2].Value = FieldInfo.Telefone; }
////            Parameters[2].Size = 50;
////
////            //Field Endereco
////            Parameters[3].SqlDbType = SqlDbType.VarChar;
////            Parameters[3].ParameterName = "@Param_Endereco";
////            if (( FieldInfo.Endereco == null ) || ( FieldInfo.Endereco == string.Empty ))
////            { Parameters[3].Value = DBNull.Value; }
////            else
////            { Parameters[3].Value = FieldInfo.Endereco; }
////            Parameters[3].Size = 150;
////
////            //Field Bairro
////            Parameters[4].SqlDbType = SqlDbType.VarChar;
////            Parameters[4].ParameterName = "@Param_Bairro";
////            if (( FieldInfo.Bairro == null ) || ( FieldInfo.Bairro == string.Empty ))
////            { Parameters[4].Value = DBNull.Value; }
////            else
////            { Parameters[4].Value = FieldInfo.Bairro; }
////            Parameters[4].Size = 150;
////
////            //Field Cidade
////            Parameters[5].SqlDbType = SqlDbType.VarChar;
////            Parameters[5].ParameterName = "@Param_Cidade";
////            if (( FieldInfo.Cidade == null ) || ( FieldInfo.Cidade == string.Empty ))
////            { Parameters[5].Value = DBNull.Value; }
////            else
////            { Parameters[5].Value = FieldInfo.Cidade; }
////            Parameters[5].Size = 150;
////
////            //Field Estado
////            Parameters[6].SqlDbType = SqlDbType.VarChar;
////            Parameters[6].ParameterName = "@Param_Estado";
////            if (( FieldInfo.Estado == null ) || ( FieldInfo.Estado == string.Empty ))
////            { Parameters[6].Value = DBNull.Value; }
////            else
////            { Parameters[6].Value = FieldInfo.Estado; }
////            Parameters[6].Size = 150;
////
////            //Field idUsuarioRecebe
////            Parameters[7].SqlDbType = SqlDbType.Int;
////            Parameters[7].ParameterName = "@Param_idUsuarioRecebe";
////            Parameters[7].Value = FieldInfo.idUsuarioRecebe;
////
////            //Field idUsuarioIndica
////            Parameters[8].SqlDbType = SqlDbType.Int;
////            Parameters[8].ParameterName = "@Param_idUsuarioIndica";
////            Parameters[8].Value = FieldInfo.idUsuarioIndica;
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
////        ~IndicacaoControl() 
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
//////using System.Collections.Generic;
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
//////    /// Tabela: Indicacao  
//////    /// Autor: DAL Creator .net 
//////    /// Data de criação: 27/03/2012 03:05:16 
//////    /// Descrição: Classe responsável pela perssitência de dados. Utiliza a classe "IndicacaoFields". 
//////    /// </summary> 
//////    public class IndicacaoControl : IDisposable 
//////    {
//////
//////        #region String de conexão 
//////        private string StrConnetionDB = ConfigurationSettings.AppSettings["StringConn"].ToString();
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
//////        public IndicacaoControl() {}
//////
//////
//////        #region Inserindo dados na tabela 
//////
//////        /// <summary> 
//////        /// Grava/Persiste um novo objeto IndicacaoFields no banco de dados
//////        /// </summary>
//////        /// <param name="FieldInfo">Objeto IndicacaoFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
//////        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////        public bool Add( ref IndicacaoFields FieldInfo )
//////        {
//////            try
//////            {
//////                this.Conn = new SqlConnection(this.StrConnetionDB);
//////                this.Conn.Open();
//////                this.Tran = this.Conn.BeginTransaction();
//////                this.Cmd = new SqlCommand("Proc_Indicacao_Add", this.Conn, this.Tran);
//////                this.Cmd.CommandType = CommandType.StoredProcedure;
//////                this.Cmd.Parameters.Clear();
//////                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
//////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
//////                this.Tran.Commit();
//////                FieldInfo.idIndicacao = (int)this.Cmd.Parameters["@Param_idIndicacao"].Value;
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
//////        /// Grava/Persiste um novo objeto IndicacaoFields no banco de dados
//////        /// </summary>
//////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
//////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
//////        /// <param name="FieldInfo">Objeto IndicacaoFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
//////        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////        public bool Add( SqlConnection ConnIn, SqlTransaction TranIn, ref IndicacaoFields FieldInfo )
//////        {
//////            try
//////            {
//////                this.Cmd = new SqlCommand("Proc_Indicacao_Add", ConnIn, TranIn);
//////                this.Cmd.CommandType = CommandType.StoredProcedure;
//////                this.Cmd.Parameters.Clear();
//////                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
//////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
//////                FieldInfo.idIndicacao = (int)this.Cmd.Parameters["@Param_idIndicacao"].Value;
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
//////        /// Grava/Persiste as alterações em um objeto IndicacaoFields no banco de dados
//////        /// </summary>
//////        /// <param name="FieldInfo">Objeto IndicacaoFields a ser alterado.</param>
//////        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////        public bool Update( IndicacaoFields FieldInfo )
//////        {
//////            try
//////            {
//////                this.Conn = new SqlConnection(this.StrConnetionDB);
//////                this.Conn.Open();
//////                this.Tran = this.Conn.BeginTransaction();
//////                this.Cmd = new SqlCommand("Proc_Indicacao_Update", this.Conn, this.Tran);
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
//////        /// Grava/Persiste as alterações em um objeto IndicacaoFields no banco de dados
//////        /// </summary>
//////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
//////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
//////        /// <param name="FieldInfo">Objeto IndicacaoFields a ser alterado.</param>
//////        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////        public bool Update( SqlConnection ConnIn, SqlTransaction TranIn, IndicacaoFields FieldInfo )
//////        {
//////            try
//////            {
//////                this.Cmd = new SqlCommand("Proc_Indicacao_Update", ConnIn, TranIn);
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
//////                this.Cmd = new SqlCommand("Proc_Indicacao_DeleteAll", this.Conn, this.Tran);
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
//////                this.Cmd = new SqlCommand("Proc_Indicacao_DeleteAll", ConnIn, TranIn);
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
//////        /// <param name="FieldInfo">Objeto IndicacaoFields a ser excluído.</param>
//////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////        public bool Delete( IndicacaoFields FieldInfo )
//////        {
//////            return Delete(FieldInfo.idIndicacao);
//////        }
//////
//////        /// <summary> 
//////        /// Exclui um registro da tabela no banco de dados
//////        /// </summary>
//////        /// <param name="Param_idIndicacao">int</param>
//////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////        public bool Delete(
//////                                     int Param_idIndicacao)
//////        {
//////            try
//////            {
//////                this.Conn = new SqlConnection(this.StrConnetionDB);
//////                this.Conn.Open();
//////                this.Tran = this.Conn.BeginTransaction();
//////                this.Cmd = new SqlCommand("Proc_Indicacao_Delete", this.Conn, this.Tran);
//////                this.Cmd.CommandType = CommandType.StoredProcedure;
//////                this.Cmd.Parameters.Clear();
//////                this.Cmd.Parameters.Add(new SqlParameter("@Param_idIndicacao", SqlDbType.Int)).Value = Param_idIndicacao;
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
//////        /// <param name="FieldInfo">Objeto IndicacaoFields a ser excluído.</param>
//////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, IndicacaoFields FieldInfo )
//////        {
//////            return Delete(ConnIn, TranIn, FieldInfo.idIndicacao);
//////        }
//////
//////        /// <summary> 
//////        /// Exclui um registro da tabela no banco de dados
//////        /// </summary>
//////        /// <param name="Param_idIndicacao">int</param>
//////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
//////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
//////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, 
//////                                     int Param_idIndicacao)
//////        {
//////            try
//////            {
//////                this.Cmd = new SqlCommand("Proc_Indicacao_Delete", ConnIn, TranIn);
//////                this.Cmd.CommandType = CommandType.StoredProcedure;
//////                this.Cmd.Parameters.Clear();
//////                this.Cmd.Parameters.Add(new SqlParameter("@Param_idIndicacao", SqlDbType.Int)).Value = Param_idIndicacao;
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
//////        /// Retorna um objeto IndicacaoFields através da chave primária passada como parâmetro
//////        /// </summary>
//////        /// <param name="Param_idIndicacao">int</param>
//////        /// <returns>Objeto IndicacaoFields</returns> 
//////        public IndicacaoFields GetItem(
//////                                     int Param_idIndicacao)
//////        {
//////            IndicacaoFields infoFields = new IndicacaoFields();
//////            try
//////            {
//////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////                {
//////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_Select", this.Conn))
//////                    {
//////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////                        this.Cmd.Parameters.Clear();
//////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_idIndicacao", SqlDbType.Int)).Value = Param_idIndicacao;
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
//////        /// Seleciona todos os registro da tabela e preenche um ArrayList com o objeto IndicacaoFields.
//////        /// </summary>
//////        /// <returns>List de objetos IndicacaoFields</returns> 
//////        public List<IndicacaoFields> GetAll()
//////        {
//////            List<IndicacaoFields> arrayInfo = new List<IndicacaoFields>();
//////            try
//////            {
//////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////                {
//////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_GetAll", this.Conn))
//////                    {
//////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////                        this.Cmd.Parameters.Clear();
//////                        this.Cmd.Connection.Open();
//////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
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
//////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_CountAll", this.Conn))
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
//////        /// <returns>IndicacaoFields</returns> 
//////        public IndicacaoFields FindByNome(
//////                               string Param_Nome )
//////        {
//////            IndicacaoFields infoFields = new IndicacaoFields();
//////            try
//////            {
//////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////                {
//////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByNome", this.Conn))
//////                    {
//////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////                        this.Cmd.Parameters.Clear();
//////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Nome", SqlDbType.VarChar, 150)).Value = Param_Nome;
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
//////        #region Selecionando dados da tabela através do campo "Telefone" 
//////
//////        /// <summary> 
//////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Telefone.
//////        /// </summary>
//////        /// <param name="Param_Telefone">string</param>
//////        /// <returns>IndicacaoFields</returns> 
//////        public IndicacaoFields FindByTelefone(
//////                               string Param_Telefone )
//////        {
//////            IndicacaoFields infoFields = new IndicacaoFields();
//////            try
//////            {
//////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////                {
//////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByTelefone", this.Conn))
//////                    {
//////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////                        this.Cmd.Parameters.Clear();
//////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Telefone", SqlDbType.VarChar, 50)).Value = Param_Telefone;
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
//////        #region Selecionando dados da tabela através do campo "Endereco" 
//////
//////        /// <summary> 
//////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Endereco.
//////        /// </summary>
//////        /// <param name="Param_Endereco">string</param>
//////        /// <returns>IndicacaoFields</returns> 
//////        public IndicacaoFields FindByEndereco(
//////                               string Param_Endereco )
//////        {
//////            IndicacaoFields infoFields = new IndicacaoFields();
//////            try
//////            {
//////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////                {
//////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByEndereco", this.Conn))
//////                    {
//////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////                        this.Cmd.Parameters.Clear();
//////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Endereco", SqlDbType.VarChar, 150)).Value = Param_Endereco;
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
//////        #region Selecionando dados da tabela através do campo "Bairro" 
//////
//////        /// <summary> 
//////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Bairro.
//////        /// </summary>
//////        /// <param name="Param_Bairro">string</param>
//////        /// <returns>IndicacaoFields</returns> 
//////        public IndicacaoFields FindByBairro(
//////                               string Param_Bairro )
//////        {
//////            IndicacaoFields infoFields = new IndicacaoFields();
//////            try
//////            {
//////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////                {
//////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByBairro", this.Conn))
//////                    {
//////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////                        this.Cmd.Parameters.Clear();
//////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Bairro", SqlDbType.VarChar, 150)).Value = Param_Bairro;
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
//////        #region Selecionando dados da tabela através do campo "Cidade" 
//////
//////        /// <summary> 
//////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Cidade.
//////        /// </summary>
//////        /// <param name="Param_Cidade">string</param>
//////        /// <returns>IndicacaoFields</returns> 
//////        public IndicacaoFields FindByCidade(
//////                               string Param_Cidade )
//////        {
//////            IndicacaoFields infoFields = new IndicacaoFields();
//////            try
//////            {
//////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////                {
//////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByCidade", this.Conn))
//////                    {
//////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////                        this.Cmd.Parameters.Clear();
//////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Cidade", SqlDbType.VarChar, 150)).Value = Param_Cidade;
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
//////        #region Selecionando dados da tabela através do campo "Estado" 
//////
//////        /// <summary> 
//////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Estado.
//////        /// </summary>
//////        /// <param name="Param_Estado">string</param>
//////        /// <returns>IndicacaoFields</returns> 
//////        public IndicacaoFields FindByEstado(
//////                               string Param_Estado )
//////        {
//////            IndicacaoFields infoFields = new IndicacaoFields();
//////            try
//////            {
//////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////                {
//////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByEstado", this.Conn))
//////                    {
//////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////                        this.Cmd.Parameters.Clear();
//////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Estado", SqlDbType.VarChar, 150)).Value = Param_Estado;
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
//////        #region Selecionando dados da tabela através do campo "idUsuarioRecebe" 
//////
//////        /// <summary> 
//////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo idUsuarioRecebe.
//////        /// </summary>
//////        /// <param name="Param_idUsuarioRecebe">int</param>
//////        /// <returns>IndicacaoFields</returns> 
//////        public IndicacaoFields FindByidUsuarioRecebe(
//////                               int Param_idUsuarioRecebe )
//////        {
//////            IndicacaoFields infoFields = new IndicacaoFields();
//////            try
//////            {
//////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////                {
//////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByidUsuarioRecebe", this.Conn))
//////                    {
//////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////                        this.Cmd.Parameters.Clear();
//////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_idUsuarioRecebe", SqlDbType.Int)).Value = Param_idUsuarioRecebe;
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
//////        #region Selecionando dados da tabela através do campo "idUsuarioIndica" 
//////
//////        /// <summary> 
//////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo idUsuarioIndica.
//////        /// </summary>
//////        /// <param name="Param_idUsuarioIndica">int</param>
//////        /// <returns>IndicacaoFields</returns> 
//////        public IndicacaoFields FindByidUsuarioIndica(
//////                               int Param_idUsuarioIndica )
//////        {
//////            IndicacaoFields infoFields = new IndicacaoFields();
//////            try
//////            {
//////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////                {
//////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByidUsuarioIndica", this.Conn))
//////                    {
//////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////                        this.Cmd.Parameters.Clear();
//////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_idUsuarioIndica", SqlDbType.Int)).Value = Param_idUsuarioIndica;
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
//////        /// Retorna um objeto IndicacaoFields preenchido com os valores dos campos do SqlDataReader
//////        /// </summary>
//////        /// <param name="dr">SqlDataReader - Preenche o objeto IndicacaoFields </param>
//////        /// <returns>IndicacaoFields</returns>
//////        private IndicacaoFields GetDataFromReader( SqlDataReader dr )
//////        {
//////            IndicacaoFields infoFields = new IndicacaoFields();
//////
//////            if (!dr.IsDBNull(0))
//////            { infoFields.idIndicacao = dr.GetInt32(0); }
//////            else
//////            { infoFields.idIndicacao = 0; }
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
//////            { infoFields.Telefone = dr.GetString(2); }
//////            else
//////            { infoFields.Telefone = string.Empty; }
//////
//////
//////
//////            if (!dr.IsDBNull(3))
//////            { infoFields.Endereco = dr.GetString(3); }
//////            else
//////            { infoFields.Endereco = string.Empty; }
//////
//////
//////
//////            if (!dr.IsDBNull(4))
//////            { infoFields.Bairro = dr.GetString(4); }
//////            else
//////            { infoFields.Bairro = string.Empty; }
//////
//////
//////
//////            if (!dr.IsDBNull(5))
//////            { infoFields.Cidade = dr.GetString(5); }
//////            else
//////            { infoFields.Cidade = string.Empty; }
//////
//////
//////
//////            if (!dr.IsDBNull(6))
//////            { infoFields.Estado = dr.GetString(6); }
//////            else
//////            { infoFields.Estado = string.Empty; }
//////
//////
//////
//////            if (!dr.IsDBNull(7))
//////            { infoFields.idUsuarioRecebe = dr.GetInt32(7); }
//////            else
//////            { infoFields.idUsuarioRecebe = 0; }
//////
//////
//////
//////            if (!dr.IsDBNull(8))
//////            { infoFields.idUsuarioIndica = dr.GetInt32(8); }
//////            else
//////            { infoFields.idUsuarioIndica = 0; }
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
//////
//////
//////        #region Função GetAllParameters
//////
//////        /// <summary> 
//////        /// Retorna um array de parâmetros com campos para atualização, seleção e inserção no banco de dados
//////        /// </summary>
//////        /// <param name="FieldInfo">Objeto IndicacaoFields</param>
//////        /// <param name="Modo">Tipo de oepração a ser executada no banco de dados</param>
//////        /// <returns>SqlParameter[] - Array de parâmetros</returns> 
//////        private SqlParameter[] GetAllParameters( IndicacaoFields FieldInfo, SQLMode Modo )
//////        {
//////            SqlParameter[] Parameters;
//////
//////            switch (Modo)
//////            {
//////                case SQLMode.Add:
//////                    Parameters = new SqlParameter[9];
//////                    for (int I = 0; I < Parameters.Length; I++)
//////                       Parameters[I] = new SqlParameter();
//////                    //Field idIndicacao
//////                    Parameters[0].SqlDbType = SqlDbType.Int;
//////                    Parameters[0].Direction = ParameterDirection.Output;
//////                    Parameters[0].ParameterName = "@Param_idIndicacao";
//////                    Parameters[0].Value = DBNull.Value;
//////
//////                    break;
//////
//////                case SQLMode.Update:
//////                    Parameters = new SqlParameter[9];
//////                    for (int I = 0; I < Parameters.Length; I++)
//////                       Parameters[I] = new SqlParameter();
//////                    //Field idIndicacao
//////                    Parameters[0].SqlDbType = SqlDbType.Int;
//////                    Parameters[0].ParameterName = "@Param_idIndicacao";
//////                    Parameters[0].Value = FieldInfo.idIndicacao;
//////
//////                    break;
//////
//////                case SQLMode.SelectORDelete:
//////                    Parameters = new SqlParameter[1];
//////                    for (int I = 0; I < Parameters.Length; I++)
//////                       Parameters[I] = new SqlParameter();
//////                    //Field idIndicacao
//////                    Parameters[0].SqlDbType = SqlDbType.Int;
//////                    Parameters[0].ParameterName = "@Param_idIndicacao";
//////                    Parameters[0].Value = FieldInfo.idIndicacao;
//////
//////                    return Parameters;
//////
//////                default:
//////                    Parameters = new SqlParameter[9];
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
//////            Parameters[1].Size = 150;
//////
//////            //Field Telefone
//////            Parameters[2].SqlDbType = SqlDbType.VarChar;
//////            Parameters[2].ParameterName = "@Param_Telefone";
//////            if (( FieldInfo.Telefone == null ) || ( FieldInfo.Telefone == string.Empty ))
//////            { Parameters[2].Value = DBNull.Value; }
//////            else
//////            { Parameters[2].Value = FieldInfo.Telefone; }
//////            Parameters[2].Size = 50;
//////
//////            //Field Endereco
//////            Parameters[3].SqlDbType = SqlDbType.VarChar;
//////            Parameters[3].ParameterName = "@Param_Endereco";
//////            if (( FieldInfo.Endereco == null ) || ( FieldInfo.Endereco == string.Empty ))
//////            { Parameters[3].Value = DBNull.Value; }
//////            else
//////            { Parameters[3].Value = FieldInfo.Endereco; }
//////            Parameters[3].Size = 150;
//////
//////            //Field Bairro
//////            Parameters[4].SqlDbType = SqlDbType.VarChar;
//////            Parameters[4].ParameterName = "@Param_Bairro";
//////            if (( FieldInfo.Bairro == null ) || ( FieldInfo.Bairro == string.Empty ))
//////            { Parameters[4].Value = DBNull.Value; }
//////            else
//////            { Parameters[4].Value = FieldInfo.Bairro; }
//////            Parameters[4].Size = 150;
//////
//////            //Field Cidade
//////            Parameters[5].SqlDbType = SqlDbType.VarChar;
//////            Parameters[5].ParameterName = "@Param_Cidade";
//////            if (( FieldInfo.Cidade == null ) || ( FieldInfo.Cidade == string.Empty ))
//////            { Parameters[5].Value = DBNull.Value; }
//////            else
//////            { Parameters[5].Value = FieldInfo.Cidade; }
//////            Parameters[5].Size = 150;
//////
//////            //Field Estado
//////            Parameters[6].SqlDbType = SqlDbType.VarChar;
//////            Parameters[6].ParameterName = "@Param_Estado";
//////            if (( FieldInfo.Estado == null ) || ( FieldInfo.Estado == string.Empty ))
//////            { Parameters[6].Value = DBNull.Value; }
//////            else
//////            { Parameters[6].Value = FieldInfo.Estado; }
//////            Parameters[6].Size = 150;
//////
//////            //Field idUsuarioRecebe
//////            Parameters[7].SqlDbType = SqlDbType.Int;
//////            Parameters[7].ParameterName = "@Param_idUsuarioRecebe";
//////            Parameters[7].Value = FieldInfo.idUsuarioRecebe;
//////
//////            //Field idUsuarioIndica
//////            Parameters[8].SqlDbType = SqlDbType.Int;
//////            Parameters[8].ParameterName = "@Param_idUsuarioIndica";
//////            Parameters[8].Value = FieldInfo.idUsuarioIndica;
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
//////        ~IndicacaoControl() 
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
////////    /// Tabela: Indicacao  
////////    /// Autor: DAL Creator .net 
////////    /// Data de criação: 27/03/2012 02:25:18 
////////    /// Descrição: Classe responsável pela perssitência de dados. Utiliza a classe "IndicacaoFields". 
////////    /// </summary> 
////////    public class IndicacaoControl : IDisposable 
////////    {
////////
////////        #region String de conexão 
////////        private string StrConnetionDB = ConfigurationSettings.AppSettings["StringConn"].ToString();
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
////////        public IndicacaoControl() {}
////////
////////
////////        #region Inserindo dados na tabela 
////////
////////        /// <summary> 
////////        /// Grava/Persiste um novo objeto IndicacaoFields no banco de dados
////////        /// </summary>
////////        /// <param name="FieldInfo">Objeto IndicacaoFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
////////        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////////        public bool Add( ref IndicacaoFields FieldInfo )
////////        {
////////            try
////////            {
////////                this.Conn = new SqlConnection(this.StrConnetionDB);
////////                this.Conn.Open();
////////                this.Tran = this.Conn.BeginTransaction();
////////                this.Cmd = new SqlCommand("Proc_Indicacao_Add", this.Conn, this.Tran);
////////                this.Cmd.CommandType = CommandType.StoredProcedure;
////////                this.Cmd.Parameters.Clear();
////////                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
////////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
////////                this.Tran.Commit();
////////                FieldInfo.idIndicacao = (int)this.Cmd.Parameters["@Param_idIndicacao"].Value;
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
////////        /// Grava/Persiste um novo objeto IndicacaoFields no banco de dados
////////        /// </summary>
////////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
////////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
////////        /// <param name="FieldInfo">Objeto IndicacaoFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
////////        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////////        public bool Add( SqlConnection ConnIn, SqlTransaction TranIn, ref IndicacaoFields FieldInfo )
////////        {
////////            try
////////            {
////////                this.Cmd = new SqlCommand("Proc_Indicacao_Add", ConnIn, TranIn);
////////                this.Cmd.CommandType = CommandType.StoredProcedure;
////////                this.Cmd.Parameters.Clear();
////////                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
////////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
////////                FieldInfo.idIndicacao = (int)this.Cmd.Parameters["@Param_idIndicacao"].Value;
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
////////        /// Grava/Persiste as alterações em um objeto IndicacaoFields no banco de dados
////////        /// </summary>
////////        /// <param name="FieldInfo">Objeto IndicacaoFields a ser alterado.</param>
////////        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////////        public bool Update( IndicacaoFields FieldInfo )
////////        {
////////            try
////////            {
////////                this.Conn = new SqlConnection(this.StrConnetionDB);
////////                this.Conn.Open();
////////                this.Tran = this.Conn.BeginTransaction();
////////                this.Cmd = new SqlCommand("Proc_Indicacao_Update", this.Conn, this.Tran);
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
////////        /// Grava/Persiste as alterações em um objeto IndicacaoFields no banco de dados
////////        /// </summary>
////////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
////////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
////////        /// <param name="FieldInfo">Objeto IndicacaoFields a ser alterado.</param>
////////        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////////        public bool Update( SqlConnection ConnIn, SqlTransaction TranIn, IndicacaoFields FieldInfo )
////////        {
////////            try
////////            {
////////                this.Cmd = new SqlCommand("Proc_Indicacao_Update", ConnIn, TranIn);
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
////////                this.Cmd = new SqlCommand("Proc_Indicacao_DeleteAll", this.Conn, this.Tran);
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
////////                this.Cmd = new SqlCommand("Proc_Indicacao_DeleteAll", ConnIn, TranIn);
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
////////        /// <param name="FieldInfo">Objeto IndicacaoFields a ser excluído.</param>
////////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////////        public bool Delete( IndicacaoFields FieldInfo )
////////        {
////////            return Delete(FieldInfo.idIndicacao);
////////        }
////////
////////        /// <summary> 
////////        /// Exclui um registro da tabela no banco de dados
////////        /// </summary>
////////        /// <param name="Param_idIndicacao">int</param>
////////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////////        public bool Delete(
////////                                     int Param_idIndicacao)
////////        {
////////            try
////////            {
////////                this.Conn = new SqlConnection(this.StrConnetionDB);
////////                this.Conn.Open();
////////                this.Tran = this.Conn.BeginTransaction();
////////                this.Cmd = new SqlCommand("Proc_Indicacao_Delete", this.Conn, this.Tran);
////////                this.Cmd.CommandType = CommandType.StoredProcedure;
////////                this.Cmd.Parameters.Clear();
////////                this.Cmd.Parameters.Add(new SqlParameter("@Param_idIndicacao", SqlDbType.Int)).Value = Param_idIndicacao;
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
////////        /// <param name="FieldInfo">Objeto IndicacaoFields a ser excluído.</param>
////////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////////        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, IndicacaoFields FieldInfo )
////////        {
////////            return Delete(ConnIn, TranIn, FieldInfo.idIndicacao);
////////        }
////////
////////        /// <summary> 
////////        /// Exclui um registro da tabela no banco de dados
////////        /// </summary>
////////        /// <param name="Param_idIndicacao">int</param>
////////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
////////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
////////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////////        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, 
////////                                     int Param_idIndicacao)
////////        {
////////            try
////////            {
////////                this.Cmd = new SqlCommand("Proc_Indicacao_Delete", ConnIn, TranIn);
////////                this.Cmd.CommandType = CommandType.StoredProcedure;
////////                this.Cmd.Parameters.Clear();
////////                this.Cmd.Parameters.Add(new SqlParameter("@Param_idIndicacao", SqlDbType.Int)).Value = Param_idIndicacao;
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
////////        /// Retorna um objeto IndicacaoFields através da chave primária passada como parâmetro
////////        /// </summary>
////////        /// <param name="Param_idIndicacao">int</param>
////////        /// <returns>Objeto IndicacaoFields</returns> 
////////        public IndicacaoFields GetItem(
////////                                     int Param_idIndicacao)
////////        {
////////            IndicacaoFields infoFields = new IndicacaoFields();
////////            try
////////            {
////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////////                {
////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_Select", this.Conn))
////////                    {
////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////////                        this.Cmd.Parameters.Clear();
////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_idIndicacao", SqlDbType.Int)).Value = Param_idIndicacao;
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
////////        /// Seleciona todos os registro da tabela e preenche um ArrayList com o objeto IndicacaoFields.
////////        /// </summary>
////////        /// <returns>List de objetos IndicacaoFields</returns> 
////////        public List<IndicacaoFields> GetAll()
////////        {
////////            List<IndicacaoFields> arrayInfo = new List<IndicacaoFields>();
////////            try
////////            {
////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////////                {
////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_GetAll", this.Conn))
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
////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_CountAll", this.Conn))
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
////////public DataSet GetAllIndicacaoByModuloUsuario(UsuarioFields usuario,TipoIndicacao tipoIndicacao)
////////        {
////////            DataSet dsIndicacao = new DataSet();
////////            try
////////            {
////////                SqlConnection Conn = new SqlConnection(this.StrConnetionDB);
////////
////////                string query = GetQueryByModuloUser(usuario, tipoIndicacao);
////////
////////                Conn.Open();
////////                DataTable dt = new DataTable();
////////                SqlCommand Cmd = new SqlCommand(query, Conn);
////////                Cmd.CommandType = CommandType.Text;
////////                SqlDataAdapter da = new SqlDataAdapter(Cmd);
////////                da.Fill(dsIndicacao, "Usuario");
////////
////////                return dsIndicacao;
////////
////////            }
////////            catch (SqlException e)
////////            {
////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.", e.Message);
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
////////                    if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////////            }
////////        }
////////
////////        public enum TipoIndicacao
////////        { 
////////            Recebida,
////////            Indicada
////////        }
////////
////////        private string GetQueryByModuloUser(UsuarioFields usuario,TipoIndicacao tipoIndicacao)
////////        {
////////            
////////            if(usuario == null)
////////                throw new Exception("Modulo de usuário não encontrado.");
////////
////////            StringBuilder query = new StringBuilder();
////////            
////////            if(tipoIndicacao == TipoIndicacao.Indicada)
////////            {
////////                query.Append(" select distinct i.*, pi.NomeUsuarioIndica ");
////////                query.Append(" from PosicaoIndicacao pi, Indicacao i, UA ua, Usuario u");
////////                query.Append(" where pi.idPosicaoIndicacao = i.FkPosicaoIndicacao");
////////            }
////////
////////            if(tipoIndicacao == TipoIndicacao.Recebida)
////////            {
////////                query.Append(" select distinct i.*, pi.NomeUsuarioRecebe");
////////                query.Append(" from PosicaoIndicacao pi, Indicacao i, UA ua, Usuario u");
////////                query.Append(" where pi.idPosicaoIndicacao = i.FkPosicaoIndicacao");
////////            }
////////                
////////            switch (usuario.Modulo)
////////            {
////////                case "U":
////////                    query.AppendFormat(" And pi.NomeUsuarioRecebe = {0}", usuario.Nome);
////////                    break;
////////                
////////                case "M":
////////                    query.AppendFormat("  And ua.idUA = {0}", usuario.FkUa);
////////                    break;
////////                
////////            }
////////    
////////            return query.ToString();
////////        }
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
////////        /// <returns>IndicacaoFields</returns> 
////////        public IndicacaoFields FindByNome(
////////                               string Param_Nome )
////////        {
////////            IndicacaoFields infoFields = new IndicacaoFields();
////////            try
////////            {
////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////////                {
////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByNome", this.Conn))
////////                    {
////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////////                        this.Cmd.Parameters.Clear();
////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Nome", SqlDbType.VarChar, 150)).Value = Param_Nome;
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
////////                }
////////
////////                return infoFields;
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
////////        #region Selecionando dados da tabela através do campo "Telefone" 
////////
////////        /// <summary> 
////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Telefone.
////////        /// </summary>
////////        /// <param name="Param_Telefone">string</param>
////////        /// <returns>IndicacaoFields</returns> 
////////        public IndicacaoFields FindByTelefone(
////////                               string Param_Telefone )
////////        {
////////            IndicacaoFields infoFields = new IndicacaoFields();
////////            try
////////            {
////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////////                {
////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByTelefone", this.Conn))
////////                    {
////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////////                        this.Cmd.Parameters.Clear();
////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Telefone", SqlDbType.VarChar, 50)).Value = Param_Telefone;
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
////////                }
////////
////////                return infoFields;
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
////////        #region Selecionando dados da tabela através do campo "Endereco" 
////////
////////        /// <summary> 
////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Endereco.
////////        /// </summary>
////////        /// <param name="Param_Endereco">string</param>
////////        /// <returns>IndicacaoFields</returns> 
////////        public IndicacaoFields FindByEndereco(
////////                               string Param_Endereco )
////////        {
////////            IndicacaoFields infoFields = new IndicacaoFields();
////////            try
////////            {
////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////////                {
////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByEndereco", this.Conn))
////////                    {
////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////////                        this.Cmd.Parameters.Clear();
////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Endereco", SqlDbType.VarChar, 150)).Value = Param_Endereco;
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
////////                }
////////
////////                return infoFields;
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
////////        #region Selecionando dados da tabela através do campo "Bairro" 
////////
////////        /// <summary> 
////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Bairro.
////////        /// </summary>
////////        /// <param name="Param_Bairro">string</param>
////////        /// <returns>IndicacaoFields</returns> 
////////        public IndicacaoFields FindByBairro(
////////                               string Param_Bairro )
////////        {
////////            IndicacaoFields infoFields = new IndicacaoFields();
////////            try
////////            {
////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////////                {
////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByBairro", this.Conn))
////////                    {
////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////////                        this.Cmd.Parameters.Clear();
////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Bairro", SqlDbType.VarChar, 150)).Value = Param_Bairro;
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
////////                }
////////
////////                return infoFields;
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
////////        #region Selecionando dados da tabela através do campo "Cidade" 
////////
////////        /// <summary> 
////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Cidade.
////////        /// </summary>
////////        /// <param name="Param_Cidade">string</param>
////////        /// <returns>IndicacaoFields</returns> 
////////        public IndicacaoFields FindByCidade(
////////                               string Param_Cidade )
////////        {
////////            IndicacaoFields infoFields = new IndicacaoFields();
////////            try
////////            {
////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////////                {
////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByCidade", this.Conn))
////////                    {
////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////////                        this.Cmd.Parameters.Clear();
////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Cidade", SqlDbType.VarChar, 150)).Value = Param_Cidade;
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
////////                }
////////
////////                return infoFields;
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
////////        #region Selecionando dados da tabela através do campo "Estado" 
////////
////////        /// <summary> 
////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Estado.
////////        /// </summary>
////////        /// <param name="Param_Estado">string</param>
////////        /// <returns>IndicacaoFields</returns> 
////////        public IndicacaoFields FindByEstado(
////////                               string Param_Estado )
////////        {
////////            IndicacaoFields infoFields = new IndicacaoFields();
////////            try
////////            {
////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////////                {
////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByEstado", this.Conn))
////////                    {
////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////////                        this.Cmd.Parameters.Clear();
////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Estado", SqlDbType.VarChar, 150)).Value = Param_Estado;
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
////////                }
////////
////////                return infoFields;
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
////////        #region Selecionando dados da tabela através do campo "idUsuarioRecebe" 
////////
////////        /// <summary> 
////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo idUsuarioRecebe.
////////        /// </summary>
////////        /// <param name="Param_idUsuarioRecebe">int</param>
////////        /// <returns>IndicacaoFields</returns> 
////////        public IndicacaoFields FindByidUsuarioRecebe(
////////                               int Param_idUsuarioRecebe )
////////        {
////////            IndicacaoFields infoFields = new IndicacaoFields();
////////            try
////////            {
////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////////                {
////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByidUsuarioRecebe", this.Conn))
////////                    {
////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////////                        this.Cmd.Parameters.Clear();
////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_idUsuarioRecebe", SqlDbType.Int)).Value = Param_idUsuarioRecebe;
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
////////                }
////////
////////                return infoFields;
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
////////        #region Selecionando dados da tabela através do campo "idUsuarioIndica" 
////////
////////        /// <summary> 
////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo idUsuarioIndica.
////////        /// </summary>
////////        /// <param name="Param_idUsuarioIndica">int</param>
////////        /// <returns>IndicacaoFields</returns> 
////////        public IndicacaoFields FindByidUsuarioIndica(
////////                               int Param_idUsuarioIndica )
////////        {
////////            IndicacaoFields infoFields = new IndicacaoFields();
////////            try
////////            {
////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////////                {
////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByidUsuarioIndica", this.Conn))
////////                    {
////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////////                        this.Cmd.Parameters.Clear();
////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_idUsuarioIndica", SqlDbType.Int)).Value = Param_idUsuarioIndica;
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
////////                }
////////
////////                return infoFields;
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
////////        /// Retorna um objeto IndicacaoFields preenchido com os valores dos campos do SqlDataReader
////////        /// </summary>
////////        /// <param name="dr">SqlDataReader - Preenche o objeto IndicacaoFields </param>
////////        /// <returns>IndicacaoFields</returns>
////////        private IndicacaoFields GetDataFromReader( SqlDataReader dr )
////////        {
////////            IndicacaoFields infoFields = new IndicacaoFields();
////////
////////            if (!dr.IsDBNull(0))
////////            { infoFields.idIndicacao = dr.GetInt32(0); }
////////            else
////////            { infoFields.idIndicacao = 0; }
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
////////            { infoFields.Telefone = dr.GetString(2); }
////////            else
////////            { infoFields.Telefone = string.Empty; }
////////
////////
////////
////////            if (!dr.IsDBNull(3))
////////            { infoFields.Endereco = dr.GetString(3); }
////////            else
////////            { infoFields.Endereco = string.Empty; }
////////
////////
////////
////////            if (!dr.IsDBNull(4))
////////            { infoFields.Bairro = dr.GetString(4); }
////////            else
////////            { infoFields.Bairro = string.Empty; }
////////
////////
////////
////////            if (!dr.IsDBNull(5))
////////            { infoFields.Cidade = dr.GetString(5); }
////////            else
////////            { infoFields.Cidade = string.Empty; }
////////
////////
////////
////////            if (!dr.IsDBNull(6))
////////            { infoFields.Estado = dr.GetString(6); }
////////            else
////////            { infoFields.Estado = string.Empty; }
////////
////////
////////
////////            if (!dr.IsDBNull(7))
////////            { infoFields.idUsuarioRecebe = dr.GetInt32(7); }
////////            else
////////            { infoFields.idUsuarioRecebe = 0; }
////////
////////
////////
////////            if (!dr.IsDBNull(8))
////////            { infoFields.idUsuarioIndica = dr.GetInt32(8); }
////////            else
////////            { infoFields.idUsuarioIndica = 0; }
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
////////
////////
////////        #region Função GetAllParameters
////////
////////        /// <summary> 
////////        /// Retorna um array de parâmetros com campos para atualização, seleção e inserção no banco de dados
////////        /// </summary>
////////        /// <param name="FieldInfo">Objeto IndicacaoFields</param>
////////        /// <param name="Modo">Tipo de oepração a ser executada no banco de dados</param>
////////        /// <returns>SqlParameter[] - Array de parâmetros</returns> 
////////        private SqlParameter[] GetAllParameters( IndicacaoFields FieldInfo, SQLMode Modo )
////////        {
////////            SqlParameter[] Parameters;
////////
////////            switch (Modo)
////////            {
////////                case SQLMode.Add:
////////                    Parameters = new SqlParameter[9];
////////                    for (int I = 0; I < Parameters.Length; I++)
////////                       Parameters[I] = new SqlParameter();
////////                    //Field idIndicacao
////////                    Parameters[0].SqlDbType = SqlDbType.Int;
////////                    Parameters[0].Direction = ParameterDirection.Output;
////////                    Parameters[0].ParameterName = "@Param_idIndicacao";
////////                    Parameters[0].Value = DBNull.Value;
////////
////////                    break;
////////
////////                case SQLMode.Update:
////////                    Parameters = new SqlParameter[9];
////////                    for (int I = 0; I < Parameters.Length; I++)
////////                       Parameters[I] = new SqlParameter();
////////                    //Field idIndicacao
////////                    Parameters[0].SqlDbType = SqlDbType.Int;
////////                    Parameters[0].ParameterName = "@Param_idIndicacao";
////////                    Parameters[0].Value = FieldInfo.idIndicacao;
////////
////////                    break;
////////
////////                case SQLMode.SelectORDelete:
////////                    Parameters = new SqlParameter[1];
////////                    for (int I = 0; I < Parameters.Length; I++)
////////                       Parameters[I] = new SqlParameter();
////////                    //Field idIndicacao
////////                    Parameters[0].SqlDbType = SqlDbType.Int;
////////                    Parameters[0].ParameterName = "@Param_idIndicacao";
////////                    Parameters[0].Value = FieldInfo.idIndicacao;
////////
////////                    return Parameters;
////////
////////                default:
////////                    Parameters = new SqlParameter[9];
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
////////            Parameters[1].Size = 150;
////////
////////            //Field Telefone
////////            Parameters[2].SqlDbType = SqlDbType.VarChar;
////////            Parameters[2].ParameterName = "@Param_Telefone";
////////            if (( FieldInfo.Telefone == null ) || ( FieldInfo.Telefone == string.Empty ))
////////            { Parameters[2].Value = DBNull.Value; }
////////            else
////////            { Parameters[2].Value = FieldInfo.Telefone; }
////////            Parameters[2].Size = 50;
////////
////////            //Field Endereco
////////            Parameters[3].SqlDbType = SqlDbType.VarChar;
////////            Parameters[3].ParameterName = "@Param_Endereco";
////////            if (( FieldInfo.Endereco == null ) || ( FieldInfo.Endereco == string.Empty ))
////////            { Parameters[3].Value = DBNull.Value; }
////////            else
////////            { Parameters[3].Value = FieldInfo.Endereco; }
////////            Parameters[3].Size = 150;
////////
////////            //Field Bairro
////////            Parameters[4].SqlDbType = SqlDbType.VarChar;
////////            Parameters[4].ParameterName = "@Param_Bairro";
////////            if (( FieldInfo.Bairro == null ) || ( FieldInfo.Bairro == string.Empty ))
////////            { Parameters[4].Value = DBNull.Value; }
////////            else
////////            { Parameters[4].Value = FieldInfo.Bairro; }
////////            Parameters[4].Size = 150;
////////
////////            //Field Cidade
////////            Parameters[5].SqlDbType = SqlDbType.VarChar;
////////            Parameters[5].ParameterName = "@Param_Cidade";
////////            if (( FieldInfo.Cidade == null ) || ( FieldInfo.Cidade == string.Empty ))
////////            { Parameters[5].Value = DBNull.Value; }
////////            else
////////            { Parameters[5].Value = FieldInfo.Cidade; }
////////            Parameters[5].Size = 150;
////////
////////            //Field Estado
////////            Parameters[6].SqlDbType = SqlDbType.VarChar;
////////            Parameters[6].ParameterName = "@Param_Estado";
////////            if (( FieldInfo.Estado == null ) || ( FieldInfo.Estado == string.Empty ))
////////            { Parameters[6].Value = DBNull.Value; }
////////            else
////////            { Parameters[6].Value = FieldInfo.Estado; }
////////            Parameters[6].Size = 150;
////////
////////            //Field idUsuarioRecebe
////////            Parameters[7].SqlDbType = SqlDbType.Int;
////////            Parameters[7].ParameterName = "@Param_idUsuarioRecebe";
////////            Parameters[7].Value = FieldInfo.idUsuarioRecebe;
////////
////////            //Field idUsuarioIndica
////////            Parameters[8].SqlDbType = SqlDbType.Int;
////////            Parameters[8].ParameterName = "@Param_idUsuarioIndica";
////////            Parameters[8].Value = FieldInfo.idUsuarioIndica;
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
////////        ~IndicacaoControl() 
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
//////////    /// Tabela: Indicacao  
//////////    /// Autor: DAL Creator .net 
//////////    /// Data de criação: 19/03/2012 23:02:06 
//////////    /// Descrição: Classe responsável pela perssitência de dados. Utiliza a classe "IndicacaoFields". 
//////////    /// </summary> 
//////////    public class IndicacaoControl : IDisposable 
//////////    {
//////////
//////////        #region String de conexão 
//////////        private string StrConnetionDB = ConfigurationManager.ConnectionStrings["StringConn"].ToString();
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
//////////        public IndicacaoControl() {}
//////////
//////////
//////////        #region Inserindo dados na tabela 
//////////
//////////        /// <summary> 
//////////        /// Grava/Persiste um novo objeto IndicacaoFields no banco de dados
//////////        /// </summary>
//////////        /// <param name="FieldInfo">Objeto IndicacaoFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
//////////        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////////        public bool Add( ref IndicacaoFields FieldInfo )
//////////        {
//////////            try
//////////            {
//////////                this.Conn = new SqlConnection(this.StrConnetionDB);
//////////                this.Conn.Open();
//////////                this.Tran = this.Conn.BeginTransaction();
//////////                this.Cmd = new SqlCommand("Proc_Indicacao_Add", this.Conn, this.Tran);
//////////                this.Cmd.CommandType = CommandType.StoredProcedure;
//////////                this.Cmd.Parameters.Clear();
//////////                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
//////////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
//////////                this.Tran.Commit();
//////////                FieldInfo.idIndicacao = (int)this.Cmd.Parameters["@Param_idIndicacao"].Value;
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
//////////        /// Grava/Persiste um novo objeto IndicacaoFields no banco de dados
//////////        /// </summary>
//////////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
//////////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
//////////        /// <param name="FieldInfo">Objeto IndicacaoFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
//////////        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////////        public bool Add( SqlConnection ConnIn, SqlTransaction TranIn, ref IndicacaoFields FieldInfo )
//////////        {
//////////            try
//////////            {
//////////                this.Cmd = new SqlCommand("Proc_Indicacao_Add", ConnIn, TranIn);
//////////                this.Cmd.CommandType = CommandType.StoredProcedure;
//////////                this.Cmd.Parameters.Clear();
//////////                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
//////////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
//////////                FieldInfo.idIndicacao = (int)this.Cmd.Parameters["@Param_idIndicacao"].Value;
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
//////////        /// Grava/Persiste as alterações em um objeto IndicacaoFields no banco de dados
//////////        /// </summary>
//////////        /// <param name="FieldInfo">Objeto IndicacaoFields a ser alterado.</param>
//////////        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////////        public bool Update( IndicacaoFields FieldInfo )
//////////        {
//////////            try
//////////            {
//////////                this.Conn = new SqlConnection(this.StrConnetionDB);
//////////                this.Conn.Open();
//////////                this.Tran = this.Conn.BeginTransaction();
//////////                this.Cmd = new SqlCommand("Proc_Indicacao_Update", this.Conn, this.Tran);
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
//////////        /// Grava/Persiste as alterações em um objeto IndicacaoFields no banco de dados
//////////        /// </summary>
//////////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
//////////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
//////////        /// <param name="FieldInfo">Objeto IndicacaoFields a ser alterado.</param>
//////////        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////////        public bool Update( SqlConnection ConnIn, SqlTransaction TranIn, IndicacaoFields FieldInfo )
//////////        {
//////////            try
//////////            {
//////////                this.Cmd = new SqlCommand("Proc_Indicacao_Update", ConnIn, TranIn);
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
//////////                this.Cmd = new SqlCommand("Proc_Indicacao_DeleteAll", this.Conn, this.Tran);
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
//////////                this.Cmd = new SqlCommand("Proc_Indicacao_DeleteAll", ConnIn, TranIn);
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
//////////        /// <param name="FieldInfo">Objeto IndicacaoFields a ser excluído.</param>
//////////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////////        public bool Delete( IndicacaoFields FieldInfo )
//////////        {
//////////            return Delete(FieldInfo.idIndicacao);
//////////        }
//////////
//////////        /// <summary> 
//////////        /// Exclui um registro da tabela no banco de dados
//////////        /// </summary>
//////////        /// <param name="Param_idIndicacao">int</param>
//////////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////////        public bool Delete(
//////////                                     int Param_idIndicacao)
//////////        {
//////////            try
//////////            {
//////////                this.Conn = new SqlConnection(this.StrConnetionDB);
//////////                this.Conn.Open();
//////////                this.Tran = this.Conn.BeginTransaction();
//////////                this.Cmd = new SqlCommand("Proc_Indicacao_Delete", this.Conn, this.Tran);
//////////                this.Cmd.CommandType = CommandType.StoredProcedure;
//////////                this.Cmd.Parameters.Clear();
//////////                this.Cmd.Parameters.Add(new SqlParameter("@Param_idIndicacao", SqlDbType.Int)).Value = Param_idIndicacao;
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
//////////        /// <param name="FieldInfo">Objeto IndicacaoFields a ser excluído.</param>
//////////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////////        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, IndicacaoFields FieldInfo )
//////////        {
//////////            return Delete(ConnIn, TranIn, FieldInfo.idIndicacao);
//////////        }
//////////
//////////        /// <summary> 
//////////        /// Exclui um registro da tabela no banco de dados
//////////        /// </summary>
//////////        /// <param name="Param_idIndicacao">int</param>
//////////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
//////////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
//////////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////////        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, 
//////////                                     int Param_idIndicacao)
//////////        {
//////////            try
//////////            {
//////////                this.Cmd = new SqlCommand("Proc_Indicacao_Delete", ConnIn, TranIn);
//////////                this.Cmd.CommandType = CommandType.StoredProcedure;
//////////                this.Cmd.Parameters.Clear();
//////////                this.Cmd.Parameters.Add(new SqlParameter("@Param_idIndicacao", SqlDbType.Int)).Value = Param_idIndicacao;
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
//////////        /// Retorna um objeto IndicacaoFields através da chave primária passada como parâmetro
//////////        /// </summary>
//////////        /// <param name="Param_idIndicacao">int</param>
//////////        /// <returns>Objeto IndicacaoFields</returns> 
//////////        public IndicacaoFields GetItem(
//////////                                     int Param_idIndicacao)
//////////        {
//////////            IndicacaoFields infoFields = new IndicacaoFields();
//////////            try
//////////            {
//////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////////                {
//////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_Select", this.Conn))
//////////                    {
//////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////////                        this.Cmd.Parameters.Clear();
//////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_idIndicacao", SqlDbType.Int)).Value = Param_idIndicacao;
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
//////////        /// Seleciona todos os registro da tabela e preenche um ArrayList com o objeto IndicacaoFields.
//////////        /// </summary>
//////////        /// <returns>List de objetos IndicacaoFields</returns> 
//////////        public List<IndicacaoFields> GetAll()
//////////        {
//////////            List<IndicacaoFields> arrayInfo = new List<IndicacaoFields>();
//////////            try
//////////            {
//////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////////                {
//////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_GetAll", this.Conn))
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
//////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_CountAll", this.Conn))
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
//////////        /// <returns>IndicacaoFields</returns> 
//////////        public IndicacaoFields FindByNome(
//////////                               string Param_Nome )
//////////        {
//////////            IndicacaoFields infoFields = new IndicacaoFields();
//////////            try
//////////            {
//////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////////                {
//////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByNome", this.Conn))
//////////                    {
//////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////////                        this.Cmd.Parameters.Clear();
//////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Nome", SqlDbType.VarChar, 150)).Value = Param_Nome;
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
//////////                }
//////////
//////////                return infoFields;
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
//////////        #region Selecionando dados da tabela através do campo "Endereco" 
//////////
//////////        /// <summary> 
//////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Endereco.
//////////        /// </summary>
//////////        /// <param name="Param_Endereco">string</param>
//////////        /// <returns>IndicacaoFields</returns> 
//////////        public IndicacaoFields FindByEndereco(
//////////                               string Param_Endereco )
//////////        {
//////////            IndicacaoFields infoFields = new IndicacaoFields();
//////////            try
//////////            {
//////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////////                {
//////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByEndereco", this.Conn))
//////////                    {
//////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////////                        this.Cmd.Parameters.Clear();
//////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Endereco", SqlDbType.VarChar, 200)).Value = Param_Endereco;
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
//////////                }
//////////
//////////                return infoFields;
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
//////////        #region Selecionando dados da tabela através do campo "Bairro" 
//////////
//////////        /// <summary> 
//////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Bairro.
//////////        /// </summary>
//////////        /// <param name="Param_Bairro">string</param>
//////////        /// <returns>IndicacaoFields</returns> 
//////////        public IndicacaoFields FindByBairro(
//////////                               string Param_Bairro )
//////////        {
//////////            IndicacaoFields infoFields = new IndicacaoFields();
//////////            try
//////////            {
//////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////////                {
//////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByBairro", this.Conn))
//////////                    {
//////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////////                        this.Cmd.Parameters.Clear();
//////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Bairro", SqlDbType.VarChar, 100)).Value = Param_Bairro;
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
//////////                }
//////////
//////////                return infoFields;
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
//////////        #region Selecionando dados da tabela através do campo "Cidade" 
//////////
//////////        /// <summary> 
//////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Cidade.
//////////        /// </summary>
//////////        /// <param name="Param_Cidade">string</param>
//////////        /// <returns>IndicacaoFields</returns> 
//////////        public IndicacaoFields FindByCidade(
//////////                               string Param_Cidade )
//////////        {
//////////            IndicacaoFields infoFields = new IndicacaoFields();
//////////            try
//////////            {
//////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////////                {
//////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByCidade", this.Conn))
//////////                    {
//////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////////                        this.Cmd.Parameters.Clear();
//////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Cidade", SqlDbType.VarChar, 150)).Value = Param_Cidade;
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
//////////                }
//////////
//////////                return infoFields;
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
//////////        #region Selecionando dados da tabela através do campo "Estado" 
//////////
//////////        /// <summary> 
//////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Estado.
//////////        /// </summary>
//////////        /// <param name="Param_Estado">string</param>
//////////        /// <returns>IndicacaoFields</returns> 
//////////        public IndicacaoFields FindByEstado(
//////////                               string Param_Estado )
//////////        {
//////////            IndicacaoFields infoFields = new IndicacaoFields();
//////////            try
//////////            {
//////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////////                {
//////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByEstado", this.Conn))
//////////                    {
//////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////////                        this.Cmd.Parameters.Clear();
//////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Estado", SqlDbType.VarChar, 2)).Value = Param_Estado;
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
//////////                }
//////////
//////////                return infoFields;
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
//////////        #region Selecionando dados da tabela através do campo "Telefone" 
//////////
//////////        /// <summary> 
//////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Telefone.
//////////        /// </summary>
//////////        /// <param name="Param_Telefone">string</param>
//////////        /// <returns>IndicacaoFields</returns> 
//////////        public IndicacaoFields FindByTelefone(
//////////                               string Param_Telefone )
//////////        {
//////////            IndicacaoFields infoFields = new IndicacaoFields();
//////////            try
//////////            {
//////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////////                {
//////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByTelefone", this.Conn))
//////////                    {
//////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////////                        this.Cmd.Parameters.Clear();
//////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Telefone", SqlDbType.VarChar, 11)).Value = Param_Telefone;
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
//////////                }
//////////
//////////                return infoFields;
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
//////////        #region Selecionando dados da tabela através do campo "FkPosicaoIndicacao" 
//////////
//////////        /// <summary> 
//////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo FkPosicaoIndicacao.
//////////        /// </summary>
//////////        /// <param name="Param_FkPosicaoIndicacao">int</param>
//////////        /// <returns>IndicacaoFields</returns> 
//////////        public IndicacaoFields FindByFkPosicaoIndicacao(
//////////                               int Param_FkPosicaoIndicacao )
//////////        {
//////////            IndicacaoFields infoFields = new IndicacaoFields();
//////////            try
//////////            {
//////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////////                {
//////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByFkPosicaoIndicacao", this.Conn))
//////////                    {
//////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////////                        this.Cmd.Parameters.Clear();
//////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_FkPosicaoIndicacao", SqlDbType.Int)).Value = Param_FkPosicaoIndicacao;
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
//////////                }
//////////
//////////                return infoFields;
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
//////////        /// Retorna um objeto IndicacaoFields preenchido com os valores dos campos do SqlDataReader
//////////        /// </summary>
//////////        /// <param name="dr">SqlDataReader - Preenche o objeto IndicacaoFields </param>
//////////        /// <returns>IndicacaoFields</returns>
//////////        private IndicacaoFields GetDataFromReader( SqlDataReader dr )
//////////        {
//////////            IndicacaoFields infoFields = new IndicacaoFields();
//////////
//////////            if (!dr.IsDBNull(0))
//////////            { infoFields.idIndicacao = dr.GetInt32(0); }
//////////            else
//////////            { infoFields.idIndicacao = 0; }
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
//////////            { infoFields.Endereco = dr.GetString(2); }
//////////            else
//////////            { infoFields.Endereco = string.Empty; }
//////////
//////////
//////////
//////////            if (!dr.IsDBNull(3))
//////////            { infoFields.Bairro = dr.GetString(3); }
//////////            else
//////////            { infoFields.Bairro = string.Empty; }
//////////
//////////
//////////
//////////            if (!dr.IsDBNull(4))
//////////            { infoFields.Cidade = dr.GetString(4); }
//////////            else
//////////            { infoFields.Cidade = string.Empty; }
//////////
//////////
//////////
//////////            if (!dr.IsDBNull(5))
//////////            { infoFields.Estado = dr.GetString(5); }
//////////            else
//////////            { infoFields.Estado = string.Empty; }
//////////
//////////
//////////
//////////            if (!dr.IsDBNull(6))
//////////            { infoFields.Telefone = dr.GetString(6); }
//////////            else
//////////            { infoFields.Telefone = string.Empty; }
//////////
//////////
//////////
//////////            if (!dr.IsDBNull(7))
//////////            { infoFields.FkPosicaoIndicacao = dr.GetInt32(7); }
//////////            else
//////////            { infoFields.FkPosicaoIndicacao = 0; }
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
//////////
//////////
//////////
//////////
//////////        #region Função GetAllParameters
//////////
//////////        /// <summary> 
//////////        /// Retorna um array de parâmetros com campos para atualização, seleção e inserção no banco de dados
//////////        /// </summary>
//////////        /// <param name="FieldInfo">Objeto IndicacaoFields</param>
//////////        /// <param name="Modo">Tipo de oepração a ser executada no banco de dados</param>
//////////        /// <returns>SqlParameter[] - Array de parâmetros</returns> 
//////////        private SqlParameter[] GetAllParameters( IndicacaoFields FieldInfo, SQLMode Modo )
//////////        {
//////////            SqlParameter[] Parameters;
//////////
//////////            switch (Modo)
//////////            {
//////////                case SQLMode.Add:
//////////                    Parameters = new SqlParameter[8];
//////////                    for (int I = 0; I < Parameters.Length; I++)
//////////                       Parameters[I] = new SqlParameter();
//////////                    //Field idIndicacao
//////////                    Parameters[0].SqlDbType = SqlDbType.Int;
//////////                    Parameters[0].Direction = ParameterDirection.Output;
//////////                    Parameters[0].ParameterName = "@Param_idIndicacao";
//////////                    Parameters[0].Value = DBNull.Value;
//////////
//////////                    break;
//////////
//////////                case SQLMode.Update:
//////////                    Parameters = new SqlParameter[8];
//////////                    for (int I = 0; I < Parameters.Length; I++)
//////////                       Parameters[I] = new SqlParameter();
//////////                    //Field idIndicacao
//////////                    Parameters[0].SqlDbType = SqlDbType.Int;
//////////                    Parameters[0].ParameterName = "@Param_idIndicacao";
//////////                    Parameters[0].Value = FieldInfo.idIndicacao;
//////////
//////////                    break;
//////////
//////////                case SQLMode.SelectORDelete:
//////////                    Parameters = new SqlParameter[1];
//////////                    for (int I = 0; I < Parameters.Length; I++)
//////////                       Parameters[I] = new SqlParameter();
//////////                    //Field idIndicacao
//////////                    Parameters[0].SqlDbType = SqlDbType.Int;
//////////                    Parameters[0].ParameterName = "@Param_idIndicacao";
//////////                    Parameters[0].Value = FieldInfo.idIndicacao;
//////////
//////////                    return Parameters;
//////////
//////////                default:
//////////                    Parameters = new SqlParameter[8];
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
//////////            Parameters[1].Size = 150;
//////////
//////////            //Field Endereco
//////////            Parameters[2].SqlDbType = SqlDbType.VarChar;
//////////            Parameters[2].ParameterName = "@Param_Endereco";
//////////            if (( FieldInfo.Endereco == null ) || ( FieldInfo.Endereco == string.Empty ))
//////////            { Parameters[2].Value = DBNull.Value; }
//////////            else
//////////            { Parameters[2].Value = FieldInfo.Endereco; }
//////////            Parameters[2].Size = 200;
//////////
//////////            //Field Bairro
//////////            Parameters[3].SqlDbType = SqlDbType.VarChar;
//////////            Parameters[3].ParameterName = "@Param_Bairro";
//////////            if (( FieldInfo.Bairro == null ) || ( FieldInfo.Bairro == string.Empty ))
//////////            { Parameters[3].Value = DBNull.Value; }
//////////            else
//////////            { Parameters[3].Value = FieldInfo.Bairro; }
//////////            Parameters[3].Size = 100;
//////////
//////////            //Field Cidade
//////////            Parameters[4].SqlDbType = SqlDbType.VarChar;
//////////            Parameters[4].ParameterName = "@Param_Cidade";
//////////            if (( FieldInfo.Cidade == null ) || ( FieldInfo.Cidade == string.Empty ))
//////////            { Parameters[4].Value = DBNull.Value; }
//////////            else
//////////            { Parameters[4].Value = FieldInfo.Cidade; }
//////////            Parameters[4].Size = 150;
//////////
//////////            //Field Estado
//////////            Parameters[5].SqlDbType = SqlDbType.VarChar;
//////////            Parameters[5].ParameterName = "@Param_Estado";
//////////            if (( FieldInfo.Estado == null ) || ( FieldInfo.Estado == string.Empty ))
//////////            { Parameters[5].Value = DBNull.Value; }
//////////            else
//////////            { Parameters[5].Value = FieldInfo.Estado; }
//////////            Parameters[5].Size = 2;
//////////
//////////            //Field Telefone
//////////            Parameters[6].SqlDbType = SqlDbType.VarChar;
//////////            Parameters[6].ParameterName = "@Param_Telefone";
//////////            if (( FieldInfo.Telefone == null ) || ( FieldInfo.Telefone == string.Empty ))
//////////            { Parameters[6].Value = DBNull.Value; }
//////////            else
//////////            { Parameters[6].Value = FieldInfo.Telefone; }
//////////            Parameters[6].Size = 11;
//////////
//////////            //Field FkPosicaoIndicacao
//////////            Parameters[7].SqlDbType = SqlDbType.Int;
//////////            Parameters[7].ParameterName = "@Param_FkPosicaoIndicacao";
//////////            Parameters[7].Value = FieldInfo.FkPosicaoIndicacao;
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
//////////        ~IndicacaoControl() 
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
//////////
//////////
//////////
//////////
//////////
////////////Projeto substituído ------------------------
////////////using System;
////////////using System.Collections;
////////////using System.Data;
////////////using System.Data.SqlClient;
////////////using System.Configuration;
////////////using System.Text;
////////////
////////////namespace SWGPgen
////////////{
////////////
////////////
////////////    /// <summary> 
////////////    /// Tabela: Indicacao  
////////////    /// Autor: DAL Creator .net 
////////////    /// Data de criação: 19/03/2012 22:46:51 
////////////    /// Descrição: Classe responsável pela perssitência de dados. Utiliza a classe "IndicacaoFields". 
////////////    /// </summary> 
////////////    public class IndicacaoControl : IDisposable 
////////////    {
////////////
////////////        #region String de conexão 
////////////        private string StrConnetionDB = ConfigurationManager.ConnectionStrings["Data Source=DEKO-PC;Initial Catalog=swgp;User Id=sureg;Password=@sureg2012;"].ToString();
////////////        #endregion
////////////
////////////
////////////        #region Propriedade que armazena erros de execução 
////////////        private string _ErrorMessage;
////////////        public string ErrorMessage { get { return _ErrorMessage; } }
////////////        #endregion
////////////
////////////
////////////        #region Objetos de conexão 
////////////        SqlConnection Conn;
////////////        SqlCommand Cmd;
////////////        SqlTransaction Tran;
////////////        #endregion
////////////
////////////
////////////        #region Funcões que retornam Conexões e Transações 
////////////
////////////        public SqlTransaction GetNewTransaction(SqlConnection connIn)
////////////        {
////////////            if (connIn.State != ConnectionState.Open)
////////////                connIn.Open();
////////////            SqlTransaction TranOut = connIn.BeginTransaction();
////////////            return TranOut;
////////////        }
////////////
////////////        public SqlConnection GetNewConnection()
////////////        {
////////////            return GetNewConnection(this.StrConnetionDB);
////////////        }
////////////
////////////        public SqlConnection GetNewConnection(string StringConnection)
////////////        {
////////////            SqlConnection connOut = new SqlConnection(StringConnection);
////////////            return connOut;
////////////        }
////////////
////////////        #endregion
////////////
////////////
////////////        #region enum SQLMode 
////////////        /// <summary>   
////////////        /// Representa o procedimento que está sendo executado na tabela.
////////////        /// </summary>
////////////        public enum SQLMode
////////////        {                     
////////////            /// <summary>
////////////            /// Adiciona registro na tabela.
////////////            /// </summary>
////////////            Add,
////////////            /// <summary>
////////////            /// Atualiza registro na tabela.
////////////            /// </summary>
////////////            Update,
////////////            /// <summary>
////////////            /// Excluir registro na tabela
////////////            /// </summary>
////////////            Delete,
////////////            /// <summary>
////////////            /// Exclui TODOS os registros da tabela.
////////////            /// </summary>
////////////            DeleteAll,
////////////            /// <summary>
////////////            /// Seleciona um registro na tabela.
////////////            /// </summary>
////////////            Select,
////////////            /// <summary>
////////////            /// Seleciona TODOS os registros da tabela.
////////////            /// </summary>
////////////            SelectAll,
////////////            /// <summary>
////////////            /// Excluir ou seleciona um registro na tabela.
////////////            /// </summary>
////////////            SelectORDelete
////////////        }
////////////        #endregion 
////////////
////////////
////////////        public IndicacaoControl() {}
////////////
////////////
////////////        #region Inserindo dados na tabela 
////////////
////////////        /// <summary> 
////////////        /// Grava/Persiste um novo objeto IndicacaoFields no banco de dados
////////////        /// </summary>
////////////        /// <param name="FieldInfo">Objeto IndicacaoFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
////////////        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////////////        public bool Add( ref IndicacaoFields FieldInfo )
////////////        {
////////////            try
////////////            {
////////////                this.Conn = new SqlConnection(this.StrConnetionDB);
////////////                this.Conn.Open();
////////////                this.Tran = this.Conn.BeginTransaction();
////////////                this.Cmd = new SqlCommand("Proc_Indicacao_Add", this.Conn, this.Tran);
////////////                this.Cmd.CommandType = CommandType.StoredProcedure;
////////////                this.Cmd.Parameters.Clear();
////////////                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
////////////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
////////////                this.Tran.Commit();
////////////                FieldInfo.idIndicacao = (int)this.Cmd.Parameters["@Param_idIndicacao"].Value;
////////////                return true;
////////////
////////////            }
////////////            catch (SqlException e)
////////////            {
////////////                this.Tran.Rollback();
////////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: {0}.",  e.Message);
////////////                return false;
////////////            }
////////////            catch (Exception e)
////////////            {
////////////                this.Tran.Rollback();
////////////                this._ErrorMessage = e.Message;
////////////                return false;
////////////            }
////////////            finally
////////////            {
////////////                if (this.Conn != null)
////////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////////////                if (this.Cmd != null)
////////////                  this.Cmd.Dispose();
////////////            }
////////////        }
////////////
////////////        #endregion
////////////
////////////
////////////        #region Inserindo dados na tabela utilizando conexão e transação externa (compartilhada) 
////////////
////////////        /// <summary> 
////////////        /// Grava/Persiste um novo objeto IndicacaoFields no banco de dados
////////////        /// </summary>
////////////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
////////////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
////////////        /// <param name="FieldInfo">Objeto IndicacaoFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
////////////        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////////////        public bool Add( SqlConnection ConnIn, SqlTransaction TranIn, ref IndicacaoFields FieldInfo )
////////////        {
////////////            try
////////////            {
////////////                this.Cmd = new SqlCommand("Proc_Indicacao_Add", ConnIn, TranIn);
////////////                this.Cmd.CommandType = CommandType.StoredProcedure;
////////////                this.Cmd.Parameters.Clear();
////////////                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
////////////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
////////////                FieldInfo.idIndicacao = (int)this.Cmd.Parameters["@Param_idIndicacao"].Value;
////////////                return true;
////////////
////////////            }
////////////            catch (SqlException e)
////////////            {
////////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: {0}.",  e.Message);
////////////                return false;
////////////            }
////////////            catch (Exception e)
////////////            {
////////////                this._ErrorMessage = e.Message;
////////////                return false;
////////////            }
////////////        }
////////////
////////////        #endregion
////////////
////////////
////////////        #region Editando dados na tabela 
////////////
////////////        /// <summary> 
////////////        /// Grava/Persiste as alterações em um objeto IndicacaoFields no banco de dados
////////////        /// </summary>
////////////        /// <param name="FieldInfo">Objeto IndicacaoFields a ser alterado.</param>
////////////        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////////////        public bool Update( IndicacaoFields FieldInfo )
////////////        {
////////////            try
////////////            {
////////////                this.Conn = new SqlConnection(this.StrConnetionDB);
////////////                this.Conn.Open();
////////////                this.Tran = this.Conn.BeginTransaction();
////////////                this.Cmd = new SqlCommand("Proc_Indicacao_Update", this.Conn, this.Tran);
////////////                this.Cmd.CommandType = CommandType.StoredProcedure;
////////////                this.Cmd.Parameters.Clear();
////////////                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Update));
////////////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar atualizar registro!!");
////////////                this.Tran.Commit();
////////////                return true;
////////////            }
////////////            catch (SqlException e)
////////////            {
////////////                this.Tran.Rollback();
////////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: {0}.",  e.Message);
////////////                return false;
////////////            }
////////////            catch (Exception e)
////////////            {
////////////                this.Tran.Rollback();
////////////                this._ErrorMessage = e.Message;
////////////                return false;
////////////            }
////////////            finally
////////////            {
////////////                if (this.Conn != null)
////////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////////////                if (this.Cmd != null)
////////////                  this.Cmd.Dispose();
////////////            }
////////////        }
////////////
////////////        #endregion
////////////
////////////
////////////        #region Editando dados na tabela utilizando conexão e transação externa (compartilhada) 
////////////
////////////        /// <summary> 
////////////        /// Grava/Persiste as alterações em um objeto IndicacaoFields no banco de dados
////////////        /// </summary>
////////////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
////////////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
////////////        /// <param name="FieldInfo">Objeto IndicacaoFields a ser alterado.</param>
////////////        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////////////        public bool Update( SqlConnection ConnIn, SqlTransaction TranIn, IndicacaoFields FieldInfo )
////////////        {
////////////            try
////////////            {
////////////                this.Cmd = new SqlCommand("Proc_Indicacao_Update", ConnIn, TranIn);
////////////                this.Cmd.CommandType = CommandType.StoredProcedure;
////////////                this.Cmd.Parameters.Clear();
////////////                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Update));
////////////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar atualizar registro!!");
////////////                return true;
////////////            }
////////////            catch (SqlException e)
////////////            {
////////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: {0}.",  e.Message);
////////////                return false;
////////////            }
////////////            catch (Exception e)
////////////            {
////////////                this._ErrorMessage = e.Message;
////////////                return false;
////////////            }
////////////        }
////////////
////////////        #endregion
////////////
////////////
////////////        #region Excluindo todos os dados da tabela 
////////////
////////////        /// <summary> 
////////////        /// Exclui todos os registros da tabela
////////////        /// </summary>
////////////        /// <returns>"true" = registros excluidos com sucesso, "false" = erro ao tentar excluir registros (consulte a propriedade ErrorMessage para detalhes)</returns> 
////////////        public bool DeleteAll()
////////////        {
////////////            try
////////////            {
////////////                this.Conn = new SqlConnection(this.StrConnetionDB);
////////////                this.Conn.Open();
////////////                this.Tran = this.Conn.BeginTransaction();
////////////                this.Cmd = new SqlCommand("Proc_Indicacao_DeleteAll", this.Conn, this.Tran);
////////////                this.Cmd.CommandType = CommandType.StoredProcedure;
////////////                this.Cmd.Parameters.Clear();
////////////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
////////////                this.Tran.Commit();
////////////                return true;
////////////            }
////////////            catch (SqlException e)
////////////            {
////////////                this.Tran.Rollback();
////////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: {0}.",  e.Message);
////////////                return false;
////////////            }
////////////            catch (Exception e)
////////////            {
////////////                this.Tran.Rollback();
////////////                this._ErrorMessage = e.Message;
////////////                return false;
////////////            }
////////////            finally
////////////            {
////////////                if (this.Conn != null)
////////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////////////                if (this.Cmd != null)
////////////                  this.Cmd.Dispose();
////////////            }
////////////        }
////////////
////////////        #endregion
////////////
////////////
////////////        #region Excluindo todos os dados da tabela utilizando conexão e transação externa (compartilhada)
////////////
////////////        /// <summary> 
////////////        /// Exclui todos os registros da tabela
////////////        /// </summary>
////////////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
////////////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
////////////        /// <returns>"true" = registros excluidos com sucesso, "false" = erro ao tentar excluir registros (consulte a propriedade ErrorMessage para detalhes)</returns> 
////////////        public bool DeleteAll(SqlConnection ConnIn, SqlTransaction TranIn)
////////////        {
////////////            try
////////////            {
////////////                this.Cmd = new SqlCommand("Proc_Indicacao_DeleteAll", ConnIn, TranIn);
////////////                this.Cmd.CommandType = CommandType.StoredProcedure;
////////////                this.Cmd.Parameters.Clear();
////////////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
////////////                return true;
////////////            }
////////////            catch (SqlException e)
////////////            {
////////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: {0}.",  e.Message);
////////////                return false;
////////////            }
////////////            catch (Exception e)
////////////            {
////////////                this._ErrorMessage = e.Message;
////////////                return false;
////////////            }
////////////        }
////////////
////////////        #endregion
////////////
////////////
////////////        #region Excluindo dados da tabela 
////////////
////////////        /// <summary> 
////////////        /// Exclui um registro da tabela no banco de dados
////////////        /// </summary>
////////////        /// <param name="FieldInfo">Objeto IndicacaoFields a ser excluído.</param>
////////////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////////////        public bool Delete( IndicacaoFields FieldInfo )
////////////        {
////////////            return Delete(FieldInfo.idIndicacao);
////////////        }
////////////
////////////        /// <summary> 
////////////        /// Exclui um registro da tabela no banco de dados
////////////        /// </summary>
////////////        /// <param name="Param_idIndicacao">int</param>
////////////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////////////        public bool Delete(
////////////                                     int Param_idIndicacao)
////////////        {
////////////            try
////////////            {
////////////                this.Conn = new SqlConnection(this.StrConnetionDB);
////////////                this.Conn.Open();
////////////                this.Tran = this.Conn.BeginTransaction();
////////////                this.Cmd = new SqlCommand("Proc_Indicacao_Delete", this.Conn, this.Tran);
////////////                this.Cmd.CommandType = CommandType.StoredProcedure;
////////////                this.Cmd.Parameters.Clear();
////////////                this.Cmd.Parameters.Add(new SqlParameter("@Param_idIndicacao", SqlDbType.Int)).Value = Param_idIndicacao;
////////////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
////////////                this.Tran.Commit();
////////////                return true;
////////////            }
////////////            catch (SqlException e)
////////////            {
////////////                this.Tran.Rollback();
////////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: {0}.",  e.Message);
////////////                return false;
////////////            }
////////////            catch (Exception e)
////////////            {
////////////                this.Tran.Rollback();
////////////                this._ErrorMessage = e.Message;
////////////                return false;
////////////            }
////////////            finally
////////////            {
////////////                if (this.Conn != null)
////////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////////////                if (this.Cmd != null)
////////////                  this.Cmd.Dispose();
////////////            }
////////////        }
////////////
////////////        #endregion
////////////
////////////
////////////        #region Excluindo dados da tabela utilizando conexão e transação externa (compartilhada)
////////////
////////////        /// <summary> 
////////////        /// Exclui um registro da tabela no banco de dados
////////////        /// </summary>
////////////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
////////////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
////////////        /// <param name="FieldInfo">Objeto IndicacaoFields a ser excluído.</param>
////////////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////////////        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, IndicacaoFields FieldInfo )
////////////        {
////////////            return Delete(ConnIn, TranIn, FieldInfo.idIndicacao);
////////////        }
////////////
////////////        /// <summary> 
////////////        /// Exclui um registro da tabela no banco de dados
////////////        /// </summary>
////////////        /// <param name="Param_idIndicacao">int</param>
////////////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
////////////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
////////////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////////////        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, 
////////////                                     int Param_idIndicacao)
////////////        {
////////////            try
////////////            {
////////////                this.Cmd = new SqlCommand("Proc_Indicacao_Delete", ConnIn, TranIn);
////////////                this.Cmd.CommandType = CommandType.StoredProcedure;
////////////                this.Cmd.Parameters.Clear();
////////////                this.Cmd.Parameters.Add(new SqlParameter("@Param_idIndicacao", SqlDbType.Int)).Value = Param_idIndicacao;
////////////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
////////////                return true;
////////////            }
////////////            catch (SqlException e)
////////////            {
////////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: {0}.",  e.Message);
////////////                return false;
////////////            }
////////////            catch (Exception e)
////////////            {
////////////                this._ErrorMessage = e.Message;
////////////                return false;
////////////            }
////////////        }
////////////
////////////        #endregion
////////////
////////////
////////////        #region Selecionando um item da tabela 
////////////
////////////        /// <summary> 
////////////        /// Retorna um objeto IndicacaoFields através da chave primária passada como parâmetro
////////////        /// </summary>
////////////        /// <param name="Param_idIndicacao">int</param>
////////////        /// <returns>Objeto IndicacaoFields</returns> 
////////////        public IndicacaoFields GetItem(
////////////                                     int Param_idIndicacao)
////////////        {
////////////            IndicacaoFields infoFields = new IndicacaoFields();
////////////            try
////////////            {
////////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////////////                {
////////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_Select", this.Conn))
////////////                    {
////////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////////////                        this.Cmd.Parameters.Clear();
////////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_idIndicacao", SqlDbType.Int)).Value = Param_idIndicacao;
////////////                        this.Cmd.Connection.Open();
////////////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
////////////                        {
////////////                            if (!dr.HasRows) return null;
////////////                            if (dr.Read())
////////////                            {
////////////                               infoFields = GetDataFromReader( dr );
////////////                            }
////////////                        }
////////////                    }
////////////                 }
////////////
////////////                 return infoFields;
////////////
////////////            }
////////////            catch (SqlException e)
////////////            {
////////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
////////////                return null;
////////////            }
////////////            catch (Exception e)
////////////            {
////////////                this._ErrorMessage = e.Message;
////////////                return null;
////////////            }
////////////            finally
////////////            {
////////////                if (this.Conn != null)
////////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////////////            }
////////////        }
////////////
////////////        #endregion
////////////
////////////
////////////        #region Selecionando todos os dados da tabela 
////////////
////////////        /// <summary> 
////////////        /// Seleciona todos os registro da tabela e preenche um ArrayList com o objeto IndicacaoFields.
////////////        /// </summary>
////////////        /// <returns>ArrayList de objetos IndicacaoFields</returns> 
////////////        public ArrayList GetAll()
////////////        {
////////////            ArrayList arrayInfo = new ArrayList();
////////////            try
////////////            {
////////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////////////                {
////////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_GetAll", this.Conn))
////////////                    {
////////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////////////                        this.Cmd.Parameters.Clear();
////////////                        this.Cmd.Connection.Open();
////////////                        using (SqlDataReader dr = this.Cmd.ExecuteReader())
////////////                        {
////////////                           if (!dr.HasRows) return null;
////////////                           while (dr.Read())
////////////                           {
////////////                              arrayInfo.Add(GetDataFromReader( dr ));
////////////                           }
////////////                        }
////////////                    }
////////////                }
////////////
////////////                return arrayInfo;
////////////
////////////            }
////////////            catch (SqlException e)
////////////            {
////////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
////////////                return null;
////////////            }
////////////            catch (Exception e)
////////////            {
////////////                this._ErrorMessage = e.Message;
////////////                return null;
////////////            }
////////////            finally
////////////            {
////////////                if (this.Conn != null)
////////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////////////            }
////////////        }
////////////
////////////        #endregion
////////////
////////////
////////////        #region Contando os dados da tabela 
////////////
////////////        /// <summary> 
////////////        /// Retorna o total de registros contidos na tabela
////////////        /// </summary>
////////////        /// <returns>int</returns> 
////////////        public int CountAll()
////////////        {
////////////            try
////////////            {
////////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////////////                {
////////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_CountAll", this.Conn))
////////////                    {
////////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////////////                        this.Cmd.Parameters.Clear();
////////////                        this.Cmd.Connection.Open();
////////////                        object CountRegs = this.Cmd.ExecuteScalar();
////////////                        if (CountRegs == null)
////////////                        { return 0; }
////////////                        else
////////////                        { return (int)CountRegs; }
////////////                    }
////////////                }
////////////            }
////////////            catch (SqlException e)
////////////            {
////////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
////////////                return 0;
////////////            }
////////////            catch (Exception e)
////////////            {
////////////                this._ErrorMessage = e.Message;
////////////                return 0;
////////////            }
////////////            finally
////////////            {
////////////                if (this.Conn != null)
////////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////////////            }
////////////        }
////////////
////////////        #endregion
////////////
////////////
////////////        #region Selecionando dados da tabela através do campo "Nome" 
////////////
////////////        /// <summary> 
////////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Nome.
////////////        /// </summary>
////////////        /// <param name="Param_Nome">string</param>
////////////        /// <returns>IndicacaoFields</returns> 
////////////        public IndicacaoFields FindByNome(
////////////                               string Param_Nome )
////////////        {
////////////            IndicacaoFields infoFields = new IndicacaoFields();
////////////            try
////////////            {
////////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////////////                {
////////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByNome", this.Conn))
////////////                    {
////////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////////////                        this.Cmd.Parameters.Clear();
////////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Nome", SqlDbType.VarChar, 150)).Value = Param_Nome;
////////////                        this.Cmd.Connection.Open();
////////////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
////////////                        {
////////////                            if (!dr.HasRows) return null;
////////////                            if (dr.Read())
////////////                            {
////////////                               infoFields = GetDataFromReader( dr );
////////////                            }
////////////                        }
////////////                    }
////////////                }
////////////
////////////                return infoFields;
////////////
////////////            }
////////////            catch (SqlException e)
////////////            {
////////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
////////////                return null;
////////////            }
////////////            catch (Exception e)
////////////            {
////////////                this._ErrorMessage = e.Message;
////////////                return null;
////////////            }
////////////            finally
////////////            {
////////////                if (this.Conn != null)
////////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////////////            }
////////////        }
////////////
////////////        #endregion
////////////
////////////
////////////
////////////        #region Selecionando dados da tabela através do campo "Endereco" 
////////////
////////////        /// <summary> 
////////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Endereco.
////////////        /// </summary>
////////////        /// <param name="Param_Endereco">string</param>
////////////        /// <returns>IndicacaoFields</returns> 
////////////        public IndicacaoFields FindByEndereco(
////////////                               string Param_Endereco )
////////////        {
////////////            IndicacaoFields infoFields = new IndicacaoFields();
////////////            try
////////////            {
////////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////////////                {
////////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByEndereco", this.Conn))
////////////                    {
////////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////////////                        this.Cmd.Parameters.Clear();
////////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Endereco", SqlDbType.VarChar, 200)).Value = Param_Endereco;
////////////                        this.Cmd.Connection.Open();
////////////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
////////////                        {
////////////                            if (!dr.HasRows) return null;
////////////                            if (dr.Read())
////////////                            {
////////////                               infoFields = GetDataFromReader( dr );
////////////                            }
////////////                        }
////////////                    }
////////////                }
////////////
////////////                return infoFields;
////////////
////////////            }
////////////            catch (SqlException e)
////////////            {
////////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
////////////                return null;
////////////            }
////////////            catch (Exception e)
////////////            {
////////////                this._ErrorMessage = e.Message;
////////////                return null;
////////////            }
////////////            finally
////////////            {
////////////                if (this.Conn != null)
////////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////////////            }
////////////        }
////////////
////////////        #endregion
////////////
////////////
////////////
////////////        #region Selecionando dados da tabela através do campo "Bairro" 
////////////
////////////        /// <summary> 
////////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Bairro.
////////////        /// </summary>
////////////        /// <param name="Param_Bairro">string</param>
////////////        /// <returns>IndicacaoFields</returns> 
////////////        public IndicacaoFields FindByBairro(
////////////                               string Param_Bairro )
////////////        {
////////////            IndicacaoFields infoFields = new IndicacaoFields();
////////////            try
////////////            {
////////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////////////                {
////////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByBairro", this.Conn))
////////////                    {
////////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////////////                        this.Cmd.Parameters.Clear();
////////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Bairro", SqlDbType.VarChar, 100)).Value = Param_Bairro;
////////////                        this.Cmd.Connection.Open();
////////////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
////////////                        {
////////////                            if (!dr.HasRows) return null;
////////////                            if (dr.Read())
////////////                            {
////////////                               infoFields = GetDataFromReader( dr );
////////////                            }
////////////                        }
////////////                    }
////////////                }
////////////
////////////                return infoFields;
////////////
////////////            }
////////////            catch (SqlException e)
////////////            {
////////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
////////////                return null;
////////////            }
////////////            catch (Exception e)
////////////            {
////////////                this._ErrorMessage = e.Message;
////////////                return null;
////////////            }
////////////            finally
////////////            {
////////////                if (this.Conn != null)
////////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////////////            }
////////////        }
////////////
////////////        #endregion
////////////
////////////
////////////
////////////        #region Selecionando dados da tabela através do campo "Cidade" 
////////////
////////////        /// <summary> 
////////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Cidade.
////////////        /// </summary>
////////////        /// <param name="Param_Cidade">string</param>
////////////        /// <returns>IndicacaoFields</returns> 
////////////        public IndicacaoFields FindByCidade(
////////////                               string Param_Cidade )
////////////        {
////////////            IndicacaoFields infoFields = new IndicacaoFields();
////////////            try
////////////            {
////////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////////////                {
////////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByCidade", this.Conn))
////////////                    {
////////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////////////                        this.Cmd.Parameters.Clear();
////////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Cidade", SqlDbType.VarChar, 150)).Value = Param_Cidade;
////////////                        this.Cmd.Connection.Open();
////////////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
////////////                        {
////////////                            if (!dr.HasRows) return null;
////////////                            if (dr.Read())
////////////                            {
////////////                               infoFields = GetDataFromReader( dr );
////////////                            }
////////////                        }
////////////                    }
////////////                }
////////////
////////////                return infoFields;
////////////
////////////            }
////////////            catch (SqlException e)
////////////            {
////////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
////////////                return null;
////////////            }
////////////            catch (Exception e)
////////////            {
////////////                this._ErrorMessage = e.Message;
////////////                return null;
////////////            }
////////////            finally
////////////            {
////////////                if (this.Conn != null)
////////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////////////            }
////////////        }
////////////
////////////        #endregion
////////////
////////////
////////////
////////////        #region Selecionando dados da tabela através do campo "Estado" 
////////////
////////////        /// <summary> 
////////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Estado.
////////////        /// </summary>
////////////        /// <param name="Param_Estado">string</param>
////////////        /// <returns>IndicacaoFields</returns> 
////////////        public IndicacaoFields FindByEstado(
////////////                               string Param_Estado )
////////////        {
////////////            IndicacaoFields infoFields = new IndicacaoFields();
////////////            try
////////////            {
////////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////////////                {
////////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByEstado", this.Conn))
////////////                    {
////////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////////////                        this.Cmd.Parameters.Clear();
////////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Estado", SqlDbType.VarChar, 2)).Value = Param_Estado;
////////////                        this.Cmd.Connection.Open();
////////////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
////////////                        {
////////////                            if (!dr.HasRows) return null;
////////////                            if (dr.Read())
////////////                            {
////////////                               infoFields = GetDataFromReader( dr );
////////////                            }
////////////                        }
////////////                    }
////////////                }
////////////
////////////                return infoFields;
////////////
////////////            }
////////////            catch (SqlException e)
////////////            {
////////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
////////////                return null;
////////////            }
////////////            catch (Exception e)
////////////            {
////////////                this._ErrorMessage = e.Message;
////////////                return null;
////////////            }
////////////            finally
////////////            {
////////////                if (this.Conn != null)
////////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////////////            }
////////////        }
////////////
////////////        #endregion
////////////
////////////
////////////
////////////        #region Selecionando dados da tabela através do campo "Telefone" 
////////////
////////////        /// <summary> 
////////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Telefone.
////////////        /// </summary>
////////////        /// <param name="Param_Telefone">string</param>
////////////        /// <returns>IndicacaoFields</returns> 
////////////        public IndicacaoFields FindByTelefone(
////////////                               string Param_Telefone )
////////////        {
////////////            IndicacaoFields infoFields = new IndicacaoFields();
////////////            try
////////////            {
////////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////////////                {
////////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByTelefone", this.Conn))
////////////                    {
////////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////////////                        this.Cmd.Parameters.Clear();
////////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Telefone", SqlDbType.VarChar, 11)).Value = Param_Telefone;
////////////                        this.Cmd.Connection.Open();
////////////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
////////////                        {
////////////                            if (!dr.HasRows) return null;
////////////                            if (dr.Read())
////////////                            {
////////////                               infoFields = GetDataFromReader( dr );
////////////                            }
////////////                        }
////////////                    }
////////////                }
////////////
////////////                return infoFields;
////////////
////////////            }
////////////            catch (SqlException e)
////////////            {
////////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
////////////                return null;
////////////            }
////////////            catch (Exception e)
////////////            {
////////////                this._ErrorMessage = e.Message;
////////////                return null;
////////////            }
////////////            finally
////////////            {
////////////                if (this.Conn != null)
////////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////////////            }
////////////        }
////////////
////////////        #endregion
////////////
////////////
////////////
////////////        #region Selecionando dados da tabela através do campo "FkPosicaoIndicacao" 
////////////
////////////        /// <summary> 
////////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo FkPosicaoIndicacao.
////////////        /// </summary>
////////////        /// <param name="Param_FkPosicaoIndicacao">int</param>
////////////        /// <returns>IndicacaoFields</returns> 
////////////        public IndicacaoFields FindByFkPosicaoIndicacao(
////////////                               int Param_FkPosicaoIndicacao )
////////////        {
////////////            IndicacaoFields infoFields = new IndicacaoFields();
////////////            try
////////////            {
////////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////////////                {
////////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByFkPosicaoIndicacao", this.Conn))
////////////                    {
////////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////////////                        this.Cmd.Parameters.Clear();
////////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_FkPosicaoIndicacao", SqlDbType.Int)).Value = Param_FkPosicaoIndicacao;
////////////                        this.Cmd.Connection.Open();
////////////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
////////////                        {
////////////                            if (!dr.HasRows) return null;
////////////                            if (dr.Read())
////////////                            {
////////////                               infoFields = GetDataFromReader( dr );
////////////                            }
////////////                        }
////////////                    }
////////////                }
////////////
////////////                return infoFields;
////////////
////////////            }
////////////            catch (SqlException e)
////////////            {
////////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
////////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
////////////                return null;
////////////            }
////////////            catch (Exception e)
////////////            {
////////////                this._ErrorMessage = e.Message;
////////////                return null;
////////////            }
////////////            finally
////////////            {
////////////                if (this.Conn != null)
////////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////////////            }
////////////        }
////////////
////////////        #endregion
////////////
////////////
////////////
////////////        #region Função GetDataFromReader
////////////
////////////        /// <summary> 
////////////        /// Retorna um objeto IndicacaoFields preenchido com os valores dos campos do SqlDataReader
////////////        /// </summary>
////////////        /// <param name="dr">SqlDataReader - Preenche o objeto IndicacaoFields </param>
////////////        /// <returns>IndicacaoFields</returns>
////////////        private IndicacaoFields GetDataFromReader( SqlDataReader dr )
////////////        {
////////////            IndicacaoFields infoFields = new IndicacaoFields();
////////////
////////////            if (!dr.IsDBNull(0))
////////////            { infoFields.idIndicacao = dr.GetInt32(0); }
////////////            else
////////////            { infoFields.idIndicacao = 0; }
////////////
////////////
////////////
////////////            if (!dr.IsDBNull(1))
////////////            { infoFields.Nome = dr.GetString(1); }
////////////            else
////////////            { infoFields.Nome = string.Empty; }
////////////
////////////
////////////
////////////            if (!dr.IsDBNull(2))
////////////            { infoFields.Endereco = dr.GetString(2); }
////////////            else
////////////            { infoFields.Endereco = string.Empty; }
////////////
////////////
////////////
////////////            if (!dr.IsDBNull(3))
////////////            { infoFields.Bairro = dr.GetString(3); }
////////////            else
////////////            { infoFields.Bairro = string.Empty; }
////////////
////////////
////////////
////////////            if (!dr.IsDBNull(4))
////////////            { infoFields.Cidade = dr.GetString(4); }
////////////            else
////////////            { infoFields.Cidade = string.Empty; }
////////////
////////////
////////////
////////////            if (!dr.IsDBNull(5))
////////////            { infoFields.Estado = dr.GetString(5); }
////////////            else
////////////            { infoFields.Estado = string.Empty; }
////////////
////////////
////////////
////////////            if (!dr.IsDBNull(6))
////////////            { infoFields.Telefone = dr.GetString(6); }
////////////            else
////////////            { infoFields.Telefone = string.Empty; }
////////////
////////////
////////////
////////////            if (!dr.IsDBNull(7))
////////////            { infoFields.FkPosicaoIndicacao = dr.GetInt32(7); }
////////////            else
////////////            { infoFields.FkPosicaoIndicacao = 0; }
////////////
////////////
////////////            return infoFields;
////////////        }
////////////        #endregion
////////////
////////////
////////////
////////////
////////////
////////////
////////////
////////////
////////////
////////////
////////////
////////////
////////////
////////////
////////////
////////////
////////////
////////////
////////////
////////////
////////////
////////////
////////////        #region Função GetAllParameters
////////////
////////////        /// <summary> 
////////////        /// Retorna um array de parâmetros com campos para atualização, seleção e inserção no banco de dados
////////////        /// </summary>
////////////        /// <param name="FieldInfo">Objeto IndicacaoFields</param>
////////////        /// <param name="Modo">Tipo de oepração a ser executada no banco de dados</param>
////////////        /// <returns>SqlParameter[] - Array de parâmetros</returns> 
////////////        private SqlParameter[] GetAllParameters( IndicacaoFields FieldInfo, SQLMode Modo )
////////////        {
////////////            SqlParameter[] Parameters;
////////////
////////////            switch (Modo)
////////////            {
////////////                case SQLMode.Add:
////////////                    Parameters = new SqlParameter[8];
////////////                    for (int I = 0; I < Parameters.Length; I++)
////////////                       Parameters[I] = new SqlParameter();
////////////                    //Field idIndicacao
////////////                    Parameters[0].SqlDbType = SqlDbType.Int;
////////////                    Parameters[0].Direction = ParameterDirection.Output;
////////////                    Parameters[0].ParameterName = "@Param_idIndicacao";
////////////                    Parameters[0].Value = DBNull.Value;
////////////
////////////                    break;
////////////
////////////                case SQLMode.Update:
////////////                    Parameters = new SqlParameter[8];
////////////                    for (int I = 0; I < Parameters.Length; I++)
////////////                       Parameters[I] = new SqlParameter();
////////////                    //Field idIndicacao
////////////                    Parameters[0].SqlDbType = SqlDbType.Int;
////////////                    Parameters[0].ParameterName = "@Param_idIndicacao";
////////////                    Parameters[0].Value = FieldInfo.idIndicacao;
////////////
////////////                    break;
////////////
////////////                case SQLMode.SelectORDelete:
////////////                    Parameters = new SqlParameter[1];
////////////                    for (int I = 0; I < Parameters.Length; I++)
////////////                       Parameters[I] = new SqlParameter();
////////////                    //Field idIndicacao
////////////                    Parameters[0].SqlDbType = SqlDbType.Int;
////////////                    Parameters[0].ParameterName = "@Param_idIndicacao";
////////////                    Parameters[0].Value = FieldInfo.idIndicacao;
////////////
////////////                    return Parameters;
////////////
////////////                default:
////////////                    Parameters = new SqlParameter[8];
////////////                    for (int I = 0; I < Parameters.Length; I++)
////////////                       Parameters[I] = new SqlParameter();
////////////                    break;
////////////            }
////////////
////////////            //Field Nome
////////////            Parameters[1].SqlDbType = SqlDbType.VarChar;
////////////            Parameters[1].ParameterName = "@Param_Nome";
////////////            if (( FieldInfo.Nome == null ) || ( FieldInfo.Nome == string.Empty ))
////////////            { Parameters[1].Value = DBNull.Value; }
////////////            else
////////////            { Parameters[1].Value = FieldInfo.Nome; }
////////////            Parameters[1].Size = 150;
////////////
////////////            //Field Endereco
////////////            Parameters[2].SqlDbType = SqlDbType.VarChar;
////////////            Parameters[2].ParameterName = "@Param_Endereco";
////////////            if (( FieldInfo.Endereco == null ) || ( FieldInfo.Endereco == string.Empty ))
////////////            { Parameters[2].Value = DBNull.Value; }
////////////            else
////////////            { Parameters[2].Value = FieldInfo.Endereco; }
////////////            Parameters[2].Size = 200;
////////////
////////////            //Field Bairro
////////////            Parameters[3].SqlDbType = SqlDbType.VarChar;
////////////            Parameters[3].ParameterName = "@Param_Bairro";
////////////            if (( FieldInfo.Bairro == null ) || ( FieldInfo.Bairro == string.Empty ))
////////////            { Parameters[3].Value = DBNull.Value; }
////////////            else
////////////            { Parameters[3].Value = FieldInfo.Bairro; }
////////////            Parameters[3].Size = 100;
////////////
////////////            //Field Cidade
////////////            Parameters[4].SqlDbType = SqlDbType.VarChar;
////////////            Parameters[4].ParameterName = "@Param_Cidade";
////////////            if (( FieldInfo.Cidade == null ) || ( FieldInfo.Cidade == string.Empty ))
////////////            { Parameters[4].Value = DBNull.Value; }
////////////            else
////////////            { Parameters[4].Value = FieldInfo.Cidade; }
////////////            Parameters[4].Size = 150;
////////////
////////////            //Field Estado
////////////            Parameters[5].SqlDbType = SqlDbType.VarChar;
////////////            Parameters[5].ParameterName = "@Param_Estado";
////////////            if (( FieldInfo.Estado == null ) || ( FieldInfo.Estado == string.Empty ))
////////////            { Parameters[5].Value = DBNull.Value; }
////////////            else
////////////            { Parameters[5].Value = FieldInfo.Estado; }
////////////            Parameters[5].Size = 2;
////////////
////////////            //Field Telefone
////////////            Parameters[6].SqlDbType = SqlDbType.VarChar;
////////////            Parameters[6].ParameterName = "@Param_Telefone";
////////////            if (( FieldInfo.Telefone == null ) || ( FieldInfo.Telefone == string.Empty ))
////////////            { Parameters[6].Value = DBNull.Value; }
////////////            else
////////////            { Parameters[6].Value = FieldInfo.Telefone; }
////////////            Parameters[6].Size = 11;
////////////
////////////            //Field FkPosicaoIndicacao
////////////            Parameters[7].SqlDbType = SqlDbType.Int;
////////////            Parameters[7].ParameterName = "@Param_FkPosicaoIndicacao";
////////////            Parameters[7].Value = FieldInfo.FkPosicaoIndicacao;
////////////
////////////            return Parameters;
////////////        }
////////////        #endregion
////////////
////////////
////////////
////////////
////////////
////////////        #region IDisposable Members 
////////////
////////////        bool disposed = false;
////////////
////////////        public void Dispose()
////////////        {
////////////            Dispose(true);
////////////            GC.SuppressFinalize(this);
////////////        }
////////////
////////////        ~IndicacaoControl() 
////////////        { 
////////////            Dispose(false); 
////////////        }
////////////
////////////        private void Dispose(bool disposing) 
////////////        {
////////////            if (!this.disposed)
////////////            {
////////////                if (disposing) 
////////////                {
////////////                    if (this.Conn != null)
////////////                        if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
////////////                }
////////////            }
////////////
////////////        }
////////////        #endregion 
////////////
////////////
////////////
////////////    }
////////////
////////////}
////////////
////////////
////////////
////////////
////////////
//////////////Projeto substituído ------------------------
//////////////using System;
//////////////using System.Collections;
//////////////using System.Collections.Generic;
//////////////using System.Data;
//////////////using System.Data.SqlClient;
//////////////using System.Configuration;
//////////////using System.Text;
//////////////
//////////////namespace SWGPgen
//////////////{
//////////////
//////////////
//////////////    /// <summary> 
//////////////    /// Tabela: Indicacao  
//////////////    /// Autor: DAL Creator .net 
//////////////    /// Data de criação: 13/03/2012 21:19:06 
//////////////    /// Descrição: Classe responsável pela perssitência de dados. Utiliza a classe "IndicacaoFields". 
//////////////    /// </summary> 
//////////////    public class IndicacaoControl : IDisposable 
//////////////    {
//////////////
//////////////        #region String de conexão 
//////////////        private string StrConnetionDB = "Data Source=DEKO-PC;Initial Catalog=swgp;User Id=sureg;Password=@sureg2012;";
//////////////        #endregion
//////////////
//////////////
//////////////        #region Propriedade que armazena erros de execução 
//////////////        private string _ErrorMessage;
//////////////        public string ErrorMessage { get { return _ErrorMessage; } }
//////////////        #endregion
//////////////
//////////////
//////////////        #region Objetos de conexão 
//////////////        SqlConnection Conn;
//////////////        SqlCommand Cmd;
//////////////        SqlTransaction Tran;
//////////////        #endregion
//////////////
//////////////
//////////////        #region Funcões que retornam Conexões e Transações 
//////////////
//////////////        public SqlTransaction GetNewTransaction(SqlConnection connIn)
//////////////        {
//////////////            if (connIn.State != ConnectionState.Open)
//////////////                connIn.Open();
//////////////            SqlTransaction TranOut = connIn.BeginTransaction();
//////////////            return TranOut;
//////////////        }
//////////////
//////////////        public SqlConnection GetNewConnection()
//////////////        {
//////////////            return GetNewConnection(this.StrConnetionDB);
//////////////        }
//////////////
//////////////        public SqlConnection GetNewConnection(string StringConnection)
//////////////        {
//////////////            SqlConnection connOut = new SqlConnection(StringConnection);
//////////////            return connOut;
//////////////        }
//////////////
//////////////        #endregion
//////////////
//////////////
//////////////        #region enum SQLMode 
//////////////        /// <summary>   
//////////////        /// Representa o procedimento que está sendo executado na tabela.
//////////////        /// </summary>
//////////////        public enum SQLMode
//////////////        {                     
//////////////            /// <summary>
//////////////            /// Adiciona registro na tabela.
//////////////            /// </summary>
//////////////            Add,
//////////////            /// <summary>
//////////////            /// Atualiza registro na tabela.
//////////////            /// </summary>
//////////////            Update,
//////////////            /// <summary>
//////////////            /// Excluir registro na tabela
//////////////            /// </summary>
//////////////            Delete,
//////////////            /// <summary>
//////////////            /// Exclui TODOS os registros da tabela.
//////////////            /// </summary>
//////////////            DeleteAll,
//////////////            /// <summary>
//////////////            /// Seleciona um registro na tabela.
//////////////            /// </summary>
//////////////            Select,
//////////////            /// <summary>
//////////////            /// Seleciona TODOS os registros da tabela.
//////////////            /// </summary>
//////////////            SelectAll,
//////////////            /// <summary>
//////////////            /// Excluir ou seleciona um registro na tabela.
//////////////            /// </summary>
//////////////            SelectORDelete
//////////////        }
//////////////        #endregion 
//////////////
//////////////
//////////////        public IndicacaoControl() {}
//////////////
//////////////
//////////////        #region Inserindo dados na tabela 
//////////////
//////////////        /// <summary> 
//////////////        /// Grava/Persiste um novo objeto IndicacaoFields no banco de dados
//////////////        /// </summary>
//////////////        /// <param name="FieldInfo">Objeto IndicacaoFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
//////////////        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////////////        public bool Add( ref IndicacaoFields FieldInfo )
//////////////        {
//////////////            try
//////////////            {
//////////////                this.Conn = new SqlConnection(this.StrConnetionDB);
//////////////                this.Conn.Open();
//////////////                this.Tran = this.Conn.BeginTransaction();
//////////////                this.Cmd = new SqlCommand("Proc_Indicacao_Add", this.Conn, this.Tran);
//////////////                this.Cmd.CommandType = CommandType.StoredProcedure;
//////////////                this.Cmd.Parameters.Clear();
//////////////                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
//////////////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
//////////////                this.Tran.Commit();
//////////////                FieldInfo.idIndicacao = (int)this.Cmd.Parameters["@Param_idIndicacao"].Value;
//////////////                return true;
//////////////
//////////////            }
//////////////            catch (SqlException e)
//////////////            {
//////////////                this.Tran.Rollback();
//////////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: {0}.",  e.Message);
//////////////                return false;
//////////////            }
//////////////            catch (Exception e)
//////////////            {
//////////////                this.Tran.Rollback();
//////////////                this._ErrorMessage = e.Message;
//////////////                return false;
//////////////            }
//////////////            finally
//////////////            {
//////////////                if (this.Conn != null)
//////////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////////////                if (this.Cmd != null)
//////////////                  this.Cmd.Dispose();
//////////////            }
//////////////        }
//////////////
//////////////        #endregion
//////////////
//////////////
//////////////        #region Inserindo dados na tabela utilizando conexão e transação externa (compartilhada) 
//////////////
//////////////        /// <summary> 
//////////////        /// Grava/Persiste um novo objeto IndicacaoFields no banco de dados
//////////////        /// </summary>
//////////////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
//////////////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
//////////////        /// <param name="FieldInfo">Objeto IndicacaoFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
//////////////        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////////////        public bool Add( SqlConnection ConnIn, SqlTransaction TranIn, ref IndicacaoFields FieldInfo )
//////////////        {
//////////////            try
//////////////            {
//////////////                this.Cmd = new SqlCommand("Proc_Indicacao_Add", ConnIn, TranIn);
//////////////                this.Cmd.CommandType = CommandType.StoredProcedure;
//////////////                this.Cmd.Parameters.Clear();
//////////////                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
//////////////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
//////////////                FieldInfo.idIndicacao = (int)this.Cmd.Parameters["@Param_idIndicacao"].Value;
//////////////                return true;
//////////////
//////////////            }
//////////////            catch (SqlException e)
//////////////            {
//////////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: {0}.",  e.Message);
//////////////                return false;
//////////////            }
//////////////            catch (Exception e)
//////////////            {
//////////////                this._ErrorMessage = e.Message;
//////////////                return false;
//////////////            }
//////////////        }
//////////////
//////////////        #endregion
//////////////
//////////////
//////////////        #region Editando dados na tabela 
//////////////
//////////////        /// <summary> 
//////////////        /// Grava/Persiste as alterações em um objeto IndicacaoFields no banco de dados
//////////////        /// </summary>
//////////////        /// <param name="FieldInfo">Objeto IndicacaoFields a ser alterado.</param>
//////////////        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////////////        public bool Update( IndicacaoFields FieldInfo )
//////////////        {
//////////////            try
//////////////            {
//////////////                this.Conn = new SqlConnection(this.StrConnetionDB);
//////////////                this.Conn.Open();
//////////////                this.Tran = this.Conn.BeginTransaction();
//////////////                this.Cmd = new SqlCommand("Proc_Indicacao_Update", this.Conn, this.Tran);
//////////////                this.Cmd.CommandType = CommandType.StoredProcedure;
//////////////                this.Cmd.Parameters.Clear();
//////////////                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Update));
//////////////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar atualizar registro!!");
//////////////                this.Tran.Commit();
//////////////                return true;
//////////////            }
//////////////            catch (SqlException e)
//////////////            {
//////////////                this.Tran.Rollback();
//////////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: {0}.",  e.Message);
//////////////                return false;
//////////////            }
//////////////            catch (Exception e)
//////////////            {
//////////////                this.Tran.Rollback();
//////////////                this._ErrorMessage = e.Message;
//////////////                return false;
//////////////            }
//////////////            finally
//////////////            {
//////////////                if (this.Conn != null)
//////////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////////////                if (this.Cmd != null)
//////////////                  this.Cmd.Dispose();
//////////////            }
//////////////        }
//////////////
//////////////        #endregion
//////////////
//////////////
//////////////        #region Editando dados na tabela utilizando conexão e transação externa (compartilhada) 
//////////////
//////////////        /// <summary> 
//////////////        /// Grava/Persiste as alterações em um objeto IndicacaoFields no banco de dados
//////////////        /// </summary>
//////////////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
//////////////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
//////////////        /// <param name="FieldInfo">Objeto IndicacaoFields a ser alterado.</param>
//////////////        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////////////        public bool Update( SqlConnection ConnIn, SqlTransaction TranIn, IndicacaoFields FieldInfo )
//////////////        {
//////////////            try
//////////////            {
//////////////                this.Cmd = new SqlCommand("Proc_Indicacao_Update", ConnIn, TranIn);
//////////////                this.Cmd.CommandType = CommandType.StoredProcedure;
//////////////                this.Cmd.Parameters.Clear();
//////////////                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Update));
//////////////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar atualizar registro!!");
//////////////                return true;
//////////////            }
//////////////            catch (SqlException e)
//////////////            {
//////////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar atualizar o(s) registro(s) solicitados: {0}.",  e.Message);
//////////////                return false;
//////////////            }
//////////////            catch (Exception e)
//////////////            {
//////////////                this._ErrorMessage = e.Message;
//////////////                return false;
//////////////            }
//////////////        }
//////////////
//////////////        #endregion
//////////////
//////////////
//////////////        #region Excluindo todos os dados da tabela 
//////////////
//////////////        /// <summary> 
//////////////        /// Exclui todos os registros da tabela
//////////////        /// </summary>
//////////////        /// <returns>"true" = registros excluidos com sucesso, "false" = erro ao tentar excluir registros (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////////////        public bool DeleteAll()
//////////////        {
//////////////            try
//////////////            {
//////////////                this.Conn = new SqlConnection(this.StrConnetionDB);
//////////////                this.Conn.Open();
//////////////                this.Tran = this.Conn.BeginTransaction();
//////////////                this.Cmd = new SqlCommand("Proc_Indicacao_DeleteAll", this.Conn, this.Tran);
//////////////                this.Cmd.CommandType = CommandType.StoredProcedure;
//////////////                this.Cmd.Parameters.Clear();
//////////////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
//////////////                this.Tran.Commit();
//////////////                return true;
//////////////            }
//////////////            catch (SqlException e)
//////////////            {
//////////////                this.Tran.Rollback();
//////////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: {0}.",  e.Message);
//////////////                return false;
//////////////            }
//////////////            catch (Exception e)
//////////////            {
//////////////                this.Tran.Rollback();
//////////////                this._ErrorMessage = e.Message;
//////////////                return false;
//////////////            }
//////////////            finally
//////////////            {
//////////////                if (this.Conn != null)
//////////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////////////                if (this.Cmd != null)
//////////////                  this.Cmd.Dispose();
//////////////            }
//////////////        }
//////////////
//////////////        #endregion
//////////////
//////////////
//////////////        #region Excluindo todos os dados da tabela utilizando conexão e transação externa (compartilhada)
//////////////
//////////////        /// <summary> 
//////////////        /// Exclui todos os registros da tabela
//////////////        /// </summary>
//////////////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
//////////////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
//////////////        /// <returns>"true" = registros excluidos com sucesso, "false" = erro ao tentar excluir registros (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////////////        public bool DeleteAll(SqlConnection ConnIn, SqlTransaction TranIn)
//////////////        {
//////////////            try
//////////////            {
//////////////                this.Cmd = new SqlCommand("Proc_Indicacao_DeleteAll", ConnIn, TranIn);
//////////////                this.Cmd.CommandType = CommandType.StoredProcedure;
//////////////                this.Cmd.Parameters.Clear();
//////////////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
//////////////                return true;
//////////////            }
//////////////            catch (SqlException e)
//////////////            {
//////////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: {0}.",  e.Message);
//////////////                return false;
//////////////            }
//////////////            catch (Exception e)
//////////////            {
//////////////                this._ErrorMessage = e.Message;
//////////////                return false;
//////////////            }
//////////////        }
//////////////
//////////////        #endregion
//////////////
//////////////
//////////////        #region Excluindo dados da tabela 
//////////////
//////////////        /// <summary> 
//////////////        /// Exclui um registro da tabela no banco de dados
//////////////        /// </summary>
//////////////        /// <param name="FieldInfo">Objeto IndicacaoFields a ser excluído.</param>
//////////////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////////////        public bool Delete( IndicacaoFields FieldInfo )
//////////////        {
//////////////            return Delete(FieldInfo.idIndicacao);
//////////////        }
//////////////
//////////////        /// <summary> 
//////////////        /// Exclui um registro da tabela no banco de dados
//////////////        /// </summary>
//////////////        /// <param name="Param_idIndicacao">int</param>
//////////////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////////////        public bool Delete(
//////////////                                     int Param_idIndicacao)
//////////////        {
//////////////            try
//////////////            {
//////////////                this.Conn = new SqlConnection(this.StrConnetionDB);
//////////////                this.Conn.Open();
//////////////                this.Tran = this.Conn.BeginTransaction();
//////////////                this.Cmd = new SqlCommand("Proc_Indicacao_Delete", this.Conn, this.Tran);
//////////////                this.Cmd.CommandType = CommandType.StoredProcedure;
//////////////                this.Cmd.Parameters.Clear();
//////////////                this.Cmd.Parameters.Add(new SqlParameter("@Param_idIndicacao", SqlDbType.Int)).Value = Param_idIndicacao;
//////////////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
//////////////                this.Tran.Commit();
//////////////                return true;
//////////////            }
//////////////            catch (SqlException e)
//////////////            {
//////////////                this.Tran.Rollback();
//////////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: {0}.",  e.Message);
//////////////                return false;
//////////////            }
//////////////            catch (Exception e)
//////////////            {
//////////////                this.Tran.Rollback();
//////////////                this._ErrorMessage = e.Message;
//////////////                return false;
//////////////            }
//////////////            finally
//////////////            {
//////////////                if (this.Conn != null)
//////////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////////////                if (this.Cmd != null)
//////////////                  this.Cmd.Dispose();
//////////////            }
//////////////        }
//////////////
//////////////        #endregion
//////////////
//////////////
//////////////        #region Excluindo dados da tabela utilizando conexão e transação externa (compartilhada)
//////////////
//////////////        /// <summary> 
//////////////        /// Exclui um registro da tabela no banco de dados
//////////////        /// </summary>
//////////////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
//////////////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
//////////////        /// <param name="FieldInfo">Objeto IndicacaoFields a ser excluído.</param>
//////////////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////////////        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, IndicacaoFields FieldInfo )
//////////////        {
//////////////            return Delete(ConnIn, TranIn, FieldInfo.idIndicacao);
//////////////        }
//////////////
//////////////        /// <summary> 
//////////////        /// Exclui um registro da tabela no banco de dados
//////////////        /// </summary>
//////////////        /// <param name="Param_idIndicacao">int</param>
//////////////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
//////////////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
//////////////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//////////////        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, 
//////////////                                     int Param_idIndicacao)
//////////////        {
//////////////            try
//////////////            {
//////////////                this.Cmd = new SqlCommand("Proc_Indicacao_Delete", ConnIn, TranIn);
//////////////                this.Cmd.CommandType = CommandType.StoredProcedure;
//////////////                this.Cmd.Parameters.Clear();
//////////////                this.Cmd.Parameters.Add(new SqlParameter("@Param_idIndicacao", SqlDbType.Int)).Value = Param_idIndicacao;
//////////////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar excluir registro!!");
//////////////                return true;
//////////////            }
//////////////            catch (SqlException e)
//////////////            {
//////////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar excluir o(s) registro(s) solicitados: {0}.",  e.Message);
//////////////                return false;
//////////////            }
//////////////            catch (Exception e)
//////////////            {
//////////////                this._ErrorMessage = e.Message;
//////////////                return false;
//////////////            }
//////////////        }
//////////////
//////////////        #endregion
//////////////
//////////////
//////////////        #region Selecionando um item da tabela 
//////////////
//////////////        /// <summary> 
//////////////        /// Retorna um objeto IndicacaoFields através da chave primária passada como parâmetro
//////////////        /// </summary>
//////////////        /// <param name="Param_idIndicacao">int</param>
//////////////        /// <returns>Objeto IndicacaoFields</returns> 
//////////////        public IndicacaoFields GetItem(
//////////////                                     int Param_idIndicacao)
//////////////        {
//////////////            IndicacaoFields infoFields = new IndicacaoFields();
//////////////            try
//////////////            {
//////////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////////////                {
//////////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_Select", this.Conn))
//////////////                    {
//////////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////////////                        this.Cmd.Parameters.Clear();
//////////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_idIndicacao", SqlDbType.Int)).Value = Param_idIndicacao;
//////////////                        this.Cmd.Connection.Open();
//////////////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
//////////////                        {
//////////////                            if (!dr.HasRows) return null;
//////////////                            if (dr.Read())
//////////////                            {
//////////////                               infoFields = GetDataFromReader( dr );
//////////////                            }
//////////////                        }
//////////////                    }
//////////////                 }
//////////////
//////////////                 return infoFields;
//////////////
//////////////            }
//////////////            catch (SqlException e)
//////////////            {
//////////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
//////////////                return null;
//////////////            }
//////////////            catch (Exception e)
//////////////            {
//////////////                this._ErrorMessage = e.Message;
//////////////                return null;
//////////////            }
//////////////            finally
//////////////            {
//////////////                if (this.Conn != null)
//////////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////////////            }
//////////////        }
//////////////
//////////////        #endregion
//////////////
//////////////
//////////////        #region Selecionando todos os dados da tabela 
//////////////
//////////////        /// <summary> 
//////////////        /// Seleciona todos os registro da tabela e preenche um ArrayList com o objeto IndicacaoFields.
//////////////        /// </summary>
//////////////        /// <returns>List de objetos IndicacaoFields</returns> 
//////////////        public List<IndicacaoFields> GetAll()
//////////////        {
//////////////            List<IndicacaoFields> arrayInfo = new List<IndicacaoFields>();
//////////////            try
//////////////            {
//////////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////////////                {
//////////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_GetAll", this.Conn))
//////////////                    {
//////////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////////////                        this.Cmd.Parameters.Clear();
//////////////                        this.Cmd.Connection.Open();
//////////////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
//////////////                        {
//////////////                           if (!dr.HasRows) return null;
//////////////                           while (dr.Read())
//////////////                           {
//////////////                              arrayInfo.Add(GetDataFromReader( dr ));
//////////////                           }
//////////////                        }
//////////////                    }
//////////////                }
//////////////
//////////////                return arrayInfo;
//////////////
//////////////            }
//////////////            catch (SqlException e)
//////////////            {
//////////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
//////////////                return null;
//////////////            }
//////////////            catch (Exception e)
//////////////            {
//////////////                this._ErrorMessage = e.Message;
//////////////                return null;
//////////////            }
//////////////            finally
//////////////            {
//////////////                if (this.Conn != null)
//////////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////////////            }
//////////////        }
//////////////
//////////////        #endregion
//////////////
//////////////
//////////////        #region Contando os dados da tabela 
//////////////
//////////////        /// <summary> 
//////////////        /// Retorna o total de registros contidos na tabela
//////////////        /// </summary>
//////////////        /// <returns>int</returns> 
//////////////        public int CountAll()
//////////////        {
//////////////            try
//////////////            {
//////////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////////////                {
//////////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_CountAll", this.Conn))
//////////////                    {
//////////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////////////                        this.Cmd.Parameters.Clear();
//////////////                        this.Cmd.Connection.Open();
//////////////                        object CountRegs = this.Cmd.ExecuteScalar();
//////////////                        if (CountRegs == null)
//////////////                        { return 0; }
//////////////                        else
//////////////                        { return (int)CountRegs; }
//////////////                    }
//////////////                }
//////////////            }
//////////////            catch (SqlException e)
//////////////            {
//////////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
//////////////                return 0;
//////////////            }
//////////////            catch (Exception e)
//////////////            {
//////////////                this._ErrorMessage = e.Message;
//////////////                return 0;
//////////////            }
//////////////            finally
//////////////            {
//////////////                if (this.Conn != null)
//////////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////////////            }
//////////////        }
//////////////
//////////////        #endregion
//////////////
//////////////
//////////////        #region Selecionando dados da tabela através do campo "Nome" 
//////////////
//////////////        /// <summary> 
//////////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Nome.
//////////////        /// </summary>
//////////////        /// <param name="Param_Nome">string</param>
//////////////        /// <returns>ArrayList</returns> 
//////////////        public ArrayList FindByNome(
//////////////                               string Param_Nome )
//////////////        {
//////////////            ArrayList arrayInfo = new ArrayList();
//////////////            try
//////////////            {
//////////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////////////                {
//////////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByNome", this.Conn))
//////////////                    {
//////////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////////////                        this.Cmd.Parameters.Clear();
//////////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Nome", SqlDbType.VarChar, 150)).Value = Param_Nome;
//////////////                        this.Cmd.Connection.Open();
//////////////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
//////////////                        {
//////////////                            if (!dr.HasRows) return null;
//////////////                            while (dr.Read())
//////////////                            {
//////////////                               arrayInfo.Add(GetDataFromReader( dr ));
//////////////                            }
//////////////                        }
//////////////                    }
//////////////                }
//////////////
//////////////                return arrayInfo;
//////////////
//////////////            }
//////////////            catch (SqlException e)
//////////////            {
//////////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
//////////////                return null;
//////////////            }
//////////////            catch (Exception e)
//////////////            {
//////////////                this._ErrorMessage = e.Message;
//////////////                return null;
//////////////            }
//////////////            finally
//////////////            {
//////////////                if (this.Conn != null)
//////////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////////////            }
//////////////        }
//////////////
//////////////        #endregion
//////////////
//////////////
//////////////
//////////////        #region Selecionando dados da tabela através do campo "Endereco" 
//////////////
//////////////        /// <summary> 
//////////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Endereco.
//////////////        /// </summary>
//////////////        /// <param name="Param_Endereco">string</param>
//////////////        /// <returns>ArrayList</returns> 
//////////////        public ArrayList FindByEndereco(
//////////////                               string Param_Endereco )
//////////////        {
//////////////            ArrayList arrayInfo = new ArrayList();
//////////////            try
//////////////            {
//////////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////////////                {
//////////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByEndereco", this.Conn))
//////////////                    {
//////////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////////////                        this.Cmd.Parameters.Clear();
//////////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Endereco", SqlDbType.VarChar, 200)).Value = Param_Endereco;
//////////////                        this.Cmd.Connection.Open();
//////////////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
//////////////                        {
//////////////                            if (!dr.HasRows) return null;
//////////////                            while (dr.Read())
//////////////                            {
//////////////                               arrayInfo.Add(GetDataFromReader( dr ));
//////////////                            }
//////////////                        }
//////////////                    }
//////////////                }
//////////////
//////////////                return arrayInfo;
//////////////
//////////////            }
//////////////            catch (SqlException e)
//////////////            {
//////////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
//////////////                return null;
//////////////            }
//////////////            catch (Exception e)
//////////////            {
//////////////                this._ErrorMessage = e.Message;
//////////////                return null;
//////////////            }
//////////////            finally
//////////////            {
//////////////                if (this.Conn != null)
//////////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////////////            }
//////////////        }
//////////////
//////////////        #endregion
//////////////
//////////////
//////////////
//////////////        #region Selecionando dados da tabela através do campo "Bairro" 
//////////////
//////////////        /// <summary> 
//////////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Bairro.
//////////////        /// </summary>
//////////////        /// <param name="Param_Bairro">string</param>
//////////////        /// <returns>ArrayList</returns> 
//////////////        public ArrayList FindByBairro(
//////////////                               string Param_Bairro )
//////////////        {
//////////////            ArrayList arrayInfo = new ArrayList();
//////////////            try
//////////////            {
//////////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////////////                {
//////////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByBairro", this.Conn))
//////////////                    {
//////////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////////////                        this.Cmd.Parameters.Clear();
//////////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Bairro", SqlDbType.VarChar, 100)).Value = Param_Bairro;
//////////////                        this.Cmd.Connection.Open();
//////////////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
//////////////                        {
//////////////                            if (!dr.HasRows) return null;
//////////////                            while (dr.Read())
//////////////                            {
//////////////                               arrayInfo.Add(GetDataFromReader( dr ));
//////////////                            }
//////////////                        }
//////////////                    }
//////////////                }
//////////////
//////////////                return arrayInfo;
//////////////
//////////////            }
//////////////            catch (SqlException e)
//////////////            {
//////////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
//////////////                return null;
//////////////            }
//////////////            catch (Exception e)
//////////////            {
//////////////                this._ErrorMessage = e.Message;
//////////////                return null;
//////////////            }
//////////////            finally
//////////////            {
//////////////                if (this.Conn != null)
//////////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////////////            }
//////////////        }
//////////////
//////////////        #endregion
//////////////
//////////////
//////////////
//////////////        #region Selecionando dados da tabela através do campo "Cidade" 
//////////////
//////////////        /// <summary> 
//////////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Cidade.
//////////////        /// </summary>
//////////////        /// <param name="Param_Cidade">string</param>
//////////////        /// <returns>ArrayList</returns> 
//////////////        public ArrayList FindByCidade(
//////////////                               string Param_Cidade )
//////////////        {
//////////////            ArrayList arrayInfo = new ArrayList();
//////////////            try
//////////////            {
//////////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////////////                {
//////////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByCidade", this.Conn))
//////////////                    {
//////////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////////////                        this.Cmd.Parameters.Clear();
//////////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Cidade", SqlDbType.VarChar, 150)).Value = Param_Cidade;
//////////////                        this.Cmd.Connection.Open();
//////////////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
//////////////                        {
//////////////                            if (!dr.HasRows) return null;
//////////////                            while (dr.Read())
//////////////                            {
//////////////                               arrayInfo.Add(GetDataFromReader( dr ));
//////////////                            }
//////////////                        }
//////////////                    }
//////////////                }
//////////////
//////////////                return arrayInfo;
//////////////
//////////////            }
//////////////            catch (SqlException e)
//////////////            {
//////////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
//////////////                return null;
//////////////            }
//////////////            catch (Exception e)
//////////////            {
//////////////                this._ErrorMessage = e.Message;
//////////////                return null;
//////////////            }
//////////////            finally
//////////////            {
//////////////                if (this.Conn != null)
//////////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////////////            }
//////////////        }
//////////////
//////////////        #endregion
//////////////
//////////////
//////////////
//////////////        #region Selecionando dados da tabela através do campo "Estado" 
//////////////
//////////////        /// <summary> 
//////////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Estado.
//////////////        /// </summary>
//////////////        /// <param name="Param_Estado">string</param>
//////////////        /// <returns>ArrayList</returns> 
//////////////        public ArrayList FindByEstado(
//////////////                               string Param_Estado )
//////////////        {
//////////////            ArrayList arrayInfo = new ArrayList();
//////////////            try
//////////////            {
//////////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////////////                {
//////////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByEstado", this.Conn))
//////////////                    {
//////////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////////////                        this.Cmd.Parameters.Clear();
//////////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Estado", SqlDbType.VarChar, 2)).Value = Param_Estado;
//////////////                        this.Cmd.Connection.Open();
//////////////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
//////////////                        {
//////////////                            if (!dr.HasRows) return null;
//////////////                            while (dr.Read())
//////////////                            {
//////////////                               arrayInfo.Add(GetDataFromReader( dr ));
//////////////                            }
//////////////                        }
//////////////                    }
//////////////                }
//////////////
//////////////                return arrayInfo;
//////////////
//////////////            }
//////////////            catch (SqlException e)
//////////////            {
//////////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
//////////////                return null;
//////////////            }
//////////////            catch (Exception e)
//////////////            {
//////////////                this._ErrorMessage = e.Message;
//////////////                return null;
//////////////            }
//////////////            finally
//////////////            {
//////////////                if (this.Conn != null)
//////////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////////////            }
//////////////        }
//////////////
//////////////        #endregion
//////////////
//////////////
//////////////
//////////////        #region Selecionando dados da tabela através do campo "Telefone" 
//////////////
//////////////        /// <summary> 
//////////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Telefone.
//////////////        /// </summary>
//////////////        /// <param name="Param_Telefone">string</param>
//////////////        /// <returns>ArrayList</returns> 
//////////////        public ArrayList FindByTelefone(
//////////////                               string Param_Telefone )
//////////////        {
//////////////            ArrayList arrayInfo = new ArrayList();
//////////////            try
//////////////            {
//////////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////////////                {
//////////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByTelefone", this.Conn))
//////////////                    {
//////////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////////////                        this.Cmd.Parameters.Clear();
//////////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Telefone", SqlDbType.VarChar, 11)).Value = Param_Telefone;
//////////////                        this.Cmd.Connection.Open();
//////////////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
//////////////                        {
//////////////                            if (!dr.HasRows) return null;
//////////////                            while (dr.Read())
//////////////                            {
//////////////                               arrayInfo.Add(GetDataFromReader( dr ));
//////////////                            }
//////////////                        }
//////////////                    }
//////////////                }
//////////////
//////////////                return arrayInfo;
//////////////
//////////////            }
//////////////            catch (SqlException e)
//////////////            {
//////////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
//////////////                return null;
//////////////            }
//////////////            catch (Exception e)
//////////////            {
//////////////                this._ErrorMessage = e.Message;
//////////////                return null;
//////////////            }
//////////////            finally
//////////////            {
//////////////                if (this.Conn != null)
//////////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////////////            }
//////////////        }
//////////////
//////////////        #endregion
//////////////
//////////////
//////////////
//////////////        #region Selecionando dados da tabela através do campo "FkPosicaoIndicacao" 
//////////////
//////////////        /// <summary> 
//////////////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo FkPosicaoIndicacao.
//////////////        /// </summary>
//////////////        /// <param name="Param_FkPosicaoIndicacao">int</param>
//////////////        /// <returns>ArrayList</returns> 
//////////////        public ArrayList FindByFkPosicaoIndicacao(
//////////////                               int Param_FkPosicaoIndicacao )
//////////////        {
//////////////            ArrayList arrayInfo = new ArrayList();
//////////////            try
//////////////            {
//////////////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//////////////                {
//////////////                    using (this.Cmd = new SqlCommand("Proc_Indicacao_FindByFkPosicaoIndicacao", this.Conn))
//////////////                    {
//////////////                        this.Cmd.CommandType = CommandType.StoredProcedure;
//////////////                        this.Cmd.Parameters.Clear();
//////////////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_FkPosicaoIndicacao", SqlDbType.Int)).Value = Param_FkPosicaoIndicacao;
//////////////                        this.Cmd.Connection.Open();
//////////////                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
//////////////                        {
//////////////                            if (!dr.HasRows) return null;
//////////////                            while (dr.Read())
//////////////                            {
//////////////                               arrayInfo.Add(GetDataFromReader( dr ));
//////////////                            }
//////////////                        }
//////////////                    }
//////////////                }
//////////////
//////////////                return arrayInfo;
//////////////
//////////////            }
//////////////            catch (SqlException e)
//////////////            {
//////////////                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
//////////////                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.",  e.Message);
//////////////                return null;
//////////////            }
//////////////            catch (Exception e)
//////////////            {
//////////////                this._ErrorMessage = e.Message;
//////////////                return null;
//////////////            }
//////////////            finally
//////////////            {
//////////////                if (this.Conn != null)
//////////////                  if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////////////            }
//////////////        }
//////////////
//////////////        #endregion
//////////////
//////////////
//////////////
//////////////        #region Função GetDataFromReader
//////////////
//////////////        /// <summary> 
//////////////        /// Retorna um objeto IndicacaoFields preenchido com os valores dos campos do SqlDataReader
//////////////        /// </summary>
//////////////        /// <param name="dr">SqlDataReader - Preenche o objeto IndicacaoFields </param>
//////////////        /// <returns>IndicacaoFields</returns>
//////////////        private IndicacaoFields GetDataFromReader( SqlDataReader dr )
//////////////        {
//////////////            IndicacaoFields infoFields = new IndicacaoFields();
//////////////
//////////////            if (!dr.IsDBNull(0))
//////////////            { infoFields.idIndicacao = dr.GetInt32(0); }
//////////////            else
//////////////            { infoFields.idIndicacao = 0; }
//////////////
//////////////
//////////////
//////////////            if (!dr.IsDBNull(1))
//////////////            { infoFields.Nome = dr.GetString(1); }
//////////////            else
//////////////            { infoFields.Nome = string.Empty; }
//////////////
//////////////
//////////////
//////////////            if (!dr.IsDBNull(2))
//////////////            { infoFields.Endereco = dr.GetString(2); }
//////////////            else
//////////////            { infoFields.Endereco = string.Empty; }
//////////////
//////////////
//////////////
//////////////            if (!dr.IsDBNull(3))
//////////////            { infoFields.Bairro = dr.GetString(3); }
//////////////            else
//////////////            { infoFields.Bairro = string.Empty; }
//////////////
//////////////
//////////////
//////////////            if (!dr.IsDBNull(4))
//////////////            { infoFields.Cidade = dr.GetString(4); }
//////////////            else
//////////////            { infoFields.Cidade = string.Empty; }
//////////////
//////////////
//////////////
//////////////            if (!dr.IsDBNull(5))
//////////////            { infoFields.Estado = dr.GetString(5); }
//////////////            else
//////////////            { infoFields.Estado = string.Empty; }
//////////////
//////////////
//////////////
//////////////            if (!dr.IsDBNull(6))
//////////////            { infoFields.Telefone = dr.GetString(6); }
//////////////            else
//////////////            { infoFields.Telefone = string.Empty; }
//////////////
//////////////
//////////////
//////////////            if (!dr.IsDBNull(7))
//////////////            { infoFields.FkPosicaoIndicacao = dr.GetInt32(7); }
//////////////            else
//////////////            { infoFields.FkPosicaoIndicacao = 0; }
//////////////
//////////////
//////////////            return infoFields;
//////////////        }
//////////////        #endregion
//////////////
//////////////
//////////////
//////////////
//////////////
//////////////
//////////////
//////////////
//////////////
//////////////
//////////////
//////////////
//////////////
//////////////
//////////////
//////////////
//////////////
//////////////
//////////////
//////////////
//////////////
//////////////
//////////////        #region Função GetAllParameters
//////////////
//////////////        /// <summary> 
//////////////        /// Retorna um array de parâmetros com campos para atualização, seleção e inserção no banco de dados
//////////////        /// </summary>
//////////////        /// <param name="FieldInfo">Objeto IndicacaoFields</param>
//////////////        /// <param name="Modo">Tipo de oepração a ser executada no banco de dados</param>
//////////////        /// <returns>SqlParameter[] - Array de parâmetros</returns> 
//////////////        private SqlParameter[] GetAllParameters( IndicacaoFields FieldInfo, SQLMode Modo )
//////////////        {
//////////////            SqlParameter[] Parameters;
//////////////
//////////////            switch (Modo)
//////////////            {
//////////////                case SQLMode.Add:
//////////////                    Parameters = new SqlParameter[8];
//////////////                    for (int I = 0; I < Parameters.Length; I++)
//////////////                       Parameters[I] = new SqlParameter();
//////////////                    //Field idIndicacao
//////////////                    Parameters[0].SqlDbType = SqlDbType.Int;
//////////////                    Parameters[0].Direction = ParameterDirection.Output;
//////////////                    Parameters[0].ParameterName = "@Param_idIndicacao";
//////////////                    Parameters[0].Value = DBNull.Value;
//////////////
//////////////                    break;
//////////////
//////////////                case SQLMode.Update:
//////////////                    Parameters = new SqlParameter[8];
//////////////                    for (int I = 0; I < Parameters.Length; I++)
//////////////                       Parameters[I] = new SqlParameter();
//////////////                    //Field idIndicacao
//////////////                    Parameters[0].SqlDbType = SqlDbType.Int;
//////////////                    Parameters[0].ParameterName = "@Param_idIndicacao";
//////////////                    Parameters[0].Value = FieldInfo.idIndicacao;
//////////////
//////////////                    break;
//////////////
//////////////                case SQLMode.SelectORDelete:
//////////////                    Parameters = new SqlParameter[1];
//////////////                    for (int I = 0; I < Parameters.Length; I++)
//////////////                       Parameters[I] = new SqlParameter();
//////////////                    //Field idIndicacao
//////////////                    Parameters[0].SqlDbType = SqlDbType.Int;
//////////////                    Parameters[0].ParameterName = "@Param_idIndicacao";
//////////////                    Parameters[0].Value = FieldInfo.idIndicacao;
//////////////
//////////////                    return Parameters;
//////////////
//////////////                default:
//////////////                    Parameters = new SqlParameter[8];
//////////////                    for (int I = 0; I < Parameters.Length; I++)
//////////////                       Parameters[I] = new SqlParameter();
//////////////                    break;
//////////////            }
//////////////
//////////////            //Field Nome
//////////////            Parameters[1].SqlDbType = SqlDbType.VarChar;
//////////////            Parameters[1].ParameterName = "@Param_Nome";
//////////////            if (( FieldInfo.Nome == null ) || ( FieldInfo.Nome == string.Empty ))
//////////////            { Parameters[1].Value = DBNull.Value; }
//////////////            else
//////////////            { Parameters[1].Value = FieldInfo.Nome; }
//////////////            Parameters[1].Size = 150;
//////////////
//////////////            //Field Endereco
//////////////            Parameters[2].SqlDbType = SqlDbType.VarChar;
//////////////            Parameters[2].ParameterName = "@Param_Endereco";
//////////////            if (( FieldInfo.Endereco == null ) || ( FieldInfo.Endereco == string.Empty ))
//////////////            { Parameters[2].Value = DBNull.Value; }
//////////////            else
//////////////            { Parameters[2].Value = FieldInfo.Endereco; }
//////////////            Parameters[2].Size = 200;
//////////////
//////////////            //Field Bairro
//////////////            Parameters[3].SqlDbType = SqlDbType.VarChar;
//////////////            Parameters[3].ParameterName = "@Param_Bairro";
//////////////            if (( FieldInfo.Bairro == null ) || ( FieldInfo.Bairro == string.Empty ))
//////////////            { Parameters[3].Value = DBNull.Value; }
//////////////            else
//////////////            { Parameters[3].Value = FieldInfo.Bairro; }
//////////////            Parameters[3].Size = 100;
//////////////
//////////////            //Field Cidade
//////////////            Parameters[4].SqlDbType = SqlDbType.VarChar;
//////////////            Parameters[4].ParameterName = "@Param_Cidade";
//////////////            if (( FieldInfo.Cidade == null ) || ( FieldInfo.Cidade == string.Empty ))
//////////////            { Parameters[4].Value = DBNull.Value; }
//////////////            else
//////////////            { Parameters[4].Value = FieldInfo.Cidade; }
//////////////            Parameters[4].Size = 150;
//////////////
//////////////            //Field Estado
//////////////            Parameters[5].SqlDbType = SqlDbType.VarChar;
//////////////            Parameters[5].ParameterName = "@Param_Estado";
//////////////            if (( FieldInfo.Estado == null ) || ( FieldInfo.Estado == string.Empty ))
//////////////            { Parameters[5].Value = DBNull.Value; }
//////////////            else
//////////////            { Parameters[5].Value = FieldInfo.Estado; }
//////////////            Parameters[5].Size = 2;
//////////////
//////////////            //Field Telefone
//////////////            Parameters[6].SqlDbType = SqlDbType.VarChar;
//////////////            Parameters[6].ParameterName = "@Param_Telefone";
//////////////            if (( FieldInfo.Telefone == null ) || ( FieldInfo.Telefone == string.Empty ))
//////////////            { Parameters[6].Value = DBNull.Value; }
//////////////            else
//////////////            { Parameters[6].Value = FieldInfo.Telefone; }
//////////////            Parameters[6].Size = 11;
//////////////
//////////////            //Field FkPosicaoIndicacao
//////////////            Parameters[7].SqlDbType = SqlDbType.Int;
//////////////            Parameters[7].ParameterName = "@Param_FkPosicaoIndicacao";
//////////////            Parameters[7].Value = FieldInfo.FkPosicaoIndicacao;
//////////////
//////////////            return Parameters;
//////////////        }
//////////////        #endregion
//////////////
//////////////
//////////////
//////////////
//////////////
//////////////        #region IDisposable Members 
//////////////
//////////////        bool disposed = false;
//////////////
//////////////        public void Dispose()
//////////////        {
//////////////            Dispose(true);
//////////////            GC.SuppressFinalize(this);
//////////////        }
//////////////
//////////////        ~IndicacaoControl() 
//////////////        { 
//////////////            Dispose(false); 
//////////////        }
//////////////
//////////////        private void Dispose(bool disposing) 
//////////////        {
//////////////            if (!this.disposed)
//////////////            {
//////////////                if (disposing) 
//////////////                {
//////////////                    if (this.Conn != null)
//////////////                        if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
//////////////                }
//////////////            }
//////////////
//////////////        }
//////////////        #endregion 
//////////////
//////////////
//////////////
//////////////    }
//////////////
//////////////}
