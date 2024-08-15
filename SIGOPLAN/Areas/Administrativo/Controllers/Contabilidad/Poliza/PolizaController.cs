using Core.DAO.Contabilidad.Poliza;
using Core.DAO.Contabilidad.Reportes;
using Core.DTO.Contabilidad.Poliza;
using Core.Entity.Administrativo.Contabilidad;
using Core.Enum.Administracion.Propuesta;
using Data.Factory.Contabilidad;
using Data.Factory.Contabilidad.Reportes;
using Data.Factory.Principal.Usuarios;
using Infrastructure.DTO;
using Infrastructure.Utils;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.Contabilidad.Poliza
{
    public class PolizaController : BaseController
    {
        #region Factory
        IPolizaDAO polizaFS;
        ICadenaProductivaDAO cadenaProductivaFS;
        ICadenaPrincipalDAO cadenaPrincipalFS;
        ICatNumNafinDAO nafinFS;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            polizaFS = new PolizaFactoryServices().getPolizaService();
            cadenaProductivaFS = new CadenaProductivaFactoryServices().getCadenaProductivaService();
            cadenaPrincipalFS = new CadenaPrincipalFactoryServices().getCadenaPrincipalService();
            nafinFS = new CatNumNafinFactoryServices().getNafinService();
            base.OnActionExecuting(filterContext);
        }
        #endregion
        // GET: Administrativo/Poliza
        public ActionResult Captura()
        {
            return View();
        }
        public ActionResult Pagar(List<tblC_CadenaProductiva> l)
        {
            var result = new Dictionary<string, object>();
            try
            {
                polizaFS.Pagar(l);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Guardar(VMPolizaDTO o)
        {
            var result = new Dictionary<string, object>();
            try
            {
                polizaFS.Guardar(o);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getPoliza(DateTime perIni, DateTime perFin, int poliza, string tp)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstTipoPoliza = polizaFS.getComboTipoPoliza();
                var lstPoliza = polizaFS.getPolizaEk(perIni, perFin, poliza, tp).Select(x => new
                {
                    POLIZA = x.poliza,
                    TIPOPOLIZA = lstTipoPoliza.FirstOrDefault(w => w.Value.Equals(x.tp)).Text,
                    FECHA = x.fechapol.ToShortDateString(),
                    CARGO = x.cargos.ToString("C2"),
                    ABONO = x.abonos.ToString("C2"),
                    DIFERENCIA = (x.abonos + x.cargos).ToString("C"),
                    ESTATUS = setEstatus(x.status),
                    GENERADA = setGenerada(x.generada),
                    CONCEPTO = x.concepto
                }).ToList();
                result.Add("data", lstPoliza);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> getMovPoliza(DateTime fecha, int poliza, string tp)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstMovPolizas = await polizaFS.agetMovPolizaEk(fecha, poliza, tp);
                var obj = lstMovPolizas.FirstOrDefault();
                var lst = lstMovPolizas.Select(x => new
                {
                    No = x.linea,
                    Cuenta = x.cta,
                    SCta = x.scta,
                    SSCta = x.sscta,
                    D = x.digito,
                    Mov = x.itm,
                    Proveedor = x.numpro,
                    Referencia = x.referencia,
                    cc = x.cc,
                    ac = string.Format("{0}-{1}", x.area, x.cuenta_oc),
                    oc = x.orden_compra ?? string.Empty,
                    isOc = polizaFS.getCuentata(x.cta, x.scta, x.sscta).requiere_oc.Equals("S"),
                    iSistema = polizaFS.getInterfaceSistema(x.cta, x.scta, x.sscta),
                    isInterface = !string.IsNullOrWhiteSpace(polizaFS.getInterfaceSistema(x.cta, x.scta, x.sscta)),
                    Concepto = x.concepto,
                    TipoMovimiento = x.tm,
                    Monto = x.monto.ToString("C")
                }).ToList();
                result.Add("objMovPolizas", obj);
                result.Add("data", lst);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getPolizaDesdeFactura(List<MovProDTO> lstFactura)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstMovPolizas = polizaFS.getPolizaDesdeFactura(lstFactura);
                var i = 0;
                result.Add("data", lstMovPolizas.Select(x => new
                {
                    No = ++i,
                    Cuenta = x.cta,
                    SCta = x.scta,
                    SSCta = x.sscta,
                    D = x.digito,
                    Mov = x.itm,
                    Proveedor = x.numpro,
                    Referencia = x.referencia,
                    cc = x.cc,
                    ac = string.Format("{0}-{1}", x.area, x.cuenta_oc),
                    oc = x.orden_compra ?? string.Empty,
                    isOc = polizaFS.getCuentata(x.cta, x.scta, x.sscta).requiere_oc.Equals("S"),
                    iSistema = polizaFS.getInterfaceSistema(x.cta, x.scta, x.sscta),
                    isInterface = !string.IsNullOrWhiteSpace(polizaFS.getInterfaceSistema(x.cta, x.scta, x.sscta)),
                    Concepto = x.concepto,
                    TipoMovimiento = x.tm,
                    Monto = x.monto
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
        public ActionResult getCtaCadena(List<MovpolDTO> f, bool isIntereses, DateTime dllFecha)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstCta = polizaFS.getCtaCadena(f, isIntereses, dllFecha);
                result.Add("objMovPolizas", lstCta.FirstOrDefault());
                result.Add("lstCta", lstCta.Select(x => new
                {
                    No = x.linea,
                    Cuenta = x.cta,
                    SCta = x.scta,
                    SSCta = x.sscta,
                    D = x.digito,
                    Mov = x.itm,
                    Proveedor = x.numpro,
                    Referencia = x.referencia,
                    cc = x.cc,
                    ac = string.Format("{0}-{1}", x.area, x.cuenta_oc),
                    oc = x.orden_compra ?? string.Empty,
                    isOc = polizaFS.getCuentata(x.cta, x.scta, x.sscta).requiere_oc.Equals("S"),
                    iSistema = polizaFS.getInterfaceSistema(x.cta, x.scta, x.sscta),
                    isInterface = !string.IsNullOrWhiteSpace(polizaFS.getInterfaceSistema(x.cta, x.scta, x.sscta)),
                    Concepto = x.concepto,
                    TipoMovimiento = x.tm,
                    Monto = x.monto.ToString("C")
                }).OrderBy(o => o.No));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCtaCadenaNuevo(List<MovpolDTO> f, bool isIntereses, DateTime dllFecha)
        {
            return Json(polizaFS.GetCtaCadenaNuevo(f, isIntereses, dllFecha), JsonRequestBehavior.AllowGet);
        }
        public ActionResult getProveedor()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add("data", polizaFS.getProveedor());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getFactura(int numPro)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add("data", polizaFS.getFactura(numPro));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCuenta()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add("data", polizaFS.getCuenta());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            //return Json(result, JsonRequestBehavior.AllowGet);
            JsonResult json = new JsonResult();
            json.MaxJsonLength = Int32.MaxValue;
            json.Data = result;
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }
        public ActionResult getCadena(int moneda)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstPal = cadenaPrincipalFS.GetDocumentosGuardados().Where(w => moneda == 1 ? w.numProveedor.ParseInt() < 9000 : w.numProveedor.ParseInt() > 8999);
                var lstResultado = lstPal.Where(w => w.estatus && !w.pagado).Select(x => new
                {
                    id = x.id,
                    numProveedor = x.numProveedor,
                    proveedor = x.proveedor,
                    saldoFactura = x.total.ToString("C2"),
                    fechaS = x.fecha.ToShortDateString(),
                    fechaVencimientoS = x.fechaVencimiento.ToShortDateString(),
                });
                result.Add("data", lstResultado);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCadenaAplicada(int moneda)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstPal = cadenaPrincipalFS.GetDocumentosAplicados().Where(w => moneda == 1 ? w.numProveedor.ParseInt() < 9000 : w.numProveedor.ParseInt() > 8999).ToList();
                var lstAnt = cadenaProductivaFS.getLstAnticipo(moneda);
                var lstProv = cadenaProductivaFS.ListaPRoveedores();
                var lstResultado = lstPal.Where(w => !w.estatus && !w.pagado).Select(x => new
                {
                    id = x.id,
                    numProveedor = x.numProveedor,
                    proveedor = lstProv.FirstOrDefault(p => p.NUMPROVEEDOR == x.numProveedor).RAZONSOCIAL,
                    saldoFactura = x.total.ToString("C2"),
                    fechaS = x.fecha.ToShortDateString(),
                    fechaVencimientoS = x.fechaVencimiento.ToShortDateString(),
                    isAnticipo = lstAnt.Exists(a => a.numProveedor.Equals(x.numProveedor) && a.centro_costos.Equals(x.centro_costos) && a.anticipo.Equals(x.total) && a.fechaVencimiento.Equals(x.fechaVencimiento))
                }).ToList();
                result.Add("data", lstResultado);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getProductiva(List<tblC_CadenaPrincipal> lstid)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstCadena = new List<tblC_CadenaProductiva>();
                lstid.ForEach(x =>
                {
                    if (x.id.Equals(0))
                    {
                        lstCadena.AddRange(cadenaProductivaFS.getLstAnticipo(x.numProveedor)
                            .Where(w => w.fechaVencimiento.Equals(x.fechaVencimiento))
                            .Select(a => new tblC_CadenaProductiva
                            {
                                banco = a.banco,
                                IVA = a.IVA,
                                centro_costos = a.centro_costos,
                                area_cuenta = "0-0",
                                cif = a.cif,
                                concepto = a.concepto,
                                estatus = false,
                                factoraje = a.factoraje,
                                factura = "0",
                                fecha = a.fecha,
                                fechaVencimiento = a.fechaVencimiento,
                                id = 0,
                                idPrincipal = 0,
                                monto = a.monto - a.IVA,
                                nombCC = a.nombCC,
                                numNafin = a.numNafin,
                                numProveedor = a.numProveedor,
                                pagado = false,
                                proveedor = a.proveedor,
                                reasignado = false,
                                saldoFactura = a.anticipo,
                                tipoCambio = a.tipoCambio,
                                tipoMoneda = a.tipoMoneda
                            }));
                    }
                    else
                    {
                        lstCadena.AddRange(cadenaProductivaFS.GetDocumentoPorPrincipal(x.id));
                    }
                });
                result.Add("data", lstCadena);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> getMovPolDescipcion(int cta, int scta, int sscta, string tp, string itm, int numprov, string referencia, string cc, int oc)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var tmDescripcio = string.Empty;
                var iSistema = polizaFS.agetInterfaceSistema(cta, scta, sscta);
                try
                {
                    tmDescripcio = (polizaFS.getComboTipoMovimiento(iSistema.Result)).First(w => w.Value == itm).Text;
                }
                catch (Exception)
                {
                    tmDescripcio = string.Empty;
                }
                var cuenta = polizaFS.agetCuentata(cta, scta, sscta);
                var idesc = polizaFS.agetInterfaceDescripcion(cta, scta, sscta, tp, numprov);
                var reff = polizaFS.agetObjReferencia(referencia, numprov, iSistema.Result);
                var desc = Task.Factory.StartNew(() =>
                {
                    return iSistema.IsFaulted ? "" : tmDescripcio;
                });
                await Task.WhenAll(cuenta, idesc, desc, reff);
                result.Add("cuenta", cuenta.Result);
                result.Add("idesc", idesc.Result);
                result.Add("desc", desc.Result);
                result.Add("iSistema", iSistema.Result);
                result.Add("isInterface", !string.IsNullOrWhiteSpace(iSistema.Result));
                result.Add("reff", reff.Result);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> getiDescipcion(int cta, int scta, int sscta, string tp, string itm, int numprov)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var tmDescripcio = string.Empty;
                var iSistema = polizaFS.agetInterfaceSistema(cta, scta, sscta);
                try
                {
                    tmDescripcio = (polizaFS.getComboTipoMovimiento(iSistema.Result)).First(w => w.Value == itm).Text;
                }
                catch (Exception)
                {
                    tmDescripcio = string.Empty;
                }
                var cuenta = polizaFS.agetCuentata(cta, scta, sscta);
                var idesc = polizaFS.agetInterfaceDescripcion(cta, scta, sscta, tp, numprov);
                var desc = Task.Factory.StartNew(() =>
                {
                    return iSistema.IsFaulted ? "" : tmDescripcio;
                });
                await Task.WhenAll(cuenta, idesc, desc);
                result.Add("cuenta", cuenta.Result);
                result.Add("idesc", idesc.Result);
                result.Add("desc", desc.Result);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCtacompl(int cta, int scta, int sscta, string iSistema, int numprov, string referencia, string tp, int tm, string cc, decimal monto)
        {
            var result = new Dictionary<string, object>();
            try
            {
                switch (string.Format("{0}-{1}-{2}", cta, scta, sscta))
                {
                    case "1110-51-1": numprov = 9676; break;
                    case "1110-50-2": numprov = 9157; break;
                    default: break;
                }
                result.Add("objDll", polizaFS.getCtacompl(cta, scta, sscta));
                result.Add("iva", polizaFS.getCtaIva(iSistema).porcentaje);
                result.Add("reff", polizaFS.getObjReferencia(referencia, numprov, iSistema));
                result.Add("dllReg", polizaFS.GetTipoCambioRegistro(tp, referencia, numprov.ToString(), tm, cc, monto));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCtaIva(string iSistema)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add("objIva", polizaFS.getCtaIva(iSistema));
                result.Add("objDiffCambiaria", polizaFS.getCtaDiffCambiaria(iSistema));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getDolarDelDia(DateTime fecha)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add("dll", cadenaProductivaFS.getDolarDelDia(fecha));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add("fecha", fecha);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getNumPoliza(string tp, DateTime fecha)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add("numPol", polizaFS.getNumPoliza(tp, fecha));
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
        public ActionResult getComboTipoPoliza()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = polizaFS.getComboTipoPoliza();
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
        public ActionResult getComboTipoMovimiento(string iSistema)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = polizaFS.getComboTipoMovimiento(iSistema);
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
        public ActionResult getCC()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, polizaFS.lstObra());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getComboCentroCostos()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, polizaFS.getComboCentroCostos());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getComboAreaCuenta()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, polizaFS.getComboAreaCuenta());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCboTipoMovimiento()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, polizaFS.getComboTipoMovimiento());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getLstDivision()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var divisiones = EnumExtensions.ToCombo<TipoCCEnum>();
                var lst = polizaFS.lstObra().Where(w => w.Prefijo != null && !w.Prefijo.Equals("0")).ToList();
                var relPropuesta = polizaFS.getRelCCPropuesta();
                result.Add("data", lst.Select(x => new
                {
                    cc = x.Text,
                    division = divisiones.FirstOrDefault(d => x.Prefijo.Equals(d.Value.ToString())).Text,
                    ccPrincipal = relPropuesta.Any(r => r.ccSecundario.Equals(x.Value)) ? lst.FirstOrDefault(c => relPropuesta.FirstOrDefault(r => r.ccSecundario.Equals(x.Value)).ccPrincipal.Equals(c.Value)).Text : string.Empty,
                    value = x.Value
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
        public ActionResult getCCPrincipal(string cc)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var ccPrincipal = polizaFS.getRelCCPropuesta(cc).FirstOrDefault().ccPrincipal ?? string.Empty;
                result.Add("ccPrincipal", ccPrincipal);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add("ccPrincipal", string.Empty);
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getLstTipoDivision()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = EnumExtensions.ToCombo<TipoCCEnum>();
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
        public ActionResult getLstTipoProrrateo()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = EnumExtensions.ToCombo<ProrrateoCCEnum>();
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
        public ActionResult getComboOc()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = polizaFS.getComboOc();
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
        string setEstatus(string status)
        {
            switch (status)
            {
                case "A": return "Actualizado";
                case "B": return "Bloqueada";
                case "C": return "Capturada";
                case "E": return "Errónea";
                case "V": return "Validada";
                default: return status;
            }
        }
        string setGenerada(string Gen)
        {
            switch (Gen)
            {
                case "A": return "Administración";
                case "B": return "Bancos";
                case "C": return "Contabilidad";
                case "F": return "Facturación";
                case "I": return "Inventarios";
                case "P": return "Proveedores";
                case "X": return "Clientes";
                default: return Gen;
            }
        }
        #endregion
    }
}