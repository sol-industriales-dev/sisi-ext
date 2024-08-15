using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Full
{
    public partial class Attachments : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack && null != Request.QueryString["File"])
            {
                ctlDoc.OpenDocument(Server.MapPath("~/files/" + Convert.ToString(Request.QueryString["File"])));
            }
        }
    }
}