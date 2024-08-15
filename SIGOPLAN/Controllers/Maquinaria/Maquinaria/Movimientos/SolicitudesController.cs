using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Maquinaria.Movimientos
{
    public class SolicitudesController : BaseController
    {
        // GET: Solicitudes
        public ActionResult Index()
        {
            ViewBag.pagina = "maquinamovi";

            return View();
        }
    }
}