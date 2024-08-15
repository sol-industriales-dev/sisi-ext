using Core.DAO.RecursosHumanos.Bajas;
using Core.DAO.RecursosHumanos.BajasPersonal;
using Core.DAO.RecursosHumanos.Captura;
using Core.DTO.RecursosHumanos.BajasPersonal;
using Data.Factory.RecursosHumanos;
using Data.Factory.RecursosHumanos.BajasPersonal;
using Data.Factory.RecursosHumanos.Captura;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.RecursosHumanos.Bajas
{
    public class BajasPersonalEntrevistaController : BaseController
    {

        private IBajasPersonalEntrevistaDAO r_BajasPersonalEntrevistaService = new BajasPersonalEntrevistaFactoryService().GetBajasPersonalEntrevistaService();
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            r_BajasPersonalEntrevistaService = new BajasPersonalEntrevistaFactoryService().GetBajasPersonalEntrevistaService();
            base.OnActionExecuting(filterContext);
        }


        #region VISTAS

        public ActionResult Entrevista()
        {
            return View();
        }

      
        #endregion 

        #region ENTREVISTA

        public ActionResult GetCapturada(int idRegistro, int empresa)
        {
            return Json(r_BajasPersonalEntrevistaService.GetCapturada(idRegistro, empresa),JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBaja(int id, int empresa)
        {
            return Json(r_BajasPersonalEntrevistaService.GetBaja(id, empresa), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarEntrevista(BajaPersonalDTO objDTO)
        {
   
            return Json(r_BajasPersonalEntrevistaService.CrearEditarEntrevista(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosPersona(int claveEmpleado, string nombre, int empresa)
        {
            return Json(r_BajasPersonalEntrevistaService.GetDatosPersona(claveEmpleado, nombre, empresa),JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboPreguntas(int idPregunta)
        {
            return Json(r_BajasPersonalEntrevistaService.FillCboPreguntas(idPregunta), JsonRequestBehavior.AllowGet);
        }
        public ActionResult getEmpleadosGeneral(string term, int empresa)
        {
            //var items = capturaFormatoCambioFS.getCatEmpleadosGeneral(term);
            var items = r_BajasPersonalEntrevistaService.getCatEmpleadosGeneral(term, empresa);
            var filteredItems = items.Select(x => new { id = x.clave_empleado, label = x.Nombre });
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region FILL COMBOS
        public ActionResult FillCboEstados()
        {
            return Json(r_BajasPersonalEntrevistaService.FillCboEstados(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboMunicipios(int idEstado)
        {
            return Json(r_BajasPersonalEntrevistaService.FillCboMunicipios(idEstado), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboEstadosCiviles()
        {
            return Json(r_BajasPersonalEntrevistaService.FillCboEstadosCiviles(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboEscolaridades()
        {
            return Json(r_BajasPersonalEntrevistaService.FillCboEscolaridades(), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}