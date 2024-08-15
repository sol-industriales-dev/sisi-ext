using System;
using System.IO;
using System.Text;
using System.Web.Mvc;
using System.Configuration;
using Microsoft.Ajax.Utilities;

// MISSING DLL REFERENCES !!!

using DotnetDaddy.DocumentConfig;
using DotnetDaddy.DocumentViewer;


// Please make sure you have copied "ALL THREE DLLS" and "Doconut.lic" file to the bin folder
// DocumentConfig.dll, DocumentViewer.dll & DocumentFormats.dll
// They are provided in the main setup, for asp.net zip file.
// Download link provided in the trial email.


namespace DoconutViewer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var viewer = new DocViewer
            {
                ID = "ctlDoc",
                IncludeJQuery = false,
                IncludeJQueryUI = true, // required
                DebugMode = false,
                BasePath = "/",
                // You will need to change the base path if the MVC project is inside a folder; 
                // eg. BasePath = "TestViewer"; if the Url is such:  http://localhost:xxx/TestViewer/
                FitType = "FitWidth",
                ShowHyperlinks = true,
                TimeOut = 5, // After how many minutes of idleness, does the document free up memory. For WebFarm we want it ASAP.
                IsWebfarm = true // Required only in current WebFarm setup
            };

            // Get the required client side script and css

            ViewBag.ViewerScripts = viewer.ReferenceScripts();   // Please make sure you have copied Doconut.lic in bin folder
            ViewBag.ViewerCSS = viewer.ReferenceCss();           // Download link provided in trial email 
            ViewBag.ViewerID = viewer.ClientID;
            ViewBag.ViewerObject = viewer.JsObject;


            // Get final Init arguments to render the viewer
            ViewBag.ViewerInit = viewer.GetAjaxInitArguments(string.Empty);

            return View();
        }

        private string CheckExportDirectory()
        {
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

            return exportPath;
        }

        [HttpPost]
        public ContentResult UploadFile()
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


        [HttpPost]
        public ContentResult OpenFile(string name)
        {
            if (name.IsNullOrWhiteSpace())
            {
                throw new Exception("File not found");
            }

            var viewer = new DocViewer
            {
                ID = "ctlDoc",
                DebugMode = true,
                TimeOut = 5, // After how many minutes of idleness, does the document free up memory. For WebFarm we want it ASAP.
                IsWebfarm = true // Required only in current WebFarm setup
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
                    config = new WordConfig { ConvertPdf = true, }; // turn off, if you don't want hyperlinks
                    var pdfConfig = (config as WordConfig).PdfConfig;
                    pdfConfig.ExtractHyperlinks = true;
                    pdfConfig.HyperlinksPageCount = 5; // check hyperlinks for first 5 pages only; specify 0 for all.
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
                throw new Exception(viewer.InternalError);
            }


            // Code to load any existing Annotation over a file

            /*
            XmlDocument uploadedXML = new XmlDocument();
            uploadedXML.LoadXml(System.IO.File.ReadAllText(Server.MapPath("~/files/Annotation.xml")));
            viewer.LoadAnnotationXML(uploadedXML);
            */

            var exportPath = CheckExportDirectory();

            if (Directory.Exists(exportPath))
            {
                string exportError = ExportDocumentToPng(viewer, documentPath, exportPath, true);

                if (!exportError.Equals(string.Empty))
                {
                    Response.StatusCode = 500;
                    Response.StatusDescription = exportError;
                    Response.End();
                }

                // Use below, ONLY if you want to use option #2 ExportAnnotationToPdf(exportFilePath, 100);  
                var pdfBytes = viewer.ExportToPdf();
                if (null != pdfBytes)
                {
                    var pathWithToken = Path.Combine(exportPath, token);
                    System.IO.File.WriteAllBytes(Path.Combine(pathWithToken, "Document.pdf"), pdfBytes); // Keep name as it is "Document.pdf"
                }

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
        public ActionResult ExportAnnotations(string token)
        {
            if (token.IsNullOrWhiteSpace())
            {
                throw new Exception("error");
            }

            var documentViewer = GetWebFarmViewerInstance(token);

            try
            {
                var fileName = $"{DateTime.Now.Ticks}-Export.pdf";
                var exportFilePath = Path.Combine(Server.MapPath("~/files"), fileName);

                // #1
                // This exports to a searchable or scalable Pdf                
                // var exportBytes = documentViewer.ExportToPdf();
                // System.IO.File.WriteAllBytes(exportFilePath, exportBytes);

                // #2
                // This exports to a physical PDF
                var pdfExport = documentViewer.ExportAnnotationsToPdf(exportFilePath, 0, true);  // adjust zoom if nativePdf is false

                // #3
                // If you just want the annotated pages as PNG
                // var exportResult = documentViewer.ExportAnnotationsToPng(Path.Combine(Server.MapPath("~/files")) , 100);

                return Content(fileName);
            }
            catch (Exception ex)
            {
                return Content("error : " + ex.Message);
            }
        }

        [HttpPost]
        public ActionResult ExportXml(string token)
        {
            if (token.IsNullOrWhiteSpace())
            {
                throw new Exception("error");
            }

            // Get stored instance from Session

            var document = GetWebFarmViewerInstance(token);

            try
            {
                var annXml = document.GetAnnotationXML();

                var fileName = $"{DateTime.Now.Ticks}-Export.xml";

                annXml.Save(Server.MapPath($"~/files/{fileName}"));

                return Content(fileName);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        private DocViewer GetWebFarmViewerInstance(string token)
        {
            var docViewerWebFarm = new DocViewer();

            var cachePath = CheckExportDirectory();

            if (!docViewerWebFarm.OpenDocumentWebFarm(cachePath, token))
            {
                throw new Exception("error : WebFarm documentViewer is null");
            }

            return docViewerWebFarm;
        }


    }
}