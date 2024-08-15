using System.Web.Mvc;
using System.IO;

using DotnetDaddy.DocumentViewer;
using System;
using System.Configuration;


// Please make sure you have copied ALL THREE DLLS and Doconut.lic file to the bin folder
// DocumentConfig.dll, DocumentViewer.dll & DocumentFormats.dll
// They are provided in the main Asp.Net zip. Download link provided in trial email


namespace DoconutViewer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // Please change the web.config handler from DocImageHandler to DiskImageHandler

            var viewer = new DocViewer
            {
                ID = "ctlDoc",
                IncludeJQuery = true,
                DebugMode = true,
                BasePath = "/",
                // You will need to change the base path if the MVC project is inside a folder; 
                // eg. BasePath = "TestViewer"; if the Url is such:  http://localhost:xxx/TestViewer/
                FitType = "",
                Zoom = 50,
                FixedZoom = true
            };

            // Get the required client side script and css

            ViewBag.ViewerScripts = viewer.ReferenceScripts(); // Please make sure you have copied Doconut.lic in bin folder
            ViewBag.ViewerCSS = viewer.ReferenceCss();         // Download link provided in trial email   
            ViewBag.ViewerID = viewer.ClientID;
            ViewBag.ViewerObject = viewer.JsObject;
            
            ViewBag.ViewerInit = viewer.GetAjaxInitArguments(""); // optional to pass a token. You may keep it blank. Pass "1000-token" here to test.


            return View();
        }

        public ContentResult OpenDocument(string docName)
        {
            // Please change the web.config handler from DiskImageHandler to DocImageHandler

            var viewer = new DocViewer
            {
                ID = "ctlDummy",
                DebugMode = false,
                TimeOut = 20
            };

            var documentPath = Path.Combine(Server.MapPath("~/Documents/"), docName);
            var token = viewer.OpenDocument(documentPath);

            if (token.Length > 0)
            {
                // Following method shows how to export document to png to use as in webfarm, using DiskImageHandler
                /*
                var error = ExportDocumentToPng(viewer, documentPath);
                if (!string.IsNullOrEmpty(error))
                    return Content(error);
                */

                // Following method shows how to generate a DCN

                /*
                var dcnBytes = viewer.SaveDocument();
                var dcnPath = Path.Combine(Server.MapPath("~/Documents/"), docName + ".dcn");
                System.IO.File.WriteAllBytes(dcnPath, dcnBytes);
                */

                return Content(token);
            }
            else
                throw new Exception(viewer.InternalError);
        }

        private string ExportDocumentToPng(DocViewer viewer)
        {
            string exportPath = Server.MapPath(ConfigurationManager.AppSettings["DoconutPngExportPath"]);

            if (Directory.Exists(exportPath))
            {
                // Export document to images/pages. To a shared/network path. 

                string exportError = viewer.ExportToPng(exportPath);

                if (!string.IsNullOrEmpty(exportError))
                {
                    return "Error : " + exportError;
                }
            }

            return string.Empty;
        }

    }

}