using Newtonsoft.Json;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Converters;
using Core.Entity.Administrativo.AgendarJunta;
using Core.Entity.Administrativo.SalaJuntas;
using Data.Factory.Administracion.AgendarJunta;
using Data.Factory.Administracion.SalaJuntas;
using Core.DAO.Administracion.AgendarJunta;
using Core.DAO.Principal.Usuarios;
using Data.Factory.Principal.Usuarios;
using Core.DTO.Principal.Generales;
using Data.DAO.Administracion.AgendarJunta;
using Core.DTO.Administracion.AgendarJunta;
using Core.DTO.Administracion.SalaJuntas;

namespace SIGOPLAN.Areas.Administrativo.Controllers.AgendarJunta
{
    public class AgendarJuntaController : BaseController
    {
        #region INIT
        IUsuarioDAO usuarioService;
        public AgendarJuntaFactoryService _AgendarJuntaFS;
        public SalaJuntasFactoryService SalasFactoryService;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            usuarioService = new UsuarioFactoryServices().getUsuarioService();
            _AgendarJuntaFS = new AgendarJuntaFactoryService();
            SalasFactoryService = new SalaJuntasFactoryService();
            base.OnActionExecuting(filterContext);
        }
        #endregion

        #region RETURN VIEWS
        public ActionResult Calendario()
        {
            bool tieneAcceso = _AgendarJuntaFS.getJuntaService().VerificarAcceso();
            if (tieneAcceso)
                return View();
            else
                return View("ErrorPermisoVista");
        }

        public ActionResult Facultamientos()
        {
            return View();
        }
        #endregion

        #region SALA JUNTAS
        public ActionResult GetSalaJuntas(SalaJuntasDTO objParamsDTO)
        {
            return Json(_AgendarJuntaFS.getJuntaService().GetSalaJuntas(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CESalaJuntas(SalaJuntasDTO objParamsDTO)
        {
            return Json(_AgendarJuntaFS.getJuntaService().CESalaJuntas(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosActualizarSalaJuntas(SalaJuntasDTO objParamsDTO)
        {
            return Json(_AgendarJuntaFS.getJuntaService().GetDatosActualizarSalaJuntas(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarSalaJuntas(SalaJuntasDTO objParamsDTO)
        {
            return Json(_AgendarJuntaFS.getJuntaService().EliminarSalaJuntas(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboCatEdificios()
        {
            return Json(_AgendarJuntaFS.getJuntaService().FillCboCatEdificios(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboCatSalas(SalaJuntasDTO objParamsDTO)
        {
            return Json(_AgendarJuntaFS.getJuntaService().FillCboCatSalas(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillSalas(SalaJuntasDTO objParamsDTO)
        {
            return Json(_AgendarJuntaFS.getJuntaService().FillSalas(objParamsDTO), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region FACULTAMIENTOS
        public ActionResult GetFacultamientos()
        {
            return Json(_AgendarJuntaFS.getJuntaService().GetFacultamientos(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearFacultamiento(FacultamientoDTO objParamsDTO)
        {
            return Json(_AgendarJuntaFS.getJuntaService().CrearFacultamiento(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActualizarFacultamiento(FacultamientoDTO objParamsDTO)
        {
            return Json(_AgendarJuntaFS.getJuntaService().ActualizarFacultamiento(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarFacultamiento(FacultamientoDTO objParamsDTO)
        {
            return Json(_AgendarJuntaFS.getJuntaService().EliminarFacultamiento(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboUsuarios()
        {
            return Json(_AgendarJuntaFS.getJuntaService().FillCboUsuarios(), JsonRequestBehavior.AllowGet); 
        }

        public ActionResult FillCboTipoFacultamientos()
        {
            return Json(_AgendarJuntaFS.getJuntaService().FillCboTipoFacultamientos(), JsonRequestBehavior.AllowGet); 
        }

        public ActionResult GetDatosActualizarFacultamiento(FacultamientoDTO objParamsDTO)
        {
            return Json(_AgendarJuntaFS.getJuntaService().GetDatosActualizarFacultamiento(objParamsDTO), JsonRequestBehavior.AllowGet); 
        }
        #endregion
    }
}