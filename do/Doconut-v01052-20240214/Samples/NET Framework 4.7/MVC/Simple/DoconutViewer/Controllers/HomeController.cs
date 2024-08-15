using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Web.Mvc;
using System.Web.SessionState; // optional
using Microsoft.Ajax.Utilities;

// MISSING DLL REFERENCES !!!

using DotnetDaddy.DocumentConfig;
using DotnetDaddy.DocumentViewer;

// Please make sure you have copied "ALL THREE DLLS" to the bin folder
// DocumentConfig.dll, DocumentViewer.dll & DocumentFormats.dll
// They are provided in the main setup, for Asp.Net zip file.

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
                IncludeJQueryUI = false,
                IncludeJQuery = false, // Set this false, when using your own jQuery script include
                DebugMode = true,
                BasePath = "/",
                // You will need to change the base path if the MVC project is inside a folder; 
                // eg. BasePath = "TestViewer"; if the Url is such:  http://localhost:xxx/TestViewer/
                FitType = "width",
                FixedZoom = true,
                ShowHyperlinks = true // use this to enable link for doc and pdf
                // TimeOut = 30 // use only if you want to auto close document within 30 mins of inactivity
            };

            // if you want to use an older version of jQuery specify this above contructor: 
            // OldJQuery = true

            // Get the required client side script and css

            ViewBag.ViewerScripts = viewer.ReferenceScripts(); // Please make sure you have copied Doconut.lic in bin folder
            ViewBag.ViewerCSS = viewer.ReferenceCss();         // Download link provided in trial email   
            ViewBag.ViewerID = viewer.ClientID;
            ViewBag.ViewerObject = viewer.JsObject;


            var token = viewer.OpenDocument(Server.MapPath("~/files/Sample.doc"));   // open default document (optional)

            if (token.IsNullOrWhiteSpace())
            {
                throw new Exception(viewer.InternalError);
            }


            // Get final Init arguments to render the viewer

            ViewBag.ViewerInit = viewer.GetAjaxInitArguments(token); // you may also pass an empty string instead of a token

            ViewBag.token = token;      // initiate value for JS token variable.



            // (optional)
            // You need to store this in session if you want to 
            // call methods like export, annotation export etc on
            // the document

            Session[token] = viewer;

            return View();
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
                ID = "ctlDoc",  // ID is important and required.
                DebugMode = true
                // TimeOut = 30 // use only if you want to auto close document within 30 mins of inactivity
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

            // (optional)
            // You need to store this in session if you want to 
            // call methods like export, annotation export etc on
            // the document

            Session[token] = viewer;

            return Content(token);
        }


        public ActionResult Grid()
        {
            // Init the main viewer object.

            var viewer = new DocViewer { ID = "ctlDoc", IncludeJQuery = false, DebugMode = false, BasePath = "/", ResourcePath = "/", FitType = "", Zoom = 40, TimeOut = 20 };

            // Get the required client side script and css

            ViewBag.ViewerScripts = viewer.ReferenceScripts();
            ViewBag.ViewerCSS = viewer.ReferenceCss();
            ViewBag.ViewerID = viewer.ClientID;
            ViewBag.ViewerObject = viewer.JsObject;


            // Get final Init arguments to render the viewer
            ViewBag.ViewerInit = viewer.GetAjaxInitArguments("");


            // Have three sample files in the Grid

            var filesToView = new[]
            {
                new FileInfo(Server.MapPath("~/files/Sample.doc")),
                new FileInfo(Server.MapPath("~/files/Sample.ppt")),
                new FileInfo(Server.MapPath("~/files/Sample.pdf"))
            };

            var fileToken = new Hashtable();
            var fileCount = 1;

            foreach (var file in filesToView)
            {
                var viewerDummy = new DocViewer { ID = "viewerDummy" + fileCount, IncludeJQuery = false, DebugMode = false, BasePath = "/", FitType = "", Zoom = 50 };

                // If only you can to explicitly open each document, before hand.
                var token = viewerDummy.OpenDocument(file.FullName);

                if (!token.IsNullOrWhiteSpace())
                {
                    fileToken.Add(file.Name, token);
                    fileCount++;
                }
            }

            return View(fileToken);
        }

        public ActionResult Modal()
        {
            // Init the main viewer object.

            var viewer = new DocViewer { ID = "ctlDoc", IncludeJQuery = false, DebugMode = false, BasePath = "/", ResourcePath = "/", FitType = "width", TimeOut = 20 };

            // Get the required client side script and css

            ViewBag.ViewerScripts = viewer.ReferenceScripts();
            ViewBag.ViewerCSS = viewer.ReferenceCss();
            ViewBag.ViewerID = viewer.ClientID;
            ViewBag.ViewerObject = viewer.JsObject;


            // Get final Init arguments to render the viewer
            ViewBag.ViewerInit = viewer.GetAjaxInitArguments("");

            return View();
        }

        public ActionResult Print()
        {
            return View();
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

        public FileResult ExportPdf(string token)
        {
            // In "Index" & "OpenFile", comment / uncomment >> Session[token] = viewer;

            if (Session[token] is DocViewer viewer)
            {
                var fileBytes = viewer.ExportToPdf();
                return File(fileBytes, "application/pdf", "Export.pdf");
            }

            return null;
        }
    }

}