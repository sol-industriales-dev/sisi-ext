using Core.DAO.Contabilidad.Poliza;
using Core.DAO.Contabilidad.Propuesta;
using Core.DAO.Contabilidad.Reportes;
using Core.DAO.Maquinaria.Catalogos;
using Core.DTO;
using Core.DTO.Contabilidad;
using Core.DTO.Contabilidad.Propuesta;
using Core.DTO.Principal.Generales;
using Core.DTO.Utils.Excel;
using Core.Entity.Administrativo.Contabilidad;
using Core.Enum;
using Core.Enum.Administracion.CadenaProductiva;
using Core.Enum.Administracion.Cotizaciones;
using Core.Enum.Administracion.Propuesta;
using Core.Enum.Multiempresa;
using Data.Factory.Contabilidad;
using Data.Factory.Contabilidad.Propuesta;
using Data.Factory.Contabilidad.Reportes;
using Data.Factory.Maquinaria.Catalogos;
using Infrastructure.DTO;
using Infrastructure.Utils;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace SIGOPLAN.Areas.Administrativo.Controllers.Contabilidad.Reportes
{
    public class ReportesController : BaseController
    {
        #region Factory
        ICadenaProductivaDAO cadenaProductivaFS;
        ICentroCostosDAO centroCostosFS;
        ICatNumNafinDAO nafinFS;
        ICadenaPrincipalDAO cadenaPrincipalFS;
        ICatCCBaseDAO BaseFS;
        IPolizaDAO polizaFS;
        IReservaDAO reservaPropuestaFS;
        ICatGiroProvDAO giroProvFS;
        ICCDivisionDAO CcDivFS;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            cadenaProductivaFS = new CadenaProductivaFactoryServices().getCadenaProductivaService();
            centroCostosFS = new CentroCostosFactoryServices().getCentroCostosService();
            nafinFS = new CatNumNafinFactoryServices().getNafinService();
            cadenaPrincipalFS = new CadenaPrincipalFactoryServices().getCadenaPrincipalService();
            BaseFS = new CatCCBaseFactoryServices().getBaseServices();
            polizaFS = new PolizaFactoryServices().getPolizaService();
            reservaPropuestaFS = new ReservaFactoryServer().getReservasService();
            giroProvFS = new CatGiroProvFactoryServices().getGiroProvServices();
            CcDivFS = new CCDivisionFactoryServices().getCcDivisionService();
            base.OnActionExecuting(filterContext);
        }
        #endregion
        // GET: Administrativo/Reportes
        public ActionResult CadenaProductiva()
        {
            return View();
        }

        public ActionResult ArchivosPendientesDescarga()
        {
            return View();
        }

        public ActionResult PagoSemanal()
        {
            return View();
        }

        public ActionResult TotalSemanal()
        {
            return View();
        }

        public ActionResult CadenaObra()
        {
            return View();
        }
        public ActionResult NumeroNafin()
        {
            return View();
        }
        public ActionResult _numeroNafin()
        {
            return PartialView();
        }
        public ActionResult _correoProv()
        {
            return PartialView();
        }
        public ActionResult _propPagoProvGrupo()
        {
            return PartialView();
        }
        public ActionResult Division()
        {
            return View();
        }
        public ActionResult Linea()
        {
            return View();
        }
        public ActionResult Anticipo()
        {
            return View();
        }
        public ActionResult _btnSyncPagos()
        {
            return PartialView();
        }
        public ActionResult _mdlTotal()
        {
            return PartialView();
        }
        public ActionResult AutorizacionCadenaPrincipal()
        {
            return View();
        }
        public ActionResult getInfoVencimiento()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cc = Request.QueryString["centrocostos"].ToString();
                var tipoFactura = Convert.ToInt32(Request.QueryString["tipoFactura"].ToString());
                var res = cadenaProductivaFS.getInforVencimiento(cc, tipoFactura).OrderBy(x => x.fechaVencimiento);

                result.Add("datosTable", res.Select(x => new
                {
                    numProveedor = x.idProveedor,
                    proveedor = x.proveedor,
                    factura = x.factura,
                    fecha = x.fecha.ToShortDateString(),
                    fechaVencimiento = x.fechaVencimiento.ToShortDateString(),
                    fechaTimbrado = x.fechaTimbrado.ToShortDateString(),
                    saldoFactura = x.total.ToString("C2"),
                    centro_costos = x.centro_costos,
                    area_cuenta = x.area_cuenta,
                    orden_compra = x.orden_compra,
                    tipoCambio = x.tipoCambio == 1 ? "Pesos" : "Dlls",
                    nombCC = x.nombCC,
                    tipoMoneda = x.tipoMoneda,
                    tipoFactura = DateTime.Now > x.fechaVencimiento ? "Vencida" : "Normal",
                    seleccione = "",
                    iva = x.IVA.ToString("C2"),
                    monto = x.monto.ToString("C2"),
                    bloqueado = x.bloqueado,
                    descripcionBloqueo = x.descripcionBloqueo
                }).OrderBy(x => x.factura));

                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CadenaPorObra()
        {
            return View();
        }
        public FileResult getFileDownload()
        {
            var memStream = new MemoryStream();
            var streamWriter = new StreamWriter(memStream);
            List<DocumentoNegociableDTO> docuementeosNeg = (List<DocumentoNegociableDTO>)Session["Documento"];
            DataIFDTO listaIF = (DataIFDTO)Session["DataIFDTO"];
            var archivo = "";
            foreach (var item in docuementeosNeg)
            {
                string temp = "";
                if (item.claveIF != null)
                {
                    temp = item.noProveedor + "|" + item.noDocumento + "|" + listaIF.FechaEmision.ToString("dd/MM/yyyy") + "|" + listaIF.FechaVencimiento.ToString("dd/MM/yyyy") + "|" + item.moneda + "|" + item.monto + "|" + item.tipoDocumento + "|" + item.referencia + "|" + item.campoAdicional1 + "|" + item.campoAdicional2 + "|" + item.campoAdicional3 + "|" + item.campoAdicional4 + "|" + item.campoAdicional5 + "|" + item.claveIF;
                    streamWriter.WriteLine(temp);
                }
                else
                {
                    item.claveIF = "";
                    temp = item.noProveedor + "|" + item.noDocumento + "|||" + item.moneda + "|" + item.monto + "|" + item.tipoDocumento + "|" + item.referencia + "|" + item.campoAdicional1 + "|" + item.campoAdicional2 + "|" + item.campoAdicional3 + "|" + item.campoAdicional4 + "|" + item.campoAdicional5 + "|";
                    streamWriter.WriteLine(temp);
                }
                archivo = item.noProveedor + "_" + listaIF.FechaEmision.ToString("ddMMyyyy") + "_" + listaIF.FechaVencimiento.ToString("ddMMyyyy") + "_" + item.proveedor.Replace(" ", "_");
            }
            streamWriter.Flush();
            memStream.Seek(0, SeekOrigin.Begin);
            Response.ContentType = "text/plain";
            Response.AddHeader("content-disposition", "attachment;filename=" + archivo + ".txt");
            Response.BinaryWrite(memStream.GetBuffer());
            Response.Flush();
            Response.End();
            return null;
        }
        public FileResult getFileDownloadAplicado()
        {
            var memStream = new MemoryStream();
            var streamWriter = new StreamWriter(memStream);
            List<DocumentoNegociableDTO> docuementeosNeg = (List<DocumentoNegociableDTO>)Session["Documento"];
            DataIFDTO listaIF = (DataIFDTO)Session["DataIFDTO"];
            var archivo = "";
            foreach (var item in docuementeosNeg)
            {
                string temp = "";
                if (item.claveIF != null)
                {

                    temp = item.noProveedor + "|" + item.noDocumento + "|" + item.fechaEmision.ToString("dd/MM/yyyy") + "|" + item.fechaVencimiento.ToString("dd/MM/yyyy") + "|" + item.moneda + "|" + item.monto + "|" + item.tipoDocumento + "|" + item.referencia + "|" + item.campoAdicional1 + "|" + item.campoAdicional2 + "|" + item.campoAdicional3 + "|" + item.campoAdicional4 + "|" + item.campoAdicional5 + "|" + item.claveIF;
                    streamWriter.WriteLine(temp);
                }
                else
                {
                    item.claveIF = "";
                    temp = item.noProveedor + "|" + item.noDocumento + "|||" + item.moneda + "|" + item.monto + "|" + item.tipoDocumento + "|" + item.referencia + "|" + item.campoAdicional1 + "|" + item.campoAdicional2 + "|" + item.campoAdicional3 + "|" + item.campoAdicional4 + "|" + item.campoAdicional5 + "|";
                    streamWriter.WriteLine(temp);
                }
                archivo = item.noProveedor + "_" + item.fechaEmision.ToString("ddMMyyyy") + "_" + item.fechaVencimiento.ToString("ddMMyyyy");
            }
            streamWriter.Flush();
            memStream.Seek(0, SeekOrigin.Begin);
            Response.ContentType = "text/plain";
            Response.AddHeader("content-disposition", "attachment;filename=" + archivo + ".txt");
            Response.BinaryWrite(memStream.GetBuffer());
            Response.Flush();
            Response.End();
            return null;
        }
        public FileResult downloadProveedores()
        {
            var memStream = new MemoryStream();
            var streamWriter = new StreamWriter(memStream);
            List<DocumentoNegociableDTO> docuementeosNeg = (List<DocumentoNegociableDTO>)Session["Documento"];
            string Cadena = "No. Proveedor|No. Documento|Fecha Emisión|Fecha Vencimiento|Moneda|Monto Documento|Tipo|Referencia|Campo Adicional1|Campo Adicional2|Campo Adicional3|Campo Adicional4|Campo Adicional5|ClaveIF \n";
            streamWriter.WriteLine(Cadena);
            DataIFDTO listaIF = (DataIFDTO)Session["DataIFDTO"];

            foreach (var item in docuementeosNeg)
            {
                string temp = "";
                item.claveIF = listaIF.IF.ToString();
                temp = item.noProveedor + "|" + item.noDocumento + "|" + listaIF.FechaEmision.ToString("dd/MM/yyyy") + "|" + listaIF.FechaVencimiento.ToString("dd/MM/yyyy") + "|" + item.moneda + "|" + item.monto + "|" + item.tipoDocumento + "|" + item.referencia + "|" + item.campoAdicional1 + "|" + item.campoAdicional2 + "|" + item.campoAdicional3 + "|" + item.campoAdicional4 + "|" + item.campoAdicional5 + "|" + item.claveIF + " \n";
                streamWriter.WriteLine(temp);
            }
            streamWriter.Flush();
            memStream.Seek(0, SeekOrigin.Begin);
            Response.ContentType = "text/plain";
            Response.AddHeader("content-disposition", "attachment;filename=myFile.txt");
            Response.BinaryWrite(memStream.GetBuffer());
            Response.Flush();
            Response.End();
            return null;

        }

        public ActionResult setDatosFactura(List<tblC_CadenaProductiva> array, string centro_costos)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<DocumentoNegociableDTO> docuementeosNeg = new List<DocumentoNegociableDTO>();
                foreach (var item in array)
                {
                    var documento = cadenaProductivaFS.GetDocumentoNegociable(Convert.ToInt32(item.factura), Convert.ToInt32(item.numProveedor)).FirstOrDefault();
                    documento.tipoDocumento = Convert.ToDateTime(documento.fechaVencimiento) > DateTime.Now ? "V" : "N";
                    docuementeosNeg.Add(documento);
                }
                Session["Documento"] = docuementeosNeg.ToList();
                Session["ListaVencimiento"] = array;
                var cc = centroCostosFS.getNombreCC(Convert.ToInt32(centro_costos));
                result.Add("centro_costos", cc);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult setVariosProveedores(List<VencimientoDTO> array, string centro_costos)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var temp = new ListaProveedoresDTO();
                var temp1 = new ListaProveedoresDTO();
                temp = (ListaProveedoresDTO)Session["ListaVariosProveedores"];
                array.ForEach(x =>
                {
                    var o = cadenaProductivaFS.getMonto(x.factura.ToString(), x.numProveedor.ToString());
                    x.IVA = o.IVA;
                    x.tipoCambio = o.tipoCambio == 0 ? cadenaProductivaFS.getDolarDelDia(o.fecha) : o.tipoCambio;
                    x.monto = o.monto;
                    x.concepto = o.concepto ?? string.Empty;
                    x.factoraje = "BANORTE";
                    x.cif = 3217;
                    x.banco = "N";
                });

                var tempLista = new List<VencimientoDTO>();
                if (temp != null)
                {
                    tempLista = temp.ListVencimientoDTO;
                    tempLista.AddRange(array);
                    temp.ListVencimientoDTO = tempLista;
                    Session["ListaVariosProveedores"] = temp;
                }
                else
                {
                    temp1.ListVencimientoDTO = array;
                    Session["ListaVariosProveedores"] = temp1;
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

        public ActionResult clearProveedores()
        {
            var result = new Dictionary<string, object>();
            try
            {
                Session["ListaVariosProveedores"] = null;
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SetDataDownload(int id, string Factoraje, DateTime FechaEmision, DateTime FechaVencimiento, int? IF, string Banco)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var res = cadenaProductivaFS.getDocumentoGuardado(id);
                var docuementeosNeg = new List<DocumentoNegociableDTO>();
                foreach (var item1 in res)
                {
                    var documento = cadenaProductivaFS.GetDocumentoNegociable(Convert.ToInt32(item1.factura), Convert.ToInt32(item1.numProveedor)).FirstOrDefault();

                    documento.tipoDocumento = Factoraje;
                    documento.claveIF = IF == null ? "" : IF.ToString(); ;
                    documento.fechaEmision = FechaEmision;
                    documento.fechaVencimiento = FechaVencimiento;
                    docuementeosNeg.Add(documento);

                }
                DataIFDTO DTO = new DataIFDTO();
                DTO.FechaEmision = FechaEmision;
                DTO.FechaVencimiento = FechaVencimiento;
                DTO.IF = IF;
                Session["Documento"] = docuementeosNeg.ToList();
                Session["DataIFDTO"] = DTO;
                cadenaProductivaFS.setDocumentoGuardado(id, Factoraje, FechaEmision, FechaVencimiento, IF, Banco);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SetDataDownloadAplicado(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var res = cadenaProductivaFS.getDocumentoGuardado(id);
                var docuementeosNeg = new List<DocumentoNegociableDTO>();
                foreach (var item1 in res)
                {
                    var documento = new DocumentoNegociableDTO();
                    documento.noProveedor = AsignaNumNafin(Convert.ToInt32(item1.numProveedor));
                    documento.noDocumento = item1.factura.ToString();
                    documento.fechaEmision = item1.fecha;
                    documento.fechaVencimiento = item1.fechaVencimiento;
                    documento.moneda = item1.tipoMoneda;
                    documento.monto = item1.saldoFactura;
                    documento.tipoDocumento = item1.factoraje;
                    documento.campoAdicional1 = "";
                    documento.campoAdicional2 = "";
                    documento.campoAdicional3 = "";
                    documento.campoAdicional4 = "";
                    documento.campoAdicional5 = "";
                    documento.claveIF = item1.factoraje.Equals("N") && item1.banco.Equals("---") ? "3217" : item1.cif.ToString();
                    docuementeosNeg.Add(documento);
                }
                Session["Documento"] = docuementeosNeg.ToList();
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public static string NombreValidoArchivo(string name)
        {
            string invalidChars = Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidReStr = string.Format(@"[{0}]+", invalidChars);
            string replace = Regex.Replace(name, invalidReStr, "_").Replace(";", "").Replace(",", "");
            return replace;
        }
        public FileResult getExcelDownload()
        {
            var excel = new ExcelUtilities();
            var Sheets = new List<excelSheetDTO>();
            #region Tab1
            var obj = new tblC_CadenaPrincipal();
            var id = Convert.ToInt32(Request.QueryString["id"]);
            var total = Convert.ToDecimal(Request.QueryString["monto"]);
            var fecha = Convert.ToDateTime(Request.QueryString["fecha"]);
            var lstAnticipo = cadenaProductivaFS.getLstAnticipo(Request.QueryString["prov"]);
            if (id == 0)
                obj = lstAnticipo.Where(w => w.anticipo.Equals(total) && w.fechaVencimiento.Equals(fecha))
                        .Select(x => new tblC_CadenaPrincipal
                        {
                            banco = x.banco,
                            centro_costos = x.centro_costos,
                            estatus = x.estatus,
                            factoraje = x.factoraje,
                            fecha = x.fecha,
                            fechaVencimiento = x.fechaVencimiento,
                            id = 0,
                            nombCC = x.nombCC,
                            numNafin = x.numNafin,
                            numProveedor = x.numProveedor,
                            pagado = true,
                            proveedor = x.proveedor,
                            total = x.anticipo
                        }).FirstOrDefault();
            else
                obj = cadenaPrincipalFS.GetDocumento(id);
            var sheetBanco = new excelSheetDTO() { name = string.Format("{0} {1}", NombreValidoArchivo(obj.proveedor), obj.fechaVencimiento.ToString("dd-MM-yy")) };
            var rows = new List<excelRowDTO>()
            {
                new excelRowDTO() {
                    cells = new List<excelCellDTO>(){
                        new excelCellDTO{ text = getEmpresaNombre(), autoWidthFit=false, fill=true, border=false,colSpan=10,rowSpan=0 },
                    }
                    
                },
                new excelRowDTO() {
                    cells = new List<excelCellDTO>(){
                        new excelCellDTO{ text="RELACION DE FACTURAS OPERADAS EN CADENAS PRODUCTIVAS NAFIN", autoWidthFit=false, fill=true, border=false,colSpan=10,rowSpan=0},
                    }
                    
                },
                new excelRowDTO() {
                    cells = new List<excelCellDTO>(){
                        new excelCellDTO{ text="", autoWidthFit=false, fill=true, border=false,colSpan=10,rowSpan=0},
                    }
                },
                new excelRowDTO() {
                    cells = new List<excelCellDTO>(){
                        new excelCellDTO{ text="", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                        new excelCellDTO{ text="FACTURA", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
                        new excelCellDTO{ text="PROVEEDOR", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
                        new excelCellDTO{ text= string.Empty, autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
                        new excelCellDTO{ text="OBRA", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
                        new excelCellDTO{ text="IMPORTE C/IVA", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
                        new excelCellDTO{ text="IMPORTE C/IVA", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
                        new excelCellDTO{ text="OPERADA", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
                        new excelCellDTO{ text="VENCIMIENTO", autoWidthFit=true, fill=true, border=true,colSpan=0,rowSpan=0},
                    },
                },
                new excelRowDTO() {
                    cells = new List<excelCellDTO>(){
                        new excelCellDTO{ text="", autoWidthFit=false, fill=true, border=false,colSpan=10,rowSpan=0},
                    }
                },
                
            };
            var lst = new List<tblC_CadenaProductiva>();
            if (obj.id == 0)
                lst.Add(new tblC_CadenaProductiva()
                {
                    banco = obj.banco,
                    IVA = obj.total * .16m,
                    centro_costos = obj.centro_costos,
                    cif = "",
                    concepto = "Anticipo",
                    estatus = obj.estatus,
                    factoraje = obj.factoraje,
                    factura = string.Empty,
                    fecha = obj.fecha,
                    fechaVencimiento = obj.fechaVencimiento,
                    id = 0,
                    idPrincipal = 0,
                    monto = obj.total - (obj.total * .16m),
                    nombCC = obj.nombCC,
                    numNafin = obj.numNafin,
                    numProveedor = obj.numProveedor,
                    pagado = true,
                    proveedor = obj.proveedor,
                    reasignado = false,
                    saldoFactura = obj.total,
                    tipoCambio = 1,
                    tipoMoneda = obj.numProveedor.ParseInt() > 9000 ? 2 : 1
                });
            else
                lst = cadenaProductivaFS.GetDocumentoPorPrincipal(obj.id);
            rows.AddRange(
                lst.
                Select(x => new excelRowDTO()
                {
                    cells = new List<excelCellDTO>(){
                        new excelCellDTO{ text="", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                        new excelCellDTO{ text= x.factura, autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0, borderType = x.factura == lst.FirstOrDefault().factura ? 1 : x.factura == lst.OrderByDescending(y => y.factura).FirstOrDefault().factura ? 2 : 3},
                        new excelCellDTO{ text= x.proveedor, autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0, borderType = x.factura == lst.FirstOrDefault().factura ? 4 : x.factura == lst.OrderByDescending(y => y.factura).FirstOrDefault().factura ? 5 : 0},
                        new excelCellDTO{ text = tipoCC(x.centro_costos, x.nombCC).Equals(x.nombCC) ? string.Empty : x.nombCC, autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0, borderType = x.factura == lst.FirstOrDefault().factura ? 4 : x.factura == lst.OrderByDescending(y => y.factura).FirstOrDefault().factura ? 5 : 0},
                        new excelCellDTO{ text=  tipoCC(x.centro_costos, x.nombCC).ToUpper(), autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0 , borderType = x.factura == lst.FirstOrDefault().factura ? 4 : x.factura == lst.OrderByDescending(y => y.factura).FirstOrDefault().factura ? 5 : 0},
                        new excelCellDTO{ text= x.saldoFactura.ToString(""), autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0 , borderType = x.factura == lst.FirstOrDefault().factura ? 4 : x.factura == lst.OrderByDescending(y => y.factura).FirstOrDefault().factura ? 5 : 0, formatType = 1},
                        new excelCellDTO{ text= string.Empty, autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0 , borderType = x.factura == lst.FirstOrDefault().factura ? 4 : x.factura == lst.OrderByDescending(y => y.factura).FirstOrDefault().factura ? 5 : 0},
                        new excelCellDTO{ text= x.fecha.ToShortDateString(), autoWidthFit=false, fill=false, border=false,colSpan=0,rowSpan=0 , borderType = x.factura == lst.FirstOrDefault().factura ? 4 : x.factura == lst.OrderByDescending(y => y.factura).FirstOrDefault().factura ? 5 : 0},
                        new excelCellDTO{ text= x.fechaVencimiento.ToShortDateString(), autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0 , borderType = x.factura == lst.FirstOrDefault().factura ? 6 : x.factura == lst.OrderByDescending(y => y.factura).FirstOrDefault().factura ? 7 : 8},
                        new excelCellDTO{ text= x.factoraje, autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType =  0},
                    }
                }));
            rows.Add(
                new excelRowDTO()
                {
                    cells = new List<excelCellDTO>(){
                        new excelCellDTO{ text= obj.total.ToString(""), autoWidthFit=false, fill=true, border=false,colSpan=11,rowSpan=0, borderType = 9, formatType=1},
                        }
                }
            );
            sheetBanco.Sheet = rows;
            Sheets.Add(sheetBanco);
            #endregion
            excel.CreateExcelFileCadenaProductiva(this, Sheets, sheetBanco.name);
            return null;
        }
        string AsignaNumNafin(int numProveedor)
        {
            try
            {
                var lstNafin = nafinFS.GetLstHanilitadosNumNafin();
                var objNafin = lstNafin.Where(x => x.NumProveedor.Equals(numProveedor.ToString())).FirstOrDefault();
                return objNafin.NumNafin.Replace("\r\n", string.Empty);
            }
            catch (Exception)
            {

                return numProveedor.ToString();
            }
        }
        public ActionResult SetDataPrint(int id, VencimientoDTO obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                EstadoAutorizacionCadenaEnum estadoAutorizacion = cadenaPrincipalFS.ObtenerEstadoAutorizacionCadena(id);
                var lst = new List<VencimientoDTO>();
                if (id == 0)
                    lst.AddRange(cadenaProductivaFS.getLstAnticipo(obj.idProveedor.ToString()).Where(w => w.anticipo.Equals(obj.total) && w.fechaVencimiento.Equals(obj.fecha)).Select(x => new VencimientoDTO
                    {
                        banco = x.banco,
                        factura = 0,
                        proveedor = x.proveedor,
                        saldoFactura = x.anticipo.ToString(),
                        tipoMoneda = x.tipoMoneda,
                        nombCC = x.nombCC,
                        centro_costos = x.centro_costos,
                    }).ToList());
                else
                    lst = cadenaProductivaFS.GetDocumentoPorPrincipal(id)
                    .Select(x => new VencimientoDTO
                    {
                        banco = x.banco,
                        factura = Convert.ToInt32(x.factura),
                        proveedor = x.proveedor,
                        saldoFactura = x.saldoFactura.ToString(),
                        tipoMoneda = x.tipoMoneda,
                        nombCC = x.nombCC,
                        centro_costos = x.centro_costos,
                    }).ToList();

                Session["ListaVencimiento"] = lst;
                Session["estadoAutorizacion"] = estadoAutorizacion;
                Session["firma"] = cadenaPrincipalFS.ObtenerFirmaAutorizacionCadena(id, 1); ;
                Session["firmaVobo"] = cadenaPrincipalFS.ObtenerFirmaAutorizacionCadena(id, 2); ;
                Session["firmaValida"] = cadenaPrincipalFS.ObtenerFirmaAutorizacionCadena(id, 3); ;
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult setDatosID(DataIFDTO array)
        {
            var result = new Dictionary<string, object>();
            try
            {
                Session["DataIFDTO"] = array;
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Guardar(List<tblC_FacturaParcial> lst)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var temp = (ListaProveedoresDTO)Session["ListaVariosProveedores"];
                var swCif = new SwitchClass<string>
                {
                    {"BANORTE", () => "3217" },
                    {"BANAMEX", () => "6544" },
                    {"SCOTIABANK", () => "32046" },
                    {"MONEX", () => "1097745" },
                };
                var total = 0m;
                var objTem = temp.ListVencimientoDTO.FirstOrDefault();
                var reasignar = string.IsNullOrEmpty(objTem.banco) && string.IsNullOrEmpty(objTem.factoraje);
                objTem.banco = string.IsNullOrEmpty(objTem.banco) ? "BANORTE" : objTem.banco;
                objTem.factoraje = string.IsNullOrEmpty(objTem.factoraje) ? "N" : objTem.factoraje;
                if (lst != null)
                {
                    temp.ListVencimientoDTO.ForEach(cp =>
                    {
                        var anticipo = lst.FirstOrDefault(a => a.factura.ParseInt().Equals(cp.factura));
                        total += anticipo != null ? anticipo.abonado : cp.saldoFactura.unmaskDinero();
                    });
                }
                else
                {
                    total = temp.ListVencimientoDTO.Sum(s => s.saldoFactura.unmaskDinero());
                }
                var objPrincipal = cadenaPrincipalFS.Guardar(new tblC_CadenaPrincipal()
                {
                    estatus = true,
                    pagado = false,
                    numProveedor = objTem.numProveedor.ToString(),
                    centro_costos = objTem.centro_costos,
                    nombCC = objTem.nombCC,
                    proveedor = objTem.proveedor,
                    total = total,
                    numNafin = AsignaNumNafin(objTem.numProveedor),
                    factoraje = objTem.banco,
                    banco = objTem.factoraje,
                    fecha = objTem.fecha,
                    fechaVencimiento = objTem.fechaVencimiento
                });
                foreach (var item in temp.ListVencimientoDTO)
                {
                    cadenaProductivaFS.Guardar(new tblC_CadenaProductiva()
                    {
                        idPrincipal = objPrincipal.id,
                        factura = item.factura.ToString(),
                        fecha = item.fecha,
                        fechaVencimiento = item.fechaVencimiento,
                        numProveedor = item.numProveedor.ToString(),
                        proveedor = item.proveedor,
                        numNafin = objPrincipal.numNafin,
                        saldoFactura = item.saldoFactura.unmaskDinero(),
                        monto = item.monto,
                        IVA = item.IVA,
                        tipoCambio = item.tipoCambio,
                        tipoMoneda = item.tipoMoneda,
                        nombCC = item.nombCC,
                        centro_costos = item.centro_costos,
                        area_cuenta = item.area_cuenta,
                        orden_compra = item.orden_compra,
                        concepto = item.concepto ?? string.Empty,
                        banco = objTem.factoraje,
                        factoraje = objTem.banco,
                        cif = swCif.Execute(objTem.factoraje),
                        estatus = true,
                        reasignado = reasignar,
                        pagado = item.pagado,
                    });
                }
                Session["ListaVariosProveedores"] = null;
                if (lst != null)
                {
                    lst.ForEach(x =>
                    {
                        x.ultimoAbono = DateTime.Now;
                        cadenaProductivaFS.Guardar(x);
                    });
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
        public ActionResult getListaAdjuntos()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var temp = new ListaProveedoresDTO();
                temp = (ListaProveedoresDTO)Session["ListaVariosProveedores"];
                if (temp == null)
                    temp = new ListaProveedoresDTO() { ListVencimientoDTO = new List<VencimientoDTO>() };
                temp.ListVencimientoDTO.ForEach(x =>
                {
                    decimal diff = 0;
                    var total = x.monto + x.IVA;
                    var saldo = x.saldoFactura.unmaskDinero();
                    var abono = cadenaProductivaFS.GetAbono(x.factura.ToString(), x.numProveedor.ToString(), saldo, out diff);
                    if (abono.Equals(decimal.Zero))
                        diff = saldo;
                    x.fechaS = x.fecha.ToShortDateString();
                    x.fechaVencimientoS = x.fechaVencimiento.ToShortDateString();
                    x.abono = abono;
                    x.diff = diff;
                    x.total = total;
                });
                Session["ListaVariosProveedores"] = temp;
                result.Add("data", temp.ListVencimientoDTO);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            } return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInfoDocumentos()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstPal = cadenaPrincipalFS.GetDocumentosGuardados();
                var permisoVobo = cadenaPrincipalFS.TienePermisoVoBo();
                var lstResultado = lstPal.Select(x => new
                {
                    id = x.id,
                    numProveedor = x.numProveedor,
                    numNafin = AsignaNumNafin(Convert.ToInt32(x.numProveedor)),
                    proveedor = x.proveedor,
                    factoraje = x.factoraje.Equals("V") ? "Vencido" : "Normal",
                    banco = x.banco,
                    saldoFactura = x.total.ToString("C2"),
                    fechaS = x.fecha.ToShortDateString(),
                    fechaVencimientoS = x.fechaVencimiento.ToShortDateString(),
                    x.estadoAutorizacion,
                    permisoVobo,
                    x.comentarioRechazo
                });
                result.Add("DataSend", lstResultado);
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
        public JsonResult GetDocumentosPorAutorizar()
        {
            return Json(cadenaPrincipalFS.GetDocumentosPorAutorizar(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AutorizarDocumentos(List<int> idsDocumentosPorAutorizar)
        {
            return Json(cadenaPrincipalFS.AutorizarDocumentos(idsDocumentosPorAutorizar), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerFacturasCadena(int cadenaID)
        {
            return Json(cadenaPrincipalFS.ObtenerFacturasCadena(cadenaID), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RechazarDocumento(int cadenaID, string comentarioRechazo)
        {
            return Json(cadenaPrincipalFS.RechazarDocumento(cadenaID, comentarioRechazo), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AsignarVoBoCadena(int cadenaID)
        {
            return Json(cadenaPrincipalFS.AsignarVoBoCadena(cadenaID), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerRutaPDFFactura(string numProveedor, string factura, string cc)
        {
            return Json(cadenaPrincipalFS.ObtenerRutaPDFFactura(numProveedor, factura, cc), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerRutaXMLFactura(string numProveedor, string factura, string cc)
        {
            return Json(cadenaPrincipalFS.ObtenerRutaXMLFactura(numProveedor, factura, cc), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetPago()
        {
            var result = new Dictionary<string, object>();
            try
            {
                cadenaProductivaFS.SetPago();
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult enviarCorreosGerardo()
        {
            var result = new Dictionary<string, object>();
            try
            {

                cadenaProductivaFS.enviarCorreo();
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult enviarCorreosGerardoPropuesta()
        {
            var result = new Dictionary<string, object>();
            try
            {
                cadenaProductivaFS.enviarCorreoPropuesta();
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetFechaSync()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var hoy = cadenaProductivaFS.getUltimaFechaPago().fecha;
                result.Add("fecha", hoy.ToShortDateString() + " " + hoy.ToLongTimeString());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInfoDocumentosAplicados()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstPal = cadenaPrincipalFS.GetDocumentosAplicados()
                            .Where(p => !p.pagado).ToList();
                var lstResultado = lstPal.Select(x => new
                {
                    id = x.id,
                    numProveedor = x.numProveedor,
                    numNafin = x.numNafin,
                    proveedor = x.proveedor,
                    factoraje = x.factoraje.Equals("V") ? "Vencido" : "Normal",
                    banco = x.banco,
                    saldoFactura = x.total.ToString("C2"),
                    fechaS = x.fecha.ToShortDateString(),
                    fechaVencimientoS = x.fechaVencimiento.ToShortDateString(),
                }).ToList();
                result.Add("DataSend", lstResultado);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInfoDocumentosAplicadosPorFecha(DateTime fecha)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstPal = cadenaPrincipalFS.GetDocumentosAplicados()
                    .Where(w => DateTime.Compare(w.fechaVencimiento, fecha) == 0).ToList();
                var lstResultado = lstPal.Select(x => new
                {
                    id = x.id,
                    numProveedor = x.numProveedor,
                    numNafin = x.numNafin,
                    proveedor = x.proveedor,
                    factoraje = x.factoraje.Equals("V") ? "Vencido" : "Normal",
                    banco = x.banco,
                    saldoFactura = x.total.ToString("C2"),
                    fechaS = x.fecha.ToShortDateString(),
                    fechaVencimientoS = x.fechaVencimiento.ToShortDateString(),
                });
                result.Add("DataSend", lstResultado);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Eliminar(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = cadenaPrincipalFS.GetDocumento(id);
                if (cadenaPrincipalFS.Eliminar(obj))
                {
                    var lst = cadenaProductivaFS.GetDocumentoPorPrincipal(obj.id);
                    lst.ForEach(x => cadenaProductivaFS.Eliminar(x));
                    var lstParcial = cadenaProductivaFS.GetParcialPorPrincipal(obj.id);
                    lstParcial.ForEach(x => cadenaProductivaFS.Eliminar(x));
                }
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private decimal changeFormat(string dato)
        {
            decimal num = 0;
            string result2 = dato;
            result2 = result2.Replace("(", "-");
            result2 = result2.Replace(")", "");
            result2 = result2.Replace("$", "");
            result2 = result2.Replace(",", "");
            num = Convert.ToDecimal(result2);
            return num;
        }

        #region ResumenSemanal
        public List<tblC_CadenaProductiva> getTotal()
        {
            try
            {
                var lstRes = new List<tblC_CadenaProductiva>();
                if (cadenaProductivaFS == null)
                {
                    cadenaProductivaFS = new CadenaProductivaFactoryServices().getCadenaProductivaService();
                }
                var lstCompleta = cadenaProductivaFS.GetAllDocumentos().Where(w => !w.estatus).ToList();
                return lstCompleta;
            }
            catch (Exception)
            {
                return new List<tblC_CadenaProductiva>();
            }
        }
        public List<tblC_Linea> lstLinea()
        {
            try
            {
                if (cadenaProductivaFS == null)
                {
                    cadenaProductivaFS = new CadenaProductivaFactoryServices().getCadenaProductivaService();
                }
                return cadenaProductivaFS.lstLinea();
            }
            catch (Exception)
            {
                return new List<tblC_Linea>();
            }
        }
        public ActionResult getLstLinea()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add("data", cadenaProductivaFS.lstLinea().Select(x => new
                {
                    banco = x.banco,
                    factoraje = string.IsNullOrEmpty(x.factoraje) ? string.Empty : x.factoraje.Equals("N") ? "Normal" : "Vencido",
                    tipoMoneda = x.moneda,
                    moneda = x.moneda == 0 ? string.Empty : x.moneda == 1 ? "MX" : "DLL",
                    linea = x.linea.ToString("C2"),
                    fecha = x.fecha.ToShortDateString()
                }));
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add("data", new List<tblC_Linea>());
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public DateTime getViernes()
        {
            var hoy = DateTime.Now;
            var noSemana = numeroSemana(hoy);
            var semana = primerDíaSemana(noSemana == 52 ? hoy.AddYears(-1).Year : hoy.Year, noSemana, CultureInfo.CurrentCulture);
            return semana.AddDays(5);
        }
        public ActionResult BuscarResumen(string factoraje, string banco, DateTime fechaVencimiento, string moneda, string tipoBusq, string tipoFactura)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstVencimientoDTO = cadenaProductivaFS.GetAllDocumentos();
                var lstFactoraje = lstVencimientoDTO.Where(x => string.IsNullOrEmpty(factoraje) ? true : factoraje.Equals(x.factoraje)).ToList();
                var lstBanco = lstFactoraje.Where(x => string.IsNullOrEmpty(banco) ? true : x.cif.Equals(banco.ToString())).ToList();
                var lstMoneda = lstBanco.Where(x => string.IsNullOrEmpty(moneda) ? true : moneda.Equals(x.tipoMoneda.ToString())).ToList();
                var lstSemana = lstMoneda.Where(x => tipoBusq.Equals("0") ? x.fechaVencimiento.Year.Equals(fechaVencimiento.Year) && numeroSemana(x.fechaVencimiento) == numeroSemana(fechaVencimiento) : tipoBusq.Equals("2") ? x.fechaVencimiento == fechaVencimiento : true).ToList();
                var lstPagado = lstSemana.Where(x => string.IsNullOrEmpty(tipoFactura) || tipoFactura.Equals("S") ? !x.estatus : false).ToList();
                var lstNoPagado = lstMoneda.Where(x => string.IsNullOrEmpty(tipoFactura) || tipoFactura.Equals("N") ? x.estatus : false).ToList();
                var lst = new List<tblC_CadenaProductiva>();
                lst.AddRange(lstPagado);
                lst.AddRange(lstNoPagado);
                var lstResultado = lst.Select(x => new VencimientoDTO
                {
                    factura = string.IsNullOrEmpty(x.factura) ? 0 : x.factura.ParseInt(),
                    proveedor = x.proveedor,
                    nombCC = x.nombCC,
                    centro_costos = x.centro_costos,
                    saldoFactura = x.saldoFactura.ToString(),
                    fecha = x.fecha,
                    fechaVencimiento = x.fechaVencimiento,
                    fechaVencimientoS = x.fechaVencimiento.ToShortDateString(),
                    banco = string.IsNullOrEmpty(x.banco) ? "BANORTE" : x.banco,
                    factoraje = string.IsNullOrEmpty(x.factoraje) ? "N" : x.factoraje,
                    total = lst.Where(w => w.idPrincipal == x.idPrincipal).Sum(s => s.saldoFactura),
                    tipoMoneda = x.numProveedor.ParseInt() >= 9000 ? 2 : 1,
                    pagado = !x.estatus
                })
                .OrderBy(x => x.pagado)
                .ThenBy(x => x.fechaVencimiento.Year)
                .ThenBy(x => x.fechaVencimiento.Month)
                .ThenBy(x => x.fechaVencimiento.Year)
                .ThenBy(x => x.banco)
                .ThenBy(x => x.factoraje)
                .ThenBy(x => x.proveedor)
                .ThenBy(x => x.nombCC)
                .ToList();
                Session["lstResultado"] = lstResultado;
                var lstPrevio = lst.GroupBy(g => new { g.estatus, g.factoraje, g.banco, g.proveedor, g.fechaVencimiento, g.tipoMoneda }, (k, g) => new
                {
                    banco = k.banco,
                    factoraje = string.IsNullOrEmpty(k.factoraje) || k.factoraje.Equals("N") ? "Normal" : "Vencido",
                    fechaVencimientoS = k.fechaVencimiento.ToShortDateString(),
                    proveedor = k.proveedor,
                    saldoFactura = g.Sum(s => s.saldoFactura).ToString("C2"),
                    moneda = k.tipoMoneda == 1 ? "MX" : "DLL",
                    pagado = !k.estatus ? "Pagado" : "No pagado"
                }).ToList();
                result.Add("lstResultado", lstPrevio);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                Session["lstResultado"] = null;
                result.Add("lstResultado", new List<VencimientoDTO>());
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public FileResult getExcelResumenSemanal()
        {
            var excel = new ExcelUtilities();
            var Sheets = new List<excelSheetDTO>();
            #region Tab1
            var lst = (List<VencimientoDTO>)Session["lstResultado"];
            var fecha = lst.FirstOrDefault().fechaVencimiento;
            var lstIntereses = cadenaProductivaFS.getlstInteresesNafin(fecha);
            var lstOrdenada = lst.OrderByDescending(x => x.pagado).ToList();
            lstOrdenada.GroupBy(g => new { g.pagado, g.banco, g.factoraje, g.tipoMoneda }).ToList().ForEach(x =>
            {
                var diff = 0m;
                var interes = new tblC_InteresesNafin();
                var esVencido = x.Key.factoraje.Equals("V");
                var esDll = x.Key.tipoMoneda.Equals(2);
                var esIntereses = false;
                if (esVencido)
                {
                    interes = lstIntereses.FirstOrDefault(i => i.banco.Equals(x.Key.banco) && i.divisa.Equals(x.Key.tipoMoneda));
                    esIntereses = interes != null;
                    if (esIntereses)
                        diff = interes.totalBanco - interes.totalCadenas;
                }
                var moneda = esDll ? "DLL" : "MX";
                var pagado = x.Key.pagado ? "P" : "N";
                var sheetBanco = new excelSheetDTO() { name = string.Format("{0} {1} {2} {3}", x.Key.banco, x.Key.factoraje, moneda, pagado) };
                var rows = new List<excelRowDTO>()
                            {
                                new excelRowDTO() {
                                    cells = new List<excelCellDTO>(){
                                        new excelCellDTO{ text=string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                        new excelCellDTO{ text="GRUPO CONSTRUCCIONES PLANIFICADAS, SA DE CV", autoWidthFit=false, fill=true, border=false,colSpan=3,rowSpan=0 },
                                    }
                                    
                                },
                                new excelRowDTO() {
                                    cells = new List<excelCellDTO>(){
                                        new excelCellDTO{ text=string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                        new excelCellDTO{ text="RELACION DE FACTURAS OPERADAS EN CADENAS PRODUCTIVAS NAFIN", autoWidthFit=false, fill=true, border=false,colSpan=3,rowSpan=0},
                                    }
                                    
                                },
                                new excelRowDTO() {
                                    cells = new List<excelCellDTO>(){
                                        new excelCellDTO{ text=string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                        new excelCellDTO{ text=string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                        new excelCellDTO{ text=string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                        new excelCellDTO{ text=string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                        new excelCellDTO{ text=string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                        new excelCellDTO{ text=string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                        new excelCellDTO{ text=string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                        new excelCellDTO{ text=string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                    }
                                },
                                new excelRowDTO() {
                                    cells = new List<excelCellDTO>(){
                                        new excelCellDTO{ text="FACTURA", autoWidthFit=false, fill=true, border=true,colSpan=0,rowSpan=0, borderType = 0},
                                        new excelCellDTO{ text="PROVEEDOR", autoWidthFit=false, fill=true, border=true,colSpan=0,rowSpan=0, borderType = 0},
                                        new excelCellDTO{ text= string.Empty, autoWidthFit=false, fill=true, border=true,colSpan=0,rowSpan=0, borderType = 0},
                                        new excelCellDTO{ text="OBRA", autoWidthFit=false, fill=true, border=true,colSpan=0,rowSpan=0, borderType = 0},
                                        new excelCellDTO{ text="IMPORTE C/IVA", autoWidthFit=false, fill=true, border=true,colSpan=0,rowSpan=0, borderType = 0},
                                        new excelCellDTO{ text="IMPORTE C/IVA", autoWidthFit=false, fill=true, border=true,colSpan=0,rowSpan=0, borderType = 0},
                                        new excelCellDTO{ text="OPERADA", autoWidthFit=false, fill=true, border=true,colSpan=0,rowSpan=0, borderType = 0},
                                        new excelCellDTO{ text="VENCIMIENTO", autoWidthFit=false, fill=true, border=true,colSpan=0,rowSpan=0, borderType = 0},
                                    },
                                },
                                new excelRowDTO() {
                                    cells = new List<excelCellDTO>(){
                                        new excelCellDTO{ text=string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                        new excelCellDTO{ text=string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                        new excelCellDTO{ text=string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                        new excelCellDTO{ text=string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                        new excelCellDTO{ text=string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                        new excelCellDTO{ text=string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                        new excelCellDTO{ text=string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                        new excelCellDTO{ text=string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                    }
                                },
                                
                            };
                decimal total = 0;
                x.GroupBy(g => g.proveedor).ToList().ForEach(g =>
                {
                    g.ToList().ForEach(e =>
                    {
                        rows.Add(new excelRowDTO()
                        {
                            cells = new List<excelCellDTO>() {

                                new excelCellDTO{ text= e.factura.ToString(), autoWidthFit=false, fill=false, border=false,colSpan=0,rowSpan=0, 
                                    borderType = x.FirstOrDefault(y => y.proveedor == e.proveedor && y.total == e.total && y.centro_costos.Equals(e.centro_costos)).factura == e.factura ? 1 : 3},

                                new excelCellDTO{ text= e.proveedor, autoWidthFit=false, fill=false, border=false,colSpan=0,rowSpan=0, 
                                    borderType = x.FirstOrDefault(y => y.proveedor == e.proveedor  && y.total == e.total && y.centro_costos.Equals(e.centro_costos)).factura == e.factura ? 4 : 0},

                                new excelCellDTO{ text = tipoCC(e.centro_costos, e.nombCC).Equals(e.nombCC) ? string.Empty : e.nombCC, autoWidthFit=false, fill=false, border=false,colSpan=0,rowSpan=0,
                                    borderType = x.FirstOrDefault(y => y.proveedor == e.proveedor  && y.total == e.total && y.centro_costos.Equals(e.centro_costos)).factura == e.factura ? 1 : 3},

                                new excelCellDTO{ text=  tipoCC(e.centro_costos, e.nombCC).ToUpper(), autoWidthFit=false, fill=false, border=false,colSpan=0,rowSpan=0 , 
                                    borderType = x.FirstOrDefault(y => y.proveedor == e.proveedor  && y.total == e.total && y.centro_costos.Equals(e.centro_costos)).factura == e.factura ? 4 : 0},

                                new excelCellDTO{ text= e.saldoFactura, autoWidthFit=false, fill=false, border=false,colSpan=0,rowSpan=0, formatType=1 , 
                                    borderType = x.Where(y => y.proveedor == e.proveedor  && y.total == e.total && y.centro_costos.Equals(e.centro_costos)).Count() == 1 ? 14 :
                                    x.FirstOrDefault(y => y.proveedor == e.proveedor  && y.total == e.total && y.centro_costos.Equals(e.centro_costos)).factura == e.factura ? 6 : 8},

                                new excelCellDTO{ text= x.LastOrDefault(y => y.proveedor == e.proveedor && y.total == e.total && y.centro_costos.Equals(e.centro_costos)).factura == e.factura && 
                                    x.Where(y => y.proveedor == e.proveedor  && y.total == e.total && y.centro_costos.Equals(e.centro_costos)).Count() > 1 ? 
                                    x.Where(y => y.proveedor == e.proveedor && y.total == e.total && y.centro_costos.Equals(e.centro_costos)).Sum(s => changeFormat(s.saldoFactura)).ToString() : string.Empty, 
                                    autoWidthFit=false, fill=false, colSpan=0,rowSpan=0 ,  
                                    formatType = x.LastOrDefault(y => y.proveedor == e.proveedor && y.total == e.total && y.centro_costos.Equals(e.centro_costos)).factura == e.factura && 
                                    x.Where(y => y.proveedor == e.proveedor  && y.total == e.total && y.centro_costos.Equals(e.centro_costos)).Count() > 1 ? 1 : 0, 
                                    border = (x.LastOrDefault(y => y.proveedor == e.proveedor && y.total == e.total).factura == e.factura && 
                                    x.Where(y => y.proveedor == e.proveedor  && y.total == e.total && y.centro_costos.Equals(e.centro_costos)).Count() > 1 ),
                                    borderType = (x.FirstOrDefault(y => y.proveedor == e.proveedor  && y.total == e.total && y.centro_costos.Equals(e.centro_costos)).factura == e.factura) && x.Count() > 1 ? 4 :
                                    (x.LastOrDefault(y => y.proveedor == e.proveedor && y.total == e.total).factura == x.FirstOrDefault(y => y.proveedor == e.proveedor && y.total == e.total).factura) ? 4 :
                                    x.LastOrDefault(y => y.proveedor == e.proveedor  && y.total == e.total && y.centro_costos.Equals(e.centro_costos)).factura == e.factura
                                    && x.Where(y => y.proveedor == e.proveedor && y.total == e.total && y.centro_costos.Equals(e.centro_costos)).Count() > 1 ? 11 : 0 },

                                new excelCellDTO{ text= e.fecha.ToLongDateString(), autoWidthFit=false, fill=false, border= false,colSpan=0,rowSpan=0 , 
                                    borderType = x.FirstOrDefault(y => y.proveedor == e.proveedor  && y.total == e.total).factura == e.factura ? 4 : 0},

                                new excelCellDTO{ text= e.fechaVencimiento.ToLongDateString(), autoWidthFit=false, fill=false, border=false,colSpan=0,rowSpan=0 , 
                                    borderType = x.FirstOrDefault(y => y.proveedor == e.proveedor  && y.total == e.total).factura == e.factura ? 6 : 8},

                                new excelCellDTO{ text= e.factoraje.First().ToString(), autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType =  0},
                                new excelCellDTO{ text= e.banco, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType =  0},
                                new excelCellDTO{ text= e.tipoMoneda == 1 ? "MX" : "DLL".ToString(), autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType =  0},
                                new excelCellDTO{ text= e.pagado ? "P" : "N", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType =  0}
                            }
                        });
                    });
                    total += g.Sum(s => changeFormat(s.saldoFactura));
                    rows.AddRange(new List<excelRowDTO>() {
                        new excelRowDTO() {
                                cells = new List<excelCellDTO>() {
                                    new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 2},
                                    new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 5},
                                    new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 10},
                                    new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 10},
                                    new excelCellDTO{ text = g.Sum(s => changeFormat(s.saldoFactura)).ToString(), autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 13, formatType = 1},
                                    new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 5},
                                    new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 5},
                                    new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 7}
                                }
                        },
                    new excelRowDTO() {
                        cells = new List<excelCellDTO>() {
                                    new excelCellDTO{ text=string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                    new excelCellDTO{ text=string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                                }
                    }

                    });
                });
                rows.AddRange(new List<excelRowDTO>(){
                        new excelRowDTO() {
                                cells = new List<excelCellDTO>() {
                                    new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0},
                                    new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0},
                                    new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0},
                                    new excelCellDTO{ text = "TOTAL " + x.Key.factoraje.ToUpper(), autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0},
                                    new excelCellDTO{ text = total.ToString(), autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 12, formatType = 1},
                                    new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0},
                                    new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0},
                                    new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0},
                                    new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0},
                                    new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0},
                                    new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0},
                                    new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0}
                                }
                            }
                    });
                if (esVencido && esIntereses)
                {
                    rows.AddRange(new List<excelRowDTO>(){
                        new excelRowDTO() {
                            cells = new List<excelCellDTO>(){
                                new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0},
                                    new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0},
                                    new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0},
                                    new excelCellDTO{ text = string.Format("{0} {1}", x.Key.banco, moneda), autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0},
                                    new excelCellDTO{ text = interes.totalBanco.ToString("0.####"), autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0, formatType = 1},
                                    new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0},
                                    new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0},
                                    new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0},
                                    new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0},
                                    new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0},
                                    new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0},
                                    new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0}
                            }
                        },
                        new excelRowDTO() {
                            cells = new List<excelCellDTO>(){
                                new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0},
                                    new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0},
                                    new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0},
                                    new excelCellDTO{ text = "INTERESES", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0},
                                    new excelCellDTO{ text = diff.ToString("0.##"), autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 12, formatType = 1},
                                    new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0},
                                    new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0},
                                    new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0},
                                    new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0},
                                    new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0},
                                    new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0},
                                    new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0}
                            }
                        },
                    });
                }
                var ccHeaderInferior = new List<excelRowDTO>()
                {
                    new excelRowDTO() {
                            cells = new List<excelCellDTO>(){
                                new excelCellDTO{ text = esVencido ? "INTERESES X FACTORAJE VENCIDO" : string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0},
                            }
                        },
                        new excelRowDTO() {
                            cells = new List<excelCellDTO>(){
                                new excelCellDTO{ text = "CENTRO COSTOS", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0},
                                new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0},
                                new excelCellDTO{ text = "IMPORTE C/IVA", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0, borderType = 0}
                            }
                        }
                };
                if (esVencido && esIntereses)
                {
                    ccHeaderInferior[1].cells.AddRange(new List<excelCellDTO>()
                        {
                            new excelCellDTO{ text= "INTERES", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0 , borderType = 0 , formatType = 0},
                            new excelCellDTO{ text= "SALDO", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0 , borderType = 0 , formatType = 0}
                        });
                }
                if (esDll)
                {
                    ccHeaderInferior[1].cells.AddRange(new List<excelCellDTO>()
                        {
                            new excelCellDTO{ text= "PROLATEO", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0 , borderType = 0 , formatType = 0},
                            new excelCellDTO{ text= "TC", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0 , borderType = 0 , formatType = 0},
                            new excelCellDTO{ text= "IMPORTE", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0 , borderType = 0 , formatType = 0},
                            new excelCellDTO{ text= "DIFERENCIA", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0 , borderType = 0 , formatType = 0}
                        });
                }
                rows.AddRange(ccHeaderInferior);
                var gCC = x.GroupBy(g => g.centro_costos).ToList();
                var lCC = gCC.Count;
                var tCC = gCC.Sum(s => s.Sum(ss => changeFormat(ss.saldoFactura)));
                var diffCC = decimal.Round(diff - (tCC * (esIntereses ? interes.interes : 1)), 4);
                var diffProlateo = decimal.Round(diffCC / lCC, 4);
                gCC.ForEach(g =>
                {
                    var sumCC = g.Sum(s => changeFormat(s.saldoFactura));
                    var intCC = sumCC * (esIntereses ? interes.interes : 1);
                    var intCCProl = intCC + diffProlateo;
                    var ccTotal = new List<excelCellDTO>() {
                            new excelCellDTO{ text = tipoCC(g.FirstOrDefault().centro_costos, g.FirstOrDefault().nombCC).Equals(g.FirstOrDefault().nombCC) ? string.Empty : g.FirstOrDefault().nombCC, autoWidthFit=false, fill=false, border=true,colSpan=0, rowSpan=0, borderType = 0},
                            new excelCellDTO{ text=  tipoCC(g.FirstOrDefault().centro_costos, g.FirstOrDefault().nombCC).ToUpper(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0 , borderType = 0},
                            new excelCellDTO{ text= sumCC.ToString(), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0 , borderType = 0 , formatType = 1},
                        };
                    if (esVencido && esIntereses)
                    {
                        ccTotal.AddRange(new List<excelCellDTO>()
                        {
                            new excelCellDTO{ text= interes.interes.ToString("0.####"), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0 , borderType = 0 , formatType = 1},
                            new excelCellDTO{ text= intCC.ToString("0.##"), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0 , borderType = 0 , formatType = 1}
                        });
                    }
                    if (esDll)
                    {
                        var tc = esVencido && esIntereses ? interes.tipoCambio : cadenaProductivaFS.getDolarDelDia(g.FirstOrDefault().fechaVencimiento); ;
                        var importe = (intCCProl == 0 ? sumCC : intCCProl) * tc;
                        var diferencia = importe - intCCProl;
                        ccTotal.AddRange(new List<excelCellDTO>()
                        {
                            new excelCellDTO{ text= diffProlateo.ToString("0.##"), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0 , borderType = 0 , formatType = 1},
                            new excelCellDTO{ text= tc.ToString("0.####"), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0 , borderType = 0 , formatType = 1},
                            new excelCellDTO{ text= importe.ToString("0.##"), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0 , borderType = 0 , formatType = 1},
                            new excelCellDTO{ text= diferencia.ToString("0.##"), autoWidthFit=false, fill=false, border=true,colSpan=0,rowSpan=0 , borderType = 0 , formatType = 1},
                        });
                    }
                    rows.Add(new excelRowDTO() { cells = ccTotal });
                });
                rows.Add(new excelRowDTO()
                {
                    cells = new List<excelCellDTO>() {
                        new excelCellDTO{ text=string.Empty, autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
                        new excelCellDTO{ text=string.Empty, autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
                        new excelCellDTO{ text=string.Empty, autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
                        new excelCellDTO{ text=string.Empty, autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
                        new excelCellDTO{ text=string.Empty, autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
                        new excelCellDTO{ text=string.Empty, autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
                        new excelCellDTO{ text=string.Empty, autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
                        new excelCellDTO{ text=string.Empty, autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
                        new excelCellDTO{ text=string.Empty, autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
                        new excelCellDTO{ text=string.Empty, autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
                        new excelCellDTO{ text=string.Empty, autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
                        new excelCellDTO{ text=string.Empty, autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
                        new excelCellDTO{ text=string.Empty, autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0},
                        new excelCellDTO{ text=string.Empty, autoWidthFit=true, fill=false, border=false,colSpan=0,rowSpan=0}
                    }
                });
                sheetBanco.Sheet = rows;
                Sheets.Add(sheetBanco);
            });

            #endregion
            excel.CreateExcelFileCadenaProductiva(this, Sheets, string.Format("Cadena productiva al {0}", lst.FirstOrDefault().fechaVencimiento.ToShortDateString()));
            return null;
        }
        #endregion
        #region Acomulado
        public ActionResult SetAcomuladoTable(int tipo, int anio)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstSemanas = setlstSemana();
                var lstRenglon = getLstVencimiento()
                    .Where(x => x.fechaVencimiento.Year == anio
                        && !tipoCC(x.centro_costos, x.nombCC).Equals(tipo == 2 ? "Civil" : "Industrial")
                        )
                    .Select(x => new
                    {
                        banco = x.banco,
                        factoraje = x.factoraje,
                        tipoMoneda = x.tipoMoneda,
                        saldoFactura = x.saldoFactura,
                        semana = setRangoSemana(x.fechaVencimiento),
                        fecha = x.fechaVencimiento
                    })
                    .GroupBy(x => new { x.banco, x.factoraje, x.tipoMoneda, x.semana }, (key, group) => new
                    {
                        banco = nombreBancoAcomulado(key.banco, key.factoraje),
                        moneda = key.tipoMoneda,
                        columna = numeroSemana(group.FirstOrDefault().fecha),
                        semana = key.semana,
                        monto = group.Sum(s => Convert.ToDecimal(s.saldoFactura))
                    })
                        .OrderByDescending(x => x.banco)
                        .ThenBy(x => x.moneda)
                        .ThenBy(x => x.columna)
                        .ToList();
                result.Add("lstSemanas", lstSemanas);
                result.Add("lstRenglon", lstRenglon);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getLstFacturaSemana(string nombre, int semana, int tipo, int anio, int moneda)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstNombre = nombre.Split();
                var banco = lstNombre[0];
                var factoraje = banco.Equals("OVERHAUL") || banco.Equals("TOTAL") || banco.Equals("CONVERSIÓN") ? string.Empty : nombre.Split()[1];
                var f = string.Empty;
                if (!string.IsNullOrEmpty(factoraje)) f = factoraje[0].ToString();
                if (banco.Equals("OVERHAUL") || (lstNombre.Length > 1 && lstNombre[1].Equals("CONSTRUPLAN")))
                {
                    moneda = 0;
                    factoraje = string.Empty;
                }
                var lstCCDivision = cadenaProductivaFS.lstObra(tipo);
                var lstFiltro = cadenaProductivaFS.GetAllDocumentos()
                    .Where(w => !w.estatus)
                    .Where(w => w.fechaVencimiento.Year == anio)
                    .Where(w => banco.Equals("TOTAL") || banco.Equals("CONVERSIÓN") || banco.Equals("OVERHAUL") ? true : w.banco.Equals(banco))
                    .Where(w => numeroSemana(w.fechaVencimiento) == semana)
                    .Where(w => lstCCDivision.Exists(e => e.cc == w.centro_costos))
                    .Where(w => banco.Equals("OVERHAUL") ? isOverhaul(w.centro_costos, w.numProveedor.ToString()) : true)
                    .Where(x => string.IsNullOrEmpty(factoraje) ? true : x.factoraje.Equals(f))
                    .Where(w => moneda == 0 ? true : moneda == w.tipoMoneda)
                    .OrderBy(o => o.centro_costos)
                    .ThenBy(o => o.proveedor)
                    .ToList();
                lstFiltro.Where(w => w.tipoMoneda == 2).ToList().ForEach(x =>
                {
                    x.tipoCambio = x.tipoCambio > 5 ? x.tipoCambio : cadenaProductivaFS.getDolarDelDia(x.fecha);
                });
                result.Add("semana", "Semana " + semana);
                result.Add("banco", banco);
                result.Add("factoraje", banco.Equals("TOTAL") || banco.Equals("CONVERSIÓN") || banco.Equals("OVERHAUL") ? string.Empty : "Factoraje " + factoraje);
                result.Add("moneda", banco.Equals("OVERHAUL") || banco.Equals("CONVERSIÓN") || (lstNombre.Length > 1 && lstNombre[1].Equals("CONSTRUPLAN")) ? "MX" : moneda == 1 ? "MX" : "DLL");
                result.Add("total", lstFiltro.Sum(s => s.saldoFactura * (banco.Equals("CONVERSIÓN") || banco.Equals("OVERHAUL") ? s.tipoMoneda == 1 ? 1 : s.tipoCambio : 1)).ToString("C2"));
                result.Add("data", lstFiltro.Select(x => new
                {
                    factura = x.factura,
                    proveedor = x.proveedor,
                    tipoCC = lstCCDivision.Exists(w => w.cc.Equals(x.centro_costos)) ? x.nombCC : string.Empty,
                    cc = lstCCDivision.Exists(w => w.cc.Equals(x.centro_costos)) ? lstCCDivision.FirstOrDefault(w => w.cc.Equals(x.centro_costos)).bit_area.Equals("1") ? "CIVIL" : lstCCDivision.FirstOrDefault(w => w.cc.Equals(x.centro_costos)).bit_area.Equals("2") ? "INDUSTRIAL" : string.Empty : x.nombCC,
                    saldo = x.saldoFactura.ToString("C2"),
                    emision = x.fecha.ToShortDateString(),
                    vencimiento = x.fechaVencimiento.ToShortDateString(),
                    banco = x.banco,
                    factoraje = x.factoraje.Equals("N") ? "Normal" : "Vencido",
                    moneda = x.tipoMoneda == 1 ? "MX" : "DLL",
                    tc = x.tipoCambio.ToString("C2")
                }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SetAcomuladoTotal(int tipo, int anio)
        {
            var result = new Dictionary<string, object>();
            //try
            //{
                var lstSemana = setlstSemana();
                if (anio == 2017)
                    lstSemana.RemoveAt(1);
                else
                    lstSemana.RemoveAt(lstSemana.Count - 1);
                Session["lstSemana"] = lstSemana;
                result.Add("lstSemanas", lstSemana);
                var lstDivIndus = new List<string>() 
                {
                    ((int)TipoCCEnum.Industrial).ToString(),
                    ((int)TipoCCEnum.AlimentosYBebidas).ToString(),
                    ((int)TipoCCEnum.Automotriz).ToString(),
                    ((int)TipoCCEnum.Energía).ToString(),
                };
                var lstCCDivision = cadenaProductivaFS.lstObra()
                    .Where(w => tipo == 0 ? true :  tipo == 2 ? lstDivIndus.Contains(w.bit_area) : !lstDivIndus.Contains(w.bit_area)).ToList();
                var lstFiltro = cadenaProductivaFS.GetAllDocumentos()
                    .Where(w => !w.estatus)
                    .Where(w => w.fechaVencimiento.Year == anio)
                    .Where(w => lstCCDivision.Exists(e => e.cc == w.centro_costos)).ToList();
                var lstInividual = lstFiltro.Select(k => new VencimientoDTO
                    {
                        banco = k.banco,
                        factoraje = k.factoraje,
                        factura = k.factura.ParseInt(),
                        tipoMoneda = k.tipoMoneda,
                        saldoFactura = k.saldoFactura.ToString(),
                        tipoCambio = k.numProveedor.ParseInt() < 9000 ? 1 : k.tipoCambio,
                        fecha = k.fechaVencimiento,
                        centro_costos = k.centro_costos,
                        proveedor = k.numProveedor,
                        fechaVencimientoS = setRangoSemana(k.fechaVencimiento)
                    }).ToList();
                Session["lstInividual"] = lstInividual;
                var lstRes2 = lstInividual.GroupBy(x => new { x.banco, x.factoraje, x.tipoMoneda, x.proveedor, x.fechaVencimientoS }, (key, group) => new
                {
                    banco = key.banco,
                    factoraje = key.factoraje,
                    moneda = key.tipoMoneda,
                    fecha = group.FirstOrDefault().fecha,
                    semana = key.fechaVencimientoS,
                    saldoFactura = group.Select(s => new { monto = s.saldoFactura, cambio = s.tipoCambio }).ToList()
                }).Distinct().ToList();
                var lstRes = lstRes2.GroupBy(x => new { x.banco, x.factoraje, x.moneda, x.semana }, (key, group) => new
                    {
                        banco = key.banco + " " + (key.factoraje.Equals("V") ? "Vencido" : "Normal"),
                        factoraje = key.factoraje,
                        moneda = key.moneda,
                        columna = numeroSemana(group.FirstOrDefault().fecha),
                        semana = key.semana,
                        monto = group.Sum(s => s.saldoFactura.Sum(ss => decimal.Parse(ss.monto)))
                    })
                    .Distinct()
                    .OrderBy(x => x.banco)
                    .ThenBy(x => x.moneda)
                    .ThenBy(x => x.columna)
                    .ToList();
                var lstConvercion = lstRes2.Where(w => w.moneda == 2).GroupBy(x => new { x.moneda, x.banco, x.factoraje, x.semana }, (key, group) => new
                {
                    banco = key.banco,
                    factoraje = key.factoraje,
                    moneda = key.moneda,
                    fecha = group.FirstOrDefault().fecha,
                    semana = key.semana,
                    saldoFactura = group.Sum(s => s.saldoFactura.Sum(ss => Convert.ToDecimal(ss.monto) * ss.cambio))
                })
                    .Distinct()
                    .GroupBy(x => new { x.banco, x.factoraje, x.moneda, x.semana }, (key, group) => new
                    {
                        banco = nombreBancoAcomulado(key.banco, key.factoraje),
                        factoraje = key.factoraje,
                        moneda = key.moneda,
                        columna = numeroSemana(group.FirstOrDefault().fecha),
                        semana = key.semana,
                        monto = group.Sum(s => Convert.ToDecimal(s.saldoFactura))
                    })
                    .Distinct()
                    .OrderBy(x => x.banco)
                    .ThenBy(x => x.moneda)
                    .ThenBy(x => x.columna)
                    .ToList();
                var lstConverSem = lstConvercion.GroupBy(x => new { x.moneda }, (key, group) => new AcomuladoAnualDTO
                {
                    banco = string.Empty,
                    factoraje = string.Empty,
                    moneda = key.moneda,
                    semana1 = group.Where(w => w.columna == 1).Sum(s => s.monto),
                    semana2 = group.Where(w => w.columna == 2).Sum(s => s.monto),
                    semana3 = group.Where(w => w.columna == 3).Sum(s => s.monto),
                    semana4 = group.Where(w => w.columna == 4).Sum(s => s.monto),
                    semana5 = group.Where(w => w.columna == 5).Sum(s => s.monto),
                    semana6 = group.Where(w => w.columna == 6).Sum(s => s.monto),
                    semana7 = group.Where(w => w.columna == 7).Sum(s => s.monto),
                    semana8 = group.Where(w => w.columna == 8).Sum(s => s.monto),
                    semana9 = group.Where(w => w.columna == 9).Sum(s => s.monto),
                    semana10 = group.Where(w => w.columna == 10).Sum(s => s.monto),
                    semana11 = group.Where(w => w.columna == 11).Sum(s => s.monto),
                    semana12 = group.Where(w => w.columna == 12).Sum(s => s.monto),
                    semana13 = group.Where(w => w.columna == 13).Sum(s => s.monto),
                    semana14 = group.Where(w => w.columna == 14).Sum(s => s.monto),
                    semana15 = group.Where(w => w.columna == 15).Sum(s => s.monto),
                    semana16 = group.Where(w => w.columna == 16).Sum(s => s.monto),
                    semana17 = group.Where(w => w.columna == 17).Sum(s => s.monto),
                    semana18 = group.Where(w => w.columna == 18).Sum(s => s.monto),
                    semana19 = group.Where(w => w.columna == 19).Sum(s => s.monto),
                    semana20 = group.Where(w => w.columna == 20).Sum(s => s.monto),
                    semana21 = group.Where(w => w.columna == 21).Sum(s => s.monto),
                    semana22 = group.Where(w => w.columna == 22).Sum(s => s.monto),
                    semana23 = group.Where(w => w.columna == 23).Sum(s => s.monto),
                    semana24 = group.Where(w => w.columna == 24).Sum(s => s.monto),
                    semana25 = group.Where(w => w.columna == 25).Sum(s => s.monto),
                    semana26 = group.Where(w => w.columna == 26).Sum(s => s.monto),
                    semana27 = group.Where(w => w.columna == 27).Sum(s => s.monto),
                    semana28 = group.Where(w => w.columna == 28).Sum(s => s.monto),
                    semana29 = group.Where(w => w.columna == 29).Sum(s => s.monto),
                    semana30 = group.Where(w => w.columna == 30).Sum(s => s.monto),
                    semana31 = group.Where(w => w.columna == 31).Sum(s => s.monto),
                    semana32 = group.Where(w => w.columna == 32).Sum(s => s.monto),
                    semana33 = group.Where(w => w.columna == 33).Sum(s => s.monto),
                    semana34 = group.Where(w => w.columna == 34).Sum(s => s.monto),
                    semana35 = group.Where(w => w.columna == 35).Sum(s => s.monto),
                    semana36 = group.Where(w => w.columna == 36).Sum(s => s.monto),
                    semana37 = group.Where(w => w.columna == 37).Sum(s => s.monto),
                    semana38 = group.Where(w => w.columna == 38).Sum(s => s.monto),
                    semana39 = group.Where(w => w.columna == 39).Sum(s => s.monto),
                    semana40 = group.Where(w => w.columna == 40).Sum(s => s.monto),
                    semana41 = group.Where(w => w.columna == 41).Sum(s => s.monto),
                    semana42 = group.Where(w => w.columna == 42).Sum(s => s.monto),
                    semana43 = group.Where(w => w.columna == 43).Sum(s => s.monto),
                    semana44 = group.Where(w => w.columna == 44).Sum(s => s.monto),
                    semana45 = group.Where(w => w.columna == 45).Sum(s => s.monto),
                    semana46 = group.Where(w => w.columna == 46).Sum(s => s.monto),
                    semana47 = group.Where(w => w.columna == 47).Sum(s => s.monto),
                    semana48 = group.Where(w => w.columna == 48).Sum(s => s.monto),
                    semana49 = group.Where(w => w.columna == 49).Sum(s => s.monto),
                    semana50 = group.Where(w => w.columna == 50).Sum(s => s.monto),
                    semana51 = group.Where(w => w.columna == 51).Sum(s => s.monto),
                    semana52 = group.Where(w => w.columna == 52).Sum(s => s.monto),
                }).FirstOrDefault();
                var lstRenglon = lstRes.GroupBy(x => new { x.banco, x.factoraje, x.moneda }, (key, group) => new AcomuladoAnualDTO
                {
                    banco = key.banco,
                    factoraje = key.factoraje,
                    moneda = key.moneda,
                    semana1 = group.Where(w => w.columna == 1).Sum(s => s.monto),
                    semana2 = group.Where(w => w.columna == 2).Sum(s => s.monto),
                    semana3 = group.Where(w => w.columna == 3).Sum(s => s.monto),
                    semana4 = group.Where(w => w.columna == 4).Sum(s => s.monto),
                    semana5 = group.Where(w => w.columna == 5).Sum(s => s.monto),
                    semana6 = group.Where(w => w.columna == 6).Sum(s => s.monto),
                    semana7 = group.Where(w => w.columna == 7).Sum(s => s.monto),
                    semana8 = group.Where(w => w.columna == 8).Sum(s => s.monto),
                    semana9 = group.Where(w => w.columna == 9).Sum(s => s.monto),
                    semana10 = group.Where(w => w.columna == 10).Sum(s => s.monto),
                    semana11 = group.Where(w => w.columna == 11).Sum(s => s.monto),
                    semana12 = group.Where(w => w.columna == 12).Sum(s => s.monto),
                    semana13 = group.Where(w => w.columna == 13).Sum(s => s.monto),
                    semana14 = group.Where(w => w.columna == 14).Sum(s => s.monto),
                    semana15 = group.Where(w => w.columna == 15).Sum(s => s.monto),
                    semana16 = group.Where(w => w.columna == 16).Sum(s => s.monto),
                    semana17 = group.Where(w => w.columna == 17).Sum(s => s.monto),
                    semana18 = group.Where(w => w.columna == 18).Sum(s => s.monto),
                    semana19 = group.Where(w => w.columna == 19).Sum(s => s.monto),
                    semana20 = group.Where(w => w.columna == 20).Sum(s => s.monto),
                    semana21 = group.Where(w => w.columna == 21).Sum(s => s.monto),
                    semana22 = group.Where(w => w.columna == 22).Sum(s => s.monto),
                    semana23 = group.Where(w => w.columna == 23).Sum(s => s.monto),
                    semana24 = group.Where(w => w.columna == 24).Sum(s => s.monto),
                    semana25 = group.Where(w => w.columna == 25).Sum(s => s.monto),
                    semana26 = group.Where(w => w.columna == 26).Sum(s => s.monto),
                    semana27 = group.Where(w => w.columna == 27).Sum(s => s.monto),
                    semana28 = group.Where(w => w.columna == 28).Sum(s => s.monto),
                    semana29 = group.Where(w => w.columna == 29).Sum(s => s.monto),
                    semana30 = group.Where(w => w.columna == 30).Sum(s => s.monto),
                    semana31 = group.Where(w => w.columna == 31).Sum(s => s.monto),
                    semana32 = group.Where(w => w.columna == 32).Sum(s => s.monto),
                    semana33 = group.Where(w => w.columna == 33).Sum(s => s.monto),
                    semana34 = group.Where(w => w.columna == 34).Sum(s => s.monto),
                    semana35 = group.Where(w => w.columna == 35).Sum(s => s.monto),
                    semana36 = group.Where(w => w.columna == 36).Sum(s => s.monto),
                    semana37 = group.Where(w => w.columna == 37).Sum(s => s.monto),
                    semana38 = group.Where(w => w.columna == 38).Sum(s => s.monto),
                    semana39 = group.Where(w => w.columna == 39).Sum(s => s.monto),
                    semana40 = group.Where(w => w.columna == 40).Sum(s => s.monto),
                    semana41 = group.Where(w => w.columna == 41).Sum(s => s.monto),
                    semana42 = group.Where(w => w.columna == 42).Sum(s => s.monto),
                    semana43 = group.Where(w => w.columna == 43).Sum(s => s.monto),
                    semana44 = group.Where(w => w.columna == 44).Sum(s => s.monto),
                    semana45 = group.Where(w => w.columna == 45).Sum(s => s.monto),
                    semana46 = group.Where(w => w.columna == 46).Sum(s => s.monto),
                    semana47 = group.Where(w => w.columna == 47).Sum(s => s.monto),
                    semana48 = group.Where(w => w.columna == 48).Sum(s => s.monto),
                    semana49 = group.Where(w => w.columna == 49).Sum(s => s.monto),
                    semana50 = group.Where(w => w.columna == 50).Sum(s => s.monto),
                    semana51 = group.Where(w => w.columna == 51).Sum(s => s.monto),
                    semana52 = group.Where(w => w.columna == 52).Sum(s => s.monto),
                }).ToList();
                var lstAcomulado = lstRenglon.Where(x => x.moneda == 1).ToList();
                var lstMoneda = lstRenglon.GroupBy(x => new { x.moneda }, (key, group) => new AcomuladoAnualDTO
                {
                    banco = "TOTAL " + (key.moneda == 1 ? "MX" : "DLL"),
                    moneda = key.moneda,
                    semana1 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana1),
                    semana2 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana2),
                    semana3 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana3),
                    semana4 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana4),
                    semana5 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana5),
                    semana6 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana6),
                    semana7 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana7),
                    semana8 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana8),
                    semana9 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana9),
                    semana10 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana10),
                    semana11 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana11),
                    semana12 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana12),
                    semana13 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana13),
                    semana14 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana14),
                    semana15 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana15),
                    semana16 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana16),
                    semana17 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana17),
                    semana18 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana18),
                    semana19 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana19),
                    semana20 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana20),
                    semana21 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana21),
                    semana22 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana22),
                    semana23 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana23),
                    semana24 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana24),
                    semana25 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana25),
                    semana26 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana26),
                    semana27 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana27),
                    semana28 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana28),
                    semana29 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana29),
                    semana30 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana30),
                    semana31 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana31),
                    semana32 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana32),
                    semana33 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana33),
                    semana34 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana34),
                    semana35 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana35),
                    semana36 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana36),
                    semana37 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana37),
                    semana38 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana38),
                    semana39 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana39),
                    semana40 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana40),
                    semana41 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana41),
                    semana42 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana42),
                    semana43 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana43),
                    semana44 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana44),
                    semana45 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana45),
                    semana46 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana46),
                    semana47 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana47),
                    semana48 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana48),
                    semana49 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana49),
                    semana50 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana50),
                    semana51 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana51),
                    semana52 = group.Where(w => w.moneda == key.moneda).Sum(s => s.semana52)
                }).ToList();
                lstAcomulado.Add(lstMoneda.FirstOrDefault(w => w.moneda == 1));
                lstAcomulado.AddRange(lstRenglon.Where(x => x.moneda == 2));
                if (lstMoneda.Any(w => w.moneda == 2))
                {
                    lstAcomulado.Add(lstMoneda.FirstOrDefault(w => w.moneda == 2));   
                }
                else
                {
                    lstAcomulado.Add(new AcomuladoAnualDTO()
                    {
                        banco = "TOTAL DLL",
                        moneda = 2,
                    });
                }
                var esConverSem =  lstConverSem == null;
                lstAcomulado.Add(new AcomuladoAnualDTO()
                {
                    banco = "CONVERSIÓN",
                    factoraje = string.Empty,
                    semana1 = esConverSem ? 0 : lstConverSem.semana1,
                    semana2 = esConverSem ? 0 : lstConverSem.semana2,
                    semana3 = esConverSem ? 0 : lstConverSem.semana3,
                    semana4 = esConverSem ? 0 : lstConverSem.semana4,
                    semana5 = esConverSem ? 0 : lstConverSem.semana5,
                    semana6 = esConverSem ? 0 : lstConverSem.semana6,
                    semana7 = esConverSem ? 0 : lstConverSem.semana7,
                    semana8 = esConverSem ? 0 : lstConverSem.semana8,
                    semana9 = esConverSem ? 0 : lstConverSem.semana9,
                    semana10 = esConverSem ? 0 : lstConverSem.semana10,
                    semana11 = esConverSem ? 0 : lstConverSem.semana11,
                    semana12 = esConverSem ? 0 : lstConverSem.semana12,
                    semana13 = esConverSem ? 0 : lstConverSem.semana13,
                    semana14 = esConverSem ? 0 : lstConverSem.semana14,
                    semana15 = esConverSem ? 0 : lstConverSem.semana15,
                    semana16 = esConverSem ? 0 : lstConverSem.semana16,
                    semana17 = esConverSem ? 0 : lstConverSem.semana17,
                    semana18 = esConverSem ? 0 : lstConverSem.semana18,
                    semana19 = esConverSem ? 0 : lstConverSem.semana19,
                    semana20 = esConverSem ? 0 : lstConverSem.semana20,
                    semana21 = esConverSem ? 0 : lstConverSem.semana21,
                    semana22 = esConverSem ? 0 : lstConverSem.semana22,
                    semana23 = esConverSem ? 0 : lstConverSem.semana23,
                    semana24 = esConverSem ? 0 : lstConverSem.semana24,
                    semana25 = esConverSem ? 0 : lstConverSem.semana25,
                    semana26 = esConverSem ? 0 : lstConverSem.semana26,
                    semana27 = esConverSem ? 0 : lstConverSem.semana27,
                    semana28 = esConverSem ? 0 : lstConverSem.semana28,
                    semana29 = esConverSem ? 0 : lstConverSem.semana29,
                    semana30 = esConverSem ? 0 : lstConverSem.semana30,
                    semana31 = esConverSem ? 0 : lstConverSem.semana31,
                    semana32 = esConverSem ? 0 : lstConverSem.semana32,
                    semana33 = esConverSem ? 0 : lstConverSem.semana33,
                    semana34 = esConverSem ? 0 : lstConverSem.semana34,
                    semana35 = esConverSem ? 0 : lstConverSem.semana35,
                    semana36 = esConverSem ? 0 : lstConverSem.semana36,
                    semana37 = esConverSem ? 0 : lstConverSem.semana37,
                    semana38 = esConverSem ? 0 : lstConverSem.semana38,
                    semana39 = esConverSem ? 0 : lstConverSem.semana39,
                    semana40 = esConverSem ? 0 : lstConverSem.semana40,
                    semana41 = esConverSem ? 0 : lstConverSem.semana41,
                    semana42 = esConverSem ? 0 : lstConverSem.semana42,
                    semana43 = esConverSem ? 0 : lstConverSem.semana43,
                    semana44 = esConverSem ? 0 : lstConverSem.semana44,
                    semana45 = esConverSem ? 0 : lstConverSem.semana45,
                    semana46 = esConverSem ? 0 : lstConverSem.semana46,
                    semana47 = esConverSem ? 0 : lstConverSem.semana47,
                    semana48 = esConverSem ? 0 : lstConverSem.semana48,
                    semana49 = esConverSem ? 0 : lstConverSem.semana49,
                    semana50 = esConverSem ? 0 : lstConverSem.semana50,
                    semana51 = esConverSem ? 0 : lstConverSem.semana51,
                    semana52 = esConverSem ? 0 : lstConverSem.semana52
                });
                var lstConstruplan = new AcomuladoAnualDTO()
                {
                    banco = "TOTAL CONSTRUPLAN",
                    factoraje = string.Empty,
                    semana1 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana1 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana1,
                    semana2 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana2 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana2,
                    semana3 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana3 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana3,
                    semana4 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana4 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana4,
                    semana5 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana5 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana5,
                    semana6 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana6 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana6,
                    semana7 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana7 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana7,
                    semana8 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana8 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana8,
                    semana9 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana9 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana9,
                    semana10 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana10 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana10,
                    semana11 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana11 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana11,
                    semana12 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana12 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana12,
                    semana13 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana13 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana13,
                    semana14 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana14 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana14,
                    semana15 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana15 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana15,
                    semana16 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana16 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana16,
                    semana17 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana17 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana17,
                    semana18 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana18 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana18,
                    semana19 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana19 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana19,
                    semana20 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana20 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana20,
                    semana21 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana21 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana21,
                    semana22 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana22 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana22,
                    semana23 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana23 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana23,
                    semana24 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana24 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana24,
                    semana25 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana25 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana25,
                    semana26 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana26 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana26,
                    semana27 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana27 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana27,
                    semana28 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana28 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana28,
                    semana29 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana29 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana29,
                    semana30 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana30 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana30,
                    semana31 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana31 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana31,
                    semana32 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana32 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana32,
                    semana33 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana33 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana33,
                    semana34 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana34 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana34,
                    semana35 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana35 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana35,
                    semana36 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana36 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana36,
                    semana37 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana37 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana37,
                    semana38 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana38 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana38,
                    semana39 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana39 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana39,
                    semana40 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana40 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana40,
                    semana41 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana41 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana41,
                    semana42 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana42 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana42,
                    semana43 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana43 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana43,
                    semana44 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana44 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana44,
                    semana45 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana45 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana45,
                    semana46 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana46 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana46,
                    semana47 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana47 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana47,
                    semana48 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana48 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana48,
                    semana49 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana49 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana49,
                    semana50 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana50 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana50,
                    semana51 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana51 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana51,
                    semana52 = lstAcomulado.FirstOrDefault(x => x.banco.Equals("TOTAL MX")).semana52 + lstAcomulado.FirstOrDefault(x => x.banco.Contains("CONVERSIÓN")).semana52
                };
                lstAcomulado.AddRange(new List<AcomuladoAnualDTO>(){
                    new AcomuladoAnualDTO(),
                     new AcomuladoAnualDTO()
                    {
                        banco = lstConstruplan.banco,
                        factoraje = string.Empty,
                        semana1 = lstConstruplan.semana1,
                        semana2 = lstConstruplan.semana2,
                        semana3 = lstConstruplan.semana3,
                        semana4 = lstConstruplan.semana4,
                        semana5 = lstConstruplan.semana5,
                        semana6 = lstConstruplan.semana6,
                        semana7 = lstConstruplan.semana7,
                        semana8 = lstConstruplan.semana8,
                        semana9 = lstConstruplan.semana9,
                        semana10 = lstConstruplan.semana10,
                        semana11 = lstConstruplan.semana11,
                        semana12 = lstConstruplan.semana12,
                        semana13 = lstConstruplan.semana13,
                        semana14 = lstConstruplan.semana14,
                        semana15 = lstConstruplan.semana15,
                        semana16 = lstConstruplan.semana16,
                        semana17 = lstConstruplan.semana17,
                        semana18 = lstConstruplan.semana18,
                        semana19 = lstConstruplan.semana19,
                        semana20 = lstConstruplan.semana20,
                        semana21 = lstConstruplan.semana21,
                        semana22 = lstConstruplan.semana22,
                        semana23 = lstConstruplan.semana23,
                        semana24 = lstConstruplan.semana24,
                        semana25 = lstConstruplan.semana25,
                        semana26 = lstConstruplan.semana26,
                        semana27 = lstConstruplan.semana27,
                        semana28 = lstConstruplan.semana28,
                        semana29 = lstConstruplan.semana29,
                        semana30 = lstConstruplan.semana30,
                        semana31 = lstConstruplan.semana31,
                        semana32 = lstConstruplan.semana32,
                        semana33 = lstConstruplan.semana33,
                        semana34 = lstConstruplan.semana34,
                        semana35 = lstConstruplan.semana35,
                        semana36 = lstConstruplan.semana36,
                        semana37 = lstConstruplan.semana37,
                        semana38 = lstConstruplan.semana38,
                        semana39 = lstConstruplan.semana39,
                        semana40 = lstConstruplan.semana40,
                        semana41 = lstConstruplan.semana41,
                        semana42 = lstConstruplan.semana42,
                        semana43 = lstConstruplan.semana43,
                        semana44 = lstConstruplan.semana44,
                        semana45 = lstConstruplan.semana45,
                        semana46 = lstConstruplan.semana46,
                        semana47 = lstConstruplan.semana47,
                        semana48 = lstConstruplan.semana48,
                        semana49 = lstConstruplan.semana49,
                        semana50 = lstConstruplan.semana50,
                        semana51 = lstConstruplan.semana51,
                        semana52 = lstConstruplan.semana52
                    }
                });
                var lstOverhaul = lstInividual.Where(x => isOverhaul(x.centro_costos, x.proveedor))
                    .GroupBy(x => new { x.banco, x.factoraje, x.tipoMoneda, x.proveedor, x.fechaVencimientoS }, (key, group) => new
                        {
                            banco = key.banco,
                            factoraje = key.factoraje,
                            moneda = key.tipoMoneda,
                            fecha = group.FirstOrDefault().fecha,
                            semana = key.fechaVencimientoS,
                            saldoFactura = group.Select(s => new { monto = s.saldoFactura.ParseDecimal(), cambio = s.tipoCambio }).ToList()
                        }).Distinct().ToList()
                    .GroupBy(x => new { x.banco, x.factoraje, x.moneda, x.semana }, (key, group) => new
                    {
                        banco = nombreBancoAcomulado(key.banco, key.factoraje),
                        factoraje = key.factoraje,
                        moneda = key.moneda,
                        columna = numeroSemana(group.FirstOrDefault().fecha),
                        semana = key.semana,
                        monto = group.Sum(s => s.saldoFactura.Sum(ss => ss.monto * ss.cambio))
                    })
                    .Distinct()
                    .OrderBy(x => x.banco)
                    .ThenBy(x => x.moneda)
                    .ThenBy(x => x.columna)
                    .ToList()
                    .GroupBy(x => new { x.moneda }, (key, group) => new AcomuladoAnualDTO
                    {
                        banco = string.Empty,
                        factoraje = string.Empty,
                        moneda = key.moneda,
                        semana1 = group.Where(w => w.columna == 1).Sum(s => s.monto),
                        semana2 = group.Where(w => w.columna == 2).Sum(s => s.monto),
                        semana3 = group.Where(w => w.columna == 3).Sum(s => s.monto),
                        semana4 = group.Where(w => w.columna == 4).Sum(s => s.monto),
                        semana5 = group.Where(w => w.columna == 5).Sum(s => s.monto),
                        semana6 = group.Where(w => w.columna == 6).Sum(s => s.monto),
                        semana7 = group.Where(w => w.columna == 7).Sum(s => s.monto),
                        semana8 = group.Where(w => w.columna == 8).Sum(s => s.monto),
                        semana9 = group.Where(w => w.columna == 9).Sum(s => s.monto),
                        semana10 = group.Where(w => w.columna == 10).Sum(s => s.monto),
                        semana11 = group.Where(w => w.columna == 11).Sum(s => s.monto),
                        semana12 = group.Where(w => w.columna == 12).Sum(s => s.monto),
                        semana13 = group.Where(w => w.columna == 13).Sum(s => s.monto),
                        semana14 = group.Where(w => w.columna == 14).Sum(s => s.monto),
                        semana15 = group.Where(w => w.columna == 15).Sum(s => s.monto),
                        semana16 = group.Where(w => w.columna == 16).Sum(s => s.monto),
                        semana17 = group.Where(w => w.columna == 17).Sum(s => s.monto),
                        semana18 = group.Where(w => w.columna == 18).Sum(s => s.monto),
                        semana19 = group.Where(w => w.columna == 19).Sum(s => s.monto),
                        semana20 = group.Where(w => w.columna == 20).Sum(s => s.monto),
                        semana21 = group.Where(w => w.columna == 21).Sum(s => s.monto),
                        semana22 = group.Where(w => w.columna == 22).Sum(s => s.monto),
                        semana23 = group.Where(w => w.columna == 23).Sum(s => s.monto),
                        semana24 = group.Where(w => w.columna == 24).Sum(s => s.monto),
                        semana25 = group.Where(w => w.columna == 25).Sum(s => s.monto),
                        semana26 = group.Where(w => w.columna == 26).Sum(s => s.monto),
                        semana27 = group.Where(w => w.columna == 27).Sum(s => s.monto),
                        semana28 = group.Where(w => w.columna == 28).Sum(s => s.monto),
                        semana29 = group.Where(w => w.columna == 29).Sum(s => s.monto),
                        semana30 = group.Where(w => w.columna == 30).Sum(s => s.monto),
                        semana31 = group.Where(w => w.columna == 31).Sum(s => s.monto),
                        semana32 = group.Where(w => w.columna == 32).Sum(s => s.monto),
                        semana33 = group.Where(w => w.columna == 33).Sum(s => s.monto),
                        semana34 = group.Where(w => w.columna == 34).Sum(s => s.monto),
                        semana35 = group.Where(w => w.columna == 35).Sum(s => s.monto),
                        semana36 = group.Where(w => w.columna == 36).Sum(s => s.monto),
                        semana37 = group.Where(w => w.columna == 37).Sum(s => s.monto),
                        semana38 = group.Where(w => w.columna == 38).Sum(s => s.monto),
                        semana39 = group.Where(w => w.columna == 39).Sum(s => s.monto),
                        semana40 = group.Where(w => w.columna == 40).Sum(s => s.monto),
                        semana41 = group.Where(w => w.columna == 41).Sum(s => s.monto),
                        semana42 = group.Where(w => w.columna == 42).Sum(s => s.monto),
                        semana43 = group.Where(w => w.columna == 43).Sum(s => s.monto),
                        semana44 = group.Where(w => w.columna == 44).Sum(s => s.monto),
                        semana45 = group.Where(w => w.columna == 45).Sum(s => s.monto),
                        semana46 = group.Where(w => w.columna == 46).Sum(s => s.monto),
                        semana47 = group.Where(w => w.columna == 47).Sum(s => s.monto),
                        semana48 = group.Where(w => w.columna == 48).Sum(s => s.monto),
                        semana49 = group.Where(w => w.columna == 49).Sum(s => s.monto),
                        semana50 = group.Where(w => w.columna == 50).Sum(s => s.monto),
                        semana51 = group.Where(w => w.columna == 51).Sum(s => s.monto),
                        semana52 = group.Where(w => w.columna == 52).Sum(s => s.monto),
                    }).ToList();
                var objDiff = new AcomuladoAnualDTO()
                {
                    banco = string.Empty,
                    factoraje = string.Empty,
                    semana1 = lstConstruplan.semana1 - lstOverhaul.Sum(s => s.semana1),
                    semana2 = lstConstruplan.semana2 - lstOverhaul.Sum(s => s.semana2),
                    semana3 = lstConstruplan.semana3 - lstOverhaul.Sum(s => s.semana3),
                    semana4 = lstConstruplan.semana4 - lstOverhaul.Sum(s => s.semana4),
                    semana5 = lstConstruplan.semana5 - lstOverhaul.Sum(s => s.semana5),
                    semana6 = lstConstruplan.semana6 - lstOverhaul.Sum(s => s.semana6),
                    semana7 = lstConstruplan.semana7 - lstOverhaul.Sum(s => s.semana7),
                    semana8 = lstConstruplan.semana8 - lstOverhaul.Sum(s => s.semana8),
                    semana9 = lstConstruplan.semana9 - lstOverhaul.Sum(s => s.semana9),
                    semana10 = lstConstruplan.semana10 - lstOverhaul.Sum(s => s.semana10),
                    semana11 = lstConstruplan.semana11 - lstOverhaul.Sum(s => s.semana11),
                    semana12 = lstConstruplan.semana12 - lstOverhaul.Sum(s => s.semana12),
                    semana13 = lstConstruplan.semana13 - lstOverhaul.Sum(s => s.semana13),
                    semana14 = lstConstruplan.semana14 - lstOverhaul.Sum(s => s.semana14),
                    semana15 = lstConstruplan.semana15 - lstOverhaul.Sum(s => s.semana15),
                    semana16 = lstConstruplan.semana16 - lstOverhaul.Sum(s => s.semana16),
                    semana17 = lstConstruplan.semana17 - lstOverhaul.Sum(s => s.semana17),
                    semana18 = lstConstruplan.semana18 - lstOverhaul.Sum(s => s.semana18),
                    semana19 = lstConstruplan.semana19 - lstOverhaul.Sum(s => s.semana19),
                    semana20 = lstConstruplan.semana20 - lstOverhaul.Sum(s => s.semana20),
                    semana21 = lstConstruplan.semana21 - lstOverhaul.Sum(s => s.semana21),
                    semana22 = lstConstruplan.semana22 - lstOverhaul.Sum(s => s.semana22),
                    semana23 = lstConstruplan.semana23 - lstOverhaul.Sum(s => s.semana23),
                    semana24 = lstConstruplan.semana24 - lstOverhaul.Sum(s => s.semana24),
                    semana25 = lstConstruplan.semana25 - lstOverhaul.Sum(s => s.semana25),
                    semana26 = lstConstruplan.semana26 - lstOverhaul.Sum(s => s.semana26),
                    semana27 = lstConstruplan.semana27 - lstOverhaul.Sum(s => s.semana27),
                    semana28 = lstConstruplan.semana28 - lstOverhaul.Sum(s => s.semana28),
                    semana29 = lstConstruplan.semana29 - lstOverhaul.Sum(s => s.semana29),
                    semana30 = lstConstruplan.semana30 - lstOverhaul.Sum(s => s.semana30),
                    semana31 = lstConstruplan.semana31 - lstOverhaul.Sum(s => s.semana31),
                    semana32 = lstConstruplan.semana32 - lstOverhaul.Sum(s => s.semana32),
                    semana33 = lstConstruplan.semana33 - lstOverhaul.Sum(s => s.semana33),
                    semana34 = lstConstruplan.semana34 - lstOverhaul.Sum(s => s.semana34),
                    semana35 = lstConstruplan.semana35 - lstOverhaul.Sum(s => s.semana35),
                    semana36 = lstConstruplan.semana36 - lstOverhaul.Sum(s => s.semana36),
                    semana37 = lstConstruplan.semana37 - lstOverhaul.Sum(s => s.semana37),
                    semana38 = lstConstruplan.semana38 - lstOverhaul.Sum(s => s.semana38),
                    semana39 = lstConstruplan.semana39 - lstOverhaul.Sum(s => s.semana39),
                    semana40 = lstConstruplan.semana40 - lstOverhaul.Sum(s => s.semana40),
                    semana41 = lstConstruplan.semana41 - lstOverhaul.Sum(s => s.semana41),
                    semana42 = lstConstruplan.semana42 - lstOverhaul.Sum(s => s.semana42),
                    semana43 = lstConstruplan.semana43 - lstOverhaul.Sum(s => s.semana43),
                    semana44 = lstConstruplan.semana44 - lstOverhaul.Sum(s => s.semana44),
                    semana45 = lstConstruplan.semana45 - lstOverhaul.Sum(s => s.semana45),
                    semana46 = lstConstruplan.semana46 - lstOverhaul.Sum(s => s.semana46),
                    semana47 = lstConstruplan.semana47 - lstOverhaul.Sum(s => s.semana47),
                    semana48 = lstConstruplan.semana48 - lstOverhaul.Sum(s => s.semana48),
                    semana49 = lstConstruplan.semana49 - lstOverhaul.Sum(s => s.semana49),
                    semana50 = lstConstruplan.semana50 - lstOverhaul.Sum(s => s.semana50),
                    semana51 = lstConstruplan.semana51 - lstOverhaul.Sum(s => s.semana51),
                    semana52 = lstConstruplan.semana52 - lstOverhaul.Sum(s => s.semana52),
                };
                lstAcomulado.AddRange(new List<AcomuladoAnualDTO>() {
                    new AcomuladoAnualDTO()
                {
                    banco = "OVERHAUL",
                    factoraje = string.Empty,
                    semana1 = lstOverhaul.Sum(s => s.semana1),
                    semana2 = lstOverhaul.Sum(s => s.semana2),
                    semana3 = lstOverhaul.Sum(s => s.semana3),
                    semana4 = lstOverhaul.Sum(s => s.semana4),
                    semana5 = lstOverhaul.Sum(s => s.semana5),
                    semana6 = lstOverhaul.Sum(s => s.semana6),
                    semana7 = lstOverhaul.Sum(s => s.semana7),
                    semana8 = lstOverhaul.Sum(s => s.semana8),
                    semana9 = lstOverhaul.Sum(s => s.semana9),
                    semana10 = lstOverhaul.Sum(s => s.semana10),
                    semana11 = lstOverhaul.Sum(s => s.semana11),
                    semana12 = lstOverhaul.Sum(s => s.semana12),
                    semana13 = lstOverhaul.Sum(s => s.semana13),
                    semana14 = lstOverhaul.Sum(s => s.semana14),
                    semana15 = lstOverhaul.Sum(s => s.semana15),
                    semana16 = lstOverhaul.Sum(s => s.semana16),
                    semana17 = lstOverhaul.Sum(s => s.semana17),
                    semana18 = lstOverhaul.Sum(s => s.semana18),
                    semana19 = lstOverhaul.Sum(s => s.semana19),
                    semana20 = lstOverhaul.Sum(s => s.semana20),
                    semana21 = lstOverhaul.Sum(s => s.semana21),
                    semana22 = lstOverhaul.Sum(s => s.semana22),
                    semana23 = lstOverhaul.Sum(s => s.semana23),
                    semana24 = lstOverhaul.Sum(s => s.semana24),
                    semana25 = lstOverhaul.Sum(s => s.semana25),
                    semana26 = lstOverhaul.Sum(s => s.semana26),
                    semana27 = lstOverhaul.Sum(s => s.semana27),
                    semana28 = lstOverhaul.Sum(s => s.semana28),
                    semana29 = lstOverhaul.Sum(s => s.semana29),
                    semana30 = lstOverhaul.Sum(s => s.semana30),
                    semana31 = lstOverhaul.Sum(s => s.semana31),
                    semana32 = lstOverhaul.Sum(s => s.semana32),
                    semana33 = lstOverhaul.Sum(s => s.semana33),
                    semana34 = lstOverhaul.Sum(s => s.semana34),
                    semana35 = lstOverhaul.Sum(s => s.semana35),
                    semana36 = lstOverhaul.Sum(s => s.semana36),
                    semana37 = lstOverhaul.Sum(s => s.semana37),
                    semana38 = lstOverhaul.Sum(s => s.semana38),
                    semana39 = lstOverhaul.Sum(s => s.semana39),
                    semana40 = lstOverhaul.Sum(s => s.semana40),
                    semana41 = lstOverhaul.Sum(s => s.semana41),
                    semana42 = lstOverhaul.Sum(s => s.semana42),
                    semana43 = lstOverhaul.Sum(s => s.semana43),
                    semana44 = lstOverhaul.Sum(s => s.semana44),
                    semana45 = lstOverhaul.Sum(s => s.semana45),
                    semana46 = lstOverhaul.Sum(s => s.semana46),
                    semana47 = lstOverhaul.Sum(s => s.semana47),
                    semana48 = lstOverhaul.Sum(s => s.semana48),
                    semana49 = lstOverhaul.Sum(s => s.semana49),
                    semana50 = lstOverhaul.Sum(s => s.semana50),
                    semana51 = lstOverhaul.Sum(s => s.semana51),
                    semana52 = lstOverhaul.Sum(s => s.semana52),
                },
                new AcomuladoAnualDTO()
                {
                    banco = string.Empty,
                    factoraje = string.Empty,
                    semana1 = objDiff.semana1,
                    semana2 = objDiff.semana2,
                    semana3 = objDiff.semana3,
                    semana4 = objDiff.semana4,
                    semana5 = objDiff.semana5,
                    semana6 = objDiff.semana6,
                    semana7 = objDiff.semana7,
                    semana8 = objDiff.semana8,
                    semana9 = objDiff.semana9,
                    semana10 = objDiff.semana10,
                    semana11 = objDiff.semana11,
                    semana12 = objDiff.semana12,
                    semana13 = objDiff.semana13,
                    semana14 = objDiff.semana14,
                    semana15 = objDiff.semana15,
                    semana16 = objDiff.semana16,
                    semana17 = objDiff.semana17,
                    semana18 = objDiff.semana18,
                    semana19 = objDiff.semana19,
                    semana20 = objDiff.semana20,
                    semana21 = objDiff.semana21,
                    semana22 = objDiff.semana22,
                    semana23 = objDiff.semana23,
                    semana24 = objDiff.semana24,
                    semana25 = objDiff.semana25,
                    semana26 = objDiff.semana26,
                    semana27 = objDiff.semana27,
                    semana28 = objDiff.semana28,
                    semana29 = objDiff.semana29,
                    semana30 = objDiff.semana30,
                    semana31 = objDiff.semana31,
                    semana32 = objDiff.semana32,
                    semana33 = objDiff.semana33,
                    semana34 = objDiff.semana34,
                    semana35 = objDiff.semana35,
                    semana36 = objDiff.semana36,
                    semana37 = objDiff.semana37,
                    semana38 = objDiff.semana38,
                    semana39 = objDiff.semana39,
                    semana40 = objDiff.semana40,
                    semana41 = objDiff.semana41,
                    semana42 = objDiff.semana42,
                    semana43 = objDiff.semana43,
                    semana44 = objDiff.semana44,
                    semana45 = objDiff.semana45,
                    semana46 = objDiff.semana46,
                    semana47 = objDiff.semana47,
                    semana48 = objDiff.semana48,
                    semana49 = objDiff.semana49,
                    semana50 = objDiff.semana50,
                    semana51 = objDiff.semana51,
                    semana52 = objDiff.semana52,
                }
            });
                Session["lstAcomulado"] = lstAcomulado;
                var arrAcomulado = lstAcomulado.Select(x => new string[]{
                    x.banco,
                    x.semana1 == 0 ? "-" : x.semana1.ToString("C2"),
                    x.semana2 == 0 ? "-" : x.semana2.ToString("C2"),
                    x.semana3 == 0 ? "-" : x.semana3.ToString("C2"),
                    x.semana4 == 0 ? "-" : x.semana4.ToString("C2"),
                    x.semana5 == 0 ? "-" : x.semana5.ToString("C2"),
                    x.semana6 == 0 ? "-" : x.semana6.ToString("C2"),
                    x.semana7 == 0 ? "-" : x.semana7.ToString("C2"),
                    x.semana8 == 0 ? "-" : x.semana8.ToString("C2"),
                    x.semana9 == 0 ? "-" : x.semana9.ToString("C2"),
                    x.semana10 == 0 ? "-" : x.semana10.ToString("C2"),
                    x.semana11 == 0 ? "-" : x.semana11.ToString("C2"),
                    x.semana12 == 0 ? "-" : x.semana12.ToString("C2"),
                    x.semana13 == 0 ? "-" : x.semana13.ToString("C2"),
                    x.semana14 == 0 ? "-" : x.semana14.ToString("C2"),
                    x.semana15 == 0 ? "-" : x.semana15.ToString("C2"),
                    x.semana16 == 0 ? "-" : x.semana16.ToString("C2"),
                    x.semana17 == 0 ? "-" : x.semana17.ToString("C2"),
                    x.semana18 == 0 ? "-" : x.semana18.ToString("C2"),
                    x.semana19 == 0 ? "-" : x.semana19.ToString("C2"),
                    x.semana20 == 0 ? "-" : x.semana20.ToString("C2"),
                    x.semana21 == 0 ? "-" : x.semana21.ToString("C2"),
                    x.semana22 == 0 ? "-" : x.semana22.ToString("C2"),
                    x.semana23 == 0 ? "-" : x.semana23.ToString("C2"),
                    x.semana24 == 0 ? "-" : x.semana24.ToString("C2"),
                    x.semana25 == 0 ? "-" : x.semana25.ToString("C2"),
                    x.semana26 == 0 ? "-" : x.semana26.ToString("C2"),
                    x.semana27 == 0 ? "-" : x.semana27.ToString("C2"),
                    x.semana28 == 0 ? "-" : x.semana28.ToString("C2"),
                    x.semana29 == 0 ? "-" : x.semana29.ToString("C2"),
                    x.semana30 == 0 ? "-" : x.semana30.ToString("C2"),
                    x.semana31 == 0 ? "-" : x.semana31.ToString("C2"),
                    x.semana32 == 0 ? "-" : x.semana32.ToString("C2"),
                    x.semana33 == 0 ? "-" : x.semana33.ToString("C2"),
                    x.semana34 == 0 ? "-" : x.semana34.ToString("C2"),
                    x.semana35 == 0 ? "-" : x.semana35.ToString("C2"),
                    x.semana36 == 0 ? "-" : x.semana36.ToString("C2"),
                    x.semana37 == 0 ? "-" : x.semana37.ToString("C2"),
                    x.semana38 == 0 ? "-" : x.semana38.ToString("C2"),
                    x.semana39 == 0 ? "-" : x.semana39.ToString("C2"),
                    x.semana40 == 0 ? "-" : x.semana40.ToString("C2"),
                    x.semana41 == 0 ? "-" : x.semana41.ToString("C2"),
                    x.semana42 == 0 ? "-" : x.semana42.ToString("C2"),
                    x.semana43 == 0 ? "-" : x.semana43.ToString("C2"),
                    x.semana44 == 0 ? "-" : x.semana44.ToString("C2"),
                    x.semana45 == 0 ? "-" : x.semana45.ToString("C2"),
                    x.semana46 == 0 ? "-" : x.semana46.ToString("C2"),
                    x.semana47 == 0 ? "-" : x.semana47.ToString("C2"),
                    x.semana48 == 0 ? "-" : x.semana48.ToString("C2"),
                    x.semana49 == 0 ? "-" : x.semana49.ToString("C2"),
                    x.semana50 == 0 ? "-" : x.semana50.ToString("C2"),
                    x.semana51 == 0 ? "-" : x.semana51.ToString("C2"),
                    x.semana52 == 0 ? "-" : x.semana52.ToString("C2")
                });
                result.Add("lstRenglon", arrAcomulado.ToArray());
                result.Add(SUCCESS, true);
            //}
            //catch (Exception e)
            //{
            //    Session["lstSemana"] = null;
            //    Session["lstAcomulado"] = null;
            //    result.Add(MESSAGE, e.Message);
            //    result.Add(SUCCESS, false);
            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        bool isOverhaul(string cc, string proveedor)
        {
            var lstCC = new List<string>() { "983", "985", "986", "989" };
            var lsProv = new List<string>() { "118", "245", "3002", "3811", "3890", "9000", "9011", "9373", "9634", "9753" };
            var lstMineria = new List<string>() { "148", "146" };
            var lstMarubeni = new List<string>() { "9075", "602" };
            var ban = lstCC.Exists(x => x.Equals(cc)) && lsProv.Exists(x => x.Equals(proveedor));
            if (!ban)
                ban = lstMineria.Exists(x => x.Equals(cc)) && lstMarubeni.Exists(x => x.Equals(proveedor));
            return ban;
        }

        string asignaCC(string cc, string nomCC)
        {
            if (new List<string>() { "101", "103", "105", "107", "108", "109", "110", "111", "112", "113", "118", "119", "120", "122", "125", "126", "127", "128", "129", "130", "131", "132", "133", "135", "136", "137", "138", "139", "140", "141", "142", "143", "144", "145", "147", "149", "150", "152", "154", "156", "202", "204", "209", "211", "213", "215", "217", "222", "300", "303", "304", "305", "307", "308", "309", "310", "311", "312", "313", "314", "318", "319", "321", "322", "325", "326", "327", "332", "335", "336", "337", "340", "341", "342", "343", "344", "345", "984", "989", "991", "995" }.Exists(w => w.Equals(cc)))
                return "998";
            if (tipoCC(cc, nomCC).Equals("Civil") || new List<string>() { "153", "346", "987", "996" }.Exists(w => w.Equals(cc)))
                return "000";
            //Minado nochebuena
            if (new List<string>() { "985" }.Exists(w => w.Equals(cc)))
                return "159";
            //Minado la colorada
            if (new List<string>() { "983", "986" }.Exists(w => w.Equals(cc)))
                return "146";
            return cc;
        }
        public FileResult getExcelAcomulado()
        {
            var excel = new ExcelUtilities();
            var Sheets = new List<excelSheetDTO>();
            #region Tab1
            var sheetBanco = new excelSheetDTO() { name = string.Format("Acum. pagos por factoraje") };
            var lstSemana = (List<string>)Session["lstSemana"];
            var rows = new List<excelRowDTO>();
            var cells = new List<excelCellDTO>();
            foreach (var x in lstSemana)
            {
                cells.Add(new excelCellDTO() { text = x, autoWidthFit = true, fill = true, border = true, colSpan = 0, rowSpan = 0, borderType = 0 });
            }
            var r = new excelRowDTO() { cells = new List<excelCellDTO>() };
            r.cells.AddRange(cells);
            rows.Add(r);
            var lstAcomulado = (List<AcomuladoAnualDTO>)Session["lstAcomulado"];
            rows.AddRange(
                lstAcomulado.Select(x => new excelRowDTO()
                {
                    cells = new List<excelCellDTO>(){
                        new excelCellDTO{ text=x.banco, autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0},
                        new excelCellDTO{ text= x.semana1 == 0 ? "-" : x.semana1.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana1 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana2 == 0 ? "-" : x.semana2.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana2 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana3 == 0 ? "-" : x.semana3.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana3 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana4 == 0 ? "-" : x.semana4.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana4 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana5 == 0 ? "-" : x.semana5.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana5 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana6 == 0 ? "-" : x.semana6.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana6 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana7 == 0 ? "-" : x.semana7.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana7 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana8 == 0 ? "-" : x.semana8.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana8 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana9 == 0 ? "-" : x.semana9.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana9 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana10 == 0 ? "-" : x.semana10.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana10 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana11 == 0 ? "-" : x.semana11.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana11 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana12 == 0 ? "-" : x.semana12.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana12 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana13 == 0 ? "-" : x.semana13.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana13 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana14 == 0 ? "-" : x.semana14.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana14 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana15 == 0 ? "-" : x.semana15.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana15 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana16 == 0 ? "-" : x.semana16.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana16 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana17 == 0 ? "-" : x.semana17.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana17 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana18 == 0 ? "-" : x.semana18.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana18 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana19 == 0 ? "-" : x.semana19.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana19 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana20 == 0 ? "-" : x.semana20.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana20 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana21 == 0 ? "-" : x.semana21.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana21 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana22 == 0 ? "-" : x.semana22.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana22 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana23 == 0 ? "-" : x.semana23.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana23 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana24 == 0 ? "-" : x.semana24.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana24 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana25 == 0 ? "-" : x.semana25.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana25 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana26 == 0 ? "-" : x.semana26.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana26 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana27 == 0 ? "-" : x.semana27.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana27 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana28 == 0 ? "-" : x.semana28.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana28 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana29 == 0 ? "-" : x.semana29.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana29 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana30 == 0 ? "-" : x.semana30.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana30 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana31 == 0 ? "-" : x.semana31.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana31 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana32 == 0 ? "-" : x.semana32.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana32 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana33 == 0 ? "-" : x.semana33.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana33 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana34 == 0 ? "-" : x.semana34.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana34 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana35 == 0 ? "-" : x.semana35.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana35 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana36 == 0 ? "-" : x.semana36.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana36 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana37 == 0 ? "-" : x.semana37.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana37 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana38 == 0 ? "-" : x.semana38.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana38 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana39 == 0 ? "-" : x.semana39.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana39 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana40 == 0 ? "-" : x.semana40.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana40 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana41 == 0 ? "-" : x.semana41.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana41 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana42 == 0 ? "-" : x.semana42.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana42 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana43 == 0 ? "-" : x.semana43.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana43 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana44 == 0 ? "-" : x.semana44.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana44 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana45 == 0 ? "-" : x.semana45.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana45 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana46 == 0 ? "-" : x.semana46.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana46 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana47 == 0 ? "-" : x.semana47.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana47 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana48 == 0 ? "-" : x.semana48.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana48 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana49 == 0 ? "-" : x.semana49.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana49 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana50 == 0 ? "-" : x.semana50.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana50 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana51 == 0 ? "-" : x.semana51.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana51 == 0 ? 0 : 1},
                        new excelCellDTO{ text= x.semana52 == 0 ? "-" : x.semana52.ToString(), autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0, formatType = x.semana52 == 0 ? 0 : 1}
                    }
                }));
            sheetBanco.Sheet = rows;
            Sheets.Add(sheetBanco);
            #endregion
            excel.CreateExcelFileCadenaProductiva(this, Sheets, string.Format("{0} {1}", sheetBanco.name, DateTime.Now.Year.ToString()));
            return null;
        }
        #endregion
        #region Cadena por Obra
        public ActionResult LoadCadenaObra(DateTime fecha)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = new List<VencimientoDTO>();
                var lstCC = cadenaProductivaFS.lstObra();
                var lstCompleta = cadenaProductivaFS.GetAllDocumentos()
                                    .Where(w => !w.estatus)
                                    .Where(w => DateTime.Compare(w.fechaVencimiento, new DateTime(2017, 11, 20)) >= 0)
                                    .Where(x => lstCC.Exists(c => c.cc.Equals(x.centro_costos) && !c.bit_area.Equals("2") && !c.bit_area.Equals("12") && !c.bit_area.Equals("-2")))
                    .Select(x => new VencimientoDTO
                    {
                        fechaVencimiento = x.fechaVencimiento,
                        fechaVencimientoS = x.fechaVencimiento.ToShortDateString(),
                        proveedor = x.proveedor,
                        numProveedor = int.Parse(x.numProveedor),
                        fechaS = tipoCC(x.centro_costos, x.nombCC).ToUpper(),
                        centro_costos = asignaCC(x.centro_costos, x.nombCC),
                        nombCC = x.nombCC,
                        saldoFactura = (x.saldoFactura * x.tipoCambio).ToString("C2"),
                        monto = x.saldoFactura
                    }).ToList();
                var lstCargoBanco = lstCompleta
                                    .Where(x => x.fechaVencimiento.Year == fecha.Year
                                                && numeroSemana(x.fechaVencimiento) == numeroSemana(fecha))
                                    .Select(x => new VencimientoDTO
                                    {
                                        fechaVencimiento = x.fechaVencimiento,
                                        fechaVencimientoS = x.fechaVencimiento.ToShortDateString(),
                                        concepto = "CARGO BANCARIO",
                                        proveedor = string.Format("CARGO NAFIN {0}", x.proveedor),
                                        fechaS = x.fechaS,
                                        nombCC = x.nombCC,
                                        centro_costos = x.centro_costos,
                                        saldoFactura = x.saldoFactura,
                                        numNafin = isOverhaul(x.centro_costos, x.numProveedor.ToString()) ? x.saldoFactura : string.Empty,
                                        monto = x.monto
                                    }).ToList();
                var lstBase = BaseFS.getHistorico();
                var lstReserva = reservaPropuestaFS.getLstReservasCadena(fecha);
                var lstAcomuladoAnt = lstCompleta
                                        .Where(w => DateTime.Compare(w.fechaVencimiento, fecha) < 0 && !isOverhaul(w.centro_costos, w.proveedor))
                                        .GroupBy(x => new { x.centro_costos }, (k, g) => new
                                        {
                                            cc = k.centro_costos,
                                            suma = g.Sum(s => changeFormat(s.saldoFactura)),
                                            nombre = g.FirstOrDefault().nombCC
                                        }).ToList();
                var lstReservaAnt = lstReserva
                                    .Where(w => DateTime.Compare(w.fecha, fecha) <= 0)
                                    .GroupBy(x => new { x.cc }, (k, g) => new
                                    {
                                        cc = asignaCC(k.cc, lstCC.FirstOrDefault(c => c.cc.Equals(k.cc)).descripcion),
                                        suma = g.Sum(s => s.cargo - s.abono),
                                        nombre = lstCC.FirstOrDefault(c => c.cc.Equals(k.cc)).descripcion
                                    }).ToList();
                var lstObraAnt = lstBase
                     .Select(x => new VencimientoDTO()
                     {
                         centro_costos = x.centro_costos,
                         nombCC = x.nombCC,
                         saldoFactura = (x.total
                                        - (lstAcomuladoAnt.Exists(w => w.cc.Equals(x.centro_costos)) ? lstAcomuladoAnt.FirstOrDefault(w => w.cc.Equals(x.centro_costos)).suma : 0)
                                        + (lstReservaAnt.Exists(w => w.cc.Equals(x.centro_costos)) ? lstReservaAnt.FirstOrDefault(w => w.cc.Equals(x.centro_costos)).suma : 0)
                                        ).ToString("C2")
                     }).ToList();
                var lstCC2 = lstObraAnt.Select(x => new VencimientoDTO()
                {
                    fechaVencimientoS = fecha.AddDays(1).ToShortDateString(),
                    numNafin = x.saldoFactura,
                    fechaS = x.nombCC,
                    centro_costos = x.centro_costos
                }).ToList();
                var lstDesgloceCargo = lstCargoBanco.Where(w => fecha.Year == w.fechaVencimiento.Year && numeroSemana(fecha) == numeroSemana(w.fechaVencimiento)).GroupBy(x => new { x.centro_costos });
                foreach (var item in lstCC2)
                {
                    var objReserva = new tblC_Reserva { fecha = fecha, cargo = 0 };
                    if (lstReserva.Any(w => w.cc.Equals(item.centro_costos) && numeroSemana(w.fecha) == numeroSemana(fecha)))
                        objReserva = lstReserva.FirstOrDefault(w => w.cc.Equals(item.centro_costos) && numeroSemana(w.fecha) == numeroSemana(fecha));
                    lst.AddRange(new List<VencimientoDTO>(){
                        item,
                        new VencimientoDTO(){
                            fechaVencimientoS =   fecha.AddDays(1).ToShortDateString(),
                            concepto = "RESERVA PAGO EN CADENAS",
                            proveedor = string.Format("RESERVA PAGO EN CADENAS {0}", item.fechaS),
                            fechaS = item.fechaS,
                            nombCC = string.Empty,
                            saldoFactura = string.Empty,
                            numNafin = objReserva.cargo.ToString("C2"),
                            monto = 0,
                            centro_costos = item.centro_costos,
                            pagado = DateTime.Compare(fecha, new DateTime(2017, 11, 26)) > 0   
                        }
                    });
                    lst.AddRange(lstDesgloceCargo.Where(x => x.Key.centro_costos.Equals(item.centro_costos)).SelectMany(y => y.OrderByDescending(o => o.fechaVencimiento.Year).ThenByDescending(o => o.fechaVencimiento.Month).ThenByDescending(o => o.fechaVencimiento.Day).ThenBy(o => o.proveedor).ToList()));
                    lst.AddRange(new List<VencimientoDTO>()
                    {
                        new VencimientoDTO()
                        {
                            numNafin = changeFormat(item.numNafin).ToString("C2"),
                            saldoFactura = lstCargoBanco.Where(x => x.centro_costos.Equals(item.centro_costos)).Sum(s => changeFormat(s.saldoFactura)).ToString("C2"),
                            banco = (changeFormat(item.numNafin) - lstCargoBanco.Where(x => x.centro_costos.Equals(item.centro_costos)).Sum(s => changeFormat(s.saldoFactura))).ToString("C2"),
                            factoraje = item.fechaS,
                            centro_costos = item.centro_costos
                        },
                        new VencimientoDTO()
                    });
                };
                var lstObraAct = lst.Where(w => !string.IsNullOrEmpty(w.banco)).ToList();
                lstObraAct.Add(new VencimientoDTO() { factoraje = string.Empty, banco = lstObraAct.Sum(s => changeFormat(s.banco)).ToString("C2") });
                result.Add("lstRes", lst);
                result.Add("lstObra", lstObraAct);
                Session["fecha"] = fecha;
                Session["lstRes"] = lst.Select(x => new VencimientoDTO
                {
                    numNafin = string.IsNullOrEmpty(x.banco) ? x.numNafin : (changeFormat(x.numNafin)
                                    + lstReserva.Where(w => !string.IsNullOrWhiteSpace(x.banco) && w.cc.Equals(x.centro_costos)).Sum(s => s.cargo)
                                    + lstCargoBanco.Where(w => !string.IsNullOrEmpty(x.numNafin) && w.numNafin.Equals(w.saldoFactura) && w.centro_costos.Equals(x.centro_costos)).Sum(s => changeFormat(s.numNafin))
                                ).ToString(),
                    banco = string.IsNullOrEmpty(x.banco) ? string.Empty : (changeFormat(x.banco ?? "0")
                            - lstReserva.Where(w => w.cc.Equals(x.centro_costos)).Sum(s => s.cargo)
                            - lstCargoBanco.Where(w => !string.IsNullOrEmpty(x.numNafin) && x.numNafin.Equals(x.saldoFactura) && w.centro_costos.Equals(x.centro_costos)).Sum(s => changeFormat(s.numNafin))
                            ).ToString(),
                    saldoFactura = x.saldoFactura,
                    factoraje = x.factoraje,
                    centro_costos = x.centro_costos,
                    fechaVencimientoS = x.fechaVencimientoS,
                    concepto = x.concepto,
                    proveedor = x.proveedor,
                    fechaS = x.fechaS,
                    nombCC = x.nombCC,
                }).ToList();
                Session["lstExcelObra"] = lstObraAct
                .Select(x => new VencimientoDTO()
                {
                    factoraje = x.factoraje,
                    banco = (changeFormat(x.banco)
                            - lstReserva.Where(w => w.cc.Equals(x.centro_costos)).Sum(s => s.cargo)
                            - lstCargoBanco.Where(w => !string.IsNullOrEmpty(x.numNafin) && x.numNafin.Equals(x.saldoFactura) && w.centro_costos.Equals(x.centro_costos)).Sum(s => changeFormat(string.IsNullOrEmpty(s.numNafin) ? "0" : s.numNafin))
                            ).ToString()
                }).ToList();
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                Session["lstRes"] = null;
                Session["lstExcelObra"] = null;
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public FileResult getExcelCadenaObra()
        {
            var excel = new ExcelUtilities();
            var Sheets = new List<excelSheetDTO>();
            var fecha = (DateTime)Session["fecha"];
            var sheetBanco = new excelSheetDTO() { name = string.Format("Cadena obra semana {0}", numeroSemana(fecha)) };
            #region Tab1;
            var rows = new List<excelRowDTO>()
            {
                new excelRowDTO() {
                    cells = new List<excelCellDTO>(){
                        new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0 },
                        new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0 },
                        new excelCellDTO{ text ="GRUPO CONSTRUCIONES PLANIFICADAS,S.A.DE C.V.", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0 },
                    }
                    
                },
                new excelRowDTO() {
                    cells = new List<excelCellDTO>(){
                        new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0 },
                        new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0 },
                        new excelCellDTO{ text="RELACION DE RESERVAS PARA CADENAS", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                    }
                    
                },
                new excelRowDTO() {
                    cells = new List<excelCellDTO>(){
                        new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0 },
                        new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0 },
                        new excelCellDTO{ text= string.Format("SEMANA DE {0}", fecha.ToShortDateString()), autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                    }
                    
                },
                new excelRowDTO() {
                    cells = new List<excelCellDTO>(){
                        new excelCellDTO{ text="", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                    }
                },
                new excelRowDTO() {
                    cells = new List<excelCellDTO>(){
                        new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0 },
                        new excelCellDTO{ text = string.Empty, autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0 },
                        new excelCellDTO{ text = "CONCEPTO", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                        new excelCellDTO{ text = "IMPORTE", autoWidthFit=false, fill=true, border=false,colSpan=0,rowSpan=0},
                    }                    
                }                
            };
            var cells = new List<excelCellDTO>();
            var r = new excelRowDTO() { cells = new List<excelCellDTO>() };
            r.cells.AddRange(cells);
            rows.Add(r);
            var lstObra = (List<VencimientoDTO>)Session["lstExcelObra"];
            rows.AddRange(
                lstObra.Select(x => new excelRowDTO()
                {
                    cells = new List<excelCellDTO>(){
                        new excelCellDTO{ text = string.Empty, autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0},
                        new excelCellDTO{ text = string.Empty, autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0},
                        new excelCellDTO{ text = x.factoraje, autoWidthFit=true, fill=true, border = !string.IsNullOrWhiteSpace(x.factoraje), colSpan=0,rowSpan=0, borderType=0},
                        new excelCellDTO{ text = x.banco, autoWidthFit=true, fill=true, border=true, colSpan=0,rowSpan=0, borderType=0, formatType=1},
                    }
                }));
            rows.AddRange(new List<excelRowDTO>(){
                new excelRowDTO() { cells = new List<excelCellDTO>() },
                new excelRowDTO() {
                    cells = new List<excelCellDTO>(){
                        new excelCellDTO{ text = "RELACION DE RESERVAS PARA CADENAS", autoWidthFit = false, fill=true, border = false, colSpan=2,rowSpan=0, borderType=0}
                    }
                },
                new excelRowDTO() { cells = new List<excelCellDTO>() }
            }
                );
            var lstRes = (List<VencimientoDTO>)Session["lstRes"];
            rows.AddRange(
                lstRes.Select(x => new excelRowDTO()
                {
                    cells = new List<excelCellDTO>(){
                        new excelCellDTO{ text = x.fechaVencimientoS ?? string.Empty, autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0},
                        new excelCellDTO{ text = x.concepto ?? string.Empty, autoWidthFit=true, fill=true, border=false,colSpan=0,rowSpan=0, borderType=0},
                        new excelCellDTO{ text = x.proveedor ?? string.Empty, autoWidthFit=true, fill=true, border=false, colSpan=2,rowSpan=0, borderType=0},
                        new excelCellDTO{ text = string.Empty, autoWidthFit=true, fill=true, border=false, colSpan=0,rowSpan=0, borderType=0},
                        new excelCellDTO{ text = x.fechaS ?? string.Empty, autoWidthFit=true, fill=true, border=false, colSpan=0,rowSpan=0, borderType=0},
                        new excelCellDTO{ text = string.IsNullOrEmpty(x.numNafin) ? string.Empty : x.numNafin.Equals("$0.00") ? "-" : changeFormat(x.numNafin).ToString(), autoWidthFit=true, fill=true, border=false, colSpan = 0,rowSpan=0, borderType=0, formatType = string.IsNullOrEmpty(x.numNafin) ? 0 : x.numNafin.Equals("$0.00") ? 0 : 1},
                        new excelCellDTO{ text = string.IsNullOrEmpty(x.saldoFactura) ? string.Empty : changeFormat(x.saldoFactura).ToString() , autoWidthFit=true, fill=true, border=false, colSpan = 0,rowSpan=0, borderType=0, formatType = string.IsNullOrEmpty(x.saldoFactura) ? 0 : 1 },
                        new excelCellDTO{ text = string.IsNullOrEmpty(x.banco) ? string.Empty : changeFormat(x.banco).ToString(), autoWidthFit=true, fill=true, border=false, colSpan = 0,rowSpan=0, borderType=0, formatType = string.IsNullOrEmpty(x.banco) ? 0 : 1},
                    }
                }));
            sheetBanco.Sheet = rows;
            Sheets.Add(sheetBanco);
            #endregion
            excel.CreateExcelFileCadenaProductiva(this, Sheets, sheetBanco.name);
            return null;
        }
        #endregion
        #region NumNafin
        public ActionResult GetLstNafin(int moneda)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = nafinFS.GetLstNafin(moneda);
                result.Add("lstNafin", lst);
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarLstProvNafin(List<tblC_CatNumNafin> lst)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var esGuardado = lst.All(prov => prov.NumProveedor.ParseInt() > 0 && !string.IsNullOrEmpty(prov.NumNafin));
                if (esGuardado)
                {
                    esGuardado = nafinFS.GuardarLstProvNafin(lst);
                }
                result.Add(SUCCESS, esGuardado);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EliminarLstNafin(tblC_CatNumNafin prov)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var vaEliminar = prov.NumProveedor.ParseInt() > 0 && !string.IsNullOrEmpty(prov.NumNafin);
                if (vaEliminar)
                {
                    vaEliminar = nafinFS.eliminarNafinProv(prov);
                }
                result.Add(SUCCESS, vaEliminar);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        List<tblC_CadenaProductiva> getLstVencimiento()
        {
            var lstCompleta = cadenaProductivaFS.GetAllDocumentos();

            return lstCompleta;
        }
        List<tblC_CadenaProductiva> getLstVencimiento2()
        {
            var lstCompleta = cadenaProductivaFS.lstCompletaCadenaProductiva();
            var lstVencimientoDTO = new List<tblC_CadenaProductiva>();
            foreach (var item1 in lstCompleta)
            {
                var datasend = new tblC_CadenaProductiva();
                datasend = item1;
                datasend.proveedor = item1.proveedor ?? "";
                if (item1.factoraje != null)
                {
                    datasend.factoraje = item1.factoraje.Equals("V") ? "Vencido" : "Normal";
                }
                else
                {
                    datasend.factoraje = "Vencido";
                }
                datasend.banco = item1.banco == null ? string.Empty : item1.banco;
                datasend.centro_costos = item1.centro_costos == null ? string.Empty : item1.centro_costos;
                datasend.nombCC = item1.nombCC == null ? string.Empty : item1.nombCC;
                datasend.fecha = item1.fecha == null ? new DateTime() : item1.fecha;
                datasend.fechaVencimiento = item1.fechaVencimiento == null ? new DateTime() : item1.fechaVencimiento;
                lstVencimientoDTO.Add(datasend);
            }
            return lstVencimientoDTO;
        }

        int numeroSemana(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        static DateTime primerDíaSemana(int year, int weekOfYear, System.Globalization.CultureInfo ci)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = (int)ci.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;
            DateTime firstWeekDay = jan1.AddDays(daysOffset);
            int firstWeek = ci.Calendar.GetWeekOfYear(jan1, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);
            if ((firstWeek <= 1 || firstWeek >= 52) && daysOffset >= -3)
            {
                weekOfYear -= 1;
            }
            return firstWeekDay.AddDays(weekOfYear * 7);
        }

        string nombreMes(int mes)
        {
            switch (mes)
            {
                case 1: { return "Ene"; }
                case 2: { return "Feb"; }
                case 3: { return "Mar"; }
                case 4: { return "Abr"; }
                case 5: { return "May"; }
                case 6: { return "Jun"; }
                case 7: { return "Jul"; }
                case 8: { return "Ago"; }
                case 9: { return "Sep"; }
                case 10: { return "Oct"; }
                case 11: { return "Nov"; }
                case 12: { return "Dic"; }
                default: return string.Empty;
            }
        }

        string setRangoSemana(DateTime fecha)
        {
            var noSemana = numeroSemana(fecha);
            var semana = primerDíaSemana(noSemana == 52 ? fecha.AddYears(-1).Year : fecha.Year, noSemana, CultureInfo.CurrentCulture);
            var lunes = semana.AddDays(1);
            var viernes = semana.AddDays(5);
            return string.Format("{0}-{1} {2}", lunes.Day, viernes.Day, nombreMes(viernes.Month));
        }

        List<string> setlstSemana()
        {
            var lst = new List<string>();
            var fecha = new DateTime(DateTime.Now.Year, 1, 1);
            while (fecha.Year == DateTime.Now.Year)
            {
                lst.Add(setRangoSemana(fecha));
                fecha = fecha.AddDays(7);
            }
            lst.Insert(0, "BANCO");
            return lst;
        }
        string tipoCC(string cc, string nombreCC)
        {
            if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Construplan) return nombreCC;
            if (string.IsNullOrEmpty(cc)) return nombreCC;
            if (cc.Substring(0, 1).Equals("C"))
            {
                var icc = Convert.ToInt32(cc.Substring(1, 2));
                if (icc > 58 && icc < 68) return "Industrial";
            }
            else
            {
                var icc = cc.ParseInt();
                if (icc > 523 && icc < 571) return "Industrial";
                if (icc > 39 && icc < 71) return "Industrial";
                if (icc < 25) return "Administración";
            }
            return nombreCC;
        }

        string nombreBancoAcomulado(string banco, string factoraje)
        {
            return string.Format("{0} ( Factoraje {1})", banco, factoraje.Equals("N") ? "Normal" : "Vencido");
        }
        public ActionResult AsignarCC()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstProducitva = cadenaProductivaFS.GetAllDocumentos().Where(x => x.centro_costos.Equals(string.Empty))
                    .Select(item1 => new tblC_CadenaProductiva()
                    {
                        id = item1.id,
                        idPrincipal = item1.idPrincipal,
                        banco = item1.banco,
                        cif = item1.cif.ToString(),
                        concepto = string.IsNullOrEmpty(item1.concepto) ? string.Empty : item1.concepto,
                        factoraje = item1.factoraje,
                        factura = item1.factura.ToString(),
                        fecha = item1.fecha,
                        fechaVencimiento = item1.fechaVencimiento,
                        IVA = item1.IVA,
                        numNafin = item1.numNafin,
                        numProveedor = item1.numProveedor,
                        pagado = item1.pagado,
                        proveedor = item1.proveedor,
                        saldoFactura = item1.saldoFactura,
                        tipoCambio = item1.tipoCambio,
                        estatus = item1.estatus,
                        tipoMoneda = item1.tipoMoneda,
                        reasignado = item1.reasignado,
                        centro_costos = cadenaProductivaFS.getCCVencimiento(item1.numProveedor, item1.factura),
                        nombCC = centroCostosFS.getNombreCCFix(cadenaProductivaFS.getCCVencimiento(item1.numProveedor, item1.factura)),
                        orden_compra = item1.orden_compra
                    }).ToList();
                lstProducitva.ForEach(x => cadenaProductivaFS.Guardar(x));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AsignaCCPrincipal()
        {
            var result = new Dictionary<string, object>();
            try
            {
                cadenaPrincipalFS.GetAllDocumentos()
                    .Where(x => x.id > 426)
                    .ToList()
                    .ForEach(x => cadenaPrincipalFS.Guardar(new tblC_CadenaPrincipal()
                    {
                        id = x.id,
                        estatus = x.estatus,
                        numProveedor = x.numProveedor.ToString(),
                        proveedor = x.proveedor,
                        total = x.total,
                        banco = x.banco ?? string.Empty,
                        factoraje = x.factoraje ?? string.Empty,
                        fecha = x.fecha,
                        fechaVencimiento = x.fechaVencimiento,
                        pagado = x.pagado,
                        numNafin = x.numNafin,
                        centro_costos = cadenaProductivaFS.getCCVencimiento(x.numProveedor,
                            cadenaProductivaFS.GetAllDocumentos().FirstOrDefault(y => y.idPrincipal == x.id).factura),
                        nombCC = centroCostosFS.getNombreCCFix(cadenaProductivaFS.getCCVencimiento(x.numProveedor,
                        cadenaProductivaFS.GetAllDocumentos().FirstOrDefault(y => y.idPrincipal == x.id).factura))
                    }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AsignarNafin()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstProducitva = cadenaProductivaFS.GetAllDocumentos()
                    .Select(item1 => new tblC_CadenaProductiva()
                    {
                        id = item1.id,
                        idPrincipal = item1.idPrincipal,
                        banco = item1.banco,
                        cif = item1.cif.ToString(),
                        concepto = string.IsNullOrEmpty(item1.concepto) ? string.Empty : item1.concepto,
                        factoraje = item1.factoraje,
                        factura = item1.factura.ToString(),
                        fecha = item1.fecha,
                        fechaVencimiento = item1.fechaVencimiento,
                        IVA = item1.IVA,
                        numNafin = AsignaNumNafin(Convert.ToInt32(item1.numProveedor)),
                        numProveedor = item1.numProveedor,
                        pagado = item1.pagado,
                        proveedor = item1.proveedor,
                        saldoFactura = item1.saldoFactura,
                        tipoCambio = item1.tipoCambio,
                        estatus = item1.estatus,
                        tipoMoneda = item1.tipoMoneda,
                        reasignado = item1.reasignado,
                        centro_costos = item1.centro_costos,
                        nombCC = item1.nombCC,
                        orden_compra = item1.orden_compra,
                    }).ToList();
                lstProducitva.ForEach(x => cadenaProductivaFS.Guardar(x));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RutinaPago(DateTime viernes)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstProducitva = cadenaProductivaFS.GetAllDocumentos().Where(x => !x.pagado && DateTime.Compare(x.fechaVencimiento, new DateTime()) <= 0)
                    .Select(item1 => new tblC_CadenaProductiva()
                    {
                        id = item1.id,
                        idPrincipal = item1.idPrincipal,
                        banco = item1.banco,
                        cif = item1.cif.ToString(),
                        concepto = string.IsNullOrEmpty(item1.concepto) ? string.Empty : item1.concepto,
                        factoraje = item1.factoraje,
                        factura = item1.factura.ToString(),
                        fecha = item1.fecha,
                        fechaVencimiento = item1.fechaVencimiento,
                        IVA = item1.IVA,
                        numNafin = item1.numNafin,
                        numProveedor = item1.numProveedor,
                        pagado = true,
                        proveedor = item1.proveedor,
                        saldoFactura = item1.saldoFactura,
                        tipoCambio = item1.tipoCambio,
                        estatus = item1.estatus,
                        tipoMoneda = item1.tipoMoneda,
                        reasignado = item1.reasignado,
                        centro_costos = item1.centro_costos,
                        nombCC = item1.nombCC,
                        orden_compra = item1.orden_compra,
                    }).ToList();
                lstProducitva.ForEach(x => cadenaProductivaFS.Guardar(x));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReasignaBanco()
        {
            var result = new Dictionary<string, object>();
            try
            {
                cadenaProductivaFS.ReasignaBanco();
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult setDivision(string cc, int div, string ccPrincipal)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var esGuardado = CcDivFS.Guardar(new tblC_CCDivision() { cc = cc, division = div });
                if (!string.IsNullOrEmpty(ccPrincipal))
                {
                    var relCC = new List<tblC_RelCCPropuesta>() { new tblC_RelCCPropuesta() { ccPrincipal = ccPrincipal, ccSecundario = cc } };
                    polizaFS.guarderRelCCPropuesta(relCC);
                }
                result.Add(SUCCESS, esGuardado);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult setLinea(tblC_Linea obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                obj.factoraje = string.IsNullOrEmpty(obj.factoraje) ? string.Empty : obj.factoraje;
                cadenaProductivaFS.Guardar(obj);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #region Combobox
        public ActionResult FillComboAnio()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var list = cadenaProductivaFS.GetAllDocumentos().Where(w => !string.IsNullOrEmpty(w.banco)).GroupBy(x => (x.fechaVencimiento.Year), x => new { x.fechaVencimiento.Year }).Select(x => x.Key).ToList();
                result.Add(ITEMS, list.Select(x => new { Text = x, Value = x }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboProv()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, cadenaProductivaFS.FillComboProv());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboProvMoneda(int moneda)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cbo = cadenaProductivaFS.FillComboProv()
                    .Where(p => moneda.Equals(1) ? p.Value.ParseInt() < 9000 : p.Value.ParseInt() >= 9000).ToList();
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
        public ActionResult FillComboTipoMoneda()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = EnumExtensions.ToCombo<TipoMonedaEnum>();
                result.Add(ITEMS, lst);
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
        #region Anticipos
        public ActionResult GuardarAnticipo(tblC_Anticipo obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                cadenaProductivaFS.Guardar(obj);
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult updateEstatus(bool estatus, int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                cadenaProductivaFS.updateEstatus(estatus, id);
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getObjAnticipo(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add("obj", cadenaProductivaFS.getObjAnticipo(id));
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getLstAnticipo(string numProveedor)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add("lstAnticipos", cadenaProductivaFS.getLstAnticipo(numProveedor));
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Intereses Nafin
        public ActionResult _mdlInteresesNafin()
        {
            return PartialView();
        }
        public ActionResult getCadenasTotales()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = (List<VencimientoDTO>)Session["lstResultado"];
                var fecha = lst.FirstOrDefault().fechaVencimiento;
                var lstInt = cadenaProductivaFS.getlstInteresesNafin(fecha);
                var lstFactoraje = lst.GroupBy(g => new { g.banco, g.factoraje, g.tipoMoneda }).Select(c => new
                {
                    factoraje = c.Key.factoraje,
                    banco = c.Key.banco,
                    moneda = c.Key.tipoMoneda.Equals(1) ? "MX" : "DLL",
                    esVencido = c.Key.factoraje.Equals("V"),
                    totalCP = c.Sum(s => s.saldoFactura.ParseDecimal()), // total de cadenas
                    totalB = c.Key.factoraje.Equals("V") && lstInt.Any(i => i.banco.Equals(c.Key.banco) && i.divisa.Equals(c.Key.tipoMoneda)) ? lstInt.FirstOrDefault(i => i.banco.Equals(c.Key.banco) && i.divisa.Equals(c.Key.tipoMoneda)).totalBanco : 0,
                    intereses = c.Key.factoraje.Equals("V") && lstInt.Any(i => i.banco.Equals(c.Key.banco) && i.divisa.Equals(c.Key.tipoMoneda)) ? lstInt.FirstOrDefault(i => i.banco.Equals(c.Key.banco) && i.divisa.Equals(c.Key.tipoMoneda)).interes : 0,
                }).OrderBy(o => o.factoraje).ToList();
                result.Add("lstFactoraje", lstFactoraje);
                result.Add("esVencido", lstFactoraje.Any(w => w.factoraje.Equals("V")));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult guardarInteresesNafin(List<tblC_InteresesNafin> lst)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var esGuardado = false;
                if (lst.All(i => i.totalCadenas > 0 && i.totalBanco > 0 && i.interes > 0 && i.tipoCambio > 0 && i.divisa > 0 && !string.IsNullOrEmpty(i.banco)))
                {
                    esGuardado = cadenaProductivaFS.guardarInteresesNafin(lst);
                }
                result.Add(SUCCESS, esGuardado);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Catálogo Giro de Proveedores

        public ActionResult saveGiro(tblC_CatGiro giro)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var esGuardado = giro.descripcion.Count() > 0;
                esGuardado = giroProvFS.saveGiro(giro);
                result.Add(SUCCESS, esGuardado);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getLstGiro()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = giroProvFS.getLstGiro().OrderBy(giro => giro.descripcion).ToList();
                var esSucces = lst.Count > 0;
                if (esSucces)
                {
                    result.Add("lst", lst);   
                }
                result.Add(SUCCESS, esSucces);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #region Combobox
        public ActionResult FillComboGiro()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = giroProvFS.getLstGiro().OrderBy(giro => giro.descripcion).ToList()
                    .Select(giro => new Core.DTO.Principal.Generales.ComboDTO()
                    {
                        Text = giro.descripcion,
                        Value = giro.id.ToString(),
                    }).ToList();
                result.Add(ITEMS, lst);
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
        #endregion
    }
}
