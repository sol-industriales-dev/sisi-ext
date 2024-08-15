using Core.DTO;
using Core.DTO.Maquinaria.Captura;
using Core.DTO.Maquinaria.Captura.OT;
using Core.DTO.Maquinaria.Captura.OT.rptConcentradoHH;
using Core.DTO.Maquinaria.Catalogos;
using Core.DTO.Principal.Generales;
using Core.DTO.RecursosHumanos;
using Core.DTO.Utils.Excel;
using Core.Entity.Maquinaria;
using Core.Entity.Maquinaria.BackLogs;
using Core.Entity.Maquinaria.Captura;
using Core.Entity.Maquinaria.Catalogo;
using Core.Enum.Maquinaria.ot;
using Data.Factory.Maquinaria.Captura;
using Data.Factory.Maquinaria.Captura.HorasHombre;
using Data.Factory.Maquinaria.Catalogos;
using Data.Factory.Maquinaria.Inventario;
using Data.Factory.Principal.Usuarios;
using Infrastructure.Utils;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Maquinaria.Capturas
{
    public class OTController : BaseController
    {

        MotivosParoFactoryServices motivosParoFactoryServices;
        CapturaOTFactoryServices capturaOTFactoryServices;
        CapturaOTDetFactoryServices capturaOTDetFactoryServices;

        CentroCostosFactoryServices centroCostosFactoryServices;
        UsuarioFactoryServices usuarioFactoryServices;
        MaquinaFactoryServices maquinaFactoryServices;
        KPIFactoryServices kpiFactoryServices;
        CapturaHorometroFactoryServices capturaHorometroFactoryServices;
        CapHorasHombreFactoryServices capHorasHombreFactoryServices;
        

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            motivosParoFactoryServices = new MotivosParoFactoryServices();
            usuarioFactoryServices = new UsuarioFactoryServices();
            capturaOTFactoryServices = new CapturaOTFactoryServices();
            capturaOTDetFactoryServices = new CapturaOTDetFactoryServices();

            centroCostosFactoryServices = new CentroCostosFactoryServices();
            maquinaFactoryServices = new MaquinaFactoryServices();
            kpiFactoryServices = new KPIFactoryServices();
            capturaHorometroFactoryServices = new CapturaHorometroFactoryServices();
            capHorasHombreFactoryServices = new CapHorasHombreFactoryServices();
            base.OnActionExecuting(filterContext);
        }
        // GET: OT
        public ActionResult CapturaOT()
        {
            return View();
        }
        public ActionResult CapturaHH()
        {
            return View();
        }
        public ActionResult rptConcentradoHH()
        {
            return View();
        }

        public ActionResult KPI()
        {
            return View();
        }

        public ActionResult rptHorasHombre()
        {
            return View();
        }

        public ActionResult rptAnalisisFrecuenciaParos()
        {
            return View();
        }

        public ActionResult SetRptMaquinariaHorasHombre(List<int> economicos, DateTime fechaInicio, DateTime fechaFin, List<int> Puestos, List<int> Empleados, string cc)
        {
            var result = new Dictionary<string, object>();
            try
            {
                fechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);
                capHorasHombreFactoryServices.getCapHorasHombreFactoryServices().setRptMaquinariaHorasHombre(economicos, fechaInicio, fechaFin, Puestos, Empleados, cc);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboCategorias()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var dataResult = capHorasHombreFactoryServices.getCapHorasHombreFactoryServices().getCategorias().Select(x => new ComboDTO { Text = x.descripcion, Value = x.id.ToString() });
                result.Add(ITEMS, dataResult);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboSubCategorias(List<int> listCategorias)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var ListacomboDTO = capHorasHombreFactoryServices.getCapHorasHombreFactoryServices().getSubCategorias(listCategorias);

                result.Add(ITEMS, ListacomboDTO.Select(x => new ComboDTO { Value = x.id.ToString(), Text = x.descripcion }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult cboFillEconomicos(string cc)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var idUsuario = getUsuario().id;
                var listaCC = usuarioFactoryServices.getUsuarioService().getCCsUsuario(idUsuario).Where(c => c.cc == cc).Select(x => x.cc).ToList();

                var economicosExcepciones = getExcepciones();


                var listaCCusuario = usuarioFactoryServices.getUsuarioService().getCCsByUsuario(idUsuario).Select(x=>x.areaCuenta).ToList();
                
                List<ComboDTO> res = maquinaFactoryServices.getMaquinaServices().fillCboNoEconomicosCC((string.IsNullOrEmpty(cc)?listaCCusuario:listaCC)).ToList();
                //List<ComboDTO> res = maquinaFactoryServices.getMaquinaServices().getCboMaquinaria("0").Where(t => listaCC.Contains(t.centro_costos)
                //    && t.grupoMaquinaria.tipoEquipoID == 1
                //    ).Select(x => new ComboDTO { Value = x.id.ToString(), Text = x.noEconomico }).OrderBy(x => x.Text).ToList();

                //List<ComboDTO> res2 = maquinaFactoryServices.getMaquinaServices().getCboMaquinaria("0").Where(t => listaCC.Contains(t.centro_costos)
                //         && economicosExcepciones.Contains(t.grupoMaquinariaID)
                // ).Select(x => new ComboDTO { Value = x.id.ToString(), Text = x.noEconomico }).OrderBy(x => x.Text).ToList();
                //res.AddRange(res2);


                result.Add(ITEMS, res);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private List<int> getExcepciones()
        {
            List<int> listaGRupo = new List<int>();

            listaGRupo.Add(210);
            listaGRupo.Add(286);
            listaGRupo.Add(287);
            listaGRupo.Add(295);
            listaGRupo.Add(296);

            return listaGRupo;
        }

        public ActionResult DeletePersonal(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (id != 0)
                {
                    capturaOTDetFactoryServices.getCapturaOTDetFactoryServices().delete(id);
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

        public ActionResult LoadTableOT(string cc, DateTime FechaInicio, DateTime FechaFin, int economicos, int tipoParo)
        {
            var result = new Dictionary<string, object>();
            try
            {

                FechaFin = FechaFin.AddHours(23).AddMinutes(59);

                var listaCC = usuarioFactoryServices.getUsuarioService().getCCsUsuario(getUsuario().id).Select(x => x.cc).ToList();
                var objOT = capturaOTFactoryServices.getCapturaOTFactoryServices().getListaOT(cc, listaCC).Where(y => (y.FechaEntrada >= FechaInicio && y.FechaEntrada <= FechaFin)).ToList();
                var res = objOT.Where(x => x.EstatusOT == false && (economicos != 0 ? x.EconomicoID == economicos : x.id == x.id) && (tipoParo != 0 ? x.MotivoParo == tipoParo : x.id == x.id)).Select(x => new
                {
                    id = x.id,
                    Folio = x.id.ToString().PadLeft(6, '0'),
                    Lugar = x.CC+" - "+centroCostosFactoryServices.getCentroCostosService().getNombreAreaCuent(x.CC),
                    Economico = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(x.EconomicoID).FirstOrDefault().noEconomico,
                    FechaInicio = ((DateTime)x.FechaEntrada).ToShortDateString(),
                    FechaFin = x.FechaSalida == null ? "" : ((DateTime)x.FechaSalida).ToShortDateString(),
                    Motivo = x.MotivoParo,
                    Estatus = x.FechaSalida == null ? "Abierto" : "Cerrada"
                });
                var res2 = objOT.Where(x => x.EstatusOT == true && (economicos != 0 ? x.EconomicoID == economicos : x.id == x.id) && (tipoParo != 0 ? x.MotivoParo == tipoParo : x.id == x.id)).Select(x => new
                {
                    id = x.id,
                    Folio = x.id.ToString().PadLeft(6, '0'),
                    Lugar = x.CC + " - " + centroCostosFactoryServices.getCentroCostosService().getNombreAreaCuent(x.CC),
                    Economico = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(x.EconomicoID).FirstOrDefault().noEconomico,
                    FechaInicio = ((DateTime)x.FechaEntrada).ToShortDateString(),
                    FechaFin = x.FechaSalida == null ? "" : ((DateTime)x.FechaSalida).ToShortDateString(),
                    Motivo = x.MotivoParo,
                    Estatus = x.FechaSalida == null ? "Abierto" : "Cerrada"
                });

                result.Add("TablaOTC", res);

                result.Add("TablaOTA", res2);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //tblRH_CatEmpleados CatEmpleado = new tblRH_CatEmpleados();
        public ActionResult GetInfoOT(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var objOT = capturaOTFactoryServices.getCapturaOTFactoryServices().GetOTbyID(id);

                var objDet = capturaOTDetFactoryServices.getCapturaOTDetFactoryServices().getListaOTDet(id);

                var maquina = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(objOT.EconomicoID).FirstOrDefault();

                result.Add("maquina", maquina.id);
                result.Add("Modelo", maquina.modeloEquipo.descripcion);
                result.Add("CCMaquina", maquina.centro_costos);

                result.Add("CCName", centroCostosFactoryServices.getCentroCostosService().getNombreAreaCuent(objOT.CC));
                result.Add("objOT", objOT);


                var motivoParoDet = motivosParoFactoryServices.getMotivosParoFactoryServices().getMotivosParo(objOT.MotivoParo);

                var getHorometro = capturaHorometroFactoryServices.getCapturaHorometroServices().getHorometro(maquina.noEconomico, objOT.FechaEntrada);

                result.Add("horometroValida", getHorometro != null ? 0 : getHorometro.FirstOrDefault().Horometro);

                result.Add("DescMotivoParo", motivoParoDet.DescripcionParo);

                List<EmpleadoOTDTO> ListaEmpleados = new List<EmpleadoOTDTO>();
                foreach (var item in objDet)
                {
                    var Empleado = capturaOTDetFactoryServices.getCapturaOTDetFactoryServices().getCatEmpleados(item.PersonalID.ToString());

                    EmpleadoOTDTO EmpleadoOT = new EmpleadoOTDTO();

                    EmpleadoOT.Accion = "<div> <button class='btn btn-danger removeRow'data-idrow='" + item.id + "' type='button' > " +
                                            "<span class='glyphicon glyphicon-remove'></span></button> " +
                                        "</div>";
                    EmpleadoOT.HorasTrabajo = (decimal)(item.HoraFin - item.HoraInicio).TotalHours;
                    EmpleadoOT.NombreE = Empleado.Nombre;
                    EmpleadoOT.OrdenTrabajoID = item.OrdenTrabajoID;

                    EmpleadoOT.PersonalID = item.PersonalID;
                    EmpleadoOT.PuestoE = Empleado.Puesto;
                    EmpleadoOT.id = item.id;
                    EmpleadoOT.HoraInicio = item.HoraInicio;
                    EmpleadoOT.HoraFin = item.HoraFin;
                    EmpleadoOT.Tipo = item.Tipo;

                    ListaEmpleados.Add(EmpleadoOT);

                }
                result.Add("objDet", ListaEmpleados.Select(EmpleadoOT => new
                {
                    Accion = EmpleadoOT.Accion,
                    HorasTrabajo = EmpleadoOT.HorasTrabajo,
                    NombreE = EmpleadoOT.NombreE,
                    OrdenTrabajoID = EmpleadoOT.OrdenTrabajoID,

                    PersonalID = EmpleadoOT.PersonalID,
                    PuestoE = EmpleadoOT.PuestoE,
                    id = EmpleadoOT.id,
                    HoraInicio = EmpleadoOT.HoraInicio.ToShortDateString() + " " + EmpleadoOT.HoraInicio.ToString("HH:mm"),
                    HoraFin = EmpleadoOT.HoraFin.ToShortDateString() + " " + EmpleadoOT.HoraFin.ToString("HH:mm"),
                    Tipo = EmpleadoOT.Tipo,

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

        public ActionResult GetInfoEconomico(int idEconomico)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var objEconomico = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(idEconomico).Select(x => new
                {
                    Economico = x.noEconomico,
                    Modelo = x.modeloEquipo.descripcion,
                    ModeloID = x.id,
                    CCName = centroCostosFactoryServices.getCentroCostosService().getNombreAreaCuent(x.centro_costos),
                    CC = x.centro_costos,
                    tipoEquipo = x.grupoMaquinaria.tipoEquipo.id
                }).FirstOrDefault();

                var horometro = capturaHorometroFactoryServices.getCapturaHorometroServices().GetHorometroFinal(objEconomico.Economico);

                var UltimaOT = capturaOTFactoryServices.getCapturaOTFactoryServices().GetOTByEconomico(idEconomico);

                if (UltimaOT != null)
                {
                    result.Add("UltimaOT", UltimaOT);
                }
                else
                {
                    result.Add("UltimaOT", null);
                }

                result.Add("dataEconomico", objEconomico);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetHorometroActuales(int idEconomico, DateTime tbHoraEntradaHH)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var Economico = maquinaFactoryServices.getMaquinaServices().EconomicoNotNull(idEconomico);


                if (Economico != null)
                {
                    var ListaHorometro = capturaHorometroFactoryServices.getCapturaHorometroServices().getHorometro(Economico.noEconomico, tbHoraEntradaHH).FirstOrDefault().Horometro;
                    result.Add("ListaHorometro", ListaHorometro);
                }

                else
                {
                    result.Add("ListaHorometro", 0);
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

        public ActionResult getEmpleados(string term)
        {
            var CentroCostosUsuario = GetListaCentroCostos().Select(x => x.cc).ToList();
            var items = capturaOTFactoryServices.getCapturaOTFactoryServices().getCatEmpleados(term, CentroCostosUsuario);
            var filteredItems = items.Select(x => new { id = x.clave_empleado, label = x.Nombre });

            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarCapturaOT(tblM_CapOrdenTrabajo objCapturaOT, List<tblM_DetOrdenTrabajo> objListPersonal, int idBL)
        {

            var result = new Dictionary<string, object>();
            try
            {
                #region SE CREA LA OT
                objCapturaOT.FechaCreacion = DateTime.Now;

                var c = objCapturaOT.FechaSalida;

                objCapturaOT.usuarioCapturaID = getUsuario().id;
                capturaOTFactoryServices.getCapturaOTFactoryServices().Guardar(objCapturaOT, idBL);

                int idSolicitud = objCapturaOT.id;

                if (objListPersonal != null)
                {
                    capturaOTDetFactoryServices.getCapturaOTDetFactoryServices().Guardar(objListPersonal, idSolicitud);
                }
                result.Add("folio", objCapturaOT.id.ToString().PadLeft(6, '0'));
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

        public ActionResult getKPIGeneral(List<string> CC, int Tipo, int Modelo, DateTime Fechainicio, DateTime FechaFin)
        {
            var result = new Dictionary<string, object>();
            try
            {

                FechaFin = FechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);
                var data = kpiFactoryServices.getKPIFactoryService().getKPIGeneral(CC, Tipo, Modelo, Fechainicio, (DateTime)FechaFin);

                result.Add("dataMain", data.ToList().OrderBy(x => x.economico));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getMDTipoMantenimiento(int id, List<string> cc, DateTime Fechainicio, DateTime FechaFin, List<tblM_CatMaquina> _lstCatMaquinas = null, List<tblM_CatCriteriosCausaParo> _lstCatCriteriosCausaParo = null, List<tblM_CapOrdenTrabajo> _lstCapOrdenTrabajo = null)
        {
            var result = new Dictionary<string, object>();
            try
            {
                FechaFin = FechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);
                var data = kpiFactoryServices.getKPIFactoryService().getMDTipoMantenimiento(id, cc, Fechainicio, FechaFin, _lstCatMaquinas, _lstCatCriteriosCausaParo, _lstCapOrdenTrabajo);

                result.Add("tdTPTiempo", data.tdTPTiempo);
                result.Add("tdTPCantidad", data.tdTPCantidad);
                result.Add("tdTPPTiempo", data.tdTPPTiempo);
                result.Add("tdTPPCantidad", data.tdTPPCantidad);
                result.Add("tdTPTotal", data.tdTPTotal);
                result.Add("tdTPTotal2", data.tdTPTotal2);
                result.Add("tdTNPTiempo", data.tdTNPTiempo);
                result.Add("tdTNPCantidad", data.tdTNPCantidad);
                result.Add("tdTNPPTiempo", data.tdTNPPTiempo);
                result.Add("tdTNPPCantidad", data.tdTNPPCantidad);
                result.Add("tdPTiempo", data.tdPTiempo);
                result.Add("tdPCantidad", data.tdPCantidad);
                result.Add("tdPPTiempo", data.tdPPTiempo);
                result.Add("tdPPCantidad", data.tdPPCantidad);
                result.Add("tdPTotal", data.tdPTotal);
                result.Add("tdPTotal2", data.tdPTotal2);
                result.Add("tdCTiempo", data.tdCTiempo);
                result.Add("tdCCantidad", data.tdCCantidad);
                result.Add("tdCPTiempo", data.tdCPTiempo);
                result.Add("tdCPCantidad", data.tdCPCantidad);
                result.Add("tdPrTiempo", data.tdPrTiempo);
                result.Add("tdPrCantidad", data.tdPrCantidad);
                result.Add("tdPrPTiempo.", data.tdPrPTiempo);
                result.Add("tdPrPCantidad", data.tdPrPCantidad);
                result.Add("tdPTotalF", data.tdPTotalF);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getInfoGeneral(int id, List<string> cc, DateTime Fechainicio, DateTime FechaFin, List<tblM_CatMaquina> _lstCatMaquinas = null, List<tblM_CapOrdenTrabajo> _lstCapOrdenTrabajo = null,
                                                    List<tblM_CapHorometro> _lstCapHorometro = null, List<tblM_DetOrdenTrabajo> _lstDetOrdenTrabajo = null, List<tblM_CatCriteriosCausaParo> _lstCatCriteriosCausaParo = null)
        {
            var result = new Dictionary<string, object>();
            try
            {
                FechaFin = FechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);
                var data = kpiFactoryServices.getKPIFactoryService().getInfoGeneral(id, cc, Fechainicio, FechaFin, _lstCatMaquinas, _lstCapOrdenTrabajo, _lstCapHorometro, _lstDetOrdenTrabajo, _lstCatCriteriosCausaParo);

                result.Add("data", data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getMTTOyParo(int id, List<string> cc, DateTime Fechainicio, DateTime FechaFin, List<tblM_CatMaquina> _lstCatMaquinas, List<tblM_CapOrdenTrabajo> _lstCapOrdenTrabajo,
                                                    List<tblM_DetOrdenTrabajo> _lstDetOrdenTrabajo, List<tblM_CatCriteriosCausaParo> _lstCatCriteriosCausaParo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                FechaFin = FechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);
                var data = kpiFactoryServices.getKPIFactoryService().getMTTOyParo(id, cc, Fechainicio, FechaFin, _lstCatMaquinas, _lstCapOrdenTrabajo, _lstDetOrdenTrabajo, _lstCatCriteriosCausaParo);

                result.Add("dataMain", data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult cboMotivosParo()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var idUsuario = getUsuario().id;
                var listaCC = usuarioFactoryServices.getUsuarioService().getCCsUsuario(idUsuario).Select(x => x.cc).ToList();
                var res = motivosParoFactoryServices.getMotivosParoFactoryServices().cboMotivosParo().Select(x => new { Value = x.id, Text = x.CausaParo }).OrderBy(x => x.Text);

                result.Add(ITEMS, res);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult dataMotivosPAro(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var res = motivosParoFactoryServices.getMotivosParoFactoryServices().getMotivosParo(id);

                result.Add("data", res);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult cboModelo(int idGrupo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                // var resultData = maquinaFactoryServices.getMaquinaServices().FillCboModeloEquipo(idMarca).Select(x => new { Value = x.id, Text = x.descripcion }).ToList();

                result.Add(ITEMS, maquinaFactoryServices.getMaquinaServices().FillCboModeloEquipoGrupo(idGrupo).Select(x => new { Value = x.id, Text = x.descripcion }));
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult loadTablaHorasHombre(FiltrosRtpHorasHombre obj)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var resData = kpiFactoryServices.getKPIFactoryService().ConsultaFiltrosOT(obj);
                var datad = resData.GroupBy(x => x.puestoID);

                List<string> datos = new List<string>();
                int i = 1;

                List<horashombreDTO> listaObj = new List<horashombreDTO>();
                foreach (var item in datad)
                {
                    horashombreDTO objSigle = new horashombreDTO();

                    var personalCount = item.Count();
                    objSigle.btn = "";

                    objSigle.Puesto = item.FirstOrDefault().descripcionPuesto;
                    objSigle.PuestoID = item.FirstOrDefault().puestoID;
                    objSigle.TotalHorasHombre = Math.Round(item.Sum(x => x.totalHorasHombre), 2);
                    objSigle.CostoHH = Math.Round(item.Sum(x => x.costoHorasHombre) / personalCount, 2);  //item.Sum(x => x.);
                    objSigle.CostoTotal = Math.Round(objSigle.TotalHorasHombre * objSigle.CostoHH, 2);
                    listaObj.Add(objSigle);
                }


                Session["reshorashombreDTO"] = listaObj.OrderByDescending(x => x.TotalHorasHombre).Select(y => new
                {
                    CostoHH = y.CostoHH,
                    CostoTotal = y.CostoTotal,
                    Puesto = y.Puesto,
                    PuestoID = y.PuestoID,
                    TotalHorasHombre = y.TotalHorasHombre,

                });

                result.Add("dataset", listaObj.OrderByDescending(x => x.TotalHorasHombre).Select(y => new
                {
                    No = i++,
                    y.btn,
                    y.CostoHH,
                    y.CostoTotal,
                    y.Puesto,
                    y.PuestoID,
                    y.TotalHorasHombre,

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

        public ActionResult GetOTEmpleado(int EmpleadoID, DateTime FechaInicio, DateTime FechaFin)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var resData = kpiFactoryServices.getKPIFactoryService().GetOTEmpleado(EmpleadoID, FechaInicio, FechaFin);


                Session["GetOTEmpleado"] = resData.ToList();
                result.Add("dataResult", resData);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult loadTablaHorasHombreDetalle(tblHorasHombreDetalleDTO obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var resData = kpiFactoryServices.getKPIFactoryService().ConsultaFiltrosOTDET(obj);


                Session["rpttblHorasHombreDetDTO"] = resData.Select(x => new tblHorasHombreDetDTO
                      {
                          cantidadOT = x.cantidadOT,
                          hrasCorrectivo = Math.Round(x.hrasCorrectivo, 2),
                          hrasPredictivo = Math.Round(x.hrasPredictivo, 2),
                          hrasPreventivo = Math.Round(x.hrasPreventivo, 2),
                          promedioHrasOT = Math.Round(x.promedioHrasOT, 2),
                          personalID = x.personalID,
                          personalNombre = x.personalNombre,
                          puesto = x.puesto
                      }).ToList();

                result.Add("dataset", resData.Select(x => new
                {
                    x.cantidadOT,
                    hrasCorrectivo = Math.Round(x.hrasCorrectivo, 2),
                    hrasPredictivo = Math.Round(x.hrasPredictivo, 2),
                    hrasPreventivo = Math.Round(x.hrasPreventivo, 2),
                    promedioHrasOT = Math.Round(x.promedioHrasOT, 2),
                    x.personalID,
                    x.personalNombre,
                    btnDetalle = ""

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

        public ActionResult GetInfoMotivoParo(int TipoParoID, DateTime FechaInicio, DateTime FechaFin)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var resData = kpiFactoryServices.getKPIFactoryService().DetalleTiposParo(TipoParoID, FechaInicio, FechaFin);

                Session["rptdetFrecuenciaParoDTO"] = resData.Select(x => new detFrecuenciaParoDTO
                 {
                     cantidadPersonas = x.cantidadPersonas,
                     comentariosSolucion = x.comentariosSolucion,
                     economico = x.economico,
                     economicoID = x.economicoID,
                     folio = x.folio,
                     horashombre = Math.Round(x.horashombre, 2),
                     tiempoMuerto = Math.Round(x.tiempoMuerto, 2),
                     tiempoUtil = Math.Round(x.tiempoUtil, 2)

                 }).ToList();

                result.Add("dataset", resData.Select(x => new
                {
                    x.cantidadPersonas,
                    x.comentariosSolucion,
                    x.economico,
                    x.economicoID,
                    x.folio,
                    horashombre = Math.Round(x.horashombre, 2),
                    tiempoMuerto = Math.Round(x.tiempoMuerto, 2),
                    tiempoUtil = Math.Round(x.tiempoUtil, 2)

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

        public ActionResult loadDataFrecuenciasParo(FiltrosRtpHorasHombre obj, decimal horaIncio, decimal horaFinal)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var list = centroCostosFactoryServices.getCentroCostosService().getListaCC();

                var rawOT = kpiFactoryServices.getKPIFactoryService().getDetOrdenTrabajo(obj, horaIncio, horaFinal);
                var rawFrecuenciaParo = kpiFactoryServices.getKPIFactoryService().getCatCriteriosCausa();
                var rawOTParos = rawOT.Select(x => x.OrdenTrabajo).ToList();

                int count = 1;
                List<frecuenciaParoDTO> objDat = new List<frecuenciaParoDTO>();

                List<StatusGeneralMnttoDTO> StatusGeneralMnttoObj = new List<StatusGeneralMnttoDTO>();

                foreach (var item in rawFrecuenciaParo)
                {
                    frecuenciaParoDTO objData = new frecuenciaParoDTO();
                    var motivoParoObj = rawOTParos.Where(x => x.MotivoParo == item.id).ToList();
                    var Calculo = rawOT.Where(x => x.OrdenTrabajo.MotivoParo == item.id).ToList();
                    objData.motivoParoID = item.id;
                    objData.detalleFrecuencia = item.CausaParo;
                    objData.frecuenciaParo = motivoParoObj.Count();
                    objData.tiempoOT = Calculo.Select(x => x.OrdenTrabajo).Distinct().ToList().Sum(y => GetTotalHoras(y.FechaEntrada, y.FechaSalida));
                    objData.horasHombre = Calculo.Sum(y => GetTotalHoras(y.HoraInicio, y.HoraFin));
                    objData.motivoParo = item.CausaParo;
                    objDat.Add(objData);
                }

                List<StatusGeneralMnttoDTO> objListStatusGeneral = new List<StatusGeneralMnttoDTO>();
                int countPreventivo = 1;
                int countCorrectivo = 1;
                int countPredictivo = 1;
                int totalGeneral = 1;
                foreach (var item in rawOT)
                {
                    var itemOT = item.OrdenTrabajo;

                    StatusGeneralMnttoDTO objStatusGeneral = new StatusGeneralMnttoDTO();

                    var objTemp = rawFrecuenciaParo.FirstOrDefault(x => x.id == item.OrdenTrabajo.MotivoParo);

                    if (objTemp.TiempoMantenimiento == "Preventivo")
                    {
                        objStatusGeneral.CC = itemOT.CC;
                        objStatusGeneral.cantidadFrecuencias = countPreventivo;
                        objStatusGeneral.CCNombre = "";
                        objStatusGeneral.porcentajeRelativo = (countPreventivo / totalGeneral) * 100;
                        objStatusGeneral.tipoParo = 1;

                        countPreventivo++;

                    }

                    if (objTemp.TiempoMantenimiento == "Correctivo")
                    {
                        objStatusGeneral.CC = itemOT.CC;
                        objStatusGeneral.cantidadFrecuencias = countCorrectivo;
                        objStatusGeneral.CCNombre = "";
                        objStatusGeneral.porcentajeRelativo = (countCorrectivo / totalGeneral) * 100;
                        objStatusGeneral.tipoParo = 2;
                        countCorrectivo++;

                    }

                    if (objTemp.TiempoMantenimiento == "Predictivo")
                    {
                        objStatusGeneral.CC = itemOT.CC;
                        objStatusGeneral.cantidadFrecuencias = countPredictivo;
                        objStatusGeneral.CCNombre = "";
                        objStatusGeneral.porcentajeRelativo = (countPredictivo / totalGeneral) * 100;
                        objStatusGeneral.tipoParo = 3;
                        countPredictivo++;
                    }
                    objStatusGeneral.tiempo = GetTotalHoras(itemOT.FechaEntrada, itemOT.FechaSalida);
                    objStatusGeneral.Mes = itemOT.FechaEntrada.Month;

                    objListStatusGeneral.Add(objStatusGeneral);

                    totalGeneral++;
                }

                List<StatusGeneralMnttoDTO> resultGeneralMtto = new List<StatusGeneralMnttoDTO>();

                decimal totalMantenimientos = objListStatusGeneral.Count();
                var general = objListStatusGeneral.GroupBy(x => x.tipoParo).Select(y => new
                {
                    Descripciones = y.Key,
                    Frecuencias = y.Count(),
                    Tiempo = y.Where(g => g.tipoParo == y.Key).Sum(t => t.tiempo)
                }).ToList();

                var sumaTiempoTotal = objListStatusGeneral.Sum(x => x.tiempo);

                foreach (var item in general)
                {
                    StatusGeneralMnttoDTO objGeneral = new StatusGeneralMnttoDTO();
                    objGeneral.cantidadFrecuencias = item.Frecuencias;
                    objGeneral.porcentajeRelativo = Math.Round((item.Frecuencias / totalMantenimientos) * 100, 2);
                    objGeneral.porcentajeTiempo = Math.Round((item.Tiempo / sumaTiempoTotal) * 100, 2);
                    objGeneral.tiempo = Math.Round(item.Tiempo, 2);

                    switch (item.Descripciones)
                    {
                        case 1:
                            objGeneral.descripcion = "Preventivo";
                            objGeneral.tipoParo = 1;
                            break;
                        case 2:
                            objGeneral.descripcion = "Correctivo";
                            objGeneral.tipoParo = 2;
                            break;
                        case 3:
                            objGeneral.descripcion = "Predictivo";
                            objGeneral.tipoParo = 3;
                            break;
                        default:
                            break;
                    }
                    resultGeneralMtto.Add(objGeneral);

                }

                Session["generalManttoSession"] = resultGeneralMtto.OrderBy(x => x.cantidadFrecuencias);


                result.Add("generalMantto", resultGeneralMtto.OrderBy(x => x.tipoParo));

                var resultPreventivo = objListStatusGeneral.GroupBy(x => x.CC).Select(y => new
                {
                    cc = list.FirstOrDefault(w=>w.Value.Equals(y.Key)).Text,
                    cantidadFrecuencias = y.Where(c => c.tipoParo == 1).Count(),
                    tiempo = y.Sum(x => x.tiempo)

                });

                var resultCorrectivo = objListStatusGeneral.GroupBy(x => x.CC).Select(y => new
                {
                    cc = list.FirstOrDefault(w => w.Value.Equals(y.Key)).Text,
                    cantidadFrecuencias = y.Where(c => c.tipoParo == 2).Count()
                    ,
                    tiempo = y.Sum(x => x.tiempo)
                });

                var resultPredictivo = objListStatusGeneral.GroupBy(x => x.CC).Select(y => new
                {
                    cc = list.FirstOrDefault(w => w.Value.Equals(y.Key)).Text,
                    cantidadFrecuencias = y.Where(c => c.tipoParo == 3).Count(),
                    tiempo = y.Sum(x => x.tiempo)
                });


                var GroupCCFrecuenciaParos = rawOT.GroupBy(X => new { X.OrdenTrabajo.CC }).Select(
                        y => new
                        {
                            CentroCostos = list.FirstOrDefault(w => w.Value.Equals(y.Key.CC)).Text,
                            Frecuencia = rawOT.Where(z => z.OrdenTrabajo.CC == y.Key.CC).Count()
                        }
                    );

                var GroupCCTiempos = rawOT.GroupBy(X => new { X.OrdenTrabajo.CC }).Select(
                        y => new
                        {
                            CentroCostos = list.FirstOrDefault(w => w.Value.Equals(y.Key.CC)).Text,
                            Tiempos = Math.Round(rawOT.Where(z => z.OrdenTrabajo.CC == y.Key.CC).Sum(z => GetTotalHoras(z.OrdenTrabajo.FechaEntrada, z.OrdenTrabajo.FechaSalida)), 2)
                        }
                    );

                var GroupCCFrecuenciaParosTendencia = rawOT.GroupBy(X => new { X.OrdenTrabajo.CC, X.OrdenTrabajo.FechaEntrada.Month }).Select(
                        y => new
                        {
                            CentroCostos = list.FirstOrDefault(w => w.Value.Equals(y.Key.CC)).Text,
                            FrecuenciaTendecia = rawOT.Where(z => z.OrdenTrabajo.CC == y.Key.CC && y.Key.Month == z.OrdenTrabajo.FechaEntrada.Month).Count(),
                            Mes = new DateTime(DateTime.Now.Year, y.Key.Month, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("es")),
                            y.Key.Month

                        }
                    ).OrderBy(h => h.CentroCostos).ThenBy(x => x.Month);

                var GroupCCTiemposTendencia = rawOT.GroupBy(X => new { X.OrdenTrabajo.CC, X.OrdenTrabajo.FechaEntrada.Month }).Select(
                        y => new
                        {
                            CentroCostos = list.FirstOrDefault(w => w.Value.Equals(y.Key.CC)).Text,
                            TiemposTendecia = Math.Round(rawOT.Where(z => z.OrdenTrabajo.CC == y.Key.CC && y.Key.Month == z.OrdenTrabajo.FechaEntrada.Month).Sum(z => GetTotalHoras(z.OrdenTrabajo.FechaEntrada, z.OrdenTrabajo.FechaSalida)), 2),
                            Mes = new DateTime(DateTime.Now.Year, y.Key.Month, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("es")),
                            MesID = y.Key.Month,
                            y.Key.Month
                        }
                    ).OrderBy(h => h.CentroCostos).ThenBy(x => x.Month);

                result.Add("GroupCCFrecuenciaParos", GroupCCFrecuenciaParos);
                result.Add("GroupCCTiempos", GroupCCTiempos);
                result.Add("GroupCCFrecuenciaParosTendencia", GroupCCFrecuenciaParosTendencia);
                result.Add("GroupCCTiemposTendencia", GroupCCTiemposTendencia);


                result.Add("dtPreventivo", resultPreventivo);
                result.Add("dtCorrectivo", resultCorrectivo);
                result.Add("dtPredictivo", resultPredictivo);

                var dtPreventivoFecha = objListStatusGeneral.GroupBy(x => x.Mes).Select(y => new
                {
                    Fecha = y.Key,
                    cantidadFrecuencias = y.Where(c => c.tipoParo == 1).Count(),
                    TipoMntto = "Preventivo"
                }).OrderBy(x => x.Fecha);
                var dtCorrectivoFecha = objListStatusGeneral.GroupBy(x => x.Mes).Select(y => new
                {
                    Fecha = y.Key,
                    cantidadFrecuencias = y.Where(c => c.tipoParo == 2).Count(),
                    TipoMntto = "Correctivo"
                }).OrderBy(x => x.Fecha);
                var dtPredictivoFecha = objListStatusGeneral.GroupBy(x => x.Mes).Select(y => new
                {
                    Fecha = y.Key,
                    cantidadFrecuencias = y.Where(c => c.tipoParo == 3).Count(),
                    TipoMntto = "Predictivo"
                }).OrderBy(x => x.Fecha);

                result.Add("dtPreventivoFecha", dtPreventivoFecha);
                result.Add("dtCorrectivoFecha", dtCorrectivoFecha);
                result.Add("dtPredictivoFecha", dtPredictivoFecha);


                var dtPreventivoFechaTendencia = objListStatusGeneral.GroupBy(x => x.Mes).Select(y => new
                {
                    Fecha = y.Key,
                    tiempo = y.Where(c => c.tipoParo == 1).Sum(s => s.tiempo),
                    TipoMntto = "Preventivo"
                }).OrderBy(x => x.Fecha);

                var dtCorrectivoFechaTendencia = objListStatusGeneral.GroupBy(x => x.Mes).Select(y => new
                {
                    Fecha = y.Key,
                    tiempo = y.Where(c => c.tipoParo == 2).Sum(s => s.tiempo),
                    TipoMntto = "Correctivo"
                }).OrderBy(x => x.Fecha);

                var dtPredictivoFechaTendencia = objListStatusGeneral.GroupBy(x => x.Mes).Select(y => new
                {
                    Fecha = y.Key,
                    tiempo = y.Where(c => c.tipoParo == 3).Sum(s => s.tiempo),
                    TipoMntto = "Predictivo"
                }).OrderBy(x => x.Fecha);

                result.Add("dtPreventivoFechaTendencia", dtPreventivoFechaTendencia);
                result.Add("dtCorrectivoFechaTendencia", dtCorrectivoFechaTendencia);
                result.Add("dtPredictivoFechaTendencia", dtPredictivoFechaTendencia);



                result.Add("dataset", objDat.OrderByDescending(x => x.frecuenciaParo).Select(x => new
                {
                    no = count++,
                    x.motivoParo,
                    x.frecuenciaParo,
                    tiempoOT = Math.Round(x.tiempoOT, 2),
                    horasHombre = Math.Round(x.horasHombre, 2),
                    detalleFrecuencia = "",
                    x.motivoParoID,

                }).ToList());

                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private decimal GetTotalHoras(DateTime dateTime1, DateTime? dateTime2)
        {
            var FechaEntrada = dateTime1;
            var FechaSalida = dateTime2;

            if (dateTime2 != null)
            {
                var horas = (decimal)((DateTime)FechaSalida - FechaEntrada).TotalHours;
                return horas;
            }
            else
            {
                return 0;
            }
        }

        public ActionResult GetIMG(string obj)
        {
            var result = new Dictionary<string, object>();
            try
            {

                Session["base64FileGrafica"] = obj;

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetListGraficas(List<string> obj)
        {
            var result = new Dictionary<string, object>();
            try
            {

                Session["base64FileGraficaList"] = obj;

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult loadRptHorasHombre(List<int> listaPuestos, List<int> listaPersonal)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var resData = kpiFactoryServices.getKPIFactoryService().ConsultaRptPersonal(listaPuestos, listaPersonal);

                var Filtro = resData.Select(x => new tblHorasHombreDetDTO
                {
                    cantidadOT = x.cantidadOT,
                    hrasCorrectivo = Math.Round(x.hrasCorrectivo, 2),
                    hrasPredictivo = Math.Round(x.hrasPredictivo, 2),
                    hrasPreventivo = Math.Round(x.hrasPreventivo, 2),
                    promedioHrasOT = Math.Round(x.promedioHrasOT, 2),
                    personalID = x.personalID,
                    personalNombre = x.personalNombre,
                    puesto = x.puesto

                }).ToList();

                Session["rpttblHorasHombreDetDTO"] = Filtro.ToList();
                result.Add("dataset", Filtro);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public ActionResult FillCboPuesto()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var LstPuestos = (List<PuestosDTO>)Session["lstPuestosEnkontrol"];


                if (LstPuestos.Count > 0)
                {
                    var res = LstPuestos.Select(x => new { Value = x.puesto, Text = x.descripcion }).GroupBy(y => y).OrderBy(x => x.Key.Text).ToList();


                    result.Add(ITEMS, res.Select(x => new { Value = x.Key.Value, Text = x.Key.Text }));
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

        public ActionResult setCCrpt(List<string> ccs)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> LstPuestos = new List<ComboDTO>();

                foreach (var item in ccs)
                {
                    ComboDTO ComboDTOobj = new ComboDTO();

                    ComboDTOobj.Value = item;
                    ComboDTOobj.Text = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(item);

                    LstPuestos.Add(ComboDTOobj);
                }

                result.Add(ITEMS, LstPuestos);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult setCCConcentrado(List<string> ccs)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> LstPuestos = new List<ComboDTO>();

                foreach (var item in ccs)
                {
                    ComboDTO ComboDTOobj = new ComboDTO();

                    ComboDTOobj.Value = item;
                    ComboDTOobj.Text = centroCostosFactoryServices.getCentroCostosService().getNombreCCArrendadoraRH(item);

                    LstPuestos.Add(ComboDTOobj);
                }

                result.Add(ITEMS, LstPuestos);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboPuestosRpt()
        {
            var result = new Dictionary<string, object>();
            try
            {

                var LstPuestos = capHorasHombreFactoryServices.getCapHorasHombreFactoryServices().getListaPuestos();
                result.Add(ITEMS, LstPuestos);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillCboPersonalPuestosRpt(List<int> Puestos, List<string> ccs)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var LstPuestos = capHorasHombreFactoryServices.getCapHorasHombreFactoryServices().getListaPersonalPuestos(Puestos, ccs);
                result.Add(ITEMS, LstPuestos);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DetalleGeneralPorPuesto(List<string> ccs, DateTime fechaInicio, DateTime fechaFin, List<int> listaCategorias, List<int> listaSubCategoria)
        {
            var result = new Dictionary<string, object>();
            try
            {


                var titulos = capHorasHombreFactoryServices.getCapHorasHombreFactoryServices().getCategorias().Select(x => x.descripcion).ToList();
                result.Add("titulos", titulos);
                Session["rptDetalleGeneralPorPuesto"] = null;
                int tipoRpt = 1;

                if (listaCategorias != null)
                {
                    tipoRpt = 2;
                }
                if (listaSubCategoria != null)
                {
                    tipoRpt = 3;
                }


                switch (tipoRpt)
                {
                    case 1:
                        {
                            var LstPuestos = capHorasHombreFactoryServices.getCapHorasHombreFactoryServices().getListaPuestos().Select(x => Convert.ToInt32(x.Value)).ToList();
                            var dtDistribucion = capHorasHombreFactoryServices.getCapHorasHombreFactoryServices().rptGeneralCCPorPuesto(ccs, fechaInicio, fechaFin, LstPuestos);
                            Session["rptDetalleGeneralPorPuesto"] = dtDistribucion;
                            result.Add("dtSet", dtDistribucion);
                            result.Add("TipoRpt", tipoRpt);

                            List<decimal> dtSetGrafica = new List<decimal>();
                            decimal totaltrabajoMaquinariaOT = dtDistribucion.Sum(x => x.trabajoMaquinariaOT);
                            decimal totaltrabajosInstalaciones = dtDistribucion.Sum(x => x.trabajosInstalaciones);
                            decimal totallimpieza = dtDistribucion.Sum(x => x.limpieza);
                            decimal totalconsultaInformacion = dtDistribucion.Sum(x => x.consultaInformacion);
                            decimal totaltiempoDescanso = dtDistribucion.Sum(x => x.tiempoDescanso);
                            decimal totalcursosCapacitaciones = dtDistribucion.Sum(x => x.cursosCapacitaciones);
                            decimal totalMonitoreoDiario = dtDistribucion.Sum(x => x.monitoreoDiario);


                            decimal TotalTotal = totaltrabajoMaquinariaOT + totaltrabajosInstalaciones + totallimpieza + totalconsultaInformacion + totaltiempoDescanso + totalcursosCapacitaciones + totalMonitoreoDiario;

                            decimal dato1 = Math.Round(totaltrabajoMaquinariaOT / TotalTotal * 100, 2);
                            decimal dato2 = Math.Round(totaltrabajosInstalaciones / TotalTotal * 100, 2);
                            decimal dato3 = Math.Round(totallimpieza / TotalTotal * 100, 2);

                            decimal dato4 = Math.Round(totalconsultaInformacion / TotalTotal * 100, 2);
                            decimal dato5 = Math.Round(totaltiempoDescanso / TotalTotal * 100, 2);
                            decimal dato6 = Math.Round(totalcursosCapacitaciones / TotalTotal * 100, 2);
                            decimal dato7 = Math.Round(totalMonitoreoDiario / TotalTotal * 100, 2);

                            dtSetGrafica.Add(dato1);
                            dtSetGrafica.Add(dato2);
                            dtSetGrafica.Add(dato3);
                            dtSetGrafica.Add(dato4);
                            dtSetGrafica.Add(dato5);
                            dtSetGrafica.Add(dato6);
                            dtSetGrafica.Add(dato7);


                            result.Add("trabajosInstalaciones", dato1);
                            result.Add("trabajoMaquinariaOT", dato2);
                            result.Add("limpieza", dato3);
                            result.Add("consultaInformacion", dato4);
                            result.Add("tiempoDescanso", dato5);
                            result.Add("cursosCapacitaciones", dato6);
                            result.Add("monitoreoDiario", dato7);

                            result.Add("dataSetGrafica", dtSetGrafica);


                            break;
                        }
                    case 2:
                    case 3:
                        {
                            var dtDistribucion = capHorasHombreFactoryServices.getCapHorasHombreFactoryServices().rptGeneralDistribucion(ccs, fechaInicio, fechaFin, listaCategorias, listaSubCategoria);
                            List<DistribucionHHPersonalDTO> objList = new List<DistribucionHHPersonalDTO>();
                            if (tipoRpt == 2)
                            {

                                var ListaPuestos = dtDistribucion.GroupBy(p => new { p.puestoID, p.puesto }).Select(x => x.Key).ToList();

                                foreach (var puesto in ListaPuestos)
                                {
                                    DistribucionHHPersonalDTO obj = new DistribucionHHPersonalDTO();

                                    obj.puesto = puesto.puesto;
                                    obj.puestoID = puesto.puestoID;

                                    var resultSuma = dtDistribucion.Where(x => x.puestoID == puesto.puestoID);
                                    obj.btn = "";
                                    obj.consultaInformacion = resultSuma.Sum(r => r.consultaInformacion);
                                    obj.cursosCapacitaciones = resultSuma.Sum(r => r.cursosCapacitaciones);
                                    obj.limpieza = resultSuma.Sum(r => r.limpieza);
                                    obj.tiempoDescanso = resultSuma.Sum(r => r.tiempoDescanso);
                                    obj.totalHorashombre = resultSuma.Sum(r => r.totalHorashombre);
                                    obj.trabajoMaquinariaOT = resultSuma.Sum(r => r.trabajoMaquinariaOT);
                                    obj.trabajosInstalaciones = resultSuma.Sum(r => r.trabajosInstalaciones);
                                    obj.monitoreoDiario = resultSuma.Sum(r => r.monitoreoDiario);

                                    objList.Add(obj);

                                }

                                Session["rptDetalleGeneralPorPuesto"] = objList;
                                result.Add("dtSet", objList);

                                List<decimal> dtSetGrafica = new List<decimal>();
                                decimal totaltrabajoMaquinariaOT = dtDistribucion.Sum(x => x.trabajoMaquinariaOT);
                                decimal totaltrabajosInstalaciones = dtDistribucion.Sum(x => x.trabajosInstalaciones);
                                decimal totallimpieza = dtDistribucion.Sum(x => x.limpieza);
                                decimal totalconsultaInformacion = dtDistribucion.Sum(x => x.consultaInformacion);
                                decimal totaltiempoDescanso = dtDistribucion.Sum(x => x.tiempoDescanso);
                                decimal totalcursosCapacitaciones = dtDistribucion.Sum(x => x.cursosCapacitaciones);
                                decimal totalMonitoreoDiario = dtDistribucion.Sum(x => x.monitoreoDiario);


                                decimal TotalTotal = totaltrabajoMaquinariaOT + totaltrabajosInstalaciones + totallimpieza + totalconsultaInformacion + totaltiempoDescanso + totalcursosCapacitaciones + totalMonitoreoDiario;
                                decimal dato1 = Math.Round(totaltrabajoMaquinariaOT / TotalTotal * 100, 2);
                                decimal dato2 = Math.Round(totaltrabajosInstalaciones / TotalTotal * 100, 2);
                                decimal dato3 = Math.Round(totallimpieza / TotalTotal * 100, 2);

                                decimal dato4 = Math.Round(totalconsultaInformacion / TotalTotal * 100, 2);
                                decimal dato5 = Math.Round(totaltiempoDescanso / TotalTotal * 100, 2);
                                decimal dato6 = Math.Round(totalcursosCapacitaciones / TotalTotal * 100, 2);
                                decimal dato7 = Math.Round(totalMonitoreoDiario / TotalTotal * 100, 2);

                                dtSetGrafica.Add(dato1);
                                dtSetGrafica.Add(dato2);
                                dtSetGrafica.Add(dato3);
                                dtSetGrafica.Add(dato4);
                                dtSetGrafica.Add(dato5);
                                dtSetGrafica.Add(dato6);
                                dtSetGrafica.Add(dato7);

                                result.Add("trabajosInstalaciones", dato1);
                                result.Add("trabajoMaquinariaOT", dato2);

                                result.Add("limpieza", dato3);
                                result.Add("consultaInformacion", dato4);
                                result.Add("tiempoDescanso", dato5);
                                result.Add("cursosCapacitaciones", dato6);
                                result.Add("monitoreoDiario", dato7);
                                result.Add("dataSetGrafica", dtSetGrafica);

                            }
                            else
                            {
                                Session["rptDetalleGeneralPorPuesto"] = dtDistribucion;

                                result.Add("dtSet", dtDistribucion);

                                List<decimal> dtSetGrafica = new List<decimal>();
                                decimal totaltrabajoMaquinariaOT = dtDistribucion.Sum(x => x.trabajoMaquinariaOT);
                                decimal totaltrabajosInstalaciones = dtDistribucion.Sum(x => x.trabajosInstalaciones);
                                decimal totallimpieza = dtDistribucion.Sum(x => x.limpieza);
                                decimal totalconsultaInformacion = dtDistribucion.Sum(x => x.consultaInformacion);
                                decimal totaltiempoDescanso = dtDistribucion.Sum(x => x.tiempoDescanso);
                                decimal totalcursosCapacitaciones = dtDistribucion.Sum(x => x.cursosCapacitaciones);
                                decimal totalMonitoreoDiario = dtDistribucion.Sum(x => x.monitoreoDiario);


                                decimal TotalTotal = totaltrabajoMaquinariaOT + totaltrabajosInstalaciones + totallimpieza + totalconsultaInformacion + totaltiempoDescanso + totalcursosCapacitaciones + totalMonitoreoDiario;
                                decimal dato1 = Math.Round(totaltrabajoMaquinariaOT / TotalTotal * 100, 2);
                                decimal dato2 = Math.Round(totaltrabajosInstalaciones / TotalTotal * 100, 2);
                                decimal dato3 = Math.Round(totallimpieza / TotalTotal * 100, 2);

                                decimal dato4 = Math.Round(totalconsultaInformacion / TotalTotal * 100, 2);
                                decimal dato5 = Math.Round(totaltiempoDescanso / TotalTotal * 100, 2);
                                decimal dato6 = Math.Round(totalcursosCapacitaciones / TotalTotal * 100, 2);
                                decimal dato7 = Math.Round(totalMonitoreoDiario / TotalTotal * 100, 2);

                                dtSetGrafica.Add(dato1);
                                dtSetGrafica.Add(dato2);
                                dtSetGrafica.Add(dato3);
                                dtSetGrafica.Add(dato4);
                                dtSetGrafica.Add(dato5);
                                dtSetGrafica.Add(dato6);
                                dtSetGrafica.Add(dato7);

                                result.Add("trabajosInstalaciones", dato1);
                                result.Add("trabajoMaquinariaOT", dato2);

                                result.Add("limpieza", dato3);
                                result.Add("consultaInformacion", dato4);
                                result.Add("tiempoDescanso", dato5);
                                result.Add("cursosCapacitaciones", dato6);
                                result.Add("monitoreoDiario", dato7);
                                result.Add("dataSetGrafica", dtSetGrafica);
                            }


                            result.Add(SUCCESS, true);
                            result.Add("TipoRpt", tipoRpt);
                            break;
                        }
                    default:
                        break;
                }

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult FillCboPersonal(List<int> Puestos)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var LstPuestos = (List<PuestosDTO>)Session["lstPuestosEnkontrol"];


                if (LstPuestos.Count > 0)
                {
                    var res = LstPuestos.Where(p => Puestos.Contains(p.puesto)).Select(x => new { Value = x.personalID, Text = x.nombre + " " + x.ape_paterno + " " + x.ape_materno }).OrderBy(x => x.Text).ToList();


                    result.Add(ITEMS, res);
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

        public ActionResult loadrptConcentradoHorasHombre(List<string> ccs, DateTime fechaInicio, DateTime fechaFin, List<int> listaCategorias, List<int> listaSubCategoria)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<GeneralConcentradoHHDTO> GeneralConcentradoHHDTOList = new List<GeneralConcentradoHHDTO>();
                List<DistribucionHHDTO> DistribucionHHDTOList = new List<DistribucionHHDTO>();
                var ConcentradoGeneral = capHorasHombreFactoryServices.getCapHorasHombreFactoryServices().getConcentradoGeneral(ccs, fechaInicio, fechaFin, listaCategorias, listaSubCategoria);
                var DistibucionGeneral = capHorasHombreFactoryServices.getCapHorasHombreFactoryServices().getDistribucionGeneral(ccs, fechaInicio, fechaFin, listaCategorias, listaSubCategoria);

                result.Add("GeneralConcentradoHHDTOList", ConcentradoGeneral);

                result.Add("DistribucionHHDTOList", DistibucionGeneral.Select(x => new
                {
                    x.consultaInformacion,
                    x.cursosCapacitaciones,
                    x.limpieza,
                    x.puesto,
                    x.puestoID,
                    x.tiempoDescanso,
                    x.totalHorashombre,
                    x.trabajoMaquinariaOT,
                    x.trabajosInstalaciones,
                    x.monitoreoDiario,
                    btn = ""

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

        public ActionResult DetalleDistribucionGeneral(List<string> ccs, DateTime fechaInicio, DateTime fechaFin, int puestoID)
        {
            var result = new Dictionary<string, object>();
            try
            {


                var ConcentradoGeneral = capHorasHombreFactoryServices.getCapHorasHombreFactoryServices().DetalleDistribucionGeneral(ccs, fechaInicio, fechaFin, puestoID);

                Session["ConcentradoGeneral"] = ConcentradoGeneral;

                result.Add("ConcentradoGeneral", ConcentradoGeneral);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DetallePersonalInfo(List<string> ccs, DateTime fechaInicio, DateTime fechaFin, int numEmpleado)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var DetallePersonaDT = capHorasHombreFactoryServices.getCapHorasHombreFactoryServices().getInfoByEmpleado(ccs, fechaInicio, fechaFin, numEmpleado);
                var infoPersonal = capHorasHombreFactoryServices.getCapHorasHombreFactoryServices().getinfoEmpleadoGeneral(numEmpleado);


                Session["DetallePersonaDTSession"] = DetallePersonaDT.ToList();
                result.Add("nombre", infoPersonal.nombre + " " + infoPersonal.ape_paterno);
                result.Add("puesto", infoPersonal.descripcion);
                result.Add("DetallePersonaDT", DetallePersonaDT);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult rptUtilizacionDetallePuesto(List<string> ccs, DateTime fechaInicio, DateTime fechaFin, int puestoID)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var DetallePersonaDT = capHorasHombreFactoryServices.getCapHorasHombreFactoryServices().getDetalleConcentradoGeneral(ccs, fechaInicio, fechaFin, puestoID);

                Session["DetallePersonaDT"] = DetallePersonaDT;
                result.Add("dt", DetallePersonaDT);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult loadParetoCategorias(List<string> ccs, DateTime fechaInicio, DateTime fechaFin, List<int> listaCategorias, List<int> listaSubCategoria)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = capHorasHombreFactoryServices.getCapHorasHombreFactoryServices().getParetoCategorias(ccs, fechaInicio, fechaFin, listaCategorias, listaSubCategoria);


                result.Add("Categoria", data.listaCategorias.OrderByDescending(x => x.valuePareto));
                result.Add("SubCategoria", data.listaSubCategorias.OrderByDescending(x => x.valuePareto));
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public FileResult ExportData()
        {
            var cc = Request.QueryString["cc"];
            var fechaInicio = DateTime.Parse(Request.QueryString["fechaInicio"]);
            var fechaFin = DateTime.Parse(Request.QueryString["fechaFin"]);
            fechaFin = fechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);
            var economico = int.Parse(Request.QueryString["economico"]);
            var motivo = int.Parse(Request.QueryString["motivo"]);
            using (ExcelPackage package = new ExcelPackage())
            {
                var mOTs = package.Workbook.Worksheets.Add("OTs");
                ExcelRange cols = mOTs.Cells["A1:Y1"];
                cols.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cols.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                var listaCC = usuarioFactoryServices.getUsuarioService().getCCsUsuario(getUsuario().id).Select(x => x.cc).ToList();
                var objOT = capturaOTFactoryServices.getCapturaOTFactoryServices().getListaOTDet(cc, listaCC, fechaInicio, fechaFin).ToList();

                var res = new List<dynamic>();
                var dataSet = objOT.Where(x => x.OrdenTrabajo.EstatusOT == false && (economico != 0 ? x.OrdenTrabajo.EconomicoID == economico : x.id == x.id) && (motivo != 0 ? x.OrdenTrabajo.MotivoParo == motivo : x.id == x.id)).Where(r => r.OrdenTrabajo.CC == cc).ToList();
                foreach (var item in dataSet)
                {
                    var Empleado = capturaOTDetFactoryServices.getCapturaOTDetFactoryServices().getCatEmpleados(item.PersonalID.ToString());
                    var maquina = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(item.OrdenTrabajo.EconomicoID).FirstOrDefault();
                    var motivoParoDet = motivosParoFactoryServices.getMotivosParoFactoryServices().getMotivosParo(item.OrdenTrabajo.MotivoParo);
                    var o = new OTDTO
                    {
                        FOLIO = item.OrdenTrabajo.id.ToString().PadLeft(6, '0'),
                        ECONOMICO = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(item.OrdenTrabajo.EconomicoID).FirstOrDefault().noEconomico,
                        NOMBRE_PERSONAL = Empleado.Nombre,
                        PUESTO = Empleado.Puesto,
                        HORA_INICIO = item.HoraInicio.ToShortDateString() + " " + item.HoraInicio.ToString("HH:mm"),
                        HORA_FIN = item.HoraFin.ToShortDateString() + " " + item.HoraFin.ToString("HH:mm"),
                        OBRA = centroCostosFactoryServices.getCentroCostosService().getNombreAreaCuent(item.OrdenTrabajo.CC),
                        MODELO = maquina.modeloEquipo.descripcion,
                        FECHA_HOROMETRO = item.OrdenTrabajo.FechaCreacion.ToShortDateString(),
                        HOROMETRO = item.OrdenTrabajo.horometro.ToString(),
                        TURNO = item.OrdenTrabajo.Turno.ToString(),
                        MOTIVO_PARO = (motivoParoDet == null ? "" : motivoParoDet.CausaParo),
                        COMENTARIO_PARO = (motivoParoDet == null ? "" : motivoParoDet.DescripcionParo),
                        TIPO_PARO_1 = (motivoParoDet == null ? "" : motivoParoDet.TipoParo),
                        TIPO_PARO_2 = (motivoParoDet == null ? "" : motivoParoDet.TiempoMantenimiento),
                        TIPO_PARO_3 = item.OrdenTrabajo.TipoParo3 == 1 ? " Maquinaria en stand by" : item.OrdenTrabajo.TipoParo3 == 2 ? "Maquinaria trabajando" : "Falta de Tramo",
                        HORA_ENTRADA = (item.OrdenTrabajo.FechaEntrada.ToShortDateString() + " " + item.OrdenTrabajo.FechaEntrada.ToString("HH:mm")),
                        HORA_SALIDA = item.OrdenTrabajo.FechaSalida == null ? "---" : (item.OrdenTrabajo.FechaSalida.Value.ToShortDateString() + " " + item.OrdenTrabajo.FechaSalida.Value.ToString("HH:mm")),
                        TIEMPO_PARO_TOTAL_HRS = "" + item.OrdenTrabajo.TiempoHorasTotal,
                        TIEMPO_PARO_TOTAL_MIN = "" + item.OrdenTrabajo.TiempoMinutosTotal,
                        TIEMPO_REPARACION_HRS = "" + item.OrdenTrabajo.TiempoHorasReparacion,
                        TIEMPO_REPARACION_MIN = "" + item.OrdenTrabajo.TiempoMinutosReparacion,
                        TIEMPO_MUERTO_HRS = "" + item.OrdenTrabajo.TiempoHorasMuerto,
                        TIEMPO_MUERTO_MIN = "" + item.OrdenTrabajo.TiempoMinutosMuerto,
                        MOTIVO_TIEMPO_MUERTO = item.OrdenTrabajo.DescripcionTiempoMuerto ?? "",
                        METODO_DE_SOLUCION = item.OrdenTrabajo.Comentario ?? ""
                    };
                    res.Add(o);
                }

                List<string> FOLIOS = new List<string>();
                List<string> ECONOMICO = new List<string>();
                List<string> PUESTO = new List<string>();
                List<string> HORA_INICIO = new List<string>();
                List<string> HORA_FIN = new List<string>();
                List<string> OBRA = new List<string>();
                List<string> MODELO = new List<string>();
                List<string> FECHA_HOROMETRO = new List<string>();
                List<string> HOROMETRO = new List<string>();
                List<string> TURNO = new List<string>();
                List<string> MOTIVO_PARO = new List<string>();
                List<string> COMENTARIO_PARO = new List<string>();
                List<string> TIPO_PARO_1 = new List<string>();
                List<string> TIPO_PARO_2 = new List<string>();
                List<string> TIPO_PARO_3 = new List<string>();
                List<string> HORA_ENTRADA = new List<string>();
                List<string> HORA_SALIDA = new List<string>();
                List<string> TIEMPO_PARO_TOTAL_HRS = new List<string>();
                List<string> TIEMPO_PARO_TOTAL_MIN = new List<string>();
                List<string> TIEMPO_REPARACION_HRS = new List<string>();
                List<string> TIEMPO_REPARACION_MIN = new List<string>();
                List<string> TIEMPO_MUERTO_HRS = new List<string>();
                List<string> TIEMPO_MUERTO_MIN = new List<string>();
                List<string> MOTIVO_TIEMPO_MUERTO = new List<string>();
                List<string> METODO_DE_SOLUCION = new List<string>();

                foreach (var item in res)
                {
                    FOLIOS.Add(item.FOLIO);
                    PUESTO.Add(item.PUESTO);
                    ECONOMICO.Add(item.ECONOMICO);
                    HORA_INICIO.Add(item.HORA_INICIO);
                    HORA_FIN.Add(item.HORA_FIN);
                    OBRA.Add(item.OBRA);
                    MODELO.Add(item.MODELO);
                    FECHA_HOROMETRO.Add(item.FECHA_HOROMETRO);
                    HOROMETRO.Add(item.HOROMETRO);
                    TURNO.Add(item.TURNO);
                    MOTIVO_PARO.Add(item.MOTIVO_PARO);
                    COMENTARIO_PARO.Add(item.COMENTARIO_PARO != null ? item.COMENTARIO_PARO : "");
                    TIPO_PARO_1.Add(item.TIPO_PARO_1);
                    TIPO_PARO_2.Add(item.TIPO_PARO_2);
                    TIPO_PARO_3.Add(item.TIPO_PARO_3);

                    HORA_ENTRADA.Add(item.HORA_ENTRADA);
                    HORA_SALIDA.Add(item.HORA_SALIDA);
                    TIEMPO_PARO_TOTAL_HRS.Add(item.TIEMPO_PARO_TOTAL_HRS);
                    TIEMPO_PARO_TOTAL_MIN.Add(item.TIEMPO_PARO_TOTAL_MIN);

                    TIEMPO_REPARACION_HRS.Add(item.TIEMPO_REPARACION_HRS);
                    TIEMPO_REPARACION_MIN.Add(item.TIEMPO_REPARACION_MIN);
                    TIEMPO_MUERTO_HRS.Add(item.TIEMPO_MUERTO_HRS);
                    TIEMPO_MUERTO_MIN.Add(item.TIEMPO_MUERTO_MIN);
                    MOTIVO_TIEMPO_MUERTO.Add(item.MOTIVO_TIEMPO_MUERTO);
                    METODO_DE_SOLUCION.Add(item.METODO_DE_SOLUCION);
                }

                mOTs.Cells["A1"].LoadFromCollection(FOLIOS);
                mOTs.Cells["B1"].LoadFromCollection(ECONOMICO);
                mOTs.Cells["C1"].LoadFromCollection(PUESTO);
                mOTs.Cells["D1"].LoadFromCollection(HORA_INICIO);
                mOTs.Cells["E1"].LoadFromCollection(HORA_FIN);
                mOTs.Cells["F1"].LoadFromCollection(OBRA);
                mOTs.Cells["G1"].LoadFromCollection(MODELO);
                mOTs.Cells["H1"].LoadFromCollection(FECHA_HOROMETRO);
                mOTs.Cells["I1"].LoadFromCollection(HOROMETRO);
                mOTs.Cells["J1"].LoadFromCollection(TURNO);
                mOTs.Cells["K1"].LoadFromCollection(MOTIVO_PARO);
                mOTs.Cells["L1"].LoadFromCollection(COMENTARIO_PARO);
                mOTs.Cells["M1"].LoadFromCollection(TIPO_PARO_1);
                mOTs.Cells["N1"].LoadFromCollection(TIPO_PARO_2);
                mOTs.Cells["O1"].LoadFromCollection(TIPO_PARO_3);
                mOTs.Cells["P1"].LoadFromCollection(HORA_ENTRADA);
                mOTs.Cells["Q1"].LoadFromCollection(HORA_SALIDA);
                mOTs.Cells["R1"].LoadFromCollection(TIEMPO_PARO_TOTAL_HRS);
                mOTs.Cells["S1"].LoadFromCollection(TIEMPO_PARO_TOTAL_MIN);
                mOTs.Cells["T1"].LoadFromCollection(TIEMPO_REPARACION_HRS);
                mOTs.Cells["U1"].LoadFromCollection(TIEMPO_REPARACION_MIN);
                mOTs.Cells["V1"].LoadFromCollection(TIEMPO_MUERTO_HRS);
                mOTs.Cells["W1"].LoadFromCollection(TIEMPO_MUERTO_MIN);
                mOTs.Cells["X1"].LoadFromCollection(MOTIVO_TIEMPO_MUERTO);
                mOTs.Cells["Y1"].LoadFromCollection(METODO_DE_SOLUCION);
                mOTs.InsertRow(1, 1);

                mOTs.Cells["A1"].Value = "FOLIO";
                mOTs.Cells["B1"].Value = "ECONOMICO";
                mOTs.Cells["C1"].Value = "PUESTO";
                mOTs.Cells["D1"].Value = "HORA_INICIO";
                mOTs.Cells["E1"].Value = "HORA_FIN";
                mOTs.Cells["F1"].Value = "OBRA";
                mOTs.Cells["G1"].Value = "MODELO";
                mOTs.Cells["H1"].Value = "FECHA_HOROMETRO";
                mOTs.Cells["I1"].Value = "HOROMETRO";
                mOTs.Cells["J1"].Value = "TURNO";
                mOTs.Cells["K1"].Value = "MOTIVO_PARO";
                mOTs.Cells["L1"].Value = "COMENTARIO_PARO";
                mOTs.Cells["M1"].Value = "TIPO_PARO_1";
                mOTs.Cells["N1"].Value = "TIPO_PARO_2";
                mOTs.Cells["O1"].Value = "TIPO_PARO_3";
                mOTs.Cells["P1"].Value = "HORA_ENTRADA";
                mOTs.Cells["Q1"].Value = "HORA_SALIDA";
                mOTs.Cells["R1"].Value = "TIEMPO_PARO_TOTAL_HRS";
                mOTs.Cells["S1"].Value = "TIEMPO_PARO_TOTAL_MIN";
                mOTs.Cells["T1"].Value = "TIEMPO_REPARACION_HRS";
                mOTs.Cells["U1"].Value = "TIEMPO_REPARACION_MIN";
                mOTs.Cells["V1"].Value = "TIEMPO_MUERTO_HRS";
                mOTs.Cells["W1"].Value = "TIEMPO_MUERTO_MIN";
                mOTs.Cells["X1"].Value = "MOTIVO_TIEMPO_MUERTO";
                mOTs.Cells["Y1"].Value = "METODO_DE_SOLUCION";


                mOTs.Cells[mOTs.Dimension.Address].AutoFitColumns();
                package.Compression = CompressionLevel.BestSpeed;

                List<byte[]> lista = new List<byte[]>();
                using (var exportData = new MemoryStream())
                {
                    package.SaveAs(exportData);
                    lista.Add(exportData.ToArray());

                    return File(exportData.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "OTs.xlsx");
                }


            }


        }

    }
}