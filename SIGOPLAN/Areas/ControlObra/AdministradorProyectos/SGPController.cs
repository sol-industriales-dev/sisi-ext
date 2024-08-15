using Core.DAO.ControlObra.AdministradorProyectos;
using Core.DTO;
using Data.Factory.ControlObra.AdministradorProyectos;
using Infrastructure.Utils;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.ControlObra.AdministradorProyectos
{
    public class SGPController : BaseController
    {
        #region init
        private ICGPDAO ICGPFS;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ICGPFS = new CGPFactoryService().getCGPService();
            cargarArchivo();
            base.OnActionExecuting(filterContext);
        }
        #endregion
        #region auxiliar
        bool cargarArchivo()
        {
            var vistaID = Request.QueryString["idDoc"].ParseInt();
            var Ruta = ICGPFS.RutaArchivoDeLaVistaId(vistaID);
            Session["RutaVisor"] = Ruta;
            return Ruta.Any();
        }
        #endregion
        #region procedimientos
        public ActionResult Documentos()
        {
            return View();
        }
        #endregion
        #region planes

        #endregion
        #region Formatos

        #endregion
    }
}