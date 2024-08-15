using Core.Entity.Maquinaria.Captura;
using Data.Factory.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Maquinaria.Capturas.Diarias
{
    public class EficienciaController : BaseController
    {
        EficienciaFactoryService eficienciaFactoryService;
        CapturaHorometroFactoryServices capturaHorometroFactoryServices;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            eficienciaFactoryService = new EficienciaFactoryService();
            capturaHorometroFactoryServices = new CapturaHorometroFactoryServices();
            base.OnActionExecuting(filterContext);
        }
        // GET: Eficiencia
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult getcboHorometro(string Econ, DateTime Fecha, string cc)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var Horometro = capturaHorometroFactoryServices.getCapturaHorometroServices().getHorometrosEficiencia(Econ, Fecha, cc);
                var lstHorometro = Horometro.Select(x => new { Text = x.Fecha.ToShortDateString() + " " + x.turno + " " + x.Horometro, Value = x.Horometro}).ToList();
                result.Add(ITEMS, lstHorometro);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getcboTurno(string Econ, DateTime Fecha, string cc)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var Horometro = capturaHorometroFactoryServices.getCapturaHorometroServices().getHorometrosEficiencia(Econ, Fecha, cc);
                var lstTurno = Horometro.Select(x => new { Text = x.turno, Value = x.turno }).Distinct().ToList();
                result.Add(ITEMS, lstTurno);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardaEficiencia(tblM_Eficiencia obj)
        {
            tblM_Eficiencia guardado = eficienciaFactoryService.getEficienciaService().GuardaEficiencia(obj);
            return Json(guardado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getTablaEficiencia(DateTime FechaInicioFiltro,DateTime FechaUltimoFiltro, string cc)
        {
            var data = eficienciaFactoryService.getEficienciaService().getTablaEficiencia(FechaInicioFiltro, FechaUltimoFiltro, cc);
            var lstEficiencia = data.Select(x => new
            {
                id = x.id,
                Economico = x.Economico,
                Fecha = x.Fecha.ToString("yyyy-MM-dd"),
                Turno = x.Turno,
                HrsTrabajada = x.HrsTrabajada,
                HrsTotalReparacion = x.HrsTotalReparacion,
                Paro = x.Paro,
                Semana = x.Semana,
                Comentarios = x.Comentarios,
                IdGrupo = x.IdGrupo
            }).OrderByDescending(x => x.IdGrupo).ToList();

            return Json(lstEficiencia, JsonRequestBehavior.AllowGet);
        }
    }
}