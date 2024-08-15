using Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers
{
    public class IndexController : Controller
    {
        // GET: Administrativo/Index
        public ActionResult Index(int id)
        {
            var usuarioDTO = vSesiones.sesionUsuarioDTO;

            if (usuarioDTO != null)
            {
                vSesiones.sesionSistemaActual = id;

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Usuario");
            }
        }
    }
}