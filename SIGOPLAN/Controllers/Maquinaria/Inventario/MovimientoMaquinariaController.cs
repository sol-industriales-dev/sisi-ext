using Core.DTO.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Inventario;
using Data.Factory.Maquinaria.Captura;
using Data.Factory.Maquinaria.Catalogos;
using Data.Factory.Maquinaria.Inventario;
using Data.Factory.Principal.Usuarios;
using Data.Factory.Proyecciones;
using Infrastructure.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Maquinaria.Inventario
{

    public class MovimientoMaquinariaController : BaseController
    {

        #region Factory
        AutorizaStandbyFactoryServices autorizaStandbyFactoryServices;
        StandbyDetFactoryServices standbyDetFactoryServices;
        CentroCostosFactoryServices centroCostosFactoryServices;
        ControlEnvioyRecepcionFactoryServices controlEnvioyRecepcionFactoryService;
        AsignacionEquiposFactoryServices asignacionEquiposFactoryServices;
        MaquinaFactoryServices maquinaFactoryServices;
        CapturaHorometroFactoryServices capturaHorometroFactoryServices;
        SolicitudEquipoFactoryServices solicitudEquipoFactoryServices;
        SolicitudEquipoDetFactoryServices solicitudEquipoDetFactoryServices;
        AutorizacionStandByFactoryServices autorizacionStandByFactoryServices;
        UsuarioFactoryServices usuarioFactoryServices;
        StandbyFactoryServices standbyFactoryServices;
        ObraFactoryServices obraFactoryServices;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            obraFactoryServices = new ObraFactoryServices();
            autorizaStandbyFactoryServices = new AutorizaStandbyFactoryServices();
            standbyDetFactoryServices = new StandbyDetFactoryServices();
            standbyFactoryServices = new StandbyFactoryServices();
            usuarioFactoryServices = new UsuarioFactoryServices();
            autorizacionStandByFactoryServices = new AutorizacionStandByFactoryServices();
            solicitudEquipoDetFactoryServices = new SolicitudEquipoDetFactoryServices();
            maquinaFactoryServices = new MaquinaFactoryServices();
            solicitudEquipoFactoryServices = new SolicitudEquipoFactoryServices();
            centroCostosFactoryServices = new CentroCostosFactoryServices();
            controlEnvioyRecepcionFactoryService = new ControlEnvioyRecepcionFactoryServices();
            asignacionEquiposFactoryServices = new AsignacionEquiposFactoryServices();
            capturaHorometroFactoryServices = new CapturaHorometroFactoryServices();
            base.OnActionExecuting(filterContext);
        }
        #endregion

        // GET: MovimientoMaquinaria
        public ActionResult ControlEnvioyRecepcion()
        {
            return View();
        }
        public ActionResult ControlCalidad()
        {
            return View();
        }
        public ActionResult EstatusDiario()
        {
            return View();
        }

        public ActionResult ReporteStandBY()
        {
            return View();
        }

        public ActionResult MovimientoControles()
        {
            return View();
        }

        public ActionResult GetMaquinariasPendientesEnvios(int obj, int tipoFiltro)
        {
            var result = new Dictionary<string, object>();
            try
            {
                int Estatus = 0;
                int filtro = 10;
                /*   switch (obj)
                   {
                       case 1:
                           Estatus = 2;
                           break;
                       case 2:
                           Estatus = 4;
                           break;
                       case 3:
                           Estatus = 6;
                           break;
                       case 4:
                           Estatus = 8;

                           break;

                       default:
                           break;
                   }*/
                var idUsuario = getUsuario().id;
                var usuario = usuarioFactoryServices.getUsuarioService().getCCsUsuario(idUsuario);

                List<tblM_AsignacionEquipos> raw = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().GetListaControles(obj, usuario, tipoFiltro);

                if (raw != null)
                {
                    var ListaEconomicos = raw.Select(x => new
                    {
                        id = x.solicitudEquipoID,
                        Folio = x.folio,
                        CCName = (obj == 3 ? "PATIO DE MAQUINARIA" : centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(x.CCOrigen)),
                        Fecha = x.fechaInicio.ToString("dd/MM/yyyy"),
                        Economico = x.noEconomicoID,
                        nomb = x.noEconomicoID == 0 ? x.Economico : maquinaFactoryServices.getMaquinaServices().GetMaquina(x.noEconomicoID).noEconomico,
                        idAsigancion = x.id,
                        cc = x.cc,
                        isRenta = x.Economico == "COMPRA" || x.Economico == "RENTA" || x.Economico == "RENTA OPCION COMPRA" || x.Economico == "Renta" ? true : false,
                        estatus = x.estatus,
                        SolicitudDetalleId = x.SolicitudDetalleId,
                        needAsignacion = x.noEconomicoID == 0 ? 0 : 1,
                        CCNameRecepcion = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(x.cc)
                    });

                    result.Add("EquiposPendientes", ListaEconomicos);

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
        public string GetLugarRecepcion(string cc, int tipo)
        {

            string resultado = "";

            switch (tipo)
            {
                case 3:

                    return resultado = "PATIO MAQUINARIA";

                default:
                    break;
            }

            return resultado;
        }
        public ActionResult GetEconomicosNoAsignados(int idAsignacion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var DatosAsignacion = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().getInfoAsignacion(idAsignacion);
                var DatosDetalleSolicitud = solicitudEquipoFactoryServices.getSolicitudEquipoServices().GetSolicitudEquipoDet(DatosAsignacion.SolicitudDetalleId);


                var ListaEconomicos = maquinaFactoryServices.getMaquinaServices().FillCboEconomicos(DatosDetalleSolicitud.grupoMaquinariaID).Where(x => x.renta.Equals(true))
                     .Select(x => new
                     {
                         idEconomico = x.id,
                         Economico = x.noEconomico,
                         Grupo = x.grupoMaquinaria.descripcion,
                         Modelo = x.modeloEquipo.descripcion,
                         Marca = x.marca.descripcion,
                         localizacion = centroCostosFactoryServices.getCentroCostosService().getNombreCC(Convert.ToInt32(x.centro_costos)),
                         idAsignacion = idAsignacion

                     }).ToList();

                result.Add("GrupoMaquinaria", DatosDetalleSolicitud.GrupoMaquinaria.descripcion);
                result.Add("TipoMaquinaria", DatosDetalleSolicitud.GrupoMaquinaria.tipoEquipo.descripcion);
                result.Add("ModeloMaquinaria", DatosDetalleSolicitud.ModeloEquipo.descripcion);
                result.Add("Horas", DatosDetalleSolicitud.horas);

                result.Add("ListaEconomicos", ListaEconomicos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDataFromEconomico(int obj, int tipoControl, int tipoAccion, int idAsignacion, int idSolicitud)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var res = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().GetInfoMaquinaria(obj);

                var Asignacion = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().getInfoAsignacion(idAsignacion);

                var esRenta = false;
                if (Asignacion.Economico == "Renta" || Asignacion.Economico == "COMPRA" || Asignacion.Economico == "RENTA OPCION COMPRA" || Asignacion.Economico == "RENTA")
                {
                    esRenta = true;

                }

                switch (tipoAccion)
                {
                    case 1:
                        {//revisar y quitar el parche segundo if != 4
                            if (tipoControl > 1 && esRenta == false)
                            {

                                int estatus = 0;

                                if (tipoControl == 1)
                                {
                                    estatus = 1;
                                }
                                else
                                {
                                    estatus = tipoControl - 1;
                                }

                                var dataControl = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().getInfoControl(idAsignacion, estatus, idSolicitud);
                                if (dataControl != null)
                                {
                                    result.Add("Horometro", dataControl.horometros);
                                    result.Add("dataControl", dataControl);


                                    var fechaRecepcionEmbarque = dataControl.fechaRecepcionEmbarque.ToString("dd/MM/yyyy");
                                    result.Add("fechaRecepcionEmbarque", fechaRecepcionEmbarque);
                                }



                            }
                            else
                            {

                            }
                        }
                        break;
                    case 2:
                        {
                            var dataControl = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().getInfoControl(idAsignacion, tipoControl, idSolicitud);
                            var fechaRecepcionEmbarque = dataControl.fechaRecepcionEmbarque.ToString("dd/MM/yyyy");
                            result.Add("dataControl", dataControl);
                            result.Add("fechaRecepcionEmbarque", fechaRecepcionEmbarque);
                        }
                        break;
                    default:
                        break;
                }

                var grupo = res.grupoMaquinaria.descripcion;
                var tipo = res.grupoMaquinaria.tipoEquipo.descripcion;
                var marca = res.marca.descripcion;
                var serie = res.noSerie;
                var modelo = res.modeloEquipo.descripcion;
                var noPoliza = res.noPoliza;
                var Economico = res.noEconomico;
                var TipoCaptura = res.TipoCaptura;
                var tipodeCaptura = res.TipoCaptura;

                var archivoSOS = res.grupoMaquinaria.sos;
                var archivoBitacora = res.grupoMaquinaria.bitacora;
                var archivoDN = res.grupoMaquinaria.dn;


                result.Add("tipodeCaptura", tipodeCaptura);
                if (tipoControl.Equals(1))
                {
                    var Horometro = capturaHorometroFactoryServices.getCapturaHorometroServices().GetHorometroFinal(Economico);

                    result.Add("Horometro", Horometro);
                    result.Add("Lugar", centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(Asignacion.CCOrigen));
                    result.Add("idCC", Asignacion.CCOrigen);
                }
                else if (tipoControl.Equals(2))
                {
                    var Horometro = capturaHorometroFactoryServices.getCapturaHorometroServices().GetHorometroFinal(Economico);

                    ///result.Add("Horometro", Horometro);
                    result.Add("Lugar", centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(Asignacion.cc));
                    result.Add("idCC", Asignacion.cc);
                }
                else if (tipoControl.Equals(3))
                {
                    var Horometro = capturaHorometroFactoryServices.getCapturaHorometroServices().GetHorometroFinal(Economico);

                    //     result.Add("Horometro", Horometro);
                    result.Add("Lugar", centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(Asignacion.cc));
                    result.Add("idCC", Asignacion.cc);
                }
                if (tipoControl.Equals(4))
                {
                    result.Add("EsTMC", true);
                }
                else
                {
                    result.Add("EsTMC", false);
                }

                result.Add("archivoSOS", archivoSOS);
                result.Add("archivoBitacora", archivoBitacora);
                result.Add("archivoDN", archivoDN);

                result.Add("TipoCaptura", TipoCaptura);
                result.Add("esRenta", esRenta);
                result.Add("tipo", tipo);
                result.Add("grupo", grupo);
                result.Add("marca", marca);
                result.Add("modelo", modelo);
                result.Add("serie", serie);
                result.Add("noPoliza", noPoliza);
                result.Add("Economico", Economico);


            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveOrUpdate(tblM_ControlEnvioMaquinaria obj, int idAsignacion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var infoAsignacion = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().getInfoAsignacion(idAsignacion);

                switch (infoAsignacion.estatus)
                {
                    case 1:
                        infoAsignacion.estatus = 2;
                        break;
                    case 2:
                        infoAsignacion.estatus = 3;
                        break;
                    default:
                        break;
                }
                obj.asignacionEquipoId = infoAsignacion.id;
                controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().SaveOrUpdate(obj, null);
                asignacionEquiposFactoryServices.getAsignacionEquiposFactoryServices().SaveOrUpdate(infoAsignacion);

                if (obj.tipoControl == 2)
                {
                    var maquina = maquinaFactoryServices.getMaquinaServices().GetMaquina(obj.noEconomico);
                    maquina.centro_costos = obj.lugar.ToString();
                    maquinaFactoryServices.getMaquinaServices().Guardar(maquina);
                }

                result.Add("idControl", obj.id);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateAsignacion(int idEconomico, int idAsignacion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var infoAsignacion = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().getInfoAsignacion(idAsignacion);
                var nombreEconomico = maquinaFactoryServices.getMaquinaServices().GetMaquina(idEconomico);
                infoAsignacion.noEconomicoID = idEconomico;
                nombreEconomico.centro_costos = "997";

                infoAsignacion.CCOrigen = infoAsignacion.cc;
                asignacionEquiposFactoryServices.getAsignacionEquiposFactoryServices().SaveOrUpdate(infoAsignacion);



                maquinaFactoryServices.getMaquinaServices().Guardar(nombreEconomico);

                result.Add("idEconomico", nombreEconomico.id);
                result.Add("idAsignacion", infoAsignacion.id);
                result.Add("numEconomico", nombreEconomico.noEconomico);
                result.Add("Folio", infoAsignacion.folio);
                result.Add("CCOrigen", nombreEconomico.centro_costos);
                result.Add("nomCC", centroCostosFactoryServices.getCentroCostosService().getNombreCC(Convert.ToInt32(nombreEconomico.centro_costos)));
                result.Add("solicitudEquipoID", infoAsignacion.solicitudEquipoID);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public FileResult getFileDownload()
        {
            int id = int.Parse(Request.QueryString["id"]);
            var o = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().getReporteEnvio(id);
            return File(o.RutaArchivo, "multipart/form-data", o.Nombre);
        }
        [HttpPost]
        public ActionResult SaveSendEspecial()
        {
            var result = new Dictionary<string, object>();
            try
            {

                // tblM_ControlEnvioMaquinaria obj, 
                // int idAsignacion, 
                // int tipoEnvio


                var obj = JsonConvert.DeserializeObject<tblM_ControlEnvioMaquinaria>(Request.Form["obj"]);

                var idAsignacion = JsonConvert.DeserializeObject<int>(Request.Form["idAsignacion"].ToString());
                var fechaRecepcionEmbarque = JsonConvert.DeserializeObject<string>(Request.Form["fechaRecepcionEmbarque"].ToString());
                int tipoEnvio = JsonConvert.DeserializeObject<int>(Request.Form["tipoEnvio"].ToString());
                var fechaElaboracion = JsonConvert.DeserializeObject<string>(Request.Form["fechaElaboracion"].ToString());

                obj.fechaRecepcionEmbarque = Convert.ToDateTime(fechaRecepcionEmbarque);
                obj.fechaElaboracion = Convert.ToDateTime(fechaElaboracion);
                HttpPostedFileBase file = Request.Files["fupAdjunto"];

                var infoAsignacion = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().getInfoAsignacion(idAsignacion);

                if (obj.tipoControl == 4)
                {
                    infoAsignacion.estatus = 10;
                }
                else
                {
                    infoAsignacion.estatus = infoAsignacion.estatus + 1;
                }

                obj.asignacionEquipoId = infoAsignacion.id;
                controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().SaveOrUpdate(obj, file);
                asignacionEquiposFactoryServices.getAsignacionEquiposFactoryServices().SaveOrUpdate(infoAsignacion);

                if (infoAsignacion.estatus == 10)
                {
                    var maquina = maquinaFactoryServices.getMaquinaServices().GetMaquina(obj.noEconomico);
                    maquina.centro_costos = obj.lugar.ToString();
                    maquinaFactoryServices.getMaquinaServices().Guardar(maquina);

                }


                if (tipoEnvio != 0 && obj.tipoControl == 4)
                {
                    var SolicitudDetalleId = infoAsignacion.SolicitudDetalleId;
                    var SolicitudDetalle = solicitudEquipoFactoryServices.getSolicitudEquipoServices().GetSolicitudEquipoDet(SolicitudDetalleId);

                    SolicitudDetalle.estatus = false;
                    solicitudEquipoDetFactoryServices.getSolicitudEquipoDetServices().Guardar(SolicitudDetalle);

                }

                result.Add("idControl", obj.id);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //UpdateAsignacion
        [HttpPost]
        public ActionResult GuardarEnvioRecepcion()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = JsonConvert.DeserializeObject<tblM_ControlEnvioMaquinaria>(Request.Form["obj"]);

                var idAsignacion = JsonConvert.DeserializeObject<int>(Request.Form["idAsignacion"].ToString());
                var fechaRecepcionEmbarque = JsonConvert.DeserializeObject<string>(Request.Form["fechaRecepcionEmbarque"].ToString());
                var fechaElaboracion = JsonConvert.DeserializeObject<string>(Request.Form["fechaElaboracion"].ToString());

                obj.fechaRecepcionEmbarque = Convert.ToDateTime(fechaRecepcionEmbarque);
                obj.fechaElaboracion = Convert.ToDateTime(fechaElaboracion);
                HttpPostedFileBase file = Request.Files["fupAdjunto"];

                var infoAsignacion = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().getInfoAsignacion(Convert.ToInt32(idAsignacion));

                switch (infoAsignacion.estatus)
                {
                    case 2:
                        infoAsignacion.estatus = 3;
                        break;
                    case 4:
                        infoAsignacion.estatus = 5;
                        break;
                    case 5:
                        infoAsignacion.estatus = 6;
                        break;
                    case 7:
                        infoAsignacion.estatus = 8;
                        break;
                    case 8:
                        infoAsignacion.estatus = 10;
                        break;
                    default:
                        break;
                }

                obj.asignacionEquipoId = infoAsignacion.id;
                controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().SaveOrUpdate(obj, file);
                asignacionEquiposFactoryServices.getAsignacionEquiposFactoryServices().SaveOrUpdate(infoAsignacion);

                if (obj.tipoControl == 4 || obj.tipoControl == 2)
                {
                    var maquina = maquinaFactoryServices.getMaquinaServices().GetMaquina(obj.noEconomico);
                    maquina.centro_costos = obj.lugar.ToString();
                    maquinaFactoryServices.getMaquinaServices().Guardar(maquina);

                    if (!asignacionEquiposFactoryServices.getAsignacionEquiposFactoryServices().GetAsginadosRecibidos(idAsignacion))
                    {
                        var objSolicitud = solicitudEquipoDetFactoryServices.getSolicitudEquipoDetServices().getSolicitudbyID(infoAsignacion.solicitudEquipoID);
                        objSolicitud.Estatus = true;
                        solicitudEquipoFactoryServices.getSolicitudEquipoServices().Guardar(objSolicitud);
                    }
                }

                result.Add("idControl", obj.id);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTablaMaquinariaAsignadaEnObra(int cc, int filtro)
        {
            var result = new Dictionary<string, object>();
            var usuario = base.getUsuario();
            try
            {
                switch (filtro)
                {
                    case 1:
                        {

                            var pendientes = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices()
                                    .getMaquinariaAsignadaPendienteAutorizar(cc).Select(x => x.idEconomico);

                            var res = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices()
                                .getMaquinariaAsignada(cc).Where(x => !pendientes.Contains(x.idEconomico)).Select(x => new
                                {
                                    x.CC,
                                    x.Economico,
                                    x.idAsignacion,
                                    x.idEconomico,
                                    x.Horas,
                                    FechaFin = x.FechaFin.ToString("dd/MM/yyyy"),
                                    x.estatusMaquina,
                                    x.Comentario,
                                    descripcion = x.descripcion,
                                    EstadoMaquina = x.estatusMaquina == 1 ? "Trabajando" : x.estatusMaquina != 0 ? "Stand By" : "",
                                    tipoUsuario = usuario.idPerfil,
                                    HasAutorizaciones = HazSolicitud(x),
                                    idAutorizacion = GetIDAutorizacion(x)
                                });

                            result.Add("EquipoAsignado", res);
                        }
                        break;
                    case 2:
                        {

                            int idUsuario = getUsuario().id;


                            var res = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices()
                                .getMaquinariaAsignadaPendienteAutorizar(cc).Where(c => (idUsuario != 1123 ? c.CC.Equals(cc) : c.idAsignacion.Equals(c.idAsignacion))).Select(x => new
                                {
                                    x.CC,
                                    x.Economico,
                                    x.idAsignacion,
                                    x.idEconomico,
                                    x.Horas,
                                    FechaFin = x.FechaFin.ToString("dd/MM/yyyy"),
                                    x.estatusMaquina,
                                    x.Comentario,
                                    descripcion = x.descripcion,
                                    EstadoMaquina = x.estatusMaquina == 1 ? "Trabajando" : x.estatusMaquina != 0 ? "Stand By" : "",
                                    tipoUsuario = usuario.idPerfil,
                                    HasAutorizaciones = HazSolicitud(x),
                                    idAutorizacion = GetIDAutorizacion(x)
                                });

                            result.Add("EquipoAsignado", res);
                        }
                        break;
                    case 3:
                        {
                            var res = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices()
                              .getMaquinariaAsignada(cc).Where(x => x.estatusMaquina != 1).Select(x => new
                              {
                                  x.CC,
                                  x.Economico,
                                  x.idAsignacion,
                                  x.idEconomico,
                                  x.Horas,
                                  FechaFin = x.FechaFin.ToString("dd/MM/yyyy"),
                                  x.estatusMaquina,
                                  x.Comentario,
                                  descripcion = x.descripcion,
                                  EstadoMaquina = x.estatusMaquina == 1 ? "Trabajando" : x.estatusMaquina != 0 ? "Stand By" : "",
                                  tipoUsuario = usuario.idPerfil,
                                  HasAutorizaciones = HazSolicitud(x),
                                  idAutorizacion = GetIDAutorizacion(x)
                              });
                            result.Add("EquipoAsignado", res);
                        }
                        break;
                    default:
                        break;
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
        private int HazSolicitud(LiberacionDTO x)
        {
            var res = autorizacionStandByFactoryServices.getAutorizacionStandByFactoryServices().GetAutorizacion(x.idAsignacion, x.idEconomico);
            if (res != null)
            {
                if (res.tipoStandBy == 1 && res.usuarioAutoriza != 0)
                {
                    return 1;
                }
                else
                {
                    return res == null ? 1 : res.autorizacion;
                }
            }
            return 1;

        }
        private int GetIDAutorizacion(LiberacionDTO x)
        {
            var res = autorizacionStandByFactoryServices.getAutorizacionStandByFactoryServices().GetAutorizacion(x.idAsignacion, x.idEconomico);

            return res == null ? 0 : res.id;
        }
        public ActionResult UpdateSetStandBy(int idAutorizacion, string Comentario, decimal horasParo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var DatosAutorizacion = autorizacionStandByFactoryServices.getAutorizacionStandByFactoryServices().GetAutorizacionByID(idAutorizacion);

                DatosAutorizacion.autorizacion = 1;
                DatosAutorizacion.usuarioAutoriza = getUsuario().id;
                DatosAutorizacion.comentario = Comentario;
                DatosAutorizacion.horasParo = horasParo;
                var Maquinaria = maquinaFactoryServices.getMaquinaServices().GetMaquina(DatosAutorizacion.idEconomico);
                DatosAutorizacion.CC = Maquinaria.centro_costos;

                autorizacionStandByFactoryServices.getAutorizacionStandByFactoryServices().Guardar(DatosAutorizacion);

                //var Maquinaria = maquinaFactoryServices.getMaquinaServices().GetMaquina(DatosAutorizacion.idEconomico);

                Maquinaria.estatus = DatosAutorizacion.tipoStandBy;
                if (DatosAutorizacion.tipoStandBy == 7)
                {
                    Maquinaria.estatus = 1;
                    var Asignacion = asignacionEquiposFactoryServices.getAsignacionEquiposFactoryServices().getEconomicoAsignado(Maquinaria.id);
                    Asignacion.estatus = 10;
                    asignacionEquiposFactoryServices.getAsignacionEquiposFactoryServices().SaveOrUpdate(Asignacion);
                    Maquinaria.centro_costos = "997";
                }

                maquinaFactoryServices.getMaquinaServices().Guardar(Maquinaria);



                result.Add("EquipoAsignado", null);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EnviarSolicitudStandBy(int idAsignacion, int tipoAccion, string Comentario)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var asignacion = asignacionEquiposFactoryServices.getAsignacionEquiposFactoryServices().GetAsiganacionById(idAsignacion);

                tblM_AutorizacionStandBy obj = new tblM_AutorizacionStandBy
                {
                    usuarioAutoriza = 0,
                    usuarioSolicita = base.getUsuario().id,
                    comentarioSolicitud = Comentario,
                    fechaSolicitud = DateTime.Now,
                    idAsignacion = idAsignacion,
                    tipoStandBy = tipoAccion,
                    estatus = true,
                    comentario = "",
                    idEconomico = asignacion.noEconomicoID,
                    autorizacion = 0,
                    horasParo = 0,
                    fechaAutorizacion = DateTime.Now,
                    CC = asignacion.cc.ToString()
                };


                autorizacionStandByFactoryServices.getAutorizacionStandByFactoryServices().Guardar(obj);

                result.Add("EquipoAsignado", null);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInfoAutorizacionView(int idAsignacion)
        {
            var result = new Dictionary<string, object>();
            var usuario = base.getUsuario();
            try
            {

                var x = asignacionEquiposFactoryServices.getAsignacionEquiposFactoryServices().GetAsiganacionById(idAsignacion);
                var res = autorizacionStandByFactoryServices.getAutorizacionStandByFactoryServices().GetAutorizacion(x.id, x.noEconomicoID);
                ///  var res = autorizacionStandByFactoryServices.getAutorizacionStandByFactoryServices().GetAutorizacion(idAutorizacion,);

                result.Add("Datos", res);


                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetMaquinariaEnObraStandby(string CC, DateTime fechaInicio,DateTime fechaFinal)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var idUsuario = getUsuario();
                var ListaCC = usuarioFactoryServices.getUsuarioService().getCCsUsuario(idUsuario.id).Where(y => y.cc == CC).Select(x => x.cc).ToList();

                if (getAction("CCLIBRE"))
                {

                }

                var Permiso = base.getAction("StandBy");
                var ListaCCName = centroCostosFactoryServices.getCentroCostosService().getListaCC();

                //var HorometrosRango = capturaHorometroFactoryServices.getCapturaHorometroServices().getTableInfoHorometros();


                var listaEquiposObra = standbyFactoryServices.getStandbyFactoryServices().GetListMaquinaria(ListaCC, fechaInicio,fechaFinal).OrderBy(m => m.noEconomico);

                result.Add("listaEquiposObra", listaEquiposObra.Where(y => string.IsNullOrEmpty(CC) ? false : CC.Equals(y.centro_costos)).Select(x => new
                {
                    noEconomicoID = x.noEconomicoID,
                    Economico = x.Economico,
                    Grupo = x.Grupo,
                    Modelo = x.Grupo,
                    cc = x.centro_costos,
                    Lugar = ListaCCName.FirstOrDefault(y => y.Value == x.centro_costos).Text,
                    TipoConsideracion = 1,
                    HorometroInicial = x.HorometroInicial,
                    HorometroFinal = x.HorometroFinal
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
        public ActionResult cboGetCentroCostos()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuarioID = getUsuario().id;
                var listCC = usuarioFactoryServices.getUsuarioService().getCCsUsuario(usuarioID).Select(x => x.cc);
                var cboCentroCostos = centroCostosFactoryServices.getCentroCostosService().getListaCC().Where(x => listCC.Contains(x.Value)).OrderBy(y => y.Value);

                result.Add(ITEMS, cboCentroCostos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarStandBy(List<standByDetDTO> standByDet, standByDTO standByObj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                tblM_CapStandBy obj = new tblM_CapStandBy();

                var Valida = autorizaStandbyFactoryServices.GetAutorizaStandby().GetUsuarioValida(standByObj.id, standByObj.cc); // .getAutorizacionStandByFactoryServices().GetAutorizacion(0, standByObj.cc);

                if (standByObj.id == 0)
                {
                    obj.id = 0;
                    obj.usuarioID = getUsuario().id;
                    obj.FechaCaptura = DateTime.Now;
                    obj.CC = standByObj.cc;
                    obj.FechaInicio = standByObj.fechaInicio;
                    obj.FechaFin = standByObj.fechaFin;
                    obj.UsuarioElabora = getUsuario().id;
                    obj.UsuarioGerente = Valida;
                }
                else
                {
                    obj.id = standByObj.id;
                    obj = standbyFactoryServices.getStandbyFactoryServices().getStandByID(obj.id);


                }
                obj.estatus = 0;
                standbyFactoryServices.getStandbyFactoryServices().GuardarStandBy(obj);
                standbyDetFactoryServices.getStandbyDetFactoryServices().GuardarStandByDet(standByDet, standByObj, obj.id);

                /*   foreach (var item in standByDet)
                   {
                       var objMaquina = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(item.EconomicoID).FirstOrDefault();

                       objMaquina.estatus = 2;
                       maquinaFactoryServices.getMaquinaServices().Guardar(objMaquina);
                   }
                   */
                tblM_AutorizaStandby autorizaStandbyObj = new tblM_AutorizaStandby();

                DateTime fecha = DateTime.Now;
                string f = fecha.ToString("ddmmyyyy") + fecha.Hour + "" + fecha.Minute;


                var standby = autorizacionStandByFactoryServices.getAutorizacionStandByFactoryServices().GetAutorizacionByIDStanby(obj.id);

                if (standby == null)
                {
                    autorizaStandbyObj.id = 0;
                    autorizaStandbyObj.CadenaElabora = obj.id + f + obj.UsuarioElabora + "A";
                    autorizaStandbyObj.CadenaGerente = "";
                    autorizaStandbyObj.FechaElaboro = DateTime.Now;
                    autorizaStandbyObj.FechaValida = DateTime.Now;
                    autorizaStandbyObj.idGerente = obj.UsuarioGerente;
                    autorizaStandbyObj.idElabora = obj.UsuarioElabora;
                    autorizaStandbyObj.standByID = obj.id;

                    autorizaStandbyObj.FirmaElabora = true;
                    autorizaStandbyObj.FirmaGerente = false;
                }
                else
                {
                    autorizaStandbyObj = standby;
                }
                autorizaStandbyFactoryServices.GetAutorizaStandby().Guardar(autorizaStandbyObj);
                List<string> Corres = new List<string>();
                var AsuntoCorreo = @"<html><head>
                                <style>
                                    table {
                                        font-family: arial, sans-serif;
                                        border-collapse: collapse;
                                        width: 100%;
                                    }

                                    td, th {
                                        border: 1px solid #dddddd;
                                        text-align: left;
                                        padding: 8px;
                                    }

                                    tr:nth-child(even) {
                                        background-color: #dddddd;
                                    }
                                </style>
                                </head>
                                <body>
                                    <p>Buen día </p><br/>
                                    <p>Se ha realizado una nueva solicitud de StandBy</p><br/>
                                    <table>
                                        <thead>
                                            <tr>
                                                <th>Economico</th>
                                                <th>CC</th>
                                                <th>Fecha Paro</th>
                                                <th>Tipo Paro</th>
                                            </tr>
                                        </thead>
                                        <tbody>";
                foreach (var i in standByDet)
                {
                    AsuntoCorreo += @"<tr>
                                                <td>" + i.Economico + @"</td>
                                                <td>" + i.cc + @"</td>
                                                <td>" + i.FechaStandBy.ToShortDateString() + @"</td>
                                                <td>" + tipoParo(i.TipoConsideracion) + @"</td>
                                        </tr>";
                }
                AsuntoCorreo += "</tbody>" +
                            @"</table><br/>
                                    <p>Mensaje autogenerado por el sistema SIGOPLAN</p>
                                </body>
                                </html>";
                var GerenteObra = usuarioFactoryServices.getUsuarioService().ListUsersById(obj.UsuarioGerente).FirstOrDefault().correo;
                var Elia = usuarioFactoryServices.getUsuarioService().ListUsersById(1123).FirstOrDefault().correo;
                var Jose = usuarioFactoryServices.getUsuarioService().ListUsersById(4).FirstOrDefault().correo;
                var RicardoPerez = usuarioFactoryServices.getUsuarioService().ListUsersById(1126).FirstOrDefault().correo;
                var Solicita = usuarioFactoryServices.getUsuarioService().ListUsersById(getUsuario().id).FirstOrDefault().correo;

                Corres.Add(Solicita);
                Corres.Add(GerenteObra);
                Corres.Add(Elia);
                Corres.Add(Jose);
               // Corres.Add(RicardoPerez);
                var d1 = solicitudEquipoFactoryServices.getSolicitudEquipoServices().GetAutorizadores(standByObj.cc).FirstOrDefault(x => x.perfilAutorizaID.Equals(2));
                if (d1 != null)
                {
                    try
                    {
                        var temp = usuarioFactoryServices.getUsuarioService().ListUsersById(d1.usuarioID).FirstOrDefault().correo;
                        Corres.Add(temp);
                    }
                    catch (Exception e) { }
                }
                var d2 = solicitudEquipoFactoryServices.getSolicitudEquipoServices().GetAutorizadores(standByObj.cc).FirstOrDefault(x => x.perfilAutorizaID.Equals(4));
                if (d2 != null)
                {
                    try
                    {
                        var temp = usuarioFactoryServices.getUsuarioService().ListUsersById(d2.usuarioID).FirstOrDefault().correo;
                        Corres.Add(temp);
                    }
                    catch (Exception e) { }
                }

                //   GlobalUtils.sendEmail("Notificación StandBy CC: " + standByObj.cc, AsuntoCorreo, Corres);
          //      Session["downloadPDF"] = null;
                var downloadPDF =  (List<Byte[]>)Session["downloadPDF"];
                //    GlobalUtils.sendEmailAdjuntoInMemorySend("Notificación StandBy CC: " + standByObj.cc, AsuntoCorreo, Corres, downloadPDF, "Standby" + standByObj.cc);
                Session["AsuntoCorreoStnby"] = "";
                Session["AsuntoCorreoStnby"] = AsuntoCorreo;

                result.Add("CC", standByObj.cc);
                result.Add("Asunto", "");
                result.Add("Correos", Corres);
                result.Add("id", obj.id);
                result.Add(SUCCESS, true);


            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SendCorreoStanby(string cc, string asunto, List<string> Correos)
        {

            var result = new Dictionary<string, object>();
            try
            {

                asunto = Session["AsuntoCorreoStnby"].ToString();
                var downloadPDF = (List<Byte[]>)Session["downloadPDF"];
               GlobalUtils.sendEmailAdjuntoInMemorySend("Notificación StandBy CC: " + cc, asunto, Correos, downloadPDF, "Standby" + cc);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public string tipoParo(int tipo)
        {
            string r = "";
            if (tipo == 1)
            {
                r = "A";
            }
            else if (tipo == 2)
            {
                r = "B";
            }
            else if (tipo == 3)
            {
                r = "C";
            }
            else if (tipo == 4)
            {
                r = "D";
            }
            else if (tipo == 5)
            {
                r = "E";
            }
            return r;
        }
        public ActionResult ValidarStandBy(int filtro, DateTime fechaInicio, DateTime fechaFin)
        {
            var result = new Dictionary<string, object>();
            try
            {

                DateTime fecha = DateTime.Now;

                DateTime FechaInicio = new DateTime();
                DateTime FechaFin = new DateTime();
                var diaSemana = (int)fecha.DayOfWeek;
                FechaInicio = fecha.AddDays(-(int)fecha.DayOfWeek - 1);
                int diasViernes = ((int)DayOfWeek.Friday - (int)fecha.DayOfWeek + 7) % 7;
                FechaFin = fecha.AddDays(diasViernes);
                var desauto = false;
                if (diaSemana <= 2)
                {
                    desauto = true;
                }


                var getAutoriza = getAction("verAllCC");
                var listCC = new List<string>();
                var id = getUsuario().id;
                if (!getAutoriza)
                {
                    listCC = usuarioFactoryServices.getUsuarioService().getCCsUsuario(id).Select(x => x.cc).ToList();
                }

                var StandyByLista = standbyFactoryServices.getStandbyFactoryServices().getListaStandBy(listCC, fechaInicio, fechaFin, filtro);

                result.Add("StandyByLista", StandyByLista.Select(x => new
                {
                    usuario = usuarioFactoryServices.getUsuarioService().ListUsersById(x.usuarioID).FirstOrDefault().nombre,
                    Fecha1 = x.FechaInicio.ToShortDateString(),
                    Fecha2 = x.FechaFin.ToShortDateString(),
                    Lugar = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(x.CC),
                    id = x.id,
                    estatus = x.estatus,
                    Elabora1 = GetNombreCompleto(x.UsuarioElabora),
                    Elabora2 = GetNombreCompleto(x.UsuarioGerente),
                    cc = x.CC,
                    validaV = getUsuario().id == x.UsuarioElabora ? true : false,
                    validaVg = getUsuario().id == x.UsuarioGerente ? true : false,
                    desauto = desauto

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
        private string GetNombreCompleto(int id)
        {
            var usuario = usuarioFactoryServices.getUsuarioService().ListUsersById(id).FirstOrDefault();

            return usuario.nombre + " " + usuario.apellidoPaterno + " " + usuario.apellidoMaterno;
        }
        public ActionResult ValidarStandByLista()
        {
            var result = new Dictionary<string, object>();
            try
            {
                DateTime fecha = DateTime.Now;

                DateTime FechaInicio = new DateTime();
                DateTime FechaFin = new DateTime();
                var diaSemana = (int)fecha.DayOfWeek;
                FechaInicio = fecha.AddDays(-(int)fecha.DayOfWeek - 1);
                int diasViernes = ((int)DayOfWeek.Friday - (int)fecha.DayOfWeek + 7) % 7;
                FechaFin = fecha.AddDays(diasViernes);

                if (diaSemana <= 1)
                {
                    FechaInicio = FechaInicio.AddDays(-(int)FechaInicio.DayOfWeek - 1);
                }

                var getAutoriza = getAction("verAllCC");
                var listCC = new List<string>();
                var id = getUsuario().id;
                if (!getAutoriza)
                {
                    listCC = usuarioFactoryServices.getUsuarioService().getCCsUsuario(id).Select(x => x.cc).ToList();
                }

                var StandyByLista = standbyFactoryServices.getStandbyFactoryServices().getListaStandBy(listCC, FechaInicio, FechaFin, 0).ToList().Where(x => x.FechaInicio.Date >= FechaInicio.Date && FechaFin.Date <= x.FechaFin.Date);

                result.Add("StandyByLista", StandyByLista.Select(x => new
                {
                    usuario = usuarioFactoryServices.getUsuarioService().ListUsersById(x.usuarioID).FirstOrDefault().nombre,
                    Fecha1 = x.FechaInicio.ToShortDateString(),
                    Fecha2 = x.FechaFin.ToShortDateString(),
                    Lugar = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(x.CC),
                    id = x.id,
                    estatus = x.estatus,
                    Elabora1 = GetNombreCompleto(x.UsuarioElabora),
                    Elabora2 = GetNombreCompleto(x.UsuarioGerente),
                    cc = x.CC,
                    validaV = getUsuario().id == x.UsuarioElabora ? true : false,
                    validaVg = getUsuario().id == x.UsuarioGerente ? true : false

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
        public ActionResult saveOrUpdateAutorizacionStandby(int obj, int estatus)
        {
            var result = new Dictionary<string, object>();
            try
            {
                // tblM_CapStandBy newObj = new tblM_CapStandBy();

                var newObj = standbyFactoryServices.getStandbyFactoryServices().getStandByID(obj);
                if (estatus == 1)
                {
                    newObj.estatus = estatus;
                }


                standbyFactoryServices.getStandbyFactoryServices().GuardarStandBy(newObj);
                var newobjAutoriza = autorizaStandbyFactoryServices.GetAutorizaStandby().getAutorizacionesbyStandbyID(obj);

                var standByDet = standbyDetFactoryServices.getStandbyDetFactoryServices().getListaDetStandBy(obj);

                foreach (var item in standByDet)
                {
                    var objMaquina = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(item.noEconomicoID).FirstOrDefault();
                    if (estatus == 1)
                    {
                        objMaquina.estatus = 2;
                    }
                    else
                    {
                        objMaquina.estatus = 1;
                    }

                    maquinaFactoryServices.getMaquinaServices().Guardar(objMaquina);
                }

                tblM_AutorizaStandby autorizaStandbyObj = new tblM_AutorizaStandby();

                DateTime fecha = DateTime.Now;
                string f = fecha.ToString("ddmmyyyy") + fecha.Hour + "" + fecha.Minute;

                if (estatus == 1)
                {
                    newobjAutoriza.FirmaGerente = true;
                    newobjAutoriza.CadenaGerente = obj + f + getUsuario().id;
                }
                else
                {
                    newobjAutoriza.FirmaGerente = false;
                    newobjAutoriza.CadenaGerente = "";
                    newobjAutoriza.FirmaElabora = false;
                    newobjAutoriza.CadenaElabora = "";

                }

                autorizaStandbyFactoryServices.GetAutorizaStandby().Guardar(newobjAutoriza);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetMaquinariaEnObraStandbyEdit(int obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var idUsuario = getUsuario();
                var ListaCC = new List<string>();
                var Permiso = base.getAction("StandBy");
                var ListaCCName = centroCostosFactoryServices.getCentroCostosService().getListaCC();
                var DetalleStanby = standbyDetFactoryServices.getStandbyDetFactoryServices().getListaDetStandBy(obj).ToList();


                var StandbyO = standbyFactoryServices.getStandbyFactoryServices().getStandByID(obj);
                if (DetalleStanby.Count > 0)
                {
                    if (StandbyO != null)
                    {
                        result.Add("CC", StandbyO.CC);
                    }


                    var StandbyOBj = DetalleStanby.FirstOrDefault().StandBy;

                    ListaCC.Add(StandbyOBj.CC);

                    var listaEquiposObra = standbyFactoryServices.getStandbyFactoryServices().GetListMaquinaria(ListaCC, StandbyOBj.FechaInicio, StandbyOBj.FechaFin).OrderBy(m => m.noEconomico);



                    result.Add("listaEquiposObra", listaEquiposObra.Where(y => string.IsNullOrEmpty(StandbyOBj.CC) ? false : StandbyOBj.CC.Equals(y.centro_costos) && !DetalleStanby.Select(x => x.noEconomicoID).ToList().Contains(y.noEconomicoID)).Select(x => new
                    {
                        noEconomicoID = x.noEconomicoID,
                        Economico = x.Economico,
                        Grupo = x.Grupo,
                        Modelo = x.Modelo,
                        cc = x.centro_costos,
                        Lugar = ListaCCName.FirstOrDefault(y => y.Value == x.centro_costos).Text,
                        TipoConsideracion = 1,
                        HorometroInicial = x.HorometroInicial,
                        HorometroFinal = x.HorometroFinal
                    }));


                    result.Add("listaEquiposObraStandby",

                        DetalleStanby.Select(x => new
                        {
                            FechaStandBy = x.DiaParo.ToShortDateString(),
                            FechaCaptura = x.FechaCaptura.ToShortDateString(),
                            FechaFinStandby = x.FechaFinStandby.ToShortDateString(),
                            HorometroFinal = x.HorometroFinal,
                            HorometroInicial = x.HorometroInicial,
                            StandByID = x.StandByID,
                            TipoConsideracion = x.TipoConsideracion,
                            estatus = x.estatus,
                            id = x.id,
                            noEconomicoID = x.noEconomicoID,
                            Economico = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(x.noEconomicoID).FirstOrDefault().noEconomico,
                            Grupo = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(x.noEconomicoID).FirstOrDefault().grupoMaquinaria.descripcion,
                            Modelo = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(x.noEconomicoID).FirstOrDefault().modeloEquipo.descripcion,
                            cc = x.StandBy.CC,
                            Lugar = x.StandBy.CC + " - " + centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(x.StandBy.CC),
                        }));
                    result.Add("StandbyOBj", StandbyOBj);
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
        public ActionResult DeleteRow(int objStanby, int objEconomico)
        {

            var result = new Dictionary<string, object>();
            try
            {
                var ObjDetStandby = standbyDetFactoryServices.getStandbyDetFactoryServices().getListaDetStandBy(objStanby);
                var objDetSingle = ObjDetStandby.FirstOrDefault(x => x.noEconomicoID == objEconomico);
                standbyDetFactoryServices.getStandbyDetFactoryServices().DeleteRow(objDetSingle);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FechaInicioFin()
        {
            var result = new Dictionary<string, object>();
            try
            {
                DateTime fecha = DateTime.Now;

                DateTime FechaInicio = new DateTime();
                DateTime FechaFin = new DateTime();
                var diaSemana = (int)fecha.DayOfWeek;
                FechaInicio = fecha.AddDays(-(int)fecha.DayOfWeek - 1);
                int diasViernes = ((int)DayOfWeek.Friday - (int)fecha.DayOfWeek + 7) % 7;
                FechaFin = fecha.AddDays(diasViernes);

                result.Add("FechaInicio", FechaInicio.ToShortDateString());
                result.Add("FechaFin", FechaFin.ToShortDateString());

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}