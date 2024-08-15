using Core.DAO.Maquinaria.Catalogos;
using Core.DTO;
using Core.DTO.Maquinaria.Captura;
using Core.DTO.Maquinaria.Catalogos;
using Core.DTO.Maquinaria.Reporte;
using Core.Entity.Maquinaria.Captura;
using Core.Enum.Maquinaria.CargoNomina;
using Data.Factory.Maquinaria.Catalogos;
using Data.Factory.Principal.Archivos;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Maquinaria.Reportes
{
    public class RepCargoNominaCCArrendadoraController : BaseController
    {
        private MaquinaFactoryServices maquinaFactoryServices = new MaquinaFactoryServices();
        private ArchivoFactoryServices archivofs = new ArchivoFactoryServices();
        CentroCostosFactoryServices centroCostosFactoryServices;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            maquinaFactoryServices = new MaquinaFactoryServices();
            archivofs = new ArchivoFactoryServices();
            centroCostosFactoryServices = new CentroCostosFactoryServices();
            base.OnActionExecuting(filterContext);
        }

        // GET: RepCargoNominaCCArrendadora
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Gestion()
        {
            return View();
        }

        public ActionResult GestionMensual()
        {
            return View();
        }

        public ActionResult GetProyectosList()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckPeriodoCapturado(string proyecto, string periodoInicial, string periodoFinal)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var check = maquinaFactoryServices.getMaquinaServices().checkPeriodoCapturado(proyecto, Convert.ToDateTime(periodoInicial), Convert.ToDateTime(periodoFinal));

                result.Add("capturado", check);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetEconomicos(string Proyectos, string periodoInicial, string periodoFinal, string nominaSemanal)
        {
            List<string> arrProyectos = new List<string>();
            var result = new Dictionary<string, object>();
            //try
            //{
                var isPeriodoActivo = maquinaFactoryServices.getMaquinaServices().existPeriodoNomina(Proyectos, Convert.ToDateTime(periodoInicial), Convert.ToDateTime(periodoFinal));
                if (isPeriodoActivo)
                {
                    var obj = maquinaFactoryServices.getMaquinaServices().getNominaCC(Proyectos, Convert.ToDateTime(periodoInicial), Convert.ToDateTime(periodoFinal));
                    var data = maquinaFactoryServices.getMaquinaServices().getNominaCCDet(obj.id);

                    arrProyectos = maquinaFactoryServices.getMaquinaServices().getNominaCCArrProyectos(obj.id);
                    var sumaHHPeriodo = data.Select(x => x.hh).Sum();
                    result.Add("data", data.Select(e => new RepCargoNominaCCArreDTO
                    {
                        economicoID = e.idEconomico,
                        noEconomico = e.economico,
                        descripcion = e.descripcion,
                        cc = e.cc,
                        hhPeriodo = e.hh
                    }));
                    result.Add("sumaHHPeriodo", sumaHHPeriodo);
                    result.Add("arrProyectos", arrProyectos);
                    vSesiones.sesionPeriodoInicial = periodoInicial;
                    vSesiones.sesionPeriodoFinal = periodoFinal;
                    vSesiones.sesionArrProyectos = arrProyectos;

                    vSesiones.sesionNominaSemanal = nominaSemanal;
                }
                else
                {
                    arrProyectos.Add(Proyectos);

                    var data = maquinaFactoryServices.getMaquinaServices().GetEconomicos(arrProyectos, periodoInicial, periodoFinal);
                    var sumaHHPeriodo = data.Select(x => x.hhPeriodo).Sum();
                    vSesiones.sesionNominaCCDetalles = data.Select(m => new tblM_CapNominaCC_Detalles
                    {
                        economico = m.noEconomico,
                        descripcion = m.descripcion,
                        cc = m.cc,
                        hh = m.hhPeriodo,
                        cargoP = m.porcentajeCargo,
                        cargoD = m.cargoMaquina,
                    }).ToList();

                    result.Add("data", data);
                    result.Add("sumaHHPeriodo", sumaHHPeriodo);
                    result.Add("arrProyectos", arrProyectos);
                    vSesiones.sesionPeriodoInicial = periodoInicial;
                    vSesiones.sesionPeriodoFinal = periodoFinal;
                    vSesiones.sesionArrProyectos = arrProyectos;
                    vSesiones.sesionNominaSemanal = nominaSemanal;
                }
                result.Add("isPeriodoActivo", isPeriodoActivo);
                result.Add(SUCCESS, true);
            //}
            //catch (Exception e)
            //{

            //    result.Add("isPeriodoActivo", false);
            //    result.Add(MESSAGE, e.Message);
            //    result.Add(SUCCESS, false);
            //    vSesiones.sesionArrProyectos = new List<string>();
            //    vSesiones.sesionPeriodoInicial = string.Empty;
            //    vSesiones.sesionPeriodoFinal = string.Empty;
            //    vSesiones.sesionNominaSemanal = string.Empty;
            //    vSesiones.sesionNominaCCDetalles = new List<tblM_CapNominaCC_Detalles>();
            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetGuardados(string fechaCaptura, string proyecto, int estatus)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<NominaCCDTO> list = new List<NominaCCDTO>();

                list = maquinaFactoryServices.getMaquinaServices().GetNominaCCGuardados(fechaCaptura, proyecto, estatus).ToList();


                result.Add("data", list);



                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getNominaCCYDet(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = maquinaFactoryServices.getMaquinaServices().getNominaCC(id);
                var lstCC = maquinaFactoryServices.getMaquinaServices().getNominaCCLstProyectos(id).Where(x => x.areaCuenta == obj.ac);
                var lstNoInv = Enum.GetValues(typeof(CCNoInventarioHH)).Cast<CCNoInventarioHH>().ToList().Select(x => x.GetDescription()).ToList();
                vSesiones.sesionPeriodoInicial = obj.periodoInicial.ToShortDateString();
                vSesiones.sesionPeriodoFinal = obj.periodoFinal.ToShortDateString();
                vSesiones.sesionNominaSemanal = obj.nominaSemanal.ToString();
                vSesiones.sesionArrProyectos = new List<string>();
                result.Add("nomina", obj);
                result.Add("lstCC", maquinaFactoryServices.getMaquinaServices().getNominaCCProyectos(id));
                result.Add("lstDet", maquinaFactoryServices.getMaquinaServices().getNominaCCDet(id));
                result.Add("lblEstatus", lstCC.Any(c => lstNoInv.Any(i => i.Equals(c.areaCuenta))) ? "Virtualizado" : "Verificado");
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarCargoNominaCC()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var downloadPDF = (List<byte[]>)Session["downloadPDF"];
                maquinaFactoryServices.getMaquinaServices().GuardarCargoNominaCC(downloadPDF);


                Session["downloadPDF"] = null;
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ActualizarCargoNominaCC(tblM_CapNominaCC nomina, List<tblM_CapNominaCC_Detalles> lstDet)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var downloadPDF = (List<byte[]>)Session["downloadPDF"];
                maquinaFactoryServices.getMaquinaServices().ActualizarCargoNominaCC(downloadPDF, nomina, lstDet);
                Session["downloadPDF"] = null;
                result.Add("id", nomina.id);
                result.Add("isVerificado", nomina.isVerificado);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public FileResult GetArchivo(string archivo)
        {
            var nombre = archivo + ".pdf";
            var Ruta = archivofs.getArchivo().getUrlDelServidor(7) + nombre;

            return File(Ruta, "application/pdf", nombre);
        }

        public ActionResult GetCCNomina(List<string> arrProyectos)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = maquinaFactoryServices.getMaquinaServices().GetCCNomina(arrProyectos);

                result.Add("data", data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboSemanas()
        {
            var result = new Dictionary<string, object>();
            try
            {
                DateTime fecha = new DateTime(DateTime.Now.Year, 01, 01);


                List<FechasDTO> ListaFechas = new List<FechasDTO>();

                ListaFechas = GetFechas(fecha);


                result.Add(ITEMS, ListaFechas);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public static List<FechasDTO> GetFechas(DateTime fecha)
        {
            List<FechasDTO> ListaFechas = new List<FechasDTO>();
            DateTime FechaInicio = new DateTime();
            DateTime FechaFin = new DateTime();

            for (int i = 1; i <= 52; i++)
            {
                if (i == 1)
                {
                    var diaSemana = (int)fecha.DayOfWeek;
                    FechaInicio = fecha.AddDays(-(int)fecha.DayOfWeek - 4);
                    int diasViernes = ((int)DayOfWeek.Tuesday - (int)fecha.DayOfWeek + 7) % 7;
                    FechaFin = fecha.AddDays(diasViernes);

                    if (FechaInicio >= DateTime.Now.Date && DateTime.Now <= FechaFin)
                    {
                        ListaFechas.Add(new FechasDTO
                        {
                            Value = i,
                            Text = FechaInicio.ToShortDateString() + " - " + FechaFin.ToShortDateString()
                        });
                    }
                }
                else
                {
                    var TempFecha = FechaFin.AddDays(1);

                    FechaInicio = TempFecha;
                    FechaFin = TempFecha.AddDays(6);

                    var fechaActual = DateTime.Now.Date.AddDays(-14).Date;
                    if (FechaInicio.Date >= fechaActual)
                    {
                        ListaFechas.Add(new FechasDTO
                        {
                            Value = i,
                            Text = FechaInicio.ToShortDateString() + " - " + FechaFin.ToShortDateString()
                        });
                    }
                }
            }
            return ListaFechas;
        }

        public ActionResult LlenarComboAC()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var listaCentroCostosActuales = centroCostosFactoryServices.getCentroCostosService().getListaCC();
                result.Add(ITEMS, listaCentroCostosActuales.Select(y => new
                {
                    Value = y.Value,
                    Text = y.Text,
                    Prefijo = y.Prefijo

                }).OrderBy(x => x.Value));

            }
            catch (Exception)
            {
                result.Add(ITEMS, null);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboMes()
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<FechasDTO> listaMeses = new List<FechasDTO>();

                string[] meses = DateTimeFormatInfo.CurrentInfo.MonthNames;
                int numeroMes = 1;
                foreach (var mes in meses)
                {
                    if (numeroMes != 13)
                    {
                        listaMeses.Add(new FechasDTO()
                        {
                            Text = mes.First().ToString().ToUpper() + mes.Substring(1),
                            Value = numeroMes++
                        });
                    }
                }

                result.Add(ITEMS, listaMeses);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboAño()
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<FechasDTO> listaAños = new List<FechasDTO>();

                var currentYear = DateTime.Now.Year;
                listaAños.Add(new FechasDTO()
                {
                    Text = currentYear.ToString(),
                    Value = currentYear++
                });

                var pastYear = DateTime.Now.AddYears(-1).Year;
                listaAños.Add(new FechasDTO()
                {
                    Text = pastYear.ToString(),
                    Value = pastYear++
                });

                result.Add(ITEMS, listaAños);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerNominaMensualCC(string[] areaCuentaArray, int mes, int año, int estatus)
        {
            IMaquinaDAO maquinaService = maquinaFactoryServices.getMaquinaServices();
            Dictionary<string, object> result = maquinaService.ObtenerNominaMensualCC(areaCuentaArray, mes, año, estatus);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNominaMensualCC(List<NominaMensualCCDTO> nominasProyectos, int mes, int año)
        {
            IMaquinaDAO maquinaService = maquinaFactoryServices.getMaquinaServices();
            Dictionary<string, object> result = maquinaService.GuardarNominaMensualCC(nominasProyectos, mes, año);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        

    }
}