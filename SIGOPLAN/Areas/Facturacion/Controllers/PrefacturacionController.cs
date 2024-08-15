using Core.Entity.Facturacion.Prefacturacion;
using Core.Enum.Facuración;
using Data.Factory.Facturacion;
using Data.Factory.Facturacion.Prefacturacion;
using Data.Factory.Maquinaria.Catalogos;
using Data.Factory.Principal.Archivos;
using Newtonsoft.Json;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Text.RegularExpressions;
using Core.Entity.Administrativo.Contabilidad.Facturas;
using Core.DTO.Facturacion.Prefactura.Insumos;
using Core.DAO.Facturacion.Enkontrol;
using Data.Factory.Facturacion.Enkontrol;
using Core.DTO.Facturacion;

namespace SIGOPLAN.Areas.Facturacion.Controllers
{
    public class PrefacturacionController : BaseController
    {
        PrefacturacionFactoryServices PrefacturacionFactoryServices;
        CentroCostosFactoryServices centroCostosFactoryServices;
        RepPrefacturacionFactoryService RepPrefacturacionFactoryService;
        FilePrefacturacionFactoryService FilePrefacturacionFactoryService;
        CapImporteFactoryService CapImporteFactoryService;
        ArchivoFactoryServices ArchivoFS;
        Data.Factory.RecursosHumanos.ReportesRH.ReportesRHFactoryServices reportesRHFactoryServices;
        FacturaFactoryService facturafs;
        IFacturasSPDAO facturasSPInterfaz = new FacturasSPFactoryService().getFacturasSPFactoryService();
        IFacturasSPDAO facturasEKInterfaz = new FacturasSPFactoryService().getFacturasEKFactoryService();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            PrefacturacionFactoryServices = new PrefacturacionFactoryServices();
            centroCostosFactoryServices = new CentroCostosFactoryServices();
            RepPrefacturacionFactoryService = new RepPrefacturacionFactoryService();
            FilePrefacturacionFactoryService = new FilePrefacturacionFactoryService();
            CapImporteFactoryService = new CapImporteFactoryService();
            ArchivoFS = new ArchivoFactoryServices();
            reportesRHFactoryServices = new Data.Factory.RecursosHumanos.ReportesRH.ReportesRHFactoryServices();
            facturafs = new FacturaFactoryService();
            base.OnActionExecuting(filterContext);
        }
        // GET: Facturacion/Prefacturacion
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Insumos()
        {
            return View();
        }

        public ActionResult PrefacturaCLONE()
        {
            return View();
        }

        public ActionResult savePrefactura(tblF_RepPrefactura obj, List<tblF_CapPrefactura> lst, List<tblF_CapImporte> lstImpuesto)
        {
            var reporte = RepPrefacturacionFactoryService.getRepPrefacturacionService().saveRepPrefactura(obj);
            foreach (var item in lst)
            {
                item.idRepPrefactura = reporte.id;
                var guardado = PrefacturacionFactoryServices.getPrefacturacionServices().savePrefactura(item);
            }
            foreach (var item in lstImpuesto)
            {
                item.idReporte = reporte.id;
                var guardado = CapImporteFactoryService.getCapImporteFactoryService().saveRepPrefactura(item);
            }
            return Json(reporte, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActualizaEstatus(int id, int estatus)
        {
            var obj = RepPrefacturacionFactoryService.getRepPrefacturacionService().ActualizaEstatus(id, estatus);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getTablaPrefactura(DateTime inicio, DateTime fin,  string CC)
        {
            var response = new Dictionary<string, object>();
            var restabla = PrefacturacionFactoryServices.getPrefacturacionServices().getPrefactura(inicio, fin, CC)
                .Select(x => new
                {
                    id = x.id,
                    Acciones = "",
                    Folio = "",
                    CC = x.CC+" - "+centroCostosFactoryServices.getCentroCostosService().getNombreCC(x.CC),
                    Fecha = x.Fecha.ToString("yyyy-MM-dd"),
                    Nombre = x.Nombre,
                    RFC = x.RFC,
                    Estado = x.Estado
                }).ToList();
            var lstEnEspera = restabla.Where(x => x.Estado == (int)PrefacturaciónEnum.EnEspera);
            var lstAceptado = restabla.Where(x => x.Estado == (int)PrefacturaciónEnum.Aceptado);
            var lstRechzado = restabla.Where(x => x.Estado == (int)PrefacturaciónEnum.Rechzado);
            response.Add("lstEnEspera", lstEnEspera);
            response.Add("lstAceptado", lstAceptado);
            response.Add("lstRechzado", lstRechzado);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getPrefactura(int id)
        {
            var response = new Dictionary<string, object>();
            var obj = RepPrefacturacionFactoryService.getRepPrefacturacionService().getPrefactura(id).FirstOrDefault();
            var restabla = PrefacturacionFactoryServices.getPrefacturacionServices().getPrefactura(id).OrderBy(x=> x.Renglon);
            var lstImpuesto = CapImporteFactoryService.getCapImporteFactoryService().getImportePorReporte(id).OrderBy(x => x.Renglon);
            response.Add("obj", obj);
            response.Add("restabla", restabla);
            response.Add("lstImpuesto", lstImpuesto);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getUltimaPrefacturaCliente(string nombre, string moneda, string cc)
        {
            var response = new Dictionary<string, object>();
            try
            {
                var obj = RepPrefacturacionFactoryService.getRepPrefacturacionService().getUltimaPrefacturaCliente(nombre, moneda, cc);
                var restabla = PrefacturacionFactoryServices.getPrefacturacionServices().getPrefactura(obj.id).OrderBy(x => x.Renglon);
                var lstImpuesto = CapImporteFactoryService.getCapImporteFactoryService().getImportePorReporte(obj.id).OrderBy(x => x.Renglon);
                response.Add("obj", obj);
                response.Add("restabla", restabla);
                response.Add("lstImpuesto", lstImpuesto);
                response.Add(SUCCESS, true);

            }
            catch (Exception)
            {
                response.Add(SUCCESS, false);
            }            
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult setFolio()
        {
            try
            {
                var folio = RepPrefacturacionFactoryService.getRepPrefacturacionService().getPrefactura()
                .OrderByDescending(x => x.id)
                .FirstOrDefault();

                return Json(folio.id, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(0, JsonRequestBehavior.AllowGet);
            }
            
        }

        public ActionResult getObjInsumo(DateTime fecha, string cc, int insumo)
        {
            var restabla = PrefacturacionFactoryServices.getPrefacturacionServices().getObjInsumo(fecha, cc, insumo);
            return Json(restabla, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getOrdenCompra(DateTime inicio, DateTime fin, string cc)
        {
            var restabla = PrefacturacionFactoryServices.getPrefacturacionServices().getlstOrdenCompra(inicio, fin, cc)
                .Select(x => new
                {
                    Partida = x.partida,
                    Numero = x.numero,
                    Fecha = x.fecha_entrega.ToString("yyyy-MM-dd"),
                    Insumo = x.insumo,
                    Unidad = x.descripcion,
                    Cantidad = x.cantidad,
                    Precio = String.Format("{0:C2}", x.precio),
                    Importe = String.Format("{0:C2}", x.importe)
                });
            return Json(restabla, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetObjCliente(int id)
        {
            var objCliente = RepPrefacturacionFactoryService.getRepPrefacturacionService().objCliente(id);
            return Json(objCliente, JsonRequestBehavior.AllowGet);
        }

        #region Archivos
        [HttpPost]
        public ActionResult SubirNuevoArchivo()
        {
            var result = new Dictionary<string, object>();
            //try
            //{
                var id = JsonConvert.DeserializeObject<int>(Request.Form["id"].ToString());
                var TipoArchivo = JsonConvert.DeserializeObject<int>(Request.Form["TipoArchivo"].ToString());
                HttpPostedFileBase file = Request.Files["fupAdjunto"];


                if (Request.Files.Count > 0)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase archivo = Request.Files[i];
                        string extension = Path.GetExtension(archivo.FileName);
                        string nombreOriginal = archivo.FileName;
                        //string FileName = nombreOriginal + id + extension;
                        string FileName = id + "_" + nombreOriginal;

                        string Ruta = ArchivoFS.getArchivo().getUrlDelServidor(1) + FileName;
                        var archivoExiste = FilePrefacturacionFactoryService.getFilePrefacturaService().SaveArchivo(archivo, Ruta);

                        var archivosNotasCredito = new tblF_FilePrefactura();
                        if (archivoExiste)
                        {
                            archivosNotasCredito.FechaSubida = DateTime.Now;
                            archivosNotasCredito.id = 0;
                            archivosNotasCredito.nombreArchivo = nombreOriginal;
                            archivosNotasCredito.idRepFactura = id;
                            archivosNotasCredito.rutaArchivo = Ruta;
                            archivosNotasCredito.tipoArchivo = TipoArchivo;
                            archivosNotasCredito.usuario = getUsuario().id;

                            FilePrefacturacionFactoryService.getFilePrefacturaService().Guardar(archivosNotasCredito);
                        }


                    }
                }

                result.Add(SUCCESS, true);

            //}
            //catch (Exception e)
            //{
            //    result.Add(MESSAGE, e.Message);
            //    result.Add(SUCCESS, false);
            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public FileResult getFileDownload()
        {
            int id = Convert.ToInt32(Request.QueryString["id"]);

            var archivo = FilePrefacturacionFactoryService.getFilePrefacturaService().getlistaByID(id);

            var nombre = archivo.nombreArchivo;
            var Ruta = archivo.rutaArchivo;

            return File(Ruta, "multipart/form-data", nombre);
        }

        public ActionResult getListaArchivos(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = FilePrefacturacionFactoryService.getFilePrefacturaService().getlistaByPrefactura(id);
                var res = data.Select(x => new
                {
                    id = x.id,
                    Acciones = "",
                    tipo = x.tipoArchivo == 1 ? "Contrato" : "Orden de compra",
                    nombArchivo = x.nombreArchivo,
                    Fecha = x.FechaSubida.ToShortDateString()
                }
                    );

                result.Add("ListaArchivos", res);
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

        #region Combobox
        public ActionResult cboTipoInsumo()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var list = PrefacturacionFactoryServices.getPrefacturacionServices().getlstTipoInsumo();
                result.Add(ITEMS, list
                    .Select(x => new { Text = x.descripcion, Value = x.tipo_insumo })
                    .OrderBy(x => x.Value));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult cboConceptoImporte()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var list = PrefacturacionFactoryServices.getPrefacturacionServices().CboConceptoImporte();
                result.Add(ITEMS, list
                    .Select(x => new { Text = x.id, Value = x.Concepto })
                    .OrderBy(x => x.Text));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboClienteNombre(string term)
        {
            try
            {
                var filteredItems = RepPrefacturacionFactoryService.getRepPrefacturacionService().FillComboClienteNombre(term)
                    .Select(x => new { label = x.Text.PadLeft(3, '0'), id = x.Value }).ToList();
                return Json(filteredItems.OrderBy(x => x.id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
            
        }
        public ActionResult FillComboUsocfdi()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cbo = PrefacturacionFactoryServices.getPrefacturacionServices().FillComboUsocfdi();
                result.Add(ITEMS, cbo);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public FileResult GetExcel(int index)
        {
            using (var package = new ExcelPackage())
            {                
                var prefactura = RepPrefacturacionFactoryService.getRepPrefacturacionService().getPrefactura(index).OrderByDescending(x => x.id).FirstOrDefault();
                //var centroCosto = reportesRHFactoryServices.getReportesRHService().getListaCCRH().FirstOrDefault(x => x.Value == prefactura.CC);

                List<ComboDTO> MetodoPagoSAT = PrefacturacionFactoryServices.getPrefacturacionServices().FillComboMetodoPagoSat()["items"] as List<ComboDTO>;
                //string txtMetodoPagoSAT = MetodoPagoSAT.FirstOrDefault(x => x.Value == prefactura.MetodoPagoSAT.ToString()).Text;
                var auxMetodoPagoSAT = MetodoPagoSAT.FirstOrDefault(x => x.Value == prefactura.MetodoPagoSAT.ToString());
                string txtMetodoPagoSAT = "";
                if (auxMetodoPagoSAT != null) txtMetodoPagoSAT = auxMetodoPagoSAT.Text;

                var centroCosto = centroCostosFactoryServices.getCentroCostosService().ListCC().FirstOrDefault(x => x.Value == prefactura.CC);
                var MetodoPago = facturafs.getFacturaService().FillComboMetodoPago().FirstOrDefault(x => x.Value == prefactura.MetodoPago);
                var Usocfdi = PrefacturacionFactoryServices.getPrefacturacionServices().FillComboUsocfdi().FirstOrDefault(x => x.Value == prefactura.Usocfdi).Text;
                var rows = PrefacturacionFactoryServices.getPrefacturacionServices().getPrefactura(index).OrderBy(x=> x.Renglon);
                var rowsTotales = PrefacturacionFactoryServices.getPrefacturacionServices().getTotales(index).OrderBy(x => x.Renglon);
                var prefacturaExcel = package.Workbook.Worksheets.Add("PREFACTURA " + prefactura.Folio);
                string startupPath = AppDomain.CurrentDomain.BaseDirectory;
                string imagen = startupPath + "Content\\img\\logo\\logo.png";
                System.Drawing.Image logo = System.Drawing.Image.FromFile(imagen);
                
                prefacturaExcel.Cells["A1"].Value = DateTime.Today.ToString("dd/MM/yyyy");
                prefacturaExcel.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                prefacturaExcel.Cells["G1"].Value = "SISI 1.1";
                prefacturaExcel.Cells["G1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                prefacturaExcel.Cells["C3:J3"].Merge = true;
                prefacturaExcel.Cells["C3"].Value = "Direccion de Aministración y finanzas";
                prefacturaExcel.Cells["C3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                prefacturaExcel.Cells["C3"].Style.Font.Bold = true;
                prefacturaExcel.Cells["K3"].Value = "Folio:";
                prefacturaExcel.Cells["B6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                prefacturaExcel.Cells["K4"].Value = prefactura.Folio;
                prefacturaExcel.Cells["A5:M5"].Merge = true;
                prefacturaExcel.Cells["A5"].Value = "Prefactura";                
                prefacturaExcel.Cells["A5"].Style.Font.Bold = true;
                prefacturaExcel.Cells["A5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                prefacturaExcel.Cells["A6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                prefacturaExcel.Cells["G5"].Style.Font.Bold = true;
                prefacturaExcel.Cells["B6"].Value = "Nombre:";
                prefacturaExcel.Cells["B6"].Style.Font.Bold = true;
                prefacturaExcel.Cells["B6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                prefacturaExcel.Cells["C6:H6"].Merge = true;
                prefacturaExcel.Cells["C6"].Value = prefactura.Nombre;
                prefacturaExcel.Cells["I6"].Value = "Fecha:";
                prefacturaExcel.Cells["I6"].Style.Font.Bold = true;
                prefacturaExcel.Cells["I6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                prefacturaExcel.Cells["J6:L6"].Merge = true;
                prefacturaExcel.Cells["J6"].Value = prefactura.Fecha.ToString("dd/MM/yyyy");
                prefacturaExcel.Cells["B7"].Value = "Dirección:";
                prefacturaExcel.Cells["B7"].Style.Font.Bold = true;
                prefacturaExcel.Cells["B7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                prefacturaExcel.Cells["C7:H7"].Merge = true;
                prefacturaExcel.Cells["C7"].Value = prefactura.Direccion;
                prefacturaExcel.Cells["C7"].Style.WrapText = true; 
                prefacturaExcel.Cells["I7"].Value = "C.P.:";
                prefacturaExcel.Cells["I7"].Style.Font.Bold = true;
                prefacturaExcel.Cells["I7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                prefacturaExcel.Cells["J7:L7"].Merge = true;
                prefacturaExcel.Cells["J7"].Value = prefactura.CP;
                prefacturaExcel.Cells["B8"].Value = "Ciudad:";
                prefacturaExcel.Cells["B8"].Style.Font.Bold = true;
                prefacturaExcel.Cells["B8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                prefacturaExcel.Cells["C8:H8"].Merge = true;
                prefacturaExcel.Cells["C8"].Value = prefactura.Ciudad;
                prefacturaExcel.Cells["I8"].Value = "RFC:";
                prefacturaExcel.Cells["I8"].Style.Font.Bold = true;
                prefacturaExcel.Cells["I8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                prefacturaExcel.Cells["J8:L8"].Merge = true;
                prefacturaExcel.Cells["J8"].Value = prefactura.RFC;
                prefacturaExcel.Cells["A11"].Value = "Cantidad";
                prefacturaExcel.Cells["A11"].Style.Font.Bold = true;
                prefacturaExcel.Cells["A11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                prefacturaExcel.Cells["B11"].Value = "Unidad";
                prefacturaExcel.Cells["B11"].Style.Font.Bold = true;
                prefacturaExcel.Cells["B11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                prefacturaExcel.Cells["C11:K11"].Merge = true;
                prefacturaExcel.Cells["C11"].Value = "Concepto";
                prefacturaExcel.Cells["C11"].Style.Font.Bold = true;
                prefacturaExcel.Cells["C11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                prefacturaExcel.Cells["L11"].Value = "Precio";
                prefacturaExcel.Cells["L11"].Style.Font.Bold = true;
                prefacturaExcel.Cells["L11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                prefacturaExcel.Cells["M11"].Value = "Importe";
                prefacturaExcel.Cells["M11"].Style.Font.Bold = true;
                prefacturaExcel.Cells["M11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                int i = 12;
                rows.ToList().ForEach(
                r => {
                    prefacturaExcel.Cells[string.Format("A{0}", i)].Value = r.Cantidad;
                    prefacturaExcel.Cells[string.Format("B{0}", i)].Value = r.Concepto;
                    prefacturaExcel.Cells[string.Format("C{0}:K{0}", i)].Merge = true;
                    var html = Server.UrlDecode(r.Unidad);
                    prefacturaExcel.Cells[string.Format("C{0}", i)].Value = GetPlainTextFromHtml(html);
                    prefacturaExcel.Cells[string.Format("L{0}", i)].Value = "$ " + string.Format("{0:#.0000}", r.Precio);
                    prefacturaExcel.Cells[string.Format("M{0}", i)].Value = "$ " + string.Format("{0:#.0000}", r.Importe);
                    prefacturaExcel.Cells[string.Format("L{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    prefacturaExcel.Cells[string.Format("M{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    prefacturaExcel.Cells[string.Format("A{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                    prefacturaExcel.Cells[string.Format("B{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                    prefacturaExcel.Cells[string.Format("C{0}:K{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                    prefacturaExcel.Cells[string.Format("L{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                    prefacturaExcel.Cells[string.Format("M{0}", i)].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                    i++;
                });
                prefacturaExcel.Cells[string.Format("A12:C{0}", i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                i++;
                prefacturaExcel.Cells[string.Format("A{0}:B{0}", i)].Merge = true;
                prefacturaExcel.Cells[string.Format("A{0}:B{0}", (i + 1))].Merge = true;
                prefacturaExcel.Cells[string.Format("A{0}:B{0}", (i + 2))].Merge = true;
                prefacturaExcel.Cells[string.Format("A{0}:B{0}", (i + 3))].Merge = true;
                prefacturaExcel.Cells[string.Format("A{0}", i)].Value = "Centro de costos:";
                prefacturaExcel.Cells[string.Format("A{0}", (i + 1))].Value = "Tipo de pago:";
                prefacturaExcel.Cells[string.Format("A{0}", (i + 2))].Value = "Metodo de pago:";
                prefacturaExcel.Cells[string.Format("A{0}", (i + 3))].Value = "Tipo de moneda:";
                prefacturaExcel.Cells[string.Format("A{0}", (i + 4))].Value = "Uso cfdi:";
                prefacturaExcel.Cells[string.Format("A{0}:A{1}", (i), (i + 4))].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                prefacturaExcel.Cells[string.Format("A{0}:A{1}", (i), (i + 4))].Style.Font.Bold = true;
                prefacturaExcel.Cells[string.Format("C{0}", i)].Value = string.Format("{0}-{1}", centroCosto.Value, centroCosto.Text);
                prefacturaExcel.Cells[string.Format("C{0}", (i + 1))].Value = MetodoPago.Text;
                prefacturaExcel.Cells[string.Format("C{0}", (i + 2))].Value = txtMetodoPagoSAT;
                prefacturaExcel.Cells[string.Format("C{0}", (i + 3))].Value = (prefactura.TipoMoneda == "MX" ? "PESOS MEXICANOS" : (prefactura.TipoMoneda == "DLL" ? "DOLARES" : ""));
                prefacturaExcel.Cells[string.Format("C{0}", (i + 4))].Value = Usocfdi;
                //prefacturaExcel.Cells[string.Format("K{0}:L{0}", i)].Merge = true;
                //prefacturaExcel.Cells[string.Format("K{0}:L{0}", (i + 1))].Merge = true;
                //prefacturaExcel.Cells[string.Format("K{0}:L{0}", (i + 2))].Merge = true;
                //-------------
                //prefacturaExcel.Cells[string.Format("K{0}", i)].Value = "Subtotal:";
                //prefacturaExcel.Cells[string.Format("K{0}", (i + 1))].Value = "16% I.V.A.:";
                //prefacturaExcel.Cells[string.Format("K{0}", (i + 2))].Value = "Total:";
                //prefacturaExcel.Cells[string.Format("K{0}:K{1}", (i), (i + 2))].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //prefacturaExcel.Cells[string.Format("K{0}:K{1}", (i), (i + 2))].Style.Font.Bold = true;
                //var subtotal = rows.Sum(x => x.Importe);
                //var iva = (subtotal * 16) / 100;
                //prefacturaExcel.Cells[string.Format("M{0}", i)].Value = "$ " + string.Format("{0:#.0000}", subtotal);
                //prefacturaExcel.Cells[string.Format("M{0}", (i + 1))].Value = "$ " + string.Format("{0:#.0000}", iva);
                //prefacturaExcel.Cells[string.Format("M{0}", (i + 2))].Value = "$ " + string.Format("{0:#.0000}", subtotal + iva);
                //prefacturaExcel.Cells[string.Format("M{0}:M{1}", (i), (i + 2))].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                int j = 0;
                rowsTotales.ToList().ForEach(
                r =>
                {
                    prefacturaExcel.Cells[string.Format("K{0}:L{0}", i+j)].Merge = true;
                    prefacturaExcel.Cells[string.Format("K{0}", i+j)].Value = r.Label;
                    prefacturaExcel.Cells[string.Format("K{0}:K{1}", (i), (i + j))].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    prefacturaExcel.Cells[string.Format("K{0}:K{1}", (i), (i + j))].Style.Font.Bold = true;
                    prefacturaExcel.Cells[string.Format("M{0}", i + j)].Value = r.Valor;
                    prefacturaExcel.Cells[string.Format("M{0}:M{1}", (i), (i + j))].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    j++;
                });
                //prefacturaExcel.Cells[string.Format("K{0}:K{1}", (i), (i + 2))].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //prefacturaExcel.Cells[string.Format("K{0}:K{1}", (i), (i + 2))].Style.Font.Bold = true;
                //prefacturaExcel.Cells[string.Format("M{0}:M{1}", (i), (i + 2))].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                prefacturaExcel.Cells["A11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                prefacturaExcel.Cells["B11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                prefacturaExcel.Cells["C11:K11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                prefacturaExcel.Cells["L11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                prefacturaExcel.Cells["M11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#808080");
                prefacturaExcel.Cells["A11:M11"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                prefacturaExcel.Cells["A11:M11"].Style.Fill.BackgroundColor.SetColor(colFromHex);

                prefacturaExcel.Cells[string.Format("A1:M{1}", (i), (i + 3))].Style.Font.Size = 14;
                prefacturaExcel.Cells["A5"].Style.Font.Size = 20;

                var picture = prefacturaExcel.Drawings.AddPicture("", logo);
                picture.SetPosition(1, 3, 0, 3);
                picture.SetSize(118, 63);
                prefacturaExcel.Cells["A2:M9"].Style.Border.BorderAround(ExcelBorderStyle.Thick);

                prefacturaExcel.Cells[prefacturaExcel.Dimension.Address].AutoFitColumns();
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
                    return File(exportData, "application/vnd.ms-excel");
                }
            } 
        }

        private string GetPlainTextFromHtml(string htmlString)
        {
            string htmlTagPattern = "<.*?>";
            var regexCss = new Regex("(\\<script(.+?)\\</script\\>)|(\\<style(.+?)\\</style\\>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            htmlString = regexCss.Replace(htmlString, string.Empty);
            htmlString = Regex.Replace(htmlString, htmlTagPattern, string.Empty);
            htmlString = Regex.Replace(htmlString, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
            htmlString = htmlString.Replace("&nbsp;", string.Empty);

            return htmlString;
        }
        
        #endregion

        #region CLONE PREFACTURA

        public ActionResult savePrefacturaCLONE(tblF_RepPrefactura obj, List<tblF_CapPrefactura> lst, List<tblF_CapImporte> lstImpuesto)
        {


            var dictPreFactura = facturasSPInterfaz.GuardarPrefactura(obj, lst, lstImpuesto);
            bool? exitoPrefactura = dictPreFactura["success"] as bool?;
            tblF_RepPrefactura reporte = dictPreFactura["items"] as tblF_RepPrefactura;

            if (exitoPrefactura.Value == false)
            {
                throw new Exception("Ocurrio algo mal con la prefactura");
            }

            return Json(reporte, JsonRequestBehavior.AllowGet);

            //var reporte = RepPrefacturacionFactoryService.getRepPrefacturacionService().saveRepPrefactura(obj);
            //foreach (var item in lst)
            //{
            //    item.idRepPrefactura = reporte.id;
            //    var guardado = PrefacturacionFactoryServices.getPrefacturacionServices().savePrefactura(item);    
            //}
            //foreach (var item in lstImpuesto)
            //{
            //    item.idReporte = reporte.id;
            //    var guardado = CapImporteFactoryService.getCapImporteFactoryService().saveRepPrefactura(item);
            //}
            //return Json(reporte, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ActualizaEstatusCLONE(int id, int estatus)
        {
            var obj = RepPrefacturacionFactoryService.getRepPrefacturacionService().ActualizaEstatusCLONE(id, estatus);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        #region FACTURAS EK

        public ActionResult fillComboMetodoPagoSat()
        {
            return Json(PrefacturacionFactoryServices.getPrefacturacionServices().FillComboMetodoPagoSat(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboRegimenFiscal()
        {
            return Json(PrefacturacionFactoryServices.getPrefacturacionServices().FillComboRegimenFiscal(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboTM()
        {
            return Json(PrefacturacionFactoryServices.getPrefacturacionServices().FillComboTM(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboTipoFlete()
        {
            return Json(PrefacturacionFactoryServices.getPrefacturacionServices().FillComboTipoFlete(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboCondEntrega()
        {
            return Json(PrefacturacionFactoryServices.getPrefacturacionServices().FillComboCondEntrega(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboTipoPedido()
        {
            return Json(PrefacturacionFactoryServices.getPrefacturacionServices().FillComboTipoPedido(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFormaPagoSat(string claveSat)
        {
            return Json(PrefacturacionFactoryServices.getPrefacturacionServices().GetFormaPagoSat(claveSat), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboTipoFactura()
        {
            return Json(PrefacturacionFactoryServices.getPrefacturacionServices().FillComboTipoFactura(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboSerie()
        {
            return Json(PrefacturacionFactoryServices.getPrefacturacionServices().FillComboSerie(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FIllComboInsumos()
        {
            return Json(PrefacturacionFactoryServices.getPrefacturacionServices().FIllComboInsumos(), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region CAT INSUMOS
        public ActionResult GetInsumosEK(string idInsumoSAT)
        {
            return Json(PrefacturacionFactoryServices.getPrefacturacionServices().GetInsumosEK(idInsumoSAT), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInsumosSAT(tblF_EK_InsumosSAT objFiltro)
        {
            return Json(PrefacturacionFactoryServices.getPrefacturacionServices().GetInsumosSAT(objFiltro), JsonRequestBehavior.AllowGet);
        }
        public ActionResult CrearEditarInsumos(InsumosSATDAO objInsumo, List<string> lstRel)
        {
            return Json(PrefacturacionFactoryServices.getPrefacturacionServices().CrearEditarInsumos(objInsumo, lstRel), JsonRequestBehavior.AllowGet);
        }
        public ActionResult EliminarInsumo(int idInsumo)
        {
            return Json(PrefacturacionFactoryServices.getPrefacturacionServices().EliminarInsumo(idInsumo), JsonRequestBehavior.AllowGet);
        }
        public ActionResult EliminarRelInsumo(string idInsumoSAT, string idInsumoEK)
        {
            return Json(PrefacturacionFactoryServices.getPrefacturacionServices().EliminarRelInsumo(idInsumoSAT, idInsumoEK));
        }
        public ActionResult GetAutoCompleteInsumosDesc(string term)
        {
            return Json(PrefacturacionFactoryServices.getPrefacturacionServices().GetAutoCompleteInsumosDesc(term),JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAutoCompleteInsumos(string term)
        {
            return Json(PrefacturacionFactoryServices.getPrefacturacionServices().GetAutoCompleteInsumos(term), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
    }
}