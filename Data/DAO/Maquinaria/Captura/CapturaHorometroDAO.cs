using Core.DAO.Maquinaria.Captura;
using Core.DAO.Maquinaria.Catalogos;
using Core.DTO;
using Core.DTO.Captura;
using Core.DTO.Maquinaria.SOS;
using Core.DTO.Maquinaria.Captura;
using Core.DTO.Reportes;
using Core.Entity.Maquinaria;
using Core.Entity.Maquinaria.Captura;
using Core.Enum.Multiempresa;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Infrastructure.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using OfficeOpenXml.Drawing;
using System.IO;
using Core.DTO.Principal.Generales;
using Core.DTO.Utils.Data;
using Core.Enum.Principal;
using System.Data.Entity;
using System.Web;
using Data.Factory.Maquinaria;
using Core.Enum.Maquinaria.StandBy;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.StandBy;
using System.Data.Odbc;
using System.Net.Mail;

namespace Data.DAO.Maquinaria.Captura
{
    public class CapturaHorometroDAO : GenericDAO<tblM_CapHorometro>, ICapturaHorometroDAO
    {
        private const string _NOMBRE_CONTROLADOR = "HorometrosController";
        private const int _SISTEMA = (int)SistemasEnum.MAQUINARIA;

        public List<CapHorometroDTO> getDataTable(string cc, int turno, DateTime fe, int tipo)
        {
            // Ajuste del centro de costos según la empresa
            if ((EmpresaEnum)vSesiones.sesionEmpresaActual == EmpresaEnum.Peru)
            {
                var areaCuenta = _context.tblP_CC.FirstOrDefault(x => x.cc == cc);
                cc = areaCuenta != null ? areaCuenta.areaCuenta : cc;
            }

            DateTime FechaActual = fe.Date;
            List<CapHorometroDTO> lista = new List<CapHorometroDTO>();

            // Obtener la lista de económicos bloqueados
            var listaMaquinasBloqueoStandby = _context.tblM_STB_EconomicoBloqueado
                .Where(x => x.registroActivo)
                .Select(x => x.noEconomico)
                .ToList();

            // Obtener la lista de económicos de máquinas filtradas
            var listaEconomicosMaquina = _context.tblM_CatMaquina
                .Where(x => x.centro_costos == cc &&
                            x.estatus != 0 &&
                            (tipo != 0 ? x.grupoMaquinaria.tipoEquipoID == tipo : true) &&
                            x.TipoCaptura != 0 &&
                            !listaMaquinasBloqueoStandby.Contains(x.noEconomico))
                .Select(x => x.noEconomico)
                .ToList();

            // Cargar todos los datos necesarios en una sola consulta
            var horometrosMaquinas = _context.tblM_CapHorometro
                .Where(x => listaEconomicosMaquina.Contains(x.Economico))
                .OrderByDescending(x => new { x.Economico, x.Fecha, x.turno })
                .ToList();

            var listaRitmos = _context.tblM_CapRitmoHorometro
                .Where(r => listaEconomicosMaquina.Contains(r.economico))
                .ToList();

            var listaDesfases = _context.tblM_CapDesfase
                .Where(d => listaEconomicosMaquina.Contains(d.Economico))
                .ToList();

            var horometrosDelDia = horometrosMaquinas
                .Where(x => x.Fecha.Date == FechaActual)
                .ToList();

            // Obtener información de las máquinas una sola vez
            var economicos = _context.tblM_CatMaquina
                .Where(x => listaEconomicosMaquina.Contains(x.noEconomico))
                .ToList();

            foreach (var noEconomico in listaEconomicosMaquina)
            {
                var horometrosMaquina = horometrosMaquinas
                    .Where(x => x.Economico == noEconomico)
                    .Take(20)
                    .ToList();

                var horometrosDelDiaMaquina = horometrosDelDia
                    .Where(x => x.Economico == noEconomico)
                    .ToList();

                if (horometrosMaquina.Any())
                {
                    bool flag = false;
                    var ultimoHorometro = horometrosMaquina.FirstOrDefault();
                    if (fe < ultimoHorometro.Fecha)
                    {
                        var temp = horometrosMaquina
                            .Where(x => x.Fecha == fe)
                            .OrderByDescending(x => x.turno)
                            .FirstOrDefault();
                        if (temp != null)
                        {
                            ultimoHorometro = temp;
                        }
                        else
                        {
                            flag = true;
                        }
                    }

                    var ultimoDesfase = listaDesfases
                        .Where(x => x.Economico == noEconomico)
                        .OrderByDescending(x => x.id)
                        .FirstOrDefault();

                    var ritmoEconomico = listaRitmos
                        .FirstOrDefault(x => x.economico == noEconomico);

                    decimal promedioHoras = 0;
                    decimal horasTrabajo = 0;
                    decimal desfase = ultimoDesfase.horasDesfaseAcumulado;

                    if (ritmoEconomico != null)
                    {
                        promedioHoras = ritmoEconomico.horasDiarias;
                    }
                    else
                    {
                        promedioHoras = horometrosMaquina.Sum(x => x.HorasTrabajo) / 20;
                    }

                    decimal horometroActual = ultimoHorometro.Horometro;
                    if (ultimoHorometro.Fecha.Date == FechaActual)
                    {
                        var valorCapturado = horometrosMaquina
                            .Where(x => x.Fecha.Date == FechaActual);

                        var valido = valorCapturado.Any(x => x.turno == turno);

                        if (valido || turno <= ultimoHorometro.turno)
                        {
                            var unico = valorCapturado.FirstOrDefault(x => x.turno == turno);
                            horasTrabajo = unico.HorasTrabajo;
                            decimal tempH = unico.Horometro;
                            if (unico == null)
                            {
                                var i = valorCapturado.FirstOrDefault(x => x.turno == ultimoHorometro.turno);
                                tempH = i.Horometro;
                            }
                            horometroActual = unico == null ? tempH : (unico.Horometro == 0 ? unico.HorasTrabajo : unico.Horometro) - unico.HorasTrabajo;
                            flag = true;
                        }
                        else if (turno == 3 && ultimoHorometro.Fecha.Date != FechaActual)
                        {
                            flag = true;
                        }
                    }

                    var capturaHoy = horometrosMaquina
                        .Where(x => x.Economico == noEconomico && x.Fecha.Date == FechaActual)
                        .ToList();

                    var economico = economicos.FirstOrDefault(x => x.noEconomico == noEconomico);

                    bool aplicaRestriccion = economico.TipoCaptura != 2;
                    var MaximoCaptura = capturaHoy.Sum(x => x.HorasTrabajo);

                    var horasTrabajoTurno = horometrosDelDiaMaquina.FirstOrDefault(x => x.turno == turno);
                    if (horasTrabajoTurno != null)
                    {
                        horasTrabajo = horasTrabajoTurno.HorasTrabajo;
                    }

                    lista.Add(new CapHorometroDTO
                    {
                        CC = cc,
                        Desfase = desfase,
                        Economico = noEconomico,
                        Fecha = ultimoHorometro.Fecha,
                        HorasTrabajo = horasTrabajo,
                        HorometroActual = horometroActual + horasTrabajo,
                        Horometro = horometroActual,
                        HorometroAcumulado = ultimoHorometro.Horometro + desfase,
                        Ritmo = ultimoHorometro.Ritmo,
                        turno = ultimoHorometro.turno,
                        habilidatado = flag,
                        promedioHoras = promedioHoras,
                        tipoRitmo = ultimoHorometro.Ritmo ? "Manual" : "Automatico",
                        maximoHoras = aplicaRestriccion ? 24 - MaximoCaptura : 9999999
                    });
                }
                else
                {
                    var capturaHoy = horometrosMaquina
                        .Where(x => x.Economico == noEconomico && x.Fecha.Date == FechaActual)
                        .ToList();

                    var economico = economicos.FirstOrDefault(x => x.noEconomico == noEconomico);

                    bool aplicaRestriccion = economico.TipoCaptura != 2;
                    var MaximoCaptura = capturaHoy.Sum(x => x.HorasTrabajo);

                    lista.Add(new CapHorometroDTO
                    {
                        CC = cc,
                        Desfase = 0,
                        Economico = noEconomico,
                        Fecha = FechaActual,
                        HorasTrabajo = 0,
                        HorometroActual = 0,
                        Horometro = 0,
                        HorometroAcumulado = 0,
                        Ritmo = false,
                        turno = turno,
                        habilidatado = false,
                        promedioHoras = 0,
                        tipoRitmo = "Automatico",
                        maximoHoras = aplicaRestriccion ? 24 - MaximoCaptura : 9999999
                    });
                }
            }

            return lista;
        }

        public List<CapHorometroDTO> getDataTable_old(string cc, int turno, DateTime fe, int tipo)
        {
            switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
            {
                case EmpresaEnum.Peru:
                    {
                        var areaCuenta = _context.tblP_CC.FirstOrDefault(x => x.cc == cc);
                        cc = areaCuenta != null ? areaCuenta.areaCuenta : cc;
                    }
                    break;
            }
            DateTime FechaActual = fe;//DateTime.Now;

            List<CapHorometroDTO> lista = new List<CapHorometroDTO>();

            List<string> listaEconomicosMaquina = new List<string>();
            string economicoFinal = "";

            //listaMaquinas = listaMaquinasTemp(cc);

            var listaMaquinasBloqueoStandby = _context.tblM_STB_EconomicoBloqueado.Where(x => x.registroActivo).Select(x => x.noEconomico).ToList();

            listaEconomicosMaquina = _context.tblM_CatMaquina
                .Where(x => x.centro_costos.Equals(cc) &&
                    x.estatus != 0 &&
                      (tipo != 0 ? x.grupoMaquinaria.tipoEquipoID.Equals(tipo) : true) && x.TipoCaptura != 0 
                      && !listaMaquinasBloqueoStandby.Contains(x.noEconomico) )
                .Select(x => x.noEconomico).ToList();


            var listaRitmos = (from r in _context.tblM_CapRitmoHorometro
                             where listaEconomicosMaquina.Contains(r.economico)
                             select r).ToList();

            var listaDesfases = (from d in _context.tblM_CapDesfase
                              where listaEconomicosMaquina.Contains(d.Economico)
                              select d).ToList();

            var horometrosDelDia = _context.tblM_CapHorometro.Where(x => listaEconomicosMaquina.Contains(x.Economico) && x.Fecha == fe.Date).ToList();

            foreach (var noEconomico in listaEconomicosMaquina)
            {
                var horometrosMaquina = _context.tblM_CapHorometro.Where(x => x.Economico.Equals(noEconomico)).OrderByDescending(x => new { x.Fecha, x.turno }).Take(20).ToList();
                var horometrosDelDiaMaquina = horometrosDelDia.Where(x => x.Economico == noEconomico).ToList();

                if (horometrosMaquina.Count() > 0)
                {

                    bool flag = false;
                    var ultimoHorometro = horometrosMaquina.FirstOrDefault();
                    if (fe < ultimoHorometro.Fecha)
                    {

                        var temp = horometrosMaquina.Where(x => x.Fecha.Equals(fe)).OrderByDescending(x => x.turno).FirstOrDefault();
                        if (temp != null)
                        {
                            ultimoHorometro = temp;
                        }
                        else
                        {
                            flag = true;
                        }
                    }



                    var ultimoDesfase = listaDesfases.Where(x => x.Economico == noEconomico).OrderByDescending(x => x.id).FirstOrDefault();

                    var ritmoEconomico = listaRitmos.FirstOrDefault(x => x.economico.Equals(noEconomico));


                    decimal promedioHoras = 0;


                    decimal horasTrabajo = 0;
                    decimal desfase = 0;
                    if (ultimoDesfase != null)
                    {
                        desfase = ultimoDesfase.horasDesfaseAcumulado;
                    }
                    if (ritmoEconomico != null)
                    {
                        promedioHoras = ritmoEconomico.horasDiarias;
                    }
                    else
                    {
                        promedioHoras = horometrosMaquina.Sum(x => x.HorasTrabajo) / 20;
                    }

                    decimal horometroActual = ultimoHorometro.Horometro;
                    if (ultimoHorometro.Fecha.ToString("dd/M/yyyy").Equals(FechaActual.ToString("dd/M/yyyy")))
                    {
                        var valorCapturado = horometrosMaquina.Where(x => x.Fecha.ToString("dd/M/yyyy").Equals(FechaActual.ToString("dd/M/yyyy")));

                        var valido = valorCapturado.ToList().Exists(x => x.turno.Equals(turno));


                        if (valido || turno <= ultimoHorometro.turno)
                        {
                            var unico = valorCapturado.FirstOrDefault(x => x.turno == turno);
                            horasTrabajo = unico == null ? 0 : unico.HorasTrabajo;
                            decimal tempH = 0;
                            if (unico == null)
                            {
                                var i = valorCapturado.FirstOrDefault(x => x.turno == ultimoHorometro.turno);

                                tempH = i.Horometro;
                            }
                            horometroActual = unico == null ? tempH : (unico.Horometro == 0 ? unico.HorasTrabajo : unico.Horometro) - unico.HorasTrabajo;
                            flag = true;
                        }
                        else
                        {
                            if (turno == 3)
                            {
                                if (!ultimoHorometro.Fecha.ToString("dd/M/yyyy").Equals(FechaActual.ToString("dd/M/yyyy")))
                                {
                                    flag = true;
                                }

                            }

                        }
                    }
                    var capturaHoy = horometrosMaquina.Where(x => x.Economico == noEconomico && x.Fecha.ToString("dd/M/yyyy").Equals(FechaActual.ToString("dd/M/yyyy"))).ToList();
                    var economico = _context.tblM_CatMaquina.FirstOrDefault(x => x.noEconomico.Equals(noEconomico));




                    bool aplicaRestriccion = false;

                    if (economico != null)
                    {
                        aplicaRestriccion = economico.TipoCaptura == 2 ? false : true;
                    }

                    var MaximoCaptura = capturaHoy.Sum(x => x.HorasTrabajo);

                    var horasTrabajoTurno = horometrosDelDiaMaquina.FirstOrDefault(x => x.turno == turno);
                    if (horasTrabajoTurno != null)
                    {
                        horasTrabajo = horasTrabajoTurno.HorasTrabajo;
                    }

                    lista.Add(new CapHorometroDTO
                    {
                        CC = cc,
                        Desfase = desfase,
                        ///   DesfaseAcumulado = desfase,
                        Economico = noEconomico,//res.Economico,
                        Fecha = ultimoHorometro.Fecha,
                        HorasTrabajo = horasTrabajo,
                        HorometroActual = horometroActual + horasTrabajo,
                        Horometro = horometroActual,
                        HorometroAcumulado = ultimoHorometro.Horometro + desfase,
                        Ritmo = ultimoHorometro.Ritmo,
                        turno = ultimoHorometro.turno,
                        habilidatado = flag,
                        promedioHoras = promedioHoras,
                        tipoRitmo = ultimoHorometro.Ritmo ? "Manual" : "Automatico",
                        maximoHoras = aplicaRestriccion ? 24 - MaximoCaptura : 9999999

                    });
                }
                else
                {
                    var capturaHoy = horometrosMaquina.Where(x => x.Economico == noEconomico && x.Fecha.ToString("dd/M/yyyy").Equals(FechaActual.ToString("dd/M/yyyy"))).ToList();
                    var economico = _context.tblM_CatMaquina.FirstOrDefault(x => x.noEconomico.Equals(noEconomico));

                    bool aplicaRestriccion = false;

                    if (economico != null)
                    {
                        aplicaRestriccion = economico.TipoCaptura == 2 ? false : true;
                    }

                    var MaximoCaptura = capturaHoy.Sum(x => x.HorasTrabajo);
                    lista.Add(new CapHorometroDTO
                    {
                        CC = cc,
                        Desfase = 0,
                        ///   DesfaseAcumulado = desfase,
                        Economico = noEconomico,
                        Fecha = DateTime.Now,
                        HorasTrabajo = 0,
                        HorometroActual = 0,
                        Horometro = 0,
                        HorometroAcumulado = 0,
                        Ritmo = false,
                        turno = turno,
                        habilidatado = false,
                        promedioHoras = 0,
                        tipoRitmo = "Automatico",
                        maximoHoras = aplicaRestriccion ? 24 - MaximoCaptura : 9999999

                    });
                }
            }


            //aqui va el codigo
            return lista;
        }
        private List<string> listaMaquinasTemp(string cc)
        {
            List<string> lista = new List<string>();
            string centro_costos = "SELECT descripcion FROM si_area_cuenta WHERE centro_costo = '" + cc + "' AND cc_activo=1;";

            var resultado = (IList<economicoDTO>)_contextEnkontrol.Where(centro_costos).ToObject<IList<economicoDTO>>();

            foreach (var item in resultado.OrderBy(x => x.descripcion))
            {
                lista.Add(item.descripcion);
            }
            return lista;
        }

        public void Guardar(List<tblM_CapHorometro> array)
        {
            using (var _ctx = new MainContext(vSesiones.sesionEmpresaActual))
            {
                using (var dbContextTransaction = _ctx.Database.BeginTransaction())
                {
                    try
                    {
                        tblM_CapHorometro objNuevaCaptura = new tblM_CapHorometro();
                        List<tblM_CapHorometro> lstNuevasCapturas = new List<tblM_CapHorometro>();
                        bool registrar = true;
                        foreach (var item in array)
                        {
                            if (string.IsNullOrEmpty(item.CC)) { throw new Exception("Ocurrió un error al registrar los horometros."); }
                            if (string.IsNullOrEmpty(item.Economico)) { throw new Exception("Ocurrió un error al registrar los horometros."); }

                            if (item.HorasTrabajo > 0)
                            {
                                tblM_CapHorometro objUltimoHorometro = _ctx.tblM_CapHorometro.Where(w => w.Economico == item.Economico).OrderByDescending(o => o.Horometro).FirstOrDefault();

                                if (objUltimoHorometro != null)
                                {
                                    DateTime fechaUltimoRegistroHorometro = objUltimoHorometro.Fecha;

                                    if (item.Fecha == fechaUltimoRegistroHorometro && item.turno <= objUltimoHorometro.turno)
                                        registrar = false;
                                    else if (item.Fecha < fechaUltimoRegistroHorometro)
                                        registrar = false;
                                    else if (item.Fecha > fechaUltimoRegistroHorometro)
                                        registrar = true;
                                }

                                if (registrar)
                                {
                                    objNuevaCaptura = new tblM_CapHorometro();
                                    objNuevaCaptura.CC = item.CC;
                                    objNuevaCaptura.Economico = item.Economico;
                                    objNuevaCaptura.HorasTrabajo = item.HorasTrabajo;
                                    objNuevaCaptura.Horometro = item.Horometro;
                                    objNuevaCaptura.HorometroAcumulado = item.HorometroAcumulado;
                                    objNuevaCaptura.Desfase = item.Desfase;
                                    objNuevaCaptura.Fecha = item.Fecha;
                                    objNuevaCaptura.turno = item.turno;
                                    objNuevaCaptura.Ritmo = item.Ritmo;
                                    objNuevaCaptura.FechaCaptura = item.FechaCaptura;
                                    objNuevaCaptura.folio = item.folio;
                                    lstNuevasCapturas.Add(objNuevaCaptura);
                                }
                                else
                                {
                                    throw new Exception("Alerta: El horómetro a registrar ya esta capturado");
                                }
                            }
                        }

                        if (lstNuevasCapturas.Count() > 0)
                        {
                            _ctx.tblM_CapHorometro.AddRange(lstNuevasCapturas);
                            _ctx.SaveChanges();
                        }

                        dbContextTransaction.Commit();

                        SaveBitacora(0, (int)AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(array));
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                        LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                        throw new Exception(e.Message);
                    }
                }
            }
        }

        public bool Exists(tblM_CapHorometro obj)
        {
            return _context.tblM_CapDesfase.Where(x => x.id.Equals(obj.id)).ToList().Count > 0 ? true : false;
        }
        public void Guardar(tblM_CapHorometro obj)
        {
            if (!Exists(obj))
            {
                if (obj.id == 0)
                    throw new Exception("No se pudo activar el ritmo de trabajo");
                else
                    Update(obj, obj.id, (int)BitacoraEnum.RITMOHOROMETRO);
            }
            else
            {
                Update(obj, obj.id, (int)BitacoraEnum.RITMOHOROMETRO);
            }
        }

        public List<tblM_CapHorometro> getDataTableByRangeDate(string noeco, DateTime start, DateTime end)
        {
            var result = _context.tblM_CapHorometro.Where(x => x.Economico.Equals(noeco) && x.Fecha >= start && x.Fecha <= end).ToList();
            return result;
        }
        public List<tblM_CapHorometro> getDataTableByRangeDate(DateTime start, DateTime end, List<string> listaEconomicos)
        {
            var result = _context.tblM_CapHorometro.Where(x => x.Fecha >= start && x.Fecha <= end && listaEconomicos.Contains(x.Economico)).ToList();
            return result;
        }
        public List<tblM_CapHorometro> getTableByRangeDateTipo(DateTime start, DateTime end, string Grupo)
        {
            var result = (from h in _context.tblM_CapHorometro
                          join m in _context.tblM_CatMaquina on h.Economico equals m.noEconomico
                          where m.grupoMaquinaria.descripcion.Contains(Grupo) && h.Fecha >= start && h.Fecha <= end
                          select h).ToList();
            return result;
        }

        public List<tblM_CapHorometro> getHorasRangoFecha(DateTime start, DateTime end, int tipo)
        {
            var result = (from h in _context.tblM_CapHorometro
                          join m in _context.tblM_CatMaquina on h.Economico equals m.noEconomico
                          where m.grupoMaquinaria.tipoEquipoID.Equals(tipo) && h.Fecha >= start && h.Fecha <= end
                          select h).ToList();
            return result;
        }
        public List<tblM_CapHorometro> getHorasSoloRangoFecha(DateTime start, DateTime end)
        {
            var result = (from h in _context.tblM_CapHorometro
                          join m in _context.tblM_CatMaquina on h.Economico equals m.noEconomico
                          where h.Fecha >= start && h.Fecha <= end
                          select h).ToList();
            return result;
        }

        private List<string> Maquinas(string cc)
        {
            List<string> lista = new List<string>();
            string centro_costos = "SELECT descripcion FROM si_area_cuenta WHERE centro_costo = '" + cc + "' AND cc_activo=1;";

            var resultado = (IList<economicoDTO>)_contextEnkontrol.Where(centro_costos).ToObject<IList<economicoDTO>>();

            foreach (var item in resultado)
            {
                lista.Add(item.descripcion);

            }
            return lista;
        }

        public string getCentroCostos(string cc)
        {
            if (cc == "1015")
                return "PATIO DE MAQUINARIA";
            if (cc == "1010")
                return "TALLER DE MAQUINARIA";
            switch (vSesiones.sesionEmpresaActual)
            {
                case (int)EmpresaEnum.Construplan:
                    var resultado = (IList<economicoDTO>)_contextEnkontrol.Where("SELECT descripcion FROM cc WHERE cc = '" + cc + "';").ToObject<IList<economicoDTO>>();
                    return resultado.Select(x => x.descripcion).FirstOrDefault();
                case (int)EmpresaEnum.Arrendadora:
                    var res = (List<dynamic>)ContextArrendadora.Where(string.Format(@"SELECT top 1 descripcion FROM si_area_cuenta WHERE  area = {0} AND cuenta = {1}", cc.Split('-')[0], cc.Split('-')[1])).ToObject<List<dynamic>>();
                    return res.Count == 0 ? string.Empty : (string)res[0].descripcion;
                default: return string.Empty;

                case (int)EmpresaEnum.Colombia:
                    var lstCCColombia=_context.Select<ComboDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Colombia,
                        consulta = "SELECT cc as Value, (cc + ' - ' + ccDescripcion) as Text FROM tblC_Nom_CatalogoCC WHERE cc = '"+ cc +"';",
                    });
                     return lstCCColombia.Select(x => x.Text).FirstOrDefault();                
                 
                case (int)EmpresaEnum.Peru:
                     var lstCCPeru = _context.Select<ComboDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.PERU,
                        consulta = "SELECT cc as Value, (cc + ' - ' + ccDescripcion) as Text FROM tblC_Nom_CatalogoCC WHERE cc = '" + cc + "';",
                    });
                    return lstCCPeru.Select(x => x.Text).FirstOrDefault();
                    
            }
        }
        /// <summary>
        /// Consutla cc o ac dependiendo de la empresa
        /// </summary>
        /// <returns>Lista de cc o ac</returns>
        public List<cboDTO> cboCentroCostos()
        {
            var lst = new List<cboDTO>();
            switch (vSesiones.sesionEmpresaActual)
            {
                case (int)EmpresaEnum.Construplan:
                    lst.AddRange(_contextEnkontrol.Select<cboDTO>(EnkontrolEnum.CplanProd, "SELECT cc AS Value, (cc+'-'+descripcion) AS Text FROM cc order by cc"));
                    break;
                case (int)EmpresaEnum.Arrendadora:
                    lst.AddRange(_contextEnkontrol.Select<cboDTO>(EnkontrolEnum.ArrenProd, @"SELECT (STR(area,2)+'-'+STR(cuenta,2)) AS Value  ,(STR(area,2)+'-'+STR(cuenta,2)+' '+descripcion) AS Text 
                                                                                                FROM si_area_cuenta
                                                                                                GROUP BY area ,cuenta ,descripcion
                                                                                                ORDER BY area ,cuenta"));
                    break;
                case (int)EmpresaEnum.Colombia:
                    lst.AddRange(_context.Select<cboDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Colombia,
                        consulta = "SELECT cc as Value, (cc + ' - ' + ccDescripcion) as Text FROM tblC_Nom_CatalogoCC ORDER BY cc",
                    }));
                    //lst.AddRange(_contextEnkontrol.Select<cboDTO>(EnkontrolEnum.CplanProd, "SELECT cc AS Value, (cc+'-'+descripcion) AS Text FROM cc order by cc"));
                    break;
                case (int)EmpresaEnum.Peru:
                    lst.AddRange(_context.Select<cboDTO>(new DapperDTO 
                { 
                    baseDatos = MainContextEnum.PERU,
                    consulta = "SELECT cc as Value, (cc + ' - ' + ccDescripcion) as Text FROM tblC_Nom_CatalogoCC ORDER BY cc",
                }));
                       //lst.AddRange(_contextEnkontrol.Select<cboDTO>(EnkontrolEnum.CplanProd, "SELECT cc AS Value, (cc+'-'+descripcion) AS Text FROM cc order by cc"));
                    
                    break;
            }
            return lst;
        }
        public tblM_CapHorometro getDatoHorometro(string maquina)
        {
            tblM_CapHorometro dato = _context.tblM_CapHorometro
                        .Where(x => x.Economico.Equals(maquina))
                        .OrderByDescending(t => t.id).FirstOrDefault();

            return dato;
        }

        public decimal GetHorometroFinal(string Econ)
        {
            decimal horometro = 0;

            var c = _context.tblM_CapHorometro.Where(x => x.Economico.Equals(Econ)).OrderByDescending(y => y.id).FirstOrDefault();
            if (c != null)
            {
                horometro = c.Horometro;
            }

            return horometro;
        }

        public decimal GetHorometroInicial(string Econ)
        {
            decimal horometro = 0;

            var c = _context.tblM_CapHorometro.Where(x => x.Economico.Equals(Econ)).OrderByDescending(y => y.id).ToList();
            if (c.Count > 2)
            {
                horometro = c[1].Horometro;
            }

            return horometro;
        }

        public decimal getTotalHorometros(string Econ)
        {
            var economicos = _context.tblM_CapHorometro.Where(x => x.Economico == Econ).ToList();
            if (economicos != null)
            {
                var result = economicos.Sum(x => x.HorasTrabajo);
                return result;
            }
            else
            {
                return 0;
            }
        }

        public List<CapHorometroDTO> getReporteDiario(tblM_CapHorometro obj)
        {
            List<CapHorometroDTO> lista = new List<CapHorometroDTO>();
            List<string> listaMaquinas = new List<string>();
            // string economicoFinal = "";
            string formatoFecha = "dd/M/yyyy";

            listaMaquinas = listaMaquinasTemp(obj.CC);
            var rawData = _context.tblM_CapHorometro.Where(x => listaMaquinas.Contains(x.Economico) && x.Fecha <= obj.Fecha && obj.turno == x.turno).ToList().OrderBy(x => x.Economico);

            foreach (var economico in listaMaquinas)
            {
                var rawMaquina = rawData.Where(x => x.Economico.Equals(economico) && x.turno == obj.turno && x.Fecha.ToString(formatoFecha).Equals(obj.Fecha.ToString(formatoFecha)));

                if (rawMaquina.Count() > 0)
                {
                    var x = rawMaquina.FirstOrDefault();

                    lista.Add(new CapHorometroDTO
                                {
                                    CC = x.CC,
                                    Economico = x.Economico,
                                    HorasTrabajo = x.HorasTrabajo,
                                    Horometro = x.Horometro - x.HorasTrabajo,
                                    HorometroAcumulado = x.HorometroAcumulado,
                                    Desfase = x.Desfase,
                                    Fecha = x.Fecha,
                                    turno = x.turno,
                                    tipoRitmo = x.Ritmo == true ? "Manual" : "Automatico",
                                });
                }
                else
                {

                    var maxFecha = rawData.Where(x => x.Economico.Equals(economico)).OrderByDescending(x => x.Fecha).FirstOrDefault();

                    if (maxFecha != null)
                    {

                        lista.Add(new CapHorometroDTO
                        {
                            CC = maxFecha.CC,
                            Economico = maxFecha.Economico,
                            HorasTrabajo = 0,
                            Horometro = maxFecha.HorometroAcumulado,
                            HorometroAcumulado = maxFecha.HorometroAcumulado,
                            Desfase = maxFecha.Desfase,
                            Fecha = maxFecha.Fecha,
                            turno = maxFecha.turno,
                            tipoRitmo = maxFecha.Ritmo == true ? "Manual" : "Automatico",
                        });
                    }
                    else
                    {
                        lista.Add(new CapHorometroDTO
                        {
                            CC = obj.CC,
                            Economico = economico,
                            HorasTrabajo = 0,
                            Horometro = 0,
                            HorometroAcumulado = 0,
                            Desfase = 0,
                            Fecha = obj.Fecha,
                            turno = 1,
                            tipoRitmo = "Automatico",
                        });
                    }
                }
            }

            var result = (_context.tblM_CapHorometro.Where(x => x.CC.Equals(obj.CC)))
                .Select(x => new CapHorometroDTO
                {
                    CC = x.CC,
                    Economico = x.Economico,
                    HorasTrabajo = x.HorasTrabajo,
                    Horometro = x.Horometro,
                    HorometroAcumulado = x.HorometroAcumulado,
                    Desfase = x.Desfase,
                    Fecha = x.Fecha,
                    turno = x.turno,
                    tipoRitmo = x.Ritmo == true ? "Manual" : "Automatico",
                })
                .ToList().Where(x => x.Fecha.ToString(formatoFecha).Equals(obj.Fecha.ToString(formatoFecha)));

            return lista.ToList();
        }

        public List<tblM_CapHorometro> getTableInfoHorometros(string cc, int turno, DateTime fechaInicia, DateTime fechaFinal, string economico, string ccFiltro, int grupo, int modelo, decimal hInicial, decimal hFinal, bool estatus)
        {


            var res = (from h in _context.tblM_CapHorometro
                       join m in _context.tblM_CatMaquina
                       on h.Economico equals m.noEconomico
                       where (estatus ? m.estatus != 0 : m.estatus ==0) &&
                       (grupo != 0 ? m.grupoMaquinariaID == grupo : m.noEconomico == m.noEconomico) && (modelo != 0 ? m.modeloEquipoID == modelo : m.noEconomico == m.noEconomico) &&
                       (!string.IsNullOrEmpty(cc) ? h.CC.Equals(cc) : h.id.Equals(h.id)) && (turno != 0 ? h.turno.Equals(turno) : true) &&
                       (h.Fecha >= fechaInicia && h.Fecha <= fechaFinal) && (string.IsNullOrEmpty(economico) ? h.Economico.Equals(h.Economico) : h.Economico.Equals(economico))
                       select h).OrderByDescending(x => x.Fecha).ThenBy(x => x.Economico).ToList();
            return res;
        }

        public Dictionary<string, object> GetEconomicosSinHorometros(string areaCuenta, string economico, DateTime fechaInicio)
        {
            var resultado = new Dictionary<string, object>();

                try
                {
                    var listaEconomicos = _context.tblM_CapHorometro.Where(x =>
                        (areaCuenta != "" && areaCuenta != null ? x.CC == areaCuenta : true) &&
                        (economico != "" && economico != null ? x.Economico == economico : true) &&
                        DbFunctions.TruncateTime(x.Fecha) >= DbFunctions.TruncateTime(fechaInicio) &&
                        DbFunctions.TruncateTime(x.Fecha) < DbFunctions.TruncateTime(DateTime.Now)
                    ).ToList();
                    var listaCentrosCosto = _context.tblP_CC.Where(x => x.estatus).ToList();

                    var data = listaEconomicos.GroupBy(x => new { x.CC, x.Economico }).Select(x => new EconomicosSinHorometrosDTO
                    {
                        cc = x.Key.CC,
                        ccDesc = "[" + x.Key.CC + "] " + listaCentrosCosto.Where(y => y.areaCuenta == x.Key.CC).Select(z => z.descripcion).FirstOrDefault(),
                        economico = x.Key.Economico,
                        horometroAcumulado = x.OrderByDescending(y => y.Fecha).FirstOrDefault().HorometroAcumulado,
                        horometroAcumuladoDesc = x.OrderByDescending(y => y.Fecha).FirstOrDefault().HorometroAcumulado.ToString("N") + GetTipoDato(x.Key.Economico),
                        fecha = x.Max(y => y.Fecha),
                        fechaString = x.Max(y => y.Fecha).ToShortDateString(),
                        diasTranscurridos = (decimal)(DateTime.Now.Date - x.Max(y => y.Fecha).Date).TotalDays
                    }).ToList();

                    HttpContext.Current.Session["ReporteEconomicosSinHorometros"] = data;

                    resultado.Add("data", data);
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, "HorometrosController", "GetEconomicosSinHorometros", e, AccionEnum.ELIMINAR, 0, new{areaCuenta=areaCuenta, economico= economico, fechaInicio= fechaInicio});
                }

            return resultado;
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

        public List<CCMuestrasDTO> getListaCentrosCostos(DateTime fechaInicia, DateTime fechaFinal, string economico)
        {


            var res = _context.tblM_CapHorometro.Where(h => h.Economico.Equals(economico)).ToList();

            var datesREs = res.Where(h => h.Fecha >= fechaInicia && h.Fecha <= fechaFinal);

            var rest = datesREs.Select(x => new CCMuestrasDTO { folio = x.CC.ToString(), descripcion = x.CC.ToString() }).ToList();

            return rest;
        }

        public tblM_CapHorometro getUltimoHorometro(string Econ)
        {
            var res = (from h in _context.tblM_CapHorometro
                       where (h.Economico.Equals(Econ))
                       select h).ToList();

            if (res.Count > 0)
            {

                return res.OrderByDescending(x => x.Fecha).First();
            }
            else
            {
                return null;
            }

        }

        public List<tblM_CapHorometro> getHorometrosEficiencia(string Econ, DateTime Fecha, string cc)
        {
            DateTime fechaIncio = new DateTime(Fecha.Year, Fecha.Month, Fecha.AddDays(-1).Day);
            DateTime fechafin = new DateTime(Fecha.Year, Fecha.Month, Fecha.AddDays(1).Day);
            var res = _context.tblM_CapHorometro
                .Where(x => x.Economico.Equals(Econ) && x.CC == cc && x.Fecha >= fechaIncio && x.Fecha <= fechafin)
                .ToList();
            return res;
        }

        List<tblM_CapHorometro> getTableInfoHorometros(List<string> cc, DateTime fechaInicia, DateTime fechaFinal)
        {

            var res = _context.tblM_CapHorometro
                .Where(x => x.Fecha >= fechaInicia && x.Fecha <= fechaFinal)
                .ToList().Where(x => cc.Contains(x.CC.ToString()));
            return res.ToList();

        }

        public List<tblM_CapHorometro> getHorometro(string Econ, DateTime Fecha)
        {
            var data = _context.tblM_CapHorometro.Where(x => x.Economico == Econ && x.Fecha <= Fecha).OrderByDescending(x => x.id).ToList();

            return _context.tblM_CapHorometro.Where(x => x.Economico == Econ && x.Fecha <= Fecha).OrderByDescending(x => x.id).ToList();
        }

        public List<EconomicosHrsDTO> getReporteHorometro(List<string> cc, DateTime fechaInicio, DateTime fechaFin)
        {
            List<EconomicosHrsDTO> listaEconomicosHrsDTO = new List<EconomicosHrsDTO>();
            try
            {
                var infoHorometros = _context.tblM_CapHorometro.ToList().Where(r => cc.Contains(r.CC) && r.Fecha.Date >= fechaInicio.Date && r.Fecha.Date <= fechaFin.Date).ToList();
                var Economicos = infoHorometros.GroupBy(x => x.Economico).ToList().Select(e => e.Key).ToList();
                var listaCC = _context.tblP_CC.ToList().Where(x => cc.Contains(x.areaCuenta));


                foreach (var e in Economicos)
                {

                    var objFinal = infoHorometros.Where(x => x.Economico == e).OrderByDescending(r => r.id).FirstOrDefault();
                    var objInicio = infoHorometros.Where(x => x.Economico == e).OrderBy(r => r.id).FirstOrDefault();

                    EconomicosHrsDTO objEconomicosHrsDTO = new EconomicosHrsDTO();

                    var rawEconomico = _context.tblM_CatMaquina.FirstOrDefault(m => m.noEconomico == e);

                    if (rawEconomico != null)
                    {

                        var Modelo = _context.tblM_CatModeloEquipo.FirstOrDefault(m => m.id == rawEconomico.modeloEquipoID);

                        if (Modelo != null)
                        {
                            objEconomicosHrsDTO.noEconomico = e;
                            objEconomicosHrsDTO.modelo = Modelo.descripcion;

                            objEconomicosHrsDTO.fechaInicio = objInicio.Fecha.ToShortDateString();
                            objEconomicosHrsDTO.fechaFin = objFinal.Fecha.ToShortDateString();
                            objEconomicosHrsDTO.horometroFinal = objFinal.HorometroAcumulado;
                            objEconomicosHrsDTO.horometroInicial = objInicio.HorometroAcumulado;
                            objEconomicosHrsDTO.cc = objFinal.CC;
                            objEconomicosHrsDTO.efectivo = objEconomicosHrsDTO.horometroFinal - objEconomicosHrsDTO.horometroInicial;
                            objEconomicosHrsDTO.nombreObra = listaCC.Where(c => c.areaCuenta == objFinal.CC).Select(y => y.descripcion).FirstOrDefault();
                            objEconomicosHrsDTO.areaCuenta = objFinal.CC;

                            listaEconomicosHrsDTO.Add(objEconomicosHrsDTO);
                        }
                    }
                }

            }
            catch (Exception e)
            {

            }

            return listaEconomicosHrsDTO;
        }

        public MemoryStream exportarArchvio(List<EconomicosHrsDTO> listaEconomicosHrsDTO)
        {
            try
            {

                using (ExcelPackage package = new ExcelPackage())
                {
                    var mMayor = package.Workbook.Worksheets.Add("Horometros");

                    //HEADERS
                    mMayor.Cells["A1"].Value = "Fecha Inicio";
                    mMayor.Cells["B1"].Value = "Fecha Final";
                    mMayor.Cells["C1"].Value = "Economico";
                    mMayor.Cells["D1"].Value = "Modelo";
                    mMayor.Cells["E1"].Value = "Horometro Inicial";
                    mMayor.Cells["F1"].Value = "Horometro Final";
                    mMayor.Cells["G1"].Value = "Efectivo";
                    mMayor.Cells["H1"].Value = "Descripción";

                    mMayor.Cells[1, 1, 1, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    mMayor.Cells[1, 1, 1, 8].Style.Fill.BackgroundColor.SetColor(0, 169, 169, 169);
                    mMayor.Cells[1, 1, 1, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    mMayor.Cells[1, 1, 1, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    mMayor.Cells[1, 1, 1, 8].Style.Font.Size = 12;
                    mMayor.Cells[1, 1, 1, 8].AutoFitColumns();

                    int startRowDetalle = 2;
                    int endRowDetalle = 2;
                    int inicio = 1;
                    string ultimaObra = "";
                    foreach (var economico in listaEconomicosHrsDTO.OrderBy(x => x.nombreObra).ToList())
                    {
                        if (inicio == 1 || economico.nombreObra == ultimaObra)
                        {
                            mMayor.Cells[startRowDetalle, 1, endRowDetalle, 1].Value = economico.fechaInicio;
                            mMayor.Cells[startRowDetalle, 2, endRowDetalle, 2].Value = economico.fechaFin;
                            mMayor.Cells[startRowDetalle, 3, endRowDetalle, 3].Value = economico.noEconomico;
                            mMayor.Cells[startRowDetalle, 4, endRowDetalle, 4].Value = economico.modelo;
                            mMayor.Cells[startRowDetalle, 5, endRowDetalle, 5].Value = economico.horometroInicial;
                            mMayor.Cells[startRowDetalle, 6, endRowDetalle, 6].Value = economico.horometroFinal;
                            mMayor.Cells[startRowDetalle, 7, endRowDetalle, 7].Value = economico.efectivo;
                            mMayor.Cells[startRowDetalle, 8, endRowDetalle, 8].Value = economico.nombreObra.Trim();
                        }
                        else
                        {
                            mMayor.Cells[startRowDetalle, 1, endRowDetalle, 1].Value = "";
                            mMayor.Cells[startRowDetalle, 2, endRowDetalle, 2].Value = "";
                            mMayor.Cells[startRowDetalle, 3, endRowDetalle, 3].Value = "";
                            mMayor.Cells[startRowDetalle, 4, endRowDetalle, 4].Value = "";
                            mMayor.Cells[startRowDetalle, 5, endRowDetalle, 5].Value = "";
                            mMayor.Cells[startRowDetalle, 6, endRowDetalle, 6].Value = "Total de horas trabajadas";
                            mMayor.Cells[startRowDetalle, 6, endRowDetalle, 6].Style.Font.Bold = true;
                            mMayor.Cells[startRowDetalle, 7, endRowDetalle, 7].Value = listaEconomicosHrsDTO.Where(x => x.nombreObra == ultimaObra).Sum(y => y.efectivo);
                            mMayor.Cells[startRowDetalle, 7, endRowDetalle, 7].Style.Font.Bold = true;
                            mMayor.Cells[startRowDetalle, 7, endRowDetalle, 7].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            mMayor.Cells[startRowDetalle, 8, endRowDetalle, 8].Value = "";

                            startRowDetalle++;
                            endRowDetalle++;

                            mMayor.Cells[startRowDetalle, 1, endRowDetalle, 1].Value = economico.fechaInicio;
                            mMayor.Cells[startRowDetalle, 2, endRowDetalle, 2].Value = economico.fechaFin;
                            mMayor.Cells[startRowDetalle, 3, endRowDetalle, 3].Value = economico.noEconomico;
                            mMayor.Cells[startRowDetalle, 4, endRowDetalle, 4].Value = economico.modelo;
                            mMayor.Cells[startRowDetalle, 5, endRowDetalle, 5].Value = economico.horometroInicial;
                            mMayor.Cells[startRowDetalle, 6, endRowDetalle, 6].Value = economico.horometroFinal;
                            mMayor.Cells[startRowDetalle, 7, endRowDetalle, 7].Value = economico.efectivo;
                            mMayor.Cells[startRowDetalle, 8, endRowDetalle, 8].Value = economico.nombreObra.Trim();
                        }

                        if (inicio == listaEconomicosHrsDTO.OrderBy(x => x.nombreObra).ToList().Count)
                        {
                            startRowDetalle++;
                            endRowDetalle++;

                            mMayor.Cells[startRowDetalle, 1, endRowDetalle, 1].Value = "";
                            mMayor.Cells[startRowDetalle, 2, endRowDetalle, 2].Value = "";
                            mMayor.Cells[startRowDetalle, 3, endRowDetalle, 3].Value = "";
                            mMayor.Cells[startRowDetalle, 4, endRowDetalle, 4].Value = "";
                            mMayor.Cells[startRowDetalle, 5, endRowDetalle, 5].Value = "";
                            mMayor.Cells[startRowDetalle, 6, endRowDetalle, 6].Value = "Total de horas trabajadas";
                            mMayor.Cells[startRowDetalle, 6, endRowDetalle, 6].Style.Font.Bold = true;
                            mMayor.Cells[startRowDetalle, 7, endRowDetalle, 7].Value = listaEconomicosHrsDTO.Where(x => x.nombreObra == ultimaObra).Sum(y => y.efectivo);
                            mMayor.Cells[startRowDetalle, 7, endRowDetalle, 7].Style.Font.Bold = true;
                            mMayor.Cells[startRowDetalle, 7, endRowDetalle, 7].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            mMayor.Cells[startRowDetalle, 8, endRowDetalle, 8].Value = "";
                        }

                        mMayor.Column(4).Width = 25;
                        mMayor.Column(6).Width = 25;
                        mMayor.Column(8).Width = 35;

                        ultimaObra = economico.nombreObra;
                        startRowDetalle++;
                        endRowDetalle++;
                        inicio++;
                    }

                    package.Compression = CompressionLevel.BestSpeed;
                    List<byte[]> lista = new List<byte[]>();
                    var bytes = new MemoryStream();
                    using (var exportData = new MemoryStream())
                    {
                        package.SaveAs(exportData);
                        bytes = exportData;
                    }
                    return bytes;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public List<ComboDTO> obtenerCentrosCostos()
        {

            try
            {

                var listaCC = _context.tblP_CC.Where(x => x.estatus == true).Select(x => new ComboDTO
                {
                    Value = x.cc,
                    Text = x.cc + " - " + x.descripcion.Trim(),
                    Prefijo = x.cc
                }).OrderBy(x => x.Text).ToList();

                return listaCC;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool GuardarHorasComponente(List<tblM_CapHorometro> array) 
        {
            try
            {
                var trackActual = _context.tblM_trackComponentes.GroupBy(x => x.componenteID, (key, g) => g.OrderByDescending(e => e.fecha).ThenByDescending(e => e.id).FirstOrDefault());
                var trackActualServicios = _context.tblM_CatServicioOverhaul.Where(x => x.estatus == true);
                foreach (var item in array)
                {
                    var maquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.noEconomico == item.Economico);
                    if (maquina != null)
                    {
                        var esOverhauleable = _context.tblM_CatModeloEquipotblM_CatSubConjunto.Where(x => x.modeloID == maquina.modeloEquipoID).Count();
                        if (esOverhauleable > 0)
                        {
                            var componentes = trackActual.Where(x => x.locacionID == maquina.id).ToList();
                            componentes.ForEach(x => x.componente.horaCicloActual = x.componente.horaCicloActual + item.HorasTrabajo);
                            componentes.ForEach(x => x.componente.horasAcumuladas = x.componente.horasAcumuladas + item.HorasTrabajo);

                            var servicios = trackActualServicios.Where(x => x.maquinaID == maquina.id).ToList();
                            servicios.ForEach(x => x.horasCicloActual = x.horasCicloActual + item.HorasTrabajo);
                            _context.SaveChanges();
                        }
                    }
                }
                return true;
            }
            catch (Exception e) { return false; }
        }

        public List<tblM_CapHorometro> getHorometrosEconomicos(List<string> Economicos, DateTime min, DateTime max) 
        {
            List<tblM_CapHorometro> data = new List<tblM_CapHorometro>();
            var horometros = _context.tblM_CapHorometro.Where(x => Economicos.Contains(x.Economico) && x.Fecha >= min && x.Fecha <= max).ToList();
            if (horometros != null) {
                data = horometros;
            }
            return data;
        }

        public List<string> getUpdatedStandBy(List<string> lst,int tipo) 
        {
            #region ACTIVACION DE EQUIPO DE STANDBY
            var economicosActivados = new List<string>();
            foreach (var item in lst)
            {
                var activacion = ActivarEconomicoPorAccionRealizada
                (
                    item,
                    null,
                    tipo == 1 ? AccionActivacionEconomicoEnum.CAPTURA_HOROMETROS : AccionActivacionEconomicoEnum.CAPTURA_COMBUSTIBLE,
                    new { economico = item }, false);

                if (activacion)
                {
                    economicosActivados.Add(item);
                }
            }
            return economicosActivados;
            #endregion
            //List<string> r= new List<string>();
            //var data = _context.tblM_CatMaquina.Where(x => lst.Contains(x.noEconomico) && x.estatus == 2).ToList();

            //data.ForEach(x=>x.estatus=1);

            //_context.SaveChanges();
            //r.AddRange(data.Select(x=>x.noEconomico).ToList());
            //try
            //{
            //    foreach (var i in lst)
            //    {
            //        var o = _context.tblM_STB_CapturaStandBy.FirstOrDefault(x=>x.Economico.Equals(i) && x.estatus==2);
            //        if (o != null)
            //        {
            //            o.estatus = 4;
            //            o.usuarioLiberaID = vSesiones.sesionUsuarioDTO.id;
            //            o.fechaLibera = DateTime.Now;
            //            o.comentarioLiberacion = "LIBERACIÓN AUTOMATICA POR CAPTURA DE "+(tipo==1?"HOROMETRO":"COMBUSTIBLE");
            //            _context.SaveChanges();
            //        }
            //    }
            //}
            //catch (Exception e) {
            //}
            //return r;
        }

        private bool ActivarEconomicoPorAccionRealizada(string numeroEconomico, int? idEconomico, AccionActivacionEconomicoEnum accion, object objeto, bool buscarEnEnkontrol = false)
        {
            if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Arrendadora)
            {
                return false;
            }

            tblM_CatMaquina maquina = null;

            if (buscarEnEnkontrol)
            {
                if (!string.IsNullOrEmpty(numeroEconomico))
                {
                    var queryEk = new OdbcConsultaDTO();
                    queryEk.consulta = "SELECT * FROM cc WHERE cc = ?";
                    queryEk.parametros.Add(new OdbcParameterDTO
                    {
                        nombre = "cc",
                        tipo = OdbcType.NVarChar,
                        valor = numeroEconomico
                    });
                    var ccDescripcion = _contextEnkontrol.Select<dynamic>(vSesiones.sesionAmbienteEnkontrolAdm, queryEk).FirstOrDefault();

                    if (ccDescripcion != null)
                    {
                        numeroEconomico = (string)ccDescripcion.descripcion;
                    }
                }
                else
                {
                    throw new Exception("Se tiene que indicar un CC");
                }
            }

            if (!string.IsNullOrEmpty(numeroEconomico))
            {
                maquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.noEconomico == numeroEconomico && x.estatus == 2); //x.estatus == 2 significa que esta en StandBy
            }
            else if (idEconomico.HasValue)
            {
                maquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == idEconomico.Value && x.estatus == 2); //x.estatus == 2 significa que esta en StandBy
            }

            if (maquina != null)
            {
                if (_context.tblM_STB_EconomicoBloqueado.Any(x => x.noEconomico == maquina.noEconomico && x.registroActivo))
                {
                    throw new Exception("No es posible realizar la acción puesto que el equipo referenciado se encuentra bloqueado por estatus StandBy");
                }

                var standBy = _context.tblM_STB_CapturaStandBy
                    .FirstOrDefault(x =>
                        x.noEconomicoID == maquina.id &&
                        x.estatus == 2 //Autorizado
                    );

                if (standBy != null)
                {
                    string motivoLiberacion = "";
                    maquina.estatus = 1;
                    standBy.estatus = 4; //Liberado
                    standBy.usuarioLiberaID = vSesiones.sesionUsuarioDTO.id;
                    standBy.fechaLibera = DateTime.Now;
                    standBy.comentarioLiberacion = "Se liberó por sistema - ";
                    switch (accion)
                    {
                        case AccionActivacionEconomicoEnum.ELABORACION_REQUISICION:
                            standBy.comentarioLiberacion += "Se realizó una requisición";
                            motivoLiberacion = "elaboración de requisición";
                            break;
                        case AccionActivacionEconomicoEnum.ELABORACION_ORDEN_COMPRA:
                            standBy.comentarioLiberacion += "Se realizó una orden de compra";
                            motivoLiberacion = "elaboración de orden de compra";
                            break;
                        case AccionActivacionEconomicoEnum.CAPTURA_HOROMETROS:
                            standBy.comentarioLiberacion += "Se capturó horómetros";
                            motivoLiberacion = "captura de horómetros";
                            break;
                        case AccionActivacionEconomicoEnum.CAPTURA_COMBUSTIBLE:
                            standBy.comentarioLiberacion += "Se capturó combustible";
                            motivoLiberacion = "captura de combustible";
                            break;
                        case AccionActivacionEconomicoEnum.CAPTURA_ACEITE:
                            standBy.comentarioLiberacion += "Se capturó aceite";
                            motivoLiberacion = "captura de aceite";
                            break;
                        case AccionActivacionEconomicoEnum.RECEPCION_FACTURA:
                            standBy.comentarioLiberacion += "Por recepción de factura";
                            motivoLiberacion = "recepción de factura";
                            break;
                        case AccionActivacionEconomicoEnum.SALIDA_ALMACEN:
                            standBy.comentarioLiberacion += "Por salida de almacén";
                            motivoLiberacion = "salida de almacén";
                            break;
                    }

                    var bitacora = new tblM_STB_BitacoraActivacionEconomico();
                    bitacora.economicoId = maquina.id;
                    bitacora.fechaAccion = DateTime.Now;
                    bitacora.motivoActivacionId = (int)accion;
                    bitacora.usuarioAccionId = vSesiones.sesionUsuarioDTO.id;
                    bitacora.objeto = JsonUtils.convertNetObjectToJson(objeto);
                    _context.tblM_STB_BitacoraActivacionEconomico.Add(bitacora);
                    _context.SaveChanges();

                    var correos = new List<string>();
                    var correosCC = new List<string>();

                    var adminsGerentes = _context.Select<Core.DTO.Maquinaria.StandBy.AutorizanteDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT
                                                u.id,
                                                u.nombre,
                                                u.apellidoPaterno,
                                                u.apellidoMaterno,
                                                u.correo,
                                                c.cc as ac,
                                                a.perfilAutorizaID
                                            FROM
                                                tblP_Autoriza AS a
                                            INNER JOIN
                                                tblP_Usuario AS u ON u.id = a.usuarioID
                                            INNER JOIN
                                                tblP_CC_Usuario AS c ON c.id = a.cc_usuario_ID
                                            WHERE
                                                u.estatus = 1 AND
                                                a.perfilAutorizaID in (5, 1) AND /*5 == Admin, 1 == Gerente*/
                                                c.cc = @paramCC",
                        parametros = new { paramCC = standBy.ccActual }
                    });

                    correosCC.AddRange(adminsGerentes.Select(x => x.correo).Distinct().ToList());

                    //correos.Add("jose.gaytan@construplan.com.mx");
                    correos.Add("oscar.roman@construplan.com.mx");
                    correosCC.Add("g.reina@construplan.com.mx");
                    correosCC.Add("e.encinas@construplan.com.mx");
                    correosCC.Add("luis.fortino@construplan.com.mx");
                    //correosCC.Add("oscar.roman@construplan.com.mx");
                    correosCC.Add("martin.valle@construplan.com.mx");
                    correosCC.Add("alan.palomera@construplan.com.mx");
                    correosCC.Add("diego.gonzalez@construplan.com.mx");
#if DEBUG
                    correos = new List<string> { "martin.zayas@construplan.com.mx" };
                    correosCC = new List<string> { "martin.zayas@construplan.com.mx" };
#endif
                    var cc = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == maquina.centro_costos);
                    var ccDescripcion = cc != null ? cc.descripcion.Trim() : maquina.centro_costos;
                    string asunto = "El equipo " + maquina.noEconomico + " ha sido liberado de StandBy por " + motivoLiberacion;
                    string contenido = string.Format(@"
                        <p>Buen día.</p>
                        <p>El equipo <strong>{0}</strong> ha sido liberado de StandBy por {1} </p>
                        <p>El equipo se encuentra en <strong>{2}.</strong>", maquina.noEconomico, motivoLiberacion, ccDescripcion);

                    var envioCorrecto = EnviarCorreo(new Infrastructure.DTO.CorreoDTO
                    {
                        asunto = asunto,
                        cuerpo = contenido,
                        correos = correos,
                        correosCC = correosCC
                    });

                    if (!envioCorrecto)
                    {
                        throw new Exception("Error al enviar correo de liberación de StandBy");
                    }

                    return true;
                }
                else
                {
                    throw new Exception("El económico esta en StandBy pero no se encuentra su registro autorizado");
                }
            }

            return false;
        }

        private bool EnviarCorreo(Infrastructure.DTO.CorreoDTO correo)
        {
            if (correo.correos == null || correo.correos.Count == 0 || string.IsNullOrEmpty(correo.asunto) || string.IsNullOrEmpty(correo.cuerpo))
            {
                return false;
            }

            MailMessage mailMessage = new MailMessage();

            correo.correos.ForEach(c => mailMessage.To.Add(new MailAddress(c)));
            correo.correosCC.ForEach(c => mailMessage.CC.Add(new MailAddress(c)));
            correo.archivos.ForEach(archivo => mailMessage.Attachments.Add(archivo));

            mailMessage.IsBodyHtml = true;
            mailMessage.From = new MailAddress("alertas.sigoplan@construplan.com.mx");
            mailMessage.Subject = correo.asunto;
            mailMessage.Body = string.Format(@"
                {0} 
                <p><o:p>&nbsp;</o:p></p>
                <p><o:p>&nbsp;</o:p></p>
                <p>Se informa que esta es un correo autogenerado por el sistema SIGOPLAN (<a href='http://sigoplan.construplan.com.mx/'>http://sigoplan.construplan.com.mx</a>). No es necesario dar una respuesta. Gracias.</p>
            ", correo.cuerpo);

            SmtpClient smptConfig = new SmtpClient();
            smptConfig.Send(mailMessage);
            smptConfig.Dispose();

            return true;
        }

        public bool getCorteKubrixAC(string ac) 
        {
            try
            {
                var cc = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == ac);
                var fechaFin = DateTime.Today;
                while (fechaFin.DayOfWeek != DayOfWeek.Wednesday)
                    fechaFin = fechaFin.AddDays(+1);
                var fechaInicio = fechaFin.AddDays(-8);
                var guardado = _context.tblM_Hor_CorteKubrix.Where(x => x.acID == cc.id && x.fechaCorte == fechaFin).ToList();
                return guardado.Count() > 0 ? true : false;
            }
            catch (Exception e) 
            {
                return false;
            }
        }

        public bool GuardarCorteKubrixAC(string ac) 
        {
            try
            {
                tblM_Hor_CorteKubrix nuevo = new tblM_Hor_CorteKubrix();
                var cc = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == ac);
                var fechaFin = DateTime.Today;
                while (fechaFin.DayOfWeek != DayOfWeek.Wednesday)
                    fechaFin = fechaFin.AddDays(+1);

                nuevo.acID = cc.id;
                nuevo.fechaCorte = fechaFin;
                nuevo.fechaCaptura = DateTime.Now;
                nuevo.usuarioCaptura = vSesiones.sesionUsuarioDTO.id;

                _context.tblM_Hor_CorteKubrix.Add(nuevo);
                _context.SaveChanges();               

                return true;
            }
            catch (Exception e) 
            {
                return false;
            }
        }



        public byte[] GetReporteHorometrosKubrix(string ac)
        {
            try
            {
                var cc = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == ac);
                var fechaFin = DateTime.Today;
                while (fechaFin.DayOfWeek != DayOfWeek.Wednesday)
                    fechaFin = fechaFin.AddDays(+1);
                var fechaInicio = fechaFin.AddDays(-8);
                var guardado = _context.tblM_Hor_CorteKubrix.Where(x => x.acID == cc.id && x.fechaCorte == fechaFin).ToList();
                if (guardado.Count() > 0)
                {
                    var horometros = _context.tblM_CapHorometro.Where(x => x.CC == ac && x.Fecha > fechaInicio && x.Fecha < fechaFin).ToList();

                    using (ExcelPackage excel = new ExcelPackage())
                    {
                        var hoja1 = excel.Workbook.Worksheets.Add(cc.areaCuenta + " " + cc.descripcion);
                        List<object> auxCell = new List<object>();

                        hoja1.Cells.Style.Font.Name = "Arial";
                        hoja1.Cells.Style.Font.Size = 10;
                        hoja1.Column(1).Style.Font.Size = hoja1.Column(2).Style.Font.Size = hoja1.Column(3).Style.Font.Size = hoja1.Column(4).Style.Font.Size = hoja1.Column(5).Style.Font.Size = hoja1.Column(6).Style.Font.Size = 11;
                        hoja1.Column(1).Style.Font.Bold = hoja1.Column(2).Style.Font.Bold = hoja1.Column(3).Style.Font.Bold = hoja1.Column(4).Style.Font.Bold = hoja1.Column(5).Style.Font.Bold = hoja1.Column(6).Style.Font.Bold = true;
                        hoja1.Column(2).Style.Numberformat.Format = hoja1.Column(3).Style.Numberformat.Format = "0.00";
                        hoja1.Column(4).Style.Numberformat.Format = "dd/mm/yyyy";
                        hoja1.Column(6).Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss AM/PM";

                        var auxHeaders = new List<string> { "Económico", "Horas Trabajo", "Horometro", "Fecha", "Turno", "Fecha Captura" };
                        List<string[]> headerRow = new List<string[]>() { auxHeaders.ToArray() };
                        string headerRange = "A1:F1";
                        hoja1.Cells[headerRange].LoadFromArrays(headerRow);

                        var cellData = new List<object[]>();

                        foreach (var movimiento in horometros)
                        {
                            cellData.Add(new object[]
                            {
                                movimiento.Economico,
                                movimiento.HorasTrabajo,
                                movimiento.Horometro,
                                movimiento.Fecha,
                                movimiento.turno,
                                movimiento.FechaCaptura,
                            });
                        }

                        hoja1.Cells[2, 1].LoadFromArrays(cellData);

                        excel.Compression = CompressionLevel.BestSpeed;
                        hoja1.Cells.AutoFitColumns();
                        hoja1.Cells[headerRange].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        byte[] binaryData = null;

                        using (var exportData = new MemoryStream())
                        {
                            excel.SaveAs(exportData);
                            binaryData = exportData.ToArray();
                        }

                        return binaryData;
                    }
                }
                else {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }            
        }

        public List<string> GetCorreoGerenteAdmin(string CC) 
        {
            List<string> correos = new List<string>();
            var auxCC_Usuario = _context.tblP_CC_Usuario.Where(x => x.cc == CC).Select(x => x.id).ToList();
            var correosGerentes = _context.tblP_Autoriza.Where(x => x.perfilAutorizaID == 8 && auxCC_Usuario.Contains(x.cc_usuario_ID)).Select(x => x.usuario.correo).ToList();
            foreach(var item in correosGerentes)
            {
                if(item != null && item != "") correos.Add(item);
            }
            var correosAdministradores = _context.tblP_Autoriza.Where(x => x.perfilAutorizaID == 5 && auxCC_Usuario.Contains(x.cc_usuario_ID)).Select(x => x.usuario.correo).ToList();
            foreach(var item in correosAdministradores)
            {
                if(item != null && item != "") correos.Add(item);
            }

            var extras = _context.tblM_Hor_UsuarioExtraEnCorreo.Where(x => x.cc == CC).ToList();

            foreach (var item in extras)
            {
                correos.Add(item.usuario.correo);
            }

            return correos;
        }
        
    }
}
