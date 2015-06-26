using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace DKO.Framework
{
    public static class Helper
    {
        public static int GetSelectedGridItemID(GridView grid, string selectedPosition)
        {
            string code = "";

            if (selectedPosition != "")
            {
                HtmlInputRadioButton rdoItem = (HtmlInputRadioButton)grid.Rows[Convert.ToInt32(selectedPosition)].FindControl("rdoItem");
                Literal litValue = (Literal)grid.Rows[Convert.ToInt32(selectedPosition)].FindControl("litValue");

                code = litValue.Text;
            }

            return Convert.ToInt32(code);
        }
    }
}
