using Core.DTO;
using Core.DTO.Principal.Generales;
using Core.DTO.Maquinaria.Overhaul;
using Core.Entity.Maquinaria.Catalogo;
using Core.Enum.Maquinaria;
using Data.Factory.Maquinaria.Catalogos;
using Data.Factory.Maquinaria.Overhaul;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Catalogos.Componentes
{
    public class CatComponentesController : BaseController
    {

        #region Factory
        ComponenteFactoryServices componenteFactoryServices;
        FolioComponenteFactoryServices folioComponenteFactoryServices;
        AdministracionComponentesFactoryServices acfs;
        MaquinaFactoryServices maquinaFS;
        #endregion
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            componenteFactoryServices = new ComponenteFactoryServices();
            folioComponenteFactoryServices = new FolioComponenteFactoryServices();
            acfs = new AdministracionComponentesFactoryServices();
            maquinaFS = new MaquinaFactoryServices();
            base.OnActionExecuting(filterContext);
        }
        // GET: CatComponentes
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
        public ActionResult FillGrid_Componente(ComponenteDTO componente, List<int> conjuntos, List<int> subconjuntos, List<ComboDTO> cc)
        {
            if (cc == null) { cc = new List<ComboDTO>(); }
            var result = new Dictionary<string, object>();
            try
            {
                List<string> obraID = cc.Where(x => x.Prefijo == "0").Select(x => x.Value).ToList();
                var locacionIDs = componenteFactoryServices.getComponenteService().GetMaquinaByListaCC(obraID);
                var almacenesYCRC = cc.Where(x => x.Prefijo == "1" || x.Prefijo == "2").Select(x => Int32.Parse(x.Value));

                var listResult = componenteFactoryServices.getComponenteService().FillGrid_Componente(componente);

                var auxListResult = listResult.Where(x => almacenesYCRC.Contains(x.locacionID) && x.tipoLocacion > 0).ToList();
                var listResultFinal = listResult.Where(x => locacionIDs.Contains(x.locacionID) && x.tipoLocacion == 0).ToList();
                listResultFinal.AddRange(auxListResult);
                //result.Add("current", 1);
                //result.Add("rowCount", 1);
                //result.Add("total", listResultFinal.Count());
                result.Add("rows", listResultFinal);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult cboModeloPrefijo(int idModelo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, componenteFactoryServices.getComponenteService().FillCboPrefijoModelo(idModelo).Select(x => new { Value = x.prefijoValue, Text = x.prefijoText, Prefijo = x.idModelo}));
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillCboGrupo_Componente()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, componenteFactoryServices.getComponenteService().FillCboGrupoMaquinaria().OrderBy(x=>x.Text));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillCboModelo_Componente(int idGrupo = -1)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var c = componenteFactoryServices.getComponenteService().FillCboModeloEquipo(idGrupo).Select(x => new { Value = x.id, Text = x.descripcion, Prefijo = x.nomCorto });
                    //.GroupBy(y => y.Text).Select(z => new { Value = z.Key, Text = z.Key });
                result.Add(ITEMS, c);
                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillCboConjunto_Componente(int idModelo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, componenteFactoryServices.getComponenteService().FillCboConjunto(idModelo));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillCboSubConjunto_Componente(List<int> idConjunto, int idModelo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, componenteFactoryServices.getComponenteService().FillCboSubConjuntos(idConjunto, idModelo).Select(x => new { Value = x.id, Text = x.descripcion, Prefijo = x.prefijo, hasPosicion = x.hasPosicion }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillCboSubConjuntoComponente(List<int> idConjunto, int idModelo = -1)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, componenteFactoryServices.getComponenteService().FillCboSubConjuntos(idConjunto, idModelo).Select(x => new { Value = x.id, Text = x.descripcion, Prefijo = x.prefijo }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillCboPosiciones_Componente(int idSubconjunto)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, componenteFactoryServices.getComponenteService().FillCboPosicionesComponente(idSubconjunto));
                //result.Add(ITEMS, GlobalUtils.ParseEnumToCombo<PosicionesEnum>());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveOrUpdate_Componente(tblM_CatComponente obj1, tblM_FolioComponente obj2, int Actualizacion, int locacion, DateTime fechaInstalacion, int tipoLocacion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                int idTracking = 0;
                if (Actualizacion == 1)
                {                    
                    int value = 1;
                    if (folioComponenteFactoryServices.getFolioComponenteServices().Exists(obj2))
                    {
                        tblM_FolioComponente f = folioComponenteFactoryServices.getFolioComponenteServices().getFolio(obj2);
                        if (obj1.subConjuntoID != 1 && obj1.subConjuntoID != 50 && obj1.subConjuntoID != 82) 
                        {                           
                            var extFolio = 13 - obj1.noComponente.Length - obj2.Folio.ToString().Length;
                            obj1.noComponente += f.Folio.ToString().PadLeft(extFolio, '0');
                        }
                        obj2.Folio = f.Folio + 1;
                        obj2.id = f.id;
                        folioComponenteFactoryServices.getFolioComponenteServices().Guardar(obj2);
                        componenteFactoryServices.getComponenteService().Guardar(obj1);
                        idTracking = componenteFactoryServices.getComponenteService().GuardarTrackingComponente(obj1, locacion, fechaInstalacion, tipoLocacion, false, obj1.ordenCompra, obj1.costo.ToString());
                        componenteFactoryServices.getComponenteService().ActualizarTracking(obj1.id, idTracking);
                    }
                    else
                    {
                        obj1.falla = false;
                        if (obj1.subConjuntoID != 1 && obj1.subConjuntoID != 50 && obj1.subConjuntoID != 82 && obj1.subConjuntoID != 84) 
                        { 
                            value = 1;
                            var extFolio = 12 - obj1.noComponente.Length;
                            if (extFolio > 0) obj1.noComponente += value.ToString().PadLeft(extFolio, '0');
                            else obj1.noComponente += value.ToString();
                            //obj1.noComponente += value.ToString().PadLeft(3, '0');                            
                        }
                        obj2.Folio = value + 1;
                        folioComponenteFactoryServices.getFolioComponenteServices().Guardar(obj2);
                        componenteFactoryServices.getComponenteService().Guardar(obj1);
                        idTracking = componenteFactoryServices.getComponenteService().GuardarTrackingComponente(obj1, locacion, fechaInstalacion, tipoLocacion, false, "", "");
                        componenteFactoryServices.getComponenteService().ActualizarTracking(obj1.id, idTracking);
                    }
                    result.Add(MESSAGE, "Se genero el numero de componente " + obj1.noComponente);
                }
                else
                {
                    var componente = componenteFactoryServices.getComponenteService().getComponenteByID(obj1.id);
                    if (componente != null)
                    {
                        obj1.horasCicloInicio = componente.horasCicloInicio;
                        obj1.horasAcumuladasInicio = componente.horasAcumuladasInicio;
                        obj1.trackComponenteID = componente.trackComponenteID;
                    }
                    componenteFactoryServices.getComponenteService().Guardar(obj1);
                    result.Add(MESSAGE, GlobalUtils.getMensaje(Actualizacion));
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

        public ActionResult FillCboFiltroModelo_Componente(int idGrupo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, componenteFactoryServices.getComponenteService().FillCboFiltroModeloEquipo().Select(x => new { Value = x.id, Text = x.descripcion }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillCbo_CentroCostos()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, componenteFactoryServices.getComponenteService().FillCboCentroCostros().Select(x => new { Value = x.id, Text = x.descripcion }).OrderBy(x => x.Text));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getFolio_Componente(tblM_FolioComponente obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                int value = 0;
                if (folioComponenteFactoryServices.getFolioComponenteServices().Exists(obj))
                {
                    value = folioComponenteFactoryServices.getFolioComponenteServices().getFolio(obj).Folio;
                }
                else
                {
                    value = 1;
                }
                result.Add(ITEMS, value > 0 ? value.ToString().PadLeft(3, '0') : "1".PadLeft(3, '0'));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboLocacion(int tipoLocacion) 
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> locaciones = new List<ComboDTO>();
                ComboDTO SE = new ComboDTO();
                SE.Value = "0"; 
                SE.Text = "Sin especificar";
                locaciones.Add(SE);
                locaciones.AddRange(acfs.getAdministracionComponentesFactoryServices().FillCboLocacion(tipoLocacion).Select(x => new ComboDTO { Value = x.id.ToString(), Text = x.descripcion }).ToList());
                result.Add(ITEMS, locaciones);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboPosiciones_Locaciones(int idModelo, int tipoBusqueda)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var locaciones = componenteFactoryServices.getComponenteService().FillCboLocaciones(tipoBusqueda).Select(x => new { Value = x.id, Text = x.descripcion, Prefijo = x.tipoLocacion }).ToList();
                if (tipoBusqueda == 2) 
                {
                    var locaciones2 = maquinaFS.getMaquinaServices().GetAllMaquinas().Select(x => new { Value = x.id, Text = x.noEconomico, Prefijo = 0 }).ToList();
                    locaciones = locaciones.Concat(locaciones2).ToList();
                }
                else if (idModelo >= 0) { 
                    var locaciones2 = componenteFactoryServices.getComponenteService().FillCboLocacionesMaquina(idModelo).Select(x => new { Value = x.id, Text = x.noEconomico, Prefijo = 0 }).ToList(); 
                    locaciones = locaciones.Concat(locaciones2).ToList();
                }
                
                result.Add(ITEMS, locaciones);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarComponente(int idComponente) 
        {
            var result = new Dictionary<string, object>();
            try
            {
                componenteFactoryServices.getComponenteService().DeleteComponente(idComponente);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getLocacion(int idComponente) {
            var result = new Dictionary<string, object>();
            try
            {
                var tracking = componenteFactoryServices.getComponenteService().getLocacion(idComponente);

                result.Add("locacion", tracking.locacionID);
                result.Add("tipoLocacion", tracking.estatus);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboMarcasComponentes() 
        {
            var result = new Dictionary<string, object>();
            try
            {
                var marcas = componenteFactoryServices.getComponenteService().getMarcas();

                result.Add(ITEMS, marcas.Select(x => new { Value = x.id, Text = x.descripcion }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboNumParte(int idModelo, int idSubconjunto) 
        {
            var result = new Dictionary<string, object>();
            try
            {
                var numParte = componenteFactoryServices.getComponenteService().getNumParte(idModelo, idSubconjunto).Split('/');
                List<ComboDTO> data = new List<ComboDTO>();
                for(int i = 0; i < numParte.Length; i++)
                {
                    ComboDTO aux = new ComboDTO();
                    aux.Value = (i + 1).ToString();
                    aux.Text = numParte[i];
                    data.Add(aux);
                }
                
                result.Add(ITEMS, data.Select(x => new { Value = x.Value, Text = x.Text }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult guardarModificacionesComponentes(int cicloVidaHoras, int garantia, int estatusNuevo, List<ComboDTO> cc, string descripcionComponente, string locacion, bool estatusActual, int subconjunto = -1, int modelo = -1) 
        {
            var result = new Dictionary<string, object>();
            try
            {
                componenteFactoryServices.getComponenteService().guardarModificaciones(cicloVidaHoras, garantia, estatusNuevo, cc, descripcionComponente, locacion, subconjunto, estatusActual, modelo);
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