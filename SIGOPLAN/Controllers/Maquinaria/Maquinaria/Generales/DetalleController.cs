using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Maquinaria.Generales
{
    public class DetalleController : BaseController
    {
        // GET: Detalle
        public ActionResult Index(int? idEconomico)
        {
            if(idEconomico!=0)
            {
                ViewBag.idEconomico = idEconomico;
            }

            return View();
        }
    }
}