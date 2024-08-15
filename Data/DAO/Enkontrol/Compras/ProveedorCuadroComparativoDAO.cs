using Core.DAO.Enkontrol.Compras;
using Core.DTO;
using Core.DTO.Enkontrol.OrdenCompra;
using Core.DTO.Enkontrol.Requisicion;
using Core.DTO.Utils.Data;
using Core.Entity.Enkontrol.Compras.OrdenCompra;
using Core.Entity.Enkontrol.Compras.Requisicion;
using Core.Entity.Principal.Bitacoras;
using Core.Entity.Principal.Usuarios;
using Core.Enum.Enkontrol.Requisicion;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Data.DAO.Enkontrol.Compras
{
    public class ProveedorCuadroComparativoDAO : GenericDAO<tblP_Usuario>, IProveedorCuadroComparativoDAO
    {
        private string NombreControlador = "ProveedorCuadroComparativoController";
        Dictionary<string, object> resultado = new Dictionary<string, object>();
        private bool productivo = Convert.ToString(System.Web.Configuration.WebConfigurationManager.AppSettings["enkontrolProductivo"]) == "1";
        private readonly string RutaArchivos;
        private readonly string RutaBase = @"\\10.1.0.112\Proyecto\SIGOPLAN\COMPRAS";
        private const string RutaLocal = @"C:\Proyecto\SIGOPLAN\COMPRAS";
        private const int _SISTEMA = (int)SistemasEnum.COMPRAS;
        private const string _NOMBRE_CONTROLADOR = "ProveedorCuadroComparativoController";

        public ProveedorCuadroComparativoDAO()
        {
            resultado.Clear();

#if DEBUG
            RutaBase = RutaLocal;
#endif

            RutaArchivos = Path.Combine(RutaBase, "CUADROS_COMPARATIVOS");
        }

        public Dictionary<string, object> VerificarProveedorRelHash(string hash)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (string.IsNullOrEmpty(hash)) { throw new Exception("Ocurrió un error al ingresar, favor de contactar a ??"); }
                #endregion

                #region SE VERIFICA SI EL PROVEEDOR CUENTA CON ACCESO A LA VISTA DE CUADRO COMPARATIVO
                tblCom_ProveedoresLinks objProveedorRelLink = _context.tblCom_ProveedoresLinks.Where(w => w.hash == hash && w.idEstatusRegistro == EstatusRegistroProveedorLinkEnum.PENDIENTE && w.registroActivo).FirstOrDefault();
                if (objProveedorRelLink == null)
                    throw new Exception("");

                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> GetDatosProveedor(string hash)
        {
            resultado = new Dictionary<string, object>();

            try
            {
                #region VALIDACIONES
                if (string.IsNullOrEmpty(hash)) { throw new Exception("Ocurrió un error al ingresar, favor de contactar a ??"); }
                #endregion

                #region SE VERIFICA SI EL HASH VIENE DE CONSTRUPLAN O ARRENDADORA
                tblCom_ProveedoresLinks objProveedorSIGOPLAN = new tblCom_ProveedoresLinks();
                using (var _SIGOPLAN = new MainContext(EmpresaEnum.Construplan))
                {
                    objProveedorSIGOPLAN = _SIGOPLAN.tblCom_ProveedoresLinks.Where(w => w.hash == hash && w.idEstatusRegistro == EstatusRegistroProveedorLinkEnum.PENDIENTE && w.registroActivo).FirstOrDefault();
                }

                if (objProveedorSIGOPLAN == null)
                {
                    using (var _ARRENDADORA = new MainContext(EmpresaEnum.Arrendadora))
                    {
                        objProveedorSIGOPLAN = _ARRENDADORA.tblCom_ProveedoresLinks.Where(w => w.hash == hash && w.idEstatusRegistro == EstatusRegistroProveedorLinkEnum.PENDIENTE && w.registroActivo).FirstOrDefault();
                    }
                }

                if (objProveedorSIGOPLAN == null)
                    throw new Exception("Ocurrió un error al ingresar, favor de contactar a ??");
                #endregion

                #region SE OBTIENE EL NOMBRE DEL PROVEEDOR
                EnkontrolEnum idEmpresa = (int)EmpresaEnum.Construplan == objProveedorSIGOPLAN.idEmpresa ? EnkontrolEnum.CplanProd : EnkontrolEnum.ArrenProd;
#if DEBUG
                //idEmpresa = (int)EmpresaEnum.Construplan == objProveedorSIGOPLAN.idEmpresa ? EnkontrolEnum.PruebaCplanProd : EnkontrolEnum.PruebaArrenADM;
#endif

                ProveedorLinkDTO objProveedorDTO = _contextEnkontrol.Select<ProveedorLinkDTO>(idEmpresa, new OdbcConsultaDTO()
                {
                    consulta = @"SELECT numpro, nombre FROM sp_proveedores WHERE numpro = ?",
                    parametros = new List<OdbcParameterDTO> {
                    new OdbcParameterDTO { nombre = "numpro", tipo = OdbcType.Int, valor = objProveedorSIGOPLAN.idProveedor }
                }
                }).FirstOrDefault();
                if (objProveedorDTO == null)
                    throw new Exception("Ocurrió un error al ingresar, favor de contactar a ??");

                string nombreProveedor = string.Empty;
                nombreProveedor = string.Format("[{0}] {1}", objProveedorDTO.numpro, !string.IsNullOrEmpty(objProveedorDTO.nombre) ? objProveedorDTO.nombre.Trim().ToUpper() : "COTIZACIÓN");

                var objProv = new
                {
                    nombreProveedor = nombreProveedor,
                    proveedor = objProveedorDTO.nombre,
                    idEmpresa = objProveedorSIGOPLAN.idEmpresa,
                    cc = objProveedorSIGOPLAN.cc,
                    numRequisicion = objProveedorSIGOPLAN.numRequisicion,
                    numpro = objProveedorDTO.numpro,
                    moneda = objProveedorDTO.numpro >= 9000 ? "USD" : "MN"
                };
                resultado.Add("objProv", objProv);
                #endregion

                #region SE OBTIENE LOS INSUMOS EN BASE AL CC Y NÚMERO DE REQUISICIÓN
                List<RequisicionDetDTO> lstRequisicionDetDTO = _contextEnkontrol.Select<RequisicionDetDTO>(idEmpresa, new OdbcConsultaDTO()
                {
                    consulta = @"SELECT * FROM so_requisicion_det WHERE cc = ? AND numero = ?",
                    parametros = new List<OdbcParameterDTO> {
                    new OdbcParameterDTO { nombre = "cc", tipo = OdbcType.Char, valor = objProveedorSIGOPLAN.cc },
                    new OdbcParameterDTO { nombre = "numero", tipo = OdbcType.Numeric, valor = objProveedorSIGOPLAN.numRequisicion }
                }
                }).ToList();
                if (lstRequisicionDetDTO == null)
                    throw new Exception("Ocurrió un error al obtener las partidas, favor de contactar a ??");

                foreach (var item in lstRequisicionDetDTO)
                {
                    item.estatus = "$0.000000";

                    // SE OBTIENE DESCRIPCION DEL INSUMO
                    ProveedorLinkDTO objDescripcionInsumo = _contextEnkontrol.Select<ProveedorLinkDTO>(idEmpresa, new OdbcConsultaDTO()
                    {
                        consulta = @"SELECT descripcion FROM insumos WHERE insumo = ?",
                        parametros = new List<OdbcParameterDTO> {
                        new OdbcParameterDTO { nombre = "insumo", tipo = OdbcType.Int, valor = item.insumo }
                    }
                    }).FirstOrDefault();

                    // SE OBTIENE DESCRIPCION DE LA PARTIDA
                    ProveedorLinkDTO objDescripcionPartida = _contextEnkontrol.Select<ProveedorLinkDTO>(idEmpresa, new OdbcConsultaDTO()
                    {
                        consulta = @"SELECT descripcion FROM so_req_det_linea WHERE cc = ? AND numero = ? AND partida = ?",
                        parametros = new List<OdbcParameterDTO> {
                        new OdbcParameterDTO { nombre = "cc", tipo = OdbcType.Char, valor = item.cc },
                        new OdbcParameterDTO { nombre = "numero", tipo = OdbcType.Numeric, valor = item.numero },
                        new OdbcParameterDTO { nombre = "partida", tipo = OdbcType.Numeric, valor = item.partida }
                    }
                    }).FirstOrDefault();

                    item.insumoDescripcion = string.Format("[{0}] {1}", item.insumo, objDescripcionInsumo.descripcion);
                    item.insumoDesc = objDescripcionPartida.descripcion;
                }

                resultado.Add("lstRequisicionDetDTO", lstRequisicionDetDTO);
                #endregion

                resultado.Add(SUCCESS, true);
                resultado.Add("idEmpresa", objProveedorSIGOPLAN.idEmpresa);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "GetNombreProveedor", e, AccionEnum.CONSULTA, 0, new { hash = hash });
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> GuardarCuadroComparativo(CuadroComparativoDTO cuadro, HttpPostedFileBase archivo)
        {
            resultado = new Dictionary<string, object>();

            using (var _contextEmpresa = new MainContext(cuadro.empresa))
            {
                using (var dbSigoplanTransaction = _contextEmpresa.Database.BeginTransaction())
                {
                    using (var con = checkConexionProductivo(cuadro.empresa))
                    {
                        using (var trans = con.BeginTransaction())
                        {
                            try
                            {
                                var registroCuadroEK = _contextEnkontrol.Select<CuadroComparativoDTO>(getEnkontrolAmbienteConsulta(cuadro.empresa), string.Format(@"
                                    SELECT TOP 1 * FROM so_cuadro_comparativo WHERE cc = '{0}' AND numero = {1} ORDER BY folio DESC", cuadro.cc, cuadro.numero
                                )).FirstOrDefault();

                                if (registroCuadroEK == null) //Nuevo registro de cuadro comparativo con folio 01.
                                {
                                    GuardarNuevoFolioCuadro(dbSigoplanTransaction, trans, _contextEmpresa, cuadro, 1, archivo);
                                }
                                else
                                {
                                    if (registroCuadroEK.prov1 == 0 || registroCuadroEK.prov1 == null) //Existe el registro pero no se llenó la primera posición. (No debería suceder)
                                    {
                                        UpdateCuadroSiguienteProveedor(dbSigoplanTransaction, trans, _contextEmpresa, cuadro, registroCuadroEK.folio, 1, archivo);
                                    }
                                    else if (registroCuadroEK.prov2 == 0 || registroCuadroEK.prov2 == null) //Si la primera posición ya se llenó se agrega en la segunda.
                                    {
                                        UpdateCuadroSiguienteProveedor(dbSigoplanTransaction, trans, _contextEmpresa, cuadro, registroCuadroEK.folio, 2, archivo);
                                    }
                                    else if (registroCuadroEK.prov3 == 0 || registroCuadroEK.prov3 == null) //Si la segunda posición ya se llenó se agrega en la tercera.
                                    {
                                        UpdateCuadroSiguienteProveedor(dbSigoplanTransaction, trans, _contextEmpresa, cuadro, registroCuadroEK.folio, 3, archivo);
                                    }
                                    else //Si la tercera posición ya se llenó se guarda un nuevo registro con el siguiente folio.
                                    {
                                        var siguienteFolio = registroCuadroEK.folio + 1;

                                        GuardarNuevoFolioCuadro(dbSigoplanTransaction, trans, _contextEmpresa, cuadro, siguienteFolio, archivo);
                                    }
                                }

                                #region Cerrar link de proveedor
                                var empresa = (int)cuadro.empresa;
                                var registroLink = _contextEmpresa.tblCom_ProveedoresLinks.FirstOrDefault(x =>
                                    x.registroActivo && x.cc == cuadro.cc && x.idProveedor == cuadro.prov1 && x.numRequisicion == cuadro.numero && x.idEmpresa == empresa && x.idEstatusRegistro == EstatusRegistroProveedorLinkEnum.PENDIENTE
                                );

                                if (registroLink != null)
                                {
                                    registroLink.idEstatusRegistro = EstatusRegistroProveedorLinkEnum.REALIZADO;
                                    _contextEmpresa.SaveChanges();
                                }
                                else
                                {
                                    throw new Exception("No se guardó la información. El link no es válido o ya fue cerrado.");
                                }
                                #endregion

                                trans.Commit();
                                dbSigoplanTransaction.Commit();

                                resultado.Add(SUCCESS, true);
                                SaveBitacoraProveedor(4, (int)AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(cuadro), cuadro.prov1, _contextEmpresa);
                            }
                            catch (Exception e)
                            {
                                trans.Rollback();
                                dbSigoplanTransaction.Rollback();

                                string asunto = string.Format(@"Error Captura Externa Cuadro Comparativo Proveedor. [Req. {0}-{1}] [Prov. {2}]", cuadro.cc, cuadro.numero, cuadro.prov1);
                                string mensaje = string.Format(@"Requisición: {0}-{1}<br/><br/>Proveedor: {2}<br/><br/>Error Mensaje: {3}<br/><br/>StackTrace: {4}", cuadro.cc, cuadro.numero, cuadro.prov1, e.Message, e.StackTrace);

                                GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), asunto), mensaje, new List<string>() { "oscar.valencia@construplan.com.mx", "omar.nunez@construplan.com.mx" });

                                LogError(0, 0, NombreControlador, "GuardarCuadroComparativo", e, AccionEnum.AGREGAR, 0, cuadro);

                                resultado.Add(MESSAGE, e.Message);
                                resultado.Add(SUCCESS, false);
                            }
                        }
                    }
                }
            }

            return resultado;
        }

        private void GuardarNuevoFolioCuadro(DbContextTransaction dbSigoplanTransaction, OdbcTransaction trans, MainContext _contextEmpresa, CuadroComparativoDTO cuadro, int folio, HttpPostedFileBase archivo)
        {
            #region SIGOPLAN
            #region General
            var cuadroSIGOPLAN = new tblCom_CuadroComparativo();

            cuadroSIGOPLAN.cc = cuadro.cc;
            cuadroSIGOPLAN.numero = cuadro.numero;
            cuadroSIGOPLAN.folio = folio;
            cuadroSIGOPLAN.fecha = DateTime.Now;

            cuadroSIGOPLAN.prov1 = cuadro.prov1 ?? 0;
            cuadroSIGOPLAN.porcent_dcto1 = cuadro.porcent_dcto1;
            cuadroSIGOPLAN.porcent_iva1 = cuadro.porcent_iva1;
            cuadroSIGOPLAN.dcto1 = cuadro.dcto1;
            cuadroSIGOPLAN.iva1 = cuadro.iva1;
            cuadroSIGOPLAN.total1 = cuadro.total1;
            cuadroSIGOPLAN.tipo_cambio1 = cuadro.tipo_cambio1;
            cuadroSIGOPLAN.fecha_entrega1 = cuadro.fecha_entrega1;
            cuadroSIGOPLAN.lab1 = 1;
            cuadroSIGOPLAN.dias_pago1 = 1;

            cuadroSIGOPLAN.prov2 = default(int);
            cuadroSIGOPLAN.porcent_dcto2 = default(decimal);
            cuadroSIGOPLAN.porcent_iva2 = default(decimal);
            cuadroSIGOPLAN.dcto2 = default(decimal);
            cuadroSIGOPLAN.iva2 = default(decimal);
            cuadroSIGOPLAN.total2 = default(decimal);
            cuadroSIGOPLAN.tipo_cambio2 = default(decimal);
            cuadroSIGOPLAN.fecha_entrega2 = cuadro.fecha_entrega1;
            cuadroSIGOPLAN.lab2 = default(int);
            cuadroSIGOPLAN.dias_pago2 = default(int);

            cuadroSIGOPLAN.prov3 = default(int);
            cuadroSIGOPLAN.porcent_dcto3 = default(decimal);
            cuadroSIGOPLAN.porcent_iva3 = default(decimal);
            cuadroSIGOPLAN.dcto3 = default(decimal);
            cuadroSIGOPLAN.iva3 = default(decimal);
            cuadroSIGOPLAN.total3 = default(decimal);
            cuadroSIGOPLAN.tipo_cambio3 = default(decimal);
            cuadroSIGOPLAN.fecha_entrega3 = cuadro.fecha_entrega1;
            cuadroSIGOPLAN.lab3 = default(int);
            cuadroSIGOPLAN.dias_pago3 = default(int);

            cuadroSIGOPLAN.solicito = 0;

            cuadroSIGOPLAN.sub_total1 = cuadro.sub_total1;
            cuadroSIGOPLAN.sub_total2 = default(decimal);
            cuadroSIGOPLAN.sub_total3 = default(decimal);

            cuadroSIGOPLAN.fletes1 = cuadro.fletes1;
            cuadroSIGOPLAN.fletes2 = default(decimal);
            cuadroSIGOPLAN.fletes3 = default(decimal);

            cuadroSIGOPLAN.gastos_imp1 = cuadro.gastos_imp1;
            cuadroSIGOPLAN.gastos_imp2 = default(decimal);
            cuadroSIGOPLAN.gastos_imp3 = default(decimal);

            cuadroSIGOPLAN.nombre_prov1 = cuadro.nombre_prov1 ?? "";
            cuadroSIGOPLAN.nombre_prov2 = "";
            cuadroSIGOPLAN.nombre_prov3 = "";

            cuadroSIGOPLAN.moneda1 = cuadro.moneda1 ?? 0;
            cuadroSIGOPLAN.moneda2 = default(int);
            cuadroSIGOPLAN.moneda3 = default(int);

            cuadroSIGOPLAN.inslic = false;
            cuadroSIGOPLAN.inslic_fecha_ini = null;
            cuadroSIGOPLAN.inslic_fecha_fin = null;

            cuadroSIGOPLAN.comentarios1 = cuadro.comentarios1 ?? "";
            cuadroSIGOPLAN.comentarios2 = "";
            cuadroSIGOPLAN.comentarios3 = "";

            #region Guardar Archivo Cotización
            string rutaArchivo = "";

            if (archivo != null)
            {
                rutaArchivo = Path.Combine(RutaArchivos, ObtenerFormatoNombreArchivo(cuadro.cc + "_" + cuadro.numero + "_" + folio + "_" + (int)cuadro.empresa, archivo.FileName));
                Tuple<bool, Exception> guardarArchivo = GlobalUtils.SaveHTTPPostedFileValidacion(archivo, rutaArchivo);

                if (!guardarArchivo.Item1)
                {
                    LogError(0, 0, NombreControlador, "GuardarCuadroComparativo_GuardarArchivo", guardarArchivo.Item2, AccionEnum.AGREGAR, 0, cuadro);
                    throw new Exception("Ocurrió un error al guardar los archivos en el servidor.");
                }
            }
            #endregion

            cuadroSIGOPLAN.rutaArchivo1 = rutaArchivo != "" ? rutaArchivo : null;
            cuadroSIGOPLAN.rutaArchivo2 = null;
            cuadroSIGOPLAN.rutaArchivo3 = null;

            cuadroSIGOPLAN.usuarioCreacion_id = 0;
            cuadroSIGOPLAN.fechaCreacion = DateTime.Now;
            cuadroSIGOPLAN.usuarioModificacion_id = 0;
            cuadroSIGOPLAN.fechaModificacion = null;
            cuadroSIGOPLAN.registroActivo = true;

            _contextEmpresa.tblCom_CuadroComparativo.Add(cuadroSIGOPLAN);
            _contextEmpresa.SaveChanges();
            #endregion

            #region Detalle
            foreach (var detalle in cuadro.detalleCuadro)
            {
                var cuadroDetalleSIGOPLAN = new tblCom_CuadroComparativoDet();

                cuadroDetalleSIGOPLAN.cc = detalle.cc;
                cuadroDetalleSIGOPLAN.numero = detalle.numero;
                cuadroDetalleSIGOPLAN.folio = folio;
                cuadroDetalleSIGOPLAN.partida = detalle.partida;
                cuadroDetalleSIGOPLAN.insumo = detalle.insumo;
                cuadroDetalleSIGOPLAN.cantidad = detalle.cantidad;
                cuadroDetalleSIGOPLAN.precio1 = detalle.precio1;
                cuadroDetalleSIGOPLAN.precio2 = default(decimal);
                cuadroDetalleSIGOPLAN.precio3 = default(decimal);
                cuadroDetalleSIGOPLAN.proveedor_uc = null;
                cuadroDetalleSIGOPLAN.oc_uc = null;
                cuadroDetalleSIGOPLAN.fecha_uc = null;
                cuadroDetalleSIGOPLAN.precio_uc = null;
                cuadroDetalleSIGOPLAN.usuarioCreacion_id = 0;
                cuadroDetalleSIGOPLAN.fechaCreacion = DateTime.Now;
                cuadroDetalleSIGOPLAN.usuarioModificacion_id = 0;
                cuadroDetalleSIGOPLAN.fechaModificacion = null;
                cuadroDetalleSIGOPLAN.registroActivo = true;

                _contextEmpresa.tblCom_CuadroComparativoDet.Add(cuadroDetalleSIGOPLAN);
                _contextEmpresa.SaveChanges();
            }
            #endregion
            #endregion

            #region Enkontrol
            #region General
            using (var cmd = new OdbcCommand(@"
                        INSERT INTO so_cuadro_comparativo (cc, numero, folio, fecha, prov1, porcent_dcto1, porcent_iva1, dcto1, iva1, total1, tipo_cambio1, fecha_entrega1, lab1, dias_pago1, 
                            prov2, porcent_dcto2, porcent_iva2, dcto2, iva2, total2, tipo_cambio2, fecha_entrega2, lab2, dias_pago2, prov3, porcent_dcto3, porcent_iva3, dcto3, iva3, total3, tipo_cambio3, fecha_entrega3, lab3, dias_pago3, 
                            solicito, sub_total1, sub_total2, sub_total3, fletes1, fletes2, fletes3, gastos_imp1, gastos_imp2, gastos_imp3, nombre_prov1, nombre_prov2, nombre_prov3, moneda1, moneda2, moneda3, 
                            inslic, inslic_fecha_ini, inslic_fecha_fin, comentarios1, comentarios2, comentarios3) 
                        VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"))
            {
                OdbcParameterCollection parameters = cmd.Parameters;

                parameters.Add("@cc", OdbcType.Char).Value = cuadro.cc;
                parameters.Add("@numero", OdbcType.Numeric).Value = cuadro.numero;
                parameters.Add("@folio", OdbcType.Numeric).Value = folio;
                parameters.Add("@fecha", OdbcType.Date).Value = DateTime.Now;

                parameters.Add("@prov1", OdbcType.Numeric).Value = cuadro.prov1 ?? (object)DBNull.Value;
                parameters.Add("@porcent_dcto1", OdbcType.Numeric).Value = cuadro.porcent_dcto1;
                parameters.Add("@porcent_iva1", OdbcType.Numeric).Value = cuadro.porcent_iva1;
                parameters.Add("@dcto1", OdbcType.Numeric).Value = cuadro.dcto1;
                parameters.Add("@iva1", OdbcType.Numeric).Value = cuadro.iva1;
                parameters.Add("@total1", OdbcType.Numeric).Value = cuadro.total1;
                parameters.Add("@tipo_cambio1", OdbcType.Numeric).Value = cuadro.tipo_cambio1;
                parameters.Add("@fecha_entrega1", OdbcType.Date).Value = cuadro.fecha_entrega1;
                parameters.Add("@lab1", OdbcType.Numeric).Value = 1;
                parameters.Add("@dias_pago1", OdbcType.Numeric).Value = 1;

                parameters.Add("@prov2", OdbcType.Numeric).Value = (object)DBNull.Value;
                parameters.Add("@porcent_dcto2", OdbcType.Numeric).Value = 0;
                parameters.Add("@porcent_iva2", OdbcType.Numeric).Value = 0;
                parameters.Add("@dcto2", OdbcType.Numeric).Value = 0;
                parameters.Add("@iva2", OdbcType.Numeric).Value = 0;
                parameters.Add("@total2", OdbcType.Numeric).Value = 0;
                parameters.Add("@tipo_cambio2", OdbcType.Numeric).Value = 1;
                parameters.Add("@fecha_entrega2", OdbcType.Date).Value = cuadro.fecha_entrega1;
                parameters.Add("@lab2", OdbcType.Numeric).Value = (object)DBNull.Value;
                parameters.Add("@dias_pago2", OdbcType.Numeric).Value = (object)DBNull.Value;

                parameters.Add("@prov3", OdbcType.Numeric).Value = (object)DBNull.Value;
                parameters.Add("@porcent_dcto3", OdbcType.Numeric).Value = 0;
                parameters.Add("@porcent_iva3", OdbcType.Numeric).Value = 0;
                parameters.Add("@dcto3", OdbcType.Numeric).Value = 0;
                parameters.Add("@iva3", OdbcType.Numeric).Value = 0;
                parameters.Add("@total3", OdbcType.Numeric).Value = 0;
                parameters.Add("@tipo_cambio3", OdbcType.Numeric).Value = 1;
                parameters.Add("@fecha_entrega3", OdbcType.Date).Value = cuadro.fecha_entrega1;
                parameters.Add("@lab3", OdbcType.Numeric).Value = (object)DBNull.Value;
                parameters.Add("@dias_pago3", OdbcType.Numeric).Value = (object)DBNull.Value;

                parameters.Add("@solicito", OdbcType.Numeric).Value = 0;

                parameters.Add("@sub_total1", OdbcType.Numeric).Value = cuadro.sub_total1;
                parameters.Add("@sub_total2", OdbcType.Numeric).Value = 0;
                parameters.Add("@sub_total3", OdbcType.Numeric).Value = 0;

                parameters.Add("@fletes1", OdbcType.Numeric).Value = cuadro.fletes1;
                parameters.Add("@fletes2", OdbcType.Numeric).Value = 0;
                parameters.Add("@fletes3", OdbcType.Numeric).Value = 0;

                parameters.Add("@gastos_imp1", OdbcType.Numeric).Value = cuadro.gastos_imp1;
                parameters.Add("@gastos_imp2", OdbcType.Numeric).Value = 0;
                parameters.Add("@gastos_imp3", OdbcType.Numeric).Value = 0;

                parameters.Add("@nombre_prov1", OdbcType.VarChar).Value = cuadro.nombre_prov1 ?? (object)DBNull.Value;
                parameters.Add("@nombre_prov2", OdbcType.VarChar).Value = (object)DBNull.Value;
                parameters.Add("@nombre_prov3", OdbcType.VarChar).Value = (object)DBNull.Value;

                parameters.Add("@moneda1", OdbcType.Char).Value = cuadro.moneda1 ?? (object)DBNull.Value;
                parameters.Add("@moneda2", OdbcType.Char).Value = (object)DBNull.Value;
                parameters.Add("@moneda3", OdbcType.Char).Value = (object)DBNull.Value;

                parameters.Add("@inslic", OdbcType.Bit).Value = false;
                parameters.Add("@inslic_fecha_ini", OdbcType.Date).Value = (object)DBNull.Value;
                parameters.Add("@inslic_fecha_fin", OdbcType.Date).Value = (object)DBNull.Value;

                parameters.Add("@comentarios1", OdbcType.Char).Value = cuadro.comentarios1 ?? (object)DBNull.Value;
                parameters.Add("@comentarios2", OdbcType.Char).Value = (object)DBNull.Value;
                parameters.Add("@comentarios3", OdbcType.Char).Value = (object)DBNull.Value;

                cmd.Connection = trans.Connection;
                cmd.Transaction = trans;
                cmd.ExecuteNonQuery();
            }
            #endregion

            #region Detalle
            foreach (var det in cuadro.detalleCuadro)
            {
                using (var cmd = new OdbcCommand(@"INSERT INTO so_cuadro_comparativo_det (cc, numero, folio, partida, insumo, cantidad, precio1, precio2, precio3, proveedor_uc, oc_uc, fecha_uc, precio_uc) VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?)"))
                {
                    OdbcParameterCollection parameters = cmd.Parameters;

                    parameters.Add("@cc", OdbcType.Char).Value = det.cc;
                    parameters.Add("@numero", OdbcType.Char).Value = det.numero;
                    parameters.Add("@folio", OdbcType.Char).Value = folio;
                    parameters.Add("@partida", OdbcType.Char).Value = det.partida;
                    parameters.Add("@insumo", OdbcType.Char).Value = det.insumo;
                    parameters.Add("@cantidad", OdbcType.Char).Value = det.cantidad;
                    parameters.Add("@precio1", OdbcType.Char).Value = det.precio1;
                    parameters.Add("@precio2", OdbcType.Char).Value = 0;
                    parameters.Add("@precio3", OdbcType.Char).Value = 0;
                    parameters.Add("@proveedor_uc", OdbcType.Char).Value = det.proveedor_uc ?? (object)DBNull.Value;
                    parameters.Add("@oc_uc", OdbcType.Char).Value = det.oc_uc ?? (object)DBNull.Value;
                    parameters.Add("@fecha_uc", OdbcType.Date).Value = det.fecha_uc ?? (object)DBNull.Value;
                    parameters.Add("@precio_uc", OdbcType.Char).Value = det.precio_uc ?? (object)DBNull.Value;

                    cmd.Connection = trans.Connection;
                    cmd.Transaction = trans;
                    cmd.ExecuteNonQuery();
                }
            }
            #endregion
            #endregion
        }

        private void UpdateCuadroSiguienteProveedor(DbContextTransaction dbSigoplanTransaction, OdbcTransaction trans, MainContext _contextEmpresa, CuadroComparativoDTO cuadro, int folio, int posicionProveedor, HttpPostedFileBase archivo)
        {
            #region SIGOPLAN
            #region General
            var cuadroSIGOPLAN = _contextEmpresa.tblCom_CuadroComparativo.FirstOrDefault(x => x.registroActivo && x.cc == cuadro.cc && x.numero == cuadro.numero && x.folio == folio);

            if (cuadroSIGOPLAN != null)
            {
                typeof(tblCom_CuadroComparativo).GetProperty("prov" + posicionProveedor).SetValue(cuadroSIGOPLAN, cuadro.prov1 ?? 0);
                typeof(tblCom_CuadroComparativo).GetProperty("porcent_dcto" + posicionProveedor).SetValue(cuadroSIGOPLAN, cuadro.porcent_dcto1);
                typeof(tblCom_CuadroComparativo).GetProperty("porcent_iva" + posicionProveedor).SetValue(cuadroSIGOPLAN, cuadro.porcent_iva1);
                typeof(tblCom_CuadroComparativo).GetProperty("dcto" + posicionProveedor).SetValue(cuadroSIGOPLAN, cuadro.dcto1);
                typeof(tblCom_CuadroComparativo).GetProperty("iva" + posicionProveedor).SetValue(cuadroSIGOPLAN, cuadro.iva1);
                typeof(tblCom_CuadroComparativo).GetProperty("total" + posicionProveedor).SetValue(cuadroSIGOPLAN, cuadro.total1);
                typeof(tblCom_CuadroComparativo).GetProperty("tipo_cambio" + posicionProveedor).SetValue(cuadroSIGOPLAN, cuadro.tipo_cambio1);
                typeof(tblCom_CuadroComparativo).GetProperty("fecha_entrega" + posicionProveedor).SetValue(cuadroSIGOPLAN, cuadro.fecha_entrega1);
                typeof(tblCom_CuadroComparativo).GetProperty("lab" + posicionProveedor).SetValue(cuadroSIGOPLAN, 1);
                typeof(tblCom_CuadroComparativo).GetProperty("dias_pago" + posicionProveedor).SetValue(cuadroSIGOPLAN, 1);
                typeof(tblCom_CuadroComparativo).GetProperty("sub_total" + posicionProveedor).SetValue(cuadroSIGOPLAN, cuadro.sub_total1);
                typeof(tblCom_CuadroComparativo).GetProperty("fletes" + posicionProveedor).SetValue(cuadroSIGOPLAN, cuadro.fletes1);
                typeof(tblCom_CuadroComparativo).GetProperty("gastos_imp" + posicionProveedor).SetValue(cuadroSIGOPLAN, cuadro.gastos_imp1);
                typeof(tblCom_CuadroComparativo).GetProperty("nombre_prov" + posicionProveedor).SetValue(cuadroSIGOPLAN, cuadro.nombre_prov1 ?? "");
                typeof(tblCom_CuadroComparativo).GetProperty("moneda" + posicionProveedor).SetValue(cuadroSIGOPLAN, cuadro.moneda1 ?? 0);
                typeof(tblCom_CuadroComparativo).GetProperty("comentarios" + posicionProveedor).SetValue(cuadroSIGOPLAN, cuadro.comentarios1 ?? "");

                #region Guardar Archivo Cotización
                string rutaArchivo = "";

                if (archivo != null)
                {
                    rutaArchivo = Path.Combine(RutaArchivos, ObtenerFormatoNombreArchivo(cuadro.cc + "_" + cuadro.numero + "_" + folio + "_" + (int)cuadro.empresa, archivo.FileName));
                    Tuple<bool, Exception> guardarArchivo = GlobalUtils.SaveHTTPPostedFileValidacion(archivo, rutaArchivo);

                    if (!guardarArchivo.Item1)
                    {
                        LogError(0, 0, NombreControlador, "GuardarCuadroComparativo_GuardarArchivo", guardarArchivo.Item2, AccionEnum.AGREGAR, 0, cuadro);
                        throw new Exception("Ocurrió un error al guardar los archivos en el servidor.");
                    }
                }
                #endregion

                typeof(tblCom_CuadroComparativo).GetProperty("rutaArchivo" + posicionProveedor).SetValue(cuadroSIGOPLAN, (rutaArchivo != "" ? rutaArchivo : null));

                cuadroSIGOPLAN.fechaModificacion = DateTime.Now;
                _contextEmpresa.SaveChanges();

                //switch (posicionProveedor)
                //{
                //    case 1:
                //        cuadroSIGOPLAN.prov1 = cuadro.prov1 ?? 0;
                //        cuadroSIGOPLAN.porcent_dcto1 = cuadro.porcent_dcto1;
                //        cuadroSIGOPLAN.porcent_iva1 = cuadro.porcent_iva1;
                //        cuadroSIGOPLAN.dcto1 = cuadro.dcto1;
                //        cuadroSIGOPLAN.iva1 = cuadro.iva1;
                //        cuadroSIGOPLAN.total1 = cuadro.total1;
                //        cuadroSIGOPLAN.tipo_cambio1 = cuadro.tipo_cambio1;
                //        cuadroSIGOPLAN.fecha_entrega1 = cuadro.fecha_entrega1;
                //        cuadroSIGOPLAN.lab1 = 1;
                //        cuadroSIGOPLAN.dias_pago1 = 1;
                //        cuadroSIGOPLAN.sub_total1 = cuadro.sub_total1;
                //        cuadroSIGOPLAN.fletes1 = cuadro.fletes1;
                //        cuadroSIGOPLAN.gastos_imp1 = cuadro.gastos_imp1;
                //        cuadroSIGOPLAN.nombre_prov1 = cuadro.nombre_prov1 ?? "";
                //        cuadroSIGOPLAN.moneda1 = cuadro.moneda1 ?? 0;
                //        cuadroSIGOPLAN.comentarios1 = cuadro.comentarios1 ?? "";
                //        cuadroSIGOPLAN.fechaModificacion = DateTime.Now;
                //        _contextEmpresa.SaveChanges();
                //        break;
                //    case 2:
                //        cuadroSIGOPLAN.prov2 = cuadro.prov1 ?? 0;
                //        cuadroSIGOPLAN.porcent_dcto2 = cuadro.porcent_dcto1;
                //        cuadroSIGOPLAN.porcent_iva2 = cuadro.porcent_iva1;
                //        cuadroSIGOPLAN.dcto2 = cuadro.dcto1;
                //        cuadroSIGOPLAN.iva2 = cuadro.iva1;
                //        cuadroSIGOPLAN.total2 = cuadro.total1;
                //        cuadroSIGOPLAN.tipo_cambio2 = cuadro.tipo_cambio1;
                //        cuadroSIGOPLAN.fecha_entrega2 = cuadro.fecha_entrega1;
                //        cuadroSIGOPLAN.lab2 = 1;
                //        cuadroSIGOPLAN.dias_pago2 = 1;
                //        cuadroSIGOPLAN.sub_total2 = cuadro.sub_total1;
                //        cuadroSIGOPLAN.fletes2 = cuadro.fletes1;
                //        cuadroSIGOPLAN.gastos_imp2 = cuadro.gastos_imp1;
                //        cuadroSIGOPLAN.nombre_prov2 = cuadro.nombre_prov1 ?? "";
                //        cuadroSIGOPLAN.moneda2 = cuadro.moneda1 ?? 0;
                //        cuadroSIGOPLAN.comentarios2 = cuadro.comentarios1 ?? "";
                //        cuadroSIGOPLAN.fechaModificacion = DateTime.Now;
                //        _contextEmpresa.SaveChanges();
                //        break;
                //    case 3:
                //        cuadroSIGOPLAN.prov3 = cuadro.prov1 ?? 0;
                //        cuadroSIGOPLAN.porcent_dcto3 = cuadro.porcent_dcto1;
                //        cuadroSIGOPLAN.porcent_iva3 = cuadro.porcent_iva1;
                //        cuadroSIGOPLAN.dcto3 = cuadro.dcto1;
                //        cuadroSIGOPLAN.iva3 = cuadro.iva1;
                //        cuadroSIGOPLAN.total3 = cuadro.total1;
                //        cuadroSIGOPLAN.tipo_cambio3 = cuadro.tipo_cambio1;
                //        cuadroSIGOPLAN.fecha_entrega3 = cuadro.fecha_entrega1;
                //        cuadroSIGOPLAN.lab3 = 1;
                //        cuadroSIGOPLAN.dias_pago3 = 1;
                //        cuadroSIGOPLAN.sub_total3 = cuadro.sub_total1;
                //        cuadroSIGOPLAN.fletes3 = cuadro.fletes1;
                //        cuadroSIGOPLAN.gastos_imp3 = cuadro.gastos_imp1;
                //        cuadroSIGOPLAN.nombre_prov3 = cuadro.nombre_prov1 ?? "";
                //        cuadroSIGOPLAN.moneda3 = cuadro.moneda1 ?? 0;
                //        cuadroSIGOPLAN.comentarios3 = cuadro.comentarios1 ?? "";
                //        cuadroSIGOPLAN.fechaModificacion = DateTime.Now;
                //        _contextEmpresa.SaveChanges();
                //        break;
                //}
            }
            #endregion

            #region Detalle
            foreach (var detalle in cuadro.detalleCuadro)
            {
                var cuadroDetalleSIGOPLAN = _contextEmpresa.tblCom_CuadroComparativoDet.FirstOrDefault(x => x.registroActivo && x.cc == cuadro.cc && x.numero == cuadro.numero && x.folio == folio && x.partida == detalle.partida);

                if (cuadroDetalleSIGOPLAN != null)
                {
                    typeof(tblCom_CuadroComparativoDet).GetProperty("precio" + posicionProveedor).SetValue(cuadroDetalleSIGOPLAN, detalle.precio1);

                    //switch (posicionProveedor)
                    //{
                    //    case 1:
                    //        cuadroDetalleSIGOPLAN.precio1 = detalle.precio1;
                    //        break;
                    //    case 2:
                    //        cuadroDetalleSIGOPLAN.precio2 = detalle.precio1;
                    //        break;
                    //    case 3:
                    //        cuadroDetalleSIGOPLAN.precio3 = detalle.precio1;
                    //        break;
                    //}

                    cuadroDetalleSIGOPLAN.fechaModificacion = DateTime.Now;
                    _contextEmpresa.SaveChanges();
                }
            }
            #endregion
            #endregion

            #region Enkontrol
            #region General
            var count = 0;

            using (var cmd = new OdbcCommand(string.Format(@"
                        UPDATE so_cuadro_comparativo
                        SET prov{0} = ?, porcent_dcto{0} = ?, porcent_iva{0} = ?, dcto{0} = ?, iva{0} = ?, total{0} = ?, tipo_cambio{0} = ?, fecha_entrega{0} = ?, lab{0} = ?, dias_pago{0} = ?, sub_total{0} = ?, fletes{0} = ?,
                            gastos_imp{0} = ?, nombre_prov{0} = ?, moneda{0} = ?, comentarios{0} = ?
                        WHERE cc = ? AND numero = ? AND folio = ?", posicionProveedor)))
            {
                OdbcParameterCollection parameters = cmd.Parameters;

                parameters.Add("@prov" + posicionProveedor, OdbcType.Numeric).Value = cuadro.prov1 ?? (object)DBNull.Value;
                parameters.Add("@porcent_dcto" + posicionProveedor, OdbcType.Numeric).Value = cuadro.porcent_dcto1;
                parameters.Add("@porcent_iva" + posicionProveedor, OdbcType.Numeric).Value = cuadro.porcent_iva1;
                parameters.Add("@dcto" + posicionProveedor, OdbcType.Numeric).Value = cuadro.dcto1;
                parameters.Add("@iva" + posicionProveedor, OdbcType.Numeric).Value = cuadro.iva1;
                parameters.Add("@total" + posicionProveedor, OdbcType.Numeric).Value = cuadro.total1;
                parameters.Add("@tipo_cambio" + posicionProveedor, OdbcType.Numeric).Value = cuadro.tipo_cambio1;
                parameters.Add("@fecha_entrega" + posicionProveedor, OdbcType.Date).Value = cuadro.fecha_entrega1;
                parameters.Add("@lab" + posicionProveedor, OdbcType.Numeric).Value = 1;
                parameters.Add("@dias_pago" + posicionProveedor, OdbcType.Numeric).Value = 1;
                parameters.Add("@sub_total" + posicionProveedor, OdbcType.Numeric).Value = cuadro.sub_total1;
                parameters.Add("@fletes" + posicionProveedor, OdbcType.Numeric).Value = cuadro.fletes1;
                parameters.Add("@gastos_imp" + posicionProveedor, OdbcType.Numeric).Value = cuadro.gastos_imp1;
                parameters.Add("@nombre_prov" + posicionProveedor, OdbcType.VarChar).Value = cuadro.nombre_prov1 ?? (object)DBNull.Value;
                parameters.Add("@moneda" + posicionProveedor, OdbcType.Char).Value = cuadro.moneda1 ?? (object)DBNull.Value;
                parameters.Add("@comentarios" + posicionProveedor, OdbcType.Char).Value = cuadro.comentarios1 ?? (object)DBNull.Value;

                parameters.Add("@cc", OdbcType.Char).Value = cuadro.cc;
                parameters.Add("@numero", OdbcType.Numeric).Value = cuadro.numero;
                parameters.Add("@folio", OdbcType.Numeric).Value = folio;

                cmd.Connection = trans.Connection;
                cmd.Transaction = trans;
                count += cmd.ExecuteNonQuery();
            }
            #endregion

            #region Detalle
            foreach (var det in cuadro.detalleCuadro)
            {
                using (var cmd = new OdbcCommand(string.Format(@"
                            UPDATE so_cuadro_comparativo_det
                            SET precio{0} = ?
                            WHERE cc = ? AND numero = ? AND folio = ? AND partida = ?", posicionProveedor)))
                {
                    OdbcParameterCollection parameters = cmd.Parameters;

                    parameters.Add("@precio" + posicionProveedor, OdbcType.Char).Value = det.precio1;

                    parameters.Add("@cc", OdbcType.Char).Value = cuadro.cc;
                    parameters.Add("@numero", OdbcType.Numeric).Value = cuadro.numero;
                    parameters.Add("@folio", OdbcType.Numeric).Value = folio;
                    parameters.Add("@partida", OdbcType.Numeric).Value = det.partida;

                    cmd.Connection = trans.Connection;
                    cmd.Transaction = trans;
                    count += cmd.ExecuteNonQuery();
                }
            }
            #endregion
            #endregion
        }

        private EnkontrolAmbienteEnum getEnkontrolAmbienteConsulta(EmpresaEnum empresa)
        {
            if (productivo)
            {
                if (empresa == EmpresaEnum.Construplan)
                {
                    return EnkontrolAmbienteEnum.ProdCPLAN;
                }
                else
                {
                    return EnkontrolAmbienteEnum.ProdARREND;
                }
            }
            else
            {
                if (empresa == EmpresaEnum.Construplan)
                {
                    return EnkontrolAmbienteEnum.PruebaCPLAN;
                }
                else
                {
                    return EnkontrolAmbienteEnum.PruebaARREND;
                }
            }
        }

        private OdbcConnection checkConexionProductivo(EmpresaEnum empresa)
        {
            if (productivo)
            {
                return new Conexion().Connect((int)empresa);
            }
            else
            {
                return new Conexion().ConnectPrueba();
            }
        }

        private string ObtenerFormatoNombreArchivo(string nombreBase, string fileName)
        {
            return String.Format("{0} {1}{2}", nombreBase, DateTime.Now.ToString("dd-MM-y HH:mm:ss").Replace(":", "-"), Path.GetExtension(fileName));
        }

        public void SaveBitacoraProveedor(int modulo, int accion, int registroID, string objeto, int? numeroProveedor, MainContext _contextEmpresa)
        {
            var bitacora = new tblP_Bitacora();

            bitacora.modulo = modulo;
            bitacora.accion = accion;
            bitacora.usuarioID = numeroProveedor ?? 0;
            bitacora.fecha = DateTime.Now;
            bitacora.registroID = registroID;
            bitacora.objeto = objeto;
            bitacora.publicIP = "";
            bitacora.localIP = "";

            try
            {
                string publicIP = new WebClient().DownloadString("http://icanhazip.com");

                bitacora.publicIP = publicIP;
            }
            catch (Exception e)
            {
                LogError(0, 0, "Generic", "SaveBitacora (publicIP)", e, AccionEnum.CONSULTA, 0, new { bitacora = bitacora });
            }

            try
            {
                string localIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (string.IsNullOrEmpty(localIP))
                {
                    localIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }

                bitacora.localIP = localIP;
            }
            catch (Exception e)
            {
                LogError(0, 0, "Generic", "SaveBitacora (localIP)", e, AccionEnum.CONSULTA, 0, new { bitacora = bitacora });
            }

            _contextEmpresa.tblP_Bitacora.Add(bitacora);
            _contextEmpresa.SaveChanges();
        }
    }
}
