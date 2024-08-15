using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Full
{
    public partial class FlipPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Session["DOC_FB_Token"] = ctlDoc.OpenDocument(Server.MapPath("~/files/Sample.ppt"));
                Page.ClientScript.RegisterStartupScript(this.GetType(), "fb", "$(document).ready(function(){  InitFlipBook(); }); ", true);
            }
        }
    }
}