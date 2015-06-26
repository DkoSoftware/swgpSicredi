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
    /// Tabela: Contato  
    /// Autor: DAL Creator .net 
    /// Data de criação: 19/03/2012 23:02:06 
    /// Descrição: Classe responsável pela perssitência de dados. Utiliza a classe "ContatoFields". 
    /// </summary> 
    public class ContatoControl : IDisposable 
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


        public ContatoControl() {}


        #region Inserindo dados na tabela 

        /// <summary> 
        /// Grava/Persiste um novo objeto ContatoFields no banco de dados
        /// </summary>
        /// <param name="FieldInfo">Objeto ContatoFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public int Add( ref ContatoFields FieldInfo )
        {
            try
            {
                this.Conn = new SqlConnection(this.StrConnetionDB);
                this.Conn.Open();
                this.Tran = this.Conn.BeginTransaction();
                this.Cmd = new SqlCommand("Proc_Contato_Add", this.Conn, this.Tran);
                this.Cmd.CommandType = CommandType.StoredProcedure;
                this.Cmd.Parameters.Clear();
                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
                this.Tran.Commit();
                FieldInfo.idContato = (int)this.Cmd.Parameters["@Param_idContato"].Value;
                return FieldInfo.idContato;

            }
            catch (SqlException e)
            {
                this.Tran.Rollback();
                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar inserir o(s) registro(s) solicitados: {0}.",  e.Message);
                return -1;
            }
            catch (Exception e)
            {
                this.Tran.Rollback();
                this._ErrorMessage = e.Message;
                return -1;
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
        /// Grava/Persiste um novo objeto ContatoFields no banco de dados
        /// </summary>
        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
        /// <param name="FieldInfo">Objeto ContatoFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Add( SqlConnection ConnIn, SqlTransaction TranIn, ref ContatoFields FieldInfo )
        {
            try
            {
                this.Cmd = new SqlCommand("Proc_Contato_Add", ConnIn, TranIn);
                this.Cmd.CommandType = CommandType.StoredProcedure;
                this.Cmd.Parameters.Clear();
                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
                FieldInfo.idContato = (int)this.Cmd.Parameters["@Param_idContato"].Value;
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
        /// Grava/Persiste as alterações em um objeto ContatoFields no banco de dados
        /// </summary>
        /// <param name="FieldInfo">Objeto ContatoFields a ser alterado.</param>
        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Update( ContatoFields FieldInfo )
        {
            try
            {
                this.Conn = new SqlConnection(this.StrConnetionDB);
                this.Conn.Open();
                this.Tran = this.Conn.BeginTransaction();
                this.Cmd = new SqlCommand("Proc_Contato_Update", this.Conn, this.Tran);
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
        /// Grava/Persiste as alterações em um objeto ContatoFields no banco de dados
        /// </summary>
        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
        /// <param name="FieldInfo">Objeto ContatoFields a ser alterado.</param>
        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Update( SqlConnection ConnIn, SqlTransaction TranIn, ContatoFields FieldInfo )
        {
            try
            {
                this.Cmd = new SqlCommand("Proc_Contato_Update", ConnIn, TranIn);
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
                this.Cmd = new SqlCommand("Proc_Contato_DeleteAll", this.Conn, this.Tran);
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
                this.Cmd = new SqlCommand("Proc_Contato_DeleteAll", ConnIn, TranIn);
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
        /// <param name="FieldInfo">Objeto ContatoFields a ser excluído.</param>
        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Delete( ContatoFields FieldInfo )
        {
            return Delete(FieldInfo.idContato);
        }

        /// <summary> 
        /// Exclui um registro da tabela no banco de dados
        /// </summary>
        /// <param name="Param_idContato">int</param>
        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Delete(
                                     int Param_idContato)
        {
            try
            {
                this.Conn = new SqlConnection(this.StrConnetionDB);
                this.Conn.Open();
                this.Tran = this.Conn.BeginTransaction();
                this.Cmd = new SqlCommand("Proc_Contato_Delete", this.Conn, this.Tran);
                this.Cmd.CommandType = CommandType.StoredProcedure;
                this.Cmd.Parameters.Clear();
                this.Cmd.Parameters.Add(new SqlParameter("@Param_idContato", SqlDbType.Int)).Value = Param_idContato;
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
        /// <param name="FieldInfo">Objeto ContatoFields a ser excluído.</param>
        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, ContatoFields FieldInfo )
        {
            return Delete(ConnIn, TranIn, FieldInfo.idContato);
        }

        /// <summary> 
        /// Exclui um registro da tabela no banco de dados
        /// </summary>
        /// <param name="Param_idContato">int</param>
        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, 
                                     int Param_idContato)
        {
            try
            {
                this.Cmd = new SqlCommand("Proc_Contato_Delete", ConnIn, TranIn);
                this.Cmd.CommandType = CommandType.StoredProcedure;
                this.Cmd.Parameters.Clear();
                this.Cmd.Parameters.Add(new SqlParameter("@Param_idContato", SqlDbType.Int)).Value = Param_idContato;
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
        /// Retorna um objeto ContatoFields através da chave primária passada como parâmetro
        /// </summary>
        /// <param name="Param_idContato">int</param>
        /// <returns>Objeto ContatoFields</returns> 
        public ContatoFields GetItem(
                                     int Param_idContato)
        {
            ContatoFields infoFields = new ContatoFields();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Contato_Select", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_idContato", SqlDbType.Int)).Value = Param_idContato;
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

        public DataSet GetAllContactsByProspect(int idProspect)
        {
            DataSet dsContatos = new DataSet();
            try
            {
                SqlConnection Conn = new SqlConnection(this.StrConnetionDB);

                StringBuilder query = new StringBuilder();
                query.Append(" SELECT  c.idContato ,c.DataContato, p.Nome ,p.Telefone , p.Email, c.Tipo, c.Descricao ,c.Situacao ,a.NumeroConta ");
                query.Append(" FROM Prospect p JOIN Contato c ON P.idProspect = c.fkProspect LEFT JOIN Associacao a ON a.fkContato = c.idContato");
                query.AppendFormat(" WHERE p.idProspect = {0}", idProspect);
                query.Append(" order by c.DataContato desc");

                Conn.Open();
                SqlCommand Cmd = new SqlCommand(query.ToString(), Conn);
                Cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(Cmd);
                da.Fill(dsContatos, "Contatos");

                return dsContatos;

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

        #region Selecionando todos os dados da tabela 

        /// <summary> 
        /// Seleciona todos os registro da tabela e preenche um ArrayList com o objeto ContatoFields.
        /// </summary>
        /// <returns>List de objetos ContatoFields</returns> 
        public List<ContatoFields> GetAll()
        {
            List<ContatoFields> arrayInfo = new List<ContatoFields>();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Contato_GetAll", this.Conn))
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
                    using (this.Cmd = new SqlCommand("Proc_Contato_CountAll", this.Conn))
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


        #region Selecionando dados da tabela através do campo "Tipo" 

        /// <summary> 
        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Tipo.
        /// </summary>
        /// <param name="Param_Tipo">string</param>
        /// <returns>ContatoFields</returns> 
        public ContatoFields FindByTipo(
                               string Param_Tipo )
        {
            ContatoFields infoFields = new ContatoFields();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Contato_FindByTipo", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Tipo", SqlDbType.VarChar, 30)).Value = Param_Tipo;
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



        #region Selecionando dados da tabela através do campo "Descricao" 

        /// <summary> 
        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Descricao.
        /// </summary>
        /// <param name="Param_Descricao">string</param>
        /// <returns>ContatoFields</returns> 
        public ContatoFields FindByDescricao(
                               string Param_Descricao )
        {
            ContatoFields infoFields = new ContatoFields();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Contato_FindByDescricao", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Descricao", SqlDbType.VarChar, 300)).Value = Param_Descricao;
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
        /// <returns>ContatoFields</returns> 
        public ContatoFields FindBySituacao(
                               string Param_Situacao )
        {
            ContatoFields infoFields = new ContatoFields();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Contato_FindBySituacao", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Situacao", SqlDbType.VarChar, 30)).Value = Param_Situacao;
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



        #region Selecionando dados da tabela através do campo "DataContato" 

        /// <summary> 
        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo DataContato.
        /// </summary>
        /// <param name="Param_DataContato">DateTime</param>
        /// <returns>ContatoFields</returns> 
        public ContatoFields FindByDataContato(
                               DateTime Param_DataContato )
        {
            ContatoFields infoFields = new ContatoFields();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Contato_FindByDataContato", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_DataContato", SqlDbType.SmallDateTime)).Value = Param_DataContato;
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
        /// <returns>ContatoFields</returns> 
        public ContatoFields FindByDataCadastro(
                               DateTime Param_DataCadastro )
        {
            ContatoFields infoFields = new ContatoFields();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Contato_FindByDataCadastro", this.Conn))
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



        #region Selecionando dados da tabela através do campo "fkProspect" 

        /// <summary> 
        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo fkProspect.
        /// </summary>
        /// <param name="Param_fkProspect">int</param>
        /// <returns>ContatoFields</returns> 
        public ContatoFields FindByfkProspect(
                               int Param_fkProspect )
        {
            ContatoFields infoFields = new ContatoFields();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Contato_FindByfkProspect", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_fkProspect", SqlDbType.Int)).Value = Param_fkProspect;
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
        /// Retorna um objeto ContatoFields preenchido com os valores dos campos do SqlDataReader
        /// </summary>
        /// <param name="dr">SqlDataReader - Preenche o objeto ContatoFields </param>
        /// <returns>ContatoFields</returns>
        private ContatoFields GetDataFromReader( SqlDataReader dr )
        {
            ContatoFields infoFields = new ContatoFields();

            if (!dr.IsDBNull(0))
            { infoFields.idContato = dr.GetInt32(0); }
            else
            { infoFields.idContato = 0; }



            if (!dr.IsDBNull(1))
            { infoFields.Tipo = dr.GetString(1); }
            else
            { infoFields.Tipo = string.Empty; }



            if (!dr.IsDBNull(2))
            { infoFields.Descricao = dr.GetString(2); }
            else
            { infoFields.Descricao = string.Empty; }



            if (!dr.IsDBNull(3))
            { infoFields.Situacao = dr.GetString(3); }
            else
            { infoFields.Situacao = string.Empty; }



            if (!dr.IsDBNull(4))
            { infoFields.DataContato = dr.GetDateTime(4); }
            else
            { infoFields.DataContato = DateTime.MinValue; }



            if (!dr.IsDBNull(5))
            { infoFields.DataCadastro = dr.GetDateTime(5); }
            else
            { infoFields.DataCadastro = DateTime.MinValue; }



            if (!dr.IsDBNull(6))
            { infoFields.fkProspect = dr.GetInt32(6); }
            else
            { infoFields.fkProspect = 0; }


            return infoFields;
        }
        #endregion




















        #region Função GetAllParameters

        /// <summary> 
        /// Retorna um array de parâmetros com campos para atualização, seleção e inserção no banco de dados
        /// </summary>
        /// <param name="FieldInfo">Objeto ContatoFields</param>
        /// <param name="Modo">Tipo de oepração a ser executada no banco de dados</param>
        /// <returns>SqlParameter[] - Array de parâmetros</returns> 
        private SqlParameter[] GetAllParameters( ContatoFields FieldInfo, SQLMode Modo )
        {
            SqlParameter[] Parameters;

            switch (Modo)
            {
                case SQLMode.Add:
                    Parameters = new SqlParameter[7];
                    for (int I = 0; I < Parameters.Length; I++)
                       Parameters[I] = new SqlParameter();
                    //Field idContato
                    Parameters[0].SqlDbType = SqlDbType.Int;
                    Parameters[0].Direction = ParameterDirection.Output;
                    Parameters[0].ParameterName = "@Param_idContato";
                    Parameters[0].Value = DBNull.Value;

                    break;

                case SQLMode.Update:
                    Parameters = new SqlParameter[7];
                    for (int I = 0; I < Parameters.Length; I++)
                       Parameters[I] = new SqlParameter();
                    //Field idContato
                    Parameters[0].SqlDbType = SqlDbType.Int;
                    Parameters[0].ParameterName = "@Param_idContato";
                    Parameters[0].Value = FieldInfo.idContato;

                    break;

                case SQLMode.SelectORDelete:
                    Parameters = new SqlParameter[1];
                    for (int I = 0; I < Parameters.Length; I++)
                       Parameters[I] = new SqlParameter();
                    //Field idContato
                    Parameters[0].SqlDbType = SqlDbType.Int;
                    Parameters[0].ParameterName = "@Param_idContato";
                    Parameters[0].Value = FieldInfo.idContato;

                    return Parameters;

                default:
                    Parameters = new SqlParameter[7];
                    for (int I = 0; I < Parameters.Length; I++)
                       Parameters[I] = new SqlParameter();
                    break;
            }

            //Field Tipo
            Parameters[1].SqlDbType = SqlDbType.VarChar;
            Parameters[1].ParameterName = "@Param_Tipo";
            if (( FieldInfo.Tipo == null ) || ( FieldInfo.Tipo == string.Empty ))
            { Parameters[1].Value = DBNull.Value; }
            else
            { Parameters[1].Value = FieldInfo.Tipo; }
            Parameters[1].Size = 30;

            //Field Descricao
            Parameters[2].SqlDbType = SqlDbType.VarChar;
            Parameters[2].ParameterName = "@Param_Descricao";
            if (( FieldInfo.Descricao == null ) || ( FieldInfo.Descricao == string.Empty ))
            { Parameters[2].Value = DBNull.Value; }
            else
            { Parameters[2].Value = FieldInfo.Descricao; }
            Parameters[2].Size = 300;

            //Field Situacao
            Parameters[3].SqlDbType = SqlDbType.VarChar;
            Parameters[3].ParameterName = "@Param_Situacao";
            if (( FieldInfo.Situacao == null ) || ( FieldInfo.Situacao == string.Empty ))
            { Parameters[3].Value = DBNull.Value; }
            else
            { Parameters[3].Value = FieldInfo.Situacao; }
            Parameters[3].Size = 30;

            //Field DataContato
            Parameters[4].SqlDbType = SqlDbType.SmallDateTime;
            Parameters[4].ParameterName = "@Param_DataContato";
            if ( FieldInfo.DataContato == DateTime.MinValue )
            { Parameters[4].Value = DBNull.Value; }
            else
            { Parameters[4].Value = FieldInfo.DataContato; }

            //Field DataCadastro
            Parameters[5].SqlDbType = SqlDbType.SmallDateTime;
            Parameters[5].ParameterName = "@Param_DataCadastro";
            if ( FieldInfo.DataCadastro == DateTime.MinValue )
            { Parameters[5].Value = DBNull.Value; }
            else
            { Parameters[5].Value = FieldInfo.DataCadastro; }

            //Field fkProspect
            Parameters[6].SqlDbType = SqlDbType.Int;
            Parameters[6].ParameterName = "@Param_fkProspect";
            Parameters[6].Value = FieldInfo.fkProspect;

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

        ~ContatoControl() 
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
//    /// Tabela: Contato  
//    /// Autor: DAL Creator .net 
//    /// Data de criação: 19/03/2012 22:46:51 
//    /// Descrição: Classe responsável pela perssitência de dados. Utiliza a classe "ContatoFields". 
//    /// </summary> 
//    public class ContatoControl : IDisposable 
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
//        public ContatoControl() {}
//
//
//        #region Inserindo dados na tabela 
//
//        /// <summary> 
//        /// Grava/Persiste um novo objeto ContatoFields no banco de dados
//        /// </summary>
//        /// <param name="FieldInfo">Objeto ContatoFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
//        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//        public bool Add( ref ContatoFields FieldInfo )
//        {
//            try
//            {
//                this.Conn = new SqlConnection(this.StrConnetionDB);
//                this.Conn.Open();
//                this.Tran = this.Conn.BeginTransaction();
//                this.Cmd = new SqlCommand("Proc_Contato_Add", this.Conn, this.Tran);
//                this.Cmd.CommandType = CommandType.StoredProcedure;
//                this.Cmd.Parameters.Clear();
//                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
//                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
//                this.Tran.Commit();
//                FieldInfo.idContato = (int)this.Cmd.Parameters["@Param_idContato"].Value;
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
//        /// Grava/Persiste um novo objeto ContatoFields no banco de dados
//        /// </summary>
//        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
//        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
//        /// <param name="FieldInfo">Objeto ContatoFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
//        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//        public bool Add( SqlConnection ConnIn, SqlTransaction TranIn, ref ContatoFields FieldInfo )
//        {
//            try
//            {
//                this.Cmd = new SqlCommand("Proc_Contato_Add", ConnIn, TranIn);
//                this.Cmd.CommandType = CommandType.StoredProcedure;
//                this.Cmd.Parameters.Clear();
//                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
//                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
//                FieldInfo.idContato = (int)this.Cmd.Parameters["@Param_idContato"].Value;
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
//        /// Grava/Persiste as alterações em um objeto ContatoFields no banco de dados
//        /// </summary>
//        /// <param name="FieldInfo">Objeto ContatoFields a ser alterado.</param>
//        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//        public bool Update( ContatoFields FieldInfo )
//        {
//            try
//            {
//                this.Conn = new SqlConnection(this.StrConnetionDB);
//                this.Conn.Open();
//                this.Tran = this.Conn.BeginTransaction();
//                this.Cmd = new SqlCommand("Proc_Contato_Update", this.Conn, this.Tran);
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
//        /// Grava/Persiste as alterações em um objeto ContatoFields no banco de dados
//        /// </summary>
//        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
//        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
//        /// <param name="FieldInfo">Objeto ContatoFields a ser alterado.</param>
//        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//        public bool Update( SqlConnection ConnIn, SqlTransaction TranIn, ContatoFields FieldInfo )
//        {
//            try
//            {
//                this.Cmd = new SqlCommand("Proc_Contato_Update", ConnIn, TranIn);
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
//                this.Cmd = new SqlCommand("Proc_Contato_DeleteAll", this.Conn, this.Tran);
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
//                this.Cmd = new SqlCommand("Proc_Contato_DeleteAll", ConnIn, TranIn);
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
//        /// <param name="FieldInfo">Objeto ContatoFields a ser excluído.</param>
//        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//        public bool Delete( ContatoFields FieldInfo )
//        {
//            return Delete(FieldInfo.idContato);
//        }
//
//        /// <summary> 
//        /// Exclui um registro da tabela no banco de dados
//        /// </summary>
//        /// <param name="Param_idContato">int</param>
//        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//        public bool Delete(
//                                     int Param_idContato)
//        {
//            try
//            {
//                this.Conn = new SqlConnection(this.StrConnetionDB);
//                this.Conn.Open();
//                this.Tran = this.Conn.BeginTransaction();
//                this.Cmd = new SqlCommand("Proc_Contato_Delete", this.Conn, this.Tran);
//                this.Cmd.CommandType = CommandType.StoredProcedure;
//                this.Cmd.Parameters.Clear();
//                this.Cmd.Parameters.Add(new SqlParameter("@Param_idContato", SqlDbType.Int)).Value = Param_idContato;
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
//        /// <param name="FieldInfo">Objeto ContatoFields a ser excluído.</param>
//        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, ContatoFields FieldInfo )
//        {
//            return Delete(ConnIn, TranIn, FieldInfo.idContato);
//        }
//
//        /// <summary> 
//        /// Exclui um registro da tabela no banco de dados
//        /// </summary>
//        /// <param name="Param_idContato">int</param>
//        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
//        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
//        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
//        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, 
//                                     int Param_idContato)
//        {
//            try
//            {
//                this.Cmd = new SqlCommand("Proc_Contato_Delete", ConnIn, TranIn);
//                this.Cmd.CommandType = CommandType.StoredProcedure;
//                this.Cmd.Parameters.Clear();
//                this.Cmd.Parameters.Add(new SqlParameter("@Param_idContato", SqlDbType.Int)).Value = Param_idContato;
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
//        /// Retorna um objeto ContatoFields através da chave primária passada como parâmetro
//        /// </summary>
//        /// <param name="Param_idContato">int</param>
//        /// <returns>Objeto ContatoFields</returns> 
//        public ContatoFields GetItem(
//                                     int Param_idContato)
//        {
//            ContatoFields infoFields = new ContatoFields();
//            try
//            {
//                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//                {
//                    using (this.Cmd = new SqlCommand("Proc_Contato_Select", this.Conn))
//                    {
//                        this.Cmd.CommandType = CommandType.StoredProcedure;
//                        this.Cmd.Parameters.Clear();
//                        this.Cmd.Parameters.Add(new SqlParameter("@Param_idContato", SqlDbType.Int)).Value = Param_idContato;
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
//        /// Seleciona todos os registro da tabela e preenche um ArrayList com o objeto ContatoFields.
//        /// </summary>
//        /// <returns>ArrayList de objetos ContatoFields</returns> 
//        public ArrayList GetAll()
//        {
//            ArrayList arrayInfo = new ArrayList();
//            try
//            {
//                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//                {
//                    using (this.Cmd = new SqlCommand("Proc_Contato_GetAll", this.Conn))
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
//                    using (this.Cmd = new SqlCommand("Proc_Contato_CountAll", this.Conn))
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
//        #region Selecionando dados da tabela através do campo "Tipo" 
//
//        /// <summary> 
//        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Tipo.
//        /// </summary>
//        /// <param name="Param_Tipo">string</param>
//        /// <returns>ContatoFields</returns> 
//        public ContatoFields FindByTipo(
//                               string Param_Tipo )
//        {
//            ContatoFields infoFields = new ContatoFields();
//            try
//            {
//                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//                {
//                    using (this.Cmd = new SqlCommand("Proc_Contato_FindByTipo", this.Conn))
//                    {
//                        this.Cmd.CommandType = CommandType.StoredProcedure;
//                        this.Cmd.Parameters.Clear();
//                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Tipo", SqlDbType.VarChar, 30)).Value = Param_Tipo;
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
//        #region Selecionando dados da tabela através do campo "Descricao" 
//
//        /// <summary> 
//        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Descricao.
//        /// </summary>
//        /// <param name="Param_Descricao">string</param>
//        /// <returns>ContatoFields</returns> 
//        public ContatoFields FindByDescricao(
//                               string Param_Descricao )
//        {
//            ContatoFields infoFields = new ContatoFields();
//            try
//            {
//                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//                {
//                    using (this.Cmd = new SqlCommand("Proc_Contato_FindByDescricao", this.Conn))
//                    {
//                        this.Cmd.CommandType = CommandType.StoredProcedure;
//                        this.Cmd.Parameters.Clear();
//                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Descricao", SqlDbType.VarChar, 300)).Value = Param_Descricao;
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
//        /// <returns>ContatoFields</returns> 
//        public ContatoFields FindBySituacao(
//                               string Param_Situacao )
//        {
//            ContatoFields infoFields = new ContatoFields();
//            try
//            {
//                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//                {
//                    using (this.Cmd = new SqlCommand("Proc_Contato_FindBySituacao", this.Conn))
//                    {
//                        this.Cmd.CommandType = CommandType.StoredProcedure;
//                        this.Cmd.Parameters.Clear();
//                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Situacao", SqlDbType.VarChar, 30)).Value = Param_Situacao;
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
//        #region Selecionando dados da tabela através do campo "DataContato" 
//
//        /// <summary> 
//        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo DataContato.
//        /// </summary>
//        /// <param name="Param_DataContato">DateTime</param>
//        /// <returns>ContatoFields</returns> 
//        public ContatoFields FindByDataContato(
//                               DateTime Param_DataContato )
//        {
//            ContatoFields infoFields = new ContatoFields();
//            try
//            {
//                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//                {
//                    using (this.Cmd = new SqlCommand("Proc_Contato_FindByDataContato", this.Conn))
//                    {
//                        this.Cmd.CommandType = CommandType.StoredProcedure;
//                        this.Cmd.Parameters.Clear();
//                        this.Cmd.Parameters.Add(new SqlParameter("@Param_DataContato", SqlDbType.SmallDateTime)).Value = Param_DataContato;
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
//        #region Selecionando dados da tabela através do campo "DataCadastro" 
//
//        /// <summary> 
//        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo DataCadastro.
//        /// </summary>
//        /// <param name="Param_DataCadastro">DateTime</param>
//        /// <returns>ContatoFields</returns> 
//        public ContatoFields FindByDataCadastro(
//                               DateTime Param_DataCadastro )
//        {
//            ContatoFields infoFields = new ContatoFields();
//            try
//            {
//                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//                {
//                    using (this.Cmd = new SqlCommand("Proc_Contato_FindByDataCadastro", this.Conn))
//                    {
//                        this.Cmd.CommandType = CommandType.StoredProcedure;
//                        this.Cmd.Parameters.Clear();
//                        this.Cmd.Parameters.Add(new SqlParameter("@Param_DataCadastro", SqlDbType.SmallDateTime)).Value = Param_DataCadastro;
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
//        #region Selecionando dados da tabela através do campo "fkProspect" 
//
//        /// <summary> 
//        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo fkProspect.
//        /// </summary>
//        /// <param name="Param_fkProspect">int</param>
//        /// <returns>ContatoFields</returns> 
//        public ContatoFields FindByfkProspect(
//                               int Param_fkProspect )
//        {
//            ContatoFields infoFields = new ContatoFields();
//            try
//            {
//                using (this.Conn = new SqlConnection(this.StrConnetionDB))
//                {
//                    using (this.Cmd = new SqlCommand("Proc_Contato_FindByfkProspect", this.Conn))
//                    {
//                        this.Cmd.CommandType = CommandType.StoredProcedure;
//                        this.Cmd.Parameters.Clear();
//                        this.Cmd.Parameters.Add(new SqlParameter("@Param_fkProspect", SqlDbType.Int)).Value = Param_fkProspect;
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
//        /// Retorna um objeto ContatoFields preenchido com os valores dos campos do SqlDataReader
//        /// </summary>
//        /// <param name="dr">SqlDataReader - Preenche o objeto ContatoFields </param>
//        /// <returns>ContatoFields</returns>
//        private ContatoFields GetDataFromReader( SqlDataReader dr )
//        {
//            ContatoFields infoFields = new ContatoFields();
//
//            if (!dr.IsDBNull(0))
//            { infoFields.idContato = dr.GetInt32(0); }
//            else
//            { infoFields.idContato = 0; }
//
//
//
//            if (!dr.IsDBNull(1))
//            { infoFields.Tipo = dr.GetString(1); }
//            else
//            { infoFields.Tipo = string.Empty; }
//
//
//
//            if (!dr.IsDBNull(2))
//            { infoFields.Descricao = dr.GetString(2); }
//            else
//            { infoFields.Descricao = string.Empty; }
//
//
//
//            if (!dr.IsDBNull(3))
//            { infoFields.Situacao = dr.GetString(3); }
//            else
//            { infoFields.Situacao = string.Empty; }
//
//
//
//            if (!dr.IsDBNull(4))
//            { infoFields.DataContato = dr.GetDateTime(4); }
//            else
//            { infoFields.DataContato = DateTime.MinValue; }
//
//
//
//            if (!dr.IsDBNull(5))
//            { infoFields.DataCadastro = dr.GetDateTime(5); }
//            else
//            { infoFields.DataCadastro = DateTime.MinValue; }
//
//
//
//            if (!dr.IsDBNull(6))
//            { infoFields.fkProspect = dr.GetInt32(6); }
//            else
//            { infoFields.fkProspect = 0; }
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
//        #region Função GetAllParameters
//
//        /// <summary> 
//        /// Retorna um array de parâmetros com campos para atualização, seleção e inserção no banco de dados
//        /// </summary>
//        /// <param name="FieldInfo">Objeto ContatoFields</param>
//        /// <param name="Modo">Tipo de oepração a ser executada no banco de dados</param>
//        /// <returns>SqlParameter[] - Array de parâmetros</returns> 
//        private SqlParameter[] GetAllParameters( ContatoFields FieldInfo, SQLMode Modo )
//        {
//            SqlParameter[] Parameters;
//
//            switch (Modo)
//            {
//                case SQLMode.Add:
//                    Parameters = new SqlParameter[7];
//                    for (int I = 0; I < Parameters.Length; I++)
//                       Parameters[I] = new SqlParameter();
//                    //Field idContato
//                    Parameters[0].SqlDbType = SqlDbType.Int;
//                    Parameters[0].Direction = ParameterDirection.Output;
//                    Parameters[0].ParameterName = "@Param_idContato";
//                    Parameters[0].Value = DBNull.Value;
//
//                    break;
//
//                case SQLMode.Update:
//                    Parameters = new SqlParameter[7];
//                    for (int I = 0; I < Parameters.Length; I++)
//                       Parameters[I] = new SqlParameter();
//                    //Field idContato
//                    Parameters[0].SqlDbType = SqlDbType.Int;
//                    Parameters[0].ParameterName = "@Param_idContato";
//                    Parameters[0].Value = FieldInfo.idContato;
//
//                    break;
//
//                case SQLMode.SelectORDelete:
//                    Parameters = new SqlParameter[1];
//                    for (int I = 0; I < Parameters.Length; I++)
//                       Parameters[I] = new SqlParameter();
//                    //Field idContato
//                    Parameters[0].SqlDbType = SqlDbType.Int;
//                    Parameters[0].ParameterName = "@Param_idContato";
//                    Parameters[0].Value = FieldInfo.idContato;
//
//                    return Parameters;
//
//                default:
//                    Parameters = new SqlParameter[7];
//                    for (int I = 0; I < Parameters.Length; I++)
//                       Parameters[I] = new SqlParameter();
//                    break;
//            }
//
//            //Field Tipo
//            Parameters[1].SqlDbType = SqlDbType.VarChar;
//            Parameters[1].ParameterName = "@Param_Tipo";
//            if (( FieldInfo.Tipo == null ) || ( FieldInfo.Tipo == string.Empty ))
//            { Parameters[1].Value = DBNull.Value; }
//            else
//            { Parameters[1].Value = FieldInfo.Tipo; }
//            Parameters[1].Size = 30;
//
//            //Field Descricao
//            Parameters[2].SqlDbType = SqlDbType.VarChar;
//            Parameters[2].ParameterName = "@Param_Descricao";
//            if (( FieldInfo.Descricao == null ) || ( FieldInfo.Descricao == string.Empty ))
//            { Parameters[2].Value = DBNull.Value; }
//            else
//            { Parameters[2].Value = FieldInfo.Descricao; }
//            Parameters[2].Size = 300;
//
//            //Field Situacao
//            Parameters[3].SqlDbType = SqlDbType.VarChar;
//            Parameters[3].ParameterName = "@Param_Situacao";
//            if (( FieldInfo.Situacao == null ) || ( FieldInfo.Situacao == string.Empty ))
//            { Parameters[3].Value = DBNull.Value; }
//            else
//            { Parameters[3].Value = FieldInfo.Situacao; }
//            Parameters[3].Size = 30;
//
//            //Field DataContato
//            Parameters[4].SqlDbType = SqlDbType.SmallDateTime;
//            Parameters[4].ParameterName = "@Param_DataContato";
//            if ( FieldInfo.DataContato == DateTime.MinValue )
//            { Parameters[4].Value = DBNull.Value; }
//            else
//            { Parameters[4].Value = FieldInfo.DataContato; }
//
//            //Field DataCadastro
//            Parameters[5].SqlDbType = SqlDbType.SmallDateTime;
//            Parameters[5].ParameterName = "@Param_DataCadastro";
//            if ( FieldInfo.DataCadastro == DateTime.MinValue )
//            { Parameters[5].Value = DBNull.Value; }
//            else
//            { Parameters[5].Value = FieldInfo.DataCadastro; }
//
//            //Field fkProspect
//            Parameters[6].SqlDbType = SqlDbType.Int;
//            Parameters[6].ParameterName = "@Param_fkProspect";
//            Parameters[6].Value = FieldInfo.fkProspect;
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
//        ~ContatoControl() 
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
////    /// Tabela: Contato  
////    /// Autor: DAL Creator .net 
////    /// Data de criação: 13/03/2012 21:19:06 
////    /// Descrição: Classe responsável pela perssitência de dados. Utiliza a classe "ContatoFields". 
////    /// </summary> 
////    public class ContatoControl : IDisposable 
////    {
////
////        #region String de conexão 
////        private string StrConnetionDB = "Data Source=DEKO-PC;Initial Catalog=swgp;User Id=sureg;Password=@sureg2012;";
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
////        public ContatoControl() {}
////
////
////        #region Inserindo dados na tabela 
////
////        /// <summary> 
////        /// Grava/Persiste um novo objeto ContatoFields no banco de dados
////        /// </summary>
////        /// <param name="FieldInfo">Objeto ContatoFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
////        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////        public bool Add( ref ContatoFields FieldInfo )
////        {
////            try
////            {
////                this.Conn = new SqlConnection(this.StrConnetionDB);
////                this.Conn.Open();
////                this.Tran = this.Conn.BeginTransaction();
////                this.Cmd = new SqlCommand("Proc_Contato_Add", this.Conn, this.Tran);
////                this.Cmd.CommandType = CommandType.StoredProcedure;
////                this.Cmd.Parameters.Clear();
////                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
////                this.Tran.Commit();
////                FieldInfo.idContato = (int)this.Cmd.Parameters["@Param_idContato"].Value;
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
////        /// Grava/Persiste um novo objeto ContatoFields no banco de dados
////        /// </summary>
////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
////        /// <param name="FieldInfo">Objeto ContatoFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
////        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////        public bool Add( SqlConnection ConnIn, SqlTransaction TranIn, ref ContatoFields FieldInfo )
////        {
////            try
////            {
////                this.Cmd = new SqlCommand("Proc_Contato_Add", ConnIn, TranIn);
////                this.Cmd.CommandType = CommandType.StoredProcedure;
////                this.Cmd.Parameters.Clear();
////                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
////                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
////                FieldInfo.idContato = (int)this.Cmd.Parameters["@Param_idContato"].Value;
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
////        /// Grava/Persiste as alterações em um objeto ContatoFields no banco de dados
////        /// </summary>
////        /// <param name="FieldInfo">Objeto ContatoFields a ser alterado.</param>
////        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////        public bool Update( ContatoFields FieldInfo )
////        {
////            try
////            {
////                this.Conn = new SqlConnection(this.StrConnetionDB);
////                this.Conn.Open();
////                this.Tran = this.Conn.BeginTransaction();
////                this.Cmd = new SqlCommand("Proc_Contato_Update", this.Conn, this.Tran);
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
////        /// Grava/Persiste as alterações em um objeto ContatoFields no banco de dados
////        /// </summary>
////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
////        /// <param name="FieldInfo">Objeto ContatoFields a ser alterado.</param>
////        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////        public bool Update( SqlConnection ConnIn, SqlTransaction TranIn, ContatoFields FieldInfo )
////        {
////            try
////            {
////                this.Cmd = new SqlCommand("Proc_Contato_Update", ConnIn, TranIn);
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
////                this.Cmd = new SqlCommand("Proc_Contato_DeleteAll", this.Conn, this.Tran);
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
////                this.Cmd = new SqlCommand("Proc_Contato_DeleteAll", ConnIn, TranIn);
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
////        /// <param name="FieldInfo">Objeto ContatoFields a ser excluído.</param>
////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////        public bool Delete( ContatoFields FieldInfo )
////        {
////            return Delete(FieldInfo.idContato);
////        }
////
////        /// <summary> 
////        /// Exclui um registro da tabela no banco de dados
////        /// </summary>
////        /// <param name="Param_idContato">int</param>
////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////        public bool Delete(
////                                     int Param_idContato)
////        {
////            try
////            {
////                this.Conn = new SqlConnection(this.StrConnetionDB);
////                this.Conn.Open();
////                this.Tran = this.Conn.BeginTransaction();
////                this.Cmd = new SqlCommand("Proc_Contato_Delete", this.Conn, this.Tran);
////                this.Cmd.CommandType = CommandType.StoredProcedure;
////                this.Cmd.Parameters.Clear();
////                this.Cmd.Parameters.Add(new SqlParameter("@Param_idContato", SqlDbType.Int)).Value = Param_idContato;
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
////        /// <param name="FieldInfo">Objeto ContatoFields a ser excluído.</param>
////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, ContatoFields FieldInfo )
////        {
////            return Delete(ConnIn, TranIn, FieldInfo.idContato);
////        }
////
////        /// <summary> 
////        /// Exclui um registro da tabela no banco de dados
////        /// </summary>
////        /// <param name="Param_idContato">int</param>
////        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
////        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
////        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
////        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, 
////                                     int Param_idContato)
////        {
////            try
////            {
////                this.Cmd = new SqlCommand("Proc_Contato_Delete", ConnIn, TranIn);
////                this.Cmd.CommandType = CommandType.StoredProcedure;
////                this.Cmd.Parameters.Clear();
////                this.Cmd.Parameters.Add(new SqlParameter("@Param_idContato", SqlDbType.Int)).Value = Param_idContato;
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
////        /// Retorna um objeto ContatoFields através da chave primária passada como parâmetro
////        /// </summary>
////        /// <param name="Param_idContato">int</param>
////        /// <returns>Objeto ContatoFields</returns> 
////        public ContatoFields GetItem(
////                                     int Param_idContato)
////        {
////            ContatoFields infoFields = new ContatoFields();
////            try
////            {
////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////                {
////                    using (this.Cmd = new SqlCommand("Proc_Contato_Select", this.Conn))
////                    {
////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////                        this.Cmd.Parameters.Clear();
////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_idContato", SqlDbType.Int)).Value = Param_idContato;
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
////        /// Seleciona todos os registro da tabela e preenche um ArrayList com o objeto ContatoFields.
////        /// </summary>
////        /// <returns>List de objetos ContatoFields</returns> 
////        public List<ContatoFields> GetAll()
////        {
////            List<ContatoFields> arrayInfo = new List<ContatoFields>();
////            try
////            {
////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////                {
////                    using (this.Cmd = new SqlCommand("Proc_Contato_GetAll", this.Conn))
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
////                    using (this.Cmd = new SqlCommand("Proc_Contato_CountAll", this.Conn))
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
////        #region Selecionando dados da tabela através do campo "Tipo" 
////
////        /// <summary> 
////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Tipo.
////        /// </summary>
////        /// <param name="Param_Tipo">string</param>
////        /// <returns>ArrayList</returns> 
////        public ArrayList FindByTipo(
////                               string Param_Tipo )
////        {
////            ArrayList arrayInfo = new ArrayList();
////            try
////            {
////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////                {
////                    using (this.Cmd = new SqlCommand("Proc_Contato_FindByTipo", this.Conn))
////                    {
////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////                        this.Cmd.Parameters.Clear();
////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Tipo", SqlDbType.VarChar, 30)).Value = Param_Tipo;
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
////        #region Selecionando dados da tabela através do campo "Descricao" 
////
////        /// <summary> 
////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Descricao.
////        /// </summary>
////        /// <param name="Param_Descricao">string</param>
////        /// <returns>ArrayList</returns> 
////        public ArrayList FindByDescricao(
////                               string Param_Descricao )
////        {
////            ArrayList arrayInfo = new ArrayList();
////            try
////            {
////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////                {
////                    using (this.Cmd = new SqlCommand("Proc_Contato_FindByDescricao", this.Conn))
////                    {
////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////                        this.Cmd.Parameters.Clear();
////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Descricao", SqlDbType.VarChar, 300)).Value = Param_Descricao;
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
////        /// <returns>ArrayList</returns> 
////        public ArrayList FindBySituacao(
////                               string Param_Situacao )
////        {
////            ArrayList arrayInfo = new ArrayList();
////            try
////            {
////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////                {
////                    using (this.Cmd = new SqlCommand("Proc_Contato_FindBySituacao", this.Conn))
////                    {
////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////                        this.Cmd.Parameters.Clear();
////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Situacao", SqlDbType.VarChar, 30)).Value = Param_Situacao;
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
////        #region Selecionando dados da tabela através do campo "DataContato" 
////
////        /// <summary> 
////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo DataContato.
////        /// </summary>
////        /// <param name="Param_DataContato">DateTime</param>
////        /// <returns>ArrayList</returns> 
////        public ArrayList FindByDataContato(
////                               DateTime Param_DataContato )
////        {
////            ArrayList arrayInfo = new ArrayList();
////            try
////            {
////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////                {
////                    using (this.Cmd = new SqlCommand("Proc_Contato_FindByDataContato", this.Conn))
////                    {
////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////                        this.Cmd.Parameters.Clear();
////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_DataContato", SqlDbType.SmallDateTime)).Value = Param_DataContato;
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
////        #region Selecionando dados da tabela através do campo "DataCadastro" 
////
////        /// <summary> 
////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo DataCadastro.
////        /// </summary>
////        /// <param name="Param_DataCadastro">DateTime</param>
////        /// <returns>ArrayList</returns> 
////        public ArrayList FindByDataCadastro(
////                               DateTime Param_DataCadastro )
////        {
////            ArrayList arrayInfo = new ArrayList();
////            try
////            {
////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////                {
////                    using (this.Cmd = new SqlCommand("Proc_Contato_FindByDataCadastro", this.Conn))
////                    {
////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////                        this.Cmd.Parameters.Clear();
////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_DataCadastro", SqlDbType.SmallDateTime)).Value = Param_DataCadastro;
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
////        #region Selecionando dados da tabela através do campo "fkProspect" 
////
////        /// <summary> 
////        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo fkProspect.
////        /// </summary>
////        /// <param name="Param_fkProspect">int</param>
////        /// <returns>ArrayList</returns> 
////        public ArrayList FindByfkProspect(
////                               int Param_fkProspect )
////        {
////            ArrayList arrayInfo = new ArrayList();
////            try
////            {
////                using (this.Conn = new SqlConnection(this.StrConnetionDB))
////                {
////                    using (this.Cmd = new SqlCommand("Proc_Contato_FindByfkProspect", this.Conn))
////                    {
////                        this.Cmd.CommandType = CommandType.StoredProcedure;
////                        this.Cmd.Parameters.Clear();
////                        this.Cmd.Parameters.Add(new SqlParameter("@Param_fkProspect", SqlDbType.Int)).Value = Param_fkProspect;
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
////        /// Retorna um objeto ContatoFields preenchido com os valores dos campos do SqlDataReader
////        /// </summary>
////        /// <param name="dr">SqlDataReader - Preenche o objeto ContatoFields </param>
////        /// <returns>ContatoFields</returns>
////        private ContatoFields GetDataFromReader( SqlDataReader dr )
////        {
////            ContatoFields infoFields = new ContatoFields();
////
////            if (!dr.IsDBNull(0))
////            { infoFields.idContato = dr.GetInt32(0); }
////            else
////            { infoFields.idContato = 0; }
////
////
////
////            if (!dr.IsDBNull(1))
////            { infoFields.Tipo = dr.GetString(1); }
////            else
////            { infoFields.Tipo = string.Empty; }
////
////
////
////            if (!dr.IsDBNull(2))
////            { infoFields.Descricao = dr.GetString(2); }
////            else
////            { infoFields.Descricao = string.Empty; }
////
////
////
////            if (!dr.IsDBNull(3))
////            { infoFields.Situacao = dr.GetString(3); }
////            else
////            { infoFields.Situacao = string.Empty; }
////
////
////
////            if (!dr.IsDBNull(4))
////            { infoFields.DataContato = dr.GetDateTime(4); }
////            else
////            { infoFields.DataContato = DateTime.MinValue; }
////
////
////
////            if (!dr.IsDBNull(5))
////            { infoFields.DataCadastro = dr.GetDateTime(5); }
////            else
////            { infoFields.DataCadastro = DateTime.MinValue; }
////
////
////
////            if (!dr.IsDBNull(6))
////            { infoFields.fkProspect = dr.GetInt32(6); }
////            else
////            { infoFields.fkProspect = 0; }
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
////        #region Função GetAllParameters
////
////        /// <summary> 
////        /// Retorna um array de parâmetros com campos para atualização, seleção e inserção no banco de dados
////        /// </summary>
////        /// <param name="FieldInfo">Objeto ContatoFields</param>
////        /// <param name="Modo">Tipo de oepração a ser executada no banco de dados</param>
////        /// <returns>SqlParameter[] - Array de parâmetros</returns> 
////        private SqlParameter[] GetAllParameters( ContatoFields FieldInfo, SQLMode Modo )
////        {
////            SqlParameter[] Parameters;
////
////            switch (Modo)
////            {
////                case SQLMode.Add:
////                    Parameters = new SqlParameter[7];
////                    for (int I = 0; I < Parameters.Length; I++)
////                       Parameters[I] = new SqlParameter();
////                    //Field idContato
////                    Parameters[0].SqlDbType = SqlDbType.Int;
////                    Parameters[0].Direction = ParameterDirection.Output;
////                    Parameters[0].ParameterName = "@Param_idContato";
////                    Parameters[0].Value = DBNull.Value;
////
////                    break;
////
////                case SQLMode.Update:
////                    Parameters = new SqlParameter[7];
////                    for (int I = 0; I < Parameters.Length; I++)
////                       Parameters[I] = new SqlParameter();
////                    //Field idContato
////                    Parameters[0].SqlDbType = SqlDbType.Int;
////                    Parameters[0].ParameterName = "@Param_idContato";
////                    Parameters[0].Value = FieldInfo.idContato;
////
////                    break;
////
////                case SQLMode.SelectORDelete:
////                    Parameters = new SqlParameter[1];
////                    for (int I = 0; I < Parameters.Length; I++)
////                       Parameters[I] = new SqlParameter();
////                    //Field idContato
////                    Parameters[0].SqlDbType = SqlDbType.Int;
////                    Parameters[0].ParameterName = "@Param_idContato";
////                    Parameters[0].Value = FieldInfo.idContato;
////
////                    return Parameters;
////
////                default:
////                    Parameters = new SqlParameter[7];
////                    for (int I = 0; I < Parameters.Length; I++)
////                       Parameters[I] = new SqlParameter();
////                    break;
////            }
////
////            //Field Tipo
////            Parameters[1].SqlDbType = SqlDbType.VarChar;
////            Parameters[1].ParameterName = "@Param_Tipo";
////            if (( FieldInfo.Tipo == null ) || ( FieldInfo.Tipo == string.Empty ))
////            { Parameters[1].Value = DBNull.Value; }
////            else
////            { Parameters[1].Value = FieldInfo.Tipo; }
////            Parameters[1].Size = 30;
////
////            //Field Descricao
////            Parameters[2].SqlDbType = SqlDbType.VarChar;
////            Parameters[2].ParameterName = "@Param_Descricao";
////            if (( FieldInfo.Descricao == null ) || ( FieldInfo.Descricao == string.Empty ))
////            { Parameters[2].Value = DBNull.Value; }
////            else
////            { Parameters[2].Value = FieldInfo.Descricao; }
////            Parameters[2].Size = 300;
////
////            //Field Situacao
////            Parameters[3].SqlDbType = SqlDbType.VarChar;
////            Parameters[3].ParameterName = "@Param_Situacao";
////            if (( FieldInfo.Situacao == null ) || ( FieldInfo.Situacao == string.Empty ))
////            { Parameters[3].Value = DBNull.Value; }
////            else
////            { Parameters[3].Value = FieldInfo.Situacao; }
////            Parameters[3].Size = 30;
////
////            //Field DataContato
////            Parameters[4].SqlDbType = SqlDbType.SmallDateTime;
////            Parameters[4].ParameterName = "@Param_DataContato";
////            if ( FieldInfo.DataContato == DateTime.MinValue )
////            { Parameters[4].Value = DBNull.Value; }
////            else
////            { Parameters[4].Value = FieldInfo.DataContato; }
////
////            //Field DataCadastro
////            Parameters[5].SqlDbType = SqlDbType.SmallDateTime;
////            Parameters[5].ParameterName = "@Param_DataCadastro";
////            if ( FieldInfo.DataCadastro == DateTime.MinValue )
////            { Parameters[5].Value = DBNull.Value; }
////            else
////            { Parameters[5].Value = FieldInfo.DataCadastro; }
////
////            //Field fkProspect
////            Parameters[6].SqlDbType = SqlDbType.Int;
////            Parameters[6].ParameterName = "@Param_fkProspect";
////            Parameters[6].Value = FieldInfo.fkProspect;
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
////        ~ContatoControl() 
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
