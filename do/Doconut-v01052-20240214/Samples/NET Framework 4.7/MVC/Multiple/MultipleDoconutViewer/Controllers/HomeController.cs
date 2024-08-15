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
// They are provided in the main setup, for asp.net zip file.

// Download links provided in the trial email.


namespace DoconutViewer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // #1 THE MAIN VIEWER
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

            // #2 THE MODAL VIEWER
            var viewerModal = new DocViewer
            {
                ID = "ctlModalDoc",
                IncludeJQueryUI = false,
                IncludeJQuery = false,
                DebugMode = true,
                BasePath = "/",
                FitType = "width",
                FixedZoom = true,
                ShowHyperlinks = true
                // TimeOut = 30 
            };


            // if you want to use an older version of jQuery specify this above contructor: 
            // OldJQuery = true

            // Get the required client side script and css

            ViewBag.ViewerScripts = viewer.ReferenceScripts(); // Please make sure you have copied Doconut.lic in bin folder
            ViewBag.ViewerCSS = viewer.ReferenceCss();         // Download link provided in trial email
                                                               
            // MAIN VIEWER
            ViewBag.ViewerID = viewer.ClientID;
            ViewBag.ViewerObject = viewer.JsObject;

            // MODAL VIEWER
            ViewBag.ModalViewerID = viewerModal.ClientID;
            ViewBag.ModalViewerObject = viewerModal.JsObject;


            var token = viewer.OpenDocument(Server.MapPath("~/files/Sample.doc"));   // open default document (optional)

            if (token.IsNullOrWhiteSpace())
            {
                throw new Exception(viewer.InternalError);
            }

            // Get final Init arguments to render the viewer

            // MAIN VIEWER
            ViewBag.ViewerInit = viewer.GetAjaxInitArguments(token);
            ViewBag.token = token;      // initiate value for JS token variable.


            // MODAL VIEWER
            ViewBag.ModalViewerInit = viewerModal.GetAjaxInitArguments(string.Empty); 


            // (optional)
            // You need to store this in session if you want to 
            // call methods like export, annotation export etc on
            // the document

            // Session[token] = viewer;

            return View();
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
                ID = "ctlDummyDoc",  // ID is important and required.
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
                throw new Exception(viewer.InternalError);
            }

            // (optional)
            // You need to store this in session if you want to 
            // call methods like export, annotation export etc on
            // the document

            // Session[token] = viewer;

            return Content(token);
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