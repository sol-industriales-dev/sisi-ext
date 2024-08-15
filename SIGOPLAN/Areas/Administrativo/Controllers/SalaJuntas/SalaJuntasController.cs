using Core.DAO.Principal.Usuarios;
using Core.DTO;
using Core.Entity.Administrativo.SalaJuntas;
using Data.Factory.Administracion.SalaJuntas;
using Data.Factory.Principal.Usuarios;
using DotnetDaddy.DocumentViewer;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.SalaJuntas
{
    public class SalaJuntasController : BaseController
    {
        #region INIT
        private SalaJuntasFactoryService SalaJuntaServices;
        private UsuarioFactoryServices usuarioFactoryServices;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SalaJuntaServices = new SalaJuntasFactoryService();
            usuarioFactoryServices = new UsuarioFactoryServices();
            base.OnActionExecuting(filterContext);
        }
        #endregion

        #region VIEWS
        public ActionResult Calendario()
        {
            return View();
        }
        #endregion

        #region CALENDARIO
        
        #endregion
    }
}