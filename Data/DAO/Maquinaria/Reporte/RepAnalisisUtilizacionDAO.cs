using Core.DAO.Maquinaria.Reporte;
using Core.DTO.Maquinaria.Reporte.RepAnalisisUtilizacion;
using Core.DTO.Principal.Generales;
using Core.Entity.Maquinaria.Captura;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Data.Factory.Maquinaria.Captura;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Reporte
{
    public class RepAnalisisUtilizacionDAO : GenericDAO<tblM_CapConciliacionHorometros>, IRepAnalisisUtilizacionDAO
    {
        ConciliacionFactoryServices conciliacionFS = new ConciliacionFactoryServices();
        public List<AnalisisDTO> getAnalisis(BusqAnalisiDTO busq)
        {
            var lstAnalisis = new List<AnalisisDTO>();
            try
            {
                var numDias = (decimal)(busq.fin - busq.ini).TotalDays;
                var lstMaq = _context.tblM_CatMaquina
                    .ToList()
                    .Where(m => !string.IsNullOrEmpty(m.noEconomico))
                    .Where(m => busq.noEco != null ? busq.noEco.Contains(m.noEconomico) : true)
                    .Where(m => busq.tipo.Equals(0) ? true : busq.tipo.Equals(m.grupoMaquinaria.tipoEquipoID))
                    .Where(m => busq.grupo == null ? true : busq.grupo.Any(g => g.Equals(m.grupoMaquinariaID)))
                    .Where(m => busq.modelo == null ? true : busq.modelo.Any(a => a.Equals(m.modeloEquipoID)))
                    .ToList();
                var cc = _context.tblP_CC
                    .ToList()
                    .Where(c => c.id.Equals(busq.cc))
                    .ToList();
                var lstCaratula = _context.tblM_EncCaratula
                    .Where(c => c.ccID.Equals(busq.cc))
                    .Where(c => busq.moneda.Equals(0) ? true : c.moneda.Equals(busq.moneda))
                    .ToList();
                var lstCapCaratual = _context.tblM_CapCaratula
                    .ToList()
                    .Where(c => lstCaratula.Any(e => e.id.Equals(c.idCaratula)))
                    .Where(c => busq.tipo.Equals(0) || busq.grupo == null ? true : busq.grupo.Any(g => g.Equals(c.idGrupo)))
                    .Where(c => busq.modelo == null ? true : busq.modelo.Any(m => m.Equals(c.idModelo)))
                    .ToList();
                var lstHoromentros = _context.tblM_CapHorometro
                    .Where(w => busq.ini <= w.Fecha && busq.fin >= w.Fecha).ToList()
                    .Where(w => lstMaq.Any(m => m.noEconomico.Equals(w.Economico)))
                    .Where(w => cc.Any(a => a.areaCuenta.Equals(w.CC)))
                    
                    .ToList();
                lstAnalisis.AddRange(
                    lstHoromentros
                    .GroupBy(g => g.Economico)
                    .Select(h => new
                    {
                        noEco = h.Key,
                        idGrupo = lstMaq.FirstOrDefault(m => m.noEconomico.Equals(h.Key)).grupoMaquinariaID,
                        idModelo = lstMaq.FirstOrDefault(m => m.noEconomico.Equals(h.Key)).modeloEquipoID,
                        modelo = lstMaq.FirstOrDefault(m => m.noEconomico.Equals(h.Key)).modeloEquipo.descripcion,
                        grupo = lstMaq.FirstOrDefault(m => m.noEconomico.Equals(h.Key)).grupoMaquinaria.descripcion,
                        hi = h.Min(c => c.Horometro - c.HorasTrabajo),
                        hf = h.Max(c => c.Horometro),
                        hTrabajo = lstHoromentros.Where(w => h.Key.Equals(w.Economico)).Sum(s => s.HorasTrabajo),
                        cc = lstMaq.FirstOrDefault(m => m.noEconomico.Equals(h.Key)).centro_costos,
                        costo = lstMaq.Any(m => h.Key.Equals(m.noEconomico) && lstCapCaratual.Any(c => c.idGrupo.Equals(m.grupoMaquinariaID) && c.idModelo.Equals(m.modeloEquipoID))) ? lstCapCaratual.FirstOrDefault(cap => cap.idGrupo.Equals(lstMaq.FirstOrDefault(m => m.noEconomico.Equals(h.Key)).grupoMaquinariaID) && cap.idModelo.Equals(lstMaq.FirstOrDefault(m => m.noEconomico.Equals(h.Key)).modeloEquipoID)).costo : 0,
                        moneda = lstMaq.Any(m => h.Key.Equals(m.noEconomico) && lstCapCaratual.Any(c => c.idGrupo.Equals(m.grupoMaquinariaID) && c.idModelo.Equals(m.modeloEquipoID))) ? lstCaratula.FirstOrDefault(car => lstCapCaratual.FirstOrDefault(cap => cap.idGrupo.Equals(lstMaq.FirstOrDefault(m => m.noEconomico.Equals(h.Key)).grupoMaquinariaID) && cap.idModelo.Equals(lstMaq.FirstOrDefault(m => m.noEconomico.Equals(h.Key)).modeloEquipoID)).idCaratula.Equals(car.id)).moneda : 1
                    })
                    .Select(h => new AnalisisDTO
                    {
                        noEco = h.noEco,
                        grupo = h.grupo,
                        modelo = h.modelo,
                        hi = h.hi,
                        hf = h.hf,
                        ht = h.hf - h.hi,
                        promSem = (h.hTrabajo / numDias) * 7,
                        totalMX = h.moneda.Equals(1) ? h.hTrabajo * h.costo : 0,
                        totalUSD = h.moneda.Equals(2) ? h.hTrabajo * h.costo : 0,
                        cc = h.cc
                    })
                    .Where(w => w.promSem >= busq.ritmoMin && w.promSem <= busq.ritmoMax)
                    .OrderBy(o => o.noEco)
                    .ToList());
            }
            catch (Exception) { }
            return lstAnalisis;
        }
        public List<RepAnalisisDTO> getRepAnalisisUtilizacion(BusqAnalisiDTO busq)
        {
            var lstRep = new List<RepAnalisisDTO>();
            try
            {

                var cc = _context.tblP_CC
                    .Where(c => c.id == busq.cc)
                    .ToList();
                var lstSol = _context.tblM_SolicitudEquipo
                    .ToList()
                    .Where(s => cc.Any(c => c.areaCuenta.Equals(s.CC)))
                    .Where(s => s.fechaElaboracion <= busq.fin)
                    .ToList();
                var lstMaq = _context.tblM_CatMaquina
                    .ToList()
                    .Where(m => !string.IsNullOrEmpty(m.noEconomico))
                    .Where(m => (busq.grupo.Count > 0 ? busq.grupo.Contains(m.grupoMaquinariaID) : true) && (busq.modelo.Count > 0 ? busq.modelo.Contains(m.modeloEquipoID) : true) && (busq.noEco.Count > 0 ? busq.noEco.Contains(m.noEconomico) : true))
                    .ToList();
                var lstAsign = _context.tblM_AsignacionEquipos
                    .ToList()
                    .Where(a => lstSol.Any(s => s.id == a.solicitudEquipoID))
                    .Where(a => lstMaq.Any(m => m.id == a.noEconomicoID))
                    .ToList();
                var lstDet = _context.tblM_SolicitudEquipoDet
                    .ToList()
                    .Where(d => lstSol.Any(s => s.id == d.solicitudEquipoID))
                    .Where(d => lstMaq.Any(m => m.grupoMaquinariaID == d.grupoMaquinariaID))
                    .Where(d => lstMaq.Any(m => m.modeloEquipoID == d.modeloEquipoID))
                    .ToList();
                var lstHoro = _context.tblM_CapHorometro
                    .Where(h => h.Fecha <= busq.fin)
                    .ToList()
                    .Where(h => lstMaq.Any(m => m.noEconomico.Equals(h.Economico)))
                    .Where(h => cc.Any(c => c.areaCuenta.Equals(h.CC)))
                    
                    .ToList();
                /*var temp = lstMaq
                    .Where(m =>
                        lstHoro.Any(h => h.Economico.Equals(m.noEconomico)) && lstAsign.Any(a => a.noEconomicoID == m.id) &&
                        lstDet.Any(d => d.grupoMaquinariaID == m.grupoMaquinariaID &&
                        d.modeloEquipoID == m.modeloEquipoID)
                    ).GroupBy(g =>
                        new { g.grupoMaquinariaID, g.modeloEquipoID }
                    ).Select(g => new
                    {
                        grupo = g.FirstOrDefault().grupoMaquinaria.descripcion,
                        modelo = g.FirstOrDefault().modeloEquipo.descripcion,
                        horo = lstHoro.Where(h => g.Any(m => m.noEconomico.Equals(h.Economico))).ToList(),
                        sol = lstAsign.Where(a => g.Any(m => m.id == a.noEconomicoID)).Select(s => new
                        {
                            sol = s.SolicitudEquipo,
                            det = lstDet.Where(d => d.id == s.SolicitudDetalleId && d.grupoMaquinariaID == g.Key.grupoMaquinariaID && d.modeloEquipoID == g.Key.modeloEquipoID).ToList(),
                            asig = lstAsign.Where(a => g.Any(m => m.id == a.noEconomicoID)).ToList()
                        }).ToList(),
                        detIni = lstAsign.Where(a => g.Any(m => m.id == a.noEconomicoID)).Min(d => d.fechaInicio.Date),
                        detFin = lstAsign.Where(a => g.Any(m => m.id == a.noEconomicoID)).Max(d => d.fechaFin.Date),
                        horoIni = lstHoro.Where(h => g.Any(m => m.noEconomico.Equals(h.Economico))).Min(h => h.Fecha.Date)
                    })*/
                ;

                lstRep.AddRange(
                    lstMaq
                    .Where(m =>
                        lstHoro.Any(h => h.Economico.Equals(m.noEconomico)) && lstAsign.Any(a => a.noEconomicoID == m.id) /*&&
                        lstDet.Any(d => d.grupoMaquinariaID == m.grupoMaquinariaID &&
                        d.modeloEquipoID == m.modeloEquipoID)*/
                    ).GroupBy(g =>
                        new { g.grupoMaquinariaID, g.modeloEquipoID }
                    ).Select(g => new
                    {
                        grupo = g.FirstOrDefault().grupoMaquinaria.descripcion,
                        grupoID = g.FirstOrDefault().grupoMaquinariaID,
                        modelo = g.FirstOrDefault().modeloEquipo.descripcion,
                        modeloID = g.FirstOrDefault().modeloEquipoID,
                        economicos = g.Select(x => new
                        {
                            id = x.id,
                            Economico = x.noEconomico
                        }),
                        horo = lstHoro.Where(h => g.Any(m => m.noEconomico.Equals(h.Economico))).ToList(),
                        sol = lstAsign.Where(a => g.Any(m => m.id == a.noEconomicoID)).Select(s => new
                        {
                            sol = s.SolicitudEquipo,
                            det = lstDet.Where(d => d.id == s.SolicitudDetalleId && d.grupoMaquinariaID == g.Key.grupoMaquinariaID && d.modeloEquipoID == g.Key.modeloEquipoID).ToList(),
                            asig = lstAsign.Where(a => g.Any(m => m.id == a.noEconomicoID)).ToList()
                        }).ToList(),
                        detIni = lstAsign.Where(a => g.Any(m => m.id == a.noEconomicoID)).Min(d => d.fechaInicio.Date),
                        detFin = lstAsign.Where(a => g.Any(m => m.id == a.noEconomicoID)).Max(d => d.fechaFin.Date),
                        horoIni = lstHoro.Where(h => g.Any(m => m.noEconomico.Equals(h.Economico))).Min(h => h.Fecha.Date)
                    }).Select(g => new
                    {
                        grupo = g.grupo,
                        modelo = g.modelo,
                        requerido = g.sol.SelectMany(a => a.det).Where(x => x.estatus && x.modeloEquipoID == g.modeloID && g.sol.Select(s => s.sol)
                            .Where(so => so.ArranqueObra).Select(y => y.id).Contains(x.solicitudEquipoID)).Count(),
                        adicional = g.sol.SelectMany(a => a.det).Where(x => x.estatus && x.modeloEquipoID == g.modeloID).Count()
                        - g.sol.SelectMany(a => a.det).Where(x => x.estatus && x.modeloEquipoID == g.modeloID && g.sol.Select(s => s.sol)
                            .Where(so => so.ArranqueObra).Select(y => y.id).Contains(x.solicitudEquipoID)).Count(),
                        asigTotal = lstMaq.Where(x => x.estatus == 1 && x.modeloEquipoID == g.modeloID && cc.Select(y => y.areaCuenta).Contains(x.centro_costos)).Count(),
                        mesEjecucion = (decimal)((g.detFin - g.horoIni).TotalDays / 30.4),
                        horasIniciales = g.sol.SelectMany(a => a.det).Where(x => x.estatus && x.modeloEquipoID == g.modeloID && g.sol.Select(s => s.sol)
                            .Where(so => so.ArranqueObra).Select(y => y.id).Contains(x.solicitudEquipoID)).Sum(z => z.horas),
                        horasAdicional = g.sol.SelectMany(a => a.det).Where(x => x.estatus && x.modeloEquipoID == g.modeloID).Sum(z => z.horas) - g.sol.SelectMany(a => a.det)
                        .Where(x => x.estatus && x.modeloEquipoID == g.modeloID && g.sol.Select(s => s.sol).Where(so => so.ArranqueObra).Select(y => y.id).Contains(x.solicitudEquipoID))
                        .Sum(z => z.horas),
                        inicio = g.horoIni,
                        fin = g.detFin,
                        ejecutadaMes = (decimal)((busq.fin - g.horoIni).TotalDays / 30.4),
                        ejecutadaHoras = (decimal)(busq.fin - g.horoIni).TotalHours,

                        //ejecutadaHoras2 = g.horo.Sum(x => x.HorasTrabajo),
                        ejecutadaHoras2 = g.horo.Where(hor => hor.Fecha >= g.horoIni && g.economicos.Where(m => (g.sol.SelectMany(x => x.asig).Where(x => g.sol.SelectMany(a => a.det).Where(y => y.estatus && y.modeloEquipoID == g.modeloID).Select(y => y.id).Contains(x.SolicitudDetalleId)).Select(z => z.noEconomicoID)).Contains(m.id)).Select(w => w.Economico).Contains(hor.Economico)).Sum(w => w.HorasTrabajo),

                        porEjecutarMes = (decimal)((g.detFin - busq.fin).TotalDays / 30.4),
                        porEjecutarHoras = (decimal)(g.detFin - busq.fin).TotalHours,
                        //comentarios = string.Join(".", g.asig.Where(d => !string.IsNullOrEmpty(d.Comentario)).Select(d => d.Comentario)) ?? string.Empty,
                        //detTotal = g.sol.Sum(s => s.det.Count)
                    }).Select(g => new RepAnalisisDTO()
                    {
                        grupo = g.grupo,
                        modelo = g.modelo,
                        requerido = g.requerido,
                        adicional = g.adicional,
                        existente = g.asigTotal,
                        mesEjecucion = g.mesEjecucion,
                        horasIniciales = g.horasIniciales,
                        horasAdicional = g.horasAdicional,
                        inicio = g.inicio,
                        fin = g.fin,
                        ejecutadaMes = g.ejecutadaMes,
                        //ejecutadaHoras = g.ejecutadaHoras,
                        ejecutadaHoras = g.ejecutadaHoras2,
                        porEjecutarMes = g.porEjecutarMes,
                        porEjecutarHoras = (g.horasIniciales + g.horasAdicional) - g.ejecutadaHoras2,
                        utilizacion = g.horasIniciales == 0 && g.horasAdicional == 0 ? 0 : g.ejecutadaHoras2 * 100 / (g.horasIniciales + g.horasAdicional),
                        comentarios = string.Empty
                    })
                    .ToList()
                    .OrderBy(o => o.grupo)
                    .ThenBy(o => o.modelo)
                    .ToList()
                    );
            }
            catch (Exception) { }
            return lstRep;
        }
        #region combobox
        public List<ComboDTO> cboAC()
        {
            try
            {
                var lstEC = _context.tblM_EncCaratula.ToList();
                var lstCC = _context.tblP_CC.ToList()
                    .Where(cc => lstEC.Any(ec => ec.ccID.Equals(cc.id)))
                    .ToList();
                return lstCC
                    .OrderBy(o => o.area)
                    .ThenBy(o => o.cuenta)
                    .Select(x => new ComboDTO
                    {
                        Text = string.Format("{0} {1}", x.areaCuenta, x.descripcion),
                        Value = x.id.ToString(),
                    })
                .ToList();
            }
            catch (Exception)
            {
                return new List<ComboDTO>();
            }

        }
        #endregion
    }
}