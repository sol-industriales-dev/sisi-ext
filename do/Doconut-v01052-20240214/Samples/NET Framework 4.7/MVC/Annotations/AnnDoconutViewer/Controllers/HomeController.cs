using System;
using System.IO;
using System.Text;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;


// MISSING DLL REFERENCES !!!

using DotnetDaddy.DocumentConfig;
using DotnetDaddy.DocumentViewer;
using System.Xml;

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
		        IncludeJQueryUI = true,
                DebugMode = false,
                BasePath = "/",
                // You will need to change the base path if the MVC project is inside a folder; 
                // eg. BasePath = "TestViewer"; if the Url is such:  http://localhost:xxx/TestViewer/
                FitType = "width",
                ShowHyperlinks = true,
                Zoom = 50,
                TimeOut = 10 // After how many minutes of idleness, does the document free up memory
            };

            // Get the required client side script and css

            ViewBag.ViewerScripts = viewer.ReferenceScripts();   // Please make sure you have copied Doconut.lic in bin folder
            ViewBag.ViewerCSS = viewer.ReferenceCss();           // Download link provided in trial email 
            ViewBag.ViewerID = viewer.ClientID;
            ViewBag.ViewerObject = viewer.JsObject;

            // open default document

            var token = viewer.OpenDocument(Server.MapPath("~/files/Sample.ppt"), new PptConfig { ConvertPdf = true, PdfConfig = new PdfConfig { DefaultRender = true } });

            // Code to load any existing Annotation over a file

            /*
            XmlDocument uploadedXML = new XmlDocument();
            uploadedXML.LoadXml(System.IO.File.ReadAllText(Server.MapPath("~/files/Annotation.xml")));
            viewer.LoadAnnotationXML(uploadedXML);
            */

            if (token.IsNullOrWhiteSpace())
            {
                throw new Exception(viewer.InternalError);
            }

            // Get final Init arguments to render the viewer

            ViewBag.ViewerInit = viewer.GetAjaxInitArguments(token);

            ViewBag.globalToken = token; // Get the first / default token value to the JS variable.


            // You need to store this in session if you want to 
            // call methods like export, annotation export etc on
            // the document

            Session[token] = viewer;

            return View();
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
                return Content("Error : File not found");
            }

            var viewer = new DocViewer
            {
                ID = "ctlDoc",
                IncludeJQuery = false,
                DebugMode = false,
                BasePath = "/",
                // You will need to change the base path if the MVC project is inside a folder; 
                // eg. BasePath = "TestViewer"; if the Url is such:  http://localhost:xxx/TestViewer/
                FitType = "FitWidth",
                ShowHyperlinks = true,
                TimeOut = 10 // After how many minutes of idleness, does the document free up memory
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
                    config = new PptConfig { ConvertPdf = true };
                    break;
                case ".TIF":
                case ".TIFF":
                    config = new TifConfig { DefaultRender = true};
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

            var token = viewer.OpenDocument(Server.MapPath("~/files/" + name), config);

            if (token.IsNullOrWhiteSpace())
            {
                return Content($"Error : {viewer.InternalError}");
            }

            // You need to store this in session if you want to 
            // call methods like export, annotation export etc on
            // the document

            Session[token] = viewer;

            return Content(token);
        }

        [HttpPost]
        public ActionResult ExportAnnotations(string token)
        {
            if (token.IsNullOrWhiteSpace())
            {
                throw new Exception("error");
            }

            // Get stored instance from Session

            if (!(Session[token] is DocViewer document))
            {
                throw new Exception("error");
            }

            try
            {
               // var exportBytes = document.ExportToPdf();

                var fileName = $"{DateTime.Now.Ticks}-Export.pdf";
                var filePath = Server.MapPath("~/files/");

                document.ExportAnnotationsToPdf(Path.Combine(filePath, fileName));

                // System.IO.File.WriteAllBytes(Server.MapPath($"~/files/{fileName}"), exportBytes);

                return Content(fileName);
            }
            catch (Exception ex)
            {
                return Content("error " + ex.Message);
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

            if (!(Session[token] is DocViewer document))
            {
                throw new Exception("error");
            }

            try
            {
                var annXml = document.GetAnnotationXML();

                var fileName = $"{DateTime.Now.Ticks}-Export.xml";

                annXml.Save(Server.MapPath($"~/files/{fileName}"));

                return Content(fileName);
            }
            catch (Exception ex)
            {
                return Content("error " + ex.Message);
            }
        }

    }
}