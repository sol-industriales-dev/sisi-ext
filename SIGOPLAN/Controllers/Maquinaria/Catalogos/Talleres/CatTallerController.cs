using Core.Entity.Maquinaria.Catalogo;
using Data.DAO.Maquinaria.Catalogos;
using Data.DAO.RecursosHumanos.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Catalogos.Talleres
{
    public class CatTallerController : BaseController
    {

        #region Atributos
        private readonly string SUCCESS = "success";
        private readonly string MESSAGE = "message";
        private const string PAGE = "page";
        private const string TOTAL_PAGE = "total";
        private const string ROWS = "rows";
        private const string ITEMS = "items";
        #endregion

        #region Factory
        //CentroCostosFactoryServices centroCostosFactoryServices;

        #endregion
        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    centroCostosFactoryServices = new CentroCostosFactoryServices();
        //}

        // GET: CatTaller
        public ActionResult Index()
        {
            ViewBag.pagina = "catalogo";
            return View();
        }

        public ActionResult FillGridTaller(tblM_CentroCostos obj)
        {

            var result = new Dictionary<string, object>();
            try
            {
                TallerDAO cc = new TallerDAO();
                var listResult = cc.FillGridTaller(obj);

                result.Add("current", 1);
                result.Add("rowCount", 1);
                result.Add("total", listResult.Count());
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

        public ActionResult fillCboDato()
        {

            var result = new Dictionary<string, object>();
            try
            {
                CatPuestosDAO cc = new CatPuestosDAO();
                var listResult = cc.FillGridTaller();



                foreach (var item in listResult)
                {
                    string Data = "";

                    var descripcionAux = item.descripcion.Split(' ');

                    int ultimoDato = descripcionAux.Length;

                    if (descripcionAux[ultimoDato].Length-1 == 1)
                    {
                        for (int i = 0; i < descripcionAux.Length - 1; i++)
                        {
                            Data += descripcionAux[i];
                        }
                    }

                }

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


    }
}