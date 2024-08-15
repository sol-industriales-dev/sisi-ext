using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Genericos
{
    public class AuthController : BaseController
    {
        // GET: Auth
        public ActionResult _authPanel()
        {
            return PartialView();
        }
        public ActionResult _authFirmas()
        {
            return PartialView();
        }
    }
}