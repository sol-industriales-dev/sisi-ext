using Core.DAO.Enkontrol.Almacen;
using Core.DTO;
using Core.DTO.Enkontrol.Alamcen;
using Core.DTO.Enkontrol.Requisicion;
using Core.DTO.RecursosHumanos;
using Core.DTO.Utils.Data;
using Core.Entity.Enkontrol.Compras.OrdenCompra;
using Core.Entity.Enkontrol.Compras.Requisicion;
using Core.Entity.RecursosHumanos.Catalogo;
using Core.Entity.StarSoft.Almacen;
using Core.Entity.StarSoft.Requisiciones;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Data.Factory.Principal.Usuarios;
using Infrastructure.Utils;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Odbc;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Data.DAO.Enkontrol.Almacen
{
    public class ResguardoDAO : GenericDAO<tblCom_Surtido>, IResguardoDAO
    {
        UsuarioFactoryServices ufs = new UsuarioFactoryServices();

        private bool productivo = Convert.ToString(System.Web.Configuration.WebConfigurationManager.AppSettings["enkontrolProductivo"]) == "1";

        private OdbcConnection checkConexionProductivo()
        {
            if (productivo)
            {
                return new Conexion().Connect();
            }
            else
            {
                return new Conexion().ConnectPrueba();
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

        private List<dynamic> consultaListCheckProductivo(List<string> listString)
        {
            if (productivo)
            {
                return _contextEnkontrol.Where(listString);
            }
            else
            {
                return _contextEnkontrolPrueba.Where(listString);
            }
        }

        public Dictionary<string, object> cambiarCCoEmpleado(int numEmpleado, int claveEmpleado, string ccNuevo, List<ResguardoEKDTO> resguardos)
        {
            var result = new Dictionary<string, object>();

            if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Peru)
            {
                using (var dbSigoplanTransaction = _context.Database.BeginTransaction())
                {
                    using (var con = checkConexionProductivo())
                    {
                        using (var trans = con.BeginTransaction())
                        {
                            try
                            {
                                var resultadoDevolucion = new Dictionary<string, object>();

                                if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Colombia)
                                {
                                    resultadoDevolucion = guardarDevolucion(resguardos, trans, dbSigoplanTransaction);
                                }
                                else
                                {
                                    resultadoDevolucion = guardarDevolucionColombia(resguardos, trans, dbSigoplanTransaction);
                                }

                                if ((bool)resultadoDevolucion[SUCCESS])
                                {
                                    foreach (var item in resguardos)
                                    {
                                        item.cc = ccNuevo;
                                        item.empleado = numEmpleado;
                                        item.claveEmpleado = claveEmpleado.ToString();
                                        item.estatus = "V";
                                        item.recibio = null;
                                        item.licencia = "S";
                                        item.recibio = null;
                                    }

                                    var resguardoGuardado = new Dictionary<string, object>();

                                    if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Colombia)
                                    {
                                        resguardoGuardado = guardarAsignacion(resguardos, true, trans, dbSigoplanTransaction);
                                    }
                                    else
                                    {
                                        resguardoGuardado = guardarAsignacionColombia(resguardos, true, trans, dbSigoplanTransaction);
                                    }

                                    if ((bool)resguardoGuardado[SUCCESS])
                                    {
                                        result.Add(SUCCESS, true);
                                        result.Add("folio", resguardoGuardado["folio"].ToString());
                                    }
                                    else
                                    {
                                        result.Add(SUCCESS, false);
                                        result.Add(MESSAGE, resguardoGuardado[MESSAGE].ToString());
                                    }
                                }
                                else
                                {
                                    result.Add(SUCCESS, false);
                                    result.Add(MESSAGE, resultadoDevolucion[MESSAGE].ToString());
                                }

                                trans.Commit();
                                dbSigoplanTransaction.Commit();
                            }
                            catch (Exception e)
                            {
                                trans.Rollback();
                                dbSigoplanTransaction.Rollback();

                                LogError(12, 12, "ResguardoController", "cambiarCCoEmpleado", e, AccionEnum.AGREGAR, 0, resguardos);

                                result.Add(MESSAGE, e.Message);
                                result.Add(SUCCESS, false);
                            }
                        }
                    }
                }
            }
            else
            {
                var resultadoDevolucion = guardarDevolucionPeru(resguardos);

                if ((bool)resultadoDevolucion[SUCCESS])
                {
                    foreach (var item in resguardos)
                    {
                        item.cc = ccNuevo;
                        item.empleado = numEmpleado;
                        item.claveEmpleado = claveEmpleado.ToString();
                        item.estatus = "V";
                        item.recibio = null;
                        item.licencia = "S";
                        item.recibio = null;
                    }

                    var resguardoGuardado = guardarAsignacionPeru(resguardos, true);

                    if ((bool)resguardoGuardado[SUCCESS])
                    {
                        result.Add(SUCCESS, true);
                        result.Add("folio", resguardoGuardado["folio"].ToString());
                    }
                    else
                    {
                        result.Add(SUCCESS, false);
                        result.Add(MESSAGE, resguardoGuardado[MESSAGE].ToString());
                    }
                }
                else
                {
                    result.Add(SUCCESS, false);
                    result.Add(MESSAGE, resultadoDevolucion[MESSAGE].ToString());
                }

            }

            return result;
        }

        public Dictionary<string, object> guardarAsignacionNormal(List<ResguardoEKDTO> resguardos)
        {
            var result = new Dictionary<string, object>();

            if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Peru)
            {
                using (var dbSigoplanTransaction = _context.Database.BeginTransaction())
                {
                    using (var con = checkConexionProductivo())
                    {
                        using (var trans = con.BeginTransaction())
                        {
                            try
                            {
                                var r = new Dictionary<string, object>();

                                if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Colombia)
                                {
                                    r = guardarAsignacion(resguardos, false, trans, dbSigoplanTransaction);
                                }
                                else
                                {
                                    r = guardarAsignacionColombia(resguardos, false, trans, dbSigoplanTransaction);
                                }

                                trans.Commit();
                                dbSigoplanTransaction.Commit();

                                SaveBitacora(12, (int)AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(resguardos));

                                result.Add("data", resguardos);
                                result.Add("folio", Convert.ToInt32(r["folio"]));
                                result.Add(SUCCESS, true);
                            }
                            catch (Exception e)
                            {
                                trans.Rollback();
                                dbSigoplanTransaction.Rollback();

                                LogError(12, 12, "ResguardoController", "guardarAsignacionNormal", e, AccionEnum.AGREGAR, 0, resguardos);

                                result.Add(MESSAGE, e.Message);
                                result.Add(SUCCESS, false);
                            }
                        }
                    }
                }
            }
            else
            {
                result = guardarAsignacionPeru(resguardos, false);
            }

            return result;
        }

        public Dictionary<string, object> guardarAsignacion(List<ResguardoEKDTO> resguardos, bool cambio, OdbcTransaction trans, DbContextTransaction dbSigoplanTransaction)
        {
            var result = new Dictionary<string, object>();
            var folioMovimiento = 0;

            #region Validación mismo insumo en diferentes partidas
            var listaResguardosAgrupados = resguardos.GroupBy(x => x.id_activo).Select(x => new { insumo = x.Key, contador = x.ToList().Count() }).ToList();

            if (listaResguardosAgrupados.Any(x => x.contador > 1))
            {
                throw new Exception("No puede capturar insumos repetidos en diferentes partidas. Debe juntarlos en una misma partida.");
            }
            #endregion

            #region SE VERIFICA SI EXISTE EL RESGUARDO
            List<dynamic> objResguardo = new List<dynamic>();

            string strQuery = @"SELECT cc, folio FROM si_resguardo_activo_fijo WHERE cc = '{0}' AND folio = {1}";
            var odbc = new OdbcConsultaDTO() { consulta = strQuery };
            odbc.consulta = String.Format(strQuery, resguardos[0].cc, resguardos[0].folio);

            objResguardo = _contextEnkontrol.Select<dynamic>(getEnkontrolEnumADM(), odbc);
            #endregion

            if (objResguardo.Count() > 0 && !cambio)
            {
                #region SE ACTUALIZA EL RESGUARDO EN ENKONTROL
                result.Add("esActualizar", true);
                folioMovimiento = resguardos[0].folio;

                for (int i = 0; i < resguardos.Count(); i++)
                {
                    #region VALIDACIONES
                    if (string.IsNullOrEmpty(resguardos[i].condiciones))
                    {
                        throw new Exception("Es necesario indicar la condición");
                    }
                    #endregion

                    var strQueryActualizarResguardo = string.Format(@"UPDATE si_resguardo_activo_fijo 
                                                                                    SET marca = ?,
                                                                                        modelo = ?,
                                                                                        color = ?,
                                                                                        num_serie = ?,
                                                                                        valor_activo = ?,
                                                                                        costo_promedio = ?,
                                                                                        plan_desc = ?,
                                                                                        condiciones = ?
                                                                                        WHERE cc = ? AND folio = ? AND id_activo = ?");

                    using (var cmd = new OdbcCommand(strQueryActualizarResguardo))
                    {
                        OdbcParameterCollection parameters = cmd.Parameters;

                        string strMensajeError = string.Empty;
                        parameters.Add("@marca", OdbcType.VarChar).Value = resguardos[i].marca ?? string.Empty;
                        parameters.Add("@modelo", OdbcType.VarChar).Value = resguardos[i].modelo ?? string.Empty;
                        parameters.Add("@color", OdbcType.VarChar).Value = resguardos[i].color ?? string.Empty;
                        parameters.Add("@num_serie", OdbcType.VarChar).Value = resguardos[i].num_serie ?? string.Empty;
                        parameters.Add("@valor_activo", OdbcType.Numeric).Value = resguardos[i].valor_activo ?? (object)DBNull.Value;
                        parameters.Add("@costo_promedio", OdbcType.Numeric).Value = resguardos[i].costo_promedio ?? (object)DBNull.Value;
                        parameters.Add("@plan_desc", OdbcType.VarChar).Value = resguardos[i].plan_desc ?? string.Empty;
                        parameters.Add("@condiciones", OdbcType.Char).Value = resguardos[i].condiciones;

                        parameters.Add("@cc", OdbcType.Char).Value = resguardos[i].cc;
                        parameters.Add("@folio", OdbcType.Numeric).Value = resguardos[i].folio;
                        parameters.Add("@id_activo", OdbcType.Numeric).Value = resguardos[i].id_activo;

                        if (!string.IsNullOrEmpty(strMensajeError))
                        {
                            throw new Exception(strMensajeError);
                        }

                        cmd.Connection = trans.Connection;
                        cmd.Transaction = trans;
                        cmd.ExecuteNonQuery();
                    }
                }
                #endregion

                #region SE ACTUALIZA RESGUARDO EN SIGOPLAN
                string cc = resguardos[0].cc;
                int folio = folio = resguardos[0].folio;
                List<tblAlm_Resguardo> lstResguardos = _context.tblAlm_Resguardo.Where(w => w.cc == cc && w.folio == folio).ToList();

                for (int i = 0; i < resguardos.Count(); i++)
                {
                    tblAlm_Resguardo objActualizarResguardo = lstResguardos.Where(w => w.id_activo == resguardos[i].id_activo).FirstOrDefault();

                    if (objActualizarResguardo != null)
                    {
                        objActualizarResguardo.marca = resguardos[i].marca ?? string.Empty;
                        objActualizarResguardo.modelo = resguardos[i].modelo ?? string.Empty;
                        objActualizarResguardo.color = resguardos[i].color ?? string.Empty;
                        objActualizarResguardo.num_serie = resguardos[i].num_serie ?? string.Empty;
                        objActualizarResguardo.valor_activo = resguardos[i].valor_activo;
                        objActualizarResguardo.costo_promedio = resguardos[i].costo_promedio;
                        objActualizarResguardo.plan_desc = resguardos[i].plan_desc ?? string.Empty;
                        objActualizarResguardo.condiciones = resguardos[i].condiciones;
                        _context.SaveChanges();
                    }
                }
                #endregion
            }
            else
            {
                #region SE CREA EL RESGUARDO
                result.Add("esActualizar", false);
                //#region Validación Centro de Costo con Presupuesto Terminado
                //var centroCostoEK = ((List<dynamic>)consultaCheckProductivo(string.Format(@"SELECT * FROM cc WHERE cc = '{0}'", resguardos[0].cc)).ToObject<List<dynamic>>())[0];
                //var presupuestoEstatus = (string)centroCostoEK.st_ppto;

                //if (presupuestoEstatus == "T")
                //{
                //    throw new Exception("El Centro de Costos tiene el presupuesto terminado.");
                //}
                //#endregion

                #region Validación ubicaciones diferentes a null o vacías
                if (resguardos.Any(x => x.area_alm == "" || x.lado_alm == "" || x.estante_alm == "" || x.nivel_alm == "" ||
                                        x.area_alm == null || x.lado_alm == null || x.estante_alm == null || x.nivel_alm == null))
                {
                    throw new Exception("No se capturó una ubicación válida.");
                }
                #endregion

                #region Validación Empleado (Usuario Enkontrol) Resguardo Capturado
                if (resguardos.Any(x => x.empleado == 0))
                {
                    throw new Exception("No se capturó el empleado a resguardar. No se puede guardar la información.");
                }
                #endregion

                #region Validación Empleado (Usuario Enkontrol) Existente
                foreach (var res in resguardos)
                {
                    var empleadoEnkontrol = consultaCheckProductivo(
                        string.Format(@"SELECT 
                                                    * 
                                                FROM empleados 
                                                WHERE empleado = {0}", res.empleado)
                    );

                    if (empleadoEnkontrol == null)
                    {
                        throw new Exception("No se encuentra el empleado con el número \"" + res.empleado + "\".");
                    }
                }
                #endregion

                #region Validar Existencias
                foreach (var det in resguardos)
                {
                    var entradasEK = consultaCheckProductivo(
                        string.Format(@"SELECT 
                                        det.insumo, 
                                        (CAST(det.insumo AS Varchar) + '-' + ins.descripcion) AS insumoDesc, 
                                        SUM(det.cantidad) AS cantidad, 
                                        det.area_alm, 
                                        det.lado_alm, 
                                        det.estante_alm, 
                                        det.nivel_alm 
                                    FROM si_movimientos mov 
                                        INNER JOIN si_movimientos_det det ON mov.almacen = det.almacen AND mov.tipo_mov = det.tipo_mov AND mov.numero = det.numero 
                                        INNER JOIN insumos ins ON det.insumo = ins.insumo 
                                    WHERE 
                                        mov.almacen = {0} AND 
                                        det.insumo = {1} AND 
                                        det.area_alm = '{2}' AND 
                                        det.lado_alm = '{3}' AND 
                                        det.estante_alm = '{4}' AND 
                                        det.nivel_alm = '{5}' AND 
                                        det.tipo_mov < 50 
                                    GROUP BY det.insumo, ins.descripcion, det.area_alm, det.lado_alm, det.estante_alm, det.nivel_alm",
                                        det.alm_salida, det.id_activo, det.area_alm, det.lado_alm, det.estante_alm, det.nivel_alm));

                    var salidasEK = consultaCheckProductivo(
                        string.Format(@"SELECT 
                                        det.insumo, 
                                        (CAST(det.insumo AS Varchar) + '-' + ins.descripcion) AS insumoDesc, 
                                        SUM(det.cantidad) AS cantidad, 
                                        det.area_alm, 
                                        det.lado_alm, 
                                        det.estante_alm, 
                                        det.nivel_alm 
                                    FROM si_movimientos mov 
                                        INNER JOIN si_movimientos_det det ON mov.almacen = det.almacen AND mov.tipo_mov = det.tipo_mov AND mov.numero = det.numero 
                                        INNER JOIN insumos ins ON det.insumo = ins.insumo 
                                    WHERE 
                                        mov.almacen = {0} AND 
                                        det.insumo = {1} AND 
                                        det.area_alm = '{2}' AND 
                                        det.lado_alm = '{3}' AND 
                                        det.estante_alm = '{4}' AND 
                                        det.nivel_alm = '{5}' AND 
                                        det.tipo_mov > 50 
                                    GROUP BY det.insumo, ins.descripcion, det.area_alm, det.lado_alm, det.estante_alm, det.nivel_alm",
                                        det.alm_salida, det.id_activo, det.area_alm, det.lado_alm, det.estante_alm, det.nivel_alm));

                    if (entradasEK != null)
                    {
                        var entradas = (List<UbicacionDetalleDTO>)entradasEK.ToObject<List<UbicacionDetalleDTO>>();

                        if (salidasEK != null)
                        {
                            var salidas = (List<UbicacionDetalleDTO>)salidasEK.ToObject<List<UbicacionDetalleDTO>>();

                            foreach (var ent in entradas)
                            {
                                var salida = salidas.FirstOrDefault(x =>
                                        x.insumoDesc == ent.insumoDesc &&
                                        x.area_alm == ent.area_alm &&
                                        x.lado_alm == ent.lado_alm &&
                                        x.estante_alm == ent.estante_alm &&
                                        x.nivel_alm == ent.nivel_alm
                                    );

                                if (salida != null)
                                {
                                    ent.cantidad = ent.cantidad - Convert.ToDecimal(salida.cantidad, CultureInfo.InvariantCulture);
                                }
                            }
                        }

                        var existenciasInsumo = entradas.Sum(x => x.cantidad);
                        var salidaTotalPorInsumo = resguardos.Where(x =>
                            x.id_activo == det.id_activo &&
                            x.area_alm == det.area_alm &&
                            x.lado_alm == det.lado_alm &&
                            x.estante_alm == det.estante_alm &&
                            x.nivel_alm == det.nivel_alm).Sum(x => x.cantidad_resguardo);

                        if (existenciasInsumo < salidaTotalPorInsumo)
                        {
                            var ubicacion = det.area_alm + "-" + det.lado_alm + "-" + det.estante_alm + "-" + det.nivel_alm;

                            throw new Exception("No se guardó la información. La cantidad solicitada del insumo \"" + det.id_activo + "\" sobrepasa las existencias (" + existenciasInsumo + ") en la ubicación \"" + ubicacion + "\".");
                        }
                    }
                    else
                    {
                        var ubicacion = det.area_alm + "-" + det.lado_alm + "-" + det.estante_alm + "-" + det.nivel_alm;

                        throw new Exception("No se guardó la información. No hay existencias para el insumo \"" + det.id_activo + "\" en la ubicación \"" + ubicacion + "\".");
                    }
                }
                #endregion

                var count = 0;

                var usuario = vSesiones.sesionUsuarioDTO;
                var relUser = ufs.getUsuarioService().getUserEk(usuario.id);

                if (relUser != null)
                {
                    var nuevoFolio = 1;

                    var ultimoFolioEK = consultaCheckProductivo(
                        string.Format(@"SELECT TOP 1 
                                                    folio 
                                                FROM si_resguardo_activo_fijo 
                                                WHERE alm_salida = {0} 
                                                ORDER BY folio DESC", resguardos[0].alm_salida)
                    );

                    if (ultimoFolioEK != null)
                    {
                        nuevoFolio = (int)(((List<dynamic>)ultimoFolioEK.ToObject<List<dynamic>>())[0].folio) + 1;
                    }

                    folioMovimiento = guardarSalidaTraspasoResguardo(resguardos, nuevoFolio, trans);

                    foreach (var resguardo in resguardos)
                    {
                        resguardo.folio = folioMovimiento; //nuevoFolio;

                        var nuevoResguardo = new tblAlm_Resguardo
                        {
                            cc = resguardo.cc,
                            folio = folioMovimiento, //folio = nuevoFolio,
                            id_activo = resguardo.id_activo,
                            id_tipo_activo = resguardo.id_tipo_activo,
                            marca = resguardo.marca,
                            modelo = resguardo.modelo,
                            color = resguardo.color,
                            num_serie = resguardo.num_serie,
                            valor_activo = resguardo.valor_activo,
                            compania = resguardo.compania,
                            plan_desc = resguardo.plan_desc,
                            condiciones = resguardo.condiciones,
                            numpro = resguardo.numpro,
                            factura = resguardo.factura,
                            fec_factura = resguardo.fec_factura,
                            empleado = resguardo.empleado,
                            empleadoSIGOPLAN = null,
                            claveEmpleado = resguardo.claveEmpleado,
                            licencia = resguardo.licencia,
                            tipo = resguardo.tipo,
                            fec_licencia = resguardo.fec_licencia,
                            observaciones = resguardo.observaciones,
                            fec_resguardo = DateTime.Now.Date,
                            foto = resguardo.foto,
                            estatus = resguardo.estatus,
                            entrega = relUser.empleado,
                            entregaSIGOPLAN = relUser.idUsuario,
                            autoriza = resguardo.autoriza,
                            autorizaSIGOPLAN = null,
                            recibio = null, //recibio = resguardo.recibio,
                            recibioSIGOPLAN = null,
                            condiciones_ret = resguardo.condiciones_ret,
                            fec_devolucion = resguardo.fec_devolucion,
                            cantidad_resguardo = resguardo.cantidad_resguardo,
                            alm_salida = resguardo.alm_salida,
                            alm_entrada = resguardo.alm_entrada,
                            foto_2 = resguardo.foto_2,
                            foto_3 = resguardo.foto_3,
                            foto_4 = resguardo.foto_4,
                            foto_5 = resguardo.foto_5,
                            costo_promedio = resguardo.costo_promedio,
                            resguardo_parcial = resguardo.resguardo_parcial,
                            estatusRegistro = true
                        };

                        _context.tblAlm_Resguardo.Add(nuevoResguardo);
                        _context.SaveChanges();

                        var consulta = @"
                                        INSERT INTO si_resguardo_activo_fijo 
                                        (cc, folio, id_activo, id_tipo_activo, marca, modelo, color, num_serie, valor_activo, 
                                        compania, plan_desc, condiciones, numpro, factura, fec_factura, empleado, licencia, tipo, fec_licencia, 
                                        observaciones, fec_resguardo, foto, estatus, entrega, autoriza, recibio, condiciones_ret, fec_devolucion, 
                                        cantidad_resguardo, alm_salida, alm_entrada, foto_2, foto_3, foto_4, foto_5, costo_promedio, resguardo_parcial) 
                                        VALUES (?,?,?,?,?,?,?,?,?,?, 
                                                ?,?,?,?,?,?,?,?,?,?, 
                                                ?,?,?,?,?,?,?,?,?,?, 
                                                ?,?,?,?,?,?,?)";

                        using (var cmd = new OdbcCommand(consulta))
                        {
                            OdbcParameterCollection parameters = cmd.Parameters;

                            parameters.Add("@cc", OdbcType.Char).Value = nuevoResguardo.cc;
                            parameters.Add("@folio", OdbcType.Numeric).Value = nuevoResguardo.folio;
                            parameters.Add("@id_activo", OdbcType.Numeric).Value = nuevoResguardo.id_activo;
                            parameters.Add("@id_tipo_activo", OdbcType.Numeric).Value = nuevoResguardo.id_tipo_activo;
                            parameters.Add("@marca", OdbcType.VarChar).Value = nuevoResguardo.marca ?? (object)DBNull.Value;
                            parameters.Add("@modelo", OdbcType.VarChar).Value = nuevoResguardo.modelo ?? (object)DBNull.Value;
                            parameters.Add("@color", OdbcType.VarChar).Value = nuevoResguardo.color ?? (object)DBNull.Value;
                            parameters.Add("@num_serie", OdbcType.VarChar).Value = nuevoResguardo.num_serie ?? (object)DBNull.Value;
                            parameters.Add("@valor_activo", OdbcType.Numeric).Value = nuevoResguardo.valor_activo ?? (object)DBNull.Value;
                            parameters.Add("@compania", OdbcType.VarChar).Value = DBNull.Value;
                            parameters.Add("@plan_desc", OdbcType.VarChar).Value = nuevoResguardo.plan_desc ?? (object)DBNull.Value;
                            parameters.Add("@condiciones", OdbcType.Char).Value = nuevoResguardo.condiciones ?? "";
                            parameters.Add("@numpro", OdbcType.Numeric).Value = DBNull.Value;
                            parameters.Add("@factura", OdbcType.Numeric).Value = DBNull.Value;
                            parameters.Add("@fec_factura", OdbcType.Date).Value = DBNull.Value;
                            parameters.Add("@empleado", OdbcType.Numeric).Value = nuevoResguardo.empleado;
                            parameters.Add("@licencia", OdbcType.Char).Value = nuevoResguardo.licencia ?? "";
                            parameters.Add("@tipo", OdbcType.VarChar).Value = DBNull.Value;
                            parameters.Add("@fec_licencia", OdbcType.Date).Value = DBNull.Value;
                            parameters.Add("@observaciones", OdbcType.VarChar).Value = DBNull.Value;
                            parameters.Add("@fec_resguardo", OdbcType.Date).Value = nuevoResguardo.fec_resguardo;
                            parameters.Add("@foto", OdbcType.VarChar).Value = DBNull.Value;
                            parameters.Add("@estatus", OdbcType.Char).Value = nuevoResguardo.estatus ?? "";
                            parameters.Add("@entrega", OdbcType.Numeric).Value = nuevoResguardo.entrega;
                            parameters.Add("@autoriza", OdbcType.Numeric).Value = nuevoResguardo.autoriza;
                            parameters.Add("@recibio", OdbcType.Numeric).Value = nuevoResguardo.recibio ?? (object)DBNull.Value;
                            parameters.Add("@condiciones_ret", OdbcType.Char).Value = nuevoResguardo.condiciones_ret ?? "";
                            parameters.Add("@fec_devolucion", OdbcType.Date).Value = nuevoResguardo.fec_devolucion ?? (object)DBNull.Value;
                            parameters.Add("@cantidad_resguardo", OdbcType.Numeric).Value = nuevoResguardo.cantidad_resguardo;
                            parameters.Add("@alm_salida", OdbcType.Numeric).Value = nuevoResguardo.alm_salida;
                            parameters.Add("@alm_entrada", OdbcType.Numeric).Value = nuevoResguardo.alm_entrada;
                            parameters.Add("@foto_2", OdbcType.VarChar).Value = DBNull.Value;
                            parameters.Add("@foto_3", OdbcType.VarChar).Value = DBNull.Value;
                            parameters.Add("@foto_4", OdbcType.VarChar).Value = DBNull.Value;
                            parameters.Add("@foto_5", OdbcType.VarChar).Value = DBNull.Value;
                            parameters.Add("@costo_promedio", OdbcType.Numeric).Value = nuevoResguardo.costo_promedio ?? (object)DBNull.Value;
                            parameters.Add("@resguardo_parcial", OdbcType.Numeric).Value = nuevoResguardo.resguardo_parcial ?? (object)DBNull.Value;

                            cmd.Connection = trans.Connection;
                            cmd.Transaction = trans;

                            count += cmd.ExecuteNonQuery();
                        }
                    }
                }
                else
                {
                    throw new Exception("No se encuentra la relación SIGOPLAN-Enkontrol del usuario logeado.");
                }
                #endregion
            }

            result.Add("folio", folioMovimiento);
            result.Add(SUCCESS, true);

            return result;
        }

        public Dictionary<string, object> guardarAsignacionColombia(List<ResguardoEKDTO> resguardos, bool cambio, OdbcTransaction trans, DbContextTransaction dbSigoplanTransaction)
        {
            var result = new Dictionary<string, object>();
            var folioMovimiento = 0;
            var resguardo_cc = resguardos[0].cc;
            var resguardo_folio = resguardos[0].folio;
            var resguardo_almacen = resguardos[0].alm_salida;
            var objResguardo = _context.tblAlm_Resguardo.FirstOrDefault(x => x.estatusRegistro && x.cc == resguardo_cc && x.folio == resguardo_folio && x.alm_salida == resguardo_almacen);

            #region Validación mismo insumo en diferentes partidas
            var listaResguardosAgrupados = resguardos.GroupBy(x => x.id_activo).Select(x => new { insumo = x.Key, contador = x.ToList().Count() }).ToList();

            if (listaResguardosAgrupados.Any(x => x.contador > 1))
            {
                throw new Exception("No puede capturar insumos repetidos en diferentes partidas. Debe juntarlos en una misma partida.");
            }
            #endregion

            if (objResguardo != null && !cambio)
            {
                #region SE ACTUALIZA EL RESGUARDO EN SIGOPLAN
                string cc = resguardos[0].cc;
                int folio = folio = resguardos[0].folio;
                List<tblAlm_Resguardo> lstResguardos = _context.tblAlm_Resguardo.Where(w => w.cc == cc && w.folio == folio).ToList();

                for (int i = 0; i < resguardos.Count(); i++)
                {
                    tblAlm_Resguardo objActualizarResguardo = lstResguardos.Where(w => w.id_activo == resguardos[i].id_activo).FirstOrDefault();

                    if (objActualizarResguardo != null)
                    {
                        objActualizarResguardo.marca = resguardos[i].marca ?? string.Empty;
                        objActualizarResguardo.modelo = resguardos[i].modelo ?? string.Empty;
                        objActualizarResguardo.color = resguardos[i].color ?? string.Empty;
                        objActualizarResguardo.num_serie = resguardos[i].num_serie ?? string.Empty;
                        objActualizarResguardo.valor_activo = resguardos[i].valor_activo;
                        objActualizarResguardo.costo_promedio = resguardos[i].costo_promedio;
                        objActualizarResguardo.plan_desc = resguardos[i].plan_desc ?? string.Empty;
                        objActualizarResguardo.condiciones = resguardos[i].condiciones;
                        _context.SaveChanges();
                    }
                }
                #endregion
            }
            else
            {
                #region SE CREA EL RESGUARDO EN SIGOPLAN
                result.Add("esActualizar", false);
                //#region Validación Centro de Costo con Presupuesto Terminado
                //var centroCostoEK = ((List<dynamic>)consultaCheckProductivo(string.Format(@"SELECT * FROM cc WHERE cc = '{0}'", resguardos[0].cc)).ToObject<List<dynamic>>())[0];
                //var presupuestoEstatus = (string)centroCostoEK.st_ppto;

                //if (presupuestoEstatus == "T")
                //{
                //    throw new Exception("El Centro de Costos tiene el presupuesto terminado.");
                //}
                //#endregion

                #region Validación ubicaciones diferentes a null o vacías
                if (resguardos.Any(x => x.area_alm == "" || x.lado_alm == "" || x.estante_alm == "" || x.nivel_alm == "" ||
                                        x.area_alm == null || x.lado_alm == null || x.estante_alm == null || x.nivel_alm == null))
                {
                    throw new Exception("No se capturó una ubicación válida.");
                }
                #endregion

                #region Validación Empleado (Usuario Enkontrol) Resguardo Capturado
                if (resguardos.Any(x => x.empleado == 0))
                {
                    throw new Exception("No se capturó el empleado a resguardar. No se puede guardar la información.");
                }
                #endregion

                #region Validación Empleado (Usuario Enkontrol) Existente
                foreach (var res in resguardos)
                {
                    var empleadoEnkontrol = consultaCheckProductivo(
                        string.Format(@"SELECT 
                                                    * 
                                                FROM empleados 
                                                WHERE empleado = {0}", res.empleado)
                    );

                    if (empleadoEnkontrol == null)
                    {
                        throw new Exception("No se encuentra el empleado con el número \"" + res.empleado + "\".");
                    }
                }
                #endregion

                #region Validar Existencias
                foreach (var det in resguardos)
                {
                    var entradasSIGOPLAN = _context.tblAlm_MovimientosDet.Where(x =>
                        x.estatusHabilitado &&
                        x.tipo_mov < 50 &&
                        x.almacen == det.alm_salida &&
                        x.insumo == det.id_activo &&
                        x.area_alm == det.area_alm &&
                        x.lado_alm == det.lado_alm &&
                        x.estante_alm == det.estante_alm &&
                        x.nivel_alm == det.nivel_alm
                    ).ToList().Select(x => new UbicacionDetalleDTO
                    {
                        insumo = x.insumo,
                        cantidad = x.cantidad,
                        area_alm = x.area_alm,
                        lado_alm = x.lado_alm,
                        estante_alm = x.estante_alm,
                        nivel_alm = x.nivel_alm
                    }).GroupBy(x => new { x.insumo, x.area_alm, x.lado_alm, x.estante_alm, x.nivel_alm }).Select(x => new UbicacionDetalleDTO
                    {
                        insumo = x.Key.insumo,
                        cantidad = x.Sum(y => y.cantidad),
                        area_alm = x.Key.area_alm,
                        lado_alm = x.Key.lado_alm,
                        estante_alm = x.Key.estante_alm,
                        nivel_alm = x.Key.nivel_alm
                    }).ToList();

                    var salidasSIGOPLAN = _context.tblAlm_MovimientosDet.Where(x =>
                        x.estatusHabilitado &&
                        x.tipo_mov > 50 &&
                        x.almacen == det.alm_salida &&
                        x.insumo == det.id_activo &&
                        x.area_alm == det.area_alm &&
                        x.lado_alm == det.lado_alm &&
                        x.estante_alm == det.estante_alm &&
                        x.nivel_alm == det.nivel_alm
                    ).ToList().Select(x => new UbicacionDetalleDTO
                    {
                        insumo = x.insumo,
                        cantidad = x.cantidad,
                        area_alm = x.area_alm,
                        lado_alm = x.lado_alm,
                        estante_alm = x.estante_alm,
                        nivel_alm = x.nivel_alm
                    }).GroupBy(x => new { x.insumo, x.area_alm, x.lado_alm, x.estante_alm, x.nivel_alm }).Select(x => new UbicacionDetalleDTO
                    {
                        insumo = x.Key.insumo,
                        cantidad = x.Sum(y => y.cantidad),
                        area_alm = x.Key.area_alm,
                        lado_alm = x.Key.lado_alm,
                        estante_alm = x.Key.estante_alm,
                        nivel_alm = x.Key.nivel_alm
                    }).ToList();

                    if (entradasSIGOPLAN.Count() > 0)
                    {
                        if (salidasSIGOPLAN.Count() > 0)
                        {
                            foreach (var ent in entradasSIGOPLAN)
                            {
                                var salida = salidasSIGOPLAN.FirstOrDefault(x =>
                                    x.insumo == ent.insumo &&
                                    x.area_alm == ent.area_alm &&
                                    x.lado_alm == ent.lado_alm &&
                                    x.estante_alm == ent.estante_alm &&
                                    x.nivel_alm == ent.nivel_alm
                                );

                                if (salida != null)
                                {
                                    ent.cantidad = ent.cantidad - Convert.ToDecimal(salida.cantidad, CultureInfo.InvariantCulture);
                                }
                            }
                        }

                        var existenciasInsumo = entradasSIGOPLAN.Sum(x => x.cantidad);
                        var salidaTotalPorInsumo = resguardos.Where(x =>
                            x.id_activo == det.id_activo &&
                            x.area_alm == det.area_alm &&
                            x.lado_alm == det.lado_alm &&
                            x.estante_alm == det.estante_alm &&
                            x.nivel_alm == det.nivel_alm).Sum(x => x.cantidad_resguardo);

                        if (existenciasInsumo < salidaTotalPorInsumo)
                        {
                            var ubicacion = det.area_alm + "-" + det.lado_alm + "-" + det.estante_alm + "-" + det.nivel_alm;

                            throw new Exception("No se guardó la información. La cantidad solicitada del insumo \"" + det.id_activo + "\" sobrepasa las existencias (" + existenciasInsumo + ") en la ubicación \"" + ubicacion + "\".");
                        }
                    }
                    else
                    {
                        var ubicacion = det.area_alm + "-" + det.lado_alm + "-" + det.estante_alm + "-" + det.nivel_alm;

                        throw new Exception("No se guardó la información. No hay existencias para el insumo \"" + det.id_activo + "\" en la ubicación \"" + ubicacion + "\".");
                    }
                }
                #endregion

                var count = 0;

                var usuario = vSesiones.sesionUsuarioDTO;
                var relUser = ufs.getUsuarioService().getUserEk(usuario.id);

                if (relUser != null)
                {
                    var nuevoFolio = 1;
                    var alm_salida = resguardos[0].alm_salida;
                    var ultimoFolio = _context.tblAlm_Resguardo.Where(x => x.estatusRegistro && x.alm_salida == alm_salida).OrderByDescending(x => x.folio).FirstOrDefault();

                    if (ultimoFolio != null)
                    {
                        nuevoFolio = ultimoFolio.folio + 1;
                    }

                    folioMovimiento = guardarSalidaTraspasoResguardoColombia(resguardos, nuevoFolio, trans);

                    foreach (var resguardo in resguardos)
                    {
                        resguardo.folio = folioMovimiento; //nuevoFolio;

                        var nuevoResguardo = new tblAlm_Resguardo
                        {
                            cc = resguardo.cc,
                            folio = folioMovimiento, //folio = nuevoFolio,
                            id_activo = resguardo.id_activo,
                            id_tipo_activo = resguardo.id_tipo_activo,
                            marca = resguardo.marca,
                            modelo = resguardo.modelo,
                            color = resguardo.color,
                            num_serie = resguardo.num_serie,
                            valor_activo = resguardo.valor_activo,
                            compania = resguardo.compania,
                            plan_desc = resguardo.plan_desc,
                            condiciones = resguardo.condiciones,
                            numpro = resguardo.numpro,
                            factura = resguardo.factura,
                            fec_factura = resguardo.fec_factura,
                            empleado = resguardo.empleado,
                            empleadoSIGOPLAN = null,
                            claveEmpleado = resguardo.claveEmpleado,
                            licencia = resguardo.licencia,
                            tipo = resguardo.tipo,
                            fec_licencia = resguardo.fec_licencia,
                            observaciones = resguardo.observaciones,
                            fec_resguardo = DateTime.Now.Date,
                            foto = resguardo.foto,
                            estatus = resguardo.estatus,
                            entrega = relUser.empleado,
                            entregaSIGOPLAN = relUser.idUsuario,
                            autoriza = resguardo.autoriza,
                            autorizaSIGOPLAN = null,
                            recibio = null, //recibio = resguardo.recibio,
                            recibioSIGOPLAN = null,
                            condiciones_ret = resguardo.condiciones_ret,
                            fec_devolucion = resguardo.fec_devolucion,
                            cantidad_resguardo = resguardo.cantidad_resguardo,
                            alm_salida = resguardo.alm_salida,
                            alm_entrada = resguardo.alm_entrada,
                            foto_2 = resguardo.foto_2,
                            foto_3 = resguardo.foto_3,
                            foto_4 = resguardo.foto_4,
                            foto_5 = resguardo.foto_5,
                            costo_promedio = resguardo.costo_promedio,
                            resguardo_parcial = resguardo.resguardo_parcial,
                            estatusRegistro = true
                        };

                        _context.tblAlm_Resguardo.Add(nuevoResguardo);
                        _context.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("No se encuentra la relación SIGOPLAN-Enkontrol del usuario logeado.");
                }
                #endregion
            }

            result.Add("folio", folioMovimiento);
            result.Add(SUCCESS, true);

            return result;
        }

        public Dictionary<string, object> guardarAsignacionPeru(List<ResguardoEKDTO> resguardos, bool cambio)
        {
            var result = new Dictionary<string, object>();

            using (var dbSigoplanTransaction = _context.Database.BeginTransaction())
            {
                using (var _starsoft = new MainContextPeruStarSoft003BDCOMUN())
                {
                    using (var dbStarsoftTransaction = _starsoft.Database.BeginTransaction())
                    {
                        try
                        {
                            #region Validación Usuario Inventarios Starsoft
                            var objUsrStarsoftInventarios = _context.tblAlm_Almacenistas.FirstOrDefault(e => e.id_usuario_sigoplan == vSesiones.sesionUsuarioDTO.id);

                            string idAlmacenistaStarsoft = "0";

                            if (objUsrStarsoftInventarios == null)
                            {
                                if (vSesiones.sesionUsuarioDTO.idPerfil == 1)
                                {
                                    idAlmacenistaStarsoft = "1";
                                }
                                else
                                {
                                    throw new Exception("Esta usuario no es un almacenista en el sistema");
                                }
                            }
                            else
                            {
                                idAlmacenistaStarsoft = objUsrStarsoftInventarios.id_usuario_inventarios;
                            }
                            #endregion

                            #region Validación mismo insumo en diferentes partidas
                            var listaResguardosAgrupados = resguardos.GroupBy(x => x.id_activo).Select(x => new { insumo = x.Key, contador = x.ToList().Count() }).ToList();

                            if (listaResguardosAgrupados.Any(x => x.contador > 1))
                            {
                                throw new Exception("No puede capturar insumos repetidos en diferentes partidas. Debe juntarlos en una misma partida.");
                            }
                            #endregion

                            string cc = resguardos[0].cc;
                            int folio = resguardos[0].folio;

                            var resguardoExistente = _context.tblAlm_Resguardo.FirstOrDefault(x => x.estatusRegistro && x.cc == cc && x.folio == folio);

                            if (resguardoExistente != null && !cambio)
                            {
                                result.Add("esActualizar", true);

                                #region Actualizar resguardo
                                List<tblAlm_Resguardo> lstResguardos = _context.tblAlm_Resguardo.Where(w => w.cc == cc && w.folio == folio).ToList();

                                for (int i = 0; i < resguardos.Count(); i++)
                                {
                                    tblAlm_Resguardo objActualizarResguardo = lstResguardos.Where(w => w.id_activo == resguardos[i].id_activo).FirstOrDefault();

                                    if (objActualizarResguardo != null)
                                    {
                                        objActualizarResguardo.marca = resguardos[i].marca ?? string.Empty;
                                        objActualizarResguardo.modelo = resguardos[i].modelo ?? string.Empty;
                                        objActualizarResguardo.color = resguardos[i].color ?? string.Empty;
                                        objActualizarResguardo.num_serie = resguardos[i].num_serie ?? string.Empty;
                                        objActualizarResguardo.valor_activo = resguardos[i].valor_activo;
                                        objActualizarResguardo.costo_promedio = resguardos[i].costo_promedio;
                                        objActualizarResguardo.plan_desc = resguardos[i].plan_desc ?? string.Empty;
                                        objActualizarResguardo.condiciones = resguardos[i].condiciones;
                                        _context.SaveChanges();
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                result.Add("esActualizar", false);

                                #region Crear Resguardo
                                #region Validación ubicaciones diferentes a null o vacías
                                if (resguardos.Any(x => x.area_alm == "" || x.lado_alm == "" || x.estante_alm == "" || x.nivel_alm == "" || x.area_alm == null || x.lado_alm == null || x.estante_alm == null || x.nivel_alm == null))
                                {
                                    throw new Exception("No se capturó una ubicación válida.");
                                }
                                #endregion

                                #region Validación Empleado (Usuario SIGOPLAN) Resguardo Capturado
                                if (resguardos.Any(x => x.empleado == 0))
                                {
                                    throw new Exception("No se capturó el empleado a resguardar. No se puede guardar la información.");
                                }
                                #endregion

                                #region Validación Empleado (Usuario SIGOPLAN) Existente
                                foreach (var res in resguardos)
                                {
                                    var registroUsuario = _context.tblAlm_EmpleadoResguardo.FirstOrDefault(x => x.registroActivo && x.folio == res.empleado);

                                    if (registroUsuario == null)
                                    {
                                        throw new Exception("No se encuentra el usuario con el número \"" + res.empleado + "\".");
                                    }
                                }
                                #endregion

                                #region Validar Existencias
                                foreach (var det in resguardos)
                                {
                                    var entradas = _context.tblAlm_MovimientosDet.Where(x =>
                                        x.estatusHabilitado &&
                                        x.almacen == det.alm_salida &&
                                        x.insumo == det.id_activo &&
                                        x.area_alm == det.area_alm &&
                                        x.lado_alm == det.lado_alm &&
                                        x.estante_alm == det.estante_alm &&
                                        x.nivel_alm == det.nivel_alm &&
                                        x.tipo_mov < 50
                                    ).GroupBy(x => new { x.insumo, x.area_alm, x.lado_alm, x.estante_alm, x.nivel_alm }).Select(x => new UbicacionDetalleDTO
                                    {
                                        insumo = x.Key.insumo,
                                        area_alm = x.Key.area_alm,
                                        lado_alm = x.Key.lado_alm,
                                        estante_alm = x.Key.estante_alm,
                                        nivel_alm = x.Key.nivel_alm,
                                        cantidad = x.Sum(y => y.cantidad)
                                    }).ToList();

                                    var salidas = _context.tblAlm_MovimientosDet.Where(x =>
                                        x.estatusHabilitado &&
                                        x.almacen == det.alm_salida &&
                                        x.insumo == det.id_activo &&
                                        x.area_alm == det.area_alm &&
                                        x.lado_alm == det.lado_alm &&
                                        x.estante_alm == det.estante_alm &&
                                        x.nivel_alm == det.nivel_alm &&
                                        x.tipo_mov > 50
                                    ).GroupBy(x => new { x.insumo, x.area_alm, x.lado_alm, x.estante_alm, x.nivel_alm }).Select(x => new UbicacionDetalleDTO
                                    {
                                        insumo = x.Key.insumo,
                                        area_alm = x.Key.area_alm,
                                        lado_alm = x.Key.lado_alm,
                                        estante_alm = x.Key.estante_alm,
                                        nivel_alm = x.Key.nivel_alm,
                                        cantidad = x.Sum(y => y.cantidad)
                                    }).ToList();

                                    if (entradas.Count() > 0)
                                    {
                                        if (salidas.Count() > 0)
                                        {
                                            foreach (var ent in entradas)
                                            {
                                                var salida = salidas.FirstOrDefault(x =>
                                                    x.insumo == ent.insumo &&
                                                    x.area_alm == ent.area_alm &&
                                                    x.lado_alm == ent.lado_alm &&
                                                    x.estante_alm == ent.estante_alm &&
                                                    x.nivel_alm == ent.nivel_alm
                                                );

                                                if (salida != null)
                                                {
                                                    ent.cantidad = ent.cantidad - salida.cantidad;
                                                }
                                            }
                                        }

                                        var existenciasInsumo = entradas.Sum(x => x.cantidad);
                                        var salidaTotalPorInsumo = resguardos.Where(x =>
                                            x.id_activo == det.id_activo &&
                                            x.area_alm == det.area_alm &&
                                            x.lado_alm == det.lado_alm &&
                                            x.estante_alm == det.estante_alm &&
                                            x.nivel_alm == det.nivel_alm
                                        ).Sum(x => x.cantidad_resguardo);

                                        if (existenciasInsumo < salidaTotalPorInsumo)
                                        {
                                            var ubicacion = det.area_alm + "-" + det.lado_alm + "-" + det.estante_alm + "-" + det.nivel_alm;

                                            throw new Exception("No se guardó la información. La cantidad solicitada del insumo \"0" + det.id_activo + "\" sobrepasa las existencias (" + existenciasInsumo + ") en la ubicación \"" + ubicacion + "\".");
                                        }
                                    }
                                    else
                                    {
                                        var ubicacion = det.area_alm + "-" + det.lado_alm + "-" + det.estante_alm + "-" + det.nivel_alm;

                                        throw new Exception("No se guardó la información. No hay existencias para el insumo \"0" + det.id_activo + "\" en la ubicación \"" + ubicacion + "\".");
                                    }
                                }
                                #endregion

                                var relUser = ufs.getUsuarioService().getUserEk(vSesiones.sesionUsuarioDTO.id);

                                if (relUser == null)
                                {
                                    throw new Exception("No se encuentra la relación SIGOPLAN-Enkontrol del usuario logeado.");
                                }

                                var nuevoFolio = 1;
                                var alm_salida = resguardos[0].alm_salida;
                                var ultimoFolio = _context.tblAlm_Movimientos.Where(x => x.estatusHabilitado && x.almacen == alm_salida && x.tipo_mov > 50).ToList().Select(x => x.numero).OrderByDescending(x => x).FirstOrDefault();

                                //if (ultimoFolio != null)
                                //{
                                nuevoFolio = ultimoFolio + 1;
                                //}

                                string almacenStarsoft = resguardos[0].alm_salida > 9 ? resguardos[0].alm_salida.ToString() : ("0" + resguardos[0].alm_salida);
                                decimal tipoCambioPeru = 0M;

                                using (var dbStarsoftConta = new MainContextPeruStarSoft003BDCONTABILIDAD())
                                {
                                    var tipoCambioActual = dbStarsoftConta.TIPO_CAMBIO.ToList().FirstOrDefault(e => e.TIPOCAMB_FECHA.Date == DateTime.Now.Date);

                                    if (tipoCambioActual != null)
                                    {
                                        tipoCambioPeru = tipoCambioActual.TIPOCAMB_COMPRA;
                                    }
                                    else
                                    {
                                        tipoCambioPeru = dbStarsoftConta.TIPO_CAMBIO.ToList().FirstOrDefault(e => e.TIPOCAMB_FECHA.Date == DateTime.Now.AddDays(-1).Date).TIPOCAMB_COMPRA;
                                    }
                                }

                                var listaInsumosStarsoft = _starsoft.MAEART.ToList();

                                #region Guardar Salida Traspaso Resguardo
                                #region Guardar Encabezado SIGOPLAN
                                tblAlm_Movimientos nuevaSalida = new tblAlm_Movimientos
                                {
                                    almacen = resguardos[0].alm_salida,
                                    tipo_mov = 52,
                                    numero = nuevoFolio,
                                    cc = resguardos[0].cc.ToUpper(),
                                    compania = 1,
                                    periodo = DateTime.Now.Month,
                                    ano = DateTime.Now.Year,
                                    orden_ct = 0,
                                    frente = 0,
                                    fecha = DateTime.Now.Date,
                                    proveedor = 0,
                                    total = resguardos.Select(x => x.cantidad_resguardo).Sum(),
                                    estatus = "A",
                                    transferida = "N",
                                    alm_destino = 97,
                                    cc_destino = resguardos[0].cc.ToUpper(),
                                    comentarios = resguardos[0].observaciones,
                                    tipo_trasp = "0",
                                    tipo_cambio = 1,
                                    numeroReq = null,
                                    estatusHabilitado = true
                                };

                                _context.tblAlm_Movimientos.Add(nuevaSalida);
                                _context.SaveChanges();

                                if (nuevaSalida.total <= 0)
                                {
                                    throw new Exception("El total no puede ser igual o menor a cero.");
                                }
                                #endregion

                                //#region Guardar Encabezado Starsoft
                                //MOVALMCAB nuevaSalidaStarsoft = new MOVALMCAB();

                                //nuevaSalidaStarsoft.CAALMA = almacenStarsoft;
                                //nuevaSalidaStarsoft.CATD = "NS";
                                //nuevaSalidaStarsoft.CANUMDOC = nuevoFolio.ToString("D10");
                                //nuevaSalidaStarsoft.CAFECDOC = DateTime.Now.Date;
                                //nuevaSalidaStarsoft.CATIPMOV = "S";
                                //nuevaSalidaStarsoft.CACODMOV = "TD";
                                //nuevaSalidaStarsoft.CASITUA = "M";
                                //nuevaSalidaStarsoft.CARFTDOC = ""; //PENDIENTE CHECAR CUANDO DEJA VER EL STOCK DE LOS INSUMOS EN EL ALMACEN (ASI SI DEJA AGREGARLOS;
                                //nuevaSalidaStarsoft.CARFNDOC = null;
                                //nuevaSalidaStarsoft.CASOLI = null;
                                //nuevaSalidaStarsoft.CAFECDEV = null;
                                //nuevaSalidaStarsoft.CACODPRO = null;
                                //nuevaSalidaStarsoft.CACENCOS = resguardos[0].cc;
                                //nuevaSalidaStarsoft.CARFALMA = null;
                                //nuevaSalidaStarsoft.CAGLOSA = resguardos[0].observaciones;
                                //nuevaSalidaStarsoft.CAFECACT = DateTime.Now.Date;
                                //nuevaSalidaStarsoft.CAHORA = DateTime.Now.ToString("HH:mm:ss");
                                //nuevaSalidaStarsoft.CAUSUARI = idAlmacenistaStarsoft;
                                //nuevaSalidaStarsoft.CACODCLI = null;
                                //nuevaSalidaStarsoft.CARUC = null;
                                //nuevaSalidaStarsoft.CANOMCLI = null;
                                //nuevaSalidaStarsoft.CAFORVEN = null;
                                //nuevaSalidaStarsoft.CACODMON = "MN"; //PENDIENTE TIPO DE MONED;
                                //nuevaSalidaStarsoft.CAVENDE = null;
                                //nuevaSalidaStarsoft.CATIPCAM = tipoCambioPeru;
                                //nuevaSalidaStarsoft.CATIPGUI = null;
                                //nuevaSalidaStarsoft.CASITGUI = "V";
                                //nuevaSalidaStarsoft.CAGUIFAC = null;
                                //nuevaSalidaStarsoft.CADIRENV = null;
                                //nuevaSalidaStarsoft.CACODTRAN = null;
                                //nuevaSalidaStarsoft.CANUMORD = null;
                                //nuevaSalidaStarsoft.CAGUIDEV = null;
                                //nuevaSalidaStarsoft.CANOMPRO = null;
                                //nuevaSalidaStarsoft.CANROPED = null;
                                //nuevaSalidaStarsoft.CACOTIZA = null;
                                //nuevaSalidaStarsoft.CAPORDESCL = 0M;
                                //nuevaSalidaStarsoft.CAPORDESES = 0M;
                                //nuevaSalidaStarsoft.CAIMPORTE = resguardos.Select(x => x.cantidad_resguardo).Sum();
                                //nuevaSalidaStarsoft.CANOMTRA = null;
                                //nuevaSalidaStarsoft.CADIRTRA = null;
                                //nuevaSalidaStarsoft.CARUCTRA = null;
                                //nuevaSalidaStarsoft.CAPLATRA = null;
                                //nuevaSalidaStarsoft.CANROIMP = null;
                                //nuevaSalidaStarsoft.CACODLIQ = null;
                                //nuevaSalidaStarsoft.CAESTIMP = null;
                                //nuevaSalidaStarsoft.CACIERRE = false;
                                //nuevaSalidaStarsoft.CATIPDEP = null;
                                //nuevaSalidaStarsoft.CAZONAF = null;
                                //nuevaSalidaStarsoft.FLAGGS = false;
                                //nuevaSalidaStarsoft.ASIENTO = false;
                                //nuevaSalidaStarsoft.CAFLETE = 0M;
                                //nuevaSalidaStarsoft.CAORDFAB = "";
                                //nuevaSalidaStarsoft.CAPEDREFE = null;
                                //nuevaSalidaStarsoft.CAIMPORTACION = false;
                                //nuevaSalidaStarsoft.CANROCAJAS = 0;
                                //nuevaSalidaStarsoft.CAPESOTOTAL = 0M;
                                //nuevaSalidaStarsoft.CADESPACHO = false;
                                //nuevaSalidaStarsoft.LINVCODIGO = null;
                                //nuevaSalidaStarsoft.COD_DIRECCION = null;
                                //nuevaSalidaStarsoft.COSTOMIN = 0M;
                                //nuevaSalidaStarsoft.CAINTERFACE = 0;
                                //nuevaSalidaStarsoft.CACTACONT = null;
                                //nuevaSalidaStarsoft.CACONTROLSTOCK = "N";
                                //nuevaSalidaStarsoft.CANOMRECEP = null;
                                //nuevaSalidaStarsoft.CADNIRECEP = null;
                                //nuevaSalidaStarsoft.CFDIREREFE = null;
                                //nuevaSalidaStarsoft.REG_COMPRA = false;
                                //nuevaSalidaStarsoft.OC_NI_GUIA = false;
                                //nuevaSalidaStarsoft.COD_AUDITORIA = "0";
                                //nuevaSalidaStarsoft.COD_MODULO = "03";
                                //nuevaSalidaStarsoft.NO_GIRO_NEGOCIO = false;
                                //nuevaSalidaStarsoft.MOTIVO_ANULACION_DOC_ELECTRONICO = null;
                                //nuevaSalidaStarsoft.DOCUMENTO_ELECTRONICO = null;
                                //nuevaSalidaStarsoft.GS_BAJA = null;
                                //nuevaSalidaStarsoft.CADocumentoImportado = null;
                                //nuevaSalidaStarsoft.SOLICITANTE = null;
                                //nuevaSalidaStarsoft.DOCUMENTO_CONTINGENCIA = null;
                                //nuevaSalidaStarsoft.GE_BAJA = null;

                                //_starsoft.MOVALMCAB.Add(nuevaSalidaStarsoft);
                                //_starsoft.SaveChanges();
                                //#endregion

                                //#region Actualizar Registro Almacén Starsoft
                                //var registroAlmacenStarsoft = _starsoft.TABALM.ToList().FirstOrDefault(x => Int32.Parse(x.TAALMA) == resguardos[0].alm_salida);

                                //if (registroAlmacenStarsoft != null)
                                //{
                                //    registroAlmacenStarsoft.TANUMSAL = nuevoFolio;
                                //    _starsoft.SaveChanges();
                                //}
                                //#endregion

                                var partidaContador = 1;

                                foreach (var res in resguardos)
                                {
                                    //#region Validar Existencias
                                    //var registroStock = _starsoft.STKART.ToList().FirstOrDefault(x => Int32.Parse(x.STALMA) == res.alm_salida && Int32.Parse(x.STCODIGO) == res.id_activo);

                                    //if (registroStock != null)
                                    //{
                                    //    if (res.cantidad_resguardo > (decimal)registroStock.STSKDIS)
                                    //    {
                                    //        throw new Exception("La cantidad solicitada del insumo \"" + ("0" + res.id_activo) + "\" sobrepasa las existencias (" + (decimal)registroStock.STSKDIS + ").");
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    throw new Exception("No hay existencias para el insumo \"" + ("0" + res.id_activo) + "\" en el almacén \"" + res.alm_salida + "\".");
                                    //}
                                    //#endregion

                                    #region Guardar Detalle SIGOPLAN
                                    var nuevaSalidaDet = new tblAlm_MovimientosDet
                                    {
                                        almacen = res.alm_salida,
                                        tipo_mov = 52,
                                        numero = nuevoFolio,
                                        partida = partidaContador,
                                        insumo = res.id_activo,
                                        comentarios = res.observaciones,
                                        area = 0,
                                        cuenta = 1,
                                        cantidad = res.cantidad_resguardo,
                                        precio = 1,
                                        importe = 1,
                                        id_resguardo = res.id_tipo_activo,
                                        area_alm = res.area_alm ?? "",
                                        lado_alm = res.lado_alm ?? "",
                                        estante_alm = res.estante_alm ?? "",
                                        nivel_alm = res.nivel_alm ?? "",
                                        transporte = "",
                                        estatusHabilitado = true
                                    };

                                    _context.tblAlm_MovimientosDet.Add(nuevaSalidaDet);
                                    _context.SaveChanges();
                                    #endregion

                                    var objInsumo = listaInsumosStarsoft.FirstOrDefault(e => e.ACODIGO == ("0" + res.id_activo.ToString()));

                                    //#region Guardar Detalle Starsoft
                                    //_starsoft.MovAlmDet.Add(new MovAlmDet()
                                    //{
                                    //    DEALMA = almacenStarsoft,
                                    //    DETD = "NS",
                                    //    DENUMDOC = nuevoFolio.ToString("D10"),
                                    //    DEITEM = partidaContador,
                                    //    DECODIGO = "0" + res.id_activo.ToString(),
                                    //    DECODREF = null,
                                    //    DECANTID = res.cantidad_resguardo,
                                    //    DECANTENT = 0M,
                                    //    DECANREF = 0M,
                                    //    DECANFAC = 0M,
                                    //    DEORDEN = null,
                                    //    DEPREUNI = 1,
                                    //    DEPRECIO = 1,
                                    //    DEPRECI1 = 1,
                                    //    DEDESCTO = 0M,
                                    //    DESTOCK = null,
                                    //    DEIGV = 0M,
                                    //    DEIMPMN = 1,
                                    //    DEIMPUS = 0,
                                    //    DESERIE = null,
                                    //    DESITUA = null,
                                    //    DEFECDOC = null,
                                    //    DECENCOS = res.cc,
                                    //    DERFALMA = null,
                                    //    DETR = null,
                                    //    DEESTADO = "V",
                                    //    DECODMOV = "TD",
                                    //    DEVALTOT = 0,
                                    //    DECOMPRO = null,
                                    //    DECODMON = "MN",
                                    //    DETIPO = null,
                                    //    DETIPCAM = tipoCambioPeru,
                                    //    DEPREVTA = null,
                                    //    DEMONVTA = null,
                                    //    DEFECVEN = null,
                                    //    DEDEVOL = 0M,
                                    //    DESOLI = null,
                                    //    DEDESCRI = objInsumo.ADESCRI,
                                    //    DEPORDES = 0M,
                                    //    DEIGVPOR = 0M,
                                    //    DEDESCLI = 0M,
                                    //    DEDESESP = 0M,
                                    //    DENUMFAC = null,
                                    //    DELOTE = null,
                                    //    DEUNIDAD = objInsumo.AUNIDAD,
                                    //    DECANTBRUTA = 0M,
                                    //    DEDSCTCANTBRUTA = 0M,
                                    //    DEORDFAB = "",
                                    //    DEQUIPO = null,
                                    //    DEFLETE = 0M,
                                    //    DEITEMI = null, //????????
                                    //    DEGLOSA = "",
                                    //    DEVALORIZADO = true,
                                    //    DESECUENORI = null,
                                    //    DEREFERENCIA = null,
                                    //    UMREFERENCIA = null,
                                    //    CANTREFERENCIA = 0M,
                                    //    DECUENTA = null,
                                    //    DETEXTO = null,
                                    //    CTA_CONSUMO = null,
                                    //    CODPARTE = "",
                                    //    CODPLANO = "",
                                    //    DETPRODUCCION = 0,
                                    //    MPMA = "",
                                    //    PorcentajeCosto = 0M,
                                    //    SALDO_NC = null,
                                    //    DEPRECIOREF = 0M,
                                    //});
                                    //_starsoft.SaveChanges();
                                    //#endregion

                                    //#region Insert/Update STKART
                                    //var registroSTKART = _starsoft.STKART.ToList().FirstOrDefault(e => e.STALMA == almacenStarsoft && e.STCODIGO == ("0" + res.id_activo.ToString()));

                                    //if (registroSTKART != null)
                                    //{
                                    //    registroSTKART.STSKDIS -= res.cantidad_resguardo;
                                    //    _starsoft.SaveChanges();
                                    //}
                                    //else
                                    //{
                                    //    _starsoft.STKART.Add(new STKART()
                                    //    {
                                    //        STALMA = almacenStarsoft,
                                    //        STCODIGO = ("0" + res.id_activo.ToString()),
                                    //        STSKDIS = res.cantidad_resguardo,
                                    //        STSKREF = 0M,
                                    //        STSKMIN = 0M,
                                    //        STSKMAX = 0M,
                                    //        STPUNREP = 0M,
                                    //        STSEMREP = 0M,
                                    //        STTIPREP = null,
                                    //        STUBIALM = null,
                                    //        STLOTCOM = 0M,
                                    //        STTIPCOM = null,
                                    //        STSKCOM = 0M,
                                    //        STKPREPRO = 0M,
                                    //        STKPREULT = 0M,
                                    //        STKFECULT = DateTime.Now.Date,
                                    //        STKPREPROUS = 0M,
                                    //        CANTREFERENCIA = 0M,
                                    //    });
                                    //    _starsoft.SaveChanges();
                                    //}
                                    //#endregion

                                    //#region Insert/Update MORESMES
                                    //var registroMoResMes = _starsoft.MoResMes.ToList().FirstOrDefault(e =>
                                    //    e.SMALMA == almacenStarsoft &&
                                    //    e.SMMESPRO == (DateTime.Now.Year.ToString() + DateTime.Now.ToString("MM")) &&
                                    //    e.SMCODIGO == ("0" + res.id_activo.ToString())
                                    //);

                                    //if (registroMoResMes != null)
                                    //{
                                    //    registroMoResMes.SMCANSAL += res.cantidad_resguardo;
                                    //    _starsoft.SaveChanges();
                                    //}
                                    //else
                                    //{
                                    //    var objCrearMoResMes = new MoResMes();
                                    //    objCrearMoResMes.SMALMA = almacenStarsoft;
                                    //    objCrearMoResMes.SMCODIGO = ("0" + res.id_activo.ToString());
                                    //    objCrearMoResMes.SMMESPRO = (DateTime.Now.Year.ToString() + DateTime.Now.ToString("MM"));
                                    //    objCrearMoResMes.SMUSPREUNI = 0M; //CAMBIAR POR EL PRESIO ADECUEDO DEPENDE EL TIPO DE MONEDA CHECAR EN OTRAS TABLAS
                                    //    objCrearMoResMes.SMMNPREUNI = 0M; //CAMBIAR POR EL PRESIO ADECUEDO DEPENDE EL TIPO DE MONEDA
                                    //    objCrearMoResMes.SMUSPREANT = 0M;
                                    //    objCrearMoResMes.SMULTMOV = null;
                                    //    objCrearMoResMes.SMCANENT = 0M;
                                    //    objCrearMoResMes.SMCANSAL = res.cantidad_resguardo;
                                    //    objCrearMoResMes.SMANTCAN = 0M;
                                    //    objCrearMoResMes.SMMNANTVAL = 0M;
                                    //    objCrearMoResMes.SMMNACTVAL = 0M;
                                    //    objCrearMoResMes.SMUSANTVAL = 0M;
                                    //    objCrearMoResMes.SMUSACTVAL = 0M;
                                    //    objCrearMoResMes.SMUSENT = 0M;
                                    //    objCrearMoResMes.SMMNENT = 0M;
                                    //    objCrearMoResMes.SMUSSAL = 0;
                                    //    objCrearMoResMes.SMMNSAL = 0M;
                                    //    objCrearMoResMes.SMCUENTA = null;
                                    //    objCrearMoResMes.SMGRUPO = null;
                                    //    objCrearMoResMes.SMFAMILIA = null;
                                    //    objCrearMoResMes.SMLINEA = null;
                                    //    objCrearMoResMes.SMTIPO = null;
                                    //    objCrearMoResMes.SMSALDOINI = 0M;
                                    //    objCrearMoResMes.COD_MODULO = "03";
                                    //    objCrearMoResMes.COD_OPCION = "Men_TraRegEnt"; //??

                                    //    _starsoft.MoResMes.Add(objCrearMoResMes);
                                    //    _starsoft.SaveChanges();
                                    //}
                                    //#endregion

                                    partidaContador++;
                                }
                                #endregion

                                foreach (var resguardo in resguardos)
                                {
                                    resguardo.folio = nuevoFolio;

                                    #region Guardar Registro Resguardo SIGOPLAN
                                    var nuevoResguardo = new tblAlm_Resguardo
                                    {
                                        cc = resguardo.cc,
                                        folio = resguardo.folio,
                                        id_activo = resguardo.id_activo,
                                        id_tipo_activo = resguardo.id_tipo_activo,
                                        marca = resguardo.marca,
                                        modelo = resguardo.modelo,
                                        color = resguardo.color,
                                        num_serie = resguardo.num_serie,
                                        valor_activo = resguardo.valor_activo,
                                        compania = resguardo.compania,
                                        plan_desc = resguardo.plan_desc,
                                        condiciones = resguardo.condiciones,
                                        numpro = resguardo.numpro,
                                        factura = resguardo.factura,
                                        fec_factura = resguardo.fec_factura,
                                        empleado = resguardo.empleado,
                                        empleadoSIGOPLAN = null,
                                        claveEmpleado = resguardo.claveEmpleado,
                                        licencia = resguardo.licencia,
                                        tipo = resguardo.tipo,
                                        fec_licencia = resguardo.fec_licencia,
                                        observaciones = resguardo.observaciones,
                                        fec_resguardo = DateTime.Now.Date,
                                        foto = resguardo.foto,
                                        estatus = resguardo.estatus,
                                        entrega = relUser.empleado,
                                        entregaSIGOPLAN = relUser.idUsuario,
                                        autoriza = resguardo.autoriza,
                                        autorizaSIGOPLAN = null,
                                        recibio = null, //recibio = resguardo.recibio,
                                        recibioSIGOPLAN = null,
                                        condiciones_ret = resguardo.condiciones_ret,
                                        fec_devolucion = resguardo.fec_devolucion,
                                        cantidad_resguardo = resguardo.cantidad_resguardo,
                                        alm_salida = resguardo.alm_salida,
                                        alm_entrada = resguardo.alm_entrada,
                                        foto_2 = resguardo.foto_2,
                                        foto_3 = resguardo.foto_3,
                                        foto_4 = resguardo.foto_4,
                                        foto_5 = resguardo.foto_5,
                                        costo_promedio = resguardo.costo_promedio,
                                        resguardo_parcial = resguardo.resguardo_parcial,
                                        estatusRegistro = true
                                    };

                                    _context.tblAlm_Resguardo.Add(nuevoResguardo);
                                    _context.SaveChanges();
                                    #endregion
                                }

                                result.Add("folio", nuevoFolio);
                                #endregion
                            }

                            dbStarsoftTransaction.Commit();
                            dbSigoplanTransaction.Commit();

                            SaveBitacora(12, (int)AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(resguardos));

                            result.Add("data", resguardos);
                            result.Add(SUCCESS, true);
                        }
                        catch (Exception e)
                        {
                            dbStarsoftTransaction.Rollback();
                            dbSigoplanTransaction.Rollback();

                            LogError(12, 12, "ResguardoController", "guardarAsignacionNormalPeru", e, AccionEnum.AGREGAR, 0, resguardos);

                            result.Add(MESSAGE, e.Message);
                            result.Add(SUCCESS, false);
                        }
                    }
                }
            }

            return result;
        }

        public int guardarSalidaTraspasoResguardo(List<ResguardoEKDTO> resguardos, int folioResguardo, OdbcTransaction trans)
        {
            var empleado = 0;
            var usuarioSigoplan = _context.tblP_Usuario_Enkontrol.FirstOrDefault(x => x.idUsuario == vSesiones.sesionUsuarioDTO.id);

            if (usuarioSigoplan != null)
            {
                empleado = usuarioSigoplan.empleado;
            }

            var nuevoNumero = 1;
            var folioTraspaso = 1;

            var ultimoMovimientoEK = consultaCheckProductivo(
                string.Format(@"SELECT 
                                    TOP 1 numero, folio_traspaso 
                                FROM si_movimientos 
                                WHERE almacen = {0} AND tipo_mov = {1} 
                                ORDER BY numero DESC", resguardos[0].alm_salida, 52)
            );

            if (ultimoMovimientoEK != null)
            {
                var ultimoMovimiento = ((List<dynamic>)ultimoMovimientoEK.ToObject<List<dynamic>>())[0];

                nuevoNumero = (int)ultimoMovimiento.numero + 1;
                folioTraspaso = (ultimoMovimiento.folio_traspaso != null) ? (int)ultimoMovimiento.folio_traspaso + 1 : 1;
            }

            tblAlm_Movimientos nuevaSalida = new tblAlm_Movimientos();
            List<tblCom_ReqDet> requisicionDet = new List<tblCom_ReqDet>();

            nuevaSalida = new tblAlm_Movimientos
            {
                almacen = resguardos[0].alm_salida,
                tipo_mov = 52,
                numero = nuevoNumero,
                cc = resguardos[0].cc.ToUpper(),
                compania = 1,
                periodo = DateTime.Now.Month,
                ano = DateTime.Now.Year,
                orden_ct = 0,
                frente = 0,
                fecha = DateTime.Now.Date,
                proveedor = 0,
                total = resguardos.Select(x => x.cantidad_resguardo).Sum(),
                estatus = "A",
                transferida = "N",
                alm_destino = resguardos[0].alm_entrada,
                cc_destino = resguardos[0].cc.ToUpper(),
                comentarios = resguardos[0].observaciones,
                tipo_trasp = "0",
                tipo_cambio = 1,
                numeroReq = null,
                estatusHabilitado = true
            };

            _context.tblAlm_Movimientos.Add(nuevaSalida);
            _context.SaveChanges();

            List<tblAlm_MovimientosDet> listSalidaDet = new List<tblAlm_MovimientosDet>();

            var partidaContador = 1;

            foreach (var res in resguardos)
            {
                var partidaMovimiento = partidaContador++;

                var nuevaSalidaDet = new tblAlm_MovimientosDet
                {
                    almacen = res.alm_salida,
                    tipo_mov = 52,
                    numero = nuevoNumero,
                    partida = partidaMovimiento,
                    insumo = res.id_activo,
                    comentarios = res.observaciones,
                    area = 0,
                    cuenta = 1,
                    cantidad = res.cantidad_resguardo,
                    precio = 1,
                    importe = 1,
                    id_resguardo = res.id_tipo_activo,
                    area_alm = res.area_alm ?? "",
                    lado_alm = res.lado_alm ?? "",
                    estante_alm = res.estante_alm ?? "",
                    nivel_alm = res.nivel_alm ?? "",
                    transporte = "",
                    estatusHabilitado = true
                };

                _context.tblAlm_MovimientosDet.Add(nuevaSalidaDet);
                _context.SaveChanges();
                listSalidaDet.Add(nuevaSalidaDet);
            }

            if (nuevaSalida.total <= 0)
            {
                throw new Exception("El total no puede ser igual o menor a cero.");
            }

            using (var con = checkConexionProductivo())
            {
                var consultaMovimientos = @"INSERT INTO si_movimientos 
                                         (almacen, tipo_mov, numero, cc, compania, periodo, ano, orden_ct, frente, fecha, proveedor, total, estatus, 
                                         transferida, poliza, empleado, alm_destino, cc_destino, comentarios, tipo_trasp, numero_destino, tp, year_poliza, 
                                         mes_poliza, tipo_cambio, hora, fecha_modifica, empleado_modifica, destajista, obra, id_residente, factura, sector_id, 
                                         tc_cc, paquete, tipo_cargo, cargo_destajista, cargo_id_residente, embarque, orden_prod, folio_traspaso, bit_sin_ubicacion) 
                                         VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                var commandMovimientos = new OdbcCommand(consultaMovimientos);

                OdbcParameterCollection parametersMovimientos = commandMovimientos.Parameters;

                parametersMovimientos.Add("@almacen", OdbcType.Numeric).Value = nuevaSalida.almacen;
                parametersMovimientos.Add("@tipo_mov", OdbcType.Numeric).Value = nuevaSalida.tipo_mov;
                parametersMovimientos.Add("@numero", OdbcType.Numeric).Value = nuevaSalida.numero;
                parametersMovimientos.Add("@cc", OdbcType.Char).Value = nuevaSalida.cc;
                parametersMovimientos.Add("@compania", OdbcType.Numeric).Value = nuevaSalida.compania;
                parametersMovimientos.Add("@periodo", OdbcType.Numeric).Value = nuevaSalida.periodo;
                parametersMovimientos.Add("@ano", OdbcType.Numeric).Value = nuevaSalida.ano;
                parametersMovimientos.Add("@orden_ct", OdbcType.Numeric).Value = nuevaSalida.orden_ct;
                parametersMovimientos.Add("@frente", OdbcType.Numeric).Value = nuevaSalida.frente;
                parametersMovimientos.Add("@fecha", OdbcType.Date).Value = nuevaSalida.fecha.Date;
                parametersMovimientos.Add("@proveedor", OdbcType.Numeric).Value = nuevaSalida.proveedor;
                parametersMovimientos.Add("@total", OdbcType.Numeric).Value = nuevaSalida.total;
                parametersMovimientos.Add("@estatus", OdbcType.Char).Value = nuevaSalida.estatus;
                parametersMovimientos.Add("@transferida", OdbcType.Char).Value = nuevaSalida.transferida;
                parametersMovimientos.Add("@poliza", OdbcType.Numeric).Value = 0;
                parametersMovimientos.Add("@empleado", OdbcType.Numeric).Value = empleado;
                parametersMovimientos.Add("@alm_destino", OdbcType.Numeric).Value = nuevaSalida.alm_destino;
                parametersMovimientos.Add("@cc_destino", OdbcType.Char).Value = nuevaSalida.cc_destino;
                parametersMovimientos.Add("@comentarios", OdbcType.Char).Value = nuevaSalida.comentarios != null ? nuevaSalida.comentarios : "";
                parametersMovimientos.Add("@tipo_trasp", OdbcType.Char).Value = nuevaSalida.tipo_trasp;
                parametersMovimientos.Add("@numero_destino", OdbcType.Numeric).Value = folioResguardo;
                parametersMovimientos.Add("@tp", OdbcType.Char).Value = DBNull.Value;
                parametersMovimientos.Add("@year_poliza", OdbcType.Numeric).Value = DateTime.Now.Year;
                parametersMovimientos.Add("@mes_poliza", OdbcType.Numeric).Value = DateTime.Now.Month;
                parametersMovimientos.Add("@tipo_cambio", OdbcType.Numeric).Value = nuevaSalida.tipo_cambio;
                parametersMovimientos.Add("@hora", OdbcType.Time).Value = DateTime.Now.TimeOfDay;
                parametersMovimientos.Add("@fecha_modifica", OdbcType.Date).Value = DateTime.Now.Date;
                parametersMovimientos.Add("@empleado_modifica", OdbcType.Numeric).Value = empleado;
                parametersMovimientos.Add("@destajista", OdbcType.Numeric).Value = DBNull.Value;
                parametersMovimientos.Add("@obra", OdbcType.Char).Value = DBNull.Value;
                parametersMovimientos.Add("@id_residente", OdbcType.Numeric).Value = DBNull.Value;
                parametersMovimientos.Add("@factura", OdbcType.Numeric).Value = DBNull.Value;
                parametersMovimientos.Add("@sector_id", OdbcType.Numeric).Value = DBNull.Value;
                parametersMovimientos.Add("@tc_cc", OdbcType.Numeric).Value = 1;
                parametersMovimientos.Add("@paquete", OdbcType.Numeric).Value = DBNull.Value;
                parametersMovimientos.Add("@tipo_cargo", OdbcType.Numeric).Value = 0;
                parametersMovimientos.Add("@cargo_Destajista", OdbcType.Numeric).Value = DBNull.Value;
                parametersMovimientos.Add("@cargo_id_residente", OdbcType.Numeric).Value = DBNull.Value;
                parametersMovimientos.Add("@embarque", OdbcType.Numeric).Value = DBNull.Value;
                parametersMovimientos.Add("@orden_prod", OdbcType.Numeric).Value = DBNull.Value;
                parametersMovimientos.Add("@folio_traspaso", OdbcType.Numeric).Value = DBNull.Value;
                parametersMovimientos.Add("@bit_sin_ubicacion", OdbcType.Numeric).Value = 0;

                commandMovimientos.Connection = trans.Connection;
                commandMovimientos.Transaction = trans;

                var success = commandMovimientos.ExecuteNonQuery();
                var successDet = 0;

                foreach (var salDet in listSalidaDet)
                {
                    if ((salDet.cantidad * 1) <= 0)
                    {
                        throw new Exception("El precio y el importe no pueden ser igual o menor a cero.");
                    }

                    var consultaMovimientosDetalle = @"INSERT INTO si_movimientos_det 
                                         (almacen, tipo_mov, numero, partida, insumo, comentarios, area, cuenta, cantidad, precio, importe, 
                                         partida_oc, costo_prom, obra, manzana_ini, lote_ini, interior_ini, manzana_fin, lote_fin, interior_fin, 
                                         remision, sector_id, id_resguardo, area_alm, lado_alm, estante_alm, nivel_alm, fecha_fisico) 
                                         VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                    var commandMovimientosDetalles = new OdbcCommand(consultaMovimientosDetalle);

                    OdbcParameterCollection parametersMovimientosDetalles = commandMovimientosDetalles.Parameters;

                    parametersMovimientosDetalles.Add("@almacen", OdbcType.Numeric).Value = salDet.almacen;
                    parametersMovimientosDetalles.Add("@tipo_mov", OdbcType.Numeric).Value = salDet.tipo_mov;
                    parametersMovimientosDetalles.Add("@numero", OdbcType.Numeric).Value = salDet.numero;
                    parametersMovimientosDetalles.Add("@partida", OdbcType.Numeric).Value = salDet.partida;
                    parametersMovimientosDetalles.Add("@insumo", OdbcType.Numeric).Value = salDet.insumo;
                    parametersMovimientosDetalles.Add("@comentarios", OdbcType.Char).Value = salDet.comentarios != null ? salDet.comentarios : "";
                    parametersMovimientosDetalles.Add("@area", OdbcType.Numeric).Value = salDet.area;
                    parametersMovimientosDetalles.Add("@cuenta", OdbcType.Numeric).Value = salDet.cuenta;
                    parametersMovimientosDetalles.Add("@cantidad", OdbcType.Numeric).Value = salDet.cantidad;
                    parametersMovimientosDetalles.Add("@precio", OdbcType.Numeric).Value = 1;
                    parametersMovimientosDetalles.Add("@importe", OdbcType.Numeric).Value = salDet.cantidad * 1;
                    parametersMovimientosDetalles.Add("@partida_oc", OdbcType.Numeric).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@costo_prom", OdbcType.Numeric).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@obra", OdbcType.Char).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@manzana_ini", OdbcType.Numeric).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@lote_ini", OdbcType.Numeric).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@interior_ini", OdbcType.Char).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@manzana_fin", OdbcType.Numeric).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@lote_fin", OdbcType.Numeric).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@interior_fin", OdbcType.Char).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@remision", OdbcType.Char).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@sector_id", OdbcType.Numeric).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@id_resguardo", OdbcType.Numeric).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@area_alm", OdbcType.Char).Value = salDet.area_alm;
                    parametersMovimientosDetalles.Add("@lado_alm", OdbcType.Char).Value = salDet.lado_alm;
                    parametersMovimientosDetalles.Add("@estante_alm", OdbcType.Char).Value = salDet.estante_alm;
                    parametersMovimientosDetalles.Add("@nivel_alm", OdbcType.Char).Value = salDet.nivel_alm;
                    parametersMovimientosDetalles.Add("@fecha_fisico", OdbcType.Date).Value = DBNull.Value;

                    commandMovimientosDetalles.Connection = trans.Connection;
                    commandMovimientosDetalles.Transaction = trans;

                    successDet = commandMovimientosDetalles.ExecuteNonQuery();

                    #region Actualizar Tablas Acumula
                    var objAcumula = new MovimientoDetalleEnkontrolDTO
                    {
                        insumo = salDet.insumo,
                        cantidad = salDet.cantidad,
                        precio = 1,
                        tipo_mov = salDet.tipo_mov,
                        costo_prom = 0
                    };

                    actualizarAcumula(nuevaSalida.almacen, nuevaSalida.cc, objAcumula, null, trans);
                    #endregion
                }

                if (success > 0 && successDet > 0)
                {
                    _context.SaveChanges();
                }
            }

            return nuevoNumero;
        }

        public int guardarSalidaTraspasoResguardoColombia(List<ResguardoEKDTO> resguardos, int folioResguardo, OdbcTransaction trans)
        {
            var empleado = 0;
            var usuarioSigoplan = _context.tblP_Usuario_Enkontrol.FirstOrDefault(x => x.idUsuario == vSesiones.sesionUsuarioDTO.id);

            if (usuarioSigoplan != null)
            {
                empleado = usuarioSigoplan.empleado;
            }

            var nuevoNumero = 1;
            //var folioTraspaso = 1;

            var alm_salida = resguardos[0].alm_salida;
            var ultimoMovimiento = _context.tblAlm_Movimientos.Where(x => x.estatusHabilitado && x.almacen == alm_salida && x.tipo_mov == 52).OrderByDescending(x => x.numero).FirstOrDefault();

            if (ultimoMovimiento != null)
            {
                nuevoNumero = ultimoMovimiento.numero + 1;
                //folioTraspaso = (ultimoMovimiento.folio_traspaso != null) ? (int)ultimoMovimiento.folio_traspaso + 1 : ;
            }

            tblAlm_Movimientos nuevaSalida = new tblAlm_Movimientos();
            List<tblCom_ReqDet> requisicionDet = new List<tblCom_ReqDet>();

            nuevaSalida = new tblAlm_Movimientos
            {
                almacen = resguardos[0].alm_salida,
                tipo_mov = 52,
                numero = nuevoNumero,
                cc = resguardos[0].cc.ToUpper(),
                compania = 1,
                periodo = DateTime.Now.Month,
                ano = DateTime.Now.Year,
                orden_ct = 0,
                frente = 0,
                fecha = DateTime.Now.Date,
                proveedor = 0,
                total = resguardos.Select(x => x.cantidad_resguardo).Sum(),
                estatus = "A",
                transferida = "N",
                alm_destino = resguardos[0].alm_entrada,
                cc_destino = resguardos[0].cc.ToUpper(),
                comentarios = resguardos[0].observaciones,
                tipo_trasp = "0",
                tipo_cambio = 1,
                numeroReq = null,
                estatusHabilitado = true
            };

            _context.tblAlm_Movimientos.Add(nuevaSalida);
            _context.SaveChanges();

            List<tblAlm_MovimientosDet> listSalidaDet = new List<tblAlm_MovimientosDet>();

            var partidaContador = 1;

            foreach (var res in resguardos)
            {
                var partidaMovimiento = partidaContador++;

                var nuevaSalidaDet = new tblAlm_MovimientosDet
                {
                    almacen = res.alm_salida,
                    tipo_mov = 52,
                    numero = nuevoNumero,
                    partida = partidaMovimiento,
                    insumo = res.id_activo,
                    comentarios = res.observaciones,
                    area = 0,
                    cuenta = 1,
                    cantidad = res.cantidad_resguardo,
                    precio = 1,
                    importe = 1,
                    id_resguardo = res.id_tipo_activo,
                    area_alm = res.area_alm ?? "",
                    lado_alm = res.lado_alm ?? "",
                    estante_alm = res.estante_alm ?? "",
                    nivel_alm = res.nivel_alm ?? "",
                    transporte = "",
                    estatusHabilitado = true
                };

                _context.tblAlm_MovimientosDet.Add(nuevaSalidaDet);
                _context.SaveChanges();
                listSalidaDet.Add(nuevaSalidaDet);
            }

            if (nuevaSalida.total <= 0)
            {
                throw new Exception("El total no puede ser igual o menor a cero.");
            }

            using (var con = checkConexionProductivo())
            {
                var consultaMovimientos = @"INSERT INTO DBA.si_movimientos 
                                         (almacen, tipo_mov, numero, cc, compania, periodo, ano, orden_ct, frente, fecha, proveedor, total, estatus, 
                                         transferida, poliza, empleado, alm_destino, cc_destino, comentarios, tipo_trasp, numero_destino, tp, year_poliza, 
                                         mes_poliza, tipo_cambio, hora, fecha_modifica, empleado_modifica, destajista, obra, id_residente, factura, sector_id, 
                                         tc_cc, paquete, tipo_cargo, cargo_destajista, cargo_id_residente, embarque, orden_prod) 
                                         VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                var commandMovimientos = new OdbcCommand(consultaMovimientos);

                OdbcParameterCollection parametersMovimientos = commandMovimientos.Parameters;

                parametersMovimientos.Add("@almacen", OdbcType.Numeric).Value = nuevaSalida.almacen;
                parametersMovimientos.Add("@tipo_mov", OdbcType.Numeric).Value = nuevaSalida.tipo_mov;
                parametersMovimientos.Add("@numero", OdbcType.Numeric).Value = nuevaSalida.numero;
                parametersMovimientos.Add("@cc", OdbcType.Char).Value = nuevaSalida.cc;
                parametersMovimientos.Add("@compania", OdbcType.Numeric).Value = nuevaSalida.compania;
                parametersMovimientos.Add("@periodo", OdbcType.Numeric).Value = nuevaSalida.periodo;
                parametersMovimientos.Add("@ano", OdbcType.Numeric).Value = nuevaSalida.ano;
                parametersMovimientos.Add("@orden_ct", OdbcType.Numeric).Value = nuevaSalida.orden_ct;
                parametersMovimientos.Add("@frente", OdbcType.Numeric).Value = nuevaSalida.frente;
                parametersMovimientos.Add("@fecha", OdbcType.Date).Value = nuevaSalida.fecha.Date;
                parametersMovimientos.Add("@proveedor", OdbcType.Numeric).Value = nuevaSalida.proveedor;
                parametersMovimientos.Add("@total", OdbcType.Numeric).Value = nuevaSalida.total;
                parametersMovimientos.Add("@estatus", OdbcType.Char).Value = nuevaSalida.estatus;
                parametersMovimientos.Add("@transferida", OdbcType.Char).Value = nuevaSalida.transferida;
                parametersMovimientos.Add("@poliza", OdbcType.Numeric).Value = 0;
                parametersMovimientos.Add("@empleado", OdbcType.Numeric).Value = empleado;
                parametersMovimientos.Add("@alm_destino", OdbcType.Numeric).Value = nuevaSalida.alm_destino;
                parametersMovimientos.Add("@cc_destino", OdbcType.Char).Value = nuevaSalida.cc_destino;
                parametersMovimientos.Add("@comentarios", OdbcType.Char).Value = nuevaSalida.comentarios != null ? nuevaSalida.comentarios : "";
                parametersMovimientos.Add("@tipo_trasp", OdbcType.Char).Value = nuevaSalida.tipo_trasp;
                parametersMovimientos.Add("@numero_destino", OdbcType.Numeric).Value = folioResguardo;
                parametersMovimientos.Add("@tp", OdbcType.Char).Value = DBNull.Value;
                parametersMovimientos.Add("@year_poliza", OdbcType.Numeric).Value = DateTime.Now.Year;
                parametersMovimientos.Add("@mes_poliza", OdbcType.Numeric).Value = DateTime.Now.Month;
                parametersMovimientos.Add("@tipo_cambio", OdbcType.Numeric).Value = nuevaSalida.tipo_cambio;
                parametersMovimientos.Add("@hora", OdbcType.Time).Value = DateTime.Now.TimeOfDay;
                parametersMovimientos.Add("@fecha_modifica", OdbcType.Date).Value = DateTime.Now.Date;
                parametersMovimientos.Add("@empleado_modifica", OdbcType.Numeric).Value = empleado;
                parametersMovimientos.Add("@destajista", OdbcType.Numeric).Value = DBNull.Value;
                parametersMovimientos.Add("@obra", OdbcType.Char).Value = DBNull.Value;
                parametersMovimientos.Add("@id_residente", OdbcType.Numeric).Value = DBNull.Value;
                parametersMovimientos.Add("@factura", OdbcType.Numeric).Value = DBNull.Value;
                parametersMovimientos.Add("@sector_id", OdbcType.Numeric).Value = DBNull.Value;
                parametersMovimientos.Add("@tc_cc", OdbcType.Numeric).Value = 1;
                parametersMovimientos.Add("@paquete", OdbcType.Numeric).Value = DBNull.Value;
                parametersMovimientos.Add("@tipo_cargo", OdbcType.Numeric).Value = 0;
                parametersMovimientos.Add("@cargo_Destajista", OdbcType.Numeric).Value = DBNull.Value;
                parametersMovimientos.Add("@cargo_id_residente", OdbcType.Numeric).Value = DBNull.Value;
                parametersMovimientos.Add("@embarque", OdbcType.Numeric).Value = DBNull.Value;
                parametersMovimientos.Add("@orden_prod", OdbcType.Numeric).Value = DBNull.Value;

                commandMovimientos.Connection = trans.Connection;
                commandMovimientos.Transaction = trans;

                var success = commandMovimientos.ExecuteNonQuery();
                var successDet = 0;

                foreach (var salDet in listSalidaDet)
                {
                    if ((salDet.cantidad * 1) <= 0)
                    {
                        throw new Exception("El precio y el importe no pueden ser igual o menor a cero.");
                    }

                    var consultaMovimientosDetalle = @"INSERT INTO DBA.si_movimientos_det 
                                         (almacen, tipo_mov, numero, partida, insumo, comentarios, area, cuenta, cantidad, precio, importe, 
                                         partida_oc, costo_prom, obra, manzana_ini, lote_ini, interior_ini, manzana_fin, lote_fin, interior_fin, 
                                         remision, sector_id) 
                                         VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                    var commandMovimientosDetalles = new OdbcCommand(consultaMovimientosDetalle);

                    OdbcParameterCollection parametersMovimientosDetalles = commandMovimientosDetalles.Parameters;

                    parametersMovimientosDetalles.Add("@almacen", OdbcType.Numeric).Value = salDet.almacen;
                    parametersMovimientosDetalles.Add("@tipo_mov", OdbcType.Numeric).Value = salDet.tipo_mov;
                    parametersMovimientosDetalles.Add("@numero", OdbcType.Numeric).Value = salDet.numero;
                    parametersMovimientosDetalles.Add("@partida", OdbcType.Numeric).Value = salDet.partida;
                    parametersMovimientosDetalles.Add("@insumo", OdbcType.Numeric).Value = salDet.insumo;
                    parametersMovimientosDetalles.Add("@comentarios", OdbcType.Char).Value = salDet.comentarios != null ? salDet.comentarios : "";
                    parametersMovimientosDetalles.Add("@area", OdbcType.Numeric).Value = salDet.area;
                    parametersMovimientosDetalles.Add("@cuenta", OdbcType.Numeric).Value = salDet.cuenta;
                    parametersMovimientosDetalles.Add("@cantidad", OdbcType.Numeric).Value = salDet.cantidad;
                    parametersMovimientosDetalles.Add("@precio", OdbcType.Numeric).Value = 1;
                    parametersMovimientosDetalles.Add("@importe", OdbcType.Numeric).Value = salDet.cantidad * 1;
                    parametersMovimientosDetalles.Add("@partida_oc", OdbcType.Numeric).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@costo_prom", OdbcType.Numeric).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@obra", OdbcType.Char).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@manzana_ini", OdbcType.Numeric).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@lote_ini", OdbcType.Numeric).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@interior_ini", OdbcType.Char).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@manzana_fin", OdbcType.Numeric).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@lote_fin", OdbcType.Numeric).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@interior_fin", OdbcType.Char).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@remision", OdbcType.Char).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@sector_id", OdbcType.Numeric).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@fecha_fisico", OdbcType.Date).Value = DBNull.Value;

                    commandMovimientosDetalles.Connection = trans.Connection;
                    commandMovimientosDetalles.Transaction = trans;

                    successDet = commandMovimientosDetalles.ExecuteNonQuery();

                    #region Actualizar Tablas Acumula
                    var objAcumula = new MovimientoDetalleEnkontrolDTO
                    {
                        insumo = salDet.insumo,
                        cantidad = salDet.cantidad,
                        precio = 1,
                        tipo_mov = salDet.tipo_mov,
                        costo_prom = 0
                    };

                    actualizarAcumulaColombia(nuevaSalida.almacen, nuevaSalida.cc, objAcumula, null, trans);
                    #endregion
                }

                if (success > 0 && successDet > 0)
                {
                    _context.SaveChanges();
                }
            }

            return nuevoNumero;
        }

        public List<ResguardoEKDTO> getResguardoReporte(string cc, int folio)
        {
            try
            {
                switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
                {
                    case EmpresaEnum.Peru:
                        {
                            #region Resguardos Perú
                            var listaUsuarios = _context.tblP_Usuario.Where(x => x.estatus).ToList();
                            var listaUsuariosResguardos = _context.tblAlm_EmpleadoResguardo.Where(x => x.registroActivo).ToList();
                            var listaInsumosStarsoft = new List<MAEART>();
                            var listaAlmacenesStarsoft = new List<TABALM>();

                            using (var _starsoft = new MainContextPeruStarSoft003BDCOMUN())
                            {
                                listaInsumosStarsoft = _starsoft.MAEART.ToList();
                                listaAlmacenesStarsoft = _starsoft.TABALM.ToList();
                            }

                            var listaCentrosCosto = _context.tblP_CC.Where(x => x.estatus).ToList();

                            var resguardos = _context.tblAlm_Resguardo.Where(x => x.estatusRegistro && x.cc == cc && x.folio == folio).ToList().Select(x => new ResguardoEKDTO
                            {
                                cc = x.cc,
                                folio = x.folio,
                                id_activo = x.id_activo,
                                id_tipo_activo = x.id_tipo_activo,
                                marca = x.marca,
                                modelo = x.modelo,
                                color = x.color,
                                num_serie = x.num_serie,
                                valor_activo = x.valor_activo,
                                compania = x.compania,
                                plan_desc = x.plan_desc,
                                condiciones = x.condiciones,
                                numpro = x.numpro,
                                factura = x.factura,
                                fec_factura = x.fec_factura,
                                empleado = (int)x.empleado,
                                empleadoSIGOPLAN = x.empleadoSIGOPLAN,
                                claveEmpleado = x.claveEmpleado,
                                licencia = x.licencia,
                                tipo = x.tipo,
                                fec_licencia = x.fec_licencia,
                                observaciones = x.observaciones,
                                fec_resguardo = x.fec_resguardo,
                                foto = x.foto,
                                estatus = x.estatus,
                                entrega = (int)x.entrega,
                                entregaSIGOPLAN = x.entregaSIGOPLAN,
                                autoriza = (int)x.autoriza,
                                autorizaSIGOPLAN = x.autorizaSIGOPLAN,
                                recibio = x.recibio,
                                recibioSIGOPLAN = x.recibioSIGOPLAN,
                                condiciones_ret = x.condiciones_ret,
                                fec_devolucion = x.fec_devolucion,
                                cantidad_resguardo = x.cantidad_resguardo,
                                alm_salida = x.alm_salida,
                                alm_entrada = x.alm_entrada,
                                foto_2 = x.foto_2,
                                foto_3 = x.foto_3,
                                foto_4 = x.foto_4,
                                foto_5 = x.foto_5,
                                costo_promedio = x.costo_promedio,
                                resguardo_parcial = x.resguardo_parcial,

                                insumoDesc = listaInsumosStarsoft.Where(y => y.ACODIGO == ("0" + x.id_activo)).Select(z => z.ADESCRI).FirstOrDefault(),
                                empleadoNombre = listaUsuariosResguardos.Where(y => y.folio == (int)x.empleado).Select(z => z.nombre + " " + z.apellidoPaterno + " " + z.apellidoMaterno).FirstOrDefault(),
                                autorizaNombre = listaUsuariosResguardos.Where(y => y.folio == (int)x.autoriza).Select(z => z.nombre + " " + z.apellidoPaterno + " " + z.apellidoMaterno).FirstOrDefault(),
                                recibioNombre = x.recibioSIGOPLAN != null ? listaUsuariosResguardos.Where(y => y.folio == (int)x.recibioSIGOPLAN).Select(z => z.nombre + " " + z.apellidoPaterno + " " + z.apellidoMaterno).FirstOrDefault() : "",
                                entregaNombre = x.entregaSIGOPLAN != null ? listaUsuarios.Where(y => y.id == (int)x.entregaSIGOPLAN).Select(z => z.nombre + " " + z.apellidoPaterno + " " + z.apellidoMaterno).FirstOrDefault() : "",
                                alm_salidaDesc = listaAlmacenesStarsoft.Where(y => Int32.Parse(y.TAALMA) == x.alm_salida).Select(z => z.TADESCRI).FirstOrDefault(),
                                tipo_activoDesc = "",
                                ccDesc = listaCentrosCosto.Where(y => y.cc == x.cc).Select(z => z.descripcion).FirstOrDefault()
                            }).ToList();

                            foreach (var res in resguardos)
                            {
                                res.alm_salidaDesc = res.alm_salida + "-" + res.alm_salidaDesc;
                                res.ccDesc = res.cc + "-" + res.ccDesc;
                                res.insumoDesc = res.id_activo + "-" + res.insumoDesc;
                                res.cantidad_resguardo = res.cantidad_resguardo;
                                res.resguardo_parcial = res.resguardo_parcial ?? 0;
                            }

                            if (resguardos.Any(x => x.estatus != "D"))
                            {
                                if (resguardos.Any(x => x.estatus == "P"))
                                {
                                    resguardos = resguardos.Where(x => (x.cantidad_resguardo - (decimal)x.resguardo_parcial) > 0).ToList();
                                }
                            }

                            return resguardos;
                            #endregion
                        }
                    case EmpresaEnum.Colombia:
                        {
                            #region Resguardos Colombia
                            var listaUsuarios = _context.tblP_Usuario.Where(x => x.estatus).ToList();
                            var listaCentrosCosto = _context.tblP_CC.Where(x => x.estatus).ToList();
                            var listaInsumos = _contextEnkontrol.Select<InsumoCatalogoDTO>(getEnkontrolEnumADM(), new OdbcConsultaDTO
                            {
                                consulta = @"SELECT ins.insumo, ins.descripcion AS insumoDesc FROM insumos ins"
                            });
                            var listaUsuariosResguardos = _contextEnkontrol.Select<dynamic>(getEnkontrolEnumADM(), new OdbcConsultaDTO
                            {
                                consulta = @"SELECT * FROM empleados"
                            });
                            var listaAlmacenes = _contextEnkontrol.Select<dynamic>(getEnkontrolEnumADM(), new OdbcConsultaDTO
                            {
                                consulta = @"SELECT * FROM si_almacen"
                            });

                            var resguardos = _context.tblAlm_Resguardo.Where(x => x.estatusRegistro && x.cc == cc && x.folio == folio).ToList().Select(x => new ResguardoEKDTO
                            {
                                cc = x.cc,
                                folio = x.folio,
                                id_activo = x.id_activo,
                                id_tipo_activo = x.id_tipo_activo,
                                marca = x.marca,
                                modelo = x.modelo,
                                color = x.color,
                                num_serie = x.num_serie,
                                valor_activo = x.valor_activo,
                                compania = x.compania,
                                plan_desc = x.plan_desc,
                                condiciones = x.condiciones,
                                numpro = x.numpro,
                                factura = x.factura,
                                fec_factura = x.fec_factura,
                                empleado = (int)x.empleado,
                                empleadoSIGOPLAN = x.empleadoSIGOPLAN,
                                claveEmpleado = x.claveEmpleado,
                                licencia = x.licencia,
                                tipo = x.tipo,
                                fec_licencia = x.fec_licencia,
                                observaciones = x.observaciones,
                                fec_resguardo = x.fec_resguardo,
                                foto = x.foto,
                                estatus = x.estatus,
                                entrega = (int)x.entrega,
                                entregaSIGOPLAN = x.entregaSIGOPLAN,
                                autoriza = (int)x.autoriza,
                                autorizaSIGOPLAN = x.autorizaSIGOPLAN,
                                recibio = x.recibio,
                                recibioSIGOPLAN = x.recibioSIGOPLAN,
                                condiciones_ret = x.condiciones_ret,
                                fec_devolucion = x.fec_devolucion,
                                cantidad_resguardo = x.cantidad_resguardo,
                                alm_salida = x.alm_salida,
                                alm_entrada = x.alm_entrada,
                                foto_2 = x.foto_2,
                                foto_3 = x.foto_3,
                                foto_4 = x.foto_4,
                                foto_5 = x.foto_5,
                                costo_promedio = x.costo_promedio,
                                resguardo_parcial = x.resguardo_parcial,
                                insumoDesc = listaInsumos.Where(y => y.insumo == x.id_activo).Select(z => z.insumoDesc).FirstOrDefault(),
                                empleadoNombre = listaUsuariosResguardos.Where(y => (int)y.empleado == (int)x.empleado).Select(z => z.descripcion).FirstOrDefault(),
                                autorizaNombre = listaUsuariosResguardos.Where(y => (int)y.empleado == (int)x.autoriza).Select(z => z.descripcion).FirstOrDefault(),
                                recibioNombre = x.recibioSIGOPLAN != null ? listaUsuariosResguardos.Where(y => (int)y.empleado == (int)x.recibioSIGOPLAN).Select(z => z.descripcion).FirstOrDefault() : "",
                                entregaNombre = x.entregaSIGOPLAN != null ? listaUsuarios.Where(y => y.id == (int)x.entregaSIGOPLAN).Select(z => z.nombre + " " + z.apellidoPaterno + " " + z.apellidoMaterno).FirstOrDefault() : "",
                                alm_salidaDesc = listaAlmacenes.Where(y => Int32.Parse(y.almacen) == x.alm_salida).Select(z => z.descripcion).FirstOrDefault(),
                                tipo_activoDesc = "",
                                ccDesc = listaCentrosCosto.Where(y => y.cc == x.cc).Select(z => z.descripcion).FirstOrDefault()
                            }).ToList();

                            foreach (var res in resguardos)
                            {
                                res.alm_salidaDesc = res.alm_salida + "-" + res.alm_salidaDesc;
                                res.ccDesc = res.cc + "-" + res.ccDesc;
                                res.insumoDesc = res.id_activo + "-" + res.insumoDesc;
                                res.cantidad_resguardo = res.cantidad_resguardo;
                                res.resguardo_parcial = res.resguardo_parcial ?? 0;
                            }

                            if (resguardos.Any(x => x.estatus != "D"))
                            {
                                if (resguardos.Any(x => x.estatus == "P"))
                                {
                                    resguardos = resguardos.Where(x => (x.cantidad_resguardo - (decimal)x.resguardo_parcial) > 0).ToList();
                                }
                            }

                            return resguardos;
                            #endregion
                        }
                    default:
                        {
                            #region Resguardos México
                            var resguardosEK = consultaCheckProductivo(
                                                string.Format(@"SELECT 
                                                        res.*, 
                                                        i.descripcion AS insumoDesc, 
                                                        emp.descripcion AS empleadoNombre, 
                                                        aut.descripcion AS autorizaNombre, 
                                                        almSal.descripcion AS alm_salidaDesc, 
                                                        t_act.descripcion AS tipo_activoDesc, 
                                                        c.descripcion AS ccDesc 
                                                    FROM si_resguardo_activo_fijo res 
                                                        INNER JOIN insumos i ON res.id_activo = i.insumo 
                                                        INNER JOIN empleados emp ON res.empleado = emp.empleado 
                                                        INNER JOIN empleados aut ON res.autoriza = aut.empleado 
                                                        INNER JOIN si_almacen almSal ON res.alm_salida = almSal.almacen 
                                                        INNER JOIN si_tipo_activo_fijo t_act ON res.id_tipo_activo = t_act.id_tipo_activo 
                                                        INNER JOIN cc c ON res.cc = c.cc 
                                                    WHERE res.cc = '{0}' AND res.folio = {1}", cc, folio));

                            if (resguardosEK != null)
                            {
                                var resguardos = (List<ResguardoEKDTO>)resguardosEK.ToObject<List<ResguardoEKDTO>>();

                                foreach (var res in resguardos)
                                {
                                    res.alm_salidaDesc = res.alm_salida + "-" + res.alm_salidaDesc;
                                    res.ccDesc = res.cc + "-" + res.ccDesc;
                                    res.insumoDesc = res.id_activo + "-" + res.insumoDesc;
                                    res.cantidad_resguardo = res.cantidad_resguardo;
                                    res.resguardo_parcial = res.resguardo_parcial ?? 0;
                                }

                                if (resguardos.Any(x => x.estatus != "D"))
                                {
                                    if (resguardos.Any(x => x.estatus == "P"))
                                    {
                                        resguardos = resguardos.Where(x => (x.cantidad_resguardo - (decimal)x.resguardo_parcial) > 0).ToList();
                                    }
                                }

                                return resguardos;
                            }
                            else
                            {
                                return new List<ResguardoEKDTO>();
                            }
                            #endregion
                        }
                }
            }
            catch (Exception e)
            {
                return new List<ResguardoEKDTO>();
            }
        }

        public Dictionary<string, object> GetResguardo(string cc, int almacen, int folio)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var data = new List<ResguardoEKDTO>();

                switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
                {

                    case EmpresaEnum.Peru:
                        {
                            #region Resguardos Perú
                            var listaUsuarios = _context.tblP_Usuario.Where(x => x.estatus).ToList();
                            var listaUsuariosResguardos = _context.tblAlm_EmpleadoResguardo.Where(x => x.registroActivo).ToList();
                            var listaInsumosStarsoft = new List<MAEART>();
                            var listaAlmacenesStarsoft = new List<TABALM>();

                            using (var _starsoft = new MainContextPeruStarSoft003BDCOMUN())
                            {
                                listaInsumosStarsoft = _starsoft.MAEART.ToList();
                                listaAlmacenesStarsoft = _starsoft.TABALM.ToList();
                            }

                            var listaCentrosCosto = _context.tblP_CC.Where(x => x.estatus).ToList();

                            var resguardos = _context.tblAlm_Resguardo.Where(x => x.estatusRegistro && x.cc == cc && x.alm_salida == almacen && x.folio == folio).ToList().Select(x => new ResguardoEKDTO
                            {
                                cc = x.cc,
                                folio = x.folio,
                                id_activo = x.id_activo,
                                id_tipo_activo = x.id_tipo_activo,
                                marca = x.marca,
                                modelo = x.modelo,
                                color = x.color,
                                num_serie = x.num_serie,
                                valor_activo = x.valor_activo,
                                compania = x.compania,
                                plan_desc = x.plan_desc,
                                condiciones = x.condiciones,
                                numpro = x.numpro,
                                factura = x.factura,
                                fec_factura = x.fec_factura,
                                empleado = (int)x.empleado,
                                empleadoSIGOPLAN = x.empleadoSIGOPLAN,
                                claveEmpleado = x.claveEmpleado,
                                licencia = x.licencia,
                                tipo = x.tipo,
                                fec_licencia = x.fec_licencia,
                                observaciones = x.observaciones,
                                fec_resguardo = x.fec_resguardo,
                                foto = x.foto,
                                estatus = x.estatus,
                                entrega = (int)x.entregaSIGOPLAN, //entrega = (int)x.entrega,
                                entregaSIGOPLAN = x.entregaSIGOPLAN,
                                autoriza = (int)x.autoriza,
                                autorizaSIGOPLAN = x.autorizaSIGOPLAN,
                                recibio = x.recibio,
                                recibioSIGOPLAN = x.recibioSIGOPLAN,
                                condiciones_ret = x.condiciones_ret,
                                fec_devolucion = x.fec_devolucion,
                                cantidad_resguardo = x.cantidad_resguardo,
                                alm_salida = x.alm_salida,
                                alm_entrada = x.alm_entrada,
                                foto_2 = x.foto_2,
                                foto_3 = x.foto_3,
                                foto_4 = x.foto_4,
                                foto_5 = x.foto_5,
                                costo_promedio = x.costo_promedio,
                                resguardo_parcial = x.resguardo_parcial,

                                insumoDesc = listaInsumosStarsoft.Where(y => y.ACODIGO == ("0" + x.id_activo)).Select(z => z.ADESCRI).FirstOrDefault(),
                                empleadoNombre = listaUsuariosResguardos.Where(y => y.folio == (int)x.empleado).Select(z => z.nombre + " " + z.apellidoPaterno + " " + z.apellidoMaterno).FirstOrDefault(),
                                autorizaNombre = listaUsuariosResguardos.Where(y => y.folio == (int)x.autoriza).Select(z => z.nombre + " " + z.apellidoPaterno + " " + z.apellidoMaterno).FirstOrDefault(),
                                recibioNombre = x.recibioSIGOPLAN != null ? listaUsuariosResguardos.Where(y => y.folio == (int)x.recibioSIGOPLAN).Select(z => z.nombre + " " + z.apellidoPaterno + " " + z.apellidoMaterno).FirstOrDefault() : "",
                                entregaNombre = x.entregaSIGOPLAN != null ? listaUsuarios.Where(y => y.id == (int)x.entregaSIGOPLAN).Select(z => z.nombre + " " + z.apellidoPaterno + " " + z.apellidoMaterno).FirstOrDefault() : "",
                                alm_salidaDesc = listaAlmacenesStarsoft.Where(y => Int32.Parse(y.TAALMA) == x.alm_salida).Select(z => z.TADESCRI).FirstOrDefault(),
                                tipo_activoDesc = "",
                                ccDesc = listaCentrosCosto.Where(y => y.cc == x.cc).Select(z => z.descripcion).FirstOrDefault()
                            }).ToList();

                            var alm_salida = resguardos[0].alm_salida;
                            var alm_entrada = resguardos[0].alm_entrada;
                            var folioResguardo = resguardos[0].folio;

                            var movimiento = _context.tblAlm_Movimientos.Where(x =>
                                x.estatusHabilitado &&
                                x.almacen == alm_salida &&
                                x.alm_destino == alm_entrada &&
                                x.tipo_mov == 52 &&
                                x.numero == folioResguardo
                            ).Join(
                                _context.tblAlm_MovimientosDet.Where(x => x.estatusHabilitado),
                                m => new { m.almacen, m.tipo_mov, m.numero },
                                d => new { d.almacen, d.tipo_mov, d.numero },
                                (m, d) => new { m, d }
                            ).ToList();

                            if (movimiento.Count() == 0)
                            {
                                throw new Exception("No se encuentra la información de la salida por traspaso para el folio \"" + resguardos[0].folio + "\" en el almacén \"" + resguardos[0].alm_salida + "\".");
                            }

                            int index = 0;
                            foreach (var res in resguardos)
                            {
                                res.cantidad_resguardo = res.cantidad_resguardo - (res.resguardo_parcial ?? 0);
                                res.area_alm = movimiento[index].d.area_alm;
                                res.lado_alm = movimiento[index].d.lado_alm;
                                res.estante_alm = movimiento[index].d.estante_alm;
                                res.nivel_alm = movimiento[index].d.nivel_alm;

                                index++;

                                if (res.fec_devolucion != null)
                                {
                                    res.fec_devolucionString = ((DateTime)res.fec_devolucion).ToShortDateString();
                                }
                                else
                                {
                                    res.fec_devolucionString = "";
                                }

                                if (res.fec_resguardo != null)
                                {
                                    res.fec_resguardoString = ((DateTime)res.fec_resguardo).ToShortDateString();
                                }
                                else
                                {
                                    res.fec_resguardoString = "";
                                }
                            }

                            data = resguardos;

                            break;
                            #endregion
                        }
                    case EmpresaEnum.Colombia:
                        {
                            #region Resguardos Colombia
                            var listaUsuarios = _context.tblP_Usuario.Where(x => x.estatus).ToList();
                            var listaCentrosCosto = _context.tblP_CC.Where(x => x.estatus).ToList();
                            var listaInsumos = _contextEnkontrol.Select<InsumoCatalogoDTO>(getEnkontrolEnumADM(), new OdbcConsultaDTO
                            {
                                consulta = @"SELECT ins.insumo, ins.descripcion AS insumoDesc FROM insumos ins"
                            });
                            var listaUsuariosResguardos = _contextEnkontrol.Select<dynamic>(getEnkontrolEnumADM(), new OdbcConsultaDTO
                            {
                                consulta = @"SELECT * FROM empleados"
                            });
                            var listaAlmacenes = _contextEnkontrol.Select<dynamic>(getEnkontrolEnumADM(), new OdbcConsultaDTO
                            {
                                consulta = @"SELECT * FROM si_almacen"
                            });

                            var resguardos = _context.tblAlm_Resguardo.Where(x => x.estatusRegistro && x.cc == cc && x.alm_salida == almacen && x.folio == folio).ToList().Select(x => new ResguardoEKDTO
                            {
                                cc = x.cc,
                                folio = x.folio,
                                id_activo = x.id_activo,
                                id_tipo_activo = x.id_tipo_activo,
                                marca = x.marca,
                                modelo = x.modelo,
                                color = x.color,
                                num_serie = x.num_serie,
                                valor_activo = x.valor_activo,
                                compania = x.compania,
                                plan_desc = x.plan_desc,
                                condiciones = x.condiciones,
                                numpro = x.numpro,
                                factura = x.factura,
                                fec_factura = x.fec_factura,
                                empleado = (int)x.empleado,
                                empleadoSIGOPLAN = x.empleadoSIGOPLAN,
                                claveEmpleado = x.claveEmpleado,
                                licencia = x.licencia,
                                tipo = x.tipo,
                                fec_licencia = x.fec_licencia,
                                observaciones = x.observaciones,
                                fec_resguardo = x.fec_resguardo,
                                foto = x.foto,
                                estatus = x.estatus,
                                entrega = (int)x.entregaSIGOPLAN, //entrega = (int)x.entrega,
                                entregaSIGOPLAN = x.entregaSIGOPLAN,
                                autoriza = (int)x.autoriza,
                                autorizaSIGOPLAN = x.autorizaSIGOPLAN,
                                recibio = x.recibio,
                                recibioSIGOPLAN = x.recibioSIGOPLAN,
                                condiciones_ret = x.condiciones_ret,
                                fec_devolucion = x.fec_devolucion,
                                cantidad_resguardo = x.cantidad_resguardo,
                                alm_salida = x.alm_salida,
                                alm_entrada = x.alm_entrada,
                                foto_2 = x.foto_2,
                                foto_3 = x.foto_3,
                                foto_4 = x.foto_4,
                                foto_5 = x.foto_5,
                                costo_promedio = x.costo_promedio,
                                resguardo_parcial = x.resguardo_parcial,

                                insumoDesc = listaInsumos.Where(y => y.insumo == x.id_activo).Select(z => z.insumoDesc).FirstOrDefault(),
                                empleadoNombre = listaUsuariosResguardos.Where(y => (int)y.empleado == (int)x.empleado).Select(z => z.descripcion).FirstOrDefault(),
                                autorizaNombre = listaUsuariosResguardos.Where(y => (int)y.empleado == (int)x.autoriza).Select(z => z.descripcion).FirstOrDefault(),
                                recibioNombre = x.recibioSIGOPLAN != null ? listaUsuariosResguardos.Where(y => (int)y.empleado == (int)x.recibioSIGOPLAN).Select(z => z.descripcion).FirstOrDefault() : "",
                                entregaNombre = x.entregaSIGOPLAN != null ? listaUsuarios.Where(y => y.id == (int)x.entregaSIGOPLAN).Select(z => z.nombre + " " + z.apellidoPaterno + " " + z.apellidoMaterno).FirstOrDefault() : "",
                                alm_salidaDesc = listaAlmacenes.Where(y => Int32.Parse(y.almacen) == x.alm_salida).Select(z => z.descripcion).FirstOrDefault(),
                                tipo_activoDesc = "",
                                ccDesc = listaCentrosCosto.Where(y => y.cc == x.cc).Select(z => z.descripcion).FirstOrDefault()
                            }).ToList();

                            if (resguardos.Count() == 0)
                            {
                                throw new Exception("No se encuentra la información del resguardo. Verifique que el centro de costo sea el correcto para el folio indicado.");
                            }

                            var alm_salida = resguardos[0].alm_salida;
                            var alm_entrada = resguardos[0].alm_entrada;
                            var folioResguardo = resguardos[0].folio;

                            var movimiento = _context.tblAlm_Movimientos.Where(x =>
                                x.estatusHabilitado &&
                                x.almacen == alm_salida &&
                                x.alm_destino == alm_entrada &&
                                x.tipo_mov == 52 &&
                                x.numero == folioResguardo
                            ).Join(
                                _context.tblAlm_MovimientosDet.Where(x => x.estatusHabilitado),
                                m => new { m.almacen, m.tipo_mov, m.numero },
                                d => new { d.almacen, d.tipo_mov, d.numero },
                                (m, d) => new { m, d }
                            ).ToList();

                            if (movimiento.Count() == 0)
                            {
                                throw new Exception("No se encuentra la información de la salida por traspaso para el folio \"" + resguardos[0].folio + "\" en el almacén \"" + resguardos[0].alm_salida + "\".");
                            }

                            int index = 0;
                            foreach (var res in resguardos)
                            {
                                res.cantidad_resguardo = res.cantidad_resguardo - (res.resguardo_parcial ?? 0);
                                res.area_alm = movimiento[index].d.area_alm;
                                res.lado_alm = movimiento[index].d.lado_alm;
                                res.estante_alm = movimiento[index].d.estante_alm;
                                res.nivel_alm = movimiento[index].d.nivel_alm;

                                index++;

                                if (res.fec_devolucion != null)
                                {
                                    res.fec_devolucionString = ((DateTime)res.fec_devolucion).ToShortDateString();
                                }
                                else
                                {
                                    res.fec_devolucionString = "";
                                }

                                if (res.fec_resguardo != null)
                                {
                                    res.fec_resguardoString = ((DateTime)res.fec_resguardo).ToShortDateString();
                                }
                                else
                                {
                                    res.fec_resguardoString = "";
                                }
                            }

                            data = resguardos;

                            break;
                            #endregion
                        }
                    default:
                        {
                            #region Resguardos México
                            var resguardosEK = consultaCheckProductivo(string.Format(@"
                        SELECT 
                            res.*, 
                            i.descripcion AS insumoDesc, 
                            emp.descripcion AS empleadoNombre, 
                            aut.descripcion AS autorizaNombre, 
                            rec.descripcion AS recibioNombre, 
                            ent.descripcion AS entregaNombre, 
                            almSal.descripcion AS alm_salidaDesc, 
                            t_act.descripcion AS tipo_activoDesc, 
                            c.descripcion AS ccDesc 
                        FROM si_resguardo_activo_fijo res 
                            INNER JOIN insumos i ON res.id_activo = i.insumo 
                            LEFT JOIN empleados emp ON res.empleado = emp.empleado 
                            LEFT JOIN empleados aut ON res.autoriza = aut.empleado 
                            LEFT JOIN empleados rec ON res.recibio = rec.empleado 
                            LEFT JOIN empleados ent ON res.entrega = ent.empleado 
                            INNER JOIN si_almacen almSal ON res.alm_salida = almSal.almacen 
                            INNER JOIN si_tipo_activo_fijo t_act ON res.id_tipo_activo = t_act.id_tipo_activo 
                            INNER JOIN cc c ON res.cc = c.cc 
                        WHERE res.cc = '{0}' AND res.alm_salida = {1} AND res.folio = {2}", cc, almacen, folio)
                            );

                            if (resguardosEK != null)
                            {
                                var resguardos = (List<ResguardoEKDTO>)resguardosEK.ToObject<List<ResguardoEKDTO>>();

                                var movimientoEK = consultaCheckProductivo(string.Format(@"
                        SELECT 
                            mov.*, 
                            det.* 
                        FROM si_movimientos mov
                            INNER JOIN si_movimientos_det det ON mov.almacen = det.almacen AND mov.tipo_mov = det.tipo_mov AND mov.numero = det.numero
                        WHERE mov.almacen = {0} AND mov.alm_destino = {1} AND mov.tipo_mov = 52 AND mov.numero = {2}", resguardos[0].alm_salida, resguardos[0].alm_entrada, resguardos[0].folio)
                                );

                                if (movimientoEK == null)
                                {
                                    throw new Exception("No se encuentra la información de la salida por traspaso para el folio \"" + resguardos[0].folio + "\" en el almacén \"" + resguardos[0].alm_salida + "\".");
                                }

                                var movimiento = (List<dynamic>)movimientoEK.ToObject<List<dynamic>>();

                                int index = 0;
                                foreach (var res in resguardos)
                                {
                                    res.cantidad_resguardo = res.cantidad_resguardo - (res.resguardo_parcial ?? 0);
                                    res.area_alm = (string)movimiento[index].area_alm;
                                    res.lado_alm = (string)movimiento[index].lado_alm;
                                    res.estante_alm = (string)movimiento[index].estante_alm;
                                    res.nivel_alm = (string)movimiento[index].nivel_alm;

                                    index++;

                                    if (res.fec_devolucion != null)
                                    {
                                        res.fec_devolucionString = ((DateTime)res.fec_devolucion).ToShortDateString();
                                    }
                                    else
                                    {
                                        res.fec_devolucionString = "";
                                    }

                                    if (res.fec_resguardo != null)
                                    {
                                        res.fec_resguardoString = ((DateTime)res.fec_resguardo).ToShortDateString();
                                    }
                                    else
                                    {
                                        res.fec_resguardoString = "";
                                    }
                                }

                                data = resguardos;
                            }

                            break;
                            #endregion
                        }
                }

                resultado.Add("data", data);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, "ResguardoController", "GetResguardo", e, AccionEnum.CONSULTA, 0, new { cc = cc, almacen = almacen, folio = folio });
            }

            return resultado;
        }

        public Dictionary<string, object> guardarDevolucionNormal(List<ResguardoEKDTO> resguardos)
        {
            var result = new Dictionary<string, object>();

            if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Peru)
            {
                using (var dbSigoplanTransaction = _context.Database.BeginTransaction())
                {
                    using (var con = checkConexionProductivo())
                    {
                        using (var trans = con.BeginTransaction())
                        {
                            try
                            {
                                var r = new Dictionary<string, object>();

                                if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Colombia)
                                {
                                    r = guardarDevolucion(resguardos, trans, dbSigoplanTransaction);
                                }
                                else
                                {
                                    r = guardarDevolucionColombia(resguardos, trans, dbSigoplanTransaction);
                                }

                                trans.Commit();
                                dbSigoplanTransaction.Commit();

                                result.Add("data", resguardos);
                                result.Add("estatusFinal", (r["estatusFinal"]).ToString());
                                result.Add(SUCCESS, true);
                            }
                            catch (Exception e)
                            {
                                trans.Rollback();
                                dbSigoplanTransaction.Rollback();

                                LogError(12, 12, "ResguardoController", "guardarDevolucionNormal", e, AccionEnum.AGREGAR, 0, resguardos);

                                result.Add(MESSAGE, e.Message);
                                result.Add(SUCCESS, false);
                            }
                        }
                    }
                }
            }
            else
            {
                result = guardarDevolucionPeru(resguardos);
            }

            return result;
        }

        public Dictionary<string, object> guardarDevolucion(List<ResguardoEKDTO> resguardos, OdbcTransaction trans, DbContextTransaction dbSigoplanTransaction)
        {
            var result = new Dictionary<string, object>();

            //using (var dbSigoplanTransaction = _context.Database.BeginTransaction())
            //{
            //    using (var con = checkConexionProductivo())
            //    {
            //        using (var trans = con.BeginTransaction())
            //        {
            //            try
            //            {
            //#region Validación Centro de Costo con Presupuesto Terminado
            //var centroCostoEK = ((List<dynamic>)consultaCheckProductivo(string.Format(@"SELECT * FROM cc WHERE cc = '{0}'", resguardos[0].cc)).ToObject<List<dynamic>>())[0];
            //var presupuestoEstatus = (string)centroCostoEK.st_ppto;

            //if (presupuestoEstatus == "T")
            //{
            //    throw new Exception("El Centro de Costos tiene el presupuesto terminado.");
            //}
            //#endregion

            #region Validación ubicaciones diferentes a null o vacías
            if (resguardos.Any(x => x.area_alm == "" || x.lado_alm == "" || x.estante_alm == "" || x.nivel_alm == "" ||
                                    x.area_alm == null || x.lado_alm == null || x.estante_alm == null || x.nivel_alm == null))
            {
                throw new Exception("No se capturó una ubicación válida.");
            }
            #endregion

            checarUbicacionesValidas(resguardos);

            var count = 0;
            var folioMovimiento = guardarEntradaTraspasoResguardo(resguardos, resguardos[0].folio, trans);
            var relacionUsuario = _context.tblP_Usuario_Enkontrol.FirstOrDefault(x => x.idUsuario == vSesiones.sesionUsuarioDTO.id);

            foreach (var resguardo in resguardos)
            {
                var registroResguardoEK = consultaCheckProductivo(
                    string.Format(@"SELECT * FROM si_resguardo_activo_fijo WHERE cc = '{0}' AND folio = {1} AND id_activo = {2}", resguardo.cc, resguardo.folio, resguardo.id_activo)
                );

                if (registroResguardoEK == null)
                {
                    throw new Exception("No se encuentra el registro del resguardo para el insumo " + resguardo.id_activo + ".");
                }

                var registroResguardo = ((List<dynamic>)registroResguardoEK.ToObject<List<dynamic>>())[0];

                var consultaUpdate = @"
                                    UPDATE si_resguardo_activo_fijo 
                                    SET 
                                        estatus = ?, 
                                        recibio = ?, 
                                        condiciones_ret = ?, 
                                        fec_devolucion = ?, 
                                        resguardo_parcial = ? 
                                    WHERE cc = ? AND folio = ? AND id_activo = ?";

                using (var cmd = new OdbcCommand(consultaUpdate))
                {
                    OdbcParameterCollection parameters = cmd.Parameters;

                    parameters.Add("@estatus", OdbcType.Char).Value = resguardo.parcial ? "P" : "D";
                    parameters.Add("@recibio", OdbcType.Numeric).Value = relacionUsuario != null ? relacionUsuario.empleado : (object)DBNull.Value;
                    parameters.Add("@condiciones_ret", OdbcType.Char).Value = resguardo.condiciones;
                    parameters.Add("@fec_devolucion", OdbcType.Date).Value = DateTime.Now.Date;
                    parameters.Add("@resguardo_parcial", OdbcType.Numeric).Value =
                        Convert.ToDecimal(registroResguardo.resguardo_parcial != null ? registroResguardo.resguardo_parcial : 0, CultureInfo.InvariantCulture) +
                        resguardo.cantidad_resguardo;

                    parameters.Add("@cc", OdbcType.Char).Value = resguardo.cc;
                    parameters.Add("@folio", OdbcType.Char).Value = resguardo.folio;
                    parameters.Add("@id_activo", OdbcType.Char).Value = resguardo.id_activo;

                    cmd.Connection = trans.Connection;
                    cmd.Transaction = trans;

                    count += cmd.ExecuteNonQuery();
                }
            }

            #region Update el estatus de todos los registros del resguardo
            var listaResguardo = (List<dynamic>)consultaCheckProductivo(
                string.Format(@"SELECT * FROM si_resguardo_activo_fijo WHERE cc = '{0}' AND folio = {1}", resguardos[0].cc, resguardos[0].folio)
            ).ToObject<List<dynamic>>();
            List<string> listaEstatusIndividual = new List<string>();

            #region Determinar el Estatus general de los registros.
            foreach (var res in listaResguardo)
            {
                var cantidadResguardo = Convert.ToDecimal(res.cantidad_resguardo, CultureInfo.InvariantCulture);
                var cantidadDevuelto = Convert.ToDecimal(res.resguardo_parcial != null ? res.resguardo_parcial : 0, CultureInfo.InvariantCulture);
                var cantidadResultado = cantidadResguardo - cantidadDevuelto;

                if (cantidadDevuelto > 0)
                {
                    if (cantidadResultado > 0)
                    {
                        listaEstatusIndividual.Add("P");
                    }
                    else
                    {
                        listaEstatusIndividual.Add("D");
                    }
                }
                else
                {
                    listaEstatusIndividual.Add("V");
                }
            }

            var estatusFinal = "";

            if (listaEstatusIndividual.All(x => x == "V"))
            {
                estatusFinal = "V";
            }
            else if (listaEstatusIndividual.All(x => x == "P"))
            {
                estatusFinal = "P";
            }
            else if (listaEstatusIndividual.Any(x => x == "V" || x == "P"))
            {
                estatusFinal = "P";
            }
            else if (listaEstatusIndividual.All(x => x == "D"))
            {
                estatusFinal = "D";
            }
            else
            {
                throw new Exception("Estatus no controlado para el resguardo.");
            }
            #endregion

            var consultaUpdateResguardo = @"
                                                            UPDATE si_resguardo_activo_fijo 
                                                            SET 
                                                                estatus = ? 
                                                            WHERE cc = ? AND folio = ?";

            using (var cmd = new OdbcCommand(consultaUpdateResguardo))
            {
                OdbcParameterCollection parameters = cmd.Parameters;

                parameters.Add("@estatus", OdbcType.Char).Value = estatusFinal;

                parameters.Add("@cc", OdbcType.Char).Value = resguardos[0].cc;
                parameters.Add("@folio", OdbcType.Char).Value = resguardos[0].folio;

                cmd.Connection = trans.Connection;
                cmd.Transaction = trans;

                count += cmd.ExecuteNonQuery();
            }
            #endregion

            //trans.Commit();
            //dbSigoplanTransaction.Commit();

            //result.Add("data", resguardos);
            result.Add("estatusFinal", estatusFinal);
            result.Add(SUCCESS, true);
            //            }
            //            catch (Exception e)
            //            {
            //                trans.Rollback();
            //                dbSigoplanTransaction.Rollback();

            //                result.Add(MESSAGE, e.Message);
            //                result.Add(SUCCESS, false);
            //            }
            //        }
            //    }
            //}

            return result;
        }

        public Dictionary<string, object> guardarDevolucionColombia(List<ResguardoEKDTO> resguardos, OdbcTransaction trans, DbContextTransaction dbSigoplanTransaction)
        {
            var result = new Dictionary<string, object>();

            #region Validación ubicaciones diferentes a null o vacías
            if (resguardos.Any(x => x.area_alm == "" || x.lado_alm == "" || x.estante_alm == "" || x.nivel_alm == "" ||
                                    x.area_alm == null || x.lado_alm == null || x.estante_alm == null || x.nivel_alm == null))
            {
                throw new Exception("No se capturó una ubicación válida.");
            }
            #endregion

            #region Validación Ubicaciones Válidas
            foreach (var res in resguardos)
            {
                var registroUbicacion = _context.tblAlm_Ubicacion.FirstOrDefault(x =>
                    x.registroActivo && x.almacen == res.alm_salida && x.area_alm == res.area_alm && x.lado_alm == res.lado_alm && x.estante_alm == res.estante_alm && x.nivel_alm == res.nivel_alm
                );

                if (registroUbicacion == null)
                {
                    throw new Exception("Debe capturar ubicaciones válidas para el almacén.");
                }
            }
            #endregion

            var folioMovimiento = guardarEntradaTraspasoResguardoColombia(resguardos, resguardos[0].folio, trans);
            var relacionUsuario = _context.tblP_Usuario_Enkontrol.FirstOrDefault(x => x.idUsuario == vSesiones.sesionUsuarioDTO.id);

            foreach (var resguardo in resguardos)
            {
                var registroResguardo = _context.tblAlm_Resguardo.FirstOrDefault(x => x.estatusRegistro && x.cc == resguardo.cc && x.folio == resguardo.folio && x.id_activo == resguardo.id_activo);

                if (registroResguardo == null)
                {
                    throw new Exception("No se encuentra el registro del resguardo para el insumo " + resguardo.id_activo + ".");
                }

                registroResguardo.estatus = resguardo.parcial ? "P" : "D";
                registroResguardo.recibio = relacionUsuario != null ? relacionUsuario.empleado : 0;
                registroResguardo.condiciones_ret = resguardo.condiciones;
                registroResguardo.fec_devolucion = DateTime.Now.Date;
                registroResguardo.resguardo_parcial = Convert.ToDecimal(registroResguardo.resguardo_parcial != null ? registroResguardo.resguardo_parcial : 0, CultureInfo.InvariantCulture) + resguardo.cantidad_resguardo;

                _context.SaveChanges();
            }

            #region Update el estatus de todos los registros del resguardo
            var cc = resguardos[0].cc;
            var folio = resguardos[0].folio;
            var listaResguardo = _context.tblAlm_Resguardo.Where(x => x.estatusRegistro && x.cc == cc && x.folio == folio).ToList();

            List<string> listaEstatusIndividual = new List<string>();

            #region Determinar el Estatus general de los registros.
            foreach (var res in listaResguardo)
            {
                var cantidadResguardo = Convert.ToDecimal(res.cantidad_resguardo, CultureInfo.InvariantCulture);
                var cantidadDevuelto = Convert.ToDecimal(res.resguardo_parcial != null ? res.resguardo_parcial : 0, CultureInfo.InvariantCulture);
                var cantidadResultado = cantidadResguardo - cantidadDevuelto;

                if (cantidadDevuelto > 0)
                {
                    if (cantidadResultado > 0)
                    {
                        listaEstatusIndividual.Add("P");
                    }
                    else
                    {
                        listaEstatusIndividual.Add("D");
                    }
                }
                else
                {
                    listaEstatusIndividual.Add("V");
                }
            }

            var estatusFinal = "";

            if (listaEstatusIndividual.All(x => x == "V"))
            {
                estatusFinal = "V";
            }
            else if (listaEstatusIndividual.All(x => x == "P"))
            {
                estatusFinal = "P";
            }
            else if (listaEstatusIndividual.Any(x => x == "V" || x == "P"))
            {
                estatusFinal = "P";
            }
            else if (listaEstatusIndividual.All(x => x == "D"))
            {
                estatusFinal = "D";
            }
            else
            {
                throw new Exception("Estatus no controlado para el resguardo.");
            }
            #endregion

            foreach (var res in listaResguardo)
            {
                res.estatus = estatusFinal;
                _context.SaveChanges();
            }
            #endregion

            result.Add("estatusFinal", estatusFinal);
            result.Add(SUCCESS, true);

            return result;
        }

        public Dictionary<string, object> guardarDevolucionPeru(List<ResguardoEKDTO> resguardos)
        {
            var result = new Dictionary<string, object>();

            using (var dbSigoplanTransaction = _context.Database.BeginTransaction())
            {
                using (var _starsoft = new MainContextPeruStarSoft003BDCOMUN())
                {
                    using (var dbStarsoftTransaction = _starsoft.Database.BeginTransaction())
                    {
                        try
                        {
                            #region Validación ubicaciones diferentes a null o vacías
                            if (resguardos.Any(x => x.area_alm == "" || x.lado_alm == "" || x.estante_alm == "" || x.nivel_alm == "" || x.area_alm == null || x.lado_alm == null || x.estante_alm == null || x.nivel_alm == null))
                            {
                                throw new Exception("No se capturó una ubicación válida.");
                            }
                            #endregion

                            #region Validación Usuario Inventarios Starsoft
                            var objUsrStarsoftInventarios = _context.tblAlm_Almacenistas.FirstOrDefault(e => e.id_usuario_sigoplan == vSesiones.sesionUsuarioDTO.id);

                            string idAlmacenistaStarsoft = "0";

                            if (objUsrStarsoftInventarios == null)
                            {
                                if (vSesiones.sesionUsuarioDTO.idPerfil == 1)
                                {
                                    idAlmacenistaStarsoft = "1";
                                }
                                else
                                {
                                    throw new Exception("Esta usuario no es un almacenista en el sistema");
                                }
                            }
                            else
                            {
                                idAlmacenistaStarsoft = objUsrStarsoftInventarios.id_usuario_inventarios;
                            }
                            #endregion

                            string cc = resguardos[0].cc;
                            int folio = resguardos[0].folio;

                            var relUser = ufs.getUsuarioService().getUserEk(vSesiones.sesionUsuarioDTO.id);

                            if (relUser == null)
                            {
                                throw new Exception("No se encuentra la relación SIGOPLAN-Enkontrol del usuario logeado.");
                            }

                            var nuevoFolio = 1;
                            var alm_salida = resguardos[0].alm_salida;
                            var ultimoFolio = _context.tblAlm_Movimientos.Where(x => x.estatusHabilitado && x.almacen == alm_salida && x.tipo_mov < 50).ToList().Select(x => x.numero).OrderByDescending(x => x).FirstOrDefault();
                            //int ultimoFolio = _starsoft.MOVALMCAB.ToList().Where(x => Int32.Parse(x.CAALMA) == alm_salida && x.CATD == "NI").Select(x => Int32.Parse(x.CANUMDOC)).OrderByDescending(x => x).FirstOrDefault();

                            //if (ultimoFolio != null)
                            //{
                            nuevoFolio = ultimoFolio + 1;
                            //}

                            string almacenStarsoft = resguardos[0].alm_salida > 9 ? resguardos[0].alm_salida.ToString() : ("0" + resguardos[0].alm_salida);
                            decimal tipoCambioPeru = 0M;

                            using (var dbStarsoftConta = new MainContextPeruStarSoft003BDCONTABILIDAD())
                            {
                                var tipoCambioActual = dbStarsoftConta.TIPO_CAMBIO.ToList().FirstOrDefault(e => e.TIPOCAMB_FECHA.Date == DateTime.Now.Date);

                                if (tipoCambioActual != null)
                                {
                                    tipoCambioPeru = tipoCambioActual.TIPOCAMB_COMPRA;
                                }
                                else
                                {
                                    tipoCambioPeru = dbStarsoftConta.TIPO_CAMBIO.ToList().FirstOrDefault(e => e.TIPOCAMB_FECHA.Date == DateTime.Now.AddDays(-1).Date).TIPOCAMB_COMPRA;
                                }
                            }

                            var listaInsumosStarsoft = _starsoft.MAEART.ToList();

                            #region Guardar Entrada Traspaso Resguardo
                            #region Guardar Encabezado SIGOPLAN
                            tblAlm_Movimientos nuevaEntrada = new tblAlm_Movimientos
                            {
                                almacen = resguardos[0].alm_salida,
                                tipo_mov = 2,
                                numero = nuevoFolio,
                                cc = resguardos[0].cc.ToUpper(),
                                compania = 1,
                                periodo = DateTime.Now.Month,
                                ano = DateTime.Now.Year,
                                orden_ct = 0,
                                frente = 0,
                                fecha = DateTime.Now.Date,
                                proveedor = 0,
                                total = resguardos.Select(x => x.cantidad_resguardo).Sum(),
                                estatus = "A",
                                transferida = "N",
                                alm_destino = resguardos[0].alm_entrada,
                                cc_destino = resguardos[0].cc.ToUpper(),
                                comentarios = resguardos[0].observaciones,
                                tipo_trasp = "0",
                                tipo_cambio = 1,
                                numeroReq = null,
                                estatusHabilitado = true
                            };

                            _context.tblAlm_Movimientos.Add(nuevaEntrada);
                            _context.SaveChanges();

                            if (nuevaEntrada.total <= 0)
                            {
                                throw new Exception("El total no puede ser igual o menor a cero.");
                            }
                            #endregion

                            //#region Guardar Encabezado Starsoft
                            //MOVALMCAB nuevaEntradaStarsoft = new MOVALMCAB();

                            //nuevaEntradaStarsoft.CAALMA = almacenStarsoft;
                            //nuevaEntradaStarsoft.CATD = "NI";
                            //nuevaEntradaStarsoft.CANUMDOC = nuevoFolio.ToString("D10");
                            //nuevaEntradaStarsoft.CAFECDOC = DateTime.Now.Date;
                            //nuevaEntradaStarsoft.CATIPMOV = "I";
                            //nuevaEntradaStarsoft.CACODMOV = "TD";
                            //nuevaEntradaStarsoft.CASITUA = "M";
                            //nuevaEntradaStarsoft.CARFTDOC = ""; //PENDIENTE CHECAR CUANDO DEJA VER EL STOCK DE LOS INSUMOS EN EL ALMACEN (ASI SI DEJA AGREGARLOS;
                            //nuevaEntradaStarsoft.CARFNDOC = null;
                            //nuevaEntradaStarsoft.CASOLI = null;
                            //nuevaEntradaStarsoft.CAFECDEV = null;
                            //nuevaEntradaStarsoft.CACODPRO = null;
                            //nuevaEntradaStarsoft.CACENCOS = resguardos[0].cc;
                            //nuevaEntradaStarsoft.CARFALMA = null;
                            //nuevaEntradaStarsoft.CAGLOSA = resguardos[0].observaciones;
                            //nuevaEntradaStarsoft.CAFECACT = DateTime.Now.Date;
                            //nuevaEntradaStarsoft.CAHORA = DateTime.Now.ToString("HH:mm:ss");
                            //nuevaEntradaStarsoft.CAUSUARI = idAlmacenistaStarsoft;
                            //nuevaEntradaStarsoft.CACODCLI = null;
                            //nuevaEntradaStarsoft.CARUC = null;
                            //nuevaEntradaStarsoft.CANOMCLI = null;
                            //nuevaEntradaStarsoft.CAFORVEN = null;
                            //nuevaEntradaStarsoft.CACODMON = "MN"; //PENDIENTE TIPO DE MONED;
                            //nuevaEntradaStarsoft.CAVENDE = null;
                            //nuevaEntradaStarsoft.CATIPCAM = tipoCambioPeru;
                            //nuevaEntradaStarsoft.CATIPGUI = null;
                            //nuevaEntradaStarsoft.CASITGUI = "V";
                            //nuevaEntradaStarsoft.CAGUIFAC = null;
                            //nuevaEntradaStarsoft.CADIRENV = null;
                            //nuevaEntradaStarsoft.CACODTRAN = null;
                            //nuevaEntradaStarsoft.CANUMORD = null;
                            //nuevaEntradaStarsoft.CAGUIDEV = null;
                            //nuevaEntradaStarsoft.CANOMPRO = null;
                            //nuevaEntradaStarsoft.CANROPED = null;
                            //nuevaEntradaStarsoft.CACOTIZA = null;
                            //nuevaEntradaStarsoft.CAPORDESCL = 0M;
                            //nuevaEntradaStarsoft.CAPORDESES = 0M;
                            //nuevaEntradaStarsoft.CAIMPORTE = resguardos.Select(x => x.cantidad_resguardo).Sum();
                            //nuevaEntradaStarsoft.CANOMTRA = null;
                            //nuevaEntradaStarsoft.CADIRTRA = null;
                            //nuevaEntradaStarsoft.CARUCTRA = null;
                            //nuevaEntradaStarsoft.CAPLATRA = null;
                            //nuevaEntradaStarsoft.CANROIMP = null;
                            //nuevaEntradaStarsoft.CACODLIQ = null;
                            //nuevaEntradaStarsoft.CAESTIMP = null;
                            //nuevaEntradaStarsoft.CACIERRE = false;
                            //nuevaEntradaStarsoft.CATIPDEP = null;
                            //nuevaEntradaStarsoft.CAZONAF = null;
                            //nuevaEntradaStarsoft.FLAGGS = false;
                            //nuevaEntradaStarsoft.ASIENTO = false;
                            //nuevaEntradaStarsoft.CAFLETE = 0M;
                            //nuevaEntradaStarsoft.CAORDFAB = "";
                            //nuevaEntradaStarsoft.CAPEDREFE = null;
                            //nuevaEntradaStarsoft.CAIMPORTACION = false;
                            //nuevaEntradaStarsoft.CANROCAJAS = 0;
                            //nuevaEntradaStarsoft.CAPESOTOTAL = 0M;
                            //nuevaEntradaStarsoft.CADESPACHO = false;
                            //nuevaEntradaStarsoft.LINVCODIGO = null;
                            //nuevaEntradaStarsoft.COD_DIRECCION = null;
                            //nuevaEntradaStarsoft.COSTOMIN = 0M;
                            //nuevaEntradaStarsoft.CAINTERFACE = 0;
                            //nuevaEntradaStarsoft.CACTACONT = null;
                            //nuevaEntradaStarsoft.CACONTROLSTOCK = "N";
                            //nuevaEntradaStarsoft.CANOMRECEP = null;
                            //nuevaEntradaStarsoft.CADNIRECEP = null;
                            //nuevaEntradaStarsoft.CFDIREREFE = null;
                            //nuevaEntradaStarsoft.REG_COMPRA = false;
                            //nuevaEntradaStarsoft.OC_NI_GUIA = false;
                            //nuevaEntradaStarsoft.COD_AUDITORIA = "0";
                            //nuevaEntradaStarsoft.COD_MODULO = "03";
                            //nuevaEntradaStarsoft.NO_GIRO_NEGOCIO = false;
                            //nuevaEntradaStarsoft.MOTIVO_ANULACION_DOC_ELECTRONICO = null;
                            //nuevaEntradaStarsoft.DOCUMENTO_ELECTRONICO = null;
                            //nuevaEntradaStarsoft.GS_BAJA = null;
                            //nuevaEntradaStarsoft.CADocumentoImportado = null;
                            //nuevaEntradaStarsoft.SOLICITANTE = null;
                            //nuevaEntradaStarsoft.DOCUMENTO_CONTINGENCIA = null;
                            //nuevaEntradaStarsoft.GE_BAJA = null;

                            //_starsoft.MOVALMCAB.Add(nuevaEntradaStarsoft);
                            //_starsoft.SaveChanges();
                            //#endregion

                            //#region Actualizar Registro Almacén Starsoft
                            //var registroAlmacenStarsoft = _starsoft.TABALM.ToList().FirstOrDefault(x => Int32.Parse(x.TAALMA) == resguardos[0].alm_salida);

                            //if (registroAlmacenStarsoft != null)
                            //{
                            //    registroAlmacenStarsoft.TANUMENT = nuevoFolio;
                            //    _starsoft.SaveChanges();
                            //}
                            //#endregion

                            var partidaContador = 1;

                            foreach (var res in resguardos)
                            {
                                #region Guardar Detalle SIGOPLAN
                                var nuevaEntradaDet = new tblAlm_MovimientosDet
                                {
                                    almacen = res.alm_salida,
                                    tipo_mov = 2,
                                    numero = nuevoFolio,
                                    partida = partidaContador,
                                    insumo = res.id_activo,
                                    comentarios = res.observaciones,
                                    area = 0,
                                    cuenta = 1,
                                    cantidad = res.cantidad_resguardo,
                                    precio = 1,
                                    importe = 1,
                                    id_resguardo = res.id_tipo_activo,
                                    area_alm = res.area_alm ?? "",
                                    lado_alm = res.lado_alm ?? "",
                                    estante_alm = res.estante_alm ?? "",
                                    nivel_alm = res.nivel_alm ?? "",
                                    transporte = "",
                                    estatusHabilitado = true
                                };

                                _context.tblAlm_MovimientosDet.Add(nuevaEntradaDet);
                                _context.SaveChanges();
                                #endregion

                                var objInsumo = listaInsumosStarsoft.FirstOrDefault(e => e.ACODIGO == ("0" + res.id_activo.ToString()));

                                //#region Guardar Detalle Starsoft
                                //_starsoft.MovAlmDet.Add(new MovAlmDet()
                                //{
                                //    DEALMA = almacenStarsoft,
                                //    DETD = "NI",
                                //    DENUMDOC = nuevoFolio.ToString("D10"),
                                //    DEITEM = partidaContador,
                                //    DECODIGO = "0" + res.id_activo.ToString(),
                                //    DECODREF = null,
                                //    DECANTID = res.cantidad_resguardo,
                                //    DECANTENT = 0M,
                                //    DECANREF = 0M,
                                //    DECANFAC = 0M,
                                //    DEORDEN = null,
                                //    DEPREUNI = 1,
                                //    DEPRECIO = 1,
                                //    DEPRECI1 = 1,
                                //    DEDESCTO = 0M,
                                //    DESTOCK = null,
                                //    DEIGV = 0M,
                                //    DEIMPMN = 1,
                                //    DEIMPUS = 0,
                                //    DESERIE = null,
                                //    DESITUA = null,
                                //    DEFECDOC = null,
                                //    DECENCOS = res.cc,
                                //    DERFALMA = null,
                                //    DETR = null,
                                //    DEESTADO = "V",
                                //    DECODMOV = "TD",
                                //    DEVALTOT = 0,
                                //    DECOMPRO = null,
                                //    DECODMON = "MN",
                                //    DETIPO = null,
                                //    DETIPCAM = tipoCambioPeru,
                                //    DEPREVTA = null,
                                //    DEMONVTA = null,
                                //    DEFECVEN = null,
                                //    DEDEVOL = 0M,
                                //    DESOLI = null,
                                //    DEDESCRI = objInsumo.ADESCRI,
                                //    DEPORDES = 0M,
                                //    DEIGVPOR = 0M,
                                //    DEDESCLI = 0M,
                                //    DEDESESP = 0M,
                                //    DENUMFAC = null,
                                //    DELOTE = null,
                                //    DEUNIDAD = objInsumo.AUNIDAD,
                                //    DECANTBRUTA = 0M,
                                //    DEDSCTCANTBRUTA = 0M,
                                //    DEORDFAB = "",
                                //    DEQUIPO = null,
                                //    DEFLETE = 0M,
                                //    DEITEMI = null, //????????
                                //    DEGLOSA = "",
                                //    DEVALORIZADO = true,
                                //    DESECUENORI = null,
                                //    DEREFERENCIA = null,
                                //    UMREFERENCIA = null,
                                //    CANTREFERENCIA = 0M,
                                //    DECUENTA = null,
                                //    DETEXTO = null,
                                //    CTA_CONSUMO = null,
                                //    CODPARTE = "",
                                //    CODPLANO = "",
                                //    DETPRODUCCION = 0,
                                //    MPMA = "",
                                //    PorcentajeCosto = 0M,
                                //    SALDO_NC = null,
                                //    DEPRECIOREF = 0M,
                                //});
                                //_starsoft.SaveChanges();
                                //#endregion

                                //#region Insert/Update STKART
                                //var registroSTKART = _starsoft.STKART.ToList().FirstOrDefault(e => e.STALMA == almacenStarsoft && e.STCODIGO == ("0" + res.id_activo.ToString()));

                                //if (registroSTKART != null)
                                //{
                                //    registroSTKART.STSKDIS += res.cantidad_resguardo;
                                //    _starsoft.SaveChanges();
                                //}
                                //else
                                //{
                                //    _starsoft.STKART.Add(new STKART()
                                //    {
                                //        STALMA = almacenStarsoft,
                                //        STCODIGO = ("0" + res.id_activo.ToString()),
                                //        STSKDIS = res.cantidad_resguardo,
                                //        STSKREF = 0M,
                                //        STSKMIN = 0M,
                                //        STSKMAX = 0M,
                                //        STPUNREP = 0M,
                                //        STSEMREP = 0M,
                                //        STTIPREP = null,
                                //        STUBIALM = null,
                                //        STLOTCOM = 0M,
                                //        STTIPCOM = null,
                                //        STSKCOM = 0M,
                                //        STKPREPRO = 0M,
                                //        STKPREULT = 0M,
                                //        STKFECULT = DateTime.Now.Date,
                                //        STKPREPROUS = 0M,
                                //        CANTREFERENCIA = 0M,
                                //    });
                                //    _starsoft.SaveChanges();
                                //}
                                //#endregion

                                //#region Insert/Update MORESMES
                                //var registroMoResMes = _starsoft.MoResMes.ToList().FirstOrDefault(e =>
                                //    e.SMALMA == almacenStarsoft &&
                                //    e.SMMESPRO == (DateTime.Now.Year.ToString() + DateTime.Now.ToString("MM")) &&
                                //    e.SMCODIGO == ("0" + res.id_activo.ToString())
                                //);

                                //if (registroMoResMes != null)
                                //{
                                //    registroMoResMes.SMCANENT += res.cantidad_resguardo;
                                //    _starsoft.SaveChanges();
                                //}
                                //else
                                //{
                                //    var objCrearMoResMes = new MoResMes();
                                //    objCrearMoResMes.SMALMA = almacenStarsoft;
                                //    objCrearMoResMes.SMCODIGO = ("0" + res.id_activo.ToString());
                                //    objCrearMoResMes.SMMESPRO = (DateTime.Now.Year.ToString() + DateTime.Now.ToString("MM"));
                                //    objCrearMoResMes.SMUSPREUNI = 0M; //CAMBIAR POR EL PRESIO ADECUEDO DEPENDE EL TIPO DE MONEDA CHECAR EN OTRAS TABLAS
                                //    objCrearMoResMes.SMMNPREUNI = 0M; //CAMBIAR POR EL PRESIO ADECUEDO DEPENDE EL TIPO DE MONEDA
                                //    objCrearMoResMes.SMUSPREANT = 0M;
                                //    objCrearMoResMes.SMULTMOV = null;
                                //    objCrearMoResMes.SMCANENT = res.cantidad_resguardo;
                                //    objCrearMoResMes.SMCANSAL = 0M;
                                //    objCrearMoResMes.SMANTCAN = 0M;
                                //    objCrearMoResMes.SMMNANTVAL = 0M;
                                //    objCrearMoResMes.SMMNACTVAL = 0M;
                                //    objCrearMoResMes.SMUSANTVAL = 0M;
                                //    objCrearMoResMes.SMUSACTVAL = 0M;
                                //    objCrearMoResMes.SMUSENT = 0M;
                                //    objCrearMoResMes.SMMNENT = 0M;
                                //    objCrearMoResMes.SMUSSAL = 0;
                                //    objCrearMoResMes.SMMNSAL = 0M;
                                //    objCrearMoResMes.SMCUENTA = null;
                                //    objCrearMoResMes.SMGRUPO = null;
                                //    objCrearMoResMes.SMFAMILIA = null;
                                //    objCrearMoResMes.SMLINEA = null;
                                //    objCrearMoResMes.SMTIPO = null;
                                //    objCrearMoResMes.SMSALDOINI = 0M;
                                //    objCrearMoResMes.COD_MODULO = "03";
                                //    objCrearMoResMes.COD_OPCION = "Men_TraRegEnt"; //??

                                //    _starsoft.MoResMes.Add(objCrearMoResMes);
                                //    _starsoft.SaveChanges();
                                //}
                                //#endregion

                                partidaContador++;
                            }
                            #endregion

                            var relacionUsuario = _context.tblP_Usuario_Enkontrol.FirstOrDefault(x => x.idUsuario == vSesiones.sesionUsuarioDTO.id);

                            foreach (var resguardo in resguardos)
                            {
                                var registroResguardo = _context.tblAlm_Resguardo.FirstOrDefault(x => x.estatusRegistro && x.cc == resguardo.cc && x.folio == resguardo.folio && x.id_activo == resguardo.id_activo);

                                if (registroResguardo == null)
                                {
                                    throw new Exception("No se encuentra el registro del resguardo para el insumo " + resguardo.id_activo + ".");
                                }

                                registroResguardo.estatus = resguardo.parcial ? "P" : "D";

                                if (relacionUsuario != null)
                                {
                                    registroResguardo.recibio = relacionUsuario.empleado;
                                }
                                else
                                {
                                    registroResguardo.recibio = null;
                                }

                                registroResguardo.recibioSIGOPLAN = vSesiones.sesionUsuarioDTO.id;
                                registroResguardo.condiciones_ret = resguardo.condiciones;
                                registroResguardo.fec_devolucion = DateTime.Now.Date;
                                registroResguardo.resguardo_parcial = Convert.ToDecimal(registroResguardo.resguardo_parcial != null ? registroResguardo.resguardo_parcial : 0, CultureInfo.InvariantCulture) + resguardo.cantidad_resguardo;

                                _context.SaveChanges();
                            }

                            #region Update el estatus de todos los registros del resguardo
                            var listaResguardo = _context.tblAlm_Resguardo.Where(x => x.estatusRegistro && x.cc == cc && x.folio == folio).ToList();
                            List<string> listaEstatusIndividual = new List<string>();

                            #region Determinar el Estatus general de los registros.
                            foreach (var res in listaResguardo)
                            {
                                var cantidadDevuelto = Convert.ToDecimal(res.resguardo_parcial != null ? res.resguardo_parcial : 0, CultureInfo.InvariantCulture);
                                var cantidadResultado = res.cantidad_resguardo - cantidadDevuelto;

                                if (cantidadDevuelto > 0)
                                {
                                    if (cantidadResultado > 0)
                                    {
                                        listaEstatusIndividual.Add("P");
                                    }
                                    else
                                    {
                                        listaEstatusIndividual.Add("D");
                                    }
                                }
                                else
                                {
                                    listaEstatusIndividual.Add("V");
                                }
                            }

                            var estatusFinal = "";

                            if (listaEstatusIndividual.All(x => x == "V"))
                            {
                                estatusFinal = "V";
                            }
                            else if (listaEstatusIndividual.All(x => x == "P"))
                            {
                                estatusFinal = "P";
                            }
                            else if (listaEstatusIndividual.Any(x => x == "V" || x == "P"))
                            {
                                estatusFinal = "P";
                            }
                            else if (listaEstatusIndividual.All(x => x == "D"))
                            {
                                estatusFinal = "D";
                            }
                            else
                            {
                                throw new Exception("Estatus no controlado para el resguardo.");
                            }
                            #endregion

                            foreach (var resguardo in listaResguardo)
                            {
                                resguardo.estatus = estatusFinal;
                                _context.SaveChanges();
                            }
                            #endregion

                            dbStarsoftTransaction.Commit();
                            dbSigoplanTransaction.Commit();

                            SaveBitacora(12, (int)AccionEnum.ACTUALIZAR, 0, JsonUtils.convertNetObjectToJson(resguardos));

                            result.Add("estatusFinal", estatusFinal);
                            result.Add("data", resguardos);
                            result.Add(SUCCESS, true);
                        }
                        catch (Exception e)
                        {
                            dbStarsoftTransaction.Rollback();
                            dbSigoplanTransaction.Rollback();

                            LogError(12, 12, "ResguardoController", "guardarDevolucionPeru", e, AccionEnum.AGREGAR, 0, resguardos);

                            result.Add(MESSAGE, e.Message);
                            result.Add(SUCCESS, false);
                        }
                    }
                }
            }

            return result;
        }

        public int guardarEntradaTraspasoResguardo(List<ResguardoEKDTO> resguardos, int folioResguardo, OdbcTransaction trans)
        {
            var empleado = 0;
            var usuarioSigoplan = _context.tblP_Usuario_Enkontrol.FirstOrDefault(x => x.idUsuario == vSesiones.sesionUsuarioDTO.id);

            if (usuarioSigoplan != null)
            {
                empleado = usuarioSigoplan.empleado;
            }

            var nuevoNumero = 1;
            var folioTraspaso = 1;

            var ultimoMovimientoEK = consultaCheckProductivo(
                string.Format(@"SELECT 
                                    TOP 1 numero, folio_traspaso 
                                FROM si_movimientos 
                                WHERE almacen = {0} AND tipo_mov = {1} 
                                ORDER BY numero DESC", resguardos[0].alm_salida, 2)
            );

            if (ultimoMovimientoEK != null)
            {
                var ultimoMovimiento = ((List<dynamic>)ultimoMovimientoEK.ToObject<List<dynamic>>())[0];

                nuevoNumero = (int)ultimoMovimiento.numero + 1;
                folioTraspaso = (ultimoMovimiento.folio_traspaso != null) ? (int)ultimoMovimiento.folio_traspaso + 1 : 1;
            }

            tblAlm_Movimientos nuevaEntrada = new tblAlm_Movimientos();

            nuevaEntrada = new tblAlm_Movimientos
            {
                almacen = resguardos[0].alm_salida,
                tipo_mov = 2,
                numero = nuevoNumero,
                cc = resguardos[0].cc.ToUpper(),
                compania = 1,
                periodo = DateTime.Now.Month,
                ano = DateTime.Now.Year,
                orden_ct = 0,
                frente = 0,
                fecha = DateTime.Now.Date,
                proveedor = 0,
                total = resguardos.Select(x => x.cantidad_resguardo).Sum(),
                estatus = "A",
                transferida = "N",
                alm_destino = resguardos[0].alm_entrada,
                cc_destino = resguardos[0].cc.ToUpper(),
                comentarios = resguardos[0].observaciones,
                tipo_trasp = "0",
                tipo_cambio = 1,
                numeroReq = null,
                estatusHabilitado = true
            };

            _context.tblAlm_Movimientos.Add(nuevaEntrada);
            _context.SaveChanges();

            List<tblAlm_MovimientosDet> listEntradaDet = new List<tblAlm_MovimientosDet>();

            var partidaContador = 1;

            foreach (var res in resguardos)
            {
                var partidaMovimiento = partidaContador++;

                var nuevaEntradaDet = new tblAlm_MovimientosDet
                {
                    almacen = res.alm_salida,
                    tipo_mov = 2,
                    numero = nuevoNumero,
                    partida = partidaMovimiento,
                    insumo = res.id_activo,
                    comentarios = res.observaciones,
                    area = 0,
                    cuenta = 1,
                    cantidad = res.cantidad_resguardo,
                    precio = 1,
                    importe = 1,
                    id_resguardo = res.id_tipo_activo,
                    area_alm = res.area_alm ?? "",
                    lado_alm = res.lado_alm ?? "",
                    estante_alm = res.estante_alm ?? "",
                    nivel_alm = res.nivel_alm ?? "",
                    transporte = "",
                    estatusHabilitado = true
                };

                _context.tblAlm_MovimientosDet.Add(nuevaEntradaDet);
                _context.SaveChanges();
                listEntradaDet.Add(nuevaEntradaDet);
            }

            if (nuevaEntrada.total <= 0)
            {
                throw new Exception("El total no puede ser igual o menor a cero.");
            }

            using (var con = checkConexionProductivo())
            {
                var consultaMovimientos = @"INSERT INTO si_movimientos 
                                         (almacen, tipo_mov, numero, cc, compania, periodo, ano, orden_ct, frente, fecha, proveedor, total, estatus, 
                                         transferida, poliza, empleado, alm_destino, cc_destino, comentarios, tipo_trasp, numero_destino, tp, year_poliza, 
                                         mes_poliza, tipo_cambio, hora, fecha_modifica, empleado_modifica, destajista, obra, id_residente, factura, sector_id, 
                                         tc_cc, paquete, tipo_cargo, cargo_destajista, cargo_id_residente, embarque, orden_prod, folio_traspaso, bit_sin_ubicacion) 
                                         VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                var commandMovimientos = new OdbcCommand(consultaMovimientos);

                OdbcParameterCollection parametersMovimientos = commandMovimientos.Parameters;

                parametersMovimientos.Add("@almacen", OdbcType.Numeric).Value = nuevaEntrada.almacen;
                parametersMovimientos.Add("@tipo_mov", OdbcType.Numeric).Value = nuevaEntrada.tipo_mov;
                parametersMovimientos.Add("@numero", OdbcType.Numeric).Value = nuevaEntrada.numero;
                parametersMovimientos.Add("@cc", OdbcType.Char).Value = nuevaEntrada.cc;
                parametersMovimientos.Add("@compania", OdbcType.Numeric).Value = nuevaEntrada.compania;
                parametersMovimientos.Add("@periodo", OdbcType.Numeric).Value = nuevaEntrada.periodo;
                parametersMovimientos.Add("@ano", OdbcType.Numeric).Value = nuevaEntrada.ano;
                parametersMovimientos.Add("@orden_ct", OdbcType.Numeric).Value = nuevaEntrada.orden_ct;
                parametersMovimientos.Add("@frente", OdbcType.Numeric).Value = nuevaEntrada.frente;
                parametersMovimientos.Add("@fecha", OdbcType.Date).Value = nuevaEntrada.fecha.Date;
                parametersMovimientos.Add("@proveedor", OdbcType.Numeric).Value = nuevaEntrada.proveedor;
                parametersMovimientos.Add("@total", OdbcType.Numeric).Value = nuevaEntrada.total;
                parametersMovimientos.Add("@estatus", OdbcType.Char).Value = nuevaEntrada.estatus;
                parametersMovimientos.Add("@transferida", OdbcType.Char).Value = nuevaEntrada.transferida;
                parametersMovimientos.Add("@poliza", OdbcType.Numeric).Value = 0;
                parametersMovimientos.Add("@empleado", OdbcType.Numeric).Value = empleado;
                parametersMovimientos.Add("@alm_destino", OdbcType.Numeric).Value = nuevaEntrada.alm_destino;
                parametersMovimientos.Add("@cc_destino", OdbcType.Char).Value = nuevaEntrada.cc_destino;
                parametersMovimientos.Add("@comentarios", OdbcType.Char).Value = nuevaEntrada.comentarios != null ? nuevaEntrada.comentarios : "";
                parametersMovimientos.Add("@tipo_trasp", OdbcType.Char).Value = nuevaEntrada.tipo_trasp;
                parametersMovimientos.Add("@numero_destino", OdbcType.Numeric).Value = folioResguardo;
                parametersMovimientos.Add("@tp", OdbcType.Char).Value = DBNull.Value;
                parametersMovimientos.Add("@year_poliza", OdbcType.Numeric).Value = DateTime.Now.Year;
                parametersMovimientos.Add("@mes_poliza", OdbcType.Numeric).Value = DateTime.Now.Month;
                parametersMovimientos.Add("@tipo_cambio", OdbcType.Numeric).Value = nuevaEntrada.tipo_cambio;
                parametersMovimientos.Add("@hora", OdbcType.Time).Value = DateTime.Now.TimeOfDay;
                parametersMovimientos.Add("@fecha_modifica", OdbcType.Date).Value = DateTime.Now.Date;
                parametersMovimientos.Add("@empleado_modifica", OdbcType.Numeric).Value = empleado;
                parametersMovimientos.Add("@destajista", OdbcType.Numeric).Value = DBNull.Value;
                parametersMovimientos.Add("@obra", OdbcType.Char).Value = DBNull.Value;
                parametersMovimientos.Add("@id_residente", OdbcType.Numeric).Value = DBNull.Value;
                parametersMovimientos.Add("@factura", OdbcType.Numeric).Value = DBNull.Value;
                parametersMovimientos.Add("@sector_id", OdbcType.Numeric).Value = DBNull.Value;
                parametersMovimientos.Add("@tc_cc", OdbcType.Numeric).Value = 1;
                parametersMovimientos.Add("@paquete", OdbcType.Numeric).Value = DBNull.Value;
                parametersMovimientos.Add("@tipo_cargo", OdbcType.Numeric).Value = 0;
                parametersMovimientos.Add("@cargo_Destajista", OdbcType.Numeric).Value = DBNull.Value;
                parametersMovimientos.Add("@cargo_id_residente", OdbcType.Numeric).Value = DBNull.Value;
                parametersMovimientos.Add("@embarque", OdbcType.Numeric).Value = DBNull.Value;
                parametersMovimientos.Add("@orden_prod", OdbcType.Numeric).Value = DBNull.Value;
                parametersMovimientos.Add("@folio_traspaso", OdbcType.Numeric).Value = DBNull.Value;
                parametersMovimientos.Add("@bit_sin_ubicacion", OdbcType.Numeric).Value = 0;

                commandMovimientos.Connection = trans.Connection;
                commandMovimientos.Transaction = trans;

                var success = commandMovimientos.ExecuteNonQuery();
                var successDet = 0;

                foreach (var entDet in listEntradaDet)
                {
                    if ((entDet.cantidad * 1) <= 0)
                    {
                        throw new Exception("El precio y el importe no pueden ser igual o menor a cero.");
                    }

                    var consultaMovimientosDetalle = @"INSERT INTO si_movimientos_det 
                                         (almacen, tipo_mov, numero, partida, insumo, comentarios, area, cuenta, cantidad, precio, importe, 
                                         partida_oc, costo_prom, obra, manzana_ini, lote_ini, interior_ini, manzana_fin, lote_fin, interior_fin, 
                                         remision, sector_id, id_resguardo, area_alm, lado_alm, estante_alm, nivel_alm, fecha_fisico) 
                                         VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                    var commandMovimientosDetalles = new OdbcCommand(consultaMovimientosDetalle);

                    OdbcParameterCollection parametersMovimientosDetalles = commandMovimientosDetalles.Parameters;

                    parametersMovimientosDetalles.Add("@almacen", OdbcType.Numeric).Value = entDet.almacen;
                    parametersMovimientosDetalles.Add("@tipo_mov", OdbcType.Numeric).Value = entDet.tipo_mov;
                    parametersMovimientosDetalles.Add("@numero", OdbcType.Numeric).Value = entDet.numero;
                    parametersMovimientosDetalles.Add("@partida", OdbcType.Numeric).Value = entDet.partida;
                    parametersMovimientosDetalles.Add("@insumo", OdbcType.Numeric).Value = entDet.insumo;
                    parametersMovimientosDetalles.Add("@comentarios", OdbcType.Char).Value = entDet.comentarios != null ? entDet.comentarios : "";
                    parametersMovimientosDetalles.Add("@area", OdbcType.Numeric).Value = entDet.area;
                    parametersMovimientosDetalles.Add("@cuenta", OdbcType.Numeric).Value = entDet.cuenta;
                    parametersMovimientosDetalles.Add("@cantidad", OdbcType.Numeric).Value = entDet.cantidad;
                    parametersMovimientosDetalles.Add("@precio", OdbcType.Numeric).Value = 1;
                    parametersMovimientosDetalles.Add("@importe", OdbcType.Numeric).Value = entDet.cantidad * 1;
                    parametersMovimientosDetalles.Add("@partida_oc", OdbcType.Numeric).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@costo_prom", OdbcType.Numeric).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@obra", OdbcType.Char).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@manzana_ini", OdbcType.Numeric).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@lote_ini", OdbcType.Numeric).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@interior_ini", OdbcType.Char).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@manzana_fin", OdbcType.Numeric).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@lote_fin", OdbcType.Numeric).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@interior_fin", OdbcType.Char).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@remision", OdbcType.Char).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@sector_id", OdbcType.Numeric).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@id_resguardo", OdbcType.Numeric).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@area_alm", OdbcType.Char).Value = entDet.area_alm;
                    parametersMovimientosDetalles.Add("@lado_alm", OdbcType.Char).Value = entDet.lado_alm;
                    parametersMovimientosDetalles.Add("@estante_alm", OdbcType.Char).Value = entDet.estante_alm;
                    parametersMovimientosDetalles.Add("@nivel_alm", OdbcType.Char).Value = entDet.nivel_alm;
                    parametersMovimientosDetalles.Add("@fecha_fisico", OdbcType.Date).Value = DBNull.Value;

                    commandMovimientosDetalles.Connection = trans.Connection;
                    commandMovimientosDetalles.Transaction = trans;

                    successDet = commandMovimientosDetalles.ExecuteNonQuery();

                    #region Actualizar Tablas Acumula
                    var objAcumula = new MovimientoDetalleEnkontrolDTO
                    {
                        insumo = entDet.insumo,
                        cantidad = entDet.cantidad,
                        precio = 1,
                        tipo_mov = entDet.tipo_mov,
                        costo_prom = 0
                    };

                    actualizarAcumula(nuevaEntrada.almacen, nuevaEntrada.cc, objAcumula, null, trans);
                    #endregion
                }

                if (success > 0 && successDet > 0)
                {
                    _context.SaveChanges();
                }
            }

            return nuevoNumero;
        }

        public int guardarEntradaTraspasoResguardoColombia(List<ResguardoEKDTO> resguardos, int folioResguardo, OdbcTransaction trans)
        {
            var empleado = 0;
            var usuarioSigoplan = _context.tblP_Usuario_Enkontrol.FirstOrDefault(x => x.idUsuario == vSesiones.sesionUsuarioDTO.id);

            if (usuarioSigoplan != null)
            {
                empleado = usuarioSigoplan.empleado;
            }

            var nuevoNumero = 1;
            //var folioTraspaso = 1;

            var alm_salida = resguardos[0].alm_salida;
            var ultimoMovimiento = _context.tblAlm_Movimientos.Where(x => x.estatusHabilitado && x.almacen == alm_salida && x.tipo_mov == 2).OrderByDescending(x => x.numero).FirstOrDefault();

            if (ultimoMovimiento != null)
            {
                nuevoNumero = ultimoMovimiento.numero + 1;
                //folioTraspaso = (ultimoMovimiento.folio_traspaso != null) ? (int)ultimoMovimiento.folio_traspaso + 1 : ;
            }

            tblAlm_Movimientos nuevaEntrada = new tblAlm_Movimientos();

            nuevaEntrada = new tblAlm_Movimientos
            {
                almacen = resguardos[0].alm_salida,
                tipo_mov = 2,
                numero = nuevoNumero,
                cc = resguardos[0].cc.ToUpper(),
                compania = 1,
                periodo = DateTime.Now.Month,
                ano = DateTime.Now.Year,
                orden_ct = 0,
                frente = 0,
                fecha = DateTime.Now.Date,
                proveedor = 0,
                total = resguardos.Select(x => x.cantidad_resguardo).Sum(),
                estatus = "A",
                transferida = "N",
                alm_destino = resguardos[0].alm_entrada,
                cc_destino = resguardos[0].cc.ToUpper(),
                comentarios = resguardos[0].observaciones,
                tipo_trasp = "0",
                tipo_cambio = 1,
                numeroReq = null,
                estatusHabilitado = true
            };

            _context.tblAlm_Movimientos.Add(nuevaEntrada);
            _context.SaveChanges();

            List<tblAlm_MovimientosDet> listEntradaDet = new List<tblAlm_MovimientosDet>();

            var partidaContador = 1;

            foreach (var res in resguardos)
            {
                var partidaMovimiento = partidaContador++;

                var nuevaEntradaDet = new tblAlm_MovimientosDet
                {
                    almacen = res.alm_salida,
                    tipo_mov = 2,
                    numero = nuevoNumero,
                    partida = partidaMovimiento,
                    insumo = res.id_activo,
                    comentarios = res.observaciones,
                    area = 0,
                    cuenta = 1,
                    cantidad = res.cantidad_resguardo,
                    precio = 1,
                    importe = 1,
                    id_resguardo = res.id_tipo_activo,
                    area_alm = res.area_alm ?? "",
                    lado_alm = res.lado_alm ?? "",
                    estante_alm = res.estante_alm ?? "",
                    nivel_alm = res.nivel_alm ?? "",
                    transporte = "",
                    estatusHabilitado = true
                };

                _context.tblAlm_MovimientosDet.Add(nuevaEntradaDet);
                _context.SaveChanges();
                listEntradaDet.Add(nuevaEntradaDet);
            }

            if (nuevaEntrada.total <= 0)
            {
                throw new Exception("El total no puede ser igual o menor a cero.");
            }

            using (var con = checkConexionProductivo())
            {
                var consultaMovimientos = @"INSERT INTO DBA.si_movimientos 
                                         (almacen, tipo_mov, numero, cc, compania, periodo, ano, orden_ct, frente, fecha, proveedor, total, estatus, 
                                         transferida, poliza, empleado, alm_destino, cc_destino, comentarios, tipo_trasp, numero_destino, tp, year_poliza, 
                                         mes_poliza, tipo_cambio, hora, fecha_modifica, empleado_modifica, destajista, obra, id_residente, factura, sector_id, 
                                         tc_cc, paquete, tipo_cargo, cargo_destajista, cargo_id_residente, embarque, orden_prod) 
                                         VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                var commandMovimientos = new OdbcCommand(consultaMovimientos);

                OdbcParameterCollection parametersMovimientos = commandMovimientos.Parameters;

                parametersMovimientos.Add("@almacen", OdbcType.Numeric).Value = nuevaEntrada.almacen;
                parametersMovimientos.Add("@tipo_mov", OdbcType.Numeric).Value = nuevaEntrada.tipo_mov;
                parametersMovimientos.Add("@numero", OdbcType.Numeric).Value = nuevaEntrada.numero;
                parametersMovimientos.Add("@cc", OdbcType.Char).Value = nuevaEntrada.cc;
                parametersMovimientos.Add("@compania", OdbcType.Numeric).Value = nuevaEntrada.compania;
                parametersMovimientos.Add("@periodo", OdbcType.Numeric).Value = nuevaEntrada.periodo;
                parametersMovimientos.Add("@ano", OdbcType.Numeric).Value = nuevaEntrada.ano;
                parametersMovimientos.Add("@orden_ct", OdbcType.Numeric).Value = nuevaEntrada.orden_ct;
                parametersMovimientos.Add("@frente", OdbcType.Numeric).Value = nuevaEntrada.frente;
                parametersMovimientos.Add("@fecha", OdbcType.Date).Value = nuevaEntrada.fecha.Date;
                parametersMovimientos.Add("@proveedor", OdbcType.Numeric).Value = nuevaEntrada.proveedor;
                parametersMovimientos.Add("@total", OdbcType.Numeric).Value = nuevaEntrada.total;
                parametersMovimientos.Add("@estatus", OdbcType.Char).Value = nuevaEntrada.estatus;
                parametersMovimientos.Add("@transferida", OdbcType.Char).Value = nuevaEntrada.transferida;
                parametersMovimientos.Add("@poliza", OdbcType.Numeric).Value = 0;
                parametersMovimientos.Add("@empleado", OdbcType.Numeric).Value = empleado;
                parametersMovimientos.Add("@alm_destino", OdbcType.Numeric).Value = nuevaEntrada.alm_destino;
                parametersMovimientos.Add("@cc_destino", OdbcType.Char).Value = nuevaEntrada.cc_destino;
                parametersMovimientos.Add("@comentarios", OdbcType.Char).Value = nuevaEntrada.comentarios != null ? nuevaEntrada.comentarios : "";
                parametersMovimientos.Add("@tipo_trasp", OdbcType.Char).Value = nuevaEntrada.tipo_trasp;
                parametersMovimientos.Add("@numero_destino", OdbcType.Numeric).Value = folioResguardo;
                parametersMovimientos.Add("@tp", OdbcType.Char).Value = DBNull.Value;
                parametersMovimientos.Add("@year_poliza", OdbcType.Numeric).Value = DateTime.Now.Year;
                parametersMovimientos.Add("@mes_poliza", OdbcType.Numeric).Value = DateTime.Now.Month;
                parametersMovimientos.Add("@tipo_cambio", OdbcType.Numeric).Value = nuevaEntrada.tipo_cambio;
                parametersMovimientos.Add("@hora", OdbcType.Time).Value = DateTime.Now.TimeOfDay;
                parametersMovimientos.Add("@fecha_modifica", OdbcType.Date).Value = DateTime.Now.Date;
                parametersMovimientos.Add("@empleado_modifica", OdbcType.Numeric).Value = empleado;
                parametersMovimientos.Add("@destajista", OdbcType.Numeric).Value = DBNull.Value;
                parametersMovimientos.Add("@obra", OdbcType.Char).Value = DBNull.Value;
                parametersMovimientos.Add("@id_residente", OdbcType.Numeric).Value = DBNull.Value;
                parametersMovimientos.Add("@factura", OdbcType.Numeric).Value = DBNull.Value;
                parametersMovimientos.Add("@sector_id", OdbcType.Numeric).Value = DBNull.Value;
                parametersMovimientos.Add("@tc_cc", OdbcType.Numeric).Value = 1;
                parametersMovimientos.Add("@paquete", OdbcType.Numeric).Value = DBNull.Value;
                parametersMovimientos.Add("@tipo_cargo", OdbcType.Numeric).Value = 0;
                parametersMovimientos.Add("@cargo_Destajista", OdbcType.Numeric).Value = DBNull.Value;
                parametersMovimientos.Add("@cargo_id_residente", OdbcType.Numeric).Value = DBNull.Value;
                parametersMovimientos.Add("@embarque", OdbcType.Numeric).Value = DBNull.Value;
                parametersMovimientos.Add("@orden_prod", OdbcType.Numeric).Value = DBNull.Value;

                commandMovimientos.Connection = trans.Connection;
                commandMovimientos.Transaction = trans;

                var success = commandMovimientos.ExecuteNonQuery();
                var successDet = 0;

                foreach (var entDet in listEntradaDet)
                {
                    if ((entDet.cantidad * 1) <= 0)
                    {
                        throw new Exception("El precio y el importe no pueden ser igual o menor a cero.");
                    }

                    var consultaMovimientosDetalle = @"INSERT INTO DBA.si_movimientos_det 
                                         (almacen, tipo_mov, numero, partida, insumo, comentarios, area, cuenta, cantidad, precio, importe, 
                                         partida_oc, costo_prom, obra, manzana_ini, lote_ini, interior_ini, manzana_fin, lote_fin, interior_fin, 
                                         remision, sector_id) 
                                         VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                    var commandMovimientosDetalles = new OdbcCommand(consultaMovimientosDetalle);

                    OdbcParameterCollection parametersMovimientosDetalles = commandMovimientosDetalles.Parameters;

                    parametersMovimientosDetalles.Add("@almacen", OdbcType.Numeric).Value = entDet.almacen;
                    parametersMovimientosDetalles.Add("@tipo_mov", OdbcType.Numeric).Value = entDet.tipo_mov;
                    parametersMovimientosDetalles.Add("@numero", OdbcType.Numeric).Value = entDet.numero;
                    parametersMovimientosDetalles.Add("@partida", OdbcType.Numeric).Value = entDet.partida;
                    parametersMovimientosDetalles.Add("@insumo", OdbcType.Numeric).Value = entDet.insumo;
                    parametersMovimientosDetalles.Add("@comentarios", OdbcType.Char).Value = entDet.comentarios != null ? entDet.comentarios : "";
                    parametersMovimientosDetalles.Add("@area", OdbcType.Numeric).Value = entDet.area;
                    parametersMovimientosDetalles.Add("@cuenta", OdbcType.Numeric).Value = entDet.cuenta;
                    parametersMovimientosDetalles.Add("@cantidad", OdbcType.Numeric).Value = entDet.cantidad;
                    parametersMovimientosDetalles.Add("@precio", OdbcType.Numeric).Value = 1;
                    parametersMovimientosDetalles.Add("@importe", OdbcType.Numeric).Value = entDet.cantidad * 1;
                    parametersMovimientosDetalles.Add("@partida_oc", OdbcType.Numeric).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@costo_prom", OdbcType.Numeric).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@obra", OdbcType.Char).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@manzana_ini", OdbcType.Numeric).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@lote_ini", OdbcType.Numeric).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@interior_ini", OdbcType.Char).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@manzana_fin", OdbcType.Numeric).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@lote_fin", OdbcType.Numeric).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@interior_fin", OdbcType.Char).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@remision", OdbcType.Char).Value = DBNull.Value;
                    parametersMovimientosDetalles.Add("@sector_id", OdbcType.Numeric).Value = DBNull.Value;

                    commandMovimientosDetalles.Connection = trans.Connection;
                    commandMovimientosDetalles.Transaction = trans;

                    successDet = commandMovimientosDetalles.ExecuteNonQuery();

                    #region Actualizar Tablas Acumula
                    var objAcumula = new MovimientoDetalleEnkontrolDTO
                    {
                        insumo = entDet.insumo,
                        cantidad = entDet.cantidad,
                        precio = 1,
                        tipo_mov = entDet.tipo_mov,
                        costo_prom = 0
                    };

                    actualizarAcumulaColombia(nuevaEntrada.almacen, nuevaEntrada.cc, objAcumula, null, trans);
                    #endregion
                }

                if (success > 0 && successDet > 0)
                {
                    _context.SaveChanges();
                }
            }

            return nuevoNumero;
        }

        public Dictionary<string, object> getEmpleados(int sessionActual)
        {
            var result = new Dictionary<string, object>();

            try
            {

                switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
                {
                    case EmpresaEnum.Peru:
                        {
                            var listaUsuarios = _context.tblAlm_EmpleadoResguardo.Where(x => x.registroActivo).ToList().Select(x => new EmpleadoResguardoDTO
                            {
                                empleado = x.folio,
                                descripcion = x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno,
                                puesto = 0,
                                telefono = "",
                                monto_inicial = 0,
                                monto = 0,
                                vobo_monto_inicial = 0,
                                vobo_monto_final = 0,
                                vobo = ""
                            }).ToList();

                            result.Add("data", listaUsuarios);
                            result.Add(SUCCESS, true);
                            break;
                        }
                    case EmpresaEnum.Colombia:
                        {
                            var empleadosEK = consultaCheckProductivo(
                                string.Format(@"SELECT
                                                emp.descripcion,
                                                emp.puesto,
                                                p.descripcion AS puestoDesc,
                                                emp.telefono,
                                                emp.monto,
                                                emp.empleado
                                            FROM empleados emp 
                                                LEFT JOIN si_puestos p ON emp.puesto = p.puesto")
                            );

                            if (empleadosEK != null)
                            {
                                var empleados = (List<EmpleadoResguardoDTO>)empleadosEK.ToObject<List<EmpleadoResguardoDTO>>();

                                result.Add("data", empleados);
                                result.Add(SUCCESS, true);
                            }
                            break;
                        }
                    default:
                        {
                            var empleadosEK = consultaCheckProductivo(
                                string.Format(@"SELECT
                                                emp.descripcion,
                                                emp.puesto,
                                                p.descripcion AS puestoDesc,
                                                emp.telefono,
                                                emp.monto_inicial,
                                                emp.monto,
                                                emp.vobo_monto_inicial,
                                                emp.vobo_monto_final,
                                                emp.vobo,
                                                emp.empleado
                                                --emp.*, 
                                                --p.descripcion AS puestoDesc 
                                            FROM empleados emp 
                                                LEFT JOIN si_puestos p ON emp.puesto = p.puesto")
                            );

                            if (empleadosEK != null)
                            {
                                var empleados = (List<EmpleadoResguardoDTO>)empleadosEK.ToObject<List<EmpleadoResguardoDTO>>();

                                result.Add("data", empleados);
                                result.Add(SUCCESS, true);
                            }
                            break;
                        }
                }
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return result;
        }


        public Dictionary<string, object> guardarNuevoEmpleado(EmpleadoResguardoDTO empleado)
        {
            var result = new Dictionary<string, object>();

            switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
            {

                case EmpresaEnum.Peru:
                    {
                        #region Perú
                        using (var dbSigoplanTransaction = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                var ultimoFolio = _context.tblAlm_EmpleadoResguardo.Where(x => x.registroActivo).Select(x => x.folio).OrderByDescending(x => x).FirstOrDefault();

                                var nuevoRegistroEmpleadoResguardo = new tblAlm_EmpleadoResguardo();

                                nuevoRegistroEmpleadoResguardo.folio = ultimoFolio + 1;
                                nuevoRegistroEmpleadoResguardo.nombre = empleado.nom_empleado;
                                nuevoRegistroEmpleadoResguardo.apellidoPaterno = empleado.ap_paterno_empleado;
                                nuevoRegistroEmpleadoResguardo.apellidoMaterno = empleado.ap_materno_empleado;
                                nuevoRegistroEmpleadoResguardo.registroActivo = true;

                                _context.tblAlm_EmpleadoResguardo.Add(nuevoRegistroEmpleadoResguardo);
                                _context.SaveChanges();

                                dbSigoplanTransaction.Commit();
                                result.Add("data", new
                                {
                                    empleado = nuevoRegistroEmpleadoResguardo.folio,
                                    descripcion = (empleado.nom_empleado ?? "") + " " + (empleado.ap_paterno_empleado ?? "") + " " + (empleado.ap_materno_empleado ?? "")
                                });
                                result.Add(SUCCESS, true);
                            }
                            catch (Exception e)
                            {
                                dbSigoplanTransaction.Rollback();
                                result.Add(SUCCESS, false);
                            }
                        }

                        break;
                        #endregion
                    }
                case EmpresaEnum.Colombia:
                    {
                        #region Colombia
                        OdbcConnection conexionColombia = new Conexion().ConnectColombiaProductivo();

                        using (var conColombia = conexionColombia)
                        {
                            using (var transColombia = conColombia.BeginTransaction())
                            {
                                try
                                {
                                    List<dynamic> ultimoEmpleadoColombia = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ColombiaProductivo, new OdbcConsultaDTO()
                                    {
                                        consulta = @"SELECT TOP 1 empleado FROM empleados ORDER BY empleado DESC"
                                    });

                                    int numeroEmpleadoConstruplan = Convert.ToInt32(ultimoEmpleadoColombia[0].empleado) + 1;

                                    var empleadoGuardado = new
                                    {
                                        empleado = numeroEmpleadoConstruplan,
                                        descripcion = (empleado.nom_empleado ?? "") + " " + (empleado.ap_paterno_empleado ?? "") + " " + (empleado.ap_materno_empleado ?? "")
                                    };

                                    var count = 0;
                                    var consulta = @"INSERT INTO DBA.empleados (empleado, descripcion, puesto, telefono, almacen, password, monto) VALUES (?,?,?,?,?,?,?)";

                                    using (var cmd = new OdbcCommand(consulta))
                                    {
                                        OdbcParameterCollection parameters = cmd.Parameters;

                                        parameters.Add("@empleado", OdbcType.Numeric).Value = numeroEmpleadoConstruplan;
                                        parameters.Add("@descripcion", OdbcType.Char).Value = empleadoGuardado.descripcion;
                                        parameters.Add("@puesto", OdbcType.Numeric).Value = 1;
                                        parameters.Add("@telefono", OdbcType.Char).Value = "1";
                                        parameters.Add("@almacen", OdbcType.Numeric).Value = DBNull.Value;
                                        parameters.Add("@password", OdbcType.Char).Value = "1";
                                        parameters.Add("@monto", OdbcType.Numeric).Value = 0;

                                        cmd.Connection = transColombia.Connection;
                                        cmd.Transaction = transColombia;

                                        count += cmd.ExecuteNonQuery();
                                    }

                                    transColombia.Commit();

                                    result.Add("data", empleadoGuardado);
                                    result.Add(SUCCESS, true);
                                }
                                catch (Exception e)
                                {
                                    transColombia.Rollback();

                                    LogError(0, 0, "ResguardoController", "guardarNuevoEmpleado", e, AccionEnum.AGREGAR, 0, empleado);

                                    result.Add(MESSAGE, e.Message);
                                    result.Add(SUCCESS, false);
                                }
                            }
                        }

                        break;
                        #endregion
                    }
                default:
                    {
                        #region México
                        OdbcConnection conexionConstruplan;
                        OdbcConnection conexionArrendadora;

                        if (productivo)
                        {
                            conexionConstruplan = new Conexion().ConnectComprasConstruplan();
                            conexionArrendadora = new Conexion().ConnectComprasArrendadora();
                        }
                        else
                        {
                            conexionConstruplan = new Conexion().ConnectComprasConstruplanPrueba();
                            conexionArrendadora = new Conexion().ConnectComprasArrendadoraPrueba();
                        }

                        using (var conConstruplan = conexionConstruplan)
                        {
                            using (var transConstruplan = conConstruplan.BeginTransaction())
                            {
                                using (var conArrendadora = conexionArrendadora)
                                {
                                    using (var transArrendadora = conArrendadora.BeginTransaction())
                                    {
                                        try
                                        {
                                            List<dynamic> ultimoEmpleadoConstruplan = _contextEnkontrol.Select<dynamic>(productivo ? EnkontrolEnum.CplanProd : EnkontrolEnum.PruebaCplanProd, new OdbcConsultaDTO()
                                            {
                                                consulta = @"SELECT TOP 1 empleado FROM empleados ORDER BY empleado DESC"
                                            });
                                            List<dynamic> ultimoEmpleadoArrendadora = _contextEnkontrol.Select<dynamic>(productivo ? EnkontrolEnum.ArrenProd : EnkontrolEnum.PruebaArrenADM, new OdbcConsultaDTO()
                                            {
                                                consulta = @"SELECT TOP 1 empleado FROM empleados ORDER BY empleado DESC"
                                            });

                                            int numeroEmpleadoConstruplan = Convert.ToInt32(ultimoEmpleadoConstruplan[0].empleado) + 1;
                                            int numeroEmpleadoArrendadora = Convert.ToInt32(ultimoEmpleadoArrendadora[0].empleado) + 1;

                                            if (numeroEmpleadoConstruplan > numeroEmpleadoArrendadora)
                                            {
                                                numeroEmpleadoArrendadora = numeroEmpleadoConstruplan;
                                            }
                                            else if (numeroEmpleadoArrendadora > numeroEmpleadoConstruplan)
                                            {
                                                numeroEmpleadoConstruplan = numeroEmpleadoArrendadora;
                                            }

                                            #region Validación para que los números coincidan entre ambas empresas.
                                            if (numeroEmpleadoConstruplan != numeroEmpleadoArrendadora)
                                            {
                                                throw new Exception("Los números no coinciden entre ambas empresas. Construplan: " + numeroEmpleadoConstruplan + ". Arrendadora: " + numeroEmpleadoArrendadora);
                                            }
                                            #endregion

                                            var empleadoGuardado = new
                                            {
                                                empleado = numeroEmpleadoConstruplan,
                                                descripcion = (empleado.nom_empleado ?? "") + " " + (empleado.ap_paterno_empleado ?? "") + " " + (empleado.ap_materno_empleado ?? "")
                                            };

                                            var count = 0;
                                            var consulta = @"
                                    INSERT INTO empleados 
                                        (empleado, descripcion, puesto, telefono, almacen, password, monto, monto_inicial, vobo_monto_inicial, 
                                        vobo_monto_final, vobo, solicito, requisita_tmc, autoriza_req_tmc, vobo_tmc, autoriza_tmc, monto_ini_tmc, 
                                        monto_fin_tmc, autoriza_activos_fijos, nom_empleado, ap_paterno_empleado, ap_materno_empleado, rfc_empleado, 
                                        venta_activos_fijos, autoriza_cancelar_facturas, autoriza_notas_credito, autoriza_factura_proveedores, 
                                        autoriza_baja_inventarios, autoriza_baja_contabilidad) 
                                    VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                                            #region CONSTRUPLAN
                                            using (var cmd = new OdbcCommand(consulta))
                                            {
                                                OdbcParameterCollection parameters = cmd.Parameters;

                                                parameters.Add("@empleado", OdbcType.Numeric).Value = numeroEmpleadoConstruplan;
                                                parameters.Add("@descripcion", OdbcType.Char).Value = empleadoGuardado.descripcion;
                                                parameters.Add("@puesto", OdbcType.Numeric).Value = 99;
                                                parameters.Add("@telefono", OdbcType.Char).Value = "";
                                                parameters.Add("@almacen", OdbcType.Numeric).Value = DBNull.Value;
                                                parameters.Add("@password", OdbcType.Char).Value = DBNull.Value;
                                                parameters.Add("@monto", OdbcType.Numeric).Value = DBNull.Value;
                                                parameters.Add("@monto_inicial", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@vobo_monto_inicial", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@vobo_monto_final", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@vobo", OdbcType.Char).Value = "N";
                                                parameters.Add("@solicito", OdbcType.Char).Value = "N";
                                                parameters.Add("@requisita_tmc", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@autoriza_req_tmc", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@vobo_tmc", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@autoriza_tmc", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@monto_ini_tmc", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@monto_fin_tmc", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@autoriza_activos_fijos", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@nom_empleado", OdbcType.VarChar).Value = empleado.nom_empleado ?? "";
                                                parameters.Add("@ap_paterno_empleado", OdbcType.VarChar).Value = empleado.ap_paterno_empleado ?? "";
                                                parameters.Add("@ap_materno_empleado", OdbcType.VarChar).Value = empleado.ap_materno_empleado ?? "";
                                                parameters.Add("@rfc_empleado", OdbcType.VarChar).Value = empleado.rfc_empleado ?? "";
                                                parameters.Add("@venta_activos_fijos", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@autoriza_cancelar_facturas", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@autoriza_notas_credito", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@autoriza_factura_proveedores", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@autoriza_baja_inventarios", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@autoriza_baja_contabilidad", OdbcType.Numeric).Value = 0;

                                                cmd.Connection = transConstruplan.Connection;
                                                cmd.Transaction = transConstruplan;

                                                count += cmd.ExecuteNonQuery();
                                            }
                                            #endregion

                                            #region ARRENDADORA
                                            using (var cmd = new OdbcCommand(consulta))
                                            {
                                                OdbcParameterCollection parameters = cmd.Parameters;

                                                parameters.Add("@empleado", OdbcType.Numeric).Value = numeroEmpleadoArrendadora;
                                                parameters.Add("@descripcion", OdbcType.Char).Value = empleadoGuardado.descripcion;
                                                parameters.Add("@puesto", OdbcType.Numeric).Value = 99;
                                                parameters.Add("@telefono", OdbcType.Char).Value = "";
                                                parameters.Add("@almacen", OdbcType.Numeric).Value = DBNull.Value;
                                                parameters.Add("@password", OdbcType.Char).Value = DBNull.Value;
                                                parameters.Add("@monto", OdbcType.Numeric).Value = DBNull.Value;
                                                parameters.Add("@monto_inicial", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@vobo_monto_inicial", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@vobo_monto_final", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@vobo", OdbcType.Char).Value = "N";
                                                parameters.Add("@solicito", OdbcType.Char).Value = "N";
                                                parameters.Add("@requisita_tmc", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@autoriza_req_tmc", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@vobo_tmc", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@autoriza_tmc", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@monto_ini_tmc", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@monto_fin_tmc", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@autoriza_activos_fijos", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@nom_empleado", OdbcType.VarChar).Value = empleado.nom_empleado ?? "";
                                                parameters.Add("@ap_paterno_empleado", OdbcType.VarChar).Value = empleado.ap_paterno_empleado ?? "";
                                                parameters.Add("@ap_materno_empleado", OdbcType.VarChar).Value = empleado.ap_materno_empleado ?? "";
                                                parameters.Add("@rfc_empleado", OdbcType.VarChar).Value = empleado.rfc_empleado ?? "";
                                                parameters.Add("@venta_activos_fijos", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@autoriza_cancelar_facturas", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@autoriza_notas_credito", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@autoriza_factura_proveedores", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@autoriza_baja_inventarios", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@autoriza_baja_contabilidad", OdbcType.Numeric).Value = 0;

                                                cmd.Connection = transArrendadora.Connection;
                                                cmd.Transaction = transArrendadora;

                                                count += cmd.ExecuteNonQuery();
                                            }
                                            #endregion

                                            #region Validar que la información se haya guardado correctamente en ambas empresas.
                                            //List<dynamic> ultimoEmpleadoConstruplanRevision = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanProd, new OdbcConsultaDTO()
                                            //{
                                            //    consulta = @"SELECT TOP 1 empleado FROM empleados ORDER BY empleado DESC"
                                            //});
                                            //List<dynamic> ultimoEmpleadoArrendadoraRevision = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenProd, new OdbcConsultaDTO()
                                            //{
                                            //    consulta = @"SELECT TOP 1 empleado FROM empleados ORDER BY empleado DESC"
                                            //});

                                            //if (Convert.ToInt32(ultimoEmpleadoConstruplanRevision[0].empleado) != Convert.ToInt32(ultimoEmpleadoArrendadoraRevision[0].empleado))
                                            //{
                                            //    throw new Exception("Error al guardar la información en ambas empresas.");
                                            //}
                                            #endregion

                                            transConstruplan.Commit();
                                            transArrendadora.Commit();

                                            result.Add("data", empleadoGuardado);
                                            result.Add(SUCCESS, true);
                                        }
                                        catch (Exception e)
                                        {
                                            transConstruplan.Rollback();
                                            transArrendadora.Rollback();

                                            LogError(0, 0, "ResguardoController", "guardarNuevoEmpleado", e, AccionEnum.AGREGAR, 0, empleado);

                                            result.Add(MESSAGE, e.Message);
                                            result.Add(SUCCESS, false);
                                        }
                                    }
                                }
                            }
                        }

                        break;
                        #endregion
                    }
            }

            return result;
        }

        public Dictionary<string, object> getUltimoFolio(string cc, int alm_salida)
        {
            var result = new Dictionary<string, object>();

            try
            {
                if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Peru)
                {
                    //                var ultimoFolioEK = consultaCheckProductivo(
                    //                    string.Format(@"SELECT TOP 1 
                    //                                        folio 
                    //                                    FROM si_resguardo_activo_fijo 
                    //                                    WHERE alm_salida = {0} 
                    //                                    ORDER BY folio DESC", alm_salida)
                    //                );
                    var ultimoFolioEK = consultaCheckProductivo(string.Format(@"SELECT TOP 1 numero FROM si_movimientos WHERE almacen = {0} AND tipo_mov = 52 ORDER BY numero DESC", alm_salida));

                    if (ultimoFolioEK != null)
                    {
                        //var ultimoFolio = (int)(((List<dynamic>)ultimoFolioEK.ToObject<List<dynamic>>())[0].folio);
                        var ultimoFolio = (int)(((List<dynamic>)ultimoFolioEK.ToObject<List<dynamic>>())[0].numero);

                        result.Add("folio", ultimoFolio + 1);
                        result.Add(SUCCESS, true);
                    }
                    else
                    {
                        result.Add("folio", 1);
                        result.Add(SUCCESS, true);
                    }
                }
                else
                {
                    //Se toma como folio del resguardo el siguiente folio de movimiento de salida para igualar la funcionalidad entre México y Perú.
                    var ultimoFolio = _context.tblAlm_Movimientos.Where(x => x.estatusHabilitado && x.almacen == alm_salida && x.tipo_mov == 52).ToList().OrderByDescending(x => x.numero).FirstOrDefault();

                    if (ultimoFolio != null)
                    {
                        result.Add("folio", ultimoFolio.numero + 1);
                        result.Add(SUCCESS, true);
                    }
                    else
                    {
                        result.Add("folio", 1);
                        result.Add(SUCCESS, true);
                    }
                }
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return result;
        }

        public bool checarUbicacionesValidas(List<ResguardoEKDTO> entradas)
        {
            foreach (var ent in entradas)
            {
                var checkUbicacionValida = consultaCheckProductivo(
                    string.Format(@"SELECT 
                                            * 
                                        FROM si_matriz_ubicaciones 
                                        WHERE 
                                            almacen = {0} AND 
                                            area_alm = '{1}' AND 
                                            lado_alm = '{2}' AND 
                                            estante_alm = '{3}' AND 
                                            nivel_alm = '{4}'", ent.alm_salida, ent.area_alm, ent.lado_alm, ent.estante_alm, ent.nivel_alm)
                );

                if (checkUbicacionValida == null)
                {
                    throw new Exception("Debe capturar ubicaciones válidas para el almacén.");
                }
            }

            return true;
        }

        public EmpleadoPuestoDTO getEmpleadoNomina(int claveEmpleado)
        {
            List<EmpleadoPuestoDTO> empleadoEK = new List<EmpleadoPuestoDTO>();
            //string strQuery = @"SELECT clave_empleado, nombre, ape_paterno, ape_materno, puesto FROM sn_empleados WHERE clave_empleado = {0}";
            //var odbc = new OdbcConsultaDTO() { consulta = strQuery };
            //odbc.consulta = String.Format(strQuery, claveEmpleado);

            //if ((int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
            //    empleadoEK = _contextEnkontrol.Select<EmpleadoPuestoDTO>(EnkontrolEnum.CplanRh, odbc);
            //else
            //    empleadoEK = _contextEnkontrol.Select<EmpleadoPuestoDTO>(EnkontrolEnum.ArrenRh, odbc);
            empleadoEK = _context.Select<EmpleadoPuestoDTO>(new DapperDTO
            {
                baseDatos = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                consulta = "SELECT clave_empleado, nombre, ape_paterno, ape_materno, puesto FROM tblRH_EK_Empleados WHERE clave_empleado = @paramCveEmpl",
                parametros = new { paramCveEmpl = claveEmpleado }
            }).ToList();

            //var empleadoEK = ContextEnKontrolNomina.Where(string.Format(@"SELECT * FROM sn_empleados WHERE clave_empleado = {0}", claveEmpleado));

            if (empleadoEK.Count() > 0)
            {
                //var empleado = ((List<EmpleadoPuestoDTO>)empleadoEK.ToObject<List<EmpleadoPuestoDTO>>())[0];
                return empleadoEK.First();
                //return empleado;
            }
            else
                return null;
        }

        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboAlmacenVirtual()
        {
            try
            {
                switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
                {
                    case EmpresaEnum.Peru:
                        {
                            using (var _starsoft = new MainContextPeruStarSoft003BDCOMUN())
                            {
                                return _starsoft.TABALM.ToList().Where(x => x.TAALMA[0] == '9').Select(x => new Core.DTO.Principal.Generales.ComboDTO
                                {
                                    Value = x.TAALMA,
                                    Text = x.TAALMA + " - " + x.TADESCRI
                                }).ToList();
                            }
                        }
                    case EmpresaEnum.Colombia:
                        {
                            return (List<Core.DTO.Principal.Generales.ComboDTO>)consultaCheckProductivo(string.Format(@"
                                SELECT 
                                    alm.almacen AS Value, 
                                    (CONVERT(varchar(12), alm.almacen) + ' - ' + alm.descripcion) AS Text 
                                FROM si_almacen alm 
                                WHERE alm.almacen >= 900 AND alm.bit_mp = 'S' 
                                ORDER BY Value
                            ")).ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();
                        }
                    default:
                        {
                            return (List<Core.DTO.Principal.Generales.ComboDTO>)consultaCheckProductivo(string.Format(@"
                                SELECT 
                                    alm.almacen AS Value, 
                                    (CONVERT(varchar(12), alm.almacen) + ' - ' + alm.descripcion) AS Text 
                                FROM si_almacen alm 
                                WHERE alm.almacen >= 900 AND alm.almacen_virtual = 1 AND alm.bit_mp = 'S' 
                                ORDER BY Value
                            ")).ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();
                        }
                }
            }
            catch (Exception)
            {
                return new List<Core.DTO.Principal.Generales.ComboDTO>();
            }
        }

        public bool actualizarAcumula(int almacen, string cc, MovimientoDetalleEnkontrolDTO det, DbContextTransaction dbSigoplanTransaction, OdbcTransaction trans)
        {
            var anio = DateTime.Now.Year;
            var insumo = det.insumo;
            var cantidad = det.cantidad;
            var precio = det.precio;
            var importe = cantidad * precio;
            var tipoMovimiento = det.tipo_mov;

            var mes = DateTime.Now.Month;
            var columnaExistencia = "existencia_" + (tipoMovimiento < 50 ? "ent_" : "sal_");
            var columnaImporte = "importe_" + (tipoMovimiento < 50 ? "ent_" : "sal_");

            #region Localizar Columnas Afectadas
            switch (mes)
            {
                case 1:
                    columnaExistencia = string.Concat(columnaExistencia, "ene");
                    columnaImporte = string.Concat(columnaImporte, "ene");
                    break;
                case 2:
                    columnaExistencia = string.Concat(columnaExistencia, "feb");
                    columnaImporte = string.Concat(columnaImporte, "feb");
                    break;
                case 3:
                    columnaExistencia = string.Concat(columnaExistencia, "mar");
                    columnaImporte = string.Concat(columnaImporte, "mar");
                    break;
                case 4:
                    columnaExistencia = string.Concat(columnaExistencia, "abr");
                    columnaImporte = string.Concat(columnaImporte, "abr");
                    break;
                case 5:
                    columnaExistencia = string.Concat(columnaExistencia, "may");
                    columnaImporte = string.Concat(columnaImporte, "may");
                    break;
                case 6:
                    columnaExistencia = string.Concat(columnaExistencia, "jun");
                    columnaImporte = string.Concat(columnaImporte, "jun");
                    break;
                case 7:
                    columnaExistencia = string.Concat(columnaExistencia, "jul");
                    columnaImporte = string.Concat(columnaImporte, "jul");
                    break;
                case 8:
                    columnaExistencia = string.Concat(columnaExistencia, "ago");
                    columnaImporte = string.Concat(columnaImporte, "ago");
                    break;
                case 9:
                    columnaExistencia = string.Concat(columnaExistencia, "sep");
                    columnaImporte = string.Concat(columnaImporte, "sep");
                    break;
                case 10:
                    columnaExistencia = string.Concat(columnaExistencia, "oct");
                    columnaImporte = string.Concat(columnaImporte, "oct");
                    break;
                case 11:
                    columnaExistencia = string.Concat(columnaExistencia, "nov");
                    columnaImporte = string.Concat(columnaImporte, "nov");
                    break;
                case 12:
                    columnaExistencia = string.Concat(columnaExistencia, "dic");
                    columnaImporte = string.Concat(columnaImporte, "dic");
                    break;
            }
            #endregion

            var count = 0;

            #region Update Registro Acumula Almacén
            var registroAcumulaAlmacenEK = consultaCheckProductivo(
                string.Format(@"SELECT * FROM si_acumula_almacen WHERE almacen = {0} AND cc = '{1}' AND ano = {2} AND insumo = {3}", almacen, cc, anio, insumo)
            );

            if (registroAcumulaAlmacenEK != null)
            {
                var registroAcumulaAlmacen = ((List<AcumulaEnkontrolDTO>)registroAcumulaAlmacenEK.ToObject<List<AcumulaEnkontrolDTO>>())[0];

                var existenciaAnteriorAlmacen = Convert.ToDecimal(registroAcumulaAlmacen.GetType().GetProperty(columnaExistencia).GetValue(registroAcumulaAlmacen), CultureInfo.InvariantCulture);
                var importeAnteriorAlmacen = Convert.ToDecimal(registroAcumulaAlmacen.GetType().GetProperty(columnaImporte).GetValue(registroAcumulaAlmacen), CultureInfo.InvariantCulture);

                var consultaUpdateAlmacen =
                    string.Format(@"UPDATE si_acumula_almacen 
                                SET {0} = ?, {1} = ? 
                                WHERE almacen = ? AND cc = ? AND ano = ? AND insumo = ?", columnaExistencia, columnaImporte);

                using (var cmd = new OdbcCommand(consultaUpdateAlmacen))
                {
                    OdbcParameterCollection parameters = cmd.Parameters;

                    parameters.Add(string.Concat("@", columnaExistencia), OdbcType.Numeric).Value = existenciaAnteriorAlmacen + cantidad;
                    parameters.Add(string.Concat("@", columnaImporte), OdbcType.Numeric).Value = importeAnteriorAlmacen + importe;

                    parameters.Add("@almacen", OdbcType.Numeric).Value = almacen;
                    parameters.Add("@cc", OdbcType.Char).Value = cc;
                    parameters.Add("@ano", OdbcType.Numeric).Value = anio;
                    parameters.Add("@insumo", OdbcType.Numeric).Value = insumo;

                    cmd.Connection = trans.Connection;
                    cmd.Transaction = trans;

                    count += cmd.ExecuteNonQuery();
                }
            }
            else
            {
                #region Insert Registro en la tabla "si_acumula_almacen"
                var consultaInsertAcumulaAlmacen = @"INSERT INTO si_acumula_almacen 
                                                        (almacen, cc, ano, 
                                                        existencia_ent_ini, importe_ent_ini, existencia_sal_ini, importe_sal_ini, 
                                                        existencia_ent_ene, importe_ent_ene, existencia_sal_ene, importe_sal_ene, 
                                                        existencia_ent_feb, importe_ent_feb, existencia_sal_feb, importe_sal_feb, 
                                                        existencia_ent_mar, importe_ent_mar, existencia_sal_mar, importe_sal_mar, 
                                                        existencia_ent_abr, importe_ent_abr, existencia_sal_abr, importe_sal_abr, 
                                                        existencia_ent_may, importe_ent_may, existencia_sal_may, importe_sal_may, 
                                                        existencia_ent_jun, importe_ent_jun, existencia_sal_jun, importe_sal_jun, 
                                                        existencia_ent_jul, importe_ent_jul, existencia_sal_jul, importe_sal_jul, 
                                                        existencia_ent_ago, importe_ent_ago, existencia_sal_ago, importe_sal_ago, 
                                                        existencia_ent_sep, importe_ent_sep, existencia_sal_sep, importe_sal_sep, 
                                                        existencia_ent_oct, importe_ent_oct, existencia_sal_oct, importe_sal_oct, 
                                                        existencia_ent_nov, importe_ent_nov, existencia_sal_nov, importe_sal_nov, 
                                                        existencia_ent_dic, importe_ent_dic, existencia_sal_dic, importe_sal_dic, 
                                                        insumo) 
                                                        VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                using (var cmd = new OdbcCommand(consultaInsertAcumulaAlmacen))
                {
                    OdbcParameterCollection parameters = cmd.Parameters;

                    parameters.Add("@almacen", OdbcType.Numeric).Value = almacen;
                    parameters.Add("@cc", OdbcType.Char).Value = cc;
                    parameters.Add("@ano", OdbcType.Numeric).Value = DateTime.Now.Year;

                    parameters.Add("@existencia_ent_ini", OdbcType.Numeric).Value = 0;
                    parameters.Add("@importe_ent_ini", OdbcType.Numeric).Value = 0;
                    parameters.Add("@existencia_sal_ini", OdbcType.Numeric).Value = 0;
                    parameters.Add("@importe_sal_ini", OdbcType.Numeric).Value = 0;

                    parameters.Add("@existencia_ent_ene", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_ene" ? cantidad : 0;
                    parameters.Add("@importe_ent_ene", OdbcType.Numeric).Value = columnaImporte == "importe_ent_ene" ? importe : 0;
                    parameters.Add("@existencia_sal_ene", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_ene" ? cantidad : 0;
                    parameters.Add("@importe_sal_ene", OdbcType.Numeric).Value = columnaImporte == "importe_sal_ene" ? importe : 0;

                    parameters.Add("@existencia_ent_feb", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_feb" ? cantidad : 0;
                    parameters.Add("@importe_ent_feb", OdbcType.Numeric).Value = columnaImporte == "importe_ent_feb" ? importe : 0;
                    parameters.Add("@existencia_sal_feb", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_feb" ? cantidad : 0;
                    parameters.Add("@importe_sal_feb", OdbcType.Numeric).Value = columnaImporte == "importe_sal_feb" ? importe : 0;

                    parameters.Add("@existencia_ent_mar", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_mar" ? cantidad : 0;
                    parameters.Add("@importe_ent_mar", OdbcType.Numeric).Value = columnaImporte == "importe_ent_mar" ? importe : 0;
                    parameters.Add("@existencia_sal_mar", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_mar" ? cantidad : 0;
                    parameters.Add("@importe_sal_mar", OdbcType.Numeric).Value = columnaImporte == "importe_sal_mar" ? importe : 0;

                    parameters.Add("@existencia_ent_abr", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_abr" ? cantidad : 0;
                    parameters.Add("@importe_ent_abr", OdbcType.Numeric).Value = columnaImporte == "importe_ent_abr" ? importe : 0;
                    parameters.Add("@existencia_sal_abr", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_abr" ? cantidad : 0;
                    parameters.Add("@importe_sal_abr", OdbcType.Numeric).Value = columnaImporte == "importe_sal_abr" ? importe : 0;

                    parameters.Add("@existencia_ent_may", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_may" ? cantidad : 0;
                    parameters.Add("@importe_ent_may", OdbcType.Numeric).Value = columnaImporte == "importe_ent_may" ? importe : 0;
                    parameters.Add("@existencia_sal_may", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_may" ? cantidad : 0;
                    parameters.Add("@importe_sal_may", OdbcType.Numeric).Value = columnaImporte == "importe_sal_may" ? importe : 0;

                    parameters.Add("@existencia_ent_jun", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_jun" ? cantidad : 0;
                    parameters.Add("@importe_ent_jun", OdbcType.Numeric).Value = columnaImporte == "importe_ent_jun" ? importe : 0;
                    parameters.Add("@existencia_sal_jun", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_jun" ? cantidad : 0;
                    parameters.Add("@importe_sal_jun", OdbcType.Numeric).Value = columnaImporte == "importe_sal_jun" ? importe : 0;

                    parameters.Add("@existencia_ent_jul", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_jul" ? cantidad : 0;
                    parameters.Add("@importe_ent_jul", OdbcType.Numeric).Value = columnaImporte == "importe_ent_jul" ? importe : 0;
                    parameters.Add("@existencia_sal_jul", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_jul" ? cantidad : 0;
                    parameters.Add("@importe_sal_jul", OdbcType.Numeric).Value = columnaImporte == "importe_sal_jul" ? importe : 0;

                    parameters.Add("@existencia_ent_ago", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_ago" ? cantidad : 0;
                    parameters.Add("@importe_ent_ago", OdbcType.Numeric).Value = columnaImporte == "importe_ent_ago" ? importe : 0;
                    parameters.Add("@existencia_sal_ago", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_ago" ? cantidad : 0;
                    parameters.Add("@importe_sal_ago", OdbcType.Numeric).Value = columnaImporte == "importe_sal_ago" ? importe : 0;

                    parameters.Add("@existencia_ent_sep", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_sep" ? cantidad : 0;
                    parameters.Add("@importe_ent_sep", OdbcType.Numeric).Value = columnaImporte == "importe_ent_sep" ? importe : 0;
                    parameters.Add("@existencia_sal_sep", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_sep" ? cantidad : 0;
                    parameters.Add("@importe_sal_sep", OdbcType.Numeric).Value = columnaImporte == "importe_sal_sep" ? importe : 0;

                    parameters.Add("@existencia_ent_oct", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_oct" ? cantidad : 0;
                    parameters.Add("@importe_ent_oct", OdbcType.Numeric).Value = columnaImporte == "importe_ent_oct" ? importe : 0;
                    parameters.Add("@existencia_sal_oct", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_oct" ? cantidad : 0;
                    parameters.Add("@importe_sal_oct", OdbcType.Numeric).Value = columnaImporte == "importe_sal_oct" ? importe : 0;

                    parameters.Add("@existencia_ent_nov", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_nov" ? cantidad : 0;
                    parameters.Add("@importe_ent_nov", OdbcType.Numeric).Value = columnaImporte == "importe_ent_nov" ? importe : 0;
                    parameters.Add("@existencia_sal_nov", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_nov" ? cantidad : 0;
                    parameters.Add("@importe_sal_nov", OdbcType.Numeric).Value = columnaImporte == "importe_sal_nov" ? importe : 0;

                    parameters.Add("@existencia_ent_dic", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_dic" ? cantidad : 0;
                    parameters.Add("@importe_ent_dic", OdbcType.Numeric).Value = columnaImporte == "importe_ent_dic" ? importe : 0;
                    parameters.Add("@existencia_sal_dic", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_dic" ? cantidad : 0;
                    parameters.Add("@importe_sal_dic", OdbcType.Numeric).Value = columnaImporte == "importe_sal_dic" ? importe : 0;

                    parameters.Add("@insumo", OdbcType.Numeric).Value = insumo;

                    cmd.Connection = trans.Connection;
                    cmd.Transaction = trans;

                    count += cmd.ExecuteNonQuery();
                }
                #endregion
            }
            #endregion

            #region Update Registro Acumula CC
            var registroAcumulaCCEK = consultaCheckProductivo(
                string.Format(@"SELECT * FROM si_acumula_cc WHERE cc = '{0}' AND ano = {1} AND insumo = {2}", cc, anio, insumo)
            );

            if (registroAcumulaCCEK != null)
            {
                var registroAcumulaCC = ((List<AcumulaEnkontrolDTO>)registroAcumulaCCEK.ToObject<List<AcumulaEnkontrolDTO>>())[0];

                var existenciaAnteriorCC = Convert.ToDecimal(registroAcumulaCC.GetType().GetProperty(columnaExistencia).GetValue(registroAcumulaCC), CultureInfo.InvariantCulture);
                var importeAnteriorCC = Convert.ToDecimal(registroAcumulaCC.GetType().GetProperty(columnaImporte).GetValue(registroAcumulaCC), CultureInfo.InvariantCulture);

                var consultaUpdateCC =
                    string.Format(@"UPDATE si_acumula_cc 
                                SET {0} = ?, {1} = ?, ultimo_cp = ?, fecha_cp = ? 
                                WHERE cc = ? AND ano = ? AND insumo = ?", columnaExistencia, columnaImporte);

                using (var cmd = new OdbcCommand(consultaUpdateCC))
                {
                    OdbcParameterCollection parameters = cmd.Parameters;

                    parameters.Add(string.Concat("@", columnaExistencia), OdbcType.Numeric).Value = existenciaAnteriorCC + cantidad;
                    parameters.Add(string.Concat("@", columnaImporte), OdbcType.Numeric).Value = importeAnteriorCC + importe;

                    if (det.costo_prom > 0)
                    {
                        parameters.Add("@ultimo_cp", OdbcType.Numeric).Value = det.costo_prom ?? 0;
                        parameters.Add("@fecha_cp", OdbcType.Date).Value = DateTime.Now.Date;
                    }
                    else
                    {
                        parameters.Add("@ultimo_cp", OdbcType.Numeric).Value = registroAcumulaCC.ultimo_cp ?? (object)DBNull.Value;
                        parameters.Add("@fecha_cp", OdbcType.Date).Value = registroAcumulaCC.fecha_cp ?? (object)DBNull.Value;
                    }

                    parameters.Add("@cc", OdbcType.Char).Value = cc;
                    parameters.Add("@ano", OdbcType.Numeric).Value = anio;
                    parameters.Add("@insumo", OdbcType.Numeric).Value = insumo;

                    cmd.Connection = trans.Connection;
                    cmd.Transaction = trans;

                    count += cmd.ExecuteNonQuery();
                }
            }
            else
            {
                #region Insert Registro en la tabla "si_acumula_cc"
                var consultaInsertAcumulaCC = @"INSERT INTO si_acumula_cc 
                                                    (cc, ano, 
                                                    existencia_ent_ini, importe_ent_ini, existencia_sal_ini, importe_sal_ini, 
                                                    existencia_ent_ene, importe_ent_ene, existencia_sal_ene, importe_sal_ene, 
                                                    existencia_ent_feb, importe_ent_feb, existencia_sal_feb, importe_sal_feb, 
                                                    existencia_ent_mar, importe_ent_mar, existencia_sal_mar, importe_sal_mar, 
                                                    existencia_ent_abr, importe_ent_abr, existencia_sal_abr, importe_sal_abr, 
                                                    existencia_ent_may, importe_ent_may, existencia_sal_may, importe_sal_may, 
                                                    existencia_ent_jun, importe_ent_jun, existencia_sal_jun, importe_sal_jun, 
                                                    existencia_ent_jul, importe_ent_jul, existencia_sal_jul, importe_sal_jul, 
                                                    existencia_ent_ago, importe_ent_ago, existencia_sal_ago, importe_sal_ago, 
                                                    existencia_ent_sep, importe_ent_sep, existencia_sal_sep, importe_sal_sep, 
                                                    existencia_ent_oct, importe_ent_oct, existencia_sal_oct, importe_sal_oct, 
                                                    existencia_ent_nov, importe_ent_nov, existencia_sal_nov, importe_sal_nov, 
                                                    existencia_ent_dic, importe_ent_dic, existencia_sal_dic, importe_sal_dic, 
                                                    insumo, ultimo_cp, fecha_cp) 
                                                VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                using (var cmd = new OdbcCommand(consultaInsertAcumulaCC))
                {
                    OdbcParameterCollection parameters = cmd.Parameters;

                    parameters.Add("@cc", OdbcType.Char).Value = cc;
                    parameters.Add("@ano", OdbcType.Numeric).Value = DateTime.Now.Year;

                    parameters.Add("@existencia_ent_ini", OdbcType.Numeric).Value = 0;
                    parameters.Add("@importe_ent_ini", OdbcType.Numeric).Value = 0;
                    parameters.Add("@existencia_sal_ini", OdbcType.Numeric).Value = 0;
                    parameters.Add("@importe_sal_ini", OdbcType.Numeric).Value = 0;

                    parameters.Add("@existencia_ent_ene", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_ene" ? cantidad : 0;
                    parameters.Add("@importe_ent_ene", OdbcType.Numeric).Value = columnaImporte == "importe_ent_ene" ? importe : 0;
                    parameters.Add("@existencia_sal_ene", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_ene" ? cantidad : 0;
                    parameters.Add("@importe_sal_ene", OdbcType.Numeric).Value = columnaImporte == "importe_sal_ene" ? importe : 0;

                    parameters.Add("@existencia_ent_feb", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_feb" ? cantidad : 0;
                    parameters.Add("@importe_ent_feb", OdbcType.Numeric).Value = columnaImporte == "importe_ent_feb" ? importe : 0;
                    parameters.Add("@existencia_sal_feb", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_feb" ? cantidad : 0;
                    parameters.Add("@importe_sal_feb", OdbcType.Numeric).Value = columnaImporte == "importe_sal_feb" ? importe : 0;

                    parameters.Add("@existencia_ent_mar", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_mar" ? cantidad : 0;
                    parameters.Add("@importe_ent_mar", OdbcType.Numeric).Value = columnaImporte == "importe_ent_mar" ? importe : 0;
                    parameters.Add("@existencia_sal_mar", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_mar" ? cantidad : 0;
                    parameters.Add("@importe_sal_mar", OdbcType.Numeric).Value = columnaImporte == "importe_sal_mar" ? importe : 0;

                    parameters.Add("@existencia_ent_abr", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_abr" ? cantidad : 0;
                    parameters.Add("@importe_ent_abr", OdbcType.Numeric).Value = columnaImporte == "importe_ent_abr" ? importe : 0;
                    parameters.Add("@existencia_sal_abr", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_abr" ? cantidad : 0;
                    parameters.Add("@importe_sal_abr", OdbcType.Numeric).Value = columnaImporte == "importe_sal_abr" ? importe : 0;

                    parameters.Add("@existencia_ent_may", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_may" ? cantidad : 0;
                    parameters.Add("@importe_ent_may", OdbcType.Numeric).Value = columnaImporte == "importe_ent_may" ? importe : 0;
                    parameters.Add("@existencia_sal_may", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_may" ? cantidad : 0;
                    parameters.Add("@importe_sal_may", OdbcType.Numeric).Value = columnaImporte == "importe_sal_may" ? importe : 0;

                    parameters.Add("@existencia_ent_jun", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_jun" ? cantidad : 0;
                    parameters.Add("@importe_ent_jun", OdbcType.Numeric).Value = columnaImporte == "importe_ent_jun" ? importe : 0;
                    parameters.Add("@existencia_sal_jun", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_jun" ? cantidad : 0;
                    parameters.Add("@importe_sal_jun", OdbcType.Numeric).Value = columnaImporte == "importe_sal_jun" ? importe : 0;

                    parameters.Add("@existencia_ent_jul", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_jul" ? cantidad : 0;
                    parameters.Add("@importe_ent_jul", OdbcType.Numeric).Value = columnaImporte == "importe_ent_jul" ? importe : 0;
                    parameters.Add("@existencia_sal_jul", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_jul" ? cantidad : 0;
                    parameters.Add("@importe_sal_jul", OdbcType.Numeric).Value = columnaImporte == "importe_sal_jul" ? importe : 0;

                    parameters.Add("@existencia_ent_ago", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_ago" ? cantidad : 0;
                    parameters.Add("@importe_ent_ago", OdbcType.Numeric).Value = columnaImporte == "importe_ent_ago" ? importe : 0;
                    parameters.Add("@existencia_sal_ago", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_ago" ? cantidad : 0;
                    parameters.Add("@importe_sal_ago", OdbcType.Numeric).Value = columnaImporte == "importe_sal_ago" ? importe : 0;

                    parameters.Add("@existencia_ent_sep", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_sep" ? cantidad : 0;
                    parameters.Add("@importe_ent_sep", OdbcType.Numeric).Value = columnaImporte == "importe_ent_sep" ? importe : 0;
                    parameters.Add("@existencia_sal_sep", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_sep" ? cantidad : 0;
                    parameters.Add("@importe_sal_sep", OdbcType.Numeric).Value = columnaImporte == "importe_sal_sep" ? importe : 0;

                    parameters.Add("@existencia_ent_oct", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_oct" ? cantidad : 0;
                    parameters.Add("@importe_ent_oct", OdbcType.Numeric).Value = columnaImporte == "importe_ent_oct" ? importe : 0;
                    parameters.Add("@existencia_sal_oct", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_oct" ? cantidad : 0;
                    parameters.Add("@importe_sal_oct", OdbcType.Numeric).Value = columnaImporte == "importe_sal_oct" ? importe : 0;

                    parameters.Add("@existencia_ent_nov", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_nov" ? cantidad : 0;
                    parameters.Add("@importe_ent_nov", OdbcType.Numeric).Value = columnaImporte == "importe_ent_nov" ? importe : 0;
                    parameters.Add("@existencia_sal_nov", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_nov" ? cantidad : 0;
                    parameters.Add("@importe_sal_nov", OdbcType.Numeric).Value = columnaImporte == "importe_sal_nov" ? importe : 0;

                    parameters.Add("@existencia_ent_dic", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_dic" ? cantidad : 0;
                    parameters.Add("@importe_ent_dic", OdbcType.Numeric).Value = columnaImporte == "importe_ent_dic" ? importe : 0;
                    parameters.Add("@existencia_sal_dic", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_dic" ? cantidad : 0;
                    parameters.Add("@importe_sal_dic", OdbcType.Numeric).Value = columnaImporte == "importe_sal_dic" ? importe : 0;

                    parameters.Add("@insumo", OdbcType.Numeric).Value = insumo;
                    parameters.Add("@ultimo_cp", OdbcType.Numeric).Value = importe;
                    parameters.Add("@fecha_cp", OdbcType.Date).Value = DateTime.Now.Date;

                    cmd.Connection = trans.Connection;
                    cmd.Transaction = trans;

                    count += cmd.ExecuteNonQuery();
                }
                #endregion
            }
            #endregion

            return true;
        }

        public bool actualizarAcumulaColombia(int almacen, string cc, MovimientoDetalleEnkontrolDTO det, DbContextTransaction dbSigoplanTransaction, OdbcTransaction trans)
        {
            var anio = DateTime.Now.Year;
            var insumo = det.insumo;
            var cantidad = det.cantidad;
            var precio = det.precio;
            var importe = cantidad * precio;
            var tipoMovimiento = det.tipo_mov;

            var mes = DateTime.Now.Month;
            var columnaExistencia = "existencia_" + (tipoMovimiento < 50 ? "ent_" : "sal_");
            var columnaImporte = "importe_" + (tipoMovimiento < 50 ? "ent_" : "sal_");

            #region Localizar Columnas Afectadas
            switch (mes)
            {
                case 1:
                    columnaExistencia = string.Concat(columnaExistencia, "ene");
                    columnaImporte = string.Concat(columnaImporte, "ene");
                    break;
                case 2:
                    columnaExistencia = string.Concat(columnaExistencia, "feb");
                    columnaImporte = string.Concat(columnaImporte, "feb");
                    break;
                case 3:
                    columnaExistencia = string.Concat(columnaExistencia, "mar");
                    columnaImporte = string.Concat(columnaImporte, "mar");
                    break;
                case 4:
                    columnaExistencia = string.Concat(columnaExistencia, "abr");
                    columnaImporte = string.Concat(columnaImporte, "abr");
                    break;
                case 5:
                    columnaExistencia = string.Concat(columnaExistencia, "may");
                    columnaImporte = string.Concat(columnaImporte, "may");
                    break;
                case 6:
                    columnaExistencia = string.Concat(columnaExistencia, "jun");
                    columnaImporte = string.Concat(columnaImporte, "jun");
                    break;
                case 7:
                    columnaExistencia = string.Concat(columnaExistencia, "jul");
                    columnaImporte = string.Concat(columnaImporte, "jul");
                    break;
                case 8:
                    columnaExistencia = string.Concat(columnaExistencia, "ago");
                    columnaImporte = string.Concat(columnaImporte, "ago");
                    break;
                case 9:
                    columnaExistencia = string.Concat(columnaExistencia, "sep");
                    columnaImporte = string.Concat(columnaImporte, "sep");
                    break;
                case 10:
                    columnaExistencia = string.Concat(columnaExistencia, "oct");
                    columnaImporte = string.Concat(columnaImporte, "oct");
                    break;
                case 11:
                    columnaExistencia = string.Concat(columnaExistencia, "nov");
                    columnaImporte = string.Concat(columnaImporte, "nov");
                    break;
                case 12:
                    columnaExistencia = string.Concat(columnaExistencia, "dic");
                    columnaImporte = string.Concat(columnaImporte, "dic");
                    break;
            }
            #endregion

            var count = 0;

            #region Update Registro Acumula Almacén
            var registroAcumulaAlmacenEK = consultaCheckProductivo(
                string.Format(@"SELECT * FROM si_acumula_almacen WHERE almacen = {0} AND cc = '{1}' AND ano = {2} AND insumo = {3}", almacen, cc, anio, insumo)
            );

            if (registroAcumulaAlmacenEK != null)
            {
                var registroAcumulaAlmacen = ((List<AcumulaEnkontrolDTO>)registroAcumulaAlmacenEK.ToObject<List<AcumulaEnkontrolDTO>>())[0];

                var existenciaAnteriorAlmacen = Convert.ToDecimal(registroAcumulaAlmacen.GetType().GetProperty(columnaExistencia).GetValue(registroAcumulaAlmacen), CultureInfo.InvariantCulture);
                var importeAnteriorAlmacen = Convert.ToDecimal(registroAcumulaAlmacen.GetType().GetProperty(columnaImporte).GetValue(registroAcumulaAlmacen), CultureInfo.InvariantCulture);

                var consultaUpdateAlmacen =
                    string.Format(@"UPDATE DBA.si_acumula_almacen 
                                SET {0} = ?, {1} = ? 
                                WHERE almacen = ? AND cc = ? AND ano = ? AND insumo = ?", columnaExistencia, columnaImporte);

                using (var cmd = new OdbcCommand(consultaUpdateAlmacen))
                {
                    OdbcParameterCollection parameters = cmd.Parameters;

                    parameters.Add(string.Concat("@", columnaExistencia), OdbcType.Numeric).Value = existenciaAnteriorAlmacen + cantidad;
                    parameters.Add(string.Concat("@", columnaImporte), OdbcType.Numeric).Value = importeAnteriorAlmacen + importe;

                    parameters.Add("@almacen", OdbcType.Numeric).Value = almacen;
                    parameters.Add("@cc", OdbcType.Char).Value = cc;
                    parameters.Add("@ano", OdbcType.Numeric).Value = anio;
                    parameters.Add("@insumo", OdbcType.Numeric).Value = insumo;

                    cmd.Connection = trans.Connection;
                    cmd.Transaction = trans;

                    count += cmd.ExecuteNonQuery();
                }
            }
            else
            {
                #region Insert Registro en la tabla "si_acumula_almacen"
                var consultaInsertAcumulaAlmacen = @"INSERT INTO DBA.si_acumula_almacen 
                                                        (almacen, cc, ano, 
                                                        existencia_ent_ini, importe_ent_ini, existencia_sal_ini, importe_sal_ini, 
                                                        existencia_ent_ene, importe_ent_ene, existencia_sal_ene, importe_sal_ene, 
                                                        existencia_ent_feb, importe_ent_feb, existencia_sal_feb, importe_sal_feb, 
                                                        existencia_ent_mar, importe_ent_mar, existencia_sal_mar, importe_sal_mar, 
                                                        existencia_ent_abr, importe_ent_abr, existencia_sal_abr, importe_sal_abr, 
                                                        existencia_ent_may, importe_ent_may, existencia_sal_may, importe_sal_may, 
                                                        existencia_ent_jun, importe_ent_jun, existencia_sal_jun, importe_sal_jun, 
                                                        existencia_ent_jul, importe_ent_jul, existencia_sal_jul, importe_sal_jul, 
                                                        existencia_ent_ago, importe_ent_ago, existencia_sal_ago, importe_sal_ago, 
                                                        existencia_ent_sep, importe_ent_sep, existencia_sal_sep, importe_sal_sep, 
                                                        existencia_ent_oct, importe_ent_oct, existencia_sal_oct, importe_sal_oct, 
                                                        existencia_ent_nov, importe_ent_nov, existencia_sal_nov, importe_sal_nov, 
                                                        existencia_ent_dic, importe_ent_dic, existencia_sal_dic, importe_sal_dic, 
                                                        insumo) 
                                                        VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                using (var cmd = new OdbcCommand(consultaInsertAcumulaAlmacen))
                {
                    OdbcParameterCollection parameters = cmd.Parameters;

                    parameters.Add("@almacen", OdbcType.Numeric).Value = almacen;
                    parameters.Add("@cc", OdbcType.Char).Value = cc;
                    parameters.Add("@ano", OdbcType.Numeric).Value = DateTime.Now.Year;

                    parameters.Add("@existencia_ent_ini", OdbcType.Numeric).Value = 0;
                    parameters.Add("@importe_ent_ini", OdbcType.Numeric).Value = 0;
                    parameters.Add("@existencia_sal_ini", OdbcType.Numeric).Value = 0;
                    parameters.Add("@importe_sal_ini", OdbcType.Numeric).Value = 0;

                    parameters.Add("@existencia_ent_ene", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_ene" ? cantidad : 0;
                    parameters.Add("@importe_ent_ene", OdbcType.Numeric).Value = columnaImporte == "importe_ent_ene" ? importe : 0;
                    parameters.Add("@existencia_sal_ene", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_ene" ? cantidad : 0;
                    parameters.Add("@importe_sal_ene", OdbcType.Numeric).Value = columnaImporte == "importe_sal_ene" ? importe : 0;

                    parameters.Add("@existencia_ent_feb", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_feb" ? cantidad : 0;
                    parameters.Add("@importe_ent_feb", OdbcType.Numeric).Value = columnaImporte == "importe_ent_feb" ? importe : 0;
                    parameters.Add("@existencia_sal_feb", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_feb" ? cantidad : 0;
                    parameters.Add("@importe_sal_feb", OdbcType.Numeric).Value = columnaImporte == "importe_sal_feb" ? importe : 0;

                    parameters.Add("@existencia_ent_mar", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_mar" ? cantidad : 0;
                    parameters.Add("@importe_ent_mar", OdbcType.Numeric).Value = columnaImporte == "importe_ent_mar" ? importe : 0;
                    parameters.Add("@existencia_sal_mar", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_mar" ? cantidad : 0;
                    parameters.Add("@importe_sal_mar", OdbcType.Numeric).Value = columnaImporte == "importe_sal_mar" ? importe : 0;

                    parameters.Add("@existencia_ent_abr", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_abr" ? cantidad : 0;
                    parameters.Add("@importe_ent_abr", OdbcType.Numeric).Value = columnaImporte == "importe_ent_abr" ? importe : 0;
                    parameters.Add("@existencia_sal_abr", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_abr" ? cantidad : 0;
                    parameters.Add("@importe_sal_abr", OdbcType.Numeric).Value = columnaImporte == "importe_sal_abr" ? importe : 0;

                    parameters.Add("@existencia_ent_may", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_may" ? cantidad : 0;
                    parameters.Add("@importe_ent_may", OdbcType.Numeric).Value = columnaImporte == "importe_ent_may" ? importe : 0;
                    parameters.Add("@existencia_sal_may", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_may" ? cantidad : 0;
                    parameters.Add("@importe_sal_may", OdbcType.Numeric).Value = columnaImporte == "importe_sal_may" ? importe : 0;

                    parameters.Add("@existencia_ent_jun", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_jun" ? cantidad : 0;
                    parameters.Add("@importe_ent_jun", OdbcType.Numeric).Value = columnaImporte == "importe_ent_jun" ? importe : 0;
                    parameters.Add("@existencia_sal_jun", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_jun" ? cantidad : 0;
                    parameters.Add("@importe_sal_jun", OdbcType.Numeric).Value = columnaImporte == "importe_sal_jun" ? importe : 0;

                    parameters.Add("@existencia_ent_jul", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_jul" ? cantidad : 0;
                    parameters.Add("@importe_ent_jul", OdbcType.Numeric).Value = columnaImporte == "importe_ent_jul" ? importe : 0;
                    parameters.Add("@existencia_sal_jul", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_jul" ? cantidad : 0;
                    parameters.Add("@importe_sal_jul", OdbcType.Numeric).Value = columnaImporte == "importe_sal_jul" ? importe : 0;

                    parameters.Add("@existencia_ent_ago", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_ago" ? cantidad : 0;
                    parameters.Add("@importe_ent_ago", OdbcType.Numeric).Value = columnaImporte == "importe_ent_ago" ? importe : 0;
                    parameters.Add("@existencia_sal_ago", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_ago" ? cantidad : 0;
                    parameters.Add("@importe_sal_ago", OdbcType.Numeric).Value = columnaImporte == "importe_sal_ago" ? importe : 0;

                    parameters.Add("@existencia_ent_sep", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_sep" ? cantidad : 0;
                    parameters.Add("@importe_ent_sep", OdbcType.Numeric).Value = columnaImporte == "importe_ent_sep" ? importe : 0;
                    parameters.Add("@existencia_sal_sep", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_sep" ? cantidad : 0;
                    parameters.Add("@importe_sal_sep", OdbcType.Numeric).Value = columnaImporte == "importe_sal_sep" ? importe : 0;

                    parameters.Add("@existencia_ent_oct", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_oct" ? cantidad : 0;
                    parameters.Add("@importe_ent_oct", OdbcType.Numeric).Value = columnaImporte == "importe_ent_oct" ? importe : 0;
                    parameters.Add("@existencia_sal_oct", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_oct" ? cantidad : 0;
                    parameters.Add("@importe_sal_oct", OdbcType.Numeric).Value = columnaImporte == "importe_sal_oct" ? importe : 0;

                    parameters.Add("@existencia_ent_nov", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_nov" ? cantidad : 0;
                    parameters.Add("@importe_ent_nov", OdbcType.Numeric).Value = columnaImporte == "importe_ent_nov" ? importe : 0;
                    parameters.Add("@existencia_sal_nov", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_nov" ? cantidad : 0;
                    parameters.Add("@importe_sal_nov", OdbcType.Numeric).Value = columnaImporte == "importe_sal_nov" ? importe : 0;

                    parameters.Add("@existencia_ent_dic", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_dic" ? cantidad : 0;
                    parameters.Add("@importe_ent_dic", OdbcType.Numeric).Value = columnaImporte == "importe_ent_dic" ? importe : 0;
                    parameters.Add("@existencia_sal_dic", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_dic" ? cantidad : 0;
                    parameters.Add("@importe_sal_dic", OdbcType.Numeric).Value = columnaImporte == "importe_sal_dic" ? importe : 0;

                    parameters.Add("@insumo", OdbcType.Numeric).Value = insumo;

                    cmd.Connection = trans.Connection;
                    cmd.Transaction = trans;

                    count += cmd.ExecuteNonQuery();
                }
                #endregion
            }
            #endregion

            #region Update Registro Acumula CC
            var registroAcumulaCCEK = consultaCheckProductivo(
                string.Format(@"SELECT * FROM si_acumula_cc WHERE cc = '{0}' AND ano = {1} AND insumo = {2}", cc, anio, insumo)
            );

            if (registroAcumulaCCEK != null)
            {
                var registroAcumulaCC = ((List<AcumulaEnkontrolDTO>)registroAcumulaCCEK.ToObject<List<AcumulaEnkontrolDTO>>())[0];

                var existenciaAnteriorCC = Convert.ToDecimal(registroAcumulaCC.GetType().GetProperty(columnaExistencia).GetValue(registroAcumulaCC), CultureInfo.InvariantCulture);
                var importeAnteriorCC = Convert.ToDecimal(registroAcumulaCC.GetType().GetProperty(columnaImporte).GetValue(registroAcumulaCC), CultureInfo.InvariantCulture);

                var consultaUpdateCC =
                    string.Format(@"UPDATE DBA.si_acumula_cc 
                                SET {0} = ?, {1} = ?, ultimo_cp = ?, fecha_cp = ? 
                                WHERE cc = ? AND ano = ? AND insumo = ?", columnaExistencia, columnaImporte);

                using (var cmd = new OdbcCommand(consultaUpdateCC))
                {
                    OdbcParameterCollection parameters = cmd.Parameters;

                    parameters.Add(string.Concat("@", columnaExistencia), OdbcType.Numeric).Value = existenciaAnteriorCC + cantidad;
                    parameters.Add(string.Concat("@", columnaImporte), OdbcType.Numeric).Value = importeAnteriorCC + importe;

                    if (det.costo_prom > 0)
                    {
                        parameters.Add("@ultimo_cp", OdbcType.Numeric).Value = det.costo_prom ?? 0;
                        parameters.Add("@fecha_cp", OdbcType.Date).Value = DateTime.Now.Date;
                    }
                    else
                    {
                        parameters.Add("@ultimo_cp", OdbcType.Numeric).Value = registroAcumulaCC.ultimo_cp ?? (object)DBNull.Value;
                        parameters.Add("@fecha_cp", OdbcType.Date).Value = registroAcumulaCC.fecha_cp ?? (object)DBNull.Value;
                    }

                    parameters.Add("@cc", OdbcType.Char).Value = cc;
                    parameters.Add("@ano", OdbcType.Numeric).Value = anio;
                    parameters.Add("@insumo", OdbcType.Numeric).Value = insumo;

                    cmd.Connection = trans.Connection;
                    cmd.Transaction = trans;

                    count += cmd.ExecuteNonQuery();
                }
            }
            else
            {
                #region Insert Registro en la tabla "si_acumula_cc"
                var consultaInsertAcumulaCC = @"INSERT INTO DBA.si_acumula_cc 
                                                    (cc, ano, 
                                                    existencia_ent_ini, importe_ent_ini, existencia_sal_ini, importe_sal_ini, 
                                                    existencia_ent_ene, importe_ent_ene, existencia_sal_ene, importe_sal_ene, 
                                                    existencia_ent_feb, importe_ent_feb, existencia_sal_feb, importe_sal_feb, 
                                                    existencia_ent_mar, importe_ent_mar, existencia_sal_mar, importe_sal_mar, 
                                                    existencia_ent_abr, importe_ent_abr, existencia_sal_abr, importe_sal_abr, 
                                                    existencia_ent_may, importe_ent_may, existencia_sal_may, importe_sal_may, 
                                                    existencia_ent_jun, importe_ent_jun, existencia_sal_jun, importe_sal_jun, 
                                                    existencia_ent_jul, importe_ent_jul, existencia_sal_jul, importe_sal_jul, 
                                                    existencia_ent_ago, importe_ent_ago, existencia_sal_ago, importe_sal_ago, 
                                                    existencia_ent_sep, importe_ent_sep, existencia_sal_sep, importe_sal_sep, 
                                                    existencia_ent_oct, importe_ent_oct, existencia_sal_oct, importe_sal_oct, 
                                                    existencia_ent_nov, importe_ent_nov, existencia_sal_nov, importe_sal_nov, 
                                                    existencia_ent_dic, importe_ent_dic, existencia_sal_dic, importe_sal_dic, 
                                                    insumo, ultimo_cp, fecha_cp) 
                                                VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                using (var cmd = new OdbcCommand(consultaInsertAcumulaCC))
                {
                    OdbcParameterCollection parameters = cmd.Parameters;

                    parameters.Add("@cc", OdbcType.Char).Value = cc;
                    parameters.Add("@ano", OdbcType.Numeric).Value = DateTime.Now.Year;

                    parameters.Add("@existencia_ent_ini", OdbcType.Numeric).Value = 0;
                    parameters.Add("@importe_ent_ini", OdbcType.Numeric).Value = 0;
                    parameters.Add("@existencia_sal_ini", OdbcType.Numeric).Value = 0;
                    parameters.Add("@importe_sal_ini", OdbcType.Numeric).Value = 0;

                    parameters.Add("@existencia_ent_ene", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_ene" ? cantidad : 0;
                    parameters.Add("@importe_ent_ene", OdbcType.Numeric).Value = columnaImporte == "importe_ent_ene" ? importe : 0;
                    parameters.Add("@existencia_sal_ene", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_ene" ? cantidad : 0;
                    parameters.Add("@importe_sal_ene", OdbcType.Numeric).Value = columnaImporte == "importe_sal_ene" ? importe : 0;

                    parameters.Add("@existencia_ent_feb", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_feb" ? cantidad : 0;
                    parameters.Add("@importe_ent_feb", OdbcType.Numeric).Value = columnaImporte == "importe_ent_feb" ? importe : 0;
                    parameters.Add("@existencia_sal_feb", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_feb" ? cantidad : 0;
                    parameters.Add("@importe_sal_feb", OdbcType.Numeric).Value = columnaImporte == "importe_sal_feb" ? importe : 0;

                    parameters.Add("@existencia_ent_mar", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_mar" ? cantidad : 0;
                    parameters.Add("@importe_ent_mar", OdbcType.Numeric).Value = columnaImporte == "importe_ent_mar" ? importe : 0;
                    parameters.Add("@existencia_sal_mar", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_mar" ? cantidad : 0;
                    parameters.Add("@importe_sal_mar", OdbcType.Numeric).Value = columnaImporte == "importe_sal_mar" ? importe : 0;

                    parameters.Add("@existencia_ent_abr", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_abr" ? cantidad : 0;
                    parameters.Add("@importe_ent_abr", OdbcType.Numeric).Value = columnaImporte == "importe_ent_abr" ? importe : 0;
                    parameters.Add("@existencia_sal_abr", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_abr" ? cantidad : 0;
                    parameters.Add("@importe_sal_abr", OdbcType.Numeric).Value = columnaImporte == "importe_sal_abr" ? importe : 0;

                    parameters.Add("@existencia_ent_may", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_may" ? cantidad : 0;
                    parameters.Add("@importe_ent_may", OdbcType.Numeric).Value = columnaImporte == "importe_ent_may" ? importe : 0;
                    parameters.Add("@existencia_sal_may", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_may" ? cantidad : 0;
                    parameters.Add("@importe_sal_may", OdbcType.Numeric).Value = columnaImporte == "importe_sal_may" ? importe : 0;

                    parameters.Add("@existencia_ent_jun", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_jun" ? cantidad : 0;
                    parameters.Add("@importe_ent_jun", OdbcType.Numeric).Value = columnaImporte == "importe_ent_jun" ? importe : 0;
                    parameters.Add("@existencia_sal_jun", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_jun" ? cantidad : 0;
                    parameters.Add("@importe_sal_jun", OdbcType.Numeric).Value = columnaImporte == "importe_sal_jun" ? importe : 0;

                    parameters.Add("@existencia_ent_jul", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_jul" ? cantidad : 0;
                    parameters.Add("@importe_ent_jul", OdbcType.Numeric).Value = columnaImporte == "importe_ent_jul" ? importe : 0;
                    parameters.Add("@existencia_sal_jul", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_jul" ? cantidad : 0;
                    parameters.Add("@importe_sal_jul", OdbcType.Numeric).Value = columnaImporte == "importe_sal_jul" ? importe : 0;

                    parameters.Add("@existencia_ent_ago", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_ago" ? cantidad : 0;
                    parameters.Add("@importe_ent_ago", OdbcType.Numeric).Value = columnaImporte == "importe_ent_ago" ? importe : 0;
                    parameters.Add("@existencia_sal_ago", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_ago" ? cantidad : 0;
                    parameters.Add("@importe_sal_ago", OdbcType.Numeric).Value = columnaImporte == "importe_sal_ago" ? importe : 0;

                    parameters.Add("@existencia_ent_sep", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_sep" ? cantidad : 0;
                    parameters.Add("@importe_ent_sep", OdbcType.Numeric).Value = columnaImporte == "importe_ent_sep" ? importe : 0;
                    parameters.Add("@existencia_sal_sep", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_sep" ? cantidad : 0;
                    parameters.Add("@importe_sal_sep", OdbcType.Numeric).Value = columnaImporte == "importe_sal_sep" ? importe : 0;

                    parameters.Add("@existencia_ent_oct", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_oct" ? cantidad : 0;
                    parameters.Add("@importe_ent_oct", OdbcType.Numeric).Value = columnaImporte == "importe_ent_oct" ? importe : 0;
                    parameters.Add("@existencia_sal_oct", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_oct" ? cantidad : 0;
                    parameters.Add("@importe_sal_oct", OdbcType.Numeric).Value = columnaImporte == "importe_sal_oct" ? importe : 0;

                    parameters.Add("@existencia_ent_nov", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_nov" ? cantidad : 0;
                    parameters.Add("@importe_ent_nov", OdbcType.Numeric).Value = columnaImporte == "importe_ent_nov" ? importe : 0;
                    parameters.Add("@existencia_sal_nov", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_nov" ? cantidad : 0;
                    parameters.Add("@importe_sal_nov", OdbcType.Numeric).Value = columnaImporte == "importe_sal_nov" ? importe : 0;

                    parameters.Add("@existencia_ent_dic", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_dic" ? cantidad : 0;
                    parameters.Add("@importe_ent_dic", OdbcType.Numeric).Value = columnaImporte == "importe_ent_dic" ? importe : 0;
                    parameters.Add("@existencia_sal_dic", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_dic" ? cantidad : 0;
                    parameters.Add("@importe_sal_dic", OdbcType.Numeric).Value = columnaImporte == "importe_sal_dic" ? importe : 0;

                    parameters.Add("@insumo", OdbcType.Numeric).Value = insumo;
                    parameters.Add("@ultimo_cp", OdbcType.Numeric).Value = importe;
                    parameters.Add("@fecha_cp", OdbcType.Date).Value = DateTime.Now.Date;

                    cmd.Connection = trans.Connection;
                    cmd.Transaction = trans;

                    count += cmd.ExecuteNonQuery();
                }
                #endregion
            }
            #endregion

            return true;
        }

        public List<tblRH_CatEmpleados> getEmpleadosAutoComplete(string term)
        {
            try
            {
                term = term.Replace(" ", "");

                List<tblRH_CatEmpleados> resultado = new List<tblRH_CatEmpleados>();
                
                List<tblRH_CatEmpleados> empleadosActivosConstruplan = _context.Select<tblRH_CatEmpleados>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = string.Format(@"
                        SELECT TOP 10 
                            clave_empleado, 
                            (LTRIM(RTRIM(nombre)) + ' ' + replace(ape_paterno, ' ', '') + ' ' + replace(ape_materno, ' ', '')) AS Nombre,
                            puesto 
                        FROM tblRH_EK_Empleados 
                        WHERE replace((nombre + ' ' + ape_paterno + ' ' + ape_materno), ' ', '') LIKE '%{0}%' AND estatus_empleado = 'A'", term)
                }).ToList();

                List<tblRH_CatEmpleados> empleadosActivosArrendadora = _context.Select<tblRH_CatEmpleados>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Arrendadora,
                    consulta = string.Format(@"
                        SELECT TOP 10 
                            clave_empleado, 
                            (LTRIM(RTRIM(nombre)) + ' ' + replace(ape_paterno, ' ', '') + ' ' + replace(ape_materno, ' ', '')) AS Nombre, 
                            puesto 
                        FROM tblRH_EK_Empleados 
                        WHERE replace((nombre + ' ' + ape_paterno + ' ' + ape_materno), ' ', '') LIKE '%{0}%' AND estatus_empleado = 'A'", term)
                }).ToList();

                List<tblRH_CatEmpleados> empleadosActivosGCPLAN = _context.Select<tblRH_CatEmpleados>(new DapperDTO
                {
                    baseDatos = MainContextEnum.GCPLAN,
                    consulta = string.Format(@"
                        SELECT TOP 10 
                            clave_empleado, 
                            (LTRIM(RTRIM(nombre)) + ' ' + replace(ape_paterno, ' ', '') + ' ' + replace(ape_materno, ' ', '')) AS Nombre,
                            puesto 
                        FROM tblRH_EK_Empleados 
                        WHERE replace((nombre + ' ' + ape_paterno + ' ' + ape_materno), ' ', '') LIKE '%{0}%' AND estatus_empleado = 'A'", term)
                }).ToList();

                resultado = empleadosActivosConstruplan;

                foreach (var empleadoArrendadora in empleadosActivosArrendadora)
                {
                    if (!empleadosActivosConstruplan.Select(x => x.clave_empleado).Contains(empleadoArrendadora.clave_empleado))
                    {
                        resultado.Add(empleadoArrendadora);
                    }
                }

                foreach (var empleadoGCPLAN in empleadosActivosGCPLAN)
                {
                    if (!empleadosActivosConstruplan.Select(x => x.clave_empleado).Contains(empleadoGCPLAN.clave_empleado))
                    {
                        resultado.Add(empleadoGCPLAN);
                    }
                }

                resultado = resultado.OrderBy(x => x.Nombre).Take(10).ToList();

                return resultado;
            }
            catch (Exception e)
            {
                return new List<tblRH_CatEmpleados>();
            }
        }

        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboCcTodosExistentes()
        {
            try
            {
                switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
                {

                    case EmpresaEnum.Peru:
                        {
                            var listaCentroCosto = _context.tblP_CC.Where(x => x.estatus && x.cc.Length > 3).Select(x => new Core.DTO.Principal.Generales.ComboDTO
                            {
                                Value = x.cc,
                                Text = "[" + x.cc + "] " + x.descripcion.Trim()
                            }).ToList();

                            return listaCentroCosto;
                        }
                    default:
                        {
                            var centrosCostoEK = consultaCheckProductivo(string.Format(@"
                                SELECT 
                                    c.cc AS Value, 
                                    (c.cc + '-' + c.descripcion) AS Text, 
                                    c.st_ppto AS Prefijo 
                                FROM cc c 
                                ORDER BY Value
                            "));

                            if (centrosCostoEK != null)
                            {
                                return (List<Core.DTO.Principal.Generales.ComboDTO>)centrosCostoEK.ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();
                            }
                            else
                            {
                                return new List<Core.DTO.Principal.Generales.ComboDTO>();
                            }
                        }
                }
            }
            catch (Exception)
            {
                return new List<Core.DTO.Principal.Generales.ComboDTO>();
            }
        }

        public Tuple<MemoryStream, string> descargarExcelUsuariosEnkontrolNoCoinciden()
        {
            try
            {
                List<dynamic> listaUsuariosEnkontrolConstruplan = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanProd, new OdbcConsultaDTO()
                {
                    consulta = @"SELECT empleado, descripcion FROM empleados"
                });
                List<dynamic> listaUsuariosEnkontrolArrendadora = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenProd, new OdbcConsultaDTO()
                {
                    consulta = @"SELECT empleado, descripcion FROM empleados"
                });

                var listaCoincidencias = (from cons in listaUsuariosEnkontrolConstruplan
                                          join arre in listaUsuariosEnkontrolArrendadora on (string)cons.descripcion equals (string)arre.descripcion
                                          where Convert.ToInt32(cons.empleado) != Convert.ToInt32(arre.empleado)
                                          select new
                                          {
                                              numeroConstruplan = Convert.ToInt32(cons.empleado),
                                              numeroArrendadora = Convert.ToInt32(arre.empleado),
                                              descripcion = (string)cons.descripcion
                                          }).OrderBy(x => x.descripcion).ToList();

                // Se agregan las columnas.
                var columnasCursos = new List<string> { "Nombre Empleado", "Número Construplan", "Número Arrendadora" };

                var headersExcel = columnasCursos.ToArray();

                using (ExcelPackage excel = new ExcelPackage())
                {
                    var hoja = excel.Workbook.Worksheets.Add("Usuarios Enkontrol");

                    List<string[]> headerRow = new List<string[]>() { headersExcel };

                    string headerRange = "A1:" + ExcelUtilities.GetExcelColumnName(headersExcel.Count()) + "1";

                    hoja.Cells[headerRange].LoadFromArrays(headerRow);

                    var cellData = new List<object[]>();

                    foreach (var usuarioEnkontrol in listaCoincidencias)
                    {
                        var arregloValores = new object[3];

                        arregloValores[0] = usuarioEnkontrol.descripcion;
                        arregloValores[1] = usuarioEnkontrol.numeroConstruplan;
                        arregloValores[2] = usuarioEnkontrol.numeroArrendadora;

                        cellData.Add(arregloValores);
                    }

                    hoja.Cells[2, 1].LoadFromArrays(cellData);

                    excel.Compression = OfficeOpenXml.CompressionLevel.BestSpeed;

                    List<byte[]> lista = new List<byte[]>();

                    var bytes = new MemoryStream();

                    using (var exportData = new MemoryStream())
                    {
                        excel.SaveAs(exportData);
                        bytes = exportData;
                    }

                    return Tuple.Create(bytes, "Usuarios que no coinciden.xlsx");
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Dictionary<string, object> cargarSesionReporteBitacoraResguardos(string centroCostoInicio, string centroCostoFin, int empleadoInicio, int empleadoFin, List<string> listaEstatus, List<string> listaNumeroSerie)
        {
            var result = new Dictionary<string, object>();

            try
            {
                List<rptResguardoDTO> listaResguardos = new List<rptResguardoDTO>();

                switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
                {
                    #region Peru
                    case EmpresaEnum.Peru:
                        {
                            using (var _starsoft = new MainContextPeruStarSoft003BDCOMUN())
                            {
                                var listaResguardosPeru = _context.tblAlm_Resguardo.Where(x =>
                                    x.estatusRegistro &&
                                    x.empleado >= empleadoInicio &&
                                    x.empleado <= empleadoFin
                                ).ToList().Where(x => x.cc.CompareTo(centroCostoInicio) >= 0 && x.cc.CompareTo(centroCostoFin) <= 0).ToList();

                                if (listaEstatus != null && listaEstatus.Count() > 0)
                                {
                                    listaResguardosPeru = listaResguardosPeru.Where(x => listaEstatus.Contains(x.estatus)).ToList();
                                }

                                if (listaNumeroSerie != null && listaNumeroSerie.Count() > 0)
                                {
                                    listaResguardosPeru = listaResguardosPeru.Where(x => listaNumeroSerie.Contains(x.num_serie)).ToList();
                                }

                                var listaCC = _context.tblP_CC.ToList();
                                var listaInsumosStarsoft = _starsoft.MAEART.ToList();
                                var listaAlmacenesStarsoft = _starsoft.TABALM.ToList();
                                var listaEmpleadosResguardoPeru = _context.tblAlm_EmpleadoResguardo.Where(x => x.registroActivo).ToList();

                                listaResguardos = listaResguardosPeru.Select(x => new rptResguardoDTO
                                {
                                    cc = x.cc,
                                    folio = x.folio.ToString(),
                                    id_activo = x.id_activo.ToString(),
                                    id_tipo_activo = x.id_tipo_activo.ToString(),
                                    marca = x.marca,
                                    modelo = x.modelo,
                                    color = x.color,
                                    num_serie = x.num_serie,
                                    plan_desc = x.plan_desc,
                                    condiciones = x.condiciones,
                                    empleado = x.empleado.ToString(),
                                    licencia = x.licencia,
                                    fec_resguardo = x.fec_resguardo.ToShortDateString(),
                                    estatus = x.estatus,
                                    entrega = x.entrega.ToString(),
                                    autoriza = x.autoriza.ToString(),
                                    recibio = x.recibio.ToString(),
                                    condiciones_ret = x.condiciones_ret,
                                    fec_devolucion = x.fec_devolucion.ToString(),
                                    cantidad_resguardo = x.cantidad_resguardo.ToString(),
                                    alm_salida = x.alm_salida.ToString(),
                                    alm_entrada = x.alm_entrada.ToString(),
                                    costo_promedio = x.costo_promedio.ToString(),
                                    resguardo_parcial = x.resguardo_parcial.ToString(),

                                    ccDesc = listaCC.Where(y => y.cc == x.cc).Select(z => z.descripcion).FirstOrDefault(),
                                    empleadoDesc = listaEmpleadosResguardoPeru.Where(y => y.folio == x.empleado).Select(z => z.nombre + " " + z.apellidoPaterno + " " + z.apellidoMaterno).FirstOrDefault(),
                                    insumoDesc = listaInsumosStarsoft.Where(y => y.ACODIGO == ("0" + x.id_activo.ToString())).Select(z => z.ADESCRI).FirstOrDefault(),
                                    fechaResguardoString = x.fec_resguardo.ToShortDateString(),
                                    almacenDesc = listaAlmacenesStarsoft.Where(y => Int32.Parse(y.TAALMA) == x.alm_salida).Select(z => z.TADESCRI).FirstOrDefault(),
                                    tipoActivoDesc = ""
                                }).OrderBy(x => x.cc).ThenBy(x => x.folio).ToList();
                            }
                            break;
                        }
                    #endregion

                    #region Colombia
                    case EmpresaEnum.Colombia:
                        {
                            var listaResguardosColombia = _context.tblAlm_Resguardo.Where(x =>
                                    x.estatusRegistro &&
                                    x.empleado >= empleadoInicio &&
                                    x.empleado <= empleadoFin
                                ).ToList().Where(x => x.cc.CompareTo(centroCostoInicio) >= 0 && x.cc.CompareTo(centroCostoFin) <= 0).ToList();

                            if (listaEstatus != null && listaEstatus.Count() > 0)
                            {
                                listaResguardosColombia = listaResguardosColombia.Where(x => listaEstatus.Contains(x.estatus)).ToList();
                            }

                            if (listaNumeroSerie != null && listaNumeroSerie.Count() > 0)
                            {
                                listaResguardosColombia = listaResguardosColombia.Where(x => listaNumeroSerie.Contains(x.num_serie)).ToList();
                            }

                            var listaCC = _context.tblP_CC.ToList();
                            //var listaInsumosColombia = consultaCheckProductivo(string.Format(@"SELECT * FROM insumos"));
                            //var listaAlmacenesColombia = consultaCheckProductivo(string.Format(@"SELECT * FROM si_almacen"));
                            var listaEmpleadosResguardoColombia = _context.tblAlm_EmpleadoResguardo.Where(x => x.registroActivo).ToList();

                            foreach (var item in listaResguardosColombia)
                            {
                                var insumoDescEK = consultaCheckProductivo(string.Format(@"SELECT * FROM insumos WHERE insumo = {0}", (int)item.id_activo));
                                var insumoDesc = (string)(((List<dynamic>)insumoDescEK.ToObject<List<dynamic>>())[0].descripcion);
                                var almacenDescEK = consultaCheckProductivo(string.Format(@"SELECT * FROM si_almacen WHERE almacen = {0}", (int)item.alm_entrada));
                                var almacenDesc = (string)(((List<dynamic>)insumoDescEK.ToObject<List<dynamic>>())[0].descripcion);

                                listaResguardos = listaResguardosColombia.Select(x => new rptResguardoDTO
                                {
                                    cc = x.cc,
                                    folio = x.folio.ToString(),
                                    id_activo = x.id_activo.ToString(),
                                    id_tipo_activo = x.id_tipo_activo.ToString(),
                                    marca = x.marca,
                                    modelo = x.modelo,
                                    color = x.color,
                                    num_serie = x.num_serie,
                                    plan_desc = x.plan_desc,
                                    condiciones = x.condiciones,
                                    empleado = x.empleado.ToString(),
                                    licencia = x.licencia,
                                    fec_resguardo = x.fec_resguardo.ToShortDateString(),
                                    estatus = x.estatus,
                                    entrega = x.entrega.ToString(),
                                    autoriza = x.autoriza.ToString(),
                                    recibio = x.recibio.ToString(),
                                    condiciones_ret = x.condiciones_ret,
                                    fec_devolucion = x.fec_devolucion.ToString(),
                                    cantidad_resguardo = x.cantidad_resguardo.ToString(),
                                    alm_salida = x.alm_salida.ToString(),
                                    alm_entrada = x.alm_entrada.ToString(),
                                    costo_promedio = x.costo_promedio.ToString(),
                                    resguardo_parcial = x.resguardo_parcial.ToString(),

                                    ccDesc = listaCC.Where(y => y.cc == x.cc).Select(z => z.descripcion).FirstOrDefault(),
                                    empleadoDesc = listaEmpleadosResguardoColombia.Where(y => y.folio == x.empleado).Select(z => z.nombre + " " + z.apellidoPaterno + " " + z.apellidoMaterno).FirstOrDefault(),
                                    insumoDesc = insumoDesc ?? "",
                                    fechaResguardoString = x.fec_resguardo.ToShortDateString(),
                                    almacenDesc = almacenDesc ?? "",
                                    tipoActivoDesc = ""
                                }).OrderBy(x => x.cc).ThenBy(x => x.folio).ToList();
                            }
                            break;
                        }
                    #endregion

                    #region Mexico
                    default:
                        {
                            var listaEstatusString = "";

                            if (listaEstatus != null && listaEstatus.Count() > 0)
                            {
                                listaEstatusString = string.Join(", ", listaEstatus.Select(x => "'" + x + "'"));
                            }

                            var listaNumeroSerieString = "";

                            if (listaNumeroSerie != null && listaNumeroSerie.Count() > 0)
                            {
                                listaNumeroSerieString = string.Join(", ", listaNumeroSerie.Select(x => "'" + x + "'"));
                            }

                            listaResguardos = _contextEnkontrol.Select<rptResguardoDTO>(vSesiones.sesionEmpresaActual == 1 ? EnkontrolEnum.CplanProd : EnkontrolEnum.ArrenProd, new OdbcConsultaDTO()
                            {
                                consulta = @"
                                    SELECT
                                        res.*,
                                        cc.descripcion AS ccDesc,
                                        emp.descripcion AS empleadoDesc,
                                        ins.descripcion AS insumoDesc,
                                        CONVERT(varchar, fec_resguardo, 103) as fechaResguardoString,
                                        alm.descripcion AS almacenDesc,
                                        tr.descripcion AS tipoActivoDesc
                                    FROM si_resguardo_activo_fijo res
                                        INNER JOIN cc cc ON res.cc = cc.cc
                                        INNER JOIN empleados emp ON res.empleado = emp.empleado
                                        INNER JOIN insumos ins ON res.id_activo = ins.insumo
                                        INNER JOIN si_almacen alm ON res.alm_salida = alm.almacen
                                        INNER JOIN si_tipo_activo_fijo tr ON res.id_tipo_activo = tr.id_tipo_activo
                                    WHERE (res.cc >= ? AND res.cc <= ?) AND (res.empleado >= ? AND res.empleado <= ?)" +
                                                    (listaEstatusString != "" ? "AND estatus IN (" + listaEstatusString + ")" : " ") +
                                                    (listaNumeroSerieString != "" ? "AND num_serie IN (" + listaNumeroSerieString + ")" : " ") +
                                                @"ORDER BY res.cc, res.folio",
                                parametros = new List<OdbcParameterDTO> {
                                    new OdbcParameterDTO { nombre = "cc", tipo = OdbcType.VarChar, valor = centroCostoInicio },
                                    new OdbcParameterDTO { nombre = "cc", tipo = OdbcType.VarChar, valor = centroCostoFin },
                                    new OdbcParameterDTO { nombre = "empleado", tipo = OdbcType.Numeric, valor = empleadoInicio },
                                    new OdbcParameterDTO { nombre = "empleado", tipo = OdbcType.Numeric, valor = empleadoFin }
                                }
                            });
                            break;
                    #endregion
                        }
                }

                // Se agregan las columnas.
                var columnasCursos = new List<string> {
                    "C.C.",
                    "cc Descripcion",
                    "Fec. Resguardo",
                    "# Almacen",
                    "Almacén",
                    "# Tipo Resguardo",
                    "Tipo Resguardo",
                    "# Empleado",
                    "Empleado",
                    "# Insumos",
                    "Insumos",
                    "Cant. de Insum.",
                    "Folio",
                    "Observaciones",
                    "Número de Serie",
                    "Condiciones",
                    "Estatus"
                };

                var headersExcel = columnasCursos.ToArray();

                using (ExcelPackage excel = new ExcelPackage())
                {
                    var hoja = excel.Workbook.Worksheets.Add("Resguardos");

                    List<string[]> headerRow = new List<string[]>() { headersExcel };

                    string headerRange = "A1:" + ExcelUtilities.GetExcelColumnName(headersExcel.Count()) + "1";

                    hoja.Cells[headerRange].LoadFromArrays(headerRow);

                    var cellData = new List<object[]>();

                    foreach (var resguardo in listaResguardos)
                    {
                        var arregloValores = new object[17];

                        arregloValores[0] = resguardo.cc;
                        arregloValores[1] = resguardo.ccDesc;
                        arregloValores[2] = resguardo.fechaResguardoString;
                        arregloValores[3] = Convert.ToInt32(resguardo.alm_salida);
                        arregloValores[4] = resguardo.almacenDesc;
                        arregloValores[5] = Convert.ToInt32(resguardo.id_tipo_activo);
                        arregloValores[6] = resguardo.tipoActivoDesc;
                        arregloValores[7] = Convert.ToInt32(resguardo.empleado);
                        arregloValores[8] = resguardo.empleadoDesc;
                        arregloValores[9] = Convert.ToInt32(resguardo.id_activo);
                        arregloValores[10] = resguardo.insumoDesc;
                        arregloValores[11] = Convert.ToDecimal(resguardo.cantidad_resguardo) - ((resguardo.resguardo_parcial != null && resguardo.resguardo_parcial != "") ? Convert.ToDecimal(resguardo.resguardo_parcial) : 0);
                        arregloValores[12] = Convert.ToInt32(resguardo.folio);
                        arregloValores[13] = resguardo.observaciones ?? "";
                        arregloValores[14] = resguardo.num_serie ?? "";
                        arregloValores[15] = resguardo.condiciones;
                        arregloValores[16] = resguardo.estatus;

                        cellData.Add(arregloValores);
                    }

                    hoja.Cells[2, 1].LoadFromArrays(cellData);

                    ExcelRange range = hoja.Cells[1, 1, hoja.Dimension.End.Row, hoja.Dimension.End.Column];
                    ExcelTable tab = hoja.Tables.Add(range, "Tabla_1");

                    tab.TableStyle = TableStyles.Medium17;

                    excel.Compression = OfficeOpenXml.CompressionLevel.BestSpeed;

                    hoja.Cells[hoja.Dimension.Address].AutoFitColumns();

                    List<byte[]> lista = new List<byte[]>();

                    var bytes = new MemoryStream();

                    using (var exportData = new MemoryStream())
                    {
                        excel.SaveAs(exportData);
                        bytes = exportData;
                    }

                    HttpContext.Current.Session["ReporteBitacoraResguardos"] = bytes;
                }

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, e.Message);
            }

            return result;
        }

        public List<rptResguardoDTO> cargarSesionReporteBitacoraResguardosCrystal(string centroCostoInicio, string centroCostoFin, int empleadoInicio, int empleadoFin, List<string> listaEstatus, List<string> listaNumeroSerie)
        {
            List<rptResguardoDTO> listaResguardos = new List<rptResguardoDTO>();

            centroCostoInicio = centroCostoInicio.ToUpper();
            centroCostoFin = centroCostoFin.ToUpper();

            try
            {
                switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
                {
                    #region Peru
                    case EmpresaEnum.Peru:
                        {
                            using (var _starsoft = new MainContextPeruStarSoft003BDCOMUN())
                            {
                                var listaResguardosPeru = _context.tblAlm_Resguardo.Where(x =>
                                    x.estatusRegistro &&
                                    x.empleado >= empleadoInicio &&
                                    x.empleado <= empleadoFin
                                ).ToList().Where(x => x.cc.CompareTo(centroCostoInicio) >= 0 && x.cc.CompareTo(centroCostoFin) <= 0).ToList();

                                if (listaEstatus != null && listaEstatus.Count() > 0)
                                {
                                    listaResguardosPeru = listaResguardosPeru.Where(x => listaEstatus.Contains(x.estatus)).ToList();
                                }

                                if (listaNumeroSerie != null && listaNumeroSerie.Count() > 0)
                                {
                                    listaResguardosPeru = listaResguardosPeru.Where(x => listaNumeroSerie.Contains(x.num_serie)).ToList();
                                }

                                var listaCC = _context.tblP_CC.ToList();
                                var listaInsumosStarsoft = _starsoft.MAEART.ToList();
                                var listaAlmacenesStarsoft = _starsoft.TABALM.ToList();
                                var listaEmpleadosResguardoPeru = _context.tblAlm_EmpleadoResguardo.Where(x => x.registroActivo).ToList();

                                listaResguardos = listaResguardosPeru.Select(x => new rptResguardoDTO
                                {
                                    cc = x.cc,
                                    folio = x.folio.ToString(),
                                    id_activo = x.id_activo.ToString(),
                                    id_tipo_activo = x.id_tipo_activo.ToString(),
                                    marca = x.marca,
                                    modelo = x.modelo,
                                    color = x.color,
                                    num_serie = x.num_serie,
                                    plan_desc = x.plan_desc,
                                    condiciones = x.condiciones,
                                    empleado = x.empleado.ToString(),
                                    licencia = x.licencia,
                                    fec_resguardo = x.fec_resguardo.ToShortDateString(),
                                    estatus = x.estatus,
                                    entrega = x.entrega.ToString(),
                                    autoriza = x.autoriza.ToString(),
                                    recibio = x.recibio.ToString(),
                                    condiciones_ret = x.condiciones_ret,
                                    fec_devolucion = x.fec_devolucion.ToString(),
                                    cantidad_resguardo = x.cantidad_resguardo.ToString(),
                                    alm_salida = x.alm_salida.ToString(),
                                    alm_entrada = x.alm_entrada.ToString(),
                                    costo_promedio = x.costo_promedio.ToString(),
                                    resguardo_parcial = x.resguardo_parcial.ToString(),

                                    ccDesc = listaCC.Where(y => y.cc == x.cc).Select(z => z.descripcion).FirstOrDefault(),
                                    empleadoDesc = listaEmpleadosResguardoPeru.Where(y => y.folio == x.empleado).Select(z => z.nombre + " " + z.apellidoPaterno + " " + z.apellidoMaterno).FirstOrDefault(),
                                    insumoDesc = listaInsumosStarsoft.Where(y => y.ACODIGO == ("0" + x.id_activo.ToString())).Select(z => z.ADESCRI).FirstOrDefault(),
                                    fechaResguardoString = x.fec_resguardo.ToShortDateString(),
                                    almacenDesc = listaAlmacenesStarsoft.Where(y => Int32.Parse(y.TAALMA) == x.alm_salida).Select(z => z.TADESCRI).FirstOrDefault(),
                                    tipoActivoDesc = ""
                                }).OrderBy(x => x.cc).ThenBy(x => x.folio).ToList();
                            }
                        }
                    #endregion
                        break;
                    #region Colombia
                    case EmpresaEnum.Colombia:
                        {
                            var listaResguardosColombia = _context.tblAlm_Resguardo.Where(x =>
                                x.estatusRegistro &&
                                x.empleado >= empleadoInicio &&
                                x.empleado <= empleadoFin
                            ).ToList().Where(x => x.cc.CompareTo(centroCostoInicio) >= 0 && x.cc.CompareTo(centroCostoFin) <= 0).ToList();

                            if (listaEstatus != null && listaEstatus.Count() > 0)
                            {
                                listaResguardosColombia = listaResguardosColombia.Where(x => listaEstatus.Contains(x.estatus)).ToList();
                            }

                            if (listaNumeroSerie != null && listaNumeroSerie.Count() > 0)
                            {
                                listaResguardosColombia = listaResguardosColombia.Where(x => listaNumeroSerie.Contains(x.num_serie)).ToList();
                            }

                            var listaCC = _context.tblP_CC.ToList();
                            var listaEmpleadosResguardoColombia = _context.tblAlm_EmpleadoResguardo.Where(x => x.registroActivo).ToList();

                            foreach (var item in listaResguardosColombia)
                            {
                                var insumoDescEK = consultaCheckProductivo(string.Format(@"SELECT * FROM insumos WHERE insumo = {0}", (int)item.id_activo));
                                var insumoDesc = (string)(((List<dynamic>)insumoDescEK.ToObject<List<dynamic>>())[0].descripcion);
                                var almacenDescEK = consultaCheckProductivo(string.Format(@"SELECT * FROM si_almacen WHERE almacen = {0}", (int)item.alm_entrada));
                                var almacenDesc = (string)(((List<dynamic>)almacenDescEK.ToObject<List<dynamic>>())[0].descripcion);

                                listaResguardos.Add(new rptResguardoDTO
                                {
                                    cc = item.cc,
                                    folio = item.folio.ToString(),
                                    id_activo = item.id_activo.ToString(),
                                    id_tipo_activo = item.id_tipo_activo.ToString(),
                                    marca = item.marca,
                                    modelo = item.modelo,
                                    color = item.color,
                                    num_serie = item.num_serie,
                                    plan_desc = item.plan_desc,
                                    condiciones = item.condiciones,
                                    empleado = item.empleado.ToString(),
                                    licencia = item.licencia,
                                    fec_resguardo = item.fec_resguardo.ToShortDateString(),
                                    estatus = item.estatus,
                                    entrega = item.entrega.ToString(),
                                    autoriza = item.autoriza.ToString(),
                                    recibio = item.recibio.ToString(),
                                    condiciones_ret = item.condiciones_ret,
                                    fec_devolucion = item.fec_devolucion.ToString(),
                                    cantidad_resguardo = item.cantidad_resguardo.ToString(),
                                    alm_salida = item.alm_salida.ToString(),
                                    alm_entrada = item.alm_entrada.ToString(),
                                    costo_promedio = item.costo_promedio.ToString(),
                                    resguardo_parcial = item.resguardo_parcial.ToString(),

                                    ccDesc = listaCC.Where(y => y.cc == item.cc).Select(z => z.descripcion).FirstOrDefault(),
                                    empleadoDesc = listaEmpleadosResguardoColombia.Where(y => y.folio == item.empleado).Select(z => z.nombre + " " + z.apellidoPaterno + " " + z.apellidoMaterno).FirstOrDefault(),
                                    insumoDesc = insumoDesc ?? "",
                                    fechaResguardoString = item.fec_resguardo.ToShortDateString(),
                                    almacenDesc = almacenDesc ?? "",
                                    tipoActivoDesc = ""
                                });
                            }

                            listaResguardos = listaResguardos.OrderBy(x => x.cc).ThenBy(x => x.folio).ToList();
                        }
                    #endregion
                        break;
                    #region Mexico
                    default:
                        {
                            var listaEstatusString = "";

                            if (listaEstatus != null && listaEstatus.Count() > 0)
                            {
                                listaEstatusString = string.Join(", ", listaEstatus.Select(x => "'" + x + "'"));
                            }

                            var listaNumeroSerieString = "";

                            if (listaNumeroSerie != null && listaNumeroSerie.Count() > 0)
                            {
                                listaNumeroSerieString = string.Join(", ", listaNumeroSerie.Select(x => "'" + x + "'"));
                            }

                            listaResguardos = _contextEnkontrol.Select<rptResguardoDTO>(vSesiones.sesionEmpresaActual == 1 ? EnkontrolEnum.CplanProd : EnkontrolEnum.ArrenProd, new OdbcConsultaDTO()
                            {
                                consulta = @"
                                    SELECT
                                        res.*,
                                        cc.descripcion AS ccDesc,
                                        emp.descripcion AS empleadoDesc,
                                        ins.descripcion AS insumoDesc,
                                        CONVERT(varchar, fec_resguardo, 103) as fechaResguardoString,
                                        alm.descripcion AS almacenDesc,
                                        tr.descripcion AS tipoActivoDesc
                                    FROM si_resguardo_activo_fijo res
                                        INNER JOIN cc cc ON res.cc = cc.cc
                                        INNER JOIN empleados emp ON res.empleado = emp.empleado
                                        INNER JOIN insumos ins ON res.id_activo = ins.insumo
                                        INNER JOIN si_almacen alm ON res.alm_salida = alm.almacen
                                        INNER JOIN si_tipo_activo_fijo tr ON res.id_tipo_activo = tr.id_tipo_activo
                                    WHERE (res.cc >= ? AND res.cc <= ?) AND (res.empleado >= ? AND res.empleado <= ?)" +
                                                    (listaEstatusString != "" ? "AND estatus IN (" + listaEstatusString + ")" : " ") +
                                                    (listaNumeroSerieString != "" ? "AND num_serie IN (" + listaNumeroSerieString + ")" : " ") +
                                                @"ORDER BY res.cc, res.folio",
                                parametros = new List<OdbcParameterDTO> {
                                    new OdbcParameterDTO { nombre = "cc", tipo = OdbcType.VarChar, valor = centroCostoInicio },
                                    new OdbcParameterDTO { nombre = "cc", tipo = OdbcType.VarChar, valor = centroCostoFin },
                                    new OdbcParameterDTO { nombre = "empleado", tipo = OdbcType.Numeric, valor = empleadoInicio },
                                    new OdbcParameterDTO { nombre = "empleado", tipo = OdbcType.Numeric, valor = empleadoFin }
                                }
                            });
                        }
                    #endregion
                        break;
                }
            }
            catch (Exception ex)
            {

            }

            return listaResguardos;
        }


        public Dictionary<string, object> getCentrosCostos(int sessionActual)
        {
            var result = new Dictionary<string, object>();

            try
            {
                switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
                {
                    case EmpresaEnum.Peru:
                        {
                            var listaCentroCosto = _context.tblP_CC.Where(x => x.estatus && x.cc.Length > 3).Select(x => new ResguardoCCDTO
                            {
                                cc = x.cc,
                                descripcion = x.descripcion.Trim()
                            }).ToList();

                            result.Add("data", listaCentroCosto);
                            result.Add(SUCCESS, true);
                            break;
                        }
                    case EmpresaEnum.Colombia:
                        {
                            //var listaCentroCosto = _context.tblC_Nom_CatalogoCC.Where(x => x.estatus).Select(x => new ResguardoCCDTO
                            //{
                            //    cc = x.cc,
                            //    descripcion = x.ccDescripcion.Trim()
                            //}).ToList();
                            var centrosEK = new List<ResguardoCCDTO>();
                            centrosEK = _contextEnkontrol.Select<ResguardoCCDTO>(EnkontrolEnum.ColombiaProductivo, @"SELECT cc, descripcion FROM DBA.cc");

                            result.Add("data", centrosEK);
                            result.Add(SUCCESS, true);
                            break;
                        }
                    default:
                        {
                            var centrosEK = new List<ResguardoCCDTO>();

                            if (sessionActual == 1)
                            {
                                centrosEK = _contextEnkontrol.Select<ResguardoCCDTO>(EnkontrolEnum.CplanRh, @"SELECT cc, descripcion FROM cc");
                            }
                            else
                            {
                                centrosEK = _contextEnkontrol.Select<ResguardoCCDTO>(EnkontrolEnum.ArrenRh, @"SELECT cc, descripcion FROM cc");
                            }

                            if (centrosEK != null)
                            {
                                result.Add("data", centrosEK);
                                result.Add(SUCCESS, true);
                            }
                            break;
                        }
                }
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return result;
        }


        private dynamic consultaCheckProductivoRH(string consulta)
        {
            if (productivo)
            {
                return ContextEnKontrolNomina.Where(consulta);
            }
            else
            {
                return ContextEnKontrolNominaPrueba.Where(consulta);
            }
        }

        private EnkontrolEnum getEnkontrolEnumADM()
        {
            var baseDatos = new EnkontrolEnum();

            if (productivo)
            {
                if (vSesiones.sesionEmpresaActual == 1)
                {
                    baseDatos = EnkontrolEnum.CplanProd;
                }
                else if (vSesiones.sesionEmpresaActual == 2)
                {
                    baseDatos = EnkontrolEnum.ArrenProd;
                }
                else if (vSesiones.sesionEmpresaActual == 3)
                {
                    baseDatos = EnkontrolEnum.ColombiaProductivo;
                }
                else
                {
                    throw new Exception("Empresa distinta a Construplan, Arrendadora y Colombia");
                }
            }
            else
            {
                if (vSesiones.sesionEmpresaActual == 1)
                {
                    baseDatos = EnkontrolEnum.PruebaCplanProd;
                }
                else if (vSesiones.sesionEmpresaActual == 2)
                {
                    baseDatos = EnkontrolEnum.PruebaArrenADM;
                }
                else
                {
                    throw new Exception("Empresa distinta a Construplan y Arrendadora");
                }
            }

            return baseDatos;
        }
    }
}
