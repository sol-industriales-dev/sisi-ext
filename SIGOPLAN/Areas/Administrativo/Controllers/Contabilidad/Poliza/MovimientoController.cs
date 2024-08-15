using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.Contabilidad.Poliza
{
    public class MovimientoController : Controller
    {
        // GET: Administrativo/Movimiento
        public ActionResult tipoMovimiento()
        {
            return View();
        }
    }
}