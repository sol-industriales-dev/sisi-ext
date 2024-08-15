using Core.DTO.Enkontrol.Alamcen;
using Data.Factory.Enkontrol.Almacen;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Infrastructure.Utils;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Enkontrol.Controllers.Almacen
{
    public class ValuacionController : BaseController
    {
        ValuacionFactoryServices vfs;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            vfs = new ValuacionFactoryServices();
            base.OnActionExecuting(filterContext);
        }
        // GET: Enkontrol/Valuacion
        #region Entrada
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult getValuacion(List<chkAlmacenDTO> lstGrupo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                lstGrupo = lstGrupo ?? new List<chkAlmacenDTO>();
                var lstValuacion = (List<ValuacionDTO>)Session["lstValuacion"];
                var lst = new List<ValuacionDTO>();
                if (lstValuacion == null)
                {
                    Session["lstCC"] = new List<ValuacionDTO>();
                    lstValuacion = vfs.getVSerice().getValuacion(lstGrupo);
                    Session["lstValuacion"] = lstValuacion;
                    lst = lstValuacion;
                }
                else
                {
                    lst = filtrarValuacionesDesdeChbox(lstValuacion, lstGrupo);
                }
                var total = lst.Sum(s => s.importe);
                var compania = convertValuacionToCompania(lst);
                var almacen = convertValuacionToAlmacen(lst);
                var grupo = convertValuacionToGrupo(lst);
                result.Add("total", total);
                result.Add("compania", compania);
                result.Add("almacen", almacen);
                result.Add("grupo", grupo);
                result.Add(SUCCESS, total > 0);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        #region Convert
        List<ValuacionDTO> filtrarValuacionesDesdeChbox(List<ValuacionDTO> lstValuacion, List<chkAlmacenDTO> lstGrupo)
        {
            var lst = (from valuacion in lstValuacion
                       let almacen = lstGrupo.SelectMany(g => g.almacen).Where(a => a.Equals(valuacion.almacen)).FirstOrDefault()
                       let insumo = lstGrupo.SelectMany(g => g.grupo).Where(a => a.compania.Equals(valuacion.compania))
                                                                      .Where(a => a.grupo.Equals(valuacion.grupo))
                                                                      .Where(a => a.tipo.Equals(valuacion.tipo))
                                                                      .FirstOrDefault() ?? new Insumogrupo()
                       let tipo = insumo.tipo
                       let grupo = insumo.grupo
                       let compania = insumo.compania
                       where compania > 0 && grupo > 0 && tipo > 0 && almacen > 0
                       select new ValuacionDTO()
                       {
                           compania = compania,
                           almacen = almacen,
                           grupo = grupo,
                           tipo = tipo,
                           descripcion = valuacion.descripcion,
                           insumo = valuacion.insumo,
                           descInsumo = valuacion.descInsumo,
                           cantidad = valuacion.cantidad,
                           importe = valuacion.importe,
                           costo = (valuacion.importe / valuacion.cantidad) ?? 0,
                           nomAlmacen = valuacion.nomAlmacen
                       }).ToList();
            return lst;
        }
        List<chkValuacion> convertValuacionToCompania(List<ValuacionDTO> lstValuacion)
        {
            var compania = lstValuacion.GroupBy(g => g.compania).Select(c => new chkValuacion
                    {
                        label = c.Key.Equals(1) ? "Construplan" : "Arrendadora",
                        Key = c.Key,
                        total = c.Sum(s => s.importe) ?? 0
                    }).OrderByDescending(o => o.total).ToList();
            return compania;
        }
        List<chkValuacion> convertValuacionToAlmacen(List<ValuacionDTO> lstValuacion)
        {
            var almacen = lstValuacion.GroupBy(g => g.almacen).Select(c => new chkValuacion()
            {
                label = c.FirstOrDefault().nomAlmacen,
                Key = c.Key,
                total = c.Sum(s => s.importe) ?? 0,
            }).OrderByDescending(o => o.total).ToList();
            return almacen;
        }
        List<chkValuacion> convertValuacionToGrupo(List<ValuacionDTO> lstValuacion)
        {
            var grupo = lstValuacion.GroupBy(g => g.descripcion).Select(c => new chkValuacion()
            {
                label = c.Key,
                Key = c.FirstOrDefault().grupo,
                total = c.Sum(s => s.importe) ?? 0
            }).OrderByDescending(o => o.total).ToList();
            return grupo;
        }
        #endregion
        #region excel
        public ActionResult getBusqExpors(List<chkAlmacenDTO> lstGrupo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstCC = (List<ValuacionDTO>)Session["lstCC"];
                var esVacioCC = lstCC.Count.Equals(0);
                var lstValuacion = (List<ValuacionDTO>)Session["lstValuacion"];
                var lst = filtrarValuacionesDesdeChbox(lstValuacion, lstGrupo);
                result.Add(SUCCESS, lst.Count > 0);
                Session["lstExport"] = lst;
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                Session["busq"] = null;
                Session["lstExport"] = null;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public MemoryStream setExport()
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                var Valuación = package.Workbook.Worksheets.Add("Dashboard Inventario");
                ExcelRange cols = Valuación.Cells["A:G"];
                var lst = ((List<ValuacionDTO>)Session["lstExport"]).ToArray();
                Valuación.Cells["A1"].LoadFromCollection(lst.Select(s => s.almacen.ToString()).ToList());
                Valuación.Cells["B1"].LoadFromCollection(lst.Select(s => s.tipo.ToString()).ToList());
                Valuación.Cells["C1"].LoadFromCollection(lst.Select(s => s.grupo.ToString()).ToList());
                Valuación.Cells["D1"].LoadFromCollection(lst.Select(s => s.insumo.ToString()).ToList());
                Valuación.Cells["E1"].LoadFromCollection(lst.Select(s => string.Format("{0:C2}", s.importe)).ToList());
                Valuación.Cells["F1"].LoadFromCollection(lst.Select(s => s.cantidad.ToString()).ToList());
                Valuación.Cells["G1"].LoadFromCollection(lst.Select(s => s.costo.ToString("C2")).ToList());
                Valuación.Cells["H1"].LoadFromCollection(lst.Select(s => s.nomAlmacen).ToList());
                Valuación.Cells["I1"].LoadFromCollection(lst.Select(s => s.descripcion).ToList());
                Valuación.Cells["J1"].LoadFromCollection(lst.Select(s => s.descInsumo).ToList());
                Valuación.Cells["K1"].LoadFromCollection(lst.Select(s => s.compania.Equals(1) ? "Construplan" : "Arrendadora").ToList());

                Valuación.InsertRow(1, 1);
                Valuación.Cells["M1"].Value = "Valuación de entrada a ";
                Valuación.Cells["N1"].Value = DateTime.Now.ToShortDateString();
                Valuación.Cells["A1"].Value = "Almacen";
                Valuación.Cells["B1"].Value = "Tipo";
                Valuación.Cells["C1"].Value = "Grúpo";
                Valuación.Cells["D1"].Value = "Innsumo";
                Valuación.Cells["E1"].Value = "Importe";
                Valuación.Cells["F1"].Value = "Cantidad";
                Valuación.Cells["G1"].Value = "Costo";
                Valuación.Cells["H1"].Value = "Almacen";
                Valuación.Cells["I1"].Value = "Grupo";
                Valuación.Cells["J1"].Value = "Insumo";
                Valuación.Cells["K1"].Value = "Compañía";
                using (var rng = Valuación.Cells["A1:L1"])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.Font.Color.SetColor(Color.Black);
                    rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(0, 0, 100, 0);
                }
                Valuación.Cells[Valuación.Dimension.Address].AutoFitColumns();
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
        #endregion
        #region checkbox
        public ActionResult getAlamcenes(List<int> lstCompania)
        {
            var result = new Dictionary<string, object>();
            try
            {
                lstCompania = lstCompania ?? new List<int>();
                var lstAlmacen = (List<chkAlmacenDTO>)Session["lstAlmacen"];
                if (lstAlmacen == null)
                {
                    lstAlmacen = vfs.getVSerice().getAlamences(lstCompania);
                    Session["lstAlmacen"] = lstAlmacen;
                    result.Add("lstAlmacen", lstAlmacen);
                }
                else
                {
                    var lst = lstAlmacen
                        .Select(a => new chkAlmacenDTO()
                        {
                            almacen = a.almacen,
                            grupo = a.grupo.Select(s => s.compania).Intersect(lstCompania).Select(s => new Insumogrupo { compania = s, grupo = 0 }).ToList(),
                            Text = a.Text,
                            Value = a.Value
                        })
                    .Where(a => a.grupo.Count > 0)
                    .ToList();
                    result.Add("lstAlmacen", lst);
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
        public ActionResult getGruposInsumos(List<chkAlmacenDTO> lstAlmacenes)
        {
            var result = new Dictionary<string, object>();
            try
            {
                lstAlmacenes = lstAlmacenes ?? new List<chkAlmacenDTO>();
                var lstGrupo = (List<chkAlmacenDTO>)Session["lstGrupo"];
                if (lstGrupo == null)
                {
                    lstGrupo = vfs.getVSerice().getGruposInsumos(lstAlmacenes);
                    Session["lstGrupo"] = lstGrupo;
                    result.Add("lstGrupo", lstGrupo);
                }
                else
                {
                    var lst = lstGrupo
                        .Select(g => new chkAlmacenDTO()
                        {
                            almacen = lstAlmacenes.SelectMany(s => s.almacen).Intersect(g.almacen).ToList(),
                            grupo = g.grupo.Where(wg =>
                                lstAlmacenes.Any(wa =>
                                    wa.grupo.Any(wag => wag.compania.Equals(wg.compania)
                                ))).Select(s => new Insumogrupo { compania = s.compania, grupo = s.grupo, tipo = s.tipo }).ToList(),
                            Text = g.Text,
                            Value = g.Value
                        })
                        .Where(w => w.almacen.Count > 0)
                        .Where(w => w.grupo.Count > 0)
                        .ToList();
                    result.Add("lstGrupo", lst);
                }
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        #endregion
        #endregion
        #region Salida
        public ActionResult Salidas()
        {
            return View();
        }
        public ActionResult getValuacionSalida(List<int> almacen, List<int> periodo, DateTime fecha)
        {
            var result = new Dictionary<string, object>();
            try
            {
                almacen = almacen ?? new List<int>();
                periodo = periodo ?? new List<int>();

                //var hoy = DateTime.Now;
                var lstValuacion = (List<ValuacionDTO>)Session["lstValuacionSalida"];

                if (lstValuacion != null && lstValuacion.FirstOrDefault().fecha.Year != fecha.Year)
                {
                    lstValuacion = null;
                }

                if (lstValuacion == null)
                {
                    lstValuacion = vfs.getVSerice().getValuacionSalida(fecha);
                    Session["lstValuacionSalida"] = lstValuacion;

                    //En almacen 1 General se restan 4 por no trabajar en domingo
                    var lstAlmacenes = lstValuacion.GroupBy(g => new { g.almacen, g.fecha.Month }).Select(s => new
                    {
                        key = s.Key.almacen,
                        label = s.FirstOrDefault(a => a.almacen.Equals(s.Key.almacen)).nomAlmacen,
                        salidas = (decimal)s.GroupBy(g => g.fecha.Date).Count(),
                        total = fecha.Month.Equals(s.Key.Month) ? fecha.Day : DateTime.DaysInMonth(fecha.Year, s.Key.Month) - (s.Key.almacen.Equals(1) ? 4 : 0)
                    }).GroupBy(g => g.key).Select(s => new
                    {
                        key = s.Key,
                        label = s.FirstOrDefault(a => a.key.Equals(s.Key)).label,
                        total = (s.Sum(ss => ss.salidas) / s.Sum(ss => ss.total)) * 100
                    }).OrderByDescending(o => o.total).ToList();

                    var lstPeriodo = lstValuacion.GroupBy(g => new { g.almacen, g.fecha.Month }).Select(s => new
                    {
                        key = s.Key.Month,
                        salidas = (decimal)s.GroupBy(g => g.fecha.Date).Count(),
                        total = fecha.Month.Equals(s.Key.Month) ? fecha.Day : DateTime.DaysInMonth(fecha.Year, s.Key.Month) - (s.Key.almacen.Equals(1) ? 4 : 0)
                    }).GroupBy(g => g.key).Select(s => new
                    {
                        key = s.Key,
                        label = new DateTime(fecha.Year, s.Key, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("es")).ToUpper(),
                        total = (s.Sum(ss => ss.salidas) / s.Sum(ss => ss.total)) * 100
                    }).OrderBy(o => o.key).ToList();

                    result.Add("lstPeriodo", lstPeriodo);
                    result.Add("lstAlmacenes", lstAlmacenes);
                    result.Add(SUCCESS, true);
                }
                else
                {
                    var lst = lstValuacion.Select(s => new ValuacionDTO()
                    {
                        almacen = almacen.Any(a => a.Equals(s.almacen)) ? s.almacen : 0,
                        periodo = periodo.Any(p => p.Equals(s.periodo)) ? s.periodo : 0,
                        importe = s.importe,
                        nomAlmacen = s.nomAlmacen,
                        descripcion = s.descripcion,
                        tipo_mov = s.tipo_mov,
                        fecha = s.fecha
                    }).Where(w => w.almacen > 0 && w.periodo > 0).ToList();

                    var lstAlmacenes = lst.GroupBy(g => new { g.almacen, g.fecha.Month }).Select(s => new
                    {
                        key = s.Key.almacen,
                        label = s.FirstOrDefault(a => a.almacen.Equals(s.Key.almacen)).nomAlmacen,
                        salidas = (decimal)s.GroupBy(g => g.fecha.Date).Count(),
                        total = fecha.Month.Equals(s.Key.Month) ? fecha.Day : DateTime.DaysInMonth(fecha.Year, s.Key.Month) - (s.Key.almacen.Equals(1) ? 4 : 0)
                    }).GroupBy(g => g.key).Select(s => new
                    {
                        key = s.Key,
                        label = s.FirstOrDefault(a => a.key.Equals(s.Key)).label,
                        total = (s.Sum(ss => ss.salidas) / s.Sum(ss => ss.total)) * 100
                    }).OrderByDescending(o => o.total).ToList();

                    var lstPeriodo = lst.GroupBy(g => new { g.almacen, g.fecha.Month }).Select(s => new
                    {
                        key = s.Key.Month,
                        salidas = (decimal)s.GroupBy(g => g.fecha.Date).Count(),
                        total = fecha.Month.Equals(s.Key.Month) ? fecha.Day : DateTime.DaysInMonth(fecha.Year, s.Key.Month) - (s.Key.almacen.Equals(1) ? 4 : 0)
                    }).GroupBy(g => g.key).Select(s => new
                    {
                        key = s.Key,
                        label = new DateTime(fecha.Year, s.Key, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("es")).ToUpper(),
                        total = (s.Sum(ss => ss.salidas) / s.Sum(ss => ss.total)) * 100
                    }).OrderBy(o => o.key).ToList();

                    result.Add("lstPeriodo", lstPeriodo);
                    result.Add("lstAlmacenes", lstAlmacenes);
                    result.Add(SUCCESS, true);
                }
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #region chekbox
        public ActionResult getPeriodos(List<chkAlmacenSalidaDTO> lstFront, DateTime fecha)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstPeridos = vfs.getVSerice().getPeriodos(fecha);

                result.Add("lstPeridos", lstPeridos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getAlmacenesSalida(List<chkAlmacenSalidaDTO> lstAlmacen, DateTime fecha)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstAlmacenes = vfs.getVSerice().getAlmacenesSalida(fecha);
                result.Add("lstAlmacenes", lstAlmacenes);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region excel
        public ActionResult getBusqSalidaExpors(List<int> almacen, List<int> periodo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstValuacion = (List<ValuacionDTO>)Session["lstValuacionSalida"];
                var lst = lstValuacion.Select(s => new ValuacionDTO()
                    {
                        almacen = almacen.Any(a => a.Equals(s.almacen)) ? s.almacen : 0,
                        periodo = periodo.Any(p => p.Equals(s.periodo)) ? s.periodo : 0,
                        importe = s.importe,
                        insumo = s.insumo,
                        tipo = s.tipo,
                        grupo = s.grupo,
                        descInsumo = s.descInsumo,
                        nomAlmacen = s.nomAlmacen,
                        descripcion = s.descripcion,
                        compania = s.compania,
                        cc = s.cc,
                        fecha = s.fecha
                    })
                    .Where(w => w.almacen > 0)
                    .Where(w => w.periodo > 0)
                    .ToList();
                result.Add(SUCCESS, true);
                Session["lstExport"] = lst;
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                Session["busq"] = null;
                Session["lstExport"] = null;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public MemoryStream setSalidaExport()
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                #region periodo
                var periodo = package.Workbook.Worksheets.Add("Salida por periodo");
                ExcelRange cols3 = periodo.Cells["A:E"];
                var hoy = DateTime.Now;
                var lst = ((List<ValuacionDTO>)Session["lstExport"]).ToList();

                var periodos = lst
                    .GroupBy(g => new { g.almacen, g.fecha.Month })
                    .Select(s => new
                    {
                        key = s.Key.Month,
                        salidas = (decimal)s.GroupBy(g => g.fecha.Date).Count(),
                        total = hoy.Month.Equals(s.Key.Month) ? hoy.Day : DateTime.DaysInMonth(hoy.Year, s.Key.Month) - (s.Key.almacen.Equals(1) ? 4 : 0)
                    })
                .GroupBy(g => g.key)
                .Select(s => new
                {
                    key = s.Key,
                    label = new DateTime(hoy.Year, s.Key, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("es")).ToUpper(),
                    salidas = s.Sum(ss => ss.salidas) / (decimal)s.Count(),
                    total = s.Sum(ss => ss.total) / (decimal)s.Count(),
                    cumplimiento = s.Sum(ss => ss.salidas) / s.Sum(ss => ss.total),
                })
                .OrderBy(o => o.key)
                .ThenBy(o => o.label)
                .ToList();


                periodo.Cells["A1"].LoadFromCollection(periodos.Select(s => s.label));
                periodo.Cells["B1"].LoadFromCollection(periodos.Select(s => decimal.Round(s.salidas, 2).ToString()));
                periodo.Cells["C1"].LoadFromCollection(periodos.Select(s => decimal.Round(s.total, 2).ToString()));
                periodo.Cells["D1"].LoadFromCollection(periodos.Select(s => s.cumplimiento.ToString("P2")));

                periodo.InsertRow(1, 1);
                periodo.Cells["G1"].Value = "Fecha";
                periodo.Cells["H1"].Value = hoy.ToShortDateString();
                periodo.Cells["A1"].Value = "Mes";
                periodo.Cells["B1"].Value = "Salidas";
                periodo.Cells["C1"].Value = "Total";
                periodo.Cells["D1"].Value = "Cumplimiento";
                using (var rng = periodo.Cells["A1:D1"])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.Font.Color.SetColor(Color.Black);
                    rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(0, 247, 109, 54);
                }
                periodo.Cells[periodo.Dimension.Address].AutoFitColumns();
                #endregion
                #region Almacen
                var Valuación = package.Workbook.Worksheets.Add("Salida por almacen");
                var almacenes = lst
                    .GroupBy(g => new { g.almacen, g.fecha.Month })
                    .Select(s => new
                    {
                        key = s.Key.almacen,
                        label = s.FirstOrDefault(a => a.almacen.Equals(s.Key.almacen)).nomAlmacen,
                        salidas = (decimal)s.GroupBy(g => g.fecha.Date).Count(),
                        total = hoy.Month.Equals(s.Key.Month) ? hoy.Day : DateTime.DaysInMonth(hoy.Year, s.Key.Month) - (s.Key.almacen.Equals(1) ? 4 : 0)
                    }).GroupBy(g => g.key).Select(s => new
                    {
                        key = s.Key,
                        label = s.FirstOrDefault(a => a.key.Equals(s.Key)).label,
                        salidas = s.Sum(ss => ss.salidas) / (decimal)s.Count(),
                        total = s.Sum(ss => ss.total) / (decimal)s.Count(),
                        cumplimiento = (s.Sum(ss => ss.salidas) / s.Sum(ss => ss.total))
                    }).OrderByDescending(o => o.total).ToList();

                ExcelRange cols2 = Valuación.Cells["A:D"];
                Valuación.Cells["A1"].LoadFromCollection(almacenes.Select(s => s.label));
                Valuación.Cells["B1"].LoadFromCollection(almacenes.Select(s => decimal.Round(s.salidas, 2).ToString()));
                Valuación.Cells["C1"].LoadFromCollection(almacenes.Select(s => decimal.Round(s.total, 2).ToString()));
                Valuación.Cells["D1"].LoadFromCollection(almacenes.Select(s => s.cumplimiento.ToString("P2")));

                Valuación.InsertRow(1, 1);
                Valuación.Cells["E1"].Value = "Fecha a ";
                Valuación.Cells["F1"].Value = hoy.ToShortDateString();
                Valuación.Cells["A1"].Value = "Proyecto";
                Valuación.Cells["B1"].Value = "Salidas";
                Valuación.Cells["C1"].Value = "Total";
                Valuación.Cells["D1"].Value = "Cumplimiento";
                using (var rng = Valuación.Cells["A1:D1"])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.Font.Color.SetColor(Color.Black);
                    rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(0, 247, 109, 54);
                }
                Valuación.Cells[Valuación.Dimension.Address].AutoFitColumns();
                #endregion
                #region registro
                var registros = package.Workbook.Worksheets.Add("Salida por registros");
                var lstRegistro = lst.OrderBy(o => o.fecha).ThenBy(o => o.almacen).ToList();

                ExcelRange cols4 = registros.Cells["A:F"];
                registros.Cells["A1"].LoadFromCollection(lstRegistro.Select(s => s.compania.Equals(1) ? "Construplan" : "Arrendadora"));
                registros.Cells["B1"].LoadFromCollection(lstRegistro.Select(s => s.cc));
                registros.Cells["C1"].LoadFromCollection(lstRegistro.Select(s => s.almacen.ToString()));
                registros.Cells["D1"].LoadFromCollection(lstRegistro.Select(s => s.nomAlmacen));
                registros.Cells["E1"].LoadFromCollection(lstRegistro.Select(s => s.fecha.ToShortDateString()));
                registros.Cells["F1"].LoadFromCollection(lstRegistro.Select(s => s.periodo.ToString()));
                registros.Cells["G1"].LoadFromCollection(lstRegistro.Select(s => (s.importe ?? 0).ToString("C2")));

                registros.InsertRow(1, 1);
                registros.Cells["H1"].Value = "Fecha a ";
                registros.Cells["I1"].Value = hoy.ToShortDateString();
                registros.Cells["A1"].Value = "Compañía";
                registros.Cells["B1"].Value = "CC";
                registros.Cells["C1"].Value = "Almacen";
                registros.Cells["D1"].Value = "Almacen";
                registros.Cells["E1"].Value = "Fecha";
                registros.Cells["F1"].Value = "Periodo";
                registros.Cells["G1"].Value = "Importe";
                using (var rng = registros.Cells["A1:G1"])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.Font.Color.SetColor(Color.Black);
                    rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(0, 247, 109, 54);
                }
                registros.Cells[registros.Dimension.Address].AutoFitColumns();
                #endregion
                package.Compression = CompressionLevel.BestSpeed;
                List<byte[]> lista = new List<byte[]>();
                using (var exportData = new MemoryStream())
                {
                    this.Response.Clear();
                    package.SaveAs(exportData);
                    lista.Add(exportData.ToArray());
                    this.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    this.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "Salida de almacen.xlsx"));
                    this.Response.BinaryWrite(exportData.ToArray());
                    this.Response.End();
                    return exportData;
                }
            }
        }
        #endregion
        #endregion
    }
}