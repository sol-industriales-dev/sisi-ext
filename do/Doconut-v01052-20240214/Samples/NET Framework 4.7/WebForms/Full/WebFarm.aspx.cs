using DotnetDaddy.DocumentConfig;
using DotnetDaddy.DocumentViewer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Full
{
    public partial class WebFarm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // BEFORE USING THIS SAMPLE PLEASE VISIT WEB.CONFIG AND READ COMMENTS FOR DiskImageHandler and DoconutPngExportPath

            if (!Page.IsPostBack)
            {
                var ctlDoc = new DocViewer();

                ctlDoc.ID = "ctlDoc";

                ctlDoc.LargeDoc = true;
                ctlDoc.ImageResolution = 200;
                ctlDoc.AutoScrollThumbs = true;
                ctlDoc.Zoom = 75;
                ctlDoc.IncludeJQuery = false;
                ctlDoc.WatermarkInfo = "^Sample Copy~Gray~24~Arial~80~-45";

                ctlDoc.TimeOut = 1; // close after one minute of non-use

                string fileName = "Sample.ppt";
 
                var config = GetBaseConfig(fileName);

                string token = ctlDoc.OpenDocument(Server.MapPath("~/files/Sample.ppt"), config);

                // for pre-loaded documents just provide the token string value
                // example:  string token = "5e645443-202a-44c9-b8a2-359829905e49"; 

                if (token.Length > 0 && ctlDoc.InternalError.Equals(string.Empty))
                {
                    // You may avoid Server.MapPath when using network or full path

                    if (null == ConfigurationManager.AppSettings["DoconutPngExportPath"])
                    {
                        Response.Write("Please provide appSetting value for [DoconutPngExportPath] in web.config <br/>Also make sure you are using DiskImageHandler.");
                        Response.End();
                    }

                    string exportPath = Server.MapPath(Convert.ToString(ConfigurationManager.AppSettings["DoconutPngExportPath"]));

                    if (!Directory.Exists(exportPath))
                    {
                        Directory.CreateDirectory(exportPath); // Ideally you should have this created beforehand
                    }

                    string exportError = ctlDoc.ExportToPng(exportPath);


                    if (exportError.Equals(string.Empty))
                    {
                        string scriptJs = " $(document).ready(function () { setTimeout(function(){ " + ctlDoc.GetAjaxInitArguments(token) + "; $('#loading').hide(); }, 5000); });";
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "docView", scriptJs, true);
                    }
                    else
                    {
                        Response.Write(exportError);
                        Response.End();
                    }
                }
                else
                {
                    Response.Write(ctlDoc.InternalError);
                    Response.End();
                }
            }
        }

        public BaseConfig GetBaseConfig(string fileName, string uploadedFile = "")
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