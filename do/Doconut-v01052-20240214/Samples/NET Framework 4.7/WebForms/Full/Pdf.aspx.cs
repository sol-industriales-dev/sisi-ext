using DotnetDaddy.DocumentConfig;
using DotnetDaddy.DocumentViewer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Full
{
    public partial class Pdf : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void LaunchAsPDF(string myFile, string uploadedFile)
        {
            using (DocViewer ctlDoc = new DocViewer())
            {
                var config = GetBaseConfig(myFile, uploadedFile);

                ctlDoc.OpenDocument(Server.MapPath($"~/files/{myFile}"), config);

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

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (txtUpload.HasFile)
            {
                string fileName = txtUpload.FileName;

                string uploadedFile = Server.MapPath("~/files/" + DateTime.Now.Ticks.ToString() + "-" + fileName);
                txtUpload.SaveAs(uploadedFile);

                LaunchAsPDF(fileName, uploadedFile);
            }
            else
            {
                Response.Write("<H1>Please select a file first.</H3>");

                Response.End();

            }
        }

        public BaseConfig GetBaseConfig(string fileName, string uploadedFile)
        {
            BaseConfig config = null;
            var fileInfo = new System.IO.FileInfo(fileName);


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

            return config;
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