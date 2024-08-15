using Core.DAO.Maquinaria.Captura;
using Core.DTO;
using Core.DTO.Captura;
using Core.DTO.Maquinaria.Captura;
using Core.DTO.Maquinaria.Reporte;
using Core.DTO.Reportes.Capturas;
using Core.Entity.Maquinaria;
using Core.Entity.Maquinaria.Captura;
using Core.Entity.Maquinaria.Catalogo;
using Core.Enum.Multiempresa;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Captura
{
    public class CapturaCombustibleDAO : GenericDAO<tblM_CapCombustible>, ICapturaCombustiblesDAO
    {
        public List<capCombusitbleDTO> getDataTable(string cc, int turno, DateTime fecha, int idTipo)
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

            List<capCombusitbleDTO> resultado = new List<capCombusitbleDTO>();
            DateTime FechaActual = DateTime.Now.AddDays(-1);

            var listaMaquinasBloqueoStandby = _context.tblM_STB_EconomicoBloqueado.Where(x => x.registroActivo).Select(x => x.noEconomico).ToList();

            var capCombusitble = (from c in _context.tblM_CatMaquina
                                  where c.centro_costos.Equals(cc.ToString()) &&
                (idTipo == 0 ? c.id == c.id : c.grupoMaquinaria.tipoEquipoID.Equals(idTipo)) && c.TipoCaptura != 0 && c.estatus != 0
                && !listaMaquinasBloqueoStandby.Contains(c.noEconomico)

                                  select c).ToList();
            var getCapturados = _context.tblM_CapCombustible.Where(x=>x.fecha == fecha && x.CC == cc && x.turno==turno).ToList();
            //var getCapturados = (from d in _context.tblM_CapCombustible
            //                     join h
            //                         in _context.tblM_CatMaquina on d.Economico equals h.noEconomico
            //                     where d.fecha == fecha && h.centro_costos == cc.ToString() &&  h.estatus != 0
            //                     select d).ToList();



            //foreach (string eco in listaMaquinas)
            //{
            //    var valid = capCombusitble.Exists(x => x.noEconomico.Equals(eco));

            //    if (!valid)
            //    {
            //        resultado.Add(new capCombusitbleDTO
            //        {
            //            id = 0,
            //            Carga1 = 0,
            //            Carga2 = 0,
            //            Carga3 = 0,
            //            Carga4 = 0,
            //            HorometroCarga1 = 0,
            //            HorometroCarga2 = 0,
            //            HorometroCarga3 = 0,
            //            HorometroCarga4 = 0,
            //            CC = 0,
            //            Economico = eco,
            //            fecha = FechaActual,
            //            surtidor = "",
            //            turno = 1,
            //            volumen_carga = 0,
            //            capacidadCarga = 0,
            //            aplicarCosto = false,
            //            pipa1 = "",
            //            pipa2 = "",
            //            pipa3 = "",
            //            pipa4 = ""
            //        });
            //    }
            //}

            foreach (var item in capCombusitble)
            {
                var economico = item.noEconomico;
                var maquina = getCapturados.FirstOrDefault(x => x.Economico.Equals(economico) && x.turno == turno && x.fecha.Equals(fecha));
                var capacidad = item.capacidadTanque;


                if (maquina != null)
                {
                    resultado.Add(new capCombusitbleDTO
                    {
                        id = maquina.id,
                        Carga1 = maquina.Carga1,
                        Carga2 = maquina.Carga2,
                        Carga3 = maquina.Carga3,
                        Carga4 = maquina.Carga4,
                        HorometroCarga1 = maquina.HorometroCarga1,
                        HorometroCarga2 = maquina.HorometroCarga2,
                        HorometroCarga3 = maquina.HorometroCarga3,
                        HorometroCarga4 = maquina.HorometroCarga4,
                        CC = maquina.CC,
                        Economico = maquina.Economico,
                        fecha = maquina.fecha,
                        surtidor = maquina.surtidor,
                        turno = maquina.turno,
                        volumen_carga = maquina.volumne_carga,
                        capacidadCarga = item.capacidadTanque,
                        PrecioLitro = maquina.PrecioLitro,
                        PrecioTotal = maquina.PrecioTotal,
                        aplicarCosto = maquina.aplicarCosto,
                        pipa1 = maquina.pipa1,
                        pipa2 = maquina.pipa2,
                        pipa3 = maquina.pipa3,
                        pipa4 = maquina.pipa4
                    });
                }
                else
                {
                    resultado.Add(new capCombusitbleDTO
                    {
                        id = 0,
                        Carga1 = 0,
                        Carga2 = 0,
                        Carga3 = 0,
                        Carga4 = 0,
                        HorometroCarga1 = 0,
                        HorometroCarga2 = 0,
                        HorometroCarga3 = 0,
                        HorometroCarga4 = 0,
                        CC = "",
                        Economico = item.noEconomico,
                        fecha = FechaActual,
                        surtidor = "",
                        turno = 1,
                        volumen_carga = 0,
                        capacidadCarga = item.capacidadTanque,
                        aplicarCosto = false,
                        pipa1 = "",
                        pipa2 = "",
                        pipa3 = "",
                        pipa4 = ""
                    });
                }
            }

            return resultado;
        }
        public void Guardar(List<tblM_CapCombustible> array)
        {

            foreach (tblM_CapCombustible obj in array)
            {
                obj.surtidor = "";
                obj.Economico = obj.Economico.TrimEnd('*');

                if (true)
                {
                    if (obj.id == 0)
                        SaveEntity(obj, (int)BitacoraEnum.RITMOHOROMETRO);
                    else
                        Update(obj, obj.id, (int)BitacoraEnum.RITMOHOROMETRO);
                }
                else
                {
                    throw new Exception("Error ocurrio un error al insertar un registro");
                }
            }
        }
        private List<string> Maquinas(int cc)
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
        public List<EcoPipaDTO> FillCboPipa(string cc)
        {
            // List<string> listaMaquinas = Maquinas(cc);


            var result = _context.tblM_CatPipa.OrderBy(x => x.maquinaID).Select(x => new EcoPipaDTO { value = x.noEconomico, descripcion = x.noEconomico }).ToList();

            //var result = (from p in _context.tblM_CatMaquina
            //              where p.grupoMaquinariaID == 225 //|| p.noEconomico == "TA-52" || p.noEconomico == "TA-59" || p.noEconomico == "TA-58"
            //              select new EcoPipaDTO { value = p.noEconomico, descripcion = p.noEconomico }).ToList();
            //result.Add(new EcoPipaDTO { value = "Pipa Mina NB", descripcion = "Pipa Mina NB" });
            //result.Add(new EcoPipaDTO { value = "Tanque estacionario Mina Subterránea", descripcion = "Tanque estacionario Mina Subterránea" });

            return result;
        }
        public string getNombreCC(string cc)
        {
            string centro_costos = "";

            List<string> lista = new List<string>();

            centro_costos = "SELECT descripcion FROM cc WHERE cc = '" + cc + "'";


            var resultado = (IList<economicoDTO>)_contextEnkontrol.Where(centro_costos).ToObject<IList<economicoDTO>>();

            string nombre_CC = resultado.Select(x => x.descripcion).FirstOrDefault();
            return nombre_CC;
        }
        public List<InfoCombustibleDTO> getDataReporteCombustibleMensual(string cc, DateTime pfecha)
        {
            List<InfoCombustibleDTO> resultado = new List<InfoCombustibleDTO>();
            //var raw1 = _context.tblM_CapCombustible.Join(
            //    _context.tblM_CatMaquina,
            //    comb => comb.Economico,
            //    mq => mq.noEconomico,
            //    (comb, mq) =>
            //    new
            //    {
            //        maquin = mq,
            //        combusti = comb
            //    }
            //    ).Where(x => x.combusti.fecha.Month.Equals(pfecha.Month) && x.combusti.CC.Equals(cc) && x.combusti.fecha.Year.Equals(pfecha.Year))
            //    .Select(x => new InfoCombustibleDTO
            //    {
            //        noEconomico = x.maquin.noEconomico,
            //        noSerie = x.maquin.noSerie,
            //        fecha = x.combusti.fecha,
            //        carga1 = x.combusti.Carga1,
            //        carga2 = x.combusti.Carga2,
            //        carga3 = x.combusti.Carga3,
            //        carga4 = x.combusti.Carga4,
            //        total = x.combusti.Carga1 + x.combusti.Carga2 + x.combusti.Carga3 + x.combusti.Carga4,
            //        descripcion = x.maquin.grupoMaquinaria != null ? x.maquin.grupoMaquinaria.descripcion : ""
            //    }).ToList();


            var raw = _context.tblM_CapCombustible.Where(x => x.fecha.Month.Equals(pfecha.Month) && x.CC.Equals(cc) && x.fecha.Year.Equals(pfecha.Year)).ToList();

            foreach (var x in raw)
            {

                var maquina = _context.tblM_CatMaquina.FirstOrDefault(y => y.noEconomico == x.Economico);

                string descripcion = "";
                string serie = "";

                if (maquina != null)
                {
                    serie = maquina.noSerie;
                    descripcion = maquina.descripcion;
                }

                InfoCombustibleDTO data = new InfoCombustibleDTO
                    {
                        noEconomico = x.Economico,
                        noSerie = serie,
                        fecha = x.fecha,
                        carga1 = x.Carga1,
                        carga2 = x.Carga2,
                        carga3 = x.Carga3,
                        carga4 = x.Carga4,
                        total = x.Carga1 + x.Carga2 + x.Carga3 + x.Carga4,
                        descripcion = descripcion
                    };

                resultado.Add(data);
            }


            return resultado.ToList();
        }

        public List<dataRepRendimientoCombustible> getReporteRendimientoComb(string cc, DateTime fInicio, DateTime fFin)
        {

            List<dataRepRendimientoCombustible> result = new List<dataRepRendimientoCombustible>();

            var rawHorometros = (_context.tblM_CapHorometro.Where(x => x.CC.Equals(cc)
                                                              && x.Fecha >= fInicio && x.Fecha <= fFin)).ToList();

            var resultadoEconomicos = rawHorometros.GroupBy(x => x.Economico).Select(y => y.Key.ToString()).ToList();

            var rawEconomicos = _context.tblM_CatMaquina.Where(x => resultadoEconomicos.Contains(x.noEconomico)).ToList();

            var rawCombustible = _context.tblM_CapCombustible.Where(x => x.CC.Equals(cc)
                                                              && x.fecha >= fInicio && x.fecha <= fFin).ToList();

            var ListaEconomicos = rawCombustible.Select(x => x.Economico).ToList();

            ListaEconomicos.AddRange(resultadoEconomicos);

            decimal TotalHorasTrabajo = 0;

            foreach (var economico in ListaEconomicos.Distinct())
            {

                List<string> Row = new List<string>();
                var noEconomico = _context.tblM_CatMaquina.Where(x => x.noEconomico.Equals(economico)).FirstOrDefault();

                if (noEconomico != null)
                {
                    var Horometro = rawHorometros.Where(y => y.Economico.Equals(economico));

                    var combustible = rawCombustible.Where(x => x.Economico.Equals(economico));
                    var rawRendimiento = _context.tblM_CatRendimientoTeorico.FirstOrDefault(x => x.modeloEquipoID.Equals(noEconomico.modeloEquipoID));

                    string alto = "";
                    string medio = "";
                    string bajo = "";
                    if (rawRendimiento != null)
                    {
                        alto = rawRendimiento.alto;
                        medio = rawRendimiento.medio;
                        bajo = rawRendimiento.bajo;
                    }


                    decimal minHorometro = 0;
                    decimal maxHorometro = 0;
                    decimal TotalComsumo = 0;
                    decimal horasTrabajo = 0;
                    if (Horometro.Count() > 0)
                    {

                        minHorometro = Horometro.OrderBy(y => y.Horometro).FirstOrDefault().Horometro - Horometro.OrderBy(y => y.Horometro).FirstOrDefault().HorasTrabajo;
                        maxHorometro = Horometro.OrderByDescending(y => y.Horometro).FirstOrDefault().Horometro;
                        horasTrabajo = maxHorometro - minHorometro;//Horometro.Sum(x => x.HorasTrabajo);

                        var horasdd = maxHorometro - minHorometro;
                    }
                    if (combustible != null && combustible.Count() > 0)
                    {
                        TotalComsumo = combustible.Sum(x => x.Carga1) + combustible.Sum(x => x.Carga2) + combustible.Sum(x => x.Carga3) + combustible.Sum(x => x.Carga4);
                    }

                    var rendimiento = horasTrabajo == 0 ? 0 : TotalComsumo / horasTrabajo;
                    TotalHorasTrabajo += horasTrabajo;


                    string descripcion = "";
                    string marca = "";
                    string modelo = "";
                    int capacidad = 0;
                    if (noEconomico != null)
                    {
                        if (noEconomico.marca != null)
                        {
                            marca = noEconomico.marca.descripcion;
                        }
                        if (noEconomico.grupoMaquinaria != null)
                        {
                            descripcion = noEconomico.grupoMaquinaria.descripcion;
                        }
                        if (noEconomico.modeloEquipo != null)
                        {
                            modelo = noEconomico.modeloEquipo.descripcion;
                        }
                        if (!string.IsNullOrEmpty(noEconomico.capacidadTanque.ToString()))
                        {
                            capacidad = (int)noEconomico.capacidadTanque;
                        }

                    }

                    var dato = rendimiento.ToString("N2");

                    result.Add(new dataRepRendimientoCombustible
                    {
                        Economico = economico,
                        Descripcion = descripcion,
                        Marca = marca,
                        Modelo = modelo,
                        HInicial = minHorometro,
                        Hfinal = maxHorometro,
                        HTrabajadas = horasTrabajo,
                        ConsumoLTS = TotalComsumo,
                        RendimientoLTS = rendimiento.ToString("N2"),
                        Capacidad = capacidad,
                        RendimientoTeorico = 0,
                        alto = alto,
                        medio = medio,
                        bajo = bajo
                    });
                }
            }

            return result;
        }


        public List<tblM_CapCombustible> getTableInfoCombustibles(string cc, int turno, DateTime fechaInicia, DateTime fechaFinal, string economico)
        {

            var resultado = (from c in _context.tblM_CapCombustible
                             where (cc == "0" ? c.id.Equals(c.id) : c.CC.Equals(cc))

                             && (turno != 0 ? c.turno.Equals(turno) : c.turno.Equals(c.turno)) &&
                                   c.fecha >= fechaInicia && c.fecha <= fechaFinal &&
                                   (string.IsNullOrEmpty(economico) ? c.Economico.Equals(c.Economico) : c.Economico.Equals(economico))
                             select c).OrderBy(x => x.fecha).ThenBy(x => x.Economico).ToList();
            return resultado;
        }


        public List<tblM_CapCombustible> getConsumoCombustibles(string cc, DateTime fechaFin, DateTime fechaInicio, string economico, List<int> lstTipoMaquinaria)
        {
            List<tblM_CapCombustible> lstResultado = new List<tblM_CapCombustible>();
            if (cc.Equals("114") || cc.Equals("148"))
            {
                lstResultado.AddRange(getConsumoCombustiblesQuery("114", fechaFin, fechaInicio, economico, lstTipoMaquinaria));
                lstResultado.AddRange(getConsumoCombustiblesQuery("148", fechaFin, fechaInicio, economico, lstTipoMaquinaria));
            }
            else
            {
                lstResultado.AddRange(getConsumoCombustiblesQuery(cc, fechaFin, fechaInicio, economico, lstTipoMaquinaria));
            }
            return lstResultado;

        }

        private List<tblM_CapCombustible> getConsumoCombustiblesQuery(string cc, DateTime fechaFin, DateTime fechaInicio, string economico, List<int> lstTipoMaquinaria)
        {
            string objCC = cc;
            try
            {
                List<tblM_CapCombustible> lstResultado = new List<tblM_CapCombustible>();
                foreach (int tipoMaquinaria in lstTipoMaquinaria)
                {
                    var resultado = (from c in _context.tblM_CapCombustible
                                     join m in _context.tblM_CatMaquina on c.Economico equals m.noEconomico
                                     join gm in _context.tblM_CatGrupoMaquinaria on m.grupoMaquinariaID equals gm.id
                                     join tm in _context.tblM_CatTipoMaquinaria on gm.tipoEquipoID equals tm.id
                                     where c.CC == objCC && c.fecha >= fechaInicio && c.fecha <= fechaFin && gm.tipoEquipoID == tipoMaquinaria
                                     && c.Economico == (string.IsNullOrEmpty(economico) ? c.Economico : economico)
                                     select c).OrderBy(x => x.Economico).ToList();
                    lstResultado.AddRange(resultado.ToList());
                }


                return lstResultado;
            }
            catch
            {
                return new List<tblM_CapCombustible>();
            }
        }


        public List<totalDieselEnkontrolDTO> getTotalEnkontrolConsumoDiesel(string cc)
        {
            var res1 = cc.Equals("148") ? getTotalEnkontrolConsumoDieselQuery("114") : getTotalEnkontrolConsumoDieselQuery(cc);
            return res1;
        }

        public List<totalDieselEnkontrolDTO> getTotalEnkontrolConsumoDieselQuery(string cc)
        {
            try
            {
                string consulta = " SELECT * from(" +
                                    " SELECT  f.descripcion as Economico, B.IMPORTE " +
                                    " FROM insumos a" +
                                    " INNER JOIN si_movimientos_det b ON a.insumo=b.insumo  " +
                                    " inner join si_area_cuenta f on f.area= b.area and f.cuenta = b.cuenta" +
                                    " INNER JOIN si_movimientos c ON b.numero=c.numero AND b.tipo_mov=c.tipo_mov AND b.almacen=c.almacen " +
                                    " INNER JOIN grupos_insumo d on d.grupo_insumo = a.grupo " +
                                    " INNER JOIN tipos_insumo e on e.tipo_insumo = a.tipo where b.tipo_mov = 51" +
                                    " and c.cc ='" + cc + "'" +
                                    " and ((d.tipo_insumo = 6 and d.grupo_insumo = 15)or(d.tipo_insumo = 1 and d.grupo_insumo = 18))" +
                                    " and f.centro_costo='" + cc + "' AND b.importe > 0" +
                                    " UNION ALL" +
                                    " SELECT  f.descripcion as Economico, B.IMPORTE " +
                                    " FROM insumos a" +
                                    " INNER JOIN so_movimientos_noinv_det b ON a.insumo=b.insumo  " +
                                    " inner join si_area_cuenta f on f.area= b.area and f.cuenta = b.cuenta" +
                                    " INNER JOIN so_movimientos_noinv c ON b.remision=c.remision AND b.tipo_mov=c.tipo_mov AND b.almacen=c.almacen " +
                                    " INNER JOIN grupos_insumo d on d.grupo_insumo = a.grupo " +
                                    " INNER JOIN tipos_insumo e on e.tipo_insumo = a.tipo where b.tipo_mov = 51" +
                                    " and c.cc ='" + cc + "'" +
                                    " and ((d.tipo_insumo = 6 and d.grupo_insumo = 15)or(d.tipo_insumo = 1 and d.grupo_insumo = 18))" +
                                    " and f.centro_costo='" + cc + "' AND b.importe > 0" +
                                    " ) AS X order by Economico;";

                var res1 = (IList<totalDieselEnkontrolDTO>)_contextEnkontrol.Where(consulta).ToObject<IList<totalDieselEnkontrolDTO>>();

                return res1.ToList();
            }
            catch { return new List<totalDieselEnkontrolDTO>(); }
        }

        public decimal getTotalContratistaConsumoDiesel(string cc, DateTime fechaFin, DateTime fechaInicio)
        {

            try
            {
                string consulta = "select sum(monto) as TotalDieselContratista" +
                     " from sc_movpol" +
                     " where cta=5000" +
                     " and scta=4" +
                     " and sscta=2" +
                     " and tp='08'" +
                     " and cc = '" + cc + "'  " +
                     " and year >= " + fechaInicio.Year +
                     " and mes >= " + fechaInicio.Month +
                     " and year <= " + fechaFin.Year +
                     " and mes <= " + fechaFin.Month + ";";

                var res1 = (IList<totalDieselContratistaDTO>)_contextEnkontrol.Where(consulta).ToObject<IList<totalDieselContratistaDTO>>();

                return res1.FirstOrDefault().TotalDieselContratista;

            }

            catch
            {
                return 0;
            }

        }


        public decimal getTotalMaquinaEnkontrolConsumoDiesel(string cc, string economico)
        {
            try
            {
                //string consulta = " Select  area, cuenta, 0 from si_area_cuenta where descripcion='" + economico + "' and centro_costo = '" + cc + "'";

                //var res1 = (IList<totalMaquinaConsumoDieselEnkontrolDTO>)_contextEnkontrol.Where(consulta).ToObject<IList<totalMaquinaConsumoDieselEnkontrolDTO>>();

                string consulta = "Select  0, 0, sum(x.total) as total from (" +
                            " Select a.cc, b.area, b.cuenta, b.partida, b.insumo, a.total" +
                            " from si_movimientos as a" +
                            " inner join si_movimientos_det as b on a.almacen = b.almacen and a.tipo_mov = b.tipo_mov and a.numero = b.numero" +
                            " inner join si_area_cuenta as c on c.area = b.area and c.cuenta = b.cuenta" +
                            " where a.cc='" + cc + "'  and c.descripcion ='" + economico + "' and c.centro_costo = '" + cc + "' AND b.tipo_mov = 51) as x" +
                            " inner join insumos as y on x.insumo = y.insumo" +
                            " where (y.tipo = 6 and y.grupo = 15)or(y.tipo = 1 and y.grupo = 18)";

                var res1 = (IList<totalMaquinaConsumoDieselEnkontrolDTO>)_contextEnkontrol.Where(consulta).ToObject<IList<totalMaquinaConsumoDieselEnkontrolDTO>>();

                return Convert.ToDecimal(res1.FirstOrDefault().total);
            }
            catch { return 0; }
        }
    }
}
