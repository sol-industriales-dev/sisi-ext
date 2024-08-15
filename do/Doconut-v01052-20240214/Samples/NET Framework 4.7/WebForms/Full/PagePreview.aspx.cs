using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotnetDaddy.DocumentViewer;
using DotnetDaddy.DocumentConfig;
using System.Text;

namespace Full
{
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

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (txtUpload.HasFile)
            {
                string fileName = txtUpload.FileName;

                string validFiles = ".DOC .DOCX .PPT .PPTX .PDF .TXT";
                var fileInfo = new System.IO.FileInfo(fileName);
                string fileExtension = fileInfo.Extension.ToUpper();

                if (validFiles.IndexOf(fileExtension) < 0)
                {
                    return;
                }

                string uploadedFile = Server.MapPath("~/files/" + DateTime.Now.Ticks.ToString() + "-" + fileName);
                txtUpload.SaveAs(uploadedFile);

                ProcessDocument(uploadedFile, fileInfo);

            }
            else
            {
                Response.Write("<H1>Please select a file first.</H3>");

                Response.End();

            }
        }

        private void ProcessDocument(string myFile, FileInfo fileInfo)
        {
            using (var ctlDoc = new DocViewer { ID = "ctlDoc" })
            {
                BaseConfig config = null;

                switch (fileInfo.Extension.ToUpper())
                {
                    case ".DWG":
                    case ".DXF":
                        config = new CadConfig { DefaultRender = false, ShowColor = false, WhiteBackground = true, ShowModel = true, ShowLayouts = true, LineWidth = 1, Check3DSolid = false };
                        break;
                    case ".DOC":
                    case ".DOCX":
                    case ".ODT":
                        config = new WordConfig { ConvertPdf = true, }; // turn off, if you don't want search, copy & hyperlinks

                        var pdfConfig = (config as WordConfig).PdfConfig;

                        pdfConfig.DefaultRender = true;
                        pdfConfig.ExtractHyperlinks = true;
                        pdfConfig.AllowCopy = true;        // text copy
                        pdfConfig.HyperlinksPageCount = 0; // check hyperlinks for first 5 pages only; specify 0 for all.

                        break;
                    case ".TXT":
                        var txtEncoding = GetEncoding(myFile);
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
                        config = new PptConfig { ConvertPdf = false }; // specify true if you want copy, search, links features like .DOC format above.
                        break;
                    case ".TIF":
                    case ".TIFF":
                        config = new TifConfig { DefaultRender = true };
                        break;
                    case ".PDF":
                        config = new PdfConfig { DefaultRender = true, ExtractHyperlinks = true, HyperlinksPageCount = 0, AllowCopy = true };
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

                string token = ctlDoc.OpenDocument(myFile, config);

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
    }
}