using Core.DTO;
using Core.DTO.Maquinaria.Reporte.ActivoFijo;
using Core.DTO.Maquinaria.Reporte.ActivoFijo.Colombia.Cedula;
using Core.DTO.Maquinaria.Reporte.ActivoFijo.Generales.Enkontrol;
using Core.DTO.Maquinaria.Reporte.ActivoFijo.PolizaDepreciacion;
using Core.DTO.Maquinaria.Reporte.ActivoFijo.PolizaDepreciacion.CapturaEnkontrol;
using Core.Entity.Maquinaria.Reporte.ActivoFijo;
using Core.Entity.Maquinaria.Reporte.ActivoFijo.Peru;
using Core.Enum.Multiempresa;
using Data.Factory.Maquinaria.Catalogos;
using Data.Factory.Maquinaria.Reporte.ActivoFijo;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SIGOPLAN.Controllers.Maquinaria.Reportes
{
    public class ActivoFijoController : BaseController
    {
        ActivoFijoFactoryServices affs;
        MaquinaFactoryServices maquinaFactoryServices;

        Dictionary<string, object> respuesta = new Dictionary<string, object>();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            affs = new ActivoFijoFactoryServices();
            maquinaFactoryServices = new MaquinaFactoryServices();
            base.OnActionExecuting(filterContext);
        }

        #region Resumen cedula
        public ActionResult Index()
        {
            ViewBag.idEmpresa = Core.DTO.vSesiones.sesionEmpresaActual;

            return View();
        }

        [HttpGet]
        public JsonResult Resumen(DateTime fechaHasta)
        {
            respuesta = affs.getActivoFijoServices().GetResumen(fechaHasta);

            if ((bool)respuesta[SUCCESS])
            {
                if ((EmpresaEnum)vSesiones.sesionEmpresaActual == EmpresaEnum.Peru || (EmpresaEnum)vSesiones.sesionEmpresaActual == EmpresaEnum.Colombia)
                {

                }
                else
                {
                    Session["rptCedulaActivoFijo"] = respuesta["items"];

                    var info = Session["rptCedulaActivoFijo"];

                    Session["diferenciasContables"] = respuesta["DiferenciasContables"];
                    Session["diferenciasContablesDep"] = respuesta["DiferenciasContablesDep"];
                    Session["Excel"] = respuesta["Excel"];
                    Session["FechaHasta"] = respuesta["FechaHasta"];

                    if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                    {
                        var CedulaColombia = (List<CedulaColombiaDTO>)respuesta["colombia"];

                        Session["ExcelColombia"] = respuesta["colombia"];
                        Session["rptSaldosColombia"] = CedulaColombia.Select(m => m.saldo).ToList();
                        Session["rptDepColombia"] = CedulaColombia.Select(m => m.dep).ToList();
                    }
                }
            }

            respuesta.Remove("DiferenciasContables");
            respuesta.Remove("DiferenciasContablesDep");
            respuesta.Remove("Excel");
            respuesta.Remove("ExcelColombia");

            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DetalleCuenta(DateTime fechaHasta, int cuenta)
        {
            var r = new Dictionary<string, object>();

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var detalles = (List<ActivoFijoDetalleCuentaDTO>)Session["Excel"];
            var ctaDetalles = detalles.First(x => x.Cuenta == cuenta).Detalles;

            r.Add(SUCCESS, true);
            r.Add(ITEMS, ctaDetalles);

            var result = new ContentResult
            {
                Content = serializer.Serialize(r),
                ContentType = "application/json"
            };
            return result;
            //respuesta = affs.getActivoFijoServices().GetDetalleCuenta(fechaHasta, cuenta);
            //var detalles = (List<ActivoFijoDetalleCuentaDTO>)Session["Excel"];
            //respuesta.Add(SUCCESS, true);
            //respuesta.Add(ITEMS, detalles.First(x => x.Cuenta == cuenta).Detalles);
            //return Json(result, JsonRequestBehavior.AllowGet);
        }

        public MemoryStream Excel(DateTime fechaSaldoActual)
        {
            respuesta = affs.getActivoFijoServices().getDetalleExcel((List<ActivoFijoDetalleCuentaDTO>)Session["Excel"], (DateTime)Session["FechaHasta"], vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? (List<CedulaColombiaDTO>)Session["ExcelColombia"] : null);


            //var respuesta = affs.getActivoFijoServices().GetExcel(fechaSaldoActual, true);
            if ((bool)respuesta[SUCCESS])
            {
                var stream = (MemoryStream)respuesta[ITEMS];
                Response.Clear();
                Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("Content-Disposition", "attachement; filename=Reporte.xlsx");
                Response.BinaryWrite(stream.ToArray());
                Response.End();
            }
            return null;
        }

        [HttpGet]
        public JsonResult GetDepreciacionCuenta()
        {
            respuesta = affs.getActivoFijoServices().GetDepreciacionCuenta();

            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ModificarDepreciacionCuenta(List<ActivoFijoDepreciacionCuentasDTO> depCuentas)
        {
            respuesta = affs.getActivoFijoServices().ModificarDepreciacionCuenta(depCuentas);
            return Json(respuesta);
        }

        [HttpGet]
        public JsonResult ObtenerDiferenciasSaldos(int cuenta)
        {
            var difContableSaldos = (List<ActivoFijoDiferenciasContablesDTO>)Session["diferenciasContables"];
            return Json(difContableSaldos.Where(x => x.Cuenta == cuenta), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerDiferenciasDep(int cuenta)
        {
            var difContableSaldos = (List<ActivoFijoDiferenciasContablesDTO>)Session["diferenciasContablesDep"];
            return Json(difContableSaldos.Where(x => x.Cuenta == cuenta), JsonRequestBehavior.AllowGet);
        }
        #endregion

        public JsonResult GetDepreciacionNoEconomico(string noEconomico, DateTime fechaHasta)
        {
            return Json(affs.getActivoFijoServices().DepreciacionNumEconomico(noEconomico, fechaHasta), JsonRequestBehavior.AllowGet);
        }

        #region Depreciación cuentas
        public ActionResult DepreciacionCuentas()
        {
            return View();
        }
        #endregion

        #region CRUD depreciación maquinas
        public ActionResult DepreciacionMaquinas()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CalcularEnviosACosto()
        {
            var resultado = affs.getActivoFijoServices().CalcularEnviosACosto();
            return Json(resultado);
        }

        [HttpGet]
        public JsonResult GenerarPolizaCostoPorInsumo(int idCatMaqDepreciacion)
        {
            var resultado = affs.getActivoFijoServices().GenerarPolizaCostoPorInsumo(idCatMaqDepreciacion);

            if (resultado.Success)
            {
                Session["AFPolizaCostoGenerada"] = resultado.Value;
            }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GenerarPolizaCosto(List<int> idEnvioCosto)
        {
            var resultado = affs.getActivoFijoServices().GenerarPolizaCosto(idEnvioCosto);
            
            if (resultado.Success)
            {
                Session["AFPolizaCostoGenerada"] = resultado.Value;
            }

            return Json(resultado);
        }

        [HttpPost]
        public JsonResult RegistrarPolizaCosto()
        {
            var resultado = affs.getActivoFijoServices().RegistrarPolizaCosto(Session["AFPolizaCostoGenerada"] as PolizaCapturaEnkontrolDTO);

            return Json(resultado);
        }

        [HttpPost]
        public JsonResult RelacionAutomaticaPolizas()
        {
            respuesta = affs.getActivoFijoServices().RelacionAutomaticaPolizas();

            if ((bool)respuesta[SUCCESS])
            {
                if (respuesta["seGeneroPolizaCosto"] != null && (bool)respuesta["seGeneroPolizaCosto"])
                {
                    Session["AFPolizaCostoGenerada"] = respuesta["polizaGeneradaCostos"];
                }
            }

            return Json(respuesta);
        }

        [HttpGet]
        public JsonResult GetMaquinas(int idCuenta, int estatusMaquina, int tipoCaptura)
        {
            respuesta = affs.getActivoFijoServices().GetMaquinas(idCuenta, estatusMaquina, tipoCaptura);
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RegistrarDepMaquina(List<ActivoFijoRegInfoDepDTO> depMaquina)
        {
            respuesta = affs.getActivoFijoServices().RegistrarDepMaquina(depMaquina);
            return Json(respuesta);
        }

        [HttpGet]
        public JsonResult ObtenerDepMaquina(int idDepMaquina)
        {
            respuesta = affs.getActivoFijoServices().ObtenerDepMaquina(idDepMaquina);
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult AgregarPolizas(int idMaquina)
        {
            respuesta = affs.getActivoFijoServices().AgregarPolizas(idMaquina);
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult AgregarPoliza(ActivoFijoAgregarPolizaDTO infoPoliza)
        {
            var respuesta = affs.getActivoFijoServices().AgregarPoliza(infoPoliza);
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ModificarDepMaquina(List<ActivoFijoRegInfoDepDTO> depMaquina)
        {
            respuesta = affs.getActivoFijoServices().ModificarDepMaquina(depMaquina);
            return Json(respuesta);
        }

        [HttpPost]
        public JsonResult EliminarDepMaquina(int idDepMaquina)
        {
            respuesta = affs.getActivoFijoServices().EliminarDepMaquina(idDepMaquina);
            return Json(respuesta);
        }

        [HttpGet]
        public JsonResult ObtenerPolizasCC(string Cc)
        {
            respuesta = affs.getActivoFijoServices().ObtenerPolizasCC(Cc);
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CRUD subcuentas
        public ActionResult AltaSubCuentas()
        {
            return View();
        }

        public ActionResult GetCuentas()
        {
            respuesta = affs.getActivoFijoServices().GetCuentas();

            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSubCuentas(int cuenta)
        {
            respuesta = affs.getActivoFijoServices().GetSubCuentas(cuenta);

            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region TabuladorDepreciacion
        public ActionResult TabuladorDepreciacion()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetDepMaquinas(int? maquinaActiva, int? cuenta, string noEconomico, List<int> tipoMovimiento, List<string> areasCuenta, int? cuentaOverhaul, DateTime? fecha)
        {
            respuesta = affs.getActivoFijoServices().GetDepMaquinas(maquinaActiva, cuenta, noEconomico, tipoMovimiento, areasCuenta, null, cuentaOverhaul, fecha, false);

            if ((bool)respuesta[SUCCESS])
            {
                Session["resumenDepreciacion"] = respuesta[ITEMS];
                Session["resumenCuenta"] = cuenta;
                Session["resumenEquipo"] = noEconomico;
                Session["resumenEstado"] = maquinaActiva;

                var json = Json(respuesta, JsonRequestBehavior.AllowGet);
                json.MaxJsonLength = int.MaxValue;
                return json;
            }

            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetConsultaEnExcel()
        {
            respuesta = affs.getActivoFijoServices().GetConsultaEnExcel((ActivoFijoDepMaquinaResumenDTO)Session["resumenDepreciacion"], (int?)Session["resumenCuenta"], (string)Session["resumenEquipo"], (int?)Session["resumenEstado"]);

            if ((bool)respuesta[SUCCESS])
            {
                var stream = (MemoryStream)respuesta[ITEMS];
                Response.Clear();
                Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("Content-Disposition", "attachement; filename=Reporte.xlsx");
                Response.BinaryWrite(stream.ToArray());
                Response.End();
            }

            return null;
        }

        [HttpPost]
        public JsonResult GetCentrosCostos()
        {
            respuesta = affs.getActivoFijoServices().GetCentrosCostos();
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAreasCuenta()
        {
            return Json(affs.getActivoFijoServices().GetAreasCuenta());
        }

        [HttpGet]
        public JsonResult GetTabulador(int idDepMaquina, bool EsExtraCatMaqDep)
        {
            respuesta = affs.getActivoFijoServices().GetTabulador(idDepMaquina, EsExtraCatMaqDep);
            if ((bool)respuesta[SUCCESS])
            {
                Session["Tabulador"] = respuesta[ITEMS];
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        public MemoryStream ExcelTabulador()
        {
            respuesta = affs.getActivoFijoServices().GetTabuladorExcel((ActivoFijoInfoTabuladorDTO)Session["Tabulador"]);


            //var respuesta = affs.getActivoFijoServices().GetExcel(fechaSaldoActual, true);
            if ((bool)respuesta[SUCCESS])
            {
                var stream = (MemoryStream)respuesta[ITEMS];
                Response.Clear();
                Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("Content-Disposition", "attachement; filename=Reporte.xlsx");
                Response.BinaryWrite(stream.ToArray());
                Response.End();
            }
            return null;
        }

        [HttpPost]
        public JsonResult GetPeriodosDepreciacion(int IdCatMaquina)
        {
            respuesta = affs.getActivoFijoServices().GetPeriodosDepreciacion(IdCatMaquina);
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PolizaDepreciacion
        public ActionResult PolizaDepreciacion()
        {
            return View();
        }

        public ActionResult getFileRuta(int id)
        {
            var result = new Dictionary<string, object>();

            result.Add(SUCCESS, Session["archivoVisor"] != null);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult FechaCaptura(int cuenta, bool esOverhaul)
        {
            return Json(affs.getActivoFijoServices().FechaCaptura(cuenta, esOverhaul), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarPoliza(int polizaId)
        {
            return Json(affs.getActivoFijoServices().EliminarPoliza(polizaId));
        }

        [HttpPost]
        public JsonResult RegistrarPoliza()
        {
            var respuesta = affs.getActivoFijoServices().RegistrarPoliza((List<PolizaGeneradaDTO>)Session["AFPolizaGenerada"]);
            Session.Remove("AFPolizaGenerada");
            if (respuesta.Success)
            {
                var reporte = respuesta.Value as ReportePolizaDTO;
                //Session["AFReportePolizaCapturada"] = reporte;
            }
            return Json(respuesta);
        }

        [HttpGet]
        public JsonResult GenerarPoliza(int cuenta, int año, int mes, int semana, int dia, bool esOverhaul, int? idCuentaDepOverhaul)
        {
            var respuesta = affs.getActivoFijoServices().GenerarPoliza(cuenta, año, mes, semana, dia, esOverhaul, idCuentaDepOverhaul);
            if (respuesta.Success)
            {
                Session["AFPolizaGenerada"] = respuesta.Value;
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerPolizaDetalle(int año, int mes, int poliza)
        {
            return Json(affs.getActivoFijoServices().ObtenerPolizaDetalle(año, mes, poliza), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerPolizasDepreciacion(int año, int mes, int cuenta)
        {
            return Json(affs.getActivoFijoServices().ObtenerPolizasDepreciacion(año, mes, cuenta), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CuentasDepOverhaul()
        {
            return Json(affs.getActivoFijoServices().CuentasDepOverhaul());
        }
        #endregion

        #region CargarExcel
        public ActionResult JalarExcel()
        {
            bool activo = false;
            if (activo)
            {
                respuesta = affs.getActivoFijoServices().JalarExcel();
                if ((bool)respuesta[SUCCESS])
                {
                    ViewBag.Resultado = "sSSSSSSS TODO! =D";
                }
                else
                {
                    ViewBag.Resultado = "RECORCHOLIS!";
                    ViewBag.Horror = (string)respuesta[MESSAGE];
                }
            }
            else
            {
                ViewBag.Resultado = "Nop";
            }

            return View();
        }
        #endregion

        [HttpPost]
        public JsonResult GetCuentas(int tipoResultado)
        {
            respuesta = affs.getActivoFijoServices().GetCuentas(tipoResultado);
            return Json(respuesta);
        }

        [HttpPost]
        public JsonResult GetCuentasCBO()
        {
            respuesta = affs.getActivoFijoServices().GetCuentasCBO();
            return Json(respuesta);
        }

        [HttpPost]
        public JsonResult GetTiposMovimiento()
        {
            respuesta = affs.getActivoFijoServices().GetTiposMovimiento();
            return Json(respuesta);
        }

        public ViewResult InsumosOverhaul()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AddInsumo(tblC_AF_InsumosOverhaul insumo)
        {
            return Json(affs.getActivoFijoServices().AddInsumo(insumo));
        }

        [HttpGet]
        public JsonResult GetInsumos()
        {
            return Json(affs.getActivoFijoServices().GetInsumos(), JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult UpdateInsumos(List<tblC_AF_InsumosOverhaul> insumos)
        {
            return Json(affs.getActivoFijoServices().UpdateInsumos(insumos));
        }

        [HttpPost]
        public JsonResult AgregarInsumosAutomaticamente()
        {
            return Json(affs.getActivoFijoServices().AgregarInsumosAutomaticamente());
        }

        //--> Función relación maquina - insumo
        #region EnviarCosto
        public ActionResult EnviarCosto()
        {
            return View();
        }
        public ActionResult CargarTablaEnvioCosto(bool enviado = false)
        {
            var data = affs.getActivoFijoServices().CargarTablaEnvioCosto(enviado);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult RelacionEquipoInsumo(int maquinaID)
        {
            return Json(affs.getActivoFijoServices().RelacionEquipoInsumo(maquinaID));
        }
        #endregion

        #region Carga de información desde Excel
        [HttpGet]
        public ActionResult RelacionarInfoExcel_NoMaquinas()
        {
            bool activo = false;

            if (activo)
            {
                var r = affs.getActivoFijoServices().RelacionarInfoExcel_NoMaquinas();

                if (r.Success)
                {
                    ViewBag.Resultado = "Operación correcta";
                }
                else
                {
                    ViewBag.Resultado = "Error";
                    ViewBag.Error = r.Message;
                }
            }
            else
            {
                ViewBag.Resultado = "Operación bloqueada";
            }

            return View();
        }
        #endregion

        #region ConstruplanColombia
        public ActionResult RelacionPolizaColombia()
        {
            return View("Colombia/RelacionPolizaColombia");
        }

        public JsonResult TipoActivoColombiaCBox()
        {
            return Json(affs.getActivoFijoServices().TipoActivoColombiaCBox());
        }

        public JsonResult GetActivosColombia(int tipoActivo, bool esMaquina)
        {
            return Json(affs.getActivoFijoServices().GetActivosColombia(tipoActivo, esMaquina), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRelacionPoliza(int idActivo, bool esMaquina)
        {
            return Json(affs.getActivoFijoServices().GetRelacionPoliza(idActivo, esMaquina), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ConstruplanPeru
        public ActionResult RelacionPolizaPeru()
        {
            return View();
        }

        public JsonResult GetAnios()
        {
            return Json(affs.getActivoFijoServices().GetAnios(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCCs()
        {
            return Json(affs.getActivoFijoServices().GetCCs(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCuentasPeru()
        {
            return Json(affs.getActivoFijoServices().GetCuentasPeru(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetActivos(int? anio, string cc, int? cuenta)
        {
            return Json(affs.getActivoFijoServices().GetActivos(anio, cc, cuenta), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEconomicosPeru()
        {
            return Json(affs.getActivoFijoServices().GetEconomicosPeru(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GuardarRelacionActivo(tblC_AF_RelacionPolizaPeru obj)
        {
            return Json(affs.getActivoFijoServices().GuardarRelacionActivo(obj));
        }

        public JsonResult EliminarRelacionActivo(int id)
        {
            return Json(affs.getActivoFijoServices().EliminarRelacionActivo(id));
        }
        #endregion
    }
}