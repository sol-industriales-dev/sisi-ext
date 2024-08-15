using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Full
{
    public partial class Multiple : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ctlDoc1.OpenDocument(Server.MapPath("~/files/Sample.ppt"));
                ctlDoc2.OpenDocument(Server.MapPath("~/files/Sample.doc"));
                ctlDoc3.OpenDocument(Server.MapPath(("~/files/Sample.tif")));
                ctlDoc4.OpenDocument(Server.MapPath(("~/files/Sample.xls")));
            }

        }
    }
}