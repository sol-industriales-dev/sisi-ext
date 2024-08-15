using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Maquinaria.Movimientos
{
    public class TransitoController : BaseController
    {
        // GET: Transito
        public ActionResult ControlMaquinaria()
        {
            ViewBag.pagina = "maquinamovi";
            return View();
        }
    }
}