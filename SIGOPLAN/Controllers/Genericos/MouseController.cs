using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Genericos
{
    public class MouseController : Controller
    {
        // GET: Mouse
        public ActionResult _menuClick()
        {
            return View();
        }
    }
}