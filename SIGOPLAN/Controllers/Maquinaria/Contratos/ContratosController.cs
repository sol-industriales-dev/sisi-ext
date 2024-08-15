using Core.DAO.Administracion.DocumentosXPagar;
using Core.DAO.Contabilidad.Cuenta;
using Core.DAO.Enkontrol.General.CC;
using Core.DAO.Principal.Archivos;
using Core.DAO.Principal.Bitacoras;
using Core.DTO;
using Core.DTO.Administracion.DocumentosXPagar;
using Core.DTO.Contabilidad.Bancos;
using Core.DTO.Contabilidad.DocumentosXPagar;
using Core.DTO.Contabilidad.DocumentosXPagar.PQ;
using Core.DTO.Contabilidad.DocumentosXPagar.Reporte_Adeudo;
using Core.DTO.Contabilidad.Poliza;
using Core.DTO.Enkontrol.Tablas.Cuenta;
using Core.DTO.Utils.DataTable;
using Core.Entity.Administrativo.Contabilidad.Cheque;
using Core.Entity.Administrativo.DocumentosXPagar;
using Core.Entity.Administrativo.DocumentosXPagar.PQ;
using Core.Enum.Administracion.DocumentosXPagar;
using Core.Enum.Principal.Bitacoras;
using Data.Factory.Administracion.DocumentosXPagar;
using Data.Factory.Contabilidad.Cuenta;
using Data.Factory.Enkontrol.General.CC;
using Data.Factory.Principal.Archivos;
using Data.Factory.Principal.Bitacora;
using Infrastructure.DTO;
using Infrastructure.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Maquinaria.Contratos
{
    public class ContratosController : BaseController
    {
        IContratosDAO dXPFS;
        IDirArchivosDAO archivoFs;
        ICCDAO ccFS;
        ICuentaDAO cuentaFS;

        Respuesta respuesta = new Respuesta();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            dXPFS = new DocumentosXPagarFactoryServices().GetDocumentosXPagarServices();
            archivoFs = new ArchivoFactoryServices().getArchivo();
            ccFS = new CCFactoryService().getCCService();
            cuentaFS = new CuentaFactoryService().GetCuentaEkService();

            base.OnActionExecuting(filterContext);
        }

          #region Vistas
        public ActionResult DocumentosPorPagar()
        {
            ViewBag.EmpresaActual = vSesiones.sesionEmpresaActual;
            return View();

        }

        public ActionResult ProgramacionPagos()
        {
            ViewBag.EmpresaActual = vSesiones.sesionEmpresaActual;
            return View();
        }

        public ActionResult ReporteDocumentosPorPagar()
        {
            ViewBag.EmpresaActual = vSesiones.sesionEmpresaActual;

            return View();
        }

        public ActionResult ReporteAdeudosDetalle()
        {
            ViewBag.EmpresaActual = vSesiones.sesionEmpresaActual;

            return View();
        }

        public ActionResult GestionPQ()
        {
            return View();
        }

        public ActionResult ReporteSaldoPendienteProyecto()
        {
            return View();
        }

        public ActionResult CatDivisiones()
        {
            return View();
        }

        public ActionResult Divisiones_Proyectos()
        {
            return View();
        }

        public ActionResult CedulaMensual()
        {
            return View();
        }

        public ActionResult ReporteInteresesPagados()
        {
            return View();
        }
        #endregion

        #region CedulaMensual
        [HttpGet]
        public JsonResult GetVersion()
        {
            var version = 1;
            return Json(version, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCedula(DateTime fechaCorte)
        {
            var resultado = dXPFS.GetCedula(fechaCorte);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost]
        public JsonResult ObtenerContratos(FiltroContratosDTO filtro)
        {
            return Json(dXPFS.ObtenerContratos(filtro), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarFechaNuevoPeriodo(int contratoID, DateTime nuevaFecha, int parcialidad)
        {
            return Json(dXPFS.GuardarFechaNuevoPeriodo(contratoID, nuevaFecha, parcialidad), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarContrato()
        {
            var r = new Respuesta();

            try
            {
                var contrato = new tblAF_DxP_Contrato();

                contrato.Id = Convert.ToInt32(Request.Form["idContrato"]);
                contrato.Folio = Request.Form["Folio"];
                contrato.Descripcion = Request.Form["Descripcion"];
                contrato.InstitucionId = Convert.ToInt32(Request.Form["InstitucionId"]);
                contrato.Plazo = Convert.ToInt32(Request.Form["Plazo"]);
                contrato.FechaInicio = Convert.ToDateTime(Request.Form["FechaInicio"]);
                contrato.Credito = Convert.ToDecimal(Request.Form["Credito"]);
                contrato.AmortizacionCapital = Convert.ToDecimal(Request.Form["AmortizacionCapital"]);
                contrato.TasaInteres = Convert.ToDecimal(Request.Form["TasaInteres"]);
                contrato.InteresMoratorio = Convert.ToDecimal(Request.Form["InteresMoratorio"]);
                contrato.Domiciliado = Convert.ToBoolean(Request.Form["Domiciliado"]);
                contrato.TipoCambio = Convert.ToDecimal(Request.Form["TipoCambio"]);
                contrato.montoOpcioncompra = Convert.ToDecimal(Request.Form["montoOpcioncompra"]);
                contrato.monedaContrato = Convert.ToInt32(Request.Form["monedaContrato"]);
                contrato.penaConvencional = Request.Form["penaConvencional"].ToString();
                contrato.rfc = Request.Form["rfc"].ToString();
                contrato.nombreCorto = Request.Form["nomCorto"].ToString();
                contrato.cta = Convert.ToInt32(Request.Form["cta"].ToString());
                contrato.scta = Convert.ToInt32(Request.Form["scta"].ToString());
                contrato.sscta = Convert.ToInt32(Request.Form["sscta"].ToString());
                contrato.digito = Convert.ToInt32(Request.Form["digito"].ToString());
                if (Request.Form["sctaIA"].ToString() != "undefined")
                {
                    contrato.ctaIA = Convert.ToInt32(Request.Form["ctaIA"].ToString());
                    contrato.sctaIA = Convert.ToInt32(Request.Form["sctaIA"].ToString());
                    contrato.ssctaIA = Convert.ToInt32(Request.Form["ssctaIA"].ToString());
                    contrato.digitoIA = Convert.ToInt32(Request.Form["digitoIA"].ToString());   
                }
                contrato.empresa = Convert.ToInt32(Request.Form["empresa"].ToString());
                contrato.tasaFija = Convert.ToBoolean(Request.Form["tasaFija"].ToString());
                contrato.aplicaInteres = Convert.ToBoolean(Request.Form["aplicaInteres"].ToString());
                contrato.aplicaContratoInteres = Convert.ToBoolean(Request.Form["aplicaContratoInteres"].ToString());
                contrato.arrendamientoPuro = Convert.ToBoolean(Request.Form["arrendamientoPuro"].ToString());

                var _interino = Request.Form["pagoInterino"].ToString();
                var _interino2 = Request.Form["pagoInterino2"].ToString();
                var _garantia = Request.Form["depGarantia"].ToString();

                if (!string.IsNullOrEmpty(_interino))
                {
                    contrato.PagoInterino = Convert.ToDecimal(Request.Form["pagoInterino"].ToString());

                    if (!_interino2.Equals("0"))
                    {
                        contrato.PagoInterino2 = Convert.ToDecimal(Request.Form["pagoInterino2"].ToString());
                        contrato.DepGarantia = Convert.ToDecimal(Request.Form["depGarantia"].ToString());
                    }
                }

                bool tasaFija = Convert.ToBoolean(Request.Form["tasaFija"].ToString());
                if (!string.IsNullOrEmpty(Request.Form["fechaFirma"].ToString()))
                    contrato.fechaFirma = Convert.ToDateTime(Request.Form["fechaFirma"]);

                var economicos = JsonConvert.DeserializeObject<List<AgregarMaquinaDTO>>(Request.Form["economicos"]);

                HttpPostedFileBase archivoContrato = Request.Files["ArchivoContrato"];

                var folder = "DOCUMENTOS_POR_PAGAR";

                var fechaArchivo = DateTime.Now.ToString("yyyy-MM-ddTHHmmssfff");
                var ruta = archivoFs.getUrlDelServidor(1019) + folder + @"\";

                if (Request.Files["ArchivoContrato"] != null)
                {
                    var nombreArchivo = archivoContrato.FileName;
                    var nombreArchivoSinExtension = System.IO.Path.GetFileNameWithoutExtension(nombreArchivo);
                    var extension = System.IO.Path.GetExtension(nombreArchivo);

#if DEBUG
                    var directorio = new DirectoryInfo(@"C:\fail\" + folder + @"\"); //local
#else
                    var directorio = new DirectoryInfo(ruta);
#endif
                    if (directorio.Exists == false)
                    {
                        directorio.Create();
                    }
                    var pathCompleto = System.IO.Path.Combine(directorio.ToString(), nombreArchivoSinExtension + "_" + fechaArchivo + extension);

                    archivoContrato.SaveAs(pathCompleto);
                    contrato.FileContrato = pathCompleto;
                }

                HttpPostedFileBase archivoPagare = null;

                if (Request.Files["ArchivoPagare"] != null)
                {
                    archivoPagare = Request.Files["ArchivoPagare"];
                    var nombreArchivo = archivoPagare.FileName;
                    var nombreArchivoSinExtension = System.IO.Path.GetFileNameWithoutExtension(nombreArchivo);

#if DEBUG
                    var directorio = new DirectoryInfo(@"C:\fail\" + folder + @"\"); //local
#else
                    var directorio = new DirectoryInfo(ruta);
#endif

                    var extension = System.IO.Path.GetExtension(nombreArchivo);
                    fechaArchivo = DateTime.Now.ToString("yyyy-mm-ddTHHmmssfff") + "0";

                    var pathCompleto = System.IO.Path.Combine(ruta, nombreArchivoSinExtension + "_" + fechaArchivo + extension);
                    archivoPagare.SaveAs(pathCompleto);
                    contrato.FilePagare = pathCompleto;
                }

                contrato.FechaVencimientoTipoId = Convert.ToInt32(Request.Form["FechaVencimientoTipo"]);
                switch (contrato.FechaVencimientoTipoId)
                {
                    case 1:
                        DateTime finMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);

                        contrato.FechaVencimiento = finMes;
                        break;
                    case 2:
                        contrato.FechaVencimiento = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 15);
                        break;
                    case 3:
                        contrato.FechaVencimiento = Convert.ToDateTime(Request.Form["FechaVencimiento"]);
                        break;
                    default:
                        break;
                }
                r = dXPFS.GuardarContrato(contrato, economicos, tasaFija);

            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return Json(r);
        }

        [HttpPost]
        public JsonResult GuardarDeudas(List<tblAF_DxP_Deuda> objDeudas, tblC_sc_polizas poliza, List<tblC_sc_movpol> movPolList)
        {
            return Json(dXPFS.GuardarDeudas(objDeudas, poliza, movPolList), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarMaquina(AgregarMaquinaDTO maquina)
        {
            return Json(dXPFS.AgregarMaquina(maquina));
        }

        [HttpGet]
        public JsonResult ObtenerMaquinas(int idContrato)
        {
            return Json(dXPFS.ObtenerMaquinas(idContrato), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerDesgloseGeneral(int idContrato)
        {
            return Json(dXPFS.ObtenerDesgloseGeneral(idContrato), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public Dictionary<string, object> autoComplCtas(string term) //TODO
        {
            var Resultado = new Dictionary<string, object>();
            try
            {
                term = term.Trim();
                term = term.Replace(";", string.Empty);
                term = term.Replace("-1", string.Empty);
                var items = dXPFS.autoComplCatCtas(term); 
                Resultado.Add(ITEMS, items);
                Resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                Resultado.Add(SUCCESS, false);
                Resultado.Add(MESSAGE, "Ocurrió un error, al momento de cargar la información de proveedores en enkontrol.");
            }
            return Resultado;
        }
        #region SE OBTIENE LOS DATOS DE ctaIA
        public ActionResult autoCompleteCtasIA(string term)
        {
            var data = dXPFS.autoCompleteCtasIA(term).ToList().Take(10);
            var dataFiltrados = data.Select(x => new { 
                id = x.cta + "-" + x.scta + "-" + x.sscta + "-" + x.digito,
                label = x.cta + "-" + x.scta + "-" + x.sscta + "-" + x.digito 
            });
            return Json(dataFiltrados, JsonRequestBehavior.AllowGet);
        }
        #endregion
        [HttpGet]
        public JsonResult ObtenerDesglosePorMaquina(int idContratoMaquina)
        {
            return Json(dXPFS.ObtenerDesglosePorMaquina(idContratoMaquina), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerInstituciones()
        {
            return Json(dXPFS.ObtenerInstituciones());
        }

        [HttpPost]
        public JsonResult ObtenerMaquinas()
        {
            return Json(dXPFS.ObtenerMaquinas());
        }

        [HttpGet]
        public JsonResult ObtenerContratoByID(int idContrato)
        {
            return Json(dXPFS.ObtenerContratoByID(idContrato), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerPagos(int contratoDetID, int parcialidad)
        {
            return Json(dXPFS.ObtenerPagos(contratoDetID, parcialidad), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveOrupdatepagos(tblAF_DxP_Pago dxpPago, List<tblAF_DxP_PagoMaquina> dxpPagoMaquina, List<tblAF_DxP_ContratoMaquinaDetalle> dxpContratoMaquinaDetalle)
        {
            dxpPago.UsuarioCreacionId = getUsuario().id;
            dxpPago.UsuarioModificacionId = getUsuario().id;
            return Json(dXPFS.SaveOrupdatepagos(dxpPago, dxpPagoMaquina, dxpContratoMaquinaDetalle));
        }

        [HttpGet]
        public ActionResult ObtenerContratosNotificaciones()
        {
            return Json(dXPFS.ObtenerContratosNotificaciones(), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult CargarPropuesta(string pInicio, string pFinal, int pEstatus, int institucion)
        {
            //string[] arrFechaInicio = pInicio.Split('/');
            //string fechaInicio = string.Format("{0}/{1}/{2}", arrFechaInicio[1], arrFechaInicio[0], arrFechaInicio[2]);

            //string[] arrFechaFin = pInicio.Split('/');
            //string fechaFin = string.Format("{0}/{1}/{2}", arrFechaFin[1], arrFechaFin[0], arrFechaFin[2]);

            DateTime inicioP = Convert.ToDateTime(pInicio/* fechaInicio*/);
            DateTime finP = Convert.ToDateTime(pFinal /*fechaFin*/);
            return Json(dXPFS.CargarPropuesta(inicioP, finP, pEstatus, institucion), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GuardarProgramacionPagos(List<tblAF_DxP_ProgramacionPagos> obj)
        {
            return Json(dXPFS.GuardarProgramacion(obj), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]

        public ActionResult loadPropuestas(int? idInstitucion, int empresa, int moneda)
        {
            return Json(dXPFS.loadPropuestas(idInstitucion, empresa, moneda), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CargarContrato(int contratoId, int parcialidad, DateTime fechaPol)
        {
            return Json(dXPFS.CargarContrato(contratoId, parcialidad, fechaPol), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult guardarPoliza(tblC_sc_polizas poliza, List<tblC_sc_movpol> movpol, List<listaContrato> contrato, decimal tipoCambio)
        {
            return Json(dXPFS.guardarPoliza(poliza, movpol, contrato, tipoCambio), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetProveedores()
        {
            return Json(dXPFS.GetProveedores(), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetTipoCambioFecha(string fecha)
        {
            DateTime fechaF = Convert.ToDateTime(fecha);
            return Json(dXPFS.GetTipoCambioFecha(fechaF), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult guardarInstitucion(string descripcion)
        {
            return Json(dXPFS.guardarInstitucion(descripcion), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult LoadProgramacionPagos(string pInicio, string pFinal, int pEstatus, List<int> institucion, int empresa, int moneda)
        {
            string[] arrFechaInicio = pInicio.Split('/');
            string fechaInicio = string.Format("{0}/{1}/{2}", arrFechaInicio[2], arrFechaInicio[1], arrFechaInicio[0]);

            string[] arrFechaFin = pFinal.Split('/');
            string fechaFin = string.Format("{0}/{1}/{2}", arrFechaFin[2], arrFechaFin[1], arrFechaFin[0]);

            DateTime inicioP = Convert.ToDateTime(fechaInicio);
            DateTime finP = Convert.ToDateTime(fechaFin);
            return Json(dXPFS.LoadProgramacionPagos(inicioP, finP, pEstatus, institucion, empresa, moneda), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetInfoLiquidar(bool liquidar, int contratoId, int parcialidad)
        {
            return Json(dXPFS.GetInfoLiquidar(liquidar, contratoId, parcialidad), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CargarDetallePago(int parcialidad, int contratoId)
        {

            return Json(dXPFS.CargarDetallePago(parcialidad, contratoId), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult LoadContratosProgramados(string pInicio, string pFinal, int empresa, string cc)
        {
            DateTime pFechaInicio = Convert.ToDateTime(pInicio);
            DateTime pFechaFin = Convert.ToDateTime(pFinal);
            return Json(dXPFS.LoadContratosProgramados(pFechaInicio, pFechaFin, cc), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult LoadContratosProgramadosCplan(string pInicio, string pFinal, int empresa, string cc)
        {
            DateTime pFechaInicio = Convert.ToDateTime(pInicio);
            DateTime pFechaFin = Convert.ToDateTime(pFinal);
            return Json(dXPFS.LoadContratosProgramadosCplan(pFechaInicio, pFechaFin, cc), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult getRptAdeudosGeneral(List<int> tipoMoneda, List<int> anio, List<int> instituciones, bool tipoArrendamiento)
        {

            var result = dXPFS.getRptAdeudosGeneral(tipoMoneda, anio, instituciones, tipoArrendamiento);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getRptAdeudosDetalle(int tipoMoneda, List<int> instituciones, string fechaFin, int tipo, List<bool> tipoArre)
        {
            if (tipoArre == null)
            {
                tipoArre = new List<bool>();
            }

            DateTime fecha = Convert.ToDateTime(fechaFin);
            var result = dXPFS.getRptAdeudosDetalle(tipoMoneda, instituciones, fecha, tipo, tipoArre);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public FileResult getFileDownload()
        {
            int id = Convert.ToInt32(Request.QueryString["id"]);

            var Archivo = dXPFS.getArchivoDownLoad(id);
            var nombre = Archivo.Text.Split('\\');
            return File(Archivo.Text, "multipart/form-data", nombre.Last());
        }

        public ActionResult getPolizaByFecha(string fechaP)
        {
            DateTime fecha = Convert.ToDateTime(fechaP);
            var result = dXPFS.getPolizaByFecha(fecha);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult comboCbosData()
        {
            var result = dXPFS.comboCbosData();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActualizarContratos(List<tblAF_DxP_ProgramacionPagos> arrayProgramacionID)
        {
            return Json(dXPFS.ActualizarContratos(arrayProgramacionID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadCtas()
        {
            return Json(dXPFS.LoadCtas(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult setRptAdeudosGeneral(List<MesesAdeudo> rptPesos, List<MesesAdeudo> rptDlls)
        {

            if (rptPesos == null)
            {
                rptPesos = new List<MesesAdeudo>();
            }
            if (rptDlls == null)
            {
                rptDlls = new List<MesesAdeudo>();
            }
            var listaRps = rptDlls != null ? rptPesos.Union(rptDlls) : rptPesos;
            Session["rptGeneralAdeudo"] = listaRps;
            var r = new Respuesta();
            r.Success = true;
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TerminarContrato(int contratoID)
        {
            return Json(dXPFS.TerminarContrato(contratoID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateContratosDet(int contratoID)
        {
            return Json(dXPFS.UpdateContratosDet(contratoID), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateContratoArchivo()
        {
            HttpPostedFileBase archivoContrato = Request.Files["ArchivoContrato"];
            string fileContrato = "";
            var folder = "DOCUMENTOS_POR_PAGAR";

            int contratoID = Convert.ToInt32(Request.Form["idContrato"]);

            var fechaArchivo = DateTime.Now.ToString("yyyy-MM-ddTHHmmssfff");
            var ruta = archivoFs.getUrlDelServidor(1019) + folder + @"\";

            if (Request.Files["ArchivoContrato"] != null)
            {
                var nombreArchivo = archivoContrato.FileName;
                var nombreArchivoSinExtension = System.IO.Path.GetFileNameWithoutExtension(nombreArchivo);
                var extension = System.IO.Path.GetExtension(nombreArchivo);

                var directorio = new DirectoryInfo(ruta);
#if DEBUG
                directorio = new DirectoryInfo(@"C:\fail\" + folder + @"\"); //local
#endif
                if (directorio.Exists == false)
                {
                    directorio.Create();
                }
                var pathCompleto = System.IO.Path.Combine(directorio.ToString(), nombreArchivoSinExtension + "_" + fechaArchivo + extension);

                archivoContrato.SaveAs(pathCompleto);
                fileContrato = pathCompleto;
            }

            return Json(dXPFS.UpdateContratoArchivo(contratoID, fileContrato), JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadCtasServerSide(DataTablesParam param)
        {
            dtLoadCtaServerDTO info = dXPFS.LoadCtasServerSide();
            int totalCount = 0;
            int pageNo = 1;
            List<ctaDTO> List = new List<ctaDTO>();
            if (param.iDisplayStart >= param.iDisplayLength)
            {
                pageNo = (param.iDisplayStart / param.iDisplayLength) + 1;
            }

            totalCount = info.data.Count();

            List = info.data.Where(r => param.sSearch != null ?
                ((r.cta.ToString() + "-" + r.scta.ToString() + "-" + r.sscta.ToString()).Contains(param.sSearch) ||
               !string.IsNullOrEmpty(r.descripcion) ? r.descripcion.Contains(param.sSearch) : true
                )
                : true)
                .OrderBy(x => x.cta)
                .Skip((pageNo - 1) * param.iDisplayLength)
                .Take(param.iDisplayLength)
                .Select(x => x).ToList();
            return Json(new
            {
                aaData = List,
                sEcho = param.sEcho,
                iTotalDisplayRecords = totalCount,
                iTotalRecords = totalCount

            }, JsonRequestBehavior.AllowGet);
        }

        #region REPORTE SALDO PENDIENTE POR PROYECTO
        [HttpPost]
        public JsonResult ObtenerCboCC(List<int> lstDivisionID)
        {
            return Json(dXPFS.ObtenerCboCC(lstDivisionID));
        }

        [HttpPost]
        public JsonResult ObtenerCboDivisiones()
        {
            return Json(dXPFS.ObtenerCboDivisiones());
        }

        //public ActionResult ObtenerListadoDivisiones(adeudosDTO objDivision)
        //{
        //    var result = new Dictionary<string, object>();
        //    try
        //    {
        //        if (objDivision.lstCC != null)
        //        {
        //            var lstDivisiones = dXPFS.ObtenerListadoDivisiones(objDivision);
        //            result.Add("lstDivisiones", lstDivisiones.OrderByDescending(x => x.saldoPendiente));
        //            result.Add(SUCCESS, true);
        //        }
        //        else
        //        {
        //            result.Add(SUCCESS, false);
        //            result.Add(MESSAGE, "Es necesario seleccionar un CC.");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        result.Add(MESSAGE, e.Message);
        //        result.Add(SUCCESS, false);
        //    }
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult ObtenerListadoDivisiones(adeudosDTO objDivision)
        {
            var result = dXPFS.ObtenerListadoDivisiones(objDivision);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CATALOGO DIVISIONES
        public ActionResult GetDivisiones()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstCatDivisiones = dXPFS.GetDivisiones();
                result.Add("lstCatDivisiones", lstCatDivisiones);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarDivisiones(tblAF_DxP_Divisiones parametros)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var GuardarDivisiones = dXPFS.GuardarDivisiones(parametros);
                result.Add("GuardarDivisiones", GuardarDivisiones);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarDivisiones(tblAF_DxP_Divisiones parametros)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var EditarDivisiones = dXPFS.EditarDivisiones(parametros);
                result.Add("EditarDivisiones", EditarDivisiones);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarDivisiones(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var EliminarDivisiones = dXPFS.EliminarDivisiones(id);
                result.Add("EliminarDivisiones", EliminarDivisiones);
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

        #region DIVISIONES_PROYECTOS
        public ActionResult GetDivisiones_Proyectos()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstDivisiones_Proyectos = dXPFS.GetDivisiones_Proyectos();
                result.Add("lstDivisiones_Proyectos", lstDivisiones_Proyectos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDivisiones_ProyectosFitro(tblAF_DxP_Divisiones_Proyecto objFiltro)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstDivisiones_Proyectos = dXPFS.GetDivisiones_ProyectosFitro(objFiltro);
                result.Add("lstDivisiones_Proyectos", lstDivisiones_Proyectos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCC()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstGetCC = dXPFS.GetCC();
                result.Add(ITEMS, lstGetCC);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetCmbDivision()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstGetCmbDivision = dXPFS.GetCmbDivision();
                result.Add(ITEMS, lstGetCmbDivision);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GuardarDivisiones_Proyectos(tblAF_DxP_Divisiones_Proyecto parametros)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var GuardarDivisiones_Proyectos = dXPFS.GuardarDivisiones_Proyectos(parametros);
                result.Add("GuardarDivisiones_Proyectos", GuardarDivisiones_Proyectos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult EditarDivisiones_Proyectos(tblAF_DxP_Divisiones_Proyecto parametros)
        //{
        //    var result = new Dictionary<string, object>();
        //    try
        //    {
        //        var EditarDivisiones_Proyectos = dXPFS.EditarDivisiones_Proyectos(parametros);
        //        result.Add("EditarDivisiones_Proyectos", EditarDivisiones_Proyectos);
        //        result.Add(SUCCESS, true);
        //    }
        //    catch (Exception e)
        //    {
        //        result.Add(MESSAGE, e.Message);
        //        result.Add(SUCCESS, false);
        //    }
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult EliminarDivisiones_Proyectos(int id)
        //{
        //    var result = new Dictionary<string, object>();
        //    try
        //    {
        //        var EliminarDivisiones_Proyectos = dXPFS.EliminarDivisiones_Proyectos(id);
        //        result.Add("EliminarDivisiones_Proyectos", EliminarDivisiones_Proyectos);
        //        result.Add(SUCCESS, true);
        //    }
        //    catch (Exception e)
        //    {
        //        result.Add(MESSAGE, e.Message);
        //        result.Add(SUCCESS, false);
        //    }
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult EliminarDivisionesProyectos(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var EliminarDivisionesProyectos = dXPFS.EliminarDivisionesProyectos(id);
                result.Add("EliminarDivisionesProyectos", EliminarDivisionesProyectos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarDivisionesProyectos(tblAF_DxP_Divisiones_Proyecto parametros)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var EditarDivisionesProyectos = dXPFS.EditarDivisionesProyectos(parametros);
                result.Add("EditarDivisionesProyectos", EditarDivisionesProyectos);
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

        #region PQ
        [HttpGet]
        public JsonResult GetVersionPQ()
        {
            var version = 1;
            return Json(version, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarPQ()
        {
            var pq = JsonUtils.convertJsonToNetObject<tblAF_DxP_PQ>(Request.Form["pq"], "es-MX");
            HttpPostedFileBase archivoContrato = Request.Files["archivo"];

            var resultado = dXPFS.GuardarPQ(pq, archivoContrato);

            return Json(resultado);
        }

        [HttpPost]
        public JsonResult GetCCs()
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var ccs = ccFS.GetCCs().Select(m => new
                {
                    Value = m.cc,
                    Text = "[" + m.cc + "] " + m.descripcion
                }).OrderBy(o => o.Text);

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, ccs);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, ex.Message);
            }

            return Json(resultado);
        }

        [HttpPost]
        public JsonResult GetMonedas()
        {
            var resultado = dXPFS.GetMonedas();

            return Json(resultado);
        }

        [HttpGet]
        public JsonResult GetPQs(bool estatus, string fechaCorte)
        {
            var resultado = dXPFS.GetPQs(estatus, fechaCorte);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPQ(int id)
        {
            var resultado = dXPFS.GetPQ(id);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCuentaAbono()
        {
            var respuesta = new Dictionary<string,object>();

            try
            {
                List<int> cuentasBanco = new List<int> { 2120 };

                var resultado = cuentaFS.GetCuentas(cuentasBanco) as List<catctaDTO>;

                var combo = resultado.Select(m => new Core.DTO.Principal.Generales.ComboDTO
                {
                    Value = m.cta + "-" + m.scta + "-" + m.sscta + "-" + m.digito,
                    Text = "[" + m.cta + "-" + m.scta + "-" + m.sscta + "-" + m.digito + "] " + m.descripcion
                });

                respuesta.Add(SUCCESS, true);
                respuesta.Add(ITEMS, combo);
            }
            catch (Exception ex)
            {
                respuesta.Add(SUCCESS, false);
                respuesta.Add(MESSAGE, ex.Message);
            }

            return Json(respuesta);
        }

        [HttpPost]
        public JsonResult GetCuentaCargo()
        {
            var respuesta = new Dictionary<string, object>();

            try
            {
                List<int> cuentasBanco = new List<int> { 1110 };

                var resultado = cuentaFS.GetCuentas(cuentasBanco) as List<catctaDTO>;

                var combo = resultado.Select(m => new Core.DTO.Principal.Generales.ComboDTO
                {
                    Value = m.cta + "-" + m.scta + "-" + m.sscta + "-" + m.digito,
                    Text = "[" + m.cta + "-" + m.scta + "-" + m.sscta + "-" + m.digito + "] " + m.descripcion
                });

                respuesta.Add(SUCCESS, true);
                respuesta.Add(ITEMS, combo);
            }
            catch (Exception ex)
            {
                respuesta.Add(SUCCESS, false);
                respuesta.Add(MESSAGE, ex.Message);
            }

            return Json(respuesta);
        }

        [HttpPost]
        public JsonResult ObtenerInstitucionesPQ()
        {
            return Json(dXPFS.ObtenerInstitucionesPQ());
        }

        [HttpGet]
        public JsonResult GetPQLiquidar(int id)
        {
            return Json(dXPFS.GetPQLiquidar(id), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPQCambiarCC(int id)
        {
            return Json(dXPFS.GetPQCambiarCC(id), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPQRenovar(int id)
        {
            return Json(dXPFS.GetPQRenovar(id), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPQAbono(int id)
        {
            return Json(dXPFS.GetPQAbono(id), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetFechaServidor()
        {
            return Json(DateTime.Now.ToString("dd/MM/yyyy"), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Liquidar(int idPq, DateTime fechaMovimiento, List<PQPolizaDTO> infoPol)
        {
            var resultado = dXPFS.Liquidar(idPq, fechaMovimiento, infoPol);

            return Json(resultado);
        }

        [HttpPost]
        public JsonResult CambiarCC(int idPq, DateTime fechaMovimiento, List<PQPolizaDTO> infoPol)
        {
            var resultado = dXPFS.CambiarCC(idPq, fechaMovimiento, infoPol);

            return Json(resultado);
        }

        [HttpPost]
        public JsonResult RenovarPQ(int idPq, DateTime fechaMovimiento, DateTime fechaFirma, DateTime fechaVencimiento, decimal interes, HttpPostedFileBase archivo)
        {
            var infoPol = JsonUtils.convertJsonToNetObject<List<PQPolizaDTO>>(Request.Form["infoPol"], "es-MX");

            var resultado = dXPFS.RenovarPQ(idPq, fechaMovimiento, infoPol, fechaFirma, fechaVencimiento, interes, archivo);

            return Json(resultado);
        }

        [HttpPost]
        public JsonResult AbonarPQ(int idPq, DateTime fechaMovimiento, List<PQPolizaDTO> infoPol)
        {
            var resultado = dXPFS.AbonarPQ(idPq, fechaMovimiento, infoPol);
            return Json(resultado);
        }

        public FileResult DescargarArchivo(int idPq)
        {
            var resultado = dXPFS.UrlArchivoPQ(idPq);

            if ((bool)resultado[SUCCESS])
            {
                try
                {
                    var url = resultado[ITEMS] as tblAF_DxP_PQ_Archivo;
#if DEBUG
                    return File(@"C:\DOCUMENTOS_POR_PAGAR\DOCUMENTOS_PQ\Banamex_15.3 MDP_14-04-2021_2021-07-22T122001370.pdf", "application/pdf", "Banamex_15.3 MDP_14-04-2021_2021-07-22T122001370.pdf");
#else
                    return File(url.ubicacionArchivo, "application/pdf", url.nombreArchivo);
#endif
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public JsonResult UrlArchivoPQ(int idPq)
        {
            var resultado = dXPFS.UrlArchivoPQ(idPq);

#if DEBUG
            if ((bool)resultado[SUCCESS])
            {
                var resultadoLocal = new Dictionary<string, object>();
                resultadoLocal.Add(SUCCESS, true);
                resultadoLocal.Add(ITEMS, new { ubicacionArchivo = @"C:\DOCUMENTOS_POR_PAGAR\DOCUMENTOS_PQ\Banamex_15.3 MDP_14-04-2021_2021-07-22T122001370.pdf" });

                return Json(resultadoLocal, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(resultado);
            }
#else
            return Json(resultado, JsonRequestBehavior.AllowGet);
#endif
        }
        #endregion

        #region POLIZA REVALUACION
        public ActionResult RevaluacionDxP()
        {
            return View();
        }

        public JsonResult GetInfoRevaluacion(DateTime fecha)
        {
            var resultado = dXPFS.GetInfoRevaluacion(fecha);

            if ((bool)resultado[SUCCESS])
            {
                Session["DxP_polizaRevaluacion"] = resultado["poliza"];
            }
            else
            {
                Session.Remove("DxP_polizaRevaluacion");
            }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RegistrarPolizaRevaluacion()
        {
            var resultado = dXPFS.RegistrarPolizaRevaluacion((PolizaMovPolEkDTO)Session["DxP_polizaRevaluacion"]);

            Session.Remove("DxP_polizaRevaluacion");

            return Json(resultado);
        }
        #endregion

        public ActionResult CargarReporteInteresesPagados(DateTime fecha)
        {
            return Json(dXPFS.CargarReporteInteresesPagados(fecha), JsonRequestBehavior.AllowGet);
        }

        public MemoryStream DescargarExcelInteresesPagados(DateTime fecha)
        {
            var resultado = dXPFS.DescargarExcelInteresesPagados(fecha);

            if ((bool)resultado[SUCCESS])
            {
                var stream = (MemoryStream)resultado[ITEMS];
                Response.Clear();
                Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("Content-Disposition", "attachement; filename=InteresesPagados" + DateTime.Now.ToString("dd_MM_yyyy") + ".xlsx");
                Response.BinaryWrite(stream.ToArray());
                Response.End();
            }

            return null;
        }
    }
}