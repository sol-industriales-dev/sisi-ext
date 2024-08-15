using System;
using System.Web.UI;
using System.IO;
using System.Text;
using System.Drawing.Imaging;
using DotnetDaddy.DocumentConfig;

public partial class Viewer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (ctlDoc.Token.Length > 0)
            {
                ctlDoc.CloseDocument();
                viewer.Visible = false;
                upload.Visible = true;
            }

            if (Request.QueryString["file"] != null)
            {
                ViewUploadedFile(Request.QueryString["file"]);
            }
        }
    }

    // Use this if you want to embed the license file (PROFESSIONAL & DISTRIBUTION LICENSE HOLDERS ONLY)
    protected override void OnInit(EventArgs e)
    {
        /*
          
        XmlDocument lic = new XmlDocument();
        lic.LoadXml(""); // Specify the xml string of the .lic file here (without any line breaks)

        DotnetDaddy.DocumentViewer.DocViewer.DoconutLicense(lic);
        base.OnInit(e);
          
         */
    }

    protected void ViewUploadedFile(string fileName)
    {
        string uploadedFile = Path.Combine(Server.MapPath("~/files/"), fileName);

        if (!File.Exists((uploadedFile)))
        {
            throw new Exception("File not found : " + uploadedFile);
        }

        // ctlDoc.Password = "provide any password here";

        BaseConfig config = null;

        switch (new FileInfo(uploadedFile).Extension.ToUpper())
        {
            case ".DWG":
            case ".DXF":
            case ".DGN":
                config = new CadConfig { DefaultRender = true, ShowColor = false, WhiteBackground = true, ShowModel = true, ShowLayouts = true, LineWidth = 1, Check3DSolid = false };
                break;
            case ".DOC":
            case ".DOCX":
            case ".ODT":
            case ".RTF":
                config = new WordConfig { ConvertPdf = true, }; // turn off, if you don't want search, copy & hyperlinks

                var pdfConfig = (config as WordConfig).PdfConfig;

                pdfConfig.DefaultRender = true;
                pdfConfig.ExtractHyperlinks = true;
                pdfConfig.AllowCopy = true;        // text copy
                pdfConfig.HyperlinksPageCount = 0; // check hyperlinks for first 5 pages only; specify 0 for all.

                break;
            case ".TXT":
                var txtEncoding = GetEncoding(uploadedFile);
                config = new WordConfig { PaperSize = DocPaperSize.A4, FileEncoding = txtEncoding };
                break;
            case ".EML":
            case ".MSG":
                config = new EmailConfig { EmailEncoding = Encoding.UTF8, ConvertHtml = false };
                break;
            case ".XLS":
            case ".XLSX":
            case ".ODS":
            case ".CSV":
                config = new ExcelConfig { SplitWorksheets = true, ShowEmptyWorkSheets = false, PaperSize = ExcelPaperSize.PaperA3, DocumentCulture = "en-US", AutoFitContents = true };
                break;
            case ".PPT":
            case ".PPTX":
            case ".ODP":
                config = new PptConfig { ConvertPdf = false }; // specify true if you want copy, search, links features like .DOC format above.
                break;
            case ".TIF":
            case ".TIFF":
                config = new TifConfig { DefaultRender = true };
                break;
            case ".PDF":
                config = new PdfConfig { DefaultRender = true, ExtractHyperlinks = true, HyperlinksPageCount = 0, AllowCopy = true };
                break;
            case ".BMP":
            case ".CDR":
            case ".CMX":
            case ".DCM":
            case ".DNG":
            case ".EPS":
            case ".GIF":
            case ".ICO":
            case ".JPG":
            case ".JPEG":
            case ".PNG":
            case ".PSD":
            case ".TGA":
            case ".WEBP":
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


        // OR

        /*

        // FileInfo fi = new FileInfo(uploadedFile);
        // FileStream ms = File.OpenRead(uploadedFile);
        // DocFileFormat format = DotnetDaddy.DocumentViewer.DocViewer.GetDocumentFormat(ms); // To know file format
        // ctlDoc.OpenDocument(ms, fi);

        */

        // OR

        /*
          
          // code to open document as byte[]

          byte[] bytes = File.ReadAllBytes(uploadedFile);
          ctlDoc.OpenDocument(bytes, fi.Extension); // or pass just extension as string, eg. "pdf"
        
        */

        if (ctlDoc.Token.Length > 0)
        {
            viewer.Visible = true;
            upload.Visible = false;
        }
        else
        {
            Response.Write("<H1>" + ctlDoc.InternalError + "</H1><H3>Please send details and sample file to admin@doconut.com</H3>");
            Response.End();
        }

    }

    private Encoding GetEncoding(string filename)
    {
        // Read the BOM
        var bom = new byte[4];
        using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
        {
            file.Read(bom, 0, 4);
        }

        // Analyze the BOM
        if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
        if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
        if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
        if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
        if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return Encoding.UTF32;
        return Encoding.ASCII;
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

    }

    protected void btnThumbnail_Click(object sender, EventArgs e)
    {
        //  If you want to save single page to tiff or a bitmap
        System.Drawing.Bitmap bmp = ctlDoc.GetThumbnail(1, 500, 0, false); // you may pass width or zoom, (1, 500, 0) or (1, 0, 50) etc..

        if (null != bmp)
        {
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=Thumbnail.jpg");
            Response.ContentType = "application/octet-stream";

            using (MemoryStream ms = new MemoryStream())
            {
                bmp.Save(ms, ImageFormat.Png);
                ms.WriteTo(Response.OutputStream);
                Response.End();
            }
        }
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        ctlDoc.CloseDocument();
        Response.Redirect("Viewer.aspx");
    }

    protected void btnSaveDocument_Click(object sender, EventArgs e)
    {
        //  Save current file to .dcn

        byte[] file = ctlDoc.SaveDocument();

        Response.Clear();
        Response.AppendHeader("Content-Disposition", "attachment; filename=Document.dcn");
        Response.AppendHeader("Content-Length", file.Length.ToString());
        Response.ContentType = "application/octet-stream";
        Response.BinaryWrite(file);
        Response.End();
        
    }

    protected void btnSaveSearch_Click(object sender, EventArgs e)
    {
        //  Save current file to .srh

        byte[] file = ctlDoc.SaveSearch();

        Response.Clear();
        Response.AppendHeader("Content-Disposition", "attachment; filename=Document.srh");
        Response.AppendHeader("Content-Length", file.Length.ToString());
        Response.ContentType = "application/octet-stream";
        Response.BinaryWrite(file);
        Response.End();

    }

    // get encoder info for specified mime type
    private static ImageCodecInfo GetEncoderInfo(String mimeType)
    {
        int j;
        ImageCodecInfo[] encoders;
        encoders = ImageCodecInfo.GetImageEncoders();
        for (j = 0; j < encoders.Length; ++j)
        {
            if (encoders[j].MimeType == mimeType)
                return encoders[j];
        }
        return null;
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        if (txtUrl.Text.Trim().Length > 0 && (txtUrl.Text.ToLower().StartsWith("http://") || txtUrl.Text.ToLower().StartsWith("https://")))
        {

            ctlDoc.OpenDocument(new Uri(txtUrl.Text.Trim()));


            if (ctlDoc.Token.Length > 0)
            {
                viewer.Visible = true;
                upload.Visible = false;
            }
            else
            {
                Response.Write("<H1>" + ctlDoc.InternalError + "</H1><H3>Please send details and sample file to admin@doconut.com</H3>");
                Response.End();
            }
        }
        else
        {
            txtUrl.Text = "Invalid url, start with http:// or https://";
        }
    }
}