using Core.DTO.Maquinaria.Reporte;
using Data.DAO.Maquinaria.Reporte;
using Data.Factory.Maquinaria.Captura;
using Data.Factory.Maquinaria.Catalogos;
using Infrastructure.DTO;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Core.DTO.Utils.Data;

namespace SIGOPLAN.Controllers.Maquinaria.Reportes
{
    public class RepGastosMaquinariaController : BaseController
    {
        #region Atributos
        private readonly string SUCCESS = "success";
        private readonly string MESSAGE = "message";
        private const string PAGE = "page";
        private const string TOTAL_PAGE = "total";
        private const string ROWS = "rows";
        private const string ITEMS = "items";
        #endregion

        //#region Factory
        //CentroCostosFactoryServices centroCostosFactoryServices;
        private MaquinaFactoryServices maquinaFactoryServices = new MaquinaFactoryServices();
        private CapturaOTDetFactoryServices capturaOTDetFactoryServices = new CapturaOTDetFactoryServices();
        //#endregion
        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    centroCostosFactoryServices = new CentroCostosFactoryServices();
        //}

        // GET: ReporteMaquinaria
        public ActionResult Index()
        {
            ViewBag.pagina = "catalogo";

            return View();
        }
        public ActionResult Analisis_Tendencias()
        {
            return View();
        }

        public ActionResult ReporteCostos()
        {
            return View();
        }

        public ActionResult fillCboTipoMaquina()
        {
            RepGastosMaquinaDAO repGastosMaquina = new RepGastosMaquinaDAO();
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, repGastosMaquina.FillCboTipoMaquinaria().Select(x => new { Value = x.id, Text = x.descripcion }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult fillCboGrupoMaquina(int idTipo)
        {
            RepGastosMaquinaDAO repGastosMaquina = new RepGastosMaquinaDAO();
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, repGastosMaquina.FillCboGrupoMaquinaria(idTipo).Select(x => new { Value = x.id, Text = x.descripcion, Prefijo = x.tipoMaquina }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult fillCboMaquinarias(int idGrupo, int idTipo)
        {
            RepGastosMaquinaDAO repGastosMaquina = new RepGastosMaquinaDAO();
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, repGastosMaquina.FillCboReporteGastosMaquinaria(idGrupo, idTipo).Select(x => new { Value = x.area, Text = x.descripcion, Prefijo = x.cuenta }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult fillGraficaReporteMaquinaria(RepGastosFiltrosDTO obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                RepGastosMaquinaDAO repGastosMaquina = new RepGastosMaquinaDAO();

                var res = repGastosMaquina.FillGraficaRepGasto(obj).OrderBy(x => x.mes);
                CultureInfo mesEs = new CultureInfo("es-ES");

                var RES = repGastosMaquina.FillInfoGastosMaquinaria(obj.maq);

                var ValidarCamposEconomico = RES.FirstOrDefault();
                var datosEconomico = maquinaFactoryServices.getMaquinaServices().GetMaquinaByNoEconomico(obj.maq);
                if (ValidarCamposEconomico.marca == null)
                {
                    RES.FirstOrDefault().marca = datosEconomico.marca.descripcion;

                }
                if (ValidarCamposEconomico.modelo == null)
                {
                    RES.FirstOrDefault().modelo = datosEconomico.modeloEquipo.descripcion;

                }



                result.Add("info", RES.Select(x => new RepGastosMaquinaInfoDTO { depreciacion = Convert.ToDecimal(x.depreciacion).ToString("C2"), descripcion = x.descripcion, fechaAdquisicion = x.fechaAdquisicion, marca = x.marca, modelo = x.modelo, saldoinicial = Convert.ToDecimal(x.saldoinicial).ToString("C2") }).FirstOrDefault());
                string anio = "";
                DateTime fechaInicio = Convert.ToDateTime(obj.fechaInicio);
                DateTime fechaFin = Convert.ToDateTime(obj.fechaFin);

                var costoOverHaul = repGastosMaquina.valorXoverhaul(obj.maq, fechaInicio, fechaFin);
                var costoOverHaulAplicado = repGastosMaquina.valorXoverhaulAplicado(obj);

                if (fechaInicio.Year == fechaFin.Year)
                {
                    result.Add(ITEMS, res.Select(x => new { importe = x.importe, mes = mesEs.DateTimeFormat.GetMonthName(x.mes), anio = x.anio }));
                }
                else
                {
                    result.Add(ITEMS, res.Select(x => new { importe = x.importe, mes = "", anio = x.anio }));
                }

                //result.Add(ITEMS, res.Select(x => new { importe = x.importe, mes = mesEs.DateTimeFormat.GetMonthName(x.mes), anio = x.anio }));
                result.Add("costoOverHaul", costoOverHaul.ToString("C2"));
                result.Add("costoOverHaulAplicado", costoOverHaulAplicado.ToString("C2"));
                result.Add("total", res.Sum(x => x.importe).ToString("C2"));


                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult fillGridReporteMaquinariaXMes(RepGastosFiltrosDTO obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                RepGastosMaquinaDAO repGastosMaquina = new RepGastosMaquinaDAO();
                Regex rex = new Regex("[a-zA-ZñÑ]");
                if (rex.IsMatch(obj.mes))
                {
                    obj.mesID = DateTime.ParseExact(obj.mes, "MMMM", new CultureInfo("es-ES")).Month;
                    result.Add("encabezadoModal", repGastosMaquina.getRango(obj.fechaInicio, obj.fechaFin, obj.mesID.ToString()));
                }
                else
                { result.Add("encabezadoModal", repGastosMaquina.getRango(obj.fechaInicio, obj.fechaFin, obj.mes)); }

                List<RepGastosMaquinariaGrid> res = repGastosMaquina.FillGridReporteGastosMaquinaria(obj).OrderBy(x => x.fecha).ToList();

                var InfoGastos = maquinaFactoryServices.getMaquinaServices().GetMaquinaByNoEconomico(obj.maq);

                if (InfoGastos != null)
                {
                    RepGastosMaquinariaGrid infos = new RepGastosMaquinariaGrid();

                    DateTime FI = Convert.ToDateTime(obj.fechaInicio);
                    DateTime FF = Convert.ToDateTime(obj.fechaFin);

                    FI = new DateTime(FI.Year, (FI.Month), FI.Day);
                    FF = new DateTime(FF.Year, (FF.Month), DateTime.DaysInMonth(FI.Year, FI.Month));
                    RepGastosMaquinariaGrid CostoHoraHombre = capturaOTDetFactoryServices.getCapturaOTDetFactoryServices().GetCostosHoraHombre(InfoGastos.id, FI, FF).FirstOrDefault();

                    if (CostoHoraHombre.importe != "0")
                    {
                        res.Add(CostoHoraHombre);
                    }

                }
                result.Add(ROWS, res.Select(x => new { economico = x.economico, descripcion = x.descripcion, importe = Convert.ToDecimal(x.importe).ToString("C2"), id = x.tipoInsumo, mes = obj.mes }));
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult FillGridReporteGastosGrupoInsumosXmes(RepGastosFiltrosDTO obj)
        {

            var result = new Dictionary<string, object>();
            try
            {
                Regex rex = new Regex("[a-zA-ZñÑ]");
                if (rex.IsMatch(obj.mes))
                {
                    obj.mesID = DateTime.ParseExact(obj.mes, "MMMM", new CultureInfo("es-ES")).Month;

                }

                if (obj.idTipo < 10)
                {
                    RepGastosMaquinaDAO repGastosMaquina = new RepGastosMaquinaDAO();
                    var res = repGastosMaquina.FillGridReporteGastosGrupoInsumos(obj);
                    result.Add(ROWS, res.Select(x => new { economico = x.economico, descripcion = x.descripcion, importe = Convert.ToDecimal(x.importe).ToString("C2"), id = x.tipoInsumo, mes = obj.mes }));

                }
                else
                {
                    RepGastosMaquinaDAO repGastosMaquina = new RepGastosMaquinaDAO();
                    var res = capturaOTDetFactoryServices.getCapturaOTDetFactoryServices().FillMotivosParo(obj);
                    result.Add(ROWS, res.Select(x => new { economico = x.economico, descripcion = x.descripcion, importe = Convert.ToDecimal(x.importe).ToString("C2"), id = x.tipoInsumo, mes = obj.mes }));
                }


                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult FillGridReporteGastosInsumosXmes(RepGastosFiltrosDTO obj)
        {

            var result = new Dictionary<string, object>();
            try
            {
                Regex rex = new Regex("[a-zA-ZñÑ]");
                if (rex.IsMatch(obj.mes))
                {
                    obj.mesID = DateTime.ParseExact(obj.mes, "MMMM", new CultureInfo("es-ES")).Month;

                }

                if (obj.idTipo < 10)
                {
                    RepGastosMaquinaDAO repGastosMaquina = new RepGastosMaquinaDAO();
                    var res = repGastosMaquina.FillGridReporteGastosInsumos(obj);
                    result.Add(ROWS, res.Select(x => new { economico = x.economico, descripcion = x.descripcion, importe = Convert.ToDecimal(x.importe).ToString("C2"), id = x.tipoInsumo, fecha = x.fecha }));

                }
                else
                {
                    RepGastosMaquinaDAO repGastosMaquina = new RepGastosMaquinaDAO();
                    var res = capturaOTDetFactoryServices.getCapturaOTDetFactoryServices().FillUsuario(obj);

                    result.Add(ROWS, res.Select(x => new { economico = x.economico, descripcion = x.descripcion, importe = Convert.ToDecimal(x.importe).ToString("C2"), id = x.tipoInsumo, mes = obj.mes }));
                }


                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        public ActionResult FillCbo_Anios()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var res = GlobalUtils.getFecha(6);
                res.Add(new ComboDTO { Value = 0, Text = "TODOS" });
                result.Add(ITEMS, res);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillInfoGastosMaquinaria(string obj)
        {

            var result = new Dictionary<string, object>();
            try
            {
                RepGastosMaquinaDAO repGastosMaquina = new RepGastosMaquinaDAO();
                var res = repGastosMaquina.FillInfoGastosMaquinaria(obj);
                result.Add(ROWS, res);
                var RES = repGastosMaquina.FillInfoGastosMaquinaria(obj.Trim());

                result.Add("info", RES.FirstOrDefault());
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public JsonResult EnviarCorreoTendenciasIngresosCostos()
        {
            RepGastosMaquinaDAO repGastosMaquina = new RepGastosMaquinaDAO();
            return Json(repGastosMaquina.EnviarCorreoTendenciasIngresosCostos(true), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getDatosGeneralesEmpresa(int empresa, int anio, List<string> cc, int grupo, int modelo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                RepGastosMaquinaDAO repGastosMaquina = new RepGastosMaquinaDAO();
                var datos = repGastosMaquina.getDatosGeneralesEmpresa(empresa, anio, cc, grupo, modelo);
                result.Add("datosGenerales", datos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getDatosGenerales(int empresa, int anio, string cc, int grupo, int modelo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                RepGastosMaquinaDAO repGastosMaquina = new RepGastosMaquinaDAO();
                var datos = repGastosMaquina.getDatosGenerales(empresa, anio, cc, grupo, modelo);
                result.Add("datosGenerales", datos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getDatosGeneralesCTA(int empresa, int cta, int anio, List<string> cc, int grupo, int modelo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                RepGastosMaquinaDAO repGastosMaquina = new RepGastosMaquinaDAO();
                var datos = repGastosMaquina.getDatosGeneralesCTA(empresa, cta, anio, cc, grupo, modelo);
                result.Add("datosGenerales", datos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getDatosDetalle(int empresa, int anio, int mes, List<string> cc, int cta, int scta, int sscta)
        {
            var result = new Dictionary<string, object>();
            try
            {
                RepGastosMaquinaDAO repGastosMaquina = new RepGastosMaquinaDAO();
                var datos = repGastosMaquina.getDatosDetalle(empresa, anio, mes, cc, cta, scta, sscta);
                result.Add("datos", datos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getDatosDetalle_Movto(int empresa, int anio, int mes, List<string> cc, int cta, int scta, int sscta, string economico)
        {
            var result = new Dictionary<string, object>();
            try
            {
                RepGastosMaquinaDAO repGastosMaquina = new RepGastosMaquinaDAO();
                var datos = repGastosMaquina.getDatosDetalle_Movto(empresa, anio, mes, cc, cta, scta, sscta, economico);
                result.Add("datos", datos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarReporteCostos(string cc, int anio)
        {
            RepGastosMaquinaDAO repGastosMaquina = new RepGastosMaquinaDAO();
            return Json(repGastosMaquina.CargarReporteCostos(cc, anio), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboCC()
        {
            RepGastosMaquinaDAO repGastosMaquina = new RepGastosMaquinaDAO();
            return Json(repGastosMaquina.FillComboCC(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCentrosCostoTodos()
        {
            var result = new Dictionary<string, object>();
            try
            {
                CentroCostosFactoryServices centroCostosFactoryServices = new CentroCostosFactoryServices();

                result.Add(ITEMS, centroCostosFactoryServices.getCentroCostosService().getListaCC());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCentrosCosto_Rep_Costos()
        {
            var result = new Dictionary<string, object>();
            try
            {
                CentroCostosFactoryServices centroCostosFactoryServices = new CentroCostosFactoryServices();

                result.Add(ITEMS, centroCostosFactoryServices.getCentroCostosService().getListaCC_Rep_Costos());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GenerarExcelReporteCostos(string cc, int anio)
        {
            RepGastosMaquinaDAO repGastosMaquina = new RepGastosMaquinaDAO();
            var stream = repGastosMaquina.GenerarExcelReporteCostos(cc, anio);
            var resultado = new Dictionary<string, object>();

            Session["reporteCostos"] = stream;

            if (stream != null)
            {
                resultado.Add(SUCCESS, true);
            }
            else
            {
                resultado.Add(SUCCESS, false);
            }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public MemoryStream DescargarExcelReporteCostos()
        {
            if (Session["reporteCostos"] != null)
            {
                var stream = Session["reporteCostos"] as MemoryStream;

                this.Response.Clear();
                this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "Reporte de Costos.xlsm"));
                this.Response.BinaryWrite(stream.ToArray());
                this.Response.End();

                return stream;
            }
            else
            {
                return null;
            }
        }
    }
}