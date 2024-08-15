using System;
using System.IO;
using System.Text;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;

// MISSING DLL REFERENCES !!!

using DotnetDaddy.DocumentConfig;
using DotnetDaddy.DocumentViewer;
using System.Threading.Tasks;

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
                DebugMode = true,
                BasePath = "/",
                // You will need to change the base path if the MVC project is inside a folder; 
                // eg. BasePath = "TestViewer"; if the Url is such:  http://localhost:xxx/TestViewer/
                FitType = "width",
                Zoom = 75, // set FitType="" for Zoom to take over.
                TimeOut = 30 // use only if you want to auto close document within 30 mins of inactivity
            };

            // if you want to use an older version of jQuery specify this above contructor: 
            // OldJQuery = true

            // Get the required client side script and css

            ViewBag.ViewerScripts = viewer.ReferenceScripts(); // Please make sure you have copied Doconut.lic in bin folder
            ViewBag.ViewerCSS = viewer.ReferenceCss();         // Download link provided in trial email   
            ViewBag.ViewerID = viewer.ClientID;
            ViewBag.ViewerObject = viewer.JsObject;


            var token = viewer.OpenDocument(Server.MapPath("~/files/Sample.doc"));   // open default document (optional)

            viewer.LoadSearchData(Server.MapPath("~/files/Sample.doc.srh")); // load search file


            if (token.IsNullOrWhiteSpace())
            {
                throw new Exception(viewer.InternalError);
            }


            // Get final Init arguments to render the viewer

            ViewBag.ViewerInit = viewer.GetAjaxInitArguments(token); // you may also pass an empty string

            ViewBag.token = token;          // initiate value for JS token variable.

            return View();
        }

        [HttpPost]
        public async Task<ContentResult> OpenFile(string name)
        {
            if (name.IsNullOrWhiteSpace())
            {
                throw new Exception("File not found");
            }

            var viewer = new DocViewer
            {
                ID = "ctlDoc",  // ID is important and required.
                IncludeJQuery = false,
                DebugMode = false,
                BasePath = "/",
                FitType = "",
                Zoom = 50,
                AutoClose = false
            };

            BaseConfig config = null;

            switch (new FileInfo(name).Extension.ToUpper())
            {
                case ".DWG":
                case ".DXF":
                case ".DGN":
                    config = new CadConfig { DefaultRender = true, ShowColor = false, WhiteBackground = true, ShowModel = false, ShowLayouts = true, LineWidth = 1, Check3DSolid = false };
                    break;
                case ".DOC":
                case ".DOCX":
                case ".RTF":
                    config = new WordConfig { ConvertPdf = true };
                    break;
                case ".TXT":
                    config = new WordConfig { PaperSize = DocPaperSize.A4 };
                    break;
                case ".EML":
                case ".MSG":
                    config = new EmailConfig { EmailEncoding = Encoding.UTF8 };
                    break;
                case ".XLS":
                case ".XLSX":
                case ".ODS":
                case ".CSV":
                    config = new ExcelConfig { SplitWorksheets = true };  // make this false, if you want full view
                    break;
                case ".PDF":
                    config = new PdfConfig { DefaultRender = true, CMapPath = "" };
                    break;

            }

            var token = viewer.OpenDocument(Server.MapPath("~/files/uploads/" + name), config);

            if (token.IsNullOrWhiteSpace())
            {
                throw new Exception(viewer.InternalError);
            }

            // store the viewer in sessin to load the search .srh file later
            // you may also store it in cache.
            Session[token] = viewer;


            /* SEARCH RELATED */

            // Export to DCN
            // Search Serve .EXE will work on this
            // DCN file

            var dcnbytes = viewer.SaveDocument();

            // Create a folder for this DCN first
            Directory.CreateDirectory(Server.MapPath($"~/files/dcn/{token}"));

            // Create DCN file inside this folder

            using (var fs = new FileStream(Server.MapPath($"~/files/dcn/{token}/{token}.dcn"), FileMode.Create))
            {
                await fs.WriteAsync(dcnbytes, 0, dcnbytes.Length);
                fs.Close();
            }

            return Content(token);
        }

        [HttpPost]
        public ContentResult IsSrhDone(string token)
        {
            var srhFile = Server.MapPath($"~/files/srh/{token}.dcn.srh");

            if (System.IO.File.Exists(srhFile))
            {
                // load search file
                (Session[token] as DocViewer).LoadSearchData(srhFile);

                // delete srh file
                try
                {
                    System.IO.File.Delete(srhFile);
                }
                catch (Exception)
                {

                }

                // Clear session
                Session[token] = null;

                return Content("OK");
            }

            return Content("");
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
                    var filePath = Server.MapPath(@"~\files\uploads") + "\\" + fName;

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