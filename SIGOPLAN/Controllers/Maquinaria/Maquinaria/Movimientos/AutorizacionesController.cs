using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Maquinaria.Movimientos
{
    public class AutorizacionesController : BaseController
    {
        // GET: Autorizaciones
        public ActionResult Index()
        {
            ViewBag.pagina = "maquinamovi";
            return View();
        }
    }
}