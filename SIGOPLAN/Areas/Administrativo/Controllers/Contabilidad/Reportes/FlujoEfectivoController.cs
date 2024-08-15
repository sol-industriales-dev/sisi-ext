using Core.DAO.Contabilidad.Poliza;
using Core.DAO.Contabilidad.Reportes;
using Core.DAO.Contabilidad.SistemaContable;
using Core.DAO.Facturacion;
using Core.DAO.Principal.Usuarios;
using Core.DTO;
using Core.DTO.Contabilidad;
using Core.DTO.Contabilidad.FlujoEfectivo;
using Core.DTO.Contabilidad.Poliza;
using Core.DTO.Contabilidad.Propuesta.Nomina;
using Core.DTO.Principal.Generales;
using Core.DTO.RecursosHumanos.Desempeno;
using Core.Entity.Administrativo.Contabilidad.FlujoEfectivo;
using Core.Enum.Administracion.FlujoEfectivo;
using Core.Enum.Administracion.Propuesta;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Data.Factory.Contabilidad;
using Data.Factory.Contabilidad.Reportes;
using Data.Factory.Contabilidad.SistemaContable;
using Data.Factory.Facturacion;
using Data.Factory.Principal.Usuarios;
using Infrastructure.Utils;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.Contabilidad.Reportes
{
    public class FlujoEfectivoController : BaseController
    {
        #region init
        private IFlujoEfectivoDAO flujoFS;
        private ICadenaProductivaDAO cadenaFS;
        private IPolizaDAO polizaFS;
        private IMesProcDAO mesFS;
        private IFacturaciónDAO facturaFS;
        private IUsuarioDAO usuarioFS;
        private IFlujoEfectivoArrendadora flujoArrendadoraFS;
        private FlujoEfectivoArrendadoraController flujoEfectivoCtrl;
        private List<CcDTO> lstCC;
        private List<Core.DTO.Principal.Generales.ComboDTO> lstAC;
        private List<CatctaDTO> catCta;
        private List<CatctaDTO> catCtaArrend;
        private List<Core.DTO.Principal.Generales.ComboDTO> lstTm;
        private List<Core.DTO.Contabilidad.Poliza.ProveedorDTO> lstProv;
        private List<Core.DTO.Contabilidad.Poliza.ProveedorDTO> lstProvArren;
        private List<Core.DTO.Facturacion.ComboDTO> lstCliente;
        private List<Core.DTO.Facturacion.ComboDTO> lstClienteArren;
        private List<tblC_FE_CatConcepto> lstConcepto;
        private List<tblC_FE_RelConceptoTm> RelConcepto;
        private List<tblC_FED_CatConcepto> lstConceptoDir;
        private List<tblC_FED_RelConceptoTm> RelConceptoDir;
        private List<tblC_FED_RelConceptoTm> RelConceptoDirArr;
        private List<int> lstCptoOmitir;
        private List<int> lstCptoInvertirSigno;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            flujoFS = new FlujoEfectivoFactoryServices().getFlujoEfectivoService();
            cadenaFS = new CadenaProductivaFactoryServices().getCadenaProductivaService();
            polizaFS = new PolizaFactoryServices().getPolizaService();
            mesFS = new MesProcFactoryServices().getMesPrcService();
            facturaFS = new FacturaFactoryService().getFacturaService();
            usuarioFS = new UsuarioFactoryServices().getUsuarioService();
            flujoEfectivoCtrl = new FlujoEfectivoArrendadoraController();
            flujoArrendadoraFS = new FlujoEfectivoArrendadoraFactorySevices().getFlujoEfectivoService();
            base.OnActionExecuting(filterContext);
        }
        void init()
        {
            lstCC = cadenaFS.lstObra();
            lstAC = flujoArrendadoraFS.getComboAreaCuenta();
            lstTm = polizaFS.getComboTipoMovimiento("B");
            lstProv = polizaFS.getProveedor();
            lstProvArren = polizaFS.CatProveedor(EnkontrolEnum.ArrenProd);
            lstCliente = facturaFS.FillComboClienteNombre();
            lstClienteArren = facturaFS.ComboClienteNombre(EnkontrolEnum.ArrenProd);
            lstConcepto = flujoFS.getCatConceptoActivo();
            RelConcepto = flujoFS.getRelConceptoTm();
            lstConceptoDir = flujoFS.getCatConceptoDirActivo();
            RelConceptoDir = flujoFS.getRelConceptoDirTm();
            RelConceptoDirArr = flujoFS.getRelConceptoDirTmArr();
            catCta = flujoFS.getCatCtaDeudoresDiversios();
            catCta.Add(new CatctaDTO()
            {
                descripcion = "Varias cuentas"
            });
            catCtaArrend = flujoArrendadoraFS.getCatCtaDeudoresDiversios();
            catCtaArrend.Add(new CatctaDTO()
            {
                descripcion = "Varias cuentas"
            });
            lstCptoOmitir = new List<int>() { 25, 31, 27, 28, 33 };
            lstCptoInvertirSigno = new List<int> { 26 };
            lstCptoOmitir.AddRange(lstCptoInvertirSigno);
        }
        void init_Optimizado()
        {

            lstCC = cadenaFS.lstObra();
            lstTm = polizaFS.getComboTipoMovimiento("B");
            lstCptoOmitir = new List<int>() { 25, 31, 27, 28, 33 };
            lstCptoInvertirSigno = new List<int> { 26 };
            lstCptoOmitir.AddRange(lstCptoInvertirSigno);
            lstConceptoDir = flujoFS.getCatConceptoDirActivo();
            RelConceptoDir = flujoFS.getRelConceptoDirTm();
            RelConceptoDirArr = flujoFS.getRelConceptoDirTmArr();
            catCta = flujoFS.getCatCtaDeudoresDiversios();
            catCta.Add(new CatctaDTO()
            {
                descripcion = "Varias cuentas"
            });
        }
        #endregion
        // GET: Administrativo/FlujoEfectivo
        #region Catalogo Operativo
        public ActionResult Conceptos()
        {
            var empresa = vSesiones.sesionEmpresaActual;
            if(empresa != (int)EmpresaEnum.Arrendadora)
            {
                return View();
            }
            else
            {
                return flujoEfectivoCtrl.Conceptos();
            }
        }
        public ActionResult _catConceptos()
        {
            lstConcepto = flujoFS.getCatConceptoActivo();
            RelConcepto = flujoFS.getRelConceptoTm();
            return PartialView();
        }
        public ActionResult getCatConcepto()
        {
            var result = new Dictionary<string, object>();
            try
            {
                if(lstTm == null)
                {
                    init();
                }
                var catalogo = flujoFS.getCatConcepto()
                    .OrderBy(o => o.idpadre).ThenBy(o => o.Concepto).ToList();
                var padre = catalogo.Where(w => w.idpadre == 0).ToList();
                var rel = flujoFS.getRelConceptoTm();
                var lst = catalogo.Where(c => c.idpadre > 0).Select(cat => new
                {
                    id = cat.id,
                    idPadre = cat.idpadre,
                    Concepto = cat.Concepto,
                    esActivo = cat.esActivo,
                    padre = padre.FirstOrDefault(p => p.id == cat.idpadre).Concepto,
                    tm = lstTm.Where(tm => rel.Any(r => r.idConcepto == cat.id && tm.Value.ParseInt() == r.tm)).ToList()
                }).ToList();
                result.Add("lst", lst);
                result.Add(SUCCESS, lst.Count > 0);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult guardarConcepto(tblC_FE_CatConcepto obj, List<int> tm)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if(obj == null || obj.Concepto.Trim().Count() == 0)
                {
                    result.Add(MESSAGE, "El concepto viene vacío.");
                    result.Add(SUCCESS, false);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                var esSucces = false;
                obj.Concepto = obj.Concepto.Trim().ToUpper();
                tm = tm == null ? new List<int>() : tm;
                var esNuevo = obj.idpadre == 0 && tm.Count == 0;
                var esConcepto = obj.idpadre > 0;
                if(esNuevo || esConcepto)
                {
                    var rel = tm.Select(r => new tblC_FE_RelConceptoTm() { tm = r }).ToList();
                    esSucces = flujoFS.guardarConcepto(obj, rel);
                }
                lstConcepto = flujoFS.getCatConceptoActivo();
                RelConcepto = flujoFS.getRelConceptoTm();
                result.Add(SUCCESS, esSucces);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Catálogo Directo
        public ActionResult ConceptosDirecto()
        {
            var empresa = vSesiones.sesionEmpresaActual;
            if(empresa != (int)EmpresaEnum.Arrendadora)
            {
                return View();
            }
            else
            {
                return flujoEfectivoCtrl.ConceptosDirecto();
            }
        }
        public ActionResult _catConceptosDirecto()
        {
            return PartialView();
        }
        public ActionResult getCatConceptoDir()
        {
            var result = new Dictionary<string, object>();
            try
            {
                if(lstTm == null)
                {
                    init();
                }
                var catalogo = flujoFS.getCatConceptoDir()
                    .OrderBy(o => o.idPadre).ThenBy(o => o.Concepto).ToList();
                var padre = catalogo.Where(w => w.idPadre == 0).ToList();
                var rel = flujoFS.getRelConceptoDirTm();
                var lst = catalogo.Where(c => c.idPadre > 0 && c.idPadre < 30).Select(cat => new
                {
                    id = cat.id,
                    idPadre = cat.idPadre,
                    Concepto = cat.Concepto,
                    esActivo = cat.esActivo,
                    operador = cat.operador.Trim(),
                    padre = padre.FirstOrDefault(p => p.id == cat.idPadre).Concepto,
                    tm = lstTm.Where(tm => rel.Any(r => r.idConceptoDir == cat.id && tm.Value.ParseInt() == r.tm)).ToList()
                }).ToList();
                result.Add("lst", lst);
                result.Add(SUCCESS, lst.Count > 0);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult guardarConceptoDir(tblC_FED_CatConcepto obj, List<int> tm)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if(obj == null || obj.Concepto.Trim().Count() == 0)
                {
                    result.Add(MESSAGE, "El concepto viene vacío.");
                    result.Add(SUCCESS, false);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                var esSucces = false;
                obj.Concepto = obj.Concepto.Trim().ToUpper();
                tm = tm == null ? new List<int>() : tm;
                var esNuevo = obj.idPadre == 0 && tm.Count == 0;
                var esConcepto = obj.idPadre > 0;
                if(esNuevo || esConcepto)
                {
                    var rel = tm.Select(r => new tblC_FED_RelConceptoTm() { tm = r }).ToList();
                    esSucces = flujoFS.guardarConceptoDirecto(obj, rel);
                }
                lstConcepto = flujoFS.getCatConceptoActivo();
                RelConcepto = flujoFS.getRelConceptoTm();
                result.Add(SUCCESS, esSucces);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region ¿Que pasa sí?
        public ActionResult ProyeccionDeCierre()
        {
            var empresa = vSesiones.sesionEmpresaActual;
            if(empresa != (int)EmpresaEnum.Arrendadora)
            {
                return View();
            }
            else
            {
                return flujoEfectivoCtrl.ProyeccionDeCierre();
            }
        }
        public ActionResult getLstCptoCierre(BusqProyeccionCierreDTO busq)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if(lstConceptoDir == null)
                {
                    init();
                }
                var lstAcciones = usuarioFS.getLstAccionesActual().Select(s => s.id).ToList();
                var lstPadreCierre = lstConceptoDir.Where(w => w.idPadre == -3).Select(s => s.id).ToList();
                var lstCptoCierre = lstConceptoDir.Where(w => lstPadreCierre.Contains(w.idPadre) && lstAcciones.Contains(w.idAccion)).OrderBy(o => o.orden).ToList();
                var lstPlaneacion = flujoFS.getPlaneacionCierre(busq);
                var lst = lstCptoCierre.Select(s => new
                {
                    id = s.id,
                    concepto = string.Format("({0}) {1}", s.operador.Trim(), s.Concepto),
                    operador = s.operador.Trim(),
                    monto = lstPlaneacion.Where(w => w.idConceptoDir == s.id).Sum(ss => ss.corte)
                }).ToList();
                result.Add("lst", lst);
                result.Add(SUCCESS, lstCptoCierre.Count > 0);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getLstDetProyeccion(BusqProyeccionCierreDTO busq)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if(lstConceptoDir == null)
                {
                    init();
                }
                var lst = flujoFS.getLstDetProyeccionCierre(busq).ToList()
                    .Where(w => busq.idConceptoDir == 29 ? busq.grupo == w.grupo : true).ToList();
                var lstIdGemelo = lst.Select(s => s.idDetProyGemelo).Where(w => w > 0).ToList();
                var lstGemelo = flujoFS.getLstDetProyeccionCierre(lstIdGemelo);
                var lstDet = lst.Select(det => new
                {
                    det.id,
                    det.idConceptoDir,
                    det.tipo,
                    det.cc,
                    det.anio,
                    det.semana,
                    det.descripcion,
                    det.monto,
                    det.naturaleza,
                    det.fecha,
                    det.esActivo,
                    det.fechaRegistro,
                    det.numcte,
                    det.numpro,
                    det.factura,
                    det.fechaFactura,
                    det.grupo,
                    det.idDetProyGemelo,
                    ccDetProyGemelo = lstGemelo.Any(gem => gem.id == det.idDetProyGemelo) ? lstGemelo.FirstOrDefault(gem => gem.id == det.idDetProyGemelo).cc : "N/A"
                }).ToList();
                var lblConcepto = lstConceptoDir.FirstOrDefault(w => w.id == busq.idConceptoDir);
                result.Add("lst", lstDet);
                result.Add("lblConcepto", string.Format("({0}) {1}", lblConcepto.operador.Trim(), lblConcepto.Concepto));
                result.Add(SUCCESS, true);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        public ActionResult guardarDetProyeccionCierre(List<tblC_FED_DetProyeccionCierre> lst)
        {
            var result = new Dictionary<string, object>();
            //try
            //{
            lst = lst.Where(x => x.monto != 0).ToList();
                var esGuardado = lst.Count > 0 && lst.All(proy => proy.idConceptoDir > 0 && proy.cc != null && proy.cc.Length > 0 && proy.monto != 0 && proy.descripcion.Length > 0);
                if(esGuardado)
                {
                    lst = flujoFS.guardarDetProyeccionCierre(lst);
                    esGuardado = lst.Count > 0;
                    var primero = lst.FirstOrDefault();
                    var max = lst.Max(m => m.fecha);
                    if(max.DayOfWeek != DayOfWeek.Saturday)
                    {
                        max = max.Siguiente(DayOfWeek.Saturday);
                    }
                    var busq = new BusqProyeccionCierreDTO()
                    {
                        max = max,
                        min = max.AddDays(-6),
                        lstCC = new List<string>() { primero.cc },
                        idConceptoDir = primero.idConceptoDir,
                        tipo = primero.tipo
                    };
                    var lstPlaneacion = flujoFS.getPlaneacionCierre(busq);
                    var total = lstPlaneacion.FirstOrDefault(w => w.idConceptoDir == busq.idConceptoDir).corte;
                    var lstRes = lst.Select(det => new
                    {
                        det.id,
                        det.idConceptoDir,
                        det.tipo,
                        det.cc,
                        det.anio,
                        det.semana,
                        det.descripcion,
                        det.monto,
                        det.naturaleza,
                        det.fecha,
                        det.esActivo,
                        det.fechaRegistro,
                        det.numcte,
                        det.numpro,
                        det.factura,
                        det.fechaFactura,
                        det.grupo,
                        det.idDetProyGemelo,
                        ccDetProyGemelo = lst.Any(gem => gem.id == det.idDetProyGemelo) ? lst.FirstOrDefault(gem => gem.id == det.idDetProyGemelo).cc : "N/A"
                    });
                    result.Add("total", total);
                    result.Add("lst", lstRes);
                }
                result.Add(SUCCESS, esGuardado);
            //}
            //catch(Exception e)
            //{
            //    result.Add(MESSAGE, e.Message);
            //    result.Add(SUCCESS, false);
            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult eliminarDetProyeccionCierre(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var esGuardado = id > 0;
                if(esGuardado)
                {
                    esGuardado = flujoFS.eliminarDetProyeccionCierre(id);
                }
                result.Add(SUCCESS, esGuardado);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getLstProyeccionCierre(BusqProyeccionCierreDTO busq, bool esProyectado, bool esNoProyectado)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if(lstCC == null)
                {
                    init();
                }
                result.Add("tipo", busq.tipo);
                result.Add("titulo", busq.tipo.GetDescription());
                if(busq.lstCC.Contains("TODOS"))
                {
                    busq.lstCC = lstCC.Select(s => s.cc).ToList();
                }
                var lstSigoplan = flujoFS.getLstDetProyeccionCierre(busq).Where(w => w.tipo == busq.tipo).ToList();
                switch(busq.tipo)
                {
                    case tipoProyeccionCierreEnum.FacturasClientes:
                        var lstFacturaCliente = facturaFS.getLstIngresosXCobrarCxCDTO(busq.lstCC);
                        lstSigoplan.AddRange(lstFacturaCliente);
                        break;
                    case tipoProyeccionCierreEnum.RetencionesClientes:
                        var lstRetencionesClientes = facturaFS.getLstRetencionesClientes(busq);
                        var checar = lstRetencionesClientes.Sum(x => x.monto);
                        lstSigoplan.AddRange(lstRetencionesClientes);
                        break;
                    case tipoProyeccionCierreEnum.MovimientoProveedor:
                        var lstMovimientoProveedor = cadenaFS.getLstMoviminetoProveedor(busq.lstCC);
                        lstSigoplan.AddRange(lstMovimientoProveedor);
                        break;
                    case tipoProyeccionCierreEnum.CadenaProductiva:
                        var lstCadenaProductiva = cadenaFS.getLstCadenaProductiva(busq.lstCC);
                        lstSigoplan.AddRange(lstCadenaProductiva);
                        break;
                    case tipoProyeccionCierreEnum.AmortizacionClientes:
                        //var lstAmortizacionClientes = facturaFS.getLstAmortizacionClientes(busq.lstCC);
                        var lstAmortizacionClientes = facturaFS.getLstAmortizacionClientes(busq);
                        lstSigoplan.AddRange(lstAmortizacionClientes);
                        break;
                    case tipoProyeccionCierreEnum.MovimientoArrendadora:
                        var lstMovimientoArrendadora = cadenaFS.getLstMoviminetoArrendadora(busq.lstCC);
                        lstSigoplan.AddRange(lstMovimientoArrendadora);
                        break;
                    case tipoProyeccionCierreEnum.AnticipoClientes:
                        var lstAnticipoClientes = facturaFS.getLstAnticipoClientes(busq);
                        lstSigoplan.AddRange(lstAnticipoClientes);
                        break;
                    case tipoProyeccionCierreEnum.AnticipoContratista:
                        var lstAnticipoContratista = facturaFS.getLstAnticipoContratistas(busq);
                        lstSigoplan.AddRange(lstAnticipoContratista);
                        break;
                    case tipoProyeccionCierreEnum.RetencionContratistas:
                        var lstRetencionContratista = facturaFS.getLstRetencionContratistas(busq);
                        lstSigoplan.AddRange(lstRetencionContratista);
                        break;
                    default:
                        break;
                }
                if (busq.tipo == tipoProyeccionCierreEnum.AnticipoContratista)
                {
                    lstSigoplan = lstSigoplan.GroupBy(o => new { cc = o.cc.Trim(), numcte = o.numcte, numpro = o.numpro, factura = o.factura, o.descripcion })
                    .OrderBy(g => g.Key.cc).ThenBy(g => g.Key.numcte).ThenBy(g => g.Key.numpro).ThenBy(g => g.Key.factura)
                    .Select(s => new tblC_FED_DetProyeccionCierre
                    {
                        ac = s.FirstOrDefault().ac,
                        anio = s.FirstOrDefault().anio,
                        cc = s.FirstOrDefault().cc,
                        descripcion = s.Key.descripcion,
                        empresa = s.FirstOrDefault().empresa,
                        esActivo = s.FirstOrDefault().esActivo,
                        factura = s.FirstOrDefault().factura,
                        fecha = s.FirstOrDefault().fecha,
                        fechaFactura = s.FirstOrDefault().fechaFactura,
                        fechaRegistro = s.FirstOrDefault().fechaRegistro,
                        grupo = s.FirstOrDefault().grupo,
                        grupoID = s.FirstOrDefault().grupoID,
                        id = s.FirstOrDefault().id,
                        idConceptoDir = s.FirstOrDefault().idConceptoDir,
                        idDetProyGemelo = s.FirstOrDefault().idDetProyGemelo,
                        monto = s.Sum(x => x.monto),
                        naturaleza = s.FirstOrDefault().naturaleza,
                        numcte = s.FirstOrDefault().numcte,
                        numpro = s.FirstOrDefault().numpro,
                        semana = s.FirstOrDefault().semana,
                        tipo = s.FirstOrDefault().tipo
                    }
                    )
                    .Where(w => w.esActivo)
                    .Where(w => esProyectado && esNoProyectado ? true :
                                esProyectado && !esNoProyectado ? w.id > 0 :
                                !esProyectado && esNoProyectado ? w.id == 0 : false)
                    .ToList();
                }
                else
                {
                    lstSigoplan = lstSigoplan.GroupBy(o => new { cc = o.cc.Trim(), numcte = o.numcte, numpro = o.numpro, factura = o.factura })
                        .OrderBy(g => g.Key.cc).ThenBy(g => g.Key.numcte).ThenBy(g => g.Key.numpro).ThenBy(g => g.Key.factura)
                        .Select(s => new tblC_FED_DetProyeccionCierre
                            {
                                ac = s.FirstOrDefault().ac,
                                anio = s.FirstOrDefault().anio,
                                cc = s.FirstOrDefault().cc,
                                descripcion = s.FirstOrDefault().descripcion,
                                empresa = s.FirstOrDefault().empresa,
                                esActivo = s.FirstOrDefault().esActivo,
                                factura = s.FirstOrDefault().factura,
                                fecha = s.FirstOrDefault().fecha,
                                fechaFactura = s.FirstOrDefault().fechaFactura,
                                fechaRegistro = s.FirstOrDefault().fechaRegistro,
                                grupo = s.FirstOrDefault().grupo,
                                grupoID = s.FirstOrDefault().grupoID,
                                id = s.FirstOrDefault().id,
                                idConceptoDir = s.FirstOrDefault().idConceptoDir,
                                idDetProyGemelo = s.FirstOrDefault().idDetProyGemelo,
                                monto = s.Sum(x => x.monto),
                                naturaleza = s.FirstOrDefault().naturaleza,
                                numcte = s.FirstOrDefault().numcte,
                                numpro = s.FirstOrDefault().numpro,
                                semana = s.FirstOrDefault().semana,
                                tipo = s.FirstOrDefault().tipo
                            }
                        )
                        .Where(w => w.esActivo)
                        .Where(w => esProyectado && esNoProyectado ? true :
                                    esProyectado && !esNoProyectado ? w.id > 0 :
                                    !esProyectado && esNoProyectado ? w.id == 0 : false)
                        .ToList();
                }
                var totalFacturaCliente = lstSigoplan.Sum(s => s.monto);
                var esGuardado = lstSigoplan.Count > 0;
                result.Add("lst", lstSigoplan);
                result.Add("total", totalFacturaCliente);
                result.Add(SUCCESS, esGuardado);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getLstGpoReserva()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = flujoFS.getAllGpoReserva();
                var esGuardado = lst.Count > 0;
                result.Add("lst", lst);
                result.Add(SUCCESS, esGuardado);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarGpoReserva(tblC_FED_CatGrupoReserva gpo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var esGuardado = gpo.grupo.Trim().Length > 0;
                if(esGuardado)
                {
                    esGuardado = flujoFS.GuardarGpoReserva(gpo);
                }
                result.Add(SUCCESS, esGuardado);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Movimiento Polizas Operativo
        public ActionResult MovPoliza()
        {
            var empresa = vSesiones.sesionEmpresaActual;
            if(empresa != (int)EmpresaEnum.Arrendadora)
            {
                return View();
            }
            else
            {
                return flujoEfectivoCtrl.MovPoliza();
            }
        }
        public ActionResult _movPoliza()
        {
            return PartialView();
        }
        public ActionResult _tblMpMovPol()
        {
            return PartialView();
        }
        public ActionResult _btnMpCorte()
        {
            return PartialView();
        }
        public ActionResult getLstMovPoliza(BusqFlujoEfectivoDTO busq)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if(catCta == null)
                {
                    init();
                }
                busq = ValidaBusquedaOperativo(busq);
                var lstMovPol = flujoFS.getLstMovPol(busq);
                var lstMov = flujoFS.getLstMovPolActiva(busq);
                var lst = lstMovPol
                        .Select(enk => new
                        {
                            enk = enk,
                            sig = getMovimientoFromSigoplan(lstMov, enk)
                        })
                        .Select(mov => new FE_MovPolDTO
                        {
                            id = mov.sig.id,
                            year = mov.enk.year,
                            mes = mov.enk.mes,
                            tp = mov.enk.tp,
                            poliza = mov.enk.poliza,
                            linea = mov.enk.linea,
                            folio = getFolio(mov.enk),
                            cc = mov.enk.cc,
                            cta = mov.enk.cta,
                            scta = mov.enk.scta,
                            sscta = mov.enk.sscta,
                            ctaStr = getCuenta(mov.enk),
                            ctaDesc = getCuentaDescripcion(mov.enk),
                            fechapol = mov.enk.fechapol,
                            fecha = mov.enk.fechapol.ToString("dd/MM/yyyy"),
                            tm = mov.enk.tm,
                            itm = mov.sig.itm,
                            itmOri = mov.sig.itmOri,
                            itmDesc = getTipoMovimientoDescripcion(mov.enk),
                            monto = mov.enk.monto,
                            concepto = mov.enk.concepto,
                            orden_compra = mov.enk.orden_compra,
                            numpro = mov.enk.numpro,
                            proveedor = getProveedorDescripcion(mov.enk),
                            idConcepto = mov.sig.idConcepto,
                            idConceptoDir = mov.sig.idConceptoDir
                        }).OrderBy(o => o.folio).ToList();
                if(busq.tipo == tipoBusqueda.FaltanteGuardar)
                {
                    lst = lst.Where(w => w.id == 0).ToList();
                }
                var esSuccess = lst.Count > 0;
                if(esSuccess)
                {
                    result.Add("lst", lst);
                }
                result.Add(SUCCESS, esSuccess);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        public ActionResult guardarMovPol(List<tblC_FE_MovPol> lst)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var esSuccess = lst.Count > 0 && !lst.All(movpol =>
                        movpol.year == 0 &&
                        movpol.mes == 0 &&
                        movpol.tp.Length == 0 &&
                        movpol.poliza == 0 &&
                        movpol.linea == 0 &&
                        movpol.cta == 0);
                if(esSuccess)
                {
                    lst = asignarConceptoParaObraYOperativo(lst);
                    esSuccess = flujoFS.guardarMovPol(lst);
                }
                result.Add(SUCCESS, esSuccess);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        public ActionResult guardarCorte(BusqFlujoEfectivoDTO busq, bool esCorte)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var esSuccess = false;
                if(esCorte)
                {
                    esSuccess = flujoFS.guardarCorte(busq);
                }
                else
                {
                    esSuccess = flujoFS.cancelarCorte(busq);
                }
                result.Add(SUCCESS, esSuccess);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getPeriodoContable()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var periodo = mesFS.getProcesosAbiertos(SistemasEnkontrolEnum.Contabilidad);
                var esSuccess = periodo.Count > 0;
                if(!esSuccess)
                {
                    periodo = mesFS.getProcesosAbiertos(0);
                    esSuccess = periodo.Count > 0;
                }
                var max = periodo.Max(p => p.fecha);
                result.Add("minPeriodo", periodo.Min(p => p.fecha));
                result.Add("maxPeriodo", new DateTime(max.Year, max.Month, DateTime.DaysInMonth(max.Year, max.Month)));
                result.Add(SUCCESS, periodo.Count > 0);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCorte()
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (vSesiones.sesionEmpresaActual != 6)
                {

                    //var corte = flujoFS.getLstMovPolActiva();
                    var corte = flujoFS.getLstCortes();
                    var esSuccess = corte.Count() > 0;
                    if (esSuccess)
                    {
                        var maxCorte = corte.OrderByDescending(x => x.fecha_fin).FirstOrDefault().fecha_fin;
                        var minCorte = corte.OrderBy(x => x.fecha_fin).FirstOrDefault().fecha_inicio;
                        if (maxCorte.DayOfWeek != DayOfWeek.Saturday)
                        {
                            maxCorte = maxCorte.Siguiente(DayOfWeek.Saturday);
                        }
                        if (minCorte.DayOfWeek != DayOfWeek.Sunday)
                        {
                            minCorte = minCorte.AddDays(-7).Siguiente(DayOfWeek.Sunday);
                        }
                        result.Add("minCorte", minCorte);
                        result.Add("maxCorte", maxCorte);
                    }
                    result.Add(SUCCESS, esSuccess);
                }
                else 
                {
                    var corte = flujoFS.getCorteActaul();
                    var esSuccess = corte != null;
                    if (esSuccess)
                    {
                        var maxCorte = corte.fecha_fin;
                        var minCorte = corte.fecha_inicio;
                        if (maxCorte.DayOfWeek != DayOfWeek.Saturday)
                        {
                            maxCorte = maxCorte.Siguiente(DayOfWeek.Saturday);
                        }
                        if (minCorte.DayOfWeek != DayOfWeek.Sunday)
                        {
                            minCorte = minCorte.AddDays(-7).Siguiente(DayOfWeek.Sunday);
                        }
                        result.Add("minCorte", minCorte);
                        result.Add("maxCorte", maxCorte);
                    }
                    result.Add(SUCCESS, esSuccess);
                }
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCorteEstado(BusqFlujoEfectivoDTO busq)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var corte = flujoFS.getLstMovPolActiva(busq);
                result.Add("esCorte", corte.Count == 0 ? false : corte.FirstOrDefault().esFlujoEfectivo);
                result.Add(SUCCESS, true);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getGuardadoEstado(BusqFlujoEfectivoDTO busq)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var movpol = flujoFS.getLstMovPolActiva(busq);
                result.Add("esGuardado", movpol.Count == 0 ? false : movpol.FirstOrDefault().esFlujoEfectivo);
                result.Add(SUCCESS, true);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Movimiento Polizas Directo
        public ActionResult MovPolizaDirecto()
        {
            var empresa = vSesiones.sesionEmpresaActual;
            if(empresa != (int)EmpresaEnum.Arrendadora)
            {
                return View();
            }
            else
            {
                return flujoEfectivoCtrl.MovPolizaDirecto();
            }
        }
        #endregion
        #region Reporte
        public ActionResult Reporte()
        {
            var empresa = vSesiones.sesionEmpresaActual;
            if(empresa != (int)EmpresaEnum.Arrendadora)
            {
                return View();
            }
            else
            {
                return flujoEfectivoCtrl.Reporte();
            }
        }
        public ActionResult getLstAuxAnual(BusqFlujoEfectivoDTO busq)
        {
            var result = new Dictionary<string, object>();
            try
            {
                busq = ValidaBusquedaOperativo(busq);
                Session["busqFlujoEfectivo"] = busq;
                busq.lstTm = lstTm.Select(s => s.Value.ParseInt()).ToList();
                Session["rptFlujoEfectivo"] = new List<FlujoEfectivoOperativoDTO>();
                var lstMovPol = flujoFS.getLstMovPolFlujoEfectivoOperativo(busq);
                var esSuccess = lstMovPol.Count > 0;
                if(esSuccess)
                {
                    lstMovPol = matchFlujoEfectivoOperativoYEnkontrol(lstMovPol, busq);
                    var lst = generarFlujoEfectivoOperativo(lstMovPol, busq);
                    Session["lstMovPol"] = lstMovPol;
                    Session["rptFlujoEfectivo"] = lst;
                    result.Add("lst", lst);
                }
                result.Add(SUCCESS, esSuccess);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        public ActionResult getMovPolFlujo(BusqFlujoEfectivoDTO busq)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var busqSession = (BusqFlujoEfectivoDTO)Session["busqFlujoEfectivo"];
                var lstMovPol = (List<tblC_FE_MovPol>)Session["lstMovPol"];
                var lstIdCpto = getLstIdCptoDirDesdeIdCpto(busq.idConcepto);
                var esAnioInicial = busq.max.Year == 2000;
                var anio = busq.max.Year;
                var mes = busq.max.Month;
                var tieneSaldoInicial = lstIdCpto.Contains(0);
                var lstFiltroMovPol = from w in lstMovPol
                                      where lstIdCpto.Contains(w.idConcepto)
                                      select w;
                switch(busq.idConcepto)
                {
                    case -1:
                        if(esAnioInicial || mes == 1)
                        {
                            lstFiltroMovPol = from w in lstFiltroMovPol
                                              where w.idConcepto == 0
                                              select w;
                        }
                        else
                        {
                            lstFiltroMovPol = from w in lstFiltroMovPol
                                              where w.year == anio && w.mes < mes
                                              select w;
                        }
                        break;
                    case -2:
                        if(!esAnioInicial)
                        {
                            lstFiltroMovPol = from w in lstFiltroMovPol
                                              where w.year == anio && w.mes <= mes
                                              select w;
                        }
                        break;
                    default:
                        if(!esAnioInicial)
                        {
                            lstFiltroMovPol = from w in lstFiltroMovPol
                                              where w.year == anio && w.mes == mes
                                              select w;
                        }
                        break;
                }
                var esSuccess = lstFiltroMovPol.Any();
                if(esSuccess)
                {
                    var lst = from mov in lstFiltroMovPol
                              orderby mov.year, mov.mes, mov.tp, mov.poliza, mov.linea
                              select new
                              {
                                  id = mov.id,
                                  year = mov.year,
                                  mes = mov.mes,
                                  tp = mov.tp,
                                  poliza = mov.poliza,
                                  linea = mov.linea,
                                  folio = getFolio(mov),
                                  cc = mov.cc,
                                  cta = mov.cta,
                                  scta = mov.scta,
                                  sscta = mov.sscta,
                                  ctaStr = getCuenta(mov),
                                  ctaDesc = getCuentaDescripcion(mov),
                                  fechapol = mov.fechapol,
                                  fecha = mov.fechapol.ToString("dd/MM/yyyy"),
                                  tm = mov.tm,
                                  itm = mov.itm,
                                  itmOri = mov.itmOri,
                                  itmDesc = getTipoMovimientoDescripcion(mov),
                                  monto = mov.monto,
                                  concepto = mov.concepto,
                                  numpro = mov.numpro,
                                  proveedor = getProveedorDescripcion(mov),
                                  orden_compra = "",
                                  idConcepto = mov.idConcepto,
                                  idConceptoDir = mov.idConceptoDir
                              };
                    result.Add("lst", lst);
                }
                result.Add(SUCCESS, esSuccess);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        #endregion
        #region Directivos
        public ActionResult Directo()
        {
            var empresa = vSesiones.sesionEmpresaActual;
            if(empresa != (int)EmpresaEnum.Arrendadora)
            {
                return View();
            }
            else
            {
                return flujoEfectivoCtrl.Directo();
            }
        }
        public ActionResult guardarCcVisto(tblC_FED_CcVisto cc)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(SUCCESS, flujoFS.guardarCcVisto(cc));
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getLstCCvistos(int anio, int semana)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = flujoFS.getLstCCvistos(anio, semana);
                result.Add("lst", lst);
                result.Add(SUCCESS, lst.Count > 0);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult getFechaUltimoCorte()
        //{
        //    var result = new Dictionary<string, object>();
        //    try
        //    {
        //        var esAdmin = usuarioFS.getLstAccionesActual().Any(s => s.id == 3007);
        //        var lst = flujoFS.getPlaneacion(false).Where(w => w.flujoTotal != 0 && w.anio >= 2020 && w.idConceptoDir <= 20).ToList();
        //        var esSuccess = lst.Count() > 0;
        //        if(esSuccess)
        //        {
        //            var año = lst.Max(p => p.anio);
        //            var maxSemana = lst.Max(p => p.semana);
        //            var maxCorte = Infrastructure.Utils.DatetimeUtils.primerDiaSemana(año, maxSemana);
        //            if(maxCorte.DayOfWeek != DayOfWeek.Saturday)
        //            {
        //                maxCorte = maxCorte.Siguiente(DayOfWeek.Saturday);
        //            }
        //            result.Add("fechaUltimoCorte", maxCorte);
        //            result.Add("fechaPrimerCorte", lst.Min(w => w.fecha));
        //            result.Add("fechaPlaneacion", maxCorte.AddDays(1));
        //            result.Add("esAdmin", esAdmin);

        //        }
        //        result.Add(SUCCESS, esSuccess);
        //    }
        //    catch(Exception e)
        //    {
        //        result.Add(MESSAGE, e.Message);
        //        result.Add(SUCCESS, false);
        //    }
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult getFechaUltimoCorte()
        {
            var result = new Dictionary<string, object>();
            try
            {
                //var esAdmin = usuarioFS.getLstAccionesActual().Any(s => s.id == 3007);
                var esAdmin = true;
                result.Add("esAdmin", esAdmin);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> getlstFlujoEfectivoDirecto_Normal(BusqFlujoEfectivoDTO busq)
        {
            var result = new Dictionary<string, object>();
            try
            {
                busq = ValidaBusquedaDirecto(busq);
                var lstPlaneacion = flujoFS.getPlaneacion(busq.esConciliado).AsQueryable().Where(w => busq.lstCC.Contains("TODOS") ? w.cc == "TODOS" : busq.lstCC.Contains(w.cc));
                var tFlujoEfectivo = flujoFS.taskLstMovPolFlujoTotal(new BusqFlujoEfectivoDTO
                    {
                        lstCC = busq.lstCC,
                        lstAC = busq.lstAC,
                        idConcepto = busq.idConcepto,
                        esCC = busq.esCC,
                        esFlujo = busq.esFlujo,
                        esConciliado = busq.esConciliado,
                        lstTm = busq.lstTm,
                        max = busq.max,
                        min = busq.esFlujo ? new DateTime(2010, 1, 1) : new DateTime(busq.max.Year, 1, 1)
                    });
                var lstMovpol = await flujoFS.getLstMovPolFlujoEfectivoDirecto(busq);
                var lstDetPlan = await flujoFS.getLstDetPlaneacionDirecto(busq);
                if(busq.esConciliado)
                {
                    BusqFlujoEfectivoDTO busqArrendadora = new BusqFlujoEfectivoDTO();
                    busqArrendadora.esCC = false;
                    busqArrendadora.esConciliado = true;
                    busqArrendadora.esFlujo = false;
                    busqArrendadora.idConcepto = 0;
                    busqArrendadora.lstAC = busq.lstCC.Contains("TODOS") ? new List<string> { "TODOS" } : busq.lstAC;
                    busqArrendadora.lstCC = busq.lstCC;
                    busqArrendadora.lstTm = new List<int>();
                    busqArrendadora.max = busq.max;
                    busqArrendadora.min = busq.min;
                    busqArrendadora.tipo = busq.tipo;
                    lstPlaneacion = lstPlaneacion.Concat(flujoArrendadoraFS.getFlujoDirectoArrendadora(busqArrendadora));
                }
                var enc = generarEncabezado(busq);
                Session["busqFlujoEfectivo"] = busq;
                Session["enc"] = enc;
                result.Add("encabezado", enc);
                var lst = generarFlujoEfectivoDirecto(lstPlaneacion, busq);
                var lstGrafico = generarFlujoEfectivoDirectoGrafico(lstPlaneacion, busq);
                result.Add("lstGrafico", lstGrafico);
                result.Add("lst", lst);
                var lstCierre = new List<FlujoEfectivoDirectoDTO>();
                //--> What If Semana Anterior
                var lstCierreAnterior = new List<FlujoEfectivoDirectoDTO>();
                BusqFlujoEfectivoDTO busqAnterior = new BusqFlujoEfectivoDTO();
                busqAnterior.esCC = busq.esCC;
                busqAnterior.esConciliado = busq.esConciliado;
                busqAnterior.esFlujo = busq.esFlujo;
                busqAnterior.idConcepto = busq.idConcepto;
                busqAnterior.lstAC = busq.lstAC;
                busqAnterior.lstCC = busq.lstCC;
                busqAnterior.lstTm = busq.lstTm;
                busqAnterior.max = busq.max.AddDays(-7);
                busqAnterior.min = busq.min.AddDays(-7);
                busqAnterior.tipo = busq.tipo;
                var lstAnterior = generarFlujoEfectivoDirecto(lstPlaneacion, busqAnterior);
                result.Add("lstAnterior", lstAnterior);
                //<--
                var esPermisoFlujoCierre = (bool)Session["esPermisoFlujoCierre"];
                result.Add("esPermisoFlujoCierre", esPermisoFlujoCierre);
                if(esPermisoFlujoCierre)
                {
                    var lstCierreSession = await flujoFS.initLstDetProyeccionCierre(busq);
                    var lstCierreAnteriorSession = await flujoFS.initLstDetProyeccionCierre(busqAnterior);
                    var lstCierreArren =  flujoArrendadoraFS.getLstDetProyeccionCierreArrendadora(busq);
                    lstCierre = generarFlujoEfectivoCierre(lstPlaneacion, busq);
                    var sumFlujoEfectivo = lst.LastOrDefault().flujoTotalProyecto;
                    var sumCierre = lstCierre.Sum(s => s.flujoTotalProyecto);
                    //-->
                    var sumFlujoEfectivoAnterior = lstAnterior.LastOrDefault().flujoTotalProyecto;
                    var sumCierreAnterior = lstCierre.Sum(s => s.flujoTotalProyectoAnterior);
                    //<--                    
                    lstCierre.Insert(0, new FlujoEfectivoDirectoDTO()
                    {
                        idCpto = -1,
                        descripcion = "(+) SALDO INICIAL",
                        flujoTotalProyecto = sumFlujoEfectivo,
                        flujoTotalProyectoAnterior = sumFlujoEfectivoAnterior,
                        clase = (tipoPropuestaEnum.Suma).GetDescription(),
                    });
                    lstCierre.Add(new FlujoEfectivoDirectoDTO()
                    {
                        idCpto = -2,
                        descripcion = "(=) TOTAL",
                        flujoTotalProyecto = sumCierre + sumFlujoEfectivo,
                        flujoTotalProyectoAnterior = sumCierreAnterior + sumFlujoEfectivoAnterior,
                        clase = (tipoPropuestaEnum.Suma).GetDescription(),
                    });
                    var lstCierreGrafico = generarFlujoEfectivoCptosGrafico(lst.AsQueryable(), lstPlaneacion, busq);
                    result.Add("lstCierreGrafico", lstCierreGrafico);
                    result.Add("lstAC", busq.lstAC);
                    Session["lstCierre"] = lstCierreSession;
                    Session["lstCierreAnterior"] = lstCierreAnteriorSession;
                    //var lstCierreArrendadora = generarFlujoEfectivoCierreArrendadora(busq);
                    //result.Add("lstCierreArrendadora", lstCierreArrendadora);
                    //Session["lstCierreArrendadora"] = lstCierreArren;
                }
                result.Add("lstCierre", lstCierre);
                lst.ForEach(x => x.planeacionConsulta = (x.idCpto == -2 ? 0 : (x.idCpto == -3 ? 0 : x.planeacionConsulta)));
                Session["lstGenerarFlujoEfectivoDirecto"] = lst;
                Session["tblC_FE_MovPol"] = lstMovpol;
                Session["tblC_FED_PlaneacionDet"] = lstDetPlan;
                Session["lstFlujo"] = await tFlujoEfectivo;
                result.Add(SUCCESS, true);
            }
            catch (Exception o_O)
            {
                result.Add(MESSAGE, o_O.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> getlstFlujoEfectivoDirecto(BusqFlujoEfectivoDTO busq)
        {
            var result = new Dictionary<string, object>();
            //try
            //{
                init_Optimizado();
                busq = ValidaBusquedaDirecto(busq);
                var lstPlaneacion = flujoFS.getPlaneacionOptimizado(busq);
                //var tFlujoEfectivo = flujoFS.taskLstMovPolFlujoTotal(new BusqFlujoEfectivoDTO
                //{
                //    lstCC = busq.lstCC,
                //    lstAC = busq.lstAC,
                //    idConcepto = busq.idConcepto,
                //    esCC = busq.esCC,
                //    esFlujo = busq.esFlujo,
                //    esConciliado = busq.esConciliado,
                //    lstTm = busq.lstTm,
                //    max = busq.max,
                //    min = busq.esFlujo ? new DateTime(2010, 1, 1) : new DateTime(busq.max.Year, 1, 1)
                //});
                //var lstMovpol = await flujoFS.getLstMovPolFlujoEfectivoDirecto(busq);
                //var lstDetPlan = await flujoFS.getLstDetPlaneacionDirecto(busq);
                if (busq.esConciliado)
                {
                    BusqFlujoEfectivoDTO busqArrendadora = new BusqFlujoEfectivoDTO();
                    busqArrendadora.esCC = false;
                    busqArrendadora.esConciliado = true;
                    busqArrendadora.esFlujo = false;
                    busqArrendadora.idConcepto = 0;
                    busqArrendadora.lstAC = busq.lstCC.Contains("TODOS") ? new List<string> { "TODOS" } : busq.lstAC;
                    busqArrendadora.lstCC = busq.lstCC;
                    busqArrendadora.lstTm = new List<int>();
                    busqArrendadora.max = busq.max;
                    busqArrendadora.min = busq.min;
                    busqArrendadora.tipo = busq.tipo;
                    lstPlaneacion = lstPlaneacion.Concat(flujoArrendadoraFS.getFlujoDirectoArrendadora(busqArrendadora));
                }
                var enc = generarEncabezado(busq);
                Session["busqFlujoEfectivo"] = busq;
                Session["enc"] = enc;
                result.Add("encabezado", enc);
                var lst = generarFlujoEfectivoDirecto(lstPlaneacion, busq);
                var lstGrafico = generarFlujoEfectivoDirectoGrafico(lstPlaneacion, busq);
                result.Add("lstGrafico", lstGrafico);
                result.Add("lst", lst);
                var lstCierre = new List<FlujoEfectivoDirectoDTO>();
                //--> What If Semana Anterior
                var lstCierreAnterior = new List<FlujoEfectivoDirectoDTO>();
                BusqFlujoEfectivoDTO busqAnterior = new BusqFlujoEfectivoDTO();
                busqAnterior.esCC = busq.esCC;
                busqAnterior.esConciliado = busq.esConciliado;
                busqAnterior.esFlujo = busq.esFlujo;
                busqAnterior.idConcepto = busq.idConcepto;
                busqAnterior.lstAC = busq.lstAC;
                busqAnterior.lstCC = busq.lstCC;
                busqAnterior.lstTm = busq.lstTm;
                busqAnterior.max = busq.max.AddDays(-7);
                busqAnterior.min = busq.min.AddDays(-7);
                busqAnterior.tipo = busq.tipo;
                var lstAnterior = generarFlujoEfectivoDirecto(lstPlaneacion, busqAnterior);
                result.Add("lstAnterior", lstAnterior);
                //<--
                var esPermisoFlujoCierre = (bool)Session["esPermisoFlujoCierre"];
                result.Add("esPermisoFlujoCierre", esPermisoFlujoCierre);
                if (esPermisoFlujoCierre)
                {
                    //var lstCierreSession = await flujoFS.initLstDetProyeccionCierre(busq);
                    //var lstCierreAnteriorSession = await flujoFS.initLstDetProyeccionCierre(busqAnterior);
                    lstCierre = generarFlujoEfectivoCierre(lstPlaneacion, busq);
                    var sumFlujoEfectivo = lst.LastOrDefault().flujoTotalProyecto;
                    var sumCierre = lstCierre.Sum(s => s.flujoTotalProyecto);
                    //-->
                    var sumFlujoEfectivoAnterior = lstAnterior.LastOrDefault().flujoTotalProyecto;
                    var sumCierreAnterior = lstCierre.Sum(s => s.flujoTotalProyectoAnterior);
                    //<--                    
                    lstCierre.Insert(0, new FlujoEfectivoDirectoDTO()
                    {
                        idCpto = -1,
                        descripcion = "(+) SALDO INICIAL",
                        flujoTotalProyecto = sumFlujoEfectivo,
                        flujoTotalProyectoAnterior = sumFlujoEfectivoAnterior,
                        clase = (tipoPropuestaEnum.Suma).GetDescription(),
                    });
                    lstCierre.Add(new FlujoEfectivoDirectoDTO()
                    {
                        idCpto = -2,
                        descripcion = "(=) TOTAL",
                        flujoTotalProyecto = sumCierre + sumFlujoEfectivo,
                        flujoTotalProyectoAnterior = sumCierreAnterior + sumFlujoEfectivoAnterior,
                        clase = (tipoPropuestaEnum.Suma).GetDescription(),
                    });
                    var lstCierreGrafico = generarFlujoEfectivoCptosGrafico(lst.AsQueryable(), lstPlaneacion, busq);
                    result.Add("lstCierreGrafico", lstCierreGrafico);
                    result.Add("lstAC", busq.lstAC);
                    //Session["lstCierre"] = lstCierreSession;
                    //Session["lstCierreAnterior"] = lstCierreAnteriorSession;
                }
                result.Add("lstCierre", lstCierre);
                lst.ForEach(x => x.planeacionConsulta = (x.idCpto == -2 ? 0 : (x.idCpto == -3 ? 0 : x.planeacionConsulta)));
                Session["lstGenerarFlujoEfectivoDirecto"] = lst;
                //Session["tblC_FE_MovPol"] = lstMovpol;
                //Session["tblC_FED_PlaneacionDet"] = lstDetPlan;
                //Session["lstFlujo"] = await tFlujoEfectivo;

                result.Add(SUCCESS, true);
            //}
            //catch (Exception o_O)
            //{
            //    result.Add(MESSAGE, o_O.Message);
            //    result.Add(SUCCESS, false);
            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult geTblDirDetalleNormal(BusqFlujoEfectivoDetDTO busq)
        {
            var result = new Dictionary<string, object>();
            try
            {
                init();
                var busqSession = (BusqFlujoEfectivoDTO)Session["busqFlujoEfectivo"];
                var nivelDesc = busq.tipo.GetDescription();
                switch (busq.tipo)
                {
                    #region ConsultaCuenta
                    case tipoDetalleEnum.ConsultaCuenta:
                        var gpoMovpol = from g in getMovPolParaDetalles(busq).AsQueryable()
                                        orderby g.cta, g.scta, g.sscta
                                        group g by new { g.cta, g.scta, g.sscta } into s
                                        select new
                                        {
                                            idConceptoDir = busq.idConceptoDir,
                                            descripcion = getCuenta(s.Key.cta, s.Key.scta, s.Key.sscta, busq.empresa),
                                            cta = s.Key.cta,
                                            scta = s.Key.scta,
                                            sscta = s.Key.sscta,
                                            monto = s.Sum(ss => ss.monto),
                                            tipo = tipoDetalleEnum.ConsultaCentroCostos,
                                            empresa = busq.empresa,
                                            fechaPlaneacion = busq.fechaPlaneacion
                                        };
                        result.Add("total", gpoMovpol.Sum(s => s.monto));
                        result.Add("data", gpoMovpol);
                        result.Add("nivelDesc", busqSession.esConciliado ? busq.empresa.GetDescription() : lstConceptoDir.FirstOrDefault(cpto => cpto.id == busq.idConceptoDir).Concepto);
                        result.Add("title", "Pólizas");
                        break;
                    #endregion
                    #region ConsultaPoliza
                    case tipoDetalleEnum.ConsultaPoliza:
                        var lstMov = from s in getMovPolParaDetalles(busq).AsQueryable()
                                     orderby s.cta, s.scta, s.sscta
                                     select new
                                     {
                                         idConceptoDir = busq.idConceptoDir,
                                         concepto = s.concepto,
                                         descripcion = getCuenta(s.cta, s.scta, s.sscta, busq.empresa),
                                         cta = s.cta,
                                         scta = s.scta,
                                         sscta = s.sscta,
                                         monto = s.monto,
                                         folio = string.Format("{0}-{1}-{2}-{3}-{4}", s.year, s.mes, s.tp, s.poliza, s.linea),
                                         tipo = tipoDetalleEnum.ConsultaCuenta,
                                         empresa = s.empresa,
                                         fechaPlaneacion = busq.fechaPlaneacion
                                     };
                        result.Add("total", lstMov.Sum(s => s.monto));
                        result.Add("data", lstMov);
                        result.Add("nivelDesc", busq.concepto);
                        result.Add("title", "Pólizas");
                        break;
                    #endregion
                    #region ConsultaConcepto
                    case tipoDetalleEnum.ConsultaConcepto:
                        var gpoMovProv = from g in getMovPolParaDetalles(busq).AsQueryable()
                                         orderby g.concepto
                                         group g by new { g.cta, g.scta, g.sscta, g.concepto, cc = (g.empresa == EmpresaEnum.Construplan ? g.cc : g.ac) } into s
                                         select new
                                         {
                                             idConceptoDir = busq.idConceptoDir,
                                             concepto = s.Key.concepto,
                                             descripcion = getCuenta(s.Key.cta, s.Key.scta, s.Key.sscta, busq.empresa),
                                             cc = s.Key.cc,
                                             centroCostos = getObra(s.Key.cc, busq.empresa),
                                             cta = s.Key.cta,
                                             scta = s.Key.scta,
                                             sscta = s.Key.sscta,
                                             monto = s.Sum(ss => ss.monto),
                                             tipo = tipoDetalleEnum.ConsultaPoliza,
                                             empresa = busq.empresa,
                                             fechaPlaneacion = busq.fechaPlaneacion
                                         };
                        result.Add("total", gpoMovProv.Sum(s => s.monto));
                        result.Add("data", gpoMovProv);
                        result.Add("nivelDesc", busq.cc);
                        result.Add("title", "Pólizas");
                        break;
                    #endregion
                    #region ConsultaCentroCostos
                    case tipoDetalleEnum.ConsultaCentroCostos:
                        var gpoMovCCl = from g in getMovPolParaDetalles(busq).AsQueryable()
                                        orderby g.empresa == EmpresaEnum.Construplan ? g.cc : g.ac
                                        group g by new { g.cta, g.scta, g.sscta, cc = (g.empresa == EmpresaEnum.Construplan ? g.cc : g.ac) } into s
                                        select new
                                        {
                                            idConceptoDir = busq.idConceptoDir,
                                            descripcion = getCuenta(s.Key.cta, s.Key.scta, s.Key.sscta, busq.empresa),
                                            cc = s.Key.cc,
                                            centroCostos = getObra(s.Key.cc, busq.empresa),
                                            cta = s.Key.cta,
                                            scta = s.Key.scta,
                                            sscta = s.Key.sscta,
                                            monto = s.Sum(ss => ss.monto),
                                            tipo = tipoDetalleEnum.ConsultaConcepto,
                                            empresa = busq.empresa,
                                            fechaPlaneacion = busq.fechaPlaneacion
                                        };
                        result.Add("total", gpoMovCCl.Sum(s => s.monto));
                        result.Add("data", gpoMovCCl);
                        result.Add("nivelDesc", string.Format("{0}-{1}-{2}", busq.cta, busq.scta, busq.sscta));
                        result.Add("title", "Pólizas");
                        break;
                    #endregion
                    #region FlujoTotalCuenta
                    case tipoDetalleEnum.FlujoTotalCuenta:
                        var gpoFlujoCta = from g in getMovFlujoParaDetalles(busq).AsQueryable()
                                          orderby g.cta, g.scta, g.sscta
                                          group g by new { g.cta, g.scta, g.sscta } into s
                                          select new
                                          {
                                              idConceptoDir = busq.idConceptoDir,
                                              descripcion = getCuenta(s.Key.cta, s.Key.scta, s.Key.sscta, busq.empresa),
                                              cta = s.Key.cta,
                                              scta = s.Key.scta,
                                              sscta = s.Key.sscta,
                                              monto = s.Sum(ss => ss.monto),
                                              tipo = tipoDetalleEnum.FlujoTotalCentroCostos,
                                              empresa = busq.empresa,
                                              fechaPlaneacion = busq.fechaPlaneacion
                                          };
                        result.Add("total", gpoFlujoCta.Sum(s => s.monto));
                        result.Add("data", gpoFlujoCta);
                        result.Add("nivelDesc", busqSession.esConciliado ? busq.empresa.GetDescription() : lstConceptoDir.FirstOrDefault(cpto => cpto.id == busq.idConceptoDir).Concepto);
                        result.Add("title", "Flujo Total");
                        break;
                    #endregion
                    #region FlujoTotalConcepto
                    case tipoDetalleEnum.FlujoTotalConcepto:
                        var gpoFlujoProv = from g in getMovFlujoParaDetalles(busq).AsQueryable()
                                           orderby g.concepto, g.cta, g.scta, g.sscta
                                           group g by new { g.cta, g.scta, g.sscta, g.concepto, cc = (g.empresa == EmpresaEnum.Construplan ? g.cc : string.Format("{0}-{1}", g.area, g.cuenta_oc)) } into s
                                           select new
                                           {
                                               idConceptoDir = busq.idConceptoDir,
                                               concepto = s.Key.concepto,
                                               descripcion = getCuenta(s.Key.cta, s.Key.scta, s.Key.sscta, busq.empresa),
                                               cta = s.Key.cta,
                                               scta = s.Key.scta,
                                               sscta = s.Key.sscta,
                                               cc = s.Key.cc,
                                               centroCostos = getObra(s.Key.cc, busq.empresa),
                                               monto = s.Sum(ss => ss.monto),
                                               tipo = tipoDetalleEnum.FlujoTotalCuenta,
                                               empresa = busq.empresa,
                                               fechaPlaneacion = busq.fechaPlaneacion
                                           };
                        result.Add("total", gpoFlujoProv.Sum(s => s.monto));
                        result.Add("data", gpoFlujoProv);
                        result.Add("nivelDesc", busq.cc);
                        result.Add("title", "Flujo Total");
                        break;
                    #endregion
                    #region FlujoTotalCentroCostos
                    case tipoDetalleEnum.FlujoTotalCentroCostos:
                        var gpoFlujoCC = from g in getMovFlujoParaDetalles(busq).AsQueryable()
                                         orderby g.cta, g.scta, g.sscta, g.cc
                                         group g by new { g.cta, g.scta, g.sscta, cc = (g.empresa == EmpresaEnum.Construplan ? g.cc : string.Format("{0}-{1}", g.area, g.cuenta_oc)) } into s
                                         select new
                                         {
                                             idConceptoDir = busq.idConceptoDir,
                                             descripcion = getCuenta(s.Key.cta, s.Key.scta, s.Key.sscta, busq.empresa),
                                             cta = s.Key.cta,
                                             scta = s.Key.scta,
                                             sscta = s.Key.sscta,
                                             cc = s.Key.cc,
                                             centroCostos = getObra(s.Key.cc, busq.empresa),
                                             monto = s.Sum(ss => ss.monto),
                                             tipo = tipoDetalleEnum.FlujoTotalConcepto,
                                             empresa = busq.empresa,
                                             fechaPlaneacion = busq.fechaPlaneacion
                                         };
                        result.Add("total", gpoFlujoCC.Sum(s => s.monto));
                        result.Add("data", gpoFlujoCC);
                        result.Add("nivelDesc", string.Format("{0}-{1}-{2}", busq.cta, busq.scta, busq.sscta));
                        result.Add("title", "Flujo Total");
                        break;
                    #endregion
                    #region CierrePrincipal
                    case tipoDetalleEnum.CierrePrincipal:
                        var gpoCierre = from g in getProyeccionCierre(busq).AsQueryable()
                                        group g by g.tipo into s
                                        select new
                                        {
                                            descripcion = s.Key.GetDescription(),
                                            monto = s.Sum(ss => ss.monto),
                                            idConceptoDir = busq.idConceptoDir,
                                            tipo = s.Key == tipoProyeccionCierreEnum.Manual ? tipoDetalleEnum.CierreManual : tipoDetalleEnum.CierreConcepto,
                                            tipoCierre = s.Key,
                                            empresa = busq.empresa,
                                            fechaPlaneacion = busq.fechaPlaneacion
                                        };
                        result.Add("total", gpoCierre.Sum(s => s.monto));
                        result.Add("data", gpoCierre);
                        result.Add("nivelDesc", busqSession.esConciliado ? busq.empresa.GetDescription() : lstConceptoDir.FirstOrDefault(cpto => cpto.id == busq.idConceptoDir).Concepto);
                        result.Add("title", "What if?");
                        break;
                    #endregion
                    #region CierreManual
                    case tipoDetalleEnum.CierreManual:
                        var gpoCierreTipo = from s in getProyeccionCierre(busq).AsQueryable()
                                            where s.tipo == busq.tipoCierre
                                            select new
                                            {
                                                descripcion = s.descripcion,
                                                concepto = s.descripcion,
                                                cc = getObra(busq.empresa == EmpresaEnum.Construplan ? s.cc : s.ac, busq.empresa),
                                                monto = s.monto,
                                                idConceptoDir = busq.idConceptoDir,
                                                tipo = tipoDetalleEnum.CierreConcepto,
                                                empresa = busq.empresa,
                                                fechaPlaneacion = busq.fechaPlaneacion
                                            };
                        result.Add("total", gpoCierreTipo.Sum(s => s.monto));
                        result.Add("data", gpoCierreTipo);
                        result.Add("nivelDesc", busq.tipoCierre.GetDescription());
                        result.Add("title", "What if?");
                        break;
                    #endregion
                    #region CierreReserva
                    case tipoDetalleEnum.CierreReserva:
                        var lstCierreReserva = getProyeccionCierre(busq);
                        var lstCargo = lstCierreReserva.Where(w => w.naturaleza == naturalezaEnum.Egreso).ToList();
                        var lstAbono = lstCierreReserva.Where(w => w.naturaleza == naturalezaEnum.Ingreso).ToList();
                        var lstT = getLstReservaT(lstCierreReserva);
                        result.Add("total", lstCargo.Sum(s => s.monto) + lstAbono.Sum(s => s.monto));
                        result.Add("data", lstT);
                        result.Add("nivelDesc", busq.concepto);
                        result.Add("title", "What if?");
                        break;
                    #endregion
                    #region CierreConcepto
                    case tipoDetalleEnum.CierreConcepto:
                        {
               
                            var gpoCierreConcepto = from g in getProyeccionCierre(busq).AsQueryable()
                                                    where g.tipo == busq.tipoCierre
                                                    orderby g.descripcion
                                                    group g by g.descripcion into s
                                                    select new
                                                    {
                                                        descripcion = s.Key,
                                                        concepto = s.Key,
                                                        monto = s.Sum(ss => ss.monto),
                                                        idConceptoDir = busq.idConceptoDir,
                                                        tipo = tipoDetalleEnum.CierreFactura,
                                                        tipoCierre = busq.tipoCierre,
                                                        empresa = busq.empresa,
                                                        fechaPlaneacion = busq.fechaPlaneacion
                                                    };
                            result.Add("total", gpoCierreConcepto.Sum(s => s.monto));
                            result.Add("data", gpoCierreConcepto);
                            result.Add("nivelDesc", busq.tipoCierre.GetDescription());
                            result.Add("title", "What if?");
                        }
                        break;
                    #endregion
                    #region CierreFactura
                    case tipoDetalleEnum.CierreFactura:
                        var gpoCierreFactura = from s in getProyeccionCierre(busq).AsQueryable()
                                               where s.tipo == busq.tipoCierre
                                               orderby s.descripcion
                                               select new
                                               {
                                                   descripcion = s.descripcion,
                                                   concepto = s.descripcion,
                                                   cc = getObra(s.empresa == EmpresaEnum.Construplan ? s.cc : s.ac, s.empresa),
                                                   factura = s.factura,
                                                   fecha = s.fechaFactura,
                                                   monto = s.monto,
                                                   idConceptoDir = busq.idConceptoDir,
                                                   tipo = tipoDetalleEnum.CierreFactura,
                                                   tipoCierre = busq.tipoCierre,
                                                   empresa = busq.empresa,
                                                   fechaPlaneacion = busq.fechaPlaneacion
                                               };
                        result.Add("total", gpoCierreFactura.Sum(s => s.monto));
                        result.Add("data", gpoCierreFactura);
                        result.Add("nivelDesc", busq.concepto);
                        result.Add("title", "What if?");
                        break;
                    #endregion
                    #region CierrePrincipalArrendadora
                    case tipoDetalleEnum.CierrePrincipalArrendadora:
                        var gpoCierreArren = from g in getProyeccionCierreArrendadora(busq).AsQueryable()
                                             group g by g.tipo into s
                                             select new
                                             {
                                                 descripcion = s.Key.GetDescription(),
                                                 monto = s.Sum(ss => ss.monto),
                                                 idConceptoDir = busq.idConceptoDir,
                                                 tipo = s.Key == tipoProyeccionCierreEnum.Manual ? tipoDetalleEnum.CierreManualArrendadora : tipoDetalleEnum.CierreConceptoArrendadora,
                                                 tipoCierre = s.Key,
                                                 fechaPlaneacion = busq.fechaPlaneacion
                                             };
                        result.Add("total", gpoCierreArren.Sum(s => s.monto));
                        result.Add("data", gpoCierreArren);
                        result.Add("nivelDesc", lstConceptoDir.FirstOrDefault(cpto => cpto.id == busq.idConceptoDir).Concepto);
                        result.Add("title", "What if Arrendadora?");
                        break;
                    #endregion
                    #region CierreManualArrendadora
                    case tipoDetalleEnum.CierreManualArrendadora:
                        var lstAreaCuenta = flujoArrendadoraFS.getComboAreaCuenta();
                        lstAreaCuenta.Add(new ComboDTO { Value = "0-0", Text = "0-0 Sin asignar" });
                        var gpoCierreTipoArren = from s in getProyeccionCierreArrendadora(busq)
                                                 where s.tipo == busq.tipoCierre
                                                 select new
                                                 {
                                                     descripcion = s.descripcion,
                                                     concepto = s.descripcion,
                                                     cc = lstAreaCuenta.FirstOrDefault(ac => ac.Value == s.ac).Text,
                                                     monto = s.monto,
                                                     idConceptoDir = busq.idConceptoDir,
                                                     tipo = tipoDetalleEnum.CierreConceptoArrendadora,
                                                     fechaPlaneacion = busq.fechaPlaneacion
                                                 };
                        result.Add("total", gpoCierreTipoArren.Sum(s => s.monto));
                        result.Add("data", gpoCierreTipoArren);
                        result.Add("nivelDesc", busq.tipoCierre.GetDescription());
                        result.Add("title", "What if Arrendadora?");
                        break;
                    #endregion
                    #region CierreReservaArrendadora
                    case tipoDetalleEnum.CierreReservaArrendadora:
                        var lstCierreReservaArren = getProyeccionCierreArrendadora(busq);
                        var lstCargoArren = lstCierreReservaArren.Where(w => w.naturaleza == naturalezaEnum.Egreso).ToList();
                        var lstAbonoArren = lstCierreReservaArren.Where(w => w.naturaleza == naturalezaEnum.Ingreso).ToList();
                        var lstTArren = getLstReservaT(lstCierreReservaArren);
                        result.Add("total", lstCargoArren.Sum(s => s.monto) + lstAbonoArren.Sum(s => s.monto));
                        result.Add("data", lstTArren);
                        result.Add("nivelDesc", busq.empresa);
                        result.Add("title", "What if Arrendadora?");
                        break;
                    #endregion
                    #region CierreConceptoArrendadora
                    case tipoDetalleEnum.CierreConceptoArrendadora:
                        var gpoCierreConceptoArren = from g in getProyeccionCierreArrendadora(busq)
                                                     where g.tipo == busq.tipoCierre
                                                     orderby g.descripcion
                                                     group g by g.descripcion into s
                                                     select new
                                                     {
                                                         descripcion = s.Key,
                                                         concepto = s.Key,
                                                         monto = s.Sum(ss => ss.monto),
                                                         idConceptoDir = busq.idConceptoDir,
                                                         tipo = tipoDetalleEnum.CierreFacturaArrendadora,
                                                         tipoCierre = busq.tipoCierre,
                                                         fechaPlaneacion = busq.fechaPlaneacion
                                                     };
                        result.Add("total", gpoCierreConceptoArren.Sum(s => s.monto));
                        result.Add("data", gpoCierreConceptoArren);
                        result.Add("nivelDesc", busq.tipoCierre.GetDescription());
                        result.Add("title", "What if Arrendadora?");
                        break;
                    #endregion
                    #region CierreFacturaArrendadora
                    case tipoDetalleEnum.CierreFacturaArrendadora:
                        var lstAreaCuenta2 = flujoArrendadoraFS.getComboAreaCuenta();
                        lstAreaCuenta2.Add(new ComboDTO { Value = "0-0", Text = "0-0 Sin asignar" });
                        var gpoCierreFacturaArren = from s in getProyeccionCierreArrendadora(busq)
                                                    where s.tipo == busq.tipoCierre
                                                    orderby s.descripcion
                                                    select new
                                                    {
                                                        descripcion = s.descripcion,
                                                        concepto = s.descripcion,
                                                        cc = lstAreaCuenta2.FirstOrDefault(ac => ac.Value == s.ac).Text,
                                                        factura = s.factura,
                                                        fecha = s.fechaFactura,
                                                        monto = s.monto,
                                                        idConceptoDir = busq.idConceptoDir,
                                                        tipo = tipoDetalleEnum.CierreFacturaArrendadora,
                                                        tipoCierre = busq.tipoCierre,
                                                        fechaPlaneacion = busq.fechaPlaneacion
                                                    };
                        result.Add("total", gpoCierreFacturaArren.Sum(s => s.monto));
                        result.Add("data", gpoCierreFacturaArren);
                        result.Add("nivelDesc", busq.concepto);
                        result.Add("title", "What if Arrendadora?");
                        break;
                    #endregion
                    #region PlanPrincipal
                    case tipoDetalleEnum.PlanPrincipal:
                        var swTipo = new Infrastructure.DTO.SwitchClass<tipoDetalleEnum>
                        {
                            { tipoDetallePlaneacionEnum.manual, () => tipoDetalleEnum.PlanManual },
                            { tipoDetallePlaneacionEnum.contratos, () => tipoDetalleEnum.PlanManual },
                            { tipoDetallePlaneacionEnum.efectivoRecibido, () => tipoDetalleEnum.PlanCliente },
                            { tipoDetallePlaneacionEnum.cadenaProductiva, () => tipoDetalleEnum.PlanProveedor },
                            { tipoDetallePlaneacionEnum.gastosProyecto, () => tipoDetalleEnum.PlanProveedor },
                            { tipoDetallePlaneacionEnum.nomina, () => tipoDetalleEnum.PlanManual },
                        };
                        var lstPlanPpal = from plan in getDetPlanParaDetalles(busq).AsQueryable()
                                          group plan by plan.tipo into gpo
                                          select new
                                          {
                                              descripcion = gpo.Key.GetDescription(),
                                              monto = gpo.Sum(s => s.monto),
                                              idConceptoDir = busq.idConceptoDir,
                                              tipo = swTipo.Execute(gpo.Key),
                                              tipoPlan = gpo.Key,
                                              fechaPlaneacion = busq.fechaPlaneacion,
                                              empresa = busq.empresa,
                                          };
                        result.Add("total", lstPlanPpal.Sum(s => s.monto));
                        result.Add("data", lstPlanPpal);
                        result.Add("nivelDesc", busqSession.esConciliado ? busq.empresa.GetDescription() : lstConceptoDir.FirstOrDefault(cpto => cpto.id == busq.idConceptoDir).Concepto);
                        result.Add("title", "Planeacion");
                        break;
                    #endregion
                    #region PlanManual
                    case tipoDetalleEnum.PlanManual:
                        {
                            var lstPlanManual = (from plan in getDetPlanParaDetalles(busq)
                                                 select new
                                                 {
                                                     concepto = plan.descripcion,
                                                     monto = plan.monto,
                                                     idConceptoDir = plan.concepto,
                                                     cc = getObra(plan.empresa == EmpresaEnum.Construplan ? plan.cc : plan.ac, plan.empresa),
                                                     tipo = tipoDetalleEnum.PlanPrincipal,
                                                     tipoPlan = plan.tipo,
                                                     fechaPlaneacion = busq.fechaPlaneacion,
                                                     empresa = busq.empresa,
                                                 }).ToList();
                            result.Add("total", lstPlanManual.Sum(s => s.monto));
                            result.Add("data", lstPlanManual);
                            result.Add("nivelDesc", busq.tipoPlan.GetDescription());
                            result.Add("title", "Planeacion");
                        }
                        break;
                    case tipoDetalleEnum.contratos:
                        {
                            var lstPlanManual = (from plan in getDetPlanParaDetalles(busq)
                                                 select new
                                                 {
                                                     concepto = plan.descripcion,
                                                     monto = plan.monto,
                                                     idConceptoDir = plan.concepto,
                                                     cc = getObra(plan.empresa == EmpresaEnum.Construplan ? plan.cc : plan.ac, plan.empresa),
                                                     tipo = tipoDetalleEnum.PlanPrincipal,
                                                     tipoPlan = plan.tipo,
                                                     fechaPlaneacion = busq.fechaPlaneacion,
                                                     empresa = busq.empresa,
                                                 }).ToList();
                            result.Add("total", lstPlanManual.Sum(s => s.monto));
                            result.Add("data", lstPlanManual);
                            result.Add("nivelDesc", busq.tipoPlan.GetDescription());
                            result.Add("title", "Planeacion");
                        }
                        break;
                    #endregion
                    #region PlanCliente
                    case tipoDetalleEnum.PlanCliente:
                        var lstPlanCliente = from plan in getDetPlanParaDetalles(busq)
                                             group plan by plan.numcte into grupo
                                             select new
                                             {
                                                 cliente = getClienteDecripcion(grupo.Key, busq.empresa),
                                                 numcte = grupo.Key,
                                                 monto = grupo.Sum(s => s.monto),
                                                 idConceptoDir = busq.idConceptoDir,
                                                 tipo = tipoDetalleEnum.PlanClienteFactura,
                                                 tipoPlan = busq.tipoPlan,
                                                 fechaPlaneacion = busq.fechaPlaneacion
                                             };
                        result.Add("total", lstPlanCliente.Sum(s => s.monto));
                        result.Add("data", lstPlanCliente);
                        result.Add("nivelDesc", busq.tipoPlan.GetDescription());
                        result.Add("title", "Planeacion");
                        break;
                    #endregion
                    #region PlanClienteFactura
                    case tipoDetalleEnum.PlanClienteFactura:
                        var lstPlanClienteFactura = (from plan in getDetPlanParaDetalles(busq)
                                                     select new
                                                     {
                                                         cliente = getClienteDecripcion(plan.numcte, plan.empresa),
                                                         numcte = plan.numcte,
                                                         factura = plan.factura,
                                                         fechaFactura = plan.fechaFactura,
                                                         cc = getObra(plan.empresa == EmpresaEnum.Construplan ? plan.cc : plan.ac, plan.empresa),
                                                         monto = plan.monto,
                                                         idConceptoDir = busq.idConceptoDir,
                                                         tipo = tipoDetalleEnum.PlanClienteFactura,
                                                         tipoPlan = busq.tipoPlan,
                                                         fechaPlaneacion = busq.fechaPlaneacion
                                                     }).ToList();
                        result.Add("total", lstPlanClienteFactura.Sum(s => s.monto));
                        result.Add("data", lstPlanClienteFactura);
                        result.Add("nivelDesc", busq.numcte);
                        result.Add("title", "Planeacion");
                        break;
                    #endregion
                    #region PlanProveedor
                    case tipoDetalleEnum.PlanProveedor:
                        {
                            if (lstProv == null)
                            {
                                lstProv = polizaFS.getProveedor();
                            }
                            var lstPlanProveedor = from plan in getDetPlanParaDetalles(busq)
                                                   group plan by plan.numprov into grupo
                                                   select new
                                                   {
                                                       proveedor = getProveedorDescripcion(grupo.Key, busq.empresa),
                                                       numpro = grupo.Key,
                                                       monto = grupo.Sum(s => s.monto),
                                                       idConceptoDir = busq.idConceptoDir,
                                                       tipo = tipoDetalleEnum.PlanProveedorFactura,
                                                       tipoPlan = busq.tipoPlan,
                                                       empresa = busq.empresa,
                                                       fechaPlaneacion = busq.fechaPlaneacion
                                                   };
                            result.Add("total", lstPlanProveedor.Sum(s => s.monto));
                            result.Add("data", lstPlanProveedor);
                            result.Add("nivelDesc", busq.tipoPlan.GetDescription());
                            result.Add("title", "Planeacion");
                        }
                        break;
                    #endregion
                    #region PlanProveedorFactura
                    case tipoDetalleEnum.PlanProveedorFactura:
                        {
                            if (lstProv == null)
                            {
                                lstProv = polizaFS.getProveedor();
                            }
                            var lstPlanProveedorFactura = from plan in getDetPlanParaDetalles(busq)
                                                          select new
                                                          {
                                                              proveedor = getProveedorDescripcion(plan.numprov, plan.empresa),
                                                              numprov = plan.numcte,
                                                              factura = plan.factura,
                                                              fechaFactura = plan.fechaFactura,
                                                              cc = getObra(plan.empresa == EmpresaEnum.Construplan ? plan.cc : plan.ac, plan.empresa),
                                                              monto = plan.monto,
                                                              idConceptoDir = busq.idConceptoDir,
                                                              tipo = tipoDetalleEnum.PlanPrincipal,
                                                              tipoPlan = plan.tipo,
                                                              empresa = busq.empresa,
                                                              fechaPlaneacion = busq.fechaPlaneacion
                                                          };
                            result.Add("total", lstPlanProveedorFactura.Sum(s => s.monto));
                            result.Add("data", lstPlanProveedorFactura);
                            result.Add("nivelDesc", busq.numpro);
                            result.Add("title", "Planeacion");
                        }
                        break;
                    #endregion
                    #region PlanEmpresa
                    case tipoDetalleEnum.PlanEmpresa:
                        var lstConsultaEmpresa = from g in getDetPlanParaDetalles(busq).AsQueryable()
                                                 group g by g.empresa into s
                                                 select new
                                                 {

                                                     empresa = s.Key,
                                                     idConceptoDir = busq.idConceptoDir,
                                                     descripcion = s.Key.GetDescription(),
                                                     monto = s.Sum(ss => ss.monto),
                                                     tipo = tipoDetalleEnum.PlanPrincipal,
                                                     fechaPlaneacion = busq.fechaPlaneacion
                                                 };
                        result.Add("total", lstConsultaEmpresa.Sum(s => s.monto));
                        result.Add("data", lstConsultaEmpresa);
                        result.Add("nivelDesc", lstConceptoDir.FirstOrDefault(cpto => cpto.id == busq.idConceptoDir).Concepto);
                        result.Add("title", "Planeacion");
                        break;
                    #endregion
                    #region ConsultaEmpresa
                    case tipoDetalleEnum.ConsultaEmpresa:
                        var movPolEmpresas = from mov in getMovPolParaDetalles(busq).AsQueryable()
                                             group mov by mov.empresa into gpo
                                             select new
                                             {
                                                 empresa = gpo.Key,
                                                 descripcion = gpo.Key.GetDescription(),
                                                 monto = gpo.Sum(s => s.monto),
                                                 idConceptoDir = busq.idConceptoDir,
                                                 tipo = tipoDetalleEnum.ConsultaCuenta,
                                                 fechaPlaneacion = busq.fechaPlaneacion
                                             };
                        result.Add("total", movPolEmpresas.Sum(s => s.monto));
                        result.Add("data", movPolEmpresas);
                        result.Add("nivelDesc", lstConceptoDir.FirstOrDefault(cpto => cpto.id == busq.idConceptoDir).Concepto);
                        result.Add("title", "Pólizas");
                        break;
                    #endregion
                    #region FlujoEmpresa
                    case tipoDetalleEnum.FlujoEmpresa:
                        var flujoEmpresas = from mov in getMovFlujoParaDetalles(busq).AsQueryable()
                                            group mov by mov.empresa into gpo
                                            select new
                                            {
                                                empresa = gpo.Key,
                                                descripcion = gpo.Key.GetDescription(),
                                                monto = gpo.Sum(s => s.monto),
                                                idConceptoDir = busq.idConceptoDir,
                                                tipo = tipoDetalleEnum.FlujoTotalCuenta,
                                                fechaPlaneacion = busq.fechaPlaneacion
                                            };
                        result.Add("total", flujoEmpresas.Sum(s => s.monto));
                        result.Add("data", flujoEmpresas);
                        result.Add("nivelDesc", lstConceptoDir.FirstOrDefault(cpto => cpto.id == busq.idConceptoDir).Concepto);
                        result.Add("title", "Flujo Total");
                        break;
                    #endregion
                    #region CierreEmpresa
                    case tipoDetalleEnum.CierreEmpresa:
                        var cierreEmpresas = from mov in getProyeccionCierre(busq).AsQueryable()
                                             group mov by mov.empresa into gpo
                                             select new
                                             {
                                                 empresa = gpo.Key,
                                                 descripcion = gpo.Key.GetDescription(),
                                                 monto = gpo.Sum(s => s.monto),
                                                 idConceptoDir = busq.idConceptoDir,
                                                 tipo = tipoDetalleEnum.CierrePrincipal,
                                                 fechaPlaneacion = busq.fechaPlaneacion
                                             };
                        result.Add("total", cierreEmpresas.Sum(s => s.monto));
                        result.Add("data", cierreEmpresas);
                        result.Add("nivelDesc", lstConceptoDir.FirstOrDefault(cpto => cpto.id == busq.idConceptoDir).Concepto);
                        result.Add("title", "What if?");
                        break;
                    #endregion
                    default:
                        break;
                }
                result.Add(SUCCESS, result.Count > 0);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult geTblDirDetalle(BusqFlujoEfectivoDetDTO busq)
        {
            var result = new Dictionary<string, object>();
            //try
            //{
                init_Optimizado();
                var busqSession = (BusqFlujoEfectivoDTO)Session["busqFlujoEfectivo"];
                var nivelDesc = busq.tipo.GetDescription();
                 switch(busq.tipo)
                {
                    #region ConsultaCuenta
                    case tipoDetalleEnum.ConsultaCuenta:
                        {
                            
                            var gpoMovpol = from g in getMovPolParaDetalles_Optimizado(busq).AsQueryable()
                                            orderby g.cta, g.scta, g.sscta
                                            group g by new { g.cta, g.scta, g.sscta } into s
                                            select new
                                            {
                                                idConceptoDir = busq.idConceptoDir,
                                                descripcion = getCuenta(s.Key.cta, s.Key.scta, s.Key.sscta, busq.empresa),
                                                cta = s.Key.cta,
                                                scta = s.Key.scta,
                                                sscta = s.Key.sscta,
                                                monto = s.Sum(ss => ss.monto),
                                                tipo = tipoDetalleEnum.ConsultaCentroCostos,
                                                empresa = busq.empresa,
                                                fechaPlaneacion = busq.fechaPlaneacion
                                            };
                            result.Add("total", gpoMovpol.Sum(s => s.monto));
                            result.Add("data", gpoMovpol);
                            result.Add("nivelDesc", busqSession.esConciliado ? busq.empresa.GetDescription() : lstConceptoDir.FirstOrDefault(cpto => cpto.id == busq.idConceptoDir).Concepto);
                            result.Add("title", "Pólizas");
                        }
                        break;
                    #endregion
                    #region ConsultaPoliza
                    case tipoDetalleEnum.ConsultaPoliza:
                        {
                            var lstMov = from s in getMovPolParaDetalles_Optimizado(busq).AsQueryable()
                                         orderby s.cta, s.scta, s.sscta
                                         select new
                                         {
                                             idConceptoDir = busq.idConceptoDir,
                                             concepto = s.concepto,
                                             descripcion = getCuenta(s.cta, s.scta, s.sscta, busq.empresa),
                                             cta = s.cta,
                                             scta = s.scta,
                                             sscta = s.sscta,
                                             monto = s.monto,
                                             folio = string.Format("{0}-{1}-{2}-{3}-{4}", s.year, s.mes, s.tp, s.poliza, s.linea),
                                             tipo = tipoDetalleEnum.ConsultaCuenta,
                                             empresa = s.empresa,
                                             fechaPlaneacion = busq.fechaPlaneacion
                                         };
                            result.Add("total", lstMov.Sum(s => s.monto));
                            result.Add("data", lstMov);
                            result.Add("nivelDesc", busq.concepto);
                            result.Add("title", "Pólizas");
                        }
                        break;
                    #endregion
                    #region ConsultaConcepto
                    case tipoDetalleEnum.ConsultaConcepto:
                        {
                            var gpoMovProv = from g in getMovPolParaDetalles_Optimizado(busq).AsQueryable()
                                             orderby g.concepto
                                             group g by new { g.cta, g.scta, g.sscta, g.concepto, cc = (g.empresa == EmpresaEnum.Construplan ? g.cc : g.ac) } into s
                                             select new
                                             {
                                                 idConceptoDir = busq.idConceptoDir,
                                                 concepto = s.Key.concepto,
                                                 descripcion = getCuenta(s.Key.cta, s.Key.scta, s.Key.sscta, busq.empresa),
                                                 cc = s.Key.cc,
                                                 centroCostos = getObra(s.Key.cc, busq.empresa),
                                                 cta = s.Key.cta,
                                                 scta = s.Key.scta,
                                                 sscta = s.Key.sscta,
                                                 monto = s.Sum(ss => ss.monto),
                                                 tipo = tipoDetalleEnum.ConsultaPoliza,
                                                 empresa = busq.empresa,
                                                 fechaPlaneacion = busq.fechaPlaneacion
                                             };
                            result.Add("total", gpoMovProv.Sum(s => s.monto));
                            result.Add("data", gpoMovProv);
                            result.Add("nivelDesc", busq.cc);
                            result.Add("title", "Pólizas");
                        }
                        break;
                    #endregion
                    #region ConsultaCentroCostos
                    case tipoDetalleEnum.ConsultaCentroCostos:
                        var gpoMovCCl = from g in getMovPolParaDetalles_Optimizado(busq).AsQueryable()
                                        orderby g.empresa == EmpresaEnum.Construplan ? g.cc : g.ac
                                        group g by new { g.cta, g.scta, g.sscta, cc = (g.empresa == EmpresaEnum.Construplan ? g.cc : g.ac) } into s
                                        select new
                                        {
                                            idConceptoDir = busq.idConceptoDir,
                                            descripcion = getCuenta(s.Key.cta, s.Key.scta, s.Key.sscta, busq.empresa),
                                            cc = s.Key.cc,
                                            centroCostos = getObra(s.Key.cc, busq.empresa),
                                            cta = s.Key.cta,
                                            scta = s.Key.scta,
                                            sscta = s.Key.sscta,
                                            monto = s.Sum(ss => ss.monto),
                                            tipo = tipoDetalleEnum.ConsultaConcepto,
                                            empresa = busq.empresa,
                                            fechaPlaneacion = busq.fechaPlaneacion
                                        };
                        result.Add("total", gpoMovCCl.Sum(s => s.monto));
                        result.Add("data", gpoMovCCl);
                        result.Add("nivelDesc", string.Format("{0}-{1}-{2}", busq.cta, busq.scta, busq.sscta));
                        result.Add("title", "Pólizas");
                        break;
                    #endregion
                    #region FlujoTotalCuenta
                    case tipoDetalleEnum.FlujoTotalCuenta:
                        var gpoFlujoCta = from g in getMovFlujoParaDetalles_Optimizado(busq).AsQueryable()
                                          orderby g.cta, g.scta, g.sscta
                                          group g by new { g.cta, g.scta, g.sscta } into s
                                          select new
                                          {
                                              idConceptoDir = busq.idConceptoDir,
                                              descripcion = getCuenta(s.Key.cta, s.Key.scta, s.Key.sscta, busq.empresa),
                                              cta = s.Key.cta,
                                              scta = s.Key.scta,
                                              sscta = s.Key.sscta,
                                              monto = s.Sum(ss => ss.monto),
                                              tipo = tipoDetalleEnum.FlujoTotalCentroCostos,
                                              empresa = busq.empresa,
                                              fechaPlaneacion = busq.fechaPlaneacion
                                          };
                        result.Add("total", gpoFlujoCta.Sum(s => s.monto));
                        result.Add("data", gpoFlujoCta);
                        result.Add("nivelDesc", busqSession.esConciliado ? busq.empresa.GetDescription() : lstConceptoDir.FirstOrDefault(cpto => cpto.id == busq.idConceptoDir).Concepto);
                        result.Add("title", "Flujo Total");
                        break;
                    #endregion
                    #region FlujoTotalConcepto
                    case tipoDetalleEnum.FlujoTotalConcepto:
                        var gpoFlujoProv = from g in getMovFlujoParaDetalles_Optimizado(busq).AsQueryable()
                                           orderby g.concepto, g.cta, g.scta, g.sscta
                                           group g by new { g.cta, g.scta, g.sscta, g.concepto, cc = (g.empresa == EmpresaEnum.Construplan ? g.cc : string.Format("{0}-{1}", g.area, g.cuenta_oc)) } into s
                                           select new
                                           {
                                               idConceptoDir = busq.idConceptoDir,
                                               concepto = s.Key.concepto,
                                               descripcion = getCuenta(s.Key.cta, s.Key.scta, s.Key.sscta, busq.empresa),
                                               cta = s.Key.cta,
                                               scta = s.Key.scta,
                                               sscta = s.Key.sscta,
                                               cc = s.Key.cc,
                                               centroCostos = getObra(s.Key.cc, busq.empresa),
                                               monto = s.Sum(ss => ss.monto),
                                               tipo = tipoDetalleEnum.FlujoTotalCuenta,
                                               empresa = busq.empresa,
                                               fechaPlaneacion = busq.fechaPlaneacion
                                           };
                        result.Add("total", gpoFlujoProv.Sum(s => s.monto));
                        result.Add("data", gpoFlujoProv);
                        result.Add("nivelDesc", busq.cc);
                        result.Add("title", "Flujo Total");
                        break;
                    #endregion
                    #region FlujoTotalCentroCostos
                    case tipoDetalleEnum.FlujoTotalCentroCostos:
                        var gpoFlujoCC = from g in getMovFlujoParaDetalles_Optimizado(busq).AsQueryable()
                                         orderby g.cta, g.scta, g.sscta, g.cc
                                         group g by new { g.cta, g.scta, g.sscta, cc = (g.empresa == EmpresaEnum.Construplan ? g.cc : string.Format("{0}-{1}", g.area, g.cuenta_oc)) } into s
                                         select new
                                         {
                                             idConceptoDir = busq.idConceptoDir,
                                             descripcion = getCuenta(s.Key.cta, s.Key.scta, s.Key.sscta, busq.empresa),
                                             cta = s.Key.cta,
                                             scta = s.Key.scta,
                                             sscta = s.Key.sscta,
                                             cc = s.Key.cc,
                                             centroCostos = getObra(s.Key.cc, busq.empresa),
                                             monto = s.Sum(ss => ss.monto),
                                             tipo = tipoDetalleEnum.FlujoTotalConcepto,
                                             empresa = busq.empresa,
                                             fechaPlaneacion = busq.fechaPlaneacion
                                         };
                        result.Add("total", gpoFlujoCC.Sum(s => s.monto));
                        result.Add("data", gpoFlujoCC);
                        result.Add("nivelDesc", string.Format("{0}-{1}-{2}", busq.cta, busq.scta, busq.sscta));
                        result.Add("title", "Flujo Total");
                        break;
                    #endregion
                    #region CierrePrincipal
                    case tipoDetalleEnum.CierrePrincipal:
                        var gpoCierre = from g in getProyeccionCierre_Optimizado(busq).AsQueryable()
                                        group g by g.tipo into s
                                        select new
                                        {
                                            descripcion = s.Key.GetDescription(),
                                            monto = s.Sum(ss => ss.monto),
                                            idConceptoDir = busq.idConceptoDir,
                                            tipo = s.Key == tipoProyeccionCierreEnum.Manual ? tipoDetalleEnum.CierreManual : tipoDetalleEnum.CierreConcepto,
                                            tipoCierre = s.Key,
                                            empresa = busq.empresa,
                                            fechaPlaneacion = busq.fechaPlaneacion
                                        };
                        result.Add("total", gpoCierre.Sum(s => s.monto));
                        result.Add("data", gpoCierre);
                        result.Add("nivelDesc", busqSession.esConciliado ? busq.empresa.GetDescription() : lstConceptoDir.FirstOrDefault(cpto => cpto.id == busq.idConceptoDir).Concepto);
                        result.Add("title", "What if?");
                        break;
                    #endregion
                    #region CierreManual
                    case tipoDetalleEnum.CierreManual:
                        var gpoCierreTipo = from s in getProyeccionCierre_Optimizado(busq).AsQueryable()
                                            where s.tipo == busq.tipoCierre
                                            select new
                                            {
                                                descripcion = s.descripcion,
                                                concepto = s.descripcion,
                                                cc = getObra(busq.empresa == EmpresaEnum.Construplan ? s.cc : s.ac, busq.empresa),
                                                monto = s.monto,
                                                idConceptoDir = busq.idConceptoDir,
                                                tipo = tipoDetalleEnum.CierreConcepto,
                                                empresa = busq.empresa,
                                                fechaPlaneacion = busq.fechaPlaneacion
                                            };
                        result.Add("total", gpoCierreTipo.Sum(s => s.monto));
                        result.Add("data", gpoCierreTipo);
                        result.Add("nivelDesc", busq.tipoCierre.GetDescription());
                        result.Add("title", "What if?");
                        break;
                    #endregion
                    #region CierreReserva
                    case tipoDetalleEnum.CierreReserva:
                        var lstCierreReserva = getProyeccionCierre_Optimizado(busq);
                        var lstCargo = lstCierreReserva.Where(w => w.naturaleza == naturalezaEnum.Egreso).ToList();
                        var lstAbono = lstCierreReserva.Where(w => w.naturaleza == naturalezaEnum.Ingreso).ToList();
                        var lstT = getLstReservaT(lstCierreReserva);
                        result.Add("total", lstCargo.Sum(s => s.monto) + lstAbono.Sum(s => s.monto));
                        result.Add("data", lstT);
                        result.Add("nivelDesc", busq.concepto);
                        result.Add("title", "What if?");
                        break;
                    #endregion
                    #region CierreConcepto
                    case tipoDetalleEnum.CierreConcepto:
                        var gpoCierreConcepto = from g in getProyeccionCierre_Optimizado(busq).AsQueryable()
                                                where g.tipo == busq.tipoCierre
                                                orderby g.descripcion
                                                group g by g.descripcion into s
                                                select new
                                                {
                                                    descripcion = s.Key,
                                                    concepto = s.Key,
                                                    monto = s.Sum(ss => ss.monto),
                                                    idConceptoDir = busq.idConceptoDir,
                                                    tipo = tipoDetalleEnum.CierreFactura,
                                                    tipoCierre = busq.tipoCierre,
                                                    empresa = busq.empresa,
                                                    fechaPlaneacion = busq.fechaPlaneacion
                                                };
                        result.Add("total", gpoCierreConcepto.Sum(s => s.monto));
                        result.Add("data", gpoCierreConcepto);
                        result.Add("nivelDesc", busq.tipoCierre.GetDescription());
                        result.Add("title", "What if?");
                        break;
                    #endregion
                    #region CierreFactura
                    case tipoDetalleEnum.CierreFactura:
                        var gpoCierreFactura = from s in getProyeccionCierre_Optimizado(busq).AsQueryable()
                                               where s.tipo == busq.tipoCierre
                                               orderby s.descripcion
                                               select new
                                               {
                                                   descripcion = s.descripcion,
                                                   concepto = s.descripcion,
                                                   cc = getObra(s.empresa == EmpresaEnum.Construplan ? s.cc : s.ac, s.empresa),
                                                   factura = s.factura,
                                                   fecha = s.fechaFactura,
                                                   monto = s.monto,
                                                   idConceptoDir = busq.idConceptoDir,
                                                   tipo = tipoDetalleEnum.CierreFactura,
                                                   tipoCierre = busq.tipoCierre,
                                                   empresa = busq.empresa,
                                                   fechaPlaneacion = busq.fechaPlaneacion
                                               };
                        result.Add("total", gpoCierreFactura.Sum(s => s.monto));
                        result.Add("data", gpoCierreFactura);
                        result.Add("nivelDesc", busq.concepto);
                        result.Add("title", "What if?");
                        break;
                    #endregion
                    #region CierrePrincipalArrendadora
                    case tipoDetalleEnum.CierrePrincipalArrendadora:
                        var gpoCierreArren = from g in getProyeccionCierreArrendadora_Optimizado(busq).AsQueryable()
                                             group g by g.tipo into s
                                             select new
                                             {
                                                 descripcion = s.Key.GetDescription(),
                                                 monto = s.Sum(ss => ss.monto),
                                                 idConceptoDir = busq.idConceptoDir,
                                                 tipo = s.Key == tipoProyeccionCierreEnum.Manual ? tipoDetalleEnum.CierreManualArrendadora : tipoDetalleEnum.CierreConceptoArrendadora,
                                                 tipoCierre = s.Key,
                                                 fechaPlaneacion = busq.fechaPlaneacion
                                             };
                        result.Add("total", gpoCierreArren.Sum(s => s.monto));
                        result.Add("data", gpoCierreArren);
                        result.Add("nivelDesc", lstConceptoDir.FirstOrDefault(cpto => cpto.id == busq.idConceptoDir).Concepto);
                        result.Add("title", "What if Arrendadora?");
                        break;
                    #endregion
                    #region CierreManualArrendadora
                    case tipoDetalleEnum.CierreManualArrendadora:
                        var lstAreaCuenta = flujoArrendadoraFS.getComboAreaCuenta();
                        lstAreaCuenta.Add(new ComboDTO { Value = "0-0", Text = "0-0 Sin asignar" });
                        var gpoCierreTipoArren = from s in getProyeccionCierreArrendadora_Optimizado(busq)
                                                 where s.tipo == busq.tipoCierre
                                                 select new
                                                 {
                                                     descripcion = s.descripcion,
                                                     concepto = s.descripcion,
                                                     cc = lstAreaCuenta.FirstOrDefault(ac => ac.Value == s.ac).Text,
                                                     monto = s.monto,
                                                     idConceptoDir = busq.idConceptoDir,
                                                     tipo = tipoDetalleEnum.CierreConceptoArrendadora,
                                                     fechaPlaneacion = busq.fechaPlaneacion
                                                 };
                        result.Add("total", gpoCierreTipoArren.Sum(s => s.monto));
                        result.Add("data", gpoCierreTipoArren);
                        result.Add("nivelDesc", busq.tipoCierre.GetDescription());
                        result.Add("title", "What if Arrendadora?");
                        break;
                    #endregion
                    #region CierreReservaArrendadora
                    case tipoDetalleEnum.CierreReservaArrendadora:
                        var lstCierreReservaArren = getProyeccionCierreArrendadora_Optimizado(busq);
                        var lstCargoArren = lstCierreReservaArren.Where(w => w.naturaleza == naturalezaEnum.Egreso).ToList();
                        var lstAbonoArren = lstCierreReservaArren.Where(w => w.naturaleza == naturalezaEnum.Ingreso).ToList();
                        var lstTArren = getLstReservaT(lstCierreReservaArren);
                        result.Add("total", lstCargoArren.Sum(s => s.monto) + lstAbonoArren.Sum(s => s.monto));
                        result.Add("data", lstTArren);
                        result.Add("nivelDesc", busq.empresa);
                        result.Add("title", "What if Arrendadora?");
                        break;
                    #endregion
                    #region CierreConceptoArrendadora
                    case tipoDetalleEnum.CierreConceptoArrendadora:
                        var gpoCierreConceptoArren = from g in getProyeccionCierreArrendadora_Optimizado(busq)
                                                     where g.tipo == busq.tipoCierre
                                                     orderby g.descripcion
                                                     group g by g.descripcion into s
                                                     select new
                                                     {
                                                         descripcion = s.Key,
                                                         concepto = s.Key,
                                                         monto = s.Sum(ss => ss.monto),
                                                         idConceptoDir = busq.idConceptoDir,
                                                         tipo = tipoDetalleEnum.CierreFacturaArrendadora,
                                                         tipoCierre = busq.tipoCierre,
                                                         fechaPlaneacion = busq.fechaPlaneacion
                                                     };
                        result.Add("total", gpoCierreConceptoArren.Sum(s => s.monto));
                        result.Add("data", gpoCierreConceptoArren);
                        result.Add("nivelDesc", busq.tipoCierre.GetDescription());
                        result.Add("title", "What if Arrendadora?");
                        break;
                    #endregion
                    #region CierreFacturaArrendadora
                    case tipoDetalleEnum.CierreFacturaArrendadora:
                        var lstAreaCuenta2 = flujoArrendadoraFS.getComboAreaCuenta();
                        lstAreaCuenta2.Add(new ComboDTO { Value = "0-0", Text = "0-0 Sin asignar" });
                        var gpoCierreFacturaArren = from s in getProyeccionCierreArrendadora_Optimizado(busq)
                                                    where s.tipo == busq.tipoCierre
                                                    orderby s.descripcion
                                                    select new
                                                    {
                                                        descripcion = s.descripcion,
                                                        concepto = s.descripcion,
                                                        cc = lstAreaCuenta2.FirstOrDefault(ac => ac.Value == s.ac).Text,
                                                        factura = s.factura,
                                                        fecha = s.fechaFactura,
                                                        monto = s.monto,
                                                        idConceptoDir = busq.idConceptoDir,
                                                        tipo = tipoDetalleEnum.CierreFacturaArrendadora,
                                                        tipoCierre = busq.tipoCierre,
                                                        fechaPlaneacion = busq.fechaPlaneacion
                                                    };
                        result.Add("total", gpoCierreFacturaArren.Sum(s => s.monto));
                        result.Add("data", gpoCierreFacturaArren);
                        result.Add("nivelDesc", busq.concepto);
                        result.Add("title", "What if Arrendadora?");
                        break;
                    #endregion
                    #region PlanPrincipal
                    case tipoDetalleEnum.PlanPrincipal:
                        var swTipo = new Infrastructure.DTO.SwitchClass<tipoDetalleEnum>
                        {
                            { tipoDetallePlaneacionEnum.manual, () => tipoDetalleEnum.PlanManual },
                            { tipoDetallePlaneacionEnum.contratos, () => tipoDetalleEnum.PlanManual },
                            { tipoDetallePlaneacionEnum.efectivoRecibido, () => tipoDetalleEnum.PlanCliente },
                            { tipoDetallePlaneacionEnum.cadenaProductiva, () => tipoDetalleEnum.PlanProveedor },
                            { tipoDetallePlaneacionEnum.gastosProyecto, () => tipoDetalleEnum.PlanProveedor },
                            { tipoDetallePlaneacionEnum.nomina, () => tipoDetalleEnum.PlanManual },
                        };
                        var lstPlanPpal = from plan in getDetPlanParaDetalles_Optimizado(busq).AsQueryable()
                                          group plan by plan.tipo into gpo
                                          select new
                                          {
                                              descripcion = gpo.Key.GetDescription(),
                                              monto = gpo.Sum(s => s.monto),
                                              idConceptoDir = busq.idConceptoDir,
                                              tipo = swTipo.Execute(gpo.Key),
                                              tipoPlan = gpo.Key,
                                              fechaPlaneacion = busq.fechaPlaneacion,
                                              empresa = busq.empresa,
                                          };
                        result.Add("total", lstPlanPpal.Sum(s => s.monto));
                        result.Add("data", lstPlanPpal);
                        result.Add("nivelDesc", busqSession.esConciliado ? busq.empresa.GetDescription() : lstConceptoDir.FirstOrDefault(cpto => cpto.id == busq.idConceptoDir).Concepto);
                        result.Add("title", "Planeacion");
                        break;
                    #endregion
                    #region PlanManual
                    case tipoDetalleEnum.PlanManual:
                        {
                            var lstPlanManual = (from plan in getDetPlanParaDetalles_Optimizado(busq)
                                                 select new
                                                 {
                                                     concepto = plan.descripcion,
                                                     monto = plan.monto,
                                                     idConceptoDir = plan.concepto,
                                                     cc = getObra(plan.empresa == EmpresaEnum.Construplan ? plan.cc : plan.ac, plan.empresa),
                                                     tipo = tipoDetalleEnum.PlanPrincipal,
                                                     tipoPlan = plan.tipo,
                                                     fechaPlaneacion = busq.fechaPlaneacion,
                                                     empresa = busq.empresa,
                                                 }).ToList();
                            result.Add("total", lstPlanManual.Sum(s => s.monto));
                            result.Add("data", lstPlanManual);
                            result.Add("nivelDesc", busq.tipoPlan.GetDescription());
                            result.Add("title", "Planeacion");
                        }
                        break;
                    case tipoDetalleEnum.contratos:
                        {
                            var lstPlanManual = (from plan in getDetPlanParaDetalles_Optimizado(busq)
                                                 select new
                                                 {
                                                     concepto = plan.descripcion,
                                                     monto = plan.monto,
                                                     idConceptoDir = plan.concepto,
                                                     cc = getObra(plan.empresa == EmpresaEnum.Construplan ? plan.cc : plan.ac, plan.empresa),
                                                     tipo = tipoDetalleEnum.PlanPrincipal,
                                                     tipoPlan = plan.tipo,
                                                     fechaPlaneacion = busq.fechaPlaneacion,
                                                     empresa = busq.empresa,
                                                 }).ToList();
                            result.Add("total", lstPlanManual.Sum(s => s.monto));
                            result.Add("data", lstPlanManual);
                            result.Add("nivelDesc", busq.tipoPlan.GetDescription());
                            result.Add("title", "Planeacion");
                        }
                        break;
                    #endregion
                    #region PlanCliente
                    case tipoDetalleEnum.PlanCliente:
                        lstCliente = facturaFS.FillComboClienteNombre();
                        var lstPlanCliente = from plan in getDetPlanParaDetalles_Optimizado(busq)
                                             group plan by plan.numcte into grupo
                                             select new
                                             {
                                                 cliente = getClienteDecripcion(grupo.Key, busq.empresa),
                                                 numcte = grupo.Key,
                                                 monto = grupo.Sum(s => s.monto),
                                                 idConceptoDir = busq.idConceptoDir,
                                                 tipo = tipoDetalleEnum.PlanClienteFactura,
                                                 tipoPlan = busq.tipoPlan,
                                                 fechaPlaneacion = busq.fechaPlaneacion
                                             };
                        result.Add("total", lstPlanCliente.Sum(s => s.monto));
                        result.Add("data", lstPlanCliente);
                        result.Add("nivelDesc", busq.tipoPlan.GetDescription());
                        result.Add("title", "Planeacion");
                        break;
                    #endregion
                    #region PlanClienteFactura
                    case tipoDetalleEnum.PlanClienteFactura:
                        lstCliente = facturaFS.FillComboClienteNombre();
                        var lstPlanClienteFactura = (from plan in getDetPlanParaDetalles_Optimizado(busq)
                                                     select new
                                                     {
                                                         cliente = getClienteDecripcion(plan.numcte, plan.empresa),
                                                         numcte = plan.numcte,
                                                         factura = plan.factura,
                                                         fechaFactura = plan.fechaFactura,
                                                         cc = getObra(plan.empresa == EmpresaEnum.Construplan ? plan.cc : plan.ac, plan.empresa),
                                                         monto = plan.monto,
                                                         idConceptoDir = busq.idConceptoDir,
                                                         tipo = tipoDetalleEnum.PlanClienteFactura,
                                                         tipoPlan = busq.tipoPlan,
                                                         fechaPlaneacion = busq.fechaPlaneacion
                                                     }).ToList();
                        result.Add("total", lstPlanClienteFactura.Sum(s => s.monto));
                        result.Add("data", lstPlanClienteFactura);
                        result.Add("nivelDesc", busq.numcte);
                        result.Add("title", "Planeacion");
                        break;
                    #endregion
                    #region PlanProveedor
                    case tipoDetalleEnum.PlanProveedor:
                        {
                            if (lstProv == null)
                            {
                                lstProv = polizaFS.getProveedor();
                            }
                            var lstPlanProveedor = from plan in getDetPlanParaDetalles_Optimizado(busq)
                                                   group plan by new { plan.numprov, plan.descripcion } into grupo
                                                   select new
                                                   {
                                                       proveedor = grupo.Key.descripcion == null ? getProveedorDescripcion(grupo.Key.numprov, busq.empresa) : grupo.Key.descripcion,
                                                       numpro = grupo.Key.numprov,
                                                       monto = grupo.Sum(s => s.monto),
                                                       idConceptoDir = busq.idConceptoDir,
                                                       tipo = tipoDetalleEnum.PlanProveedorFactura,
                                                       tipoPlan = busq.tipoPlan,
                                                       empresa = busq.empresa,
                                                       fechaPlaneacion = busq.fechaPlaneacion
                                                   };
                            result.Add("total", lstPlanProveedor.Sum(s => s.monto));
                            result.Add("data", lstPlanProveedor);
                            result.Add("nivelDesc", busq.tipoPlan.GetDescription());
                            result.Add("title", "Planeacion");
                        }
                        break;
                    #endregion
                    #region PlanProveedorFactura
                    case tipoDetalleEnum.PlanProveedorFactura:
                        {
                            if (lstProv == null)
                            {
                                lstProv = polizaFS.getProveedor();
                            }
                            var lstPlanProveedorFactura = from plan in getDetPlanParaDetalles_Optimizado(busq)
                                                          select new
                                                          {
                                                              proveedor = plan.descripcion == null ? getProveedorDescripcion(plan.numprov, plan.empresa) : plan.descripcion,
                                                              numprov = plan.numcte,
                                                              factura = plan.factura,
                                                              fechaFactura = plan.fechaFactura,
                                                              cc = getObra(plan.empresa == EmpresaEnum.Construplan ? plan.cc : plan.ac, plan.empresa),
                                                              monto = plan.monto,
                                                              idConceptoDir = busq.idConceptoDir,
                                                              tipo = tipoDetalleEnum.PlanPrincipal,
                                                              tipoPlan = plan.tipo,
                                                              empresa = busq.empresa,
                                                              fechaPlaneacion = busq.fechaPlaneacion
                                                          };
                            result.Add("total", lstPlanProveedorFactura.Sum(s => s.monto));
                            result.Add("data", lstPlanProveedorFactura);
                            result.Add("nivelDesc", busq.numpro);
                            result.Add("title", "Planeacion");
                        }
                        break;
                    #endregion
                    #region PlanEmpresa
                    case tipoDetalleEnum.PlanEmpresa:
                        var lstConsultaEmpresa = from g in getDetPlanParaDetalles_Optimizado(busq).AsQueryable()
                                                 group g by g.empresa into s
                                                 select new
                                                 {

                                                     empresa = s.Key,
                                                     idConceptoDir = busq.idConceptoDir,
                                                     descripcion = s.Key.GetDescription(),
                                                     monto = s.Sum(ss => ss.monto),
                                                     tipo = tipoDetalleEnum.PlanPrincipal,
                                                     fechaPlaneacion = busq.fechaPlaneacion
                                                 };
                        result.Add("total", lstConsultaEmpresa.Sum(s => s.monto));
                        result.Add("data", lstConsultaEmpresa);
                        result.Add("nivelDesc", lstConceptoDir.FirstOrDefault(cpto => cpto.id == busq.idConceptoDir).Concepto);
                        result.Add("title", "Planeacion");
                        break;
                    #endregion
                    #region ConsultaEmpresa
                    case tipoDetalleEnum.ConsultaEmpresa:
                        var movPolEmpresas = from mov in getMovPolParaDetalles_Optimizado(busq).AsQueryable()
                                             group mov by mov.empresa into gpo
                                             select new
                                             {
                                                 empresa = gpo.Key,
                                                 descripcion = gpo.Key.GetDescription(),
                                                 monto = gpo.Sum(s => s.monto),
                                                 idConceptoDir = busq.idConceptoDir,
                                                 tipo = tipoDetalleEnum.ConsultaCuenta,
                                                 fechaPlaneacion = busq.fechaPlaneacion
                                             };
                        result.Add("total", movPolEmpresas.Sum(s => s.monto));
                        result.Add("data", movPolEmpresas);
                        result.Add("nivelDesc", lstConceptoDir.FirstOrDefault(cpto => cpto.id == busq.idConceptoDir).Concepto);
                        result.Add("title", "Pólizas");
                        break;
                    #endregion
                    #region FlujoEmpresa
                    case tipoDetalleEnum.FlujoEmpresa:
                        var flujoEmpresas = from mov in getMovFlujoParaDetalles_Optimizado(busq).AsQueryable()
                                            group mov by mov.empresa into gpo
                                            select new
                                            {
                                                empresa = gpo.Key,
                                                descripcion = gpo.Key.GetDescription(),
                                                monto = gpo.Sum(s => s.monto),
                                                idConceptoDir = busq.idConceptoDir,
                                                tipo = tipoDetalleEnum.FlujoTotalCuenta,
                                                fechaPlaneacion = busq.fechaPlaneacion
                                            };
                        result.Add("total", flujoEmpresas.Sum(s => s.monto));
                        result.Add("data", flujoEmpresas);
                        result.Add("nivelDesc", lstConceptoDir.FirstOrDefault(cpto => cpto.id == busq.idConceptoDir).Concepto);
                        result.Add("title", "Flujo Total");
                        break;
                    #endregion
                    #region CierreEmpresa
                    case tipoDetalleEnum.CierreEmpresa:
                        var cierreEmpresas = from mov in getProyeccionCierre_Optimizado(busq).AsQueryable()
                                             group mov by mov.empresa into gpo
                                             select new
                                             {
                                                 empresa = gpo.Key,
                                                 descripcion = gpo.Key.GetDescription(),
                                                 monto = gpo.Sum(s => s.monto),
                                                 idConceptoDir = busq.idConceptoDir,
                                                 tipo = tipoDetalleEnum.CierrePrincipal,
                                                 fechaPlaneacion = busq.fechaPlaneacion
                                             };
                        result.Add("total", cierreEmpresas.Sum(s => s.monto));
                        result.Add("data", cierreEmpresas);
                        result.Add("nivelDesc", lstConceptoDir.FirstOrDefault(cpto => cpto.id == busq.idConceptoDir).Concepto);
                        result.Add("title", "What if?");
                        break;
                    #endregion
                    default:
                        break;
                }
                result.Add(SUCCESS, result.Count > 0);
            //}
            //catch(Exception e)
            //{
            //    result.Add(MESSAGE, e.Message);
            //    result.Add(SUCCESS, false);
            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Planeacion
        public ActionResult GuardarPlaneacion(List<tblC_FED_CapPlaneacion> lst)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var esSuccess = lst.Count > 0 && lst.All(a => a.idConceptoDir > 0);
                if(esSuccess)
                {
                    lst.ForEach(plan =>
                    {
                        plan.anio = plan.fecha.Year;
                        plan.semana = plan.fecha.noSemana();
                    });
                    esSuccess = flujoFS.GuardarPlaneacion(lst);
                }
                result.Add(SUCCESS, esSuccess);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Resumen
        public ActionResult Resumen()
        {
            var empresa = vSesiones.sesionEmpresaActual;
            if(empresa != (int)EmpresaEnum.Arrendadora)
            {
                return View();
            }
            else
            {
                return flujoEfectivoCtrl.Resumen();
            }
        }
        #endregion
        #region export
        public MemoryStream exportFlujoEfectivo()
        {
            using(var package = new ExcelPackage())
            {
                init();
                var lstFlujoEfectivo = ((List<FlujoEfectivoOperativoDTO>)Session["rptFlujoEfectivo"]).ToArray();
                var busq = (BusqFlujoEfectivoDTO)Session["busqFlujoEfectivo"];
                #region Flujo Efectivo
                var titulo = string.Format("ESTADO DE FLUJO DE EFECTIVO");
                var flujoEfectivo = package.Workbook.Worksheets.Add(busq.max.Year.ToString());
                var ccTitulo = "";
                switch(busq.lstCC.FirstOrDefault())
                {
                    case "TODOS":
                        ccTitulo = "TODOS";
                        break;
                    default:
                        var cc = busq.lstCC.FirstOrDefault();
                        var ccDesc = lstCC.FirstOrDefault(c => c.cc == cc).descripcion;
                        ccTitulo = cc + "-" + ccDesc;
                        break;
                }
                var j = 6;
                var limite = lstFlujoEfectivo.Count() + j;
                foreach(var s in lstFlujoEfectivo)
                {
                    flujoEfectivo.Cells[string.Format("A{0}", j)].Value = s.descripcion ?? string.Empty;
                    if(s.clase != tipoPropuestaEnum.Encabezado.GetDescription())
                    {
                        flujoEfectivo.Cells[string.Format("B{0}", j)].Value = s.acum;
                        flujoEfectivo.Cells[string.Format("C{0}", j)].Value = s.acumPorcentaje / 100;
                        flujoEfectivo.Cells[string.Format("E{0}", j)].Value = s.mes;
                        flujoEfectivo.Cells[string.Format("F{0}", j)].Value = s.mesPorcentaje / 100;
                        flujoEfectivo.Cells[string.Format("G{0}", j)].Value = s.ene;
                        flujoEfectivo.Cells[string.Format("H{0}", j)].Value = s.feb;
                        flujoEfectivo.Cells[string.Format("I{0}", j)].Value = s.mar;
                        flujoEfectivo.Cells[string.Format("J{0}", j)].Value = s.abr;
                        flujoEfectivo.Cells[string.Format("K{0}", j)].Value = s.may;
                        flujoEfectivo.Cells[string.Format("L{0}", j)].Value = s.jun;
                        flujoEfectivo.Cells[string.Format("M{0}", j)].Value = s.jul;
                        flujoEfectivo.Cells[string.Format("N{0}", j)].Value = s.ago;
                        flujoEfectivo.Cells[string.Format("O{0}", j)].Value = s.sep;
                        flujoEfectivo.Cells[string.Format("P{0}", j)].Value = s.oct;
                        flujoEfectivo.Cells[string.Format("Q{0}", j)].Value = s.nov;
                        flujoEfectivo.Cells[string.Format("R{0}", j)].Value = s.dic;
                    }
                    j++;
                }
                flujoEfectivo.Cells["A2"].Value = titulo;
                flujoEfectivo.Cells["A3"].Value = string.Format("DEL {0:dd}° {0:MMMM} AL {1:dd}° DE {1:MMMM} DE {1:yyyy}", busq.max, busq.max).ToUpper();
                flujoEfectivo.Cells["A4"].Value = "CENTRO COSTOS: " + ccTitulo;
                flujoEfectivo.Cells["A5"].Value = "DESCRIPCIÓN";
                flujoEfectivo.Cells["B5"].Value = "ACUMULADO";
                flujoEfectivo.Cells["E5"].Value = "DEL MES";
                flujoEfectivo.Cells["G5"].Value = "ENERO";
                flujoEfectivo.Cells["H5"].Value = "FEBRERO";
                flujoEfectivo.Cells["I5"].Value = "MARZO";
                flujoEfectivo.Cells["J5"].Value = "ABRIL";
                flujoEfectivo.Cells["K5"].Value = "MAYO";
                flujoEfectivo.Cells["L5"].Value = "JUNIO";
                flujoEfectivo.Cells["M5"].Value = "JULIO";
                flujoEfectivo.Cells["N5"].Value = "AGOSTO";
                flujoEfectivo.Cells["O5"].Value = "SEPTIEMBRE";
                flujoEfectivo.Cells["P5"].Value = "OCTUBRE";
                flujoEfectivo.Cells["Q5"].Value = "NOVIEMBRE";
                flujoEfectivo.Cells["R5"].Value = "DICIEMBRE";
                using(var rng = flujoEfectivo.Cells["A5:S5"])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Justify;
                }
                using(var rng = flujoEfectivo.Cells["B5:C5"])
                {
                    rng.Merge = true;
                }
                using(var rng = flujoEfectivo.Cells["E5:F5"])
                {
                    rng.Merge = true;
                }
                j = 6;
                foreach(var s in lstFlujoEfectivo)
                {
                    var clase = (tipoPropuestaEnum)Enum.Parse(typeof(tipoPropuestaEnum), s.clase);
                    using(var rng = flujoEfectivo.Cells[string.Format("A{0}:R{0}", j)])
                    {
                        switch(clase)
                        {
                            case tipoPropuestaEnum.SinSaldo:
                                rng.Style.Font.Bold = true;
                                break;
                            case tipoPropuestaEnum.Saldo:
                                break;
                            case tipoPropuestaEnum.Suma:
                                rng.Style.Font.Bold = true;
                                break;
                            case tipoPropuestaEnum.Encabezado:
                                rng.Style.Font.Bold = true;
                                rng.Style.Border.Top.Style = ExcelBorderStyle.Medium;
                                break;
                            case tipoPropuestaEnum.SaldoEncabezado:
                                break;
                            case tipoPropuestaEnum.Input:
                                break;
                            case tipoPropuestaEnum.InputEncabezado:
                                break;
                            default:
                                break;
                        }
                    }
                    using(var rng = flujoEfectivo.Cells[string.Format("B{0}:R{0}", j)])
                    {
                        rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rng.Style.Numberformat.Format = "#,##0.00;[Red](#,##0.00)";
                        if(clase == tipoPropuestaEnum.SaldoEncabezado || clase == tipoPropuestaEnum.Suma)
                        {
                            rng.Style.Font.Bold = true;
                            rng.Style.WrapText = true;
                            rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rng.Style.Fill.BackgroundColor.SetColor(0, 170, 200, 233);
                        }
                        if(clase == tipoPropuestaEnum.SinSaldo)
                        {
                            rng.Clear();
                        }
                        if(rng.Value.Equals(decimal.Zero))
                        {
                            rng.Clear();
                        }
                        if(j == limite)
                        {
                            rng.Style.Border.Top.Style = ExcelBorderStyle.Medium;
                        }
                    }
                    using(var rng = flujoEfectivo.Cells[string.Format("D{0}", j)])
                    {
                        rng.Style.Numberformat.Format = "#,##0.00%;[Red](#,##0.00%)";
                    }
                    using(var rng = flujoEfectivo.Cells[string.Format("F{0}", j)])
                    {
                        rng.Style.Numberformat.Format = "#,##0.00%;[Red](#,##0.00%)";
                    }
                    j++;
                }
                flujoEfectivo.Cells[flujoEfectivo.Dimension.Address].AutoFitColumns();
                #endregion
                #region Registros
                var movpol = package.Workbook.Worksheets.Add("Polizas");
                var lstMovPol = (List<tblC_FE_MovPol>)Session["lstMovPol"];
                j = 1;
                foreach(var s in lstMovPol)
                {
                    var catCtaTemp1 = catCta.FirstOrDefault(c => c.cta == s.cta && c.scta == 0 && c.sscta == 0);
                    var catCtaTemp2 = catCta.FirstOrDefault(c => c.cta == s.cta && c.scta == s.scta && c.sscta == 0);//.descripcion;
                    var catCtaTemp3 = catCta.FirstOrDefault(c => c.cta == s.cta && c.scta == s.scta && c.sscta == s.sscta);
                    var ccDescripcion = lstCC.FirstOrDefault(c => c.cc == s.cc);
                    var listaConceptos = lstConcepto.FirstOrDefault(c => c.id == s.idConcepto);
                    var listaConceptos2 = lstConceptoDir.FirstOrDefault(c => c.id == s.idConceptoDir);

                    movpol.Cells[string.Format("A{0}", j)].Value = s.cta;
                    movpol.Cells[string.Format("B{0}", j)].Value = s.scta;
                    movpol.Cells[string.Format("C{0}", j)].Value = s.sscta;
                    movpol.Cells[string.Format("D{0}", j)].Value = s.year;
                    movpol.Cells[string.Format("E{0}", j)].Value = s.mes;
                    movpol.Cells[string.Format("F{0}", j)].Value = s.poliza;
                    movpol.Cells[string.Format("G{0}", j)].Value = s.tp;
                    movpol.Cells[string.Format("H{0}", j)].Value = s.linea;
                    movpol.Cells[string.Format("I{0}", j)].Value = s.tm;
                    movpol.Cells[string.Format("J{0}", j)].Value = s.cc;
                    movpol.Cells[string.Format("K{0}", j)].Value = s.monto;
                    movpol.Cells[string.Format("L{0}", j)].Value = s.itm;
                    movpol.Cells[string.Format("M{0}", j)].Value = s.itmOri;
                    movpol.Cells[string.Format("N{0}", j)].Value = s.idConcepto;
                    movpol.Cells[string.Format("O{0}", j)].Value = s.fechapol;
                    movpol.Cells[string.Format("P{0}", j)].Value = catCtaTemp1 != null ? catCtaTemp1.descripcion : "";
                    movpol.Cells[string.Format("Q{0}", j)].Value = catCtaTemp2 != null ? catCtaTemp2.descripcion : "";
                    movpol.Cells[string.Format("R{0}", j)].Value = catCtaTemp3 != null ? catCtaTemp3.descripcion : "";
                    movpol.Cells[string.Format("S{0}", j)].Value = ccDescripcion != null ? ccDescripcion.descripcion : "";
                    movpol.Cells[string.Format("T{0}", j)].Value = lstTm.FirstOrDefault(c => c.Value == s.itm.ToString()).Text;
                    movpol.Cells[string.Format("U{0}", j)].Value = s.itmOri == 0 ? " " : lstTm.FirstOrDefault(c => c.Value == s.itmOri.ToString()).Text;
                    movpol.Cells[string.Format("V{0}", j)].Value = listaConceptos != null ? listaConceptos.Concepto : "";
                    movpol.Cells[string.Format("W{0}", j)].Value = listaConceptos2 != null ? listaConceptos2.Concepto : "";

                    j++;
                }

                movpol.Cells["A1"].Value = "cta";
                movpol.Cells["B1"].Value = "scta";
                movpol.Cells["C1"].Value = "sscta";
                movpol.Cells["D1"].Value = "year";
                movpol.Cells["E1"].Value = "mes";
                movpol.Cells["F1"].Value = "poliza";
                movpol.Cells["G1"].Value = "tp";
                movpol.Cells["H1"].Value = "linea";
                movpol.Cells["I1"].Value = "tm";
                movpol.Cells["J1"].Value = "cc";
                movpol.Cells["K1"].Value = "monto";
                movpol.Cells["L1"].Value = "itm";
                movpol.Cells["M1"].Value = "itm original";
                movpol.Cells["N1"].Value = "concepto";
                movpol.Cells["O1"].Value = "fecha";
                movpol.Cells["P1"].Value = "cta";
                movpol.Cells["Q1"].Value = "scta";
                movpol.Cells["R1"].Value = "sscta";
                movpol.Cells["S1"].Value = "cc";
                movpol.Cells["T1"].Value = "itm";
                movpol.Cells["U1"].Value = "itm original";
                movpol.Cells["V1"].Value = "concepto directo";
                movpol.Cells["W1"].Value = "concepto proyectado";

                movpol.Cells[movpol.Dimension.Address].AutoFitColumns();
                #endregion
                package.Compression = CompressionLevel.BestSpeed;
                List<byte[]> lista = new List<byte[]>();
                using(var exportData = new MemoryStream())
                {
                    this.Response.Clear();
                    package.SaveAs(exportData);
                    lista.Add(exportData.ToArray());
                    this.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    this.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", string.Format("{0} {1}.xlsx", titulo, busq.max.Year)));
                    this.Response.BinaryWrite(exportData.ToArray());
                    this.Response.End();
                    return exportData;
                }
            }
        }
        #endregion
        #region Combobox
        public ActionResult getIniTblPoliza()
        {
            var result = new Dictionary<string, object>();
            try
            {
                if(lstConcepto == null)
                {
                    init();
                }
                result.Add("lstTmBancario", lstTm);
                result.Add("lstConcepto", lstConcepto);
                result.Add("lstRel", RelConcepto);
                result.Add("lstConceptoDir", lstConceptoDir.Select(c => new
                {
                    id = c.id,
                    idpadre = c.idPadre,
                    Concepto = c.Concepto,
                    esActivo = c.esActivo,
                    fechaRegistro = c.fechaRegistro
                }));
                result.Add("lstRelDir", RelConceptoDir.Select(r => new
                {
                    idConcepto = r.idConceptoDir,
                    tm = r.tm,
                }).ToList());
                result.Add(SUCCESS, lstConceptoDir.Count > 0);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCboPadreConcepto()
        {
            var result = new Dictionary<string, object>();
            try
            {
                if(lstConcepto == null)
                {
                    init();
                }
                var cbo = lstConcepto
                    .Where(c => c.idpadre == 0)
                    .Select(s => new ComboDTO()
                    {
                        Text = s.Concepto,
                        Value = s.id.ToString()
                    }).ToList();
                result.Add(ITEMS, cbo);
                result.Add(SUCCESS, cbo.Count > 0);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCboPadreConceptoDir()
        {
            var result = new Dictionary<string, object>();
            try
            {
                if(lstConceptoDir == null)
                {
                    init();
                }
                var cbo = lstConceptoDir
                    .Where(c => c.idPadre == 0)
                    .Select(s => new ComboDTO()
                    {
                        Text = s.Concepto,
                        Value = s.id.ToString()
                    }).ToList();
                result.Add(ITEMS, cbo);
                result.Add(SUCCESS, cbo.Count > 0);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCboNaturaleza()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cbo = EnumExtensions.ToCombo<naturalezaEnum>();
                result.Add(ITEMS, cbo);
                result.Add(SUCCESS, cbo.Count > 0);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCboTipoBusqueda()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cbo = EnumExtensions.ToCombo<tipoBusqueda>();
                result.Add(ITEMS, cbo);
                result.Add(SUCCESS, cbo.Count > 0);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCboOperadorConcepto()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cbo = EnumExtensions.ToArray<operadorCptoEnum>().Select(s => new ComboDTO()
                {
                    Value = s.GetDescription(),
                    Text = s.GetDescription()
                }).ToList();
                result.Add(ITEMS, cbo);
                result.Add(SUCCESS, cbo.Count > 0);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCboTipoMovimientoBancario()
        {
            var result = new Dictionary<string, object>();
            try
            {
                if(lstTm == null)
                {
                    init();
                }
                var esSuccess = lstTm.Count > 0;
                if(esSuccess)
                {
                    result.Add(ITEMS, lstTm);
                }
                result.Add(SUCCESS, esSuccess);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCboCCActivos()
        {
            var result = new Dictionary<string, object>();
            try
            {
                if(lstTm == null)
                {
                    init();
                }
                var esSuccess = lstTm.Count > 0;
                if(esSuccess)
                {
                    var lstCCActivas = lstCC.Where(w => w.st_ppto != "T").ToList();
                    result.Add(ITEMS, lstCCActivas.Select(c => new ComboDTO()
                    {
                        Text = c.cc + "-" + c.descripcion,
                        Value = c.cc,
                        Prefijo = c.inicioObra.ToString("MMMM / yyyy").ToUpper()
                    }).ToList());
                }
                result.Add(SUCCESS, esSuccess);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getCboCCActivosSigoplan()
        {
            var result = new Dictionary<string, object>();
            try
            {
                if(lstCC == null)
                {
                    init();
                }
                var cbo = lstCC.Select(s => new ComboDTO()
                {
                    Value = s.cc,
                    Text = s.cc + " - " + s.descripcion
                }).ToList();
                result.Add(ITEMS, cbo);
                result.Add(SUCCESS, true);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCboCConRevision()
        {
            var result = new Dictionary<string, object>();
            try
            {
                lstCC = cadenaFS.lstObra();
                var relObraUsuario = flujoFS.getRelObraUsuario();
                var esTODO = relObraUsuario.Any(s => s.tipo == tipoObraUsuarioEnum.Todos);
                var relGpoObra = relObraUsuario.Where(s => s.tipo == tipoObraUsuarioEnum.Grupo).Select(s => s.obra).ToList();
                var relIndObra = relObraUsuario.Where(s => s.tipo == tipoObraUsuarioEnum.Obra).Select(s => s.obra).ToList();
                var gpoCbo = new List<ComboGroupDTO>();
                var cbo = flujoFS.getCboCCActivosSigoplan();
                cbo = cbo.Where(w => esTODO ? true : relIndObra.Contains(w.Value) || lstCC.Where(c => c.cc == w.Value).Any(a => relGpoObra.Contains(a.bit_area))).ToList();
                cbo.Remove(cbo.FirstOrDefault(w => w.Value == "0"));
                cbo.Remove(cbo.FirstOrDefault(w => w.Value == "999"));
                var lstTipoCCCerrada = new List<int>() {
                    (int)TipoCCEnum.ObraCerradaGeneral,
                    (int)TipoCCEnum.ObraCerradaIndustrial
                };
                var cboIndividual = cbo.Where(w => lstCC.Where(c => c.cc == w.Value && w.Orden < 200).Any(c => !lstTipoCCCerrada.Contains(c.bit_area.ParseInt())));
                var cboIndividualSinOrden = cbo.Where(w => lstCC.Where(c => c.cc == w.Value && w.Orden >= 200).Any(c => !lstTipoCCCerrada.Contains(c.bit_area.ParseInt())));
                var cboCerrado = cbo.Where(w => lstCC.Where(c => c.cc == w.Value).Any(c => lstTipoCCCerrada.Contains(c.bit_area.ParseInt())));
                var cboAdministracion = cbo.Where(w => lstCC.Where(c => c.cc == w.Value && (esTODO ? true : relGpoObra.Contains(c.bit_area))).Any(c => c.bit_area.ParseInt() == (int)TipoCCEnum.Administración));
                var cboEdificio = cbo.Where(w => lstCC.Where(c => c.cc == w.Value && (esTODO ? true : relGpoObra.Contains(c.bit_area))).Any(c => c.bit_area.ParseInt() == (int)TipoCCEnum.InversionEdificio));
                var cboAlimentos = cbo.Where(w => lstCC.Where(c => c.cc == w.Value && (esTODO ? true : relGpoObra.Contains(c.bit_area))).Any(c => c.bit_area.ParseInt() == (int)TipoCCEnum.AlimentosYBebidas));
                var cboAutomotriz = cbo.Where(w => lstCC.Where(c => c.cc == w.Value && (esTODO ? true : relGpoObra.Contains(c.bit_area))).Any(c => c.bit_area.ParseInt() == (int)TipoCCEnum.Automotriz));
                var cboEnergia = cbo.Where(w => lstCC.Where(c => c.cc == w.Value && (esTODO ? true : relGpoObra.Contains(c.bit_area))).Any(c => c.bit_area.ParseInt() == (int)TipoCCEnum.Energía));
                var cboCerroPelon = cbo.Where(w => lstCC.Where(c => c.cc == w.Value && (esTODO ? true : relGpoObra.Contains(c.bit_area))).Any(c => c.bit_area.ParseInt() == (int)TipoCCEnum.CerroPelon));
                var cboMineria = cbo.Where(w => lstCC.Where(c => c.cc == w.Value && (esTODO ? true : relGpoObra.Contains(c.bit_area))).Any(c => c.bit_area.ParseInt() == (int)TipoCCEnum.Mineria));
                var cboColorada = cbo.Where(w => lstCC.Where(c => c.cc == w.Value && (esTODO ? true : relGpoObra.Contains(c.bit_area))).Any(c => c.bit_area.ParseInt() == (int)TipoCCEnum.Colorada));
                var cboNocheBuena = cbo.Where(w => lstCC.Where(c => c.cc == w.Value && (esTODO ? true : relGpoObra.Contains(c.bit_area))).Any(c => c.bit_area.ParseInt() == (int)TipoCCEnum.NocheBuena1y2));
                var cboSanAgustin = cbo.Where(w => lstCC.Where(c => c.cc == w.Value && (esTODO ? true : relGpoObra.Contains(c.bit_area))).Any(c => c.bit_area.ParseInt() == (int)TipoCCEnum.SanAgustin));
                var cboConstPesada = cbo.Where(w => lstCC.Where(c => c.cc == w.Value && (esTODO ? true : relGpoObra.Contains(c.bit_area))).Any(c => c.bit_area.ParseInt() == (int)TipoCCEnum.ConstruccionPesada));
                var cboGastosFinancieros = cbo.Where(w => lstCC.Where(c => c.cc == w.Value && (esTODO ? true : relGpoObra.Contains(c.bit_area))).Any(c => c.bit_area.ParseInt() == (int)TipoCCEnum.GastosFininacierosYOtros));
                if(cboMineria.Any())
                {
                    gpoCbo.Add(new ComboGroupDTO()
                    {
                        Text = "MINERÍA",
                        Value = "MIN",
                        Prefijo = "ENERO / 2011",
                        Selectable = true,
                        isGroup = true,
                        options = cboMineria.ToList()
                    });
                }
                if(cboColorada.Any())
                {
                    gpoCbo.Add(new ComboGroupDTO()
                    {
                        Text = "MINADO LA COLORADA",
                        Value = "COL",
                        Prefijo = "ENERO / 2011",
                        Selectable = true,
                        isGroup = true,
                        options = cboColorada.ToList()
                    });
                }

                if(cboSanAgustin.Any())
                {
                    gpoCbo.Add(new ComboGroupDTO()
                    {
                        Text = "MINADO SAN AGUSTÍN",
                        Value = "AGU",
                        Prefijo = "ENERO / 2011",
                        Selectable = true,
                        isGroup = true,
                        options = cboSanAgustin.ToList()
                    });
                }
                if(cboCerroPelon.Any())
                {
                    gpoCbo.Add(new ComboGroupDTO()
                    {
                        Text = "MINADO CERRO PELÓN Y SALTO",
                        Value = "DCP",
                        Prefijo = "ENERO / 2011",
                        Selectable = true,
                        isGroup = true,
                        options = cboCerroPelon.ToList()
                    });
                }
                if(cboConstPesada.Any())
                {
                    gpoCbo.Add(new ComboGroupDTO()
                    {
                        Text = "CONSTRUCCIÓN PESADA",
                        Value = "PES",
                        Prefijo = "ENERO / 2011",
                        Selectable = true,
                        isGroup = true,
                        options = cboConstPesada.ToList()
                    });
                }
                if(cboAdministracion.Any())
                {
                    gpoCbo.Add(new ComboGroupDTO()
                    {
                        Text = "ADMINISTRACIÓN",
                        Value = "000",
                        Prefijo = "ENERO / 2011",
                        Selectable = true,
                        isGroup = true,
                        options = cboAdministracion.ToList()
                    });
                }
                if(cboEdificio.Any())
                {
                    gpoCbo.Add(new ComboGroupDTO()
                    {
                        Text = "INVERSIÓN EDIFICIO",
                        Value = "IED",
                        Prefijo = "ENERO / 2011",
                        Selectable = true,
                        isGroup = true,
                        options = cboEdificio.ToList()
                    });
                }
                if(cboGastosFinancieros.Any())
                {
                    gpoCbo.Add(new ComboGroupDTO()
                    {
                        Text = "GASTOS FINANCIEROS Y OTROS",
                        Value = "IGF",
                        Prefijo = "ENERO / 2011",
                        Selectable = true,
                        isGroup = true,
                        options = cboGastosFinancieros.ToList()
                    });
                }
                if(cboAlimentos.Any())
                {
                    gpoCbo.Add(new ComboGroupDTO()
                    {
                        Text = "ALIMENTOS Y BEBIDAS",
                        Value = "IAB",
                        Prefijo = "ENERO / 2011",
                        Selectable = true,
                        isGroup = true,
                        options = cboAlimentos.ToList()
                    });
                }
                if(cboAutomotriz.Any())
                {
                    gpoCbo.Add(new ComboGroupDTO()
                    {
                        Text = "AUTOMOTRIZ",
                        Value = "IAM",
                        Prefijo = "ENERO / 2011",
                        Selectable = true,
                        isGroup = true,
                        options = cboAutomotriz.ToList()
                    });
                }
                if(cboEnergia.Any())
                {
                    gpoCbo.Add(new ComboGroupDTO()
                    {
                        Text = "ENERGÍA",
                        Value = "IEN",
                        Prefijo = "ENERO / 2011",
                        Selectable = true,
                        isGroup = true,
                        options = cboEnergia.ToList()
                    });
                }
                if(cboNocheBuena.Any())
                {
                    gpoCbo.Add(new ComboGroupDTO()
                    {
                        Text = "MINADO NOCHE BUENA I Y II",
                        Value = "NOB",
                        Prefijo = "ENERO / 2011",
                        Selectable = true,
                        isGroup = true,
                        options = cboNocheBuena.ToList()
                    });
                }
                var ltsStr = new List<string>();
                ltsStr.AddRange(cboMineria.Select(x => x.Value));
                ltsStr.AddRange(cboColorada.Select(x => x.Value));
                ltsStr.AddRange(cboNocheBuena.Select(x => x.Value));
                ltsStr.AddRange(cboSanAgustin.Select(x => x.Value));
                ltsStr.AddRange(cboConstPesada.Select(x => x.Value));
                ltsStr.AddRange(cboCerroPelon.Select(x => x.Value));
                ltsStr.AddRange(cboAdministracion.Select(x => x.Value));
                ltsStr.AddRange(cboEdificio.Select(x => x.Value));
                ltsStr.AddRange(cboAlimentos.Select(x => x.Value));
                ltsStr.AddRange(cboAutomotriz.Select(x => x.Value));
                ltsStr.AddRange(cboEnergia.Select(x => x.Value));
                if(cboIndividual.Any())
                {
                    gpoCbo.AddRange(cboIndividual.Where(x => !ltsStr.Contains(x.Value)).Select(x => new ComboGroupDTO
                    {
                        Text = x.Text,
                        Value = x.Value,
                        isGroup = false,
                        Prefijo = x.Prefijo
                    }));
                }
                ltsStr.AddRange(cboIndividual.Select(x => x.Value));
                if(cboIndividualSinOrden.Any())
                {
                    gpoCbo.AddRange(cboIndividualSinOrden.Where(x => !ltsStr.Contains(x.Value)).Select(x => new ComboGroupDTO
                    {
                        Text = x.Text,
                        Value = x.Value,
                        isGroup = false,
                        Prefijo = x.Prefijo
                    }));
                }
                ltsStr.AddRange(cboIndividualSinOrden.Select(x => x.Value));
                if(cboCerrado.Any())
                {
                    gpoCbo.AddRange(cboCerrado.Where(x => !ltsStr.Contains(x.Value)).Select(x => new ComboGroupDTO
                    {
                        Text = x.Text,
                        Value = x.Value,
                        isGroup = false,
                        Prefijo = x.Prefijo,
                        addClass = "ccObraCerrada"
                    }));
                }
                if(esTODO)
                {
                    gpoCbo.Insert(0, new ComboGroupDTO
                    {
                        Text = "TODOS",
                        Value = "TODOS",
                        isGroup = false,
                        Prefijo = "ENERO / 2020",
                    });
                }
                result.Add(ITEMS, gpoCbo);
                result.Add(SUCCESS, true);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        List<ComboGroupDTO> cboGrupoConcepto(MovpolDTO mov, List<tblC_FE_CatConcepto> lstConcepto, List<tblC_FE_RelConceptoTm> RelConcepto)
        {
            return lstConcepto
                  .Where(cto => RelConcepto.Any(rel => rel.idConcepto == cto.id && rel.tm == mov.itm))
                  .GroupBy(g => g.idpadre)
                  .Select(cto => new ComboGroupDTO
                  {
                      label = lstConcepto.FirstOrDefault(pad => pad.id == cto.Key).Concepto,
                      options = cto.Select(opt => new ComboDTO()
                      {
                          Text = opt.Concepto,
                          Value = opt.id.ToString()
                      }).ToList()
                  }).ToList();
        }
        public ActionResult getLstGrupoReserva()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cbo = flujoFS.getLstGrupoReserva();
                result.Add(ITEMS, cbo);
                result.Add(SUCCESS, cbo.Count > 0);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCboSemana()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cbo = cboSemana();
                result.Add(ITEMS, cbo);
                result.Add(SUCCESS, cbo.Count > 0);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //List<object> cboSemana()
        //{
        //    var cbo = new List<object>();
        //    init();

        //    var lstCptoCierre = lstConceptoDir.Where(cpto => cpto.idPadre == 30).Select(cpto => cpto.id);
        //    var lst = flujoFS.getPlaneacion(false).Where(w => !lstCptoCierre.Contains(w.idConceptoDir) && w.flujoTotal + w.corte != 0);
        //    var lstAnio = from s in lst group s by s.anio into anio select anio.Key;
        //    foreach(var anio in lstAnio.OrderBy(o => o))
        //    {
        //        var lstSemana = from s in lst where s.anio == anio group s by s.semana into semana select semana.Key;
        //        foreach(var semana in lstSemana.OrderBy(o => o))
        //        {
        //            var max = (from sem in lst where sem.anio == anio && sem.semana == semana select sem).Max(sem => sem.fecha);
        //            var min = max.AddDays(-6);
        //            cbo.Add(new
        //            {
        //                Text = string.Format("Semana {0} - {1:dd/MM/yyyy} - {2:dd/MM/yyyy}", semana, min, max),
        //                Value = "",
        //                Prefijo = JsonConvert.SerializeObject(new
        //                {
        //                    anio,
        //                    semana,
        //                    min = min.ToShortDateString(),
        //                    max = max.ToShortDateString()
        //                })
        //            });
        //        }
        //    }
        //    return cbo;
        //}
        List<object> cboSemana()
        {
            var cbo = new List<object>();
            var cortes = flujoFS.getSemanasCorte();
            foreach (var i in cortes)
            {
                cbo.Add(new
                {
                    Text = string.Format("Semana {0} - {1:dd/MM/yyyy} - {2:dd/MM/yyyy}", i.semana, i.fecha_inicio, i.fecha_fin),
                    Value = "",
                    Prefijo = JsonConvert.SerializeObject(new
                    {
                        i.anio,
                        i.semana,
                        min = i.fecha_inicio.ToShortDateString(),
                        max = i.fecha_fin.ToShortDateString()
                    })
                });
            }
            
            return cbo;
        }
        #endregion
        #region Conversiones
        List<FlujoEfectivoOperativoDTO> generarFlujoEfectivoOperativo(List<tblC_FE_MovPol> lstMovPol, BusqFlujoEfectivoDTO busq)
        {
            var lst = new List<FlujoEfectivoOperativoDTO>();
            var blanco = new FlujoEfectivoOperativoDTO() { clase = (tipoPropuestaEnum.Encabezado).GetDescription() };
            var idIncremeno = 19;
            var idCobroCliente = 4;
            var idGastosCorp = 23;
            var idRevaluacionTC = 20;
            var fecha = busq.max;
            var anioAnt = fecha.Year - 1;
            var gpoContepto = lstConcepto.Where(w => w.idpadre > 0 && w.idpadre < idIncremeno).GroupBy(g => g.idpadre).OrderBy(o => o.Key).ToList();
            var lstBanMovPol = new List<FlujoEfectivoOperativoDTO>();
            var cobroCliente = lstMovPol.Where(w => w.idConcepto == idCobroCliente).Sum(s => s.monto);
            var cobroClienteMes = lstMovPol.Where(w => w.idConcepto == idCobroCliente && w.mes == fecha.Month).Sum(s => s.monto);
            var lstSaldoInicial = flujoFS.getLstFE_SaldoInicial(anioAnt).Where(w => busq.lstCC.Contains("TODOS") ? w.cc == "TODOS" : busq.lstCC.Contains(w.cc)).ToList();
            gpoContepto.ForEach(ctos =>
            {
                var padre = lstConcepto.FirstOrDefault(p => p.id == ctos.Key);
                var lstFlujoEfectivo = new List<FlujoEfectivoOperativoDTO>();
                var lstPadreMovpol = lstMovPol.Where(w => ctos.Any(a => a.id == w.idConcepto)).ToList();
                lstFlujoEfectivo.Add(new FlujoEfectivoOperativoDTO(padre) { clase = (tipoPropuestaEnum.Encabezado).GetDescription() });
                ctos.OrderBy(o => o.Concepto).ToList().ForEach(concepto =>
                {
                    var movpol = concepto.id == idGastosCorp ? lstMovPol.Where(w => w.cc == "A03").ToList() : lstMovPol.Where(w => w.idConcepto == concepto.id).ToList();
                    var acumulado = new FlujoEfectivoOperativoDTO(concepto, movpol, fecha, cobroCliente, cobroClienteMes) { clase = (tipoPropuestaEnum.Saldo).GetDescription() };
                    lstFlujoEfectivo.Add(acumulado);
                    lstBanMovPol.Add(acumulado);
                });
                var GenActividades = new FlujoEfectivoOperativoDTO(padre, lstFlujoEfectivo, fecha, cobroCliente, cobroClienteMes) { clase = (tipoPropuestaEnum.Suma).GetDescription() };
                switch(ctos.Key)
                {
                    case 2:
                        GenActividades.idConcepto = -3;
                        lstFlujoEfectivo.Add(GenActividades);
                        var neto = new FlujoEfectivoOperativoDTO(padre, lstBanMovPol, fecha, cobroCliente, cobroClienteMes) { clase = (tipoPropuestaEnum.Suma).GetDescription() };
                        neto.descripcion = string.Format("FLUJO DE EFECTIVO EXCEDENTE A UTLIZAR EN ACTIVIDADES DE FINANCIACION").ToUpper();
                        lstFlujoEfectivo.Add(neto);
                        break;
                    default:
                        lstFlujoEfectivo.Add(GenActividades);
                        break;
                }
                lst.AddRange(lstFlujoEfectivo);
            });
            lst.Add(blanco);
            lstConcepto.Where(cto => cto.id == idIncremeno).ToList().ForEach(ctos =>
            {
                var lstFlujoEfectivo = new List<FlujoEfectivoOperativoDTO>();
                var lstPadreMovpol = lstMovPol.Where(w => ctos.id == w.idConcepto).ToList();
                var incremento = new FlujoEfectivoOperativoDTO(ctos, lstBanMovPol, fecha, cobroCliente, cobroClienteMes) { clase = (tipoPropuestaEnum.Saldo).GetDescription() };
                incremento.descripcion = string.Format("{0} NETO DE EFECTIVO OPERATIVO", ctos.Concepto);
                lstFlujoEfectivo.Add(incremento);
                lstConcepto.Where(cto => cto.idpadre == idIncremeno && cto.id != idRevaluacionTC).ToList().ForEach(concepto =>
                {
                    var movpol = lstMovPol.Where(w => w.idConcepto == concepto.id).ToList();
                    var acumulado = new FlujoEfectivoOperativoDTO(concepto, movpol, fecha, cobroCliente, cobroClienteMes) { clase = (tipoPropuestaEnum.Saldo).GetDescription() };
                    lstFlujoEfectivo.Add(acumulado);
                    lstBanMovPol.Add(acumulado);
                });
                var incrementoNeto = new FlujoEfectivoOperativoDTO(ctos, lstFlujoEfectivo, fecha, cobroCliente, cobroClienteMes) { clase = (tipoPropuestaEnum.SaldoEncabezado).GetDescription() };
                incrementoNeto.descripcion = string.Format("{0} NETO DE EFECTIVO", ctos.Concepto);
                lstFlujoEfectivo.Add(incrementoNeto);
                lst.AddRange(lstFlujoEfectivo);
            });
            var ctoRevaluacionTC = lstConcepto.FirstOrDefault(cpto => cpto.id == idRevaluacionTC);
            var lstMovRevaluacionTC = lstMovPol.Where(w => w.idConcepto == idRevaluacionTC).ToList();
            var acumRevaluacionTC = new FlujoEfectivoOperativoDTO(ctoRevaluacionTC, lstMovRevaluacionTC, fecha, cobroCliente, cobroClienteMes) { clase = (tipoPropuestaEnum.Saldo).GetDescription() };
            lstBanMovPol.Add(acumRevaluacionTC);
            lst.Add(acumRevaluacionTC);
            lst.Add(blanco);
            var inicio = new FlujoEfectivoOperativoDTO(lstSaldoInicial, lstMovPol, fecha) { clase = (tipoPropuestaEnum.SaldoEncabezado).GetDescription() };
            lstBanMovPol.Add(inicio);
            lst.Add(inicio);
            lst.Add(blanco);
            var fin = new FlujoEfectivoOperativoDTO(new tblC_FE_CatConcepto(), lstBanMovPol, fecha, cobroCliente, cobroClienteMes) { clase = (tipoPropuestaEnum.SaldoEncabezado).GetDescription() };
            fin.descripcion = "EFECTIVO Y EQUIVALENTE DE EFECTIVO AL FINAL DEL AÑO";
            fin.idConcepto = -2;
            fin.acum = lstSaldoInicial.Sum(s => s.saldo) + lstMovPol.Sum(s => s.monto);
            fin.acumPorcentaje = 0;
            fin.mesPorcentaje = 0;
            lst.Add(fin);
            lstMovPol.Insert(0, new tblC_FE_MovPol()
            {
                year = fecha.Year,
                mes = 1,
                cc = busq.lstCC.FirstOrDefault(),
                concepto = "Inicio de periodo " + fecha.Year,
                monto = lstSaldoInicial.Sum(s => s.saldo),
                itm = 50,
                itmOri = 50,
                fechapol = new DateTime(fecha.Year, 1, 1)
            });
            return lst;
        }
        List<tblC_FE_MovPol> matchFlujoEfectivoOperativoYEnkontrol(List<tblC_FE_MovPol> lstMovPol, BusqFlujoEfectivoDTO busq)
        {
            var ahora = DateTime.Now;
            var idCostoProyecto = 6;
            var idGastosOperativo = 7;
            var lstCostos = new List<int>()
            {
                idCostoProyecto,
                idGastosOperativo
            };
            var lstDivAdmin = new List<int>()
            {
                (int)TipoCCEnum.Administración,
                (int)TipoCCEnum.GastosFininacierosYOtros
            };
            var lstCCAdmin = lstCC.Where(w => lstDivAdmin.Contains(w.bit_area.ParseInt())).Select(s => s.cc).ToList();
            var lstEnk = flujoFS.getLstMovPol(busq);
            lstEnk.Where(e => RelConcepto.Any(r => r.tm == e.itm)).ToList().ForEach(enk =>
            {
                var movPol = (from mov in lstMovPol
                              where mov.year == enk.year && mov.mes == enk.mes && mov.tp == enk.tp && mov.poliza == enk.poliza && mov.linea == enk.linea
                              select mov).FirstOrDefault();
                if(movPol == null)
                {
                    var idCptoDir = RelConceptoDir.Any(r => r.tm == enk.tm) ? RelConceptoDir.FirstOrDefault(r => r.tm == enk.tm).idConceptoDir : 0;
                    if(lstCostos.Contains(idCptoDir))
                    {
                        var cc = lstCC.FirstOrDefault(c => c.cc == enk.cc);
                        if(lstDivAdmin.Contains(cc.bit_area.ParseInt()))
                        {
                            idCptoDir = idGastosOperativo;
                        }
                        else
                        {
                            idCptoDir = idCostoProyecto;
                        }
                    }
                    lstMovPol.Add(new tblC_FE_MovPol()
                    {
                        year = enk.year,
                        mes = enk.mes,
                        tp = enk.tp,
                        poliza = enk.poliza,
                        linea = enk.linea,
                        cc = enk.cc,
                        ac = enk.area.ToString() + "-" + enk.cuenta_oc.ToString(),
                        concepto = enk.concepto,
                        cta = enk.cta,
                        scta = enk.scta,
                        sscta = enk.sscta,
                        fechapol = enk.fechapol,
                        monto = enk.monto,
                        numpro = enk.numpro.Value,
                        tm = enk.tm,
                        itm = enk.itm,
                        itmOri = enk.itm,
                        idConcepto = RelConcepto.FirstOrDefault(r => r.tm == enk.itm).idConcepto,
                        idConceptoDir = idCptoDir,
                        fechaRegistro = ahora,
                        esFlujoEfectivo = true,
                        esActivo = true
                    });
                }
            });
            return lstMovPol;
        }
        List<MovpolDTO> getMovFlujoParaDetalles(BusqFlujoEfectivoDetDTO busqDet)
        {
            var busq = (BusqFlujoEfectivoDTO)Session["busqFlujoEfectivo"];
            var esTodos = busq.lstCC.Contains("TODOS");
            var lstIdCpto = getLstIdCptoDirDesdeIdCptoDir(busqDet.idConceptoDir);
            var lstTmMovdir = (from r in RelConceptoDir where lstIdCpto.Contains(r.idConceptoDir) select r.tm).ToList();
            lstIdCpto.Remove(7);
            var lstTmMovdirArr = (from r in RelConceptoDirArr where lstIdCpto.Contains(r.idConceptoDir) select r.tm).ToList();            
            //busq.max = getUltimaFechaCorte();
            busq.min = busq.esFlujo ? new DateTime(2010, 1, 1) : new DateTime(busq.max.Year, 1, 1);
            var lstMovFlujo = (List<MovpolDTO>)Session["lstFlujo"] ?? new List<MovpolDTO>();
            List<MovpolDTO> lstMovCpto = new List<MovpolDTO>();

            if (busqDet.empresa == 0 || busqDet.empresa == EmpresaEnum.Construplan)
            {
                var lstMovCptoCplan = (from mov in lstMovFlujo.AsQueryable()
                              where lstTmMovdir.Contains(mov.itm) && mov.fechapol >= busq.min && mov.fechapol <= busq.max
                              && (mov.empresa == EmpresaEnum.Construplan)
                              && (busqDet.cta == 0 ? true : mov.cta == busqDet.cta)
                              && (busqDet.scta == 0 ? true : mov.scta == busqDet.scta)
                              && (busqDet.sscta == 0 ? true : mov.sscta == busqDet.sscta)
                              && ((busqDet.concepto == null || busqDet.concepto == string.Empty) ? true : mov.concepto == busqDet.concepto)
                              && ((busqDet.cc == null || busqDet.cc == string.Empty) ?
                                    true :
                                        (mov.empresa == EmpresaEnum.Construplan ?
                                            mov.cc == busqDet.cc :
                                            mov.area + "-" + mov.cuenta_oc == busqDet.cc
                                         )
                              )
                              select mov).ToList();
                lstMovCpto.AddRange(lstMovCptoCplan);
            }
            if (busqDet.empresa == 0 || busqDet.empresa == EmpresaEnum.Arrendadora)
            {
                var lstMovCptoArr = (from mov in lstMovFlujo.AsQueryable()
                                     where lstTmMovdirArr.Contains(mov.itm) && mov.fechapol >= busq.min && mov.fechapol <= busq.max
                                  && (mov.empresa == EmpresaEnum.Arrendadora)
                                  && (busqDet.cta == 0 ? true : mov.cta == busqDet.cta)
                                  && (busqDet.scta == 0 ? true : mov.scta == busqDet.scta)
                                  && (busqDet.sscta == 0 ? true : mov.sscta == busqDet.sscta)
                                  && ((busqDet.concepto == null || busqDet.concepto == string.Empty) ? true : mov.concepto == busqDet.concepto)
                                  && ((busqDet.cc == null || busqDet.cc == string.Empty) ?
                                        true :
                                            (mov.empresa == EmpresaEnum.Construplan ?
                                                mov.cc == busqDet.cc :
                                                mov.area + "-" + mov.cuenta_oc == busqDet.cc
                                             )
                                  )
                                     select mov).ToList();
                lstMovCpto.AddRange(lstMovCptoArr);
            }
            lstMovCpto = asignarConceptoParaObraYOperativo(lstMovCpto, busqDet);
            return lstMovCpto;
        }
        List<MovpolDTO> getMovFlujoParaDetalles_Optimizado(BusqFlujoEfectivoDetDTO busqDet)
        {
            
            var busq = (BusqFlujoEfectivoDTO)Session["busqFlujoEfectivo"];
            var esTodos = busq.lstCC.Contains("TODOS");
            var lstIdCpto = getLstIdCptoDirDesdeIdCptoDir(busqDet.idConceptoDir);
            var lstTmMovdir = (from r in RelConceptoDir where lstIdCpto.Contains(r.idConceptoDir) select r.tm).ToList();
            lstIdCpto.Remove(7);
            var lstTmMovdirArr = (from r in RelConceptoDirArr where lstIdCpto.Contains(r.idConceptoDir) select r.tm).ToList();
            //busq.max = getUltimaFechaCorte();
            busq.min = busq.esFlujo ? new DateTime(2010, 1, 1) : new DateTime(busq.max.Year, 1, 1);
            FiltroPolizasDTO filtro = new FiltroPolizasDTO();
            #region filtrar CCs
            var idCostoProyecto = 6;
            var idGastosOperativo = 7;
            var lstCostos = new List<int>()
            {
                idCostoProyecto,
                idGastosOperativo
            };
            var lstDivAdmin = new List<int>()
            {
                (int)TipoCCEnum.Administración,
                (int)TipoCCEnum.GastosFininacierosYOtros
            };
            List<string> listaCCs = new List<string>();
            if (string.IsNullOrEmpty(busqDet.cc))
            {
                listaCCs.AddRange(busq.lstCC);
            }
            else
            {
                listaCCs.Add(busqDet.cc);
            }
            var lstCCAdmin = lstCC.Where(w => lstDivAdmin.Contains(w.bit_area.ParseInt())).Select(s => s.cc).ToList();

            if (lstCostos.Contains(busqDet.idConceptoDir))
            {

                if (esTodos)
                {
                    List<string> ccs = new List<string>();
                    if (busqDet.idConceptoDir == idGastosOperativo)
                    {
                        ccs.AddRange(listaCCs.Where(x => lstCCAdmin.Contains(x)).ToList());
                    }
                    if (busqDet.idConceptoDir == idCostoProyecto)
                    {
                        ccs.AddRange(listaCCs.Where(x => !lstCCAdmin.Contains(x)).ToList());
                    }
                    filtro.lstCC = ccs;
                }
                else
                {
                    filtro.lstCC = listaCCs;
                }
            }
            else
            {
                filtro.lstCC = listaCCs;
            }
            #endregion

            List<MovpolDTO> lstMovCpto = new List<MovpolDTO>();
            filtro.empresa = EmpresaEnum.Construplan;
            filtro.listTM = lstTmMovdir;
            filtro.listTMArr = lstTmMovdirArr;
            filtro.busq = busq;
            filtro.busqDet = busqDet;
            filtro.tipo = busqDet.tipo;

            var lstMovFlujo = flujoFS.taskLstMovPolFlujoTotal_Optimizado(filtro);
            lstMovCpto.AddRange(lstMovFlujo);
            //if (busqDet.empresa == 0 || busqDet.empresa == EmpresaEnum.Construplan)
            //{
            //    FiltroPolizasDTO filtro = new FiltroPolizasDTO();
            //    filtro.empresa = EmpresaEnum.Construplan;
            //    filtro.listTM = lstTmMovdir;
            //    filtro.busq = busq;
            //    filtro.busqDet = busqDet;
            //    filtro.tipo = busqDet.tipo;
            //    filtro.lstCC = busq.lstCC;
            //    var lstMovFlujo = flujoFS.taskLstMovPolFlujoTotal_Optimizado(filtro);
            //    var lstMovCptoCplan = (from mov in lstMovFlujo
            //                           where lstTmMovdir.Contains(mov.itm)
            //                           select mov).ToList();
            //    lstMovCpto.AddRange(lstMovCptoCplan);
            //}
            //if (busqDet.empresa == 0 || busqDet.empresa == EmpresaEnum.Arrendadora)
            //{
            //    FiltroPolizasDTO filtro = new FiltroPolizasDTO();
            //    filtro.empresa = EmpresaEnum.Arrendadora;
            //    filtro.listTM = lstTmMovdirArr;
            //    filtro.busq = busq;
            //    filtro.busqDet = busqDet;
            //    filtro.tipo = busqDet.tipo;
            //    var lstMovFlujo = flujoFS.taskLstMovPolFlujoTotal_Optimizado(filtro);
            //    var lstMovCptoArr = (from mov in lstMovFlujo
            //                         where lstTmMovdirArr.Contains(mov.itm) && mov.fechapol >= busq.min && mov.fechapol <= busq.max
            //                      && (mov.empresa == EmpresaEnum.Arrendadora)
            //                      && (busqDet.cta == 0 ? true : mov.cta == busqDet.cta)
            //                      && (busqDet.scta == 0 ? true : mov.scta == busqDet.scta)
            //                      && (busqDet.sscta == 0 ? true : mov.sscta == busqDet.sscta)
            //                         select mov).ToList();
            //    lstMovCpto.AddRange(lstMovCptoArr);
            //}
            //lstMovCpto = asignarConceptoParaObraYOperativo_Optimizado(lstMovCpto, busqDet);
            return lstMovCpto;
        }
        
        List<tblC_FE_MovPol> getMovPolParaDetalles(BusqFlujoEfectivoDetDTO busqDet)
        {
            var busq = (BusqFlujoEfectivoDTO)Session["busqFlujoEfectivo"];
            var esTodos = busq.lstCC.Contains("TODOS");
            var lstIdCpto = getLstIdCptoDirDesdeIdCptoDir(busqDet.idConceptoDir);
            busq.min = busq.max.AddDays(-6);
            var lstMovpol = (List<tblC_FE_MovPol>)Session["tblC_FE_MovPol"] ?? new List<tblC_FE_MovPol>();
            var lstMovCpto = (from movpol in lstMovpol.AsQueryable()
                              where lstIdCpto.Contains(movpol.idConceptoDir) && movpol.fechapol >= busq.min && movpol.fechapol <= busq.max
                              && (busqDet.empresa == 0 ? true : busqDet.empresa == movpol.empresa)
                              && (busqDet.cta == 0 ? true : movpol.cta == busqDet.cta)
                              && (busqDet.scta == 0 ? true : movpol.scta == busqDet.scta)
                              && (busqDet.sscta == 0 ? true : movpol.sscta == busqDet.sscta)
                              && (busqDet.concepto == null || busqDet.concepto == string.Empty ? true : movpol.concepto == busqDet.concepto)
                              && (busqDet.cc == null || busqDet.cc == string.Empty ?
                                    true :
                                    (movpol.empresa == EmpresaEnum.Construplan ?
                                        movpol.cc == busqDet.cc :
                                        movpol.ac == busqDet.cc
                                     )
                                 )
                              select movpol).ToList();
            return lstMovCpto;
        }
        List<tblC_FE_MovPol> getMovPolParaDetalles_Optimizado(BusqFlujoEfectivoDetDTO busqDet)
        {
            var busq = (BusqFlujoEfectivoDTO)Session["busqFlujoEfectivo"];
            var esTodos = busq.lstCC.Contains("TODOS");
            var lstIdCpto = getLstIdCptoDirDesdeIdCptoDir(busqDet.idConceptoDir);
            busq.min = busq.max.AddDays(-6);
            var lstMovpol = flujoFS.getLstMovPolFlujoEfectivoDirecto_Optimizado(busq);
            var lstMovCpto = (from movpol in lstMovpol.AsQueryable()
                              where lstIdCpto.Contains(movpol.idConceptoDir) && movpol.fechapol >= busq.min && movpol.fechapol <= busq.max
                              && (busqDet.empresa == 0 ? true : busqDet.empresa == movpol.empresa)
                              && (busqDet.cta == 0 ? true : movpol.cta == busqDet.cta)
                              && (busqDet.scta == 0 ? true : movpol.scta == busqDet.scta)
                              && (busqDet.sscta == 0 ? true : movpol.sscta == busqDet.sscta)
                              && (busqDet.concepto == null || busqDet.concepto == string.Empty ? true : movpol.concepto == busqDet.concepto)
                              && (busqDet.cc == null || busqDet.cc == string.Empty ?
                                    true :
                                    (movpol.empresa == EmpresaEnum.Construplan ?
                                        movpol.cc == busqDet.cc :
                                        movpol.ac == busqDet.cc
                                     )
                                 )
                              select movpol).ToList();
            return lstMovCpto;
        }
        List<tblC_FED_PlaneacionDet> getDetPlanParaDetalles(BusqFlujoEfectivoDetDTO busqDet)
        {
            var busq = (BusqFlujoEfectivoDTO)Session["busqFlujoEfectivo"];
            var noEsCliente = busqDet.numcte > 0;
            var noEsProveedor = busqDet.numpro > 0;
            var anio = busqDet.fechaPlaneacion.Year;
            var semana = busqDet.fechaPlaneacion.noSemana();
            var lstIdCpto = getLstIdCptoDirDesdeIdCptoDir(busqDet.idConceptoDir);
            var lstPlanDet = (List<tblC_FED_PlaneacionDet>)Session["tblC_FED_PlaneacionDet"] ?? new List<tblC_FED_PlaneacionDet>();
            var tipoPlan = new List<tipoDetallePlaneacionEnum>();
            switch(busqDet.tipo)
            {
                case tipoDetalleEnum.PlanManual:
                    {
                        tipoPlan.Add(busqDet.tipoPlan);
                        if (lstPlanDet.Any(x=>x.idDetProyGemelo>0 && x.concepto==13))
                            tipoPlan.Add(tipoDetallePlaneacionEnum.contratos);
                    }
                    break;
                case tipoDetalleEnum.contratos:
                    tipoPlan.Add(busqDet.tipoPlan);
                    break;
                case tipoDetalleEnum.PlanProveedor:
                    {
                        if (busqDet.tipoPlan == tipoDetallePlaneacionEnum.cadenaProductiva)
                        tipoPlan.Add(tipoDetallePlaneacionEnum.cadenaProductiva);
                        else
                        tipoPlan.Add(tipoDetallePlaneacionEnum.gastosProyecto);
                        break;
                    }
                    
                case tipoDetalleEnum.PlanProveedorFactura:
                    tipoPlan.Add(busqDet.tipoPlan);
                    break;
                case tipoDetalleEnum.PlanCliente:
                    tipoPlan.Add(tipoDetallePlaneacionEnum.efectivoRecibido);
                    break;
                case tipoDetalleEnum.PlanClienteFactura:
                    tipoPlan.Add(busqDet.tipoPlan);
                    break;
            case tipoDetalleEnum.PlanPrincipal:
                default:
                    break;
            }
            var lstPlanCpto = (from plan in lstPlanDet
                               where plan.año == anio && plan.semana == semana && lstIdCpto.Contains(plan.concepto)
                               && (busqDet.tipoPlan == 0 ?  (tipoPlan.Any() ? tipoPlan.Contains(plan.tipo) : true) : plan.tipo == busqDet.tipoPlan)
                               && (noEsCliente ? plan.numcte == busqDet.numcte : true)
                               && (noEsProveedor ? plan.numprov == busqDet.numpro : true)
                               && (busqDet.empresa == 0 ? true : plan.empresa == busqDet.empresa)
                               select plan).ToList();
            lstPlanCpto.Where(x => x.concepto == 13 && x.idDetProyGemelo == 0).ToList().ForEach(x=>x.tipo = tipoDetallePlaneacionEnum.contratos);
            return lstPlanCpto;
        }
        List<tblC_FED_PlaneacionDet> getDetPlanParaDetalles_Optimizado(BusqFlujoEfectivoDetDTO busqDet)
        {
            var busq = (BusqFlujoEfectivoDTO)Session["busqFlujoEfectivo"];
            var noEsCliente = busqDet.numcte > 0;
            var noEsProveedor = busqDet.numpro > 0;
            var anio = busqDet.fechaPlaneacion.Year;
            var semana = busqDet.fechaPlaneacion.noSemana();
            var lstIdCpto = getLstIdCptoDirDesdeIdCptoDir(busqDet.idConceptoDir);
            var lstPlanDet = flujoFS.getLstDetPlaneacionDirecto_Optimizado(busq);
            var tipoPlan = new List<tipoDetallePlaneacionEnum>();
            switch (busqDet.tipo)
            {
                case tipoDetalleEnum.PlanManual:
                    {
                        tipoPlan.Add(busqDet.tipoPlan);
                        if (lstPlanDet.Any(x => x.idDetProyGemelo > 0 && x.concepto == 13))
                            tipoPlan.Add(tipoDetallePlaneacionEnum.contratos);
                    }
                    break;
                case tipoDetalleEnum.contratos:
                    tipoPlan.Add(busqDet.tipoPlan);
                    break;
                case tipoDetalleEnum.PlanProveedor:
                    {
                        if (busqDet.tipoPlan == tipoDetallePlaneacionEnum.cadenaProductiva)
                            tipoPlan.Add(tipoDetallePlaneacionEnum.cadenaProductiva);
                        else
                            tipoPlan.Add(tipoDetallePlaneacionEnum.gastosProyecto);
                        break;
                    }

                case tipoDetalleEnum.PlanProveedorFactura:
                    tipoPlan.Add(busqDet.tipoPlan);
                    break;
                case tipoDetalleEnum.PlanCliente:
                    tipoPlan.Add(tipoDetallePlaneacionEnum.efectivoRecibido);
                    break;
                case tipoDetalleEnum.PlanClienteFactura:
                    tipoPlan.Add(busqDet.tipoPlan);
                    break;
                case tipoDetalleEnum.PlanPrincipal:
                default:
                    break;
            }
            var prueba = lstPlanDet.Where(x => x.concepto == 13 && x.semana == 37 && x.año == 2021).ToList();
            if (prueba.Count > 0)
            {
                var a = 0;
            }
            lstPlanDet.Where(x => x.concepto == 13 && x.idDetProyGemelo == 0).ToList().ForEach(x => x.tipo = tipoDetallePlaneacionEnum.contratos);
            var lstPlanCpto = (from plan in lstPlanDet
                               where plan.año == anio && plan.semana == semana && lstIdCpto.Contains(plan.concepto)
                               && (busqDet.tipoPlan == 0 ? (tipoPlan.Any() ? tipoPlan.Contains(plan.tipo) : true) : plan.tipo == busqDet.tipoPlan)
                               && (noEsCliente ? plan.numcte == busqDet.numcte : true)
                               && (noEsProveedor ? plan.numprov == busqDet.numpro : true)
                               && (busqDet.empresa == 0 ? true : plan.empresa == busqDet.empresa)
                               select plan).ToList();
            
            return lstPlanCpto;
        }
        List<tblC_FED_DetProyeccionCierre> getProyeccionCierre(BusqFlujoEfectivoDetDTO busqDet)
        {
            var busq = (BusqFlujoEfectivoDTO)Session["busqFlujoEfectivo"];
            var semanaBusqDet = busqDet.fechaPlaneacion == null ? 0 : busqDet.fechaPlaneacion.noSemana();
            var anioBusqDet = busqDet.fechaPlaneacion == null ? 0 : busqDet.fechaPlaneacion.Year;
            var esTodos = busq.lstCC.Contains("TODOS");
            busq.min = busq.max.AddDays(-6);
            busq.idConcepto = busqDet.idConceptoDir;
            var lstCierre = busqDet.esAnterior ? (List<tblC_FED_DetProyeccionCierre>)Session["lstCierreAnterior"] ?? new List<tblC_FED_DetProyeccionCierre>()
                : (List<tblC_FED_DetProyeccionCierre>)Session["lstCierre"] ?? new List<tblC_FED_DetProyeccionCierre>();
            var lstCierreCpto = (from cierre in lstCierre.AsQueryable()
                                 where cierre.idConceptoDir == busqDet.idConceptoDir
                                 && (anioBusqDet > 0 ? cierre.fecha.Year <= anioBusqDet : true)
                                 && (cierre.fecha.Year == anioBusqDet ? cierre.semana <= semanaBusqDet : true)
                                 && (busqDet.empresa == 0 ? true : busqDet.empresa == cierre.empresa)
                                 && ((busqDet.concepto == null || busqDet.concepto == string.Empty) ? true : busqDet.tipo == tipoDetalleEnum.CierreReserva ? cierre.grupo == busqDet.concepto : cierre.descripcion == busqDet.concepto)
                                 select cierre).Distinct().ToList();
            return lstCierreCpto;
        }
        List<tblC_FED_DetProyeccionCierre> getProyeccionCierre_Optimizado(BusqFlujoEfectivoDetDTO busqDet)
        {
            var lstCierre = new List<tblC_FED_DetProyeccionCierre>();
            var busq = (BusqFlujoEfectivoDTO)Session["busqFlujoEfectivo"];
            var semanaBusqDet = busqDet.fechaPlaneacion == null ? 0 : busqDet.fechaPlaneacion.noSemana();
            var anioBusqDet = busqDet.fechaPlaneacion == null ? 0 : busqDet.fechaPlaneacion.Year;
            var esTodos = busq.lstCC.Contains("TODOS");
            busq.min = busq.max.AddDays(-6);
            busq.idConcepto = busqDet.idConceptoDir;

            

            if (busqDet.esAnterior)
            {
                BusqFlujoEfectivoDTO busqAnterior = new BusqFlujoEfectivoDTO();
                busqAnterior.esCC = busq.esCC;
                busqAnterior.esConciliado = busq.esConciliado;
                busqAnterior.esFlujo = busq.esFlujo;
                busqAnterior.idConcepto = busq.idConcepto;
                busqAnterior.lstAC = busq.lstAC;
                busqAnterior.lstCC = busq.lstCC;
                busqAnterior.lstTm = busq.lstTm;
                busqAnterior.max = busq.max.AddDays(-7);
                busqAnterior.min = busq.min.AddDays(-7);
                busqAnterior.tipo = busq.tipo;
                lstCierre = flujoFS.initLstDetProyeccionCierre_Optimizado(busqAnterior);
            }
            else {
                lstCierre = flujoFS.initLstDetProyeccionCierre_Optimizado(busq);
            }
            //var lstCierre = busqDet.esAnterior ? (List<tblC_FED_DetProyeccionCierre>)Session["lstCierreAnterior"] ?? new List<tblC_FED_DetProyeccionCierre>()
            //    : (List<tblC_FED_DetProyeccionCierre>)Session["lstCierre"] ?? new List<tblC_FED_DetProyeccionCierre>();
            var lstCierreCpto = (from cierre in lstCierre.AsQueryable()
                                 where cierre.idConceptoDir == busqDet.idConceptoDir
                                 && (anioBusqDet > 0 ? cierre.fecha.Year <= anioBusqDet : true)
                                 && (cierre.fecha.Year == anioBusqDet ? cierre.semana <= semanaBusqDet : true)
                                 && (busqDet.empresa == 0 ? true : busqDet.empresa == cierre.empresa)
                                 && ((busqDet.concepto == null || busqDet.concepto == string.Empty) ? true : busqDet.tipo == tipoDetalleEnum.CierreReserva ? cierre.grupo == busqDet.concepto : cierre.descripcion == busqDet.concepto)
                                 select cierre).Distinct().ToList();
            return lstCierreCpto;
        }
        List<tblC_FED_DetProyeccionCierre> getProyeccionCierreArrendadora(BusqFlujoEfectivoDetDTO busqDet)
        {
            var busq = (BusqFlujoEfectivoDTO)Session["busqFlujoEfectivo"];
            var esTodos = busq.lstAC.Contains("TODOS");
            busq.min = busq.max.AddDays(-6);
            busq.idConcepto = busqDet.idConceptoDir;
            var lstCierre = (List<tblC_FED_DetProyeccionCierre>)Session["lstCierreArrendadora"] ?? new List<tblC_FED_DetProyeccionCierre>();
            var lstCierreCpto = (from w in lstCierre.AsQueryable()
                                 where w.idConceptoDir == busqDet.idConceptoDir && (esTodos ? true : busq.lstAC.Contains(w.ac))
                                 && busqDet.concepto == null || busqDet.concepto == string.Empty ? true : busqDet.tipo == tipoDetalleEnum.CierreReservaArrendadora ? w.grupo == busqDet.concepto : w.descripcion == busqDet.concepto
                                 select w).ToList();
            return lstCierreCpto;
        }
        List<tblC_FED_DetProyeccionCierre> getProyeccionCierreArrendadora_Optimizado(BusqFlujoEfectivoDetDTO busqDet)
        {
            var busq = (BusqFlujoEfectivoDTO)Session["busqFlujoEfectivo"];
            var esTodos = busq.lstAC.Contains("TODOS");
            busq.min = busq.max.AddDays(-6);
            busq.idConcepto = busqDet.idConceptoDir;
            var lstCierre = (List<tblC_FED_DetProyeccionCierre>)Session["lstCierreArrendadora"] ?? new List<tblC_FED_DetProyeccionCierre>();
            var lstCierreCpto = (from w in lstCierre.AsQueryable()
                                 where w.idConceptoDir == busqDet.idConceptoDir && (esTodos ? true : busq.lstAC.Contains(w.ac))
                                 && busqDet.concepto == null || busqDet.concepto == string.Empty ? true : busqDet.tipo == tipoDetalleEnum.CierreReservaArrendadora ? w.grupo == busqDet.concepto : w.descripcion == busqDet.concepto
                                 select w).ToList();
            return lstCierreCpto;
        }
        List<object> getLstReservaT(List<tblC_FED_DetProyeccionCierre> lst)
        {
            var lstCargo = new List<naturalezaEnum>() { naturalezaEnum.Egreso };
            var lstAbono = new List<naturalezaEnum>() { naturalezaEnum.Ingreso };
            var lstT = lst.Select(s => new
            {
                idConceptoDir = 29,
                tipo = tipoDetalleEnum.CierreReserva,
                concepto = s.descripcion,
                naturaleza = s.naturaleza,
                cargoDesc = lstCargo.Contains(s.naturaleza) ? string.Format("{0} - {1}", s.fecha.ToShortDateString(), s.descripcion) : string.Empty,
                cargo = lstCargo.Contains(s.naturaleza) ? s.monto : 0,
                abonoDesc = lstAbono.Contains(s.naturaleza) ? string.Format("{0} - {1}", s.fecha.ToShortDateString(), s.descripcion) : string.Empty,
                abono = lstAbono.Contains(s.naturaleza) ? s.monto : 0
            }).ToList<Object>();
            return lstT;
        }
        /// <summary>
        /// Genra un lista de IdConceptos proyectados para sumatoria del cuadro
        /// </summary>
        /// <param name="idConceptoDir">idConceptoDir</param>
        /// <returns>lista de IdConceptosDir</returns>
        List<int> getLstIdCptoDirDesdeIdCptoDir(int idConceptoDir)
        {
            if(lstConceptoDir == null)
            {
                init_Optimizado();
            }
            var lstIdCpto = new List<int>();
            if(lstConceptoDir.Any(c => c.idPadre == idConceptoDir))
            {
                lstIdCpto.AddRange(lstConceptoDir.Where(c => c.idPadre <= idConceptoDir).Select(s => s.id).ToList());
            }
            else
            {
                lstIdCpto.Add(idConceptoDir);
            }
            return lstIdCpto;
        }
        /// <summary>
        /// Genra un lista de IdConceptos directo para sumatoria del reporte
        /// </summary>
        /// <param name="idConcepto">idConcepto</param>
        /// <returns>lista de IdConceptos</returns>
        List<int> getLstIdCptoDirDesdeIdCpto(int idConcepto)
        {
            if(lstConcepto == null)
            {
                init();
            }
            var lstIdCpto = new List<int>();
            switch(idConcepto)
            {
                case 19:
                    lstIdCpto.AddRange(lstConcepto.Where(c => c.idpadre != 19).Select(s => s.id));
                    break;
                case -1:
                case -2:
                    lstIdCpto.Add(0);
                    lstIdCpto.AddRange(lstConcepto.Select(s => s.id).ToList());
                    break;
                case -3:
                    lstIdCpto.AddRange(lstConcepto.Where(c => c.idpadre == 2).Select(s => s.id));
                    break;
                default:
                    if(lstConcepto.Any(c => c.idpadre == idConcepto))
                    {
                        lstIdCpto.AddRange(lstConcepto.Where(c => c.idpadre > 0 && c.idpadre <= idConcepto).Select(s => s.id));
                    }
                    else
                    {
                        lstIdCpto.Add(idConcepto);
                    }
                    break;
            }
            return lstIdCpto;
        }
        tblDirectoTheadDTO generarEncabezado(BusqFlujoEfectivoDTO busq)
        {
            var enc = new tblDirectoTheadDTO()
            {
                cc = busq.lstCC.FirstOrDefault(),
            };
            //var fechaCorte = getUltimaFechaCorte();
            var fechaCorte = busq.max;
            var semanaSiguiente = fechaCorte.AddDays(3);
            enc.noSemanaConsulta = busq.max.noSemana();
            enc.fecha = busq.max;
            enc.noSemana = fechaCorte.noSemana();
            enc.fechaCorteMax = fechaCorte;
            enc.noSemanaCorte = fechaCorte.noSemana();
            enc.noSemanaSiguiente = semanaSiguiente.noSemana(DayOfWeek.Thursday);
            if(fechaCorte.Year != semanaSiguiente.Year)
            {
                enc.noSemanaSiguiente = 1;
            }
            return enc;
        }
        List<FlujoEfectivoDirectoDTO> generarFlujoEfectivoDirecto(IQueryable<tblC_FED_CapPlaneacion> lstPlaneacion, BusqFlujoEfectivoDTO busq)
        {
            var idEfectivoRecibido = 1;
            var idEfectivoRecibidoSaldo = 17;
            var lst = new List<FlujoEfectivoDirectoDTO>();
            var fecha = busq.max;
            var semanaActual = fecha.noSemana();
            var fechaAnt = fecha.AddDays(-7);
            var semanaAnt = fechaAnt.noSemana();
            var anioActual = fecha.Year;
            //var fechaCorteMax = getUltimaFechaCorte();
            var fechaCorteMax = fecha;
            var semanaCorteMax = fechaCorteMax.noSemana();
            var fechaSiguiente = fechaCorteMax.AddDays(7);
            var semanaSiguiente = fechaSiguiente.noSemana();
            var anioSiguiente = fechaSiguiente.Year;
            lstConceptoDir = flujoFS.getCatConceptoDirActivo();
            if(anioActual != anioSiguiente)
            {
                semanaSiguiente = 1;
            }
            var esTodos = busq.lstCC.Contains("TODOS");
            var semanaCorteSig = semanaCorteMax + 1;
            var semanaCorteAnt = semanaCorteMax - 1;
            var lstPlanSiguiente = lstPlaneacion.Where(w => anioSiguiente == w.anio && w.semana == semanaSiguiente).ToList();
            var lstPlanActual = lstPlaneacion.Where(w => anioActual == w.anio && w.semana == semanaActual).ToList();
            var lstCorteMax = lstPlaneacion.Where(w => anioActual == w.anio && w.semana == semanaCorteMax).ToList();
            //var lstCorteAnt = lstPlaneacion.Where(w => anioActual == w.anio && w.semana == semanaCorteAnt).ToList();
            var lstCptoPadre = lstConceptoDir.Where(w => w.id > idEfectivoRecibido && w.idPadre == 0).GroupBy(g => g.id);
            var objEfectivoRecibido = lstConceptoDir.FirstOrDefault(w => w.id == idEfectivoRecibidoSaldo);
            //if(semanaActual == 1)
            //{
            //    var anioAnt = anioActual - 1;
            //    var anioAntMaxSemana = lstPlaneacion.Where(w => w.anio == anioAnt).Max(m => m.semana);
            //}
            //if(semanaCorteMax == 1)
            //{
            //    var anioAnt = anioActual - 1;
            //    var anioAntMaxSemana = lstPlaneacion.Where(w => w.anio == anioAnt).Max(m => m.semana);
            //    lstCorteAnt = lstPlaneacion.Where(w => w.anio == anioAnt && w.semana == anioAntMaxSemana).ToList();
            //}
            var saldoInicial = lstCorteMax.Sum(corte => corte.strSaldoInicial.ParseDecimal(0));
            if(busq.esFlujo && !esTodos)
            {
                lstPlanActual.ForEach(corte => corte.flujoTotal += corte.strSaldoInicial.ParseDecimal());
            }
            #region Conceptos
            var lstActualEfectivo = lstPlanActual.Where(w => w.idConceptoDir == idEfectivoRecibidoSaldo);
            var lstCorteMaxEfectivo = lstCorteMax.Where(w => w.idConceptoDir == idEfectivoRecibidoSaldo);
            var lstAcumPlan = new List<tblC_FED_CapPlaneacion>() { 
                new tblC_FED_CapPlaneacion() { 
                    corte = lstActualEfectivo.Sum(w=>w.corte),
                    planeado = lstActualEfectivo.Sum(w=>w.planeado)
                },
                new tblC_FED_CapPlaneacion() { 
                    corte = lstCorteMaxEfectivo.Sum(w=>w.corte),
                    flujoTotal = lstCorteMaxEfectivo.Sum(w=>w.flujoTotal),
                    planeado = lstCorteMaxEfectivo.Sum(w=>w.planeado)
                },
                new tblC_FED_CapPlaneacion() { 
                    planeado = lstPlanSiguiente.Where(w => w.idConceptoDir == idEfectivoRecibidoSaldo).Sum(w=>w.planeado)
                }
            };
            lst.Add(new FlujoEfectivoDirectoDTO(objEfectivoRecibido, lstAcumPlan) { clase = (tipoPropuestaEnum.Suma).GetDescription() });
            lstCptoPadre.ToList().ForEach(cptoPadre =>
            {
                var objPadre = lstConceptoDir.FirstOrDefault(w => w.id == cptoPadre.Key);
                var lstCpto = lstConceptoDir.Where(w => w.idPadre == cptoPadre.Key).ToList();
                for(int i = 0; i < lstCpto.Count; i++)
                {
                    var cpto = lstCpto[i];
                    var lstPlanActualAcum = lstPlanActual.Where(w => w.idConceptoDir == cpto.id);
                    var lstCorteMaxlAcum = lstCorteMax.Where(w => w.idConceptoDir == cpto.id);
                    var lstPlanSiguienteAcum = lstPlanSiguiente.Where(w => w.idConceptoDir == cpto.id);
                    var objFlujo = new FlujoEfectivoDirectoDTO(cpto, lstPlanActualAcum, lstCorteMaxlAcum, lstPlanSiguienteAcum);
                    lst.Add(objFlujo);
                    lstAcumPlan[1].flujoTotal += objFlujo.flujoTotalProyecto;
                    lstAcumPlan[0].corte += objFlujo.consulta;
                    lstAcumPlan[1].planeado += objFlujo.recientePlaneacion1;
                    lstAcumPlan[1].corte += objFlujo.recientePlaneacion2;
                    lstAcumPlan[2].planeado += objFlujo.planeacion;
                }
                lst.Add(new FlujoEfectivoDirectoDTO(objPadre, lstAcumPlan) { clase = (tipoPropuestaEnum.Suma).GetDescription() });
            });
            #endregion
            #region FlujoTotal
            var flujoAcumulado = lstAcumPlan[1].flujoTotal + (esTodos ? saldoInicial : 0);
            var antMax = flujoAcumulado - lst.LastOrDefault().recientePlaneacion2;
            var flujoPlan = flujoAcumulado + lstAcumPlan[2].planeado;
            var flujoAntConsulta = flujoAcumulado - lstAcumPlan[0].corte;

            lst.Add(new FlujoEfectivoDirectoDTO("(=) Flujo Anterior")
            {
                clase = (tipoPropuestaEnum.Suma).GetDescription(),
                idCpto = -2,
                flujoTotalProyecto = esTodos ? saldoInicial : 0,
                consulta = flujoAntConsulta,
                planeacionConsulta = lstPlanActual.Sum(s => s.flujoTotal),
                recientePlaneacion1 = antMax,
                recientePlaneacion2 = antMax,
                planeacion = flujoAcumulado
            });
            var acumPlanConsulta = antMax + lstAcumPlan[1].planeado;
            lst.Add(new FlujoEfectivoDirectoDTO("(=) Flujo Acumulado")
            {
                clase = (tipoPropuestaEnum.Suma).GetDescription(),
                idCpto = -3,
                flujoTotalProyecto = flujoAcumulado,
                consulta = flujoAcumulado,
                planeacionConsulta = lst.LastOrDefault().planeacion + lstAcumPlan[0].planeado,
                recientePlaneacion1 = acumPlanConsulta,
                recientePlaneacion2 = flujoAcumulado,
                planeacion = flujoPlan,
            });
            #endregion
            #region Gastos Oficina Cetral
            var esOfiCentral = (bool)Session["esPermisGastosOficiana"];
            if(!esTodos && !busq.esFlujo && esOfiCentral)
            {
                decimal efectivoRecibido = lst.FirstOrDefault().flujoTotalProyecto;
                var flujoAcumOfiCentral = setGatosOficinaCentral(flujoAcumulado, anioActual, semanaActual, efectivoRecibido);
                lst.Add(new FlujoEfectivoDirectoDTO("(-) Gastos Oficina Central")
                {
                    clase = (tipoPropuestaEnum.Saldo).GetDescription(),
                    idCpto = -4,
                    flujoTotalProyecto = flujoAcumOfiCentral,
                    consulta = flujoAcumOfiCentral,
                    planeacionConsulta = 0,
                    recientePlaneacion1 = 0,
                    recientePlaneacion2 = flujoAcumOfiCentral,
                    planeacion = 0
                });
                lst.Add(new FlujoEfectivoDirectoDTO("(=) Flujo Total")
                {
                    clase = (tipoPropuestaEnum.Suma).GetDescription(),
                    idCpto = -5,
                    flujoTotalProyecto = flujoAcumulado + flujoAcumOfiCentral,
                    consulta = flujoAcumulado + flujoAcumOfiCentral,
                    planeacionConsulta = acumPlanConsulta,
                    recientePlaneacion1 = acumPlanConsulta,
                    recientePlaneacion2 = flujoAcumulado + flujoAcumOfiCentral,
                    planeacion = flujoPlan,
                });
            }
            #endregion
            if(busq.esFlujo && !esTodos)
            {
                lstPlanActual.ForEach(corte => corte.flujoTotal = corte.strFlujoEfectivo.ParseDecimal());
            }
            return lst;
        }
        decimal setGatosOficinaCentral(decimal monto, int anio, int noSemana, decimal efectivoRecibido)
        {
            var lstTodo = flujoFS.getFlujoTodoEfctivoYCostos(anio, noSemana);
            var gastosOperativo = lstTodo.Where(w => w.idConceptoDir == 7).Sum(s => s.flujoTotal);
            var efectivoRecibo = lstTodo.Where(w => w.idConceptoDir == 17).Sum(s => s.flujoTotal);
            var gastos100 = gastosOperativo * 100;
            var gastosEfectivo = efectivoRecibo == 0 ? 0 : decimal.Divide(gastos100, efectivoRecibo);
            var gastosEfectivoMonto = (gastosEfectivo == 0 ? 0 : (gastosEfectivo / 100)) * efectivoRecibido;

            return gastosEfectivoMonto;
        }
        List<FlujoEfectivoDirectoDTO> generarFlujoEfectivoCierre(IQueryable<tblC_FED_CapPlaneacion> lstPlaneacion, BusqFlujoEfectivoDTO busq)
        {
            var lst = new List<FlujoEfectivoDirectoDTO>();
            var semana192020 = new DateTime(2020, 4, 10);
            var idReserva = 29;
            var idPadre = -3;
            //var maxCorte = getUltimaFechaCorte();
            var maxCorte = busq.max;
            var maxCorteAnterior = busq.max.AddDays(-7);
            if(maxCorte.DayOfWeek != DayOfWeek.Saturday)
            {
                maxCorte = maxCorte.Siguiente(DayOfWeek.Saturday);
            }
            if (maxCorteAnterior.DayOfWeek != DayOfWeek.Saturday)
            {
                maxCorteAnterior = maxCorteAnterior.Siguiente(DayOfWeek.Saturday);
            }
            lstConceptoDir = flujoFS.getCatConceptoDirActivo();
            var anio = maxCorte.Year;
            var anioAnterior = maxCorteAnterior.Year;
            var semana = maxCorte.noSemana();
            var semanaAnterior = maxCorteAnterior.noSemana();
            var padre = lstConceptoDir.FirstOrDefault(w => w.idPadre == idPadre);
            var lstCpto = lstConceptoDir.Where(w => w.idPadre == padre.id).OrderBy(o => o.orden);
            var lstPlan = lstPlaneacion.Where(w => w.anio == anio && w.semana == semana && lstCpto.Any(c => c.id == w.idConceptoDir));
            var lstPlanAnterior = lstPlaneacion.Where(w => w.anio == anioAnterior && w.semana == semanaAnterior && lstCpto.Any(c => c.id == w.idConceptoDir));
            var planReserva = lstPlan.FirstOrDefault(w => w.idConceptoDir == idReserva);
            var catReserva = lstConceptoDir.FirstOrDefault(w => w.id == idReserva);
            
            lstCpto.Where(w => w.id != idReserva).ToList().ForEach(cpto =>
            {
                var plan = lstPlan.FirstOrDefault(w => w.idConceptoDir == cpto.id);
                if(plan == null)
                {
                    plan = new tblC_FED_CapPlaneacion()
                    {
                        anio = anio,
                        semana = semana,
                        idConceptoDir = cpto.id
                    };
                }
                var monto = lstPlan.Where(w => w.idConceptoDir == cpto.id).Sum(s => s.corte);
                var montoAnterior = lstPlanAnterior.Where(w => w.idConceptoDir == cpto.id && (w.fecha.Year < anio || (w.fecha.Year == anio && w.semana <= semanaAnterior))).Sum(s => s.corte);
                if(cpto.operador.Trim() == "-")
                {
                    if(!lstCptoOmitir.Contains(cpto.id))
                    {
                        if (monto > 0) {
                            monto *= -1;
                        }
                        if (montoAnterior > 0)
                        {
                            montoAnterior *= -1;
                        }
                    }
                }
                if(lstCptoInvertirSigno.Contains(cpto.id))
                {
                    monto *= -1;
                    montoAnterior *= -1;
                }
                lst.Add(new FlujoEfectivoDirectoDTO()
                {
                    descripcion = string.Format("({0}) {1}", cpto.operador.Trim(), cpto.Concepto),
                    idCpto = cpto.id,
                    flujoTotalProyecto = monto,
                    flujoTotalProyectoAnterior = montoAnterior,
                    clase = (tipoPropuestaEnum.Saldo).GetDescription()
                });
            });
            if(busq.lstCC.Contains("TODOS"))
            {
                busq.idConcepto = idReserva;
                var gpoReserva = flujoFS.getLstDetProyeccionCierre(busq)
                    .Where(w => w.fecha <= busq.max).Distinct()
                    .GroupBy(g => g.grupo);
                lst.AddRange(gpoReserva.Select(s => new FlujoEfectivoDirectoDTO()
                {
                    descripcion = string.Format("({1}) {2} - {0}", s.Key, catReserva.operador.Trim(), catReserva.Concepto),
                    idCpto = idReserva,
                    flujoTotalProyecto = s.Sum(ss => ss.monto),
                    flujoTotalProyectoAnterior = s.Where(ss => ss.fecha <= maxCorteAnterior && (ss.fecha.Year < anio || (ss.fecha.Year == anio && ss.semana <= semanaAnterior))).Sum(ss => ss.monto),
                    clase = (tipoPropuestaEnum.Saldo).GetDescription(),
                }));
            }
            return lst;
        }
        List<FlujoEfectivoDirectoDTO> generarFlujoEfectivoCierreArrendadora(BusqFlujoEfectivoDTO busq)
        {
            BusqFlujoEfectivoDTO busqArrendadora = new BusqFlujoEfectivoDTO();
            busqArrendadora.esCC = false;
            busqArrendadora.esConciliado = false;
            busqArrendadora.esFlujo = false;
            busqArrendadora.idConcepto = 0;
            busqArrendadora.lstAC = new List<string> { "TODOS" };
            busqArrendadora.lstCC = new List<string> { "TODOS" };
            busqArrendadora.lstTm = new List<int>();
            busqArrendadora.max = busq.max;
            busqArrendadora.min = busq.min;
            busqArrendadora.tipo = busq.tipo;
            var lst = new List<FlujoEfectivoDirectoDTO> 
            {
                new FlujoEfectivoDirectoDTO
                {
                    idCpto = -1,
                    descripcion = "(+) SALDO INICIAL",
                    flujoTotalProyecto = flujoArrendadoraFS.getFlujoEfectivoTotalArrendadora(busqArrendadora),
                    clase = (tipoPropuestaEnum.Suma).GetDescription(),
                }
            };
            var idReserva = 29;
            var idPadre = -3;
            var maxCorte = busq.max;
            if(maxCorte.DayOfWeek != DayOfWeek.Saturday)
            {
                maxCorte = maxCorte.Siguiente(DayOfWeek.Saturday);
            }
            var anio = maxCorte.Year;
            var semana = maxCorte.noSemana();
            var padre = lstConceptoDir.FirstOrDefault(w => w.idPadre == idPadre);
            var lstCpto = lstConceptoDir.Where(w => w.idPadre == padre.id).OrderBy(o => o.orden);
            var lstPlan = flujoArrendadoraFS.getPlaneacionCierreArrendadora(new BusqProyeccionCierreDTO()
            {
                max = busq.max,
                min = busq.min,
                esCC = busq.esCC,
                lstCC = busq.lstCC,
                lstAC = busq.lstAC,
                idConceptoDir = busq.idConcepto,
                grupo = string.Empty,
                tipo = 0
            });
            var lstCptoOmitir = new List<int>() { 25, 31, 27, 28, 33 };
            lstCpto.Where(w => w.id != idReserva).ToList().ForEach(cpto =>
            {
                var plan = lstPlan.FirstOrDefault(w => w.idConceptoDir == cpto.id);
                if(plan == null)
                {
                    plan = new tblC_FED_DetProyeccionCierre
                    {
                        anio = anio,
                        semana = semana,
                        idConceptoDir = cpto.id
                    };
                }
                var monto = lstPlan.Where(w => w.idConceptoDir == cpto.id).Sum(s => s.monto);
                if(cpto.operador.Trim() == "-")
                {
                    if(!lstCptoOmitir.Contains(cpto.id) && monto > 0)
                    {
                        monto *= -1;
                    }
                }
                if(lstCptoInvertirSigno.Contains(cpto.id))
                {
                    monto *= -1;
                }
                lst.Add(new FlujoEfectivoDirectoDTO()
                {
                    descripcion = string.Format("({0}) {1}", cpto.operador.Trim(), cpto.Concepto),
                    idCpto = cpto.id,
                    flujoTotalProyecto = monto,
                    clase = (tipoPropuestaEnum.Saldo).GetDescription()
                });
            });
            lst.Add(new FlujoEfectivoDirectoDTO()
            {
                idCpto = -2,
                descripcion = "(=) TOTAL",
                flujoTotalProyecto = lst.Sum(s => s.flujoTotalProyecto),
                clase = (tipoPropuestaEnum.Suma).GetDescription(),
            });
            return lst;
        }
        List<FlujoEfectivoDirectoGraficaDTO> generarFlujoEfectivoDirectoGrafico(IQueryable<tblC_FED_CapPlaneacion> lstMovPol, BusqFlujoEfectivoDTO busq)
        {
            var lst = new List<FlujoEfectivoDirectoGraficaDTO>();
            var esTodos = busq.lstCC.Contains("TODOS");
            var esObraAcumulada = esTodos ? true : busq.esFlujo;
            lst = (from plan in lstMovPol
                   where plan.fecha <= busq.max && (plan.corte + plan.flujoTotal != 0)
                   && (esObraAcumulada ? true : plan.anio == busq.max.Year)
                   group plan by new
                   {
                       plan.anio,
                       plan.semana
                   } into gpo
                   orderby gpo.Key.anio descending, gpo.Key.semana descending
                   select new FlujoEfectivoDirectoGraficaDTO
                   {
                       anio = gpo.Key.anio,
                       noSemana = gpo.Key.semana
                   }).Take(20).Reverse().ToList();
            return lst;
            //si se usara el modal de grafica, elimine las pasada dos lineas
            //var lstIngresos = new List<int>() { 9, 17 };
            //lstSemanas.ForEach(semana =>
            //{
            //    var lstMov = lstMovPol.Where(w => w.anio == busq.max.Year && w.semana == semana);
            //    var total = lstMov.Sum(s => s.corte);
            //    var noAbsIngreso = lstMov.Where(w => lstIngresos.Any(i => i == w.idConceptoDir)).Sum(s => s.corte);
            //    var noAbsEgreso = lstMov.Where(w => !lstIngresos.Any(i => i == w.idConceptoDir)).Sum(s => s.corte);
            //    var montoIngresos = Math.Abs(noAbsIngreso);
            //    var montoEgresos = Math.Abs(noAbsEgreso);
            //    lst.Add(new FlujoEfectivoDirectoGraficaDTO()
            //    {
            //        noSemana = semana,
            //        name = "Ingresos",
            //        monto = new { semana, monto = montoIngresos },
            //        stack = "Ingresos"
            //    });
            //    lst.Add(new FlujoEfectivoDirectoGraficaDTO()
            //    {
            //        noSemana = semana,
            //        name = "Egresos",
            //        monto = new { semana, monto = montoEgresos },
            //        stack = "Egresos"
            //    });
            //    lst.Add(new FlujoEfectivoDirectoGraficaDTO
            //    {
            //        noSemana = semana,
            //        name = "Ganancia",
            //        monto = new { semana, monto = montoIngresos > montoEgresos ? montoIngresos - montoEgresos : 0 },
            //        stack = "Ganancia"
            //    });
            //    lst.Add(new FlujoEfectivoDirectoGraficaDTO
            //    {
            //        noSemana = semana,
            //        name = "Perdida",
            //        monto = new { semana, monto = montoEgresos > montoIngresos ? montoEgresos - montoIngresos : 0 },
            //        stack = "Perdida"
            //    });
            //    lst.Add(new FlujoEfectivoDirectoGraficaDTO
            //    {
            //        noSemana = semana,
            //        name = "Perdida o Ganancia",
            //        monto = new { semana, monto = noAbsIngreso + noAbsEgreso },
            //        stack = "PerdidaGanancia"
            //    });

            //});
        }
        List<MetaGraficoDTO> generarFlujoEfectivoCptosGrafico(IQueryable<FlujoEfectivoDirectoDTO> lstCierre, IQueryable<tblC_FED_CapPlaneacion> lstPlaneacion, BusqFlujoEfectivoDTO busq)
        {
            var idReserva = 29;
            var idPadreCierre = -3;
            var limitGrafica = 20;
            var esTodos = busq.lstCC.Contains("TODOS");
            var esObraAcumulada = esTodos ? true : busq.esFlujo;
            var lst = new List<MetaGraficoDTO>();
            var lstLabelIgnorar = new List<int> { -4, -2, 2, 3, 4 };
            var lstLabelVisible = new List<int> { -6 };
            lstLabelVisible.Add(esTodos ? -3 : busq.esFlujo ? -3 : -5);
            var lstlabel = lstCierre.GroupBy(g => g.idCpto)
                .Where(w => !lstLabelIgnorar.Contains(w.Key))
                .Select(s => new
                {
                    idCpto = s.Key,
                    descripcion = s.FirstOrDefault().descripcion
                }).ToList();
            lstlabel.Add(new
            {
                idCpto = -6,
                descripcion = "(=) WHAT IF?"
            });
            lstConceptoDir = flujoFS.getCatConceptoDirActivo();
            var semana192020 = new DateTime(2020, 4, 10);
            var anioActual = busq.max.Year;
            var maxSemana = busq.max.noSemana(DayOfWeek.Saturday);
            var minSemana = anioActual == 2020 ? 16 : 1;
            var esOfiCentral = (bool)Session["esPermisGastosOficiana"];
            var lstAnioSemana = (from plan in lstPlaneacion
                                 where plan.fecha <= busq.max && plan.corte + plan.flujoTotal != 0
                                     && (esObraAcumulada ? true : plan.anio == anioActual)
                                 group plan by new
                                 {
                                     plan.anio,
                                     plan.semana
                                 } into gpo
                                 orderby gpo.Key.anio descending, gpo.Key.semana descending
                                 select new
                                 {
                                     anio = gpo.Key.anio,
                                     semana = gpo.Key.semana
                                 }).Take(limitGrafica).Reverse().ToList();
            var lstMontoFlujo = lstPlaneacion.Where(w => lstlabel.Any(a => a.idCpto == w.idConceptoDir)).ToList();
            var lstSaldoIni = lstPlaneacion.Where(w => esTodos ? w.cc == "TODOS" : busq.lstCC.Contains(w.cc)).ToList();
            var padre = lstConceptoDir.FirstOrDefault(w => w.idPadre == idPadreCierre);
            var lstCptoCierre = lstConceptoDir.Where(w => w.idPadre == padre.id).ToList();
            var lstMontoCierre = lstPlaneacion.Where(w => lstCptoCierre.Any(a => a.id == w.idConceptoDir) && w.idConceptoDir != idReserva).ToList();
            busq.idConcepto = idReserva;
            var lstReserva = flujoFS.getLstDetProyeccionCierre(busq);
            lstMontoCierre.ForEach(flujo =>
            {
                var cpto = lstConceptoDir.FirstOrDefault(dir => dir.id == flujo.idConceptoDir);
                if(cpto.operador.Trim() == "-")
                {
                    if(!lstCptoOmitir.Contains(cpto.id) && flujo.corte > 0)
                    {
                        flujo.corte *= -1;
                    }
                }
                if(lstCptoInvertirSigno.Contains(cpto.id))
                {
                    flujo.corte *= -1;
                }
            });
            lstlabel.ForEach(cpto =>
            {
                var data = new decimal[lstAnioSemana.Count];
                var isCptoCierre = cpto.idCpto > 0;
                for(int i = 0; i < lstAnioSemana.Count; i++)
                {
                    var semana = lstAnioSemana[i];
                    var fechaSemana = DatetimeUtils.primerDiaSemana(semana.anio, semana.semana);
                    if(fechaSemana.DayOfWeek != DayOfWeek.Saturday)
                    {
                        fechaSemana = fechaSemana.Siguiente(DayOfWeek.Saturday);
                    }
                    var dataSaldoIni = lstSaldoIni.Where(w => w.anio == semana.anio && w.semana == semana.semana).Sum(s => s.strSaldoInicial.ParseDecimal(0));
                    var dataIni = esTodos ? dataSaldoIni : (busq.esFlujo ? dataSaldoIni : 0);
                    var dataFlujo = lstMontoFlujo.Where(w => w.anio == semana.anio && w.semana == semana.semana).Sum(s => s.flujoTotal);
                    switch(cpto.idCpto)
                    {
                        case 5:
                            data[i] = dataFlujo;
                            break;
                        case -2:
                            data[i] = dataIni;
                            break;
                        case -3:
                            data[i] = dataIni + dataFlujo;
                            break;
                        case -5:
                            var flujoAcumulado = dataIni + dataFlujo;
                            var efectivoRecibido = lstMontoFlujo.Where(w => w.anio == semana.anio && w.semana == semana.semana && w.idConceptoDir == 17).Sum(s => s.flujoTotal);
                            var flujoAcumOfiCentral = busq.esFlujo ? 0 : setGatosOficinaCentral(flujoAcumulado, semana.anio, semana.semana, efectivoRecibido);
                            data[i] = flujoAcumulado + flujoAcumOfiCentral;
                            break;
                        case -6:
                            var dataReserva = 0m;
                            var dataAcumOfiCentral = 0m;
                            var dataAcumulado = dataIni + dataFlujo;
                            var lstCCCierre = lstMontoCierre.Where(w => w.anio == semana.anio && w.semana == semana.semana);
                            var dataCierre = lstCCCierre.Sum(s => s.corte);
                            if(esTodos)
                            {
                                dataReserva = lstReserva.Where(w => w.fecha <= fechaSemana).Sum(s => s.monto);
                            }
                            if(esOfiCentral && !esTodos && !busq.esFlujo)
                            {
                                var dataEfectivoRecibido = lstMontoFlujo.Where(w => w.anio == semana.anio && w.semana == semana.semana && w.idConceptoDir == 17).Sum(s => s.flujoTotal);
                                dataAcumOfiCentral = setGatosOficinaCentral(dataAcumulado, semana.anio, semana.semana, dataEfectivoRecibido);
                            }
                            data[i] = dataAcumulado + dataReserva + dataCierre + dataAcumOfiCentral;
                            break;
                        default:
                            data[i] = lstMontoFlujo.Where(w => w.anio == semana.anio && w.semana == semana.semana && w.idConceptoDir == cpto.idCpto).Sum(s => s.flujoTotal + (busq.esFlujo ? s.strSaldoInicial.ParseDecimal() : 0));
                            break;
                    }
                }
                lst.Add(new MetaGraficoDTO()
                {
                    name = cpto.descripcion,
                    stack = cpto.descripcion,
                    visible = lstLabelVisible.Contains(cpto.idCpto),
                    data = data,
                    type = "line",
                    tooltip = new { valueDecimals = 2 }
                });
            });
            return lst;
        }
        List<tblC_FE_MovPol> asignarConceptoParaObraYOperativo(List<tblC_FE_MovPol> lst)
        {
            if(lstCC == null)
            {
                init();
            }
            var idCostoProyecto = 6;
            var idGastosOperativo = 7;
            var lstDivAdmin = new List<int>()
            {
                (int)TipoCCEnum.Administración,
                (int)TipoCCEnum.GastosFininacierosYOtros
            };
            lst.ForEach(gpo =>
            {
                if(gpo.idConceptoDir == idCostoProyecto || gpo.idConceptoDir == idGastosOperativo)
                {
                    var cc = lstCC.FirstOrDefault(c => c.cc == gpo.cc);
                    if(cc != null && lstDivAdmin.Contains(cc.bit_area.ParseInt()))
                    {
                        gpo.idConceptoDir = idGastosOperativo;
                    }
                    else
                    {
                        gpo.idConceptoDir = idCostoProyecto;
                    }
                }
            });
            return lst;
        }
        List<MovpolDTO> asignarConceptoParaObraYOperativo(List<MovpolDTO> lstMov, BusqFlujoEfectivoDetDTO busq)
        {
            var idCostoProyecto = 6;
            var idGastosOperativo = 7;
            var lstCostos = new List<int>()
            {
                idCostoProyecto,
                idGastosOperativo
            };
            var lstDivAdmin = new List<int>()
            {
                (int)TipoCCEnum.Administración,
                (int)TipoCCEnum.GastosFininacierosYOtros
            };
            var lstCCAdmin = lstCC.Where(w => lstDivAdmin.Contains(w.bit_area.ParseInt())).Select(s => s.cc).ToList();
            if(!lstCostos.Contains(busq.idConceptoDir))
            {
                return lstMov;
            }
            var lst = new List<MovpolDTO>();
            lstMov.ForEach(mov =>
            {
                if (mov.empresa == EmpresaEnum.Construplan)
                {
                    var cc = lstCC.FirstOrDefault(c => c.cc == mov.cc);
                    if (cc != null && lstDivAdmin.Contains(cc.bit_area.ParseInt()) && busq.idConceptoDir == idGastosOperativo)
                    {
                        lst.Add(mov);
                    }
                    if (cc != null && !lstDivAdmin.Contains(cc.bit_area.ParseInt()) && busq.idConceptoDir == idCostoProyecto)
                    {
                        lst.Add(mov);
                    }
                }
                else {
                    if (busq.idConceptoDir == idGastosOperativo)
                    {
                        lst.Add(mov);
                    }
                    if (busq.idConceptoDir == idCostoProyecto)
                    {
                        lst.Add(mov);
                    }
                }
            });
            return lst;
        }
        List<MovpolDTO> asignarConceptoParaObraYOperativo_Optimizado(List<MovpolDTO> lstMov, BusqFlujoEfectivoDetDTO busq)
        {
            var idCostoProyecto = 6;
            var idGastosOperativo = 7;
            var lstCostos = new List<int>()
            {
                idCostoProyecto,
                idGastosOperativo
            };
            var lstDivAdmin = new List<int>()
            {
                (int)TipoCCEnum.Administración,
                (int)TipoCCEnum.GastosFininacierosYOtros
            };
            var lstCCAdmin = lstCC.Where(w => lstDivAdmin.Contains(w.bit_area.ParseInt())).Select(s => s.cc).ToList();
            if (!lstCostos.Contains(busq.idConceptoDir))
            {
                return lstMov;
            }
            var lst = new List<MovpolDTO>();
            lstMov.ForEach(mov =>
            {
                var cc = lstCC.FirstOrDefault(c => c.cc == mov.cc);
                if (cc != null && lstDivAdmin.Contains(cc.bit_area.ParseInt()) && busq.idConceptoDir == idGastosOperativo)
                {
                    lst.Add(mov);
                }
                if (cc != null && !lstDivAdmin.Contains(cc.bit_area.ParseInt()) && busq.idConceptoDir == idCostoProyecto)
                {
                    lst.Add(mov);
                }
            });
            return lst;
        }
        /// <summary>
        /// Asigna tm a buscar conceptos operativos por tipoBuscquedaEnum
        /// </summary>
        /// <param name="busq">Busqueda</param>
        /// <returns>Buscqieda con RelConcepto</returns>
        BusqFlujoEfectivoDTO ValidaBusquedaOperativo(BusqFlujoEfectivoDTO busq)
        {
            init();
            #region Divisiones
            if(busq.lstCC.Contains("TODOS"))
            {
                busq.lstCC.AddRange(lstCC.Select(s => s.cc).ToList());
            }
            if(busq.lstCC.Contains("000"))
            {
                busq.lstCC.AddRange(lstCC.Where(w => w.bit_area.ParseInt() == (int)TipoCCEnum.Administración).Select(c => c.cc).ToList());
            }
            if(busq.lstCC.Contains("IED"))
            {
                busq.lstCC.AddRange(lstCC.Where(w => w.bit_area.ParseInt() == (int)TipoCCEnum.InversionEdificio).Select(c => c.cc).ToList());
            }
            if(busq.lstCC.Contains("IAB"))
            {
                busq.lstCC.AddRange(lstCC.Where(w => w.bit_area.ParseInt() == (int)TipoCCEnum.AlimentosYBebidas).Select(c => c.cc).ToList());
            }
            if(busq.lstCC.Contains("IAM"))
            {
                busq.lstCC.AddRange(lstCC.Where(w => w.bit_area.ParseInt() == (int)TipoCCEnum.Automotriz).Select(c => c.cc).ToList());
            }
            if(busq.lstCC.Contains("IEN"))
            {
                busq.lstCC.AddRange(lstCC.Where(w => w.bit_area.ParseInt() == (int)TipoCCEnum.Energía).Select(c => c.cc).ToList());
            }
            if(busq.lstCC.Contains("DCP"))
            {
                busq.lstCC.AddRange(lstCC.Where(w => w.bit_area.ParseInt() == (int)TipoCCEnum.CerroPelon).Select(c => c.cc).ToList());
            }
            if(busq.lstCC.Contains("PES"))
            {
                busq.lstCC.AddRange(lstCC.Where(w => w.bit_area.ParseInt() == (int)TipoCCEnum.ConstruccionPesada).Select(c => c.cc).ToList());
            }
            if(busq.lstCC.Contains("MIN"))
            {
                busq.lstCC.AddRange(lstCC.Where(w => w.bit_area.ParseInt() == (int)TipoCCEnum.Mineria).Select(c => c.cc).ToList());
            }
            if(busq.lstCC.Contains("COL"))
            {
                busq.lstCC.AddRange(lstCC.Where(w => w.bit_area.ParseInt() == (int)TipoCCEnum.Colorada).Select(c => c.cc).ToList());
            }
            if(busq.lstCC.Contains("NOB"))
            {
                busq.lstCC.AddRange(lstCC.Where(w => w.bit_area.ParseInt() == (int)TipoCCEnum.NocheBuena1y2).Select(c => c.cc).ToList());
            }
            if(busq.lstCC.Contains("AGU"))
            {
                busq.lstCC.AddRange(lstCC.Where(w => w.bit_area.ParseInt() == (int)TipoCCEnum.SanAgustin).Select(c => c.cc).ToList());
            }
            if(busq.lstCC.Contains("IGF"))
            {
                busq.lstCC.AddRange(lstCC.Where(w => w.bit_area.ParseInt() == (int)TipoCCEnum.GastosFininacierosYOtros).Select(c => c.cc).ToList());
            }
            #endregion
            #region iTipoMovimiento
            switch(busq.tipo)
            {
                case tipoBusqueda.Todos:
                    break;
                case tipoBusqueda.UnConcepto:
                    busq.lstTm = busq.lstTm.Where(itm => RelConcepto.Where(r => r.tm == itm).GroupBy(g => g.idConcepto).Count() == 1).ToList();
                    break;
                case tipoBusqueda.VariosConceptos:
                    busq.lstTm = busq.lstTm.Where(itm => RelConcepto.Where(r => r.tm == itm).GroupBy(g => g.idConcepto).Count() > 1).ToList();
                    break;
                case tipoBusqueda.SinConceptos:
                    busq.lstTm = busq.lstTm.Where(itm => RelConcepto.Where(r => r.tm == itm).GroupBy(g => g.idConcepto).Count() == 0).ToList();
                    break;
                case tipoBusqueda.FaltanteGuardar:
                    break;
                default:
                    break;
            }
            #endregion
            return busq;
        }
        /// <summary>
        /// Asigna tm a buscar conceptos directos por tipoBuscquedaEnum
        /// </summary>
        /// <param name="busq">Busqueda</param>
        /// <returns>Buscqieda con RelConceptoDir</returns>
        BusqFlujoEfectivoDTO ValidaBusquedaDirecto(BusqFlujoEfectivoDTO busq)
        {
            init();
            
            #region Divisiones
            if(busq.lstCC.Contains("TODOS"))
            {
                busq.lstCC.AddRange(lstCC.Select(s => s.cc).ToList());
            }
            if(busq.lstCC.Contains("000"))
            {
                busq.lstCC.AddRange(lstCC.Where(w => w.bit_area.ParseInt() == (int)TipoCCEnum.Administración).Select(c => c.cc).ToList());
            }
            if(busq.lstCC.Contains("IED"))
            {
                busq.lstCC.AddRange(lstCC.Where(w => w.bit_area.ParseInt() == (int)TipoCCEnum.InversionEdificio).Select(c => c.cc).ToList());
            }
            if(busq.lstCC.Contains("IAB"))
            {
                busq.lstCC.AddRange(lstCC.Where(w => w.bit_area.ParseInt() == (int)TipoCCEnum.AlimentosYBebidas).Select(c => c.cc).ToList());
            }
            if(busq.lstCC.Contains("IAM"))
            {
                busq.lstCC.AddRange(lstCC.Where(w => w.bit_area.ParseInt() == (int)TipoCCEnum.Automotriz).Select(c => c.cc).ToList());
            }
            if(busq.lstCC.Contains("IEN"))
            {
                busq.lstCC.AddRange(lstCC.Where(w => w.bit_area.ParseInt() == (int)TipoCCEnum.Energía).Select(c => c.cc).ToList());
            }
            if(busq.lstCC.Contains("DCP"))
            {
                busq.lstCC.AddRange(lstCC.Where(w => w.bit_area.ParseInt() == (int)TipoCCEnum.CerroPelon).Select(c => c.cc).ToList());
            }
            if(busq.lstCC.Contains("PES"))
            {
                busq.lstCC.AddRange(lstCC.Where(w => w.bit_area.ParseInt() == (int)TipoCCEnum.ConstruccionPesada).Select(c => c.cc).ToList());
            }
            if(busq.lstCC.Contains("MIN"))
            {
                busq.lstCC.AddRange(lstCC.Where(w => w.bit_area.ParseInt() == (int)TipoCCEnum.Mineria).Select(c => c.cc).ToList());
            }
            if(busq.lstCC.Contains("COL"))
            {
                busq.lstCC.AddRange(lstCC.Where(w => w.bit_area.ParseInt() == (int)TipoCCEnum.Colorada).Select(c => c.cc).ToList());
            }
            if(busq.lstCC.Contains("NOB"))
            {
                busq.lstCC.AddRange(lstCC.Where(w => w.bit_area.ParseInt() == (int)TipoCCEnum.NocheBuena1y2).Select(c => c.cc).ToList());
            }
            if(busq.lstCC.Contains("AGU"))
            {
                busq.lstCC.AddRange(lstCC.Where(w => w.bit_area.ParseInt() == (int)TipoCCEnum.SanAgustin).Select(c => c.cc).ToList());
            }
            if(busq.lstCC.Contains("IGF"))
            {
                busq.lstCC.AddRange(lstCC.Where(w => w.bit_area.ParseInt() == (int)TipoCCEnum.GastosFininacierosYOtros).Select(c => c.cc).ToList());
            }
            #endregion
            #region Areacuenta
            busq = flujoArrendadoraFS.setAreaCuentaPorCentroCostos(busq);
            #endregion
            #region iTipo Movimiento
            switch(busq.tipo)
            {
                case tipoBusqueda.Todos:
                    busq.lstTm = lstTm.Select(s => s.Value.ParseInt()).ToList();
                    break;
                case tipoBusqueda.UnConcepto:
                    busq.lstTm = busq.lstTm.Where(itm => RelConceptoDir.Where(r => r.tm == itm).GroupBy(g => g.idConceptoDir).Count() == 1).ToList();
                    break;
                case tipoBusqueda.VariosConceptos:
                    busq.lstTm = busq.lstTm.Where(itm => RelConceptoDir.Where(r => r.tm == itm).GroupBy(g => g.idConceptoDir).Count() > 1).ToList();
                    break;
                case tipoBusqueda.SinConceptos:
                    busq.lstTm = busq.lstTm.Where(itm => RelConceptoDir.Where(r => r.tm == itm).GroupBy(g => g.idConceptoDir).Count() == 0).ToList();
                    break;
                default:
                    break;
            }
            #endregion
            #region Permisos
            var esPermisoFlujoCierre = Session["esPermisoFlujoCierre"];
            if(esPermisoFlujoCierre == null || !(bool)esPermisoFlujoCierre)
            {
                var lstPermiso = usuarioFS.getLstAccionesActual();
                //Session["esPermisoFlujoCierre"] = lstPermiso.Any(p => p.id == 3007);
                //Session["esPermisGastosOficiana"] = lstPermiso.Any(p => p.id == 3021);
                Session["esPermisoFlujoCierre"] = true;
                Session["esPermisGastosOficiana"] = true;
            }
            #endregion
            return busq;
        }
        /// <summary>
        /// Compara movimientos de polizas de enkontrol contra las de sigoplan
        /// </summary>
        /// <param name="lst">Movimientos de sigoplan</param>
        /// <param name="mov">Moviento de enkontrol</param>
        /// <returns>Movimiento de poliza</returns>
        tblC_FE_MovPol getMovimientoFromSigoplan(List<tblC_FE_MovPol> lst, MovpolDTO mov)
        {
            if(lst.Count > 0 && lst.Any(m => m.year == mov.year && m.mes == mov.mes && m.tp == mov.tp && m.poliza == mov.poliza && m.linea == mov.linea))
            {
                return lst.FirstOrDefault(m => m.year == mov.year && m.mes == mov.mes && m.tp == mov.tp && m.poliza == mov.poliza && m.linea == mov.linea);
            }
            else
            {
                return new tblC_FE_MovPol() { itm = mov.itm, itmOri = mov.itm };
            }
        }
        /// <summary>
        /// Genera un folio con las keys de la poliza
        /// </summary>
        /// <param name="mov">moviviemot poliza</param>
        /// <returns>Folio unico</returns>
        string getFolio(MovpolDTO mov)
        {
            return string.Format("{0}-{1}-{2}-{3}-{4}", mov.year, mov.mes, mov.tp, mov.poliza, mov.linea);
        }
        string getFolio(tblC_FE_MovPol mov)
        {
            return string.Format("{0}-{1}-{2}-{3}-{4}", mov.year, mov.mes, mov.tp, mov.poliza, mov.linea);
        }
        string getCuenta(MovpolDTO mov)
        {
            return string.Format("{0}-{1}-{2}", mov.cta, mov.scta, mov.sscta);
        }
        string getCuenta(tblC_FE_MovPol mov)
        {
            return string.Format("{0}-{1}-{2}", mov.cta, mov.scta, mov.sscta);
        }
        string getCuentaDescripcion(MovpolDTO mov)
        {
            return string.Format("{0}-{1}-{2} {3}", mov.cta, mov.scta, mov.sscta, catCta.FirstOrDefault(cta => cta.cta == mov.cta && cta.scta == mov.scta && cta.sscta == mov.sscta).descripcion);
        }
        string getCuenta(int cta, int scta, int sscta, EmpresaEnum empresa)
        {
            var cuenta = string.Format("{0}-{1}-{2} ", cta, scta, sscta);
            switch(empresa)
            {
                case EmpresaEnum.Construplan:
                    cuenta += catCta.FirstOrDefault(cat => cat.cta == cta && cat.scta == scta && cat.sscta == sscta).descripcion;
                    break;
                case EmpresaEnum.Arrendadora:
                    cuenta += catCtaArrend.FirstOrDefault(cat => cat.cta == cta && cat.scta == scta && cat.sscta == sscta).descripcion;
                    break;
                default:
                    break;
            }
            return cuenta;
        }
        string getCuentaDescripcion(tblC_FE_MovPol mov)
        {
            return string.Format("{0}-{1}-{2} {3}", mov.cta, mov.scta, mov.sscta, catCta.FirstOrDefault(cta => cta.cta == mov.cta && cta.scta == mov.scta && cta.sscta == mov.sscta).descripcion);
        }
        string getTipoMovimientoDescripcion(MovpolDTO mov)
        {
            return lstTm.FirstOrDefault(m => m.Value.ParseInt() == mov.itm).Text;
        }
        string getTipoMovimientoDescripcion(tblC_FE_MovPol mov)
        {
            return lstTm.FirstOrDefault(m => m.Value.ParseInt() == mov.itm).Text;
        }
        string getProveedorDescripcion(MovpolDTO mov)
        {
            return mov.numpro == null || mov.numpro == 0 ? string.Empty : string.Format("{0}-{1}", mov.numpro, lstProv.FirstOrDefault(p => p.numpro == mov.numpro).nombre);
        }
        string getProveedorDescripcion(int numpro, EmpresaEnum empresa)
        {
            if(numpro == 0)
            {
                return string.Empty;
            }
            string proveedor = numpro.ToString() + "-";
            switch(empresa)
            {
                case EmpresaEnum.Construplan:
                    {
                        var _proveedor = lstProv.FirstOrDefault(p => p.numpro == numpro);
                        proveedor += _proveedor == null ? "PERU" : lstProv.FirstOrDefault(p => p.numpro == numpro).nombre;
                    }
                    break;
                case EmpresaEnum.Arrendadora:
                    {
                        proveedor += lstProvArren.FirstOrDefault(p => p.numpro == numpro).nombre;
                    }
                    break;
                default:
                    break;
            }
            return proveedor;
        }
        string getProveedorDescripcion(tblC_FE_MovPol mov)
        {
            return mov.numpro == 0 ? string.Empty : string.Format("{0}-{1}", mov.numpro, lstProv.FirstOrDefault(p => p.numpro == mov.numpro).nombre);
        }
        string getClienteDecripcion(int numcte, EmpresaEnum empresa)
        {
            if(numcte == 0)
            {
                return string.Empty;
            }
            var cliente = numcte.ToString() + "-";
            switch(empresa)
            {
                case EmpresaEnum.Construplan:
                    {
                        var info = lstCliente.FirstOrDefault(c => c.Value == numcte.ToString());
                        if (info != null)
                        {
                            cliente = info.Text;
                        }
                        //cliente += lstCliente.FirstOrDefault(c => c.Value == numcte.ToString());
                    }
                    break;
                case EmpresaEnum.Arrendadora:
                    {
                        var info = lstClienteArren.FirstOrDefault(c => c.Value == numcte.ToString());
                        if (info != null)
                        {
                            cliente = info.Text;
                        }
                        //cliente += lstClienteArren.FirstOrDefault(c => c.Value == numcte.ToString());
                    }
                    break;
                default:
                    break;
            }
            return cliente;
        }
        string getObra(string obra, EmpresaEnum empresa)
        {
            switch(empresa)
            {
                case EmpresaEnum.Construplan:
                    if(lstCC.Any(o => o.cc == obra))
                    {
                        obra += "-" + lstCC.FirstOrDefault(o => o.cc == obra).descripcion;
                    }
                    break;
                case EmpresaEnum.Arrendadora:
                    if(lstAC.Any(o => o.Value == obra))
                    {
                        obra = lstAC.FirstOrDefault(o => o.Value == obra).Text;
                    }
                    break;
                default:
                    break;
            }
            return obra;
        }
        DateTime getUltimaFechaCorte()
        {
            var lst = flujoFS.getPlaneacion(false).Where(w => w.flujoTotal != 0 && w.anio >= 2020 && w.idConceptoDir <= 20).ToList();
            var año = lst.Max(p => p.anio);
            var maxSemana = lst.Max(p => p.semana);
            var maxCorte = Infrastructure.Utils.DatetimeUtils.primerDiaSemana(año, maxSemana);
            if(maxCorte.DayOfWeek != DayOfWeek.Saturday)
            {
                maxCorte = maxCorte.Siguiente(DayOfWeek.Saturday);
            }
            return maxCorte;
        }
        #endregion
        #region Catalogo de Planeacion
        public ViewResult PlaneacionDetalle()
        {
            var empresa = vSesiones.sesionEmpresaActual;
            if(empresa != (int)EmpresaEnum.Arrendadora)
            {
                return View();
            }
            else
            {
                return flujoEfectivoCtrl.PlaneacionDetalle();
            }
        }

        [HttpPost]
        public ActionResult guardarDetalle()
        {
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerInfoConceptos(string centroCostos, int semana, int anio)
        {
            return Json(flujoFS.ObtenerInfoConceptos(centroCostos, semana, anio), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult getDetallePlaneacion(int concepto, string cc, int semana, int anio, int tipo)
        {
            return Json(flujoFS.getDetallePlaneacion(concepto, cc, semana, anio, tipo), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult getGastosProv(string fechaInicio, string fechaFin, string cc, int semana, int anio)
        {
            DateTime pFechaInicio = Convert.ToDateTime(fechaInicio);
            DateTime pFechaFin = Convert.ToDateTime(fechaFin);
            return Json(flujoFS.getGastosProyecto(pFechaInicio, pFechaFin, cc, semana, anio), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult getGastosOperativos(string fechaInicio, string fechaFin, string cc, int semana, int anio)
        {
            DateTime pFechaInicio = Convert.ToDateTime(fechaInicio);
            DateTime pFechaFin = Convert.ToDateTime(fechaFin);
            return Json(flujoFS.getGastosOperativos(pFechaInicio, pFechaFin, cc, semana, anio), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult getEfectivoRecibido(string fechaInicio, string fechaFin, string cc, int semana, int anio)
        {
            DateTime pFechaInicio = Convert.ToDateTime(fechaInicio);
            DateTime pFechaFin = Convert.ToDateTime(fechaFin);
            return Json(flujoFS.getEfectivoRecibido(pFechaInicio, pFechaFin, cc, semana, anio), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult saveOrUpdateDetalle(List<tblC_FED_PlaneacionDet> nuevo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var esGuardado = nuevo.Count > 0 && nuevo.All(det => det.concepto > 0 && det.cc != null && det.cc.Length > 0 && det.monto != 0 && det.descripcion.Length > 0);
                if(esGuardado)
                {
                    return Json(flujoFS.saveOrUpdateDetalle(nuevo), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var o_O = new Exception();
                    throw o_O;
                }
            }
            catch(Exception O_O)
            {
                result.Add(MESSAGE, O_O.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult saveDetallesMasivos(List<tblC_FED_PlaneacionDet> lista, int idConceptoDir, int anio, int semana, string cc)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var esGuardado = lista.Count > 0 && lista.All(det => det.concepto > 0 && det.cc != null && det.cc.Length > 0 && det.monto != 0 && det.descripcion.Length > 0);
                if(esGuardado)
                {
                    return Json(flujoFS.saveDetallesMasivos(lista, idConceptoDir, anio, semana, cc), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    throw new Exception();
                }

            }
            catch(Exception O_O)
            {
                result.Add(MESSAGE, O_O.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult getDescripcionesPlaneacion(int conceptoID, string centroCostos, int semana, int anio)
        {
            return Json(flujoFS.getDescripcionesPlaneacion(conceptoID, centroCostos, semana, anio), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult getDetalleDescripcionPlaneacion(PlaneacionDetDTO planeacionDetDTO, string centroCostos)
        {
            return Json(flujoFS.getDetalleDescripcionPlaneacion(planeacionDetDTO, centroCostos), JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCargaNomina(List<PeriodosNominaDTO> lstPeriodo, string cc, int semana, int anio)
        {
            return Json(flujoFS.getCargaNomina(lstPeriodo, cc, semana, anio), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult getCadenasProductivas(string fechaInicio, string fechaFin, string cc, int semana, int anio)
        {
            DateTime pFechaInicio = Convert.ToDateTime(fechaInicio);
            DateTime pFechaFin = Convert.ToDateTime(fechaFin);
            return Json(flujoFS.getCadenasProductivas(pFechaInicio, pFechaFin, cc, semana, anio), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult getDetallesPlaneacionPPal(int conceptoID, string centroCostos, int semana, int anio, bool esConciliado)
        {
            return Json(flujoFS.getDetallesPlaneacionPPal(conceptoID, centroCostos, semana, anio, esConciliado), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult getSubNivelDetallePlaneacion(int conceptoID, string cc, int semana, int anio, int tipo)
        {
            return Json(flujoFS.getSubNivelDetallePlaneacion(conceptoID, cc, semana, anio, tipo), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult getSubDetalle(string cc, int semana, int anio, int concepto, int numProv, int numcte)
        {
            return Json(flujoFS.getSubDetalle(cc, semana, anio, concepto, numProv, numcte), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}