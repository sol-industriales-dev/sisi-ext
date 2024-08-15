using Core.DTO.Maquinaria.Reporte;
using Core.DTO.Reportes;
using Data.DAO.Maquinaria.Reporte;
using Data.Factory.Maquinaria.Captura;
using Data.Factory.Maquinaria.Reporte;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Maquinaria.Reportes
{
    public class RepComparativaTiposController : BaseController
    {
        #region Factory
        RepComparativaTiposFactoryServices repComparativaTiposFactoryServices;
        CapturaHorometroFactoryServices capturaHorometroFactoryServices;

        #endregion
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            repComparativaTiposFactoryServices = new RepComparativaTiposFactoryServices();
            capturaHorometroFactoryServices = new CapturaHorometroFactoryServices();
            base.OnActionExecuting(filterContext);
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult getTableStruct(RepGastosFiltrosDTO obj, string centroCostos)
        {
            var result = new Dictionary<string, object>();
            RepGastosMaquinaDAO repGastosMaquina = new RepGastosMaquinaDAO();
            List<GraficaComparativaDTO> Grafica = new List<GraficaComparativaDTO>();
            Session["dtReporteComparativaTipos"] = "";
            var guardar = false;
            try
            {

                var raw = new List<RepComparativaTiposDTO>();
                var rawsoh = new List<RepComparativaTiposDTO>();




                if (obj.area == 0)
                {
                    if (centroCostos != "" && obj.idTipo == 0)
                    {
                        List<string> ListaEconomicosCC = repComparativaTiposFactoryServices.getComparativoTiposService().getEconomicosXCentroCostos(centroCostos).Select(x => x.descripcion).ToList();
                        raw = repComparativaTiposFactoryServices.getComparativoTiposService().getAmountbyType(obj).Where(x => ListaEconomicosCC.Contains(x.noEco)).ToList();
                        rawsoh = repComparativaTiposFactoryServices.getComparativoTiposService().getAmountbyTypeNoOverhaul(obj).Where(x => ListaEconomicosCC.Contains(x.noEco)).ToList();
                    }
                    else
                    {
                        guardar = true;
                        raw = repComparativaTiposFactoryServices.getComparativoTiposService().getAmountbyGroup(obj, centroCostos).ToList();
                        rawsoh = repComparativaTiposFactoryServices.getComparativoTiposService().getAmountbyTypeNoOverhaulByTipo(obj, centroCostos).ToList();
                    }

                }
                else
                {
                    if (centroCostos != "")
                    {
                        List<string> ListaEconomicosCC = repComparativaTiposFactoryServices.getComparativoTiposService().getEconomicosXCentroCostos(centroCostos).Select(x => x.descripcion).ToList();
                        raw = repComparativaTiposFactoryServices.getComparativoTiposService().getAmountbyType(obj).Where(x => ListaEconomicosCC.Contains(x.noEco)).ToList();
                        rawsoh = repComparativaTiposFactoryServices.getComparativoTiposService().getAmountbyTypeNoOverhaul(obj).Where(x => ListaEconomicosCC.Contains(x.noEco)).ToList();
                    }
                    else
                    {
                        raw = repComparativaTiposFactoryServices.getComparativoTiposService().getAmountbyType(obj).ToList();
                        rawsoh = repComparativaTiposFactoryServices.getComparativoTiposService().getAmountbyTypeNoOverhaul(obj).ToList();
                    }

                }






                ///   var rawsoh = repComparativaTiposFactoryServices.getComparativoTiposService().getAmountbyTypeNoOverhaulByTipo(obj);
                var listNoEco = raw.Select(x => x.noEco).Distinct().ToList().OrderBy(x => x);
                var listTipos = raw.Select(x => x.descripcion).Distinct().ToList();

                var data = new List<string[]>();
                var fechainicio = Convert.ToDateTime(obj.fechaInicio);
                var fechafin = Convert.ToDateTime(obj.fechaFin);

                //foreach (var x in listNoEco)
                //{
                //    string[] ar = new string[listTipos.Count+1];
                //    ar[0] = x;
                //    foreach (var y in listTipos)
                //    {
                //        var valor = raw.FirstOrDefault(z => z.noEco.Equals(x) && z.descripcion.Equals(y));
                //        ar[listTipos.IndexOf(y)+1] = Convert.ToDecimal(valor==null?"0":valor.importe).ToString("C0");
                //    }
                //    data.Add(ar);
                //}
                var listTiposFinal = new List<string>();
                listTiposFinal.Add("NO. ECO");
                listTiposFinal.AddRange(listTipos);
                listTiposFinal.Add("TOTAL");
                listTiposFinal.Add("TOTAL SIN OVERHAUL");
                listTiposFinal.Add("HORAS");
                listTiposFinal.Add("COSTO HORARIO");


                //dynamic expando = new ExpandoObject();
                //AddProperty(expando, "Language", "English");
                //table table-condensed table-hover table-striped text-center
                var table = "<table id='grid_Main' class='easyui-datagrid' style='min-height:300px;'>";
                table += "<thead class='bg-table-header'>";
                table += "  <tr>";
                int c = 0;
                DataTable dtReporte = new DataTable();

                foreach (var i in listTiposFinal)
                {
                    var columnName = i.Replace(" ", "");
                    columnName = columnName.Replace(".", "");

                    dtReporte.Columns.Add(columnName);

                    if (i.Equals("NO. ECO"))
                        table += "      <th data-options=\"field:'" + i + "',width:140,align:'center'\" >";
                    else if (i.Equals("COSTO HORARIO"))
                        table += "      <th data-options=\"field:'" + i + "',width:150,align:'center'\" >";
                    else if (i.Equals("MATERIALES"))
                        table += "      <th data-options=\"field:'" + i + "',width:100\" >";
                    else if (i.Equals("MAQUINARIA"))
                        table += "      <th data-options=\"field:'" + i + "',width:110\" >";
                    else if (i.Equals("ADMINISTRATIVOS"))
                        table += "      <th data-options=\"field:'" + i + "',width:140\" >";
                    else if (i.Equals("ACTIVO FIJO"))
                        table += "      <th data-options=\"field:'" + i + "',width:100\" >";
                    else if (i.Equals("TOTAL"))
                        table += "      <th data-options=\"field:'" + i + "',width:100\" >";
                    else if (i.Equals("TOTAL SIN OVERHAUL"))
                        table += "      <th data-options=\"field:'" + i + "',width:150\" >";
                    else if (i.Equals("HORAS"))
                        table += "      <th data-options=\"field:'" + i + "',width:60\" >";
                    else if (i.Equals("COSTO HORARIO"))
                        table += "      <th data-options=\"field:'" + i + "',width:130\" >";
                    else
                        table += "      <th data-options=\"field:'" + i + "',width:170\" >";
                    table += "          " + i;
                    table += "      </th>";
                }
                table += "  </tr>";
                table += "</thead>";
                table += "<tbody>";
                table += "  <tr style='background: orange;'>";
                table += "      <td>";
                table += "      Total General: ";
                table += "      </td>";
                decimal totalGeneral = 0;
                decimal totalGeneralsoh = 0;
                DataRow toInsert = dtReporte.NewRow();
                toInsert[0] = "Total General:";
                int counterData = 0;
                foreach (var y in listTipos)
                {

                    decimal total = raw.Where(z => z.descripcion.Equals(y)).Sum(z => Convert.ToDecimal(z.importe == null ? "0" : z.importe));
                    decimal totalsoh = rawsoh.Where(z => z.descripcion.Equals(y)).Sum(z => Convert.ToDecimal(z.importe == null ? "0" : z.importe));
                    table += "      <td >";
                    if (total != 0)
                    {
                        table += "          <span class='clsTotal' style='float:right;'>" + total.ToString("C0") + "</span>";
                    }
                    else
                    {
                        total = 0;
                        table += "          <span class='clsTotal' style='float:right;'>" + total.ToString("C0") + "</span>";
                    }
                    table += "      </td>";

                    totalGeneral += total;
                    totalGeneralsoh += totalsoh;
                    counterData += 1;
                    toInsert[counterData] = total.ToString("C0");

                }


                decimal totalHorasTrabajo = 1;

                if (obj.area == 0 && obj.idTipo != 0)
                {
                    List<string> tipoID = new List<string>();
                    tipoID.Add(obj.idTipo.ToString());
                    totalHorasTrabajo = capturaHorometroFactoryServices.getCapturaHorometroServices().getHorasRangoFecha(fechainicio, fechafin, obj.idTipo).Sum(x => x.HorasTrabajo);
                }
                else
                {
                    totalHorasTrabajo = capturaHorometroFactoryServices.getCapturaHorometroServices().getDataTableByRangeDate(fechainicio, fechafin, listNoEco.ToList()).Sum(x => x.HorasTrabajo);
                }


                table += "      <td class='colorTotal'>";
                table += "          <span class='clsTotal' style='float:right;'>" + totalGeneral.ToString("C0") + "</span>";
                counterData += 1;
                toInsert[counterData] = totalGeneral.ToString("C0");
                table += "      </td >";
                table += "      <td class='colorTotal'>";
                table += "          <span class='clsTotal' style='float:right;'>" + totalGeneralsoh.ToString("C0") + "</span>";
                counterData += 1;
                toInsert[counterData] = totalGeneralsoh.ToString("C0");
                table += "      </td >";
                table += "      <td class='colorTotal'>";
                table += "          <span class='clsTotal' style='float:right;'>" + totalHorasTrabajo.ToString("0,0", CultureInfo.InvariantCulture) + "</span>";
                counterData += 1;
                toInsert[counterData] = totalHorasTrabajo.ToString("0,0", CultureInfo.InvariantCulture);
                table += "      </td>";
                table += "      <td class='colorTotal'>";
                table += "          <span class='clsTotal' style='float:right;padding-right: 21px;'>" + (totalHorasTrabajo > 0 ? (totalGeneralsoh / totalHorasTrabajo).ToString("C2") : "$0") + "</span>";
                counterData += 1;
                toInsert[counterData] = (totalHorasTrabajo > 0 ? (totalGeneralsoh / totalHorasTrabajo).ToString("C2") : "$0");
                dtReporte.Rows.Add(toInsert);
                table += "      </td>";
                table += "  </tr>";



                foreach (var x in listNoEco)
                {
                    DataRow Content = dtReporte.NewRow();
                    table += "  <tr>";
                    //string[] ar = new string[listTipos.Count + 1];
                    //ar[0] = x;
                    table += "      <td>";
                    //  table += "          " + x;

                    if (obj.area == 0 && obj.idTipo != 0 && centroCostos == "")
                    {
                        var tiposMaquina = repGastosMaquina.FillCboGrupoMaquinaria(obj.idTipo).Select(h => new { Value = h.id, Text = h.descripcion, Prefijo = h.tipoMaquina });
                        var filtroGrupo = tiposMaquina.FirstOrDefault(g => g.Text.Equals(x));
                        var idGrupo = filtroGrupo.Value;
                        table += "<span  class='eveRefreshTable' data-area=" + idGrupo + " >" + x + "</span>";

                    }
                    else
                    {

                        table += "<span >" + x + "</span>";
                    }
                    Content[0] = x;
                    table += "      </td>";
                    decimal total = 0;
                    decimal totalsoh = 0;
                    int countCells = 1;
                    foreach (var y in listTipos)
                    {
                        table += "      <td>";

                        var valor = raw.FirstOrDefault(z => z.noEco.Equals(x) && z.descripcion.Equals(y));
                        var valorsoh = rawsoh.FirstOrDefault(z => z.noEco.Equals(x) && z.descripcion.Equals(y));
                        if (valor != null)
                        {
                            if (obj.area == 0 && obj.idTipo!=0)
                            {
                                table += "      <span  data-area='" + valor.area + "' data-cuenta='" + valor.cuenta + "' data-tipo='" + valor.tipoInsumo + "' data-tipoS='" + y + "' data-valor='" + Convert.ToDecimal(valor == null ? "0" : valor.importe).ToString("C0") + "' data-noeco='" + x + "' style='float:right;'>    " + Convert.ToDecimal(valor == null ? "0" : valor.importe).ToString("C0") + "</span>";

                            }
                            else
                            {
                                table += "      <span class='eveOpenModal' data-area='" + valor.area + "' data-cuenta='" + valor.cuenta + "' data-tipo='" + valor.tipoInsumo + "' data-tipoS='" + y + "' data-valor='" + Convert.ToDecimal(valor == null ? "0" : valor.importe).ToString("C0") + "' data-noeco='" + x + "' style='float:right;'>    " + Convert.ToDecimal(valor == null ? "0" : valor.importe).ToString("C0") + "</span>";

                            }
                            Content[countCells] = Convert.ToDecimal(valor == null ? "0" : valor.importe).ToString("C0");
                            countCells += 1;
                            //table += "      <span class='eveOpenModal' data-area='" + valor.area + "' data-cuenta='" + valor.cuenta + "' data-tipo='" + valor.tipoInsumo + "' data-tipoS='" + y + "' data-valor='" + Convert.ToDecimal(valor == null ? "0" : valor.importe).ToString("C0") + "' data-noeco='" + x + "' style='float:right;'>    " + Convert.ToDecimal(valor == null ? "0" : valor.importe).ToString("C0") + "</span>";
                        }
                        else
                        {
                            Content[countCells] = "$0";
                            countCells += 1;
                            table += "<span style='float:right;'>$0</span>";
                        }
                        //ar[listTipos.IndexOf(y) + 1] = Convert.ToDecimal(valor == null ? "0" : valor.importe).ToString("C0");
                        table += "      </td>";
                        total += valor == null ? 0 : Convert.ToDecimal(valor.importe);
                        totalsoh += valorsoh == null ? 0 : Convert.ToDecimal(valorsoh.importe);
                    }
                    //Total
                    table += "      <td>";
                    table += "          <span class='clsTotal' style='float:right;'>" + total.ToString("C0") + "</span>";
                    Content[countCells] = total.ToString("C0");
                    countCells += 1;
                    table += "      </td>";
                    //Total sin overhaul

                    table += "      <td>";
                    table += "          <span class='clsTotal' style='float:right;'>" + totalsoh.ToString("C0") + "</span>";
                    Content[countCells] = totalsoh.ToString("C0");
                    countCells += 1;
                    table += "      </td>";
                    //Horas
                    decimal horas = 0;
                    if (obj.area == 0 && obj.idTipo!=0)
                    {
                        var horometros = capturaHorometroFactoryServices.getCapturaHorometroServices().getTableByRangeDateTipo(fechainicio, fechafin, x);
                        horas = horometros.Sum(z => z.HorasTrabajo);
                    }
                    else
                    {
                        var horometros = capturaHorometroFactoryServices.getCapturaHorometroServices().getDataTableByRangeDate(x, fechainicio, fechafin);
                        horas = horometros.Sum(z => z.HorasTrabajo);
                    }




                    table += "      <td>";
                    /*String.Format("{0:n0}", ) .ToString("0,0", CultureInfo.InvariantCulture)*/
                    table += "          <span class='clsTotal' style='float:right;'>" + String.Format("{0:#,##0.##}", horas) + "</span>";
                    Content[countCells] = String.Format("{0:#,##0.##}", horas);
                    countCells += 1;
                    table += "      </td>";
                    //Costo horario
                    table += "      <td>";
                    table += "          <span class='clsTotal' style='float:right;padding-right: 21px;'>" + (horas > 0 ? (totalsoh / horas).ToString("C2") : "$0") + "</span>";
                    Content[countCells] = (horas > 0 ? (totalsoh / horas).ToString("C2") : "$0");
                    countCells += 1;
                    table += "      </td>";
                    //data.Add(ar);
                    table += "  </tr>";

                    dtReporte.Rows.Add(Content);
                    Grafica.Add(new GraficaComparativaDTO
                    {
                        Descripcion = x,
                        CostoHorario = (horas > 0 ? (totalsoh / horas) : 0)
                    });


                }

                Session["dtReporteComparativaTipos"] = dtReporte;
                table += "</tbody>";
                table += "</table>";

                result.Add("Grafica", Grafica);
                result.Add("table", table);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            if (guardar==true)
            {
                Session["resultComparativaTipos"] = result;
            }
 
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getTableStructSesionValidate()
        {
            var validar = Session["resultComparativaTipos"]==null?false:true;
            var result = new Dictionary<string, object>();
            result.Add(SUCCESS, validar);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getTableStructSesion()
        {
            var result = (Dictionary<string, object>)Session["resultComparativaTipos"];
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getTableGrupos(RepGastosFiltrosDTO obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var noEco = Request.QueryString["noEco"];
                RepGastosMaquinaDAO repGastosMaquina = new RepGastosMaquinaDAO();

                var res = repComparativaTiposFactoryServices.getComparativoTiposService().getGrupoInsumos(obj);

                result.Add(ROWS, res.Select(x => new { economico = noEco, descripcion = x.descripcion, importe = Convert.ToDecimal(x.importe).ToString("C0"), id = x.tipoInsumo, area = obj.area, cuenta = obj.cuenta, tipo = obj.idTipo }));


                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getTableInsumos(RepGastosFiltrosDTO obj)
        {
            var noEco = Request.QueryString["noEco"];
            var result = new Dictionary<string, object>();
            try
            {
                RepGastosMaquinaDAO repGastosMaquina = new RepGastosMaquinaDAO();

                var res = repComparativaTiposFactoryServices.getComparativoTiposService().getInsumos(obj);

                result.Add(ROWS, res.Select(x => new { economico = noEco, descripcion = x.descripcion, importe = Convert.ToDecimal(x.importe).ToString("C0"), id = x.tipoInsumo, fecha = x.fecha }));


                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public static void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }
    }
}