﻿using Core.DAO.Maquinaria.Barrenacion;
using Core.DTO.Utils.Data;
using Core.Entity.Maquinaria.Captura;
using Core.Enum.Multiempresa;
using Data.EntityFramework.Context;
using Core.DTO;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Odbc;
using System.Linq;
using Core.DTO.Principal.Generales;
using Core.Enum.Principal.Bitacoras;
using Core.Entity.Maquinaria.Barrenacion;
using Core.Enum.Maquinaria.Barrenacion;
using Core.DTO.Administracion.Facultamiento;
using Core.Entity.Administrativo.FacultamientosDpto;
using Core.Entity.Maquinaria;
using Infrastructure.Utils;
using Core.DTO.Maquinaria.Barrenacion;
using System.Globalization;
using Core.DTO.Maquinaria.Barrenacion.Reporte;
using System.Web;
using Core.DTO.Maquinaria.Reporte;
using Data.Factory.Maquinaria.Captura;
using Core.DTO.Maquinaria.Inventario.Controles;
using System.Web.SessionState;
using Core.DTO.Enkontrol.Alamcen;
using Core.Enum.Principal;

namespace Data.DAO.Maquinaria.Barrenacion
{
    public class BarrenacionDAO : GenericDAO<tblM_CatAceitesLubricantes>, IBarrenacionDAO
    {

        #region variables y constructor
        private readonly Dictionary<string, object> resultado = new Dictionary<string, object>();
        private PrecioDieselFactoryServices precioDieselFactoryServices = new PrecioDieselFactoryServices();
        KPIFactoryServices kpiFactoryServices = new KPIFactoryServices();
        private const string nombreControlador = "BarrenacionController";
        private bool productivo = Convert.ToString(System.Web.Configuration.WebConfigurationManager.AppSettings["enkontrolProductivo"]) == "1";

        /// <summary>
        /// Constructor
        /// </summary>
        public BarrenacionDAO()
        {
            resultado.Clear();
        }
        #endregion

        #region Combos
        public Dictionary<string, object> ObtenerAC()
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                List<ComboDTO> listaAC = _context.tblP_CC.Where(x => x.estatus && x.descripcion.Contains("minado"))
                    .OrderBy(x => x.area).ThenBy(x => x.cuenta)
                    .Select(archivo => new ComboDTO
                    {
                        Text = archivo.areaCuenta + " - " + archivo.descripcion,
                        Value = archivo.areaCuenta
                    }).ToList();

                resultado.Add(ITEMS, listaAC);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, nombreControlador, "ObtenerAC", e, AccionEnum.CONSULTA, 0, null);
            }
            return resultado;
        }

        public Dictionary<string, object> GetComboBancos(string areaCuenta)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                List<ComboDTO> listaAC = _context.tblB_CatalogoBanco.Where(b => b.estatus && b.areaCuenta == areaCuenta)
                    .Select(banco => new ComboDTO
                    {
                        Text = banco.banco,
                        Value = banco.id.ToString()
                    }).ToList();

                resultado.Add(ITEMS, listaAC);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, nombreControlador, "ObtenerAC", e, AccionEnum.CONSULTA, 0, null);
            }
            return resultado;
        }
        #endregion

        #region Mano de Obra
        public Dictionary<string, object> ObtenerBarrenadorasOperadores(string areaCuenta, int estatusOperadores)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                var listaEquipos = _context.tblB_Barrenadora
                    .Where(x => areaCuenta == "Todos" ? true : x.maquina.centro_costos == areaCuenta && x.activa)
                    .ToList()
                    .Where(x => estatusOperadores == 2 ? true : x.operadoresAsignados == Convert.ToBoolean(estatusOperadores))
                    .ToList();

                var listaBarrenadoras = listaEquipos.Select(x => new
                {
                    id = x.id,
                    noEconomico = x.maquina.noEconomico,
                    descripcion = x.maquina.descripcion,
                    noSerie = x.maquina.noSerie,
                    estatus = x.operadoresAsignados ? "Operadores asignados" : "Sin asignar"
                });

                resultado.Add("listaBarrenadoras", listaBarrenadoras);
                resultado.Add(SUCCESS, true);

                return resultado;
            }
            catch (Exception e)
            {
                LogError(0, 0, nombreControlador, "ObtenerBarrenadorasOperadores", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                return resultado;
            }
        }

        public Dictionary<string, object> ObtenerOperadoresBarrenadora(int barrenadoraID, int turno)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                List<tblB_ManoObra> operadores = _context.tblB_ManoObra.Where(x => x.activo).ToList();

                if (operadores.Count > 0)
                {
                    var listaOperadores = operadores.Select(x => new
                    {
                        claveEmpleado = x.claveEmpleado,
                        descripcion = ObtenerNombreEmpleadoPorClave(x.claveEmpleado),
                        tipoOperador = x.tipoOperador,
                        sueldo = x.sueldo,
                        jornada = x.jornada,
                        fsr = x.fsr

                    }).ToList();

                    resultado.Add(ITEMS, listaOperadores);
                }
                else
                {
                    resultado.Add(ITEMS, null);
                }

                resultado.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al obtener los operadores de la barrenadora.");
            }

            return resultado;
        }

        private string ObtenerNombreEmpleadoPorClave(int claveEmpleado)
        {
//            var odbc = new OdbcConsultaDTO();
//            odbc.consulta = @"
//                    SELECT (e.nombre + ' ' + e.ape_paterno + ' ' + e.ape_materno) as label FROM sn_empleados e
//                    WHERE clave_empleado = ?";
//            odbc.parametros.Add(new OdbcParameterDTO()
//            {
//                nombre = "claveEmpleado",
//                tipo = OdbcType.Decimal,
//                valor = Convert.ToDecimal(claveEmpleado)
//            });
//            List<dynamic> listaEmpleados = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanRh, odbc);
            

            var listaEmpleados = _context.Select<dynamic>(new DapperDTO
            {
                baseDatos = MainContextEnum.Construplan,
                consulta = @"SELECT (e.nombre + ' ' + e.ape_paterno + ' ' + e.ape_materno) as label FROM tblRH_EK_Empleados e
                            WHERE clave_empleado = @claveEmpleado",
                parametros = new { claveEmpleado }
            });

            var empleado = listaEmpleados.OrderByDescending(e => e.id).FirstOrDefault();
            return empleado != null ? empleado.label : "INDEFINIDO";
        }

        public dynamic ObtenerEmpleadosEnKontrol(string term, bool porDesc)
        {
            try
            {
//                var odbc = new OdbcConsultaDTO();
//                odbc.consulta = @"
//                    SELECT TOP 12 (e.nombre + ' ' + e.ape_paterno + ' ' + e.ape_materno) as label, e.clave_empleado as id,s.suma / tn.num_dias as sueldo FROM sn_empleados  e
//                    INNER JOIN DBA.sn_tipos_nomina AS tn  ON E.tipo_nomina = tn.tipo_nomina
//                    INNER JOIN DBA.sn_tabulador_historial AS s on e.clave_empleado=s.clave_empleado
//                    INNER JOIN (SELECT f.clave_empleado , MAX(f.id) as idmax FROM DBA.sn_tabulador_historial f GROUP BY f.clave_empleado ) tb on tb.clave_empleado = e.clave_empleado and tb.idmax = s.id
//                    WHERE" + (porDesc ? @" label " : @" id ") + @"LIKE ? ORDER BY id";
//                odbc.parametros.Add(new OdbcParameterDTO()
//                {
//                    nombre = "label",
//                    tipo = OdbcType.VarChar,
//                    valor = (string)"%" + term.Trim() + "%"
//                });
//                List<empleadoDTO> listaEmpleados = _contextEnkontrol.Select<empleadoDTO>(EnkontrolEnum.CplanRh, odbc);

                var listaEmpleados = _context.Select<empleadoDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT TOP 12 (e.nombre + ' ' + e.ape_paterno + ' ' + e.ape_materno) as label, e.clave_empleado as id,s.suma / tn.num_dias as sueldo 
                                FROM tblRH_EK_Empleados  e
                                INNER JOIN tblRH_EK_Tipos_Nomina AS tn  ON E.tipo_nomina = tn.tipo_nomina
                                INNER JOIN tblRH_EK_Tabulador_Historial AS s on e.clave_empleado=s.clave_empleado
                                INNER JOIN (SELECT f.clave_empleado , MAX(f.id) as idmax FROM tblRH_EK_Tabulador_Historial f GROUP BY f.clave_empleado ) tb on tb.clave_empleado = e.clave_empleado and tb.idmax = s.id
                                WHERE" + (porDesc ? @" label " : @" id ") + @"LIKE '%@term%' ORDER BY id",
                    parametros = new { term = term.Trim()}
                });

                if (porDesc)
                {
                    return listaEmpleados.Select(x => new
                    {
                        id = x.id.ToString(),
                        value = x.label.ToString(),
                        sueldo = x.sueldo.ToString("0.##")
                    }).ToList();
                }
                else
                {
                    return listaEmpleados.Select(x => new
                    {
                        id = x.label.ToString(),
                        value = x.id.ToString(),
                        sueldo = x.sueldo.ToString("0.##")
                    }).ToList();
                }

            }
            catch (Exception)
            {
                return null;
            }
        }


        public Dictionary<string, object> GuardarOperadoresBarrenadora(List<tblB_ManoObra> listaOperadores)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    int usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;

                    if (listaOperadores == null || listaOperadores.Count == 0)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "La lista de operadores viene vacía.");
                        return resultado;
                    }

                    int barrenadoraID = listaOperadores.First().barrenadoraID;
                    int turno = listaOperadores.First().turno;

                    // Si ya había empleados asignados, los da de baja.
                    var operadoresExistentes = _context.tblB_ManoObra.Where(x => x.barrenadoraID == barrenadoraID && x.activo && x.turno == turno).ToList();

                    if (operadoresExistentes.Count > 0)
                    {
                        operadoresExistentes.ForEach(operadorExistente =>
                        {
                            // Verifica que no sea igual al ya existente.
                            var operadorPorAgregar = listaOperadores.FirstOrDefault(y => y.tipoOperador == operadorExistente.tipoOperador);

                            if (operadorPorAgregar != null && operadorPorAgregar.claveEmpleado != operadorExistente.claveEmpleado)
                            {
                                operadorExistente.fechaBaja = DateTime.Now;
                                operadorExistente.activo = false;

                                if (operadorPorAgregar.claveEmpleado == 0)
                                {
                                    listaOperadores.Remove(operadorPorAgregar);
                                }
                            }
                            // Si ya existe, lo elimna de la lista por agregar.
                            else
                            {
                                listaOperadores.Remove(operadorPorAgregar);
                            }
                        });
                        _context.SaveChanges();
                    }

                    listaOperadores.RemoveAll(x => x.claveEmpleado == 0);

                    if (listaOperadores.Count > 0)
                    {
                        // Da de alta a los nuevos operadores
                        listaOperadores.ForEach(operador =>
                        {
                            operador.fechaAlta = DateTime.Now;
                            operador.fechaBaja = null;
                            operador.activo = true;
                            operador.usuarioCreadorID = usuarioCreadorID;
                        });

                        _context.tblB_ManoObra.AddRange(listaOperadores);
                        _context.SaveChanges();
                    }

                    ActualizarEstatusOperadoresBarrenadora(barrenadoraID);

                    _context.SaveChanges();
                    resultado.Add(SUCCESS, true);
                    dbTransaction.Commit();

                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, nombreControlador, "GuardarOperadoresBarrenadora", e, AccionEnum.ACTUALIZAR, 0, listaOperadores);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error al guardar los operadores de la barrenadora.");
                }
                return resultado;
            }
        }

        private void ActualizarEstatusOperadoresBarrenadora(int barrenadoraID)
        {
            // Verifica si hay 2 empleados activos en un mismo turno.
            var operadoresAsignados = _context.tblB_ManoObra
                .Where(x => x.barrenadoraID == barrenadoraID && x.activo)
                .GroupBy(x => x.turno)
                .Any(x => x.Any(y => y.tipoOperador == TipoOperadorEnum.Operador && y.activo));

            var barrenadora = _context.tblB_Barrenadora.FirstOrDefault(x => x.id == barrenadoraID);
            barrenadora.operadoresAsignados = operadoresAsignados;
            _context.SaveChanges();
        }
        #endregion

        #region Piezas Barrendaora

        public Dictionary<string, object> GetPiezaID(int piezaID)
        {
            try
            {

                var piezas = _context.tblB_PiezaBarrenadora.FirstOrDefault(r => r.id == piezaID);
                resultado.Add(SUCCESS, piezas != null ? true : false);
                resultado.Add("pieza", piezas);

            }
            catch (Exception)
            {

                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al obtener las piezas de la barrenadora.");
            }
            return resultado;
        }
        public Dictionary<string, object> ObtenerBarrenadorasPiezas(string areaCuenta, int estatusPiezas)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                var listaEquipos = _context.tblB_Barrenadora
                    .Where(x => areaCuenta == "Todos" ? true : x.maquina.centro_costos == areaCuenta && x.activa)
                    .ToList()
                    .Where(x => estatusPiezas == 2 ? true : x.piezasAsignadas == Convert.ToBoolean(estatusPiezas))
                    .ToList();

                var listaBarrenadoras = listaEquipos.Select(x => new
                {
                    id = x.id,
                    noEconomico = x.maquina.noEconomico,
                    descripcion = x.maquina.descripcion,
                    noSerie = x.maquina.noSerie,
                    estatus = x.piezasAsignadas ? "Completa" : "Piezas faltantes"
                });

                resultado.Add("listaBarrenadoras", listaBarrenadoras);
                resultado.Add(SUCCESS, true);

                return resultado;
            }
            catch (Exception e)
            {
                LogError(0, 0, nombreControlador, "ObtenerBarrenadorasPiezas", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al buscar las barrenadoras.");
                return resultado;
            }
        }

        public Dictionary<string, object> ObtenerPiezasBarrenadora(int barrenadoraID, string areaCuenta)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;
                var barrenadora = _context.tblB_Barrenadora.FirstOrDefault(x => x.id == barrenadoraID);

                List<tblB_PiezaBarrenadora> listaPiezas = _context.tblB_PiezaBarrenadora.ToList().Where(x => x.activa && x.montada && x.barrenadoraID == barrenadoraID && x.areaCuenta == areaCuenta).ToList();

                if (listaPiezas.Count > 0)
                {
                    //var listaPiezasInsumos = listaPiezas.Select(x => x.insumo).ToList();
                    //var almacen = 144;
                    //var auxAlmacen = _context.tblAlm_RelAreaCuentaXAlmacen.FirstOrDefault(x => x.AreaCuenta == areaCuenta);
                    //if (auxAlmacen != null) { 
                    //    var auxAlmacenDet = _context.tblAlm_RelAreaCuentaXAlmacenDet.FirstOrDefault(x => x.idRelacion == auxAlmacen.id);
                    //    if (auxAlmacenDet != null) almacen = auxAlmacenDet.Almacen;
                    //}
                    //var costosPromedio = getCostoPromedioKardex(almacen, listaPiezasInsumos);

                    foreach (var pieza in listaPiezas)
                    {
                        //var precio = costosPromedio.FirstOrDefault(x => x.insumo == pieza.insumo);
                        //if (precio != null) pieza.precio = precio.costoPromedio;


                        //if (pieza.precio <= 0)
                        //{
                            

                            if (barrenadora != null)
                            {
                                var maquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == barrenadora.maquinaID);
                                var cc = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == maquina.centro_costos).cc;

                                var ultimaCompraEK = _contextEnkontrol.Where(
                                    string.Format(@"SELECT AVG(X.tipo_cambio * X.precio) as precio FROM
                                        (SELECT TOP 5
                                            oc.tipo_cambio,
                                            det.precio
                                        FROM so_orden_compra oc INNER JOIN so_orden_compra_det det ON oc.cc = det.cc AND oc.numero = det.numero 
                                        WHERE oc.cc = '{0}' AND det.insumo = {1}  
                                        ORDER BY oc.fecha DESC) X", cc, pieza.insumo), 1);
                                decimal precio = 0;
                                if (ultimaCompraEK.Count > 0)
                                {
                                    if (ultimaCompraEK[0].precio!=null)
                                    {
                                        precio = Convert.ToDecimal(((List<dynamic>)ultimaCompraEK.ToObject<List<dynamic>>())[0].precio, CultureInfo.InvariantCulture);
                                    }
                                   
                                    //decimal tipoCambio = Convert.ToDecimal(((List<dynamic>)ultimaCompraEK.ToObject<List<dynamic>>())[0].tipo_cambio, CultureInfo.InvariantCulture);

                                    //pieza.precio = precio * tipoCambio;
                                    pieza.precio = precio;
                                }
                            }
                        //}
                    }
                }

                resultado.Add(ITEMS, listaPiezas.Count > 0 ? listaPiezas : null);
                resultado.Add("piezasAsignadas", barrenadora == null ? false : barrenadora.piezasAsignadas);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al obtener las piezas de la barrenadora.");
            }

            return resultado;
        }

        public Dictionary<string, object> ObtenerInsumosPorPiezaPrecio(string areaCuenta)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                var listaTipoPiezas = EnumExtensions.ToCombo<TipoPiezaEnum>();


                foreach (var tipoPiezaDTO in listaTipoPiezas)
                {
                    List<insumosPorPiezaDTO> listaInsumos = new List<insumosPorPiezaDTO>();
                    List<PiezasBarrenadoraDTO> listaPiezas = new List<PiezasBarrenadoraDTO>();
                    TipoPiezaEnum pieza = (TipoPiezaEnum)tipoPiezaDTO.Value;

                    var listaInsumosPorPieza = _context.tblB_CatalogoPieza
                                                .Where(x => x.tipoPieza == pieza && x.activo && areaCuenta == x.areaCuenta)
                                                .OrderBy(x => x.id)
                                                .ToList();
                    List<PiezasBarrenadoraDTO> listaPiezasActivas = _context.tblB_PiezaBarrenadora.ToList().Where(r => r.tipoPieza == (TipoPiezaEnum)tipoPiezaDTO.Value && r.activa && r.areaCuenta == areaCuenta).ToList().Select(p => new PiezasBarrenadoraDTO
                    {
                        precio = p.precio,
                        horasAcumuladas = p.horasAcumuladas,
                        id = p.id,
                        serieAutomatica = p.noSerie,
                        serieManual = p.serialExcel

                    }).ToList();

                    var relacionAlmacenObra = _context.tblAlm_RelAreaCuentaXAlmacen.FirstOrDefault(x => x.AreaCuenta == areaCuenta);
                    List<int> almacenes = new List<int>();
                    if (relacionAlmacenObra != null) almacenes = _context.tblAlm_RelAreaCuentaXAlmacenDet.ToList().Where(x => x.idRelacion == relacionAlmacenObra.id).Select(x => x.Almacen).ToList();
                    //int ano = DateTime.Now.Year;
                    //var conexion = _contextEnkontrol.BaseDatosProductivoDesdeEmpresa(EmpresaEnum.Construplan);
                    foreach (var x in listaInsumosPorPieza)
                    {
                        decimal precioPza = 0;
                        precioPza = getCostoPromedioKardex(almacenes, x.insumo);

             //           var odbc = new OdbcConsultaDTO();
             //var promedioEnkontrol = _contextEnkontrol.Select<dynamic>(conexion, odbc);
             //   decimal sumPrecio = 0;
             //   if (promedioEnkontrol[0].precio != null)
             //   {
             //       sumPrecio = promedioEnkontrol[0].precio;
             //   }
             //   if (sumPrecio > 0)
             //   {
             //       return sumPrecio;
             //   }


//                        var ultimaCompraEK = _contextEnkontrol.Where(
//                                   string.Format(@"SELECT TOP 1 
//                                                        oc.tipo_cambio, 
//                                                        det.* 
//                                                    FROM so_orden_compra oc 
//                                                        INNER JOIN so_orden_compra_det det ON oc.cc = det.cc AND oc.numero = det.numero 
//                                                    WHERE det.insumo = {0} 
//                                                    ORDER BY oc.fecha DESC", x.insumo));
//                        if (ultimaCompraEK.Count > 0)
//                        {
//                            decimal precio = Convert.ToDecimal(((List<dynamic>)ultimaCompraEK.ToObject<List<dynamic>>())[0].precio, CultureInfo.InvariantCulture);
//                            decimal tipoCambio = Convert.ToDecimal(((List<dynamic>)ultimaCompraEK.ToObject<List<dynamic>>())[0].tipo_cambio, CultureInfo.InvariantCulture);

//                            precioPza = precio * tipoCambio;
//                        }

                        listaInsumos.Add((new insumosPorPiezaDTO
                        {
                            id = x.id,
                            descripcion = String.Format("{0} ({1})", x.descripcion, ObtenerUltimoPrecioInsumo(x.insumo)),
                            insumo = x.insumo,
                            precioPieza = precioPza,
                            tipoPieza = (TipoPiezaEnum)tipoPiezaDTO.Value,

                        }));

                    }
                    resultado.Add(pieza.GetDescription() + "Barrenadora", listaPiezasActivas);
                    resultado.Add(pieza.GetDescription(), listaInsumos);

                }
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, nombreControlador, "ObtenerInsumosPorPiezaPrecio", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(MESSAGE, "Ocurrió un error al buscar los insumos por pieza.");
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        private string ObtenerUltimoPrecioInsumo(int insumo)
        {
            var odbc = new OdbcConsultaDTO();
            odbc.consulta = @"SELECT TOP 1 precio FROM so_orden_compra_det as det WHERE det.insumo = ? ORDER BY fecha_recibido DESC";
            odbc.parametros.Add(new OdbcParameterDTO()
            {
                nombre = "insumo",
                tipo = OdbcType.VarChar,
                valor = (int)insumo
            });
            //  List<dynamic> listaInsumos = _contextEnkontrol.Select<dynamic>(EnkontrolAmbienteEnum.Prod, odbc);
            List<dynamic> listaInsumos = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanProd, odbc);
            var precio = listaInsumos.Select(x => new
            {
                precio = (decimal)x.precio
            }).FirstOrDefault();

            return precio != null ? String.Format("{0:0.00}", precio.precio) : "0.00";
        }

        public dynamic getInsumo(string term, bool porDesc)
        {
            try
            {
                var odbc = new OdbcConsultaDTO();
                odbc.consulta = @"
                    SELECT TOP 12 
                        insumo as Id, descripcion  as Text
                    FROM insumos
                    WHERE " + (porDesc ? @" descripcion " : @" insumo ") + @" LIKE ? 
                    ORDER BY insumo";
                odbc.parametros.Add(new OdbcParameterDTO()
                {
                    nombre = "label",
                    tipo = OdbcType.VarChar,
                    valor = "%" + (string)term.Trim() + "%"
                });

                List<ComboDTO> listaInsumos = _contextEnkontrol.Select<ComboDTO>(EnkontrolEnum.CplanProd, odbc);

                if (porDesc)
                {
                    return listaInsumos.Select(x => new
                    {
                        id = x.Id,
                        value = x.Text
                    }).ToList();
                }
                else
                {
                    return listaInsumos.Select(x => new
                    {
                        id = x.Text,
                        value = x.Id
                    }).ToList();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public dynamic ObtenerSerieMartilloReparadoNoAsignado(string term)
        {
            var catalogoPiezas = _context.tblB_CatalogoPieza.Where(x => x.activo).ToList();

            var martillosDisponibles = _context.tblB_PiezaBarrenadora.Where(x =>
                x.activa &&
                x.barrenadoraID == null &&
                x.cantidadReparaciones > 0 &&
                x.noSerie.Contains(term)
            ).Take(12).ToList().Select(y => new
            {
                id = y.id,
                value = y.noSerie,
                Prefijo = JsonUtils.Json(y)
            }).ToList();

            return martillosDisponibles;
        }

        public Dictionary<string, object> GuardarPiezasBarrenadoraViejo(List<tblB_PiezaBarrenadora> listaPiezas, string desechoMartillo, string desechoBarra)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    int usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;

                    if (listaPiezas == null || listaPiezas.Count == 0)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "La lista de piezas viene vacía.");
                        return resultado;
                    }

                    int barrenadoraID = (int)listaPiezas.First().barrenadoraID;

                    // Si ya había piezas asignadas, las da de baja.
                    var piezasExistentes = _context.tblB_PiezaBarrenadora.Where(x => x.barrenadoraID == barrenadoraID && x.activa && !x.reparando).ToList();

                    if (piezasExistentes.Count > 0)
                    {
                        piezasExistentes.ForEach(piezaExistente =>
                        {
                            var piezaPorAgregar = listaPiezas.FirstOrDefault(y => y.tipoPieza == piezaExistente.tipoPieza);

                            // Verifica que no sea igual al ya existente.
                            if (piezaPorAgregar != null && piezaPorAgregar.id != piezaExistente.id)
                            {
                                // Si es martillo, verifica si es por reparación.
                                if (piezaPorAgregar.tipoPieza == TipoPiezaEnum.Martillo && piezaPorAgregar.reparando)
                                {

                                    piezaPorAgregar.reparando = false;
                                    piezaExistente.reparando = true;
                                    piezaExistente.activa = false;
                                    //piezaExistente.barrenadoraID = null;

                                    // Se resetea el contador de horas trabajadas y se pasa a horas acumuladas
                                    piezaExistente.horasAcumuladas += piezaExistente.horasTrabajadas;
                                    piezaExistente.horasTrabajadas = 0;

                                    RegistrarMovimientoHistorial(piezaExistente, TipoMovimientoPiezaEnum.BajaReparacion);
                                }
                                else
                                {
                                    if (piezaExistente.tipoPieza == TipoPiezaEnum.Martillo)
                                    {
                                        RegistrarMovimientoHistorial(piezaExistente, TipoMovimientoPiezaEnum.BajaDesecho, desechoMartillo);
                                    }
                                    else if (piezaExistente.tipoPieza == TipoPiezaEnum.Barra)
                                    {
                                        RegistrarMovimientoHistorial(piezaExistente, TipoMovimientoPiezaEnum.BajaDesecho, desechoBarra);
                                    }
                                    else
                                    {
                                        RegistrarMovimientoHistorial(piezaExistente, TipoMovimientoPiezaEnum.BajaDesecho);
                                    }

                                    piezaExistente.activa = false;
                                    piezaExistente.barrenadoraID = 0;
                                }

                                // Si la "nueva" pieza viene sin insumo, significa que sólo se eliminó la pieza, por lo tanto se elimina de la lista de agregar.
                                if (piezaPorAgregar.insumo == 0)
                                {
                                    listaPiezas.Remove(piezaPorAgregar);
                                }
                            }
                            // Si ya existe, no se cambió nada y lo elimna de la lista por agregar.
                            else
                            {
                                listaPiezas.Remove(piezaPorAgregar);
                            }
                        });
                        _context.SaveChanges();
                    }

                    // Si un martillo reparado se volvió a montar, se actualiza la columna barrenadoraID para indicar que está montada en ese equipo.
                    var martilloReparado = listaPiezas.FirstOrDefault(x => x.tipoPieza == TipoPiezaEnum.Martillo && x.id > 0);
                    if (martilloReparado != null)
                    {
                        var martillo = _context.tblB_PiezaBarrenadora.FirstOrDefault(x => x.id == martilloReparado.id);
                        martillo.barrenadoraID = barrenadoraID;
                        _context.SaveChanges();
                        RegistrarMovimientoHistorial(martillo, TipoMovimientoPiezaEnum.AltaReparada);
                    }

                    // Se eliminan los que no tengan número de insumo.
                    listaPiezas.RemoveAll(x => x.insumo == 0);

                    if (listaPiezas.Count > 0)
                    {
                        // Se le asigna el número de serie a cada pieza
                        listaPiezas.ForEach(nuevaPieza =>
                        {
                            // Se valida que no exista otro martillo con el mismo número de serie.
                            var martilloPorAgregar = listaPiezas.FirstOrDefault(x => x.tipoPieza == TipoPiezaEnum.Martillo);
                            if (martilloPorAgregar != null)
                            {
                                if (_context.tblB_PiezaBarrenadora.Any(y => y.tipoPieza == TipoPiezaEnum.Martillo && y.noSerie == martilloPorAgregar.noSerie))
                                {
                                    throw new Exception("Ya existe una pieza de tipo martillo con ese número de serie.");
                                }
                            }

                            // Se busca el tipo de broca del insumo
                            var pieza = _context.tblB_CatalogoPieza.FirstOrDefault(x => x.insumo == nuevaPieza.insumo && x.activo);
                            nuevaPieza.tipoBroca = pieza != null ? pieza.tipoBroca : TipoBrocaEnum.NO_APLICA;

                            nuevaPieza.noSerie = ObtenerNumeroSeriePieza(nuevaPieza);
                        });

                        _context.tblB_PiezaBarrenadora.AddRange(listaPiezas);
                        _context.SaveChanges();

                        // Se registra la alta de las nuevas piezas.
                        listaPiezas.ForEach(pieza =>
                        {
                            RegistrarMovimientoHistorial(pieza, TipoMovimientoPiezaEnum.AltaNueva);
                        });
                    }

                    _context.SaveChanges();

                    ActualizarEstatusPiezasBarrenadora(barrenadoraID, true);

                    _context.SaveChanges();
                    resultado.Add(SUCCESS, true);
                    dbTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, nombreControlador, "GuardarPiezasBarrenadora", e, AccionEnum.ACTUALIZAR, 0, listaPiezas);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error al guardar las piezas de la barrenadora.");
                }
                return resultado;
            }
        }

        public Dictionary<string, object> GuardarPiezasBarrenadora(List<tblB_PiezaBarrenadora> listaPiezas, string desechoMartillo, string desechoBarra, bool pzasCompletas)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    int usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;

                    if (listaPiezas == null || listaPiezas.Count == 0)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "La lista de piezas viene vacía.");
                        return resultado;
                    }

                    int barrenadoraID = (int)listaPiezas.First().barrenadoraID;

                    var piezasMontadas = _context.tblB_PiezaBarrenadora.Where(x => !x.reparando && x.activa && x.montada && x.barrenadoraID == barrenadoraID).ToList();
                    List<tblB_PiezaBarrenadora> listaPiezasNuevas = new List<tblB_PiezaBarrenadora>();

                    foreach (var item in listaPiezas.Where(f => f.id != 0))
                    {
                        var piezaExiste = _context.tblB_PiezaBarrenadora.FirstOrDefault(r => r.id == item.id);

                        piezaExiste.serialExcel = item.serialExcel;
                        piezaExiste.precio = item.precio;

                        _context.SaveChanges();
                    }


                    foreach (var pieza in listaPiezas)
                    {
                        var piezaMontada = piezasMontadas.FirstOrDefault(x => x.tipoPieza == pieza.tipoPieza);

                        if (piezaMontada != null)
                        {
                            if (piezaMontada.tipoPieza == TipoPiezaEnum.Martillo)
                            {
                                if (pieza.reparando && !piezaMontada.reparando) //Verifica si el martillo montado pasa a repararse.
                                {
                                    // Se resetea el contador de horas trabajadas y se pasa a horas acumuladas
                                    piezaMontada.horasAcumuladas += piezaMontada.horasTrabajadas;
                                    piezaMontada.horasTrabajadas = 0;

                                    RegistrarMovimientoHistorial(piezaMontada, TipoMovimientoPiezaEnum.BajaReparacion);

                                    #region Se deshechan la culata y el cilindro asignados cuando el martillo se repara.
                                    pieza.culataID = 0;
                                    pieza.cilindroID = 0;

                                    var culata = _context.tblB_PiezaBarrenadora.FirstOrDefault(x => x.id == piezaMontada.culataID);
                                    var cilindro = _context.tblB_PiezaBarrenadora.FirstOrDefault(x => x.id == piezaMontada.cilindroID);

                                    culata.reparando = false;
                                    culata.activa = false;
                                    culata.montada = false;

                                    cilindro.reparando = false;
                                    cilindro.activa = false;
                                    cilindro.montada = false;

                                    RegistrarMovimientoHistorial(culata, TipoMovimientoPiezaEnum.BajaDesecho);
                                    RegistrarMovimientoHistorial(cilindro, TipoMovimientoPiezaEnum.BajaDesecho);

                                    piezaMontada.culataID = 0;
                                    piezaMontada.cilindroID = 0;
                                    #endregion
                                }

                                if (!pieza.activa && piezaMontada.activa) //Verifica si el martillo montado pasa a deshecharse.
                                {
                                    RegistrarMovimientoHistorial(piezaMontada, TipoMovimientoPiezaEnum.BajaDesecho, desechoMartillo);

                                    #region Se deshechan la culata y el cilindro asignados cuando el martillo se deshecha.
                                    pieza.culataID = 0;
                                    pieza.cilindroID = 0;

                                    var culata = _context.tblB_PiezaBarrenadora.FirstOrDefault(x => x.id == piezaMontada.culataID);
                                    var cilindro = _context.tblB_PiezaBarrenadora.FirstOrDefault(x => x.id == piezaMontada.cilindroID);
                                    if (culata != null)
                                    {
                                        culata.reparando = false;
                                        culata.activa = false;
                                        culata.montada = false;
                                        RegistrarMovimientoHistorial(culata, TipoMovimientoPiezaEnum.BajaDesecho);
                                    }
                                    if (cilindro != null)
                                    {
                                        cilindro.reparando = false;
                                        cilindro.activa = false;
                                        cilindro.montada = false;
                                        RegistrarMovimientoHistorial(cilindro, TipoMovimientoPiezaEnum.BajaDesecho);
                                    }
                                    piezaMontada.culataID = 0;
                                    piezaMontada.cilindroID = 0;
                                    #endregion
                                }

                                if (!pieza.montada && piezaMontada.montada) //Verifica si el martillo montado pasa a desmontarse.
                                {
                                    RegistrarMovimientoHistorial(piezaMontada, TipoMovimientoPiezaEnum.Baja, "");

                                    #region Se desmontan la culata y el cilindro asignados cuando el martillo se desmonta.
                                    var culata = _context.tblB_PiezaBarrenadora.FirstOrDefault(x => x.id == piezaMontada.culataID);
                                    var cilindro = _context.tblB_PiezaBarrenadora.FirstOrDefault(x => x.id == piezaMontada.cilindroID);
                                    if (culata != null)
                                    {
                                        culata.reparando = false;
                                        culata.activa = true;
                                        culata.montada = false;

                                        RegistrarMovimientoHistorial(culata, TipoMovimientoPiezaEnum.Baja);
                                    }
                                    if (cilindro != null)
                                    {
                                        cilindro.reparando = false;
                                        cilindro.activa = true;
                                        cilindro.montada = false;
                                        RegistrarMovimientoHistorial(cilindro, TipoMovimientoPiezaEnum.Baja);
                                    }
                                    #endregion
                                }
                            }
                            else
                            {
                                if (!pieza.activa && piezaMontada.activa) //Verifica si la pieza pasa a deshecharse.
                                {
                                    RegistrarMovimientoHistorial(piezaMontada, TipoMovimientoPiezaEnum.BajaDesecho, "");

                                    #region Se deshecha la pieza montada.
                                    var registroPiezaMontada = _context.tblB_PiezaBarrenadora.FirstOrDefault(x => x.id == piezaMontada.id);
                                    if (registroPiezaMontada != null)
                                    {
                                        registroPiezaMontada.reparando = false;
                                        registroPiezaMontada.activa = false;
                                        registroPiezaMontada.montada = false;
                                        _context.SaveChanges();

                                        RegistrarMovimientoHistorial(registroPiezaMontada, TipoMovimientoPiezaEnum.BajaDesecho);
                                    }
                                    #endregion
                                }

                                if (!pieza.montada && piezaMontada.montada) //Verifica si la pieza pasa a desmontarse.
                                {
                                    RegistrarMovimientoHistorial(piezaMontada, TipoMovimientoPiezaEnum.Baja, "");

                                    #region Se desmonta la pieza montada.
                                    var registroPiezaMontada = _context.tblB_PiezaBarrenadora.FirstOrDefault(x => x.id == piezaMontada.id);
                                    if (registroPiezaMontada != null)
                                    {
                                        registroPiezaMontada.reparando = false;
                                        registroPiezaMontada.activa = true;
                                        registroPiezaMontada.montada = false;
                                        _context.SaveChanges();

                                        RegistrarMovimientoHistorial(registroPiezaMontada, TipoMovimientoPiezaEnum.Baja);
                                    }
                                    #endregion
                                }

                                piezaMontada.culataID = pieza.culataID;
                                piezaMontada.cilindroID = pieza.cilindroID;
                            }

                            piezaMontada.reparando = pieza.reparando;
                            piezaMontada.activa = pieza.activa;
                            piezaMontada.montada = pieza.montada;

                            //Se agrega a la lista nueva si era cambio de mismo tipo de pieza pero diferente número de serie.
                            if (piezaMontada.noSerie != pieza.noSerie)
                            {
                                listaPiezasNuevas.Add(pieza);
                            }
                        }
                        else
                        {
                            listaPiezasNuevas.Add(pieza);
                        }
                    }

                    #region Se agregan las piezas nuevas a la barrenadora.
                    // Se eliminan los que no tengan número de insumo.
                    listaPiezasNuevas.RemoveAll(x => x.insumo == 0);

                    if (listaPiezasNuevas.Count > 0)
                    {
                        // Se le asigna el número de serie a cada pieza
                        listaPiezasNuevas.ForEach(nuevaPieza =>
                        {
                            // Se valida que no exista otro martillo con el mismo número de serie.
                            /*  var martilloPorAgregar = listaPiezasNuevas.FirstOrDefault(x => x.tipoPieza == TipoPiezaEnum.Martillo);
                              if (martilloPorAgregar != null)
                              {
                                  if (_context.tblB_PiezaBarrenadora.Any(y => y.tipoPieza == TipoPiezaEnum.Martillo && y.noSerie == martilloPorAgregar.noSerie && y.areaCuenta == nuevaPieza.areaCuenta))
                                  {
                                      throw new Exception("Ya existe una pieza de tipo martillo con ese número de serie.");
                                  }
                              }*/

                            // Se busca el tipo de broca del insumo
                            var pieza = _context.tblB_CatalogoPieza.FirstOrDefault(x => x.insumo == nuevaPieza.insumo && x.activo && nuevaPieza.areaCuenta == x.areaCuenta);
                            nuevaPieza.tipoBroca = pieza != null ? pieza.tipoBroca : TipoBrocaEnum.NO_APLICA;

                            nuevaPieza.noSerie = ObtenerNumeroSeriePieza(nuevaPieza);
                            nuevaPieza.reparando = false;
                            nuevaPieza.activa = true;
                            nuevaPieza.montada = true;

                        });

                        _context.tblB_PiezaBarrenadora.AddRange(listaPiezasNuevas);
                        _context.SaveChanges();

                        var martilloNuevo = listaPiezasNuevas.FirstOrDefault(x => x.tipoPieza == TipoPiezaEnum.Martillo);

                        if (martilloNuevo != null)
                        {
                            martilloNuevo.culataID = listaPiezasNuevas.FirstOrDefault(x => x.tipoPieza == TipoPiezaEnum.Culata).id;
                            martilloNuevo.cilindroID = listaPiezasNuevas.FirstOrDefault(x => x.tipoPieza == TipoPiezaEnum.Cilindro).id;
                        }

                        // Se registra la alta de las nuevas piezas.
                        listaPiezasNuevas.ForEach(pieza =>
                        {
                            RegistrarMovimientoHistorial(pieza, TipoMovimientoPiezaEnum.AltaNueva);
                            IncremetarContadorPiezas(pieza.insumo);
                        });

                    }

                    _context.SaveChanges();
                    #endregion
                    ActualizarEstatusPiezasBarrenadora(barrenadoraID, pzasCompletas);

                    _context.SaveChanges();
                    resultado.Add(SUCCESS, true);
                    dbTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, nombreControlador, "GuardarPiezasBarrenadora", e, AccionEnum.ACTUALIZAR, 0, listaPiezas);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error al guardar las piezas de la barrenadora.");
                }
                return resultado;
            }
        }

        public Dictionary<string, object> SaveOrUpdatePieza(List<tblB_PiezaBarrenadora> obj)
        {

            try
            {

            }
            catch (Exception e)
            {

            }
            return null;
        }


        private void IncremetarContadorPiezas(int insumo)
        {
            var catalogoPieza = _context.tblB_CatalogoPieza.FirstOrDefault(x => x.insumo == insumo);
            catalogoPieza.incremento = catalogoPieza.incremento + 1;
            _context.SaveChanges();
        }
        private void ActualizarEstatusPiezasBarrenadora(int barrenadoraID, bool pzasCompletas)
        {
            var barrenadora = _context.tblB_Barrenadora.FirstOrDefault(x => x.id == barrenadoraID);
            var piezasBarrenadora = _context.tblB_PiezaBarrenadora.Where(x => x.barrenadoraID == barrenadoraID && x.montada).ToList();

            barrenadora.piezasAsignadas = pzasCompletas; // (piezasBarrenadora.Count == 5 || piezasBarrenadora.Count == 6);
            _context.SaveChanges();
        }

        private string ObtenerNumeroSeriePieza(tblB_PiezaBarrenadora pieza)
        {
            var noSerie = _context.tblB_PiezaBarrenadora.Where(x => x.tipoPieza == pieza.tipoPieza && pieza.areaCuenta == x.areaCuenta).Count();

            switch (pieza.tipoPieza)
            {
                case TipoPiezaEnum.Broca:
                    return String.Format("BR{0:00000}", noSerie);
                case TipoPiezaEnum.Martillo:
                    return pieza.noSerie;
                case TipoPiezaEnum.Barra:
                    return String.Format("BA{0:00000}", noSerie);
                case TipoPiezaEnum.Culata:
                    return String.Format("CU{0:00000}", noSerie);
                /* case TipoPiezaEnum.Portabit:
                     return String.Format("PB{0:00000}", noSerie);*/
                case TipoPiezaEnum.Cilindro:
                    return String.Format("CI{0:00000}", noSerie);
                case TipoPiezaEnum.Zanco:
                    return String.Format("ZA{0:00000}", noSerie);
                default:
                    throw new NotImplementedException();
            }
        }

        private void RegistrarMovimientoHistorial(tblB_PiezaBarrenadora pieza, TipoMovimientoPiezaEnum tipoMovimiento, string comentario = null)
        {
            comentario = (comentario == null || comentario.Trim().Length == 0) ? tipoMovimiento.GetDescription() : comentario.Trim().ToUpper();

            _context.tblB_HistorialPieza.Add(new tblB_HistorialPieza
            {
                piezaID = pieza.id,
                horasAcumuladas = pieza.horasTrabajadas + pieza.horasAcumuladas,
                barrenadoraID = pieza.barrenadoraID.HasValue ? pieza.barrenadoraID.Value : 0,
                tipoMovimiento = tipoMovimiento,
                fecha = DateTime.Now,
                comentario = comentario,
                usuarioID = vSesiones.sesionUsuarioDTO.id,
                precio = pieza.precio
            });
        }

        public object ObtenerBarrenadorasAutocomplete(string term)
        {
            var barrenadoras = _context.tblM_CatMaquina.Where(x => x.estatus == 1 && x.noEconomico.Contains(term)).Select(x => new
            {
                x.id,
                value = x.noEconomico
            }).ToList();

            return barrenadoras;
        }

        public Dictionary<string, object> GuardarNuevaBarrenadora(int maquinaID)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Verifica si no existe una barrenadora ya con esa maquinaID
                    if (_context.tblB_Barrenadora.Any(x => x.activa && x.maquinaID == maquinaID))
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "Ya existe una barrenadora para el número económico indicado.");
                        return resultado;
                    }

                    _context.tblB_Barrenadora.Add(new tblB_Barrenadora
                    {
                        activa = true,
                        maquinaID = maquinaID,
                        fechaCreacion = DateTime.Now,
                        usuarioCreadorID = vSesiones.sesionUsuarioDTO.id
                    });
                    _context.SaveChanges();

                    resultado.Add(SUCCESS, true);
                    dbTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, nombreControlador, "GuardarNuevaBarrenadora", e, AccionEnum.AGREGAR, 0, maquinaID);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error al guardar las piezas de la barrenadora.");
                }

            }
            return resultado;
        }
        #endregion

        #region Captura Diaria

        public Dictionary<string, object> ObtenerBarrenadorasCaptura(string areaCuenta, int turno, DateTime fecha)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                var listaEquiposDisponibles = _context.tblB_Barrenadora
                    .Where(x =>
                                x.maquina.centro_costos == areaCuenta &&
                                x.activa &&
                                x.operadoresAsignados &&
                                x.piezasAsignadas).ToList();

                if (listaEquiposDisponibles.Count == 0)
                {
                    resultado.Add(SUCCESS, true);
                    return resultado;
                }

                var listaEmpleados = _context.tblB_ManoObra
                    .Where(emp => emp.turno == turno && emp.activo)
                    .ToList()
                    .Where(emp => listaEquiposDisponibles.Any(eq => emp.barrenadoraID == eq.id))
                    .ToList();

                var listaBarrenadoras = listaEquiposDisponibles.Select(barrendora => new listaBarrenadorasCapturaDTO
                {
                    id = barrendora.id,
                    noEconomico = barrendora.maquina.noEconomico,
                    noSerie = barrendora.maquina.noSerie,
                    horometro = ObtenerHorometroBarrenadora(barrendora.maquina.noEconomico, fecha),
                    operador = listaEmpleados.Where(x => x.barrenadoraID == barrendora.id && x.tipoOperador == TipoOperadorEnum.Operador)
                    .Select(x => new empleadoCapturaDTO
                    {
                        claveEmpleado = x.claveEmpleado,
                        tipoOperador = x.tipoOperador,
                        turno = x.turno,
                        nombre = ObtenerNombreEmpleadoPorClave(x.claveEmpleado)
                    }).FirstOrDefault(),
                    ayudante = listaEmpleados.Where(x => x.barrenadoraID == barrendora.id && x.tipoOperador == TipoOperadorEnum.Ayudante)
                    .Select(x => new empleadoCapturaDTO
                    {
                        claveEmpleado = x.claveEmpleado,
                        tipoOperador = x.tipoOperador,
                        turno = x.turno,
                        nombre = ObtenerNombreEmpleadoPorClave(x.claveEmpleado)
                    }).FirstOrDefault()
                }).ToList();
                /*HttpSessionState session;
                    Session["rptListaCapturaDiaria"] = listaBarrenadoras;*/

                HttpContext.Current.Session["rptListaCapturaDiaria"] = listaBarrenadoras;

                resultado.Add(ITEMS, listaBarrenadoras);
                resultado.Add(SUCCESS, listaBarrenadoras.Count > 0);
            }
            catch (Exception e)
            {
                LogError(0, 0, nombreControlador, "ObtenerBarrenadorasCaptura", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(MESSAGE, "Ocurrió un error al buscar las barrenadoras para su captura diaria.");
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        private bool noTieneRegistroCapturaBarrenacion(int barrenadoraID, DateTime fecha, int turno)
        {
            var registro = _context.tblB_CapturaDiaria.FirstOrDefault(x => x.fechaCaptura == fecha && x.barrenadoraID == barrenadoraID && x.turno == turno);
            return registro == null;
        }

        private decimal ObtenerHorometroBarrenadora(string noEconomico, DateTime fecha)
        {
            var listaCapturas = new List<Tuple<DateTime, decimal>>();

            // Busca en la tabla de horómetros el último registro igual o anterior a la fecha indicada.
            var ultimoRegistroHorometroFecha = _context.tblM_CapHorometro
                .Where(x => x.Economico == noEconomico && x.Fecha <= fecha)
                .OrderByDescending(x => x.Fecha)
                .FirstOrDefault();

            return ultimoRegistroHorometroFecha != null ? ultimoRegistroHorometroFecha.Horometro : 0;
        }

        public Dictionary<string, object> GuardarCapturaDiaria(List<tblB_CapturaDiaria> listaCaptura, DateTime fecha)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
                try
                {
                    int usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;
                    if (listaCaptura == null || listaCaptura.Count == 0)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "La lista de captura viene vacía.");
                        return resultado;
                    }


                    if (listaCaptura.Any(x =>
                            x.tipoCaptura == TipoCapturaEnum.Normal &&
                            (x.horasTrabajadas <= 0 ||
                                //(x.detalles == null || x.detalles.Count == 0) ||
                            x.claveOperador <= 0)))
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "Los datos por capturar son inválidos.");
                        return resultado;
                    }
                    bool horasCorrectas = true;

                    var listaCapturaHorometro = new List<tblM_CapHorometro>();

                    foreach (var captura in listaCaptura)
                    {
                        var barrenadora = _context.tblB_Barrenadora.FirstOrDefault(x => x.id == captura.barrenadoraID);

                        // Se valida que las horas capturadas no excedan de 24 horas
                        decimal horasCapturadas = ObtenerHorasCapturadas(fecha, barrenadora.maquina.noEconomico);

                        bool horasSuperanLimiteDiario = (horasCapturadas + captura.horasTrabajadas) > 24;
                        if (horasSuperanLimiteDiario)
                        {
                            horasCorrectas = false;
                            break;
                        }
                        else
                        {
                            captura.fechaCaptura = fecha;
                            captura.fechaCreacion = DateTime.Now;
                            captura.usuarioCreadorID = usuarioCreadorID;
                            captura.banco = "0";


                            // Se agregan las horas capturadas a cada pieza.
                            var listaPiezas = _context.tblB_PiezaBarrenadora.Where(y => y.barrenadoraID == captura.barrenadoraID && y.activa).ToList();
                            listaPiezas.ForEach(x =>
                            {
                                x.horasTrabajadas += captura.horasTrabajadas;
                                switch (x.tipoPieza)
                                {
                                    case TipoPiezaEnum.Broca:
                                        captura.brocaID = x.id;
                                        captura.brocaSerie = x.noSerie;
                                        break;
                                    case TipoPiezaEnum.Martillo:
                                        captura.martilloID = x.id;
                                        captura.martilloSerie = x.noSerie;
                                        break;
                                    case TipoPiezaEnum.Barra:
                                        captura.barraID = x.id;
                                        captura.barraSerie = x.noSerie;
                                        break;
                                    case TipoPiezaEnum.Culata:
                                        captura.culataID = x.id;
                                        captura.culataSerie = x.noSerie;
                                        break;
                                    case TipoPiezaEnum.Cilindro:
                                        captura.cilindroID = x.id;
                                        captura.cilindroSerie = x.noSerie;
                                        break;
                                    default:
                                        throw new Exception("Pieza no definida.");
                                }
                            });

                        }
                    }

                    if (horasCorrectas)
                    {
                        _context.tblB_CapturaDiaria.AddRange(listaCaptura);
                        _context.SaveChanges();
                        resultado.Add(SUCCESS, true);
                        dbTransaction.Commit();
                    }
                    else
                    {
                        dbTransaction.Rollback();
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "Las horas trabajadas exceden las 24 horas diarias.");
                    }
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, nombreControlador, "GuardarCapturaDiaria", e, AccionEnum.AGREGAR, 0, 0);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error al guardar la captura diaria.");
                }
            return resultado;
        }

        private decimal ObtenerHorasCapturadas(DateTime fecha, string noEconomico)
        {
            var horasCapturadas = _context.tblB_CapturaDiaria
                .Where(x => x.fechaCaptura == fecha && x.barrenadora.maquina.noEconomico == noEconomico)
                .ToList();

            return horasCapturadas.Count > 0 ? horasCapturadas.Sum(x => x.horasTrabajadas) : 0;
        }

        private decimal ObtenerHorasPromedioHorometro(string noEconomico)
        {
            var ultimoRegistro = _context.tblM_CapHorometro
                .Where(x => x.Economico == noEconomico)
                .OrderByDescending(x => x.Fecha)
                .ThenBy(x => x.Horometro)
                .FirstOrDefault();

            if (ultimoRegistro.Ritmo)
            {
                return _context.tblM_CapHorometro
                     .Where(x => x.Economico == noEconomico && x.HorometroAcumulado <= ultimoRegistro.HorometroAcumulado)
                     .OrderByDescending(x => x.Fecha).Take(20).Average(x => x.HorasTrabajo);
            }
            else
            {
                return _context.tblM_CapRitmoHorometro.FirstOrDefault(x => x.economico == noEconomico).horasDiarias;
            }
        }

        private bool noTieneRegistroHorometro(string noEconomico, DateTime fecha, int turno)
        {
            var registro = _context.tblM_CapHorometro.FirstOrDefault(x => x.Fecha == fecha && x.Economico == noEconomico && x.turno == turno);
            return registro == null;
        }

        private tblM_CapHorometro ObtenerUltimoRegistroHorometro(string noEconomico)
        {
            return _context.tblM_CapHorometro.Where(x => x.Economico == noEconomico).OrderByDescending(x => x.Fecha).FirstOrDefault();
        }

        #endregion

        #region Reparacion
        public Dictionary<string, object> ObtenerPiezasPorReparar()
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                var listaPiezas = _context.tblB_PiezaBarrenadora.Where(x => x.reparando && x.tipoPieza == TipoPiezaEnum.Martillo).ToList();
                var lstBarrenadora = _context.tblB_Barrenadora;
                var items = new List<object>();

                listaPiezas.ForEach(pieza =>
                {

                    var barrenadora = lstBarrenadora.FirstOrDefault(x => x.id == pieza.barrenadoraID);

                    var piezaDTO = new
                    {
                        id = pieza.id,
                        economico = barrenadora.maquina.noEconomico,
                        cc = barrenadora.maquina.centro_costos,
                        metrosLineales = ObtenerMetrosLinealesMartillo(pieza.id),
                        descripcion = _context.tblB_CatalogoPieza.Where(x => pieza.insumo == x.insumo).Select(x => x.descripcion),
                        descTipoPieza = EnumExtensions.GetDescription((TipoPiezaEnum)pieza.tipoPieza),
                        noSerie = pieza.noSerie,
                    };
                    items.Add(piezaDTO);
                });

                resultado.Add(ITEMS, items);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, nombreControlador, "ObtenerPiezasPorReparar", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(MESSAGE, "Ocurrió un error al buscar las piezas por reparar.");
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        /// <summary>
        /// Obtiene la cantidad de metros lineales realizados por un martillo.
        /// </summary>
        /// <param name="piezaID"></param>
        /// <returns></returns>
        private string ObtenerMetrosLinealesMartillo(int piezaID)
        {
            // Busca todos los registros de captura con esa pieza.
            var capturasPieza = _context.tblB_CapturaDiaria.Where(captura => captura.martilloID == piezaID).ToList();

            var sumaMetrosLineales = capturasPieza
                //Suma  todos los metros lineales de cada captura
                .Sum(captura =>
                {
                    // Los metros lineales de cada detalle se sacan como : Produndidad barreno * # Barrenos
                    return captura.detalles != null ?
                                    captura.detalles.Sum(detalleCaptura => (detalleCaptura.barrenos * detalleCaptura.profundidad)) :
                                    0;
                }
            ).ToString();

            return sumaMetrosLineales;
        }
        private decimal ObtenerMetrosLinealesMartillo(int piezaID, DateTime fechaInicio, DateTime fechaFin)
        {
            // Busca todos los registros de captura con esa pieza.
            var capturasPieza = _context.tblB_CapturaDiaria.Where(captura => captura.martilloID == piezaID && captura.barrenos > 0 && captura.fechaCaptura >= fechaInicio && captura.fechaCaptura <= fechaFin).ToList();

            var sumaMetrosLineales = capturasPieza
                //Suma  todos los metros lineales de cada captura
                .Sum(captura =>
                {
                    // Los metros lineales de cada detalle se sacan como : Produndidad barreno * # Barrenos
                    return captura.detalles != null ?
                                    captura.detalles.Sum(detalleCaptura => (detalleCaptura.barrenos * detalleCaptura.profundidad)) :
                                    0;
                }
            );

            return sumaMetrosLineales;
        }
        private decimal ObtenerToneladasMartillo(int piezaID, DateTime fechaInicio, DateTime fechaFin)
        {
            // Busca todos los registros de captura con esa pieza.
            var capturasPieza = _context.tblB_CapturaDiaria.Where(captura => captura.martilloID == piezaID && captura.barrenos > 0).Where(f => f.fechaCaptura >= fechaInicio && fechaFin <= f.fechaCaptura).ToList();

            var sumaMetrosLineales = capturasPieza
                //Suma  todos los metros lineales de cada captura
                .Sum(captura =>
                {
                    // Los metros lineales de cada detalle se sacan como : Produndidad barreno * # Barrenos
                    return captura.detalles != null ?
                                    captura.detalles.Sum(detalleCaptura => (detalleCaptura.barrenos * detalleCaptura.profundidad) * detalleCaptura.bordo * detalleCaptura.espaciamiento * detalleCaptura.densidadMaterial) :
                                    0;
                }
            );

            return sumaMetrosLineales;
        }
        public bool ActualizarPiezaEstadoReparacion(TipoMovimientoPiezaEnum tipoMovimiento, int piezaID, string comentario)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var pieza = _context.tblB_PiezaBarrenadora.FirstOrDefault(p => p.id == piezaID);

                    switch (tipoMovimiento)
                    {
                        case TipoMovimientoPiezaEnum.ReparacionMartillo:
                            pieza.reparando = false;
                            pieza.activa = true;
                            pieza.barrenadoraID = null;
                            pieza.cantidadReparaciones++;
                            break;
                        case TipoMovimientoPiezaEnum.BajaDesecho:
                            pieza.reparando = false;
                            pieza.activa = false;
                            break;
                        default:
                            break;
                    }

                    RegistrarMovimientoHistorial(pieza, tipoMovimiento);

                    _context.SaveChanges();
                    dbTransaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, nombreControlador, "ActualizarPiezaEstadoReparacion", e, AccionEnum.ACTUALIZAR, piezaID, null);
                    return false;
                }
            }
        }
        #endregion

        #region Catálogo Piezas
        public Dictionary<string, object> ObtenerInsumosPorPieza(string areaCuenta)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                var listaTipoPiezas = EnumExtensions.ToCombo<TipoPiezaEnum>();

                foreach (var tipoPiezaDTO in listaTipoPiezas)
                {
                    TipoPiezaEnum pieza = (TipoPiezaEnum)tipoPiezaDTO.Value;

                    var listaInsumosPorPieza = _context.tblB_CatalogoPieza
                        .Where(x => x.tipoPieza == pieza && x.activo && x.areaCuenta == areaCuenta)
                        .OrderByDescending(x => x.id)
                        .ToList()
                        .Select(x => new
                        {
                            x.id,
                            x.descripcion,
                            tipoBroca = x.tipoBroca.GetDescription()
                        })
                        .ToList();
                    if (listaInsumosPorPieza.Count > 0)
                        resultado.Add(pieza.GetDescription(), listaInsumosPorPieza);


                }
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, nombreControlador, "ObtenerInsumosPorPieza", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(MESSAGE, "Ocurrió un error al buscar los insumos por pieza.");
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> AgregarInsumoPieza(tblB_CatalogoPieza nuevoInsumoPieza)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (nuevoInsumoPieza == null || nuevoInsumoPieza.insumo == 0)
                    {
                        resultado.Add(MESSAGE, "Información para agregar un nuevo insumo a la pieza es inválida.");
                        resultado.Add(SUCCESS, false);
                        return resultado;
                    }

                    if (esInsumoRepetido(nuevoInsumoPieza))
                    {
                        resultado.Add(MESSAGE, "Ya se ha dado de alta este insumo para la pieza indicada.");
                        resultado.Add(SUCCESS, false);
                        return resultado;
                    }

                    nuevoInsumoPieza.fechaAlta = DateTime.Now;
                    nuevoInsumoPieza.usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;
                    nuevoInsumoPieza.descripcion = nuevoInsumoPieza.descripcion.Trim();

                    _context.tblB_CatalogoPieza.Add(nuevoInsumoPieza);

                    _context.SaveChanges();
                    resultado.Add(SUCCESS, true);
                    resultado.Add("id", nuevoInsumoPieza.id);
                    resultado.Add("tipoBroca", nuevoInsumoPieza.tipoBroca.GetDescription());
                    dbTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, nombreControlador, "AgregarInsumoPieza", e, AccionEnum.CONSULTA, 0, nuevoInsumoPieza);
                    resultado.Add(MESSAGE, "Ocurrió un error al agregar el insumo por pieza.");
                    resultado.Add(SUCCESS, false);
                }
            }

            return resultado;
        }

        private bool esInsumoRepetido(tblB_CatalogoPieza nuevoInsumo)
        {
            return _context.tblB_CatalogoPieza.Any(x => x.activo && x.insumo == nuevoInsumo.insumo && x.tipoPieza == nuevoInsumo.tipoPieza && x.areaCuenta == nuevoInsumo.areaCuenta);
        }

        public Dictionary<string, object> EliminarInsumoPieza(int id)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (id == 0)
                    {
                        resultado.Add(MESSAGE, "Información para eliminar un insumo a la pieza es inválida.");
                        resultado.Add(SUCCESS, false);
                        return resultado;
                    }

                    var piezaPorEliminar = _context.tblB_CatalogoPieza.FirstOrDefault(x => x.id == id);
                    if (piezaPorEliminar != null)
                    {
                        piezaPorEliminar.activo = false;
                    }
                    else
                    {
                        resultado.Add(MESSAGE, "No se encontró la pieza que buscaba eliminar.");
                        resultado.Add(SUCCESS, false);
                        return resultado;
                    }

                    _context.SaveChanges();
                    resultado.Add(SUCCESS, true);
                    dbTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, nombreControlador, "EliminarInsumoPieza", e, AccionEnum.ELIMINAR, id, null);
                    resultado.Add(MESSAGE, "Ocurrió un error al eliminar el insumo de la pieza.");
                    resultado.Add(SUCCESS, false);
                }
            }

            return resultado;
        }
        #endregion

        #region Reporte de captura diaria
        public Dictionary<string, object> CargarCapturasDiarias(List<string> areaCuenta, List<int> barrenadoraID, List<int> turnos, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                List<ReporteCapturaDiariaDTO> capturasDiarias = new List<ReporteCapturaDiariaDTO>();
                var capturas = _context.tblB_CapturaDiaria
                    .Where(x =>
                       barrenadoraID.Contains(x.barrenadoraID) &&
                         areaCuenta.Contains(x.areaCuenta) &&
                        turnos.Contains(x.turno) &&
                        x.fechaCaptura >= fechaInicio &&
                        x.fechaCaptura <= fechaFin
                        ).ToList();


                foreach (var item in capturas)
                {
                    ReporteCapturaDiariaDTO objCapturasDiaria = new ReporteCapturaDiariaDTO();

                    objCapturasDiaria.id = item.id;
                    objCapturasDiaria.fechaCapturaDate = item.fechaCaptura;
                    objCapturasDiaria.fechaCaptura = Convert.ToDateTime(item.fechaCaptura.ToString("dd/MM/yyyy"));
                    objCapturasDiaria.noEconomico = item.barrenadora.maquina.noEconomico != "" ? item.barrenadora.maquina.noEconomico : "N/A";
                    objCapturasDiaria.horasTrabajadas = item.horasTrabajadas != 0 ? item.horasTrabajadas : 0;
                    objCapturasDiaria.turno = item.turno != 0 ? item.turno : 0;
                    objCapturasDiaria.operador = item.claveOperador != 0 ? ObtenerNombreEmpleadoPorClave(item.claveOperador) : "N/A";
                    objCapturasDiaria.ayudante = item.claveAyudante != 0 ? ObtenerNombreEmpleadoPorClave(item.claveAyudante) : "N/A";
                    objCapturasDiaria.tipoCaptura = item.tipoCaptura == TipoCapturaEnum.Normal ? item.tipoCaptura.GetDescription() : String.Format("Especial / {0}", item.tipoCaptura.GetDescription());

                    objCapturasDiaria.metrosLineales = (item.barrenos * (item.profundidad + item.subBarreno)) + (item.rehabilitacion * (item.profundidad + item.subBarreno));
                    objCapturasDiaria.metrosLinealesHora = item.horasTrabajadas != 0 ? (item.barrenos * (item.profundidad + item.subBarreno)) + (item.rehabilitacion * (item.profundidad + item.subBarreno)) / item.horasTrabajadas : 0;
                    objCapturasDiaria.toneladas = (item.barrenos * (item.profundidad + item.subBarreno) * item.densidadMaterial * item.bordo * item.espaciamiento);
                    objCapturasDiaria.toneladasHora = item.horasTrabajadas != 0 ?(item.barrenos * (item.profundidad + item.subBarreno) * item.densidadMaterial * item.bordo * item.espaciamiento) / item.horasTrabajadas:0;
                    //objCapturasDiaria.metrosLineales = (item.barrenos * (item.profundidad + item.subBarreno)) + (item.rehabilitacion * (item.profundidad + item.subBarreno)) ;
                    //objCapturasDiaria.metrosLinealesHora = (item.barrenos * (item.profundidad + item.subBarreno)) + (item.rehabilitacion * (item.profundidad + item.subBarreno));
                    //objCapturasDiaria.toneladas = (item.barrenos * item.profundidad * item.densidadMaterial * item.bordo * item.espaciamiento);
                    //objCapturasDiaria.toneladasHora =  (item.barrenos * item.profundidad * item.densidadMaterial * item.bordo * item.espaciamiento);
                    objCapturasDiaria.barrenos = item.barrenos;
                    objCapturasDiaria.areaCuenta = item.areaCuenta != "" ? item.areaCuenta : "N/A";

                    capturasDiarias.Add(objCapturasDiaria);
                }



                HttpContext.Current.Session["rptBarrenacion_rptCaptura"] = capturasDiarias.Select(r => new ReporteGeneralCapturaDTO
                {
                    fechaCaptura = r.fechaCaptura.ToString("dd/MM/yyyy"),
                    equipo = r.noEconomico,
                    turno = Convert.ToInt32(r.turno),
                    horasTrabajo = r.horasTrabajadas,
                    barrenos = r.barrenos,
                    metrosLineales = r.metrosLineales,
                    metrosLinealesHora = r.metrosLinealesHora,
                    toneladas = r.toneladas,
                    areaCuenta = r.areaCuenta,
                    toneladasHora = r.toneladasHora
                }).ToList();


                //HttpContext.Current.Session["rptBarrenacion_rptCaptura"] = capturas.Select(r => new ReporteGeneralCapturaDTO
                //{
                //    fechaCaptura = r.fechaCaptura,
                //    equipo = r.noEconomico,
                //    turno = Convert.ToInt32(r.turno),
                //    horasTrabajo = r.horasTrabajadas,
                //    barrenos = r.barrenos,
                //    metrosLineales = r.metrosLineales,
                //    metrosLinealesHora = r.metrosLinealesHora,
                //    toneladas = r.toneladas,
                //    areaCuenta = r.areaCuenta,
                //    toneladasHora = r.toneladasHora
                //}).ToList();

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, capturasDiarias);
                //resultado.Add(ITEMS, capturas);
            }
            catch (Exception e)
            {
                LogError(0, 0, nombreControlador, "CargarCapturasDiarias", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(MESSAGE, "Ocurrió un error al buscar las capturas diarias de barrenación.");
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        private decimal metrosLinealesHrs(int barrenos, int rehabilitacion, decimal horastrabajadas, decimal profundidad)
        {
            if (barrenos == 0 && rehabilitacion == 0)
                return 0;
            else
                if (horastrabajadas == 0)
                    return 0;
                else
                    return ((barrenos * profundidad) + (rehabilitacion * profundidad)) / horastrabajadas;
        }

        public ReporteCapturaDTO ObtenerCapturaDiariaPorId(int capturaID)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                var captura = _context.tblB_CapturaDiaria
                    .Where(x => x.id == capturaID && x.horasTrabajadas > 0)
                    .ToList()
                    .Select(x => new ReporteCapturaDTO
                    {
                        id = x.id,
                        noEconomico = x.barrenadora.maquina.noEconomico,
                        areaCuenta = x.barrenadora.maquina.centro_costos,
                        fechaCaptura = x.fechaCaptura.ToString("dd/MM/yyyy"),
                        horasTrabajadas = x.horasTrabajadas,
                        turno = x.turno.ToString(),
                        operador = x.claveOperador != 0 ? ObtenerNombreEmpleadoPorClave(x.claveOperador) : "N/A",
                        ayudante = x.claveAyudante != 0 ? ObtenerNombreEmpleadoPorClave(x.claveAyudante) : "N/A",
                        tipoCaptura = x.tipoCaptura == TipoCapturaEnum.Normal ?
                        x.tipoCaptura.GetDescription() : String.Format("Especial / {0}", x.tipoCaptura.GetDescription()),
                        detalles = detalle(x.bordo, x.espaciamiento, x.barrenos, x.profundidad, x.banco, x.densidadMaterial, x.tipoBarreno, x.subBarreno), // x.detalles,
                        metrosLineales = (x.barrenos * x.profundidad) + (x.rehabilitacion * x.profundidad), /// x.detalles != null ? x.detalles.Sum(y => y.profundidad * y.barrenos) : 0,
                        metrosLinealesHora = /*metrosLinealesHrs(x.barrenos, x.rehabilitacion, x.horasTrabajadas),*/ ((x.barrenos * x.profundidad) + (x.rehabilitacion * x.profundidad)) / x.horasTrabajadas,  // x.horasTrabajadas > 0 ? x.detalles != null ? x.detalles.Sum(y => y.profundidad * y.barrenos) / x.horasTrabajadas : 0 : 0,
                        //metrosLineales = x.profundidad * x.barrenos,// x.detalles != null ? x.detalles.Sum(y => y.profundidad * y.barrenos) : 0,
                        //metrosLinealesHora = (x.profundidad * x.barrenos) / x.horasTrabajadas //x.horasTrabajadas > 0 ? x.detalles != null ? x.detalles.Sum(y => y.profundidad * y.barrenos) / x.horasTrabajadas : 0 : 0
                    }).FirstOrDefault();

                var descripcionAreaCuenta = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == captura.areaCuenta);
                captura.areaCuenta = descripcionAreaCuenta != null ? String.Format("{0} - {1}", descripcionAreaCuenta.areaCuenta, descripcionAreaCuenta.descripcion.Trim()) : "Indefinido";

                return captura;
            }
            catch (Exception e)
            {
                LogError(0, 0, nombreControlador, "ObtenerCapturaDiariaPorId", e, AccionEnum.CONSULTA, capturaID, null);
                return new ReporteCapturaDTO();
            }
        }

        public Dictionary<string, object> ObtenerCapturaDiaria(int barrenadoraID, DateTime fechaActual, int turno)
        {

            try
            {
                //var capturaDiaria = _context.tblB_CapturaDiaria.Where(r => r.barrenadoraID == barrenadoraID).ToList().Where(r => r.fechaCaptura.ToShortDateString() == fechaActual.ToShortDateString()));

                var capturaDiaria = _context.tblB_CapturaDiaria
               .Where(x => x.barrenadoraID == barrenadoraID).ToList().Where(r => r.fechaCaptura.Date == fechaActual.Date && turno == r.turno).Select(r => new CapturaDiariaDTO
               {
                   horasTrabajadas = r.horasTrabajadas,
                   bordo = r.bordo,
                   espaciamiento = r.espaciamiento,
                   barrenos = r.barrenos,
                   profundidad = r.profundidad,
                   banco = r.banco,
                   densidadMaterial = r.densidadMaterial,
                   tipoBarreno = r.tipoBarreno,
                   subbarreno = r.subBarreno,
                   id = r.id,
                   rehabilitacion = r.rehabilitacion,
                   claveOperador = r.claveOperador,
                   areaCuenta = r.areaCuenta,
                   barraID = r.barraID,
                   barraSegundaID = r.barraSegundaID,
                   barraSegundaSerie = r.barraSegundaSerie,
                   barraSerie = r.barraSerie,
                   barrenadoraID = r.barrenadoraID,
                   brocaID = r.brocaID,
                   brocaSerie = r.brocaSerie,
                   cilindroID = r.cilindroID,
                   cilindroSerie = r.cilindroSerie,
                   claveAyudante = r.claveAyudante,
                   culataID = r.culataID,
                   culataSerie = r.culataSerie,
                   fechaCaptura = r.fechaCaptura.ToShortDateString(),
                   fechaCreacion = r.fechaCreacion.ToShortDateString(),
                   fsrOperador = r.fsrOperador,
                   fsrAyudante = r.fsrAyudante,
                   horometroFinal = r.horometroFinal,
                   martilloID = r.martilloID,
                   martilloSerie = r.martilloSerie,
                   portabitID = r.portabitID,
                   portabitSerie = r.portabitSerie,
                   precioAyudante = r.precioAyudante,
                   precioOperador = r.precioOperador,
                   tipoCaptura = (int)r.tipoCaptura,
                   totalAyudante = r.totalAyudante,
                   totalOperador = r.totalOperador,
                   turno = r.turno,
                   edit = false
               }).ToList();

                var ultimaCapturaDiaria = _context.tblB_CapturaDiaria
                .Where(x => x.barrenadoraID == barrenadoraID).OrderByDescending(r => r.id).FirstOrDefault();
                bool disabled = true;
                var existeCapturaActual = capturaDiaria.FirstOrDefault();

                if (ultimaCapturaDiaria != null)
                {
                    if (existeCapturaActual != null)
                    {
                        if (ultimaCapturaDiaria.fechaCaptura.ToShortDateString() == existeCapturaActual.fechaCaptura)
                            disabled = false;
                    }
                }
                if (capturaDiaria.Count > 0)
                {
                    int maxVal = capturaDiaria.Max(r => r.id);

                    capturaDiaria.ForEach(r =>
                    {
                        if (r.id == maxVal)
                            r.edit = true;
                    });
                }
                decimal ultimaDensidad = 0;
                if (ultimaCapturaDiaria != null) ultimaDensidad = ultimaCapturaDiaria.densidadMaterial;

                resultado.Add("disabled", disabled);
                resultado.Add("detalles", capturaDiaria);
                resultado.Add("ultimaDensidad", ultimaDensidad);
                resultado.Add(SUCCESS, true);
                return resultado;
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
                return resultado;
            }
        }

        public Dictionary<string, object> EliminarCaptura(int capturaID)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    tblB_CapturaDiaria capturaDiaria = _context.tblB_CapturaDiaria.FirstOrDefault(r => r.id == capturaID);
                    _context.tblB_CapturaDiaria.Remove(capturaDiaria);
                    _context.SaveChanges();
                    dbTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, nombreControlador, "CapturaDiaria", e, AccionEnum.CONSULTA, 0, null);
                    resultado.Add(MESSAGE, "Ocurrió un error al tratar de eliminar la captura diaria.");
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }

        private List<tblB_DetalleCaptura> detalle(decimal bordo, decimal espaciamineto, int barrenos, decimal profundidad, string banco, decimal densidadMaterial, int tipoBarreno, decimal subbarreno)
        {
            List<tblB_DetalleCaptura> resultdo = new List<tblB_DetalleCaptura>();


            resultdo.Add(new tblB_DetalleCaptura
            {
                id = 0,
                bordo = bordo,
                espaciamiento = espaciamineto,
                barrenos = barrenos,
                profundidad = profundidad,
                banco = banco,
                densidadMaterial = densidadMaterial,
                tipoBarreno = tipoBarreno == 1 ? TipoBarrenoEnum.Normal : TipoBarrenoEnum.Rehabilitacion,
                subbarreno = subbarreno,
                capturaID = 0
            });
            return resultdo;
        }

        public Dictionary<string, object> ObtenerBarrenadorasPorCC(string cc)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                List<ComboDTO> listaBarrenadoras = _context.tblB_Barrenadora
                    //   .Where(x => x.maquina.centro_costos == cc && x.activa)
                    .OrderBy(x => x.maquina.noEconomico)
                    .Select(barrenadora => new ComboDTO
                    {
                        Text = barrenadora.maquina.descripcion + " - " + barrenadora.maquina.noEconomico,
                        Value = barrenadora.id.ToString()
                    }).ToList();

                resultado.Add(ITEMS, listaBarrenadoras);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, nombreControlador, "ObtenerBarrenadorasPorCC", e, AccionEnum.CONSULTA, 0, null);
            }
            return resultado;
        }

        public List<ReporteGeneralCapturaDTO> ObtenerReporteGeneralCapturas(string areaCuenta, int barrenadoraID, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                var descripcionAreaCuenta = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == areaCuenta);
                string areaCuentaTexto = descripcionAreaCuenta != null ?
                    String.Format("{0} - {1}", descripcionAreaCuenta.areaCuenta, descripcionAreaCuenta.descripcion.Trim()) : "Indefinido";

                var capturas = _context.tblB_CapturaDiaria
                    .Where(x =>
                        (barrenadoraID == 0 ? true : x.barrenadoraID == barrenadoraID) &&
                            // x.tipoCaptura == TipoCapturaEnum.Normal &&
                        x.areaCuenta == areaCuenta &&
                        x.fechaCaptura >= fechaInicio &&
                        x.fechaCaptura <= fechaFin &&
                        x.horasTrabajadas > 0
                        )
                    .ToList()
                    .Select(x => new ReporteGeneralCapturaDTO
                    {
                        fechaCaptura = x.fechaCaptura.ToString("dd/MM/yyyy"),
                        areaCuenta = areaCuentaTexto,
                        equipo = x.barrenadora.maquina.noEconomico,
                        horasTrabajo = x.horasTrabajadas,
                        turno = x.turno,
                        barrenos = x.barrenos,//x.detalles.Sum(y => y.barrenos),
                        metrosLineales = x.barrenos * x.profundidad, // x.detalles.Sum(y => y.barrenos * y.profundidad),
                        metrosLinealesHora = (x.barrenos * x.profundidad) / x.horasTrabajadas, // // x.horasTrabajadas > 0 ? x.detalles.Sum(y => y.barrenos * y.profundidad) / x.horasTrabajadas : 0,
                        toneladas = x.barrenos * x.profundidad * x.densidadMaterial * x.bordo * x.espaciamiento  //x.detalles.Sum(y => y.barrenos * y.profundidad * y.densidadMaterial * y.bordo * y.espaciamiento)
                    })
                    .ToList();

                return capturas;
            }
            catch (Exception e)
            {
                LogError(0, 0, nombreControlador, "ObtenerReporteGeneralCapturas", e, AccionEnum.CONSULTA, 0, null);
                return new List<ReporteGeneralCapturaDTO>();
            }
        }
        #endregion

        #region Reporte de rendimiento de pieza
        public Dictionary<string, object> CargarRendimientoPiezas(string areaCuenta, List<int> listatipoPieza, DateTime fechaInicio, DateTime fechaFin)
        {
            //    try
            //  {
            _context.Configuration.AutoDetectChangesEnabled = false;

            var capturas = _context.tblB_CapturaDiaria.Where(x =>
                    x.areaCuenta == areaCuenta &&
                    x.tipoCaptura == TipoCapturaEnum.Normal &&
                    x.fechaCaptura >= fechaInicio &&
                    x.fechaCaptura <= fechaFin)
                .ToList();

            var listaPiezas = new List<RendimientoPiezaDTO>();

            if (capturas.Count > 0)
            {
                // Lista piezas generales 
                var listaPiezasGenerales = new List<Tuple<int, TipoPiezaEnum>>();

                foreach (var tipoPieza in listatipoPieza)
                {
                    var tipoPiezaFiltro = (TipoPiezaEnum)tipoPieza;

                    var listaCapturas = new List<Tuple<int, TipoPiezaEnum>>();

                    switch (tipoPiezaFiltro)
                    {
                        case TipoPiezaEnum.Broca:
                            listaCapturas = capturas.Select(x => Tuple.Create(x.brocaID, TipoPiezaEnum.Broca)).Distinct().ToList();
                            break;
                        case TipoPiezaEnum.Martillo:
                            listaCapturas = capturas.Select(x => Tuple.Create(x.martilloID, TipoPiezaEnum.Martillo)).Distinct().ToList();
                            break;
                        case TipoPiezaEnum.Barra:
                            listaCapturas = capturas.Select(x => Tuple.Create(x.barraID, TipoPiezaEnum.Barra)).Distinct().ToList();
                            break;
                        case TipoPiezaEnum.Culata:
                            listaCapturas = capturas.Select(x => Tuple.Create(x.culataID, TipoPiezaEnum.Culata)).Distinct().ToList();
                            break;
                        /*case TipoPiezaEnum.Portabit:
                            listaCapturas = capturas.Select(x => Tuple.Create(x.portabitID, TipoPiezaEnum.Portabit)).Distinct().ToList();
                            break;*/
                        case TipoPiezaEnum.Cilindro:
                            listaCapturas = capturas.Select(x => Tuple.Create(x.cilindroID, TipoPiezaEnum.Cilindro)).Distinct().ToList();
                            break;
                        case TipoPiezaEnum.Zanco:
                            listaCapturas = capturas.Select(x => Tuple.Create(x.cilindroID, TipoPiezaEnum.Zanco)).Distinct().ToList();
                            break;
                        default:
                            break;
                    }
                    if (listaCapturas.Count > 0)
                        listaPiezasGenerales.AddRange(listaCapturas);
                }

                foreach (var infoPieza in listaPiezasGenerales)
                {
                    var pieza = _context.tblB_PiezaBarrenadora.FirstOrDefault(x => x.id == infoPieza.Item1 && !x.activa);
                    if (pieza != null)
                    {

                        var barrenadora = _context.tblB_Barrenadora.FirstOrDefault(x => x.id == pieza.barrenadoraID);
                        var rendimientoPieza = new RendimientoPiezaDTO();

                        var capturasPieza = capturas.Where(x =>
                        {
                            switch (infoPieza.Item2)
                            {
                                case TipoPiezaEnum.Broca:
                                    return x.brocaID == infoPieza.Item1;
                                case TipoPiezaEnum.Martillo:
                                    return x.martilloID == infoPieza.Item1;
                                case TipoPiezaEnum.Barra:
                                    return x.barraID == infoPieza.Item1;
                                case TipoPiezaEnum.Culata:
                                    return x.culataID == infoPieza.Item1;
                                case TipoPiezaEnum.Cilindro:
                                    return x.cilindroID == infoPieza.Item1;
                                case TipoPiezaEnum.Zanco:
                                    return x.zancoID == infoPieza.Item1;
                                default:
                                    return false;
                            }
                        }).ToList();

                        rendimientoPieza.tipoPieza = pieza.tipoPieza.GetDescription();
                        rendimientoPieza.piezaID = pieza.id;
                        rendimientoPieza.noSerie = pieza.noSerie;
                        rendimientoPieza.barrenadora = barrenadora != null ? String.Format("{0} [{1}]", barrenadora.maquina.descripcion, barrenadora.maquina.noEconomico) : "Sin asignar";
                        rendimientoPieza.horasTrabajadas = pieza.horasAcumuladas;
                        rendimientoPieza.totalBarrenos = capturasPieza.Sum(y => y.barrenos);
                        rendimientoPieza.metrosLineales = capturasPieza.Sum(x => (x.barrenos * (x.profundidad + x.subBarreno)) + (x.rehabilitacion * (x.profundidad + x.subBarreno)));
                        rendimientoPieza.toneladasBarreno = capturasPieza
                            .Where(y => y.tipoBarreno == (int)TipoBarrenoEnum.Normal)
                                .Sum(y =>
                                {
                                    //switch (pieza.tipoBroca)
                                    //{
                                        //case TipoBrocaEnum.NO_APLICA:
                                        //    return 0;
                                        //default:
                                            return (y.barrenos * y.profundidad * y.densidadMaterial * y.bordo * y.espaciamiento);
                                    //}
                                });
                        rendimientoPieza.toneladasBarrenoRealizados = capturasPieza
                            .Where(y => y.tipoBarreno == (int)TipoBarrenoEnum.Normal)
                                .Sum(y =>
                                {
                                    //switch (pieza.tipoBroca)
                                    //{
                                        //case TipoBrocaEnum.NO_APLICA:
                                        //    return 0;
                                        //default:
                                            return y.barrenos * (y.profundidad * y.subBarreno) * y.densidadMaterial * y.bordo * y.espaciamiento;
                                    //}
                                });
                        rendimientoPieza.metrosCubicos = capturasPieza.Sum(x => (x.barrenos * x.profundidad * x.bordo * x.espaciamiento));
                        listaPiezas.Add(rendimientoPieza);
                    }
                }
            }

            resultado.Add(SUCCESS, true);
            resultado.Add(ITEMS, listaPiezas);
            //    }
            //    catch (Exception e)
            //  {
            //      LogError(0, 0, nombreControlador, "CargarRendimientoPiezas", e, AccionEnum.CONSULTA, 0, null);
            //         resultado.Add(MESSAGE, "Ocurrió un error al buscar las piezas y su rendimiento.");
            //           resultado.Add(SUCCESS, false);
            //       }
            return resultado;
        }

        public RendimientoPiezaDTO CargarRendimientoPieza(int piezaID, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                var capturas = _context.tblB_CapturaDiaria.Where(x =>
                        x.fechaCaptura >= fechaInicio &&
                        x.fechaCaptura <= fechaFin)
                    .ToList();

                var listaPiezas = new List<RendimientoPiezaDTO>();

                if (capturas.Count > 0)
                {
                    var pieza = _context.tblB_PiezaBarrenadora.FirstOrDefault(x => x.id == piezaID);

                    var barrenadora = _context.tblB_Barrenadora.FirstOrDefault(x => x.id == pieza.barrenadoraID);

                    var rendimientoPieza = new RendimientoPiezaDTO();

                    var capturasPieza = capturas.Where(x =>
                    {
                        switch (pieza.tipoPieza)
                        {
                            case TipoPiezaEnum.Broca:
                                return x.brocaID == piezaID;
                            case TipoPiezaEnum.Martillo:
                                return x.martilloID == piezaID;
                            case TipoPiezaEnum.Barra:
                                return x.barraID == piezaID;
                            case TipoPiezaEnum.Culata:
                                return x.culataID == piezaID;
                            /*    case TipoPiezaEnum.Portabit:
                                    return x.portabitID == piezaID;*/
                            case TipoPiezaEnum.Cilindro:
                                return x.cilindroID == piezaID;
                            default:
                                return false;
                        }
                    }).ToList();

                    rendimientoPieza.tipoPieza = pieza.tipoPieza.GetDescription();
                    rendimientoPieza.piezaID = pieza.id;
                    rendimientoPieza.noSerie = pieza.noSerie;
                    rendimientoPieza.barrenadora = barrenadora != null ? String.Format("{0} [{1}]", barrenadora.maquina.descripcion, barrenadora.maquina.noEconomico) : "Sin asignar";
                    rendimientoPieza.horasTrabajadas = pieza.horasAcumuladas + pieza.horasTrabajadas;
                    rendimientoPieza.totalBarrenos = capturasPieza.Sum(y => y.barrenos);
                    rendimientoPieza.metrosLineales = capturasPieza.Sum(y => y.profundidad * y.barrenos);
                    rendimientoPieza.toneladasBarreno = capturasPieza
                        .Where(y => y.tipoBarreno == (int)TipoBarrenoEnum.Normal)
                            .Sum(y =>
                            {
                                switch (pieza.tipoBroca)
                                {
                                    case TipoBrocaEnum.NO_APLICA:
                                        return 0;
                                    default:
                                        return (y.profundidad * y.subBarreno) * y.densidadMaterial * y.bordo * y.espaciamiento;
                                }
                            });
                    rendimientoPieza.toneladasBarrenoRealizados = capturasPieza
                        .Where(y => y.tipoBarreno == (int)TipoBarrenoEnum.Normal)
                            .Sum(y =>
                            {
                                switch (pieza.tipoBroca)
                                {
                                    case TipoBrocaEnum.NO_APLICA:
                                        return 0;
                                    default:
                                        return y.barrenos * (y.profundidad * y.subBarreno) * y.densidadMaterial * y.bordo * y.espaciamiento;
                                }
                            });

                    return rendimientoPieza;
                }

                return new RendimientoPiezaDTO();
            }
            catch (Exception e)
            {
                LogError(0, 0, nombreControlador, "CargarRendimientoPieza", e, AccionEnum.CONSULTA, piezaID, null);
                resultado.Add(MESSAGE, "Ocurrió un error al buscar el rendimiento de la pieza indicada.");
                resultado.Add(SUCCESS, false);
                return new RendimientoPiezaDTO();
            }
        }
        #endregion

        #region Barrenacio Costo raguilar 14/01/20
        public Dictionary<string, object> GuardarBarrenacionCosto(tblB_BarrenacionCosto registroInformacion, List<tblB_BarrenacionCostoOtroDetalle> lstOtroDetalle, List<tblB_BarrenacionCostoPiezaDetalle> lstPiezaDetalle)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    registroInformacion.activa = true;
                    int usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;
                    registroInformacion.fechaCreacion = DateTime.Now;
                    _context.tblB_BarrenacionCosto.Add(registroInformacion);
                    _context.SaveChanges();
                    int id = registroInformacion.id;//idRegistro
                    GuardarRegistroInformacionDetalleCostoBarrenacion(id, lstOtroDetalle, lstPiezaDetalle);
                    resultado.Add(SUCCESS, true);
                    dbTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    //LogError(0, 0, nombreControlador, "GuardarOperadoresBarrenadora", e, AccionEnum.ACTUALIZAR, 0, listaOperadores);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error al guardar los operadores de la barrenadora.");
                }
                return resultado;
            }
        }

        private void GuardarRegistroInformacionDetalleCostoBarrenacion(int id, List<tblB_BarrenacionCostoOtroDetalle> lstOtroDetalle, List<tblB_BarrenacionCostoPiezaDetalle> lstPiezaDetalle)
        {
            lstPiezaDetalle.ForEach(x =>
            {
                x.idBarrenacionCosto = id;
                x.usuarioCreadorID = 1;
                x.fechaCreacion = DateTime.Now;
                x.activa = true;
            });
            lstOtroDetalle.ForEach(x =>
            {
                x.idBarrenacionCosto = id;
                x.usuarioCreadorID = 1;
                x.fechaCreacion = DateTime.Now;
                x.activa = true;
            });
            _context.tblB_BarrenacionCostoPiezaDetalle.AddRange(lstPiezaDetalle);
            _context.tblB_BarrenacionCostoOtroDetalle.AddRange(lstOtroDetalle);

            _context.SaveChanges();
        }
        public Dictionary<string, object> GetBarrenacionCosto()
        {
            try
            {
                //agregar el centro de costo del usuario actual en el sisema.
                var lstBarrenacionCosto = _context.tblB_BarrenacionCosto.Where(x => x.activa == true).ToList();
                List<BarrenacionCostoDTO> lstResult = new List<BarrenacionCostoDTO>();
                lstBarrenacionCosto.ForEach(x =>
                {
                    BarrenacionCostoDTO objBarrenacion = new BarrenacionCostoDTO();
                    objBarrenacion.id = x.id;
                    objBarrenacion.manoObra = x.manoObra;
                    objBarrenacion.costoRenta = x.costoRenta;
                    objBarrenacion.diesel = x.diesel;
                    objBarrenacion.totalCosto = x.totalCosto;
                    objBarrenacion.totalPieza = _context.tblB_BarrenacionCostoPiezaDetalle.Where(y => y.idBarrenacionCosto == x.id).Sum(i => i.totalPieza);
                    objBarrenacion.totalOtro = _context.tblB_BarrenacionCostoOtroDetalle.Where(z => z.idBarrenacionCosto == x.id).Sum(i => i.totalOtro);
                    lstResult.Add(objBarrenacion);
                });
                resultado.Add(ITEMS, lstResult);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {

                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        //GetBarrenacionCosto
        #endregion

        #region Catalógo Banco

        public Dictionary<string, object> AgregarBanco(tblB_CatalogoBanco nuevoBanco)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    int usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;

                    if (nuevoBanco.id == 0)
                    {
                        if (_context.tblB_CatalogoBanco.Any(x => x.banco == nuevoBanco.banco))
                        {
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "El banco esta duplicado.");
                            return resultado;
                        }

                        nuevoBanco.usuarioCreadorID = usuarioCreadorID;
                        nuevoBanco.fechaCreacion = DateTime.Now;
                        nuevoBanco.estatus = true;

                        _context.tblB_CatalogoBanco.Add(nuevoBanco);
                        _context.SaveChanges();
                    }
                    else
                    {
                        var actualizacionBanco = _context.tblB_CatalogoBanco.FirstOrDefault(x => x.id == nuevoBanco.id);

                        actualizacionBanco.descripcion = nuevoBanco.descripcion;
                        actualizacionBanco.banco = nuevoBanco.banco;
                        actualizacionBanco.estatus = nuevoBanco.estatus;
                        actualizacionBanco.areaCuenta = nuevoBanco.areaCuenta;
                        _context.SaveChanges();
                    }

                    _context.SaveChanges();
                    resultado.Add(SUCCESS, true);
                    dbTransaction.Commit();

                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, nombreControlador, "GuardarBanco", e, AccionEnum.ACTUALIZAR, 0, nuevoBanco);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error al guardar los operadores de la barrenadora.");
                }
                return resultado;
            }


        }

        public Dictionary<string, object> ObtenerBancos(string areacuenta)
        {

            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                var listaBancos = _context.tblB_CatalogoBanco
                    .Where(x => x.estatus && x.areaCuenta == areacuenta)
                    .ToList();

                resultado.Add("listaBancos", listaBancos);
                resultado.Add(SUCCESS, true);

                return resultado;
            }
            catch (Exception e)
            {
                LogError(0, 0, nombreControlador, "ObtenerBancos", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al buscar las barrenadoras.");
                return resultado;
            }
        }
        #endregion

        public Dictionary<string, object> getInfoInsumo(int barrenadoraID, int insumo, string areaCuenta)
        {
            try
            {
                InsumoBarrenadoraDTO obj = new InsumoBarrenadoraDTO();

                var barrenadora = _context.tblB_Barrenadora.FirstOrDefault(x => x.id == barrenadoraID);
                var nuevoInsumo = getNoSerie(insumo, areaCuenta);

                if (barrenadora != null)
                {
                    obj.insumo = insumo;
                    obj.barrenadoraID = barrenadoraID;

                    var pieza = _context.tblB_PiezaBarrenadora.FirstOrDefault(x => x.barrenadoraID == barrenadoraID && x.insumo == insumo && x.activa);
                    if (pieza != null)
                    {
                        obj.noSerie = pieza.noSerie;
                        if (pieza.precio == 0)
                        {
                            obj.precio = obtenerPrecio(barrenadora, insumo);
                        }
                        else
                        {
                            obj.precio = pieza.precio;
                        }
                    }
                    else
                    {
                        obj.precio = obtenerPrecio(barrenadora, insumo);
                        obj.noSerie = getNoSerie(insumo, areaCuenta);
                    }
                }
                resultado.Add("nuevoInsumo", nuevoInsumo);
                resultado.Add("data", obj);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        private decimal obtenerPrecio(tblB_Barrenadora barrenadora, int insumo)
        {
            decimal precio = 0;
            #region Sacar precio.
            var maquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == barrenadora.maquinaID);
            var cc = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == maquina.centro_costos).cc;

            var ultimaCompraEK = _contextEnkontrol.Where(
                string.Format(@"SELECT TOP 1 
                                                        oc.tipo_cambio, 
                                                        det.* 
                                                    FROM so_orden_compra oc 
                                                        INNER JOIN so_orden_compra_det det ON oc.cc = det.cc AND oc.numero = det.numero 
                                                    WHERE oc.cc = '{0}' AND det.insumo = {1} 
                                                    ORDER BY oc.fecha DESC", cc, insumo));

            if (ultimaCompraEK.Count > 0)
            {
                precio = Convert.ToDecimal(((List<dynamic>)ultimaCompraEK.ToObject<List<dynamic>>())[0].precio, CultureInfo.InvariantCulture);
                decimal tipoCambio = Convert.ToDecimal(((List<dynamic>)ultimaCompraEK.ToObject<List<dynamic>>())[0].tipo_cambio, CultureInfo.InvariantCulture);

                precio = precio * tipoCambio;
            }
            else
            {
                precio = 0;
            }
            #endregion
            return precio;
        }

        private string getNoSerie(int insumo, string areaCuenta)
        {
            var insumoCatalogo = _context.tblB_CatalogoPieza.FirstOrDefault(x => x.insumo == insumo && x.areaCuenta == areaCuenta);

            string noSerie = "";

            if (insumoCatalogo != null)
            {
                int contador = insumoCatalogo.incremento + 1; //_context.tblB_PiezaBarrenadora.FirstOrDefault(x => x.tipoPieza == insumoCatalogo.tipoPieza && x.insumo == insumo).incre + 1;

                switch (insumoCatalogo.tipoPieza)
                {
                    case TipoPiezaEnum.Broca:
                        noSerie = String.Format("BR{0:00000}", contador);
                        break;
                    case TipoPiezaEnum.Martillo:
                        noSerie = String.Format("MA{0:00000}", contador);
                        break;
                    case TipoPiezaEnum.Barra:
                        noSerie = String.Format("BA{0:00000}", contador);
                        break;
                    case TipoPiezaEnum.Culata:
                        noSerie = String.Format("CU{0:00000}", contador);
                        break;
                    /* case TipoPiezaEnum.Portabit:
                         noSerie = String.Format("PB{0:00000}", contador);
                         break;*/
                    case TipoPiezaEnum.Cilindro:
                        noSerie = String.Format("CI{0:00000}", contador);
                        break;
                    case TipoPiezaEnum.Zanco:
                        noSerie = String.Format("ZA{0:00000}", contador);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            return noSerie;
        }

        public Dictionary<string, object> getPiezaNueva(int insumo, string areaCuenta)
        {

            try
            {
                resultado.Add("noSerie", getNoSerie(insumo, areaCuenta));
                resultado.Add("precioInsumo", ObtenerUltimoPrecioInsumo(insumo));
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
            }
            return resultado;

        }

        public Dictionary<string, object> setInfoCapturaDiaria(List<tblB_CapturaDiaria> listaCapturaDiaria, List<tblB_PiezaBarrenadora> listaPiezas)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    int usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;

                    if (listaPiezas == null || listaPiezas.Count == 0)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "La lista de piezas viene vacía.");
                        return resultado;
                    }

                    int barrenadoraID = (int)listaCapturaDiaria.First().barrenadoraID;

                    #region Agregar valor de barrenadoraID a todas las piezas.
                    foreach (var pieza in listaPiezas)
                    {
                        pieza.barrenadoraID = barrenadoraID;
                    }
                    #endregion

                    var listaPiezasExistentes = listaPiezas.Where(x => x.id > 0).ToList();
                    var listaPiezasNuevas = listaPiezas.Where(x => x.id == 0).ToList();
                    var piezasMontadas = _context.tblB_PiezaBarrenadora.Where(x => !x.reparando && x.activa && x.montada && x.barrenadoraID == barrenadoraID).ToList();

                    if (listaPiezasExistentes.Count > 0)
                    {
                        foreach (var pieza in listaPiezasExistentes)
                        {
                            var piezaMontada = piezasMontadas.FirstOrDefault(x => x.tipoPieza == pieza.tipoPieza);

                            if (piezaMontada != null)
                            {
                                if (piezaMontada.tipoPieza == TipoPiezaEnum.Martillo)
                                {
                                    if (pieza.reparando && !piezaMontada.reparando) //Verifica si el martillo montado pasa a repararse.
                                    {
                                        // Se resetea el contador de horas trabajadas y se pasa a horas acumuladas
                                        piezaMontada.horasAcumuladas += piezaMontada.horasTrabajadas;
                                        piezaMontada.horasTrabajadas = 0;

                                        RegistrarMovimientoHistorial(piezaMontada, TipoMovimientoPiezaEnum.BajaReparacion);

                                        #region Se deshechan la culata y el cilindro asignados cuando el martillo se repara.
                                        pieza.culataID = 0;
                                        pieza.cilindroID = 0;

                                        var culata = _context.tblB_PiezaBarrenadora.FirstOrDefault(x => x.id == piezaMontada.culataID);
                                        var cilindro = _context.tblB_PiezaBarrenadora.FirstOrDefault(x => x.id == piezaMontada.cilindroID);

                                        culata.reparando = false;
                                        culata.activa = false;
                                        culata.montada = false;

                                        cilindro.reparando = false;
                                        cilindro.activa = false;
                                        cilindro.montada = false;

                                        RegistrarMovimientoHistorial(culata, TipoMovimientoPiezaEnum.BajaDesecho);
                                        RegistrarMovimientoHistorial(cilindro, TipoMovimientoPiezaEnum.BajaDesecho);

                                        piezaMontada.culataID = 0;
                                        piezaMontada.cilindroID = 0;
                                        #endregion
                                    }

                                    if (!pieza.activa && piezaMontada.activa) //Verifica si el martillo montado pasa a deshecharse.
                                    {
                                        RegistrarMovimientoHistorial(piezaMontada, TipoMovimientoPiezaEnum.BajaDesecho, "");

                                        #region Se deshechan la culata y el cilindro asignados cuando el martillo se deshecha.
                                        pieza.culataID = 0;
                                        pieza.cilindroID = 0;

                                        var culata = _context.tblB_PiezaBarrenadora.FirstOrDefault(x => x.id == piezaMontada.culataID);
                                        var cilindro = _context.tblB_PiezaBarrenadora.FirstOrDefault(x => x.id == piezaMontada.cilindroID);

                                        if (culata != null)
                                        {
                                            culata.reparando = false;
                                            culata.activa = false;
                                            culata.montada = false;
                                        }
                                        if (cilindro != null)
                                        {
                                            cilindro.reparando = false;
                                            cilindro.activa = false;
                                            cilindro.montada = false;
                                        }



                                        RegistrarMovimientoHistorial(culata, TipoMovimientoPiezaEnum.BajaDesecho);
                                        RegistrarMovimientoHistorial(cilindro, TipoMovimientoPiezaEnum.BajaDesecho);

                                        piezaMontada.culataID = 0;
                                        piezaMontada.cilindroID = 0;
                                        #endregion
                                    }

                                    if (!pieza.montada && piezaMontada.montada) //Verifica si el martillo montado pasa a desmontarse.
                                    {
                                        RegistrarMovimientoHistorial(piezaMontada, TipoMovimientoPiezaEnum.Baja, "");

                                        #region Se desmontan la culata y el cilindro asignados cuando el martillo se desmonta.
                                        var culata = _context.tblB_PiezaBarrenadora.FirstOrDefault(x => x.id == piezaMontada.culataID);
                                        var cilindro = _context.tblB_PiezaBarrenadora.FirstOrDefault(x => x.id == piezaMontada.cilindroID);

                                        culata.reparando = false;
                                        culata.activa = true;
                                        culata.montada = false;

                                        cilindro.reparando = false;
                                        cilindro.activa = true;
                                        cilindro.montada = false;

                                        RegistrarMovimientoHistorial(culata, TipoMovimientoPiezaEnum.Baja);
                                        RegistrarMovimientoHistorial(cilindro, TipoMovimientoPiezaEnum.Baja);
                                        #endregion
                                    }
                                }
                                else
                                {
                                    if (!pieza.activa && piezaMontada.activa) //Verifica si la pieza pasa a deshecharse.
                                    {
                                        RegistrarMovimientoHistorial(piezaMontada, TipoMovimientoPiezaEnum.BajaDesecho, "");
                                    }

                                    if (!pieza.montada && piezaMontada.montada) //Verifica si la pieza pasa a desmontarse.
                                    {
                                        RegistrarMovimientoHistorial(piezaMontada, TipoMovimientoPiezaEnum.Baja, "");
                                    }

                                    piezaMontada.culataID = pieza.culataID;
                                    piezaMontada.cilindroID = pieza.cilindroID;
                                }

                                piezaMontada.reparando = pieza.reparando;
                                piezaMontada.activa = pieza.activa;
                                piezaMontada.montada = pieza.montada;
                            }
                        }
                    }

                    #region Se agregan las piezas nuevas a la barrenadora.
                    if (listaPiezasNuevas.Count > 0)
                    {
                        // Se le asigna el número de serie a cada pieza
                        listaPiezasNuevas.ForEach(nuevaPieza =>
                        {
                            // Se valida que no exista otro martillo con el mismo número de serie.
                            var martilloPorAgregar = listaPiezasNuevas.FirstOrDefault(x => x.tipoPieza == TipoPiezaEnum.Martillo);
                            if (martilloPorAgregar != null)
                            {
                                if (_context.tblB_PiezaBarrenadora.Any(y => y.tipoPieza == TipoPiezaEnum.Martillo && y.noSerie == martilloPorAgregar.noSerie))
                                {
                                    throw new Exception("Ya existe una pieza de tipo martillo con ese número de serie.");
                                }
                            }

                            // Se busca el tipo de broca del insumo
                            var pieza = _context.tblB_CatalogoPieza.FirstOrDefault(x => x.insumo == nuevaPieza.insumo && x.activo);
                            nuevaPieza.tipoBroca = pieza != null ? pieza.tipoBroca : TipoBrocaEnum.NO_APLICA;
                            pieza.incremento += 1;

                            #region Validar si el número de serie existe o no
                            var noSerieAnterior = nuevaPieza.noSerie;
                            var checkNumeroSerieExistente = _context.tblB_PiezaBarrenadora.FirstOrDefault(x => x.insumo == nuevaPieza.insumo && x.noSerie == noSerieAnterior);

                            if (checkNumeroSerieExistente != null)
                            {
                                nuevaPieza.noSerie = ObtenerNumeroSeriePieza(nuevaPieza);

                                foreach (var diaria in listaCapturaDiaria)
                                {
                                    if (diaria.brocaSerie == noSerieAnterior)
                                    {
                                        diaria.brocaSerie = nuevaPieza.noSerie;
                                    }
                                    else if (diaria.martilloSerie == noSerieAnterior)
                                    {
                                        diaria.martilloSerie = nuevaPieza.noSerie;
                                    }
                                    else if (diaria.barraSerie == noSerieAnterior)
                                    {
                                        diaria.barraSerie = nuevaPieza.noSerie;
                                    }
                                    else if (diaria.barraSegundaSerie == noSerieAnterior)
                                    {
                                        diaria.barraSegundaSerie = nuevaPieza.noSerie;
                                    }
                                    else if (diaria.culataSerie == noSerieAnterior)
                                    {
                                        diaria.culataSerie = nuevaPieza.noSerie;
                                    }
                                    else if (diaria.cilindroSerie == noSerieAnterior)
                                    {
                                        diaria.cilindroSerie = nuevaPieza.noSerie;
                                    }
                                }
                            }
                            #endregion

                            nuevaPieza.areaCuenta = listaCapturaDiaria.FirstOrDefault().areaCuenta;
                            nuevaPieza.reparando = false;
                            nuevaPieza.activa = true;
                            nuevaPieza.montada = true;
                        });

                        _context.tblB_PiezaBarrenadora.AddRange(listaPiezasNuevas);
                        _context.SaveChanges();

                        var martilloNuevo = listaPiezasNuevas.FirstOrDefault(x => x.tipoPieza == TipoPiezaEnum.Martillo);

                        if (martilloNuevo != null)
                        {
                            martilloNuevo.culataID = listaPiezasNuevas.FirstOrDefault(x => x.tipoPieza == TipoPiezaEnum.Culata).id;
                            martilloNuevo.cilindroID = listaPiezasNuevas.FirstOrDefault(x => x.tipoPieza == TipoPiezaEnum.Cilindro).id;
                        }

                        // Se registra la alta de las nuevas piezas.
                        listaPiezasNuevas.ForEach(pieza =>
                        {
                            RegistrarMovimientoHistorial(pieza, TipoMovimientoPiezaEnum.AltaNueva);
                        });
                    }
                    #endregion

                    ActualizarEstatusPiezasBarrenadora(barrenadoraID, true);

                    #region Se inserta las plantillas
                    foreach (var diaria in listaCapturaDiaria.Where(x => x.brocaID == 0 || x.martilloID == 0 || x.barraID == 0 || x.barraSegundaID == 0 || x.culataID == 0 || x.cilindroID == 0))
                    {
                        diaria.brocaID = diaria.brocaID == 0 ? listaPiezasNuevas.FirstOrDefault(x => x.noSerie == diaria.brocaSerie).id : diaria.brocaID;

                        diaria.martilloID = diaria.martilloID == 0 ? (listaPiezasNuevas.FirstOrDefault(x => x.noSerie == diaria.martilloSerie) != null ? listaPiezasNuevas.FirstOrDefault(x => x.noSerie == diaria.martilloSerie).id : 0) : diaria.martilloID;
                        diaria.barraID = diaria.barraID == 0 ? (listaPiezasNuevas.FirstOrDefault(x => x.noSerie == diaria.barraSerie) != null ? listaPiezasNuevas.FirstOrDefault(x => x.noSerie == diaria.barraSerie).id : 0) : diaria.barraID;
                        diaria.barraSegundaID =
                            (diaria.barraSegundaID == 0 && diaria.barraSegundaSerie != null && diaria.barraSegundaSerie.Count() > 0) ?
                            listaPiezasNuevas.FirstOrDefault(x => x.noSerie == diaria.barraSegundaSerie).id : diaria.barraSegundaID;
                        if (diaria.martilloID != 0)
                        {
                            diaria.culataID = diaria.culataID == 0 ? listaPiezasNuevas.FirstOrDefault(x => x.noSerie == diaria.culataSerie).id : diaria.culataID;
                            diaria.cilindroID = diaria.cilindroID == 0 ? listaPiezasNuevas.FirstOrDefault(x => x.noSerie == diaria.cilindroSerie).id : diaria.cilindroID;
                        }
                        else
                        {
                            diaria.culataID = 0;
                            diaria.cilindroID = 0;
                        }
                        diaria.zancoID = diaria.zancoID == 0 ? (listaPiezasNuevas.FirstOrDefault(x => x.noSerie == diaria.zancoSerie) != null ? listaPiezasNuevas.FirstOrDefault(x => x.noSerie == diaria.zancoSerie).id : 0) : diaria.zancoID;

                        diaria.fechaCreacion = DateTime.Now;
                    }
                    #endregion
                    #region Agregar horas trabajadas
                    #endregion
                    var tablaPiezas = _context.tblB_PiezaBarrenadora.ToList();

                    foreach (var diaria in listaCapturaDiaria)
                    {
                        var broca = tablaPiezas.First(x => x.id == diaria.brocaID);
                        if (broca != null)
                            broca.horasTrabajadas += diaria.horasTrabajadas;

                        if (diaria.martilloID != 0)
                        {
                            var martillo = tablaPiezas.First(x => x.id == diaria.martilloID);
                            martillo.horasTrabajadas += diaria.horasTrabajadas;
                        }

                        if (diaria.barraID != 0)
                        {
                            var barra = tablaPiezas.First(x => x.id == diaria.barraID);
                            barra.horasTrabajadas += diaria.horasTrabajadas;
                        }

                        if (diaria.barraSegundaID != 0)
                        {
                            var barraSegunda = tablaPiezas.FirstOrDefault(x => x.id == diaria.barraSegundaID);

                            if (barraSegunda != null)
                            {
                                barraSegunda.horasTrabajadas += diaria.horasTrabajadas;
                            }
                        }

                        if (diaria.martilloID != 0)
                        {
                            var culata = tablaPiezas.First(x => x.id == diaria.culataID);
                            culata.horasTrabajadas += diaria.horasTrabajadas;
                            var cilindro = tablaPiezas.First(x => x.id == diaria.cilindroID);
                            cilindro.horasTrabajadas += diaria.horasTrabajadas;
                        }

                        if (diaria.zancoID != 0)
                        {
                            var zanco = tablaPiezas.First(x => x.id == diaria.zancoID);
                            if (zanco != null)
                                zanco.horasTrabajadas += diaria.horasTrabajadas;
                        }

                        if (diaria.id == 0)
                            _context.tblB_CapturaDiaria.Add(diaria);
                        else
                        {
                            var capturaDiariaActual = _context.tblB_CapturaDiaria.FirstOrDefault(f => f.id == diaria.id);

                            capturaDiariaActual.areaCuenta = diaria.areaCuenta;
                            capturaDiariaActual.banco = diaria.banco;
                            capturaDiariaActual.barraID = diaria.barraID;
                            capturaDiariaActual.barraSegundaID = diaria.barraSegundaID;
                            capturaDiariaActual.barraSegundaSerie = diaria.barraSegundaSerie;
                            capturaDiariaActual.barraSerie = diaria.barraSerie;
                            capturaDiariaActual.barrenos = diaria.barrenos;
                            capturaDiariaActual.bordo = diaria.bordo;
                            capturaDiariaActual.brocaID = diaria.brocaID;
                            capturaDiariaActual.brocaSerie = diaria.brocaSerie;
                            capturaDiariaActual.cilindroID = diaria.cilindroID;
                            capturaDiariaActual.cilindroSerie = diaria.cilindroSerie;
                            capturaDiariaActual.claveAyudante = diaria.claveAyudante;
                            capturaDiariaActual.claveOperador = diaria.claveOperador;
                            capturaDiariaActual.culataID = diaria.culataID;
                            capturaDiariaActual.culataSerie = diaria.culataSerie;
                            capturaDiariaActual.densidadMaterial = diaria.densidadMaterial;
                            capturaDiariaActual.detalles = diaria.detalles;
                            capturaDiariaActual.espaciamiento = diaria.espaciamiento;
                            capturaDiariaActual.fsrAyudante = diaria.fsrAyudante;
                            capturaDiariaActual.fsrOperador = diaria.fsrOperador;
                            capturaDiariaActual.horasTrabajadas = diaria.horasTrabajadas;
                            capturaDiariaActual.horometroFinal = diaria.horometroFinal;
                            capturaDiariaActual.martilloID = diaria.martilloID;
                            capturaDiariaActual.martilloSerie = diaria.martilloSerie;
                            capturaDiariaActual.portabitID = diaria.portabitID;
                            capturaDiariaActual.portabitSerie = diaria.portabitSerie;
                            capturaDiariaActual.precioAyudante = diaria.precioAyudante;
                            capturaDiariaActual.precioOperador = diaria.precioOperador;
                            capturaDiariaActual.profundidad = diaria.profundidad;
                            capturaDiariaActual.rehabilitacion = diaria.rehabilitacion;
                            capturaDiariaActual.subBarreno = diaria.subBarreno;
                            capturaDiariaActual.tipoBarreno = diaria.tipoBarreno;
                            capturaDiariaActual.tipoCaptura = diaria.tipoCaptura;
                            capturaDiariaActual.totalAyudante = diaria.totalAyudante;
                            capturaDiariaActual.totalOperador = diaria.totalOperador;
                            capturaDiariaActual.turno = diaria.turno;
                            capturaDiariaActual.usuarioCreadorID = diaria.usuarioCreadorID;
                            capturaDiariaActual.zancoID = diaria.zancoID;
                            capturaDiariaActual.zancoSerie = diaria.zancoSerie;
                            _context.SaveChanges();
                        }
                    }

                    _context.SaveChanges();
                    resultado.Add(SUCCESS, true);
                    dbTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, nombreControlador, "GuardarPiezasBarrenadora", e, AccionEnum.ACTUALIZAR, 0, listaPiezas);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error al guardar las piezas de la barrenadora.");
                }
                return resultado;
            }
        }

        #region Reporte Ejecutivo
        private decimal getCostosPiezas(TipoPiezaEnum tipoPza)
        {
            if (TipoPiezaEnum.Martillo == tipoPza)
            {
                var precioPromedio = _context.tblB_PiezaBarrenadora.Where(r => r.tipoPieza == tipoPza).Average(r => r.precio);
                var horasPromedio = _context.tblB_PiezaBarrenadora.Where(r => r.tipoPieza == tipoPza).Average(r => r.horasAcumuladas / (r.cantidadReparaciones + 1));
                return (precioPromedio / horasPromedio);
            }
            else
            {
                var precioPromedio = _context.tblB_PiezaBarrenadora.Where(r => !r.activa && r.tipoPieza == tipoPza).Average(r => r.precio);
                var horasPromedio = _context.tblB_PiezaBarrenadora.Where(r => !r.activa && r.tipoPieza == tipoPza).Average(r => r.horasAcumuladas);
                return (precioPromedio / horasPromedio);
            }
        }

        public Dictionary<string, object> setReporteEjecutivo(DateTime fechaInicio, DateTime fechaFinal, List<string> areaCuenta, List<int> barrenadorasLista)
        {
            var kpiData = kpiFactoryServices.getKPIFactoryService().getKPIGeneralNoFormat(new List<string> { areaCuenta.FirstOrDefault() }, 221, 18, fechaInicio, fechaFinal);
            var tipoCambioPerdiodo = getPromedioTipoCambio(fechaInicio, fechaFinal); //Se obtiene el tipo de cambio de un periodo seleccionado

            List<reporteEjecutivoDTO> dt = new List<reporteEjecutivoDTO>();
            reporteEjecutivoDTO obj = new reporteEjecutivoDTO();

            string equipos = "";
            try
            {
                ///Obtener datos Generales.
                var capturasDiarias = _context.tblB_CapturaDiaria.Where(r => r.fechaCaptura >= fechaInicio && r.fechaCaptura <= fechaFinal && areaCuenta.Contains(r.areaCuenta) && r.horasTrabajadas > 0 && r.barrenos > 0 && barrenadorasLista.Contains(r.barrenadoraID)).ToList();
                var otrosGastos = _context.tblB_OtrosGastos.Where(r => r.fechaCaptura >= fechaInicio && r.fechaCaptura <= fechaFinal && areaCuenta.Contains(r.areaCuenta));

                var barrenadoras = capturasDiarias.Select(r => r.barrenadora.id).ToList<int>().Distinct();
                var herramientas = _context.tblB_PiezaBarrenadora.Where(r => barrenadoras.Contains((int)r.barrenadoraID));
                decimal horasTotalesPeriodo = capturasDiarias.Sum(r => r.horasTrabajadas);

                var equiposModelo = capturasDiarias.Select(r => r.barrenadora.maquinaID).ToList();
                var ccID = _context.tblP_CC.Where(r => areaCuenta.Contains(r.areaCuenta)).Select(f => f.id);
                var conciliacion = _context.tblM_CapEncConciliacionHorometros.Where(r => ccID.Contains(r.centroCostosID) && r.fechaInicio >= fechaInicio && r.fechaFin <= fechaFinal).Select(r => r.id).ToList();
                var costoRentaModelos = _context.tblM_CapConciliacionHorometros.Where(r => conciliacion.Contains(r.idEncCaratula) && equiposModelo.Contains(r.noEconomicoID));

                //_context.tblM_CapCaratula.FirstOrDefault(r => r.idCaratula == encCaratula.id && equiposModelo.Contains(r.idModelo));

                var agrupoModelos = costoRentaModelos.GroupBy(a => a.modelo).Select(r => new rentaEquipoDetalleDTO
                {
                    modelo = r.Key,
                    horasPeriodo = r.Sum(f => f.horometroEfectivo),
                    total = r.FirstOrDefault(a => a.modelo == r.Key).moneda == 1 ? r.Where(h => h.modelo == r.Key).Sum(f => f.total) : r.Where(h => h.modelo == r.Key).Sum(f => f.total) * tipoCambioPerdiodo,
                    moneda = r.FirstOrDefault(a => a.modelo == r.Key).moneda == 1 ? "MXN" : "USD",
                    totalUSD = r.FirstOrDefault(a => a.modelo == r.Key).moneda == 2 ? r.Where(h => h.modelo == r.Key).Sum(f => f.total) : (r.Where(h => h.modelo == r.Key).Sum(f => f.total) / tipoCambioPerdiodo)

                }).ToList();


                foreach (var item in costoRentaModelos)
                {
                    if (item.moneda == 1)
                    {
                        obj.rentaEquipos += item.total;
                        obj.rentaEquiposDLLS += item.total / tipoCambioPerdiodo;
                    }
                    else
                    {
                        obj.rentaEquipos += item.total * tipoCambioPerdiodo;
                        obj.rentaEquiposDLLS += item.total;//(costoRentaModelo.costo * (decimal)totalDias) / tipoCambioPerdiodo;
                    }
                }

                var totalDias = (fechaFinal - fechaInicio).TotalDays;

                capturasDiarias.Select(r => r.barrenadora.maquina.noEconomico).Distinct().ToList().ForEach(r =>
                {
                    equipos += r + ",";
                });

                var listaEconomicos = capturasDiarias.Select(b => b.barrenadora.maquina.noEconomico).Distinct().ToList();

                #region Combustibles.
                var combistible = _context.tblM_CapCombustible.Where(r => r.fecha >= fechaInicio && r.fecha <= fechaFinal).Where(r => listaEconomicos.Contains(r.Economico) && areaCuenta.Contains(r.CC));
                decimal precioD = precioDieselFactoryServices.getPrecioDieselService().GetPrecioDiesel().precio; //Precio del Diesel esta en persos.
                obj.combustibles = precioD * combistible.Sum(r => r.volumne_carga);
                obj.precioCombustible = precioD;
                obj.precioCombustibleDLLS = precioD / tipoCambioPerdiodo;
                obj.LitrosCombustible = combistible.Sum(r => r.volumne_carga);

                #endregion
                // la mayoria de datos en BD de datos esta en 0
                var operadoresCostos = capturasDiarias.Where(o => o.claveOperador > 0 && o.totalOperador > 0).ToList();
                if(operadoresCostos.Count() > 0)
                {
                    obj.equipos = equipos.TrimEnd(',');
                    obj.fechaInicio = fechaInicio;
                    obj.fechaFin = fechaFinal;
                    obj.operador = operadoresCostos.Sum(r => r.totalOperador * r.horasTrabajadas); //* horasTotalesPeriodo;
                    obj.costoPromedioOperador = (decimal)operadoresCostos.Average(r => r.totalOperador);
                    obj.horasTotales = horasTotalesPeriodo;
                }
                else
                {
                    obj.equipos = equipos.TrimEnd(',');
                    obj.fechaInicio = fechaInicio;
                    obj.fechaFin = fechaFinal;
                    obj.operador = 0m; //* horasTotalesPeriodo;
                    obj.costoPromedioOperador = 0m;
                    obj.horasTotales = 0m;
                }
        

                /*
                 Se encarga de obtener las piezas para sacar el calculo de acero de desgaste.
                 */

                var historialPiezas = _context.tblB_HistorialPieza.Where(x => x.tipoMovimiento == TipoMovimientoPiezaEnum.AltaNueva).ToList();

                var precioPromedioBarra = getCostosPiezas(TipoPiezaEnum.Barra);
                var precioPromedioBroca = getCostosPiezas(TipoPiezaEnum.Broca);
                var precioPromedioMartillo = getCostosPiezas(TipoPiezaEnum.Martillo);

                obj.costoBarra = precioPromedioBarra;
                obj.costoBroca = precioPromedioBroca;
                obj.costoMartillo = precioPromedioMartillo;

                var costoBarra = precioPromedioBarra * horasTotalesPeriodo;
                var costoBroca = precioPromedioBroca * horasTotalesPeriodo;
                var costoMartillo = precioPromedioMartillo * horasTotalesPeriodo;

                decimal TotalAceroDesgasteT = costoBarra + costoBroca + costoMartillo;
                obj.aceroDesgaste = TotalAceroDesgasteT;

                //Total Costos
                obj.suma += obj.rentaEquipos + obj.combustibles + obj.aceroDesgaste + obj.operador;


                decimal metrosLineales = 0;
                decimal toneladas = 0;

                /*Mestros Lineaeles Totales.*/
                metrosLineales = capturasDiarias.Sum(x => (x.barrenos * 6) + (x.rehabilitacion * 6));

                toneladas = capturasDiarias.Sum(x => ((x.barrenos * (x.profundidad + x.subBarreno)) + (x.rehabilitacion * (x.profundidad + x.subBarreno))) * x.bordo * x.espaciamiento * x.densidadMaterial);

                decimal m3 = capturasDiarias.Sum(x => ((x.barrenos * (x.profundidad + x.subBarreno)) + (x.rehabilitacion * (x.profundidad + x.subBarreno))) * x.bordo * x.espaciamiento);
                obj.CostoM3 = obj.suma / m3;
                obj.CostoM3DLLS = (obj.suma / m3) / tipoCambioPerdiodo;

                obj.costosMetroLineal = obj.suma / metrosLineales;
                obj.costoTonelada = obj.suma / toneladas;

                #region rendimiento

                var brocasPeriodo = capturasDiarias.GroupBy(g => g.brocaID).Select(r => r.Key).ToList();
                var listaBrocas = _context.tblB_PiezaBarrenadora.Where(r => brocasPeriodo.Contains(r.id) && !r.activa).ToList();

                if (listaBrocas != null)
                {
                    var promedioHrs = listaBrocas.Average(r => r.horasAcumuladas);
                    obj.brocaHR = promedioHrs;
                    var pzasSelect = listaBrocas.Select(r => r.id).ToList();

                    var grupoBroca = from a in _context.tblB_CapturaDiaria
                                     where pzasSelect.Contains(a.brocaID) && a.fechaCaptura <= fechaFinal
                                     group a by a.brocaSerie into g
                                     select new { Category = g.Key, AveragePrice = g.Sum(p => (p.barrenos * p.profundidad) + (p.rehabilitacion * p.profundidad)) };

                    obj.brocaML = Convert.ToDecimal(grupoBroca.Average(r => r.AveragePrice));
                }


                var barrasPeriodo = capturasDiarias.GroupBy(g => g.barraID).Select(r => r.Key).ToList();
                var barrasPzas = _context.tblB_PiezaBarrenadora.Where(r => barrasPeriodo.Contains(r.id) && !r.activa).ToList();
                if (barrasPzas.Count > 0)
                {
                    var barrasPromedioHrs = barrasPzas.Average(r => r.horasAcumuladas);
                    obj.barraHR = barrasPromedioHrs;

                    var listaBarras = barrasPzas.Select(r => r.id).ToList();

                    var resultBarras = from a in _context.tblB_CapturaDiaria
                                       where listaBarras.Contains(a.barraID) && a.fechaCaptura <= fechaFinal
                                       group a by a.barraSerie into g
                                       select new { Category = g.Key, AveragePrice = g.Sum(p => (p.barrenos * p.profundidad) + (p.rehabilitacion * p.profundidad)) };

                    obj.barraML = Convert.ToDecimal(resultBarras.Average(r => r.AveragePrice));

                }

                var MartilloPeriodo = capturasDiarias.GroupBy(g => g.martilloID).Select(r => r.Key).ToList();
                var martilloPzas = _context.tblB_PiezaBarrenadora.Where(r => MartilloPeriodo.Contains(r.id) && r.cantidadReparaciones > 0).ToList();
                if (martilloPzas.Count > 0)
                {
                    var hrasPromedioMartillo = martilloPzas.Average(r => r.horasAcumuladas / (r.cantidadReparaciones + 1));
                    obj.martilloHR = hrasPromedioMartillo;

                    var listaMartillo = martilloPzas.Select(r => r.id).ToList();

                    var resultMartillo = from a in _context.tblB_CapturaDiaria
                                         where listaMartillo.Contains(a.martilloID) && a.fechaCaptura <= fechaFinal
                                         group a by a.barraSerie into g
                                         select new { Category = g.Key, AveragePrice = g.Sum(p => (p.barrenos * p.profundidad) + (p.rehabilitacion * p.profundidad)) };

                    obj.martilloML = Convert.ToDecimal(resultMartillo.Average(r => r.AveragePrice));
                }
                else
                {
                    obj.martilloHR = 0;
                    obj.barraML = 0;
                }

                #endregion

                obj.fechaInicio = fechaInicio;
                obj.fechaFin = fechaFinal;
                //obj.barraHR = 0;      // por mientras se agregan las piezas a las capturas
                //obj.barraML = 0;      // por mientras se agregan las piezas a las capturas
                //obj.brocaHR = 0;      // por mientras se agregan las piezas a las capturas
                //obj.brocaML = 0;      // por mientras se agregan las piezas a las capturas
                //obj.martilloML = 0;   // por mientras se agregan las piezas a las capturas
                //obj.martilloHR = 0;   // por mientras se agregan las piezas a las capturas
                //obj.barraML = 0;      // por mientras se agregan las piezas a las capturas
                obj.precioOtros = otrosGastos.ToList().Count > 0 ? otrosGastos.Sum(r => r.monto) : 0;
                obj.precioOtrosDLLS = otrosGastos.ToList().Count > 0 ? otrosGastos.Sum(r => r.monto) / tipoCambioPerdiodo : 0;
                obj.disponibilidad = kpiData.Sum(r => r.pDisponibilidad) / kpiData.Count;
                obj.velocidadBarrenacion = metrosLineales / horasTotalesPeriodo;
                obj.combustibleDLLS = obj.combustibles / tipoCambioPerdiodo;
                obj.aceroDesgasteDLLS = obj.aceroDesgaste / tipoCambioPerdiodo;
                obj.operadorDLLS = obj.operador / tipoCambioPerdiodo;
                obj.sumaDLLS = obj.suma / tipoCambioPerdiodo;
                obj.costoMetroLinealDLLS = obj.costosMetroLineal / tipoCambioPerdiodo;
                obj.costoToneladaDLLS = obj.costoTonelada / tipoCambioPerdiodo;
                obj.utilizacion = kpiData.Sum(r => r.pUtilizacion) / kpiData.Count;

                HttpContext.Current.Session["dtoEjecutivo"] = obj;
                HttpContext.Current.Session["dtoDetRentaQuipos"] = agrupoModelos;
                resultado.Add(SUCCESS, true);

                return resultado;
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, "No hay informacion completa para mostrar en el reporte pruebe con diferentes filtros.");
                resultado.Add(SUCCESS, false);
                return resultado;
            }
        }

        private decimal getPromedioTipoCambio(DateTime fechaInicio, DateTime fechaFinal)
        {
            decimal tipoCambio = 0;
            try
            {
                string consulta = @"SELECT tipo_cambio AS TotalDieselContratista FROM tipo_cambio
                                    WHERE fecha BETWEEN '" + fechaInicio.ToString("yyyyMMdd") + "' AND '" + fechaFinal.ToString("yyyyMMdd") + "';";
                var res1 = (IList<totalDieselContratistaDTO>)_contextEnkontrol.Where(consulta).ToObject<IList<totalDieselContratistaDTO>>();
                return res1.FirstOrDefault().TotalDieselContratista;
            }
            catch (Exception)
            {

                return 0;
            }

        }

        private decimal calculos(decimal dividendo, decimal divisor)
        {
            decimal resultado = 0;
            if (dividendo != 0 && divisor != 0)
                resultado = dividendo / divisor;
            return resultado;
        }

        private decimal getTotalContratistaConsumoDiesel(string cc, DateTime fechaFin, DateTime fechaInicio)
        {
            try
            {
                string consulta = @"SELECT AVG( precio) AS TotalDieselContratista FROM si_movimientos_det A 
                                    INNER JOIN si_movimientos B
                                    ON A.almacen = B.almacen AND A.tipo_mov = B.tipo_mov AND A.numero = B.numero
                                    WHERE insumo = 1180001 AND fecha BETWEEN '" + fechaInicio.ToShortDateString() + "' AND '" + fechaFin.ToShortDateString() + "';";

                var res1 = (IList<totalDieselContratistaDTO>)_contextEnkontrol.Where(consulta).ToObject<IList<totalDieselContratistaDTO>>();
                return res1.FirstOrDefault().TotalDieselContratista;

            }
            catch
            {
                return 0;
            }

        }
        #endregion

        #region Captura Agua
        public Dictionary<string, object> guardarAgua(string areaCuenta, int turno, string fechaCaptura, decimal litros, int id)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                tblB_CapturaLitrosAgua capturaLitrosAguas = new tblB_CapturaLitrosAgua();
                try
                {
                    int usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;

                    if (id == 0)
                    {
                        capturaLitrosAguas.id = 0;
                        capturaLitrosAguas.areaCuenta = areaCuenta;
                        capturaLitrosAguas.estatus = true;
                        capturaLitrosAguas.fechaCaptura = Convert.ToDateTime(fechaCaptura);
                        capturaLitrosAguas.fechaCreacion = DateTime.Now;
                        capturaLitrosAguas.usuarioCreadorID = usuarioCreadorID;
                        capturaLitrosAguas.litros = litros;

                        _context.tblB_CapturaLitrosAgua.Add(capturaLitrosAguas);
                        _context.SaveChanges();
                    }
                    else
                    {
                        var actualizacionLitrosAgua = _context.tblB_CapturaLitrosAgua.FirstOrDefault(x => x.id == id);
                        actualizacionLitrosAgua.litros = litros;

                        _context.SaveChanges();
                    }
                    _context.SaveChanges();
                    resultado.Add(SUCCESS, true);
                    dbTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, nombreControlador, "GuardarLitrosAgus", e, AccionEnum.ACTUALIZAR, 0, capturaLitrosAguas);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error al guardar los operadores de la barrenadora.");
                }
                return resultado;
            }
        }

        public Dictionary<string, object> guardarOtrosPrecios(string areaCuenta, int turno, string fechaCaptura, decimal gasto, int id)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                tblB_OtrosGastos OtrosGastos = new tblB_OtrosGastos();
                try
                {
                    int usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;

                    if (id == 0)
                    {
                        OtrosGastos.id = 0;
                        OtrosGastos.areaCuenta = areaCuenta;
                        OtrosGastos.estatus = true;
                        OtrosGastos.fechaCaptura = Convert.ToDateTime(fechaCaptura);
                        OtrosGastos.fechaCreacion = DateTime.Now;
                        OtrosGastos.usuarioCreadorID = usuarioCreadorID;
                        OtrosGastos.monto = gasto;

                        _context.tblB_OtrosGastos.Add(OtrosGastos);
                        _context.SaveChanges();
                    }
                    else
                    {
                        var actualizacionLitrosAgua = _context.tblB_OtrosGastos.FirstOrDefault(x => x.id == id);
                        actualizacionLitrosAgua.monto = gasto;

                        _context.SaveChanges();
                    }
                    _context.SaveChanges();
                    resultado.Add(SUCCESS, true);
                    dbTransaction.Commit();

                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, nombreControlador, "GuardarLitrosAgus", e, AccionEnum.ACTUALIZAR, 0, OtrosGastos);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error al guardar los operadores de la barrenadora.");
                }
                return resultado;
            }
        }

        #endregion

        #region ReporteCapturaGeneral
        public Dictionary<string, object> CargarRptGeneralCapturas(List<string> areaCuenta, List<int> turno, DateTime fechaInicio, DateTime fechaFin)
        {

            try
            {
                if (turno == null) turno = new List<int>();
                var capturaDiaria = _context.tblB_CapturaDiaria
                .Where(x =>

                        (areaCuenta.Count == 0 ? true : areaCuenta.Contains(x.areaCuenta)) &&
                        (turno.Count == 0 ? true : turno.Contains(x.turno)) &&
                        x.fechaCaptura >= fechaInicio &&
                        x.fechaCaptura <= fechaFin &&
                        x.horasTrabajadas > 0
                        )
                    .ToList().GroupBy(r => new { r.turno, r.fechaCaptura })
                    .Select(r => new rptGeneralesCapturas
                    {
                        turno = r.Key.turno,
                        fecha = r.Key.fechaCaptura,
                        barrenos = r.Sum(b => b.barrenos),
                        rehabilitacion = r.Sum(b => b.rehabilitacion),
                        metrosLineales = r.Sum(b => (b.barrenos * 6) + (b.rehabilitacion * 6)),
                        metrosLinealesEfectivos = r.Sum(b => ((b.barrenos * 6) + (b.rehabilitacion * 6)) - b.barrenos),
                        bordo = r.FirstOrDefault().bordo,
                        espaciamiento = r.FirstOrDefault().espaciamiento, //.Sum(b => b.espaciamiento),
                        densidadMaterial = r.Average(b => b.densidadMaterial),
                        m3 = r.Sum(b => (b.barrenos * (b.profundidad + b.subBarreno) * b.bordo * b.espaciamiento)), //.Sum(b => ((b.barrenos * 6) + (b.rehabilitacion * 6)) * (b.bordo) * b.espaciamiento),
                        metrolinealHr = r.Sum(b => (b.barrenos * (b.profundidad + b.subBarreno)) + (b.rehabilitacion * (b.profundidad + b.subBarreno))) / r.Sum(f => f.horasTrabajadas),
                        toneladaHR = r.Sum(b => (b.barrenos * (b.profundidad + b.subBarreno) * b.densidadMaterial * b.bordo * b.espaciamiento)) / r.Sum(f => f.horasTrabajadas),
                        m3HR = r.Sum(b => (b.barrenos * (b.profundidad + b.subBarreno) * b.bordo * b.espaciamiento)) / r.Sum(f => f.horasTrabajadas),
                    }).OrderBy(r => r.turno).ThenBy(r => r.fecha).ToList();
                resultado.Add(ITEMS, capturaDiaria);
                resultado.Add(SUCCESS, true);
                HttpContext.Current.Session["rptCapturaGeneral"] = capturaDiaria;
            }
            catch (Exception)
            {

            }

            return resultado;
        }

        private decimal getM3(List<tblB_CapturaDiaria> list)
        {

            decimal m3 = 0;

            foreach (var item in list)
            {
                decimal metrosLineales = 0;
                metrosLineales = (item.barrenos * item.profundidad) + (item.rehabilitacion * item.profundidad);

                m3 += metrosLineales * item.bordo * item.espaciamiento;
            }

            return m3;
        }
        #endregion

        #region Reporte operadores
        public Dictionary<string, object> ObtenerOperadores(string areaCuenta)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                List<ComboDTO> listaAC = _context.tblB_ManoObra.Where(b => b.activo).ToList() //;&& b.a == areaCuenta)
                    .Select(operador => new ComboDTO
                    {
                        Text = ObtenerNombreEmpleadoPorClave(operador.claveEmpleado),
                        Value = operador.claveEmpleado.ToString()
                    }).ToList();

                resultado.Add(ITEMS, listaAC);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, nombreControlador, "ObtenerAC", e, AccionEnum.CONSULTA, 0, null);
            }
            return resultado;
        }

        public Dictionary<string, object> CargarRptOperadores(List<string> areaCuenta, List<int> claveEmpleados, DateTime fechaInicio, DateTime fechaFin)
        {

            try
            {
                var capturaDiaria = _context.tblB_CapturaDiaria
                .Where(x =>

                         (areaCuenta.Count == 0 ? true : areaCuenta.Contains(x.areaCuenta)) &&
                        (claveEmpleados.Count == 0 ? true : claveEmpleados.Contains(x.claveOperador)) &&
                        x.fechaCaptura >= fechaInicio &&
                        x.fechaCaptura <= fechaFin &&
                        x.horasTrabajadas > 0
                        )
                    .ToList().GroupBy(r => new { r.turno, r.fechaCaptura, r.claveOperador })
                    .Select(r => {
                        var horasTrabajadas = r.Where(f => r.Key.turno == f.turno && f.fechaCaptura.Date == r.Key.fechaCaptura.Date).Sum(x => x.horasTrabajadas);
                        return new ReporteOperadoresDTO
                        {
                            operador = ObtenerNombreEmpleadoPorClave(r.Key.claveOperador),
                            turno = r.Key.turno,
                            fecha = r.Key.fechaCaptura,
                            barrenos = r.Where(f => r.Key.turno == f.turno && f.fechaCaptura.Date == r.Key.fechaCaptura.Date).Sum(b => b.barrenos),
                            rehabilitacion = r.Where(f => r.Key.turno == f.turno && f.fechaCaptura.Date == r.Key.fechaCaptura.Date).Sum(b => b.rehabilitacion),
                            metrosLineales = r.Where(f => r.Key.turno == f.turno && f.fechaCaptura.Date == r.Key.fechaCaptura.Date).Sum(b => (b.barrenos * 6) + (b.rehabilitacion * 6)),
                            metrosLinealesEfectivos = r.Where(f => r.Key.turno == f.turno && f.fechaCaptura.Date == r.Key.fechaCaptura.Date).Sum(b => ((b.barrenos * 6) + (b.rehabilitacion * 6)) - b.barrenos),
                            bordo = r.Where(f => r.Key.turno == f.turno && f.fechaCaptura.Date == r.Key.fechaCaptura.Date).Sum(b => b.bordo),
                            espaciamiento = r.Where(f => r.Key.turno == f.turno && f.fechaCaptura.Date == r.Key.fechaCaptura.Date).Sum(b => b.espaciamiento),
                            densidadMaterial = r.Where(f => r.Key.turno == f.turno && f.fechaCaptura.Date == r.Key.fechaCaptura.Date).Sum(b => b.densidadMaterial),
                            m3 = r.Where(f => r.Key.turno == f.turno && f.fechaCaptura.Date == r.Key.fechaCaptura.Date).Sum(b => (b.barrenos * (b.profundidad + b.subBarreno) * b.bordo * b.espaciamiento)),
                            metrolinealHr = r.Where(f => r.Key.turno == f.turno && f.fechaCaptura.Date == r.Key.fechaCaptura.Date).Sum(b => (b.barrenos * (b.profundidad + b.subBarreno)) + (b.rehabilitacion * (b.profundidad + b.subBarreno))) / (horasTrabajadas == 0 ? 1 : horasTrabajadas) ,
                            toneladaHR = r.Where(f => r.Key.turno == f.turno && f.fechaCaptura.Date == r.Key.fechaCaptura.Date).Sum(b => (b.barrenos * (b.profundidad + b.subBarreno) * b.densidadMaterial * b.bordo * b.espaciamiento)) / (horasTrabajadas == 0 ? 1 : horasTrabajadas),
                            m3HR = r.Where(f => r.Key.turno == f.turno && f.fechaCaptura.Date == r.Key.fechaCaptura.Date).Sum(b => (b.barrenos * (b.profundidad + b.subBarreno) * b.bordo * b.espaciamiento)) / (horasTrabajadas == 0 ? 1 : horasTrabajadas),

                        };
                    }).OrderBy(r => r.operador).ThenBy(r => r.fecha).ToList();
                resultado.Add(ITEMS, capturaDiaria);
                resultado.Add(SUCCESS, true);
                HttpContext.Current.Session["rptBarrenacionOperadores"] = capturaDiaria;
            }
            catch (Exception)
            {

            }

            return resultado;
        }

        public Dictionary<string, object> CargarRptEquiposstanby(List<string> areaCuenta, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var capturaDiaria = _context.tblB_CapturaDiaria
                .Where(x =>

                         (areaCuenta.Count == 0 ? true : areaCuenta.Contains(x.areaCuenta)) &&
                        x.fechaCaptura >= fechaInicio &&
                        x.fechaCaptura <= fechaFin && x.barrenadora != null && x.barrenadoraID != 0
                        )
                    .ToList().GroupBy(r => new { r.barrenadora.maquina.noEconomico })
                    .Select(r => new
                    {
                        noEconomico = r.Key.noEconomico,
                        combustible = r.Where(f => f.tipoCaptura == TipoCapturaEnum.FaltaDeCombustible).Count(),
                        malClima = r.Where(f => f.tipoCaptura == TipoCapturaEnum.Clima).Count(),
                        standby = r.Where(f => f.tipoCaptura == TipoCapturaEnum.StandBy).Count(),
                        faltaTramo = r.Where(f => f.tipoCaptura == TipoCapturaEnum.FaltaDeTramo).Count(),
                        mantenimiento = r.Where(f => f.tipoCaptura == TipoCapturaEnum.Mantenimiento).Count()

                    }).OrderBy(r => r.noEconomico);
                resultado.Add(ITEMS, capturaDiaria);
                resultado.Add(SUCCESS, true);

            }
            catch (Exception)
            {

            }

            return resultado;
        }
        #endregion

        #region Captura PagoMensual

        public Dictionary<string, object> guardarPagoMensual(string areaCuenta, DateTime fechaCaptura, decimal cantidad, int id)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                tblB_PagoMensual pagoMensual = new tblB_PagoMensual();
                try
                {
                    int usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;

                    if (id == 0)
                    {
                        pagoMensual.id = 0;
                        pagoMensual.areaCuenta = areaCuenta;
                        pagoMensual.mes = fechaCaptura.Month;
                        pagoMensual.year = fechaCaptura.Year;
                        pagoMensual.fechaCaptura = Convert.ToDateTime(fechaCaptura);
                        pagoMensual.fechaEdita = DateTime.Now;
                        pagoMensual.capturaUsuarioID = usuarioCreadorID;
                        pagoMensual.editaUsuarioID = usuarioCreadorID;
                        pagoMensual.montoMensual = cantidad;

                        _context.tblB_PagoMensual.Add(pagoMensual);
                        _context.SaveChanges();
                    }
                    else
                    {
                        var actualizacionLitrosAgua = _context.tblB_PagoMensual.FirstOrDefault(x => x.id == id);
                        actualizacionLitrosAgua.montoMensual = cantidad;

                        _context.SaveChanges();
                    }
                    _context.SaveChanges();
                    resultado.Add(SUCCESS, true);
                    dbTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, nombreControlador, "GuardarLitrosAgus", e, AccionEnum.ACTUALIZAR, 0, pagoMensual);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error al guardar los operadores de la barrenadora.");
                }
                return resultado;
            }
        }
        #endregion


        public Dictionary<string, object> SaveOrUpdatePiezasBarrenadora(List<tblB_PiezaBarrenadora> listaPzas, bool pzasCompletas)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    listaPzas = listaPzas.Where(x => x.noSerie != null).ToList();
                    int usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;
                    int barrenadora = (int)listaPzas.FirstOrDefault().barrenadoraID;
                    foreach (var item in listaPzas)
                    {
                        if (item.id == 0)
                        {
                            _context.tblB_PiezaBarrenadora.Add(item);
                            RegistrarMovimientoHistorial(item, TipoMovimientoPiezaEnum.AltaNueva);
                        }
                        else
                        {
                            var pieza = _context.tblB_PiezaBarrenadora.FirstOrDefault(r => r.id == item.id);
                            pieza.reparando = item.reparando;
                            pieza.montada = item.montada;
                            pieza.activa = item.activa;
                            pieza.barrenadoraID = item.barrenadoraID;
                            pieza.culataID = item.culataID;
                            pieza.cilindroID = item.cilindroID;
                            _context.SaveChanges();

                            if (!pieza.activa)
                                RegistrarMovimientoHistorial(item, TipoMovimientoPiezaEnum.BajaDesecho);
                            if (pieza.reparando)
                                RegistrarMovimientoHistorial(item, TipoMovimientoPiezaEnum.BajaReparacion);
                            if (!pieza.montada)
                                RegistrarMovimientoHistorial(item, TipoMovimientoPiezaEnum.Baja);
                            if (pieza.montada && pieza.activa)
                                RegistrarMovimientoHistorial(item, TipoMovimientoPiezaEnum.Alta);
                        }

                        var catPza = _context.tblB_CatalogoPieza.FirstOrDefault(r => r.insumo == item.insumo && r.areaCuenta == item.areaCuenta);
                        catPza.incremento = catPza.incremento + 1;
                        _context.SaveChanges();


                    }
                    var martillo = listaPzas.FirstOrDefault(r => r.tipoPieza == TipoPiezaEnum.Martillo && r.montada);
                    if (martillo != null)
                    {
                        var martilloDBO = _context.tblB_PiezaBarrenadora.FirstOrDefault(a => a.id == martillo.id);
                        var cilindro = listaPzas.FirstOrDefault(r => r.tipoPieza == TipoPiezaEnum.Cilindro && r.montada);
                        if (cilindro != null)
                            martilloDBO.cilindroID = cilindro.id;
                        var culata = listaPzas.FirstOrDefault(r => r.tipoPieza == TipoPiezaEnum.Culata && r.montada);
                        if (culata != null)
                            martilloDBO.culataID = culata.id;
                        _context.SaveChanges();
                    }
                    ActualizarEstatusPiezasBarrenadora(barrenadora, pzasCompletas);

                    resultado.Add(SUCCESS, true);
                    dbTransaction.Commit();

                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, nombreControlador, "GuardarBanco", e, AccionEnum.ACTUALIZAR, 0, listaPzas);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error al guardar los operadores de la barrenadora.");
                }
                return resultado;
            }
        }

        public Dictionary<string, object> SaveOrUpdateCapturaDiaria(List<tblB_CapturaDiaria> listaCapturaDiaria, List<tblB_PiezaBarrenadora> listaPzas)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    int usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;
                    int barrenadora = (int)listaPzas.FirstOrDefault().barrenadoraID;
                    string areaCuenta = listaCapturaDiaria.FirstOrDefault().areaCuenta;
                    foreach (var item in listaPzas)
                    {
                        if (item.id == 0)
                        {
                            _context.tblB_PiezaBarrenadora.Add(item);
                            RegistrarMovimientoHistorial(item, TipoMovimientoPiezaEnum.AltaNueva);
                            var catPza = _context.tblB_CatalogoPieza.FirstOrDefault(r => r.insumo == item.insumo && r.areaCuenta == item.areaCuenta);
                            catPza.incremento = catPza.incremento + 1;
                            _context.SaveChanges();
                        }
                        else
                        {
                            var pieza = _context.tblB_PiezaBarrenadora.FirstOrDefault(r => r.id == item.id);

                            if (TipoPiezaEnum.Martillo == pieza.tipoPieza)
                            {
                                if (!item.montada && item.reparando)
                                {
                                    item.reparando = false;
                                    item.montada = true;
                                    item.cantidadReparaciones = pieza.cantidadReparaciones + 1;
                                    pieza.cantidadReparaciones = item.cantidadReparaciones;
                                    pieza.reparando = item.reparando;
                                    RegistrarMovimientoHistorial(item, TipoMovimientoPiezaEnum.ReparacionMartillo);
                                }


                            }
                            pieza.montada = item.montada;
                            pieza.activa = item.activa;
                            pieza.barrenadoraID = item.barrenadoraID;
                            pieza.culataID = 0;
                            pieza.cilindroID = 0;
                            _context.SaveChanges();

                            if (!pieza.activa)
                                RegistrarMovimientoHistorial(item, TipoMovimientoPiezaEnum.BajaDesecho);
                            if (!pieza.montada)
                                RegistrarMovimientoHistorial(item, TipoMovimientoPiezaEnum.Baja);
                            if (pieza.montada && pieza.activa)
                                RegistrarMovimientoHistorial(item, TipoMovimientoPiezaEnum.Alta);
                        }
                    }

                    var martillo = listaPzas.FirstOrDefault(r => r.tipoPieza == TipoPiezaEnum.Martillo && r.montada);
                    if (martillo != null)
                    {
                        var martilloDBO = _context.tblB_PiezaBarrenadora.FirstOrDefault(a => a.id == martillo.id);
                        var cilindro = listaPzas.FirstOrDefault(r => r.tipoPieza == TipoPiezaEnum.Cilindro && r.montada);
                        if (cilindro != null)
                            martilloDBO.cilindroID = cilindro.id;
                        var culata = listaPzas.FirstOrDefault(r => r.tipoPieza == TipoPiezaEnum.Culata && r.montada);
                        if (culata != null)
                            martilloDBO.culataID = culata.id;
                        _context.SaveChanges();
                    }
                    ActualizarEstatusPiezasBarrenadora(barrenadora, true);

                    foreach (var diaria in listaCapturaDiaria)
                    {
                        var tablaPiezas = _context.tblB_PiezaBarrenadora.Where(x => !x.reparando && x.activa && x.montada && x.barrenadoraID == barrenadora && areaCuenta == x.areaCuenta).ToList();

                        tblB_PiezaBarrenadora broca = tablaPiezas.FirstOrDefault(x => x.id == diaria.brocaID);
                        if (broca != null)
                        {
                            broca.horasTrabajadas += diaria.horasTrabajadas;
                            diaria.brocaID = broca.id;
                            diaria.brocaSerie = broca.noSerie;
                        }
                        else
                        {
                            broca = _context.tblB_PiezaBarrenadora.FirstOrDefault(x => x.noSerie == diaria.brocaSerie && x.barrenadoraID == barrenadora && areaCuenta == x.areaCuenta);
                            if (broca != null)
                            {
                                broca.horasTrabajadas += diaria.horasTrabajadas;
                                diaria.brocaID = broca.id;
                                diaria.brocaSerie = broca.noSerie;
                            }

                        }

                        tblB_PiezaBarrenadora objMartillo = tablaPiezas.FirstOrDefault(x => x.id == diaria.martilloID);
                        if (objMartillo != null)
                        {
                            objMartillo.horasTrabajadas += diaria.horasTrabajadas;
                            diaria.martilloID = objMartillo.id;
                            diaria.martilloSerie = objMartillo.noSerie;
                        }
                        else
                        {
                            objMartillo = _context.tblB_PiezaBarrenadora.FirstOrDefault(x => x.noSerie == diaria.martilloSerie && x.barrenadoraID == barrenadora && areaCuenta == x.areaCuenta);
                            if (objMartillo != null)
                            {
                                objMartillo.horasTrabajadas += diaria.horasTrabajadas;
                                diaria.martilloID = objMartillo.id;
                                diaria.martilloSerie = objMartillo.noSerie;
                            }

                        }

                        tblB_PiezaBarrenadora barra = tablaPiezas.FirstOrDefault(x => x.id == diaria.barraID);
                        if (barra != null)
                        {
                            barra.horasTrabajadas += diaria.horasTrabajadas;
                            diaria.barraID = barra.id;
                            diaria.barraSerie = barra.noSerie;
                        }
                        else
                        {
                            barra = _context.tblB_PiezaBarrenadora.FirstOrDefault(x => x.noSerie == diaria.barraSerie && x.barrenadoraID == barrenadora && areaCuenta == x.areaCuenta);
                            if (barra != null)
                            {
                                barra.horasTrabajadas += diaria.horasTrabajadas;
                                diaria.barraID = barra.id;
                                diaria.barraSerie = barra.noSerie;
                            }
                        }

                        if (diaria.barraSegundaID != 0)
                        {
                            var barraSegunda = tablaPiezas.FirstOrDefault(x => x.id == diaria.barraSegundaID && areaCuenta == x.areaCuenta);
                            if (barraSegunda != null)
                            {
                                barraSegunda.horasTrabajadas += diaria.horasTrabajadas;
                                diaria.barraSegundaID = barraSegunda.id;
                                diaria.barraSegundaSerie = barraSegunda.noSerie;
                            }

                        }


                        if (diaria.martilloID != 0)
                        {
                            tblB_PiezaBarrenadora culata = tablaPiezas.FirstOrDefault(x => x.id == diaria.culataID);
                            if (culata != null)
                            {
                                culata.horasTrabajadas += diaria.horasTrabajadas;
                                diaria.culataID = culata.id;
                                diaria.culataSerie = culata.noSerie;
                            }
                            else
                            {
                                culata = _context.tblB_PiezaBarrenadora.FirstOrDefault(x => x.noSerie == diaria.culataSerie && x.barrenadoraID == barrenadora && areaCuenta == x.areaCuenta);
                                if (culata != null)
                                {
                                    culata.horasTrabajadas += diaria.horasTrabajadas;
                                    diaria.culataID = culata.id;
                                    diaria.culataSerie = culata.noSerie;
                                }

                            }

                            tblB_PiezaBarrenadora cilindro = tablaPiezas.FirstOrDefault(x => x.id == diaria.cilindroID);
                            if (cilindro != null)
                            {
                                cilindro.horasTrabajadas += diaria.horasTrabajadas;
                                diaria.cilindroID = cilindro.id;
                                diaria.cilindroSerie = cilindro.noSerie;
                            }
                            else
                            {
                                cilindro = _context.tblB_PiezaBarrenadora.FirstOrDefault(x => x.noSerie == diaria.cilindroSerie && x.barrenadoraID == barrenadora && areaCuenta == x.areaCuenta);
                                if (cilindro != null)
                                {
                                    cilindro.horasTrabajadas += diaria.horasTrabajadas;
                                    diaria.cilindroID = cilindro.id;
                                    diaria.cilindroSerie = cilindro.noSerie;
                                }
                            }
                        }

                        tblB_PiezaBarrenadora zanco = tablaPiezas.FirstOrDefault(x => x.id == diaria.zancoID && areaCuenta == x.areaCuenta);
                        if (zanco != null)
                        {
                            if (zanco != null)
                            {
                                zanco.horasTrabajadas += diaria.horasTrabajadas;
                                diaria.zancoID = zanco.id;
                                diaria.zancoSerie = zanco.noSerie;
                            }
                            else
                            {
                                zanco = _context.tblB_PiezaBarrenadora.FirstOrDefault(x => x.noSerie == diaria.zancoSerie && x.barrenadoraID == barrenadora && areaCuenta == x.areaCuenta);
                                zanco.horasTrabajadas += diaria.horasTrabajadas;
                                diaria.zancoID = zanco.id;
                                diaria.zancoSerie = zanco.noSerie;
                            }
                        }
                        diaria.fechaCreacion = DateTime.Now;
                        diaria.usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;
                        if (diaria.id == 0)
                        {
                            _context.tblB_CapturaDiaria.Add(diaria);
                            _context.SaveChanges();
                        }
                        else
                        {
                            var capturaDiariaActual = _context.tblB_CapturaDiaria.FirstOrDefault(f => f.id == diaria.id);

                            capturaDiariaActual.areaCuenta = diaria.areaCuenta;
                            capturaDiariaActual.banco = diaria.banco;
                            capturaDiariaActual.barrenos = diaria.barrenos;
                            capturaDiariaActual.bordo = diaria.bordo;
                            capturaDiariaActual.claveAyudante = diaria.claveAyudante;
                            capturaDiariaActual.claveOperador = diaria.claveOperador;
                            capturaDiariaActual.densidadMaterial = diaria.densidadMaterial;
                            capturaDiariaActual.detalles = diaria.detalles;
                            capturaDiariaActual.espaciamiento = diaria.espaciamiento;
                            capturaDiariaActual.fsrAyudante = diaria.fsrAyudante;
                            capturaDiariaActual.fsrOperador = diaria.fsrOperador;
                            capturaDiariaActual.horasTrabajadas = diaria.horasTrabajadas;
                            capturaDiariaActual.horometroFinal = diaria.horometroFinal;
                            capturaDiariaActual.precioAyudante = diaria.precioAyudante;
                            capturaDiariaActual.precioOperador = diaria.precioOperador;
                            capturaDiariaActual.profundidad = diaria.profundidad;
                            capturaDiariaActual.rehabilitacion = diaria.rehabilitacion;
                            capturaDiariaActual.subBarreno = diaria.subBarreno;
                            capturaDiariaActual.tipoBarreno = diaria.tipoBarreno;
                            capturaDiariaActual.tipoCaptura = diaria.tipoCaptura;
                            capturaDiariaActual.totalAyudante = diaria.totalAyudante;
                            capturaDiariaActual.totalOperador = diaria.totalOperador;
                            capturaDiariaActual.turno = diaria.turno;
                            capturaDiariaActual.usuarioCreadorID = diaria.usuarioCreadorID;


                            //Piezas Montadas Actuales

                            capturaDiariaActual.brocaID = diaria.brocaID;
                            capturaDiariaActual.brocaSerie = diaria.brocaSerie;
                            capturaDiariaActual.barraID = diaria.barraID;
                            capturaDiariaActual.barraSerie = diaria.barraSerie;
                            capturaDiariaActual.barraSegundaID = diaria.barraSegundaID;
                            capturaDiariaActual.barraSegundaSerie = diaria.barraSegundaSerie;
                            capturaDiariaActual.martilloID = diaria.martilloID;
                            capturaDiariaActual.martilloSerie = diaria.martilloSerie;
                            capturaDiariaActual.cilindroID = diaria.cilindroID;
                            capturaDiariaActual.cilindroSerie = diaria.cilindroSerie;
                            capturaDiariaActual.culataID = diaria.culataID;
                            capturaDiariaActual.culataSerie = diaria.culataSerie;
                            capturaDiariaActual.portabitID = diaria.portabitID;
                            capturaDiariaActual.portabitSerie = diaria.portabitSerie;
                            capturaDiariaActual.zancoID = diaria.zancoID;
                            capturaDiariaActual.zancoSerie = diaria.zancoSerie;
                            _context.SaveChanges();

                        }
                    }
                    resultado.Add(SUCCESS, true);
                    dbTransaction.Commit();

                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, nombreControlador, "GuardarCapturaDiaria", e, AccionEnum.ACTUALIZAR, 0, listaPzas);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error al guardar captura diaria.");
                }
                return resultado;
            }
        }
        private dynamic consultaCheckProductivo(string consulta)
        {
            if (productivo)
            {
                return _contextEnkontrol.WhereComprasOrigen(consulta);
            }
            else
            {
                return _contextEnkontrolPrueba.Where(consulta);
            }
        }
        private decimal getCostoPromedioKardex(List<int> almacenes, int insumo)
        {
            int ano = DateTime.Now.Year;
            foreach (var almacen in almacenes)
            {
                var conexion = _contextEnkontrol.BaseDatosProductivoDesdeEmpresa(EmpresaEnum.Construplan);

                var odbc = new OdbcConsultaDTO();
                odbc.consulta = string.Format(@"SELECT AVG(precio) as precio
                                            FROM si_movimientos_det 
                                            WHERE 
                                                insumo = {0} 
                                                AND tipo_mov = 1 
                                                AND almacen = {1} 
                                                AND numero in 
                                                    (SELECT numero 
                                                    FROM si_movimientos 
                                                    WHERE 
                                                        almacen = {2}
                                                        AND ano >= {3}
                                                        )", insumo, almacen, almacen, ano);

                var promedioEnkontrol = _contextEnkontrol.Select<dynamic>(conexion, odbc);
                decimal sumPrecio = 0;
                if (promedioEnkontrol[0].precio != null)
                {
                    sumPrecio = promedioEnkontrol[0].precio;
                }
                if (sumPrecio > 0)
                {
                    return sumPrecio;
                }
//                var promedioEnkontrol = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanProd,
//                    new OdbcConsultaDTO()
//                    {
//                        consulta = @"
//                            SELECT 
//                                AVG(precio) as precio
//                            FROM si_movimientos_det 
//                            WHERE 
//                                insumo = ? 
//                                AND tipo_mov = 1 
//                                AND almacen = ? 
//                                AND numero in 
//                                    (SELECT numero 
//                                    FROM si_movimientos 
//                                    WHERE 
//                                        almacen = ? 
//                                        AND ano >= ?)",
//                        parametros = new List<OdbcParameterDTO>() {
//                            new OdbcParameterDTO() { nombre = "insumo", tipo = OdbcType.Numeric, valor = insumo },
//                            new OdbcParameterDTO() { nombre = "almacen", tipo = OdbcType.Numeric, valor = almacen },
//                            new OdbcParameterDTO() { nombre = "almacen", tipo = OdbcType.Numeric, valor = almacen },
//                            new OdbcParameterDTO() { nombre = "ano", tipo = OdbcType.Numeric, valor = DateTime.Now.Year }
//                        }
//                    }
//                );

                //if (Convert.ToDecimal(promedioEnkontrol[0].precio) > 0)
                //    return promedioEnkontrol[0].precio;
            }
            return 0;
        }

        public string getCCDescByAC(string AC) 
        {
            string data = AC;
            var cc = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == AC);
            if (cc != null) data = AC + " " + cc.descripcion;
            return data;
        }
    }
}
