using Core.DTO;
using Core.DTO.Captura;
using Core.Entity.Maquinaria;
using Core.Entity.Maquinaria.Captura;
using Data.Factory.Maquinaria.Captura;
using Data.Factory.Maquinaria.Catalogos;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.DTO.Maquinaria.Captura;
using Core.DTO.Principal.Generales;
using Data.Factory.Principal.Usuarios;

namespace SIGOPLAN.Controllers.Maquinaria.Capturas.Diarias
{
    public class HorometrosController : BaseController
    {
        // GET: Horometros
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CapturaHorometro()
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

        public ActionResult ReporteHorometro()
        {
            return View();
        }

        #region Factory
        RitmoHorometroFactoryServices ritmoHorometroFactoryServices;
        CapturaHorometroFactoryServices capturaHorometroFactoryServices;
        DesfaseFactoryServices desfaseFactoryServices;
        MaquinaFactoryServices maquinaFactoryServices;
        UsuarioFactoryServices usuarioFactoryServices;
        CentroCostosFactoryServices centroCostosFactoryServices;

        #endregion
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            capturaHorometroFactoryServices = new CapturaHorometroFactoryServices();
            ritmoHorometroFactoryServices = new RitmoHorometroFactoryServices();
            desfaseFactoryServices = new DesfaseFactoryServices();
            maquinaFactoryServices = new MaquinaFactoryServices();
            usuarioFactoryServices = new UsuarioFactoryServices();
            centroCostosFactoryServices = new CentroCostosFactoryServices();

            base.OnActionExecuting(filterContext);
        }



        public ActionResult getTableData1(string obj, int turno, DateTime fecha, int Tipo)
        {
            var result = new Dictionary<string, object>();
            //try
            //{
            var res = capturaHorometroFactoryServices.getCapturaHorometroServices().getDataTable(obj, turno, fecha, Tipo).OrderByDescending(x => x.habilidatado).ThenBy(x => x.Economico);
            var cc = capturaHorometroFactoryServices.getCapturaHorometroServices().getCentroCostos(obj);
            // --> Checar si existe registro de guardado para ac en corte semanal kubrix
            bool corteKubrix = capturaHorometroFactoryServices.getCapturaHorometroServices().getCorteKubrixAC(obj);

            Session["ReporteHorometro"] = res.ToList();

            result.Add("centroCostos", cc);
            result.Add(SUCCESS, true);
            result.Add("maquinasHorometro", res);
            result.Add("corteKubrix", corteKubrix);
            //}
            //catch (Exception e)
            //{
            //    result.Add(MESSAGE, e.Message);
            //    result.Add(SUCCESS, false);
            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getDataRitmo(string obj)
        {

            var result = new Dictionary<string, object>();
            try
            {
                var res = ritmoHorometroFactoryServices.RitmoHorometroServices().CapRitmoHorometro(obj);

                result.Add(SUCCESS, true);
                result.Add("dataRitmo", res);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getDataDesfase(string obj)
        {

            var result = new Dictionary<string, object>();
            try
            {

                var res = desfaseFactoryServices.CapturaDesfaseService().getDesfase(obj);

                result.Add(SUCCESS, true);
                result.Add("dataDesfase", res);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult cboModalEconomico(string obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, maquinaFactoryServices.getMaquinaServices().getCboMaquinaria(obj).Select(x => new { Value = x.noEconomico, Text = x.noEconomico }).OrderBy(x => x.Text));
                if (obj != "")
                {
                    //result.Add(ITEMS, desfaseFactoryServices.CapturaDesfaseService().getEconomicos(obj).Select(x => new { Value = x.descripcion, Text = x.descripcion }).OrderBy(x => x.Text));

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

        public ActionResult SaveOrUpdate_Horometros(List<tblM_CapHorometro> array)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (array != null)
                {

                    foreach (var item in array)
                    {
                        item.FechaCaptura = DateTime.Now;
                    }

                    capturaHorometroFactoryServices.getCapturaHorometroServices().Guardar(array);
                    //capturaHorometroFactoryServices.getCapturaHorometroServices().GuardarHorasComponente(array);
                    var updated = capturaHorometroFactoryServices.getCapturaHorometroServices().getUpdatedStandBy(array.Select(x => x.Economico).ToList(), 1);
                    if (updated.Count > 0)
                    {
                        string st = "Los siguientes equipos estaban en StandBy y se regresaron a estatus operativo : " + string.Join(",", updated);

                        result.Add(MESSAGE, st);
                    }
                    else
                    {
                        result.Add(MESSAGE, GlobalUtils.getMensaje(1));
                    }

                    result.Add(SUCCESS, true);
                }
                else
                {
                    result.Add(MESSAGE, "La captura se encuentra vacía  favor de verificarla");
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

        public ActionResult SaveOrUpdate_Ritmo(tblM_CapRitmoHorometro obj, bool ritmo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                ritmoHorometroFactoryServices.RitmoHorometroServices().GuardarRitmo(obj);
                tblM_CapHorometro datoHorometro = capturaHorometroFactoryServices.getCapturaHorometroServices().getDatoHorometro(obj.economico);
                datoHorometro.Ritmo = ritmo;
                capturaHorometroFactoryServices.getCapturaHorometroServices().Guardar(datoHorometro);


                result.Add(MESSAGE, GlobalUtils.getMensaje(1));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveOrUpdate_Desfase(tblM_CapDesfase obj)
        {
            var result = new Dictionary<string, object>();
            try
            {

                desfaseFactoryServices.CapturaDesfaseService().Guardar(obj);

                var getTotalDesfase = desfaseFactoryServices.CapturaDesfaseService().getDesfase(obj.Economico).horasDesfaseAcumulado;
                var horometroEconomico = capturaHorometroFactoryServices.getCapturaHorometroServices().getUltimoHorometro(obj.Economico);

                horometroEconomico.Desfase = obj.horasDesfase;
                horometroEconomico.HorometroAcumulado = horometroEconomico.HorometroAcumulado + getTotalDesfase;
                horometroEconomico.HorasTrabajo = 0;
                horometroEconomico.FechaCaptura = DateTime.Now;
                horometroEconomico.turno = 0;
                horometroEconomico.Desfase = getTotalDesfase;
                horometroEconomico.Horometro = 0;

                capturaHorometroFactoryServices.getCapturaHorometroServices().Guardar(horometroEconomico);

                result.Add(MESSAGE, GlobalUtils.getMensaje(1));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getCentroCostos(string obj)
        {

            var result = new Dictionary<string, object>();
            try
            {
                var res = capturaHorometroFactoryServices.getCapturaHorometroServices().getCentroCostos(obj);

                result.Add(SUCCESS, true);
                result.Add("centroCostos", res);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult cboCentroCostos()
        {

            var result = new Dictionary<string, object>();
            try
            {
                var cbo = capturaHorometroFactoryServices.getCapturaHorometroServices().cboCentroCostos();
                result.Add(SUCCESS, cbo.Count > 0);
                result.Add(ITEMS, cbo);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReporteCapturaHorometros()
        {
            return View();
        }
        public ActionResult ReporteEconomicosSinHorometros()
        {
            return View();
        }

        public ActionResult GetInfoReporteCapturaHorometros(string cc, int turno, DateTime fechaInicia, DateTime fechaFinal, string economico, string ccFiltro, int grupo, int modelo, decimal hInicial, decimal hFinal, bool estatus)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var res = capturaHorometroFactoryServices.getCapturaHorometroServices().getTableInfoHorometros(
                    cc, turno, fechaInicia, fechaFinal, economico, ccFiltro, grupo, modelo, hInicial, hFinal, estatus
                ).OrderByDescending(x => x.Fecha).ThenByDescending(x => x.turno);
                var getHorometroMayor = res.Where(x => x.Horometro >= hInicial && x.Horometro <= hFinal).GroupBy(x => x.Economico).ToList();
                var resultHorometroMayor = getHorometroMayor.Select(x => new
                {
                    Fecha = x.OrderByDescending(y => y.Horometro).FirstOrDefault().Fecha.ToShortDateString(),
                    Economico = x.OrderByDescending(y => y.Horometro).FirstOrDefault().Economico,
                    HorometroActual = x.OrderByDescending(y => y.Horometro).FirstOrDefault().Horometro,
                    Desfase = x.OrderByDescending(y => y.Horometro).FirstOrDefault().Desfase,
                    HorometroAcumulado = x.OrderByDescending(y => y.Horometro).FirstOrDefault().HorometroAcumulado,
                    Turno = x.OrderByDescending(y => y.Horometro).FirstOrDefault().turno,
                    CC = x.OrderByDescending(y => y.Horometro).FirstOrDefault().CC
                }).OrderBy(x => x.Economico).ToList();

                if (ccFiltro == "")
                {
                    var envioInfo = res.Select(h => new
                    {
                        Fecha = h.Fecha.ToString("dd/MM/yyyy"),
                        Economico = h.Economico,
                        Horometro = (h.Horometro - h.HorasTrabajo) <= 0 ? 0 : (h.Horometro - h.HorasTrabajo),
                        HorometroDesc = ((h.Horometro - h.HorasTrabajo) <= 0 ? 0 : (h.Horometro - h.HorasTrabajo)) + GetTipoDato(h.Economico),
                        horasTrabajadas = h.HorasTrabajo,
                        HorometroActual = h.Horometro,
                        Desfase = h.Desfase,
                        HorometroAcumulado = h.HorometroAcumulado,
                        HorometroAcumuladoDesc = h.HorometroAcumulado + GetTipoDato(h.Economico),
                        Turno = h.turno,
                        CC = h.CC,
                        HorasTrabajo = h.HorasTrabajo
                    }).ToList();

                    result.Add("infoHorometros", envioInfo);

                    var horometros = envioInfo.Select(h => new CapHorometroDTO
                    {
                        fechaFormat = h.Fecha,
                        Economico = h.Economico,
                        Horometro = h.Horometro,
                        HorometroDesc = h.HorometroDesc,
                        HorasTrabajo = h.HorasTrabajo,
                        HorometroActual = h.Horometro + h.HorasTrabajo,
                        HorometroActualDesc = (h.Horometro + h.HorasTrabajo) + GetTipoDato(h.Economico),
                        Desfase = h.Desfase,
                        HorometroAcumulado = h.HorometroAcumulado,
                        HorometroAcumuladoDesc = h.HorometroAcumuladoDesc,
                        turno = h.Turno
                    }).ToList();

                    var total = envioInfo.Sum(x => x.HorometroActual);
                    var totalHoras = envioInfo.Sum(x => x.horasTrabajadas);

                    result.Add("totalHoras", totalHoras);
                    Session["ReporteHorometro"] = horometros;
                }
                else
                {
                    var infoHOro = res.Where(x => x.CC.Equals(ccFiltro)).Select(h => new
                    {
                        Fecha = h.Fecha.ToString("dd/MM/yyyy"),
                        Economico = h.Economico,
                        Horometro = ((h.Horometro - h.HorasTrabajo) <= 0 ? 0 : (h.Horometro - h.HorasTrabajo)),
                        HorometroDesc = ((h.Horometro - h.HorasTrabajo) <= 0 ? 0 : (h.Horometro - h.HorasTrabajo)) + GetTipoDato(h.Economico),
                        horasTrabajadas = h.HorasTrabajo,
                        HorometroActual = h.Horometro,
                        Desfase = h.Desfase,
                        HorometroAcumulado = h.HorometroAcumulado,
                        HorometroAcumuladoDesc = h.HorometroAcumulado + GetTipoDato(h.Economico),
                        Turno = h.turno,
                        CC = h.CC
                    });
                    var total = infoHOro.Sum(x => x.HorometroActual);
                    var totalHoras = infoHOro.Sum(x => x.horasTrabajadas);

                    result.Add("infoHorometros", infoHOro.OrderBy(x => x.Fecha).ThenBy(x => x.Horometro).ThenByDescending(x => x.Turno));

                    var horometros = infoHOro.Select(h => new CapHorometroDTO
                    {
                        fechaFormat = h.Fecha,
                        Economico = h.Economico,
                        Horometro = ((h.Horometro - h.horasTrabajadas) > 0 ? (h.Horometro - h.horasTrabajadas) : 0),
                        HorometroDesc = ((h.Horometro - h.horasTrabajadas) > 0 ? (h.Horometro - h.horasTrabajadas) : 0) + GetTipoDato(h.Economico),
                        HorasTrabajo = h.horasTrabajadas,
                        HorometroActual = h.Horometro,
                        HorometroActualDesc = h.Horometro + GetTipoDato(h.Economico),
                        Desfase = h.Desfase,
                        HorometroAcumulado = h.HorometroAcumulado,
                        HorometroAcumuladoDesc = h.HorometroAcumulado + GetTipoDato(h.Economico),
                        turno = h.Turno
                    }).ToList();

                    Session["ReporteHorometro"] = horometros;
                    result.Add("totalHoras", totalHoras);
                }

                result.Add("tblHorometroMayor", resultHorometroMayor);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            var json = Json(result, JsonRequestBehavior.AllowGet);

            json.MaxJsonLength = int.MaxValue;

            return json;
        }

        public ActionResult GetEconomicosSinHorometros(string areaCuenta, string economico, DateTime fechaInicio)
        {
            return Json(capturaHorometroFactoryServices.getCapturaHorometroServices().GetEconomicosSinHorometros(areaCuenta, economico, fechaInicio), JsonRequestBehavior.AllowGet);
        }

        private string GetTipoDato(string noEconomico)
        {
            var tipoDato = "";

            switch (noEconomico.Split('-')[0])
            {
                case "PU":
                case "TA":
                case "TRA":
                case "CAP":
                case "CV":
                case "CGA":
                case "TU":
                case "CP":
                case "PD":
                case "OR":
                case "CEX":
                case "CLL":
                case "CSE":
                case "CPE":
                    tipoDato = " KM";
                    break;
                default:
                    tipoDato = " HR";
                    break;
            }

            switch (noEconomico)
            {
                case "CGA-03":
                    tipoDato = " HR";
                    break;
                case "PD-22":
                case "CEX-05":
                case "CEX-06":
                case "OR-35":
                case "CM-03":
                case "OR-38":
                case "CLL-07":
                case "CUA-03":
                    tipoDato = " KM";
                    break;
                default:
                    break;
            }

            return tipoDato;
        }

        public ActionResult GetCentroCosto(string obj, DateTime fechaInicia, DateTime fechaFinal)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var res = capturaHorometroFactoryServices.getCapturaHorometroServices()
                         .getListaCentrosCostos(fechaInicia, fechaFinal, obj).Distinct().Distinct().GroupBy(p => new { p.folio })
                          .Select(g => g.First())
                          .ToList();

                var CentrosDeCostos = res.Select(x => new { Value = x.folio, Text = capturaHorometroFactoryServices.getCapturaHorometroServices().getCentroCostos(x.folio) }).Distinct();

                result.Add(ITEMS, CentrosDeCostos);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetReporteHorometro(List<string> cc, DateTime fechaInicio, DateTime fechaFin)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var listaHorometros = capturaHorometroFactoryServices.getCapturaHorometroServices().getReporteHorometro(cc, fechaInicio, fechaFin);
                Session["listaHorometros"] = listaHorometros;

                result.Add(SUCCESS, true);
                result.Add("listaHorometros", listaHorometros);

            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public MemoryStream Download()
        {
            var listaHorometros = ((List<EconomicosHrsDTO>)Session["listaHorometros"]).ToList();
            var stream = capturaHorometroFactoryServices.getCapturaHorometroServices().exportarArchvio(listaHorometros);

            if (stream != null)
            {
                this.Response.Clear();
                this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "Horometros.xlsx"));
                this.Response.BinaryWrite(stream.ToArray());
                this.Response.End();

                return stream;
            }
            else
            {
                return null;
            }
        }
        public ActionResult ObtenerCentrosCostos()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuarioID = getUsuario().id;


                var listaCCusuario = usuarioFactoryServices.getUsuarioService().getCCsUsuario(usuarioID).Select(x => x.cc);


                var listaCentroCostosActuales = centroCostosFactoryServices.getCentroCostosService().getListaCC();
                if (base.getAction("AllCC"))
                {

                    result.Add(ITEMS, listaCentroCostosActuales.Select(y => new
                    {
                        Value = y.Value,
                        Text = y.Text,
                        Prefijo = y.Prefijo

                    }).OrderBy(x => x.Value));
                }
                else
                {
                    List<ComboDTO> Resultado = listaCentroCostosActuales.Where(x => listaCCusuario.Contains(x.Value)).Select(y => new ComboDTO
                    {
                        Value = y.Value,
                        Text = y.Text
                    }).ToList();

                    if (listaCCusuario.Contains("1010"))
                    {
                        Resultado.Add(new ComboDTO
                        {
                            Value = "1010",
                            Text = "1010-TALLER DE MAQUINARIA."
                        });
                    }
                    if (listaCCusuario.Contains("1015"))
                    {
                        Resultado.Add(new ComboDTO
                        {
                            Value = "1015",
                            Text = "1015-PATIO DE MAQUINARIA."
                        });
                    }

                    result.Add(ITEMS, Resultado.OrderBy(x => x.Value));
                }

                //result.Add(ITEMS, maquinaFactoryServices.getMaquinaServices().getCboMaquinaria(obj).Select(x => new { Value = x.noEconomico, Text = x.noEconomico }).OrderBy(x => x.Text));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarCorteKubrixAC(string AC)
        {
            var result = new Dictionary<string, object>();
            try
            {
                bool corteKubrix = capturaHorometroFactoryServices.getCapturaHorometroServices().getCorteKubrixAC(AC);
                if (!corteKubrix)
                {
                    bool guardadoCorteKubrixAC = capturaHorometroFactoryServices.getCapturaHorometroServices().GuardarCorteKubrixAC(AC);
                    if (guardadoCorteKubrixAC)
                    {
                        var fechaFin = DateTime.Today;
                        while (fechaFin.DayOfWeek != DayOfWeek.Wednesday)
                            fechaFin = fechaFin.AddDays(+1);
                        fechaFin.AddDays(-1);
                        var fechaInicio = fechaFin.AddDays(-7);

                        var correo = new Infrastructure.DTO.CorreoDTO();
                        System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                        var bytesReporteHorometros = capturaHorometroFactoryServices.getCapturaHorometroServices().GetReporteHorometrosKubrix(AC);
                        Stream streamReporteHorometros = new MemoryStream(bytesReporteHorometros);
                        System.Net.Mail.Attachment reporteHorometros = new System.Net.Mail.Attachment(streamReporteHorometros, ct);
                        reporteHorometros.ContentDisposition.FileName = "horometros.xlsx";
                        correo.archivos.Add(reporteHorometros);
                        var descripcionAC = capturaHorometroFactoryServices.getCapturaHorometroServices().getCentroCostos(AC);

                        correo.asunto = "Captura de horometros " + AC + " " + descripcionAC + " -- Periodo: " + fechaInicio.ToString("dd/MM/yyyy") + " - " + fechaFin.AddDays(-1).ToString("dd/MM/yyyy") + " para corte semanal Kubrix";
                        var correosPpales = capturaHorometroFactoryServices.getCapturaHorometroServices().GetCorreoGerenteAdmin(AC);
                        correo.correos.AddRange(correosPpales);
                        correo.correos.Add("laura.olivas@construplan.com.mx");
                        correo.correos.Add("francisco.salazar@construplan.com.mx");
                        correo.correos.Add("ramon.hernandez@construplan.com.mx");
                        //correo.correos.Add("laura.rodriguez@construplan.com.mx");
                        correo.correos.Add("e.encinas@construplan.com.mx");
                        correo.correos.Add("martin.valle@construplan.com.mx");
                        correo.correos.Add("martin.zayas@construplan.com.mx");
                        correo.correos.Add("rene.olea@construplan.com.mx");
                        correo.correos.Add("oscar.roman@construplan.com.mx");
                        correo.cuerpo =
                            "Buen día<br><br>" +
                            "Se notifica de la captura de horometros para reporte Kubrix semanal<br><br>" +
                            "Obra: " + AC + " " + descripcionAC + "<br>" +
                            "Periodo: " + fechaInicio.ToString("dd/MM/yyyy") + " - " + fechaFin.AddDays(-1).ToString("dd/MM/yyyy") + "<br>" +
                            "Usuario Generó: " + vSesiones.sesionUsuarioDTO.nombre + " " + vSesiones.sesionUsuarioDTO.apellidoPaterno + " " + vSesiones.sesionUsuarioDTO.apellidoMaterno;
                        correo.Enviar();
                        result.Add(SUCCESS, true);
                    }
                    else
                    {
                        result.Add(MESSAGE, "No se puede guardar el registro especificado. Favor de intentar de nuevo.");
                        result.Add(SUCCESS, false);
                    }
                }
                else
                {
                    result.Add(MESSAGE, "Ya existe registro de guardado en la semana actual para el AC establecido.");
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

    }
}