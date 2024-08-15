using Core.DAO.RecursosHumanos.Evaluacion360;
using Core.DTO;
using Core.DTO.Principal.Generales;
using Core.DTO.RecursosHumanos.Evaluacion360;
using Core.DTO.Utils.Data;
using Core.Entity.Administrativo.Contabilidad.Nomina;
using Core.Entity.Administrativo.RecursosHumanos.Enkontrol;
using Core.Entity.Principal.Multiempresa;
using Core.Entity.Principal.Usuarios;
using Core.Entity.RecursosHumanos.Evaluacion360;
using Core.Enum;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Core.Enum.Principal.Bitacoras;
using Core.Enum.RecursosHumanos.Evaluacion360;
using Data.EntityFramework.Generic;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.RecursosHumanos.Evaluacion360
{
    public class Evaluacion360DAO : GenericDAO<tblP_Usuario>, IEvaluacion360DAO
    {
        private Dictionary<string, object> resultado = new Dictionary<string, object>();
        private const string _NOMBRE_CONTROLADOR = "Evaluacion360Controller";
        private const int _SISTEMA = (int)SistemasEnum.RH;

        #region CATALOGO DE PERSONAL
        public Dictionary<string, object> GetCatalogoPersonal(CatPersonalDTO objPersonalDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE LISTADO DEL PERSONAL REGISTRADO EN EL MODULO EVALUACION 360
                List<tblRH_Eval360_CatPersonal> lstPersonalSIGOPLAN = _context.tblRH_Eval360_CatPersonal.Where(w => w.registroActivo).ToList();

                List<CatPersonalDTO> lstPersonal = new List<CatPersonalDTO>();
                CatPersonalDTO objPersonal = new CatPersonalDTO();
                foreach (var itemPersonal in lstPersonalSIGOPLAN)
                {
                    objPersonal = new CatPersonalDTO();
                    objPersonal.id = itemPersonal.id;
                    objPersonal.nombreCompleto = GetNombreCompletoUsuarioSIGOPLAN(itemPersonal.idUsuario, itemPersonal.idEmpresa);
                    objPersonal.correo = GetCorreoUsuario(itemPersonal.idUsuario, itemPersonal.idEmpresa);
                    objPersonal.descripcionCC = GetCCUsuario(itemPersonal.idUsuario, itemPersonal.idEmpresa);
                    objPersonal.idTipoUsuario = itemPersonal.idTipoUsuario;
                    lstPersonal.Add(objPersonal);
                }


                //                List<CatPersonalDTO> lstPersonal = _context.Select<CatPersonalDTO>(new DapperDTO
                //                {
                //                    baseDatos = MainContextEnum.Construplan,
                //                    consulta = @"SELECT t1.id, t2.nombre, t2.apellidoPaterno, t2.apellidoMaterno, t2.correo, t4.cc, t4.ccDescripcion AS descripcion, t1.idTipoUsuario
                //	                                    FROM tblRH_Eval360_CatPersonal AS t1
                //	                                    INNER JOIN tblP_Usuario AS t2 ON t2.id = t1.idUsuario
                //	                                    INNER JOIN tblRH_EK_Empleados AS t3 ON t2.cveEmpleado = t3.clave_empleado
                //	                                    INNER JOIN tblC_Nom_CatalogoCC AS t4 ON t4.cc = t3.cc_contable
                //	                                    INNER JOIN tblRH_EK_Puestos AS t5 ON t5.puesto = t3.puesto
                //		                                    WHERE t1.registroActivo = @registroActivo AND t2.estatus = @estatus AND t3.estatus_empleado = @estatus_empleado AND t4.estatus = @estatus AND t5.esActivo = @esActivo
                //			                                    ORDER BY nombre",
                //                    parametros = new { registroActivo = true, estatus = true, estatus_empleado = 'A', esActivo = true }
                //                }).ToList();

                // SE FILTRA POR CC
                if (!string.IsNullOrEmpty(objPersonalDTO.cc) && lstPersonal.Count() > 0)
                    lstPersonal = lstPersonal.Where(w => w.cc == objPersonalDTO.cc).ToList();

                // SE FILTRA POR TIPO DE USUARIO
                if (objPersonalDTO.idTipoUsuario > 0 && lstPersonal.Count() > 0)
                    lstPersonal = lstPersonal.Where(w => w.idTipoUsuario == objPersonalDTO.idTipoUsuario).ToList();

                foreach (var item in lstPersonal)
                {
                    //SE OBTIENE LA DESCRIPCIÓN DEL TIPO DE USUARIO
                    item.tipoUsuario = EnumHelper.GetDescription((TipoUsuarioEnum)item.idTipoUsuario);

                    //SE CONCATENA CC Y SU DESCRIPCIÓN
                    item.descripcionCC = SetPrimeraLetraMayuscula(item.descripcionCC);

                    #region SE CONVIERTE EN MAYUSCULA SOLAMENTE LA PRIMERA LETRA POR PALABRA
                    item.nombreCompleto = SetPrimeraLetraMayuscula(item.nombreCompleto);
                    item.descripcionCC = item.descripcionCC;
                    item.tipoUsuario = item.tipoUsuario;
                    #endregion
                }

                resultado.Add(SUCCESS, true);
                resultado.Add("lstPersonal", lstPersonal);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetCatalogoPersonal", e, AccionEnum.CONSULTA, objPersonalDTO.id, objPersonalDTO);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> CECatalogoPersonal(CatPersonalDTO objPersonalDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    if (objPersonalDTO.idUsuario <= 0) { throw new Exception("Es necesario seleccionar a un usuario."); }
                    if (objPersonalDTO.idTipoUsuario <= 0) { throw new Exception("Es necesario seleccionar el tipo de usuario."); }
                    if (objPersonalDTO.idEmpresa <= 0) { throw new Exception("Ocurrió un error al obtener la empresa del usuario."); }
                    #endregion

                    tblRH_Eval360_CatPersonal objCE = _context.tblRH_Eval360_CatPersonal.Where(w => w.id == objPersonalDTO.id).FirstOrDefault();
                    if (objCE == null)
                    {
                        #region SE VERIFICA QUE NO SE DUPLIQUE LA INFORMACIÓN
                        tblRH_Eval360_CatPersonal objPersonal = _context.tblRH_Eval360_CatPersonal.Where(w => w.idUsuario == objPersonalDTO.idUsuario && w.registroActivo).FirstOrDefault();
                        if (objPersonal != null)
                            throw new Exception("Este usuario ya se encuentra registrado.");
                        #endregion

                        #region NUEVO REGISTRO
                        objCE = new tblRH_Eval360_CatPersonal();
                        objCE.idUsuario = objPersonalDTO.idUsuario;
                        objCE.idEmpresa = objPersonalDTO.idEmpresa;
                        objCE.idTipoUsuario = objPersonalDTO.idTipoUsuario;
                        objCE.telefono = !string.IsNullOrEmpty(objPersonalDTO.telefono) ? objPersonalDTO.telefono.Trim() : null;
                        objCE.nivelAcceso = (int)NivelAccesoEnum.REGULAR;
                        objCE.esPrimerEnvioCorreo = true;
                        objCE.idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCE.fechaCreacion = DateTime.Now;
                        objCE.registroActivo = true;
                        _context.tblRH_Eval360_CatPersonal.Add(objCE);
                        _context.SaveChanges();
                        #endregion
                    }
                    else
                    {
                        #region ACTUALIZAR REGISTRO
                        objCE.idTipoUsuario = objPersonalDTO.idTipoUsuario;
                        objCE.telefono = !string.IsNullOrEmpty(objPersonalDTO.telefono) ? objPersonalDTO.telefono.Trim() : null;
                        objCE.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCE.fechaModificacion = DateTime.Now;
                        _context.SaveChanges();
                        #endregion
                    }

                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, objPersonalDTO.id <= 0 ? "Se ha registrado con éxito." : "Se ha actualizado con éxito.");
                    dbContextTransaction.Commit();

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, objPersonalDTO.id <= 0 ? (int)AccionEnum.AGREGAR : (int)AccionEnum.ACTUALIZAR, objPersonalDTO.id, JsonUtils.convertNetObjectToJson(objPersonalDTO));
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "CECatalogoPersonal", e, objPersonalDTO.id <= 0 ? AccionEnum.AGREGAR : AccionEnum.ACTUALIZAR, objPersonalDTO.id, objPersonalDTO);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> EliminarCatalogoPersonal(int idCatalogoPersonal)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    if (idCatalogoPersonal <= 0) { throw new Exception("Ocurrió un error al eliminar el registro."); }

                    #region SE VERIFICA QUE EL USUARIO NO SE ENCUENTRE EN ALGUNA EVALUACIÓN ACTIVA
                    int tieneEvaluacionActiva = _context.Select<int>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Construplan,
                        consulta = @"SELECT TOP(1)t1.id
	                                        FROM tblRH_Eval360_EvaluacionesEvaluador AS t1
	                                        INNER JOIN tblRH_Eval360_EvaluacionesEvaluadorDet AS t2 ON t2.idEvaluacion = t1.id
		                                        WHERE t1.registroActivo = @registroActivo AND t2.registroActivo = @registroActivo AND t1.idPersonalEvaluador = @idPersonalEvaluador",
                        parametros = new { registroActivo = true, idPersonalEvaluador = idCatalogoPersonal }
                    }).FirstOrDefault();
                    if (tieneEvaluacionActiva > 0)
                        throw new Exception("No se puede eliminar al personal, ya que cuenta con evaluaciones respondidas.");
                    #endregion

                    #endregion

                    #region SE ELIMINA AL PERSONAL SELECCIONADO
                    tblRH_Eval360_CatPersonal objEliminar = _context.tblRH_Eval360_CatPersonal.Where(w => w.id == idCatalogoPersonal).FirstOrDefault();
                    if (objEliminar == null)
                        throw new Exception("Ocurrió un error al eliminar el registro.");

                    objEliminar.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                    objEliminar.fechaModificacion = DateTime.Now;
                    objEliminar.registroActivo = false;
                    _context.SaveChanges();
                    #endregion

                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha eliminado con éxito el registro.");
                    dbContextTransaction.Commit();

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, (int)AccionEnum.ELIMINAR, idCatalogoPersonal, JsonUtils.convertNetObjectToJson(idCatalogoPersonal));
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "EliminarCatalogoPersonal", e, AccionEnum.ELIMINAR, idCatalogoPersonal, new { id = idCatalogoPersonal });
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> GetDatosActualizarPersonal(int id)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE LA INFORMACIÓN DEL REGISTRO A ACTUALIZAR
                tblRH_Eval360_CatPersonal objPersonal = _context.tblRH_Eval360_CatPersonal.Where(w => w.id == id).FirstOrDefault();

                resultado.Add(SUCCESS, true);
                resultado.Add("objPersonal", objPersonal);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetDatosActualizarPersonal", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        #endregion

        #region CATALOGO DE CONDUCTAS
        public Dictionary<string, object> GetConductas(CatConductasDTO objFiltroDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE LISTADO DE CONDUCTAS
                List<CatConductasDTO> lstConductas = _context.Select<CatConductasDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT t1.id, t1.descripcionConducta, t2.id AS idCompetencia, t2.idGrupo
	                                    FROM tblRH_Eval360_CatConductas AS t1
	                                    INNER JOIN tblRH_Eval360_CatCompetencias AS t2 ON t2.id = t1.idCompetencia
		                                    WHERE t1.registroActivo = @registroActivo AND t2.registroActivo = @registroActivo",
                    parametros = new { registroActivo = true }
                }).ToList();

                // SE FILTRA POR GRUPO
                if (objFiltroDTO.idGrupo > 0 && lstConductas.Count() > 0)
                    lstConductas = lstConductas.Where(w => w.idGrupo == objFiltroDTO.idGrupo).ToList();

                // SE FILTRA POR COMPETENCIA
                if (objFiltroDTO.idCompetencia > 0 && lstConductas.Count() > 0)
                    lstConductas = lstConductas.Where(w => w.idCompetencia == objFiltroDTO.idCompetencia).ToList();

                // SE CONVIERTE LA DESCRIPCIÓN EN MINISCULA Y LA PRIMERA LETRA POR PALABRA EN MAYUSCULA
                foreach (var item in lstConductas)
                {
                    item.descripcionConducta = item.descripcionConducta;
                }

                resultado.Add(SUCCESS, true);
                resultado.Add("lstConductas", lstConductas);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetConductas", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> CEConducta(CatConductasDTO objConductaDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    if (string.IsNullOrEmpty(objConductaDTO.descripcionConducta)) { throw new Exception("Es necesario indicar la descripción de la conducta."); }
                    if (objConductaDTO.idCompetencia <= 0) { throw new Exception("Es necesario seleccionar la competencia."); }
                    #endregion

                    tblRH_Eval360_CatConductas objCE = _context.tblRH_Eval360_CatConductas.Where(w => w.id == objConductaDTO.id).FirstOrDefault();
                    if (objCE == null)
                    {
                        #region NUEVO REGISTRO
                        objCE = new tblRH_Eval360_CatConductas();
                        objCE.descripcionConducta = objConductaDTO.descripcionConducta.Trim();
                        objCE.idCompetencia = objConductaDTO.idCompetencia;
                        objCE.orden = GetUltimoOrdenConducta();
                        objCE.idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCE.fechaCreacion = DateTime.Now;
                        objCE.registroActivo = true;
                        _context.tblRH_Eval360_CatConductas.Add(objCE);
                        _context.SaveChanges();
                        #endregion
                    }
                    else
                    {
                        #region SE VERIFICA SI LA COMPETENCIA SELECCIONADA, YA SE ENCUENTRA EN UNA EVALUACIÓN YA RESPONDIDA
                        int tieneEvaluacionActiva = _context.Select<int>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.Construplan,
                            consulta = @"SELECT TOP(1)t2.idCompetencia
	                                            FROM tblRH_Eval360_EvaluacionesEvaluadorDet AS t1
	                                            INNER JOIN tblRH_Eval360_CatConductas AS t2 ON t2.id = t1.idConducta
		                                            WHERE t1.registroActivo = @registroActivo AND t2.registroActivo = @registroActivo AND t2.idCompetencia = @idCompetencia",
                            parametros = new { registroActivo = true, idCompetencia = objConductaDTO.idCompetencia }
                        }).Count();
                        if (tieneEvaluacionActiva > 0)
                        {
                            if (objCE.descripcionConducta != objConductaDTO.descripcionConducta.Trim().ToUpper()) { throw new Exception("No se puede actualizar la descripción de la conducta, ya que se encuentra en una evaluación respondida."); };
                            if (objCE.idCompetencia != objConductaDTO.idCompetencia) { throw new Exception("No se puede actualizar la competencia, ya que se encuentra en una evaluación respondida."); };
                        }
                        #endregion

                        #region ACTUALIZAR REGISTRO
                        objCE.descripcionConducta = objConductaDTO.descripcionConducta.Trim();
                        objCE.idCompetencia = objConductaDTO.idCompetencia;
                        objCE.orden = GetUltimoOrdenConducta();
                        objCE.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCE.fechaModificacion = DateTime.Now;
                        _context.SaveChanges();
                        #endregion
                    }

                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, objConductaDTO.id <= 0 ? "Se ha registrado con éxito." : "Se ha actualizado con éxito.");
                    dbContextTransaction.Commit();

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, objConductaDTO.id <= 0 ? (int)AccionEnum.AGREGAR : (int)AccionEnum.ACTUALIZAR, objConductaDTO.id, JsonUtils.convertNetObjectToJson(objConductaDTO));
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "CEConducta", e, objConductaDTO.id <= 0 ? AccionEnum.AGREGAR : AccionEnum.ACTUALIZAR, objConductaDTO.id, objConductaDTO);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> EliminarConducta(int idConducta)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    if (idConducta <= 0) { throw new Exception("Ocurrió un error al eliminar el registro."); }

                    #region SE VERIFICA SI LA CONDUCTA SE ENCUENTRA RELACIONADA A UNA EVALUACIÓN YA RESPONDIDA
                    int tieneEvaluacionActiva = _context.tblRH_Eval360_EvaluacionesEvaluadorDet.Where(w => w.idConducta == idConducta && w.registroActivo).Count();
                    if (tieneEvaluacionActiva > 0)
                        throw new Exception("No se puede eliminar la conducta ya que se encuentra relacionada a una evaluación ya respondida.");
                    #endregion

                    #endregion

                    #region SE ELIMINA LA CONDUCTA SELECCIONADA
                    tblRH_Eval360_CatConductas objEliminar = _context.tblRH_Eval360_CatConductas.Where(w => w.id == idConducta).FirstOrDefault();
                    if (objEliminar == null)
                        throw new Exception("Ocurrió un error al eliminar el registro.");

                    objEliminar.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                    objEliminar.fechaModificacion = DateTime.Now;
                    objEliminar.registroActivo = false;
                    _context.SaveChanges();
                    #endregion

                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha eliminado con éxito el registro.");
                    dbContextTransaction.Commit();

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, (int)AccionEnum.ELIMINAR, idConducta, JsonUtils.convertNetObjectToJson(idConducta));
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "EliminarConducta", e, AccionEnum.ELIMINAR, idConducta, new { idConducta = idConducta });
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> GetDatosActualizarConducta(int id)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (id <= 0) { throw new Exception("Ocurrió un error al obtener la información de la conducta."); }
                #endregion

                #region SE OBTIENE CONDUCTA A ACTUALIZAR
                CatConductasDTO objConducta = _context.Select<CatConductasDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT t1.id, t2.idGrupo, t1.idCompetencia, t2.definicion, t1.descripcionConducta
	                                    FROM tblRH_Eval360_CatConductas AS t1
	                                    INNER JOIN tblRH_Eval360_CatCompetencias AS t2 ON t1.idCompetencia = t2.id
		                                    WHERE t1.id = @id",
                    parametros = new { id = id }
                }).FirstOrDefault();

                if (objConducta != null)
                {
                    if (!string.IsNullOrEmpty(objConducta.definicion))
                        objConducta.definicion = objConducta.definicion;

                    if (!string.IsNullOrEmpty(objConducta.descripcionConducta))
                        objConducta.descripcionConducta = objConducta.descripcionConducta;
                }

                resultado.Add(SUCCESS, true);
                resultado.Add("objConducta", objConducta);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetDatosActualizarConducta", e, AccionEnum.CONSULTA, id, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        #region CATALOGO DE GRUPOS
        public Dictionary<string, object> GetGrupos()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE LISTADO DE GRUPOS
                List<tblRH_Eval360_CatGrupos> lstGrupos = _context.tblRH_Eval360_CatGrupos.Where(w => w.registroActivo).ToList();
                resultado.Add(SUCCESS, true);
                resultado.Add("lstGrupos", lstGrupos);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetGrupos", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> CEGrupo(CatGrupoDTO objGrupoDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    if (string.IsNullOrEmpty(objGrupoDTO.nombreGrupo)) { throw new Exception("Es necesario indicar el nombre del grupo."); }
                    #endregion

                    tblRH_Eval360_CatGrupos objCE = _context.tblRH_Eval360_CatGrupos.Where(w => w.id == objGrupoDTO.id).FirstOrDefault();
                    if (objCE == null)
                    {
                        #region NUEVO REGISTRO
                        objCE = new tblRH_Eval360_CatGrupos();
                        objCE.nombreGrupo = objGrupoDTO.nombreGrupo.Trim();
                        objCE.idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCE.fechaCreacion = DateTime.Now;
                        objCE.registroActivo = true;
                        _context.tblRH_Eval360_CatGrupos.Add(objCE);
                        _context.SaveChanges();
                        #endregion
                    }
                    else
                    {
                        #region SE VERIFICA SI EL GRUPO SELECCIONADO, YA SE ENCUENTRA EN UNA EVALUACIÓN YA RESPONDIDA
                        int tieneEvaluacionActiva = _context.Select<int>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.Construplan,
                            consulta = @"SELECT TOP(1)t3.idGrupo
	                                            FROM tblRH_Eval360_EvaluacionesEvaluadorDet AS t1
	                                            INNER JOIN tblRH_Eval360_CatConductas AS t2 ON t2.id = t1.idConducta
	                                            INNER JOIN tblRH_Eval360_CatCompetencias AS t3 ON t3.id = t2.idCompetencia
		                                            WHERE t1.registroActivo = @registroActivo AND t2.registroActivo = @registroActivo AND t3.registroActivo = @registroActivo AND t3.idGrupo = @idGrupo",
                            parametros = new { registroActivo = true, idGrupo = objGrupoDTO.idGrupo }
                        }).Count();
                        if (tieneEvaluacionActiva > 0)
                            if (objCE.nombreGrupo != objGrupoDTO.nombreGrupo.Trim().ToUpper()) { throw new Exception("No se puede actualizar el nombre del grupo, ya que se encuentra en una evaluación respondida."); }
                        #endregion

                        #region ACTUALIZAR REGISTRO
                        objCE.nombreGrupo = objGrupoDTO.nombreGrupo.Trim();
                        objCE.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCE.fechaModificacion = DateTime.Now;
                        _context.SaveChanges();
                        #endregion
                    }

                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, objGrupoDTO.id <= 0 ? "Se ha registrado con éxito." : "Se ha actualizado con éxito.");
                    dbContextTransaction.Commit();

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, objGrupoDTO.id <= 0 ? (int)AccionEnum.AGREGAR : (int)AccionEnum.ACTUALIZAR, objGrupoDTO.id, JsonUtils.convertNetObjectToJson(objGrupoDTO));
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "CEGrupo", e, objGrupoDTO.id <= 0 ? AccionEnum.CONSULTA : AccionEnum.ACTUALIZAR, objGrupoDTO.id, objGrupoDTO);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> EliminarGrupo(int idGrupo)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    if (idGrupo <= 0) { throw new Exception("Ocurrió un error al eliminar el registro."); }

                    #region SE VERIFICA SI EL GRUPO SE ENCUENTRA RELACIONADO A UN CUESTIONARIO
                    int tieneEvaluacionActiva = _context.Select<int>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Construplan,
                        consulta = @"SELECT TOP(1)t3.idGrupo
	                                        FROM tblRH_Eval360_EvaluacionesEvaluadorDet AS t1
	                                        INNER JOIN tblRH_Eval360_CatConductas AS t2 ON t2.id = t1.idConducta
	                                        INNER JOIN tblRH_Eval360_CatCompetencias AS t3 ON t3.id = t2.idCompetencia
		                                        WHERE t1.registroActivo = @registroActivo AND t2.registroActivo = @registroActivo AND t3.registroActivo = @registroActivo AND t3.idGrupo = @idGrupo",
                        parametros = new { registroActivo = true, idGrupo = idGrupo }
                    }).Count();
                    if (tieneEvaluacionActiva > 0)
                        throw new Exception("No se puede eliminar el grupo ya que se encuentra relacionado a una evaluación ya respondida.");
                    #endregion

                    #endregion

                    tblRH_Eval360_CatGrupos objEliminar = _context.tblRH_Eval360_CatGrupos.Where(w => w.id == idGrupo).FirstOrDefault();
                    if (objEliminar == null)
                        throw new Exception("Ocurrió un error al eliminar el registro.");

                    objEliminar.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                    objEliminar.fechaModificacion = DateTime.Now;
                    objEliminar.registroActivo = false;
                    _context.SaveChanges();

                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha eliminado con éxito el registro");
                    dbContextTransaction.Commit();

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, (int)AccionEnum.ELIMINAR, idGrupo, JsonUtils.convertNetObjectToJson(idGrupo));
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "EliminarGrupo", e, AccionEnum.ELIMINAR, idGrupo, new { idGrupo = idGrupo });
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> GetDatosActualizarGrupo(int id)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE EL GRUPO PARA SER ACTUALIZADO
                tblRH_Eval360_CatGrupos objGrupo = _context.tblRH_Eval360_CatGrupos.Where(w => w.id == id).FirstOrDefault();
                if (objGrupo == null)
                    throw new Exception("Ocurrió un error al obtener la información del grupo.");

                resultado.Add(SUCCESS, true);
                resultado.Add("objGrupo", objGrupo);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetDatosActualizarGrupo", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        #endregion

        #region CATALOGO DE COMPETENCIAS
        public Dictionary<string, object> GetCompetencias(int idGrupo = 0)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE LISTADO DE COMPETENCIAS
                List<tblRH_Eval360_CatCompetencias> lstCompetencias = new List<tblRH_Eval360_CatCompetencias>();
                if (idGrupo > 0)
                    lstCompetencias = _context.tblRH_Eval360_CatCompetencias.Where(w => w.idGrupo == idGrupo && w.registroActivo).ToList();

                resultado.Add(SUCCESS, true);
                resultado.Add("lstCompetencias", lstCompetencias);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetCompetencias", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> CECompetencia(CatCompetenciaDTO objCompetenciaDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    if (objCompetenciaDTO.idGrupo <= 0) { throw new Exception("Es necesario seleccionar un grupo."); }
                    if (string.IsNullOrEmpty(objCompetenciaDTO.nombreCompetencia)) { throw new Exception("Es necesario indicar el nombre de la competencia."); }
                    if (string.IsNullOrEmpty(objCompetenciaDTO.definicion)) { throw new Exception("Es necesario indicar la definición de la competencia."); }
                    #endregion

                    tblRH_Eval360_CatCompetencias objCE = _context.tblRH_Eval360_CatCompetencias.Where(w => w.id == objCompetenciaDTO.id).FirstOrDefault();
                    if (objCE == null)
                    {
                        #region NUEVO REGISTRO
                        objCE = new tblRH_Eval360_CatCompetencias();
                        objCE.idGrupo = objCompetenciaDTO.idGrupo;
                        objCE.nombreCompetencia = objCompetenciaDTO.nombreCompetencia.Trim();
                        objCE.definicion = objCompetenciaDTO.definicion.Trim();
                        objCE.idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCE.fechaCreacion = DateTime.Now;
                        objCE.registroActivo = true;
                        _context.tblRH_Eval360_CatCompetencias.Add(objCE);
                        _context.SaveChanges();
                        #endregion
                    }
                    else
                    {
                        #region SE VERIFICA SI LA COMPETENCIA SELECCIONADA, YA SE ENCUENTRA EN UNA EVALUACIÓN YA RESPONDIDA
                        int tieneEvaluacionActiva = _context.Select<int>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.Construplan,
                            consulta = @"SELECT TOP(1)t2.idCompetencia
	                                            FROM tblRH_Eval360_EvaluacionesEvaluadorDet AS t1
	                                            INNER JOIN tblRH_Eval360_CatConductas AS t2 ON t2.id = t1.idConducta
		                                            WHERE t1.registroActivo = @registroActivo AND t2.registroActivo = @registroActivo AND t2.idCompetencia = @idCompetencia",
                            parametros = new { registroActivo = true, idCompetencia = objCompetenciaDTO.id }
                        }).Count();
                        if (tieneEvaluacionActiva > 0)
                        {
                            if (objCE.idGrupo != objCompetenciaDTO.idGrupo) { throw new Exception("No se puede actualizar el grupo, ya que se encuentra en una evaluación respondida."); }
                            if (objCE.nombreCompetencia != objCompetenciaDTO.nombreCompetencia.Trim().ToUpper()) { throw new Exception("No se puede actualizar la competencia, ya que se encuentra en una evaluación respondida."); }
                        }
                        #endregion

                        #region ACTUALIZAR REGISTRO
                        objCE.idGrupo = objCompetenciaDTO.idGrupo;
                        objCE.nombreCompetencia = objCompetenciaDTO.nombreCompetencia.Trim();
                        objCE.definicion = objCompetenciaDTO.definicion.Trim();
                        objCE.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCE.fechaModificacion = DateTime.Now;
                        _context.SaveChanges();
                        #endregion
                    }

                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, objCompetenciaDTO.id <= 0 ? "Se ha registrado con éxito." : "Se ha actualizado con éxito.");
                    dbContextTransaction.Commit();

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, objCompetenciaDTO.id <= 0 ? (int)AccionEnum.AGREGAR : (int)AccionEnum.ACTUALIZAR, objCompetenciaDTO.id, JsonUtils.convertNetObjectToJson(objCompetenciaDTO));
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "CECompetencia", e, objCompetenciaDTO.id <= 0 ? AccionEnum.AGREGAR : AccionEnum.ACTUALIZAR, objCompetenciaDTO.id, objCompetenciaDTO);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> EliminarCompetencia(int idCompetencia)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    if (idCompetencia <= 0) { throw new Exception("Ocurrió un error al eliminar el registro."); }

                    #region SE VERIFICA SI LA COMPETENCIA SE ENCUENTRA RELACIONADA A UN CUESTIONARIO
                    int tieneEvaluacionActiva = _context.Select<int>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Construplan,
                        consulta = @"SELECT TOP(1)t2.idCompetencia
	                                        FROM tblRH_Eval360_EvaluacionesEvaluadorDet AS t1
	                                        INNER JOIN tblRH_Eval360_CatConductas AS t2 ON t2.id = t1.idConducta
		                                        WHERE t1.registroActivo = @registroActivo AND t2.registroActivo = @registroActivo AND t2.idCompetencia = @idCompetencia",
                        parametros = new { registroActivo = true, idCompetencia = idCompetencia }
                    }).Count();
                    if (tieneEvaluacionActiva > 0)
                        throw new Exception("No se puede eliminar la competencia ya que se encuentra relacionada a una evaluación ya respondida.");
                    #endregion

                    #endregion

                    #region SE ELIMINA LA COMPETENCIA SELECCIONADA
                    tblRH_Eval360_CatCompetencias objEliminar = _context.tblRH_Eval360_CatCompetencias.Where(w => w.id == idCompetencia).FirstOrDefault();
                    if (objEliminar == null)
                        throw new Exception("Ocurrió un error al eliminar el registro.");

                    objEliminar.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                    objEliminar.fechaModificacion = DateTime.Now;
                    objEliminar.registroActivo = false;
                    _context.SaveChanges();
                    #endregion

                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha eliminado con éxito el registro.");
                    dbContextTransaction.Commit();

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, (int)AccionEnum.ELIMINAR, idCompetencia, JsonUtils.convertNetObjectToJson(idCompetencia));
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "EliminarCompetencia", e, AccionEnum.ELIMINAR, idCompetencia, new { idCompetencia = idCompetencia });
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> GetDatosActualizarCompetencia(int id)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (id <= 0) { throw new Exception("Ocurrió un error al obtener la información de la competencia."); }
                #endregion

                #region SE OBTIENE LA INFORMACIÓN DE LA COMPETENCIA A ACTUALIZAR
                tblRH_Eval360_CatCompetencias objCompetencia = _context.tblRH_Eval360_CatCompetencias.Where(w => w.id == id).FirstOrDefault();
                if (objCompetencia == null)
                    throw new Exception("Ocurrió un error al obtener la información de la competencia.");

                resultado.Add(SUCCESS, true);
                resultado.Add("objCompetencia", objCompetencia);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetDatosActualizarCompetencia", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        #endregion

        #endregion

        #region CATALOGO DE PLANTILLAS
        public Dictionary<string, object> GetPlantillas()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE LISTADO DE PLANTILLAS
                List<tblRH_Eval360_CatPlantillas> lstPlantillas = _context.tblRH_Eval360_CatPlantillas.Where(w => w.registroActivo).ToList();

                foreach (var item in lstPlantillas)
                {
                    item.plantilla = item.plantilla;
                    item.descripcion = item.descripcion;
                }

                resultado.Add(SUCCESS, true);
                resultado.Add("lstPlantillas", lstPlantillas);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetPlantillas", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> CEPlantilla(CatPlantillaDTO objPlantillaDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    if (string.IsNullOrEmpty(objPlantillaDTO.plantilla)) { throw new Exception("Es necesario indicar el nombre de la plantilla."); }
                    if (string.IsNullOrEmpty(objPlantillaDTO.descripcion)) { throw new Exception("Es necesario indicar la descripción de la plantilla."); }
                    #endregion

                    tblRH_Eval360_CatPlantillas objCE = _context.tblRH_Eval360_CatPlantillas.Where(w => w.id == objPlantillaDTO.id).FirstOrDefault();
                    if (objCE == null)
                    {
                        #region NUEVO REGISTRO
                        objCE = new tblRH_Eval360_CatPlantillas();
                        objCE.plantilla = objPlantillaDTO.plantilla.Trim();
                        objCE.descripcion = objPlantillaDTO.descripcion.Trim();
                        objCE.idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCE.fechaCreacion = DateTime.Now;
                        objCE.registroActivo = true;
                        _context.tblRH_Eval360_CatPlantillas.Add(objCE);
                        _context.SaveChanges();
                        #endregion
                    }
                    else
                    {
                        #region ACTUALIZAR REGISTRO
                        objCE.plantilla = objPlantillaDTO.plantilla.Trim();
                        objCE.descripcion = objPlantillaDTO.descripcion.Trim();
                        objCE.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCE.fechaModificacion = DateTime.Now;
                        _context.SaveChanges();
                        #endregion
                    }

                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, objPlantillaDTO.id <= 0 ? "Se ha registrado con éxito." : "Se ha actualizado con éxito.");
                    dbContextTransaction.Commit();

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, objPlantillaDTO.id <= 0 ? (int)AccionEnum.AGREGAR : (int)AccionEnum.ACTUALIZAR, objPlantillaDTO.id, JsonUtils.convertNetObjectToJson(objPlantillaDTO));
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "CEPlantilla", e, objPlantillaDTO.id <= 0 ? AccionEnum.AGREGAR : AccionEnum.ACTUALIZAR, objPlantillaDTO.id, objPlantillaDTO);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> EliminarPlantilla(int idPlantilla)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    if (idPlantilla <= 0) { throw new Exception("Ocurrió un error al eliminar la plantilla."); }

                    #region SE VERIFICA SI LA PLANTILLA SE ENCUENTRA RELACIONADA A UN CUESTIONARIO
                    int tieneEvaluacionActiva = _context.Select<int>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Construplan,
                        consulta = @"SELECT TOP(1)t1.id
	                                        FROM tblRH_Eval360_CatPlantillas AS t1
	                                        INNER JOIN tblRH_Eval360_CatCriterios AS t2 ON t2.idPlantilla = t1.id
	                                        INNER JOIN tblRH_Eval360_EvaluacionesEvaluadorDet AS t3 ON t3.idCriterio = t2.id
		                                        WHERE t1.registroActivo = @registroActivo AND t2.registroActivo = @registroActivo AND t3.registroActivo = @registroActivo AND t1.id = @idPlantilla",
                        parametros = new { registroActivo = true, idPlantilla = idPlantilla }
                    }).Count();
                    if (tieneEvaluacionActiva > 0)
                        throw new Exception("No se puede eliminar la plantilla ya que se encuentra relacionada a una evaluación ya respondida.");
                    #endregion

                    #endregion

                    #region SE ELIMINA LA PLANTILLA SELECCIONADA

                    #region SE ELIMINA LOS CRITERIOS RELACIONADOS A LA PLANTILLA
                    List<tblRH_Eval360_CatCriterios> lstCriterios = _context.tblRH_Eval360_CatCriterios.Where(w => w.idPlantilla == idPlantilla && w.registroActivo).ToList();
                    foreach (var item in lstCriterios)
                    {
                        item.registroActivo = false;
                        item.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        item.fechaModificacion = DateTime.Now;
                        _context.SaveChanges();
                    }
                    #endregion

                    tblRH_Eval360_CatPlantillas objEliminar = _context.tblRH_Eval360_CatPlantillas.Where(w => w.id == idPlantilla).FirstOrDefault();
                    objEliminar.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                    objEliminar.fechaModificacion = DateTime.Now;
                    objEliminar.registroActivo = false;
                    _context.SaveChanges();
                    #endregion

                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha eliminado con éxito.");
                    dbContextTransaction.Commit();

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, (int)AccionEnum.ELIMINAR, idPlantilla, JsonUtils.convertNetObjectToJson(idPlantilla));

                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "EliminarPlantilla", e, AccionEnum.ELIMINAR, idPlantilla, new { idPlantilla = idPlantilla });
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> GetDatosActualizarPlantilla(int id)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (id <= 0) { throw new Exception("Ocurrió un error al obtener la información de la plantilla."); }
                #endregion

                #region SE OBTIENE LA INFORMACIÓN DE LA PLANTILLA A ACTUALIZAR
                tblRH_Eval360_CatPlantillas objPlantilla = _context.tblRH_Eval360_CatPlantillas.Where(w => w.id == id).FirstOrDefault();
                if (id <= 0)
                    throw new Exception("Ocurrió un error al obtener la información de la plantilla.");

                resultado.Add(SUCCESS, true);
                resultado.Add("objPlantilla", objPlantilla);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetDatosActualizarPlantilla", e, AccionEnum.CONSULTA, id, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        #region CATALOGO DE CRITERIOS
        public Dictionary<string, object> GetCriterios(int idPlantilla)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (idPlantilla <= 0) { throw new Exception("Ocurrió un error al obtener el listado de criterios."); }
                #endregion

                #region SE OBTIENE LISTADO DE CRITERIOS
                List<CatCriterioDTO> lstCriterios = _context.Select<CatCriterioDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT t1.id, t1.limInferior, t1.limSuperior, t1.etiqueta, t1.descripcionEtiqueta, t2.nombreCuestionario, t1.color
                                        FROM tblRH_Eval360_CatCriterios AS t1
                                        INNER JOIN tblRH_Eval360_Cuestionarios AS t2 ON t1.idCuestionario = t2.id
	                                        WHERE t1.idPlantilla = @idPlantilla AND t1.registroActivo = @registroActivo AND t2.registroActivo = @registroActivo",
                    parametros = new { idPlantilla = idPlantilla, registroActivo = true }
                }).ToList();

                foreach (var item in lstCriterios)
                {
                    item.etiqueta = item.etiqueta;
                    item.nombreCuestionario = item.nombreCuestionario;
                    item.descripcionEtiqueta = item.descripcionEtiqueta;
                }

                resultado.Add(SUCCESS, true);
                resultado.Add("lstCriterios", lstCriterios);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetCriterios", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> CECriterio(CatCriterioDTO objCriterioDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    if ((decimal)objCriterioDTO.limInferior <= -1) { throw new Exception("Es necesario indicar el límite inferior."); }
                    if ((decimal)objCriterioDTO.limSuperior <= -1) { throw new Exception("Es necesario indicar el límite superior."); }
                    if (string.IsNullOrEmpty(objCriterioDTO.etiqueta)) { throw new Exception("Es necesario indicar la etiqueta."); }
                    if (string.IsNullOrEmpty(objCriterioDTO.descripcionEtiqueta)) { throw new Exception("Es necesario indicar la descripción de la etiqueta."); }
                    if (string.IsNullOrEmpty(objCriterioDTO.color)) { throw new Exception("Es necesario indicar el color de la etiqueta."); }
                    if (objCriterioDTO.idCuestionario <= 0) { throw new Exception("Es necesario seleccionar un cuestionario."); }
                    if (objCriterioDTO.idPlantilla <= 0) { throw new Exception("Es necesario seleccionar una plantilla."); }

                    if (objCriterioDTO.id <= 0)
                    {
                        #region SE VERIFICA SI YA CUENTA CON 5 CRITERIOS, YA QUE SON LOS MAXIMOS
                        List<tblRH_Eval360_CatCriterios> lstCantCriterios = _context.tblRH_Eval360_CatCriterios.Where(w => w.idPlantilla == objCriterioDTO.idPlantilla && w.registroActivo).ToList();
                        if (lstCantCriterios.Count() >= 5)
                            throw new Exception("Solamente se permite 5 criterios por plantilla.");
                        #endregion
                    }

                    #region SE VERIFICA SI YA EXISTEN CRITERIOS Y SI VIENE SIENDO EL MISMO CUESTIONARIO A CREAR/EDITAR
                    List<tblRH_Eval360_CatCriterios> lstCriterios = _context.tblRH_Eval360_CatCriterios.Where(w => w.idPlantilla == objCriterioDTO.idPlantilla && w.registroActivo).ToList();
                    if (lstCriterios.Count() > 0)
                    {
                        if (lstCriterios[0].idCuestionario != objCriterioDTO.idCuestionario)
                            throw new Exception("Solo dar de alta los criterios con el mismo cuestionario.");
                    }
                    #endregion

                    #endregion

                    tblRH_Eval360_CatCriterios objCE = _context.tblRH_Eval360_CatCriterios.Where(w => w.id == objCriterioDTO.id).FirstOrDefault();
                    if (objCE == null)
                    {
                        #region NUEVO REGISTRO
                        objCE = new tblRH_Eval360_CatCriterios();
                        objCE.limInferior = objCriterioDTO.limInferior;
                        objCE.limSuperior = objCriterioDTO.limSuperior;
                        objCE.etiqueta = objCriterioDTO.etiqueta.Trim();
                        objCE.descripcionEtiqueta = objCriterioDTO.descripcionEtiqueta.Trim();
                        objCE.color = objCriterioDTO.color.Trim();

                        Color color = ColorTranslator.FromHtml(objCriterioDTO.color.Trim());
                        objCE.R = Convert.ToInt32(color.R);
                        objCE.G = Convert.ToInt32(color.G);
                        objCE.B = Convert.ToInt32(color.B);

                        objCE.idCuestionario = objCriterioDTO.idCuestionario;
                        objCE.idPlantilla = objCriterioDTO.idPlantilla;
                        objCE.idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCE.fechaCreacion = DateTime.Now;
                        objCE.registroActivo = true;
                        _context.tblRH_Eval360_CatCriterios.Add(objCE);
                        _context.SaveChanges();
                        #endregion
                    }
                    else
                    {
                        #region SE VERIFICA SI EL CRITERIO SELECCIONADO, YA SE ENCUENTRA EN UNA EVALUACIÓN YA RESPONDIDA
                        int tieneEvaluacionActiva = _context.tblRH_Eval360_EvaluacionesEvaluadorDet.Where(w => w.idCriterio == objCriterioDTO.id && w.registroActivo).Count();

                        if (tieneEvaluacionActiva > 0)
                        {
                            if (objCE.limInferior != objCriterioDTO.limInferior) { throw new Exception("No se puede actualizar el límite inferior, ya que el criterio se encuentra en una evaluación respondida."); }
                            if (objCE.limSuperior != objCriterioDTO.limSuperior) { throw new Exception("No se puede actualizar el límite superior, ya que el criterio se encuentra en una evaluación respondida."); }
                            if (objCE.etiqueta != objCriterioDTO.etiqueta.Trim().ToUpper()) { throw new Exception("No se puede actualizar la etiqueta, ya que el criterio se encuentra en una evaluación respondida."); }
                            if (objCE.descripcionEtiqueta != objCriterioDTO.descripcionEtiqueta.Trim().ToUpper()) { throw new Exception("No se puede actualizar la descripción, ya que el criterio se encuentra en una evaluación respondida."); }
                            if (objCE.idCuestionario != objCriterioDTO.idCuestionario) { throw new Exception("No se puede actualizar el cuestionario, ya que el criterio se encuentra en una evaluación respondida."); }
                            if (objCE.idPlantilla != objCriterioDTO.idPlantilla) { throw new Exception("No se puede actualizar la plantilla, ya que el criterio se encuentra en una evaluación respondida."); }
                        }
                        #endregion

                        #region ACTUALIZAR REGISTRO
                        objCE.limInferior = objCriterioDTO.limInferior;
                        objCE.limSuperior = objCriterioDTO.limSuperior;
                        objCE.etiqueta = objCriterioDTO.etiqueta.Trim();
                        objCE.descripcionEtiqueta = objCriterioDTO.descripcionEtiqueta.Trim();
                        objCE.color = objCriterioDTO.color.Trim();

                        Color color = ColorTranslator.FromHtml(objCriterioDTO.color.Trim());
                        objCE.R = Convert.ToInt32(color.R);
                        objCE.G = Convert.ToInt32(color.G);
                        objCE.B = Convert.ToInt32(color.B);

                        objCE.idCuestionario = objCriterioDTO.idCuestionario;
                        objCE.idPlantilla = objCriterioDTO.idPlantilla;
                        objCE.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCE.fechaModificacion = DateTime.Now;
                        _context.SaveChanges();
                        #endregion
                    }

                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, objCriterioDTO.id <= 0 ? "Se ha registrado con éxito." : "Se ha actualizado con éxito.");
                    dbContextTransaction.Commit();

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, objCriterioDTO.id <= 0 ? (int)AccionEnum.AGREGAR : (int)AccionEnum.ACTUALIZAR, objCriterioDTO.id, JsonUtils.convertNetObjectToJson(objCriterioDTO));
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "CECriterio", e, objCriterioDTO.id <= 0 ? AccionEnum.AGREGAR : AccionEnum.ACTUALIZAR, objCriterioDTO.id, objCriterioDTO);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> EliminarCriterio(int idCriterio)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    if (idCriterio <= 0) { throw new Exception("Ocurrió un error al eliminar el criterio."); }

                    #region SE VERIFICA SI EL CRITERIO SE ENCUENTRA RELACIONADO A UNA EVALUACIÓN ACTIVA
                    int tieneEvaluacionActiva = _context.tblRH_Eval360_EvaluacionesEvaluadorDet.Where(w => w.idCriterio == idCriterio && w.registroActivo).Count();
                    if (tieneEvaluacionActiva > 0)
                        throw new Exception("No se puede eliminar el criterio ya que se encuentra relacionado a una evaluación ya respondida.");
                    #endregion

                    #endregion

                    #region SE ELIMINA EL CRITERIO SELECCIONADO
                    tblRH_Eval360_CatCriterios objEliminar = _context.tblRH_Eval360_CatCriterios.Where(w => w.id == idCriterio).FirstOrDefault();
                    objEliminar.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                    objEliminar.fechaModificacion = DateTime.Now;
                    objEliminar.registroActivo = false;
                    _context.SaveChanges();
                    #endregion

                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha eliminado con éxito.");
                    dbContextTransaction.Commit();

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, (int)AccionEnum.ELIMINAR, idCriterio, JsonUtils.convertNetObjectToJson(idCriterio));

                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "EliminarCriterio", e, AccionEnum.ELIMINAR, idCriterio, new { idCriterio = idCriterio });
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> GetDatosActualizarCriterio(int id)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (id <= 0) { throw new Exception("Ocurrió un error al obtener la información del criterio."); }
                #endregion

                #region SE OBTIENE INFORMACIÓN DEL CRITERIO A ACTUALIZAR
                tblRH_Eval360_CatCriterios objCriterio = _context.tblRH_Eval360_CatCriterios.Where(w => w.id == id).FirstOrDefault();
                if (objCriterio == null)
                    throw new Exception("Ocurrió un error al obtener la información del criterio.");

                resultado.Add(SUCCESS, true);
                resultado.Add("objCriterio", objCriterio);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetDatosActualizarCriterio", e, AccionEnum.CONSULTA, id, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        #endregion
        #endregion

        #region CATALOGO DE PERIODOS
        public Dictionary<string, object> GetPeriodos()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE LISTADO DE PERIODOS
                List<tblRH_Eval360_CatPeriodos> lstPeriodos = _context.tblRH_Eval360_CatPeriodos.Where(w => w.registroActivo).ToList();

                foreach (var item in lstPeriodos)
                {
                    item.nombrePeriodo = item.nombrePeriodo;
                }

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstPeriodos);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetPeriodos", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> CEPeriodo(CatPeriodosDTO objPeriodoDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    if (string.IsNullOrEmpty(objPeriodoDTO.nombrePeriodo)) { throw new Exception("Es necesario indicar el nombre del periodo."); }
                    if (objPeriodoDTO.fechaCierre != null && objPeriodoDTO.fechaCierre.Year < 2000) { throw new Exception("Es necesario verificar la fecha cierre."); }
                    if (string.IsNullOrEmpty(objPeriodoDTO.estado)) { throw new Exception("Es necesario indicar la etiqueta."); }
                    #endregion

                    tblRH_Eval360_CatPeriodos objCE = _context.tblRH_Eval360_CatPeriodos.Where(w => w.id == objPeriodoDTO.id).FirstOrDefault();
                    if (objCE == null)
                    {
                        #region NUEVO REGISTRO
                        objCE = new tblRH_Eval360_CatPeriodos();
                        objCE.nombrePeriodo = objPeriodoDTO.nombrePeriodo;
                        objCE.fechaCierre = objPeriodoDTO.fechaCierre;
                        objCE.estado = objPeriodoDTO.estado.Trim();
                        objCE.idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCE.fechaCreacion = DateTime.Now;
                        objCE.registroActivo = true;
                        _context.tblRH_Eval360_CatPeriodos.Add(objCE);
                        _context.SaveChanges();
                        #endregion
                    }
                    else
                    {
                        #region SE VERIFICA SI EL PERIODO SELECCIONADO, YA SE ENCUENTRA EN UNA EVALUACIÓN YA RESPONDIDA
                        int tieneEvaluacionActiva = _context.Select<int>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.Construplan,
                            consulta = @"SELECT TOP(1)t1.id 
	                                            FROM tblRH_Eval360_EvaluacionesEvaluador AS t1
	                                            INNER JOIN tblRH_Eval360_EvaluacionesEvaluadorDet AS t2 ON t2.idEvaluacion = t1.id
		                                            WHERE t1.registroActivo = @registroActivo AND t2.registroActivo = @registroActivo AND t1.idPeriodo = @idPeriodo",
                            parametros = new { registroActivo = true, idPeriodo = objPeriodoDTO.id }
                        }).Count();
                        if (tieneEvaluacionActiva > 0)
                        {
                            if (objCE.nombrePeriodo != objPeriodoDTO.nombrePeriodo) { throw new Exception("No se puede actualizar el nombre del periodo, ya que se encuentra en una evaluación respondida."); }
                        }
                        //throw new Exception("No se puede actualizar este registro, ya que la competencia ya se encuentra en una evaluación respondida.");
                        #endregion

                        #region ACTUALIZAR REGISTRO
                        objCE.nombrePeriodo = objPeriodoDTO.nombrePeriodo.Trim();
                        objCE.fechaCierre = objPeriodoDTO.fechaCierre;
                        objCE.estado = objPeriodoDTO.estado.Trim();
                        objCE.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCE.fechaModificacion = DateTime.Now;
                        _context.SaveChanges();
                        #endregion
                    }

                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, objPeriodoDTO.id <= 0 ? "Se ha registrado con éxito." : "Se ha actualizado con éxito.");
                    dbContextTransaction.Commit();

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, objPeriodoDTO.id <= 0 ? (int)AccionEnum.AGREGAR : (int)AccionEnum.ACTUALIZAR, objPeriodoDTO.id, JsonUtils.convertNetObjectToJson(objPeriodoDTO));
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "CEPeriodo", e, objPeriodoDTO.id <= 0 ? AccionEnum.AGREGAR : AccionEnum.ACTUALIZAR, objPeriodoDTO.id, objPeriodoDTO);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> EliminarPeriodo(int idPeriodo)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    if (idPeriodo <= 0) { throw new Exception("Ocurrió un error al eliminar el periodo."); }

                    #region SE VERIFICA SI EL PERIODO SE ENCUENTRA RELACIONADO A UN CUESTIONARIO
                    int tieneEvaluacionActiva = _context.tblRH_Eval360_EvaluacionesEvaluador.Where(w => w.idPeriodo == idPeriodo && w.registroActivo).Count();
                    if (tieneEvaluacionActiva > 0)
                        throw new Exception("No se puede eliminar el periodo ya que se encuentra relacionado a una evaluación ya respondida.");
                    #endregion

                    #endregion

                    #region SE ELIMINA EL PERIODO SELECCIONADO
                    tblRH_Eval360_CatPeriodos objEliminar = _context.tblRH_Eval360_CatPeriodos.Where(w => w.id == idPeriodo).FirstOrDefault();
                    objEliminar.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                    objEliminar.fechaModificacion = DateTime.Now;
                    objEliminar.registroActivo = false;
                    _context.SaveChanges();
                    #endregion

                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha eliminado con éxito.");
                    dbContextTransaction.Commit();

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, (int)AccionEnum.ELIMINAR, idPeriodo, JsonUtils.convertNetObjectToJson(idPeriodo));

                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "EliminarPeriodo", e, AccionEnum.ELIMINAR, idPeriodo, new { idCriterio = idPeriodo });
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> GetDatosActualizarPeriodo(int id)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (id <= 0) { throw new Exception("Ocurrió un error al obtener la información del periodo."); }
                #endregion

                #region SE OBTIENE LA INFORMACIÓN DEL PERIODO A ACTUALIZAR
                tblRH_Eval360_CatPeriodos objPeriodo = _context.tblRH_Eval360_CatPeriodos.Where(w => w.id == id).FirstOrDefault();
                if (objPeriodo == null)
                    throw new Exception("Ocurrió un error al obtener la información del periodo.");

                resultado.Add(SUCCESS, true);
                resultado.Add("objPeriodo", objPeriodo);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetDatosActualizarPeriodo", e, AccionEnum.CONSULTA, id, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        #endregion

        #region CUESTIONARIOS
        public Dictionary<string, object> GetCuestionarios()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE LISTADO DE CUESTIONARIOS
                List<tblRH_Eval360_Cuestionarios> lstCuestionarios = _context.tblRH_Eval360_Cuestionarios.Where(w => w.registroActivo).ToList();

                foreach (var item in lstCuestionarios)
                {
                    item.nombreCuestionario = item.nombreCuestionario;
                }

                resultado.Add("lstCuestionarios", lstCuestionarios);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetCuestionarios", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> CECuestionario(CuestionarioDTO objCuestionarioDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    if (string.IsNullOrEmpty(objCuestionarioDTO.nombreCuestionario)) { throw new Exception("Es necesario indicar el nombre del cuestionario."); }
                    #endregion

                    tblRH_Eval360_Cuestionarios objCE = _context.tblRH_Eval360_Cuestionarios.Where(w => w.id == objCuestionarioDTO.id).FirstOrDefault();
                    if (objCE == null)
                    {
                        #region NUEVO REGISTRO
                        objCE = new tblRH_Eval360_Cuestionarios();
                        objCE.nombreCuestionario = objCuestionarioDTO.nombreCuestionario.Trim();
                        objCE.idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCE.fechaCreacion = DateTime.Now;
                        objCE.registroActivo = true;
                        _context.tblRH_Eval360_Cuestionarios.Add(objCE);
                        _context.SaveChanges();
                        #endregion
                    }
                    else
                    {
                        #region ACTUALIZAR REGISTRO
                        objCE.nombreCuestionario = objCuestionarioDTO.nombreCuestionario.Trim();
                        objCE.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCE.fechaModificacion = DateTime.Now;
                        _context.SaveChanges();
                        #endregion
                    }

                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, objCuestionarioDTO.id <= 0 ? "Se ha registrado con éxito." : "Se ha actualizado con éxito.");
                    dbContextTransaction.Commit();

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, objCuestionarioDTO.id <= 0 ? (int)AccionEnum.AGREGAR : (int)AccionEnum.ACTUALIZAR, objCuestionarioDTO.id, JsonUtils.convertNetObjectToJson(objCuestionarioDTO));
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "CECuestionario", e, objCuestionarioDTO.id <= 0 ? AccionEnum.AGREGAR : AccionEnum.ACTUALIZAR, objCuestionarioDTO.id, objCuestionarioDTO);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> EliminarCuestionario(int idCuestionario)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    if (idCuestionario <= 0) { throw new Exception("Ocurrió un error al eliminar el registro."); }

                    #region SE VERIFICA SI SE ENCUENTRA RELACIONADO A UNA EVALUACIÓN
                    int tieneEvaluacionActiva = _context.tblRH_Eval360_EvaluacionesEvaluador.Where(w => w.idCuestionario == idCuestionario && w.registroActivo).Count();
                    if (tieneEvaluacionActiva > 0)
                        throw new Exception("No se puede eliminar el cuestionario ya se encuentra relacionado a una evaluación ya respondida.");
                    #endregion

                    #endregion

                    #region SE ELIMINA EL REGISTRO SELECCIONADO
                    tblRH_Eval360_Cuestionarios objEliminar = _context.tblRH_Eval360_Cuestionarios.Where(w => w.id == idCuestionario).FirstOrDefault();
                    if (objEliminar == null)
                        throw new Exception("Ocurrió un error al eliminar el registro.");

                    objEliminar.registroActivo = false;
                    objEliminar.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                    objEliminar.fechaModificacion = DateTime.Now;
                    _context.SaveChanges();
                    #endregion

                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha eliminado con éxito el registro.");
                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "EliminarCuestionario", e, AccionEnum.CONSULTA, idCuestionario, 0);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> GetDatosActualizarCuestionario(int id)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE LA INFORMACIÓN DEL CUESTIONARIO A ACTUALIZAR
                tblRH_Eval360_Cuestionarios objCuestionario = _context.tblRH_Eval360_Cuestionarios.Where(w => w.id == id).FirstOrDefault();
                if (objCuestionario == null)
                    throw new Exception("Ocurrió un error al obtener la información del cuestionario.");

                resultado.Add(SUCCESS, true);
                resultado.Add("objCuestionario", objCuestionario);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetDatosActualizarCuestionario", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboCuestionarios()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region FILL CBO CUESTIONARIOS
                List<ComboDTO> lstCuestionariosDTO = _context.Select<ComboDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT id AS VALUE, nombreCuestionario AS TEXT FROM tblRH_Eval360_Cuestionarios WHERE registroActivo = @registroActivo",
                    parametros = new { registroActivo = true }
                });

                foreach (var item in lstCuestionariosDTO)
                {
                    item.Text = item.Text;
                }

                resultado.Add(ITEMS, lstCuestionariosDTO);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "FillCboCuestionarios", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;

        }

        #region CONDUCTAS REL CUESTIONARIOS
        public Dictionary<string, object> GetConductasRelCuestionario(CuestionarioDetDTO objCuestionarioDetDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (objCuestionarioDetDTO.idCuestionario <= 0) { throw new Exception("Ocurrió un error al obtener el listado de conductas del cuestionario seleccionado."); }
                #endregion

                #region SE OBTIENE LISTADO DE CONDUCTAS RELACIONADOS AL CUESTIONARIO SELECCIONADO
                List<CuestionarioDetDTO> lstConductasRelCuestionario = _context.Select<CuestionarioDetDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT t2.id, t1.nombreCuestionario, t3.descripcionConducta, t4.id AS idCompetencia, t4.nombreCompetencia, t4.definicion, t5.id AS idGrupo, t5.nombreGrupo
	                                    FROM tblRH_Eval360_Cuestionarios AS t1
	                                    INNER JOIN tblRH_Eval360_CuestionariosDet AS t2 ON t1.id = t2.idCuestionario
	                                    INNER JOIN tblRH_Eval360_CatConductas AS t3 ON t2.idConducta = t3.id
	                                    INNER JOIN tblRH_Eval360_CatCompetencias AS t4 ON t3.idCompetencia = t4.id
	                                    INNER JOIN tblRH_Eval360_CatGrupos AS t5 ON t4.idGrupo = t5.id
		                                    WHERE t2.idCuestionario = @idCuestionario AND 
                                                  t2.registroActivo = @registroActivo AND 
                                                  t3.registroActivo = @registroActivo AND 
                                                  t4.registroActivo = @registroActivo AND 
                                                  t5.registroActivo = @registroActivo",
                    parametros = new { idCuestionario = objCuestionarioDetDTO.idCuestionario, registroActivo = true }
                }).ToList();

                if (lstConductasRelCuestionario != null && objCuestionarioDetDTO.idGrupo > 0)
                    lstConductasRelCuestionario = lstConductasRelCuestionario.Where(w => w.idGrupo == objCuestionarioDetDTO.idGrupo).ToList();

                if (lstConductasRelCuestionario != null && objCuestionarioDetDTO.idCompetencia > 0)
                    lstConductasRelCuestionario = lstConductasRelCuestionario.Where(w => w.idCompetencia == objCuestionarioDetDTO.idCompetencia).ToList();

                foreach (var item in lstConductasRelCuestionario)
                {
                    item.nombreCuestionario = item.nombreCuestionario;
                    item.nombreGrupo = item.nombreGrupo;
                    item.nombreCompetencia = item.nombreCompetencia;
                    item.descripcionConducta = item.descripcionConducta;
                }

                resultado.Add(SUCCESS, true);
                resultado.Add("lstConductasRelCuestionario", lstConductasRelCuestionario);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetConductasRelCuestionario", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> CrearConductaRelCuestionario(CuestionarioDetDTO objCuestionarioDetDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    if (objCuestionarioDetDTO.lstConductasID.Count() <= 0) { throw new Exception("Es necesario seleccionar al menos una conduta."); }
                    if (objCuestionarioDetDTO.idCuestionario <= 0)
                    {
                        string mensajeError = string.Empty;
                        mensajeError = string.Format("Ocurrió un error al guardar {0}", objCuestionarioDetDTO.lstConductasID.Count() > 1 ? "las conductas seleccionadas." : "la conducta seleccionada.");
                        throw new Exception(mensajeError);
                    }
                    #endregion

                    #region SE VERIFICA QUE LAS CONDUCTAS SELECCIONADAS, NO SE ENCUENTREN EN EL CUESTIONARIO SELECCIONADO
                    List<tblRH_Eval360_CuestionariosDet> lstConductasRelCuestionario = _context.tblRH_Eval360_CuestionariosDet.Where(w => w.idCuestionario == objCuestionarioDetDTO.idCuestionario && w.registroActivo).ToList();
                    foreach (var item in objCuestionarioDetDTO.lstConductasID)
                    {
                        tblRH_Eval360_CuestionariosDet objConductaRelCuestionario = lstConductasRelCuestionario.Where(w => w.idConducta == item).FirstOrDefault();
                        if (objConductaRelCuestionario != null)
                        {
                            tblRH_Eval360_CatConductas objConducta = _context.tblRH_Eval360_CatConductas.Where(w => w.id == item).FirstOrDefault();
                            if (objConducta != null)
                            {
                                string mensajeError =
                                    string.Format("La conducta: {0}, ya se encuentra en el cuestionario, favor de reportar este detalle al departamento de TI.",
                                    !string.IsNullOrEmpty(objConducta.descripcionConducta) ? objConducta.descripcionConducta.Trim().ToUpper() : "ERROR");
                                throw new Exception(mensajeError);
                            }
                        }
                    }
                    #endregion

                    #region NUEVO REGISTRO
                    List<tblRH_Eval360_CuestionariosDet> lstGuardarConductasRelCuestionario = new List<tblRH_Eval360_CuestionariosDet>();
                    tblRH_Eval360_CuestionariosDet objGuardarConductaRelCuestionario = new tblRH_Eval360_CuestionariosDet();
                    foreach (var item in objCuestionarioDetDTO.lstConductasID)
                    {
                        objGuardarConductaRelCuestionario = new tblRH_Eval360_CuestionariosDet();
                        objGuardarConductaRelCuestionario.idCuestionario = objCuestionarioDetDTO.idCuestionario;
                        objGuardarConductaRelCuestionario.idConducta = item;
                        objGuardarConductaRelCuestionario.idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objGuardarConductaRelCuestionario.fechaCreacion = DateTime.Now;
                        objGuardarConductaRelCuestionario.registroActivo = true;
                        lstGuardarConductasRelCuestionario.Add(objGuardarConductaRelCuestionario);
                    }
                    _context.tblRH_Eval360_CuestionariosDet.AddRange(lstGuardarConductasRelCuestionario);
                    _context.SaveChanges();
                    #endregion

                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, objCuestionarioDetDTO.id <= 0 ? "Se ha registrado con éxito." : "Se ha actualizado con éxito.");
                    dbContextTransaction.Commit();

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, objCuestionarioDetDTO.id <= 0 ? (int)AccionEnum.AGREGAR : (int)AccionEnum.ACTUALIZAR, objCuestionarioDetDTO.id, JsonUtils.convertNetObjectToJson(objCuestionarioDetDTO));
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "CrearConductaRelCuestionario", e, AccionEnum.CONSULTA, objCuestionarioDetDTO.id, objCuestionarioDetDTO);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> EliminarConductaRelCuestionario(int idConductaRelCuestonario)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    if (idConductaRelCuestonario <= 0) { throw new Exception("Ocurrió un error al eliminar el registro seleccionado."); }
                    #endregion

                    #region SE ELIMINA EL REGISTRO SELECCIONADO
                    tblRH_Eval360_CuestionariosDet objEliminar = _context.tblRH_Eval360_CuestionariosDet.Where(w => w.id == idConductaRelCuestonario).FirstOrDefault();
                    if (objEliminar == null)
                        throw new Exception("Ocurrió un error al eliminar el registro seleccionado.");

                    objEliminar.registroActivo = false;
                    objEliminar.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                    objEliminar.fechaModificacion = DateTime.Now;
                    _context.SaveChanges();
                    #endregion

                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha eliminado con éxito el registro.");
                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "EliminarConductaRelCuestionario", e, AccionEnum.CONSULTA, 0, 0);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }
        #endregion

        #region CONDUCTAS DISPONIBLES PARA LIGARLOS A UN CUESTIONARIO
        public Dictionary<string, object> GetConductasDisponibles(CuestionarioDetDTO objCuestionarioDetDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (objCuestionarioDetDTO.idCuestionario <= 0) { throw new Exception("Ocurrió un error al obtener el listado de conductas disponibles."); }
                #endregion

                #region SE OBTIENE LISTADO DE CONDUCTAS DISPONIBLES (LOS QUE NO SE ENCUENTRAN ASIGNADOS YA AL CUESTIONARIO SELECCIONADO)

                // SE OBTIENE LISTADO DE CONDUCTAS YA ASIGNADAS EN EL CUESTIONARIO SELECCIONADO
                List<tblRH_Eval360_CuestionariosDet> lstCondutasNoDisponiblesRelCuestionario = _context.tblRH_Eval360_CuestionariosDet.Where(w => w.idCuestionario == objCuestionarioDetDTO.idCuestionario && w.registroActivo).ToList();
                List<int> lstCondutasNoDisponibles_ID = new List<int>();
                foreach (var item in lstCondutasNoDisponiblesRelCuestionario)
                {
                    if (item.idConducta > 0)
                        lstCondutasNoDisponibles_ID.Add(item.idConducta);
                }

                // SE OBTIENE LISTADO DE CONDUCTAS DISPONIBLES PARA ASIGNAR AL CUESTIONARIO SELECCIONADO
                List<CuestionarioDetDTO> lstConductasDisponibles = _context.Select<CuestionarioDetDTO>(new DapperDTO
                {
                    baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                    consulta = @"SELECT t1.id, t1.descripcionConducta, t1.idCompetencia, t2.idGrupo
	                                    FROM tblRH_Eval360_CatConductas AS t1
	                                    INNER JOIN tblRH_Eval360_CatCompetencias AS t2 ON t2.id = t1.idCompetencia
		                                    WHERE t1.registroActivo = @registroActivo AND t2.registroActivo = @registroActivo",
                    parametros = new { registroActivo = true }
                }).ToList();

                lstConductasDisponibles = lstConductasDisponibles.Where(w => !lstCondutasNoDisponibles_ID.Contains(w.id)).ToList();

                foreach (var item in lstConductasDisponibles)
                {
                    item.descripcionConducta = item.descripcionConducta;
                }

                #region FILTROS
                // SE FILTRA POR GRUPO
                if (lstConductasDisponibles.Count() > 0 && objCuestionarioDetDTO.idGrupo > 0)
                    lstConductasDisponibles = lstConductasDisponibles.Where(w => w.idGrupo == objCuestionarioDetDTO.idGrupo).ToList();

                // SE FILTRA POR COMPETENCIA
                if (lstConductasDisponibles.Count() > 0 && objCuestionarioDetDTO.idCompetencia > 0)
                    lstConductasDisponibles = lstConductasDisponibles.Where(w => w.idCompetencia == objCuestionarioDetDTO.idCompetencia).ToList();
                #endregion

                resultado.Add(SUCCESS, true);
                resultado.Add("lstConductasDisponibles", lstConductasDisponibles);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetConductasDisponibles", e, AccionEnum.CONSULTA, objCuestionarioDetDTO.id, objCuestionarioDetDTO);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        #endregion
        #endregion

        #region RELACIONES
        public Dictionary<string, object> GetRelaciones(RelacionDTO objRelacionDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (objRelacionDTO.idPeriodo <= 0) { throw new Exception("Es necesario indicar el periodo."); }
                #endregion

                #region SE OBTIENE LISTADO DE RELACIONES EN BASE AL PERIODO SELECCIONADO

                // SE OBTIENE TODAS LAS RELACIONES DEL PERIODO SELECCIONADO
                List<RelacionDTO> lstRelaciones = _context.Select<RelacionDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT t1.id, t1.idPersonalEvaluado, t4.nombre, t4.apellidoPaterno, t4.apellidoMaterno, t5.id AS idCuestionario, t5.nombreCuestionario, t2.idPersonalEvaluador, t3.idEmpresa
	                                    FROM tblRH_Eval360_Relaciones AS t1
	                                    INNER JOIN tblRH_Eval360_RelacionesDet AS t2 ON t2.idRelacion = t1.id
	                                    INNER JOIN tblRH_Eval360_CatPersonal AS t3 ON t3.id = t2.idPersonalEvaluador
	                                    INNER JOIN tblP_Usuario AS t4 ON t4.id = t3.idUsuario
	                                    INNER JOIN tblRH_Eval360_Cuestionarios AS t5 ON t5.id = t2.idCuestionario
		                                    WHERE t1.registroActivo = @registroActivo AND t2.registroActivo = @registroActivo AND t3.registroActivo = @registroActivo AND 
                                                  t4.estatus = @estatus AND t5.registroActivo = @registroActivo AND t2.tipoRelacion = @tipoRelacion AND t1.idPeriodo = @idPeriodo",
                    parametros = new { registroActivo = true, estatus = 1, tipoRelacion = (int)TipoRelacionEnum.AUTOEVALUACION, idPeriodo = objRelacionDTO.idPeriodo }
                }).ToList();

                // SE CONSTRUYE TABLA PARA MOSTRAR QUIENES SON LOS EVALUADORES POR CADA EVALUADO
                List<RelacionDTO> lstDTO = new List<RelacionDTO>();
                RelacionDTO objDTO = new RelacionDTO();
                string nombreCompleto = string.Empty;
                foreach (var item in lstRelaciones)
                {
                    objDTO = new RelacionDTO();
                    objDTO.id = item.id;
                    objDTO.idPersonalEvaluado = item.idPersonalEvaluado;

                    nombreCompleto = string.Empty;
                    if (!string.IsNullOrEmpty(item.nombre))
                        nombreCompleto = item.nombre.Trim();

                    if (!string.IsNullOrEmpty(item.apellidoPaterno))
                        nombreCompleto += string.Format(" {0}", item.apellidoPaterno.Trim().ToUpper());

                    if (!string.IsNullOrEmpty(item.apellidoMaterno))
                        nombreCompleto += string.Format(" {0}", item.apellidoMaterno.Trim().ToUpper());

                    objDTO.nombreEvaluado = SetPrimeraLetraMayuscula(nombreCompleto);
                    objDTO.nombreCuestionario = item.nombreCuestionario;
                    objDTO.nombreAutoevaluacion = string.Format("<button class='btn btn-xs btn-info'>{0}</button><br><button class='btn btn-xs btn-warning'>{1}</button>", objDTO.nombreEvaluado, objDTO.nombreCuestionario);
                    objDTO.idPersonalEvaluador = item.idPersonalEvaluador;

                    #region SE OBTIENE LISTADO DE EVALUADORES DEL EVALUADO
                    List<tblRH_Eval360_RelacionesDet> lstEvaluadores = _context.tblRH_Eval360_RelacionesDet.Where(w => w.idRelacion == item.id && w.registroActivo).ToList();
                    foreach (var item2 in lstEvaluadores)
                    {
                        switch (item2.tipoRelacion)
                        {
                            case (int)TipoRelacionEnum.CLIENTES_INTERNOS:
                                #region CLIENTES INTERNOS
                                if (string.IsNullOrEmpty(objDTO.lstEvaluadores_CLIENTES_INTERNOS))
                                    objDTO.lstEvaluadores_CLIENTES_INTERNOS = string.Format("<button class='btn btn-xs btn-info'>{0}</button>", GetNombreCompletoUsuarioSIGOPLAN(GetUsuarioID(item2.idPersonalEvaluador), item.idEmpresa));
                                else
                                    objDTO.lstEvaluadores_CLIENTES_INTERNOS += string.Format("<br><button class='btn btn-xs btn-info'>{0}</button>", GetNombreCompletoUsuarioSIGOPLAN(GetUsuarioID(item2.idPersonalEvaluador), item.idEmpresa));
                                #endregion
                                break;
                            case (int)TipoRelacionEnum.COLABORADORES:
                                #region COLABORADORES
                                if (string.IsNullOrEmpty(objDTO.lstEvaluadores_COLABORADORES))
                                    objDTO.lstEvaluadores_COLABORADORES = string.Format("<button class='btn btn-xs btn-info'>{0}</button>", GetNombreCompletoUsuarioSIGOPLAN(GetUsuarioID(item2.idPersonalEvaluador), item.idEmpresa));
                                else
                                    objDTO.lstEvaluadores_COLABORADORES += string.Format("<br><button class='btn btn-xs btn-info'>{0}</button>", GetNombreCompletoUsuarioSIGOPLAN(GetUsuarioID(item2.idPersonalEvaluador), item.idEmpresa));
                                #endregion
                                break;
                            case (int)TipoRelacionEnum.JEFE:
                                #region JEFE
                                if (string.IsNullOrEmpty(objDTO.lstEvaluadores_JEFE))
                                    objDTO.lstEvaluadores_JEFE = string.Format("<button class='btn btn-xs btn-info'>{0}</button>", GetNombreCompletoUsuarioSIGOPLAN(GetUsuarioID(item2.idPersonalEvaluador), item.idEmpresa));
                                else
                                    objDTO.lstEvaluadores_JEFE += string.Format("<br><button class='btn btn-xs btn-info'>{0}</button>", GetNombreCompletoUsuarioSIGOPLAN(GetUsuarioID(item2.idPersonalEvaluador), item.idEmpresa));
                                #endregion
                                break;
                            case (int)TipoRelacionEnum.PARES:
                                #region PARES
                                if (string.IsNullOrEmpty(objDTO.lstEvaluadores_PARES))
                                    objDTO.lstEvaluadores_PARES = string.Format("<button class='btn btn-xs btn-info'>{0}</button>", GetNombreCompletoUsuarioSIGOPLAN(GetUsuarioID(item2.idPersonalEvaluador), item.idEmpresa));
                                else
                                    objDTO.lstEvaluadores_PARES += string.Format("<br><button class='btn btn-xs btn-info'>{0}</button>", GetNombreCompletoUsuarioSIGOPLAN(GetUsuarioID(item2.idPersonalEvaluador), item.idEmpresa));
                                #endregion
                                break;
                            default:
                                break;
                        }
                    }
                    #endregion

                    lstDTO.Add(objDTO);
                }

                resultado.Add(SUCCESS, true);
                resultado.Add("lstDTO", lstDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetRelaciones", e, AccionEnum.CONSULTA, objRelacionDTO.id, objRelacionDTO);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> CERelacion(RelacionDTO objRelacionDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    string mensajeErrorCE = string.Format("Ocurrió un error al {0} la información.", objRelacionDTO.id > 0 ? "actualizar" : "registrar");
                    if (objRelacionDTO.idPeriodo <= 0) { throw new Exception("Es necesario seleccionar un periodo."); }
                    if (objRelacionDTO.idPersonalEvaluado <= 0 && objRelacionDTO.lstPersonalID.Count() <= 0) { throw new Exception("Es necesario seleccionar al personal que sera evaluado."); }
                    foreach (var item in objRelacionDTO.lstPersonalID) { if (item <= 0) { throw new Exception(mensajeErrorCE); } }
                    #endregion

                    if (objRelacionDTO.idPersonalEvaluado > 0 && objRelacionDTO.lstPersonalID.Count() <= 0)
                    {
                        #region ACCIONES DESDE EL MODULO DE RELACIONES
                        tblRH_Eval360_Relaciones objCE = _context.tblRH_Eval360_Relaciones.Where(w => w.id == objRelacionDTO.id).FirstOrDefault();
                        if (objCE == null)
                        {
                            #region NUEVO REGISTRO | TABLA PRINCIPAL
                            objCE = new tblRH_Eval360_Relaciones();
                            objCE.idPeriodo = objRelacionDTO.idPeriodo;
                            objCE.idPersonalEvaluado = objRelacionDTO.idPersonalEvaluado;
                            objCE.idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                            objCE.fechaCreacion = DateTime.Now;
                            objCE.registroActivo = true;
                            _context.tblRH_Eval360_Relaciones.Add(objCE);
                            _context.SaveChanges();
                            #endregion

                            #region NUEVO REGISTRO | TABLA DETALLE

                            // SE OBTIENE EL ID DEL REGISTRO QUE SE ACABA DE REALIZAR
                            tblRH_Eval360_Relaciones objRelacion = _context.tblRH_Eval360_Relaciones.OrderByDescending(o => o.id).FirstOrDefault();
                            if (objRelacion == null)
                                throw new Exception("Ocurrió un error al momento de realizar el registro.");

                            // SE REGISTRA EL EVALUADO EN EL DETALLE COMO AUTOEVALUACIÓN
                            tblRH_Eval360_RelacionesDet objCEDet = new tblRH_Eval360_RelacionesDet();
                            objCEDet.idRelacion = objRelacion.id;
                            objCEDet.idPersonalEvaluador = objRelacionDTO.idPersonalEvaluado;
                            objCEDet.tipoRelacion = (int)TipoRelacionEnum.AUTOEVALUACION;
                            objCEDet.idCuestionario = objRelacionDTO.idCuestionario;
                            objCEDet.seEnvioCorreo = false;
                            objCEDet.idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                            objCEDet.fechaCreacion = DateTime.Now;
                            objCEDet.registroActivo = true;
                            _context.tblRH_Eval360_RelacionesDet.Add(objCEDet);
                            _context.SaveChanges();
                            #endregion

                            #region NUEVO REGISTRO | TABLA EVALUACIONES
                            // SE REGISTRA QUE EL EVALUADO TIENE UNA AUTOEVALUACION PENDIENTE POR REALIZAR
                            tblRH_Eval360_EvaluacionesEvaluador objEvaluacion = new tblRH_Eval360_EvaluacionesEvaluador();
                            objEvaluacion.idPeriodo = objRelacionDTO.idPeriodo;
                            objEvaluacion.idPersonalEvaluado = objRelacionDTO.idPersonalEvaluado;
                            objEvaluacion.idPersonalEvaluador = objRelacionDTO.idPersonalEvaluado;
                            objEvaluacion.idCuestionario = objRelacionDTO.idCuestionario;
                            objEvaluacion.idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                            objEvaluacion.fechaCreacion = DateTime.Now;
                            objEvaluacion.registroActivo = true;
                            _context.tblRH_Eval360_EvaluacionesEvaluador.Add(objEvaluacion);
                            _context.SaveChanges();
                            #endregion
                        }
                        else
                        {
                            #region ACTUALIZAR REGISTRO
                            objCE.idPeriodo = objRelacionDTO.idPeriodo;
                            objCE.idPersonalEvaluado = objRelacionDTO.idPersonalEvaluado;
                            objCE.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                            objCE.fechaModificacion = DateTime.Now;
                            _context.SaveChanges();
                            #endregion
                        }
                        #endregion
                    }
                    else if (objRelacionDTO.idPersonalEvaluado <= 0 && objRelacionDTO.lstPersonalID.Count() > 0)
                    {
                        #region ACCIONES DESDE EL MODULO DE PERSONAL
                        objRelacionDTO.registroConExito = false;
                        foreach (var item in objRelacionDTO.lstPersonalID)
                        {
                            objRelacionDTO.idPersonalEvaluado = item;

                            #region SE VERIFICA QUE EL EVALUADO NO SE ENCUENTRE REGISTRADO EN EL PERIODO SELECCIONADO
                            tblRH_Eval360_Relaciones objPersonalRelPeriodo = _context.tblRH_Eval360_Relaciones
                                .Where(w => w.idPeriodo == objRelacionDTO.idPeriodo && w.idPersonalEvaluado == objRelacionDTO.idPersonalEvaluado && w.registroActivo).FirstOrDefault();
                            if (objPersonalRelPeriodo == null)
                            {
                                #region NUEVO REGISTRO | TABLA PRINCIPAL
                                tblRH_Eval360_Relaciones objCE = new tblRH_Eval360_Relaciones();
                                objCE.idPeriodo = objRelacionDTO.idPeriodo;
                                objCE.idPersonalEvaluado = objRelacionDTO.idPersonalEvaluado;
                                objCE.idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                                objCE.fechaCreacion = DateTime.Now;
                                objCE.registroActivo = true;
                                _context.tblRH_Eval360_Relaciones.Add(objCE);
                                _context.SaveChanges();
                                #endregion

                                #region NUEVO REGISTRO | TABLA DETALLE
                                // SE OBTIENE EL ID DEL REGISTRO QUE SE ACABA DE REALIZAR
                                tblRH_Eval360_Relaciones objRelacion = _context.tblRH_Eval360_Relaciones.OrderByDescending(o => o.id).FirstOrDefault();
                                if (objRelacion == null)
                                    throw new Exception("Ocurrió un error al momento de realizar el registro.");

                                // SE REGISTRA EL EVALUADO EN EL DETALLE COMO AUTOEVALUACIÓN
                                tblRH_Eval360_RelacionesDet objCEDet = new tblRH_Eval360_RelacionesDet();
                                objCEDet.idRelacion = objRelacion.id;
                                objCEDet.idPersonalEvaluador = objRelacionDTO.idPersonalEvaluado;
                                objCEDet.tipoRelacion = (int)TipoRelacionEnum.AUTOEVALUACION;
                                objCEDet.idCuestionario = objRelacionDTO.idCuestionario;
                                objCEDet.seEnvioCorreo = false;
                                objCEDet.idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                                objCEDet.fechaCreacion = DateTime.Now;
                                objCEDet.registroActivo = true;
                                _context.tblRH_Eval360_RelacionesDet.Add(objCEDet);
                                _context.SaveChanges();
                                #endregion

                                #region NUEVO REGISTRO | TABLA EVALUACIONES
                                // SE REGISTRA QUE EL EVALUADO TIENE UNA AUTOEVALUACION PENDIENTE POR REALIZAR
                                tblRH_Eval360_EvaluacionesEvaluador objEvaluacion = new tblRH_Eval360_EvaluacionesEvaluador();
                                objEvaluacion.idPeriodo = objRelacionDTO.idPeriodo;
                                objEvaluacion.idPersonalEvaluado = objRelacionDTO.idPersonalEvaluado;
                                objEvaluacion.idPersonalEvaluador = objRelacionDTO.idPersonalEvaluado;
                                objEvaluacion.idCuestionario = objRelacionDTO.idCuestionario;
                                objEvaluacion.idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                                objEvaluacion.fechaCreacion = DateTime.Now;
                                objEvaluacion.registroActivo = true;
                                _context.tblRH_Eval360_EvaluacionesEvaluador.Add(objEvaluacion);
                                _context.SaveChanges();
                                #endregion

                                objRelacionDTO.registroConExito = true;
                            }
                            #endregion
                        }
                        #endregion
                    }


                    if (objRelacionDTO.id > 0 && objRelacionDTO.lstPersonalID.Count() <= 0)
                        resultado.Add(MESSAGE, objRelacionDTO.id <= 0 ? "Se ha registrado con éxito." : "Se ha actualizado con éxito.");
                    else if (objRelacionDTO.id <= 0 && objRelacionDTO.lstPersonalID.Count() > 0 && objRelacionDTO.registroConExito)
                        resultado.Add(MESSAGE, "Se ha registrado con éxito en el módulo relaciones.");
                    else if (objRelacionDTO.id <= 0 && objRelacionDTO.lstPersonalID.Count() > 0 && !objRelacionDTO.registroConExito)
                        throw new Exception("El personal seleccionado ya se encuentra registrado en el periodo seleccionado.");

                    resultado.Add(SUCCESS, true);
                    dbContextTransaction.Commit();

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, objRelacionDTO.id <= 0 ? (int)AccionEnum.AGREGAR : (int)AccionEnum.ACTUALIZAR, objRelacionDTO.id, JsonUtils.convertNetObjectToJson(objRelacionDTO));
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "CERelacion", e, objRelacionDTO.id <= 0 ? AccionEnum.AGREGAR : AccionEnum.ACTUALIZAR, objRelacionDTO.id, objRelacionDTO);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> EliminarRelacion(int id)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    string mensajeError = "Ocurrió un error al eliminar el registro.";
                    if (id <= 0) { throw new Exception(mensajeError); }
                    #endregion

                    #region SE ELIMINA EL REGISTRO SELECCIONADO
                    tblRH_Eval360_Relaciones objEliminar = _context.tblRH_Eval360_Relaciones.Where(w => w.id == id).FirstOrDefault();
                    if (objEliminar == null)
                        throw new Exception(mensajeError);

                    objEliminar.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                    objEliminar.fechaModificacion = DateTime.Now;
                    objEliminar.registroActivo = false;
                    _context.SaveChanges();

                    // SE ELIMINA EL DETALLE DE LA RELACIÓN
                    List<tblRH_Eval360_RelacionesDet> lstEliminarDet = _context.tblRH_Eval360_RelacionesDet.Where(w => w.idRelacion == id).ToList();
                    foreach (var item in lstEliminarDet)
                    {
                        item.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        item.fechaModificacion = DateTime.Now;
                        item.registroActivo = false;
                    }
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha eliminado con éxito el registro.");

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, (int)AccionEnum.ELIMINAR, id, JsonUtils.convertNetObjectToJson(id));
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "EliminarRelacion", e, AccionEnum.ELIMINAR, id, new { id = id });
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
                return resultado;
            }
        }

        public Dictionary<string, object> FillCboPersonalRelRelacionDisponibles(int idPeriodo)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (idPeriodo <= 0) { throw new Exception("Es necesario seleccionar un periodo."); }
                #endregion

                #region SE OBTIENE PERSONAL QUE NO SE ENCUENTRA REGISTRADO EN EL PERIODO ACTUAL

                // LISTADO DE PERSONAL REGISTRADO EN EL PERIODO SELECCIONADO
                List<int> lstPersonalRelPeriodo_ID = _context.Select<int>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT idPersonalEvaluado FROM tblRH_Eval360_Relaciones WHERE registroActivo = @registroActivo AND idPeriodo = @idPeriodo",
                    parametros = new { registroActivo = true, idPeriodo = idPeriodo }
                }).ToList();

                // LISTADO DE PERSONAL DISPONIBLE PARA REGISTRAR EN EL PERIODO SELECCIONADO
                List<CatPersonalDTO> lstPersonalDisponible = _context.Select<CatPersonalDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT t2.id, t1.nombre, t1.apellidoPaterno, t1.apellidoMaterno
	                                    FROM tblP_Usuario AS t1
	                                    INNER JOIN tblRH_Eval360_CatPersonal AS t2 ON t1.id = t2.idUsuario
		                                    WHERE t1.estatus = @estatus AND t2.registroActivo = @registroActivo",
                    parametros = new { estatus = 1, registroActivo = true }
                }).ToList();
                lstPersonalDisponible = lstPersonalDisponible.Where(w => !lstPersonalRelPeriodo_ID.Contains(w.id)).ToList();

                // SE CONTRUYE COMBO
                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                foreach (var item in lstPersonalDisponible)
                {
                    string nombreCompleto = string.Empty;
                    if (!string.IsNullOrEmpty(item.nombre))
                        nombreCompleto = item.nombre.Trim();

                    if (!string.IsNullOrEmpty(item.apellidoPaterno))
                        nombreCompleto += string.Format(" {0}", item.apellidoPaterno.Trim().ToUpper());

                    if (!string.IsNullOrEmpty(item.apellidoMaterno))
                        nombreCompleto += string.Format(" {0}", item.apellidoMaterno.Trim().ToUpper());

                    objComboDTO = new ComboDTO();
                    objComboDTO.Value = item.id.ToString();
                    objComboDTO.Text = nombreCompleto;
                    lstComboDTO.Add(objComboDTO);
                }

                foreach (var item in lstComboDTO)
                {
                    item.Text = item.Text;
                }

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "FillCboPersonalRelRelacionDisponibles", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboEvaluadores(List<int> lstEvaluadores_ID)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE LISTADO DE EVALUADORES
                List<CatPersonalDTO> lstEvaluadores = _context.Select<CatPersonalDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT t2.id, t1.nombre, t1.apellidoPaterno, t1.apellidoMaterno
	                                    FROM tblP_Usuario AS t1
	                                    INNER JOIN tblRH_Eval360_CatPersonal AS t2 ON t1.id = t2.idUsuario
		                                    WHERE t1.estatus = @estatus AND t2.registroActivo = @registroActivo AND t2.idTipoUsuario = @idTipoUsuario",
                    parametros = new { estatus = 1, registroActivo = true, idTipoUsuario = (int)TipoUsuarioEnum.EVALUADOR }
                }).ToList();

                // EN CASO QUE EL EVALUADO YA CUENTE CON EVALUADORES, SE ELIMINA DEL LISTADO A DICHO EVALUADOR, PARA SOLO MOSTRAR LOS DISPONIBLES
                //if (lstEvaluadores_ID.Count() > 0)
                //    lstEvaluadores = lstEvaluadores.Where(w => !lstEvaluadores_ID.Contains(w.id)).ToList();

                // SE CONTRUYE COMBO
                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                foreach (var item in lstEvaluadores)
                {
                    string nombreCompleto = string.Empty;
                    if (!string.IsNullOrEmpty(item.nombre))
                        nombreCompleto = item.nombre.Trim();

                    if (!string.IsNullOrEmpty(item.apellidoPaterno))
                        nombreCompleto += string.Format(" {0}", item.apellidoPaterno.Trim().ToUpper());

                    if (!string.IsNullOrEmpty(item.apellidoMaterno))
                        nombreCompleto += string.Format(" {0}", item.apellidoMaterno.Trim().ToUpper());

                    nombreCompleto = SetPrimeraLetraMayuscula(nombreCompleto);

                    objComboDTO = new ComboDTO();
                    objComboDTO.Value = item.id.ToString();
                    objComboDTO.Text = nombreCompleto;
                    lstComboDTO.Add(objComboDTO);
                }

                foreach (var item in lstComboDTO)
                {
                    item.Text = item.Text;
                }

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "FillCboEvaluadores", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        private string GetEvaluadorRelEvaluado(int idPersonalEvaluado, int tipoRelacion)
        {
            string nombrePares = string.Empty;
            string nombreCompleto = string.Empty;
            try
            {
                #region SE OBTIENE LISTADO DE EVALUADORES RELACIONAS AL EVALUADO

                // SE OBTIENE EL NOMBRE COMPLETO DEL EVALUADOR
                List<RelacionDTO> lstUsuariosTipoPares = _context.Select<RelacionDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT t2.nombre, t2.apellidoPaterno, t2.apellidoMaterno
	                                    FROM tblRH_Eval360_Relaciones AS t1
	                                    INNER JOIN tblP_Usuario AS t2 ON t1.idPersonalEvaluador = t2.id
		                                    WHERE t1.registroActivo = @registroActivo AND t1.tipoRelacion = @tipoRelacion AND t1.idPersonalEvaluado = @idPersonalEvaluado",
                    parametros = new { registroActivo = true, tipoRelacion = tipoRelacion, idPersonalEvaluado = idPersonalEvaluado }
                }).ToList();

                foreach (var item in lstUsuariosTipoPares)
                {
                    nombreCompleto = string.Empty;
                    if (!string.IsNullOrEmpty(item.nombre))
                        nombreCompleto = item.nombre.Trim();

                    if (!string.IsNullOrEmpty(item.apellidoPaterno))
                        nombreCompleto = string.Format(" {0}", item.apellidoPaterno);

                    if (!string.IsNullOrEmpty(item.apellidoMaterno))
                        nombreCompleto = string.Format(" {0}", item.apellidoMaterno);

                    if (string.IsNullOrEmpty(nombrePares))
                        nombrePares = nombreCompleto;
                    else
                        nombrePares += string.Format("|{0}", nombreCompleto);
                }
                return nombrePares;
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetParesRelEvaluado", e, AccionEnum.CONSULTA, 0, new { idPersonalEvaluado = idPersonalEvaluado });
                return string.Empty;
            }
        }

        public Dictionary<string, object> GetListadoEvaluadoresRelEvaluador(int idPersonalEvaluado, int tipoRelacion, int idPeriodo)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE LISTADO DE EVALUADORES RELACIONADO AL EVALUADO
                // SE OBTIENE EL NOMBRE COMPLETO DEL EVALUADOR
                List<RelacionDTO> lstEvaluadorRelEvaluado = _context.Select<RelacionDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT t2.id, t1.idPersonalEvaluado, t4.nombre, t4.apellidoPaterno, t4.apellidoMaterno, t5.nombreCuestionario, t2.idPersonalEvaluador, t1.idPeriodo
	                                    FROM tblRH_Eval360_Relaciones AS t1
	                                    INNER JOIN tblRH_Eval360_RelacionesDet AS t2 ON t2.idRelacion = t1.id
	                                    INNER JOIN tblRH_Eval360_CatPersonal AS t3 ON t3.id = t2.idPersonalEvaluador
	                                    INNER JOIN tblP_Usuario AS t4 ON t4.id = t3.idUsuario
	                                    INNER JOIN tblRH_Eval360_Cuestionarios AS t5 ON t5.id = t2.idCuestionario
		                                    WHERE t1.registroActivo = @registroActivo AND t2.registroActivo = @registroActivo AND t3.registroActivo = @registroActivo AND 
                                                  t4.estatus = @estatus AND t5.registroActivo = @registroActivo AND t2.tipoRelacion = @tipoRelacion AND t1.idPersonalEvaluado = @idPersonalEvaluado AND t1.idPeriodo = @idPeriodo",
                    parametros = new { registroActivo = true, estatus = 1, tipoRelacion = tipoRelacion, idPersonalEvaluado = idPersonalEvaluado, idPeriodo = idPeriodo }
                }).ToList();

                List<RelacionDetDTO> lstEvaluadores = new List<RelacionDetDTO>();
                RelacionDetDTO objEvaluador = new RelacionDetDTO();
                string nombreCompleto = string.Empty;
                foreach (var item in lstEvaluadorRelEvaluado)
                {
                    nombreCompleto = string.Empty;
                    if (!string.IsNullOrEmpty(item.nombre))
                        nombreCompleto = item.nombre.Trim();

                    if (!string.IsNullOrEmpty(item.apellidoPaterno))
                        nombreCompleto += string.Format(" {0}", item.apellidoPaterno.Trim().ToUpper());

                    if (!string.IsNullOrEmpty(item.apellidoMaterno))
                        nombreCompleto += string.Format(" {0}", item.apellidoMaterno.Trim().ToUpper());

                    if (!string.IsNullOrEmpty(nombreCompleto))
                    {
                        objEvaluador = new RelacionDetDTO();
                        objEvaluador.id = item.id;
                        objEvaluador.idPeriodo = item.idPeriodo;
                        objEvaluador.nombreCompleto = nombreCompleto;
                        objEvaluador.nombreCuestionario = !string.IsNullOrEmpty(item.nombreCuestionario) ? item.nombreCuestionario.Trim().ToUpper() : string.Empty;
                        objEvaluador.idPersonalEvaluado = item.idPersonalEvaluado;
                        objEvaluador.idPersonalEvaluador = item.idPersonalEvaluador;
                        lstEvaluadores.Add(objEvaluador);
                    }
                }

                foreach (var item in lstEvaluadores)
                {
                    item.nombreCompleto = item.nombreCompleto;
                    item.nombreCuestionario = item.nombreCuestionario;
                }

                resultado.Add(SUCCESS, true);
                resultado.Add("lstEvaluadores", lstEvaluadores);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetListadoEvaluadoresRelEvaluador", e, AccionEnum.CONSULTA, 0, new { idPersonalEvaluado = idPersonalEvaluado, tipoRelacion = tipoRelacion });
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> CE_edicionEvaluado(RelacionDetDTO objRelacionDetDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    string mensajeError = string.Format("Ocurrió un error al {0} la información.", objRelacionDetDTO.id > 0 ? "actualizar" : "registrar");
                    if (objRelacionDetDTO.idRelacion <= 0) { throw new Exception(mensajeError); }
                    if (objRelacionDetDTO.idPersonalEvaluador <= 0) { throw new Exception("Es necesario seleccionar un evaluador."); }
                    if (objRelacionDetDTO.tipoRelacion <= 0) { throw new Exception(mensajeError); }
                    if (objRelacionDetDTO.idCuestionario <= 0) { throw new Exception("Es necesario seleccionar un cuestionario."); }
                    #endregion

                    #region NUEVO REGISTRO
                    tblRH_Eval360_RelacionesDet objNuevoDetalle = new tblRH_Eval360_RelacionesDet();
                    objNuevoDetalle.idRelacion = objRelacionDetDTO.idRelacion;
                    objNuevoDetalle.idPersonalEvaluador = objRelacionDetDTO.idPersonalEvaluador;
                    objNuevoDetalle.tipoRelacion = objRelacionDetDTO.tipoRelacion;
                    objNuevoDetalle.idCuestionario = objRelacionDetDTO.idCuestionario;
                    objNuevoDetalle.idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                    objNuevoDetalle.fechaCreacion = DateTime.Now;
                    objNuevoDetalle.registroActivo = true;
                    _context.tblRH_Eval360_RelacionesDet.Add(objNuevoDetalle);
                    _context.SaveChanges();
                    #endregion

                    #region NUEVO REGISTRO | TABLA EVALUACIONES
                    tblRH_Eval360_EvaluacionesEvaluador objEvaluacion = new tblRH_Eval360_EvaluacionesEvaluador();
                    objEvaluacion.idPeriodo = _context.tblRH_Eval360_Relaciones.Where(w => w.id == objRelacionDetDTO.idRelacion && w.registroActivo).Select(s => s.idPeriodo).FirstOrDefault();
                    objEvaluacion.idPersonalEvaluado = _context.tblRH_Eval360_Relaciones.Where(w => w.id == objRelacionDetDTO.idRelacion && w.registroActivo).Select(s => s.idPersonalEvaluado).FirstOrDefault();
                    objEvaluacion.idPersonalEvaluador = objRelacionDetDTO.idPersonalEvaluador;
                    objEvaluacion.idCuestionario = objRelacionDetDTO.idCuestionario;
                    objEvaluacion.idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                    objEvaluacion.fechaCreacion = DateTime.Now;
                    objEvaluacion.registroActivo = true;
                    _context.tblRH_Eval360_EvaluacionesEvaluador.Add(objEvaluacion);
                    _context.SaveChanges();
                    #endregion

                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha registrado con éxito.");
                    dbContextTransaction.Commit();

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, objRelacionDetDTO.id <= 0 ? (int)AccionEnum.AGREGAR : (int)AccionEnum.ACTUALIZAR, objRelacionDetDTO.id, JsonUtils.convertNetObjectToJson(objRelacionDetDTO));
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "CE_edicionEvaluado", e, AccionEnum.AGREGAR, objRelacionDetDTO.id, objRelacionDetDTO);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> EliminarEvaluadorRelEvaluado(RelacionDetDTO objRelacionDetDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    if (objRelacionDetDTO.id <= 0) { throw new Exception("Ocurrió un error al eliminar el registro."); }
                    if (objRelacionDetDTO.idPeriodo <= 0) { throw new Exception("Ocurrió un error al eliminar el registro."); }
                    if (objRelacionDetDTO.idPersonalEvaluado <= 0) { throw new Exception("Ocurrió un error al eliminar el registro."); }
                    if (objRelacionDetDTO.idPersonalEvaluador <= 0) { throw new Exception("Ocurrió un error al eliminar el registro."); }
                    #endregion

                    #region SE ELIMINA LA RELACIÓN DEL EVALUADOR CON EL EVALUADO
                    tblRH_Eval360_RelacionesDet objEliminar = _context.tblRH_Eval360_RelacionesDet.Where(w => w.id == objRelacionDetDTO.id).FirstOrDefault();
                    if (objEliminar == null)
                        throw new Exception("Ocurrió un error al eliminar el registro.");

                    objEliminar.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                    objEliminar.fechaModificacion = DateTime.Now;
                    objEliminar.registroActivo = false;
                    _context.SaveChanges();
                    #endregion

                    #region SE ELIMINA EL INIT DE LA EVALUACIÓN DEL EVALUADOR RELACIONADO AL EVALUADO

                    // SE VERIFICA SI EL EVALUADOR YA CUENTA CON EL CUESTIONARIO RESPONDIDO SOBRE EL EVALUADO
                    List<RelacionDetDTO> lstConductasRespondidas = _context.Select<RelacionDetDTO>(new DapperDTO
                    {
                        baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                        consulta = @"SELECT t2.id
	                                    FROM tblRH_Eval360_EvaluacionesEvaluador AS t1
	                                    INNER JOIN tblRH_Eval360_EvaluacionesEvaluadorDet AS t2 ON t2.idEvaluacion = t1.id
		                                    WHERE t1.idPeriodo = @idPeriodo AND t1.idPersonalEvaluado = @idPersonalEvaluado AND t1.idPersonalEvaluador = @idPersonalEvaluador AND 
                                                  t1.registroActivo = @registroActivo AND t2.registroActivo = @registroActivo",
                        parametros = new
                        {
                            idPeriodo = objRelacionDetDTO.idPeriodo,
                            idPersonalEvaluado = objRelacionDetDTO.idPersonalEvaluado,
                            idPersonalEvaluador = objRelacionDetDTO.idPersonalEvaluador,
                            registroActivo = true
                        }
                    }).ToList();
                    if (lstConductasRespondidas.Count() > 0)
                        throw new Exception("No se puede eliminar la relación, ya que cuenta con evaluaciones respondidas.");

                    // SE ELIMINA EL INIT DEL CUESTIONARIO DE LA RELACIÓN
                    tblRH_Eval360_EvaluacionesEvaluador objEliminarInitCuestionario = _context.tblRH_Eval360_EvaluacionesEvaluador.Where(w => w.idPeriodo == objRelacionDetDTO.idPeriodo &&
                                                                                                                                              w.idPersonalEvaluado == objRelacionDetDTO.idPersonalEvaluado &&
                                                                                                                                              w.idPersonalEvaluador == objRelacionDetDTO.idPersonalEvaluador &&
                                                                                                                                              w.registroActivo).FirstOrDefault();
                    if (objEliminarInitCuestionario != null)
                    {
                        objEliminarInitCuestionario.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objEliminarInitCuestionario.fechaModificacion = DateTime.Now;
                        objEliminarInitCuestionario.registroActivo = false;
                        _context.SaveChanges();
                    }
                    #endregion

                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha eliminado con éxito.");
                    dbContextTransaction.Commit();

                    // SE REGISTRA BITACORA
                    SaveBitacora(0, (int)AccionEnum.ELIMINAR, objEliminar.id, JsonUtils.convertNetObjectToJson(objEliminar));
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "EliminarEvaluadorRelEvaluado", e, AccionEnum.ELIMINAR, objRelacionDetDTO.id, objRelacionDetDTO);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }
        #endregion

        #region EVALUACIONES EVALUADOR
        public Dictionary<string, object> GetEvaluaciones(EvaluacionEvaluadorDTO objDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                objDTO.idPersonalEvaluador = GetPersonalID();
                List<EvaluacionEvaluadorDTO> lstEvaluacionesEvaluador = new List<EvaluacionEvaluadorDTO>();
                if (objDTO.idPersonalEvaluador > 0)
                {
                    lstEvaluacionesEvaluador = _context.Select<EvaluacionEvaluadorDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Construplan,
                        consulta = @"SELECT t1.id, t3.nombre, t3.apellidoPaterno, t3.apellidoMaterno, t1.idPeriodo, t1.idPersonalEvaluado, t1.idPersonalEvaluador, t4.fechaCierre AS fechaLimiteEvaluacion, t1.idCuestionario, t4.nombrePeriodo
		                                    FROM tblRH_Eval360_EvaluacionesEvaluador AS t1
		                                    INNER JOIN tblRH_Eval360_CatPersonal AS t2 ON t2.id = t1.idPersonalEvaluado
		                                    INNER JOIN tblP_Usuario AS t3 ON t3.id = t2.idUsuario
		                                    INNER JOIN tblRH_Eval360_CatPeriodos AS t4 ON t4.id = t1.idPeriodo
		                                        WHERE t1.registroActivo = @registroActivo AND t2.registroActivo = @registroActivo AND t3.estatus = @estatus AND t1.idPersonalEvaluador = @idPersonalEvaluador",
                        parametros = new { registroActivo = true, estatus = true, idPersonalEvaluador = objDTO.idPersonalEvaluador }
                    }).ToList();

                    string nombreCompleto = string.Empty;
                    if (lstEvaluacionesEvaluador.Count() > 0)
                    {
                        #region SE OBTIENE LISTADO DE CUESTIONARIOS
                        List<tblRH_Eval360_Cuestionarios> lstCuestionarios = _context.tblRH_Eval360_Cuestionarios.Where(w => w.registroActivo).ToList();
                        List<tblRH_Eval360_CuestionariosDet> lstCuestionariosDet = _context.tblRH_Eval360_CuestionariosDet.Where(w => w.registroActivo).ToList();
                        #endregion

                        foreach (var item in lstEvaluacionesEvaluador)
                        {
                            #region SE OBTIENE NOMBRE COMPLETO DEL EVALUADO
                            nombreCompleto = string.Empty;
                            if (!string.IsNullOrEmpty(item.nombre))
                                nombreCompleto = item.nombre.Trim();

                            if (!string.IsNullOrEmpty(item.apellidoPaterno))
                                nombreCompleto += string.Format(" {0}", item.apellidoPaterno.Trim().ToUpper());

                            if (!string.IsNullOrEmpty(item.apellidoMaterno))
                                nombreCompleto += string.Format(" {0}", item.apellidoMaterno.Trim().ToUpper());

                            item.nombreCompleto = SetPrimeraLetraMayuscula(nombreCompleto);
                            #endregion

                            #region SE OBTIENE EL TIPO DE RELACION DEL EVALUADOR CON EL EVALUADO
                            RelacionDetDTO objRelacion = _context.Select<RelacionDetDTO>(new DapperDTO
                            {
                                baseDatos = MainContextEnum.Construplan,
                                consulta = @"SELECT t2.tipoRelacion
	                                                FROM tblRH_Eval360_Relaciones AS t1
	                                                INNER JOIN tblRH_Eval360_RelacionesDet AS t2 ON t2.idRelacion = t1.id
		                                                WHERE t1.registroActivo = @registroActivo AND t2.registroActivo = @registroActivo AND 
                                                              t1.idPersonalEvaluado = @idPersonalEvaluado AND t2.idPersonalEvaluador = @idPersonalEvaluador",
                                parametros = new { registroActivo = true, idPersonalEvaluado = item.idPersonalEvaluado, idPersonalEvaluador = item.idPersonalEvaluador }
                            }).FirstOrDefault();

                            if (objRelacion != null)
                            {
                                item.tipoRelacion = objRelacion.tipoRelacion;
                                switch (item.tipoRelacion)
                                {
                                    case (int)TipoRelacionEnum.AUTOEVALUACION:
                                        item.relacion = EnumHelper.GetDescription((TipoRelacionEnum.AUTOEVALUACION));
                                        break;
                                    case (int)TipoRelacionEnum.CLIENTES_INTERNOS:
                                        item.relacion = EnumHelper.GetDescription((TipoRelacionEnum.CLIENTES_INTERNOS));
                                        break;
                                    case (int)TipoRelacionEnum.COLABORADORES:
                                        item.relacion = EnumHelper.GetDescription((TipoRelacionEnum.COLABORADORES));
                                        break;
                                    case (int)TipoRelacionEnum.JEFE:
                                        item.relacion = EnumHelper.GetDescription((TipoRelacionEnum.JEFE));
                                        break;
                                    case (int)TipoRelacionEnum.PARES:
                                        item.relacion = EnumHelper.GetDescription((TipoRelacionEnum.PARES));
                                        break;
                                }
                            }
                            #endregion

                            #region SE OBTIENE EL AVANCE DEL CUESTIONARIO
                            int cantConductasCuestionario = 0;
                            int cantConductasRespondidas = 0;

                            // SE OBTIENE LA CANTIDAD DE CONDUCTAS QUE TIENE UN CUESTIONARIO
                            cantConductasCuestionario = _context.tblRH_Eval360_CuestionariosDet.Where(w => w.idCuestionario == item.idCuestionario && w.registroActivo).Count();

                            // SE OBTIENE EL ID_EVALUACION RELACIONADO CON EL CUESTIONARIO, EVALUADO Y EVALUADOR
                            tblRH_Eval360_EvaluacionesEvaluador objEvaluacion = _context.tblRH_Eval360_EvaluacionesEvaluador.Where(w => w.idPeriodo == item.idPeriodo &&
                                                                                                                                        w.idPersonalEvaluado == item.idPersonalEvaluado &&
                                                                                                                                        w.idPersonalEvaluador == item.idPersonalEvaluador &&
                                                                                                                                        w.registroActivo).FirstOrDefault();
                            // SE OBTIENE LA CANTIDAD DE CONDUCTAS RESPONDIDAS
                            if (objEvaluacion != null)
                                cantConductasRespondidas = _context.tblRH_Eval360_EvaluacionesEvaluadorDet.Where(w => w.idEvaluacion == objEvaluacion.id && w.registroActivo).Count();

                            item.avance = string.Format("{0} de {1}", cantConductasRespondidas, cantConductasCuestionario);

                            if (cantConductasRespondidas == cantConductasCuestionario)
                                item.cuestionarioTerminado = true;
                            #endregion
                        }
                    }

                    foreach (var item in lstEvaluacionesEvaluador)
                    {
                        item.nombreCompleto = item.nombreCompleto;
                        item.relacion = item.relacion;
                        item.nombrePeriodo = item.nombrePeriodo;
                    }
                }

                resultado.Add(SUCCESS, true);
                resultado.Add("lstEvaluacionesEvaluador", lstEvaluacionesEvaluador.Where(w => !w.cuestionarioTerminado && !string.IsNullOrEmpty(w.relacion)).ToList());
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetEvaluaciones", e, AccionEnum.CONSULTA, objDTO.id, objDTO);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> GetEvaluacionEvaluadoRelEvaluador(EvaluacionEvaluadorDTO objDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (objDTO.idEvaluacion <= 0) { throw new Exception("Ocurrió un error al obtener la evaluación del evaluado."); }
                #endregion

                #region SE OBTIENE LA CONDUCTA A EVALUAR
                // SE OBTIENE LA EVALUACIÓN SELECCIONADA POR PARTE DEL EVALUADOR
                tblRH_Eval360_EvaluacionesEvaluador objEvaluacion = _context.tblRH_Eval360_EvaluacionesEvaluador.Where(w => w.id == objDTO.idEvaluacion && w.registroActivo).FirstOrDefault();
                if (objEvaluacion == null)
                    throw new Exception("Ocurrió un error al obtener la evaluación seleccionada.");

                // SE OBTIENE LISTADO DE CONDUCTAS RESPONDIDAS POR EL EVALUADOR
                List<tblRH_Eval360_EvaluacionesEvaluadorDet> objEvaluacionDet = _context.tblRH_Eval360_EvaluacionesEvaluadorDet.Where(w => w.idEvaluacion == objEvaluacion.id && w.registroActivo).ToList();

                // SE OBTIENE LOS CRITERIOS A EVALUAR DE LA CONDUCTA
                List<tblRH_Eval360_CatCriterios> lstCriteriosRelConducta = _context.tblRH_Eval360_CatCriterios.Where(w => w.idCuestionario == objEvaluacion.idCuestionario && w.registroActivo).ToList();
                if (lstCriteriosRelConducta.Count() <= 0)
                    throw new Exception("No se encontró criterios a tomar en cuenta, favor de contactarse con el administrador.");

                #region SE OBTIENE LA CONDUCTA A RESPONDER
                List<EvaluacionEvaluadorDetDTO> lstConductas = _context.Select<EvaluacionEvaluadorDetDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT t4.nombreCompetencia, t3.descripcionConducta, t2.idConducta
	                                    FROM tblRH_Eval360_EvaluacionesEvaluador AS t1
	                                    INNER JOIN tblRH_Eval360_CuestionariosDet AS t2 ON t2.idCuestionario = t1.idCuestionario
	                                    INNER JOIN tblRH_Eval360_CatConductas AS t3 ON t3.id = t2.idConducta
	                                    INNER JOIN tblRH_Eval360_CatCompetencias AS t4 ON t4.id = t3.idCompetencia
		                                    WHERE t1.id = @idEvaluacion AND t1.registroActivo = @registroActivo AND t2.registroActivo = @registroActivo AND t3.registroActivo = @registroActivo AND t4.registroActivo = @registroActivo
			                                    ORDER BY t3.orden",
                    parametros = new { idEvaluacion = objDTO.idEvaluacion, registroActivo = true }
                }).ToList();
                if (lstConductas.Count() <= 0)
                    throw new Exception("Ocurrió un error al obtener la conducta.");
                #endregion

                string objNombreCriterios = string.Empty;
                string objRadioCriterios = string.Empty;
                int idConducta = lstConductas[0].idConducta; // PRIMER CONDUCTA A EVALUAR
                foreach (var item in lstCriteriosRelConducta)
                {
                    // SE VERIFICA SI LA PRIMERA CONDUCTA YA SE ENCUENTRA RESPONDIDA
                    tblRH_Eval360_EvaluacionesEvaluadorDet objConductaRelCriterio = objEvaluacionDet.Where(w => w.idConducta == idConducta && w.idCriterio == item.id && w.registroActivo).FirstOrDefault();

                    objNombreCriterios += string.Format("<div class='col-lg-2'><label>{0}</label></div>", item.etiqueta);
                    //if (objConductaRelCriterio != null)
                    //    objRadioCriterios += string.Format("<div class='col-lg-2'><input type='radio' value='{0}' name='name' checked='checked'></div>", item.id);
                    //else
                        objRadioCriterios += string.Format("<div class='col-lg-2'><input type='radio' value='{0}' name='name'></div>", item.id);
                }

                #region SE VERIFICA SI ES LA UNICA CONDUCTA DEL CUESTIONARIO
                EvaluacionEvaluadorDetDTO objConducta = lstConductas[0];
                lstConductas[0].btnSiguienteFinalizar = "Siguiente";
                if (lstConductas.Count() == 1)
                {
                    lstConductas[0].mostrarComentario = true;
                    lstConductas[0].btnSiguienteFinalizar = "Finalizar";
                }
                else if (lstConductas.Count() > 1)
                    objConducta.idConductaSiguiente = lstConductas[1].idConducta;
                #endregion

                #region SE VERIFICA SI LA CONDUCTA YA SE ENCUENTRA RESPONDIDA, SI NO, PARA VERIFICAR LA SIGUIENTE Y MOSTRAR LA QUE SIGUE POR RESPONDER
                bool mostrarComentario = false;
                string btnSiguienteFinalizar = string.Empty;
                int ultimaConductaID = lstConductas.Select(s => s.idConducta).LastOrDefault();
                List<tblRH_Eval360_EvaluacionesEvaluadorDet> lstConductasRespondidas = _context.tblRH_Eval360_EvaluacionesEvaluadorDet.Where(w => w.idEvaluacion == objDTO.idEvaluacion && w.registroActivo).ToList();
                foreach (var item in lstConductas)
                {
                    tblRH_Eval360_EvaluacionesEvaluadorDet objConductaRespondida = lstConductasRespondidas.Where(w => w.idConducta == item.idConducta).FirstOrDefault();
                    if (objConductaRespondida == null)
                    {
                        objConducta.idConductaSiguiente = item.idConducta;
                        if (ultimaConductaID == item.idConducta)
                        {
                            mostrarComentario = true;
                            btnSiguienteFinalizar = "Finalizar";
                        }
                        else
                        {
                            mostrarComentario = false;
                            btnSiguienteFinalizar = "Siguiente";
                        }
                        break;
                    }
                }
                #endregion

                resultado.Add(SUCCESS, true);
                resultado.Add("objConducta", lstConductas.Where(w => w.idConducta == objConducta.idConductaSiguiente).FirstOrDefault());
                resultado.Add("objNombreCriterios", objNombreCriterios);
                resultado.Add("objRadioCriterios", objRadioCriterios);
                resultado.Add("mostrarComentario", mostrarComentario);
                resultado.Add("btnSiguienteFinalizar", btnSiguienteFinalizar);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetEvaluacionEvaluadoRelEvaluador", e, AccionEnum.CONSULTA, 0, objDTO);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> GuardarRespuestaConducta(EvaluacionEvaluadorDetDTO objDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    string mensajeError = "Ocurrió un error al registrar la respuesta.";
                    if (objDTO.idEvaluacion <= 0) { throw new Exception(mensajeError); }
                    if (objDTO.idConducta <= 0) { throw new Exception(mensajeError); }
                    if (objDTO.idCriterio <= 0) { throw new Exception(mensajeError); }
                    #endregion

                    bool nuevaRespuesta = false;
                    tblRH_Eval360_EvaluacionesEvaluadorDet objCERespuesta = _context.tblRH_Eval360_EvaluacionesEvaluadorDet.Where(w => w.idEvaluacion == objDTO.idEvaluacion && w.idConducta == objDTO.idConducta && w.idCriterio == objDTO.idCriterio && w.registroActivo).FirstOrDefault();
                    if (objCERespuesta == null)
                    {
                        #region REGISTRAR RESPUESTA
                        nuevaRespuesta = true;
                        objCERespuesta = new tblRH_Eval360_EvaluacionesEvaluadorDet();
                        objCERespuesta.idEvaluacion = objDTO.idEvaluacion;
                        objCERespuesta.idConducta = objDTO.idConducta;
                        objCERespuesta.idCriterio = objDTO.idCriterio;
                        objCERespuesta.comentario = !string.IsNullOrEmpty(objDTO.comentario) ? objDTO.comentario.Trim() : null;
                        objCERespuesta.idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCERespuesta.fechaCreacion = DateTime.Now;
                        objCERespuesta.registroActivo = true;
                        _context.tblRH_Eval360_EvaluacionesEvaluadorDet.Add(objCERespuesta);
                        _context.SaveChanges();
                        #endregion
                    }
                    else if (objCERespuesta.idCriterio != objDTO.idCriterio)
                    {
                        #region ACTUALIZAR RESPUESTA
                        objCERespuesta.idCriterio = objDTO.idCriterio;
                        objCERespuesta.comentario = !string.IsNullOrEmpty(objDTO.comentario) ? objDTO.comentario.Trim() : null;
                        objCERespuesta.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCERespuesta.fechaModificacion = DateTime.Now;
                        _context.SaveChanges();
                        #endregion
                    }

                    List<EvaluacionEvaluadorDetDTO> lstConductas = new List<EvaluacionEvaluadorDetDTO>();

                    #region SE OBTIENE LA SIGUIENTE CONDUCTA A RESPONDER
                    lstConductas = _context.Select<EvaluacionEvaluadorDetDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Construplan,
                        consulta = @"SELECT t4.nombreCompetencia, t3.descripcionConducta, t2.idConducta, t3.orden
	                                        FROM tblRH_Eval360_EvaluacionesEvaluador AS t1
	                                        INNER JOIN tblRH_Eval360_CuestionariosDet AS t2 ON t2.idCuestionario = t1.idCuestionario
	                                        INNER JOIN tblRH_Eval360_CatConductas AS t3 ON t3.id = t2.idConducta
	                                        INNER JOIN tblRH_Eval360_CatCompetencias AS t4 ON t4.id = t3.idCompetencia
		                                        WHERE t1.id = @idEvaluacion AND t1.registroActivo = @registroActivo AND t2.registroActivo = @registroActivo AND 
                                                        t3.registroActivo = @registroActivo AND t4.registroActivo = @registroActivo
			                                            ORDER BY t3.orden",
                        parametros = new { idEvaluacion = objDTO.idEvaluacion, registroActivo = true }
                    }).ToList();
                    if (lstConductas.Count() <= 0)
                        throw new Exception("Ocurrió un error al obtener la conducta.");

                    int numOrdenConducta = lstConductas.Where(w => w.idConducta == objDTO.idConducta).Select(s => s.orden).FirstOrDefault();
                    if (numOrdenConducta > 0)
                        objDTO.idConductaSiguiente = numOrdenConducta + 1;

                    // SE OBTIENE LOS CRITERIOS A EVALUAR DE LA CONDUCTA
                    List<tblRH_Eval360_CatCriterios> lstCriteriosRelConducta = _context.tblRH_Eval360_CatCriterios.Where(w => w.idCuestionario == objDTO.idCuestionario && w.registroActivo).ToList();

                    if (objDTO.idConductaSiguiente > 0)
                    {
                        #region SE OBTIENE LA INFORMACIÓN GENERAL DE LA EVALUACION
                        // SE OBTIENE LISTADO DE CONDUCTAS RESPONDIDAS POR EL EVALUADOR
                        List<tblRH_Eval360_EvaluacionesEvaluadorDet> objEvaluacionDet = _context.tblRH_Eval360_EvaluacionesEvaluadorDet.Where(w => w.idEvaluacion == objDTO.idEvaluacion && w.registroActivo).ToList();

                        
                        #endregion

                        int idConducta = lstConductas.Where(w => w.idConducta == objDTO.idConductaSiguiente).Select(s => s.idConducta).FirstOrDefault();
                        for (int i = 0; i < lstConductas.Count(); i++)
                        {
                            objDTO.idConductaSiguiente += 1;
                            idConducta = lstConductas.Where(w => w.idConducta == objDTO.idConductaSiguiente).Select(s => s.idConducta).FirstOrDefault();
                            if (idConducta > 0)
                                break;
                        }

                        if (idConducta > 0)
                        {
                            //#region SIGUIENTE OBTIENE LA SIGUIENTE CONDUCTA A EVALUAR
                            //string objNombreCriterios = string.Empty;
                            //string objRadioCriterios = string.Empty;
                            //foreach (var item in lstCriteriosRelConducta)
                            //{
                            //    #region SE VERIFICA SI LA PRIMERA CONDUCTA YA SE ENCUENTRA RESPONDIDA
                            //    tblRH_Eval360_EvaluacionesEvaluadorDet objConductaRelCriterio = objEvaluacionDet.Where(w => w.idConducta == idConducta && w.idCriterio == item.id && w.registroActivo).FirstOrDefault();

                            //    objNombreCriterios += string.Format("<div class='col-lg-2'><label>{0}</label></div>", item.etiqueta);
                            //    if (objConductaRelCriterio != null)
                            //        objRadioCriterios += string.Format("<div class='col-lg-2'><input type='radio' value='{0}' name='name' checked='checked'></div>", item.id);
                            //    else
                            //        objRadioCriterios += string.Format("<div class='col-lg-2'><input type='radio' value='{0}' name='name'></div>", item.id);
                            //    #endregion
                            //}

                            //#region SE INDICA LA CONDUCTA ANTERIOR
                            //EvaluacionEvaluadorDetDTO objConductaAnterior = lstConductas.Where(w => w.idConducta == objDTO.idConducta).FirstOrDefault();
                            //if (objConductaAnterior == null)
                            //    throw new Exception("Ocurrió un error al obtener la conducta anterior.");

                            //objConductaAnterior.idConductaAnterior = objDTO.idConducta;
                            //#endregion

                            //#region SE VERIFICA SI ES LA UNICA CONDUCTA DEL CUESTIONARIO
                            //EvaluacionEvaluadorDetDTO objConducta = new EvaluacionEvaluadorDetDTO();
                            //int ultimaConductaID = lstConductas.OrderByDescending(w => w.idConducta).Select(s => s.idConducta).FirstOrDefault();
                            //objConducta = lstConductas.Where(w => w.idConducta == idConducta).FirstOrDefault();
                            //if (ultimaConductaID == idConducta)
                            //{
                            //    objConducta.mostrarComentario = true;
                            //    objConducta.btnSiguienteFinalizar = "Finalizar";
                            //}
                            //else
                            //{
                            //    objConducta.mostrarComentario = false;
                            //    objConducta.btnSiguienteFinalizar = "Siguiente";
                            //}
                            //#endregion

                            //resultado.Add("objConducta", lstConductas.Where(w => w.idConducta == idConducta).FirstOrDefault());
                            //resultado.Add("objNombreCriterios", objNombreCriterios);
                            //resultado.Add("objRadioCriterios", objRadioCriterios);
                            //#endregion
                        }
                    }

                    string objNombreCriterios = string.Empty;
                    string objRadioCriterios = string.Empty;
                    //int idConducta = lstConductas[0].idConducta; // PRIMER CONDUCTA A EVALUAR
                    foreach (var item in lstCriteriosRelConducta)
                    {
                        // SE VERIFICA SI LA PRIMERA CONDUCTA YA SE ENCUENTRA RESPONDIDA
                        //tblRH_Eval360_EvaluacionesEvaluadorDet objConductaRelCriterio = objEvaluacionDet.Where(w => w.idConducta == idConducta && w.idCriterio == item.id && w.registroActivo).FirstOrDefault();

                        objNombreCriterios += string.Format("<div class='col-lg-2'><label>{0}</label></div>", item.etiqueta);
                        //if (objConductaRelCriterio != null)
                        //    objRadioCriterios += string.Format("<div class='col-lg-2'><input type='radio' value='{0}' name='name' checked='checked'></div>", item.id);
                        //else
                        objRadioCriterios += string.Format("<div class='col-lg-2'><input type='radio' value='{0}' name='name'></div>", item.id);
                    }

                    #region SE VERIFICA SI LA CONDUCTA YA SE ENCUENTRA RESPONDIDA, SI NO, PARA VERIFICAR LA SIGUIENTE Y MOSTRAR LA QUE SIGUE POR RESPONDER
                    int idConductaSiguiente = 0;
                    bool mostrarComentario = false;
                    string btnSiguienteFinalizar = string.Empty;
                    int ultimaConductaID = lstConductas.Select(s => s.idConducta).LastOrDefault();
                    List<tblRH_Eval360_EvaluacionesEvaluadorDet> lstConductasRespondidas = _context.tblRH_Eval360_EvaluacionesEvaluadorDet.Where(w => w.idEvaluacion == objDTO.idEvaluacion && w.registroActivo).ToList();
                    foreach (var item in lstConductas)
                    {
                        tblRH_Eval360_EvaluacionesEvaluadorDet objConductaRespondida = lstConductasRespondidas.Where(w => w.idConducta == item.idConducta).FirstOrDefault();
                        if (objConductaRespondida == null)
                        {
                            if (ultimaConductaID == item.idConducta)
                            {
                                mostrarComentario = true;
                                btnSiguienteFinalizar = "Finalizar";
                            }
                            else
                            {
                                mostrarComentario = false;
                                btnSiguienteFinalizar = "Siguiente";
                            }

                            idConductaSiguiente = item.idConducta;
                            break;
                        }
                    }
                    #endregion
                    #endregion

                    #region SE VERIFICA SI YA SE CONCLUYO EL CUESTIONARIO
                    int idCuestionario = _context.tblRH_Eval360_EvaluacionesEvaluador.Where(w => w.id == objDTO.idEvaluacion && w.registroActivo).Select(s => s.idCuestionario).FirstOrDefault();
                    if (idCuestionario <= 0)
                        throw new Exception("Ocurrió un error al obtener el estatus del cuestionario.");

                    int cantConductasEvaluar = _context.tblRH_Eval360_CuestionariosDet.Where(w => w.idCuestionario == idCuestionario && w.registroActivo).Count();
                    int cantConductasEvaluadas = _context.tblRH_Eval360_EvaluacionesEvaluadorDet.Where(w => w.idEvaluacion == objDTO.idEvaluacion && w.registroActivo).Count();

                    if (cantConductasEvaluar == cantConductasEvaluadas)
                    {
                        resultado.Add("cuestionarioTerminado", true);
                        resultado.Add(MESSAGE, "Se ha finalizado el cuestionario con éxito.");
                    }
                    else if (nuevaRespuesta)
                    {
                        resultado.Add(MESSAGE, "Se ha registrado con éxito la respuesta.");
                    }
                    else
                    {
                        resultado.Add("cuestionarioTerminado", false);
                        resultado.Add(MESSAGE, "Se ha actualizado con éxito la respuesta.");
                    }
                    #endregion

                    resultado.Add(SUCCESS, true);
                    resultado.Add("objConducta", lstConductas.Where(w => w.idConducta == idConductaSiguiente).FirstOrDefault());
                    resultado.Add("objNombreCriterios", objNombreCriterios);
                    resultado.Add("objRadioCriterios", objRadioCriterios);
                    resultado.Add("mostrarComentario", mostrarComentario);
                    resultado.Add("btnSiguienteFinalizar", btnSiguienteFinalizar);
                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "GuardarRespuestaConducta", e, AccionEnum.CONSULTA, 0, objDTO);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }
        #endregion

        #region REPORTE 360
        public Dictionary<string, object> GetEstatusEvaluados(Reporte360DTO objFiltro)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE EL ESTATUS DE LOS EVALUADORES SOBRE LA PERSONA EVALUADA

                // SE OBTIENE LISTADO DE EVALUADOS CON LA RELACIÓN DE EVALUADORES
                List<Reporte360DTO> lstReporteDTO = new List<Reporte360DTO>();
                List<tblRH_Eval360_Relaciones> lstRelaciones = _context.tblRH_Eval360_Relaciones.Where(w => w.registroActivo).ToList();
                if (lstRelaciones.Count() > 0)
                {
                    #region SE OBTIENE LISTADO DEL CATALOGO DE PERSONAL
                    List<tblRH_Eval360_CatPersonal> lstCatPersonal = _context.tblRH_Eval360_CatPersonal.Where(w => w.registroActivo).ToList();
                    if (lstCatPersonal.Count() <= 0)
                        throw new Exception("No se encuentra personal registrado.");
                    #endregion

                    Reporte360DTO objReporteDTO = new Reporte360DTO();
                    foreach (var item in lstRelaciones)
                    {
                        objReporteDTO = new Reporte360DTO();
                        objReporteDTO.id = item.id;
                        objReporteDTO.idPersonalEvaluado = item.idPersonalEvaluado;
                        objReporteDTO.idPeriodo = item.idPeriodo;

                        #region SE OBTIENE NOMBRE COMPLETO DEL EVALUADO
                        tblRH_Eval360_CatPersonal objPersonal = lstCatPersonal.Where(w => w.id == item.idPersonalEvaluado && w.registroActivo).FirstOrDefault();
                        if (objPersonal != null)
                            objReporteDTO.nombreCompleto = GetNombreCompletoUsuarioSIGOPLAN(objPersonal.idUsuario, objPersonal.idEmpresa);
                        #endregion

                        #region SE VERIFICA EL ESTATUS DEL EVALUADO RELACIONADO A SUS EVALUADORES
                        objReporteDTO.estatusCuestionario = GetEstatusEvaluadoRelEvaluadores(item.id, item.idPersonalEvaluado);
                        #endregion

                        lstReporteDTO.Add(objReporteDTO);
                    }

                    if (objFiltro.idPeriodo > 0)
                    {
                        #region SE FILTRA POR PERIODO
                        lstReporteDTO = lstReporteDTO.Where(w => w.idPeriodo == objFiltro.idPeriodo).ToList();
                        #endregion
                    }
                }
                resultado.Add(SUCCESS, true);
                resultado.Add("lstReporteDTO", lstReporteDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetEstatusEvaluados", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        private string GetEstatusEvaluadoRelEvaluadores(int idRelacion, int idPersonalEvaluado)
        {
            string estatusEvaluado = "Evaluación completa";
            try
            {
                #region VALIDACIONES
                string mensajeError = "Ocurrió un error al obtener el estatus del evaluado.";
                if (idRelacion <= 0) { throw new Exception(mensajeError); }
                #endregion

                #region SE OBTIENE EL ESTATUS DE LAS EVALUACIONES DEL EVALUADO

                // SE OBTIENE LISTADO DE EVALUADORES
                List<tblRH_Eval360_RelacionesDet> lstRelacionesDet = _context.tblRH_Eval360_RelacionesDet.Where(w => w.idRelacion == idRelacion && w.registroActivo).ToList();

                foreach (var item in lstRelacionesDet)
                {
                    if (estatusEvaluado == "Evaluación completa")
                    {
                        int idPersonalEvaluador = item.idPersonalEvaluador;
                        int idCuestionario = item.idCuestionario;

                        // SE OBTIENE ID_EVALUACION
                        int idEvaluacion = _context.tblRH_Eval360_EvaluacionesEvaluador.Where(w => w.idPersonalEvaluado == idPersonalEvaluado && w.idPersonalEvaluador == idPersonalEvaluador && w.registroActivo).Select(s => s.id).FirstOrDefault();
                        if (idEvaluacion <= 0)
                            throw new Exception(mensajeError);

                        int cantConductasPorResponder = _context.tblRH_Eval360_CuestionariosDet.Where(w => w.idCuestionario == idCuestionario).Count();
                        int cantConductasRespondidas = _context.tblRH_Eval360_EvaluacionesEvaluadorDet.Where(w => w.idEvaluacion == idEvaluacion && w.registroActivo).Count();

                        if (cantConductasPorResponder != cantConductasRespondidas)
                            estatusEvaluado = "Evaluación incompleta";
                    }
                }
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetEstatusEvaluadoRelEvaluadores", e, AccionEnum.CONSULTA, 0, 0);
                return "Evaluación incompleta";
            }
            return estatusEvaluado;
        }

        public Dictionary<string, object> GetEstatusCuestionariosEvaluadores(Reporte360DTO objDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                string mensajeError = "Ocurrió un error al obtener la información.";
                if (objDTO.idRelacion <= 0) { throw new Exception(mensajeError); }
                if (objDTO.tipoRelacion <= 0) { throw new Exception(mensajeError); }
                #endregion

                #region SE OBTIENE EL ESTATUS DE LOS CUESTIONARIOS DE LOS EVALUADORES EN BASE AL TIPO RELACION SELECCIONADO

                #region SE OBTIENE ID_PERSONAL_EVALUADO
                tblRH_Eval360_Relaciones objRelacion = _context.tblRH_Eval360_Relaciones.Where(w => w.id == objDTO.idRelacion && w.registroActivo).FirstOrDefault();
                if (objRelacion == null)
                    throw new Exception(mensajeError);

                objDTO.idPersonalEvaluado = objRelacion.idPersonalEvaluado;
                #endregion

                // SE OBTIENE ID_PERSONAL_EVALUADOR
                List<tblRH_Eval360_RelacionesDet> lstRelacionesDet = _context.tblRH_Eval360_RelacionesDet.Where(w => w.idRelacion == objDTO.idRelacion && w.tipoRelacion == objDTO.tipoRelacion && w.registroActivo).ToList();
                List<int> lstPersonalEvaluador_ID = new List<int>();
                foreach (var item in lstRelacionesDet)
                {
                    lstPersonalEvaluador_ID.Add(item.idPersonalEvaluador);
                }

                List<Reporte360DTO> lstReporte360DTO = new List<Reporte360DTO>();
                Reporte360DTO objReporte360DTO = new Reporte360DTO();
                foreach (var item in lstPersonalEvaluador_ID)
                {
                    tblRH_Eval360_EvaluacionesEvaluador objEvaluacionEvaluador = _context.tblRH_Eval360_EvaluacionesEvaluador.Where(w => w.idPersonalEvaluado == objDTO.idPersonalEvaluado && w.idPersonalEvaluador == item && w.registroActivo).FirstOrDefault();
                    if (objEvaluacionEvaluador != null)
                    {
                        objReporte360DTO = new Reporte360DTO();
                        int cantConductasRespondidas = _context.tblRH_Eval360_EvaluacionesEvaluadorDet.Where(w => w.idEvaluacion == objEvaluacionEvaluador.id && w.registroActivo).Count();
                        int cantConductasPorResponder = _context.tblRH_Eval360_CuestionariosDet.Where(w => w.idCuestionario == objEvaluacionEvaluador.idCuestionario && w.registroActivo).Count();

                        if (cantConductasRespondidas != cantConductasPorResponder)
                            objReporte360DTO.estatusCuestionario = "Evaluación incompleta";
                        else
                            objReporte360DTO.estatusCuestionario = "Evaluación completa";

                        int idUsuario = _context.tblRH_Eval360_CatPersonal.Where(w => w.id == item && w.registroActivo).Select(s => s.idUsuario).FirstOrDefault();
                        objReporte360DTO.nombreCompleto = GetNombreCompletoUsuarioSIGOPLAN(idUsuario, GetEmpresaPersonal(objEvaluacionEvaluador.idPersonalEvaluado));
                        lstReporte360DTO.Add(objReporte360DTO);
                    }
                }
                #endregion

                resultado.Add(SUCCESS, true);
                resultado.Add("lstReporte360DTO", lstReporte360DTO);
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetEstatusCuestionariosEvaluadores", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> GenerarReporte360(Reporte360DTO objDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                string mensajeError = "Ocurrió un error al generar el reporte 360.";
                if (objDTO.idRelacion <= 0) { throw new Exception(mensajeError); }
                if (objDTO.idPersonalEvaluado <= 0) { throw new Exception(mensajeError); }
                #endregion

                #region SE OBTIENE LA INFORMACIÓN A MOSTRAR EN EL REPORTE 360

                #region SE OBTIENE EL NOMBRE, PUESTO Y FECHA DE INGRESO DEL EVALUADO
                int idEmpresa = _context.tblRH_Eval360_CatPersonal.Where(w => w.id == objDTO.idPersonalEvaluado).Select(s => s.idEmpresa).FirstOrDefault();
                MainContextEnum objEmpresa = GetEmpresaLogueada(idEmpresa);
                Reporte360DTO objPersonal = objPersonal = _context.Select<Reporte360DTO>(new DapperDTO
                {
                    baseDatos = objEmpresa,
                    consulta = @"SELECT t3.nombre, t3.ape_paterno AS apellidoPaterno, t3.ape_materno AS apellidoMaterno, t4.descripcion AS puesto, t3.fecha_alta AS fechaIngreso
	                                    FROM tblRH_Eval360_CatPersonal AS t1
	                                    INNER JOIN tblP_Usuario AS t2 ON t2.id = t1.idUsuario
	                                    INNER JOIN tblRH_EK_Empleados AS t3 ON t3.clave_empleado = CONVERT(NVARCHAR(100), t2.cveEmpleado)
	                                    INNER JOIN tblRH_EK_Puestos AS t4 ON t4.puesto = t3.puesto
		                                    WHERE t1.registroActivo = @registroActivo AND t2.estatus = @estatus AND t3.estatus_empleado = @estatus_empleado AND t3.esActivo = @esActivo AND 
                                                  t4.registroActivo = @registroActivo AND t1.id = @idPersonalEvaluado",
                    parametros = new { registroActivo = true, estatus = true, estatus_empleado = 'A', esActivo = true, idPersonalEvaluado = objDTO.idPersonalEvaluado }
                }).FirstOrDefault();
                if (objPersonal == null)
                    throw new Exception("Ocurrió un error al obtener la información del evaluado.");

                objDTO.nombreCompleto = SetNombreCompleto(objPersonal.nombre, objPersonal.apellidoPaterno, objPersonal.apellidoMaterno);
                objDTO.nombreCompleto = SetPrimeraLetraMayuscula(objDTO.nombreCompleto);
                objDTO.puesto = objPersonal.puesto;
                objDTO.puesto = SetPrimeraLetraMayuscula(objDTO.puesto);
                objDTO.fechaIngreso = objPersonal.fechaIngreso;
                #endregion

                #region SE OBTIENE LISTADO DE CRITERIOS LIGADOS AL CUESTIONARIO DEL EVALUADO
                int idCuestionario = _context.tblRH_Eval360_RelacionesDet.Where(w => w.idRelacion == objDTO.idRelacion && w.registroActivo).Select(s => s.idCuestionario).FirstOrDefault();
                if (idCuestionario <= 0)
                    throw new Exception(mensajeError);

                List<tblRH_Eval360_CatCriterios> lstCriteriosRelCuestionario = _context.tblRH_Eval360_CatCriterios.Where(w => w.idCuestionario == idCuestionario && w.registroActivo).ToList();

                foreach (var item in lstCriteriosRelCuestionario)
                {
                    item.descripcionEtiqueta = item.descripcionEtiqueta;
                }
                #endregion

                #region SE OBTIENE PROMEDIOS
                List<Reporte360DTO> lstPromedioPorGrupo = new List<Reporte360DTO>();
                List<Reporte360DTO> lstPromedioPorCompetencia = new List<Reporte360DTO>();
                Reporte360DTO objReporte360 = new Reporte360DTO();

                // SE OBTIENE ID_PERIODO
                objDTO.idPeriodo = _context.tblRH_Eval360_Relaciones.Where(w => w.id == objDTO.idRelacion && w.registroActivo).Select(s => s.idPeriodo).FirstOrDefault();
                if (objDTO.idPeriodo <= 0)
                    throw new Exception(mensajeError);

                // SE OBTIENE LAS EVALUACIONES APLICADAS DEL EVALUADO SELECCIONADO
                List<tblRH_Eval360_EvaluacionesEvaluador> lstEvaluaciones = _context.tblRH_Eval360_EvaluacionesEvaluador.Where(w => w.idPeriodo == objDTO.idPeriodo && w.idPersonalEvaluado == objDTO.idPersonalEvaluado && w.registroActivo).ToList();
                if (lstEvaluaciones.Count() <= 0)
                    throw new Exception("No se encuentra ninguna evaluación registrada.");

                #region SE OBTIENE PROMEDIO POR GRUPO
                foreach (var item in lstEvaluaciones)
                {
                    List<Reporte360DTO> lstRespuestasPorGrupo = _context.Select<Reporte360DTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Construplan,
                        consulta = @"SELECT t4.idGrupo, t5.limSuperior, t1.idPersonalEvaluador
	                                        FROM tblRH_Eval360_EvaluacionesEvaluador AS t1
	                                        INNER JOIN tblRH_Eval360_EvaluacionesEvaluadorDet AS t2 ON t2.idEvaluacion = t1.id
	                                        INNER JOIN tblRH_Eval360_CatConductas AS t3 ON t3.id = t2.idConducta
	                                        INNER JOIN tblRH_Eval360_CatCompetencias AS t4 ON t4.id = t3.idCompetencia
	                                        INNER JOIN tblRH_Eval360_CatCriterios AS t5 ON t5.id = t2.idCriterio
		                                        WHERE t1.idPeriodo = @idPeriodo AND t1.idPersonalEvaluado = @idPersonalEvaluado AND 
                                                      t1.registroActivo = @registroActivo AND t2.registroActivo = @registroActivo AND 
                                                      t3.registroActivo = @registroActivo AND t4.registroActivo = @registroActivo AND 
                                                      t5.registroActivo = @registroActivo AND t2.idEvaluacion = @idEvaluacion
						                                  ORDER BY t4.idGrupo",
                        parametros = new { idPeriodo = objDTO.idPeriodo, idPersonalEvaluado = objDTO.idPersonalEvaluado, registroActivo = true, idEvaluacion = item.id }
                    }).ToList();

                    foreach (var itemGrupo in lstRespuestasPorGrupo)
                    {
                        Reporte360DTO objEvalPorGrupo = lstPromedioPorGrupo.Where(w => w.idGrupo == itemGrupo.idGrupo).FirstOrDefault();
                        if (objEvalPorGrupo == null)
                        {
                            #region SE AGREGA GRUPO A LA LISTA
                            objEvalPorGrupo = new Reporte360DTO();
                            objEvalPorGrupo.idGrupo = itemGrupo.idGrupo;
                            objEvalPorGrupo.nombreGrupo = GetNombreGrupo(itemGrupo.idGrupo);
                            objEvalPorGrupo.tipoRelacion = GetTipoRelacion(objDTO.idPeriodo, objDTO.idPersonalEvaluado, itemGrupo.idPersonalEvaluador);
                            objEvalPorGrupo.limSuperior = itemGrupo.limSuperior;

                            #region SE INDICA QUE HAY UN CRITERIO REGISTRADO COMO INICIAL EN LA RELACIÓN CORRESPONDIENTE
                            switch ((int)objEvalPorGrupo.tipoRelacion)
                            {
                                case (int)TipoRelacionEnum.AUTOEVALUACION:
                                    objEvalPorGrupo.cantCriterios_Autoevaluacion = 1;
                                    objEvalPorGrupo.limSuperior_Autoevaluacion = itemGrupo.limSuperior;
                                    break;
                                case (int)TipoRelacionEnum.CLIENTES_INTERNOS:
                                    objEvalPorGrupo.cantCriterios_ClientesInternos = 1;
                                    objEvalPorGrupo.limSuperior_ClientesInternos = itemGrupo.limSuperior;
                                    break;
                                case (int)TipoRelacionEnum.COLABORADORES:
                                    objEvalPorGrupo.cantCriterios_Colaboradores = 1;
                                    objEvalPorGrupo.limSuperior_Colaboradores = itemGrupo.limSuperior;
                                    break;
                                case (int)TipoRelacionEnum.JEFE:
                                    objEvalPorGrupo.cantCriterios_Jefe = 1;
                                    objEvalPorGrupo.limSuperior_Jefe = itemGrupo.limSuperior;
                                    break;
                                case (int)TipoRelacionEnum.PARES:
                                    objEvalPorGrupo.cantCriterios_Pares = 1;
                                    objEvalPorGrupo.limSuperior_Pares = itemGrupo.limSuperior;
                                    break;
                            }
                            #endregion

                            lstPromedioPorGrupo.Add(objEvalPorGrupo);
                            #endregion
                        }
                        else
                        {
                            #region SE ACTUALIZA SU CANT DE CRITERIOS REGISTRADOS Y EL PROMEDIO
                            objEvalPorGrupo.tipoRelacion = GetTipoRelacion(objDTO.idPeriodo, objDTO.idPersonalEvaluado, itemGrupo.idPersonalEvaluador);
                            switch ((int)objEvalPorGrupo.tipoRelacion)
                            {
                                case (int)TipoRelacionEnum.AUTOEVALUACION:
                                    objEvalPorGrupo.cantCriterios_Autoevaluacion += 1;
                                    objEvalPorGrupo.limSuperior_Autoevaluacion += itemGrupo.limSuperior;
                                    break;
                                case (int)TipoRelacionEnum.CLIENTES_INTERNOS:
                                    objEvalPorGrupo.cantCriterios_ClientesInternos += 1;
                                    objEvalPorGrupo.limSuperior_ClientesInternos += itemGrupo.limSuperior;
                                    break;
                                case (int)TipoRelacionEnum.COLABORADORES:
                                    objEvalPorGrupo.cantCriterios_Colaboradores += 1;
                                    objEvalPorGrupo.limSuperior_Colaboradores += itemGrupo.limSuperior;
                                    break;
                                case (int)TipoRelacionEnum.JEFE:
                                    objEvalPorGrupo.cantCriterios_Jefe += 1;
                                    objEvalPorGrupo.limSuperior_Jefe += itemGrupo.limSuperior;
                                    break;
                                case (int)TipoRelacionEnum.PARES:
                                    objEvalPorGrupo.cantCriterios_Pares += 1;
                                    objEvalPorGrupo.limSuperior_Pares += itemGrupo.limSuperior;
                                    break;
                            }
                            #endregion
                        }
                    }

                    foreach (var itemPromGrupo in lstPromedioPorGrupo)
                    {
                        #region SE OBTIENE EL PROMEDIO POR GRUPO EN BASE AL LIM_SUPERIOR Y CANT_CRITERIOS POR RELACIÓN.
                        if ((decimal)itemPromGrupo.limSuperior_Autoevaluacion > 0 && itemPromGrupo.cantCriterios_Autoevaluacion > 0)
                        {
                            itemPromGrupo.promedio_Autoevaluacion = (decimal)itemPromGrupo.limSuperior_Autoevaluacion / itemPromGrupo.cantCriterios_Autoevaluacion;
                            itemPromGrupo.promedio = (decimal)itemPromGrupo.promedio_Autoevaluacion;

                            itemPromGrupo.R_Autoevaluacion = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= itemPromGrupo.promedio_Autoevaluacion && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.R).FirstOrDefault();
                            itemPromGrupo.G_Autoevaluacion = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= itemPromGrupo.promedio_Autoevaluacion && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.G).FirstOrDefault();
                            itemPromGrupo.B_Autoevaluacion = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= itemPromGrupo.promedio_Autoevaluacion && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.B).FirstOrDefault();
                        }

                        if ((decimal)itemPromGrupo.limSuperior_ClientesInternos > 0 && itemPromGrupo.cantCriterios_ClientesInternos > 0)
                        {
                            itemPromGrupo.promedio_ClientesInternos = (decimal)itemPromGrupo.limSuperior_ClientesInternos / itemPromGrupo.cantCriterios_ClientesInternos;
                            itemPromGrupo.promedio = (decimal)itemPromGrupo.promedio_ClientesInternos;

                            itemPromGrupo.R_ClientesInternos = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= itemPromGrupo.promedio_ClientesInternos && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.R).FirstOrDefault();
                            itemPromGrupo.G_ClientesInternos = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= itemPromGrupo.promedio_ClientesInternos && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.G).FirstOrDefault();
                            itemPromGrupo.B_ClientesInternos = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= itemPromGrupo.promedio_ClientesInternos && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.B).FirstOrDefault();
                        }

                        if ((decimal)itemPromGrupo.limSuperior_Colaboradores > 0 && itemPromGrupo.cantCriterios_Colaboradores > 0)
                        {
                            itemPromGrupo.promedio_Colaboradores = (decimal)itemPromGrupo.limSuperior_Colaboradores / itemPromGrupo.cantCriterios_Colaboradores;
                            itemPromGrupo.promedio = (decimal)itemPromGrupo.promedio_Colaboradores;

                            itemPromGrupo.R_Colaboradores = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= itemPromGrupo.promedio_Colaboradores && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.R).FirstOrDefault();
                            itemPromGrupo.G_Colaboradores = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= itemPromGrupo.promedio_Colaboradores && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.G).FirstOrDefault();
                            itemPromGrupo.B_Colaboradores = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= itemPromGrupo.promedio_Colaboradores && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.B).FirstOrDefault();
                        }

                        if ((decimal)itemPromGrupo.limSuperior_Jefe > 0 && itemPromGrupo.cantCriterios_Jefe > 0)
                        {
                            itemPromGrupo.promedio_Jefe = (decimal)itemPromGrupo.limSuperior_Jefe / itemPromGrupo.cantCriterios_Jefe;
                            itemPromGrupo.promedio = (decimal)itemPromGrupo.promedio_Jefe;

                            itemPromGrupo.R_Jefe = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= itemPromGrupo.promedio_Jefe && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.R).FirstOrDefault();
                            itemPromGrupo.G_Jefe = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= itemPromGrupo.promedio_Jefe && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.G).FirstOrDefault();
                            itemPromGrupo.B_Jefe = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= itemPromGrupo.promedio_Jefe && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.B).FirstOrDefault();
                        }

                        if ((decimal)itemPromGrupo.limSuperior_Pares > 0 && itemPromGrupo.cantCriterios_Pares > 0)
                        {
                            itemPromGrupo.promedio_Pares = (decimal)itemPromGrupo.limSuperior_Pares / itemPromGrupo.cantCriterios_Pares;
                            itemPromGrupo.promedio = (decimal)itemPromGrupo.promedio_Pares;

                            itemPromGrupo.R_Pares = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= itemPromGrupo.promedio_Pares && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.R).FirstOrDefault();
                            itemPromGrupo.G_Pares = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= itemPromGrupo.promedio_Pares && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.G).FirstOrDefault();
                            itemPromGrupo.B_Pares = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= itemPromGrupo.promedio_Pares && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.B).FirstOrDefault();
                        }
                        #endregion
                    }
                }

                #region SE OBTIENE EL PROMEDIO POR FILA (CELDA FINAL)
                foreach (var itemPromGrupo in lstPromedioPorGrupo)
                {

                    List<Reporte360DTO> lstPromedios = lstPromedioPorGrupo.Where(w => w.idGrupo == itemPromGrupo.idGrupo).ToList();

                    foreach (var itemPromedio in lstPromedios)
                    {
                        decimal promedio_Autoevaluacion = (decimal)itemPromedio.promedio_Autoevaluacion;
                        decimal promedio_ClientesInternos = (decimal)itemPromedio.promedio_ClientesInternos;
                        decimal promedio_Colaboradores = (decimal)itemPromedio.promedio_Colaboradores;
                        decimal promedio_Jefe = (decimal)itemPromedio.promedio_Jefe;
                        decimal promedio_Pares = (decimal)itemPromedio.promedio_Pares;
                        decimal sumaPromedios = 0;

                        int contadorPromedios = 0;
                        if (promedio_Autoevaluacion > 0) { contadorPromedios++; sumaPromedios = promedio_Autoevaluacion; }
                        if (promedio_ClientesInternos > 0) { contadorPromedios++; sumaPromedios += promedio_ClientesInternos; }
                        if (promedio_Colaboradores > 0) { contadorPromedios++; sumaPromedios += promedio_Colaboradores; }
                        if (promedio_Jefe > 0) { contadorPromedios++; sumaPromedios += promedio_Jefe; }
                        if (promedio_Pares > 0) { contadorPromedios++; sumaPromedios += promedio_Pares; }

                        if (contadorPromedios > 0 && (decimal)sumaPromedios > 0)
                        {
                            itemPromGrupo.promedio = (decimal)sumaPromedios / contadorPromedios;
                            itemPromGrupo.R_Promedio = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= itemPromedio.promedio && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.R).FirstOrDefault();
                            itemPromGrupo.G_Promedio = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= itemPromedio.promedio && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.G).FirstOrDefault();
                            itemPromGrupo.B_Promedio = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= itemPromedio.promedio && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.B).FirstOrDefault();

                            int contador = 0;
                            foreach (var item in lstCriteriosRelCuestionario.OrderBy(o => o.limInferior))
                            {
                                if (itemPromGrupo.R_Promedio == item.R && itemPromGrupo.G_Promedio == item.G && itemPromGrupo.B_Promedio == item.B)
                                    itemPromGrupo.ordenImagenPromedio = contador;
                                else
                                    contador++;
                            }
                        }
                    }
                }
                #endregion

                #region SE AGREGA PROMEDIOS POR COLUMNA
                Reporte360DTO objPromedioColumnaGrupo = new Reporte360DTO();
                decimal sumaPromedioGrupo = 0;

                #region AUTOEVALUACION
                int tipoRelacion = (int)TipoRelacionEnum.AUTOEVALUACION;
                objPromedioColumnaGrupo = new Reporte360DTO();
                objPromedioColumnaGrupo.idGrupo = -1;
                objPromedioColumnaGrupo.nombreGrupo = "Promedio";
                objPromedioColumnaGrupo.cantCriterios = lstPromedioPorGrupo.Where(w => w.promedio_Autoevaluacion > 0).Count();

                sumaPromedioGrupo = lstPromedioPorGrupo.Sum(s => s.promedio_Autoevaluacion);
                if (objPromedioColumnaGrupo.cantCriterios > 0 && (decimal)sumaPromedioGrupo > 0)
                {
                    objPromedioColumnaGrupo.promedio_Autoevaluacion = (decimal)sumaPromedioGrupo / objPromedioColumnaGrupo.cantCriterios;
                    objPromedioColumnaGrupo.R_Autoevaluacion = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= objPromedioColumnaGrupo.promedio_Autoevaluacion && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.R).FirstOrDefault();
                    objPromedioColumnaGrupo.G_Autoevaluacion = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= objPromedioColumnaGrupo.promedio_Autoevaluacion && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.G).FirstOrDefault();
                    objPromedioColumnaGrupo.B_Autoevaluacion = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= objPromedioColumnaGrupo.promedio_Autoevaluacion && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.B).FirstOrDefault();
                }

                lstPromedioPorGrupo.Add(objPromedioColumnaGrupo);
                #endregion

                #region COLABORADORES
                tipoRelacion = (int)TipoRelacionEnum.COLABORADORES;
                objPromedioColumnaGrupo = new Reporte360DTO();
                objPromedioColumnaGrupo = lstPromedioPorGrupo.Where(w => w.idGrupo == -1).FirstOrDefault();
                objPromedioColumnaGrupo.cantCriterios = lstPromedioPorGrupo.Where(w => w.promedio_Colaboradores > 0).Count();

                sumaPromedioGrupo = lstPromedioPorGrupo.Sum(s => s.promedio_Colaboradores);
                if (objPromedioColumnaGrupo.cantCriterios > 0 && (decimal)sumaPromedioGrupo > 0)
                {
                    objPromedioColumnaGrupo.promedio_Colaboradores = (decimal)sumaPromedioGrupo / objPromedioColumnaGrupo.cantCriterios;
                    objPromedioColumnaGrupo.R_Colaboradores = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= objPromedioColumnaGrupo.promedio_Colaboradores && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.R).FirstOrDefault();
                    objPromedioColumnaGrupo.G_Colaboradores = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= objPromedioColumnaGrupo.promedio_Colaboradores && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.G).FirstOrDefault();
                    objPromedioColumnaGrupo.B_Colaboradores = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= objPromedioColumnaGrupo.promedio_Colaboradores && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.B).FirstOrDefault();
                }
                #endregion

                #region PARES
                tipoRelacion = (int)TipoRelacionEnum.PARES;
                objPromedioColumnaGrupo = new Reporte360DTO();
                objPromedioColumnaGrupo = lstPromedioPorGrupo.Where(w => w.idGrupo == -1).FirstOrDefault();
                objPromedioColumnaGrupo.cantCriterios = lstPromedioPorGrupo.Where(w => w.promedio_Pares > 0).Count();

                sumaPromedioGrupo = lstPromedioPorGrupo.Sum(s => s.promedio_Pares);
                if (objPromedioColumnaGrupo.cantCriterios > 0 && (decimal)sumaPromedioGrupo > 0)
                {
                    objPromedioColumnaGrupo.promedio_Pares = (decimal)sumaPromedioGrupo / objPromedioColumnaGrupo.cantCriterios;
                    objPromedioColumnaGrupo.R_Pares = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= objPromedioColumnaGrupo.promedio_Pares && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.R).FirstOrDefault();
                    objPromedioColumnaGrupo.G_Pares = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= objPromedioColumnaGrupo.promedio_Pares && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.G).FirstOrDefault();
                    objPromedioColumnaGrupo.B_Pares = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= objPromedioColumnaGrupo.promedio_Pares && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.B).FirstOrDefault();
                }
                #endregion

                #region JEFE
                tipoRelacion = (int)TipoRelacionEnum.JEFE;
                objPromedioColumnaGrupo = new Reporte360DTO();
                objPromedioColumnaGrupo = lstPromedioPorGrupo.Where(w => w.idGrupo == -1).FirstOrDefault();
                objPromedioColumnaGrupo.cantCriterios = lstPromedioPorGrupo.Where(w => w.promedio_Jefe > 0).Count();

                sumaPromedioGrupo = lstPromedioPorGrupo.Sum(s => s.promedio_Jefe);
                if (objPromedioColumnaGrupo.cantCriterios > 0 && (decimal)sumaPromedioGrupo > 0)
                {
                    objPromedioColumnaGrupo.promedio_Jefe = (decimal)sumaPromedioGrupo / objPromedioColumnaGrupo.cantCriterios;
                    objPromedioColumnaGrupo.R_Jefe = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= objPromedioColumnaGrupo.promedio_Jefe && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.R).FirstOrDefault();
                    objPromedioColumnaGrupo.G_Jefe = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= objPromedioColumnaGrupo.promedio_Jefe && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.G).FirstOrDefault();
                    objPromedioColumnaGrupo.B_Jefe = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= objPromedioColumnaGrupo.promedio_Jefe && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.B).FirstOrDefault();
                }
                #endregion

                #region CLIENTES INTERNOS
                tipoRelacion = (int)TipoRelacionEnum.CLIENTES_INTERNOS;
                objPromedioColumnaGrupo = new Reporte360DTO();
                objPromedioColumnaGrupo = lstPromedioPorGrupo.Where(w => w.idGrupo == -1).FirstOrDefault();
                objPromedioColumnaGrupo.cantCriterios = lstPromedioPorGrupo.Where(w => w.promedio_ClientesInternos > 0).Count();

                sumaPromedioGrupo = lstPromedioPorGrupo.Sum(s => s.promedio_ClientesInternos);
                if (objPromedioColumnaGrupo.cantCriterios > 0 && (decimal)sumaPromedioGrupo > 0)
                {
                    objPromedioColumnaGrupo.promedio_ClientesInternos = (decimal)sumaPromedioGrupo / objPromedioColumnaGrupo.cantCriterios;
                    objPromedioColumnaGrupo.R_ClientesInternos = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= objPromedioColumnaGrupo.promedio_ClientesInternos && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.R).FirstOrDefault();
                    objPromedioColumnaGrupo.G_ClientesInternos = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= objPromedioColumnaGrupo.promedio_ClientesInternos && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.G).FirstOrDefault();
                    objPromedioColumnaGrupo.B_ClientesInternos = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= objPromedioColumnaGrupo.promedio_ClientesInternos && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.B).FirstOrDefault();
                }
                #endregion

                #endregion

                #region SE CONVIERTE SOLAMENTE LA PRIMERA LETRA EN MAYUSCULA AL NOMBRE DEL GRUPO
                foreach (var item in lstPromedioPorGrupo)
                {
                    item.nombreGrupo = item.nombreGrupo;
                }
                #endregion

                #endregion

                #region SE OBTIENE PROMEDIO POR COMPETENCIA
                foreach (var item in lstEvaluaciones)
                {
                    List<Reporte360DTO> lstRespuestasPorCompetencias = _context.Select<Reporte360DTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Construplan,
                        consulta = @"SELECT t3.idCompetencia, t4.nombreCompetencia, t5.limSuperior, t1.idPersonalEvaluador
		                                    FROM tblRH_Eval360_EvaluacionesEvaluador AS t1
		                                    INNER JOIN tblRH_Eval360_EvaluacionesEvaluadorDet AS t2 ON t2.idEvaluacion = t1.id
		                                    INNER JOIN tblRH_Eval360_CatConductas AS t3 ON t3.id = t2.idConducta
		                                    INNER JOIN tblRH_Eval360_CatCompetencias AS t4 ON t4.id = t3.idCompetencia
		                                    INNER JOIN tblRH_Eval360_CatCriterios AS t5 ON t5.id = t2.idCriterio
			                                    WHERE t1.idPeriodo = @idPeriodo AND t1.idPersonalEvaluado = @idPersonalEvaluado AND 
					                                    t1.registroActivo = @registroActivo AND t2.registroActivo = @registroActivo AND 
					                                    t3.registroActivo = @registroActivo AND t4.registroActivo = @registroActivo AND 
					                                    t5.registroActivo = @registroActivo AND t2.idEvaluacion = @idEvaluacion
						                                    ORDER BY t3.idCompetencia",
                        parametros = new { idPeriodo = objDTO.idPeriodo, idPersonalEvaluado = objDTO.idPersonalEvaluado, registroActivo = true, idEvaluacion = item.id }
                    }).ToList();

                    foreach (var itemCompetencia in lstRespuestasPorCompetencias)
                    {
                        Reporte360DTO objEvalPorCompetencia = lstPromedioPorCompetencia.Where(w => w.idCompetencia == itemCompetencia.idCompetencia).FirstOrDefault();
                        if (objEvalPorCompetencia == null)
                        {
                            #region SE AGREGA GRUPO A LA LISTA
                            objEvalPorCompetencia = new Reporte360DTO();
                            objEvalPorCompetencia.idCompetencia = itemCompetencia.idCompetencia;
                            objEvalPorCompetencia.nombreCompetencia = GetNombreCompetencia(itemCompetencia.idCompetencia);
                            objEvalPorCompetencia.tipoRelacion = GetTipoRelacion(objDTO.idPeriodo, objDTO.idPersonalEvaluado, itemCompetencia.idPersonalEvaluador);
                            objEvalPorCompetencia.limSuperior = itemCompetencia.limSuperior;

                            #region SE INDICA QUE HAY UN CRITERIO REGISTRADO COMO INICIAL EN LA RELACIÓN CORRESPONDIENTE
                            switch ((int)objEvalPorCompetencia.tipoRelacion)
                            {
                                case (int)TipoRelacionEnum.AUTOEVALUACION:
                                    objEvalPorCompetencia.cantCriterios_Autoevaluacion = 1;
                                    objEvalPorCompetencia.limSuperior_Autoevaluacion = itemCompetencia.limSuperior;
                                    break;
                                case (int)TipoRelacionEnum.CLIENTES_INTERNOS:
                                    objEvalPorCompetencia.cantCriterios_ClientesInternos = 1;
                                    objEvalPorCompetencia.limSuperior_ClientesInternos = itemCompetencia.limSuperior;
                                    break;
                                case (int)TipoRelacionEnum.COLABORADORES:
                                    objEvalPorCompetencia.cantCriterios_Colaboradores = 1;
                                    objEvalPorCompetencia.limSuperior_Colaboradores = itemCompetencia.limSuperior;
                                    break;
                                case (int)TipoRelacionEnum.JEFE:
                                    objEvalPorCompetencia.cantCriterios_Jefe = 1;
                                    objEvalPorCompetencia.limSuperior_Jefe = itemCompetencia.limSuperior;
                                    break;
                                case (int)TipoRelacionEnum.PARES:
                                    objEvalPorCompetencia.cantCriterios_Pares = 1;
                                    objEvalPorCompetencia.limSuperior_Pares = itemCompetencia.limSuperior;
                                    break;
                            }
                            #endregion

                            lstPromedioPorCompetencia.Add(objEvalPorCompetencia);
                            #endregion
                        }
                        else
                        {
                            #region SE ACTUALIZA SU CANT DE CRITERIOS REGISTRADOS Y EL PROMEDIO
                            objEvalPorCompetencia.tipoRelacion = GetTipoRelacion(objDTO.idPeriodo, objDTO.idPersonalEvaluado, itemCompetencia.idPersonalEvaluador);
                            switch ((int)objEvalPorCompetencia.tipoRelacion)
                            {
                                case (int)TipoRelacionEnum.AUTOEVALUACION:
                                    objEvalPorCompetencia.cantCriterios_Autoevaluacion += 1;
                                    objEvalPorCompetencia.limSuperior_Autoevaluacion += itemCompetencia.limSuperior;
                                    break;
                                case (int)TipoRelacionEnum.CLIENTES_INTERNOS:
                                    objEvalPorCompetencia.cantCriterios_ClientesInternos += 1;
                                    objEvalPorCompetencia.limSuperior_ClientesInternos += itemCompetencia.limSuperior;
                                    break;
                                case (int)TipoRelacionEnum.COLABORADORES:
                                    objEvalPorCompetencia.cantCriterios_Colaboradores += 1;
                                    objEvalPorCompetencia.limSuperior_Colaboradores += itemCompetencia.limSuperior;
                                    break;
                                case (int)TipoRelacionEnum.JEFE:
                                    objEvalPorCompetencia.cantCriterios_Jefe += 1;
                                    objEvalPorCompetencia.limSuperior_Jefe += itemCompetencia.limSuperior;
                                    break;
                                case (int)TipoRelacionEnum.PARES:
                                    objEvalPorCompetencia.cantCriterios_Pares += 1;
                                    objEvalPorCompetencia.limSuperior_Pares += itemCompetencia.limSuperior;
                                    break;
                            }
                            #endregion
                        }
                    }

                    foreach (var itemPromCompetencia in lstPromedioPorCompetencia)
                    {
                        #region SE OBTIENE EL PROMEDIO POR GRUPO EN BASE AL LIM_SUPERIOR Y CANT_CRITERIOS POR RELACIÓN.
                        int cantPromedios = 0;
                        if ((decimal)itemPromCompetencia.limSuperior_Autoevaluacion > 0 && itemPromCompetencia.cantCriterios_Autoevaluacion > 0)
                        {
                            itemPromCompetencia.promedio_Autoevaluacion = (decimal)itemPromCompetencia.limSuperior_Autoevaluacion / itemPromCompetencia.cantCriterios_Autoevaluacion;
                            itemPromCompetencia.promedio = itemPromCompetencia.promedio_Autoevaluacion;
                            cantPromedios++;

                            itemPromCompetencia.R_Autoevaluacion = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= itemPromCompetencia.promedio_Autoevaluacion && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.R).FirstOrDefault();
                            itemPromCompetencia.G_Autoevaluacion = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= itemPromCompetencia.promedio_Autoevaluacion && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.G).FirstOrDefault();
                            itemPromCompetencia.B_Autoevaluacion = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= itemPromCompetencia.promedio_Autoevaluacion && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.B).FirstOrDefault();
                        }

                        if ((decimal)itemPromCompetencia.limSuperior_ClientesInternos > 0 && itemPromCompetencia.cantCriterios_ClientesInternos > 0)
                        {
                            itemPromCompetencia.promedio_ClientesInternos = (decimal)itemPromCompetencia.limSuperior_ClientesInternos / itemPromCompetencia.cantCriterios_ClientesInternos;
                            itemPromCompetencia.promedio = itemPromCompetencia.promedio_ClientesInternos;
                            cantPromedios++;

                            itemPromCompetencia.R_ClientesInternos = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= itemPromCompetencia.promedio_ClientesInternos && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.R).FirstOrDefault();
                            itemPromCompetencia.G_ClientesInternos = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= itemPromCompetencia.promedio_ClientesInternos && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.G).FirstOrDefault();
                            itemPromCompetencia.B_ClientesInternos = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= itemPromCompetencia.promedio_ClientesInternos && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.B).FirstOrDefault();
                        }

                        if ((decimal)itemPromCompetencia.limSuperior_Colaboradores > 0 && itemPromCompetencia.cantCriterios_Colaboradores > 0)
                        {
                            itemPromCompetencia.promedio_Colaboradores = (decimal)itemPromCompetencia.limSuperior_Colaboradores / itemPromCompetencia.cantCriterios_Colaboradores;
                            itemPromCompetencia.promedio = itemPromCompetencia.promedio_Colaboradores;
                            cantPromedios++;

                            itemPromCompetencia.R_Colaboradores = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= itemPromCompetencia.promedio_Colaboradores && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.R).FirstOrDefault();
                            itemPromCompetencia.G_Colaboradores = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= itemPromCompetencia.promedio_Colaboradores && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.G).FirstOrDefault();
                            itemPromCompetencia.B_Colaboradores = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= itemPromCompetencia.promedio_Colaboradores && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.B).FirstOrDefault();
                        }

                        if ((decimal)itemPromCompetencia.limSuperior_Jefe > 0 && itemPromCompetencia.cantCriterios_Jefe > 0)
                        {
                            itemPromCompetencia.promedio_Jefe = (decimal)itemPromCompetencia.limSuperior_Jefe / itemPromCompetencia.cantCriterios_Jefe;
                            itemPromCompetencia.promedio = itemPromCompetencia.promedio_Jefe;
                            cantPromedios++;

                            itemPromCompetencia.R_Jefe = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= itemPromCompetencia.promedio_Jefe && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.R).FirstOrDefault();
                            itemPromCompetencia.G_Jefe = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= itemPromCompetencia.promedio_Jefe && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.G).FirstOrDefault();
                            itemPromCompetencia.B_Jefe = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= itemPromCompetencia.promedio_Jefe && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.B).FirstOrDefault();
                        }

                        if ((decimal)itemPromCompetencia.limSuperior_Pares > 0 && itemPromCompetencia.cantCriterios_Pares > 0)
                        {
                            itemPromCompetencia.promedio_Pares = (decimal)itemPromCompetencia.limSuperior_Pares / itemPromCompetencia.cantCriterios_Pares;
                            itemPromCompetencia.promedio = itemPromCompetencia.promedio_Pares;
                            cantPromedios++;

                            itemPromCompetencia.R_Pares = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= itemPromCompetencia.promedio_Pares && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.R).FirstOrDefault();
                            itemPromCompetencia.G_Pares = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= itemPromCompetencia.promedio_Pares && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.G).FirstOrDefault();
                            itemPromCompetencia.B_Pares = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= itemPromCompetencia.promedio_Pares && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.B).FirstOrDefault();
                        }

                        if (cantPromedios > 0)
                            itemPromCompetencia.promedio = (decimal)itemPromCompetencia.promedio / cantPromedios;
                        #endregion
                    }

                    foreach (var itemCompetencia in lstPromedioPorCompetencia)
                    {
                        itemCompetencia.nombreCompetencia = itemCompetencia.nombreCompetencia;
                    }
                }

                #region SE OBTIENE EL PROMEDIO POR FILA (CELDA FINAL)
                foreach (var itemPromCompetencia in lstPromedioPorCompetencia)
                {

                    List<Reporte360DTO> lstPromedios = lstPromedioPorCompetencia.Where(w => w.idCompetencia == itemPromCompetencia.idCompetencia).ToList();

                    foreach (var itemPromedio in lstPromedios)
                    {
                        decimal promedio_Autoevaluacion = (decimal)itemPromedio.promedio_Autoevaluacion;
                        decimal promedio_ClientesInternos = (decimal)itemPromedio.promedio_ClientesInternos;
                        decimal promedio_Colaboradores = (decimal)itemPromedio.promedio_Colaboradores;
                        decimal promedio_Jefe = (decimal)itemPromedio.promedio_Jefe;
                        decimal promedio_Pares = (decimal)itemPromedio.promedio_Pares;
                        decimal sumaPromedios = 0;

                        int contadorPromedios = 0;
                        if (promedio_Autoevaluacion > 0) { contadorPromedios++; sumaPromedios = promedio_Autoevaluacion; }
                        if (promedio_ClientesInternos > 0) { contadorPromedios++; sumaPromedios += promedio_ClientesInternos; }
                        if (promedio_Colaboradores > 0) { contadorPromedios++; sumaPromedios += promedio_Colaboradores; }
                        if (promedio_Jefe > 0) { contadorPromedios++; sumaPromedios += promedio_Jefe; }
                        if (promedio_Pares > 0) { contadorPromedios++; sumaPromedios += promedio_Pares; }

                        if (contadorPromedios > 0 && (decimal)sumaPromedios > 0)
                        {
                            itemPromCompetencia.promedio = (decimal)sumaPromedios / contadorPromedios;
                            itemPromCompetencia.R_Promedio = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= itemPromCompetencia.promedio && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.R).FirstOrDefault();
                            itemPromCompetencia.G_Promedio = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= itemPromCompetencia.promedio && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.G).FirstOrDefault();
                            itemPromCompetencia.B_Promedio = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= itemPromCompetencia.promedio && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.B).FirstOrDefault();

                            int contador = 0;
                            foreach (var item in lstCriteriosRelCuestionario.OrderBy(o => o.limInferior))
                            {
                                if (itemPromCompetencia.R_Promedio == item.R && itemPromCompetencia.G_Promedio == item.G && itemPromCompetencia.B_Promedio == item.B)
                                    itemPromCompetencia.ordenImagenPromedio = contador;
                                else
                                    contador++;
                            }
                        }
                    }
                }
                #endregion

                #region SE AGREGA PROMEDIO POR COLUMNA
                Reporte360DTO objPromedioColumnaCompetencia = new Reporte360DTO();
                decimal sumaPromedioCompetencia = 0;

                #region AUTOEVALUACION
                tipoRelacion = (int)TipoRelacionEnum.AUTOEVALUACION;
                objPromedioColumnaCompetencia = new Reporte360DTO();
                objPromedioColumnaCompetencia.idCompetencia = -1;
                objPromedioColumnaCompetencia.nombreCompetencia = "Promedio";
                objPromedioColumnaCompetencia.cantCriterios = lstPromedioPorCompetencia.Where(w => w.promedio_Autoevaluacion > 0).Count();

                sumaPromedioCompetencia = lstPromedioPorCompetencia.Sum(s => s.promedio_Autoevaluacion);
                if (objPromedioColumnaCompetencia.cantCriterios > 0 && (decimal)sumaPromedioCompetencia > 0)
                {
                    objPromedioColumnaCompetencia.promedio_Autoevaluacion = (decimal)sumaPromedioCompetencia / objPromedioColumnaCompetencia.cantCriterios;
                    objPromedioColumnaCompetencia.R_Autoevaluacion = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= objPromedioColumnaCompetencia.promedio_Autoevaluacion && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.R).FirstOrDefault();
                    objPromedioColumnaCompetencia.G_Autoevaluacion = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= objPromedioColumnaCompetencia.promedio_Autoevaluacion && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.G).FirstOrDefault();
                    objPromedioColumnaCompetencia.B_Autoevaluacion = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= objPromedioColumnaCompetencia.promedio_Autoevaluacion && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.B).FirstOrDefault();
                }

                lstPromedioPorCompetencia.Add(objPromedioColumnaCompetencia);
                #endregion

                #region CLIENTES INTERNOS
                tipoRelacion = (int)TipoRelacionEnum.CLIENTES_INTERNOS;
                objPromedioColumnaCompetencia = lstPromedioPorCompetencia.Where(w => w.idCompetencia == -1).FirstOrDefault();
                objPromedioColumnaCompetencia.cantCriterios = lstPromedioPorCompetencia.Where(w => w.promedio_ClientesInternos > 0).Count();

                sumaPromedioCompetencia = lstPromedioPorCompetencia.Sum(s => s.promedio_ClientesInternos);
                if (objPromedioColumnaCompetencia.cantCriterios > 0 && (decimal)sumaPromedioCompetencia > 0)
                {
                    objPromedioColumnaCompetencia.promedio_ClientesInternos = (decimal)sumaPromedioCompetencia / objPromedioColumnaCompetencia.cantCriterios;
                    objPromedioColumnaCompetencia.R_ClientesInternos = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= objPromedioColumnaCompetencia.promedio_ClientesInternos && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.R).FirstOrDefault();
                    objPromedioColumnaCompetencia.G_ClientesInternos = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= objPromedioColumnaCompetencia.promedio_ClientesInternos && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.G).FirstOrDefault();
                    objPromedioColumnaCompetencia.B_ClientesInternos = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= objPromedioColumnaCompetencia.promedio_ClientesInternos && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.B).FirstOrDefault();
                }
                #endregion

                #region COLABORADORES
                tipoRelacion = (int)TipoRelacionEnum.COLABORADORES;
                objPromedioColumnaCompetencia = lstPromedioPorCompetencia.Where(w => w.idCompetencia == -1).FirstOrDefault();
                objPromedioColumnaCompetencia.cantCriterios = lstPromedioPorCompetencia.Where(w => w.promedio_Colaboradores > 0).Count();

                sumaPromedioCompetencia = lstPromedioPorCompetencia.Sum(s => s.promedio_Colaboradores);
                if (objPromedioColumnaCompetencia.cantCriterios > 0 && (decimal)sumaPromedioCompetencia > 0)
                {
                    objPromedioColumnaCompetencia.promedio_Colaboradores = (decimal)sumaPromedioCompetencia / objPromedioColumnaCompetencia.cantCriterios;
                    objPromedioColumnaCompetencia.R_Colaboradores = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= objPromedioColumnaCompetencia.promedio_Colaboradores && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.R).FirstOrDefault();
                    objPromedioColumnaCompetencia.G_Colaboradores = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= objPromedioColumnaCompetencia.promedio_Colaboradores && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.G).FirstOrDefault();
                    objPromedioColumnaCompetencia.B_Colaboradores = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= objPromedioColumnaCompetencia.promedio_Colaboradores && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.B).FirstOrDefault();
                }
                #endregion

                #region JEFE
                tipoRelacion = (int)TipoRelacionEnum.JEFE;
                objPromedioColumnaCompetencia = lstPromedioPorCompetencia.Where(w => w.idCompetencia == -1).FirstOrDefault();
                objPromedioColumnaCompetencia.cantCriterios = lstPromedioPorCompetencia.Where(w => w.promedio_Jefe > 0).Count();

                sumaPromedioCompetencia = lstPromedioPorCompetencia.Sum(s => s.promedio_Jefe);
                if (objPromedioColumnaCompetencia.cantCriterios > 0 && (decimal)sumaPromedioCompetencia > 0)
                {
                    objPromedioColumnaCompetencia.promedio_Jefe = (decimal)sumaPromedioCompetencia / objPromedioColumnaCompetencia.cantCriterios;
                    objPromedioColumnaCompetencia.R_Jefe = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= objPromedioColumnaCompetencia.promedio_Jefe && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.R).FirstOrDefault();
                    objPromedioColumnaCompetencia.G_Jefe = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= objPromedioColumnaCompetencia.promedio_Jefe && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.G).FirstOrDefault();
                    objPromedioColumnaCompetencia.B_Jefe = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= objPromedioColumnaCompetencia.promedio_Jefe && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.B).FirstOrDefault();
                }
                #endregion

                #region PARES
                tipoRelacion = (int)TipoRelacionEnum.PARES;
                objPromedioColumnaCompetencia = lstPromedioPorCompetencia.Where(w => w.idCompetencia == -1).FirstOrDefault();
                objPromedioColumnaCompetencia.cantCriterios = lstPromedioPorCompetencia.Where(w => w.promedio_Pares > 0).Count();

                sumaPromedioCompetencia = lstPromedioPorCompetencia.Sum(s => s.promedio_Pares);
                if (objPromedioColumnaCompetencia.cantCriterios > 0 && (decimal)sumaPromedioCompetencia > 0)
                {
                    objPromedioColumnaCompetencia.promedio_Pares = (decimal)sumaPromedioCompetencia / objPromedioColumnaCompetencia.cantCriterios;
                    objPromedioColumnaCompetencia.R_Pares = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= objPromedioColumnaCompetencia.promedio_Pares && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.R).FirstOrDefault();
                    objPromedioColumnaCompetencia.G_Pares = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= objPromedioColumnaCompetencia.promedio_Pares && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.G).FirstOrDefault();
                    objPromedioColumnaCompetencia.B_Pares = lstCriteriosRelCuestionario.Where(w => w.limSuperior >= objPromedioColumnaCompetencia.promedio_Pares && w.registroActivo).OrderBy(o => o.limSuperior).Select(s => s.B).FirstOrDefault();
                }
                #endregion

                #endregion

                #endregion

                #endregion

                #region SE OBTIENE IMAGEN MEDIDOR PARA LOS CRITERIOS
                List<string> lstImagenesMedidorCriterio = _context.Select<string>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT imagenBase64 FROM tblRH_Eval360_CatImagenesMedidor ORDER BY ordenMedidor"
                }).ToList();
                #endregion

                #region SE OBTIENE ICONOS
                List<string> lstIconos = _context.Select<string>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = "SELECT imagenBase64 FROM tblRH_Eval360_CatIconosGruposCompetencias ORDER BY orden"
                }).ToList();
                #endregion

                #region SE OBTIENE EL NOMBRE DEL JEFE
                objDTO.nombreJefe = string.Empty;
                List<RelacionDetDTO> lstJefes = _context.Select<RelacionDetDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT t1.idPeriodo, idPersonalEvaluado, t2.idPersonalEvaluador, t2.tipoRelacion
	                                    FROM tblRH_Eval360_Relaciones AS t1
	                                    INNER JOIN tblRH_Eval360_RelacionesDet AS t2 ON t2.idRelacion = t1.id
		                                    WHERE t1.idPeriodo = @idPeriodo AND t1.idPersonalEvaluado = @idPersonalEvaluado AND t2.tipoRelacion = @tipoRelacion AND t1.registroActivo = @registroActivo AND t2.registroActivo = @registroActivo",
                    parametros = new { idPeriodo = objDTO.idPeriodo, idPersonalEvaluado = objDTO.idPersonalEvaluado, tipoRelacion = (int)TipoRelacionEnum.JEFE, registroActivo = true }
                }).ToList();
                if (lstJefes.Count() > 0)
                    objDTO.nombreJefe = SetPrimeraLetraMayuscula(GetNombreCompletoUsuarioSIGOPLAN(GetUsuarioID(lstJefes[0].idPersonalEvaluador), GetEmpresaPersonal(lstJefes[0].idPersonalEvaluador)));
                #endregion

                #region SE INDICA SI SE GENERA EL REPORTE O NO
                if (lstPromedioPorGrupo.Count() <= 1 && lstPromedioPorCompetencia.Count() <= 1)
                    throw new Exception("No se encontraron evaluaciones a mostrar en el reporte 360.");
                #endregion

                #region SE OBTIENE EL COMENTARIO GENERAL DE LA EVALUACIÓN
                objReporte360.comentarioGeneral = string.Empty;
                foreach (var item in lstEvaluaciones)
                {
                    List<tblRH_Eval360_EvaluacionesEvaluadorDet> lstEvaluacionDet = _context.tblRH_Eval360_EvaluacionesEvaluadorDet.Where(w => w.idEvaluacion == item.id && w.registroActivo).ToList();
                    foreach (var itemComentario in lstEvaluacionDet)
                    {
                        if (!string.IsNullOrEmpty(itemComentario.comentario))
                        {
                            if (string.IsNullOrEmpty(objReporte360.comentarioGeneral))
                                objReporte360.comentarioGeneral = itemComentario.comentario.Trim();
                            else
                                objReporte360.comentarioGeneral += string.Format(". {0}", itemComentario.comentario.Trim());
                        }
                    }
                }
                #endregion

                #region SE OBTIENE IMAGEN FOOTER
                List<string> lstFooter = _context.Select<string>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT imagenBase64 FROM tblRH_Eval360_CatImagenes",
                    parametros = new { registroActivo = true }
                }).ToList();
                #endregion

                resultado.Add(SUCCESS, true);
                resultado.Add("nombreCompleto", objDTO.nombreCompleto);
                resultado.Add("puesto", objDTO.puesto);
                resultado.Add("fechaIngreso", objDTO.fechaIngreso);
                resultado.Add("lstCriteriosRelCuestionario", lstCriteriosRelCuestionario);
                resultado.Add("lstPromedioPorGrupo", lstPromedioPorGrupo);
                resultado.Add("lstPromedioPorCompetencia", lstPromedioPorCompetencia);
                resultado.Add("lstImagenesMedidorCriterio", lstImagenesMedidorCriterio);
                resultado.Add("lstIconos", lstIconos);
                resultado.Add("nombreJefe", objDTO.nombreJefe);
                resultado.Add("comentarioGeneral", objReporte360.comentarioGeneral);
                resultado.Add("lstFooter", lstFooter);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GenerarReporte360", e, AccionEnum.CONSULTA, 0, objDTO);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> GetCompetenciasRelEvaluado(Reporte360DTO objDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                string mensajeError = "Ocurrió un error al generar el reporte 360.";
                if (objDTO.idRelacion <= 0) { throw new Exception(mensajeError); }
                if (objDTO.idPersonalEvaluado <= 0) { throw new Exception(mensajeError); }
                #endregion

                #region SE OBTIENE LA INFORMACIÓN A MOSTRAR EN EL REPORTE 360

                #region SE OBTIENE LISTADO DE CRITERIOS LIGADOS AL CUESTIONARIO DEL EVALUADO
                int idCuestionario = _context.tblRH_Eval360_RelacionesDet.Where(w => w.idRelacion == objDTO.idRelacion && w.registroActivo).Select(s => s.idCuestionario).FirstOrDefault();
                if (idCuestionario <= 0)
                    throw new Exception(mensajeError);

                List<tblRH_Eval360_CatCriterios> lstCriteriosRelCuestionario = _context.tblRH_Eval360_CatCriterios.Where(w => w.idCuestionario == idCuestionario && w.registroActivo).ToList();
                #endregion

                #region SE OBTIENE PROMEDIOS
                List<Reporte360DTO> lstPromedioPorGrupo = new List<Reporte360DTO>();
                List<Reporte360DTO> lstPromedioPorCompetencia = new List<Reporte360DTO>();
                Reporte360DTO objReporte360 = new Reporte360DTO();

                // SE OBTIENE ID_PERIODO
                objDTO.idPeriodo = _context.tblRH_Eval360_Relaciones.Where(w => w.id == objDTO.idRelacion && w.registroActivo).Select(s => s.idPeriodo).FirstOrDefault();
                if (objDTO.idPeriodo <= 0)
                    throw new Exception(mensajeError);

                // SE OBTIENE LAS EVALUACIONES APLICADAS DEL EVALUADO SELECCIONADO
                List<tblRH_Eval360_EvaluacionesEvaluador> lstEvaluaciones = _context.tblRH_Eval360_EvaluacionesEvaluador.Where(w => w.idPeriodo == objDTO.idPeriodo && w.idPersonalEvaluado == objDTO.idPersonalEvaluado && w.registroActivo).ToList();
                if (lstEvaluaciones.Count() <= 0)
                    throw new Exception("No se encuentra ninguna evaluación registrada.");

                #region SE OBTIENE PROMEDIO POR COMPETENCIA
                foreach (var item in lstEvaluaciones)
                {
                    List<Reporte360DTO> lstRespuestasPorCompetencias = _context.Select<Reporte360DTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Construplan,
                        consulta = @"SELECT t3.idCompetencia, t4.nombreCompetencia, t5.limSuperior, t1.idPersonalEvaluador
		                                    FROM tblRH_Eval360_EvaluacionesEvaluador AS t1
		                                    INNER JOIN tblRH_Eval360_EvaluacionesEvaluadorDet AS t2 ON t2.idEvaluacion = t1.id
		                                    INNER JOIN tblRH_Eval360_CatConductas AS t3 ON t3.id = t2.idConducta
		                                    INNER JOIN tblRH_Eval360_CatCompetencias AS t4 ON t4.id = t3.idCompetencia
		                                    INNER JOIN tblRH_Eval360_CatCriterios AS t5 ON t5.id = t2.idCriterio
			                                    WHERE t1.idPeriodo = @idPeriodo AND t1.idPersonalEvaluado = @idPersonalEvaluado AND 
					                                    t1.registroActivo = @registroActivo AND t2.registroActivo = @registroActivo AND 
					                                    t3.registroActivo = @registroActivo AND t4.registroActivo = @registroActivo AND 
					                                    t5.registroActivo = @registroActivo AND t2.idEvaluacion = @idEvaluacion
						                                    ORDER BY t3.idCompetencia",
                        parametros = new { idPeriodo = objDTO.idPeriodo, idPersonalEvaluado = objDTO.idPersonalEvaluado, registroActivo = true, idEvaluacion = item.id }
                    }).ToList();

                    foreach (var itemCompetencia in lstRespuestasPorCompetencias)
                    {
                        Reporte360DTO objEvalPorCompetencia = lstPromedioPorCompetencia.Where(w => w.idCompetencia == itemCompetencia.idCompetencia).FirstOrDefault();
                        if (objEvalPorCompetencia == null)
                        {
                            #region SE AGREGA GRUPO A LA LISTA
                            objEvalPorCompetencia = new Reporte360DTO();
                            objEvalPorCompetencia.idCompetencia = itemCompetencia.idCompetencia;
                            objEvalPorCompetencia.nombreCompetencia = GetNombreCompetencia(itemCompetencia.idCompetencia);
                            objEvalPorCompetencia.tipoRelacion = GetTipoRelacion(objDTO.idPeriodo, objDTO.idPersonalEvaluado, itemCompetencia.idPersonalEvaluador);
                            objEvalPorCompetencia.limSuperior = itemCompetencia.limSuperior;

                            #region SE INDICA QUE HAY UN CRITERIO REGISTRADO COMO INICIAL EN LA RELACIÓN CORRESPONDIENTE
                            switch ((int)objEvalPorCompetencia.tipoRelacion)
                            {
                                case (int)TipoRelacionEnum.AUTOEVALUACION:
                                    objEvalPorCompetencia.cantCriterios_Autoevaluacion = 1;
                                    objEvalPorCompetencia.limSuperior_Autoevaluacion = itemCompetencia.limSuperior;
                                    break;
                                case (int)TipoRelacionEnum.CLIENTES_INTERNOS:
                                    objEvalPorCompetencia.cantCriterios_ClientesInternos = 1;
                                    objEvalPorCompetencia.limSuperior_ClientesInternos = itemCompetencia.limSuperior;
                                    break;
                                case (int)TipoRelacionEnum.COLABORADORES:
                                    objEvalPorCompetencia.cantCriterios_Colaboradores = 1;
                                    objEvalPorCompetencia.limSuperior_Colaboradores = itemCompetencia.limSuperior;
                                    break;
                                case (int)TipoRelacionEnum.JEFE:
                                    objEvalPorCompetencia.cantCriterios_Jefe = 1;
                                    objEvalPorCompetencia.limSuperior_Jefe = itemCompetencia.limSuperior;
                                    break;
                                case (int)TipoRelacionEnum.PARES:
                                    objEvalPorCompetencia.cantCriterios_Pares = 1;
                                    objEvalPorCompetencia.limSuperior_Pares = itemCompetencia.limSuperior;
                                    break;
                            }
                            #endregion

                            lstPromedioPorCompetencia.Add(objEvalPorCompetencia);
                            #endregion
                        }
                        else
                        {
                            #region SE ACTUALIZA SU CANT DE CRITERIOS REGISTRADOS Y EL PROMEDIO
                            switch ((int)objEvalPorCompetencia.tipoRelacion)
                            {
                                case (int)TipoRelacionEnum.AUTOEVALUACION:
                                    objEvalPorCompetencia.cantCriterios_Autoevaluacion += 1;
                                    objEvalPorCompetencia.limSuperior_Autoevaluacion += itemCompetencia.limSuperior;
                                    break;
                                case (int)TipoRelacionEnum.CLIENTES_INTERNOS:
                                    objEvalPorCompetencia.cantCriterios_ClientesInternos += 1;
                                    objEvalPorCompetencia.limSuperior_ClientesInternos += itemCompetencia.limSuperior;
                                    break;
                                case (int)TipoRelacionEnum.COLABORADORES:
                                    objEvalPorCompetencia.cantCriterios_Colaboradores += 1;
                                    objEvalPorCompetencia.limSuperior_Colaboradores += itemCompetencia.limSuperior;
                                    break;
                                case (int)TipoRelacionEnum.JEFE:
                                    objEvalPorCompetencia.cantCriterios_Jefe += 1;
                                    objEvalPorCompetencia.limSuperior_Jefe += itemCompetencia.limSuperior;
                                    break;
                                case (int)TipoRelacionEnum.PARES:
                                    objEvalPorCompetencia.cantCriterios_Pares += 1;
                                    objEvalPorCompetencia.limSuperior_Pares += itemCompetencia.limSuperior;
                                    break;
                            }
                            #endregion
                        }
                    }

                    foreach (var itemPromCompetencia in lstPromedioPorCompetencia)
                    {
                        #region SE OBTIENE EL PROMEDIO POR GRUPO EN BASE AL LIM_SUPERIOR Y CANT_CRITERIOS POR RELACIÓN.
                        if ((decimal)itemPromCompetencia.limSuperior_Autoevaluacion > 0 && itemPromCompetencia.cantCriterios_Autoevaluacion > 0)
                            itemPromCompetencia.promedio = (decimal)itemPromCompetencia.limSuperior_Autoevaluacion / itemPromCompetencia.cantCriterios_Autoevaluacion;

                        if ((decimal)itemPromCompetencia.limSuperior_ClientesInternos > 0 && itemPromCompetencia.cantCriterios_ClientesInternos > 0)
                            itemPromCompetencia.promedio = (decimal)itemPromCompetencia.limSuperior_ClientesInternos / itemPromCompetencia.cantCriterios_ClientesInternos;

                        if ((decimal)itemPromCompetencia.limSuperior_Colaboradores > 0 && itemPromCompetencia.cantCriterios_Colaboradores > 0)
                            itemPromCompetencia.promedio = (decimal)itemPromCompetencia.limSuperior_Colaboradores / itemPromCompetencia.cantCriterios_Colaboradores;

                        if ((decimal)itemPromCompetencia.limSuperior_Jefe > 0 && itemPromCompetencia.cantCriterios_Jefe > 0)
                            itemPromCompetencia.promedio = (decimal)itemPromCompetencia.limSuperior_Jefe / itemPromCompetencia.cantCriterios_Jefe;

                        if ((decimal)itemPromCompetencia.limSuperior_Pares > 0 && itemPromCompetencia.cantCriterios_Pares > 0)
                            itemPromCompetencia.promedio = (decimal)itemPromCompetencia.limSuperior_Pares / itemPromCompetencia.cantCriterios_Pares;
                        #endregion
                    }
                }

                Reporte360GraficaDTO objGraficaDTO = new Reporte360GraficaDTO();
                List<string> lstConductas = new List<string>();
                List<decimal> lstPromedios = new List<decimal>();
                foreach (var item in lstPromedioPorCompetencia)
                {
                    lstConductas.Add(item.nombreCompetencia);
                    decimal promedio = Convert.ToDecimal(item.promedio.ToString("N2"));
                    lstPromedios.Add(promedio);
                }
                objGraficaDTO.lstConductas = new List<string>();
                objGraficaDTO.lstConductas.AddRange(lstConductas);

                objGraficaDTO.lstPromedios = new List<decimal>();
                objGraficaDTO.lstPromedios.AddRange(lstPromedios);
                #endregion

                #endregion

                resultado.Add(SUCCESS, true);
                resultado.Add("objGraficaDTO", objGraficaDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetCompetenciasRelEvaluado", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        #endregion

        #region AVANCES
        public Dictionary<string, object> GetEstatusEvaluadores(EstatusEvaluadorDTO objFiltroDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (objFiltroDTO.idPeriodo <= 0) { throw new Exception("Es necesario seleccionar un periodo."); }
                #endregion

                #region SE OBTIENE EL ESTATUS POR CADA EVALUADOR DEL PERIODO SELECCIONADO

                // SE OBTIENE LISTADO DE EVALUADORES
                List<EstatusEvaluadorDTO> lstEvaluadores = _context.Select<EstatusEvaluadorDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT t1.idPersonalEvaluado, t1.idPersonalEvaluador, t1.idCuestionario, t2.nombreCuestionario
	                                    FROM tblRH_Eval360_EvaluacionesEvaluador AS t1
                                        INNER JOIN tblRH_Eval360_Cuestionarios AS t2 ON t2.id = t1.idCuestionario
			                                WHERE t1.idPeriodo = @idPeriodo AND t1.registroActivo = @registroActivo AND t2.registroActivo = @registroActivo",
                    parametros = new { idPeriodo = objFiltroDTO.idPeriodo, registroActivo = true }
                }).ToList();

                // SE ARMA DTO DE EVALUADORES PARA MOSTRARLO EN EL DATATABLE
                List<EstatusEvaluadorDTO> lstEvaluadoresDTO = new List<EstatusEvaluadorDTO>();
                EstatusEvaluadorDTO objDTO = new EstatusEvaluadorDTO();
                int contador = 1;
                int cantNoIniciadas = 0;
                int cantEnProceso = 0;
                int cantContestadas = 0;
                foreach (var item in lstEvaluadores)
                {
                    objDTO = new EstatusEvaluadorDTO();
                    objDTO.idPersonalEvaluador = item.idPersonalEvaluador;
                    objDTO.numRegistro = contador;
                    objDTO.nombreEvaluador = GetNombreCompletoUsuarioSIGOPLAN(GetUsuarioID(item.idPersonalEvaluador), GetEmpresaPersonal(item.idPersonalEvaluador));
                    objDTO.nombreEvaluado = GetNombreCompletoUsuarioSIGOPLAN(GetUsuarioID(item.idPersonalEvaluado), GetEmpresaPersonal(item.idPersonalEvaluado));
                    objDTO.descripcionTipoRelacion = GetTipoRelacionDescripcion(GetTipoRelacion(objFiltroDTO.idPeriodo, item.idPersonalEvaluado, item.idPersonalEvaluador));
                    objDTO.nombreCuestionario = item.nombreCuestionario;
                    objDTO.estatusAvance = GetEstatusCuestionarioEvaluadorRelEvaluado(objFiltroDTO.idPeriodo, item.idPersonalEvaluado, item.idPersonalEvaluador);

                    if (objDTO.estatusAvance == EnumHelper.GetDescription((EstatusCuestionarioEvaluadorEnum.NO_INICIADA)))
                        cantNoIniciadas++;
                    else if (objDTO.estatusAvance == EnumHelper.GetDescription((EstatusCuestionarioEvaluadorEnum.EN_PROCESO)))
                        cantEnProceso++;
                    else if (objDTO.estatusAvance == EnumHelper.GetDescription((EstatusCuestionarioEvaluadorEnum.CONTESTADA)))
                        cantContestadas++;

                    lstEvaluadoresDTO.Add(objDTO);
                    contador++;
                }

                // SE AGREGA EL ESTATUS DE LOS CUESTIONARIOS PARA GRAFICAR LA INFORMACIÓN
                EstatusEvaluadorDTO objEstatusGrafica = new EstatusEvaluadorDTO();
                objEstatusGrafica.cantNoIniciadas = cantNoIniciadas;
                objEstatusGrafica.cantEnProceso = cantEnProceso;
                objEstatusGrafica.cantContestadas = cantContestadas;

                foreach (var item in lstEvaluadoresDTO)
                {
                    item.nombreEvaluador = SetPrimeraLetraMayuscula(item.nombreEvaluador);
                    item.nombreEvaluado = SetPrimeraLetraMayuscula(item.nombreEvaluado);
                    item.descripcionTipoRelacion = item.descripcionTipoRelacion;
                    item.nombreCuestionario = item.nombreCuestionario;
                    item.estatusAvance = item.estatusAvance;
                }

                resultado.Add(SUCCESS, true);
                resultado.Add("lstEvaluadoresDTO", lstEvaluadoresDTO);
                resultado.Add("objEstatusGrafica", objEstatusGrafica);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetEstatusEvaluadores", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> EnviarCorreo(List<int> lstPersonalEvaluadorID, int idPeriodo)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    if (lstPersonalEvaluadorID == null) { throw new Exception("Es necesario seleccionar al menos un usuario."); }
                    if (idPeriodo <= 0) { throw new Exception("Es necesario seleccionar al menos un periodo."); }
                    #endregion

                    #region SE ENVIA CORREO A LOS USUARIOS CORRESPONDIENTES
                    List<RelacionDetDTO> lstRelacionesDet = new List<RelacionDetDTO>();
                    string strQuery = string.Format(@"SELECT t3.id, t4.correo
	                                                        FROM tblRH_Eval360_RelacionesDet AS t1
	                                                        INNER JOIN tblRH_Eval360_Cuestionarios AS t2 ON t1.idCuestionario = t2.id
	                                                        INNER JOIN tblRH_Eval360_CatPersonal AS t3 ON t3.id = t1.idPersonalEvaluador
	                                                        INNER JOIN tblP_Usuario AS t4 ON t4.id = t3.idUsuario
		                                                        WHERE t1.registroActivo = {0} AND t2.registroActivo = {0} AND t3.registroActivo = {0} AND t4.estatus = {0} AND 
                                                                      t1.idPersonalEvaluador IN ({1})
				                                                        GROUP BY t3.id, t4.correo", 1, string.Join(",", lstPersonalEvaluadorID));
                    lstRelacionesDet = _context.Select<RelacionDetDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Construplan,
                        consulta = strQuery
                    }).ToList();

                    if (lstRelacionesDet.Count() <= 0) { throw new Exception("Ocurrió un error al enviar el correo."); }
                    List<string> lstCorreos = new List<string>();
                    foreach (var item in lstRelacionesDet)
                    {
                        lstCorreos.Add(item.correo);
                    }

                    #region SE REGISTRA/AUMENTA QUE SE ENVIO UN CORREO AL EVALUADOR EN EL PERIODO SELECCIONADO
                    //bool esPrimerEnvioCorreo = false;
                    //foreach (var item in lstRelacionesDet)
                    //{
                    //    tblRH_Eval360_BitacoraEnvioCorreos objCE = _context.tblRH_Eval360_BitacoraEnvioCorreos.Where(w => w.idPersonal == item.id).FirstOrDefault();
                    //    if (objCE == null)
                    //    {
                    //        #region NUEVO REGISTRO | BITACORA
                    //        objCE = new tblRH_Eval360_BitacoraEnvioCorreos();
                    //        objCE.idPersonal = item.id;
                    //        objCE.idPeriodo = idPeriodo;
                    //        objCE.cantEnvioCorreos = 1;
                    //        objCE.idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                    //        objCE.fechaCreacion = DateTime.Now;
                    //        objCE.registroActivo = true;
                    //        _context.tblRH_Eval360_BitacoraEnvioCorreos.Add(objCE);
                    //        _context.SaveChanges();
                    //        #endregion

                    //        #region SE INDICA QUE YA SE ENVIO CORREO POR PRIMERA VEZ AL PERSONAL
                    //        tblRH_Eval360_CatPersonal objPersonal = _context.tblRH_Eval360_CatPersonal.Where(w => w.id == item.id).FirstOrDefault();
                    //        if (objPersonal == null)
                    //            throw new Exception("Ocurrió un error al enviar el correo.");

                    //        esPrimerEnvioCorreo = true;
                    //        objPersonal.esPrimerEnvioCorreo = false;
                    //        _context.SaveChanges();
                    //        #endregion
                    //    }
                    //    else
                    //    {
                    //        #region ACTUALIZAR REGISTRO
                    //        objCE.cantEnvioCorreos += 1;
                    //        objCE.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                    //        objCE.fechaModificacion = DateTime.Now;
                    //        _context.SaveChanges();
                    //        #endregion
                    //    }
                    //}
                    #endregion

                    #region SE ENVIA EL CORREO INDIVIDUALMENTE
                    foreach (var item in lstCorreos)
                    {
                        string subject = @"Evaluación 360";
                        string asunto = string.Empty;
                        List<string> objCorreo = new List<string>();
                        objCorreo.Add(item);

                        asunto = "Buen día, cuenta con una evaluación pendiente en SIGOPLAN.<br><br>Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>Construplan > Capital Humano > Desarrollo Organizacional > Evaluación 360." +
                                    "<br><br>Se informa que este es un correo autogenerado por el sistema SIGOPLAN (<a href='http://sigoplan.construplan.com.mx/'>http://sigoplan.construplan.com.mx</a>). No es necesario dar una respuesta. Gracias.";

#if DEBUG
                        objCorreo = new List<string>();
                        objCorreo.Add("omar.nunez@construplan.com.mx");
#endif
                        GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), asunto, objCorreo);
                    }
                    #endregion

                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha enviado el correo con éxito.");
                    dbContextTransaction.Commit();
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, _NOMBRE_CONTROLADOR, "EnviarCorreo", e, AccionEnum.CORREO, 0, new { lstPersonalEvaluadorID = lstPersonalEvaluadorID });
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
                return resultado;
            }
        }
        #endregion

        #region METODOS GENERALES
        public Dictionary<string, object> FillCboCC()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region FILL CBO
                List<tblC_Nom_CatalogoCC> lstCC = _context.tblC_Nom_CatalogoCC.Where(w => w.estatus && w.cc != "0").OrderBy(o => o.cc).ToList();

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objDTO = new ComboDTO();
                foreach (var item in lstCC)
                {
                    if (!string.IsNullOrEmpty(item.cc) && !string.IsNullOrEmpty(item.ccDescripcion))
                    {
                        string cc = item.cc.Trim();
                        string descripcion = SetPrimeraLetraMayuscula(item.ccDescripcion.Trim());
                        objDTO = new ComboDTO();
                        objDTO.Value = item.cc.ToString();
                        objDTO.Text = string.Format("[{0}] {1}", cc, descripcion);
                        lstComboDTO.Add(objDTO);
                    }
                }

                resultado.Add(ITEMS, lstComboDTO);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "FillCbo", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboDepartamentos()
        {
            return null;
        }

        public Dictionary<string, object> FillCboTipoUsuarios()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region FILL CBO
                List<ComboDTO> lstComboDTO = new List<ComboDTO>();

                // EVALUADOR
                ComboDTO obj = new ComboDTO();
                int tipoUsuario = (int)TipoUsuarioEnum.EVALUADOR;
                obj.Value = tipoUsuario.ToString();
                obj.Text = EnumHelper.GetDescription((TipoUsuarioEnum.EVALUADOR));
                lstComboDTO.Add(obj);

                // EVALUADOR
                obj = new ComboDTO();
                tipoUsuario = (int)TipoUsuarioEnum.EVALUADO;
                obj.Value = tipoUsuario.ToString();
                obj.Text = EnumHelper.GetDescription((TipoUsuarioEnum.EVALUADO));
                lstComboDTO.Add(obj);

                resultado.Add(ITEMS, lstComboDTO);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "FillCboTipoUsuarios", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboGrupos()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region FILL CBO GRUPOS
                List<ComboDTO> lstGruposDTO = _context.Select<ComboDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT id AS VALUE, nombreGrupo AS TEXT FROM tblRH_Eval360_CatGrupos WHERE registroActivo = @registroActivo",
                    parametros = new { registroActivo = true }
                }).ToList();

                foreach (var item in lstGruposDTO)
                {
                    item.Text = item.Text;
                }

                resultado.Add(ITEMS, lstGruposDTO);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "FillCboGrupos", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboCompetencias(int idGrupo)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region FILL CBO GRUPOS
                List<ComboDTO> lstCompetenciasDTO = _context.Select<ComboDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT id AS VALUE, nombreCompetencia AS TEXT, definicion AS Prefijo FROM tblRH_Eval360_CatCompetencias WHERE idGrupo = @idGrupo AND registroActivo = @registroActivo",
                    parametros = new { idGrupo = idGrupo, registroActivo = true }
                }).ToList();

                foreach (var item in lstCompetenciasDTO)
                {
                    item.Text = item.Text;
                    item.Prefijo = item.Prefijo;
                }

                resultado.Add(ITEMS, lstCompetenciasDTO);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "FillCboCompetencias", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboUsuarios()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region FILL CBO USUARIOS

                #region USUARIOS DE SIGOPLAN
                List<CatPersonalDTO> lstUsuariosCP = _context.Select<CatPersonalDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT t1.id, t1.nombre, t1.apellidoPaterno, t1.apellidoMaterno 
	                                    FROM tblP_Usuario AS t1
	                                    INNER JOIN tblRH_EK_Empleados AS t2 ON CONVERT(NVARCHAR(100), t2.clave_empleado) = t1.cveEmpleado
		                                    WHERE t1.estatus = @estatus AND t2.esActivo = @esActivo AND t2.estatus_empleado = @estatus_empleado
			                                    ORDER BY nombre",
                    parametros = new { estatus = true, esActivo = true, estatus_empleado = 'A' }
                });
                #endregion

                #region USUARIOS DE GCPLAN
                List<CatPersonalDTO> lstUsuariosGCPLAN = _context.Select<CatPersonalDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.GCPLAN,
                    consulta = @"SELECT t1.id, t1.nombre, t1.apellidoPaterno, t1.apellidoMaterno 
	                                    FROM tblP_Usuario AS t1
	                                    INNER JOIN tblRH_EK_Empleados AS t2 ON CONVERT(NVARCHAR(100), t2.clave_empleado) = t1.cveEmpleado
		                                    WHERE t1.estatus = @estatus AND t2.esActivo = @esActivo AND t2.estatus_empleado = @estatus_empleado
			                                    ORDER BY nombre",
                    parametros = new { estatus = true, esActivo = true, estatus_empleado = 'A' }
                });
                #endregion

                #region USUARIOS DE ARRENDADORA
                List<CatPersonalDTO> lstUsuariosARR = _context.Select<CatPersonalDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Arrendadora,
                    consulta = @"SELECT t1.id, t1.nombre, t1.apellidoPaterno, t1.apellidoMaterno 
	                                    FROM tblP_Usuario AS t1
	                                    INNER JOIN tblRH_EK_Empleados AS t2 ON CONVERT(NVARCHAR(100), t2.clave_empleado) = t1.cveEmpleado
		                                    WHERE t1.estatus = @estatus AND t2.esActivo = @esActivo AND t2.estatus_empleado = @estatus_empleado
			                                    ORDER BY nombre",
                    parametros = new { estatus = true, esActivo = true, estatus_empleado = 'A' }
                });
                #endregion

                #region USUARIOS DE COLOMBIA
                List<CatPersonalDTO> lstUsuariosCOLOMBIA = _context.Select<CatPersonalDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Colombia,
                    consulta = @"SELECT t1.id, t1.nombre, t1.apellidoPaterno, t1.apellidoMaterno 
	                                    FROM tblP_Usuario AS t1
	                                    INNER JOIN tblRH_EK_Empleados AS t2 ON CONVERT(NVARCHAR(100), t2.clave_empleado) = t1.cveEmpleado
		                                    WHERE t1.estatus = @estatus AND t2.esActivo = @esActivo AND t2.estatus_empleado = @estatus_empleado
			                                    ORDER BY nombre",
                    parametros = new { estatus = true, esActivo = true, estatus_empleado = 'A' }
                });
                #endregion

                #region USUARIOS DE PERU
                List<CatPersonalDTO> lstUsuariosPERU = _context.Select<CatPersonalDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.PERU,
                    consulta = @"SELECT t1.id, t1.nombre, t1.apellidoPaterno, t1.apellidoMaterno 
	                                    FROM tblP_Usuario AS t1
	                                    INNER JOIN tblRH_EK_Empleados AS t2 ON CONVERT(NVARCHAR(100), t2.clave_empleado) = t1.cveEmpleado
		                                    WHERE t1.estatus = @estatus AND t2.esActivo = @esActivo AND t2.estatus_empleado = @estatus_empleado
			                                    ORDER BY nombre",
                    parametros = new { estatus = true, esActivo = true, estatus_empleado = 'A' }
                });
                #endregion

                List<CatPersonalDTO> lstUsuarios = new List<CatPersonalDTO>();
                CatPersonalDTO objUsuario = new CatPersonalDTO();
                foreach (var item in lstUsuariosCP)
                {
                    #region CONSTRUPLAN
                    objUsuario = new CatPersonalDTO();
                    objUsuario.id = item.id;
                    objUsuario.nombre = !string.IsNullOrEmpty(item.nombre) ? item.nombre.Trim() : string.Empty;
                    objUsuario.apellidoPaterno = !string.IsNullOrEmpty(item.apellidoPaterno) ? item.apellidoPaterno.Trim() : string.Empty;
                    objUsuario.apellidoMaterno = !string.IsNullOrEmpty(item.apellidoMaterno) ? item.apellidoMaterno.Trim() : string.Empty;
                    objUsuario.idEmpresa = (int)EmpresaEnum.Construplan;
                    lstUsuarios.Add(objUsuario);
                    #endregion
                }

                foreach (var item in lstUsuariosGCPLAN)
                {
                    #region GCPLAN
                    objUsuario = new CatPersonalDTO();
                    objUsuario.id = item.id;
                    objUsuario.nombre = !string.IsNullOrEmpty(item.nombre) ? item.nombre.Trim() : string.Empty;
                    objUsuario.apellidoPaterno = !string.IsNullOrEmpty(item.apellidoPaterno) ? item.apellidoPaterno.Trim() : string.Empty;
                    objUsuario.apellidoMaterno = !string.IsNullOrEmpty(item.apellidoMaterno) ? item.apellidoMaterno.Trim() : string.Empty;
                    objUsuario.idEmpresa = (int)EmpresaEnum.GCPLAN;
                    lstUsuarios.Add(objUsuario);
                    #endregion
                }

                foreach (var item in lstUsuariosARR)
                {
                    #region ARRENDADORA
                    string nombre = !string.IsNullOrEmpty(item.nombre) ? item.nombre.Trim() : string.Empty;
                    string paterno = !string.IsNullOrEmpty(item.apellidoPaterno) ? item.apellidoPaterno.Trim() : string.Empty;
                    string materno = !string.IsNullOrEmpty(item.apellidoMaterno) ? item.apellidoMaterno.Trim() : string.Empty;
                    CatPersonalDTO objVerificarUsuario = lstUsuarios.Where(w => w.nombre == nombre && w.apellidoPaterno == paterno && w.apellidoMaterno == materno).FirstOrDefault();
                    if (objVerificarUsuario == null)
                    {
                        objUsuario = new CatPersonalDTO();
                        objUsuario.id = item.id;
                        objUsuario.nombre = nombre;
                        objUsuario.apellidoPaterno = paterno;
                        objUsuario.apellidoMaterno = materno;
                        objUsuario.idEmpresa = (int)EmpresaEnum.Arrendadora;
                        lstUsuarios.Add(objUsuario);
                    }
                    #endregion
                }

                foreach (var item in lstUsuariosCOLOMBIA)
                {
                    #region COLOMBIA
                    string nombre = !string.IsNullOrEmpty(item.nombre) ? item.nombre.Trim() : string.Empty;
                    string paterno = !string.IsNullOrEmpty(item.apellidoPaterno) ? item.apellidoPaterno.Trim() : string.Empty;
                    string materno = !string.IsNullOrEmpty(item.apellidoMaterno) ? item.apellidoMaterno.Trim() : string.Empty;
                    CatPersonalDTO objVerificarUsuario = lstUsuarios.Where(w => w.nombre == nombre && w.apellidoPaterno == paterno && w.apellidoMaterno == materno).FirstOrDefault();
                    if (objVerificarUsuario == null)
                    {
                        objUsuario = new CatPersonalDTO();
                        objUsuario.id = item.id;
                        objUsuario.nombre = nombre;
                        objUsuario.apellidoPaterno = paterno;
                        objUsuario.apellidoMaterno = materno;
                        objUsuario.idEmpresa = (int)EmpresaEnum.Colombia;
                        lstUsuarios.Add(objUsuario);
                    }
                    #endregion
                }

                foreach (var item in lstUsuariosPERU)
                {
                    #region PERU
                    string nombre = !string.IsNullOrEmpty(item.nombre) ? item.nombre.Trim() : string.Empty;
                    string paterno = !string.IsNullOrEmpty(item.apellidoPaterno) ? item.apellidoPaterno.Trim() : string.Empty;
                    string materno = !string.IsNullOrEmpty(item.apellidoMaterno) ? item.apellidoMaterno.Trim() : string.Empty;
                    CatPersonalDTO objVerificarUsuario = lstUsuarios.Where(w => w.nombre == nombre && w.apellidoPaterno == paterno && w.apellidoMaterno == materno).FirstOrDefault();
                    if (objVerificarUsuario == null)
                    {
                        objUsuario = new CatPersonalDTO();
                        objUsuario.id = item.id;
                        objUsuario.nombre = nombre;
                        objUsuario.apellidoPaterno = paterno;
                        objUsuario.apellidoMaterno = materno;
                        objUsuario.idEmpresa = (int)EmpresaEnum.Peru;
                        lstUsuarios.Add(objUsuario);
                    }
                    #endregion
                }

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objDTO = new ComboDTO();
                foreach (var item in lstUsuarios)
                {
                    string nombreCompleto = string.Empty;
                    if (!string.IsNullOrEmpty(item.nombre))
                        nombreCompleto = item.nombre.Trim();

                    if (!string.IsNullOrEmpty(item.apellidoPaterno))
                        nombreCompleto += string.Format(" {0}", item.apellidoPaterno.Trim().ToUpper());

                    if (!string.IsNullOrEmpty(item.apellidoMaterno))
                        nombreCompleto += string.Format(" {0}", item.apellidoMaterno.Trim().ToUpper());

                    if (!string.IsNullOrEmpty(nombreCompleto))
                    {
                        objDTO = new ComboDTO();
                        objDTO.Value = item.id.ToString();
                        objDTO.Text = SetPrimeraLetraMayuscula(nombreCompleto);
                        objDTO.Prefijo = item.idEmpresa.ToString();
                        lstComboDTO.Add(objDTO);
                    }
                }

                lstComboDTO = lstComboDTO.OrderBy(o => o.Text).ToList();

                resultado.Add(ITEMS, lstComboDTO);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "FillCboUsuarios", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> GetInformacionUsuario(int idUsuario, int idEmpresa)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                string mensajeError = "Ocurrió un error al obtener la información del colaborador.";
                if (idUsuario <= 0) { throw new Exception(mensajeError); }
                if (idEmpresa <= 0) { throw new Exception(mensajeError); }
                #endregion

                #region SE OBTIENE LA INFORMACIÓN DEL COLABORADOR
                MainContextEnum objEmpresa = GetEmpresaDapper(idEmpresa);

                CatPersonalDTO objInfoColaborador = objInfoColaborador = _context.Select<CatPersonalDTO>(new DapperDTO
                {
                    baseDatos = objEmpresa,
                    consulta = @"SELECT '[' + t2.cc_contable + '] ' + t3.ccDescripcion AS cc, t1.correo, t4.descripcion AS descripcionPuesto
		                                FROM tblP_Usuario AS t1
		                                INNER JOIN tblRH_EK_Empleados AS t2 ON CONVERT(NVARCHAR(100), t2.clave_empleado) = t1.cveEmpleado
		                                INNER JOIN tblC_Nom_CatalogoCC AS t3 ON t3.cc = t2.cc_contable
		                                INNER JOIN tblRH_EK_Puestos AS t4 ON t4.puesto = t2.puesto
		                                    WHERE t1.estatus = @estatus AND t2.estatus_empleado = @estatus_empleado AND t2.esActivo = @esActivo AND t1.id = @idUsuario",
                    parametros = new { estatus = true, estatus_empleado = 'A', esActivo = true, idUsuario = idUsuario }
                }).FirstOrDefault();
                if (objInfoColaborador == null)
                    throw new Exception("Ocurrió un error al obtener la información del colaborador.");

                objInfoColaborador.cc = SetPrimeraLetraMayuscula(objInfoColaborador.cc);
                objInfoColaborador.descripcionPuesto = SetPrimeraLetraMayuscula(objInfoColaborador.descripcionPuesto);

                resultado.Add(SUCCESS, true);
                resultado.Add("objInfoColaborador", objInfoColaborador);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetInformacionUsuario", e, AccionEnum.CONSULTA, 0, new { idUsuario = idUsuario });
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboPuestos()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region FILL CBO
                List<tblRH_EK_Puestos> lstPuestos = _context.tblRH_EK_Puestos.Where(w => w.registroActivo).OrderBy(o => o.descripcion).ToList();
                if (lstPuestos.Count() <= 0)
                    throw new Exception("Ocurrió un error al obtener el listado de puestos.");

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objDTO = new ComboDTO();
                foreach (var item in lstPuestos)
                {
                    if (!string.IsNullOrEmpty(item.descripcion))
                    {
                        string descripcion = SetPrimeraLetraMayuscula(item.descripcion);

                        objDTO = new ComboDTO();
                        objDTO.Value = item.puesto.ToString();
                        objDTO.Text = descripcion.Trim();
                        lstComboDTO.Add(objDTO);
                    }
                }

                resultado.Add(ITEMS, lstComboDTO);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "FillCboPuestos", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboPeriodos()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region FILL CBO PERIODOS
                List<ComboDTO> lstCboPeriodos = _context.Select<ComboDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT id AS VALUE, nombrePeriodo AS TEXT, estado PREFIJO FROM tblRH_Eval360_CatPeriodos WHERE registroActivo = @registroActivo ORDER BY TEXT DESC",
                    parametros = new { registroActivo = true }
                }).ToList();

                foreach (var item in lstCboPeriodos)
                {
                    if (!string.IsNullOrEmpty(item.Text) && !string.IsNullOrEmpty(item.Prefijo))
                        item.Text += string.Format(" - {0}", item.Prefijo == "A" ? "Abierto" : item.Prefijo == "C" ? "Cerrado" : string.Empty);

                    item.Text = item.Text;
                }

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstCboPeriodos);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "FillCboPeriodos", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboTipoRelacionEvaluado()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region FILL CBO
                List<ComboDTO> lstComboDTO = new List<ComboDTO>();

                // AUTOEVALUACION
                ComboDTO obj = new ComboDTO();
                int tipoRelacion = (int)TipoRelacionEnum.AUTOEVALUACION;
                obj.Value = tipoRelacion.ToString();
                obj.Text = EnumHelper.GetDescription((TipoRelacionEnum.AUTOEVALUACION));
                lstComboDTO.Add(obj);

                // CLIENTES INTERNOS
                obj = new ComboDTO();
                tipoRelacion = (int)TipoRelacionEnum.CLIENTES_INTERNOS;
                obj.Value = tipoRelacion.ToString();
                obj.Text = EnumHelper.GetDescription((TipoRelacionEnum.CLIENTES_INTERNOS));
                lstComboDTO.Add(obj);

                // COLABORADORES
                obj = new ComboDTO();
                tipoRelacion = (int)TipoRelacionEnum.COLABORADORES;
                obj.Value = tipoRelacion.ToString();
                obj.Text = EnumHelper.GetDescription((TipoRelacionEnum.COLABORADORES));
                lstComboDTO.Add(obj);

                // JEFE
                obj = new ComboDTO();
                tipoRelacion = (int)TipoRelacionEnum.JEFE;
                obj.Value = tipoRelacion.ToString();
                obj.Text = EnumHelper.GetDescription((TipoRelacionEnum.JEFE));
                lstComboDTO.Add(obj);

                // PARES
                obj = new ComboDTO();
                tipoRelacion = (int)TipoRelacionEnum.PARES;
                obj.Value = tipoRelacion.ToString();
                obj.Text = EnumHelper.GetDescription((TipoRelacionEnum.PARES));
                lstComboDTO.Add(obj);

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstComboDTO);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "FillCboTipoRelacionEvaluado", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        private string GetNombreCompletoUsuarioSIGOPLAN(int idUsuario, int idEmpresa)
        {
            string nombreCompleto = string.Empty;
            try
            {
                #region SE OBTIENE NOMBRE COMPLETO DEL USUARIO DE SIGOPLAN
                MainContextEnum objEmpresa = GetEmpresaDapper(idEmpresa);
                tblP_Usuario objUsuario = _context.Select<tblP_Usuario>(new DapperDTO
                {
                    baseDatos = objEmpresa,
                    consulta = @"SELECT nombre, apellidoPaterno, apellidoMaterno FROM tblP_Usuario WHERE id = @idUsuario AND estatus = @estatus",
                    parametros = new { idUsuario = idUsuario, estatus = 1 }
                }).FirstOrDefault();
                if (objUsuario == null)
                    throw new Exception("Ocurrió un error al obtener el nombre del usuario.");

                if (!string.IsNullOrEmpty(objUsuario.nombre))
                    nombreCompleto = objUsuario.nombre.Trim();

                if (!string.IsNullOrEmpty(objUsuario.apellidoPaterno))
                    nombreCompleto += string.Format(" {0}", objUsuario.apellidoPaterno.Trim().ToUpper());

                if (!string.IsNullOrEmpty(objUsuario.apellidoMaterno))
                    nombreCompleto += string.Format(" {0}", objUsuario.apellidoMaterno.Trim().ToUpper());

                nombreCompleto = SetPrimeraLetraMayuscula(nombreCompleto);
                return nombreCompleto;
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetNombreCompletoUsuarioSIGOPLAN", e, AccionEnum.CONSULTA, idUsuario, new { idUsuario = idUsuario });
                return string.Empty;
            }
        }

        private string GetCorreoUsuario(int idUsuario, int idEmpresa)
        {
            string correo = string.Empty;
            try
            {
                #region SE OBTIENE CORREO DEL USUARIO
                MainContextEnum objEmpresa = GetEmpresaDapper(idEmpresa);
                tblP_Usuario objUsuario = _context.Select<tblP_Usuario>(new DapperDTO
                {
                    baseDatos = objEmpresa,
                    consulta = @"SELECT correo FROM tblP_Usuario WHERE id = @idUsuario AND estatus = @estatus",
                    parametros = new { idUsuario = idUsuario, estatus = 1 }
                }).FirstOrDefault();
                if (objUsuario == null)
                    throw new Exception("Ocurrió un error al obtener el correo del usuario.");

                correo = objUsuario.correo;
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetCorreoUsuario", e, AccionEnum.CONSULTA, idUsuario, new { idUsuario = idUsuario, idEmpresa = idEmpresa });
                return string.Empty;
            }
            return correo;
        }

        private string GetCCUsuario(int idUsuario, int idEmpresa)
        {
            string ccDescripcion = string.Empty;
            try
            {
                #region SE OBTIENE CC Y DESCRIPCIÓN
                MainContextEnum objEmpresa = GetEmpresaDapper(idEmpresa);

                CatPersonalDTO objUsuario = objUsuario = _context.Select<CatPersonalDTO>(new DapperDTO
                {
                    baseDatos = objEmpresa,
                    consulta = @"SELECT '[' + t1.cc + '] ' + t1.ccDescripcion AS descripcionCC
	                                    FROM tblC_Nom_CatalogoCC AS t1
	                                    INNER JOIN tblRH_EK_Empleados AS t2 ON t2.cc_contable = t1.cc
	                                    INNER JOIN tblP_Usuario AS t3 ON t3.cveEmpleado = CONVERT(NVARCHAR(100), t2.clave_empleado)
		                                    WHERE t3.id = @idUsuario AND t2.estatus_empleado = @estatus_empleado AND t2.esActivo = @esActivo AND t3.estatus = @estatus",
                    parametros = new { idUsuario = idUsuario, estatus_empleado = 'A', esActivo = true, estatus = true }
                }).FirstOrDefault();
                if (objUsuario == null)
                    throw new Exception("Ocurrió un error al obtener el cc del usuario.");

                ccDescripcion = objUsuario.descripcionCC;
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetCorreoUsuario", e, AccionEnum.CONSULTA, idUsuario, new { idUsuario = idUsuario, idEmpresa = idEmpresa });
                return string.Empty;
            }
            return ccDescripcion;
        }

        private int GetPersonalID()
        {
            int personalID = 0;
            try
            {
                #region SE OBTIENE EL ID DEL CATALOGO DE PERSONAL RELACIONADO AL USUARIO LOGUEADO
                tblRH_Eval360_CatPersonal objPersonal = _context.tblRH_Eval360_CatPersonal.Where(w => w.idUsuario == (int)vSesiones.sesionUsuarioDTO.id && w.registroActivo).FirstOrDefault();
                if (objPersonal != null)
                    personalID = objPersonal.id;
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetPersonalID", e, AccionEnum.CONSULTA, 0, 0);
                return 0;
            }
            return personalID;
        }

        public Dictionary<string, object> GetNivelAcceso()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE VERIFICA EL NIVEL DE ACCESO DEL USUARIO LOGUEADO
                tblRH_Eval360_CatPersonal objPersonal = _context.tblRH_Eval360_CatPersonal.Where(w => w.idUsuario == (int)vSesiones.sesionUsuarioDTO.id && w.registroActivo).FirstOrDefault();
                if (objPersonal == null)
                    throw new Exception("Ocurrió un error al obtener la información del usuario.");

                resultado.Add(SUCCESS, true);
                resultado.Add("nivelAcceso", objPersonal.nivelAcceso);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetNivelAcceso", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        private string GetNombreGrupo(int idGrupo)
        {
            string nombreGrupo = string.Empty;
            try
            {
                #region VALIDACIONES
                if (idGrupo <= 0) { return nombreGrupo; }
                #endregion

                #region SE OBTIENE EL NOMBRE DEL GRUPO
                tblRH_Eval360_CatGrupos objGrupo = _context.tblRH_Eval360_CatGrupos.Where(w => w.id == idGrupo).FirstOrDefault();
                if (objGrupo != null)
                    nombreGrupo = objGrupo.nombreGrupo.Trim();
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetNombreGrupo", e, AccionEnum.CONSULTA, idGrupo, new { idGrupo = idGrupo });
                return nombreGrupo;
            }
            return nombreGrupo;
        }

        private int GetTipoRelacion(int idPeriodo, int idPersonalEvaluado, int idPersonalEvaluador)
        {
            int tipoRelacion = 0;
            try
            {
                #region VALIDACIONES
                if (idPeriodo <= 0 || idPersonalEvaluado <= 0 || idPersonalEvaluador <= 0) { return tipoRelacion; }
                #endregion

                #region SE OBTIENE EL TIPO DE RELACIÓN DEL EVALUADOR SOBRE EL EVALUADO
                RelacionDetDTO objTipoRelacion = _context.Select<RelacionDetDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT t2.tipoRelacion 
	                                    FROM tblRH_Eval360_Relaciones AS t1
	                                    INNER JOIN tblRH_Eval360_RelacionesDet AS t2 ON t2.idRelacion = t1.id
		                                    WHERE t1.idPeriodo = @idPeriodo AND t1.idPersonalEvaluado = @idPersonalEvaluado AND 
                                                    t2.idPersonalEvaluador = @idPersonalEvaluador AND t1.registroActivo = @registroActivo AND 
                                                    t2.registroActivo = @registroActivo",
                    parametros = new { idPeriodo = idPeriodo, idPersonalEvaluado = idPersonalEvaluado, idPersonalEvaluador = idPersonalEvaluador, registroActivo = true }
                }).FirstOrDefault();
                if (objTipoRelacion != null)
                    tipoRelacion = (int)objTipoRelacion.tipoRelacion;
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetTipoRelacion", e, AccionEnum.CONSULTA, 0, new { idPeriodo = idPeriodo, idPersonalEvaluado = idPersonalEvaluado, idPersonalEvaluador = idPersonalEvaluador });
                return tipoRelacion;
            }
            return tipoRelacion;
        }

        private string GetNombreCompetencia(int idCompetencia)
        {
            string nombreCompetencia = string.Empty;
            try
            {
                #region VALIDACIONES
                if (idCompetencia <= 0) { return nombreCompetencia; }
                #endregion

                #region SE OBTIENE EL NOMBRE DE LA COMPETENCIA
                tblRH_Eval360_CatCompetencias objCompetencia = _context.tblRH_Eval360_CatCompetencias.Where(w => w.id == idCompetencia).FirstOrDefault();
                if (objCompetencia != null)
                    nombreCompetencia = objCompetencia.nombreCompetencia.Trim();
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                return nombreCompetencia;
            }
            return nombreCompetencia;
        }

        private string GetTipoRelacionDescripcion(int tipoRelacion)
        {
            string tipoRelacionDescripcion = string.Empty;
            try
            {
                #region VALIDACIONES
                if (tipoRelacion <= 0) { throw new Exception("Ocurrió un error al obtener el tipo de relación del personal."); }
                #endregion

                #region SE OBTIENE LA DESCRIPCIÓN DE LA RELACIÓN
                switch (tipoRelacion)
                {
                    case (int)TipoRelacionEnum.AUTOEVALUACION:
                        tipoRelacionDescripcion = EnumHelper.GetDescription((TipoRelacionEnum.AUTOEVALUACION));
                        break;
                    case (int)TipoRelacionEnum.CLIENTES_INTERNOS:
                        tipoRelacionDescripcion = EnumHelper.GetDescription((TipoRelacionEnum.CLIENTES_INTERNOS));
                        break;
                    case (int)TipoRelacionEnum.COLABORADORES:
                        tipoRelacionDescripcion = EnumHelper.GetDescription((TipoRelacionEnum.COLABORADORES));
                        break;
                    case (int)TipoRelacionEnum.JEFE:
                        tipoRelacionDescripcion = EnumHelper.GetDescription((TipoRelacionEnum.JEFE));
                        break;
                    case (int)TipoRelacionEnum.PARES:
                        tipoRelacionDescripcion = EnumHelper.GetDescription((TipoRelacionEnum.PARES));
                        break;
                }
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetTipoRelacionDescripcion", e, AccionEnum.CONSULTA, 0, new { tipoRelacion = tipoRelacion });
                return string.Empty;
            }
            return tipoRelacionDescripcion;
        }

        private int GetUsuarioID(int idPersonal)
        {
            int idUsuario = 0;
            try
            {
                #region VALIDACIONES
                string mensajeError = "Ocurrió un error al obtener la información del personal.";
                if (idPersonal <= 0) { throw new Exception(mensajeError); }
                #endregion

                #region SE OBTIENE EL ID DEL USUARIO
                tblRH_Eval360_CatPersonal objPersonal = _context.tblRH_Eval360_CatPersonal.Where(w => w.id == idPersonal && w.registroActivo).FirstOrDefault();
                if (objPersonal == null)
                    throw new Exception(mensajeError);

                idUsuario = objPersonal.idUsuario;
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetUsuarioID", e, AccionEnum.CONSULTA, 0, new { idPersonal = idPersonal });
                return idUsuario;
            }
            return idUsuario;
        }

        private string GetEstatusCuestionarioEvaluadorRelEvaluado(int idPeriodo, int idPersonalEvaluado, int idPersonalEvaluador)
        {
            string estatusCuestionario = string.Empty;
            try
            {
                #region VALIDACIONES
                string mensajeError = "Ocurrió un error al obtener el avance del cuestionario.";
                if (idPeriodo <= 0) { throw new Exception(mensajeError); }
                if (idPersonalEvaluado <= 0) { throw new Exception(mensajeError); }
                if (idPersonalEvaluador <= 0) { throw new Exception(mensajeError); }
                #endregion

                #region SE OBTIENE EL AVANCE DEL CUESTIONARIO DEL EVALUADOR SOBRE EL EVALUADO
                // SE CONSULTA EL ID DE LA EVALUACION
                tblRH_Eval360_EvaluacionesEvaluador objEvaluacion = _context.tblRH_Eval360_EvaluacionesEvaluador.Where(w => w.idPeriodo == idPeriodo && w.idPersonalEvaluado == idPersonalEvaluado &&
                                                                                                                            w.idPersonalEvaluador == idPersonalEvaluador && w.registroActivo).FirstOrDefault();
                if (objEvaluacion == null)
                    throw new Exception(mensajeError);

                // SE OBTIENE LA CANTIDAD DE CONDUCTAS RESPONDIDAS
                List<tblRH_Eval360_EvaluacionesEvaluadorDet> lstConductasRespondidas = _context.tblRH_Eval360_EvaluacionesEvaluadorDet.Where(w => w.idEvaluacion == objEvaluacion.id && w.registroActivo).ToList();

                // SE OBTIENE LA CANTIDAD DE CONDUCTAS QUE CONTIENE EL CUESTIONARIO
                List<tblRH_Eval360_CuestionariosDet> lstConductasCuestionario = _context.tblRH_Eval360_CuestionariosDet.Where(w => w.idCuestionario == objEvaluacion.idCuestionario && w.registroActivo).ToList();

                int cantConductasRespondidas = lstConductasRespondidas.Count();
                int cantConductasCuestionario = lstConductasCuestionario.Count();

                // SE INDICA EN TEXTO EL ESTATUS DEL CUESTIONARIO
                if (cantConductasRespondidas == cantConductasCuestionario && cantConductasRespondidas > 0)
                    estatusCuestionario = EnumHelper.GetDescription((EstatusCuestionarioEvaluadorEnum.CONTESTADA));
                else if (cantConductasRespondidas <= 0)
                    estatusCuestionario = EnumHelper.GetDescription((EstatusCuestionarioEvaluadorEnum.NO_INICIADA));
                else if (cantConductasRespondidas < cantConductasCuestionario && cantConductasRespondidas > 0)
                    estatusCuestionario = EnumHelper.GetDescription((EstatusCuestionarioEvaluadorEnum.EN_PROCESO));
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetEstatusCuestionarioEvaluadorRelEvaluado", e, AccionEnum.CONSULTA, 0,
                    new { idPeriodo = idPeriodo, idPersonalEvaluado = idPersonalEvaluado, idPersonalEvaluador = idPersonalEvaluador });
                return estatusCuestionario;
            }
            return estatusCuestionario;
        }

        private string SetPrimeraLetraMayuscula(string palabra)
        {
            string resultadoPalabra = string.Empty;
            try
            {
                #region VALIDACIONES
                if (string.IsNullOrEmpty(palabra)) throw new Exception("Ocurrió un error al convertir en minusculas el nombre.");
                #endregion

                #region CONVIERTE LA PALABRA EN MINUSCULAS Y CONVIERTE LA PRIMERA LETRA POR PALABRA EN MAYUSCULAS
                string[] nombreSplit = palabra.Split(' ');
                foreach (var item in nombreSplit)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        string palabraEnMinusculas = item.ToLower();
                        if (string.IsNullOrEmpty(resultadoPalabra))
                            resultadoPalabra = char.ToUpper(palabraEnMinusculas[0]) + palabraEnMinusculas.Substring(1);
                        else
                            resultadoPalabra += string.Format(" {0}", char.ToUpper(palabraEnMinusculas[0]) + palabraEnMinusculas.Substring(1));
                    }
                }
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "SetPrimeraLetraMayuscula", e, AccionEnum.METODO_GENERAL, 0, new { palabra = palabra });
                return resultadoPalabra;
            }
            return resultadoPalabra;
        }

        private string SetNombreCompleto(string nombre, string apellidoPaterno, string apellidoMaterno)
        {
            string nombreCompleto = string.Empty;
            try
            {
                #region SE CONSTRUYE NOMBRE COMPLETO
                if (!string.IsNullOrEmpty(nombre))
                    nombreCompleto = nombre.Trim();
                if (!string.IsNullOrEmpty(apellidoPaterno))
                    nombreCompleto += string.Format(" {0}", apellidoPaterno.Trim());
                if (!string.IsNullOrEmpty(apellidoMaterno))
                    nombreCompleto += string.Format(" {0}", apellidoMaterno.Trim());
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "SetNombreCompleto", e, AccionEnum.CONSULTA, 0, new { nombre = nombre, apellidoPaterno = apellidoPaterno, apellidoMaterno = apellidoMaterno });
                return nombreCompleto;
            }
            return nombreCompleto;
        }

        private MainContextEnum GetEmpresaDapper(int idEmpresa)
        {
            MainContextEnum objEmpresa = new MainContextEnum();
            try
            {
                switch (idEmpresa)
                {
                    case (int)EmpresaEnum.Construplan:
                        objEmpresa = MainContextEnum.Construplan;
                        break;
                    case (int)EmpresaEnum.GCPLAN:
                        objEmpresa = MainContextEnum.GCPLAN;
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
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return objEmpresa;
        }

        public int GetEmpresaPersonal(int idPersonal)
        {
            int idEmpresa = 0;
            try
            {
                #region SE OBTIENE LA EMPRESA DEL PERSONAL
                tblRH_Eval360_CatPersonal objPersonal = _context.tblRH_Eval360_CatPersonal.Where(w => w.id == idPersonal && w.registroActivo).FirstOrDefault();
                if (objPersonal == null)
                    throw new Exception("Ocurrió un error al obtener la empresa del usuario.");

                idEmpresa = objPersonal.idEmpresa;
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, _NOMBRE_CONTROLADOR, "GetEmpresaPersonal", e, AccionEnum.CONSULTA, idPersonal, new { idPersonal = idPersonal });
                return idEmpresa;
            }
            return idEmpresa;
        }

        private int GetUltimoOrdenConducta()
        {
            int orden = 0;
            try
            {
                #region SE OBTIENE EL ULTIMO ORDEN Y SE INCREMENTA EN UN
                tblRH_Eval360_CatConductas objConducta = _context.tblRH_Eval360_CatConductas.Where(w => w.registroActivo).OrderByDescending(o => o.orden).FirstOrDefault();
                if (objConducta == null)
                    orden = 1;
                else
                    orden = objConducta.orden + 1;
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                return orden;
            }
            return orden;
        }

        private MainContextEnum GetEmpresaLogueada(int idEmpresa)
        {
            MainContextEnum objEmpresa = new MainContextEnum();
            try
            {
                #region SE OBTIENE MainContextEnum DE LA EMPRESA LOGUEADA
                switch (idEmpresa)
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
        #endregion
    }
}