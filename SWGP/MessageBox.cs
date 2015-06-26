using System;
using System.Text;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Exibe mensagens visuais na página atual
/// </summary>
public class MessageBox
{
    protected static Hashtable handlerPages = new Hashtable();

    //Classe com os dados que serão armazenados das mensagens
    private class MessageBoxItem
    {
        public string Message;
        public MessageType Type;

        public MessageBoxItem(string message,MessageType type)
        {
            Message = message;
            Type = type;
        }
    }

    //Enum com os possíveis tipos de mensagens
    public enum MessageType
    {
        Error = 1,
        Info = 2,
        Success = 3,
        Warning = 4
    }

    /// <summary>
    /// Adiciona uma nova mensagem na página
    /// </summary>
    public static void Show(string message, MessageType type)
    {
        if (!(handlerPages.Contains(HttpContext.Current.Handler)))
        {
            Page currentPage = (Page)HttpContext.Current.Handler;
            if (!((currentPage == null)))
            {
                Queue queue = new Queue();
                queue.Enqueue(new MessageBoxItem(message, type));
                handlerPages.Add(HttpContext.Current.Handler, queue);
                currentPage.PreRenderComplete += new EventHandler(CurrentPagePreRender);
            }
        }
        else
        {
            Queue queue = ((Queue)(handlerPages[HttpContext.Current.Handler]));
            queue.Enqueue(new MessageBoxItem(message, type));
        }
    }

    private static void CurrentPagePreRender(object sender, EventArgs e)
    {
        Page currentPage = (Page)HttpContext.Current.Handler;

        Include(currentPage);

        Queue queue = ((Queue)(handlerPages[HttpContext.Current.Handler]));
        if (queue != null)
        {
            String script = "";
            while (queue.Count > 0)
            {
                MessageBoxItem iMsg = (MessageBoxItem)queue.Dequeue();

                script += "MessageBox.queue.push(function(){";
                script += "Ext.Msg.show({ title: '', msg: '" + EncodeJsString(iMsg.Message) + "', width: 400, buttons: Ext.MessageBox.OK, icon: '" + iMsg.Type.ToString().ToLower() + "', fn: MessageBox.showNext })";
                script += "});";
            }

            script += "Ext.onReady(function(){ MessageBox.showNext(); });";
            ScriptManager.RegisterStartupScript(currentPage, currentPage.GetType(), "messageScript", script, true);
        }
    }

    private static void IncludeExtJS(Page page)
    {
        if (page.Header.FindControl("_ExtJsIncludes") == null)
        {
            LiteralControl includeControl = new LiteralControl();
            includeControl.ID = "_ExtJsIncludes";

            //ExtJS
            includeControl.Text += String.Format("<link rel=\"stylesheet\" href=\"{0}\" type=\"text/css\" media=\"screen\" />\n", page.ResolveUrl("~/messagebox/ExtJS/resources/css/ext-all.css"));
            includeControl.Text += String.Format("<script src=\"{0}\" type=\"text/javascript\"></script>\n", page.ResolveUrl("~/messagebox/ExtJS/ext-base.js"));
            includeControl.Text += String.Format("<script src=\"{0}\" type=\"text/javascript\"></script>\n", page.ResolveUrl("~/messagebox/ExtJS/ext-all.js"));
            
            //

            page.Header.Controls.Add(includeControl);
        }
    }

    private static void IncludeMessageBox(Page page)
    {
        LiteralControl includeControl = new LiteralControl();
        includeControl.ID = "_MessageBoxInclude";
        includeControl.Text += String.Format("<link rel=\"stylesheet\" href=\"{0}\" type=\"text/css\" media=\"screen\" />\n", page.ResolveUrl("~/messagebox/messagebox.css"));
        includeControl.Text += String.Format("<script src=\"{0}\" type=\"text/javascript\"></script>\n", page.ResolveUrl("~/messagebox/messagebox.js"));
        page.Header.Controls.Add(includeControl);
    }

    public static void Include(Page page)
    {
        IncludeExtJS(page);
        IncludeMessageBox(page);
    }

    /// <summary>
    /// Encodes a string to be represented as a string literal. The format
    /// is essentially a JSON string.
    /// 
    /// The string returned includes outer quotes 
    /// Example Output: "Hello \"Rick\"!\r\nRock on"
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string EncodeJsString(string s)
    {
        StringBuilder sb = new StringBuilder();
        foreach (char c in s)
        {
            switch (c)
            {
                case '\"':
                    sb.Append("\\\"");
                    break;
                case '\'':
                    sb.Append("\\'");
                    break;
                case '\\':
                    sb.Append("\\\\");
                    break;
                case '\b':
                    sb.Append("\\b");
                    break;
                case '\f':
                    sb.Append("\\f");
                    break;
                case '\n':
                    sb.Append("\\n");
                    break;
                case '\r':
                    sb.Append("\\r");
                    break;
                case '\t':
                    sb.Append("\\t");
                    break;
                default:
                    int i = (int)c;
                    if (i < 32 || i > 127)
                    {
                        sb.AppendFormat("\\u{0:X04}", i);
                    }
                    else
                    {
                        sb.Append(c);
                    }
                    break;
            }
        }

        return sb.ToString();
    }
}