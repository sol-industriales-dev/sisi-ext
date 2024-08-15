using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using DotnetDaddy.DocumentConfig;
using System.Text;

public partial class Ribbon : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ctlDoc.OpenDocument(Server.MapPath("~/files/Sample.doc"));
        }
    }

    protected void SaveDCN_Click(object sender, EventArgs e)
    {
        byte[] file = ctlDoc.SaveDocument();

        Response.Clear();
        Response.AppendHeader("Content-Disposition", "attachment; filename=Doconut.dcn");
        Response.AppendHeader("Content-Length", file.Length.ToString());
        Response.ContentType = "application/octet-stream";
        Response.BinaryWrite(file);
        Response.End();
    }


    protected void SavePDF_Click(object sender, EventArgs e)
    {
        byte[] pdf = ctlDoc.ExportToPdf();

        if (null != pdf)
        {
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=Doconut.pdf");
            Response.AppendHeader("Content-Length", pdf.Length.ToString());
            Response.ContentType = "application/octet-stream";
            Response.BinaryWrite(pdf);
            Response.End();
        }
        else
        {
            ShowInternalError();
        }
    }


    protected string GetIPAddress()
    {
        if (Request.IsLocal)
        {
            return "--";
        }

        string VisitorsIPAddr = string.Empty;

        if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
        {
            VisitorsIPAddr = Convert.ToString(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]);
        }
        else
        {
            VisitorsIPAddr = HttpContext.Current.Request.UserHostAddress;
        }

        return "--" + VisitorsIPAddr;
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        if (txtUrl.Text.Length > 0 && txtUrl.Text.StartsWith("http://"))
        {
            ctlDoc.OpenDocument(new Uri(txtUrl.Text));

            if (ctlDoc.InternalError.Length > 0)
            {
                ShowInternalError();
            }
        }
        else
        {
            txtUrl.Text = "Invalid url, start with http://";
        }
    }

    private void ShowInternalError()
    {
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "showalert",
                       "$(window).load(function () {alert('Error: " + ctlDoc.InternalError + "');});", true);
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        string fileName = uploadFile.PostedFile.FileName;

        string invalidFiles = ".EXE .JS .JAR .VBS .VB .SFX .BAT .DLL .TMP .PY .ASP .ASPX .ASHX .ASMX .AXD .PHP .MSI .COM .CMD .VBE .LNK .ZIP .RAR .7Z";
        string fileExtension = new System.IO.FileInfo(fileName).Extension.ToUpper();

        if (invalidFiles.IndexOf(fileExtension) > -1)
        {
            return;
        }

        string hostAdd = GetIPAddress();

        string uploadedFile = Server.MapPath("~/files/" + DateTime.Now.ToShortDateString().Replace("/", "-") + hostAdd + "--" + fileName);
        uploadFile.PostedFile.SaveAs(uploadedFile);

        // provide password if any
        // ctlDoc.Password = "";


        BaseConfig config = null;

	switch (new FileInfo(uploadedFile).Extension.ToUpper())
        {
            case ".DWG":
            case ".DXF":
                config = new CadConfig {DefaultRender = false , ShowColor = false, WhiteBackground = true, ShowModel = true, ShowLayouts = true, LineWidth = 1, Check3DSolid = false };
                break;
            case ".DOC":
            case ".DOCX":
            case ".ODT": 
                config = new WordConfig { ConvertPdf = true,  }; // turn off, if you don't want hyperlinks
                    var pdfConfig = (config as WordConfig).PdfConfig;
                    pdfConfig.ExtractHyperlinks = true;
                    pdfConfig.HyperlinksPageCount = 5; // check hyperlinks for first 5 pages only; specify 0 for all.
                break;
            case ".TXT":
                var txtEncoding = System.Text.Encoding.UTF8;
                config = new WordConfig { PaperSize = DocPaperSize.A4, FileEncoding = txtEncoding };
                break;
            case ".EML":
            case ".MSG":
                config = new EmailConfig { EmailEncoding = Encoding.UTF8, ConvertHtml = false };
                break;
            case ".XLS":
            case ".XLSX":
            case ".ODS":
                config = new ExcelConfig { SplitWorksheets = true, ShowEmptyWorkSheets = false, PaperSize = ExcelPaperSize.PaperA3, DocumentCulture = "en-US", AutoFitContents = true };
                break;
            case ".PPT":
            case ".PPTX":
            case ".ODP": 
                config = new PptConfig();
                break;
            case ".PDF":
                config = new PdfConfig { DefaultRender = true, ExtractHyperlinks = true, HyperlinksPageCount = 5 }; // specify true if you need hyperlinks
                break;
            case ".PNG":
            case ".BMP":
            case ".JPG":
            case ".JPEG":
            case ".PSD":
            case ".GIF":
                config = new ImageConfig { MaxImagePixelSize = 2000, TransparentPng = false };
                break;
            case ".MPP":
            case ".MPPX":
                config = new ProjectConfig { ExportPdfA = true, PaperSize = MppPaperSize.A3 };
                break;
            case ".VSD":
            case ".VSDX":
                config = new VisioConfig { ExportPdfA = true };
                break;
        }

        ctlDoc.OpenDocument(uploadedFile, config);


        if (ctlDoc.InternalError.Length > 0)
        {
            ShowInternalError();
        }
    }
}