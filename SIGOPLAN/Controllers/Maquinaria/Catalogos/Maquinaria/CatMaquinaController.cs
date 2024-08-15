using Core.DAO.Contabilidad.Reportes;
using Core.DAO.Maquinaria.Captura;
using Core.DAO.Maquinaria.Catalogos;
using Core.DAO.Maquinaria.Inventario;
using Core.DAO.Maquinaria.Reporte.CuadroComparativo;
using Core.DAO.Principal.Alertas;
using Core.DTO;
using Core.DTO.Contabilidad;
using Core.DTO.Maquinaria.Catalogos;
using Core.DTO.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Reporte.CuadroComparativo.Equipo;
using Core.Entity.Principal.Alertas;
using Core.Enum.Maquinaria;
using Core.Enum.Principal.Alertas;
using Data.DAO.Maquinaria.Inventario;
using Data.Factory.Contabilidad.Reportes;
using Data.Factory.Maquinaria.Captura;
using Data.Factory.Maquinaria.Catalogos;
using Data.Factory.Maquinaria.Inventario;
using Data.Factory.Maquinaria.Reporte.CuadroComparativo;
using Data.Factory.Principal.Alertas;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.DTO.Principal.Generales;
using Core.DTO.Maquinaria;
using Core.DTO.Maquinaria.Inventario.Comparativos;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SIGOPLAN.Controllers.Maquinaria.Catalogos.Maquinaria
{
    public class CatMaquinaController : BaseController
    {



        #region Factory
        private IAlertasDAO alertasFS;
        private ICadenaProductivaDAO cadenaFS;
        private IMaquinaDAO maquinaFS;
        private IGrupoMaquinariaDAO grupoMaquinariaFS;
        private IAsignacionEquiposDAO asignacionEquiposFS;
        private ISolicitudEquipoDetDAO solicitudEquipoDetFS;
        private ISolicitudEquipoDAO solicitudEquipoFS;
        private ITipoBajaDAO tipoBajaFS;
        private ICapturaHorometroDAO horometroFS;
        private ICCEquipoDAO CCEquipoFS;
        private ICCFinanieroDAO CCFinancieroFS;
        private IComparativoDAO ComparativoDAO;
        #endregion
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            cadenaFS = new CadenaProductivaFactoryServices().getCadenaProductivaService();
            solicitudEquipoFS = new SolicitudEquipoFactoryServices().getSolicitudEquipoServices();
            solicitudEquipoDetFS = new SolicitudEquipoDetFactoryServices().getSolicitudEquipoDetServices();
            maquinaFS = new MaquinaFactoryServices().getMaquinaServices();
            asignacionEquiposFS = new AsignacionEquiposFactoryServices().getAsignacionEquiposFactoryServices();
            grupoMaquinariaFS = new GrupoMaquinariaFactoryServices().getGrupoMaquinariaService();
            tipoBajaFS = new TipoBajaFactoryServices().getTipoBajaService();
            horometroFS = new CapturaHorometroFactoryServices().getCapturaHorometroServices();
            alertasFS = new AlertaFactoryServices().getAlertaService();
            CCEquipoFS = new CCEquipoFactoryServices().getEquipoServices();
            CCFinancieroFS = new CCFinancieroFactoryServices().getFinancieroServices();
            ComparativoDAO = new ComparativoFactoryServices().getComparativoFactoryService();

            base.OnActionExecuting(filterContext);
        }

        // GET: CatMaquina
        public ActionResult Index()
        {
            var usuarioDTO = vSesiones.sesionUsuarioDTO;

            if (usuarioDTO != null)
            {
                ViewBag.pagina = "catalogo";
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Usuario");
            }
        }

        public ActionResult AnexosMaquinaria()
        {
            return View();
        }

        public ActionResult FillGrid_Maquina(MaquinaFiltrosDTO obj)
        {
            var result = new Dictionary<string, object>();
            var listResult = maquinaFS.FillGridMaquina(obj).
                Select(x => new
                {
                    id = x.id,
                    noEconomico = x.noEconomico,
                    tipo = x.grupoMaquinaria.tipoEquipo.descripcion,
                    grupo = x.grupoMaquinaria.descripcion,
                    modelo = x.modeloEquipo.descripcion,
                    descripcion = string.IsNullOrEmpty(x.descripcion) ? "" : x.descripcion,
                    grupoMaquinariaID = x.grupoMaquinariaID,
                    modeloEquipoID = x.modeloEquipoID,
                    tipoEquipoID = x.grupoMaquinaria.tipoEquipoID,
                    marcaID = x.marcaID,
                    anio = x.anio,
                    placas = x.placas,
                    noSerie = x.noSerie,
                    aseguradoraID = x.aseguradoraID,
                    noPoliza = x.noPoliza,
                    fechaAdquisicion = x.fechaAdquisicion.Year + "-" + x.fechaAdquisicion.Month + "-" + x.fechaAdquisicion.Day,
                    proveedor = x.proveedor,
                    fechaPoliza = x.fechaPoliza.Year + "-" + x.fechaPoliza.Month + "-" + x.fechaPoliza.Day,
                    TipoCombustibleID = x.TipoCombustibleID,
                    capacidadTanque = x.capacidadTanque,
                    unidadCarga = x.unidadCarga,
                    capacidadCarga = x.capacidadCarga,
                    horometroAdquisicion = x.horometroAdquisicion,
                    horometroActual = x.horometroActual,
                    renta = x.renta == true ? "1" : "0",
                    tipoEncierro = x.tipoEncierro,
                    estatus = (x.estatus == 1) ? "ACTIVO" : "INACTIVO",
                    tipoCaptura = x.TipoCaptura
                });

            result.Add("current", 1);
            result.Add("rowCount", 1);
            result.Add("total", listResult.Count());
            result.Add("rows", listResult);
            result.Add(SUCCESS, true);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillCboTipo_Maquina(bool estatus)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, maquinaFS.FillCboTipoMaquinaria(estatus).Select(x => new { Value = x.id, Text = x.descripcion }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillCboFiltro_Maquina(bool estatus)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, maquinaFS.FillCboFiltroGrupoMaquinaria(estatus).Select(x => new { Value = x.id, Text = x.descripcion }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillCboGrupo_Maquina(int idTipo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, maquinaFS.FillCboGrupoMaquinaria(idTipo).Select(x => new { Value = x.id, Text = x.descripcion, Prefijo = (x.prefijo + "-" + x.noEco.ToString().PadLeft(3, '0')) }).OrderBy(x => x.Text));
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboGrupos()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, maquinaFS.GetGrupoMaquinarias().Select(x => new { Value = x.id, Text = x.descripcion, Prefijo = (x.prefijo + "-" + x.noEco.ToString().PadLeft(3, '0')) }).OrderBy(x => x.Text));
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboModelo_Maquina(int idMarca)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, maquinaFS.FillCboModeloEquipo(idMarca).Select(x => new { Value = x.id, Text = x.descripcion }));
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillCboMarca_Maquina(int idGrupo)
        {
            var result = new Dictionary<string, object>();
            //try
            //{
                result.Add(ITEMS, maquinaFS.FillCboMarcasEquipo(idGrupo).Select(x => new { Value = x.id, Text = x.descripcion }));
                result.Add(SUCCESS, true);

            //}
            //catch (Exception e)
            //{
            //    result.Add(MESSAGE, e.Message);
            //    result.Add(SUCCESS, false);
            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillCboAseguradora_Maquina(bool estatus)
        {
            var result = new Dictionary<string, object>();
            try
            {
                //  var resultData = maquinaFactoryServices.FillCboMarcasEquipo(idGrupo).Select(x => new { Value = x.id, Text = x.descripcion }).ToList();
                result.Add(ITEMS, maquinaFS.FillCboAseguradora(estatus).Select(x => new { Value = x.id, Text = x.descripcion }));
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult fillCboEconomicos(int idGrupo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                //  var resultData = maquinaFactoryServices.FillCboMarcasEquipo(idGrupo).Select(x => new { Value = x.id, Text = x.descripcion }).ToList();
                result.Add(ITEMS, maquinaFS.FillCboEconomicos(idGrupo).Select(x => new { Value = x.id, Text = x.noEconomico }));
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillCbo_TipoEncierro()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, GlobalUtils.ParseEnumToCombo<Tipo_EncierroEnum>());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillCbo_TipoCombustible()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, GlobalUtils.ParseEnumToCombo<TipoCombustibleEnum>());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillCbo_UnidadCarga()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, GlobalUtils.ParseEnumToCombo<UnidadCargaEnum>());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillCboTiposArchivos()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, GlobalUtils.ParseEnumToCombo<AnexosArchivosMaquinariaEnum>());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult FillCbo_Anios()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, GlobalUtils.getFecha(15));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillCboTipoBaja()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var item = tipoBajaFS.FillCboTipoBaja();
                result.Add(ITEMS, item.Select(x => new { Text = x.Motivo, Value = x.id }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveOrUpdate_Maquinaria(tblM_CatMaquina obj, int Actualizacion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (Actualizacion == 1)
                {
                    if (obj.fechaEntregaSitio != null && obj.fechaEntregaSitio.Year < 2000)
                    {
                        obj.fechaEntregaSitio = DateTime.Now;
                    }
                    maquinaFS.Guardar(obj);
                    var r = maquinaFS.getGrupoMaquina(obj.grupoMaquinariaID);
                    tblM_CatGrupoMaquinaria maquina = new tblM_CatGrupoMaquinaria
                    {
                        id = r.id,
                        descripcion = r.descripcion,
                        estatus = r.estatus,
                        tipoEquipo = r.tipoEquipo,
                        prefijo = r.prefijo,
                        tipoEquipoID = r.tipoEquipoID,
                        noEco = r.noEco + 1
                    };
                    grupoMaquinariaFS.Guardar(maquina);


                }
                else
                {
                    obj.IdUsuarioBaja = Actualizacion == 3 ? vSesiones.sesionUsuarioDTO.id : 0;
                    obj.fechaEntregaSitio = DateTime.Now;
                    obj.fechaBaja = DateTime.Now;
                    obj.estatus = Actualizacion == 3 ? 0 : 1;
                    obj.TipoBajaID = obj.TipoBajaID;
                    obj.kmBaja = obj.kmBaja;
                    maquinaFS.Guardar(obj);
                }
                result.Add(MESSAGE, GlobalUtils.getMensaje(Actualizacion));
                result.Add(SUCCESS, true);
            }

            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadTablaAnexosMaquinaria(List<string> ccs, int grupo, int Economico)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var listaEconomicos = maquinaFS.GetAllMaquinas().Where(x => (ccs != null ? ccs.Contains(x.centro_costos) : x.id == x.id) && (grupo != 0 ? x.grupoMaquinariaID == grupo : x.id == x.id) && (Economico != 0 ? x.id == Economico : x.id == x.id)).ToList();

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveOrUpdateDarBaja(tblM_CatMaquina obj, int Actualizacion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var Maquinaria = maquinaFS.GetMaquinaByID(obj.id).FirstOrDefault();
                if (Actualizacion == 1)
                {
                    if (obj.fechaEntregaSitio != null && obj.fechaEntregaSitio.Year < 2000)
                    {
                        obj.fechaEntregaSitio = DateTime.Now;
                    }


                    if (Maquinaria != null)
                    {

                        Maquinaria.fechaBaja = obj.fechaBaja;
                        Maquinaria.HorometroBaja = obj.HorometroBaja;
                        Maquinaria.IdUsuarioBaja = getUsuario().id;
                        Maquinaria.kmBaja = obj.kmBaja;
                        Maquinaria.TipoBajaID = obj.TipoBajaID;
                        Maquinaria.estatus = 0;


                        maquinaFS.Guardar(Maquinaria);
                        var r = maquinaFS.getGrupoMaquina(obj.grupoMaquinariaID);

                        if (r != null)
                        {
                            tblM_CatGrupoMaquinaria maquina = new tblM_CatGrupoMaquinaria
                            {
                                id = r.id,
                                descripcion = r.descripcion,
                                estatus = r.estatus,
                                tipoEquipo = r.tipoEquipo,
                                prefijo = r.prefijo,
                                tipoEquipoID = r.tipoEquipoID,
                                noEco = r.noEco + 1
                            };
                            grupoMaquinariaFS.Guardar(maquina);
                        }

                    }

                }
                else
                {

                    tblM_CatMaquina objActualizar = new tblM_CatMaquina();
                    var objRawMaquina = maquinaFS.GetMaquinaByID(obj.id).FirstOrDefault();


                    if (objRawMaquina != null)
                    {

                        objActualizar = objRawMaquina;
                        objActualizar.IdUsuarioBaja = Actualizacion == 3 ? vSesiones.sesionUsuarioDTO.id : 0;
                        objActualizar.fechaEntregaSitio = DateTime.Now;
                        objActualizar.fechaBaja = DateTime.Now;
                        objActualizar.estatus = 0;
                        objActualizar.TipoBajaID = obj.TipoBajaID;
                        objActualizar.kmBaja = obj.kmBaja;
                        objActualizar.centro_costos = "0";

                        //objActualizar.grupoMaquinaria = new tblM_CatGrupoMaquinaria();
                        //objActualizar.marca = new tblM_CatMarcaEquipo();
                        //objActualizar.modeloEquipo = new tblM_CatModeloEquipo();

                        maquinaFS.Guardar(objActualizar);

                    }

                }
                result.Add(MESSAGE, GlobalUtils.getMensaje(Actualizacion));
                result.Add(SUCCESS, true);

                var AletaVisto = alertasFS.getAlertasByUsuarioAndSistema(1123, 1);

                if (AletaVisto.Count > 0)
                {
                    var objtemp = AletaVisto.FirstOrDefault(x => x.objID.Equals(Maquinaria.id));

                    if (objtemp != null)
                    {
                        objtemp.visto = true;

                        alertasFS.updateAlerta(objtemp);
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

        public ActionResult AutorizarAsignacionNoEconomico()
        {
            return View();
        }

        public ActionResult AsignacionNoEconomico()
        {
            var usuario = base.getUsuario();
            int idDep = usuario.puesto.departamentoID;
            // getAction("asignacionEconomico");
            ViewBag.vista = 1;
            if (getAction("asignacionEconomico"))
            {
                ViewBag.VisibleAsignar = "active";
                ViewBag.VisiblePendientes = "hidden";
            }
            else if (getAction("fichaTecnica"))
            {
                ViewBag.VisibleAsignar = "hidden";
                ViewBag.VisiblePendientes = "active";
            }
            else if (getAction("LibreVista"))
            {
                ViewBag.VisibleAsignar = "activo";
            }
            else
            {
                ViewBag.VisiblePendientes = "active";
            }


            return View();
        }
        public ActionResult getListaMaquinas(MaquinaFiltrosDTO obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var res = maquinaFS.getListaMaquinaria(obj);
                if (obj.estatus == 0)
                {
                    result.Add("Maquinas", res.Select
                        (x => new
                        {
                            Economico = (x.noEconomico == null || x.noEconomico == "" ? "No Asignado" : x.noEconomico),
                            Descripcion = x.descripcion,
                            idMaquina = x.id

                        }).ToList());
                }
                else
                {
                    result.Add("Maquinas", res.Where(x => x.noEconomico != "").Select(x => new
                    {
                        Economico = (x.noEconomico == null ? "No Asignado" : x.noEconomico),
                        Descripcion = x.descripcion,
                        idMaquina = x.id

                    }).ToList());
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
        public ActionResult setDataEconomicos(FichaTecnicaAltaDTO obj, int idAsignacion, FichaTecnicaAltaDTO setImprimible, int lugarEntrega, string LibreAbordo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<FichaTecnicaAltaDTO> setReporte = new List<FichaTecnicaAltaDTO>();
                setReporte.Add(setImprimible);
                Session["RFichaTecnica"] = setReporte;
                DateTime fecha = Convert.ToDateTime(obj.EntregaSitio);
                var grupo = grupoMaquinariaFS.getDataGrupo(Convert.ToInt32(obj.Descripcion));
                var objAsignacion = asignacionEquiposFS.GetAsiganacionById(idAsignacion);
                string centro_Costos = "";
                if (lugarEntrega == 1)
                {
                    objAsignacion.estatus = 2;
                    objAsignacion.CCOrigen = objAsignacion.cc;
                    centro_Costos = objAsignacion.cc;
                }
                else
                {

                    objAsignacion.estatus = 1;
                    objAsignacion.CCOrigen = "1010";
                    centro_Costos = "1010";
                }
                tblM_CatMaquina objeto = new tblM_CatMaquina();
                objeto = new tblM_CatMaquina
                {
                    ProveedorID = obj.ProveedorID,
                    proveedor = obj.Proveedor,
                    fechaEntregaSitio = fecha,
                    lugarEntregaProveedor = obj.LugarEntrega,
                    costoEquipo = Convert.ToDecimal(obj.CostoEquipo),
                    numArreglo = obj.Arreglo,
                    anio = Convert.ToInt32(obj.añoEquipo),
                    arregloCPL = obj.ArregloMotor,
                    ordenCompra = obj.OrdenCompra,
                    marcaMotor = obj.MarcaMotor,
                    modeloMotor = obj.ModeloMotor,
                    numSerieMotor = obj.SerieMotor,
                    renta = obj.CodicionesUso == "1" ? false : true,
                    CondicionUso = obj.CodicionesUso != "0" ? 0 : 1,
                    tipoAdquisicion = Convert.ToInt32(obj.Adquisicion),
                    fabricacion = Convert.ToInt32(obj.LugarFabricacion),
                    numPedimento = obj.Pedimento,
                    marcaID = Convert.ToInt32(obj.Marca),
                    modeloEquipoID = Convert.ToInt32(obj.Modelo),
                    grupoMaquinariaID = Convert.ToInt32(obj.Descripcion),
                    horometroAdquisicion = Convert.ToInt32(Convert.ToDecimal(obj.horometro)),
                    noSerie = obj.NoSerie,
                    fechaAdquisicion = DateTime.Now,
                    fechaPoliza = DateTime.Now,
                    aseguradoraID = 3,
                    descripcion = grupo != null ? grupo.descripcion : "",
                    centro_costos = centro_Costos,
                    CostoRenta = obj.CostoRenta,
                    UtilizacionHoras = obj.UtilizacionHoras,
                    TipoCambio = obj.TipoCambio,
                    LibreAbordo = LibreAbordo,
                    EconomicoCC = obj.EconomicoCC,
                    Garantia = obj.Garantia,
                    Comentario = obj.Comentario,
                    redireccionamientoVenta = false,
                    empresa = obj.empresa,
                    tieneSeguro = obj.tieneSeguro,
                    ManualesOperacion = obj.ManualesOperacion
                };

                maquinaFS.Guardar(objeto);

                //  var objAsignacion = asignacionEquiposFactoryServices.GetAsiganacionById(idAsignacion);
                objAsignacion.noEconomicoID = objeto.id;

                asignacionEquiposFS.SaveOrUpdate(objAsignacion);

                maquinaFS.NotificarAltaFichaTecnica(objeto, setImprimible);

                result.Add(MESSAGE, "");
                result.Add(SUCCESS, true);
            }

            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getDataSolicitudes()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var table = asignacionEquiposFS.getAsignacionesCompras();
                if (table.Any())
                {

                    var lstIdAsignacion = (from asignacion in table select asignacion.id).ToList();
                    //var lstCCEquipo = CCEquipoFS.GetCuadroEquipo(lstIdAsignacion);
                    var lstCCFinanciero = CCFinancieroFS.getCuadrosFinancieros(lstIdAsignacion);
                    var obj = table.Select(x => new
                    {
                        idAsignacion = x.id,
                        idSolicitud = x.solicitudEquipoID,
                        CentroCostos = x.SolicitudEquipo.CC,
                        CCDescripcion = ComparativoDAO.getDescripcion(x.SolicitudEquipo.CC),
                        noSolicitud = x.SolicitudEquipo.folio,
                        TipoSolicitud = x.Economico,
                        GrupoEquipo = x.SolicitudDetalle.GrupoMaquinaria.descripcion,
                        Modelo = x.SolicitudDetalle.ModeloEquipo.descripcion,
                        Comentario = x.SolicitudDetalle.Comentario,
                        FechaPromesa = x.FechaPromesa == null ? "---" : x.FechaPromesa.ToShortDateString(),
                        /*CCEquipo = lstCCEquipo.Where(w => w.IdAsignacion == x.id).Select(s => new 
                        { 
                            id = s.Id,
                            Estado = s.Estado
                        }).FirstOrDefault(),*/
                        btnFinanciero = ComparativoDAO.getAutFin(x.id, 1).Select(y => y.check).FirstOrDefault(),
                        btnAdquisicion = ComparativoDAO.getAutFin(x.id, 0).Select(y => y.check).FirstOrDefault(),

                        CCFinanciero = lstCCFinanciero.Where(w => w.IdAsignacion == x.id).Select(s => new
                        {
                            id = s.Id,
                            Estado = s.Estado
                        }).FirstOrDefault(),
                        Activarbutton = true // ComparativoDAO.ActivarBtn(x.id)
                    });
                    result.Add("tblEquiporenta", obj);
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
        private string GetInfoSolicitud(int id, int consulta)
        {
            var obj = solicitudEquipoDetFS.DetSolicitud(id);
            try
            {


                if (obj != null)
                {
                    switch (consulta)
                    {
                        case 1:
                            {
                                string cadena = "";
                                cadena = obj.GrupoMaquinaria.descripcion;
                                return cadena;

                            }
                        case 2:
                            {
                                string cadena = "";
                                if (obj.ModeloEquipo != null)
                                {
                                    cadena = obj.ModeloEquipo.descripcion;
                                }

                                return cadena;

                            }
                        case 3:
                            {
                                string cadena = "";
                                cadena = obj.Comentario;
                                return cadena;
                            }
                        case 4:
                            {
                                var objAsignacion = asignacionEquiposFS.getAsignacionesCompras().FirstOrDefault(x => x.SolicitudDetalleId.Equals(id));
                                string cadena = "";
                                if (objAsignacion != null)
                                {
                                    cadena = objAsignacion.FechaPromesa.ToShortDateString();
                                }
                                return cadena;

                            }
                        default:
                            return "";
                    }
                }
                return "";
            }
            catch (Exception)
            {
                return "";
            }



        }
        public ActionResult getMaquinaFichaTecnica(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var res = maquinaFS.GetMaquinaByID(id);
                if (res != null)
                {

                    Session["RFichaTecnica"] = res.Select(x => new FichaTecnicaAltaDTO
                    {
                        Adquisicion = x.tipoAdquisicion.ToString(),
                        añoEquipo = x.anio.ToString(),
                        Arreglo = x.numArreglo,
                        ArregloMotor = x.arregloCPL,
                        CodicionesUso = x.CondicionUso == 1 ? "Renta" : "Propio",
                        CostoEquipo = (x.CondicionUso == 1 ? x.CostoRenta.ToString("C2") : x.costoEquipo.ToString("C2")) + (x.TipoCambio == 1 ? " MN" : "dlls") + "+ IVA",
                        Descripcion = x.descripcion,
                        EntregaSitio = x.fechaEntregaSitio.ToShortDateString(),
                        //horometro = x.CondicionUso == 1 ? x.UtilizacionHoras.ToString() : x.horometroAdquisicion.ToString(),
                        horometro = x.horometroAdquisicion.ToString(),
                        LugarEntrega = x.lugarEntregaProveedor,
                        LugarFabricacion = x.fabricacion.ToString(),
                        Marca = x.marca.descripcion,
                        MarcaMotor = x.marcaMotor,
                        Modelo = x.modeloEquipo.descripcion,
                        ModeloMotor = x.modeloMotor,
                        NoSerie = x.noSerie,
                        OrdenCompra = x.ordenCompra,
                        Pedimento = x.numPedimento,
                        Proveedor = x.proveedor,
                        Economico = x.noEconomico,
                        SerieMotor = x.numSerieMotor,
                        TipoEquipo = x.grupoMaquinaria.tipoEquipoID.ToString(),
                        fechaAdquisicion = x.fechaAdquisicion.ToShortDateString(),
                        LibreAbordo = x.LibreAbordo ?? "",
                        Comentario = x.Comentario,
                        Garantia = x.Garantia,
                        tieneSeguro = x.tieneSeguro ?? false
                    }).ToList();
                    Session["RFichaTecnicaUtilizacionHoras"] = res.First().UtilizacionHoras.ToString();

                    result.Add(SUCCESS, true);
                }
                else
                {
                    result.Add(SUCCESS, false);
                    Session["RFichaTecnicaUtilizacionHoras"] = "";
                }
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getInformacionMaquinaEDIT(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var res = maquinaFS.GetMaquinaByID(id);


                if (res != null)
                {
                    // res.FirstOrDefault(x=>x.grupoMaquinariaID)
                    var GetEconomico = maquinaFS.GetParcialEconomico(id);

                    result.Add("EditEquipo", res.Select(x => new
                    {
                        Tipo = x.grupoMaquinaria.tipoEquipoID,
                        Grupo = x.grupoMaquinariaID,
                        Descripcion = x.descripcion,
                        Marca = x.marcaID,
                        Modelo = x.modeloEquipoID,
                        Año = x.anio,
                        noSerie = x.noSerie,
                        Aseguradora = x.aseguradoraID,
                        Poliza = x.noPoliza,
                        FechaPoliza = x.fechaPoliza.ToShortDateString(),
                        Placas = x.placas,
                        TipoEncierro = x.tipoEncierro,
                        TipoCombustible = x.TipoCombustibleID,
                        CapacidadTanque = x.capacidadTanque,
                        TipoCaptura = x.TipoCaptura,
                        Proveedor = x.proveedor,
                        horometroAdquisicion = x.horometroAdquisicion,
                        horometroActual = x.horometroActual,
                        renta = x.renta,
                        noEconomico = x.noEconomico ?? "",
                        tieneSeguro = x.tieneSeguro.HasValue ? x.tieneSeguro.Value : false,
                        empresa = x.empresa > 0 ? x.empresa : 2
                    }));

                    result.Add("NumEconomico", GetEconomico);
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
        public ActionResult updateEconomico(tblM_CatMaquina obj)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var tempMaquina = maquinaFS.GetMaquinaByID(obj.id).FirstOrDefault(); ;
                obj.fechaEntregaSitio = tempMaquina.fechaEntregaSitio;
                obj.lugarEntregaProveedor = tempMaquina.lugarEntregaProveedor;
                obj.ordenCompra = tempMaquina.ordenCompra == "" ? "0" : tempMaquina.ordenCompra;
                obj.costoEquipo = tempMaquina.costoEquipo;
                obj.numArreglo = tempMaquina.numArreglo;
                obj.marcaMotor = tempMaquina.marcaMotor;
                obj.modeloMotor = tempMaquina.modeloMotor;
                obj.numSerieMotor = tempMaquina.numSerieMotor;
                obj.arregloCPL = tempMaquina.arregloCPL;
                obj.CondicionUso = tempMaquina.CondicionUso;
                obj.tipoAdquisicion = tempMaquina.tipoAdquisicion;
                obj.fabricacion = tempMaquina.fabricacion;
                obj.numPedimento = tempMaquina.numPedimento;
                obj.fechaAdquisicion = DateTime.Now;
                obj.CostoRenta = tempMaquina.CostoRenta;
                obj.UtilizacionHoras = tempMaquina.UtilizacionHoras;
                obj.TipoCambio = tempMaquina.TipoCambio;
                obj.centro_costos = tempMaquina.centro_costos;
                obj.fechaAdquisicion = tempMaquina.fechaAdquisicion;
                obj.fechaEntregaSitio = tempMaquina.fechaEntregaSitio;
                obj.Garantia = tempMaquina.Garantia;
                obj.Comentario = tempMaquina.Comentario;
                obj.LibreAbordo = tempMaquina.LibreAbordo;
                //obj.tieneSeguro = obj.aseguradoraID != 3 && obj.aseguradoraID != 0;
                obj.empresa = tempMaquina.empresa;
                obj.tieneSeguro = tempMaquina.tieneSeguro;

                var x = asignacionEquiposFS.getEconomicoAsignado(obj.id);
                maquinaFS.Guardar(obj);
                asignacionEquiposFS.SaveOrUpdate(x);

                if (!asignacionEquiposFS.GetAsginadosRecibidos(x.id))
                {
                    var objSolicitud = solicitudEquipoDetFS.getSolicitudbyID(x.solicitudEquipoID);
                    objSolicitud.Estatus = true;
                    solicitudEquipoFS.Guardar(objSolicitud);
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
        public ActionResult GetHorometroFinal(string eco)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var horometro = horometroFS.GetHorometroFinal(eco);
                result.Add("horometro", horometro);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetHormetroFinalByMaquina(int noEconomidoID)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var Maquina = maquinaFS.GetMaquinaByID(noEconomidoID).FirstOrDefault();
                string econo = "";

                if (Maquina != null)
                {
                    econo = Maquina.noEconomico;

                }

                var horometro = horometroFS.GetHorometroFinal(econo);

                result.Add("Economico", econo);
                result.Add("horometro", horometro);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetNumeroEconomico(int idGrupo, bool renta)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var GetEconomico = maquinaFS.GetNumeroEconomico(idGrupo, renta);

                result.Add("NumEconomico", GetEconomico);
                result.Add(SUCCESS, true);


            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getInfoAsignacion(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var objAsignacion = asignacionEquiposFS.GetAsiganacionById(id);

                if (objAsignacion != null)
                {
                    var idDetalle = objAsignacion.SolicitudDetalleId;


                    var objSolicitudDetalle = solicitudEquipoDetFS.DetSolicitud(idDetalle);
                    if (objSolicitudDetalle != null)
                    {
                        result.Add("tipoId", objSolicitudDetalle.tipoMaquinariaID);
                        result.Add("grupoId", objSolicitudDetalle.grupoMaquinariaID);

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
        public ActionResult fillCboProveedores()
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<ProveedorDTO> resultData = new List<ProveedorDTO>();
                ProveedorDTO dato = new ProveedorDTO();

                dato.RAZONSOCIAL = "CONSTRUPLAN";
                dato.NUMPROVEEDOR = "0";


                resultData = cadenaFS.ListaPRoveedores().OrderBy(x => x.RAZONSOCIAL).ToList();
                resultData.Add(dato);
                result.Add(ITEMS, resultData.Select(x => new { Value = x.NUMPROVEEDOR, Text = x.RAZONSOCIAL }));
                result.Add(SUCCESS, true);


            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillMultipleEconomicos(List<int> lstGrupo, List<int> lstModelo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var listaEconomicos = maquinaFS.GetAllMaquinas()
                    .Where(e => lstGrupo.Any(g => g.Equals(e.grupoMaquinariaID)))
                    .Where(e => lstModelo.Any(m => m.Equals(e.modeloEquipoID)))
                    .Select(e => new
                    {
                        Text = e.noEconomico,
                        Value = e.noEconomico
                    }).ToList();
                result.Add(ITEMS, listaEconomicos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region CUADRO COMPARATIVO
        public ActionResult AltaFinancieros()
        {
            return View();
        }

        public ActionResult getTablaComparativoAdquisicion(ComparativoDTO objFiltro)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstTablaComparativa = ComparativoDAO.getTablaComparativoAdquisicion(objFiltro);
                result.Add(ITEMS, lstTablaComparativa);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getTablaComparativoAutorizar(ComparativoDTO objFiltro)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstTablaComparativa = ComparativoDAO.getTablaComparativoAutorizar(objFiltro);
                result.Add(ITEMS, lstTablaComparativa);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getTablaComparativoAdquisicionDetalle(int idAsignacion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstTablaComparativa = ComparativoDAO.getTablaComparativoAdquisicionDetalle(idAsignacion, getUsuario().id);
                result.Add(ITEMS, lstTablaComparativa);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getTablaComparativoAdquisicionDetallePorCuadro(int idCuadro)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstTablaComparativa = ComparativoDAO.getTablaComparativoAdquisicionDetallePorCuadro(idCuadro, getUsuario().id);
                result.Add(ITEMS, lstTablaComparativa);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CargarCuadrosComparativos()
        {
            var json = Json(ComparativoDAO.CargarCuadrosComparativos(), JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        public ActionResult getTablaComparativoFinanciero()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstTablaComparativa = ComparativoDAO.getTablaComparativoFinanciero();
                result.Add(ITEMS, lstTablaComparativa);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getTablaComparativoFinancieroDetalle(int idFinanciero)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstTablaComparativa = ComparativoDAO.getTablaComparativoFinancieroDetalle(idFinanciero, getUsuario().id);

                result = lstTablaComparativa;
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult addeditTablaComparativoAdiquisicion(List<HttpPostedFileBase> file, List<ComparativoAdquisicion> objComparativo)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var lstTablaComparativa = ComparativoDAO.addeditTablaComparativoAdiquisicion(file, objComparativo);
                result.Add(ITEMS, lstTablaComparativa);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult deleteTablaComparativoAdiquisicion(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstTablaComparativa = ComparativoDAO.deleteTablaComparativoAdiquisicion(id);
                result.Add(ITEMS, lstTablaComparativa);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult addeditTablaComparativoFinanciero(List<ComparativoDTO> objComparativo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstTablaComparativa = ComparativoDAO.addeditTablaComparativoFinanciero(objComparativo);
                result.Add(ITEMS, lstTablaComparativa);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult deleteTablaComparativoFinanciero(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstTablaComparativa = ComparativoDAO.deleteTablaComparativoFinanciero(id);
                result.Add(ITEMS, lstTablaComparativa);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult addAdquisisionP(ComparativoDTO objComparativo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstTablaComparativa = ComparativoDAO.addAdquisisionP(objComparativo);
                result.Add(ITEMS, lstTablaComparativa);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CargarCuadroComparativo()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstTablaComparativa = ComparativoDAO.CargarCuadroComparativo();
                result.Add(ITEMS, lstTablaComparativa);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult guardarAutorizacion(List<tblM_ComparativoAdquisicionyRentaAutorizante> lstComparativo, ComparativoDTO objFiltro, bool Financiero)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var lstTablaComparativa = ComparativoDAO.guardarAutorizacion(lstComparativo, objFiltro, Financiero, getUsuario().id);
                result.Add(ITEMS, lstTablaComparativa);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ObtenerInformacionCuadro(int idCuadro)
        {
            return Json(ComparativoDAO.ObtenerInformacionCuadro(idCuadro), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarCuadroIndependiente(List<HttpPostedFileBase> listaArchivos)
        {
            
            var comparativo = JsonConvert.DeserializeObject<ComparativoDTO>(Request.Form["comparativo"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            var detalle = (JsonConvert.DeserializeObject<ComparativoAdquisicion[]>(Request.Form["detalle"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" })).ToList();
            var listaAutorizantes = (JsonConvert.DeserializeObject<tblM_ComparativoAdquisicionyRentaAutorizante[]>(Request.Form["listaAutorizantes"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" })).ToList();

            return Json(ComparativoDAO.GuardarCuadroIndependiente(comparativo, detalle, listaAutorizantes, listaArchivos), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarAutorizadores(int idAsignacion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstTablaComparativa = ComparativoDAO.CargarAutorizadores(idAsignacion);
                result.Add(ITEMS, lstTablaComparativa);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult CargarAutorizadoresFinanciero(int idAsignacion)
        {
            var result = new Dictionary<string, object>();
            try
            {

                Session["gpxBarra"] = null;
                Session["gpxLinea"] = null;

                var lstTablaComparativa = ComparativoDAO.CargarAutorizadoresFinanciero(idAsignacion);
                result.Add(ITEMS, lstTablaComparativa);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult CargarSolicitudes(int estado, string obra, DateTime fechaInicio, DateTime fechaFin)
        {
            var result = new Dictionary<string, object>();

            try
            {
                result.Add("tblEquiporenta", asignacionEquiposFS.CargarSolicitudes(estado, obra, fechaInicio, fechaFin));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarAsignacionSolicitud(int idCuadro, string folio)
        {
            return Json(ComparativoDAO.GuardarAsignacionSolicitud(idCuadro, folio), JsonRequestBehavior.AllowGet);
        }
        public ActionResult AutorizandoComparativo(int idComparativoDetalle, int idAsignacion, int idCuadro)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var lstTablaComparativa = ComparativoDAO.AutorizandoComparativo(idComparativoDetalle, idAsignacion, idCuadro, getUsuario().id);
                result.Add(ITEMS, lstTablaComparativa);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult indicadorColumnaMaximoVoto(int idAsignacion, int Tipo)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var lstTablaComparativa = ComparativoDAO.indicadorColumnaMaximoVoto(idAsignacion, Tipo);
                result.Add(ITEMS, lstTablaComparativa);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult getTablaComparativoFinancieroAutorizar()
        {
            var result = new Dictionary<string, object>();
            try
            {
                Session["gpxBarra"] = null;
                Session["gpxLinea"] = null;

                var lstTablaComparativa = ComparativoDAO.getTablaComparativoFinancieroAutorizar();
                result.Add(ITEMS, lstTablaComparativa);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult AutorizandoComparativoFinanciera(int idRow, int idAsignacion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstTablaComparativa = ComparativoDAO.AutorizandoComparativoFinanciera(idRow, idAsignacion, getUsuario().id);
                result.Add(ITEMS, lstTablaComparativa);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getAutorizanteAdquisicion(int idAsignacion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstTablaComparativa = ComparativoDAO.getAutorizanteAdquisicion(idAsignacion);
                result.Add(ITEMS, lstTablaComparativa);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult getAutorizanteAdquisicionPorCuadro(int idCuadro)
        {
            var result = new Dictionary<string, object>();

            try
            {
                result.Add(ITEMS, ComparativoDAO.getAutorizanteAdquisicionPorCuadro(idCuadro));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult getAutorizanteFinanciero(int idAsignacion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstTablaComparativa = ComparativoDAO.getAutorizanteFinanciero(idAsignacion);
                result.Add(ITEMS, lstTablaComparativa);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult FillFinanciero()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstFinancieros = ComparativoDAO.FillFinanciero();

                result.Add(ITEMS, lstFinancieros.Select(x => new ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = x.descripcion
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

        public ActionResult GuardarFinanciero(tblM_Comp_CatFinanciero financiero)
        {
            financiero.fechaRegistro = DateTime.Now;
            financiero.usuarioRegistra = getUsuario().id;
            var result = ComparativoDAO.GuardarFinanciero(financiero);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarPlazo(tblM_Comp_CapPlazo plazo, bool esEdicion)
        {
            plazo.fechaRegistro = DateTime.Now;
            plazo.usuarioRegistra = getUsuario().id;
            Dictionary<string, object> result = new Dictionary<string, object>();
            if (esEdicion) result = ComparativoDAO.EditarPlazo(plazo);
            else result = ComparativoDAO.GuardarPlazo(plazo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPlazo(int financieroID = 0, int plazoMeses = 0)
        {
            var result = ComparativoDAO.GetPlazo(financieroID, plazoMeses);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPlazoByID(int plazoID)
        {
            var result = ComparativoDAO.GetPlazoByID(plazoID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboFinancieros()
        {
            var result = ComparativoDAO.FillCboFinancieros();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LimpiarGxp(bool todas)
        {
            var r = new Dictionary<string, object>();

            if (todas)
            {
                Session["gpxBarra"] = null;
                Session["gpxLinea"] = null;
                r.Add(SUCCESS, true);
                r.Add("gpxBarra", null);
                r.Add("gpxLinea", null);
            }
            else
            {
                if (Session["gpxBarra"] != null)
                {
                    var _gpxBarras = (GpxsHighCharts)Session["gpxBarra"];
                    var _gpxLineas = (List<GpxSerieLineasBasicasDTO>)Session["gpxLinea"];

                    if (_gpxBarras.categories.Count <= 1)
                    {
                        Session["gpxBarra"] = null;
                        Session["gpxLinea"] = null;
                        r.Add(SUCCESS, true);
                        r.Add("gpxBarra", null);
                        r.Add("gpxLinea", null);
                    }
                    else
                    {
                        _gpxBarras.categories.RemoveAt(_gpxBarras.categories.Count - 1);
                        _gpxBarras.series.First().data.RemoveAt(_gpxBarras.series.First().data.Count - 1);
                        _gpxBarras.series.Last().data.RemoveAt(_gpxBarras.series.Last().data.Count - 1);

                        _gpxLineas.Remove(_gpxLineas.Last());

                        Session["gpxBarra"] = _gpxBarras;
                        Session["gpxLinea"] = _gpxLineas;
                        r.Add(SUCCESS, true);
                        r.Add("gpxBarra", _gpxBarras);
                        r.Add("gpxLinea", _gpxLineas);
                    }
                }
                else
                {
                    r.Add(SUCCESS, true);
                    r.Add("gpxBarra", null);
                    r.Add("gpxLinea", null);
                }
            }

            return Json(r);
        }

        public ActionResult LlenarDatosFinanciero(int financieraID, int plazoMeses, decimal precio, int mesesRestantes, int posicion)
        {
            var result = ComparativoDAO.LlenarDatosFinanciero(financieraID, plazoMeses, precio, mesesRestantes);

            if ((bool)result[SUCCESS])
            {
                if (Session["gpxBarra"] != null)
                {
                    var _gpxBarraAnterior = (GpxsHighCharts)Session["gpxBarra"];
                    var _gpxBarraNueva = (GpxsHighCharts)result["gpxBarra"];

                    var _gpxLineaAnterior = (List<GpxSerieLineasBasicasDTO>)Session["gpxLinea"];
                    var _gpxLineaNueva = (List<GpxSerieLineasBasicasDTO>)result["gpxLinea"];

                    //
                    if (posicion < _gpxLineaAnterior.Count)
                    {
                        //_gpxBarraAnterior.categories.RemoveAt(posicion);
                        //_gpxBarraAnterior.series.First().data.RemoveAt(posicion);
                        //_gpxBarraAnterior.series.Last().data.RemoveAt(posicion);

                        _gpxBarraAnterior.categories[posicion] = _gpxBarraNueva.categories.First();
                        _gpxBarraAnterior.series.First().data[posicion] = _gpxBarraNueva.series.First().data.First();
                        _gpxBarraAnterior.series.Last().data[posicion] = _gpxBarraNueva.series.Last().data.First();

                        _gpxLineaAnterior.ElementAt(posicion).name = _gpxLineaNueva.First().name;
                        _gpxLineaAnterior.ElementAt(posicion).data = _gpxLineaNueva.First().data;
                    }
                    else
                    {
                        _gpxBarraAnterior.categories.AddRange(_gpxBarraNueva.categories);
                        _gpxBarraAnterior.series.First().data.AddRange(_gpxBarraNueva.series.First().data);
                        _gpxBarraAnterior.series.Last().data.AddRange(_gpxBarraNueva.series.Last().data);

                        _gpxLineaAnterior.AddRange(_gpxLineaNueva);
                    }
                    //

                    result["gpxBarra"] = _gpxBarraAnterior;
                    Session["gpxBarra"] = result["gpxBarra"];

                    result["gpxLinea"] = _gpxLineaAnterior;
                    Session["gpxLinea"] = result["gpxLinea"];
                }
                else
                {
                    Session["gpxBarra"] = result["gpxBarra"];
                    Session["gpxLinea"] = result["gpxLinea"];
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerMensualidades(int financieraID, int plazoMeses, decimal precio)
        {
            var result = ComparativoDAO.ObtenerMensualidades(financieraID, plazoMeses, precio);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DescargarArchivo(long examen_id)
        {
            var array = ComparativoDAO.descargarArchivo(examen_id);
            if (array == null)
            {
                var resultado = new Dictionary<string,object>();
                resultado.Add(ITEMS, "No hay archivo descargable");
                resultado.Add(SUCCESS, true);
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            string pathExamen = ComparativoDAO.getFileName(examen_id);

            if (array != null)
            {
                return File(array, System.Net.Mime.MediaTypeNames.Application.Octet, pathExamen);
            }
            else
            {
                return View("ErrorDescarga");
            }

        }
        #endregion

    }
}