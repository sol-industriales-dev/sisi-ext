using Core.DTO.Maquinaria.Reporte.RepAnalisisUtilizacion;
using Core.DTO.Utils.Excel;
using Core.Enum;
using Core.Enum.Maquinaria.ConciliacionHorometros;
using Core.Enum.Multiempresa;
using Data.Factory.Maquinaria.Catalogos;
using Data.Factory.Maquinaria.Reporte;
using Infrastructure.Utils;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Maquinaria.Reportes
{
    public class RepAnalisisUtilizacionController : BaseController
    {
        RepAnalisisUtilizacionFactorySerrvices aufs;
        CentroCostosFactoryServices ccfs;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            aufs = new RepAnalisisUtilizacionFactorySerrvices();
            ccfs = new CentroCostosFactoryServices();
            base.OnActionExecuting(filterContext);
        }
        // GET: RepAnalisisUtilizacion
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult getAnalisis(BusqAnalisiDTO busq)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = aufs.getUsoService().getAnalisis(busq);
                result.Add("data", lst);
                result.Add("totalMX", lst.Sum(s => s.totalMX));
                result.Add("totalUSD", lst.Sum(s => s.totalUSD));
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getBusqExpors(BusqAnalisiDTO busq)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = aufs.getUsoService().getAnalisis(busq);
                result.Add("data", lst);
                result.Add(SUCCESS, true);
                Session["busq"] = busq;
                Session["lstAnalisis"] = lst;
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                Session["busq"] = null;
                Session["lstAnalisis"] = null;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public MemoryStream setExport()
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                var busq = (BusqAnalisiDTO)Session["busq"];
                var Conciliacion = package.Workbook.Worksheets.Add("Análisis de Utilización");
                ExcelRange cols = Conciliacion.Cells["A:L"];
                var lst = ((List<AnalisisDTO>)Session["lstAnalisis"]).ToArray();
                var no = Enumerable.Range(1, lst.Count()).ToArray();
                var cc = ccfs.getCentroCostosService().getEntityCCConstruplan(busq.cc);
                int n = 7;
                for (int i = 0; i < lst.Count(); i++)
                {

                    Conciliacion.Cells[n, 1].Value = i + 1;
                    Conciliacion.Cells[n, 2].Value = lst[i].noEco;
                    Conciliacion.Cells[n, 3].Value = lst[i].grupo;
                    Conciliacion.Cells[n, 4].Value = lst[i].modelo;
                    Conciliacion.Cells[n, 5].Value = lst[i].hi;
                    Conciliacion.Cells[n, 5].Style.Numberformat.Format = "0.00";
                    Conciliacion.Cells[n, 6].Value = lst[i].hf;
                    Conciliacion.Cells[n, 6].Style.Numberformat.Format = "0.00";
                    Conciliacion.Cells[n, 7].Value = lst[i].ht;
                    Conciliacion.Cells[n, 7].Style.Numberformat.Format = "0.00";
                    Conciliacion.Cells[n, 8].Value = lst[i].promSem;
                    Conciliacion.Cells[n, 8].Style.Numberformat.Format = "0.00";
                    Conciliacion.Cells[n, 9].Value = lst[i].totalUSD;
                    Conciliacion.Cells[n, 9].Style.Numberformat.Format = "$#,##0.00";
                    Conciliacion.Cells[n, 10].Value = lst[i].totalMX;
                    Conciliacion.Cells[n, 10].Style.Numberformat.Format = "$#,##0.00";

                    n++;
                }

                /*  Conciliacion.Cells["A6"].LoadFromCollection(no.Select(s => s.ToString()).ToList());
                  Conciliacion.Cells["B6"].LoadFromCollection(lst.Select(s => s.noEco).ToList());
                  Conciliacion.Cells["C6"].LoadFromCollection(lst.Select(s => s.grupo).ToList());
                  Conciliacion.Cells["D6"].LoadFromCollection(lst.Select(s => s.modelo).ToList());
                  Conciliacion.Cells["E6"].LoadFromCollection(lst.Select(s => s.hi.ToString()).ToList(),);
                  Conciliacion.Cells["F6"].LoadFromCollection(lst.Select(s => s.hf.ToString()).ToList());
                  Conciliacion.Cells["G6"].LoadFromCollection(lst.Select(s => s.ht.ToString()).ToList());
                  Conciliacion.Cells["H6"].LoadFromCollection(lst.Select(s => (s.promSem / 100).ToString()).ToList());
                  Conciliacion.Cells["I6"].LoadFromCollection(lst.Select(s => s.totalUSD.ToString()).ToList());*
                  Conciliacion.Cells["J6"].LoadFromCollection(lst.Select(s => s.totalMX.ToString()).ToList());*/

                // Conciliacion.InsertRow(1, 1);
                Conciliacion.Cells["D2"].Value = "Análisis de Utilización";
                Conciliacion.Cells["I3"].Value = "DEL:";
                Conciliacion.Cells["J3"].Value = busq.ini.ToShortDateString();
                Conciliacion.Cells["I4"].Value = "AL:";
                Conciliacion.Cells["J4"].Value = busq.fin.ToShortDateString();
                Conciliacion.Cells["C4"].Value = "PROYECTO";
                Conciliacion.Cells["D4"].Value = string.Format("{0}-{1}", cc.areaCuenta, cc.descripcion);
                Conciliacion.Cells["A6"].Value = "No.";
                Conciliacion.Cells["B6"].Value = "Centro Costo";
                Conciliacion.Cells["C6"].Value = "Grúpo";
                Conciliacion.Cells["D6"].Value = "Modelo";
                Conciliacion.Cells["E6"].Value = "Horometro Inicial";
                Conciliacion.Cells["F6"].Value = "Horometro Final";
                Conciliacion.Cells["G6"].Value = "Horometro Efectivo";
                Conciliacion.Cells["H6"].Value = "Promedio horas por semana";
                Conciliacion.Cells["I6"].Value = "Total Renta USD";
                Conciliacion.Cells["J6"].Value = "Total Renta MX";
                var ultimo = lst.Count() + 7;
                Conciliacion.Cells[string.Format("H{0}", ultimo)].Value = "Total";
                Conciliacion.Cells[string.Format("I{0}", ultimo)].Value = lst.Sum(s => s.totalUSD);

                Conciliacion.Cells[string.Format("I{0}", ultimo)].Style.Numberformat.Format = "$#,##0.00";
                Conciliacion.Cells[string.Format("J{0}", ultimo)].Value = lst.Sum(s => s.totalMX);
                Conciliacion.Cells[string.Format("J{0}", ultimo)].Style.Numberformat.Format = "$#,##0.00";


                using (var rng = Conciliacion.Cells["D2"])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.Font.Color.SetColor(Color.White);
                    rng.Style.Font.Size = 16;
                    rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.Black);
                }
                using (var rng = Conciliacion.Cells["A6:J6"])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.Font.Color.SetColor(Color.Black);
                    rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(0, 0, 100, 0);
                }
                using (var rng = Conciliacion.Cells[string.Format("H{0}", ultimo)])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.Font.Color.SetColor(Color.Black);
                    rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(0, 0, 100, 0);
                }
                Conciliacion.Cells[Conciliacion.Dimension.Address].AutoFitColumns();
                package.Compression = CompressionLevel.BestSpeed;
                List<byte[]> lista = new List<byte[]>();
                using (var exportData = new MemoryStream())
                {
                    this.Response.Clear();
                    package.SaveAs(exportData);
                    lista.Add(exportData.ToArray());
                    this.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    this.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", package.Workbook.Worksheets.FirstOrDefault().Name + ".xlsx"));
                    this.Response.BinaryWrite(exportData.ToArray());
                    this.Response.End();
                    return exportData;
                }
            }
        }
        #region combobx
        public ActionResult cboAC()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, aufs.getUsoService().cboAC());
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}