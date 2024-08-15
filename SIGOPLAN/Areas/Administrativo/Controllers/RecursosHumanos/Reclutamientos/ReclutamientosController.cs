using Core.DAO.RecursosHumanos.Reclutamientos;
using Core.DAO.RecursosHumanos.Tabuladores;
using Core.DTO;
using Core.DTO.Enkontrol.Tablas.RH.Empleado;
using Core.DTO.Principal.Generales;
using Core.DTO.RecursosHumanos;
using Core.DTO.RecursosHumanos.Enkontrol;
using Core.DTO.RecursosHumanos.Reclutamientos;
using Core.DTO.RecursosHumanos.Starsoft;
using Core.Entity.Administrativo.RecursosHumanos.Enkontrol;
using Core.Entity.RecursosHumanos.Reclutamientos;
using Core.Enum.Multiempresa;
using Core.Enum.RecursosHumanos.Reclutamientos;
using Core.Enum.RecursosHumanos.Tabuladores;
using Data.Factory.RecursosHumanos;
using Data.Factory.RecursosHumanos.Tabuladores;
using DotnetDaddy.DocumentViewer;
//using Data.Factory.RecursosHumanos.ReportesRH;
using Infrastructure.Utils;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SIGOPLAN.Areas.Administrativo.Controllers.RecursosHumanos.Reclutamiento
{
    public class ReclutamientosController : BaseController
    {
        #region INIT
        Dictionary<string, object> result = new Dictionary<string, object>();
        public IReclutamientosDAO reclutamientoFS;
        public ITabuladoresDAO _TabuladorFS;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            reclutamientoFS = new ReclutamientosFactoryServices().getReclutamientosService();
            _TabuladorFS = new TabuladoresFactoryService().GetTabuladoresService();
            base.OnActionExecuting(filterContext);
        }
        #endregion

        #region RETURN VISTAS
        public ActionResult Solicitudes()
        {
            ViewBag.Empresa = vSesiones.sesionEmpresaActual;
            return View();
        }

        public ActionResult GestionSolicitudes()
        {
            ViewBag.Empresa = vSesiones.sesionEmpresaActual;
            return View();
        }

        public ActionResult GestionCandidatos()
        {
            ViewBag.Empresa = vSesiones.sesionEmpresaActual;

            return View();
        }

        public ActionResult Fases()
        {
            return View();
        }

        public ActionResult SegCandidatos()
        {
            return View();
        }

        public ActionResult AltaEmpleados(int? empleadosAdmn)
        {
            ViewBag.UsuarioDeConsulta = reclutamientoFS.UsuarioDeConsulta();
            ViewBag.UsuarioDeConsultaTeorico = reclutamientoFS.UsuarioDeConsultaTeorico();
            ViewBag.UsuarioPermisoAccionEditarFechaCambio = reclutamientoFS.UsuarioPermisoAccionEditarFechaCambio();
            ViewBag.UsuarioPermisoEditarDiasIngresos = reclutamientoFS.GetPuedeEditarDiasDisponibles();
            ViewBag.UsuarioPermisoAccionOcultarSalarioByCC = reclutamientoFS.UsuarioPermisoAccionOcultarSalarioByCC();
            ViewBag.UsuarioPermisoMostarBotones = reclutamientoFS.UsuarioPermisoMostrarBotones();
            ViewBag.Empresa = vSesiones.sesionEmpresaActual;

            var diasDisponibles = reclutamientoFS.GetDiasDisponiblesParaLimitarFecha()[ITEMS] as DiasDisponiblesIngresosDTO;
            ViewBag.fechaMinima = diasDisponibles.anterior;
            ViewBag.fechaMaxima = diasDisponibles.posterior;

            ViewBag.PermisoTabuladorAltaEmpleado = reclutamientoFS.PermisoTabuladorAltaEmpleado();

            ViewBag.AutorizacionMasivaActiva = vSesiones.sesionUsuarioDTO.id == 1041 || vSesiones.sesionUsuarioDTO.id == 1019 || vSesiones.sesionUsuarioDTO.id == 79377;

            ViewBag.UsuarioVerSalarios = reclutamientoFS.UsuarioVerSalarios();

            if (!empleadosAdmn.HasValue)
            {
                empleadosAdmn = Session["empleadosAdmn"] as int?;

                if (empleadosAdmn == null)
                {
                    empleadosAdmn = 1;
                }
            }
            else
            {
                Session["empleadosAdmn"] = empleadosAdmn;
            }

            ViewBag.empleadosAdmn = empleadosAdmn;

            return View();
        }

        public ActionResult ReporteSegCandidatos()
        {
            return View();
        }

        public ActionResult Plataformas()
        {
            return View();
        }

        public ActionResult CatCorreos()
        {
            return View();
        }
        public ActionResult ExpedienteDigital()
        {
            return View();
        }

        public ActionResult Archivos()
        {
            return View();
        }

        public ActionResult _menuClick()
        {
            return View();
        }

        public ActionResult RelRegPatronales()
        {
            ViewBag.Empresa = vSesiones.sesionEmpresaActual;

            return View();
        }

        public ActionResult RelFases()
        {
            return View();
        }

        public ActionResult CatContratos()
        {
            return View();
        }

        public ActionResult CatCategorias()
        {
            return View();
        }

        public ActionResult CatPuestos()
        {
            bool tieneAcceso = _TabuladorFS.VerificarAcceso((int)MenuTabuladoresEnum.CATALOGO_DE_PUESTOS);
            if (tieneAcceso)
                return View();
            else
                return View("ErrorPermisoVista");
        }

        public ActionResult CatTabuladores()
        {
            return View();
        }
        #endregion

        #region SOLICITUDES
        public ActionResult GetSolicitudes(SolicitudesDTO objFiltro)
        {
            result = new Dictionary<string, object>();
            try
            {
                List<SolicitudesDTO> lstSolicitudes = reclutamientoFS.GetSolicitudes(objFiltro);
                result.Add("lstSolicitudes", lstSolicitudes);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarSolicitud(tblRH_REC_Solicitudes objSolicitud)
        {
            result = new Dictionary<string, object>();
            try
            {
                #region SE CREA/ACTUALIZA LA SOLICITUD
                if (objSolicitud.id > 0)
                {
                    #region SE ACTUALIZA LA SOLICITUD
                    bool esActualizarSolicitud = reclutamientoFS.ActualizarSolicitud(objSolicitud);
                    if (!esActualizarSolicitud)
                        throw new Exception("Ocurrió un error al actualizar la solicitud.");
                    else
                        result.Add(SUCCESS, true);
                    #endregion
                }
                else
                {
                    #region SE CREA LA SOLICITUD
                    bool esCrearSolicitud = reclutamientoFS.CrearSolicitud(objSolicitud);
                    if (!esCrearSolicitud)
                        throw new Exception("Ocurrió un error al crear la solicitud.");
                    else
                        result.Add(SUCCESS, true);
                    #endregion
                }
                #endregion
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarSolicitud(int idSolicitud)
        {
            try
            {
                result = new Dictionary<string, object>();
                if (idSolicitud > 0)
                {
                    #region SE VERIFICA SI LA SOLICITUD NO SE ENCUENTRA ASIGNADA A UN CANDIDATO, EN CASO QUE NO, SE PUEDE ELIMINAR LA SOLICITUD
                    bool existeSolicitudRelCandidato = reclutamientoFS.ExisteSolicitudRelCandidato(idSolicitud);
                    #endregion

                    #region SE ELIMINA LA SOLICITUD
                    if (!existeSolicitudRelCandidato)
                    {
                        bool esEliminarSolicitud = reclutamientoFS.EliminarSolicitud(idSolicitud);
                        if (esEliminarSolicitud)
                            result.Add(SUCCESS, true);
                        else
                            throw new Exception("Ocurrió un error al eliminar la solicitud");
                    }
                    else
                        throw new Exception("No se puede eliminar la solicitud, ya que se encuentra relacionada a un candidato.");
                    #endregion
                }
                else
                    throw new Exception("Ocurrió un error al eliminar la solicitud");
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region METODOS COPIADOS DE ADITIVA CONTROLLER
        public ActionResult FillCboPuestosSolicitudes(string cc)
        {
            Core.Entity.RecursosHumanos.Catalogo.tblRH_CatPuestos objCatPuestos = new Core.Entity.RecursosHumanos.Catalogo.tblRH_CatPuestos();
            List<Core.Entity.RecursosHumanos.Catalogo.tblRH_CatPuestos> listobjCatPuestos = new List<Core.Entity.RecursosHumanos.Catalogo.tblRH_CatPuestos>();

            var items = reclutamientoFS.FillCboPuestosSolicitudes(cc);
            var filteredItems = items.Select(x => new { id = x.puesto, label = x.descripcion });
            List<string> lstPuestosCategorias = new List<string>();
            foreach (var objfilteredItems in filteredItems)
            {
                objCatPuestos = new Core.Entity.RecursosHumanos.Catalogo.tblRH_CatPuestos();
                string Puesto = CargaPuesto(objfilteredItems.label);
                objCatPuestos.descripcion = Puesto;
                objCatPuestos.puesto = objfilteredItems.id;
                listobjCatPuestos.Add(objCatPuestos);
            }

            var ListaPuestos = listobjCatPuestos.GroupBy(a => a.descripcion).Select(b => b.First());
            //List<ComboDTO> cboPuestosDTO = ListaPuestos.OrderBy(o => o.descripcion).Select(s => new ComboDTO
            //{
            //    Value = s.puesto.ToString(),
            //    Text = !string.IsNullOrEmpty(s.descripcion) ? s.descripcion.Trim().ToUpper() : string.Empty
            //}).ToList();
            var cboPuestosDTO = filteredItems.OrderBy(x => x.label).Select(x => new ComboDTO
            {
                Value = x.id.ToString(),
                Text = !string.IsNullOrEmpty(x.label) ? x.label.Trim().ToUpper() : string.Empty
            }).ToList();

            result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, cboPuestosDTO);
                result.Add("lstPuestosCategorias", lstPuestosCategorias);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private string CargaPuesto(string strPuesCat)
        {
            string puestoFinal = strPuesCat, categoriaFinal = "", Cadena = strPuesCat;
            int LongUltEspacio = Cadena.LastIndexOf(' '), longitudPuesto = Cadena.Length;
            if (LongUltEspacio != -1)
            {
                int longUltPalabra = longitudPuesto - (LongUltEspacio);
                string ultPalabra = (Cadena.Substring(LongUltEspacio, longUltPalabra));
                Char delimiter = ' ';
                String[] substrings = ultPalabra.Split(delimiter);
                foreach (var palabra in substrings)
                {
                    ultPalabra = substrings[1];
                    break;
                }
                if (ultPalabra.Length == 1)
                {
                    puestoFinal = (Cadena.Substring(0, LongUltEspacio));
                    categoriaFinal = ultPalabra;
                }
                else
                {
                    puestoFinal = Cadena;
                    categoriaFinal = "N/A";
                }
            }
            strPuesCat = puestoFinal;
            return strPuesCat;
        }
        #endregion

        public ActionResult GetCategoriasRelPuesto(string _cc, string _puesto)
        {
            result = new Dictionary<string, object>();
            try
            {
                var lstCategoriasRelPuesto = reclutamientoFS.GetCategoriasRelPuesto(_cc, _puesto);
                result.Add("lstCategoriasRelPuesto", lstCategoriasRelPuesto);
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

        #region GESTIÓN DE SOLICITUDES
        public JsonResult GetGestionSolicitudes(GestionSolicitudesDTO objFiltro)
        {
            result = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE LISTADO DE GESTIÓN DE SOLICITUDES
                List<GestionSolicitudesDTO> lstGestionSolicitudes = reclutamientoFS.GetGestionSolicitudes(objFiltro);
                #endregion

                #region SE VERIFICA SI ES NECESARIO CREAR UNA ADITIVA
                //SIGOPLAN.Areas.Administrativo.Controllers.RecursosHumanos.AditivaPersonal.AditivaPersonalController objAditivaPersonalController = new AditivaPersonal.AditivaPersonalController();
                //List<dynamic> lstAditivaPersonal = new List<dynamic>();
                //foreach (var item in lstGestionSolicitudes)
                //{
                //    if (item.idPuesto > 0 && !item.esPuestoNuevo)
                //    {
                //        var objAditiva = objAditivaPersonalController.getCategorias(item.puesto, item.cc);
                //        lstAditivaPersonal.Add(objAditiva);
                //    }
                //}
                //result.Add("lstAditivaPersonal", lstAditivaPersonal);
                #endregion

                //for (int i = 0; i < lstAditivaPersonal.Count(); i++)
                //{
                //    if (lstGestionSolicitudes[i].idPuesto > 0)
                //    {
                //        lstGestionSolicitudes[i].personalExistente = lstAditivaPersonal[i].Data[0].altas;
                //        lstGestionSolicitudes[i].personalRequerido = lstAditivaPersonal[i].Data[0].cantidad;
                //    }
                //}

                result.Add("lstGestionSolicitudes", lstGestionSolicitudes);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RechazarSolicitud(GestionSolicitudesDTO objGestionSolicitud)
        {
            try
            {
                result = new Dictionary<string, object>();
                if (objGestionSolicitud.idSolicitud > 0)
                {
                    #region SE VERIFICA SI LA SOLICITUD YA SE ENCUENTRA ASIGNADA A UN CANDIDATO, DE LO CONTRARIO, SI SE PUEDE RECHAZAR DICHA SOLICITUD
                    bool existeSolicitudRelCandidato = reclutamientoFS.ExisteSolicitudRelCandidato(objGestionSolicitud.idSolicitud);
                    #endregion

                    if (!existeSolicitudRelCandidato)
                    {
                        #region SE RECHAZA LA SOLICITUD
                        bool esRechazadaSolicitud = reclutamientoFS.RechazarSolicitud(objGestionSolicitud);
                        if (esRechazadaSolicitud)
                            result.Add(SUCCESS, true);
                        else
                            throw new Exception("Ocurrió un error al rechazar la solicitud");
                        #endregion
                    }
                    else
                        throw new Exception("No es posible rechazar la solicitud, ya que se encuentra relacionada a un candidato.");

                }
                else
                    throw new Exception("Ocurrió un error al rechazar la solicitud");
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GESTIÓN DE CANDIDATOS
        public ActionResult GetCandidatos(CandidatosDTO objCandidatos)
        {
            result = new Dictionary<string, object>();
            //try
            //{
            List<CandidatosDTO> lstCandidatos = reclutamientoFS.GetCandidatos(objCandidatos);
            result.Add("lstCandidatos", lstCandidatos);
            result.Add(SUCCESS, true);
            //}
            //catch (Exception e)
            //{
            //    result.Add(MESSAGE, e.Message);
            //    result.Add(SUCCESS, false);
            //}
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult CrearEditarCandidato(HttpPostedFileBase objFile)
        {
            result = new Dictionary<string, object>();
            try
            {
                GestionCandidatosDTO objCandidato = JsonUtils.convertJsonToNetObject<GestionCandidatosDTO>(Request.Form["objCandidato"], "es-MX");

                #region SE CREA/ACTUALIZA AL CANDIDATO
                if (objCandidato.id > 0 && objCandidato != null)
                {
                    #region SE ACTUALIZA AL CANDIDATO
                    var actualizacion = reclutamientoFS.ActualizarCandidato(objCandidato, objFile);
                    if (!actualizacion.Item1)
                        throw new Exception(actualizacion.Item2);
                    else
                        result.Add(SUCCESS, true);
                    #endregion
                }
                else if (objCandidato != null)
                {
                    #region SE CREA AL CANDIDATO
                    var creacion = reclutamientoFS.CrearCandidato(objCandidato, objFile);
                    return Json(creacion, JsonRequestBehavior.AllowGet);
                    #endregion
                }
                #endregion
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarCandidato(int idCandidato)
        {
            try
            {
                result = new Dictionary<string, object>();
                if (idCandidato > 0)
                {
                    #region SE ELIMINA AL CANDIDATO
                    bool esEliminarCandidato = reclutamientoFS.EliminarCandidato(idCandidato);
                    if (esEliminarCandidato)
                        result.Add(SUCCESS, true);
                    else
                        throw new Exception("Ocurrió un error al eliminar al candidato");
                    #endregion
                }
                else
                    throw new Exception("Ocurrió un error al eliminar al candidato");
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillFiltroCboPuestosDisponibles()
        {
            result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstPuestosDisponibles = reclutamientoFS.FillFiltroCboPuestosDisponibles();
                result.Add(ITEMS, lstPuestosDisponibles);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEntrevistaInicial(EntrevistaInicialDTO objEntrevistaInicialDTO)
        {
            result = new Dictionary<string, object>();
            try
            {
                if (objEntrevistaInicialDTO.idCandidato > 0)
                {
                    EntrevistaInicialDTO objEntrevistaInicial = reclutamientoFS.GetEntrevistaInicial(objEntrevistaInicialDTO);
                    result.Add("objEntrevistaInicial", objEntrevistaInicial);
                    result.Add(SUCCESS, true);
                }
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result);
        }

        public ActionResult CrearEditarEntrevistaInicial(EntrevistaInicialDTO objEntrevistaInicialDTO)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                if (objEntrevistaInicialDTO.idCandidato > 0)
                {
                    Dictionary<string, object> objEntrevistaInicial = reclutamientoFS.CrearEditarEntrevistaInicial(objEntrevistaInicialDTO);
                    if (objEntrevistaInicial.Count() > 0)
                    {
                        result.Add("esCrearEditar", objEntrevistaInicial["esCrearEditar"]);
                        result.Add("strMensaje", objEntrevistaInicial["strMensaje"]);
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

        public ActionResult DescargarArchivoCV(int candidatoID)
        {
            var resultadoTupla = reclutamientoFS.DescargarArchivoCV(candidatoID);

            if (resultadoTupla != null)
            {
                string nombreArchivo = resultadoTupla.Item2;
                string tipo = MimeMapping.GetMimeMapping(nombreArchivo);

                var fileStreamResult = new FileStreamResult(resultadoTupla.Item1, tipo);
                fileStreamResult.FileDownloadName = nombreArchivo;

                return fileStreamResult;
            }
            else
            {
                return View(RUTA_VISTA_ERROR_DESCARGA);
            }
        }

        public ActionResult GetUsuarioEntrevistaActual()
        {
            return Json(reclutamientoFS.GetUsuarioEntrevistaActual(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getAutoCompleteEmpleadosBaja(string term)
        {
            var items = reclutamientoFS.getAutoCompleteEmpleadosBaja(term);
            var filteredItems = items.Select(x => new { id = x.clave_empleado, label = x.Nombre });
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getAutoCompleteCandidatos(string term)
        {
            var items = reclutamientoFS.getAutoCompleteCandidatos(term);
            var filteredItems = items.Select(x => new { id = x.clave_empleado, label = x.Nombre });
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region FASES
        public ActionResult GetFases()
        {
            result = new Dictionary<string, object>();
            try
            {
                List<FasesDTO> lstFases = reclutamientoFS.GetFases();
                result.Add("lstFases", lstFases);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarFase(tblRH_REC_Fases objFase)
        {
            result = new Dictionary<string, object>();
            try
            {
                #region SE CREA/ACTUALIZA LA FASE
                if (objFase.id > 0)
                {
                    #region SE ACTUALIZA LA FASE
                    bool esActualizarFase = reclutamientoFS.ActualizarFase(objFase);
                    if (!esActualizarFase)
                        throw new Exception("Ocurrió un error al actualizar la fase.");
                    else
                        result.Add(SUCCESS, true);
                    #endregion
                }
                else
                {
                    #region SE CREA LA FASE
                    bool esCrearFase = reclutamientoFS.CrearFase(objFase);
                    if (!esCrearFase)
                        throw new Exception("Ocurrió un error al crear la fase.");
                    else
                        result.Add(SUCCESS, true);
                    #endregion
                }
                #endregion
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarFase(int idFase)
        {
            try
            {
                result = new Dictionary<string, object>();
                if (idFase > 0)
                {
                    #region SE ELIMINA LA FASE
                    bool esEliminarFase = reclutamientoFS.EliminarFase(idFase);
                    if (esEliminarFase)
                        result.Add(SUCCESS, true);
                    else
                        throw new Exception("Ocurrió un error al eliminar la fase");
                    #endregion
                }
                else
                    throw new Exception("Ocurrió un error al eliminar la fase");
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region FASES RELACIÓN ACTIVIDADES
        public ActionResult GetActividades(SegCandidatosDTO objSegCandidatosDTO)
        {

            return Json(reclutamientoFS.GetActividades(objSegCandidatosDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarActividad(tblRH_REC_Actividades objActividad)
        {
            result = new Dictionary<string, object>();
            try
            {
                #region SE CREA/ACTUALIZA LA ACTIVIDAD
                if (objActividad.id > 0)
                {
                    #region SE ACTUALIZA LA ACTIVIDAD
                    bool esActualizarActividad = reclutamientoFS.ActualizarActividad(objActividad);
                    if (!esActualizarActividad)
                        throw new Exception("Ocurrió un error al actualizar la actividad.");
                    else
                        result.Add(SUCCESS, true);
                    #endregion
                }
                else
                {
                    #region SE CREA LA ACTIVIDAD
                    bool esCrearActividad = reclutamientoFS.CrearActividad(objActividad);
                    if (!esCrearActividad)
                        throw new Exception("Ocurrió un error al crear la actividad.");
                    else
                        result.Add(SUCCESS, true);
                    #endregion
                }
                #endregion
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarActividad(int idActividad)
        {
            try
            {
                result = new Dictionary<string, object>();
                if (idActividad > 0)
                {
                    #region SE ELIMINA LA ACTIVIDAD
                    bool esEliminarActividad = reclutamientoFS.EliminarActividad(idActividad);
                    if (esEliminarActividad)
                        result.Add(SUCCESS, true);
                    else
                        throw new Exception("Ocurrió un error al eliminar la actividad");
                    #endregion
                }
                else
                    throw new Exception("Ocurrió un error al eliminar la actividad");
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AsignarEncargadoFase(ActividadesDTO objActividadDTO)
        {
            result = new Dictionary<string, object>();
            try
            {
                if (objActividadDTO.idUsuarioEncargado > 0)
                {
                    bool esAsignarEncargado = reclutamientoFS.AsignarEncargadoFase(objActividadDTO);
                    if (!esAsignarEncargado)
                        throw new Exception("Ocurrió un error al asignar al encargado.");
                    else
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
        #endregion

        #region PUESTOS RELACIONADOS A FASES
        public ActionResult GetPuestosRelFase(int idFase)
        {
            result = new Dictionary<string, object>();
            try
            {
                List<PuestosRelFasesDTO> lstPuestosRelFase = reclutamientoFS.GetPuestosRelFase(idFase);
                result.Add("lstPuestosRelFase", lstPuestosRelFase);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearPuestoRelFase(tblRH_REC_PuestosRelFases objPuestoRelFase)
        {
            result = new Dictionary<string, object>();
            try
            {
                bool existePuestoEnFase = reclutamientoFS.ExistePuestoEnFase(objPuestoRelFase.idFase, objPuestoRelFase.idPuesto);
                if (existePuestoEnFase)
                {
                    bool esCrearPuestoRelFase = reclutamientoFS.CrearPuestoRelFase(objPuestoRelFase);
                    if (!esCrearPuestoRelFase)
                        throw new Exception("Ocurrió un error al crear la relación del puesto con la fase.");
                    else
                        result.Add(SUCCESS, true);
                }
                else
                    throw new Exception("El puesto seleccionado ya se encuentra registrado en la fase.");
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarPuestoRelFase(int idPuestoRelFase)
        {
            try
            {
                result = new Dictionary<string, object>();
                if (idPuestoRelFase > 0)
                {
                    #region SE ELIMINA LA RELACIÓN DEL PUESTO CON LA FASE
                    bool esEliminarPuestoRelFase = reclutamientoFS.EliminarPuestoRelFase(idPuestoRelFase);
                    if (esEliminarPuestoRelFase)
                        result.Add(SUCCESS, true);
                    else
                        throw new Exception("Ocurrió un error al eliminar la relación del puesto con la fase.");
                    #endregion
                }
                else
                    throw new Exception("Ocurrió un error al eliminar la relación del puesto con la fase.");
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region SEGUIMIENTO DE CANDIDATOS
        public ActionResult GetSegCandidatos(SegCandidatosDTO objFiltro)
        {
            result = new Dictionary<string, object>();
            //try
            //{
            List<SegCandidatosDTO> lstSegCandidatos = reclutamientoFS.GetSegCandidatos(objFiltro);
            result.Add("lstSegCandidatos", lstSegCandidatos);
            result.Add(SUCCESS, true);
            //}
            //catch (Exception e)
            //{
            //    result.Add(MESSAGE, e.Message);
            //    result.Add(SUCCESS, false);
            //}

            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;

            return json;
        }

        public ActionResult GetSegDetCandidatos(SegCandidatosDTO objFiltro)
        {
            result = new Dictionary<string, object>();
            try
            {
                List<SegCandidatosDTO> lstSegDetCandidatos = reclutamientoFS.GetSegDetCandidatos(objFiltro);
                result.Add("lstSegDetCandidatos", lstSegDetCandidatos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarSegCandidatos(SegCandidatosDTO objSegCandidatoDTO)
        {
            result = new Dictionary<string, object>();
            try
            {
                #region SE CREAR/ACTUALIZA EL SEGUIMIENTO DE UNA ACTIVIDAD DEL CANDIDATO SELECCIONADO
                bool esCrearActualizar = reclutamientoFS.CrearEditarSegCandidatos(objSegCandidatoDTO);
                if (!esCrearActualizar)
                    throw new Exception("Ocurrió un error al actualizar la información");
                else
                    result.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarComentarioActividad(SegCandidatosDTO objSegCandidatoDTO)
        {
            result = new Dictionary<string, object>();
            try
            {
                bool esCrearActualizar = reclutamientoFS.CrearEditarComentarioActividad(objSegCandidatoDTO);
                if (!esCrearActualizar)
                    throw new Exception("Ocurrió un error al actualizar la información");
                else
                    result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetObservacionActividad(SegCandidatosDTO objSegCandidatoDTO)
        {
            result = new Dictionary<string, object>();
            try
            {
                string strObservacion = reclutamientoFS.GetObservacionActividad(objSegCandidatoDTO);
                result.Add("strObservacion", strObservacion);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearArchivoActividad(List<HttpPostedFileBase> lstFiles)
        {
            result = new Dictionary<string, object>();
            try
            {
                SegCandidatosDTO objSegCandidatoDTO = JsonUtils.convertJsonToNetObject<SegCandidatosDTO>(Request.Form["objSegCandidatoDTO"], "es-MX");

                bool esCrearActualizar = reclutamientoFS.CrearArchivoActividad(objSegCandidatoDTO, lstFiles);
                if (!esCrearActualizar)
                    throw new Exception("Ocurrió un error al actualizar la información");
                else
                    result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetArchivosActividadesRelFase(SegCandidatosDTO objSegCandidatoDTO)
        {
            result = new Dictionary<string, object>();
            try
            {
                List<ArchivosDTO> lstArchivos = reclutamientoFS.GetArchivosActividadesRelFase(objSegCandidatoDTO);
                result.Add("lstArchivos", lstArchivos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarArchivoActividad(int idArchivo)
        {
            try
            {
                result = new Dictionary<string, object>();
                if (idArchivo > 0)
                {
                    #region SE ELIMINA EL ARCHIVO
                    bool esEliminarArchivo = reclutamientoFS.EliminarArchivoActividad(idArchivo);
                    if (esEliminarArchivo)
                        result.Add(SUCCESS, true);
                    else
                        throw new Exception("Ocurrió un error al eliminar el archivo.");
                    #endregion
                }
                else
                    throw new Exception("Ocurrió un error al eliminar el archivo.");
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarCalificacionActividad(SegCandidatosDTO objSegCandidatoDTO)
        {
            result = new Dictionary<string, object>();
            try
            {
                bool esCrearActualizar = reclutamientoFS.CrearEditarCalificacionActividad(objSegCandidatoDTO);
                if (!esCrearActualizar)
                    throw new Exception("Ocurrió un error al registrar la calificación.");
                else
                    result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFasesAutorizadas(int idPuesto)
        {
            return Json(reclutamientoFS.GetFasesAutorizadas(idPuesto), JsonRequestBehavior.AllowGet);
        }

        public ActionResult NotificarActividad(int idCandidato, int idActividad, int estatus, int? idNotificante)
        {
            return Json(reclutamientoFS.NotificarActividad(idCandidato, idActividad, estatus, idNotificante), JsonRequestBehavior.AllowGet);
        }

        public FileResult DescargarArchivoEvidencia()
        {
            try
            {
                int file_id = Convert.ToInt32(Request.QueryString["file_id"]);

                var ruta = reclutamientoFS.GetArchivoEvidencia(file_id);

                return File(ruta, "multipart/form-data", Path.GetFileName(ruta));
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public ActionResult getCatUsuariosGeneral(string term)
        {
            var resultado = reclutamientoFS.getCatUsuariosGeneral(term);
            var filteredItems = resultado.Select(x => new { id = x.clave_empleado, label = x.Nombre });
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetNotiEstatusActividad(int idCandidato, int idFase, int idActividad)
        {
            return Json(reclutamientoFS.SetNotiEstatusActividad(idCandidato, idFase, idActividad));
        }
        #endregion

        #region EMPLEADOS EK
        public ActionResult GetEmpleadosEK(List<string> cc, List<string> lstEstatusEmpleado)
        {
            result = new Dictionary<string, object>();
            try
            {
                List<EmpleadosDTO> lstEmpleadosEK = reclutamientoFS.GetEmpleadosEK(cc, lstEstatusEmpleado);
                result.Add("lstEmpleadosEK", lstEmpleadosEK);
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

        public ActionResult CambiarContratable(string claveEmpleado, string esContratable)
        {
            return Json(reclutamientoFS.CambiarContratable(claveEmpleado, esContratable), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarInformacionEmpleado(EmpleadosDTO objEmpleadoDTO, GeneralesContactoDTO objGenContactoDTO, BeneficiariosDTO objBeneficiariosDTO, ContEmergenciasDTO objContEmergenciasDTO, CompaniaDTO objCompaniaDTO, List<FamiliaresDTO> lstFamiliares, UniformesDTO objUniforme, NuevoTabuladorDTO objTabulador, ContratoDTO objContrato, InfoEmpleadoPeruDTO objDatosPeru)
        {
            result = new Dictionary<string, object>();
            try
            {
                DateTime fechaAlta = DateTime.Parse(objEmpleadoDTO.str_fecha_alta);
                objEmpleadoDTO.fecha_alta = fechaAlta;
                DateTime fechaNacimiento = DateTime.Parse(objEmpleadoDTO.fecha_nac_string);
                objEmpleadoDTO.fecha_nac = fechaNacimiento;
                #region SE VERIFICA QUE SE ENCUENTREN LOS DATOS OBLIGATORIOS

                #region DATOS EMPLEADO
                if ((int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Peru || (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Colombia)
                {

                }
                else
                {
                    if (string.IsNullOrEmpty(objEmpleadoDTO.rfc)) throw new Exception("Es necesario llenar el rfc.");
                    if (string.IsNullOrEmpty(objEmpleadoDTO.curp)) throw new Exception("Es necesario llenar la curp.");
                    if (string.IsNullOrEmpty(objCompaniaDTO.nss)) throw new Exception("Es necesario llenar el NSS.");
                }
                if (string.IsNullOrEmpty(objEmpleadoDTO.nombre)) throw new Exception("Es necesario llenar el nombre del empleado.");
                if (string.IsNullOrEmpty(objEmpleadoDTO.ape_paterno)) throw new Exception("Es necesario llenar el apellido paterno");
                if (objEmpleadoDTO.clave_pais_nac <= 0) throw new Exception("Es necesario llenar el pais.");
                if (objEmpleadoDTO.clave_estado_nac <= 0) throw new Exception("Es necesario llenar el estado.");
                if (objEmpleadoDTO.clave_ciudad_nac <= 0) throw new Exception("Es necesario llenar la ciudad.");
                if (objEmpleadoDTO.fecha_nac == null) throw new Exception("Es necesario llenar la fecha de nacimiento.");
                if (objEmpleadoDTO.fecha_alta == null) throw new Exception("Es necesario llenar la fecha de alta.");
                if (string.IsNullOrEmpty(objEmpleadoDTO.localidad_nacimiento)) throw new Exception("Es necesario llenar la localidad de nacimiento.");
                if (objEmpleadoDTO.sexo == null) throw new Exception("Es necesario llenar el campo de sexo.");
                #endregion

                #region COMPAÑIA
                if (objCompaniaDTO.requisicion <= 0) throw new Exception("Es necesario llenar la requisicion.");
                if (objCompaniaDTO.id_regpat <= 0) throw new Exception("Es necesario llenar el registro patronal.");
                if (string.IsNullOrEmpty(objCompaniaDTO.cc_contable)) throw new Exception("Es necesario llenar el cc.");
                if (objCompaniaDTO.puesto <= 0) throw new Exception("Es necesario llenar el puesto.");
                if (objCompaniaDTO.duracion_contrato <= 0) throw new Exception("Es necesario llenar la duracion de contrato.");
                if (objCompaniaDTO.jefe_inmediato <= 0) throw new Exception("Es necesario llenar el jefe inmediato.");
                if (objCompaniaDTO.autoriza <= 0) throw new Exception("Es necesario llenar el autorizante.");
                //if (objCompaniaDTO.usuario_compras <= 0) throw new Exception("Es necesario llenar los campos obligatorios.");
                if (objCompaniaDTO.clave_depto <= 0) throw new Exception("Es necesario llenar el departamento.");
                //if (objCompaniaDTO.unidad_medica <= 0) throw new Exception("Es necesario llenar los campos obligatorios."); //SE CONSULTA QUE UNIDAD MEDICA TIENE RELACIÓN CON EL EMPLEADO EN BASE A SU CODIGO POSTAL
                //if (objCompaniaDTO.tipo_formula_imss == null) throw new Exception("Es necesario llenar los campos obligatorios.");
                #endregion

                #endregion

                #region SE CREA/ACTUALIZA AL EMPLEADO
                return Json(reclutamientoFS.CrearEditarInformacionEmpleado(objEmpleadoDTO, objGenContactoDTO, objBeneficiariosDTO, objContEmergenciasDTO, objCompaniaDTO, lstFamiliares, objUniforme, objTabulador, objContrato, objDatosPeru), JsonRequestBehavior.AllowGet);
                #endregion
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarEmpleado(int claveEmpleado)
        {
            try
            {
                result = new Dictionary<string, object>();
                if (claveEmpleado > 0)
                {
                    #region SE ELIMINA AL EMPLEADO
                    bool esEliminar = reclutamientoFS.EliminarEmpleado(claveEmpleado);
                    if (esEliminar)
                        result.Add(SUCCESS, true);
                    else
                        throw new Exception("Ocurrió un error al eliminar al empleado.");
                    #endregion
                }
                else
                    throw new Exception("Ocurrió un error al eliminar al empleado.");
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFamiliares(int clave_empleado)
        {
            result = new Dictionary<string, object>();
            try
            {
                List<FamiliaresDTO> lstFamiliaresEK = reclutamientoFS.GetFamiliares(clave_empleado);
                result.Add("lstFamiliaresEK", lstFamiliaresEK);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetContratos(int clave_empleado)
        {
            result = new Dictionary<string, object>();
            try
            {
                var listaContratosEK = reclutamientoFS.GetContratos(clave_empleado);
                result.Add("data", listaContratosEK);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarInformacionFamiliar(FamiliaresDTO objFamiliarDTO)
        {
            result = new Dictionary<string, object>();
            try
            {
                #region SE CREA/ACTUALIZA AL FAMILIAR
                //bool esCrearActualizar = reclutamientoFS.CrearEditarInformacionFamiliar(objFamiliarDTO);
                //if (!esCrearActualizar)
                //{
                //    if (!objFamiliarDTO.esActualizar)
                //        throw new Exception("Ocurrió un error al guardar la información.");
                //    else
                //        throw new Exception("Ocurrió un error al actualizar la información.");
                //}
                //else
                //    result.Add(SUCCESS, true);

                return Json(reclutamientoFS.CrearEditarInformacionFamiliar(objFamiliarDTO), JsonRequestBehavior.AllowGet);
                #endregion
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EliminarContrato(int id_contrato_empleado)
        {
            return Json(reclutamientoFS.EliminarContrato(id_contrato_empleado));
        }

        public ActionResult EliminarFamiliar(int idFamiliar, int clave_empleado)
        {
            try
            {
                result = new Dictionary<string, object>();
                //if (idFamiliar > 0 && clave_empleado > 0)
                //{
                //    #region SE ELIMINA AL FAMILIAR
                //    bool esEliminar = reclutamientoFS.EliminarFamiliar(idFamiliar, clave_empleado);
                //    if (esEliminar)
                //        result.Add(SUCCESS, true);
                //    else
                //        throw new Exception("Ocurrió un error al eliminar al familiar.");
                //    #endregion
                //}
                //else
                //    throw new Exception("Ocurrió un error al eliminar al familiar.");

                #region SE ELIMINA AL FAMILIAR
                bool esEliminar = reclutamientoFS.EliminarFamiliar(idFamiliar, clave_empleado);
                if (esEliminar)
                    result.Add(SUCCESS, true);
                else
                    throw new Exception("Ocurrió un error al eliminar al familiar.");
                #endregion


            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboCandidatosAprobados()
        {
            result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstCandidatosAprobados = reclutamientoFS.FillCboCandidatosAprobados();
                result.Add(ITEMS, lstCandidatosAprobados);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboParentesco()
        {
            result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstParentesco = reclutamientoFS.FillCboParentesco();
                result.Add(ITEMS, lstParentesco);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboTipoSangre()
        {
            result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstTipoSangre = reclutamientoFS.FillCboTipoSangre();
                result.Add(ITEMS, lstTipoSangre);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboTipoCasa()
        {
            result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstTipoCasa = reclutamientoFS.FillCboTipoCasa();
                result.Add(ITEMS, lstTipoCasa);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosEmpleadoDocumentos(int claveEmpleado)
        {
            result = new Dictionary<string, object>();
            try
            {
                List<dynamic> lstDatosEmpleado = reclutamientoFS.GetDatosEmpleadoDocumentos(claveEmpleado);
                result.Add("lstDatosEmpleado", lstDatosEmpleado);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUniformes(int claveEmpleado)
        {
            result = new Dictionary<string, object>();
            try
            {
                UniformesDTO lstUniformes = reclutamientoFS.GetUniformes(claveEmpleado);
                result.Add("lstUniformes", lstUniformes);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarUniforme(UniformesDTO objUniforme)
        {
            //result = new Dictionary<string, object>();
            //try
            //{
            //    #region SE CREA/ACTUALIZA UNIFORME
            //    bool esCrearActualizar = reclutamientoFS.CrearEditarUniforme(objUniforme);
            //    if (!esCrearActualizar)
            //    {
            //        if (!objUniforme.esActualizar)
            //            throw new Exception("Ocurrió un error al guardar la información.");
            //        else
            //            throw new Exception("Ocurrió un error al actualizar la información.");
            //    }
            //    else
            //        result.Add(SUCCESS, true);
            //    #endregion


            //}
            //catch (Exception e)
            //{
            //    result.Add(MESSAGE, e.Message);
            //    result.Add(SUCCESS, false);
            //}
            //return Json(result, JsonRequestBehavior.AllowGet);
            return Json(reclutamientoFS.CrearEditarUniforme(objUniforme), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarUniforme(int idUniforme)
        {
            try
            {
                result = new Dictionary<string, object>();
                if (idUniforme > 0)
                {
                    #region SE ELIMINA EL UNIFORME DEL EMPLEADO
                    bool esEliminar = reclutamientoFS.EliminarUniforme(idUniforme);
                    if (esEliminar)
                        result.Add(SUCCESS, true);
                    else
                        throw new Exception("Ocurrió un error al eliminar el registro.");
                    #endregion
                }
                else
                    throw new Exception("Ocurrió un error al eliminar el registro.");
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetArchivoExamenMedico(int claveEmpleado)
        {
            result = new Dictionary<string, object>();
            try
            {
                List<ArchivosDTO> lstArchivos = reclutamientoFS.GetArchivoExamenMedico(claveEmpleado);
                result.Add("lstArchivos", lstArchivos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearArchivoExamenMedico(HttpPostedFileBase objFile)
        {
            result = new Dictionary<string, object>();
            try
            {
                ArchivosDTO objArchivoDTO = JsonUtils.convertJsonToNetObject<ArchivosDTO>(Request.Form["objArchivoDTO"], "es-MX");

                bool esCrearActualizar = reclutamientoFS.CrearArchivoExamenMedico(objArchivoDTO, objFile);
                if (!esCrearActualizar)
                    throw new Exception("Ocurrió un error al registrar el archivo.");
                else
                    result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarExamenMedico(int idExamenMedico)
        {
            try
            {
                result = new Dictionary<string, object>();
                if (idExamenMedico > 0)
                {
                    #region SE ELIMINA EL EXAMEN MEDICO
                    bool esEliminar = reclutamientoFS.EliminarExamenMedico(idExamenMedico);
                    if (esEliminar)
                        result.Add(SUCCESS, true);
                    else
                        throw new Exception("Ocurrió un error al eliminar el archivo medico.");
                    #endregion
                }
                else
                    throw new Exception("Ocurrió un error al eliminar el archivo medico.");
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetArchivoMaquinaria(int claveEmpleado)
        {
            result = new Dictionary<string, object>();
            try
            {
                List<ArchivosDTO> lstArchivos = reclutamientoFS.GetArchivoMaquinaria(claveEmpleado);
                result.Add("lstArchivos", lstArchivos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearArchivoMaquinaria(HttpPostedFileBase objFile)
        {
            result = new Dictionary<string, object>();
            try
            {
                ArchivosDTO objArchivoDTO = JsonUtils.convertJsonToNetObject<ArchivosDTO>(Request.Form["objArchivoDTO"], "es-MX");

                bool esCrearActualizar = reclutamientoFS.CrearArchivoMaquinaria(objArchivoDTO, objFile);
                if (!esCrearActualizar)
                    throw new Exception("Ocurrió un error al registrar el archivo.");
                else
                    result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarMaquinaria(int idMaquinaria)
        {
            try
            {
                result = new Dictionary<string, object>();
                if (idMaquinaria > 0)
                {
                    #region SE ELIMINA EL ARCHIVO MAQUINARIA
                    bool esEliminar = reclutamientoFS.EliminarMaquinaria(idMaquinaria);
                    if (esEliminar)
                        result.Add(SUCCESS, true);
                    else
                        throw new Exception("Ocurrió un error al eliminar el archivo de maquinaria.");
                    #endregion
                }
                else
                    throw new Exception("Ocurrió un error al eliminar el archivo de maquinaria.");
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTabuladores(TabuladoresDTO objTabDTO)
        {
            result = new Dictionary<string, object>();
            try
            {
                List<TabuladoresDTO> lstNomina = reclutamientoFS.GetTabuladores(objTabDTO);

                int clave_empleado = Convert.ToInt32(objTabDTO.clave_empleado);
                bool esOcultarSalarioSoloGerencia = reclutamientoFS.UsuarioOcultarSalariosGernecia(clave_empleado);

                result.Add("lstNomina", lstNomina);
                result.Add("esOcultarSalarioSoloGerencia", esOcultarSalarioSoloGerencia);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearTabuladorPuesto(TabuladoresPuestoDTO objTabuladorPuestoDTO)
        {
            return Json(reclutamientoFS.CrearTabuladorPuesto(objTabuladorPuestoDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearTabulador(TabuladoresDTO objTabuladorDTO)
        {
            return Json(reclutamientoFS.CrearTabulador(objTabuladorDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CambiarFechaCambioTabulador(int id, DateTime fecha_cambio, int claveEmpleado)
        {
            return Json(reclutamientoFS.CambiarFechaCambioTabulador(id, fecha_cambio, claveEmpleado), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboBancos()
        {
            result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstBancos = reclutamientoFS.FillCboBancos();
                result.Add(ITEMS, lstBancos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetReporteSegCandidatos(ReporteSegCandidatosDTO objFiltroDTO)
        {
            Dictionary<string, object> objResult = new Dictionary<string, object>();
            try
            {
                var lstReporteIndicadores = reclutamientoFS.GetReporteSegCandidatos(objFiltroDTO);
                if (lstReporteIndicadores.Count() > 0)
                {
                    result.Add("lstCandidatosDTO", lstReporteIndicadores["lstCandidatosDTO"]);
                    result.Add("lstFasesDTO", lstReporteIndicadores["lstFasesDTO"]);
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

        public ActionResult GetDatosActualizarEmpleado(int claveEmpleado, bool esReingresoEmpleado)
        {
            return Json(reclutamientoFS.GetDatosActualizarEmpleado(claveEmpleado, esReingresoEmpleado), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarContratoFirmado(int idArchivo, int claveEmpleado)
        {
            try
            {
                result = new Dictionary<string, object>();
                if (idArchivo > 0 && claveEmpleado > 0)
                {
                    #region SE ELIMINA EL CONTRATO FIRMADO DEL EMPLEADO
                    bool esEliminar = reclutamientoFS.EliminarContratoFirmado(idArchivo, claveEmpleado);
                    if (esEliminar)
                        result.Add(SUCCESS, true);
                    else
                        throw new Exception("Ocurrió un error al eliminar el contrato.");
                    #endregion
                }
                else
                    throw new Exception("Ocurrió un error al eliminar el contrato.");
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetContratosFirmados(int claveEmpleado)
        {
            result = new Dictionary<string, object>();
            try
            {
                List<ArchivosDTO> lstArchivos = reclutamientoFS.GetContratosFirmados(claveEmpleado);
                result.Add("lstArchivos", lstArchivos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GuardarExcelActoCondicionCargaMasiva()
        {
            if (Request.Files.Count > 0)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase _archivoExcel = Request.Files[i];
                    result = reclutamientoFS.GuardarExcelActoCondicionCargaMasiva(_archivoExcel);
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region METODOS PARA DESCARGAR EXCEL, SOLAMENTE EMPLEADOS CON ESTATUS PENDIENTE
        public ActionResult ExportarExcel(List<string> _lstClaveEmpleados) // PROBAR CODIGO
        {
            result = new Dictionary<string, object>();
            try
            {
                Session["ListaLayoutAltasRHDTO"] = reclutamientoFS.GetEmpleadosLayoutAlta(_lstClaveEmpleados);
                Session["setListEmpleados"] = _lstClaveEmpleados;
                //GetArchivoLayoutAltas();

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public FileResult GetArchivoLayoutAltas()
        {
            try
            {
                MemoryStream memStream = new MemoryStream();
                StreamWriter streamWriter = new StreamWriter(memStream);
                string Cadena = "EMP_TRAB|EMP_ALTA|EMP_NOM|EMP_RFC|EMP_CURP|EMP_CC|EMP_DP|EMP_SM|EMP_SEXO|EMP_NAC_EF|EMP_NAC_FECHA|EMP_ULTIMO_REINGRESO|EMP_AGUINALDO|EMP_VACACIONES|EMP_NOMINA|EMP_IMSS_TIPO|EMP_SUCURSAL|EMP_CL|EMP_TURNO|EMP_PUESTO|EMP_PUESTO_DESCRIPCION|EMP_NSS|EMP_UMF|EMP_FSUELDO|EMP_SUELDO|EMP_SUELDO1|EMP_SUELDO2|EMP_SDI|EMP_DIR_CALLE|EMP_DIR_NO|EMP_DIR_COLONIA|EMP_DIR_CP|EMP_DIR_MUNICIPIO|EMP_DIR_POBLACION|EMP_DIR_ESTADO|EMP_DIR_TELEFONO|EMP_DIR_CELULAR|NOM_BENEF|PARENT_BENEF|BENEF_NAC_FECHA|EMP_MAIL";
                streamWriter.WriteLine(Cadena);
                List<Core.DTO.RecursosHumanos.Reclutamientos.LayoutAltasRHDTO> listaIF = (List<Core.DTO.RecursosHumanos.Reclutamientos.LayoutAltasRHDTO>)Session["ListaLayoutAltasRHDTO"];
                var EmpleadosSEleccionados = (List<string>)Session["setListEmpleados"];
                DataTable dtLayout = new DataTable();
                var EncabezadoTabla = Cadena.Split('|');

                for (int i = 0; i < EncabezadoTabla.Length; i++)
                {
                    dtLayout.Columns.Add(EncabezadoTabla[i]);
                }

                foreach (var listaRow in listaIF.Where(x => EmpleadosSEleccionados.Contains(x.EMP_TRAB)))
                {
                    int dias = listaRow.dias, diasSueldo = 0;
                    switch (dias)
                    {
                        case 1:
                            diasSueldo = 7;
                            break;
                        case 2:
                            diasSueldo = 10;
                            break;
                        case 3:
                            diasSueldo = 14;
                            break;
                        case 4:
                            diasSueldo = 15;
                            break;
                        case 5:
                            diasSueldo = 30;
                            break;
                        default:
                            break;
                    }
                    if (diasSueldo != 0)
                    {
                        double saldodiario = Math.Round((listaRow.EMP_SUELDO == "0" ? 0 : Convert.ToDouble(listaRow.EMP_SUELDO) / diasSueldo), 2);
                        double SDI = Math.Round(saldodiario * 1.0493, 2);
                        dtLayout.Rows.Add(
                            listaRow.EMP_TRAB,
                            listaRow.EMP_ALTA,
                            listaRow.EMP_NOM,
                            listaRow.EMP_RFC,
                            listaRow.EMP_CURP,
                            listaRow.EMP_CC,
                            listaRow.EMP_DP,
                            listaRow.EMP_SM,
                            listaRow.EMP_SEXO,
                            listaRow.EMP_NAC_EF,
                            listaRow.EMP_NAC_FECHA,
                            listaRow.EMP_ULTIMO_REINGRESO,
                            listaRow.EMP_AGUINALDO,
                            listaRow.EMP_VACACIONES,
                            listaRow.EMP_NOMINA,
                            listaRow.EMP_IMSS_TIPO,
                            listaRow.EMP_SUCURSAL,
                            listaRow.EMP_CL,
                            listaRow.EMP_TURNO,
                            listaRow.EMP_PUESTO,
                            listaRow.EMP_PUESTO_DESCRIPCION,
                            listaRow.EMP_NSS,
                            listaRow.EMP_UMF,
                            listaRow.EMP_FSUELDO,
                            saldodiario,
                            listaRow.EMP_SUELDO1,
                            listaRow.EMP_SUELDO2,
                            SDI,
                            listaRow.EMP_DIR_CALLE,
                            listaRow.EMP_DIR_NO,
                            listaRow.EMP_DIR_COLONIA,
                            listaRow.EMP_DIR_CP,
                            listaRow.EMP_DIR_MUNICIPIO,
                            listaRow.EMP_DIR_POBLACION,
                            listaRow.EMP_DIR_ESTADO,
                            listaRow.EMP_DIR_TELEFONO,
                            listaRow.EMP_DIR_CELULAR,
                            listaRow.NOM_BENEF,
                            listaRow.PARENT_BENEF,
                            listaRow.BENEF_NAC_FECHA,
                            listaRow.EMP_MAIL
                       );
                    }
                }
                WriteExcelWithNPOI(dtLayout, "xls");
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private MemoryStream WriteExcelWithNPOI(DataTable dt, String extension)
        {

            try
            {
                IWorkbook workbook;

                if (extension == "xlsx")
                {
                    workbook = new XSSFWorkbook();
                }
                else if (extension == "xls")
                {
                    workbook = new HSSFWorkbook();
                }
                else
                {
                    throw new Exception("This format is not supporte<d");
                }

                ISheet sheet1 = workbook.CreateSheet("CONSTRUPLAN");

                //make a header row
                IRow row1 = sheet1.CreateRow(0);

                for (int j = 0; j < dt.Columns.Count; j++)
                {

                    ICell cell = row1.CreateCell(j);
                    String columnName = dt.Columns[j].ToString();
                    cell.SetCellValue(columnName);
                }

                //loops through data
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    IRow row = sheet1.CreateRow(i + 1);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {

                        ICell cell = row.CreateCell(j);
                        String columnName = dt.Columns[j].ToString();
                        cell.SetCellValue(dt.Rows[i][columnName].ToString());
                    }
                }

                using (var exportData = new MemoryStream())
                {
                    Response.Clear();
                    workbook.Write(exportData);
                    if (extension == "xlsx") //xlsx file format
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "LayoutALTARH - CONSTRUPLAN.xlsx"));
                        Response.BinaryWrite(exportData.ToArray());
                    }
                    else if (extension == "xls")  //xls file format
                    {
                        Response.ContentType = "application/vnd.ms-excel";
                        Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "LayoutALTARH - CONSTRUPLAN.xls"));
                        Response.BinaryWrite(exportData.GetBuffer());
                    }
                    Response.End();
                    return exportData;
                }
            }
            catch (Exception e)
            {

                return null;
            }


        }
        #endregion

        public ActionResult GetDatosCandidatoAprobado(int idCandidatoAprobado)
        {
            return Json(reclutamientoFS.GetDatosCandidatoAprobado(idCandidatoAprobado), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReingresarEmpleado(int clave_empleado, int requisicion_id)
        {
            return Json(reclutamientoFS.ReingresarEmpleado(clave_empleado, requisicion_id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInformacionRequisicion(int requisicion_id)
        {
            return Json(reclutamientoFS.GetInformacionRequisicion(requisicion_id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChecarPermisoTabuladorLibre()
        {
            return Json(reclutamientoFS.ChecarPermisoTabuladorLibre(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetIDUsuarioEK()
        {
            return Json(reclutamientoFS.GetIDUsuarioEK(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CambiarEstatusEmpleado(int claveEmpleado, string status)
        {
            return Json(reclutamientoFS.CambiarEstatusEmpleado(claveEmpleado, status), JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddContratos(int idEmpleado, int tipoDuracionContrato)
        {
            return Json(reclutamientoFS.AddContratos(idEmpleado, tipoDuracionContrato), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDocs(int? clave_empleado, int? id_candidato)
        {
            return Json(reclutamientoFS.GetDocs(clave_empleado, id_candidato), JsonRequestBehavior.AllowGet);
        }

        #region FOTO DEL EMPLEADO
        public ActionResult GuardarFotoEmpleado(List<HttpPostedFileBase> objFotoEmpleado, ArchivosDTO objDTO)
        {
            return Json(reclutamientoFS.GuardarFotoEmpleado(objFotoEmpleado, objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFotoEmpleado(ArchivosDTO objDTO)
        {
            return Json(reclutamientoFS.GetFotoEmpleado(objDTO), JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult GetHistorialCC(int clave_empleado)
        {
            return Json(reclutamientoFS.GetHistorialCC(clave_empleado), JsonRequestBehavior.AllowGet);
        }

        public JsonResult AutorizacionMultiple(List<int> claveEmpleados)
        {
            return Json(reclutamientoFS.AutorizacionMultiple(claveEmpleados));
        }

        public JsonResult GetHeaderEmpleados()
        {
            return Json(reclutamientoFS.GetHeaderEmpleados(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckEmpleado(string curp, string rfc, string nss)
        {
            return Json(reclutamientoFS.CheckEmpleado(curp, rfc, nss), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EnviarCorreos(List<int> lstClaveEmpleado)
        {
            return Json(reclutamientoFS.EnviarCorreos(lstClaveEmpleado), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboTipoEmpleados()
        {
            return Json(reclutamientoFS.FillCboTipoEmpleados(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarSustentoHijo(List<HttpPostedFileBase> lstSustentos, int claveEmpleado, int FK_EmplFamilia)
        {
            return Json(reclutamientoFS.GuardarSustentoHijo(lstSustentos, claveEmpleado, FK_EmplFamilia), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSustentos(int claveEmpleado, int FK_EmplFamilia)
        {
            return Json(reclutamientoFS.GetSustentos(claveEmpleado, FK_EmplFamilia), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DescagarSustento(int id)
        {
            try
            {
                var resultadoTupla = reclutamientoFS.DescagarSustento(id);
                string nombreArchivo = resultadoTupla.Item2;
                string tipo = MimeMapping.GetMimeMapping(nombreArchivo);
                var fileStreamResult = new FileStreamResult(resultadoTupla.Item1, tipo);
                fileStreamResult.FileDownloadName = nombreArchivo;

                return fileStreamResult;
            }
            catch (Exception ex)
            {
                return View(RUTA_VISTA_ERROR_DESCARGA);
            }
        }
        #endregion

        #region GESTION DE ARCHIVOS DEL CANDIDATO/EMPLEADO
        public ActionResult GetArchivosCandidato(int idCandidato)
        {
            result = new Dictionary<string, object>();
            try
            {
                List<ArchivosDTO> lstArchivos = reclutamientoFS.GetArchivosCandidato(idCandidato);
                result.Add("lstArchivos", lstArchivos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarArchivoCandidato(int idArchivo)
        {
            result = new Dictionary<string, object>();
            try
            {
                if (idArchivo <= 0)
                    throw new Exception("Ocurrió un error al eliminar el archivo del candidato");

                bool esEliminarArchivo = reclutamientoFS.EliminarArchivoCandidato(idArchivo);
                if (esEliminarArchivo)
                    result.Add(SUCCESS, true);
                else
                    throw new Exception("Ocurrió un error al eliminar el archivo del candidato");
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PLATAFORMAS
        public ActionResult GetPlataformas()
        {
            result = new Dictionary<string, object>();
            try
            {
                List<PlataformasDTO> lstPlataformas = reclutamientoFS.GetPlataformas();
                result.Add("lstPlataformas", lstPlataformas);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result);
        }

        public ActionResult CrearEditarPlataforma(PlataformasDTO objCEDTO)
        {
            result = new Dictionary<string, object>();
            try
            {
                bool esCrearEditar = reclutamientoFS.CrearEditarPlataforma(objCEDTO);
                if (!esCrearEditar && objCEDTO.esActualizar)
                    throw new Exception("Ocurrió un error al actualizar la plataforma.");
                else if (!esCrearEditar && !objCEDTO.esActualizar)
                    throw new Exception("Ocurrió un error al registrar la plataforma.");
                else
                    result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarPlataforma(int idPlataforma)
        {
            try
            {
                result = new Dictionary<string, object>();
                if (idPlataforma > 0)
                {
                    #region SE ELIMINA LA PLATAFORMA
                    bool esEliminar = reclutamientoFS.EliminarPlataforma(idPlataforma);
                    if (esEliminar)
                        result.Add(SUCCESS, true);
                    else
                        throw new Exception("Ocurrió un error al eliminar la plataforma.");
                    #endregion
                }
                else
                    throw new Exception("Ocurrió un error al eliminar la plataforma.");
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CATALOGO CORREOS
        public ActionResult GetCorreos(CatCorreosDTO objFiltroDTO)
        {
            result = new Dictionary<string, object>();
            try
            {
                List<tblRH_REC_CatCorreos> lstCatCorreos = reclutamientoFS.GetCorreos(objFiltroDTO);
                result.Add("lstCatCorreos", lstCatCorreos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result);
        }

        public ActionResult CrearEditarCorreo(CatCorreosDTO objCEDTO)
        {
            result = new Dictionary<string, object>();
            try
            {
                bool esCrearEditar = reclutamientoFS.CrearEditarCorreo(objCEDTO);
                if (!esCrearEditar && objCEDTO.id > 0)
                    throw new Exception("Ocurrió un error al actualizar el correo.");
                else if (!esCrearEditar && objCEDTO.id <= 0)
                    throw new Exception("Ocurrió un error al registrar el correo.");
                else if (!esCrearEditar)
                    throw new Exception("Ocurrió un error en el sistema.");
                else
                    result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarCorreo(int idCorreo)
        {
            try
            {
                result = new Dictionary<string, object>();
                if (idCorreo > 0)
                {
                    #region SE ELIMINA EL CORREO
                    bool esEliminar = reclutamientoFS.EliminarCorreo(idCorreo);
                    if (esEliminar)
                        result.Add(SUCCESS, true);
                    else
                        throw new Exception("Ocurrió un error al eliminar el correo.");
                    #endregion
                }
                else
                    throw new Exception("Ocurrió un error al eliminar el correo.");
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region FILL COMBOS
        public ActionResult FillCboCC()
        {
            result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstCC = reclutamientoFS.FillCboCC();
                result.Add(ITEMS, lstCC);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboCCUnique()
        {
            return Json(reclutamientoFS.FillComboCCUnique(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboPuestos()
        {
            result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstPuestos = reclutamientoFS.FillCboPuestos();
                result.Add(ITEMS, lstPuestos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboPaises()
        {
            result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstPaises = reclutamientoFS.FillCboPaises();
                result.Add(ITEMS, lstPaises);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboEstados(int _clavePais)
        {
            result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstEstados = reclutamientoFS.FillCboEstados(_clavePais);
                result.Add(ITEMS, lstEstados);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboMunicipios(int _clavePais = 0, int _claveEstado = 0)
        {
            result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstMunicipios = reclutamientoFS.FillCboMunicipios(_clavePais, _claveEstado);
                result.Add(ITEMS, lstMunicipios);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboMotivos()
        {
            result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstMotivos = reclutamientoFS.FillCboMotivos();
                result.Add(ITEMS, lstMotivos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboEscolaridades()
        {
            result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstEscolaridades = reclutamientoFS.FillCboEscolaridades();
                result.Add(ITEMS, lstEscolaridades);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillFiltroCboCC()
        {
            result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstFiltroCC = reclutamientoFS.FillFiltroCboCC();
                result.Add(ITEMS, lstFiltroCC);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillFiltroCboPuestos()
        {
            result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstFiltroPuestos = reclutamientoFS.FillFiltroCboPuestos();
                result.Add(ITEMS, lstFiltroPuestos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillFiltroCboPuestosGestion()
        {
            result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstFiltroPuestos = reclutamientoFS.FillFiltroCboPuestosGestion();
                result.Add(ITEMS, lstFiltroPuestos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboTipoFormulaIMSS()
        {
            result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstTipoFormulaIMSS = reclutamientoFS.FillCboTipoFormulaIMSS();
                result.Add(ITEMS, lstTipoFormulaIMSS);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboDepartamentos(string cc)
        {
            result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstDepartamentos = reclutamientoFS.FillCboDepartamentos(cc);
                result.Add(ITEMS, lstDepartamentos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboUsuarios()
        {
            result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstUsuarios = reclutamientoFS.FillCboUsuarios();
                result.Add(ITEMS, lstUsuarios);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboPlataformas()
        {
            result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstPlataformas = reclutamientoFS.FillCboPlataformas();
                result.Add(ITEMS, lstPlataformas);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboRegistroPatronal(string cc)
        {
            result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> data = reclutamientoFS.FillComboRegistroPatronal(cc);
                result.Add(ITEMS, data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboDuracionContrato()
        {
            result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> data = reclutamientoFS.FillComboDuracionContrato();
                result.Add(ITEMS, data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillEstatusFiltro()
        {
            return Json(reclutamientoFS.FillEstatusFiltro(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillDepartamentos(string cc)
        {
            return Json(reclutamientoFS.FillDepartamentos(cc), JsonRequestBehavior.AllowGet);
        }

        public JsonResult CargarTiposNomina()
        {
            return Json(reclutamientoFS.CargarTiposNomina(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult CargarBancos()
        {
            return Json(reclutamientoFS.CargarBancos(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboEDArchivos()
        {
            return Json(reclutamientoFS.FillComboEDArchivos(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboCCRegistrosPatronales(int? clave_reg_pat)
        {
            return Json(reclutamientoFS.FillCboCCRegistrosPatronales(clave_reg_pat), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboRelFases()
        {
            return Json(reclutamientoFS.FillComboRelFases(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult FillMotivoSueldo()
        {
            return Json(reclutamientoFS.FillMotivoSueldo(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboGeoDepartamentos()
        {
            return Json(reclutamientoFS.FillComboGeoDepartamentos(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboEstadosPERU(int claveDepartamento)
        {
            return Json(reclutamientoFS.FillCboEstadosPERU(claveDepartamento), JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region EXPEDIENTE DIGITAL
        public ActionResult CargarExpedientesDigitales(string estatus_emp, string cc, List<int> estado)
        {
            return Json(reclutamientoFS.CargarExpedientesDigitales(estatus_emp, cc, estado), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetArchivosCombo()
        {
            return Json(reclutamientoFS.GetArchivosCombo(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarInformacionEmpleado(int claveEmpleado)
        {
            return Json(reclutamientoFS.CargarInformacionEmpleado(claveEmpleado), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevoExpediente(int claveEmpleado, List<int> listaArchivosAplicables)
        {
            return Json(reclutamientoFS.GuardarNuevoExpediente(claveEmpleado, listaArchivosAplicables), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarExpediente(int claveEmpleado, List<int> listaArchivosAplicables)
        {
            return Json(reclutamientoFS.EditarExpediente(claveEmpleado, listaArchivosAplicables), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarArchivoExpediente(HttpPostedFileBase archivo)
        {
            var expediente_id = Convert.ToInt32(Request.Form["expediente_id"]);
            var archivo_id = Convert.ToInt32(Request.Form["archivo_id"]);

            return Json(reclutamientoFS.GuardarArchivoExpediente(expediente_id, archivo_id, archivo), JsonRequestBehavior.AllowGet);
        }

        public FileResult DescargarArchivoExpediente()
        {
            try
            {
                int archivoCargado_id = Convert.ToInt32(Request.QueryString["archivoCargado_id"]);
                int archivoId = Convert.ToInt32(Request.QueryString["archivoId"]);
                var archivo = reclutamientoFS.GetArchivoExpediente(archivoCargado_id, archivoId);

                return File(archivo.rutaArchivo, "multipart/form-data", Path.GetFileName(archivo.rutaArchivo));
            }
            catch (Exception)
            {
                return null;
            }
        }
        public ActionResult EliminarArchivoExpediente(int expediente_id, int archivo_id)
        {
            return Json(reclutamientoFS.EliminarArchivoExpediente(expediente_id, archivo_id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetArchivos()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                var lstArchivos = reclutamientoFS.GetArchivos();

                result.Add(ITEMS, lstArchivos);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarArchivos(ExpedienteArchivosDTO objArchivo)
        {

            return Json(reclutamientoFS.CrearEditarArchivos(objArchivo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarArchivo(int idArchivo)
        {
            return Json(reclutamientoFS.EliminarArchivo(idArchivo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarDatosArchivoExpediente(int archivoCargado_id)
        {
            var resultado = new Dictionary<string, object>();

            var archivo = reclutamientoFS.GetArchivoExpediente(archivoCargado_id, 0);
            var fileData = Tuple.Create(System.IO.File.ReadAllBytes(archivo.rutaArchivo), Path.GetExtension(archivo.rutaArchivo));

            Session["archivoVisor"] = fileData;

            resultado.Add(SUCCESS, true);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VerHistorialExpediente(int expediente_id, int archivo_id)
        {
            return Json(reclutamientoFS.VerHistorialExpediente(expediente_id, archivo_id), JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult _visorGrid()
        {
            var viewer = new DocViewer { ID = "ctlDoc", IncludeJQuery = false, DebugMode = false, BasePath = "/", ResourcePath = "/", FitType = "", Zoom = 40, TimeOut = 20 };

            ViewBag.ViewerScripts = viewer.ReferenceScripts();
            ViewBag.ViewerCSS = viewer.ReferenceCss();
            ViewBag.ViewerID = viewer.ClientID;
            ViewBag.ViewerObject = viewer.JsObject;
            ViewBag.ViewerInit = viewer.GetAjaxInitArguments("");

            return PartialView();
        }

        public MemoryStream DescargarAvanceExcel(string estatus_emp, string cc)
        {
            var resultado = reclutamientoFS.DescargarAvanceExcel(estatus_emp, cc);

            if ((bool)resultado[SUCCESS])
            {
                var fecha = DateTime.Now.ToString("dd_MM_yyyy");

                var stream = (MemoryStream)resultado[ITEMS];
                Response.Clear();
                Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("Content-Disposition", "attachement; filename=AvanceExpediente_" + fecha + ".xlsx");
                Response.BinaryWrite(stream.ToArray());
                Response.End();
            }

            return null;
        }
        #endregion

        #region ALTA EMPLEADO (REGION TEMPORAL)
        public ActionResult CargarRequisiciones()
        {
            var json = Json(reclutamientoFS.CargarRequisiciones(), JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult CargarAutoriza(int autoriza)
        {
            return Json(reclutamientoFS.CargarAutoriza(autoriza), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarUsuarioResg(int usuarioResg)
        {
            return Json(reclutamientoFS.CargarUsuarioResg(usuarioResg), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarDepto(int depto)
        {
            return Json(reclutamientoFS.CargarDepto(depto), JsonRequestBehavior.AllowGet);
        }

        public JsonResult CargarTabulador(string cc, int puesto, int? idTabuladorDet)
        {
            return Json(reclutamientoFS.CargarTabulador(cc, puesto, idTabuladorDet), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DescargarArchivoEmpleado(int id)
        {
            try
            {
                var resultadoTupla = reclutamientoFS.DescargarArchivoEmpleado(id);

                string nombreArchivo = resultadoTupla.Item2;
                string tipo = MimeMapping.GetMimeMapping(nombreArchivo);

                var fileStreamResult = new FileStreamResult(resultadoTupla.Item1, tipo);
                fileStreamResult.FileDownloadName = nombreArchivo;

                return fileStreamResult;
            }
            catch (Exception ex)
            {
                return View(RUTA_VISTA_ERROR_DESCARGA);
            }
        }

        public ActionResult GenerarCURP(string nombres, string paterno, string materno, SexoEnum sexo, DateTime fechaNacimiento, EstadoEnum estado)
        {
            try
            {
                result.Add("curp", reclutamientoFS.GenerarCURP(nombres, paterno, materno, sexo, fechaNacimiento, estado));
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRFC(GestionCandidatosDTO objCandidato)
        {
            try
            {
                result.Add("rfc", reclutamientoFS.GetRFC(objCandidato));
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region REQUISICION
        public ActionResult RequisicionEmpleado()
        {
            ViewBag.AutorizacionMasivaActiva = vSesiones.sesionUsuarioDTO.id == 1041 || vSesiones.sesionUsuarioDTO.id == 1019 || vSesiones.sesionUsuarioDTO.id == 79377;
            return View();
        }

        public JsonResult GetCCs()
        {
            var json = Json(reclutamientoFS.GetCCs(), JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
            //return Json(reclutamientoFS.GetCCs(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRequisiciones(List<string> ccs, string estatus)
        {
            var json = Json(reclutamientoFS.GetRequisiciones(ccs, estatus), JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
            //return Json(reclutamientoFS.GetRequisiciones(ccs, estatus));
        }

        public JsonResult GetIdRequisicionDisponible()
        {
            return Json(reclutamientoFS.GetIdRequisicionDisponible(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPlantilla(string cc, int? puesto)
        {
            return Json(reclutamientoFS.GetPlantilla(cc, puesto), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTipoContrato()
        {
            return Json(reclutamientoFS.GetTipoContrato(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRazonSolicitud()
        {
            return Json(reclutamientoFS.GetRazonSolicitud(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetJefeInmediato(string cc)
        {
            return Json(reclutamientoFS.GetJefeInmediato(cc), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAutoriza(string cc)
        {
            return Json(reclutamientoFS.GetAutoriza(cc), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAutocompleteJefe(string term)
        {
            return Json(reclutamientoFS.GetAutocompleteJefe(term), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSolicita()
        {
            return Json(reclutamientoFS.GetSolicita(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAutocompleteSolicita(string term)
        {
            return Json(reclutamientoFS.GetAutocompleteSolicita(term), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAutocompleteAutoriza(string term, string cc)
        {
            return Json(reclutamientoFS.GetAutocompleteAutoriza(term, cc), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GuardarRequisicion(sn_requisicion_personal requisicion)
        {
            return Json(reclutamientoFS.GuardarRequisicion(requisicion), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInformacionRequisicionConsulta(int requisicion_id, string cc)
        {
            return Json(reclutamientoFS.GetInformacionRequisicionConsulta(requisicion_id, cc), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutorizarRechazarRequisicion(RequisicionRHDTO objDTO)
        {
            return Json(reclutamientoFS.AutorizarRechazarRequisicion(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFechaVigencia7DiasNaturales()
        {
            return Json(reclutamientoFS.GetFechaVigencia7DiasNaturales(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarRequisicion(int requisicion_id)
        { 
            return Json(reclutamientoFS.EliminarRequisicion(requisicion_id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCategoriasPuesto(int idPuesto, string cc)
        {
            return Json(reclutamientoFS.GetCategoriasPuesto(idPuesto, cc), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCategoriaPuesto(int tabuladorDetID)
        {
            return Json(reclutamientoFS.GetCategoriaPuesto(tabuladorDetID), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTabuladorCategoria(int categoriaID)
        {
            return Json(reclutamientoFS.GetTabuladorCategoria(categoriaID), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region COMENTARIO
        public ActionResult GetComentario(int claveEmpleado)
        {
            return Json(reclutamientoFS.GetComentario(claveEmpleado), JsonRequestBehavior.AllowGet);
        }
        public ActionResult CrearComentario(int claveEmpleado, string comentario)
        {
            return Json(reclutamientoFS.CrearComentario(claveEmpleado, comentario), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region FNC GRALES
        public ActionResult GetContratoReporte(int clave_empleado)
        {
            return Json(reclutamientoFS.GetContratoReporte(clave_empleado), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCategoriasByLineaNegocio(int idLineaNegocio, int idPuesto)
        {
            return Json(reclutamientoFS.GetCategoriasByLineaNegocio(idLineaNegocio, idPuesto), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetLineaNegocioByCC(string cc)
        {
            return Json(reclutamientoFS.GetLineaNegocioByCC(cc), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region REL REGISTRO PATRONALES
        public ActionResult GetRelRegPatronales(tblRH_EK_Registros_Patronales objFiltro)
        {
            return Json(reclutamientoFS.GetRelRegPatronales(objFiltro), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetRelRegPatCC(int clave_reg_pat)
        {
            return Json(reclutamientoFS.GetRelRegPatCC(clave_reg_pat), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddCCRegPatronal(int clave_reg_pat, string cc)
        {
            return Json(reclutamientoFS.AddCCRegPatronal(clave_reg_pat, cc), JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteCCRegPatronal(int clave_reg_pat, string cc)
        {
            return Json(reclutamientoFS.DeleteCCRegPatronal(clave_reg_pat, cc), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarRegistroPatronal(HttpPostedFileBase archivoAdjunto)
        {
            tblRH_EK_Registros_Patronales objCERegPat = JsonUtils.convertJsonToNetObject<tblRH_EK_Registros_Patronales>(Request.Form["objCERegPat"], "es-MX");
            return Json(reclutamientoFS.CrearEditarRegistroPatronal(objCERegPat, archivoAdjunto), JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteRegistroPatronal(int idRegPat)
        {
            return Json(reclutamientoFS.DeleteRegistroPatronal(idRegPat), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetUltimoIdRegPat()
        {
            return Json(reclutamientoFS.GetUltimoIdRegPat(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetRelRegPatronalesReporte()
        {
            return Json(reclutamientoFS.GetRelRegPatronalesReporte(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DescargarArchivoRegPat(int id)
        {
            var resultadoTupla = reclutamientoFS.DescargarArchivoRegPat(id);

            if (resultadoTupla != null)
            {
                string nombreArchivo = resultadoTupla.Item2;
                string tipo = MimeMapping.GetMimeMapping(nombreArchivo);

                var fileStreamResult = new FileStreamResult(resultadoTupla.Item1, tipo);
                fileStreamResult.FileDownloadName = nombreArchivo;

                return fileStreamResult;
            }
            else
            {
                return View(RUTA_VISTA_ERROR_DESCARGA);
            }
        }
        #endregion

        #region REALCION FASES
        public ActionResult GetFasesUsuarios(int idFase)
        {
            return Json(reclutamientoFS.GetFasesUsuarios(idFase), JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddFasesUsuarios(int idUsuario, int idFase)
        {
            return Json(reclutamientoFS.AddFasesUsuarios(idUsuario, idFase), JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteFasesUsuarios(int idUsuario, int idFase)
        {
            return Json(reclutamientoFS.DeleteFasesUsuarios(idUsuario, idFase), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CAT CONTRATOS
        public ActionResult GetDuracionContratos()
        {
            return Json(reclutamientoFS.GetDuracionContratos(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddDuracionContrato(string duracion_desc, int? dias, int? meses, int? años, bool indef)
        {
            return Json(reclutamientoFS.AddDuracionContrato(duracion_desc, dias, meses, años, indef), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteDuracionContrato(int id_duracion)
        {
            return Json(reclutamientoFS.DeleteDuracionContrato(id_duracion), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region CAT PUESTOS
        public ActionResult GetPuestos()
        {
            return Json(reclutamientoFS.GetPuestos(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevoPuesto(tblRH_EK_Puestos puesto)
        {
            return Json(reclutamientoFS.GuardarNuevoPuesto(puesto), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarPuesto(tblRH_EK_Puestos puesto)
        {
            return Json(reclutamientoFS.EditarPuesto(puesto), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarPuesto(tblRH_EK_Puestos puesto)
        {
            return Json(reclutamientoFS.EliminarPuesto(puesto), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarArchivoDescriptor(HttpPostedFileBase file, int puesto)
        {
            return Json(reclutamientoFS.CargarArchivoDescriptor(file, puesto));
        }

        public FileResult GetFileDownloadDescriptor(int id)
        {
            var nombre = "";
            var ruta = "";

            var archivo = reclutamientoFS.GetFileDownloadDescriptor(id);
            if (archivo != null)
            {
                nombre = Path.GetFileName(archivo.ruta);
                ruta = archivo.ruta;
            }

            return File(ruta, "multipart/form-data", nombre);
        }
        #endregion

        #region CAT CATEGORIAS
        public ActionResult FillComboPuestos(string cC)
        {
            return Json(reclutamientoFS.FillComboPuestos(cC), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPuestosCategoriasRelPuesto(string _cc, string _strPuesto, int idPuesto)
        {
            return Json(reclutamientoFS.GetPuestosCategoriasRelPuesto(_cc, _strPuesto, idPuesto), JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditarPlantilla(List<InputCategoriasDTO> lstCambios, string cc, int puesto, string descPuesto)
        {
            return Json(reclutamientoFS.EditarPlantilla(lstCambios, cc, puesto, descPuesto), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllPuestos()
        {
            return Json(reclutamientoFS.GetAllPuestos(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddPlantillaPuesto(string _cc, string _strPuesto, int idPuesto, string _strNuevoPuesto, int nuevoPuesto)
        {
            return Json(reclutamientoFS.AddPlantillaPuesto(_cc, _strPuesto, idPuesto, _strNuevoPuesto, nuevoPuesto), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPuestoDetalle(PuestoDetalleDTO objDTO)
        {
            return Json(reclutamientoFS.GetPuestoDetalle(objDTO), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CAT TABULADORES
        public ActionResult GetPuestosTabuladores(string cc)
        {
            return Json(reclutamientoFS.GetPuestosTabuladores(cc), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarTabuladorPuesto(string cc, CatTabuladoresDTO objTabulador)
        {
            return Json(reclutamientoFS.CrearEditarTabuladorPuesto(cc, objTabulador), JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region DIAS PARA INGRESOS
        public JsonResult GetDiasDisponibles()
        {
            return Json(reclutamientoFS.GetDiasDisponibles(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult SetDiasDisponibles(int anteriores, int posteriores)
        {
            return Json(reclutamientoFS.SetDiasDisponibles(anteriores, posteriores));
        }

        public ActionResult factorizar()
        {
            reclutamientoFS.factorizar();

            return View("CatCorreos");
        }
        #endregion

        #region Datos Perú
        public ActionResult FillComboAFPPeru()
        {
            result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstEstados = reclutamientoFS.FillComboAFPPeru();
                result.Add(ITEMS, lstEstados);
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

        #region DATOS EMPLEADO PERU
        public JsonResult GetSituacion(bool afiliado)
        {
            return Json(reclutamientoFS.GetSituacion(afiliado), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRucEps()
        {
            return Json(reclutamientoFS.GetRucEps(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAfps()
        {
            return Json(reclutamientoFS.GetAfps(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboTiposTrabajadores()
        {
            return Json(reclutamientoFS.FillComboTiposTrabajadores(), JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult FillComboBancosPeru()
        {
            return Json(reclutamientoFS.FillComboBancosPeru() , JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboRegimenLaboralPeru()
        {
            return Json(reclutamientoFS.FillComboRegimenLaboralPeru(), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}