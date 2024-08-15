using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Maquinaria
{
    public class Maq_PreAsignaController : BaseController
    {
        // GET: Maq_PreAsigna
        public ActionResult Index()
        {
            ViewBag.pagina = "maquinamovi";
            return View();
        }
    }
}