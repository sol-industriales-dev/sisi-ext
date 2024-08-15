using System;
using System.IO;
using System.Text;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;

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
            };


            // Get the required client side script and css


            ViewBag.ViewerScripts = viewer.ReferenceScripts(); // Please make sure you have copied Doconut.lic in bin folder
            ViewBag.ViewerCSS = viewer.ReferenceCss();         // Download link provided in trial email   
            ViewBag.ViewerID = viewer.ClientID;
            ViewBag.ViewerObject = viewer.JsObject;

            // open default document (optional)

            var documentPath = Server.MapPath("~/files/Sample.pdf");
            var token = viewer.OpenDocument(documentPath, new PdfConfig { AllowSearch = true, AllowCopy = true, ExtractHyperlinks = true });

            if (token.IsNullOrWhiteSpace())
            {
                throw new Exception(viewer.InternalError);
            }

            // Get final Init arguments to render the viewer
            ViewBag.ViewerInit = viewer.GetAjaxInitArguments(token); // you may also pass an empty string

            ViewBag.token = token;      // initiate value for JS token variable.

            return View();
        }

        [HttpPost]
        public ContentResult OpenFile(string name)
        {
            if (name.IsNullOrWhiteSpace())
            {
                throw new Exception("File name is empty");
            }

            var viewer = new DocViewer
            {
                ID = "ctlDoc" + DateTime.Now.Ticks.ToString(),  // ID is important and required.
                ShowHyperlinks = true,
                DebugMode = true,
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
                    config = new WordConfig { ConvertPdf = true, PdfConfig = new PdfConfig { ExtractHyperlinks = true, HyperlinksPageCount = 5, AllowSearch = true, AllowCopy = true } };
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
                    config = new PptConfig { ConvertPdf = true, PdfConfig = new PdfConfig { ExtractHyperlinks = true, AllowSearch = true, AllowCopy = true } };
                    break;
                case ".PDF":
                    config = new PdfConfig { DefaultRender = true, ExtractHyperlinks = true, HyperlinksPageCount = 5, AllowSearch = true, AllowCopy = true };
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