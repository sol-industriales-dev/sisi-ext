using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Principal
{
    public class MenuController : BaseController
    {
        // GET: Menu
        public ActionResult Index()
        {
            Session["tipoMenu"] = "subMenu";
            return View();
        }
        public ActionResult Vacio()
        {
            Session["tipoMenu"] = "otro";
            return View();
        }

    }
}