using Core.Entity.Kubrix;
using Core.Entity.Kubrix.Analisis;
using Data.Factory.Kubrix;
using Data.Factory.Maquinaria.Catalogos;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Kubrix.Controllers
{
    public class CatalogoController : BaseController
    {
        #region Factory
        CatalogoFactoryService CatFS = new CatalogoFactoryService();
        CentroCostosFactoryServices CcFS = new CentroCostosFactoryServices();
        #endregion
        #region View
        // GET: Kubrix/Catalogo
        public ActionResult CentroCostos()
        {
            ViewBag.cc = string.Empty;
            ViewBag.idDiv = 0;
            return View();
        }
        public ActionResult Bal12()
        {
            Session["cc"] = "001";
            return View();
        }
        #endregion
        #region PartialView
        public ActionResult _tblCC()
        {
            return PartialView();
        }
        public ActionResult _tblBal12(string cc)
        {
            Session["cc"] = cc;
            return PartialView();
        }
        public ActionResult _CapturaBal12()
        {
            return PartialView();
        }
        #endregion
        #region cbo
        public ActionResult getCboDivision()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, CatFS.getCatalogoService().getCboDivision());
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
        public List<object> getlstCcDiv(string cc, int idDiv)
        {
            var lstDiv = CatFS.getCatalogoService().getLstDiv();
            var lst = CatFS.getCatalogoService().getlstCcDiv(cc, idDiv)
                .Select(x => new { 
                    cc = x.cc,
                    NombreUne = CcFS.getCentroCostosService().getNombreCCFix(x.cc),
                    idDiv = x.idDivision,
                    division = lstDiv.FirstOrDefault(w => w.id == x.idDivision).Divsion,
                    estatus = x.estatus
                })
                .Cast<object>()
                .ToList();
            return lst;
        }
        public List<tblK_Bal12> getlstBal12(string cc)
        {
            return CatFS.getCatalogoService().getlstBal12(cc);
        }
    }
}