using Core.DAO.Enkontrol.Almacen;
using Core.DTO;
using Core.DTO.Enkontrol.Alamcen;
using Data.Factory.Enkontrol.Resguardo;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Enkontrol.Controllers.Resguardo
{
    public class ResguardoController : BaseController
    {
        IResguardoDAO resService;
        Dictionary<string, object> result;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            resService = new ResguardoFactoryService().getResguardoService();
            base.OnActionExecuting(filterContext);
        }

        public ActionResult Asignacion()
        {
            return View();
        }

        public ActionResult ReporteBitacoraResguardos()
        {
            return View();
        }

        [HttpPost]
        public JsonResult cambiarCCoEmpleado(int numEmpleado, int claveEmpleado, string ccNuevo, List<ResguardoEKDTO> resguardos)
        {
            return Json(resService.cambiarCCoEmpleado(numEmpleado, claveEmpleado, ccNuevo, resguardos));
        }

        public ActionResult GuardarAsignacion(List<ResguardoEKDTO> resguardos)
        {
            return Json(resService.guardarAsignacionNormal(resguardos), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetResguardo(string cc, int almacen, int folio)
        {
            return Json(resService.GetResguardo(cc, almacen, folio), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarDevolucion(List<ResguardoEKDTO> resguardos)
        {
            return Json(resService.guardarDevolucionNormal(resguardos), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmpleados()
        {
            var json = Json(resService.getEmpleados(vSesiones.sesionEmpresaActual), JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        public ActionResult getCentrosCostos()
        {
            return Json(resService.getCentrosCostos(vSesiones.sesionEmpresaActual), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevoEmpleado(EmpleadoResguardoDTO empleado)
        {
            return Json(resService.guardarNuevoEmpleado(empleado), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUltimoFolio(string cc, int alm_salida)
        {
            return Json(resService.getUltimoFolio(cc, alm_salida), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmpleadoNomina(int claveEmpleado)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var empleado = resService.getEmpleadoNomina(claveEmpleado);

                result.Add("empleado", empleado);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboAlmacenVirtual()
        {
            var result = new Dictionary<string, object>();

            try
            {
                result.Add(ITEMS, resService.FillComboAlmacenVirtual());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmpleadosAutoComplete(string term)
        {
            var items = resService.getEmpleadosAutoComplete(term);
            var filteredItems = items.Select(x => new { id = x.clave_empleado, label = x.Nombre });

            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboCcTodosExistentes()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, resService.FillComboCcTodosExistentes());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public MemoryStream DescargarExcelUsuariosEnkontrolNoCoinciden()
        {
            var resultadoTupla = resService.descargarExcelUsuariosEnkontrolNoCoinciden();

            if (resultadoTupla != null)
            {
                string nombreArchivo = resultadoTupla.Item2;

                this.Response.Clear();
                this.Response.ContentType = MimeMapping.GetMimeMapping(nombreArchivo);

                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", nombreArchivo));
                this.Response.BinaryWrite(resultadoTupla.Item1.ToArray());
                this.Response.End();
                return resultadoTupla.Item1;
            }
            else
            {
                return null;
            }
        }

        public ActionResult CargarSesionReporteBitacoraResguardos(string centroCostoInicio, string centroCostoFin, int empleadoInicio, int empleadoFin, List<string> listaEstatus, List<string> listaNumeroSerie)
        {
            return new JsonResult
            {
                Data = resService.cargarSesionReporteBitacoraResguardos(centroCostoInicio, centroCostoFin, empleadoInicio, empleadoFin, listaEstatus, listaNumeroSerie),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }

        public ActionResult cargarSesionReporteBitacoraResguardosCrystal(string centroCostoInicio, string centroCostoFin, int empleadoInicio, int empleadoFin, List<string> listaEstatus, List<string> listaNumeroSerie)
        {
            List<rptResguardoDTO> resultado = resService.cargarSesionReporteBitacoraResguardosCrystal(centroCostoInicio, centroCostoFin, empleadoInicio, empleadoFin, listaEstatus, listaNumeroSerie);

            Session["rptBitacoraResguardos"] = resultado;

            return new JsonResult
            {
                Data = resultado,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }

        public MemoryStream CrearExcelReporteBitacoraResguardos()
        {
            var sesionExcel = Session["ReporteBitacoraResguardos"];

            if (sesionExcel != null)
            {
                var excel = sesionExcel as MemoryStream;

                string nombreArchivo = "Reporte Bitácora de Resguardos.xlsx";

                this.Response.Clear();
                this.Response.ContentType = MimeMapping.GetMimeMapping(nombreArchivo);

                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", nombreArchivo));
                this.Response.BinaryWrite(excel.ToArray());
                this.Response.End();
                return excel;
            }
            else
            {
                return null;
            }
        }
    }
}