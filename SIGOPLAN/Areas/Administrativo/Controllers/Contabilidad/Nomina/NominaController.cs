using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIGOPLAN.Controllers;
using Core.DAO.Contabilidad.Nomina;
using Data.Factory.Contabilidad.Nomina;
using System.IO;
using Core.Entity.Administrativo.Contabilidad.Nomina;
using Core.DAO.Contabilidad.Propuesta;
using Data.Factory.Contabilidad.Propuesta;
using Core.DTO.Principal.Generales;
using Core.DTO.Contabilidad.Propuesta.Nomina;
using Infrastructure.Utils;
using Core.Enum.Administracion.Propuesta.Nomina;
using Core.Enum.Administracion.Nomina;
using Core.DAO.Enkontrol.General.CC;
using Data.Factory.Enkontrol.General.CC;
using Core.DTO.Contabilidad.Nomina.PolizaNomina;
using Core.DAO.Contabilidad.Cuenta;
using Data.Factory.Contabilidad.Cuenta;
using Core.DTO.Enkontrol.Tablas.Cuenta;
using Core.DTO.Contabilidad.Nomina.CuentaEmpleado;
using Core.DTO.Contabilidad.Poliza;
using Core.Enum.Principal;
using Core.DTO.Utils.Auth;
using Data.DAO.Enkontrol.General.CC;
using Core.DTO.Contabilidad.Nomina;
using System.Globalization;
using Newtonsoft.Json;
using Core.DTO.Contabilidad.Nomina.AcomuladoMensual;
using Core.DTO.Contabilidad.Nomina.ReporteRangoCC;
using Core.DTO.Contabilidad.Nomina.ReporteEmpleadoCC;
using Newtonsoft.Json.Linq;
using Core.Entity.RecursosHumanos.Captura;
using Core.DTO.Enkontrol.Alamcen;
using Data.Factory.RecursosHumanos.Empleado;
using Core.DAO.RecursosHumanos.Empleado;

namespace SIGOPLAN.Areas.Administrativo.Controllers.Contabilidad.Nomina
{
    public class NominaController : BaseController
    {
        private INominaDAO _nominaFs;
        private INominaResumenDAO _nominaResumenFs;
        private ICCDAO _ccEkFS;
        private ICuentaDAO _cuentaFS;
        private IEmpleadoDAO _empleadoFS;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _nominaFs = new NominaFactoryService().getNominaService();
            _nominaResumenFs = new NominaResumenFactoryServices().getNominaServices();
            _ccEkFS = new CCFactoryService().getCCService();
            _cuentaFS = new CuentaFactoryService().GetCuentaEkService();
            _empleadoFS = new EmpleadoFactoryService().GetEmpleadoService();

            base.OnActionExecuting(filterContext);
        }


        #region Generales
        public ActionResult GetTipoNominaPropuestaEnumCombo()
        {
            return Json(new { items = GlobalUtils.ParseEnumToCombo<tipoNominaPropuestaEnum>() }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTipoDescuentoPrenominaEnumCombo()
        {
            return Json(new { items = GlobalUtils.ParseEnumToCombo<TipoDescuentoPrenominaEnum>() }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCbotPeriodoNomina(int tipoNomina)
        {
            var respuesta = _nominaFs.PeriodosNomina(tipoNomina);

            return Json(respuesta);
        }

        public ActionResult GetCbotPeriodoNominaAguinaldo()
        {
            var respuesta = _nominaFs.PeriodosNominaAguinaldo();

            return Json(respuesta);
        }

        public JsonResult ObtenerEmpleadoPorClave(int clave_empleado)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var empleado = _empleadoFS.ObtenerEmpleadoPorClave(clave_empleado);

                resultado.Add("data", empleado);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, ex.Message);
            }

            return Json(resultado);
        }

        public JsonResult GetCCs()
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var ccs = _ccEkFS.GetCCs().Select(m => new
                {
                    Value = m.cc,
                    Text = "[" + m.cc + "] " + m.descripcion,
                    Prefijo = m.descripcion
                });

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

        public JsonResult GetTipoRaya()
        {
            var resultado = _nominaFs.GetTipoRaya();

            return Json(resultado);
        }

        public JsonResult GetClasificacionCC()
        {
            var resultado = _nominaFs.GetClasificacionCC();

            return Json(resultado);
        }
        #endregion

        #region Raya
        public ActionResult CargarArchivo(HttpPostedFileBase files1)
        {
            string fName = "";
            var result = new Dictionary<string, object>();
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];

                    BinaryReader b = new BinaryReader(file.InputStream);
                    byte[] binData = b.ReadBytes(file.ContentLength);
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {
                        var entidad = _nominaFs.ConvertCSVTABtoPrenomina(binData);
                        result.Add("data", entidad);
                        result.Add(MESSAGE, "El archvivo se importó correctamente");
                        result.Add(SUCCESS, true);
                    }
                }

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        List<ComboGroupDTO> SetCboNominas(List<PeriodosNominaDTO> lst)
        {
            return lst.GroupBy(g => g.tipo_nomina).Select(per => new ComboGroupDTO()
            {
                label = EnumExtensions.GetDescription((tipoNominaPropuestaEnum)per.Key),
                options = per.Select(nom => new ComboDTO()
                {
                    Value = nom.periodo.ToString(),
                    Text = string.Format("{0} #{1:00} NÓMINA DEL {2:00} AL {3:00} {4} {5}",
                        EnumExtensions.GetDescription((tipoNominaPropuestaEnum)per.Key),
                        nom.periodo,
                        nom.fecha_inicial.Day,
                        nom.fecha_final.Day,
                        nom.fecha_final.ToString("MMMMM").ToUpper(),
                        nom.fecha_final.Year),
                    Prefijo = string.Format("{0}-{1}-{2}-{3}",
                    nom.fecha_inicial.ToShortDateString(),
                    nom.fecha_final.ToShortDateString(),
                    nom.tipo_nomina,
                    nom.year),
                }).ToList()
            }).ToList();
        }

        public ActionResult FillCboCC()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = _nominaFs.FillCboCentroCostros();
                result.Add(ITEMS, lst);
                result.Add(SUCCESS, lst.Count > 0);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult FillCboEmpresasNomina()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var empresas = Enum.GetValues(typeof(EmpresasNominaEnum));
                List<ComboDTO> lst = new List<ComboDTO>();

                foreach (var item in empresas)
                {
                    ComboDTO auxCombo = new ComboDTO
                    {
                        Value = ((int)item).ToString(),
                        Text = ((EmpresasNominaEnum)item).GetDescription()
                    };
                    lst.Add(auxCombo);
                }

                result.Add(ITEMS, lst);
                result.Add(SUCCESS, lst.Count > 0);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        //public ActionResult GuardarPrenomina(tblC_Nom_Nomina nomina, List<tblC_Nom_PreNomina_Det> detalles, List<tblC_Nom_PreNomina_Aut> autorizantes)
        //{
        //    var result = _nominaFs.GuardarPrenomina(nomina, detalles, autorizantes);
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        #endregion

        #region Raya
        public ActionResult Raya()
        {
            return View();
        }

        public ActionResult CargarRaya(HttpPostedFileBase raya, int periodo, int tipoPeriodo, int year, int tipoRaya)
        {
            var resultado = _nominaFs.CargarRaya(raya, periodo, tipoPeriodo, year, tipoRaya);

            return Json(resultado);
        }

        public ActionResult CargarSUA(HttpPostedFileBase raya, int periodo, int tipoPeriodo, int year, int tipoRaya)
        {
            var resultado = _nominaFs.CargarSUA(raya, periodo, tipoPeriodo, year, tipoRaya);

            return Json(resultado);
        }

        public JsonResult GetRayaCargada(int periodo, int tipoPeriodo, int year, int tipoRaya, int clasificacionCC)
        {
            var resultado = _nominaFs.GetRayaCargada(periodo, tipoPeriodo, year, tipoRaya, clasificacionCC);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRayaDetalleCargada(int nominaId)
        {
            var resultado = _nominaFs.GetRayaDetalleCargada(nominaId);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Nómina
        public ActionResult Nomina()
        {
            return View();
        }

        public JsonResult GetNominas(int year, int tipoPeriodo, int periodo, int tipoRaya)
        {
            var resultado = _nominaFs.GetNominas(year, tipoPeriodo, periodo, tipoRaya);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ResumenNomina(int nominaId)
        {
            var resultado = _nominaFs.ResumenNomina(nominaId);

            if ((bool)resultado[SUCCESS])
            {
                var resumenNomina = resultado[ITEMS] as ResumenNominaDTO;

                Session["nom_resumenNomina"] = resumenNomina.detalle;
            }
            else
            {
                Session["nom_resumenNomina"] = null;
            }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ResumenNominaAguinaldo(int anio, string cc)
        {
            var resultado = _nominaFs.ResumenNominaAguinaldo(anio, cc);

            if ((bool)resultado[SUCCESS])
            {
                var resumenNomina = resultado[ITEMS] as ResumenNominaDTO;

                Session["nom_resumenNomina"] = resumenNomina.detalle;
            }
            else
            {
                Session["nom_resumenNomina"] = null;
            }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidarNomina(int nominaId)
        {
            var resultado = _nominaFs.ValidarNomina(nominaId, Session["nom_resumenNomina"] as List<ResumenDetalleNominaDTO>);

            if ((bool)resultado[SUCCESS])
            {
                Session["nom_resumenNomina"] = null;
            }

            return Json(resultado);
        }

        public JsonResult GenerarPoliza(int nominaId, DateTime fechaPol)
        {
            var resultado = _nominaFs.GenerarPoliza(nominaId, fechaPol);

            if ((bool)resultado[SUCCESS])
            {
                var r = new Dictionary<string, object>();
                r.Add(SUCCESS, resultado[SUCCESS]);
                r.Add(ITEMS, resultado["tblPoliza"]);

                Session["nom_poliza_movimientos"] = resultado[ITEMS];
                Session["nom_tipoRayaId"] = resultado["tipoRayaId"];

                return Json(r, JsonRequestBehavior.AllowGet);
            }

            Session.Remove("nom_poliza_movimientos");
            Session.Remove("nom_tipoRayaId");

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RegistrarPoliza()
        {
            var resultado = _nominaFs.RegistrarPoliza(Session["nom_poliza_movimientos"] as PolizaMovPolEkDTO, (int)Session["nom_tipoRayaId"]);

            if ((bool)resultado[SUCCESS])
            {
                Session["nom_poliza_movimientos"] = null;
            }

            return Json(resultado);
        }

        public MemoryStream DescargarExcelNomina(int tipo_nomina, int anio, int periodo)
        {
            var stream = _nominaFs.DescargarExcelNomina(tipo_nomina, anio, periodo);

            if (stream != null)
            {
                this.Response.Clear();
                this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "Reporte Nómina.xlsx"));
                this.Response.BinaryWrite(stream.ToArray());
                this.Response.End();

                return stream;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Empleado
        public JsonResult GetTipoCuenta()
        {
            var resultado = _nominaFs.GetTipoCuenta();

            return Json(resultado);
        }

        public JsonResult GetCuenta(string term)
        {
            var resultado = _cuentaFS.BuscarCuenta(term) as List<catctaDTO>;

            var resultado10 = resultado.Take(10).Select(m => new
            {
                id = m.cta + "-" + m.scta + "-" + m.sscta + "-" + m.digito,
                label = "[" + m.cta + "-" + m.scta + "-" + m.sscta + "-" + m.digito + "] " + m.descripcion
            });

            return Json(resultado10, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CuentaEmpleado()
        {
            return View();
        }

        public JsonResult CatalogoCuentaEmpleado(int? tipoCuentaId, string cc)
        {
            var resultado = _nominaFs.CatalogoCuentaEmpleado(tipoCuentaId, cc);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RegistrarEmpleado(tblC_Nom_CuentaEmpleado empleado)
        {
            var resultado = _nominaFs.RegistrarEmpleado(empleado);

            return Json(resultado);
        }

        public JsonResult EliminarEmpleado(int id)
        {
            var resultado = _nominaFs.EliminarEmpleado(id);

            return Json(resultado);
        }

        public JsonResult ModificarEmpleado(tblC_Nom_CuentaEmpleado empleado)
        {
            var resultado = _nominaFs.ModificarEmpleado(empleado);

            return Json(resultado);
        }

        public JsonResult ValidarCuentaEmpleado(List<ValidarCuentaEmpleadoDTO> ids)
        {
            var resultado = _nominaFs.ValidarCuentaEmpleado(ids);

            return Json(resultado);
        }

        public ActionResult RelacionarCuentasEmpleadosAutomaticamente()
        {
            var resultado = _nominaFs.RelacionarCuentasEmpleadosAutomaticamente();

            return View();
        }
        #endregion

        #region Prenomina
        public ActionResult Prenomina()
        {
            ViewBag.idEmpresa = Core.DTO.vSesiones.sesionEmpresaActual;
            return View();
        }

        public ActionResult PrenominaPeru()
        {
            return View();
        }

        public ActionResult GetEmpresaActual()
        {
            var result = new Dictionary<string, object>();
            result.Add("empresa", getEmpresa());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarPrenomina(string CC, int periodo, tipoNominaPropuestaEnum tipoNomina, int anio)
        {
            var result = _nominaFs.CargarPrenomina(CC, periodo, tipoNomina, anio);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarPrenominaPeru(string CC, int periodo, tipoNominaPropuestaEnum tipoNomina, int anio)
        {
            var result = _nominaFs.CargarPrenominaPeru(CC, periodo, tipoNomina, anio);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarPrenomina(int prenominaID, List<tblC_Nom_PreNomina_Det> detalles, List<tblC_Nom_PreNomina_Aut> autorizantes, string CC, int periodo, tipoNominaPropuestaEnum tipoNomina, int anio)
        {
            var result = _nominaFs.GuardarPrenomina(prenominaID, detalles, autorizantes, CC, periodo, tipoNomina, anio);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public MemoryStream CrearExcelPrenomina(int prenominaID)
        {
            var empresa = Core.DTO.vSesiones.sesionEmpresaActual;
            MemoryStream stream = new MemoryStream();
            var nomina = _nominaFs.GetPrenominaByID(prenominaID);
            var esAguinado = nomina == null ? false : nomina.tipoNomina == 10;

            switch (empresa) 
            {
                case 3:
                    if (esAguinado) stream = _nominaFs.crearExcelAguinaldo(prenominaID, 1);
                    else stream = _nominaFs.crearExcelPrenominaColombia(prenominaID, 1);
                    break;
                case 6:
                    if (esAguinado) stream = _nominaFs.crearExcelAguinaldo(prenominaID, 1);
                    else stream = _nominaFs.crearExcelPrenominaPeru(prenominaID, 1);
                    break;
                default:
                    if (esAguinado) stream = _nominaFs.crearExcelAguinaldo(prenominaID, 1);
                    else stream = _nominaFs.crearExcelPrenomina(prenominaID, 1);
                    break;
            }

            if (stream != null)
            {
                this.Response.Clear();
                this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "prenomina.xlsx"));
                this.Response.BinaryWrite(stream.ToArray());
                this.Response.End();

                return stream;
            }
            else
            {
                return null;
            }
        }

        public ActionResult ValidarPrenomina(int prenominaID, List<tblC_Nom_PreNomina_Det> detalles, List<tblC_Nom_PreNomina_Aut> autorizantes)
        {
            var result = _nominaFs.ValidarPrenomina(prenominaID, detalles, autorizantes);
            if ((bool)result[SUCCESS])
            {
                var autorizantesObra = autorizantes.Where(x => x.esObra).ToList();
                var exito = EnviarCorreoAutorizantes(prenominaID, autorizantesObra);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DesValidarPrenomina(int prenominaID)
        {
            var result = _nominaFs.DesValidarPrenomina(prenominaID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private bool EnviarCorreoAutorizantes(int prenominaID, List<tblC_Nom_PreNomina_Aut> autorizantes)
        {
            var exito = false;
            try
            {
                List<int> autorizantesIDs = autorizantes.Select(x => x.aprobadorClave).ToList();
                List<string> correosAutorizantes = _nominaFs.GetCorreosUsuarios(autorizantesIDs);
                var prenomina = _nominaFs.GetPrenominaByID(prenominaID);
                var ccCompleto = _ccEkFS.GetCC(prenomina.CC);

                var correo = new Infrastructure.DTO.CorreoDTO();
                correo.asunto = "AUTORIZACIÓN " + ((tipoNominaPropuestaEnum)prenomina.tipoNomina).GetDescription() + " #" + prenomina.periodo.ToString() + " " + prenomina.year.ToString() + " [" + prenomina.CC + "] " + ccCompleto.descripcion;

                correo.correos.AddRange(correosAutorizantes);
                correo.cuerpo = "SE REQUIERE SU AUTORIZACIÓN PARA " + ((tipoNominaPropuestaEnum)prenomina.tipoNomina).GetDescription() + " DEL PERIODO #" + prenomina.periodo + " " + prenomina.year.ToString() + " en [" + prenomina.CC + "] " + ccCompleto.descripcion + ".";
                correo.Enviar();
                exito = true;
            }
            catch (Exception e)
            {
                exito = false;
            }
            return exito;
        }

        public bool EnviarCorreoAutorizantesOficina(List<int> prenominasIDs, tipoNominaPropuestaEnum tipoPrenomina, int periodo, int anio)
        {
            var exito = false;
            try
            {
                var autorizantes = _nominaFs.GetListaAutorizantesOficina(prenominasIDs).Where(x => !x.esObra).ToList();
                List<string> CCs = new List<string>();
                DateTime diaActual = DateTime.Now.AddDays(1);
                var periodoStr = _nominaFs.GetPeriodoNomina(anio, periodo, (int)tipoPrenomina);

                var autorizantesGroup = autorizantes.GroupBy(x => x.aprobadorClave).ToList();

                foreach (var item in autorizantesGroup)
                {
                    CCs.Clear();
                    var correosAutorizantes = _nominaFs.GetCorreosUsuarios(new List<int> { item.Key });
                    var auxPrenominas = item.Select(x => x.prenominaID).ToList();
                    foreach (var prenomina in auxPrenominas)
                    {
                        var auxCC = _nominaFs.GetPrenominaByID(prenomina).CC;
                        CCs.Add(auxCC);
                    }
                    var correo = new Infrastructure.DTO.CorreoDTO();

                    if (tipoPrenomina == tipoNominaPropuestaEnum.Aguinaldo) correo.asunto = "AUTORIZACIÓN AGUINADO " + (periodo == 1 ? "SEMANAL " : "QUINCENAL ") + anio.ToString();
                    else correo.asunto = "AUTORIZACIÓN " + tipoPrenomina.GetDescription() + " #" + periodo.ToString();

                    if (tipoPrenomina == tipoNominaPropuestaEnum.Aguinaldo) correo.cuerpo = "SE REQUIERE SU AUTORIZACIÓN PARA AGUINADO " + (periodo == 1 ? "SEMANAL " : "QUINCENAL ") + anio.ToString() + " " + periodoStr + " DE LOS SIGUIENTES PROYECTOS: <br><br>";
                    else correo.cuerpo = "SE REQUIERE SU AUTORIZACIÓN PARA " + tipoPrenomina.GetDescription() + " DEL PERIODO #" + periodo.ToString() + " " + periodoStr + " DE LOS SIGUIENTES PROYECTOS: <br><br>";
                    
                    foreach (var auxCC in CCs)
                    {
                        var ccCompleto = _nominaFs.GetCCNominas(auxCC);
                        correo.cuerpo += "* [" + auxCC + "] " + ccCompleto + ".<br>";
                    }
                    correo.correos.AddRange(correosAutorizantes);
                    if (tipoPrenomina == tipoNominaPropuestaEnum.Semanal)
                    {
                        correo.cuerpo += "<br>DICHA AUTORIZACIÓN, DE NO SER APLICADA ANTES DEL DIA MIÉRCOLES A LAS 16:00 HORAS, SE AUTORIZARÁ EN AUTOMÁTICO Y SE PROCEDERA CON EL PAGO. EN CUALQUIER MOMENTO PODRÁN INGRESAR PARA VISUALIZAR COMO FUE PAGADA Y HACER OBSERVACIONES EN CASO NECESARIO.";
                       // correo.correos.Add("l.rodriguez@construplan.com.mx");
                      //  correo.correos.Add("melissa.molina@construplan.com.mx");
                    }
                    else 
                    {
//                        correo.cuerpo += "<br>DICHA AUTORIZACIÓN, DE NO SER APLICADA ANTES DEL DIA " + diaActual.ToString("dd/MM/yyyy") + " A LAS " + diaActual.ToString("HH:mm") + " HORAS, SE VALIDARÁ POR PARTE DEL DEPARTAMENTO DE NOMINAS Y SE PROCEDERA CON EL PAGO. EN CUALQUIER MOMENTO PODRÁN INGRESAR PARA VISUALIZAR COMO FUE PAGADA Y HACER OBSERVACIONES EN CASO NECESARIO.";
                        correo.cuerpo += "<br>DICHA AUTORIZACIÓN, DE NO SER APLICADA ANTES DEL DIA " + diaActual.ToString("dd/MM/yyyy") + " A LAS " + diaActual.ToString("HH:mm") + " HORAS, SE AUTORIZARÁ EN AUTOMÁTICO Y SE PROCEDERA CON EL PAGO. EN CUALQUIER MOMENTO PODRÁN INGRESAR PARA VISUALIZAR COMO FUE PAGADA Y HACER OBSERVACIONES EN CASO NECESARIO.<br><br>";
                        //correo.cuerpo += "<b>NOTA: SE LES RECUERDA QUE ESTA QUINCENA TRAE 13 DÍAS EN EL CALENDARIO, POR LO CUAL SE PAGAN 13 DÍAS POR BASE NÓMINA + 2 DÍAS POR COMPLEMENTO PARA TOTALIZAR LOS 15, SIEMPRE Y CUANDO NO PRESENTEN FALTAS.</b>";
                       // correo.correos.Add("ivana.lucero@construplan.com.mx");
                      //  correo.correos.Add("l.madrid@construplan.com.mx");
                    }                    
                    correo.Enviar();
                }
                exito = _nominaFs.AplicarPrenominaNotificadaOficina(prenominasIDs);
            }
            catch (Exception e)
            {
                exito = false;
            }
            return exito;
        }

        public ActionResult GetCCsIncidencias(tipoNominaPropuestaEnum tipoNomina, int periodo, int anio)
        {
            var result = _nominaFs.GetCCsIncidencias(tipoNomina, periodo, anio);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboBancos(tipoNominaPropuestaEnum tipoNomina, int periodo)
        {
            var result = _nominaFs.FillCboBancos(tipoNomina, periodo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUsuariosAutorizantes()
        {
            var result = _nominaFs.GetUsuariosAutorizantes();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult EnviarCorreoDespacho(int anio, tipoNominaPropuestaEnum tipoNomina, int periodo)
        {
            var result = new Dictionary<string, object>();
            try
            {               
                System.Net.Mime.ContentType ctExcel = new System.Net.Mime.ContentType("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                System.Net.Mime.ContentType ctPDF = new System.Net.Mime.ContentType("application/pdf");
                var correo = new Infrastructure.DTO.CorreoDTO();

                List<Tuple<int, string>> bancos = new List<Tuple<int, string>>();
                //bancos.Add(new Tuple<int, string>(1, "BANAMEX"));
                //bancos.Add(new Tuple<int, string>(2, "SANTANDER"));                
                ////if (getEmpresaID() == 1) bancos.Add(new Tuple<int, string>(3, "EICI"));
                var auxBancos = _nominaFs.GetBancos(tipoNomina == tipoNominaPropuestaEnum.Aguinaldo ? (periodo == 1 ? tipoNominaPropuestaEnum.Semanal : tipoNominaPropuestaEnum.Quincenal) : tipoNomina).GroupBy(x => new { x.tipoSolicitudCheque, x.tipoSolicitudChequeDescripcion });
                foreach (var item in auxBancos) 
                {
                    bancos.Add(new Tuple<int, string>(item.Key.tipoSolicitudCheque, item.Key.tipoSolicitudChequeDescripcion));
                }

                var descripcionTipoPrenomina = tipoNomina.GetDescription();
                var descripcionEmpresa = getEmpresaDescripcion();

                correo = new Infrastructure.DTO.CorreoDTO();
                foreach (var item in bancos)
                {
                    byte[] bytesReportePrenomina = null;
                    if (tipoNomina == tipoNominaPropuestaEnum.Aguinaldo) bytesReportePrenomina = _nominaFs.ExcelAguinaldoPorPeriodo(anio, tipoNomina, periodo, item.Item1);
                    else bytesReportePrenomina = _nominaFs.ExcelPrenominasPorPeriodo(anio, tipoNomina, periodo, item.Item1);
                    if (bytesReportePrenomina != null)
                    {
                        Stream streamReportePrenomina = new MemoryStream(bytesReportePrenomina);
                        System.Net.Mail.Attachment ReportePrenomina = new System.Net.Mail.Attachment(streamReportePrenomina, ctExcel);
                        if (tipoNomina == tipoNominaPropuestaEnum.Aguinaldo) ReportePrenomina.ContentDisposition.FileName = "AGUINALDO " + (periodo == 1 ? "SEMANAL" : "QUINCENAL") + " " + anio + " " + descripcionEmpresa + " " + item.Item2 + ".xlsx";
                        else ReportePrenomina.ContentDisposition.FileName = "PRE" + descripcionTipoPrenomina + " PERIODO #" + periodo + " " + anio + " " + descripcionEmpresa + " " + item.Item2 + ".xlsx";
                        correo.archivos.Add(ReportePrenomina);
                        List<Byte[]> downloadPDF = null;
                        List<Byte[]> downloadPDF2 = null;

                        switch (item.Item1) 
                        {
                            case 1:
                                downloadPDF = (List<Byte[]>)Session["rptSolicitudCheque1"];
                                downloadPDF2 = (List<Byte[]>)Session["rptCedulaCostosNomina"]; 
                                break;
                            case 2:
                                downloadPDF = (List<Byte[]>)Session["rptSolicitudCheque2"];
                                downloadPDF2 = (List<Byte[]>)Session["rptCedulaCostosNomina2"]; 
                                break;
                            case 3:
                                downloadPDF = (List<Byte[]>)Session["rptSolicitudCheque3"];
                                downloadPDF2 = (List<Byte[]>)Session["rptCedulaCostosNomina3"]; 
                                break;
                            case 4:
                                downloadPDF = (List<Byte[]>)Session["rptSolicitudCheque4"];
                                downloadPDF2 = (List<Byte[]>)Session["rptCedulaCostosNomina4"]; 
                                break;
                        }

                        Byte[] bytesPDF = downloadPDF.SelectMany(x => x).ToArray();
                        Byte[] bytesPDF2 = downloadPDF2.SelectMany(x => x).ToArray();

                        Stream streamSolicitudCheque = new MemoryStream(bytesPDF);
                        System.Net.Mail.Attachment solicitudCheque = new System.Net.Mail.Attachment(streamSolicitudCheque, ctPDF);
                        if (tipoNomina == tipoNominaPropuestaEnum.Aguinaldo) solicitudCheque.ContentDisposition.FileName = "SOLICITUD CHEQUE AGUINALDO " + (periodo == 1 ? "SEMANAL" : "QUINCENAL") + " " + anio + " " + descripcionEmpresa + " " + item.Item2 + ".pdf";
                        else solicitudCheque.ContentDisposition.FileName = "SOLICITUD CHEQUE " + descripcionTipoPrenomina + " PERIODO #" + periodo + " " + anio + " " + descripcionEmpresa + " " + item.Item2 + ".pdf";
                        correo.archivos.Add(solicitudCheque);

                        Stream streamCedula = new MemoryStream(bytesPDF2);
                        System.Net.Mail.Attachment cedula = new System.Net.Mail.Attachment(streamCedula, ctPDF);
                        if (tipoNomina == tipoNominaPropuestaEnum.Aguinaldo) cedula.ContentDisposition.FileName = "CEDULA AGUINALDO " + (periodo == 1 ? "SEMANAL" : "QUINCENAL") + " " + anio + " " + descripcionEmpresa + " " + item.Item2 + ".pdf";
                        else cedula.ContentDisposition.FileName = "CEDULA " + descripcionTipoPrenomina + " PERIODO #" + periodo + " " + anio + " " + descripcionEmpresa + " " + item.Item2 + ".pdf";
                        correo.archivos.Add(cedula);
                        
                    }
                }
                if (correo.archivos.Count() > 0)
                {
                    if (tipoNomina == tipoNominaPropuestaEnum.Aguinaldo) correo.asunto = "ARCHIVOS AGUINALDO " + (periodo == 1 ? "SEMANAL" : "QUINCENAL") + " " + anio + " " + descripcionEmpresa;
                    else correo.asunto = "ARCHIVOS " + descripcionTipoPrenomina + " PERIODO #" + periodo + " " + anio + " " + descripcionEmpresa;
                    if (getEmpresaID() == 6)
                    {
                      /*  correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("m.cruz@construplan.com.mx");*/
                    }
                    else 
                    {
                       /* correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("l.madrid@construplan.com.mx");
                        correo.correos.Add("l.rodriguez@construplan.com.mx");
                        correo.correos.Add("melissa.molina@construplan.com.mx");
                        correo.correos.Add("ivana.lucero@construplan.com.mx");*/
                    }

                    if (tipoNomina == tipoNominaPropuestaEnum.Aguinaldo) correo.cuerpo = "SE ENVÍAN LOS ARCHIVOS CORRESPONDIENTES AL AGUINALDO " + (periodo == 1 ? "SEMANAL" : "QUINCENAL") + " " + anio + ". PARA LA EMPRESA " + descripcionEmpresa + ".";
                    else correo.cuerpo = "SE ENVÍAN LOS ARCHIVOS CORRESPONDIENTES A " + descripcionTipoPrenomina + " PERIODO #" + periodo + " " + anio + ". PARA LA EMPRESA " + descripcionEmpresa + ".";
                    correo.Enviar();
                    result.Add(SUCCESS, true);
                }
                else 
                {
                    result.Add(SUCCESS, false);
                    result.Add(MESSAGE, "No se encontró información autorizada para prenóminas con las características seleccionadas.");
                }
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;

            Session["rptSolicitudCheque1"] = null;
            Session["rptSolicitudCheque2"] = null;
            Session["rptSolicitudCheque3"] = null;
            return json;
        }

        public ActionResult EnviarCorreoSolicitudes(int anio, tipoNominaPropuestaEnum tipoNomina, int periodo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                System.Net.Mime.ContentType ctExcel = new System.Net.Mime.ContentType("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                System.Net.Mime.ContentType ctPDF = new System.Net.Mime.ContentType("application/pdf");
                var correo = new Infrastructure.DTO.CorreoDTO();

                List<Tuple<int, string>> bancos = new List<Tuple<int, string>>();
                //bancos.Add(new Tuple<int, string>(1, "BANAMEX"));
                //bancos.Add(new Tuple<int, string>(2, "SANTANDER"));                
                ////if (getEmpresaID() == 1) bancos.Add(new Tuple<int, string>(3, "EICI"));
                var auxBancos = _nominaFs.GetBancos(tipoNomina == tipoNominaPropuestaEnum.Aguinaldo ? (periodo == 1 ? tipoNominaPropuestaEnum.Semanal : tipoNominaPropuestaEnum.Quincenal) : tipoNomina).GroupBy(x => new { x.tipoSolicitudCheque, x.tipoSolicitudChequeDescripcion });
                foreach (var item in auxBancos)
                {
                    bancos.Add(new Tuple<int, string>(item.Key.tipoSolicitudCheque, item.Key.tipoSolicitudChequeDescripcion));
                }

                var descripcionTipoPrenomina = tipoNomina.GetDescription();
                var descripcionEmpresa = getEmpresaDescripcion();

                correo = new Infrastructure.DTO.CorreoDTO();
                foreach (var item in bancos)
                {
                    byte[] bytesReportePrenomina = null;

                    
                    List<Byte[]> downloadPDF = null;
                    List<Byte[]> downloadPDF2 = null;

                    switch (item.Item1)
                    {
                        case 1:
                            downloadPDF = (List<Byte[]>)Session["rptSolicitudCheque1"];
                            break;
                        case 2:
                            downloadPDF = (List<Byte[]>)Session["rptSolicitudCheque2"];
                            break;
                        case 3:
                            downloadPDF = (List<Byte[]>)Session["rptSolicitudCheque3"];
                            break;
                        case 4:
                            downloadPDF = (List<Byte[]>)Session["rptSolicitudCheque4"];
                            break;
                    }
                    if (downloadPDF != null)
                    {
                        Byte[] bytesPDF = downloadPDF.SelectMany(x => x).ToArray();

                        Stream streamSolicitudCheque = new MemoryStream(bytesPDF);
                        System.Net.Mail.Attachment solicitudCheque = new System.Net.Mail.Attachment(streamSolicitudCheque, ctPDF);
                        if (tipoNomina == tipoNominaPropuestaEnum.Aguinaldo) solicitudCheque.ContentDisposition.FileName = "SOLICITUD CHEQUE AGUINALDO " + (periodo == 1 ? "SEMANAL" : "QUINCENAL") + " " + anio + " " + descripcionEmpresa + " " + item.Item2 + ".pdf";
                        else solicitudCheque.ContentDisposition.FileName = "SOLICITUD CHEQUE " + descripcionTipoPrenomina + " PERIODO #" + periodo + " " + anio + " " + descripcionEmpresa + " " + item.Item2 + ".pdf";
                        correo.archivos.Add(solicitudCheque);
                    }
                }
                if (correo.archivos.Count() > 0)
                {
                    if (tipoNomina == tipoNominaPropuestaEnum.Aguinaldo) correo.asunto = "ARCHIVOS AGUINALDO " + (periodo == 1 ? "SEMANAL" : "QUINCENAL") + " " + anio + " " + descripcionEmpresa;
                    else correo.asunto = "NOTIFICACIÓN SOLICITUD CHEQUE NÓMINA " + (tipoNomina == tipoNominaPropuestaEnum.Semanal ? "SEMANAL" : "QUINCENAL") + " PERIODO " + periodo.ToString() + " " + anio.ToString() + " (" + descripcionEmpresa + ")";
                    if (getEmpresaID() == 6)
                    {
                      /*  correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("m.cruz@construplan.com.mx");*/
                    }
                    else
                    {
                      /*  correo.correos.Add("alberto.azpe@construplan.com.mx");
                        correo.correos.Add("jose.buenrostro@construplan.com.mx");                        
                        correo.correos.Add("l.madrid@construplan.com.mx");
                        correo.correos.Add("l.rodriguez@construplan.com.mx");
                        correo.correos.Add("melissa.molina@construplan.com.mx");
                        correo.correos.Add("ivana.lucero@construplan.com.mx");
                        correo.correos.Add("rene.olea@construplan.com.mx");    */                    
                    }

                    if (tipoNomina == tipoNominaPropuestaEnum.Aguinaldo) correo.cuerpo = "SE ENVÍAN LOS ARCHIVOS CORRESPONDIENTES AL AGUINALDO " + (periodo == 1 ? "SEMANAL" : "QUINCENAL") + " " + anio + ". PARA LA EMPRESA " + descripcionEmpresa + ".";
                    else correo.cuerpo = "SE ENVÍAN SOLICITUDES DE CHEQUE PARA NOTIFICAR CIERRE DE NÓMINA (" + (tipoNomina == tipoNominaPropuestaEnum.Semanal ? "SEMANAL" : "QUINCENAL") + ") DEL PERIODO #" + periodo.ToString() + " " + anio + ". PARA LA EMPRESA (" + descripcionEmpresa + ").";
                    correo.Enviar();
                    result.Add(SUCCESS, true);
                }
                else
                {
                    result.Add(SUCCESS, false);
                    result.Add(MESSAGE, "No se encontró información autorizada para prenóminas con las características seleccionadas.");
                }
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al obtener la información del servidor.");
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;

            Session["rptSolicitudCheque1"] = null;
            Session["rptSolicitudCheque2"] = null;
            Session["rptSolicitudCheque3"] = null;
            return json;
        }

        public ActionResult GenerarReciboNomina(ReciboNominaDTO objParamsDTO)
        {
            return Json(_nominaFs.GenerarReciboNomina(objParamsDTO), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Gestion Prenomina
        public ActionResult GestionPrenomina()
        {
            ViewBag.idEmpresa = Core.DTO.vSesiones.sesionEmpresaActual;
            return View();
        }

        public ActionResult GetEstadosAutotizacion()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cbo = GlobalUtils.ParseEnumToCombo<authEstadoEnum2>();
                var esSuccess = cbo.Count > 0;
                if (esSuccess)
                {
                    result.Add(ITEMS, cbo);
                }
                result.Add(SUCCESS, esSuccess);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetLstGestionPrenomina(string CC, int periodo, tipoNominaPropuestaEnum tipoNomina, int anio, string estatus)
        {

            var result = _nominaFs.GetLstGestionPrenomina(CC, periodo, tipoNomina, anio, estatus);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListaAutorizantes(int prenominaID)
        {
            var result = _nominaFs.GetListaAutorizantes(prenominaID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AutorizarPrenomina(authDTO auth)
        {
            var result = _nominaFs.AutorizarPrenomina(auth);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RechazarPrenomina(authDTO auth)
        {
            var result = _nominaFs.RechazarPrenomina(auth);

            if ((bool)result["success"] == true) 
            {
                var correo = new Infrastructure.DTO.CorreoDTO();
                tblC_Nom_Prenomina prenomina = (tblC_Nom_Prenomina)result["prenomina"];
                correo.asunto = "RECHAZO NÓMINA " + ((tipoNominaPropuestaEnum)prenomina.tipoNomina == tipoNominaPropuestaEnum.Semanal ? "SEMANAL" : "QUINCENAL") + " PARA EL CC " + prenomina.CC + " PERIODO #" + prenomina.periodo.ToString() + " " + prenomina.year.ToString() + " (" + (string)result["empresa"] + ")";

                correo.cuerpo = "SE RECHAZÓ LA NÓMINA " + ((tipoNominaPropuestaEnum)prenomina.tipoNomina == tipoNominaPropuestaEnum.Semanal ? "SEMANAL" : "QUINCENAL") + " PARA EL CC " + prenomina.CC + " PERIODO #" + prenomina.periodo.ToString() + " " + prenomina.year.ToString() + " (" + (string)result["empresa"] + ").<br/><br/>"
                    + "RESPONSABLE: " + (string)result["responsable"] + "<br/>"
                    + "MOTIVO DEL RECHAZO: " + auth.comentario + "";

             /*   correo.correos.Add("l.madrid@construplan.com.mx");
                correo.correos.Add("l.rodriguez@construplan.com.mx");
                correo.correos.Add("melissa.molina@construplan.com.mx");
                correo.correos.Add("ivana.lucero@construplan.com.mx");
                correo.correos.Add("rene.olea@construplan.com.mx");*/

                correo.Enviar();
            }


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Alerta Autorizacion Prenomina
        public ActionResult AlertaAutorizacion()
        {
            return View();
        }

        public ActionResult CargarPrenominasValidadas(string CC, int periodo, tipoNominaPropuestaEnum tipoNomina, int anio)
        {
            var result = _nominaFs.CargarPrenominasValidadas(CC, periodo, tipoNomina, anio);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult VerificarCorreoDespacho(int periodo, tipoNominaPropuestaEnum tipoNomina, int anio)
        {
            var result = _nominaFs.VerificarCorreoDespacho(periodo, tipoNomina, anio);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //public bool EnviarCorreoDespacho(int periodo, tipoNominaPropuestaEnum tipoNomina, int anio)
        //{
        //    var exito = false;
        //    try
        //    {
        //        List<string> correosDespacho = new List<string> { "rene.olea@construplan.com.mx" };
        //        System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        //        var prenominas = _nominaFs.GetPrenominasPeriodo(periodo, anio, (int)tipoNomina);

        //        var correo = new Infrastructure.DTO.CorreoDTO();
        //        correo.asunto = "NOMINA " + ((tipoNominaPropuestaEnum)tipoNomina).GetDescription() + " #" + periodo.ToString() + " " + anio.ToString();
        //        List<tblC_Nom_Prenomina> prenominasAutorizadas = _nominaFs.GetPrenominasAutorizadas(periodo, tipoNomina, anio);
        //        foreach (var item in prenominasAutorizadas)
        //        {
        //            byte[] bytesReporteEntradas = _nominaFs.DataExcelPrenomina(item.id);
        //            Stream streamReporteEntradas = new MemoryStream(bytesReporteEntradas);
        //            System.Net.Mail.Attachment ReporteEntradas = new System.Net.Mail.Attachment(streamReporteEntradas, ct);
        //            ReporteEntradas.ContentDisposition.FileName = "[" + item.CC + "] - " + ((tipoNominaPropuestaEnum)tipoNomina).GetDescription() + " #" + periodo.ToString() + " " + anio.ToString() + ".xlsx";
        //            correo.archivos.Add(ReporteEntradas);
        //        }

        //        correo.correos.AddRange(correosDespacho);
        //        correo.cuerpo = "SE ENVÍA DOCUMENTACION PARA NOMINA " + ((tipoNominaPropuestaEnum)tipoNomina).GetDescription() + " DEL PERIODO #" + periodo + " " + anio.ToString() + ".";
        //        correo.Enviar();
        //        exito = true;
        //    }
        //    catch (Exception e)
        //    {
        //        exito = false;
        //    }
        //    return exito;
        //}


        #endregion

        #region Reporte Anual

        //VISTA
        public ActionResult ReporteAnualNomina()
        {
            return View();
        }

        public ActionResult GetReportes(string bottom, string top)
        {

            var result = new Dictionary<string, object>();
            try
            {
                List<reporteAnualDTO> reporte = _nominaFs.GetReportes(bottom, top);
                if (reporte.Count() != 0)
                {

                    //Guardar las parejas de mes/año unicos
                    var months = reporte.Select(y => new { y.nombreDeMes, y.año , y.mes}).Distinct().ToList();
                    //Guardas las cuentas y su descripcion unicas
                    var lstCtaUnq = reporte.Select(y => new { y.cta, y.concepto }).Distinct().OrderBy(x => x.cta).ToList();

                    List<reporteAnualDTO> lstMontos = reporte.Select(x => x).ToList();
                    List<reporteAnualDTO> lstMontosnew = new List<reporteAnualDTO>();

                    foreach (var month in months)
                    {
                        foreach (var cuenta in lstCtaUnq)
                        {
                            //Buscar si existe un registro de cada cuenta
                            reporteAnualDTO objUnico = lstMontos.Where(r => r.cta == cuenta.cta && r.concepto == cuenta.concepto && r.nombreDeMes == month.nombreDeMes && r.año == month.año).FirstOrDefault();
                            reporteAnualDTO objUnico2 = lstMontosnew.Where(r => r.cta == cuenta.cta && r.concepto == cuenta.concepto && r.nombreDeMes == month.nombreDeMes && r.año == month.año).FirstOrDefault();
                            //List<reporteAnualDTO> lstObjDuplicado = lstMontos.Where(r => r.cta == cuenta.cta && r.nombreDeMes == month.nombreDeMes && r.año == month.año).Select(x => x).ToList();
                            if (objUnico == null && objUnico2 == null)
                            {
                                //Si no existe el registro para dicha cuenta se crea uno con el valor de monto de 0
                                reporteAnualDTO obj = new reporteAnualDTO();
                                obj.mes = month.mes;
                                obj.nombreDeMes = month.nombreDeMes;
                                obj.año = month.año;
                                obj.monto = 0;
                                obj.cta = cuenta.cta;
                                obj.cta1 = cuenta.cta.Substring(0,4);
                                obj.cta2 = cuenta.cta.Substring(5,2);
                                obj.cta3 = cuenta.cta.Substring(8);
                                obj.concepto = cuenta.concepto;
                                lstMontosnew.Add(obj);

                            }
                        }
                    }

                    //Agregar los registron con cuentas en 0 y los nuevos registros de cuenta duplicada
                    lstMontos.AddRange(lstMontosnew);

                    //lstMontos = lstMontos.OrderBy(x => Int32.Parse(x.cta.Replace("-",string.Empty))).ToList();

                    //Agrupar el arreglo de valores por mes año y sumar los montos para obetener el total del mes
                    var lst = months.Select(y => new
                    {
                        nombreMes = y.nombreDeMes,
                        año = y.año,
                        mes = y.mes,
                        lstMontos = lstMontos.Where(n => n.nombreDeMes == y.nombreDeMes && n.año == y.año).Select(x => new {
                            monto = x.monto.ToString("C", CultureInfo.CurrentCulture),
                            x.cta,
                            x.cta1,
                            x.cta2,
                            x.cta3,
                            x.concepto
                        }).OrderBy(x => Int32.Parse(x.cta.Substring(0,4)))
                        .ThenBy(x => Int32.Parse(x.cta.Substring(5, 2))).ThenBy(x => Int32.Parse(x.cta.Substring(8))).ToList(),
                        total = lstMontos.Where(n => n.nombreDeMes == y.nombreDeMes && n.año == y.año).Select(x => x.monto).Sum().ToString("C", CultureInfo.CurrentCulture),
                    }).ToList();

                    //foreach(var mList in lst)
                    //{
                    //    mList.lstMontos.OrderBy(x => Int32.Parse(x.cta.Substring(0, 2))).ThenBy(x => Int32.Parse(x.cta.Substring(5, 2))).ThenBy(x => Int32.Parse(x.cta.Substring(8)));
                    //}

                    lst = lst.OrderBy(e => e.año).ToList();

                    result.Add(ITEMS, lst);
                    result.Add(SUCCESS, true);
                }
                else
                {
                    result.Add(ITEMS, null);
                    result.Add(SUCCESS, false);
                }

            }
            catch (Exception e)
            {
                if (result[ITEMS] == null)
                {
                    result.Add(MESSAGE, "Rango vacio");
                }
                else
                {
                    result.Add(MESSAGE, e.Message);
                }
                
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult creaVariableDeSesion(string lstReporte, string tp, string btm)
        {
            var result = new Dictionary<string, object>();
            List<string[]> listaReg = JsonConvert.DeserializeObject<List<string[]>>(lstReporte).ToList();
            //List<dynamic> lstHeaderReg = JsonConvert.DeserializeObject<List<dynamic>>(lstHeader).ToList();
            Session["Documento"] = _nominaFs.crearReporte(listaReg,tp,btm);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public MemoryStream crearReporte()
        {
            var result = new Dictionary<string, object>();
            MemoryStream stream = (MemoryStream)Session["Documento"];

            if (stream != null)
            {
                this.Response.Clear();
                this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "Reporte Anual Nomina.xlsx"));
                this.Response.BinaryWrite(stream.ToArray());
                this.Response.End();

                Session["Documento"] = null;
                return stream;
            }
            else
            {
                return null;
            }
        }
        #endregion

        public ActionResult Descuentos()
        {
            return View();
        }

        public ActionResult AutorizaBajasCH()
        {
            return View();
        }

        #region Acumulado Mensual

        //VISTA
        public ActionResult AcumuladoMensual()
        {
            return View();
        }

        public ActionResult GetNominaDetalle(string botomDate, string topDate)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = _nominaFs.GetNominaDetalle(botomDate, topDate);
                result.Add(ITEMS,data);
                result.Add(SUCCESS, true);
            }catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            
            

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult creaVariableDeSesionMensual(List<ReporteAcomuladoMensualDTO> lstReporteData, ReporteAcomuladoMensualDTO lstReporteHeaders, string numeroEmpleados, string periodoDate)
        {
            var result = new Dictionary<string, object>();

            Session["Documento"] = _nominaFs.crearReporteMensual(lstReporteData, lstReporteHeaders, numeroEmpleados, periodoDate);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public MemoryStream crearReporteMensual()
        {
            var result = new Dictionary<string, object>();
            MemoryStream stream = (MemoryStream)Session["Documento"];

            if (stream != null)
            {
                this.Response.Clear();
                this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "Reporte Acumulado Mensual.xlsx"));
                this.Response.BinaryWrite(stream.ToArray());
                this.Response.End();

                Session["Documento"] = null;
                return stream;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Reporte Rango Centro de costo

        public ActionResult ReporteRangoCC()
        {
            return View();
        }

        public ActionResult GetReportesCC(string bottom, string top)
        {

            var result = new Dictionary<string, object>();
            try
            {
                List<ReporteRangoCCDTO> reporte = _nominaFs.GetReportesCC(bottom, top);
                if (reporte.Count() != 0)
                {

                    //Guardar las parejas de mes/año unicos
                    var months = reporte.Select(y => new { y.nombreDeMes, y.año, y.mes }).Distinct().ToList();
                    //Guardas las cuentas y su descripcion unicas
                    var lstCtaUnq = reporte.Select(y => new { y.cc, y.nombreCC }).Distinct().OrderBy(x => x.cc).ToList();

                    List<ReporteRangoCCDTO> lstMontos = reporte.Select(x => x).ToList();
                    List<ReporteRangoCCDTO> lstMontosnew = new List<ReporteRangoCCDTO>();

                    foreach (var month in months)
                    {
                        foreach (var cuenta in lstCtaUnq)
                        {
                            //Buscar si existe un registro de cada cuenta
                            ReporteRangoCCDTO objUnico = lstMontos.Where(r => r.cc == cuenta.cc && r.nombreDeMes == month.nombreDeMes && r.año == month.año).FirstOrDefault();
                            ReporteRangoCCDTO objUnico2 = lstMontosnew.Where(r => r.cc == cuenta.cc && r.nombreDeMes == month.nombreDeMes && r.año == month.año).FirstOrDefault();
                            if (objUnico == null && objUnico2 == null)
                            {
                                //Si no existe el registro para dicha cuenta se crea uno con el valor de monto de 0
                                ReporteRangoCCDTO obj = new ReporteRangoCCDTO();
                                obj.mes = month.mes;
                                obj.nombreDeMes = month.nombreDeMes;
                                obj.año = month.año;
                                obj.monto = 0;
                                obj.cc = cuenta.cc;
                                obj.nombreCC = cuenta.nombreCC;
                                lstMontosnew.Add(obj);

                            }
                        }
                    }

                    //Agregar los registron con cuentas en 0 y los nuevos registros de cuenta duplicada
                    lstMontos.AddRange(lstMontosnew);
                    lstMontos = lstMontos.OrderBy(x => x.mes).ToList();

                    //Agrupar el arreglo de valores por mes año y sumar los montos para obetener el total del mes
                    var lst = months.Select(y => new
                    {
                        nombreMes = y.nombreDeMes,
                        año = y.año,
                        lstMontos = lstMontos.Where(n => n.nombreDeMes == y.nombreDeMes && n.año == y.año).Select(x => new { monto = x.monto.ToString("C", CultureInfo.CurrentCulture), x.cc, x.nombreCC }).OrderBy(x => x.cc).ToList(),
                        total = lstMontos.Where(n => n.nombreDeMes == y.nombreDeMes && n.año == y.año).Select(x => x.monto).Sum().ToString("C", CultureInfo.CurrentCulture),
                    }).ToList();

                    lst = lst.OrderBy(e => e.año).ToList();

                    result.Add(ITEMS, lst);
                    result.Add(SUCCESS, true);
                }
                else
                {
                    result.Add(ITEMS, null);
                    result.Add(SUCCESS, false);
                }

            }
            catch (Exception e)
            {
                if (result[ITEMS] == null)
                {
                    result.Add(MESSAGE, "Rango vacio");
                }
                else
                {
                    result.Add(MESSAGE, e.Message);
                }
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult creaVariableDeSesionCC(string lstReporte, string tp, string btm)
        {
            var result = new Dictionary<string, object>();
            List<string[]> listaReg = JsonConvert.DeserializeObject<List<string[]>>(lstReporte).ToList();
            //List<dynamic> lstHeaderReg = JsonConvert.DeserializeObject<List<dynamic>>(lstHeader).ToList();
            Session["Documento"] = _nominaFs.crearReporteCC(listaReg, tp, btm);

            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public MemoryStream crearReporteCC()
        {
            var result = new Dictionary<string, object>();
            MemoryStream stream = (MemoryStream)Session["Documento"];

            if (stream != null)
            {
                this.Response.Clear();
                this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "Reporte (Rango) por Centro de costo.xlsx"));
                this.Response.BinaryWrite(stream.ToArray());
                this.Response.End();

                Session["Documento"] = null;
                return stream;
            }
            else
            {
                return null;
            }
        }

        public ActionResult GetNumEmpleados(string bottom, string top)
        {
            var result = new Dictionary<string, object>();

            try
            {
                int numEmpleados = _nominaFs.GetNumEmpleados(bottom,top);
                result.Add(ITEMS, numEmpleados);
                result.Add(SUCCESS, true);

            }catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Reporte rango por centro de costo y cuenta (cta)

        public ActionResult ReporteCentroCuenta()
        {
            return View();
        }

        public ActionResult GetReporteCtroCta(string bottom, string top)
        {
            var result = new Dictionary<string, object>();

            try
            {

                var values = _nominaFs.GetReporteCtroCta(bottom,top);

                result.Add(ITEMS, values);
                result.Add(SUCCESS, true);

            }catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult creaVariableDeSesionCentroCuenta(string lstReporte, string tp, string btm)
        {
            var result = new Dictionary<string, object>();
            List<string[]> listaReg = JsonConvert.DeserializeObject<List<string[]>>(lstReporte).ToList();
            //List<dynamic> lstHeaderReg = JsonConvert.DeserializeObject<List<dynamic>>(lstHeader).ToList();
            Session["Documento"] = _nominaFs.crearReporteCentroCuenta(listaReg, tp, btm);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public MemoryStream crearReporteCentroCuenta()
        {
            var result = new Dictionary<string, object>();
            MemoryStream stream = (MemoryStream)Session["Documento"];

            if (stream != null)
            {
                this.Response.Clear();
                this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "Reporte rango por Centro de costo y Cuenta.xlsx"));
                this.Response.BinaryWrite(stream.ToArray());
                this.Response.End();

                Session["Documento"] = null;
                return stream;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Reporte rango por Empleado y centro de costo

        public ActionResult ReporteEmpleadoCC()
        {
            return View();
        }

        public ActionResult GetReporteEmpleadoCC(string bottom, string top, string cc, string empleado, int tipoRaya)
        {
            var result = new Dictionary<string, object>();
            JsonResult json = new JsonResult();
            try
            {
                var list = _nominaFs.GetReporteEmpleadoCC(bottom,top,cc,empleado, tipoRaya);
                //Session["Documento"] = _nominaFs.crearReporteEmpleadoCC(list);
                //result.Add(ITEMS,list);
                //result.Add(SUCCESS, true);

                json.MaxJsonLength = Int32.MaxValue;
                json.Data = list;

            }catch(Exception e)
            {
                //if (result[ITEMS] == null)
                //{
                //    result.Add(MESSAGE, "Rango vacio");
                //}
                //else
                //{
                //    result.Add(MESSAGE, e.Message);
                //}
                
                result.Add(SUCCESS, false);    
            }

            return json;

        }

        public ActionResult creaVariableDeSesionEmpleadoCC(string bottom, string top, string cc, string empleado, string tp, string btm, int tipoRaya)
        {
            Dictionary<string, object> list = new Dictionary<string, object>();
            var list2 = _nominaFs.GetReporteEmpleadoCC(bottom, top, cc, empleado, tipoRaya);// CODIGO REDUNDANTE
            Session["Documento"] = _nominaFs.crearReporteEmpleadoCC(list2, tp, btm);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public MemoryStream crearReporteEmpleadoCC()
        {
            //try
            //{
                MemoryStream stream = (MemoryStream)Session["Documento"];

                if (stream != null)
                {
                    this.Response.Clear();
                    this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "Reporte rango por Empleado y Centro de costo.xlsx"));
                    this.Response.BinaryWrite(stream.ToArray());
                    this.Response.End();

                    Session["Documento"] = null;
                    return stream;
                }
                else
                {
                    return null;
                }
            //}catch(Exception e)
            //{

            //}
            //return null;
            
        }

        public JsonResult GetCentroCostos()
        {
            var resultado = _nominaFs.GetCentroCostos();

            return Json(resultado);
        }

        public JsonResult GetListaEmpleados()
        {
            var resultado = _nominaFs.GetListaEmpleados();

            return Json(resultado);
        }

        #endregion

        #region Reporte Concentrado
        public ActionResult ReporteConcentrado()
        {
            return View();
        }

        public ActionResult GetReporteConcentrado(DateTime? fechaInicial, DateTime? fechaFinal, int? tipoRaya, int? tipoNomina)
        {
            return Json(_nominaFs.GetReporteConcentrado(fechaInicial, fechaFinal, tipoRaya, tipoNomina), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Layout Incidencias
        public ActionResult LayoutIncidencias()
        {
            return View();
        }

        public ActionResult CargarLayoutIncidencias(int anio, int periodo, int tipo_nomina, string estatus)
        {
            return Json(_nominaFs.CargarLayoutIncidencias(anio, periodo, tipo_nomina, estatus), JsonRequestBehavior.AllowGet);
        }

        public MemoryStream DescargarExcelLayoutIncidencias()
        {
            var resultadoTupla = _nominaFs.DescargarExcelLayoutIncidencias(0);

            if (resultadoTupla != null)
            {
                string nombreArchivo = resultadoTupla.Item2;

                this.Response.Clear();
                this.Response.ContentType = MimeMapping.GetMimeMapping(nombreArchivo);

                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", nombreArchivo));
                this.Response.BinaryWrite(resultadoTupla.Item1.ToArray());
                this.Response.End();

                return resultadoTupla.Item1;
            }
            else
            {
                return null;
            }
        }

        public MemoryStream DescargarExcelReporteIncidencias(int anio, int tipo_nomina, int periodo, bool autorizado)
        {
            var resultadoTupla = _nominaFs.DescargarExcelReporteIncidencias(anio, tipo_nomina, periodo, autorizado);

            if (resultadoTupla != null)
            {
                string nombreArchivo = resultadoTupla.Item2;

                this.Response.Clear();
                this.Response.ContentType = MimeMapping.GetMimeMapping(nombreArchivo);

                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", nombreArchivo));
                this.Response.BinaryWrite(resultadoTupla.Item1.ToArray());
                this.Response.End();

                return resultadoTupla.Item1;
            }
            else
            {
                return null;
            }
        }

        public ActionResult EnviarCorreoLayoutIncidencias()
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var resultadoTupla = _nominaFs.DescargarExcelLayoutIncidencias(1);

                if (resultadoTupla != null)
                {
                    string nombreArchivo = resultadoTupla.Item2;

                    this.Response.Clear();
                    this.Response.ContentType = MimeMapping.GetMimeMapping(nombreArchivo);

                    this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", nombreArchivo));
                    this.Response.BinaryWrite(resultadoTupla.Item1.ToArray());
                    this.Response.End();

                    string asunto = "Layout Incidencias";
                    string mensaje = "";

                    List<string> emails = new List<string>();
#if DEBUG
                 //   emails.Add("rene.olea@construplan.com.mx");
#else
                  /*  emails.Add("l.madrid@construplan.com.mx");
                    emails.Add("l.rodriguez@construplan.com.mx");
                    emails.Add("melissa.molina@construplan.com.mx");
                    emails.Add("ivana.lucero@construplan.com.mx");*/
#endif

                    List<Byte[]> listaArchivos = new List<byte[]> { resultadoTupla.Item1.ToArray() };

                    List<tblRH_BN_Incidencia> listaIncidencias = resultadoTupla.Item3;

                    _nominaFs.ActualizarIncidencias(listaIncidencias);

                    GlobalUtils.sendEmailAdjuntoInMemorySendExcel(asunto, mensaje, emails, listaArchivos, nombreArchivo);
                    resultado.Add(SUCCESS, true);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Todos los centros de costos han sido enviados para el periodo solicitado.");
                }                
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult GetEmpleadosPendientesLiberacion()
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = _nominaFs.getEmpleadosPendientesLiberacion();

                result.Add("data", data);
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

        public ActionResult GuardarBajas(List<EmpleadoPendienteLiberacionDTO> empleados)
        {
            var result = new Dictionary<string, object>();

            try
            {
                _nominaFs.guardarBajas(empleados);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region Descuentos
        public ActionResult GetDescuentos()
        {
            var json = Json(_nominaFs.GetDescuentos(), JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;

            return json;
        }

        public ActionResult GuardarNuevoDescuento(tblC_Nom_PreNomina_Descuento descuento)
        {
            return Json(_nominaFs.GuardarNuevoDescuento(descuento), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarDescuento(tblC_Nom_PreNomina_Descuento descuento)
        {
            return Json(_nominaFs.EditarDescuento(descuento), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarDescuento(tblC_Nom_PreNomina_Descuento descuento)
        {
            return Json(_nominaFs.EliminarDescuento(descuento), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarExcelDescuentos()
        {
            return Json(_nominaFs.CargarExcelDescuentos(Request.Files), JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult GetEstatusPeriodo(int tipo_nomina, int anio)
        {
            return Json(_nominaFs.GetEstatusPeriodo(tipo_nomina, anio), JsonRequestBehavior.AllowGet);
        }
        #region SUA
        public ActionResult CargaProvisionSUA()
        {
            return View();
        }
        public JsonResult GetTipoDocumento()
        {
            var resultado = _nominaFs.GetTipoDocumento();

            return Json(resultado);
        }

        public ActionResult CedulaSUA()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetSUA(int tipoDocumento, int year, int periodo)
        {
            var resultado = _nominaFs.GetSUA(tipoDocumento, year, periodo);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GenerarPolizaSUA(int suaId, DateTime fecha, int tipoDocumento)
        {
            var resultado = _nominaFs.GenerarPolizaSUA(suaId, fecha, tipoDocumento);

            if ((bool)resultado[SUCCESS])
            {
                var r = new Dictionary<string, object>();
                r.Add(SUCCESS, resultado[SUCCESS]);
                r.Add(ITEMS, resultado[ITEMS]);

                Session["sua_poliza_movimientos"] = resultado[ITEMS];
                Session["sua_suaId"] = resultado["suaId"];

                return Json(r, JsonRequestBehavior.AllowGet);
            }

            Session.Remove("sua_poliza_movimientos");
                
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RegistrarPolizaSUA(ClasificacionDocumentosSUAEnum tipoDocumento)
        {
            var resultado = _nominaFs.RegistrarPolizaSUA(Session["sua_poliza_movimientos"] as PolizaMovPolEkDTO, Convert.ToInt32(Session["sua_suaId"]), tipoDocumento);

            if ((bool)resultado[SUCCESS])
            {
                Session["sua_poliza_movimientos"] = null;
            }

            return Json(resultado);
        }

        public JsonResult GetSUACargado(int periodo, int tipoPeriodo, int year, int tipoDocumento)
        {
            var resultado = _nominaFs.GetSUACargado(periodo, tipoPeriodo, year, tipoDocumento);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public MemoryStream DescargarExcelSUA(int tipo_nomina, int anio, int periodo, int tipo_documento)
        {
            var stream = _nominaFs.DescargarExcelSUA(tipo_nomina, anio, periodo, tipo_documento);

            if (stream != null)
            {
                this.Response.Clear();
                this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "Reporte SUA.xlsx"));
                this.Response.BinaryWrite(stream.ToArray());
                this.Response.End();

                return stream;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region PERU
        public ActionResult GuardarPrenominaPeru(int prenominaID, List<tblC_Nom_PreNominaPeru_Det> detalles, List<tblC_Nom_PreNomina_Aut> autorizantes, string CC, int periodo, tipoNominaPropuestaEnum tipoNomina, int anio)
        {
            var result = _nominaFs.GuardarPrenominaPeru(prenominaID, detalles, autorizantes, CC, periodo, tipoNomina, anio);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ValidarPrenominaPeru(int prenominaID, List<tblC_Nom_PreNominaPeru_Det> detalles, List<tblC_Nom_PreNomina_Aut> autorizantes)
        {
            var result = _nominaFs.ValidarPrenominaPeru(prenominaID, detalles, autorizantes);
            if ((bool)result[SUCCESS])
            {
                var autorizantesObra = autorizantes.Where(x => x.esObra).ToList();
                var exito = EnviarCorreoAutorizantes(prenominaID, autorizantesObra);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region CATALOGO AFP
        public ActionResult PeruAFP()
        {
            return View();
        }

        public ActionResult GetRegistrosPeruAFP()
        {
            return Json(_nominaFs.GetRegistrosPeruAFP(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarRegistroPeruAFP(PeruAFPDTO objParamsDTO)
        {
            return Json(_nominaFs.CrearEditarRegistroPeruAFP(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRegistroActualizarPeruAFP(PeruAFPDTO objParamsDTO)
        {
            return Json(_nominaFs.GetRegistroActualizarPeruAFP(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarRegistroPeruAFP(PeruAFPDTO objParamsDTO)
        {
            return Json(_nominaFs.EliminarRegistroPeruAFP(objParamsDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboCatAFP()
        {
            return Json(_nominaFs.FillCboCatAFP(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetAnioMes()
        {
            return Json(_nominaFs.SetAnioMes(), JsonRequestBehavior.AllowGet); 
        }
        #endregion

        #endregion
    }
}