using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace SWGPgen
{
    public class Reports
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



        public DataTable GeraRelatorioAnalitico(int idUsuario, DateTime dtInical, DateTime dtFinal)
        {
             DataSet dsRelAnalitico = new DataSet();
            try
            {
                if (idUsuario <= 0)
                    idUsuario = 0;

                SqlConnection Conn = new SqlConnection(this.StrConnetionDB);

                string query = GetQueryAnalitico(idUsuario, dtInical, dtFinal);

                Conn.Open();
                DataTable dt = new DataTable();
                SqlCommand Cmd = new SqlCommand(query.ToString(), Conn);
                Cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(Cmd);
                da.Fill(dsRelAnalitico, "RelAnalitico");

                return  dt = dsRelAnalitico.Tables[0];

            }
            catch (SqlException e)
            {
                this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar gerar o relatórios: {0}.", e.Message);
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

        private string GetQueryAnalitico(int idUsuario, DateTime dtInical, DateTime dtFinal)
        {
            StringBuilder query = new StringBuilder();

            query.Append(" SELECT Usuario.Nome, Prospect.Nome AS NomeProspect, Prospect.Tipo, Prospect.Segmento, ");
            query.Append(" Contato.Tipo AS TipoContato, Contato.Situacao,Contato.DataCadastro, Contato.DataContato,  Contato.Descricao ");
            query.Append(" FROM Contato INNER JOIN   Prospect ON Contato.fkProspect = Prospect.idProspect INNER JOIN ");
            query.Append(" Usuario ON Prospect.FkUsuario = Usuario.idUsuario ");
            query.Append("Where 1 = 1");

             if(dtInical.Date != null)
                 query.AppendFormat(" And Contato.DataCadastro >= {0}", dtInical.ToShortDateString());

             if (dtFinal.Date != null)
                 query.AppendFormat(" And Contato.DataCadastro <= {0}", dtFinal.ToShortDateString());

             if (idUsuario > 0)
                 query.AppendFormat(" And Usuario.idUsuario <= {0}", idUsuario);

            query.Append(" GROUP BY Usuario.Nome, Prospect.Nome, Prospect.Tipo, Prospect.Segmento, Contato.Tipo, Contato.Situacao, Contato.DataContato, Contato.DataCadastro,  Contato.Descricao ");
            query.Append(" ORDER BY Usuario.Nome, Contato.DataContato ");
            
            return query.ToString();
        }

        //public DataTable GeraRelatorioAnalitico()
        //{
        //    DataSet dsRelAnalitico = new DataSet();
        //    try
        //    {
                
        //        SqlConnection Conn = new SqlConnection(this.StrConnetionDB);

        //        string query = GetQuery(0, Convert.ToDateTime("01/01/1900"), Convert.ToDateTime("01/01/1900"));

        //        Conn.Open();
        //        DataTable dt = new DataTable();
        //        SqlCommand Cmd = new SqlCommand(query.ToString(), Conn);
        //        Cmd.CommandType = CommandType.Text;
        //        SqlDataAdapter da = new SqlDataAdapter(Cmd);
        //        da.Fill(dsRelAnalitico, "RelAnalitico");

        //        return dt = dsRelAnalitico.Tables[0];

        //    }
        //    catch (SqlException e)
        //    {
        //        this._ErrorMessage = string.Format(@"Houve um erro imprevisto ao tentar gerar o relatórios: {0}.", e.Message);
        //        return null;
        //    }
        //    catch (Exception e)
        //    {
        //        this._ErrorMessage = e.Message;
        //        return null;
        //    }
        //    finally
        //    {
        //        if (this.Conn != null)
        //            if (this.Conn.State == ConnectionState.Open) { this.Conn.Dispose(); }
        //    }
        //}
    }
}
