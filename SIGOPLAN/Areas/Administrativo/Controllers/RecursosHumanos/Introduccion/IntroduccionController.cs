using DotnetDaddy.DocumentConfig;
using DotnetDaddy.DocumentViewer;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.RecursosHumanos.Introduccion
{
    public class IntroduccionController : BaseController
    {
        // GET: Administrativo/Introduccion
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CodigoEtica()
        {
            var viewer = new DocViewer
            {
                ID = "ctlDoc",
                IncludeJQuery = false,
                DebugMode = false,
                BasePath = "/",
                FitType = "width",
                FixedZoom = true,
                ShowHyperlinks = true
            };

            ViewBag.ViewerScripts = viewer.ReferenceScripts();
            ViewBag.ViewerCSS = viewer.ReferenceCss();
            ViewBag.ViewerID = viewer.ClientID;
            ViewBag.ViewerObject = viewer.JsObject;


            var token = viewer.OpenDocument("\\\\REPOSITORIO\\Proyecto\\SIGOPLAN\\ETICA\\CODIGO_DE_ETICA_CONSTRUPLAN.pdf");
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new Exception(viewer.InternalError);
            }

            ViewBag.ViewerInit = viewer.GetAjaxInitArguments(token);

            ViewBag.token = token;

            return View();
        }

    }
}