using Core.DAO.Administracion.ControlInterno;
using Core.DTO;
using Core.DTO.Administracion.ControlInterno.Obra;
using Core.DTO.Principal.Generales;
using Core.Entity.Administrativo.ControlInterno.Obra;
using Core.Entity.Principal.Multiempresa;
using Core.Enum.Administracion.ControlInterno.Obra;
using Core.Enum.Principal;
using Data.Factory.Administracion.ControlInterno;
using Infrastructure.Utils;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.ControlInterno
{
    public class ObraController : BaseController
    {
        #region init
        private IObraDAO obraFS;
        private Dictionary<string, object> response;
        private List<tblM_O_CatCCAC> lstObraSigoplan;
        /// <summary>
        /// Usuarios que pueden indicar que se actualizo en enkontrol
        /// </summary>
        private List<int> lstIdUsuarioEnkontrol = new List<int>() { 11, 1184 };
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            response = new Dictionary<string, object>();
            obraFS = new ObraFactoryServices().getObraServices();
            base.OnActionExecuting(filterContext);
        }
        #endregion
        #region Gestion
        public ActionResult Gestion()
        {
            return View();
        }
        public ActionResult getTblObras(BusqObraGestionDTO busq)
        {
            try
            {
                busq.verificaBusq();
                var lstCatalogo = obraFS.getLstObra(busq);
                var lst = ConvierteLstCatalogoALstViewModel(lstCatalogo);
                response.Add("lst", lst);
                response.Add(SUCCESS, lst.Count > 0);
            }
            catch(Exception o_O)
            {
                response.Add(SUCCESS, false);
                response.Add(MESSAGE, o_O.Message);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region _formObra
        public ActionResult _formObra()
        {
            return PartialView();
        }
        public ActionResult GuardarObra(List<tblM_O_CatCCAC> lst)
        {
            try
            {
                var esSucces = lst.Count > 0 && lst.All(catalogo =>  verificaCatalogoAGuardar(catalogo));
                if(esSucces)
                {
                    esSucces = obraFS.GuardarObra(lst);
                }
                response.Add(SUCCESS, esSucces);
            }
            catch(Exception o_O)
            {
                response.Add(SUCCESS, false);
                response.Add(MESSAGE, o_O.Message);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getFormDesdeClave(string clave)
        {
            try
            {
                var catalogo = obraFS.getFormDesdeClave(clave);
                var esSuccess = catalogo.id > 0;
                if(esSuccess)
                {
                    var catalogoVM = ConvierteCatalogoAViewModel(catalogo);
                    response.Add("catalogoVM", catalogoVM);
                }
                response.Add(SUCCESS, esSuccess);
            }
            catch(Exception o_O)
            {
                response.Add(SUCCESS, false);
                response.Add(MESSAGE, o_O.Message);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getAutocompleteClave(string term)
        {
            try
            {
                var lstCatalogo = obraFS.getAutocompleteClave(term);
                var lst = ConvierteLstCatalogoALstViewModel(lstCatalogo);
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
            catch(Exception o_O)
            {
                response.Add(SUCCESS, false);
                response.Add(MESSAGE, o_O.Message);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region combobox
        public ActionResult getCboObra()
        {
            try
            {
                init();
                var cboCC = from cc in lstObraSigoplan
                            where cc.tipo != tipoCatalogoEnum.AreaCuenta
                            orderby cc.clave
                            select new ComboDTO()
                            {
                                Text = string.Format("{0}-{1}", cc.clave, cc.descripcion),
                                Value = cc.clave
                            };
                var cboAC = from ac in lstObraSigoplan
                            where ac.tipo == tipoCatalogoEnum.AreaCuenta
                            orderby ac.clave
                            select new ComboDTO()
                            {
                                Text = string.Format("{0} {1}", ac.clave, ac.descripcion),
                                Value = ac.clave
                            };
                var cboTipo = EnumExtensions.ToCombo<tipoCatalogoEnum>();
                var cboAuth = EnumExtensions.ToCombo<authEstadoEnum>();
                var esSuccess = cboCC.Count() > 0 || cboAC.Count() > 0;
                response.Add("optionTipo", cboTipo);
                response.Add("optionAuth", cboAuth);
                response.Add("optionCC", cboCC);
                response.Add("optionAC", cboAC);
                response.Add(SUCCESS, esSuccess);
            }
            catch(Exception o_O)
            {
                response.Add(SUCCESS, false);
                response.Add(MESSAGE, o_O.Message);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCboTipoCatalogo()
        {
            try
            {
                var items = EnumExtensions.ToCombo<tipoCatalogoEnum>();
                var lstAuthEstado = EnumExtensions.ToCombo<authEstadoEnum>();
                response.Add(ITEMS, items);
                response.Add(SUCCESS, items.Count > 0 && lstAuthEstado.Count > 0);
                response.Add("itemsAuthEstado", lstAuthEstado);
            }
            catch(Exception o_O)
            {
                response.Add(SUCCESS, false);
                response.Add(MESSAGE, o_O.Message);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region auxiliares
        private bool verificaCatalogoAGuardar(tblM_O_CatCCAC catalogo)
        {
            var mensaje = string.Empty;
            var lstAuth = EnumExtensions.ToCombo<authEstadoEnum>().Select(a => a.Value);
            if(catalogo.descripcion == null || catalogo.descripcion.Length < 5)
            {
                mensaje += "La descripción es muy corta. ";
            }
            if(!lstAuth.Contains((int)catalogo.authEstado) || !catalogo.lstAuth.All(auth => lstAuth.Contains((int)auth.authEstado)))
            {
                mensaje += "El estado de la autorización no está correcto. ";
            }
            if(catalogo.authEstado == authEstadoEnum.Rechazado && catalogo.lstAuth.Any(a => a.authEstado == authEstadoEnum.Rechazado && a.motivoRechazo.Length < 5))
            {
                mensaje += "Escriba el motivo del rechazo. ";
            }
            switch(catalogo.tipo)
            {
                case tipoCatalogoEnum.AreaCuenta:
                    if(catalogo.clave == null || !catalogo.clave.Contains("-") || catalogo.clave.Split('-').Length != 2)
                    {
                        mensaje += "La clave no tiene formato correcto para Area Cuenta. ";
                    }
                    break;
                case tipoCatalogoEnum.DepartamentoAdministrativo:
                case tipoCatalogoEnum.DepartamentoDeNomina:
                    if(catalogo.clave == null || !catalogo.clave.All(clave => char.IsNumber(clave)) || catalogo.clave.All(clave => char.IsLetter(clave)) || catalogo.clave.Contains("-") || catalogo.clave.Length != 3)
                    {
                        mensaje += "La clave no tiene el formato correcto para " + catalogo.tipo.GetDescription() + ". ";
                    }
                    break;
                default:
                    mensaje += "No existe el tipo para el catálogo.  ";
                    break;
            }
            if(mensaje.Length > 0)
            {
                throw new System.InvalidOperationException(mensaje);
            }
            return mensaje.Length == 0;
        }
        private List<object> ConvierteLstCatalogoALstViewModel(List<tblM_O_CatCCAC> lstCatalogo)
        {
            return (from catalogo in lstCatalogo select ConvierteCatalogoAViewModel(catalogo)).ToList();
        }
        private object ConvierteCatalogoAViewModel(tblM_O_CatCCAC catalogo)
        {
            return new
            {
                catalogo.id,
                catalogo.idUsuarioRegistro,
                catalogo.clave,
                catalogo.descripcion,
                catalogo.tipo,
                catalogo.fechaArranque,
                catalogo.authEstado,
                catalogo.exiteEnkontrol,
                catalogo.esActivo,
                lstAuth = (from auth in catalogo.lstAuth
                           select new
                           {
                               auth.id,
                               auth.idCatalogo,
                               auth.idUsuario,
                               auth.orden,
                               nombre = string.Format("{0} {1} {2}", auth.usuario.nombre, auth.usuario.apellidoPaterno, auth.usuario.apellidoMaterno),
                               auth.motivoRechazo,
                               auth.authEstado,
                           }).ToList(),
                #region autocompletado
                label = catalogo.clave, //autocompletado
                value = catalogo.clave, //autocompletado
                #endregion
                #region indicadores de autorizacion
                puedeDarVobo = catalogo.lstAuth.Any(a => a.authEstado == authEstadoEnum.EnTurno && a.orden == 1 && a.idUsuario == vSesiones.sesionUsuarioDTO.id),
                puedeDarAuth = catalogo.lstAuth.Any(a => a.authEstado == authEstadoEnum.EnTurno && a.orden == 2 && a.idUsuario == vSesiones.sesionUsuarioDTO.id) && catalogo.lstAuth.Any(a => a.authEstado == authEstadoEnum.Autorizado && a.orden == 1),
                puedeDarEnkoltrol = catalogo.authEstado == authEstadoEnum.Autorizado && !catalogo.exiteEnkontrol && lstIdUsuarioEnkontrol.Contains(vSesiones.sesionUsuarioDTO.id)
                #endregion
            };
        }
        void init()
        {
            if(lstObraSigoplan != null)
            {
                return;
            }
            lstObraSigoplan = obraFS.getAllObra();
        }
        #endregion
    }
}