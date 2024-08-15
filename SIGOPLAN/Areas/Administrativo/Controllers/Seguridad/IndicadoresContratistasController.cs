using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.Seguridad
{
    public class IndicadoresContratistasController : Controller
    {
        // GET: Administrativo/IndicadoresContratistas
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AgrupacionCC()
        {
            return View();
        }

        public ViewResult CapturaColaboradores()
        {
            return View();
        }



    }
}