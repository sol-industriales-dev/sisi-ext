using Core.DTO;
using Core.DTO.Maquinaria.Captura.conciliacion;
using Core.DTO.Maquinaria.Catalogos;
using Core.Entity.Maquinaria.Captura;
using Core.Entity.Maquinaria.Catalogo.Cararatulas;
using Core.Enum.Maquinaria.ConciliacionHorometros;
using Data.Factory.Maquinaria.Captura;
using Data.Factory.Principal.Usuarios;
using Data.Factory.Maquinaria.Reporte;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Maquinaria.Capturas
{
    public class ConciliacionController : BaseController
    {
        ConciliacionFactoryServices cfs;
        RentabilidadFactoryServices rfs;
        UsuarioFactoryServices usuarioFactoryServices;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            cfs = new ConciliacionFactoryServices();
            rfs = new RentabilidadFactoryServices();
            usuarioFactoryServices = new UsuarioFactoryServices();
            base.OnActionExecuting(filterContext);
        }
        public ActionResult AutorizacionCaratula()
        {
            return View();
        }
        // GET: Conciliacion
        public ActionResult Caratula()
        {
            return View();
        }
        public ActionResult _nuevoRowPrecio()
        {
            return View();
        }
        public ActionResult Horometros()
        {
            return View();
        }
        public ActionResult Autoriza()
        {
            return View();
        }
        public ActionResult Facturado()
        {
            return View();
        }

        public ActionResult GetInfoAutorizacion(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var data = cfs.getConciliacionServices().loadAutorizacionCaratula(id);
                var usuario = getUsuario().id;
                bool usuarioFirma = false;
                string ElementoFirma = "";
                switch (usuario)
                {
                    case 4:
                        if (data.firmaVobo2 == 0 && data.usuarioFirma == 3)
                        {
                            usuarioFirma = true;
                            ElementoFirma = "btnVobo2";
                        }
                        break;
                    case 1064:
                        if (data.firmaVobo1 == 0 && data.usuarioFirma == 2)
                        {
                            usuarioFirma = true;
                            ElementoFirma = "btnVobo1";
                        }
                        break;
                    case 1164:
                        if (data.firmaAutoriza == 0 && data.usuarioFirma == 4)
                        {
                            usuarioFirma = true;
                            ElementoFirma = "btnDireccion";
                        }
                        break;
                    default:
                        break;
                }
                result.Add("ElementoFirma", ElementoFirma);
                result.Add("usuarioFirma", usuarioFirma);
                Session["infoAutorizacionCaratula"] = data;

                result.Add("data", data);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveOrUpdateAutorizacion(int obj, int Autoriza, int tipo, string comentario)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (tipo == 2 && (comentario == null || comentario.Trim().Length < 10))
                {
                    result.Add(MESSAGE, "No se rechazó la solicitud. El comentario viene vacío.");
                    result.Add(SUCCESS, false);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                var objReturn = cfs.getConciliacionServices().autorizacionUsuario(obj, Autoriza, tipo, comentario);
                result.Add(SUCCESS, objReturn);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        private string GetNombre(int usuarioID)
        {

            var objUsuario = usuarioFactoryServices.getUsuarioService().ListUsersById(usuarioID).FirstOrDefault();
            if (objUsuario != null)
            {
                return objUsuario.nombre + " " + objUsuario.apellidoPaterno + " " + objUsuario.apellidoMaterno;
            }
            return "";

        }
        public ActionResult loadAutorizadoresConciliacion(int conciliacionID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var objReturn = cfs.getConciliacionServices().loadAutorizacionFromConciliacacionId(conciliacionID);
                var usuarioActual = getUsuario().id;
                if (objReturn != null)
                {
                    var objUsuarioAdmin = usuarioFactoryServices.getUsuarioService().ListUsersById(objReturn.autorizaAdmin).FirstOrDefault();
                    var objUsuarioGerente = usuarioFactoryServices.getUsuarioService().ListUsersById(objReturn.autorizaGerenteID).FirstOrDefault();
                    var objUsuarioDirector = usuarioFactoryServices.getUsuarioService().ListUsersById(objReturn.autorizaDirector).FirstOrDefault();

                    result.Add("NombreAdmin", GetNombre(objReturn.autorizaAdmin));
                    result.Add("NombreGerente", GetNombre(objReturn.autorizaGerenteID));
                    result.Add("NombreDirector", GetNombre(objReturn.autorizaDirector));
                    result.Add("objAutorizacion", objReturn);
                    //bool autorizando = false;
                    switch (objReturn.autorizando)
                    {
                        case 1:
                            {
                                if (usuarioActual == objReturn.autorizaAdmin)
                                {
                                    result.Add("autorizando", true);
                                }
                                break;
                            }
                        case 2:
                            {
                                if (usuarioActual == objReturn.autorizaGerenteID)
                                {
                                    result.Add("autorizando", true);
                                }
                                break;
                            }
                        case 3:
                            {
                                if (usuarioActual == objReturn.autorizaDirector)
                                {
                                    result.Add("autorizando", true);
                                }
                                break;
                            }
                        default:
                            break;
                    }
                    result.Add(SUCCESS, true);
                }
                else
                {
                    result.Add(SUCCESS, false);
                }

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult loadTlbAutorizacionesCaratula(int cc, int estatus)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var objReturn = cfs.getConciliacionServices().loadTlbAutorizacionesCaratula(cc, estatus);
                result.Add("dataSet", objReturn.Select(x => new
                {
                    estadoCaratula = x.estadoCaratula == 0 ? "Pendiente" : x.estadoCaratula == 1 ? "Autorizado" : "Rechazada",
                    obraID = cfs.getConciliacionServices().GetNameObra(x.obraID),
                    autorizando = validaBoton(x.usuarioFirma, x),
                    id = x.id,
                    comentario = (x.comentario != null) ? x.comentario.Replace("\n", " ") : "",
                    ids = x.id
                }));
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private bool validaBoton(int p, tblM_AutorizacionCaratulaPreciosU x)
        {
            bool valida = false;
            switch (p)
            {
                case 1:
                    {
                        if (getUsuario().id == x.usuarioElaboraID)
                            return true;
                        else
                            return false;
                    }
                case 2:
                    {
                        if (getUsuario().id == x.usuarioVobo1)
                            return true;
                        else
                            return false;
                    }
                case 3:
                    {
                        if (getUsuario().id == x.usuarioVobo2)
                            return true;
                        else
                            return false;
                    }
                case 4:
                    {
                        if (getUsuario().id == x.usuarioAutoriza)
                            return true;
                        else
                            return false;
                    }
                default:
                    break;
            }
            return valida;
        }
        public ActionResult loadAutorizacionCaratula(int autoriazacionID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var objReturn = cfs.getConciliacionServices().loadAutorizacionCaratula(autoriazacionID);
                result.Add("objAutorizadores", objReturn);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult loadtlbAutorizaciones(int centroCostosID, int fechaID, DateTime? fechaInicio, DateTime? fechaFin, int estatus, bool esQuincena)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (fechaID == 0)
                {
                    fechaInicio = DateTime.Now;
                    fechaFin = DateTime.Now;
                }
                var objReturn = cfs.getConciliacionServices().getAutorizaciones(centroCostosID, fechaID, (DateTime)fechaInicio, (DateTime)fechaFin, estatus, esQuincena);
                result.Add(ITEMS, objReturn);
                result.Add("permiso", base.getAction("ValidaConsolidacion"));
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult loadtlbAutorizacionesPendientes(int centroCostosID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var objReturn = cfs.getConciliacionServices().getAutorizacionesPendientes(centroCostosID);
                result.Add(ITEMS, objReturn);
                result.Add("permiso", base.getAction("ValidaConsolidacion"));
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult loadAutorizacion(int validaID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add("autorizaciones", cfs.getConciliacionServices().loadAutorizacion(validaID));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult FillCboSemanas()
        {
            var result = new Dictionary<string, object>();
            try
            {
                DateTime fecha = new DateTime(DateTime.Now.Year, 01, 01);
                List<FechasDTO> ListaFechas = new List<FechasDTO>();
                ListaFechas = GlobalUtils.GetFechas(fecha);
                result.Add(ITEMS, ListaFechas);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult loadTablaConciliacionHorometros(tblM_EncCaratula enc, DateTime fechaInicio, DateTime fechaFinal, int fechaID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var getValida = cfs.getConciliacionServices().getConciliacionesExiste(fechaID, enc.ccID, fechaInicio, fechaFinal);
                //fechaFinal = fechaFinal.AddDays(-3);
                var dataResult = cfs.getConciliacionServices().getTblConciliacion(enc, fechaInicio, fechaFinal);
                result.Add(ITEMS, dataResult);
                result.Add("getValida", getValida);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult saveOrUpdateConciliacion(int centroCostosID, int fechaID, DateTime fechaInicio, DateTime fechaFin, List<tblM_CapConciliacionHorometros> obj)
        {
            var result = new Dictionary<string, object>();
            if (obj == null) obj = new List<tblM_CapConciliacionHorometros>();
            try
            {
                //var centroCostosActivos = getUsuario().;
                tblM_CapEncConciliacionHorometros objSet = new tblM_CapEncConciliacionHorometros();
                objSet.id = 0;
                objSet.centroCostosID = centroCostosID;
                objSet.estatus = 0;
                objSet.FechaCaptura = DateTime.Now;
                objSet.fechaID = fechaID;
                objSet.esQuincena = true;
                objSet.usuarioCaptura = getUsuario().id;
                objSet.anio = DateTime.Now.Year;
                objSet.fechaInicio = fechaInicio;
                objSet.fechaFin = fechaFin;
                objSet.facturado = false;
                objSet.factura = "";
                var dataResult = cfs.getConciliacionServices().saveOrUpdateConciliacion(obj, objSet);
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
        public ActionResult sendValidacion(int conciliacionID, int respuesta, string comentario)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (respuesta == 2 && (comentario == null || comentario.Trim().Length < 10))
                {
                    result.Add(MESSAGE, "No se rechazó la solicitud. El comentario viene vacío.");
                    result.Add(SUCCESS, false);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                var res = cfs.getConciliacionServices().sendValidacion(conciliacionID, respuesta, vSesiones.sesionUsuarioDTO.id, comentario);
                var objReturn = cfs.getConciliacionServices().loadAutorizacionFromConciliacacionId(conciliacionID);
                result.Add("Autorizadores", conciliacionID);
                if (objReturn != null)
                {
                    if (objReturn.autorizando == 4)
                    {
                        result.Add("enviarCorreo", true);
                    }
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
        public ActionResult fillCboCentrosCosto()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuarioID = getUsuario().id;
                //var centroCostosActivos = ufs.getUsuarioService().getCCsUsuario(usuarioID).Select(x=>x.cc).tol
                var dataResult = rfs.getRentabilidadDAO().getListaCCByUsuario(usuarioID, 1);
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

        public ActionResult getConsideraciones()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = cfs.getConciliacionServices().getLstFullConsiceracion();
                result.Add("lstIncluye", lst.Where(w => w.tipo.Equals((int)ConsideracionEnum.Incluye)).ToList());
                result.Add("lstNoIncluye", lst.Where(w => w.tipo.Equals((int)ConsideracionEnum.NoIncluye)).ToList());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getLstPrecios(int ccID, string cc)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstPrecio = new List<tblM_CapCaratula>();
                var lstConci = new List<tblM_EncCaratula_Concideracion>();
                var enc = cfs.getConciliacionServices().getEncabezado(ccID);

                DateTime fechaVigencia = DateTime.Now;
                if (enc.id == 0)
                {
                    lstPrecio = cfs.getConciliacionServices().getNewLstPrecios(cc);
                    fechaVigencia = fechaVigencia.AddMonths(6);
                }
                else
                {
                    lstPrecio = cfs.getConciliacionServices().getLstPrecios(enc.id);
                    lstConci = cfs.getConciliacionServices().getLstConsiceracionWhereEnc(enc.id);

                    fechaVigencia = enc.fechaVigencia;
                }
                result.Add("encabezado", enc);
                result.Add("Vencimiento", fechaVigencia.ToShortDateString());
                result.Add("lstConci", lstConci);
                var dtSet = lstPrecio
                    .GroupBy(g => new
                    {
                        g.idGrupo,
                        g.costo,
                        g.cargoFijo,
                        g.cOverhaul,
                        g.cMttoCorrectivo,
                        g.cCombustible,
                        g.cAceites,
                        g.cFiltros,
                        g.cAnsul,
                        g.cLlantas,
                        g.cCarrileria,
                        g.cHerramientasDesgaste,
                        g.cCargoOperador,
                        g.cPersonalMtto
                    })
                    .Select(p => new
                    {
                        activo = p.FirstOrDefault().activo,
                        costo = p.Key.costo,
                        EncCaratula = p.FirstOrDefault().EncCaratula,
                        equipo = p.FirstOrDefault().equipo,
                        id = p.FirstOrDefault().id,
                        idCaratula = p.FirstOrDefault().idCaratula,
                        unidad = p.FirstOrDefault().unidad,
                        idGrupo = p.Key.idGrupo,
                        idModelo = p.Select(s => s.idModelo).ToList(),
                        cargoFijo = p.Key.cargoFijo,
                        cOverhaul = p.Key.cOverhaul,
                        cMttoCorrectivo = p.Key.cMttoCorrectivo,
                        cCombustible = p.Key.cCombustible,
                        cAceites = p.Key.cAceites,
                        cFiltros = p.Key.cFiltros,
                        cAnsul = p.Key.cAnsul,
                        cLlantas = p.Key.cLlantas,
                        cCarrileria = p.Key.cCarrileria,
                        cHerramientasDesgaste = p.Key.cHerramientasDesgaste,
                        cCargoOperador = p.Key.cCargoOperador,
                        cPersonalMtto = p.Key.cPersonalMtto

                    }).ToList();
                result.Add("lstPrecio", dtSet);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarCaratula(tblM_EncCaratula enc, List<tblM_CapCaratula> lst, List<tblM_EncCaratula_Concideracion> lstCon)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var objGrupo = lst.GroupBy(x => new { x.idGrupo, x.idModelo, x.costo }).ToList();

                List<tblM_CapCaratula> lst2 = new List<tblM_CapCaratula>();
                foreach (var item in objGrupo)
                {

                    var obj = lst.Where(x => x.idGrupo == item.Key.idGrupo && x.idModelo == item.Key.idModelo && item.Key.costo == x.costo).FirstOrDefault();


                    lst2.Add(obj);
                }



                result.Add(SUCCESS, cfs.getConciliacionServices().GuardarCaratula(enc, lst2.ToList(), lstCon));
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult sendCorreoAjunto(int conciliacionID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                cfs.getConciliacionServices().setCorreo(conciliacionID);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public void sendCorreo(int conciliacionID)
        {
            try
            {
                cfs.getConciliacionServices().setCorreo(conciliacionID);
            }
            catch (Exception e)
            {
            }
        }
        public ActionResult fnReenviarCorreo(int conciliacionID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                cfs.getConciliacionServices().setReenviarCorreo(conciliacionID);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region ComboBox
        public ActionResult getCboCC()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, cfs.getConciliacionServices().getCboCC());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCboGrupo()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, cfs.getConciliacionServices().getCboGrupo());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCboModelo(int idGrupo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, cfs.getConciliacionServices().getCboModelo(idGrupo));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getCboUnidad()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, Enum.GetValues(typeof(unidadCostoEnum)).Cast<unidadCostoEnum>().ToList().Select(x => new
                {
                    Text = x.GetDescription(),
                    Value = x.GetHashCode()
                }));
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillCboQuincenas()
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<FechasDTO> resultFechas = new List<FechasDTO>();
                resultFechas.AddRange(GlobalUtils.GetQuincenas(DateTime.Now.Year));
                resultFechas.AddRange(GetQuincenas());
                result.Add(ITEMS, resultFechas);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillCboQuincenasVariables(int? ccID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<FechasDTO> resultFechas = new List<FechasDTO>();
                if (ccID != null)
                {
                    var ccData = cfs.getConciliacionServices().getCC((int)ccID);
                    var normal = ccData.esQuincenaNormal;

                    if (normal)
                    {
                        resultFechas.AddRange(GlobalUtils.GetQuincenasNormales(DateTime.Now.Year));

                    }
                    else
                    {
                        resultFechas.AddRange(GlobalUtils.GetQuincenasPorDia(DateTime.Now.Year, 28));
                        //resultFechas.AddRange(GetQuincenas());
                    }
                }
                else
                {
                    //resultFechas.AddRange(GlobalUtils.GetQuincenas(DateTime.Now.Year));
                    resultFechas.AddRange(GlobalUtils.GetQuincenasNormales(DateTime.Now.Year));
                    //resultFechas.AddRange(GetQuincenas());
                }
                result.Add(ITEMS, resultFechas);
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

        private List<FechasDTO> GetQuincenas()
        {
            var ListaFechas = new List<FechasDTO>();
            var FechaFin = new DateTime();
            var FechaInicio = new DateTime(DateTime.Now.Year - 1, 01, 01);
            var anioActual = FechaInicio.Year;
            var diasSem = 7;
            var i = 0;
            FechaInicio = FechaInicio.AddDays(diasSem);
            while (FechaInicio.Year == anioActual)
            {
                int diasMiercoles = ((int)DayOfWeek.Wednesday - (int)FechaInicio.DayOfWeek + 7) % 7;
                FechaInicio = FechaInicio.AddDays(diasMiercoles);
                FechaFin = FechaInicio.AddDays(diasSem);
                int diasMartes = ((int)DayOfWeek.Tuesday - (int)FechaInicio.DayOfWeek + 7) % 7;
                FechaFin = FechaFin.AddDays(diasMartes);
                ListaFechas.Add(new FechasDTO()
                {
                    Value = ++i,
                    Text = FechaInicio.ToShortDateString() + " - " + FechaFin.ToShortDateString()
                });
                FechaInicio = FechaFin.AddDays(1);
            }
            return ListaFechas;
        }
        public ActionResult GetCaratulaComparacion(int caratulaActualID, int caratulaNuevaID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var ca = cfs.getConciliacionServices().getCaratulaByID(caratulaActualID);
                var cn = cfs.getConciliacionServices().getCaratulaByID(caratulaNuevaID);
                var htmlA = "";
                var htmlN = "";
                htmlA += "<table class='table table-responsive dataTable' style='width:100%'>";
                htmlA += "  <head>";
                htmlA += "      <tr style='background-color:#81bd72;'>";
                htmlA += "          <th>";
                htmlA += "              EQUIPO";
                htmlA += "          </th>";
                htmlA += "          <th>";
                htmlA += "              MODELO";
                htmlA += "          </th>";
                htmlA += "          <th>";
                htmlA += "              UNIDAD";
                htmlA += "          </th>";
                htmlA += "          <th>";
                htmlA += "              COSTO";
                htmlA += "          </th>";
                htmlA += "      </tr>";
                htmlA += "  </head>";
                htmlA += "  <body>";
                foreach (var x in ca)
                {
                    var existe = cn.FirstOrDefault(y => y.Equipo.Equals(x.Equipo));
                    var clase = existe == null ? "clsEliminado" : "";
                    htmlA += "      <tr class='" + clase + "'>";
                    htmlA += "          <td>";
                    htmlA += "              " + x.Equipo;
                    htmlA += "          </td>";
                    htmlA += "          <td>";
                    htmlA += "              " + x.Modelo;
                    htmlA += "          </td>";
                    htmlA += "          <td>";
                    htmlA += "              " + x.Unidad;
                    htmlA += "          </td>";
                    htmlA += "          <td>";
                    htmlA += "              " + x.CostoTotal;
                    htmlA += "          </td>";
                    htmlA += "      </tr>";
                }

                htmlA += "  </body>";
                htmlA += "</table>";
                //------------------------------------------
                htmlN += "<table class='table table-responsive dataTable' style='width:100%'>";
                htmlN += "  <head>";
                htmlN += "      <tr style='background-color:#81bd72;'>";
                htmlN += "          <th>";
                htmlN += "              EQUIPO";
                htmlN += "          </th>";
                htmlN += "          <th>";
                htmlN += "              MODELO";
                htmlN += "          </th>";
                htmlN += "          <th>";
                htmlN += "              UNIDAD";
                htmlN += "          </th>";
                htmlN += "          <th>";
                htmlN += "              COSTO";
                htmlN += "          </th>";
                htmlN += "      </tr>";
                htmlN += "  </head>";
                htmlN += "  <body>";
                foreach (var x in cn)
                {
                    var existe = ca.FirstOrDefault(y => y.Equipo.Equals(x.Equipo));
                    var clase = existe == null ? "clsAgregado" : "";
                    var modelo = "";
                    var unidad = "";
                    var costo = "";
                    if (existe != null)
                    {
                        var existeModelo = ca.FirstOrDefault(y => y.Equipo.Equals(x.Equipo) && y.Modelo.Equals(x.Modelo));
                        var existeUnidad = ca.FirstOrDefault(y => y.Equipo.Equals(x.Equipo) && y.Modelo.Equals(x.Modelo) && y.Unidad.Equals(x.Unidad));
                        var existeCosto = ca.FirstOrDefault(y => y.Equipo.Equals(x.Equipo) && y.CostoTotal.Equals(x.CostoTotal));
                        modelo = existeModelo == null ? "clsActualizado" : "";
                        unidad = existeUnidad == null ? "clsActualizado" : "";
                        costo = existeCosto == null ? "clsActualizado" : "";
                    }

                    htmlN += "      <tr class='" + clase + "'>";
                    htmlN += "          <td>";
                    htmlN += "              " + x.Equipo;
                    htmlN += "          </td>";
                    htmlN += "          <td class='" + modelo + "'>";
                    htmlN += "              " + x.Modelo;
                    htmlN += "          </td>";
                    htmlN += "          <td class='" + unidad + "'>";
                    htmlN += "              " + x.Unidad;
                    htmlN += "          </td>";
                    htmlN += "          <td class='" + costo + "'>";
                    htmlN += "              " + x.CostoTotal;
                    htmlN += "          </td>";
                    htmlN += "      </tr>";
                }

                htmlN += "  </body>";
                htmlN += "</table>";
                result.Add("actual", htmlA);
                result.Add("nueva", htmlN);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        #region Facturado

        public ActionResult getConciliacionesAFacturar(bool estado, DateTime ? fechaInicio, DateTime ? fechaFin, string folio, int cc = -1)
        {
            if (fechaInicio == null) { fechaInicio = DateTime.MinValue; }
            if (fechaFin == null) { fechaFin = DateTime.MaxValue; }
            var result = new Dictionary<string, object>();
            try
            {
                var conciliaciones = cfs.getConciliacionServices().getConciliacionesAFacturar(estado, folio, cc, (fechaInicio ?? default(DateTime)), (fechaFin ?? default(DateTime)));
                result.Add("conciliaciones", conciliaciones);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult indicarFacturacion(int conciliacionID, List<string> factura)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var exito = cfs.getConciliacionServices().indicarFacturacion(conciliacionID, factura);
                if(exito) result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getFacturasConciliacion(int conciliacionID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var facturas = cfs.getConciliacionServices().getFacturasConciliacion(conciliacionID).Select(x => new { 
                    factura = x
                });
                result.Add("facturas", facturas);
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

    }
}