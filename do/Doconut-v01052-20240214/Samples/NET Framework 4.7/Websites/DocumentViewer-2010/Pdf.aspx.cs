using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using DotnetDaddy.DocumentViewer;

public partial class Pdf : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void LaunchAsPDF(string myFile)
    {
        using (DocViewer ctlDoc = new DocViewer())
        {
            ctlDoc.OpenDocument(myFile);

            byte[] pdf = ctlDoc.ExportToPdf();

            if (pdf != null)
            {
                Response.Clear();

                Response.ContentType = "Application/pdf";

                Response.BinaryWrite(pdf);

                Response.End();
            }
            else
            {
                Response.Write("<H1>" + ctlDoc.InternalError + "</H1><H3>If it is a file format error then please send details and sample file to admin@doconut.com</H3>");

                Response.End();

            }
        }
    }

    protected void Upload_Insert_Click(object sender, EventArgs e)
    {
        if (txtUpload.HasFile)
        {
            string fileName = txtUpload.FileName;

            string uploadedFile = Server.MapPath("~/files/" + DateTime.Now.Ticks.ToString() + "-" + fileName);
            txtUpload.SaveAs(uploadedFile);

            LaunchAsPDF(uploadedFile);
        }
        else
        {
            Response.Write("<H1>Please select a file first.</H3>");

            Response.End();

        }
    }

}