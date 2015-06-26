using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using SWGPgen;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using SIMLgen;

namespace SWGP
{
    public partial class MainImportIndicacao : Common.BaseWebUi
    {
        private string ddlUaValue;

        public string txtCaminhofile { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            ddlUaValue = ddlUA.SelectedValue;
            ScriptManager.GetCurrent(this).RegisterPostBackControl(btnImportaLote);
            MessageBox.Include(this);
            BoundDropDownUA();
        }

        protected void btnImportaLote_Click(object sender, EventArgs e)
        {
            string filename = fuNomeArquivo.PostedFile.FileName;
            //string filename = "C:\fileFGP" + Path.GetExtension(fuNomeArquivo.PostedFile.FileName);
            string extensionFile = Path.GetExtension(filename);
            if (fuNomeArquivo.HasFile)
            {
                if (extensionFile.ToLower().Equals(".xls") || extensionFile.ToLower().Equals(".xlsx"))
                {
                    int idUsuarioRecebe = int.Parse(ddlUsuarioParaIndicacao.SelectedItem.Value);
                    ReadExcelInfo(filename, idUsuarioRecebe);

                }
                else
                {
                    MessageBox.Show("Tipo de arquivo incorreto, favor verificar", MessageBox.MessageType.Warning);
                    return;
                }
                
                
            }
            else 
            {
                MessageBox.Show("Nenhum arquivo selecionado.", MessageBox.MessageType.Info);
            }
            
            
        }

        private void ReadExcelInfo(string fileName, int idUsuario)
        {
            SqlTransaction trans = null;
            SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["StringConn"].ToString());
            Conn.Open();
            trans = Conn.BeginTransaction();
            
            try
            {
                DataSet dsFile = new DataSet(); 
                OleDbDataAdapter MyCommand;
               // OleDbConnection MyConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties=Excel 8.0;"); 
                OleDbConnection MyConnection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\"+ fileName + ";Extended Properties=Excel 12.0;"); 
                MyCommand = new System.Data.OleDb.OleDbDataAdapter("SELECT * FROM [Plan1$]", MyConnection); 
                MyCommand.TableMappings.Add("Table", "TableIndicacao");
                MyCommand.Fill(dsFile);

                foreach (DataRow row in dsFile.Tables["TableIndicacao"].Rows)
                {
                    
                    UsuarioFields userFazIndicacao = (UsuarioFields)Session["usuarioObj"];

                    ProspectControl prospectDal = new ProspectControl();
                    ProspectFields indicacaoRecebida = new ProspectFields();
                    ProspectValidator prospectBus = new ProspectValidator();

                    indicacaoRecebida.Bairro = row["Bairro"].ToString();
                    indicacaoRecebida.Cidade = row["Cidade"].ToString();
                    indicacaoRecebida.Endereco = row["Endereço"].ToString();
                    indicacaoRecebida.Estado = row["Estado"].ToString();
                    indicacaoRecebida.Nome = row["Nome"].ToString();
                    indicacaoRecebida.Telefone = row["Telefone"].ToString();
                    indicacaoRecebida.Tipo = row["Tipo_Pessoa"].ToString();
                    indicacaoRecebida.Segmento = row["Segmento"].ToString();
                    indicacaoRecebida.CPF = row["CPF"].ToString();
                    indicacaoRecebida.CNPJ = row["CNPJ"].ToString();
                    indicacaoRecebida.PessoaContato = row["Contato"].ToString();
                    indicacaoRecebida.Email = row["Email"].ToString();
                    indicacaoRecebida.DataCadastro = DateTime.Now;
                    indicacaoRecebida.SituacaoProspect = "Indicação";
                    indicacaoRecebida.FkUsuario = int.Parse(ddlUsuarioParaIndicacao.SelectedValue);
                    indicacaoRecebida.fkIndicacao = 999;
                    if (prospectBus.isValid(indicacaoRecebida))
                        prospectDal.Add(ref indicacaoRecebida);
                    else
                    {
                        trans.Rollback();
                        Conn.Close();
                        MyConnection.Close();
                        throw new Exception("Erro ao tentar importar indicação.");
                    }
                    
                }

                trans.Commit();
                Conn.Close();
                MyConnection.Close();
                MessageBox.Show("Importação realizada com sucesso",MessageBox.MessageType.Success);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, MessageBox.MessageType.Error);
            }
            
        }

        private void BoundDropDownUA()
        {
            ddlUA.ClearSelection();
            ddlUA.DataTextField = "Nome";
            ddlUA.DataValueField = "idUA";
            ddlUA.DataSource = new UAControl().GetAll();
            ddlUA.DataBind();
            ddlUA.Items.Insert(0,(new ListItem("Selecione...")));

        }

        protected void ddlUA_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            UsuarioControl usuarios = new UsuarioControl();
            ddlUsuarioParaIndicacao.ClearSelection();
            ddlUsuarioParaIndicacao.DataTextField = "UserName";
            ddlUsuarioParaIndicacao.DataValueField = "idUsuario";
            ddlUsuarioParaIndicacao.DataTextField = "UserName";
            ddlUsuarioParaIndicacao.DataValueField = "idUsuario";
            ddlUsuarioParaIndicacao.DataSource = usuarios.GetAllUsersByUA(int.Parse(ddlUaValue));
            ddlUsuarioParaIndicacao.DataBind();
            this.ddlUA.Items.FindByValue(ddlUaValue).Selected = true;
            
        }

      }

    }
