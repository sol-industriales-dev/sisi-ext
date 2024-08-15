using System;
using System.IO;
using System.Text;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using System.Configuration;

// MISSING DLL REFERENCES !!!

using DotnetDaddy.DocumentConfig;
using DotnetDaddy.DocumentViewer;

// Please make sure you have copied "ALL THREE DLLS" to the bin folder
// DocumentConfig.dll, DocumentViewer.dll & DocumentFormats.dll
// They are provided in the main setup, for asp.net zip file.

// Download link provided in the trial email.


namespace DoconutViewer.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {

        }

        public ActionResult Index()
        {
            // Load the path for export. When using shared network path adjust the code accordingly
            // by removing Server.MapPath

            string exportPath = Server.MapPath(ConfigurationManager.AppSettings["DoconutPngExportPath"]);


            var viewer = new DocViewer
            {
                ID = "ctlDoc",
                IncludeJQueryUI = false,
                IncludeJQuery = false, // Set this false, when using your own jQuery script include
                DebugMode = true,
                BasePath = "/",
                // You will need to change the base path if the MVC project is inside a folder; 
                // eg. BasePath = "TestViewer"; if the Url is such:  http://localhost:xxx/TestViewer/
                FitType = "width",
                FixedZoom = true,
                ShowHyperlinks = true, // use this to enable link for doc and pdf
                TimeOut = 5, // As it is a webfarm we want to close it soon.
                IsWebfarm = true
            };


            // Get the required client side script and css


            ViewBag.ViewerScripts = viewer.ReferenceScripts(); // Please make sure you have copied Doconut.lic in bin folder
            ViewBag.ViewerCSS = viewer.ReferenceCss();         // Download link provided in trial email   
            ViewBag.ViewerID = viewer.ClientID;
            ViewBag.ViewerObject = viewer.JsObject;

            // open default document (optional)

            var documentPath = Server.MapPath("~/files/Sample.doc");
            var token = viewer.OpenDocument(documentPath);

            if (token.IsNullOrWhiteSpace())
            {
                throw new Exception(viewer.InternalError);
            }

            if (Directory.Exists(exportPath))
            {
                // Export document to images/pages. To a shared/network path. 
                // Every webfarm instance/process will look here

                string exportError = ExportDocumentToPng(viewer, documentPath, exportPath, false);

                if (!exportError.Equals(string.Empty))
                {
                    Response.Write(exportError);
                    Response.End();
                }

                // Get final Init arguments to render the viewer
                ViewBag.ViewerInit = viewer.GetAjaxInitArguments(token); // you may also pass an empty string

                ViewBag.token = token;      // initiate value for JS token variable.

            }
            else
            {
                Response.Write("Please provide appSetting value for [DoconutPngExportPath] in web.config <br/>Also make sure you are using DiskImageHandler.");
                Response.End();
            }

            return View();
        }

        [HttpPost]
        public ContentResult OpenFile(string name)
        {
            if (name.IsNullOrWhiteSpace())
            {
                throw new Exception("File name is empty");
            }

            // Load the path for export. When using shared network path adjust the code accordingly
            // by removing Server.MapPath

            string exportPath = Server.MapPath(ConfigurationManager.AppSettings["DoconutPngExportPath"]);


            var viewer = new DocViewer
            {
                ID = "ctlDoc" + DateTime.Now.Ticks.ToString(),  // ID is important and required.
                ShowHyperlinks = true,
                DebugMode = true,
                TimeOut = 5         // use only if you want to auto close document within X mins of inactivity
            };

            BaseConfig config = null;

            switch (new FileInfo(name).Extension.ToUpper())
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
                    config = new WordConfig { ConvertPdf = true, PdfConfig = new PdfConfig { ExtractHyperlinks = true, HyperlinksPageCount = 5 } }; // turn it off, if you don't want hyperlinks
                    break;
                case ".TXT":
                    config = new WordConfig { PaperSize = DocPaperSize.A4, FileEncoding = Encoding.UTF8 };
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
                    config = new PptConfig();
                    break;
                case ".PDF":
                    config = new PdfConfig { DefaultRender = true, ExtractHyperlinks = true, HyperlinksPageCount = 5 }; // specify true if you need hyperlinks
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

            var documentPath = Server.MapPath("~/files/" + name);
            var token = viewer.OpenDocument(documentPath, config);

            if (token.IsNullOrWhiteSpace())
            {
                Response.StatusCode = 500;
                Response.StatusDescription = viewer.InternalError;
                Response.End();
            }

            // Get the export path

            if (Directory.Exists(exportPath))
            {
                string exportError = ExportDocumentToPng(viewer, documentPath, exportPath, true);

                if (!exportError.Equals(string.Empty))
                {
                    Response.StatusCode = 500;
                    Response.StatusDescription = exportError;
                    Response.End();
                }
            }
            else
            {
                throw new Exception("Please provide appSetting value for [DoconutPngExportPath] in web.config <br/>Also make sure you are using DiskImageHandler.");
            }

            return Content(token);
        }

        private string ExportDocumentToPng(DocViewer docViewer, string documentPath, string exportPath, bool multipleInstances)
        {
            var totalPages = docViewer.TotalPages;

            if (totalPages <= 0)
                return "Invalid document pages";

            docViewer.ExportMetadata(exportPath);

            if (!multipleInstances || totalPages <= 5)
            {
                return docViewer.ExportToPng(exportPath);
            }
            else
            {
                var token = docViewer.Token;

                int extraInstances = 3; // adjust as required
                int pagePerInstance = totalPages / (extraInstances + 1);

                int startPage = 1;
                int endPage = pagePerInstance;

                string exportError = docViewer.ExportToPng(exportPath, token, startPage, endPage, false);

                if (exportError.Length > 0)
                    return exportError;

                for (var instCount = 1; instCount <= extraInstances; instCount++)
                {
                    startPage = endPage + 1;
                    if (instCount == extraInstances)
                    {
                        endPage = totalPages;
                    }
                    else
                    {
                        endPage = endPage + pagePerInstance;
                    }

                    var ctlDocInstance = new DocViewer { TimeOut = 5 }; // adjust timeout as required

                    ctlDocInstance.OpenDocument(documentPath);
                    exportError += ctlDocInstance.ExportToPng(exportPath, token, startPage, endPage, false);

                }

                return exportError;
            }
        }

        [HttpPost]
        public ActionResult UploadFile()
        {
            var isSavedSuccessfully = true;
            var fName = "";

            try
            {
                foreach (string fileName in Request.Files)
                {
                    var file = Request.Files[fileName];

                    if (null == file)
                        continue;

                    if (file.ContentLength <= 0) continue;

                    // check for any malicious file types
                    var invalidFiles = ".EXE .JS .JAR .VBS .VB .SFX .BAT .DLL .TMP .PY .ASP .ASPX .ASHX .ASMX .AXD .PHP .MSI .COM .CMD .VBE .LNK .ZIP .RAR .7Z";
                    var fileExtension = new FileInfo(file.FileName).Extension.ToUpper();

                    if (invalidFiles.IndexOf(fileExtension, StringComparison.Ordinal) > -1)
                    {
                        throw new Exception("Invalid file extension");
                    }


                    fName = DateTime.Now.ToShortDateString().Replace("/", "-") + "--" + file.FileName;

                    foreach (char c in Path.GetInvalidFileNameChars())
                    {
                        fName = fName.Replace(c, '-');
                    }

                    // Remove special characters
                    fName = fName.Replace("%", "-").Replace("&", "-").Replace("#", "-").Replace(";", "-").Replace("+", "-").Replace(" ", "-");
                    var filePath = Server.MapPath(@"~\files") + "\\" + fName;

                    file.SaveAs(filePath);
                }

            }
            catch (Exception)
            {
                isSavedSuccessfully = false;
            }

            return Content(isSavedSuccessfully ? fName : "");
        }

    }

}