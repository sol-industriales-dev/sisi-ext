using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.DAO.Maquinaria.Mantenimiento; // interfaz
using Core.Service.Maquinaria.Mantenimiento;//mantenimiento
using Data.Factory.Maquinaria.Mantenimiento;
using Core.Entity.Maquinaria.Mantenimiento;//propiedades
using Data.Factory.Maquinaria.Captura;//factory maquinaria
using Core.Entity.Maquinaria;
using Core.Entity.Maquinaria.Captura;//tabla horometros maquinaria
using Core.DTO.Maquinaria.Mantenimiento;
using SIGOPLAN.Controllers.Maquinaria.Capturas.Diarias;
using Data.DAO.Maquinaria.Reporte;
using Data.Factory.Maquinaria.Catalogos;
using Core.DTO.Maquinaria.Reporte;
using Core.Enum.Maquinaria.Reportes;
using Newtonsoft.Json;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Inventario;
using System.IO;
using Data.Factory.Principal.Archivos;
using OfficeOpenXml;
using Core.Entity.Maquinaria.Mantenimiento2;
using Infrastructure.DTO;
using Core.DTO.Maquinaria.Mantenimiento.DTO2._0;
using System.IO.Compression;
using Core.DTO.Maquinaria.Captura;
using Infrastructure.Utils;
using Core.DTO;
using Data.EntityFramework.Mapping;
using Core.DTO.Utils.Data;
using System.IO;
using Infrastructure.Utils;
using System.Collections.Generic;

namespace SIGOPLAN.Controllers.Maquinaria.Mantenimiento
{

    public class MatenimientoPMController : BaseController
    {

        private ModeloEquipoFactoryServices modeloEquipoFactoryServices = new ModeloEquipoFactoryServices();
        private CentroCostosFactoryServices CCs = new CentroCostosFactoryServices();
        private ParosFactoryServices parosServices = new ParosFactoryServices();
        private MaquinaFactoryServices maquinaServices = new MaquinaFactoryServices();
        private CapturaHorometroFactoryServices objCapturaHorometro = new CapturaHorometroFactoryServices();
        private MantenimientoFactoryServices objMantenimientoFactory = new MantenimientoFactoryServices();
        private RitmoHorometroFactoryServices objRitmoTrabajo = new RitmoHorometroFactoryServices();
        private CapturaHorometroFactoryServices horometroServices = new CapturaHorometroFactoryServices();
        private CapturaOTFactoryServices capturaOTFactoryServices = new CapturaOTFactoryServices();
        private RitmoHorometroFactoryServices ritmoHorometroFactoryServices = new RitmoHorometroFactoryServices();
        private ArchivoFactoryServices archivofs = new ArchivoFactoryServices();
        private MaquinaFactoryServices maquinaFactoryServices = new MaquinaFactoryServices();

        private GrupoComponenteModeloFactoryServices grupoComponenteModeloFactoryServices = new GrupoComponenteModeloFactoryServices();
        private PMComponenteLubricanteFactoryServices pmComponenteLubricanteFactoryServices = new PMComponenteLubricanteFactoryServices();
        private PMComponenteFiltroFactoryServices pmComponenteFiltroFactoryServices = new PMComponenteFiltroFactoryServices();
        private readonly string RutaBase = @"\\REPOSITORIO\Proyecto\SIGOPLAN\MAQUINARIA\MANTENIMIENTOPM";
        private const string RutaLocal = @"C:\Proyecto\SIGOPLAN\MAQUINARIA\MANTENIMIENTOPM";
        public PartialViewResult PanelAgrupacionComponentesPMs()
        {
            return PartialView("_AgrupacionComponentesModeloPM");
        }
        public PartialViewResult PanelAgrupacionComponenteFiltroPMs()
        {
            return PartialView("_AgrupacionComponenteFiltroPMs");
        }

        public ActionResult PanelMantenimiento()
        {
            return View();
        }
        public ActionResult CapturaMatenimientoPM()
        {
            if (base.getAction("Desafase") || base.getUsuario().id == 13)
            {
                ViewBag.PermisoDesfase = true;
            }
            else
            {
                ViewBag.PermisoDesfase = false;
            }

            return View();
        }
        public ActionResult FillCombotablaPM()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, objMantenimientoFactory.getMantenimientoService().FillCombotablaPM(0, 0, "").Select(x => new { Value = x.id, Text = x.tipoMantenimiento }).OrderBy(x => x.Value));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ConsultaPM(tblM_MatenimientoPm objMantenimiento)
        {
            var result = new Dictionary<string, object>();
            try
            {
                tblM_CapRitmoHorometro ObjRitmoHorometro = new tblM_CapRitmoHorometro();//ritmo obj
                List<tblM_CapRitmoHorometro> lstObjRitmoHorometro = new List<tblM_CapRitmoHorometro>();//ritmo lst
                List<tblM_MatenimientoPm> lstObjMantenimientoPm = new List<tblM_MatenimientoPm>();//mantenimiento actual
                List<tblM_CatPM> lstObjCatPM = new List<tblM_CatPM>();//catalogo
                lstObjCatPM = objMantenimientoFactory.getMantenimientoService().FillCombotablaPM(0, 0, "");//lista objeto catalogo
                result.Add(ITEMS, objMantenimientoFactory.getMantenimientoService().ConsultarPMActivo());
                foreach (var objMantneimientoPm in lstObjMantenimientoPm)
                {
                    ObjRitmoHorometro = objRitmoTrabajo.RitmoHorometroServices().CapRitmoHorometro(objMantneimientoPm.economicoID);
                    lstObjRitmoHorometro.Add(ObjRitmoHorometro);
                }
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActividadadesbyID(int IDmaquinaria)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, objMantenimientoFactory.getMantenimientoService().ActividadadesbyID(IDmaquinaria));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //Este metodo es el que se encarga de guardar la alta de los PMs
        public ActionResult GuardarPm(tblM_MatenimientoPm objMantenimiento, List<tblM_BitacoraControlAceiteMant> arrJG, List<tblM_BitacoraControlActExt> arrAE, List<tblM_BitacoraControlDN> arrDN)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (objMantenimiento.id == 0)
                {
                    int idMantenimiento = 0;
                    objMantenimiento.fechaCaptura = DateTime.Now;
                    objMantenimiento.UsuarioCap = base.getUsuario().id;
                    objMantenimiento.horometroPMEjecutado = objMantenimiento.horometroPM;
                    objMantenimiento.fechaProy = objMantenimiento.fechaProy.AddHours(5);
                    objMantenimiento.fechaProyFin = objMantenimiento.fechaProy.AddHours(1);

                    idMantenimiento = objMantenimientoFactory.getMantenimientoService().GuardarPM(objMantenimiento).id;
                    if (idMantenimiento != 0)
                    {
                        foreach (var objbitJG in arrJG)
                        {
                            objbitJG.fechaCaptura = DateTime.Now;
                            objbitJG.UsuarioCap = base.getUsuario().id;
                            objbitJG.idMant = idMantenimiento;
                            objMantenimientoFactory.getMantenimientoService().GuardarBitJG(objbitJG);
                        }

                        foreach (var objAE in arrAE)
                        {
                            objAE.fechaCaptura = DateTime.Now;
                            objAE.UsuarioCap = base.getUsuario().id;
                            objAE.idMant = idMantenimiento;
                            objMantenimientoFactory.getMantenimientoService().GuardarBitAE(objAE);
                        }
                        foreach (var objDN in arrDN)
                        {
                            objDN.fechaCaptura = DateTime.Now;
                            objDN.UsuarioCap = base.getUsuario().id;
                            objDN.idMant = idMantenimiento;
                            objMantenimientoFactory.getMantenimientoService().GuardarBitDN(objDN);
                        }


                        var mmto = objMantenimientoFactory.getMantenimientoService().ConsultarPMbyID(idMantenimiento);
                        var economico = maquinaServices.getMaquinaServices().GetMaquinaByID(mmto.idMaquina).FirstOrDefault();



                        var getListaActividadesExtra = objMantenimientoFactory.getMantenimientoService().getActividadesByPM(economico.modeloEquipoID, mmto.tipoPM);

                        foreach (var actividadExtra in getListaActividadesExtra)
                        {

                            tblM_BitacoraActividadesMantProy objtblM_BitacoraActividadesMantProy = new tblM_BitacoraActividadesMantProy();

                            objtblM_BitacoraActividadesMantProy.id = 0;
                            objtblM_BitacoraActividadesMantProy.aplicar = true;
                            objtblM_BitacoraActividadesMantProy.estatus = true;
                            objtblM_BitacoraActividadesMantProy.fechaCaptura = DateTime.Now;
                            objtblM_BitacoraActividadesMantProy.idAct = actividadExtra.idAct;
                            objtblM_BitacoraActividadesMantProy.idMant = mmto.id;
                            objtblM_BitacoraActividadesMantProy.idPm = actividadExtra.idPM;
                            objtblM_BitacoraActividadesMantProy.Observaciones = "";
                            objtblM_BitacoraActividadesMantProy.UsuarioCap = getUsuario().id;
                            objMantenimientoFactory.getMantenimientoService().GuardarActividadProy(objtblM_BitacoraActividadesMantProy);

                            // objMantenimientoFactory.getMantenimientoService().guardarDetAct(arrJG, objtblM_BitacoraActividadesMantProy, mmto.id, economico);


                        }


                    }
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

        public ActionResult ConsultarRitmoAutomatico(string EconomicoID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var ObjRitmoAutomatico = new object();//mantenimiento actual
                ObjRitmoAutomatico = objMantenimientoFactory.getMantenimientoService().ConsultarRitmoAutomatico(EconomicoID);
                result.Add(ITEMS, ObjRitmoAutomatico);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargaDeProyectado(int idmantenimiento)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var objProy = objMantenimientoFactory.getMantenimientoService().CargaDeProyectado(idmantenimiento);
                var objComponentes = objMantenimientoFactory.getMantenimientoService().getCatComponentesViscosidades();


                result.Add(ITEMS, objProy);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CargaDeAEProyectado(int idmantenimiento)
        {
            var result = new Dictionary<string, object>();
            object objProy = new object();
            try
            {
                objProy = objMantenimientoFactory.getMantenimientoService().CargaDeAEProyectado(idmantenimiento);
                result.Add(ITEMS, objProy);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CargaDeDNProyectado(int idmantenimiento)
        {
            var result = new Dictionary<string, object>();
            object objProy = new object();
            try
            {
                objProy = objMantenimientoFactory.getMantenimientoService().CargaDeDNProyectado(idmantenimiento);
                result.Add(ITEMS, objProy);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ConsultaUltHrsByEco(string EconomicoID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var horometro = horometroServices.getCapturaHorometroServices().getUltimoHorometro(EconomicoID);//horometro ultimo
                result.Add(ITEMS, horometro);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ConsultarIDmaquinaria(string EconomicoID)
        {
            var result = new Dictionary<string, object>();
            try
            {

                result.Add(ITEMS, objMantenimientoFactory.getMantenimientoService().ConsultarIDmaquinaria(EconomicoID));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ConsultarUltimoHorometro(string fechaIniMante, string EconomicoID)
        {
            var result = new Dictionary<string, object>();
            try
            {

                result.Add(ITEMS, objMantenimientoFactory.getMantenimientoService().ConsultarUltimoHorometro(fechaIniMante, EconomicoID));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ModificacionFecha(int idMaquina, DateTime FechaUpdate)
        {
            var result = new Dictionary<string, object>();
            try
            {

                result.Add(ITEMS, objMantenimientoFactory.getMantenimientoService().ModificacionFecha(idMaquina, FechaUpdate));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ModificacionHorarioServicio(int idMaquina, DateTime inicio, DateTime fin)
        {
            var result = new Dictionary<string, object>();
            try
            {

                result.Add(ITEMS, objMantenimientoFactory.getMantenimientoService().ModificacionHorarioServicio(idMaquina, fin, inicio));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ConsultarFechaUltimoHorometro(decimal Horometro, string EconomicoID)
        {
            {
                var result = new Dictionary<string, object>();
                try
                {

                    result.Add(ITEMS, objMantenimientoFactory.getMantenimientoService().ConsultarFechaUltimoHorometro(Horometro, EconomicoID));
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
        public ActionResult ConsultarIntervaloFecha(DateTime Fecha, string EconomicoID)
        {
            {
                var result = new Dictionary<string, object>();
                try
                {
                    result.Add(ITEMS, objMantenimientoFactory.getMantenimientoService().ConsultarIntervaloFecha(Fecha, EconomicoID));
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
        public ActionResult getEmpleados(string term)
        {
            var CentroCostosUsuario = GetListaCentroCostos().Select(x => x.cc).ToList()
                ;
            var items = capturaOTFactoryServices.getCapturaOTFactoryServices().getCatEmpleados(term, CentroCostosUsuario);
            var filteredItems = items.Select(x => new { id = x.clave_empleado, label = x.Nombre });
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getEmpleadosID(string id)
        {
            try
            {
                var items = objMantenimientoFactory.getMantenimientoService().getEmpleadosID(id);
                return Json(items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult FillGridActividad(tblM_CatActividadPM obj)
        {
            var result = new Dictionary<string, object>();
            var listResult = objMantenimientoFactory.getMantenimientoService().FillGridActividad(obj);
            result.Add("current", 1);
            result.Add("rowCount", 1);
            result.Add("rows", listResult);
            result.Add(SUCCESS, true);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillGridMiselaneo(int modeloEquipoID, int idAct, int idCompVis, int idTipo)
        {
            var result = new Dictionary<string, object>();
            var listResult = objMantenimientoFactory.getMantenimientoService().FillGridMiselaneo(modeloEquipoID, idAct, idCompVis, idTipo);
            result.Add("current", 1);
            result.Add("rowCount", 1);
            result.Add("rows", listResult);
            result.Add(SUCCESS, true);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillGridComponenteVin(int modeloEquipoID, int idActs, int idTipoAct, int idpm)
        {
            var result = new Dictionary<string, object>();
            var listResult = objMantenimientoFactory.getMantenimientoService().FillGridComponenteVin(modeloEquipoID, idActs, idTipoAct, idpm);
            result.Add("current", 1);
            result.Add("rowCount", 1);
            result.Add("rows", listResult);
            result.Add(SUCCESS, true);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillGridParte(tblM_CatParteVidaUtil obj)
        {
            var result = new Dictionary<string, object>();
            var listResult = objMantenimientoFactory.getMantenimientoService().FillGridParte(obj);
            result.Add("current", 1);
            result.Add("rowCount", 1);
            //result.Add("total", listResult.Count());
            result.Add("rows", listResult);
            result.Add(SUCCESS, true);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCatActividad(string term, int idTipo)
        {
            var result = new Dictionary<string, object>();
            var listResult = objMantenimientoFactory.getMantenimientoService().getCatActividad(term, idTipo);
            var filteredItems = listResult.Select(x => new { id = x.id, label = x.descripcionActividad });
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getTipoActividad(bool estatus)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var a = objMantenimientoFactory.getMantenimientoService().getTipoActividad(estatus).Select(x => new { Value = x.id, Text = x.descripcion });

                result.Add(ITEMS, objMantenimientoFactory.getMantenimientoService().getTipoActividad(estatus).Select(x => new { Value = x.id, Text = x.descripcion }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCatComponente(string term)
        {
            var result = new Dictionary<string, object>();
            var listResult = objMantenimientoFactory.getMantenimientoService().getCatComponente(term);
            var filteredItems = listResult.Select(x => new { id = x.id, label = x.descripcion });
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarNuevaActividad(tblM_CatActividadPM objCatActividadPM)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (objCatActividadPM.id == 0)
                {
                    result.Add("Actividad", objMantenimientoFactory.getMantenimientoService().GuardarNuevaActividad(objCatActividadPM));
                    result.Add(SUCCESS, true);
                }
                else
                {
                    result.Add("Actividad", objMantenimientoFactory.getMantenimientoService().GuardarNuevaActividad(objCatActividadPM));
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
        public ActionResult GuardarParte(tblM_CatParteVidaUtil objCatparte)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (objCatparte.id == 0)
                {
                    result.Add("Actividad", objMantenimientoFactory.getMantenimientoService().GuardarParte(objCatparte));
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
        public ActionResult EliminarVinculacion(tblM_CatPM_CatActividadPM obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add("Actividad", objMantenimientoFactory.getMantenimientoService().ELiminarVinc(obj));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult tipoLeyenda(tblM_CatPM_CatActividadPM obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add("Actividad", objMantenimientoFactory.getMantenimientoService().tipoLeyenda(obj));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ELiminarActividad(tblM_CatActividadPM objCatActividadPM)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add("Actividad", objMantenimientoFactory.getMantenimientoService().ELiminarActividad(objCatActividadPM));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AjustarFullcalendarFront(string objMantenimiento, string objObra)
        {
            var result = new Dictionary<string, object>();


            result.Add("d", objMantenimientoFactory.getMantenimientoService().ConsultarPMActivoByObra(objObra));
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BuscarFicha(string obj)
        {
            var objMaquina = new List<object>();
            RepGastosMaquinaDAO repGastosMaquina = new RepGastosMaquinaDAO();
            var maquina = maquinaServices.getMaquinaServices().GetMaquinaByNoEconomico(obj);
            if (maquina != null)
            {
                var CostoHora = 0; // Total / Convert.ToDouble(horasTotales) == 0 ? 1 : Convert.ToDouble(horasTotales);
                var paro = parosServices.getParosServices().getParosMaquina(maquina.id).First();
                var horometro = horometroServices.getCapturaHorometroServices().getUltimoHorometro(maquina.noEconomico);
                var CC = CCs.getCentroCostosService().getNombreCC(Int32.Parse(maquina.centro_costos));
                var RES = repGastosMaquina.FillInfoGastosMaquinaria(maquina.noEconomico);
                var saldoinicial = "";
                if (RES != null)
                {
                    var gastos = RES.Select(x => new RepGastosMaquinaInfoDTO { depreciacion = Convert.ToDecimal(x.depreciacion).ToString("C2"), descripcion = x.descripcion, fechaAdquisicion = x.fechaAdquisicion, marca = x.marca, modelo = x.modelo, saldoinicial = Convert.ToDecimal(x.saldoinicial).ToString("C2") }).FirstOrDefault();
                    saldoinicial = gastos.saldoinicial;
                }
                var costoOverHaul = repGastosMaquina.valorXoverhaul(obj, maquina.fechaAdquisicion, DateTime.Now);
                var costoOverHaulAplicado = repGastosMaquina.valorXoverhaulAplicadoByMaquina(maquina.noEconomico);
                objMaquina.Add(new
                {
                    noEconomico = maquina.noEconomico,
                    descripcion = maquina.descripcion,
                    marca = maquina.marca.descripcion,
                    modelo = maquina.modeloEquipo.descripcion,
                    noSerie = maquina.noSerie,
                    anio = maquina.anio,
                    fechaCompra = maquina.fechaAdquisicion.ToString("dd/MM/yyyy"),
                    horometroInicio = maquina.horometroAdquisicion,
                    horometroActual = horometro != null ? horometro.Horometro : 0,
                    fechaParo = paro.id > 0 ? paro.fecha_paro.ToString("dd/MM/yyyy") : "",
                    detParo = paro.id > 0 ? paro.descripcion : "",
                    ubicacion = maquina.centro_costos + " - " + CC,
                    costoAdquisicion = saldoinicial,
                    costoOverHaul = Convert.ToDecimal(costoOverHaul).ToString("C2"),
                    costoOverHaulAplicado = Convert.ToDecimal(costoOverHaulAplicado).ToString("C2"),
                    CostoHora = Convert.ToDecimal(CostoHora).ToString("C2")
                });

            }

            else
            {
                objMaquina.Add(new
                {
                    noEconomico = "",
                    descripcion = "",
                    marca = "",
                    modelo = "",
                    noSerie = "",
                    anio = "",
                    fechaCompra = "",
                    horometroInicio = "",
                    horometroActual = "",
                    fechaParo = "",
                    detParo = "",
                    ubicacion = "",
                    costoAdquisicion = "",
                    costoOverHaul = "",
                    costoOverHaulAplicado = ""
                });
            }
            Session["cr" + ReportesEnum.MRepFichaTecnica] = JsonConvert.SerializeObject(objMaquina);
            return Json(objMaquina.First(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCatModelo(string term)
        {
            var result = new Dictionary<string, object>();
            var listResult = objMantenimientoFactory.getMantenimientoService().getCatModelo(term);
            var filteredItems = listResult.Select(x => new { id = x.id, label = x.descripcion });
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getFiltrosMiselaneos(int idModelo, int idmantenimiento)
        {
            var result = new Dictionary<string, object>();
            try
            {

                bool programa = true;
                var objMntto = objMantenimientoFactory.getMantenimientoService().ConsultarPMbyID(idmantenimiento);
                if (objMntto != null)
                {
                    if (objMntto.tipoPM == 1 || objMntto.tipoPM == 3 || objMntto.tipoPM == 5 || objMntto.tipoPM == 7)
                    {
                        programa = false;
                    }
                }
                var resultData = objMantenimientoFactory.getMantenimientoService().getDetActividadesMantProy(idModelo).ToList().Distinct();

                var DatosRep = resultData.GroupBy(x => new { x.componente, x.idAct, x.idCompVis }).Distinct().Select(y => new objMiselaneosDTO
                    {
                        componente = y.Key.componente,
                        cantidad = "",
                        chkProgramado = "",
                        modeloEquipoID = idModelo,
                        idCompVis = y.Key.idCompVis,
                        modelo = 0,
                        idFiltro = 0,
                        aplicar = false,
                        programado = programa,
                        idMant = 0,
                        tipoPMid = 0

                    }).ToList().Distinct().ToList();

                result.Add("resultData", resultData);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// obtiene las actividades del modelo.
        /// </summary>
        /// <param name="idModelo"></param>
        /// <param name="idtipo"></param>
        /// <returns></returns>
        public ActionResult getActividadModelo33(int idModelo, int idtipo)
        {
            DateTime fecha = DateTime.Now;
            var result = new Dictionary<string, object>();
            var listResult = objMantenimientoFactory.getMantenimientoService().getActividadModelo(idModelo);
            result.Add("rows", listResult);
            result.Add(SUCCESS, true);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getActividadModelo(int idModelo, int idtipo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var listResult = objMantenimientoFactory.getMantenimientoService().getActividadModelo(idModelo);
                result.Add("rows", listResult);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VinculaNuevaActividad(tblM_CatPM_CatActividadPM obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (obj.id == 0 && obj.idAct != 0)
                {
                    bool existeRelacion = false;
                    existeRelacion = objMantenimientoFactory.getMantenimientoService().ActividadExistente(obj);
                    if (existeRelacion == false)
                    {
                        obj.UsuarioCap = base.getUsuario().id;
                        obj.fechaCaptura = DateTime.Now;
                        result.Add("Actividad", objMantenimientoFactory.getMantenimientoService().VinculaNuevaActividad(obj));
                        result.Add(SUCCESS, true);
                    }
                    else
                    {
                        result.Add("Existente", existeRelacion);
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
        public ActionResult VincularEdadMis(int id, int edad)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (id != 0 && edad != 0)
                {
                    result.Add("Mis", objMantenimientoFactory.getMantenimientoService().VincularEdadMis(id, edad));
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
        public ActionResult VincularCantidadMis(int id, int cantidad)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (id != 0 && cantidad != 0)
                {
                    result.Add("Mis", objMantenimientoFactory.getMantenimientoService().VincularCantidadMis(id, cantidad));
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
        public ActionResult VincularEdadAct(int id, int edad)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (id != 0 && edad != 0)
                {
                    result.Add("Mis", objMantenimientoFactory.getMantenimientoService().VincularEdadAct(id, edad));
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
        public ActionResult VincularMis(tblM_MiscelaneoMantenimiento obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                obj.UsuarioCap = base.getUsuario().id;
                obj.fechaCaptura = DateTime.Now;
                result.Add("Mis", objMantenimientoFactory.getMantenimientoService().VincularMis(obj));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EliminarVincDoc(int idActividad, int modelo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (idActividad != 0 && modelo != 0)
                {
                    objMantenimientoFactory.getMantenimientoService().EliminarVincDoc(idActividad, modelo);
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
        public ActionResult FillCboTipo_Maquina(bool estatus)
        {
            //seteado para solo tipo mayor
            var result = new Dictionary<string, object>();
            try
            {
                object objTipoMaquinaria = new object();
                result.Add(ITEMS, objMantenimientoFactory.getMantenimientoService().FillCboTipoMaquinaria(estatus)/*.Where(y => y.id == 1)*/.Select(x => new { Value = x.id, Text = x.descripcion }).ToList());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillCboModelo_Maquina(int idTipo)
        {
            //seteado para solo tipo mayor
            var result = new Dictionary<string, object>();
            try
            {
                object objTipoMaquinaria = new object();
                result.Add(ITEMS, objMantenimientoFactory.getMantenimientoService().FillCboModelo_Maquina(idTipo).Select(x => new { Value = x.id, Text = x.descripcion }));
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
                //santiago
                result.Add(ITEMS, objMantenimientoFactory.getMantenimientoService().FillCboEconomicos(idGrupo, getUsuario().id).Select(x => new { Value = x.id, Text = x.noEconomico }).OrderBy(x => x.Text).Distinct());
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
        public ActionResult SaveOrUpdate()
        {
            HttpPostedFileBase file1 = Request.Files["fFormato"];
            string FileName = "";
            string ruta = "";
            var idAct = JsonConvert.DeserializeObject<int>(Request.Form["idActividad"]);
            var modelo = JsonConvert.DeserializeObject<int>(Request.Form["modelo"]);
            var idPM = JsonConvert.DeserializeObject<int>(Request.Form["idPM"]);
            var idDN = JsonConvert.DeserializeObject<int>(Request.Form["idDN"]);
            var idCatTipoActividad = JsonConvert.DeserializeObject<int>(Request.Form["idCatTipoActividad"]);
            //var tipo = JsonConvert.DeserializeObject<int>(Request.Form["tipo"]);
            var result = new Dictionary<string, object>();
            bool pathExist = false;
            try
            {
                DateTime fecha = DateTime.Now;
                string f = fecha.ToString("ddMMyyyy") + fecha.Hour + "" + fecha.Minute;
                if (file1 != null && file1.ContentLength > 0)
                {
                    FileName = file1.FileName;
                    //ruta = @"C:\Users\raguilar\Desktop" + f + FileName;
                    //ruta = @"C:\Proyecto\SIGOPLAN\MAQUINARIA\MANTENIMIENTO\" + f + FileName;
                    ruta = archivofs.getArchivo().getUrlDelServidor(5) + f + FileName;
                    //C:\Proyecto\SIGOPLAN\MAQUINARIA\MANTENIMIENTO
                    //C:\Proyectos\deploy\docPruebaMant
                    pathExist = GuardarDocumentos(file1, ruta);
                    int idDocGuardado = 0;
                    if (pathExist)
                    {
                        tblM_DocumentosMaquinaria objDoc = new tblM_DocumentosMaquinaria();
                        objDoc.nombreArchivo = FileName;
                        objDoc.nombreRuta = ruta;
                        objDoc.tipoArchivo = 10;
                        objDoc.economicoID = modelo;
                        objDoc.usuarioSubeArchivo = getUsuario().id;
                        objDoc.fechaCarga = DateTime.Now;
                        idDocGuardado = objMantenimientoFactory.getMantenimientoService().GuardarDoc(objDoc);
                        //guardar info extra  validar la referencia para eliminar o actualizar 
                        tblM_FormatoManteniento objDocFormato = new tblM_FormatoManteniento();
                        objDocFormato.idAct = idAct;
                        objDocFormato.DocumentosMaquinariaID = idDocGuardado;
                        objDocFormato.modeloEquipoID = modelo;
                        objDocFormato.idPM = idPM;
                        objDocFormato.idCatTipoActividad = idCatTipoActividad;
                        objDocFormato.estado = true;
                        objDocFormato.idDN = idDN;
                        objMantenimientoFactory.getMantenimientoService().GuardarDocFormat(objDocFormato);
                        result.Add(ITEMS, idDocGuardado);//guardar formato
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
        private bool GuardarDocumentos(HttpPostedFileBase archivo, string ruta)
        {
            bool result = false;
            byte[] data;
            using (Stream inputStream = archivo.InputStream)
            {
                MemoryStream memoryStream = inputStream as MemoryStream;
                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }
                data = memoryStream.ToArray();
            }
            ruta = ruta.Replace("C:\\", "\\REPOSITORIO\\");
            System.IO.File.WriteAllBytes(ruta, data);
            result = System.IO.File.Exists(ruta);
            return result;
        }
        public FileResult getFileDownload(int id)
        {
            var a = id;
            var archivo = objMantenimientoFactory.getMantenimientoService().GetObjRutaDocumentobyID(id);
            var nombre = " ";
            var Ruta = " ";
            if (archivo != null)
            {
                nombre = archivo.nombreArchivo;
                Ruta = archivo.nombreRuta;
            }
            return File(Ruta, "multipart/form-data", nombre);
        }
        public ActionResult VincularComponete(tblM_ComponenteMantenimiento objVincularComponete)
        {
            var result = new Dictionary<string, object>();
            try
            {
                //if (objVincularComponete.id == 0 && objVincularComponete.estado == true)
                if (objVincularComponete.id == 0)//modificacion desvinculado 22/05/18
                {
                    objVincularComponete.UsuarioCap = base.getUsuario().id;//agragado de usuario id
                    objVincularComponete.fechaCaptura = DateTime.Now;
                    result.Add("Actividad", objMantenimientoFactory.getMantenimientoService().VincularComponete(objVincularComponete));
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
        public ActionResult ConsultarJGEstructura(int modeloEquipoID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = objMantenimientoFactory.getMantenimientoService().ConsultarJGEstructura(modeloEquipoID);
                if (obj.Count() == 0)
                {
                    var modelo = modeloEquipoFactoryServices.getModeloEquipoService().getModeloByID(modeloEquipoID);
                    result.Add("modelo", modelo.descripcion);
                    result.Add(SUCCESS, false);
                }
                else
                {
                    result.Add("Actividad", obj);
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
        /*Matus Carga de Estructura de Lubricantes*/
        public ActionResult ConsultaInfoLubricantesAlta(int modeloEquipoID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var dataSet = pmComponenteLubricanteFactoryServices.getPMComponenteLubricanteFactoryServices()
                                .tblComponenteLubricante(modeloEquipoID);


                result.Add("dataSet", dataSet);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult loadTblEconomicosPMs(string areaCuenta)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var listaResult = objMantenimientoFactory.getMantenimientoService().getListaEquiposAC(areaCuenta);


                result.Add("dataSet", listaResult.Select(x => new
                {

                    id = x.id,
                    Economico = x.economicoID,
                    economicoID = x.idMaquina,
                    horometro = horometroServices.getCapturaHorometroServices().GetHorometroFinal(x.economicoID),
                    proximoPM = x.fechaProy.ToShortDateString(),
                    horometroPM = x.horometroProy,
                    btnViewLub = ""
                }).ToList());
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public ActionResult fnSetResetInfo(int idHistorico, int idProyectado, decimal idHorometro, int idMant)
        {

            var result = new Dictionary<string, object>();
            try
            {

                if (idProyectado != 0)
                {
                    var proyectadoLubricante = objMantenimientoFactory.getMantenimientoService().getBitProyLubByid(idProyectado);

                    if (proyectadoLubricante != null)
                    {
                        proyectadoLubricante.Hrsaplico = idHorometro;
                        objMantenimientoFactory.getMantenimientoService().GuardarBitProyLub(proyectadoLubricante);
                    }

                }

                if (idHistorico != 0)
                {
                    var historicoLubricante = objMantenimientoFactory.getMantenimientoService().getBitHisLubByid(idHistorico);

                    if (historicoLubricante != null)
                    {
                        historicoLubricante.Hrsaplico = idHorometro;
                        historicoLubricante.vidaActual = 0;
                        historicoLubricante.VidaRestante = historicoLubricante.Vigencia;
                        objMantenimientoFactory.getMantenimientoService().GuardarBitJG(historicoLubricante);
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


        /*/*/
        public ActionResult getLubricantesRest(int modeloEquipoID, int idMantenimiento)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var dataSet = objMantenimientoFactory.getMantenimientoService().getBitacoraLubricantesByMant(idMantenimiento);

                result.Add("dataSet", dataSet);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult setResetInformacion(List<objRestLubricantesDTO> objs, int idMantenimiento)
        {
            var result = new Dictionary<string, object>();
            try
            {
                objMantenimientoFactory.getMantenimientoService().saveInfoResetLubricantes(objs);
                var dataSet = objMantenimientoFactory.getMantenimientoService().getBitacoraLubricantesByMant(idMantenimiento);

                result.Add("dataSet", dataSet);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ConsultarJGEstructuraLubricantes(int modeloEquipoID, int idmantenimiento)
        {
            var result = new Dictionary<string, object>();
            //try
            //{
            if (modeloEquipoID == 0)
            {
                var objesd = objMantenimientoFactory.getMantenimientoService().ConsultarMantenimientobyID(idmantenimiento);

                string economico = objesd.economico;
                var maquinaria = maquinaFactoryServices.getMaquinaServices().getEconomicoIDNo(economico);
                modeloEquipoID = maquinaria.modeloEquipoID;

            }

            var obj = objMantenimientoFactory.getMantenimientoService().ConsultarJGEstructura2(modeloEquipoID);
            var objHist = objMantenimientoFactory.getMantenimientoService().ConsultarJGHis(idmantenimiento);

            var objProy = objMantenimientoFactory.getMantenimientoService().CargaDeProyectado(idmantenimiento);
            var objComponentes = objMantenimientoFactory.getMantenimientoService().getCatComponentesViscosidades();


            var objSuministros = obj.Select(x => x.aceiteDTO.Select(y => y.edadSuministro).Select(f => f));

            List<dataSetGridLubProxDTO> dataSetGridLubProx = new List<dataSetGridLubProxDTO>();
            //    dataSetGridLubProx = obj.Select(x => ).Where(x => x.objHis != null).ToList();


            foreach (var x in obj)
            {

                var DataTemp = GetBitacoraControlAceiteMantProy(x, objProy, objHist, idmantenimiento);

                if (DataTemp != null)
                {
                    dataSetGridLubProxDTO dataObj = new dataSetGridLubProxDTO
                    {
                        objComponente = x.componenteMantenimiento,
                        componente = x.descripcion,
                        Suministros = x.aceiteDTO.Where(y => y.componenteID == x.componenteMantenimiento.idCompVis).ToList(),
                        TipoPrueba = "",
                        VidaUtil = "",
                        Info = "",
                        VidaConsumida = "",
                        VidaRestante = "",
                        Programar = "",
                        objHis = objHist.FirstOrDefault(oh => oh.idComp == x.componenteMantenimiento.idCompVis) != null ? objHist.FirstOrDefault(oh => oh.idComp == x.componenteMantenimiento.idCompVis) : null,
                        proyectado = GetBitacoraControlAceiteMantProy(x, objProy, objHist, idmantenimiento),
                        idComponente = x.aceiteDTO.FirstOrDefault() != null ? x.aceiteDTO.FirstOrDefault().componenteID : 0,
                        idmantenimiento = idmantenimiento
                    };

                    /*if (dataObj.objHis != null) {*/
                    dataSetGridLubProx.Add(dataObj); //}
                }
            }

            result.Add("dataSetGridLubProx", dataSetGridLubProx);

            result.Add("JGHis", objHist);

            if (obj.Count() == 0)
            {
                result.Add(SUCCESS, false);
            }
            else
            {
                result.Add("Actividad", obj);
                result.Add(SUCCESS, true);
            }
            //}
            //catch (Exception e)
            //{
            //    result.Add(MESSAGE, e.Message);
            //    result.Add(SUCCESS, false);
            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private tblM_BitacoraControlAceiteMantProy GetBitacoraControlAceiteMantProy(dataSetLubProxDTO x, List<tblM_BitacoraControlAceiteMantProy> objProy, List<JGHisDTO> objHist, int idmantenimiento)
        {
            var obj = objHist.FirstOrDefault(oh => oh.idComp == x.componenteMantenimiento.idCompVis) != null ? objHist.FirstOrDefault(oh => oh.idComp == x.componenteMantenimiento.idCompVis) : null;

            if (obj != null && objProy.Count > 0)
                return objProy.FirstOrDefault(p => p.idComp == x.componenteMantenimiento.idCompVis);
            else
            {
                if (x.aceiteDTO.FirstOrDefault(r => r.componenteID == x.componenteMantenimiento.idCompVis) != null)
                {
                    return new tblM_BitacoraControlAceiteMantProy
                    {
                        aplicado = false,
                        estatus = true,
                        fechaCaptura = DateTime.Now,
                        FechaServicio = DateTime.Now,
                        Hrsaplico = obj != null ? obj.hrsAplico : 0,
                        id = 0,
                        idAct = 0,
                        idComp = x.componenteMantenimiento.idCompVis,
                        idMant = idmantenimiento,
                        idMisc = x.aceiteDTO.FirstOrDefault(r => r.componenteID == x.componenteMantenimiento.idCompVis).suministroID,
                        Observaciones = "",
                        programado = false,
                        prueba = false,
                        UsuarioCap = getUsuario().id,
                        Vigencia = obj != null ? obj.vidaA : 0

                    };
                }
                else
                {
                    return null;
                }

            }
        }

        public ActionResult ConsultarJGHis(int idMantenimiento)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (idMantenimiento != 0)
                {
                    var obj = objMantenimientoFactory.getMantenimientoService().ConsultarJGHis(idMantenimiento);
                    result.Add("JGHis", obj);
                }
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ConsultarActividadesExtrashis(int idMantenimiento)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (idMantenimiento != 0)
                {

                    /// Pestaña2
                    var obj = objMantenimientoFactory.getMantenimientoService().ConsultarActividadesExtrashis(idMantenimiento);

                    result.Add("ActHis", obj);
                }
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ConsultarActividadesExtras(int modeloEquipoID)
        {
            var result = new Dictionary<string, object>();
            try
            {

                ////Pestaña 2
                result.Add("Actividad", objMantenimientoFactory.getMantenimientoService().ConsultarActividadesExtras(modeloEquipoID));
                var modelo = modeloEquipoFactoryServices.getModeloEquipoService().getModeloByID(modeloEquipoID);
                result.Add("modelo", modelo == null ? "N/A" : modelo.descripcion);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetModeloEconomico(int idNoEconomico)
        {
            return Json(objMantenimientoFactory.getMantenimientoService().GetModeloEconomico(idNoEconomico), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getActividadesExtras(int modeloEquipoID, int idMantenimiento)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var objActividadesExtras = objMantenimientoFactory.getMantenimientoService().ConsultarActividadesExtras(modeloEquipoID);
                var objActividadesExtrashis = objMantenimientoFactory.getMantenimientoService().ConsultarActividadesExtrashis(idMantenimiento);
                var objProy = objMantenimientoFactory.getMantenimientoService().CargaDeAEProyectado(idMantenimiento);

                result.Add("objActividadesExtras", objActividadesExtras.Select(x => new
                {
                    actividad = x.descripcion,
                    vidaUtil = x.perioricidad,
                    info = "",
                    vidaConsumida = "",
                    vidaRestante = "",
                    programar = false,
                    x.Componente,
                    x.descripcion,
                    x.id,
                    x.idAct,
                    x.idformato,
                    x.idTipo,
                    x.leyenda,
                    x.orden,
                    x.perioricidad,
                    x.PM,
                    x.Tipo,
                    hrsAplico = objActividadesExtrashis.FirstOrDefault(y => y.actividad.Equals(x.descripcion)) != null ? objActividadesExtrashis.FirstOrDefault(y => y.actividad.Equals(x.descripcion)).Hrsaplico : 0,
                    proyectado = GetactividadesExtraHisDTO(x, objActividadesExtrashis, objProy, idMantenimiento),
                    idMant = idMantenimiento,
                    UsuarioCap = getUsuario().id,
                    fechaCaptura = DateTime.Now.ToShortDateString()

                }));

                result.Add("objActividadesExtrashis", objActividadesExtrashis);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private tblM_BitacoraControlAEMantProy GetactividadesExtraHisDTO(actividadesExtraDTO x, List<actividadesExtraHisDTO> objActividadesExtrashis, List<tblM_BitacoraControlAEMantProy> objProy, int idMantenimiento)
        {
            if (objProy.Count != 0)
                return objProy.FirstOrDefault(y => y.idAct == x.idAct);
            else
                return new tblM_BitacoraControlAEMantProy
                {
                    aplicado = false,
                    estatus = true,
                    fechaCaptura = DateTime.Now,
                    FechaServicio = DateTime.Now,
                    Hrsaplico = objActividadesExtrashis.FirstOrDefault(y => y.actividad.Equals(x.descripcion)) != null ? objActividadesExtrashis.FirstOrDefault(y => y.actividad.Equals(x.descripcion)).Hrsaplico : 0,
                    id = 0,
                    idAct = x.idAct,
                    idMant = idMantenimiento,
                    Observaciones = "",
                    programado = false,
                    UsuarioCap = getUsuario().id,
                    Vigencia = objActividadesExtrashis.FirstOrDefault(y => y.actividad.Equals(x.descripcion)) != null ? objActividadesExtrashis.FirstOrDefault(y => y.actividad.Equals(x.descripcion)).perioricidad : 0,
                };

        }
        public ActionResult GuardarDocumentoPM(HttpPostedFileBase objFile)
        {
            tblM_DocumentoMantenimientoPM objDocumentoPM = JsonUtils.convertJsonToNetObject<tblM_DocumentoMantenimientoPM>(Request.Form["objDocumentoPM"], "es-MX");
            return Json(objMantenimientoFactory.getMantenimientoService().GuardarDocumentoPM(objDocumentoPM, objFile), JsonRequestBehavior.AllowGet);
        }
        public ActionResult DescargarArchivo(int idArchivo)
        {
            var resultadoTupla = objMantenimientoFactory.getMantenimientoService().descargarArchivo(idArchivo);

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
        public ActionResult GetArchivosAdjuntos(int idArchivo)
        {
            return Json(objMantenimientoFactory.getMantenimientoService().GetArchivosAdjuntos(idArchivo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult sendInfoSave(int idMantenimiento, List<tblM_BitacoraControlAceiteMantProy> tblGridLubProxTbl, List<tblM_BitacoraControlAEMantProy> tblGridActProxTbl, List<tblM_BitacoraControlDNMantProy> tblgridDNProxTbl, int tipoGuardardo, List<tblM_BitacoraDetActividadesMantProy> tlbDetACtividades, int planeador = 0, int responsable = 0)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var mmto = objMantenimientoFactory.getMantenimientoService().ConsultarPMbyID(idMantenimiento);

                if (tblGridLubProxTbl != null)
                {
                    foreach (var ObjBitProyLub in tblGridLubProxTbl)
                    {
                        try
                        {
                            ObjBitProyLub.idMant = idMantenimiento;
                            ObjBitProyLub.fechaCaptura = DateTime.Now;
                            ObjBitProyLub.FechaServicio = DateTime.Now;
                            ObjBitProyLub.UsuarioCap = getUsuario().id;
                            ObjBitProyLub.estatus = true;
                            //ObjBitProyLub.Hrsaplico = ObjBitProyLub.programado ? mmto.horometroProy : ObjBitProyLub.Hrsaplico;
                            objMantenimientoFactory.getMantenimientoService().GuardarBitProyLub(ObjBitProyLub);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }

                if (tlbDetACtividades != null)
                {
                    foreach (var ObjDet in tlbDetACtividades)
                    {
                        try
                        {
                            ObjDet.idMant = idMantenimiento;
                            objMantenimientoFactory.getMantenimientoService().GuardarBitacoraDetActividadesMantProy(ObjDet);
                        }
                        catch (Exception)
                        {
                        }

                    }
                }

                if (tblGridActProxTbl != null)
                {
                    foreach (var ObjBitProyAE in tblGridActProxTbl)
                    {
                        try
                        {
                            ObjBitProyAE.idMant = idMantenimiento;
                            ObjBitProyAE.FechaServicio = mmto.fechaPM;
                            objMantenimientoFactory.getMantenimientoService().GuardarBitProyAE(ObjBitProyAE);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }

                if (tblgridDNProxTbl != null)
                {
                    foreach (var ObjBitProyDN in tblgridDNProxTbl)
                    {
                        try
                        {
                            ObjBitProyDN.idMant = idMantenimiento;
                            ObjBitProyDN.UsuarioCap = getUsuario().id;
                            ObjBitProyDN.fechaCaptura = DateTime.Now;
                            ObjBitProyDN.FechaServicio = mmto.fechaPM;
                            objMantenimientoFactory.getMantenimientoService().GuardarBitProyDN(ObjBitProyDN);
                        }
                        catch (Exception)
                        {


                        }

                    }
                }

                if (tipoGuardardo == 2)
                {
                    mmto.estadoMantenimiento = 2;
                    var bools = objMantenimientoFactory.getMantenimientoService().GuardarPM(mmto);
                }
                #region Edicion
                if (vSesiones.sesionEmpresaActual == 6)
                {
                    mmto.estadoMantenimiento = 2;
                    if (planeador > 0)
                    {
                        mmto.planeador = planeador;
                    }
                    else
                    {
                        mmto.planeador = mmto.planeador;
                    }

                    if (responsable >0)
                    {
                        mmto.personalRealizo = responsable;
                    }
                    else
                    {
                        mmto.personalRealizo = mmto.personalRealizo;
                    }
                 
               
                    var bools = objMantenimientoFactory.getMantenimientoService().GuardarPM(mmto);
                }
                #endregion

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult getActividadesDNS(int modeloEquipoID, int idMantenimiento)
        {
            var result = new Dictionary<string, object>();
            var idPM = 0;
            var mantenimiento = objMantenimientoFactory.getMantenimientoService().ConsultarPMbyID(idMantenimiento);
            if (mantenimiento != null)
            {
                switch (mantenimiento.tipoMantenimientoProy)
                {
                    case 1:
                    case 3:
                    case 5:
                    case 7: idPM = 1; break;
                    case 2:
                    case 6: idPM = 2; break;
                    case 4: idPM = 3; break;
                    case 8: idPM = 4; break;
                }
            }
            try
            {
                var objActividadesDN = objMantenimientoFactory.getMantenimientoService().ConsultarActividadesDN(modeloEquipoID, idPM);
                var objActividadesDNHis = objMantenimientoFactory.getMantenimientoService().ConsultarActividadesDNhis(idMantenimiento);
                var objDNProyectado = objMantenimientoFactory.getMantenimientoService().CargaDeDNProyectado(idMantenimiento);
                result.Add("objActividadesDN",
                    objActividadesDN.Select(x => new
                    {
                        id = x.id,
                        actividad = x.descripcion,
                        idAct = x.idAct,
                        idMant = idMantenimiento,

                        vidaUtil = x.perioricidad,
                        info = "",
                        vidaConsumida = "",
                        vidaRestante = "",
                        programar = "",
                        proyectado = setActividadesDNDTO(x, objDNProyectado, idMantenimiento, objActividadesDNHis),
                        Hrsaplico = objActividadesDNHis.FirstOrDefault(f => f.actividad.Equals(x.descripcion)) != null ? objActividadesDNHis.FirstOrDefault(f => f.actividad.Equals(x.descripcion)).Hrsaplico : 0
                    }));
                result.Add("objActividadesDNHis", objActividadesDNHis);


                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private tblM_BitacoraControlDNMantProy setActividadesDNDTO(ActividadesDNDTO x, List<tblM_BitacoraControlDNMantProy> objDNProyectado, int idMantenimiento, List<ActividadesDNHisDTO> objActividadesDNHis)
        {
            var data = objActividadesDNHis.FirstOrDefault(f => f.actividad.Equals(x.descripcion)) != null ? objActividadesDNHis.FirstOrDefault(f => f.actividad.Equals(x.descripcion)) : new ActividadesDNHisDTO();
            if (objDNProyectado.FirstOrDefault(y => y.idAct == x.idAct) != null)
                return objDNProyectado.FirstOrDefault(y => y.idAct == x.idAct);
            else
                return new tblM_BitacoraControlDNMantProy
                {
                    id = 0,
                    aplicado = false,
                    estatus = true,
                    fechaCaptura = DateTime.Now,
                    FechaServicio = DateTime.Now,
                    Hrsaplico = data != null ? data.Hrsaplico : 0,
                    idAct = x.idAct,
                    idMant = idMantenimiento,
                    Observaciones = "",
                    programado = false,
                    UsuarioCap = getUsuario().id,
                    Vigencia = data != null ? data.perioricidad : 0,
                };
        }

        public ActionResult ConsultarActividadesDN(int modeloEquipoID, int idPM = 1)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add("Actividad", objMantenimientoFactory.getMantenimientoService().ConsultarActividadesDN(modeloEquipoID, idPM));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ConsultarActividadesDNhis(int idMantenimiento)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add("Actividad", objMantenimientoFactory.getMantenimientoService().ConsultarActividadesDNhis(idMantenimiento));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarDatosAltaPM(string noEconmico, string fechaIniMante)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var modelo = objMantenimientoFactory.getMantenimientoService().ConsultarModelo(noEconmico);
                result.Add("modelo", modelo);
                tblM_CapHorometro horometro = objMantenimientoFactory.getMantenimientoService().ConsultarUltimoHorometro(fechaIniMante, noEconmico);
                result.Add("horometro", horometro);
                var dataSet = pmComponenteLubricanteFactoryServices.getPMComponenteLubricanteFactoryServices().tblComponenteLubricante(modelo);
                result.Add("dataSet", dataSet);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ConsultarModelo(string noEconmico)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add("Actividad", objMantenimientoFactory.getMantenimientoService().ConsultarModelo(noEconmico));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConsultarModeloEquipoPM(int idMantenimiento)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var mmto = objMantenimientoFactory.getMantenimientoService().ConsultarPMbyID(idMantenimiento);

                result.Add("Actividad", objMantenimientoFactory.getMantenimientoService().ConsultarModelo(mmto.economicoID));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ConsultarBitacora(string economicoiD)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add("Actividad", objMantenimientoFactory.getMantenimientoService().ConsultarBitacora(economicoiD));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ProgramaActividades(int idmantenimiento)
        {
            var result = new Dictionary<string, object>();
            try
            {
                objMantenimientoFactory.getMantenimientoService().ProgramaActividades(idmantenimiento);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RenderizadoServicio(int idmantenimiento, string EconomicoID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var objResult = objMantenimientoFactory.getMantenimientoService().ConsultarPMbyID(idmantenimiento);


                if (objResult.estadoMantenimiento <= 3)
                {
                    var objCoodinadorPM = objMantenimientoFactory.getMantenimientoService().ConsultaPersonalIdManteniminto(objResult.planeador);
                    var objResponsablePM = objMantenimientoFactory.getMantenimientoService().ConsultaPersonalIdManteniminto(objResult.personalRealizo);
                    var maquina = maquinaFactoryServices.getMaquinaServices().GetMaquinaByNoEconomico(EconomicoID);

                    result.Add("Mantenimiento", objResult);
                    result.Add("maquina", maquina);
                    if (objCoodinadorPM != null)
                        result.Add("CoodinadorPM", objCoodinadorPM);
                    if (objResponsablePM != null)
                        result.Add("ResponsablePM", objResponsablePM);

                    //-->
                    var modeloEquipoID = maquina.modeloEquipoID;
                    if (modeloEquipoID == 0)
                    {
                        var objesd = objMantenimientoFactory.getMantenimientoService().ConsultarMantenimientobyID(idmantenimiento);
                        string economico = objesd.economico;
                        var maquinaria = maquinaFactoryServices.getMaquinaServices().getEconomicoIDNo(economico);
                        modeloEquipoID = maquinaria.modeloEquipoID;
                    }
                    var obj = objMantenimientoFactory.getMantenimientoService().ConsultarJGEstructura2(modeloEquipoID);
                    var objHist = objMantenimientoFactory.getMantenimientoService().ConsultarJGHis(idmantenimiento);
                    var objProy = objMantenimientoFactory.getMantenimientoService().CargaDeProyectado(idmantenimiento);
                    var objComponentes = objMantenimientoFactory.getMantenimientoService().getCatComponentesViscosidades();
                    var objSuministros = obj.Select(x => x.aceiteDTO.Select(y => y.edadSuministro).Select(f => f));
                    List<dataSetGridLubProxDTO> dataSetGridLubProx = new List<dataSetGridLubProxDTO>();
                    foreach (var x in obj)
                    {
                        var DataTemp = GetBitacoraControlAceiteMantProy(x, objProy, objHist, idmantenimiento);
                        if (DataTemp != null)
                        {
                            dataSetGridLubProxDTO dataObj = new dataSetGridLubProxDTO
                            {
                                objComponente = x.componenteMantenimiento,
                                componente = x.descripcion,
                                Suministros = x.aceiteDTO.Where(y => y.componenteID == x.componenteMantenimiento.idCompVis).ToList(),
                                TipoPrueba = "",
                                VidaUtil = "",
                                Info = "",
                                VidaConsumida = "",
                                VidaRestante = "",
                                Programar = "",
                                objHis = objHist.FirstOrDefault(oh => oh.idComp == x.componenteMantenimiento.idCompVis) != null ? objHist.FirstOrDefault(oh => oh.idComp == x.componenteMantenimiento.idCompVis) : null,
                                proyectado = GetBitacoraControlAceiteMantProy(x, objProy, objHist, idmantenimiento),
                                idComponente = x.aceiteDTO.FirstOrDefault() != null ? x.aceiteDTO.FirstOrDefault().componenteID : 0,
                                idmantenimiento = idmantenimiento
                            };
                            dataSetGridLubProx.Add(dataObj);
                        }
                    }
                    result.Add("dataSetGridLubProx", dataSetGridLubProx);
                    result.Add("JGHis", objHist);
                    if (obj.Count() != 0) { result.Add("Actividad", obj); }
                    //<--
                    //--> getActividadesExtras
                    var objActividadesExtras = objMantenimientoFactory.getMantenimientoService().ConsultarActividadesExtras(modeloEquipoID);
                    var objActividadesExtrashis = objMantenimientoFactory.getMantenimientoService().ConsultarActividadesExtrashis(idmantenimiento);
                    var objProyAE = objMantenimientoFactory.getMantenimientoService().CargaDeAEProyectado(idmantenimiento);
                    result.Add("objActividadesExtras", objActividadesExtras.Select(x => new
                    {
                        actividad = x.descripcion,
                        vidaUtil = x.perioricidad,
                        info = "",
                        vidaConsumida = "",
                        vidaRestante = "",
                        programar = false,
                        x.Componente,
                        x.descripcion,
                        x.id,
                        x.idAct,
                        x.idformato,
                        x.idTipo,
                        x.leyenda,
                        x.orden,
                        x.perioricidad,
                        x.PM,
                        x.Tipo,
                        hrsAplico = objActividadesExtrashis.FirstOrDefault(y => y.actividad.Equals(x.descripcion)) != null ? objActividadesExtrashis.FirstOrDefault(y => y.actividad.Equals(x.descripcion)).Hrsaplico : 0,
                        proyectado = GetactividadesExtraHisDTO(x, objActividadesExtrashis, objProyAE, idmantenimiento),
                        idMant = idmantenimiento,
                        UsuarioCap = getUsuario().id,
                        fechaCaptura = DateTime.Now.ToShortDateString()
                    }));

                    result.Add("objActividadesExtrashis", objActividadesExtrashis);
                    //<--
                    //-->GetActividadesDN
                    var idPM = 0;
                    if (objResult != null)
                    {
                        switch (objResult.tipoMantenimientoProy)
                        {
                            case 1:
                            case 3:
                            case 5:
                            case 7: idPM = 1; break;
                            case 2:
                            case 6: idPM = 2; break;
                            case 4: idPM = 3; break;
                            case 8: idPM = 4; break;
                        }
                    }
                    var objActividadesDN = objMantenimientoFactory.getMantenimientoService().ConsultarActividadesDN(modeloEquipoID, idPM);
                    var objActividadesDNHis = objMantenimientoFactory.getMantenimientoService().ConsultarActividadesDNhis(idmantenimiento);
                    var objDNProyectado = objMantenimientoFactory.getMantenimientoService().CargaDeDNProyectado(idmantenimiento);
                    result.Add("objActividadesDN",
                        objActividadesDN.Select(x => new
                        {
                            id = x.id,
                            actividad = x.descripcion,
                            idAct = x.idAct,
                            idMant = idmantenimiento,
                            vidaUtil = x.perioricidad,
                            info = "",
                            vidaConsumida = "",
                            vidaRestante = "",
                            programar = "",
                            proyectado = setActividadesDNDTO(x, objDNProyectado, idmantenimiento, objActividadesDNHis),
                            Hrsaplico = objActividadesDNHis.FirstOrDefault(f => f.actividad.Equals(x.descripcion)) != null ? objActividadesDNHis.FirstOrDefault(f => f.actividad.Equals(x.descripcion)).Hrsaplico : 0
                        }));
                    result.Add("objActividadesDNHis", objActividadesDNHis);
                    //<--
                    //-->ConsultarGestorPM
                    result.Add("ObjActProy", objMantenimientoFactory.getMantenimientoService().ConsultarGestorPM(modeloEquipoID, idPM, idmantenimiento));
                    //<--
                    //-->CargaDeProyectado
                    var objCargaDeProyectado = objMantenimientoFactory.getMantenimientoService().CargaDeProyectado(idmantenimiento);
                    result.Add("CargaDeProyectado", objCargaDeProyectado);
                    //<--
                    //-->CargaDeAEProyectado
                    var objCargaDeAEProyectado = objMantenimientoFactory.getMantenimientoService().CargaDeAEProyectado(idmantenimiento);
                    result.Add("CargaDeAEProyectado", objCargaDeAEProyectado);
                    //<--
                    //-->CargaDeDNProyectado
                    var objCargaDeDNProyectado = objMantenimientoFactory.getMantenimientoService().CargaDeDNProyectado(idmantenimiento);
                    result.Add("CargaDeDNProyectado", objCargaDeDNProyectado);
                    //<--
                    //-->
                    bool programa = true;
                    if (objResult != null)
                    {
                        if (objResult.tipoPM == 1 || objResult.tipoPM == 3 || objResult.tipoPM == 5 || objResult.tipoPM == 7) programa = false;
                    }
                    var resultData = objMantenimientoFactory.getMantenimientoService().getDetActividadesMantProy(modeloEquipoID).ToList().Distinct();
                    var DatosRep = resultData.GroupBy(x => new { x.componente, x.idAct, x.idCompVis }).Distinct().Select(y => new objMiselaneosDTO
                    {
                        componente = y.Key.componente,
                        cantidad = "",
                        chkProgramado = "",
                        modeloEquipoID = modeloEquipoID,
                        idCompVis = y.Key.idCompVis,
                        modelo = 0,
                        idFiltro = 0,
                        aplicar = false,
                        programado = programa,
                        idMant = 0,
                        tipoPMid = 0
                    }).ToList().Distinct().ToList();
                    result.Add("CargaDeFiltros", resultData);
                    //<--

                    //-->Traer Formatos
                    var objFormato = objMantenimientoFactory.getMantenimientoService().getFormato(idmantenimiento, idPM);
                    result.Add("objFormato", objFormato);
                    //<--
                    result.Add(SUCCESS, true);
                }
                else

                    result.Add(SUCCESS, false);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarBitProyLub(tblM_BitacoraControlAceiteMantProy ObjBitProyLub)
        {
            var result = new Dictionary<string, object>();
            try
            {
                ObjBitProyLub.fechaCaptura = DateTime.Now;
                ObjBitProyLub.UsuarioCap = base.getUsuario().id;
                result.Add("ObjBitProyLub", objMantenimientoFactory.getMantenimientoService().GuardarBitProyLub(ObjBitProyLub));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarBitProyLub2(int id, string Comentario)
        {
            var result = new Dictionary<string, object>();
            try
            {

                tblM_BitacoraControlAceiteMantProy ObjBitProyLub = objMantenimientoFactory.getMantenimientoService().getBitProyLubByid(id);
                ObjBitProyLub.Observaciones = Comentario;
                ObjBitProyLub.fechaCaptura = DateTime.Now;
                ObjBitProyLub.UsuarioCap = base.getUsuario().id;
                result.Add("ObjBitProyLub", objMantenimientoFactory.getMantenimientoService().GuardarBitProyLub(ObjBitProyLub));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CargaLubObs(int idMantProy)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add("ObjBitProyLub", objMantenimientoFactory.getMantenimientoService().CargaLubObs(idMantProy));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ConsultarGestorPM(int modeloEquipoID, int idPM, int idmantenimiento)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add("ObjActProy", objMantenimientoFactory.getMantenimientoService().ConsultarGestorPM(modeloEquipoID, idPM, idmantenimiento));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarActividadesMantProy(tblM_BitacoraActividadesMantProy objActvProy)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var saveDataObj = objMantenimientoFactory.getMantenimientoService().getActividadesProByID(objActvProy.id);

                var guardarActividad = objMantenimientoFactory.getMantenimientoService().ActualizarActividadProgramada(objActvProy);

                var getMntto = objMantenimientoFactory.getMantenimientoService().ConsultarPMbyID(saveDataObj.idMant);
                var getMntoomodelo = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(getMntto.idMaquina).FirstOrDefault().modeloEquipoID;
                result.Add("mntto", getMntoomodelo);
                result.Add("idTipoPM", objActvProy.idPm);
                result.Add("idMatn", saveDataObj.idMant);


                result.Add("objActvProy", objMantenimientoFactory.getMantenimientoService().GuardarActividadesMantProy(saveDataObj));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarActividadProy(tblM_BitacoraActividadesMantProy ObjAct)
        {
            var result = new Dictionary<string, object>();
            try
            {
                ObjAct.fechaCaptura = DateTime.Now;
                ObjAct.UsuarioCap = base.getUsuario().id;
                result.Add("objActvProy", objMantenimientoFactory.getMantenimientoService().GuardarActividadProy(ObjAct));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EliminarActividadProy(tblM_BitacoraActividadesMantProy objActvProy)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var deleteDataObj = objMantenimientoFactory.getMantenimientoService().getActividadesProByID(objActvProy.id);
                var getMntto = objMantenimientoFactory.getMantenimientoService().ConsultarPMbyID(deleteDataObj.idMant);
                var getMntoomodelo = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(getMntto.idMaquina).FirstOrDefault().modeloEquipoID;
                var idMant = deleteDataObj.idMant;

                var exito = objMantenimientoFactory.getMantenimientoService().EliminarActividadProy(objActvProy);
                result.Add("exito", exito);
                result.Add("mntto", getMntoomodelo);
                result.Add("idTipoPM", getMntto.tipoPM);
                result.Add("idMatn", idMant);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ActOtroPm(int idMant)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add("objActvProy", objMantenimientoFactory.getMantenimientoService().ActOtroPm(idMant));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ConsultarObservacionActividad(int idObjAct)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add("objActvObs", objMantenimientoFactory.getMantenimientoService().ConsultarObservacionActividad(idObjAct));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarBitProyAE(tblM_BitacoraControlAEMantProy ObjBitProyAE)
        {
            var result = new Dictionary<string, object>();
            try
            {
                ObjBitProyAE.fechaCaptura = DateTime.Now;
                ObjBitProyAE.UsuarioCap = base.getUsuario().id;
                result.Add("ObjBitProyAE", objMantenimientoFactory.getMantenimientoService().GuardarBitProyAE(ObjBitProyAE));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarBitProyAE2(tblM_BitacoraControlAEMantProy ObjBitProyAE)
        {
            var result = new Dictionary<string, object>();
            try
            {
                tblM_BitacoraControlAEMantProy ObjBitProyAE2 = objMantenimientoFactory.getMantenimientoService().GetBitProyID(ObjBitProyAE.id);


                ObjBitProyAE2.Observaciones = ObjBitProyAE.Observaciones;
                ObjBitProyAE2.fechaCaptura = DateTime.Now;
                ObjBitProyAE2.UsuarioCap = base.getUsuario().id;
                result.Add("ObjBitProyAE", objMantenimientoFactory.getMantenimientoService().GuardarBitProyAE(ObjBitProyAE2));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarBitProyDN(tblM_BitacoraControlDNMantProy ObjBitProyDN)
        {
            var result = new Dictionary<string, object>();
            try
            {
                ObjBitProyDN.fechaCaptura = DateTime.Now;
                ObjBitProyDN.UsuarioCap = base.getUsuario().id;
                result.Add("ObjBitProyDN", objMantenimientoFactory.getMantenimientoService().GuardarBitProyDN(ObjBitProyDN));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeshabilitarLubProy(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add("obj", objMantenimientoFactory.getMantenimientoService().DeshabilitarLubProy(id));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //DeshabilitarACProy
        public ActionResult DeshabilitarACProy(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add("obj", objMantenimientoFactory.getMantenimientoService().DeshabilitarACProy(id));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ConsultaModelobyMantenimiento(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add("obj", objMantenimientoFactory.getMantenimientoService().ConsultaModelobyMantenimiento(id));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeshabilitarDNProy(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add("obj", objMantenimientoFactory.getMantenimientoService().DeshabilitarDNProy(id));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        //public FileResult getDocumentos()
        //{
        //    int id = Convert.ToInt32(Request.QueryString["id"]);

        //    var Archivo = objMantenimientoFactory.getMantenimientoService().getDocumentosByID(id);

        //    //return File(Archivo.nombreRuta, "multipart/form-data", Archivo.nombreArchivo);
        //    return File("C:\\Users\\raguilar\\Documents\\Consulta1.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Archivo.nombreArchivo);
        //}

        public FileResult getDocumentos()
        {
            //int id = Convert.ToInt32(Request.QueryString["id"]);
            string id = Request.QueryString["id"];
            List<int> Ids = Request.QueryString["id"].Split(',').Select(int.Parse).ToList();
            var archivos = objMantenimientoFactory.getMantenimientoService().getDocumentosByID(Ids);
            var zip = objMantenimientoFactory.getMantenimientoService().GetZipDocumentosPM(archivos);
            return File(zip, "application/zip", "Documentos-PM.zip");
        }

        public ActionResult getFormato(int id, int idpm)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var objFormato = objMantenimientoFactory.getMantenimientoService().getFormato(id, idpm);

                result.Add("obj", objFormato);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetReporte(int obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                Session["rptAutorizantesDTO"] = null;
                Session["rptAutoriacionResguardos"] = null;

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConsultarActPmbyModelo(int modelo, int idact)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add("obj", objMantenimientoFactory.getMantenimientoService().ConsultarActPmbyModelo(modelo, idact));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillGridComponenteRestrinccion(int modeloEquipoID, int idActs, int idTipoAct, int idpm)
        {
            var result = new Dictionary<string, object>();
            var listResult = objMantenimientoFactory.getMantenimientoService().FillGridComponenteRestrinccion(modeloEquipoID, idActs, idTipoAct, idpm);


            //   var listaResultado = objMantenimientoFactory.getMantenimientoService().getDetActividadesMantProy(idpm);
            result.Add("current", 1);
            result.Add("rowCount", 1);
            result.Add("rows", listResult);
            result.Add(SUCCESS, true);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult setProgramacionActividades(int mantenimientoId, int idActs)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var listaResultado = objMantenimientoFactory.getMantenimientoService().getDetActividadesMantProy(mantenimientoId).Where(x => x.idAct == idActs).ToList();



                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //Oscar
        public ActionResult GetMantenimientosProg(string cc = "")
        {
            var result = new Dictionary<string, object>();

            try
            {
                result.Add("data", objMantenimientoFactory.getMantenimientoService().GetMantenimientosProg(cc).Select(x =>
                    new
                    {
                        x.id,
                        x.economicoID,
                        x.horometroUltCapturado,
                        x.fechaUltCapturado,
                        x.tipoPM,
                        fechaPM = x.fechaPM.ToShortDateString(),
                        x.horometroPM,
                        x.personalRealizo,
                        x.observaciones,
                        x.horometroProy,
                        x.fechaProy,
                        x.tipoMantenimientoProy,
                        x.fechaProyFin,
                        x.actual,
                        x.fechaCaptura,
                        x.idMaquina,
                        x.estatus,
                        x.planeador,
                        x.UsuarioCap,
                        x.estadoMantenimiento

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

        public ActionResult sendEjecutadoSave()
        {
            var result = new Dictionary<string, object>();

            try
            {
                var referencias = new List<tblM_MantenimientoPm_Archivo>();

                var objGeneral = JsonConvert.DeserializeObject<tblM_MatenimientoPm>(Request.Form["objGeneral"]);

                objGeneral.fechaCaptura = DateTime.Now;

                var idMantenimiento = JsonConvert.DeserializeObject<int>(Request.Form["idMantenimiento"]);
                var tblGridLubProxTbl = JsonConvert.DeserializeObject<tblM_BitacoraControlAceiteMantProy[]>(Request.Form["lubricantes[]"]).ToList();
                foreach (var item in tblGridLubProxTbl) item.estatus = true;
                var tblGridActProxTbl = JsonConvert.DeserializeObject<tblM_BitacoraControlAEMantProy[]>(Request.Form["actividadesExtra[]"]).ToList();
                var tblgridDNProxTbl = JsonConvert.DeserializeObject<tblM_BitacoraControlDNMantProy[]>(Request.Form["dns[]"]).ToList();

                if (Request.Files.Count > 0)
                {
                    var economico = objGeneral.economicoID;
                    var carpeta = economico + "_" + idMantenimiento;

                    string rutaCarpeta = "";// archivofs.getArchivo().getRegistro(1015).dirVirtual + carpeta + "\\";

                    try
                    {
                        rutaCarpeta = archivofs.getArchivo().getRegistro(1015).dirVirtual + carpeta + "\\";
                        System.IO.Directory.CreateDirectory(rutaCarpeta);

                    }
                    catch (Exception)
                    {
                        rutaCarpeta = @"C:\Proyecto\SIGOPLAN\MAQUINARIA\" + carpeta + "\\";
                        System.IO.Directory.CreateDirectory(rutaCarpeta);

                    }


                    var ultimoArchivoMantenimiento = objMantenimientoFactory.getMantenimientoService().GetUltimoArchivoMantenimiento();
                    var ultimoArchivoMantenimientoID = ultimoArchivoMantenimiento != null ? ultimoArchivoMantenimiento.id : 0;

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase archivo = Request.Files[i];
                        string nombre = string.Format("{0:D8}", ++ultimoArchivoMantenimientoID);

                        var extension = "";
                        int idx = archivo.FileName.LastIndexOf('.');
                        if (idx != -1)
                        {
                            extension = archivo.FileName.Substring(idx + 1);
                        }
                        else
                        {
                            extension = "pdf";
                        }

                        string ruta = rutaCarpeta + nombre + "." + extension;
                        referencias.Add(new tblM_MantenimientoPm_Archivo()
                        {
                            idMantenimiento = idMantenimiento,
                            nombre = nombre,
                            ruta = ruta,
                            estatus = true
                        });
                        SaveArchivo(archivo, ruta);
                    }
                }

                var mant = objMantenimientoFactory.getMantenimientoService().GuardarEjecutado(objGeneral, idMantenimiento, tblGridLubProxTbl, tblGridActProxTbl, tblgridDNProxTbl, referencias);

                result.Add("data", mant);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        void SaveArchivo(HttpPostedFileBase archivo, string ruta)
        {
            byte[] data;

            using (Stream inputStream = archivo.InputStream)
            {
                MemoryStream memoryStream = inputStream as MemoryStream;
                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }
                data = memoryStream.ToArray();
            }

            //ruta = ruta.Replace("C:\\", "\\REPOSITORIO\\");

            System.IO.File.WriteAllBytes(ruta, data);
        }
        //---

        private void getRptMensualManttoPreventivo()
        {
            using (ExcelPackage package = new ExcelPackage())
            {

                var mEjecita = package.Workbook.Worksheets.Add("Ejecutado");

                ExcelRange cols = mEjecita.Cells["A:AH"];
            }
        }

        public ActionResult fillCboMarcaFiltro()
        {
            var result = new Dictionary<string, object>();

            try
            {
                var items = objMantenimientoFactory.getMantenimientoService().fillCboMarcaFiltro().Select(x => new
                {
                    Value = x.id,
                    Text = x.marca
                });
                result.Add(ITEMS, items);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarFiltro(tblM_CatFiltroMant obj)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var noExisteFiltro = objMantenimientoFactory.getMantenimientoService().GuardarFiltro(obj);
                result.Add("noExisteFiltro", noExisteFiltro);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveOrUpdateAgrupacionComponenteModelo(tblM_PMComponenteModelo obj)
        {
            var result = new Dictionary<string, object>();

            try
            {
                obj.usuariosCaptura = getUsuario().id;
                if (grupoComponenteModeloFactoryServices.GrupoComponenteModeloService().SaveOrUpdateAgrupacionComponenteModelo(obj))
                {
                    result.Add(SUCCESS, true);
                    result.Add(MESSAGE, "La información se guardo correctamente");
                }
                else
                {
                    result.Add(SUCCESS, false);
                    result.Add(MESSAGE, "Sucedio un error al momento de procesar el guardado de la información, favor de verificar e intentar nuevamante, si el error persiste favor de contactar el area de TI.");
                }

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, "Sucedio un error al momento de procesar el guardado de la información, favor de verificar e intentar nuevamante, si el error persiste favor de contactar el area de TI.");
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult DeleteAgrupacionComponenteModelo(int id)
        {
            var result = new Dictionary<string, object>();

            try
            {
                tblM_PMComponenteModelo obj = grupoComponenteModeloFactoryServices.GrupoComponenteModeloService().getPMComponenteModeloByID(id);

                if (obj != null)
                {
                    var objComponenteLubricante = pmComponenteLubricanteFactoryServices.getPMComponenteLubricanteFactoryServices().getTblComponentesLubricantes(obj.modeloID);
                    var objComponenteFiltros = pmComponenteFiltroFactoryServices.getPMComponenteFiltroFactoryServices().FillTblComponenteFiltro(obj.modeloID);

                    foreach (var item in objComponenteFiltros.Where(x => x.componenteID == obj.componenteID))
                    {
                        pmComponenteFiltroFactoryServices.getPMComponenteFiltroFactoryServices().DeleteComponenteFiltro(item);
                    }

                    foreach (var item in objComponenteLubricante.Where(x => obj.componenteID == x.componenteID))
                    {
                        pmComponenteLubricanteFactoryServices.getPMComponenteLubricanteFactoryServices().DeleteComponenteLubricante(item);
                    }


                    grupoComponenteModeloFactoryServices.GrupoComponenteModeloService().DeleteComponetneModelo(obj);

                    var dataSet = grupoComponenteModeloFactoryServices.GrupoComponenteModeloService().filltblAgrupacionComponenteModelo(obj.modeloID).Select(x =>
                    new
                    {
                        x.id,
                        x.componenteID,
                        x.modeloID,
                        x.estatus,
                        x.usuariosCaptura,
                        x.fechaCaptura,
                        modelo = x.Modelo.descripcion,
                        componente = x.Componente.Descripcion
                    }).ToList();

                    if (dataSet.Count > 0)
                    {
                        result.Add("dataSet", dataSet);
                    }
                    result.Add(SUCCESS, true);
                    result.Add(MESSAGE, "La información se guardo correctamente");
                }
                else
                {
                    result.Add(SUCCESS, false);
                    result.Add(MESSAGE, "Sucedio un error al momento de procesar el guardado de la información, favor de verificar e intentar nuevamante, si el error persiste favor de contactar el area de TI.");
                }

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, "Sucedio un error al momento de procesar el guardado de la información, favor de verificar e intentar nuevamante, si el error persiste favor de contactar el area de TI.");
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteFiltrosComponente(int id, int modeloID)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var objComponenteFiltros = pmComponenteFiltroFactoryServices.getPMComponenteFiltroFactoryServices().FillTblComponenteFiltro(modeloID);

                pmComponenteFiltroFactoryServices.getPMComponenteFiltroFactoryServices().DeleteComponenteFiltro(objComponenteFiltros.FirstOrDefault(x => x.id == id));

                if (modeloID != null)
                {
                    var dataSet = grupoComponenteModeloFactoryServices.GrupoComponenteModeloService().filltblAgrupacionComponenteModelo(modeloID).Select(x =>
                    new
                    {
                        x.id,
                        x.componenteID,
                        x.modeloID,
                        x.estatus,
                        x.usuariosCaptura,
                        x.fechaCaptura,
                        modelo = x.Modelo.descripcion,
                        componente = x.Componente.Descripcion
                    }).ToList();

                    if (dataSet.Count > 0)
                    {
                        result.Add("dataSet", dataSet);
                    }
                    result.Add(SUCCESS, true);
                    result.Add(MESSAGE, "La información se guardo correctamente");
                }
                else
                {
                    result.Add(SUCCESS, false);
                    result.Add(MESSAGE, "Sucedio un error al momento de procesar el guardado de la información, favor de verificar e intentar nuevamante, si el error persiste favor de contactar el area de TI.");
                }

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, "Sucedio un error al momento de procesar el guardado de la información, favor de verificar e intentar nuevamante, si el error persiste favor de contactar el area de TI.");
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteLubricanteComponente(int id, int modeloID)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var objComponenteLubricante = pmComponenteLubricanteFactoryServices.getPMComponenteLubricanteFactoryServices().getTblComponentesLubricantes(modeloID);

                pmComponenteLubricanteFactoryServices.getPMComponenteLubricanteFactoryServices().DeleteComponenteLubricante(objComponenteLubricante.FirstOrDefault(x => x.id == id));

                if (modeloID != null)
                {
                    var dataSet = grupoComponenteModeloFactoryServices.GrupoComponenteModeloService().filltblAgrupacionComponenteModelo(modeloID).Select(x =>
                    new
                    {
                        x.id,
                        x.componenteID,
                        x.modeloID,
                        x.estatus,
                        x.usuariosCaptura,
                        x.fechaCaptura,
                        modelo = x.Modelo.descripcion,
                        componente = x.Componente.Descripcion
                    }).ToList();

                    if (dataSet.Count > 0)
                    {
                        result.Add("dataSet", dataSet);
                    }
                    result.Add(SUCCESS, true);
                    result.Add(MESSAGE, "La información se guardo correctamente");
                }
                else
                {
                    result.Add(SUCCESS, false);
                    result.Add(MESSAGE, "Sucedio un error al momento de procesar el guardado de la información, favor de verificar e intentar nuevamante, si el error persiste favor de contactar el area de TI.");
                }

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, "Sucedio un error al momento de procesar el guardado de la información, favor de verificar e intentar nuevamante, si el error persiste favor de contactar el area de TI.");
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboCatFiltros(string term = "")
        {
            var dataDTO = objMantenimientoFactory.getMantenimientoService().FillCboCatFiltros().Select(r => new { id = r.id, label = r.modelo + " " + r.descripcion }).Where(r => r.label.Contains(term)).ToList();
            return Json(dataDTO, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillCboCatComponentes()
        {
            var result = new Dictionary<string, object>();
            try
            {

                result.Add(ITEMS, objMantenimientoFactory.getMantenimientoService().getCatComponentesViscosidades().Select(x => new { Value = x.id, Text = x.Descripcion }).OrderBy(r => r.Text));
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public ActionResult FillCboCatComponentesByModelo(int modeloID)
        {
            var result = new Dictionary<string, object>();
            try
            {

                result.Add(ITEMS, objMantenimientoFactory.getMantenimientoService().getCatComponentesViscosidadesByModelo(modeloID).Select(x => new { Value = x.id, Text = x.Descripcion }).Distinct().OrderBy(r => r.Text));
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult filltblAgrupacionComponenteModelo(int modeloID)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var listaDatos = grupoComponenteModeloFactoryServices.GrupoComponenteModeloService().filltblAgrupacionComponenteModelo(modeloID);

                if (listaDatos.Count > 0)
                {
                    result.Add("dataSet", listaDatos.Select(x => new
                    {
                        x.id,
                        x.componenteID,
                        x.modeloID,
                        x.estatus,
                        x.usuariosCaptura,
                        x.fechaCaptura,
                        modelo = x.Modelo.descripcion,
                        componente = x.Componente.Descripcion
                    }).ToList());
                    result.Add(SUCCESS, true);
                }
                else
                {
                    result.Add(SUCCESS, false);
                    result.Add(MESSAGE, "No se encontró información.");
                }

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getPlaneacionSemanal(string areaCuenta, string strFechaInicio, string strFechaFin, string economico)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var fechaInicio = Convert.ToDateTime(strFechaInicio);
                var fechaFin = Convert.ToDateTime(strFechaFin);

                var planeacionSemanal = objMantenimientoFactory
                    .getMantenimientoService()
                    .getPlaneacionSemanal(areaCuenta, fechaInicio, fechaFin, economico, true)
                    .OrderBy(x => x.fechaProgramado);

                var lstPlaneacionSemanal = planeacionSemanal.Select(x => new
                    {
                        id = x.idMant,
                        economico = x.economico,
                        tipoServicio = x.tipoServicio,
                        //fechaProgramado = x.fechaProgramado.ToShortDateString(),
                        fechaProgramado = Convert.ToDateTime(x.fechaProgramado),
                        horometroProgramado = x.horometroProgramado,
                        componentes = x.componentes
                    });

                result.Add(ITEMS, lstPlaneacionSemanal);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CargarHorasPlaneacionSemanal(List<cboDTO> listaDuracion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                Session["listaDuracion"] = listaDuracion;
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public MemoryStream DescargarExcelJOMAGALI(string areaCuenta, DateTime fechaInicio, DateTime fechaFin, string economico)
        {
            var stream = objMantenimientoFactory.getMantenimientoService().DescargarExcelJOMAGALI(areaCuenta, fechaInicio, fechaFin, economico);

            if (stream != null)
            {
                this.Response.Clear();
                this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "Reporte JOMAGALI.xlsx"));
                this.Response.BinaryWrite(stream.ToArray());
                this.Response.End();

                return stream;
            }
            else
            {
                return null;
            }
        }


        #region Agrupacion de modelos componentes
        public ActionResult fillTblComponenteLubricante(int modeloID)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var dataSet = pmComponenteLubricanteFactoryServices.getPMComponenteLubricanteFactoryServices()
                    .getTblComponentesLubricantes(modeloID).Select(x => new
                    {
                        x.cantidadLitros,
                        x.componenteID,
                        x.estatus,
                        x.fechaCaptura,
                        x.id,
                        x.lubricanteID,
                        x.modeloID,
                        x.usuarioID,
                        x.vidaLubricante,
                    });

                result.Add("dataSet", dataSet);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboCatLubricantes()
        {
            var result = new Dictionary<string, object>();
            try
            {

                result.Add(ITEMS, pmComponenteLubricanteFactoryServices.getPMComponenteLubricanteFactoryServices().FillCboCatLubricantes().Select(x => new ComboDTO { Value = x.id, Text = x.nomeclatura }));
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveOrUpdateComponenteLubricante(tblM_PMComponenteLubricante obj)
        {
            var result = new Dictionary<string, object>();

            try
            {
                obj.usuarioID = getUsuario().id;
                obj.fechaCaptura = DateTime.Now;
                if (pmComponenteLubricanteFactoryServices.getPMComponenteLubricanteFactoryServices().SaveOrUpdateComponenteLubricante(obj))
                {
                    result.Add(SUCCESS, true);
                    result.Add(MESSAGE, "La información se guardo correctamente");
                }
                else
                {
                    result.Add(SUCCESS, false);
                    result.Add(MESSAGE, "Sucedio un error al momento de procesar el guardado de la información, favor de verificar e intentar nuevamante, si el error persiste favor de contactar el area de TI.");
                }

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, "Sucedio un error al momento de procesar el guardado de la información, favor de verificar e intentar nuevamante, si el error persiste favor de contactar el area de TI.");
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Agrupacion de filtros con componentes
        public ActionResult SaveOrUpdateComponenteFiltro(tblM_PMComponenteFiltro obj)
        {
            var result = new Dictionary<string, object>();

            try
            {

                if (obj.componenteID != 0)
                {
                    obj.usuarioID = getUsuario().id;
                    obj.fechaCaptura = DateTime.Now;
                    if (pmComponenteFiltroFactoryServices.getPMComponenteFiltroFactoryServices().SaveOrUpdateAgrupacionComponenteFiltro(obj))
                    {
                        result.Add(SUCCESS, true);
                        result.Add(MESSAGE, "La información se guardo correctamente");
                    }
                    else
                    {
                        result.Add(SUCCESS, false);
                        result.Add(MESSAGE, "Sucedio un error al momento de procesar el guardado de la información, favor de verificar e intentar nuevamante, si el error persiste favor de contactar el area de TI.");
                    }
                }
                else
                {
                    result.Add(MESSAGE, "Se debe seleccionar un componente.");
                }

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, "Sucedio un error al momento de procesar el guardado de la información, favor de verificar e intentar nuevamante, si el error persiste favor de contactar el area de TI.");
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillTblComponenteFiltro(int modeloID)
        {
            var result = new Dictionary<string, object>();

            try
            {

                var filtros = objMantenimientoFactory.getMantenimientoService().FillCboCatFiltros();
                foreach (var item in filtros) { item.modelo = item.modelo.Replace("-", "").Trim(); }
                var codigos = filtros.Select(x => x.modelo).ToList();
                var insumos = objMantenimientoFactory.getMantenimientoService().GetInsumoEnkontrol(codigos);
                var dataSet = pmComponenteFiltroFactoryServices.getPMComponenteFiltroFactoryServices().FillTblComponenteFiltro(modeloID).Select(x =>
                    new
                    {
                        x.id,
                        x.componenteID,
                        x.filtroID,
                        x.estatus,
                        x.usuarioID,
                        x.fechaCaptura,
                        x.cantidad,
                        descripcion = filtros.FirstOrDefault(r => r.id == x.filtroID) != null ? filtros.FirstOrDefault(r => r.id == x.filtroID).descripcion : "",
                        Codigo = filtros.FirstOrDefault(r => r.id == x.filtroID) != null ? filtros.FirstOrDefault(r => r.id == x.filtroID).modelo : "",
                        insumo = filtros.FirstOrDefault(r => r.id == x.filtroID) != null ?
                        (insumos.FirstOrDefault(r => r.Text == filtros.FirstOrDefault(s => s.id == x.filtroID).modelo) != null ? insumos.FirstOrDefault(r => r.Text == filtros.FirstOrDefault(s => s.id == x.filtroID).modelo).Value : "--") : "",
                    }
                    ).ToList();
                if (dataSet.Count > 0)
                {
                    result.Add("dataSet", dataSet);
                    result.Add(SUCCESS, true);
                }
                else
                {
                    result.Add(SUCCESS, false);
                    result.Add(MESSAGE, "No se encontró información.");
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
    }
}
