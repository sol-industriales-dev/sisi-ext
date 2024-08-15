using Core.DTO.Contabilidad;
using Core.DTO.Contabilidad.Bancos;
using Core.DTO.Principal.Generales;
using Core.Enum.Administracion.Banco;
using Data.Factory.Contabilidad;
using Data.Factory.Contabilidad.Propuesta;
using Data.Factory.Contabilidad.Reportes;
using Infrastructure.Utils;
using OfficeOpenXml;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.Contabilidad.Poliza
{
    public class BancoController : BaseController
    {
        private ChequeFactoryServices chequeFS;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            chequeFS = new ChequeFactoryServices();
            base.OnActionExecuting(filterContext);
        }
        // GET: Administrativo/Banco
        public ActionResult Movimientos()
        {
            return View();
        }
        public ActionResult _divBancoMov()
        {
            return PartialView();
        }
        public ActionResult getLstBancoMov(BusqBancoMov busq)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstBanMov = chequeFS.getChequeService().getLstBancosMov(busq);
                var esSuccess = lstBanMov.Count > 0;
                if (esSuccess)
                {
                    Session["lstBanMov"] = lstBanMov;
                    Session["busq"] = busq;
                    result.Add("lst", lstBanMov);
                    result.Add(SUCCESS, esSuccess);
                }
            }
            catch (Exception)
            {
                Session["lstBanMov"] = new List<RptBanMovDTO>();
                Session["busq"] = new BusqBancoMov();
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #region Export archivos
        public MemoryStream exportBanMov()
        {
            using (var package = new ExcelPackage())
            {
                var lstBanMov = ((List<RptBanMovDTO>)Session["lstBanMov"]).ToArray();
                var busq = (BusqBancoMov)Session["busq"];
                var titulo = string.Format("MOVIMIENTOS DE BANCOS DE {0} AL {1}", busq.min.ToShortDateString(), busq.max.ToShortDateString());
                var bancoMovimientos = package.Workbook.Worksheets.Add(titulo);
                var i = 1;
                bancoMovimientos.Cells["A1"].LoadFromCollection(lstBanMov.Select(s => s.fecha_mov.ToString("dd/MM/yyyy")));
                bancoMovimientos.Cells["B1"].LoadFromCollection(lstBanMov.Select(s => s.numero.ToString()));
                bancoMovimientos.Cells["C1"].LoadFromCollection(lstBanMov.Select(s => s.tm.ToString()));
                bancoMovimientos.Cells["D1"].LoadFromCollection(lstBanMov.Select(s => string.Empty));
                lstBanMov.ToList().ForEach(mov => bancoMovimientos.Cells[string.Format("E{0}", i++)].Value = mov.cargo - mov.abono);
                bancoMovimientos.Cells["F1"].LoadFromCollection(lstBanMov.Select(s => s.st_che));
                bancoMovimientos.Cells["G1"].LoadFromCollection(lstBanMov.Select(s => s.ipoliza.ToString()));
                bancoMovimientos.Cells["H1"].LoadFromCollection(lstBanMov.Select(s => s.itp));
                bancoMovimientos.Cells["I1"].LoadFromCollection(lstBanMov.Select(s => s.ilinea.ToString()));
                bancoMovimientos.Cells["J1"].LoadFromCollection(lstBanMov.Select(s => s.cuenta));
                bancoMovimientos.Cells["K1"].LoadFromCollection(lstBanMov.Select(s => s.descripcion));
                bancoMovimientos.Cells["L1"].LoadFromCollection(lstBanMov.Select(s => s.banco.ToString()));
                bancoMovimientos.Cells["M1"].LoadFromCollection(lstBanMov.Select(s => s.banDesc));
                bancoMovimientos.Cells["N1"].LoadFromCollection(lstBanMov.Select(s => s.ctaDesc));
                bancoMovimientos.Cells["O1"].LoadFromCollection(lstBanMov.Select(s => s.tmDesc));
                bancoMovimientos.Cells["P1"].LoadFromCollection(lstBanMov.Select(s => s.cc));
                bancoMovimientos.Cells["Q1"].LoadFromCollection(lstBanMov.Select(s => s.ccDesc));
                bancoMovimientos.Cells["R1"].LoadFromCollection(lstBanMov.Select(s => s.status_lp));

                bancoMovimientos.InsertRow(1, 1);
                bancoMovimientos.Cells["A1"].Value = "fecha_mov";
                bancoMovimientos.Cells["B1"].Value = "numero";
                bancoMovimientos.Cells["C1"].Value = "tm";
                bancoMovimientos.Cells["D1"].Value = "desc";
                bancoMovimientos.Cells["E1"].Value = "monto";
                bancoMovimientos.Cells["F1"].Value = "st_che";
                bancoMovimientos.Cells["G1"].Value = "ipoliza";
                bancoMovimientos.Cells["H1"].Value = "itp";
                bancoMovimientos.Cells["I1"].Value = "ilinea";
                bancoMovimientos.Cells["J1"].Value = "cuenta";
                bancoMovimientos.Cells["K1"].Value = "descripcion";
                bancoMovimientos.Cells["L1"].Value = "banco";
                bancoMovimientos.Cells["M1"].Value = "descripcion";
                bancoMovimientos.Cells["N1"].Value = "descripcion";
                bancoMovimientos.Cells["O1"].Value = "descripcion";
                bancoMovimientos.Cells["P1"].Value = "cc";
                bancoMovimientos.Cells["Q1"].Value = "descripcion";
                bancoMovimientos.Cells["R1"].Value = "status_lp";
                bancoMovimientos.Cells[bancoMovimientos.Dimension.Address].AutoFitColumns();
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
        #region combobox
        public ActionResult cboFormatoBanMov()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cbo = EnumExtensions.ToCombo<formatoRptBanMovEnum>();
                var esSuccess = cbo.Count() > 0;
                if (esSuccess)
                {
                    result.Add(ITEMS, cbo);
                    result.Add(SUCCESS, esSuccess);
                }
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