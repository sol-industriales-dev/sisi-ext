using Core.DAO.RecursosHumanos.Vacaciones;
using Core.DTO.Principal.Generales;
using Core.DTO.RecursosHumanos.Vacaciones;
using Core.Entity.RecursosHumanos.Vacaciones;
using Data.Factory.RecursosHumanos;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.RecursosHumanos.Vacaciones
{
    public class VacacionesController : BaseController
    {

        Dictionary<string, object> result = new Dictionary<string, object>();
        public IVacacionesDAO vacacionesInterfaz;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            vacacionesInterfaz = new VacacionesFactoryService().GetVacacionesService();
            base.OnActionExecuting(filterContext);
        }

        #region VISTAS

        public ActionResult Vacaciones()
        {
            ViewBag.esConsulta = vacacionesInterfaz.GetEsPermisoSoloConsultaVacaciones();

            return View();
        }
        public ActionResult Periodos()
        {
            return View();
        }
        public ActionResult Gestion()
        {
            return View();
        }
        public ActionResult Responsables()
        {
            return View();
        }
        public ActionResult Pendientes()
        {
            return View();
        }
        public ActionResult Permisos()
        {
            ViewBag.esConsulta = vacacionesInterfaz.GetEsPermisoSoloConsultaPermisos();

            return View();
        }
        public ActionResult AplicarIncidencias()
        {
            return View();
        }
        public ActionResult Incapacidades()
        {
            return View();
        }
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult Saldos()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ReporteDias()
        {
            return View();
        }
        public ActionResult DashboardVacaciones()
        {
            return View();
        }
        public ActionResult Retardos()
        {
            ViewBag.esConsulta = vacacionesInterfaz.GetEsPermisoSoloConsultaPermisos();

            return View();
        }
        public ActionResult RetardosGestion()
        {
            return View();
        }
        #endregion

        #region FILL COMBOS

        public ActionResult FillComboPeriodos()
        {
            result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstData = vacacionesInterfaz.FillComboPeriodos();
                result.Add(ITEMS, lstData);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboCC()
        {
            result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstData = vacacionesInterfaz.FillComboCC();
                result.Add(ITEMS, lstData);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboAutorizantes(int clave_empleado)
        {
            return Json(vacacionesInterfaz.FillComboAutorizantes(clave_empleado), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboUsuarios()
        {
            return Json(vacacionesInterfaz.FillCboUsuarios(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region FNC GRALES

        public ActionResult GetDatosPersona(int? claveEmpleado, string nombre)
        {
            return Json(vacacionesInterfaz.GetDatosPersona(claveEmpleado, nombre), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetNumDias(string claveEmpleado)
        {
            return Json(vacacionesInterfaz.GetNumDias(claveEmpleado), JsonRequestBehavior.AllowGet);
        }
        public ActionResult SetNotificada(int id)
        {
            return Json(vacacionesInterfaz.SetNotificada(id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetResponsable(int clvEmpleado)
        {
            return Json(vacacionesInterfaz.GetResponsable(clvEmpleado), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region CRUD VACACIONES

        public ActionResult GetVacaciones(VacacionesDTO objFiltro)
        {
            return Json(vacacionesInterfaz.GetVacaciones(objFiltro), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetVacacionesIncidencias(VacacionesDTO objFiltro)
        {
            return Json(vacacionesInterfaz.GetVacacionesIncidencias(objFiltro), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarVacacionesIncidencias(List<int> fechasIDs)
        {
            return Json(vacacionesInterfaz.GuardarVacacionesIncidencias(fechasIDs), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarVacaciones(VacacionesDTO objVacaciones)
        {
            return Json(vacacionesInterfaz.CrearEditarVacaciones(objVacaciones), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarVacacion(int id)
        {
            return Json(vacacionesInterfaz.EliminarVacacion(id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarArchivoActa(HttpPostedFileBase archivoActa)
        {
            var vacacion_id = Int32.Parse(Request.Form["vacacion_id"]);

            return Json(vacacionesInterfaz.GuardarArchivoActa(vacacion_id, archivoActa), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DescargarArchivoActa(int id)
        {
            var resultadoTupla = vacacionesInterfaz.DescargarArchivoActa(id);

            if (resultadoTupla != null)
            {
                string nombreArchivo = resultadoTupla.Item2;
                string tipo = MimeMapping.GetMimeMapping(nombreArchivo);

                var fileStreamResult = new FileStreamResult(resultadoTupla.Item1, tipo);
                fileStreamResult.FileDownloadName = nombreArchivo;

                return fileStreamResult;
            }
            else
            {
                return View(RUTA_VISTA_ERROR_DESCARGA);
            }
        }
        #endregion

        #region GESTION VACACIONES

        public ActionResult AutorizarVacacion(int id, int estado, string msg)
        {
            return Json(vacacionesInterfaz.AutorizarVacacion(id, estado, msg), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetVacacionesGestion(VacacionesDTO objFiltro)
        {
            return Json(vacacionesInterfaz.GetVacacionesGestion(objFiltro), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CRUD FECHAS

        public ActionResult GetFechas(int idReg, int tipoPermiso, int? clave_empleado, bool esGestion = false)
        {
            return Json(vacacionesInterfaz.GetFechas(idReg, tipoPermiso, clave_empleado, esGestion), JsonRequestBehavior.AllowGet);
        }


        public ActionResult CrearEditarFechas(int idVacacion, List<DateTime> lstFechas, int tipoPermiso, bool esSobreEscribir, bool esEditar, int diasPermitidos = 0)
        {
            return Json(vacacionesInterfaz.CrearEditarFechas(idVacacion, lstFechas, tipoPermiso, esSobreEscribir, esEditar, diasPermitidos), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region CRUD PERIODOS

        public ActionResult GetPeriodos(PeriodosDTO objFiltro)
        {
            return Json(vacacionesInterfaz.GetPeriodos(objFiltro), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarPeriodo(PeriodosDTO objPeriodo)
        {
            return Json(vacacionesInterfaz.CrearEditarPeriodo(objPeriodo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarPeriodo(int id)
        {
            return Json(vacacionesInterfaz.EliminarPeriodo(id), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region RESPONSABLES
        public ActionResult GetResponsables(string cc, int claveEmpleado)
        {
            return Json(vacacionesInterfaz.GetResponsables(cc, claveEmpleado), JsonRequestBehavior.AllowGet);
        }

        public ActionResult VerificarAntiguedadEmpleado(int claveEmpleado)
        {
            return Json(vacacionesInterfaz.VerificarAntiguedadEmpleado(claveEmpleado), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarResponsable(VacacionesDTO objDTO)
        {
            return Json(vacacionesInterfaz.CrearEditarResponsable(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarResponsable(int id)
        {
            return Json(vacacionesInterfaz.EliminarResponsable(id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetHistorialDias(int id)
        {
            return Json(vacacionesInterfaz.GetHistorialDias(id), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PENDIENTES

        public ActionResult GetVacacionesPendientes(VacacionesDTO objFiltro)
        {
            return Json(vacacionesInterfaz.GetVacacionesPendientes(objFiltro), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region PERMISOS
        public ActionResult GetDiasDispPermisos(int cveUsuario, int tipoPermiso)
        {
            return Json(vacacionesInterfaz.GetDiasDispPermisos(cveUsuario, tipoPermiso), JsonRequestBehavior.AllowGet);
        }
        
        #endregion

        #region INCAPACIDADES
        public ActionResult GetIncapacidades(int? estatus, List<string> ccs, DateTime? fechaInicio, DateTime? fechaTerminacion, int? claveEmpleado, string nombreEmpleado)
        {
            return Json(vacacionesInterfaz.GetIncapacidades(estatus, ccs, fechaInicio, fechaTerminacion, claveEmpleado, nombreEmpleado), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetHistorialIncapacidades(int clave_empleado)
        {
            return Json(vacacionesInterfaz.GetHistorialIncapacidades(clave_empleado), JsonRequestBehavior.AllowGet);
        }
        public ActionResult CrearEditarIncapacidades(IncapacidadesDTO objIncaps)
        {
            return Json(vacacionesInterfaz.CrearEditarIncapacidades(objIncaps), JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteIncapacidades(int id_incap)
        {
            return Json(vacacionesInterfaz.DeleteIncapacidades(id_incap), JsonRequestBehavior.AllowGet);
        }
        public ActionResult NotificarIncapacidades(int id_incapacidad)
        {
            return Json(vacacionesInterfaz.NotificarIncapacidades(id_incapacidad), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarArchivo(int id_incap, int tipoArchivo, HttpPostedFileBase archivo)
        {
            return Json(vacacionesInterfaz.GuardarArchivo(id_incap, tipoArchivo, archivo), JsonRequestBehavior.AllowGet);
        }
        public FileResult DescargarArchivo()
        {
            try
            {
                int archivoCargado_id = Convert.ToInt32(Request.QueryString["archivoCargado_id"]);
                var archivo = vacacionesInterfaz.GetArchivo(archivoCargado_id);

                return File(archivo.ubicacionArchivo, "multipart/form-data", Path.GetFileName(archivo.ubicacionArchivo));
            }
            catch (Exception)
            {
                return null;
            }
        }
        public ActionResult GetIncapacidadesVencer()
        {
            return Json(vacacionesInterfaz.GetIncapacidadesVencer(), JsonRequestBehavior.AllowGet); 
        }
        #endregion

        #region DASHBOARD INCAPS
        public ActionResult GetDashboard(List<string> ccs, DateTime? fechaInicio, DateTime? fechaFin)
        {
            return Json(vacacionesInterfaz.GetDashboard(ccs, fechaInicio, fechaFin), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region SALDOS
        public ActionResult GetSaldos(FiltroSaldosDTO objFiltro)
        {
            return Json(vacacionesInterfaz.GetSaldos(objFiltro), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSaldosDet(int clave_empleado)
        {
            return Json(vacacionesInterfaz.GetSaldosDet(clave_empleado), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarSaldo(SaldosDTO objFiltro)
        {
            return Json(vacacionesInterfaz.CrearEditarSaldo(objFiltro), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteSaldoDet(int id)
        {
            return Json(vacacionesInterfaz.DeleteSaldoDet(id), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region REPORTE DIAS
        public ActionResult GetVacacionesReporte(VacacionesDTO objFiltro)
        {
            return Json(vacacionesInterfaz.GetVacacionesReporte(objFiltro), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetFiltrosExcel(VacacionesDTO objFiltro)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                Session["objFiltroVacacionesExcel"] = objFiltro;

                resultado.Add(SUCCESS, true);

            } 
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);   
            }

            return Json(resultado, JsonRequestBehavior.AllowGet);

        }

        public MemoryStream GetExcelReporte()
        {
            #region EXCEL
            var objFiltro = Session["objFiltroVacacionesExcel"] as VacacionesDTO;

            var objRow = vacacionesInterfaz.GetExcelReporte(objFiltro);

            using (var package = new ExcelPackage())
            {
                //var fecha = (DateTime)Session["EstfechaFinal"];
                var hojaSaldos = package.Workbook.Worksheets.Add("VACACIONES CC " + objFiltro.cc);

                hojaSaldos.Cells["A6"].Value = "CC";
                hojaSaldos.Cells["B6"].Value = "#.";
                hojaSaldos.Cells["C6"].Value = "EMPLEADO";
                hojaSaldos.Cells["D6"].Value = "PUESTO";
                hojaSaldos.Cells["E6"].Value = "FECHA INGRESO";
                hojaSaldos.Cells["F6"].Value = "AÑOS DE SERVICIO";
                hojaSaldos.Cells["G6"].Value = "DIAS DISP AL " + DateTime.Now.ToString("dd/MM/yyyy");
                hojaSaldos.Cells["H6"].Value = "DIAS DEL NUEVO PERIODO";
                hojaSaldos.Cells["I6"].Value = "TOTAL DE DIAS DIPS";
                hojaSaldos.Cells["J6"].Value = "PROGRAMACION";

                var tempFechaIni = objFiltro.fechaInicial.Value;
                int xCell = 11;
                int yCell = 6;

                for (DateTime i = tempFechaIni; i < objFiltro.fechaFinal.Value; i = i.AddMonths(1))
                {
                    string nombreMes = i.Date.ToString("MMM", new System.Globalization.CultureInfo("es-ES"));
                    hojaSaldos.Cells[yCell, xCell].Value = nombreMes;
                    hojaSaldos.Column(xCell).Width = 10;
                    hojaSaldos.Column(xCell).Style.WrapText = true;
                    xCell++;
                }

                hojaSaldos.Cells[6, xCell].Value = "DIAS DISFRUTADOS";
                hojaSaldos.Cells[6, xCell + 1].Value = "DIAS RESTANTES";

                hojaSaldos.Cells[7, 1].LoadFromArrays(objRow);

                hojaSaldos.Cells[6, 1, 6, xCell + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hojaSaldos.Cells[6, 1, 6, xCell + 1].Style.Fill.BackgroundColor.SetColor(1, 255, 192, 0);

                hojaSaldos.Cells[6, 1, (objRow.Count() + 6 ), xCell + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                hojaSaldos.Cells[6, 1, (objRow.Count() + 6 ), xCell + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                hojaSaldos.Cells[6, 1, (objRow.Count() + 6 ), xCell + 1].Style.Border.Left.Style = ExcelBorderStyle.Thick;
                hojaSaldos.Cells[6, 1, (objRow.Count() + 6 ), xCell + 1].Style.Border.Right.Style = ExcelBorderStyle.Thick;

                hojaSaldos.Cells[5, 1, (objRow.Count() + 6 ), xCell + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hojaSaldos.Cells[5, 1, (objRow.Count() + 6 ), xCell + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                hojaSaldos.Cells[5, 1, (objRow.Count() + 6 ), xCell + 1].Style.Font.Size = 8;

                hojaSaldos.Column(3).Width = 15;
                hojaSaldos.Column(4).Width = 15;
                hojaSaldos.Column(5).Width = 10;
                hojaSaldos.Column(6).Width = 10;
                hojaSaldos.Column(7).Width = 10;
                hojaSaldos.Column(8).Width = 10;
                hojaSaldos.Column(9).Width = 10;
                hojaSaldos.Column(10).Width = 10;
                hojaSaldos.Column(xCell).Width = 10;
                hojaSaldos.Column(xCell + 1).Width = 10;

                hojaSaldos.Column(1).Style.WrapText = true;
                hojaSaldos.Column(3).Style.WrapText = true;
                hojaSaldos.Column(4).Style.WrapText = true;
                hojaSaldos.Column(5).Style.WrapText = true;
                hojaSaldos.Column(6).Style.WrapText = true;
                hojaSaldos.Column(7).Style.WrapText = true;
                hojaSaldos.Column(8).Style.WrapText = true;
                hojaSaldos.Column(9).Style.WrapText = true;
                hojaSaldos.Column(10).Style.WrapText = true;
                hojaSaldos.Column(xCell).Style.WrapText = true;
                hojaSaldos.Column(xCell + 1).Style.WrapText = true;

                hojaSaldos.Column(xCell).Style.Font.Bold = true;
                hojaSaldos.Column(xCell).Style.Font.Size = 16;
                hojaSaldos.Column(xCell + 1).Style.Font.Bold = true;
                hojaSaldos.Column(xCell + 1).Style.Font.Size = 16;

                hojaSaldos.Cells[6, 1, 6, xCell].Style.Font.Size = 8;
                hojaSaldos.Cells[6, 1, 6, xCell].Style.Font.Bold = false;
                hojaSaldos.Cells[6, 1, 6, xCell + 1].Style.Font.Size = 8;
                hojaSaldos.Cells[6, 1, 6, xCell + 1].Style.Font.Bold = false;

                hojaSaldos.Cells[5, 11].Value = "PROGRAMA DE VACACIONES";
                hojaSaldos.Cells[5, 11, 5, xCell - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                hojaSaldos.Cells[5, 11, 5, xCell - 1].Style.Border.Top.Style = ExcelBorderStyle.Thick;
                hojaSaldos.Cells[5, 11, 5, xCell - 1].Style.Border.Left.Style = ExcelBorderStyle.Thick;
                hojaSaldos.Cells[5, 11, 5, xCell - 1].Style.Border.Right.Style = ExcelBorderStyle.Thick;
                hojaSaldos.Cells[5, 11, 5, xCell - 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hojaSaldos.Cells[5, 11, 5, xCell - 1].Style.Fill.BackgroundColor.SetColor(1, 255, 192, 0);

                hojaSaldos.Cells[5, 11, 5, xCell - 1].Merge = true;


                using (var exportData = new MemoryStream())
                {
                    this.Response.Clear();
                    package.SaveAs(exportData);
                    this.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    this.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", package.Workbook.Worksheets.FirstOrDefault().Name + ".xlsx"));
                    this.Response.BinaryWrite(exportData.ToArray());
                    this.Response.End();
                    return exportData;

                }
            }
            #endregion

        }
        #endregion

        #region DASHBOARD
        public ActionResult GetDashboardVacaciones(VacacionesDTO objFiltro)
        {
            return Json(vacacionesInterfaz.GetDashboardVacaciones(objFiltro), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region RETARDOS
        public ActionResult GetRetardos(RetardosDTO objFiltro)
        {
            return Json(vacacionesInterfaz.GetRetardos(objFiltro), JsonRequestBehavior.AllowGet);
        }
        public ActionResult CrearEditarRetardo(RetardosDTO objRetardo)
        {
            return Json(vacacionesInterfaz.CrearEditarRetardo(objRetardo), JsonRequestBehavior.AllowGet);
        }
        public ActionResult RemoveRetardo(int idRetardo)
        {
            return Json(vacacionesInterfaz.RemoveRetardo(idRetardo), JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboMotivosByTipo(int tipoRetardo)
        {
            return Json(vacacionesInterfaz.FillComboMotivosByTipo(tipoRetardo), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarArchivoRetardo(int idRetardo, HttpPostedFileBase archivoActa)
        {
            return Json(vacacionesInterfaz.GuardarArchivoRetardo(idRetardo, archivoActa));
        }
        public ActionResult DescargarArchivoRetardo(int id)
        {
            var resultadoTupla = vacacionesInterfaz.DescargarArchivoRetardo(id);

            if (resultadoTupla != null)
            {
                string nombreArchivo = resultadoTupla.Item2;
                string tipo = MimeMapping.GetMimeMapping(nombreArchivo);

                var fileStreamResult = new FileStreamResult(resultadoTupla.Item1, tipo);
                fileStreamResult.FileDownloadName = nombreArchivo;

                return fileStreamResult;
            }
            else
            {
                return View(RUTA_VISTA_ERROR_DESCARGA);
            }
        }
        #endregion

        #region GESTION PRESTAMO
        public ActionResult AutorizarRetardo(int id, int estado, string msg)
        {
            return Json(vacacionesInterfaz.AutorizarRetardo(id, estado, msg), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetRetardosGestion(RetardosDTO objFiltro)
        {
            return Json(vacacionesInterfaz.GetRetardosGestion(objFiltro), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}