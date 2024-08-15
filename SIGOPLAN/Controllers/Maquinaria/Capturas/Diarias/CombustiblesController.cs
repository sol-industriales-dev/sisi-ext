using Core.DTO.Maquinaria.Captura;
using Core.DTO.Maquinaria.Reporte;
using Core.Entity.Maquinaria.Captura;
using Data.Factory.Maquinaria.Captura;
using Data.Factory.Maquinaria.Catalogos;
using Data.Factory.Principal.Alertas;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SIGOPLAN.Controllers.Maquinaria.Capturas.Diarias
{
    public class CombustiblesController : BaseController
    {
        // GET: Combustibles7
        

        #region Factory
        AlertaFactoryServices alertaFactoryServices;
        CapturaCombustibleFactoryServices capturaCombustibleFactoryServices;
        PrecioDieselFactoryServices precioDieselFactoryServices;
        CentroCostosFactoryServices centroCostosFactoryServices;
        #endregion
        CapturaHorometroFactoryServices capturaHorometroFactoryServices;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            capturaHorometroFactoryServices = new CapturaHorometroFactoryServices();
            precioDieselFactoryServices = new PrecioDieselFactoryServices();
            centroCostosFactoryServices = new CentroCostosFactoryServices();
            capturaCombustibleFactoryServices = new CapturaCombustibleFactoryServices();
            alertaFactoryServices = new AlertaFactoryServices();

            base.OnActionExecuting(filterContext);

        }
        public ActionResult Index()
        {
            if (base.getAction("EditarPrecio") || base.getUsuario().id == 13)
            {
                ViewBag.EditarPrecio = true;
            }
            else
            {
                ViewBag.EditarPrecio = false;
            }
            return View();
        }
        public ActionResult FillCboPipa(string obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, capturaCombustibleFactoryServices.getCapturaCombustiblesServices().FillCboPipa(obj).Select(x => new { Value = x.value, Text = x.descripcion }));
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getDataTable(string obj, int turno, DateTime fecha, int idTipo)
        {

            var result = new Dictionary<string, object>();
            try
            {
                var res = capturaCombustibleFactoryServices.getCapturaCombustiblesServices()
                          .getDataTable(obj, turno, fecha, idTipo)
                          .Select(x => new
                          {
                              id = x.id,
                              Carga1 = x.Carga1,
                              Carga2 = x.Carga2,
                              Carga3 = x.Carga3,
                              Carga4 = x.Carga4,
                              HorometroCarga1 = x.HorometroCarga1,
                              HorometroCarga2 = x.HorometroCarga2,
                              HorometroCarga3 = x.HorometroCarga3,
                              HorometroCarga4 = x.HorometroCarga4,
                              CC = x.CC,
                              Economico = x.Economico,
                              fecha = x.fecha,
                              surtidor = x.surtidor,
                              turno = x.turno,
                              volumne_carga = x.volumen_carga,
                              Pipa1 = x.pipa1,
                              Pipa2 = x.pipa2,
                              Pipa3 = x.pipa3,
                              Pipa4 = x.pipa4,
                              capacidadCarga = x.capacidadCarga,
                              PrecioTotal = x.PrecioTotal.ToString("C2"),
                              PrecioLitro = x.PrecioLitro,

                          }).OrderByDescending(x => x.Carga1 > 0).ThenBy(x => x.Economico);

                var raw = capturaCombustibleFactoryServices.getCapturaCombustiblesServices().FillCboPipa(obj).Select(x => new { Value = x.value, Text = x.descripcion });
                var data = new List<string[]>();

                var select = "<select class='form-control itemPipa'>";
                select += "<option value='0'>Seleccione:</option>";
                select += "<option value='Pipa Mina NB'>Pipa Mina NB</option>";
                foreach (var item in raw)
                {
                    select += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                }
                select += "</select>";

                result.Add("combo", select);
                result.Add(SUCCESS, true);
                result.Add("economico", res);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult tablaCboPipa(string obj)
        {

            var result = new Dictionary<string, object>();
            try
            {
                var raw = capturaCombustibleFactoryServices.getCapturaCombustiblesServices().FillCboPipa(obj).Select(x => new { Value = x.value, Text = x.descripcion });
                var data = new List<string[]>();

                var select = "<select class='form-control itemPipa'>";
                select += "<option value='0'>Seleccione:</option>";
                select += "<option value='Pipa Mina NB'>Pipa Mina NB</option>";
                foreach (var item in raw)
                {
                    select += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                }
                select += "<option value='TA-52'>TA-52</option>";
                select += "<option value='CSE-01'>CSE-01</opcion>";

                select += "</select>";

                result.Add("table", select);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveOrUpdate_Combustible(List<tblM_CapCombustible> array)
        {
            var result = new Dictionary<string, object>();
            try
            {

                capturaCombustibleFactoryServices.getCapturaCombustiblesServices().Guardar(array);
                var updated = capturaHorometroFactoryServices.getCapturaHorometroServices().getUpdatedStandBy(array.Select(x => x.Economico).ToList(),2);
                if (updated.Count > 0)
                {
                    string st = "Los siguientes equipos estaban en StandBy y se regresaron a estatus operativo : " + string.Join(",", updated);

                    result.Add(MESSAGE, st);
                }
                else
                {
                    result.Add(MESSAGE, GlobalUtils.getMensaje(1));
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

        public ActionResult SaveOrUpdateAlerta(ActionExecutingContext filterContext, int alerta)
        {
            var result = new Dictionary<string, object>();
            try
            {
                alertaFactoryServices.getAlertaService().updateAlertaByModulo(alerta, 91);
                // Redirect("/Combustibles/RendimientoCombustible");

                return RedirectToAction("RendimientoCombustible", "Combustibles");

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getPrecioDiesel()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = precioDieselFactoryServices.getPrecioDieselService().GetPrecioDiesel();
                result.Add("Precio", data.precio);
                //result.Add(MESSAGE, GlobalUtils.getMensaje(1));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getCentroCostos(string obj)
        {

            var result = new Dictionary<string, object>();
            try
            {
                var res = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(obj);

                result.Add(SUCCESS, true);
                result.Add("centroCostos", res);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RendimientoCombustible()
        {
            return View();
        }

        public ActionResult ReporteCapturasCombustible()
        {
            return View();
        }

        public ActionResult fillGridRendimiento(ReporteDTO obj)//int cc, DateTime fInicio, DateTime fFin)
        {

            var result = new Dictionary<string, object>();
            try
            {
                var listResult = capturaCombustibleFactoryServices.getCapturaCombustiblesServices().getReporteRendimientoComb(obj.cc, obj.fechainicio, obj.fechaFin);
                var nombreCentroCostos = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(obj.cc);

                result.Add("current", 1);
                result.Add("rowCount", 1);
                result.Add("total", listResult.Count());
                result.Add("rows", listResult.OrderBy(x => x.Economico));
                result.Add("nombreCC", nombreCentroCostos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult fillGridCapturaMensual(ReporteDTO obj)
        {
            var result = new Dictionary<string, object>();
            DataTable table = new DataTable();
            try
            {

                var listResult = capturaCombustibleFactoryServices.getCapturaCombustiblesServices().getDataReporteCombustibleMensual(obj.cc, obj.fechainicio);
                var nombreCentroCostos = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(obj.cc);

                var listaEconomicos = listResult.GroupBy(x => x.noEconomico);
                var diasMes = new List<int>();
                List<string> Row;

                int dias = DateTime.DaysInMonth(obj.fechainicio.Year, obj.fechainicio.Month);
                table.Columns.Add("Economico");
                table.Columns.Add("Descripcion");
                table.Columns.Add("Serie");

                for (int i = 1; i <= dias; i++)
                {
                    table.Columns.Add("D" + i);
                    diasMes.Add(i);
                }
                table.Columns.Add("Total");
                decimal totalGeneral = 0;
                foreach (var numEconomico in listaEconomicos)
                {
                    Row = new List<string>();

                    var noSerie = listResult.FirstOrDefault(x => x.noEconomico.Equals(numEconomico.Key)).noSerie;
                    var descripcion = listResult.FirstOrDefault(x => x.noEconomico.Equals(numEconomico.Key)).descripcion;
                    Row.Add(numEconomico.Key);
                    Row.Add(descripcion);
                    Row.Add(noSerie);

                    decimal Total = 0;
                    foreach (var j in diasMes)
                    {

                        DateTime current = new DateTime(obj.fechainicio.Year, obj.fechainicio.Month, j);

                        var celda = listResult.Where(x => x.noEconomico.Equals(numEconomico.Key) && x.fecha.Equals(current));

                        if (celda != null)
                        {
                            decimal total1 = celda.Sum(x => (x.carga1));
                            decimal total2 = celda.Sum(x => (x.carga2));
                            decimal total3 = celda.Sum(x => (x.carga3));
                            decimal total4 = celda.Sum(x => (x.carga4));

                            decimal totalParcial = total1 + total2 + total3 + total4;
                            Total += totalParcial;


                            ///  var val = Convert.ToDecimal(valor == null ? "0" : valor.importe).ToString("C2");
                            ///  
                            Row.Add(Convert.ToDecimal(celda == null ? 0 : decimal.Round(totalParcial, 0)).ToString());//"0,0", CultureInfo.InvariantCulture));
                        }
                        else
                        {
                            Row.Add(0.ToString("0,0"));//, CultureInfo.InvariantCulture));
                        }


                    }
                    totalGeneral += Total;
                    Row.Add(Total.ToString());
                    table.Rows.Add(Row.ToArray());
                }
                Row = new List<string>();
                Row.Add("");
                Row.Add("");
                Row.Add("Total Por Dia");
                for (int i = 1; i <= dias; i++)
                {
                    decimal totalesDia = 0;
                    // var field1 = table.Columns["D1"].ToString();
                    foreach (DataRow dtRow in table.Rows)
                    {

                        var totalDiario = decimal.Round(Convert.ToDecimal(dtRow["D" + i + ""]), 0);
                        totalesDia += totalDiario;
                    }
                    Row.Add(totalesDia.ToString());
                }
                Row.Add(totalGeneral.ToString());

                DataRow toInsert = table.NewRow();
                toInsert.ItemArray = Row.ToArray();
                table.Rows.InsertAt(toInsert, 0);

                Session["reporteCombustiblesMensual"] = table;

                result.Add("nombreCC", nombreCentroCostos);
                result.Add("current", 1);
                result.Add("rowCount", 1);
                result.Add("total", table.Rows.Count);
                result.Add("rows", DataTableToJSON(table));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public static object DataTableToJSON(DataTable table)
        {
            var list = new List<Dictionary<string, object>>();

            foreach (DataRow row in table.Rows)
            {
                var dict = new Dictionary<string, object>();

                foreach (DataColumn col in table.Columns)
                {
                    dict[col.ColumnName] = (Convert.ToString(row[col]));
                }
                list.Add(dict);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            return serializer.Serialize(list);

        }

        public ActionResult ReporteCapturaDiesel()
        {
            return View();
        }

        public ActionResult GetInfoReporteCapturaCombustibles(string cc, int turno, DateTime fechaInicia, DateTime fechaFinal, string economico)
        {
            var result = new Dictionary<string, object>();
            //try
            //{
                var res = capturaCombustibleFactoryServices.getCapturaCombustiblesServices()
                         .getTableInfoCombustibles(cc, turno, fechaInicia, fechaFinal, economico)
                         .Select(c => new
                         {
                             Fecha = c.fecha.ToString("dd/MM/yyyy"),
                             Economico = c.Economico,
                             Litros = c.volumne_carga,
                             Turno = c.turno,
                             CC = c.CC
                         });

                var totalLitros = res.Sum(x => x.Litros);

                result.Add("Total", totalLitros.ToString("0,0.0", CultureInfo.InvariantCulture));
                result.Add(SUCCESS, true);
                result.Add("infoCombustibles", res);
            //}
            //catch (Exception e)
            //{
            //    result.Add(MESSAGE, e.Message);
            //    result.Add(SUCCESS, false);
            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}