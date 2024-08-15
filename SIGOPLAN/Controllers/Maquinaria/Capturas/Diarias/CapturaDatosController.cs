using Core.DTO.Maquinaria.Captura;
using Core.DTO.Maquinaria.Captura.DatosDiarios;
using Core.Entity.Maquinaria.Captura;
using Data.Factory.Maquinaria.Captura;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Maquinaria.Capturas.Diarias
{
    public class CapturaDatosController : BaseController
    {
        DatosDiariosFactoryServices datosDiariosFactoryServices;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            datosDiariosFactoryServices = new DatosDiariosFactoryServices();

            base.OnActionExecuting(filterContext);
        }
        public ActionResult DatosDiarios()
        {
            return View();
        }
        public ActionResult EstatusDiario()
        {
            return View();
        }
        public ActionResult DashboardEstatusDiario()
        {
            return View();
        }
        public ActionResult PermisoBoton()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = datosDiariosFactoryServices.getDatosDiarios().PermisoBoton(getUsuario().id);
                result.Add(ITEMS, lst);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(ITEMS, "OCURRIO ALGUN ERROR EN LA SOLICITUD COMUNIQUESE CON EL DEPARTAMENTO DE TI");
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObtenerAreaCuenta()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = datosDiariosFactoryServices.getDatosDiarios().ObtenerAreaCuenta();
                result.Add(ITEMS, lst);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(ITEMS, "OCURRIO ALGUN ERROR EN LA SOLICITUD COMUNIQUESE CON EL DEPARTAMENTO DE TI");
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObtenerCatMaquinas(datosDiariosDTO parametros)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (parametros.Economico == null)
                {
                    parametros.Economico = "";
                }
                var lst = datosDiariosFactoryServices.getDatosDiarios().ObtenerCatMaquinas(parametros, getEmpresaID());
                result.Add(ITEMS, lst);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(ITEMS, "OCURRIO ALGUN ERROR EN LA SOLICITUD COMUNIQUESE CON EL DEPARTAMENTO DE TI");
                result.Add(SUCCESS, false);
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        public ActionResult ObtenerCapturaDeDatosDiario(datosDiariosDTO parametros)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = datosDiariosFactoryServices.getDatosDiarios().ObtenerCapturaDeDatosDiario(parametros, getEmpresaID());
                result.Add(ITEMS, lst);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(ITEMS, "OCURRIO ALGUN ERROR EN LA SOLICITUD COMUNIQUESE CON EL DEPARTAMENTO DE TI");
                result.Add(SUCCESS, false);
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        public ActionResult CapturarDatosDiaros(string parametros)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<tblM_CapturaDatosDiariosMaquinaria> paramsa = JsonConvert.DeserializeObject<List<tblM_CapturaDatosDiariosMaquinaria>>(parametros);
                var lst = datosDiariosFactoryServices.getDatosDiarios().CapturarDatosDiaros(paramsa);
                result.Add(ITEMS, lst);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(ITEMS, "OCURRIO ALGUN ERROR EN LA SOLICITUD COMUNIQUESE CON EL DEPARTAMENTO DE TI");
                result.Add(SUCCESS, false);
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        public ActionResult GenerarExcel(datosDiariosDTO parametros)
        {
            var result = new Dictionary<string, object>();
            Session["parametros"] = parametros;
            result.Add(SUCCESS, true);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public MemoryStream GenerarExcelDatosDiarios()
        {
            datosDiariosDTO parametros = (datosDiariosDTO)Session["parametros"];
            var stream = datosDiariosFactoryServices.getDatosDiarios().GenerarExcelDatosDiarios(parametros, getEmpresaID());
            if (stream != null)
            {
                this.Response.Clear();
                this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "Captura De Datos Diarios.xlsx"));
                this.Response.BinaryWrite(stream.ToArray());
                this.Response.End();

                Session["parametros"] = null;
                return stream;
            }
            else
            {
                return null;
            }
        }

        public ActionResult EnviarCorreos(datosDiariosDTO parametros)
        {
            var result = new Dictionary<string, object>();
            var stream = datosDiariosFactoryServices.getDatosDiarios().GenerarExcelDatosDiariosEnviandocorreo(parametros, getEmpresaID());
            result.Add(SUCCESS, true);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerBotonEnviarExcel(DateTime Fecha)
        {
            var result = new Dictionary<string, object>();
            var lst = datosDiariosFactoryServices.getDatosDiarios().ObtenerBotonEnviarExcel(Fecha);
            result.Add(ITEMS, lst);
            result.Add(SUCCESS, lst);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerGrupo()
        {
            var result = new Dictionary<string, object>();
            var lst = datosDiariosFactoryServices.getDatosDiarios().ObtenerGrupo();
            result.Add(ITEMS, lst);
            result.Add(SUCCESS, lst);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObtenerModelo(int idGrupo)
        {
            var result = new Dictionary<string, object>();
            var lst = datosDiariosFactoryServices.getDatosDiarios().ObtenerModelo(idGrupo);
            result.Add(ITEMS, lst);
            result.Add(SUCCESS, lst);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult guardar_Estatus_Diario(tblM_CatMaquina_EstatusDiario obj, List<tblM_CatMaquina_EstatusDiario_Det> det)
        {
            var result = new Dictionary<string, object>();
            try
            {
                datosDiariosFactoryServices.getDatosDiarios().guardar_Estatus_Diario(obj,det);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(ITEMS, "OCURRIO ALGUN ERROR EN LA SOLICITUD COMUNIQUESE CON EL DEPARTAMENTO DE TI");
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getEstatus_Diario(DateTime fecha, string cc)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = datosDiariosFactoryServices.getDatosDiarios().getEstatus_Diario(fecha, cc);
                result.Add("data", lst);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(ITEMS, "OCURRIO ALGUN ERROR EN LA SOLICITUD COMUNIQUESE CON EL DEPARTAMENTO DE TI");
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult saveCapturarDatosDiaros(tblM_CatMaquina_EstatusDiario obj, List<tblM_CatMaquina_EstatusDiario_Det> det)
        {
            var result = new Dictionary<string, object>();
            try
            {
   
                datosDiariosFactoryServices.getDatosDiarios().saveCapturarDatosDiaros(obj, det);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(ITEMS, "OCURRIO ALGUN ERROR EN LA SOLICITUD COMUNIQUESE CON EL DEPARTAMENTO DE TI");
                result.Add(SUCCESS, false);
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult CargarGraficasDashboard(List<string> listaAreaCuenta)
        {
            return Json(datosDiariosFactoryServices.getDatosDiarios().CargarGraficasDashboard(listaAreaCuenta), JsonRequestBehavior.AllowGet);
        }
    }
}
