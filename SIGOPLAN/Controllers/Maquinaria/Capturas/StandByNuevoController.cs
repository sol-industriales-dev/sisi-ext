
using Core.DTO;
using Core.DTO.Maquinaria.Inventario;
using Core.DTO.Maquinaria.Reporte.ActivoFijo;
using Core.DTO.Maquinaria.StandBy;
using Core.Entity.Maquinaria.Inventario;
using Core.Enum.Maquinaria;
using Core.Enum.Maquinaria.StandBy;
using Data.Factory.Maquinaria;
using Data.Factory.Maquinaria.Catalogos;
using Data.Factory.Maquinaria.Reporte.ActivoFijo;
using Data.Factory.Principal.Usuarios;
using Infrastructure.Utils;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Maquinaria.Capturas
{
    public class StandByNuevoController : BaseController
    {
        StandByNuevoFactoryServices standByNuevoFactoryServices;
        CentroCostosFactoryServices centroCostosFactoryServices;
        UsuarioFactoryServices usuarioFactoryServices;
        ActivoFijoFactoryServices affs;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            affs = new ActivoFijoFactoryServices();
            standByNuevoFactoryServices = new StandByNuevoFactoryServices();
            centroCostosFactoryServices = new CentroCostosFactoryServices();
            usuarioFactoryServices = new UsuarioFactoryServices();
            base.OnActionExecuting(filterContext);
        }
        // GET: OT
        public ActionResult Captura()
        {
            return View();
        }
        public ActionResult Validacion()
        {
            return View();
        }
        public ActionResult Historico()
        {
            return View();
        }
        public ActionResult GuardarCaptura(List<tblM_STB_CapturaStandBy> lst)
        {
            var result = new Dictionary<string, object>();
            try
            {
                foreach (var x in lst)
                {
                    x.usuarioCapturaID = vSesiones.sesionUsuarioDTO.id;
                    x.fechaCaptura = DateTime.Now;
                    x.estatus = 1;
                }
                var dataRaw = standByNuevoFactoryServices.getStandbyFactoryServices().GuardarCaptura(lst);

                result.Add("data", dataRaw);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarValidacion(List<StandByNuevoDTO> lst)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var dataRaw = standByNuevoFactoryServices.getStandbyFactoryServices().GuardarValidacion(lst);

                result.Add("data", dataRaw);
                result.Add(SUCCESS, dataRaw);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarLibracion(List<StandByNuevoDTO> lst)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var dataRaw = standByNuevoFactoryServices.getStandbyFactoryServices().GuardarLibracion(lst);

                result.Add("data", dataRaw);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getListaDisponible(string cc)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var dataRaw = standByNuevoFactoryServices.getStandbyFactoryServices().getListaDisponible(cc);
                var data = dataRaw.Select(x => new { 
                    noEconomicoID = x.id,
                    modelo = x.grupoMaquinaria.descripcion + " " + x.modeloEquipo.descripcion,
                    Economico = x.noEconomico,
                    cc = x.centro_costos,
                    ccNombre = x.centro_costos + " - "+ centroCostosFactoryServices.getCentroCostosService().getNombreCcFromSIGOPLAN(x.centro_costos)
                }).ToList();
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
        public ActionResult getListaByEstatus(int estatus, string noAC, string noEconomico)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var dataRaw = standByNuevoFactoryServices.getStandbyFactoryServices().getListaByEstatus(estatus,noAC,noEconomico).ToList();
                var data = new List<ListaStandByAutorizarDTO>();
                foreach (var item in dataRaw)
                {
                    //var infoDep = affs.getActivoFijoServices().DepreciacionNumEconomico(item.Economico, DateTime.Now);
                    //var detDep = new DepreciacionMaquinaConOverhaulDTO();
                    //if (infoDep.Success)
                    //{
                    //    detDep = infoDep.Value as DepreciacionMaquinaConOverhaulDTO;
                    //}

                    var d = new ListaStandByAutorizarDTO();
                    d.id = item.id;
                    d.Economico = item.Economico;
                    d.noEconomicoID = item.noEconomicoID;
                    d.modelo = item.noEconomico.grupoMaquinaria.descripcion + " " + item.noEconomico.modeloEquipo.descripcion;
                    d.estatus = EnumExtensions.GetDescription((EstatusStandByEnum)item.estatus);
                    d.usuarioCapturaID = item.usuarioCapturaID;
                    d.usuarioCapturaNombre = (item.usuarioCaptura==null?"":usuarioFactoryServices.getUsuarioService().getNombreUsuario(item.usuarioCapturaID));
                    d.fechaCaptura = item.fechaCaptura.ToShortDateString();
                    d.usuarioAutorizaID = item.usuarioAutorizaID==null?0:item.usuarioAutorizaID;
                    d.usuarioAutorizaNombre = (item.usuarioAutoriza == null ? "---" : usuarioFactoryServices.getUsuarioService().getNombreUsuario((int)item.usuarioAutorizaID));
                    d.fechaAutoriza = item.fechaAutoriza!=null?((DateTime)item.fechaAutoriza).ToShortDateString():"---";
                    d.usuarioLiberaID = item.usuarioLiberaID==null?0:item.usuarioLiberaID;
                    d.usuarioLiberaNombre = (item.usuarioLibera == null ? "---" : usuarioFactoryServices.getUsuarioService().getNombreUsuario((int)item.usuarioLiberaID));
                    d.fechaLibera = item.fechaLibera != null ? ((DateTime)item.fechaLibera).ToShortDateString() : "---";
                    d.ccActual = (item.ccActual + " - " + centroCostosFactoryServices.getCentroCostosService().getNombreCcFromSIGOPLAN(item.ccActual)).Trim();
                    d.comentarioJustificacion = item.comentarioJustificacion ?? "";
                    d.comentarioValidacion = item.comentarioValidacion ?? "";
                    d.comentarioLiberacion = item.comentarioLiberacion ?? "";
                    d.evidenciaJustificacion = item.evidenciaJustificacion ?? "";
                    d.moiEquipo = item.moiEquipo;
                    d.valorEnLibroEquipo = item.valorEnLibroEquipo;
                    d.depreciacionMensualEquipo = item.depreciacionMensualEquipo;
                    d.valorEnLibroOverhaul = item.valorEnLibroOverhaul;
                    d.depreciacionMensualOverhaul = item.depreciacionMensualOverhaul;
                    d.esVoBo = item.esVoBo;
                    //d.moiEquipo = detDep.MoiEquipo;
                    //d.moiOverhaul = detDep.MoiOverhaul;
                    //d.dep = detDep.DepreciacionEquipo + detDep.DepreciacionOverhaul;
                    //d.valorEnLibro = (d.moiEquipo + d.moiOverhaul) - (detDep.DepreciacionEquipo + detDep.DepreciacionOverhaul);
                    data.Add(d);
                }
                //var data = dataRaw.Select(x => new
                //{
                //    id = x.id,
                //    Economico = x.Economico,
                //    noEconomicoID = x.noEconomicoID,
                //    modelo = x.noEconomico.grupoMaquinaria.descripcion + " " + x.noEconomico.modeloEquipo.descripcion,
                //    estatus = EnumExtensions.GetDescription((EstatusStandByEnum)x.estatus),
                //    usuarioCapturaID = x.usuarioCapturaID,
                //    usuarioCapturaNombre = (x.usuarioCaptura==null?"":usuarioFactoryServices.getUsuarioService().getNombreUsuario(x.usuarioCapturaID)),
                //    fechaCaptura = x.fechaCaptura.ToShortDateString(),
                //    usuarioAutorizaID = x.usuarioAutorizaID==null?0:x.usuarioAutorizaID,
                //    usuarioAutorizaNombre = (x.usuarioAutoriza == null ? "---" : usuarioFactoryServices.getUsuarioService().getNombreUsuario((int)x.usuarioAutorizaID)),
                //    fechaAutoriza = x.fechaAutoriza!=null?((DateTime)x.fechaAutoriza).ToShortDateString():"---",
                //    usuarioLiberaID = x.usuarioLiberaID==null?0:x.usuarioLiberaID,
                //    usuarioLiberaNombre = (x.usuarioLibera == null ? "---" : usuarioFactoryServices.getUsuarioService().getNombreUsuario((int)x.usuarioLiberaID)),
                //    fechaLibera = x.fechaLibera != null ? ((DateTime)x.fechaLibera).ToShortDateString() : "---",
                //    ccActual = x.ccActual + " - " + centroCostosFactoryServices.getCentroCostosService().getNombreCcFromSIGOPLAN(x.ccActual),
                //    comentarioJustificacion = x.comentarioJustificacion ?? "",
                //    comentarioValidacion = x.comentarioValidacion ?? "",
                //    comentarioLiberacion = x.comentarioLiberacion ?? "",
                //    evidenciaJustificacion = x.evidenciaJustificacion ?? ""
                //}).ToList();
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

        public ActionResult EnviarExcelCorteStandBy(int inventario)
        {
            var r = new Respuesta();

            var fechaInicio = new DateTime();
            var fechaFin = new DateTime();

            DatetimeUtils.PeriodoSemanaCorte_Martes(ref fechaInicio, ref fechaFin);

            try
            {
                var standBy = getListaConDep(0, null, null, fechaInicio, fechaFin, 2, true);
                var porUbicacion = getListaConDep(0, null, null, fechaInicio, fechaFin, 3, true);

                var datos = new List<StandByDepDTO>();

                datos.Add(new StandByDepDTO
                {
                    Tipo = 2,
                    Descripcion = "StandBy",
                    Datos = standBy
                });
                datos.Add(new StandByDepDTO
                {
                    Tipo = 3,
                    Descripcion = "Por ubicación",
                    Datos = porUbicacion
                });

                var excel = GenerarExcel2(datos);

                var maquinaFS = new MaquinaFactoryServices();
                var correos = maquinaFS.getMaquinaServices().getListaCorreosInventario(3);
#if DEBUG
                correos = new List<string> { "martin.zayas@construplan.com.mx" };
#endif
                //var correos = new List<string>();
                //correos.Add("angel.devora@construplan.com.mx");

                var cuerpo = "<h3>Buenas tardes.</h3>" +
                             "<p>Se envía inventario de StandBy de maquinaria y equipo del día " + fechaInicio.Day + " de " + fechaInicio.ToString("MMMM") + " de " + fechaInicio.Year +
                             " al " + fechaFin.Day + " de " + fechaFin.ToString("MMMM") + " de " + fechaFin.Year + "</p>" +
                             "<p>Saludos</p>";
                //List<string> co = new List<string>();
                //co.Add("adan.gonzalez@construplan.com.mx");
                bool envioCorrecto = false;
                
                var empresaActual = vSesiones.sesionEmpresaActual;

                switch(empresaActual)
                {
                    case 2:
                        envioCorrecto = GlobalUtils.sendEmailAdjuntoInMemory2(
                            "[SIGOPLAN ARRENDADORA] Reporte StandBy de maquinaria y equipo del día " + fechaInicio.Day + " de " + fechaInicio.ToString("MMMM") + " de " + fechaInicio.Year + " al " + fechaFin.Day + " de " + fechaFin.ToString("MMMM") + " de " + fechaFin.Year,
                            cuerpo, correos, excel);
                        break;
                    case 6:
                        envioCorrecto = GlobalUtils.sendEmailAdjuntoInMemory2(
                            "[SIGOPLAN PERÚ] Reporte StandBy de maquinaria y equipo del día " + fechaInicio.Day + " de " + fechaInicio.ToString("MMMM") + " de " + fechaInicio.Year + " al " + fechaFin.Day + " de " + fechaFin.ToString("MMMM") + " de " + fechaFin.Year,
                            cuerpo, correos, excel);
                        break;
                    default:
                        envioCorrecto = GlobalUtils.sendEmailAdjuntoInMemory2(
                            "Reporte StandBy de maquinaria y equipo del día " + fechaInicio.Day + " de " + fechaInicio.ToString("MMMM") + " de " + fechaInicio.Year + " al " + fechaFin.Day + " de " + fechaFin.ToString("MMMM") + " de " + fechaFin.Year,
                            cuerpo, correos, excel);
                        break;
                }
                

                if (envioCorrecto)
                {
                    r.Success = true;
                    r.Message = "Ok";
                }
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return Json(r);
        }

        private List<dynamic> getListaConDep(int estatus, string noAC, string noEconomico, DateTime fechaInicio, DateTime fechaFinal, int tipo, bool corteSemanal)
        {
            fechaFinal = fechaFinal.AddHours(23).AddMinutes(59).AddSeconds(59);

            var data = new List<dynamic>();

            if (tipo == 2)
            {
                var dataRaw = standByNuevoFactoryServices.getStandbyFactoryServices().getDepreciacionPorStandBy(noAC, noEconomico, fechaInicio, fechaFinal, corteSemanal);
                
                var economicos = dataRaw.Select(m => new StandByEcosDTO
                {
                    NumEconomico = m.equipo.noEconomico,
                    Fecha = m.fechaDepreciacion
                }).ToList();

                if (dataRaw.Count > 0)
                {
                    var dep = affs.getActivoFijoServices().DepreciacionNumEconomicoDTO(economicos, fechaInicio, fechaFinal);

                    foreach (var x in dataRaw)
                    {
                        var depEquipo = dep.FirstOrDefault(f => f.NoEconomico == x.equipo.noEconomico && f.FechaFin == x.fechaDepreciacion);

                        var o = new
                        {
                            id = x.equipo.id,
                            Economico = x.equipo.noEconomico,
                            noEconomicoID = x.equipo.id,
                            modelo = x.equipo.grupoMaquinaria.descripcion + " " + x.equipo.modeloEquipo.descripcion,
                            ccActual = x.standBy.ccActual + " - " + centroCostosFactoryServices.getCentroCostosService().getNombreCcFromSIGOPLAN(x.standBy.ccActual),
                            fechaDepreciacion = x.fechaDepreciacion.ToShortDateString(),
                            DepreciacionEquipo = depEquipo != null ? depEquipo.DepreciacionEquipo : 0,
                            DepreciacionEquipoSemanal = depEquipo != null ? depEquipo.DepreciacionEquipoSemanal : 0,
                            DepreciacionEquipoPeriodo = depEquipo != null ? depEquipo.DepreciacionEquipoPeriodo : 0,
                            DepreciacionOverhaul = depEquipo != null ? depEquipo.DepreciacionOverhaul : 0,
                            DepreciacionOverhaulSemanal = depEquipo != null ? depEquipo.DepreciacionOverhaulSemanal : 0,
                            DepreciacionOverhaulPeriodo = depEquipo != null ? depEquipo.DepreciacionOverhaulPeriodo : 0
                        };

                        data.Add(o);
                    }
                }
            }
            if (tipo == 3)
            {
                var dataRaw = standByNuevoFactoryServices.getStandbyFactoryServices().getDepreciacionPorNoasignado(noEconomico, fechaInicio, fechaFinal, corteSemanal);

                var economicos = dataRaw.Select(m => new StandByEcosDTO
                {
                    NumEconomico = m.equipo.noEconomico,
                    Fecha = m.fechaDepreciacion
                }).ToList();

                if (dataRaw.Count > 0)
                {
                    var dep = affs.getActivoFijoServices().DepreciacionNumEconomicoDTO(economicos, fechaInicio, fechaFinal);

                    foreach (var x in dataRaw)
                    {
                        var depEquipo = dep.FirstOrDefault(f => f.NoEconomico == x.equipo.noEconomico && f.FechaFin == x.fechaDepreciacion);

                        var o = new
                        {
                            id = x.equipo.id,
                            Economico = x.equipo.noEconomico,
                            noEconomicoID = x.equipo.id,
                            modelo = x.equipo.grupoMaquinaria.descripcion + " " + x.equipo.modeloEquipo.descripcion,
                            fechaDepreciacion = x.fechaDepreciacion.ToShortDateString(),
                            DepreciacionEquipo = depEquipo != null ? depEquipo.DepreciacionEquipo : 0,
                            DepreciacionEquipoSemanal = depEquipo != null ? depEquipo.DepreciacionEquipoSemanal : 0,
                            DepreciacionEquipoPeriodo = depEquipo != null ? depEquipo.DepreciacionEquipoPeriodo : 0,
                            DepreciacionOverhaul = depEquipo != null ? depEquipo.DepreciacionOverhaul : 0,
                            DepreciacionOverhaulSemanal = depEquipo != null ? depEquipo.DepreciacionOverhaulSemanal : 0,
                            DepreciacionOverhaulPeriodo = depEquipo != null ? depEquipo.DepreciacionOverhaulPeriodo : 0
                        };

                        data.Add(o);
                    }
                }
            }

            return data;
        }

        public ActionResult getListaByEstatusConDepreciacion(int estatus, string noAC, string noEconomico,DateTime fechaInicio,DateTime fechaFinal, int tipo)
        {
            fechaFinal = fechaFinal.AddHours(23).AddMinutes(59).AddSeconds(59);
            
            var result = new Dictionary<string, object>();
            var data = new List<dynamic>();
            try
            {
                if (tipo == 1)
                {
                    var dataRaw = standByNuevoFactoryServices.getStandbyFactoryServices().getListaByEstatusConDepreciacion(estatus, noAC, noEconomico, fechaInicio, fechaFinal, tipo);
                    foreach (var x in dataRaw)
                    {
                        //var dep = affs.getActivoFijoServices().DepreciacionNumEconomicoDTO(x.Economico, fechaFin);
                        var o = new {
                            id = x.id,
                            Economico = x.Economico,
                            noEconomicoID = x.noEconomicoID,
                            modelo = x.noEconomico.grupoMaquinaria.descripcion + " " + x.noEconomico.modeloEquipo.descripcion,
                            estatus = EnumExtensions.GetDescription((EstatusStandByEnum)x.estatus),
                            usuarioCapturaID = x.usuarioCapturaID,
                            usuarioCapturaNombre = (x.usuarioCaptura == null ? "" : usuarioFactoryServices.getUsuarioService().getNombreUsuario(x.usuarioCapturaID)),
                            fechaCaptura = x.fechaCaptura.ToShortDateString(),
                            usuarioAutorizaID = x.usuarioAutorizaID == null ? 0 : x.usuarioAutorizaID,
                            usuarioAutorizaNombre = (x.usuarioAutoriza == null ? "---" : usuarioFactoryServices.getUsuarioService().getNombreUsuario((int)x.usuarioAutorizaID)),
                            fechaAutoriza = x.fechaAutoriza != null ? ((DateTime)x.fechaAutoriza).ToShortDateString() : "---",
                            usuarioLiberaID = x.usuarioLiberaID == null ? 0 : x.usuarioLiberaID,
                            usuarioLiberaNombre = (x.usuarioLibera == null ? "---" : usuarioFactoryServices.getUsuarioService().getNombreUsuario((int)x.usuarioLiberaID)),
                            fechaLibera = x.fechaLibera != null ? ((DateTime)x.fechaLibera).ToShortDateString() : "---",
                            ccActual = x.ccActual + " - " + centroCostosFactoryServices.getCentroCostosService().getNombreCcFromSIGOPLAN(x.ccActual),
                            comentarioJustificacion = x.comentarioJustificacion ?? "",
                            comentarioValidacion = x.comentarioValidacion ?? "",
                            comentarioLiberacion = x.comentarioLiberacion ?? "",
                            evidenciaJustificacion = x.evidenciaJustificacion ?? ""
                        };
                        data.Add(o);
                    }

                    result.Add("data", data);
                    result.Add(SUCCESS, true);
                }
                else if(tipo == 2){
                    data = getListaConDep(estatus, noAC, noEconomico, fechaInicio, fechaFinal, tipo, false);
                    //var dataRaw = standByNuevoFactoryServices.getStandbyFactoryServices().getDepreciacionPorStandBy(noAC, noEconomico, fechaInicio, fechaFinal);
                    ////var economicos = dataRaw.Select(m => m.equipo.noEconomico).Distinct().ToList();
                    //var ecos = dataRaw.Select(m => new StandByEcosDTO
                    //{
                    //    NumEconomico = m.equipo.noEconomico,
                    //    Fecha = m.fechaDepreciacion
                    //}).ToList();

                    //if (dataRaw.Count > 0)
                    //{
                    //    var dep = affs.getActivoFijoServices().DepreciacionNumEconomicoDTO(ecos, fechaInicio, fechaFinal);
                    //    foreach (var x in dataRaw)
                    //    {
                    //        var depEquipo = dep.FirstOrDefault(f => f.NoEconomico == x.equipo.noEconomico && f.FechaFin == x.fechaDepreciacion);

                    //        var o = new
                    //        {
                    //            id = x.equipo.id,
                    //            Economico = x.equipo.noEconomico,
                    //            noEconomicoID = x.equipo.id,
                    //            modelo = x.equipo.grupoMaquinaria.descripcion + " " + x.equipo.modeloEquipo.descripcion,
                    //            ccActual = x.standBy.ccActual + " - " + centroCostosFactoryServices.getCentroCostosService().getNombreCcFromSIGOPLAN(x.standBy.ccActual),
                    //            fechaDepreciacion = x.fechaDepreciacion.ToShortDateString(),
                    //            DepreciacionEquipo = depEquipo != null ? depEquipo.DepreciacionEquipo : 0,
                    //            DepreciacionEquipoSemanal = depEquipo != null ? depEquipo.DepreciacionEquipoSemanal : 0,
                    //            DepreciacionEquipoPeriodo = depEquipo != null ? depEquipo.DepreciacionEquipoPeriodo : 0,
                    //            DepreciacionOverhaul = depEquipo != null ? depEquipo.DepreciacionOverhaul : 0,
                    //            DepreciacionOverhaulSemanal = depEquipo != null ? depEquipo.DepreciacionOverhaulSemanal : 0,
                    //            DepreciacionOverhaulPeriodo = depEquipo != null ? depEquipo.DepreciacionOverhaulPeriodo : 0
                    //        };
                    //        data.Add(o);
                    //    }
                    //}

                    GenerarExcel(data, TipoEnum.stanby);

                    result.Add("data", data);
                    result.Add(SUCCESS, true);
                }
                else
                {
                    data = getListaConDep(estatus, noAC, noEconomico, fechaInicio, fechaFinal, tipo, false);
                    //var dataRaw = standByNuevoFactoryServices.getStandbyFactoryServices().getDepreciacionPorNoasignado(noEconomico, fechaInicio, fechaFinal);

                    ////var economicos = dataRaw.Select(m => m.equipo.noEconomico).Distinct().ToList();
                    //var ecos = dataRaw.Select(m => new StandByEcosDTO
                    //{
                    //    NumEconomico = m.equipo.noEconomico,
                    //    Fecha = m.fechaDepreciacion
                    //}).ToList();

                    //if (dataRaw.Count > 0)
                    //{
                    //    var dep = affs.getActivoFijoServices().DepreciacionNumEconomicoDTO(ecos, fechaInicio, fechaFinal);

                    //    foreach (var x in dataRaw)
                    //    {
                    //        var depEquipo = dep.FirstOrDefault(f => f.NoEconomico == x.equipo.noEconomico && f.FechaFin == x.fechaDepreciacion);

                    //        var o = new
                    //        {
                    //            id = x.equipo.id,
                    //            Economico = x.equipo.noEconomico,
                    //            noEconomicoID = x.equipo.id,
                    //            modelo = x.equipo.grupoMaquinaria.descripcion + " " + x.equipo.modeloEquipo.descripcion,
                    //            fechaDepreciacion = x.fechaDepreciacion.ToShortDateString(),
                    //            DepreciacionEquipo = depEquipo != null ? depEquipo.DepreciacionEquipo : 0,
                    //            DepreciacionEquipoSemanal = depEquipo != null ? depEquipo.DepreciacionEquipoSemanal : 0,
                    //            DepreciacionEquipoPeriodo = depEquipo != null ? depEquipo.DepreciacionEquipoPeriodo : 0,
                    //            DepreciacionOverhaul = depEquipo != null ? depEquipo.DepreciacionOverhaul : 0,
                    //            DepreciacionOverhaulSemanal = depEquipo != null ? depEquipo.DepreciacionOverhaulSemanal : 0,
                    //            DepreciacionOverhaulPeriodo = depEquipo != null ? depEquipo.DepreciacionOverhaulPeriodo : 0
                    //        };
                    //        data.Add(o);
                    //    }
                    //}

                    GenerarExcel(data, TipoEnum.ubicacion);

                    result.Add("data", data);
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

        public MemoryStream GetDepExcel()
        {
            if (Session["excelDepStanBy"] != null)
            {
                var stream = Session["excelDepStanBy"] as MemoryStream;

                Response.Clear();
                Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("Content-Disposition", "attachement; filename=Reporte.xlsx");
                Response.BinaryWrite(stream.ToArray());
                Response.End();
            }
            
            return null;
        }

        private List<byte[]> GenerarExcel2(List<StandByDepDTO> datos)
        {
            using (ExcelPackage excel = new ExcelPackage())
            {
                var header = new List<string>();
                var headerGeneralesInfo = new List<string>{
                    "Número", "Número económico", "Modelo"
                };
                var headerGeneralesDep = new List<string>
                {
                    "Fecha de depreciación", "Depreciación del equipo",
                    "Depreciación del equipo semanal", "Depreciación del equipo del periodo", "Depreciación de overhaul",
                    "Depreciación de overhaul semanal", "Depreciación de overhaul del periodo"
                };

                foreach (var hoja in datos)
                {
                    var excelDetalles = excel.Workbook.Worksheets.Add(hoja.Descripcion);

                    header = new List<string>();

                    header.AddRange(headerGeneralesInfo);
                    if (hoja.Tipo == (int)TipoEnum.stanby)
                    {
                        header.Add("CC Actual");
                    }
                    header.AddRange(headerGeneralesDep);

                    for (int i = 1; i <= header.Count; i++)
                    {
                        excelDetalles.Cells[1, i].Value = header[i - 1];
                    }

                    var cellData = new List<object[]>();
                    int contador = 1;
                    foreach (var item in hoja.Datos)
                    {
                        if (hoja.Tipo == (int)TipoEnum.stanby)
                        {
                            cellData.Add(new object[] {
                                contador,
                                item.Economico,
                                item.modelo,
                                item.ccActual,
                                item.fechaDepreciacion,
                                item.DepreciacionEquipo,
                                item.DepreciacionEquipoSemanal,
                                item.DepreciacionEquipoPeriodo,
                                item.DepreciacionOverhaul,
                                item.DepreciacionOverhaulSemanal,
                                item.DepreciacionOverhaulPeriodo
                            });
                        }
                        if (hoja.Tipo == (int)TipoEnum.ubicacion)
                        {
                            cellData.Add(new object[] {
                                contador,
                                item.Economico,
                                item.modelo,
                                item.fechaDepreciacion,
                                item.DepreciacionEquipo,
                                item.DepreciacionEquipoSemanal,
                                item.DepreciacionEquipoPeriodo,
                                item.DepreciacionOverhaul,
                                item.DepreciacionOverhaulSemanal,
                                item.DepreciacionOverhaulPeriodo
                            });
                        }
                        
                        contador++;
                    }

                    excelDetalles.Cells[2, 1].LoadFromArrays(cellData);

                    ExcelRange range = excelDetalles.Cells[1, 1, excelDetalles.Dimension.End.Row, excelDetalles.Dimension.End.Column];
                    ExcelTable table = excelDetalles.Tables.Add(range, "Tabla_" + hoja.Descripcion.Replace(" ", ""));

                    excelDetalles.Cells[1, hoja.Tipo == (int)TipoEnum.stanby ? 5 : 4, excelDetalles.Dimension.End.Row, excelDetalles.Dimension.End.Column].Style.Numberformat.Format = "$#,##0.00";
                    table.TableStyle = TableStyles.Medium17;

                    excelDetalles.Cells[excelDetalles.Dimension.Address].AutoFitColumns();
                }

                var lista = new List<byte[]>();
                //var bytes = new MemoryStream();
                using (var stream = new MemoryStream())
                {
                    excel.SaveAs(stream);
                    //bytes = stream;
                    lista.Add(stream.ToArray());
                }

                //return bytes;
                return lista;
            }
        }

        private void GenerarExcel(List<dynamic> data, TipoEnum tipo)
        {
            using(ExcelPackage excel = new ExcelPackage()){
                var excelDetalles = excel.Workbook.Worksheets.Add("Standby histórico");

                var header = new List<string>();
                var headerGeneralesInfo = new List<string>{
                    "Número", "Número económico", "Modelo"
                };
                var headerGeneralesDep = new List<string>
                {
                    "Fecha de depreciación", "Depreciación del equipo",
                    "Depreciación del equipo semanal", "Depreciación del equipo del periodo", "Depreciación de overhaul",
                    "Depreciación de overhaul semanal", "Depreciación de overhaul del periodo"
                };

                if (tipo == TipoEnum.stanby)
                {
                    header.AddRange(headerGeneralesInfo);
                    header.AddRange(new List<string>
                    {
                        "CC Actual"
                    });
                    header.AddRange(headerGeneralesDep);
                }
                else
                {
                    header.AddRange(headerGeneralesInfo);
                    header.AddRange(headerGeneralesDep);
                }
                for (int i = 1; i <= header.Count; i++)
                {
                    excelDetalles.Cells[1, i].Value = header[i - 1];
                }

                for (int i = 2; i < data.Count; i++)
                {
                    excelDetalles.Cells[i, 1].Value = i - 1;
                }

                var cellData = new List<object[]>();
                int contador = 1;
                foreach (dynamic item in data)
                {
                    if (tipo == TipoEnum.stanby)
                    {
                        cellData.Add(new object[] {
                            contador,
                            item.Economico,
                            item.modelo,
                            item.ccActual,
                            item.fechaDepreciacion,
                            item.DepreciacionEquipo,
                            item.DepreciacionEquipoSemanal,
                            item.DepreciacionEquipoPeriodo,
                            item.DepreciacionOverhaul,
                            item.DepreciacionOverhaulSemanal,
                            item.DepreciacionOverhaulPeriodo
                        });
                    }
                    if (tipo == TipoEnum.ubicacion)
                    {
                        cellData.Add(new object[] {
                        contador,
                        item.Economico,
                        item.modelo,
                        item.fechaDepreciacion,
                        item.DepreciacionEquipo,
                        item.DepreciacionEquipoSemanal,
                        item.DepreciacionEquipoPeriodo,
                        item.DepreciacionOverhaul,
                        item.DepreciacionOverhaulSemanal,
                        item.DepreciacionOverhaulPeriodo
                    });
                    }

                    contador++;
                }

                excelDetalles.Cells[2, 1].LoadFromArrays(cellData);

                ExcelRange range = excelDetalles.Cells[1, 1, excelDetalles.Dimension.End.Row, excelDetalles.Dimension.End.Column];
                
                ExcelTable tab = excelDetalles.Tables.Add(range, "Tabla");

                excelDetalles.Cells[1, tipo == TipoEnum.stanby ? 5 : 4, excelDetalles.Dimension.End.Row, excelDetalles.Dimension.End.Column].Style.Numberformat.Format = "$#,##0.00";

                tab.TableStyle = TableStyles.Medium17;

                excelDetalles.Cells[excelDetalles.Dimension.Address].AutoFitColumns();

                var bytes = new MemoryStream();
                using (var stream = new MemoryStream()){
                    excel.SaveAs(stream);
                    bytes = stream;
                }

                Session["excelDepStanBy"] = bytes;
            }
        }

        public ActionResult GetUsuarioTipoAutorizacion()
        {
            return Json(standByNuevoFactoryServices.getStandbyFactoryServices().GetUsuarioTipoAutorizacion(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarVoBo(List<StandByNuevoDTO> lstStandByDTO)
        {
            return Json(standByNuevoFactoryServices.getStandbyFactoryServices().GuardarVoBo(lstStandByDTO), JsonRequestBehavior.AllowGet);
        }
    }
}