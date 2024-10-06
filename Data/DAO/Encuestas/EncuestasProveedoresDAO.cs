using Core.DAO.Encuestas;
using Core.DTO;
using Core.DTO.Administracion.Seguridad.Capacitacion;
using Core.DTO.COMPRAS;
using Core.DTO.Encuestas;
using Core.DTO.Encuestas.Proveedores;
using Core.DTO.Encuestas.Proveedores.Reportes;
using Core.DTO.Enkontrol.Tablas.CC;
using Core.DTO.Principal.Generales;
using Core.DTO.Utils.Data;
using Core.Entity.Encuestas;
using Core.Entity.Encuestas.Proveedores;
using Core.Entity.Enkontrol.Compras.OrdenCompra;
using Core.Entity.Principal.Usuarios;
using Core.Enum.Encuesta;
using Core.Enum.Encuesta.Proveedores;
using Core.Enum.Multiempresa;
using Data.DAO.Enkontrol.General.Proveedor;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Data.Factory.Encuestas;
using Data.Factory.Enkontrol.General.CC;
using Data.Factory.Enkontrol.General.Proveedor;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Encuestas
{
    public class EncuestasProveedoresDAO : GenericDAO<tblEN_Encuesta>, IEncuestasProveedorDAO
    {
        ProveedorFactoryService _proveedorFS = new ProveedorFactoryService();

        public Respuesta ResponderEncuestaTop20(int tipoEncuesta, int encuestaID, int numeroProveedor)
        {
            var r = new Respuesta();

            try
            {
                var encuesta = getEncuesta(encuestaID);
                var preguntas = getPreguntasProveedores(encuestaID);
                var proveedor = InfoProveedor(numeroProveedor);
                var evaluador = vSesiones.sesionUsuarioDTO.nombre + " " + vSesiones.sesionUsuarioDTO.apellidoPaterno + " " + vSesiones.sesionUsuarioDTO.apellidoMaterno;

                dynamic resultado = new
                {
                    encuesta = new
                    {
                        id = encuesta.id,
                        titulo = encuesta.titulo,
                        descripcion = encuesta.descripcion,
                        tipoEncuesta = encuesta.tipoEncuesta
                    },
                    proveedor = proveedor,
                    preguntas = preguntas.Select(m => new
                    {
                        m.encuestaID,
                        m.estatus,
                        m.id,
                        m.orden,
                        m.pregunta,
                        m.tipo,
                        m.descripcionTipo,
                        m.ponderacion
                    }).ToList(),
                    evaluador = evaluador
                };

                r.Success = true;
                r.Message = "Ok";
                r.Value = resultado;
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return r;
        }

        public Respuesta ResponderEncuestaTop20Peru(int tipoEncuesta, int encuestaID, string numeroProveedor)
        {
            var r = new Respuesta();

            try
            {
                var encuesta = getEncuesta(encuestaID);
                var preguntas = getPreguntasProveedores(encuestaID);
                var proveedor = InfoProveedorPeru(numeroProveedor);
                var evaluador = vSesiones.sesionUsuarioDTO.nombre + " " + vSesiones.sesionUsuarioDTO.apellidoPaterno + " " + vSesiones.sesionUsuarioDTO.apellidoMaterno;

                dynamic resultado = new
                {
                    encuesta = new
                    {
                        id = encuesta.id,
                        titulo = encuesta.titulo,
                        descripcion = encuesta.descripcion,
                        tipoEncuesta = encuesta.tipoEncuesta
                    },
                    proveedor = proveedor,
                    preguntas = preguntas.Select(m => new
                    {
                        m.encuestaID,
                        m.estatus,
                        m.id,
                        m.orden,
                        m.pregunta,
                        m.tipo,
                        m.descripcionTipo,
                        m.ponderacion
                    }).ToList(),
                    evaluador = evaluador
                };

                r.Success = true;
                r.Message = "Ok";
                r.Value = resultado;
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return r;
        }

        private ProveedorDTO InfoProveedor(int numeroProveedor)
        {
            var odbcProveedor = new OdbcConsultaDTO();
            odbcProveedor.consulta =
                @"SELECT TOP 1
                    prov.numpro AS numero,
                    prov.nombre AS nombre,
                    prov.nomcorto AS nombreCorto,
                    prov.ciudad AS ubicacion,
                    prov.moneda AS tipoMoneda,
                    prov.tipo_prov AS tipo,
                    '1988-01-01' AS fechaAntiguedad
                  FROM
                    sp_proveedores AS prov                  
                  WHERE
                    prov.numpro = ?";
            odbcProveedor.parametros.Add(new OdbcParameterDTO
            {
                nombre = "numpro",
                tipo = System.Data.Odbc.OdbcType.Int,
                valor = numeroProveedor
            });

            return _contextEnkontrol.Select<ProveedorDTO>(EnkontrolAmbienteEnum.Prod, odbcProveedor).FirstOrDefault();
        }

        private ProveedorPeruDTO InfoProveedorPeru(string numeroProveedor)
        {
            return _context.tblCom_MAEPROV.Where(x => x.PRVCCODIGO == numeroProveedor && x.registroActivo && x.Autorizado && x.statusAutorizacion)
                .Select(x => new ProveedorPeruDTO
                {
                    Numero = x.PRVCCODIGO,
                    Nombre = x.PRVCNOMBRE,
                    NombreCorto = "",
                    Ubicacion = x.PRVCLOCALI,
                    TipoMoneda = 1,
                    FechaAntiguedad = new DateTime(1988, 1, 1)
                }).FirstOrDefault();
        }

        public Respuesta GetProveedoresTop20(int idUsuario, int tipoEncuestaId, bool desdeCompras = false)
        {
            var r = new Respuesta();

            switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
            {
                case EmpresaEnum.Peru:
                    {
                        var proveedores = _context.tblCom_MAEPROV.Where(x => x.registroActivo && x.Autorizado && x.statusAutorizacion).Select(x => new ComboDTO
                        {
                            Text = x.PRVCNOMBRE,
                            Value = x.PRVCCODIGO,
                            Prefijo = ""
                        }).ToList();

                        r.Success = true;
                        r.Message = "Ok";
                        r.Value = proveedores;

                        return r;
                    }
                    break;
            }
            /*
             * var optionDTO = proveedor.Select(m => new ComboDTO
                                {
                                    Text = ++contador + ". [N] " + m.proveedorNombre,
                                    Value = m.proveedorNumero.ToString(),
                                    Prefijo = m.proveedorNombreCorto
                                }).First();
                                top20.Add(optionDTO);
             * */

            var fecha = DateTime.Now.AddMonths(-1);
            var añoTop20 = fecha.Year;
            var mesTop20 = fecha.Month;
            var ultimoDiaMesTop20 = DateTime.DaysInMonth(añoTop20, mesTop20);

            var top20 = new List<ComboDTO>();

            var tipoEncuestasIds = new List<int>();

            try
            {
                var tipoEncuestasProv = new List<tblEN_TipoEncuestaProveedor>();

                if (desdeCompras)
                {
                    tipoEncuestasProv = _context.tblEN_TipoEncuestaProveedor.Where(x => x.esActivo).ToList();
                    tipoEncuestasIds.AddRange(tipoEncuestasProv.Select(m => m.id).ToList());
                }
                else
                {
                    tipoEncuestasIds.Add(tipoEncuestaId);
                }

                foreach (var _tipoEncuesta in tipoEncuestasIds)
                {
                    if (_tipoEncuesta == 2) //Encuestas de servicio se realizan cuatro meses después
                    {
                        fecha = DateTime.Now.AddMonths(-4);
                        añoTop20 = fecha.Year;
                        mesTop20 = fecha.Month;
                        ultimoDiaMesTop20 = DateTime.DaysInMonth(añoTop20, mesTop20);
                    }
                    else
                    {
                        fecha = DateTime.Now.AddMonths(-1);
                        añoTop20 = fecha.Year;
                        mesTop20 = fecha.Month;
                        ultimoDiaMesTop20 = DateTime.DaysInMonth(añoTop20, mesTop20);
                    }

                    //Usuario excepción
                    var existeExcepcion = _context.tblEN_UsuariosExcepcionTop20.Any(a => a.IdUsuario == idUsuario && a.Estatus);

                    if (desdeCompras && existeExcepcion)
                    {
                        r.Success = true;
                        r.Message = "Ok";
                        r.Value = false;

                        return r;
                    }

                    tblEN_Top20Proveedores existeSinResponder = null;

                    if (existeExcepcion)
                    {
                        var topGenerado = _context.tblEN_Top20Proveedores.Any
                            (a =>
                                a.UsuarioId == idUsuario &&
                                a.TipoEncuestaId == _tipoEncuesta &&
                                a.FechaTop20 == new DateTime(añoTop20, mesTop20, 01) &&
                                a.Estatus
                            );

                        if (topGenerado && desdeCompras)
                        {
                            r.Success = true;
                            r.Message = "Ok";
                            r.Value = false;

                            continue;
                        }
                    }
                    else
                    {
                        // Busca si hay algun proveedor del top20 sin encuesta realizada
                        existeSinResponder = _context.tblEN_Top20Proveedores.FirstOrDefault
                                (f =>
                                    f.UsuarioId == idUsuario &&
                                    f.TipoEncuestaId == _tipoEncuesta &&
                                    f.Estatus &&
                                    f.CantidadEvaluaciones == 0
                                );
                    }
                    //

                    // Si encontro un proveedor sin encuesta contestada se toma la fecha de cuando ese proveedor entro al top20
                    if (existeSinResponder != null)
                    {
                        añoTop20 = existeSinResponder.FechaTop20.Year;
                        mesTop20 = existeSinResponder.FechaTop20.Month;
                        ultimoDiaMesTop20 = DateTime.DaysInMonth(añoTop20, mesTop20);

                        if (desdeCompras)
                        {
                            var mesesDif = (DateTime.Now - new DateTime(añoTop20, mesTop20, ultimoDiaMesTop20)).Days;
                            //var mesesDif = ((DateTime.Now.Year - añoTop20) * 12) + (DateTime.Now.Month - mesTop20);

                            r.Success = true;
                            r.Message = "Ok";

                            if (mesesDif > 3)
                            {
                                r.Message = "Tienes encuestas de proveedores sin responder en: " + tipoEncuestasProv.First(f => f.id == _tipoEncuesta).descripcion;
                                r.Value = true;
                                return r;
                            }
                            else
                            {
                                r.Value = false;
                                continue;
                            }
                        }
                    }

                    // Se busca en sigoplan el top20 de proveedores de la encuesta y usuario en cuestion segun la fecha obtenida anteriormente
                    var proveedoresRegistrador = _context.tblEN_Top20Proveedores.Where
                        (w =>
                            w.UsuarioId == idUsuario &&
                            w.TipoEncuestaId == _tipoEncuesta &&
                            w.Estatus &&
                            w.FechaTop20 == new DateTime(añoTop20, mesTop20, 01)
                        ).ToList();

                    // Si encontro informacion del top 20 en sigoplan se arma el top20 para mostrar en el comboBox con la informacion obtenida,
                    // si no encontro informacion se realizan las consultas para armar el top20
                    if (proveedoresRegistrador.Count > 0)
                    {
                        if (desdeCompras)
                        {
                            r.Success = true;
                            r.Message = "Ok";
                            r.Value = false;
                        }

                        int contador = 0;

                        foreach (var proveedor in proveedoresRegistrador.Where(w => w.CantidadEvaluaciones == 0).GroupBy(g => g.Numero))
                        {
                            var optionDTO = new ComboDTO
                            {
                                Text = ++contador + /*". [" +
                                    EnumExtensions.GetDescription((TipoTop20Enum)proveedor.TipoTop20Id) + "] "*/ ". " +
                                    proveedor.First().Nombre + " (" + proveedor.Count() + ")",
                                Value = proveedor.First().Numero.ToString()
                            };
                            top20.Add(optionDTO);
                        }

                        r.Success = true;
                        r.Message = "Ok";
                        r.Value = top20;

                        return r;
                    }

                    var usuarioSIGONTROL = _context.tblP_Usuario_Enkontrol.FirstOrDefault(f => f.idUsuario == idUsuario);
                    if (usuarioSIGONTROL == null)
                    {
                        r.Message = "Favor de ingresar al módulo de compras o almacén y realizar el registro único de información de empleado";
                        r.Success = false;
                        //r.Message = "";
                    }
                    else
                    {
                        var odbcConsulta = new OdbcConsultaDTO();

                        var registroTop20 = new List<tblEN_Top20Proveedores>();

                        IQueryable<OrdenCompraDTO> busquedaProveedores = Enumerable.Empty<OrdenCompraDTO>().AsQueryable();

                        IQueryable<OrdenCompraDTO> comprasUnicas = Enumerable.Empty<OrdenCompraDTO>().AsQueryable();

                        var gruposNoInv = _context.tblEN_GrupoInsumo.Where(w => w.Estatus && w.TipoEncuestaId == _tipoEncuesta).ToList();

                        var parametroFechaIni = new OdbcParameterDTO
                        {
                            nombre = "fecha",
                            tipo = System.Data.Odbc.OdbcType.Date,
                            valor = new DateTime(añoTop20, mesTop20, 1)
                        };
                        var parametroFechaFin = new OdbcParameterDTO
                        {
                            nombre = "fecha",
                            tipo = System.Data.Odbc.OdbcType.Date,
                            valor = new DateTime(añoTop20, mesTop20, ultimoDiaMesTop20)
                        };
                        var parametroComprador = new OdbcParameterDTO
                        {
                            nombre = "comprador",
                            tipo = System.Data.Odbc.OdbcType.Int,
                            valor = usuarioSIGONTROL.empleado
                        };

                        var usuariosRequi = _tipoEncuesta == (int)tiposEncuestasEnum.EvaluaciónContinuaDeProveedores ? _context.tblEN_UsuarioInsumo.FirstOrDefault(f => f.TipoEncuestaId == _tipoEncuesta && f.Estatus && f.UsuarioId == vSesiones.sesionUsuarioDTO.id) : (tblEN_UsuarioInsumo)null;

                        switch ((tiposEncuestasEnum)_tipoEncuesta)
                        {
                            case tiposEncuestasEnum.EvaluaciónContinuaDeProveedores:
                                #region Evaluación continua de proveedores

                                if (usuariosRequi != null)
                                {
                                    odbcConsulta.consulta = string.Format
                                        (
                                            @"SELECT
                                                orden.cc,
                                                orden.numero,
                                                almacen.fecha,
                                                orden.proveedor AS proveedorNumero,
                                                pro.nombre AS proveedorNombre,
                                                pro.nomcorto AS proveedorNombreCorto,
                                                orden.moneda,
                                                orden.tipo_cambio AS tipoCambio,
                                                (orden.total * orden.tipo_cambio) AS total
                                              FROM
                                                so_orden_compra AS orden
                                              INNER JOIN sp_proveedores AS pro ON
                                                orden.proveedor = pro.numpro
                                              INNER JOIN si_movimientos AS almacen ON
                                                orden.cc = almacen.cc AND
                                                orden.numero = almacen.orden_ct AND
                                                almacen.tipo_mov = 1 AND
                                                almacen.fecha >= ? AND
                                                almacen.fecha <= ?
                                              INNER JOIN so_orden_compra_det AS orden_det ON
                                                orden.cc = orden_det.cc AND
                                                orden.numero = orden_det.numero
                                              INNER JOIN so_requisicion AS requi ON
                                                requi.cc = orden_det.cc AND
                                                requi.numero = orden_det.num_requisicion
                                              WHERE
                                                orden.ST_OC = 'A' AND
                                                requi.solicito = ?"
                                        );
                                    odbcConsulta.parametros.Clear();
                                    odbcConsulta.parametros.Add(parametroFechaIni);
                                    odbcConsulta.parametros.Add(parametroFechaFin);
                                    odbcConsulta.parametros.Add(parametroComprador);

                                    IQueryable<OrdenCompraDTO> comprasInv = _contextEnkontrol.Select<OrdenCompraDTO>(EnkontrolAmbienteEnum.Prod, odbcConsulta).AsQueryable();

                                    IQueryable<OrdenCompraDTO> comprasNoInv = null;

                                    if (gruposNoInv.Count > 0)
                                    {
                                        var listaGrupos = new List<string>();
                                        listaGrupos.AddRange
                                            (
                                                gruposNoInv.Select
                                                    (m =>
                                                        m.TipoInsumo.ToString() +
                                                        (m.GrupoInsumo >= 10 ?
                                                            m.GrupoInsumo.ToString() :
                                                            "0" + m.GrupoInsumo.ToString()
                                                        )
                                                    )
                                            );

                                        odbcConsulta.consulta = string.Format
                                            (
                                                @"SELECT
                                                    orden.cc,
                                                    orden.numero,
                                                    almacen.fecha,
                                                    orden.proveedor AS proveedorNumero,
                                                    pro.nombre AS proveedorNombre,
                                                    pro.nomcorto AS proveedorNombreCorto,
                                                    orden.moneda,
                                                    orden.tipo_cambio AS tipoCambio,
                                                    (orden.total * orden.tipo_cambio) as total
                                                  FROM
                                                    so_orden_compra AS orden
                                                  INNER JOIN sp_proveedores AS pro ON
                                                    orden.proveedor = pro.numpro
                                                  INNER JOIN so_movimientos_noinv AS almacen ON
                                                    orden.cc = almacen.cc AND
                                                    orden.numero = almacen.orden_ct AND
                                                    almacen.tipo_mov = 1 AND
                                                    almacen.fecha >= ? AND
                                                    almacen.fecha <= ?
                                                  INNER JOIN so_orden_compra_det AS det ON
                                                    orden.cc = det.cc AND
                                                    orden.numero = det.numero AND
                                                    LEFT(det.insumo, 3) in {0}
                                                  INNER JOIN so_requisicion AS requi ON
                                                    requi.cc = det.cc AND
                                                    requi.numero = det.num_requisicion
                                                  WHERE
                                                    requi.solicito = ? AND
                                                    orden.ST_OC = 'A'",
                                                    listaGrupos.ToParamInValue()
                                            );
                                        odbcConsulta.parametros.Clear();
                                        odbcConsulta.parametros.Add(parametroFechaIni);
                                        odbcConsulta.parametros.Add(parametroFechaFin);

                                        var parametrosGrupos = listaGrupos.Select(m => new OdbcParameterDTO
                                        {
                                            nombre = "grupo_insumo",
                                            tipo = System.Data.Odbc.OdbcType.Int,
                                            valor = int.Parse(m)
                                        }).ToList();

                                        odbcConsulta.parametros.AddRange(parametrosGrupos);
                                        odbcConsulta.parametros.Add(parametroComprador);

                                        comprasNoInv = _contextEnkontrol.Select<OrdenCompraDTO>(EnkontrolAmbienteEnum.Prod, odbcConsulta).AsQueryable();
                                    }

                                    busquedaProveedores = comprasInv;
                                    busquedaProveedores = busquedaProveedores.Concat(comprasNoInv);
                                }
                                else
                                {
                                    odbcConsulta.consulta =
                                    @"SELECT
                                    orden.cc,
                                    orden.numero,
                                    almacen.fecha,
                                    orden.proveedor AS proveedorNumero,
                                    pro.nombre AS proveedorNombre,
                                    pro.nomcorto AS proveedorNombreCorto,
                                    orden.moneda,
                                    orden.tipo_cambio AS tipoCambio,
                                    (orden.total * orden.tipo_cambio) AS total
                                  FROM
                                    so_orden_compra AS orden
                                  INNER JOIN sp_proveedores AS pro ON
                                    orden.proveedor = pro.numpro
                                  INNER JOIN si_movimientos AS almacen ON
                                    orden.cc = almacen.cc AND
                                    orden.numero = almacen.orden_ct AND
                                    almacen.tipo_mov = 1 AND
                                    almacen.fecha >= ? AND
                                    almacen.fecha <= ?
                                  WHERE
                                    orden.comprador = ? AND
                                    orden.ST_OC = 'A'";
                                    odbcConsulta.parametros.Add(parametroFechaIni);
                                    odbcConsulta.parametros.Add(parametroFechaFin);
                                    odbcConsulta.parametros.Add(parametroComprador);

                                    IQueryable<OrdenCompraDTO> comprasInv = _contextEnkontrol.Select<OrdenCompraDTO>(EnkontrolAmbienteEnum.Prod, odbcConsulta).AsQueryable();

                                    odbcConsulta.consulta =
                                        @"SELECT DISTINCT
                                    orden.proveedor AS proveedorNumero,
                                    CONVERT(date, STR(YEAR(almacen.fecha)) + '-' + STR(MONTH(almacen.fecha)) + '-01') AS fecha
                                  FROM
                                    so_orden_compra AS orden
                                  INNER JOIN sp_proveedores AS pro ON
                                    orden.proveedor = pro.numpro
                                  INNER JOIN si_movimientos AS almacen ON
                                    orden.cc = almacen.cc AND
                                    orden.numero = almacen.orden_ct AND
                                    almacen.tipo_mov = 1 AND
                                    almacen.fecha < ?
                                  WHERE
                                    orden.comprador = ? AND
                                    orden.ST_OC = 'A'";
                                    odbcConsulta.parametros.Clear();
                                    odbcConsulta.parametros.Add(parametroFechaIni);
                                    odbcConsulta.parametros.Add(parametroComprador);

                                    comprasUnicas = _contextEnkontrol.Select<OrdenCompraDTO>(EnkontrolAmbienteEnum.Prod, odbcConsulta).AsQueryable();

                                    IQueryable<OrdenCompraDTO> comprasNoInv = null;

                                    if (gruposNoInv.Count > 0)
                                    {
                                        var listaGrupos = new List<string>();
                                        listaGrupos.AddRange
                                            (
                                                gruposNoInv.Select
                                                    (m =>
                                                        m.TipoInsumo.ToString() +
                                                        (m.GrupoInsumo >= 10 ?
                                                            m.GrupoInsumo.ToString() :
                                                            "0" + m.GrupoInsumo.ToString()
                                                        )
                                                    )
                                            );

                                        odbcConsulta.consulta = string.Format
                                            (
                                                @"SELECT
                                            orden.cc,
                                            orden.numero,
                                            almacen.fecha,
                                            orden.proveedor AS proveedorNumero,
                                            pro.nombre AS proveedorNombre,
                                            pro.nomcorto AS proveedorNombreCorto,
                                            orden.moneda,
                                            orden.tipo_cambio AS tipoCambio,
                                            (orden.total * orden.tipo_cambio) as total
                                          FROM
                                            so_orden_compra AS orden
                                          INNER JOIN sp_proveedores AS pro ON
                                            orden.proveedor = pro.numpro
                                          INNER JOIN so_movimientos_noinv AS almacen ON
                                            orden.cc = almacen.cc AND
                                            orden.numero = almacen.orden_ct AND
                                            almacen.tipo_mov = 1 AND
                                            almacen.fecha >= ? AND
                                            almacen.fecha <= ?
                                          INNER JOIN so_orden_compra_det AS det ON
                                            orden.cc = det.cc AND
                                            orden.numero = det.numero AND
                                            LEFT(det.insumo, 3) in {0}
                                          WHERE
                                            orden.comprador = ? AND
                                            orden.ST_OC = 'A'",
                                                listaGrupos.ToParamInValue()
                                            );
                                        odbcConsulta.parametros.Clear();
                                        odbcConsulta.parametros.Add(parametroFechaIni);
                                        odbcConsulta.parametros.Add(parametroFechaFin);

                                        var parametrosGrupos = listaGrupos.Select(m => new OdbcParameterDTO
                                        {
                                            nombre = "grupo_insumo",
                                            tipo = System.Data.Odbc.OdbcType.Int,
                                            valor = int.Parse(m)
                                        }).ToList();

                                        odbcConsulta.parametros.AddRange(parametrosGrupos);
                                        odbcConsulta.parametros.Add(parametroComprador);

                                        comprasNoInv = _contextEnkontrol.Select<OrdenCompraDTO>(EnkontrolAmbienteEnum.Prod, odbcConsulta).AsQueryable();

                                        odbcConsulta.consulta = string.Format
                                            (
                                                @"SELECT DISTINCT
                                            orden.proveedor AS proveedorNumero,
                                            CONVERT(date, STR(YEAR(almacen.fecha)) + '-' + STR(MONTH(almacen.fecha)) + '-01') AS fecha
                                          FROM
                                            so_orden_compra AS orden
                                          INNER JOIN sp_proveedores AS pro ON
                                            orden.proveedor = pro.numpro
                                          INNER JOIN so_movimientos_noinv AS almacen ON
                                            orden.cc = almacen.cc AND
                                            orden.numero = almacen.orden_ct AND
                                            almacen.tipo_mov = 1
                                          INNER JOIN so_orden_compra_det AS det ON
                                            orden.cc = det.cc AND
                                            orden.numero = det.numero AND
                                            LEFT(det.insumo, 3) in {0}
                                          WHERE
                                            orden.comprador = ? AND
                                            orden.ST_OC = 'A'",
                                              listaGrupos.ToParamInValue()
                                            );
                                        odbcConsulta.parametros.Clear();
                                        odbcConsulta.parametros.Add(parametroComprador);
                                        odbcConsulta.parametros.AddRange(parametrosGrupos);

                                        var comprasUnicasNoInv = _contextEnkontrol.Select<OrdenCompraDTO>(EnkontrolAmbienteEnum.Prod, odbcConsulta).AsQueryable();

                                        if (comprasUnicasNoInv != null)
                                        {
                                            comprasUnicas = comprasUnicas.Concat(comprasUnicasNoInv);
                                        }
                                    }

                                    if (comprasInv != null)
                                    {
                                        busquedaProveedores = comprasInv;
                                    }

                                    if (comprasNoInv != null)
                                    {
                                        busquedaProveedores = busquedaProveedores.Concat(comprasNoInv);
                                    }
                                }
                                #endregion
                                break;
                            case tiposEncuestasEnum.ProveedoresDeServicio:
                                #region Proveedores de servicio

                                if (gruposNoInv.Count > 0)
                                {
                                    var listaGrupos = new List<int>();
                                    var listaInsumos = new List<int>();
                                    listaGrupos.AddRange(gruposNoInv.Where(w => w.Familia).Select(m => (m.TipoInsumo * 100) + m.GrupoInsumo));
                                    listaInsumos.AddRange(gruposNoInv.Where(w => !w.Familia).Select(m => (((m.TipoInsumo * 100) + m.GrupoInsumo) * 10000) + m.Consecutivo.Value));
                                    var listaUsuariosRequi = _context.tblEN_UsuarioInsumo.FirstOrDefault(f => f.TipoEncuestaId == _tipoEncuesta && f.Estatus && f.UsuarioId == vSesiones.sesionUsuarioDTO.id);

                                    if (listaUsuariosRequi == null)
                                    {
                                        if (desdeCompras)
                                        {
                                            continue;
                                        }

                                        r.Message = "No estas registrado como usuario para realizar este tipo de encuestas";
                                        return r;
                                    }

                                    var parametrosGrupos = listaGrupos.Select(m => new OdbcParameterDTO
                                    {
                                        nombre = "grupo_insumo",
                                        tipo = System.Data.Odbc.OdbcType.Int,
                                        valor = m
                                    });
                                    var parametrosInsumos = listaInsumos.Select(m => new OdbcParameterDTO
                                    {
                                        nombre = "insumo",
                                        tipo = System.Data.Odbc.OdbcType.Int,
                                        valor = m
                                    });
                                    var parametrosUsuario = new OdbcParameterDTO
                                    {
                                        nombre = "insumo",
                                        tipo = System.Data.Odbc.OdbcType.Int,
                                        valor = listaUsuariosRequi.EmpleadoId
                                    };

                                    odbcConsulta.consulta = string.Format
                                    (
                                        @"SELECT
                                        orden.cc,
                                        orden.numero,
                                        orden.fecha,
                                        orden.proveedor AS proveedorNumero,
                                        pro.nombre AS proveedorNombre,
                                        pro.nomcorto AS proveedorNombreCorto,
                                        orden.moneda,
                                        orden.tipo_cambio AS tipoCambio,
                                        (orden.total * orden.tipo_cambio) AS total
                                      FROM
                                        so_orden_compra AS orden
                                      INNER JOIN so_orden_compra_det AS orden_det ON
                                        orden.cc = orden_det.cc AND
                                        orden.numero = orden_det.numero AND
                                        (
                                            LEFT(orden_det.insumo, 3) in {0} OR
                                            orden_det.insumo in {1}
                                        ) AND
                                        orden.fecha >= ? AND
                                        orden.fecha <= ?
                                      INNER JOIN so_requisicion AS requi ON
                                        requi.cc = orden_det.cc AND
                                        requi.numero = orden_det.num_requisicion
                                      INNER JOIN sp_proveedores AS pro ON
                                        orden.proveedor = pro.numpro
                                      WHERE
                                        orden.ST_OC = 'A' AND
                                        requi.solicito = ?",
                                        listaGrupos.ToParamInValue(),
                                        listaInsumos.ToParamInValue()
                                    );
                                    odbcConsulta.parametros.Clear();
                                    odbcConsulta.parametros.AddRange(parametrosGrupos);
                                    odbcConsulta.parametros.AddRange(parametrosInsumos);
                                    odbcConsulta.parametros.Add(parametroFechaIni);
                                    odbcConsulta.parametros.Add(parametroFechaFin);
                                    odbcConsulta.parametros.Add(parametrosUsuario);

                                    IQueryable<OrdenCompraDTO> compras = _contextEnkontrol.Select<OrdenCompraDTO>(EnkontrolAmbienteEnum.Prod, odbcConsulta).AsQueryable();

                                    if (compras != null)
                                    {
                                        busquedaProveedores = compras;
                                    }
                                }
                                else
                                {
                                    if (desdeCompras)
                                    {
                                        continue;
                                    }

                                    r.Message = "No se encontraron registrados los grupos de insumos para este tipo de encuesta";
                                    return r;
                                }
                                #endregion
                                break;
                            case tiposEncuestasEnum.ProveedoresDeConsignación:
                                #region Proveedores de consignación

                                var usuariosCC = _context.tblEN_UsuariosAC.Where(f => f.UsuarioInsumo.TipoEncuestaId == _tipoEncuesta && f.Estatus && f.UsuarioInsumo.Estatus && f.UsuarioInsumo.UsuarioId == vSesiones.sesionUsuarioDTO.id).ToList();
                                //var listaUsuarioCC = _context.tblEN_UsuariosAC.Where(f => f.UsuarioInsumo.TipoEncuestaId == _tipoEncuesta && f.Estatus && f.UsuarioInsumo.Estatus && f.UsuarioInsumo.UsuarioId == vSesiones.sesionUsuarioDTO.id).Select(m => m.AreaCuenta).ToList();
                                var listaUsuarioCC = usuariosCC.Select(x => x.AreaCuenta).ToList();

                                if (listaUsuarioCC.Count > 0)
                                {
                                    var usuarioCcId = usuariosCC.Select(x => x.UsuarioInsumoId).First();
                                    var proveedoresConsigna = _context.tblEN_UsuarioProveedorConsigna.Where(x => x.usuarioCcId == usuarioCcId).Select(x => x.proveedor).ToList();

                                    IQueryable<tblCom_OrdenCompra> orden = null;
                                    if (listaUsuarioCC.Any(a => a == "ALL"))
                                    {
                                        orden = _context.tblCom_OrdenCompra.Where
                                        (w =>
                                            w.fecha >= new DateTime(añoTop20, mesTop20, 01) &&
                                            w.fecha <= new DateTime(añoTop20, mesTop20, ultimoDiaMesTop20) &&
                                            w.ST_OC == "A" &&
                                            w.proveedor != null &&
                                            w.estatusRegistro
                                        ).AsQueryable();
                                    }
                                    else
                                    {
                                        orden = _context.tblCom_OrdenCompra.Where
                                        (w =>
                                            listaUsuarioCC.Contains(w.cc) &&
                                            w.fecha >= new DateTime(añoTop20, mesTop20, 01) &&
                                            w.fecha <= new DateTime(añoTop20, mesTop20, ultimoDiaMesTop20) &&
                                            w.ST_OC == "A" &&
                                            w.proveedor != null &&
                                            proveedoresConsigna.Contains(w.proveedor.Value) &&
                                            w.estatusRegistro
                                        ).AsQueryable();
                                    }
                                    var ordenDet = _context.tblCom_OrdenCompraDet.Where
                                        (w =>
                                            orden.Select(m => m.id).Contains(w.idOrdenCompra) &&
                                            w.estatusRegistro
                                        ).AsQueryable();
                                    var req = _context.tblCom_Req.Where
                                        (w =>
                                            ordenDet.Select(m => new { m.num_requisicion, m.cc }).Any(a => a.num_requisicion == w.numero && a.cc == w.cc) &&
                                            w.consigna != null &&
                                            w.consigna.Value &&
                                            w.estatusRegistro
                                        ).AsQueryable();
                                    var ordenDetReq = _context.tblCom_OrdenCompraDet.Where
                                        (w =>
                                            req.Select(m => new { m.numero, m.cc }).Any(a => a.numero == w.num_requisicion && a.cc == w.cc)
                                        ).AsQueryable();
                                    var consulta = _context.tblCom_OrdenCompra.Where(w => ordenDetReq.Select(m => m.idOrdenCompra).Contains(w.id) && w.proveedor != null && proveedoresConsigna.Contains(w.proveedor.Value)).ToList().Select(m => new OrdenCompraDTO
                                    {
                                        cc = m.cc,
                                        numero = m.numero,
                                        fecha = m.fecha,
                                        proveedorNumero = m.proveedor.Value,
                                        proveedorNombre = null,
                                        proveedorNombreCorto = null,
                                        moneda = m.moneda,
                                        tipoCambio = m.tipo_cambio,
                                        total = m.total * m.tipo_cambio
                                    }).ToList();

                                    if (consulta.Count == 0)
                                    {
                                        break;
                                    }

                                    var listaProveedores = consulta.Select(m => m.proveedorNumero).Distinct().ToList();

                                    odbcConsulta.consulta = string.Format("SELECT numpro AS Numero, nombre AS Nombre, nomcorto AS NombreCorto  FROM sp_proveedores WHERE numpro in {0}", listaProveedores.ToParamInValue());
                                    odbcConsulta.parametros.AddRange(listaProveedores.Select(m => new OdbcParameterDTO
                                    {
                                        nombre = "numpro",
                                        tipo = System.Data.Odbc.OdbcType.Int,
                                        valor = m
                                    }).ToList());

                                    var provInfo = _contextEnkontrol.Select<ProveedorDTO>(EnkontrolAmbienteEnum.Prod, odbcConsulta);

                                    foreach (var proveedores in consulta.GroupBy(g => g.proveedorNumero))
                                    {
                                        var info = provInfo.FirstOrDefault(f => f.Numero == proveedores.First().proveedorNumero);
                                        foreach (var item in proveedores)
                                        {
                                            item.proveedorNombre = info != null ? info.Nombre : null;
                                            item.proveedorNombreCorto = info != null ? info.NombreCorto : null;
                                        }
                                    }

                                    busquedaProveedores = consulta.AsQueryable();
                                }
                                else
                                {
                                    if (desdeCompras)
                                    {
                                        continue;
                                    }

                                    r.Message = "No se encontró información de obras de este usuario";
                                    return r;
                                }
                                #endregion
                                break;
                        }

                        var contador = 0;

                        // LOS 12 FRECUENTES
                        var numeroFrecuentes = usuariosRequi == null ? 12 : 6;
                        var topFrecuentes = busquedaProveedores.GroupBy(g => g.proveedorNumero).OrderByDescending(o => o.Count()).Take(numeroFrecuentes);
                        foreach (var proveedor in topFrecuentes)
                        {
                            var optionDTO = proveedor.Select(m => new ComboDTO
                            {
                                Text = ++contador + ". [C] " + m.proveedorNombre,
                                Value = m.proveedorNumero.ToString(),
                                Prefijo = m.proveedorNombreCorto
                            }).First();
                            top20.Add(optionDTO);

                            var regTop = proveedor.Select(m => new tblEN_Top20Proveedores
                            {
                                Numero = m.proveedorNumero,
                                Nombre = m.proveedorNombre,
                                NombreCorto = m.proveedorNombreCorto,
                                TipoTop20Id = (int)TipoTop20Enum.Cantidad,
                                TipoEncuestaId = _tipoEncuesta,
                                CantidadEvaluaciones = 0,
                                FechaTop20 = new DateTime(añoTop20, mesTop20, 01),
                                UsuarioId = idUsuario,
                                Estatus = true
                            }).First();
                            registroTop20.Add(regTop);
                        }

                        busquedaProveedores = busquedaProveedores.Where(w => !topFrecuentes.Select(m => m.Key).Contains(w.proveedorNumero));

                        // LOS 8 DE MAYOR MONTO
                        numeroFrecuentes = usuariosRequi == null ? 8 : 4;
                        var topMonto = busquedaProveedores.GroupBy(g => g.proveedorNumero).OrderByDescending(o => o.Sum(s => s.total)).Take(numeroFrecuentes);
                        foreach (var proveedor in topMonto)
                        {
                            var optionDTO = proveedor.Select(m => new ComboDTO
                            {
                                Text = ++contador + ". [M] " + m.proveedorNombre,
                                Value = m.proveedorNumero.ToString(),
                                Prefijo = m.proveedorNombreCorto
                            }).First();
                            top20.Add(optionDTO);

                            var regTop = proveedor.Select(m => new tblEN_Top20Proveedores
                            {
                                Numero = m.proveedorNumero,
                                Nombre = m.proveedorNombre,
                                NombreCorto = m.proveedorNombreCorto,
                                TipoTop20Id = (int)TipoTop20Enum.Monto,
                                TipoEncuestaId = _tipoEncuesta,
                                CantidadEvaluaciones = 0,
                                FechaTop20 = new DateTime(añoTop20, mesTop20, 01),
                                UsuarioId = idUsuario,
                                Estatus = true
                            }).First();
                            registroTop20.Add(regTop);
                        }

                        busquedaProveedores = busquedaProveedores.Where(w => !topMonto.Select(m => m.Key).Contains(w.proveedorNumero));

                        // LOS NUEVOS
                        foreach (var proveedor in busquedaProveedores.GroupBy(g => g.proveedorNumero))
                        {
                            if (!comprasUnicas.Select(m => m.proveedorNumero).Contains(proveedor.Key))
                            {
                                var optionDTO = proveedor.Select(m => new ComboDTO
                                {
                                    Text = ++contador + ". [N] " + m.proveedorNombre,
                                    Value = m.proveedorNumero.ToString(),
                                    Prefijo = m.proveedorNombreCorto
                                }).First();
                                top20.Add(optionDTO);

                                var regTop = proveedor.Select(m => new tblEN_Top20Proveedores
                                {
                                    Numero = m.proveedorNumero,
                                    Nombre = m.proveedorNombre,
                                    NombreCorto = m.proveedorNombreCorto,
                                    TipoTop20Id = (int)TipoTop20Enum.Nuevo,
                                    TipoEncuestaId = _tipoEncuesta,
                                    CantidadEvaluaciones = 0,
                                    FechaTop20 = new DateTime(añoTop20, mesTop20, 01),
                                    UsuarioId = idUsuario,
                                    Estatus = true
                                }).First();
                                registroTop20.Add(regTop);
                            }
                        }

                        if (registroTop20.Count > 0)
                        {
                            _context.tblEN_Top20Proveedores.AddRange(registroTop20);
                            _context.SaveChanges();

                            if (desdeCompras)
                            {
                                r.Success = true;
                                r.Message = "Ok";
                                r.Value = false;

                                continue;
                            }
                        }

                        r.Success = true;
                        r.Message = "Ok";
                        r.Value = top20;

                        if (desdeCompras)
                        {
                            r.Success = true;
                            r.Message = "Ok";
                            r.Value = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                r.Success = false;
                r.Message += ex.Message;

                if (desdeCompras)
                {
                    r.Value = false;
                }
            }
            return r;
        }

        public void saveEncuestaResultRequisiciones(List<tblEN_ResultadoProveedorRequisiciones> obj, tblEN_ResultadoProveedorRequisicionDet objSingle, string comentario)
        {
            IObjectSet<tblEN_ResultadoProveedorRequisicionDet> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblEN_ResultadoProveedorRequisicionDet>();

            var estrellas = _context.tblEN_Estrellas.ToList();
            obj.ForEach(e =>
            {
                e.porcentaje = estrellas.FirstOrDefault(x => x.estrellas == e.calificacion) != null ? estrellas.FirstOrDefault(x => x.estrellas == e.calificacion).maximo : 0;
            });

            objSingle.fechaRequisicion = DateTime.Now;
            if (objSingle == null) { throw new ArgumentNullException("Entity"); }

            objSingle.calificacion = (decimal)Math.Truncate(100 * (double)(obj.Where(y => y.porcentaje != null).Select(x => x.porcentaje).Sum() / obj.Count).Value) / 100;

            _objectSet.AddObject(objSingle);
            _context.SaveChanges();

            foreach (var item in obj)
            {
                item.encuestaFolioID = objSingle.id;
            }

            _context.tblEN_ResultadoProveedorRequisiciones.AddRange(obj);
            _context.SaveChanges();

        }

        public int saveEncuestasProveedores(tblEN_EncuestaProveedores obj, List<tblEN_PreguntasProveedores> listObj)
        {
            try
            {
                IObjectSet<tblEN_EncuestaProveedores> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblEN_EncuestaProveedores>();

                if (obj == null) { throw new ArgumentNullException("Entity"); }
                _objectSet.AddObject(obj);
                _context.SaveChanges();

                int encuestasID = obj.id;

                foreach (var item in listObj)
                {
                    item.encuestaID = encuestasID;
                }
                _context.tblEN_PreguntasProveedores.AddRange(listObj);
                _context.SaveChanges();
                return encuestasID;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public void updateEncuesta(tblEN_EncuestaProveedores obj, List<tblEN_PreguntasProveedores> listObj)
        {

            IObjectSet<tblEN_EncuestaProveedores> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblEN_EncuestaProveedores>();
            IObjectSet<tblEN_PreguntasProveedores> _objctListaPreguntas = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblEN_PreguntasProveedores>();

            tblEN_EncuestaProveedores existe = _context.tblEN_EncuestaProveedores.Find(obj.id);

            if (existe != null)
            {
                existe.descripcion = obj.descripcion;
                existe.estatus = obj.estatus;
                existe.tipoEncuesta = obj.tipoEncuesta;
                existe.titulo = obj.titulo;
                _context.SaveChanges();
            }

            List<tblEN_PreguntasProveedores> listaExiste = new List<tblEN_PreguntasProveedores>();

            foreach (var item in listObj)
            {

                if (item.id != 0)
                {
                    tblEN_PreguntasProveedores Pregunta = _context.tblEN_PreguntasProveedores.Find(item.id);
                    Pregunta.pregunta = item.pregunta;
                    Pregunta.estatus = item.estatus;
                    Pregunta.tipo = item.tipo;
                }
                else
                {
                    _context.tblEN_PreguntasProveedores.Add(item);
                }
            }
            _context.SaveChanges();

        }

        public List<tblEN_EncuestaProveedores> getEncuestasByTipo(int tipoEncuesta)
        {
            var result = _context.tblEN_EncuestaProveedores.Where(x => x.tipoEncuesta == tipoEncuesta).ToList();
            return result;
        }

        public List<PreguntasDTO> getPreguntasProveedores(int encuestaID)
        {
            var result = _context.tblEN_PreguntasProveedores.Where(x => x.encuestaID == encuestaID).ToList();
            return result.Select(x => new PreguntasDTO
             {
                 encuestaID = x.encuestaID,
                 estatus = x.estatus,
                 id = x.id,
                 orden = x.orden,
                 pregunta = x.pregunta,
                 tipo = x.tipo,
                 descripcionTipo = x.TipoPregunta.Descripcion,
                 visible = x.visible,
                 ponderacion = x.ponderacion
             }).ToList();
        }


        public tblEN_EncuestaProveedores getEncuesta(int id)
        {
            var result = _context.tblEN_EncuestaProveedores.FirstOrDefault(x => x.id == id);
            return result;
        }

        public EncuentaDTO getEncuestaResult(int id)
        {
            return null;
        }

        public List<EncuestaResultsDTO> getEncuestaResults(int id, DateTime fechaInicio, DateTime fechaFin)
        {
            return null;
        }

        public List<ResultProveedoresDTO> loadEncuestas(int estatus, DateTime fechaInicio, DateTime fechaFin)
        {

            List<ResultProveedoresDTO> res = new List<ResultProveedoresDTO>();
            var consulta = @"SELECT A.cc AS centrocostos,A.numero As numeroOC,A.fecha AS FechaOC,A.proveedor AS NumProveedor,A.comentarios AS Comentarios,b.nombre AS NombreProveedor,C.FechaAntiguedad, 0 AS estadoEncuesta 
                            FROM DBA.so_orden_compra A INNER JOIN
                            DBA.sp_proveedores B ON B.numpro =A.proveedor INNER JOIN
                            (SELECT MIN(bit_fecha) AS FechaAntiguedad,numpro AS numpro FROM DBA.px_bit_sp_proveedores group by (numpro)) C ON C.numpro = B.numpro 
                            WHERE estatus = 'T' AND A.fecha BETWEEN '" + fechaInicio.ToString("yyyyMMdd") + "' AND '" + fechaFin.ToString("yyyyMMdd") + "'" +
                            " ORDER BY A.fecha DESC";
            try
            {
                var rawDataEnkontrol = (List<ResultProveedoresDTO>)_contextEnkontrol.Where(consulta).ToObject<List<ResultProveedoresDTO>>();

                var rawCompare = from e in rawDataEnkontrol
                                 join oc in _context.tblEN_ResultadoProveedoresDet on e.numeroOC equals oc.numeroOC
                                 into encDet
                                 from oc in encDet.DefaultIfEmpty()
                                 select new { e, oc };

                foreach (var item in rawCompare)
                {
                    var datosSigoplan = item.e;
                    var datosEnkontrol = item.oc;

                    if (item.oc != null)
                    {
                        item.e.estadoEncuesta = false;
                        item.e.id = item.oc.id;

                    }
                    else
                    {
                        item.e.estadoEncuesta = true;
                        item.e.id = 0;

                    }

                    res.Add(item.e);

                }
                return res.ToList();
            }
            catch (Exception)
            {
                return new List<ResultProveedoresDTO>();
            }
        }

        public List<ResultProveedoresDTO> evaluacionesRespondidas(int estatus, DateTime fechaInicio, DateTime fechaFin, List<int> compradores)
        {

            var temp = _context.tblEN_ResultadoProveedores.Where(x => x.fecha >= fechaInicio && x.fecha <= fechaFin && x.tipoEncuesta == 1).GroupBy(x => x.encuestaFolioID).Select(y => y.Key).ToList();

            var temp2 = _context.tblEN_ResultadoProveedoresDet.Where(x => temp.Contains(x.id)).ToList();

            var resultado = (from rpd in temp2
                             where compradores.Contains(rpd.evaluadorID)
                             select new { rpd }
                             ).ToList().Select(x => new ResultProveedoresDTO
                             {
                                 centrocostos = x.rpd.centrocostos,
                                 comentarios = x.rpd.comentarios,
                                 encuestaID = x.rpd.encuestaID,
                                 estadoEncuesta = x.rpd.estadoEncuesta,
                                 fechaAntiguedad = x.rpd.fechaAntiguedad,
                                 fechaOC = x.rpd.fechaOC != null ? x.rpd.fechaOC.Value : x.rpd.fechaEvaluacion.Value,
                                 id = x.rpd.id,
                                 nombreEvaluador = getNombreEvaluador(x.rpd.evaluadorID),
                                 nombreProveedor = x.rpd.nombreProveedor,
                                 numeroOC = x.rpd.numeroOC,
                                 numProveedor = x.rpd.numProveedor,
                                 tipoMoneda = "",
                                 tipoProveedor = x.rpd.tipoProveedor,
                                 ubicacionProveedor = x.rpd.ubicacionProveedor,
                                 usuarioID = x.rpd.evaluadorID,
                                 ponderacion = _context.tblEN_ResultadoProveedores.Where(y => y.encuestaFolioID == x.rpd.id).ToList().Where(p => p.calificacion >= 3).Sum(c => c.pregunta.ponderacion)
                             }).ToList();

            return resultado;
        }


        private string getNombreEvaluador(int p)
        {
            var usuario = _context.tblP_Usuario.FirstOrDefault(x => x.id == p);

            if (usuario != null)
            {
                return usuario.nombre + " " + usuario.apellidoMaterno + " " + usuario.apellidoMaterno;
            }
            else
            {
                return "";
            }
        }

        public List<ResultProveedoresDTO> loadEncuestasByOC(int numeroOC, string centrocostos)
        {

            List<ResultProveedoresDTO> res = new List<ResultProveedoresDTO>();
            var consulta = @"SELECT top 10 A.cc AS centrocostos,A.numero As numeroOC,A.fecha AS FechaOC,A.proveedor AS NumProveedor,A.comentarios AS Comentarios,b.nombre AS NombreProveedor,'1988-01-01' AS fechaAntiguedad, 0 AS estadoEncuesta 
                            FROM DBA.so_orden_compra A INNER JOIN
                            DBA.sp_proveedores B ON B.numpro =A.proveedor 
                            WHERE A.numero='" + numeroOC + "' AND A.cc='" + centrocostos + "' " +
                            "ORDER BY A.fecha DESC";
            try
            {
                var rawDataEnkontrol = (List<ResultProveedoresDTO>)_contextEnkontrol.Where(consulta).ToObject<List<ResultProveedoresDTO>>();


                var rawCompare = from e in rawDataEnkontrol
                                 join oc in _context.tblEN_ResultadoProveedoresDet on e.numeroOC equals oc.numeroOC
                                 into encDet
                                 from oc in encDet.DefaultIfEmpty()
                                 select new { e, oc };

                foreach (var item in rawCompare)
                {
                    var datosSigoplan = item.e;
                    var datosEnkontrol = item.oc;

                    if (item.oc != null)
                    {
                        item.e.encuestaID = item.e.encuestaID;
                    }
                    else
                    {
                        item.e.encuestaID = 0;
                    }

                    res.Add(item.e);

                }
                return res.ToList();
            }
            catch (Exception)
            {
                return new List<ResultProveedoresDTO>();
            }
        }


        public ResultProveedoresDTO datosProveedor(int encuestaID, int numeroOC, string centrocostos)
        {

            ResultProveedoresDTO obj = new ResultProveedoresDTO();
            var respuesta = _context.tblEN_ResultadoProveedoresDet.Where(x => x.numeroOC == numeroOC && x.centrocostos == centrocostos).FirstOrDefault();

            if (respuesta == null)
            {
                var consulta = @"SELECT top 1  A.cc AS centrocostos,d.descripcion AS centrocostosName, A.numero As numeroOC,A.fecha AS FechaOC,A.proveedor AS NumProveedor,
                            A.comentarios AS Comentarios,b.nombre AS NombreProveedor,'19880101' AS FechaAntiguedad,
                            B.ciudad AS ubicacionProveedor ,B.moneda  AS  tipoMoneda 
                            FROM DBA.so_orden_compra A INNER JOIN 
                            DBA.sp_proveedores B ON B.numpro =A.proveedor INNER JOIN 
                            DBA.CC D ON D.CC =  A.cc 
                            WHERE A.ST_OC ='A' AND A.numero='" + numeroOC + "' AND A.cc='" + centrocostos + "' " +
                           "ORDER BY A.fecha DESC";

                var rawDataEnkontrol = (List<ResultProveedoresDTO>)_contextEnkontrol.Where(consulta).ToObject<List<ResultProveedoresDTO>>();

                if (rawDataEnkontrol.FirstOrDefault() != null)
                {
                    var singleData = rawDataEnkontrol.FirstOrDefault();
                    obj.id = 0;
                    obj.centrocostos = centrocostos;
                    obj.centrocostosName = singleData.centrocostosName;
                    obj.comentarios = "";
                    obj.encuestaID = 0;
                    obj.estadoEncuesta = false;
                    obj.fechaAntiguedad = singleData.fechaAntiguedad;
                    obj.fechaOC = singleData.fechaOC;
                    obj.nombreProveedor = singleData.nombreProveedor;
                    obj.numeroOC = singleData.numeroOC;
                    obj.numProveedor = singleData.numProveedor;
                    obj.tipoProveedor = "";
                    obj.ubicacionProveedor = singleData.ubicacionProveedor;
                    obj.tipoMoneda = singleData.tipoMoneda == "1" ? "PESOS" : "DOLARES";
                }

                return obj;
            }
            else
            {

                var singleData = respuesta;
                obj.id = singleData.id;
                obj.centrocostos = centrocostos;
                obj.comentarios = singleData.comentarios;
                obj.estadoEncuesta = true;
                obj.estadoEncuesta = singleData.estadoEncuesta;
                obj.fechaAntiguedad = singleData.fechaAntiguedad;
                obj.fechaOC = singleData.fechaOC != null ? singleData.fechaOC.Value : singleData.fechaEvaluacion.Value;
                obj.nombreProveedor = singleData.nombreProveedor;
                obj.numeroOC = singleData.numeroOC;
                obj.numProveedor = singleData.numProveedor;
                obj.tipoProveedor = singleData.tipoProveedor;
                obj.ubicacionProveedor = singleData.ubicacionProveedor;

                return obj;
            }
        }

        public Respuesta saveEncuestaResultReq(List<tblEN_ResultadoProveedorRequisiciones> obj, tblEN_ResultadoProveedorRequisicionDet objSingle, int tipoEncuesta)
        {
            var r = new Respuesta();

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    objSingle.estatus = true;
                    objSingle.evaluadorID = vSesiones.sesionUsuarioDTO.id;
                    objSingle.fechaEvaluacion = DateTime.Now;

                    //
                    tblEN_Top20Proveedores top20 = null;

                    var provTop20SinRespuesta = _context.tblEN_Top20Proveedores.AsQueryable().Where
                        (w =>
                            w.Numero == objSingle.numProveedor &&
                            w.TipoEncuestaId == tipoEncuesta &&
                            w.UsuarioId == objSingle.evaluadorID &&
                            w.Estatus
                        ).OrderBy(o => o.Id).ToList();

                    foreach (var provSinCalificacion in provTop20SinRespuesta.GroupBy(g => g.FechaTop20))
                    {
                        foreach (var item in provSinCalificacion)
                        {
                            if (item.CantidadEvaluaciones == 0) { top20 = item; break; }
                        }
                        if (top20 != null) { break; }
                    }

                    if (top20 == null)
                    {
                        top20 = provTop20SinRespuesta.LastOrDefault();
                    }
                    //

                    if (top20 != null)
                    {
                        top20.CantidadEvaluaciones++;

                        _context.SaveChanges();

                        var estrellas = _context.tblEN_Estrellas.ToList();
                        var encuestaID = obj.First().encuestaID;
                        var preguntas = _context.tblEN_PreguntasProveedores.Where(w => w.encuestaID == encuestaID);
                        obj.ForEach(e =>
                        {
                            e.calificacionPonderacion = (estrellas.First(f => f.estrellas == e.calificacion).maximo * preguntas.First(f => f.id == e.preguntaID).ponderacion);
                            e.porcentaje = estrellas.FirstOrDefault(x => x.estrellas == e.calificacion) != null ? estrellas.FirstOrDefault(x => x.estrellas == e.calificacion).maximo : 0;
                        });

                        //objSingle.calificacion = (decimal)Math.Truncate(100 * (double)(obj.Where(y => y.porcentaje != null).Select(x => x.porcentaje).Sum() / obj.Count).Value) / 100;
                        objSingle.calificacion = obj.Sum(s => s.calificacionPonderacion);

                        _context.tblEN_ResultadoProveedorRequisicionDet.Add(objSingle);
                        _context.SaveChanges();

                        foreach (var item in obj)
                        {
                            item.usuarioRespondioID = objSingle.evaluadorID;
                            item.encuestaFolioID = objSingle.id;
                            item.fecha = objSingle.fechaEvaluacion.Value;
                            item.tipoEncuesta = tipoEncuesta;
                        }

                        _context.tblEN_ResultadoProveedorRequisiciones.AddRange(obj);
                        _context.SaveChanges();

                        var encuesta = _context.tblEN_EncuestaProveedores.First(f => f.id == encuestaID);

                        var usuarioEvaluador = _context.tblP_Usuario.First(f => f.id == objSingle.evaluadorID);

                        if (objSingle.calificacion < 90)
                        {
                            EnviarNotificacion
                                (
                                    usuarioEvaluador.nombre + " " + usuarioEvaluador.apellidoPaterno + " " + usuarioEvaluador.apellidoMaterno,
                                    objSingle.nombreProveedor,
                                    objSingle.calificacion.Value,
                                    encuesta.descripcion,
                                    top20.TipoEncuesta.descripcion
                                );
                        }


                        if (obj.Any(a => a.calificacion <= 3))
                        {
                            EnviarNotificacionPorPreguntas
                                (
                                    objSingle.nombreProveedor,
                                    usuarioEvaluador.nombre + " " + usuarioEvaluador.apellidoPaterno + " " + usuarioEvaluador.apellidoMaterno,
                                    obj.Where(w => w.calificacion <= 3).OrderBy(o => o.id).Select(m => m.pregunta.pregunta).ToList(),
                                    obj.Where(w => w.calificacion <= 3).OrderBy(o => o.id).Select(m => (int)m.calificacion).ToList()
                                );
                        }
                        
                        transaction.Commit();

                        r.Success = true;
                        r.Message = "Ok";
                    }
                    else
                    {
                        r.Message = "No se guardó la encuesta debido a que no se encontró un registro del proveedor en el top20";
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    r.Message += ex.Message;
                }
            }

            return r;
        }

        private void EnviarNotificacionPorPreguntas(string nombreProveedor, string nombreEvaluador, List<string> preguntas, List<int> calificacion)
        {
            var enviarCorreo = new Infrastructure.DTO.CorreoDTO();

            enviarCorreo.asunto = string.Format("Evaluación del proveedor {0}, con preguntas con baja calificación", nombreProveedor);
            var correos = _context.tblEN_ProvCorreosNotificacion.Where(w => w.Estatus && w.motivoNotificacion == 2).Select(m => m.Correo).ToList();

            string preguntasCalificacion = "<dl>";
            for (int i = 0; i < preguntas.Count; i++)
            {
                preguntasCalificacion += "<dt>" + calificacion[i] + (calificacion[i] > 1 ? " estrellas" : " estrella");
                preguntasCalificacion += "<dd>" + preguntas[i] + "</dd>";
            }
            preguntasCalificacion += "</dl>";

            enviarCorreo.correos = correos;
            enviarCorreo.cuerpo = string.Format
                (
                    @"<p>
                        El usuario: {0} ha realizado la evaluación del proveedor {1}, la cual tiene las siguientes preguntas con baja calificación:
                     </p>
                     <br/>
                     {2}",
                     nombreEvaluador,
                     nombreProveedor,
                     preguntasCalificacion
                );

            string iniCuerpo =
                @"<html><head>
                    <style>
                        table {
                            font-family: arial, sans-serif;
                            border-collapse: collapse;
                            width: 100%;
                        }
                        td, th {
                            border: 1px solid #dddddd;
                            text-align: left;
                            padding: 8px;
                        }
                        tr:nth-child(even) {
                            background-color: #dddddd;
                        }
                    </style>
                </head>
                <body lang=ES-MX link='#0563C1' vlink='#954F72'><div class=WordSection1>";
            string finCuerpo =
                @"<p class=MsoNormal><o:p>&nbsp;</o:p></p><p class=MsoNormal><o:p>&nbsp;</o:p></p>
                    <p class=MsoNormal>
                        Se informa que esta es un correo autogenerado por el sistema SIGOPLAN (<a href='http://sigoplan.construplan.com.mx/'>http://sigoplan.construplan.com.mx</a>). No es necesario dar una respuesta. Gracias.
                    </p>
                    </div></body></html>";

            GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), enviarCorreo.asunto), iniCuerpo + enviarCorreo.cuerpo + finCuerpo, correos);
        }

        private void EnviarNotificacion(string nombreEvaluador, string nombreProveedor, decimal calificacion, string nombreEncuesta, string tipoEncuesta)
        {
            var enviarCorreoNotificacion = new Infrastructure.DTO.CorreoDTO();
            enviarCorreoNotificacion.asunto = string.Format("Evaluación del proveedor {0} es de: {1}", nombreProveedor, calificacion);

            var correos = _context.tblEN_ProvCorreosNotificacion.Where(w => w.Estatus && w.motivoNotificacion == 1).Select(m => m.Correo).ToList();
            //var correos = new List<string>() { "martin.zayas@construplan.com.mx" };

            enviarCorreoNotificacion.correos = correos;
            enviarCorreoNotificacion.cuerpo = string.Format
                (
                    @"<p>El usuario: {0} ha realizado la evaluación del proveedor {1}, con una calificación de {2}.</p>
                                      <br>
                                      <strong>Encuesta:</strong> {3}
                                      <br>
                                      <strong>Tipo encuesta:</strong> {4}",
                    nombreEvaluador,
                    nombreProveedor,
                    calificacion,
                    nombreEncuesta,
                    tipoEncuesta
                );
            //enviarCorreoNotificacion.Enviar();

            string iniCuerpo =
                @"<html><head>
                    <style>
                        table {
                            font-family: arial, sans-serif;
                            border-collapse: collapse;
                            width: 100%;
                        }
                        td, th {
                            border: 1px solid #dddddd;
                            text-align: left;
                            padding: 8px;
                        }
                        tr:nth-child(even) {
                            background-color: #dddddd;
                        }
                    </style>
                </head>
                <body lang=ES-MX link='#0563C1' vlink='#954F72'><div class=WordSection1>";
            string finCuerpo =
                @"<p class=MsoNormal><o:p>&nbsp;</o:p></p><p class=MsoNormal><o:p>&nbsp;</o:p></p>
                    <p class=MsoNormal>
                        Se informa que esta es un correo autogenerado por el sistema SIGOPLAN (<a href='http://sigoplan.construplan.com.mx/'>http://sigoplan.construplan.com.mx</a>). No es necesario dar una respuesta. Gracias.
                    </p>
                    </div></body></html>";

            GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), enviarCorreoNotificacion.asunto), iniCuerpo + enviarCorreoNotificacion.cuerpo + finCuerpo, correos);
        }

        public Respuesta saveEncuestaResult(List<tblEN_ResultadoProveedores> obj, tblEN_ResultadoProveedoresDet objSingle, int tipoEncuesta)
        {
            var r = new Respuesta();

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    objSingle.estadoEncuesta = true;
                    objSingle.evaluadorID = vSesiones.sesionUsuarioDTO.id;
                    objSingle.fechaEvaluacion = DateTime.Now;

                    if ((EmpresaEnum)vSesiones.sesionEmpresaActual == EmpresaEnum.Peru)
                    {
                        var estrellas = _context.tblEN_Estrellas.ToList();
                        var preguntas = _context.tblEN_PreguntasProveedores.Where(w => w.encuestaID == objSingle.encuestaID);
                        obj.ForEach(e =>
                        {
                            e.calificacionPonderacion = (estrellas.First(f => f.estrellas == e.calificacion).maximo * preguntas.First(f => f.id == e.preguntaID).ponderacion);
                            e.porcentaje = estrellas.FirstOrDefault(x => x.estrellas == e.calificacion) != null ? estrellas.FirstOrDefault(x => x.estrellas == e.calificacion).maximo : 0;
                        });

                        //objSingle.calificacion = (decimal)Math.Truncate(100 * (double)(obj.Where(y => y.porcentaje != null).Select(x => x.porcentaje).Sum() / obj.Count).Value) / 100;
                        objSingle.calificacion = obj.Sum(s => s.calificacionPonderacion);

                        _context.tblEN_ResultadoProveedoresDet.Add(objSingle);
                        _context.SaveChanges();

                        foreach (var item in obj)
                        {
                            item.encuestaFolioID = objSingle.id;
                            item.usuarioRespondioID = objSingle.evaluadorID;
                            item.tipoEncuesta = tipoEncuesta;
                            item.fecha = objSingle.fechaEvaluacion.Value;
                        }

                        _context.tblEN_ResultadoProveedores.AddRange(obj);
                        _context.SaveChanges();

                        int encuestaID = obj.First().encuestaID;
                        var encuesta = _context.tblEN_EncuestaProveedores.First(f => f.id == encuestaID);

                        var usuarioEvaluador = _context.tblP_Usuario.First(f => f.id == objSingle.evaluadorID);

                        var tipoEncuestaDescripcion = _context.tblEN_TipoEncuestaProveedor.FirstOrDefault(x => x.id == tipoEncuesta);

                        if (objSingle.calificacion < 90)
                        {
                            EnviarNotificacion
                                (
                                    usuarioEvaluador.nombre + " " + usuarioEvaluador.apellidoPaterno + " " + usuarioEvaluador.apellidoMaterno,
                                    objSingle.nombreProveedor,
                                    objSingle.calificacion.Value,
                                    encuesta.descripcion,
                                    tipoEncuestaDescripcion.descripcion
                                );
                        }

                        transaction.Commit();

                        r.Success = true;
                        r.Message = "Ok";
                    }

                    //
                    tblEN_Top20Proveedores top20 = null;

                    var provTop20SinRespuesta = _context.tblEN_Top20Proveedores.AsQueryable().Where
                        (w =>
                            w.Numero == objSingle.numProveedor &&
                            w.TipoEncuestaId == tipoEncuesta &&
                            w.UsuarioId == objSingle.evaluadorID &&
                            w.Estatus
                        ).OrderBy(o => o.Id).ToList();

                    foreach (var provSinCalificacion in provTop20SinRespuesta.GroupBy(g => g.FechaTop20))
                    {
                        foreach (var item in provSinCalificacion)
                        {
                            if (item.CantidadEvaluaciones == 0) { top20 = item; break; }
                        }
                        if (top20 != null) { break; }
                    }

                    if (top20 == null)
                    {
                        top20 = provTop20SinRespuesta.LastOrDefault();
                    }
                    //

                    if (top20 != null)
                    {
                        top20.CantidadEvaluaciones++;

                        _context.SaveChanges();

                        var estrellas = _context.tblEN_Estrellas.ToList();
                        var preguntas = _context.tblEN_PreguntasProveedores.Where(w => w.encuestaID == objSingle.encuestaID);
                        obj.ForEach(e =>
                        {
                            e.calificacionPonderacion = (estrellas.First(f => f.estrellas == e.calificacion).maximo * preguntas.First(f => f.id == e.preguntaID).ponderacion);
                            e.porcentaje = estrellas.FirstOrDefault(x => x.estrellas == e.calificacion) != null ? estrellas.FirstOrDefault(x => x.estrellas == e.calificacion).maximo : 0;
                        });

                        //objSingle.calificacion = (decimal)Math.Truncate(100 * (double)(obj.Where(y => y.porcentaje != null).Select(x => x.porcentaje).Sum() / obj.Count).Value) / 100;
                        objSingle.calificacion = obj.Sum(s => s.calificacionPonderacion);

                        _context.tblEN_ResultadoProveedoresDet.Add(objSingle);
                        _context.SaveChanges();

                        foreach (var item in obj)
                        {
                            item.encuestaFolioID = objSingle.id;
                            item.usuarioRespondioID = objSingle.evaluadorID;
                            item.tipoEncuesta = tipoEncuesta;
                            item.fecha = objSingle.fechaEvaluacion.Value;
                        }

                        _context.tblEN_ResultadoProveedores.AddRange(obj);
                        _context.SaveChanges();

                        int encuestaID = obj.First().encuestaID;
                        var encuesta = _context.tblEN_EncuestaProveedores.First(f => f.id == encuestaID);

                        var usuarioEvaluador = _context.tblP_Usuario.First(f => f.id == objSingle.evaluadorID);

                        if (objSingle.calificacion < 90)
                        {
                            EnviarNotificacion
                                (
                                    usuarioEvaluador.nombre + " " + usuarioEvaluador.apellidoPaterno + " " + usuarioEvaluador.apellidoMaterno,
                                    objSingle.nombreProveedor,
                                    objSingle.calificacion.Value,
                                    encuesta.descripcion,
                                    top20.TipoEncuesta.descripcion
                                );
                        }

                        transaction.Commit();

                        r.Success = true;
                        r.Message = "Ok";
                    }
                    else
                    {
                        r.Message = "No se guardó la encuesta debido a que no se encontró un registro del proveedor en el top20";
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    r.Message += ex.Message;
                }
            }

            return r;
        }

        public List<RespuestasEncuestasDTO> GetEncuestaContestada(int idEncuestaDEtalle, int tipo)
        {
            var estrellas = _context.tblEN_Estrellas.ToList();

            if (tipo == 1)
            {
                var Respuestas = _context.tblEN_ResultadoProveedores.Where(x => x.encuestaFolioID == idEncuestaDEtalle).ToList();

                List<RespuestasEncuestasDTO> lstObjRespuestasDTO = new List<RespuestasEncuestasDTO>();

                foreach (var item in Respuestas)
                {
                    RespuestasEncuestasDTO ObjRespuestasDTO = new RespuestasEncuestasDTO();

                    ObjRespuestasDTO.Pregunta = item.pregunta.pregunta;
                    ObjRespuestasDTO.tipoPregunta = item.pregunta.tipo;
                    ObjRespuestasDTO.Calificacion = item.calificacion;
                    ObjRespuestasDTO.Comentario = item.respuesta;
                    ObjRespuestasDTO.DescripcionTipo = EnumExtensions.GetDescription((TiposPreguntasEnum)item.pregunta.tipo);
                    ObjRespuestasDTO.CalificacionDescripcion = estrellas.FirstOrDefault(x => x.estrellas == item.calificacion) != null ? estrellas.FirstOrDefault(x => x.estrellas == item.calificacion).descripcion : "";

                    lstObjRespuestasDTO.Add(ObjRespuestasDTO);
                }

                return lstObjRespuestasDTO;
            }
            else
            {
                var Respuestas = _context.tblEN_ResultadoProveedorRequisiciones.Where(x => x.encuestaFolioID == idEncuestaDEtalle).ToList();

                List<RespuestasEncuestasDTO> lstObjRespuestasDTO = new List<RespuestasEncuestasDTO>();

                foreach (var item in Respuestas)
                {
                    RespuestasEncuestasDTO ObjRespuestasDTO = new RespuestasEncuestasDTO();

                    ObjRespuestasDTO.Pregunta = item.pregunta.pregunta;
                    ObjRespuestasDTO.tipoPregunta = item.pregunta.tipo;
                    ObjRespuestasDTO.Calificacion = item.calificacion;
                    ObjRespuestasDTO.DescripcionTipo = EnumExtensions.GetDescription((TiposPreguntasEnum)item.pregunta.tipo);
                    ObjRespuestasDTO.CalificacionDescripcion = estrellas.FirstOrDefault(x => x.estrellas == item.calificacion) != null ? estrellas.FirstOrDefault(x => x.estrellas == item.calificacion).descripcion : "";

                    lstObjRespuestasDTO.Add(ObjRespuestasDTO);
                }

                return lstObjRespuestasDTO;

            }
        }

        public tblEN_ResultadoProveedoresDet GetEncabezadoEncuesta(int proveedorDetID, int ptipo)
        {

            return _context.tblEN_ResultadoProveedoresDet.FirstOrDefault(x => x.id == proveedorDetID);

        }

        public tblEN_ResultadoProveedorRequisicionDet GetEncabezadoEncuestaRequisicion(int proveedorDetID, int ptipo)
        {
            return _context.tblEN_ResultadoProveedorRequisicionDet.FirstOrDefault(x => x.id == proveedorDetID);
        }

        public List<tblRequisicionesDTO> loadRequisiciones(int estatus, DateTime fechainicio, DateTime fechafin)
        {
            string consulta = @"SELECT A.cc AS centroCostos,A.numero AS noRequisicion ,A.comentarios AS comentarios, C.descripcion AS centroCostosName,B.partida, A.fecha_autoriza AS fechaRequisicion FROM so_requisicion AS A
                              INNER JOIN so_requisicion_det AS B 
                              ON A.CC = B.CC AND A.numero = B.numero
                              INNER JOIN cc AS C 
                              ON C.cc = A.cc
                              WHERE A.st_estatus = 'T' AND A.fecha_autoriza BETWEEN '" + fechainicio.ToString("yyyyMMdd") + "' AND '" + fechafin.ToString("yyyyMMdd") + "'" +

                 " ORDER BY A.fecha_autoriza DESC";
            List<tblRequisicionesDTO> res = new List<tblRequisicionesDTO>();
            try
            {
                var rawDataEnkontrol = (List<tblRequisicionesDTO>)_contextEnkontrol.Where(consulta).ToObject<List<tblRequisicionesDTO>>();


                var rawCompare = from e in rawDataEnkontrol
                                 join re in _context.tblEN_ResultadoProveedorRequisicionDet on e.noRequisicion equals re.numeroRequisicion
                                 into encDet
                                 from re in encDet.DefaultIfEmpty()
                                 select new { e, re };


                foreach (var item in rawCompare)
                {
                    var datosSigoplan = item.e;
                    var datosEnkontrol = item.re;

                    if (datosEnkontrol != null)
                    {
                        item.e.id = datosEnkontrol.id;
                        item.e.estatus = true;
                    }
                    else
                    {
                        item.e.id = 0;
                        item.e.estatus = false;
                    }

                    item.e.requisicion = item.e.centroCostos + "-" + item.e.noRequisicion.ToString();

                    res.Add(item.e);

                }

                return rawDataEnkontrol;


            }
            catch (Exception)
            {

                throw;
            }
        }


        public tblRequisicionesDTO loadEncuestasByRequisicion(int requisicion, string centrocostos)
        {

            List<tblRequisicionesDTO> res = new List<tblRequisicionesDTO>();
            string consulta = @"SELECT A.cc AS centroCostos,A.numero AS noRequisicion ,A.comentarios AS comentarios, C.descripcion AS centroCostosName,B.partida, A.fecha_autoriza AS fechaRequisicion FROM so_requisicion AS A
                              INNER JOIN so_requisicion_det AS B 
                              ON A.CC = B.CC AND A.numero = B.numero
                              INNER JOIN cc AS C 
                              ON C.cc = A.cc
                              WHERE A.st_autoriza = 'S' AND A.numero = " + requisicion + " AND  A.cc= '" + centrocostos + "'  ORDER BY A.fecha_autoriza DESC";
            try
            {
                var rawDataEnkontrol = (List<tblRequisicionesDTO>)_contextEnkontrol.Where(consulta).ToObject<List<tblRequisicionesDTO>>();


                var rawCompare = from e in rawDataEnkontrol
                                 join re in _context.tblEN_ResultadoProveedorRequisicionDet on e.noRequisicion equals re.numeroRequisicion
                                 into encDet
                                 from oc in encDet.DefaultIfEmpty()
                                 select new { e, oc };

                foreach (var item in rawCompare)
                {
                    var datosSigoplan = item.e;
                    var datosEnkontrol = item.oc;

                    if (item.oc != null)
                    {
                        item.e.estatus = true;
                        item.e.id = datosEnkontrol.id;
                    }
                    else
                    {
                        item.e.estatus = false;


                    }

                    res.Add(item.e);


                }
                return res.FirstOrDefault();
            }
            catch (Exception)
            {
                return new tblRequisicionesDTO();
            }
        }


        public List<tblRequisicionesDTO> loadRequisicionesByFiltros(int estatus, DateTime fechainicio, DateTime fechafin, List<int> compradores)
        {
            var proveedores = _proveedorFS.getProveedorService().GetProveedores();

            var resultadoEncuestas = _context.tblEN_ResultadoProveedorRequisicionDet
                .Where(w =>
                    w.estatus &&
                    w.calificacion.HasValue &&
                    w.detalles.Any(a => a.tipoEncuesta == estatus) &&
                    compradores.Contains(w.evaluadorID) &&
                    w.numProveedor != 0 &&
                    w.fechaEvaluacion.HasValue &&
                    w.fechaEvaluacion.Value >= fechainicio &&
                    w.fechaEvaluacion.Value <= fechafin
                ).ToList();

            var info = new List<tblRequisicionesDTO>();

            foreach (var resultado in resultadoEncuestas)
            {
                var proveedor = proveedores.FirstOrDefault(f => f.numpro == resultado.numProveedor);

                var nombreCompletoEvaluador =
                    resultado.usuario.nombre +
                    (!string.IsNullOrEmpty(resultado.usuario.apellidoPaterno) ? " " + resultado.usuario.apellidoPaterno : "") +
                    (!string.IsNullOrEmpty(resultado.usuario.apellidoMaterno) ? " " + resultado.usuario.apellidoMaterno : "");

                var tbl = new tblRequisicionesDTO();

                tbl.id = resultado.id;
                tbl.numeroProveedor = resultado.numProveedor;
                tbl.nombreProveedor = resultado.nombreProveedor;
                tbl.monedaProveedor = proveedor != null ? proveedor.descripcionMoneda : "";
                tbl.nombreEvaluador = nombreCompletoEvaluador;
                tbl.fechaEvaluacion = resultado.fechaEvaluacion.Value;
                tbl.calificacion = resultado.calificacion.Value;
                tbl.comentario = resultado.comentarios;

                info.Add(tbl);
            }



            return info;
        }

        public int getTipoEncuesta(int encuestaId)
        {
            return _context.tblEN_EncuestaProveedores.First(x => x.estatus && x.id == encuestaId).tipoEncuesta;
        }

        public ObjGraficasDTO getGraficaEvaluacionProveedores(DateTime fechainicio, DateTime fechafin, int encuesta, int tipoEncuesta, List<int> listaUsuarios)
        {
            try
            {
                var proveedores = _proveedorFS.getProveedorService().GetProveedores();

                //List<int> idPuesto = new List<int>();
                //idPuesto.Add(6);
                //idPuesto.Add(14);

                //var UsuariosPuestos = _context.tblP_Usuario.Join(idPuesto, o => o.puestoID, id => id, (o, id) => o).ToList();
                //
                //var encuestaId = tipoEncuesta;
                //tipoEncuesta = _context.tblEN_EncuestaProveedores.First(x => x.estatus && x.id == tipoEncuesta).tipoEncuesta;
                //

                if (encuesta == 1)
                {
                    var rawResult = _context.tblEN_ResultadoProveedores.Where
                        (x =>
                            x.Detalle.calificacion != null &&
                            x.Detalle.estadoEncuesta &&
                            x.encuestaID == tipoEncuesta &&
                            (
                                (x.Detalle.fechaEvaluacion == null && (x.fecha >= fechainicio && x.fecha <= fechafin)) ||
                                (x.Detalle.fechaEvaluacion != null && (x.Detalle.fechaEvaluacion.Value >= fechainicio && x.Detalle.fechaEvaluacion.Value <= fechafin))
                            ) &&
                            listaUsuarios.Contains(x.Detalle.evaluadorID)

                        ).OrderBy(x => x.usuarioRespondioID).ToList();

                    List<EvaluacionCompradoresDTO> Compradores = new List<EvaluacionCompradoresDTO>();
                    foreach (var usuarioID in listaUsuarios)
                    {

                        var usuario = _context.tblP_Usuario.Where(x => x.id == usuarioID).FirstOrDefault();
                        EvaluacionCompradoresDTO obj = new EvaluacionCompradoresDTO();
                        var R = rawResult.Where(x => x.usuarioRespondioID == usuarioID).ToList();

                        var Cantidad = R.GroupBy(x => x.encuestaFolioID).Count();

                        obj.CantidadEvaluaciones = Cantidad;
                        obj.idComprador = usuarioID;
                        obj.nombreComprador = usuario.nombre + " " + usuario.apellidoPaterno;

                        Compradores.Add(obj);
                    }

                    ObjGraficasDTO res = new ObjGraficasDTO();
                    res.CompradoresList = Compradores;
                    res.ProveedoresList = rawResult;

                    //
                    var rEncuestas = rawResult.Select(m => m.Detalle).Distinct();

                    var rPreguntas = rawResult.GroupBy(g => g.preguntaID).OrderBy(o => o.Key);
                    res.Preguntas = rPreguntas.Select(m => m.First().pregunta.pregunta).ToList();
                    foreach (var pregunta in rPreguntas)
                    {
                        var ponderacionTotal = pregunta.Sum(s => s.calificacionPonderacion.Value) / pregunta.Count();
                        res.Calificaciones.Add(Math.Round(ponderacionTotal / pregunta.First().pregunta.ponderacion, 2));
                    }

                    var rMasEvaluados = rEncuestas.GroupBy(g => g.numProveedor).OrderByDescending(o => o.Count()).Take(15);
                    res.ProveedoresMasEvaluados = rMasEvaluados.Select(m => (m.First().tipoProveedor == "1" ? "" : "(USD) ") + m.First().nombreProveedor).ToList();
                    foreach (var item in rMasEvaluados)
                    {
                        res.CalificacionesMasEvaluados.Add(Math.Round(item.Sum(s => s.calificacion.Value) / item.Count(), 2));
                    }

                    var rPeorEvaluados = rEncuestas.GroupBy(g => g.numProveedor).Select(m => new { cali = m.Sum(s => s.calificacion) / m.Count(), prov = (m.First().tipoProveedor == "1" ? "" : "(USD) ") + m.First().nombreProveedor }).Where(w => w.cali < 100).OrderBy(o => o.cali).Take(5);
                    res.ProveedoresPeorEvaluados = rPeorEvaluados.Select(m => m.prov).ToList();
                    res.CalificacionesPeorEvaluados = rPeorEvaluados.Select(m => Math.Round(m.cali.Value, 2)).ToList();

                    var provBest = rEncuestas.GroupBy(g => g.numProveedor).Where(w => w.Count() >= 5).Select(m => new { cali = m.Sum(s => s.calificacion) / m.Count(), prov = (m.First().tipoProveedor == "1" ? "" : "(USD) ") + m.First().nombreProveedor, cantidadEvaluaciones = m.Count() }).OrderByDescending(o => o.cali).ThenByDescending(o => o.cantidadEvaluaciones).Take(10).ToList();
                    res.ProveedoresBest = provBest.Select(m => m.prov).ToList();
                    res.CalificacionesBest = provBest.Select(m => Math.Round(m.cali.Value, 2)).ToList();

                    return res;
                }
                else
                {
                    //var rawResult = _context.tblEN_ResultadoProveedorRequisiciones.Where(x => (x.fecha >= fechainicio && x.fecha <= fechafin) && x.tipoEncuesta == tipoEncuesta).ToList();
                    var rawResult = _context.tblEN_ResultadoProveedorRequisiciones.Where
                        (x =>
                            x.encuestaID == tipoEncuesta &&
                            x.encuesta.estatus &&
                            x.Detalle.calificacion != null &&
                            (
                                (x.Detalle.fechaEvaluacion == null && (x.fecha >= fechainicio && x.fecha <= fechafin)) ||
                                (x.Detalle.fechaEvaluacion != null && (x.Detalle.fechaEvaluacion.Value >= fechainicio && x.Detalle.fechaEvaluacion.Value <= fechafin))
                            ) &&
                            listaUsuarios.Contains(x.Detalle.evaluadorID) &&
                            x.calificacionPonderacion != null
                        ).ToList();
                    List<EvaluacionCompradoresDTO> Compradores = new List<EvaluacionCompradoresDTO>();
                    foreach (var item in listaUsuarios)
                    {
                        var usuario = _context.tblP_Usuario.First(f => f.id == item);
                        EvaluacionCompradoresDTO obj = new EvaluacionCompradoresDTO();
                        var R = rawResult.Where(x => x.usuarioRespondioID == item).ToList();

                        var Cantidad = R.GroupBy(x => x.encuestaFolioID).Count();

                        obj.CantidadEvaluaciones = Cantidad;
                        obj.idComprador = item;
                        obj.nombreComprador = usuario.nombre + " " + usuario.apellidoPaterno;

                        Compradores.Add(obj);
                    }

                    ObjGraficasDTO res = new ObjGraficasDTO();
                    res.CompradoresList = Compradores;
                    res.ProveedoresList = null;
                    res.ProveedoresListOC = rawResult;

                    //
                    var rEncuestas = rawResult.Select(m => m.Detalle).Distinct();

                    var rPreguntas = rawResult.GroupBy(g => g.preguntaID).OrderBy(o => o.Key);
                    res.Preguntas = rPreguntas.Select(m => m.First().pregunta.pregunta).ToList();
                    foreach (var pregunta in rPreguntas)
                    {
                        var ponderacionTotal = pregunta.Sum(s => s.calificacionPonderacion.Value) / pregunta.Count();
                        res.Calificaciones.Add(Math.Round(ponderacionTotal / pregunta.First().pregunta.ponderacion, 2));
                    }

                    var rMasEvaluados = rEncuestas.GroupBy(g => g.numProveedor).OrderByDescending(o => o.Count()).Take(15);
                    foreach (var item in rMasEvaluados)
                    {
                        var infoProv = proveedores.FirstOrDefault(f => f.numpro == item.First().numProveedor);

                        if (infoProv != null && string.IsNullOrEmpty(item.First().tipoMoneda))
                        {
                            item.First().tipoMoneda = infoProv.descripcionMoneda;
                        }
                    }
                    res.ProveedoresMasEvaluados = rMasEvaluados.Select(m => (!string.IsNullOrEmpty(m.First().tipoMoneda) ? "(" + m.First().tipoMoneda + ") " : "") + m.First().nombreProveedor).ToList();
                    foreach (var item in rMasEvaluados)
                    {
                        res.CalificacionesMasEvaluados.Add(Math.Round(item.Sum(s => s.calificacion.Value) / item.Count(), 2));
                    }

                    var rPeorEvaluados = rEncuestas.GroupBy(g => g.numProveedor).Select(m => new { cali = m.Sum(s => s.calificacion) / m.Count(), prov = m.First().nombreProveedor, numProv = m.First().numProveedor, tipoMoneda = m.First().tipoMoneda }).Where(w => w.cali < 100).OrderBy(o => o.cali).Take(5);
                    foreach (var item in rPeorEvaluados)
                    {
                        if (!string.IsNullOrEmpty(item.tipoMoneda))
                        {
                            res.ProveedoresPeorEvaluados.Add("(" + item.tipoMoneda + ") " + item.prov);
                        }
                        else
                        {
                            var infoProv = proveedores.FirstOrDefault(f => f.numpro == item.numProv);

                            if (infoProv != null)
                            {
                                res.ProveedoresPeorEvaluados.Add("(" + infoProv.descripcionMoneda + ") " + item.prov);
                            }
                            else
                            {
                                res.ProveedoresPeorEvaluados.Add(item.prov);
                            }
                        }
                    }
                    res.CalificacionesPeorEvaluados = rPeorEvaluados.Select(m => Math.Round(m.cali.Value, 2)).ToList();

                    var provBest = rEncuestas.GroupBy(g => g.numProveedor).Where(w => w.Count() >= 5).Select(m => new { cali = m.Sum(s => s.calificacion) / m.Count(), prov = m.First().nombreProveedor, cantidadEvaluaciones = m.Count(), numProv = m.First().numProveedor, tipoMoneda = m.First().tipoMoneda }).OrderByDescending(o => o.cali).ThenByDescending(o => o.cantidadEvaluaciones).Take(10).ToList();
                    foreach (var item in provBest)
                    {
                        if (!string.IsNullOrEmpty(item.tipoMoneda))
                        {
                            res.ProveedoresBest.Add("(" + item.tipoMoneda + ") " + item.prov);
                        }
                        else
                        {
                            var infoProv = proveedores.FirstOrDefault(f => f.numpro == item.numProv);

                            if (infoProv != null)
                            {
                                res.ProveedoresBest.Add("(" + infoProv.descripcionMoneda + ") " + item.prov);
                            }
                            else
                            {
                                res.ProveedoresBest.Add(item.prov);
                            }
                        }
                    }
                    res.CalificacionesBest = provBest.Select(m => Math.Round(m.cali.Value, 2)).ToList();
                    //

                    return res;
                }
            }
            catch (Exception)
            {
                
                throw;
            }

           
        }

        private EncuestaConDetalleDTO getEncuestaProvDTO(List<int> rawListProveedores, int tipoEncuesta)
        {
            var lstProv = new List<EncuestaProvDTO>();
            var lstProvDet = new List<EncuestaProvDetDTO>();
            
            var encuesta = new EncuestaConDetalleDTO();

            var proveedores = _proveedorFS.getProveedorService().GetProveedores();

            switch (tipoEncuesta)
            {
                case 1:
                    lstProv = _context.tblEN_ResultadoProveedores.Where(x => rawListProveedores.Contains(x.encuestaFolioID)).Select(m => new EncuestaProvDTO()
                    {
                        id = m.id,
                        encuestaID = m.encuestaID,
                        encuesta = m.encuesta,
                        preguntaID = m.preguntaID,
                        usuarioRespondioID = m.usuarioRespondioID,
                        usuarioRespondio = m.evaluador,
                        encuestaFolioID = m.encuestaFolioID,
                        calificacion = m.calificacion,
                        fecha = m.fecha,
                        tipoEncuesta = m.tipoEncuesta,
                        respuesta = m.respuesta,
                        porcentaje = m.porcentaje,
                        pregunta = m.pregunta,
                        detalle = new EncuestaProvDetDTO()
                        {
                            id = m.Detalle.id,
                            centroCostos = m.Detalle.centrocostos,
                            nombreProveedor = m.Detalle.nombreProveedor,
                            comentarios = m.Detalle.comentarios,
                            evaluadorID = m.Detalle.evaluadorID,
                            numProveedor = m.Detalle.numProveedor,
                            calificacion = m.Detalle.calificacion
                        }
                    }).ToList();
                    break;
                default:
                    lstProv = _context.tblEN_ResultadoProveedorRequisiciones.Where(x => rawListProveedores.Contains(x.encuestaFolioID)).Select(m => new EncuestaProvDTO()
                    {
                        id = m.id,
                        encuestaID = m.encuestaID,
                        encuesta = m.encuesta,
                        preguntaID = m.preguntaID,
                        usuarioRespondioID = m.usuarioRespondioID,
                        usuarioRespondio = m.usuarioRespondio,
                        encuestaFolioID = m.encuestaFolioID,
                        calificacion = m.calificacion,
                        fecha = m.fecha,
                        tipoEncuesta = m.tipoEncuesta,
                        respuesta = m.respuesta,
                        porcentaje = m.porcentaje,
                        pregunta = m.pregunta,
                        detalle = new EncuestaProvDetDTO()
                        {
                            id = m.Detalle.id,
                            centroCostos = m.Detalle.centroCostos,
                            nombreProveedor = m.Detalle.nombreProveedor,
                            comentarios = m.Detalle.comentarios,
                            evaluadorID = m.Detalle.evaluadorID,
                            numProveedor = m.Detalle.numProveedor,
                            calificacion = m.Detalle.calificacion
                        }
                    }).ToList();
                    break;
            }
            
            lstProvDet = lstProv.Select(m => m.detalle).GroupBy(g => g.id).Select(m => m.First()).ToList();

            foreach (var item in lstProvDet)
            {
                var infoProv = proveedores.FirstOrDefault(f => f.numpro == item.numProveedor);

                if (infoProv != null)
                {
                    item.tipoMoneda = infoProv.descripcionMoneda;
                }
            }

            encuesta.EncuestaProvDTO.AddRange(lstProv);
            encuesta.EncuestaProvDetDTO.AddRange(lstProvDet);

            return encuesta;
        }

        public List<ResEvaluacionProveedoresDTO> getProveedoresCalificaciones(List<int> rawListProveedores, int tipoEncuesta)
        {
            var encuesta = getEncuestaProvDTO(rawListProveedores, tipoEncuesta);

            var lstProv = encuesta.EncuestaProvDTO;
            var lstProvDet = encuesta.EncuestaProvDetDTO;

            List<ResEvaluacionProveedoresDTO> returnData = new List<ResEvaluacionProveedoresDTO>();

            foreach (var item in lstProv.GroupBy(x => x.encuestaFolioID))
            {
                ResEvaluacionProveedoresDTO res = new ResEvaluacionProveedoresDTO();

                var foliosEncuestas = lstProvDet.FirstOrDefault(x => x.id == item.Key);

                if (foliosEncuestas != null)
                {
                    string tipoProveedor = "";
                    int countRegular = 0;
                    int countMalos = 0;
                    //int countBuenos = 0;


                    var getRespuestasSI = item.Where(x => x.calificacion >= 3);
                    var resultadosRespuestasSI = getRespuestasSI.Select(x => x.pregunta.ponderacion).ToList();

                    decimal SumaCalificacion = resultadosRespuestasSI.Sum(x => x);

                    decimal SumaCalificacionEstrella = item.Select(x => x.calificacion).Sum();

                    if (SumaCalificacion >= 0 && SumaCalificacion <= 0.44M)
                    {
                        countMalos++;
                        tipoProveedor = "Malo";
                    }
                    else if (SumaCalificacion >= 0.45M && SumaCalificacion <= 0.74M)
                    {
                        countRegular++;
                        tipoProveedor = "Regular";
                    }

                    if (!string.IsNullOrEmpty(tipoProveedor))
                    {
                        res.folioID = item.Key;
                        res.proveedorID = foliosEncuestas.numProveedor;
                        res.proveedorName = foliosEncuestas.nombreProveedor;
                        res.comentario = foliosEncuestas.comentarios;
                        res.tipoProveedor = tipoProveedor;
                        returnData.Add(res);
                    }
                }
            }
            return returnData;
        }

        public List<ResEvaluacionProveedoresDTO> getProveedoresCalificacionesEstrellas(List<int> rawListProveedores, int tipoEncuesta)
        {
            var encuesta = getEncuestaProvDTO(rawListProveedores, tipoEncuesta);

            var lstProv = encuesta.EncuestaProvDTO;
            var lstProvDet = encuesta.EncuestaProvDetDTO;

            List<ResEvaluacionProveedoresDTO> returnData = new List<ResEvaluacionProveedoresDTO>();

            foreach (var item in lstProv.GroupBy(g => g.encuestaFolioID))
            {
                ResEvaluacionProveedoresDTO res = new ResEvaluacionProveedoresDTO();

                var foliosEncuestas = lstProvDet.FirstOrDefault(x => x.id == item.Key);

                if (foliosEncuestas != null)
                {
                    string tipoProveedor = "";

                    var getRespuestasSI = item.Where(x => x.calificacion >= 3);
                    var resultadosRespuestasSI = getRespuestasSI.Select(x => x.pregunta.ponderacion).ToList();

                    var estrellas = getRespuestasSI.Select(x => x.calificacion).ToList();

                    int SumaCalificacion = (int)(item.Select(x => x.calificacion).Sum() / item.Count());

                    switch (SumaCalificacion)
                    {
                        case 1:
                            tipoProveedor = "Pésimo";
                            break;
                        case 2:
                            tipoProveedor = "Malo";
                            break;
                        case 3:
                            tipoProveedor = "Regular";
                            break;
                        case 4:
                            tipoProveedor = "Aceptable";
                            break;
                        case 5:
                            tipoProveedor = "Estupendo";
                            break;
                    }

                    if (!string.IsNullOrEmpty(tipoProveedor))
                    {
                        res.folioID = item.Key;
                        res.proveedorID = foliosEncuestas.numProveedor;
                        res.proveedorName = foliosEncuestas.nombreProveedor;
                        res.comentario = foliosEncuestas.comentarios;
                        res.nombreEvaluador = item.First().usuarioRespondio.nombre + " " + item.First().usuarioRespondio.apellidoPaterno ?? "" + " " + item.First().usuarioRespondio.apellidoMaterno ?? "";
                        res.fechaEvaluacion = item.First().fecha;
                        res.tipoProveedor = tipoProveedor;
                        //res.tipoMoneda = infoProv != null ? infoProv.descripcionMoneda : "";
                        res.tipoMoneda = item.First().detalle.tipoMoneda ?? "";
                        returnData.Add(res);
                    }
                }
            }

            return returnData;
        }

        public string getEncuestaByFolioIDOC(int id)
        {
            var obj = _context.tblEN_ResultadoProveedores.FirstOrDefault(x => x.encuestaFolioID == id);

            if (obj != null)
            {
                return obj.fecha.ToShortDateString();
            }
            else
            {
                return "";

            }

        }

        public List<ProveedoresDTO> getNombreProveedores(string term)
        {
            List<ProveedoresDTO> lstProveedores = new List<ProveedoresDTO>();
            string consulta = "SELECT numpro AS noProveedor, nombre AS nomProveedor " +
                              "FROM sp_proveedores " +
                              "WHERE nombre like '%" + term + "%'";
            try
            {
                var resultado = (IList<ProveedoresDTO>)_contextEnkontrol.Where(consulta).ToObject<IList<ProveedoresDTO>>();
                foreach (var item in resultado)
                {
                    lstProveedores.Add(item);
                }
            }
            catch
            {
                return lstProveedores;
            }
            return lstProveedores;
        }

        public List<tblP_Usuario> getUsuariosRealizadoEncuestas()
        {
            var usuarios = new List<tblP_Usuario>();

            var usuariosProv = _context.tblEN_ResultadoProveedoresDet.Where(w => w.usuario.estatus).Select(m => m.usuario).Distinct().ToList();
            var usuariosProvReq = _context.tblEN_ResultadoProveedorRequisicionDet.Where(w => w.estatus).Select(m => m.usuario).Distinct().ToList();
            
            usuarios.AddRange(usuariosProv);
            usuarios.AddRange(usuariosProvReq);

            return usuarios.Distinct().ToList();
        }

        public List<ComboDTO> ComboDtoProveedores(List<int> listaFoliosID, int tipoEncuesta)
        {
            if (tipoEncuesta == 1)
            {
                var detProveedorDet = _context.tblEN_ResultadoProveedoresDet.Where(x => listaFoliosID.Contains(x.id)).ToList();

                var listaProveedores = detProveedorDet.Select(x => x.numProveedor).Distinct().ToList().Select(x => new ComboDTO
                {
                    Text = detProveedorDet.FirstOrDefault(p => p.numProveedor == x).nombreProveedor,
                    Value = x.ToString()

                }).ToList();

                return listaProveedores;
            }
            else
            {
                var detProveedorDet = _context.tblEN_ResultadoProveedorRequisicionDet.Where(x => listaFoliosID.Contains(x.id)).ToList();
                var listaProveedores = detProveedorDet.Select(x => x.numProveedor).Distinct().ToList().Select(x => new ComboDTO
                {
                    Text = detProveedorDet.FirstOrDefault(p => p.numProveedor == x).nombreProveedor,
                    Value = x.ToString()

                }).ToList();

                return listaProveedores;
            }
        }
        public List<ComboDTO> comboTipoEncuesta()
        {
            var cbo = (from tipo in _context.tblEN_TipoEncuestaProveedor
                       where tipo.esActivo
                       select new ComboDTO()
                       {
                           Text = tipo.descripcion,
                           Value = tipo.id.ToString(),
                           Prefijo = ((int)tipo.tipoDependencia).ToString()
                       }).ToList();
            return cbo;
        }
        public List<ResEvaluacionProveedoresDTO> ListaProveedores(List<int> listaFoliosID, int tipoEncuesta)
        {
            var encuesta = getEncuestaProvDTO(listaFoliosID, tipoEncuesta);

            var lstProv = encuesta.EncuestaProvDTO;
            var lstProvDet = encuesta.EncuestaProvDetDTO;

            List<ResEvaluacionProveedoresDTO> objResultList = new List<ResEvaluacionProveedoresDTO>();

            foreach (var item in lstProvDet.Select(x => x.numProveedor).Distinct().ToList())
            {

                ResEvaluacionProveedoresDTO objResult = new ResEvaluacionProveedoresDTO();
                var data = lstProvDet.Where(x => x.numProveedor == item);
                var listaID = data.Select(x => x.id).ToList();

                var nombreProveedor = lstProvDet.FirstOrDefault(x => x.numProveedor == item).nombreProveedor;

                var result = lstProv.Where(x => listaID.Contains(x.encuestaFolioID)).ToList();

                int countBuenos = 0;
                int countRegular = 0;
                int countMalos = 0;
                int calificaciones = 0;
                decimal totalCalificaciones = 0.0M;

                foreach (var folioID in listaID)
                {
                    var SumaCalificacion = result.Where(x => x.calificacion >= 3 && x.encuestaFolioID == folioID).Sum(x => x.pregunta.ponderacion);
                    if (SumaCalificacion >= 0.75M && SumaCalificacion <= 1)
                    {
                        countBuenos++;
                    }
                    else if (SumaCalificacion >= 0.45M && SumaCalificacion <= 0.74M)
                    {
                        countRegular++;
                    }
                    else
                        if (SumaCalificacion >= 0 && SumaCalificacion <= .44M)
                        {
                            countMalos++;
                        }
                    calificaciones += result.Where(x => x.encuestaFolioID == folioID).Count();
                    totalCalificaciones += result.Where(x => x.encuestaFolioID == folioID).Sum(s => s.porcentaje.Value);
                }


                objResult.cantidadBuenos = countBuenos;
                objResult.cantidadMalos = countMalos;
                objResult.cantidadRegulares = countRegular;
                objResult.proveedorID = item;
                objResult.proveedorName = nombreProveedor;
                objResult.porcentaje = totalCalificaciones / calificaciones;
                objResultList.Add(objResult);

            }

            return objResultList;
        }

        public List<ResEvaluacionProveedoresDTO> ListaProveedoresEstrellas(List<int> listaFoliosID, int tipoEncuesta)
        {
            var detProveedorDet = getEncuestaProvDTO(listaFoliosID, tipoEncuesta);
            //var detProveedorDet = _context.tblEN_ResultadoProveedoresDet.Where(x => listaFoliosID.Contains(x.id)).ToList();
            List<ResEvaluacionProveedoresDTO> objResultList = new List<ResEvaluacionProveedoresDTO>();
            foreach (var item in detProveedorDet.EncuestaProvDetDTO.Select(x => new { x.numProveedor, x.tipoMoneda }).Distinct().ToList())
            {

                ResEvaluacionProveedoresDTO objResult = new ResEvaluacionProveedoresDTO();
                var data = detProveedorDet.EncuestaProvDetDTO.Where(x => x.numProveedor == item.numProveedor);
                var listaID = data.Select(x => x.id).ToList();

                var nombreProveedor = detProveedorDet.EncuestaProvDetDTO.FirstOrDefault(x => x.numProveedor == item.numProveedor).nombreProveedor;

                var result = detProveedorDet.EncuestaProvDTO.Where(w => listaID.Contains(w.encuestaFolioID)).ToList();
                //var result = _context.tblEN_ResultadoProveedores.Where(x => listaID.Contains(x.encuestaFolioID)).ToList();

                var countPesimos = 0;
                var countMalos = 0;
                var countRegulares = 0;
                var countAceptables = 0;
                var countEstupendos = 0;
                int calificaciones = 0;
                decimal totalCalificaciones = 0.0M;
                var porcentajes = new List<decimal> { 25.99M, 59.99M, 79.99M, 90.49M, 100.00M };

                var caliProm = data.Sum(s => s.calificacion) / data.Count();

                foreach (var folioID in listaID)
                {

                    int SumaCalificacion = (int)result.Where(x => x.encuestaFolioID == folioID).Sum(x => x.calificacion) / result.Where(x => x.encuestaFolioID == folioID).Count();

                    switch (SumaCalificacion)
                    {
                        case 1:
                            countPesimos++;
                            break;
                        case 2:
                            countMalos++;
                            break;
                        case 3:
                            countRegulares++;
                            break;
                        case 4:
                            countAceptables++;
                            break;
                        case 5:
                            countEstupendos++;
                            break;
                    }

                    calificaciones++;
                    totalCalificaciones += porcentajes[SumaCalificacion - 1];
                }

                objResult.cantidadPesimos = countPesimos;
                objResult.cantidadMalos = countMalos;
                objResult.cantidadRegulares = countRegulares;
                objResult.cantidadAceptables = countAceptables;
                objResult.cantidadEstupendos = countEstupendos;
                //objResult.porcentaje = totalCalificaciones / calificaciones;
                objResult.porcentaje = caliProm.Value;
                objResult.proveedorID = item.numProveedor;
                objResult.proveedorName = nombreProveedor;
                objResult.tipoMoneda = item.tipoMoneda;
                objResultList.Add(objResult);
            }

            return objResultList;
        }

        public Respuesta setGraficaPreguntasProv(int numProv, int tipoEncuesta, int encuestaID, DateTime fechaIni, DateTime fechaFin, List<int> listaUsuario)
        {
            var r = new Respuesta();

            try
            {
                if (tipoEncuesta == (int)tiposEncuestasEnum.EvaluaciónContinuaDeProveedores)
                {
                    var encuestaPreguntasResult = _context.tblEN_ResultadoProveedores.Where
                        (w =>
                            w.encuestaID == encuestaID &&
                            (
                                (w.Detalle.fechaEvaluacion == null && (w.fecha >= fechaIni && w.fecha <= fechaFin)) ||
                                (w.Detalle.fechaEvaluacion != null && (w.Detalle.fechaEvaluacion.Value >= fechaIni && w.Detalle.fechaEvaluacion.Value <= fechaFin))
                            ) &&
                            w.Detalle.numProveedor == numProv &&
                            w.Detalle.estadoEncuesta &&
                            w.Detalle.calificacion != null &&
                            listaUsuario.Contains(w.Detalle.evaluadorID)
                        ).ToList();

                    var preguntas = encuestaPreguntasResult.GroupBy(g => g.preguntaID).OrderBy(o => o.Key).Select(m => m.First().pregunta.pregunta);
                    var calificaciones = new List<decimal>();
                    foreach (var item in encuestaPreguntasResult.GroupBy(g => g.preguntaID).OrderBy(o => o.Key))
                    {
                        var caliPonde = item.Sum(s => s.calificacionPonderacion.Value) / item.Count();
                        calificaciones.Add(Math.Round(caliPonde * 100 / (item.First().pregunta.ponderacion * 100), 2));
                        //calificaciones.Add(Math.Round(item.Sum(s => s.porcentaje.Value) / item.Count(), 2));
                    }

                    r.Success = true;
                    r.Message = "Ok";
                    r.Value = new { preguntas = preguntas, calificaciones = calificaciones };
                }
                else
                {
                    var encuestaPreguntasResult = _context.tblEN_ResultadoProveedorRequisiciones.Where
                        (w =>
                            w.encuestaID == encuestaID &&
                            w.Detalle.numProveedor == numProv &&
                            w.Detalle.estatus &&
                            w.Detalle.calificacion != null &&
                            (
                                (w.Detalle.fechaEvaluacion == null && (w.fecha >= fechaIni && w.fecha <= fechaFin)) ||
                                (w.Detalle.fechaEvaluacion != null && (w.Detalle.fechaEvaluacion.Value >= fechaIni && w.Detalle.fechaEvaluacion.Value <= fechaFin))
                            ) &&
                            listaUsuario.Contains(w.Detalle.evaluadorID)
                        ).ToList();

                    var preguntas = encuestaPreguntasResult.GroupBy(g => g.preguntaID).OrderBy(o => o.Key).Select(m => m.First().pregunta.pregunta);
                    var calificaciones = new List<decimal>();
                    foreach (var item in encuestaPreguntasResult.GroupBy(g => g.preguntaID).OrderBy(o => o.Key))
                    {
                        var caliPonde = item.Sum(s => s.calificacionPonderacion.Value) / item.Count();
                        calificaciones.Add(Math.Round(caliPonde * 100 / (item.First().pregunta.ponderacion * 100), 2));
                        //calificaciones.Add(Math.Round(item.Sum(s => s.porcentaje.Value) / item.Count(), 2));
                    }

                    r.Success = true;
                    r.Message = "Ok";
                    r.Value = new { preguntas = preguntas, calificaciones = calificaciones };
                }
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return r;
        }

        public Respuesta setGraficaMensualProv(int numProv, int tipoEncuesta, int encuestaID, List<int> listaUsuario)
        {
            var r = new Respuesta();

            try
            {
                if (tipoEncuesta == (int)tiposEncuestasEnum.EvaluaciónContinuaDeProveedores)
                {
                    var encuestaPreguntasResult = _context.tblEN_ResultadoProveedores.Where
                        (w =>
                            w.encuestaID == encuestaID &&
                            w.Detalle.numProveedor == numProv &&
                            w.Detalle.estadoEncuesta &&
                            w.Detalle.calificacion != null &&
                            listaUsuario.Contains(w.Detalle.evaluadorID)
                        ).ToList();

                    var result = encuestaPreguntasResult.GroupBy
                        (g =>
                            g.Detalle.fechaEvaluacion == null ? g.fecha.ToString("yyyy/MM") : g.Detalle.fechaEvaluacion.Value.ToString("yyyy/MM")
                        ).OrderBy(o => o.Key).ToList();
                    var preguntas = result.Select(m => m.Key.Substring(5) + "/" + m.Key.Substring(0,4));
                    var calificaciones = new List<decimal>();
                    foreach (var item in result)
                    {
                        calificaciones.Add(Math.Round(item.Sum(s => s.Detalle.calificacion.Value) / item.Count(), 2));
                    }

                    r.Success = true;
                    r.Message = "Ok";
                    r.Value = new { preguntas = preguntas, calificaciones = calificaciones };
                }
                else
                {
                    var encuestaPreguntasResult = _context.tblEN_ResultadoProveedorRequisiciones.Where
                        (w =>
                            w.encuestaID == encuestaID &&
                            w.Detalle.numProveedor == numProv &&
                            w.Detalle.estatus &&
                            w.Detalle.calificacion != null &&
                            listaUsuario.Contains(w.Detalle.evaluadorID)
                        ).ToList();

                    var result = encuestaPreguntasResult.GroupBy
                        (g =>
                            g.Detalle.fechaEvaluacion == null ? g.fecha.ToString("yyyy/MM") : g.Detalle.fechaEvaluacion.Value.ToString("yyyy/MM")
                        ).OrderBy(o => o.Key).ToList();
                    var preguntas = result.Select(m => m.Key.Substring(5) + "/" + m.Key.Substring(0, 4));
                    var calificaciones = new List<decimal>();
                    foreach (var item in result)
                    {
                        calificaciones.Add(Math.Round(item.Sum(s => s.Detalle.calificacion.Value) / item.Count(), 2));
                    }

                    r.Success = true;
                    r.Message = "Ok";
                    r.Value = new { preguntas = preguntas, calificaciones = calificaciones };
                }
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return r;
        }
    }
}
