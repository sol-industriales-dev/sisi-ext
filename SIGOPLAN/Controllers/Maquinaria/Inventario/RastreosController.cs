using Core.DTO.Principal.Generales;
using Core.Entity.Maquinaria.Inventario;
using Data.Factory.Maquinaria.Catalogos;
using Data.Factory.Maquinaria.Inventario;
using Data.Factory.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SIGOPLAN.Controllers.Maquinaria.Inventario
{
    public class RastreosController : BaseController
    {
        private SolicitudEquipoDetFactoryServices solicitudEquipoDetFactoryServices = new SolicitudEquipoDetFactoryServices();
        private MaquinaFactoryServices maquinaFactoryServices = new MaquinaFactoryServices();
        private UsuarioFactoryServices usuarioFactoryServices = new UsuarioFactoryServices();
        private AsignacionEquiposFactoryServices asignacionEquiposFactoryServices = new AsignacionEquiposFactoryServices();
        private CentroCostosFactoryServices centroCostosFactoryServices = new CentroCostosFactoryServices();
        private SolicitudEquipoFactoryServices solicitudEquipoFactoryServices = new SolicitudEquipoFactoryServices();


        // GET: Rastreos
        public ActionResult RastreSolicitudes()
        {
            return View();
        }
        public ActionResult ReporteVacantes()
        {
            return View();
        }
        public ActionResult ReporteTiemposSolicitud()
        {
            return View();
        }

        public ActionResult ReporteEquiposPendientePorSurtir()
        {
            return View();
        }
        public ActionResult tblSolicitudesEquipoPendientesAsignacion(string cc, int Tipo, int grupo)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var res = solicitudEquipoFactoryServices.getSolicitudEquipoServices().getEquiposPendientes(cc, Tipo, grupo);
                var res2 = solicitudEquipoFactoryServices.getSolicitudEquipoServices().getEquiposReemplazo(cc, Tipo, grupo);

                Session["rptEquiposPendientesDTO"] = res;

                Session["rptEquiposPendientesReemplazoDTO"] = res2;
                result.Add("DataTables", res);
                result.Add("DataTables2", res2);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult tblPrincipal(string JsonData, string Folio, string Economico, string Estado)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuarioID = base.getUsuario().id;
                var LoadLista = asignacionEquiposFactoryServices.getAsignacionEquiposFactoryServices().GetListaAsignaciones().Where(x=>x.Economico!=null).ToList();
                var listaCC = centroCostosFactoryServices.getCentroCostosService().getListaCCConstruplan();

                ComboDTO dato = new ComboDTO();
                dato.Text = "MONTAJE ELECTRICO OHL";
                dato.Value = "70";
                listaCC.Add(dato);
                dato = new ComboDTO();
                dato.Text = "PATIO DE MAQUINARIA";
                dato.Value = "1010";
                listaCC.Add(dato);
                dato = new ComboDTO();
                dato.Text = "TALLER DE MAQUINARIA";
                dato.Value = "1015";
                listaCC.Add(dato);


                var ListaEstatus = LoadLista.Where(c => (string.IsNullOrEmpty(Economico) ? true : c.Economico.StartsWith(Economico)) &&
                        (string.IsNullOrEmpty(Folio) ? true : c.folio.StartsWith(Folio)) &&
                         (string.IsNullOrEmpty(Estado) ? true : c.estatus == Convert.ToInt32(Estado))
                        ).ToList();
                var data = ListaEstatus.Select(x => new
                {
                    Folio = x.folio,
                    LugarRecepcion = listaCC.FirstOrDefault(y => y.Value == x.cc).Text, //centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(x.cc),
                    LugarOrigen = string.IsNullOrEmpty(x.CCOrigen) || x.CCOrigen == "0" ? "En espera" : listaCC.FirstOrDefault(y => y.Value == x.CCOrigen).Text,
                    Economico = GetEconomico(x.noEconomicoID, x.Economico),
                    TipoEconomico = GetTipoEconomico(x.Economico),
                    FechaPromesa = x.FechaPromesa.ToShortDateString(),
                    Estatus = x.estatus,
                    idEconomico = x.noEconomicoID,
                    Descripcion = solicitudEquipoDetFactoryServices.getSolicitudEquipoDetServices().DetSolicitud(x.SolicitudDetalleId) != null ? solicitudEquipoDetFactoryServices.getSolicitudEquipoDetServices().DetSolicitud(x.SolicitudDetalleId).GrupoMaquinaria.descripcion : ""
                }).ToList();


                //var jsonSerialiser = new JavaScriptSerializer();
                //var json = jsonSerialiser.Serialize(LoadLista);


                result.Add("ListaEstatus", data);
                result.Add("dataSetJson", "");
                //result.Add("dataSetJson", json);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public string GetEconomico(int obj, string noEconomico)
        {
            if (obj != 0)
            {
                var economico = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(obj).FirstOrDefault();
                if (economico != null)
                {
                    if (economico.noEconomico != null)
                    {
                        return economico.noEconomico;
                    }
                    else
                    {
                        return "Pendiente Economico";
                    }
                }
                else
                {
                    return "En proceso " + noEconomico;
                }
            }
            else
            {
                return "En proceso " + noEconomico;
            }
        }
        public string GetTipoEconomico(string noEconomico)
        {
            var economico = maquinaFactoryServices.getMaquinaServices().GetMaquinaByNoEconomico(noEconomico);

            return economico == null ? noEconomico : "PROPIO";
        }

        public ActionResult ActualizarTabla(string JsonData, string Folio, string Economico, string Estado)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuarioID = base.getUsuario().id;

                var LoadLista = asignacionEquiposFactoryServices.getAsignacionEquiposFactoryServices().GetListaAsignaciones();
                var jsonSerialiser = new JavaScriptSerializer();
                var json = jsonSerialiser.Serialize(LoadLista);

                if (json.Equals(JsonData))
                {
                    result.Add(SUCCESS, false);
                }
                else
                {
                    var listaCC = centroCostosFactoryServices.getCentroCostosService().ListCC();

                    ComboDTO dato = new ComboDTO();
                    dato.Text = "MONTAJE ELECTRICO OHL";
                    dato.Value = "70";
                    listaCC.Add(dato);
                    dato = new ComboDTO();
                    dato.Text = "PATIO DE MAQUINARIA";
                    dato.Value = "1010";
                    listaCC.Add(dato);
                    dato = new ComboDTO();
                    dato.Text = "TALLER DE MAQUINARIA";
                    dato.Value = "1015";
                    listaCC.Add(dato);

                    var ListaEstatus = LoadLista.Where(c => (string.IsNullOrEmpty(Economico) ? c.id == c.id : c.Economico.Contains(Economico)) &&
                        (string.IsNullOrEmpty(Folio) ? c.id == c.id : c.folio.Contains(Folio)) &&
                         (Estado == "0" ? c.id == c.id : c.estatus == Convert.ToInt32(Estado))
                        ).Select(x => new
                    {
                        Folio = x.folio,
                        LugarRecepcion = listaCC.FirstOrDefault(y => y.Value == x.cc).Text, //centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(x.cc),
                        LugarOrigen = string.IsNullOrEmpty(x.CCOrigen) || x.CCOrigen == "0" ? "En espera" : listaCC.FirstOrDefault(y => y.Value == x.CCOrigen).Text,
                        Economico = GetEconomico(x.noEconomicoID, x.Economico),
                        TipoEconomico = GetTipoEconomico(x.Economico),
                        FechaPromesa = x.FechaPromesa.ToShortDateString(),
                        Estatus = x.estatus,
                        idEconomico = x.noEconomicoID
                    });
                    result.Add("ListaEstatus", ListaEstatus);
                    result.Add("dataSetJson", json);

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
        public ActionResult RptVacantesAsignados(string CentroCostos)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var SolicitudesRealizadas = solicitudEquipoFactoryServices.getSolicitudEquipoServices().GetSolicitudesEquipo(CentroCostos);

                result.Add("DataTables", SolicitudesRealizadas);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult RptDetalleVacantes(int idSolicitud)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var SolicitudesRealizadas = solicitudEquipoFactoryServices.getSolicitudEquipoServices().GetDetalleSolicitud(idSolicitud);

                result.Add("DataTables", SolicitudesRealizadas);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RptDetalleVacantesGrupo(int idSolicitud, int idGrupo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var SolicitudesRealizadas = solicitudEquipoFactoryServices.getSolicitudEquipoServices().GetDataSolicitudesGrupo(idSolicitud, idGrupo);


                result.Add("DataTables", SolicitudesRealizadas);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RptListaEquiposAsignados(List<string> CentroCostos, DateTime pFechaInicio, DateTime pFechaFin)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var SolicitudesRealizadas = solicitudEquipoFactoryServices.getSolicitudEquipoServices().getTiemposAsignacion(CentroCostos).ToList();

                List<rptTiemposEntreAutorizaciones> listReporte = new List<rptTiemposEntreAutorizaciones>();
                foreach (var item in SolicitudesRealizadas)
                {
                    rptTiemposEntreAutorizaciones rptA = new rptTiemposEntreAutorizaciones();

                    if (item.SolicitudEquipo.fechaElaboracion >= pFechaInicio && item.SolicitudEquipo.fechaElaboracion <= pFechaFin)
                    {
                        rptA.folio = item.folio;

                        if (item.FechaElabora != null)
                        {
                            var data = asignacionEquiposFactoryServices.getAsignacionEquiposFactoryServices().getAsignacionesByID(item.solicitudEquipoID);

                            if (data.Count > 0)
                            {
                                var FechaAsignacion = data.FirstOrDefault().fechaAsignacion;

                                rptA.FechaAdministrador = item.FechaElabora != null ? item.FechaElabora.Value.ToShortDateString() : "";
                                rptA.FechaGerente = item.FechaGerenteObra != null ? item.FechaGerenteObra.Value.ToShortDateString() : "";
                                rptA.DirectorArea = item.FechaGerenteDirector != null ? item.FechaGerenteDirector.Value.ToShortDateString() : "";
                                rptA.DirectorDivision = item.FechaDirectorDivision != null ? item.FechaDirectorDivision.Value.ToShortDateString() : "";
                                rptA.DirectorGeneral = item.FechaDireccion != null ? item.FechaDireccion.Value.ToShortDateString() : "";
                                rptA.Asigno = FechaAsignacion != null ? FechaAsignacion.ToShortDateString() : "";
                                DateTime FechaInicio = (DateTime)item.FechaElabora;
                                DateTime FechaFin = FechaAsignacion;

                                if (FechaAsignacion != null)
                                {
                                    var FechaFinal = FechaFin - FechaInicio;

                                    rptA.TiempoTotal = FechaFinal.Days.ToString();
                                }
                                else
                                {
                                    rptA.TiempoTotal = "No Definido";
                                }

                                listReporte.Add(rptA);
                            }

                        }
                    }
                }

                Session["rptTiemposAutorizacion"] = listReporte;


                result.Add("DataTables", listReporte);
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