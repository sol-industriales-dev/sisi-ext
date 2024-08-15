using System.Web.Mvc;

using DotnetDaddy.DocumentViewer;

// Please make sure you have copied ALL THREE DLLS and Doconut.lic file to the bin folder
// DocumentConfig.dll, DocumentViewer.dll & DocumentFormats.dll
// They are provided in the main Asp.Net zip. Download link provided in trial email

    
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
                FitType = "",
                Zoom = 40,
                FixedZoom = false
                // TimeOut = 10 // use only if you want to auto close document within 10 mins of inactivity
            };

            // if you want to use an older version of jQuery specify this above contructor: 
            // OldJQuery = true

            // Get the required client side script and css

            ViewBag.ViewerScripts = viewer.ReferenceScripts(); // Please make sure you have copied Doconut.lic in bin folder
            ViewBag.ViewerCSS = viewer.ReferenceCss();         // Download link provided in trial email   
            ViewBag.ViewerID = viewer.ClientID;
            ViewBag.ViewerObject = viewer.JsObject;
            
            ViewBag.ViewerInit = viewer.GetAjaxInitArguments(""); // you may also pass empty string


            return View();
        }
        
    }

}