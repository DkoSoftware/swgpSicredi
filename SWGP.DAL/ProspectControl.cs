using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Linq;
using SWGPgen;

namespace SIMLgen
{


    /// <summary> 
    /// Tabela: Prospect  
    /// Autor: DAL Creator .net 
    /// Data de criação: 07/05/2012 21:02:06 
    /// Descrição: Classe responsável pela perssitência de dados. Utiliza a classe "ProspectFields". 
    /// </summary> 
    public class ProspectControl : IDisposable 
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


        public ProspectControl() {}


        #region Inserindo dados na tabela 

        /// <summary> 
        /// Grava/Persiste um novo objeto ProspectFields no banco de dados
        /// </summary>
        /// <param name="FieldInfo">Objeto ProspectFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Add( ref ProspectFields FieldInfo )
        {
            try
            {
                this.Conn = new SqlConnection(this.StrConnetionDB);
                this.Conn.Open();
                this.Tran = this.Conn.BeginTransaction();
                this.Cmd = new SqlCommand("Proc_Prospect_Add", this.Conn, this.Tran);
                this.Cmd.CommandType = CommandType.StoredProcedure;
                this.Cmd.Parameters.Clear();
                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
                this.Tran.Commit();
                FieldInfo.idProspect = (int)this.Cmd.Parameters["@Param_idProspect"].Value;
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
        /// Grava/Persiste um novo objeto ProspectFields no banco de dados
        /// </summary>
        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
        /// <param name="FieldInfo">Objeto ProspectFields a ser gravado.Caso o parâmetro solicite a expressão "ref", será adicionado um novo valor a algum campo auto incremento.</param>
        /// <returns>"true" = registro gravado com sucesso, "false" = erro ao gravar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Add( SqlConnection ConnIn, SqlTransaction TranIn, ref ProspectFields FieldInfo )
        {
            try
            {
                this.Cmd = new SqlCommand("Proc_Prospect_Add", ConnIn, TranIn);
                this.Cmd.CommandType = CommandType.StoredProcedure;
                this.Cmd.Parameters.Clear();
                this.Cmd.Parameters.AddRange(GetAllParameters(FieldInfo, SQLMode.Add));
                if (!(this.Cmd.ExecuteNonQuery() > 0)) throw new Exception("Erro ao tentar inserir registro!!");
                FieldInfo.idProspect = (int)this.Cmd.Parameters["@Param_idProspect"].Value;
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
        /// Grava/Persiste as alterações em um objeto ProspectFields no banco de dados
        /// </summary>
        /// <param name="FieldInfo">Objeto ProspectFields a ser alterado.</param>
        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Update( ProspectFields FieldInfo )
        {
            try
            {
                this.Conn = new SqlConnection(this.StrConnetionDB);
                this.Conn.Open();
                this.Tran = this.Conn.BeginTransaction();
                this.Cmd = new SqlCommand("Proc_Prospect_Update", this.Conn, this.Tran);
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
        /// Grava/Persiste as alterações em um objeto ProspectFields no banco de dados
        /// </summary>
        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
        /// <param name="FieldInfo">Objeto ProspectFields a ser alterado.</param>
        /// <returns>"true" = registro alterado com sucesso, "false" = erro ao tentar alterar registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Update( SqlConnection ConnIn, SqlTransaction TranIn, ProspectFields FieldInfo )
        {
            try
            {
                this.Cmd = new SqlCommand("Proc_Prospect_Update", ConnIn, TranIn);
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
                this.Cmd = new SqlCommand("Proc_Prospect_DeleteAll", this.Conn, this.Tran);
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
                this.Cmd = new SqlCommand("Proc_Prospect_DeleteAll", ConnIn, TranIn);
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
        /// <param name="FieldInfo">Objeto ProspectFields a ser excluído.</param>
        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Delete( ProspectFields FieldInfo )
        {
            return Delete(FieldInfo.idProspect);
        }

        /// <summary> 
        /// Exclui um registro da tabela no banco de dados
        /// </summary>
        /// <param name="Param_idProspect">int</param>
        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Delete(
                                     int Param_idProspect)
        {
            try
            {
                this.Conn = new SqlConnection(this.StrConnetionDB);
                this.Conn.Open();
                this.Tran = this.Conn.BeginTransaction();
                this.Cmd = new SqlCommand("Proc_Prospect_Delete", this.Conn, this.Tran);
                this.Cmd.CommandType = CommandType.StoredProcedure;
                this.Cmd.Parameters.Clear();
                this.Cmd.Parameters.Add(new SqlParameter("@Param_idProspect", SqlDbType.Int)).Value = Param_idProspect;
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
        /// <param name="FieldInfo">Objeto ProspectFields a ser excluído.</param>
        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, ProspectFields FieldInfo )
        {
            return Delete(ConnIn, TranIn, FieldInfo.idProspect);
        }

        /// <summary> 
        /// Exclui um registro da tabela no banco de dados
        /// </summary>
        /// <param name="Param_idProspect">int</param>
        /// <param name="ConnIn">Objeto SqlConnection responsável pela conexão com o banco de dados.</param>
        /// <param name="TranIn">Objeto SqlTransaction responsável pela transação iniciada no banco de dados.</param>
        /// <returns>"true" = registro excluido com sucesso, "false" = erro ao tentar excluir registro (consulte a propriedade ErrorMessage para detalhes)</returns> 
        public bool Delete( SqlConnection ConnIn, SqlTransaction TranIn, 
                                     int Param_idProspect)
        {
            try
            {
                this.Cmd = new SqlCommand("Proc_Prospect_Delete", ConnIn, TranIn);
                this.Cmd.CommandType = CommandType.StoredProcedure;
                this.Cmd.Parameters.Clear();
                this.Cmd.Parameters.Add(new SqlParameter("@Param_idProspect", SqlDbType.Int)).Value = Param_idProspect;
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
        /// Retorna um objeto ProspectFields através da chave primária passada como parâmetro
        /// </summary>
        /// <param name="Param_idProspect">int</param>
        /// <returns>Objeto ProspectFields</returns> 
        public ProspectFields GetItem(
                                     int Param_idProspect)
        {
            ProspectFields infoFields = new ProspectFields();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Prospect_Select", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_idProspect", SqlDbType.Int)).Value = Param_idProspect;
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
        /// Seleciona todos os registro da tabela e preenche um ArrayList com o objeto ProspectFields.
        /// </summary>
        /// <returns>List de objetos ProspectFields</returns> 
        public List<ProspectFields> GetAll()
        {
            List<ProspectFields> arrayInfo = new List<ProspectFields>();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Prospect_GetAll", this.Conn))
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
                    using (this.Cmd = new SqlCommand("Proc_Prospect_CountAll", this.Conn))
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
        /// <returns>List ProspectFields</returns> 
        public List<ProspectFields> FindByNome(
                               string Param_Nome )
        {
            List<ProspectFields> arrayList = new List<ProspectFields>();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Prospect_FindByNome", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Nome", SqlDbType.VarChar, 150)).Value = Param_Nome;
                        this.Cmd.Connection.Open();
                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                        {
                            if (!dr.HasRows) return null;
                            while (dr.Read())
                            {
                               arrayList.Add(GetDataFromReader( dr ));
                            }
                        }
                    }
                }

                return arrayList;

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
        /// <returns>List ProspectFields</returns> 
        public List<ProspectFields> FindByEndereco(
                               string Param_Endereco )
        {
            List<ProspectFields> arrayList = new List<ProspectFields>();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Prospect_FindByEndereco", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Endereco", SqlDbType.VarChar, 250)).Value = Param_Endereco;
                        this.Cmd.Connection.Open();
                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                        {
                            if (!dr.HasRows) return null;
                            while (dr.Read())
                            {
                               arrayList.Add(GetDataFromReader( dr ));
                            }
                        }
                    }
                }

                return arrayList;

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
        /// <returns>List ProspectFields</returns> 
        public List<ProspectFields> FindByTelefone(
                               string Param_Telefone )
        {
            List<ProspectFields> arrayList = new List<ProspectFields>();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Prospect_FindByTelefone", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Telefone", SqlDbType.VarChar, 11)).Value = Param_Telefone;
                        this.Cmd.Connection.Open();
                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                        {
                            if (!dr.HasRows) return null;
                            while (dr.Read())
                            {
                               arrayList.Add(GetDataFromReader( dr ));
                            }
                        }
                    }
                }

                return arrayList;

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



        #region Selecionando dados da tabela através do campo "Tipo" 

        /// <summary> 
        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Tipo.
        /// </summary>
        /// <param name="Param_Tipo">string</param>
        /// <returns>List ProspectFields</returns> 
        public List<ProspectFields> FindByTipo(
                               string Param_Tipo )
        {
            List<ProspectFields> arrayList = new List<ProspectFields>();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Prospect_FindByTipo", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Tipo", SqlDbType.VarChar, 2)).Value = Param_Tipo;
                        this.Cmd.Connection.Open();
                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                        {
                            if (!dr.HasRows) return null;
                            while (dr.Read())
                            {
                               arrayList.Add(GetDataFromReader( dr ));
                            }
                        }
                    }
                }

                return arrayList;

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



        #region Selecionando dados da tabela através do campo "Segmento" 

        /// <summary> 
        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Segmento.
        /// </summary>
        /// <param name="Param_Segmento">string</param>
        /// <returns>List ProspectFields</returns> 
        public List<ProspectFields> FindBySegmento(
                               string Param_Segmento )
        {
            List<ProspectFields> arrayList = new List<ProspectFields>();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Prospect_FindBySegmento", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Segmento", SqlDbType.VarChar, 30)).Value = Param_Segmento;
                        this.Cmd.Connection.Open();
                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                        {
                            if (!dr.HasRows) return null;
                            while (dr.Read())
                            {
                               arrayList.Add(GetDataFromReader( dr ));
                            }
                        }
                    }
                }

                return arrayList;

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



        #region Selecionando dados da tabela através do campo "Observacao" 

        /// <summary> 
        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Observacao.
        /// </summary>
        /// <param name="Param_Observacao">string</param>
        /// <returns>List ProspectFields</returns> 
        public List<ProspectFields> FindByObservacao(
                               string Param_Observacao )
        {
            List<ProspectFields> arrayList = new List<ProspectFields>();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Prospect_FindByObservacao", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Observacao", SqlDbType.VarChar, 300)).Value = Param_Observacao;
                        this.Cmd.Connection.Open();
                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                        {
                            if (!dr.HasRows) return null;
                            while (dr.Read())
                            {
                               arrayList.Add(GetDataFromReader( dr ));
                            }
                        }
                    }
                }

                return arrayList;

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



        #region Selecionando dados da tabela através do campo "Email" 

        /// <summary> 
        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo Email.
        /// </summary>
        /// <param name="Param_Email">string</param>
        /// <returns>List ProspectFields</returns> 
        public List<ProspectFields> FindByEmail(
                               string Param_Email )
        {
            List<ProspectFields> arrayList = new List<ProspectFields>();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Prospect_FindByEmail", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Email", SqlDbType.VarChar, 50)).Value = Param_Email;
                        this.Cmd.Connection.Open();
                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                        {
                            if (!dr.HasRows) return null;
                            while (dr.Read())
                            {
                               arrayList.Add(GetDataFromReader( dr ));
                            }
                        }
                    }
                }

                return arrayList;

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
        /// <returns>List ProspectFields</returns> 
        public List<ProspectFields> FindByBairro(
                               string Param_Bairro )
        {
            List<ProspectFields> arrayList = new List<ProspectFields>();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Prospect_FindByBairro", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Bairro", SqlDbType.VarChar, 100)).Value = Param_Bairro;
                        this.Cmd.Connection.Open();
                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                        {
                            if (!dr.HasRows) return null;
                            while (dr.Read())
                            {
                               arrayList.Add(GetDataFromReader( dr ));
                            }
                        }
                    }
                }

                return arrayList;

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
        /// <returns>List ProspectFields</returns> 
        public List<ProspectFields> FindByCidade(
                               string Param_Cidade )
        {
            List<ProspectFields> arrayList = new List<ProspectFields>();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Prospect_FindByCidade", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_Cidade", SqlDbType.VarChar, 100)).Value = Param_Cidade;
                        this.Cmd.Connection.Open();
                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                        {
                            if (!dr.HasRows) return null;
                            while (dr.Read())
                            {
                               arrayList.Add(GetDataFromReader( dr ));
                            }
                        }
                    }
                }

                return arrayList;

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



        #region Selecionando dados da tabela através do campo "CPF" 

        /// <summary> 
        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo CPF.
        /// </summary>
        /// <param name="Param_CPF">string</param>
        /// <returns>ProspectFields</returns> 
        public ProspectFields FindByCPF(
                               string Param_CPF )
        {
            ProspectFields infoFields = new ProspectFields();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Prospect_FindByCPF", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_CPF", SqlDbType.VarChar, 50)).Value = Param_CPF;
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



        #region Selecionando dados da tabela através do campo "CNPJ" 

        /// <summary> 
        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo CNPJ.
        /// </summary>
        /// <param name="Param_CNPJ">string</param>
        /// <returns>ProspectFields</returns> 
        public ProspectFields FindByCNPJ(
                               string Param_CNPJ )
        {
            ProspectFields infoFields = new ProspectFields();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Prospect_FindByCNPJ", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_CNPJ", SqlDbType.VarChar, 50)).Value = Param_CNPJ;
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



        #region Selecionando dados da tabela através do campo "FkUsuario" 

        /// <summary> 
        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo FkUsuario.
        /// </summary>
        /// <param name="Param_FkUsuario">int</param>
        /// <returns>List ProspectFields</returns> 
        public List<ProspectFields> FindByFkUsuario(
                               int Param_FkUsuario )
        {
            List<ProspectFields> arrayList = new List<ProspectFields>();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Prospect_FindByFkUsuario", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_FkUsuario", SqlDbType.Int)).Value = Param_FkUsuario;
                        this.Cmd.Connection.Open();
                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                        {
                            if (!dr.HasRows) return null;
                            while (dr.Read())
                            {
                               arrayList.Add(GetDataFromReader( dr ));
                            }
                        }
                    }
                }

                return arrayList;

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



        #region Selecionando dados da tabela através do campo "SituacaoProspect" 

        /// <summary> 
        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo SituacaoProspect.
        /// </summary>
        /// <param name="Param_SituacaoProspect">string</param>
        /// <returns>List ProspectFields</returns> 
        public List<ProspectFields> FindBySituacaoProspect(
                               string Param_SituacaoProspect )
        {
            List<ProspectFields> arrayList = new List<ProspectFields>();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Prospect_FindBySituacaoProspect", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_SituacaoProspect", SqlDbType.VarChar, 20)).Value = Param_SituacaoProspect;
                        this.Cmd.Connection.Open();
                        using (SqlDataReader dr = this.Cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                        {
                            if (!dr.HasRows) return null;
                            while (dr.Read())
                            {
                               arrayList.Add(GetDataFromReader( dr ));
                            }
                        }
                    }
                }

                return arrayList;

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



        #region Selecionando dados da tabela através do campo "fkIndicacao" 

        /// <summary> 
        /// Retorna um ou mais registros da tabela no banco de dados, filtrados pelo campo fkIndicacao.
        /// </summary>
        /// <param name="Param_fkIndicacao">int</param>
        /// <returns>ProspectFields</returns> 
        public ProspectFields FindByfkIndicacao(
                               int Param_fkIndicacao )
        {
            ProspectFields infoFields = new ProspectFields();
            try
            {
                using (this.Conn = new SqlConnection(this.StrConnetionDB))
                {
                    using (this.Cmd = new SqlCommand("Proc_Prospect_FindByfkIndicacao", this.Conn))
                    {
                        this.Cmd.CommandType = CommandType.StoredProcedure;
                        this.Cmd.Parameters.Clear();
                        this.Cmd.Parameters.Add(new SqlParameter("@Param_fkIndicacao", SqlDbType.Int)).Value = Param_fkIndicacao;
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

        public DataTable GetAllProspectdPrincipal(int idUsuario)
        {
            DataSet dsProspects = new DataSet();
            try
            {
                SqlConnection Conn = new SqlConnection(this.StrConnetionDB);

                string query = GetQueryByModuloUser(idUsuario);

                Conn.Open();
                //DataTable dt = new DataTable();
                SqlCommand Cmd = new SqlCommand(query.ToString(), Conn);
                Cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(Cmd);
                da.Fill(dsProspects, "Usuario");

                return dsProspects.Tables[0];

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

        public DataTable GetAllProspectdPrincipal(int idUsuario, String NameProspect, String NameContact, String TypeProspect, String CPF, String CNPJ, String Situacao)
        {
            DataSet dsProspects = new DataSet();
            try
            {
                SqlConnection Conn = new SqlConnection(this.StrConnetionDB);

                string query = GetQueryByModuloUser(idUsuario, NameProspect, NameContact, TypeProspect, CPF, CNPJ, Situacao);

                Conn.Open();
                DataTable dt = new DataTable();
                SqlCommand Cmd = new SqlCommand(query.ToString(), Conn);
                Cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(Cmd);
                da.Fill(dsProspects, "Usuario");

                return dsProspects.Tables[0];
                
            }
            catch (SqlException e)
            {
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

        private string GetQueryByModuloUser(int idUsuario)
        {
            StringBuilder query = new StringBuilder();
            string where = string.Empty;

            query.Append("select a.idProspect, a.Nome, a.Telefone, a.Tipo, a.Email, a.PessoaContato, a.Observacao, b.Situacao, b.DataContato from prospect a LEFT JOIN contato b ON a.idProspect = b.fkProspect");

            return query.ToString(); ;
        }

        private string GetQueryByModuloUser(int idUsuario, String NameProspect, String NameContact, String TypeProspect, String CPF, String CNPJ, String Situacao)
        {
            UsuarioFields usuarioObj = new UsuarioControl().GetItem(idUsuario);
            StringBuilder query = new StringBuilder();
            string where = string.Empty;

            query.Append(@"select distinct a.idProspect, a.Nome, a.Telefone, a.Tipo, a.Email, a.PessoaContato, a.Observacao, a.SituacaoProspect,(select Max(c.DataContato) from Contato c where c.fkProspect = a.idProspect) as dataContato , a.CPF, a.CNPJ from prospect a LEFT JOIN contato b ON a.idProspect = b.fkProspect inner join Usuario u on u.idUsuario = a.FkUsuario WHERE 1=1 ");

            if (!String.IsNullOrEmpty(Situacao))
            {
                switch (Situacao)
                {
                    case "Em Andamento":
                        query.Append(" AND a.SituacaoProspect = 'Em Andamento' ");
                        break;
                    case "Associada":
                        query.Append(" AND a.SituacaoProspect = 'Associada' ");
                        break;
                    case "Encerrada":
                        query.Append(" AND a.SituacaoProspect = 'Encerrada' ");
                        break;
                    case "Não Contatado":
                        query.Append(" AND a.SituacaoProspect = 'Não Contatado' ");
                        break;
                    case "Indicação":
                        query.Append(" AND a.SituacaoProspect = 'Indicação' ");
                        break;
                }
            }

            switch (usuarioObj.Modulo)
            {
                case "U":
                    query.AppendFormat(" AND u.idUsuario = {0}", usuarioObj.idUsuario);
                    break;

                case "A":
                    query.AppendFormat(" AND u.FkUa = {0}", usuarioObj.FkUa);
                    break;

                case "M":
                    break;
            }


            if (!String.IsNullOrEmpty(NameProspect))
                query.AppendFormat(" AND a.Nome like '%{0}%'", NameProspect);

            if (!String.IsNullOrEmpty(NameContact))
                query.AppendFormat(" AND a.PessoaContato like '%{0}%'", NameContact);

            if (!TypeProspect.Equals("Todas"))
                query.AppendFormat(" AND a.Tipo = '{0}'", TypeProspect);

            if (!String.IsNullOrEmpty(CPF))
                query.AppendFormat(" AND a.CPF = '{0}'", CPF);

            if (!String.IsNullOrEmpty(CNPJ))
                query.AppendFormat(" AND a.CNPJ = '{0}'", CNPJ);

            query.Append(@" Group by a.idProspect
			                          , a.Nome
                                      , a.Telefone
                                      , a.Tipo
                                      , a.Email
                                      , a.PessoaContato
                                      , a.Observacao
                                      , a.SituacaoProspect
                                     -- , b.DataContato
                                      , a.CPF
                                      , a.CNPJ 
                        ORDER BY dataContato DESC ");

            return query.ToString(); ;
        }

        private string GetWhereByModuloUser(int idUsuario, int idProspect)
        {
            UsuarioFields usuarioObj = new UsuarioFields();
            UsuarioControl usuarioDal = new UsuarioControl();
            StringBuilder where = new StringBuilder();
            usuarioObj = usuarioDal.GetItem(idUsuario);

            switch (usuarioObj.Modulo)
            {
                case "U":
                    where.AppendFormat(" AND u.idUsuario = {0}", idUsuario);
                    break;

                case "A":
                    where.AppendFormat(" AND u.FkUa = {0}", usuarioObj.FkUa);
                    break;

                case "M":
                    break;
            }

            where.AppendFormat(" And c.Situacao = '{0}'", GetSituacaoProspect(idProspect));

            return where.ToString();
        }

        public string GetSituacaoProspect(int idProspect)
        {
            try
            {
                SqlConnection Conn = new SqlConnection(this.StrConnetionDB);
                StringBuilder querySituacao = new StringBuilder();
                DataSet dsSituacaoProspect = new DataSet();
                string situacaoProspectAtual = string.Empty;

                //Encerrada  
                querySituacao.Clear();
                dsSituacaoProspect.Clear();
                querySituacao.Append(" SELECT distinct c.Situacao");
                querySituacao.Append(" FROM Prospect p , Contato c");
                querySituacao.Append(" WHERE p.idProspect = c.fkProspect");
                querySituacao.Append(" AND c.Situacao = 'Encerrada'");
                querySituacao.AppendFormat(" AND p.idProspect = {0}", idProspect);

                SqlCommand Cmd = new SqlCommand(querySituacao.ToString(), Conn);
                Cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(Cmd);
                da.Fill(dsSituacaoProspect, "Prospect");

                if (dsSituacaoProspect.Tables[0].Rows.Count > 0)
                    situacaoProspectAtual = "Encerrada";


                if (string.IsNullOrEmpty(situacaoProspectAtual))
                {
                    //Em Andamento

                    querySituacao.Append(" SELECT distinct c.Situacao");
                    querySituacao.Append(" FROM Prospect p , Contato c");
                    querySituacao.Append(" WHERE p.idProspect = c.fkProspect");
                    querySituacao.Append(" AND c.Situacao = 'Em Andamento'");
                    querySituacao.AppendFormat(" AND p.idProspect = {0}", idProspect);

                    Conn.Open();
                    Cmd = new SqlCommand(querySituacao.ToString(), Conn);
                    Cmd.CommandType = CommandType.Text;
                    da = new SqlDataAdapter(Cmd);
                    da.Fill(dsSituacaoProspect, "Prospect");

                    if (dsSituacaoProspect.Tables[0].Rows.Count > 0)
                        situacaoProspectAtual = "Em Andamento";
                }

                else
                {
                    //Associada  
                    querySituacao.Clear();
                    dsSituacaoProspect.Clear();
                    querySituacao.Append(" SELECT distinct c.Situacao");
                    querySituacao.Append(" FROM Prospect p , Contato c");
                    querySituacao.Append(" WHERE p.idProspect = c.fkProspect");
                    querySituacao.Append(" AND c.Situacao = 'Associada'");
                    querySituacao.AppendFormat(" AND p.idProspect = {0}", idProspect);

                    Cmd = new SqlCommand(querySituacao.ToString(), Conn);
                    Cmd.CommandType = CommandType.Text;
                    if (dsSituacaoProspect.Tables[0].Rows.Count > 0)
                        situacaoProspectAtual = "Associada";
                }

                return situacaoProspectAtual;
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

        public bool GetProspectAssociado(int idProspect)
        {
            try
            {
                SqlConnection Conn = new SqlConnection(this.StrConnetionDB);
                StringBuilder querySituacao = new StringBuilder();
                DataSet dsSituacaoProspect = new DataSet();
                bool prospectAssociado = false;

                Conn.Open();
                //cria teste associacao Associada  
                querySituacao.Clear();
                dsSituacaoProspect.Clear();
                querySituacao.Append(" SELECT distinct c.Situacao");
                querySituacao.Append(" FROM Prospect p , Contato c");
                querySituacao.Append(" WHERE p.idProspect = c.fkProspect");
                querySituacao.Append(" AND c.Situacao = 'Associada'");
                querySituacao.AppendFormat(" AND p.idProspect = {0}", idProspect);

                SqlCommand Cmd = new SqlCommand(querySituacao.ToString(), Conn);
                Cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(Cmd);
                da.Fill(dsSituacaoProspect, "ProspectAssociado");

                if (dsSituacaoProspect != null)
                    prospectAssociado = true;


                return prospectAssociado;
            }
            catch (SqlException e)
            {
                //this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: Código do erro: {0}, Mensagem: {1}, Procedimento: {2}, Linha do erro {3}.", e.ErrorCode, e.Message, e.Procedure, e.LineNumber);
                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar selecionar o(s) registro(s) solicitados: {0}.", e.Message);
                return false;
            }
            catch (Exception e)
            {
                this._ErrorMessage = e.Message;
                return false;
            }
            finally
            {
                if (this.Conn != null)
                    if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
            }
        }



        #region Função GetDataFromReader

        /// <summary> 
        /// Retorna um objeto ProspectFields preenchido com os valores dos campos do SqlDataReader
        /// </summary>
        /// <param name="dr">SqlDataReader - Preenche o objeto ProspectFields </param>
        /// <returns>ProspectFields</returns>
        private ProspectFields GetDataFromReader( SqlDataReader dr )
        {
            ProspectFields infoFields = new ProspectFields();

            if (!dr.IsDBNull(0))
            { infoFields.idProspect = dr.GetInt32(0); }
            else
            { infoFields.idProspect = 0; }



            if (!dr.IsDBNull(1))
            { infoFields.Nome = dr.GetString(1); }
            else
            { infoFields.Nome = string.Empty; }



            if (!dr.IsDBNull(2))
            { infoFields.Endereco = dr.GetString(2); }
            else
            { infoFields.Endereco = string.Empty; }



            if (!dr.IsDBNull(3))
            { infoFields.Telefone = dr.GetString(3); }
            else
            { infoFields.Telefone = string.Empty; }



            if (!dr.IsDBNull(4))
            { infoFields.Tipo = dr.GetString(4); }
            else
            { infoFields.Tipo = string.Empty; }



            if (!dr.IsDBNull(5))
            { infoFields.Segmento = dr.GetString(5); }
            else
            { infoFields.Segmento = string.Empty; }



            if (!dr.IsDBNull(6))
            { infoFields.Observacao = dr.GetString(6); }
            else
            { infoFields.Observacao = string.Empty; }



            if (!dr.IsDBNull(7))
            { infoFields.Email = dr.GetString(7); }
            else
            { infoFields.Email = string.Empty; }



            if (!dr.IsDBNull(8))
            { infoFields.Bairro = dr.GetString(8); }
            else
            { infoFields.Bairro = string.Empty; }



            if (!dr.IsDBNull(9))
            { infoFields.Cidade = dr.GetString(9); }
            else
            { infoFields.Cidade = string.Empty; }



            if (!dr.IsDBNull(10))
            { infoFields.Estado = dr.GetString(10); }
            else
            { infoFields.Estado = string.Empty; }



            if (!dr.IsDBNull(11))
            { infoFields.DataCadastro = dr.GetDateTime(11); }
            else
            { infoFields.DataCadastro = DateTime.MinValue; }



            if (!dr.IsDBNull(12))
            { infoFields.PessoaContato = dr.GetString(12); }
            else
            { infoFields.PessoaContato = string.Empty; }



            if (!dr.IsDBNull(13))
            { infoFields.CPF = dr.GetString(13); }
            else
            { infoFields.CPF = string.Empty; }



            if (!dr.IsDBNull(14))
            { infoFields.CNPJ = dr.GetString(14); }
            else
            { infoFields.CNPJ = string.Empty; }



            if (!dr.IsDBNull(15))
            { infoFields.FkUsuario = dr.GetInt32(15); }
            else
            { infoFields.FkUsuario = 0; }



            if (!dr.IsDBNull(16))
            { infoFields.SituacaoProspect = dr.GetString(16); }
            else
            { infoFields.SituacaoProspect = string.Empty; }



            if (!dr.IsDBNull(17))
            { infoFields.fkIndicacao = dr.GetInt32(17); }
            else
            { infoFields.fkIndicacao = 0; }


            return infoFields;
        }
        #endregion










































        #region Função GetAllParameters

        /// <summary> 
        /// Retorna um array de parâmetros com campos para atualização, seleção e inserção no banco de dados
        /// </summary>
        /// <param name="FieldInfo">Objeto ProspectFields</param>
        /// <param name="Modo">Tipo de oepração a ser executada no banco de dados</param>
        /// <returns>SqlParameter[] - Array de parâmetros</returns> 
        private SqlParameter[] GetAllParameters( ProspectFields FieldInfo, SQLMode Modo )
        {
            SqlParameter[] Parameters;

            switch (Modo)
            {
                case SQLMode.Add:
                    Parameters = new SqlParameter[18];
                    for (int I = 0; I < Parameters.Length; I++)
                       Parameters[I] = new SqlParameter();
                    //Field idProspect
                    Parameters[0].SqlDbType = SqlDbType.Int;
                    Parameters[0].Direction = ParameterDirection.Output;
                    Parameters[0].ParameterName = "@Param_idProspect";
                    Parameters[0].Value = DBNull.Value;

                    break;

                case SQLMode.Update:
                    Parameters = new SqlParameter[18];
                    for (int I = 0; I < Parameters.Length; I++)
                       Parameters[I] = new SqlParameter();
                    //Field idProspect
                    Parameters[0].SqlDbType = SqlDbType.Int;
                    Parameters[0].ParameterName = "@Param_idProspect";
                    Parameters[0].Value = FieldInfo.idProspect;

                    break;

                case SQLMode.SelectORDelete:
                    Parameters = new SqlParameter[1];
                    for (int I = 0; I < Parameters.Length; I++)
                       Parameters[I] = new SqlParameter();
                    //Field idProspect
                    Parameters[0].SqlDbType = SqlDbType.Int;
                    Parameters[0].ParameterName = "@Param_idProspect";
                    Parameters[0].Value = FieldInfo.idProspect;

                    return Parameters;

                default:
                    Parameters = new SqlParameter[18];
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

            //Field Endereco
            Parameters[2].SqlDbType = SqlDbType.VarChar;
            Parameters[2].ParameterName = "@Param_Endereco";
            if (( FieldInfo.Endereco == null ) || ( FieldInfo.Endereco == string.Empty ))
            { Parameters[2].Value = DBNull.Value; }
            else
            { Parameters[2].Value = FieldInfo.Endereco; }
            Parameters[2].Size = 250;

            //Field Telefone
            Parameters[3].SqlDbType = SqlDbType.VarChar;
            Parameters[3].ParameterName = "@Param_Telefone";
            if (( FieldInfo.Telefone == null ) || ( FieldInfo.Telefone == string.Empty ))
            { Parameters[3].Value = DBNull.Value; }
            else
            { Parameters[3].Value = FieldInfo.Telefone; }
            Parameters[3].Size = 11;

            //Field Tipo
            Parameters[4].SqlDbType = SqlDbType.VarChar;
            Parameters[4].ParameterName = "@Param_Tipo";
            if (( FieldInfo.Tipo == null ) || ( FieldInfo.Tipo == string.Empty ))
            { Parameters[4].Value = DBNull.Value; }
            else
            { Parameters[4].Value = FieldInfo.Tipo; }
            Parameters[4].Size = 2;

            //Field Segmento
            Parameters[5].SqlDbType = SqlDbType.VarChar;
            Parameters[5].ParameterName = "@Param_Segmento";
            if (( FieldInfo.Segmento == null ) || ( FieldInfo.Segmento == string.Empty ))
            { Parameters[5].Value = DBNull.Value; }
            else
            { Parameters[5].Value = FieldInfo.Segmento; }
            Parameters[5].Size = 30;

            //Field Observacao
            Parameters[6].SqlDbType = SqlDbType.VarChar;
            Parameters[6].ParameterName = "@Param_Observacao";
            if (( FieldInfo.Observacao == null ) || ( FieldInfo.Observacao == string.Empty ))
            { Parameters[6].Value = DBNull.Value; }
            else
            { Parameters[6].Value = FieldInfo.Observacao; }
            Parameters[6].Size = 300;

            //Field Email
            Parameters[7].SqlDbType = SqlDbType.VarChar;
            Parameters[7].ParameterName = "@Param_Email";
            if (( FieldInfo.Email == null ) || ( FieldInfo.Email == string.Empty ))
            { Parameters[7].Value = DBNull.Value; }
            else
            { Parameters[7].Value = FieldInfo.Email; }
            Parameters[7].Size = 50;

            //Field Bairro
            Parameters[8].SqlDbType = SqlDbType.VarChar;
            Parameters[8].ParameterName = "@Param_Bairro";
            if (( FieldInfo.Bairro == null ) || ( FieldInfo.Bairro == string.Empty ))
            { Parameters[8].Value = DBNull.Value; }
            else
            { Parameters[8].Value = FieldInfo.Bairro; }
            Parameters[8].Size = 100;

            //Field Cidade
            Parameters[9].SqlDbType = SqlDbType.VarChar;
            Parameters[9].ParameterName = "@Param_Cidade";
            if (( FieldInfo.Cidade == null ) || ( FieldInfo.Cidade == string.Empty ))
            { Parameters[9].Value = DBNull.Value; }
            else
            { Parameters[9].Value = FieldInfo.Cidade; }
            Parameters[9].Size = 100;

            //Field Estado
            Parameters[10].SqlDbType = SqlDbType.VarChar;
            Parameters[10].ParameterName = "@Param_Estado";
            if (( FieldInfo.Estado == null ) || ( FieldInfo.Estado == string.Empty ))
            { Parameters[10].Value = DBNull.Value; }
            else
            { Parameters[10].Value = FieldInfo.Estado; }
            Parameters[10].Size = 2;

            //Field DataCadastro
            Parameters[11].SqlDbType = SqlDbType.SmallDateTime;
            Parameters[11].ParameterName = "@Param_DataCadastro";
            if ( FieldInfo.DataCadastro == DateTime.MinValue )
            { Parameters[11].Value = DBNull.Value; }
            else
            { Parameters[11].Value = FieldInfo.DataCadastro; }

            //Field PessoaContato
            Parameters[12].SqlDbType = SqlDbType.VarChar;
            Parameters[12].ParameterName = "@Param_PessoaContato";
            if (( FieldInfo.PessoaContato == null ) || ( FieldInfo.PessoaContato == string.Empty ))
            { Parameters[12].Value = DBNull.Value; }
            else
            { Parameters[12].Value = FieldInfo.PessoaContato; }
            Parameters[12].Size = 150;

            //Field CPF
            Parameters[13].SqlDbType = SqlDbType.VarChar;
            Parameters[13].ParameterName = "@Param_CPF";
            if (( FieldInfo.CPF == null ) || ( FieldInfo.CPF == string.Empty ))
            { Parameters[13].Value = DBNull.Value; }
            else
            { Parameters[13].Value = FieldInfo.CPF; }
            Parameters[13].Size = 50;

            //Field CNPJ
            Parameters[14].SqlDbType = SqlDbType.VarChar;
            Parameters[14].ParameterName = "@Param_CNPJ";
            if (( FieldInfo.CNPJ == null ) || ( FieldInfo.CNPJ == string.Empty ))
            { Parameters[14].Value = DBNull.Value; }
            else
            { Parameters[14].Value = FieldInfo.CNPJ; }
            Parameters[14].Size = 50;

            //Field FkUsuario
            Parameters[15].SqlDbType = SqlDbType.Int;
            Parameters[15].ParameterName = "@Param_FkUsuario";
            Parameters[15].Value = FieldInfo.FkUsuario;

            //Field SituacaoProspect
            Parameters[16].SqlDbType = SqlDbType.VarChar;
            Parameters[16].ParameterName = "@Param_SituacaoProspect";
            if (( FieldInfo.SituacaoProspect == null ) || ( FieldInfo.SituacaoProspect == string.Empty ))
            { Parameters[16].Value = DBNull.Value; }
            else
            { Parameters[16].Value = FieldInfo.SituacaoProspect; }
            Parameters[16].Size = 20;

            //Field fkIndicacao
            Parameters[17].SqlDbType = SqlDbType.Int;
            Parameters[17].ParameterName = "@Param_fkIndicacao";
            Parameters[17].Value = FieldInfo.fkIndicacao;

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

        ~ProspectControl() 
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
