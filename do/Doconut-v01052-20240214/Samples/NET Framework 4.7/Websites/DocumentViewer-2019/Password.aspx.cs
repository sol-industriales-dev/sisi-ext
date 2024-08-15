using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotnetDaddy.DocumentViewer;

public partial class Password : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private string Document
    {
        get
        {
            if (null == ViewState["doc"])
                return "";

            return Convert.ToString(ViewState["doc"]);
        }
        set
        {
            ViewState["doc"] = value;
        }
    }

    protected void Upload_Click(object sender, EventArgs e)
    {
        if (txtUpload.HasFile)
        {
            string fileName = txtUpload.FileName;

            string uploadedFile = Server.MapPath("~/files/" + DateTime.Now.Ticks.ToString() + "-" + fileName);
            txtUpload.SaveAs(uploadedFile);


            ctlDoc.OpenDocument(uploadedFile);

            this.Document = uploadedFile;

            if (ctlDoc.InternalError.Length > 0 && ctlDoc.InternalError.IndexOf("password", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                pnlPassword.Visible = true;
                txtUpload.Visible = false;
                btnUpload.Visible = false;

                ctlDoc.CloseDocument();

                txtPassword.Focus();
            }
            else
            {
                Response.Write(ctlDoc.InternalError);
                Response.End();
            }

        }
        else
        {
            Response.Write("Please select a file first.");
            Response.End();

        }
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        if (this.Document.Length > 0 && txtPassword.Text.Length > 0)
        {
            // provide password
            ctlDoc.Password = txtPassword.Text;

            string token = ctlDoc.OpenDocument(this.Document);

            if (token.Length > 0)
            {
                pnlPassword.Visible = false;
                divDocViewer.Visible = true;
            }
            else
            {
                Response.Write(ctlDoc.InternalError);
                Response.End();
            }
        }

    }
}