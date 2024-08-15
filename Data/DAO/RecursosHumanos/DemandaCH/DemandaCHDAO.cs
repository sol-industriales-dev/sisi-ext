using Core.DAO.RecursosHumanos.Demandas;
using Core.DTO;
using Core.DTO.Administracion.Seguridad.Indicadores;
using Core.DTO.Principal.Generales;
using Core.DTO.Principal.GraficasHighcharts;
using Core.DTO.RecursosHumanos.Demandas;
using Core.DTO.Utils.Data;
using Core.Entity.Administrativo.Contabilidad.Nomina;
using Core.Entity.Administrativo.RecursosHumanos.Enkontrol;
using Core.Entity.Administrativo.RecursosHumanos.Reclutamientos;
using Core.Entity.Principal.Multiempresa;
using Core.Entity.Principal.Usuarios;
using Core.Entity.RecursosHumanos.Bajas;
using Core.Entity.RecursosHumanos.Demandas;
using Core.Enum;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Core.Enum.Principal.Bitacoras;
using Core.Enum.RecursosHumanos.Demandas;
using Core.Enum.RecursosHumanos.Reclutamientos;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Data.DAO.RecursosHumanos.Demandas
{
    public class DemandaCHDAO : GenericDAO<tblP_Usuario>, IDemandaCHDAO
    {
        #region INIT
        private const string _RUTA_BASE = @"\\REPOSITORIO\Proyecto\SIGOPLAN\CAPITAL_HUMANO\DEMANDAS\DOCUMENTOS_LEGALES";
        private const string _RUTA_LOCAL = @"C:\Proyecto\SIGOPLAN\CAPITAL_HUMANO\DEMANDAS\DOCUMENTOS_LEGALES";
        private readonly string _RUTA_DOCUMENTOS_LEGALES = string.Empty;

        private Dictionary<string, object> resultado = new Dictionary<string, object>();
        private const string _NOMBRE_CONTROLADOR = "DemandaCHController";
        private const int _SISTEMA = (int)SistemasEnum.RH;

        public DemandaCHDAO()
        {
#if DEBUG
            _RUTA_DOCUMENTOS_LEGALES = _RUTA_LOCAL;
#else
            _RUTA_DOCUMENTOS_LEGALES = _RUTA_BASE;
#endif
        }
        #endregion

        #region CAPTURAS
        public Dictionary<string, object> GetCapturas(CapturaDTO objDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE LISTADO DE CAPTURAS
                MainContextEnum objEmpresa = GetEmpresaLogueada();
                List<CapturaDTO> lstCapturas = _context.Select<CapturaDTO>(new DapperDTO
                {
                    baseDatos = objEmpresa,
                    consulta = @"SELECT id, claveEmpleado, nombreDemandante, esEmpleadoCP, numExpediente, esDemandaCerrada 
                                        FROM tblRH_DMS_CapturaDemandas 
                                            WHERE registroActivo = @registroActivo
                                                ORDER BY FechaCreacion",
                    parametros = new { registroActivo = true }
                }).ToList();

                foreach (var item in lstCapturas)
                {
                    #region SE OBTIENE EL ESTATUS DE LA DEMANDA
                    switch (item.esDemandaCerrada)
                    {
                        case (int)EstatusDemandaEnum.ABIERTO:
                            item.strDemandaCerrada = EnumHelper.GetDescription((EstatusDemandaEnum)item.esDemandaCerrada);
                            break;
                        case (int)EstatusDemandaEnum.CERRADO:
                            item.strDemandaCerrada = EnumHelper.GetDescription((EstatusDemandaEnum)item.esDemandaCerrada);
                            break;
                    }
                    #endregion
                }

                resultado.Add(SUCCESS, true);
                resultado.Add("lstCapturas", lstCapturas);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetCapturas", e, AccionEnum.CONSULTA, objDTO.id, objDTO);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> GetInformacionEmpleado(CapturaDTO objDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                string MensajeError = "Ocurrió un error al obtener la información del empleado.";
                if (objDTO.claveEmpleado <= 0) { throw new Exception(MensajeError); }
                #endregion

                #region SE OBTIENE LA INFORMACIÓN DEL EMPLEADO

                // SE OBTIENE LA INFORMACIÓN GENERAL DEL EMPLEADO
                MainContextEnum objEmpresa = GetEmpresaLogueada();
                CapturaDTO objEmpleado = _context.Select<CapturaDTO>(new DapperDTO
                {
                    baseDatos = objEmpresa,
                    consulta = @"SELECT TOP(1) t1.*, t2.*
	                                    FROM tblRH_EK_Empleados AS t1
	                                    INNER JOIN tblRH_EK_Tabulador_Historial AS t2 ON t2.clave_empleado = t1.clave_empleado
		                                    WHERE t2.clave_empleado = @clave_empleado
			                                    ORDER BY fecha_cambio DESC",
                    parametros = new { clave_empleado = objDTO.claveEmpleado }
                }).FirstOrDefault();
                if (objEmpleado == null)
                    throw new Exception("No se encuentra al empleado en base a la clave proporcionada.");

                if (string.IsNullOrEmpty(objEmpleado.puesto))
                    throw new Exception("El empleado no cuenta con un puesto registrado en el sistema.");

                // SE OBTIENE LA DESCRIPCIÓN DE SU PUESTO
                int idPuesto = Convert.ToInt32(objEmpleado.puesto);
                tblRH_EK_Puestos objPuesto = _context.tblRH_EK_Puestos.Where(w => w.puesto == idPuesto).FirstOrDefault();
                if (objPuesto == null)
                    throw new Exception("No se encuentra la descripción del puesto en base a la clave proporcionada.");

                // SE OBTIENE LA FECHA DE BAJA
                tblRH_EK_Empl_Baja objFechaBaja = _context.tblRH_EK_Empl_Baja.Where(w => w.clave_empleado == objDTO.claveEmpleado).OrderByDescending(o => o.id).FirstOrDefault();
                if (objFechaBaja == null)
                    throw new Exception("No se encuentra la fecha de baja en base a la clave proporcionada.");

                if (objFechaBaja.motivo_baja <= 0)
                    throw new Exception("El empleado no cuenta con el motivo de baja registrado en el sistema.");

                // SE OBTIENE EL MOTIVO DE BAJA
                tblRH_EK_Razones_Baja objMotivoBaja = _context.tblRH_EK_Razones_Baja.Where(w => w.clave_razon_baja == objFechaBaja.motivo_baja).FirstOrDefault();

                CapturaDTO objEmpleadoDTO = new CapturaDTO();
                objEmpleadoDTO.nombre = objEmpleado.nombre;
                objEmpleadoDTO.ape_paterno = objEmpleado.ape_paterno;
                objEmpleadoDTO.ape_materno = objEmpleado.ape_materno;
                objEmpleadoDTO.cc = objEmpleado.cc_contable;
                objEmpleadoDTO.fechaIngreso = objEmpleado.fecha_alta;
                objEmpleadoDTO.salarioBase = Convert.ToDecimal(objEmpleado.salario_base);
                objEmpleadoDTO.complemento = Convert.ToDecimal(objEmpleado.complemento);
                objEmpleadoDTO.tipoNomina = Convert.ToInt32(objEmpleado.tipo_nomina);
                objEmpleadoDTO.estatus_empleado = objEmpleado.estatus_empleado;
                objEmpleadoDTO.puesto = objPuesto.descripcion;
                objEmpleadoDTO.fechaBaja = objFechaBaja.fecha_baja;
                objEmpleadoDTO.motivoBaja = objMotivoBaja.desc_motivo_baja;
                objEmpleadoDTO.esEmpleadoCP = true;
                objEmpleadoDTO.claveEmpleado = objEmpleado.claveEmpleado;
                objEmpleadoDTO.bono_zona = objEmpleado.bono_zona;

                // SE FORMATEA EL NOMBRE COMPLETO DEL EMPLEADO
                objEmpleadoDTO.nombreDemandante = SetNombreCompleto(objEmpleadoDTO.nombre, objEmpleadoDTO.ape_paterno, objEmpleadoDTO.ape_materno);
                if (string.IsNullOrEmpty(objEmpleadoDTO.nombreDemandante)) throw new Exception("Ocurrió un error al obtener el nombre de la persona.");

                // SE OBTIENE EL SUELDO DIARIO
                if (objEmpleadoDTO.fechaBaja != null)
                    objEmpleadoDTO.sueldoDiario = SetSueldoDiario(objEmpleadoDTO.salarioBase, objEmpleadoDTO.complemento, objEmpleadoDTO.bono_zona, objEmpleadoDTO.fechaBaja, objEmpleadoDTO.tipoNomina);

                // SE OBTIENE LA ANTIGUEDAD EN FORMATO: AÑOS, MESES, DÍAS
                if (objEmpleadoDTO.fechaIngreso != null && objEmpleadoDTO.fechaBaja != null)
                {
                    objEmpleadoDTO.antiguedad = SetAntiguedad(objEmpleadoDTO.fechaIngreso, objEmpleadoDTO.fechaBaja);
                    if (string.IsNullOrEmpty(objEmpleadoDTO.antiguedad)) throw new Exception("Ocurrió un error al obtener la antiguedad de la persona.");
                }

                // SE OBTIENE EL PORCENTAJE DE EXPEDIENTE DEL EMPLEADO
                objEmpleadoDTO.porcentajeExpediente = SetPorcentajeExpediente(objEmpleadoDTO.claveEmpleado);

                // SE OBTIENE EL PORCENTAJE DEL FINIQUITO
                CapturaDTO objPorcentajeFiniquito = SetPorcentajeFiniquito(objEmpleadoDTO.claveEmpleado);
                objEmpleadoDTO.porcentajeFiniquito = objPorcentajeFiniquito.porcentajeFiniquito;
                objEmpleadoDTO.colorPorcentajeFiniquito = objPorcentajeFiniquito.colorPorcentajeFiniquito;

                resultado.Add(SUCCESS, true);
                resultado.Add("objEmpleado", objEmpleadoDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetInformacionEmpleado", e, AccionEnum.CONSULTA, objDTO.id, objDTO);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> CECaptura(CapturaDTO objDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    if (string.IsNullOrEmpty(objDTO.nombreDemandante)) { throw new Exception("Es necesario indicar el nombre del demandante."); }
                    if (objDTO.fechaRecibioDemanda == null) { throw new Exception("Es necesario indicar la fecha de cuando recibió la demanda."); }
                    if (objDTO.fechaDemanda == null) { throw new Exception("Es necesario indicar la fecha de la demanda."); }
                    if (objDTO.FK_Juzgado <= 0) { throw new Exception("Es necesario seleccionar el juzgado."); }
                    if (objDTO.FK_TipoDemanda <= 0) { throw new Exception("Es necesario seleccionar el tipo de demanda."); }
                    if (objDTO.FK_Estado <= 0) { throw new Exception("Es necesario indicar el estado."); }
                    if (string.IsNullOrEmpty(objDTO.demandado)) { throw new Exception("Es necesario indicar al demandado."); }
                    if (string.IsNullOrEmpty(objDTO.estadoActual)) { throw new Exception("Es necesario seleccionar el estado."); }
                    if (string.IsNullOrEmpty(objDTO.abogadoDemandante)) { throw new Exception("Es necesario indicar al abogado demandante."); }
                    if (objDTO.semaforo <= 0) { throw new Exception("Es necesario seleccionar el estatus del semaforo."); }
                    #endregion

                    tblRH_DMS_CapturaDemandas objCE = _context.tblRH_DMS_CapturaDemandas.Where(w => w.id == objDTO.id).FirstOrDefault();
                    if (objCE == null)
                    {
                        #region NUEVO REGISTRO
                        objCE = new tblRH_DMS_CapturaDemandas();
                        objCE.claveEmpleado = objDTO.esEmpleadoCP ? objDTO.claveEmpleado : 0;
                        objCE.nombreDemandante = objDTO.nombreDemandante;
                        objCE.puesto = objDTO.puesto;
                        objCE.esEmpleadoCP = objDTO.esEmpleadoCP;
                        objCE.cc = objDTO.cc;
                        objCE.cc_Descripcion = !objDTO.ccLibre ? GetDescripcionCC(objDTO.cc) : string.Empty;
                        objCE.fechaIngreso = objDTO.fechaIngreso;
                        objCE.fechaBaja = objDTO.fechaBaja;
                        objCE.motivoSalida = objDTO.motivoSalida;
                        objCE.sueldoDiario = objDTO.sueldoDiario;
                        objCE.ofertaInicial = objDTO.ofertaInicial;
                        objCE.antiguedad = objDTO.antiguedad;
                        objCE.fechaRecibioDemanda = objDTO.fechaRecibioDemanda;
                        objCE.fechaDemanda = objDTO.fechaDemanda;
                        objCE.numExpediente = objDTO.numExpediente;
                        objCE.FK_Juzgado = objDTO.FK_Juzgado;
                        objCE.FK_TipoDemanda = objDTO.FK_TipoDemanda;
                        objCE.FK_Estado = objDTO.FK_Estado;
                        objCE.demandado = objDTO.demandado;
                        objCE.salarioDiario = objDTO.salarioDiario;
                        objCE.hechos = !string.IsNullOrEmpty(objDTO.hechos) ? objDTO.hechos.Trim() : null;
                        objCE.peticiones = !string.IsNullOrEmpty(objDTO.peticiones) ? objDTO.peticiones.Trim() : null;
                        objCE.estadoActual = objDTO.estadoActual;
                        objCE.fechaAudiencia = objDTO.fechaAudiencia;
                        objCE.comentarioFechaAudiencia = objDTO.comentarioFechaAudiencia;
                        objCE.abogadoDemandante = objDTO.abogadoDemandante;
                        objCE.cuantiaTotal = objDTO.cuantiaTotal;
                        objCE.negociadoCerrado = objDTO.negociadoCerrado;
                        objCE.finiquitoAl100 = objDTO.finiquitoAl100;
                        objCE.diferencia = objDTO.diferencia;
                        objCE.semaforo = objDTO.semaforo;
                        objCE.esDemandaCerrada = (int)EstatusDemandaEnum.ABIERTO;
                        objCE.resolucionLaudo = !string.IsNullOrEmpty(objDTO.resolucionLaudo) ? objDTO.resolucionLaudo.Trim() : null;
                        objCE.FK_UsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCE.fechaCreacion = DateTime.Now;
                        objCE.registroActivo = true;
                        _context.tblRH_DMS_CapturaDemandas.Add(objCE);
                        _context.SaveChanges();
                        #endregion
                    }
                    else
                    {
                        #region ACTUALIZAR REGISTRO
                        objCE.claveEmpleado = objDTO.esEmpleadoCP ? objDTO.claveEmpleado : 0;
                        objCE.nombreDemandante = objDTO.nombreDemandante;
                        objCE.puesto = objDTO.puesto;
                        objCE.esEmpleadoCP = objDTO.esEmpleadoCP;
                        objCE.cc = objDTO.cc;
                        objCE.cc_Descripcion = !objDTO.ccLibre ? GetDescripcionCC(objDTO.cc) : string.Empty;
                        objCE.fechaIngreso = objDTO.fechaIngreso;
                        objCE.fechaBaja = objDTO.fechaBaja;
                        objCE.motivoSalida = objDTO.motivoSalida;
                        objCE.sueldoDiario = objDTO.sueldoDiario;
                        objCE.ofertaInicial = objDTO.ofertaInicial;
                        objCE.antiguedad = objDTO.antiguedad;
                        objCE.fechaRecibioDemanda = objDTO.fechaRecibioDemanda;
                        objCE.fechaDemanda = objDTO.fechaDemanda;
                        objCE.numExpediente = objDTO.numExpediente;
                        objCE.FK_Juzgado = objDTO.FK_Juzgado;
                        objCE.FK_TipoDemanda = objDTO.FK_TipoDemanda;
                        objCE.FK_Estado = objDTO.FK_Estado;
                        objCE.demandado = objDTO.demandado;
                        objCE.salarioDiario = objDTO.salarioDiario;
                        objCE.hechos = !string.IsNullOrEmpty(objDTO.hechos) ? objDTO.hechos.Trim() : null;
                        objCE.peticiones = !string.IsNullOrEmpty(objDTO.peticiones) ? objDTO.peticiones.Trim() : null;
                        objCE.estadoActual = objDTO.estadoActual;
                        objCE.fechaAudiencia = objDTO.fechaAudiencia;
                        objCE.comentarioFechaAudiencia = objDTO.comentarioFechaAudiencia;
                        objCE.abogadoDemandante = objDTO.abogadoDemandante;
                        objCE.cuantiaTotal = objDTO.cuantiaTotal;
                        objCE.negociadoCerrado = objDTO.negociadoCerrado;
                        objCE.finiquitoAl100 = objDTO.finiquitoAl100;
                        objCE.diferencia = objDTO.diferencia;
                        objCE.semaforo = objDTO.semaforo;
                        objCE.resolucionLaudo = !string.IsNullOrEmpty(objDTO.resolucionLaudo) ? objDTO.resolucionLaudo.Trim() : null;
                        objCE.FK_UsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCE.fechaModificacion = DateTime.Now;
                        _context.SaveChanges();
                        #endregion
                    }

                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, objDTO.id <= 0 ? "Se ha registrado con éxito." : "Se ha actualizado con éxito");

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, objDTO.id <= 0 ? (int)AccionEnum.AGREGAR : (int)AccionEnum.ACTUALIZAR, objDTO.id, JsonUtils.convertNetObjectToJson(objDTO));
                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "CECaptura", e, objDTO.id <= 0 ? AccionEnum.AGREGAR : AccionEnum.ACTUALIZAR, objDTO.id, objDTO);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> EliminarCaptura(CapturaDTO objDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    string MensajeError = "Ocurrió un error al eliminar el registro.";
                    if (objDTO.id <= 0) { throw new Exception(MensajeError); }
                    #endregion

                    #region SE ELIMINA LA DEMANDA SELECCIONADA

                    #region SE ELIMINA LOS DOCUMENTOS CARGADOS A LA DEMANDA
                    List<tblRH_DMS_ArchivosAdjuntos> lstArchivos = _context.tblRH_DMS_ArchivosAdjuntos.Where(w => w.FK_Captura == objDTO.id && w.registroActivo).ToList();
                    if (lstArchivos.Count() > 0)
                    {
                        foreach (var item in lstArchivos)
                        {
                            tblRH_DMS_ArchivosAdjuntos objArchivo = lstArchivos.Where(w => w.id == item.id).FirstOrDefault();
                            objArchivo.FK_UsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                            objArchivo.fechaModificacion = DateTime.Now;
                            objArchivo.registroActivo = false;
                            _context.SaveChanges();
                        }
                    }
                    #endregion

                    #region SE ELIMINA LOS SEGUIMIENTOS DE LA DEMANDA
                    List<tblRH_DMS_SeguimientoDemanda> lstSeguimientos = _context.tblRH_DMS_SeguimientoDemanda.Where(w => w.FK_Demanda == objDTO.id && w.registroActivo).ToList();
                    if (lstSeguimientos.Count() > 0)
                    {
                        foreach (var item in lstSeguimientos)
                        {
                            tblRH_DMS_SeguimientoDemanda objSeguimiento = lstSeguimientos.Where(w => w.id == item.id).FirstOrDefault();
                            objSeguimiento.FK_UsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                            objSeguimiento.fechaModificacion = DateTime.Now;
                            objSeguimiento.registroActivo = false;
                            _context.SaveChanges();
                        }
                    }
                    #endregion

                    tblRH_DMS_CapturaDemandas objEliminar = _context.tblRH_DMS_CapturaDemandas.Where(w => w.id == objDTO.id).FirstOrDefault();
                    if (objEliminar == null)
                        throw new Exception(MensajeError);

                    objEliminar.FK_UsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                    objEliminar.fechaModificacion = DateTime.Now;
                    objEliminar.registroActivo = false;
                    _context.SaveChanges();
                    #endregion

                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha eliminado con éxito.");
                    dbContextTransaction.Commit();

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, (int)AccionEnum.ELIMINAR, objDTO.id, JsonUtils.convertNetObjectToJson(objDTO));
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "EliminarCaptura", e, AccionEnum.ELIMINAR, objDTO.id, objDTO);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboSemaforo()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region FILL CBO SEMAFORO
                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();

                // VERDE
                int ColorSemaforo = (int)SemaforoEnum.VERDE;
                objComboDTO = new ComboDTO();
                objComboDTO.Value = ColorSemaforo.ToString();
                objComboDTO.Text = EnumHelper.GetDescription((SemaforoEnum.VERDE));
                lstComboDTO.Add(objComboDTO);

                // AMBAR
                ColorSemaforo = (int)SemaforoEnum.AMBAR;
                objComboDTO = new ComboDTO();
                objComboDTO.Value = ColorSemaforo.ToString();
                objComboDTO.Text = EnumHelper.GetDescription((SemaforoEnum.AMBAR));
                lstComboDTO.Add(objComboDTO);

                // ROJO
                ColorSemaforo = (int)SemaforoEnum.ROJO;
                objComboDTO = new ComboDTO();
                objComboDTO.Value = ColorSemaforo.ToString();
                objComboDTO.Text = EnumHelper.GetDescription((SemaforoEnum.ROJO));
                lstComboDTO.Add(objComboDTO);

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "FillCboSemaforo", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> GetDatosActualizarCaptura(CapturaDTO objDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                string MensajeError = "Ocurrió un error al obtener la información de la captura.";
                if (objDTO.id <= 0) throw new Exception(MensajeError);
                #endregion

                #region SE OBTIENE LA INFORMACIÓN DEL REGISTRO SELECCIONADO
                tblRH_DMS_CapturaDemandas objCaptura = _context.tblRH_DMS_CapturaDemandas.Where(w => w.id == objDTO.id && w.registroActivo).FirstOrDefault();
                if (objCaptura == null)
                    throw new Exception(MensajeError);

                CapturaDTO objCapturaDTO = new CapturaDTO();
                objCapturaDTO.id = objCaptura.id;
                objCapturaDTO.claveEmpleado = objCaptura.claveEmpleado;
                objCapturaDTO.nombreDemandante = objCaptura.nombreDemandante;
                objCapturaDTO.puesto = objCaptura.puesto;
                objCapturaDTO.esEmpleadoCP = objCaptura.esEmpleadoCP;
                objCapturaDTO.cc = SetFormatCC(objCaptura.cc, objCaptura.cc_Descripcion);
                objCapturaDTO.fechaIngreso = objCaptura.fechaIngreso;
                objCapturaDTO.fechaBaja = objCaptura.fechaBaja;
                objCapturaDTO.motivoSalida = objCaptura.motivoSalida;
                objCapturaDTO.sueldoDiario = objCaptura.salarioDiario;
                objCapturaDTO.antiguedad = objCaptura.antiguedad;
                objCapturaDTO.porcentajeExpediente = SetPorcentajeExpediente(objCapturaDTO.claveEmpleado);
                CapturaDTO objPorcentajeFiniquito = SetPorcentajeFiniquito(objCapturaDTO.claveEmpleado);
                objCapturaDTO.porcentajeFiniquito = objPorcentajeFiniquito.porcentajeFiniquito;
                objCapturaDTO.colorPorcentajeFiniquito = objPorcentajeFiniquito.colorPorcentajeFiniquito;
                objCapturaDTO.fechaRecibioDemanda = objCaptura.fechaRecibioDemanda;
                objCapturaDTO.fechaDemanda = objCaptura.fechaDemanda;
                objCapturaDTO.numExpediente = objCaptura.numExpediente;
                objCapturaDTO.FK_Juzgado = objCaptura.FK_Juzgado;
                objCapturaDTO.FK_TipoDemanda = objCaptura.FK_TipoDemanda;
                objCapturaDTO.FK_Estado = objCaptura.FK_Estado;
                objCapturaDTO.demandado = objCaptura.demandado;
                objCapturaDTO.salarioDiario = objCaptura.salarioDiario;
                objCapturaDTO.ofertaInicial = objCaptura.ofertaInicial;
                objCapturaDTO.hechos = objCaptura.hechos;
                objCapturaDTO.peticiones = objCaptura.peticiones;
                objCapturaDTO.estadoActual = objCaptura.estadoActual;
                objCapturaDTO.fechaAudiencia = objCaptura.fechaAudiencia;
                objCapturaDTO.comentarioFechaAudiencia = objCaptura.comentarioFechaAudiencia;
                objCapturaDTO.semaforo = objCaptura.semaforo;
                objCapturaDTO.abogadoDemandante = objCaptura.abogadoDemandante;
                objCapturaDTO.cuantiaTotal = objCaptura.cuantiaTotal;
                objCapturaDTO.negociadoCerrado = objCaptura.negociadoCerrado;
                objCapturaDTO.finiquitoAl100 = objCaptura.finiquitoAl100;
                objCapturaDTO.diferencia = objCaptura.diferencia;
                objCapturaDTO.cantDocumentosCargados = GetCantDocumentosCargadosRelDemanda(objCaptura.id);
                objCapturaDTO.resolucionLaudo = objCaptura.resolucionLaudo;

                resultado.Add(SUCCESS, true);
                resultado.Add("objCaptura", objCapturaDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetDatosActualizarCaptura", e, AccionEnum.CONSULTA, objDTO.id, new { objDTO = objDTO });
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboJuzgados()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region FILL CBO
                MainContextEnum objEmpresa = GetEmpresaLogueada();
                List<ComboDTO> lstJuzgados = _context.Select<ComboDTO>(new DapperDTO
                {
                    baseDatos = objEmpresa,
                    consulta = @"SELECT id AS VALUE, juzgado AS TEXT FROM tblRH_DMS_CatJuzgados WHERE registroActivo = @registroActivo ORDER BY TEXT",
                    parametros = new { registroActivo = true }
                }).ToList();

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstJuzgados);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "FillCboJuzgados", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboTipoDemandas()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region FILL CBO
                MainContextEnum objEmpresa = GetEmpresaLogueada();
                List<ComboDTO> lstTipoDemandas = _context.Select<ComboDTO>(new DapperDTO
                {
                    baseDatos = objEmpresa,
                    consulta = @"SELECT id AS VALUE, tipoDemanda AS TEXT FROM tblRH_DMS_CatTipoDemandas WHERE registroActivo = @registroActivo ORDER BY TEXT",
                    parametros = new { registroActivo = true }
                }).ToList();

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstTipoDemandas);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "FillCboTipoDemandas", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> CerrarDemanda(int idCaptura)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    string mensajeError = "Ocurrió un error al indicar como cerrado la demanda.";
                    if (idCaptura <= 0) { throw new Exception(mensajeError); }

                    #region SE VERIFICA SI LA DEMANDA CUENTA CON EL ARCHIVO "FINIQUITO NEGOCIADO CERRADO"
                    tblRH_DMS_ArchivosAdjuntos objArchivo = _context.tblRH_DMS_ArchivosAdjuntos
                                                                .Where(w => w.FK_Captura == idCaptura && w.tipoArchivo == (int)TipoArchivoDemandaEnum.FINIQUITO_NEGOCIADO_CERRADO && w.registroActivo).FirstOrDefault();
                    if (objArchivo == null)
                        throw new Exception("Es necesario capturar el archivo 'Finiquito negociado cerrado' para poder cambiar el estatus a cerrado.");
                    #endregion
                    #endregion

                    #region SE INDICA COMO CERRADA LA DEMANDA
                    tblRH_DMS_CapturaDemandas objDemanda = _context.tblRH_DMS_CapturaDemandas.Where(w => w.id == idCaptura && w.registroActivo).FirstOrDefault();
                    if (objDemanda == null)
                        throw new Exception(mensajeError);

                    objDemanda.esDemandaCerrada = (int)EstatusDemandaEnum.CERRADO;
                    objDemanda.fechaCierreDemanda = DateTime.Now;
                    objDemanda.FK_UsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                    objDemanda.fechaModificacion = DateTime.Now;
                    _context.SaveChanges();
                    #endregion

                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha indicado como cerrada la demanda.");
                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }

        private CapturaDTO SetPorcentajeFiniquito(int claveEmpleado)
        {
            CapturaDTO objCapturaDTO = new CapturaDTO();
            try
            {
                #region SE OBTIENE EL PORCENTAJE DEL FINIQUITO
                MainContextEnum objEmpresa = GetEmpresaLogueada();
                tblRH_Baja_Registro objFiniquitoEmpleado = _context.Select<tblRH_Baja_Registro>(new DapperDTO
                {
                    baseDatos = objEmpresa,
                    consulta = @"SELECT t1.est_inventario, t1.est_contabilidad, t1.est_compras
		                                FROM tblRH_Baja_Registro AS t1
		                                LEFT JOIN tblRH_Baja_Entrevista AS t2 ON t1.id = t2.registroID
		                                LEFT JOIN tblP_Usuario AS t3 on t3.id = t1.idUsuarioCreacion
			                                WHERE t1.registroActivo = @registroActivo AND t1.numeroEmpleado = @numeroEmpleado
				                                ORDER BY t1.fechaCreacion DESC",
                    parametros = new { registroActivo = true, numeroEmpleado = claveEmpleado }
                }).FirstOrDefault();
                if (objFiniquitoEmpleado != null)
                {
                    int cantLiberacion = 3;
                    int cantLiberacionEmpleado = 0;

                    if (objFiniquitoEmpleado.est_inventario == "A")
                        cantLiberacionEmpleado++;

                    if (objFiniquitoEmpleado.est_contabilidad == "A")
                        cantLiberacionEmpleado++;

                    if (objFiniquitoEmpleado.est_compras == "A")
                        cantLiberacionEmpleado++;

                    switch (cantLiberacionEmpleado)
                    {
                        case 0:
                            objCapturaDTO.colorPorcentajeFiniquito = "#d9534f";
                            break;
                        case 1:
                            objCapturaDTO.colorPorcentajeFiniquito = "#e88640";
                            break;
                        case 2:
                            objCapturaDTO.colorPorcentajeFiniquito = "#e88640";
                            break;
                        case 3:
                            objCapturaDTO.colorPorcentajeFiniquito = "#008000";
                            break;
                    }

                    objCapturaDTO.porcentajeFiniquito = ((decimal)cantLiberacionEmpleado / (decimal)cantLiberacion) * 100;
                    objCapturaDTO.est_inventario = objFiniquitoEmpleado.est_inventario.Trim().ToUpper();
                    objCapturaDTO.est_contabilidad = objFiniquitoEmpleado.est_contabilidad.Trim().ToUpper();
                    objCapturaDTO.est_compras = objFiniquitoEmpleado.est_compras.Trim().ToUpper();
                }
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "SetPorcentajeFiniquito", e, AccionEnum.CONSULTA, 0, new { claveEmpleado = claveEmpleado });
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return objCapturaDTO;
        }

        private int GetCantDocumentosCargadosRelDemanda(int FK_Captura) 
        {
            int cantDocumentosCargados = 0;
            try
            {
                #region VALIDACIONES
                if (FK_Captura <= 0) { throw new Exception("Ocurrió un error al obtener los documentos cargados de la demanda."); }
                #endregion

                #region SE OBTIENE LA CANTIDAD DE DOCUMENTOS CARGADOS EN LA DEMANDA
                List<tblRH_DMS_ArchivosAdjuntos> lstArchivos = _context.tblRH_DMS_ArchivosAdjuntos.Where(w => w.FK_Captura == FK_Captura && w.registroActivo).ToList();
                cantDocumentosCargados = lstArchivos.Count();
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetCantDocumentosCargadosRelDemanda", e, AccionEnum.CONSULTA, 0, new { FK_Captura = FK_Captura });
                return cantDocumentosCargados;
            }
            return cantDocumentosCargados;
        }

        private string SetFormatCC(string cc, string cc_Descripcion)
        {
            string formatCC = string.Empty;
            try
            {
                #region SE DA FORMATO AL CC
                if (!string.IsNullOrEmpty(cc) && !string.IsNullOrEmpty(cc_Descripcion))
                    formatCC = string.Format("[{0}] {1}", cc.Trim(), cc_Descripcion.Trim());
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "SetFormatCC", e, AccionEnum.CONSULTA, 0, new { cc = cc, cc_Descripcion = cc_Descripcion });
                return formatCC;
            }
            return formatCC;
        }

        #region ARCHIVOS ADJUNTOS
        public Dictionary<string, object> GuardarArchivoAdjunto(List<HttpPostedFileBase> lstArchivos, int FK_Captura, int tipoArchivo)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    #region VALIDACIONES
                    if (FK_Captura <= 0) { throw new Exception("Ocurrió un error al registrar el archivo."); }
                    if (tipoArchivo <= 0) { throw new Exception("Es necesario seleccionar el tipo de archivo."); }
                    #endregion

                    #region SE REGISTRA EL ARCHIVO ADJUNTO
                    var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();
                    var CarpetaNueva = Path.Combine(_RUTA_DOCUMENTOS_LEGALES, FK_Captura.ToString());
                    VerificarExisteCarpeta(CarpetaNueva, true);
                    foreach (var objArchivo in lstArchivos)
                    {
                        string nombreArchivo = SetNombreArchivo("Acto", objArchivo.FileName);
                        string rutaArchivo = Path.Combine(CarpetaNueva, nombreArchivo);
                        listaRutaArchivos.Add(Tuple.Create(objArchivo, rutaArchivo));

                        // GUARDAR TABLA ARCHIVOS
                        tblRH_DMS_ArchivosAdjuntos objEvidencia = new tblRH_DMS_ArchivosAdjuntos()
                        {
                            FK_Captura = FK_Captura,
                            archivo = nombreArchivo,
                            rutaArchivo = rutaArchivo,
                            tipoArchivo = tipoArchivo,
                            FK_UsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id,
                            fechaCreacion = DateTime.Now,
                            registroActivo = true
                        };
                        _context.tblRH_DMS_ArchivosAdjuntos.Add(objEvidencia);
                        _context.SaveChanges();
                    }

                    foreach (var objArchivo in listaRutaArchivos)
                    {
                        if (GlobalUtils.SaveHTTPPostedFile(objArchivo.Item1, objArchivo.Item2) == false)
                        {
                            dbContextTransaction.Rollback();
                            resultado.Clear();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                            return resultado;
                        }
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(MESSAGE, "Se ha registado con éxito.");
                    resultado.Add(SUCCESS, true);
                    #endregion
                }
                catch (Exception e)
                {
                    resultado.Add(SUCCESS, false);
                    dbContextTransaction.Rollback();
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, "DemandaCHController", "GuardarArchivoAdjunto", e, AccionEnum.AGREGAR, FK_Captura, new { lstArchivos = lstArchivos, FK_Captura = FK_Captura });
                }
            }
            return resultado;
        }

        public Dictionary<string, object> GetArchivosAdjuntos(int FK_Captura)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (FK_Captura <= 0) { throw new Exception("Ocurrió un error al obtener el listado de archivos adjuntos."); }
                #endregion

                #region SE OBTIENE LISTADO DE ARCHIVOS ADJUNTOS EN BASE AL ACTO SELECCIONADO
                List<ArchivosDemandasDTO> lstArchivos = _context.Select<ArchivosDemandasDTO>(new DapperDTO
                {
                    baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                    consulta = @"SELECT * FROM tblRH_DMS_ArchivosAdjuntos WHERE FK_Captura = @FK_Captura AND registroActivo = @registroActivo ORDER BY fechaCreacion",
                    parametros = new { FK_Captura = FK_Captura, registroActivo = true }
                }).ToList();

                foreach (var item in lstArchivos)
                {
                    if (item.tipoArchivo > 0)
                        item.strTipoArchivo = EnumHelper.GetDescription((TipoArchivoDemandaEnum)item.tipoArchivo);
                }

                resultado.Add(SUCCESS, true);
                resultado.Add("lstArchivos", lstArchivos);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetArchivosAdjuntos", e, AccionEnum.CONSULTA, 0, new { FK_Captura = FK_Captura });
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> VisualizarArchivoAdjunto(int idArchivo)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                tblRH_DMS_ArchivosAdjuntos objArchivoAdjunto = _context.tblRH_DMS_ArchivosAdjuntos.Where(w => w.id == idArchivo && w.registroActivo).FirstOrDefault();
                if (objArchivoAdjunto == null)
                    throw new Exception("Ocurrió un error al visualizar el archivo.");

                Stream fileStream = GlobalUtils.GetFileAsStream(objArchivoAdjunto.rutaArchivo);
                var byteArray = GlobalUtils.ConvertFileToByte(fileStream);

                resultado.Add("archivo", byteArray);
                resultado.Add("extension", Path.GetExtension(objArchivoAdjunto.rutaArchivo).ToUpper());
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "VisualizarArchivoAdjunto", e, AccionEnum.CONSULTA, idArchivo, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> EliminarArchivoAdjunto(int idArchivo)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (idArchivo <= 0) { throw new Exception("Ocurrió un error al eliminar el archivo."); }
                #endregion

                #region SE ELIMINA EL ARCHIVO
                tblRH_DMS_ArchivosAdjuntos objEliminar = _context.tblRH_DMS_ArchivosAdjuntos.Where(w => w.id == idArchivo && w.registroActivo).FirstOrDefault();
                if (objEliminar == null)
                    throw new Exception("Ocurrió un error al eliminar el archivo.");

                objEliminar.FK_UsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                objEliminar.fechaModificacion = DateTime.Now;
                objEliminar.registroActivo = false;
                _context.SaveChanges();
                #endregion

                resultado.Add(SUCCESS, true);
                resultado.Add(MESSAGE, "Se ha eliminado con éxito.");
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "EliminarArchivoAdjunto", e, AccionEnum.ELIMINAR, idArchivo, new { idArchivo = idArchivo });
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        private static bool VerificarExisteCarpeta(string path, bool crear = false)
        {
            bool existe = false;
            try
            {
                existe = Directory.Exists(path);
                if (!existe && crear)
                {
                    Directory.CreateDirectory(path);
                    existe = true;
                }
            }
            catch (Exception)
            {
                existe = false;
            }
            return existe;
        }

        private string SetNombreArchivo(string nombreBase, string fileName)
        {
            return String.Format("{0}{1}{2}", nombreBase, fileName.Split('.')[0], Path.GetExtension(fileName));
        }

        public Dictionary<string, object> FillCboTipoArchivos()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region FILL COMBO TIPO DE ARCHIVOS
                List<ComboDTO> lstComboDTO = new List<ComboDTO>();

                int tipoArchivo = (int)TipoArchivoDemandaEnum.DEMANDA_RECIBIDA;
                ComboDTO objComboDTO = new ComboDTO();
                objComboDTO.Value = tipoArchivo.ToString();
                objComboDTO.Text = EnumHelper.GetDescription((TipoArchivoDemandaEnum)tipoArchivo);
                lstComboDTO.Add(objComboDTO);

                tipoArchivo = (int)TipoArchivoDemandaEnum.FINIQUITO_NEGOCIADO_CERRADO;
                objComboDTO = new ComboDTO();
                objComboDTO.Value = tipoArchivo.ToString();
                objComboDTO.Text = EnumHelper.GetDescription((TipoArchivoDemandaEnum)tipoArchivo);
                lstComboDTO.Add(objComboDTO);

                tipoArchivo = (int)TipoArchivoDemandaEnum.DESISTIMIENTO_DE_DEMANDA;
                objComboDTO = new ComboDTO();
                objComboDTO.Value = tipoArchivo.ToString();
                objComboDTO.Text = EnumHelper.GetDescription((TipoArchivoDemandaEnum)tipoArchivo);
                lstComboDTO.Add(objComboDTO);

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "FillCboTipoArchivos", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Tuple<Stream, string> DescargarArchivo(int idArchivo)
        {
            try
            {
                tblRH_DMS_ArchivosAdjuntos objArchivo = _context.tblRH_DMS_ArchivosAdjuntos.Where(w => w.id == idArchivo && w.registroActivo).FirstOrDefault();
                if (objArchivo == null)
                    throw new Exception("No se encuentra el archivo a descargar.");

                var fileStream = GlobalUtils.GetFileAsStream(objArchivo.rutaArchivo);
                string nombreArchivo = Path.GetFileName(objArchivo.rutaArchivo);

                return Tuple.Create(fileStream, nombreArchivo);
            }
            catch (Exception e)
            {
                LogError(3, 0, _NOMBRE_CONTROLADOR, "DescargarArchivo", e, AccionEnum.CONSULTA, 0, new { idArchivo = idArchivo });
                return null;
            }
        }
        #endregion
        #endregion

        #region SEGUIMIENTOS
        public Dictionary<string, object> CrearSeguimiento(SeguimientoDTO objDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    if (objDTO.FK_Demanda <= 0) { throw new Exception("Ocurrió un error al registrar el seguimiento."); }
                    if (objDTO.cuantia <= 0) { throw new Exception("Es necesario indicar la cuantia."); }
                    if (string.IsNullOrEmpty(objDTO.abogadoDemandante)) { throw new Exception("Es necesario indicar el nombre del abogado demandante."); }
                    #endregion

                    #region SE REGISTRA EL NUEVO SEGUIMIENTO A LA DEMANDA
                    tblRH_DMS_SeguimientoDemanda objSeguimiento = new tblRH_DMS_SeguimientoDemanda();
                    objSeguimiento.FK_Demanda = objDTO.FK_Demanda;
                    objSeguimiento.cuantia = objDTO.cuantia;
                    objSeguimiento.abogadoDemandante = objDTO.abogadoDemandante.Trim();
                    objSeguimiento.fechaAudiencia = objDTO.fechaAudiencia;
                    objSeguimiento.semaforo = objDTO.semaforo;
                    objSeguimiento.estadoActual = objDTO.estadoActual;
                    objSeguimiento.FK_UsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                    objSeguimiento.fechaCreacion = DateTime.Now;
                    objSeguimiento.registroActivo = true;
                    _context.tblRH_DMS_SeguimientoDemanda.Add(objSeguimiento);
                    _context.SaveChanges();
                    #endregion

                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha registrado con éxito.");
                    dbContextTransaction.Commit();

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, (int)AccionEnum.AGREGAR, objDTO.id, JsonUtils.convertNetObjectToJson(objDTO));
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "CrearSeguimiento", e, AccionEnum.AGREGAR, objDTO.FK_Demanda, objDTO);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> GetEstatusFiniquitoEmpleadoDemanda(int claveEmpleado)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (claveEmpleado <= 0) { throw new Exception("Ocurrió un error al obtener el estatus del finiquito del ex empleado."); }
                #endregion

                #region SE OBTIENE EL ESTATUS DEL FINIQUITO
                MainContextEnum objEmpresa = GetEmpresaLogueada();
                tblRH_Baja_Registro objBaja = _context.Select<tblRH_Baja_Registro>(new DapperDTO
                {
                    baseDatos = objEmpresa,
                    consulta = @"SELECT t1.est_inventario, t1.est_contabilidad, t1.est_compras
		                                FROM tblRH_Baja_Registro AS t1
		                                LEFT JOIN tblRH_Baja_Entrevista AS t2 ON t1.id = t2.registroID
		                                LEFT JOIN tblP_Usuario AS t3 on t3.id = t1.idUsuarioCreacion
			                                WHERE t1.registroActivo = 1 AND t1.numeroEmpleado = 116
				                                ORDER BY t1.fechaCreacion DESC",
                    parametros = new { registroActivo = true }
                }).FirstOrDefault();
                if (objBaja == null)
                    throw new Exception("No se encuentra registro de baja del ex empleado.");

                List<CapturaDTO> lstEstatusFiniquito = new List<CapturaDTO>();
                CapturaDTO objEstatusFiniquito = new CapturaDTO();

                objEstatusFiniquito = new CapturaDTO();
                objEstatusFiniquito.descripcionFiniquito = "Inventario";
                objEstatusFiniquito.estatusFiniquito = objBaja.est_inventario == "A" ? true : false;
                lstEstatusFiniquito.Add(objEstatusFiniquito);

                objEstatusFiniquito = new CapturaDTO();
                objEstatusFiniquito.descripcionFiniquito = "Contabilidad";
                objEstatusFiniquito.estatusFiniquito = objBaja.est_contabilidad == "A" ? true : false;
                lstEstatusFiniquito.Add(objEstatusFiniquito);

                objEstatusFiniquito = new CapturaDTO();
                objEstatusFiniquito.descripcionFiniquito = "Compras";
                objEstatusFiniquito.estatusFiniquito = objBaja.est_compras == "A" ? true : false;
                lstEstatusFiniquito.Add(objEstatusFiniquito);

                resultado.Add(SUCCESS, true);
                resultado.Add("lstEstatusFiniquito", lstEstatusFiniquito);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetEstatusFiniquitoEmpleadoDemanda", e, AccionEnum.CONSULTA, 0, new { claveEmpleado = claveEmpleado });
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        #endregion

        #region HISTORICO
        public Dictionary<string, object> GetHistorico(HistoricoDTO objDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE LAS DEMANDAS CON SUS SEGUIMIENTOS

                // SE OBTIENE LAS DEMANDAS
                List<tblRH_DMS_CapturaDemandas> lstHistorico = _context.tblRH_DMS_CapturaDemandas.Where(w => w.registroActivo).ToList();

                #region FILTROS
                if (objDTO.esDemandaCerrada > 0)
                    lstHistorico = lstHistorico.Where(w => w.esDemandaCerrada == objDTO.esDemandaCerrada).ToList();

                if (!string.IsNullOrEmpty(objDTO.cc))
                    lstHistorico = lstHistorico.Where(w => w.cc == objDTO.cc).ToList();
                #endregion

                List<HistoricoDTO> lstHistoricoDTO = new List<HistoricoDTO>();
                HistoricoDTO objHistoricoDTO = new HistoricoDTO();
                foreach (var item in lstHistorico)
                {
                    objHistoricoDTO = new HistoricoDTO();
                    objHistoricoDTO.id = item.id;
                    objHistoricoDTO.estatusDemanda = EnumHelper.GetDescription((EstatusDemandaEnum)item.esDemandaCerrada);
                    objHistoricoDTO.claveEmpleado = item.claveEmpleado;
                    objHistoricoDTO.nombreDemandante = item.nombreDemandante;
                    objHistoricoDTO.puesto = item.puesto;
                    objHistoricoDTO.cc = !string.IsNullOrEmpty(item.cc) && !string.IsNullOrEmpty(item.cc_Descripcion) ? string.Format("[{0}] {1}", item.cc, item.cc_Descripcion) : "-";

                    if (objHistoricoDTO.fechaIngreso != null)
                        objHistoricoDTO.fechaIngreso = Convert.ToDateTime(item.fechaIngreso);

                    objHistoricoDTO.motivoSalida = item.motivoSalida;
                    objHistoricoDTO.demandado = item.demandado;
                    objHistoricoDTO.esDemandaCerrada = item.esDemandaCerrada;

                    HistoricoDTO objUltimoSeguimiento = GetUltimoSeguimiento(item.id);
                    objHistoricoDTO.estadoActual = !string.IsNullOrEmpty(objUltimoSeguimiento.estadoActual) ? objUltimoSeguimiento.estadoActual : item.estadoActual;
                    objHistoricoDTO.fechaAudiencia = objUltimoSeguimiento.fechaAudiencia != null ? objUltimoSeguimiento.fechaAudiencia : item.fechaAudiencia;
                    objHistoricoDTO.cuantiaTotal = objUltimoSeguimiento.cuantiaTotal > 0 ? objUltimoSeguimiento.cuantiaTotal : item.cuantiaTotal;
                    objHistoricoDTO.abogadoDemandante = !string.IsNullOrEmpty(objUltimoSeguimiento.abogadoDemandante) ? objUltimoSeguimiento.abogadoDemandante : item.abogadoDemandante;
                    objHistoricoDTO.semaforo = objUltimoSeguimiento.semaforo > 0 ? objUltimoSeguimiento.semaforo : item.semaforo;
                    objHistoricoDTO.strSemaforo = EnumHelper.GetDescription((SemaforoEnum)objHistoricoDTO.semaforo);

                    objHistoricoDTO.negociadoCerrado = item.negociadoCerrado;
                    objHistoricoDTO.diferencia = (decimal)item.cuantiaTotal - (decimal)objHistoricoDTO.negociadoCerrado;
                    objHistoricoDTO.colorDiferencia = (decimal)objHistoricoDTO.diferencia > 0 ? "success" : "danger";
                    lstHistoricoDTO.Add(objHistoricoDTO);
                }

                #region SE FILTRA POR SEMAFORO
                switch (objDTO.semaforo)
                {
                    case (int)SemaforoEnum.ROJO:
                        lstHistoricoDTO = lstHistoricoDTO.Where(w => w.semaforo == (int)SemaforoEnum.ROJO).ToList();
                        break;
                    case (int)SemaforoEnum.AMBAR:
                        lstHistoricoDTO = lstHistoricoDTO.Where(w => w.semaforo == (int)SemaforoEnum.AMBAR).ToList();
                        break;
                    case (int)SemaforoEnum.VERDE:
                        lstHistoricoDTO = lstHistoricoDTO.Where(w => w.semaforo == (int)SemaforoEnum.VERDE).ToList();
                        break;
                }
                #endregion

                // SE OBTIENE LA CANTIDAD DE DEMANDAS EN BASE A SU COLOR DEL SEMAFORO
                HistoricoDTO objCantDemandasSemaforo = new HistoricoDTO();
                objCantDemandasSemaforo.cantDemandasSemaforoRojo = lstHistoricoDTO.Where(w => w.semaforo == (int)SemaforoEnum.ROJO).Count();
                objCantDemandasSemaforo.cantDemandasSemaforoAmbar = lstHistoricoDTO.Where(w => w.semaforo == (int)SemaforoEnum.AMBAR).Count();
                objCantDemandasSemaforo.cantDemandasSemaforoVerde = lstHistoricoDTO.Where(w => w.semaforo == (int)SemaforoEnum.VERDE).Count();

                // SE OBTIENE LA CANTIDAD DE DEMANDAS ACTIVAS Y CERRADAS
                HistoricoDTO objCantDemandasActivasCerradas = new HistoricoDTO();
                objCantDemandasActivasCerradas.cantDemandasActivas = lstHistoricoDTO.Where(w => w.esDemandaCerrada == (int)EstatusDemandaEnum.ABIERTO).Count();
                objCantDemandasActivasCerradas.cantDemandasCerradas = lstHistoricoDTO.Where(w => w.esDemandaCerrada == (int)EstatusDemandaEnum.CERRADO).Count();

                resultado.Add(SUCCESS, true);
                resultado.Add("lstHistoricoDTO", lstHistoricoDTO);
                resultado.Add("objCantDemandasSemaforo", objCantDemandasSemaforo);
                resultado.Add("objCantDemandasActivasCerradas", objCantDemandasActivasCerradas);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetHistorico", e, AccionEnum.CONSULTA, objDTO.id, objDTO);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        private HistoricoDTO GetUltimoSeguimiento(int idCaptura)
        {
            HistoricoDTO objHistoricoDTO = new HistoricoDTO();
            try
            {
                #region VALIDACIONES
                string mensajeError = "Ocurrió un error al obtener el ultimo seguimiento de la demanda.";
                if (idCaptura <= 0) { throw new Exception(mensajeError); }
                #endregion

                // SE OBTIENE EL ULTIMO SEGUIMIENTO DE LA DEMANDA
                tblRH_DMS_SeguimientoDemanda objSeguimiento = _context.tblRH_DMS_SeguimientoDemanda.Where(w => w.FK_Demanda == idCaptura && w.registroActivo).OrderByDescending(o => o.id).FirstOrDefault();
                if (objSeguimiento != null)
                {
                    // SE OBTIENE LA INFORMACIÓN DEL ULTIMO SEGUIMIENTO
                    objHistoricoDTO = new HistoricoDTO();
                    objHistoricoDTO.estadoActual = objSeguimiento.estadoActual;
                    objHistoricoDTO.fechaAudiencia = objSeguimiento.fechaAudiencia;
                    objHistoricoDTO.ofertaInicial = objSeguimiento.cuantia;
                    objHistoricoDTO.abogadoDemandante = objSeguimiento.abogadoDemandante;
                    objHistoricoDTO.semaforo = objSeguimiento.semaforo;
                }
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetUltimoSeguimiento", e, AccionEnum.CONSULTA, idCaptura, new { idCaptura = idCaptura });
                return objHistoricoDTO;
            }
            return objHistoricoDTO;
        }

        public Dictionary<string, object> GetLstSeguimientos(int FK_Captura)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                string mensajeError = "Ocurrió un error al obtener el seguimiento.";
                if (FK_Captura <= 0) { throw new Exception(mensajeError); }
                #endregion

                #region SE OBTIENE EL SEGUIMIENTO DE LA DEMANDA SELECCIONADA
                List<tblRH_DMS_SeguimientoDemanda> lstSeguimientos = _context.tblRH_DMS_SeguimientoDemanda.Where(w => w.FK_Demanda == FK_Captura && w.registroActivo).OrderBy(o => o.fechaCreacion).ToList();

                List<SeguimientoDTO> lstSeguimientosDTO = new List<SeguimientoDTO>();
                SeguimientoDTO objSeguimientoDTO = new SeguimientoDTO();
                int numConsecutivo = 1;
                foreach (var item in lstSeguimientos)
                {
                    objSeguimientoDTO = new SeguimientoDTO();
                    objSeguimientoDTO.cuantia = item.cuantia;
                    objSeguimientoDTO.abogadoDemandante = item.abogadoDemandante;
                    objSeguimientoDTO.fechaAudiencia = item.fechaAudiencia;
                    objSeguimientoDTO.semaforo = item.semaforo;
                    objSeguimientoDTO.strSemaforo = EnumHelper.GetDescription((SemaforoEnum)item.semaforo);
                    objSeguimientoDTO.estadoActual = item.estadoActual;
                    objSeguimientoDTO.fechaCreacion = item.fechaCreacion;
                    objSeguimientoDTO.numConsecutivo = numConsecutivo;
                    lstSeguimientosDTO.Add(objSeguimientoDTO);
                    numConsecutivo++;
                }

                resultado.Add(SUCCESS, true);
                resultado.Add("lstSeguimientosDTO", lstSeguimientosDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetLstSeguimientos", e, AccionEnum.CONSULTA, FK_Captura, new { FK_Captura = FK_Captura });
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        #endregion

        #region DASHBOARD
        public Dictionary<string, object> GetDashboard(DashboardDTO objFiltroDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                // SE OBTIENE LA EMPRESA LOGUEADA
                MainContextEnum objEmpresa = GetEmpresaLogueada();
                GraficaDTO objGraficaDTO = new GraficaDTO();
                string strQuery = string.Empty;

                List<tblRH_DMS_CapturaDemandas> lstDemandas = _context.tblRH_DMS_CapturaDemandas
                                                                .Where(w => objFiltroDTO.anioDemanda > 0 ? w.fechaDemanda.Year == objFiltroDTO.anioDemanda : true && 
                                                                            !string.IsNullOrEmpty(objFiltroDTO.filtro_CC) ? w.cc == objFiltroDTO.filtro_CC : true &&
                                                                            w.registroActivo).ToList();

                List<int> lstFK_Captura = new List<int>();
                foreach (var item in lstDemandas) { lstFK_Captura.Add(item.id); }
                List<tblRH_DMS_SeguimientoDemanda> lstSeguimientos = _context.tblRH_DMS_SeguimientoDemanda.Where(w => lstFK_Captura.Contains(w.FK_Demanda) && w.registroActivo).ToList();

                #region GRAFICA: COMPORTAMIENTO HISTORICO DE LAS DEMANDAS
                // SE OBTIENE LISTADO DE AÑOS DE LAS DEMANDAS REGISTRADAS
                List<int> lstAniosDemandasRegistradas = _context.Select<int>(new DapperDTO
                {
                    baseDatos = objEmpresa,
                    consulta = @"SELECT DATEPART(yyyy, fechaDemanda) AS anioDemanda
		                                FROM tblRH_DMS_CapturaDemandas 
			                                WHERE registroActivo = @registroActivo
				                                GROUP BY DATEPART(yyyy, fechaDemanda)",
                    parametros = new { registroActivo = true }
                }).ToList();

                List<GraficaDTO> lstGraficaComportamientoHistorico = new List<GraficaDTO>();
                objGraficaDTO = new GraficaDTO();
                foreach (var item in lstAniosDemandasRegistradas)
                {
                    int cantDemandasAnio = lstDemandas.Where(w => w.fechaDemanda.Year == item).Count();
                    int cantDemandasCerradas = lstDemandas.Where(w => w.fechaDemanda.Year == item && w.esDemandaCerrada == (int)EstatusDemandaEnum.CERRADO).Count();
                    int cantCierreCadaAnio = ((GetDemandasAbiertasAniosPasados(item)) - cantDemandasCerradas);
                    if (cantCierreCadaAnio < 0)
                        cantCierreCadaAnio = 0;

                    objGraficaDTO = new GraficaDTO();
                    objGraficaDTO.name = item.ToString();
                    objGraficaDTO.lst_y = new List<decimal>();
                    objGraficaDTO.lst_y.Add(cantDemandasAnio);
                    objGraficaDTO.lst_y.Add(cantDemandasCerradas);
                    objGraficaDTO.lst_y.Add(cantCierreCadaAnio);
                    lstGraficaComportamientoHistorico.Add(objGraficaDTO);
                }
                #endregion

                #region GRAFICA: DEMANDAS ACTIVAS POR AÑO DE PRESENTACIÓN
                strQuery = string.Format(@"SELECT COUNT(fechaDemanda) AS cantDemandasActivas, DATEPART(yyyy, fechaDemanda) AS anioDemanda
                                                    FROM tblRH_DMS_CapturaDemandas 
	                                                    WHERE registroActivo = {0} AND esDemandaCerrada = {1} ", 1, (int)EstatusDemandaEnum.ABIERTO);

                #region FILTROS
                if (objFiltroDTO.filtro_Anio > 0)
                    strQuery += string.Format(" AND fechaDemanda LIKE '%{0}%'", objFiltroDTO.filtro_Anio);

                if (!string.IsNullOrEmpty(objFiltroDTO.filtro_CC))
                    strQuery += string.Format(" AND cc = '{0}'", objFiltroDTO.filtro_CC);

                strQuery += string.Format(" GROUP BY DATEPART(yyyy, fechaDemanda)");
                #endregion

                List<DashboardDTO> lstDemandasActivas = _context.Select<DashboardDTO>(new DapperDTO
                {
                    baseDatos = objEmpresa,
                    consulta = strQuery
                }).ToList();

                List<GraficaDTO> lstGraficaCantDemandasActivas = new List<GraficaDTO>();
                objGraficaDTO = new GraficaDTO();
                foreach (var item in lstDemandasActivas)
                {
                    objGraficaDTO = new GraficaDTO();
                    objGraficaDTO.name = item.anioDemanda.ToString();
                    objGraficaDTO.y = item.cantDemandasActivas;
                    objGraficaDTO.drilldown = objGraficaDTO.name;
                    lstGraficaCantDemandasActivas.Add(objGraficaDTO);
                }
                #endregion

                #region GRAFICA: COMPORTAMIENTO DE LAS DEMANDAS EN BASE AL AÑO SELECCIONADO
                // SE OBTIENE TODAS LAS DEMANDAS REGISTRADAS
                List<tblRH_DMS_CapturaDemandas> lstDemandasComportamiento = _context.tblRH_DMS_CapturaDemandas.Where(w => w.fechaDemanda.Year == objFiltroDTO.filtro_Anio && w.registroActivo).ToList();
                DashboardDTO objTblComportamientosDTO = new DashboardDTO();

                objTblComportamientosDTO.tdAnio = objFiltroDTO.filtro_Anio;
                objTblComportamientosDTO.tdTotalBajas_Anio = objFiltroDTO.filtro_Anio;
                objTblComportamientosDTO.tdTotalEmpleados_Anio = objFiltroDTO.filtro_Anio;

                #region NUEVAS
                objTblComportamientosDTO.tdNuevas_Ene = lstDemandasComportamiento.Where(w => w.fechaDemanda.Month == 01 && w.registroActivo).Count();
                objTblComportamientosDTO.tdNuevas_Feb = lstDemandasComportamiento.Where(w => w.fechaDemanda.Month == 02 && w.registroActivo).Count();
                objTblComportamientosDTO.tdNuevas_Mar = lstDemandasComportamiento.Where(w => w.fechaDemanda.Month == 03 && w.registroActivo).Count();
                objTblComportamientosDTO.tdNuevas_Abr = lstDemandasComportamiento.Where(w => w.fechaDemanda.Month == 04 && w.registroActivo).Count();
                objTblComportamientosDTO.tdNuevas_May = lstDemandasComportamiento.Where(w => w.fechaDemanda.Month == 05 && w.registroActivo).Count();
                objTblComportamientosDTO.tdNuevas_Jun = lstDemandasComportamiento.Where(w => w.fechaDemanda.Month == 06 && w.registroActivo).Count();
                objTblComportamientosDTO.tdNuevas_Jul = lstDemandasComportamiento.Where(w => w.fechaDemanda.Month == 07 && w.registroActivo).Count();
                objTblComportamientosDTO.tdNuevas_Ago = lstDemandasComportamiento.Where(w => w.fechaDemanda.Month == 08 && w.registroActivo).Count();
                objTblComportamientosDTO.tdNuevas_Sep = lstDemandasComportamiento.Where(w => w.fechaDemanda.Month == 09 && w.registroActivo).Count();
                objTblComportamientosDTO.tdNuevas_Oct = lstDemandasComportamiento.Where(w => w.fechaDemanda.Month == 10 && w.registroActivo).Count();
                objTblComportamientosDTO.tdNuevas_Nov = lstDemandasComportamiento.Where(w => w.fechaDemanda.Month == 11 && w.registroActivo).Count();
                objTblComportamientosDTO.tdNuevas_Dic = lstDemandasComportamiento.Where(w => w.fechaDemanda.Month == 12 && w.registroActivo).Count();
                objTblComportamientosDTO.tdNuevas_Total = (objTblComportamientosDTO.tdNuevas_Ene +
                                                            objTblComportamientosDTO.tdNuevas_Feb +
                                                            objTblComportamientosDTO.tdNuevas_Mar +
                                                            objTblComportamientosDTO.tdNuevas_Abr +
                                                            objTblComportamientosDTO.tdNuevas_May +
                                                            objTblComportamientosDTO.tdNuevas_Jun +
                                                            objTblComportamientosDTO.tdNuevas_Jul +
                                                            objTblComportamientosDTO.tdNuevas_Ago +
                                                            objTblComportamientosDTO.tdNuevas_Sep +
                                                            objTblComportamientosDTO.tdNuevas_Oct +
                                                            objTblComportamientosDTO.tdNuevas_Nov +
                                                            objTblComportamientosDTO.tdNuevas_Dic);
                #endregion

                #region CIERRES
                objTblComportamientosDTO.tdCierres_Ene = lstDemandasComportamiento.Where(w => w.fechaDemanda.Month == 01 && w.esDemandaCerrada == (int)EstatusDemandaEnum.CERRADO).Count();
                objTblComportamientosDTO.tdCierres_Feb = lstDemandasComportamiento.Where(w => w.fechaDemanda.Month == 02 && w.esDemandaCerrada == (int)EstatusDemandaEnum.CERRADO).Count();
                objTblComportamientosDTO.tdCierres_Mar = lstDemandasComportamiento.Where(w => w.fechaDemanda.Month == 03 && w.esDemandaCerrada == (int)EstatusDemandaEnum.CERRADO).Count();
                objTblComportamientosDTO.tdCierres_Abr = lstDemandasComportamiento.Where(w => w.fechaDemanda.Month == 04 && w.esDemandaCerrada == (int)EstatusDemandaEnum.CERRADO).Count();
                objTblComportamientosDTO.tdCierres_May = lstDemandasComportamiento.Where(w => w.fechaDemanda.Month == 05 && w.esDemandaCerrada == (int)EstatusDemandaEnum.CERRADO).Count();
                objTblComportamientosDTO.tdCierres_Jun = lstDemandasComportamiento.Where(w => w.fechaDemanda.Month == 06 && w.esDemandaCerrada == (int)EstatusDemandaEnum.CERRADO).Count();
                objTblComportamientosDTO.tdCierres_Jul = lstDemandasComportamiento.Where(w => w.fechaDemanda.Month == 07 && w.esDemandaCerrada == (int)EstatusDemandaEnum.CERRADO).Count();
                objTblComportamientosDTO.tdCierres_Ago = lstDemandasComportamiento.Where(w => w.fechaDemanda.Month == 08 && w.esDemandaCerrada == (int)EstatusDemandaEnum.CERRADO).Count();
                objTblComportamientosDTO.tdCierres_Sep = lstDemandasComportamiento.Where(w => w.fechaDemanda.Month == 09 && w.esDemandaCerrada == (int)EstatusDemandaEnum.CERRADO).Count();
                objTblComportamientosDTO.tdCierres_Oct = lstDemandasComportamiento.Where(w => w.fechaDemanda.Month == 10 && w.esDemandaCerrada == (int)EstatusDemandaEnum.CERRADO).Count();
                objTblComportamientosDTO.tdCierres_Nov = lstDemandasComportamiento.Where(w => w.fechaDemanda.Month == 11 && w.esDemandaCerrada == (int)EstatusDemandaEnum.CERRADO).Count();
                objTblComportamientosDTO.tdCierres_Dic = lstDemandasComportamiento.Where(w => w.fechaDemanda.Month == 12 && w.esDemandaCerrada == (int)EstatusDemandaEnum.CERRADO).Count();
                objTblComportamientosDTO.tdCierres_Total = (objTblComportamientosDTO.tdCierres_Ene +
                                                                objTblComportamientosDTO.tdCierres_Feb +
                                                                objTblComportamientosDTO.tdCierres_Mar +
                                                                objTblComportamientosDTO.tdCierres_Abr +
                                                                objTblComportamientosDTO.tdCierres_May +
                                                                objTblComportamientosDTO.tdCierres_Jun +
                                                                objTblComportamientosDTO.tdCierres_Jul +
                                                                objTblComportamientosDTO.tdCierres_Ago +
                                                                objTblComportamientosDTO.tdCierres_Sep +
                                                                objTblComportamientosDTO.tdCierres_Oct +
                                                                objTblComportamientosDTO.tdCierres_Nov +
                                                                objTblComportamientosDTO.tdCierres_Dic);
                #endregion

                #region FIN DE MES
                objTblComportamientosDTO.tdFinDeMes_Ene = ((objTblComportamientosDTO.tdInicioMes_Ene + objTblComportamientosDTO.tdNuevas_Ene) - objTblComportamientosDTO.tdCierres_Ene);
                objTblComportamientosDTO.tdFinDeMes_Feb = ((objTblComportamientosDTO.tdInicioMes_Feb + objTblComportamientosDTO.tdNuevas_Feb) - objTblComportamientosDTO.tdCierres_Feb);
                objTblComportamientosDTO.tdFinDeMes_Mar = ((objTblComportamientosDTO.tdInicioMes_Mar + objTblComportamientosDTO.tdNuevas_Mar) - objTblComportamientosDTO.tdCierres_Mar);
                objTblComportamientosDTO.tdFinDeMes_Abr = ((objTblComportamientosDTO.tdInicioMes_Abr + objTblComportamientosDTO.tdNuevas_Abr) - objTblComportamientosDTO.tdCierres_Abr);
                objTblComportamientosDTO.tdFinDeMes_May = ((objTblComportamientosDTO.tdInicioMes_May + objTblComportamientosDTO.tdNuevas_May) - objTblComportamientosDTO.tdCierres_May);
                objTblComportamientosDTO.tdFinDeMes_Jun = ((objTblComportamientosDTO.tdInicioMes_Jun + objTblComportamientosDTO.tdNuevas_Jun) - objTblComportamientosDTO.tdCierres_Jun);
                objTblComportamientosDTO.tdFinDeMes_Jul = ((objTblComportamientosDTO.tdInicioMes_Jul + objTblComportamientosDTO.tdNuevas_Jul) - objTblComportamientosDTO.tdCierres_Jul);
                objTblComportamientosDTO.tdFinDeMes_Ago = ((objTblComportamientosDTO.tdInicioMes_Ago + objTblComportamientosDTO.tdNuevas_Ago) - objTblComportamientosDTO.tdCierres_Ago);
                objTblComportamientosDTO.tdFinDeMes_Sep = ((objTblComportamientosDTO.tdInicioMes_Sep + objTblComportamientosDTO.tdNuevas_Sep) - objTblComportamientosDTO.tdCierres_Sep);
                objTblComportamientosDTO.tdFinDeMes_Oct = ((objTblComportamientosDTO.tdInicioMes_Oct + objTblComportamientosDTO.tdNuevas_Oct) - objTblComportamientosDTO.tdCierres_Oct);
                objTblComportamientosDTO.tdFinDeMes_Nov = ((objTblComportamientosDTO.tdInicioMes_Nov + objTblComportamientosDTO.tdNuevas_Nov) - objTblComportamientosDTO.tdCierres_Nov);
                objTblComportamientosDTO.tdFinDeMes_Dic = ((objTblComportamientosDTO.tdInicioMes_Dic + objTblComportamientosDTO.tdNuevas_Dic) - objTblComportamientosDTO.tdCierres_Dic);
                objTblComportamientosDTO.tdFinDeMes_Total = ((objTblComportamientosDTO.tdInicioMes_Total + objTblComportamientosDTO.tdNuevas_Total) - objTblComportamientosDTO.tdCierres_Total);
                #endregion

                #region TOTAL BAJAS | TOTAL EMPLEADOS
                // LISTADO DE EMPLEADOS DADOS DE BAJA
                List<tblRH_EK_Empl_Baja> lstEmpleadosBajas = _context.Select<tblRH_EK_Empl_Baja>(new DapperDTO
                {
                    baseDatos = objEmpresa,
                    consulta = @"SELECT t2.fecha_baja
	                                    FROM tblRH_EK_Empleados AS t1
	                                    INNER JOIN tblRH_EK_Empl_Baja AS t2 ON t2.clave_empleado = t1.clave_empleado
		                                    WHERE t1.estatus_empleado = @estatus_empleado AND t1.esActivo = @esActivo AND t2.estatus = @estatus
			                                    ORDER BY fecha_baja",
                    parametros = new { estatus_empleado = "B", esActivo = true, estatus = "A" }
                }).ToList();

                // LISTADO EMPLEADOS ALTA
                List<tblRH_EK_Empleados> lstEmpleadosAltas = _context.Select<tblRH_EK_Empleados>(new DapperDTO
                {
                    baseDatos = objEmpresa,
                    consulta = @"SELECT fecha_alta
	                                    FROM tblRH_EK_Empleados
		                                    WHERE estatus_empleado = @estatus_empleado AND esActivo = @esActivo
			                                    ORDER BY fecha_alta",
                    parametros = new { estatus_empleado = "A", esActivo = true }
                }).ToList();

                for (int i = 1; i < 13; i++)
                {
                    int MM = Convert.ToInt32(i);
                    int yyyy = Convert.ToInt32(objFiltroDTO.filtro_Anio);
                    int dd = DateTime.DaysInMonth(yyyy, MM);

                    DateTime fechaInicioMes = new DateTime(yyyy, MM, i);
                    DateTime fechaFinMes = new DateTime(yyyy, MM, dd);

                    #region SE OBTIENE LA CANTIDAD DE BAJAS POR MES
                    int cantBajasMes = lstEmpleadosBajas.Where(w => w.fecha_baja >= fechaInicioMes && w.fecha_baja <= fechaFinMes).Count();
                    switch (i)
                    {
                        case 1:
                            objTblComportamientosDTO.tdTotalBajas_Ene = cantBajasMes;
                            break;
                        case 2:
                            objTblComportamientosDTO.tdTotalBajas_Feb = cantBajasMes;
                            break;
                        case 3:
                            objTblComportamientosDTO.tdTotalBajas_Mar = cantBajasMes;
                            break;
                        case 4:
                            objTblComportamientosDTO.tdTotalBajas_Abr = cantBajasMes;
                            break;
                        case 5:
                            objTblComportamientosDTO.tdTotalBajas_May = cantBajasMes;
                            break;
                        case 6:
                            objTblComportamientosDTO.tdTotalBajas_Jun = cantBajasMes;
                            break;
                        case 7:
                            objTblComportamientosDTO.tdTotalBajas_Jul = cantBajasMes;
                            break;
                        case 8:
                            objTblComportamientosDTO.tdTotalBajas_Ago = cantBajasMes;
                            break;
                        case 9:
                            objTblComportamientosDTO.tdTotalBajas_Sep = cantBajasMes;
                            break;
                        case 10:
                            objTblComportamientosDTO.tdTotalBajas_Oct = cantBajasMes;
                            break;
                        case 11:
                            objTblComportamientosDTO.tdTotalBajas_Nov = cantBajasMes;
                            break;
                        case 12:
                            objTblComportamientosDTO.tdTotalBajas_Dic = cantBajasMes;
                            break;
                    }
                    objTblComportamientosDTO.tdTotalBajas_Total = (objTblComportamientosDTO.tdTotalBajas_Ene +
                                                                    objTblComportamientosDTO.tdTotalBajas_Feb +
                                                                    objTblComportamientosDTO.tdTotalBajas_Mar +
                                                                    objTblComportamientosDTO.tdTotalBajas_Abr +
                                                                    objTblComportamientosDTO.tdTotalBajas_May +
                                                                    objTblComportamientosDTO.tdTotalBajas_Jun +
                                                                    objTblComportamientosDTO.tdTotalBajas_Jul +
                                                                    objTblComportamientosDTO.tdTotalBajas_Ago +
                                                                    objTblComportamientosDTO.tdTotalBajas_Sep +
                                                                    objTblComportamientosDTO.tdTotalBajas_Oct +
                                                                    objTblComportamientosDTO.tdTotalBajas_Nov +
                                                                    objTblComportamientosDTO.tdTotalBajas_Dic);
                    #endregion

                    #region SE OBTIENE LA CANTIDAD DE ALTAS DEL MES
                    int cantAltasMes = lstEmpleadosAltas.Where(w => w.fecha_alta >= fechaInicioMes && w.fecha_alta <= fechaFinMes).Count();
                    switch (i)
                    {
                        case 1:
                            objTblComportamientosDTO.tdTotalEmpleados_Ene = cantAltasMes;
                            break;
                        case 2:
                            objTblComportamientosDTO.tdTotalEmpleados_Feb = cantAltasMes;
                            break;
                        case 3:
                            objTblComportamientosDTO.tdTotalEmpleados_Mar = cantAltasMes;
                            break;
                        case 4:
                            objTblComportamientosDTO.tdTotalEmpleados_Abr = cantAltasMes;
                            break;
                        case 5:
                            objTblComportamientosDTO.tdTotalEmpleados_May = cantAltasMes;
                            break;
                        case 6:
                            objTblComportamientosDTO.tdTotalEmpleados_Jun = cantAltasMes;
                            break;
                        case 7:
                            objTblComportamientosDTO.tdTotalEmpleados_Jul = cantAltasMes;
                            break;
                        case 8:
                            objTblComportamientosDTO.tdTotalEmpleados_Ago = cantAltasMes;
                            break;
                        case 9:
                            objTblComportamientosDTO.tdTotalEmpleados_Sep = cantAltasMes;
                            break;
                        case 10:
                            objTblComportamientosDTO.tdTotalEmpleados_Oct = cantAltasMes;
                            break;
                        case 11:
                            objTblComportamientosDTO.tdTotalEmpleados_Nov = cantAltasMes;
                            break;
                        case 12:
                            objTblComportamientosDTO.tdTotalEmpleados_Dic = cantAltasMes;
                            break;
                    }
                    objTblComportamientosDTO.tdTotalEmpleados_Total = (objTblComportamientosDTO.tdTotalEmpleados_Ene +
                                                                    objTblComportamientosDTO.tdTotalEmpleados_Feb +
                                                                    objTblComportamientosDTO.tdTotalEmpleados_Mar +
                                                                    objTblComportamientosDTO.tdTotalEmpleados_Abr +
                                                                    objTblComportamientosDTO.tdTotalEmpleados_May +
                                                                    objTblComportamientosDTO.tdTotalEmpleados_Jun +
                                                                    objTblComportamientosDTO.tdTotalEmpleados_Jul +
                                                                    objTblComportamientosDTO.tdTotalEmpleados_Ago +
                                                                    objTblComportamientosDTO.tdTotalEmpleados_Sep +
                                                                    objTblComportamientosDTO.tdTotalEmpleados_Oct +
                                                                    objTblComportamientosDTO.tdTotalEmpleados_Nov +
                                                                    objTblComportamientosDTO.tdTotalEmpleados_Dic);
                    #endregion
                }
                #endregion
                #endregion

                #region GRAFICA: CANTIDAD DE DEMANDAS SEGUN SEMAFORO
                #region GRAFICA
                int cantDemandas = 0, cantDemandasSemaforoVerde = 0, cantDemandasSemaforoAmbar = 0, cantDemandasSemaforoRojo = 0;
                foreach (var item in lstDemandas)
                {
                    // SE VERIFICA SI TIENE ALGUN SEGUIMIENTO LA DEMANDA
                    int idSemaforo = item.semaforo;
                    tblRH_DMS_SeguimientoDemanda objSeguimiento = _context.tblRH_DMS_SeguimientoDemanda.Where(w => w.FK_Demanda == item.id && w.registroActivo).OrderByDescending(o => o.id).FirstOrDefault();
                    if (objSeguimiento != null)
                        idSemaforo = objSeguimiento.semaforo;

                    switch (idSemaforo)
                    {
                        case (int)SemaforoEnum.ROJO:
                            cantDemandasSemaforoRojo++;
                            break;
                        case (int)SemaforoEnum.AMBAR:
                            cantDemandasSemaforoAmbar++;
                            break;
                        case (int)SemaforoEnum.VERDE:
                            cantDemandasSemaforoVerde++;
                            break;
                    }
                    cantDemandas++;
                }

                List<GraficaDTO> lstGraficaCantDemandasSegunSemaforo = new List<GraficaDTO>();

                objGraficaDTO = new GraficaDTO();
                objGraficaDTO.name = "Demandas";
                objGraficaDTO.y = cantDemandas;
                objGraficaDTO.drilldown = cantDemandas.ToString();
                lstGraficaCantDemandasSegunSemaforo.Add(objGraficaDTO);

                objGraficaDTO = new GraficaDTO();
                objGraficaDTO.name = "Verde";
                objGraficaDTO.y = cantDemandasSemaforoVerde;
                objGraficaDTO.drilldown = cantDemandasSemaforoVerde.ToString();
                lstGraficaCantDemandasSegunSemaforo.Add(objGraficaDTO);

                objGraficaDTO = new GraficaDTO();
                objGraficaDTO.name = "Ambar";
                objGraficaDTO.y = cantDemandasSemaforoAmbar;
                objGraficaDTO.drilldown = cantDemandasSemaforoAmbar.ToString();
                lstGraficaCantDemandasSegunSemaforo.Add(objGraficaDTO);

                objGraficaDTO = new GraficaDTO();
                objGraficaDTO.name = "Rojo";
                objGraficaDTO.y = cantDemandasSemaforoRojo;
                objGraficaDTO.drilldown = cantDemandasSemaforoRojo.ToString();
                lstGraficaCantDemandasSegunSemaforo.Add(objGraficaDTO);
                #endregion

                #region TABLA
                List<CapturaDTO> lstDemandasDTO = new List<CapturaDTO>();
                CapturaDTO objDemandaDTO = new CapturaDTO();
                foreach (var item in lstDemandas)
                {
                    if (item.semaforo != (int)SemaforoEnum.VERDE)
                    {
                        objDemandaDTO = new CapturaDTO();
                        objDemandaDTO.nombreDemandante = item.nombreDemandante;
                        objDemandaDTO.cc = string.Format("[{0}] {1}", item.cc, item.cc_Descripcion);
                        objDemandaDTO.puesto = item.puesto;
                        objDemandaDTO.anioFechaDemanda = item.fechaDemanda.Year;
                        objDemandaDTO.motivoSalida = item.motivoSalida;
                        objDemandaDTO.demandado = item.demandado;

                        // SE VERIFICA SI CUENTA CON SEGUIMIENTO LA DEMANDA
                        tblRH_DMS_SeguimientoDemanda objUltimoSeguimiento = _context.tblRH_DMS_SeguimientoDemanda.Where(w => w.FK_Demanda == item.id && w.registroActivo).OrderByDescending(o => o.id).FirstOrDefault();
                        if (objUltimoSeguimiento != null)
                        {
                            objDemandaDTO.estadoActual = objUltimoSeguimiento.estadoActual;
                            objDemandaDTO.cuantiaTotal = objUltimoSeguimiento.cuantia;
                            objDemandaDTO.semaforo = objUltimoSeguimiento.semaforo;
                            objDemandaDTO.strSemaforo = EnumHelper.GetDescription((SemaforoEnum)objDemandaDTO.semaforo);
                        }
                        else
                        {
                            objDemandaDTO.estadoActual = item.estadoActual;
                            objDemandaDTO.cuantiaTotal = item.ofertaInicial;
                            objDemandaDTO.semaforo = item.semaforo;
                            objDemandaDTO.strSemaforo = EnumHelper.GetDescription((SemaforoEnum)objDemandaDTO.semaforo);
                        }
                        lstDemandasDTO.Add(objDemandaDTO);
                    }
                }
                #endregion
                #endregion

                #region TABLA: CUANTILLA FINAL - FINIQUITO 100%
                List<DashboardDTO> lstDiferencias = new List<DashboardDTO>();
                DashboardDTO objDiferencia = new DashboardDTO();
                foreach (var item in lstDemandas)
                {
                    objDiferencia = new DashboardDTO();
                    objDiferencia.cuantillaTotal = item.cuantiaTotal;
                    objDiferencia.finiquitoAl100 = item.negociadoCerrado;
                    objDiferencia.diferencia = item.diferencia;

                    tblRH_DMS_SeguimientoDemanda objSeguimiento = lstSeguimientos.Where(w => w.FK_Demanda == item.id).OrderByDescending(o => o.fechaCreacion).FirstOrDefault();
                    if (objSeguimiento != null)
                    {
                        objDiferencia.cuantillaTotal = objSeguimiento.cuantia;
                        objDiferencia.diferencia = ((decimal)objDiferencia.cuantillaTotal - (decimal)item.negociadoCerrado);
                    }

                    objDiferencia.nombre = item.nombreDemandante;
                    objDiferencia.puesto = item.puesto;
                    lstDiferencias.Add(objDiferencia);
                }
                #endregion

                #region RESULTADOS
                resultado.Add(SUCCESS, true);
                resultado.Add("lstGraficaComportamientoHistorico", lstGraficaComportamientoHistorico);
                resultado.Add("lstGraficaCantDemandasActivas", lstGraficaCantDemandasActivas);
                resultado.Add("objTblComportamientosDTO", objTblComportamientosDTO);
                resultado.Add("lstGraficaCantDemandasSegunSemaforo", lstGraficaCantDemandasSegunSemaforo);
                resultado.Add("lstDemandasDTO", lstDemandasDTO);
                resultado.Add("lstDiferencias", lstDiferencias);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetDashboard", e, AccionEnum.CONSULTA, 0, objFiltroDTO);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        private int GetDemandasAbiertasAniosPasados(int anio)
        {
            int cantCierreCadaAnio = 0;
            try
            {
                #region VALIDACIONES
                if (anio <= 0) { throw new Exception("Ocurrió un error al obtener el parametro año para obtener la cantidad de cierre de cada año."); }
                #endregion

                #region SE OBTIENE LA CANTIDAD DE DEMANDAS ABIERTAS DE LOS AÑOS PASADOS, EN BASE AL AÑO DEL PARAMETRO
                string strQuery = string.Format(@"SELECT fechaDemanda, esDemandaCerrada, fechaCierreDemanda FROM tblRH_DMS_CapturaDemandas 
	                                                    WHERE registroActivo = {0} AND fechaDemanda < '{1}-01-01' AND (fechaCierreDemanda IS NULL OR fechaCierreDemanda LIKE '%{1}%')", 1, anio);

                MainContextEnum objEmpresa = GetEmpresaLogueada();
                List<tblRH_DMS_CapturaDemandas> lstDemandasAniosPasados = _context.Select<tblRH_DMS_CapturaDemandas>(new DapperDTO
                {
                    baseDatos = objEmpresa,
                    consulta = strQuery,
                    parametros = new { registroActivo = true }
                }).ToList();
                cantCierreCadaAnio = lstDemandasAniosPasados.Count();
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetDemandasAbiertasAniosPasados", e, AccionEnum.CONSULTA, 0, new { anio = anio });
                return cantCierreCadaAnio;
            }
            return cantCierreCadaAnio;
        }

        public Dictionary<string, object> FillCboAnios()
        {
            try
            {
                #region FILL COMBO AÑOS | SE CONSULTA LOS AÑOS REGISTRADOS EN BASE A LAS FECHAS DE LAS DEMANDAS
                MainContextEnum objEmpresa = GetEmpresaLogueada();
                List<ComboDTO> lstAniosDemandas = _context.Select<ComboDTO>(new DapperDTO
                {
                    baseDatos = objEmpresa,
                    consulta = @"SELECT DATEPART(yyyy, fechaDemanda) AS VALUE, DATEPART(yyyy, fechaDemanda) AS TEXT
                                        FROM tblRH_DMS_CapturaDemandas
			                                WHERE registroActivo = 1
				                                GROUP BY DATEPART(yyyy, fechaDemanda)
					                                ORDER BY DATEPART(yyyy, fechaDemanda) DESC"
                }).ToList();

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstAniosDemandas);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "FillCboAnios", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboCCDemandasRegistradas()
        {
            try
            {
                #region FILL COMBO AÑOS | SE CONSULTA LOS AÑOS REGISTRADOS EN BASE A LAS FECHAS DE LAS DEMANDAS
                MainContextEnum objEmpresa = GetEmpresaLogueada();
                List<CapturaDTO> lstCC = _context.Select<CapturaDTO>(new DapperDTO
                {
                    baseDatos = objEmpresa,
                    consulta = @"SELECT t1.cc, t2.descripcion AS cc_Descripcion
                                        FROM tblRH_DMS_CapturaDemandas AS t1
		                                INNER JOIN tblP_CC AS t2 ON t2.cc = t1.cc
			                                WHERE t1.registroActivo = @registroActivo AND t1.cc IS NOT NULL AND t2.estatus = @estatus
                                                GROUP BY t1.cc, t2.descripcion
				                                    ORDER BY cc",
                    parametros = new { registroActivo = true, estatus = true }
                }).ToList();

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                foreach (var item in lstCC)
                {
                    if (!string.IsNullOrEmpty(item.cc) && !string.IsNullOrEmpty(item.cc_Descripcion))
                    {
                        objComboDTO = new ComboDTO();
                        objComboDTO.Value = item.cc.ToString();
                        objComboDTO.Text = string.Format("[{0}] {1}", item.cc.Trim().ToUpper(), item.cc_Descripcion.Trim().ToUpper());
                        lstComboDTO.Add(objComboDTO);
                    }
                }

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "FillCboCCDemandasRegistradas", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        #endregion

        #region GENERALES
        private MainContextEnum GetEmpresaLogueada()
        {
            MainContextEnum objEmpresa = new MainContextEnum();
            try
            {
                #region SE OBTIENE MainContextEnum DE LA EMPRESA LOGUEADA
                switch ((int)vSesiones.sesionEmpresaActual)
                {
                    case (int)EmpresaEnum.Construplan:
                        objEmpresa = MainContextEnum.Construplan;
                        break;
                    case (int)EmpresaEnum.Arrendadora:
                        objEmpresa = MainContextEnum.Arrendadora;
                        break;
                    case (int)EmpresaEnum.Colombia:
                        objEmpresa = MainContextEnum.Colombia;
                        break;
                    case (int)EmpresaEnum.Peru:
                        objEmpresa = MainContextEnum.PERU;
                        break;
                    default:
                        break;
                }
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetEmpresaLogueada", e, AccionEnum.CONSULTA, (int)vSesiones.sesionEmpresaActual, new { idEmpresa = (int)vSesiones.sesionEmpresaActual });
                return objEmpresa;
            }
            return objEmpresa;
        }

        private string SetNombreCompleto(string nombre, string ape_paterno, string ape_materno)
        {
            string NombreCompleto = string.Empty;
            try
            {
                #region SE FORMATEA NOMBRE COMPLETO DEL EMPLEADO
                if (!string.IsNullOrEmpty(nombre))
                    NombreCompleto = nombre.Trim();
                if (!string.IsNullOrEmpty(ape_paterno))
                    NombreCompleto += string.Format(" {0}", ape_paterno.Trim());
                if (!string.IsNullOrEmpty(ape_materno))
                    NombreCompleto += string.Format(" {0}", ape_materno.Trim());
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "SetNombreCompleto", e, AccionEnum.CONSULTA, 0, new {nombre = nombre, ape_paterno = ape_paterno, ape_materno = ape_materno});
                return string.Empty;
            }
            return NombreCompleto;
        }

        private string SetCC_Descripcion(string cc, string ccDescripcion)
        {
            string CC_Descripcion = string.Empty;
            try
            {
                #region VALIDACIONES
                if (string.IsNullOrEmpty(cc) || string.IsNullOrEmpty(cc)) throw new Exception("Ocurrió un error al obtener el CC de la persona.");
                #endregion

                #region SE FORMATEA EL CC: [CC] DESCRIPCION
                CC_Descripcion = string.Format("[{0}] {1}", cc, ccDescripcion.Trim());
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "SetCC_Descripcion", e, AccionEnum.CONSULTA, 0, new { cc = cc, ccDescripcion = ccDescripcion });
                return string.Empty;
            }
            return CC_Descripcion;
        }

        private decimal SetSueldoDiario(decimal SalarioBase, decimal Complemento, decimal Bono_Zona, DateTime? FechaBaja, int TipoNomina)
        {
            decimal SalarioDiario = 0;
            try
            {
                #region VALIDACIONES
                if (FechaBaja == null) { throw new Exception("No se encontró la fecha baja del empleado."); }
                #endregion

                #region SE OBTIENE EL SUELDO DIARIO
                decimal SalarioSemanal = 0;
                decimal SalarioQuincenal = 0;
                DateTime fechaBaja = Convert.ToDateTime(FechaBaja);

                switch (TipoNomina)
                {
                    case (int)TipoNominaEnum.semanal:
                        SalarioSemanal = (SalarioBase + Complemento + Bono_Zona);
                        SalarioDiario = SalarioSemanal / 7;
                        break;
                    case (int)TipoNominaEnum.quincenal:
                        SalarioQuincenal = (SalarioBase + Complemento + Bono_Zona);
                        SalarioDiario = SalarioQuincenal / 15;
                        break;
                    default:
                        break;
                }
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "SetSueldoDiario", e, AccionEnum.CONSULTA, 0, new { SalarioBase = SalarioBase, Complemento = Complemento });
                return 0;
            }
            return SalarioDiario;
        }

        private string SetAntiguedad(DateTime? FechaIngreso, DateTime? FechaBaja)
        {
            string Antiguedad = string.Empty;
            try
            {
                #region VALIDACIONES
                string MensajeError = "Ocurrió un error al obtener la antiguedad de la persona.";
                if (FechaIngreso == null) throw new Exception(MensajeError);
                if (FechaBaja == null) throw new Exception(MensajeError);
                #endregion

                #region SE OBTIENE LA ANTIGUEDAD DEL EMPLEADO
                int anios = 0;
                int meses = 0;
                int dias = 0;
                DateTime fechaIngreso = Convert.ToDateTime(FechaIngreso);
                DateTime fechaBaja = Convert.ToDateTime(FechaBaja);

                anios = (fechaBaja.Year - fechaIngreso.Year);
                meses = (fechaBaja.Month - fechaIngreso.Month);
                dias = (fechaBaja.Day - fechaIngreso.Day);

                if (meses < 0) { anios -= 1; meses += 12; }
                if (dias < 0) { meses -= 1; dias += DateTime.DaysInMonth(fechaBaja.Year, fechaBaja.Month); }
                if (anios < 0) throw new Exception(MensajeError);

                if (anios > 0)
                    Antiguedad = string.Format("{0} {1},", anios, anios == 1 ? "año" : "años");
                if (meses > 0)
                    Antiguedad += string.Format("{0} {1},", string.IsNullOrEmpty(Antiguedad) ? meses.ToString() : string.Format(" {0}", meses), meses == 1 ? "mes" : "meses");
                if (dias > 0)
                    Antiguedad += string.Format(" {0} {1}", string.IsNullOrEmpty(Antiguedad) ? dias.ToString() : string.Format(" {0}", dias), dias == 1 ? "día" : "días");
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "SetAntiguedad", e, AccionEnum.CONSULTA, 0, new { FechaIngreso = FechaIngreso, FechaBaja = FechaBaja });
                return string.Empty;
            }
            return Antiguedad;
        }

        public Dictionary<string, object> FillCboCC()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region FILL CBO
                string strQuery = string.Empty;
                List<tblP_CC> lstCC = _context.tblP_CC.ToList();

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                foreach (var item in lstCC)
                {
                    if (!string.IsNullOrEmpty(item.cc) && !string.IsNullOrEmpty(item.descripcion))
                    {
                        objComboDTO = new ComboDTO();
                        objComboDTO.Value = item.cc.ToString();
                        objComboDTO.Text = string.Format("[{0}] {1}", item.cc.Trim(), item.descripcion.Trim());
                        lstComboDTO.Add(objComboDTO);

                        if (string.IsNullOrEmpty(strQuery))
                            strQuery = string.Format("SELECT cc, descripcion FROM cc WHERE cc != '{0}'", item.cc.Trim());
                        else
                            strQuery += string.Format(" AND cc != '{0}'", item.cc.Trim());
                    }
                }

                // SE OBTIENE LISTADO CC DE EK
                var odbc = new OdbcConsultaDTO()
                {
                    consulta = strQuery
                };
                var lstCC_EK = _contextEnkontrol.Select<dynamic>((int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? EnkontrolEnum.CplanRh : (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora ? EnkontrolEnum.ArrenProd : EnkontrolEnum.ColombiaProductivo, odbc);

                foreach (var item in lstCC_EK)
                {
                    if (!string.IsNullOrEmpty(item.cc) && !string.IsNullOrEmpty(item.descripcion))
                    {
                        objComboDTO = new ComboDTO();
                        objComboDTO.Value = item.cc.ToString();
                        objComboDTO.Text = string.Format("[{0}] {1}", item.cc.Trim(), item.descripcion.Trim());
                        lstComboDTO.Add(objComboDTO);
                    }
                }

                // SE ORDENA POR CC
                lstComboDTO = lstComboDTO.OrderBy(o => o.Value).ToList();

                // OTRO (CC CAMPO ABIERTO)
                int valueOtro = -1;
                objComboDTO = new ComboDTO();
                objComboDTO.Value = valueOtro.ToString();
                objComboDTO.Text = "OTRO";
                lstComboDTO.Add(objComboDTO);

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "FillCboCC", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboEstados()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region FILL CBO ESTADOS
                MainContextEnum objEmpresa = GetEmpresaLogueada();
                List<ComboDTO> lstEstados = _context.Select<ComboDTO>(new DapperDTO
                {
                    baseDatos = objEmpresa,
                    consulta = @"SELECT idEstado AS VALUE, estado AS TEXT FROM tblP_Estados ORDER BY estado"
                }).ToList();

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstEstados);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "FillCboEstados", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        private string GetDescripcionCC(string CC)
        {
            string DescripcionCC = string.Empty;
            try
            {
                #region SE OBTIENE LA DESCRIPCIÓN DEL CC
                tblC_Nom_CatalogoCC objCC = _context.tblC_Nom_CatalogoCC.Where(w => w.cc == CC).FirstOrDefault();
                if (objCC == null)
                    throw new Exception("Ocurrió un error al obtener la descripción del CC.");

                DescripcionCC = objCC.ccDescripcion.Trim();
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetDescripcionCC", e, AccionEnum.CONSULTA, 0, new { CC = CC });
                return string.Empty;
            }
            return DescripcionCC;
        }

        private decimal SetPorcentajeExpediente(int claveEmpleado)
        {
            decimal PorcentajeExpediente = 0;
            try
            {
                #region SE OBTIENE EL PORCENTAJE DE EXPEDIENTES DIGITALES EN BASE A LOS ARCHIVOS OBLIGATORIOS
                int CantExpedientesObligatorios = 0;
                List<tblRH_REC_ED_Archivo> lstExpedientesObligatorios = _context.tblRH_REC_ED_Archivo.Where(w => w.archivoObligatorio && w.registroActivo).ToList();
                if (lstExpedientesObligatorios.Count() <= 0)
                    throw new Exception("No se cuenta con expedientes digitales obligatorios.");
                else 
                    CantExpedientesObligatorios = lstExpedientesObligatorios.Count();

                MainContextEnum objEmpresa = GetEmpresaLogueada();
                int CantExpedientesObligatoriosCargados = _context.Select<int>(new DapperDTO
                {
                    baseDatos = objEmpresa,
                    consulta = @"SELECT t1.claveEmpleado
	                                    FROM tblRH_REC_ED_Expediente AS t1
	                                    INNER JOIN tblRH_REC_ED_RelacionExpedienteArchivo AS t2 ON t2.expediente_id = t1.id
	                                    INNER JOIN tblRH_REC_ED_Archivo AS t3 ON t3.id = t2.archivo_id 
		                                    WHERE t1.claveEmpleado = @claveEmpleado AND t1.registroActivo = @registroActivo AND t2.registroActivo = @registroActivo AND t3.archivoObligatorio = @archivoObligatorio",
                    parametros = new { claveEmpleado = claveEmpleado, registroActivo = true, archivoObligatorio = true }
                }).Count();

                if (CantExpedientesObligatorios > 0 && CantExpedientesObligatoriosCargados > 0)
                    PorcentajeExpediente = ((decimal)CantExpedientesObligatoriosCargados / (decimal)CantExpedientesObligatorios) * 100;

                if (PorcentajeExpediente > 100)
                    PorcentajeExpediente = 100;
	            #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "SetPorcentajeExpediente", e, AccionEnum.CONSULTA, 0, new { ClaveEmpleado = claveEmpleado });
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return PorcentajeExpediente;
        }

        public Dictionary<string, object> FillCboEstatus()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region FILL CBO ENUM ESTATUS DEMANDAS
                List<ComboDTO> lstComboDTO = new List<ComboDTO>();

                // ESTATUS CERRADO
                int estatusDemanda = (int)EstatusDemandaEnum.CERRADO;
                ComboDTO objComboDTO = new ComboDTO();
                objComboDTO.Value = estatusDemanda.ToString();
                objComboDTO.Text = EnumHelper.GetDescription((EstatusDemandaEnum.CERRADO));
                lstComboDTO.Add(objComboDTO);

                // ESTATUS ABIERTO
                estatusDemanda = (int)EstatusDemandaEnum.ABIERTO;
                objComboDTO = new ComboDTO();
                objComboDTO.Value = estatusDemanda.ToString();
                objComboDTO.Text = EnumHelper.GetDescription((EstatusDemandaEnum.ABIERTO));
                lstComboDTO.Add(objComboDTO);

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "FillCboEstatus", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboEmpleados()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region FILL COMBO EMPLEADOS DADOS DE BAJA
                MainContextEnum objEmpresa = GetEmpresaLogueada();
                List<tblRH_EK_Empleados> lstEmpleados = _context.Select<tblRH_EK_Empleados>(new DapperDTO
                {
                    baseDatos = objEmpresa,
                    consulta = @"SELECT clave_empleado, nombre, ape_paterno, ape_materno 
	                                    FROM tblRH_EK_Empleados
		                                    WHERE estatus_empleado = @estatus_empleado AND esActivo = @esActivo
                                                ORDER BY nombre, ape_paterno, ape_materno",
                    parametros = new { estatus_empleado = "B", esActivo = true }
                }).ToList();

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                string nombreCompleto = string.Empty;
                foreach (var item in lstEmpleados)
                {
                    nombreCompleto = string.Empty;
                    if (!string.IsNullOrEmpty(item.nombre))
                        nombreCompleto = item.nombre.Trim();
                    if (!string.IsNullOrEmpty(item.ape_paterno))
                        nombreCompleto += string.Format(" {0}", item.ape_paterno.Trim());
                    if (!string.IsNullOrEmpty(item.ape_materno))
                        nombreCompleto += string.Format(" {0}", item.ape_materno.Trim());

                    if (!string.IsNullOrEmpty(nombreCompleto))
                    {
                        objComboDTO = new ComboDTO();
                        objComboDTO.Value = item.clave_empleado.ToString();
                        objComboDTO.Text = nombreCompleto;
                        lstComboDTO.Add(objComboDTO);
                    }
                }

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "FillCboEmpleados", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        #endregion
    }
}
