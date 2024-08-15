using Core.DAO.Maquinaria.Captura;
using Core.DTO.Maquinaria;
using Core.DTO.Maquinaria.Captura;
using Core.DTO.Maquinaria.Captura.KPI;
using Core.DTO.Maquinaria.Captura.OT;
using Core.DTO.RecursosHumanos;
using Core.DTO.Principal.Usuarios;
using Core.Entity.Maquinaria;
using Core.Entity.Maquinaria.Captura;
using Core.Enum.Maquinaria;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.ComponentModel;
using System.Reflection;
using Core.Entity.Principal.Usuarios;
using Core.DTO.Utils.Data;
using Core.DTO;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Principal.Multiempresa;

namespace Data.DAO.Maquinaria
{
    public class KPIDAO : GenericDAO<tblM_KPI>, IKPIDAO
    {
        public struct stTop3Paros
        {
            public string tipo { get; set; }
            public int cantidad { get; set; }
        }

        private List<string> _meses = new List<string>
        {
            "enero",
            "febrero",
            "marzo",
            "abril",
            "mayo",
            "junio",
            "julio",
            "agosto",
            "septiembre",
            "octubre",
            "noviembre",
            "diciembre"
        };

        #region MEJORA

        private List<MaquinaKPIGeneralDTO> _maquinas = new List<MaquinaKPIGeneralDTO>();
        private List<OrdenKPIDTO> _ordenes = new List<OrdenKPIDTO>();
        private List<OrdenKPIDTO> _ordenesCerradas = new List<OrdenKPIDTO>();
        private List<OrdenKPIDTO> _ordenesAbiertas = new List<OrdenKPIDTO>();
        private List<OrdenKPIDTO> _ordenesCerradasPorEquipo = new List<OrdenKPIDTO>();
        private List<OrdenKPIDTO> _ordenesAbiertasPorEquipo = new List<OrdenKPIDTO>();
        private List<OrdenKPIDTO> _ordenesCombinadasPorEquipo = new List<OrdenKPIDTO>();
        private List<tblM_CapHorometro> _horometrosMes = new List<tblM_CapHorometro>();
        private List<tblM_CapHorometro> _horometrosMesPorEquipo = new List<tblM_CapHorometro>();
        private List<int> _programado = new List<int>();
        private List<int> _noProgramado = new List<int>();
        private List<int> _preventivo = new List<int>();
        private List<int> _correctivo = new List<int>();
        private List<int> _predictivo = new List<int>();


        private List<kpiMTTOyParoDTO> getMTTOyParo(MaquinaKPIGeneralDTO maquina, List<string> cc, DateTime fechaInicio, DateTime fechaFin)
        {
            var ordenes = _ordenesCombinadasPorEquipo.OrderBy(x => x.horometro).ThenBy(x => x.FechaCreacion).ToList();

            var idOTL = ordenes.Select(x => x.id).ToList();

            var ordenesDetalle = _context.tblM_DetOrdenTrabajo
                .Where(x =>
                    idOTL.Contains(x.OrdenTrabajoID)).ToList();

            var horometrosMes = _horometrosMesPorEquipo.OrderBy(x => x.Fecha).ToList();

            if (horometrosMes.Count == 0)
            {
                horometrosMes = _context.tblM_CapHorometro
                    .Where(x =>
                        x.Economico.Equals(maquina.noEconomico))
                    .OrderByDescending(x => x.id).Take(1).ToList();
            }

            var result = new List<kpiMTTOyParoDTO>();
            var horometrosParcial = 0M;
            for (int i = 0; i < ordenes.Count; i++)
            {
                var horaSalida = ordenes[i].TipoOT;
                var oID = ordenes[i].id;
                var oMotivioParoID = ordenes[i].MotivoParo;

                var det = ordenesDetalle.Where(x => x.OrdenTrabajoID == oID).OrderBy(x => x.HoraInicio).ToList();

                if (det.Count > 0)
                {
                    var CausasParo = _context.tblM_CatCriteriosCausaParo.FirstOrDefault(x => x.id == oMotivioParoID);
                    var o = new kpiMTTOyParoDTO();
                    o.fecha = ordenes[i].FechaEntrada.ToShortDateString();
                    o.horometro = ordenes[i].horometro.ToString();
                    o.aplicaCalculo = ordenes[i].TipoParo3;

                    var falla = "";
                    var programado = "";
                    var mantenimiento = "";

                    if (CausasParo != null)
                    {
                        falla = CausasParo.CausaParo;
                        programado = CausasParo.TipoParo;
                        mantenimiento = CausasParo.TiempoMantenimiento;
                        var mm = mantenimiento.Substring(0, 1)[0].ToString();
                        mantenimiento = mm;
                    }

                    o.falla = falla;
                    o.programado = programado;
                    o.tipo = mantenimiento;

                    var horasUtilH = 0M;
                    var horasTiempoMuerto = 0M;

                    if (ordenes[i].TipoOT == 1)
                    {
                        var fInicioMes = new DateTime();
                        if (ordenes[i].FechaEntrada >= fechaInicio)
                        {
                            fInicioMes = ordenes[i].FechaEntrada;
                        }
                        else
                        {
                            fInicioMes = fechaInicio;
                        }

                        if (ordenes[i].EstatusOT)
                        {
                            var FinDeMes = fechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);
                            var horas = (decimal)(FinDeMes - fInicioMes).TotalHours;

                            horasUtilH += horas;
                        }
                        else
                        {
                            var FinDeMes = fechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);
                            var horas = (FinDeMes - fInicioMes).TotalHours;
                            horasUtilH += (decimal)horas;
                        }
                    }
                    else
                    {
                        var fInicioMes = new DateTime();
                        fInicioMes = ordenes[i].FechaEntrada;

                        if (ordenes[i].FechaEntrada >= fechaInicio)
                        {
                            fInicioMes = ordenes[i].FechaEntrada;
                        }
                        else
                        {
                            fInicioMes = fechaInicio;
                        }

                        var finDeMes = new DateTime();
                        finDeMes = ordenes[i].FechaSalida.Value;

                        var horas = (decimal)(finDeMes - fInicioMes).TotalHours;
                        horasUtilH += horas;
                    }

                    o.tiempoUtil = horasUtilH;
                    o.tiempoMuerto = horasTiempoMuerto;
                    if (o.aplicaCalculo == 2)
                    {
                        if (i == 0)
                        {
                            var horometroInicial = 0M;
                            try
                            {
                                horometroInicial = horometrosMes.First().Horometro - horometrosMes.First().HorasTrabajo;
                            }
                            catch (Exception ex)
                            {
                                horometroInicial = 0;
                            }
                            o.MTBS = ordenes[i].horometro - horometroInicial;
                        }
                        else
                        {
                            if (horometrosParcial == 0)
                            {
                                var horometroInicial = 0M;
                                try
                                {
                                    horometroInicial = horometrosMes.First().Horometro - horometrosMes.First().HorasTrabajo;
                                }
                                catch (Exception ex)
                                {
                                    horometroInicial = 0;
                                }
                                o.MTBS = ordenes[i].horometro - horometroInicial;
                            }
                            else
                            {
                                o.MTBS = ordenes[i].horometro - horometrosParcial;
                            }
                        }

                        if (o.aplicaCalculo == 2)
                        {
                            horometrosParcial = ordenes[i].horometro;
                        }
                    }
                    else
                    {
                        o.MTBS = 0;
                    }

                    o.tipoEnParo = EnumExtensions.GetDescription((TipoParo3OTEnum)o.aplicaCalculo);
                    o.MTTR = o.tiempoUtil - o.tiempoMuerto;
                    o.personal = det.Count;
                    o.horasHombre = det.Sum(x => SumData(x.HoraFin, x.HoraInicio));
                    result.Add(o);
                }
            }

            return result;
        }

        private kpiInfoGeneralDTO getInfoGeneralPorEconomico(MaquinaKPIGeneralDTO maquina, List<string> cc, DateTime fechaInicio, DateTime fechaFin)
        {
            _ordenesCombinadasPorEquipo = new List<OrdenKPIDTO>();
            _ordenesCombinadasPorEquipo.AddRange(_ordenesCerradasPorEquipo);
            _ordenesCombinadasPorEquipo.AddRange(_ordenesAbiertasPorEquipo);

            var horometrosMes = _horometrosMesPorEquipo.Where(x => cc.Contains(x.CC)).ToList();

            if (horometrosMes.Count == 0)
            {
                horometrosMes = _context.tblM_CapHorometro
                    .Where(x =>
                        x.Economico.Equals(maquina.noEconomico) &&
                        cc.Contains(x.CC))
                    .OrderByDescending(x => x.id).Take(1).ToList();
            }

            var detResult = getMTTOyParo(maquina, cc, fechaInicio, fechaFin);

            var result = new kpiInfoGeneralDTO();
            result.noEconomico = maquina.noEconomico;
            if (_ordenesCombinadasPorEquipo.Count == 0)
            {
                result.horometroInicial = 0;
                result.horometroFinal = 0;
            }
            else
            {
                var horometroInicial = 0M;
                try
                {
                    horometroInicial = horometrosMes.First().HorometroAcumulado - _horometrosMesPorEquipo.First().HorasTrabajo;
                }
                catch (Exception ex)
                {
                    horometroInicial = 0;
                }

                result.horometroInicial = horometroInicial;
                var horometroFinal = 0M;
                try
                {
                    horometroFinal = horometrosMes.Last().HorometroAcumulado;
                }
                catch (Exception ex)
                {
                    horometroFinal = 0;
                }
                result.horometroFinal = horometroFinal;
            }

            var ultimoHorometro = 0M;
            try
            {
                ultimoHorometro = Convert.ToDecimal(detResult.Where(x => x.aplicaCalculo == 2).OrderByDescending(x => x.horometro).First().horometro);
            }
            catch (Exception ex)
            {
                ultimoHorometro = 0M;
            }

            var countCalculoMTTR = detResult.Where(x => x.MTBS != 0 && x.aplicaCalculo == 2).Count();

            if (ultimoHorometro != 0)
            {
                var horometroInicialFinal = horometrosMes.LastOrDefault();
                if (horometroInicialFinal != null)
                {
                    if ((horometroInicialFinal.HorometroAcumulado - ultimoHorometro) != 0)
                    {
                        detResult.Add(new kpiMTTOyParoDTO
                        {
                            aplicaCalculo = 2,
                            MTBS = horometroInicialFinal.HorometroAcumulado - ultimoHorometro
                        });
                    }
                }
            }

            var aplicaCalculo = detResult.Where(x => x.aplicaCalculo == 2).Count();

            var countCalculo = detResult.Where(x => x.MTBS != 0 && x.aplicaCalculo == 2).Count();
            var divisormtbs = detResult.Where(x => x.aplicaCalculo == 2).Sum(x => x.MTBS);
            var divisormttr = countCalculo != 1 ? detResult.Where(x => x.MTBS != 0 && x.aplicaCalculo == 2).Sum(x => x.MTTR) : detResult.Where(x => x.aplicaCalculo == 2).Sum(x => x.MTTR);

            result.MTBS = aplicaCalculo == 0 ? 0 : divisormtbs / (countCalculo == 0 ? 1 : countCalculo);
            result.MTTR = aplicaCalculo == 0 ? 0 : divisormttr / (countCalculoMTTR == 0 ? 1 : countCalculoMTTR);

            result.disponibilidad = (result.MTBS + result.MTTR) == 0 ? ("100%") : (Math.Round(((result.MTBS / (result.MTBS + result.MTTR)) * 100), 2) + "%");

            result.horasHombre = detResult.Sum(x => x.horasHombre);
            var horasParo = 0M;
            foreach (var i in _ordenesCombinadasPorEquipo)
            {
                if (i.TipoOT != 1)
                {
                    var fEntrada = i.FechaEntrada;
                    var fSalida = i.FechaSalida;
                    var intervalo = (i.FechaSalida.Value - i.FechaEntrada);
                    var horas = (i.FechaSalida.Value - i.FechaEntrada).TotalHours;
                    horasParo += (decimal)horas;
                }
                else
                {
                    var fEntrada = new DateTime();
                    var fsalida = new DateTime();

                    if (i.FechaCreacion >= fechaInicio && i.FechaSalida <= fechaFin)
                    {
                        fEntrada = i.FechaEntrada;
                    }
                    else
                    {
                        fEntrada = fechaInicio;
                    }

                    if (i.EstatusOT)
                    {
                        fsalida = fechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);

                        var horas = (fsalida - fEntrada).TotalHours;
                        horasParo += (decimal)horas;
                    }
                    else
                    {
                        fsalida = i.FechaSalida.Value;
                        var horas = (fsalida - fEntrada).TotalHours;
                        horasParo += (decimal)horas;
                    }
                }
            }

            result.horasParo = detResult.Where(x => x.MTBS != 0).Sum(x => x.tiempoUtil);
            result.horasTrabajadas = horometrosMes.Sum(x => x.HorasTrabajo);
            result.ratioMantenimiento = result.horasTrabajadas == 0 ? 0 : (result.horasHombre / result.horasTrabajadas);

            return result;
        }

        private kpiFrecuenciaParosDTO getTop3FrecuenciaParo(MaquinaKPIGeneralDTO maquina, List<string> cc, DateTime fechaInicio, DateTime fechaFin)
        {

            var motivosUnicos = _ordenes
                .Where(x =>
                    x.EconomicoID == maquina.id &&
                    x.FechaSalida != null)
                .Select(x => x.MotivoParo).ToList();

            var listaInicio = new List<stTop3Paros>();
            foreach (var i in motivosUnicos.Distinct())
            {
                var o = new stTop3Paros();
                if (i != 0)
                {
                    o.tipo = _context.tblM_CatCriteriosCausaParo.FirstOrDefault(x => x.id == i).CausaParo;
                    o.cantidad = motivosUnicos.Where(x => x == i).Count();
                    listaInicio.Add(o);
                }
            }

            var listaFin = new List<stTop3Paros>();
            listaFin.AddRange(listaInicio.OrderByDescending(x => x.cantidad));
            var result = new kpiFrecuenciaParosDTO();

            var cantidadListaFin = listaFin.Count;

            result.paro1 = cantidadListaFin >= 1 ? listaFin[0].tipo : "";
            result.paro2 = cantidadListaFin >= 2 ? listaFin[1].tipo : "";
            result.paro3 = cantidadListaFin >= 3 ? listaFin[2].tipo : "";

            return result;
        }

        private kpiTipoMantenimientoDTO getMDTipoMantenimiento(MaquinaKPIGeneralDTO maquina, List<string> cc, DateTime fechaInicio, DateTime fechaFin)
        {
            var ordenesMaquina = _ordenes.Where(x => x.EconomicoID == maquina.id).ToList();
            
            var ordenesProgramadas = ordenesMaquina.Where(x => _programado.Contains(x.MotivoParo) && x.FechaSalida != null).ToList();
            var ordenesNoProgramadas = ordenesMaquina.Where(x => _noProgramado.Contains(x.MotivoParo) && x.FechaSalida != null).ToList();
            var ordenesPreventidas = ordenesMaquina.Where(x => _preventivo.Contains(x.MotivoParo) && x.FechaSalida != null).ToList();
            var ordenesCorrectivas = ordenesMaquina.Where(x => _correctivo.Contains(x.MotivoParo) && x.FechaSalida != null).ToList();
            var ordenesPred = ordenesMaquina.Where(x => _predictivo.Contains(x.MotivoParo) && x.FechaSalida != null).ToList();

            var obj = new kpiTipoMantenimientoDTO();
            obj.tdTPTiempo = ordenesProgramadas.Sum(x => SumData(x.FechaSalida, x.FechaEntrada));
            obj.tdTPCantidad = ordenesProgramadas.Count;

            obj.tdTNPTiempo = ordenesNoProgramadas.Sum(x => SumData(x.FechaSalida, x.FechaEntrada));
            obj.tdTNPCantidad = ordenesNoProgramadas.Count;

            obj.tdPTiempo = ordenesPreventidas.Sum(x => SumData(x.FechaSalida, x.FechaEntrada));
            obj.tdPCantidad = ordenesPreventidas.Count;

            obj.tdCTiempo = ordenesCorrectivas.Sum(x => SumData(x.FechaSalida, x.FechaEntrada));
            obj.tdCCantidad = ordenesCorrectivas.Count;

            obj.tdPrTiempo = ordenesPred.Sum(x => SumData(x.FechaSalida, x.FechaEntrada));
            obj.tdPrCantidad = ordenesPred.Count;

            obj.tdTPTotal = obj.tdTPTiempo + obj.tdTNPTiempo;
            obj.tdTPTotal2 = obj.tdTPCantidad + obj.tdTNPCantidad;
            obj.tdPTotal = obj.tdPTiempo + obj.tdCTiempo + obj.tdPrTiempo;
            obj.tdPTotal2 = obj.tdPCantidad + obj.tdCCantidad + obj.tdPrCantidad;

            obj.tdTPPTiempo = obj.tdTPTotal == 0 ? 0 : ((obj.tdTPTiempo / obj.tdTPTotal) * 100);
            obj.tdTPPCantidad = obj.tdTPTotal2 == 0 ? 0 : ((obj.tdTPCantidad / obj.tdTPTotal2) * 100);
            obj.tdTNPPTiempo = obj.tdTPTotal == 0 ? 0 : ((obj.tdTNPTiempo / obj.tdTPTotal) * 100);
            obj.tdTNPPCantidad = obj.tdTPTotal2 == 0 ? 0 : ((obj.tdTNPCantidad / obj.tdTPTotal2) * 100);

            obj.tdPPTiempo = obj.tdPTotal == 0 ? 0 : ((obj.tdPTiempo / obj.tdPTotal) * 100);
            obj.tdPPCantidad = obj.tdPTotal2 == 0 ? 0 : ((obj.tdPCantidad / obj.tdPTotal2) * 100);
            obj.tdCPTiempo = obj.tdPTotal == 0 ? 0 : ((obj.tdCTiempo / obj.tdPTotal) * 100);
            obj.tdCPCantidad = obj.tdPTotal2 == 0 ? 0 : ((obj.tdCCantidad / obj.tdPTotal2) * 100);
            obj.tdPrPTiempo = obj.tdPTotal == 0 ? 0 : ((obj.tdPrTiempo / obj.tdPTotal) * 100);
            obj.tdPrPCantidad = obj.tdPTotal2 == 0 ? 0 : ((obj.tdPrCantidad / obj.tdPTotal2) * 100);
            obj.tdPTotalF = (obj.tdTPCantidad + obj.tdTNPCantidad) == 0 ? 0 : ((obj.tdTPCantidad / (obj.tdTPCantidad + obj.tdTNPCantidad)) * 100);
            return obj;
        }

        public IList<KPIDTO> getKPIGeneral2(List<string> cc, int tipo, int modelo, DateTime fechaInicio, DateTime fechaFin)
        {
            var lista = new List<KPIDTO>();

            _maquinas = _context.tblM_CatMaquina
                .Where(x =>
                    x.grupoMaquinaria.tipoEquipoID == 1 &&
                    x.estatus != 0 &&
                    (tipo != 0 ? x.grupoMaquinaria.id == tipo : true) &&
                    (modelo != 0 ? x.modeloEquipoID == modelo : true))
                .Select(x => new MaquinaKPIGeneralDTO
                {
                    id = x.id,
                    noEconomico = x.noEconomico,
                    descripcionGrupoMaquina = x.grupoMaquinaria.descripcion
                }).ToList();

            var idsMaquinas = _maquinas.Select(x => x.id).ToList();
            if ((int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Peru)
            {
                List<string> lstCC = new List<string>();
                lstCC.AddRange(cc);
                cc = new List<string>();
                foreach (var item in lstCC)
                {
                    tblP_CC objCC = _context.tblP_CC.Where(w => w.cc == item).FirstOrDefault();
                    if (objCC != null)
                    {
                        string areaCuenta = objCC.areaCuenta;
                        cc.Add(areaCuenta);
                    }
                }
            }

            _ordenes = _context.tblM_CapOrdenTrabajo
                .Where(x =>
                    idsMaquinas.Contains(x.EconomicoID) &&
                    cc.Contains(x.CC) &&
                    x.FechaEntrada >= fechaInicio &&
                    x.FechaEntrada <= fechaFin)
                .Select(x => new OrdenKPIDTO
                {
                    id = x.id,
                    EconomicoID = x.EconomicoID,
                    horometro = x.horometro,
                    FechaCreacion = x.FechaCreacion,
                    FechaSalida = x.FechaSalida,
                    MotivoParo = x.MotivoParo,
                    TipoOT = x.TipoOT,
                    EstatusOT = x.EstatusOT,
                    FechaEntrada = x.FechaEntrada,
                    TipoParo3 = x.TipoParo3
                }).ToList();

            var idsMaquinasOrdenes = _ordenes.Select(x => x.EconomicoID).ToList();

            _ordenesAbiertas = _ordenes
                .Where(x =>
                    x.TipoOT == 1 &&
                    x.EstatusOT == true).ToList();
            _ordenesCerradas = _ordenes
                .Where(x =>
                    x.FechaSalida != null &&
                    x.TipoOT == 0).ToList();

            _maquinas.RemoveAll(x => !idsMaquinasOrdenes.Contains(x.id));

            idsMaquinasOrdenes = _maquinas.Select(x => x.id).ToList();

            var maquinasNoOC = _context.tblM_CatMaquina
                .Where(x =>
                    cc.Contains(x.centro_costos) &&
                    x.grupoMaquinaria.tipoEquipoID == 1 &&
                    x.estatus != 0 &&
                    (tipo != 0 ? x.grupoMaquinaria.id == tipo : true) &&
                    (modelo != 0 ? x.modeloEquipoID == modelo : true) &&
                    (!idsMaquinasOrdenes.Contains(x.id)) &&
                    (fechaFin.Month == DateTime.Now.Month ? true : false))
                .Select(x => new MaquinaKPIGeneralDTO
                {
                    id = x.id,
                    noEconomico = x.noEconomico,
                    descripcionGrupoMaquina = x.grupoMaquinaria.descripcion
                }).ToList();

            _maquinas.AddRange(maquinasNoOC);

            idsMaquinasOrdenes.AddRange(maquinasNoOC.Select(x => x.id));

            var economicos = _maquinas.Select(x => x.noEconomico).ToList();

            var horometros = _context.tblM_CapHorometro
                .Where(x =>
                    !economicos.Contains(x.Economico) &&
                    cc.Contains(x.CC) &&
                    x.Fecha >= fechaInicio &&
                    x.Fecha <= fechaFin &&
                    (fechaFin.Month < DateTime.Now.Month ? true : false)).ToList();

            economicos.AddRange(horometros.Select(x => x.Economico).ToList());
            var economicosHorometros = horometros.Select(x => x.Economico).ToList();

            _maquinas.AddRange(_context.tblM_CatMaquina
                .Where(x =>
                    economicosHorometros.Contains(x.noEconomico) &&
                    x.grupoMaquinaria.tipoEquipoID == 1 &&
                    (tipo != 0 ? x.grupoMaquinaria.id == tipo : true) &&
                    (modelo != 0 ? x.modeloEquipoID == modelo : true))
                .Select(x => new MaquinaKPIGeneralDTO
                {
                    id = x.id,
                    noEconomico = x.noEconomico,
                    descripcionGrupoMaquina = x.grupoMaquinaria.descripcion
                }));

            _horometrosMes = _context.tblM_CapHorometro
                .Where(x =>
                    economicos.Contains(x.Economico) &&
                    //cc.Contains(x.CC) &&
                    x.Fecha >= fechaInicio &&
                    x.Fecha <= fechaFin).OrderBy(x => x.Fecha).ToList();

            var criterios = _context.tblM_CatCriteriosCausaParo.ToList();
            _programado = criterios.Where(x => x.TipoParo.Equals("Programado")).Select(x => x.id).ToList();
            _noProgramado = criterios.Where(x => x.TipoParo.Equals("No Programado")).Select(x => x.id).ToList();
            _preventivo = criterios.Where(x => x.TiempoMantenimiento.Equals("Preventivo")).Select(x => x.id).ToList();
            _correctivo = criterios.Where(x => x.TiempoMantenimiento.Equals("Correctivo")).Select(x => x.id).ToList();
            _predictivo = criterios.Where(x => x.TiempoMantenimiento.Equals("Predictivo")).Select(x => x.id).ToList();

            foreach (var maquina in _maquinas)
            {
                _ordenesCerradasPorEquipo = _ordenesCerradas.Where(x => x.EconomicoID == maquina.id).OrderBy(x => x.FechaEntrada).ToList();
                _ordenesAbiertasPorEquipo = _ordenesAbiertas.Where(x => x.EconomicoID == maquina.id).ToList();

                _horometrosMesPorEquipo = _horometrosMes.Where(x => x.Economico.Equals(maquina.noEconomico)).OrderBy(x => x.Fecha).ToList();
                if (_horometrosMesPorEquipo.Count == 0)
                {
                    _horometrosMesPorEquipo = _context.tblM_CapHorometro
                        .Where(x =>
                            x.Economico.Equals(maquina.noEconomico) &&
                            cc.Contains(x.CC)).OrderByDescending(x => x.id).Take(1).ToList();
                }

                var kpi = new KPIDTO();

                var infoGeneral = getInfoGeneralPorEconomico(maquina, cc, fechaInicio, fechaFin);
                var top3Paros = getTop3FrecuenciaParo(maquina, cc, fechaInicio, fechaFin);
                var tipoMantenimiento = getMDTipoMantenimiento(maquina, cc, fechaInicio, fechaFin);
                var horasTrabajadas = Math.Round(infoGeneral.horasTrabajadas, 2);
                var horasParo = Math.Round(tipoMantenimiento.tdPTiempo, 2) + Math.Round(tipoMantenimiento.tdCTiempo, 2) + Math.Round(tipoMantenimiento.tdPrTiempo, 2);

                int diasPeriodo = Convert.ToInt32((fechaFin - fechaInicio).TotalDays) + 1;

                kpi.id = maquina.id;
                kpi.btnEquipo = "<button class='btn btn-primary' onclick='getKPIEquipo(" + maquina.id + ",\"" + maquina.noEconomico + "\")' title='Inf. General'><i class='glyphicon glyphicon-eye-open'></<i></button>";
                kpi.economico = maquina.noEconomico;
                //kpi.horasIdealMensual = "";
                kpi.pDisponibilidad = infoGeneral.disponibilidad.Trim('%') + "%"; // infoGeneral.horasParo == 0 ? "100%" : (Math.Round(((infoGeneral.MTBS + infoGeneral.MTTR) == 0 ? 0 : (infoGeneral.MTBS / (infoGeneral.MTBS + infoGeneral.MTTR))) * 100, 2).ToString() + "%");
                kpi.horasTrabajado = Math.Round(infoGeneral.horasTrabajadas, 2).ToString();
                kpi.horasParo = Math.Round(horasParo, 2).ToString();//Math.Round(infoGeneral.horasParo, 2).ToString();
                kpi.pMProgramadoTiempo = Math.Round(tipoMantenimiento.tdTPPTiempo, 2) + "%";
                kpi.pMProgramadoCantidad = Math.Round(tipoMantenimiento.tdTPPCantidad, 2) + "%";
                kpi.pPreventivoHoras = Math.Round(tipoMantenimiento.tdPTiempo, 2).ToString();
                kpi.pCorrectivoHoras = Math.Round(tipoMantenimiento.tdCTiempo, 2).ToString();
                kpi.pPredictivoHoras = Math.Round(tipoMantenimiento.tdPrTiempo, 2).ToString();
                kpi.horasHombre = Math.Round(infoGeneral.horasHombre, 2).ToString();
                kpi.MTBS = Math.Round(infoGeneral.MTBS, 2).ToString();
                kpi.MTTR = Math.Round(infoGeneral.MTTR, 2).ToString();
                kpi.pUtilizacion = (Math.Round((horasTrabajadas / ((24 * diasPeriodo) - horasParo)) * 100, 2)) + "%";
                kpi.parosPrincipal1 = top3Paros.paro1;
                kpi.parosPrincipal2 = top3Paros.paro2;
                kpi.parosPrincipal3 = top3Paros.paro3;
                lista.Add(kpi);
            }

            return lista;
        }

        public kpiRepGraficas getKPIRepGraficas2(List<string> cc, int tipo, int modelo, DateTime fechaInicio, DateTime fechaFin, List<KPIDTO> data = null)
        {
            var gfx = new kpiRepGraficas();
            gfx.GraficaFamiliasDTO = new List<kpiGraficaFamiliasDTO>();
            gfx.MotivosParoDTO = new List<kpiMotivosParoDTO>();

            var maquinas = _context.tblM_CatMaquina
                .Where(x =>
                    cc.Contains(x.centro_costos) &&
                    (tipo != 0 ? x.grupoMaquinaria.id == tipo : x.grupoMaquinaria.tipoEquipoID == 1))
                .Select(x => new MaquinaKPIGeneralDTO
                {
                    id = x.id,
                    noEconomico = x.noEconomico,
                    descripcionGrupoMaquina = x.grupoMaquinaria.descripcion
                }).ToList();
            var maquinasIds = maquinas.Select(x => x.id).ToList();
            var economicos = maquinas.Select(x => x.noEconomico).ToList();
            var grupos = maquinas.Select(x => x.descripcionGrupoMaquina).Distinct().ToList();
            var motivos = _context.tblM_CatCriteriosCausaParo.Select(x => new { x.id, x.TiempoMantenimiento }).ToList();
            var motivosTiempoMantenimiento = motivos.Select(x => x.TiempoMantenimiento).Distinct().ToList();
            var motivosIds = motivos.Select(x => x.id).ToList();

            _ordenes = _context.tblM_CapOrdenTrabajo
                .Where(x =>
                    maquinasIds.Contains(x.EconomicoID) &&
                    cc.Contains(x.CC) &&
                    x.FechaEntrada >= fechaInicio &&
                    x.FechaEntrada <= fechaFin)
                .Select(x => new OrdenKPIDTO
                {
                    id = x.id,
                    EconomicoID = x.EconomicoID,
                    horometro = x.horometro,
                    FechaCreacion = x.FechaCreacion,
                    FechaSalida = x.FechaSalida,
                    MotivoParo = x.MotivoParo,
                    TipoOT = x.TipoOT,
                    EstatusOT = x.EstatusOT,
                    FechaEntrada = x.FechaEntrada
                }).ToList();
            _ordenesAbiertas = _ordenes
                .Where(x =>
                    x.TipoOT == 1 &&
                    x.EstatusOT == true).ToList();
            _ordenesCerradas = _ordenes
                .Where(x =>
                    x.FechaSalida != null &&
                    x.TipoOT == 0).ToList();

            _horometrosMes = _context.tblM_CapHorometro
                .Where(x =>
                    economicos.Contains(x.Economico) &&
                    x.Fecha >= fechaInicio &&
                    x.Fecha <= fechaFin)
                .OrderBy(x => x.Fecha).ToList();

            //foreach (var grupo in maquinas.GroupBy(x => x.descripcionGrupoMaquina))
            //{
            //    var infoGpx = new kpiGraficaFamiliasDTO();
            //    infoGpx.Concepto = grupo.Key;

            //    decimal disponibilidad = 0;
            //    foreach (var maquina in grupo)
            //    {
            //        _ordenesCerradasPorEquipo = _ordenesCerradas.Where(x => x.EconomicoID == maquina.id).OrderBy(x => x.FechaEntrada).ToList();
            //        _ordenesAbiertasPorEquipo = _ordenesAbiertas.Where(x => x.EconomicoID == maquina.id).ToList();

            //        _horometrosMesPorEquipo = _horometrosMes.Where(x => x.Economico.Equals(maquina.noEconomico)).OrderBy(x => x.Fecha).ToList();
            //        var infoGeneral = getInfoGeneralPorEconomico(maquina, cc, fechaInicio, fechaFin);
            //        disponibilidad += infoGeneral.horasParo == 0 ? 100 : (Math.Round(((infoGeneral.MTBS + infoGeneral.MTTR) == 0 ? 0 : (infoGeneral.MTBS / (infoGeneral.MTBS + infoGeneral.MTTR))) * 100, 2));
            //    }

            //    //var valor = (disponibilidad * 100) / (grupo.Count() * 100);
            //    var valor = (disponibilidad) / (grupo.Count());
            //    infoGpx.Valor = Convert.ToInt32(Math.Round(valor, 0));
            //    gfx.GraficaFamiliasDTO.Add(infoGpx);
            //}

            List<KPIDTO> lstEconomicosOrderByGrupoMaquinaria = new List<KPIDTO>();
            foreach (var item in data)
            {
                tblM_CatMaquina objMaquina = _context.tblM_CatMaquina.Where(w => w.noEconomico == item.economico).FirstOrDefault();
                if (objMaquina != null)
                {
                    KPIDTO obj = new KPIDTO();
                    obj.nombreGrupoMaquinaria = objMaquina.grupoMaquinaria.descripcion.Trim().ToUpper();
                    obj.promedioDisponibilidad = Convert.ToDecimal(item.pDisponibilidad.Replace("%", ""));
                    lstEconomicosOrderByGrupoMaquinaria.Add(obj);
                }
            }

            List<KPIDTO> lstEconomicosAgrupados = new List<KPIDTO>();
            foreach (var item in lstEconomicosOrderByGrupoMaquinaria)
            {
                KPIDTO obj = lstEconomicosAgrupados.Where(w => w.nombreGrupoMaquinaria == item.nombreGrupoMaquinaria).FirstOrDefault();
                if (obj == null)
                {
                    KPIDTO objDTO = new KPIDTO();
                    objDTO.nombreGrupoMaquinaria = item.nombreGrupoMaquinaria;
                    objDTO.promedioDisponibilidad = item.promedioDisponibilidad;
                    lstEconomicosAgrupados.Add(objDTO);
                }
                else
                {
                    obj.promedioDisponibilidad += item.promedioDisponibilidad;
                }
            }

            foreach (var item in lstEconomicosAgrupados)
            {
                var infoGpx = new kpiGraficaFamiliasDTO();
                infoGpx.Concepto = item.nombreGrupoMaquinaria;

                decimal promedioGrupoMaquinaria = item.promedioDisponibilidad;
                decimal cantRegistrosRelGrupoMaquinaria = lstEconomicosOrderByGrupoMaquinaria.Where(w => w.nombreGrupoMaquinaria == item.nombreGrupoMaquinaria).Count();
                decimal promedioGeneral = 0;
                if ((decimal)promedioGrupoMaquinaria > 0 && (decimal)cantRegistrosRelGrupoMaquinaria > 0)
                {
                    promedioGeneral = (decimal)promedioGrupoMaquinaria / (decimal)cantRegistrosRelGrupoMaquinaria;
                }

                int entero = Convert.ToInt32(promedioGeneral);
                infoGpx.Valor = entero;
                gfx.GraficaFamiliasDTO.Add(infoGpx);
            }

            if (gfx.GraficaFamiliasDTO.Count == 0)
            {
                var infoGpx = new kpiGraficaFamiliasDTO();
                infoGpx.Concepto = "";
                infoGpx.Valor = 0;
                gfx.GraficaFamiliasDTO.Add(infoGpx);
            }

            var ordenes = _context.tblM_CapOrdenTrabajo
                .Where(x =>
                    maquinasIds.Contains(x.EconomicoID) &&
                    motivosIds.Contains(x.MotivoParo) &&
                    cc.Contains(x.CC) &&
                    x.FechaEntrada >= fechaInicio &&
                    x.FechaEntrada <= fechaFin &&
                    x.FechaSalida != null).OrderBy(x => x.FechaEntrada).ToList();

            foreach (var motivo in motivosTiempoMantenimiento)
            {
                var infoGfx = new kpiMotivosParoDTO();
                infoGfx.codigo = motivo;
                var ids = motivos.Where(x => x.TiempoMantenimiento.Equals(motivo)).Select(x => x.id).ToList();
                infoGfx.cantidad = ordenes.Where(x => ids.Contains(x.MotivoParo)).Count();
                gfx.MotivosParoDTO.Add(infoGfx);
            }

            return gfx;
        }

        public kpiRepMetricasDTO getKPIRepMetricasDTO2(List<string> cc, int tipo, int modelo, DateTime fechaInicio, DateTime fechafin)
        {
            switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
            {
                case EmpresaEnum.Peru:
                    {
                        var ccs = _context.tblP_CC.Where(x => cc.Contains(x.cc)).ToList();
                        cc = ccs.Select(x => x.areaCuenta).ToList();
                    }
                    break;
            }

            #region Data
            var infoMeses = new List<List<KPINoFormatDTO>>();
            foreach (var mes in _meses)
            {
                infoMeses.Add(new List<KPINoFormatDTO>());
            }

            var criterios = _context.tblM_CatCriteriosCausaParo.ToList();
            _programado = criterios.Where(x => x.TipoParo.Equals("Programado")).Select(x => x.id).ToList();
            _noProgramado = criterios.Where(x => x.TipoParo.Equals("No Programado")).Select(x => x.id).ToList();
            _preventivo = criterios.Where(x => x.TiempoMantenimiento.Equals("Preventivo")).Select(x => x.id).ToList();
            _correctivo = criterios.Where(x => x.TiempoMantenimiento.Equals("Correctivo")).Select(x => x.id).ToList();
            _predictivo = criterios.Where(x => x.TiempoMantenimiento.Equals("Predictivo")).Select(x => x.id).ToList();

            _maquinas = _context.tblM_CatMaquina
                .Where(x =>
                    x.grupoMaquinaria.tipoEquipoID == 1 &&
                    x.estatus != 0 &&
                    (tipo != 0 ? x.grupoMaquinaria.id == tipo : true) &&
                    (modelo != 0 ? x.modeloEquipoID == modelo : true))
                .Select(x => new MaquinaKPIGeneralDTO
                {
                    id = x.id,
                    noEconomico = x.noEconomico,
                    descripcionGrupoMaquina = x.grupoMaquinaria.descripcion
                }).ToList();

            var maquinasTemporales = new List<MaquinaKPIGeneralDTO>();

            foreach (var item in _maquinas)
            {
                var m = new MaquinaKPIGeneralDTO();
                m.id = item.id;
                m.descripcionGrupoMaquina = item.descripcionGrupoMaquina;
                m.noEconomico = item.noEconomico;
                maquinasTemporales.Add(m);
            }

            var idsMaquinas = maquinasTemporales.Select(x => x.id).ToList();

            for (int mes = 0; mes < (fechaInicio.Year < DateTime.Now.Year ? _meses.Count : fechaInicio.Month); mes++)
            {
                _maquinas = new List<MaquinaKPIGeneralDTO>();
                foreach (var item in maquinasTemporales)
                {
                    var m = new MaquinaKPIGeneralDTO();
                    m.id = item.id;
                    m.descripcionGrupoMaquina = item.descripcionGrupoMaquina;
                    m.noEconomico = item.noEconomico;
                    _maquinas.Add(m);
                }

                var lista = new List<KPINoFormatDTO>();

                var fechaInicialMensual = new DateTime(fechaInicio.Year, mes + 1, 1);
                var fechaFinalMensual = new DateTime(fechaInicio.Year, mes + 1, DateTime.DaysInMonth(fechaInicio.Year, mes + 1));

                _ordenes = _context.tblM_CapOrdenTrabajo
                    .Where(x =>
                        idsMaquinas.Contains(x.EconomicoID) &&
                        cc.Contains(x.CC) &&
                        x.FechaEntrada >= fechaInicialMensual &&
                        x.FechaEntrada <= fechaFinalMensual)
                    .Select(x => new OrdenKPIDTO
                    {
                        id = x.id,
                        EconomicoID = x.EconomicoID,
                        horometro = x.horometro,
                        FechaCreacion = x.FechaCreacion,
                        FechaSalida = x.FechaSalida,
                        MotivoParo = x.MotivoParo,
                        TipoOT = x.TipoOT,
                        EstatusOT = x.EstatusOT,
                        FechaEntrada = x.FechaEntrada,
                        TipoParo3 = x.TipoParo3
                    }).ToList();

                var idsMaquinasOrdenes = _ordenes.Select(x => x.EconomicoID).ToList();

                _ordenesAbiertas = _ordenes
                    .Where(x =>
                        x.TipoOT == 1 &&
                        x.EstatusOT == true).ToList();
                _ordenesCerradas = _ordenes
                    .Where(x =>
                        x.FechaSalida != null &&
                        x.TipoOT == 0).ToList();

                _maquinas.RemoveAll(x => !idsMaquinasOrdenes.Contains(x.id));

                idsMaquinasOrdenes = _maquinas.Select(x => x.id).ToList();

                var maquinasNoOC = _context.tblM_CatMaquina
                    .Where(x =>
                        cc.Contains(x.centro_costos) &&
                        x.grupoMaquinaria.tipoEquipoID == 1 &&
                        x.estatus != 0 &&
                        (tipo != 0 ? x.grupoMaquinaria.id == tipo : true) &&
                        (modelo != 0 ? x.modeloEquipoID == modelo : true) &&
                        (!idsMaquinasOrdenes.Contains(x.id)) &&
                        (fechaFinalMensual.Month == DateTime.Now.Month ? true : false))
                    .Select(x => new MaquinaKPIGeneralDTO
                    {
                        id = x.id,
                        noEconomico = x.noEconomico,
                        descripcionGrupoMaquina = x.grupoMaquinaria.descripcion
                    }).ToList();

                _maquinas.AddRange(maquinasNoOC);

                idsMaquinasOrdenes.AddRange(maquinasNoOC.Select(x => x.id));

                var economicos = _maquinas.Select(x => x.noEconomico).ToList();

                var horometros = _context.tblM_CapHorometro
                    .Where(x =>
                        !economicos.Contains(x.Economico) &&
                        cc.Contains(x.CC) &&
                        x.Fecha >= fechaInicialMensual &&
                        x.Fecha <= fechaFinalMensual &&
                        (fechaInicialMensual.Month < DateTime.Now.Month ? true : false)).ToList();

                economicos.AddRange(horometros.Select(x => x.Economico).ToList());
                var economicosHorometros = horometros.Select(x => x.Economico).ToList();

                _maquinas.AddRange(_context.tblM_CatMaquina
                    .Where(x =>
                        economicosHorometros.Contains(x.noEconomico) &&
                        x.grupoMaquinaria.tipoEquipoID == 1 &&
                        (tipo != 0 ? x.grupoMaquinaria.id == tipo : true) &&
                        (modelo != 0 ? x.modeloEquipoID == modelo : true))
                    .Select(x => new MaquinaKPIGeneralDTO
                    {
                        id = x.id,
                        noEconomico = x.noEconomico,
                        descripcionGrupoMaquina = x.grupoMaquinaria.descripcion
                    }));

                _horometrosMes = _context.tblM_CapHorometro
                    .Where(x =>
                        economicos.Contains(x.Economico) &&
                        x.Fecha >= fechaInicialMensual &&
                        x.Fecha <= fechaFinalMensual)
                    .OrderBy(x => x.Fecha).ToList();

                foreach (var maquina in _maquinas)
                {
                    _ordenesCerradasPorEquipo = _ordenesCerradas.Where(x => x.EconomicoID == maquina.id).OrderBy(x => x.FechaEntrada).ToList();
                    _ordenesAbiertasPorEquipo = _ordenesAbiertas.Where(x => x.EconomicoID == maquina.id).ToList();

                    _horometrosMesPorEquipo = _horometrosMes.Where(x => x.Economico.Equals(maquina.noEconomico)).OrderBy(x => x.Fecha).ToList();
                    //if (_horometrosMesPorEquipo.Count == 0)
                    //{
                    //    _horometrosMesPorEquipo = _context.tblM_CapHorometro
                    //        .Where(x =>
                    //            x.Economico.Equals(maquina.noEconomico) &&
                    //            cc.Contains(x.CC)).OrderByDescending(x => x.id).Take(1).ToList();
                    //}

                    var kpi = new KPINoFormatDTO();

                    var infoGeneral = getInfoGeneralPorEconomico(maquina, cc, new DateTime(fechaInicio.Year, mes + 1, 1), new DateTime(fechaInicio.Year, mes + 1, DateTime.DaysInMonth(fechaInicio.Year, mes + 1)));
                    var top3Paros = getTop3FrecuenciaParo(maquina, cc, new DateTime(fechaInicio.Year, mes + 1, 1), new DateTime(fechaInicio.Year, mes + 1, DateTime.DaysInMonth(fechaInicio.Year, mes + 1)));
                    var tipoMantenimiento = getMDTipoMantenimiento(maquina, cc, new DateTime(fechaInicio.Year, mes + 1, 1), new DateTime(fechaInicio.Year, mes + 1, DateTime.DaysInMonth(fechaInicio.Year, mes + 1)));
                    var horasTrabajadas = Math.Round(infoGeneral.horasTrabajadas, 2);
                    var horasParo = Math.Round(tipoMantenimiento.tdPTiempo, 2) + Math.Round(tipoMantenimiento.tdCTiempo, 2) + Math.Round(tipoMantenimiento.tdPrTiempo, 2);
                    int DiasPeriodo = Convert.ToInt32((new DateTime(fechaInicio.Year, mes + 1, DateTime.DaysInMonth(fechaInicio.Year, mes + 1)) - new DateTime(fechaInicio.Year, mes + 1, 1)).TotalDays) + 1;

                    if (_ordenesCerradasPorEquipo.Count > 0 || _ordenesAbiertasPorEquipo.Count > 0)
                    {
                        kpi.id = maquina.id;
                        kpi.economico = maquina.noEconomico;
                        kpi.pDisponibilidad = horasParo == 0 ? 100 : (Math.Round(((infoGeneral.MTBS + infoGeneral.MTTR) == 0 ? 1 : (infoGeneral.MTBS / (infoGeneral.MTBS + infoGeneral.MTTR))) * 100, 2));
                        kpi.horasTrabajado = Math.Round(infoGeneral.horasTrabajadas, 2);
                        kpi.horasParo = Math.Round(horasParo, 2);
                        kpi.pMProgramadoTiempo = Math.Round(tipoMantenimiento.tdTPPTiempo, 2);
                        kpi.pMProgramadoCantidad = Math.Round(tipoMantenimiento.tdTPPCantidad, 2);
                        kpi.pPreventivoHoras = Math.Round(tipoMantenimiento.tdPTiempo, 2);
                        kpi.pCorrectivoHoras = Math.Round(tipoMantenimiento.tdCTiempo, 2);
                        kpi.pPredictivoHoras = Math.Round(tipoMantenimiento.tdPrTiempo, 2);
                        kpi.horasHombre = Math.Round(infoGeneral.horasHombre, 2);
                        kpi.MTBS = Math.Round(infoGeneral.MTBS, 2);
                        kpi.MTTR = Math.Round(infoGeneral.MTTR, 2);
                        kpi.pUtilizacion = (Math.Round((horasTrabajadas / ((24 * DiasPeriodo) - horasParo)) * 100, 2));
                    }
                    else
                    {
                        kpi.id = maquina.id;
                        kpi.economico = maquina.noEconomico;
                        kpi.pDisponibilidad = 100;
                        kpi.horasTrabajado = Math.Round(infoGeneral.horasTrabajadas, 2);
                        kpi.horasParo = Math.Round(horasParo, 2);
                        kpi.pMProgramadoTiempo = Math.Round(tipoMantenimiento.tdTPPTiempo, 2);
                        kpi.pMProgramadoCantidad = Math.Round(tipoMantenimiento.tdTPPCantidad, 2);
                        kpi.pPreventivoHoras = Math.Round(tipoMantenimiento.tdPTiempo, 2);
                        kpi.pCorrectivoHoras = Math.Round(tipoMantenimiento.tdCTiempo, 2);
                        kpi.pPredictivoHoras = Math.Round(tipoMantenimiento.tdPrTiempo, 2);
                        kpi.horasHombre = Math.Round(infoGeneral.horasHombre, 2);
                        kpi.MTBS = Math.Round(infoGeneral.MTBS, 2);
                        kpi.MTTR = Math.Round(infoGeneral.MTTR, 2);
                        kpi.pUtilizacion = (Math.Round((horasTrabajadas / ((24 * DiasPeriodo) - horasParo)) * 100, 2));
                    }

                    lista.Add(kpi);
                }

                infoMeses[mes] = lista;
            }
            #endregion

            var result = new kpiRepMetricasDTO();

            #region TablaPrincipal
            result.AnualDTO = new List<kpiAnualDTO>();
            string[] conceptos = new string[] { "MTBS (Hrs)", "MTTR (Hrs)", "DISPONIBILIDAD (%)", "PREVENTIVO", "CORRECTIVO", "PREDICTIVO", "RATIO DE MTTO", "MTTO PROGRAMADO (%)" };

            foreach (var concepto in conceptos)
            {
                var kpi = new kpiAnualDTO();
                if (concepto.Equals("MTBS (Hrs)"))
                {
                    kpi.Concepto = concepto;
                    kpi.Enero = (infoMeses[0].Count == 0 ? 0 : infoMeses[0].Sum(x => x.MTBS) / infoMeses[0].Count);
                    kpi.Febrero = (infoMeses[1].Count == 0 ? 0 : infoMeses[1].Sum(x => x.MTBS) / infoMeses[1].Count);
                    kpi.Marzo = (infoMeses[2].Count == 0 ? 0 : infoMeses[2].Sum(x => x.MTBS) / infoMeses[2].Count);
                    kpi.Abril = (infoMeses[3].Count == 0 ? 0 : infoMeses[3].Sum(x => x.MTBS) / infoMeses[3].Count);
                    kpi.Mayo = (infoMeses[4].Count == 0 ? 0 : infoMeses[4].Sum(x => x.MTBS) / infoMeses[4].Count);
                    kpi.Junio = (infoMeses[5].Count == 0 ? 0 : infoMeses[5].Sum(x => x.MTBS) / infoMeses[5].Count);
                    kpi.Julio = (infoMeses[6].Count == 0 ? 0 : infoMeses[6].Sum(x => x.MTBS) / infoMeses[6].Count);
                    kpi.Agosto = (infoMeses[7].Count == 0 ? 0 : infoMeses[7].Sum(x => x.MTBS) / infoMeses[7].Count);
                    kpi.Septiembre = (infoMeses[8].Count == 0 ? 0 : infoMeses[8].Sum(x => x.MTBS) / infoMeses[8].Count);
                    kpi.Octubre = (infoMeses[9].Count == 0 ? 0 : infoMeses[9].Sum(x => x.MTBS) / infoMeses[9].Count);
                    kpi.Noviembre= (infoMeses[10].Count == 0 ? 0 : infoMeses[10].Sum(x => x.MTBS) / infoMeses[10].Count);
                    kpi.Diciembre = (infoMeses[11].Count == 0 ? 0 : infoMeses[11].Sum(x => x.MTBS) / infoMeses[11].Count);
                    int divisor = 0;
                    if (kpi.Enero > 0)
                        divisor += 1;
                    if (kpi.Febrero > 0)
                        divisor += 1;
                    if (kpi.Marzo > 0)
                        divisor += 1;
                    if (kpi.Abril > 0)
                        divisor += 1;
                    if (kpi.Mayo > 0)
                        divisor += 1;
                    if (kpi.Junio > 0)
                        divisor += 1;
                    if (kpi.Julio > 0)
                        divisor += 1;
                    if (kpi.Agosto > 0)
                        divisor += 1;
                    if (kpi.Septiembre > 0)
                        divisor += 1;
                    if (kpi.Octubre > 0)
                        divisor += 1;
                    if (kpi.Noviembre > 0)
                        divisor += 1;
                    if (kpi.Diciembre > 0)
                        divisor += 1;

                    divisor = divisor == 0 ? 1 : divisor;
                    kpi.Total = (kpi.Enero + kpi.Febrero + kpi.Marzo + kpi.Abril + kpi.Mayo + kpi.Junio + kpi.Julio + kpi.Agosto + kpi.Septiembre + kpi.Octubre + kpi.Noviembre + kpi.Diciembre) / divisor;//(fechaInicio.Year < DateTime.Now.Year ? 12 : fechafin.Month);
                }
                else if (concepto.Equals("MTTR (Hrs)"))
                {
                    kpi.Concepto = concepto;
                    kpi.Enero = (infoMeses[0].Count == 0 ? 0 : infoMeses[0].Sum(x => x.MTTR) / infoMeses[0].Count);
                    kpi.Febrero = (infoMeses[1].Count == 0 ? 0 : infoMeses[1].Sum(x => x.MTTR) / infoMeses[1].Count);
                    kpi.Marzo = (infoMeses[2].Count == 0 ? 0 : infoMeses[2].Sum(x => x.MTTR) / infoMeses[2].Count);
                    kpi.Abril = (infoMeses[3].Count == 0 ? 0 : infoMeses[3].Sum(x => x.MTTR) / infoMeses[3].Count);
                    kpi.Mayo = (infoMeses[4].Count == 0 ? 0 : infoMeses[4].Sum(x => x.MTTR) / infoMeses[4].Count);
                    kpi.Junio = (infoMeses[5].Count == 0 ? 0 : infoMeses[5].Sum(x => x.MTTR) / infoMeses[5].Count);
                    kpi.Julio = (infoMeses[6].Count == 0 ? 0 : infoMeses[6].Sum(x => x.MTTR) / infoMeses[6].Count);
                    kpi.Agosto = (infoMeses[7].Count == 0 ? 0 : infoMeses[7].Sum(x => x.MTTR) / infoMeses[7].Count);
                    kpi.Septiembre = (infoMeses[8].Count == 0 ? 0 : infoMeses[8].Sum(x => x.MTTR) / infoMeses[8].Count);
                    kpi.Octubre = (infoMeses[9].Count == 0 ? 0 : infoMeses[9].Sum(x => x.MTTR) / infoMeses[9].Count);
                    kpi.Noviembre= (infoMeses[10].Count == 0 ? 0 : infoMeses[10].Sum(x => x.MTTR) / infoMeses[10].Count);
                    kpi.Diciembre = (infoMeses[11].Count == 0 ? 0 : infoMeses[11].Sum(x => x.MTTR) / infoMeses[11].Count);
                    int divisor = 0;
                    if (kpi.Enero > 0)
                        divisor += 1;
                    if (kpi.Febrero > 0)
                        divisor += 1;
                    if (kpi.Marzo > 0)
                        divisor += 1;
                    if (kpi.Abril > 0)
                        divisor += 1;
                    if (kpi.Mayo > 0)
                        divisor += 1;
                    if (kpi.Junio > 0)
                        divisor += 1;
                    if (kpi.Julio > 0)
                        divisor += 1;
                    if (kpi.Agosto > 0)
                        divisor += 1;
                    if (kpi.Septiembre > 0)
                        divisor += 1;
                    if (kpi.Octubre > 0)
                        divisor += 1;
                    if (kpi.Noviembre > 0)
                        divisor += 1;
                    if (kpi.Diciembre > 0)
                        divisor += 1;

                    divisor = divisor == 0 ? 1 : divisor;
                    kpi.Total = (kpi.Enero + kpi.Febrero + kpi.Marzo + kpi.Abril + kpi.Mayo + kpi.Junio + kpi.Julio + kpi.Agosto + kpi.Septiembre + kpi.Octubre + kpi.Noviembre + kpi.Diciembre) / divisor;// (fechaInicio.Year < DateTime.Now.Year ? 12 : fechafin.Month);
                }
                else if (concepto.Equals("DISPONIBILIDAD (%)"))
                {
                    kpi.Concepto = concepto;
                    kpi.Enero = (infoMeses[0].Count == 0 ? 0 : infoMeses[0].Sum(x => x.pDisponibilidad) / infoMeses[0].Count);
                    kpi.Febrero = (infoMeses[1].Count == 0 ? 0 : infoMeses[1].Sum(x => x.pDisponibilidad) / infoMeses[1].Count);
                    kpi.Marzo = (infoMeses[2].Count == 0 ? 0 : infoMeses[2].Sum(x => x.pDisponibilidad) / infoMeses[2].Count);
                    kpi.Abril = (infoMeses[3].Count == 0 ? 0 : infoMeses[3].Sum(x => x.pDisponibilidad) / infoMeses[3].Count);
                    kpi.Mayo = (infoMeses[4].Count == 0 ? 0 : infoMeses[4].Sum(x => x.pDisponibilidad) / infoMeses[4].Count);
                    kpi.Junio = (infoMeses[5].Count == 0 ? 0 : infoMeses[5].Sum(x => x.pDisponibilidad) / infoMeses[5].Count);
                    kpi.Julio = (infoMeses[6].Count == 0 ? 0 : infoMeses[6].Sum(x => x.pDisponibilidad) / infoMeses[6].Count);
                    kpi.Agosto = (infoMeses[7].Count == 0 ? 0 : infoMeses[7].Sum(x => x.pDisponibilidad) / infoMeses[7].Count);
                    kpi.Septiembre = (infoMeses[8].Count == 0 ? 0 : infoMeses[8].Sum(x => x.pDisponibilidad) / infoMeses[8].Count);
                    kpi.Octubre = (infoMeses[9].Count == 0 ? 0 : infoMeses[9].Sum(x => x.pDisponibilidad) / infoMeses[9].Count);
                    kpi.Noviembre= (infoMeses[10].Count == 0 ? 0 : infoMeses[10].Sum(x => x.pDisponibilidad) / infoMeses[10].Count);
                    kpi.Diciembre = (infoMeses[11].Count == 0 ? 0 : infoMeses[11].Sum(x => x.pDisponibilidad) / infoMeses[11].Count);
                    int divisor = 0;
                    if (kpi.Enero > 0)
                        divisor += 1;
                    if (kpi.Febrero > 0)
                        divisor += 1;
                    if (kpi.Marzo > 0)
                        divisor += 1;
                    if (kpi.Abril > 0)
                        divisor += 1;
                    if (kpi.Mayo > 0)
                        divisor += 1;
                    if (kpi.Junio > 0)
                        divisor += 1;
                    if (kpi.Julio > 0)
                        divisor += 1;
                    if (kpi.Agosto > 0)
                        divisor += 1;
                    if (kpi.Septiembre > 0)
                        divisor += 1;
                    if (kpi.Octubre > 0)
                        divisor += 1;
                    if (kpi.Noviembre > 0)
                        divisor += 1;
                    if (kpi.Diciembre > 0)
                        divisor += 1;

                    divisor = divisor == 0 ? 1 : divisor;
                    kpi.Total = (kpi.Enero + kpi.Febrero + kpi.Marzo + kpi.Abril + kpi.Mayo + kpi.Junio + kpi.Julio + kpi.Agosto + kpi.Septiembre + kpi.Octubre + kpi.Noviembre + kpi.Diciembre) / divisor;// (fechaInicio.Year < DateTime.Now.Year ? 12 : fechafin.Month);
                }
                else if (concepto.Equals("PREVENTIVO"))
                {
                    kpi.Concepto = concepto;
                    kpi.Enero = infoMeses[0].Sum(x => x.pPreventivoHoras);
                    kpi.Febrero = infoMeses[1].Sum(x => x.pPreventivoHoras);
                    kpi.Marzo = infoMeses[2].Sum(x => x.pPreventivoHoras);
                    kpi.Abril = infoMeses[3].Sum(x => x.pPreventivoHoras);
                    kpi.Mayo = infoMeses[4].Sum(x => x.pPreventivoHoras);
                    kpi.Junio = infoMeses[5].Sum(x => x.pPreventivoHoras);
                    kpi.Julio = infoMeses[6].Sum(x => x.pPreventivoHoras);
                    kpi.Agosto = infoMeses[7].Sum(x => x.pPreventivoHoras);
                    kpi.Septiembre = infoMeses[8].Sum(x => x.pPreventivoHoras);
                    kpi.Octubre = infoMeses[9].Sum(x => x.pPreventivoHoras);
                    kpi.Noviembre = infoMeses[10].Sum(x => x.pPreventivoHoras);
                    kpi.Diciembre = infoMeses[11].Sum(x => x.pPreventivoHoras);

                    kpi.Total = (kpi.Enero + kpi.Febrero + kpi.Marzo + kpi.Abril + kpi.Mayo + kpi.Junio + kpi.Julio + kpi.Agosto + kpi.Septiembre + kpi.Octubre + kpi.Noviembre + kpi.Diciembre);
                }
                else if (concepto.Equals("CORRECTIVO"))
                {
                    kpi.Concepto = concepto;
                    kpi.Enero = infoMeses[0].Sum(x => x.pCorrectivoHoras);
                    kpi.Febrero = infoMeses[1].Sum(x => x.pCorrectivoHoras);
                    kpi.Marzo = infoMeses[2].Sum(x => x.pCorrectivoHoras);
                    kpi.Abril = infoMeses[3].Sum(x => x.pCorrectivoHoras);
                    kpi.Mayo = infoMeses[4].Sum(x => x.pCorrectivoHoras);
                    kpi.Junio = infoMeses[5].Sum(x => x.pCorrectivoHoras);
                    kpi.Julio = infoMeses[6].Sum(x => x.pCorrectivoHoras);
                    kpi.Agosto = infoMeses[7].Sum(x => x.pCorrectivoHoras);
                    kpi.Septiembre = infoMeses[8].Sum(x => x.pCorrectivoHoras);
                    kpi.Octubre = infoMeses[9].Sum(x => x.pCorrectivoHoras);
                    kpi.Noviembre = infoMeses[10].Sum(x => x.pCorrectivoHoras);
                    kpi.Diciembre = infoMeses[11].Sum(x => x.pCorrectivoHoras);

                    kpi.Total = (kpi.Enero + kpi.Febrero + kpi.Marzo + kpi.Abril + kpi.Mayo + kpi.Junio + kpi.Julio + kpi.Agosto + kpi.Septiembre + kpi.Octubre + kpi.Noviembre + kpi.Diciembre);
                }
                else if (concepto.Equals("PREDICTIVO"))
                {
                    kpi.Concepto = concepto;
                    kpi.Enero = infoMeses[0].Sum(x => x.pPredictivoHoras);
                    kpi.Febrero = infoMeses[1].Sum(x => x.pPredictivoHoras);
                    kpi.Marzo = infoMeses[2].Sum(x => x.pPredictivoHoras);
                    kpi.Abril = infoMeses[3].Sum(x => x.pPredictivoHoras);
                    kpi.Mayo = infoMeses[4].Sum(x => x.pPredictivoHoras);
                    kpi.Junio = infoMeses[5].Sum(x => x.pPredictivoHoras);
                    kpi.Julio = infoMeses[6].Sum(x => x.pPredictivoHoras);
                    kpi.Agosto = infoMeses[7].Sum(x => x.pPredictivoHoras);
                    kpi.Septiembre = infoMeses[8].Sum(x => x.pPredictivoHoras);
                    kpi.Octubre = infoMeses[9].Sum(x => x.pPredictivoHoras);
                    kpi.Noviembre = infoMeses[10].Sum(x => x.pPredictivoHoras);
                    kpi.Diciembre = infoMeses[11].Sum(x => x.pPredictivoHoras);

                    kpi.Total = (kpi.Enero + kpi.Febrero + kpi.Marzo + kpi.Abril + kpi.Mayo + kpi.Junio + kpi.Julio + kpi.Agosto + kpi.Septiembre + kpi.Octubre + kpi.Noviembre + kpi.Diciembre);
                }
                else if (concepto.Equals("RATIO DE MTTO"))
                {
                    kpi.Concepto = concepto;
                    kpi.Enero = infoMeses[0].Sum(x => x.horasTrabajado) == 0 ? 0 : infoMeses[0].Sum(x => x.horasHombre) / infoMeses[0].Sum(x => x.horasTrabajado);
                    kpi.Febrero = infoMeses[1].Sum(x => x.horasTrabajado) == 0 ? 0 : infoMeses[1].Sum(x => x.horasHombre) / infoMeses[1].Sum(x => x.horasTrabajado);
                    kpi.Marzo = infoMeses[2].Sum(x => x.horasTrabajado) == 0 ? 0 : infoMeses[2].Sum(x => x.horasHombre) / infoMeses[2].Sum(x => x.horasTrabajado);
                    kpi.Abril = infoMeses[3].Sum(x => x.horasTrabajado) == 0 ? 0 : infoMeses[3].Sum(x => x.horasHombre) / infoMeses[3].Sum(x => x.horasTrabajado);
                    kpi.Mayo = infoMeses[4].Sum(x => x.horasTrabajado) == 0 ? 0 : infoMeses[4].Sum(x => x.horasHombre) / infoMeses[4].Sum(x => x.horasTrabajado);
                    kpi.Junio = infoMeses[5].Sum(x => x.horasTrabajado) == 0 ? 0 : infoMeses[5].Sum(x => x.horasHombre) / infoMeses[5].Sum(x => x.horasTrabajado);
                    kpi.Julio = infoMeses[6].Sum(x => x.horasTrabajado) == 0 ? 0 : infoMeses[6].Sum(x => x.horasHombre) / infoMeses[6].Sum(x => x.horasTrabajado);
                    kpi.Agosto = infoMeses[7].Sum(x => x.horasTrabajado) == 0 ? 0 : infoMeses[7].Sum(x => x.horasHombre) / infoMeses[7].Sum(x => x.horasTrabajado);
                    kpi.Septiembre = infoMeses[8].Sum(x => x.horasTrabajado) == 0 ? 0 : infoMeses[8].Sum(x => x.horasHombre) / infoMeses[8].Sum(x => x.horasTrabajado);
                    kpi.Octubre = infoMeses[9].Sum(x => x.horasTrabajado) == 0 ? 0 : infoMeses[9].Sum(x => x.horasHombre) / infoMeses[9].Sum(x => x.horasTrabajado);
                    kpi.Noviembre = infoMeses[10].Sum(x => x.horasTrabajado) == 0 ? 0 : infoMeses[10].Sum(x => x.horasHombre) / infoMeses[10].Sum(x => x.horasTrabajado);
                    kpi.Diciembre = infoMeses[11].Sum(x => x.horasTrabajado) == 0 ? 0 : infoMeses[11].Sum(x => x.horasHombre) / infoMeses[11].Sum(x => x.horasTrabajado);
                    int divisor = 0;
                    if (kpi.Enero > 0)
                        divisor += 1;
                    if (kpi.Febrero > 0)
                        divisor += 1;
                    if (kpi.Marzo > 0)
                        divisor += 1;
                    if (kpi.Abril > 0)
                        divisor += 1;
                    if (kpi.Mayo > 0)
                        divisor += 1;
                    if (kpi.Junio > 0)
                        divisor += 1;
                    if (kpi.Julio > 0)
                        divisor += 1;
                    if (kpi.Agosto > 0)
                        divisor += 1;
                    if (kpi.Septiembre > 0)
                        divisor += 1;
                    if (kpi.Octubre > 0)
                        divisor += 1;
                    if (kpi.Noviembre > 0)
                        divisor += 1;
                    if (kpi.Diciembre > 0)
                        divisor += 1;

                    divisor = divisor == 0 ? 1 : divisor;
                    kpi.Total = (kpi.Enero + kpi.Febrero + kpi.Marzo + kpi.Abril + kpi.Mayo + kpi.Junio + kpi.Julio + kpi.Agosto + kpi.Septiembre + kpi.Octubre + kpi.Noviembre + kpi.Diciembre) / divisor;// (fechaInicio.Year < DateTime.Now.Year ? 12 : fechafin.Month);
                }
                else if (concepto.Equals("MTTO PROGRAMADO (%)"))
                {
                    kpi.Concepto = concepto;
                    kpi.Enero = (infoMeses[0].Count == 0 ? 0 : infoMeses[0].Sum(x => x.pMProgramadoTiempo) / infoMeses[0].Count);
                    kpi.Febrero = (infoMeses[1].Count == 0 ? 0 : infoMeses[1].Sum(x => x.pMProgramadoTiempo) / infoMeses[1].Count);
                    kpi.Marzo = (infoMeses[2].Count == 0 ? 0 : infoMeses[2].Sum(x => x.pMProgramadoTiempo) / infoMeses[2].Count);
                    kpi.Abril = (infoMeses[3].Count == 0 ? 0 : infoMeses[3].Sum(x => x.pMProgramadoTiempo) / infoMeses[3].Count);
                    kpi.Mayo = (infoMeses[4].Count == 0 ? 0 : infoMeses[4].Sum(x => x.pMProgramadoTiempo) / infoMeses[4].Count);
                    kpi.Junio = (infoMeses[5].Count == 0 ? 0 : infoMeses[5].Sum(x => x.pMProgramadoTiempo) / infoMeses[5].Count);
                    kpi.Julio = (infoMeses[6].Count == 0 ? 0 : infoMeses[6].Sum(x => x.pMProgramadoTiempo) / infoMeses[6].Count);
                    kpi.Agosto = (infoMeses[7].Count == 0 ? 0 : infoMeses[7].Sum(x => x.pMProgramadoTiempo) / infoMeses[7].Count);
                    kpi.Septiembre = (infoMeses[8].Count == 0 ? 0 : infoMeses[8].Sum(x => x.pMProgramadoTiempo) / infoMeses[8].Count);
                    kpi.Octubre = (infoMeses[9].Count == 0 ? 0 : infoMeses[9].Sum(x => x.pMProgramadoTiempo) / infoMeses[9].Count);
                    kpi.Noviembre= (infoMeses[10].Count == 0 ? 0 : infoMeses[10].Sum(x => x.pMProgramadoTiempo) / infoMeses[10].Count);
                    kpi.Diciembre = (infoMeses[11].Count == 0 ? 0 : infoMeses[11].Sum(x => x.pMProgramadoTiempo) / infoMeses[11].Count);
                    int divisor = 0;
                    if (kpi.Enero > 0)
                        divisor += 1;
                    if (kpi.Febrero > 0)
                        divisor += 1;
                    if (kpi.Marzo > 0)
                        divisor += 1;
                    if (kpi.Abril > 0)
                        divisor += 1;
                    if (kpi.Mayo > 0)
                        divisor += 1;
                    if (kpi.Junio > 0)
                        divisor += 1;
                    if (kpi.Julio > 0)
                        divisor += 1;
                    if (kpi.Agosto > 0)
                        divisor += 1;
                    if (kpi.Septiembre > 0)
                        divisor += 1;
                    if (kpi.Octubre > 0)
                        divisor += 1;
                    if (kpi.Noviembre > 0)
                        divisor += 1;
                    if (kpi.Diciembre > 0)
                        divisor += 1;

                    divisor = divisor == 0 ? 1 : divisor;
                    kpi.Total = (kpi.Enero + kpi.Febrero + kpi.Marzo + kpi.Abril + kpi.Mayo + kpi.Junio + kpi.Julio + kpi.Agosto + kpi.Septiembre + kpi.Octubre + kpi.Noviembre + kpi.Diciembre) / divisor;// (fechaInicio.Year < DateTime.Now.Year ? 12 : fechafin.Month);
                }
                result.AnualDTO.Add(kpi);
            }
            #endregion

            var TempMTBS = result.AnualDTO.FirstOrDefault(x => x.Concepto.Equals("MTBS (Hrs)"));
            var TempMTTR = result.AnualDTO.FirstOrDefault(x => x.Concepto.Equals("MTTR (Hrs)"));
            var TempDISPONIBILIDAD = result.AnualDTO.FirstOrDefault(x => x.Concepto.Equals("DISPONIBILIDAD (%)"));
            var TempPREVENTIVO = result.AnualDTO.FirstOrDefault(x => x.Concepto.Equals("PREVENTIVO"));
            var TempCORRECTIVO = result.AnualDTO.FirstOrDefault(x => x.Concepto.Equals("CORRECTIVO"));
            var TempPREDICTIVO = result.AnualDTO.FirstOrDefault(x => x.Concepto.Equals("PREDICTIVO"));
            var TempRATIO_MTTO = result.AnualDTO.FirstOrDefault(x => x.Concepto.Equals("RATIO DE MTTO"));
            var TempMTTO_PROGRAMADO = result.AnualDTO.FirstOrDefault(x => x.Concepto.Equals("MTTO PROGRAMADO (%)"));

            #region GraficaTiempos_Paro
            result.kpiMTGraficaTiemposParo = new List<kpiMTGraficaTiemposParoDTO>();

            result.kpiMTGraficaTiemposParo.Add(new kpiMTGraficaTiemposParoDTO
            {
                CONCEPTO = "ENE",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Enero,
                MTBS = TempMTBS.Enero,
                MTTR = TempMTTR.Enero,
                RATIO_MTTO = TempRATIO_MTTO.Enero
            });//Enero
            result.kpiMTGraficaTiemposParo.Add(new kpiMTGraficaTiemposParoDTO
            {
                CONCEPTO = "FEB",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Febrero,
                MTBS = TempMTBS.Febrero,
                MTTR = TempMTTR.Febrero,
                RATIO_MTTO = TempRATIO_MTTO.Febrero
            });//Febrero
            result.kpiMTGraficaTiemposParo.Add(new kpiMTGraficaTiemposParoDTO
            {
                CONCEPTO = "MAR",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Marzo,
                MTBS = TempMTBS.Marzo,
                MTTR = TempMTTR.Marzo,
                RATIO_MTTO = TempRATIO_MTTO.Marzo
            });//Marzo
            result.kpiMTGraficaTiemposParo.Add(new kpiMTGraficaTiemposParoDTO
            {
                CONCEPTO = "ABR",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Abril,
                MTBS = TempMTBS.Abril,
                MTTR = TempMTTR.Abril,
                RATIO_MTTO = TempRATIO_MTTO.Abril
            });//Abril
            result.kpiMTGraficaTiemposParo.Add(new kpiMTGraficaTiemposParoDTO
            {
                CONCEPTO = "MAY",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Mayo,
                MTBS = TempMTBS.Mayo,
                MTTR = TempMTTR.Mayo,
                RATIO_MTTO = TempRATIO_MTTO.Mayo
            });//Mayo
            result.kpiMTGraficaTiemposParo.Add(new kpiMTGraficaTiemposParoDTO
            {
                CONCEPTO = "JUN",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Junio,
                MTBS = TempMTBS.Junio,
                MTTR = TempMTTR.Junio,
                RATIO_MTTO = TempRATIO_MTTO.Junio
            });//Junio
            result.kpiMTGraficaTiemposParo.Add(new kpiMTGraficaTiemposParoDTO
            {
                CONCEPTO = "JUL",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Julio,
                MTBS = TempMTBS.Julio,
                MTTR = TempMTTR.Julio,
                RATIO_MTTO = TempRATIO_MTTO.Julio
            });//Julio
            result.kpiMTGraficaTiemposParo.Add(new kpiMTGraficaTiemposParoDTO
            {
                CONCEPTO = "AGO",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Agosto,
                MTBS = TempMTBS.Agosto,
                MTTR = TempMTTR.Agosto,
                RATIO_MTTO = TempRATIO_MTTO.Agosto
            });//Agosto
            result.kpiMTGraficaTiemposParo.Add(new kpiMTGraficaTiemposParoDTO
            {
                CONCEPTO = "SEP",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Septiembre,
                MTBS = TempMTBS.Septiembre,
                MTTR = TempMTTR.Septiembre,
                RATIO_MTTO = TempRATIO_MTTO.Septiembre
            });//Septiembre
            result.kpiMTGraficaTiemposParo.Add(new kpiMTGraficaTiemposParoDTO
            {
                CONCEPTO = "OCT",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Octubre,
                MTBS = TempMTBS.Octubre,
                MTTR = TempMTTR.Octubre,
                RATIO_MTTO = TempRATIO_MTTO.Octubre
            });//Octubre
            result.kpiMTGraficaTiemposParo.Add(new kpiMTGraficaTiemposParoDTO
            {
                CONCEPTO = "NOV",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Noviembre,
                MTBS = TempMTBS.Noviembre,
                MTTR = TempMTTR.Noviembre,
                RATIO_MTTO = TempRATIO_MTTO.Noviembre
            });//Noviembre
            result.kpiMTGraficaTiemposParo.Add(new kpiMTGraficaTiemposParoDTO
            {
                CONCEPTO = "DIC",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Diciembre,
                MTBS = TempMTBS.Diciembre,
                MTTR = TempMTTR.Diciembre,
                RATIO_MTTO = TempRATIO_MTTO.Diciembre
            });//Diciembre
            #endregion

            #region GraficaPorcentaje_Disponibildiad
            result.kpiMTGraficaDisponibilidad = new List<kpiMTGraficaDisponibilidadDTO>();
            result.kpiMTGraficaDisponibilidad.Add(new kpiMTGraficaDisponibilidadDTO
            {
                CONCEPTO = "ENE",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Enero
            });//Enero
            result.kpiMTGraficaDisponibilidad.Add(new kpiMTGraficaDisponibilidadDTO
            {
                CONCEPTO = "FEB",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Febrero
            });//Febrero
            result.kpiMTGraficaDisponibilidad.Add(new kpiMTGraficaDisponibilidadDTO
            {
                CONCEPTO = "MAR",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Marzo
            });//Marzo
            result.kpiMTGraficaDisponibilidad.Add(new kpiMTGraficaDisponibilidadDTO
            {
                CONCEPTO = "ABR",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Abril
            });//Abril
            result.kpiMTGraficaDisponibilidad.Add(new kpiMTGraficaDisponibilidadDTO
            {
                CONCEPTO = "MAY",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Mayo
            });//Mayo
            result.kpiMTGraficaDisponibilidad.Add(new kpiMTGraficaDisponibilidadDTO
            {
                CONCEPTO = "JUN",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Junio
            });//Junio
            result.kpiMTGraficaDisponibilidad.Add(new kpiMTGraficaDisponibilidadDTO
            {
                CONCEPTO = "JUL",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Julio
            });//Julio
            result.kpiMTGraficaDisponibilidad.Add(new kpiMTGraficaDisponibilidadDTO
            {
                CONCEPTO = "AGO",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Agosto
            });//Agosto
            result.kpiMTGraficaDisponibilidad.Add(new kpiMTGraficaDisponibilidadDTO
            {
                CONCEPTO = "SEP",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Septiembre
            });//Septiembre
            result.kpiMTGraficaDisponibilidad.Add(new kpiMTGraficaDisponibilidadDTO
            {
                CONCEPTO = "OCT",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Octubre
            });//Octubre
            result.kpiMTGraficaDisponibilidad.Add(new kpiMTGraficaDisponibilidadDTO
            {
                CONCEPTO = "NOV",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Noviembre
            });//Noviembre
            result.kpiMTGraficaDisponibilidad.Add(new kpiMTGraficaDisponibilidadDTO
            {
                CONCEPTO = "DIC",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Diciembre
            });//Diciembre
            #endregion

            #region GraficaTendencia_TiposMTTO
            result.kpiMTGraficaTendenciaMTTO = new List<kpiMTGraficaTendenciaMTTODTO>();
            result.kpiMTGraficaTendenciaMTTO.Add(new kpiMTGraficaTendenciaMTTODTO
            {
                CONCEPTO = "ENE",
                PREVENTIVO = TempPREVENTIVO.Enero,
                CORRECTIVO = TempCORRECTIVO.Enero,
                PREDICTIVO = TempPREDICTIVO.Enero
            });//Enero
            result.kpiMTGraficaTendenciaMTTO.Add(new kpiMTGraficaTendenciaMTTODTO
            {
                CONCEPTO = "FEB",
                PREVENTIVO = TempPREVENTIVO.Febrero,
                CORRECTIVO = TempCORRECTIVO.Febrero,
                PREDICTIVO = TempPREDICTIVO.Febrero
            });//Febrero
            result.kpiMTGraficaTendenciaMTTO.Add(new kpiMTGraficaTendenciaMTTODTO
            {
                CONCEPTO = "MAR",
                PREVENTIVO = TempPREVENTIVO.Marzo,
                CORRECTIVO = TempCORRECTIVO.Marzo,
                PREDICTIVO = TempPREDICTIVO.Marzo
            });//Marzo
            result.kpiMTGraficaTendenciaMTTO.Add(new kpiMTGraficaTendenciaMTTODTO
            {
                CONCEPTO = "ABR",
                PREVENTIVO = TempPREVENTIVO.Abril,
                CORRECTIVO = TempCORRECTIVO.Abril,
                PREDICTIVO = TempPREDICTIVO.Abril
            });//Abril
            result.kpiMTGraficaTendenciaMTTO.Add(new kpiMTGraficaTendenciaMTTODTO
            {
                CONCEPTO = "MAY",
                PREVENTIVO = TempPREVENTIVO.Mayo,
                CORRECTIVO = TempCORRECTIVO.Mayo,
                PREDICTIVO = TempPREDICTIVO.Mayo
            });//Mayo
            result.kpiMTGraficaTendenciaMTTO.Add(new kpiMTGraficaTendenciaMTTODTO
            {
                CONCEPTO = "JUN",
                PREVENTIVO = TempPREVENTIVO.Junio,
                CORRECTIVO = TempCORRECTIVO.Junio,
                PREDICTIVO = TempPREDICTIVO.Junio
            });//Junio
            result.kpiMTGraficaTendenciaMTTO.Add(new kpiMTGraficaTendenciaMTTODTO
            {
                CONCEPTO = "JUL",
                PREVENTIVO = TempPREVENTIVO.Julio,
                CORRECTIVO = TempCORRECTIVO.Julio,
                PREDICTIVO = TempPREDICTIVO.Julio
            });//Julio
            result.kpiMTGraficaTendenciaMTTO.Add(new kpiMTGraficaTendenciaMTTODTO
            {
                CONCEPTO = "AGO",
                PREVENTIVO = TempPREVENTIVO.Agosto,
                CORRECTIVO = TempCORRECTIVO.Agosto,
                PREDICTIVO = TempPREDICTIVO.Agosto
            });//Agosto
            result.kpiMTGraficaTendenciaMTTO.Add(new kpiMTGraficaTendenciaMTTODTO
            {
                CONCEPTO = "SEP",
                PREVENTIVO = TempPREVENTIVO.Septiembre,
                CORRECTIVO = TempCORRECTIVO.Septiembre,
                PREDICTIVO = TempPREDICTIVO.Septiembre
            });//Septiembre
            result.kpiMTGraficaTendenciaMTTO.Add(new kpiMTGraficaTendenciaMTTODTO
            {
                CONCEPTO = "OCT",
                PREVENTIVO = TempPREVENTIVO.Octubre,
                CORRECTIVO = TempCORRECTIVO.Octubre,
                PREDICTIVO = TempPREDICTIVO.Octubre
            });//Octubre
            result.kpiMTGraficaTendenciaMTTO.Add(new kpiMTGraficaTendenciaMTTODTO
            {
                CONCEPTO = "NOV",
                PREVENTIVO = TempPREVENTIVO.Noviembre,
                CORRECTIVO = TempCORRECTIVO.Noviembre,
                PREDICTIVO = TempPREDICTIVO.Noviembre
            });//Noviembre
            result.kpiMTGraficaTendenciaMTTO.Add(new kpiMTGraficaTendenciaMTTODTO
            {
                CONCEPTO = "DIC",
                PREVENTIVO = TempPREVENTIVO.Diciembre,
                CORRECTIVO = TempCORRECTIVO.Diciembre,
                PREDICTIVO = TempPREDICTIVO.Diciembre
            });//Diciembre
            #endregion

            #region GraficaAnual_TiposMTTO
            result.kpiMTGraficaTiposMTTO = new List<kpiMTGraficaTiposMTTODTO>();
            result.kpiMTGraficaTiposMTTO.Add(new kpiMTGraficaTiposMTTODTO
            {
                CONCEPTO = "PREVENTIVO",
                CANTIDAD = TempPREVENTIVO.Total
            });//Preventivo
            result.kpiMTGraficaTiposMTTO.Add(new kpiMTGraficaTiposMTTODTO
            {
                CONCEPTO = "CORRECTIVO",
                CANTIDAD = TempCORRECTIVO.Total
            });//Correctivo
            result.kpiMTGraficaTiposMTTO.Add(new kpiMTGraficaTiposMTTODTO
            {
                CONCEPTO = "PREDICTIVO",
                CANTIDAD = TempPREDICTIVO.Total
            });//Predictivo
            #endregion

            return result;
        }
        #endregion

        public IList<KPIDTO> getKPIGeneral(List<string> cc, int tipo, int modelo, DateTime Fechainicio, DateTime FechaFin)
        {
            return getKPIGeneral2(cc, tipo, modelo, Fechainicio, FechaFin);

            List<KPIDTO> lista = new List<KPIDTO>();

            /*     var maquinas = _context.tblM_CatMaquina
                     .Where(
                         x => x.centro_costos.Equals(cc) &&
                              x.estatus != 0 &&
                              (tipo != 0 ? x.grupoMaquinaria.id == tipo /*&& x.modeloEquipoID==modelo: x.grupoMaquinaria.tipoEquipoID == 1)
                              ).Where(y => (modelo != 0 ? y.modeloEquipoID == modelo : y.id == y.id)).ToList();*/


            var maquinasRaw = (from or in _context.tblM_CapOrdenTrabajo
                            join m in _context.tblM_CatMaquina on or.EconomicoID equals m.id
                            where cc.Contains(or.CC) && m.grupoMaquinaria.tipoEquipoID==1 && m.estatus != 0 && (tipo != 0 ? m.grupoMaquinaria.id == tipo : m.id == m.id) &&
                            (modelo != 0 ? m.modeloEquipoID == modelo : m.id == m.id) && (or.FechaEntrada >= Fechainicio && or.FechaEntrada <= FechaFin)
                            select m).GroupBy(x => x).Select(x => x.Key).ToList();

            var noEcos = new List<string>();
            noEcos.AddRange(maquinasRaw.Select(x => x.noEconomico));
            
            var maquinasNoOC = (from m in _context.tblM_CatMaquina
                                where cc.Contains(m.centro_costos) && m.grupoMaquinaria.tipoEquipoID == 1 && m.estatus != 0 && (tipo != 0 ? m.grupoMaquinaria.id == tipo : m.id == m.id) &&
                                (modelo != 0 ? m.modeloEquipoID == modelo : m.id == m.id) && (!noEcos.Contains(m.noEconomico)) && (FechaFin.Month==DateTime.Now.Month?true:false)
                                select m).GroupBy(x => x).Select(x => x.Key).ToList();
            var noEcosA = new List<string>();
            noEcosA.AddRange(maquinasNoOC.Select(x => x.noEconomico));
            var maquinasXH = (from m in _context.tblM_CatMaquina
                              join h in _context.tblM_CapHorometro on m.noEconomico equals h.Economico
                              where cc.Contains(h.CC) && m.grupoMaquinaria.tipoEquipoID == 1 && (tipo != 0 ? m.grupoMaquinaria.id == tipo : m.id == m.id) &&
                                (modelo != 0 ? m.modeloEquipoID == modelo : m.id == m.id) && (!noEcos.Contains(m.noEconomico) && !noEcosA.Contains(m.noEconomico)) && (FechaFin.Month < DateTime.Now.Month ? true : false) && (h.Fecha >= Fechainicio && h.Fecha <= FechaFin)
                              select m).GroupBy(x => x).Select(x => x.Key).ToList();
            var maquinas = maquinasRaw.Union(maquinasNoOC).Union(maquinasXH).ToList();
            /*&& x.modeloEquipoID==modelo: x.grupoMaquinaria.tipoEquipoID == 1)*/



            foreach (var i in maquinas)
            {
                if (i.noEconomico.Equals("CFC-R03")) {
                    var a = "";
                }
                //var ordenes = _context.
                //                tblM_CapOrdenTrabajo
                //                .Where(x => x.EconomicoID == i.id &&
                //                            x.CC.Equals(cc) &&
                //                    /*x.FechaEntrada.Year == anio &&
                //                    x.FechaEntrada.Month == mes &&*/
                //                             (x.FechaEntrada >= Fechainicio && x.FechaEntrada <= FechaFin) &&
                //                            x.FechaSalida != null)
                //                            .OrderBy(x => x.FechaEntrada).ToList();
                //var OrdenesAbiertas = _context.
                //                tblM_CapOrdenTrabajo
                //                .Where(x => x.EconomicoID == i.id &&
                //                            x.CC.Equals(cc) &&
                //                           (x.TipoOT == 1 ||
                //                            x.EstatusOT == true))
                //                            .OrderBy(x => x.FechaEntrada).ToList();

                //     ordenes.AddRange(OrdenesAbiertas);

                var o = new KPIDTO();
                //------------------------
                // var proyeccionObj = _context.tblM_KPI.FirstOrDefault(x => x.noEconomico.Equals(i.noEconomico) && x.cc.Equals(cc) && x.anio == anio && x.mes == mes); // No se usa,
                //var proyeccion = proyeccionObj == null ? 0 : proyeccionObj.proyeccionHoras; //No se usa,

                var infoGeneral = getInfoGeneral(i.id, cc, Fechainicio, FechaFin);
                var top3Paros = getTop3FrecuenciaParo(i.id, cc, Fechainicio, FechaFin);
                var tipoMantenimiento = getMDTipoMantenimiento(i.id, cc, Fechainicio, FechaFin);
                var horasTrabajadas = Math.Round(infoGeneral.horasTrabajadas, 2);
                var horasParo = Math.Round(tipoMantenimiento.tdPTiempo, 2) + Math.Round(tipoMantenimiento.tdCTiempo, 2) + Math.Round(tipoMantenimiento.tdPrTiempo, 2);//Math.Round(infoGeneral.horasParo, 2);

                int DiasPeriodo = Convert.ToInt32((FechaFin - Fechainicio).TotalDays) + 1;

                o.id = i.id;
                o.btnEquipo = "<button class='btn btn-primary' onclick='getKPIEquipo(" + i.id + ",\"" + i.noEconomico + "\")' title='Inf. General'><i class='glyphicon glyphicon-eye-open'></<i></button>";
                o.economico = i.noEconomico;
                //o.horasIdealMensual = Math.Round(proyeccion, 0).ToString();
                o.pDisponibilidad = infoGeneral.disponibilidad.Trim('%') + "%"; // infoGeneral.horasParo == 0 ? "100%" : (Math.Round(((infoGeneral.MTBS + infoGeneral.MTTR) == 0 ? 0 : (infoGeneral.MTBS / (infoGeneral.MTBS + infoGeneral.MTTR))) * 100, 2).ToString() + "%");
                o.horasTrabajado = Math.Round(infoGeneral.horasTrabajadas, 2).ToString();
                o.horasParo = Math.Round(horasParo,2).ToString();//Math.Round(infoGeneral.horasParo, 2).ToString();
                o.pMProgramadoTiempo = Math.Round(tipoMantenimiento.tdTPPTiempo, 2) + "%";
                o.pMProgramadoCantidad = Math.Round(tipoMantenimiento.tdTPPCantidad, 2) + "%";
                o.pPreventivoHoras = Math.Round(tipoMantenimiento.tdPTiempo, 2).ToString();
                o.pCorrectivoHoras = Math.Round(tipoMantenimiento.tdCTiempo, 2).ToString();
                o.pPredictivoHoras = Math.Round(tipoMantenimiento.tdPrTiempo, 2).ToString();
                o.horasHombre = Math.Round(infoGeneral.horasHombre, 2).ToString();
                o.MTBS = Math.Round(infoGeneral.MTBS, 2).ToString();
                o.MTTR = Math.Round(infoGeneral.MTTR, 2).ToString();
                o.pUtilizacion = (Math.Round((horasTrabajadas / ((24 * DiasPeriodo) - horasParo)) * 100, 2)) + "%";
                o.parosPrincipal1 = top3Paros.paro1;
                o.parosPrincipal2 = top3Paros.paro2;
                o.parosPrincipal3 = top3Paros.paro3;
                lista.Add(o);
            }
            return lista;
        }

        public IList<KPINoFormatDTO> getKPIGeneralNoFormat(List<string> cc, int tipo, int modelo, DateTime Fechainicio, DateTime FechaFin)
        {
            #region
            List<KPINoFormatDTO> lista = new List<KPINoFormatDTO>();

            //var maquinas = _context.tblM_CatMaquina
            //    .Where(
            //        x => x.centro_costos.Equals(cc) &&
            //             x.estatus != 0 &&
            //             (tipo != 0 ? x.grupoMaquinaria.id == tipo /*&& x.modeloEquipoID==modelo*/: x.grupoMaquinaria.tipoEquipoID == 1)
            //             ).Where(y => (modelo != 0 ? y.modeloEquipoID == modelo : y.id == y.id)).ToList();
            var maquinasRaw = (from or in _context.tblM_CapOrdenTrabajo
                               join m in _context.tblM_CatMaquina on or.EconomicoID equals m.id
                               where cc.Contains(or.CC) && m.grupoMaquinaria.tipoEquipoID == 1 && m.estatus != 0 && (tipo != 0 ? m.grupoMaquinaria.id == tipo : m.id == m.id) &&
                               (modelo != 0 ? m.modeloEquipoID == modelo : m.id == m.id) && (or.FechaEntrada >= Fechainicio && or.FechaEntrada <= FechaFin)
                               select m).GroupBy(x => x).Select(x => x.Key).ToList();
            var noEcos = new List<string>();
            noEcos.AddRange(maquinasRaw.Select(x => x.noEconomico));

            var maquinasNoOC = (from m in _context.tblM_CatMaquina
                                where cc.Contains(m.centro_costos) && m.grupoMaquinaria.tipoEquipoID == 1 && m.estatus != 0 && (tipo != 0 ? m.grupoMaquinaria.id == tipo : m.id == m.id) &&
                                (modelo != 0 ? m.modeloEquipoID == modelo : m.id == m.id) && (!noEcos.Contains(m.noEconomico)) && (FechaFin.Month == DateTime.Now.Month ? true : false)
                                select m).GroupBy(x => x).Select(x => x.Key).ToList();
            var noEcosA = new List<string>();
            noEcosA.AddRange(maquinasNoOC.Select(x => x.noEconomico));
            var maquinasXH = (from m in _context.tblM_CatMaquina
                              join h in _context.tblM_CapHorometro on m.noEconomico equals h.Economico
                              where cc.Contains(h.CC) && m.grupoMaquinaria.tipoEquipoID == 1 && (tipo != 0 ? m.grupoMaquinaria.id == tipo : m.id == m.id) &&
                                (modelo != 0 ? m.modeloEquipoID == modelo : m.id == m.id) && (!noEcos.Contains(m.noEconomico) && !noEcosA.Contains(m.noEconomico)) && (FechaFin.Month < DateTime.Now.Month ? true : false) && (h.Fecha >= Fechainicio && h.Fecha <= FechaFin)
                              select m).GroupBy(x => x).Select(x => x.Key).ToList();
            var maquinas = maquinasRaw.Union(maquinasNoOC).Union(maquinasXH).ToList();
            #endregion

            #region CONSULTAS
            MainContextEnum idEmpresa = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora;

            #region SE OBTIENE CAP. ORDEN DE TRABAJO
            List<tblM_CapOrdenTrabajo> _lstCapOrdenTrabajoDapper = _context.Select<tblM_CapOrdenTrabajo>(new DapperDTO
            {
                baseDatos = idEmpresa,
                consulta = @"SELECT id, EconomicoID, CC, horometro, Turno, FechaCreacion, TipoParo1, TipoParo2, TipoParo3, MotivoParo, FechaEntrada, FechaSalida, 
		                        TiempoTotalParo, TiempoReparacion, TiempoMuerto, DescripcionTiempoMuerto, Comentario, DescripcionMotivo, TiempoHorasTotal, 
		                        TiempoHorasReparacion, TiempoHorasMuerto, TiempoMinutosTotal, TiempoMinutosReparacion, TiempoMinutosMuerto, usuarioCapturaID, TipoOT, EstatusOT, folio 
			                        FROM tblM_CapOrdenTrabajo"
            });
            #endregion

            #region SE OBTIENE LISTADO DE MAQUINAS
            List<tblM_CatMaquina> _lstCatMaquinasDapper = _context.Select<tblM_CatMaquina>(new DapperDTO
            {
                baseDatos = idEmpresa,
                consulta = @"SELECT id, grupoMaquinariaID, noEconomico, modeloEquipoID, anio, placas, noSerie, aseguradoraID, noPoliza, tipoCombustibleID, capacidadTanque, unidadCarga, 
		                        capacidadCarga, horometroAdquisicion, horometroActual, estatus, trackComponenteID, descripcion, renta, marcaID, tipoEncierro, fechaPoliza, 
		                        fechaAdquisicion, proveedor, centro_costos, ComentarioStandBy, TipoCaptura, fechaEntregaSitio, lugarEntregaProveedor, ordenCompra, costoEquipo, 
		                        numArreglo, marcaMotor, modeloMotor, numSerieMotor, arregloCPL, CondicionUso, tipoAdquisicion, fabricacion, numPedimento, CostoRenta, UtilizacionHoras, 
		                        TipoCambio, ProveedorID, TipoBajaID, IdUsuarioBaja, LibreAbordo, fechaBaja, kmBaja, HorometroBaja, EconomicoCC, CargoEmpresa, Comentario, Garantia, 
		                        DepreciacionCapturada, redireccionamientoVenta, empresa, tieneSeguro 
			                        FROM tblM_CatMaquina"
            });
            #endregion

            #region SE OBTIENE LISTADO DE CRITERIOS CAUSA PARO
            List<tblM_CatCriteriosCausaParo> _lstCatCriteriosCausaParoDapper = _context.Select<tblM_CatCriteriosCausaParo>(new DapperDTO
            {
                baseDatos = idEmpresa,
                consulta = @"SELECT id, CausaParo, TiempoMantenimiento, TipoParo, DescripcionParo FROM tblM_CatCriteriosCausaParo"
            });
            #endregion

            #region SE OBTIENE LISTADO DE CRITERIOS CAUSA PARO
            List<tblM_CapHorometro> _lstCapHorometro = _context.Select<tblM_CapHorometro>(new DapperDTO
            {
                baseDatos = idEmpresa,
                consulta = @"SELECT CC, Economico, HorasTrabajo, Horometro, HorometroAcumulado, Desfase, Fecha, Turno, Ritmo, FechaCaptura, folio FROM tblM_CapHorometro"
            });
            #endregion

            #region SE OBTIENE LISTADO DET ORDEN DE TRABAJO
            List<tblM_DetOrdenTrabajo> _lstDetOrdenTrabajo = _context.Select<tblM_DetOrdenTrabajo>(new DapperDTO
            {
                baseDatos = idEmpresa,
                consulta = @"SELECT id, OrdenTrabajoID, PersonalID, HorasTrabajo, HoraInicio, HoraFin, Tipo FROM tblM_DetOrdenTrabajo"
            });
            #endregion

            #endregion

            foreach (var i in maquinas)
            {
                #region INIT VARIABLES
                int ordenesCerradas = 0, ordenesAbiertas = 0;
                #endregion

                if (_lstCapOrdenTrabajoDapper != null)
                {
                    #region v2
                    ordenesCerradas = _lstCapOrdenTrabajoDapper.Where(x => x.EconomicoID == i.id && cc.Contains(x.CC) && (x.FechaEntrada >= Fechainicio && x.FechaEntrada <= FechaFin) && x.FechaSalida != null && x.TipoOT == 0).Count();
                    ordenesAbiertas = _lstCapOrdenTrabajoDapper.Where(x => x.EconomicoID == i.id && cc.Contains(x.CC) && (x.FechaEntrada >= Fechainicio && x.FechaEntrada <= FechaFin) && x.EstatusOT == true && x.TipoOT == 1).OrderBy(x => x.FechaEntrada).Count();
                    #endregion
                }
                else
                {
                    #region v1
                    ordenesCerradas = _context.tblM_CapOrdenTrabajo.Where(x => x.EconomicoID == i.id && cc.Contains(x.CC) && (x.FechaEntrada >= Fechainicio && x.FechaEntrada <= FechaFin) && x.FechaSalida != null && x.TipoOT == 0).Count();
                    ordenesAbiertas = _context.tblM_CapOrdenTrabajo.Where(x => x.EconomicoID == i.id && cc.Contains(x.CC) && (x.FechaEntrada >= Fechainicio && x.FechaEntrada <= FechaFin) && x.EstatusOT == true && x.TipoOT == 1).OrderBy(x => x.FechaEntrada).Count();
                    #endregion   
                }

                if (ordenesCerradas > 0 || ordenesAbiertas > 0) //OMAR
                {
                    var o = new KPINoFormatDTO();

                    var infoGeneral = getInfoGeneral(i.id, cc, Fechainicio, FechaFin, _lstCatMaquinasDapper, _lstCapOrdenTrabajoDapper, _lstCapHorometro, _lstDetOrdenTrabajo, _lstCatCriteriosCausaParoDapper);
                    var top3Paros = getTop3FrecuenciaParo(i.id, cc, Fechainicio, FechaFin, _lstCapOrdenTrabajoDapper, _lstCatCriteriosCausaParoDapper);
                    var tipoMantenimiento = getMDTipoMantenimiento(i.id, cc, Fechainicio, FechaFin, _lstCatMaquinasDapper, _lstCatCriteriosCausaParoDapper, _lstCapOrdenTrabajoDapper);
                    var horasTrabajadas = Math.Round(infoGeneral.horasTrabajadas, 2);
                    //var horasParo = Math.Round(infoGeneral.horasParo, 2);
                    var horasParo = Math.Round(tipoMantenimiento.tdPTiempo, 2) + Math.Round(tipoMantenimiento.tdCTiempo, 2) + Math.Round(tipoMantenimiento.tdPrTiempo, 2);//Math.Round(infoGeneral.horasParo, 2);
                    int DiasPeriodo = Convert.ToInt32((FechaFin - Fechainicio).TotalDays) + 1;

                    o.id = i.id;
                    o.economico = i.noEconomico;
                    o.pDisponibilidad = horasParo == 0 ? 100 : (Math.Round(((infoGeneral.MTBS + infoGeneral.MTTR) == 0 ? 1 : (infoGeneral.MTBS / (infoGeneral.MTBS + infoGeneral.MTTR))) * 100, 2));
                    o.horasTrabajado = Math.Round(infoGeneral.horasTrabajadas, 2);
                    o.horasParo = Math.Round(horasParo, 2);
                    o.pMProgramadoTiempo = Math.Round(tipoMantenimiento.tdTPPTiempo, 2);
                    o.pMProgramadoCantidad = Math.Round(tipoMantenimiento.tdTPPCantidad, 2);
                    o.pPreventivoHoras = Math.Round(tipoMantenimiento.tdPTiempo, 2);
                    o.pCorrectivoHoras = Math.Round(tipoMantenimiento.tdCTiempo, 2);
                    o.pPredictivoHoras = Math.Round(tipoMantenimiento.tdPrTiempo, 2);
                    o.horasHombre = Math.Round(infoGeneral.horasHombre, 2);
                    o.MTBS = Math.Round(infoGeneral.MTBS, 2);
                    o.MTTR = Math.Round(infoGeneral.MTTR, 2);
                    o.pUtilizacion = (Math.Round((horasTrabajadas / ((24 * DiasPeriodo) - horasParo)) * 100, 2));
                    lista.Add(o);
                }
                else
                {
                    var o = new KPINoFormatDTO();

                    var infoGeneral = getInfoGeneral(i.id, cc, Fechainicio, FechaFin, _lstCatMaquinasDapper, _lstCapOrdenTrabajoDapper, _lstCapHorometro);
                    var top3Paros = getTop3FrecuenciaParo(i.id, cc, Fechainicio, FechaFin, _lstCapOrdenTrabajoDapper, _lstCatCriteriosCausaParoDapper);
                    var tipoMantenimiento = getMDTipoMantenimiento(i.id, cc, Fechainicio, FechaFin, _lstCatMaquinasDapper, _lstCatCriteriosCausaParoDapper, _lstCapOrdenTrabajoDapper);
                    var horasTrabajadas = Math.Round(infoGeneral.horasTrabajadas, 2);
                    //var horasParo = Math.Round(infoGeneral.horasParo, 2);
                    var horasParo = Math.Round(tipoMantenimiento.tdPTiempo, 2) + Math.Round(tipoMantenimiento.tdCTiempo, 2) + Math.Round(tipoMantenimiento.tdPrTiempo, 2);//Math.Round(infoGeneral.horasParo, 2);
                    int DiasPeriodo = Convert.ToInt32((FechaFin - Fechainicio).TotalDays) + 1;

                    o.id = i.id;
                    o.economico = i.noEconomico;
                    o.pDisponibilidad = 100;
                    o.horasTrabajado = Math.Round(infoGeneral.horasTrabajadas, 2);
                    o.horasParo = Math.Round(horasParo, 2);
                    o.pMProgramadoTiempo = Math.Round(tipoMantenimiento.tdTPPTiempo, 2);
                    o.pMProgramadoCantidad = Math.Round(tipoMantenimiento.tdTPPCantidad, 2);
                    o.pPreventivoHoras = Math.Round(tipoMantenimiento.tdPTiempo, 2);
                    o.pCorrectivoHoras = Math.Round(tipoMantenimiento.tdCTiempo, 2);
                    o.pPredictivoHoras = Math.Round(tipoMantenimiento.tdPrTiempo, 2);
                    o.horasHombre = Math.Round(infoGeneral.horasHombre, 2);
                    o.MTBS = Math.Round(infoGeneral.MTBS, 2);
                    o.MTTR = Math.Round(infoGeneral.MTTR, 2);
                    o.pUtilizacion = (Math.Round((horasTrabajadas / ((24 * DiasPeriodo) - horasParo)) * 100, 2));
                    lista.Add(o);
                }
            }
            return lista;
        }
        public kpiTipoMantenimientoDTO getMDTipoMantenimiento(int id, List<string> cc, DateTime Fechainicio, DateTime FechaFin, List<tblM_CatMaquina> _lstCatMaquinas = null, List<tblM_CatCriteriosCausaParo> _lstCatCriteriosCausaParo = null, List<tblM_CapOrdenTrabajo> _lstCapOrdenTrabajo = null)
        {
            tblM_CatMaquina maquina = new tblM_CatMaquina();
            if (_lstCatMaquinas != null)
            {
                #region v2
                maquina = _lstCatMaquinas.FirstOrDefault(x => x.id == id);
                #endregion
            }
            else
            {
                #region v1
                maquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == id);
                #endregion   
            }

            List<tblM_CatCriteriosCausaParo> criterios = new List<tblM_CatCriteriosCausaParo>();
            if (_lstCatCriteriosCausaParo != null)
            {
                #region v2
                criterios = _lstCatCriteriosCausaParo.ToList();
                #endregion
            }
            else
            {
                #region v1
                criterios = _context.tblM_CatCriteriosCausaParo.ToList();
                #endregion   
            }

            var p = criterios.Where(x => x.TipoParo.Equals("Programado")).Select(x => x.id);
            var np = criterios.Where(x => x.TipoParo.Equals("No Programado")).Select(x => x.id);

            var prev = criterios.Where(x => x.TiempoMantenimiento.Equals("Preventivo")).Select(x => x.id);
            var corr = criterios.Where(x => x.TiempoMantenimiento.Equals("Correctivo")).Select(x => x.id);
            var pred = criterios.Where(x => x.TiempoMantenimiento.Equals("Predictivo")).Select(x => x.id);

            /* var ordenesP = _context.tblM_CapOrdenTrabajo.Where(x => x.EconomicoID == id && x.CC.Equals(cc) && x.FechaEntrada.Year == anio && x.FechaEntrada.Month == mes && p.Contains(x.MotivoParo) && x.FechaSalida != null).OrderBy(x => x.FechaEntrada).ToList();
             var ordenesNP = _context.tblM_CapOrdenTrabajo.Where(x => x.EconomicoID == id && x.CC.Equals(cc) && x.FechaEntrada.Year == anio && x.FechaEntrada.Month == mes && np.Contains(x.MotivoParo) && x.FechaSalida != null).OrderBy(x => x.FechaEntrada).ToList();
             var ordenesPrev = _context.tblM_CapOrdenTrabajo.Where(x => x.EconomicoID == id && x.CC.Equals(cc) && x.FechaEntrada.Year == anio && x.FechaEntrada.Month == mes && prev.Contains(x.MotivoParo) && x.FechaSalida != null).OrderBy(x => x.FechaEntrada).ToList();
             var ordenesCorr = _context.tblM_CapOrdenTrabajo.Where(x => x.EconomicoID == id && x.CC.Equals(cc) && x.FechaEntrada.Year == anio && x.FechaEntrada.Month == mes && corr.Contains(x.MotivoParo) && x.FechaSalida != null).OrderBy(x => x.FechaEntrada).ToList();
             var ordenesPred = _context.tblM_CapOrdenTrabajo.Where(x => x.EconomicoID == id && x.CC.Equals(cc) && x.FechaEntrada.Year == anio && x.FechaEntrada.Month == mes && pred.Contains(x.MotivoParo) && x.FechaSalida != null).OrderBy(x => x.FechaEntrada).ToList();
             */

            List<tblM_CapOrdenTrabajo> ordenesP = new List<tblM_CapOrdenTrabajo>();
            List<tblM_CapOrdenTrabajo> ordenesNP = new List<tblM_CapOrdenTrabajo>();
            List<tblM_CapOrdenTrabajo> ordenesPrev = new List<tblM_CapOrdenTrabajo>();
            List<tblM_CapOrdenTrabajo> ordenesCorr = new List<tblM_CapOrdenTrabajo>();
            List<tblM_CapOrdenTrabajo> ordenesPred = new List<tblM_CapOrdenTrabajo>();

            if (_lstCapOrdenTrabajo != null)
            {
                #region v2
                ordenesP = _lstCapOrdenTrabajo.Where(x => x.EconomicoID == id && cc.Contains(x.CC) && (x.FechaEntrada >= Fechainicio && x.FechaEntrada <= FechaFin) && p.Contains(x.MotivoParo) && x.FechaSalida != null).OrderBy(x => x.FechaEntrada).ToList();
                ordenesNP = _lstCapOrdenTrabajo.Where(x => x.EconomicoID == id && cc.Contains(x.CC) && (x.FechaEntrada >= Fechainicio && x.FechaEntrada <= FechaFin) && np.Contains(x.MotivoParo) && x.FechaSalida != null).OrderBy(x => x.FechaEntrada).ToList();
                ordenesPrev = _lstCapOrdenTrabajo.Where(x => x.EconomicoID == id && cc.Contains(x.CC) && (x.FechaEntrada >= Fechainicio && x.FechaEntrada <= FechaFin) && prev.Contains(x.MotivoParo) && x.FechaSalida != null).OrderBy(x => x.FechaEntrada).ToList();
                ordenesCorr = _lstCapOrdenTrabajo.Where(x => x.EconomicoID == id && cc.Contains(x.CC) && (x.FechaEntrada >= Fechainicio && x.FechaEntrada <= FechaFin) && corr.Contains(x.MotivoParo) && x.FechaSalida != null).OrderBy(x => x.FechaEntrada).ToList();
                ordenesPred = _lstCapOrdenTrabajo.Where(x => x.EconomicoID == id && cc.Contains(x.CC) && (x.FechaEntrada >= Fechainicio && x.FechaEntrada <= FechaFin) && pred.Contains(x.MotivoParo) && x.FechaSalida != null).OrderBy(x => x.FechaEntrada).ToList();
                #endregion
            }
            else
            {
                #region v1
                ordenesP = _context.tblM_CapOrdenTrabajo.Where(x => x.EconomicoID == id && cc.Contains(x.CC) && (x.FechaEntrada >= Fechainicio && x.FechaEntrada <= FechaFin) && p.Contains(x.MotivoParo) && x.FechaSalida != null).OrderBy(x => x.FechaEntrada).ToList();
                ordenesNP = _context.tblM_CapOrdenTrabajo.Where(x => x.EconomicoID == id && cc.Contains(x.CC) && (x.FechaEntrada >= Fechainicio && x.FechaEntrada <= FechaFin) && np.Contains(x.MotivoParo) && x.FechaSalida != null).OrderBy(x => x.FechaEntrada).ToList();
                ordenesPrev = _context.tblM_CapOrdenTrabajo.Where(x => x.EconomicoID == id && cc.Contains(x.CC) && (x.FechaEntrada >= Fechainicio && x.FechaEntrada <= FechaFin) && prev.Contains(x.MotivoParo) && x.FechaSalida != null).OrderBy(x => x.FechaEntrada).ToList();
                ordenesCorr = _context.tblM_CapOrdenTrabajo.Where(x => x.EconomicoID == id && cc.Contains(x.CC) && (x.FechaEntrada >= Fechainicio && x.FechaEntrada <= FechaFin) && corr.Contains(x.MotivoParo) && x.FechaSalida != null).OrderBy(x => x.FechaEntrada).ToList();
                ordenesPred = _context.tblM_CapOrdenTrabajo.Where(x => x.EconomicoID == id && cc.Contains(x.CC) && (x.FechaEntrada >= Fechainicio && x.FechaEntrada <= FechaFin) && pred.Contains(x.MotivoParo) && x.FechaSalida != null).OrderBy(x => x.FechaEntrada).ToList();
                #endregion   
            }

            // var ordenesProg = _context.tblM_CapOrdenTrabajo.Where(x => x.EconomicoID == id && x.CC.Equals(cc) && (x.FechaEntrada >= Fechainicio && x.OrdenTrabajo.FechaEntrada <= FechaFin) && p.Contains(x.OrdenTrabajo.MotivoParo) && x.OrdenTrabajo.FechaSalida != null).OrderBy(x => x.OrdenTrabajo.FechaEntrada).ToList();
            kpiTipoMantenimientoDTO obj = new kpiTipoMantenimientoDTO();




            obj.tdTPTiempo = ordenesP.Sum(x => SumData(x.FechaSalida, x.FechaEntrada));
            obj.tdTPCantidad = ordenesP.Count();


            obj.tdTNPTiempo = ordenesNP.Sum(x => SumData(x.FechaSalida, x.FechaEntrada));
            obj.tdTNPCantidad = ordenesNP.Count();

            obj.tdPTiempo = ordenesPrev.Sum(x => SumData(x.FechaSalida, x.FechaEntrada));
            obj.tdPCantidad = ordenesPrev.Count();

            obj.tdCTiempo = ordenesCorr.Sum(x => SumData(x.FechaSalida, x.FechaEntrada));
            obj.tdCCantidad = ordenesCorr.Count();

            obj.tdPrTiempo = ordenesPred.Sum(x => SumData(x.FechaSalida, x.FechaEntrada));
            obj.tdPrCantidad = ordenesPred.Count();

            obj.tdTPTotal = obj.tdTPTiempo + obj.tdTNPTiempo;
            obj.tdTPTotal2 = obj.tdTPCantidad + obj.tdTNPCantidad;
            obj.tdPTotal = obj.tdPTiempo + obj.tdCTiempo + obj.tdPrTiempo;
            obj.tdPTotal2 = obj.tdPCantidad + obj.tdCCantidad + obj.tdPrCantidad;

            obj.tdTPPTiempo = obj.tdTPTotal == 0 ? 0 : ((obj.tdTPTiempo / obj.tdTPTotal) * 100);
            obj.tdTPPCantidad = obj.tdTPTotal2 == 0 ? 0 : ((obj.tdTPCantidad / obj.tdTPTotal2) * 100);
            obj.tdTNPPTiempo = obj.tdTPTotal == 0 ? 0 : ((obj.tdTNPTiempo / obj.tdTPTotal) * 100);
            obj.tdTNPPCantidad = obj.tdTPTotal2 == 0 ? 0 : ((obj.tdTNPCantidad / obj.tdTPTotal2) * 100);

            obj.tdPPTiempo = obj.tdPTotal == 0 ? 0 : ((obj.tdPTiempo / obj.tdPTotal) * 100);
            obj.tdPPCantidad = obj.tdPTotal2 == 0 ? 0 : ((obj.tdPCantidad / obj.tdPTotal2) * 100);
            obj.tdCPTiempo = obj.tdPTotal == 0 ? 0 : ((obj.tdCTiempo / obj.tdPTotal) * 100);
            obj.tdCPCantidad = obj.tdPTotal2 == 0 ? 0 : ((obj.tdCCantidad / obj.tdPTotal2) * 100);
            obj.tdPrPTiempo = obj.tdPTotal == 0 ? 0 : ((obj.tdPrTiempo / obj.tdPTotal) * 100);
            obj.tdPrPCantidad = obj.tdPTotal2 == 0 ? 0 : ((obj.tdPrCantidad / obj.tdPTotal2) * 100);
            obj.tdPTotalF = (obj.tdTPCantidad + obj.tdTNPCantidad) == 0 ? 0 : ((obj.tdTPCantidad / (obj.tdTPCantidad + obj.tdTNPCantidad)) * 100);

            return obj;
        }


        private decimal SumData(DateTime? FechaSalida, DateTime fechaEntrada)
        {

            var data = FechaSalida - fechaEntrada;

            var dataReslt = data.Value.TotalHours;

            return (decimal)dataReslt;
        }

        public kpiInfoGeneralDTO getInfoGeneral(int id, List<string> cc, DateTime fechainicio, DateTime fechaFin, List<tblM_CatMaquina> _lstCatMaquinas = null, List<tblM_CapOrdenTrabajo> _lstCapOrdenTrabajo = null, 
                                                    List<tblM_CapHorometro> _lstCapHorometro = null, List<tblM_DetOrdenTrabajo> _lstDetOrdenTrabajo = null, List<tblM_CatCriteriosCausaParo> _lstCatCriteriosCausaParo = null) //OMAR
        {
            tblM_CatMaquina maquina = new tblM_CatMaquina();
            if (_lstCatMaquinas != null)
            {
                #region v2
                maquina = _lstCatMaquinas.FirstOrDefault(f => f.id == id);
                #endregion
            }
            else
            {
                #region v1
                maquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == id);
                #endregion
            }

            List<tblM_CapOrdenTrabajo> ordenes = new List<tblM_CapOrdenTrabajo>();
            List<tblM_CapOrdenTrabajo> ordenesAbiertas = new List<tblM_CapOrdenTrabajo>();
            if (_lstCapOrdenTrabajo != null)
            {
                #region v2
                ordenes = _lstCapOrdenTrabajo.Where(x => x.EconomicoID == id && cc.Contains(x.CC) && (x.FechaEntrada >= fechainicio && x.FechaEntrada <= fechaFin) && x.FechaSalida != null && x.TipoOT == 0).OrderBy(x => x.FechaEntrada).ToList();
                ordenesAbiertas = _lstCapOrdenTrabajo.Where(x => x.EconomicoID == id && cc.Contains(x.CC) && (x.FechaEntrada >= fechainicio && x.FechaEntrada <= fechaFin) && x.TipoOT == 1 && x.EstatusOT == true).ToList();
                #endregion
            }
            else
            {
                #region v1
                ordenes = _context.tblM_CapOrdenTrabajo.Where(x => x.EconomicoID == id && cc.Contains(x.CC) && (x.FechaEntrada >= fechainicio && x.FechaEntrada <= fechaFin) && x.FechaSalida != null && x.TipoOT == 0).OrderBy(x => x.FechaEntrada).ToList();
        ordenesAbiertas = _context.tblM_CapOrdenTrabajo.Where(x => x.EconomicoID == id && cc.Contains(x.CC) && (x.FechaEntrada >= fechainicio && x.FechaEntrada <= fechaFin) && x.TipoOT == 1 && x.EstatusOT == true).ToList();
                #endregion
            }

            List<tblM_CapHorometro> horometrosMes = new List<tblM_CapHorometro>();
            if (_lstCapHorometro != null)
            {
                #region v2
                horometrosMes = _lstCapHorometro.Where(x => x.Economico.Equals(maquina.noEconomico) && (x.Fecha >= fechainicio && x.Fecha <= fechaFin) && cc.Contains(x.CC)).OrderBy(x => x.Fecha).ToList();
                #endregion
            }
            else
            {
                #region v1
                horometrosMes = _context.tblM_CapHorometro.Where(x => x.Economico.Equals(maquina.noEconomico) && (x.Fecha >= fechainicio && x.Fecha <= fechaFin) && cc.Contains(x.CC)).OrderBy(x => x.Fecha).ToList();
                #endregion   
            }

            if (horometrosMes.Count() == 0)
                horometrosMes = _context.tblM_CapHorometro.Where(x => x.Economico.Equals(maquina.noEconomico) && cc.Contains(x.CC)).OrderByDescending(x => x.id).Take(1).ToList();

            ordenes.AddRange(ordenesAbiertas);

            DateTime FechaInicialOTAbierta = new DateTime();
            if (ordenesAbiertas.Count > 0)
                FechaInicialOTAbierta = ordenesAbiertas.FirstOrDefault().FechaEntrada;

            var detResult = new List<kpiMTTOyParoDTO>();
            detResult = getMTTOyParo(id, cc, fechainicio, fechaFin, _lstCatMaquinas, _lstCapOrdenTrabajo, _lstDetOrdenTrabajo, _lstCatCriteriosCausaParo).ToList();
            var result = new kpiInfoGeneralDTO();
            result.noEconomico = maquina.noEconomico;
            if (ordenes.Count == 0)
            {
                result.horometroInicial = 0;
                result.horometroFinal = 0;
            }
            else
            {
                decimal horometroInicial = 0;
                try
                {
                    horometroInicial = horometrosMes.FirstOrDefault().HorometroAcumulado - horometrosMes.FirstOrDefault().HorasTrabajo;
                }
                catch (Exception e)
                {
                    horometroInicial = 0;
                }
                result.horometroInicial = horometroInicial;
                decimal horometroFinal = 0;
                try
                {
                    horometroFinal = horometrosMes.LastOrDefault().HorometroAcumulado;
                }
                catch (Exception e)
                {
                    horometroFinal = 0;
                }
                result.horometroFinal = horometroFinal;
            }

            decimal ultimoHorometro = 0;
            try
            {
                ultimoHorometro = Convert.ToDecimal(detResult.Where(x => x.aplicaCalculo == 2).OrderByDescending(x => x.horometro).FirstOrDefault().horometro);
            }
            catch (Exception e)
            {
                ultimoHorometro = 0;
            }

            var countCalculoMTTR = detResult.Where(x => x.MTBS != 0 && x.aplicaCalculo == 2).ToList().Count;

            if (ultimoHorometro != 0)
            {
                var horometroInicialFInal = horometrosMes.LastOrDefault();
                if (horometroInicialFInal != null)
                {
                    if ((horometrosMes.LastOrDefault().HorometroAcumulado - ultimoHorometro) != 0)
                    {
                        detResult.Add(new kpiMTTOyParoDTO
                        {
                            aplicaCalculo = 2,
                            MTBS = horometrosMes.LastOrDefault().HorometroAcumulado - ultimoHorometro
                        });
                    }
                }
            }

            var countCalculo = detResult.Where(x => x.MTBS != 0 && x.aplicaCalculo == 2).ToList().Count;
            var divisormtbs = detResult.Where(x => x.aplicaCalculo == 2).Sum(x => x.MTBS);
            var divisormttr = countCalculo != 1 ? detResult.Where(x => x.MTBS != 0 && x.aplicaCalculo == 2).Sum(x => x.MTTR) : detResult.Where(x => x.aplicaCalculo == 2).Sum(x => x.MTTR);

            result.MTBS = detResult.Where(x => x.aplicaCalculo == 2).Count() == 0 ? 0 : divisormtbs / (countCalculo == 0 ? 1 : countCalculo);
            result.MTTR = detResult.Where(x => x.aplicaCalculo == 2).Count() == 0 ? 0 : divisormttr / (countCalculoMTTR == 0 ? 1 : countCalculoMTTR);

            result.disponibilidad = (result.MTBS + result.MTTR) == 0 ? ("100%") : (Math.Round(((result.MTBS / (result.MTBS + result.MTTR)) * 100), 2) + "%");

            result.horasHombre = detResult.Sum(x => x.horasHombre);
            var horasParo = (decimal)0;
            foreach (var i in ordenes)
            {

                if (i.TipoOT != 1)
                {
                    var fEntrada = i.FechaEntrada;
                    var fSalida = i.FechaSalida;
                    var intervalo = ((DateTime)i.FechaSalida - i.FechaEntrada);
                    var horas = (decimal)((DateTime)i.FechaSalida - i.FechaEntrada).TotalHours;
                    horasParo += horas;

                    /*Aqui va ir agregada el chequeo contra los tiempos quse generen despues de la fecha,*/
                }
                else
                {
                    var FEntrada = new DateTime();
                    var FSalida = new DateTime();

                    if (i.FechaCreacion >= fechainicio && i.FechaSalida <= fechaFin)
                        FEntrada = i.FechaEntrada;
                    else
                        FEntrada = fechainicio;

                    if (i.EstatusOT) // Si es true quiere decir que la orden esta abierta y vamos a verificar que corresponsa a la fecha de rango.
                    {
                        FSalida = fechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);

                        var horas = (decimal)((DateTime)FSalida - FEntrada).TotalHours;
                        horasParo += horas;
                    }
                    else
                    {
                        FSalida = (DateTime)i.FechaSalida;
                        var horas = (decimal)((DateTime)FSalida - FEntrada).TotalHours;
                        horasParo += horas;
                    }
                }
            }
            //result.horasParo = ordenes.Sum(x => (decimal)((DateTime)x.FechaSalida-x.FechaEntrada).TotalHours);

            result.horasParo = detResult.Where(X => X.MTBS != 0).Sum(x => x.tiempoUtil);//  horasParo;
            result.horasTrabajadas = horometrosMes.Sum(x => x.HorasTrabajo);
            result.ratioMantenimiento = result.horasTrabajadas == 0 ? 0 : (result.horasHombre / result.horasTrabajadas);
            return result;
        }
        public IList<kpiMTTOyParoDTO> getMTTOyParo(int id, List<string> cc, DateTime fechaInicio, DateTime fechaFin, 
                                                    List<tblM_CatMaquina> _lstCatMaquinas = null, List<tblM_CapOrdenTrabajo> _lstCapOrdenTrabajo = null, 
                                                    List<tblM_DetOrdenTrabajo> _lstDetOrdenTrabajo = null, List<tblM_CatCriteriosCausaParo> _lstCatCriteriosCausaParo = null) //OMAR
        {

            var maquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == id);
            var ordenesCerradas = _context.tblM_CapOrdenTrabajo.Where(x => x.EconomicoID == id && cc.Contains(x.CC) && (x.FechaEntrada >= fechaInicio && x.FechaEntrada <= fechaFin) && x.FechaSalida != null && x.TipoOT == 0).OrderBy(x => x.horometro).ToList();
            //  var ordenesAbiertas = _context.tblM_CapOrdenTrabajo.Where(x => x.EconomicoID == id && x.CC.Equals(cc) && x.TipoOT == 1 && x.EstatusOT == true).OrderBy(x => x.FechaEntrada).ToList();
            var ordenesAbiertas = _context.tblM_CapOrdenTrabajo.Where(x => x.EconomicoID == id && cc.Contains(x.CC) && (x.FechaEntrada >= fechaInicio && x.FechaEntrada <= fechaFin) && x.TipoOT == 1 && x.EstatusOT == true).ToList();


            ordenesCerradas.AddRange(ordenesAbiertas);
            var ordenes = ordenesCerradas.OrderBy(x => x.horometro).ThenBy(x => x.FechaCreacion).ToList();
            DateTime FechaInicialOTAbierta = new DateTime();
            if (ordenesAbiertas.Count > 0)
            {
                FechaInicialOTAbierta = ordenesAbiertas.FirstOrDefault().FechaEntrada;
            }
            List<int> idOTL = new List<int>();

            idOTL = ordenes.Select(x => x.id).ToList();
            var ordenDetalle = _context.tblM_DetOrdenTrabajo.Where(x => idOTL.Contains(x.OrdenTrabajoID)).ToList();
            //   var horometrosMes = _context.tblM_CapHorometro.Where(x => x.Economico.Equals(maquina.noEconomico) && (x.Fecha >= fechaInicio && x.Fecha <= fechaFin)).OrderBy(x => x.Fecha);
            List<tblM_CapHorometro> horometrosMes = _context.tblM_CapHorometro.Where(x => x.Economico.Equals(maquina.noEconomico) && (x.Fecha >= fechaInicio && x.Fecha <= fechaFin)).OrderBy(x => x.Fecha).ToList();

            if (horometrosMes.Count() == 0)
            {
                horometrosMes = _context.tblM_CapHorometro.Where(x => x.Economico.Equals(maquina.noEconomico)).OrderByDescending(x => x.id).Take(1).ToList();
            }
            var result = new List<kpiMTTOyParoDTO>();
            decimal horometrosParcial = 0;
            for (int i = 0; i < ordenes.Count; i++)
            {

                var horaSalida = ordenes[i].TipoOT;

                var oID = ordenes[i].id;
                var oMotivoParoID = ordenes[i].MotivoParo;
                var det = _context.tblM_DetOrdenTrabajo.Where(x => x.OrdenTrabajoID == oID).OrderBy(x => x.HoraInicio).ToList();

                if (det.Count > 0)
                {
                    var CausasParo = _context.tblM_CatCriteriosCausaParo.FirstOrDefault(c => c.id == oMotivoParoID);
                    var o = new kpiMTTOyParoDTO();
                    o.fecha = ordenes[i].FechaEntrada.ToShortDateString();
                    o.horometro = ordenes[i].horometro.ToString();
                    o.aplicaCalculo = ordenes[i].TipoParo3;

                    string falla = "";
                    string programado = "";
                    string mantenimiento = "";
                    if (CausasParo != null)
                    {
                        falla = CausasParo.CausaParo;
                        programado = CausasParo.TipoParo;
                        mantenimiento = CausasParo.TiempoMantenimiento;
                        var mm = mantenimiento.Substring(0, 1)[0].ToString();
                        mantenimiento = mm;
                    }

                    o.falla = falla;//CausasParo.CausaParo; // EnumExtensions.GetDescription((MotivoParoOTEnum)ordenes[i].MotivoParo);
                    o.programado = programado;
                    o.tipo = mantenimiento;//; // EnumExtensions.GetDescription((TipoParo2OTEnum)ordenes[i].TipoParo2);

                    var horasUtilH = (decimal)0;
                    var horasTiempoMuerto = (decimal)0;
                    DateTime fEntrada = new DateTime();
                    bool flag = true;

                    if (ordenes[i].TipoOT == 1)
                    {
                        DateTime fInicoMes = new DateTime();
                        if (ordenes[i].FechaEntrada >= fechaInicio)
                        {
                            fInicoMes = ordenes[i].FechaEntrada;
                        }
                        else
                        {
                            fInicoMes = fechaInicio;
                        }

                        if (ordenes[i].EstatusOT)
                        {
                            //   var diasMes = DateTime.DaysInMonth(anio, mes);
                            DateTime FindeMes = fechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);

                            var horas = (decimal)((DateTime)FindeMes - fInicoMes).TotalHours;
                            horasUtilH += horas;
                        }
                        else
                        {
                            DateTime FindeMes = fechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);
                            var horas = (decimal)((DateTime)FindeMes - fInicoMes).TotalHours;
                            horasUtilH += horas;
                        }

                    }
                    else
                    {
                        bool bandera = false;

                        DateTime fInicoMes = new DateTime();
                        fInicoMes = ordenes[i].FechaEntrada;

                        if (ordenes[i].FechaEntrada >= fechaInicio)
                        {
                            fInicoMes = ordenes[i].FechaEntrada;
                        }
                        else
                        {
                            fInicoMes = fechaInicio;
                        }


                        //   var diasMes = DateTime.DaysInMonth(anio, mes);
                        DateTime FindeMes = new DateTime();

                        FindeMes = (DateTime)ordenes[i].FechaSalida;

                        var horas = (decimal)((DateTime)FindeMes - fInicoMes).TotalHours;
                        horasUtilH += horas;
                    }
                    //Horas Util es para sacar las horas hombre.
                    /// horasUtilH
                    o.tiempoUtil = horasUtilH; // ordenes[i].TiempoHorasReparacion;
                    o.tiempoMuerto = horasTiempoMuerto;// ordenes[i].TiempoHorasMuerto;
                    if (o.aplicaCalculo == 2)
                    {
                        if (i == 0)
                        {
                            decimal horometroInicial = 0;
                            try
                            {
                                horometroInicial = horometrosMes.FirstOrDefault().Horometro - horometrosMes.FirstOrDefault().HorasTrabajo;
                            }
                            catch (Exception e)
                            {
                                horometroInicial = 0;
                            }
                            o.MTBS = ordenes[i].horometro - horometroInicial;
                        }
                        else
                        {
                            if (horometrosParcial == 0)
                            {
                                decimal horometroInicial = 0;
                                try
                                {
                                    horometroInicial = horometrosMes.FirstOrDefault().Horometro - horometrosMes.FirstOrDefault().HorasTrabajo;
                                }
                                catch (Exception e)
                                {
                                    horometroInicial = 0;
                                }
                                o.MTBS = ordenes[i].horometro - horometroInicial;

                            }
                            else
                            {
                                o.MTBS = ordenes[i].horometro - horometrosParcial;
                            }
                        }
                        if (o.aplicaCalculo == 2)
                        {
                            horometrosParcial = ordenes[i].horometro;
                        }
                    }
                    else
                    {
                        o.MTBS = 0;
                    }

                    o.tipoEnParo = EnumExtensions.GetDescription((TipoParo3OTEnum)o.aplicaCalculo);
                    o.MTTR = o.tiempoUtil - o.tiempoMuerto;
                    o.personal = det.Count;
                    o.horasHombre = det.ToList().Sum(x => SumData(x.HoraFin, x.HoraInicio));
                    // o.tiempoParo = 
                    result.Add(o);

                }
            }

            return result;
        }

        public kpiFrecuenciaParosDTO getTop3FrecuenciaParo(int id, List<string> cc, DateTime fechainicio, DateTime fechafin, List<tblM_CapOrdenTrabajo> _lstCapOrdenTrabajo = null, List<tblM_CatCriteriosCausaParo> _lstCatCriteriosCausaParoDapper = null)
        {
            List<int> motivosUnicos = new List<int>();
            if (_lstCapOrdenTrabajo != null)
            {
                #region v2
                motivosUnicos = _lstCapOrdenTrabajo.Where(x => x.EconomicoID == id && cc.Contains(x.CC) && (x.FechaEntrada >= fechainicio && x.FechaEntrada <= fechafin) && x.FechaSalida != null).Select(x => x.MotivoParo).ToList();
                #endregion
            }
            else
            {
                #region v1
                motivosUnicos = _context.tblM_CapOrdenTrabajo.Where(x => x.EconomicoID == id && cc.Contains(x.CC) && (x.FechaEntrada >= fechainicio && x.FechaEntrada <= fechafin) && x.FechaSalida != null).Select(x => x.MotivoParo).ToList();
                #endregion   
            }

            List<stTop3Paros> listaInicio = new List<stTop3Paros>();
            foreach (var i in motivosUnicos.Distinct())
            {
                var o = new stTop3Paros();
                if (i != 0)
                {
                    if (_lstCatCriteriosCausaParoDapper != null)
                    {   
                        #region v2
                        o.tipo = _lstCatCriteriosCausaParoDapper.FirstOrDefault(x => x.id == i).CausaParo;
                        o.cantidad = motivosUnicos.Where(x => x == i).Count();
                        listaInicio.Add(o);
                        #endregion
                    }
                    else
                    {
                        #region v1
                        o.tipo = _context.tblM_CatCriteriosCausaParo.FirstOrDefault(x => x.id == i).CausaParo;
                        o.cantidad = motivosUnicos.Where(x => x == i).Count();
                        listaInicio.Add(o);
                        #endregion
                    }
                }
            }
            List<stTop3Paros> listaFin = new List<stTop3Paros>();
            listaFin.AddRange(listaInicio.OrderByDescending(x => x.cantidad));
            var result = new kpiFrecuenciaParosDTO();
            result.paro1 = listaFin.Count >= 1 ? listaFin[0].tipo : "";
            result.paro2 = listaFin.Count >= 2 ? listaFin[1].tipo : "";
            result.paro3 = listaFin.Count >= 3 ? listaFin[2].tipo : "";
            return result;
        }

        public IList<kpiMotivosParoDTO> getkpiMotivosParo(int id, List<string> cc, DateTime fechainicio, DateTime fechafin)
        {
            var maquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == id);
            var ordenes = _context.tblM_CapOrdenTrabajo.Where(x => x.EconomicoID == id && cc.Contains(x.CC) && (x.FechaEntrada >= fechainicio && x.FechaEntrada <= fechafin) && x.FechaSalida != null).OrderBy(x => x.FechaEntrada).ToList();
            List<tblM_CapHorometro> horometrosMes = _context.tblM_CapHorometro.Where(x => x.Economico.Equals(maquina.noEconomico) && (x.Fecha >= fechainicio && x.Fecha <= fechafin)).OrderBy(x => x.Fecha).ToList();

            if (horometrosMes.Count() == 0)
            {
                horometrosMes = _context.tblM_CapHorometro.Where(x => x.Economico.Equals(maquina.noEconomico)).OrderByDescending(x => x.id).Take(1).ToList();
            }

            var result = new List<kpiMotivosParoDTO>();
            var motivos = _context.tblM_CatCriteriosCausaParo.ToList();
            foreach (var i in motivos)
            {
                var det = _context.tblM_DetOrdenTrabajo.Where(x => x.OrdenTrabajoID == i.id).ToList();
                var o = new kpiMotivosParoDTO();
                o.codigo = i.CausaParo;
                o.cantidad = ordenes.Where(x => x.MotivoParo == i.id).Count();

                var ordenesData = ordenes.Where(x => x.MotivoParo == i.id).ToList().Select(y => y.id).ToList();
                var DetalleOrdenes = _context.tblM_DetOrdenTrabajo.Where(x => ordenesData.Contains(x.OrdenTrabajoID)).ToList();

                if (DetalleOrdenes.Count > 0)
                {
                    o.tiempo = DetalleOrdenes.Sum(X => (X.HoraFin - X.HoraInicio).TotalHours).ToString("#.##");
                }
                else
                {
                    o.tiempo = "0.00";
                }
                o.tipo = i.TiempoMantenimiento.Substring(0, 1);
                result.Add(o);
            }
            return result;
        }

        public void Guardar(tblM_KPI obj)
        {
            //if (!Exists(obj))
            //{
            if (obj.id == 0)
                SaveEntity(obj);
            else
                Update(obj, obj.id);
            //}
            //else
            //{
            //    if (obj.id == 0)
            //        throw new Exception("Se encuentra un desfase pendiente por aplicar");
            //    else
            //        Update(obj, obj.id, (int)BitacoraEnum.MAQUINA);
            //}
        }

        public KPIReporteEquipoDTO getKPIReporteEquipo(int id, List<string> cc, DateTime fechaInicio, DateTime FechaFin)
        {

            FechaFin = FechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);
            var result = new KPIReporteEquipoDTO();
            result.kpiInfoGeneral = new kpiInfoGeneralDTO();
            result.kpiTipoMantenimiento = new kpiTipoMantenimientoDTO();
            result.kpiMotivosParo = new List<kpiMotivosParoDTO>();
            result.kpiFrecuenciaParos = new kpiFrecuenciaParosDTO();
            result.kpiMTTOyParo = new List<kpiMTTOyParoDTO>();
            var stringCC = "";
            if (cc.Count() > 1) {
                var areaCuenta = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == cc[0]);
                if (areaCuenta != null) {
                    var division = _context.tblM_KBDivisionDetalle.FirstOrDefault(x => x.acID == areaCuenta.id && x.estatus);
                    if (division != null) 
                    {
                        stringCC = division.division.division;
                    }
                }
            }
            else { stringCC = cc.FirstOrDefault(); }
            result.kpiInfoGeneral = getInfoGeneral(id, cc, fechaInicio, FechaFin);
            result.kpiTipoMantenimiento = getMDTipoMantenimiento(id, cc, fechaInicio, FechaFin);
            result.kpiMotivosParo = getkpiMotivosParo(id, cc, fechaInicio, FechaFin).ToList();
            result.kpiFrecuenciaParos = getTop3FrecuenciaParo(id, cc, fechaInicio, FechaFin);
            result.kpiMTTOyParo = getMTTOyParo(id, cc, fechaInicio, FechaFin).ToList();


            return result;
        }

        public kpiRepMetricasDTO getKPIRepMetricasDTO(List<string> cc, int tipo, int modelo, DateTime fechaInicio, DateTime fechaFin)
        {
            return getKPIRepMetricasDTO2(cc, tipo, modelo, fechaInicio, fechaFin);
            #region Data
            var Enero = new List<KPINoFormatDTO>();
            var Febrero = new List<KPINoFormatDTO>();
            var Marzo = new List<KPINoFormatDTO>();
            var Abril = new List<KPINoFormatDTO>();
            var Mayo = new List<KPINoFormatDTO>();
            var Junio = new List<KPINoFormatDTO>();
            var Julio = new List<KPINoFormatDTO>();
            var Agosto = new List<KPINoFormatDTO>();
            var Septiembre = new List<KPINoFormatDTO>();
            var Octubre = new List<KPINoFormatDTO>();
            var Noviembre = new List<KPINoFormatDTO>();
            var Diciembre = new List<KPINoFormatDTO>();
            if (fechaInicio.Year < DateTime.Now.Year)
            {
                Enero = getKPIGeneralNoFormat(cc, tipo, modelo, DateTime.Parse("01/01/" + fechaInicio.Year), DateTime.Parse(DateTime.DaysInMonth(fechaInicio.Year, 1) + "/01/" + fechaInicio.Year)).ToList();
                Febrero = getKPIGeneralNoFormat(cc, tipo, modelo, DateTime.Parse("01/02/" + fechaInicio.Year), DateTime.Parse(DateTime.DaysInMonth(fechaInicio.Year, 2) + "/02/" + fechaInicio.Year)).ToList();
                Marzo = getKPIGeneralNoFormat(cc, tipo, modelo, DateTime.Parse("01/03/" + fechaInicio.Year), DateTime.Parse(DateTime.DaysInMonth(fechaInicio.Year, 3) + "/03/" + fechaInicio.Year)).ToList();
                Abril = getKPIGeneralNoFormat(cc, tipo, modelo, DateTime.Parse("01/04/" + fechaInicio.Year), DateTime.Parse(DateTime.DaysInMonth(fechaInicio.Year, 4) + "/04/" + fechaInicio.Year)).ToList();
                Mayo = getKPIGeneralNoFormat(cc, tipo, modelo, DateTime.Parse("01/05/" + fechaInicio.Year), DateTime.Parse(DateTime.DaysInMonth(fechaInicio.Year, 5) + "/05/" + fechaInicio.Year)).ToList();
                Junio = getKPIGeneralNoFormat(cc, tipo, modelo, DateTime.Parse("01/06/" + fechaInicio.Year), DateTime.Parse(DateTime.DaysInMonth(fechaInicio.Year, 6) + "/06/" + fechaInicio.Year)).ToList();
                Julio = getKPIGeneralNoFormat(cc, tipo, modelo, DateTime.Parse("01/07/" + fechaInicio.Year), DateTime.Parse(DateTime.DaysInMonth(fechaInicio.Year, 7) + "/07/" + fechaInicio.Year)).ToList();
                Agosto = getKPIGeneralNoFormat(cc, tipo, modelo, DateTime.Parse("01/08/" + fechaInicio.Year), DateTime.Parse(DateTime.DaysInMonth(fechaInicio.Year, 8) + "/08/" + fechaInicio.Year)).ToList();
                Septiembre = getKPIGeneralNoFormat(cc, tipo, modelo, DateTime.Parse("01/09/" + fechaInicio.Year), DateTime.Parse(DateTime.DaysInMonth(fechaInicio.Year, 9) + "/09/" + fechaInicio.Year)).ToList();
                Octubre = getKPIGeneralNoFormat(cc, tipo, modelo, DateTime.Parse("01/10/" + fechaInicio.Year), DateTime.Parse(DateTime.DaysInMonth(fechaInicio.Year, 10) + "/10/" + fechaInicio.Year)).ToList();
                Noviembre = getKPIGeneralNoFormat(cc, tipo, modelo, DateTime.Parse("01/11/" + fechaInicio.Year), DateTime.Parse(DateTime.DaysInMonth(fechaInicio.Year, 11) + "/11/" + fechaInicio.Year)).ToList();
                Diciembre = getKPIGeneralNoFormat(cc, tipo, modelo, DateTime.Parse("01/12/" + fechaInicio.Year), DateTime.Parse(DateTime.DaysInMonth(fechaInicio.Year, 12) + "/12/" + fechaInicio.Year)).ToList();
            }
            else if (fechaInicio.Year == DateTime.Now.Year)
            {



                if (1 <= fechaInicio.Month)
                    Enero = getKPIGeneralNoFormat(cc, tipo, modelo, DateTime.Parse("01/01/" + fechaInicio.Year), DateTime.Parse(DateTime.DaysInMonth(fechaInicio.Year, 1) + "/01/" + fechaInicio.Year)).ToList();

                if (2 <= fechaInicio.Month)
                    Febrero = getKPIGeneralNoFormat(cc, tipo, modelo, DateTime.Parse("01/02/" + fechaInicio.Year), DateTime.Parse(DateTime.DaysInMonth(fechaInicio.Year, 2) + "/02/" + fechaInicio.Year)).ToList();
                if (3 <= fechaInicio.Month)
                    Marzo = getKPIGeneralNoFormat(cc, tipo, modelo, DateTime.Parse("01/03/" + fechaInicio.Year), DateTime.Parse(DateTime.DaysInMonth(fechaInicio.Year, 3) + "/03/" + fechaInicio.Year)).ToList();
                if (4 <= fechaInicio.Month)
                    Abril = getKPIGeneralNoFormat(cc, tipo, modelo, DateTime.Parse("01/04/" + fechaInicio.Year), DateTime.Parse(DateTime.DaysInMonth(fechaInicio.Year, 4) + "/04/" + fechaInicio.Year)).ToList();
                if (5 <= fechaInicio.Month)
                    Mayo = getKPIGeneralNoFormat(cc, tipo, modelo, DateTime.Parse("01/05/" + fechaInicio.Year), DateTime.Parse(DateTime.DaysInMonth(fechaInicio.Year, 5) + "/05/" + fechaInicio.Year)).ToList();
                if (6 <= fechaInicio.Month)
                    Junio = getKPIGeneralNoFormat(cc, tipo, modelo, DateTime.Parse("01/06/" + fechaInicio.Year), DateTime.Parse(DateTime.DaysInMonth(fechaInicio.Year, 6) + "/06/" + fechaInicio.Year)).ToList();
                if (7 <= fechaInicio.Month)
                    Julio = getKPIGeneralNoFormat(cc, tipo, modelo, DateTime.Parse("01/07/" + fechaInicio.Year), DateTime.Parse(DateTime.DaysInMonth(fechaInicio.Year, 7) + "/07/" + fechaInicio.Year)).ToList();
                if (8 <= fechaInicio.Month)
                    Agosto = getKPIGeneralNoFormat(cc, tipo, modelo, DateTime.Parse("01/08/" + fechaInicio.Year), DateTime.Parse(DateTime.DaysInMonth(fechaInicio.Year, 8) + "/08/" + fechaInicio.Year)).ToList();
                if (9 <= fechaInicio.Month)
                    Septiembre = getKPIGeneralNoFormat(cc, tipo, modelo, DateTime.Parse("01/09/" + fechaInicio.Year), DateTime.Parse(DateTime.DaysInMonth(fechaInicio.Year, 9) + "/09/" + fechaInicio.Year)).ToList();
                if (10 <= fechaInicio.Month)
                    Octubre = getKPIGeneralNoFormat(cc, tipo, modelo, DateTime.Parse("01/10/" + fechaInicio.Year), DateTime.Parse(DateTime.DaysInMonth(fechaInicio.Year, 10) + "/10/" + fechaInicio.Year)).ToList();
                if (11 <= fechaInicio.Month)
                    Noviembre = getKPIGeneralNoFormat(cc, tipo, modelo, DateTime.Parse("01/11/" + fechaInicio.Year), DateTime.Parse(DateTime.DaysInMonth(fechaInicio.Year, 11) + "/11/" + fechaInicio.Year)).ToList();
                if (12 <= fechaInicio.Month)
                    Diciembre = getKPIGeneralNoFormat(cc, tipo, modelo, DateTime.Parse("01/12/" + fechaInicio.Year), DateTime.Parse(DateTime.DaysInMonth(fechaInicio.Year, 12) + "/12/" + fechaInicio.Year)).ToList();
            }
            #endregion
            var result = new kpiRepMetricasDTO();
            #region TablaPrincipal
            result.AnualDTO = new List<kpiAnualDTO>();
            string[] conceptos = new string[] { "MTBS (Hrs)", "MTTR (Hrs)", "DISPONIBILIDAD (%)", "PREVENTIVO", "CORRECTIVO", "PREDICTIVO", "RATIO DE MTTO", "MTTO PROGRAMADO (%)" };
            foreach (var i in conceptos)
            {
                var o = new kpiAnualDTO();
                if (i.Equals("MTBS (Hrs)"))
                {
                    o.Concepto = i;
                    o.Enero = (Enero.Count == 0 ? 0 : Enero.Sum(x => x.MTBS) / Enero.Count);
                    o.Febrero = (Febrero.Count == 0 ? 0 : Febrero.Sum(x => x.MTBS) / Febrero.Count);
                    o.Marzo = (Marzo.Count == 0 ? 0 : Marzo.Sum(x => x.MTBS) / Marzo.Count);
                    o.Abril = (Abril.Count == 0 ? 0 : Abril.Sum(x => x.MTBS) / Abril.Count);
                    o.Mayo = (Mayo.Count == 0 ? 0 : Mayo.Sum(x => x.MTBS) / Mayo.Count);
                    o.Junio = (Junio.Count == 0 ? 0 : Junio.Sum(x => x.MTBS) / Junio.Count);
                    o.Julio = (Julio.Count == 0 ? 0 : Julio.Sum(x => x.MTBS) / Julio.Count);
                    o.Agosto = (Agosto.Count == 0 ? 0 : Agosto.Sum(x => x.MTBS) / Agosto.Count);
                    o.Septiembre = (Septiembre.Count == 0 ? 0 : Septiembre.Sum(x => x.MTBS) / Septiembre.Count);
                    o.Octubre = (Octubre.Count == 0 ? 0 : Octubre.Sum(x => x.MTBS) / Octubre.Count);
                    o.Noviembre = (Noviembre.Count == 0 ? 0 : Noviembre.Sum(x => x.MTBS) / Noviembre.Count);
                    o.Diciembre = (Diciembre.Count == 0 ? 0 : Diciembre.Sum(x => x.MTBS) / Diciembre.Count);

                    int divisor = 0;
                    if (o.Enero > 0)
                        divisor += 1;
                    if (o.Febrero > 0)
                        divisor += 1;
                    if (o.Marzo > 0)
                        divisor += 1;
                    if (o.Abril > 0)
                        divisor += 1;
                    if (o.Mayo > 0)
                        divisor += 1;
                    if (o.Junio > 0)
                        divisor += 1;
                    if (o.Julio > 0)
                        divisor += 1;
                    if (o.Agosto > 0)
                        divisor += 1;
                    if (o.Septiembre > 0)
                        divisor += 1;
                    if (o.Octubre > 0)
                        divisor += 1;
                    if (o.Noviembre > 0)
                        divisor += 1;
                    if (o.Diciembre > 0)
                        divisor += 1;

                    divisor = divisor == 0 ? 1 : divisor;

                    if (fechaInicio.Year < DateTime.Now.Year)
                        o.Total = (o.Enero + o.Febrero + o.Marzo + o.Abril + o.Mayo + o.Junio + o.Julio + o.Agosto + o.Septiembre + o.Octubre + o.Noviembre + o.Diciembre) / divisor;
                    else
                    {

                        o.Total = (o.Enero + o.Febrero + o.Marzo + o.Abril + o.Mayo + o.Junio + o.Julio + o.Agosto + o.Septiembre + o.Octubre + o.Noviembre + o.Diciembre) / divisor;
                    }
                }
                else if (i.Equals("MTTR (Hrs)"))
                {
                    o.Concepto = i;
                    o.Enero = (Enero.Count == 0 ? 0 : Enero.Sum(x => x.MTTR) / Enero.Count);
                    o.Febrero = (Febrero.Count == 0 ? 0 : Febrero.Sum(x => x.MTTR) / Febrero.Count);
                    o.Marzo = (Marzo.Count == 0 ? 0 : Marzo.Sum(x => x.MTTR) / Marzo.Count);
                    o.Abril = (Abril.Count == 0 ? 0 : Abril.Sum(x => x.MTTR) / Abril.Count);
                    o.Mayo = (Mayo.Count == 0 ? 0 : Mayo.Sum(x => x.MTTR) / Mayo.Count);
                    o.Junio = (Junio.Count == 0 ? 0 : Junio.Sum(x => x.MTTR) / Junio.Count);
                    o.Julio = (Julio.Count == 0 ? 0 : Julio.Sum(x => x.MTTR) / Julio.Count);
                    o.Agosto = (Agosto.Count == 0 ? 0 : Agosto.Sum(x => x.MTTR) / Agosto.Count);
                    o.Septiembre = (Septiembre.Count == 0 ? 0 : Septiembre.Sum(x => x.MTTR) / Septiembre.Count);
                    o.Octubre = (Octubre.Count == 0 ? 0 : Octubre.Sum(x => x.MTTR) / Octubre.Count);
                    o.Noviembre = (Noviembre.Count == 0 ? 0 : Noviembre.Sum(x => x.MTTR) / Noviembre.Count);
                    o.Diciembre = (Diciembre.Count == 0 ? 0 : Diciembre.Sum(x => x.MTTR) / Diciembre.Count);

                    int divisor = 0;
                    if (o.Enero > 0)
                        divisor += 1;
                    if (o.Febrero > 0)
                        divisor += 1;
                    if (o.Marzo > 0)
                        divisor += 1;
                    if (o.Abril > 0)
                        divisor += 1;
                    if (o.Mayo > 0)
                        divisor += 1;
                    if (o.Junio > 0)
                        divisor += 1;
                    if (o.Julio > 0)
                        divisor += 1;
                    if (o.Agosto > 0)
                        divisor += 1;
                    if (o.Septiembre > 0)
                        divisor += 1;
                    if (o.Octubre > 0)
                        divisor += 1;
                    if (o.Noviembre > 0)
                        divisor += 1;
                    if (o.Diciembre > 0)
                        divisor += 1;

                    divisor = divisor == 0 ? 1 : divisor;

                    if (fechaInicio.Year < DateTime.Now.Year)
                        o.Total = (o.Enero + o.Febrero + o.Marzo + o.Abril + o.Mayo + o.Junio + o.Julio + o.Agosto + o.Septiembre + o.Octubre + o.Noviembre + o.Diciembre) / divisor;
                    else
                    {

                        o.Total = (o.Enero + o.Febrero + o.Marzo + o.Abril + o.Mayo + o.Junio + o.Julio + o.Agosto + o.Septiembre + o.Octubre + o.Noviembre + o.Diciembre) / divisor;
                    }
                }
                else if (i.Equals("DISPONIBILIDAD (%)"))
                {
                    o.Concepto = i;
                    o.Enero = (Enero.Count == 0 ? 0 : Enero.Sum(x => x.pDisponibilidad) / Enero.Count);
                    o.Febrero = (Febrero.Count == 0 ? 0 : Febrero.Sum(x => x.pDisponibilidad) / Febrero.Count);
                    o.Marzo = (Marzo.Count == 0 ? 0 : Marzo.Sum(x => x.pDisponibilidad) / Marzo.Count);
                    o.Abril = (Abril.Count == 0 ? 0 : Abril.Sum(x => x.pDisponibilidad) / Abril.Count);
                    o.Mayo = (Mayo.Count == 0 ? 0 : Mayo.Sum(x => x.pDisponibilidad) / Mayo.Count);
                    o.Junio = (Junio.Count == 0 ? 0 : Junio.Sum(x => x.pDisponibilidad) / Junio.Count);
                    o.Julio = (Julio.Count == 0 ? 0 : Julio.Sum(x => x.pDisponibilidad) / Julio.Count);
                    o.Agosto = (Agosto.Count == 0 ? 0 : Agosto.Sum(x => x.pDisponibilidad) / Agosto.Count);
                    o.Septiembre = (Septiembre.Count == 0 ? 0 : Septiembre.Sum(x => x.pDisponibilidad) / Septiembre.Count);
                    o.Octubre = (Octubre.Count == 0 ? 0 : Octubre.Sum(x => x.pDisponibilidad) / Octubre.Count);
                    o.Noviembre = (Noviembre.Count == 0 ? 0 : Noviembre.Sum(x => x.pDisponibilidad) / Noviembre.Count);
                    o.Diciembre = (Diciembre.Count == 0 ? 0 : Diciembre.Sum(x => x.pDisponibilidad) / Diciembre.Count);

                    int divisor = 0;
                    if (o.Enero > 0)
                        divisor += 1;
                    if (o.Febrero > 0)
                        divisor += 1;
                    if (o.Marzo > 0)
                        divisor += 1;
                    if (o.Abril > 0)
                        divisor += 1;
                    if (o.Mayo > 0)
                        divisor += 1;
                    if (o.Junio > 0)
                        divisor += 1;
                    if (o.Julio > 0)
                        divisor += 1;
                    if (o.Agosto > 0)
                        divisor += 1;
                    if (o.Septiembre > 0)
                        divisor += 1;
                    if (o.Octubre > 0)
                        divisor += 1;
                    if (o.Noviembre > 0)
                        divisor += 1;
                    if (o.Diciembre > 0)
                        divisor += 1;

                    divisor = divisor == 0 ? 1 : divisor;

                    if (fechaInicio.Year < DateTime.Now.Year)
                        o.Total = (o.Enero + o.Febrero + o.Marzo + o.Abril + o.Mayo + o.Junio + o.Julio + o.Agosto + o.Septiembre + o.Octubre + o.Noviembre + o.Diciembre) / divisor;
                    else
                    {

                        o.Total = (o.Enero + o.Febrero + o.Marzo + o.Abril + o.Mayo + o.Junio + o.Julio + o.Agosto + o.Septiembre + o.Octubre + o.Noviembre + o.Diciembre) / divisor;
                    }
                }
                else if (i.Equals("PREVENTIVO"))
                {
                    o.Concepto = i;
                    o.Enero = Enero.Sum(x => x.pPreventivoHoras);
                    o.Febrero = Febrero.Sum(x => x.pPreventivoHoras);
                    o.Marzo = Marzo.Sum(x => x.pPreventivoHoras);
                    o.Abril = Abril.Sum(x => x.pPreventivoHoras);
                    o.Mayo = Mayo.Sum(x => x.pPreventivoHoras);
                    o.Junio = Junio.Sum(x => x.pPreventivoHoras);
                    o.Julio = Julio.Sum(x => x.pPreventivoHoras);
                    o.Agosto = Agosto.Sum(x => x.pPreventivoHoras);
                    o.Septiembre = Septiembre.Sum(x => x.pPreventivoHoras);
                    o.Octubre = Octubre.Sum(x => x.pPreventivoHoras);
                    o.Noviembre = Noviembre.Sum(x => x.pPreventivoHoras);
                    o.Diciembre = Diciembre.Sum(x => x.pPreventivoHoras);

                    int divisor = 0;
                    if (o.Enero > 0)
                        divisor += 1;
                    if (o.Febrero > 0)
                        divisor += 1;
                    if (o.Marzo > 0)
                        divisor += 1;
                    if (o.Abril > 0)
                        divisor += 1;
                    if (o.Mayo > 0)
                        divisor += 1;
                    if (o.Junio > 0)
                        divisor += 1;
                    if (o.Julio > 0)
                        divisor += 1;
                    if (o.Agosto > 0)
                        divisor += 1;
                    if (o.Septiembre > 0)
                        divisor += 1;
                    if (o.Octubre > 0)
                        divisor += 1;
                    if (o.Noviembre > 0)
                        divisor += 1;
                    if (o.Diciembre > 0)
                        divisor += 1;

                    divisor = divisor == 0 ? 1 : divisor;


                    if (fechaInicio.Year < DateTime.Now.Year)
                        o.Total = (o.Enero + o.Febrero + o.Marzo + o.Abril + o.Mayo + o.Junio + o.Julio + o.Agosto + o.Septiembre + o.Octubre + o.Noviembre + o.Diciembre);
                    else
                    {

                        o.Total = (o.Enero + o.Febrero + o.Marzo + o.Abril + o.Mayo + o.Junio + o.Julio + o.Agosto + o.Septiembre + o.Octubre + o.Noviembre + o.Diciembre);
                    }
                }
                else if (i.Equals("CORRECTIVO"))
                {
                    o.Concepto = i;
                    o.Enero = Enero.Sum(x => x.pCorrectivoHoras);
                    o.Febrero = Febrero.Sum(x => x.pCorrectivoHoras);
                    o.Marzo = Marzo.Sum(x => x.pCorrectivoHoras);
                    o.Abril = Abril.Sum(x => x.pCorrectivoHoras);
                    o.Mayo = Mayo.Sum(x => x.pCorrectivoHoras);
                    o.Junio = Junio.Sum(x => x.pCorrectivoHoras);
                    o.Julio = Julio.Sum(x => x.pCorrectivoHoras);
                    o.Agosto = Agosto.Sum(x => x.pCorrectivoHoras);
                    o.Septiembre = Septiembre.Sum(x => x.pCorrectivoHoras);
                    o.Octubre = Octubre.Sum(x => x.pCorrectivoHoras);
                    o.Noviembre = Noviembre.Sum(x => x.pCorrectivoHoras);
                    o.Diciembre = Diciembre.Sum(x => x.pCorrectivoHoras);
                    if (fechaInicio.Year < DateTime.Now.Year)
                        o.Total = (o.Enero + o.Febrero + o.Marzo + o.Abril + o.Mayo + o.Junio + o.Julio + o.Agosto + o.Septiembre + o.Octubre + o.Noviembre + o.Diciembre);
                    else
                    {

                        o.Total = (o.Enero + o.Febrero + o.Marzo + o.Abril + o.Mayo + o.Junio + o.Julio + o.Agosto + o.Septiembre + o.Octubre + o.Noviembre + o.Diciembre);
                    }
                }
                else if (i.Equals("PREDICTIVO"))
                {
                    o.Concepto = i;
                    o.Enero = Enero.Sum(x => x.pPredictivoHoras);
                    o.Febrero = Febrero.Sum(x => x.pPredictivoHoras);
                    o.Marzo = Marzo.Sum(x => x.pPredictivoHoras);
                    o.Abril = Abril.Sum(x => x.pPredictivoHoras);
                    o.Mayo = Mayo.Sum(x => x.pPredictivoHoras);
                    o.Junio = Junio.Sum(x => x.pPredictivoHoras);
                    o.Julio = Julio.Sum(x => x.pPredictivoHoras);
                    o.Agosto = Agosto.Sum(x => x.pPredictivoHoras);
                    o.Septiembre = Septiembre.Sum(x => x.pPredictivoHoras);
                    o.Octubre = Octubre.Sum(x => x.pPredictivoHoras);
                    o.Noviembre = Noviembre.Sum(x => x.pPredictivoHoras);
                    o.Diciembre = Diciembre.Sum(x => x.pPredictivoHoras);

                    if (fechaInicio.Year < DateTime.Now.Year)
                        o.Total = (o.Enero + o.Febrero + o.Marzo + o.Abril + o.Mayo + o.Junio + o.Julio + o.Agosto + o.Septiembre + o.Octubre + o.Noviembre + o.Diciembre);
                    else
                    {

                        o.Total = (o.Enero + o.Febrero + o.Marzo + o.Abril + o.Mayo + o.Junio + o.Julio + o.Agosto + o.Septiembre + o.Octubre + o.Noviembre + o.Diciembre);
                    }
                }
                else if (i.Equals("RATIO DE MTTO"))
                {
                    o.Concepto = i;
                    o.Enero = Enero.Sum(x => x.horasTrabajado) == 0 ? 0 : Enero.Sum(x => x.horasHombre) / Enero.Sum(x => x.horasTrabajado);
                    o.Febrero = Febrero.Sum(x => x.horasTrabajado) == 0 ? 0 : Febrero.Sum(x => x.horasHombre) / Febrero.Sum(x => x.horasTrabajado);
                    o.Marzo = Marzo.Sum(x => x.horasTrabajado) == 0 ? 0 : Marzo.Sum(x => x.horasHombre) / Marzo.Sum(x => x.horasTrabajado);
                    o.Abril = Abril.Sum(x => x.horasTrabajado) == 0 ? 0 : Abril.Sum(x => x.horasHombre) / Abril.Sum(x => x.horasTrabajado);
                    o.Mayo = Mayo.Sum(x => x.horasTrabajado) == 0 ? 0 : Mayo.Sum(x => x.horasHombre) / Mayo.Sum(x => x.horasTrabajado);

                    o.Junio = Junio.Sum(x => x.horasTrabajado) == 0 ? 0 : Junio.Sum(x => x.horasHombre) / Junio.Sum(x => x.horasTrabajado);
                    o.Julio = Julio.Sum(x => x.horasTrabajado) == 0 ? 0 : Julio.Sum(x => x.horasHombre) / Julio.Sum(x => x.horasTrabajado);
                    o.Agosto = Agosto.Sum(x => x.horasTrabajado) == 0 ? 0 : Agosto.Sum(x => x.horasHombre) / Agosto.Sum(x => x.horasTrabajado);
                    o.Septiembre = Septiembre.Sum(x => x.horasTrabajado) == 0 ? 0 : Septiembre.Sum(x => x.horasHombre) / Septiembre.Sum(x => x.horasTrabajado);
                    o.Octubre = Octubre.Sum(x => x.horasTrabajado) == 0 ? 0 : Octubre.Sum(x => x.horasHombre) / Octubre.Sum(x => x.horasTrabajado);
                    o.Noviembre = Noviembre.Sum(x => x.horasTrabajado) == 0 ? 0 : Noviembre.Sum(x => x.horasHombre) / Noviembre.Sum(x => x.horasTrabajado);
                    o.Diciembre = Diciembre.Sum(x => x.horasTrabajado) == 0 ? 0 : Diciembre.Sum(x => x.horasHombre) / Diciembre.Sum(x => x.horasTrabajado);

                    int divisor = 0;
                    if (o.Enero > 0)
                        divisor += 1;
                    if (o.Febrero > 0)
                        divisor += 1;
                    if (o.Marzo > 0)
                        divisor += 1;
                    if (o.Abril > 0)
                        divisor += 1;
                    if (o.Mayo > 0)
                        divisor += 1;
                    if (o.Junio > 0)
                        divisor += 1;
                    if (o.Julio > 0)
                        divisor += 1;
                    if (o.Agosto > 0)
                        divisor += 1;
                    if (o.Septiembre > 0)
                        divisor += 1;
                    if (o.Octubre > 0)
                        divisor += 1;
                    if (o.Noviembre > 0)
                        divisor += 1;
                    if (o.Diciembre > 0)
                        divisor += 1;

                    divisor = divisor == 0 ? 1 : divisor;

                    if (fechaInicio.Year < DateTime.Now.Year)
                        o.Total = (o.Enero + o.Febrero + o.Marzo + o.Abril + o.Mayo + o.Junio + o.Julio + o.Agosto + o.Septiembre + o.Octubre + o.Noviembre + o.Diciembre) / divisor;
                    else
                    {

                        o.Total = (o.Enero + o.Febrero + o.Marzo + o.Abril + o.Mayo + o.Junio + o.Julio + o.Agosto + o.Septiembre + o.Octubre + o.Noviembre + o.Diciembre) / divisor;
                    }
                }
                else if (i.Equals("MTTO PROGRAMADO (%)"))
                {

                    o.Concepto = i;
                    o.Enero = (Enero.Count == 0 ? 0 : Enero.Sum(x => x.pMProgramadoTiempo) / Enero.Count);
                    o.Febrero = (Febrero.Count == 0 ? 0 : Febrero.Sum(x => x.pMProgramadoTiempo) / Febrero.Count);
                    o.Marzo = (Marzo.Count == 0 ? 0 : Marzo.Sum(x => x.pMProgramadoTiempo) / Marzo.Count);
                    o.Abril = (Abril.Count == 0 ? 0 : Abril.Sum(x => x.pMProgramadoTiempo) / Abril.Count);
                    o.Mayo = (Mayo.Count == 0 ? 0 : Mayo.Sum(x => x.pMProgramadoTiempo) / Mayo.Count);
                    o.Junio = (Junio.Count == 0 ? 0 : Junio.Sum(x => x.pMProgramadoTiempo) / Junio.Count);
                    o.Julio = (Julio.Count == 0 ? 0 : Julio.Sum(x => x.pMProgramadoTiempo) / Julio.Count);
                    o.Agosto = (Agosto.Count == 0 ? 0 : Agosto.Sum(x => x.pMProgramadoTiempo) / Agosto.Count);
                    o.Septiembre = (Septiembre.Count == 0 ? 0 : Septiembre.Sum(x => x.pMProgramadoTiempo) / Septiembre.Count);
                    o.Octubre = (Octubre.Count == 0 ? 0 : Octubre.Sum(x => x.pMProgramadoTiempo) / Octubre.Count);
                    o.Noviembre = (Noviembre.Count == 0 ? 0 : Noviembre.Sum(x => x.pMProgramadoTiempo) / Noviembre.Count);
                    o.Diciembre = (Diciembre.Count == 0 ? 0 : Diciembre.Sum(x => x.pMProgramadoTiempo) / Diciembre.Count);

                    int divisor = 0;
                    if (o.Enero > 0)
                        divisor += 1;
                    if (o.Febrero > 0)
                        divisor += 1;
                    if (o.Marzo > 0)
                        divisor += 1;
                    if (o.Abril > 0)
                        divisor += 1;
                    if (o.Mayo > 0)
                        divisor += 1;
                    if (o.Junio > 0)
                        divisor += 1;
                    if (o.Julio > 0)
                        divisor += 1;
                    if (o.Agosto > 0)
                        divisor += 1;
                    if (o.Septiembre > 0)
                        divisor += 1;
                    if (o.Octubre > 0)
                        divisor += 1;
                    if (o.Noviembre > 0)
                        divisor += 1;
                    if (o.Diciembre > 0)
                        divisor += 1;

                    divisor = divisor == 0 ? 1 : divisor;

                    if (fechaInicio.Year < DateTime.Now.Year)
                        o.Total = (o.Enero + o.Febrero + o.Marzo + o.Abril + o.Mayo + o.Junio + o.Julio + o.Agosto + o.Septiembre + o.Octubre + o.Noviembre + o.Diciembre) / divisor;
                    else
                    {

                        o.Total = (o.Enero + o.Febrero + o.Marzo + o.Abril + o.Mayo + o.Junio + o.Julio + o.Agosto + o.Septiembre + o.Octubre + o.Noviembre + o.Diciembre) / divisor;
                    }
                }
                result.AnualDTO.Add(o);
            }
            #endregion
            var TempMTBS = result.AnualDTO.FirstOrDefault(x => x.Concepto.Equals("MTBS (Hrs)"));
            var TempMTTR = result.AnualDTO.FirstOrDefault(x => x.Concepto.Equals("MTTR (Hrs)"));
            var TempDISPONIBILIDAD = result.AnualDTO.FirstOrDefault(x => x.Concepto.Equals("DISPONIBILIDAD (%)"));
            var TempPREVENTIVO = result.AnualDTO.FirstOrDefault(x => x.Concepto.Equals("PREVENTIVO"));
            var TempCORRECTIVO = result.AnualDTO.FirstOrDefault(x => x.Concepto.Equals("CORRECTIVO"));
            var TempPREDICTIVO = result.AnualDTO.FirstOrDefault(x => x.Concepto.Equals("PREDICTIVO"));
            var TempRATIO_MTTO = result.AnualDTO.FirstOrDefault(x => x.Concepto.Equals("RATIO DE MTTO"));
            var TempMTTO_PROGRAMADO = result.AnualDTO.FirstOrDefault(x => x.Concepto.Equals("MTTO PROGRAMADO (%)"));
            #region GraficaTiempos_Paro
            result.kpiMTGraficaTiemposParo = new List<kpiMTGraficaTiemposParoDTO>();

            result.kpiMTGraficaTiemposParo.Add(new kpiMTGraficaTiemposParoDTO
            {
                CONCEPTO = "ENE",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Enero,
                MTBS = TempMTBS.Enero,
                MTTR = TempMTTR.Enero,
                RATIO_MTTO = TempRATIO_MTTO.Enero
            });//Enero
            result.kpiMTGraficaTiemposParo.Add(new kpiMTGraficaTiemposParoDTO
            {
                CONCEPTO = "FEB",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Febrero,
                MTBS = TempMTBS.Febrero,
                MTTR = TempMTTR.Febrero,
                RATIO_MTTO = TempRATIO_MTTO.Febrero
            });//Febrero
            result.kpiMTGraficaTiemposParo.Add(new kpiMTGraficaTiemposParoDTO
            {
                CONCEPTO = "MAR",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Marzo,
                MTBS = TempMTBS.Marzo,
                MTTR = TempMTTR.Marzo,
                RATIO_MTTO = TempRATIO_MTTO.Marzo
            });//Marzo
            result.kpiMTGraficaTiemposParo.Add(new kpiMTGraficaTiemposParoDTO
            {
                CONCEPTO = "ABR",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Abril,
                MTBS = TempMTBS.Abril,
                MTTR = TempMTTR.Abril,
                RATIO_MTTO = TempRATIO_MTTO.Abril
            });//Abril
            result.kpiMTGraficaTiemposParo.Add(new kpiMTGraficaTiemposParoDTO
            {
                CONCEPTO = "MAY",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Mayo,
                MTBS = TempMTBS.Mayo,
                MTTR = TempMTTR.Mayo,
                RATIO_MTTO = TempRATIO_MTTO.Mayo
            });//Mayo
            result.kpiMTGraficaTiemposParo.Add(new kpiMTGraficaTiemposParoDTO
            {
                CONCEPTO = "JUN",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Junio,
                MTBS = TempMTBS.Junio,
                MTTR = TempMTTR.Junio,
                RATIO_MTTO = TempRATIO_MTTO.Junio
            });//Junio
            result.kpiMTGraficaTiemposParo.Add(new kpiMTGraficaTiemposParoDTO
            {
                CONCEPTO = "JUL",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Julio,
                MTBS = TempMTBS.Julio,
                MTTR = TempMTTR.Julio,
                RATIO_MTTO = TempRATIO_MTTO.Julio
            });//Julio
            result.kpiMTGraficaTiemposParo.Add(new kpiMTGraficaTiemposParoDTO
            {
                CONCEPTO = "AGO",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Agosto,
                MTBS = TempMTBS.Agosto,
                MTTR = TempMTTR.Agosto,
                RATIO_MTTO = TempRATIO_MTTO.Agosto
            });//Agosto
            result.kpiMTGraficaTiemposParo.Add(new kpiMTGraficaTiemposParoDTO
            {
                CONCEPTO = "SEP",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Septiembre,
                MTBS = TempMTBS.Septiembre,
                MTTR = TempMTTR.Septiembre,
                RATIO_MTTO = TempRATIO_MTTO.Septiembre
            });//Septiembre
            result.kpiMTGraficaTiemposParo.Add(new kpiMTGraficaTiemposParoDTO
            {
                CONCEPTO = "OCT",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Octubre,
                MTBS = TempMTBS.Octubre,
                MTTR = TempMTTR.Octubre,
                RATIO_MTTO = TempRATIO_MTTO.Octubre
            });//Octubre
            result.kpiMTGraficaTiemposParo.Add(new kpiMTGraficaTiemposParoDTO
            {
                CONCEPTO = "NOV",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Noviembre,
                MTBS = TempMTBS.Noviembre,
                MTTR = TempMTTR.Noviembre,
                RATIO_MTTO = TempRATIO_MTTO.Noviembre
            });//Noviembre
            result.kpiMTGraficaTiemposParo.Add(new kpiMTGraficaTiemposParoDTO
            {
                CONCEPTO = "DIC",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Diciembre,
                MTBS = TempMTBS.Diciembre,
                MTTR = TempMTTR.Diciembre,
                RATIO_MTTO = TempRATIO_MTTO.Diciembre
            });//Diciembre
            #endregion
            #region GraficaPorcentaje_Disponibildiad
            result.kpiMTGraficaDisponibilidad = new List<kpiMTGraficaDisponibilidadDTO>();
            result.kpiMTGraficaDisponibilidad.Add(new kpiMTGraficaDisponibilidadDTO
            {
                CONCEPTO = "ENE",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Enero
            });//Enero
            result.kpiMTGraficaDisponibilidad.Add(new kpiMTGraficaDisponibilidadDTO
            {
                CONCEPTO = "FEB",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Febrero
            });//Febrero
            result.kpiMTGraficaDisponibilidad.Add(new kpiMTGraficaDisponibilidadDTO
            {
                CONCEPTO = "MAR",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Marzo
            });//Marzo
            result.kpiMTGraficaDisponibilidad.Add(new kpiMTGraficaDisponibilidadDTO
            {
                CONCEPTO = "ABR",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Abril
            });//Abril
            result.kpiMTGraficaDisponibilidad.Add(new kpiMTGraficaDisponibilidadDTO
            {
                CONCEPTO = "MAY",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Mayo
            });//Mayo
            result.kpiMTGraficaDisponibilidad.Add(new kpiMTGraficaDisponibilidadDTO
            {
                CONCEPTO = "JUN",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Junio
            });//Junio
            result.kpiMTGraficaDisponibilidad.Add(new kpiMTGraficaDisponibilidadDTO
            {
                CONCEPTO = "JUL",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Julio
            });//Julio
            result.kpiMTGraficaDisponibilidad.Add(new kpiMTGraficaDisponibilidadDTO
            {
                CONCEPTO = "AGO",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Agosto
            });//Agosto
            result.kpiMTGraficaDisponibilidad.Add(new kpiMTGraficaDisponibilidadDTO
            {
                CONCEPTO = "SEP",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Septiembre
            });//Septiembre
            result.kpiMTGraficaDisponibilidad.Add(new kpiMTGraficaDisponibilidadDTO
            {
                CONCEPTO = "OCT",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Octubre
            });//Octubre
            result.kpiMTGraficaDisponibilidad.Add(new kpiMTGraficaDisponibilidadDTO
            {
                CONCEPTO = "NOV",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Noviembre
            });//Noviembre
            result.kpiMTGraficaDisponibilidad.Add(new kpiMTGraficaDisponibilidadDTO
            {
                CONCEPTO = "DIC",
                DISPONIBILIDAD = TempDISPONIBILIDAD.Diciembre
            });//Diciembre
            #endregion
            #region GraficaTendencia_TiposMTTO
            result.kpiMTGraficaTendenciaMTTO = new List<kpiMTGraficaTendenciaMTTODTO>();
            result.kpiMTGraficaTendenciaMTTO.Add(new kpiMTGraficaTendenciaMTTODTO
            {
                CONCEPTO = "ENE",
                PREVENTIVO = TempPREVENTIVO.Enero,
                CORRECTIVO = TempCORRECTIVO.Enero,
                PREDICTIVO = TempPREDICTIVO.Enero
            });//Enero
            result.kpiMTGraficaTendenciaMTTO.Add(new kpiMTGraficaTendenciaMTTODTO
            {
                CONCEPTO = "FEB",
                PREVENTIVO = TempPREVENTIVO.Febrero,
                CORRECTIVO = TempCORRECTIVO.Febrero,
                PREDICTIVO = TempPREDICTIVO.Febrero
            });//Febrero
            result.kpiMTGraficaTendenciaMTTO.Add(new kpiMTGraficaTendenciaMTTODTO
            {
                CONCEPTO = "MAR",
                PREVENTIVO = TempPREVENTIVO.Marzo,
                CORRECTIVO = TempCORRECTIVO.Marzo,
                PREDICTIVO = TempPREDICTIVO.Marzo
            });//Marzo
            result.kpiMTGraficaTendenciaMTTO.Add(new kpiMTGraficaTendenciaMTTODTO
            {
                CONCEPTO = "ABR",
                PREVENTIVO = TempPREVENTIVO.Abril,
                CORRECTIVO = TempCORRECTIVO.Abril,
                PREDICTIVO = TempPREDICTIVO.Abril
            });//Abril
            result.kpiMTGraficaTendenciaMTTO.Add(new kpiMTGraficaTendenciaMTTODTO
            {
                CONCEPTO = "MAY",
                PREVENTIVO = TempPREVENTIVO.Mayo,
                CORRECTIVO = TempCORRECTIVO.Mayo,
                PREDICTIVO = TempPREDICTIVO.Mayo
            });//Mayo
            result.kpiMTGraficaTendenciaMTTO.Add(new kpiMTGraficaTendenciaMTTODTO
            {
                CONCEPTO = "JUN",
                PREVENTIVO = TempPREVENTIVO.Junio,
                CORRECTIVO = TempCORRECTIVO.Junio,
                PREDICTIVO = TempPREDICTIVO.Junio
            });//Junio
            result.kpiMTGraficaTendenciaMTTO.Add(new kpiMTGraficaTendenciaMTTODTO
            {
                CONCEPTO = "JUL",
                PREVENTIVO = TempPREVENTIVO.Julio,
                CORRECTIVO = TempCORRECTIVO.Julio,
                PREDICTIVO = TempPREDICTIVO.Julio
            });//Julio
            result.kpiMTGraficaTendenciaMTTO.Add(new kpiMTGraficaTendenciaMTTODTO
            {
                CONCEPTO = "AGO",
                PREVENTIVO = TempPREVENTIVO.Agosto,
                CORRECTIVO = TempCORRECTIVO.Agosto,
                PREDICTIVO = TempPREDICTIVO.Agosto
            });//Agosto
            result.kpiMTGraficaTendenciaMTTO.Add(new kpiMTGraficaTendenciaMTTODTO
            {
                CONCEPTO = "SEP",
                PREVENTIVO = TempPREVENTIVO.Septiembre,
                CORRECTIVO = TempCORRECTIVO.Septiembre,
                PREDICTIVO = TempPREDICTIVO.Septiembre
            });//Septiembre
            result.kpiMTGraficaTendenciaMTTO.Add(new kpiMTGraficaTendenciaMTTODTO
            {
                CONCEPTO = "OCT",
                PREVENTIVO = TempPREVENTIVO.Octubre,
                CORRECTIVO = TempCORRECTIVO.Octubre,
                PREDICTIVO = TempPREDICTIVO.Octubre
            });//Octubre
            result.kpiMTGraficaTendenciaMTTO.Add(new kpiMTGraficaTendenciaMTTODTO
            {
                CONCEPTO = "NOV",
                PREVENTIVO = TempPREVENTIVO.Noviembre,
                CORRECTIVO = TempCORRECTIVO.Noviembre,
                PREDICTIVO = TempPREDICTIVO.Noviembre
            });//Noviembre
            result.kpiMTGraficaTendenciaMTTO.Add(new kpiMTGraficaTendenciaMTTODTO
            {
                CONCEPTO = "DIC",
                PREVENTIVO = TempPREVENTIVO.Diciembre,
                CORRECTIVO = TempCORRECTIVO.Diciembre,
                PREDICTIVO = TempPREDICTIVO.Diciembre
            });//Diciembre
            #endregion
            #region GraficaAnual_TiposMTTO
            result.kpiMTGraficaTiposMTTO = new List<kpiMTGraficaTiposMTTODTO>();
            result.kpiMTGraficaTiposMTTO.Add(new kpiMTGraficaTiposMTTODTO
            {
                CONCEPTO = "PREVENTIVO",
                CANTIDAD = TempPREVENTIVO.Total
            });//Preventivo
            result.kpiMTGraficaTiposMTTO.Add(new kpiMTGraficaTiposMTTODTO
            {
                CONCEPTO = "CORRECTIVO",
                CANTIDAD = TempCORRECTIVO.Total
            });//Correctivo
            result.kpiMTGraficaTiposMTTO.Add(new kpiMTGraficaTiposMTTODTO
            {
                CONCEPTO = "PREDICTIVO",
                CANTIDAD = TempPREDICTIVO.Total
            });//Predictivo
            #endregion
            return result;
        }

        public kpiRepGraficas getKPIRepGraficas(List<string> cc, int tipo, int modelo, DateTime fechaInicio, DateTime fechaFin, List<KPIDTO> data = null)
        {
            return getKPIRepGraficas2(cc, tipo, modelo, fechaInicio, fechaFin, data);

            var result = new kpiRepGraficas();
            result.GraficaFamiliasDTO = new List<kpiGraficaFamiliasDTO>();
            result.MotivosParoDTO = new List<kpiMotivosParoDTO>();
            var maquinas = _context.tblM_CatMaquina
                .Where(
                    x => cc.Contains(x.centro_costos) &&
                        //x.estatus == 1 && 
                         (tipo != 0 ? x.grupoMaquinaria.id == tipo /*&& x.modeloEquipoID==modelo*/: x.grupoMaquinaria.tipoEquipoID == 1)
                      ).ToList();
            var maquinasID = maquinas.Select(x => x.id);
            var grupo = maquinas.Select(x => x.grupoMaquinaria.descripcion).Distinct().ToList();
            var motivos = _context.tblM_CatCriteriosCausaParo.Select(x => x.TiempoMantenimiento).Distinct().ToList();

            foreach (var i in grupo)
            {
                var o = new kpiGraficaFamiliasDTO();
                o.Concepto = i;
                var equipos = maquinas.Where(x => x.grupoMaquinaria.descripcion.Equals(i));

                decimal disponibilidad = 0;
                foreach (var j in equipos)
                {
                    var infoGeneral = getInfoGeneral(j.id, cc, fechaInicio, fechaFin);
                    disponibilidad += infoGeneral.horasParo == 0 ? 100 : (Math.Round(((infoGeneral.MTBS + infoGeneral.MTTR) == 0 ? 0 : (infoGeneral.MTBS / (infoGeneral.MTBS + infoGeneral.MTTR))) * 100, 2));
                }
                var valor = (disponibilidad * 100) / (equipos.Count() * 100);
                o.Valor = Convert.ToInt32(Math.Round(valor, 0));
                result.GraficaFamiliasDTO.Add(o);
            }
            foreach (var i in motivos)
            {
                var o = new kpiMotivosParoDTO();
                o.codigo = i;

                var ids = _context.tblM_CatCriteriosCausaParo.Where(x => x.TiempoMantenimiento.Equals(i)).Select(x => x.id).ToList();
                var ordenes = _context.tblM_CapOrdenTrabajo.Where(x => maquinasID.Contains(x.EconomicoID) && ids.Contains(x.MotivoParo) && cc.Contains(x.CC) && (x.FechaEntrada >= fechaInicio && x.FechaEntrada <= fechaFin) && x.FechaSalida != null).OrderBy(x => x.FechaEntrada).Count();
                o.cantidad = ordenes;
                result.MotivosParoDTO.Add(o);
            }
            return result;
        }

        public List<tblHorasHombreDTO> ConsultaFiltrosOT(FiltrosRtpHorasHombre obj)
        {

            List<string> CCs = obj.CC.ToList();
            obj.FechaFin = obj.FechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);
            var c = CCs.Count();

            string condicionesParo = CondicionesParo(obj.TipoMatto);
            string TipoParo = tipoParo(obj.TipoParo);


            var rawOT = (from dot in _context.tblM_DetOrdenTrabajo
                         join tm in _context.tblM_CatCriteriosCausaParo
                         on dot.OrdenTrabajo.MotivoParo equals tm.id
                         join m in _context.tblM_CatMaquina
                         on dot.OrdenTrabajo.EconomicoID equals m.id
                         where (dot.OrdenTrabajo.FechaEntrada >= obj.FechaInicio && dot.OrdenTrabajo.FechaEntrada <= obj.FechaFin) && obj.CC.Contains(dot.OrdenTrabajo.CC)
                         && (obj.economico != 0 ? dot.OrdenTrabajo.EconomicoID == obj.economico : dot.id == dot.id)
                         /*/(obj.CC.Count() > 0 ? obj.CC.Contains(dot.OrdenTrabajo.CC) : dot.id == dot.id)
                         && 
                         && (obj.MotivoParo != 0 ? dot.OrdenTrabajo.MotivoParo == obj.MotivoParo : dot.id == dot.id)
                         && (obj.grupo != 0 ? m.grupoMaquinariaID == obj.grupo : dot.id == dot.id)
                         && (obj.economico != 0 ? obj.economico == dot.OrdenTrabajo.EconomicoID : dot.id == dot.id)
                         && (obj.MotivoParo == 0 ? (TipoParo != "" ? tm.TipoParo == TipoParo : dot.id == dot.id) : dot.id == dot.id)
                         && (obj.MotivoParo == 0 ? (obj.CondicionParo != 0 ? dot.OrdenTrabajo.TipoParo3 == obj.CondicionParo : dot.id == dot.id) : dot.id == dot.id)
                         && (obj.MotivoParo == 0 ? (condicionesParo != "" ? tm.TiempoMantenimiento.Equals(condicionesParo) : dot.id == dot.id) : dot.id == dot.id)*/
                         select dot).ToList();

            var LsitaOTS = rawOT.Select(x => x.OrdenTrabajoID).ToList();

            HttpContext.Current.Session["lstRawDataOT"] = rawOT.ToList();

            List<tblHorasHombreDTO> tblHorasHombreObj = new List<tblHorasHombreDTO>();

            var rawPersonal = rawOT.GroupBy(x => x.PersonalID).Select(y => new
            {
                id = y.FirstOrDefault().PersonalID
            }).ToList();

            if (rawPersonal.Count > 0)
            {
                //string Consutal = "SELECT p.puesto,p.descripcion,e.clave_empleado AS personalID, s.salario_base AS salarioBase, s.complemento AS salarioComplemento, ";
                //Consutal += "e.nombre,ape_paterno,e.ape_materno ";
                //Consutal += "FROM DBA.sn_empleados AS e ";
                //Consutal += "INNER JOIN DBA.si_puestos as p on e.puesto=p.puesto ";
                //Consutal += "INNER JOIN DBA.sn_tabulador_historial AS s ON e.clave_empleado=s.clave_empleado ";
                //Consutal += "INNER JOIN (SELECT  MAX(id) AS id ,clave_empleado FROM DBA.sn_tabulador_historial GROUP BY (clave_empleado)) AS F ON F.id = s.id ";
                //Consutal += "WHERE e.clave_empleado IN (" + getListaUsuarios(rawPersonal.Select(x => x.id)) + ")";

                List<PuestosDTO> newListapuestos = new List<PuestosDTO>();
                try
                {
                    //var lstPuestosEnkontrol = (List<PuestosDTO>)ContextEnKontrolNominaArrendadora.Where(Consutal, 1).ToObject<List<PuestosDTO>>();

                    var lstPuestosEnkontrol = _context.Select<PuestosDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Construplan,
                        consulta = @"SELECT p.puesto,p.descripcion,e.clave_empleado AS personalID, s.salario_base AS salarioBase, s.complemento AS salarioComplemento, 
                                        e.nombre,ape_paterno,e.ape_materno 
                                    FROM tblRH_EK_Empleados AS e 
                                    INNER JOIN tblRH_EK_Puestos as p on e.puesto=p.puesto 
                                    INNER JOIN tblRH_EK_Tabulador_Historial AS s ON e.clave_empleado=s.clave_empleado 
                                    INNER JOIN (SELECT  MAX(id) AS id ,clave_empleado FROM tblRH_EK_Tabulador_Historial GROUP BY (clave_empleado)) AS F ON F.id = s.id 
                                    WHERE e.clave_empleado IN (" + getListaUsuarios(rawPersonal.Select(x => x.id)) + ")",
                    });

                    newListapuestos.AddRange(lstPuestosEnkontrol);
                }
                catch (Exception)
                {


                }
                try
                {
                    //var lstPuestosEnkontrolTemp = (List<PuestosDTO>)ContextEnKontrolNominaArrendadora.Where(Consutal, 2).ToObject<List<PuestosDTO>>();

                    var lstPuestosEnkontrolTemp = _context.Select<PuestosDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Arrendadora,
                        consulta = @"SELECT p.puesto,p.descripcion,e.clave_empleado AS personalID, s.salario_base AS salarioBase, s.complemento AS salarioComplemento, 
                                        e.nombre,ape_paterno,e.ape_materno 
                                    FROM tblRH_EK_Empleados AS e 
                                    INNER JOIN tblRH_EK_Puestos as p on e.puesto=p.puesto 
                                    INNER JOIN tblRH_EK_Tabulador_Historial AS s ON e.clave_empleado=s.clave_empleado 
                                    INNER JOIN (SELECT  MAX(id) AS id ,clave_empleado FROM tblRH_EK_Tabulador_Historial GROUP BY (clave_empleado)) AS F ON F.id = s.id 
                                    WHERE e.clave_empleado IN (" + getListaUsuarios(rawPersonal.Select(x => x.id)) + ")",
                    });

                    newListapuestos.AddRange(lstPuestosEnkontrolTemp);

                }
                catch (Exception)
                {


                }

                try
                {
                    //var lstPuestosEnkontrolTemp = (List<PuestosDTO>)ContextEnKontrolNominaArrendadora.Where(Consutal, 2).ToObject<List<PuestosDTO>>();

                    var lstPuestosEnkontrolTemp2 = _context.Select<PuestosDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT p.puesto,p.descripcion,e.clave_empleado AS personalID, s.salario_base AS salarioBase, s.complemento AS salarioComplemento, 
                                        e.nombre,ape_paterno,e.ape_materno 
                                    FROM tblRH_EK_Empleados AS e 
                                    INNER JOIN tblRH_EK_Puestos as p on e.puesto=p.puesto 
                                    INNER JOIN tblRH_EK_Tabulador_Historial AS s ON e.clave_empleado=s.clave_empleado 
                                    INNER JOIN (SELECT  MAX(id) AS id ,clave_empleado FROM tblRH_EK_Tabulador_Historial GROUP BY (clave_empleado)) AS F ON F.id = s.id 
                                    WHERE e.clave_empleado IN (" + getListaUsuarios(rawPersonal.Select(x => x.id)) + ")",
                    });

                    newListapuestos.AddRange(lstPuestosEnkontrolTemp2);

                }
                catch (Exception)
                {


                }

                HttpContext.Current.Session["lstPuestosEnkontrol"] = newListapuestos.Distinct().ToList();

                foreach (var item in newListapuestos.OrderBy(x => x.puesto))
                {

                    tblHorasHombreDTO objHH = new tblHorasHombreDTO();

                    var inforData = rawOT.Where(x => x.PersonalID == item.personalID).ToList();

                    objHH.puestoID = item.puesto;
                    objHH.descripcionPuesto = item.descripcion;
                    objHH.totalHorasHombre = inforData.Sum(x => GetTotalHoras(x.HoraInicio, x.HoraFin));

                    var SueldoBase = item.salarioBase;
                    var Complemento = item.salarioComplemento;
                    var CostoHora = (SueldoBase + Complemento) / 55;
                    var PrecioHHS = objHH.totalHorasHombre * CostoHora;

                    objHH.costoHorasHombre = CostoHora;// PrecioHHS;
                    tblHorasHombreObj.Add(objHH);
                }
            }
            return tblHorasHombreObj;

        }

        public string tipoParo(int p)
        {
            switch (p)
            {
                case 1:
                    return "Programado";
                case 2:
                    return "No Programado";
                default:
                    return "";
            }
        }

        public string CondicionesParo(int p)
        {
            switch (p)
            {
                case 1:
                    return "Preventivo";
                case 2:
                    return "Correctivo";
                case 3:
                    return "Predictivo";
                default:
                    return "";
            }
        }

        private decimal GetTotalHoras(DateTime dateTime1, DateTime? dateTime2)
        {
            var FechaEntrada = dateTime1;
            var FechaSalida = dateTime2;

            if (dateTime2 != null)
            {
                var horas = (decimal)((DateTime)FechaSalida - FechaEntrada).TotalHours;
                return horas;
            }
            else
            {
                return 0;
            }
        }

        public List<tblHorasHombreDetDTO> ConsultaFiltrosOTDET(tblHorasHombreDetalleDTO obj)
        {
            var rawOT = (List<tblM_DetOrdenTrabajo>)HttpContext.Current.Session["lstRawDataOT"];

            var rawPersonal = rawOT.GroupBy(x => x.PersonalID).Select(y => new
            {
                id = y.FirstOrDefault().PersonalID
            }).ToList();

            var rawSession = (List<PuestosDTO>)HttpContext.Current.Session["lstPuestosEnkontrol"];

            var resultado = rawSession.Where(x => x.puesto == obj.puestoID);

            var listaPersonalID = resultado.Select(x => x.personalID).ToList();

            var ListaEmpleados = rawOT.Where(x => listaPersonalID.Contains(x.PersonalID)).ToList();

            List<tblHorasHombreDetDTO> objListaHorasHombreDET = new List<tblHorasHombreDetDTO>();

            foreach (var personalitem in listaPersonalID)
            {

                tblHorasHombreDetDTO objHorasHombre = new tblHorasHombreDetDTO();

                var PersonalOBJ = resultado.FirstOrDefault(x => x.personalID == personalitem);
                objHorasHombre.personalID = personalitem;
                if (PersonalOBJ != null)
                {
                    objHorasHombre.personalNombre = PersonalOBJ.nombre + " " + PersonalOBJ.ape_paterno;
                }

                var personalRawDAaa = ListaEmpleados.Where(x => x.PersonalID == personalitem);

                var Preventivo = _context.tblM_CatCriteriosCausaParo.Where(x => x.TiempoMantenimiento == "Preventivo").Select(x => x.id).ToList();
                var Predictivo = _context.tblM_CatCriteriosCausaParo.Where(x => x.TiempoMantenimiento == "Predictivo").Select(x => x.id).ToList();
                var Correctivo = _context.tblM_CatCriteriosCausaParo.Where(x => x.TiempoMantenimiento == "Correctivo").Select(x => x.id).ToList();

                var CalculosPredictivo = personalRawDAaa.Where(x => Predictivo.Contains(x.OrdenTrabajo.MotivoParo));
                var CalculosPreventivos = personalRawDAaa.Where(x => Preventivo.Contains(x.OrdenTrabajo.MotivoParo));
                var CalculosCorrectivos = personalRawDAaa.Where(x => Correctivo.Contains(x.OrdenTrabajo.MotivoParo));

                objHorasHombre.hrasPreventivo = CalculosPreventivos.Sum(x => GetTotalHoras(x.HoraInicio, x.HoraFin));
                objHorasHombre.hrasPredictivo = CalculosPredictivo.Sum(x => GetTotalHoras(x.HoraInicio, x.HoraFin));
                objHorasHombre.hrasCorrectivo = CalculosCorrectivos.Sum(x => GetTotalHoras(x.HoraInicio, x.HoraFin));
                objHorasHombre.cantidadOT = personalRawDAaa.Select(x => x.OrdenTrabajo).Count();
                objHorasHombre.promedioHrasOT = (objHorasHombre.hrasPreventivo + objHorasHombre.hrasPredictivo + objHorasHombre.hrasCorrectivo);
                objHorasHombre.puesto = PersonalOBJ.descripcion;


                objListaHorasHombreDET.Add(objHorasHombre);
            }
            return objListaHorasHombreDET;
        }

        public List<tblHorasHombreDetDTO> ConsultaRptPersonal(List<int> Listapuestos, List<int> listaPersonalID)
        {
            var rawOT = (List<tblM_DetOrdenTrabajo>)HttpContext.Current.Session["lstRawDataOT"];

            var rawSession = (List<PuestosDTO>)HttpContext.Current.Session["lstPuestosEnkontrol"];

            var ListaEmpleadosXPuesto = rawSession.Where(x => Listapuestos.Contains(x.puesto)).Select(x => x.personalID).ToList();
            var ListaEmpleados = rawOT.Where(x => (listaPersonalID != null ? listaPersonalID.Contains(x.PersonalID) : (ListaEmpleadosXPuesto.Contains(x.PersonalID)))).ToList();

            var FiltrarEmpleados = ListaEmpleados.GroupBy(x => x.PersonalID).ToList();
            List<tblHorasHombreDetDTO> objListaHorasHombreDET = new List<tblHorasHombreDetDTO>();

            foreach (var personalitem in FiltrarEmpleados.Select(x => x.Key))
            {

                tblHorasHombreDetDTO objHorasHombre = new tblHorasHombreDetDTO();

                var PersonalOBJ = rawSession.FirstOrDefault(x => x.personalID == personalitem);

                objHorasHombre.personalID = personalitem;
                if (PersonalOBJ != null)
                {
                    objHorasHombre.personalNombre = PersonalOBJ.nombre + " " + PersonalOBJ.ape_paterno;
                }

                var personalRawDAaa = ListaEmpleados.Where(x => x.PersonalID == personalitem);

                var Preventivo = _context.tblM_CatCriteriosCausaParo.Where(x => x.TiempoMantenimiento == "Preventivo").Select(x => x.id).ToList();
                var Predictivo = _context.tblM_CatCriteriosCausaParo.Where(x => x.TiempoMantenimiento == "Predictivo").Select(x => x.id).ToList();
                var Correctivo = _context.tblM_CatCriteriosCausaParo.Where(x => x.TiempoMantenimiento == "Correctivo").Select(x => x.id).ToList();

                var CalculosPredictivo = personalRawDAaa.Where(x => Predictivo.Contains(x.OrdenTrabajo.MotivoParo));
                var CalculosPreventivos = personalRawDAaa.Where(x => Preventivo.Contains(x.OrdenTrabajo.MotivoParo));
                var CalculosCorrectivos = personalRawDAaa.Where(x => Correctivo.Contains(x.OrdenTrabajo.MotivoParo));

                objHorasHombre.hrasPreventivo = CalculosPreventivos.Sum(x => GetTotalHoras(x.HoraInicio, x.HoraFin));
                objHorasHombre.hrasPredictivo = CalculosPredictivo.Sum(x => GetTotalHoras(x.HoraInicio, x.HoraFin));
                objHorasHombre.hrasCorrectivo = CalculosCorrectivos.Sum(x => GetTotalHoras(x.HoraInicio, x.HoraFin));
                objHorasHombre.cantidadOT = personalRawDAaa.Select(x => x.OrdenTrabajo).Count();
                objHorasHombre.promedioHrasOT = (objHorasHombre.hrasPreventivo + objHorasHombre.hrasPredictivo + objHorasHombre.hrasCorrectivo);
                objHorasHombre.puesto = PersonalOBJ.descripcion;

                objListaHorasHombreDET.Add(objHorasHombre);
            }


            return objListaHorasHombreDET;
        }

        private string getListaUsuarios(IEnumerable<int> enumerable)
        {
            string cadena = "";

            foreach (var item in enumerable)
            {
                cadena += "'" + item + "',";
            }
            return cadena.TrimEnd(',');
        }

        public List<tblDetallePersonalDTO> GetOTEmpleado(int PersonalID, DateTime FechaInicio, DateTime FechaFin)
        {


            var tblM_DetOrdenTrabajo = (List<tblM_DetOrdenTrabajo>)HttpContext.Current.Session["lstRawDataOT"];


            var DetalleOT = (from detot in tblM_DetOrdenTrabajo.ToList()
                             join maquina in _context.tblM_CatMaquina on detot.OrdenTrabajo.EconomicoID equals maquina.id
                             join tipoParo in _context.tblM_CatCriteriosCausaParo on detot.OrdenTrabajo.MotivoParo equals tipoParo.id
                             where detot.PersonalID == PersonalID && (detot.OrdenTrabajo.FechaEntrada >= FechaInicio && detot.OrdenTrabajo.FechaEntrada <= FechaFin)
                             select new
                             {
                                 economico = maquina.noEconomico,
                                 finParo = detot.OrdenTrabajo.FechaSalida,
                                 folio = detot.OrdenTrabajoID,
                                 inicioParo = detot.OrdenTrabajo.FechaEntrada,
                                 motivoParo = tipoParo.CausaParo

                             }).ToList();


            var Dt = DetalleOT.Select(x => new tblDetallePersonalDTO
            {
                economico = x.economico,
                finParo = x.finParo != null ? x.finParo.Value.ToString() : "",
                folio = x.folio.ToString(),
                inicioParo = x.inicioParo.ToString(),
                motivoParo = x.motivoParo
            }).ToList();


            return Dt;
        }

        public List<tblM_CatCriteriosCausaParo> getCatCriteriosCausa()
        {
            return _context.tblM_CatCriteriosCausaParo.ToList();
        }

        public List<frecuenciaParoDTO> getDataFrecuenciasParo(FiltrosRtpHorasHombre obj, decimal horaIncio, decimal horaFinal)
        {
            List<string> CCs = obj.CC.ToList();
            obj.FechaFin = obj.FechaFin.AddHours(23).AddMinutes(59).AddSeconds(60);
            var c = CCs.Count();


            string condicionesParo = CondicionesParo(obj.TipoMatto);
            string TipoParo = tipoParo(obj.TipoParo);


            var rawOT = (from dot in _context.tblM_DetOrdenTrabajo
                         join tm in _context.tblM_CatCriteriosCausaParo
                         on dot.OrdenTrabajo.MotivoParo equals tm.id
                         join m in _context.tblM_CatMaquina
                         on dot.OrdenTrabajo.EconomicoID equals m.id
                         where (obj.CC.Count() > 0 ? obj.CC.Contains(dot.OrdenTrabajo.CC) : dot.id == dot.id)
                         && (dot.OrdenTrabajo.FechaEntrada >= obj.FechaInicio && dot.OrdenTrabajo.FechaEntrada <= obj.FechaFin)
                         && (obj.MotivoParo != 0 ? dot.OrdenTrabajo.MotivoParo == obj.MotivoParo : dot.id == dot.id)
                         && (obj.grupo != 0 ? m.grupoMaquinariaID == obj.grupo : dot.id == dot.id)
                         && (obj.modelo != 0 ? m.modeloEquipoID == obj.modelo : dot.id == dot.id)
                         && (obj.economico != 0 ? obj.economico == dot.OrdenTrabajo.EconomicoID : dot.id == dot.id)
                         && (obj.MotivoParo == 0 ? (TipoParo != "" ? tm.TipoParo == TipoParo : dot.id == dot.id) : dot.id == dot.id)
                         && (obj.MotivoParo == 0 ? (obj.CondicionParo != 0 ? dot.OrdenTrabajo.TipoParo3 == obj.CondicionParo : dot.id == dot.id) : dot.id == dot.id)
                         && (obj.MotivoParo == 0 ? (condicionesParo != "" ? tm.TiempoMantenimiento.Equals(condicionesParo) : dot.id == dot.id) : dot.id == dot.id)
                         && (horaIncio != 0 ? horaIncio > dot.OrdenTrabajo.horometro && dot.OrdenTrabajo.horometro < horaFinal : dot.id == dot.id)
                         select dot).ToList();

            HttpContext.Current.Session["lstRawDataFrecuenciaParo"] = rawOT;

            var rawFrecuenciaParo = _context.tblM_CatCriteriosCausaParo.ToList();
            var rawOTParos = rawOT.Select(x => x.OrdenTrabajo).ToList();

            int count = 1;
            List<frecuenciaParoDTO> objDat = new List<frecuenciaParoDTO>();

            foreach (var item in rawFrecuenciaParo)
            {
                frecuenciaParoDTO objData = new frecuenciaParoDTO();
                var motivoParoObj = rawOTParos.Where(x => x.MotivoParo == item.id).ToList();
                var Calculo = rawOT.Where(x => x.OrdenTrabajo.MotivoParo == item.id).ToList();
                objData.motivoParoID = item.id;
                objData.detalleFrecuencia = item.CausaParo;
                objData.frecuenciaParo = motivoParoObj.Count();
                objData.tiempoOT = Calculo.Select(x => x.OrdenTrabajo).ToList().Sum(y => GetTotalHoras(y.FechaEntrada, y.FechaSalida));
                objData.horasHombre = Calculo.Sum(y => GetTotalHoras(y.HoraInicio, y.HoraFin));
                objData.motivoParo = item.CausaParo;
                objDat.Add(objData);

            }
            return objDat.OrderByDescending(x => x.frecuenciaParo).Select(X => new frecuenciaParoDTO { no = count++, motivoParoID = X.motivoParoID, detalleFrecuencia = X.detalleFrecuencia, frecuenciaParo = X.frecuenciaParo, tiempoOT = X.tiempoOT, horasHombre = X.horasHombre, motivoParo = X.motivoParo }).ToList();
        }

        public List<tblM_DetOrdenTrabajo> getDetOrdenTrabajo(FiltrosRtpHorasHombre obj, decimal horaIncio, decimal horaFinal)
        {
            List<string> CCs = obj.CC.ToList();
            obj.FechaFin = obj.FechaFin.AddHours(23).AddMinutes(59).AddSeconds(60);
            var c = CCs.Count();


            string condicionesParo = CondicionesParo(obj.TipoMatto);
            string TipoParo = tipoParo(obj.TipoParo);


            var rawOT = (from dot in _context.tblM_DetOrdenTrabajo
                         join tm in _context.tblM_CatCriteriosCausaParo
                         on dot.OrdenTrabajo.MotivoParo equals tm.id
                         join m in _context.tblM_CatMaquina
                         on dot.OrdenTrabajo.EconomicoID equals m.id
                         where (obj.CC.Count() > 0 ? obj.CC.Contains(dot.OrdenTrabajo.CC) : dot.id == dot.id)
                         && (dot.OrdenTrabajo.FechaEntrada >= obj.FechaInicio && dot.OrdenTrabajo.FechaEntrada <= obj.FechaFin)
                         && (obj.MotivoParo != 0 ? dot.OrdenTrabajo.MotivoParo == obj.MotivoParo : dot.id == dot.id)
                         && (obj.grupo != 0 ? m.grupoMaquinariaID == obj.grupo : dot.id == dot.id)
                         && (obj.modelo != 0 ? m.modeloEquipoID == obj.modelo : dot.id == dot.id)
                         && (obj.economico != 0 ? obj.economico == dot.OrdenTrabajo.EconomicoID : dot.id == dot.id)
                         && (obj.MotivoParo == 0 ? (TipoParo != "" ? tm.TipoParo == TipoParo : dot.id == dot.id) : dot.id == dot.id)
                         && (obj.MotivoParo == 0 ? (obj.CondicionParo != 0 ? dot.OrdenTrabajo.TipoParo3 == obj.CondicionParo : dot.id == dot.id) : dot.id == dot.id)
                         && (obj.MotivoParo == 0 ? (condicionesParo != "" ? tm.TiempoMantenimiento.Equals(condicionesParo) : dot.id == dot.id) : dot.id == dot.id)
                         && (horaIncio != 0 ? horaIncio > dot.OrdenTrabajo.horometro && dot.OrdenTrabajo.horometro < horaFinal : dot.id == dot.id)
                         select dot).ToList();

            HttpContext.Current.Session["lstRawDataFrecuenciaParo"] = rawOT;

            return rawOT;
        }

        public List<detFrecuenciaParoDTO> DetalleTiposParo(int TipoParoID, DateTime FechaInicio, DateTime FechaFin)
        {
            //    var otDet = (from otd in _context.tblM_DetOrdenTrabajo
            //                 join m in _context.tblM_CatMaquina on otd.OrdenTrabajo.EconomicoID equals m.id
            //                 where otd.OrdenTrabajo.MotivoParo == TipoParoID && (otd.OrdenTrabajo.FechaEntrada >= FechaInicio && otd.OrdenTrabajo.FechaEntrada <= FechaFin)
            //                 select new { otd, m.noEconomico }).ToList();

            var otDet = (List<tblM_DetOrdenTrabajo>)HttpContext.Current.Session["lstRawDataFrecuenciaParo"];

            var RawOT = otDet.Where(x => x.OrdenTrabajo.MotivoParo == TipoParoID).GroupBy(y => y.OrdenTrabajo).Select(x => x.Key).ToList();

            List<detFrecuenciaParoDTO> objList = new List<detFrecuenciaParoDTO>();

            foreach (var item in RawOT)
            {
                detFrecuenciaParoDTO objData = new detFrecuenciaParoDTO();
                var objRaw = otDet.Where(x => x.OrdenTrabajoID == item.id).ToList();

                objData.folio = item.id.ToString();

                objData.horashombre = otDet.Where(x => x.OrdenTrabajoID == item.id).ToList().Count();

                objData.cantidadPersonas = objRaw.FirstOrDefault(x => x.OrdenTrabajo.id == item.id).OrdenTrabajo.horometro;

                var economico = objRaw.FirstOrDefault(x => x.OrdenTrabajo.id == item.id).OrdenTrabajo.EconomicoID;
                var objMaquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == economico).noEconomico;

                objData.economico = objMaquina;
                objData.economicoID = item.EconomicoID;
                objData.horashombre = objRaw.Sum(x => GetTotalHoras(x.HoraInicio, x.HoraFin));

                objData.tiempoUtil = GetTotalHoras(item.FechaEntrada, item.FechaSalida);

                objData.comentariosSolucion = item.Comentario;
                objData.tiempoMuerto = CalcularTiempoMuerto(item.TiempoHorasMuerto, item.TiempoMinutosMuerto);

                objList.Add(objData);
            }

            return objList;
        }

        private decimal CalcularTiempoMuerto(int horas, int minutos)
        {
            decimal minutosTotales = 0;

            minutosTotales = ((horas * 60) + minutos) / 60;

            return minutosTotales;
        }

        public List<tblM_DisponibilidadMaquina> indiceDisponibilidad(List<tblP_CC_Usuario> listObj)
        {
            var listCC = listObj.Select(x => x.cc).ToList();
            return _context.tblM_DisponibilidadMaquina.Where(x => listCC.Contains(x.cc.ToString()) && x.estatus == 0).ToList();
        }

        public List<tblM_RendimientoMaquina> alertasRendimiento(List<tblP_CC_Usuario> listObj)
        {
            var listCC = listObj.Select(x => x.cc).ToList();
            return _context.tblM_RendimientoMaquina.Where(x => listCC.Contains(x.cc.ToString()) && x.estatus == 0).ToList();
        }

        private string getCCByID(string cc)
        {
            try { return _context.tblP_CC.FirstOrDefault(x => x.cc == cc).areaCuenta; }
            catch (Exception) { return cc; }
        }
        public decimal promedioRendimiento(string cc, int modelo)
        {
            var aux = _context.tblM_RendimientoMaquina.Where(x => x.cc == cc && x.modelo == modelo && x.estatus == 0).ToList();
            decimal promedio = 0;
            for (var i = 0; i < aux.Count(); i++)
            {
                promedio += aux[i].rendimiento;
            }
            promedio = promedio / aux.Count();
            return promedio;
        }
    }
}
