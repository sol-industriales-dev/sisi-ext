using Core.DAO.Principal.Usuarios;
using Core.DAO.RecursosHumanos.Captura;
using Core.DTO;
using Core.Entity.RecursosHumanos.Captura;
using Core.Enum;
using Data.Factory.Principal.Usuarios;
using Data.Factory.RecursosHumanos.Captura;
using Infrastructure.Utils;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.RecursosHumanos.PlantillaPersonal
{
    public class PlantillaPersonalController : BaseController
    {
        IPlantillaPersonalDAO plantillaPersonalFS;
        IUsuarioDAO usuarioFS;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            plantillaPersonalFS = new PlantillaPersonalFactoryService().getPlantillaPersonalFactoryService();
            usuarioFS = new UsuarioFactoryServices().getUsuarioService();
            base.OnActionExecuting(filterContext);
        }
        public ActionResult Captura()
        {
            return View();
        }
        public ActionResult Gestion()
        {
            return View();
        }
        public ActionResult Consulta()
        {
            return View();
        
        }
        public ActionResult GuardarPlantilla(tblRH_PP_PlantillaPersonal plantilla, List<tblRH_PP_PlantillaPersonal_Det> dets, List<tblRH_PP_PlantillaPersonal_Aut> auts)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var resultData = plantillaPersonalFS.GuardarPlantilla(plantilla, dets, auts);

                result.Add("folio", resultData.ToString().PadLeft(6,'0'));
                result.Add("plantillaID", resultData);
                result.Add(SUCCESS, (resultData>0?true:false));
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPlantillas(string cc,int estatus)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var resultData = plantillaPersonalFS.GetPlantillas(cc,estatus);
                var data = new List<dynamic>();
                foreach (var x in resultData)
                {
                    data.Add(new
                    {
                        id = x.id,
                        cc = x.cc,
                        fechaInicio = (x.fechaInicio == null ? "" : x.fechaInicio.ToShortDateString()),
                        fechaFin = (x.fechaFin == null ? "" : x.fechaFin.Value.ToShortDateString()),
                        fechaRegistro = (x.fechaMod == null ? "" : x.fechaMod.ToShortDateString()),
                        autorizar = "<button class='btn btn-primary clsDetalle' data-id='" + x.id + "'>Detalle</button>",
                        reporte = "<button class='btn btn-primary clsReporte' data-id='" + x.id + "'><i class='glyphicon glyphicon-print'></i></button>",
                    });    
                }

                result.Add("dataMain", data);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPlantillasEK(string cc)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var resultData = plantillaPersonalFS.GetPlantillaEK(cc);

                result.Add("dataMain", resultData);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAutorizadores(int plantillaID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var resultData = plantillaPersonalFS.GetAutorizadores(plantillaID);
                var data = new List<dynamic>();
                foreach (var x in resultData)
	            {

                    var estatus = vSesiones.sesionUsuarioDTO.id==x.aprobadorClave?
                                  (
                                    x.autorizando?
                                    "<button class='btn btn-success clsAutorizar' data-id='" + x.id + "'><i class='glyphicon glyphicon-ok'></i></button><button class='btn btn-danger clsRechazar' data-id='" + x.id + "'><i class='glyphicon glyphicon-remove'></i></button>" :
                                    EnumExtensions.GetDescription((EstatusRegEnum)x.estatus)
                                  ):
                                  EnumExtensions.GetDescription((EstatusRegEnum)x.estatus);
		            data.Add(new
                    {
                        id = x.id,
                        nombre = x.aprobadorNombre,
                        tipo = x.tipo,
                        puesto = x.aprobadorPuesto,
                        estatus = estatus,
                        firma = x.firma ?? "S/F",
                        capturo = x.plantilla.usuario.nombre + " " + x.plantilla.usuario.apellidoPaterno + " " + x.plantilla.usuario.apellidoMaterno
                    });
	            }
                result.Add("dataMain", data);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboDepartamentos(string cc)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var resultData = plantillaPersonalFS.GetDepartamentos(cc);

                result.Add(ITEMS, resultData);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboPuestos()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var resultData = plantillaPersonalFS.GetPuestos().OrderBy(x=>x.Text);

                result.Add(ITEMS, resultData);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTipoNomina()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var resultData = plantillaPersonalFS.GetTipoNomina();

                result.Add(ITEMS, resultData);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AutorizarPlantilla(int plantillaID, int autorizacion, int estatus,string comentario)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var resultData = plantillaPersonalFS.AutorizarPlantilla(plantillaID,autorizacion,estatus);

                result.Add(SUCCESS, resultData);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboCC(bool plantilla)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var resultData = plantillaPersonalFS.FillComboCC(plantilla).OrderBy(x=>x.Value);

                result.Add(ITEMS, resultData);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EnviarCorreo(int plantillaID, int autorizacion,int estatus)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var resultData = plantillaPersonalFS.EnviarCorreo(plantillaID, autorizacion, estatus);

                result.Add("enviado", resultData);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}