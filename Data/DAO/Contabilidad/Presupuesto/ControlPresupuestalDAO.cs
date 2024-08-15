using Core.DAO.Contabilidad.Presupuesto;
using Core.DTO;
using Core.DTO.Contabilidad.ControlPresupuestal;
using Core.DTO.Contabilidad.Presupuesto;
using Core.DTO.Principal.Generales;
using Core.DTO.Utils.Data;
using Core.Entity.Maquinaria._Caratulas;
using Core.Entity.Principal.Usuarios;
using Core.Enum.Contabilidad.ControlPresupuestal;
using Core.Enum.Multiempresa;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Odbc;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Contabilidad.Presupuesto
{
    public class ControlPresupuestalDAO : GenericDAO<tblP_Usuario>, IControlPresupuestalDAO
    {
        private Dictionary<string, object> resultado = new Dictionary<string, object>();
        private const string NombreControlador = "CapacitacionController";
        public Dictionary<string, object> getComboEconomicos(string AreaCuenta, int? modelo)
        {
            if (AreaCuenta == null)
            {
                AreaCuenta = "";
            }
            try
            {
                var listaEconomicos = new List<ComboDTO>();
                if (modelo.HasValue && !string.IsNullOrEmpty(AreaCuenta))
                {
                    listaEconomicos = _context.tblM_CatMaquina.Where(x => x.noEconomico != "" && (AreaCuenta == "" ? x.centro_costos == x.centro_costos : x.centro_costos == AreaCuenta)
                                                                                              && (modelo == 0 ? x.modeloEquipoID == x.modeloEquipoID : x.modeloEquipoID == modelo)).Select(x => new ComboDTO
                    {
                        Value = x.id.ToString(),
                        Text = x.noEconomico
                    }).OrderBy(x => x.Text).ToList();
                }

                var lst = listaEconomicos.Where(r => r.Text != null).ToList();
                resultado.Add(ITEMS, lst);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "getComboEconomicos", e, AccionEnum.CONSULTA, 0, null);
            }

            return resultado;
        }
        public Dictionary<string, object> getComboEconomicosMultiple(string AreaCuenta, List<int> listaModelo)
        {
            try
            {
                var listaEconomicos = _context.tblM_CatMaquina.Where(x =>
                    x.noEconomico != "" &&
                    x.noEconomico != null &&
                    ((AreaCuenta != "" && AreaCuenta != null) ? x.centro_costos == AreaCuenta : true)).ToList().Where(x =>
                    (listaModelo != null ? listaModelo.Contains(x.modeloEquipoID) : true)
                ).Select(x => new ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = x.noEconomico
                }).OrderBy(x => x.Text).ToList();

                resultado.Add(ITEMS, listaEconomicos);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "getComboEconomicosMultiple", e, AccionEnum.CONSULTA, 0, null);
            }

            return resultado;
        }
        public Dictionary<string, object> FillCboModeloEquipo(int? idGrupo)
        {
            try
            {
                var listaEconomicos = new List<ComboDTO>();
                if (idGrupo.HasValue)
                {
                    listaEconomicos = _context.tblM_CatModeloEquipo.Where(x => x.estatus && (idGrupo == 0 ? x.idGrupo == x.idGrupo : x.idGrupo == idGrupo)).Select(x => new ComboDTO
                    {
                        Value = x.id.ToString(),
                        Text = x.descripcion
                    }).OrderBy(x => x.Text).ToList();
                }

                var lst = listaEconomicos.Where(r => r.Text != null).ToList();
                resultado.Add(ITEMS, lst);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "getComboEconomicos", e, AccionEnum.CONSULTA, 0, null);
            }

            return resultado;
        }
        public Dictionary<string, object> FillCboModeloEquipoMultiple(List<int> listaGrupos)
        {
            try
            {
                var listaModelos = new List<ComboDTO>();

                if (listaGrupos != null)
                {
                    listaModelos = _context.tblM_CatModeloEquipo.Where(x => x.estatus && x.idGrupo != null).ToList().Where(x => listaGrupos.Contains((int)x.idGrupo)).Select(x => new ComboDTO
                    {
                        Value = x.id.ToString(),
                        Text = x.descripcion
                    }).OrderBy(x => x.Text).ToList();
                }

                var lst = listaModelos.Where(r => r.Text != null).ToList();

                resultado.Add(ITEMS, lst);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "FillCboModeloEquipoMultiple", e, AccionEnum.CONSULTA, 0, null);
            }

            return resultado;
        }
        public List<ComboDTO> obtenerGruposMaquinaria(int idTipo)
        {
            List<ComboDTO> lstDatos = new List<ComboDTO>();
            lstDatos = _context.tblM_CatGrupoMaquinaria.Where(r => r.tipoEquipoID == idTipo && r.estatus).ToList().Select(y => new ComboDTO
            {
                Value = y.id.ToString(),
                Text = y.descripcion
            }).ToList();
            return lstDatos;
        }
        public Dictionary<string, object> cargarControlPresupuestal(FiltrosControlPresupuestalDTO filtros)
        {
            try
            {
                #region Filtrar CatMaquina
                filtros.listaEconomico = filtros.listaEconomico ?? new List<string>();
                var listaMaquina = _context.tblM_CatMaquina.Where(x =>
                        (!string.IsNullOrEmpty(filtros.areaCuenta) ? x.centro_costos == filtros.areaCuenta : true)).ToList().Where(x =>
                        (filtros.listaGrupos != null ? filtros.listaGrupos.Contains(x.grupoMaquinariaID) : true) &&
                        (filtros.listaModelos != null ? filtros.listaModelos.Contains(x.modeloEquipoID) : true) &&
                        (filtros.listaEconomico.Count > 0 ? filtros.listaEconomico.Contains(x.noEconomico) : true)
                    ).Select(x => new
                    {
                        noEconomico = x.noEconomico,
                        modeloDesc = x.modeloEquipo.descripcion,
                        grupoMaquinariaID = x.grupoMaquinariaID,
                        modeloEquipoID = x.modeloEquipoID
                    }).ToList();

                var equipos = listaMaquina.Select(y => y.noEconomico).ToList();
                var listaHorometros = _context.tblM_CapHorometro.Where(x =>
                        equipos.Contains(x.Economico) &&
                        DbFunctions.TruncateTime(x.Fecha) >= DbFunctions.TruncateTime(filtros.fechaInicial) &&
                        DbFunctions.TruncateTime(x.Fecha) <= DbFunctions.TruncateTime(filtros.fechaFinal)
                    ).GroupBy(y => new
                    {
                        y.Economico,
                        y.Fecha.Year,
                        y.Fecha.Month
                    }).Select(z => new horasMesDTO
                    {
                        Economico = z.Key.Economico,
                        Year = z.Key.Year,
                        Mes = z.Key.Month,
                        Horas = z.Sum(h => h.HorasTrabajo)
                    });

                #endregion
                #region Filtrar Pólizas

                var consulta = @"
                    SELECT
                        c.descripcion AS economico,
                        pol.year,
                        pol.mes, 
                        SUM(mov.monto) as monto,
                        cu.conceptoID
                    FROM sc_polizas pol
                        INNER JOIN sc_movpol mov ON pol.year = mov.year AND pol.mes = mov.mes AND pol.poliza = mov.poliza AND pol.tp = mov.tp
                        INNER JOIN cc c ON mov.cc = c.cc
                        INNER JOIN CPLAN_CTRL_PTAL_CUENTAS cu ON mov.cta = cu.cta AND mov.scta = cu.scta AND mov.sscta = cu.sscta
                    WHERE ";
                var listaParametros = new List<OdbcParameterDTO>();

                //if (filtros.fechaInicial != null && filtros.fechaFinal != null)
                //{
                consulta += @"fechapol BETWEEN ? AND ? AND";

                listaParametros.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.Date, valor = filtros.fechaInicial });
                listaParametros.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.Date, valor = filtros.fechaFinal });
                //}

                consulta += @" economico IN (" + string.Join(", ", listaMaquina.Select(x => "'" + x.noEconomico + "'")) + @") ";

                if (filtros.area > 0 && filtros.cuenta > 0 && !filtros.acumulado)
                {
                    consulta += @"AND area = ? AND cuenta_oc = ?";

                    listaParametros.Add(new OdbcParameterDTO() { nombre = "area", tipo = OdbcType.Numeric, valor = filtros.area });
                    listaParametros.Add(new OdbcParameterDTO() { nombre = "cuenta", tipo = OdbcType.Numeric, valor = filtros.cuenta });
                }

                consulta += @" group by economico, pol.year, pol.mes, cu.conceptoID";

                var listaPolizas = _contextEnkontrol.Select<MovimientoPolizaDTO>(getEnkontrolEnumADM(), new OdbcConsultaDTO() { consulta = consulta, parametros = listaParametros });
                #endregion

                //var listaConceptos = _context.tblM_ControlPresupuestalConcepto.Where(x => x.estatus).ToList();
                //var listaCuentas = _context.tblM_ControlPresupuestalConceptoCuenta.Where(x => x.estatus).ToList().Where(x => listaConceptos.Select(y => y.id).Contains(x.conceptoID)).ToList();
                var caratulaPadreAutorizada = _context.tblM_Caratula.Where(x => x.autorizada == 1).OrderByDescending(x => x.fechaAutorizacion).FirstOrDefault();
                var listaCaratula = _context.tblM_CaratulaDet.Where(x => x.caratula == caratulaPadreAutorizada.idCaratula).ToList();
                var Datos = new List<Economico_MesDTO>();
                #region Gráfica Tendencia
                var graficaTendenciaPresupuestoReal = new GraficaDTO();
                var meses = MonthsBetween(filtros.fechaInicial, filtros.fechaFinal);

                int cantDias = (filtros.fechaFinal - filtros.fechaInicial).Days + 1;
                if (cantDias == 0)
                    cantDias++;

                string t = string.Empty;
                
                foreach (var mes in meses)
                {
                    var totalPresupuesto = 0m;
                    var totalReal = 0m;

                    foreach (var maquina in listaMaquina)
                    {
                        var objs = GlobalUtils.ParseEnumToCtrlPresupuestal<ConceptoEnum>();
                        objs.ForEach(x =>
                            {
                                x.noEconomico = maquina.noEconomico;
                                x.year = mes.Item2;
                                x.mes = mes.Item3;
                                x.presupuesto = 0;
                                x.real = 0;
                                x.total = 0;
                            }
                        );

                        #region Presupuesto
                        var caratulasModelo = listaCaratula.FirstOrDefault(x => x.idModelo == maquina.modeloEquipoID);

                        decimal horasTotal = 0;
                        if (caratulasModelo != null)
                        {
                            var listaHorometrosFiltrada = listaHorometros.FirstOrDefault(x => x.Economico == maquina.noEconomico && x.Year == mes.Item2 && x.Mes == mes.Item3);
                            if (listaHorometrosFiltrada != null)
                            {
                                horasTotal = listaHorometrosFiltrada.Horas;

                                #region v1
                                ////if () //Horas
                                ////{
                                //objs[((int)ConceptoEnum.depreciacion - 1)].horas = horasTotal;
                                //objs[((int)ConceptoEnum.depreciacion - 1)].presupuesto = caratulasModelo.depreciacionMXN * horasTotal;
                                //objs[((int)ConceptoEnum.seguro - 1)].presupuesto = caratulasModelo.seguroMXN * horasTotal;
                                //objs[((int)ConceptoEnum.filtros - 1)].presupuesto = caratulasModelo.filtroMXN * horasTotal;
                                //objs[((int)ConceptoEnum.correctivo - 1)].presupuesto = caratulasModelo.mantenimientoMXN * horasTotal;
                                //objs[((int)ConceptoEnum.depreciacionOverhaul - 1)].presupuesto = caratulasModelo.depreciacionOHMXN * horasTotal;
                                //objs[((int)ConceptoEnum.aceite - 1)].presupuesto = caratulasModelo.aceiteMXN * horasTotal;
                                //objs[((int)ConceptoEnum.carrileria - 1)].presupuesto = caratulasModelo.carilleriaMXN * horasTotal;
                                //objs[((int)ConceptoEnum.ansul - 1)].presupuesto = caratulasModelo.ansulMXN * horasTotal;
                                //objs[((int)ConceptoEnum.otros - 1)].presupuesto = 0;
                                //objs[((int)ConceptoEnum.danos - 1)].presupuesto = 0;
                                ////}
                                ////else //Días
                                ////{
                                ////    //Todavía no se define.
                                ////}
                                #endregion

                                #region v2
                                if (caratulasModelo.tipoHoraDia == 1) //Horas
                                {
                                    objs[((int)ConceptoEnum.depreciacion - 1)].horas = horasTotal;
                                    objs[((int)ConceptoEnum.depreciacion - 1)].presupuesto = caratulasModelo.depreciacionMXN * horasTotal;
                                    objs[((int)ConceptoEnum.seguro - 1)].presupuesto = caratulasModelo.seguroMXN * horasTotal;
                                    objs[((int)ConceptoEnum.filtros - 1)].presupuesto = caratulasModelo.filtroMXN * horasTotal;
                                    objs[((int)ConceptoEnum.correctivo - 1)].presupuesto = caratulasModelo.mantenimientoMXN * horasTotal;
                                    objs[((int)ConceptoEnum.depreciacionOverhaul - 1)].presupuesto = caratulasModelo.depreciacionOHMXN * horasTotal;
                                    objs[((int)ConceptoEnum.aceite - 1)].presupuesto = caratulasModelo.aceiteMXN * horasTotal;
                                    objs[((int)ConceptoEnum.carrileria - 1)].presupuesto = caratulasModelo.carilleriaMXN * horasTotal;
                                    objs[((int)ConceptoEnum.ansul - 1)].presupuesto = caratulasModelo.ansulMXN * horasTotal;
                                    objs[((int)ConceptoEnum.otros - 1)].presupuesto = 0;
                                    objs[((int)ConceptoEnum.danos - 1)].presupuesto = 0;
                                }
                                else if (caratulasModelo.tipoHoraDia == 2) //Días
                                {
                                    objs[((int)ConceptoEnum.depreciacion - 1)].presupuesto = caratulasModelo.depreciacionMXN * cantDias;
                                    objs[((int)ConceptoEnum.seguro - 1)].presupuesto = caratulasModelo.seguroMXN * cantDias;
                                    objs[((int)ConceptoEnum.filtros - 1)].presupuesto = caratulasModelo.filtroMXN * cantDias;
                                    objs[((int)ConceptoEnum.correctivo - 1)].presupuesto = caratulasModelo.mantenimientoMXN * cantDias;
                                    objs[((int)ConceptoEnum.depreciacionOverhaul - 1)].presupuesto = caratulasModelo.depreciacionOHMXN * cantDias;
                                    objs[((int)ConceptoEnum.aceite - 1)].presupuesto = caratulasModelo.aceiteMXN * cantDias;
                                    objs[((int)ConceptoEnum.carrileria - 1)].presupuesto = caratulasModelo.carilleriaMXN * cantDias;
                                    objs[((int)ConceptoEnum.ansul - 1)].presupuesto = caratulasModelo.ansulMXN * cantDias;
                                    objs[((int)ConceptoEnum.otros - 1)].presupuesto = 0;
                                    objs[((int)ConceptoEnum.danos - 1)].presupuesto = 0;
                                }
                                #endregion
                            }
                        }
                        else {
                            var listaHorometrosFiltrada = listaHorometros.FirstOrDefault(x => x.Economico == maquina.noEconomico && x.Year == mes.Item2 && x.Mes == mes.Item3);
                            if (listaHorometrosFiltrada != null)
                            {
                                horasTotal = listaHorometrosFiltrada.Horas;
                                objs[((int)ConceptoEnum.depreciacion - 1)].horas = horasTotal;
                            }
                        }
                        var presupuestoTotal = objs.Sum(x => x.presupuesto);
                        #endregion

                        #region Real
                        var listaPolizasEconomico = listaPolizas.Where(x => x.year == mes.Item2 && x.mes == mes.Item3 && x.economico == maquina.noEconomico);

                        if (listaPolizasEconomico != null)
                        {
                            objs[((int)ConceptoEnum.depreciacion - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.depreciacion)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.seguro - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.seguro)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.filtros - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.filtros)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.correctivo - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.correctivo)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.depreciacionOverhaul - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.depreciacionOverhaul)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.aceite - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.aceite)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.carrileria - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.carrileria)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.ansul - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.ansul)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.otros - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.otros)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.danos - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.danos)).Sum(x => Math.Abs(x.monto));
                        }
                        var realTotal = objs.Sum(x => x.real);
                        #endregion

                        Datos.AddRange(objs);

                        totalPresupuesto += presupuestoTotal;
                        totalReal += realTotal;
                    }

                    graficaTendenciaPresupuestoReal.meses.Add(string.Join("", mes.Item1.Take(3)) + "-" + mes.Item2);
                    graficaTendenciaPresupuestoReal.serie1Descripcion = "Ppto";
                    graficaTendenciaPresupuestoReal.serie1.Add(totalPresupuesto);

                    graficaTendenciaPresupuestoReal.serie2Descripcion = "Real";
                    graficaTendenciaPresupuestoReal.serie2.Add(totalReal);
                }
                #endregion
                #region Tabla Principal
                var data = new List<dynamic>();
                foreach (var maquina in listaMaquina)
                {
                    var eco = Datos.Where(x => x.noEconomico == maquina.noEconomico);
                    var horasTrabajadas = eco.Sum(x => x.horas);
                    #region Presupuesto
                    var presupuestoDepreciacion = eco.Where(x => x.conceptoID == ((int)ConceptoEnum.depreciacion)).Sum(x => x.presupuesto);
                    var presupuestoSeguro = eco.Where(x => x.conceptoID == ((int)ConceptoEnum.seguro)).Sum(x => x.presupuesto);
                    var presupuestoFiltros = eco.Where(x => x.conceptoID == ((int)ConceptoEnum.filtros)).Sum(x => x.presupuesto);
                    var presupuestoCorrectivo = eco.Where(x => x.conceptoID == ((int)ConceptoEnum.correctivo)).Sum(x => x.presupuesto);
                    var presupuestoDepreciacionOverhaul = eco.Where(x => x.conceptoID == ((int)ConceptoEnum.depreciacionOverhaul)).Sum(x => x.presupuesto);
                    var presupuestoAceite = eco.Where(x => x.conceptoID == ((int)ConceptoEnum.aceite)).Sum(x => x.presupuesto);
                    var presupuestoCarrileria = eco.Where(x => x.conceptoID == ((int)ConceptoEnum.carrileria)).Sum(x => x.presupuesto);
                    var presupuestoAnsul = eco.Where(x => x.conceptoID == ((int)ConceptoEnum.ansul)).Sum(x => x.presupuesto);

                    var presupuestoTotal = eco.Sum(x => x.presupuesto);
                    #endregion
                    #region Real
                    var realDepreciacion = eco.Where(x => x.conceptoID == ((int)ConceptoEnum.depreciacion)).Sum(x => x.real);
                    var realSeguro = eco.Where(x => x.conceptoID == ((int)ConceptoEnum.seguro)).Sum(x => x.real);
                    var realFiltros = eco.Where(x => x.conceptoID == ((int)ConceptoEnum.filtros)).Sum(x => x.real);
                    var realCorrectivo = eco.Where(x => x.conceptoID == ((int)ConceptoEnum.correctivo)).Sum(x => x.real);
                    var realDepreciacionOverhaul = eco.Where(x => x.conceptoID == ((int)ConceptoEnum.depreciacionOverhaul)).Sum(x => x.real);
                    var realAceite = eco.Where(x => x.conceptoID == ((int)ConceptoEnum.aceite)).Sum(x => x.real);
                    var realCarrileria = eco.Where(x => x.conceptoID == ((int)ConceptoEnum.carrileria)).Sum(x => x.real);
                    var realAnsul = eco.Where(x => x.conceptoID == ((int)ConceptoEnum.ansul)).Sum(x => x.real);
                    var realOtros = eco.Where(x => x.conceptoID == ((int)ConceptoEnum.otros)).Sum(x => x.real);
                    var realDanos = eco.Where(x => x.conceptoID == ((int)ConceptoEnum.danos)).Sum(x => Math.Abs(x.real));

                    var realTotal = eco.Sum(x => x.real);
                    #endregion
                    #region Diferencias
                    var diferenciaDepreciacion = presupuestoDepreciacion - realDepreciacion;
                    var diferenciaSeguro = presupuestoSeguro - realSeguro;
                    var diferenciaFiltros = presupuestoFiltros - realFiltros;
                    var diferenciaCorrectivo = presupuestoCorrectivo - realCorrectivo;
                    var diferenciaDepreciacionOverhaul = presupuestoDepreciacionOverhaul - realDepreciacionOverhaul;
                    var diferenciaAceite = presupuestoAceite - realAceite;
                    var diferenciaCarrileria = presupuestoCarrileria - realCarrileria;
                    var diferenciaAnsul = presupuestoAnsul - realAnsul;
                    #endregion
                    #region Tabla por conceptos
                    data.Add(new
                    {
                        economico = maquina.noEconomico,
                        modelo = maquina.modeloDesc,
                        horasTrabajadas = horasTrabajadas,
                        diasTrabajados = 0,
                        presupuestoDepreciacion = presupuestoDepreciacion,
                        realDepreciacion = realDepreciacion,
                        diferenciaDepreciacion = diferenciaDepreciacion,
                        presupuestoSeguro = presupuestoSeguro,
                        realSeguro = realSeguro,
                        diferenciaSeguro = diferenciaSeguro,
                        presupuestoFiltros = presupuestoFiltros,
                        realFiltros = realFiltros,
                        diferenciaFiltros = diferenciaFiltros,
                        presupuestoCorrectivo = presupuestoCorrectivo,
                        realCorrectivo = realCorrectivo,
                        diferenciaCorrectivo = diferenciaCorrectivo,
                        presupuestoDepreciacionOverhaul = presupuestoDepreciacionOverhaul,
                        realDepreciacionOverhaul = realDepreciacionOverhaul,
                        diferenciaDepreciacionOverhaul = diferenciaDepreciacionOverhaul,
                        presupuestoAceite = presupuestoAceite,
                        realAceite = realAceite,
                        diferenciaAceite = diferenciaAceite,
                        presupuestoCarrileria = presupuestoCarrileria,
                        realCarrileria = realCarrileria,
                        diferenciaCarrileria = diferenciaCarrileria,
                        presupuestoAnsul = presupuestoAnsul,
                        realAnsul = realAnsul,
                        diferenciaAnsul = diferenciaAnsul,
                        realOtros = realOtros,
                        realDanos = realDanos,
                        presupuestoTotal = presupuestoTotal,
                        realTotal = realTotal,
                        diferenciaTotal = presupuestoTotal - realTotal,
                    });
                    #endregion
                }
                #endregion
                resultado.Add("data", data);
                resultado.Add("graficaTendenciaPresupuestoReal", graficaTendenciaPresupuestoReal);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "cargarControlPresupuestal", e, AccionEnum.CONSULTA, 0, filtros);
            }

            return resultado;
        }
        public Dictionary<string, object> cargarControlPresupuestal_Solo_Grafica(FiltrosControlPresupuestalDTO filtros)
        {
            try
            {
                #region Filtrar CatMaquina
                filtros.listaEconomico = filtros.listaEconomico ?? new List<string>();
                var listaMaquina = _context.tblM_CatMaquina.Where(x =>
                        (!string.IsNullOrEmpty(filtros.areaCuenta) ? x.centro_costos == filtros.areaCuenta : true)).ToList().Where( x=>
                        (filtros.listaGrupos != null ? filtros.listaGrupos.Contains(x.grupoMaquinariaID) : true) &&
                        (filtros.listaModelos != null ? filtros.listaModelos.Contains(x.modeloEquipoID) : true) &&
                        (filtros.listaEconomico.Count > 0 ? filtros.listaEconomico.Contains(x.noEconomico) : true)
                    ).Select(x => new
                    {
                        noEconomico = x.noEconomico,
                        modeloDesc = x.modeloEquipo.descripcion,
                        grupoMaquinariaID = x.grupoMaquinariaID,
                        modeloEquipoID = x.modeloEquipoID
                    }).ToList();

                var equipos = listaMaquina.Select(y => y.noEconomico).ToList();
                var listaHorometros = _context.tblM_CapHorometro.Where(x =>
                        equipos.Contains(x.Economico) &&
                        DbFunctions.TruncateTime(x.Fecha) >= DbFunctions.TruncateTime(filtros.fechaInicial) &&
                        DbFunctions.TruncateTime(x.Fecha) <= DbFunctions.TruncateTime(filtros.fechaFinal)
                    ).GroupBy(y => new
                    {
                        y.Economico,
                        y.Fecha.Year,
                        y.Fecha.Month
                    }).Select(z => new horasMesDTO
                    {
                        Economico = z.Key.Economico,
                        Year = z.Key.Year,
                        Mes = z.Key.Month,
                        Horas = z.Sum(h => h.HorasTrabajo)
                    });

                #endregion
                #region Filtrar Pólizas

                var consulta = @"
                    SELECT
                        c.descripcion AS economico,
                        pol.year,
                        pol.mes, 
                        SUM(mov.monto) as monto,
                        cu.conceptoID
                    FROM sc_polizas pol
                        INNER JOIN sc_movpol mov ON pol.year = mov.year AND pol.mes = mov.mes AND pol.poliza = mov.poliza AND pol.tp = mov.tp
                        INNER JOIN cc c ON mov.cc = c.cc
                        INNER JOIN CPLAN_CTRL_PTAL_CUENTAS cu ON mov.cta = cu.cta AND mov.scta = cu.scta AND mov.sscta = cu.sscta
                    WHERE ";
                var listaParametros = new List<OdbcParameterDTO>();

                //if (filtros.fechaInicial != null && filtros.fechaFinal != null)
                //{
                consulta += @"fechapol BETWEEN ? AND ? AND";

                listaParametros.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.Date, valor = filtros.fechaInicial });
                listaParametros.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.Date, valor = filtros.fechaFinal });
                //}

                consulta += @" economico IN (" + string.Join(", ", listaMaquina.Select(x => "'" + x.noEconomico + "'")) + @") ";

                if (filtros.area > 0 && filtros.cuenta > 0 && !filtros.acumulado)
                {
                    consulta += @"AND area = ? AND cuenta_oc = ?";

                    listaParametros.Add(new OdbcParameterDTO() { nombre = "area", tipo = OdbcType.Numeric, valor = filtros.area });
                    listaParametros.Add(new OdbcParameterDTO() { nombre = "cuenta", tipo = OdbcType.Numeric, valor = filtros.cuenta });
                }

                consulta += @" group by economico, pol.year, pol.mes, cu.conceptoID";

                var listaPolizas = _contextEnkontrol.Select<MovimientoPolizaDTO>(getEnkontrolEnumADM(), new OdbcConsultaDTO() { consulta = consulta, parametros = listaParametros });
                #endregion

                var caratulaPadreAutorizada = _context.tblM_Caratula.Where(x => x.autorizada == 1).OrderByDescending(x => x.fechaAutorizacion).FirstOrDefault();
                var listaCaratula = _context.tblM_CaratulaDet.Where(x => x.caratula == caratulaPadreAutorizada.idCaratula).ToList();

                #region Gráfica Actos
                var graficaTendenciaPresupuestoReal = new GraficaDTO();
                var meses = MonthsBetween(filtros.fechaInicial, filtros.fechaFinal);

                foreach (var mes in meses)
                {
                    var totalPresupuesto = 0m;
                    var totalReal = 0m;

                    foreach (var maquina in listaMaquina)
                    {
                        var objs = GlobalUtils.ParseEnumToCtrlPresupuestal<ConceptoEnum>();
                        objs.ForEach(x =>
                        {
                            x.noEconomico = maquina.noEconomico;
                            x.year = mes.Item2;
                            x.mes = mes.Item3;
                            x.presupuesto = 0;
                            x.real = 0;
                            x.total = 0;
                        }
                        );

                        #region Presupuesto
                        var caratulasModelo = listaCaratula.FirstOrDefault(x => x.idModelo == maquina.modeloEquipoID);

                        decimal horasTotal = 0;
                        if (caratulasModelo != null)
                        {
                            var listaHorometrosFiltrada = listaHorometros.FirstOrDefault(x => x.Economico == maquina.noEconomico && x.Year == mes.Item2 && x.Mes == mes.Item3);
                            if (listaHorometrosFiltrada != null)
                            {
                                horasTotal = listaHorometrosFiltrada.Horas;

                                //if () //Horas
                                //{
                                objs[((int)ConceptoEnum.depreciacion - 1)].horas = horasTotal;
                                objs[((int)ConceptoEnum.depreciacion - 1)].presupuesto = caratulasModelo.depreciacionMXN * horasTotal;
                                objs[((int)ConceptoEnum.seguro - 1)].presupuesto = caratulasModelo.seguroMXN * horasTotal;
                                objs[((int)ConceptoEnum.filtros - 1)].presupuesto = caratulasModelo.filtroMXN * horasTotal;
                                objs[((int)ConceptoEnum.correctivo - 1)].presupuesto = caratulasModelo.mantenimientoMXN * horasTotal;
                                objs[((int)ConceptoEnum.depreciacionOverhaul - 1)].presupuesto = caratulasModelo.depreciacionOHMXN * horasTotal;
                                objs[((int)ConceptoEnum.aceite - 1)].presupuesto = caratulasModelo.aceiteMXN * horasTotal;
                                objs[((int)ConceptoEnum.carrileria - 1)].presupuesto = caratulasModelo.carilleriaMXN * horasTotal;
                                objs[((int)ConceptoEnum.ansul - 1)].presupuesto = caratulasModelo.ansulMXN * horasTotal;
                                objs[((int)ConceptoEnum.otros - 1)].presupuesto = 0;
                                objs[((int)ConceptoEnum.danos - 1)].presupuesto = 0;
                                //}
                                //else //Días
                                //{
                                //    //Todavía no se define.
                                //}
                            }
                        }
                        var presupuestoTotal = objs.Sum(x => x.presupuesto);
                        #endregion

                        #region Real
                        var listaPolizasEconomico = listaPolizas.Where(x => x.year == mes.Item2 && x.mes == mes.Item3 && x.economico == maquina.noEconomico);

                        if (listaPolizasEconomico != null)
                        {
                            objs[((int)ConceptoEnum.depreciacion - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.depreciacion)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.seguro - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.seguro)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.filtros - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.filtros)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.correctivo - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.correctivo)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.depreciacionOverhaul - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.depreciacionOverhaul)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.aceite - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.aceite)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.carrileria - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.carrileria)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.ansul - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.ansul)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.otros - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.otros)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.danos - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.danos)).Sum(x => Math.Abs(x.monto));
                        }
                        var realTotal = objs.Sum(x => x.real);
                        #endregion

                        totalPresupuesto += presupuestoTotal;
                        totalReal += realTotal;
                    }

                    graficaTendenciaPresupuestoReal.meses.Add(string.Join("", mes.Item1.Take(3)) + "-" + mes.Item2);
                    graficaTendenciaPresupuestoReal.serie1Descripcion = "Ppto";
                    graficaTendenciaPresupuestoReal.serie1.Add(totalPresupuesto);

                    graficaTendenciaPresupuestoReal.serie2Descripcion = "Real";
                    graficaTendenciaPresupuestoReal.serie2.Add(totalReal);
                }
                #endregion

                resultado.Add("graficaTendenciaPresupuestoReal", graficaTendenciaPresupuestoReal);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "cargarControlPresupuestal", e, AccionEnum.CONSULTA, 0, filtros);
            }

            return resultado;
        }
        public Dictionary<string, object> cargarDetalleAgrupado(FiltrosControlPresupuestalDTO filtros, string economico, int concepto)
        {
            try
            {
                #region Filtrar CatMaquina
                filtros.listaEconomico = filtros.listaEconomico ?? new List<string>();
                var listaMaquina = _context.tblM_CatMaquina.Where(x =>
                        x.noEconomico == economico
                    ).Select(x => new
                    {
                        noEconomico = x.noEconomico,
                        modeloDesc = x.modeloEquipo.descripcion,
                        grupoMaquinariaID = x.grupoMaquinariaID,
                        modeloEquipoID = x.modeloEquipoID
                    }).ToList();

                var equipos = listaMaquina.Select(y => y.noEconomico).ToList();
                var listaHorometros = _context.tblM_CapHorometro.Where(x =>
                        equipos.Contains(x.Economico) &&
                        DbFunctions.TruncateTime(x.Fecha) >= DbFunctions.TruncateTime(filtros.fechaInicial) &&
                        DbFunctions.TruncateTime(x.Fecha) <= DbFunctions.TruncateTime(filtros.fechaFinal)
                    ).GroupBy(y => new
                    {
                        y.Economico,
                        y.Fecha.Year,
                        y.Fecha.Month
                    }).Select(z => new horasMesDTO
                    {
                        Economico = z.Key.Economico,
                        Year = z.Key.Year,
                        Mes = z.Key.Month,
                        Horas = z.Sum(h => h.HorasTrabajo)
                    });

                #endregion
                #region Filtrar Pólizas
                var consulta = @"
                    SELECT
                        c.descripcion AS economico,
                        cu.descripcion AS cuentaDescripcion,
                        pol.year,
                        pol.mes, 
                        mov.cta, 
                        mov.scta, 
                        mov.sscta,
                        SUM(mov.monto) as monto
                    FROM sc_polizas pol
                        INNER JOIN sc_movpol mov ON pol.year = mov.year AND pol.mes = mov.mes AND pol.poliza = mov.poliza AND pol.tp = mov.tp
                        INNER JOIN cc c ON mov.cc = c.cc
                        INNER JOIN catcta cu ON mov.cta = cu.cta AND mov.scta = cu.scta AND mov.sscta = cu.sscta
                        INNER JOIN CPLAN_CTRL_PTAL_CUENTAS cu2 ON mov.cta = cu2.cta AND mov.scta = cu2.scta AND mov.sscta = cu2.sscta
                    WHERE ";
                var listaParametros = new List<OdbcParameterDTO>();

                //if (filtros.periodoInicialID > 0 && filtros.periodoFinID > 0)
                //{
                consulta += @"fechapol BETWEEN ? AND ? AND";

                listaParametros.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.Date, valor = filtros.fechaInicial });
                listaParametros.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.Date, valor = filtros.fechaFinal });
                //}

                consulta += @" economico = '" + economico + @"' ";

                if (filtros.area > 0 && filtros.cuenta > 0 && !filtros.acumulado)
                {
                    consulta += @"AND area = ? AND cuenta_oc = ?";

                    listaParametros.Add(new OdbcParameterDTO() { nombre = "area", tipo = OdbcType.Numeric, valor = filtros.area });
                    listaParametros.Add(new OdbcParameterDTO() { nombre = "cuenta", tipo = OdbcType.Numeric, valor = filtros.cuenta });
                }

                consulta += @" AND cu2.conceptoID = ? ";

                listaParametros.Add(new OdbcParameterDTO() { nombre = "conceptoID", tipo = OdbcType.Int, valor = concepto });

                consulta += @" group by economico, cuentaDescripcion, pol.year, pol.mes, mov.cta, mov.scta, mov.sscta";
                var listaPolizas = _contextEnkontrol.Select<MovimientoPolizaDTO>(getEnkontrolEnumADM(), new OdbcConsultaDTO() { consulta = consulta, parametros = listaParametros });
                #endregion
                var caratulaPadreAutorizada = _context.tblM_Caratula.Where(x => x.autorizada == 1).OrderByDescending(x => x.fechaAutorizacion).FirstOrDefault();
                var listaCaratula = _context.tblM_CaratulaDet.Where(x => x.caratula == caratulaPadreAutorizada.idCaratula).ToList();
                var Datos = new List<Economico_MesDTO>();
                #region Gráfica Tendencia
                var graficaTendenciaPresupuestoReal = new GraficaDTO();
                var meses = MonthsBetween(filtros.fechaInicial, filtros.fechaFinal);
                foreach (var mes in meses)
                {
                    var totalPresupuesto = 0m;
                    var totalReal = 0m;

                    foreach (var maquina in listaMaquina)
                    {
                        var objs = GlobalUtils.ParseEnumToCtrlPresupuestal<ConceptoEnum>();
                        objs.ForEach(x =>
                        {
                            x.noEconomico = maquina.noEconomico;
                            x.year = mes.Item2;
                            x.mes = mes.Item3;
                            x.presupuesto = 0;
                            x.real = 0;
                            x.total = 0;
                        }
                        );

                        #region Presupuesto
                        var caratulasModelo = listaCaratula.FirstOrDefault(x => x.idModelo == maquina.modeloEquipoID);

                        decimal horasTotal = 0;
                        if (caratulasModelo != null)
                        {
                            var listaHorometrosFiltrada = listaHorometros.FirstOrDefault(x => x.Economico == maquina.noEconomico && x.Year == mes.Item2 && x.Mes == mes.Item3);
                            if (listaHorometrosFiltrada != null)
                            {
                                horasTotal = listaHorometrosFiltrada.Horas;

                                //if () //Horas
                                //{
                                objs[((int)ConceptoEnum.depreciacion - 1)].horas = horasTotal;
                                objs[((int)ConceptoEnum.depreciacion - 1)].presupuesto = caratulasModelo.depreciacionMXN * horasTotal;
                                objs[((int)ConceptoEnum.seguro - 1)].presupuesto = caratulasModelo.seguroMXN * horasTotal;
                                objs[((int)ConceptoEnum.filtros - 1)].presupuesto = caratulasModelo.filtroMXN * horasTotal;
                                objs[((int)ConceptoEnum.correctivo - 1)].presupuesto = caratulasModelo.mantenimientoMXN * horasTotal;
                                objs[((int)ConceptoEnum.depreciacionOverhaul - 1)].presupuesto = caratulasModelo.depreciacionOHMXN * horasTotal;
                                objs[((int)ConceptoEnum.aceite - 1)].presupuesto = caratulasModelo.aceiteMXN * horasTotal;
                                objs[((int)ConceptoEnum.carrileria - 1)].presupuesto = caratulasModelo.carilleriaMXN * horasTotal;
                                objs[((int)ConceptoEnum.ansul - 1)].presupuesto = caratulasModelo.ansulMXN * horasTotal;
                                objs[((int)ConceptoEnum.otros - 1)].presupuesto = 0;
                                objs[((int)ConceptoEnum.danos - 1)].presupuesto = 0;
                                //}
                                //else //Días
                                //{
                                //    //Todavía no se define.
                                //}
                            }
                        }
                        var presupuestoTotal = objs.Sum(x => x.presupuesto);
                        #endregion

                        #region Real
                        var listaPolizasEconomico = listaPolizas.Where(x => x.year == mes.Item2 && x.mes == mes.Item3 && x.economico == maquina.noEconomico);

                        if (listaPolizasEconomico != null)
                        {
                            objs[((int)ConceptoEnum.depreciacion - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.depreciacion)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.seguro - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.seguro)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.filtros - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.filtros)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.correctivo - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.correctivo)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.depreciacionOverhaul - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.depreciacionOverhaul)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.aceite - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.aceite)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.carrileria - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.carrileria)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.ansul - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.ansul)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.otros - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.otros)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.danos - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.danos)).Sum(x => Math.Abs(x.monto));
                        }
                        var realTotal = objs.Sum(x => x.real);
                        #endregion

                        //Datos.AddRange(objs);

                        totalPresupuesto += presupuestoTotal;
                        totalReal += realTotal;
                        //totalPresupuesto += presupuestoConcepto;




                        // totalReal += realConcepto;
                    }

                    graficaTendenciaPresupuestoReal.meses.Add(string.Join("", mes.Item1.Take(3)) + "-" + mes.Item2);
                    graficaTendenciaPresupuestoReal.serie1Descripcion = "Ppto";
                    graficaTendenciaPresupuestoReal.serie1.Add(concepto != 9 ? totalPresupuesto : 0);

                    graficaTendenciaPresupuestoReal.serie2Descripcion = "Real";
                    graficaTendenciaPresupuestoReal.serie2.Add(totalReal);
                }
                #endregion
                #region Tabla Principal
                var data = new List<dynamic>();
                foreach (var grp in listaPolizas.GroupBy(x => x.cuentaDescripcion))
                {
                    data.Add(new
                    {
                        economico = economico,
                        concepto = concepto,
                        cta = grp.First().cta,
                        scta = grp.First().scta,
                        sscta = grp.First().sscta,
                        cuentaDesc = grp.First().cta + " " + grp.First().scta + " " + grp.First().sscta,
                        descripcion = grp.Key,
                        importe = concepto == 10 ? grp.Sum(y => Math.Abs(y.monto)) : grp.Sum(y => y.monto)
                    });
                }
                #endregion

                resultado.Add("graficaTendenciaPresupuestoReal", graficaTendenciaPresupuestoReal);
                resultado.Add("data", data.OrderByDescending(x => x.cuentaDesc).ToList());
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "cargarDetalleAgrupado", e, AccionEnum.CONSULTA, 0, new { filtros = filtros, economico = economico, concepto = concepto });
            }

            return resultado;
        }
        public Dictionary<string, object> CargarDetallePresupuestal(FiltrosControlPresupuestalDTO filtros, string economico, int concepto)
        {
            try
            {
                #region Filtrar CatMaquina
                filtros.listaEconomico = filtros.listaEconomico ?? new List<string>();
                var listaMaquina = _context.tblM_CatMaquina.Where(x =>
                        x.noEconomico == economico
                    ).Select(x => new
                    {
                        noEconomico = x.noEconomico,
                        modeloDesc = x.modeloEquipo.descripcion,
                        grupoMaquinariaID = x.grupoMaquinariaID,
                        modeloEquipoID = x.modeloEquipoID
                    }).ToList();

                var equipos = listaMaquina.Select(y => y.noEconomico).ToList();
                var listaHorometros = _context.tblM_CapHorometro.Where(x =>
                        equipos.Contains(x.Economico) &&
                        DbFunctions.TruncateTime(x.Fecha) >= DbFunctions.TruncateTime(filtros.fechaInicial) &&
                        DbFunctions.TruncateTime(x.Fecha) <= DbFunctions.TruncateTime(filtros.fechaFinal)
                    ).GroupBy(y => new
                    {
                        y.Economico,
                        y.Fecha.Year,
                        y.Fecha.Month
                    }).Select(z => new horasMesDTO
                    {
                        Economico = z.Key.Economico,
                        Year = z.Key.Year,
                        Mes = z.Key.Month,
                        Horas = z.Sum(h => h.HorasTrabajo)
                    });

                #endregion
                #region Filtrar Pólizas
                var consulta = @"
                    SELECT
                        c.descripcion AS economico,
                        pol.year,
                        pol.mes, 
                        SUM(mov.monto) as monto,
                        cu.conceptoID
                    FROM sc_polizas pol
                        INNER JOIN sc_movpol mov ON pol.year = mov.year AND pol.mes = mov.mes AND pol.poliza = mov.poliza AND pol.tp = mov.tp
                        INNER JOIN cc c ON mov.cc = c.cc
                        INNER JOIN CPLAN_CTRL_PTAL_CUENTAS cu ON mov.cta = cu.cta AND mov.scta = cu.scta AND mov.sscta = cu.sscta
                    WHERE ";
                var listaParametros = new List<OdbcParameterDTO>();

                //if (filtros.fechaInicial != null && filtros.fechaFinal != null)
                //{
                consulta += @"fechapol BETWEEN ? AND ? AND";

                listaParametros.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.Date, valor = filtros.fechaInicial });
                listaParametros.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.Date, valor = filtros.fechaFinal });
                //}

                consulta += @" economico IN (" + string.Join(", ", listaMaquina.Select(x => "'" + x.noEconomico + "'")) + @") ";

                if (filtros.area > 0 && filtros.cuenta > 0 && !filtros.acumulado)
                {
                    consulta += @"AND area = ? AND cuenta_oc = ?";

                    listaParametros.Add(new OdbcParameterDTO() { nombre = "area", tipo = OdbcType.Numeric, valor = filtros.area });
                    listaParametros.Add(new OdbcParameterDTO() { nombre = "cuenta", tipo = OdbcType.Numeric, valor = filtros.cuenta });
                }

                consulta += @" group by economico, pol.year, pol.mes, cu.conceptoID";

                var listaPolizas = _contextEnkontrol.Select<MovimientoPolizaDTO>(getEnkontrolEnumADM(), new OdbcConsultaDTO() { consulta = consulta, parametros = listaParametros });
                #endregion
                var caratulaPadreAutorizada = _context.tblM_Caratula.Where(x => x.autorizada == 1).OrderByDescending(x => x.fechaAutorizacion).FirstOrDefault();
                var listaCaratula = _context.tblM_CaratulaDet.Where(x => x.caratula == caratulaPadreAutorizada.idCaratula).ToList();
                #region Gráfica Tendencia
                var graficaTendenciaPresupuestoReal = new GraficaDTO();
                var meses = MonthsBetween(filtros.fechaInicial, filtros.fechaFinal);
                var Datos = new List<Economico_MesDTO>();
                foreach (var mes in meses)
                {
                    var totalPresupuesto = 0m;
                    var totalReal = 0m;

                    foreach (var maquina in listaMaquina)
                    {
                        var objs = GlobalUtils.ParseEnumToCtrlPresupuestal<ConceptoEnum>();
                        objs.ForEach(x =>
                        {
                            x.noEconomico = maquina.noEconomico;
                            x.year = mes.Item2;
                            x.mes = mes.Item3;
                            x.presupuesto = 0;
                            x.real = 0;
                            x.total = 0;
                        }
                        );

                        #region Presupuesto
                        var caratulasModelo = listaCaratula.FirstOrDefault(x => x.idModelo == maquina.modeloEquipoID);

                        decimal horasTotal = 0;
                        if (caratulasModelo != null)
                        {
                            var listaHorometrosFiltrada = listaHorometros.FirstOrDefault(x => x.Economico == maquina.noEconomico && x.Year == mes.Item2 && x.Mes == mes.Item3);
                            if (listaHorometrosFiltrada != null)
                            {
                                horasTotal = listaHorometrosFiltrada.Horas;

                                //if () //Horas
                                //{
                                objs[((int)ConceptoEnum.depreciacion - 1)].horas = horasTotal;
                                objs[((int)ConceptoEnum.depreciacion - 1)].presupuesto = caratulasModelo.depreciacionMXN * horasTotal;
                                objs[((int)ConceptoEnum.seguro - 1)].presupuesto = caratulasModelo.seguroMXN * horasTotal;
                                objs[((int)ConceptoEnum.filtros - 1)].presupuesto = caratulasModelo.filtroMXN * horasTotal;
                                objs[((int)ConceptoEnum.correctivo - 1)].presupuesto = caratulasModelo.mantenimientoMXN * horasTotal;
                                objs[((int)ConceptoEnum.depreciacionOverhaul - 1)].presupuesto = caratulasModelo.depreciacionOHMXN * horasTotal;
                                objs[((int)ConceptoEnum.aceite - 1)].presupuesto = caratulasModelo.aceiteMXN * horasTotal;
                                objs[((int)ConceptoEnum.carrileria - 1)].presupuesto = caratulasModelo.carilleriaMXN * horasTotal;
                                objs[((int)ConceptoEnum.ansul - 1)].presupuesto = caratulasModelo.ansulMXN * horasTotal;
                                objs[((int)ConceptoEnum.otros - 1)].presupuesto = 0;
                                objs[((int)ConceptoEnum.danos - 1)].presupuesto = 0;
                                //}
                                //else //Días
                                //{
                                //    //Todavía no se define.
                                //}
                            }
                        }
                        var presupuestoTotal = objs.Sum(x => x.presupuesto);
                        #endregion

                        #region Real
                        var listaPolizasEconomico = listaPolizas.Where(x => x.year == mes.Item2 && x.mes == mes.Item3 && x.economico == maquina.noEconomico);

                        if (listaPolizasEconomico != null)
                        {
                            objs[((int)ConceptoEnum.depreciacion - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.depreciacion)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.seguro - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.seguro)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.filtros - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.filtros)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.correctivo - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.correctivo)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.depreciacionOverhaul - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.depreciacionOverhaul)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.aceite - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.aceite)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.carrileria - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.carrileria)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.ansul - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.ansul)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.otros - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.otros)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.danos - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.danos)).Sum(x => Math.Abs(x.monto));
                        }
                        var realTotal = objs.Sum(x => x.real);
                        #endregion

                        Datos.AddRange(objs);

                        totalPresupuesto += presupuestoTotal;
                        totalReal += realTotal;
                    }

                    graficaTendenciaPresupuestoReal.meses.Add(string.Join("", mes.Item1.Take(3)) + "-" + mes.Item2);
                    graficaTendenciaPresupuestoReal.serie1Descripcion = "Ppto";
                    graficaTendenciaPresupuestoReal.serie1.Add(totalPresupuesto);

                    graficaTendenciaPresupuestoReal.serie2Descripcion = "Real";
                    graficaTendenciaPresupuestoReal.serie2.Add(totalReal);
                }
                #endregion
                #region Tabla Principal
                var data = new List<TablaDetalleEconomicoDTO>();
                foreach (var maquina in listaMaquina)
                {
                    var eco = Datos.Where(x => x.noEconomico == maquina.noEconomico);
                    var horasTrabajadas = eco.Sum(x => x.horas);
                    #region Presupuesto
                    var presupuestoDepreciacion = eco.Where(x => x.conceptoID == ((int)ConceptoEnum.depreciacion)).Sum(x => x.presupuesto);
                    var presupuestoSeguro = eco.Where(x => x.conceptoID == ((int)ConceptoEnum.seguro)).Sum(x => x.presupuesto);
                    var presupuestoFiltros = eco.Where(x => x.conceptoID == ((int)ConceptoEnum.filtros)).Sum(x => x.presupuesto);
                    var presupuestoCorrectivo = eco.Where(x => x.conceptoID == ((int)ConceptoEnum.correctivo)).Sum(x => x.presupuesto);
                    var presupuestoDepreciacionOverhaul = eco.Where(x => x.conceptoID == ((int)ConceptoEnum.depreciacionOverhaul)).Sum(x => x.presupuesto);
                    var presupuestoAceite = eco.Where(x => x.conceptoID == ((int)ConceptoEnum.aceite)).Sum(x => x.presupuesto);
                    var presupuestoCarrileria = eco.Where(x => x.conceptoID == ((int)ConceptoEnum.carrileria)).Sum(x => x.presupuesto);
                    var presupuestoAnsul = eco.Where(x => x.conceptoID == ((int)ConceptoEnum.ansul)).Sum(x => x.presupuesto);

                    var presupuestoTotal = eco.Sum(x => x.presupuesto);
                    #endregion
                    #region Real
                    var realDepreciacion = eco.Where(x => x.conceptoID == ((int)ConceptoEnum.depreciacion)).Sum(x => x.real);
                    var realSeguro = eco.Where(x => x.conceptoID == ((int)ConceptoEnum.seguro)).Sum(x => x.real);
                    var realFiltros = eco.Where(x => x.conceptoID == ((int)ConceptoEnum.filtros)).Sum(x => x.real);
                    var realCorrectivo = eco.Where(x => x.conceptoID == ((int)ConceptoEnum.correctivo)).Sum(x => x.real);
                    var realDepreciacionOverhaul = eco.Where(x => x.conceptoID == ((int)ConceptoEnum.depreciacionOverhaul)).Sum(x => x.real);
                    var realAceite = eco.Where(x => x.conceptoID == ((int)ConceptoEnum.aceite)).Sum(x => x.real);
                    var realCarrileria = eco.Where(x => x.conceptoID == ((int)ConceptoEnum.carrileria)).Sum(x => x.real);
                    var realAnsul = eco.Where(x => x.conceptoID == ((int)ConceptoEnum.ansul)).Sum(x => x.real);
                    var realOtros = eco.Where(x => x.conceptoID == ((int)ConceptoEnum.otros)).Sum(x => x.real);
                    var realDanos = eco.Where(x => x.conceptoID == ((int)ConceptoEnum.danos)).Sum(x => x.real);

                    var realTotal = eco.Sum(x => x.real);
                    #endregion
                    #region Diferencias
                    var diferenciaDepreciacion = presupuestoDepreciacion - realDepreciacion;
                    var diferenciaSeguro = presupuestoSeguro - realSeguro;
                    var diferenciaFiltros = presupuestoFiltros - realFiltros;
                    var diferenciaCorrectivo = presupuestoCorrectivo - realCorrectivo;
                    var diferenciaDepreciacionOverhaul = presupuestoDepreciacionOverhaul - realDepreciacionOverhaul;
                    var diferenciaAceite = presupuestoAceite - realAceite;
                    var diferenciaCarrileria = presupuestoCarrileria - realCarrileria;
                    var diferenciaAnsul = presupuestoAnsul - realAnsul;
                    #endregion
                    #region tabla por concepto
                    var dato = new TablaDetalleEconomicoDTO();
                    dato.concepto = "Depreciacion";
                    dato.real = realDepreciacion;
                    data.Add(dato);

                    dato = new TablaDetalleEconomicoDTO();
                    dato.concepto = "Seguro";
                    dato.real = realSeguro;
                    data.Add(dato);

                    dato = new TablaDetalleEconomicoDTO();
                    dato.concepto = "Filtros";
                    dato.real = realFiltros;
                    data.Add(dato);

                    dato = new TablaDetalleEconomicoDTO();
                    dato.concepto = "Correctivo";
                    dato.real = realCorrectivo;
                    data.Add(dato);

                    dato = new TablaDetalleEconomicoDTO();
                    dato.concepto = "Dep. OVH";
                    dato.real = realDepreciacionOverhaul;
                    data.Add(dato);

                    dato = new TablaDetalleEconomicoDTO();
                    dato.concepto = "Aceite";
                    dato.real = realAceite;
                    data.Add(dato);

                    dato = new TablaDetalleEconomicoDTO();
                    dato.concepto = "Carrilería";
                    dato.real = realCarrileria;
                    data.Add(dato);

                    dato = new TablaDetalleEconomicoDTO();
                    dato.concepto = "Ansul";
                    dato.real = realAnsul;
                    data.Add(dato);

                    dato = new TablaDetalleEconomicoDTO();
                    dato.concepto = "Otros";
                    dato.real = realOtros;
                    data.Add(dato);

                    dato = new TablaDetalleEconomicoDTO();
                    dato.concepto = "Daños";
                    dato.real = Math.Abs(realDanos);
                    data.Add(dato);

                    dato = new TablaDetalleEconomicoDTO();
                    dato.concepto = "Total";
                    dato.real = realTotal;
                    data.Add(dato);
                    #endregion
                }
                #endregion

                resultado.Add("data", data);
                resultado.Add("graficaTendenciaPresupuestoReal", graficaTendenciaPresupuestoReal);
                resultado.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "cargarDetalleAgrupado", e, AccionEnum.CONSULTA, 0, new { filtros = filtros, economico = economico, concepto = concepto });
            }

            return resultado;
        }
        public Dictionary<string, object> cargarDetalleMovimientos(FiltrosControlPresupuestalDTO filtros, string economico, int concepto, int cta, int scta, int sscta)
        {
            try
            {
                #region Filtrar Pólizas
                var consulta = @"
                    SELECT
                        pol.year, pol.mes, pol.poliza, pol.tp, pol.fechapol,
                        mov.linea, mov.cta, mov.scta, mov.sscta, mov.cc, mov.concepto, mov.monto, mov.area, mov.cuenta_oc,
                        c.descripcion AS economico
                    FROM sc_polizas pol
                        INNER JOIN sc_movpol mov ON pol.year = mov.year AND pol.mes = mov.mes AND pol.poliza = mov.poliza AND pol.tp = mov.tp
                        INNER JOIN cc c ON mov.cc = c.cc
                    WHERE ";
                var listaParametros = new List<OdbcParameterDTO>();

                //if (filtros.periodoInicialID > 0 && filtros.periodoFinID > 0)
                //{
                consulta += @"fechapol BETWEEN ? AND ? AND";

                listaParametros.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.Date, valor = filtros.fechaInicial });
                listaParametros.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.Date, valor = filtros.fechaFinal });
                //}

                consulta += @" economico = '" + economico + @"' ";

                if (filtros.area > 0 && filtros.cuenta > 0 && !filtros.acumulado)
                {
                    consulta += @"AND area = ? AND cuenta_oc = ?";

                    listaParametros.Add(new OdbcParameterDTO() { nombre = "area", tipo = OdbcType.Numeric, valor = filtros.area });
                    listaParametros.Add(new OdbcParameterDTO() { nombre = "cuenta", tipo = OdbcType.Numeric, valor = filtros.cuenta });
                }
                consulta += @" AND mov.cta = ? AND mov.scta = ? AND mov.sscta = ? ";
                listaParametros.Add(new OdbcParameterDTO() { nombre = "cta", tipo = OdbcType.Numeric, valor = cta });
                listaParametros.Add(new OdbcParameterDTO() { nombre = "scta", tipo = OdbcType.Numeric, valor = scta });
                listaParametros.Add(new OdbcParameterDTO() { nombre = "sscta", tipo = OdbcType.Numeric, valor = sscta });
                var listaPolizas = _contextEnkontrol.Select<MovimientoPolizaDTO>(getEnkontrolEnumADM(), new OdbcConsultaDTO() { consulta = consulta, parametros = listaParametros });
                #endregion

                var data = new List<dynamic>();
                #region Tabla Principal
                foreach (var poliza in listaPolizas)
                {
                    data.Add(new
                    {
                        economico = economico,
                        concepto = concepto,
                        cta = cta,
                        scta = scta,
                        sscta = sscta,
                        fechaPoliza = poliza.fechapol,
                        fechaPolizaString = poliza.fechapol.ToShortDateString(),
                        descripcion = poliza.concepto,
                        importe = concepto==10 ?Math.Abs(poliza.monto):poliza.monto
                    });
                }
                var lstDatos = data.OrderBy(x => x.fechaPoliza).ToList();
                #endregion
                resultado.Add("data", lstDatos);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "cargarDetalleMovimientos", e, AccionEnum.CONSULTA, 0, new { filtros = filtros, economico = economico, concepto = concepto, cta = cta, scta = scta, sscta = sscta });
            }

            return resultado;
        }
        private EnkontrolEnum getEnkontrolEnumADM()
        {
            var baseDatos = new EnkontrolEnum();

            //if (productivo)
            //{
            if (vSesiones.sesionEmpresaActual == 1)
            {
                baseDatos = EnkontrolEnum.CplanProd;
            }
            else if (vSesiones.sesionEmpresaActual == 2)
            {
                baseDatos = EnkontrolEnum.ArrenProd;
            }
            else
            {
                throw new Exception("Empresa distinta a Construplan y Arrendadora");
            }
            //}
            //else
            //{
            //    if (vSesiones.sesionEmpresaActual == 1)
            //    {
            //        baseDatos = EnkontrolEnum.PruebaCplanProd;
            //    }
            //    else if (vSesiones.sesionEmpresaActual == 2)
            //    {
            //        baseDatos = EnkontrolEnum.PruebaArrenADM;
            //    }
            //    else
            //    {
            //        throw new Exception("Empresa distinta a Construplan y Arrendadora");
            //    }
            //}

            return baseDatos;
        }
        public static IEnumerable<Tuple<string, int, int>> MonthsBetween(DateTime startDate, DateTime endDate)
        {
            DateTime iterator;
            DateTime limit;

            if (endDate > startDate)
            {
                iterator = new DateTime(startDate.Year, startDate.Month, 1);
                limit = endDate;
            }
            else
            {
                iterator = new DateTime(endDate.Year, endDate.Month, 1);
                limit = startDate;
            }

            var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;
            while (iterator <= limit)
            {
                yield return Tuple.Create(dateTimeFormat.GetMonthName(iterator.Month), iterator.Year, iterator.Month);
                iterator = iterator.AddMonths(1);
            }
        }
        public Dictionary<string, object> cargarDashboard(FiltrosDashboardDTO filtros)
        {
            try
            {
                List<int> filtroListaDivisiones = filtros.listaDivisiones != null ? filtros.listaDivisiones : new List<int>();

                var listaDivisiones = _context.tblM_KBDivision.Where(x => x.estatus && (filtroListaDivisiones.Count() > 0 ? filtroListaDivisiones.Contains(x.id) : true)).Select(x => new { id = x.id, division = x.division }).ToList();

                IEnumerable<int> listaDivisiones_id = listaDivisiones.Select(y => y.id);

                var listaDivisionesDetalle = _context.tblM_KBDivisionDetalle.Where(x => x.estatus && (listaDivisiones_id.Count() > 0 ? listaDivisiones_id.Contains(x.divisionID) : true)).Select(x => new { id = x.id, acID = x.acID, ac = x.ac.areaCuenta, divisionID = x.divisionID }).ToList();

                IEnumerable<int> listaDivisionesDetalle_acID = listaDivisionesDetalle.Select(y => y.acID);

                List<int> filtroListaProyectos = filtros.listaProyectos != null ? filtros.listaProyectos : new List<int>();

                var listaProyectos = _context.tblP_CC.Where(x => x.estatus && listaDivisionesDetalle_acID.Contains(x.id) && (filtroListaProyectos.Count() > 0 ? filtroListaProyectos.Contains(x.id) : true)).Select(x => new { id = x.id, areaCuenta = x.areaCuenta, descripcion = x.descripcion }).ToList();
                List<string> listaAC = new List<string>();
                listaAC.AddRange(listaProyectos.Select(x => x.areaCuenta));
                var listaMaquinas = _context.tblM_CatMaquina.Where(x =>
                    (listaAC.Count > 0 ? listaAC.Contains(x.centro_costos) : true) &&
                    (filtros.tipo > 0 ? x.grupoMaquinaria.tipoEquipoID == filtros.tipo : true) &&
                    (filtros.grupo > 0 ? x.grupoMaquinariaID == filtros.grupo : true) &&
                    (filtros.modelo > 0 ? x.modeloEquipoID == filtros.modelo : true)
                ).Select(x => new { id = x.id, centro_costos = x.centro_costos, noEconomico = x.noEconomico, modeloEquipoID = x.modeloEquipoID, modelo = x.modeloEquipo.descripcion, grupoMaquinariaID = x.grupoMaquinariaID }).ToList();
                var equipos = listaMaquinas.Select(y => y.noEconomico).ToList();
                var modelos = listaMaquinas.Select(y => new { modeloID = y.modeloEquipoID, modelo = y.modelo }).Distinct().ToList();
                var listaHorometros = _context.tblM_CapHorometro.Where(x =>
                        equipos.Contains(x.Economico) &&
                        DbFunctions.TruncateTime(x.Fecha) >= DbFunctions.TruncateTime(filtros.fechaInicial) &&
                        DbFunctions.TruncateTime(x.Fecha) <= DbFunctions.TruncateTime(filtros.fechaFinal)
                    ).GroupBy(y => new
                    {
                        y.Economico,
                        y.Fecha.Year,
                        y.Fecha.Month
                    }).Select(z => new horasMesDTO
                    {
                        Economico = z.Key.Economico,
                        Year = z.Key.Year,
                        Mes = z.Key.Month,
                        Horas = z.Sum(h => h.HorasTrabajo)
                    });
                List<int> filtroListaConceptos = filtros.listaConceptos != null ? filtros.listaConceptos : new List<int>();

                var listaConceptos = _context.tblM_ControlPresupuestalConcepto.Where(x => x.estatus && (filtroListaConceptos.Count() > 0 ? filtroListaConceptos.Contains(x.id) : true)).Select(x => new { id = x.id, concepto = x.concepto, descripcion = x.descripcion }).ToList();

                var caratulaPadreAutorizada = _context.tblM_Caratula.Where(x => x.autorizada == 1).OrderByDescending(x => x.fechaAutorizacion).FirstOrDefault();

                if (caratulaPadreAutorizada == null)
                {
                    throw new Exception("No se encontró información de caratula autorizada");
                }
                var listaCaratula = _context.tblM_CaratulaDet.Where(x => x.caratula == caratulaPadreAutorizada.idCaratula).Select(x => new
                {
                    id = x.id,
                    idModelo = x.idModelo,
                    depreciacionMXN = x.depreciacionMXN,
                    seguroMXN = x.seguroMXN,
                    filtroMXN = x.filtroMXN,
                    mantenimientoMXN = x.mantenimientoMXN,
                    depreciacionOHMXN = x.depreciacionOHMXN,
                    aceiteMXN = x.aceiteMXN,
                    carilleriaMXN = x.carilleriaMXN,
                    ansulMXN = x.ansulMXN
                }).ToList();
                #region Filtrar Pólizas

                var consulta = @"
                    SELECT
                        c.descripcion AS economico,
                        pol.year,
                        pol.mes, 
                        SUM(mov.monto) as monto,
                        cu.conceptoID
                    FROM sc_polizas pol
                        INNER JOIN sc_movpol mov ON pol.year = mov.year AND pol.mes = mov.mes AND pol.poliza = mov.poliza AND pol.tp = mov.tp
                        INNER JOIN cc c ON mov.cc = c.cc
                        INNER JOIN CPLAN_CTRL_PTAL_CUENTAS cu ON mov.cta = cu.cta AND mov.scta = cu.scta AND mov.sscta = cu.sscta
                    WHERE ";
                var listaParametros = new List<OdbcParameterDTO>();

                //if (filtros.fechaInicial != null && filtros.fechaFinal != null)
                //{
                consulta += @"fechapol BETWEEN ? AND ? AND";

                listaParametros.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.Date, valor = filtros.fechaInicial });
                listaParametros.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.Date, valor = filtros.fechaFinal });
                //}

                consulta += @" economico IN (" + string.Join(", ", listaMaquinas.Select(x => "'" + x.noEconomico + "'")) + @") ";

                consulta += @" group by economico, pol.year, pol.mes, cu.conceptoID";

                var listaPolizas = _contextEnkontrol.Select<MovimientoPolizaDTO>(getEnkontrolEnumADM(), new OdbcConsultaDTO() { consulta = consulta, parametros = listaParametros });
                #endregion

                var Datos = new List<Economico_MesDTO>();

                #region Datos Base
                var graficaTendenciaPresupuestoReal = new GraficaDTO();
                var meses = MonthsBetween(filtros.fechaInicial, filtros.fechaFinal);
                foreach (var mes in meses)
                {
                    var totalPresupuesto = 0m;
                    var totalReal = 0m;

                    foreach (var maquina in listaMaquinas)
                    {
                        var division = listaDivisionesDetalle.FirstOrDefault(x => x.ac == maquina.centro_costos);
                        var objs = GlobalUtils.ParseEnumToCtrlPresupuestal<ConceptoEnum>();
                        objs.ForEach(x =>
                        {
                            x.noEconomico = maquina.noEconomico;
                            x.divisionID = division.divisionID;
                            x.acID = division.acID;
                            x.ac = maquina.centro_costos;
                            x.modeloID = maquina.modeloEquipoID;
                            x.year = mes.Item2;
                            x.mes = mes.Item3;
                            x.presupuesto = 0;
                            x.real = 0;
                            x.total = 0;
                        }
                        );

                        #region Presupuesto
                        var caratulasModelo = listaCaratula.FirstOrDefault(x => x.idModelo == maquina.modeloEquipoID);

                        decimal horasTotal = 0;
                        if (caratulasModelo != null)
                        {
                            var listaHorometrosFiltrada = listaHorometros.FirstOrDefault(x => x.Economico == maquina.noEconomico && x.Year == mes.Item2 && x.Mes == mes.Item3);
                            if (listaHorometrosFiltrada != null)
                            {
                                horasTotal = listaHorometrosFiltrada.Horas;

                                //if () //Horas
                                //{
                                objs[((int)ConceptoEnum.depreciacion - 1)].horas = horasTotal;
                                objs[((int)ConceptoEnum.depreciacion - 1)].presupuesto = caratulasModelo.depreciacionMXN * horasTotal;
                                objs[((int)ConceptoEnum.seguro - 1)].presupuesto = caratulasModelo.seguroMXN * horasTotal;
                                objs[((int)ConceptoEnum.filtros - 1)].presupuesto = caratulasModelo.filtroMXN * horasTotal;
                                objs[((int)ConceptoEnum.correctivo - 1)].presupuesto = caratulasModelo.mantenimientoMXN * horasTotal;
                                objs[((int)ConceptoEnum.depreciacionOverhaul - 1)].presupuesto = caratulasModelo.depreciacionOHMXN * horasTotal;
                                objs[((int)ConceptoEnum.aceite - 1)].presupuesto = caratulasModelo.aceiteMXN * horasTotal;
                                objs[((int)ConceptoEnum.carrileria - 1)].presupuesto = caratulasModelo.carilleriaMXN * horasTotal;
                                objs[((int)ConceptoEnum.ansul - 1)].presupuesto = caratulasModelo.ansulMXN * horasTotal;
                                objs[((int)ConceptoEnum.otros - 1)].presupuesto = 0;
                                //}
                                //else //Días
                                //{
                                //    //Todavía no se define.
                                //}
                            }
                        }
                        #endregion
                        #region Real
                        var listaPolizasEconomico = listaPolizas.Where(x => x.year == mes.Item2 && x.mes == mes.Item3 && x.economico == maquina.noEconomico);

                        if (listaPolizasEconomico != null)
                        {
                            objs[((int)ConceptoEnum.depreciacion - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.depreciacion)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.seguro - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.seguro)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.filtros - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.filtros)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.correctivo - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.correctivo)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.depreciacionOverhaul - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.depreciacionOverhaul)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.aceite - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.aceite)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.carrileria - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.carrileria)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.ansul - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.ansul)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.otros - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.otros)).Sum(x => x.monto);
                            objs[((int)ConceptoEnum.danos - 1)].real = listaPolizasEconomico.Where(x => x.conceptoID == ((int)ConceptoEnum.danos)).Sum(x => Math.Abs(x.monto));
                        }
                        #endregion

                        Datos.AddRange(objs);
                    }

                    graficaTendenciaPresupuestoReal.meses.Add(string.Join("", mes.Item1.Take(3)) + "-" + mes.Item2);
                    graficaTendenciaPresupuestoReal.serie1Descripcion = "Ppto";
                    graficaTendenciaPresupuestoReal.serie1.Add(totalPresupuesto);

                    graficaTendenciaPresupuestoReal.serie2Descripcion = "Real";
                    graficaTendenciaPresupuestoReal.serie2.Add(totalReal);
                }
                Datos.ForEach(x => x.total = (x.presupuesto - x.real));
                #endregion
                IEnumerable<int> listaConceptos_id = listaConceptos.Select(y => y.id);
                var listaCuentas = _context.tblM_ControlPresupuestalConceptoCuenta.Where(x => x.estatus && (listaConceptos_id.Count() > 0 ? listaConceptos_id.Contains(x.conceptoID) : true)).Select(x => new { id = x.id, conceptoID = x.conceptoID, cta = x.cta, scta = x.scta, sscta = x.sscta }).ToList();
                var cuentasDepreciacion = listaCuentas.Where(x => x.conceptoID == 1);
                var cuentasSeguro = listaCuentas.Where(x => x.conceptoID == 2);
                var cuentasFiltros = listaCuentas.Where(x => x.conceptoID == 3);
                var cuentasCorrectivo = listaCuentas.Where(x => x.conceptoID == 4);
                var cuentasDepreciacionOverhaul = listaCuentas.Where(x => x.conceptoID == 5);
                var cuentasAceite = listaCuentas.Where(x => x.conceptoID == 6);
                var cuentasCarrileria = listaCuentas.Where(x => x.conceptoID == 7);
                var cuentasAnsul = listaCuentas.Where(x => x.conceptoID == 8);
                var cuentasOtros = listaCuentas.Where(x => x.conceptoID == 9);
                var cuentasDanos = listaCuentas.Where(x => x.conceptoID == 10);

                if (filtros.FiltroBusqueda == 0)
                {
                    #region Tabla Empresa-División
                    var tablaEmpresaDivision = new
                    {
                        data = new List<Dictionary<string, object>>(),
                        columns = new List<dynamic>()
                    };

                    tablaEmpresaDivision.columns.Add(new
                    {
                        data = "division",
                        title = "División"
                    });

                    var count = 1;
                    foreach (var mes in meses)
                    {
                        tablaEmpresaDivision.columns.Add(new
                        {
                            data = "mes" + count,
                            title = string.Join("", mes.Item1.Take(3)) + "-" + mes.Item2
                        });
                        count++;
                    }
                    #endregion

                    #region Gráfica Empresa-División
                    var graficaEmpresaDivision = new
                    {
                        meses = new List<string>(),
                        series = new List<dynamic>()
                    };

                    foreach (var mes in meses)
                    {
                        graficaEmpresaDivision.meses.Add(string.Join("", mes.Item1.Take(3)) + "-" + mes.Item2);
                    }

                    foreach (var division in listaDivisiones)
                    {
                        var data = new List<decimal>();
                        var renglonTabla = new Dictionary<string, object>();
                        renglonTabla.Add("division", division.division);

                        var contadorRenglon = 1;
                        foreach (var mes in meses)
                        {

                            var diferenciaDivision = Datos.Where(x => x.divisionID == division.id && x.year == mes.Item2 && x.mes == mes.Item3).Sum(x => x.total);

                            data.Add(diferenciaDivision);

                            renglonTabla.Add(("mes" + contadorRenglon), diferenciaDivision);

                            contadorRenglon++;
                        }

                        if (data.Sum(s => s) != 0)
                        {
                            graficaEmpresaDivision.series.Add(new
                            {
                                name = division.division,
                                data = data
                            });

                            tablaEmpresaDivision.data.Add(renglonTabla);
                        }
                    }
                    #endregion

                    #region Tabla Proyecto
                    var tablaProyecto = new
                    {
                        data = new List<Dictionary<string, object>>(),
                        columns = new List<dynamic>()
                    };

                    tablaProyecto.columns.Add(new
                    {
                        data = "proyecto",
                        title = "Proyecto"
                    });

                    var count2 = 1;
                    foreach (var mes in meses)
                    {
                        tablaProyecto.columns.Add(new
                        {
                            data = "mes" + count2,
                            title = string.Join("", mes.Item1.Take(3)) + "-" + mes.Item2
                        });
                        count2++;
                    }
                    #endregion

                    #region Gráfica Proyectos
                    var graficaProyecto = new
                    {
                        meses = new List<string>(),
                        series = new List<dynamic>()
                    };

                    foreach (var mes in meses)
                    {
                        graficaProyecto.meses.Add(string.Join("", mes.Item1.Take(3)) + "-" + mes.Item2);
                    }

                    foreach (var proyecto in listaProyectos)
                    {
                        var data = new List<decimal>();
                        var renglonTabla = new Dictionary<string, object>();
                        renglonTabla.Add("proyecto", proyecto.descripcion);

                        var contadorRenglon = 1;
                        foreach (var mes in meses)
                        {
                            var diferenciaProyecto = Datos.Where(x => x.acID == proyecto.id && x.year == mes.Item2 && x.mes == mes.Item3).Sum(x => x.total);

                            data.Add(diferenciaProyecto);

                            renglonTabla.Add(("mes" + contadorRenglon), diferenciaProyecto);

                            contadorRenglon++;
                        }

                        if (data.Sum(s => s) != 0)
                        {
                            graficaProyecto.series.Add(new
                            {
                                name = proyecto.areaCuenta + " - " + proyecto.descripcion,
                                data = data
                            });

                            tablaProyecto.data.Add(renglonTabla);
                        }
                    }
                    #endregion

                    #region Tabla Modelo
                    var tablaModelo = new
                    {
                        data = new List<Dictionary<string, object>>(),
                        columns = new List<dynamic>()
                    };

                    tablaModelo.columns.Add(new
                    {
                        data = "modelo",
                        title = "Modelo"
                    });

                    var count3 = 1;
                    foreach (var mes in meses)
                    {
                        tablaModelo.columns.Add(new
                        {
                            data = "mes" + count3,
                            title = string.Join("", mes.Item1.Take(3)) + "-" + mes.Item2
                        });
                        count3++;
                    }
                    #endregion

                    #region Gráfica Modelos
                    var graficaModelo = new
                    {
                        meses = new List<string>(),
                        series = new List<dynamic>()
                    };

                    foreach (var mes in meses)
                    {
                        graficaModelo.meses.Add(string.Join("", mes.Item1.Take(3)) + "-" + mes.Item2);
                    }


                    foreach (var modelo in modelos)
                    {
                        var data = new List<decimal>();
                        var renglonTabla = new Dictionary<string, object>();
                        renglonTabla.Add("modelo", modelo.modelo);

                        var contadorRenglon = 1;
                        foreach (var mes in meses)
                        {
                            var diferenciaModelo = Datos.Where(x => x.modeloID == modelo.modeloID && x.year == mes.Item2 && x.mes == mes.Item3).Sum(x => x.total);

                            data.Add(diferenciaModelo);

                            renglonTabla.Add(("mes" + contadorRenglon), diferenciaModelo);

                            contadorRenglon++;
                        }

                        if (data.Sum(s => s) != 0)
                        {
                            graficaModelo.series.Add(new
                            {
                                name = modelo.modelo,
                                data = data
                            });

                            tablaModelo.data.Add(renglonTabla);
                        }
                    }
                    #endregion

                    #region Tabla Concepto
                    var tablaConcepto = new
                    {
                        data = new List<Dictionary<string, object>>(),
                        columns = new List<dynamic>()
                    };

                    tablaConcepto.columns.Add(new
                    {
                        data = "concepto",
                        title = "Concepto"
                    });

                    var count4 = 1;
                    foreach (var mes in meses)
                    {
                        tablaConcepto.columns.Add(new
                        {
                            data = "mes" + count4,
                            title = string.Join("", mes.Item1.Take(3)) + "-" + mes.Item2
                        });
                        count4++;
                    }
                    #endregion

                    #region Gráfica Conceptos
                    var graficaConcepto = new
                    {
                        meses = new List<string>(),
                        series = new List<dynamic>()
                    };

                    foreach (var mes in meses)
                    {
                        graficaConcepto.meses.Add(string.Join("", mes.Item1.Take(3)) + "-" + mes.Item2);
                    }

                    foreach (var concepto in listaConceptos)
                    {
                        var data = new List<decimal>();
                        var renglonTabla = new Dictionary<string, object>();
                        renglonTabla.Add("concepto", concepto.descripcion);

                        var contadorRenglon = 1;
                        foreach (var mes in meses)
                        {
                            var diferenciaConcepto = Datos.Where(x => x.conceptoID == concepto.id && x.year == mes.Item2 && x.mes == mes.Item3).Sum(x => x.total);

                            data.Add(diferenciaConcepto);

                            renglonTabla.Add(("mes" + contadorRenglon), diferenciaConcepto);

                            contadorRenglon++;
                        }

                        if (data.Sum(s => s) != 0)
                        {
                            graficaConcepto.series.Add(new
                            {
                                name = concepto.descripcion,
                                data = data
                            });
                        }

                        tablaConcepto.data.Add(renglonTabla);
                    }
                    #endregion

                    #region Gráfica Diagrama Pareto Presupuesto
                    var graficaDiagramaParetoPresupuesto = new
                    {
                        conceptos = new List<string>(),
                        series = new List<dynamic>()
                    };

                    var dataParetoPresupuesto = new List<Tuple<string, decimal>>();
                    var dataParetoPresupuestoPorcentaje = new List<decimal>();
                    var dataParetoPresupuestoPorcentajeFijo = new List<decimal>();
                    var presupuestoPareto = 0m;

                    foreach (var concepto in listaConceptos)
                    {
                        presupuestoPareto = Datos.Where(x => x.conceptoID == concepto.id).Sum(x => x.presupuesto);
                        dataParetoPresupuesto.Add(new Tuple<string, decimal>(concepto.descripcion, presupuestoPareto));
                        dataParetoPresupuestoPorcentajeFijo.Add(80);
                    }

                    graficaDiagramaParetoPresupuesto.series.Add(new
                    {
                        name = "",
                        data = dataParetoPresupuesto.OrderByDescending(x => x.Item2).Select(x => x.Item2).ToList()
                    });

                    var valorMaximo = dataParetoPresupuesto.Select(x => x.Item2).Max();

                    foreach (var conceptoPresupuesto in dataParetoPresupuesto.OrderByDescending(x => x.Item2).ToList())
                    {
                        graficaDiagramaParetoPresupuesto.conceptos.Add(conceptoPresupuesto.Item1);

                        var porcentajeEquivalente = (conceptoPresupuesto.Item2 * 100) / (valorMaximo > 0 ? valorMaximo : 1);
                        var porcentajeResiduo = Math.Truncate(100 * (100 - porcentajeEquivalente)) / 100;

                        dataParetoPresupuestoPorcentaje.Add(porcentajeResiduo);
                    }

                    graficaDiagramaParetoPresupuesto.series.Add(new
                    {
                        name = "",
                        data = dataParetoPresupuestoPorcentaje
                    });
                    graficaDiagramaParetoPresupuesto.series.Add(new
                    {
                        name = "",
                        data = dataParetoPresupuestoPorcentajeFijo
                    });
                    #endregion

                    #region Gráfica Diagrama Pareto Real
                    //var graficaDiagramaParetoReal = new
                    //{
                    //    conceptos = new List<string>(),
                    //    series = new List<dynamic>()
                    //};

                    //var dataParetoReal = new List<Tuple<string, decimal>>();
                    //var dataParetoRealPorcentaje = new List<decimal>();
                    //var dataParetoRealPorcentajeFijo = new List<decimal>();
                    //var realPareto = 0m;

                    //foreach (var concepto in listaConceptos)
                    //{
                    //    foreach (var maquina in listaMaquinas)
                    //    {
                    //        #region Real
                    //        var listaPolizasEconomico = listaPolizas.Where(x => x.economico == maquina.noEconomico).ToList();

                    //        var cuentasConcepto = listaCuentas.Where(x => x.conceptoID == (int)concepto.concepto).ToList();

                    //        var movimientosPolizaConcepto = (from pol in listaPolizasEconomico join cuen in cuentasConcepto on new { pol.cta, pol.scta, pol.sscta } equals new { cuen.cta, cuen.scta, cuen.sscta } select pol).ToList();

                    //        var realConceptoMaquina = movimientosPolizaConcepto.Sum(x => x.monto);
                    //        #endregion

                    //        realPareto += realConceptoMaquina;
                    //    }

                    //    dataParetoReal.Add(new Tuple<string, decimal>(concepto.descripcion, realPareto));
                    //    dataParetoRealPorcentajeFijo.Add(80);
                    //}

                    //graficaDiagramaParetoReal.series.Add(new
                    //{
                    //    name = "",
                    //    data = dataParetoReal.OrderByDescending(x => x.Item2).Select(x => x.Item2).ToList()
                    //});

                    //var valorMaximoReal = dataParetoReal.Select(x => x.Item2).Max();

                    //foreach (var conceptoReal in dataParetoReal.OrderByDescending(x => x.Item2).ToList())
                    //{
                    //    graficaDiagramaParetoReal.conceptos.Add(conceptoReal.Item1);

                    //    var porcentajeEquivalente = (conceptoReal.Item2 * 100) / (valorMaximoReal > 0 ? valorMaximoReal : 1);
                    //    var porcentajeResiduo = Math.Truncate(100 * (100 - porcentajeEquivalente)) / 100;

                    //    dataParetoRealPorcentaje.Add(porcentajeResiduo);
                    //}

                    //graficaDiagramaParetoReal.series.Add(new
                    //{
                    //    name = "",
                    //    data = dataParetoRealPorcentaje
                    //});
                    //graficaDiagramaParetoReal.series.Add(new
                    //{
                    //    name = "",
                    //    data = dataParetoRealPorcentajeFijo
                    //});
                    #endregion

                    resultado.Add("graficaEmpresaDivision", graficaEmpresaDivision);
                    resultado.Add("tablaEmpresaDivision", tablaEmpresaDivision);
                    resultado.Add("graficaProyecto", graficaProyecto);
                    resultado.Add("tablaProyecto", tablaProyecto);
                    resultado.Add("graficaModelo", graficaModelo);
                    resultado.Add("tablaModelo", tablaModelo);
                    resultado.Add("graficaConcepto", graficaConcepto);
                    resultado.Add("tablaConcepto", tablaConcepto);
                    resultado.Add("graficaDiagramaParetoPresupuesto", graficaDiagramaParetoPresupuesto);
                    //resultado.Add("graficaDiagramaParetoReal", graficaDiagramaParetoReal);
                    resultado.Add(SUCCESS, true);
                }

            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "cargarDashboard", e, AccionEnum.CONSULTA, 0, filtros);
            }

            return resultado;
        }

        public Dictionary<string, object> getComboDivision()
        {
            try
            {
                var listaDivisiones = _context.tblM_KBDivision.Where(x => x.estatus).Select(x => new ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = x.division
                }).OrderBy(x => x.Text).ToList();

                resultado.Add(ITEMS, listaDivisiones);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "getComboDivision", e, AccionEnum.CONSULTA, 0, null);
            }

            return resultado;
        }
        public Dictionary<string, object> getComboConcepto()
        {
            try
            {
                var items = listaConceptos.Select(x => new ComboDTO
                {
                    Value = x.Item1.ToString(),
                    Text = x.Item2
                }).OrderBy(x => x.Text).ToList();

                resultado.Add(ITEMS, items);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "getComboConcepto", e, AccionEnum.CONSULTA, 0, null);
            }

            return resultado;
        }

        #region LISTA CONCEPTOS
        private List<Tuple<int, string>> listaConceptos = new List<Tuple<int, string>> {
            new Tuple<int, string>(1, "Depreciación"),
            new Tuple<int, string>(2, "Seguro"),
            new Tuple<int, string>(3, "Filtros"),
            new Tuple<int, string>(4, "Correctivo"),
            new Tuple<int, string>(5, "Depreciación Overhaul"),
            new Tuple<int, string>(6, "Aceites"),
            new Tuple<int, string>(7, "Carrelería"),
            new Tuple<int, string>(8, "Ansul"),
        };
        #endregion
        public dynamic getCboCC(List<int> divisionesIDs)
        {
            switch (vSesiones.sesionEmpresaActual)
            {
                case (int)EmpresaEnum.Construplan:
                    return _context.tblP_CC.Where(x => x.estatus).ToList().Select(c => new
                    {
                        Text = c.cc + " - " + c.descripcion,
                        Value = c.id,
                        Prefijo = c.cc
                    }).OrderBy(o => o.Prefijo);
                case (int)EmpresaEnum.Arrendadora:
                    {
                        divisionesIDs = divisionesIDs ?? new List<int>();
                        var listaDivisionesDetalle = _context.tblM_KBDivisionDetalle.Where(x => x.estatus && (divisionesIDs.Count > 0 ? divisionesIDs.Contains(x.divisionID) : true)).Select(x => new { id = x.id, acID = x.acID, divisionID = x.divisionID }).ToList();
                        List<int> listaDivisionesDetalle_acID = listaDivisionesDetalle.Select(y => y.acID).ToList();
                        var data = _context.tblP_CC.Where(x => x.estatus && (listaDivisionesDetalle_acID.Count > 0 ? listaDivisionesDetalle_acID.Contains(x.id) : true)).Select(c => new
                        {
                            Text = c.areaCuenta + " - " + c.descripcion,
                            Value = c.id,
                            Prefijo = c.areaCuenta,
                            area = c.area,
                            cuenta = c.cuenta
                        }).OrderBy(o => o.area).ThenBy(o => o.cuenta).ToList();
                        return data;
                    }
                case (int)EmpresaEnum.Colombia:
                    return _context.tblP_CC.Where(x => x.estatus).ToList().Select(c => new
                    {
                        Text = c.cc + " - " + c.descripcion,
                        Value = c.id,
                        Prefijo = c.cc
                    }).OrderBy(o => o.Prefijo);
                default:
                    return null;
            }
        }
    }
}
