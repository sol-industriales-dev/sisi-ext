using Core.DAO.RecursosHumanos.CatNotificantes;
using Core.DTO.RecursosHumanos.CatNotificantes;
using Data.Factory.RecursosHumanos.CatNotificantes;
using SIGOPLAN.Controllers;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.RecursosHumanos.CatNotificantes
{
    public class CatNotificantesController : BaseController
    {

        Dictionary<string, object> result = new Dictionary<string, object>();
        public ICatNotificantesDAO catNotificantesInterfaz;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            catNotificantesInterfaz = new CatNotificantesFactoryService().GetCatNotificantesService();
            base.OnActionExecuting(filterContext);
        }

        #region VISTAS
        public ActionResult CatNotificantes()
        {
            return View();
        }
        #endregion

        #region CAT NOTIFICANTES

        public ActionResult GetNotificantes()
        {
            return Json(catNotificantesInterfaz.GetNotificantes(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetNotificantesDet(string cc, int idConcepto)
        {
            return Json(catNotificantesInterfaz.GetNotificantesDet(cc, idConcepto), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarNotificantes(string cc, int idConcepto, List<int> lstUsuariosNuevos)
        {
            return Json(catNotificantesInterfaz.CrearEditarNotificantes(cc, idConcepto, lstUsuariosNuevos), JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemoveNotificante(int idRelNoti)
        {
            return Json(catNotificantesInterfaz.RemoveNotificante(idRelNoti), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region FILLCOMBO
        public ActionResult FillCboCC()
        {
            return Json(catNotificantesInterfaz.FillCboCC(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboUsuarios()
        {
            return Json(catNotificantesInterfaz.FillCboUsuarios(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboConceptos()
        {
            return Json(catNotificantesInterfaz.FillCboConceptos(), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}