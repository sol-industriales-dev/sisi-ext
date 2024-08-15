using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotnetDaddy.DocumentViewer;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

public partial class PagePreview : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["page"] != null && Request.QueryString["token"] != null)
        {
            var iThumb = Request.QueryString["page"];
            var token = Request.QueryString["token"];

            using (var ms = new MemoryStream())
            {
                (Session[token + "-" + iThumb.ToString()] as Bitmap).Save(ms, ImageFormat.Png);
                Response.ContentType = "image/png";
                ms.WriteTo(Response.OutputStream);
                Response.End();
            }
        }
    }

    protected void Upload_Insert_Click(object sender, EventArgs e)
    {
        if (txtUpload.HasFile)
        {
            string fileName = txtUpload.FileName;

            string validFiles = ".DOC .DOCX .PPT .PPTX .PDF";
            string fileExtension = new System.IO.FileInfo(fileName).Extension.ToUpper();

            if (validFiles.IndexOf(fileExtension) < 0)
            {
                return;
            }

            string uploadedFile = Server.MapPath("~/files/" + DateTime.Now.Ticks.ToString() + "-" + fileName);
            txtUpload.SaveAs(uploadedFile);

            ProcessDocument(uploadedFile);

        }
        else
        {
            Response.Write("<H1>Please select a file first.</H3>");

            Response.End();

        }
    }

    private void ProcessDocument(string myFile)
    {
        using (var ctlDoc = new DocViewer { ID = "ctlDoc" })
        {
            string token = ctlDoc.OpenDocument(myFile);

            if (token.Length > 0)
            {
                int pages = ctlDoc.TotalPages;

                for (int iThumb = 1; iThumb <= pages; iThumb++)
                {
                    Session[token + "-" + iThumb.ToString()] = ctlDoc.GetThumbnail(iThumb, 300, 0, false); // change params as required
                }

                GenerateUI(token, pages);
            }
            else
            {
                Response.Write("<H1>" + ctlDoc.InternalError + "</H1>");
                Response.End();
            }
        }
    }


    private string GenerateUI(string token, int pages)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "showThumbnailUI", "$().ready(function() { ShowThumbnails('" + token + "'," + pages.ToString() + "); } );", true);
        return "";
    }
}