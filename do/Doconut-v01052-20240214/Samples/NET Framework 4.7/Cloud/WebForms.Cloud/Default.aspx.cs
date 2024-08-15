using System;
using System.Web.UI;

public partial class _Default : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string token = "";

            if (null != Request.QueryString["token"])
            {
                token = Convert.ToString(Request.QueryString["token"]); // Receive a token from Upload.aspx
		        // Once a file is uploaded to cloud, use it's token each time to view the document
            }
            else
            {
                Response.Redirect("Upload.aspx");
            }

            // View the token (existing value or a newly generated one)

            if (token.Length > 0)
            {
                string scriptJs = " $(document).ready(function () {  " + ctlDoc.GetAjaxInitArguments(token) + "; " + " $('#divLoading').hide(); });";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "docView", scriptJs, true);
            }
        }
    }
   
}
