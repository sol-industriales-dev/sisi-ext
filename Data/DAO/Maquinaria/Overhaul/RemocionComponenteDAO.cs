using Core.DAO.Maquinaria.Overhaul;
using Core.Entity.Maquinaria.Overhaul;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Catalogo;
using Core.DTO.Maquinaria.Overhaul;
using Core.DTO.Principal.Generales;
using Newtonsoft.Json;
using Core.Entity.RecursosHumanos.Catalogo;
using Data.EntityFramework.Context;
using Core.Enum;
using Core.Enum.Maquinaria;
using Infrastructure.Utils;
using Newtonsoft.Json.Linq;
using Core.DTO.Maquinaria.Catalogos;
using Core.DTO.Utils.Data;
using System.Data.Odbc;
using Core.Enum.Multiempresa;
using Core.DTO;
using Core.Enum.Principal;

namespace Data.DAO.Maquinaria.Overhaul
{
    public class RemocionComponenteDAO : GenericDAO<tblM_ReporteRemocionComponente>, IRemocionComponenteDAO
    {
        public void Guardar(tblM_ReporteRemocionComponente obj, int tipo = 0)
        {
            if (obj.fechaRemocion != null && tipo == 0)
            {
                //obj.horasComponente = CalcularHrsCompRemovido(obj.componenteRemovidoID, obj.fechaRemocion);
                obj.horasComponente = GetHrsCicloActualComponente(obj.componenteRemovidoID, obj.fechaRemocion, Int32.MaxValue, false);
                obj.horasMaquina = CalcularHrsMaquina(obj.maquinaID, obj.fechaRemocion);
            }

            if (obj.id == 0)
                SaveEntity(obj, (int)BitacoraEnum.REPORTEREMOCIONCOMPONENTE);
            else
            {
                var reporte = _context.tblM_ReporteRemocionComponente.FirstOrDefault(x => x.id == obj.id);
                if (reporte != null)
                {
                    obj.fechaVoBo = reporte.fechaVoBo;
                    obj.fechaEnvio = reporte.fechaEnvio;
                    obj.fechaAutorizacion = reporte.fechaAutorizacion;
                    obj.realiza = reporte.realiza;
                }
                Update(obj, obj.id, (int)BitacoraEnum.REPORTEREMOCIONCOMPONENTE);
            }
        }

        private decimal CalcularHrsCompRemovido(int idComponente, DateTime fecha)
        {
            decimal hrsCiclo = 0;
            try
            {
                bool bandera = false;
                var trackUltimoReciclado = _context.tblM_trackComponentes.OrderByDescending(x => x.fecha).ThenByDescending(x => x.id).FirstOrDefault(x => x.componenteID == idComponente && x.reciclado == true && x.fecha <= fecha);
                if (trackUltimoReciclado == null)
                {
                    bandera = true;
                    trackUltimoReciclado = _context.tblM_trackComponentes.OrderBy(x => x.fecha).ThenBy(x => x.id).FirstOrDefault(x => x.componenteID == idComponente);
                    hrsCiclo += trackUltimoReciclado.horasCiclo;
                }
                var trackFechaUltimo = trackUltimoReciclado.fecha;
                var trackIdUltimo = trackUltimoReciclado.id;
                var trackComponente = _context.tblM_trackComponentes.Where(x => x.componenteID == idComponente && x.fecha >= trackFechaUltimo && x.fecha <= fecha && (bandera ? x.id >= trackIdUltimo : x.id > trackIdUltimo)).OrderBy(x => x.fecha).ThenBy(x => x.id).ToList();

                DateTime fechaActual = new DateTime();
                DateTime fechaSiguiente = new DateTime();
                string Economico = "";
                int locacion = 0;
                if (trackComponente.Count() > 0)
                {
                    for (int i = 0; i < trackComponente.Count(); i++)
                    {
                        fechaActual = trackComponente[i].fecha ?? default(DateTime);
                        if ((i + 1) < trackComponente.Count()) { fechaSiguiente = trackComponente[i + 1].fecha ?? default(DateTime); }
                        else { fechaSiguiente = fecha; }
                        if (trackComponente[i].tipoLocacion == false)
                        {
                            locacion = trackComponente[i].locacionID ?? default(int);
                            Economico = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == locacion).noEconomico;
                            var HorometroFinal = _context.tblM_CapHorometro.OrderBy(x => x.HorometroAcumulado).FirstOrDefault(x => x.Economico == Economico && x.Fecha >= fechaSiguiente);
                            if (HorometroFinal == null)
                            {
                                HorometroFinal = _context.tblM_CapHorometro.OrderByDescending(x => x.Fecha).FirstOrDefault(x => x.Economico == Economico);
                            }

                            var HorometroInicial = _context.tblM_CapHorometro.OrderByDescending(x => x.Fecha).FirstOrDefault(x => x.Economico == Economico && x.Fecha <= fechaActual);
                            if (HorometroInicial != null)
                            {
                                hrsCiclo += (HorometroFinal.HorometroAcumulado - HorometroInicial.HorometroAcumulado);
                            }
                            else
                            {
                                hrsCiclo += HorometroFinal.HorometroAcumulado;
                            }
                        }
                    }
                }
                else
                {
                    locacion = trackUltimoReciclado.locacionID ?? default(int);
                    Economico = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == locacion).noEconomico;
                    var HorometroInicial = _context.tblM_CapHorometro.OrderBy(x => x.Fecha).FirstOrDefault(x => x.Economico == Economico && x.Fecha >= trackFechaUltimo);
                    var HorometroFinal = _context.tblM_CapHorometro.OrderByDescending(x => x.Fecha).ThenByDescending(x => x.turno).FirstOrDefault(x => x.Economico == Economico && x.Fecha <= fecha);
                    hrsCiclo += (HorometroFinal.HorometroAcumulado - HorometroInicial.HorometroAcumulado);
                }
            }
            catch (Exception e)
            {
                hrsCiclo = 0;
            }
            return hrsCiclo;
        }

        //public decimal CalcularHrsAcumuladas(int idComponente, DateTime fecha)
        //{
        //    decimal hrsCiclo = 0;
        //    decimal hrsIniciales = 0;
        //    try
        //    {
        //        bool bandera = false;
        //        var trackUltimoReciclado = _context.tblM_trackComponentes.OrderByDescending(x => x.fecha).ThenByDescending(x => x.id).FirstOrDefault(x => x.componenteID == idComponente && x.reciclado == true && x.fecha <= fecha);
        //        if (trackUltimoReciclado == null)
        //        {
        //            bandera = true;
        //            trackUltimoReciclado = _context.tblM_trackComponentes.OrderBy(x => x.fecha).ThenBy(x => x.id).FirstOrDefault(x => x.componenteID == idComponente);
        //            hrsCiclo += trackUltimoReciclado.horasCiclo;
        //        }
        //        var trackFechaUltimo = trackUltimoReciclado.fecha;
        //        var trackIdUltimo = trackUltimoReciclado.id;
        //        hrsIniciales = trackUltimoReciclado.horasAcumuladas;
        //        var trackComponente = _context.tblM_trackComponentes.Where(x => x.componenteID == idComponente && x.fecha >= trackFechaUltimo && x.fecha <= fecha && (bandera ? x.id >= trackIdUltimo : x.id > trackIdUltimo)).OrderBy(x => x.fecha).ThenBy(x => x.id).ToList();

        //        DateTime fechaActual = new DateTime();
        //        DateTime fechaSiguiente = new DateTime();
        //        string Economico = "";
        //        int locacion = 0;
        //        if (trackComponente.Count() > 0)
        //        {
        //            for (int i = 0; i < trackComponente.Count(); i++)
        //            {
        //                fechaActual = trackComponente[i].fecha ?? default(DateTime);
        //                if ((i + 1) < trackComponente.Count()) { fechaSiguiente = trackComponente[i + 1].fecha ?? default(DateTime); }
        //                else { fechaSiguiente = fecha; }
        //                if (trackComponente[i].tipoLocacion == false)
        //                {
        //                    locacion = trackComponente[i].locacionID ?? default(int);
        //                    Economico = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == locacion).noEconomico;
        //                    var HorometroFinal = _context.tblM_CapHorometro.OrderBy(x => x.HorometroAcumulado).FirstOrDefault(x => x.Economico == Economico && x.Fecha >= fechaSiguiente);
        //                    if (HorometroFinal == null)
        //                    {
        //                        HorometroFinal = _context.tblM_CapHorometro.OrderByDescending(x => x.Fecha).FirstOrDefault(x => x.Economico == Economico);
        //                    }

        //                    var HorometroInicial = _context.tblM_CapHorometro.OrderBy(x => x.Fecha).FirstOrDefault(x => x.Economico == Economico && x.Fecha >= fechaActual);
        //                    if (HorometroInicial != null)
        //                    {
        //                        hrsCiclo += (HorometroFinal.HorometroAcumulado - HorometroInicial.HorometroAcumulado);
        //                    }
        //                    else
        //                    {
        //                        hrsCiclo += HorometroFinal.HorometroAcumulado;
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            locacion = trackUltimoReciclado.locacionID ?? default(int);
        //            Economico = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == locacion).noEconomico;
        //            var HorometroInicial = _context.tblM_CapHorometro.OrderBy(x => x.Fecha).FirstOrDefault(x => x.Economico == Economico && x.Fecha >= trackFechaUltimo);
        //            var HorometroFinal = _context.tblM_CapHorometro.OrderByDescending(x => x.Fecha).ThenByDescending(x => x.turno).FirstOrDefault(x => x.Economico == Economico && x.Fecha <= fecha);
        //            hrsCiclo += (HorometroFinal.HorometroAcumulado - HorometroInicial.HorometroAcumulado);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        hrsCiclo = 0;
        //    }
        //    return hrsCiclo + hrsIniciales;
        //}

        public decimal CalcularHrsCicloComponente(int idComponente, DateTime fechaLimite)
        {
            decimal horometro = 0;
            try
            {
                var tracking = _context.tblM_trackComponentes.Where(x => x.fecha <= fechaLimite && x.componenteID == idComponente).OrderByDescending(x => x.fecha).ThenByDescending(x => x.id).FirstOrDefault();
                if (tracking == null) { tracking = _context.tblM_trackComponentes.Where(x => x.fecha >= fechaLimite && x.componenteID == idComponente).OrderBy(x => x.fecha).ThenByDescending(x => x.id).FirstOrDefault(); }
                if (tracking != null) { horometro = tracking.horasCiclo; }
                //var trackingTotal = _context.tblM_trackComponentes.Where(x => x.componenteID == idComponente).OrderByDescending(x => x.fecha).ThenByDescending(x => x.id).ToList();
                //var fechaSiguiente = fechaLimite;
                //var locacion = "";
                //var fechaActual = new DateTime();
                //for (int i = 0; i < trackingTotal.Count(); i++)
                //{
                //    if (trackingTotal[i].tipoLocacion == false)
                //    {
                //        locacion = trackingTotal[i].locacion;
                //        fechaActual = trackingTotal[i].fecha ?? default(DateTime);
                //        var horometroLocacion = _context.tblM_CapHorometro.Where(x => x.Economico == locacion && x.Fecha >= fechaActual && x.Fecha <= fechaSiguiente);
                //        horometro += horometroLocacion.Sum(x => x.HorasTrabajo);
                //    }
                //    fechaSiguiente = trackingTotal[i].fecha ?? default(DateTime);
                //}
            }
            catch (Exception e)
            {
                horometro = 0;
            }
            return horometro;
        }

        public List<horometrosComponentesDTO> GetHrsCicloActualComponentes(List<int> componentesIDs)
        {
            List<horometrosComponentesDTO> data = new List<horometrosComponentesDTO>();
            DateTime fechaMinima = DateTime.Now;
            DateTime fechaInicioHorometros = new DateTime(2011, 3, 28);
            bool banderaReciclado = false;
            bool banderaEntraCiclo = false;

            var componentesHorasInicio = _context.tblM_CatComponente.Where(x => componentesIDs.Contains(x.id)).Select(x => new { id = x.id, horasInicio = x.horasCicloInicio }).ToList();

            var tracking = _context.tblM_trackComponentes.Where(x => componentesIDs.Contains(x.componenteID)).OrderByDescending(x => x.fecha).ThenByDescending(x => x.id).Select(x => new TrackingHistorialDTO
            {
                componenteID = x.componenteID,
                fecha = x.fecha ?? default(DateTime),
                locacion = x.locacion,
                reciclado = x.reciclado,
                tipoLocacion = x.tipoLocacion ?? false,
                horasHistorial = x.horasCiclo
            }).ToList();

            var trackingGrouped = _context.tblM_trackComponentes.Where(x => componentesIDs.Contains(x.componenteID)).Select(x => new TrackingHistorialDTO
            {
                componenteID = x.componenteID,
                fecha = x.fecha ?? default(DateTime),
                locacion = x.locacion,
                reciclado = x.reciclado,
                tipoLocacion = x.tipoLocacion ?? false,
                id = x.id
            }).GroupBy(x => x.componenteID).ToList();

            List<string> maquinas = new List<string>();
            foreach (var item in trackingGrouped)
            {
                foreach (var item2 in item.OrderByDescending(x => x.fecha).ThenByDescending(x => x.id))
                {
                    if (item2.tipoLocacion == false)
                    {
                        if (item2.fecha < fechaMinima) fechaMinima = item2.fecha;
                        maquinas.Add(item2.locacion);
                    }
                    if (item2.reciclado) break;
                }
            }
            maquinas = maquinas.Distinct().ToList();

            var horometros = _context.tblM_CapHorometro.GroupJoin(maquinas, x => x.Economico, economico => economico, (x, economico) => new { x, economico })
                .Where(e => e.economico.Any() /*&& e.x.Fecha >= fechaMinima*/).OrderByDescending(e => e.x.Fecha).ThenByDescending(e => e.x.turno)
                .Select(e => new HorasTrackingDTO
                {
                    Fecha = e.x.Fecha,
                    Economico = e.x.Economico,
                    HorometroAcumulado = e.x.HorometroAcumulado,
                    HorasTrabajo = e.x.HorasTrabajo,
                    turno = e.x.turno
                }).GroupBy(x => x.Economico).ToList();

            data = componentesIDs.Select(x =>
            {                
                DateTime fechaFin = DateTime.Now;
                DateTime fechaInicio = DateTime.Now;
                var auxTracking = tracking.Where(y => y.componenteID == x).ToList();
                var auxMaquinasTrack = auxTracking.Select(y => y.locacion).ToList();
                decimal horometroActual = 0;
                if (auxTracking.Count() > 0)
                {
                    var lstHorometros = horometros.Where(y => auxMaquinasTrack.Contains(y.Key)).SelectMany(y => y).OrderByDescending(y => y.Fecha).ThenByDescending(y => y.turno).ToList();
                    var componente = _context.tblM_CatComponente.FirstOrDefault(y => y.id == x);
                    decimal horaAnterior = componente.horaCicloActual;
                    decimal horasInicio = 0;
                    decimal horasFinal = 0;
                    banderaReciclado = false;
                    banderaEntraCiclo = false;
                    foreach (var item in auxTracking)
                    {
                        fechaInicio = item.fecha;
                        if (item.tipoLocacion == false)
                        {
                            if (/*fechaInicio < fechaInicioHorometros ||*/ fechaFin < fechaInicioHorometros)
                            {
                                horometroActual += horaAnterior;
                                //if (fechaFin < fechaInicioHorometros) break;
                            }
                            else
                            {
                                var horometroInicial = lstHorometros.LastOrDefault(y => y.Economico == item.locacion && y.Fecha >= fechaInicio && y.Fecha <= fechaFin);
                                horasInicio = horometroInicial == null ? 0 : horometroInicial.HorometroAcumulado - horometroInicial.HorasTrabajo;
                                var horometroFinal = lstHorometros.FirstOrDefault(y => y.Economico == item.locacion && y.Fecha <= fechaFin && y.Fecha >= fechaInicio && y.HorometroAcumulado >= horasInicio);
                                horasFinal = horometroFinal == null ? 0 : horometroFinal.HorometroAcumulado;
                                decimal restaHorometros = horasFinal - horasInicio;
                                horometroActual += restaHorometros;
                                horasFinal = horasInicio;
                                banderaEntraCiclo = true;
                                if (fechaInicio < fechaInicioHorometros && horasInicio != horaAnterior) horometroActual += horasInicio;
                            }
                        }
                        if (banderaEntraCiclo) horaAnterior = item.horasHistorial;
                        fechaFin = fechaInicio;
                        if (item.reciclado)
                        {
                            banderaReciclado = true;
                            break;
                        }
                    }
                }
                if (!banderaReciclado)
                {
                    var auxComponente = componentesHorasInicio.FirstOrDefault(y => y.id == x);
                    if (auxComponente != null) horometroActual += auxComponente.horasInicio;
                }
                return new horometrosComponentesDTO
                {
                    componenteID = x,
                    horometroActual = horometroActual
                };
            }).ToList();
            return data;
        }

        public decimal GetHrsCicloActualComponente(int componenteID, DateTime fechaActual, int id = 0, bool esHistorial = false)
        {
            List<horometrosComponentesDTO> data = new List<horometrosComponentesDTO>();
            DateTime fechaMinima = DateTime.Now;
            DateTime fechaInicioHorometros = new DateTime(2011, 3, 28);
            bool banderaReciclado = false;
            bool trackActualReciclado = false;

            var componente = _context.tblM_CatComponente.FirstOrDefault(x => x.id == componenteID);

            var tracking = _context.tblM_trackComponentes.Where(x => x.componenteID == componenteID && (esHistorial ? x.fecha < fechaActual : x.fecha <= fechaActual)).OrderByDescending(x => x.fecha).ThenByDescending(x => x.id).Select(x => new TrackingHistorialDTO
            {
                componenteID = x.componenteID,
                fecha = x.fecha ?? default(DateTime),
                locacion = x.locacion,
                reciclado = x.reciclado,
                tipoLocacion = x.tipoLocacion ?? false,
                horasHistorial = x.horasCiclo
            }).ToList();

            var trackingSig = _context.tblM_trackComponentes.Where(x => x.componenteID == componenteID && x.fecha >= fechaActual).OrderBy(x => x.fecha).ThenBy(x => x.id).FirstOrDefault();

            decimal horometroActual = 0;
            var trackingSingle = _context.tblM_trackComponentes.Where(x => x.componenteID == componenteID && x.fecha == fechaActual && (id == 0 ? true : x.id < id));
            if (trackingSingle != null) trackActualReciclado = trackingSingle.Any(x => x.reciclado);

            if (tracking.Count() > 0 && !trackActualReciclado)
            {

                List<string> maquinas = new List<string>();
                foreach (var item2 in tracking.OrderByDescending(x => x.fecha).ThenByDescending(x => x.id))
                {
                    if (item2.tipoLocacion == false)
                    {
                        if (item2.fecha < fechaMinima) fechaMinima = item2.fecha;
                        maquinas.Add(item2.locacion);
                    }
                    if (item2.reciclado) break;
                }

                maquinas = maquinas.Distinct().ToList();

                var horometros = _context.tblM_CapHorometro.GroupJoin(maquinas, x => x.Economico, economico => economico, (x, economico) => new { x, economico })
                    .Where(e => e.economico.Any() && e.x.Fecha >= fechaMinima).OrderByDescending(e => e.x.Fecha).ThenByDescending(e => e.x.turno)
                    .Select(e => new HorasTrackingDTO
                    {
                        Fecha = e.x.Fecha,
                        Economico = e.x.Economico,
                        HorometroAcumulado = e.x.HorometroAcumulado,
                        HorasTrabajo = e.x.HorasTrabajo,
                        turno = e.x.turno
                    }).GroupBy(x => x.Economico).ToList();

                DateTime fechaFin = fechaActual;
                DateTime fechaInicio = fechaActual;
                var lstHorometros = horometros.Where(y => maquinas.Contains(y.Key)).SelectMany(y => y).OrderByDescending(y => y.Fecha).ThenByDescending(y => y.turno).ToList();
                decimal horasInicio = 0;
                decimal horasFinal = 0;
                decimal horaAcumuladaAnterior = trackingSig == null ? componente.horaCicloActual : trackingSig.horasCiclo;
                banderaReciclado = false;
                var banderaEntraCiclo = false;
                foreach (var item in tracking)
                {
                    fechaInicio = item.fecha;
                    if (item.tipoLocacion == false)
                    {
                        if (/*fechaInicio < fechaInicioHorometros ||*/ fechaFin < fechaInicioHorometros)
                        {
                            horometroActual += horaAcumuladaAnterior;
                            //if (fechaFin < fechaInicioHorometros) break;
                        }
                        else
                        {
                            var horometroInicial = lstHorometros.LastOrDefault(y => y.Economico == item.locacion && y.Fecha >= fechaInicio && y.Fecha <= fechaFin);
                            horasInicio = horometroInicial == null ? 0 : horometroInicial.HorometroAcumulado - horometroInicial.HorasTrabajo;
                            var horometroFinal = lstHorometros.FirstOrDefault(y => y.Economico == item.locacion && y.Fecha <= fechaFin && y.Fecha >= fechaInicio && y.HorometroAcumulado >= horasInicio);
                            horasFinal = horometroFinal == null ? 0 : horometroFinal.HorometroAcumulado;
                            decimal restaHorometros = horasFinal - horasInicio;
                            horometroActual += restaHorometros;
                            horasFinal = horasInicio;
                            banderaEntraCiclo = true;
                            if (fechaInicio < fechaInicioHorometros && horasInicio != horaAcumuladaAnterior) horometroActual += horasInicio;
                        }
                    }
                    fechaFin = fechaInicio;
                    if (banderaEntraCiclo) horaAcumuladaAnterior = item.horasHistorial;
                    if (item.reciclado)
                    {
                        banderaReciclado = true;
                        break;
                    }
                }
                if (!banderaReciclado)
                {
                    if (componente != null) horometroActual += componente.horasCicloInicio;
                }
            }
            else
            {
                if (!trackActualReciclado)
                {
                    if (componente != null) horometroActual += componente.horasCicloInicio;
                }
            }
            return horometroActual;
        }

        public List<horometrosComponentesDTO> GetHrsAcumuladasComponentes(List<int> componentesIDs)
        {
            List<horometrosComponentesDTO> data = new List<horometrosComponentesDTO>();
            DateTime fechaMinima = DateTime.Now;
            DateTime fechaInicioHorometros = new DateTime(2011, 3, 28);
            bool banderaEntraCiclo = false;

            var componentesHorasInicio = _context.tblM_CatComponente.Where(x => componentesIDs.Contains(x.id)).Select(x => new { id = x.id, horasInicio = x.horasAcumuladasInicio }).ToList();

            var tracking = _context.tblM_trackComponentes.Where(x => componentesIDs.Contains(x.componenteID)).OrderByDescending(x => x.fecha).ThenByDescending(x => x.id).Select(x => new TrackingHistorialDTO
            {
                componenteID = x.componenteID,
                fecha = x.fecha ?? default(DateTime),
                locacion = x.locacion,
                reciclado = x.reciclado,
                tipoLocacion = x.tipoLocacion ?? false,
                horasHistorial = x.horasAcumuladas
            }).ToList();

            var trackingGrouped = _context.tblM_trackComponentes.Where(x => componentesIDs.Contains(x.componenteID)).Select(x => new TrackingHistorialDTO
            {
                componenteID = x.componenteID,
                fecha = x.fecha ?? default(DateTime),
                locacion = x.locacion,
                reciclado = x.reciclado,
                tipoLocacion = x.tipoLocacion ?? false,
                id = x.id
            }).GroupBy(x => x.componenteID).ToList();

            List<string> maquinas = new List<string>();
            foreach (var item in trackingGrouped)
            {
                foreach (var item2 in item.OrderByDescending(x => x.fecha).ThenByDescending(x => x.id))
                {
                    if (item2.tipoLocacion == false)
                    {
                        if (item2.fecha < fechaMinima) fechaMinima = item2.fecha;
                        maquinas.Add(item2.locacion);
                    }
                    //if (item2.reciclado) break;
                }
            }
            maquinas = maquinas.Distinct().ToList();

            var horometros = _context.tblM_CapHorometro.GroupJoin(maquinas, x => x.Economico, economico => economico, (x, economico) => new { x, economico })
                .Where(e => e.economico.Any() && e.x.Fecha >= fechaMinima).OrderByDescending(e => e.x.Fecha).ThenByDescending(e => e.x.turno)
                .Select(e => new HorasTrackingDTO
                {
                    Fecha = e.x.Fecha,
                    Economico = e.x.Economico,
                    HorometroAcumulado = e.x.HorometroAcumulado,
                    HorasTrabajo = e.x.HorasTrabajo,
                    turno = e.x.turno
                }).GroupBy(x => x.Economico).ToList();

            data = componentesIDs.Select(x =>
            {
                DateTime fechaFin = DateTime.Now;
                DateTime fechaInicio = DateTime.Now;
                var auxTracking = tracking.Where(y => y.componenteID == x).ToList();
                var auxMaquinasTrack = auxTracking.Select(y => y.locacion).ToList();
                decimal horometroActual = 0;
                var lstHorometros = horometros.Where(y => auxMaquinasTrack.Contains(y.Key)).SelectMany(y => y).OrderByDescending(y => y.Fecha).ThenByDescending(y => y.turno).ToList();
                var componente = _context.tblM_CatComponente.FirstOrDefault(y => y.id == x);
                decimal horaAnterior = componente.horaCicloActual;
                decimal horasInicio = 0;
                decimal horasFinal = 0;
                foreach (var item in auxTracking)
                {
                    fechaInicio = item.fecha;
                    if (item.tipoLocacion == false)
                    {
                        if (/*fechaInicio < fechaInicioHorometros ||*/ fechaFin < fechaInicioHorometros)
                        {
                            horometroActual += horaAnterior;
                            //if (fechaFin < fechaInicioHorometros) break;
                        }
                        else
                        {
                            var horometroInicial = lstHorometros.LastOrDefault(y => y.Economico == item.locacion && y.Fecha >= fechaInicio && y.Fecha <= fechaFin);
                            horasInicio = horometroInicial == null ? 0 : horometroInicial.HorometroAcumulado - horometroInicial.HorasTrabajo;

                            var horometroFinal = lstHorometros.FirstOrDefault(y => y.Economico == item.locacion && y.Fecha <= fechaFin && y.Fecha >= fechaInicio && y.HorometroAcumulado >= horasInicio);
                            horasFinal = horometroFinal == null ? 0 : horometroFinal.HorometroAcumulado;

                            decimal restaHorometros = horasFinal - horasInicio;
                            horometroActual += restaHorometros;
                            horasFinal = horasInicio;
                            banderaEntraCiclo = true;
                            if (fechaInicio < fechaInicioHorometros && horasInicio != horaAnterior) horometroActual += horasInicio;
                        }


                    }
                    if (banderaEntraCiclo) horaAnterior = item.horasHistorial;
                    fechaFin = fechaInicio;
                }
                var auxComponente = componentesHorasInicio.FirstOrDefault(y => y.id == x);
                if (auxComponente != null) horometroActual += auxComponente.horasInicio;
                return new horometrosComponentesDTO
                {
                    componenteID = x,
                    horometroActual = horometroActual
                };
            }).ToList();
            return data;
        }

        public decimal GetHrsAcumuladasComponente(int componenteID, DateTime fechaActual, bool esHistorial) //DESARROLLO
        {
            List<horometrosComponentesDTO> data = new List<horometrosComponentesDTO>();
            DateTime fechaMinima = DateTime.Now;
            DateTime fechaInicioHorometros = new DateTime(2011, 3, 28);

            var componente = _context.tblM_CatComponente.FirstOrDefault(x => x.id == componenteID);

            var tracking = _context.tblM_trackComponentes.Where(x => x.componenteID == componenteID && (esHistorial ? x.fecha < fechaActual : x.fecha <= fechaActual)).OrderByDescending(x => x.fecha).ThenByDescending(x => x.id).
                Select(x => new TrackingHistorialDTO
                {
                    componenteID = x.componenteID,
                    fecha = x.fecha ?? default(DateTime),
                    locacion = x.locacion,
                    reciclado = x.reciclado,
                    tipoLocacion = x.tipoLocacion ?? false,
                    horasHistorial = x.horasAcumuladas
                }
            ).ToList();

            var trackingSig = _context.tblM_trackComponentes.Where(x => x.componenteID == componenteID && x.fecha >= fechaActual).OrderBy(x => x.fecha).ThenBy(x => x.id).FirstOrDefault();

            decimal horometroActual = 0;

            if (tracking.Count() > 0)
            {
                var maquinas = tracking.Where(x => x.tipoLocacion == false).Select(x => x.locacion).Distinct().ToList();
                fechaMinima = tracking.Min(x => x.fecha);
                var horometros = _context.tblM_CapHorometro.GroupJoin(maquinas, x => x.Economico, economico => economico, (x, economico) => new { x, economico })
                    .Where(e => e.economico.Any() && e.x.Fecha >= fechaMinima).OrderByDescending(e => e.x.Fecha).ThenByDescending(e => e.x.turno)
                    .Select(e => new HorasTrackingDTO
                    {
                        Fecha = e.x.Fecha,
                        Economico = e.x.Economico,
                        HorometroAcumulado = e.x.HorometroAcumulado,
                        HorasTrabajo = e.x.HorasTrabajo,
                        turno = e.x.turno
                    }).GroupBy(x => x.Economico).ToList();

                DateTime fechaFin = fechaActual;
                DateTime fechaInicio = fechaActual;

                var lstHorometros = horometros.Where(y => maquinas.Contains(y.Key)).SelectMany(y => y).OrderByDescending(y => y.Fecha).ThenByDescending(y => y.turno).ToList();
                decimal horasInicio = 0;
                decimal horasFinal = 0;
                decimal horaAcumuladaAnterior = trackingSig == null ? componente.horasAcumuladas : trackingSig.horasAcumuladas;
                var banderaEntraCiclo = false;
                //bool banderaTrackingDESC = false;
                foreach (var item in tracking)
                {
                    fechaInicio = item.fecha;
                    //fechaInicio = DateTime.Now;
                    if (item.tipoLocacion == false)
                    {
                        if (/*fechaInicio < fechaInicioHorometros ||*/ fechaFin < fechaInicioHorometros)
                        {
                            horometroActual += horaAcumuladaAnterior;
                            //if (fechaFin < fechaInicioHorometros) break;
                        }
                        else
                        {
                            if (fechaInicio < fechaFin) //LA FECHA INICIAL DEBE SER MENOR A LA FECHA FINAL.
                            {
                                //banderaTrackingDESC = true;
                                var horometroInicial = lstHorometros.LastOrDefault(y => y.Economico == item.locacion && y.Fecha >= fechaInicio && y.Fecha <= fechaFin);
                                horasInicio = horometroInicial == null ? 0 : horometroInicial.HorometroAcumulado - horometroInicial.HorasTrabajo;

                                var horometroFinal = lstHorometros.FirstOrDefault(y => y.Economico == item.locacion && y.Fecha <= fechaFin && y.Fecha >= fechaInicio && y.HorometroAcumulado >= horasInicio);
                                horasFinal = horometroFinal == null ? 0 : horometroFinal.HorometroAcumulado;

                                if (horometroFinal == null)
                                {
                                    horometroFinal = lstHorometros.FirstOrDefault(y => y.Economico == item.locacion && y.Fecha <= fechaFin && y.Fecha >= fechaInicio);
                                    horasFinal = horometroFinal == null ? 0 : horometroFinal.HorometroAcumulado;

                                    horometroInicial = lstHorometros.LastOrDefault(y => y.Economico == item.locacion && y.Fecha >= fechaInicio && y.Fecha <= fechaFin && y.HorometroAcumulado < horasFinal);
                                    horasInicio = horometroInicial == null ? 0 : horometroInicial.HorometroAcumulado - horometroInicial.HorasTrabajo;
                                }

                                decimal restaHorometros = horasFinal - horasInicio;
                                horometroActual += restaHorometros;
                                banderaEntraCiclo = true;
                                if (fechaInicio < fechaInicioHorometros && horasInicio != horaAcumuladaAnterior) horometroActual += horasInicio;
                            }
                            //else if (!banderaTrackingDESC)
                            //{
                            //    //VERIFICAR LOGICA
                            //    var trackingASC = tracking.OrderBy(x => x.fecha).ToList();
                            //    foreach (var itemTrackingASC in trackingASC)
                            //    {
                            //        var lstHorometrosASC = lstHorometros.OrderBy(x => x.Fecha).ToList();

                            //        var horometroInicial = lstHorometrosASC.FirstOrDefault(y => y.Economico == itemTrackingASC.locacion && y.Fecha <= fechaInicio && y.Fecha >= fechaFin);
                            //        horasInicio = horometroInicial == null ? 0 : horometroInicial.HorometroAcumulado - horometroInicial.HorasTrabajo;

                            //        var horometroFinal = lstHorometrosASC.LastOrDefault(y => y.Economico == itemTrackingASC.locacion && y.Fecha >= fechaFin && y.Fecha <= fechaInicio && y.HorometroAcumulado >= horasInicio);
                            //        horasFinal = horometroFinal == null ? 0 : horometroFinal.HorometroAcumulado;

                            //        decimal restaHorometros = horasFinal - horasInicio;
                            //        horometroActual += restaHorometros;
                            //        banderaEntraCiclo = true;
                            //    }
                            //    //END: VERIFICAR LOGICA
                            //}
                        }
                    }
                    fechaFin = fechaInicio;
                    if (banderaEntraCiclo) horaAcumuladaAnterior = item.horasHistorial;
                }
            }
            if (componente != null) horometroActual += componente.horasAcumuladasInicio;
            return horometroActual;
        }

        private decimal CalcularHrsMaquina(int maquinaID, DateTime fecha)
        {
            decimal hrsMaquina = 0;
            try
            {
                var economico = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == maquinaID).noEconomico;
                hrsMaquina = _context.tblM_CapHorometro.OrderByDescending(x => x.Fecha).FirstOrDefault(x => x.Economico == economico && x.Fecha <= fecha).Horometro;
            }
            catch (Exception e)
            {
                hrsMaquina = 0;
            }
            return hrsMaquina;
        }

        public bool GuardarReporteActualizacion(tblM_ReporteRemocionComponente obj)
        {
            try
            {
                //Fecha de instalación del dia de hoy
                DateTime fInstCompRemovido = new DateTime();
                var tracking = new tblM_trackComponentes();
                var componente = _context.tblM_CatComponente.FirstOrDefault(x => x.id == obj.componenteRemovidoID);
                if (componente != null && componente.trackComponenteID != null) { tracking = _context.tblM_trackComponentes.FirstOrDefault(x => x.id == componente.trackComponenteID); }
                if (tracking != null)
                {
                    fInstCompRemovido = tracking.fecha ?? default(DateTime);
                    obj.horasComponente = tracking.horasCiclo;
                }
                var auxHrsMaquina = GetHorasMaquinaIDPorFecha(obj.maquinaID, obj.fechaRemocion);
                var auxHrsComponente = auxHrsMaquina - GetHorasMaquinaIDPorFecha(obj.maquinaID, fInstCompRemovido);
                obj.horasMaquina = auxHrsMaquina;
                obj.horasComponente = obj.horasComponente + auxHrsComponente;

                if (obj.id == 0)
                    SaveEntity(obj, (int)BitacoraEnum.REPORTEREMOCIONCOMPONENTE);
                else
                {
                    var reporte = _context.tblM_ReporteRemocionComponente.FirstOrDefault(x => x.id == obj.id);
                    if (reporte != null)
                    {
                        obj.fechaVoBo = reporte.fechaVoBo;
                        obj.fechaEnvio = reporte.fechaEnvio;
                        obj.fechaAutorizacion = reporte.fechaAutorizacion;
                        obj.realiza = reporte.realiza;
                    }
                    Update(obj, obj.id, (int)BitacoraEnum.REPORTEREMOCIONCOMPONENTE);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool Exists(tblM_ReporteRemocionComponente obj)
        {
            return _context.tblM_ReporteRemocionComponente.Where(x => x.componenteRemovidoID == obj.componenteRemovidoID && x.estatus != 2).ToList().Count > 0 ? true : false;
        }

        public RemocionDTO cargarDatosRemocionComponente(int idComponente)
        {
            RemocionDTO data = new RemocionDTO();


            var reporte = _context.tblM_ReporteRemocionComponente.FirstOrDefault(x => x.componenteRemovidoID == idComponente && x.estatus < 5);
            if (reporte != null)
            {
                var obra = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == reporte.maquina.centro_costos);

                data.descripcionComponente = //reporte.componenteRemovido.subConjunto.descripcion;
                    reporte.componenteRemovido.subConjunto.descripcion + " " + (reporte.componenteRemovido.posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)reporte.componenteRemovido.posicionID).ToUpper() : "");
                data.noEconomico = reporte.maquina.noEconomico;
                data.modelo = reporte.maquina.modeloEquipo.descripcion;

                data.numParteComponente = reporte.componenteRemovido.numParte;
                data.serieComponenteRemovido = reporte.componenteRemovido.noComponente;
                data.serieMaquina = reporte.maquina.noSerie;
                data.modeloID = reporte.maquina.modeloEquipoID;
                data.subconjuntoID = reporte.componenteRemovido.subConjuntoID ?? default(int);
                data.tipoLocacion = false;
                data.cc = obra != null ? obra.cc : "";
                data.nombreCC = obra != null ? obra.descripcion : "";
                data.idModelo = reporte.maquina.modeloEquipoID;
                data.fechaInstalacionRemovidoRaw = reporte.fechaInstalacionCRemovido ?? default(DateTime);
                data.fechaInstalacionRemovido = data.fechaInstalacionRemovidoRaw.ToString("dd/MM/yyyy");
                data.fechaInstalacionInstaladoRaw = reporte.fechaInstalacionCInstalado ?? default(DateTime);
                data.fechaInstalacionInstalado = data.fechaInstalacionInstaladoRaw.ToString("dd/MM/yyyy");
                //data.ultimaReparacion = getUltimaReparacion(idComponente);

                data.fechaNum = reporte.fechaRemocion;
                data.componenteRemovidoID = reporte.componenteRemovidoID;
                data.maquinaID = reporte.maquinaID;
                data.ccID = reporte.maquina.centro_costos;
                data.garantia = reporte.garantia;

                data.motivoID = reporte.motivoRemocionID;
                data.destinoID = reporte.destinoID;
                data.comentario = reporte.comentario;
                data.componenteInstaladoID = reporte.componenteInstaladoID;
                data.empresaResponsable = reporte.empresaResponsable;
                data.personal = reporte.personal;
                data.imgRemovido = reporte.imgComponenteRemovido;
                data.imgInstalado = reporte.imgComponenteInstalado;
                data.folioReporte = reporte.id;
                data.fecha = reporte.fechaRemocion.ToString("dd/MM/yyyy");
                data.estatus = reporte.estatus;
                data.empresaInstala = reporte.empresaInstala;
                data.horas = reporte.horasMaquina;
                data.horasComponenteRemovido = reporte.horasComponente;
            }
            else
            {
                var join = _context.tblM_CatComponente.Where(x => x.id == idComponente).Join(_context.tblM_trackComponentes, x => x.id, y => y.componenteID, ((x, y) => new { x, y })).GroupBy(x => x.x.id, (key, g) => g.OrderByDescending(e => e.y.fecha).ThenByDescending(e => e.y.id).FirstOrDefault()).Join(_context.tblM_CatMaquina, z => z.y.locacionID, w => w.id, ((z, w) => new { z, w })).FirstOrDefault();
                data.descripcionComponente = //join.z.x.subConjunto.descripcion;
                    join.z.x.subConjunto.descripcion + " " + (join.z.x.posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)join.z.x.posicionID).ToUpper() : "");
                data.noEconomico = join.w.noEconomico;
                data.modelo = _context.tblM_CatModeloEquipo.FirstOrDefault(x => x.id == (join.z.x.modeloEquipoID ?? default(int))).descripcion;

                data.numParteComponente = join.z.x.numParte;
                data.serieComponenteRemovido = join.z.x.noComponente;
                data.serieMaquina = join.w.noSerie;
                data.modeloID = join.z.x.modeloEquipoID ?? default(int);
                data.subconjuntoID = join.z.x.subConjuntoID ?? default(int);
                data.tipoLocacion = join.z.y.tipoLocacion ?? default(bool);
                data.cc = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == join.w.centro_costos).cc;
                data.nombreCC = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == join.w.centro_costos).descripcion;
                data.idModelo = join.w.modeloEquipoID;
                data.fechaInstalacionRemovidoRaw = join.z.x.fecha ?? default(DateTime);
                data.fechaInstalacionRemovido = data.fechaInstalacionRemovidoRaw.ToString("dd/MM/yyyy");
                //data.ultimaReparacion = getUltimaReparacion(idComponente);

                data.fechaNum = join.z.y.fecha ?? default(DateTime);
                data.componenteRemovidoID = join.z.x.id;
                data.maquinaID = join.w.id;
                data.ccID = join.w.centro_costos;
                data.garantia = true;

                var horometroEquipo = _context.tblM_CapHorometro.Where(x => x.Economico.Contains(join.w.noEconomico)).OrderByDescending(y => y.Fecha).FirstOrDefault();

                data.horas = horometroEquipo == null ? 0 : horometroEquipo.HorometroAcumulado;
                var horaCicloActual = GetHrsCicloActualComponente(join.z.x.id, DateTime.Today, join.z.y.id, false);
                data.horasComponenteRemovido = horaCicloActual;
                data.motivoID = -1;
                data.destinoID = -1;
                data.comentario = "";
                data.componenteInstaladoID = -1;
                data.empresaResponsable = -1;
                data.personal = "";
                data.imgInstalado = "";
                data.imgRemovido = "";
                data.folioReporte = 0;
                data.fecha = DateTime.Today.ToString("dd/MM/yyyy");
                data.estatus = -1;
                data.empresaInstala = -1;
            }
            return data;
        }

        private string getUltimaReparacion(int idComponente)
        {
            var data = "N/A";
            var tracking = _context.tblM_trackComponentes.Where(x => x.componenteID == idComponente).OrderByDescending(x => x.fecha).ThenByDescending(e => e.id).ToList();
            int i = 0;
            var estatus = 0;
            while (tracking.Count() > i && data == "N/A")
            {
                estatus = tracking.Skip(i).First().estatus;
                if (estatus == 2 || (estatus > 3 && estatus < 11))
                {
                    var ultimaReparacionLocacion = tracking.Skip(i).First().locacionID;
                    data = _context.tblM_CatLocacionesComponentes.FirstOrDefault(x => x.id == ultimaReparacionLocacion).descripcion;
                }
                i++;
            }
            return data;
        }

        public decimal GetHorasMaquina(string noEconomico)
        {
            try
            {
                return _context.tblM_CapHorometro.Where(x => x.Economico.Contains(noEconomico)).OrderByDescending(y => y.Fecha).FirstOrDefault().HorometroAcumulado;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public decimal GetHorasMaquinaPorFecha(string noEconomico, DateTime fecha)
        {
            try
            {
                return _context.tblM_CapHorometro.Where(x => x.Economico.Contains(noEconomico) && x.Fecha <= fecha).OrderByDescending(y => y.Fecha).FirstOrDefault().HorometroAcumulado;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public decimal GetHorasMaquinaIDPorFecha(int idMaquina, DateTime fecha)
        {
            try
            {
                var noEconomico = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == idMaquina).noEconomico;
                return _context.tblM_CapHorometro.Where(x => x.Economico.Contains(noEconomico) && x.Fecha <= fecha).OrderByDescending(y => y.Fecha).ThenByDescending(x => x.turno).FirstOrDefault().HorometroAcumulado;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public List<ComboDTO> cargarCboComponenteInstalado(int idModelo, int idSubconjunto)
        {
            List<ComboDTO> data = new List<ComboDTO>();
            List<int> subconjuntosCompatibles789 = new List<int> { 1, 61, 79, 77, 38, 32, 26, 15, 12, 7, 96, 82, 4, 7, 52, 50 };
            List<int> subconjuntosCompatibles994 = new List<int> { 1, 52, 53, 57, 60, 61, 32, 35, 38, 14, 15, 17, 27, 102, 101, 100, 107, 50, 108, 5, 6, 96, 66, 85, 2 };
            List<int> diferenciales994 = new List<int> { 52, 53, 57 };
            List<int> subconjuntosCompatibles775G_773F = new List<int> { 77 };
            List<int> subconjuntosCompatibles777G_777F = new List<int> { 77 };
            List<int> subconjuntosCompatibles777G_777D_777F = new List<int> { 38, 77 };
            List<int> subconjuntosCompatibles992K_992G = new List<int> { 69, 60, 61 };
            List<int> subconjuntosCompatiblesDM45_DML = new List<int> { 49, 29, 26 };

            var join = _context.tblM_CatComponente.Join(_context.tblM_trackComponentes, x => x.id, y => y.componenteID, ((x, y) => new { x, y })).GroupBy(x => x.x.id, (key, g) => g.OrderByDescending(e => e.y.fecha).ThenByDescending(e => e.y.id).FirstOrDefault());
            //Agregar casos especiales
            if ((idModelo == 90 || idModelo == 91) && (idSubconjunto == 6 || idSubconjunto == 32 || idSubconjunto == 52))
            {
                join = join.Where(z => (z.x.modeloEquipoID == 90 || z.x.modeloEquipoID == 91) && z.x.subConjuntoID == idSubconjunto && z.y.estatus == 1);
            }
            else if ((idModelo == 97 || idModelo == 3781) && (idSubconjunto == 10))
            {
                join = join.Where(z => (z.x.modeloEquipoID == 97 || z.x.modeloEquipoID == 3781) && z.x.subConjuntoID == idSubconjunto && z.y.estatus == 1);
            }
            else  if ((idModelo == 18 || idModelo == 3878) && (idSubconjunto == 44))
            {
                join = join.Where(z => (z.x.modeloEquipoID == 18 || z.x.modeloEquipoID == 3878) && z.x.subConjuntoID == idSubconjunto && z.y.estatus == 1);
            }
            else if (idSubconjunto == 105 || idSubconjunto == 106) 
            {
                join = join.Where(z => z.x.modeloEquipoID == idModelo && (z.x.subConjuntoID == idSubconjunto || z.x.subConjuntoID == 79) && z.y.estatus == 1); 
            }
            else if ((idModelo == 97 || idModelo == 1677) && idSubconjunto == 38) 
            {
                join = join.Where(z => (z.x.modeloEquipoID == 97 || z.x.modeloEquipoID == 1677) && z.x.subConjuntoID == idSubconjunto && z.y.estatus == 1);
            }
            else if ((idModelo == 3780 || idModelo == 6935) && subconjuntosCompatibles789.Contains(idSubconjunto))
            {                
                if (idSubconjunto == 79) {
                    join = join.Where(z => (z.x.modeloEquipoID == 3780 || z.x.modeloEquipoID == 6935) && (z.x.subConjuntoID == idSubconjunto || z.x.subConjuntoID == 105 || z.x.subConjuntoID == 106) && z.y.estatus == 1);
                }
                else { 
                    join = join.Where(z => (z.x.modeloEquipoID == 3780 || z.x.modeloEquipoID == 6935) && z.x.subConjuntoID == idSubconjunto && z.y.estatus == 1); 
                }
            }
            else if ((idModelo == 3781 || idModelo == 6936) && subconjuntosCompatibles994.Contains(idSubconjunto))
            {
                if (diferenciales994.Contains(idSubconjunto)) join = join.Where(z => (z.x.modeloEquipoID == 3781 || z.x.modeloEquipoID == 6936) && diferenciales994.Contains(z.x.subConjuntoID ?? 0) && z.y.estatus == 1);
                else join = join.Where(z => (z.x.modeloEquipoID == 3781 || z.x.modeloEquipoID == 6936) && z.x.subConjuntoID == idSubconjunto && z.y.estatus == 1);
            }
            else if ((idModelo == 57 || idModelo == 89) && subconjuntosCompatibles775G_773F.Contains(idSubconjunto))
            {
                join = join.Where(z => (z.x.modeloEquipoID == 57 || z.x.modeloEquipoID == 89) && z.x.subConjuntoID == idSubconjunto && z.y.estatus == 1);
            }
            else if ((idModelo == 90 || idModelo == 91) && subconjuntosCompatibles777G_777F.Contains(idSubconjunto))
            {
                join = join.Where(z => (z.x.modeloEquipoID == 90 || z.x.modeloEquipoID == 91) && z.x.subConjuntoID == idSubconjunto && z.y.estatus == 1);
            }
            else if ((idModelo == 1676 || idModelo == 91 || idModelo == 90) && subconjuntosCompatibles777G_777D_777F.Contains(idSubconjunto))
            {
                join = join.Where(z => (z.x.modeloEquipoID == 91 || z.x.modeloEquipoID == 1676 || z.x.modeloEquipoID == 90) && z.x.subConjuntoID == idSubconjunto && z.y.estatus == 1);
            }
            else if (idModelo == 97 && (idSubconjunto == 53 || idSubconjunto == 57))
            {
                join = join.Where(z => z.x.modeloEquipoID == idModelo && (z.x.subConjuntoID == 53 || z.x.subConjuntoID == 57) && z.y.estatus == 1); 
            }
            else if ((idModelo == 97 || idModelo == 1677) && subconjuntosCompatibles992K_992G.Contains(idSubconjunto))
            {
                join = join.Where(z => (z.x.modeloEquipoID == 97 || z.x.modeloEquipoID == 1677) && z.x.subConjuntoID == idSubconjunto && z.y.estatus == 1);
            }
            else if (idSubconjunto == 79)
            {
                join = join.Where(z => z.x.modeloEquipoID == idModelo && (z.x.subConjuntoID == idSubconjunto || z.x.subConjuntoID == 105 || z.x.subConjuntoID == 106) && z.y.estatus == 1);
            }
            else if ((idModelo == 18 || idModelo == 3878) && subconjuntosCompatiblesDM45_DML.Contains(idSubconjunto))
            {
                join = join.Where(z => (z.x.modeloEquipoID == 18 || z.x.modeloEquipoID == 3878) && z.x.subConjuntoID == idSubconjunto && z.y.estatus == 1);
            }
            else
            {                    
                join = join.Where(z => z.x.modeloEquipoID == idModelo && z.x.subConjuntoID == idSubconjunto && z.y.estatus == 1);
            }      
            
            var join2 = join.Join(_context.tblM_CatLocacionesComponentes, x => x.y.locacionID, y => y.id, ((x, y) => new { x, y })).ToList();
            foreach (var item in join2)
            {
                ComboDTO aux = new ComboDTO();
                var subconjunto = _context.tblM_CatSubConjunto.FirstOrDefault(x => x.id == item.x.x.subConjuntoID);
                var auxSubconjunto = subconjunto != null ? subconjunto.descripcion : "";
                var auxPosicion = (item.x.x.posicionID > 0 ? EnumHelper.GetDescription((PosicionesEnum)item.x.x.posicionID).ToUpper() : "");
                aux.Value = item.x.x.id.ToString(); // item.x.id.ToString();
                aux.Text = item.x.x.noComponente + " - " + auxSubconjunto + " " + auxPosicion + " (" + item.y.descripcion + ")";
                aux.Prefijo = item.x.x.noComponente;
                data.Add(aux);
            }
            return data;
        }

        public List<ComboDTO> cargarCboPersonal(string cc)
        {
            List<ComboDTO> data = new List<ComboDTO>();
            var personalOverhaul = _context.tblP_Usuario.Where(x => x.cc == cc).ToList();
            foreach (var item in personalOverhaul)
            {
                ComboDTO aux = new ComboDTO();
                aux.Value = item.id.ToString();
                aux.Text = item.nombre + " " + item.apellidoPaterno + " " + item.apellidoMaterno;
                data.Add(aux);
            }
            return data;
        }

        public List<ComboDTO> getCatEmpleados()
        {

            List<ComboDTO> data = new List<ComboDTO>();
            var getCatEmpleado =
                @"SELECT " +
                    "a.clave_empleado AS Value, " +
                    "(LTRIM(RTRIM(a.nombre))+' '+replace(a.ape_paterno, ' ', '')+' '+replace(a.ape_materno, ' ', '')) AS Text, " +
                    "a.CC_Contable AS Prefijo " +
                "FROM DBA.sn_empleados a "; //+
            //"WHERE (CC_Contable = '" + CentroCostos + "' OR CC_Contable = '988')";
            try
            {
                //var resultado = (IList<ComboDTO>)ContextEnKontrolNominaArrendadora.Where(getCatEmpleado, 1).ToObject<IList<ComboDTO>>();

                var resultado = _context.Select<ComboDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT 
                                    a.clave_empleado AS Value, 
                                    (LTRIM(RTRIM(a.nombre))+' '+replace(a.ape_paterno, ' ', '')+' '+replace(a.ape_materno, ' ', '')) AS Text, 
                                    a.CC_Contable AS Prefijo 
                                FROM tblRH_EK_Empleados a ",
                });

                foreach (var item in resultado) { data.Add(item); }
            }
            catch { }
            //try
            //{
            //    var resultado2 = (IList<ComboDTO>)ContextEnKontrolNominaArrendadora.Where(getCatEmpleado, 2).ToObject<IList<ComboDTO>>();
            //    foreach (var item in resultado2) { data.Add(item); }
            //}
            //catch { }
            return data;
        }

        public string getPuestoDescripcion(int idPuesto)
        {
            return _context.tblP_Puesto.FirstOrDefault(x => x.id == idPuesto).descripcion;
        }

        public List<ComboDTO> getMaquinasByModelo(int idModelo, int tipoLocacion)
        {
            List<ComboDTO> data = new List<ComboDTO>();
            if (tipoLocacion == 2)
            {
                var locaciones = _context.tblM_CatLocacionesComponentes.Where(x => x.estatus == true && x.tipoLocacion == 1).OrderBy(x => x.tipoLocacion).ToList();
                foreach (var item in locaciones)
                {
                    ComboDTO aux = new ComboDTO();
                    aux.Value = item.id.ToString();
                    aux.Text = item.descripcion;
                    data.Add(aux);
                }
            }
            else
            {
                var locaciones = _context.tblM_CatLocacionesComponentes.Where(x => x.estatus == true && x.tipoLocacion != 3).OrderBy(x => x.tipoLocacion).ToList();
                foreach (var item in locaciones)
                {
                    ComboDTO aux = new ComboDTO();
                    aux.Value = item.id.ToString();
                    aux.Text = item.descripcion;
                    data.Add(aux);
                }
            }
            return data;
        }

        public string getDescripcionCC(string cc)
        {
            var data = _context.tblP_CC.FirstOrDefault(x => x.cc == cc);
            if (data == null) { return cc; }
            else { return data.descripcion; }
        }

        public List<tblM_ReporteRemocionComponente> cargarReportes(int estatus, string descripcionComponente, string noEconomico, int motivoRemocion, DateTime? fechaInicio, DateTime? fechaFinal, List<string> cc, List<int> modelos, string noComponente)
        {
            List<int> estados = new List<int>();
            noEconomico = noEconomico.Trim().ToUpper();
            if (cc == null) cc = new List<string>();
            if (modelos == null) modelos = new List<int>();
            DateTime auxFechaInicio = fechaInicio ?? default(DateTime);
            DateTime auxFechaFin = fechaFinal ?? DateTime.Today;
            switch (estatus)
            {
                case -2:
                    estados.Add(3);
                    estados.Add(4);
                    estados.Add(5);
                    break;
                case 0:
                    estados.Add(0);
                    estados.Add(1);
                    break;
                case 1:
                    estados.Add(2);
                    break;
                case 2:
                    estados.Add(3);
                    estados.Add(4);
                    break;
                case 5:
                    estados.Add(5);
                    break;
                case 6:
                    estados.Add(6);
                    break;
                default:
                    estados.Add(-1);
                    break;
            }
            List<string> areaCuentaActuales = new List<string>();
            areaCuentaActuales = _context.tblP_CC.Where(x => cc.Contains(x.areaCuenta)).Select(x => x.cc).ToList();
            var data = _context.tblM_ReporteRemocionComponente
                .Where(x => 
                    (cc.Count() > 0 ? areaCuentaActuales.Contains(x.areaCuenta) : true) 
                    && (estatus == -1 ? (x.fechaAutorizacion == null || x.fechaEnvio == null) : estados.Contains(x.estatus)) 
                    && (descripcionComponente == "" ? true : x.componenteRemovido.subConjunto.descripcion == descripcionComponente)
                    && (noEconomico == "-1" ? true : x.maquina.noEconomico.Contains(noEconomico))
                    && (motivoRemocion == -1 ? true : x.motivoRemocionID == motivoRemocion)
                    && (fechaInicio == null ? true : x.fechaRemocion >= auxFechaInicio)
                    && (fechaFinal == null ? true : x.fechaRemocion <= auxFechaFin)
                    && x.estatus < 6
                    && x.motivoRemocionID < 5 
                    && x.componenteRemovido.noComponente.Contains(noComponente) 
                    && (modelos.Count() > 0 ? modelos.Contains(x.maquina.modeloEquipoID) : true)
            ).ToList();
            foreach (var item in data) 
            {
                var aux = _context.tblM_trackComponentes.FirstOrDefault(x => x.fecha == item.fechaRemocion && x.componenteID == item.componenteRemovidoID);
                if (aux != null) item.horasMaquina = aux.costoCRC;
            }
            return data;
        }

        public void Eliminar(int id)
        {
            if (ExistsByID(id))
            {
                var aux = _context.tblM_ReporteRemocionComponente.FirstOrDefault(x => x.id == id && x.estatus < 2);
                _context.tblM_ReporteRemocionComponente.Remove(aux);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("No se encuenta el registro que desea eliminar");
            }
        }

        private bool ExistsByID(int id)
        {
            return _context.tblM_ReporteRemocionComponente.Where(x => x.id == id && x.estatus < 2).ToList().Count > 0 ? true : false;
        }

        public tblM_ReporteRemocionComponente getReporteRemocionByID(int idReporte)
        {
            var data = _context.tblM_ReporteRemocionComponente.FirstOrDefault(x => x.id == idReporte);
            return data;
        }

        public string getCC(string areaCuenta)
        {
            try
            {
                return _context.tblP_CC.FirstOrDefault(x => x.cc == areaCuenta).descripcion;
            }
            catch (Exception)
            {
                return areaCuenta;
            }
        }

        public bool verificarReporte(int idReporte)
        {
            var reporte = _context.tblM_ReporteRemocionComponente.FirstOrDefault(x => x.id == idReporte);
            if (reporte.estatus == 1/* && reporte.imgComponenteInstalado != null && reporte.componenteInstaladoID != 0*/)
            {
                reporte.estatus = 2;
                reporte.fechaVoBo = DateTime.Now;
                Guardar(reporte, 1);
                return true;
            }
            else
            {
                /*reporte.estatus = 2;*/
                return true;
            }
        }

        public bool enviarReporte(int idReporte, int trackID)
        {
            var reporte = _context.tblM_ReporteRemocionComponente.FirstOrDefault(x => x.id == idReporte);
            if (reporte.estatus == 2)
            {
                if (reporte.imgComponenteInstalado != null && reporte.empresaInstala != null && reporte.fechaInstalacionCInstalado != null) { reporte.estatus = 4; }
                else { reporte.estatus = 3; }
                reporte.trackID = trackID;
            }
            reporte.fechaEnvio = DateTime.Now; 
            Guardar(reporte, 1);
            return true;
        }

        public bool aprobarReporte(int idReporte)
        {
            var reporte = _context.tblM_ReporteRemocionComponente.FirstOrDefault(x => x.id == idReporte && x.estatus != 5);
            if (reporte.estatus == 4)
            {
                reporte.estatus = 5;
            }
            reporte.fechaAutorizacion = DateTime.Now;    
            Guardar(reporte, 1);
            return true;
        }

        public bool UpdateReporteDesecho(int idReporte)
        {
            var reporte = _context.tblM_ReporteRemocionComponente.FirstOrDefault(x => x.id == idReporte);
            if (reporte.estatus == 5)
            {
                reporte.estatus = 6;
                Guardar(reporte, 1);
                return true;
            }
            else { return true; }
        }

        public DateTime fechaInstalacion(int idComponente)
        {
            DateTime fecha = new DateTime();
            var tracking = _context.tblM_trackComponentes.Where(x => x.componenteID == idComponente).OrderByDescending(x => x.fecha).ThenByDescending(e => e.id).FirstOrDefault();
            if (tracking != null)
                fecha = tracking.fecha ?? default(DateTime);
            return fecha;
        }

        public bool EliminarReporteRemocionByID(int idReporte)
        {
            try
            {
                var reporte = _context.tblM_ReporteRemocionComponente.FirstOrDefault(x => x.id == idReporte);
                _context.tblM_ReporteRemocionComponente.Remove(reporte);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public List<ComboDTO> getEmpleadosRemocion(string term)
        {
            string auxTermino = term + "%";
            List<ComboDTO> data = new List<ComboDTO>();
            List<string> terminos = term.Split(' ').ToList();
            if (terminos.Count() > 1)
            {
                auxTermino = "%";
                foreach (var item in terminos)
                {
                    auxTermino += item;
                    auxTermino += "%";
                }
            }
            var getCatEmpleado =
                @"SELECT TOP 10 " +
                    "a.clave_empleado AS Value, " +
                    "(LTRIM(RTRIM(a.nombre))+' '+replace(a.ape_paterno, ' ', '')+' '+replace(a.ape_materno, ' ', '')) AS Text, " +
                    "a.CC_Contable AS Prefijo " +
                "FROM DBA.sn_empleados a WHERE Text LIKE '" + auxTermino + "'";
            try
            {
                //var resultado = (IList<ComboDTO>)ContextEnKontrolNominaArrendadora.Where(getCatEmpleado, 1).ToObject<IList<ComboDTO>>();

                var resultado = _context.Select<ComboDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT 
                                    a.clave_empleado AS Value, 
                                    (LTRIM(RTRIM(a.nombre))+' '+replace(a.ape_paterno, ' ', '')+' '+replace(a.ape_materno, ' ', '')) AS Text, 
                                    a.CC_Contable AS Prefijo 
                                FROM tblRH_EK_Empleados a WHERE (LTRIM(RTRIM(a.nombre))+' '+replace(a.ape_paterno, ' ', '')+' '+replace(a.ape_materno, ' ', '')) LIKE '%" + auxTermino + "%'",
                });

                foreach (var item in resultado) { data.Add(item); }
            }
            catch { }
            return data;
        }

        public bool EliminarArchivoTrackComponentes(int idArchivo, int idComponente)
        {
            try
            {
                ModeloArchivoDTO trackComponentesDTO = new ModeloArchivoDTO();
                var getArchivoJSON = _context.tblM_trackComponentes.Where(x => x.id == idComponente).Select(x => x.JsonArchivos).ToList();
                string strArchivoJSON = getArchivoJSON[0].ToString();

                List<ModeloArchivoDTO> lstArchivos = JsonConvert.DeserializeObject<List<ModeloArchivoDTO>>(strArchivoJSON);
                foreach (var item in lstArchivos)
                {
                    if (item.id == idArchivo)
                    {
                        var itemRemove = lstArchivos.SingleOrDefault(x => x.id == item.id);
                        lstArchivos.Remove(itemRemove);
                        break;
                    }
                }
                string serealizarLstArchivos = JsonUtils.convertNetObjectToJson(lstArchivos);

                var EliminarArchivo = _context.tblM_trackComponentes.FirstOrDefault(x => x.id == idComponente);
                EliminarArchivo.JsonArchivos = serealizarLstArchivos;
                _context.SaveChanges();

                SaveBitacora(0, (int)AccionEnum.ELIMINAR, idComponente, JsonUtils.convertNetObjectToJson(EliminarArchivo));
                return true;
            }
            catch (Exception e)
            {
                LogError(2, 0, "OverhaulController", "EliminarArchivoTrackComponentes", e, AccionEnum.ELIMINAR, idArchivo, 0);
                return false;
            }
        }
    }
}