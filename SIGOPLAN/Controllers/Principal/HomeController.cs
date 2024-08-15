using Core.DTO.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.DTO;
using Core.DTO.Principal.Generales;
using Data.Factory.Principal.Alertas;
using Core.Entity.Principal.Alertas;
using Data.Factory.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Inventario;
using Data.Factory.Principal.Usuarios;
using Data.Factory.Maquinaria.Captura;
using System.Globalization;

namespace SIGOPLAN.Controllers.Principal
{
    public class HomeController : BaseController
    {

        private AlertaFactoryServices alertaFactoryServices = new AlertaFactoryServices();
        private ResguardoEquipoFactoryServices resguardoEquipoFactoryServices = new ResguardoEquipoFactoryServices();
        private ControlEnvioyRecepcionFactoryServices controlEnvioyRecepcionFactoryService = new ControlEnvioyRecepcionFactoryServices();
        private UsuarioFactoryServices usuarioFactoryServices = new UsuarioFactoryServices();
        private KPIFactoryServices kpiFactoryServices = new KPIFactoryServices();

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Ayuda()
        {
            return View();
        }
        public ActionResult Dashboard()
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index", new { id = 1 });
            return View();
        }
        public ActionResult LayoutDashboard()
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index", new { id = 1 });
            return View();
        }
        public ActionResult Licencias()
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index", new { id = 1 });
            return View();
        }
        public ActionResult Polizas()
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index", new { id = 1 });
            return View();
        }
        public ActionResult Disponibilidad()
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index", new { id = 1 });
            return View();
        }
        public ActionResult Rendimiento()
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index", new { id = 1 });
            return View();
        }
        public ActionResult RecepcionMaquinaria()
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index", new { id = 1 });
            return View();
        }
        public ActionResult RecepcionProveedor()
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index", new { id = 1 });
            return View();
        }
        public ActionResult CursoManejo()
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index", new { id = 1 });
            return View();
        }
        public ActionResult EstadisticasLicencias()
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index", new { id = 1 });
            return View();
        }
        public ActionResult EstadisticasPolizas()
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index", new { id = 1 });
            return View();
        }
        public ActionResult EstadisticasDisponibilidad()
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index", new { id = 1 });
            return View();
        }
        public ActionResult EstadisticasRendimiento()
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index", new { id = 1 });
            return View();
        }
        public ActionResult EstadisticasRecepcionMaquinaria()
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index", new { id = 1 });
            return View();
        }
        public ActionResult EstadisticasRecepcionProveedor()
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index", new { id = 1 });
            return View();
        }
        public ActionResult EstadisticasCursoManejo()
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index", new { id = 1 });
            return View();
        }
        public ActionResult getResolucion(int width, int height)
        {
            var result = new Dictionary<string, object>();
            try
            {
                ResolutionDTO resolution = new ResolutionDTO();
                resolution.height = height;
                resolution.width = width;
                vSesiones.sesionCurrentResolution = resolution;
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getNotificaciones()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var alertas = alertaFactoryServices.getAlertaService().getAlertasByUsuarioAndSistema(getUsuario().id, 1);
                result.Add("alertas", alertas);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getNotificacionesLicencias()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuario = usuarioFactoryServices.getUsuarioService().getCCsUsuario(getUsuario().id);
                var licencias = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().getListaResguardosPendientesLicencia(usuario)
                     .Select(x => new
                     {
                         id = x.id,
                         noEmpleado = x.noEmpleado,
                         Obra = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().getCCByArea(x.Obra),
                         nombEmpleado = x.nombEmpleado,
                         diasVencimiento = DateTime.Today.Subtract(x.fechaVencimiento).Days,
                         Kilometraje = string.Format("{0:#,0}", x.Kilometraje),
                         kilometrajeRaw = x.Kilometraje,
                         Placas = x.Placas,
                         fechaVencimiento = x.fechaVencimiento.ToString("dd/MM/yyyy"),
                         fecha = x.fechaVencimiento,
                         mes = x.fechaVencimiento.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")),
                         anio = Convert.ToString(x.fechaVencimiento.Year),
                         maquina = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().getMaquinaByID(x.MaquinariaID),
                         maquinaNoEconomico = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().getNoEconomicoMaquinaByID(x.MaquinariaID)
                     }).OrderByDescending(x => x.diasVencimiento).ToList();
                result.Add("licencias", licencias);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getNotificacionesPolizas()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuario = usuarioFactoryServices.getUsuarioService().getCCsUsuario(getUsuario().id);
                var polizas = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().getListaResguardosPendientesPoliza(usuario)
                    .Select(x => new
                    {
                        id = x.id,
                        noEmpleado = x.noEmpleado,
                        Obra = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().getCCByArea(x.Obra),
                        nombEmpleado = x.nombEmpleado,
                        fechaVencimientoPoliza = x.fechaVencimientoPoliza.ToString("dd/MM/yyyy"),
                        fecha = x.fechaVencimientoPoliza,
                        diasVencimientoPoliza = DateTime.Today.Subtract(x.fechaVencimientoPoliza).Days,
                        Kilometraje = string.Format("{0:#,0}", x.Kilometraje),
                        kilometrajeRaw = x.Kilometraje,
                        Placas = x.Placas,
                        mes = x.fechaVencimientoPoliza.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")),
                        anio = Convert.ToString(x.fechaVencimientoPoliza.Year),
                        maquina = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().getMaquinaByID(x.MaquinariaID),
                        maquinaNoEconomico = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().getNoEconomicoMaquinaByID(x.MaquinariaID)
                    }).OrderByDescending(x => x.diasVencimientoPoliza).ToList();

                result.Add("polizas", polizas);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getNotificacionesDisponibilidad()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuario = usuarioFactoryServices.getUsuarioService().getCCsUsuario(getUsuario().id);
                var disponibilidad = kpiFactoryServices.getKPIFactoryService().indiceDisponibilidad(usuario)
                .Select
                (x => new
                {
                    id = x.id,
                    maquinaID = x.maquina.noEconomico,
                    descripcion = x.maquina.descripcion,
                    cc = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().getCCByArea(x.cc),
                    //cc = x.cc,
                    disponibilidad = x.disponibilidad > 100 ? "100%" : x.disponibilidad.ToString() + "%",
                    decimalDisponibilidad = x.disponibilidad > 100 ? 100 : x.disponibilidad,
                    fecha = x.fecha.ToString("dd/MM/yyyy"),
                }).OrderBy(x => x.decimalDisponibilidad).ToList();

                result.Add("disponibilidad", disponibilidad);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult getNotificacionesRendimiento()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var perfil = getUsuario().dashboardMaquinariaAdmin;
                var usuario = usuarioFactoryServices.getUsuarioService().getCCsUsuario(getUsuario().id);
                var rendimiento = kpiFactoryServices.getKPIFactoryService().alertasRendimiento(usuario)
                .Select
                (x => new
                {
                    id = x.id,
                    maquinaID = x.maquinaID,
                    economico = x.maquina.noEconomico,
                    descripcion = x.maquina.descripcion,
                    cc = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().getCCByArea(x.cc),
                    fecha = x.fecha.ToString("dd/MM/yyyy"),
                    modelo = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().getModeloByID(x.modelo),
                    rendimiento = x.rendimiento,
                    rendimientofinal = perfil == true ? x.rendimiento + (x.rendimiento * (decimal)0.05) : x.rendimiento,
                    AVGRendimiento = kpiFactoryServices.getKPIFactoryService().promedioRendimiento(x.cc, x.modelo)
                }).ToList();
                int j = 0;
                int validacion = rendimiento.Count();
                for (int i = 0; i < validacion; i++)
                {
                    if (rendimiento[i].rendimiento >= rendimiento[i].AVGRendimiento)
                    {
                        rendimiento.Add(rendimiento[i]);
                        j++;
                    }
                }
                var aux2 = rendimiento.GetRange(rendimiento.Count() - j, j);
                result.Add("rendimiento", aux2);
                result.Add(SUCCESS, true);

                //var perfil = getUsuario().dashboardMaquinariaAdmin;
                //var usuario = usuarioFactoryServices.getUsuarioService().getCCsUsuario(getUsuario().id);
                //var rendimiento = kpiFactoryServices.getKPIFactoryService().alertasRendimiento(usuario).GroupBy(x => new { x.cc, x.modelo })
                //.Select
                //(g => new
                //{
                //    cc = g.Key.cc,
                //    CCName = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().getCCByArea(g.Key.cc),
                //    modeloID = g.Key.modelo,
                //    modelo = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().getModeloByID(g.Key.modelo),
                //    AVGRendimiento = perfil == true ? g.Average(x => x.rendimiento) + (g.Average(x => x.rendimiento) * (decimal)0.05) : g.Average(x => x.rendimiento)
                //}).OrderBy(g => g.modelo).ToList();

                //result.Add("rendimiento", rendimiento);
                //result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getNotificacionesCursoManejo()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuario = usuarioFactoryServices.getUsuarioService().getCCsUsuario(getUsuario().id);
                var cursosVencidos = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().GetCursosManejoVencidos()
                     .Select(x => new
                     {
                         id = x.id,
                         noEmpleado = x.noEmpleado,
                         Obra = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().getCCByArea(x.Obra),
                         nombEmpleado = x.nombEmpleado,
                         diasVencimiento = DateTime.Today.Subtract(((DateTime)x.fechaVigenciaCurso)).Days,
                         Kilometraje = string.Format("{0:#,0}", x.Kilometraje),
                         kilometrajeRaw = x.Kilometraje,
                         //Placas = x.Placas,
                         fechaVencimiento = ((DateTime)x.fechaVigenciaCurso).ToString("dd/MM/yyyy"),
                         fecha = x.fechaVigenciaCurso,
                         mes = ((DateTime)x.fechaVigenciaCurso).ToString("MMMM", CultureInfo.CreateSpecificCulture("es")),
                         anio = Convert.ToString(((DateTime)x.fechaVigenciaCurso).Year),
                         maquina = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().getMaquinaByID(x.MaquinariaID),
                         maquinaNoEconomico = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().getNoEconomicoMaquinaByID(x.MaquinariaID)
                     }).OrderByDescending(x => x.diasVencimiento).ToList();
                result.Add("cursoManejo", cursosVencidos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private int CountRendimiento()
        {
            int aux = 0;
            var result = new Dictionary<string, object>();
            try
            {
                var perfil = getUsuario().dashboardMaquinariaAdmin;
                var usuario = usuarioFactoryServices.getUsuarioService().getCCsUsuario(getUsuario().id);
                var rendimiento = kpiFactoryServices.getKPIFactoryService().alertasRendimiento(usuario)
                .Select
                (x => new
                {
                    rendimiento = x.rendimiento,
                    AVGRendimiento = kpiFactoryServices.getKPIFactoryService().promedioRendimiento(x.cc, x.modelo)
                }).ToList();
                for (int i = 0; i < rendimiento.Count(); i++)
                {
                    if (rendimiento[i].rendimiento >= rendimiento[i].AVGRendimiento)
                    {
                        aux++;
                    }
                }
            }
            catch (Exception e)
            {
            }
            return aux;
        }

        public ActionResult getNotificacionesCount()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var idUsuario = getUsuario().id;
                var usuario = usuarioFactoryServices.getUsuarioService().getCCsUsuario(idUsuario);

                var licencias = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().getListaResguardosPendientesLicencia(usuario).Count;
                var polizas = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().getListaResguardosPendientesPoliza(usuario).Count;
                var disponibilidad = kpiFactoryServices.getKPIFactoryService().indiceDisponibilidad(usuario).Count;
                var rendimiento = CountRendimiento();
                var recepcion1 = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().GetListaControlesCalidad(2, usuario, 1, null, null, null, null).Where(x => x.noEconomicoID != 0).ToList().Count;
                var cursoManejo = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().GetCursosManejoVencidos().Count;

                try
                {
                    var recepcion2 = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().GetListaControlesCalidad(4, usuario, 1, null, null, null, null).Where(x => x.noEconomicoID != 0).ToList().Count;
                    result.Add("recepcion2", recepcion2);
                }
                catch (Exception e)
                {
                    result.Add("recepcion2", null);
                }

                result.Add("licencias", licencias);
                result.Add("polizas", polizas);
                result.Add("disponibilidad", disponibilidad);
                result.Add("rendimiento", rendimiento);
                result.Add("recepcion1", recepcion1);
                result.Add("cursoManejo", cursoManejo);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public void ProcessRequest()
        {
            Session["Heartbeat"] = DateTime.Now;
        }

        public ActionResult DashboardAdminFinanzas()
        {
            if (!Request.IsAjaxRequest())
                return RedirectToAction("Index", new { id = 1 });
            return View();
        }

    }
}