using Core.DAO.Maquinaria.Captura;
using Core.DTO;
using Core.DTO.COMPRAS;
using Core.DTO.Maquinaria.Captura;
using Core.DTO.Maquinaria.Captura.MaquinariaRentada;
using Core.DTO.Principal.Generales;
using Core.DTO.Utils.Data;
using Core.Entity.Maquinaria;
using Core.Entity.Maquinaria.Captura;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Principal.Multiempresa;
using Core.Enum.Administracion.Cotizaciones;
using Core.Enum.Maquinaria.MaquinariaRentada;
using Core.Enum.Multiempresa;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Captura
{
    public class RN_MaquinariaDAO : GenericDAO<tblM_RN_Maquinaria>, IRN_MaquinariaDAO
    {
        // Metodos nueva renta
        public Dictionary<string, object> GetInfoReporteTiempoRequeridoVsUtilizado(List<int> idsAreasCuenta, List<int> idsCentrosCosto, DateTime periodoInicio, DateTime periodoFinal)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var tiempoRequeridoVsUtilizado = _context.tblM_RN_Maquinaria.Where(
                                                    x => idsAreasCuenta.Contains(x.IdAreaCuenta) &&
                                                    idsCentrosCosto.Contains(x.IdCentroCosto) &&
                                                    x.PeriodoDel >= periodoInicio &&
                                                    x.PeriodoA <= periodoFinal)
                                                 .ToList();
                var periodosRequeridosDTO = tiempoRequeridoVsUtilizado.Select(x => new RN_Maquinaria_PeriodosDTO
                                            {
                                                IdRenta = x.Id,
                                                IdPeriodoInicial = x.PeriodoInicial,
                                                PeriodoInicio = x.PeriodoDel,
                                                PeriodoFinal = x.PeriodoA
                                            }).ToList();
                var tiempoRequeridoVsUtilizadoDTO = tiempoRequeridoVsUtilizado.Where(z => z.PeriodoInicial == 0).Select(
                                                    x => new RN_Maquinaria_TiempoRequeridoVsUtilizadoDTO
                                                    {
                                                        AreaCuenta = x.AreaCuenta.descripcion +  " - " + x.CC.noEconomico,
                                                        CentroCosto = x.CC.noEconomico,
                                                        TiempoRequerido = periodosRequeridosDTO.Where(y => y.IdRenta == x.Id && y.IdPeriodoInicial == 0).Select(s => new { DiasUtilizados = (s.PeriodoFinal - s.PeriodoInicio).Days}).Sum(sum => sum.DiasUtilizados) == 0 ?
                                                                          1 : periodosRequeridosDTO.Where(y => y.IdRenta == x.Id && y.IdPeriodoInicial == 0).Select(s => new { DiasUtilizados = (s.PeriodoFinal - s.PeriodoInicio).Days}).Sum(sum => sum.DiasUtilizados),
                                                        TiempoUtilizado = periodosRequeridosDTO.Where(y => y.IdPeriodoInicial == x.Id && y.IdPeriodoInicial != 0).Select(s => new { DiasUtilizados = ((s.PeriodoFinal - s.PeriodoInicio).Days) }).Sum(sum => sum.DiasUtilizados) == 0 ?
                                                                          periodosRequeridosDTO.Where(y => y.IdRenta == x.Id && y.IdPeriodoInicial == 0).Select(s => new { DiasUtilizados = (s.PeriodoFinal - s.PeriodoInicio).Days}).Sum(sum => sum.DiasUtilizados) == 0 ?
                                                                          1 : periodosRequeridosDTO.Where(y => y.IdRenta == x.Id && y.IdPeriodoInicial == 0).Select(s => new { DiasUtilizados = (s.PeriodoFinal - s.PeriodoInicio).Days}).Sum(sum => sum.DiasUtilizados) :
                                                                          periodosRequeridosDTO.Where(y => y.IdPeriodoInicial == x.Id && y.IdPeriodoInicial != 0).Select(s => new { DiasUtilizados = ((s.PeriodoFinal - s.PeriodoInicio).Days) }).Sum(sum => sum.DiasUtilizados) +
                                                                          (periodosRequeridosDTO.Where(y => y.IdRenta == x.Id && y.IdPeriodoInicial == 0).Select(s => new { DiasUtilizados = (s.PeriodoFinal - s.PeriodoInicio).Days }).Sum(sum => sum.DiasUtilizados) == 0 ?
                                                                          1 : periodosRequeridosDTO.Where(y => y.IdRenta == x.Id && y.IdPeriodoInicial == 0).Select(s => new { DiasUtilizados = (s.PeriodoFinal - s.PeriodoInicio).Days }).Sum(sum => sum.DiasUtilizados))

                                                    }).ToList();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, tiempoRequeridoVsUtilizadoDTO);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al obtener las maquinas rentadas.\n" + ex.ToString());
            }
            return resultado;
        }

        public Dictionary<string, object> GetMaquinasRentadas(List<int> idAreaCuenta, List<int> idCentroCosto, DateTime periodoDel, DateTime periodoA)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var maquinasRentadas = _context.tblM_RN_Maquinaria.Where(
                                            x => idAreaCuenta.Contains(x.IdAreaCuenta) &&
                                            idCentroCosto.Contains(x.IdCentroCosto) &&
                                            x.PeriodoDel >= periodoDel &&
                                            x.PeriodoA <= periodoA)
                                       .Select(x => new MaquinasRentadasDTO
                                       {
                                           Id = x.Id,
                                           Folio = x.Folio,
                                           CentroCosto = x.CC.noEconomico,
                                           AreaCuenta = x.AreaCuenta.descripcion,
                                           PeriodoDel = x.PeriodoDel,
                                           PeriodoA = x.PeriodoA,
                                           TotalRenta = x.TotalRenta,
                                           OrdenCompra = x.OrdenCompra,
                                           RentaTerminada = x.HorometroFinal == null ? false : true,
                                           DifHora = x.DifHora,
                                           CargoDaño = x.CargoDaño,
                                           Fletes = x.Fletes,
                                           IdTipoMoneda = x.IdTipoMoneda
                                       }).ToList();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, maquinasRentadas);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al obtener las maquinas rentadas.\n" + ex.ToString());
            }
            return resultado;
        }

        public Dictionary<string, object> GetInfoMaquinaRentada(int idMaquinaRentada)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var maquinaRentada = _context.tblM_RN_Maquinaria.FirstOrDefault(x => x.Id == idMaquinaRentada && x.Activo == true);
                if (maquinaRentada != null)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, ConstruirDtoMaquinaRentada(maquinaRentada));
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "No se encontró registro");
                }
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al obtener la maquina rentada.\n" + ex.ToString());
            }
            return resultado;
        }

        public Dictionary<string, object> GetAreasCuentaPorUsuario()
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                //var areasCuentasActuales = _context.tblP_CC.Select(x => new ComboDTO
                //{
                //    Value = x.areaCuenta,
                //    Text = x.areaCuenta + " - " + x.descripcion,
                //    Prefijo = x.id.ToString()
                //}).ToList();

                //var usuarioId = vSesiones.sesionUsuarioDTO.id;
                //var listaAreasCuentausuario = _context.tblP_CC_Usuario.Where(x => x.usuarioID == usuarioId).Select(x => x.cc).ToList();
                //var comboBoxAreaCuenta = areasCuentasActuales.Where(x => listaAreasCuentausuario.Contains(x.Value)).Select(y => new ComboDTO
                //{
                //    //Value = y.Value,
                //    //Text = y.Text,
                //    //Prefijo = y.Prefijo
                //    Value = y.Prefijo,
                //    Text = y.Text,
                //    Prefijo = y.Value
                //}).ToList();

                //if (listaAreasCuentausuario.Contains("1010"))
                //{
                //    comboBoxAreaCuenta.Add(new ComboDTO
                //    {
                //        Value = "60",
                //        Text = "1010 - TALLER DE MAQUINARIA",
                //        Prefijo = "1010"
                //    });
                //}
                //if (listaAreasCuentausuario.Contains("1015"))
                //{
                //    comboBoxAreaCuenta.Add(new ComboDTO
                //    {
                //        Value = "47",
                //        Text = "1015 - PATIO DE MAQUINARIA",
                //        Prefijo = "1015"
                //    });
                //}
                if (vSesiones.sesionEmpresaActual == 1 || vSesiones.sesionEmpresaActual == 2)
                 {
                    var areasCuentasActuales = _context.tblP_CC.Select(x => new ComboDTO
                 {
                 Value = x.id.ToString(),
                 Text = x.areaCuenta + " - " + x.descripcion,
                 Prefijo = x.areaCuenta
                  }).ToList();
                    resultado.Add(ITEMS, areasCuentasActuales.OrderBy(x => x.Value));
                }
                else if(vSesiones.sesionEmpresaActual==3)
                {
                    var areasCuentasActuales = _context.tblP_CC.Select(x => new ComboDTO
                    {
                        Value = x.id.ToString(),
                        Text = x.areaCuenta + " - " + x.descripcion,
                        Prefijo = x.areaCuenta
                    }).ToList();
                    resultado.Add(ITEMS, areasCuentasActuales.OrderBy(x => x.Value));
                }
                else if(vSesiones.sesionEmpresaActual==6){
                    var areasCuentasActuales = _context.tblP_CC.Select(x => new ComboDTO
                    {
                        Value = x.id.ToString(),
                        Text = x.areaCuenta + " - " + x.descripcion,
                        Prefijo = x.areaCuenta
                    }).ToList();
                    resultado.Add(ITEMS, areasCuentasActuales.OrderBy(x => x.Value));
                }
             

                resultado.Add(SUCCESS, true);
                //resultado.Add(ITEMS, areasCuentasActuales.OrderBy(x => x.Value));
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al obtener las área cuenta.\n" + ex.ToString());
            }
            return resultado;
        }

        public Dictionary<string, object> GetCentrosCostoRentados(List<int> idAreasCuenta)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var centrosCosto = _context.tblM_RN_Maquinaria.Where(x => idAreasCuenta.Contains(x.IdAreaCuenta)).Select(x => new ComboDTO
                {
                    Value = x.IdCentroCosto.ToString(),
                    Text = x.CC.noEconomico,
                    Prefijo = x.CC.noEconomico
                }).Distinct().ToList();
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, centrosCosto);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al obtener los centros de costo.\n" + ex.ToString());
            }
            return resultado;
        }

        public Dictionary<string, object> TerminarRentaMaquina(int idRentaMaquina)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var rentaTerminada = _context.tblM_RN_Maquinaria.Where(x => x.Id == idRentaMaquina && x.HorometroFinal == null && x.Activo == true).FirstOrDefault();
                if (rentaTerminada != null)
                {
                    var horometroFinal = GetHorometroPorPeriodoYCentroCosto(rentaTerminada.PeriodoDel, rentaTerminada.PeriodoA, rentaTerminada.CC.noEconomico, (int)rentaTerminada.HorometroInicial, (int)rentaTerminada.HorometroFinal);
                    if ((bool)horometroFinal[SUCCESS])
                    {
                        var hmFinal = (HorometroDTO)horometroFinal[ITEMS];
                        if (hmFinal.HorometroFinal != null)
                        {
                            rentaTerminada.HorometroFinal = hmFinal.HorometroFinal;
                            var rentaTerminadaActualizada = Update(rentaTerminada, rentaTerminada.Id);
                            if (rentaTerminadaActualizada != null)
                            {
                                resultado.Add(SUCCESS, true);
                                rentaTerminadaActualizada.HorometroInicial = hmFinal.HorometroInicial;
                                rentaTerminadaActualizada.HorometroFinal = null;
                                resultado.Add(ITEMS, ConstruirDtoMaquinaRentada(rentaTerminadaActualizada));
                            }
                            else
                            {
                                resultado.Add(SUCCESS, false);
                                resultado.Add(MESSAGE, "Ocurrió un error al actualizar el horometroFinal (terminar renta)");
                            }
                        }
                        else
                        {
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "No se encontró un horometro registrado para el periodo final de este centro de costo");
                        }
                    }
                    else
                    {
                        return horometroFinal;
                    }
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "No se encontró el registro de la renta");
                }
                return resultado;
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al terminar la renta.\n" + ex.ToString());
            }
            return resultado;
        }

        public Dictionary<string, object> GetCentrosCosto()
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                if (vSesiones.sesionEmpresaActual == 2)
                {
                    var centrosCosto = _context.tblM_CatMaquina.Where(x => x.estatus != 0 && !string.IsNullOrEmpty(x.noEconomico)).Select(y => new ComboDTO
                    {
                        Value = y.id.ToString(),
                        Text = y.noEconomico,
                        Prefijo = y.id.ToString()
                    }).OrderBy(x => x.Text).Where(x => x.Text.Contains("-R")).ToList();
                    if (centrosCosto.Count > 0)
                    {
                        resultado.Add(SUCCESS, true);
                        resultado.Add(ITEMS, centrosCosto);
                    }
                }else if(vSesiones.sesionEmpresaActual==3){

                    var centrosCostoCol = _context.tblM_CatMaquina.Where(x => x.estatus != 0 && !string.IsNullOrEmpty(x.noEconomico)).Select(y => new ComboDTO
                    {
                        Value = y.id.ToString(),
                        Text = y.noEconomico,
                        Prefijo = y.id.ToString()
                    }).ToList();
                  
                        resultado.Add(SUCCESS, true);
                        resultado.Add(ITEMS, centrosCostoCol);

                }
                else if (vSesiones.sesionEmpresaActual == 6)
                {
                    var centrosCostoPeru = _context.tblM_CatMaquina.Where(x => x.estatus != 0 && !string.IsNullOrEmpty(x.noEconomico)).Select(y => new ComboDTO
                    {
                        Value = y.id.ToString(),
                        Text = y.noEconomico,
                        Prefijo = y.id.ToString()
                    }).ToList();
                  
                        resultado.Add(SUCCESS, true);
                        resultado.Add(ITEMS, centrosCostoPeru);
                }
              
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "No se encontraron centros de costo.");
                }
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, true);
                resultado.Add(MESSAGE, "Ocurrió un error al obtener los centros de costo.\n" + ex.ToString());
            }
            return resultado;
        }

        public Dictionary<string, object> GetInformacionCentroCosto(int idCentroCosto)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var existeRentaAbierta = _context.tblM_RN_Maquinaria.FirstOrDefault(x => x.IdCentroCosto == idCentroCosto && x.Activo == true && x.HorometroFinal == null);
                if (existeRentaAbierta == null)
                {
                    var informacionCentroCosto = _context.tblM_CatMaquina.Where(x => x.id == idCentroCosto && x.estatus != 0).Select(x => new CentroCostoDTO()
                    {
                        Id = x.id,
                        Cc = x.noEconomico,
                        Equipo = x.descripcion,
                        IdModelo = x.modeloEquipoID,
                        Modelo = x.modeloEquipo.descripcion,
                        NumeroSerie = x.noSerie,
                        Proveedor = x.proveedor,
                        AreaCuenta = x.centro_costos,
                        NumeroAreaCuenta = x.centro_costos
                    }).FirstOrDefault();
                    if (informacionCentroCosto != null)
                    {
                        var areaCuenta = GetAreaCuentaPorCc(informacionCentroCosto.NumeroAreaCuenta);
                        if ((bool)areaCuenta[SUCCESS])
                        {
                            var ac = (AreaCuentaDTO)areaCuenta[ITEMS];
                            informacionCentroCosto.AreaCuenta = ac.Nombre;
                            var horometroInicial = GetHorometroPorCentroCosto(informacionCentroCosto.Cc);
                            if ((bool)horometroInicial[SUCCESS])
                            {
                                var hm = (HorometroDTO)horometroInicial[ITEMS];
                                informacionCentroCosto.HorometroInicial = hm.HorometroInicial;
                                resultado.Add(SUCCESS, true);
                                resultado.Add(ITEMS, informacionCentroCosto);
                            }
                            else
                            {
                                resultado.Add(SUCCESS, false);
                                resultado.Add(MESSAGE, (string)horometroInicial[MESSAGE]);
                            }
                        }
                        else
                        {
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, (string)areaCuenta[MESSAGE]);
                        }
                    }
                    else
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "No se encontró información del centro de costo");
                    }
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "No se puede utilizar este centro de costo debido a que existe un registro abierto con este centro de costo.\n Folio: " + existeRentaAbierta.Folio);
                }
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, true);
                resultado.Add(MESSAGE, "Ocurrió un error al consultar la base de datos para obtener información del centro de costo.\n" + ex.ToString());
            }
            return resultado;
        }

        public Dictionary<string, object> RegistrarRenta(tblM_RN_Maquinaria informacionRenta, string tipoRenta)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                var resultado = new Dictionary<string, object>();
                try
                {
                    var rentaTipo = Convert.ToInt32(tipoRenta);
                    if (rentaTipo == (int)TipoCapturaRenta.Editar)
                    {
                        informacionRenta.FechaModificacion = DateTime.Now;
                        informacionRenta.IdUsuario = vSesiones.sesionUsuarioDTO.id;
                        informacionRenta.Activo = true;

                        try
                        {
                            var operacionTerminada = Update(informacionRenta, informacionRenta.Id);
                            if (operacionTerminada != null)
                            {
                                resultado.Add(SUCCESS, true);
                            }
                            else
                            {
                                resultado.Add(SUCCESS, false);
                                resultado.Add(MESSAGE, "Ha ocurrido un error al actualizar la información");
                            }
                        }
                        catch (Exception ex)
                        {
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "Ocurrió un error al actualziar la maquina rentada \n" + ex.ToString());
                        }
                    }
                    else
                    {
                        var cc = GetInformacionCentroCosto(informacionRenta.IdCentroCosto);
                        if (!(bool)cc[SUCCESS])
                        {
                            return cc;
                        }
                        var centroCosto = (CentroCostoDTO)cc[ITEMS];

                        // SE VERIFICA SI EL ECONOMICO CUENTA CON PROVEEDOR
                        //if (string.IsNullOrEmpty(centroCosto.Proveedor) || centroCosto.Proveedor == "N/A")
                        //    throw new Exception("La maquina no cuenta con proveedor.");

                        var ac = GetAreaCuentaPorCc(centroCosto.NumeroAreaCuenta);
                        if (!(bool)ac[SUCCESS])
                        {
                            return ac;
                        }
                        var areaCuenta = (AreaCuentaDTO)ac[ITEMS];
                        informacionRenta.IdAreaCuenta = areaCuenta.Id;
                        var folio = GetFolioMaquinaRentada(centroCosto.Cc, areaCuenta.Abreviacion, informacionRenta.IdCentroCosto, informacionRenta.IdAreaCuenta);
                        if (!(bool)folio[SUCCESS])
                        {
                            return folio;
                        }

                        informacionRenta.FechaCreacion = DateTime.Now;
                        informacionRenta.FechaModificacion = DateTime.Now;
                        informacionRenta.IdUsuario = vSesiones.sesionUsuarioDTO.id;
                        informacionRenta.Activo = true;
                        informacionRenta.Folio = (string)folio[ITEMS];

                        if (!(rentaTipo == (int)TipoCapturaRenta.Renovar))
                        {
                            var proveedor = GetProveedorEkPorNombreProveedor(centroCosto.Proveedor, informacionRenta.IdTipoMoneda);
                            if (!(bool)proveedor[SUCCESS])
                            {
                                //return proveedor;
                                informacionRenta.IdProveedor = 0;
                                informacionRenta.PeriodoInicial = 0;
                            }
                            else
                            {
                                var pr = (ProveedoresDTO)proveedor[ITEMS];
                                informacionRenta.IdProveedor = pr.noProveedor;
                                informacionRenta.PeriodoInicial = 0;
                            }
                        }

                        try
                        {
                            SaveEntity(informacionRenta);
                            resultado.Add(SUCCESS, true);
                            resultado.Add(ITEMS, informacionRenta.Folio);
                        }
                        catch (Exception ex)
                        {
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "Ocurrió un error al realizar la operación \n" + ex.ToString());
                        }
                    }
                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                    dbContextTransaction.Rollback();
                }
                return resultado;
            }
        }

        private Dictionary<string, object> GetAreaCuentaPorCc(string centroCosto)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var areaCuenta = _context.tblP_CC.Where(x => x.areaCuenta == centroCosto).Select(x => new AreaCuentaDTO
                {
                    Id = x.id,
                    AreaCuenta = x.areaCuenta,
                    Nombre = x.descripcion,
                    Abreviacion = x.abreviacion
                }).FirstOrDefault();
                if (areaCuenta != null)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, areaCuenta);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "No se encontró un área cuenta para el centro de costo seleccionado");
                }
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al consultar la base de datos para obtener la información del área cuenta.\n" + ex.ToString());
            }
            return resultado;
        }

        private MaquinaRentadaDTO ConstruirDtoMaquinaRentada(tblM_RN_Maquinaria objMaquina)
        {
            MaquinaRentadaDTO maquinaRentada = new MaquinaRentadaDTO
            {
                Id = objMaquina.Id,
                Folio = objMaquina.Folio,
                PeriodoInicial = objMaquina.PeriodoInicial,
                IdCentroCosto = objMaquina.IdCentroCosto,
                CentroCosto = objMaquina.CC.noEconomico,
                IdProveedor = objMaquina.IdProveedor,
                Proveedor = objMaquina.CC.proveedor,
                Equipo = objMaquina.CC.descripcion,
                NumeroSerie = objMaquina.CC.noSerie,
                Modelo = objMaquina.CC.modeloEquipo.descripcion,
                IdAreaCuenta = objMaquina.IdAreaCuenta,
                AreaCuenta = objMaquina.AreaCuenta.descripcion,
                NumeroFactura = objMaquina.NumFactura,
                DepGarantia = objMaquina.DepGarantia,
                TramiteDG = objMaquina.TramiteDG,
                NotaCredito = objMaquina.NotaCredito,
                AplicaNC = objMaquina.AplicaNC,
                BaseHoraMensual = objMaquina.BaseHoraMensual,
                PeriodoDel = objMaquina.PeriodoDel,
                PeriodoA = objMaquina.PeriodoA,
                HorometroInicial = objMaquina.HorometroInicial,
                HorometroFinal = objMaquina.HorometroFinal,
                HorasTrabajadas = objMaquina.HorasTrabajadas,
                HorasExtras = objMaquina.HorasExtras,
                TotalHorasExtras = objMaquina.TotalHorasExtras,
                PrecioPorMes = objMaquina.PrecioPorMes,
                SeguroPorMes = objMaquina.SeguroPorMes,
                IVA = objMaquina.IVA,
                TotalRenta = objMaquina.TotalRenta,
                OrdenCompra = objMaquina.OrdenCompra,
                ContraRecibo = objMaquina.ContraRecibo,
                Anotaciones = objMaquina.Anotaciones,
                IdTipoMoneda = objMaquina.IdTipoMoneda,
                DifHora = objMaquina.DifHora,
                DifHoraExtra = objMaquina.DifHoraExtra,
                DifFactura = objMaquina.DifFactura,
                DifContraRecibo = objMaquina.DifContraRecibo,
                DifOrdenCompra = objMaquina.DifOrdenCompra,
                DifFechaContraRecibo = objMaquina.DifFechaContraRecibo,
                CargoDaño = objMaquina.CargoDaño,
                CargoDañoFactura = objMaquina.CargoDañoFactura,
                CargoDañoOrdenCompra = objMaquina.CargoDañoOrdenCompra,
                Fletes = objMaquina.Fletes,
                FletesFactura = objMaquina.FletesFactura,
                FletesOrdenCompra = objMaquina.FletesOrdenCompra

            };
            return maquinaRentada;
        }

        public Dictionary<string, object> GetHorometroPorPeriodoYCentroCosto(DateTime periodoInicio, DateTime periodoFinal, string Cc, int horometroInicial = 0, int horometroFinal = 0)
        {
            var resultado = new Dictionary<string, object>();
            var periodos = new List<DateTime>() { periodoInicio, periodoFinal };
            var horometros = new HorometroDTO();

            try
            {
                List<HorometroDTO> hm = _context.tblM_CapHorometro.Where(x => x.Economico == Cc && periodos.Contains(x.Fecha)).OrderBy(x => x.Horometro).Select(x => new HorometroDTO
                {
                    Id = x.id,
                    AreaCuenta = x.CC,
                    CentroCosto = x.Economico,
                    HorometroInicial = x.Horometro,
                    HorometroFinal = x.HorometroAcumulado,
                    Fecha = x.Fecha
                }).ToList();

                if (hm.Count > 0)
                {
                    horometros.Id = hm[0].Id;
                    horometros.AreaCuenta = hm[0].AreaCuenta;
                    horometros.CentroCosto = hm[0].CentroCosto;
                    if (hm.Count == 2)
                    {
                        horometros.HorometroInicial = hm[0].HorometroInicial;
                        horometros.HorometroFinal = hm[1].HorometroFinal;
                    }
                    else
                    {
                        foreach (var h in hm)
                        {
                            if (h.Fecha == periodoInicio)
                            {
                                horometros.HorometroInicial = h.HorometroInicial;
                            }
                            if (h.Fecha == periodoFinal)
                            {
                                horometros.HorometroFinal = h.HorometroInicial;
                            }
                        }
                    }
                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, horometros);
                }
                else if (horometroInicial > 0 && horometroFinal > 0)
                {
                    HorometroDTO objDTO = new HorometroDTO();
                    objDTO.HorometroInicial = horometroInicial;
                    objDTO.HorometroFinal = horometroFinal;
                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, objDTO);
                }
                else
                {
                    #region VALIDACIONES
                    if (horometroInicial <= 0) { throw new Exception("Es necesario indicar el horometro inicial."); }
                    if (horometroFinal <= 0) { throw new Exception("Es necesario indicar el horometro final."); }
                    #endregion

                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "No se encontraron horometros registrados para el centro de costo segun el periodo seleccionado");
                }
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                //resultado.Add(MESSAGE, "Ocurrió un error al consultar la base de datos para obtener el horometro.\n" + ex.ToString());
                resultado.Add(MESSAGE, ex.Message);
            }
            return resultado;
        }

        private Dictionary<string, object> GetHorometroPorCentroCosto(string Cc)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var hm = _context.tblM_CapHorometro.Where(x => x.Economico == Cc).OrderByDescending(x => x.Horometro).Select(x => new HorometroDTO
                {
                    Id = x.id,
                    AreaCuenta = x.CC,
                    CentroCosto = x.Economico,
                    HorometroInicial = x.Horometro,
                    HorometroFinal = x.HorometroAcumulado
                }).FirstOrDefault();
                if (hm != null)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, hm);
                }
                else
                {
                    //resultado.Add(SUCCESS, false);
                    //resultado.Add(MESSAGE, "No se encontró información del horometro");
                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, new HorometroDTO { HorometroInicial = 0.0M, HorometroFinal = 0.0M });
                }
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al consultar la base de datos para obtener el horometro.\n" + ex.ToString());
            }
            return resultado;
        }

        private Dictionary<string, object> GetFolioMaquinaRentada(string centroCosto, string areaCuentaAbreviacion, int idCentroCosto, int idAreaCuenta)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var consecutivofolio = _context.tblM_RN_Maquinaria.Where(x => x.IdCentroCosto == idCentroCosto && x.IdAreaCuenta == idAreaCuenta).ToList().Count + 1;
                if (consecutivofolio != 0)
                {
                    var consecutivo = "";
                    consecutivo += consecutivofolio;

                    var folio = areaCuentaAbreviacion + "-" + centroCosto + "-" + consecutivo;
                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, folio);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ha ocurridó un error al crear el folio");
                }
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al consultar la base de datos para generar folio\n" + ex.ToString());
            }
            return resultado;
        }

        private Dictionary<string, object> GetProveedorEkPorNombreProveedor(string nombreProveedor, int idTipoMoneda)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
                {
                    case EmpresaEnum.Construplan:
                        {
                            var pro = _context.tblCom_sp_proveedores
                                .Where(x =>
                                    x.statusAutorizacion &&
                                    x.registroActivo &&
                                    x.nombre.Contains(nombreProveedor)).FirstOrDefault();

                            var proveedor = new ProveedoresDTO();
                            proveedor.noProveedor = pro.numpro;
                            proveedor.nomProveedor = pro.nomcorto;
                            proveedor.tipoCambio = 2;

                            if (proveedor == null)
                            {
                                resultado.Add(SUCCESS, false);
                                resultado.Add(MESSAGE, "No se encontró información del proveedor en sigoplan");
                            }
                            else
                            {
                                resultado.Add(SUCCESS, true);
                                resultado.Add(ITEMS, proveedor);
                            }
                        }
                        break;
                    default:
                        {
                            var odbc = new OdbcConsultaDTO();
                            odbc.consulta = @"SELECT * FROM dba.sp_proveedores WHERE cancelado = 'A' AND moneda = ? AND nombre like ?";
                            odbc.parametros.Add(new OdbcParameterDTO()
                            {
                                nombre = "moneda",
                                tipo = OdbcType.Int,
                                valor = idTipoMoneda
                            });
                            odbc.parametros.Add(new OdbcParameterDTO()
                            {
                                nombre = "nombre",
                                tipo = OdbcType.VarChar,
                                valor = "%" + nombreProveedor.Trim() + "%"
                            });
                            List<dynamic> proveedores = _contextEnkontrol.Select<dynamic>(EnkontrolAmbienteEnum.Prod, odbc);

                            if (proveedores == null || proveedores.Count == 0)
                            {
                                resultado.Add(SUCCESS, false);
                                resultado.Add(MESSAGE, "No se encontró información del proveedor en enkontrol");
                            }
                            else
                            {
                                var proveedor = proveedores.Select(x => new ProveedoresDTO
                                {
                                    noProveedor = Convert.ToInt64(x.numpro),
                                    nomProveedor = (string)x.nombre,
                                    tipoCambio = int.Parse(x.moneda)
                                }).FirstOrDefault();

                                resultado.Add(SUCCESS, true);
                                resultado.Add(ITEMS, proveedor);
                            }
                        }
                        break;
                }
                
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al consultar la base de datos para obtener el número de proveedor en enkontrol\n" + ex.ToString());
            }
            return resultado;
        }
        // Metodos nueva renta fin
    }
}